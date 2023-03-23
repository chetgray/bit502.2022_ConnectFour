using ConnectFour.Business.Models.Interfaces;

namespace ConnectFour.Business.BLLs.Interfaces
{
    public interface IPlayerBLL
    {
        IPlayerModel AddPlayerToRoom(IPlayerModel model, int roomId);
        IPlayerModel[] GetPlayersInRoom(int roomId);
    }
}
