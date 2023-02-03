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
            DataTable dataTable = _dal.ExecuteStoredProcedure("dbo.spA_Room_GetRoomByIdAndNullResult", paramDictionary);
            if(dataTable.Rows.Count > 0)
            {
                return ConvertToDto(dataTable.Rows[0]);
            }
            return new RoomDTO();
        }
        private RoomDTO ConvertToDto(DataRow row)
        {
            RoomDTO roomDTO = new RoomDTO();
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
            return roomDTO;
        }
    }
}
