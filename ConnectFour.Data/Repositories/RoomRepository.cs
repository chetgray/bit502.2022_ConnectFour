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
                    resultDTODictionary.Add(roomId, new ResultDTO
                    {
                        RoomId = roomId,
                        CreationTime = (DateTime)row["CreationTime"],
                        ResultCode = (int)row["ResultCode"],
                        LastTurn = TurnRepository.ConvertToDto(row),
                        Players = new List<PlayerDTO>
                        {
                            PlayerRepository.ConvertToDto(row)
                        }
                    });
                }
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
