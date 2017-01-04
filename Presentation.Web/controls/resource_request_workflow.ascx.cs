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

namespace NCC.ClearView.Presentation.Web
{
    public partial class resource_request_workflow : System.Web.UI.UserControl
    {

        private DataSet ds;
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        private string dsnServiceEditor = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ServiceEditorDSN"]].ConnectionString;
        private string dsnZeus = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ZeusDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intViewPage = Int32.Parse(ConfigurationManager.AppSettings["ViewWorkload"]);
        protected int intViewRequest = Int32.Parse(ConfigurationManager.AppSettings["ViewRequest"]);
        protected int intViewResourceRequest = Int32.Parse(ConfigurationManager.AppSettings["ResourceRequest"]);
        protected int intAssignPage = Int32.Parse(ConfigurationManager.AppSettings["AssignWorkload"]);
        protected int intResourceRequestApprove = Int32.Parse(ConfigurationManager.AppSettings["ResourceRequestApprove"]);
        protected int intProfile;
        protected ProjectRequest oProjectRequest;
        protected ResourceRequest oResourceRequest;
        protected ProjectNumber oProjectNumber;
        protected Pages oPage;
        protected Users oUser;
        protected Requests oRequest;
        protected RequestItems oRequestItem;
        protected Applications oApplication;
        protected Functions oFunction;
        protected Variables oVariable;
        protected Projects oProject;
        protected Platforms oPlatform;
        protected ServiceRequests oServiceRequest;
        protected Services oService;
        protected RequestFields oRequestField;
        protected Delegates oDelegate;
        protected Log oLog;
        protected int intApplication = 0;
        protected int intPage = 0;
        private string strEMailIdsBCC = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oProjectRequest = new ProjectRequest(intProfile, dsn);
            oResourceRequest = new ResourceRequest(intProfile, dsn);
            oProjectNumber = new ProjectNumber(intProfile, dsn);
            oPage = new Pages(intProfile, dsn);
            oUser = new Users(intProfile, dsn);
            oRequest = new Requests(intProfile, dsn);
            oRequestItem = new RequestItems(intProfile, dsn);
            oApplication = new Applications(intProfile, dsn);
            oFunction = new Functions(intProfile, dsn, intEnvironment);
            oVariable = new Variables(intEnvironment);
            oProject = new Projects(intProfile, dsn);
            oPlatform = new Platforms(intProfile, dsn);
            oServiceRequest = new ServiceRequests(intProfile, dsn);
            oService = new Services(intProfile, dsn);
            oRequestField = new RequestFields(intProfile, dsn);
            oDelegate = new Delegates(intProfile, dsn);
            oLog = new Log(intProfile, dsn);

            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);

