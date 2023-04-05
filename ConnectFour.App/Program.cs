using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using ConnectFour.Business.BLLs;
using ConnectFour.Business.BLLs.Interfaces;
using ConnectFour.Business.Models;
using ConnectFour.Business.Models.Interfaces;

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
                        LocalGameAgainstAI();
                        break;
                    case "2":
                        HostNewGame();
                        break;
                    case "3":
                        JoinMultiPlayerGame();
                        break;
                    case "4":
                        DisplayAllResults();
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

        private static void HostNewGame()
        {
            IPlayerModel localPlayer = new PlayerModel();
            bool isWaiting = true;
            string opponentName = string.Empty;

            if (_localPlayerName == string.Empty)
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

            IRoomBLL rBLL = new RoomBLL();
            IRoomModel room = rBLL.AddPlayerToRoom(_localPlayerName, rBLL.InsertNewRoom());
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
                Thread.Sleep(2000);
                room = rBLL.GetRoomById((int)room.Id);
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

                    GamePlayLoop(room, rBLL);
                    Console.Clear();
                }

                if (Console.KeyAvailable == true)
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

        private static void GamePlayLoop(IRoomModel room, IRoomBLL rBLL)
        {
            bool isPlaying = true;
            room = rBLL.UpdateWithLastTurn(room);
            while (isPlaying)
            {
                Console.Clear();
                Console.Write("             ");
                WriteTitle();
                DisplayBoard(room);
                if (room.ResultCode != null)
                {
                    HandleGameEnd(room);
                    return;
                }
                Console.Write($"\n     {room.Message}\n");
                Console.ResetColor();
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
                            "Please enter an integer for the column you would like to choose.";
                        continue;
                    }

                    room = rBLL.TryAddTurnToRoom(colNum, room);
                }
                else if (room.LocalPlayerNum != room.CurrentPlayerNum)
                {
                    room = rBLL.LetThemPlay(room);
                }
            }
        }

        private static void HandleGameEnd(IRoomModel roomModel)
        {
            if (roomModel.ResultCode == 0)
            {
                WriteInColor($"\n     DRAW!!!", ConsoleColor.Blue);
            }
            else
            {
                WriteInColor(
                    $"\n     {roomModel.Players[(int)roomModel.ResultCode - 1].Name} Wins! "
                        + $"Last move was in Column {roomModel.Turns.Last().ColNum}.",
                    roomModel.Players[(int)roomModel.ResultCode - 1].Color
                );
            }
            Console.ResetColor();
            Console.WriteLine("\n");
            WriteResultTable(
                new List<IResultModel> { RoomBLL.ConvertToResultModel(roomModel) }
            );
            Console.Write("\n     Press any key to return to the Main Menu.");
            Console.ReadKey(intercept: true);
        }

        private static void JoinMultiPlayerGame()
        {
            IRoomModel room = new RoomModel();
            //initializes variables for the line that the user will be writing at with these two ints
            int inputLineFromTopLine = 3;
            int inputLineWidth = 4;
            bool isJoining = true;
            if (_localPlayerName == string.Empty)
            {
                _localPlayerName = GetPlayerName();
                if (_localPlayerName == null)
                {
                    _localPlayerName = string.Empty;
                    return;
                }
            }
            IRoomBLL rBLL = new RoomBLL();
            while (isJoining)
            {
                Console.Clear();
                WriteTitle();
                Console.WriteLine("What is the Room Id you would like to join?");
                Console.Write($"--> ");
                WriteInColor($"\n\n{room.Message}", ConsoleColor.Red);
                Console.ResetColor();
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
                catch (FormatException e)
                {
                    room.Message =
                        "Please enter an integer ID. To quit trying to join a room press the escape(Esc) key.";
                    continue;
                }
                try
                {
                    room = rBLL.AddPlayerToRoom(_localPlayerName, roomId);
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
            GamePlayLoop(room, rBLL);
        }

        private static void LocalGameAgainstAI()
        {
            if (_localPlayerName == string.Empty)
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

            IRoomBLL rBLL = new NPCRoomBLL();
            IRoomModel room = rBLL.AddPlayerToRoom(_localPlayerName, rBLL.InsertNewRoom());
            string opponentName = (room.LocalPlayerNum == 1) ? room.Players[1].Name : room.Players[0].Name;

            Console.WriteLine($"       Room ID: {room.Id}");
            Console.WriteLine($"\n{opponentName} has joined!");
            Console.WriteLine("\nPress any key to continue to the game.");
            Console.ReadKey();

            GamePlayLoop(room, rBLL);
        }

        private static string GetPlayerName()
        {
            string message = string.Empty;
            int inputLineFromTopLine = 3;
            int inputLineWidth = 4;
            while (_localPlayerName == string.Empty)
            {
                Console.Clear();
                WriteTitle();
                Console.Write("What is your name?\n");
                Console.Write($"--> ");
                WriteInColor($"\n\n{message}", ConsoleColor.Red);
                Console.ResetColor();
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

        private static void DisplayAllResults()
        {
            RoomBLL rBLL = new RoomBLL();
            List<IResultModel> results = rBLL.GetAllFinished();
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

        private static void WriteResultTable(List<IResultModel> results)
        {
            // initialize table array with headers
            string[,] resultTable = new string[results.Count + 1, 7];
            resultTable[0, 0] = "Room ID";
            resultTable[0, 1] = "Started At";
            resultTable[0, 2] = "Duration";
            resultTable[0, 3] = "Player 1";
            resultTable[0, 4] = "Player 2";
            resultTable[0, 5] = "Winner";
            resultTable[0, 6] = "Number of Moves";

            // initialize column widths to header widths
            int[] columnWidths = new int[resultTable.GetLength(1)];
            for (int c = 0; c < columnWidths.Length; c++)
            {
                columnWidths[c] = resultTable[0, c].Length;
            }

            // fill table array with data
            const int maxPlayerNameLength = 15;
            for (int r = 1; r <= results.Count; r++)
            {
                resultTable[r, 0] = results[r - 1].RoomId.ToString();
                resultTable[r, 1] = results[r - 1].CreationTime.ToString("MM/dd/yyyy hh:mm tt");
                resultTable[r, 2] = GetGameDuration(results[r - 1].Duration);
                resultTable[r, 3] =
                    results[r - 1].Players[0].Length <= maxPlayerNameLength
                        ? results[r - 1].Players[0]
                        : $"{results[r - 1].Players[0].Substring(0, maxPlayerNameLength - 3)}...";
                resultTable[r, 4] =
                    results[r - 1].Players[1].Length <= maxPlayerNameLength
                        ? results[r - 1].Players[1]
                        : $"{results[r - 1].Players[1].Substring(0, maxPlayerNameLength - 3)}...";
                resultTable[r, 5] =
                    results[r - 1].WinnerName.Length <= maxPlayerNameLength
                        ? results[r - 1].WinnerName
                        : $"{results[r - 1].WinnerName.Substring(0, maxPlayerNameLength - 3)}...";
                resultTable[r, 6] = results[r - 1].LastTurnNum;
                // update column widths if necessary
                for (int c = 0; c < resultTable.GetLength(1); c++)
                {
                    if (columnWidths[c] < resultTable[r, c].Length)
                    {
                        columnWidths[c] = resultTable[r, c].Length;
                    }
                }
            }

            StringBuilder rowBuilder = new StringBuilder();
            for (int r = 0; r < resultTable.GetLength(0); r++)
            {
                if (r >= 1)
                {
                    // write row top border
                    (string leftBorder, string middleBorder, string rightBorder) =
                        (r == 1) ? (" ╔═", "═╦═", "═╗") : (" ╠═", "═╬═", "═╣");
                    WriteBorder('═', leftBorder, middleBorder, rightBorder);
                }
                // write row (header or body)
                string vertcalBorder = (r > 0) ? " ║ " : "   ";
                for (int c = 0; c < columnWidths.Length; c++)
                {
                    rowBuilder.Append(vertcalBorder);
                    // center resultTable[r, c] within columnWidths[c]
                    rowBuilder.Append(
                        resultTable[r, c]
                            .PadRight((columnWidths[c] + resultTable[r, c].Length) / 2)
                            .PadLeft(columnWidths[c])
                    );
                }
                rowBuilder.Append(vertcalBorder);
                Console.WriteLine(rowBuilder.ToString());
                rowBuilder.Clear();
            }
            // write table bottom border
            WriteBorder('═', " ╚═", "═╩═", "═╝");

            void WriteBorder(
                char horizontalBorder,
                string leftBorder,
                string middleBorder,
                string rightBorder
            )
            {
                StringBuilder borderBuilder = new StringBuilder();
                borderBuilder.Append(leftBorder);
                for (int c = 0; c < columnWidths.Length; c++)
                {
                    borderBuilder.Append(new string(horizontalBorder, columnWidths[c]));
                    if (c < columnWidths.Length - 1)
                    {
                        borderBuilder.Append(middleBorder);
                    }
                }
                borderBuilder.Append(rightBorder);
                Console.WriteLine(borderBuilder.ToString());
            }
        }

        internal static string GetGameDuration(TimeSpan duration)
        {
            int days = (int)duration.TotalDays;
            int hours = (int)duration.TotalHours;
            int minutes = (int)duration.TotalMinutes;
            if (days >= 1)
            {
                if (days > 1)
                {
                    return $"{days} Days";
                }
                else
                {
                    return $"{days} Day";
                }
            }
            else if (hours >= 1)
            {
                if (hours > 1)
                {
                    return $"{hours} Hours";
                }
                else
                {
                    return $"{hours} Hour";
                }
            }
            else
            {
                if (minutes > 1)
                {
                    return $"{minutes} Minutes";
                }
                else
                {
                    return $"{minutes} Minute";
                }
            }
        }

        private static void DisplayBoard(IRoomModel room)
        {
            string noPiece = "     ";
            string p1Piece = $"░ {room.Players[0].Symbol} ░";
            string p2Piece = $"░ {room.Players[1].Symbol} ░";

            if (p1Piece == p2Piece)
            {
                p1Piece = "░ 1 ░";
                p2Piece = "░ 2 ░";
            }

            for (int r = 0; r < room.Board.GetLength(0); r++)
            {
                for (int c = 0; c < room.Board.GetLength(1); c++)
                {
                    room.Board[r, c] = 0;
                }
            }

            for (int t = 0; t < room.Turns.Count; t++)
            {
                if (room.Turns[t].Num % 2 == 0)
                {
                    room.Board[room.Turns[t].RowNum - 1, room.Turns[t].ColNum - 1] = 2;
                }
                else if (room.Turns[t].Num % 2 != 0)
                {
                    room.Board[room.Turns[t].RowNum - 1, room.Turns[t].ColNum - 1] = 1;
                }
            }

            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine("\n    ╔═════╦═════╦═════╦═════╦═════╦═════╦═════╗");

            for (int r = 0; r < room.Board.GetLength(0); r++)
            {
                Console.Write("    ");
                for (int c = 0; c < room.Board.GetLength(1); c++)
                {
                    Console.Write("║");
                    if (room.Board[r, c] == 1)
                    {
                        Console.ForegroundColor = room.Players[0].Color;
                        Console.Write(p1Piece);
                    }
                    else if (room.Board[r, c] == 2)
                    {
                        Console.ForegroundColor = room.Players[1].Color;
                        Console.Write(p2Piece);
                    }
                    else
                    {
                        Console.Write(noPiece);
                    }
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                }
                Console.Write("║");
                Console.ResetColor();
                switch (r)
                {
                    case 1:
                        Console.Write($"    Room ID: {room.Id}");
                        break;

                    case 2:
                        Console.Write($"    Player 1: ");
                        Console.ForegroundColor = room.Players[0].Color;
                        Console.Write(room.Players[0].Name);
                        Console.ResetColor();
                        break;

                    case 3:
                        Console.Write($"    Player 2: ");
                        Console.ForegroundColor = room.Players[1].Color;
                        Console.Write(room.Players[1].Name);
                        Console.ResetColor();
                        break;

                    case 4:
                        Console.Write($"    Last play: ");
                        if (room.Turns.Count != 0)
                        {
                            Console.Write(room.Turns.Last().ColNum);
                        }
                        else
                        {
                            Console.Write("N/A");
                        }
                        break;

                    case 5:
                        Console.Write("    Current turn: ");

                        if (room.CurrentTurnNum % 2 == 0)
                        {
                            Console.ForegroundColor = room.Players[1].Color;
                            Console.Write(room.Players[1].Name);
                        }
                        else
                        {
                            Console.ForegroundColor = room.Players[0].Color;
                            Console.Write(room.Players[0].Name);
                        }
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                        break;

                    default:
                        break;
                }

                if (r != 5)
                {
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.WriteLine("\n    ╠═════╬═════╬═════╬═════╬═════╬═════╬═════╣");
                }
            }

            Console.WriteLine("\n    ╚═════╩═════╩═════╩═════╩═════╩═════╩═════╝");
            Console.ResetColor();
            Console.WriteLine("       1     2     3     4     5     6     7");
        }

        private static void WriteTitle()
        {
            Console.Write("         ");
            WriteInColor("C", ConsoleColor.DarkRed);
            WriteInColor("O", ConsoleColor.DarkYellow);
            WriteInColor("NNE", ConsoleColor.DarkCyan);
            WriteInColor("C", ConsoleColor.DarkRed);
            WriteInColor("T4\n\n", ConsoleColor.DarkCyan);
            Console.ResetColor();
        }

        private static void WriteInColor(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(text);
        }
    }
}
