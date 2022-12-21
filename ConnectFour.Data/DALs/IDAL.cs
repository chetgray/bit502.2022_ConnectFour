using System.Collections.Generic;
using System.Data;

namespace ConnectFour.Data.DALs
{
    public interface IDAL
    {
        DataTable ExecuteStoredProcedure(
            string storedProcedureName,
            Dictionary<string, object> parameters
        );
    }
}
