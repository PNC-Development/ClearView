using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;

namespace NCC.ClearView.Application.Core
{
	public class Models
	{
		private string dsn = "";
		private int user = 0;
		private SqlParameter[] arParams;
        public Models(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
		}
        public DataSet Gets(int _typeid, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@typeid", _typeid);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getModelsType", arParams);
        }
        public DataSet Gets(int _enabled)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getModels", arParams);
        }
        public int GetType(int _id)
        {
            DataSet ds = Get(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return Int32.Parse(ds.Tables[0].Rows[0]["typeid"].ToString());
            else
                return 0;
        }
        public DataSet Get(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getModel", arParams);
        }
        public string Get(int _id, string _column)
        {
            DataSet ds = Get(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public void Add(int _typeid, string _name, string _displayname, string _make, string _pdf, int _sale, int _grouping, int _hostid, int _destroy, int _ParentModel, int _Slots, int _Us, int _solaris_interfaceid1, int _solaris_interfaceid2, int _solaris_build_typeid, int _boot_groupid, int _powerdown_prod, int _powerdown_test, string _factory_code, string _factory_code_specific, int _display, int _enabled)
        {
            arParams = new SqlParameter[22];
            arParams[0] = new SqlParameter("@typeid", _typeid);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@displayname", _displayname);
            arParams[3] = new SqlParameter("@make", _make);
            arParams[4] = new SqlParameter("@pdf", _pdf);
            arParams[5] = new SqlParameter("@sale", _sale);
            arParams[6] = new SqlParameter("@grouping", _grouping);
            arParams[7] = new SqlParameter("@hostid", _hostid);
            arParams[8] = new SqlParameter("@destroy", _destroy);
            arParams[9] = new SqlParameter("@ParentModel", _ParentModel);
            arParams[10] = new SqlParameter("@Slots", _Slots);
            arParams[11] = new SqlParameter("@Us", _Us);
            arParams[12] = new SqlParameter("@solaris_interfaceid1", _solaris_interfaceid1);
            arParams[13] = new SqlParameter("@solaris_interfaceid2", _solaris_interfaceid2);
            arParams[14] = new SqlParameter("@solaris_build_typeid", _solaris_build_typeid);
            arParams[15] = new SqlParameter("@boot_groupid", _boot_groupid);
            arParams[16] = new SqlParameter("@powerdown_prod", _powerdown_prod);
            arParams[17] = new SqlParameter("@powerdown_test", _powerdown_test);
            arParams[18] = new SqlParameter("@factory_code", _factory_code);
            arParams[19] = new SqlParameter("@factory_code_specific", _factory_code_specific);
            arParams[20] = new SqlParameter("@display", _display);
            arParams[21] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addModel", arParams);
        }
        public void Update(int _id, int _typeid, string _name, string _displayname, string _make, string _pdf, int _sale, int _grouping, int _hostid, int _destroy, int _ParentModel, int _Slots, int _Us, int _solaris_interfaceid1, int _solaris_interfaceid2, int _solaris_build_typeid, int _boot_groupid, int _powerdown_prod, int _powerdown_test, string _factory_code, string _factory_code_specific, int _enabled)
        {
            arParams = new SqlParameter[22];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@typeid", _typeid);
            arParams[2] = new SqlParameter("@name", _name);
            arParams[3] = new SqlParameter("@displayname", _displayname);
            arParams[4] = new SqlParameter("@make", _make);
            arParams[5] = new SqlParameter("@pdf", _pdf);
            arParams[6] = new SqlParameter("@sale", _sale);
            arParams[7] = new SqlParameter("@grouping", _grouping);
            arParams[8] = new SqlParameter("@hostid", _hostid);
            arParams[9] = new SqlParameter("@destroy", _destroy);
            arParams[10] = new SqlParameter("@ParentModel", _ParentModel);
            arParams[11] = new SqlParameter("@Slots", _Slots);
            arParams[12] = new SqlParameter("@Us", _Us);
            arParams[13] = new SqlParameter("@solaris_interfaceid1", _solaris_interfaceid1);
            arParams[14] = new SqlParameter("@solaris_interfaceid2", _solaris_interfaceid2);
            arParams[15] = new SqlParameter("@solaris_build_typeid", _solaris_build_typeid);
            arParams[16] = new SqlParameter("@boot_groupid", _boot_groupid);
            arParams[17] = new SqlParameter("@powerdown_prod", _powerdown_prod);
            arParams[18] = new SqlParameter("@powerdown_test", _powerdown_test);
            arParams[19] = new SqlParameter("@factory_code", _factory_code);
            arParams[20] = new SqlParameter("@factory_code_specific", _factory_code_specific);
            arParams[21] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateModel", arParams);
        }
        public void UpdateOrder(int _id, int _display)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@display", _display);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateModelOrder", arParams);
        }
        public void Enable(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateModelEnabled", arParams);
        }
        public void Delete(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteModel", arParams);
        }

        public DataSet GetLocation(int _classid, int _environmentid, int _addressid, int _typeid)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@classid", _classid);
            arParams[1] = new SqlParameter("@environmentid", _environmentid);
            arParams[2] = new SqlParameter("@addressid", _addressid);
            arParams[3] = new SqlParameter("@typeid", _typeid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getModelLocations", arParams);
        }

        public int AddReservation(int _modelid, int _classid, int _environmentid)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@modelid", _modelid);
            arParams[1] = new SqlParameter("@classid", _classid);
            arParams[2] = new SqlParameter("@environmentid", _environmentid);
            arParams[3] = new SqlParameter("@id", SqlDbType.Int);
            arParams[3].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addModelReservation", arParams);
            return Int32.Parse(arParams[3].Value.ToString());
        }
        public DataSet GetReservation(int _modelid, int _classid, int _environmentid)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@modelid", _modelid);
            arParams[1] = new SqlParameter("@classid", _classid);
            arParams[2] = new SqlParameter("@environmentid", _environmentid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getModelReservation", arParams);
        }


        public void AddReservationList(int _parent, int _classid, int _environmentid, int _use_removable, int _display)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@parent", _parent);
            arParams[1] = new SqlParameter("@classid", _classid);
            arParams[2] = new SqlParameter("@environmentid", _environmentid);
            arParams[3] = new SqlParameter("@use_removable", _use_removable);
            arParams[4] = new SqlParameter("@display", _display);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addModelReservationList", arParams);
        }
        public void UpdateReservationList(int _id, int _display)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@display", _display);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateModelReservationList", arParams);
        }
        public void DeleteReservationList(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteModelReservationList", arParams);
        }
        public DataSet GetReservationLists(int _modelid, int _classid, int _environmentid)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@modelid", _modelid);
            arParams[1] = new SqlParameter("@classid", _classid);
            arParams[2] = new SqlParameter("@environmentid", _environmentid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getModelReservationLists", arParams);
        }

        public DataSet GetBootGroups(int _enabled)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getModelsBootGroups", arParams);
        }
        public DataSet GetBootGroup(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getModelsBootGroup", arParams);
        }
        public string GetBootGroup(int _id, string _column)
        {
            DataSet ds = GetBootGroup(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public void AddBootGroup(string _name, string _regular, string _username, string _password, string _return_to_alom, string _mac_query_command, string _mac_query_substring_start, int _enabled)
        {
            arParams = new SqlParameter[8];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@regular", _regular);
            arParams[2] = new SqlParameter("@username", _username);
            arParams[3] = new SqlParameter("@password", _password);
            arParams[4] = new SqlParameter("@return_to_alom", _return_to_alom);
            arParams[5] = new SqlParameter("@mac_query_command", _mac_query_command);
            arParams[6] = new SqlParameter("@mac_query_substring_start", _mac_query_substring_start);
            arParams[7] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addModelsBootGroup", arParams);
        }
        public void UpdateBootGroup(int _id, string _name, string _regular, string _username, string _password, string _return_to_alom, string _mac_query_command, string _mac_query_substring_start, int _enabled)
        {
            arParams = new SqlParameter[9];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@regular", _regular);
            arParams[3] = new SqlParameter("@username", _username);
            arParams[4] = new SqlParameter("@password", _password);
            arParams[5] = new SqlParameter("@return_to_alom", _return_to_alom);
            arParams[6] = new SqlParameter("@mac_query_command", _mac_query_command);
            arParams[7] = new SqlParameter("@mac_query_substring_start", _mac_query_substring_start);
            arParams[8] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateModelsBootGroup", arParams);
        }
        public void EnableBootGroup(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateModelsBootGroupEnabled", arParams);
        }
        public void DeleteBootGroup(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteModelsBootGroup", arParams);
        }

        public DataSet GetBootGroupSteps(int _parent, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@parent", _parent);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getModelBootGroupSteps", arParams);
        }
        public DataSet GetBootGroupStep(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getModelBootGroupStep", arParams);
        }
        public string GetBootGroupStep(int _id, string _column)
        {
            DataSet ds = GetBootGroupStep(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public void AddBootGroupStep(int _parent, string _wait_for, string _then_write, int _immediately, int _timeout, int _power, int _display, int _enabled)
        {
            arParams = new SqlParameter[8];
            arParams[0] = new SqlParameter("@parent", _parent);
            arParams[1] = new SqlParameter("@wait_for", _wait_for);
            arParams[2] = new SqlParameter("@then_write", _then_write);
            arParams[3] = new SqlParameter("@immediately", _immediately);
            arParams[4] = new SqlParameter("@timeout", _timeout);
            arParams[5] = new SqlParameter("@power", _power);
            arParams[6] = new SqlParameter("@display", _display);
            arParams[7] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addModelBootGroupStep", arParams);
        }
        public void UpdateBootGroupStep(int _id, int _display)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@display", _display);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateModelBootGroupStep", arParams);
        }
        public void DeleteBootGroupStep(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteModelBootGroupStep", arParams);
        }
        public DataSet GetBootGroupStepNext(int _modelid, int _serverid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@modelid", _modelid);
            arParams[1] = new SqlParameter("@serverid", _serverid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getModelBootGroupStepNext", arParams);
        }
        public void DeleteBootGroupSteps(int _modelid, int _serverid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@modelid", _modelid);
            arParams[1] = new SqlParameter("@serverid", _serverid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteModelBootGroupSteps", arParams);
        }


        public DataSet GetChipsets(int _enabled)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getModelChipsets", arParams);
        }
        public DataSet GetChipset(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getModelChipset", arParams);
        }
        public string GetChipset(int _id, string _column)
        {
            DataSet ds = GetChipset(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public void AddChipset(string _name, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addModelChipset", arParams);
        }
        public void UpdateChipset(int _id, string _name, int _enabled)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateModelChipset", arParams);
        }
        public void EnableChipset(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateModelChipsetEnabled", arParams);
        }
        public void DeleteChipset(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteModelChipset", arParams);
        }
    }
}
