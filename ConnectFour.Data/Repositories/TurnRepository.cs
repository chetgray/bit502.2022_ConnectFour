using System;
using System.Collections.Generic;
using System.Data;

using ConnectFour.Data.DALs;
using ConnectFour.Data.DTOs;
using ConnectFour.Data.Repositories.Interfaces;

namespace ConnectFour.Data.Repositories
{
    public class TurnRepository : BaseRepository, ITurnRepository
    {
        /// <inheritdoc cref="BaseRepository()"/>
        public TurnRepository() { }

        /// <inheritdoc/>
        public TurnRepository(IDAL dal) : base(dal) { }

        public TurnDTO GetLastTurnInRoom(int roomId)
        {
            Dictionary<string, object> paramDictionary = new Dictionary<string, object>
            {
                { "@RoomId", roomId }
            };
            DataTable dataTable = _dal.ExecuteStoredProcedure("dbo.spA_Turn_GetLastTurnInRoom", paramDictionary);
            if (dataTable.Rows.Count == 0)
            {
                return null;
            }
            return ConvertToDto(dataTable.Rows[0]);
        }

        public void AddTurnToRoom(TurnDTO dto)
        {
            Dictionary<string, object> paramDictionary = new Dictionary<string, object>
            {
                { "@TurnRoomId", dto.RoomId },
                { "@TurnTime", dto.Time },
                { "@TurnRowNum", dto.RowNum },
                { "@TurnColNum", dto.ColNum },
                { "@TurnNum", dto.Num }
            };

            _dal.ExecuteStoredProcedure("dbo.spA_Turn_AddTurnToRoom", paramDictionary);
        }

        private static TurnDTO ConvertToDto(DataRow row)
        {
            TurnDTO turnDTO = new TurnDTO
            {
                Id = (int?)(row["TurnId"]),
                RoomId = (int)row["TurnRoomId"],
                Time = (DateTime)row["TurnTime"],
                Num = (int)row["TurnNum"],
                RowNum = (int)row["TurnRowNum"],
                ColNum = (int)row["TurnColNum"]
            };

            return turnDTO;
        }
    }
}
