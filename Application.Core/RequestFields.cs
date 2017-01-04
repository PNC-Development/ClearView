using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.IO;
using System.Web;
using System.Configuration;
using System.Text;
using System.Collections.Generic;

namespace NCC.ClearView.Application.Core
{
	public class RequestFields
	{
		private string dsn = "";
		private int user = 0;
		private SqlParameter[] arParams;
		private DataSet ds;
        public RequestFields(int _user, string _dsn)
		{
			user = _user;
			dsn = _dsn;
		}
		public DataSet GetName(int _id)
		{
			arParams = new SqlParameter[1];
			arParams[0] = new SqlParameter("@id", _id);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getFieldName", arParams);
		}
        public int GetName(string _name)
        {
            arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@name", _name);
            object o = SqlHelper.ExecuteScalar(dsn, CommandType.StoredProcedure, "pr_getFieldNameName", arParams);
            if (o == null)
                return 0;
            else
                return Int32.Parse(o.ToString());
        }
        public string GetName(int _id, string _column)
        {
            ds = GetName(_id);
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0][_column].ToString();
            else
                return "";
        }
        public DataSet GetNames(int _enabled)
		{
			arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@enabled", _enabled);
            return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getFieldNames", arParams);
		}
        public void AddName(int _nameid, string _name, string _datatype, int _enabled)
        {
			arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@nameid", _nameid);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@datatype", _datatype);
            arParams[3] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addFieldName", arParams);
		}
        public void UpdateName(int _id, string _name, string _datatype, int _enabled)
		{
			arParams = new SqlParameter[4];
			arParams[0] = new SqlParameter("@id", _id);
            arParams[1] = new SqlParameter("@name", _name);
            arParams[2] = new SqlParameter("@datatype", _datatype);
            arParams[3] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateFieldName", arParams);
		}
        public void EnableName(int _id, int _enabled) 
		{
			arParams = new SqlParameter[2];
			arParams[0] = new SqlParameter("@id", _id);
			arParams[1] = new SqlParameter("@enabled", _enabled);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateFieldNameEnabled", arParams);
		}
        public void DeleteName(int _id)
		{
			arParams = new SqlParameter[1];
			arParams[0] = new SqlParameter("@id", _id);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteFieldName", arParams);
		}

        public string GetBodyOverall(int _resourcerequestid, int _resourcerequestworkflowid, string _se_dsn, int _environment, string _dsn_asset, string _dsn_ip)
        {
            ResourceRequest oResourceRequest = new ResourceRequest(user, dsn);
            DataSet dsResource = oResourceRequest.Get(_resourcerequestid);
            if (dsResource.Tables[0].Rows.Count > 0)
                return GetBodyNoEnv(Int32.Parse(dsResource.Tables[0].Rows[0]["requestid"].ToString()), Int32.Parse(dsResource.Tables[0].Rows[0]["itemid"].ToString()), Int32.Parse(dsResource.Tables[0].Rows[0]["number"].ToString()), Int32.Parse(dsResource.Tables[0].Rows[0]["serviceid"].ToString()), _resourcerequestid, _resourcerequestworkflowid, _se_dsn, _environment, _dsn_asset, _dsn_ip);
            else
                return "** WARNING: No Resource Request for ID " + _resourcerequestid.ToString();
        }
        public string GetBodyWorkflow(int _resourcerequestworkflowid, string _se_dsn, int _environment, string _dsn_asset, string _dsn_ip)
        {
            ResourceRequest oResourceRequest = new ResourceRequest(user, dsn);
            return GetBodyOverall(oResourceRequest.GetWorkflowParent(_resourcerequestworkflowid), _resourcerequestworkflowid, _se_dsn, _environment, _dsn_asset, _dsn_ip);
        }
        public string GetBodyNoEnv(int _requestid, int _itemid, int _number, int _serviceid, int _rrid, int _rr_workflowid, string _se_dsn, int _environment, string _dsn_asset, string _dsn_ip)
        {
            return GetBody(_requestid, _itemid, _number, _serviceid, _rrid, _rr_workflowid, _se_dsn, _environment, _dsn_asset, _dsn_ip);
        }
        public string GetBody(int _requestid, int _itemid, int _number, int _serviceid, int _rrid, int _rr_workflowid, string _se_dsn, int _environment, string _dsn_asset, string _dsn_ip)
        {
            string strView = "";
            Applications oApplication = new Applications(user, dsn);
            RequestItems oRequestItem = new RequestItems(user, dsn);
            ResourceRequest oResourceRequest = new ResourceRequest(user, dsn);
            Users oUser = new Users(user, dsn);
            Services oService = new Services(user, dsn);
            Variables oVariable = new Variables(_environment);
            Functions oFunction = new Functions(0, dsn, _environment);
            Requests oRequest = new Requests(user, dsn);
            Projects oProject = new Projects(user, dsn);
            ServiceRequests oServiceRequest = new ServiceRequests(user, dsn);
            ServiceEditor oServiceEditor = new ServiceEditor(user, _se_dsn);
            Servers oServer= new Servers(user, dsn);
            Workstations oWorkstation = new Workstations(user, dsn);
            AssetOrder oAssetOrder = new AssetOrder(user, dsn, _dsn_asset, _environment);
            AssetSharedEnvOrder oAssetSharedEnvOrder = new AssetSharedEnvOrder(user, dsn, _dsn_asset, _environment);
            PNCTasks oPNCTask = new PNCTasks(user, dsn);
            TSM oTSM = new TSM(user, dsn);
            StatusLevels oStatusLevel = new StatusLevels();

            DataSet dsRR = oResourceRequest.GetRequestService(_requestid, _serviceid, _number);
            if (dsRR.Tables[0].Rows.Count > 0)
            {
                if (_rrid == 0)
                    Int32.TryParse(dsRR.Tables[0].Rows[0]["parent"].ToString(), out _rrid);
                if (_rr_workflowid == 0)
                    Int32.TryParse(dsRR.Tables[0].Rows[0]["id"].ToString(), out _rr_workflowid);
            }

            // Workflow
            string strWorkflowName = "";
            int intProject = oRequest.GetProjectNumber(_requestid);
            if (intProject > 0)
            {
                strView += "<tr><td valign=\"top\"><b>Project Name:</b></td>";
                strView += "<td colspan=\"40\">" + oProject.Get(intProject, "name") + "</td></tr>";
                strView += "<tr><td valign=\"top\"><b>Project Number:</b></td>";
                strView += "<td>" + "<a href=\"" + oRequest.GetDataPointLink(_requestid, _environment) + "\" target=\"_blank\">" + oProject.Get(intProject, "number") + "</a></td></tr>";
            }
            else
            {
                string strTaskName = "N/A";
                if (_rr_workflowid > 0)
                    strTaskName = oResourceRequest.GetWorkflow(_rr_workflowid, "name").Trim();
                if ((strTaskName == "" || strTaskName == "N/A") && _rrid > 0)
                    strTaskName = oResourceRequest.Get(_rrid, "name").Trim();
                if ((strTaskName == "" || strTaskName == "N/A") && _requestid > 0)
                    strTaskName = oServiceRequest.Get(_requestid, "name").Trim();
                strView += "<tr><td valign=\"top\"><b>Task Name:</b></td>";
                strView += "<td colspan=\"40\">" + strTaskName + "</td></tr>";
            }
            strView += "<tr><td valign=\"top\"><b>Task Number:</b></td>";
            strView += "<td>" + "<a href=\"javascript:void(0);\" class=\"lookup\" onclick=\"OpenNewWindowMenu('/datapoint/service/resource.aspx?id=" + oFunction.encryptQueryString(_rrid.ToString()) + "', '800', '600');\">CVT" + _requestid.ToString() + "-" + _serviceid.ToString() + "-" + _number.ToString() + "</a></td></tr>";
            //strWorkflowName += "<td>" + "<a href=\"javascript:void(0);\" class=\"lookup\" onclick=\"OpenNewWindowMenu('" + oRequest.GetDataPointLink(_requestid, _environment) + "', '800', '600');\">CVT" + _requestid.ToString() + "</a></td>";

            string strWorkflowRequested = "";
            string strWorkflowCreated = "";
            string strWorkflowService = "";
            string strWorkflowDepartment = "";
            string strWorkflowResources = "";
            string strWorkflowStatus = "";
            string strWorkflowComments = "";
            int intStatus = 0;
            // Requested
            strWorkflowRequested += "<td valign=\"top\"><b>Requested By:</b></td>";
            int intRequestor = oRequest.GetUser(_requestid);
            int intRequestorManager = 0;
            Int32.TryParse(oUser.Get(intRequestor, "manager"), out intRequestorManager);
            Int32.TryParse(oResourceRequest.Get(_rrid, "status"), out intStatus);
            strWorkflowRequested += "<td>" + oUser.GetFullName(intRequestor) + " (" + oUser.GetName(intRequestor) + ")" + "</td>";
            strWorkflowCreated += "<td valign=\"top\"><b>Created On:</b></td>";
            if (_rrid > 0)
                strWorkflowCreated += "<td>" + DateTime.Parse(oResourceRequest.Get(_rrid, "created")).ToLongDateString() + "</td>";
            else
                strWorkflowCreated += "<td>" + DateTime.Parse(oRequest.Get(_requestid, "created")).ToLongDateString() + "</td>";
            // Service
            strWorkflowService += "<td valign=\"top\"><b>Service:</b></td>";
            if (_serviceid == 0)
                strWorkflowService += "<td>" + oRequestItem.GetItem(_itemid, "service_title") + "</td>";
            else
                strWorkflowService += "<td>" + oService.GetName(_serviceid) + "</td>";
            // Department
            strWorkflowDepartment += "<td valign=\"top\"><b>Department:</b></td>";
            strWorkflowDepartment += "<td>" + oApplication.Get(oRequestItem.GetItemApplication(_itemid), "service_title") + "</td>";
            // Resources
            string strApprovers = "";
            DataSet dsApprovals = oResourceRequest.GetApprovals(_requestid, _serviceid, _number);
            foreach (DataRow drApprover in dsApprovals.Tables[0].Rows)
            {
                int intApprover = Int32.Parse(drApprover["userid"].ToString());
                if (drApprover["approved"].ToString() == "" && drApprover["denied"].ToString() == "")
                {
                    if (strApprovers != "")
                        strApprovers += "\\n";
                    strApprovers += oUser.GetFullName(intApprover) + " (" + oUser.GetName(intApprover) + ")";
                }
            }
            bool boolAutomated = (oService.Get(_serviceid, "automate") == "1");
            if (boolAutomated == false)
            {
                DataSet dsReqForm = oRequestItem.GetForm(_requestid, _serviceid, _itemid, _number);
                if (dsReqForm.Tables[0].Rows.Count > 0)
                    boolAutomated = (dsReqForm.Tables[0].Rows[0]["automated"].ToString() == "1" ? true : false);
            
            }
            strWorkflowResources += "<td valign=\"top\"><b>Assigned:</b></td>";
            strWorkflowStatus += "<td><b>Status:</b></td>";
            if (boolAutomated == true)
            {
                strWorkflowResources += "<td valign=\"top\">---</td>";
                intStatus = (int)ResourceRequestStatus.NotAvailable;  // Set to N/A since it is automated...
                strWorkflowStatus += "<td>" + oStatusLevel.HTML(intStatus) + "</td>";
            }
            else
            {
                List<WorkflowStatus> RR = oResourceRequest.GetStatus(null, _rrid, null, null, null, null, false, _se_dsn);
                if (RR.Count > 0)
                {
                    StringBuilder strUsers = new StringBuilder();
                    foreach (string strUser in RR[0].users)
                    {
                        if (String.IsNullOrEmpty(strUser) == false)
                        {
                            strUsers.Append(strUser);
                            strUsers.AppendLine("<br/>");
                        }
                    }
                    strWorkflowResources += "<td valign=\"top\">" + strUsers.ToString() + "</td>";
                    strWorkflowStatus += "<td>" + RR[0].status + "</td>";
                    if (String.IsNullOrEmpty(RR[0].comments) == false)
                    {
                        strWorkflowComments += "<td valign=\"top\"><b>Comments:</b></td>";
                        strWorkflowComments += "<td valign=\"top\">" + oFunction.FormatText(RR[0].comments) + "</td>";
                    }
                }
                else
                {
                    strWorkflowResources += "<td valign=\"top\"> N / A </td>";
                    strWorkflowStatus += "<td> N / A </td>";
                }
            }
            strView += "<tr>" + strWorkflowRequested + "</tr><tr>" + strWorkflowCreated + "</tr><tr>" + strWorkflowService + "</tr><tr>" + strWorkflowDepartment + "</tr>" + "<tr>" + strWorkflowStatus + "</tr>" + (strWorkflowResources == "" ? "" : "<tr>" + strWorkflowResources + "</tr>") + (strWorkflowComments == "" ? "" : "<tr>" + strWorkflowComments + "</tr>");

            if (strView == "")
                strView = "Information Unavailable";
            else
            {
                strView = "<table cellpadding=\"3\" cellspacing=\"2\" border=\"0\" style=\"" + oVariable.DefaultFontStyle() + "\">" + strView + "</table>";
                StringBuilder sbViewRequest = new StringBuilder();
                Customized oCustomized = new Customized(user, dsn);
                DNS oDNS = new DNS(user, dsn);
                OnDemandTasks oOnDemandTask = new OnDemandTasks(user, dsn);
                Reports oReport = new Reports(user, dsn);
                Audit oAudit = new Audit(user, dsn);
                Enhancements oEnhancement = new Enhancements(user, dsn);
                ServerDecommission oServerDecommission = new ServerDecommission(0, dsn);
                Storage oStorage = new Storage(0, dsn);
                
                string strCatch = "1";
                try
                {
                    sbViewRequest.Append(oServiceEditor.GetRequestBody(_requestid, _serviceid, _number, dsn));
                    if (sbViewRequest.ToString() == "")
                    {
                        strCatch = "2";
                        sbViewRequest.Append(oCustomized.GetPNCDNSConflictBody(_requestid, _itemid, _number));
                    }
                    if (sbViewRequest.ToString() == "")
                    {
                        strCatch = "3";
                        sbViewRequest.Append(oCustomized.GetStorage3rdBody(_requestid, _itemid, _number, _environment));
                    }
                    if (sbViewRequest.ToString() == "")
                    {
                        strCatch = "4";
                        sbViewRequest.Append(oOnDemandTask.GetServerOther(_requestid, _serviceid, _number, _environment, _dsn_asset, _dsn_ip));
                    }
                    if (sbViewRequest.ToString() == "")
                    {
                        strCatch = "5";
                        sbViewRequest.Append(GetBody(oReport.GetOrderReport(_requestid, _itemid, _number), _serviceid));
                    }
                    if (sbViewRequest.ToString() == "")
                    {
                        strCatch = "6";
                        sbViewRequest.Append(oServerDecommission.GetBody(_requestid, _number, _dsn_asset, _environment));
                    }
                    if (sbViewRequest.ToString() == "")
                    {
                        strCatch = "7";
                        sbViewRequest.Append(oCustomized.GetDecommissionServerBody(_requestid, _itemid, _number, _dsn_asset));
                    }
                    if (sbViewRequest.ToString() == "")
                    {
                        strCatch = "7.1";
                        sbViewRequest.Append(oWorkstation.GetApprovalSummary(_requestid));
                    }
                    if (sbViewRequest.ToString() == "")
                    {
                        strCatch = "8";
                        int intEnhancementID = oCustomized.GetEnhancementID(_requestid);
                        if (intEnhancementID > 0)
                        {
                            sbViewRequest.Append("<tr><td>");
                            sbViewRequest.Append(oCustomized.GetEnhancementBody(intEnhancementID, _environment, false));
                            sbViewRequest.Append("</td></tr>");
                        }
                    }
                    if (sbViewRequest.ToString() == "")
                    {
                        strCatch = "9";
                        int intIssueID = oCustomized.GetIssueID(_requestid);
                        if (intIssueID > 0)
                        {
                            sbViewRequest.Append("<tr><td>");
                            sbViewRequest.Append(oCustomized.GetIssueBody(intIssueID, _environment, false));
                            sbViewRequest.Append("</td></tr>");
                        }
                    }
                    if (sbViewRequest.ToString() == "")
                    {
                        strCatch = "10";
                        sbViewRequest.Append(GetBody(oCustomized.GetIIS(_requestid, _itemid, _number), _serviceid));
                    }
                    if (sbViewRequest.ToString() == "")
                    {
                        strCatch = "11";
                        sbViewRequest.Append(GetBody(oCustomized.GetRemediation(_requestid, _itemid, _number), _serviceid));
                    }
                    if (sbViewRequest.ToString() == "")
                    {
                        strCatch = "12";
                        sbViewRequest.Append(GetBody(oCustomized.GetServerArchive(_requestid, _itemid, _number), _serviceid));
                    }
                    if (sbViewRequest.ToString() == "")
                    {
                        strCatch = "13";
                        sbViewRequest.Append(GetBody(oCustomized.GetServerRetrieve(_requestid, _itemid, _number), _serviceid));
                    }
                    if (sbViewRequest.ToString() == "")
                    {
                        strCatch = "14";
                        sbViewRequest.Append(GetBody(oCustomized.GetTPM(_requestid, _itemid, _number), _serviceid));
                    }
                    if (sbViewRequest.ToString() == "")
                    {
                        strCatch = "15";
                        sbViewRequest.Append(GetBody(oCustomized.GetWorkstation(_requestid, _itemid, _number), _serviceid));
                    }
                    if (sbViewRequest.ToString() == "")
                    {
                        strCatch = "16";
                        sbViewRequest.Append(GetBody(oCustomized.GetThirdTierDistributed(_requestid, _itemid, _number), _serviceid));
                    }
                    if (sbViewRequest.ToString() == "")
                    {
                        strCatch = "17";
                        sbViewRequest.Append(GetBody(oCustomized.GetGeneric(_requestid, _itemid, _number), _serviceid));
                    }
                    if (sbViewRequest.ToString() == "")
                    {
                        strCatch = "18";
                        sbViewRequest.Append(oDNS.GetDNSBody(_requestid, _itemid, _number, false, _environment));
                    }
                    if (sbViewRequest.ToString() == "")
                    {
                        strCatch = "19";
                        sbViewRequest.Append(oAudit.GetErrorBody(_requestid, _serviceid, _number));
                    }

                    if (sbViewRequest.ToString() == "")//Server Error Provisioning Support
                    {
                        strCatch = "20";
                        sbViewRequest.Append(oServer.GetErrorDetailsBody(_requestid, _itemid, _number, _environment));
                           
                    }

                    if (sbViewRequest.ToString() == "") //Workstation Error Provisioning Support
                    {
                        strCatch = "21";
                        sbViewRequest.Append(oWorkstation.GetVirtualErrorDetailsBody(_requestid, _number, _environment));
                    }

                    if (sbViewRequest.ToString() == "") //Asset Procurement
                    {
                        strCatch = "22";
                        sbViewRequest.Append(oAssetOrder.GetOrderBody(_requestid,_itemid,_number));
                    }

                    if (sbViewRequest.ToString() == "") //Shared Environement - IM
                    {
                        strCatch = "23";
                        sbViewRequest.Append(oAssetSharedEnvOrder.GetOrderBody(_requestid, _itemid, _number));
                    }

                    if (sbViewRequest.ToString() == "") //Backup
                    {
                        strCatch = "24";
                        sbViewRequest.Append(oTSM.GetBody(_requestid, _itemid, _number, _dsn_asset, _dsn_ip));
                    }

                    if (sbViewRequest.ToString() == "") // New Enhancement
                    {
                        strCatch = "25";
                        sbViewRequest.Append(oEnhancement.GetBodyRequest(_requestid, _environment));
                    }

                    if (sbViewRequest.ToString() == "") // New Enhancement
                    {
                        strCatch = "26";
                        sbViewRequest.Append(oStorage.GetBody(_requestid, _itemid, _number, _dsn_asset, _dsn_ip, _environment, false));
                    }


                    if (sbViewRequest.ToString() == "")
                    {
                        //strViewRequest = "Information Unavailable";
                        sbViewRequest.Append("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");
                    }
                    else
                    {
                        if (sbViewRequest.ToString().Trim().StartsWith("<table") == false)
                        {
                            sbViewRequest.Insert(0, "<table cellpadding=\"3\" cellspacing=\"2\" border=\"0\" style=\"" + oVariable.DefaultFontStyle() + "\">");
                            sbViewRequest.Append("</table>");
                        }
                    }
                }
                catch
                {
                    sbViewRequest = new StringBuilder("&nbsp;&nbsp;** WARNING: Information Unavailable (# " + strCatch + ") **&nbsp;&nbsp;");
                }
                sbViewRequest.Insert(0, "<table cellpadding=\"3\" cellspacing=\"2\" border=\"0\" style=\"" + oVariable.DefaultFontStyle() + "\"><tr><td>");
                sbViewRequest.Append("</td></tr></table>");
                strView += "<br/>" + sbViewRequest.ToString();
            }
            return strView;
        }
        private string GetBody(DataSet _dataset, int _serviceid)
        {
            StringBuilder sbReturn = new StringBuilder();
            Field oField = new Field(user, dsn);
            DataSet dsFields = oField.Gets2(_serviceid);
            foreach (DataRow dr in _dataset.Tables[0].Rows)
            {
                foreach (DataRow drField in dsFields.Tables[0].Rows)
                {
                    if (drField["hidden"].ToString() != "1")
                    {
                        sbReturn.Append("<tr>");
                        sbReturn.Append("<td valign=\"top\" nowrap><b>");
                        sbReturn.Append(drField["name"].ToString());
                        sbReturn.Append(":</b></td>");
                        if (_dataset.Tables[0].Columns.Contains(drField["fieldname"].ToString()) == true)
                        {
                            sbReturn.Append("<td>");
                            sbReturn.Append(GetValue(dr[drField["fieldname"].ToString()].ToString(), drField["datatype"].ToString(), drField["join_table"].ToString(), drField["join_on"].ToString(), drField["join_field"].ToString()));
                            sbReturn.Append("</td>");
                        }
                        else
                        {
                            sbReturn.Append("<td><i>Could not find column name ");
                            sbReturn.Append(drField["fieldname"].ToString());
                            sbReturn.Append(" for serviceID # ");
                            sbReturn.Append(_serviceid.ToString());
                            sbReturn.Append("</i></td>");
                        }
                        sbReturn.Append("</tr>");
                    }
                }
            }
            return sbReturn.ToString();
        }
        public string GetValue(string _value, string _datatype, string _join_table, string _join_on, string _join_field)
        {
            if (_value.Trim() != "")
            {
                switch (_datatype)
                {
                    case "F":
                        try { _value = double.Parse(_value).ToString("F"); }
                        catch { _value = "Error resolving DOUBLE value"; }
                        break;
                    case "D":
                        try { _value = DateTime.Parse(_value).ToShortDateString(); }
                        catch { _value = "Error resolving DATE value"; }
                        break;
                    case "T":
                        try { _value = DateTime.Parse(_value).ToShortTimeString(); }
                        catch { _value = "Error resolving TIME value"; }
                        break;
                    case "DT":
                        try { _value = DateTime.Parse(_value).ToShortDateString() + " " + DateTime.Parse(_value).ToShortTimeString(); }
                        catch { _value = "Error resolving DATETIME value"; }
                        break;
                    case "U":
                        Users oUser = new Users(user, dsn);
                        try { _value = oUser.GetFullName(Int32.Parse(_value)); }
                        catch { _value = "Error resolving USERID value"; }
                        break;
                    case "TF":
                        if (_value == "1")
                            _value = "True";
                        else
                            _value = "False";
                        break;
                    case "P":
                        if (_value == "1")
                            _value = "High";
                        else if (_value == "2")
                            _value = "Medium - High";
                        else if (_value == "3")
                            _value = "Medium";
                        else if (_value == "4")
                            _value = "Medium - Low";
                        else if (_value == "5")
                            _value = "Low";
                        break;
                    case "YN":
                        if (_value == "1")
                            _value = "Yes";
                        else
                            _value = "No";
                        break;
                    case "J":
                        object o = SqlHelper.ExecuteScalar(dsn, CommandType.Text, "SELECT " + _join_field + " FROM " + _join_table + " WHERE " + _join_on + " = " + _value);
                        if (o == null)
                            _value = "---";
                        else
                            _value = o.ToString();
                        break;
                }
            }
            return _value;
        }
    }
}
