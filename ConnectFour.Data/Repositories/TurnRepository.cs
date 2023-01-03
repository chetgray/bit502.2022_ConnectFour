using System;
using System.Data;

using ConnectFour.Data.DALs;
using ConnectFour.Data.DTOs;

namespace ConnectFour.Data.Repositories
{
    public class TurnRepository : BaseRepository
    {
        /// <inheritdoc cref="BaseRepository()"/>
        public TurnRepository() { }

        /// <inheritdoc/>
        public TurnRepository(IDAL dal) : base(dal) { }

        private static TurnDTO ConvertToDto(DataRow row)
        {
            throw new NotImplementedException();
        }
    }
}
