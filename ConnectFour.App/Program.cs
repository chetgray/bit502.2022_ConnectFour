using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using ConnectFour.Business.BLLs;
using ConnectFour.Business.BLLs.Interfaces;
using ConnectFour.Business.Models;
using ConnectFour.Business.Models.Interfaces;

using static ConnectFour.App.ProgramHelpers;

namespace ConnectFour.App
{
    internal static class Program
    {
        private static string _localPlayerName = string.Empty;

        private static void Main()
        {
            bool isChoosing = true;
            while (isChoosing)
            {
                Console.Clear();
                WriteTitle();
                Console.WriteLine(
                    "What would you like to do?\n"
                        + "[1] - New Single-Player Game\n"
                        + "[2] - New Multi-Player Game\n"
                        + "[3] - Join Multi-Player Game\n"
                        + "[4] - View Game Results\n"
                        + "[5] - Quit\n"
                );
                Console.Write("--> ");
                string userResponse = Console.ReadLine();
                switch (userResponse)
                {
                    case "1":
                        HandleNpcGame();
                        break;
                    case "2":
                        HandleHostingGame();
                        break;
                    case "3":
                        HandleJoiningGame();
                        break;
                    case "4":
                        HandleDisplayingResults();
                        break;
                    case "5":
                        isChoosing = false;
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine(
                            "Invalid response please choose 1, 2, 3, 4, or 5\nPress any key to go back to the main menu"
                        );
                        Console.ReadKey();
                        break;
                }
            }
        }

        private static void GameEnd(IRoomModel roomModel)
        {
            if (roomModel.ResultCode == 0)
            {
                WriteInColor("\n     DRAW!!!", ConsoleColor.Blue);
            }
            else
            {
                WriteInColor(
                    $"\n     {roomModel.Players[(int)roomModel.ResultCode - 1].Name} Wins! "
                        + $"Last move was in Column {roomModel.Turns.Last().ColNum}.",
                    roomModel.Players[(int)roomModel.ResultCode - 1].Color
                );
            }
            Console.WriteLine("\n");
            WriteResultTable(
                new List<IResultModel> { RoomBLL.ConvertToResultModel(roomModel) }
            );
            Console.Write("\n     Press any key to return to the Main Menu.");
            Console.ReadKey(intercept: true);
        }

        private static void GamePlayLoop(IRoomModel room, IRoomBLL roomBll)
        {
            bool isPlaying = true;
            room = roomBll.UpdateWithLatestTurn(room);
            while (isPlaying)
            {
                Console.Clear();
                Console.Write("             ");
                WriteTitle();
                WriteBoard(room);
                if (room.ResultCode != null)
                {
                    GameEnd(room);
                    return;
                }
                Console.Write($"\n     {room.Message}\n");
                if (room.LocalPlayerNum == room.CurrentPlayerNum)
                {
                    Console.Write("\n     --> ");
                    string response = Console.ReadLine();

                    int colNum;
                    try
                    {
                        colNum = int.Parse(response);
                    }
                    catch (FormatException)
                    {
                        room.Message =
                            "Please enter a number for the column you would like to choose.";
                        continue;
                    }

                    room = roomBll.TryAddTurnToRoom(colNum, room);
                }
                else if (room.LocalPlayerNum != room.CurrentPlayerNum)
                {
                    room = roomBll.WaitForOpponentToPlay(room);
                }
            }
        }

        private static string GetPlayerName()
        {
            string message = string.Empty;
            //initializes variables for the line that the user will be writing at with these two ints
            const int inputLineFromTopLine = 3;
            const int inputLineWidth = 4;
            while (_localPlayerName?.Length == 0)
            {
                Console.Clear();
                WriteTitle();
                Console.Write("What is your name?\n");
                Console.Write("--> ");
                WriteInColor($"\n\n{message}", ConsoleColor.Red);
                _localPlayerName = GetUserInput(inputLineWidth, inputLineFromTopLine);
                if (_localPlayerName != null && string.IsNullOrWhiteSpace(_localPlayerName))
                {
                    message =
                        "Please enter a name or press escape(Esc) to return to the main menu.";
                }
            }
            return _localPlayerName;
        }

        /// <summary>
        /// Allows user to type a string with the ability to edit it, press enter to return the string, or press escape to return a null string.
        /// </summary>
        /// <param name="inputLineWidth">The character location from the left of the window for the cursor on the line. Starts at 1 value.</param>
        /// <param name="inputLineFromTopLine">The line from the first line on the top of the window that the user will write on. Starts at 0 value.</param>
        /// <returns>String if enter is pressed and null if escape is pressed</returns>
        private static string GetUserInput(int inputLineWidth, int inputLineFromTopLine)
        {
            Console.CursorTop = inputLineFromTopLine;
            Console.CursorLeft = inputLineWidth;
            StringBuilder sb = new StringBuilder();
            ConsoleKeyInfo input = new ConsoleKeyInfo();
            if (Console.KeyAvailable)
            {
                Console.CursorLeft = inputLineWidth;
                //This blocks the user from spamming and clears input stream (KeyAvailable) up to this point
                while (Console.KeyAvailable)
                {
                    Console.ReadKey(true);
                }
            }
            while (input.Key != ConsoleKey.Enter && input.Key != ConsoleKey.Escape)
            {
                input = Console.ReadKey(false);
                if (input.Key.Equals(ConsoleKey.Backspace))
                {
                    if (sb.Length > 0)
                    {
                        string removeChar = sb.ToString(0, sb.Length - 1);
                        Console.CursorLeft = inputLineWidth + removeChar.Length;
                        Console.Write(" ");
                        Console.CursorLeft = inputLineWidth + removeChar.Length;
                        sb.Clear();
                        sb.Append(removeChar);
                    }
                    else
                    {
                        Console.CursorLeft = inputLineWidth;
                    }
                }
                else if (input.Key != ConsoleKey.Enter && input.Key != ConsoleKey.Escape)
                {
                    sb.Append(input.KeyChar);
                }
            }
            if (input.Key == ConsoleKey.Escape)
            {
                return null;
            }
            return sb.ToString();
        }

