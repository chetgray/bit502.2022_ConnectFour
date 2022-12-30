using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectFour.Data.DTOs
{
    public class ResultDTO
    {
        public DateTime CreationTime { get; set; }
        public int? CurrentTurnNum { get; set; }
        public List<PlayerDTO> Players { get; set; }
        public int? ResultCode { get; set; }
        public int? RoomId { get; set; }
        public List<TurnDTO> Turns { get; set; }
    }
}
