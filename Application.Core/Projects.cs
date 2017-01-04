using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Data.SqlTypes;
using System.Text;

namespace NCC.ClearView.Application.Core
{
	public class Projects
	{
		private string dsn = "";
		private int user = 0;
		private SqlParameter[] arParams;
		private DataSet ds;
        public Projects(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
		}
		public DataSet GetAll(string _name)
		{
			arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@name", _name);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getProjectsAll", arParams);
		}
        public DataSet GetActive()
        {
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getProjectsActive");
        }
        public DataSet GetForecast()
        {
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getProjectsForecast");
        }
        public DataSet Get(int _id)
		{
			arParams = new SqlParameter[1];
			arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getProject", arParams);
		}
        public string Get(int _id, string _column)
        {
            ds = Get(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public DataSet Get(string _number)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@number", _number);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getProjectNumber", arParams);
        }
        public DataSet GetName(string _name)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@name", _name);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getProjectName", arParams);
        }
        public string GetName(int _id)
		{
			string strName = "Unavailable";
			try { strName = Get(_id).Tables[0].Rows[0]["name"].ToString(); }
			catch {}
			return strName;
		}
        public DataSet GetNames(string _name)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@name", _name);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getProjectNames", arParams);
        }
        public DataSet GetNumbers(string _number)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@number", _number);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getProjectNumbers", arParams);
        }
        public DataSet GetNames(string _name, int _applicationid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@applicationid", _applicationid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getProjectNamesApplication", arParams);
        }
        public DataSet GetNumbers(string _number, int _applicationid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@number", _number);
            arParams[1] = new SqlParameter("@applicationid", _applicationid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getProjectNumbersApplication", arParams);
        }
        public DataSet Gets(int _status)
		{
			arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@status", _status);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getProjects", arParams);
		}
        public DataSet GetUsers(int _userid, int _status)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@userid", _userid);
            arParams[1] = new SqlParameter("@status", _status);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getProjectsUser", arParams);
        }
        public int Add(string _name, string _bd, string _number, int _userid, int _organization, int _segmentid, int _status)
		{
            if (_organization == 0)
                _organization = 1;
            arParams = new SqlParameter[8];
			arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@bd", _bd);
            arParams[2] = new SqlParameter("@number", _number);
            arParams[3] = new SqlParameter("@userid", _userid);
            arParams[4] = new SqlParameter("@organization", _organization);
            arParams[5] = new SqlParameter("@segmentid", _segmentid);
            arParams[6] = new SqlParameter("@status", _status);
            arParams[7] = new SqlParameter("@id", SqlDbType.Int);
            arParams[7].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addProject", arParams);
            return Int32.Parse(arParams[7].Value.ToString());
		}
        public void Update(int _id, string _name, string _bd, string _number, int _userid, int _organization, int _segmentid, int _status)
        {
            arParams = new SqlParameter[8];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@bd", _bd);
            arParams[3] = new SqlParameter("@number", _number);
            arParams[4] = new SqlParameter("@userid", _userid);
            arParams[5] = new SqlParameter("@organization", _organization);
            arParams[6] = new SqlParameter("@segmentid", _segmentid);
            arParams[7] = new SqlParameter("@status", _status);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateProject", arParams);
        }
        public void UpdateLocation(int _projectid, int _addressid, int _bir)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@projectid", _projectid);
            arParams[1] = new SqlParameter("@addressid", _addressid);
            arParams[2] = new SqlParameter("@bir", _bir);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateProjectLocation", arParams);
        }
        
        public void Import()
        {
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateProjectImports");
        }
        public int Import(int _id, string _number, string _name, string _manager, string _created, int _status)
        {
            arParams = new SqlParameter[7];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@number", _number);
            arParams[2] = new SqlParameter("@name", _name);
            arParams[3] = new SqlParameter("@manager", _manager);
            arParams[4] = new SqlParameter("@created", (_created == "" ? SqlDateTime.Null : DateTime.Parse(_created)));
            arParams[5] = new SqlParameter("@status", _status);
            arParams[6] = new SqlParameter("@projectid", SqlDbType.Int);
            arParams[6].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addProjectImport", arParams);
            if (_id > 0)
                return _id;
            else
                return Int32.Parse(arParams[6].Value.ToString());
        }
        public DataSet Imports()
        {
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getProjectImports");
        }
        public void Update(int _id, string _name, string _bd, string _number, int _organization, int _segmentid)
        {
            arParams = new SqlParameter[6];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@bd", _bd);
            arParams[3] = new SqlParameter("@number", _number);
            arParams[4] = new SqlParameter("@organization", _organization);
            arParams[5] = new SqlParameter("@segmentid", _segmentid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateProjectInfo", arParams);
        }
        public void Update(int _id, int _lead, int _executive, int _working, int _technical, int _engineer, int _other)
		{
            arParams = new SqlParameter[7];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@lead", _lead);
            arParams[2] = new SqlParameter("@executive", _executive);
            arParams[3] = new SqlParameter("@working", _working);
            arParams[4] = new SqlParameter("@technical", _technical);
            arParams[5] = new SqlParameter("@engineer", _engineer);
            arParams[6] = new SqlParameter("@other", _other);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateProjectUsers", arParams);
        }
        public void Update(int _id, int _status)
        {
            if (_id > 0)
            {
                arParams = new SqlParameter[2];
                arParams[0] = new SqlParameter("@id", _id);
                arParams[1] = new SqlParameter("@status", _status);
                SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateProjectStatus", arParams);
                if (_status != 0)
                {
                    Requests oRequest = new Requests(user, dsn);
                    DataSet dsRequests = oRequest.Gets(_id);
                    foreach (DataRow drRequest in dsRequests.Tables[0].Rows)
                    {
                        arParams = new SqlParameter[1];
                        arParams[0] = new SqlParameter("@requestid", Int32.Parse(drRequest["requestid"].ToString()));
                        SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateProjectRequestCompleted", arParams);
                    }
                }
                if (_status < 1 || _status > 2)
                {
                    arParams = new SqlParameter[1];
                    arParams[0] = new SqlParameter("@id", _id);
                    SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateProjectCompleted", arParams);
                }
            }
        }
        public void Update(int _id, string _number)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@number", _number);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateProjectNumber", arParams);
        }
        public void Delete(int _id)
		{
			arParams = new SqlParameter[1];
			arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteProject", arParams);
		}
        public void Close(int _id, int _environment)
        {
            ResourceRequest oResourceRequest = new ResourceRequest(user, dsn);
            Users oUser = new Users(user, dsn);
            string strEmail = oUser.GetName(Int32.Parse(Get(_id, "userid")));
            ds = oResourceRequest.GetWorkflowProjectAll(_id);
            bool boolClose = true;
            string strCC = "";
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                int intStatus = Int32.Parse(dr["status"].ToString());
                if (intStatus != (int)ResourceRequestStatus.Closed && intStatus != (int)ResourceRequestStatus.Denied && intStatus != (int)ResourceRequestStatus.Cancelled)
                {
                    boolClose = false;
                    break;
                }
                strCC += oUser.GetName(Int32.Parse(dr["userid"].ToString())) + ";";
            }
            if (boolClose == true)
            {
                Update(_id, 3);
                Functions oFunction = new Functions(user, dsn, _environment);
                // NOTIFICATION
                string strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_PROJECT");
                oFunction.SendEmail("ClearView Notification", strEmail, strCC, strEMailIdsBCC, "ClearView Notification", "<p><b>The following PROJECT has been completed!</b></p><p>" + GetBody(_id, _environment, true) + "</p>", true, false);
                //oFunction.SendEmail("ClearView Notification", "", "", strEMailIdsBCC, "ClearView Notification", "<p>TO: " + strEmail + "<br/>CC: " + strCC + "<b>The following PROJECT has been completed!</b></p><p>" + GetBody(_id, _environment, true) + "</p>", true, false);
            }
        }
        public string GetBody(int _id, int _environment, bool _highlight)
        {
            Users oUser = new Users(user, dsn);
            Organizations oOrganization = new Organizations(user, dsn);
            Segment oSegment = new Segment(user, dsn);
            Variables oVariable = new Variables(_environment);
            StatusLevels oStatusLevel = new StatusLevels();
            StringBuilder sbBody = new StringBuilder();
            string strSpacerRow = "<tr><td colspan=\"3\"><img src=\"" + oVariable.ImageURL() + "/images/spacer.gif\" border=\"0\" width=\"1\" height=\"7\" /></td></tr>";
            if (_id > 0)
            {
                DataSet dsProject = Get(_id);
                if (dsProject.Tables[0].Rows.Count > 0)
                {
                    sbBody.Append("<tr><td nowrap><b>Project Name:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                    sbBody.Append(dsProject.Tables[0].Rows[0]["name"].ToString());
                    sbBody.Append("</td></tr>");
                    sbBody.Append(strSpacerRow);
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
                    sbBody.Append("<tr><td nowrap><b>Initiative Type:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                    sbBody.Append(dsProject.Tables[0].Rows[0]["bd"].ToString());
                    sbBody.Append("</td></tr>");
                    sbBody.Append(strSpacerRow);
                    sbBody.Append("<tr><td nowrap><b>Organization:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                    sbBody.Append(oOrganization.GetName(Int32.Parse(dsProject.Tables[0].Rows[0]["organization"].ToString())));
                    sbBody.Append("</td></tr>");
                    sbBody.Append(strSpacerRow);
                    string strSegment = oSegment.GetName(Int32.Parse(dsProject.Tables[0].Rows[0]["segmentid"].ToString()));
                    if (strSegment != "")
                    {
                        sbBody.Append("<tr><td nowrap><b>Segment:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                        sbBody.Append(oSegment.GetName(Int32.Parse(dsProject.Tables[0].Rows[0]["segmentid"].ToString())));
                        sbBody.Append("</td></tr>");
                        sbBody.Append(strSpacerRow);
                    }
                    sbBody.Append("<tr><td nowrap><b>Project Initiated:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                    sbBody.Append(DateTime.Parse(dsProject.Tables[0].Rows[0]["modified"].ToString()).ToLongDateString());
                    sbBody.Append("</td></tr>");
                    sbBody.Append(strSpacerRow);
                    sbBody.Append("<tr><td nowrap><b>Status:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                    sbBody.Append(oStatusLevel.HTML(Int32.Parse(dsProject.Tables[0].Rows[0]["status"].ToString())));
                    sbBody.Append("</td></tr>");
                }
            }
            if (sbBody.ToString() != "")
            {
                sbBody.Insert(0, "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"" + oVariable.DefaultFontStyle() + "\">");
                sbBody.Append("</table>");
            }
            return sbBody.ToString();
        }
        public bool IsApproved(int _projectid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@projectid", _projectid);
            object o = SqlHelper.ExecuteScalar(dsn, CommandType.StoredProcedure, "pr_getProjectRequestApproved", arParams);
            return (o != null);
        }
        public DataSet GetCoordinator(int _projectid, int _requestid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@projectid", _projectid);
            arParams[1] = new SqlParameter("@requestid", _requestid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getProjectCoordinator", arParams);
        }
        public string GetLastUpdated(int _projectid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@projectid", _projectid);
            object o = SqlHelper.ExecuteScalar(dsn, CommandType.StoredProcedure, "pr_getProjectLastUpdated", arParams);
            if (o == null)
                return "";
            else
                return o.ToString();
        }
        public DataSet GetProjectsAjax(string _name)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@name", _name);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getProjectsAjax", arParams);
        }
        public DataSet GetProjectsLikeName(string strName)
        {
           return SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT * FROM cv_projects WHERE name LIKE '" + strName + "%' AND deleted = 0");    
        }
        public DataSet GetProjectsLikeNumber(string strNumber)
        {
            return SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT * FROM cv_projects WHERE number LIKE '" + strNumber + "%' AND deleted = 0");
        }
        public DataSet GetProjectsLead(int intLead)
        {
            return SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT * FROM cv_projects WHERE lead = " + intLead.ToString() + " AND deleted = 0");
        }
        public DataSet GetProjectsEngineer(int intEngineer)
        {
            return SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT * FROM cv_projects WHERE engineer = " + intEngineer.ToString() + " AND deleted = 0");
        }
        public DataSet GetProjectForecast(int intProject)
        {
            return SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT cv_forecast_answers.id, cv_forecast_answers.forecastid, cv_forecast.requestid from cv_forecast_answers inner join cv_forecast inner join cv_requests on cv_forecast.requestid = cv_requests.requestid and cv_requests.deleted = 0 and cv_requests.projectid = " + intProject.ToString() + " on cv_forecast_answers.forecastid = cv_forecast.id and cv_forecast.deleted = 0 where cv_forecast_answers.deleted = 0");
        }
        public void UpdateForecastAnswer(int intEngineer, int intAnswer)
        {
            SqlHelper.ExecuteNonQuery(dsn, CommandType.Text, "UPDATE cv_forecast_answers SET userid = " + intEngineer.ToString() + " WHERE id = " + intAnswer.ToString());
        }
        public void UpdateForecastForID(int intEngineer, int intForecast)
        {
            SqlHelper.ExecuteNonQuery(dsn, CommandType.Text, "UPDATE cv_forecast SET userid = " + intEngineer.ToString() + " WHERE id = " + intForecast.ToString());
        }
        public void UpdateForecastForRequestID(int intEngineer, int intRequest)
        {
            SqlHelper.ExecuteNonQuery(dsn, CommandType.Text, "UPDATE cv_requests SET userid = " + intEngineer.ToString() + " WHERE requestid = " + intRequest.ToString());
        }
        public bool IsTest(int _projectid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@projectid", _projectid);
            DataSet dsTest = SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getProjectsTest", arParams);
            return (dsTest.Tables[0].Rows.Count > 0);
        }
    }
}
