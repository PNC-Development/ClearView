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
    public partial class wm_server_decommission : System.Web.UI.UserControl
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
        protected int intNumber = 0;
        protected bool boolJoined = false;
        protected Servers oServer;
        protected Customized oCustomized;
        protected Classes oClass;
        protected Asset oAsset;
        protected ModelsProperties oModelsProperties;
        protected Forecast oForecast;
        protected IPAddresses oIPAddress;
        protected Variables oVariables;

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
            oClass = new Classes(intProfile, dsn);
            oAsset = new Asset(intProfile, dsnAsset);
            oModelsProperties = new ModelsProperties(intProfile, dsn);
            oForecast = new Forecast(intProfile, dsn);
            oIPAddress = new IPAddresses(intProfile, dsnIP, dsn);
            oVariables = new Variables(intEnvironment);
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

                int intService = Int32.Parse(ds.Tables[0].Rows[0]["serviceid"].ToString());
                lblService.Text = oService.Get(intService, "name");
                int intApp = oRequestItem.GetItemApplication(intItem);

                if (Request.QueryString["save"] != null && Request.QueryString["save"] != "")
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "saved", "<script type=\"text/javascript\">alert('Information Saved Successfully');<" + "/" + "script>");

                if (Request.QueryString["status"] != null && Request.QueryString["status"] != "")
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "statusd", "<script type=\"text/javascript\">alert('Status Updates has been Added');<" + "/" + "script>");

                if (!IsPostBack)
                {
                    double dblUsed = oResourceRequest.GetWorkflowUsed(intResourceWorkflow);
                    lblUpdated.Text = oResourceRequest.GetUpdated(intResourceParent);
                    // dblAllocated = dblUsed;




                    if (dblAllocated == dblUsed)
                    {
                        if (boolComplete == false)
                        {
                            oFunction.ConfigureToolButton(btnComplete, "/images/tool_complete");

                            //btnComplete.Attributes.Add("onclick", "return confirm('Are you sure you want to mark this as completed and remove it from your workload?');");


                            btnComplete.Attributes.Add("onclick", "return ValidateNumber('" + txtIPBuild1.ClientID + "','Please enter a valid IP address')" +
                            " && ValidateNumber('" + txtIPBuild2.ClientID + "','Please enter a valid IP address')" +
                            " && ValidateNumber('" + txtIPBuild3.ClientID + "','Please enter a valid IP address')" +
                            " && ValidateNumber('" + txtIPBuild4.ClientID + "','Please enter a valid IP address')" +
                            " && ValidateNumber('" + txtIPFinal1.ClientID + "','Please enter a valid IP address')" +
                            " && ValidateNumber('" + txtIPFinal2.ClientID + "','Please enter a valid IP address')" +
                            " && ValidateNumber('" + txtIPFinal3.ClientID + "','Please enter a valid IP address')" +
                            " && ValidateNumber('" + txtIPFinal4.ClientID + "','Please enter a valid IP address')" +
                            " && ValidateRadioButtons('" + radSANYes.ClientID + "','" + radSANNo.ClientID + "','Please select the option for - Is this device attached to SAN?')" +
                            " && ValidateRadioButtons('" + radCSMYes.ClientID + "','" + radCSMNo.ClientID + "','Please select the option for - Is this device load balanced via CSM?')" +
                            " && confirm('Are you sure you want to mark this as completed and remove it from your workload?')" +
                            ";");

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
                    lblDocuments.Text = oDocument.GetDocuments_Service(intRequest, intService, oVariables.DocumentsFolder(), 1, (Request.QueryString["doc"] != null));

                    // Load Decom
                    DataSet dsDecom = oCustomized.GetDecommissionServer(intRequest, intItem, intNumber);
                    if (dsDecom.Tables[0].Rows.Count > 0)
                    {
                        chkDecom.Checked = (dsDecom.Tables[0].Rows[0]["decommed"].ToString() == "1");
                        chkDispose.Checked = (dsDecom.Tables[0].Rows[0]["disposed"].ToString() == "1");

                        int intServer = Int32.Parse(dsDecom.Tables[0].Rows[0]["serverid"].ToString());
                        bool boolBlackout = false;
                        bool boolVMware = false;

                        int intIPAssign = 0;
                        int intIPFinal = 0;

                        DataSet dsServer = oServer.Get(intServer);
                        if (dsServer.Tables[0].Rows.Count > 0)
                        {
                            int intAnswer = Int32.Parse(dsServer.Tables[0].Rows[0]["answerid"].ToString());
                            if (intAnswer > 0)
                            {
                                if (oForecast.GetAnswer(intAnswer, "storage") == "1")
                                {
                                    if (dsDecom.Tables[0].Rows[0]["SAN"] == DBNull.Value)
                                        radSANYes.Checked = true;
                                    radSANYes.CssClass = "bold";
                                }
                                else
                                {
                                    if (dsDecom.Tables[0].Rows[0]["SAN"] == DBNull.Value)
                                        radSANNo.Checked = true;
                                    radSANNo.CssClass = "bold";
                                }

                                if (oForecast.IsHACSM(intAnswer) == true)
                                {
                                    if (dsDecom.Tables[0].Rows[0]["CSM"] == DBNull.Value)
                                        radCSMYes.Checked = true;
                                    radCSMYes.CssClass = "bold";
                                }
                                else
                                {
                                    if (dsDecom.Tables[0].Rows[0]["CSM"] == DBNull.Value)
                                        radCSMNo.Checked = true;
                                    radCSMNo.CssClass = "bold";
                                }
                            }
                            else
                            {
                                //radSANDK.Checked = true;
                                //radSANDK.CssClass = "bold";
                                //radCSMDK.Checked = true;
                                //radCSMDK.CssClass = "bold";
                            }

                            int intAsset = 0;
                            int intAssetDR = 0;
                            DataSet dsAsset = oServer.GetAssets(intServer);
                            foreach (DataRow drAsset in dsAsset.Tables[0].Rows)
                            {
                                if (drAsset["latest"].ToString() == "1")
                                    intAsset = Int32.Parse(drAsset["assetid"].ToString());
                                if (drAsset["dr"].ToString() == "1")
                                    intAssetDR = Int32.Parse(drAsset["assetid"].ToString());
                            }

                            if (intAsset > 0)
                            {
                                int intClass = Int32.Parse(oAsset.GetServerOrBlade(intAsset, "classid"));
                                int intEnv = Int32.Parse(oAsset.GetServerOrBlade(intAsset, "environmentid"));
                                int intAddress = Int32.Parse(oAsset.GetServerOrBlade(intAsset, "addressid"));
                                int intModel = 0;
                                if (oAsset.Get(intAsset, "modelid") != "")
                                    intModel = Int32.Parse(oAsset.Get(intAsset, "modelid"));
                                if (oClass.IsProd(intClass) || oClass.IsQA(intClass))
                                    boolBlackout = true;
                                if (intModel > 0 && oModelsProperties.IsTypeVMware(intModel) == true)
                                {
                                    boolVMware = true;
                                }
                            }

                            DataSet dsIP = oServer.GetIP(intServer, 0, 0, 0, 0);
                            foreach (DataRow drIP in dsIP.Tables[0].Rows)
                            {
                                if (drIP["auto_assign"].ToString() == "1")
                                    intIPAssign = Int32.Parse(drIP["ipAddressID"].ToString());
                                if (drIP["final"].ToString() == "1")
                                    intIPFinal = Int32.Parse(drIP["ipAddressID"].ToString());
                            }
                        }
                        else
                        {
                            Int32.TryParse(dsDecom.Tables[0].Rows[0]["ip_build"].ToString(), out intIPAssign);
                            Int32.TryParse(dsDecom.Tables[0].Rows[0]["ip_final"].ToString(), out intIPFinal);
                        }

                        if (dsDecom.Tables[0].Rows[0]["SAN"] != DBNull.Value)
                        {
                            if (Int32.Parse(dsDecom.Tables[0].Rows[0]["SAN"].ToString()) == 1)
                                radSANYes.Checked = true;
                            else if (Int32.Parse(dsDecom.Tables[0].Rows[0]["SAN"].ToString()) == 0)
                                radSANNo.Checked = true;
                        }
                        if (dsDecom.Tables[0].Rows[0]["CSM"] != DBNull.Value)
                        {
                            if (Int32.Parse(dsDecom.Tables[0].Rows[0]["CSM"].ToString()) == 1)
                                radCSMYes.Checked = true;
                            else if (Int32.Parse(dsDecom.Tables[0].Rows[0]["CSM"].ToString()) == 0)
                                radCSMNo.Checked = true;
                        }

                        if (intIPAssign > 0)
                        {
                            string strIP = oIPAddress.GetName(intIPAssign, 0);
                            int intIP1 = Int32.Parse(strIP.Substring(0, strIP.IndexOf(".")));
                            strIP = strIP.Substring(strIP.IndexOf(".") + 1);
                            int intIP2 = Int32.Parse(strIP.Substring(0, strIP.IndexOf(".")));
                            strIP = strIP.Substring(strIP.IndexOf(".") + 1);
                            int intIP3 = Int32.Parse(strIP.Substring(0, strIP.IndexOf(".")));
                            strIP = strIP.Substring(strIP.IndexOf(".") + 1);
                            int intIP4 = Int32.Parse(strIP);
                            txtIPBuild1.Text = intIP1.ToString();
                            txtIPBuild2.Text = intIP2.ToString();
                            txtIPBuild3.Text = intIP3.ToString();
                            txtIPBuild4.Text = intIP4.ToString();
                        }
                        else
                        {
                            txtIPBuild1.Text = "0";
                            txtIPBuild2.Text = "0";
                            txtIPBuild3.Text = "0";
                            txtIPBuild4.Text = "0";
                        }
                        if (intIPFinal > 0)
                        {
                            string strIP = oIPAddress.GetName(intIPFinal, 0);
                            int intIP1 = Int32.Parse(strIP.Substring(0, strIP.IndexOf(".")));
                            strIP = strIP.Substring(strIP.IndexOf(".") + 1);
                            int intIP2 = Int32.Parse(strIP.Substring(0, strIP.IndexOf(".")));
                            strIP = strIP.Substring(strIP.IndexOf(".") + 1);
                            int intIP3 = Int32.Parse(strIP.Substring(0, strIP.IndexOf(".")));
                            strIP = strIP.Substring(strIP.IndexOf(".") + 1);
                            int intIP4 = Int32.Parse(strIP);
                            txtIPFinal1.Text = intIP1.ToString();
                            txtIPFinal2.Text = intIP2.ToString();
                            txtIPFinal3.Text = intIP3.ToString();
                            txtIPFinal4.Text = intIP4.ToString();
                        }
                        else
                        {
                            txtIPFinal1.Text = "0";
                            txtIPFinal2.Text = "0";
                            txtIPFinal3.Text = "0";
                            txtIPFinal4.Text = "0";
                        }

                        if (boolBlackout == true)
                            panBlackout.Visible = true;
                        else
                            panBlackoutNO.Visible = true;
                        if (boolVMware == true)
                            panRename.Visible = true;
                        else
                            chkRename.Checked = true;   // Skip rename process for non-vmware builds
                        bool boolFinished = true;
                       
                        DateTime datPower = DateTime.Now;
                        chkRename.Text += "<b>" + dsDecom.Tables[0].Rows[0]["servername"].ToString() + "-DECOM</b>";
                        if (dsDecom.Tables[0].Rows[0]["blackedout"].ToString() != "")
                        {
                            chkBlackout.Enabled = false;
                            chkBlackout.Checked = true;
                            chkBlackout.Text += " (Completed on " + dsDecom.Tables[0].Rows[0]["blackedout"].ToString() + ")";
                        }
                        else if (boolBlackout == true)
                            boolFinished = false;
                        if (dsDecom.Tables[0].Rows[0]["poweredoff"].ToString() != "")
                        {
                            datPower = DateTime.Parse(dsDecom.Tables[0].Rows[0]["poweredoff"].ToString());
                            chkPower.Enabled = false;
                            chkPower.Checked = true;
                            chkPower.Text += " (Completed on " + dsDecom.Tables[0].Rows[0]["poweredoff"].ToString() + ")";
                        }
                        else
                            boolFinished = false;
                        if (dsDecom.Tables[0].Rows[0]["renamed"].ToString() != "")
                        {
                            chkRename.Enabled = false;
                            chkRename.Checked = true;
                            chkRename.Text += " (Completed on " + dsDecom.Tables[0].Rows[0]["renamed"].ToString() + ")";
                        }
                        else if (boolVMware == true)
                            boolFinished = false;

                        if (boolFinished == true)
                        {
                            // Finished with initial steps...validate that it is 7 days after power off date to continue
                            TimeSpan oSpan = DateTime.Now.Subtract(datPower);
                            if (oSpan.Days >= 7)
                            {
                                panWaitingNO.Visible = true;
                                if (boolVMware == true)
                                    panDestroy.Visible = true;
                                if (dsDecom.Tables[0].Rows[0]["destroy"].ToString() != "")
                                {
                                    chkDestroy.Enabled = false;
                                    chkDestroy.Checked = true;
                                    chkDestroy.Text += " (Completed on " + dsDecom.Tables[0].Rows[0]["destroy"].ToString() + ")";
                                }
                                else if (boolVMware == true)
                                    boolFinished = false;
                            }
                            else
                            {
                                panWaiting.Visible = true;
                                datPower = datPower.AddDays(7.00);
                                lblWaiting.Text = datPower.ToString();
                            }
                        }
                    }

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
            //Save the current status
            SaveRequest();

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

            DateTime datPower = DateTime.Now;
            // Load Decom
            DataSet dsDecom = oCustomized.GetDecommissionServer(intRequest, intItem, intNumber);
            if (dsDecom.Tables[0].Rows.Count > 0)
            {
                if (dsDecom.Tables[0].Rows[0]["poweredoff"].ToString() != "")
                    datPower = DateTime.Parse(dsDecom.Tables[0].Rows[0]["poweredoff"].ToString());

                TimeSpan oSpan = DateTime.Now.Subtract(datPower);
                if (oSpan.Days >= 7)
                {
                    double dblAllocated = double.Parse(oResourceRequest.GetWorkflow(intResourceWorkflow, "allocated"));
                    oResourceRequest.UpdateWorkflowHoursOverwrite(intResourceWorkflow, dblAllocated);
                }
            }

            //oServiceDetail.UpdateCheckboxes(Request, intResourceWorkflow, intRequest, intItem, intNumber);
            //double dblAllocated = oResourceRequest.GetDetailsHoursUsed(intRequest, intItem, intNumber, (boolJoined ? 0 : intResourceWorkflow), false);
            //oResourceRequest.UpdateWorkflowAllocated(intResourceWorkflow, dblAllocated);
            oResourceRequest.UpdateWorkflowName(intResourceWorkflow, txtCustom.Text, intProfile);
            Response.Redirect(Request.Path + "?rrid=" + intResourceWorkflow.ToString() + "&div=E&save=true");
        }

        private void SaveRequest()
        {
            int intServer;
            int intBuildIPaddressId;
            int intFinalIPaddressId;
            int? intSAN = null;
            int? intCSM = null;
            int intIPAssign = 0;
            int intIPFinal = 0;
            bool boolIPAddressAssignChanged = false;
            bool boolIPAddressFinalChanged = false;
            // Save current State
            DataSet dsDecom = oCustomized.GetDecommissionServer(intRequest, intItem, intNumber);
            intServer = Int32.Parse(dsDecom.Tables[0].Rows[0]["serverid"].ToString());
            oServer.UpdateDecommissioned(intServer, DateTime.Now.ToString());
            string strName = dsDecom.Tables[0].Rows[0]["servername"].ToString();
            if (chkRename.Checked == true && chkPower.Checked == true)
            {
                // Add device to automated decom process (already established) to power off and rename device
                if (dsDecom.Tables[0].Rows.Count > 0)
                {
                    DataSet dsAssets = oServer.GetAssets(intServer);
                    foreach (DataRow drAsset in dsAssets.Tables[0].Rows)
                    {
                        if (drAsset["latest"].ToString() == "1" || drAsset["dr"].ToString() == "1")
                        {
                            int intAsset = Int32.Parse(drAsset["assetid"].ToString());
                            bool boolUnique = oAsset.AddDecommission(intRequest, intItem, intNumber, intAsset, intProfile, dsDecom.Tables[0].Rows[0]["reason"].ToString(), DateTime.Now, strName + (drAsset["dr"].ToString() == "1" ? "-DR" : ""), (drAsset["dr"].ToString() == "1" ? 1 : 0), "");
                            oServer.UpdateAssetDecom(intServer, intAsset, DateTime.Now.ToString());
                        }
                    }
                }
            }
            if (panWaitingNO.Visible == false)
            {
                intSAN = null;
                intCSM = null;
                if (chkPower.Enabled == true)
                    oCustomized.UpdateDecommissionServer(intRequest, intItem, intNumber, (chkPower.Checked ? DateTime.Now.ToString() : ""), (chkBlackout.Checked ? DateTime.Now.ToString() : ""), (chkRename.Checked ? DateTime.Now.ToString() : ""), (chkDestroy.Checked ? DateTime.Now.ToString() : ""), (chkDecom.Checked ? 1 : 0), (chkDispose.Checked ? 1 : 0), "", intSAN, intCSM, null, null);
                else
                    oCustomized.UpdateDecommissionServer(intRequest, intItem, intNumber, "", (chkBlackout.Checked ? DateTime.Now.ToString() : ""), (chkRename.Checked ? DateTime.Now.ToString() : ""), (chkDestroy.Checked ? DateTime.Now.ToString() : ""), (chkDecom.Checked ? 1 : 0), (chkDispose.Checked ? 1 : 0), "", intSAN, intCSM, null, null);
            }
            else
            {
                if (radSANYes.Checked == true)
                    intSAN = 1;
                if (radSANNo.Checked == true)
                    intSAN = 0;

                if (radCSMYes.Checked == true)
                    intCSM = 1;
                if (radCSMNo.Checked == true)
                    intCSM = 0;

                IPAddresses oIPAddresses;
                oIPAddresses = new IPAddresses(intProfile, dsnIP, dsn);

                //Validate if IP Address is over written
                if (intServer > 0)
                {
                    DataSet dsIP = oServer.GetIP(intServer, 0, 0, 0, 0);
                    foreach (DataRow drIP in dsIP.Tables[0].Rows)
                    {
                        if (drIP["auto_assign"].ToString() == "1")
                            intIPAssign = Int32.Parse(drIP["ipAddressID"].ToString());
                        if (drIP["final"].ToString() == "1")
                            intIPFinal = Int32.Parse(drIP["ipAddressID"].ToString());
                    }
                }
                else
                {
                    Int32.TryParse(dsDecom.Tables[0].Rows[0]["ip_build"].ToString(), out intIPAssign);
                    Int32.TryParse(dsDecom.Tables[0].Rows[0]["ip_final"].ToString(), out intIPFinal);
                }
                if (intIPAssign > 0)
                {
                    string strIP = oIPAddress.GetName(intIPAssign, 0);
                    int intIP1 = Int32.Parse(strIP.Substring(0, strIP.IndexOf(".")));
                    strIP = strIP.Substring(strIP.IndexOf(".") + 1);
                    int intIP2 = Int32.Parse(strIP.Substring(0, strIP.IndexOf(".")));
                    strIP = strIP.Substring(strIP.IndexOf(".") + 1);
                    int intIP3 = Int32.Parse(strIP.Substring(0, strIP.IndexOf(".")));
                    strIP = strIP.Substring(strIP.IndexOf(".") + 1);
                    int intIP4 = Int32.Parse(strIP);

                    if (txtIPBuild1.Text.Trim() != intIP1.ToString() || txtIPBuild2.Text.Trim() != intIP2.ToString() ||
                        txtIPBuild3.Text.Trim() != intIP3.ToString() || txtIPBuild4.Text.Trim() != intIP4.ToString())
                        boolIPAddressAssignChanged = true;
                }
                else if ((txtIPBuild1.Text.Trim() != "0" || txtIPBuild2.Text.Trim() != "0" ||
                          txtIPBuild3.Text.Trim() != "0" || txtIPBuild4.Text.Trim() != "0") && (intIPAssign == 0))
                    boolIPAddressAssignChanged = true;


                if (intIPFinal > 0)
                {
                    string strIP = oIPAddress.GetName(intIPFinal, 0);
                    int intIP1 = Int32.Parse(strIP.Substring(0, strIP.IndexOf(".")));
                    strIP = strIP.Substring(strIP.IndexOf(".") + 1);
                    int intIP2 = Int32.Parse(strIP.Substring(0, strIP.IndexOf(".")));
                    strIP = strIP.Substring(strIP.IndexOf(".") + 1);
                    int intIP3 = Int32.Parse(strIP.Substring(0, strIP.IndexOf(".")));
                    strIP = strIP.Substring(strIP.IndexOf(".") + 1);
                    int intIP4 = Int32.Parse(strIP);
                    if (txtIPFinal1.Text.Trim() != intIP1.ToString() || txtIPFinal2.Text.Trim() != intIP2.ToString() ||
                        txtIPFinal3.Text.Trim() != intIP3.ToString() || txtIPFinal4.Text.Trim() != intIP4.ToString())
                        boolIPAddressFinalChanged = true;

                }
                else if ((txtIPFinal1.Text.Trim() != "0" || txtIPFinal2.Text.Trim() != "0" ||
                           txtIPFinal3.Text.Trim() != "0" || txtIPFinal4.Text.Trim() != "0") && (intIPFinal == 0))
                    boolIPAddressFinalChanged = true;


                if (boolIPAddressAssignChanged == true)  //Assigned IP Address changed by User
                {
                    if (intIPAssign > 0 && intServer > 0)//Previous Assigned IP Address exist - Delete the same
                        oServer.DeleteServerIP(intServer, intIPAssign, dsnIP);

                    //Add new assigned IP Address
                    int intIPBuild1 = 0;
                    Int32.TryParse(txtIPBuild1.Text, out intIPBuild1);
                    int intIPBuild2 = 0;
                    Int32.TryParse(txtIPBuild2.Text, out intIPBuild2);
                    int intIPBuild3 = 0;
                    Int32.TryParse(txtIPBuild3.Text, out intIPBuild3);
                    int intIPBuild4 = 0;
                    Int32.TryParse(txtIPBuild4.Text, out intIPBuild4);
                    if (intIPBuild1 > 0 && intIPBuild2 > 0 && intIPBuild3 > 0 && intIPBuild4 > 0)
                    {
                        intBuildIPaddressId = oIPAddresses.Add(0, intIPBuild1, intIPBuild2, intIPBuild3, intIPBuild4, intProfile);
                        if (intServer > 0)
                            oServer.AddIP(intServer, intBuildIPaddressId, 1, 0, 0, 0);
                        else
                            intIPAssign = intBuildIPaddressId;
                    }
                }

                if (boolIPAddressFinalChanged == true)//Final IP Address changed by User
                {
                    if (intIPFinal > 0 && intServer > 0)  //Previous Final IP Address exist - Delete the same 
                        oServer.DeleteServerIP(intServer, intIPFinal, dsnIP);

                    //Add new Final IP Address
                    int intIPFinal1 = 0;
                    Int32.TryParse(txtIPFinal1.Text, out intIPFinal1);
                    int intIPFinal2 = 0;
                    Int32.TryParse(txtIPFinal2.Text, out intIPFinal2);
                    int intIPFinal3 = 0;
                    Int32.TryParse(txtIPFinal3.Text, out intIPFinal3);
                    int intIPFinal4 = 0;
                    Int32.TryParse(txtIPFinal4.Text, out intIPFinal4);
                    if (intIPFinal1 > 0 && intIPFinal2 > 0 && intIPFinal3 > 0 && intIPFinal4 > 0)
                    {
                        intFinalIPaddressId = oIPAddresses.Add(0, intIPFinal1, intIPFinal2, intIPFinal3, intIPFinal4, intProfile);
                        if (intServer > 0)
                            oServer.AddIP(intServer, intFinalIPaddressId, 0, 1, 0, 0);
                        else
                            intIPFinal = intFinalIPaddressId;
                    }
                }

                oCustomized.UpdateDecommissionServer(intRequest, intItem, intNumber, "", (chkBlackout.Checked ? DateTime.Now.ToString() : ""), (chkRename.Checked ? DateTime.Now.ToString() : ""), (chkDestroy.Checked ? DateTime.Now.ToString() : ""), (chkDecom.Checked ? 1 : 0), (chkDispose.Checked ? 1 : 0), "", intSAN, intCSM, (intServer == 0 ? intIPAssign : 0), (intServer == 0 ? intIPFinal : 0));

            }

            //Save the IP Addresses


        }
        protected void btnComplete_Click(Object Sender, EventArgs e)
        {
            //Save the current status
            SaveRequest();

            int intResourceWorkflow = Int32.Parse(lblResourceWorkflow.Text);
            int intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);
            double dblAllocated = double.Parse(oResourceRequest.GetWorkflow(intResourceWorkflow, "allocated"));

            oResourceRequest.UpdateWorkflowHoursOverwrite(intResourceWorkflow, dblAllocated);
            oResourceRequest.UpdateWorkflowStatus(intResourceWorkflow, 3, true);

            int intServer = 0;
            int intModel = 0;
            string strName = "";
            DataSet dsDecom = oCustomized.GetDecommissionServer(intRequest, intItem, intNumber);
            if (dsDecom.Tables[0].Rows.Count > 0)
            {
                Int32.TryParse(dsDecom.Tables[0].Rows[0]["serverid"].ToString(), out intServer);
                Int32.TryParse(dsDecom.Tables[0].Rows[0]["modelid"].ToString(), out intModel);
                strName = dsDecom.Tables[0].Rows[0]["servername"].ToString();
            }

            bool MissedFix = true; // so that we don't cause an outage
            DataSet dsDecommission = oAsset.GetDecommission(strName);
            if (dsDecommission.Tables[0].Rows.Count > 0)
                MissedFix = (dsDecommission.Tables[0].Rows[0]["missed_fix"].ToString() != "");

            //SendServiceCenterNotification(intRequest, intItem, intNumber, intProfile);
            int intIMDecommServiceId = Int32.Parse(ConfigurationManager.AppSettings["SERVICEID_SERVER_DECOMMISSION_IM"]);
            ServerDecommission oServerDecommission = new ServerDecommission(intProfile, dsn);
            oServerDecommission.InitiateDecom(intServer, intModel, strName, intRequest, intItem, intNumber,
                                (radSANYes.Checked == true ? 1 : 0),
                                (radCSMYes.Checked == true ? 1 : 0),
                                intAssignPage, intViewPage, intEnvironment,
                                intIMDecommServiceId,
                                dsnServiceEditor, dsnAsset, dsnIP, "", MissedFix);
       
            oResourceRequest.CloseWorkflow(intResourceWorkflow, intEnvironment, 0, dsnServiceEditor, true, intResourceRequestApprove, intAssignPage, intViewPage, dsnAsset, dsnIP);
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "reload", "<script type=\"text/javascript\">RefreshOpeningWindow();window.close();<" + "/" + "script>");
        }
        /*
        private void SendServiceCenterNotification(int intRequestId, int intItemId, int intNumberId, int intAssignedTo)
        {
            string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
            string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
            string dsnZeus = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ZeusDSN"]].ConnectionString;
            string dsnServiceEditor = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ServiceEditorDSN"]].ConnectionString;

            string strVirtual = ConfigurationManager.AppSettings["VirtualGatekeeper"];
            int intAssignPage = Int32.Parse(ConfigurationManager.AppSettings["AssignWorkload"]);
            int intViewPage = Int32.Parse(ConfigurationManager.AppSettings["ViewWorkload"]);
            int intResourceRequestApprove = Int32.Parse(ConfigurationManager.AppSettings["ResourceRequestApprove"]);
            int intMyWork = Int32.Parse(ConfigurationManager.AppSettings["MyWork"]);
            int intImplementorDistributed = Int32.Parse(ConfigurationManager.AppSettings["ITEMID_IMPLEMENTOR_DISTRIBUTED"]);
            int intImplementorMidrange = Int32.Parse(ConfigurationManager.AppSettings["ITEMID_IMPLEMENTOR_MIDRANGE"]);
            int intWorkstationPlatform = Int32.Parse(ConfigurationManager.AppSettings["WorkstationPlatformID"]);
            int intTest = Int32.Parse(ConfigurationManager.AppSettings["TestClassID"]);
            int intUnder48A = Int32.Parse(ConfigurationManager.AppSettings["DR_HOUR_RESPONSE"]);
            int intUnder48Q = Int32.Parse(ConfigurationManager.AppSettings["DR_HOUR_QUESTION"]);
            int intStorageItem = Int32.Parse(ConfigurationManager.AppSettings["ITEMID_STORAGE"]);
            int intBackupItem = Int32.Parse(ConfigurationManager.AppSettings["ITEMID_BACKUP"]);
            int intStorageService = Int32.Parse(ConfigurationManager.AppSettings["SERVICEID_STORAGE"]);
            int intBackupService = Int32.Parse(ConfigurationManager.AppSettings["SERVICEID_BACKUP"]);
            int intServerPlatformID = Int32.Parse(ConfigurationManager.AppSettings["ServerPlatformID"]);
            string strHA = ConfigurationManager.AppSettings["FORECAST_HIGH_AVAILABILITY"];
            string strServiceCenterXID = ConfigurationManager.AppSettings["ServiceCenterInputXID"];
            bool boolUsePNCNaming = (ConfigurationManager.AppSettings["USE_PNC_NAMING"] == "1");

            PDFs oPDF = new PDFs(dsn, dsnAsset, dsnIP, intEnvironment);
            oPDF.CreateServerDecommSCRequest(intRequestId, intItemId, intNumberId, intAssignedTo);

        }
        */

    }
}
