using ConnectFour.Data.DTOs;
using System.Collections.Generic;

namespace ConnectFour.Data.Repositories.Interfaces
{
    public interface IPlayerRepository
    {
        PlayerDTO AddPlayerToRoom(PlayerDTO dto);
        List<PlayerDTO> GetPlayersInRoom(int roomId);
    }
}
