using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;

namespace NCC.ClearView.Application.Core
{
    public class Errors
	{
		private string dsn = "";
		private int user = 0;
		private SqlParameter[] arParams;
        public Errors(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
		}
        public int Add(string _error, string _problem, string _resolution, int _typeid, string _path, int _userid)
        {
            arParams = new SqlParameter[7];
            arParams[0] = new SqlParameter("@error", _error);
            arParams[1] = new SqlParameter("@problem", _problem);
            arParams[2] = new SqlParameter("@resolution", _resolution);
            arParams[3] = new SqlParameter("@typeid", _typeid);
            arParams[4] = new SqlParameter("@path", _path);
            arParams[5] = new SqlParameter("@userid", _userid);
            arParams[6] = new SqlParameter("@id", SqlDbType.Int);
            arParams[6].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addError", arParams);
            return Int32.Parse(arParams[6].Value.ToString());
        }
        public DataSet Gets(string _error, int _userid)
        {
            string strError = _error;
            // Get general error message (up to ~ character)
            if (strError.Contains("~") == true)
                strError = strError.Substring(0, strError.IndexOf("~"));
            strError = strError.Trim();

            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@error", strError);
            arParams[1] = new SqlParameter("@userid", _userid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getErrors", arParams);
        }
        public void CheckError(DataSet dsLatest)
        {
            if (dsLatest.Tables[0].Rows.Count > 0)
            {
                // dsLatest has the latest error message for the workstationID / serverID AND step
                int intErrorID = 0;
                Int32.TryParse(dsLatest.Tables[0].Rows[0]["errorid"].ToString(), out intErrorID);
                if (intErrorID > 0)
                {
                    DataSet dsErrors = GetAll(intErrorID);
                    if (dsErrors.Tables[0].Rows.Count == 1)
                    {
                        // Since this was the only error (and it didn't fix it since the workstationid and stepid are the same), delete the error
                        Delete(intErrorID);
                    }
                }
            }
        }
        public DataSet GetAll(int _errorid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@errorid", _errorid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getErrorsAll", arParams);
        }
        public DataSet Delete(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_deleteError", arParams);
        }

        public int AddType(string _name, int _display, int _enabled)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@display", _display);
            arParams[2] = new SqlParameter("@enabled", _enabled);
            arParams[3] = new SqlParameter("@id", SqlDbType.Int);
            arParams[3].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addErrorType", arParams);
            return Int32.Parse(arParams[3].Value.ToString());
        }
        public void UpdateType(int _id, string _name, int _enabled)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateErrorType", arParams);
        }
        public void UpdateTypeOrder(int _id, int _display)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@display", _display);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateErrorTypeOrder", arParams);
        }
        public void EnableType(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateErrorTypeEnabled", arParams);
        }
        public void DeleteType(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteErrorType", arParams);
        }
        public DataSet GetType(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getErrorType", arParams);
        }
        public string GetType(int _id, string _column)
        {
            DataSet ds = GetType(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public DataSet GetTypes(int _enabled)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getErrorTypes", arParams);
        }


        public int AddTypeType(int _typeid, string _name, int _display, int _enabled)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@typeid", _typeid);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@display", _display);
            arParams[3] = new SqlParameter("@enabled", _enabled);
            arParams[4] = new SqlParameter("@id", SqlDbType.Int);
            arParams[4].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addErrorTypeType", arParams);
            return Int32.Parse(arParams[4].Value.ToString());
        }
        public void UpdateTypeType(int _id, int _typeid, string _name, int _enabled)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@typeid", _typeid);
            arParams[2] = new SqlParameter("@name", _name);
            arParams[3] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateErrorTypeType", arParams);
        }
        public void UpdateTypeTypeOrder(int _id, int _display)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@display", _display);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateErrorTypeTypeOrder", arParams);
        }
        public void EnableTypeType(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateErrorTypeTypeEnabled", arParams);
        }
        public void DeleteTypeType(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteErrorTypeType", arParams);
        }
        public DataSet GetTypeType(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getErrorTypeType", arParams);
        }
        public string GetTypeType(int _id, string _column)
        {
            DataSet ds = GetTypeType(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public DataSet GetTypeTypes(int _typeid, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@typeid", _typeid);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getErrorTypeTypes", arParams);
        }

    }
}
