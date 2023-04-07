using System;
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
        int CurrentTurnNum { get; set; }
        int LocalPlayerNum { get; set; }
        string Message { get; set; }
        IPlayerModel[] Players { get; set; }
        int? ResultCode { get; set; }
        List<ITurnModel> Turns { get; }
        bool Vacancy { get; set; }
    }
}
