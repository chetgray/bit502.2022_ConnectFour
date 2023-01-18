using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using ConnectFour.Data.DALs;
using ConnectFour.Data.DTOs;

namespace ConnectFour.Data.Repositories
{
    public class PlayerRepository : BaseRepository
    {
        public PlayerDTO AddPlayerToRoom(PlayerDTO dto)
        {
            Dictionary<string, object> paramDictionary = new Dictionary<string, object>();
            paramDictionary.Add("@PlayerName", dto.Name);
            paramDictionary.Add("@RoomId", dto.RoomId);
            paramDictionary.Add("@PlayerNum", dto.Num);
            DataTable dataTable = _dal.ExecuteStoredProcedure("dbo.spA_Player_AddPlayerToRoom", paramDictionary);
            return ConvertToDto(dataTable.Rows[0]);
        }
        internal PlayerDTO ConvertToDto(DataRow row)
        {
            PlayerDTO dto = new PlayerDTO
            {
                Id = (int)row["PlayerId"],
                Name = (string)row["PlayerName"],
                RoomId = (int)row["PlayerRoomId"],
                Num = (int)row["PlayerNum"]
            };
            return dto;
        }
    }
}
