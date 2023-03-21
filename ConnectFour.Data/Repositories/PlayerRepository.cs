using System.Collections.Generic;
using System.Data;

using ConnectFour.Data.DALs;
using ConnectFour.Data.DTOs;
using ConnectFour.Data.Repositories.Interfaces;

namespace ConnectFour.Data.Repositories
{
    public class PlayerRepository : BaseRepository, IPlayerRepository
    {
        /// <inheritdoc cref="BaseRepository()"/>
        public PlayerRepository() { }

        /// <inheritdoc/>
        public PlayerRepository(IDAL dal) : base(dal) { }

        public PlayerDTO AddPlayerToRoom(PlayerDTO dto)
        {
            Dictionary<string, object> paramDictionary = new Dictionary<string, object>();
            paramDictionary.Add("@PlayerName", dto.Name);
            paramDictionary.Add("@RoomId", dto.RoomId);
            paramDictionary.Add("@PlayerNum", dto.Num);
            DataTable dataTable = _dal.ExecuteStoredProcedure("dbo.spA_Player_AddPlayerToRoom", paramDictionary);
            return ConvertToDto(dataTable.Rows[0]);
        }

        public PlayerDTO[] GetPlayersInRoom(int roomId)
        {
            Dictionary<string, object> paramDictionary = new Dictionary<string, object>();
            paramDictionary.Add("@RoomId", roomId);
            DataTable dataTable = _dal.ExecuteStoredProcedure(
                "dbo.spA_Player_GetPlayersInRoom",
                paramDictionary
            );
            PlayerDTO[] dtos = new PlayerDTO[2];
            foreach (PlayerDTO dto in ConvertManyToDTOs(dataTable))
            {
                dtos[dto.Num - 1] = dto;
            }
            return dtos;
        }

        internal List<PlayerDTO> ConvertManyToDTOs(DataTable dataTable)
        {
            List<PlayerDTO> dtos = new List<PlayerDTO>();
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                dtos.Add(ConvertToDto(dataTable.Rows[i]));
            }
            return dtos;
        }
        internal PlayerDTO ConvertToDto(DataRow row)
        {
            PlayerDTO dto = new PlayerDTO
            {
                Id = (int)row["PlayerId"],
                Name = (string)row["PlayerName"],
                RoomId = (int)row["PlayerRoomId"],
                Num = (int)row["PlayerNum"]
            };
            return dto;
        }
    }
}
