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
using NCBClass;
namespace NCC.ClearView.Presentation.Web
{
    public partial class wm_pnc_PREPROD : System.Web.UI.UserControl
    {
        private DataSet ds;
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        private string dsnServiceEditor = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ServiceEditorDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected string strVirtual = ConfigurationManager.AppSettings["VirtualGatekeeper"];
        protected int intAssignPage = Int32.Parse(ConfigurationManager.AppSettings["AssignWorkload"]);
        protected int intResourceRequestApprove = Int32.Parse(ConfigurationManager.AppSettings["ResourceRequestApprove"]);
        protected int intViewPage = Int32.Parse(ConfigurationManager.AppSettings["ViewWorkload"]);
        protected int intMyWork = Int32.Parse(ConfigurationManager.AppSettings["MyWork"]);
        protected string strBCC = ConfigurationManager.AppSettings["WorkflowBCC"];
        protected int intProfile;
        protected Projects oProject;
        protected Functions oFunction;
        protected Users oUser;
        protected Pages oPage;
        protected ResourceRequest oResourceRequest;
        protected RequestItems oRequestItem;
        protected Requests oRequest;
        protected Services oService;
        protected ServiceRequests oServiceRequest;
        protected RequestFields oRequestField;
        protected Applications oApplication;
        protected ServiceDetails oServiceDetail;
        protected Delegates oDelegate;
        protected Documents oDocument;
        protected OnDemandTasks oOnDemandTasks;
        protected int intApplication = 0;
        protected int intPage = 0;
        protected int intProject = 0;
        protected bool boolDetails = false;
        protected bool boolExecution = false;
        protected bool boolChange = false;
        protected bool boolDocuments = false;
        protected int intRequest = 0;
        protected int intItem = 0;
        protected int intService = 0;
        protected int intNumber = 0;
        protected bool boolCheckboxes = false;
        protected string strCheckboxes = "";
        protected bool boolJoined = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oProject = new Projects(intProfile, dsn);
            oFunction = new Functions(intProfile, dsn, intEnvironment);
            oUser = new Users(intProfile, dsn);
            oPage = new Pages(intProfile, dsn);
            oResourceRequest = new ResourceRequest(intProfile, dsn);
            oRequestItem = new RequestItems(intProfile, dsn);
            oRequest = new Requests(intProfile, dsn);
            oService = new Services(intProfile, dsn);
            oServiceRequest = new ServiceRequests(intProfile, dsn);
            oRequestField = new RequestFields(intProfile, dsn);
            oApplication = new Applications(intProfile, dsn);
            oServiceDetail = new ServiceDetails(intProfile, dsn);
            oDelegate = new Delegates(intProfile, dsn);
            oDocument = new Documents(intProfile, dsn);
            oOnDemandTasks = new OnDemandTasks(intProfile, dsn);

            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            if (Request.QueryString["rrid"] != null && Request.QueryString["rrid"] != "")
            {
                // Start Workflow Change
                lblResourceWorkflow.Text = Request.QueryString["rrid"];
                int intResourceWorkflow = Int32.Parse(Request.QueryString["rrid"]);
                int intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);
                ds = oResourceRequest.Get(intResourceParent);
                // End Workflow Change
                intRequest = Int32.Parse(ds.Tables[0].Rows[0]["requestid"].ToString());
                lblRequestedBy.Text = oUser.GetFullName(oRequest.GetUser(intRequest));
                lblRequestedOn.Text = DateTime.Parse(oRequest.Get(intRequest, "created")).ToString();
                lblDescription.Text = oRequest.Get(intRequest, "description");
                if (lblDescription.Text == "")
                    lblDescription.Text = "<i>No information</i>";
                intItem = Int32.Parse(ds.Tables[0].Rows[0]["itemid"].ToString());
                intNumber = Int32.Parse(ds.Tables[0].Rows[0]["number"].ToString());
                // Start Workflow Change
                bool boolComplete = (oResourceRequest.GetWorkflow(intResourceWorkflow, "status") == "3");
                int intUser = Int32.Parse(oResourceRequest.GetWorkflow(intResourceWorkflow, "userid"));
                txtCustom.Text = oResourceRequest.GetWorkflow(intResourceWorkflow, "name");
                double dblAllocated = double.Parse(oResourceRequest.GetWorkflow(intResourceWorkflow, "allocated"));
                boolJoined = (oResourceRequest.GetWorkflow(intResourceWorkflow, "joined") == "1");
                // End Workflow Change
                intService = Int32.Parse(ds.Tables[0].Rows[0]["serviceid"].ToString());
                lblService.Text = oService.Get(intService, "name");
                int intApp = oRequestItem.GetItemApplication(intItem);


