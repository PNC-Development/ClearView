using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Data.SqlTypes;
using System.Text;
using System.IO;

namespace NCC.ClearView.Application.Core
{
    public enum AuditStatus : int
    {
        NetUseError = -1000,
        TimedOut = -999,
        Running = -10,
        Error = -1,
        Warning = 0,
        Success = 1,
        Ignore = 10,
        NoLongerAvailable = 64,
        InvalidParameter = 87,
    }
    public class Audit
	{
        private string dsn = "";
        private string dsnRemote = "";
        private int user = 0;
		private SqlParameter[] arParams;
        public Audit(int _user, string _dsn)
        {
            user = _user;
            dsn = _dsn;
            Variables oVariable = new Variables(0);
            dsnRemote = oVariable.dsnBetween();
        }
        public Audit(int _user, string _dsn, int _environment)
        {
            user = _user;
            dsn = _dsn;
            Variables oVariable = new Variables(_environment);
            dsnRemote = oVariable.dsnBetween();
        }
        public int AddScript(string _name, string _hardcode, int _local, int _languageid, string _path, string _parameters, int _timeout, int _enabled)
        {
            arParams = new SqlParameter[9];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@hardcode", _hardcode);
            arParams[2] = new SqlParameter("@local", _local);
            arParams[3] = new SqlParameter("@languageid", _languageid);
            arParams[4] = new SqlParameter("@path", _path);
            arParams[5] = new SqlParameter("@parameters", _parameters);
            arParams[6] = new SqlParameter("@timeout", _timeout);
            arParams[7] = new SqlParameter("@enabled", _enabled);
            arParams[8] = new SqlParameter("@id", SqlDbType.Int);
            arParams[8].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addAuditScript", arParams);
            return Int32.Parse(arParams[8].Value.ToString());
        }
        public void UpdateScript(int _id, string _name, string _hardcode, int _local, int _languageid, string _path, string _parameters, int _timeout, int _enabled)
        {
            arParams = new SqlParameter[9];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@hardcode", _hardcode);
            arParams[3] = new SqlParameter("@local", _local);
            arParams[4] = new SqlParameter("@languageid", _languageid);
            arParams[5] = new SqlParameter("@path", _path);
            arParams[6] = new SqlParameter("@parameters", _parameters);
            arParams[7] = new SqlParameter("@timeout", _timeout);
            arParams[8] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateAuditScript", arParams);
        }
        public void EnableScript(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateAuditScriptEnabled", arParams);
        }
        public void DeleteScript(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteAuditScript", arParams);
        }
        public DataSet GetScript(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getAuditScript", arParams);
        }
        public string GetScript(int _id, string _column)
        {
            DataSet ds = GetScript(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public DataSet GetScripts(int _enabled)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getAuditScripts", arParams);
        }
        public string GetScriptParameters(string _parameters, int _audit_id, string _server_name, string _server_ipaddress, int _is_tsm, int _mhs)
        {
            string strReturn = "";
            char[] strSplit = { ' ' };
            string[] strParameters = _parameters.Split(strSplit);
            for (int ii = 0; ii < strParameters.Length; ii++)
            {
                string strParameter = strParameters[ii].Trim();
                if (strParameter != "")
                {
                    if (strReturn != "")
                        strReturn += " ";

                    
                    // WARNING: Be sure that if you are adding more, that the more letters go higher...
                    // EXAMPLE: Contains("%i") above Contains("%id") will return TRUE and break the return (since it should be %id)
                    if (strParameter.Contains("%s") == true)
                        strReturn += GetScriptParameters(strParameter, "%s", _server_name);
                    else if (strParameter.Contains("%mhs") == true)
                        strReturn += GetScriptParameters(strParameter, "%mhs", _mhs.ToString());
                    else if (strParameter.Contains("%id") == true)
                        strReturn += GetScriptParameters(strParameter, "%id", _audit_id.ToString());
                    else if (strParameter.Contains("%i") == true)
                        strReturn += GetScriptParameters(strParameter, "%i", _server_ipaddress);
                    else if (strParameter.Contains("%t") == true)
                        strReturn += GetScriptParameters(strParameter, "%t", _is_tsm.ToString());
                    else
                        strReturn += strParameter;
                }
            }
            return strReturn;
        }
        private string GetScriptParameters(string _parameter, string _search, string _replace)
        {
            string strReturn = _parameter;
            string strBefore = strReturn.Substring(0, strReturn.IndexOf(_search));
            string strAfter = strReturn.Substring(strReturn.IndexOf(_search) + _search.Length);
            return strBefore + _replace + strAfter;
        }


        public int AddScriptSet(string _name, int _models, int _san, int _cluster, int _mis, int _enabled)
        {
            arParams = new SqlParameter[7];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@models", _models);
            arParams[2] = new SqlParameter("@san", _san);
            arParams[3] = new SqlParameter("@cluster", _cluster);
            arParams[4] = new SqlParameter("@mis", _mis);
            arParams[5] = new SqlParameter("@enabled", _enabled);
            arParams[6] = new SqlParameter("@id", SqlDbType.Int);
            arParams[6].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addAuditScriptSet", arParams);
            return Int32.Parse(arParams[6].Value.ToString());
        }
        public void UpdateScriptSet(int _id, string _name, int _models, int _san, int _cluster, int _mis, int _enabled)
        {
            arParams = new SqlParameter[7];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@models", _models);
            arParams[3] = new SqlParameter("@san", _san);
            arParams[4] = new SqlParameter("@cluster", _cluster);
            arParams[5] = new SqlParameter("@mis", _mis);
            arParams[6] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateAuditScriptSet", arParams);
        }
        public void EnableScriptSet(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateAuditScriptSetEnabled", arParams);
        }
        public void DeleteScriptSet(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteAuditScriptSet", arParams);
        }
        public DataSet GetScriptSet(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getAuditScriptSet", arParams);
        }
        public string GetScriptSet(int _id, string _column)
        {
            DataSet ds = GetScriptSet(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public DataSet GetScriptSets(int _enabled)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getAuditScriptSets", arParams);
        }


        public int AddScriptSetDetail(int _scriptsetid, int _scriptid, int _display)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@scriptsetid", _scriptsetid);
            arParams[1] = new SqlParameter("@scriptid", _scriptid);
            arParams[2] = new SqlParameter("@display", _display);
            arParams[3] = new SqlParameter("@id", SqlDbType.Int);
            arParams[3].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addAuditScriptSetDetail", arParams);
            return Int32.Parse(arParams[3].Value.ToString());
        }
        public void DeleteScriptSetDetail(int _scriptsetid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@scriptsetid", _scriptsetid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteAuditScriptSetDetail", arParams);
        }
        public DataSet GetScriptSetDetails(int _scriptsetid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@scriptsetid", _scriptsetid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getAuditScriptSetDetails", arParams);
        }
        public void UpdateScriptSetDetail(int _id, int _display)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@display", _display);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateAuditScriptSetDetail", arParams);
        }


        public int AddScriptLanguage(string _name, string _exe, string _extension, int _enabled)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@exe", _exe);
            arParams[2] = new SqlParameter("@extension", _extension);
            arParams[3] = new SqlParameter("@enabled", _enabled);
            arParams[4] = new SqlParameter("@id", SqlDbType.Int);
            arParams[4].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addAuditScriptLanguage", arParams);
            return Int32.Parse(arParams[4].Value.ToString());
        }
        public void UpdateScriptLanguage(int _id, string _name, string _exe, string _extension, int _enabled)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@exe", _exe);
            arParams[3] = new SqlParameter("@extension", _extension);
            arParams[4] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateAuditScriptLanguage", arParams);
        }
        public void EnableScriptLanguage(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateAuditScriptLanguageEnabled", arParams);
        }
        public void DeleteScriptLanguage(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteAuditScriptLanguage", arParams);
        }
        public DataSet GetScriptLanguage(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getAuditScriptLanguage", arParams);
        }
        public string GetScriptLanguage(int _id, string _column)
        {
            DataSet ds = GetScriptLanguage(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public DataSet GetScriptLanguages(int _enabled)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getAuditScriptLanguages", arParams);
        }


        // CLASS & ENVIRONMENTS
        public int AddScriptSetCE(int _scriptsetid, int _classid, int _environmentid)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@scriptsetid", _scriptsetid);
            arParams[1] = new SqlParameter("@classid", _classid);
            arParams[2] = new SqlParameter("@environmentid", _environmentid);
            arParams[3] = new SqlParameter("@id", SqlDbType.Int);
            arParams[3].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addAuditScriptSetCE", arParams);
            return Int32.Parse(arParams[3].Value.ToString());
        }
        public void DeleteScriptSetCE(int _scriptsetid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@scriptsetid", _scriptsetid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteAuditScriptSetCE", arParams);
        }
        public DataSet GetScriptSetCEs(int _scriptsetid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@scriptsetid", _scriptsetid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getAuditScriptSetCEs", arParams);
        }

        // LOCATIONS
        public int AddScriptSetLocation(int _scriptsetid, int _addressid)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@scriptsetid", _scriptsetid);
            arParams[1] = new SqlParameter("@addressid", _addressid);
            arParams[2] = new SqlParameter("@id", SqlDbType.Int);
            arParams[2].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addAuditScriptSetLocation", arParams);
            return Int32.Parse(arParams[2].Value.ToString());
        }
        public void DeleteScriptSetLocation(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteAuditScriptSetLocation", arParams);
        }
        public DataSet GetScriptSetLocations(int _scriptsetid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@scriptsetid", _scriptsetid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getAuditScriptSetLocations", arParams);
        }


        // OPERATING SYSTEMS & SERVICE PACKS
        public int AddScriptSetOsSp(int _scriptsetid, int _osid, int _spid)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@scriptsetid", _scriptsetid);
            arParams[1] = new SqlParameter("@osid", _osid);
            arParams[2] = new SqlParameter("@spid", _spid);
            arParams[3] = new SqlParameter("@id", SqlDbType.Int);
            arParams[3].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addAuditScriptSetOsSp", arParams);
            return Int32.Parse(arParams[3].Value.ToString());
        }
        public void DeleteScriptSetOsSp(int _scriptsetid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@scriptsetid", _scriptsetid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteAuditScriptSetOsSp", arParams);
        }
        public DataSet GetScriptSetOsSps(int _scriptsetid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@scriptsetid", _scriptsetid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getAuditScriptSetOsSps", arParams);
        }


        // MODELS
        public int AddScriptSetModel(int _scriptsetid, int _modelid)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@scriptsetid", _scriptsetid);
            arParams[1] = new SqlParameter("@modelid", _modelid);
            arParams[2] = new SqlParameter("@id", SqlDbType.Int);
            arParams[2].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addAuditScriptSetModel", arParams);
            return Int32.Parse(arParams[2].Value.ToString());
        }
        public void DeleteScriptSetModel(int _scriptsetid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@scriptsetid", _scriptsetid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteAuditScriptSetModel", arParams);
        }
        public DataSet GetScriptSetModels(int _scriptsetid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@scriptsetid", _scriptsetid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getAuditScriptSetModels", arParams);
        }

        // SERVERS
        public DataSet GetServerScripts(int _serverid, int _classid, int _environmentid, int _modelid, int _osid, int _spid, int _addressid, bool _san, bool _cluster, bool _mis, bool _show_all)
        {
            arParams = new SqlParameter[11];
            arParams[0] = new SqlParameter("@serverid", _serverid);
            arParams[1] = new SqlParameter("@classid", _classid);
            arParams[2] = new SqlParameter("@environmentid", _environmentid);
            arParams[3] = new SqlParameter("@modelid", _modelid);
            arParams[4] = new SqlParameter("@osid", _osid);
            arParams[5] = new SqlParameter("@spid", _spid);
            arParams[6] = new SqlParameter("@addressid", _addressid);
            arParams[7] = new SqlParameter("@san", (_san ? 1 : 0));
            arParams[8] = new SqlParameter("@cluster", (_cluster ? 1 : 0));
            arParams[9] = new SqlParameter("@mis", (_mis ? 1 : 0));
            arParams[10] = new SqlParameter("@all", (_show_all ? 1 : 0));
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerAuditScripts", arParams);
        }
        public DataSet GetServerScriptsMIS(string _types)
        {
            Functions oFunction = new Functions(0, dsn, 0);
            string strServerTypes = oFunction.BuildXmlStringType(_types);
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@types", strServerTypes);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerAuditScriptsMIS", arParams);
        }

        public DataSet GetServers(int _serverid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@serverid", _serverid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerAudits", arParams);
        }
        public DataSet GetServer(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerAudit", arParams);
        }
        public int AddServer(int _serverid, int _scriptsetid, int _scriptid, bool _mis, AuditStatus _status)
        {
            // Called when adding a new script to the system (during audit phase of provisioning)
            arParams = new SqlParameter[6];
            arParams[0] = new SqlParameter("@serverid", _serverid);
            arParams[1] = new SqlParameter("@scriptsetid", _scriptsetid);
            arParams[2] = new SqlParameter("@scriptid", _scriptid);
            arParams[3] = new SqlParameter("@mis", (_mis ? 1 : 0));
            arParams[4] = new SqlParameter("@status", _status);
            arParams[5] = new SqlParameter("@id", SqlDbType.Int);
            arParams[5].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addServerAudit", arParams);
            return Int32.Parse(arParams[5].Value.ToString());
        }
        public void UpdateServer(int _id, AuditStatus _status, string _completed)
        {
            // Called when a script has finished processing (during audit phase of provisioning)
            // Completed is "" unless it finished successfully
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@status", _status);
            arParams[2] = new SqlParameter("@completed", (_completed == "" ? SqlDateTime.Null : DateTime.Parse(_completed)));
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServerAudit", arParams);
        }
        public void DeleteServer(int _serverid, int _mis)
        {
            // Called when deleting all audit results for a particular server
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@serverid", _serverid);
            arParams[1] = new SqlParameter("@mis", _mis);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteServerAudit", arParams);
        }

        // REMOTE
        public DataSet GetServerDetailsRemote(int _parent)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@parent", _parent);
            return SqlHelper.ExecuteDataset(dsnRemote, CommandType.StoredProcedure, "pr_getServerAuditDetails", arParams);
        }
        public void DeleteServerDetailRemote(int _parent)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@parent", _parent);
            SqlHelper.ExecuteNonQuery(dsnRemote, CommandType.StoredProcedure, "pr_deleteServerAuditDetail", arParams);
        }

        // WORKFLOW
        public DataSet GetError(int _requestid, int _serviceid, int _number)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@serviceid", _serviceid);
            arParams[2] = new SqlParameter("@number", _number);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerAuditError", arParams);
        }
        public DataSet GetErrors(int _serverid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@serverid", _serverid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getServerAuditErrors", arParams);
        }
        public void AddError(int _requestid, int _serviceid, int _number, int _auditid, int _step, bool _mis)
        {
            arParams = new SqlParameter[6];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@serviceid", _serviceid);
            arParams[2] = new SqlParameter("@number", _number);
            arParams[3] = new SqlParameter("@auditid", _auditid);
            arParams[4] = new SqlParameter("@step", _step);
            arParams[5] = new SqlParameter("@mis", (_mis ? 1 : 0));
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addServerAuditError", arParams);
        }
        public void UpdateError(int _requestid, int _serviceid, int _number, int _fixed, string _reason)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@serviceid", _serviceid);
            arParams[2] = new SqlParameter("@number", _number);
            arParams[3] = new SqlParameter("@fixed", _fixed);
            arParams[4] = new SqlParameter("@reason", _reason);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServerAuditError", arParams);
        }
        public void UpdateErrorCompleted(int _requestid, int _serviceid, int _number)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@serviceid", _serviceid);
            arParams[2] = new SqlParameter("@number", _number);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateServerAuditErrorCompleted", arParams);
        }
        public void DeleteError(int _requestid, int _serviceid, int _number)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@serviceid", _serviceid);
            arParams[2] = new SqlParameter("@number", _number);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteServerAuditError", arParams);
        }
        public string GetErrorBody(int _requestid, int _serviceid, int _number)
        {
            DataSet ds = GetError(_requestid, _serviceid, _number);
            Users oUser = new Users(0, dsn);
            Requests oRequest = new Requests(0, dsn);
            Servers oServer = new Servers(0, dsn);
            StringBuilder sbDetails = new StringBuilder();
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                int intUser = oRequest.GetUser(_requestid);
                int intAudit = Int32.Parse(ds.Tables[0].Rows[0]["auditid"].ToString());
                DataSet dsAudit = GetServer(intAudit);
                if (dsAudit.Tables[0].Rows.Count > 0)
                {
                    int intServer = Int32.Parse(dsAudit.Tables[0].Rows[0]["serverid"].ToString());
                    sbDetails.Append("<tr><td colspan=\"5\"><b>SERVER: &nbsp;" + oServer.GetName(intServer, true) + "</b></td></tr>");
                    sbDetails.Append("<tr><td colspan=\"5\"><b>SCRIPT: &nbsp;" + dsAudit.Tables[0].Rows[0]["name"].ToString() + "</b></td></tr>");
                    sbDetails.Append("<tr>");
                    sbDetails.Append("<td>&nbsp;&nbsp;&nbsp;&nbsp;</td>");
                    sbDetails.Append("<td><b>Status</b></td>");
                    sbDetails.Append("<td><b>Code</b></td>");
                    sbDetails.Append("<td><b>Result</b></td>");
                    sbDetails.Append("<td><b>Date</b></td>");
                    sbDetails.Append("</tr>");
                    try
                    {
                        DataSet dsAuditResult = GetServerDetailsRemote(intAudit);
                        foreach (DataRow drAuditResult in dsAuditResult.Tables[0].Rows)
                        {
                            sbDetails.Append("<tr>");
                            sbDetails.Append("<td></td>");
                            try
                            {
                                int intAuditStatus = Int32.Parse(drAuditResult["status"].ToString());
                                AuditStatus oAuditStatus = (AuditStatus)intAuditStatus;
                                sbDetails.Append("<td valign=\"top\" nowrap>" + oAuditStatus.ToString() + "</td>");
                            }
                            catch
                            {
                                sbDetails.Append("<td valign=\"top\" nowrap>Unknown Status (" + drAuditResult["status"].ToString() + ")</td>");
                            }
                            sbDetails.Append("<td valign=\"top\" nowrap>" + drAuditResult["code"].ToString() + "</td>");
                            sbDetails.Append("<td valign=\"top\" width=\"100%\">" + drAuditResult["result"].ToString() + "</td>");
                            sbDetails.Append("<td valign=\"top\" nowrap>" + drAuditResult["created"].ToString() + "</td>");
                            sbDetails.Append("</tr>");
                        }
                    }
                    catch { }
                }
            }
            return sbDetails.ToString();
        }

        public string GetImage(AuditStatus oStatus)
        {
            string strImage = "/images/bigHelp.gif";
            switch (oStatus) 
            {
                case AuditStatus.Error:
                    strImage = "/images/bigError.gif";
                    break;
                case AuditStatus.Ignore:
                    strImage = "/images/bigCheck.gif";
                    break;
                case AuditStatus.Running:
                    strImage = "/images/bigArrowRight.gif";
                    break;
                case AuditStatus.Success:
                    strImage = "/images/bigCheck.gif";
                    break;
                case AuditStatus.TimedOut:
                    strImage = "/images/clock.gif";
                    break;
                case AuditStatus.Warning:
                    strImage = "/images/bigAlert.gif";
                    break;
            }
            return strImage;
        }

    }
}
