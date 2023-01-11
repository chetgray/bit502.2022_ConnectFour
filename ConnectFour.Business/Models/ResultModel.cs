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
        public DateTime CreationTime { get; set; } = DateTime.Now;
        public List<IPlayerModel> Players { get; set; } = new List<IPlayerModel>();
        public int? ResultCode { get; set; }
        public ITurnModel LastTurn { get; set; }
    }
}
