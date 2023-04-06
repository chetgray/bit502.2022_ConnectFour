using System;

namespace ConnectFour.Data.DTOs
{
    public class TurnDTO
    {
        public int ColNum { get; set; }
        public int? Id { get; set; }
        public int Num { get; set; }
        public int RoomId { get; set; }
        public int RowNum { get; set; }
        public DateTime Time { get; set; }
    }
}
