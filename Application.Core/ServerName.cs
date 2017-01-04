using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Web.UI.WebControls;

namespace NCC.ClearView.Application.Core
{
    public class ServerName
	{
		private string dsn = "";
		private int user = 0;
		private SqlParameter[] arParams;
        private Functions oFunction;
        private string strNotify = "";
        public ServerName(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
            oFunction = new Functions(user, dsn, 4);
		}
        public DataSet GetCodes(int _addressid, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@addressid", _addressid);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerNameCodes", arParams);
        }
        public DataSet GetCodeClasses(int _addressid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@addressid", _addressid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerNameCodesClass", arParams);
        }
        public DataSet GetCodeEnvironments(int _addressid, int _classid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@addressid", _addressid);
            arParams[1] = new SqlParameter("@classid", _classid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerNameCodesEnvironment", arParams);
        }
        public DataSet GetCode(int _addressid, int _classid, int _environmentid)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@addressid", _addressid);
            arParams[1] = new SqlParameter("@classid", _classid);
            arParams[2] = new SqlParameter("@environmentid", _environmentid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerNameCode", arParams);
        }
        public DataSet GetCode(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerNameCodeId", arParams);
        }
        public void AddCode(string _sitecode, int _classid, int _environmentid, int _addressid, int _enabled)
		{
			arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@sitecode", _sitecode);
            arParams[1] = new SqlParameter("@classid", _classid);
            arParams[2] = new SqlParameter("@environmentid", _environmentid);
            arParams[3] = new SqlParameter("@addressid", _addressid);
            arParams[4] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addServerNameCode", arParams);
		}
        public void UpdateCode(int _id, string _sitecode, int _classid, int _environmentid, int _addressid, int _enabled)
        {
            arParams = new SqlParameter[6];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@sitecode", _sitecode);
            arParams[2] = new SqlParameter("@classid", _classid);
            arParams[3] = new SqlParameter("@environmentid", _environmentid);
            arParams[4] = new SqlParameter("@addressid", _addressid);
            arParams[5] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServerNameCode", arParams);
        }
        public void EnableCode(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServerNameCodeEnabled", arParams);
        }
        public void DeleteCode(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteServerNameCode", arParams);
        }


        public int Add(int _classid, int _environmentid, int _addressid, string _prefix2, int _userid, string _name, int _number, string _dsn_service_editor)
        {
            // Used for generation (from custom functions and auto-provisioning)
            DataSet ds = GetCode(_addressid, _classid, _environmentid);
            if (ds.Tables[0].Rows.Count >= _number)
            {
                int intRow = _number - 1;
                int _codeid = Int32.Parse(ds.Tables[0].Rows[intRow]["id"].ToString());
                string _prefix1 = ds.Tables[0].Rows[intRow]["statecity"].ToString();
                string _sitecode = ds.Tables[0].Rows[intRow]["sitecode"].ToString();
                int intID = AddNew(_classid, _environmentid, _codeid, _prefix1, _prefix2, _sitecode, _userid, _name, _dsn_service_editor);
                if (intID == -1)
                    return Add(_classid, _environmentid, _addressid, _prefix2, _userid, _name, _number + 1, _dsn_service_editor);
                else
                    return intID;
            }
            else
                return -1;
        }
        public int Add(int _id, string _name, int _userid, string _dsn_service_editor)
        {
            // Used for duplication (from custom functions)
            DataSet ds = Get(_id);
            if (ds.Tables[0].Rows.Count > 0)
            {
                int _codeid = Int32.Parse(ds.Tables[0].Rows[0]["codeid"].ToString());
                string _prefix2 = ds.Tables[0].Rows[0]["prefix2"].ToString();
                if (_name == "")
                    _name = ds.Tables[0].Rows[0]["name"].ToString();
                DataSet dsCode = GetCode(_codeid);
                if (dsCode.Tables[0].Rows.Count > 0)
                {
                    int _addressid = Int32.Parse(dsCode.Tables[0].Rows[0]["addressid"].ToString());
                    int _classid = Int32.Parse(dsCode.Tables[0].Rows[0]["classid"].ToString());
                    int _environmentid = Int32.Parse(dsCode.Tables[0].Rows[0]["environmentid"].ToString());
                    return Add(_classid, _environmentid, _addressid, _prefix2, _userid, _name, 1, _dsn_service_editor);
                }
                else
                    return 0;
            }
            else
                return 0;
        }
        private int AddNew(int _classid, int _environmentid, int _codeid, string _prefix1, string _prefix2, string _sitecode, int _userid, string _name, string _dsn_service_editor)
        {
            DataSet ds = Get(_prefix1, _prefix2, _sitecode, 0);
            // NO O's and no I's
            string[] aLetters = new string[33] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F", "G", "H", "J", "K", "M", "N", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            int intID = 0;
            // Loop through all the variations of names for the sitecode. Check if it exists. If not, add it and set intID to break loop and return.
            for (int intFirst = 0; intFirst < 33 && intID == 0; intFirst++)
            {
                for (int intSecond = 0; intSecond < 33 && intID == 0; intSecond++)
                {
                    string strName1 = aLetters[intFirst];
                    string strName2 = aLetters[intSecond];
                    DataTable dt = ds.Tables[0];
                    DataRow[] dr = dt.Select("name1 = '" + strName1 + "' AND name2 = '" + strName2 + "'");
                    if (dr.Length == 0)
                    {
                        string strNewName = _prefix1 + _prefix2 + _sitecode + strName1 + strName2;
                        if (Manual(strNewName, _dsn_service_editor) == true)
                            Add(_codeid, _prefix1, _prefix2, _sitecode, strName1, strName2, 0, "Manual Process", 0);
                        else if (oFunction.Ping(strNewName, _classid, _environmentid) == true)
                            Add(_codeid, _prefix1, _prefix2, _sitecode, strName1, strName2, 0, "RESPONSE!!", 0);
                        else
                            intID = Add(_codeid, _prefix1, _prefix2, _sitecode, strName1, strName2, _userid, _name, 0);
                    }
                }
            }
            if (intID == 0)
            {
                // An unused name was not found. Check for names that have been used but are now available for reuse (available == 1).
                ds = Get(_prefix1, _prefix2, _sitecode, 1);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    intID = Int32.Parse(ds.Tables[0].Rows[0]["id"].ToString());
                    Update(intID, 0);
                    Update(intID, _userid, _name);
                }
            }
            return intID;
        }
        public int Add(int _codeid, string _prefix1, string _prefix2, string _sitecode, string _name1, string _name2, int _userid, string _name, int _available)
        {
            arParams = new SqlParameter[10];
            arParams[0] = new SqlParameter("@codeid", _codeid);
            arParams[1] = new SqlParameter("@prefix1", _prefix1);
            arParams[2] = new SqlParameter("@prefix2", _prefix2);
            arParams[3] = new SqlParameter("@sitecode", _sitecode);
            arParams[4] = new SqlParameter("@name1", _name1);
            arParams[5] = new SqlParameter("@name2", _name2);
            arParams[6] = new SqlParameter("@userid", _userid);
            arParams[7] = new SqlParameter("@name", _name);
            arParams[8] = new SqlParameter("@available", _available);
            arParams[9] = new SqlParameter("@id", SqlDbType.Int);
            arParams[9].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addServerName", arParams);
            return Int32.Parse(arParams[9].Value.ToString());
        }
        public void Update(int _id, int _available)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@available", _available);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServerNameAvailable", arParams);
        }
        public void Update(int _id, string _serial)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@serial", _serial);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServerNameSerial", arParams);
        }
        public void Update(int _id, int _userid, string _name)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@userid", _userid);
            arParams[2] = new SqlParameter("@name", _name);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServerName", arParams);
        }
        public DataSet Get(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerName", arParams);
        }
        public string Get(int _id, string _column)
        {
            DataSet ds = Get(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public int GetName(string _name)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@name", _name);
            object o = SqlHelper.ExecuteScalar(dsn, CommandType.StoredProcedure, "pr_getServerNameByName", arParams);
            if (o == null || o.ToString() == "")
                return 0;
            else
                return Int32.Parse(o.ToString());
        }
        public string GetName(int _id, int _userid)
        {
            DataSet ds = Get(_id);
            if (ds.Tables[0].Rows.Count > 0)
            {
                int intCode = Int32.Parse(ds.Tables[0].Rows[0]["codeid"].ToString());
                int intUser = Int32.Parse(ds.Tables[0].Rows[0]["userid"].ToString());
                if (_userid != intUser && _userid > 0)
                    return "DENIED";
                string _sitecode = ds.Tables[0].Rows[0]["sitecode"].ToString();
                string _prefix1 = ds.Tables[0].Rows[0]["prefix1"].ToString();
                string _prefix2 = ds.Tables[0].Rows[0]["prefix2"].ToString();
                string _name = ds.Tables[0].Rows[0]["name1"].ToString() + ds.Tables[0].Rows[0]["name2"].ToString();
                return _prefix1 + _prefix2 + _sitecode + _name;
            }
            else
                return "Unavailable";
        }
        public DataSet Get(string _prefix1, string _prefix2, string _sitecode, int _available)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@prefix1", _prefix1);
            arParams[1] = new SqlParameter("@prefix2", _prefix2);
            arParams[2] = new SqlParameter("@sitecode", _sitecode);
            arParams[3] = new SqlParameter("@available", _available);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerNames", arParams);
        }
        public DataSet GetMine(int _userid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@userid", _userid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerNamesMine", arParams);
        }
        public DataSet GetMineUnused(int _userid, int _pnc)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@userid", _userid);
            arParams[1] = new SqlParameter("@pnc", _pnc);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerNamesMineUnused", arParams);
        }


        #region Applications
        public DataSet GetApplications(int _enabled)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerNameApplications", arParams);
        }
        public DataSet GetApplicationsForecast(int _enabled)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerNameApplicationsForecast", arParams);
        }
        public DataSet GetApplication(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerNameApplication", arParams);
        }
        public string GetApplication(int _id, string _column)
        {
            DataSet ds = GetApplication(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public void AddApplication(string _name, string _code, string _factory_code, string _factory_code_specific, string _zeus_array_config, string _zeus_os, string _zeus_os_version, string _zeus_build_type, string _ad_move_location, int _forecast, int _permit_no_replication, int _display, int _solutioncode, int _enabled)
        {
            arParams = new SqlParameter[14];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@code", _code);
            arParams[2] = new SqlParameter("@factory_code", _factory_code);
            arParams[3] = new SqlParameter("@factory_code_specific", _factory_code_specific);
            arParams[4] = new SqlParameter("@zeus_array_config", _zeus_array_config);
            arParams[5] = new SqlParameter("@zeus_os", _zeus_os);
            arParams[6] = new SqlParameter("@zeus_os_version", _zeus_os_version);
            arParams[7] = new SqlParameter("@zeus_build_type", _zeus_build_type);
            arParams[8] = new SqlParameter("@ad_move_location", _ad_move_location);
            arParams[9] = new SqlParameter("@forecast", _forecast);
            arParams[10] = new SqlParameter("@permit_no_replication", _permit_no_replication);
            arParams[11] = new SqlParameter("@solutioncode", _solutioncode);
            arParams[12] = new SqlParameter("@display", _display);
            arParams[13] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addServerNameApplication", arParams);
        }
        public void UpdateApplication(int _id, string _name, string _code, string _factory_code, string _factory_code_specific, string _zeus_array_config, string _zeus_os, string _zeus_os_version, string _zeus_build_type, string _ad_move_location, int _forecast, int _permit_no_replication, int _solutioncode, int _enabled)
        {
            arParams = new SqlParameter[14];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@code", _code);
            arParams[3] = new SqlParameter("@factory_code", _factory_code);
            arParams[4] = new SqlParameter("@factory_code_specific", _factory_code_specific);
            arParams[5] = new SqlParameter("@zeus_array_config", _zeus_array_config);
            arParams[6] = new SqlParameter("@zeus_os", _zeus_os);
            arParams[7] = new SqlParameter("@zeus_os_version", _zeus_os_version);
            arParams[8] = new SqlParameter("@zeus_build_type", _zeus_build_type);
            arParams[9] = new SqlParameter("@ad_move_location", _ad_move_location);
            arParams[10] = new SqlParameter("@forecast", _forecast);
            arParams[11] = new SqlParameter("@permit_no_replication", _permit_no_replication);
            arParams[12] = new SqlParameter("@enabled", _enabled);
            arParams[13] = new SqlParameter("@solutioncode", _solutioncode);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServerNameApplication", arParams);
        }
        public void UpdateApplicationOrder(int _id, int _display)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@display", _display);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServerNameApplicationOrder", arParams);
        }
        public void EnableApplication(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServerNameApplicationEnabled", arParams);
        }
        public void DeleteApplication(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteServerNameApplication", arParams);
        }
        #endregion

        #region SubApplications
        public DataSet GetSubApplications(int _enabled)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerNameSubApplicationsAll", arParams);
        }
        public DataSet GetSubApplications(int _applicationid, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@applicationid", _applicationid);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerNameSubApplications", arParams);
        }
        public DataSet GetSubApplication(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerNameSubApplication", arParams);
        }
        public string GetSubApplication(int _id, string _column)
        {
            DataSet ds = GetSubApplication(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public int AddSubApplication(int _applicationid, string _name, string _code, string _factory_code, string _factory_code_specific, string _zeus_array_config, string _zeus_os, string _zeus_os_version, string _zeus_build_type, string _ad_move_location, int _permit_no_replication, int _solutioncode, int _networks, int _display, int _enabled)
        {
            arParams = new SqlParameter[16];
            arParams[0] = new SqlParameter("@applicationid", _applicationid);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@code", _code);
            arParams[3] = new SqlParameter("@factory_code", _factory_code);
            arParams[4] = new SqlParameter("@factory_code_specific", _factory_code_specific);
            arParams[5] = new SqlParameter("@zeus_array_config", _zeus_array_config);
            arParams[6] = new SqlParameter("@zeus_os", _zeus_os);
            arParams[7] = new SqlParameter("@zeus_os_version", _zeus_os_version);
            arParams[8] = new SqlParameter("@zeus_build_type", _zeus_build_type);
            arParams[9] = new SqlParameter("@ad_move_location", _ad_move_location);
            arParams[10] = new SqlParameter("@permit_no_replication", _permit_no_replication);
            arParams[11] = new SqlParameter("@solutioncode", _solutioncode);
            arParams[12] = new SqlParameter("@networks", _networks);
            arParams[13] = new SqlParameter("@display", _display);
            arParams[14] = new SqlParameter("@enabled", _enabled);
            arParams[15] = new SqlParameter("@id", SqlDbType.Int);
            arParams[15].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addServerNameSubApplication", arParams);
            return Int32.Parse(arParams[15].Value.ToString());
        }
        public void UpdateSubApplication(int _id, int _applicationid, string _name, string _code, string _factory_code, string _factory_code_specific, string _zeus_array_config, string _zeus_os, string _zeus_os_version, string _zeus_build_type, string _ad_move_location, int _permit_no_replication, int _solutioncode, int _networks, int _enabled)
        {
            arParams = new SqlParameter[15];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@applicationid", _applicationid);
            arParams[2] = new SqlParameter("@name", _name);
            arParams[3] = new SqlParameter("@code", _code);
            arParams[4] = new SqlParameter("@factory_code", _factory_code);
            arParams[5] = new SqlParameter("@factory_code_specific", _factory_code_specific);
            arParams[6] = new SqlParameter("@zeus_array_config", _zeus_array_config);
            arParams[7] = new SqlParameter("@zeus_os", _zeus_os);
            arParams[8] = new SqlParameter("@zeus_os_version", _zeus_os_version);
            arParams[9] = new SqlParameter("@zeus_build_type", _zeus_build_type);
            arParams[10] = new SqlParameter("@ad_move_location", _ad_move_location);
            arParams[11] = new SqlParameter("@permit_no_replication", _permit_no_replication);
            arParams[12] = new SqlParameter("@solutioncode", _solutioncode);
            arParams[13] = new SqlParameter("@networks", _networks);
            arParams[14] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServerNameSubApplication", arParams);
        }
        public void UpdateSubApplicationOrder(int _id, int _display)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@display", _display);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServerNameSubApplicationOrder", arParams);
        }
        public void EnableSubApplication(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServerNameSubApplicationEnabled", arParams);
        }
        public void DeleteSubApplication(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteServerNameSubApplication", arParams);
        }

        public void AddSubApplicationNetwork(int _subapplicationid, int _networkid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@subapplicationid", _subapplicationid);
            arParams[1] = new SqlParameter("@networkid", _networkid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addServerNameSubApplicationNetwork", arParams);
        }
        public void DeleteSubApplicationNetwork(int _subapplicationid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@subapplicationid", _subapplicationid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteServerNameSubApplicationNetwork", arParams);
        }
        public DataSet GetSubApplicationNetworks(int _subapplicationid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@subapplicationid", _subapplicationid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerNameSubApplicationNetworks", arParams);
        }
        #endregion

        #region Components
        public DataSet GetComponentsAvailable(int _classid, int _environmentid, int _modelid, int _osid, int _spid)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@classid", _classid);
            arParams[1] = new SqlParameter("@environmentid", _environmentid);
            arParams[2] = new SqlParameter("@modelid", _modelid);
            arParams[3] = new SqlParameter("@osid", _osid);
            arParams[4] = new SqlParameter("@spid", _spid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerNameComponentDetailsAvailable", arParams);
        }
        public DataSet GetComponents(int _enabled)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerNameComponents", arParams);
        }
        public DataSet GetComponent(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerNameComponent", arParams);
        }
        public string GetComponent(int _id, string _column)
        {
            DataSet ds = GetComponent(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public void AddComponent(string _name, string _code, string _factory_code, string _factory_code_specific, string _zeus_code, int _iis, int _web, int _sql, int _dbase, int _reset_storage, int _display, int _enabled)
        {
            arParams = new SqlParameter[12];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@code", _code);
            arParams[2] = new SqlParameter("@factory_code", _factory_code);
            arParams[3] = new SqlParameter("@factory_code_specific", _factory_code_specific);
            arParams[4] = new SqlParameter("@zeus_code", _zeus_code);
            arParams[5] = new SqlParameter("@iis", _iis);
            arParams[6] = new SqlParameter("@web", _web);
            arParams[7] = new SqlParameter("@sql", _sql);
            arParams[8] = new SqlParameter("@dbase", _dbase);
            arParams[9] = new SqlParameter("@reset_storage", _reset_storage);
            arParams[10] = new SqlParameter("@display", _display);
            arParams[11] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addServerNameComponent", arParams);
        }
        public void UpdateComponent(int _id, string _name, string _code, string _factory_code, string _factory_code_specific, string _zeus_code, int _iis, int _web, int _sql, int _dbase, int _reset_storage, int _enabled)
        {
            arParams = new SqlParameter[12];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@code", _code);
            arParams[3] = new SqlParameter("@factory_code", _factory_code);
            arParams[4] = new SqlParameter("@factory_code_specific", _factory_code_specific);
            arParams[5] = new SqlParameter("@zeus_code", _zeus_code);
            arParams[6] = new SqlParameter("@iis", _iis);
            arParams[7] = new SqlParameter("@web", _web);
            arParams[8] = new SqlParameter("@sql", _sql);
            arParams[9] = new SqlParameter("@dbase", _dbase);
            arParams[10] = new SqlParameter("@reset_storage", _reset_storage);
            arParams[11] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServerNameComponent", arParams);
        }
        public void UpdateComponentOrder(int _id, int _display)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@display", _display);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServerNameComponentOrder", arParams);
        }
        public void EnableComponent(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServerNameComponentEnabled", arParams);
        }
        public void DeleteComponent(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteServerNameComponent", arParams);
        }

        public int AddComponentDetail(int _componentid, string _name, int _zeus_array_config_id, int _zeus_build_type_id, int _approval, int _models, int _networks, int _install, int _mount, string _network_path, string _install_path, string _ad_move_location, int _scriptid, int _environment, int _enabled)
        {
            arParams = new SqlParameter[16];
            arParams[0] = new SqlParameter("@componentid", _componentid);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@zeus_array_config_id", _zeus_array_config_id);
            arParams[3] = new SqlParameter("@zeus_build_type_id", _zeus_build_type_id);
            arParams[4] = new SqlParameter("@approval", _approval);
            arParams[5] = new SqlParameter("@models", _models);
            arParams[6] = new SqlParameter("@networks", _networks);
            arParams[7] = new SqlParameter("@install", _install);
            arParams[8] = new SqlParameter("@mount", _mount);
            arParams[9] = new SqlParameter("@network_path", _network_path);
            arParams[10] = new SqlParameter("@install_path", _install_path);
            arParams[11] = new SqlParameter("@ad_move_location", _ad_move_location);
            arParams[12] = new SqlParameter("@scriptid", _scriptid);
            arParams[13] = new SqlParameter("@environment", _environment);
            arParams[14] = new SqlParameter("@enabled", _enabled);
            arParams[15] = new SqlParameter("@id", SqlDbType.Int);
            arParams[15].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addServerNameComponentDetail", arParams);
            return Int32.Parse(arParams[15].Value.ToString());
        }
        public void UpdateComponentDetail(int _id, int _componentid, string _name, int _zeus_array_config_id, int _zeus_build_type_id, int _approval, int _models, int _networks, int _install, int _mount, string _network_path, string _install_path, string _ad_move_location, int _scriptid, int _environment, int _enabled)
        {
            arParams = new SqlParameter[16];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@componentid", _componentid);
            arParams[2] = new SqlParameter("@name", _name);
            arParams[3] = new SqlParameter("@zeus_array_config_id", _zeus_array_config_id);
            arParams[4] = new SqlParameter("@zeus_build_type_id", _zeus_build_type_id);
            arParams[5] = new SqlParameter("@approval", _approval);
            arParams[6] = new SqlParameter("@models", _models);
            arParams[7] = new SqlParameter("@networks", _networks);
            arParams[8] = new SqlParameter("@install", _install);
            arParams[9] = new SqlParameter("@mount", _mount);
            arParams[10] = new SqlParameter("@network_path", _network_path);
            arParams[11] = new SqlParameter("@install_path", _install_path);
            arParams[12] = new SqlParameter("@ad_move_location", _ad_move_location);
            arParams[13] = new SqlParameter("@scriptid", _scriptid);
            arParams[14] = new SqlParameter("@environment", _environment);
            arParams[15] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServerNameComponentDetail", arParams);
        }
        public void DeleteComponentDetail(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteServerNameComponentDetail", arParams);
        }
        public DataSet GetComponentDetail(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerNameComponentDetail", arParams);
        }
        public string GetComponentDetail(int _id, string _column)
        {
            DataSet ds = GetComponentDetail(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public DataSet GetComponentDetails(int _componentid, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@componentid", _componentid);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerNameComponentDetails", arParams);
        }
        public string GetComponentDetailName(int _id)
        {
            int intComponent = 0;
            if (Int32.TryParse(GetComponentDetail(_id, "componentid"), out intComponent) == true)
                return GetComponent(intComponent, "name");
            else
                return "N/A";
        }

        public void AddComponentDetailCE(int _detailid, int _classid, int _environmentid)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@detailid", _detailid);
            arParams[1] = new SqlParameter("@classid", _classid);
            arParams[2] = new SqlParameter("@environmentid", _environmentid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addServerNameComponentDetailCE", arParams);
        }
        public void DeleteComponentDetailCE(int _detailid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@detailid", _detailid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteServerNameComponentDetailCE", arParams);
        }
        public DataSet GetComponentDetailCEs(int _detailid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@detailid", _detailid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerNameComponentDetailCEs", arParams);
        }

        public void AddComponentDetailModel(int _detailid, int _modelid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@detailid", _detailid);
            arParams[1] = new SqlParameter("@modelid", _modelid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addServerNameComponentDetailModel", arParams);
        }
        public void DeleteComponentDetailModel(int _detailid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@detailid", _detailid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteServerNameComponentDetailModel", arParams);
        }
        public DataSet GetComponentDetailModels(int _detailid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@detailid", _detailid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerNameComponentDetailModels", arParams);
        }

        public void AddComponentDetailOsSp(int _detailid, int _osid, int _spid)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@detailid", _detailid);
            arParams[1] = new SqlParameter("@osid", _osid);
            arParams[2] = new SqlParameter("@spid", _spid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addServerNameComponentDetailOsSp", arParams);
        }
        public void DeleteComponentDetailOsSp(int _detailid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@detailid", _detailid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteServerNameComponentDetailOsSp", arParams);
        }
        public DataSet GetComponentDetailOsSps(int _detailid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@detailid", _detailid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerNameComponentDetailOsSps", arParams);
        }

        public void AddComponentDetailNetwork(int _detailid, int _networkid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@detailid", _detailid);
            arParams[1] = new SqlParameter("@networkid", _networkid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addServerNameComponentDetailNetwork", arParams);
        }
        public void DeleteComponentDetailNetwork(int _detailid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@detailid", _detailid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteServerNameComponentDetailNetwork", arParams);
        }
        public DataSet GetComponentDetailNetworks(int _detailid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@detailid", _detailid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerNameComponentDetailNetworks", arParams);
        }

        public void AddComponentDetailUser(int _detailid, int _userid, int _enabled)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@detailid", _detailid);
            arParams[1] = new SqlParameter("@userid", _userid);
            arParams[2] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addServerNameComponentDetailUser", arParams);
        }
        public void DeleteComponentDetailUser(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteServerNameComponentDetailUser", arParams);
        }
        public DataSet GetComponentDetailUsers(int _detailid, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@detailid", _detailid);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerNameComponentDetailUsers", arParams);
        }
        public void EnableComponentDetailUser(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServerNameComponentDetailUserEnabled", arParams);
        }

        public void AddComponentDetailUserApproval(int _serverid, int _detailid, int _userid, int _approved, string _comments, string _license)
        {
            arParams = new SqlParameter[6];
            arParams[0] = new SqlParameter("@serverid", _serverid);
            arParams[1] = new SqlParameter("@detailid", _detailid);
            arParams[2] = new SqlParameter("@userid", _userid);
            arParams[3] = new SqlParameter("@approved", _approved);
            arParams[4] = new SqlParameter("@comments", _comments);
            arParams[5] = new SqlParameter("@license", _license);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addServerNameComponentDetailUserApproval", arParams);
        }
        public DataSet GetComponentDetailUserApprovalsByUser(int _userid, int _answerid, int _done)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@userid", _userid);
            arParams[1] = new SqlParameter("@answerid", _answerid);
            arParams[2] = new SqlParameter("@done", _done);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerNameComponentDetailUserApprovalsByUser", arParams);
        }
        public DataSet GetComponentDetailUserApprovalsByServer(int _serverid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@serverid", _serverid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerNameComponentDetailUserApprovalsByServer", arParams);
        }
        public DataSet GetComponentDetailUserApprovals(int _serverid, int _detailid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@serverid", _serverid);
            arParams[1] = new SqlParameter("@detailid", _detailid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerNameComponentDetailUserApprovals", arParams);
        }
        public void DeleteComponentDetailUserApproval(int _serverid, int _detailid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@serverid", _serverid);
            arParams[1] = new SqlParameter("@detailid", _detailid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteServerNameComponentDetailUserApproval", arParams);
        }

        public void AddComponentDetailRelated(int _detailid, int _relatedid, int _include)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@detailid", _detailid);
            arParams[1] = new SqlParameter("@relatedid", _relatedid);
            arParams[2] = new SqlParameter("@include", _include);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addServerNameComponentDetailRelated", arParams);
        }
        public void DeleteComponentDetailRelated(int _detailid, int _include)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@detailid", _detailid);
            arParams[1] = new SqlParameter("@include", _include);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteServerNameComponentDetailRelated", arParams);
        }
        public DataSet GetComponentDetailRelateds(int _detailid, int _include)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@detailid", _detailid);
            arParams[1] = new SqlParameter("@include", _include);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerNameComponentDetailRelateds", arParams);
        }
        public DataSet GetComponentDetailRelatedsByRelated(int _relatedid, int _include)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@relatedid", _relatedid);
            arParams[1] = new SqlParameter("@include", _include);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerNameComponentDetailRelatedsByRelated", arParams);
        }
        public int IsComponentRelatedExclude(ListBox _list, int _checkid)
        {
            int intExclude = 0;
            for (int ii = 0; ii < _list.Items.Count; ii++)
            {
                int intDetail = Int32.Parse(_list.Items[ii].Value);
                if (intDetail != _checkid)
                {
                    int intTemp = IsComponentRelatedExclude(intDetail, _checkid);
                    if (intTemp > 0)
                    {
                        intExclude = intTemp;
                        break;
                    }
                }
            }
            return intExclude;
        }
        private int IsComponentRelatedExclude(int _detailid, int _checkid)
        {
            int intExclude = 0;
            DataSet dsExclude = GetComponentDetailRelateds(_checkid, 0);
            foreach (DataRow drExclude in dsExclude.Tables[0].Rows)
            {
                if (Int32.Parse(drExclude["relatedid"].ToString()) == _detailid && drExclude["include"].ToString() != "1")
                {
                    intExclude = _detailid;
                    break;
                }
            }
            dsExclude = GetComponentDetailRelatedsByRelated(_checkid, 0);
            foreach (DataRow drExclude in dsExclude.Tables[0].Rows)
            {
                if (Int32.Parse(drExclude["detailid"].ToString()) == _detailid && drExclude["include"].ToString() != "1")
                {
                    intExclude = _detailid;
                    break;
                }
            }
            if (intExclude == 0)
            {
                DataSet dsInclude = GetComponentDetailRelatedsByRelated(_detailid, 1);
                foreach (DataRow drInclude in dsInclude.Tables[0].Rows)
                {
                    int intTemp = IsComponentRelatedExclude(Int32.Parse(drInclude["detailid"].ToString()), _checkid);
                    if (intTemp > 0)
                    {
                        intExclude = intTemp;
                        break;
                    }
                }
            }
            return intExclude;
        }

        public void AddComponentDetailRelatedApplication(int _detailid, int _applicationid, int _include)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@detailid", _detailid);
            arParams[1] = new SqlParameter("@applicationid", _applicationid);
            arParams[2] = new SqlParameter("@include", _include);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addServerNameComponentDetailRelatedApplication", arParams);
        }
        public void DeleteComponentDetailRelatedApplication(int _detailid, int _include)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@detailid", _detailid);
            arParams[1] = new SqlParameter("@include", _include);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteServerNameComponentDetailRelatedApplication", arParams);
        }
        public DataSet GetComponentDetailRelatedApplications(int _detailid, int _include)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@detailid", _detailid);
            arParams[1] = new SqlParameter("@include", _include);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerNameComponentDetailRelatedApplications", arParams);
        }

        public void AddComponentDetailSelected(int _serverid, int _detailid, int _prerequisiteid, bool _notify)
        {
            // Delete all prior approvals
            DeleteComponentDetailUserApproval(_serverid, _detailid);

            // Add 
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@serverid", _serverid);
            arParams[1] = new SqlParameter("@detailid", _detailid);
            arParams[2] = new SqlParameter("@prerequisiteid", _prerequisiteid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addServerNameComponentDetailSelected", arParams);

            if (_notify == true)
            {
                // Add people to notify (get ready for SendComponentDetailSelectedNotification)
                DataSet dsNotify = GetComponentDetailUsers(_detailid, 1);
                foreach (DataRow drNotify in dsNotify.Tables[0].Rows)
                {
                    int intUser = Int32.Parse(drNotify["userid"].ToString());
                    AddComponentNotification(intUser);
                }
            }
        }
        public void AddComponentDetailPrerequisites(int _serverid, int _detailid, bool _notify)
        {
            DataSet dsInclude = GetComponentDetailRelatedsByRelated(_detailid, 1);
            foreach (DataRow drInclude in dsInclude.Tables[0].Rows)
            {
                int intRelated = Int32.Parse(drInclude["detailid"].ToString());
                AddComponentDetailSelected(_serverid, intRelated, _detailid, _notify);
                AddComponentDetailPrerequisites(_serverid, intRelated, _notify);
            }
        }
        public void AddComponentNotification(int _userid)
        {
            bool boolFound = false;
            char[] strSplit = { ';' };
            string[] strNotifys = strNotify.Split(strSplit);
            for (int ii = 0; ii < strNotifys.Length; ii++)
            {
                if (strNotifys[ii].Trim() != "")
                {
                    if (strNotifys[ii].Trim().ToUpper() == _userid.ToString().Trim().ToUpper())
                    {
                        boolFound = true;
                        break;
                    }
                }
            }
            if (boolFound == false)
            {
                if (strNotify != "")
                    strNotify += ";";
                strNotify += _userid.ToString();
            }
        }
        public void SendComponentNotification(int _answerid, int _environment,  string _dsn_asset, string _dsn_ip, int _pageid)
        {
            Functions oFunction = new Functions(user, dsn, _environment);
            Variables oVariable = new Variables(_environment);
            Forecast oForecast = new Forecast(user, dsn);
            Pages oPage = new Pages(user, dsn);
            Users oUser = new Users(user, dsn);
            char[] strSplit = { ';' };
            string[] strNotifys = strNotify.Split(strSplit);
            string strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DESIGN_BUILDER");
            for (int ii = 0; ii < strNotifys.Length; ii++)
            {
                if (strNotifys[ii].Trim() != "")
                {
                    int intUser = 0;
                    if (Int32.TryParse(strNotifys[ii].Trim(), out intUser) == true)
                    {
                        string strDefault = oUser.GetApplicationUrl(intUser, _pageid);
                        if (strDefault == "")
                            oFunction.SendEmail("Software Component Approval", oUser.GetName(intUser), "", strEMailIdsBCC, "Software Component Approval", "<p><b>A design has been configured with software components that require your approval.</b><p><p>" + oForecast.GetAnswerBody(_answerid, _environment, _dsn_asset, _dsn_ip) + "</p>", true, false);
                        else
                            oFunction.SendEmail("Software Component Approval", oUser.GetName(intUser), "", strEMailIdsBCC, "Software Component Approval", "<p><b>A design has been configured with software components that require your approval.</b><p><p>" + oForecast.GetAnswerBody(_answerid, _environment, _dsn_asset, _dsn_ip) + "</p><p><a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=/" + strDefault + oPage.GetFullLink(_pageid) + "?id=" + _answerid.ToString() + "\" target=\"_blank\">Click here to review this request.</a></p>", true, false);
                    }
                }
            }
        }
        public void UpdateComponentDetailSelecteds(int _serverid, int _done_from, int _done_to)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@serverid", _serverid);
            arParams[1] = new SqlParameter("@done_from", _done_from);
            arParams[2] = new SqlParameter("@done_to", _done_to);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServerNameComponentDetailSelecteds", arParams);
        }
        public void UpdateComponentDetailSelected(int _serverid, int _detailid, int _done)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@serverid", _serverid);
            arParams[1] = new SqlParameter("@detailid", _detailid);
            arParams[2] = new SqlParameter("@done", _done);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServerNameComponentDetailSelected", arParams);
        }
        public void DeleteComponentDetailSelected(int _serverid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@serverid", _serverid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteServerNameComponentDetailSelected", arParams);
        }
        public DataSet GetComponentDetailSelected(int _serverid, int _prerequisiteid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@serverid", _serverid);
            arParams[1] = new SqlParameter("@prerequisiteid", _prerequisiteid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerNameComponentDetailSelected", arParams);
        }
        public DataSet GetComponentDetailSelected(string _types)
        {
            Functions oFunction = new Functions(0, dsn, 0);
            string strServerTypes = oFunction.BuildXmlStringType(_types);
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@types", strServerTypes);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerNameComponentDetailSelectedAll", arParams);
        }
        public DataSet GetComponentDetailSelectedActive(int _serverid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@serverid", _serverid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerNameComponentDetailSelectedActive", arParams);
        }
        public DataSet GetComponentDetailSelectedRelated(int _applicationid, int _include)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@applicationid", _applicationid);
            arParams[1] = new SqlParameter("@include", _include);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerNameComponentDetailSelectedRelated", arParams);
        }




        public void AddComponentDetailScript(int _detailid, string _name, string _script, int _display, int _enabled)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@detailid", _detailid);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@script", _script);
            arParams[3] = new SqlParameter("@display", _display);
            arParams[4] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addServerNameComponentDetailScript", arParams);
        }
        public void UpdateComponentDetailScript(int _id, int _detailid, string _name, string _script, int _enabled)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@detailid", _detailid);
            arParams[2] = new SqlParameter("@name", _name);
            arParams[3] = new SqlParameter("@script", _script);
            arParams[4] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServerNameComponentDetailScript", arParams);
        }
        public void UpdateComponentDetailScriptOrder(int _id, int _display)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@display", _display);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServerNameComponentDetailScriptOrder", arParams);
        }
        public void EnableComponentDetailScript(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServerNameComponentDetailScriptEnabled", arParams);
        }
        public void DeleteComponentDetailScript(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteServerNameComponentDetailScript", arParams);
        }
        public DataSet GetComponentDetailScript(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerNameComponentDetailScript", arParams);
        }
        public string GetComponentDetailScript(int _id, string _column)
        {
            DataSet ds = GetComponentDetailScript(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public DataSet GetComponentDetailScripts(int _detailid, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@detailid", _detailid);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerNameComponentDetailScripts", arParams);
        }
        #endregion


        #region Permissions
        public void AddApplicationPermission(int _applicationid, int _osid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@applicationid", _applicationid);
            arParams[1] = new SqlParameter("@osid", _osid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addServerNameApplicationPermission", arParams);
        }
        public void DeleteApplicationPermission(int _applicationid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@applicationid", _applicationid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteServerNameApplicationPermission", arParams);
        }
        public DataSet GetApplicationPermissions(int _applicationid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@applicationid", _applicationid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerNameApplicationPermissions", arParams);
        }
        public DataSet GetApplicationPermissionsOS(int _osid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@osid", _osid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerNameApplicationPermissionsOS", arParams);
        }

        //public void AddComponentPermission(int _componentid, int _osid)
        //{
        //    arParams = new SqlParameter[2];
        //    arParams[0] = new SqlParameter("@componentid", _componentid);
        //    arParams[1] = new SqlParameter("@osid", _osid);
        //    SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addServerNameComponentPermission", arParams);
        //}
        //public void DeleteComponentPermission(int _componentid)
        //{
        //    arParams = new SqlParameter[1];
        //    arParams[0] = new SqlParameter("@componentid", _componentid);
        //    SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteServerNameComponentPermission", arParams);
        //}
        //public DataSet GetComponentPermissions(int _componentid)
        //{
        //    arParams = new SqlParameter[1];
        //    arParams[0] = new SqlParameter("@componentid", _componentid);
        //    return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerNameComponentPermissions", arParams);
        //}
        //public DataSet GetComponentPermissionsOS(int _osid, int _pnc)
        //{
        //    arParams = new SqlParameter[2];
        //    arParams[0] = new SqlParameter("@osid", _osid);
        //    arParams[1] = new SqlParameter("@pnc", _pnc);
        //    return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerNameComponentPermissionsOS", arParams);
        //}
        //public DataSet GetComponentPermissionsOSInstall(int _osid, int _pnc)
        //{
        //    arParams = new SqlParameter[2];
        //    arParams[0] = new SqlParameter("@osid", _osid);
        //    arParams[1] = new SqlParameter("@pnc", _pnc);
        //    return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerNameComponentPermissionsOSInstall", arParams);
        //}
        #endregion

        
        #region Related
        public void AddRelated(int _answerid, int _clusterid, int _nameid)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            arParams[1] = new SqlParameter("@clusterid", _clusterid);
            arParams[2] = new SqlParameter("@nameid", _nameid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addServerNameRelated", arParams);
        }
        public DataSet GetRelated(int _answerid, int _clusterid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            arParams[1] = new SqlParameter("@clusterid", _clusterid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerNameRelated", arParams);
        }
        #endregion


        public int AddFactory(string _os, string _location, string _mnemonic, string _environment, string _name1, string _name2, string _func, string _specific, int _userid, string _name, int _available)
        {
            arParams = new SqlParameter[12];
            arParams[0] = new SqlParameter("@os", _os);
            arParams[1] = new SqlParameter("@location", _location);
            arParams[2] = new SqlParameter("@mnemonic", _mnemonic);
            arParams[3] = new SqlParameter("@environment", _environment);
            arParams[4] = new SqlParameter("@name1", _name1);
            arParams[5] = new SqlParameter("@name2", _name2);
            arParams[6] = new SqlParameter("@func", _func);
            arParams[7] = new SqlParameter("@specific", _specific);
            arParams[8] = new SqlParameter("@userid", _userid);
            arParams[9] = new SqlParameter("@name", _name);
            arParams[10] = new SqlParameter("@available", _available);
            arParams[11] = new SqlParameter("@id", SqlDbType.Int);
            arParams[11].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addServerNameFactory", arParams);
            return Int32.Parse(arParams[11].Value.ToString());
        }
        public void UpdateFactory(int _id, int _available)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@available", _available);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServerNameFactoryAvailable", arParams);
        }
        public void UpdateFactory(int _id, string _serial)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@serial", _serial);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServerNameFactorySerial", arParams);
        }
        public void UpdateFactory(int _id, int _userid, string _name)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@userid", _userid);
            arParams[2] = new SqlParameter("@name", _name);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServerNameFactory", arParams);
        }
        public DataSet GetFactory(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerNameFactory", arParams);
        }
        public int GetNameFactory(string _name)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@name", _name);
            object o = SqlHelper.ExecuteScalar(dsn, CommandType.StoredProcedure, "pr_getServerNameFactoryByName", arParams);
            if (o == null || o.ToString() == "")
                return 0;
            else
                return Int32.Parse(o.ToString());
        }
        public string GetNameFactory(int _id, int _userid)
        {
            DataSet ds = GetFactory(_id);
            if (ds.Tables[0].Rows.Count > 0)
            {
                int intUser = Int32.Parse(ds.Tables[0].Rows[0]["userid"].ToString());
                if (_userid != intUser && _userid > 0)
                    return "DENIED";
                string _os = ds.Tables[0].Rows[0]["os"].ToString();
                string _location = ds.Tables[0].Rows[0]["location"].ToString();
                string _mnemonic = ds.Tables[0].Rows[0]["mnemonic"].ToString();
                string _environment = ds.Tables[0].Rows[0]["environment"].ToString();
                string _sequence = ds.Tables[0].Rows[0]["name1"].ToString() + ds.Tables[0].Rows[0]["name2"].ToString();
                string _function = ds.Tables[0].Rows[0]["func"].ToString();
                string _specifics = ds.Tables[0].Rows[0]["specific"].ToString();
                return _os + _location + _mnemonic + _environment + _sequence + _function + _specifics;
            }
            else
                return "Unavailable(" + _id.ToString() + ")";
        }
        public int AddFactory(string _os, string _location, string _mnemonic, string _environment, int _classid, int _environmentid, string _function, string _specific, int _userid, string _name, string _dsn_service_editor)
        {
            // Used for generation (from custom functions and auto-provisioning)
            return AddFactoryNew(_os, _location, _mnemonic, _environment, _classid, _environmentid, _function, _specific, _userid, _name, _dsn_service_editor);
        }
        public int AddFactory(int _id, string _name, int _userid, string _dsn_service_editor)
        {
            // Used for duplication (from custom functions)
            DataSet ds = GetFactory(_id);
            if (ds.Tables[0].Rows.Count > 0)
            {
                string _os = ds.Tables[0].Rows[0]["os"].ToString();
                string _location = ds.Tables[0].Rows[0]["location"].ToString();
                string _mnemonic = ds.Tables[0].Rows[0]["mnemonic"].ToString();
                string _environment = ds.Tables[0].Rows[0]["environment"].ToString();
                string _sequence = ds.Tables[0].Rows[0]["name1"].ToString() + ds.Tables[0].Rows[0]["name2"].ToString();
                string _function = ds.Tables[0].Rows[0]["func"].ToString();
                string _specifics = ds.Tables[0].Rows[0]["specific"].ToString();
                if (_name == "")
                    _name = ds.Tables[0].Rows[0]["name"].ToString();
                return AddFactory(_os, _location, _mnemonic, _environment, 0, 0, _function, _specifics, _userid, _name, _dsn_service_editor);
            }
            else
                return 0;
        }
        private int AddFactoryNew(string _os, string _location, string _mnemonic, string _environment, int _classid, int _environmentid, string _func, string _specific, int _userid, string _name, string _dsn_service_editor)
        {
            DataSet ds = GetFactory(_os, _mnemonic, _environment, 0);
            // NO O's and no I's
            string[] aLetters1 = new string[10] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
            string[] aLetters2 = new string[23] { "A", "B", "C", "D", "E", "F", "G", "H", "J", "K", "M", "N", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            int intID = 0;
            // 00 - 99
            for (int intFirst = 0; intFirst < 10 && intID == 0; intFirst++)
            {
                for (int intSecond = 0; intSecond < 10 && intID == 0; intSecond++)
                {
                    string strName1 = aLetters1[intFirst];
                    string strName2 = aLetters1[intSecond];
                    DataTable dt = ds.Tables[0];
                    DataRow[] dr = dt.Select("name1 = '" + strName1 + "' AND name2 = '" + strName2 + "'");
                    if (dr.Length == 0)
                    {
                        string strNewName = _os + _location + _mnemonic + _environment + strName1 + strName2 + _func + _specific;
                        if (Manual(strNewName, _dsn_service_editor) == true)
                            AddFactory(_os, _location, _mnemonic, _environment, strName1, strName2, _func, _specific, 0, "Manual Process", 0);
                        else if (oFunction.Ping(strNewName, _classid, _environmentid) == true)
                            AddFactory(_os, _location, _mnemonic, _environment, strName1, strName2, _func, _specific, 0, "RESPONSE!!", 0);
                        else
                            intID = AddFactory(_os, _location, _mnemonic, _environment, strName1, strName2, _func, _specific, _userid, _name, 0);
                    }
                }
            }
            // 0A - 9Z
            for (int intFirst = 0; intFirst < 10 && intID == 0; intFirst++)
            {
                for (int intSecond = 0; intSecond < 23 && intID == 0; intSecond++)
                {
                    string strName1 = aLetters1[intFirst];
                    string strName2 = aLetters2[intSecond];
                    DataTable dt = ds.Tables[0];
                    DataRow[] dr = dt.Select("name1 = '" + strName1 + "' AND name2 = '" + strName2 + "'");
                    if (dr.Length == 0)
                    {
                        string strNewName = _os + _location + _mnemonic + _environment + strName1 + strName2 + _func + _specific;
                        if (Manual(strNewName, _dsn_service_editor) == true)
                            AddFactory(_os, _location, _mnemonic, _environment, strName1, strName2, _func, _specific, 0, "Manual Process", 0);
                        else if (oFunction.Ping(strNewName, _classid, _environmentid) == true)
                            AddFactory(_os, _location, _mnemonic, _environment, strName1, strName2, _func, _specific, 0, "RESPONSE!!", 0);
                        else
                            intID = AddFactory(_os, _location, _mnemonic, _environment, strName1, strName2, _func, _specific, _userid, _name, 0);
                    }
                }
            }
            // A0 - Z9
            for (int intFirst = 0; intFirst < 23 && intID == 0; intFirst++)
            {
                for (int intSecond = 0; intSecond < 10 && intID == 0; intSecond++)
                {
                    string strName1 = aLetters2[intFirst];
                    string strName2 = aLetters1[intSecond];
                    DataTable dt = ds.Tables[0];
                    DataRow[] dr = dt.Select("name1 = '" + strName1 + "' AND name2 = '" + strName2 + "'");
                    if (dr.Length == 0)
                    {
                        string strNewName = _os + _location + _mnemonic + _environment + strName1 + strName2 + _func + _specific;
                        if (Manual(strNewName, _dsn_service_editor) == true)
                            AddFactory(_os, _location, _mnemonic, _environment, strName1, strName2, _func, _specific, 0, "Manual Process", 0);
                        else if (oFunction.Ping(strNewName, _classid, _environmentid) == true)
                            AddFactory(_os, _location, _mnemonic, _environment, strName1, strName2, _func, _specific, 0, "RESPONSE!!", 0);
                        else
                            intID = AddFactory(_os, _location, _mnemonic, _environment, strName1, strName2, _func, _specific, _userid, _name, 0);
                    }
                }
            }
            // AA - ZZ
            for (int intFirst = 0; intFirst < 23 && intID == 0; intFirst++)
            {
                for (int intSecond = 0; intSecond < 23 && intID == 0; intSecond++)
                {
                    string strName1 = aLetters2[intFirst];
                    string strName2 = aLetters2[intSecond];
                    DataTable dt = ds.Tables[0];
                    DataRow[] dr = dt.Select("name1 = '" + strName1 + "' AND name2 = '" + strName2 + "'");
                    if (dr.Length == 0)
                    {
                        string strNewName = _os + _location + _mnemonic + _environment + strName1 + strName2 + _func + _specific;
                        if (Manual(strNewName, _dsn_service_editor) == true)
                            AddFactory(_os, _location, _mnemonic, _environment, strName1, strName2, _func, _specific, 0, "Manual Process", 0);
                        else if (oFunction.Ping(strNewName, _classid, _environmentid) == true)
                            AddFactory(_os, _location, _mnemonic, _environment, strName1, strName2, _func, _specific, 0, "RESPONSE!!", 0);
                        else
                            intID = AddFactory(_os, _location, _mnemonic, _environment, strName1, strName2, _func, _specific, _userid, _name, 0);
                    }
                }
            }
            if (intID == 0)
            {
                // An unused name was not found. Check for names that have been used but are now available for reuse (available == 1).
                ds = GetFactory(_os, _mnemonic, _environment, 1);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    intID = Int32.Parse(ds.Tables[0].Rows[0]["id"].ToString());
                    UpdateFactory(intID, 0);
                    UpdateFactory(intID, _userid, _name);
                }
            }
            return intID;
        }
        public DataSet GetFactory(string _os, string _mnemonic, string _environment, int _available)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@os", _os);
            arParams[1] = new SqlParameter("@mnemonic", _mnemonic);
            arParams[2] = new SqlParameter("@environment", _environment);
            arParams[3] = new SqlParameter("@available", _available);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerNameFactorys", arParams);
        }
        public DataSet GetFactoryMine(int _userid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@userid", _userid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerNameFactorysMine", arParams);
        }

        public DataSet GetFunctions()
        {
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerNameFunctions");
        }
        public bool Manual(string _name, string _dsn_service_editor) 
        {
            ServiceEditor oServiceEditor = new ServiceEditor(0, _dsn_service_editor);
            DataSet dsManual = oServiceEditor.APGetNames();
            DataView dvManual = dsManual.Tables[0].DefaultView;
            dvManual.RowFilter = "name = '" + _name + "'";
            return (dvManual.Count > 0);
        }
    }
}
