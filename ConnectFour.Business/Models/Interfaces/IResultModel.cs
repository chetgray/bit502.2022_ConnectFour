using System;

namespace ConnectFour.Business.Models.Interfaces
{
    public interface IResultModel
    {
        DateTime CreationTime { get; set; }
        TimeSpan Duration { get; set; }
        int? LastTurnNum { get; set; }
        string[] Players { get; set; }
        int? ResultCode { get; set; }
        int? RoomId { get; set; }
        string WinnerName { get; set; }
    }
}
