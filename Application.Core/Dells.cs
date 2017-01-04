using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;

namespace NCC.ClearView.Application.Core
{
    public class Dells
	{
		private string dsn = "";
		private int user = 0;
		private SqlParameter[] arParams;
        public Dells(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
		}
		public DataSet Get(int _id)
		{
			arParams = new SqlParameter[1];
			arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDellConfig", arParams);
		}
		public DataSet Gets(int _enabled)
		{
			arParams = new SqlParameter[1];
			arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDellConfigs", arParams);
		}
        public void Add(string _name, string _xml_split, string _xml_operator, string _xml_start, string _query_power, string _query_mac1, string _query_mac2, string _success_power_on, string _success_power_off, string _username, string _password, int _enabled)
		{
			arParams = new SqlParameter[12];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@xml_split", _xml_split);
            arParams[2] = new SqlParameter("@xml_operator", _xml_operator);
            arParams[3] = new SqlParameter("@xml_start", _xml_start);
            arParams[4] = new SqlParameter("@query_power", _query_power);
            arParams[5] = new SqlParameter("@query_mac1", _query_mac1);
            arParams[6] = new SqlParameter("@query_mac2", _query_mac2);
            arParams[7] = new SqlParameter("@success_power_on", _success_power_on);
            arParams[8] = new SqlParameter("@success_power_off", _success_power_off);
            arParams[9] = new SqlParameter("@username", _username);
            arParams[10] = new SqlParameter("@password", _password);
            arParams[11] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addDellConfig", arParams);
		}
        public void Update(int _id, string _name, string _xml_split, string _xml_operator, string _xml_start, string _query_power, string _query_mac1, string _query_mac2, string _success_power_on, string _success_power_off, string _username, string _password, int _enabled)
		{
			arParams = new SqlParameter[13];
			arParams[0] = new SqlParameter("@id", _id);
			arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@xml_split", _xml_split);
            arParams[3] = new SqlParameter("@xml_operator", _xml_operator);
            arParams[4] = new SqlParameter("@xml_start", _xml_start);
            arParams[5] = new SqlParameter("@query_power", _query_power);
            arParams[6] = new SqlParameter("@query_mac1", _query_mac1);
            arParams[7] = new SqlParameter("@query_mac2", _query_mac2);
            arParams[8] = new SqlParameter("@success_power_on", _success_power_on);
            arParams[9] = new SqlParameter("@success_power_off", _success_power_off);
            arParams[10] = new SqlParameter("@username", _username);
            arParams[11] = new SqlParameter("@password", _password);
            arParams[12] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateDellConfig", arParams);
		}
		public void Enable(int _id, int _enabled) 
		{
			arParams = new SqlParameter[2];
			arParams[0] = new SqlParameter("@id", _id);
			arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateDellConfigEnabled", arParams);
		}
		public void Delete(int _id)
		{
			arParams = new SqlParameter[1];
			arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteDellConfig", arParams);
		}
	}
}
