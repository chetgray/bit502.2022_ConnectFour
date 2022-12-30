using System;
using System.Collections.Generic;
using System.Linq;

using ConnectFour.Business.Models;
using ConnectFour.Business.Models.Interfaces;

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
    }
}
