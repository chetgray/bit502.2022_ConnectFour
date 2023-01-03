﻿using System;
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

        private static PlayerDTO ConvertToDto(DataRow row)
        {
            throw new NotImplementedException();
        }
    }
}
