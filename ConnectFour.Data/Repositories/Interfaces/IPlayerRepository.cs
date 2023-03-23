using ConnectFour.Data.DTOs;

namespace ConnectFour.Data.Repositories.Interfaces
{
    public interface IPlayerRepository
    {
        PlayerDTO AddPlayerToRoom(PlayerDTO dto);
        PlayerDTO[] GetPlayersInRoom(int roomId);
    }
}
