using System.Collections.Generic;
using System.Data;

using ConnectFour.Data.DALs;

namespace ConnectFour.Tests.TestDoubles
{
    internal class DALStub : IDAL
    {
        public DataTable TestDataTable { get; set; }
        public object TestObject { get; set; }

        public void ExecuteStoredProcedure(
            string storedProcedureName,
            Dictionary<string, object> parameters
        )
        { }

        public DataTable GetTableFromStoredProcedure(
            string storedProcedureName,
            Dictionary<string, object> parameters
        )
        {
            return TestDataTable;
        }

        public object GetValueFromStoredProcedure(
            string storedProcedureName,
            Dictionary<string, object> parameters
        )
        {
            return TestObject;
        }
    }
}
