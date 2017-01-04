using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Data;
using NCC.ClearView.Application.UI.BusinessLogic;

namespace NCC.ClearView.Application.UI.DataAccess
{
    public class BaseDal : IDisposable
    {

        public static string SQLERROR = "SQLERROR";
        public static string SQLERRORCODE = "SQLERRORCODE";
        public static string SQLERRORNUMBER = "SQLERRORNUMBER";
        public static string SQLERRORMESSAGE = "SQLERRORMESSAGE";
        public static string ERRORMESSAGEDELIMITER = "|";

        private string _methodName = "unknown";
        private string _storedProcName;

        private string _sqlString;
        private static Database db;

        private DbConnection dbConnection;
        private DbCommand dbCommand;
        private IDataReader dataReader;
        private DbTransaction dbTransaction;

        /// <summary> 
        /// Constructor DataSource 
        /// </summary> 
        /// <param name="dataSource"></param> 
        public BaseDal(DataSource dataSource)
        {
            CreateDatabase(dataSource);
            CreateConnection();

        }

        public BaseDal(string  ConnectionName)
        {
            CreateDatabase(ConnectionName);
            CreateConnection();

        }

        /// <summary> 
        /// Constructor with a datasource and transaction 
        /// </summary> 
        /// <param name="dataSource"></param> 
        /// <param name="transaction"></param> 
        public BaseDal(DataSource dataSource, DbTransaction transaction)
        {
            CreateDatabase(dataSource);
            CreateConnection();
            dbTransaction = transaction;
        }



        public BaseDal(string ConnectionName, DbTransaction transaction)
        {
            CreateDatabase(ConnectionName);
            CreateConnection();
            dbTransaction = transaction;
        }

        /// <summary> 
        /// Private creation of database 
        /// </summary> 
        /// <remarks></remarks> 
        private void CreateDatabase(DataSource dataSource)
        {
            try
            {
                db = DatabaseFactory.CreateDatabase(dataSource.ToString());
            }
            catch (Exception ex)
            {
                throw new Exception();
                //throw new DatabaseException(string.Format("Unable to connect to the desired database: {0}. Check the app.config or web.config. Exception Message: {1}", dataSource.ToString, ex.Message), ex);
            }
        }

        private void CreateDatabase(string strConnectionName)
        {
            try
            {
                db = DatabaseFactory.CreateDatabase(strConnectionName);
            }
            catch (Exception ex)
            {
                throw new Exception();
                //throw new DatabaseException(string.Format("Unable to connect to the desired database: {0}. Check the app.config or web.config. Exception Message: {1}", dataSource.ToString, ex.Message), ex);
            }
        }
        /// <summary> 
        /// Private creation of connection 
        /// </summary> 
        /// <remarks></remarks> 
        /// 
        private void CreateConnection()
        {
            try
            {
                if ((db != null))
                {
                    dbConnection = db.CreateConnection();
                    dbConnection.Open();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary> 
        /// Gets the database 
        /// </summary> 
        public Database Database
        {
            get { return db; }
        }

        /// <summary> 
        /// Gets/Sets the method name 
        /// </summary> 
        public string MethodName
        {
            get { return _methodName; }
            set { _methodName = value; }
        }

        /// <summary> 
        /// Gets/Sets the procedure to call 
        /// </summary> 
        public string ProcedureName
        {
            get { return _storedProcName; }
            set { _storedProcName = value; }
        }

        /// <summary> 
        /// Gets/Sets the sql string command 
        /// </summary> 
        public string _sqlStringCommand
        {
            get { return _sqlString; }
            set { _sqlString = value; }
        }

        /// <summary> 
        /// Gets the database connection 
        /// </summary> 
        public DbConnection Connection
        {
            get { return dbConnection; }
        }

        /// <summary> 
        /// Gets/Sets the database transaction 
        /// </summary> 
        public DbTransaction Transaction
        {
            get { return dbTransaction; }
            set { dbTransaction = value; }
        }


        /// <summary> 
        /// Executes a DataReader 
        /// </summary> 
        /// <param name="command"></param> 
        /// <returns></returns> 
        public IDataReader ExecuteReader(DbCommand command)
        {
            if (Transaction == null)
            {
                command.Connection = dbConnection;
                return command.ExecuteReader();
            }
            else
            {
                command.Connection = dbConnection;
                command.Transaction = Transaction;
                return command.ExecuteReader();
            }
        }

        /// <summary> 
        /// Executes a Non Query 
        /// </summary> 
        /// <param name="command"></param> 
        /// <returns></returns> 
        public int ExecuteNonQuery(DbCommand command)
        {
            if (Transaction == null)
            {
                command.Connection = dbConnection;
                return command.ExecuteNonQuery();
            }
            else
            {
                command.Connection = dbConnection;
                command.Transaction = Transaction;
                return command.ExecuteNonQuery();
            }
        }

        /// <summary> 
        /// Executes a Scalar 
        /// </summary> 
        /// <param name="command"></param> 
        /// <returns></returns> 
        public object ExecuteScalar(DbCommand command)
        {
            if (Transaction == null)
            {
                command.Connection = dbConnection;
                return command.ExecuteScalar();
            }
            else
            {
                command.Connection = dbConnection;
                command.Transaction = Transaction;
                return command.ExecuteScalar();
            }
        }

        /// <summary> 
        /// Executes a DataSet 
        /// </summary> 
        /// <param name="command"></param> 
        /// <returns></returns> 
        public DataSet ExecuteDataSet(DbCommand command)
        {

            if (Transaction == null)
            {
                return Database.ExecuteDataSet(command);
            }
            else
            {

                return Database.ExecuteDataSet(command, Transaction);
            }

        }


        /// <summary> 
        /// Converts DataReader into DataSet 
        /// </summary> 
        /// <param name="reader"></param> 
        /// <returns></returns> 
        /// <remarks></remarks> 


        /// <summary> 
        /// Clears the Command Parameters 
        /// </summary> 
        public void ClearDBCommandParameters()
        {
            if (dbCommand != null && dbCommand.Parameters != null)
            {

                dbCommand.Parameters.Clear();
            }
        }


        public byte SqlState(SqlException sqlEx)
        {
            if (sqlEx == null)
            {
                return 0;
            }
            else
            {
                return sqlEx.State;
            }
        }

        public enum SqlErrorPart
        {
            Code = 0,
            Number = 1,
            Message = 2
        }

        //public static DatabaseException CreateDatabaseException(string code, string number, string msg)
        //{
        //    DbException dbException = new DatabaseException();
        //    dbException.Data.Add(SQLERRORCODE, code);
        //    dbException.Data.Add(SQLERRORNUMBER, number);
        //    dbException.Data.Add(SQLERRORMESSAGE, msg);
        //    return dbException;
        //}


        private bool disposedValue = false;
        // To detect redundant calls 

        // IDisposable 
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                }
            }

            this.disposedValue = true;
        }

        #region " IDisposable Support "
        public void Dispose()
        {

            if (((dbConnection != null)))
            {
                if ((!(dbConnection.State == ConnectionState.Closed) | !(dbConnection.State == ConnectionState.Broken)))
                {
                    dbConnection.Close();
                }
            }

            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion


        #region IDisposable Members

        void IDisposable.Dispose()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }

}
