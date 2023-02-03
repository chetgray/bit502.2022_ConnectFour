using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using ConnectFour.Business.BLLs;
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
                Console.WriteLine("What would you like to do?\n" +
                "[1] - New Single-Player Game\n" +
                "[2] - New Multi-Player Game\n" +
                "[3] - Join Multi-Player Game\n" +
                "[4] - View Game Results\n" +
                "[5] - Quit\n");
                Console.Write("--> ");
                string userResponse = Console.ReadLine();
                switch (userResponse)
                {
                    case "1":
                        //New Single-Player Game
                        break;
                    case "2":
                        //New Multi-Player Game
                        break;
                    case "3":
                        JoinMultiPlayerGame();
                        break;
                    case "4":
                        //View Game Results
                        break;
                    case "5":
                        isChoosing = false;
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("Invalid response please choose 1, 2, 3, 4, or 5\nPress any key to go back to the main menu");
                        Console.ReadKey();
                        break;
                }
            }

            IRoomModel testRoom = new RoomModel
            {
                Id = 100,
                CurrentTurnNum = 4,
                Players = new List<IPlayerModel>
                {
                    new PlayerModel { Id = 1, Name = "Aaa", Symbol = "A", Color = ConsoleColor.Red },
                    new PlayerModel { Id = 2, Name = "Bbb", Symbol = "B", Color = ConsoleColor.Yellow }
                },
                Turns = new List<ITurnModel>
                {
                    new TurnModel { Id = 1, ColNum = 1, RowNum = 6, Num = 1 },
                    new TurnModel { Id = 2, ColNum = 2, RowNum = 6, Num = 2 },
                    new TurnModel { Id = 3, ColNum = 1, RowNum = 5, Num = 3 }
                }
            };

            DisplayBoard(testRoom);

            Console.ReadKey();
        }
        private static void JoinMultiPlayerGame()
        {
            //initializes variables for the line that the user will be writing at with these two ints
            int inputLineFromTopLine = 3;
            int inputLineWidth = 4;
            string message = string.Empty;
            bool isJoining = true;
            _localPlayerName = GetPlayerName();
            if(_localPlayerName == string.Empty)
            {
                return;
            }
            while (isJoining)
            {
                Console.Clear();
                WriteTitle();
                Console.WriteLine("What is the Room Id you would like to join?");
                Console.Write($"--> ");
                WriteInColor($"\n\n{message}", ConsoleColor.Red);
                Console.ResetColor();
                string userInput = GetUserInput(inputLineWidth, inputLineFromTopLine);
                if(userInput == "ReturnToMainMenu")
                {
                    return;
                }
                bool successfullInput = int.TryParse(userInput, out int roomId);
                IRoomModel roomModel = new RoomModel();
                RoomBLL rBLL = new RoomBLL();
                if (successfullInput)
                {
                    roomModel = rBLL.GetRoomOccupancy(roomId);
                }
                if (roomModel.Id == null || !successfullInput || roomModel.ResultCode != null)
                {
                    message = $"Room Id {roomId} does not match any open rooms. To quit trying to join a room press the escape(Esc) key.";
                }
                else if (roomModel.Vacancy)
                {
                    Console.Clear();
                    WriteTitle();
                    roomModel = rBLL.AddPlayerToRoom(_localPlayerName, roomModel);
                    Console.Write($"Successfully joined room agaisnt {roomModel.Players[0].Name}\nPress any key to continue...");
                    Console.ReadKey();
                    isJoining = false;
                    //Call gameplay loop with the roomModel
                }
                else if (!roomModel.Vacancy)
                {
                    message = "That room is full! To quit trying to join a room press the escape(Esc) key.";
                }
                else
                {
                    message = "Something went wrong!\nTo quit trying to join a room press the escape(Esc) key.";
                }
                Console.Clear();
            }
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
                if (_localPlayerName == string.Empty)
                {
                    message = "Please enter a name or press escape(Esc) to return to the main menu.";
                }
                else if (_localPlayerName == "ReturnToMainMenu")
                {
                    _localPlayerName = string.Empty;
                    return _localPlayerName;
                }
            }
            return _localPlayerName;
        }

        /// <summary>
        /// Allows user to type a string with the ability to edit it, press enter to return the string, or press escape to return string containing "ReturnToMainMenu".
        /// </summary>
        /// <param name="inputLineWidth">The character location from the left of the window for the cursor on the line</param>
        /// <param name="inputLineFromTopLine">The line from the first line on the top of the window that the user will write on. Starts at 0 value.</param>
        /// <returns></returns>
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
                Thread.Sleep(2000);
                while (Console.KeyAvailable)
                {                    
                    Console.ReadKey(false);
                    Console.CursorLeft = inputLineWidth;
                    Console.Write(" ");
                    Console.CursorLeft = inputLineWidth;
                }
            }
            do
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
                }
                else if (input.Key != ConsoleKey.Enter && input.Key != ConsoleKey.Escape)
                {
                    sb.Append(input.KeyChar);
                }
            } while (input.Key != ConsoleKey.Enter && input.Key != ConsoleKey.Escape);
            if (input.Key == ConsoleKey.Escape)
            {
                return "ReturnToMainMenu";
            }
            return sb.ToString();
        }

        private static void DisplayBoard(IRoomModel room)
        {
            string p1 = $"░ {room.Players[0].Symbol} ░";
            string p2 = $"░ {room.Players[1].Symbol} ░";

            if (p1 == p2)
            {
                p1 = "░ 1 ░";
                p2 = "░ 2 ░";
            }

            for (int i = 0; i < room.Board.GetLength(0); i++)
            {
                for (int j = 0; j < room.Board.GetLength(1); j++)
                {
                    room.Board[i, j] = "     ";
                }
            }

            for (int i = 0; i < room.Turns.Count; i++)
            {
                if (room.Turns[i].Num % 2 == 0)
                {
                    room.Board[room.Turns[i].RowNum - 1, room.Turns[i].ColNum - 1] = p2;
                }
                else if (room.Turns[i].Num % 2 != 0)
                {
                    room.Board[room.Turns[i].RowNum - 1, room.Turns[i].ColNum - 1] = p1;
                }
            }

            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine("\n    ╔═════╦═════╦═════╦═════╦═════╦═════╦═════╗");

            for (int i = 0; i < room.Board.GetLength(0); i++)
            {
                Console.Write("    ");
                for (int j = 0; j < room.Board.GetLength(1); j++)
                {
                    Console.Write("║");
                    if (room.Board[i, j] == p1)
                    {
                        Console.ForegroundColor = room.Players[0].Color;
                        Console.Write(room.Board[i, j]);
                    }
                    else if (room.Board[i, j] == p2)
                    {
                        Console.ForegroundColor = room.Players[1].Color;
                        Console.Write(room.Board[i, j]);
                    }
                    else
                    {
                        Console.Write(room.Board[i, j]);
                    }
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                }
                Console.Write("║");
                Console.ResetColor();
                switch (i)
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
                        Console.Write($"    Player 1: ");
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

                if (i != 5)
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