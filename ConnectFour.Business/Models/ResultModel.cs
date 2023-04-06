using System;

using ConnectFour.Business.Models.Interfaces;

namespace ConnectFour.Business.Models
{
    public class ResultModel : IResultModel
    {
        public int? RoomId { get; set; }

        public DateTime CreationTime { get; set; }
        public TimeSpan Duration { get; set; }
        public int? LastTurnNum { get; set; }
        public string[] Players { get; set; }
        public int? ResultCode { get; set; }
        public string WinnerName { get; set; }
    }
}
