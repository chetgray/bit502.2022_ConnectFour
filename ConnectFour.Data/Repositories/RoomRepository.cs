using System;
using System.Data;

using ConnectFour.Data.DTOs;
using ConnectFour.Data.Repositories.Interfaces;

namespace ConnectFour.Data.Repositories
{
    public class RoomRepository : BaseRepository, IRoomRepository
    {
        private static RoomDTO ConvertToDto(DataRow row)
        {
            throw new NotImplementedException();
        }
    }
}