        private static void HandleDisplayingResults()
        {
            IRoomBLL roomBll = new RoomBLL();
            List<IResultModel> results = roomBll.GetAllFinished();
            Console.Clear();
            if (results.Count == 0)
            {
                Console.WriteLine("No game results");
                return;
            }
            WriteResultTable(results);
            Console.WriteLine("Press any key to return to main menu...");
            Console.ReadKey();
            Console.Clear();
        }

        private static void HandleHostingGame()
        {
            IPlayerModel localPlayer = new PlayerModel();
            bool isWaiting = true;
            string opponentName = string.Empty;

            if (_localPlayerName?.Length == 0)
            {
                _localPlayerName = GetPlayerName();
                if (_localPlayerName == null)
                {
                    _localPlayerName = string.Empty;
                    return;
                }
            }
            Console.Clear();
            WriteTitle();

            IRoomBLL roomBll = new RoomBLL();
            IRoomModel room = roomBll.AddPlayerToRoom(_localPlayerName, roomBll.AddNewRoom());
            int localPlayerNum = room.LocalPlayerNum;

            if (room.Players[0] == null)
            {
                localPlayer = room.Players[1];
            }
            else
            {
                localPlayer = room.Players[0];
            }

            Console.WriteLine($"       Room ID: {room.Id}");
            Console.WriteLine("\nWaiting for opponent...");
            Console.WriteLine("\nPress escape to return to the main menu.");

            while (isWaiting)
            {
                Thread.Sleep(1000);
                room = roomBll.GetRoomById((int)room.Id);
                room.LocalPlayerNum = localPlayerNum;

                if (!room.Vacancy)
                {
                    isWaiting = false;
                    Console.Clear();
                    WriteTitle();
                    Console.WriteLine($"       Room ID: {room.Id}");

                    if (localPlayer.Num == 1)
                    {
                        opponentName = room.Players[1].Name;
                    }
                    else
                    {
                        opponentName = room.Players[0].Name;
                    }

                    Console.WriteLine($"\n{opponentName} has joined!");
                    Console.WriteLine("\nPress any key to continue to the game.");
                    Console.ReadKey();

                    GamePlayLoop(room, roomBll);
                    Console.Clear();
                }

                if (Console.KeyAvailable)
                {
                    if (Console.ReadKey().Key == ConsoleKey.Escape)
                    {
                        room.ResultCode = -1;
                        Console.Clear();
                        WriteTitle();
                        Console.WriteLine(
                            "The room has been closed. Returning to the main menu."
                        );
                        Thread.Sleep(2000);
                        Console.Clear();
                        isWaiting = false;
                    }
                }
            }
        }

        private static void HandleJoiningGame()
        {
            IRoomModel room = new RoomModel();
            //initializes variables for the line that the user will be writing at with these two ints
            const int inputLineFromTopLine = 3;
            const int inputLineWidth = 4;
            bool isJoining = true;
            if (_localPlayerName?.Length == 0)
            {
                _localPlayerName = GetPlayerName();
                if (_localPlayerName == null)
                {
                    _localPlayerName = string.Empty;
                    return;
                }
            }
            IRoomBLL roomBll = new RoomBLL();
            while (isJoining)
            {
                Console.Clear();
                WriteTitle();
                Console.WriteLine("What is the Room Id you would like to join?");
                Console.Write("--> ");
                WriteInColor($"\n\n{room.Message}", ConsoleColor.Red);
                string userInput = GetUserInput(inputLineWidth, inputLineFromTopLine);
                if (userInput == null)
                {
                    return;
                }
                int roomId;
                try
                {
                    roomId = int.Parse(userInput);
                }
                catch (FormatException)
                {
                    room.Message =
                        "Please enter an integer ID. To quit trying to join a room press the escape(Esc) key.";
                    continue;
                }
                try
                {
                    room = roomBll.AddPlayerToRoom(_localPlayerName, roomId);
                    isJoining = false;
                }
                catch (ArgumentException e)
                {
                    room.Message =
                        e.Message + " To quit trying to join a room press the escape(Esc) key.";
                }
            }
            Console.Clear();
            WriteTitle();
            Console.WriteLine(room.Message);
            Console.Write("Press any key to continue . . . ");
            Console.ReadKey(intercept: false);
            GamePlayLoop(room, roomBll);
        }

        private static void HandleNpcGame()
        {
            if (_localPlayerName?.Length == 0)
            {
                _localPlayerName = GetPlayerName();
                if (_localPlayerName == null)
                {
                    _localPlayerName = string.Empty;
                    return;
                }
            }
            Console.Clear();
            WriteTitle();

            IRoomBLL roomBll = new NPCRoomBLL();
            IRoomModel room = roomBll.AddPlayerToRoom(_localPlayerName, roomBll.AddNewRoom());
            string opponentName =
                (room.LocalPlayerNum == 1) ? room.Players[1].Name : room.Players[0].Name;

            Console.WriteLine($"       Room ID: {room.Id}");
            Console.WriteLine($"\n{opponentName} has joined!");
            Console.WriteLine("\nPress any key to continue to the game.");
            Console.ReadKey();

            GamePlayLoop(room, roomBll);
        }
    }
}
