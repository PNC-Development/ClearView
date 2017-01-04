using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using NCC.ClearView.Application.Core;
using System.Text;

namespace NCC.ClearView.Presentation.Web
{
    public partial class wucWorkflow : System.Web.UI.UserControl
    {
        private class wucWorkflowItem
        {
            public StringBuilder Html;
            public int Children;
        }

        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnServiceEditor = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ServiceEditorDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        private int _RequestID;
        public int RequestID
        {
            set { this._RequestID = value; }
            get { return this._RequestID; }
        }
        private int _ServiceID;
        public int ServiceID
        {
            set { this._ServiceID = value; }
            get { return this._ServiceID; }
        }
        private int _Number;
        public int Number
        {
            set { this._Number = value; }
            get { return this._Number; }
        }
        private int intCount = 0;

        private ResourceRequest oResourceRequest;
        private Services oService;
        private Users oUser;
        private Functions oFunction;
        private ServiceEditor oServiceEditor;
        private int intProfile = 0;
        private int intApplication = 0;
        private string strCompletedLast = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                if (Request.Cookies["profileid"] != null && Request.Cookies["profileid"].Value != "")
                    intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
                if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                    intApplication = Int32.Parse(Request.QueryString["applicationid"]);
                if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                    intApplication = Int32.Parse(Request.Cookies["application"].Value);
                oResourceRequest = new ResourceRequest(intProfile, dsn);
                oService = new Services(intProfile, dsn);
                oUser = new Users(intProfile, dsn);
                oFunction = new Functions(intProfile, dsn, intEnvironment);
                oServiceEditor = new ServiceEditor(intProfile, dsnServiceEditor);

                wucWorkflowItem _workflow = LoadWorkflow(RequestID, ServiceID, Number, false);
                StringBuilder strWorkflow = new StringBuilder(_workflow.Html.ToString());
                if (intCount > 0)
                {
                    strWorkflow.Append("<tr>");
                    strWorkflow.Append("<td align=\"center\">");
                    strWorkflow.Append("<img src=\"/images/down_arrow_black.gif\" border=\"0\" align=\"absmiddle\" />");
                    strWorkflow.Append("</td>");
                    strWorkflow.Append("</tr>");
                }
                intCount++;
                strWorkflow.Append("<tr>");
                strWorkflow.Append("<td style=\"border:solid 1px #CCC\" align=\"center\"" + (intCount % 2 == 0 ? "" : " style=\"background-color:#E6E6E6\"") + ">");
                strWorkflow.Append("<img src=\"/images/bigCheckBox.gif\" align=\"absmiddle\" border=\"0\">&nbsp;<span class=\"header\">End of Request</span>");
                strWorkflow.Append("</td>");
                strWorkflow.Append("</tr>");

