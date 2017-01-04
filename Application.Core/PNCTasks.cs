using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;

namespace NCC.ClearView.Application.Core
{
    public class PNCTasks
	{
		private string dsn = "";
		private int user = 0;
		private SqlParameter[] arParams;
        public PNCTasks(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
		}
        public DataSet Gets(int _pnc, int _ncb, int _distributed, int _midrange, int _decom, int _enabled)
        {
            arParams = new SqlParameter[6];
            arParams[0] = new SqlParameter("@pnc", _pnc);
            arParams[1] = new SqlParameter("@ncb", _ncb);
            arParams[2] = new SqlParameter("@distributed", _distributed);
            arParams[3] = new SqlParameter("@midrange", _midrange);
            arParams[4] = new SqlParameter("@decom", _decom);
            arParams[5] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getPNCTasks", arParams);
        }
        public DataSet GetAnswer(int _answerid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getPNCTasksAnswer", arParams);
        }
        public DataSet Get(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getPNCTask", arParams);
        }
        public string Get(int _id, string _column)
        {
            DataSet ds = Get(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        /*
        public int GetStep(int _serviceid, int _decom)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@serviceid", _serviceid);
            arParams[1] = new SqlParameter("@decom", _decom);
            object o = SqlHelper.ExecuteScalar(dsn, CommandType.StoredProcedure, "pr_getPNCTaskStep", arParams);
            if (o != null && o.ToString() != "")
                return Int32.Parse(o.ToString());
            else
                return 0;
        }
        */
        public DataSet GetStepsDesign(int _answerid, int _current, int _serverid)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@answerid", _answerid);
            arParams[1] = new SqlParameter("@current", _current);
            arParams[2] = new SqlParameter("@serverid", _serverid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getPNCTaskStepsDesign", arParams);
        }
        public DataSet GetStepsDesigns()
        {
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getPNCTaskStepsDesigns");
        }
        /*
        public DataSet GetSteps(int _pnc, int _ncb, int _distributed, int _midrange, int _decom, int _step)
        {
            arParams = new SqlParameter[6];
            arParams[0] = new SqlParameter("@pnc", _pnc);
            arParams[1] = new SqlParameter("@ncb", _ncb);
            arParams[2] = new SqlParameter("@distributed", _distributed);
            arParams[3] = new SqlParameter("@midrange", _midrange);
            arParams[4] = new SqlParameter("@decom", _decom);
            arParams[5] = new SqlParameter("@step", _step);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getPNCTaskSteps", arParams);
        }
        */
        public void Add(int _serviceid, int _if_cluster, int _if_sql, int _if_citrix, int _if_ltm_config, int _if_ltm_install, int _if_virtual, int _if_physical, int _storage, int _dns, int _tsm, int _legato, int _pnc, int _ncb, int _distributed, int _midrange, int _offsite, int _implementor, int _network_engineer, int _dba, int _project_manager, int _departmental_manager, int _application_lead, int _administrative_contact, int _application_owner, int _requestor, int _decom, int _step, int _substep, int _non_transparent, int _client, int _enabled)
		{
			arParams = new SqlParameter[32];
            arParams[0] = new SqlParameter("@serviceid", _serviceid);
            arParams[1] = new SqlParameter("@if_cluster", _if_cluster);
            arParams[2] = new SqlParameter("@if_sql", _if_sql);
            arParams[3] = new SqlParameter("@if_citrix", _if_citrix);
            arParams[4] = new SqlParameter("@if_ltm_config", _if_ltm_config);
            arParams[5] = new SqlParameter("@if_ltm_install", _if_ltm_install);
            arParams[6] = new SqlParameter("@if_virtual", _if_virtual);
            arParams[7] = new SqlParameter("@if_physical", _if_physical);
            arParams[8] = new SqlParameter("@storage", _storage);
            arParams[9] = new SqlParameter("@dns", _dns);
            arParams[10] = new SqlParameter("@tsm", _tsm);
            arParams[11] = new SqlParameter("@legato", _legato);
            arParams[12] = new SqlParameter("@pnc", _pnc);
            arParams[13] = new SqlParameter("@ncb", _ncb);
            arParams[14] = new SqlParameter("@distributed", _distributed);
            arParams[15] = new SqlParameter("@midrange", _midrange);
            arParams[16] = new SqlParameter("@offsite", _offsite);
            arParams[17] = new SqlParameter("@implementor", _implementor);
            arParams[18] = new SqlParameter("@network_engineer", _network_engineer);
            arParams[19] = new SqlParameter("@dba", _dba);
            arParams[20] = new SqlParameter("@project_manager", _project_manager);
            arParams[21] = new SqlParameter("@departmental_manager", _departmental_manager);
            arParams[22] = new SqlParameter("@application_lead", _application_lead);
            arParams[23] = new SqlParameter("@administrative_contact", _administrative_contact);
            arParams[24] = new SqlParameter("@application_owner", _application_owner);
            arParams[25] = new SqlParameter("@requestor", _requestor);
            arParams[26] = new SqlParameter("@decom", _decom);
            arParams[27] = new SqlParameter("@step", _step);
            arParams[28] = new SqlParameter("@substep", _substep);
            arParams[29] = new SqlParameter("@non_transparent", _non_transparent);
            arParams[30] = new SqlParameter("@client", _client);
            arParams[31] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addPNCTask", arParams);
		}
        public void Update(int _id, int _serviceid, int _if_cluster, int _if_sql, int _if_citrix, int _if_ltm_config, int _if_ltm_install, int _if_virtual, int _if_physical, int _storage, int _dns, int _tsm, int _legato, int _pnc, int _ncb, int _distributed, int _midrange, int _offsite, int _implementor, int _network_engineer, int _dba, int _project_manager, int _departmental_manager, int _application_lead, int _administrative_contact, int _application_owner, int _requestor, int _decom, int _step, int _substep, int _non_transparent, int _client, int _enabled)
        {
            arParams = new SqlParameter[33];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@serviceid", _serviceid);
            arParams[2] = new SqlParameter("@if_cluster", _if_cluster);
            arParams[3] = new SqlParameter("@if_sql", _if_sql);
            arParams[4] = new SqlParameter("@if_citrix", _if_citrix);
            arParams[5] = new SqlParameter("@if_ltm_config", _if_ltm_config);
            arParams[6] = new SqlParameter("@if_ltm_install", _if_ltm_install);
            arParams[7] = new SqlParameter("@if_virtual", _if_virtual);
            arParams[8] = new SqlParameter("@if_physical", _if_physical);
            arParams[9] = new SqlParameter("@storage", _storage);
            arParams[10] = new SqlParameter("@dns", _dns);
            arParams[11] = new SqlParameter("@tsm", _tsm);
            arParams[12] = new SqlParameter("@legato", _legato);
            arParams[13] = new SqlParameter("@pnc", _pnc);
            arParams[14] = new SqlParameter("@ncb", _ncb);
            arParams[15] = new SqlParameter("@distributed", _distributed);
            arParams[16] = new SqlParameter("@midrange", _midrange);
            arParams[17] = new SqlParameter("@offsite", _offsite);
            arParams[18] = new SqlParameter("@implementor", _implementor);
            arParams[19] = new SqlParameter("@network_engineer", _network_engineer);
            arParams[20] = new SqlParameter("@dba", _dba);
            arParams[21] = new SqlParameter("@project_manager", _project_manager);
            arParams[22] = new SqlParameter("@departmental_manager", _departmental_manager);
            arParams[23] = new SqlParameter("@application_lead", _application_lead);
            arParams[24] = new SqlParameter("@administrative_contact", _administrative_contact);
            arParams[25] = new SqlParameter("@application_owner", _application_owner);
            arParams[26] = new SqlParameter("@requestor", _requestor);
            arParams[27] = new SqlParameter("@decom", _decom);
            arParams[28] = new SqlParameter("@step", _step);
            arParams[29] = new SqlParameter("@substep", _substep);
            arParams[30] = new SqlParameter("@non_transparent", _non_transparent);
            arParams[31] = new SqlParameter("@client", _client);
            arParams[32] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updatePNCTask", arParams);
        }
        public void Enable(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updatePNCTaskEnabled", arParams);
        }
        public void Delete(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deletePNCTask", arParams);
        }

        public string InitiateNextStep(int _requestid, int _answerid, int _modelid, int _environment, int _assign_pageid, int _view_pageid, string _dsn_asset, string _dsn_ip, string _dsn_service_editor)
        {
            return InitiateNextStep(_requestid, _answerid, _modelid, _environment, _assign_pageid, _view_pageid, _dsn_asset, _dsn_ip, _dsn_service_editor, true);
        }
        public string InitiateNextStep(int _requestid, int _answerid, int _modelid, int _environment, int _assign_pageid, int _view_pageid, string _dsn_asset, string _dsn_ip, string _dsn_service_editor, bool _initiate_next_step)
        {
            string strError = "";
            // Generate Next Task...
            Forecast oForecast = new Forecast(0, dsn);
            Classes oClass = new Classes(0, dsn);
            Users oUser = new Users(0, dsn);
            Variables oVariable = new Variables(_environment);
            Functions oFunction = new Functions(0, dsn, _environment);
            Servers oServer = new Servers(0, dsn);
            Log oLog = new Log(0, dsn);
            DataSet dsServers = oServer.GetAnswer(_answerid);
            int intClass = 0;
            Int32.TryParse(oForecast.GetAnswer(_answerid, "classid"), out intClass);
            bool boolPNC = (oClass.Get(intClass, "pnc") == "1");
            oLog.AddEvent(_answerid, "", "", "InitiateNextStep(" + _answerid + ")", LoggingType.Debug);

            bool boolStarted = true;
            bool boolCompleted = true;
            bool boolFinished = true;
            bool boolReady = true;
            int intServer = 0;
            foreach (DataRow drServer in dsServers.Tables[0].Rows)
            {
                intServer = Int32.Parse(drServer["id"].ToString());
                if (boolStarted == true)
                    boolStarted = (drServer["build_started"].ToString() != "");
                if (boolCompleted == true)
                    boolCompleted = (drServer["build_completed"].ToString() != "");
                DataSet dsSteps = GetStepsDesign(_answerid, 0, intServer);
                foreach (DataRow drStep in dsSteps.Tables[0].Rows)
                {
                    //if (drStep["non_transparent"].ToString() != "1" && (drStep["created"].ToString() == "" || drStep["completed"].ToString() == ""))
                    if (drStep["created"].ToString() == "" || drStep["completed"].ToString() == "")
                    {
                        // One or more tasks are still open
                        oLog.AddEvent(_answerid, "", "", "InitiateNextStep...One or more tasks are still open (" + drStep["name"].ToString() + " ~ " + drStep["created"].ToString() + " ~ " + drStep["completed"].ToString() + " ~ " + drStep["username"].ToString() + ")", LoggingType.Debug);
                        boolFinished = false;
                        // Check to see if it is a Non-Transparent task
                        if (drStep["non_transparent"].ToString() != "1")
                        {
                            // drStep["non_transparent"].ToString() == "0" means that Non-Transparent == false meaning the task is Transparent and is required for READY flag to be set.
                            oLog.AddEvent(_answerid, "", "", "InitiateNextStep...A transparent task is open so design is not ready for Birth Certificate", LoggingType.Debug);
                            boolReady = false;
                        }
                    }
                }
            }

            if (_initiate_next_step == true)
            {
                if (boolFinished == false)
                {
                    // Initiate Next Step
                    oServer.GetExecution(intServer, _environment, _dsn_asset, _dsn_ip, _dsn_service_editor, _assign_pageid, _view_pageid);
                }
                else
                {
                    // All tasks have been completed, close the design for future tasks
                    oForecast.UpdateAnswerFinished(_answerid, DateTime.Now.ToString());
                }
            }

            if (boolReady == true && boolStarted == true && boolCompleted == true)
            {
                oLog.AddEvent(_answerid, "", "", "InitiateNextStep...All transparent tasks are completed. Mark the server(s) as READY and send Birth Certificate", LoggingType.Debug);
                // All TRANSPARENT tasks are completed, mark the server(s) as READY and send Birth Certificate.
                if (boolPNC == true)
                {
                    // Assume the workflow is done and notify requestor(s) and implementors
                    if (_answerid > 0)
                    {
                        // Send Birth Certificate
                        try
                        {
                            PDFs oPDF = new PDFs(dsn, _dsn_asset, _dsn_ip, _environment);
                            oPDF.CreateDocuments(_answerid, false, false, null, true, true, true, false, true, true);
                            oLog.AddEvent(_answerid, "", "", "InitiateNextStep...Birth Certificate sent", LoggingType.Debug);
                        }
                        catch (Exception exB)
                        {
                            strError = "There was a problem sending the Birth Certificate ~ " + exB.Message;
                            string strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_ERROR,EMAILGRP_PROVISIONING_SUPPORT");
                            oFunction.SendEmail("ERROR: Sending Birth Certificate Form", strEMailIdsBCC, "", "", "ERROR: Sending Birth Certificate Form", "<p>There was a problem sending the BIRTH CERTIFICATE form for Design # " + _answerid.ToString() + "</p><p>Error Message: " + exB.Message + "</p><p>Source: " + exB.Source + "</p><p>Stack Trace: " + exB.StackTrace + "</p>", true, false);
                        }
                    }
                }

                // Update the servers - build is ready.
                foreach (DataRow drServer in dsServers.Tables[0].Rows)
                    oServer.UpdateBuildReady(Int32.Parse(drServer["id"].ToString()), DateTime.Now.ToString(), false);
            }
            else
                oLog.AddEvent(_answerid, "", "", "InitiateNextStep...Birth Certificate NOT sent", LoggingType.Debug);
            return strError;
        }
    }
}
