using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using ConnectFour.Data.DTOs;

namespace ConnectFour.Data.Repositories
{
    public class RoomRepository : BaseRepository
    {
        public int InsertNewRoom()
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            DataTable dataTable = _dal.ExecuteStoredProcedure("spA_Room_InsertNewRoom", parameters);

            return Convert.ToInt32(dataTable.Rows[0][0]);
        }
        public List<ResultDTO> GetAllFinished()
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            DataTable dataTable = _dal.ExecuteStoredProcedure("dbo.spA_Room_GetAllFinished", parameters);

            Dictionary<int, ResultDTO> resultDTODictionary = new Dictionary<int, ResultDTO>();
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow row = dataTable.Rows[i];
                resultDTODictionary = ConvertToDto(row, resultDTODictionary);
            }

            List<ResultDTO> resultDTOS = resultDTODictionary.Values.ToList();
            return resultDTOS;
        }
        private static Dictionary<int, ResultDTO> ConvertToDto(DataRow row, Dictionary<int, ResultDTO> resultDTODictionary)
        {
            int roomId = (int)row["RoomId"];
            if (resultDTODictionary.ContainsKey(roomId))
            {
                resultDTODictionary[roomId].Players.Add((int)row["PlayerNum"], row["PlayerName"].ToString());
            }
            else
            {
                resultDTODictionary.Add(roomId, new ResultDTO
                {
                    RoomId = roomId,
                    CreationTime = (DateTime)row["RoomCreationTime"],
                    ResultCode = (int)row["RoomResultCode"],
                    LastTurnTime = (DateTime)row["TurnTime"],
                    LastTurnNum = (int)row["TurnNum"]
                });
                resultDTODictionary[roomId].Players.Add((int)row["PlayerNum"], row["PlayerName"].ToString());
            }
            return resultDTODictionary;
        }
        private static RoomDTO ConvertToDto(DataRow row)
        {
            throw new NotImplementedException();
        }
    }
}
