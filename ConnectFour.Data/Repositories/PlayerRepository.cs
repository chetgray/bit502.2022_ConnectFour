﻿using System.Collections.Generic;
using System.Data;

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
        public List<PlayerDTO> GetPlayersInRoom(int roomId)
        {
            Dictionary<string, object> paramDictionary = new Dictionary<string, object>();
            paramDictionary.Add("@RoomId", roomId);
            DataTable dataTable = _dal.ExecuteStoredProcedure("dbo.spA_Player_GetPlayersInRoom", paramDictionary);
            return ConvertManyToDTOs(dataTable);
        }
        internal List<PlayerDTO> ConvertManyToDTOs(DataTable dataTable)
        {
            List<PlayerDTO> dtos = new List<PlayerDTO>();
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                dtos.Add(ConvertToDto(dataTable.Rows[i]));
            }
            return dtos;
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
