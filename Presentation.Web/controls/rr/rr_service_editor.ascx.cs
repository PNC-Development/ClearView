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
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Collections.Generic;
namespace NCC.ClearView.Presentation.Web
{
    public partial class rr_service_editor : System.Web.UI.UserControl
    {

        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        private string dsnServiceEditor = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ServiceEditorDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intViewPage = Int32.Parse(ConfigurationManager.AppSettings["ViewWorkload"]);
        protected int intViewRequest = Int32.Parse(ConfigurationManager.AppSettings["ViewRequest"]);
        protected int intAssignPage = Int32.Parse(ConfigurationManager.AppSettings["AssignWorkload"]);
        protected int intResourceRequestApprove = Int32.Parse(ConfigurationManager.AppSettings["ResourceRequestApprove"]);
        protected Pages oPage;
        protected RequestItems oRequestItem;
        protected ServiceRequests oServiceRequest;
        protected Applications oApplication;
        protected ServiceEditor oServiceEditor;
        protected Customized oCustomized;
        protected Requests oRequest;
        protected Projects oProject;
        protected Services oService;
        protected Functions oFunction;

        protected int intProfile;
        protected int intPage = 0;
        protected int intApplication = 0;
        protected string strForm = "";
        private int intReqReturnedId = 0;
        private bool boolReqReturned = false;
        private bool boolReqDenied = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oPage = new Pages(intProfile, dsn);
            oRequestItem = new RequestItems(intProfile, dsn);
            oServiceRequest = new ServiceRequests(intProfile, dsn);
            oApplication = new Applications(intProfile, dsn);
            oServiceEditor = new ServiceEditor(intProfile, dsnServiceEditor);
            oCustomized = new Customized(intProfile, dsn);
            oRequest = new Requests(intProfile, dsn);
            oProject = new Projects(intProfile, dsn);
            oService = new Services(intProfile, dsn);
            oFunction = new Functions(intProfile, dsn, intEnvironment);
            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);

            if (Request.QueryString["returned"] != null)
                boolReqReturned = true;
            if (Request.QueryString["denied"] != null)
                boolReqDenied = true;

