using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ConnectFour.Business.Models.Interfaces;

namespace ConnectFour.App
{
    internal static class ProgramHelpers
    {
        /// <summary>
        /// Converts the value of the this <see cref="TimeSpan"/> object to a truncated string representation.
        /// </summary>
        /// <param name="unitCount">
        /// The number of units to include in the string representation.
        /// </param>
        /// <returns>
        /// A truncated string representation of this <see cref="TimeSpan"/> object, with only the largest <paramref
        /// name="unitCount"/> number of units included.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when <paramref name="unitCount"/> is less than 1.
        /// </exception>
        internal static string ToTruncatedString(this TimeSpan timeSpan, int unitCount)
        {
            if (unitCount < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(unitCount));
            }
            StringBuilder sb = new StringBuilder();
            if (timeSpan.TotalDays >= 1)
            {
                sb.Append(timeSpan.Days).Append("d").Append(unitCount > 1 ? " " : "");
                unitCount--;
            }
            if (unitCount > 0 && timeSpan.TotalHours >= 1)
            {
                sb.Append(timeSpan.Hours).Append("h").Append(unitCount > 1 ? " " : "");
                unitCount--;
            }
            if (unitCount > 0 && timeSpan.TotalMinutes >= 1)
            {
                sb.Append(timeSpan.Minutes).Append("m").Append(unitCount > 1 ? " " : "");
                unitCount--;
            }
            if (unitCount > 0)
            {
                sb.Append(timeSpan.Seconds).Append("s");
            }
            return sb.ToString();
        }

        internal static void WriteBoard(IRoomModel room)
        {
            const string noPiece = "     ";
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

            WriteInColor(
                "\n    ╔═════╦═════╦═════╦═════╦═════╦═════╦═════╗\n",
                ConsoleColor.DarkBlue
            );

            for (int r = 0; r < room.Board.GetLength(0); r++)
            {
                Console.Write("    ");
                for (int c = 0; c < room.Board.GetLength(1); c++)
                {
                    WriteInColor("║", ConsoleColor.DarkBlue);
                    if (room.Board[r, c] == 1)
                    {
                        WriteInColor(p1Piece, room.Players[0].Color);
                    }
                    else if (room.Board[r, c] == 2)
                    {
                        WriteInColor(p2Piece, room.Players[1].Color);
                    }
                    else
                    {
                        Console.Write(noPiece);
                    }
                }
                WriteInColor("║", ConsoleColor.DarkBlue);
                switch (r)
                {
                    case 1:
                        Console.Write($"    Room ID: {room.Id}");
                        break;

                    case 2:
                        Console.Write($"    Player 1: ");
                        WriteInColor(room.Players[0].Name, room.Players[0].Color);
                        break;

                    case 3:
                        Console.Write($"    Player 2: ");
                        WriteInColor(room.Players[1].Name, room.Players[1].Color);
                        break;

                    case 4:
                        Console.Write("    Last play: ");
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
                            WriteInColor(room.Players[1].Name, room.Players[1].Color);
                        }
                        else
                        {
                            WriteInColor(room.Players[0].Name, room.Players[0].Color);
                        }
                        break;

                    default:
                        break;
                }

                if (r != 5)
                {
                    WriteInColor(
                        "\n    ╠═════╬═════╬═════╬═════╬═════╬═════╬═════╣\n",
                        ConsoleColor.DarkBlue
                    );
                }
            }
            WriteInColor(
                "\n    ╚═════╩═════╩═════╩═════╩═════╩═════╩═════╝\n",
                ConsoleColor.DarkBlue
            );
            Console.WriteLine("       1     2     3     4     5     6     7");
        }

        internal static void WriteInColor(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ResetColor();
        }

        internal static void WriteResultTable(List<IResultModel> results)
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
                resultTable[r, 2] = results[r - 1].Duration.ToTruncatedString(2);
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
                resultTable[r, 6] = results[r - 1].LastTurnNum.ToString();
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

        internal static void WriteTitle()
        {
            Console.Write("         ");
            WriteInColor("C", ConsoleColor.DarkRed);
            WriteInColor("O", ConsoleColor.DarkYellow);
            WriteInColor("NNE", ConsoleColor.DarkCyan);
            WriteInColor("C", ConsoleColor.DarkRed);
            WriteInColor("T4\n\n", ConsoleColor.DarkCyan);
        }
    }
}
