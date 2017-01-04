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
using System.Collections.Generic;

namespace NCC.ClearView.Presentation.Web
{
    public partial class resource_request_return : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        private string dsnServiceEditor = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ServiceEditorDSN"]].ConnectionString;
        private int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        private int intViewPage = Int32.Parse(ConfigurationManager.AppSettings["ViewWorkload"]);
        private int intViewRequest = Int32.Parse(ConfigurationManager.AppSettings["ViewRequest"]);
        private int intViewResourceRequest = Int32.Parse(ConfigurationManager.AppSettings["ResourceRequest"]);
        private int intAssignPage = Int32.Parse(ConfigurationManager.AppSettings["AssignWorkload"]);
        private int intSearchPage = Int32.Parse(ConfigurationManager.AppSettings["SearchPage"]);
        private int intImplementorDistributed = Int32.Parse(ConfigurationManager.AppSettings["ITEMID_IMPLEMENTOR_DISTRIBUTED"]);
        private int intImplementorMidrange = Int32.Parse(ConfigurationManager.AppSettings["ITEMID_IMPLEMENTOR_MIDRANGE"]);
        private int intDesignBuilder = Int32.Parse(ConfigurationManager.AppSettings["ForecastEdit"]);
        private int intServiceStorage = Int32.Parse(ConfigurationManager.AppSettings["SERVICEID_STORAGE"]);
        
        protected Requests oRequest;
        protected ResourceRequest oResourceRequest;
        protected RequestItems oRequestItem;
        protected Services oService;
        protected Users oUser;
        protected Functions oFunction;
        protected Applications oApplication;
        protected Projects oProject;
        protected ProjectsPending oProjectsPending;
        protected Variables oVariable;
        protected Pages oPage;
        protected ServiceEditor oServiceEditor;

        protected int intRequest = 0;
        protected int intService = 0;
        protected int intItem = 0;
        protected int intNumber = 0;
        protected int intProject = 0;
        protected int intRequester = 0;
        //protected int intPreRRId = 0;
        //protected int intPreRRItem = 0;
        //protected int intPreNumber = 0;
        //protected int intPreService = 0;
        //protected string strService = "";
        //protected string strComments = "";
        protected string strEmailIdsCC = "";
        protected int intRRId;
        protected int intProfile;
        
