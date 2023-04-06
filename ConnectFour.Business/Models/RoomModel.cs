using System;
using System.Collections.Generic;

using ConnectFour.Business.Models.Interfaces;

namespace ConnectFour.Business.Models
{
    public class RoomModel : IRoomModel
    {
        public int? Id { get; set; }

        public int[,] Board { get; set; } = new int[6, 7];
        public DateTime CreationTime { get; set; } = DateTime.Now;
        public int CurrentPlayerNum => DeterminePlayerNum(CurrentTurnNum);
        public int CurrentTurnNum { get; set; }
        public int LocalPlayerNum { get; set; }
        public string Message { get; set; }
        public IPlayerModel[] Players { get; set; } = new IPlayerModel[2];
        public int? ResultCode { get; set; }
        public List<ITurnModel> Turns { get; set; } = new List<ITurnModel>();
        public bool Vacancy { get; set; }

        public int DeterminePlayerNum(int turnNum)
        {
            return ((turnNum - 1) % Players.Length) + 1;
        }
    }
}
