using ConnectFour.Business.Models.Interfaces;
using System;
using System.Collections.Generic;

namespace ConnectFour.Business.Models.Interfaces
{
    public interface IResultModel
    {
        int? RoomId { get; set; }
        DateTime CreationTime { get; set; }
        string Duration { get; set; }
        string[] Players { get; set; }
        int? ResultCode { get; set; }
        string WinnerName { get; set; }
        string LastTurnNum { get; set; }
    }
}