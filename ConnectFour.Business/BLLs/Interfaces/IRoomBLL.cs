using System.Collections.Generic;

using ConnectFour.Business.Models.Interfaces;

namespace ConnectFour.Business.BLLs.Interfaces
{
    public interface IRoomBLL
    {
        int InsertNewRoom();
        IRoomModel AddPlayerToRoom(string localPlayerName, int roomId);

        IRoomModel AddTurnToRoom(int colNum, IRoomModel room);

        List<IResultModel> GetAllFinished();

        IRoomModel UpdateWithLastTurn(IRoomModel roomModel);

        /// <summary>
        /// Gets a <see cref="IRoomModel"/> by its <paramref name="roomId"/>.
        /// </summary>
        /// <param name="roomId">
        /// The ID of the room to get.
        /// </param>
        /// <returns>
        /// A <see cref="IRoomModel"/> with the passed <paramref name="roomId"/>, or <see
        /// langword="null"/> if no room with that ID exists.
        /// </returns>
        IRoomModel GetRoomById(int roomId);

        IRoomModel LetThemPlay(IRoomModel roomModel);
    }
}
