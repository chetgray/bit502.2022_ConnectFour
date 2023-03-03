using System;
using System.Collections.Generic;

using ConnectFour.Business.Models;
using ConnectFour.Business.Models.Interfaces;
using ConnectFour.Data.DTOs;
using ConnectFour.Data.Repositories;
using ConnectFour.Data.Repositories.Interfaces;

namespace ConnectFour.Business.BLLs
{
    public class PlayerBLL
    {
        private IPlayerRepository _repository;

        /// <summary>
        /// Creates a <see cref="RoomBLL"/> instance with a default <see cref="RoomRepository"/>
        /// backend.
        /// </summary>
        public PlayerBLL()
        {
            _repository = new PlayerRepository();
        }

        /// <summary>
        /// Creates a <see cref="RoomBLL"/> instance with the passed <paramref name="repository"/>
        /// as the backend.
        /// </summary>
        /// <param name="repository">The <see cref="IRoomRepository"/> to use as the backend.</param>
        public PlayerBLL(IPlayerRepository repository)
        {
            _repository =
                repository ?? throw new ArgumentNullException(nameof(repository));
        }
        public IPlayerModel AddPlayerToRoom(IPlayerModel model, int roomId)
        {
            PlayerDTO dto = ConvertToDto(model);
            dto.RoomId = roomId;
            dto = _repository.AddPlayerToRoom(dto);
            return ConvertToModel(dto);
        }

        public List<IPlayerModel> GetPlayersInRoom(int roomId)
        {
            List<PlayerDTO> dtos = _repository.GetPlayersInRoom(roomId);
            return ConvertManyToModels(dtos);
        }
        internal PlayerDTO ConvertToDto(IPlayerModel playerModel)
        {
            PlayerDTO pDTO = new PlayerDTO();
            pDTO.Id = playerModel.Id;
            pDTO.Name = playerModel.Name;
            pDTO.Num = playerModel.Num;
            return pDTO;
        }
        internal List<IPlayerModel> ConvertManyToModels(List<PlayerDTO> dtos)
        {
            List<IPlayerModel> models = new List<IPlayerModel>();
            for (int i = 0; i < dtos.Count; i++)
            {
                models.Add(ConvertToModel(dtos[i]));
            }
            return models;
        }
        internal IPlayerModel ConvertToModel(PlayerDTO dto)
        {
            PlayerModel pM = new PlayerModel();
            pM.Id = dto.Id;
            pM.Name = dto.Name;
            pM.Num = dto.Num;
            pM.Symbol = dto.Name.Substring(0, 1).ToUpper();
            pM.Color = (pM.Num == 1) ? ConsoleColor.Red : ConsoleColor.Yellow;
            return pM;
        }
    }
}
