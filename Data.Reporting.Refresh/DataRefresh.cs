using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Data.Common;

namespace NCC.ClearView.Data.Reporting.Refresh
{
    /// <summary>
    /// This class is responsible for refreshing or copying the contents of a 
    /// given source database to a given target database.
    /// <remarks>TODO - Need to move T-SQL statements in this class to stored 
    /// procedures to improve speed</remarks>
    /// </summary>
    public class DataRefresh
    {
        #region Member Variables
        private SqlConnection sourceDatabaseConnection;
        private SqlConnection targetDatabaseConnection;
        private int errorCount;
        #endregion

        #region Constructors
        public DataRefresh(string sourceDatabase, string targetDatabase)
        {
            // Reset counter
            ErrorCount = 0;

            // Setup database connections
            SourceDatabaseConnection = GetDatabaseConnection(sourceDatabase);
            TargetDatabaseConnection = GetDatabaseConnection(targetDatabase);

            // Open database connections
            OpenDatabaseConnection(SourceDatabaseConnection);
            OpenDatabaseConnection(TargetDatabaseConnection);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets/Sets the source database connection
        /// </summary>
        private SqlConnection SourceDatabaseConnection
        {
            get { return sourceDatabaseConnection; }
            set { sourceDatabaseConnection = value; }
        }

        /// <summary>
        /// Gets/Sets the target database connection
        /// </summary>
        private SqlConnection TargetDatabaseConnection
        {
            get { return targetDatabaseConnection; }
            set { targetDatabaseConnection = value; }
        }

        /// <summary>
        /// Gets/Sets the errors encountered during the data refresh
        /// </summary>
        private int ErrorCount
        {
            get { return errorCount; }
            set { errorCount = value; }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Performs the data warehouse refresh
        /// </summary>
        public void Execute()
        {
            if (ErrorCount == 0)
            {
                List<string> tableNames = GetTableNames(SourceDatabaseConnection);

                foreach (string tableName in tableNames)
                {
                    if (VerifyTableSchemas(tableName))
                    {
                        CopyData(tableName);
                    }
                }
            }
            
            CloseDatabaseConnection(SourceDatabaseConnection);
            CloseDatabaseConnection(TargetDatabaseConnection);

            LogCompletion();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Verifies the schemas for a given table in the source and target databases
        /// </summary>
        /// <param name="tableName">Table name to verify</param>
        /// <returns>True if the table schemas match, false if they do not</returns>
        private bool VerifyTableSchemas(string tableName)
        {
            DataTable sourceDataTable = GetTableSchema(tableName, SourceDatabaseConnection);
            DataTable targetDataTable = GetTableSchema(tableName, TargetDatabaseConnection);
            bool tableSchemaValid = false;

            try
            {
                if (sourceDataTable != null && targetDataTable != null)
                {
                    for (int columnIndex = 0; columnIndex < sourceDataTable.Columns.Count; columnIndex++)
                    {
                        if (sourceDataTable.Columns[columnIndex].AllowDBNull != targetDataTable.Columns[columnIndex].AllowDBNull)
                        {
                            break;
                        }

                        if (sourceDataTable.Columns[columnIndex].AutoIncrement != targetDataTable.Columns[columnIndex].AutoIncrement)
                        {
                            break;
                        }

                        if (sourceDataTable.Columns[columnIndex].ColumnName != targetDataTable.Columns[columnIndex].ColumnName)
                        {
                            break;
                        }

                        if (sourceDataTable.Columns[columnIndex].DataType != targetDataTable.Columns[columnIndex].DataType)
                        {
                            break;
                        }
                    }

                    tableSchemaValid = true;
                }
            }
            catch (Exception exception)
            {
                LogException(exception, "Error verifying schema for table: " + tableName);
                ErrorCount += 1;
            }

            return tableSchemaValid;
        }

        /// <summary>
        /// Gets the schema for a given table name
        /// </summary>
        /// <param name="tableName">Table name</param>
        /// <param name="connection">SQL Connection</param>
        /// <returns>Table schema if the table is found, else null</returns>
        private DataTable GetTableSchema(string tableName, SqlConnection connection)
        {
            DataTable dataTable;

            try
            {
                SqlCommand command = new SqlCommand("SELECT TOP 1 * FROM " + tableName, connection);
                SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                dataTable = new DataTable();
                dataAdapter.FillSchema(dataTable, SchemaType.Source);
            }
            catch (Exception exception)
            {
                LogException(exception, string.Format("Error retrieving schema for table: {0} for connection: {1}.", tableName, connection.ConnectionString));
                ErrorCount += 1;
                dataTable = null;
            }

            return dataTable;
        }

        /// <summary>
        /// Gets all the user table names for a given SQL connection
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        private List<string> GetTableNames(SqlConnection connection)
        {
            List<string> tables = new List<string>();

            SqlCommand command = new SqlCommand("SELECT * FROM sysobjects WHERE type = 'U'", connection);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                tables.Add(reader[0].ToString());
            }

            reader.Close();

            return tables;
        }

        /// <summary>
        /// Gets a database connection from the app config for a given database
        /// </summary>
        /// <param name="configSettingKey">Database connection string key</param>
        /// <returns>Database connection </returns>
        private SqlConnection GetDatabaseConnection(string configSettingKey)
        {
            string databaseConnectionString = null;
            SqlConnection databaseConnection = null;

            try
            {
                databaseConnectionString = ConfigurationManager.ConnectionStrings[configSettingKey].ToString();
            }
            catch (Exception exception)
            {
                LogException(exception, "Unable to retrieve database connection string for entry: " + configSettingKey + ".");
                ErrorCount += 1;
            }

            if (databaseConnectionString != null || databaseConnectionString != string.Empty)
            {
                try
                {
                    databaseConnection = new SqlConnection(databaseConnectionString);
                }
                catch (Exception exception)
                {
                    LogException(exception, "Unable to retrieve database connection for: " + databaseConnectionString + ".");
                    ErrorCount += 1;
                }
            }

            return databaseConnection;
        }

        /// <summary>
        /// Opens a database connection
        /// </summary>
        /// <param name="databaseConnection">SQL Connection object to open</param>
        private void OpenDatabaseConnection(SqlConnection databaseConnection)
        {
            try
            {
                databaseConnection.Open();
            }
            catch (Exception exception)
            {
                LogException(exception, "Unable to open database connection for connection string: " + databaseConnection.ConnectionString + ".");
                ErrorCount += 1;
            }
        }

        /// <summary>
        /// Closes a database connection
        /// </summary>
        /// <param name="databaseConnection">SQL Connection object to close</param>
        private void CloseDatabaseConnection(SqlConnection databaseConnection)
        {
            try
            {
                databaseConnection.Close();
            }
            catch (Exception exception)
            {
                LogException(exception, "Unable to close database connection for connection string: " + databaseConnection.ConnectionString + ".");
                ErrorCount += 1;
            }
        }

        /// <summary>
        /// Log exception to event viewer
        /// </summary>
        /// <param name="exception">Exception to log</param>
        /// <param name="errorMessage">Error message associated with exception</param>
        private void LogException(Exception exception, string errorMessage)
        {
            string source = CreateEventSource();

            StringBuilder message = new StringBuilder();

            message.Append(errorMessage);
            message.Append(System.Environment.NewLine);
            message.Append(System.Environment.NewLine);
            message.Append("Exception information:");
            message.Append(System.Environment.NewLine);
            message.Append(exception.ToString());

            EventLog.WriteEntry(source, message.ToString(), EventLogEntryType.Error);
        }

        /// <summary>
        /// Logs the completion message for the data refresh program
        /// </summary>
        private void LogCompletion()
        {
            string source = CreateEventSource();

            string message = string.Format("ClearView Data Warehouse Data Refresh has completed with {0} errors.", ErrorCount);

            EventLog.WriteEntry(source, message, EventLogEntryType.Information);
        }

        /// <summary>
        /// Creates an event source for the data fresh program (if it does not exist)
        /// </summary>
        /// <returns>Event source name</returns>
        private string CreateEventSource()
        {
            string source = "Clearview";

            if (!EventLog.SourceExists(source))
            {
                EventLog.CreateEventSource(source, "Application");
            }

            return source;
        }

        /// <summary>
        /// Copies the actual data for a given table from the source database to the target database.  This
        /// method assumes the tables exist, the table structures match (have been verified), and all database 
        /// connections are open and availble.
        /// </summary>
        /// <param name="tableName">Table to copy data over</param>
        private void CopyData(string tableName)
        {
            SqlDataReader dataReader = null;

            try
            {
                // Retrieve source database contents
                SqlCommand readTableCommand = new SqlCommand("SELECT * FROM " + tableName, SourceDatabaseConnection);
                dataReader = readTableCommand.ExecuteReader();

                // Clear all target database contents
                SqlCommand clearTableCommand = new SqlCommand("DELETE FROM " + tableName, TargetDatabaseConnection);
                clearTableCommand.ExecuteNonQuery();

                // Copy transactional database table to reporting database table
                SqlBulkCopy bulkCopy = new SqlBulkCopy(TargetDatabaseConnection);
                bulkCopy.DestinationTableName = tableName;
                bulkCopy.WriteToServer(dataReader);

                dataReader.Close();
            }
            catch (Exception exception)
            {
                LogException(exception, "Unable to copy data for table: " + tableName + ".");
                ErrorCount += 1;

                if (dataReader != null && !dataReader.IsClosed)
                {
                    dataReader.Close();
                }
            }
        }
        #endregion
    }
}