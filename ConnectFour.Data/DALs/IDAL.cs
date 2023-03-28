using System.Collections.Generic;
using System.Data;

namespace ConnectFour.Data.DALs
{
    public interface IDAL
    {
        /// <summary>
        /// Executes a stored procedure and returns the final result as a DataTable.
        /// </summary>
        /// <returns>The last DataTable in the result DataSet.</returns>
        /// <param name="storedProcedureName">The name of the stored procedure to execute.</param>
        /// <param name="parameters">The Dictionary of parameters to pass to the stored procedure.</param>
        /// <remarks>
        /// The stored procedure could <c>SELECT</c> multiple result tables, or nothing. The
        /// desired result table should be the last <c>SELECT</c> in the stored procedure.
        /// </remarks>
        DataTable ExecuteStoredProcedure(
            string storedProcedureName,
            Dictionary<string, object> parameters
        );

        /// <summary>
        /// Inserts value(s) via stored procedure and returns the first column of the first row in the result set or a null reference if the result set is empty.
        /// </summary>
        /// <returns>The first column of the first row in the result set, or a null reference if the result set is empty. Returns a maximum of 2033 characters.</returns>
        /// <param name="storedProcedureName">The name of the stored procedure to execute.</param>
        /// <param name="parameters">The Dictionary of parameters to pass to the stored procedure.</param>
        object InsertDataViaStoredProcedure(string storedProcedureName, Dictionary<string, object> parameters);
    }
}
