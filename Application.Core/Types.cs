using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;

namespace NCC.ClearView.Application.Core
{
    public class Types
	{
		private string dsn = "";
		private int user = 0;
		private SqlParameter[] arParams;
        public Types(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
		}
        public DataSet Gets(int _platformid, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@platformid", _platformid);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getTypesPlatform", arParams);
        }
        public DataSet Gets(int _enabled)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getTypes", arParams);
        }
        public int GetPlatform(int _id)
        {
            DataSet ds = Get(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return Int32.Parse(ds.Tables[0].Rows[0]["platformid"].ToString());
            else
                return 0;
        }
        public DataSet Get(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getType", arParams);
        }
        public string Get(int _id, string _column)
        {
            DataSet ds = Get(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public void Add(int _platformid, string _name, string _configuration_path, string _asset_deploy_path, string _design_execution_path, string _forecast_execution_path, string _ondemand_execution_path, string _ondemand_steps_path, int _inventory_warning, int _inventory_critical, int _display, int _enabled)
		{
			arParams = new SqlParameter[12];
            arParams[0] = new SqlParameter("@platformid", _platformid);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@configuration_path", _configuration_path);
            arParams[3] = new SqlParameter("@asset_deploy_path", _asset_deploy_path);
            arParams[4] = new SqlParameter("@design_execution_path", _design_execution_path);
            arParams[5] = new SqlParameter("@forecast_execution_path", _forecast_execution_path);
            arParams[6] = new SqlParameter("@ondemand_execution_path", _ondemand_execution_path);
            arParams[7] = new SqlParameter("@ondemand_steps_path", _ondemand_steps_path);
            arParams[8] = new SqlParameter("@inventory_warning", _inventory_warning);
            arParams[9] = new SqlParameter("@inventory_critical", _inventory_critical);
            arParams[10] = new SqlParameter("@display", _display);
            arParams[11] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addType", arParams);
		}
        public void Update(int _id, int _platformid, string _name, string _configuration_path, string _asset_deploy_path, string _design_execution_path, string _forecast_execution_path, string _ondemand_execution_path, string _ondemand_steps_path, int _inventory_warning, int _inventory_critical, int _enabled)
        {
            arParams = new SqlParameter[12];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@platformid", _platformid);
            arParams[2] = new SqlParameter("@name", _name);
            arParams[3] = new SqlParameter("@configuration_path", _configuration_path);
            arParams[4] = new SqlParameter("@asset_deploy_path", _asset_deploy_path);
            arParams[5] = new SqlParameter("@design_execution_path", _design_execution_path);
            arParams[6] = new SqlParameter("@forecast_execution_path", _forecast_execution_path);
            arParams[7] = new SqlParameter("@ondemand_execution_path", _ondemand_execution_path);
            arParams[8] = new SqlParameter("@ondemand_steps_path", _ondemand_steps_path);
            arParams[9] = new SqlParameter("@inventory_warning", _inventory_warning);
            arParams[10] = new SqlParameter("@inventory_critical", _inventory_critical);
            arParams[11] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateType", arParams);
        }
        public void Update(int _id, int _inventory_warning, int _inventory_critical)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@inventory_warning", _inventory_warning);
            arParams[2] = new SqlParameter("@inventory_critical", _inventory_critical);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateTypeInventory", arParams);
        }
        public void UpdateOrder(int _id, int _display)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@display", _display);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateTypeOrder", arParams);
        }
        public void Enable(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateTypeEnabled", arParams);
        }
        public void Delete(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteType", arParams);
        }
    }
}
