using ConnectFour.Business.Models.Interfaces;

namespace ConnectFour.Business.BLLs.Interfaces
{
    public interface ITurnBLL
    {
        ITurnModel GetLastTurnInRoom(int roomId);

        void AddTurnToRoom(ITurnModel turn, int roomId);
    }
}