        private int intApplication=0;
       
        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);

            oRequest = new Requests(0, dsn);
            oResourceRequest = new ResourceRequest(0, dsn);
            oRequestItem = new RequestItems(intProfile, dsn);
            oService = new Services(0, dsn);
            oUser = new Users(intProfile, dsn);
            oFunction = new Functions(intProfile, dsn, intEnvironment);
            oApplication = new Applications(intProfile, dsn);
            oProject = new Projects(intProfile, dsn);
            oProjectsPending = new ProjectsPending(intProfile, dsn, intEnvironment);
            oVariable = new Variables(intEnvironment);
            oPage = new Pages(intProfile, dsn);
            oServiceEditor = new ServiceEditor(intProfile, dsnServiceEditor);

            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);

            if (Request.QueryString["rrid"] != null && Request.QueryString["rrid"] != "")
                intRRId = Int32.Parse(Request.QueryString["rrid"]);

            getReturningRequestDetails();

            btnSave.Attributes.Add("onclick", "return ValidateText('" + txtComments.ClientID + "','Please enter comment for returning request') && AlertMessage('This action might take a few seconds to complete...\\n\\nPlease be patient and wait for the window to finish processing...') && ProcessButton(this);");
            
            //btnSave.Attributes.Add("onclick", "parent.HidePanel();");
        }


    

        protected void btnSave_Click(Object Sender, EventArgs e)
        {
            int intResourceWorkflow = 0;
            Int32.TryParse(lblStorage.Text, out intResourceWorkflow);
            if (intResourceWorkflow > 0)
            {
                // Auto-provisioning storage task.  Send to ClearView administrators.
                oResourceRequest.AddStatus(intResourceWorkflow, 1, txtComments.Text, intProfile);
                oResourceRequest.UpdateStatusOverallWorkflow(intRRId, (int)ResourceRequestStatus.OnHold);
                string strComments = "";
                string strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_PROVISIONING_SUPPORT");
                if (txtComments.Text.Trim() != "")
                    strComments = "<p>The following comments were added:<br/>" + txtComments.Text + "</p>";
                oFunction.SendEmail("Auto-Provisioning Storage Task Returned", strEMailIdsBCC, "", "", "Auto-Provisioning Storage Task Returned", "<p><b>The following auto-provisioning storage task has been returned by " + oUser.GetFullName(intProfile) + "</b><p><p>" + oResourceRequest.GetWorkflowSummary(intResourceWorkflow, intEnvironment, dsnServiceEditor, dsnAsset, dsnIP) + "</p>" + strComments + "<p>Effective immediately, this request has been put ON HOLD.  You should set the status of this request (in Datapoint) back to ACTIVE once the issue has been resolved.</p><p><a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=/frame/resource_request.aspx?rrid=" + intResourceWorkflow.ToString() + "\" target=\"_blank\">Click here to view this request.</a></p>", true, false);
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "reload", "<script type=\"text/javascript\">alert('This request has been returned');window.parent.close();<" + "/" + "script>");
            }
            else
            {
                updateStatus();
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "reload", "<script type=\"text/javascript\">if (window.opener == null) { RefreshOpeningWindow(true);parent.HidePanel();parent.close(); } else { window.close(); }<" + "/" + "script>");
            }
        }

        private void getReturningRequestDetails()
        {
            
            //Get Current Request Details
            DataSet ds = oResourceRequest.Get(intRRId);
            if (ds.Tables[0].Rows.Count > 0)
            {
                intRequest = Int32.Parse(ds.Tables[0].Rows[0]["requestid"].ToString());
                intService = Int32.Parse(ds.Tables[0].Rows[0]["serviceid"].ToString());
                if (intService == intServiceStorage)
                    lblStorage.Text = Request.QueryString["rrwfid"];
                intItem = Int32.Parse(ds.Tables[0].Rows[0]["itemid"].ToString());
                intNumber = Int32.Parse(ds.Tables[0].Rows[0]["number"].ToString());
                intProject = Int32.Parse(oRequest.Get(intRequest, "projectid"));
                intRequester = Int32.Parse(oRequest.Get(intRequest, "userid"));
                //lblReturnToValue.Text = oUser.GetFullName(intRequester);
            }

            strEmailIdsCC = getProjectEmailIds(intProject, intRequest);

            DataSet dsWorkflow = oServiceEditor.GetWorkflow(intRequest);
            ddlReturnTo.Items.Add(new ListItem("Original Requestor" + " -> " + oUser.GetFullName(oRequest.GetUser(intRequest)), "0"));
            // Get leveling
            int intLevel = 0;
            foreach (DataRow drWorkflow in dsWorkflow.Tables[0].Rows)
            {
                if (Int32.Parse(drWorkflow["serviceid"].ToString()) == intService)
                {
                    intLevel = Int32.Parse(drWorkflow["leveling"].ToString());
                    break;
                }
            }
            foreach (DataRow drWorkflow in dsWorkflow.Tables[0].Rows)
            {
                if (Int32.Parse(drWorkflow["leveling"].ToString()) < intLevel && drWorkflow["ResourceID"].ToString() != "")
                    ddlReturnTo.Items.Insert(0, new ListItem(drWorkflow["ServiceName"].ToString() + " -> " + drWorkflow["username"].ToString(), drWorkflow["ResourceID"].ToString()));
            }

            // //check if this request came from another service request(service workflow)
            //List<WorkflowServices> lstBefore = oService.PreviousService(intRequest, intService, intNumber);
            //for (int ii = 0; ii < lstBefore.Count; ii++)
            //{
            //    if (lstBefore[ii].Completed != "")
            //    {
            //        intPreService = lstBefore[ii].ServiceID;
            //        break;
            //    }
            //}

            //if (intPreService > 0) //Request came from service workflow
            //{
            //    strService = oService.GetName(intPreService);
            //    lblServiceToValue.Text = strService;
            //    DataSet dsPreRR = oResourceRequest.GetRequestService(intRequest, intPreService, intNumber);
            //    if (dsPreRR.Tables[0].Rows.Count > 0)
            //    {

            //        intPreRRId = Int32.Parse(dsPreRR.Tables[0].Rows[0]["parent"].ToString());
            //        intPreRRItem = Int32.Parse(dsPreRR.Tables[0].Rows[0]["itemid"].ToString());
            //        intPreNumber = Int32.Parse(dsPreRR.Tables[0].Rows[0]["number"].ToString());
            //        DataSet dsPreRRWF = oResourceRequest.GetWorkflowsParent(intPreRRId);
            //        lblReturnToValue.Text = "";
            //        if (dsPreRRWF.Tables[0].Rows.Count > 0)
            //        {
            //            foreach (DataRow drWF in dsPreRRWF.Tables[0].Rows)
            //            {
            //                int intRRWFUserId = Int32.Parse(drWF["userid"].ToString());
            //                if (lblReturnToValue.Text!="")
            //                    lblReturnToValue.Text = lblReturnToValue.Text + "<br/>" + oUser.GetFullName(intRRWFUserId);
            //                else
            //                    lblReturnToValue.Text =  oUser.GetFullName(intRRWFUserId);
            //            }
            //        }
            //    }
            //}
            //else
            //    lblServiceToValue.Text = "Original Requestor";

            // Check if it already exists
            //DataSet dsRRReturn = oResourceRequest.getResourceRequestReturn(intRequest, intService, intNumber, 1, 0);
        
        }

        private void updateStatus()
        {

            string strComments = "";
            if (txtComments.Text.Trim() != "")
                strComments = "<p>" + txtComments.Text + "</p>";

            int intReturnedTo = Int32.Parse(ddlReturnTo.SelectedItem.Value);

            if (intReturnedTo > 0) //Request came from service workflow
            {
                DataSet dsRR = oResourceRequest.Get(intReturnedTo);
                if (dsRR.Tables[0].Rows.Count > 0)
                {
                    DataRow drRR = dsRR.Tables[0].Rows[0];
                    int intServiceReturn = Int32.Parse(drRR["serviceid"].ToString());
                    //  Add to Resource Request Return Table
                    oResourceRequest.addResourceRequestReturn(intRequest, intReturnedTo, intServiceReturn, Int32.Parse(drRR["number"].ToString()), 0, 1,
                                                                          intRRId, intService, intNumber, 0,
                                                                          intProfile, txtComments.Text.Trim());

                    //Change status of previous resource request and resource request workflow to active
                    oResourceRequest.UpdateStatusRequest(intReturnedTo, 2);

                    DataSet dsPrevRRWF = oResourceRequest.GetWorkflowsParent(intReturnedTo);
                    foreach (DataRow dr in dsPrevRRWF.Tables[0].Rows)
                    {
                        int intRRWFId = Int32.Parse(dr["id"].ToString());
                        oResourceRequest.UpdateWorkflowStatus(intRRWFId, 2, true);
                    }

                    sendMailNotification(oService.GetName(intServiceReturn), strComments);

                    ////Change status of current resource request and resource request workflow to Awaiting client response
                    //DataSet dsRRWF = oResourceRequest.GetWorkflowsParent(intRRId);
                    //oResourceRequest.UpdateStatusRequest(intRRId, 7);
                    //foreach (DataRow dr in dsRRWF.Tables[0].Rows)
                    //{
                    //    int intRRWFId = Int32.Parse(dr["id"].ToString());
                    //    oResourceRequest.UpdateWorkflowStatus(intRRWFId, 7, true);
                    //}
                    // Change status of all current services (in workflow) to awaiting client response
                    DataSet dsWorkflow = oServiceEditor.GetWorkflow(intRequest);
                    // Get leveling
                    int intLevel = 0;
                    foreach (DataRow drWorkflow in dsWorkflow.Tables[0].Rows)
                    {
                        if (Int32.Parse(drWorkflow["serviceid"].ToString()) == intService)
                        {
                            intLevel = Int32.Parse(drWorkflow["leveling"].ToString());
                            break;
                        }
                    }
                    foreach (DataRow drWorkflow in dsWorkflow.Tables[0].Rows)
                    {
                        if (Int32.Parse(drWorkflow["leveling"].ToString()) == intLevel)
                        {
                            //Change status of current resource request and resource request workflow to Awaiting client response
                            int intResourceRequestID = 0;
                            if (Int32.TryParse(drWorkflow["ResourceID"].ToString(), out intResourceRequestID))
                            {
                                DataSet dsRRWF = oResourceRequest.GetWorkflowsParent(intResourceRequestID);
                                oResourceRequest.UpdateStatusRequest(intResourceRequestID, 7);
                                foreach (DataRow dr in dsRRWF.Tables[0].Rows)
                                {
                                    int intRRWFId = Int32.Parse(dr["id"].ToString());
                                    oResourceRequest.UpdateWorkflowStatus(intRRWFId, 7, true);
                                }
                            }
                        }
                    }
                }
            }
            else //Request not came from service workflow
            {
                //  Add to Resource Request Return Table
                oResourceRequest.addResourceRequestReturn(intRequest, intRRId, intService, intNumber, 0, 0, 0, 0, 0, 0, intProfile, txtComments.Text.Trim());

                DataSet dsRRWF = oResourceRequest.GetWorkflowsParent(intRRId);
                oResourceRequest.UpdateStatusRequest(intRRId, 7);
                foreach (DataRow dr in dsRRWF.Tables[0].Rows)
                {
                    int intRRWFId = Int32.Parse(dr["id"].ToString());
                    oResourceRequest.UpdateWorkflowStatus(intRRWFId, 7, true);
                }


                sendMailNotification(oService.GetName(intService), "");
            }
        }

        private string getProjectEmailIds(int intProject, int intRequest)
        {
            string strEmailIds = "";
            int intPC = 0;
            int intIE = 0;
            if (intProject > 0)
            {
                if (oProject.Get(intProject, "lead") != "")
                    intPC = Int32.Parse(oProject.Get(intProject, "lead"));
                if (oProject.Get(intProject, "engineer") != "")
                    intIE = Int32.Parse(oProject.Get(intProject, "engineer"));
            }
            else
            {
                try
                {
                    intPC = Int32.Parse(oProjectsPending.GetRequest(intRequest, "lead"));
                    intIE = Int32.Parse(oProjectsPending.GetRequest(intRequest, "engineer"));
                }
                catch { }
            }
            
            if (intPC > 0)
                strEmailIds += oUser.GetName(intPC) + ";";
            if (intIE > 0)
                strEmailIds += oUser.GetName(intIE) + ";";

            return strEmailIds;
        }

        private void sendMailNotification(string strService, string strComments)
        {
            string strNotify = "";
            string strDefault = "";
            int intAssigned = 0;
            //int intApp = 0;
            int intPage = 0;
            string strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_REQUEST_ASSIGNMENT");
            string strSpacerRow = "<tr><td colspan=\"3\"><img src=\"" + oVariable.ImageURL() + "/images/spacer.gif\" border=\"0\" width=\"1\" height=\"7\" /></td></tr>";


            if (txtComments.Text.Trim() != "")
                strComments = "<p>" + txtComments.Text + "</p>";

            string strBody = oFunction.EmailComments(oUser.GetFullNameWithLanID(intProfile), strComments);
            
            int intReturnedTo = Int32.Parse(ddlReturnTo.SelectedItem.Value);
            if (intReturnedTo > 0)
            {
                //Get all workflow users
                DataSet dsPrevRRWF = oResourceRequest.GetWorkflowsParent(intReturnedTo);
                //intApp = oRequestItem.GetItemApplication(intPreRRItem);
                foreach (DataRow dr in dsPrevRRWF.Tables[0].Rows)
                {
                    int intResourceWorkflow = Int32.Parse(dr["id"].ToString());
                    string strRequestAssigedTo = "";
                    intAssigned = Int32.Parse(dr["userid"].ToString());
                    strRequestAssigedTo = oUser.GetName(intAssigned);

                    strDefault = oUser.GetApplicationUrl(intAssigned, intViewPage);

                  
                    //if (oProject.Get(intProject, "number").StartsWith("CV") == false)
                    //    strNotify = "<p><span style=\"color:#0000FF\"><b>PROJECT COORDINATOR:</b> Please allocate the hours listed above for each resource in Clarity.</span></p>";
                    if (strDefault == "")
                        oFunction.SendEmail("Service Request Returned: " + strService, strRequestAssigedTo, "", strEMailIdsBCC, "Service Request Returned: " + strService, "<p><b>The following request has been returned to you by " + oUser.GetFullName(intProfile) + "</b></p>" + (strBody == "" ? "" : "<p>" + strBody + "</p>") + "<p>" + oResourceRequest.GetWorkflowSummary(intResourceWorkflow, intEnvironment, dsnServiceEditor, dsnAsset, dsnIP) + "</p>", true, false);
                    else
                    {
                        if (intProject > 0)
                            oFunction.SendEmail("Service Request Returned: " + strService, strRequestAssigedTo, "", strEMailIdsBCC, "Service Request Returned: " + strService, "<p><b>The following request has been returned to you by " + oUser.GetFullName(intProfile) + "</b></p>" + (strBody == "" ? "" : "<p>" + strBody + "</p>") + "<p>" + oResourceRequest.GetWorkflowSummary(intResourceWorkflow, intEnvironment, dsnServiceEditor, dsnAsset, dsnIP) + "</p><p><a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=/" + strDefault + oPage.GetFullLink(intViewPage) + "?pid=" + intProject.ToString() + "\" target=\"_blank\">Click here to review your new assignment.</a></p>", true, false);
                        else
                            oFunction.SendEmail("Service Request Returned: " + strService, strRequestAssigedTo, "", strEMailIdsBCC, "Service Request Returned: " + strService, "<p><b>The following request has been returned to you by " + oUser.GetFullName(intProfile) + "</b></p>" + (strBody == "" ? "" : "<p>" + strBody + "</p>") + "<p>" + oResourceRequest.GetWorkflowSummary(intResourceWorkflow, intEnvironment, dsnServiceEditor, dsnAsset, dsnIP) + "</p><p><a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=/frame/resource_request.aspx?rrid=" + intResourceWorkflow.ToString() + "\" target=\"_blank\">Click here to review your new assignment.</a></p>", true, false);
                    }
                    //string strActivity = "<tr><td><b>Resource:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>" + oUser.GetFullName(intAssigned) + "</td></tr>";
                    //strActivity += strSpacerRow;
                    //strActivity += "<tr><td><b>Service:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>" + strService + "</td></tr>";
                    //strActivity = "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"" + oVariable.DefaultFontStyle() + "\">" + strActivity + "</table>";
                    //string strDeliverable = oApplication.Get(intApp, "deliverables_doc");
                    //if (strDeliverable.Trim() != "")
                    //    strDeliverable = "<p><a href=\"" + oVariable.URL() + strDeliverable + "\">Click here to view the service deliverables</a></p>";
                    //if (oService.Get(intService, "notify_client") != "0")
                    //    oFunction.SendEmail("Request Assignment: " + strService, strRequestAssigedTo, strEmailIdsCC, strEMailIdsBCC, "Request Returned: " + strService, "<p><b>The following request has been returned...</b><p><p>" + oResourceRequest.GetWorkflowSummary(intResourceWorkflow, intEnvironment, dsnServiceEditor, dsnAsset, dsnIP) + "</p><p>" + strActivity + "</p>" + strDeliverable + strNotify, true, false);
                }
            }
            else
            {
                //Send Mail Alerts
                //intApp = oRequestItem.GetItemApplication(intItem);
                strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_REQUEST_ASSIGNMENT,EMAILGRP_REQUEST_STATUS");
                string strPath = oPage.GetFullLink(intViewResourceRequest) + "?rid=" + intRequest.ToString() + "&returned=true";

                if (strPath == "")
                    oFunction.SendEmail("Service Request Returned: " + strService, oUser.GetName(intRequester), strEmailIdsCC, strEMailIdsBCC, "Service Request Returned: " + strService, "<p><b>The following request has been returned by " + oUser.GetFullName(intProfile) + "</b></p>" + (strBody == "" ? "" : "<p>" + strBody + "</p>") + "<p>" + oResourceRequest.GetSummary(intRRId, 0, intEnvironment, dsnServiceEditor, dsnAsset, dsnIP) + "</p>" + strComments, true, false);
                else
                    oFunction.SendEmail("Service Request Returned: " + strService, oUser.GetName(intRequester), "", strEMailIdsBCC, "Service Request Returned: " + strService, "<p><b>The following request has been returned to you by " + oUser.GetFullName(intProfile) + "</b></p>" + (strBody == "" ? "" : "<p>" + strBody + "</p>") + "<p>" + oResourceRequest.GetSummary(intRRId, 0, intEnvironment, dsnServiceEditor, dsnAsset, dsnIP) + "</p>" + strComments + "<p><a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=" + strPath + "\" target=\"_blank\">Click here to review your request.</a></p>", true, false);
            
            }
        }

        //Send Mail Alerts
        //int intApp = oRequestItem.GetItemApplication(intItem);
        //string strDefault = oUser.GetApplicationUrl(intRequester, intAssignPage);
        //string strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_REQUEST_ASSIGNMENT,EMAILGRP_REQUEST_STATUS");
        //if (strDefault == "" || oApplication.Get(intApp, "tpm") != "1")
        //    oFunction.SendEmail("Request Returned: " + strService, oUser.GetName(intRequester), strEmailIdsCC, strEMailIdsBCC, "Request Returned: " + strService, "<p><b>The following request has been returned by " + oUser.GetFullName(intProfile) + "</b><p><p>" + oResourceRequest.GetSummary(intRRId, 0, intEnvironment, dsnServiceEditor, dsnAsset, dsnIP) + "</p>" + strComments, true, false);
        //else
        //    oFunction.SendEmail("Request Returned: " + strService, oUser.GetName(intRequester), strEmailIdsCC, strEMailIdsBCC, "Request Returned: " + strService, "<p><b>The following request has been returned by " + oUser.GetFullName(intProfile) + "</b><p><p>" + oResourceRequest.GetSummary(intRRId, 0, intEnvironment, dsnServiceEditor, dsnAsset, dsnIP) + "</p>" + strComments + "<p><a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=/" + strDefault + oPage.GetFullLink(intAssignPage) + "?rrid=" + intRRId.ToString() + "\" target=\"_blank\">Click here to assign a new project manager to your request.</a></p>", true, false);
    }
}
