using ConnectFour.Business.BLLs;
using ConnectFour.Business.Models;
using ConnectFour.Business.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConnectFour.App
{
    internal static class Program
    {
        private static IPlayerModel localPlayer = new PlayerModel();
        private static IRoomModel localRoom = new RoomModel();
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
                    isChoosing = false;
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
                            isQuitting = true;
                            isRunning = false;
                            break;
                        default:
                            Console.Clear();
                            Console.WriteLine("Invalid response please choose 1, 2, 3, 4, or 5");
                            isChoosing = true;
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
        private static void JoinMultiPlayerGame()
        {            
            if (localPlayer.Name == string.Empty)
            {
                Console.Clear();
                WriteTitle();
                Console.Write("What is your name?\n--> ");
                localPlayer.Name = Console.ReadLine();
            }
            bool isChoosing = true;
            while (isChoosing)
            {
                Console.Clear();
                WriteTitle();
                Console.Write("What is the Room Id you would like to join?\n--> ");
                bool successfullInput = int.TryParse(Console.ReadLine(), out int roomId);
                IRoomModel roomModel = new RoomModel();
                RoomBLL rBLL = new RoomBLL();
                if (successfullInput)
                {                    
                    roomModel = rBLL.GetRoomOccupancy(roomId);
                }
                Console.Clear();
                if (roomModel.Id == null || !successfullInput)
                {
                    WriteTitle();
                    Console.WriteLine("Room Id does not match any open rooms.\nPress any key to continue...\nTo quit trying to join a room press the escape(Esc) key.");
                    isChoosing = !IsPressingEscapeKey();
                }
                else if (roomModel.Vacancy)
                {
                    WriteTitle();
                    localRoom = roomModel;
                    localPlayer = rBLL.AddPlayerToRoom(localPlayer, roomModel);
                    localRoom.Players.Add(localPlayer);
                    Console.Write($"Successfully joined room agaisnt {roomModel.Players[0].Name}\nPress any key to continue...");
                    Console.ReadKey();
                    isChoosing = false;
                    //Call gameplay loop
                }
                else
                {
                    WriteTitle();
                    Console.WriteLine("That room is full!\nPress any key to continue...\nTo quit trying to join a room press the escape(Esc) key.");
                    isChoosing = !IsPressingEscapeKey();
                }
            }
        }

        private static bool IsPressingEscapeKey()
        {
            if (Console.ReadKey().Key == ConsoleKey.Escape)
            {
                return true;
            }
            return false;
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