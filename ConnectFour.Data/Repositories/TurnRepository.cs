using System;
using System.Data;

using ConnectFour.Data.DTOs;

namespace ConnectFour.Data.Repositories
{
    public class TurnRepository : BaseRepository
    {
        internal static TurnDTO ConvertToDto(DataRow row)
        {
            TurnDTO dto = new TurnDTO
            {
                Id = (int)row["TurnId"],
                Time = (DateTime)row["Time"],
                RowNum = (int)row["RowNum"],
                ColNum = (int)row["ColNum"],
                RoomId = (int)row["TurnRoomId"],
                Num = (int)row["TurnNum"]
            };
            return dto;
        }
    }
}
