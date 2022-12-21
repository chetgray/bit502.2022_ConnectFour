using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ConnectFour.Data.DALs
{
    internal class DAL : IDAL
    {
        private readonly string _connectionString;

        internal DAL(string connectionString)
        {
            _connectionString =
                connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public DataTable ExecuteStoredProcedure(
            string storedProcedureName,
            Dictionary<string, object> parameters
        )
        {
            DataTable dataTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (
                    SqlCommand command = new SqlCommand(storedProcedureName, connection)
                    {
                        CommandType = CommandType.StoredProcedure
                    }
                )
                {
                    foreach (KeyValuePair<string, object> parameter in parameters)
                    {
                        command.Parameters.AddWithValue(parameter.Key, parameter.Value);
                    }
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        // Get the last DataTable in the result DataSet
                        DataSet dataSet = new DataSet();
                        adapter.Fill(dataSet);
                        dataTable = dataSet.Tables[dataSet.Tables.Count - 1];
                    }
                }
            }

            return dataTable;
        }
    }
}
