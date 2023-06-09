﻿using System;
using System.Collections.Generic;

using ConnectFour.Business.Models.Interfaces;

namespace ConnectFour.Business.Models
{
    public class RoomModel : IRoomModel
    {
        public int? Id { get; set; }

        public int[,] Board { get; set; } = new int[6, 7];
        public DateTime CreationTime { get; set; } = DateTime.Now;
        public int CurrentPlayerNum => DeterminePlayerNum(CurrentTurnNum);
        public int CurrentTurnNum { get; set; }
        public int LocalPlayerNum { get; set; }
        public string Message { get; set; }
        public IPlayerModel[] Players { get; set; } = new IPlayerModel[2];
        public int? ResultCode { get; set; }
        public List<ITurnModel> Turns { get; set; } = new List<ITurnModel>();
        public bool Vacancy { get; set; }

        public int DeterminePlayerNum(int turnNum)
        {
            return ((turnNum - 1) % Players.Length) + 1;
        }

        public int GetNextRowInCol(int colNum)
        {
            if (colNum < 1 || colNum > Board.GetLength(1))
            {
                throw new ArgumentException(
                    $"Please choose a column between 1 - {Board.GetLength(1)}"
                );
            }
            int turnsInColumnCount = 0;
            foreach (ITurnModel turn in Turns)
            {
                if (turn.ColNum == colNum)
                {
                    turnsInColumnCount++;
                }
            }
            int rowNum = Board.GetLength(0) - turnsInColumnCount;
            if (rowNum < 1)
            {
                throw new ArgumentException($"Column {colNum} is Full!");
            }

            return rowNum;
        }

        public bool WillTurnWin(ITurnModel turn)
        {
            // Bail if the turn's cell is already occupied.
            if (Board[turn.RowNum - 1, turn.ColNum - 1] != 0)
            {
                return false;
            }

            int playerNum = DeterminePlayerNum(turn.Num);

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
