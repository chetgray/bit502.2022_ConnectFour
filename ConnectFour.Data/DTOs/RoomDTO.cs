using System;
using System.Collections.Generic;

namespace ConnectFour.Data.DTOs
{
    public class RoomDTO
    {
        public int? Id { get; set; }
        public DateTime CreationTime { get; set; }
        public int? CurrentTurnNumber { get; set; }
        public int? ResultCode { get; set; }
    }
}
