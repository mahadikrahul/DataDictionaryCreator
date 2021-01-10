using DataDictionaryCreator.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DataDictionaryCreator.Helpers
{
    public class DatabaseHelper
    {
        /// <summary>
        /// Gets or sets the connection timeout.
        /// </summary>
        /// <value>
        /// The connection timeout.
        /// </value>
        public int ConnectionTimeout { get; set; } = 30;

        /// <summary>
        /// The connection string
        /// </summary>
        private readonly string _connectionString = GlobalProperties.ConnectionString;

        /// <summary>
        /// Executes the non query.
        /// </summary>
        /// <param name="procedureName">Name of the procedure.</param>
        /// <param name="parameters">The parameters.</param>
        public void ExecuteNonQuery(string procedureName, Dictionary<string, object> parameters = null)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            using var command = new SqlCommand(procedureName, connection)
            {
                CommandText = procedureName,
                CommandType = CommandType.StoredProcedure,
                CommandTimeout = ConnectionTimeout
            };
            AddParameters(command, parameters);
            command.ExecuteNonQuery();
        }

        public DataTable ExecuteQuery(string query)
        {
            DataTable dataTable = new DataTable();
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            using var command = connection.CreateCommand();
            command.CommandText = query;
            command.CommandType = CommandType.Text;
            command.CommandTimeout = ConnectionTimeout;
            SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
            dataAdapter.Fill(dataTable);
            return dataTable;
        }

        /// <summary>
        /// Gets the data set.
        /// </summary>
        /// <param name="procedureName">Name of the procedure.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public Dictionary<int, object> GetDataSet(string procedureName, Dictionary<string, object> parameters = null)
        {
            Dictionary<int, object> resultSet = new Dictionary<int, object>();
            DataSet dataSet = new DataSet();
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            using var command = new SqlCommand(procedureName, connection)
            {
                CommandText = procedureName,
                CommandType = CommandType.StoredProcedure,
                CommandTimeout = ConnectionTimeout
            };
            AddParameters(command, parameters);
            SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
            dataAdapter.Fill(dataSet);

            if (dataSet.Tables.Count > 0)
            {
                for (int i = 1; i <= dataSet.Tables.Count; i++)
                {
                    resultSet[i] = dataSet.Tables[i - 1];
                }
            }

            return resultSet;
        }

        /// <summary>
        /// Gets the data table.
        /// </summary>
        /// <param name="procedureName">Name of the procedure.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public DataTable GetDataTable(string procedureName, Dictionary<string, object> parameters = null)
        {
            DataTable dataTable = new DataTable();
            Dictionary<int, object> resultSet = GetDataSet(procedureName, parameters);
            if (resultSet != null && resultSet.Count == 1)
            {
                dataTable = (DataTable)resultSet[1];
            }

            return dataTable;
        }

        /// <summary>
        /// Adds the parameters.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="parameters">The parameters.</param>
        private void AddParameters(SqlCommand command, Dictionary<string, object> parameters)
        {
            if (parameters != null && parameters.Any())
            {
                command.Parameters.AddRange(parameters.Select(x => new SqlParameter()
                {
                    ParameterName = $"{x.Key}",
                    Value = x.Value
                }).ToArray());
            }
        }
    }
}
