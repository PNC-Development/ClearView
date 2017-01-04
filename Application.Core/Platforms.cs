using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;

namespace NCC.ClearView.Application.Core
{
	public class Platforms
	{
		private string dsn = "";
		private int user = 0;
		private bool logging = false;
		private SqlParameter[] arParams;
		private DataSet ds;
		private Log oLog;
        public Platforms(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
			oLog = new Log(user, dsn);
		}
		public DataSet Get(int _id)
		{
			arParams = new SqlParameter[1];
			arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getPlatform", arParams);
		}
        public string Get(int _id, string _column)
        {
            ds = Get(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public string GetName(int _id)
		{
			string strName = "Unavailable";
			try { strName = Get(_id).Tables[0].Rows[0]["name"].ToString(); }
			catch {}
			return strName;
		}
        public int GetManager(int _id)
        {
            Users oUser = new Users(user, dsn);
            int intManager = 0;
            try
            {
                Int32.TryParse(Get(_id).Tables[0].Rows[0]["managerid"].ToString(), out intManager);
            }
            catch { }
            return intManager;
        }
		public DataSet Gets(int _enabled)
		{
			arParams = new SqlParameter[1];
			arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getPlatforms", arParams);
		}
        public DataSet GetSystems(int _enabled)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getPlatformsSystem", arParams);
        }
        public DataSet GetAssets(int _enabled)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getPlatformsAsset", arParams);
        }
        public DataSet GetForecasts(int _enabled)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getPlatformsForecast", arParams);
        }
        public DataSet GetInventorys(int _userid, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@userid", _userid);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getPlatformsInventory", arParams);
        }
        public void Add(string _name, int _userid, int _managerid, string _image, string _big_image, int _asset, int _forecast, int _system, int _inventory, string _action_form, string _demand_form, string _supply_form, string _order_form, string _order_view_form, string _add_form, string _settings_form, string _forms_form, string _alert_form, int _max_inventory1, int _max_inventory2, int _max_inventory3, int _enabled)
		{
			if (logging == true)
                oLog.Add("Add platform " + _name);
			arParams = new SqlParameter[22];
			arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@userid", _userid);
            arParams[2] = new SqlParameter("@managerid", _managerid);
            arParams[3] = new SqlParameter("@image", _image);
            arParams[4] = new SqlParameter("@big_image", _big_image);
            arParams[5] = new SqlParameter("@asset", _asset);
            arParams[6] = new SqlParameter("@forecast", _forecast);
            arParams[7] = new SqlParameter("@system", _system);
            arParams[8] = new SqlParameter("@inventory", _inventory);
            arParams[9] = new SqlParameter("@action_form", _action_form);
            arParams[10] = new SqlParameter("@demand_form", _demand_form);
            arParams[11] = new SqlParameter("@supply_form", _supply_form);
            arParams[12] = new SqlParameter("@order_form", _order_form);
            arParams[13] = new SqlParameter("@order_view_form", _order_view_form);
            arParams[14] = new SqlParameter("@add_form", _add_form);
            arParams[15] = new SqlParameter("@settings_form", _settings_form);
            arParams[16] = new SqlParameter("@forms_form", _forms_form);
            arParams[17] = new SqlParameter("@alert_form", _alert_form);
            arParams[18] = new SqlParameter("@max_inventory1", _max_inventory1);
            arParams[19] = new SqlParameter("@max_inventory2", _max_inventory2);
            arParams[20] = new SqlParameter("@max_inventory3", _max_inventory3);
            arParams[21] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addPlatform", arParams);
		}
        public void Update(int _id, string _name, int _userid, int _managerid, string _image, string _big_image, int _asset, int _forecast, int _system, int _inventory, string _action_form, string _demand_form, string _supply_form, string _order_form, string _order_view_form, string _add_form, string _settings_form, string _forms_form, string _alert_form, int _max_inventory1, int _max_inventory2, int _max_inventory3, int _enabled)
        {
            if (logging == true)
                oLog.Add("Update platform " + GetName(_id));
            arParams = new SqlParameter[23];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@userid", _userid);
            arParams[3] = new SqlParameter("@managerid", _managerid);
            arParams[4] = new SqlParameter("@image", _image);
            arParams[5] = new SqlParameter("@big_image", _big_image);
            arParams[6] = new SqlParameter("@asset", _asset);
            arParams[7] = new SqlParameter("@forecast", _forecast);
            arParams[8] = new SqlParameter("@system", _system);
            arParams[9] = new SqlParameter("@inventory", _inventory);
            arParams[10] = new SqlParameter("@action_form", _action_form);
            arParams[11] = new SqlParameter("@demand_form", _demand_form);
            arParams[12] = new SqlParameter("@supply_form", _supply_form);
            arParams[13] = new SqlParameter("@order_form", _order_form);
            arParams[14] = new SqlParameter("@order_view_form", _order_view_form);
            arParams[15] = new SqlParameter("@add_form", _add_form);
            arParams[16] = new SqlParameter("@settings_form", _settings_form);
            arParams[17] = new SqlParameter("@forms_form", _forms_form);
            arParams[18] = new SqlParameter("@alert_form", _alert_form);
            arParams[19] = new SqlParameter("@max_inventory1", _max_inventory1);
            arParams[20] = new SqlParameter("@max_inventory2", _max_inventory2);
            arParams[21] = new SqlParameter("@max_inventory3", _max_inventory3);
            arParams[22] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updatePlatform", arParams);
        }
        public void Update(int _id, int _max_inventory1, int _max_inventory2, int _max_inventory3)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@max_inventory1", _max_inventory1);
            arParams[2] = new SqlParameter("@max_inventory2", _max_inventory2);
            arParams[3] = new SqlParameter("@max_inventory3", _max_inventory3);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updatePlatformInventory", arParams);
        }
        public void UpdateOrder(int _id, int _display)
        {
            if (logging == true)
                oLog.Add("Update platform order " + GetName(_id));
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@display", _display);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updatePlatformOrder", arParams);
        }
        public void Enable(int _id, int _enabled) 
		{
			if (logging == true) 
			{
				if (_enabled == 1)
                    oLog.Add("Enable platform " + GetName(_id));
				else
                    oLog.Add("Disable platform " + GetName(_id));
			}
			arParams = new SqlParameter[2];
			arParams[0] = new SqlParameter("@id", _id);
			arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updatePlatformEnabled", arParams);
		}
		public void Delete(int _id)
		{
			if (logging == true)
                oLog.Add("Delete platform " + GetName(_id));
			arParams = new SqlParameter[1];
			arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deletePlatform", arParams);
		}


        public void AddUser(int _platformid, int _userid, int _inventory)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@platformid", _platformid);
            arParams[1] = new SqlParameter("@userid", _userid);
            arParams[2] = new SqlParameter("@inventory", _inventory);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addPlatformUser", arParams);
        }
        public void DeleteUser(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deletePlatformUser", arParams);
        }
        public DataSet GetUsers(int _platformid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@platformid", _platformid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getPlatformUsers", arParams);
        }
        public DataSet GetUser(int _platformid, int _inventory)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@platformid", _platformid);
            arParams[1] = new SqlParameter("@inventory", _inventory);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getPlatformUser", arParams);
        }
        public bool IsManager(int _platformid, int _userid, int _inventory)
        {
            bool boolReturn = false;
            Delegates oDelegate = new Delegates(user, dsn);
            DataSet ds = GetUser(_platformid, _inventory);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (Int32.Parse(dr["userid"].ToString()) == _userid || oDelegate.Get(Int32.Parse(dr["userid"].ToString()), _userid) > 0)
                {
                    boolReturn = true;
                    break;
                }
            }
            return boolReturn;
        }

        public DataSet GetForm(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getPlatformForm", arParams);
        }
        public string GetForm(int _id, string _column)
        {
            ds = GetForm(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public DataSet GetForms(int _platformid, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@platformid", _platformid);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getPlatformForms", arParams);
        }
        public void AddForm(int _platformid, string _name, string _description, string _image, string _path, int _max1, int _max2, int _max3, int _max4, int _max5, int _display, int _enabled)
        {
            arParams = new SqlParameter[12];
            arParams[0] = new SqlParameter("@platformid", _platformid);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@description", _description);
            arParams[3] = new SqlParameter("@image", _image);
            arParams[4] = new SqlParameter("@path", _path);
            arParams[5] = new SqlParameter("@max1", _max1);
            arParams[6] = new SqlParameter("@max2", _max2);
            arParams[7] = new SqlParameter("@max3", _max3);
            arParams[8] = new SqlParameter("@max4", _max4);
            arParams[9] = new SqlParameter("@max5", _max5);
            arParams[10] = new SqlParameter("@display", _display);
            arParams[11] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addPlatformForm", arParams);
        }
        public void UpdateForm(int _id, int _platformid, string _name, string _description, string _image, string _path, int _max1, int _max2, int _max3, int _max4, int _max5, int _enabled)
        {
            arParams = new SqlParameter[12];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@platformid", _platformid);
            arParams[2] = new SqlParameter("@name", _name);
            arParams[3] = new SqlParameter("@description", _description);
            arParams[4] = new SqlParameter("@image", _image);
            arParams[5] = new SqlParameter("@path", _path);
            arParams[6] = new SqlParameter("@max1", _max1);
            arParams[7] = new SqlParameter("@max2", _max2);
            arParams[8] = new SqlParameter("@max3", _max3);
            arParams[9] = new SqlParameter("@max4", _max4);
            arParams[10] = new SqlParameter("@max5", _max5);
            arParams[11] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updatePlatformForm", arParams);
        }
        public void UpdateFormOrder(int _id, int _display)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@display", _display);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updatePlatformFormOrder", arParams);
        }
        public void DeleteForm(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deletePlatformForm", arParams);
        }
    }
}
