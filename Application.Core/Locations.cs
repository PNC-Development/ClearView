using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;

namespace NCC.ClearView.Application.Core
{
	public class Locations
	{
		private string dsn = "";
		private int user = 0;
		private SqlParameter[] arParams;
        public Locations(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
		}
        public string GetFull(int _addressid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _addressid);
            object o = SqlHelper.ExecuteScalar(dsn, CommandType.StoredProcedure, "pr_getLocationAddressFull", arParams);
            if (o == null)
                return "Unavailable";
            else
                return o.ToString();
        }
        public DataSet GetState(int _id)
		{
			arParams = new SqlParameter[1];
			arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getLocationState", arParams);
		}
        public string GetState(int _id, string _column)
        {
            DataSet ds = GetState(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public DataSet GetStates(int _enabled)
		{
			arParams = new SqlParameter[1];
			arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getLocationStates", arParams);
		}
        public void AddState(string _name, string _code, int _enabled)
		{
			arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@code", _code);
            arParams[2] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addLocationState", arParams);
		}
        public void UpdateState(int _id, string _name, string _code, int _enabled)
		{
			arParams = new SqlParameter[4];
			arParams[0] = new SqlParameter("@id", _id);
			arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@code", _code);
            arParams[3] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateLocationState", arParams);
		}
        public void UpdateStateOrder(int _id, int _display)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@display", _display);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateLocationStateOrder", arParams);
        }
        public void EnableState(int _id, int _enabled) 
		{
			arParams = new SqlParameter[2];
			arParams[0] = new SqlParameter("@id", _id);
			arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateLocationStateEnabled", arParams);
		}
		public void DeleteState(int _id)
		{
			arParams = new SqlParameter[1];
			arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteLocationState", arParams);
		}

