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

        public IPlayerModel[] Players { get; set; } = new IPlayerModel[2];
        public List<ITurnModel> Turns { get; set; } = new List<ITurnModel>();

        public string[,] Board { get; set; } = new string[6, 7];
        public bool Vacancy { get; set; }

        public string Message { get; set; }

        public int CurrentTurnPlayersNum 
        {
            get
            {
                return (((int)CurrentTurnNum - 1) % 2) + 1;
            }
        }

        public int LocalPlayerNum { get; set; }

        public bool CheckForWin 
        {
            get
            {
                if (Turns.Count > 10)
                {
                    return true;
                }
                return false;
            }
        }
    }
}
