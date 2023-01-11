using ConnectFour.Business.Models.Interfaces;
using System;
using System.Collections.Generic;

namespace ConnectFour.Business.Models.Interfaces
{
    public interface IResultModel
    {
        DateTime CreationTime { get; set; }
        List<IPlayerModel> Players { get; set; }
        int? ResultCode { get; set; }
        int? RoomId { get; set; }
        ITurnModel LastTurn { get; set; }
    }
}