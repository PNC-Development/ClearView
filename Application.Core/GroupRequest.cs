using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.DirectoryServices;

namespace NCC.ClearView.Application.Core
{
	public class GroupRequest
	{
		private string dsn = "";
		private int user = 0;
		private SqlParameter[] arParams;
        public GroupRequest(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
		}
        public DataSet GetMaintenance(int _requestid, string _maintenance)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@maintenance", _maintenance);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getGroupMaintenances", arParams);
        }
        public DataSet GetMaintenance(int _requestid, int _itemid, int _number)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getGroupMaintenance", arParams);
        }
        public void AddMaintenance(int _requestid, int _itemid, int _number, string _maintenance, string _name, int _domain)
        {
            arParams = new SqlParameter[6];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            arParams[3] = new SqlParameter("@maintenance", _maintenance);
            arParams[4] = new SqlParameter("@name", _name);
            arParams[5] = new SqlParameter("@domain", _domain);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addGroupMaintenance", arParams);
        }
        public void UpdateMaintenance(int _requestid, int _itemid, int _number, int _approval, string _reason)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            arParams[3] = new SqlParameter("@approval", _approval);
            arParams[4] = new SqlParameter("@reason", _reason);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateGroupMaintenance", arParams);
        }
        public DataSet GetMaintenanceParameters(int _requestid, int _itemid, int _number)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getGroupMaintenanceParameters", arParams);
        }
        public void AddMaintenanceParameter(int _requestid, int _itemid, int _number, string _value)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            arParams[3] = new SqlParameter("@value", _value);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addGroupMaintenanceParameter", arParams);
        }
        public void DeleteMaintenanceParameters(int _requestid, int _itemid, int _number)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteGroupMaintenanceParameters", arParams);
        }
    }
}
