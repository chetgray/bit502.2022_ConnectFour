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
        public List<PlayerDTO> Players { get; set; } = new List<PlayerDTO>();
        public int? ResultCode { get; set; }
        public TurnDTO LastTurn { get; set; } = new TurnDTO();
    }
}
