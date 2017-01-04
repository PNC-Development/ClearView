using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;

namespace NCC.ClearView.Application.Core
{
    public class Supports
	{
		private string dsn = "";
		private int user = 0;
		private SqlParameter[] arParams;
        public Supports(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
		}
        public DataSet Gets()
        {
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getSupports");
        }
        public DataSet GetsAdmin()
        {
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getSupportsAdmin");
        }
        public DataSet GetsStatus(int _status)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@status", _status);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getSupportsStatus", arParams);
        }
        public DataSet GetsUser(int _userid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@userid", _userid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getSupportsUser", arParams);
        }
        public DataSet Get(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getSupport", arParams);
        }
        public string Get(int _id, string _column)
        {
            DataSet ds = Get(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public void Add(string _name, int _pageid, int _type, string _description, int _userid)
		{
			arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@pageid", _pageid);
            arParams[2] = new SqlParameter("@type", _type);
            arParams[3] = new SqlParameter("@description", _description);
            arParams[4] = new SqlParameter("@userid", _userid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addSupport", arParams);
		}
        public void Update(int _id, string _name, int _pageid, int _type, string _description, int _userid)
        {
            arParams = new SqlParameter[6];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@pageid", _pageid);
            arParams[3] = new SqlParameter("@type", _type);
            arParams[4] = new SqlParameter("@description", _description);
            arParams[5] = new SqlParameter("@userid", _userid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateSupport", arParams);
        }
        public void Update(int _id, string _comments, int _status)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@comments", _comments);
            arParams[2] = new SqlParameter("@status", _status);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateSupportComplete", arParams);
        }
        public void Delete(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteSupport", arParams);
        }
    }
}
