using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;

namespace NCC.ClearView.Application.Core
{
	public class WebServices
	{
		private string dsn = "";
		private int user = 0;
		private SqlParameter[] arParams;
        public WebServices(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
		}
        public DataSet Gets(int _enabled)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getWebServices", arParams);
        }
        public DataSet Get(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getWebService", arParams);
        }
        public string Get(int _id, string _column)
        {
            DataSet ds = Get(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public void Add(string _name, int _can_read, int _can_write, int _enabled)
		{
			arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@can_read", _can_read);
            arParams[2] = new SqlParameter("@can_write", _can_write);
            arParams[3] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addWebService", arParams);
		}
        public void Update(int _id, string _name, int _can_read, int _can_write, int _enabled)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@can_read", _can_read);
            arParams[3] = new SqlParameter("@can_write", _can_write);
            arParams[4] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateWebService", arParams);
        }
        public void Enable(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateWebServiceEnabled", arParams);
        }
        public void Delete(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteWebService", arParams);
        }
        public void AddUser(int _webserviceid, int _userid, int _can_read, int _can_write)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@webserviceid", _webserviceid);
            arParams[1] = new SqlParameter("@userid", _userid);
            arParams[2] = new SqlParameter("@can_read", _can_read);
            arParams[3] = new SqlParameter("@can_write", _can_write);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addWebServiceUser", arParams);
        }
        public void DeleteUser(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteWebServiceUser", arParams);
        }
        public DataSet GetUsers(int _webserviceid, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@webserviceid", _webserviceid);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getWebServiceUsers", arParams);
        }
        public DataSet GetUser(string _name, int _userid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@userid", _userid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getWebServiceUser", arParams);
        }
        public bool CanRead(string _name, int _userid)
        {
            bool boolReturn = false;
            DataSet ds = GetUser(_name, _userid);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                int intID = Int32.Parse(dr["webserviceid"].ToString());
                if (Get(intID, "can_read") == "1" || dr["can_read"].ToString() == "1")
                {
                    boolReturn = true;
                    break;
                }
            }
            return boolReturn;
        }
        public bool CanWrite(string _name, int _userid)
        {
            bool boolReturn = false;
            DataSet ds = GetUser(_name, _userid);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                int intID = Int32.Parse(dr["webserviceid"].ToString());
                if (Get(intID, "can_write") == "1" || dr["can_write"].ToString() == "1")
                {
                    boolReturn = true;
                    break;
                }
            }
            return boolReturn;
        }
    }
}
