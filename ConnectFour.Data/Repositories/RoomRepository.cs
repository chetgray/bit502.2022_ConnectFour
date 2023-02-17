using System;
using System.Data;

using ConnectFour.Data.DALs;
using ConnectFour.Data.DTOs;
using ConnectFour.Data.Repositories.Interfaces;

namespace ConnectFour.Data.Repositories
{
    public class RoomRepository : BaseRepository, IRoomRepository
    {
        /// <inheritdoc cref="BaseRepository()"/>
        public RoomRepository() { }

        /// <inheritdoc/>
        public RoomRepository(IDAL dal) : base(dal) { }

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
                int result = (int)row["RoomResultCode"];
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

        internal RoomDTO ConvertToDto(DataRow row)
        {
            throw new NotImplementedException();
        }
    }
}