            if (Request.QueryString["rid"] != "" && Request.QueryString["rid"] != null)
            {
                LoadValues();
                if (!IsPostBack)
                    LoadRequest();
                radExpediteYes.Attributes.Add("onclick", "Expedite(this, '" + radExpediteNo.ClientID + "');");
                // Custom Loads
                int intItem = Int32.Parse(lblItem.Text);
                int intApp = oRequestItem.GetItemApplication(intItem);
                string strDeliverable = oApplication.Get(intApp, "deliverables_doc");
                if (strDeliverable != "")
                {
                    btnDeliverable.Visible = true;
                    btnDeliverable.Attributes.Add("onclick", "return OpenWindow('NEW_WINDOW','" + strDeliverable + "');");
                }
                btnDocuments.Attributes.Add("onclick", "return OpenWindow('DOCUMENTS_SECURE','?rid=" + Request.QueryString["rid"] + "');");
            }
            btnBack.Attributes.Add("onclick", "return confirm('WARNING: Any unsaved changes will be lost.\\n\\nAre you sure you want to continue?') && ProcessButton(this) && LoadWait();");
            btnCancel.Attributes.Add("onclick", "return confirm('WARNING: Any unsaved changes will be lost.\\n\\nAre you sure you want to continue?') && ProcessButton(this) && LoadWait();");
            btnCancelR.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this service request?') && ProcessButton(this) && LoadWait();");
        }
        private void LoadValues()
        {
            int intRequest = Int32.Parse(Request.QueryString["rid"]);
            DataSet dsItems;
            int intForm = 0;
            if (Request.QueryString["formid"] != null && Request.QueryString["formid"] != "")
            {
                intForm = Int32.Parse(Request.QueryString["formid"]);
                dsItems = oRequestItem.GetForm(intRequest, intForm);
            }
            else
                dsItems = oRequestItem.GetForms(intRequest);
            if (dsItems.Tables[0].Rows.Count > 0)
            {
                lblReturned.Text = intForm.ToString();
                foreach (DataRow drItem in dsItems.Tables[0].Rows)
                {
                    if (drItem["done"].ToString() == "-1" || intForm > 0)
                    {
                        lblItem.Text = drItem["itemid"].ToString();
                        if (intForm > 0 && Request.QueryString["num"] != null && Request.QueryString["num"] != "")
                            lblNumber.Text = Request.QueryString["num"];
                        else
                            lblNumber.Text = drItem["number"].ToString();
                        lblService.Text = drItem["serviceid"].ToString();
                        break;
                    }
                }
            }
            else if (intForm > 0 && Request.QueryString["num"] != null && Request.QueryString["num"] != "")
            {
                lblNumber.Text = Request.QueryString["num"];
                // Coming from workflow - get back to original request.
                List<WorkflowServices> lstBefore = oService.PreviousService(intRequest, intForm, Int32.Parse(lblNumber.Text));
                int serviceid = 0;
                lblReturned.Text = intForm.ToString();
                foreach (WorkflowServices lstService in lstBefore)
                {
                    if (String.IsNullOrEmpty(lstService.Created) == false)
                        serviceid = lstService.ServiceID;
                }
                lblService.Text = serviceid.ToString();
                lblItem.Text = oService.GetItemId(serviceid).ToString();
            }
        }
        private void LoadRequest()
        {
            int intRequest = Int32.Parse(Request.QueryString["rid"]);
            int intProject = oRequest.GetProjectNumber(intRequest);
            int intPriority = 3;
            int intService = Int32.Parse(lblService.Text);
            int intItem = oService.GetItemId(intService);
           
            string strStatement = "";
            string strExpedite = "";
            if (oService.Get(intService, "statement") == "1")
            {
                panStatement.Visible = true;
                strStatement += " && ValidateText('" + txtStatement.ClientID + "','Please enter a statement of work')";
            }
            else
                panStatement.Visible = false;
            panUpload.Visible = (oService.Get(intService, "upload") == "1");
            //if (oService.Get(intService, "expedite") == "1")
            //{
            //    panExpedite.Visible = true;
            //    strExpedite += " && ValidateRadioButtons('" + radExpediteYes.ClientID + "','" + radExpediteNo.ClientID + "','Please select whether or not you want to expedite this request')";
            //}
            //else
                panExpedite.Visible = false;
            if (Request.QueryString["formid"] != null && Request.QueryString["formid"] != "")
            {
                panUpdate.Visible = true;
                btnCancelR.Visible = boolReqReturned;
                btnCancel.Visible = (boolReqReturned == false);
                DataSet ds = oServiceEditor.GetRequestData(intRequest, intService, Int32.Parse(lblNumber.Text), 0, dsn);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtTitle.Text = ds.Tables[0].Rows[0]["title"].ToString();
                    txtStatement.Text = ds.Tables[0].Rows[0]["statement"].ToString();
                    intPriority = Int32.Parse(ds.Tables[0].Rows[0]["priority"].ToString());
                    radExpediteYes.Checked = (ds.Tables[0].Rows[0]["expedite"].ToString() == "1");
                    radExpediteNo.Checked = (ds.Tables[0].Rows[0]["expedite"].ToString() == "0");
                }
                strForm = oServiceEditor.LoadForm(intService, false, false, "", intEnvironment, ds, dsn);
            }
            else
            {
                panNavigation.Visible = true;
                strForm = oServiceEditor.LoadForm(intService, false, false, "", intEnvironment, null, dsn);
            }
            // Priority and SLA
            int intApp = oRequestItem.GetItemApplication(intItem);
            int intWorkingDays = oApplication.GetLead(intApp, intPriority);
            if (intWorkingDays > 0)
            {
                panDeliverable.Visible = true;
                //oApplication.AssignPriority(intApp, radPriority, lblDeliverable.ClientID, txtEnd.ClientID, hdnEnd.ClientID);
                lblDeliverable.Text = intWorkingDays.ToString();
                //if (boolFound == false)
                //    txtEnd.Text = DateTime.Today.AddDays(intWorkingDays).ToShortDateString();
                hdnEnd.Value = DateTime.Today.AddDays(intWorkingDays).ToShortDateString();
            }
            radPriority.SelectedValue = intPriority.ToString();
            string strRequired = oServiceEditor.GetRequired();
            // Load Title if no project
            if (intProject == -1)
            {
                panTitle.Visible = true;
                if (oService.Get(intService, "title_override") == "1")
                    lblTitleName.Text = oService.Get(intService, "title_name");
                btnNext.Attributes.Add("onclick", "return ValidateText('" + txtTitle.ClientID + "','Please enter a title')" +
                    strStatement +
                    strRequired +
                    strExpedite +
                    " && ProcessButton(this) && LoadWait();");
                btnUpdate.Attributes.Add("onclick", "return ValidateText('" + txtTitle.ClientID + "','Please enter a title')" +
                    strStatement +
                    strRequired +
                    strExpedite +
                    " && ProcessButton(this) && LoadWait();");
            }
            else
            {
                btnNext.Attributes.Add("onclick", "return true" +
                    strStatement +
                    strRequired +
                    strExpedite +
                    " && ProcessButton(this) && LoadWait();");
                btnUpdate.Attributes.Add("onclick", "return true" +
                    strStatement +
                    strRequired +
                    strExpedite +
                    " && ProcessButton(this) && LoadWait();");
            }
            //Check for returned requests
            if (boolReqReturned == true || boolReqDenied == true)
            {
                int intReturned = 0;
                Int32.TryParse(lblReturned.Text, out intReturned);
                ResourceRequest oResourceRequest = new ResourceRequest(0, dsn);
                DataSet dsResource = oResourceRequest.GetAllService(intRequest, intReturned, Int32.Parse(lblNumber.Text));
                if (dsResource.Tables[0].Rows.Count > 0)
                {
                    int intResource = Int32.Parse(dsResource.Tables[0].Rows[0]["rrid"].ToString());
                    DataSet dsRR = oResourceRequest.Get(intResource);
                    Users oUser = new Users(0, dsn);
                    if (dsRR.Tables[0].Rows.Count > 0)
                    {
                        if (dsRR.Tables[0].Rows[0]["status"].ToString() == "-1")
                        {
                            // Request Denied
                            lblReqDenyCommentValue.Text = dsRR.Tables[0].Rows[0]["reason"].ToString();
                            if (lblReqDenyCommentValue.Text == "")
                            {
                                int intApproved = 0;
                                // Try getting from service approval.
                                DataSet dsSelected2 = oService.GetSelected(intRequest, intReturned, Int32.Parse(lblNumber.Text));
                                for (int ii = Int32.Parse(lblNumber.Text); ii > 0 && dsSelected2.Tables[0].Rows.Count == 0; ii--)
                                    dsSelected2 = oService.GetSelected(intRequest, intReturned, ii - 1);
                                if (dsSelected2.Tables[0].Rows.Count > 0)
                                    intApproved = Int32.Parse(dsSelected2.Tables[0].Rows[0]["approved"].ToString());
                                if (intApproved < 0)
                                    lblReqDenyCommentValue.Text = dsSelected2.Tables[0].Rows[0]["reason"].ToString();
                            }
                            pnlReqDenied.Visible = true;
                        }
                        else if (dsRR.Tables[0].Rows[0]["status"].ToString() == "7")
                        {
                            DataSet dsRRReturn = oResourceRequest.getResourceRequestReturn(intResource, intReturned, Int32.Parse(lblNumber.Text), 0, 0);
                            if (dsRRReturn.Tables[0].Rows.Count > 0)
                            {
                                lblReqReturnedId.Text = dsRRReturn.Tables[0].Rows[0]["Id"].ToString();
                                lblReqReturnCommentValue.Text = oFunction.FormatText(dsRRReturn.Tables[0].Rows[0]["Comments"].ToString());
                                lblReqReturnedByValue.Text = oUser.GetName(Int32.Parse(dsRRReturn.Tables[0].Rows[0]["ReturnedByUser"].ToString()));
                                lblReqReturnedByValue.Text = oUser.GetFullName(Int32.Parse(dsRRReturn.Tables[0].Rows[0]["ReturnedByUser"].ToString()));
                                pnlReqReturn.Visible = true;
                            }
                            DataSet dsRRReturn2 = oResourceRequest.getResourceRequestReturn(intResource, intReturned, Int32.Parse(lblNumber.Text), 1, 0);
                            if (dsRRReturn2.Tables[0].Rows.Count > 0)
                            {
                                lblReqReturnedId2.Text = dsRRReturn2.Tables[0].Rows[0]["Id"].ToString();
                                lblReqReturnCommentValue2.Text = oFunction.FormatText(dsRRReturn2.Tables[0].Rows[0]["Comments"].ToString());
                                lblReqReturnedByValue2.Text = oUser.GetName(Int32.Parse(dsRRReturn2.Tables[0].Rows[0]["ReturnedByUser"].ToString()));
                                lblReqReturnedByValue2.Text = oUser.GetFullName(Int32.Parse(dsRRReturn2.Tables[0].Rows[0]["ReturnedByUser"].ToString()));
                                pnlReqReturn2.Visible = true;
                            }
                        }
                        else
                        {
                            pnlReqReturn.Visible = false;
                            pnlReqReturn2.Visible = false;
                        }
                    }
                }
            }

        }
        protected void btnBack_Click(Object Sender, EventArgs e)
        {
            int intRequest = Int32.Parse(Request.QueryString["rid"]);
            oRequestItem.UpdateForm(intRequest, false);
            Response.Redirect(oPage.GetFullLink(intPage) + "?rid=" + intRequest.ToString());
        }
        protected void btnNext_Click(Object Sender, EventArgs e)
        {
            int intRequest = Int32.Parse(Request.QueryString["rid"]);
            Save(intRequest);
            oRequestItem.UpdateForm(intRequest, true);
            Response.Redirect(oPage.GetFullLink(intPage) + "?rid=" + intRequest.ToString());
        }
        protected void btnUpdate_Click(Object Sender, EventArgs e)
        {
            int intRequest = Int32.Parse(Request.QueryString["rid"]);
            Save(intRequest);
            Response.Redirect(oPage.GetFullLink(intPage) + "?rid=" + intRequest.ToString());
        }
        private void Save(int intRequest)
        {
            int intItem = Int32.Parse(lblItem.Text);
            int intNumber = Int32.Parse(lblNumber.Text);
            int intService = Int32.Parse(lblService.Text);
            int intProject = oRequest.GetProjectNumber(intRequest);
            //if (intProject == -1)
            //    oServiceRequest.Update(intRequest, txtTitle.Text);
            oServiceEditor.AddRequest(intRequest, intService, intNumber, txtTitle.Text, Int32.Parse(radPriority.SelectedItem.Value), txtStatement.Text, DateTime.Now, DateTime.Now, (radExpediteYes.Checked ? 1 : 0));
            oServiceEditor.SaveForm(Request, intRequest, intService, intNumber, false, intEnvironment, dsn);

            //Reset the Resource Request and Workflow status for returned requests
            if (boolReqReturned==true)
            {
                int intReturned = 0;
                Int32.TryParse(lblReturned.Text, out intReturned);
                ResourceRequest oResourceRequest = new ResourceRequest(0, dsn);
                DataSet dsRR = oResourceRequest.GetRequestService(intRequest, intReturned, Int32.Parse(lblNumber.Text));
                if (dsRR.Tables[0].Rows.Count > 0)
                {
                    int intRRId = Int32.Parse(dsRR.Tables[0].Rows[0]["parent"].ToString());
                    oResourceRequest.updateResourceRequestReturnCompleted(Int32.Parse(lblReqReturnedId.Text));
                    DataSet dsRRWF = oResourceRequest.GetWorkflowsParent(intRRId);
                    oResourceRequest.UpdateStatusRequest(intRRId, 2);
                    foreach (DataRow dr in dsRRWF.Tables[0].Rows)
                    {
                        int intRRWFId = Int32.Parse(dr["id"].ToString());
                        oResourceRequest.UpdateWorkflowStatus(intRRWFId, 2, true);
                    }
                }

            }
            //Reset the Resource Request and approval process for denied requests
            if (boolReqDenied == true)
            {
                ResourceRequest oResourceRequest = new ResourceRequest(0, dsn);
                DataSet dsResource = oResourceRequest.GetAllService(intRequest, intService, Int32.Parse(lblNumber.Text));
                if (dsResource.Tables[0].Rows.Count > 0)
                {
                    int intResource = Int32.Parse(dsResource.Tables[0].Rows[0]["rrid"].ToString());
                    DataSet dsRR = oResourceRequest.Get(intResource);
                    if (dsRR.Tables[0].Rows.Count > 0)
                    {
                        oResourceRequest.UpdateStatusOverall(intResource, 2);
                        oService.UpdateSelectedApprove(intRequest, intService, Int32.Parse(lblNumber.Text), 0, 0, DateTime.Now, "");
                        if (oServiceRequest.NotifyApproval(intResource, intResourceRequestApprove, intEnvironment, "", dsnServiceEditor) == false)
                            oServiceRequest.NotifyTeamLead(intItem, intResource, intAssignPage, intViewPage, intEnvironment, "", dsnServiceEditor, dsnAsset, dsnIP, 0);
                    }
                }
            }
        }
        protected void btnCancel_Click(Object Sender, EventArgs e)
        {
            int intRequest = Int32.Parse(Request.QueryString["rid"]);
            Response.Redirect(oPage.GetFullLink(intPage) + "?rid=" + intRequest.ToString());
        }
        protected void btnCancelR_Click(Object Sender, EventArgs e)
        {
            int intRequest = Int32.Parse(Request.QueryString["rid"]);
            ServiceRequests oServiceRequest = new ServiceRequests(intProfile, dsn);
            ResourceRequest oResourceRequest = new ResourceRequest(intProfile, dsn);
            Requests oRequest = new Requests(intProfile, dsn);
            Users oUser = new Users(intProfile, dsn);
            Projects oProject = new Projects(intProfile, dsn);
            int intItem = Int32.Parse(lblItem.Text);
            int intNumber = Int32.Parse(lblNumber.Text);
            int intService = Int32.Parse(lblService.Text);
            int intProject = oRequest.GetProjectNumber(intRequest);

            // Remove Resource Request
            DataSet dsForm = oRequestItem.GetForms(intRequest, intService);
            string strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_REQUEST_ASSIGNMENT,EMAILGRP_REQUEST_STATUS");
            foreach (DataRow drForm in dsForm.Tables[0].Rows)
            {
                int intCancelNum = Int32.Parse(drForm["number"].ToString());
                if (intNumber > 0)
                    intCancelNum = intNumber;
                DataSet dsResource = oResourceRequest.Get(intRequest, intItem, intCancelNum);
                if (dsResource.Tables[0].Rows.Count > 0)
                {
                    int intResourceWorkflow = Int32.Parse(dsResource.Tables[0].Rows[0]["id"].ToString());
                    int intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);
                    oResourceRequest.UpdateStatusOverall(intResourceParent, -2);
                    DataSet dsManager = oService.GetUser(intService, 1);
                    DataSet dsResources = oResourceRequest.GetWorkflowsParent(intResourceParent);
                    foreach (DataRow drResources in dsResources.Tables[0].Rows)
                    {
                        int intUser = Int32.Parse(drResources["userid"].ToString());
                        if (intUser == 0)
                        {
                            foreach (DataRow drManager in dsManager.Tables[0].Rows)
                            {
                                int intManager = Int32.Parse(drManager["userid"].ToString());
                                oFunction.SendEmail("ClearView Service Request CANCELLED", oUser.GetName(intManager), "", strEMailIdsBCC, "ClearView Service Request CANCELLED", "<p><b>The following service request has been cancelled by " + oUser.GetFullName(intProfile) + "</b><p><p>" + oResourceRequest.GetWorkflowSummary(Int32.Parse(drResources["id"].ToString()), intEnvironment, dsnServiceEditor, dsnAsset, dsnIP) + "</p>", true, false);
                            }
                        }
                        else
                        {
                            foreach (DataRow drManager in dsManager.Tables[0].Rows)
                            {
                                int intManager = Int32.Parse(drManager["userid"].ToString());
                                oFunction.SendEmail("ClearView Service Request CANCELLED", oUser.GetName(intUser), oUser.GetName(intManager), strEMailIdsBCC, "ClearView Service Request CANCELLED", "<p><b>The following service request has been cancelled by " + oUser.GetFullName(intProfile) + "</b><p><p>" + oResourceRequest.GetWorkflowSummary(Int32.Parse(drResources["id"].ToString()), intEnvironment, dsnServiceEditor, dsnAsset, dsnIP) + "</p>", true, false);
                            }
                        }
                    }
                }
                oService.CancelSelected(intRequest, intService, intCancelNum);
                if (intCancelNum > 0)
                    break;
            }
            DataSet ds = oRequest.Gets(intProject);
            if (ds.Tables[0].Rows.Count == 0)
                oProject.Delete(intProject);
            Response.Redirect(oPage.GetFullLink(intPage));
        }
    }
}