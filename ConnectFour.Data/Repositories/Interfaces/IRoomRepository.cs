using ConnectFour.Data.DTOs;
using System.Collections.Generic;

namespace ConnectFour.Data.Repositories.Interfaces
{
    public interface IRoomRepository
    {
        List<ResultDTO> GetAllFinished();
        RoomDTO GetRoomById(int roomId);

        void UpdateRoomResultCode(int roomId, int resultCode);
    }
}
