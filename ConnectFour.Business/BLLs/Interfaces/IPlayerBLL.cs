using System.Collections.Generic;

using ConnectFour.Business.Models.Interfaces;

namespace ConnectFour.Business.BLLs.Interfaces
{
    public interface IPlayerBLL
    {
        IPlayerModel AddPlayerToRoom(IPlayerModel model, int roomId);
        List<IPlayerModel> GetPlayersInRoom(int roomId);
    }
}