        public DataSet GetCitys(int _stateid, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@stateid", _stateid);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getLocationCitys", arParams);
        }
        public DataSet GetCitys(int _enabled)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getLocationCitysAll", arParams);
        }
        public DataSet GetCity(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getLocationCity", arParams);
        }
        public string GetCity(int _id, string _column)
        {
            DataSet ds = GetCity(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public void AddCity(int _stateid, string _name, string _code, string _zip, string _enclosure_name, int _enabled)
        {
            arParams = new SqlParameter[6];
            arParams[0] = new SqlParameter("@stateid", _stateid);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@code", _code);
            arParams[3] = new SqlParameter("@zip", _zip);
            arParams[4] = new SqlParameter("@enclosure_name", _enclosure_name);
            arParams[5] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addLocationCity", arParams);
        }
        public void UpdateCity(int _id, int _stateid, string _name, string _code, string _zip, string _enclosure_name, int _enabled)
        {
            arParams = new SqlParameter[7];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@stateid", _stateid);
            arParams[2] = new SqlParameter("@name", _name);
            arParams[3] = new SqlParameter("@code", _code);
            arParams[4] = new SqlParameter("@zip", _zip);
            arParams[5] = new SqlParameter("@enclosure_name", _enclosure_name);
            arParams[6] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateLocationCity", arParams);
        }
        public void UpdateCityOrder(int _id, int _display)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@display", _display);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateLocationCityOrder", arParams);
        }
        public void EnableCity(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateLocationCityEnabled", arParams);
        }
        public void DeleteCity(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteLocationCity", arParams);
        }

        public DataSet GetAddresss(int _cityid, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@cityid", _cityid);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getLocationAddresss", arParams);
        }
        public DataSet GetAddresss(int _enabled)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getLocationAddresssAll", arParams);
        }
        public DataSet GetAddresssOrdered(int _enabled)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getLocationAddresssAllOrdered", arParams);
        }
        public DataSet GetAddress(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getLocationAddress", arParams);
        }
        public string GetAddress(int _id, string _column)
        {
            DataSet ds = GetAddress(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public void AddAddress(int _cityid, string _name, string _factory_code, int _common, string _commonname, int _storage, int _tsm, int _dr, int _offsite_build, int _manual_build, string _building_code, string _service_now, int _recovery, int _vmware_ipaddress, int _prod, int _qa, int _test, int _enabled)
        {
            arParams = new SqlParameter[18];
            arParams[0] = new SqlParameter("@cityid", _cityid);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@factory_code", _factory_code);
            arParams[3] = new SqlParameter("@common", _common);
            arParams[4] = new SqlParameter("@commonname", _commonname);
            arParams[5] = new SqlParameter("@storage", _storage);
            arParams[6] = new SqlParameter("@tsm", _tsm);
            arParams[7] = new SqlParameter("@dr", _dr);
            arParams[8] = new SqlParameter("@offsite_build", _offsite_build);
            arParams[9] = new SqlParameter("@manual_build", _manual_build);
            arParams[10] = new SqlParameter("@building_code", _building_code);
            arParams[11] = new SqlParameter("@service_now", _service_now);
            arParams[12] = new SqlParameter("@recovery", _recovery);
            arParams[13] = new SqlParameter("@vmware_ipaddress", _vmware_ipaddress);
            arParams[14] = new SqlParameter("@prod", _prod);
            arParams[15] = new SqlParameter("@qa", _qa);
            arParams[16] = new SqlParameter("@test", _test);
            arParams[17] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addLocationAddress", arParams);
        }
        public void UpdateAddress(int _id, int _cityid, string _name, string _factory_code, int _common, string _commonname, int _storage, int _tsm, int _dr, int _offsite_build, int _manual_build, string _building_code, string _service_now, int _recovery, int _vmware_ipaddress, int _prod, int _qa, int _test, int _enabled)
        {
            arParams = new SqlParameter[19];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@cityid", _cityid);
            arParams[2] = new SqlParameter("@name", _name);
            arParams[3] = new SqlParameter("@factory_code", _factory_code);
            arParams[4] = new SqlParameter("@common", _common);
            arParams[5] = new SqlParameter("@commonname", _commonname);
            arParams[6] = new SqlParameter("@storage", _storage);
            arParams[7] = new SqlParameter("@tsm", _tsm);
            arParams[8] = new SqlParameter("@dr", _dr);
            arParams[9] = new SqlParameter("@offsite_build", _offsite_build);
            arParams[10] = new SqlParameter("@manual_build", _manual_build);
            arParams[11] = new SqlParameter("@building_code", _building_code);
            arParams[12] = new SqlParameter("@service_now", _service_now);
            arParams[13] = new SqlParameter("@recovery", _recovery);
            arParams[14] = new SqlParameter("@vmware_ipaddress", _vmware_ipaddress);
            arParams[15] = new SqlParameter("@prod", _prod);
            arParams[16] = new SqlParameter("@qa", _qa);
            arParams[17] = new SqlParameter("@test", _test);
            arParams[18] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateLocationAddress", arParams);
        }
        public void UpdateAddressOrder(int _id, int _display)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@display", _display);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateLocationAddressOrder", arParams);
        }
        public void EnableAddress(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateLocationAddressEnabled", arParams);
        }
        public void DeleteAddress(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteLocationAddress", arParams);
        }
        public DataSet GetAddressCommon()
        {
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getLocationAddressCommon");
        }
        public DataSet GetAddressClass(int _dr, int _prod, int _qa, int _test)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@dr", _dr);
            arParams[1] = new SqlParameter("@prod", _prod);
            arParams[2] = new SqlParameter("@qa", _qa);
            arParams[3] = new SqlParameter("@test", _test);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getLocationAddressClass", arParams);
        }
        public DataSet GetAddressRecovery()
        {
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getLocationAddressRecovery");
        }

        public string LoadDDL(string _state_id, string _city_id, string _address_id, string _hidden_id, int _current, bool _common, string _common_id)
        {
            return LoadDDL(_state_id, _city_id, _address_id, _hidden_id, _current, _common, _common_id, "-- SELECT --");
        }
        public string LoadDDL(string _state_id, string _city_id, string _address_id, string _hidden_id, int _current, bool _common, string _common_id, string _first_common)
        {
            string strCommon = "";
            bool boolCommonFound = false;
            if (_common == true)
            {
                strCommon = "<td>";
                strCommon += "<select name=\"" + _common_id + "\" id=\"" + _common_id + "\" class=\"default\" onchange=\"UpdateDropDownLocation('" + _common_id + "','" + _hidden_id + "','" + _state_id + "','" + _city_id + "','" + _address_id + "');\">";
                strCommon += "<option value=\"0\">" + _first_common + "</option>";
                DataSet dsCommon = GetAddressCommon();
                foreach (DataRow drCommon in dsCommon.Tables[0].Rows)
                {
                    if (Int32.Parse(drCommon["id"].ToString()) == _current)
                    {
                        boolCommonFound = true;
                        strCommon += "<option value=\"" + drCommon["id"].ToString() + "\" selected=\"selected\">" + drCommon["commonname"].ToString() + "</option>";
                    }
                    else
                        strCommon += "<option value=\"" + drCommon["id"].ToString() + "\">" + drCommon["commonname"].ToString() + "</option>";
                }
                if (boolCommonFound == false && _current > 0)
                    strCommon += "<option value=\"-1\" selected=\"selected\">&nbsp;&nbsp;&nbsp;More...</option>";
                else
                {
                    if (_current == 0)
                        boolCommonFound = true;
                    strCommon += "<option value=\"-1\">&nbsp;&nbsp;&nbsp;More...</option>";
                }
                strCommon += "</select>";
                strCommon += "</td>";
            }
            // ADDRESS
            int intCity = 0;
            string strAddress = "<td>";
            strAddress += "<select name=\"" + _address_id + "\" id=\"" + _address_id + "\"" + (_current == 0 ? " disabled=\"disabled\"" : "") + " class=\"default\" onchange=\"UpdateDropDownHidden('" + _address_id + "','" + _hidden_id + "');\" style=\"display:" + (_common == true && boolCommonFound == true ? "none" : "inline") + "\">";
            if (_current == 0)
                strAddress += "<option value=\"0\">-- Select a City --</option>";
            else
            {
                intCity = Int32.Parse(GetAddress(_current, "cityid"));
                DataSet dsAddress = GetAddresss(intCity, 1);
                foreach (DataRow drAddress in dsAddress.Tables[0].Rows)
                {
                    if (Int32.Parse(drAddress["id"].ToString()) == _current)
                        strAddress += "<option value=\"" + drAddress["id"].ToString() + "\" selected=\"selected\">" + drAddress["name"].ToString() + "</option>";
                    else
                        strAddress += "<option value=\"" + drAddress["id"].ToString() + "\">" + drAddress["name"].ToString() + "</option>";
                }
            }
            strAddress += "</select>";
            strAddress += "</td>";
            // CITY
            int intState = 0;
            string strCity = "<td>";
            strCity += "<select name=\"" + _city_id + "\" id=\"" + _city_id + "\"" + (intCity == 0 ? " disabled=\"disabled\"" : "") + " class=\"default\" onchange=\"PopulateAddresss('" + _city_id + "','" + _address_id + "');ResetDropDownHidden('" + _hidden_id + "');\" style=\"display:" + (_common == true && boolCommonFound == true ? "none" : "inline") + "\">";
            if (intCity == 0)
                strCity += "<option value=\"0\">-- Select a State --</option>";
            else
            {
                intState = Int32.Parse(GetCity(intCity, "stateid"));
                DataSet dsCity = GetCitys(intState, 1);
                foreach (DataRow drCity in dsCity.Tables[0].Rows)
                {
                    if (Int32.Parse(drCity["id"].ToString()) == intCity)
                        strCity += "<option value=\"" + drCity["id"].ToString() + "\" selected=\"selected\">" + drCity["name"].ToString() + "</option>";
                    else
                        strCity += "<option value=\"" + drCity["id"].ToString() + "\">" + drCity["name"].ToString() + "</option>";
                }
            }
            strCity += "</select>";
            strCity += "</td>";
            // STATE
            string strState = "<td>";
            strState += "<select name=\"" + _state_id + "\" id=\"" + _state_id + "\" class=\"default\" onchange=\"PopulateCitys('" + _state_id + "','" + _city_id + "');ResetDropDownHidden('" + _hidden_id + "');\" style=\"display:" + (_common == true && boolCommonFound == true ? "none" : "inline") + "\">";
            strState += "<option value=\"0\">-- SELECT --</option>";
            DataSet dsState = GetStates(1);
            foreach (DataRow drState in dsState.Tables[0].Rows)
            {
                if (Int32.Parse(drState["id"].ToString()) == intState)
                    strState += "<option value=\"" + drState["id"].ToString() + "\" selected=\"selected\">" + drState["name"].ToString() + "</option>";
                else
                    strState += "<option value=\"" + drState["id"].ToString() + "\">" + drState["name"].ToString() + "</option>";
            }
            strState += "</select>";
            strState += "</td>";

            return "<table cellpadding=\"2\" cellspacing=\"1\" border=\"0\"><tr>" + strCommon + strState + strCity + strAddress + "</tr></table><script type=\"text/javascript\">LoadLocationCity('" + _city_id + "');LoadLocationAddress('" + _address_id + "');<" + "/script>";
        }

        public int GetState(string _name)
        {
            DataSet ds = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT * FROM cv_location_state WHERE name = '" + _name + "' AND enabled = 1 AND deleted = 0");
            if (ds.Tables[0].Rows.Count == 0)
                return 0;
            else
                return Int32.Parse(ds.Tables[0].Rows[0]["id"].ToString());
        }
        public int GetCity(string _name)
        {
            DataSet ds = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT * FROM cv_location_city WHERE name = '" + _name + "' AND enabled = 1 AND deleted = 0");
            if (ds.Tables[0].Rows.Count == 0)
                return 0;
            else
                return Int32.Parse(ds.Tables[0].Rows[0]["id"].ToString());
        }
        public int GetAddress(string _name)
        {
            DataSet ds = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT * FROM cv_location_address WHERE name = '" + _name + "' AND enabled = 1 AND deleted = 0");
            if (ds.Tables[0].Rows.Count == 0)
                return 0;
            else
                return Int32.Parse(ds.Tables[0].Rows[0]["id"].ToString());
        }

        public int GetAddressDR()
        {
            DataSet ds = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT top 1 * FROM cv_location_address WHERE DR = 1 AND enabled = 1 AND deleted = 0");
            if (ds.Tables[0].Rows.Count == 0)
                return 0;
            else
                return Int32.Parse(ds.Tables[0].Rows[0]["id"].ToString());
        }

        public bool IsManual(int _addressid)
        {
            return (GetAddress(_addressid, "manual_build") == "1");
        }
        public bool IsOffsite(int _addressid)
        {
            return (GetAddress(_addressid, "offsite_build") == "1");
        }

        public DataSet GetInventory(int? _addressid, int? _roomid, int? _zoneid)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@addressid", _addressid);
            arParams[1] = new SqlParameter("@roomid", _roomid);
            arParams[2] = new SqlParameter("@zoneid", _zoneid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getInventoryFacilities", arParams);
        }
    }
}
