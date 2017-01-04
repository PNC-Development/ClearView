using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Data.SqlTypes;
using System.Text;

namespace NCC.ClearView.Application.Core
{
	public class ProjectRequest
	{
		private string dsn = "";
		private int user = 0;
		private SqlParameter[] arParams;
        public ProjectRequest(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
		}
        public DataSet Get(int _requestid)
		{
			arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getProjectRequest", arParams);
		}
        public DataSet GetStatus(int _requestid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getProjectRequestStatusAll", arParams);
        }
        public DataSet GetProject(int _projectid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@projectid", _projectid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getProjectRequestProject", arParams);
        }
        public DataSet GetProjectName(string _name)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@name", _name);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getProjectRequestProjectName", arParams);
        }
        public DataSet GetProjectNumber(string _number)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@number", _number);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getProjectRequestProjectNumber", arParams);
        }
        public int GetId(int _projectid)
        {
            DataSet ds = GetProject(_projectid);
            if (ds.Tables[0].Rows.Count > 0)
                return Int32.Parse(ds.Tables[0].Rows[0]["requestid"].ToString());
            else
                return 0;
        }
        public string Get(int _requestid, string _column)
        {
            DataSet ds = Get(_requestid);
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
        public DataSet GetApproval(int _approval)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@approval", _approval);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getProjectRequestApprovals", arParams);
        }
        public DataSet GetApproval(int _approval, int _userid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@approval", _approval);
            arParams[1] = new SqlParameter("@userid", _userid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getProjectRequestApprovalUsers", arParams);
        }
        public DataSet Gets(int _enabled)
		{
			arParams = new SqlParameter[1];
			arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getProjectRequests", arParams);
		}
        public DataSet Gets(int _userid, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@userid", _userid);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getProjectRequestsUser", arParams);
        }

        public DataSet GetProjectRequestDetails(int? _ProjectId)
        {
            arParams = new SqlParameter[1];
            if (_ProjectId != null)
                arParams[0] = new SqlParameter("@ProjectId", _ProjectId);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getDatapointProjectRequest", arParams);

        }
        public void Add(int _requestid, int _req_type, string _req_date, string _interdependency, string _projects, string _capability, int _man_hours, string _expected_capital, string _internal_labor, string _external_labor, string _maintenance_increase, string _project_expenses, string _estimated_avoidance, string _estimated_savings, string _realized_savings, string _business_avoidance, string _maintenance_avoidance, string _asset_reusability, string _internal_impact, string _external_impact, string _business_impact, string _strategic_opportunity, string _acquisition, string _technology_capabilities, int _c1, int _endlife, string _endlife_date, int _tpm)
		{
            arParams = new SqlParameter[28];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@req_type", _req_type);
            arParams[2] = new SqlParameter("@req_date", (_req_date == "" ? SqlDateTime.Null : DateTime.Parse(_req_date)));
            arParams[3] = new SqlParameter("@interdependency", _interdependency);
            arParams[4] = new SqlParameter("@projects", _projects);
            arParams[5] = new SqlParameter("@capability", _capability);
            arParams[6] = new SqlParameter("@man_hours", _man_hours);
            arParams[7] = new SqlParameter("@expected_capital", _expected_capital);
            arParams[8] = new SqlParameter("@internal_labor", _internal_labor);
            arParams[9] = new SqlParameter("@external_labor", _external_labor);
            arParams[10] = new SqlParameter("@maintenance_increase", _maintenance_increase);
            arParams[11] = new SqlParameter("@project_expenses", _project_expenses);
            arParams[12] = new SqlParameter("@estimated_avoidance", _estimated_avoidance);
            arParams[13] = new SqlParameter("@estimated_savings", _estimated_savings);
            arParams[14] = new SqlParameter("@realized_savings", _realized_savings);
            arParams[15] = new SqlParameter("@business_avoidance", _business_avoidance);
            arParams[16] = new SqlParameter("@maintenance_avoidance", _maintenance_avoidance);
            arParams[17] = new SqlParameter("@asset_reusability", _asset_reusability);
            arParams[18] = new SqlParameter("@internal_impact", _internal_impact);
            arParams[19] = new SqlParameter("@external_impact", _external_impact);
            arParams[20] = new SqlParameter("@business_impact", _business_impact);
            arParams[21] = new SqlParameter("@strategic_opportunity", _strategic_opportunity);
            arParams[22] = new SqlParameter("@acquisition", _acquisition);
            arParams[23] = new SqlParameter("@technology_capabilities", _technology_capabilities);
            arParams[24] = new SqlParameter("@c1", _c1);
            arParams[25] = new SqlParameter("@endlife", _endlife);
            arParams[26] = new SqlParameter("@endlife_date", (_endlife_date == "" ? SqlDateTime.Null : DateTime.Parse(_endlife_date)));
            arParams[27] = new SqlParameter("@tpm", _tpm);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addProjectRequest", arParams);
		}
        public void Update(int _requestid, int _req_type, string _req_date, string _interdependency, string _projects, string _capability, int _man_hours, string _expected_capital, string _internal_labor, string _external_labor, string _maintenance_increase, string _project_expenses, string _estimated_avoidance, string _estimated_savings, string _realized_savings, string _business_avoidance, string _maintenance_avoidance, string _asset_reusability, string _internal_impact, string _external_impact, string _business_impact, string _strategic_opportunity, string _acquisition, string _technology_capabilities, int _c1, int _endlife, string _endlife_date, int _tpm)
		{
            arParams = new SqlParameter[28];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@req_type", _req_type);
            arParams[2] = new SqlParameter("@req_date", (_req_date == "" ? SqlDateTime.Null : DateTime.Parse(_req_date)));
            arParams[3] = new SqlParameter("@interdependency", _interdependency);
            arParams[4] = new SqlParameter("@projects", _projects);
            arParams[5] = new SqlParameter("@capability", _capability);
            arParams[6] = new SqlParameter("@man_hours", _man_hours);
            arParams[7] = new SqlParameter("@expected_capital", _expected_capital);
            arParams[8] = new SqlParameter("@internal_labor", _internal_labor);
            arParams[9] = new SqlParameter("@external_labor", _external_labor);
            arParams[10] = new SqlParameter("@maintenance_increase", _maintenance_increase);
            arParams[11] = new SqlParameter("@project_expenses", _project_expenses);
            arParams[12] = new SqlParameter("@estimated_avoidance", _estimated_avoidance);
            arParams[13] = new SqlParameter("@estimated_savings", _estimated_savings);
            arParams[14] = new SqlParameter("@realized_savings", _realized_savings);
            arParams[15] = new SqlParameter("@business_avoidance", _business_avoidance);
            arParams[16] = new SqlParameter("@maintenance_avoidance", _maintenance_avoidance);
            arParams[17] = new SqlParameter("@asset_reusability", _asset_reusability);
            arParams[18] = new SqlParameter("@internal_impact", _internal_impact);
            arParams[19] = new SqlParameter("@external_impact", _external_impact);
            arParams[20] = new SqlParameter("@business_impact", _business_impact);
            arParams[21] = new SqlParameter("@strategic_opportunity", _strategic_opportunity);
            arParams[22] = new SqlParameter("@acquisition", _acquisition);
            arParams[23] = new SqlParameter("@technology_capabilities", _technology_capabilities);
            arParams[24] = new SqlParameter("@c1", _c1);
            arParams[25] = new SqlParameter("@endlife", _endlife);
            arParams[26] = new SqlParameter("@endlife_date", (_endlife_date == "" ? SqlDateTime.Null : DateTime.Parse(_endlife_date)));
            arParams[27] = new SqlParameter("@tpm", _tpm);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateProjectRequest", arParams);
        }
        public void Update(int _requestid, int _notify)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@notify", _notify);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateProjectRequestNotify", arParams);
        }
        public void Delete(int _requestid)
		{
			arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteProjectRequest", arParams);
		}
        public void AddPlatform(int _requestid, int _platformid)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@platformid", _platformid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addProjectRequestPlatform", arParams);
        }
        public void DeletePlatforms(int _requestid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteProjectRequestPlatforms", arParams);
        }
        public DataSet GetPlatforms(int _requestid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getProjectRequestPlatforms", arParams);
        }
        public DataSet GetResources(int _requestid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getProjectRequestResources", arParams);
        }
        public string GetBody(int _requestid, int _environment, bool _highlight)
        {
            Variables oVariable = new Variables(_environment);
            Users oUser = new Users(user, dsn);
            Projects oProject = new Projects(user, dsn);
            Requests oRequest = new Requests(user, dsn);
            int intProject = oRequest.GetProjectNumber(_requestid);
            StringBuilder sbBody = new StringBuilder();
            string strSpacerRow = "<tr><td colspan=\"3\"><img src=\"" + oVariable.ImageURL() + "/images/spacer.gif\" border=\"0\" width=\"1\" height=\"7\" /></td></tr>";
            DataSet ds = Get(_requestid);
            if (ds.Tables[0].Rows.Count == 0)
                return oProject.GetBody(intProject, _environment, _highlight);
            else
            {
                DataSet dsRequest = oRequest.Get(_requestid);
                if (dsRequest.Tables[0].Rows.Count > 0)
                {
                    DataSet dsProject = oProject.Get(intProject);
                    //strBody += "<tr><td nowrap><b>Request ID:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>" + dsRequest.Tables[0].Rows[0]["requestid"].ToString() + "</td></tr>";
                    //strBody += strSpacerRow;
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
                    sbBody.Append("<tr><td nowrap><b>Initiative Type:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                    sbBody.Append(dsProject.Tables[0].Rows[0]["bd"].ToString());
                    sbBody.Append("</td></tr>");
                    sbBody.Append(strSpacerRow);
                    sbBody.Append("<tr><td nowrap><b>Submitter:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                    sbBody.Append(oUser.GetFullName(oUser.GetName(Int32.Parse(dsRequest.Tables[0].Rows[0]["userid"].ToString()))));
                    sbBody.Append("</td></tr>");
                    sbBody.Append(strSpacerRow);
                    sbBody.Append("<tr><td nowrap><b>Submitted On:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                    sbBody.Append(DateTime.Parse(dsRequest.Tables[0].Rows[0]["modified"].ToString()).ToLongDateString());
                    sbBody.Append("</td></tr>");
                    sbBody.Append(strSpacerRow);
                    if (ds.Tables[0].Rows[0]["req_type"].ToString() == "1")
                    {
                        sbBody.Append("<tr><td nowrap><b>Audit Requirement</b></td><td>&nbsp;&nbsp;&nbsp;</td><td><img src=\"");
                        sbBody.Append(oVariable.ImageURL());
                        sbBody.Append("/images/siren.gif\" border=\"0\" align=\"absmiddle\"></td></tr>");
                        sbBody.Append(strSpacerRow);
                    }
                    if (ds.Tables[0].Rows[0]["endlife"].ToString() == "1")
                    {
                        sbBody.Append("<tr><td nowrap><b>End Life</b></td><td>&nbsp;&nbsp;&nbsp;</td><td><img src=\"");
                        sbBody.Append(oVariable.ImageURL());
                        sbBody.Append("/images/xx.gif\" border=\"0\" align=\"absmiddle\"></td></tr>");
                        sbBody.Append(strSpacerRow);
                        sbBody.Append("<tr><td nowrap><b>End Life Date</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                        sbBody.Append(DateTime.Parse(ds.Tables[0].Rows[0]["endlife_date"].ToString()).ToLongDateString());
                        sbBody.Append("</td></tr>");
                        sbBody.Append(strSpacerRow);
                    }
                    sbBody.Append("<tr><td colspan=\"3\">&nbsp;</td></tr>");
                    sbBody.Append("<tr><td colspan=\"3\">");
                    sbBody.Append(GetPriority(_requestid, _environment));
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
        }
        public string GetBodyNoPriority(int _requestid, int _environment, bool _highlight)
        {
            Variables oVariable = new Variables(_environment);
            Users oUser = new Users(user, dsn);
            Projects oProject = new Projects(user, dsn);
            StringBuilder sbBody = new StringBuilder();
            string strSpacerRow = "<tr><td colspan=\"3\"><img src=\"" + oVariable.ImageURL() + "/images/spacer.gif\" border=\"0\" width=\"1\" height=\"7\" /></td></tr>";
            Requests oRequest = new Requests(user, dsn);
            DataSet dsRequest = oRequest.Get(_requestid);
            if (dsRequest.Tables[0].Rows.Count > 0)
            {
                DataSet ds = Get(_requestid);
                int intProject = Int32.Parse(dsRequest.Tables[0].Rows[0]["projectid"].ToString());
                DataSet dsProject = oProject.Get(intProject);
                //strBody += "<tr><td nowrap><b>Request ID:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>" + dsRequest.Tables[0].Rows[0]["requestid"].ToString() + "</td></tr>";
                //strBody += strSpacerRow;
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
                sbBody.Append("<tr><td nowrap><b>Initiative Type:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                sbBody.Append(dsProject.Tables[0].Rows[0]["bd"].ToString());
                sbBody.Append("</td></tr>");
                sbBody.Append(strSpacerRow);
                sbBody.Append("<tr><td nowrap><b>Submitter:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                sbBody.Append(oUser.GetFullName(oUser.GetName(Int32.Parse(dsRequest.Tables[0].Rows[0]["userid"].ToString()))));
                sbBody.Append("</td></tr>");
                sbBody.Append(strSpacerRow);
                sbBody.Append("<tr><td nowrap><b>Submitted On:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                sbBody.Append(DateTime.Parse(dsRequest.Tables[0].Rows[0]["modified"].ToString()).ToLongDateString());
                sbBody.Append("</td></tr>");
                sbBody.Append(strSpacerRow);
                if (ds.Tables[0].Rows[0]["req_type"].ToString() == "1")
                {
                    sbBody.Append("<tr><td nowrap><b>Audit Requirement</b></td><td>&nbsp;&nbsp;&nbsp;</td><td><img src=\"");
                    sbBody.Append(oVariable.ImageURL());
                    sbBody.Append("/images/siren.gif\" border=\"0\" align=\"absmiddle\"></td></tr>");
                    sbBody.Append(strSpacerRow);
                }
                if (ds.Tables[0].Rows[0]["endlife"].ToString() == "1")
                {
                    sbBody.Append("<tr><td nowrap><b>End Life</b></td><td>&nbsp;&nbsp;&nbsp;</td><td><img src=\"");
                    sbBody.Append(oVariable.ImageURL());
                    sbBody.Append("/images/xx.gif\" border=\"0\" align=\"absmiddle\"></td></tr>");
                    sbBody.Append(strSpacerRow);
                    sbBody.Append("<tr><td nowrap><b>End Life Date</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                    sbBody.Append(DateTime.Parse(ds.Tables[0].Rows[0]["endlife_date"].ToString()).ToLongDateString());
                    sbBody.Append("</td></tr>");
                    sbBody.Append(strSpacerRow);
                }
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
        public string GetPriority(int _requestid, int _environment)
        {
            Variables oVariable = new Variables(_environment);
            DataSet ds = Get(_requestid);
            StringBuilder sbPriority = new StringBuilder();
            sbPriority.Append("<tr bgcolor=\"#EEEEEE\">");
            sbPriority.Append("<td align=\"center\" width=\"85\"><b>Expected<br>Cost</b></td>");
            sbPriority.Append("<td align=\"center\" width=\"10\"><img src=\"/images/spacer.gif\" border=\"0\" width=\"13\" height=\"1\"></td>");
            sbPriority.Append("<td align=\"center\" width=\"85\"><b>Cost<br>Avoidance</b></td>");
            sbPriority.Append("<td align=\"center\" width=\"10\"><img src=\"/images/spacer.gif\" border=\"0\" width=\"13\" height=\"1\"></td>");
            sbPriority.Append("<td align=\"center\" width=\"85\"><b>Impact</b></td>");
            sbPriority.Append("<td align=\"center\" width=\"10\"><img src=\"/images/spacer.gif\" border=\"0\" width=\"13\" height=\"1\"></td>");
            sbPriority.Append("<td align=\"center\" width=\"85\"><b>Overall</b></td>");
            sbPriority.Append("<td align=\"center\" width=\"10\"><img src=\"/images/spacer.gif\" border=\"0\" width=\"13\" height=\"1\"></td>");
            sbPriority.Append("<td align=\"center\" width=\"85\"><b>Overall<br>Weight</b></td>");
            sbPriority.Append("</tr>");
            sbPriority.Append("<tr>");
            sbPriority.Append("<td align=\"center\" width=\"85\" ");
            sbPriority.Append(GetPriorityColor(Double.Parse(ds.Tables[0].Rows[0]["expected_cost"].ToString())));
            sbPriority.Append(">");
            sbPriority.Append(Double.Parse(ds.Tables[0].Rows[0]["expected_cost"].ToString()).ToString("P"));
            sbPriority.Append("</td>");
            sbPriority.Append("<td align=\"center\" width=\"10\"><img src=\"/images/spacer.gif\" border=\"0\" width=\"13\" height=\"1\"></td>");
            sbPriority.Append("<td align=\"center\" width=\"85\" ");
            sbPriority.Append(GetPriorityColor(Double.Parse(ds.Tables[0].Rows[0]["cost_avoidance"].ToString())));
            sbPriority.Append(">");
            sbPriority.Append(Double.Parse(ds.Tables[0].Rows[0]["cost_avoidance"].ToString()).ToString("P"));
            sbPriority.Append("</td>");
            sbPriority.Append("<td align=\"center\" width=\"10\"><img src=\"/images/spacer.gif\" border=\"0\" width=\"13\" height=\"1\"></td>");
            sbPriority.Append("<td align=\"center\" width=\"85\" ");
            sbPriority.Append(GetPriorityColor(Double.Parse(ds.Tables[0].Rows[0]["impact_analysis"].ToString())));
            sbPriority.Append(">");
            sbPriority.Append(Double.Parse(ds.Tables[0].Rows[0]["impact_analysis"].ToString()).ToString("P"));
            sbPriority.Append("</td>");
            sbPriority.Append("<td align=\"center\" width=\"10\"><img src=\"/images/spacer.gif\" border=\"0\" width=\"13\" height=\"1\"></td>");
            sbPriority.Append("<td align=\"center\" width=\"85\" ");
            sbPriority.Append(GetPriorityColor(Double.Parse(ds.Tables[0].Rows[0]["overall_priority"].ToString())));
            sbPriority.Append(">");
            sbPriority.Append(Double.Parse(ds.Tables[0].Rows[0]["overall_priority"].ToString()).ToString("P"));
            sbPriority.Append("</td>");
            sbPriority.Append("<td align=\"center\" width=\"10\"><img src=\"/images/spacer.gif\" border=\"0\" width=\"13\" height=\"1\"></td>");
            sbPriority.Append("<td align=\"center\" width=\"85\" ");
            sbPriority.Append(GetPriorityColor(Double.Parse(ds.Tables[0].Rows[0]["overall_priority"].ToString())));
            sbPriority.Append(">");
            sbPriority.Append(GetPriorityValue(Double.Parse(ds.Tables[0].Rows[0]["overall_priority"].ToString())));
            sbPriority.Append("</td>");
            sbPriority.Append("</tr>");
            sbPriority.Append("<tr><td colspan=\"9\">&nbsp;</td></tr>");
            sbPriority.Append("<tr><td align=\"left\" colspan=\"9\"><a href=\"");
            sbPriority.Append(oVariable.URL());
            sbPriority.Append("/help/ClearView EIPP Weight Explanation.pdf\">Click here for Weight Explanation</a></td></tr>");
            if (sbPriority.ToString() != "")
            {
                sbPriority.Insert(0, "<table width=\"465\" border=\"0\" cellpadding=\"2\" cellspacing=\"0\" style=\"" + oVariable.DefaultFontStyle() + "\">");
                sbPriority.Append("</table>");
            }
            return sbPriority.ToString();
        }
        public string GetPriorityColor(double _value)
        {
            if (_value >= .80)
                return "bgcolor=\"#cdffcd\" style=\"border:solid 3px #00ee00\"";
            else if (_value >= .60)
                return "bgcolor=\"#ffffcd\" style=\"border:solid 3px #eeee00\"";
            else
                return "bgcolor=\"#ffcdcd\" style=\"border:solid 3px #ee0000\"";
        }
        public string GetPriorityValue(double _value)
        {
            if (_value >= .80)
                return "High";
            else if (_value >= .60)
                return "Medium";
            else
                return "Low";
        }
        public string GetPriority(int _requestid)
        {
            double dblPriority = 0.60;
            DataSet ds = Get(_requestid);
            if (ds.Tables[0].Rows.Count > 0)
                dblPriority = double.Parse(ds.Tables[0].Rows[0]["overall_priority"].ToString());
            string strPriority = "<td align=\"center\" width=\"85\" " + GetPriorityColor(dblPriority) + ">" + GetPriorityValue(dblPriority) + "</td>";
            Variables oVariable = new Variables(0);
            strPriority = "<table width=\"85\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"" + oVariable.DefaultFontStyle() + "\">" + strPriority + "</table>";
            return strPriority;
        }
        public void AddComment(int _requestid, int _userid, string _comment, int _environment,  int _viewpage)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@userid", _userid);
            arParams[2] = new SqlParameter("@comment", _comment);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addProjectRequestComment", arParams);
            string strBody = GetBodyNoPriority(_requestid, _environment, true);
            Users oUser = new Users(user, dsn);
            Functions oFunction = new Functions(user, dsn, _environment);
            Pages oPage = new Pages(user, dsn);
            Variables oVariable = new Variables(_environment);
            string strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_PROJECT");
            if (Get(_requestid, "notify") == "1")
            {
                Requests oRequest = new Requests(user, dsn);
                int intUser = oRequest.GetUser(_requestid);
                string strComment = "<p>" + oUser.GetFullName(_userid) + " wrote:<br/>" + _comment + "</p>";
                string strDefault = oUser.GetApplicationUrl(intUser, _viewpage);
                if (strDefault == "")
                    oFunction.SendEmail("ClearView - Project Request Discussion Notification", oUser.GetName(intUser), "", strEMailIdsBCC, "ClearView - Project Request Discussion Notification", "<p><b>A new comment has been posted to the discussion section for the following project request...</b></p><p>" + strBody + "</p><p>" + strComment + "</p>", true, false);
                else
                    oFunction.SendEmail("ClearView - Project Request Discussion Notification", oUser.GetName(intUser), "", strEMailIdsBCC, "ClearView - Project Request Discussion Notification", "<p><b>A new comment has been posted to the discussion section for the following project request...</b></p><p><a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=/" + strDefault + oPage.GetFullLink(_viewpage) + "?rid=" + _requestid.ToString() + "&div=C\" target=\"_blank\">Click here to view this project request.</a></p><p>" + strBody + "</p><p>" + strComment + "</p>", true, false);
            }
            // Notify Approval People
            ProjectRequest_Approval oApproval = new ProjectRequest_Approval(user, dsn, _environment);
            DataSet dsNotify = GetCommentsNotify(_requestid);
            foreach (DataRow dr in dsNotify.Tables[0].Rows)
            {
                int intUser = Int32.Parse(dr["userid"].ToString());
                string strComment = "<p>" + oUser.GetFullName(_userid) + " wrote:<br/>" + _comment + "</p>";
                string strDefault = oUser.GetApplicationUrl(intUser, _viewpage);
                if (strDefault == "")
                    oFunction.SendEmail("ClearView - Project Request Discussion Notification", oUser.GetName(intUser), "", strEMailIdsBCC, "ClearView - Project Request Discussion Notification", "<p><b>A new comment has been posted to the discussion section for the following project request...</b></p><p>" + strBody + "</p><p>" + strComment + "</p>", true, false);
                else
                    oFunction.SendEmail("ClearView - Project Request Discussion Notification", oUser.GetName(intUser), "", strEMailIdsBCC, "ClearView - Project Request Discussion Notification", "<p><b>A new comment has been posted to the discussion section for the following project request...</b></p><p><a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=/" + strDefault + oPage.GetFullLink(_viewpage) + "?rid=" + _requestid.ToString() + "&div=C\" target=\"_blank\">Click here to view this project request.</a></p><p>" + strBody + "</p><p>" + strComment + "</p>", true, false);
            }
        }
        public void DeleteComment(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteProjectRequestComment", arParams);
        }
        public DataSet GetComments(int _requestid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getProjectRequestComments", arParams);
        }
        public DataSet GetCommentsNotify(int _requestid)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getProjectRequestStatusDetailComments", arParams);
        }
        public void AddPriority(int _requestid, double _expected_cost, double _cost_avoidance, double _impact_analysis)
        {
            double _overall_priority = (_expected_cost + _cost_avoidance + _impact_analysis);
            double dblTotal = 3.00;
            _overall_priority = _overall_priority / dblTotal;
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@requestid", _requestid);
            arParams[1] = new SqlParameter("@expected_cost", _expected_cost);
            arParams[2] = new SqlParameter("@cost_avoidance", _cost_avoidance);
            arParams[3] = new SqlParameter("@impact_analysis", _impact_analysis);
            arParams[4] = new SqlParameter("@overall_priority", _overall_priority);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addProjectRequestPriority", arParams);
        }
        public DataSet GetReminder()
        {
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getProjectRequestsReminder");
        }

        public string GetBodyFull(int _requestid, int _environment)
        {
            Users oUser = new Users(user, dsn);
            Projects oProject = new Projects(user, dsn);
            Requests oRequest = new Requests(user, dsn);
            Variables oVariable = new Variables(_environment);
            Functions oFunction = new Functions(0, dsn, _environment);
            StatusLevels oStatusLevel = new StatusLevels();
            RequestItems oRequestItem = new RequestItems(user, dsn);
            StringBuilder sbBody = new StringBuilder();
            string strSpacerRow = "<tr><td colspan=\"3\"><img src=\"" + oVariable.ImageURL() + "/images/spacer.gif\" border=\"0\" width=\"1\" height=\"7\" /></td></tr>";
            DataSet ds = Get(_requestid);
            DataSet dsRequest = oRequest.Get(_requestid);
            if (ds.Tables[0].Rows.Count > 0 && dsRequest.Tables[0].Rows.Count > 0) 
            {
                int intProject = Int32.Parse(dsRequest.Tables[0].Rows[0]["projectid"].ToString());
                sbBody.Append("<tr><td nowrap><b>Executive Sponsor:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                sbBody.Append(oUser.GetFullName(Int32.Parse(oProject.Get(intProject, "executive"))));
                sbBody.Append("</td></tr>");
                sbBody.Append(strSpacerRow);
                sbBody.Append("<tr><td nowrap><b>Working Sponsor:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                sbBody.Append(oUser.GetFullName(Int32.Parse(oProject.Get(intProject, "working"))));
                sbBody.Append("</td></tr>");
                sbBody.Append(strSpacerRow);
                sbBody.Append("<tr><td nowrap><b>Require a C1:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                sbBody.Append(ds.Tables[0].Rows[0]["c1"].ToString() == "1" ? "Yes" : "No");
                sbBody.Append("</td></tr>");
                sbBody.Append(strSpacerRow);
                sbBody.Append("<tr><td nowrap><b>End Life:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                sbBody.Append(ds.Tables[0].Rows[0]["endlife"].ToString() == "1" ? "Yes" : "No");
                sbBody.Append("</td></tr>");
                sbBody.Append(strSpacerRow);
                sbBody.Append("<tr><td nowrap><img src=\"");
                sbBody.Append(oVariable.ImageURL());
                sbBody.Append("/images/spacer.gif\" border=\"0\" width=\"40\" height=\"1\" /><b>End Life Date:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                sbBody.Append(ds.Tables[0].Rows[0]["endlife_date"].ToString() != "" ? DateTime.Parse(ds.Tables[0].Rows[0]["endlife_date"].ToString()).ToShortDateString() : "");
                sbBody.Append("</td></tr>");
                sbBody.Append(strSpacerRow);
                sbBody.Append("<tr><td nowrap><b>Initiative Opportunity:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                sbBody.Append(oFunction.FormatText(dsRequest.Tables[0].Rows[0]["description"].ToString()));
                sbBody.Append("</td></tr>");
                sbBody.Append(strSpacerRow);
                string strPlatforms = "";
                DataSet dsPlatform = GetPlatforms(_requestid);
                foreach (DataRow drPlatform in dsPlatform.Tables[0].Rows)
                    strPlatforms += drPlatform["name"].ToString() + "<br/>";
                sbBody.Append("<tr><td nowrap><b>Platform(s):</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                sbBody.Append(strPlatforms);
                sbBody.Append("</td></tr>");
                sbBody.Append(strSpacerRow);
                sbBody.Append("<tr><td nowrap><b>Audit Requirement:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                sbBody.Append(ds.Tables[0].Rows[0]["endlife"].ToString() == "1" ? "Yes" : "No");
                sbBody.Append("</td></tr>");
                sbBody.Append(strSpacerRow);
                sbBody.Append("<tr><td nowrap><img src=\"");
                sbBody.Append(oVariable.ImageURL());
                sbBody.Append("/images/spacer.gif\" border=\"0\" width=\"40\" height=\"1\" /><b>Audit Requirement Date:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                sbBody.Append(ds.Tables[0].Rows[0]["req_date"].ToString() != "" ? DateTime.Parse(ds.Tables[0].Rows[0]["req_date"].ToString()).ToShortDateString() : "");
                sbBody.Append("</td></tr>");
                sbBody.Append(strSpacerRow);
                sbBody.Append("<tr><td nowrap><b>Interdependency With Other Projects/Initiatives:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                sbBody.Append(ds.Tables[0].Rows[0]["interdependency"].ToString());
                sbBody.Append("</td></tr>");
                sbBody.Append(strSpacerRow);
                sbBody.Append("<tr><td nowrap><img src=\"");
                sbBody.Append(oVariable.ImageURL());
                sbBody.Append("/images/spacer.gif\" border=\"0\" width=\"40\" height=\"1\" /><b>Project Name(s):</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                sbBody.Append(ds.Tables[0].Rows[0]["projects"].ToString());
                sbBody.Append("</td></tr>");
                sbBody.Append(strSpacerRow);
                sbBody.Append("<tr><td nowrap><b>Technology Capability:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                sbBody.Append(ds.Tables[0].Rows[0]["capability"].ToString());
                sbBody.Append("</td></tr>");
                sbBody.Append(strSpacerRow);
                sbBody.Append("<tr><td nowrap><b>Estimated Man Hours:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                sbBody.Append(ds.Tables[0].Rows[0]["man_hours"].ToString());
                sbBody.Append("</td></tr>");
                sbBody.Append(strSpacerRow);
                sbBody.Append("<tr><td nowrap><b>Proposed Discovery Start Date:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                sbBody.Append(DateTime.Parse(dsRequest.Tables[0].Rows[0]["start_date"].ToString()).ToShortDateString());
                sbBody.Append("</td></tr>");
                sbBody.Append(strSpacerRow);
                sbBody.Append("<tr><td nowrap><b>Expected Project Completion Date:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                sbBody.Append(DateTime.Parse(dsRequest.Tables[0].Rows[0]["end_date"].ToString()).ToShortDateString());
                sbBody.Append("</td></tr>");
                sbBody.Append(strSpacerRow);
                sbBody.Append("<tr><td nowrap><b>Expected Capital Cost:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                sbBody.Append(ds.Tables[0].Rows[0]["expected_capital"].ToString());
                sbBody.Append("</td></tr>");
                sbBody.Append(strSpacerRow);
                sbBody.Append("<tr><td nowrap><b>Internal Labor:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                sbBody.Append(ds.Tables[0].Rows[0]["internal_labor"].ToString());
                sbBody.Append("</td></tr>");
                sbBody.Append(strSpacerRow);
                sbBody.Append("<tr><td nowrap><b>External Labor:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                sbBody.Append(ds.Tables[0].Rows[0]["external_labor"].ToString());
                sbBody.Append("</td></tr>");
                sbBody.Append(strSpacerRow);
                sbBody.Append("<tr><td nowrap><b>Maintenance Cost Increase:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                sbBody.Append(ds.Tables[0].Rows[0]["maintenance_increase"].ToString());
                sbBody.Append("</td></tr>");
                sbBody.Append(strSpacerRow);
                sbBody.Append("<tr><td nowrap><b>Project Expenses:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                sbBody.Append(ds.Tables[0].Rows[0]["project_expenses"].ToString());
                sbBody.Append("</td></tr>");
                sbBody.Append(strSpacerRow);
                sbBody.Append("<tr><td nowrap><b>Estimated Net Cost Avoidance:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                sbBody.Append(ds.Tables[0].Rows[0]["estimated_avoidance"].ToString());
                sbBody.Append("</td></tr>");
                sbBody.Append(strSpacerRow);
                sbBody.Append("<tr><td nowrap><b>Estimated Net Cost Savings:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                sbBody.Append(ds.Tables[0].Rows[0]["estimated_savings"].ToString());
                sbBody.Append("</td></tr>");
                sbBody.Append(strSpacerRow);
                sbBody.Append("<tr><td nowrap><b>Realized Cost Savings:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                sbBody.Append(ds.Tables[0].Rows[0]["realized_savings"].ToString());
                sbBody.Append("</td></tr>");
                sbBody.Append(strSpacerRow);
                sbBody.Append("<tr><td nowrap><b>Business Impact Aviodance:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                sbBody.Append(ds.Tables[0].Rows[0]["business_avoidance"].ToString());
                sbBody.Append("</td></tr>");
                sbBody.Append(strSpacerRow);
                sbBody.Append("<tr><td nowrap><b>Maintenance Cost Avoidance:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                sbBody.Append(ds.Tables[0].Rows[0]["maintenance_avoidance"].ToString());
                sbBody.Append("</td></tr>");
                sbBody.Append(strSpacerRow);
                sbBody.Append("<tr><td nowrap><b>Asset Reusability:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                sbBody.Append(ds.Tables[0].Rows[0]["asset_reusability"].ToString());
                sbBody.Append("</td></tr>");
                sbBody.Append(strSpacerRow);
                sbBody.Append("<tr><td nowrap><b>Internal Customer Impact:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                sbBody.Append(ds.Tables[0].Rows[0]["internal_impact"].ToString());
                sbBody.Append("</td></tr>");
                sbBody.Append(strSpacerRow);
                sbBody.Append("<tr><td nowrap><b>External Customer Impact:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                sbBody.Append(ds.Tables[0].Rows[0]["external_impact"].ToString());
                sbBody.Append("</td></tr>");
                sbBody.Append(strSpacerRow);
                sbBody.Append("<tr><td nowrap><b>Business Impact:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                sbBody.Append(ds.Tables[0].Rows[0]["business_impact"].ToString());
                sbBody.Append("</td></tr>");
                sbBody.Append(strSpacerRow);
                sbBody.Append("<tr><td nowrap><b>Strategic Opportunity:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                sbBody.Append(ds.Tables[0].Rows[0]["strategic_opportunity"].ToString());
                sbBody.Append("</td></tr>");
                sbBody.Append(strSpacerRow);
                sbBody.Append("<tr><td nowrap><b>Acquisition / BIC:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                sbBody.Append(ds.Tables[0].Rows[0]["acquisition"].ToString());
                sbBody.Append("</td></tr>");
                sbBody.Append(strSpacerRow);
                sbBody.Append("<tr><td nowrap><b>Technology Capabilities:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                sbBody.Append(ds.Tables[0].Rows[0]["technology_capabilities"].ToString());
                sbBody.Append("</td></tr>");
                sbBody.Append(strSpacerRow);
                sbBody.Append("<tr><td nowrap><b>Technical Project Manager Requested:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                sbBody.Append(ds.Tables[0].Rows[0]["tpm"].ToString() == "1" ? "Yes" : "No");
                sbBody.Append("</td></tr>");
                sbBody.Append(strSpacerRow);
                DataSet dsTempResources = GetResources(_requestid);
                if (dsTempResources.Tables[0].Rows.Count > 0)
                {
                    int intItem = Int32.Parse(dsTempResources.Tables[0].Rows[0]["itemid"].ToString());
                    int intAccepted = Int32.Parse(dsTempResources.Tables[0].Rows[0]["accepted"].ToString());
                    int intManager = Int32.Parse(dsTempResources.Tables[0].Rows[0]["userid"].ToString());
                    if (intItem > 0)
                    {
                        sbBody.Append("<tr><td nowrap><img src=\"");
                        sbBody.Append(oVariable.ImageURL());
                        sbBody.Append("/images/spacer.gif\" border=\"0\" width=\"40\" height=\"1\" /><b>Service Type:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                        sbBody.Append(oRequestItem.GetItemName(intItem));
                        sbBody.Append("</td></tr>");
                        sbBody.Append(strSpacerRow);
                    }
                    if (intAccepted == -1) 
                    {
                        sbBody.Append("<tr><td nowrap><img src=\"");
                        sbBody.Append(oVariable.ImageURL());
                        sbBody.Append("/images/spacer.gif\" border=\"0\" width=\"40\" height=\"1\" /><b>Project Lead:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>Pending Assignment (");
                        sbBody.Append(oUser.GetFullName(Int32.Parse(dsRequest.Tables[0].Rows[0]["userid"].ToString())));
                        sbBody.Append(")</td></tr>");
                        sbBody.Append(strSpacerRow);
                    }
                    else if (intManager > 0) 
                    {
                        sbBody.Append("<tr><td nowrap><img src=\"");
                        sbBody.Append(oVariable.ImageURL());
                        sbBody.Append("/images/spacer.gif\" border=\"0\" width=\"40\" height=\"1\" /><b>Project Lead:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>");
                        sbBody.Append(oUser.GetFullName(intManager));
                        sbBody.Append("</td></tr>");
                        sbBody.Append(strSpacerRow);
                    }
                    else if (intItem > 0) 
                    {
                        sbBody.Append("<tr><td nowrap><img src=\"");
                        sbBody.Append(oVariable.ImageURL());
                        sbBody.Append("/images/spacer.gif\" border=\"0\" width=\"40\" height=\"1\" /><b>Project Lead:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>Pending Assignment</td></tr>");
                        sbBody.Append(strSpacerRow);
                    }
                }
            }
            if (sbBody.ToString() != "")
            {
                sbBody.Insert(0, "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"" + oVariable.DefaultFontStyle() + "\">");
                sbBody.Append("</table>");
            }
            
            return sbBody.ToString();
        }

        // Vijay Code - Start
        #region Project_Prioritization
        #region Questions
        public int AddQuestion(string _name, string _question, int _display, int _enabled, int _required)
        {
            arParams = new SqlParameter[6];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@question", _question);
            arParams[2] = new SqlParameter("@display", _display);
            arParams[3] = new SqlParameter("@enabled", _enabled);
            arParams[4] = new SqlParameter("@required", _required);
            arParams[5] = new SqlParameter("@id", SqlDbType.Int);
            arParams[5].Direction = ParameterDirection.Output;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addProjectRequestQuestion", arParams);
            return Int32.Parse(arParams[5].Value.ToString());

        }
        public void UpdateQuestion(int _id, string _name, string _question, int _enabled, int _required)
        {
            arParams = new SqlParameter[5];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@question", _question);
            arParams[3] = new SqlParameter("@enabled", _enabled);
            arParams[4] = new SqlParameter("@required", _required);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateProjectRequestQuestion", arParams);
        }
        public void UpdateQuestionOrder(int _id, int _display)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@display", _display);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateProjectRequestQuestionOrder", arParams);
        }
        public void EnableQuestion(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateProjectRequestQuestionEnabled", arParams);
        }
        public void DeleteQuestion(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteProjectRequestQuestion", arParams);
        }
        public string GetQuestion(int _id, string _column)
        {
            DataSet ds = GetQuestion(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }

        public DataSet GetQuestion(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getProjectRequestQuestion", arParams);
        }
        public DataSet GetQuestions(int _enabled)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getProjectRequestQuestions", arParams);
        }
        #endregion
        #region Responses
        public void AddResponse(int _questionid, string _name, string _response, int _weight, int _display, int _enabled)
        {
            arParams = new SqlParameter[6];
            arParams[0] = new SqlParameter("@questionid", _questionid);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@response", _response);
            arParams[3] = new SqlParameter("@weight", _weight);
            arParams[4] = new SqlParameter("@display", _display);
            arParams[5] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addProjectRequestResponse", arParams);
        }
        public void UpdateResponse(int _id, int _questionid, string _name, string _response, int _weight, int _enabled)
        {
            arParams = new SqlParameter[6];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@questionid", _questionid);
            arParams[2] = new SqlParameter("@name", _name);
            arParams[3] = new SqlParameter("@response", _response);
            arParams[4] = new SqlParameter("@weight", _weight);
            arParams[5] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateProjectRequestResponse", arParams);
        }
        public void UpdateResponseOrder(int _id, int _display)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@display", _display);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateProjectRequestResponseOrder", arParams);
        }
        public void EnableResponse(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateProjectRequestResponseEnabled", arParams);
        }
        public void DeleteResponse(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteProjectRequestResponse", arParams);
        }
        public string GetResponse(int _id, string _column)
        {
            DataSet ds = GetResponse(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public DataSet GetResponse(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getProjectRequestResponse", arParams);
        }
        public DataSet GetResponses(int _questionid, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@questionid", _questionid);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getProjectRequestResponses", arParams);
        }
        public DataSet GetResponsesQuestions(int _classid)
        {
            arParams = new SqlParameter[1];
            // arParams[0] = new SqlParameter("@responseid", _responseid);                     
            arParams[0] = new SqlParameter("@classid", _classid);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getProjectRequestResponsesQuestions", arParams);
        }

        #endregion
        #region QA
        public void AddQA(string _bd, int _organization_id, int _question_id)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@bd", _bd);
            arParams[1] = new SqlParameter("@organizationid", _organization_id);
            arParams[2] = new SqlParameter("@questionid", _question_id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addProjectRequestQA", arParams);
        }
        public void UpdateQA(int _id, string _bd, int _organization_id, int _question_id)
        {
            arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@bd", _bd);
            arParams[2] = new SqlParameter("@organizationid", _organization_id);
            arParams[3] = new SqlParameter("@questionid", _question_id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateProjectRequestQA", arParams);
        }
        public void DeleteQA(string _bd,int _organization_id)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@bd", _bd);
            arParams[1] = new SqlParameter("@organizationid", _organization_id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteProjectRequestQA", arParams);
        }
        public void DeleteQA(int _question_id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@questionid", _question_id);             
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteProjectRequestQAbyQuestion", arParams);
        }
        public void DeleteQA(string _bd, int _organization_id,int _question_id)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@bd", _bd);
            arParams[1] = new SqlParameter("@organizationid", _organization_id);
            arParams[2] = new SqlParameter("@questionid", _question_id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteProjectRequestQAbyQuestionAndPortfolio", arParams);
        }
        public DataSet GetQA(string _bd, int _organization_id)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@bd", _bd);
            arParams[1] = new SqlParameter("@organizationid", _organization_id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getProjectRequestQA", arParams);
        }
        public DataSet GetQAByQuestion(int _question_id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@questionid", _question_id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getProjectRequestQAByQuestion", arParams);
        }
        public DataSet GetQAs()
        {
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getProjectRequestQAs");
        }
        #endregion
        #region Classes
        public void AddClass(string _name, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@name", _name);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addProjectRequestClass", arParams);
        }
        public void UpdateClass(int _id, string _name, int _enabled)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateProjectRequestClass", arParams);
        }

        public void EnableClass(int _id, int _enabled)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateProjectRequestClassEnabled", arParams);
        }
        public void DeleteClass(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteProjectRequestClass", arParams);
        }
        public string GetClass(int _id, string _column)
        {
            DataSet ds = GetClass(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public DataSet GetClass(int _id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getProjectRequestClass", arParams);
        }
        public DataSet GetClasses(int _enabled)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getProjectRequestClasses", arParams);
        }

        #endregion
        #region Project_Request_Response_Class

        public string GetResponseWeight(int _response_id, int _question_id)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@responseid", _response_id);
            arParams[1] = new SqlParameter("@questionid", _question_id);
            object obj = SqlHelper.ExecuteScalar(dsn, CommandType.StoredProcedure, "pr_getProjectRequestResponsesWeight", arParams);
            return obj == null ? "0" : obj.ToString();
        }

        #endregion
        #region Project_Request_Submission
        public void AddSubmission(int _request_id, int _question_id, int _response_id)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@requestid", _request_id);
            arParams[1] = new SqlParameter("@questionid", _question_id);
            arParams[2] = new SqlParameter("@responseid", _response_id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addProjectRequestSubmission", arParams);
        }

        public void UpdateSubmission(int _request_id, int _question_id, int _response_id)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@requestid", _request_id);
            arParams[1] = new SqlParameter("@questionid", _question_id);
            arParams[2] = new SqlParameter("@responseid", _response_id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateProjectRequestSubmission", arParams);
        }
        #endregion
        #region Project_Request_Weight_Priority
        public void AddWeightPriority(int _request_id, int _class_id, double _weight)
        {
            arParams = new SqlParameter[3];
            arParams[0] = new SqlParameter("@requestid", _request_id);
            arParams[1] = new SqlParameter("@classid", _class_id);
            arParams[2] = new SqlParameter("@weight", _weight);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addProjectRequestWeightPriority", arParams);
        }

        public DataSet GetWeightPriority(int _request_id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@requestid", _request_id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getProjectRequestWeightPriority", arParams);
        }

        #endregion
        #region Project_Request_Questions_Class
        public void AddQuestionsClass(int _question_id, int _class_id)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@questionid", _question_id);
            arParams[1] = new SqlParameter("@classid", _class_id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addProjectRequestQuestionsClass", arParams);
        }
        public DataSet GetQuestionsClass(int _question_id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@questionid", _question_id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getProjectRequestQuestionsClass", arParams);
        }
        public DataSet GetQuestionsByClass(int _class_id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@classid", _class_id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getProjectRequestQuestionsbyClass", arParams);
        }
        public DataSet DeleteQuestionsClass(int _question_id)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@questionid", _question_id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_deleteProjectRequestQuestionsClass", arParams);
        }

        public void UpdateQuestionsClass(int _question_id, int _class_id)
        {
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@questionid", _question_id);
            arParams[1] = new SqlParameter("@classid", _class_id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateProjectRequestQuestionsClass", arParams);
        }
        #endregion

        #endregion
        // Vijay Code - End

      
        public string MaximumProjectRequestResponses(int _intQuestionId)
        {
            return SqlHelper.ExecuteScalar(dsn, CommandType.Text, "select max(display) from cv_project_request_responses where questionid=" + _intQuestionId).ToString();
        }

        public int ProjectRequestResponsesCount(int _intQuestionId, int _intWeight)
        {
            return Int32.Parse(SqlHelper.ExecuteScalar(dsn, CommandType.Text, "select count(weight) from cv_project_request_responses where questionid=" + _intQuestionId + " and weight=" + _intWeight + " and deleted=0").ToString());
        }

        public int ProjectRequestResponsesCount(int _intQuestionId, int _intWeight, int _intResponseId)
        {
            return Int32.Parse(SqlHelper.ExecuteScalar(dsn, CommandType.Text, "select count(weight) from cv_project_request_responses where questionid=" + _intQuestionId + " and weight=" + _intWeight + " and deleted=0 and id=" + _intResponseId).ToString());
        }

        public string MaximumProjectRequestQuestions()
        {
            return SqlHelper.ExecuteScalar(dsn, CommandType.Text, "select max(display) from cv_project_request_questions").ToString();
        }
    }
}
