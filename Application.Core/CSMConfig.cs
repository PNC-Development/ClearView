using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Data.SqlTypes;

namespace NCC.ClearView.Application.Core
{
	public class CSMConfig
	{
		private string dsn = "";
		private int user = 0;
		private SqlParameter[] arParams;
        public CSMConfig(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
		}
        public int Add(int _requestid, string _name, int _servers, int _users)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@servers", _servers);
            arParams[3] = new SqlParameter("@users", _users);
            arParams[4] = new SqlParameter("@id", SqlDbType.Int);
            arParams[4].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addCSMConfig", arParams);
            return Int32.Parse(arParams[4].Value.ToString());
        }
        public void Update(int _id, string _name, int _servers, int _users)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@servers", _servers);
            arParams[3] = new SqlParameter("@users", _users);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateCSMConfig", arParams);
        }
        public void UpdateLocalNodes(int _id, int _local_nodes)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@local_nodes", _local_nodes);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateCSMConfigLocalNodes", arParams);
        }
        public void Delete(int _id, int _answerid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteCSMConfig", arParams);
            Storage oStorage = new Storage(user, dsn);
            oStorage.DeleteLuns(_answerid, 0, 0, _id, 0);
        }
        public DataSet Gets(int _requestid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getCSMConfigs", arParams);
        }
        public DataSet Get(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getCSMConfig", arParams);
        }
        public string Get(int _id, string _column)
        {
            DataSet ds = Get(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
    }
}
