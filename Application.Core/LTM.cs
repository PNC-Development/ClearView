using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;

namespace NCC.ClearView.Application.Core
{
    public class LTM
	{
		private string dsn = "";
		private int user = 0;
		private SqlParameter[] arParams;
        public LTM(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
		}
        public void AddConfig(int _answerid, string _name, int _ip1, int _ip2, int _ip3, int _ip4, string _path, int _userid)
        {
            arParams = new SqlParameter[8];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@ip1", _ip1);
            arParams[3] = new SqlParameter("@ip2", _ip2);
            arParams[4] = new SqlParameter("@ip3", _ip3);
            arParams[5] = new SqlParameter("@ip4", _ip4);
            arParams[6] = new SqlParameter("@path", _path);
            arParams[7] = new SqlParameter("@userid", _userid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addLtmConfig", arParams);
        }
        public void UpdateConfig(int _answerid, string _name, int _ip1, int _ip2, int _ip3, int _ip4, string _path, int _userid)
        {
            arParams = new SqlParameter[8];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@ip1", _ip1);
            arParams[3] = new SqlParameter("@ip2", _ip2);
            arParams[4] = new SqlParameter("@ip3", _ip3);
            arParams[5] = new SqlParameter("@ip4", _ip4);
            arParams[6] = new SqlParameter("@path", _path);
            arParams[7] = new SqlParameter("@userid", _userid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateLtmConfig", arParams);
        }
        public void UpdateConfigInstalled(int _answerid, DateTime _installed)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            arParams[1] = new SqlParameter("@installed", _installed);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateLtmConfigInstalled", arParams);
        }
        public void UpdateConfigCompleted(int _answerid, DateTime _completed, string _result)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            arParams[1] = new SqlParameter("@completed", _completed);
            arParams[2] = new SqlParameter("@result", _result);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateLtmConfigCompleted", arParams);
        }
        public void DeleteConfig(int _answerid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteLtmConfig", arParams);
        }
        public DataSet GetConfigInstalled()
        {
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getLtmConfigInstalled");
        }
        public DataSet GetConfig(int _answerid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getLtmConfig", arParams);
        }
        public string GetConfig(int _answerid, string _column)
        {
            DataSet ds = GetConfig(_answerid);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }


        public void AddConfigs(int _serverid, int _ip1, int _ip2, int _ip3, int _ip4, int _vlan)
        {
            arParams = new SqlParameter[6];
            arParams[0] = new SqlParameter("@serverid", _serverid);
            arParams[1] = new SqlParameter("@ip1", _ip1);
            arParams[2] = new SqlParameter("@ip2", _ip2);
            arParams[3] = new SqlParameter("@ip3", _ip3);
            arParams[4] = new SqlParameter("@ip4", _ip4);
            arParams[5] = new SqlParameter("@vlan", _vlan);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addLtmConfigs", arParams);
        }
        public void UpdateConfigs(int _serverid, int _ip1, int _ip2, int _ip3, int _ip4, int _vlan)
        {
            arParams = new SqlParameter[6];
            arParams[0] = new SqlParameter("@serverid", _serverid);
            arParams[1] = new SqlParameter("@ip1", _ip1);
            arParams[2] = new SqlParameter("@ip2", _ip2);
            arParams[3] = new SqlParameter("@ip3", _ip3);
            arParams[4] = new SqlParameter("@ip4", _ip4);
            arParams[5] = new SqlParameter("@vlan", _vlan);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateLtmConfigs", arParams);
        }
        public void UpdateConfigsInstalled(int _serverid, DateTime _installed)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@serverid", _serverid);
            arParams[1] = new SqlParameter("@installed", _installed);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateLtmConfigsInstalled", arParams);
        }
        public void UpdateConfigsCompleted(int _serverid, DateTime _completed)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@serverid", _serverid);
            arParams[1] = new SqlParameter("@completed", _completed);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateLtmConfigsCompleted", arParams);
        }
        public void DeleteConfigs(int _serverid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@serverid", _serverid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteLtmConfigs", arParams);
        }
        public DataSet GetConfigsInstalled()
        {
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getLtmConfigsInstalled");
        }
        public DataSet GetConfigs(int _serverid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@serverid", _serverid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getLtmConfigs", arParams);
        }
        public string GetConfigs(int _serverid, string _column)
        {
            DataSet ds = GetConfigs(_serverid);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }



        public void AddConfigsResult(int _serverid, string _result, int _error)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@serverid", _serverid);
            arParams[1] = new SqlParameter("@result", _result);
            arParams[2] = new SqlParameter("@error", _error);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addLtmConfigsResult", arParams);
        }
        public void DeleteConfigsResult(int _serverid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@serverid", _serverid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteLtmConfigsResults", arParams);
        }
        public DataSet GetConfigsResult(int _answerid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getLtmConfigsResults", arParams);
        }
    }
}
