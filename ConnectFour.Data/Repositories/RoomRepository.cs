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
        private RoomDTO ConvertToDto(DataTable dataTable)
        {
            RoomDTO roomDTO = new RoomDTO();
            if(dataTable.Rows.Count == 0)
            {
                return roomDTO;
            }
            PlayerRepository pRepo = new PlayerRepository();

            roomDTO.Id = (int?)(dataTable.Rows[0]["RoomId"]);
            roomDTO.CreationTime = (DateTime)dataTable.Rows[0]["RoomCreationTime"];
            roomDTO.CurrentTurnNumber = (int?)dataTable.Rows[0]["RoomCurrentTurnNum"];

            if (DBNull.Value.Equals(dataTable.Rows[0]["RoomResultCode"]))
            {
                roomDTO.ResultCode = null;
            }
            else
            {
                roomDTO.ResultCode = (int)dataTable.Rows[0]["RoomResultCode"];
            }

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                roomDTO.Players.Add(pRepo.ConvertToDto(dataTable.Rows[i]));
            }        
            
            return roomDTO;
        }
    }
}
