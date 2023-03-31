using System;
using System.Collections.Generic;
using System.Linq;
using ConnectFour.Business.BLLs.Interfaces;
using ConnectFour.Business.Models;
using ConnectFour.Business.Models.Interfaces;
using ConnectFour.Data.Repositories;
using ConnectFour.Data.Repositories.Interfaces;

namespace ConnectFour.Business.BLLs
{
    public class NPCRoomBLL : RoomBLL, IRoomBLL
    {
        /// <summary>
        /// Creates an <see cref="NPCRoomBLL"/> instance with a default <see cref="RoomRepository"/>
        /// backend.
        /// </summary>
        public NPCRoomBLL()
        {
        }

        /// <summary>
        /// Creates an <see cref="NPCRoomBLL"/> instance with the passed <paramref name="repository"/>,
        /// <paramref name="playerBLL"/>, and <paramref name="turnBLL"/> as the backend.
        /// </summary>
        /// <param name="repository">The <see cref="IRoomRepository"/> to use in the backend.</param>
        /// <param name="playerBLL">The <see cref="IPlayerBLL"/> to use in the backend.</param>
        /// <param name="turnBLL">The <see cref="ITurnBLL"/> to use in the backend.</param>
        public NPCRoomBLL(IRoomRepository repository, IPlayerBLL playerBLL, ITurnBLL turnBLL) : base(repository, playerBLL, turnBLL)
        {
        }
        public override IRoomModel LetThemPlay(IRoomModel roomModel)
        {
            //Call logic here to handle computers turn.
            throw new NotImplementedException();
        }

        public List<(int, int)> GetValidPlays(IRoomModel room)
        {
            List<(int, int)> validPlays = new List<(int, int)>();

            for (int i = 1; i >= 7; i++)
            {
                try
                {
                    int rowNum = room.GetNextRowInCol(i);
                    validPlays.Add((rowNum, i));
                }
                catch (ArgumentException)
                {
                    continue;
                }
            }

            return validPlays;
        }

        public TurnModel RandyTakesATurn(IRoomModel room)
        {
            List<(int, int)> validPlays = GetValidPlays(room);
            Random random = new Random();
            (int rowNum, int colNum) play = validPlays.ElementAt(random.Next(0, validPlays.Count));
            TurnModel turn = new TurnModel
            {
                ColNum = play.colNum,
                RowNum = play.rowNum,
                Num = room.CurrentTurnNum
            };

            return turn;
        }
    }
}