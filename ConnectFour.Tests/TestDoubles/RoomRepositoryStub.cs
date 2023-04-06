using System.Collections.Generic;

using ConnectFour.Data.DTOs;
using ConnectFour.Data.Repositories.Interfaces;

namespace ConnectFour.Tests.TestDoubles
{
    internal class RoomRepositoryStub : IRoomRepository
    {
        public RoomDTO TestDto { get; set; }
        public List<ResultDTO> TestResults { get; set; }

        public int AddNewRoom()
        {
            throw new System.NotImplementedException();
        }

        public List<ResultDTO> GetAllFinishedResults()
        {
            return TestResults;
        }

        public RoomDTO GetRoomById(int roomId)
        {
            return TestDto;
        }

        public void SetRoomResultCode(int roomId, int resultCode) { }
    }
}
