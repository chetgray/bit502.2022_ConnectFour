using ConnectFour.Data.DTOs;
using System.Collections.Generic;

namespace ConnectFour.Data.Repositories.Interfaces
{
    public interface IRoomRepository
    {
        int InsertNewRoom();
        List<ResultDTO> GetAllFinished();
        RoomDTO GetRoomById(int roomId);
    }
}
