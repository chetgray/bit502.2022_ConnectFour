using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ConnectFour.Data.DALs
{
    internal class DAL : IDAL
    {
        private readonly string _connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="DAL"/> class with the passed <paramref
        /// name="connectionString">connection string</paramref>.
        /// </summary>
        /// <param name="connectionString">
        /// The connection string to use for any <see cref="SqlConnection">SQL
        /// connection</see>s.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the passed <paramref name="connectionString">connection
        /// string</paramref> is <c><see langword="null">null</see></c>.
        /// </exception>
        internal DAL(string connectionString)
        {
            _connectionString =
                connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        /// <inheritdoc/>
        public void ExecuteStoredProcedure(
            string storedProcedureName,
            Dictionary<string, object> parameters
        )
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (
                SqlCommand command = new SqlCommand(storedProcedureName, connection)
                {
                    CommandType = CommandType.StoredProcedure,
                }
            )
            {
                connection.Open();
                foreach (KeyValuePair<string, object> parameter in parameters)
                {
                    command.Parameters.AddWithValue(parameter.Key, parameter.Value);
                }
                command.ExecuteNonQuery();
            }
        }

        public DataTable GetTableFromStoredProcedure(
            string storedProcedureName,
            Dictionary<string, object> parameters
        )
        {
            DataTable resultTable = new DataTable();

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
                resultTable = dataSet.Tables[dataSet.Tables.Count - 1];
            }

            return resultTable;
        }

        public object GetValueFromStoredProcedure(
            string storedProcedureName,
            Dictionary<string, object> parameters
        )
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (
                SqlCommand command = new SqlCommand(storedProcedureName, connection)
                {
                    CommandType = CommandType.StoredProcedure
                }
            )
            {
                connection.Open();
                foreach (KeyValuePair<string, object> parameter in parameters)
                {
                    command.Parameters.AddWithValue(parameter.Key, parameter.Value);
                }
                return command.ExecuteScalar();
            }
        }
    }
}
