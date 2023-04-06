using System;

using ConnectFour.Business.Models.Interfaces;

namespace ConnectFour.Business.BLLs
{
    internal static class RoomBLLHelpers
    {
        /// <summary>
        /// Gets the next row in the given column.
        /// </summary>
        /// <param name="colNum">The column number (1-based).</param>
        /// <returns>The row number of the next row in the given column.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown if the given <paramref name="colNum"/> is less than 1 or greater than 7, or
        /// if the column is already full.
        /// </exception>
        public static int GetNextRowInCol(this IRoomModel room, int colNum)
        {
            if (colNum < 1 || colNum > room.Board.GetLength(1))
            {
                throw new ArgumentException(
                    $"Please choose a column between 1 - {room.Board.GetLength(1)}"
                );
            }
            int turnsInColumnCount = 0;
            foreach (ITurnModel turn in room.Turns)
            {
                if (turn.ColNum == colNum)
                {
                    turnsInColumnCount++;
                }
            }
            int rowNum = room.Board.GetLength(0) - turnsInColumnCount;
            if (rowNum < 1)
            {
                throw new ArgumentException($"Column {colNum} is Full!");
            }

            return rowNum;
        }

        /// <summary>
        /// Checks if the given <paramref name="turn"/> will result in a win.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if the <paramref name="turn"/> will result in a win, otherwise <see langword="false"/>.
        /// </returns>
        /// <param name="turn">The <see cref="ITurnModel"/> to check.</param>
        public static bool WillTurnWin(this IRoomModel room, ITurnModel turn)
        {
            // Bail if the turn's cell is already occupied.
            if (room.Board[turn.RowNum - 1, turn.ColNum - 1] != 0)
            {
                return false;
            }

            int playerNum = room.DeterminePlayerNum(turn.Num);

            // Check for four-in-a-row in each direction.
            foreach (
                (int rowDir, int colDir) in new (int, int)[]
                {
                    (-1, -1), // ↖
                    (-1, 1), // ↗
                    (0, 1), // →
                    (-1, 0), // ↑
                }
            )
            {
                int count = 0;
                for (int i = -3; i <= 3; i++)
                {
                    int r = turn.RowNum + rowDir * i - 1;
                    int c = turn.ColNum + colDir * i - 1;
                    // Skip to the next cell if we're outside the board.
                    if (
                        r < 0
                        || r >= room.Board.GetLength(0)
                        || c < 0
                        || c >= room.Board.GetLength(1)
                    )
                    {
                        continue;
                    }
                    // Reset count if this isn't our piece (aside from the new piece's cell).
                    if (
                        room.Board[r, c] != playerNum
                        && (r != turn.RowNum - 1 || c != turn.ColNum - 1)
                    )
                    {
                        count = 0;
                        continue;
                    }
                    count++;
                    if (count == 4)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
