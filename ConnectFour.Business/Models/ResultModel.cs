using ConnectFour.Business.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectFour.Business.Models
{
    public class ResultModel : IResultModel
    {
        public int? RoomId { get; set; }
        public DateTime CreationTime { get; set; }
        public string Duration { get; set; }
        public string[] Players { get; set; }
        public int? ResultCode { get; set; }
        public string WinnerName { get; set; }
        public string LastTurnNum { get; set; }
    }
}