                if (Request.QueryString["save"] != null && Request.QueryString["save"] != "")
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "saved", "<script type=\"text/javascript\">alert('Information Saved Successfully');<" + "/" + "script>");
                if (Request.QueryString["status"] != null && Request.QueryString["status"] != "")
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "statusd", "<script type=\"text/javascript\">alert('Weekly Status has been Added');<" + "/" + "script>");
                if (!IsPostBack)
                {
                    double dblUsed = oResourceRequest.GetWorkflowUsed(intResourceWorkflow);
                    //Show Service related panels

                    if (intService == Int32.Parse(ConfigurationManager.AppSettings["SERVICEID_PNC_SAV_ESM"]))
                    {
                        panSAV.Visible = true;
                        chkSAV.Checked = (dblAllocated == dblUsed);
                    }

                    if (intService == Int32.Parse(ConfigurationManager.AppSettings["SERVICEID_PNC_HIDS"]))
                    {
                        panHIDS.Visible = true;
                        chkHIDS.Checked = (dblAllocated == dblUsed);

                    }

                    if (intService == Int32.Parse(ConfigurationManager.AppSettings["SERVICEID_PNC_ECM"]))
                    {
                        panECM.Visible = true;
                        chkECM.Checked = (dblAllocated == dblUsed);
                    }

                    if (intService == Int32.Parse(ConfigurationManager.AppSettings["SERVICEID_PNC_NETIQ"]))
                    {
                        panNetIQ.Visible = true;
                        chkNetIQ.Checked = (dblAllocated == dblUsed);
                    }

                    if (intService == Int32.Parse(ConfigurationManager.AppSettings["SERVICEID_PNC_AUDIT"]))
                    {
                        panAudit.Visible = true;
                        chkAudit.Checked = (dblAllocated == dblUsed);
                    }

                    lblUpdated.Text = oResourceRequest.GetUpdated(intResourceParent);
                    if (dblAllocated == dblUsed)
                    {
                        if (boolComplete == false)
                        {
                            oFunction.ConfigureToolButton(btnComplete, "/images/tool_complete");
                            btnComplete.Attributes.Add("onclick", "return confirm('Are you sure you want to mark this as completed and remove it from your workload?');");
                        }
                        else
                        {
                            oFunction.ConfigureToolButton(btnComplete, "/images/tool_complete");
                            btnComplete.Attributes.Add("onclick", "alert('This task has already been marked COMPLETE. You can close this window.');return false;");
                        }
                    }
                    else
                    {
                        btnComplete.ImageUrl = "/images/tool_complete_dbl.gif";
                        btnComplete.Enabled = false;
                    }
                    if (oService.Get(intService, "sla") != "")
                    {
                        oFunction.ConfigureToolButton(btnSLA, "/images/tool_sla");
                        int intDays = oResourceRequest.GetSLA(intResourceParent);
                        if (intDays < 1)
                            btnSLA.Style["border"] = "solid 2px #FF0000";
                        else if (intDays < 3)
                            btnSLA.Style["border"] = "solid 2px #FF9999";
                        btnSLA.Attributes.Add("onclick", "return OpenWindow('RESOURCE_REQUEST_SLA','?rrid=" + intResourceParent.ToString() + "');");
                    }
                    else
                    {
                        btnSLA.ImageUrl = "/images/tool_sla_dbl.gif";
                        btnSLA.Enabled = false;
                    }
                    oFunction.ConfigureToolButton(btnEmail, "/images/tool_email");
                    btnEmail.Attributes.Add("onclick", "return OpenWindow('RESOURCE_REQUEST_EMAIL','?rrid=" + intResourceWorkflow.ToString() + "&type=GENERIC');");
                    dblUsed = (dblUsed / dblAllocated) * 100;
                    intProject = oRequest.GetProjectNumber(intRequest);
                    hdnTab.Value = "D";
                    panWorkload.Visible = true;
                    LoadStatus(intResourceWorkflow);
                    LoadChange(intResourceWorkflow);
                    LoadInformation(intResourceWorkflow);
                    chkDescription.Checked = (Request.QueryString["doc"] != null);
                    lblDocuments.Text = oDocument.GetDocuments_Request(intRequest, 0, Request.PhysicalApplicationPath, 1, (Request.QueryString["doc"] != null));
                    // GetDocuments(string _physical, int _projectid, int _requestid, int _userid, int _security, bool _show_description, bool _mine)
                    //lblDocuments.Text = oDocument.GetDocuments(Request.PhysicalApplicationPath, 0, intRequest, 0, 1, (Request.QueryString["doc"] != null), false);

                    btnDenied.Attributes.Add("onclick", "return CloseWindow();");
                    oFunction.ConfigureToolButton(btnSave, "/images/tool_save");
                    oFunction.ConfigureToolButton(btnPrint, "/images/tool_print");
                    btnPrint.Attributes.Add("onclick", "return PrintWindow();");
                    oFunction.ConfigureToolButton(btnClose, "/images/tool_close");
                    btnClose.Attributes.Add("onclick", "return ExitWindow();");
                    btnSave.Attributes.Add("onclick", "return ValidateStatus('" + ddlStatus.ClientID + "','" + txtComments.ClientID + "');");
                    btnChange.Attributes.Add("onclick", "return ValidateText('" + txtNumber.ClientID + "','Please enter a change control number')" +
                        " && ValidateDate('" + txtDate.ClientID + "','Please enter a valid implementation date')" +
                        " && ValidateTime('" + txtTime.ClientID + "','Please enter a valid implementation time')" +
                        ";");
                    imgDate.Attributes.Add("onclick", "return ShowCalendar('" + txtDate.ClientID + "');");
                    // 6/1/2009 - Load ReadOnly View
                    if (oResourceRequest.CanUpdate(intProfile, intResourceWorkflow, false) == false)
                    {
                        oFunction.ConfigureToolButtonRO(btnSave, btnComplete);
                        //panDenied.Visible = true;
                    }
                }
            }
            else
                panDenied.Visible = true;
        }
        private void LoadStatus(int _resourceid)
        {
            DataSet dsStatus = oResourceRequest.GetStatuss(_resourceid);
            rptStatus.DataSource = dsStatus;
            rptStatus.DataBind();
            lblNoStatus.Visible = (rptStatus.Items.Count == 0);
            double dblTotalStatus = 0.00;
            foreach (RepeaterItem ri in rptStatus.Items)
            {
                Label _status = (Label)ri.FindControl("lblStatus");
                double dblStatus = double.Parse(_status.Text);
                if (dblTotalStatus == 0.00)
                    dblTotalStatus = dblStatus;
                _status.Text = oResourceRequest.GetStatus(dblStatus, 50, 15);
            }
            lblStatus.Text = oResourceRequest.GetStatus(dblTotalStatus, 50, 15);
        }
        private void LoadChange(int _resourceid)
        {
            DataSet dsChange = oResourceRequest.GetChangeControls(_resourceid);
            rptChange.DataSource = dsChange;
            rptChange.DataBind();
            lblNoChange.Visible = (rptChange.Items.Count == 0);
            foreach (RepeaterItem ri in rptChange.Items)
            {
                LinkButton _delete = (LinkButton)ri.FindControl("btnDeleteChange");
                _delete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this change control?');");
            }
        }
        private void LoadInformation(int _request)
        {
            lblView.Text = oRequestField.GetBodyWorkflow(_request, dsnServiceEditor, intEnvironment, dsnAsset, dsnIP);
            if (intProject > 0)
            {
                lblName.Text = oProject.Get(intProject, "name");
                lblNumber.Text = oProject.Get(intProject, "number");
                lblType.Text = "Project";
            }
            else
            {
                lblName.Text = oResourceRequest.GetWorkflow(_request, "name");
                //lblName.Text = oResourceRequest.GetWorkflow(_request, "name");
                lblNumber.Text = "CVT" + intRequest.ToString();
                lblType.Text = "Task";
            }
            if (Request.QueryString["div"] != null)
            {
                switch (Request.QueryString["div"])
                {
                    case "E":
                        boolExecution = true;
                        break;
                    case "C":
                        boolChange = true;
                        break;
                    case "D":
                        boolDocuments = true;
                        break;
                }
            }
            if (boolDetails == false && boolExecution == false && boolChange == false && boolDocuments == false)
                boolDetails = true;
        }
        protected void btnStatus_Click(Object Sender, EventArgs e)
        {
            int intResourceWorkflow = Int32.Parse(lblResourceWorkflow.Text);
            int intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);
            oResourceRequest.AddStatus(intResourceWorkflow, Int32.Parse(ddlStatus.SelectedItem.Value), txtComments.Text);
            Response.Redirect(Request.Path + "?rrid=" + intResourceWorkflow.ToString() + "&div=E&status=true");
        }
        protected void btnChange_Click(Object Sender, EventArgs e)
        {
            int intResourceWorkflow = Int32.Parse(lblResourceWorkflow.Text);
            int intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);
            oResourceRequest.AddChangeControl(intResourceWorkflow, txtNumber.Text, DateTime.Parse(txtDate.Text + " " + txtTime.Text), txtChange.Text);
            Response.Redirect(Request.Path + "?rrid=" + intResourceWorkflow.ToString() + "&div=C&save=true");
        }
        protected void btnDeleteChange_Click(Object Sender, EventArgs e)
        {
            LinkButton oButton = (LinkButton)Sender;
            oResourceRequest.DeleteChangeControl(Int32.Parse(oButton.CommandArgument));
            Response.Redirect(Request.Path + "?rrid=" + lblResourceWorkflow.Text + "&div=C");
        }
        protected void chkDescription_Change(Object Sender, EventArgs e)
        {
            if (chkDescription.Checked == true)
                Response.Redirect(Request.Path + "?rrid=" + Request.QueryString["rrid"] + "&doc=true&div=D");
            else
                Response.Redirect(Request.Path + "?rrid=" + Request.QueryString["rrid"] + "&div=D");
        }
        protected void btnSave_Click(Object Sender, EventArgs e)
        {
            int intResourceWorkflow = Int32.Parse(lblResourceWorkflow.Text);
            int intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);
            if (ddlStatus.SelectedIndex > -1 && txtComments.Text.Trim() != "")
                oResourceRequest.AddStatus(intResourceWorkflow, Int32.Parse(ddlStatus.SelectedItem.Value), txtComments.Text);

            bool boolChecked = false;
            if (panSAV.Visible == true && chkSAV.Checked == true)
                boolChecked = true;

            if (panHIDS.Visible == true && chkHIDS.Checked == true)
                boolChecked = true;

            if (panECM.Visible == true && chkECM.Checked == true)
                boolChecked = true;

            if (panNetIQ.Visible == true && chkNetIQ.Checked == true)
                boolChecked = true;

            if (panAudit.Visible == true && chkAudit.Checked == true)
                boolChecked = true;


            if (boolChecked == true)
            {
                double dblAllocated = double.Parse(oResourceRequest.GetWorkflow(intResourceWorkflow, "allocated"));
                oResourceRequest.UpdateWorkflowHoursOverwrite(intResourceWorkflow, dblAllocated);
            }
            else
                oResourceRequest.UpdateWorkflowHoursOverwrite(intResourceWorkflow, 0.00);

            oResourceRequest.UpdateWorkflowName(intResourceWorkflow, txtCustom.Text, intProfile);
            Response.Redirect(Request.Path + "?rrid=" + intResourceWorkflow.ToString() + "&div=E&save=true");
        }
        protected void btnComplete_Click(Object Sender, EventArgs e)
        {
            int intResourceWorkflow = Int32.Parse(lblResourceWorkflow.Text);
            int intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);
            //oResourceRequest.UpdateWorkflowStatus(intResourceWorkflow, 3, true);
            //oResourceRequest.CloseWorkflow(intResourceWorkflow, intEnvironment, strBCC, 0, dsnServiceEditor, true, intResourceRequestApprove, intAssignPage, intViewPage, dsnAsset, dsnIP);

            int intAnswer = 0;
            int intModel = 0;
            DataSet ds = oOnDemandTasks.GetServerOther(intRequest, intService, intNumber);
            if (ds.Tables[0].Rows.Count > 0)
            {
                intAnswer = Int32.Parse(ds.Tables[0].Rows[0]["answerid"].ToString());
                intModel = Int32.Parse(ds.Tables[0].Rows[0]["modelid"].ToString());
            }

            bool boolComplete = true;

            //Check for all the service are complete and kick of the other services
            int intService1 = 0;
            DataSet dsRR;
            intService1 = Int32.Parse(ConfigurationManager.AppSettings["SERVICEID_PNC_SAV_ESM"]);
            dsRR = oResourceRequest.GetRequestService(intRequest, intService1, intNumber);
            if (dsRR.Tables[0].Rows.Count > 0)
            {
                int intStatus = Int32.Parse(dsRR.Tables[0].Rows[0]["status"].ToString());
                if (intStatus != 3)
                    boolComplete = false;
            }

            intService1 = Int32.Parse(ConfigurationManager.AppSettings["SERVICEID_PNC_HIDS"]);
            dsRR = oResourceRequest.GetRequestService(intRequest, intService1, intNumber);

            if (dsRR.Tables[0].Rows.Count > 0)
            {
                int intStatus = Int32.Parse(dsRR.Tables[0].Rows[0]["status"].ToString());
                if (intStatus != 3)
                    boolComplete = false;
            }


            intService1 = Int32.Parse(ConfigurationManager.AppSettings["SERVICEID_PNC_ECM"]);
            dsRR = oResourceRequest.GetRequestService(intRequest, intService1, intNumber);
            if (dsRR.Tables[0].Rows.Count > 0)
            {
                int intStatus = Int32.Parse(dsRR.Tables[0].Rows[0]["status"].ToString());
                if (intStatus != 3)
                    boolComplete = false;
            }


            intService1 = Int32.Parse(ConfigurationManager.AppSettings["SERVICEID_PNC_NETIQ"]);
            dsRR = oResourceRequest.GetRequestService(intRequest, intService1, intNumber);
            if (dsRR.Tables[0].Rows.Count > 0)
            {
                int intStatus = Int32.Parse(dsRR.Tables[0].Rows[0]["status"].ToString());
                if (intStatus != 3)
                    boolComplete = false;
            }


            intService1 = Int32.Parse(ConfigurationManager.AppSettings["SERVICEID_PNC_AUDIT"]);
            dsRR = oResourceRequest.GetRequestService(intRequest, intService1, intNumber);
            if (dsRR.Tables[0].Rows.Count > 0)
            {
                int intStatus = Int32.Parse(dsRR.Tables[0].Rows[0]["status"].ToString());
                if (intStatus != 3)
                    boolComplete = false;
            }


            if (boolComplete == true)
            {
                // Send next 3 services

                Forecast oForecast = new Forecast(0, dsn);

                bool boolCluster = false;

                if (oForecast.IsHACluster(intAnswer) == true)

                    boolCluster = true;

                if (boolCluster == true)
                {

                    // send Cluster 
                    int intImplementorUser = 0;
                    int intImplementorResource = 0;

                    DataSet dsImplementor = oOnDemandTasks.GetPending(intAnswer);
                    if (dsImplementor.Tables[0].Rows.Count > 0)
                    {
                        intImplementorResource = Int32.Parse(dsImplementor.Tables[0].Rows[0]["resourceid"].ToString());
                        intImplementorUser = Int32.Parse(oResourceRequest.GetWorkflow(intImplementorResource, "userid"));
                    }

                    if (intImplementorUser > 0)
                    {
                        Variables oVariable = new Variables(intEnvironment);
                        int intServiceId = Int32.Parse(ConfigurationManager.AppSettings["SERVICEID_PNC_CLUSTER"]);
                        int intServiceItemId = oService.GetItemId(intServiceId);
                        int intServiceNumber = 1;
                        oOnDemandTasks.AddServerOther(intRequest, intServiceId, intServiceNumber, intAnswer, intModel);
                        double dblServiceHours = oServiceDetail.GetHours(intServiceId, 1);
                        int intResource = oServiceRequest.AddRequest(intRequest, intServiceItemId, intServiceId, 1, dblServiceHours, 2, intServiceNumber, dsnServiceEditor);
                        // Comment the following...
                        //oServiceRequest.NotifyTeamLead(intServiceItemId, intResource, intAssignPage, intViewPage, intEnvironment, strBCC, "", dsnServiceEditor, dsnAsset, dsnIP);
                        // Include the following...
                        int intResourceWorkflowNEW = oResourceRequest.AddWorkflow(intResource, 0, oResourceRequest.Get(intResource, "name"), intImplementorUser, Int32.Parse(oResourceRequest.Get(intResource, "devices")), double.Parse(oResourceRequest.Get(intResource, "allocated")), 2, 1);
                        string strService = oService.GetName(intServiceId);
                        if (intServiceId == 0)
                            strService = oRequestItem.GetItemName(intServiceItemId);
                        int intApp = oRequestItem.GetItemApplication(intServiceItemId);
                        string strDefault = oUser.GetApplicationUrl(intImplementorUser, intViewPage);
                        if (strDefault == "")
                            oFunction.SendEmail("Request Assignment: " + strService, oUser.GetName(intImplementorUser), "", strBCC, "Request Assignment: " + strService, "<p><b>The following request has been automatically assigned to you</b><p><p>" + oResourceRequest.GetWorkflowSummary(intResourceWorkflowNEW, intEnvironment, dsnServiceEditor, dsnAsset, dsnIP) + "</p>", true, false);
                        else
                        {
                            if (intProject > 0)
                                oFunction.SendEmail("Request Assignment: " + strService, oUser.GetName(intImplementorUser), "", strBCC, "Request Assignment: " + strService, "<p><b>The following request has been automatically assigned to you</b><p><p><a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=/" + strDefault + oPage.GetFullLink(intViewPage) + "?pid=" + intProject.ToString() + "\" target=\"_blank\">Click here to review your new assignment.</a></p><p>" + oResourceRequest.GetWorkflowSummary(intResourceWorkflowNEW, intEnvironment, dsnServiceEditor, dsnAsset, dsnIP) + "</p>" + oVariable.EmailFooter(), true, false);
                            else
                                oFunction.SendEmail("Request Assignment: " + strService, oUser.GetName(intImplementorUser), "", strBCC, "Request Assignment: " + strService, "<p><b>The following request has been automatically assigned to you</b><p><p><a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=/frame/resource_request.aspx?rrid=" + intResourceWorkflowNEW.ToString() + "\" target=\"_blank\">Click here to review your new assignment.</a></p><p>" + oResourceRequest.GetWorkflowSummary(intResourceWorkflowNEW, intEnvironment, dsnServiceEditor, dsnAsset, dsnIP) + "</p>" + oVariable.EmailFooter(), true, false);
                        }
                        oProject.Update(intProject, 2);
                        oResourceRequest.UpdateAccepted(intResource, 1);
                        oResourceRequest.UpdateAssignedBy(intResource, -1000);

                    }

                }

                //Send SQL Task
                bool boolSQL = false;

                Servers oServer = new Servers(0, dsn);

                DataSet dsServers = oServer.GetAnswer(intAnswer);

                foreach (DataRow drServer in dsServers.Tables[0].Rows)
                {

                    int intServer = Int32.Parse(drServer["id"].ToString());

                    DataSet dsComponents = oServer.GetComponents(intServer);

                    foreach (DataRow drComponent in dsComponents.Tables[0].Rows)
                    {

                        boolSQL = (drComponent["code"].ToString() == "SQL");

                        if (boolSQL == true)

                            break;

                    }

                    if (boolSQL == true)

                        break;

                }

                if (boolSQL == true)
                {

                    // send SQL
                    //Generate HIDS Task
                    int intServiceId = Int32.Parse(ConfigurationManager.AppSettings["SERVICEID_PNC_SQL"]);
                    int intServiceItemId = oService.GetItemId(intServiceId);
                    int intServiceNumber = 1;
                    oOnDemandTasks.AddServerOther(intRequest, intServiceId, intServiceNumber, intAnswer, intModel);
                    double dblServiceHours = oServiceDetail.GetHours(intServiceId, 1);
                    int intResource = oServiceRequest.AddRequest(intRequest, intServiceItemId, intServiceId, 1, dblServiceHours, 2, intServiceNumber, dsnServiceEditor);
                    oServiceRequest.NotifyTeamLead(intServiceItemId, intResource, intAssignPage, intViewPage, intEnvironment, strBCC, "", dsnServiceEditor, dsnAsset, dsnIP, 0);

                }

                //Lotus notes task generation
            }



            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "reload", "<script type=\"text/javascript\">if(window.opener!=null)window.opener.navigate('" + oPage.GetFullLink(intMyWork) + "');window.close();<" + "/" + "script>");
        }
    }
}