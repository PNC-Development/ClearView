using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Data.SqlTypes;
using System.Text;

namespace NCC.ClearView.Application.Core
{
	public class Requests
	{
		private string dsn = "";
		private int user = 0;
		private SqlParameter[] arParams;
        public Requests(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
		}
        public int AddTask(int _projectid, int _userid, string _description, DateTime _start_date, DateTime _end_date)
        {
            arParams = new SqlParameter[6];
            arParams[0] = new SqlParameter("@projectid", _projectid);
            arParams[1] = new SqlParameter("@userid", _userid);
            arParams[2] = new SqlParameter("@description", _description);
            arParams[3] = new SqlParameter("@start_date", _start_date);
            arParams[4] = new SqlParameter("@end_date", _end_date);
            arParams[5] = new SqlParameter("@requestid", SqlDbType.Int);
            arParams[5].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addRequestTask", arParams);
            return Int32.Parse(arParams[5].Value.ToString());
        }
        public void UpdateTask(int _requestid, string _description, DateTime _start_date, DateTime _end_date)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@description", _description);
            arParams[2] = new SqlParameter("@start_date", _start_date);
            arParams[3] = new SqlParameter("@end_date", _end_date);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateRequestTask", arParams);
        }
        public void UpdateDescription(int _requestid, string _description)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@description", _description);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateRequestDescription", arParams);
        }
        public void UpdateStartDate(int _requestid, DateTime _start_date)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@start_date", _start_date);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateRequestStartDate", arParams);
        }
        public void UpdateEndDate(int _requestid, DateTime _end_date)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@end_date", _end_date);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateRequestEndDate", arParams);
        }

        public int Add(int _projectid, int _userid)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@projectid", _projectid);
            arParams[1] = new SqlParameter("@userid", _userid);
            arParams[2] = new SqlParameter("@requestid", SqlDbType.Int);
            arParams[2].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addRequest", arParams);
            return Int32.Parse(arParams[2].Value.ToString());
        }
        public void Update(int _requestid, int _projectid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@projectid", _projectid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateRequest", arParams);
        }
        public int GetUser(int _requestid)
        {
            string strUser = Get(_requestid, "userid");
            if (strUser != "")
                return Int32.Parse(strUser);
            else
                return 0;
        }
        public DataSet Get(int _requestid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getRequest", arParams);
        }
        public string Get(int _requestid, string _column)
        {
            DataSet ds = Get(_requestid);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public DataSet Gets(int _projectid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@projectid", _projectid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getRequests", arParams);
        }
        public int GetProjectNumber(int _requestid)
        {
            DataSet dsTemp = Get(_requestid);
            if (dsTemp.Tables[0].Rows.Count > 0)
                return Int32.Parse(dsTemp.Tables[0].Rows[0]["projectid"].ToString());
            else
                return 0;
        }
        public bool Allowed(int _requestid, int _serviceid, int _number, int _profile, bool _manager)
        {
            DataSet ds = Get(_requestid);
            if (ds.Tables[0].Rows.Count > 0)
            {
                int intRequester = Int32.Parse(ds.Tables[0].Rows[0]["userid"].ToString());
                if (intRequester == _profile)
                    return true;
                else
                {
                    int intProject = Int32.Parse(ds.Tables[0].Rows[0]["projectid"].ToString());
                    Projects oProject = new Projects(user, dsn);
                    Users oUser = new Users(user, dsn);
                    if (intProject > 0 && Int32.Parse(oProject.Get(intProject, "userid")) == _profile)
                        return true;
                    else if (_manager == true && oUser.IsManager(intRequester, _profile, true) == true)
                        return true;
                    else
                    {
                        // Check Service Members
                        Services oService = new Services(user, dsn);
                        ResourceRequest oResourceRequest = new ResourceRequest(user, dsn);
                        Delegates oDelegate = new Delegates(user, dsn);
                        DataSet dsUser = oService.GetUser(_serviceid, -10); // Approver
                        foreach (DataRow drUser in dsUser.Tables[0].Rows)
                        {
                            if (Int32.Parse(drUser["userid"].ToString()) == _profile || oDelegate.Get(Int32.Parse(drUser["userid"].ToString()), _profile) > 0)
                                return true;
                        }
                        // Check previous workflow user
                        int intPreviousService = 0;
                        if (Int32.TryParse(oService.Get(_serviceid, "workflow_userid"), out intPreviousService) == true && intPreviousService > 0)
                        {
                            DataSet dsRR = oResourceRequest.GetRequestService(_requestid, intPreviousService, _number);
                            if (dsRR.Tables[0].Rows.Count > 0)
                            {
                                int intAssignedTo = 0;
                                if (Int32.TryParse(dsRR.Tables[0].Rows[0]["userid"].ToString(), out intAssignedTo) && intAssignedTo == _profile)
                                    return true;
                            }
                        }
                        // Check 3rd Party Approvers
                        DataSet dsApproval = oResourceRequest.GetApprovals(_requestid, _serviceid, _number);
                        foreach (DataRow drApproval in dsApproval.Tables[0].Rows)
                        {
                            if (Int32.Parse(drApproval["userid"].ToString()) == _profile)
                                return true;
                        }
                        return false;
                    }
                }
            }
            else
                return false;
        }
        public void CloseOverall(int _requestid, int _environment)
        {
            Users oUser = new Users(user, dsn);
            Projects oProject = new Projects(user, dsn);
            Functions oFunction = new Functions(user, dsn, _environment);
            RequestItems oRequestItem = new RequestItems(user, dsn);
            ResourceRequest oResourceRequest = new ResourceRequest(user, dsn);
            string strEmail = "";
            string strCC = "";
            bool boolClose = true;
            DataSet ds = oResourceRequest.GetRequestUnApprovedAll(_requestid);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (Int32.Parse(dr["status"].ToString()) < 3)
                {
                    boolClose = false;
                    break;
                }
            }
            if (boolClose == true)
            {
                int intRequestUser = GetUser(_requestid);
                if (intRequestUser > 0)
                    strEmail = oUser.GetName(intRequestUser);
                int intProject = GetProjectNumber(_requestid);
                if (intProject > 0)
                {
                    int intProjectUser = Int32.Parse(oProject.Get(intProject, "userid"));
                    if (intProjectUser > 0)
                        strCC = oUser.GetName(intProjectUser);
                }
            }
        }
        public string GetBody2(int _id, int _environment, bool _highlight)
        {
            Variables oVariable = new Variables(_environment);
            Functions oFunction = new Functions(0, dsn, _environment);
            Users oUser = new Users(user, dsn);
            Projects oProject = new Projects(user, dsn);
            StringBuilder sbBody = new StringBuilder();
            string strSpacerRow = "<tr><td colspan=\"3\"><img src=\"" + oVariable.ImageURL() + "/images/spacer.gif\" border=\"0\" width=\"1\" height=\"7\" /></td></tr>";
            DataSet ds = Get(_id);
            if (ds.Tables[0].Rows.Count > 0)
            {
                int intProject = Int32.Parse(ds.Tables[0].Rows[0]["projectid"].ToString());
                DataSet dsProject = oProject.Get(intProject);
                if (dsProject.Tables[0].Rows[0]["number"].ToString() != "")
                {
                    sbBody.Append("<tr><td nowrap><b>Project Number:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                    sbBody.Append(dsProject.Tables[0].Rows[0]["number"].ToString());
                    if (_highlight == true)
                    {
                        sbBody.Append("&nbsp;&nbsp;&nbsp;<span style=\"color:#990000; padding:3px; background-color:#FFFFCC\"><b>Note:</b> Please retain this project number for future reference.</span>");
                    }
                    sbBody.Append("</td></tr>");
                    sbBody.Append(strSpacerRow);
                }
                sbBody.Append("<tr><td nowrap><b>Project Name:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                sbBody.Append(dsProject.Tables[0].Rows[0]["name"].ToString());
                sbBody.Append("</td></tr>");
                sbBody.Append(strSpacerRow);
                sbBody.Append("<tr><td nowrap><b>Submitter:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                sbBody.Append(oUser.GetFullName(oUser.GetName(Int32.Parse(ds.Tables[0].Rows[0]["userid"].ToString()))));
                sbBody.Append("</td></tr>");
                if (sbBody.ToString() != "")
                {
                    sbBody.Insert(0, "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"" + oVariable.DefaultFontStyle() + "\">");
                    sbBody.Append("</table>");
                }
            }
            else
            {
                sbBody = new StringBuilder("Unavailable");
            }

            return sbBody.ToString();
        }

        public string GetStatus(int _id)
        {
            string strStatus = "Unavailable";
            Projects oProject = new Projects(user, dsn);
            int intProject = GetProjectNumber(_id);
            if (oProject.Get(intProject, "status") == "-2")
                strStatus = "<span class=\"denied\">Cancelled</span>";
            else if (oProject.Get(intProject, "status") == "-1")
                strStatus = "<span class=\"denied\">Denied</span>";
            else if (oProject.Get(intProject, "status") == "5")
                strStatus = "<span class=\"shelved\">Hold</span>";
            else if (oProject.Get(intProject, "status") == "10")
                strStatus = "<span class=\"shelved\">Future</span>";
            else
            {
                ServiceRequests oServiceRequest = new ServiceRequests(user, dsn);
                ProjectRequest oProjectRequest = new ProjectRequest(user, dsn);
                DataSet ds = oServiceRequest.GetStatus(_id);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["status"].ToString() == "-2")
                        strStatus = "<span class=\"denied\">Cancelled</span>";
                    else if (ds.Tables[0].Rows[0]["status"].ToString() == "-1")
                        strStatus = "<span class=\"waiting\">Draft</span>";
                    else if (ds.Tables[0].Rows[0]["status"].ToString() == "0")
                        strStatus = "<span class=\"waiting\">Draft</span>";
                    else if (ds.Tables[0].Rows[0]["status"].ToString() == "1")
                        strStatus = "<span class=\"approved\">Submitted</span>";
                    else if (ds.Tables[0].Rows[0]["status"].ToString() == "2")
                        strStatus = "<span class=\"default\">In Progress</span>";
                    else
                        strStatus = "<span class=\"shelved\">Completed</span>";
                }
                else
                {
                    ds = oProjectRequest.GetStatus(_id);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        if (ds.Tables[0].Rows[0]["manager"].ToString() == "-1" || ds.Tables[0].Rows[0]["platform"].ToString() == "-1" || ds.Tables[0].Rows[0]["board"].ToString() == "-1" || ds.Tables[0].Rows[0]["director"].ToString() == "-1")
                            strStatus = "<span class=\"denied\">Denied</span>";
                        else if (ds.Tables[0].Rows[0]["manager"].ToString() == "10" || ds.Tables[0].Rows[0]["platform"].ToString() == "10" || ds.Tables[0].Rows[0]["board"].ToString() == "10" || ds.Tables[0].Rows[0]["director"].ToString() == "10")
                            strStatus = "<span class=\"shelved\">Shelved</span>";
                        else if (ds.Tables[0].Rows[0]["manager"].ToString() == "0" || ds.Tables[0].Rows[0]["platform"].ToString() == "0" || ds.Tables[0].Rows[0]["board"].ToString() == "0" || ds.Tables[0].Rows[0]["director"].ToString() == "0")
                            strStatus = "<span class=\"pending\">Pending</span>";
                        else
                            strStatus = "<span class=\"approved\">Approved</span>";
                    }
                }
            }
            return strStatus;
        }

        public void AddResult(int _requestid, int _itemid, int _number, string _title, string _error, string _success, int _environment, bool _notify, string _to)
        {
            StringBuilder sbResult = new StringBuilder();
            Variables oVariable = new Variables(_environment);
            if (_error == "")
            {
                // Success
                sbResult.Append("<table cellpadding=\"2\" cellspacing=\"2\" border=\"0\" style=\"");
                sbResult.Append(oVariable.DefaultFontStyle());
                sbResult.Append("\">");
                sbResult.Append("<tr>");
                sbResult.Append("<td valign=\"top\"><img src=\"");
                sbResult.Append(oVariable.ImageURL());
                sbResult.Append("/images/check.gif\" border=\"0\" align=\"absmiddle\" /></td>");
                sbResult.Append("<td valign=\"top\"><b>");
                sbResult.Append(_title);
                sbResult.Append(":</b></td>");
                sbResult.Append("<td valign=\"top\">");
                sbResult.Append(_success);
                sbResult.Append("</td>");
                sbResult.Append("</tr>");
                sbResult.Append("</table>");
                sbResult.Append("<br/>");
            }
            else
            {
                // Error
                sbResult.Append("<table cellpadding=\"2\" cellspacing=\"2\" border=\"0\" style=\"");
                sbResult.Append(oVariable.DefaultFontStyle());
                sbResult.Append("\">");
                sbResult.Append("<tr>");
                sbResult.Append("<td valign=\"top\"><img src=\"");
                sbResult.Append(oVariable.ImageURL());
                sbResult.Append("/images/error.gif\" border=\"0\" align=\"absmiddle\" /></td>");
                sbResult.Append("<td valign=\"top\"><b>");
                sbResult.Append(_title);
                sbResult.Append(":</b></td>");
                sbResult.Append("<td valign=\"top\">");
                sbResult.Append(_error);
                sbResult.Append("</td>");
                sbResult.Append("</tr>");
                sbResult.Append("</table>");
                sbResult.Append("<br/>");
            }
            AddResult(_requestid, _itemid, _number, sbResult.ToString());
            if (_notify == true)
            {
                Functions oFunction = new Functions(user, dsn, _environment);
                string strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_REQUEST_STATUS");
                try { oFunction.SendEmail(_title, _to, "", strEMailIdsBCC, _title, sbResult.ToString(), true, false); }
                catch { }
            }
        }
        public void AddResult(int _requestid, int _itemid, int _number, string _result)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            arParams[3] = new SqlParameter("@result", _result);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addRequestResult", arParams);
        }
        public DataSet GetResult(int _requestid, int _itemid, int _number)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@itemid", _itemid);
            arParams[2] = new SqlParameter("@number", _number);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getRequestResult", arParams);
        }
        public DataSet GetResult(int _requestid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getRequestResults", arParams);
        }
        public DataSet DeleteResults(int _requestid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_deleteRequestResults", arParams);
        }

        public void UpdateUnNamedRequest(int _requestid, string _name)
        {
            int intProject = GetProjectNumber(_requestid);
            if (intProject < 1)
            {
                ServiceRequests oServiceRequest = new ServiceRequests(user, dsn);
                if (oServiceRequest.Get(_requestid, "name") == "")
                    oServiceRequest.Update(_requestid, _name);
            }
        }
        public string GetLastUpdated(int _requestid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            object o = SqlHelper.ExecuteScalar(dsn, CommandType.StoredProcedure, "pr_getRequestLastUpdated", arParams);
            if (o == null)
                return "";
            else
                return o.ToString();
        }
        public void Cancel(int _requestid)
        {
            ServiceRequests oServiceRequest = new ServiceRequests(user, dsn);
            Projects oProject = new Projects(user, dsn);
            oServiceRequest.DeleteAll(_requestid);
            int intProject = GetProjectNumber(_requestid);
            DataSet ds = Gets(intProject);
            if (ds.Tables[0].Rows.Count == 0)
                oProject.Delete(intProject);
        }
        public string GetDataPointLink(int _requestid, int _environment)
        {
            Variables oVariable = new Variables(_environment);
            Functions oFunction = new Functions(0, dsn, _environment);
            //https://clearview/datapoint/service/request.aspx?t=id&q=Q1ZUNTA2MDA=&id=NTA2MDA=
            return oVariable.URL() + "/datapoint/service/request.aspx?t=id&q=" + oFunction.encryptQueryString(_requestid.ToString()) + "&id=" + oFunction.encryptQueryString(_requestid.ToString());
        }
        public DataSet GetRequestResultsApplication(string _applicationid)
        {
            return SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT * FROM cv_request_results WHERE itemid IN (SELECT itemid FROM cv_request_items WHERE applicationid = " + _applicationid + ") ORDER BY modified DESC");
        }
    }
}
