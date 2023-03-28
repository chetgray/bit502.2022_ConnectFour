﻿using System.Collections.Generic;
using System.Data;

using ConnectFour.Data.DALs;

namespace ConnectFour.Tests.TestDoubles
{
    internal class DALStub : IDAL
    {
        public DataTable TestDataTable { get; set; }

        public DataTable ExecuteStoredProcedure(
            string storedProcedureName,
            Dictionary<string, object> parameters
        )
        {
            return TestDataTable;
        }
    }
}