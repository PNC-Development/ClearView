using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;

namespace NCC.ClearView.Application.Core
{
    public class OnDemandSending
	{
		private string dsn = "";
		private int user = 0;
		private SqlParameter[] arParams;
        public OnDemandSending(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
		}
        public DataSet GetConfigs(int _enabled)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getOnDemandSendingConfigs", arParams);
        }
        public DataSet GetConfig(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getOnDemandSendingConfigId", arParams);
        }
        public DataSet GetConfig(int _classid, int _environmentid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@classid", _classid);
            arParams[1] = new SqlParameter("@environmentid", _environmentid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getOnDemandSendingConfig", arParams);
        }
        public void AddConfig(int _classid, int _environmentid, string _name, int _enabled)
		{
			arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@classid", _classid);
            arParams[1] = new SqlParameter("@environmentid", _environmentid);
            arParams[2] = new SqlParameter("@name", _name);
            arParams[3] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addOnDemandSendingConfig", arParams);
		}
        public void UpdateConfig(int _id, int _classid, int _environmentid, string _name, int _enabled)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@classid", _classid);
            arParams[2] = new SqlParameter("@environmentid", _environmentid);
            arParams[3] = new SqlParameter("@name", _name);
            arParams[4] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateOnDemandSendingConfig", arParams);
        }
        public void EnableConfig(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateOnDemandSendingConfigEnabled", arParams);
        }
        public void DeleteConfig(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteOnDemandSendingConfig", arParams);
        }

    }
}
