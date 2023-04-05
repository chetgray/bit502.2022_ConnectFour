﻿using System;
using System.Configuration;

using ConnectFour.Data.DALs;

namespace ConnectFour.Data.Repositories
{
    public abstract class BaseRepository
    {
        protected readonly IDAL _dal;

        /// <summary>
        /// Creates a repository instance with a default <see cref="DAL"/> backend.
        /// </summary>
        protected BaseRepository()
        {
            _dal = new DAL(
                ConfigurationManager.ConnectionStrings["ConnectFourData"].ConnectionString
            );
        }

        /// <summary>
        /// Creates a repository instance with the passed <paramref name="dal"/> as the backend.
        /// </summary>
        /// <param name="dal">The <see cref="IDAL"/> to use as the backend.</param>
        protected BaseRepository(IDAL dal)
        {
            _dal = dal ?? throw new ArgumentNullException(nameof(dal));
        }
    }
}
