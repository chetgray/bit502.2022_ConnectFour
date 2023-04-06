using ConnectFour.Data.DTOs;

namespace ConnectFour.Data.Repositories.Interfaces
{
    public interface ITurnRepository
    {
        void AddTurnToRoom(TurnDTO dto);
        TurnDTO GetLatestTurnInRoom(int roomId);
    }
}
