using System.Collections.Generic;

using ConnectFour.Data.DTOs;
using ConnectFour.Data.Repositories.Interfaces;

namespace ConnectFour.Tests.TestDoubles
{
    internal class RoomRepositoryStub : IRoomRepository
    {
        public RoomDTO TestDto { get; set; }
        public List<ResultDTO> TestResults { get; set; }

        public List<ResultDTO> GetAllFinished()
        {
            return TestResults;
        }

        public RoomDTO GetRoomById(int roomId)
        {
            return TestDto;
        }

        public int InsertNewRoom()
        {
            throw new System.NotImplementedException();
        }

        public void UpdateRoomResultCode(int roomId, int resultCode)
        {
        }
    }
}
