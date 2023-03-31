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
            ITurnModel turn = RandyTakesATurn(room);
            room = AddTurnToRoom(turn, room);
            room.Message = "Where would you like to place a piece?";
            return room;
        }

        public ITurnModel RandyTakesATurn(IRoomModel room)
        {
            throw new NotImplementedException();
        }
    }
}
