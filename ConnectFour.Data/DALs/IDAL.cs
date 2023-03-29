using System.Collections.Generic;
using System.Data;

namespace ConnectFour.Data.DALs
{
    public interface IDAL
    {
        /// <summary>Executes a stored procedure with parameters.</summary>
        /// <param name="storedProcedureName">
        /// The name of the stored procedure to execute.
        /// </param>
        /// <param name="parameters">
        /// The <see cref="Dictionary{string, object}">collection</see> of parameters to pass.
        /// </param>
        void ExecuteStoredProcedure(
            string storedProcedureName,
            Dictionary<string, object> parameters
        );

        /// <inheritdoc cref="ExecuteStoredProcedure(string, Dictionary{string, object})"/>
        /// <summary>
        /// Executes a stored procedure and returns the final result as a <see cref="DataTable"/>.
        /// </summary>
        /// <returns>The last <see cref="DataTable"/> in the result <see cref="DataSet"/>.</returns
        /// <remarks>
        /// The stored procedure could <c>SELECT</c> multiple result tables, or nothing. The
        /// desired result table should be the last <c>SELECT</c> in the stored procedure.
        /// </remarks>
        DataTable GetTableFromStoredProcedure(
            string storedProcedureName,
            Dictionary<string, object> parameters
        );

        /// <inheritdoc cref="ExecuteStoredProcedure(string, Dictionary{string, object})"/>
        /// <summary>
        /// Executes a stored procedure with parameters, returning a single value.
        /// </summary>
        /// <returns>The first column of the first row in the result set, or a null reference if the result set is empty. Returns a maximum of 2033 characters.</returns>
        object GetValueFromStoredProcedure(
            string storedProcedureName,
            Dictionary<string, object> parameters
        );
    }
}
