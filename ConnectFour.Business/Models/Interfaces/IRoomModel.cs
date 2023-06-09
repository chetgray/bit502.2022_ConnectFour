﻿using System;
using System.Collections.Generic;

namespace ConnectFour.Business.Models.Interfaces
{
    public interface IRoomModel
    {
        int? Id { get; set; }

        /// <summary>
        /// Gets or sets the representation of the board grid's current state.
        /// </summary>
        /// <value>
        /// A two-dimensional array representing the pieces in each cell.
        /// </value>
        int[,] Board { get; set; }
        DateTime CreationTime { get; set; }
        int CurrentPlayerNum { get; }
        int CurrentTurnNum { get; set; }
        int LocalPlayerNum { get; set; }
        string Message { get; set; }
        IPlayerModel[] Players { get; set; }
        int? ResultCode { get; set; }
        List<ITurnModel> Turns { get; }
        bool Vacancy { get; set; }

        int DeterminePlayerNum(int turnNum);

        /// <summary>
        /// Gets the next row in the given column.
        /// </summary>
        /// <param name="colNum">The column number (1-based).</param>
        /// <returns>The row number of the next row in the given column.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown if the given <paramref name="colNum"/> is less than 1 or greater than 7, or
        /// if the column is already full.
        /// </exception>
        int GetNextRowInCol(int colNum);

        /// <summary>
        /// Checks if the given <paramref name="turn"/> will result in a win.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if the <paramref name="turn"/> will result in a win, otherwise <see langword="false"/>.
        /// </returns>
        /// <param name="turn">The <see cref="ITurnModel"/> to check.</param>
        bool WillTurnWin(ITurnModel turn);
    }
}
