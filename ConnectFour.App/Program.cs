using ConnectFour.Business.BLLs;
using ConnectFour.Business.Models;
using ConnectFour.Business.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConnectFour.App
{
    internal static class Program
    {
        private static void Main()
        {
            DisplayResults(RoomBLL.GetAllFinished());

            Console.ReadKey();
        }
        private static void DisplayResults(List<ResultModel> results)
        {
            int characterCounter = 0;
            foreach (ResultModel result in results)
            {
                for (int i = 0; i < result.Players.Count; i++)
                {
                   int playerNameLength = result.Players[i].Name.Length;
                   if (playerNameLength > characterCounter)
                   {
                       characterCounter = playerNameLength;
                   }
                }
            }

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < characterCounter; i++)
            {
                sb.Append(" ");
            }
            string characterExtender = sb.ToString();
            Console.WriteLine("     Room   Started   Duration      Player 1 "+ characterExtender + "  Player 2  "+ characterExtender +"  Winner "+ characterExtender +"# of");
            Console.WriteLine("       #      At                      Name     "+ characterExtender + "  Name    "+ characterExtender +"         "+ characterExtender +"Moves");
            sb.Clear();
            for (int i = 0; i < characterCounter; i++)
            {
                sb.Append("═");
            }
            characterExtender = sb.ToString();
            Console.WriteLine("    ╔═════╦═════════╦══════════╦═══════════"+ characterExtender + "╦══════════"+ characterExtender + "╦═════════"+ characterExtender + "╦═══════╗");

            Console.WriteLine("    ╠═════╬═════════╬══════════╬═══════════"+ characterExtender + "╬══════════"+ characterExtender + "╬═════════"+ characterExtender + "╬═══════╣");

            Console.WriteLine("    ╚═════╩═════════╩══════════╩═══════════"+ characterExtender + "╩══════════"+ characterExtender + "╩═════════"+ characterExtender + "╩═══════╝");
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