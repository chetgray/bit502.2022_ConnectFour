using ConnectFour.Business.Models.Interfaces;

namespace ConnectFour.Business.BLLs.Interfaces
{
    public interface ITurnBLL
    {
        void AddTurnToRoom(ITurnModel turn, int roomId);
        ITurnModel GetLatestTurnInRoom(int roomId);
    }
}
