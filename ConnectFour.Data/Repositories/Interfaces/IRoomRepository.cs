using System.Collections.Generic;

using ConnectFour.Data.DTOs;

namespace ConnectFour.Data.Repositories.Interfaces
{
    public interface IRoomRepository
    {
        int AddNewRoom();
        List<ResultDTO> GetAllFinishedResults();
        RoomDTO GetRoomById(int roomId);
        void SetRoomResultCode(int roomId, int resultCode);
    }
}
