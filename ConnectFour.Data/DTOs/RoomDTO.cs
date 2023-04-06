using System;

namespace ConnectFour.Data.DTOs
{
    public class RoomDTO
    {
        public DateTime CreationTime { get; set; }
        public int CurrentTurnNumber { get; set; }
        public int? Id { get; set; }
        public int? ResultCode { get; set; }
    }
}
