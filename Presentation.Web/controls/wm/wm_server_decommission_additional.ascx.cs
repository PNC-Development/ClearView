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
    public partial class wm_server_decommission_additional : System.Web.UI.UserControl
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
        protected int intServiceStorage = Int32.Parse(ConfigurationManager.AppSettings["SERVICEID_SERVER_DECOMMISSION_STORAGE"]);
        protected int intServiceBackup = Int32.Parse(ConfigurationManager.AppSettings["SERVICEID_SERVER_DECOMMISSION_BACKUP"]);
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
        protected int intServer = 0;
        protected bool boolCheckboxes = false;
        protected string strCheckboxes = "";
        protected bool boolJoined = false;
        protected Servers oServer;
        protected Customized oCustomized;
        protected Variables oVariable;

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
            oServer = new Servers(intProfile, dsn);
            oCustomized = new Customized(intProfile, dsn);
            oVariable = new Variables(intEnvironment);
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
                lblRequestedOn.Text = DateTime.Parse(oResourceRequest.Get(intResourceParent, "created")).ToString();
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
                // Add in amount panel for reclaiming of storage
                string strRequired = "true";
                string strAmount = "0";
                if (intServiceStorage == intService || intServiceBackup == intService)
                {
                    lblStorageAmt.Text = "0.00";
                    lblStorageTier.Text = "0";
                    lblStorageEnvironment.Text = "";
                    DataSet dsDecommission = oCustomized.GetDecommissionServer(intRequest, intItem, intNumber);
                    if (dsDecommission.Tables[0].Rows.Count > 0)
                    {
                        intServer = Int32.Parse(dsDecommission.Tables[0].Rows[0]["serverid"].ToString());
                        if (intServiceStorage == intService)
                        {
                            panStorage.Visible = true;
                            strRequired = "ValidateStorage('" + txtPreCooldown.ClientID + "','" + txtCooldown.ClientID + "','" + txtCR2.ClientID + "','" + txtStorage.ClientID + "','" + radClassificationM.ClientID + "','" + radClassificationL.ClientID + "','" + radVendorE.ClientID + "','" + radVendorH.ClientID + "','" + ddlLocation.ClientID + "','" + txtSerial.ClientID + "')";
                            radClassificationM.Attributes.Add("onclick", "ShowHideDiv('" + trMainstream1.ClientID + "','inline');ShowHideDiv('" + trMainstream2.ClientID + "','inline');ShowHideDiv('" + trMainstream3.ClientID + "','inline');");
                            radClassificationL.Attributes.Add("onclick", "ShowHideDiv('" + trMainstream1.ClientID + "','none');ShowHideDiv('" + trMainstream2.ClientID + "','none');ShowHideDiv('" + trMainstream3.ClientID + "','none');");
                            imgPreCooldown.Attributes.Add("onclick", "return ShowCalendar('" + txtPreCooldown.ClientID + "');");
                            imgCooldown.Attributes.Add("onclick", "return ShowCalendar('" + txtCooldown.ClientID + "');");
                            strAmount = dsDecommission.Tables[0].Rows[0]["reclaimed_storage"].ToString();
                        }
                        if (intServiceBackup == intService)
                        {
                            panBackup.Visible = true;
                            strRequired = "ValidateNumber0('" + txtBackup.ClientID + "', 'Please enter a valid amount')";
                            strAmount = dsDecommission.Tables[0].Rows[0]["reclaimed_backup"].ToString();
                        }
                        Storage oStorage = new Storage(0, dsn);
                        DataSet dsDW = oStorage.GetStorageDW(dsDecommission.Tables[0].Rows[0]["servername"].ToString());
                        if (dsDW.Tables[0].Rows.Count > 0)
                        {
                            lblStorageAmt.Text = dsDW.Tables[0].Rows[0]["StorageAmt"].ToString();
                            lblStorageTier.Text = dsDW.Tables[0].Rows[0]["Tier"].ToString();
                            lblStorageEnvironment.Text = dsDW.Tables[0].Rows[0]["Environment"].ToString();
                            if (intServiceStorage == intService)
                                lblStorage.Text = " of " + dsDW.Tables[0].Rows[0]["StorageAmt"].ToString() + " GB on <b>" + dsDW.Tables[0].Rows[0]["Environment"].ToString() + "</b>";
                            if (intServiceBackup == intService)
                                lblBackup.Text = " of " + dsDW.Tables[0].Rows[0]["StorageAmt"].ToString() + " GB on <b>" + dsDW.Tables[0].Rows[0]["Environment"].ToString() + "</b>";
                        }
                        DateTime datPreCooldown = DateTime.MinValue;
                        DateTime.TryParse(dsDecommission.Tables[0].Rows[0]["reclaimed_storage_precooldown"].ToString(), out datPreCooldown);
                        if (datPreCooldown != DateTime.MinValue)
                            txtPreCooldown.Text = datPreCooldown.ToShortDateString();
                        DateTime datCooldown = DateTime.MinValue;
                        DateTime.TryParse(dsDecommission.Tables[0].Rows[0]["reclaimed_storage_cooldown"].ToString(), out datCooldown);
                        if (datCooldown != DateTime.MinValue)
                            txtCooldown.Text = datCooldown.ToShortDateString();
                        txtCR2.Text = dsDecommission.Tables[0].Rows[0]["reclaimed_storage_cr2"].ToString();
                        radClassificationM.Checked = (dsDecommission.Tables[0].Rows[0]["reclaimed_storage_classification"].ToString() == "M");
                        radClassificationL.Checked = (dsDecommission.Tables[0].Rows[0]["reclaimed_storage_classification"].ToString() == "L");
                        if (radClassificationM.Checked)
                        {
                            trMainstream1.Style["display"] = trMainstream2.Style["display"] = trMainstream3.Style["display"] = "inline";
                            radVendorE.Checked = (dsDecommission.Tables[0].Rows[0]["reclaimed_storage_vendor"].ToString() == "E");
                            radVendorH.Checked = (dsDecommission.Tables[0].Rows[0]["reclaimed_storage_vendor"].ToString() == "H");
                            ddlLocation.SelectedValue = dsDecommission.Tables[0].Rows[0]["reclaimed_storage_location"].ToString();
                            txtSerial.Text = dsDecommission.Tables[0].Rows[0]["reclaimed_storage_array"].ToString();
                        }
                        txtNotes.Text = dsDecommission.Tables[0].Rows[0]["reclaimed_storage_notes"].ToString();
                    }
                }
                lblService.Text = oService.Get(intService, "name");
                int intApp = oRequestItem.GetItemApplication(intItem);

                if (Request.QueryString["save"] != null && Request.QueryString["save"] != "")
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "saved", "<script type=\"text/javascript\">alert('Information Saved Successfully');<" + "/" + "script>");
                if (Request.QueryString["status"] != null && Request.QueryString["status"] != "")
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "statusd", "<script type=\"text/javascript\">alert('Status Updates has been Added');<" + "/" + "script>");
                if (!IsPostBack)
                {
                    if (intServiceStorage == intService || intServiceBackup == intService)
                    {
                        if (intServer > 0)
                            txtStorage.Text = txtBackup.Text = oServer.Get(intServer, (intServiceStorage == intService ? "reclaimed_storage" : "reclaimed_backup"));
                        else
                            txtStorage.Text = txtBackup.Text = strAmount;
                    }
                    double dblUsed = oResourceRequest.GetWorkflowUsed(intResourceWorkflow);
                    lblUpdated.Text = oResourceRequest.GetUpdated(intResourceParent);
                    if (dblAllocated == dblUsed)
                    {
                        if (boolComplete == false)
                        {
                            oFunction.ConfigureToolButton(btnComplete, "/images/tool_complete");
                            btnComplete.Attributes.Add("onclick", "return " + strRequired + " && confirm('Are you sure you want to mark this as completed and remove it from your workload?') && ProcessControlButton();");
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
                    bool boolSLABreached = false;
                    if (oService.Get(intService, "sla") != "")
                    {
                        oFunction.ConfigureToolButton(btnSLA, "/images/tool_sla");
                        int intDays = oResourceRequest.GetSLA(intResourceParent);
                        if (intDays > -99999)
                        {
                            if (intDays < 1)
                                btnSLA.Style["border"] = "solid 2px #FF0000";
                            else if (intDays < 3)
                                btnSLA.Style["border"] = "solid 2px #FF9999";
                            boolSLABreached = (intDays < 0);
                            btnSLA.Attributes.Add("onclick", "return OpenWindow('RESOURCE_REQUEST_SLA','?rrid=" + intResourceParent.ToString() + "');");
                        }
                        else
                        {
                            btnSLA.ImageUrl = "/images/tool_sla_dbl.gif";
                            btnSLA.Enabled = false;
                        }
                    }
                    else
                    {
                        btnSLA.ImageUrl = "/images/tool_sla_dbl.gif";
                        btnSLA.Enabled = false;
                    }
                    oFunction.ConfigureToolButton(btnEmail, "/images/tool_email");
                    btnEmail.Attributes.Add("onclick", "return OpenWindow('RESOURCE_REQUEST_EMAIL','?rrid=" + intResourceWorkflow.ToString() + "&type=GENERIC');");
                    dblUsed = (dblUsed / dblAllocated) * 100;
                    strCheckboxes = oServiceDetail.LoadCheckboxes(intRequest, intItem, intNumber, intResourceWorkflow, intService);
                    if (oService.Get(intService, "tasks") != "1" || strCheckboxes == "")
                    {
                        if (oService.Get(intService, "no_slider") == "1")
                        {
                            panNoSlider.Visible = true;
                            btnComplete.ImageUrl = "/images/tool_complete.gif";
                            btnComplete.Enabled = true;
                            btnComplete.Attributes.Add("onclick", "return " + strRequired + " && confirm('Are you sure you want to mark this as completed and remove it from your workload?') && ProcessControlButton();");
                            btnComplete.CommandArgument = "FAST";
                        }
                        else
                        {
                            panSlider.Visible = true;
                            sldHours._StartPercent = dblUsed.ToString();
                            sldHours._TotalHours = dblAllocated.ToString();
                        }
                    }
                    else
                        panCheckboxes.Visible = true;
                    intProject = oRequest.GetProjectNumber(intRequest);
                    hdnTab.Value = "D";
                    panWorkload.Visible = true;
                    bool boolRed = LoadStatus(intResourceWorkflow);
                    if (boolRed == false && boolSLABreached == true)
                        btnComplete.Attributes.Add("onclick", "alert('NOTE: Your Service Level Agreement (SLA) has been breached!\\n\\nYou must provide a RED STATUS update with an explanation of why your SLA was breached for this request.\\n\\nOnce a RED STATUS update has been provided, you will be able to complete this request.');return false;");
                    LoadChange(intResourceWorkflow);
                    LoadInformation(intResourceWorkflow);
                    chkDescription.Checked = (Request.QueryString["doc"] != null);
                    lblDocuments.Text = oDocument.GetDocuments_Service(intRequest, intService, oVariable.DocumentsFolder(), 1, (Request.QueryString["doc"] != null));
                    // GetDocuments(string _physical, int _projectid, int _requestid, int _userid, int _security, bool _show_description, bool _mine)
                    //lblDocuments.Text = oDocument.GetDocuments(Request.PhysicalApplicationPath, 0, intRequest, 0, 1, (Request.QueryString["doc"] != null), false);

                    btnDenied.Attributes.Add("onclick", "return CloseWindow();");
                    oFunction.ConfigureToolButton(btnSave, "/images/tool_save");
                    oFunction.ConfigureToolButton(btnPrint, "/images/tool_print");
                    btnPrint.Attributes.Add("onclick", "return PrintWindow();");
                    oFunction.ConfigureToolButton(btnClose, "/images/tool_close");
                    btnClose.Attributes.Add("onclick", "return ExitWindow();");
                    btnSave.Attributes.Add("onclick", "return ValidateStatus('" + ddlStatus.ClientID + "','" + txtComments.ClientID + "') && ProcessControlButton();");
                    btnChange.Attributes.Add("onclick", "return ValidateText('" + txtNumber.ClientID + "','Please enter a change control number')" +
                        " && ValidateDate('" + txtDate.ClientID + "','Please enter a valid implementation date')" +
                        " && ValidateTime('" + txtTime.ClientID + "','Please enter a valid implementation time')" +
                        " && ProcessControlButton()" +
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
        private bool LoadStatus(int _resourceid)
        {
            bool boolRed = false;
            DataSet dsStatus = oResourceRequest.GetStatuss(_resourceid);
            rptStatus.DataSource = dsStatus;
            rptStatus.DataBind();
            lblNoStatus.Visible = (rptStatus.Items.Count == 0);
            double dblTotalStatus = 0.00;
            foreach (RepeaterItem ri in rptStatus.Items)
            {
                Label _status = (Label)ri.FindControl("lblStatus");
                if (boolRed == false && _status.Text == "1")
                    boolRed = true;
                double dblStatus = double.Parse(_status.Text);
                if (dblTotalStatus == 0.00)
                    dblTotalStatus = dblStatus;
                _status.Text = oResourceRequest.GetStatus(dblStatus, 50, 15);
            }
            lblStatus.Text = oResourceRequest.GetStatus(dblTotalStatus, 50, 15);
            return boolRed;
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
            oResourceRequest.AddStatus(intResourceWorkflow, Int32.Parse(ddlStatus.SelectedItem.Value), txtComments.Text, intProfile);
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
            {
                oResourceRequest.AddStatus(intResourceWorkflow, Int32.Parse(ddlStatus.SelectedItem.Value), txtComments.Text, intProfile);
                //CVT62149 Workload Manager Red Light Status =Hold
                if (ddlStatus.SelectedValue == "1") //Red
                    oResourceRequest.UpdateWorkflowStatus(intResourceWorkflow, 5, true);
                else
                    oResourceRequest.UpdateWorkflowStatus(intResourceWorkflow, 2, true); 
            }
            if (panNoSlider.Visible == false)
            {
                double dblHours = 0.00;
                if (panSlider.Visible == true)
                {
                    if (Request.Form["hdnHours"] != null && Request.Form["hdnHours"] != "")
                        dblHours = double.Parse(Request.Form["hdnHours"]);
                    double dblUsed = oResourceRequest.GetWorkflowUsed(intResourceWorkflow);
                    dblHours = (dblHours - dblUsed);
                    if (dblHours > 0.00)
                        oResourceRequest.UpdateWorkflowHours(intResourceWorkflow, dblHours);
                }
                else
                {
                    oServiceDetail.UpdateCheckboxes(Request, intResourceWorkflow, intRequest, intItem, intNumber);
                    double dblAllocated = oResourceRequest.GetDetailsHoursUsed(intRequest, intItem, intNumber, intResourceWorkflow, false);
                    oResourceRequest.UpdateWorkflowAllocated(intResourceWorkflow, dblAllocated);
                }
            }
            SaveStorageBackup();
            oResourceRequest.UpdateWorkflowName(intResourceWorkflow, txtCustom.Text, intProfile);
            Response.Redirect(Request.Path + "?rrid=" + intResourceWorkflow.ToString() + "&div=E&save=true");
        }
        protected void btnComplete_Click(Object Sender, EventArgs e)
        {
            int intResourceWorkflow = Int32.Parse(lblResourceWorkflow.Text);
            int intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);
            if (panNoSlider.Visible == true)
            {
                double dblAllocated = double.Parse(oResourceRequest.GetWorkflow(intResourceWorkflow, "allocated"));
                oResourceRequest.UpdateWorkflowHoursOverwrite(intResourceWorkflow, dblAllocated);
            }
            // Add a green / completed status if there are no updates, OR the last status is not green
            DataSet dsStatus = oResourceRequest.GetStatuss(intResourceWorkflow);
            if (dsStatus.Tables[0].Rows.Count == 0 || dsStatus.Tables[0].Rows[0]["status"].ToString() != "3")
                oResourceRequest.AddStatus(intResourceWorkflow, 3, "Completed", intProfile);
            SaveStorageBackup();
            oResourceRequest.UpdateWorkflowStatus(intResourceWorkflow, 3, true);
            oResourceRequest.CloseWorkflowAndEMail(intResourceWorkflow, intEnvironment, 0, dsnServiceEditor, true, intResourceRequestApprove, intAssignPage, intViewPage, dsnAsset, dsnIP);
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "reload", "<script type=\"text/javascript\">RefreshOpeningWindow();window.close();<" + "/" + "script>");

            # region Verify completed task and send notification
            int intIMDecommServiceId = Int32.Parse(ConfigurationManager.AppSettings["SERVICEID_SERVER_DECOMMISSION_IM"]); 

            Asset oAsset = new Asset(0, dsnAsset, dsn);
            bool boolVerifyDecomCompletionAndNofity = false;
            // Running = 2 will cancel any future notifications / workflow, etc....  Used for fixing decom errors
            DataSet dsDecommission = oAsset.GetDecommission(intRequest, intNumber, 2);
            if (dsDecommission.Tables[0].Rows.Count > 0)
                boolVerifyDecomCompletionAndNofity = true;
            if (boolVerifyDecomCompletionAndNofity == true)
            {
                // Running flag is < 2, so continue processing
                ServerDecommission oServerDecommission = new ServerDecommission(intProfile, dsn);
                oServerDecommission.VerifyDecomCompletionAndNofity(intRequest, intItem, intNumber,
                                    intAssignPage, intViewPage, intEnvironment,
                                    intIMDecommServiceId,
                                    dsnServiceEditor, dsnAsset, dsnIP);
            }
            #endregion
        }
        protected void SaveStorageBackup()
        {
            // Update Reclaim if applicable
            double dblStorageAmt = 0;
            double.TryParse(lblStorageAmt.Text, out dblStorageAmt);
            int intStorageTier = 0;
            Int32.TryParse(lblStorageTier.Text, out intStorageTier);
            double dblReclaimed = 0;
            if (intServiceStorage == intService)
            {
                double.TryParse(txtStorage.Text, out dblReclaimed);
                if (intServer > 0)
                    oServer.UpdateReclaimStorage(intServer, dblReclaimed, dblStorageAmt, intStorageTier, lblStorageEnvironment.Text, txtPreCooldown.Text, txtCooldown.Text, txtCR2.Text, (radClassificationM.Checked ? "M" : (radClassificationL.Checked ? "L" : "")), (radClassificationM.Checked && radVendorE.Checked ? "E" : (radClassificationM.Checked && radVendorH.Checked ? "H" : "")), (radClassificationM.Checked ? Int32.Parse(ddlLocation.SelectedItem.Value) : 0), (radClassificationM.Checked ? txtSerial.Text : ""), txtNotes.Text);
                oCustomized.UpdateDecommissionServerReclaimStorage(intRequest, intItem, intNumber, dblReclaimed, dblStorageAmt, intStorageTier, lblStorageEnvironment.Text, txtPreCooldown.Text, txtCooldown.Text, txtCR2.Text, (radClassificationM.Checked ? "M" : (radClassificationL.Checked ? "L" : "")), (radClassificationM.Checked && radVendorE.Checked ? "E" : (radClassificationM.Checked && radVendorH.Checked ? "H" : "")), (radClassificationM.Checked ? Int32.Parse(ddlLocation.SelectedItem.Value) : 0), (radClassificationM.Checked ? txtSerial.Text : ""), txtNotes.Text);
            }
            if (intServiceBackup == intService)
            {
                double.TryParse(txtBackup.Text, out dblReclaimed);
                if (intServer > 0)
                    oServer.UpdateReclaimBackup(intServer, dblReclaimed);
                oCustomized.UpdateDecommissionServerReclaimBackup(intRequest, intItem, intNumber, dblReclaimed);
            }
        }
    }
}