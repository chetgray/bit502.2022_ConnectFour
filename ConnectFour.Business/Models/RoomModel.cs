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
                    int r = turn.RowNum + (rowDir * i) - 1;
                    int c = turn.ColNum + (colDir * i) - 1;
                    // Skip to the next cell if we're outside the board.
                    if (r < 0 || r >= Board.GetLength(0) || c < 0 || c >= Board.GetLength(1))
                    {
                        continue;
                    }
                    // Reset count if this isn't our piece (aside from the new piece's cell).
                    if (
                        Board[r, c] != playerNum
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
