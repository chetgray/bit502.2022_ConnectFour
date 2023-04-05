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

        public int InsertNewRoom()
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            int roomId = Convert.ToInt32(
                _dal.GetValueFromStoredProcedure("spA_Room_InsertNewRoom", parameters)
            );

            return roomId;
        }

        public List<ResultDTO> GetAllFinished()
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            DataTable resultTable = _dal.GetTableFromStoredProcedure(
                "dbo.spA_Room_GetAllFinished",
                parameters
            );

            return ConvertTableToResultDtos(resultTable).ToList();
        }

        public RoomDTO GetRoomById(int roomId)
        {
            Dictionary<string, object> paramDictionary = new Dictionary<string, object>
            {
                { "@RoomId", roomId }
            };
            DataTable dataTable = _dal.GetTableFromStoredProcedure(
                "dbo.spA_Room_GetRoomById",
                paramDictionary
            );
            if (dataTable.Rows.Count == 0)
            {
                return null;
            }
            return ConvertToDto(dataTable.Rows[0]);
        }

        public void UpdateRoomResultCode(int roomId, int resultCode)
        {
            Dictionary<string, object> paramDictionary = new Dictionary<string, object>
            {
                { "@RoomId", roomId },
                { "@RoomResultCode", resultCode }
            };

            _dal.ExecuteStoredProcedure("dbo.spA_Room_UpdateRoomResultCode", paramDictionary);
        }

        internal RoomDTO ConvertToDto(DataRow row)
        {
            RoomDTO roomDto = new RoomDTO()
            {
                Id = row.IsNull("RoomId") ? null : (int?)(row["RoomId"]),
                CreationTime = (DateTime)row["RoomCreationTime"],
                CurrentTurnNumber = (int)row["RoomCurrentTurnNum"],
                ResultCode = row.IsNull("RoomResultCode") ? null : (int?)row["RoomResultCode"]
            };
            return roomDto;
        }

        internal static IEnumerable<ResultDTO> ConvertTableToResultDtos(DataTable table)
        {
            Dictionary<int, ResultDTO> resultDtos = new Dictionary<int, ResultDTO>();
            foreach (DataRow row in table.Rows)
            {
                int roomId = (int)row["RoomId"];
                if (!resultDtos.ContainsKey(roomId))
                {
                    ResultDTO resultDto = new ResultDTO
                    {
                        RoomId = roomId,
                        CreationTime = (DateTime)row["RoomCreationTime"],
                        ResultCode = (int)row["RoomResultCode"]
                    };
                    if (!row.IsNull("TurnTime"))
                    {
                        resultDto.LastTurnTime = (DateTime)row["TurnTime"];
                    }
                    if (!row.IsNull("TurnNum"))
                    {
                        resultDto.LastTurnNum = (int)row["TurnNum"];
                    }
                    resultDtos.Add(roomId, resultDto);
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