                litWorkflow.Text = strWorkflow.ToString();
                lblWorkflow.Visible = (strWorkflow.ToString() == "");
            }
        }

        private wucWorkflowItem LoadWorkflow(int _requestid, int _serviceid, int _number, bool _workflow)
        {
            StringBuilder strReturn = new StringBuilder();
            bool boolHasChildren = false;

            // Load Workflow services First!
            DataSet dsReceive = oService.GetWorkflowsReceive(_serviceid);
            foreach (DataRow drReceive in dsReceive.Tables[0].Rows)
            {
                int intWorkflowService = Int32.Parse(drReceive["serviceid"].ToString());
                wucWorkflowItem _child = LoadWorkflow(RequestID, intWorkflowService, Number, true);
                strReturn.Append(_child.Html);
                if (_child.Children > 0)
                    boolHasChildren = true;
            }

            // Now Load this service
            bool boolPending = false;
            //DataSet dsWorkflow = oResourceRequest.GetWorkflow(_requestid, _serviceid, _number);
            DataSet dsWorkflow = oServiceEditor.GetWorkflow(_requestid, _serviceid, _number);
            foreach (DataRow drWorkflow in dsWorkflow.Tables[0].Rows)
            {
                int intStatus = 0;
                Int32.TryParse(drWorkflow["status"].ToString(), out intStatus);
                int intResource = 0;
                Int32.TryParse(drWorkflow["userid"].ToString(), out intResource);
                int intRR = 0;
                Int32.TryParse(drWorkflow["resourceid"].ToString(), out intRR);

                string strStep = drWorkflow["step"].ToString();
                string strComments = drWorkflow["comments"].ToString();
                //if (strComments != "")
                //    strComments = "Additional Comments:<br/>" + strComments;
                string strNotified = drWorkflow["created"].ToString();
                string strWorkflowCompleted = strCompletedLast;
                string strCompleted = strCompletedLast = drWorkflow["completed"].ToString();
                string strLabel = "";
                string strFinish = "Completed";

                string strHeader = "";
                string strAdditional = "";
                string strStatus = "";
                string strPicture = "";
                string strResource = "";
                string strIcon = "";
                bool boolActive = false;
                bool boolReturned = false;
                bool boolHide = false;
                bool boolComment = true;   // only show comment gif for actual comments.

                switch (strStep)
                {
                    case "REQUESTOR":
                        strIcon = "/images/check_small.gif";
                        strNotified = "";
                        string strWorkflowTitle = oService.Get(_serviceid, "workflow_title");
                        if (dsReceive.Tables[0].Rows.Count > 0)
                        {
                            strHeader = "Workflow Generated";
                            intResource = -999;
                            strComments = "<b>" + (strWorkflowTitle == "" ? oService.GetName(_serviceid) : strWorkflowTitle) + "</b> generated as part of a workflow";
                            boolComment = false;
                            strCompleted = strWorkflowCompleted;
                            strStatus = "Initiated";
                            strLabel = "Initiated&nbsp;By";
                        }
                        else
                        {
                            strHeader = "Request Initiated";
                            strComments = "<b>" + (strWorkflowTitle == "" ? oService.GetName(_serviceid) : strWorkflowTitle) + "</b> submitted";
                            boolComment = false;
                            strStatus = "Submitted";
                            strLabel = "Submitted&nbsp;By";
                        }
                        strFinish = "Initiated&nbsp;On";
                        break;
                    case "MANAGER_APPROVAL":
                    case "SERVICE_APPROVAL":
                    case "PLATFORM_APPROVAL":
                    case "3RD_PARTY_APPROVAL":
                        if (strStep == "MANAGER_APPROVAL")
                            strHeader = "Requestor's Manager Approval";
                        else if (strStep == "SERVICE_APPROVAL")
                            strHeader = "Service Manager Approval";
                        else if (strStep == "PLATFORM_APPROVAL")
                            strHeader = "Platform Manager Approval";
                        else if (strStep == "3RD_PARTY_APPROVAL")
                            strHeader = "3rd Party Approval";

                        switch (intStatus)
                        {
                            case 1:
                                strIcon = "/images/check_small.gif";
                                strStatus = "Approved";
                                strFinish = "Approved";
                                break;
                            case 0:
                                strIcon = "/images/active.gif";
                                strStatus = "Pending Approval";
                                boolActive = true;
                                boolPending = true;
                                break;
                            case -1:
                                strIcon = "/images/cancel.gif";
                                strStatus = "Denied";
                                strFinish = "Denied";
                                break;
                        }
                        strLabel = "Approver";
                        break;
                    case "RETURN":
                    case "TECHNICIAN":
                        strHeader = oService.GetName(_serviceid);
                        strStatus = "In Progress";
                        if (strStep == "RETURN")
                        {
                            //strHeader = "Request Returned";
                            strLabel = "Resource";
                            strIcon = "/images/returned.gif";
                            strStatus = "Returned";
                            if (intStatus == 0)
                                boolReturned = true;
                        }
                        else if (strStep == "TECHNICIAN")
                        {
                            //strHeader = "Resource Assigned";
                            strLabel = "Resource";
                            if (intStatus == (int)ResourceRequestStatus.Cancelled)
                            {
                                strIcon = "/images/cancel.gif";
                                strStatus = "Cancelled by Requestor";
                                strFinish = "Cancelled";
                                strCompleted = drWorkflow["modified"].ToString();
                            }
                            else if (intStatus == (int)ResourceRequestStatus.OnHold)
                            {
                                strIcon = "/images/pending.gif";
                                strStatus = "On Hold";
                            }
                            else if (intStatus == (int)ResourceRequestStatus.AwaitingResponse)
                            {
                                strIcon = "/images/delegate2.gif";
                                strStatus = "Awaiting Response";
                            }
                            else if (strCompleted != "")
                            {
                                strIcon = "/images/check_small.gif";
                                strStatus = "Completed";
                            }

                            //if (strIcon != "/images/ico_check.gif" && strComments.ToUpper() == "COMPLETED")
                            //    strComments = "";
                        }

                        if (intRR > 0)
                        {
                            bool boolThis = false;
                            DataSet dsRR = oResourceRequest.Get(intRR);
                            if (dsRR.Tables[0].Rows.Count > 0)
                            {
                                DataRow drRR = dsRR.Tables[0].Rows[0];
                                int intRequest = 0;
                                int intService = 0;
                                int intNumber = 0;
                                if (Int32.TryParse(drRR["requestid"].ToString(), out intRequest) && Int32.TryParse(drRR["serviceid"].ToString(), out intService) && Int32.TryParse(drRR["number"].ToString(), out intNumber))
                                {
                                    if (intRequest == RequestID && intService == ServiceID && intNumber == Number)
                                        boolThis = true;
                                }
                            }
                            if (boolThis == false)
                                strAdditional = "<p><a href=\"javascript:void(0);\" class=\"lookup\" onclick=\"OpenNewWindowMenu('/datapoint/service/resource.aspx?id=" + oFunction.encryptQueryString(drWorkflow["resourceid"].ToString()) + "', '800', '600');\">View Request Detail</a></p>";
                        }
                        break;
                    case "MANAGER_ASSIGNMENT":
                        strHeader = "Service Manager Assignment";
                        switch (intStatus)
                        {
                            case 1:
                                if (strCompleted == "")
                                {
                                    strIcon = "/images/active.gif";
                                    strStatus = "Awaiting Assignment";
                                    boolActive = true;
                                }
                                else
                                {
                                    strIcon = "/images/check_small.gif";
                                    strStatus = "Assigned to a Resource";
                                    strFinish = "Assigned";
                                }
                                break;
                            case 0:
                                if (boolPending == true)
                                {
                                    // Still pending one or more approvals
                                    strIcon = "/images/pending.gif";
                                    strStatus = "Awaiting Approval";
                                }
                                else
                                {
                                    strIcon = "/images/active.gif";
                                    strStatus = "Awaiting Assignment";
                                    boolActive = true;
                                }
                                break;
                            case -1:
                                strIcon = "/images/cancel.gif";
                                strStatus = "Rejected by service manager";
                                strFinish = "Rejected";
                                break;
                        }
                        strLabel = "Manager(s)";
                        break;
                }

                if (intResource == -999)
                {
                    strResource = "<a href=\"javascript:void(0);\" class=\"lookup\" onclick=\"alert('ClearView is a system account which is used for automated tasks.');\">ClearView</a>";
                    strPicture = "/images/clearview.gif";
                }
                else
                {
                    if (strStep == "MANAGER_ASSIGNMENT" && intResource == 0)
                    {
                        // Pending assignment, intRR = SERVICEID
                        DataSet dsManagers = oService.GetUser(intRR, 1); // get managers
                        strResource = "";
                        foreach (DataRow drManager in dsManagers.Tables[0].Rows)
                        {
                            int intManager = Int32.Parse(drManager["userid"].ToString());
                            strResource += "<a href=\"javascript:void(0);\" class=\"lookup\" onclick=\"OpenWindow('PROFILE','?userid=" + intManager.ToString() + "');\">" + oUser.GetFullName(intManager) + " (" + oUser.GetName(intManager) + ")</a><br/>";
                        }
                    }
                    else if (intResource > 0)
                        strResource = "<a href=\"javascript:void(0);\" class=\"lookup\" onclick=\"OpenWindow('PROFILE','?userid=" + intResource.ToString() + "');\">" + oUser.GetFullName(intResource) + " (" + oUser.GetName(intResource) + ")</a>";
                    else
                        strResource = "---";

                    if (intResource > 0)
                        strPicture = "/frame/picture.aspx?xid=" + oUser.GetName(intResource);
                    else if (intResource == -1)
                        strPicture = "/images/nobody.gif";
                    else
                        strPicture = "/images/nophoto.gif";
                }


                if (boolHide == false)
                {
                    if (intCount > 0)
                    {
                        strReturn.Append("<tr>");
                        strReturn.Append("<td align=\"center\">");
                        strReturn.Append("<img src=\"/images/down_arrow_black.gif\" border=\"0\" align=\"absmiddle\" />");
                        //strReturn.Append("<img src=\"/images/arrow_down.png\" border=\"0\" align=\"absmiddle\" />");
                        strReturn.Append("</td>");
                        strReturn.Append("</tr>");
                    }

                    intCount++;


                    strReturn.Append("<tr>");
                    //strReturn.Append("<td valign=\"top\"></td>");
                    strReturn.Append("<td style=\"border:solid 1px #CCC\">");
                    
                        strReturn.Append("<table width=\"500\" cellpadding=\"0\" cellspacing=\"5\" border=\"0\"" + (boolReturned ? " style=\"background-color:#FCDAD4\"" : (boolActive ? " style=\"background-color:#FFFF99\"" : (intCount % 2 == 0 ? "" : " style=\"background-color:#E6E6E6\">"))));
                        strReturn.Append("<tr>");
                        strReturn.Append("<td valign=\"top\" align=\"center\">");
                        strReturn.Append("<img src=\"");
                        strReturn.Append(strPicture);
                        strReturn.Append("\" align=\"absmiddle\" border=\"0\" style='height:90px;width:90px;border-width:0px;border:solid 1px #666666;' />");
                        strReturn.Append("</td>");
                        strReturn.Append("<td width=\"100%\" valign=\"top\">");
                            // Right side table
                            strReturn.Append("<table cellpadding=\"3\" cellspacing=\"5\" border=\"0\">");
                            // Header
                            strReturn.Append("<tr>");
                            strReturn.Append("<td colspan=\"2\" valign=\"top\" class=\"header\">" + strHeader + "</td>");
                            strReturn.Append("</tr>");
                            // Status
                            if (strAdditional != "")
                            {
                                strReturn.Append("<tr>");
                                strReturn.Append("<td colspan=\"2\" valign=\"top\">" + strAdditional + "</td>");
                                strReturn.Append("</tr>");
                            }
                            strReturn.Append("</table>");


                            strReturn.Append("<table cellpadding=\"0\" cellspacing=\"5\" border=\"0\">");
                            // Status
                            strReturn.Append("<tr>");
                            strReturn.Append("<td nowrap><b>Status</b>:</td>");
                            strReturn.Append("<td width=\"100%\">");
                            strReturn.Append((strIcon == "" ? "" : "<img src=\"" + strIcon + "\" align=\"absmiddle\" border=\"0\" />&nbsp;") + strStatus);
                            strReturn.Append("</td>");
                            strReturn.Append("</tr>");
                            // Submitted
                            strReturn.Append("<tr>");
                            strReturn.Append("<td nowrap valign=\"top\"><b>" + strLabel + "</b>:</td>");
                            strReturn.Append("<td width=\"100%\">");
                            strReturn.Append(strResource);
                            strReturn.Append("</td>");
                            strReturn.Append("</tr>");
                            // Notified
                            if (strNotified != "")
                            {
                                strReturn.Append("<tr>");
                                strReturn.Append("<td nowrap><b>Notified</b>:</td>");
                                strReturn.Append("<td width=\"100%\">");
                                strReturn.Append(strNotified);
                                strReturn.Append("</td>");
                                strReturn.Append("</tr>");
                            }
                            // Completed
                            strReturn.Append("<tr>");
                            strReturn.Append("<td nowrap><b>" + strFinish + "</b>:</td>");
                            strReturn.Append("<td width=\"100%\">");
                            strReturn.Append(strCompleted);
                            strReturn.Append("</td>");
                            strReturn.Append("</tr>");
                            strReturn.Append("</table>");

                        strReturn.Append("</td>");
                        strReturn.Append("</tr>");

                        if (strComments != "")    // added a comment
                        {
                            strReturn.Append("<tr>");
                            strReturn.Append("<td colspan=\"2\">");
                                strReturn.Append("<table width=\"100%\" cellpadding=\"3\" cellspacing=\"2\" border=\"0\">");
                                strReturn.Append("<tr>");
                                if (boolComment)
                                    strReturn.Append("<td valign=\"top\"><img src=\"/images/comment.gif\" border=\"0\" /></td>");
                                else
                                    strReturn.Append("<td valign=\"top\"><img src=\"/images/biggerPlus.gif\" border=\"0\" /></td>");
                                strReturn.Append("<td valign=\"top\" colspan=\"2\" width=\"100%\">" + strComments + "</td>");
                                strReturn.Append("</tr>");
                                strReturn.Append("</table>");
                            strReturn.Append("</td>");
                            strReturn.Append("</tr>");
                        }
                        strReturn.Append("</table>");
                    
                    strReturn.Append("</td>");
                    strReturn.Append("</tr>");
                }
            }

            wucWorkflowItem _return = new wucWorkflowItem();
            _return.Html = strReturn;
            _return.Children = dsWorkflow.Tables[0].Rows.Count;
            
            return _return;
        }
    }
}
