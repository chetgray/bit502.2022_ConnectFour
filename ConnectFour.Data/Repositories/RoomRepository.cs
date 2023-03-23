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

        public RoomDTO GetRoomById(int roomId)
        {
            Dictionary<string, object> paramDictionary = new Dictionary<string, object>();
            paramDictionary.Add("@RoomId", roomId);
            DataTable dataTable = _dal.ExecuteStoredProcedure("dbo.spA_Room_GetRoomById", paramDictionary);
            if (dataTable.Rows.Count == 0)
            {
                return null;
            }
            return ConvertToDto(dataTable.Rows[0]);           
        }

        internal RoomDTO ConvertToDto(DataRow row)
        {
            RoomDTO roomDTO = new RoomDTO();
            if (row.IsNull("RoomId"))
            {
                roomDTO.Id = null;
            }
            else
            {
                roomDTO.Id = (int?)(row["RoomId"]);
            }
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
    }
}
