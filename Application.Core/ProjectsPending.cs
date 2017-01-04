using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Data.SqlTypes;

namespace NCC.ClearView.Application.Core
{
	public class ProjectsPending
	{
		private string dsn = "";
        private int user = 0;
        private int intEnvironment = 0;
        private SqlParameter[] arParams;
        public ProjectsPending(int _user, string _dsn, int _environment)
		{
			user = _user;
            dsn = _dsn;
            intEnvironment = _environment;
        }
		public DataSet Get(int _id)
		{
			arParams = new SqlParameter[1];
			arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getProjectPending", arParams);
		}
        public string Get(int _id, string _column)
        {
            DataSet ds = Get(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public DataSet GetRequest(int _requestid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getProjectPendingRequest", arParams);
        }
        public string GetRequest(int _id, string _column)
        {
            DataSet ds = GetRequest(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public DataSet Gets()
        {
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getProjectPendings");
        }
        public void Add(int _requestid, string _name, string _bd, string _number, int _userid, int _organization, int _segmentid, int _task, string _description)
		{
            if (_organization == 0)
                _organization = 1;
            arParams = new SqlParameter[10];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@bd", _bd);
            arParams[3] = new SqlParameter("@number", _number);
            arParams[4] = new SqlParameter("@userid", _userid);
            arParams[5] = new SqlParameter("@organization", _organization);
            arParams[6] = new SqlParameter("@segmentid", _segmentid);
            arParams[7] = new SqlParameter("@task", _task);
            arParams[8] = new SqlParameter("@description", _description);
            arParams[9] = new SqlParameter("@id", SqlDbType.Int);
            arParams[9].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addProjectPending", arParams);
            int intId = Int32.Parse(arParams[9].Value.ToString());
            Send(intId, "A new project has been submitted and requires your approval.");
        }
        public void Send(int _id, string _text)
        {
            DataSet ds = SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getProjectApproval");
            Variables oVariable = new Variables(intEnvironment);
            Functions oFunction = new Functions(user, dsn, intEnvironment);
            Users oUser = new Users(user, dsn);
            string strEmail = "";
            foreach (DataRow dr in ds.Tables[0].Rows)
                strEmail += oUser.GetName(Int32.Parse(dr["userid"].ToString())) + ";";
            string strURL = oVariable.URL() + "/admin/projects_pending.aspx?id=" + _id.ToString();
            string strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_PROJECT");
            oFunction.SendEmail("ClearView Project Approval", strEmail, "", strEMailIdsBCC, "ClearView Project Approval", "<p><b>" + _text + "</b></p><p><a href=\"" + strURL + "\" target=\"_blank\">Click here to view this project.</a></p>", true, false);
        }
        public void Update(int _requestid, int _lead, int _working, int _executive, int _technical, int _engineer, int _other)
        {
            arParams = new SqlParameter[7];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@lead", _lead);
            arParams[2] = new SqlParameter("@working", _working);
            arParams[3] = new SqlParameter("@executive", _executive);
            arParams[4] = new SqlParameter("@technical", _technical);
            arParams[5] = new SqlParameter("@engineer", _engineer);
            arParams[6] = new SqlParameter("@other", _other);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateProjectPendingUsers", arParams);
        }
        public void Update(int _id, string _name, string _bd, string _number, int _organization, int _segmentid, int _task, string _description)
        {
            arParams = new SqlParameter[8];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@bd", _bd);
            arParams[3] = new SqlParameter("@number", _number);
            arParams[4] = new SqlParameter("@organization", _organization);
            arParams[5] = new SqlParameter("@segmentid", _segmentid);
            arParams[6] = new SqlParameter("@task", _task);
            arParams[7] = new SqlParameter("@description", _description);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateProjectPending", arParams);
            Send(_id, "A project has been updated and requires your approval.");
        }
        public void Update(int _id, string _name, string _number)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@number", _number);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateProjectPendingNameNumber", arParams);
        }
        public void Approve(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateProjectPendingApprove", arParams);
        }
        public void Deny(int _id, string _reason, int _request_pageid,  bool _notify)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@reason", _reason);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateProjectPendingDeny", arParams);
            if (_notify == true)
            {
                DataSet ds = Get(_id);
                int intUser = Int32.Parse(ds.Tables[0].Rows[0]["userid"].ToString());
                int intRequest = Int32.Parse(ds.Tables[0].Rows[0]["requestid"].ToString());
                Variables oVariable = new Variables(intEnvironment);
                Functions oFunction = new Functions(user, dsn, intEnvironment);
                Users oUser = new Users(user, dsn);
                Pages oPage = new Pages(user, dsn);
                string strDefault = oUser.GetApplicationUrl(intUser, _request_pageid);
                string strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_PROJECT");

                if (_reason != "")
                    _reason = "<p>The following comments were added:<br/>" + _reason + "</p>";
                if (strDefault != "")
                    oFunction.SendEmail("ClearView Project Approval", oUser.GetName(intUser), "", strEMailIdsBCC, "ClearView Project Approval", "<p><b>Your project has been DENIED.</b></p>" + _reason + "<p><a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=/" + strDefault + oPage.GetFullLink(_request_pageid) + "?rid=" + intRequest.ToString() + "\" target=\"_blank\">Click here to continue.</a></p>", true, false);
            }
        }
    }
}
