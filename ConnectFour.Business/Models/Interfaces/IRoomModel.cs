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

        string[,] Board { get; set; }
        bool Vacancy { get; set; }
    }
}