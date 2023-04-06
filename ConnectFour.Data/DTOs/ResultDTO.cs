using System;
using System.Collections.Generic;

namespace ConnectFour.Data.DTOs
{
    public class ResultDTO
    {
        public DateTime CreationTime { get; set; }
        public int? LastTurnNum { get; set; }
        public DateTime? LastTurnTime { get; set; }
        public Dictionary<int, string> Players { get; set; } = new Dictionary<int, string>();
        public int? ResultCode { get; set; }
        public int? RoomId { get; set; }
    }
}
