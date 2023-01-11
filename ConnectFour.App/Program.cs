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
        private static void DisplayResults(List<IResultModel> results)
        {
            string[,] ResultTable = new string[results.Count + 1,7];
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
                string winnerName = string.Empty;
                if (resultCode > 0 && resultCode < 3)
                {
                    winnerName = $"{results[i - 1].Players[resultCode - 1].Name}";
                }
                else if(resultCode == 0)
                {
                    winnerName = "DRAW";
                }
                else
                {
                    winnerName = "NULL";
                }
                ResultTable[i, 0] = results[i - 1].RoomId.ToString();
                ResultTable[i, 1] = results[i - 1].CreationTime.ToString();
                ResultTable[i, 2] = GetGameDuration(results[i-1].LastTurn.Time - results[i-1].CreationTime);
                ResultTable[i, 3] = results[i - 1].Players[0].Name;
                ResultTable[i, 4] = results[i - 1].Players[1].Name;
                ResultTable[i, 5] = winnerName;
                ResultTable[i, 6] = results[i - 1].LastTurn.Num.ToString();
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
                if(i == 0)
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
                else
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
                for (int j = 0; j < columns; j++)
                {
                    sb.Append(" ║ ");
                    int columnWidth = widths[j];
                    int stringLength = ResultTable[i, j].Length;
                    int spacingBeforeAndAfter = columnWidth - stringLength;
                    if(spacingBeforeAndAfter % 2 == 0)
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

                sb.Append(" ║");
                Console.WriteLine(sb.ToString());
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

        private static string GetGameDuration(TimeSpan duration)
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