using System;

using ConnectFour.Business.BLLs.Interfaces;
using ConnectFour.Business.Models;
using ConnectFour.Business.Models.Interfaces;
using ConnectFour.Data.DTOs;
using ConnectFour.Data.Repositories;
using ConnectFour.Data.Repositories.Interfaces;

namespace ConnectFour.Business.BLLs
{
    internal class TurnBLL : ITurnBLL
    {
        private ITurnRepository _repository;

        /// <summary>
        /// Creates a <see cref="TurnBLL"/> instance with a default <see cref="TurnRepository"/>
        /// backend.
        /// </summary>
        public TurnBLL()
        {
            _repository = new TurnRepository();
        }

        /// <summary>
        /// Creates a <see cref="TurnBLL"/> instance with the passed <paramref name="repository"/>
        /// as the backend.
        /// </summary>
        /// <param name="repository">The <see cref="ITurnRepository"/> to use as the backend.</param>
        public TurnBLL(ITurnRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public ITurnModel GetLastTurnInRoom(int roomId)
        {
            TurnRepository turnRepository = new TurnRepository();
            TurnDTO turnDTO = turnRepository.GetLastTurnInRoom(roomId);
            return ConvertToModel(turnDTO);
        }

        public void AddTurnToRoom(ITurnModel turn, int roomId)
        {
            TurnRepository turnRepository = new TurnRepository();

            TurnDTO turnDTO = ConvertToDTO(turn);
            turnDTO.RoomId = roomId;

            turnRepository.AddTurnToRoom(turnDTO);

        }
        private ITurnModel ConvertToModel(TurnDTO dto)
        {
            if (dto == null)
            {
                return null;
            }
            TurnModel model = new TurnModel
            {
                Id = dto.Id,
                Time = dto.Time,
                RowNum = dto.RowNum,
                ColNum = dto.ColNum,
                Num = dto.Num
            };
            return model;
        }

        private TurnDTO ConvertToDTO(ITurnModel model)
        {
            TurnDTO dto = new TurnDTO
            {
                Id = model.Id,
                Time = model.Time,
                RowNum = model.RowNum,
                ColNum = model.ColNum,
                Num = model.Num
            };
            return dto;
        }
    }
}
