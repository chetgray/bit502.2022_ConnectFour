using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
                        //New Single-Player Game
                        break;
                    case "2":
                        //New Multi-Player Game
                        break;
                    case "3":
                        JoinMultiPlayerGame();
                        break;
                    case "4":
                        RoomBLL rBLL = new RoomBLL();
                        DisplayResults(rBLL.GetAllFinished());
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

            IRoomModel testRoom = new RoomModel
            {
                Id = 100,
                CurrentTurnNum = 4,
                Players = new IPlayerModel[]
                {
                    new PlayerModel
                    {
                        Id = 1,
                        Name = "Aaa",
                        Symbol = "A",
                        Color = ConsoleColor.Red
                    },
                    new PlayerModel
                    {
                        Id = 2,
                        Name = "Bbb",
                        Symbol = "B",
                        Color = ConsoleColor.Yellow
                    }
                },
                Turns = new List<ITurnModel>
                {
                    new TurnModel
                    {
                        Id = 1,
                        ColNum = 1,
                        RowNum = 6,
                        Num = 1
                    },
                    new TurnModel
                    {
                        Id = 2,
                        ColNum = 2,
                        RowNum = 6,
                        Num = 2
                    },
                    new TurnModel
                    {
                        Id = 3,
                        ColNum = 1,
                        RowNum = 5,
                        Num = 3
                    }
                }
            };

            DisplayBoard(testRoom);

            Console.ReadKey();
        }

        private static void DisplayResults(List<IResultModel> results)
        {
            Console.Clear();
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
                if (results[i - 1].WinnerName.Length > 15)
                {
                    ResultTable[i, 5] = $"{results[i - 1].WinnerName.Substring(0, 15)}...";
                }
                else
                {
                    ResultTable[i, 5] = results[i - 1].WinnerName;
                }
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
            Console.WriteLine("Press any key to return to main menu...");
            Console.ReadKey();
            Console.Clear();
        }

        private static void JoinMultiPlayerGame()
        {
            //initializes variables for the line that the user will be writing at with these two ints
            int inputLineFromTopLine = 3;
            int inputLineWidth = 4;
            string message = string.Empty;
            bool isJoining = true;
            _localPlayerName = GetPlayerName();
            if (_localPlayerName == string.Empty)
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
                if (userInput == null)
                {
                    return;
                }
                bool successfullInput = int.TryParse(userInput, out int roomId);
                IRoomModel roomModel = new RoomModel();
                RoomBLL rBLL = new RoomBLL();
                if (successfullInput)
                {
                    roomModel = rBLL.GetRoomById(roomId);
                }
                if (roomModel == null || !successfullInput || roomModel.ResultCode != null)
                {
                    message =
                        $"Room Id {roomId} does not match any open rooms. To quit trying to join a room press the escape(Esc) key.";
                }
                else if (roomModel.Vacancy)
                {
                    Console.Clear();
                    WriteTitle();
                    string opponentName = roomModel.Players[0].Name;
                    roomModel = rBLL.AddPlayerToOpenSeat(_localPlayerName, roomModel);
                    Console.Write(
                        $"Successfully joined room agaisnt {opponentName}\nPress any key to continue..."
                    );
                    Console.ReadKey();
                    isJoining = false;
                    //Call gameplay loop with the roomModel
                }
                else if (!roomModel.Vacancy)
                {
                    message =
                        "That room is full! To quit trying to join a room press the escape(Esc) key.";
                }
                else
                {
                    message =
                        "Something went wrong!\nTo quit trying to join a room press the escape(Esc) key.";
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
                    message =
                        "Please enter a name or press escape(Esc) to return to the main menu.";
                }
                else if (_localPlayerName == null)
                {
                    _localPlayerName = string.Empty;
                    return _localPlayerName;
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
