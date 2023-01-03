using System;
using System.Collections.Generic;

namespace ConnectFour.Business.Models.Interfaces
{
    public interface IRoomModel
    {
        int? Id { get; set; }
        DateTime CreationTime { get; set; }
        int? CurrentTurnNum { get; set; }
        int? ResultCode { get; set; }

        List<IPlayerModel> Players { get; }
        List<ITurnModel> Turns { get; }

        /// <summary>
        /// Gets or sets the representation of the board grid's current state.
        /// </summary>
        /// <value>
        /// A two-dimensional array representing the pieces in each cell.
        /// </value>
        int[,] Board { get; set; }

        /// <summary>
        /// Checks if the given <paramref name="turn"/> will result in a win.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if the <paramref name="turn"/> will result in a win, otherwise <see langword="false"/>.
        /// </returns>
        /// <param name="turn">The <see cref="ITurnModel"/> to check.</param>
        bool CheckForWin(ITurnModel turn);
    }
}
