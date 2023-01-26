using System;

using ConnectFour.Business.BLLs.Interfaces;
using ConnectFour.Business.Models.Interfaces;
using ConnectFour.Data.DTOs;
using ConnectFour.Data.Repositories;
using ConnectFour.Data.Repositories.Interfaces;

namespace ConnectFour.Business.BLLs
{
    public class RoomBLL : IRoomBLL
    {
        private IRoomRepository _repository;

        /// <summary>
        /// Creates a <see cref="RoomBLL"/> instance with a default <see cref="RoomRepository"/>
        /// backend.
        /// </summary>
        public RoomBLL()
        {
            _repository = new RoomRepository();
        }

        /// <summary>
        /// Creates a <see cref="RoomBLL"/> instance with the passed <paramref name="repository"/>
        /// as the backend.
        /// </summary>
        /// <param name="repository">The <see cref="IRoomRepository"/> to use as the backend.</param>
        public RoomBLL(IRoomRepository repository)
        {
            _repository =
                repository ?? throw new ArgumentNullException(nameof(repository));
        }

        internal RoomDTO ConvertToDto(IRoomModel model)
        {
            throw new NotImplementedException();
        }

        internal IRoomModel ConvertToModel(RoomDTO dto)
        {
            throw new NotImplementedException();
        }
    }
}
