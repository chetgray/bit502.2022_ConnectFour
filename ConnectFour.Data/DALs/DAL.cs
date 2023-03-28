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
            using (
                SqlCommand command = new SqlCommand(storedProcedureName, connection)
                {
                    CommandType = CommandType.StoredProcedure
                }
            )
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                connection.Open();
                foreach (KeyValuePair<string, object> parameter in parameters)
                {
                    command.Parameters.AddWithValue(parameter.Key, parameter.Value);
                }
                // Get the last DataTable in the result DataSet
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet);
                dataTable = dataSet.Tables[dataSet.Tables.Count - 1];
            }

            return dataTable;
        }
        public object InsertDataViaStoredProcedure(string storedProcedureName, Dictionary<string, object> parameters)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(storedProcedureName, connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    foreach (var param in parameters)
                    {
                        cmd.Parameters.Add(new SqlParameter(param.Key, param.Value));
                    }
                    return cmd.ExecuteScalar();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
