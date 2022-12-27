using ConnectFour.Business.Models;
using ConnectFour.Business.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConnectFour.App
{
    internal static class Program
    {
        private static void Main()
        {
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
    }
}