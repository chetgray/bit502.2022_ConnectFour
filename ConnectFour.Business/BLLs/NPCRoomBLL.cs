using System;

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
        /// Creates a <see cref="NPCRoomBLL"/> instance with a default <see cref="RoomRepository"/>
        /// backend.
        /// </summary>
        public NPCRoomBLL()
        {
        }

        /// <summary>
        /// Creates a <see cref="NPCRoomBLL"/> instance with the passed <paramref name="repository"/>,
        /// <paramref name="playerBLL"/>, and <paramref name="turnBLL"/> as the backend.
        /// </summary>
        /// <param name="repository">The <see cref="IRoomRepository"/> to use in the backend.</param>
        /// <param name="playerBLL">The <see cref="IPlayerBLL"/> to use in the backend.</param>
        /// <param name="turnBLL">The <see cref="ITurnBLL"/> to use in the backend.</param>
        public NPCRoomBLL(IRoomRepository repository, IPlayerBLL playerBLL, ITurnBLL turnBLL) : base(repository, playerBLL, turnBLL)
        {
        }
        public override IRoomModel LetThemPlay(IRoomModel room)
        {
            int colNum = 1;
            //Get turn (colNum) from Jess's method or could receive a turnModel
            int rowNum = room.GetNextRowInCol(colNum);

            TurnModel turn = new TurnModel
            {
                ColNum = colNum,
                RowNum = rowNum,
                Num = room.CurrentTurnNum
            };
            _turnBLL.AddTurnToRoom(turn, (int)room.Id);

            base.LetThemPlay(room);
            return room;
        }
    }
}
