using System.Collections.Generic;

using ConnectFour.Data.DTOs;

namespace ConnectFour.Data.Repositories.Interfaces
{
    public interface IRoomRepository
    {
        int InsertNewRoom();
        List<ResultDTO> GetAllFinished();
        RoomDTO GetRoomById(int roomId);

        void UpdateRoomResultCode(int roomId, int resultCode);
    }
}
