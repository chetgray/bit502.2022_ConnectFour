using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
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

            DataRow row = dataTable.Rows[0];
            roomDTO.Id = (int?)(row["RoomId"]);
            roomDTO.CreationTime = (DateTime)row["RoomCreationTime"];
            if (row.IsNull("RoomCurrentTurnNum"))
            {
                roomDTO.CurrentTurnNumber = null;
            }
            else
            {
                roomDTO.CurrentTurnNumber = (int?)row["RoomCurrentTurnNum"];
            }
            if (row.IsNull("RoomResultCode"))
            {
                roomDTO.ResultCode = null;
            }
            else
            {
                roomDTO.ResultCode = (int?)row["RoomResultCode"];
            }

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                roomDTO.Players.Add(pRepo.ConvertToDto(dataTable.Rows[i]));
            }        
            
            return roomDTO;
        }
    }
}
