using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectFour.Data.DTOs
{
    public class ResultDTO
    {
        public int? RoomId { get; set; }
        public DateTime CreationTime { get; set; }
        public Dictionary<int, string> Players { get; set; } = new Dictionary<int, string>();
        public int? ResultCode { get; set; }
        public DateTime LastTurnTime { get; set; }
        public int? LastTurnNum { get; set; }
    }
}
