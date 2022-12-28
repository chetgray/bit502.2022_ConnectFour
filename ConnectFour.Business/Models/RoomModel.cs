using System;
using System.Collections.Generic;

using ConnectFour.Business.Models.Interfaces;

namespace ConnectFour.Business.Models
{
    public class RoomModel : IRoomModel
    {
        public int? Id { get; set; }
        public DateTime CreationTime { get; set; } = DateTime.Now;
        public int? CurrentTurnNum { get; set; }
        public int? ResultCode { get; set; }

        public List<IPlayerModel> Players { get; set; } = new List<IPlayerModel>();
        public List<ITurnModel> Turns { get; set; } = new List<ITurnModel>();

        public string[,] Board { get; set; } = new string[6, 7];
    }
}
