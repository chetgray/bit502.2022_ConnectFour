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

        public List<IPlayerModel> Players { get; set; } = new List<IPlayerModel>();
        public List<ITurnModel> Turns { get; set; } = new List<ITurnModel>();

        public int[,] Board { get; set; } = new int[6, 7];

        public bool CheckForWin(ITurnModel turn)
        {
            int playerNum = (turn.Num - 1) % Players.Count + 1;
            int count;

            // Check up to three cells to the left and three cells to the right of the Turn's column
            // for four in a row.
            count = 0;
            for (
                int c = Math.Max(0, turn.ColNum - 1 - 3);
                c < Math.Min(turn.ColNum - 1 + 3, Board.GetLength(1));
                c++
            )
            {
                if (Board[turn.RowNum - 1, c - 1] != playerNum)
                {
                    count = 0;
                    if (c == turn.ColNum - 1)
                    {
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }
                count++;
                if (count == 4)
                {
                    return true;
                }
            }

            // Check up to three cells below and three cells above the Turn's row for four in a row.
            count = 0;
            for (
                int r = Math.Min(turn.RowNum - 1 + 3, Board.GetLength(0));
                r > Math.Max(0, turn.RowNum - 1 - 3);
                r--
            )
            {
                if (Board[r - 1, turn.ColNum - 1] != playerNum)
                {
                    count = 0;
                    if (r == turn.RowNum - 1)
                    {
                        break;
                    }
                    else
                    {
                        continue;
                    }
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
