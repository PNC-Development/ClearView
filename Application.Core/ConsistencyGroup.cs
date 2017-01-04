using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;

namespace NCC.ClearView.Application.Core
{
	public class ConsistencyGroups
	{
		private string dsn = "";
		private int user = 0;
		private SqlParameter[] arParams;
        public ConsistencyGroups(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
		}
        public DataSet Gets(int _enabled)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getConsistencyGroups", arParams);
        }
        public DataSet Get(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getConsistencyGroup", arParams);
        }
        public DataSet Get(string _name)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@name", _name);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getConsistencyGroupName", arParams);
        }
        public string Get(int _id, string _column)
        {
            DataSet ds = Get(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public int Add(string _name, int _display, int _enabled)
		{
            DataSet ds = Get(_name);
            if (ds.Tables[0].Rows.Count > 0)
                return 0;
            else
            {
                arParams = new SqlParameter[4];
                arParams[0] = new SqlParameter("@name", _name);
                arParams[1] = new SqlParameter("@display", _display);
                arParams[2] = new SqlParameter("@enabled", _enabled);
                arParams[3] = new SqlParameter("@id", SqlDbType.Int);
                arParams[3].Direction = ParameterDirection.Output;
                SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addConsistencyGroup", arParams);
                return Int32.Parse(arParams[3].Value.ToString());
            }
		}
        public void Update(int _id, string _name, int _enabled)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateConsistencyGroup", arParams);
        }
        public void UpdateOrder(int _id, int _display)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@display", _display);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateConsistencyGroupOrder", arParams);
        }
        public void Enable(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateConsistencyGroupEnabled", arParams);
        }
        public void Delete(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteConsistencyGroup", arParams);
        }
        public DataSet GetServer(string _name)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@name", _name);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getConsistencyGroupServers", arParams);
        }
        public DataSet GetMember(int _dr_consistencyid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@dr_consistencyid", _dr_consistencyid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getConsistencyGroupMembers", arParams);
        }
    }
}
