using System;
using System.Collections.Generic;

using ConnectFour.Business.Models.Interfaces;

namespace ConnectFour.Business.Models
{
    public class RoomModel : IRoomModel
    {
        public int? Id { get; set; }
        public DateTime CreationTime { get; set; } = DateTime.Now;
        public int? CurrentTurnNum { get; set; }
        public int? ResultCode { get; set; }

        public IPlayerModel[] Players { get; set; } = new IPlayerModel[2];
        public List<ITurnModel> Turns { get; set; } = new List<ITurnModel>();

        public int[,] Board { get; set; } = new int[6, 7];
        public bool Vacancy { get; set; }

        public string Message { get; set; }

        public bool CheckForWin(ITurnModel turn)
        {
            // Bail if the turn's cell is already occupied.
            if (Board[turn.RowNum - 1, turn.ColNum - 1] != 0)
            {
                return false;
            }

            int playerNum = ((turn.Num - 1) % Players.Length) + 1;

            // →
            // Check up to three cells to the left and three cells to the right of the Turn's
            // column for four in a row.
            int count = 0;
            for (
                int c = Math.Max(0, turn.ColNum - 3 - 1);
                c <= Math.Min(turn.ColNum + 3 - 1, Board.GetLength(1) - 1);
                c++
            )
            {
                if (Board[turn.RowNum - 1, c] != playerNum && c != turn.ColNum - 1)
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

            // ↑
            // Check up to three cells below Turn's row for four in a row. Not checking above
            // because a new play will only ever be at the top.
            count = 0;
            for (
                int r = Math.Min(turn.RowNum + 3 - 1, Board.GetLength(0) - 1);
                r >= turn.RowNum - 1;
                r--
            )
            {
                if (Board[r, turn.ColNum - 1] != playerNum && r != turn.RowNum - 1)
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

            // ↗
            // Check ascending (left-to-right) diagonal
            count = 0;
            for (
                int r = turn.RowNum + 3 - 1, c = turn.ColNum - 3 - 1;
                r >= Math.Max(0, turn.RowNum - 3 - 1)
                    && c <= Math.Min(turn.ColNum + 3 - 1, Board.GetLength(1) - 1);
                r--, c++
            )
            {
                if (r > Board.GetLength(0) - 1 || c < 0)
                {
                    continue;
                }
                if (Board[r, c] != playerNum && r != turn.RowNum - 1 && c != turn.ColNum - 1)
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

            // ↘
            // Check descending (left-to-right) diagonal
            count = 0;
            for (
                int r = turn.RowNum - 3 - 1, c = turn.ColNum - 3 - 1;
                r <= Math.Min(turn.RowNum + 3 - 1, Board.GetLength(0) - 1)
                    && c <= Math.Min(turn.ColNum + 3 - 1, Board.GetLength(1) - 1);
                r++, c++
            )
            {
                if (r < 0 || c < 0)
                {
                    continue;
                }
                if (Board[r, c] != playerNum && r != turn.RowNum - 1 && c != turn.ColNum - 1)
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

            return false;
        }
    }
}
