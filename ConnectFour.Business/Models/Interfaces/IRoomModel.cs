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
    }
}