            if (Request.QueryString["action"] != null && Request.QueryString["action"] != "")
                panFinish.Visible = true;
            else
            {
                if (!IsPostBack)
                {
                    if (Request.QueryString["rrid"] != null && Request.QueryString["rrid"] != "")
                    {
                        // PLATFORM
                        int intResource = Int32.Parse(Request.QueryString["rrid"]);
                        lblRequest.Text = Request.QueryString["rrid"];
                        if (intResource > 0)
                        {
                            DataSet dsResource = oResourceRequest.Get(intResource);
                            if (dsResource.Tables[0].Rows.Count > 0)
                            {
                                int intRequest = Int32.Parse(dsResource.Tables[0].Rows[0]["requestid"].ToString());
                                int intService = Int32.Parse(dsResource.Tables[0].Rows[0]["serviceid"].ToString());
                                int intNumber = Int32.Parse(dsResource.Tables[0].Rows[0]["number"].ToString());
                                if (oRequest.Allowed(intRequest, intService, intNumber, intProfile, true))
                                {
                                    panRequest.Visible = true;
                                    int intProject = oRequest.GetProjectNumber(intRequest);
                                    int intItem = Int32.Parse(dsResource.Tables[0].Rows[0]["itemid"].ToString());
                                    int intApp = oRequestItem.GetItemApplication(intItem);
                                    lblView.Text = oRequestField.GetBodyOverall(intResource, 0, dsnServiceEditor, intEnvironment, dsnAsset, dsnIP);
                                    //LoadProject(intProject, intRequest);
                                }
                                else
                                    panDenied.Visible = true;
                            }
                            else
                                panDenied.Visible = true;
                        }
                        else
                            panDenied.Visible = true;
                    }
                    else if (Request.QueryString["srid"] != null && Request.QueryString["srid"] != "")
                    {
                        // SERVICE
                        int intServiceSelectedID = Int32.Parse(Request.QueryString["srid"]);
                        if (intServiceSelectedID > 0)
                        {
                            DataSet dsServiceSelected = oService.GetSelectedById(intServiceSelectedID);
                            if (dsServiceSelected.Tables[0].Rows.Count > 0)
                            {
                                DataRow drServiceSelected = dsServiceSelected.Tables[0].Rows[0];
                                int intRequest = Int32.Parse(drServiceSelected["requestid"].ToString());
                                int intService = Int32.Parse(drServiceSelected["serviceid"].ToString());
                                int intItem = oService.GetItemId(intService);
                                int intNumber = Int32.Parse(drServiceSelected["number"].ToString());
                                if (drServiceSelected["approvedon"].ToString() == "" || drServiceSelected["approved"].ToString() == "0")
                                {
                                    int intApp = oRequestItem.GetItemApplication(intItem);
                                    lblRequest.Text = intRequest.ToString();
                                    if (oRequest.Allowed(intRequest, intService, intNumber, intProfile, true))
                                    {
                                        panRequest.Visible = true;
                                        int intProject = oRequest.GetProjectNumber(intRequest);
                                        lblView.Text = oRequestField.GetBodyNoEnv(intRequest, intItem, intNumber, intService, 0, 0, dsnServiceEditor, intEnvironment, dsnAsset, dsnIP);
                                        //LoadProject(intProject, intRequest);
                                    }
                                    else
                                        panDenied.Visible = true;
                                }
                                else
                                {
                                    // Already approved
                                    litAlreadyStatus.Text = (drServiceSelected["approved"].ToString() == "1" ? "Approved" : "Denied");
                                    litAlreadyBy.Text = oUser.GetFullNameWithLanID(Int32.Parse(drServiceSelected["approvedby"].ToString()));
                                    litAlreadyOn.Text = " on " + drServiceSelected["approvedon"].ToString();
                                    panAlready.Visible = true;
                                }
                            }
                            else
                                panDenied.Visible = true;
                        }
                        else
                            panDenied.Visible = true;
                    }
                    else if (Request.QueryString["rid"] != null && Request.QueryString["rid"] != "")
                    {
                        // MANAGER
                        int intRequest = Int32.Parse(Request.QueryString["rid"]);
                        lblRequest.Text = Request.QueryString["rid"];
                        int intProject = Int32.Parse(oRequest.Get(intRequest, "projectid"));
                        //ds = oResourceRequest.GetRequestAll(intRequest);
                        int intRequester = Int32.Parse(oRequest.Get(intRequest, "userid"));
                        if (intRequest > 0)
                        {
                            bool boolAllowed = oUser.IsManager(intRequester, intProfile, true);
                            if (boolAllowed == false)
                            {
                                int intManager = oUser.GetManager(intRequester, true);
                                boolAllowed = (oDelegate.Get(intManager, intProfile) > 0);
                            }
                            if (oUser.IsAdmin(intProfile) || boolAllowed)
                            {
                                trHR.Visible = false;
                                panRequest.Visible = true;
                                lblView.Text = "As the manager of <b>" + oUser.GetFullName(intRequester) + "</b>, you will need to approve this request before it is submitted. You can view the details of the request by clicking [<a href=\"javascript:void(0);\">View</a>] next to each item.";
                                Control oControl = (Control)LoadControl("/controls/resource_request_new.ascx");
                                PHForm.Controls.Add(oControl);
                                if (String.IsNullOrEmpty(Request.QueryString["approve"]))
                                {
                                    btnApprove.Visible = btnDeny.Visible = false;
                                    lblView.Text = "Here are the details of the service request...";
                                }

                                if (oServiceRequest.Get(intRequest, "manager_approval") != "0")
                                {
                                    // Already approved
                                    btnApprove.Enabled = btnDeny.Enabled = false;
                                    litAlreadyStatus.Text = (oServiceRequest.Get(intRequest, "manager_approval") == "1" ? "Approved" : "Denied");
                                    litAlreadyBy.Text = oUser.GetFullNameWithLanID(oUser.GetManager(intRequester, true));
                                    panAlready.Visible = true;
                                }
                            }
                            else
                                panDenied.Visible = true;
                        }
                        else
                            panDenied.Visible = true;
                    }
                    else
                        panDenied.Visible = true;
                }
            }
            btnClose.Attributes.Add("onclick", "return CloseWindow();");
            btnFinish.Attributes.Add("onclick", "return ProcessButton(this) && LoadWait();");
            btnAlready.Attributes.Add("onclick", "return ProcessButton(this) && LoadWait();");
            btnApprove.Attributes.Add("onclick", "return confirm('Are you sure you want to APPROVE this request?') && ProcessButton(this) && LoadWait();");
            btnDeny.Attributes.Add("onclick", "return Deny('" + divDeny.ClientID + "','" + divApprove.ClientID + "','" + txtReason.ClientID + "');");
            btnDone.Attributes.Add("onclick", "return ValidateText('" + txtReason.ClientID + "', 'Please enter a reason') && confirm('Are you sure you want to DENY this request?') && ProcessButton(this) && LoadWait();");
            btnCancel.Attributes.Add("onclick", "return Cancel('" + divDeny.ClientID + "','" + divApprove.ClientID + "');");
        }
        protected void btnApprove_Click(Object Sender, EventArgs e)
        {
            if (Request.QueryString["rrid"] != null)
            {
                // Approved; Send to team lead for assignment
                int intResource = Int32.Parse(Request.QueryString["rrid"]);
                int intRequest = Int32.Parse(oResourceRequest.Get(intResource, "requestid"));
                int intService = Int32.Parse(oResourceRequest.Get(intResource, "serviceid"));
                int intNumber = Int32.Parse(oResourceRequest.Get(intResource, "number"));
                string strCVT = "CVT" + intRequest.ToString() + "-" + intService.ToString() + "-" + intNumber.ToString();

                int ApprovalID = 0;
                DataSet dsApproval = oResourceRequest.GetApprovals(intRequest, intService, intNumber);
                foreach (DataRow drApproval in dsApproval.Tables[0].Rows)
                {
                    if (Int32.Parse(drApproval["userid"].ToString()) == intProfile)
                    {
                        if (String.IsNullOrEmpty(drApproval["approved"].ToString()) == false
                            || String.IsNullOrEmpty(drApproval["denied"].ToString()) == false)
                        {
                            ApprovalID = Int32.Parse(drApproval["id"].ToString());
                            break;
                        }
                    }
                }

                if (ApprovalID == 0)
                {
                    oLog.AddEvent(intRequest.ToString(), strCVT, "Not already approved for user " + oUser.GetName(intProfile), LoggingType.Debug);
                    oResourceRequest.UpdateStatusOverall(intResource, 2);
                    if (oResourceRequest.Get(intResource, "platform_approval") == "0")
                    {
                        // Platform Approve
                        oResourceRequest.ApprovePlatform(intResource, 1);
                        SendEmail(intRequest, true, "PLATFORM", txtReason.Text);
                        oLog.AddEvent(intRequest.ToString(), strCVT, "User " + oUser.GetName(intProfile) + " approved PLATFORM", LoggingType.Debug);
                    }
                    else if (oService.GetSelected(intRequest, intService, intNumber, "approved") == "0")
                    {
                        // Service Manager Approve
                        oService.UpdateSelectedApprove(intRequest, intService, intNumber, 1, intProfile, DateTime.Now, "");
                        SendEmail(intRequest, true, "SERVICE MANAGER", txtReason.Text);
                        oLog.AddEvent(intRequest.ToString(), strCVT, "User " + oUser.GetName(intProfile) + " approved SERVICE MANAGER", LoggingType.Debug);
                    }
                    else
                    {
                        oResourceRequest.UpdateApproval(intRequest, intService, intNumber, intProfile, txtReason.Text, DateTime.Now.ToString(), "");
                        SendEmail(intRequest, true, "3RD PARTY APPROVER", txtReason.Text);
                        oLog.AddEvent(intRequest.ToString(), strCVT, "User " + oUser.GetName(intProfile) + " approved 3RD PARTY APPROVER", LoggingType.Debug);
                    }
                    int intItem = Int32.Parse(oResourceRequest.Get(intResource, "itemid"));
                    if (oServiceRequest.NotifyApproval(intResource, intResourceRequestApprove, intEnvironment, "", dsnServiceEditor) == false)
                        oServiceRequest.NotifyTeamLead(intItem, intResource, intAssignPage, intViewPage, intEnvironment, "", dsnServiceEditor, dsnAsset, dsnIP, 0);
                }
                else
                    oLog.AddEvent(intRequest.ToString(), strCVT, "Already approved for user " + oUser.GetName(intProfile), LoggingType.Debug);
            }
            if (Request.QueryString["srid"] != null)
            {
                // Approved; Permit submission
                int intServiceSelectedID = Int32.Parse(Request.QueryString["srid"]);
                DataSet dsServiceSelected = oService.GetSelectedById(intServiceSelectedID);
                DataRow drServiceSelected = dsServiceSelected.Tables[0].Rows[0];
                int intRequest = Int32.Parse(drServiceSelected["requestid"].ToString());
                int intService = Int32.Parse(drServiceSelected["serviceid"].ToString());
                int intItem = oService.GetItemId(intService);
                int intNumber = Int32.Parse(drServiceSelected["number"].ToString());
                string strCVT = "CVT" + intRequest.ToString() + "-" + intService.ToString() + "-" + intNumber.ToString();
                oService.UpdateSelectedApprove(intRequest, intService, intNumber, 1, intProfile, DateTime.Now, "");
                SendEmail(intRequest, true, "SERVICE MANAGER", txtReason.Text);
                oLog.AddEvent(intRequest.ToString(), strCVT, "User " + oUser.GetName(intProfile) + " approved SERVICE MANAGER", LoggingType.Debug);

                // Kick off any automated processing...
                Workstations oWorkstation = new Workstations(intProfile, dsn);
                oWorkstation.ExecuteVMware(intRequest);
            }
            if (Request.QueryString["rid"] != null)
            {
                // Manager Approve
                int intRequest = Int32.Parse(Request.QueryString["rid"]);
                oServiceRequest.UpdateApproval(intRequest, 1);
                oResourceRequest.UpdateStatusRequest(intRequest, 2);
                SendEmail(intRequest, true, "MANAGER", txtReason.Text);
                DataSet dsForm = oRequestItem.GetForms(intRequest);
                foreach (DataRow drForm in dsForm.Tables[0].Rows)
                {
                    int intService = 0;
                    Int32.TryParse(drForm["serviceid"].ToString(), out intService);
                    int intNumber = 0;
                    Int32.TryParse(drForm["number"].ToString(), out intNumber);
                    if (intService > 0 && intNumber > 0)
                    {
                        if (oServiceRequest.NotifyApproval(intRequest, intService, intNumber, intResourceRequestApprove, intEnvironment, "", dsnServiceEditor) == false)
                        {
                            // Send to Team Lead for assignment
                            ds = oResourceRequest.GetUnAssigned(intRequest, 0);
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                int intItem = Int32.Parse(dr["itemid"].ToString());
                                oServiceRequest.NotifyTeamLead(intItem, Int32.Parse(dr["id"].ToString()), intAssignPage, intViewPage, intEnvironment, "", dsnServiceEditor, dsnAsset, dsnIP, 0);
                            }
                        }
                    }
                }
            }
            Response.Redirect(oPage.GetFullLink(intPage) + "?action=finish");
        }
        protected void btnDone_Click(Object Sender, EventArgs e)
        {
            if (Request.QueryString["rrid"] != null)
            {
                int intResource = Int32.Parse(Request.QueryString["rrid"]);
                int intRequest = Int32.Parse(oResourceRequest.Get(intResource, "requestid"));
                int intService = Int32.Parse(oResourceRequest.Get(intResource, "serviceid"));
                int intNumber = Int32.Parse(oResourceRequest.Get(intResource, "number"));
                string strCVT = "CVT" + intRequest.ToString() + "-" + intService.ToString() + "-" + intNumber.ToString();

                bool boolAlready = false;
                DataSet dsApproval = oResourceRequest.GetApprovals(intRequest, intService, intNumber);
                foreach (DataRow drApproval in dsApproval.Tables[0].Rows)
                {
                    if (Int32.Parse(drApproval["userid"].ToString()) == intProfile)
                    {
                        if (String.IsNullOrEmpty(drApproval["approved"].ToString()) == false
                            || String.IsNullOrEmpty(drApproval["denied"].ToString()) == false)
                        {
                            boolAlready = true;
                            break;
                        }
                    }
                }

                if (boolAlready == false)
                {
                    oLog.AddEvent(intRequest.ToString(), strCVT, "Not already denied for user " + oUser.GetName(intProfile), LoggingType.Debug);
                    oResourceRequest.UpdateStatusOverall(intResource, -1);
                    if (oResourceRequest.Get(intResource, "platform_approval") == "0")
                    {
                        // Platform Deny
                        oResourceRequest.ApprovePlatform(intResource, -1);
                        SendEmail(intRequest, false, "PLATFORM", txtReason.Text);
                        oLog.AddEvent(intRequest.ToString(), strCVT, "User " + oUser.GetName(intProfile) + " denied PLATFORM", LoggingType.Debug);
                    }
                    else if (oService.GetSelected(intRequest, intService, intNumber, "approved") == "0")
                    {
                        // Service Manager Deny
                        oService.UpdateSelectedApprove(intRequest, intService, intNumber, -1, intProfile, DateTime.Now, txtReason.Text);
                        SendEmail(intRequest, false, "SERVICE MANAGER", txtReason.Text);
                        oLog.AddEvent(intRequest.ToString(), strCVT, "User " + oUser.GetName(intProfile) + " denied SERVICE MANAGER", LoggingType.Debug);
                    }
                    else
                    {
                        oResourceRequest.UpdateApproval(intRequest, intService, intNumber, intProfile, txtReason.Text, "", DateTime.Now.ToString());
                        SendEmail(intRequest, false, "3RD PARTY APPROVER", txtReason.Text);
                        oLog.AddEvent(intRequest.ToString(), strCVT, "User " + oUser.GetName(intProfile) + " denied 3RD PARTY APPROVER", LoggingType.Debug);
                    }
                }
                else
                    oLog.AddEvent(intRequest.ToString(), strCVT, "Already denied for user " + oUser.GetName(intProfile), LoggingType.Debug);
            }
            if (Request.QueryString["srid"] != null)
            {
                // Approved; Send to team lead for assignment
                int intServiceSelectedID = Int32.Parse(Request.QueryString["srid"]);
                DataSet dsServiceSelected = oService.GetSelectedById(intServiceSelectedID);
                DataRow drServiceSelected = dsServiceSelected.Tables[0].Rows[0];
                int intRequest = Int32.Parse(drServiceSelected["requestid"].ToString());
                int intService = Int32.Parse(drServiceSelected["serviceid"].ToString());
                int intItem = oService.GetItemId(intService);
                int intNumber = Int32.Parse(drServiceSelected["number"].ToString());
                string strCVT = "CVT" + intRequest.ToString() + "-" + intService.ToString() + "-" + intNumber.ToString();
                oService.UpdateSelectedApprove(intRequest, intService, intNumber, -1, intProfile, DateTime.Now, txtReason.Text);
                SendEmail(intRequest, false, "SERVICE MANAGER", txtReason.Text);
                oLog.AddEvent(intRequest.ToString(), strCVT, "User " + oUser.GetName(intProfile) + " denied SERVICE MANAGER", LoggingType.Debug);
            }
            if (Request.QueryString["rid"] != null)
            {
                // Manager Deny
                int intRequest = Int32.Parse(Request.QueryString["rid"]);
                oServiceRequest.UpdateApproval(intRequest, -1);
                oService.UpdateSelectedApprove(intRequest, 0, 0, -1, intProfile, DateTime.Now, txtReason.Text);
                oResourceRequest.UpdateStatusRequest(intRequest, -1);
                SendEmail(intRequest, false, "MANAGER", txtReason.Text);
            }
            Response.Redirect(oPage.GetFullLink(intPage) + "?action=finish");
        }
        protected void SendEmail(int _requestid, bool _approved, string _level, string _comments)
        {
            int intUser = Int32.Parse(oRequest.Get(_requestid, "userid"));
            string strDefault = oUser.GetApplicationUrl(intUser, intViewResourceRequest);
            strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_REQUEST_STATUS");
            if (strDefault == "")
                oFunction.SendEmail("Request #CVT" + _requestid.ToString() + " " + (_approved ? "APPROVED" : "DENIED"), oUser.GetName(intUser), "", strEMailIdsBCC, "Request #CVT" + _requestid.ToString(), "<p><b>Your service request has been " + (_approved ? "APPROVED" : "DENIED") + " by the " + _level + " (" + oUser.GetFullName(intProfile) + ").</b></p>" + (_comments == "" ? "" : "<p>The following comments were added...<br/>" + oFunction.FormatText(_comments) + "</p>"), true, false);
            else
                oFunction.SendEmail("Request #CVT" + _requestid.ToString() + " " + (_approved ? "APPROVED" : "DENIED"), oUser.GetName(intUser), "", strEMailIdsBCC, "Request #CVT" + _requestid.ToString(), "<p><b>Your service request has been " + (_approved ? "APPROVED" : "DENIED") + " by the " + _level + " (" + oUser.GetFullName(intProfile) + ").</b></p>" + (_comments == "" ? "" : "<p>The following comments were added...<br/>" + oFunction.FormatText(_comments) + "</p>") + "<p><a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=/" + strDefault + oPage.GetFullLink(intViewResourceRequest) + "?rid=" + _requestid.ToString() + "\" target=\"_blank\">Click here to view this service request.</a></p>", true, false);
        }
        protected void btnFinish_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(oPage.GetFullLinkRelated(intPage));
        }
    }
}