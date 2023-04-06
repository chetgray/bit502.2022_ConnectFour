using System;
using System.Collections.Generic;

using ConnectFour.Business.BLLs.Interfaces;
using ConnectFour.Business.Models;
using ConnectFour.Business.Models.Interfaces;
using ConnectFour.Data.DTOs;
using ConnectFour.Data.Repositories;
using ConnectFour.Data.Repositories.Interfaces;

namespace ConnectFour.Business.BLLs
{
    public class PlayerBLL : IPlayerBLL
    {
        private readonly IPlayerRepository _repository;

        /// <summary>
        /// Creates a <see cref="PlayerBLL"/> instance with a default <see cref="PlayerRepository"/>
        /// backend.
        /// </summary>
        public PlayerBLL()
        {
            _repository = new PlayerRepository();
        }

        /// <summary>
        /// Creates a <see cref="PlayerBLL"/> instance with the passed <paramref name="repository"/>
        /// as the backend.
        /// </summary>
        /// <param name="repository">The <see cref="IPlayerRepository"/> to use as the backend.</param>
        public PlayerBLL(IPlayerRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public IPlayerModel AddPlayerToRoom(IPlayerModel playerModel, int roomId)
        {
            PlayerDTO playerDto = ConvertToDto(playerModel);
            playerDto.RoomId = roomId;
            playerDto = _repository.AddPlayerToRoom(playerDto);
            return ConvertToModel(playerDto);
        }

        public IPlayerModel[] GetPlayersInRoom(int roomId)
        {
            PlayerDTO[] dtos = _repository.GetPlayersInRoom(roomId);
            return ConvertManyToModels(dtos).ToArray();
        }

        internal List<IPlayerModel> ConvertManyToModels(IEnumerable<PlayerDTO> dtos)
        {
            List<IPlayerModel> models = new List<IPlayerModel>();
            foreach (PlayerDTO dto in dtos)
            {
                models.Add(ConvertToModel(dto));
            }
            return models;
        }

        internal PlayerDTO ConvertToDto(IPlayerModel model)
        {
            PlayerDTO dto = new PlayerDTO()
            {
                Id = model.Id,
                Name = model.Name,
                Num = model.Num
            };
            return dto;
        }

        internal IPlayerModel ConvertToModel(PlayerDTO dto)
        {
            if (dto == null)
            {
                return null;
            }
            IPlayerModel model = new PlayerModel
            {
                Id = dto.Id,
                Name = dto.Name,
                Num = dto.Num,
                Symbol = dto.Name.Substring(0, 1).ToUpper(),
                Color = (dto.Num == 1) ? ConsoleColor.Red : ConsoleColor.Yellow
            };
            return model;
        }
    }
}
