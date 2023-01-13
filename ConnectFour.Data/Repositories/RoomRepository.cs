using System;
using System.Collections.Generic;
using System.Data;

using ConnectFour.Data.DTOs;

namespace ConnectFour.Data.Repositories
{
    public class RoomRepository : BaseRepository
    {
        public RoomDTO GetRoomOccupancy(int roomId)
        {
            Dictionary<string, object> paramDictionary = new Dictionary<string, object>();
            paramDictionary.Add("@RoomId", roomId);
            DataTable dataTable = _dal.ExecuteStoredProcedure("dbo.spA_Room_GetRoomOccupancy", paramDictionary);
            return ConvertToDto(dataTable);
        }
        private static RoomDTO ConvertToDto(DataTable dataTable)
        {
            RoomDTO roomDTO = new RoomDTO();
            for (int i = 0; i < dataTable.Rows.Count; i++)
            { 
                roomDTO.Id = (int?)(dataTable.Rows[i]["RoomId"]);
                roomDTO.CreationTime = (DateTime)dataTable.Rows[i]["CreationTime"];
                roomDTO.CurrentTurnNumber = (int?)dataTable.Rows[i]["CurrentTurnNum"];
                bool resultCodeSuccess = int.TryParse(dataTable.Rows[i]["ResultCode"].ToString(), out int result);
                roomDTO.ResultCode = (resultCodeSuccess) ? roomDTO.ResultCode = result : roomDTO.ResultCode = null;
                roomDTO.Players.Add(PlayerRepository.ConvertToDto(dataTable.Rows[i]));
            }
            return roomDTO;
        }
    }
}
