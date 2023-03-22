using System;
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

        internal TurnDTO ConvertToDto(DataRow row)
        {
            throw new NotImplementedException();
        }
    }
}
