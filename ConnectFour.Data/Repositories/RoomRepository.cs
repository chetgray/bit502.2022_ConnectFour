using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ConnectFour.Data.DTOs;

namespace ConnectFour.Data.Repositories
{
    public class RoomRepository : BaseRepository
    {
        public List<ResultDTO> GetAllFinished()
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            DataTable dataTable = _dal.ExecuteStoredProcedure("dbo.spA_Room_GetAllFinished", parameters);

            Dictionary<int, ResultDTO> resultDTODictionary = new Dictionary<int, ResultDTO>();
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow row = dataTable.Rows[i];
                int roomId = (int)row["RoomId"];
                if (resultDTODictionary.ContainsKey(roomId))
                {
                    resultDTODictionary[roomId].Players.Add(PlayerRepository.ConvertToDto(row));
                    resultDTODictionary[roomId].Players.Sort((x, y) => x.Num.CompareTo(y.Num));
                }
                else
                {
                    resultDTODictionary.Add(roomId, new ResultDTO());
                    resultDTODictionary[roomId].RoomId = roomId;
                    resultDTODictionary[roomId].CreationTime = (DateTime)row["CreationTime"];
                    resultDTODictionary[roomId].ResultCode = (int)row["ResultCode"];
                    resultDTODictionary[roomId].LastTurn = TurnRepository.ConvertToDto(row);
                    resultDTODictionary[roomId].Players.Add(PlayerRepository.ConvertToDto(row));
                }
            }

            foreach (KeyValuePair<int, ResultDTO> item in resultDTODictionary)
            {
                item.Value.Players.Sort((x, y) => x.Num.CompareTo(y.Num));
            }

            List<ResultDTO> resultDTOS = resultDTODictionary.Values.ToList();

            return resultDTOS;
        }
        
        private static RoomDTO ConvertToDto(DataRow row)
        {
            throw new NotImplementedException();
        }
    }
}
