using ConnectFour.Business.BLLs;
using ConnectFour.Business.Models;
using ConnectFour.Business.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ConnectFour.App
{
    internal static class Program
    {
        private static string _localPlayerName = string.Empty;

        private static void Main()
        {
            bool isRunning = true;
            while (isRunning)
            {
                bool isChoosing = true;
                bool isQuitting = false;
                while (isChoosing)
                {
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
                            HostNewGame();
                            break;

                        case "3":
                            //Join Multi-Player Game
                            break;

                        case "4":
                            Console.Clear();
                            RoomBLL rBLL = new RoomBLL();
                            DisplayResults(rBLL.GetAllFinished());
                            Console.WriteLine("Press any key to continue...");
                            Console.ReadKey();
                            break;

                        case "5":
                            isQuitting = true;
                            isRunning = false;
                            break;

                        default:
                            Console.Clear();
                            Console.WriteLine("Invalid response please choose 1, 2, 3, 4, or 5");
                            break;
                    }
                }
                Console.Clear();
                if (isQuitting == false)
                {
                    WriteTitle();
                    Console.WriteLine("What would you like to do?\n" +
                    "[1] Return to the Main Menu\n" +
                    "[2] Quit");
                    Console.Write("--> ");
                    string continueProgram = Console.ReadLine();
                    if (continueProgram == "2")
                    {
                        isRunning = false;
                    }
                    Console.Clear();
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

        private static void HostNewGame()
        {
            Random random = new Random();
            bool isWaiting = true;

            Console.Clear();
            WriteTitle();

            RoomBLL roomBLL = new RoomBLL();
            RoomModel newRoom = new RoomModel
            {
                Id = roomBLL.InsertNewRoom(),
            };

            if (_localPlayerName == string.Empty)
            {
                Console.Write("What is your name?\n--> ");
                _localPlayerName = Console.ReadLine();
                Console.Clear();
                WriteTitle();
            }

            PlayerModel localPlayer = new PlayerModel
            {
                Name = _localPlayerName,
                Symbol = _localPlayerName.Substring(0, 1),
                Num = random.Next(1, 3)
            };
            newRoom.Players.Add(localPlayer);


            Console.WriteLine($"       Room ID: {newRoom.Id}");
            Console.WriteLine("\nWaiting for opponent...");
            Console.WriteLine("\nPress escape to return to the main menu.");

            while (isWaiting)
            {
                Thread.Sleep(2000);

                if (newRoom.Players.Count() == 2)
                {
                    isWaiting = false;
                    Console.Clear();
                    WriteTitle();
                    Console.WriteLine($"       Room ID: {newRoom.Id}");
                    Console.WriteLine($"\n{newRoom.Players[1].Name} has joined!");
                    Console.WriteLine("\nPress any key to continue to the game.");
                    Console.ReadKey();

                    //Sending to main menu until gameplay loop has been implemented
                    Console.Clear();
                }

                if (Console.KeyAvailable == true)
                {
                    if (Console.ReadKey().Key == ConsoleKey.Escape)
                    {
                        newRoom.ResultCode = -1;
                        Console.Clear();
                        WriteTitle();
                        Console.WriteLine("The room has been closed. Returning to the main menu.");
                        Thread.Sleep(2000);
                        Console.Clear();
                        isWaiting = false;
                    }
                }
                //Test
                //Adding second "player" to room to demo oppononent joining
                newRoom.Players.Add(localPlayer);
            }
        }

        private static void DisplayResults(List<IResultModel> results)
        {
            if (results.Count == 0)
            {
                Console.WriteLine("No game results");
                return;
            }
            string[,] ResultTable = new string[results.Count + 1, 7];
            ResultTable[0, 0] = "Room ID";
            ResultTable[0, 1] = "Started At";
            ResultTable[0, 2] = "Duration";
            ResultTable[0, 3] = "Player 1";
            ResultTable[0, 4] = "Player 2";
            ResultTable[0, 5] = "Winner";
            ResultTable[0, 6] = "Number of Moves";
            int amountOfResults = results.Count();

            for (int i = 1; i <= amountOfResults; i++)
            {
                int resultCode = (int)results[i - 1].ResultCode;
                ResultTable[i, 0] = results[i - 1].RoomId.ToString();
                ResultTable[i, 1] = results[i - 1].CreationTime.ToString("MM/dd/yyyy hh:mm tt");
                ResultTable[i, 2] = results[i - 1].Duration;
                ResultTable[i, 3] = results[i - 1].Players[0];
                ResultTable[i, 4] = results[i - 1].Players[1];
                ResultTable[i, 5] = results[i - 1].WinnerName;
                ResultTable[i, 6] = results[i - 1].LastTurnNum;
            }

            int rows = ResultTable.GetLength(0);
            int columns = ResultTable.GetLength(1);
            int[] widths = new int[columns];

            for (int i = 0; i < columns; i++)
            {
                widths[i] = ResultTable[0, i].Length;

                for (int j = 0; j < rows; j++)
                {
                    if (ResultTable[j, i].Length > widths[i])
                    {
                        widths[i] = ResultTable[j, i].Length;
                    }
                }
            }

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < rows; i++)
            {
                if (i == 1)
                {
                    sb.Append(" ╔═");
                    for (int j = 0; j < widths.Count(); j++)
                    {
                        sb.Append(new string('═', widths[j]));
                        if (j < widths.Count() - 1)
                        {
                            sb.Append("═╦═");
                        }
                        else
                        {
                            sb.Append("═╗");
                        }
                    }
                    Console.WriteLine(sb.ToString());
                }
                else if (i > 1)
                {
                    sb.Clear();
                    sb.Append(" ╠═");
                    for (int j = 0; j < widths.Count(); j++)
                    {
                        sb.Append(new string('═', widths[j]));
                        if (j < widths.Count() - 1)
                        {
                            sb.Append("═╬═");
                        }
                        else
                        {
                            sb.Append("═╣");
                        }
                    }
                    Console.WriteLine(sb.ToString());
                }

                sb.Clear();
                string border = (i > 0) ? border = " ║ " : border = "   ";
                for (int j = 0; j < columns; j++)
                {
                    sb.Append(border);
                    int columnWidth = widths[j];
                    int stringLength = ResultTable[i, j].Length;
                    int spacingBeforeAndAfter = columnWidth - stringLength;
                    if (spacingBeforeAndAfter % 2 == 0)
                    {
                        sb.Append(' ', spacingBeforeAndAfter / 2);
                        sb.Append(ResultTable[i, j]);
                        sb.Append(' ', spacingBeforeAndAfter / 2);
                    }
                    else
                    {
                        sb.Append(" ");
                        sb.Append(' ', spacingBeforeAndAfter / 2);
                        sb.Append(ResultTable[i, j]);
                        sb.Append(' ', spacingBeforeAndAfter / 2);
                    }
                }
                sb.Append(border);
                Console.WriteLine(sb.ToString());
                sb.Clear();
            }
            sb.Clear();
            sb.Append(" ╚═");
            for (int j = 0; j < widths.Count(); j++)
            {
                sb.Append(new string('═', widths[j]));
                if (j < widths.Count() - 1)
                {
                    sb.Append("═╩═");
                }
                else
                {
                    sb.Append("═╝");
                }
            }
            Console.WriteLine(sb.ToString());
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