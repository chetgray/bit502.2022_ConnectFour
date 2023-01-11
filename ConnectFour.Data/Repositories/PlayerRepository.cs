using System;
using System.Data;

using ConnectFour.Data.DTOs;

namespace ConnectFour.Data.Repositories
{
    public class PlayerRepository : BaseRepository
    {
        internal static PlayerDTO ConvertToDto(DataRow row)
        {
            PlayerDTO dto = new PlayerDTO
            {
                Id = (int)row["PlayerId"],
                Name = (string)row["Name"],
                RoomId = (int)row["PlayerRoomId"],
                Num = (int)row["PlayerRoomId"]
            };
            return dto;
        }
    }
}
