using ConnectFour.Data.DTOs;

namespace ConnectFour.Data.Repositories.Interfaces
{
    public interface ITurnRepository
    {
        TurnDTO GetLastTurnInRoom(int roomId);
        void AddTurnToRoom(TurnDTO dto);
    }
}
