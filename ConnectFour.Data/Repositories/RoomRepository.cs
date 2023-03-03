using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

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
            DataTable resultTable = _dal.ExecuteStoredProcedure(
                "dbo.spA_Room_GetAllFinished",
                parameters
            );

            return ConvertTableToResultDtos(resultTable).ToList();
        }

        private static IEnumerable<ResultDTO> ConvertTableToResultDtos(DataTable table)
        {
            Dictionary<int, ResultDTO> resultDtos = new Dictionary<int, ResultDTO>();
            foreach (DataRow row in table.Rows)
            {
                int roomId = (int)row["RoomId"];
                if (!resultDtos.ContainsKey(roomId))
                {
                    resultDtos.Add(
                        roomId,
                        new ResultDTO
                        {
                            RoomId = roomId,
                            CreationTime = (DateTime)row["RoomCreationTime"],
                            ResultCode = (int)row["RoomResultCode"],
                            LastTurnTime = (DateTime)row["TurnTime"],
                            LastTurnNum = (int)row["TurnNum"]
                        }
                    );
                }
                resultDtos[roomId].Players.Add(
                    (int)row["PlayerNum"],
                    row["PlayerName"].ToString()
                );
            }

            return resultDtos.Values;
        }

        internal RoomDTO ConvertToDto(DataRow row)
        {
            throw new NotImplementedException();
        }
    }
}
