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
    public partial class wm_vmware : System.Web.UI.UserControl
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        private string dsnZeus = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ZeusDSN"]].ConnectionString;
        private string dsnServiceEditor = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ServiceEditorDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected string strVirtual = ConfigurationManager.AppSettings["VirtualGatekeeper"];
        protected int intAssignPage = Int32.Parse(ConfigurationManager.AppSettings["AssignWorkload"]);
        protected int intViewPage = Int32.Parse(ConfigurationManager.AppSettings["ViewWorkload"]);
        protected int intResourceRequestApprove = Int32.Parse(ConfigurationManager.AppSettings["ResourceRequestApprove"]);
        protected int intMyWork = Int32.Parse(ConfigurationManager.AppSettings["MyWork"]);
        protected int intImplementorDistributed = Int32.Parse(ConfigurationManager.AppSettings["ITEMID_IMPLEMENTOR_DISTRIBUTED"]);
        protected int intImplementorMidrange = Int32.Parse(ConfigurationManager.AppSettings["ITEMID_IMPLEMENTOR_MIDRANGE"]);
        protected int intStorageItem = Int32.Parse(ConfigurationManager.AppSettings["ITEMID_STORAGE"]);
        protected int intBackupItem = Int32.Parse(ConfigurationManager.AppSettings["ITEMID_BACKUP"]);
        protected int intStorageService = Int32.Parse(ConfigurationManager.AppSettings["SERVICEID_STORAGE"]);
        protected int intBackupService = Int32.Parse(ConfigurationManager.AppSettings["SERVICEID_BACKUP"]);
        protected bool boolUsePNCNaming = (ConfigurationManager.AppSettings["USE_PNC_NAMING"] == "1");
        protected int intProfile;
        protected Projects oProject;
        protected Functions oFunction;
        protected Users oUser;
        protected Pages oPage;
        protected ResourceRequest oResourceRequest;
        protected RequestItems oRequestItem;
        protected Requests oRequest;
        protected Services oService;
        protected RequestFields oRequestField;
        protected Applications oApplication;
        protected OnDemandTasks oOnDemandTasks;
        protected Delegates oDelegate;
        protected Classes oClass;
        protected Servers oServer;
        protected int intApplication = 0;
        protected int intPage = 0;
        protected int intProject = 0;
        protected bool boolDetails = false;
        protected bool boolExecution = false;
        protected int intRequest = 0;
        protected int intItem = 0;
        protected int intNumber = 0;
        protected ServiceRequests oServiceRequest;
        protected int intWorkloadPage = Int32.Parse(ConfigurationManager.AppSettings["ViewWorkload"]);
        protected bool boolMove = false;
        private double dbl1 = 0.50;
       
        private double dbl3 = 0.25;
        private double dbl4 = 3.00;
        private double dbl5 = 0.50;
        private double dbl6 = 0.25;
        private double dbl7 = 0.00;
        private double dbl8 = 1.25;
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
            oRequestField = new RequestFields(intProfile, dsn);
            oServiceRequest = new ServiceRequests(intProfile, dsn);
            oApplication = new Applications(intProfile, dsn);
            oOnDemandTasks = new OnDemandTasks(intProfile, dsn);
            oDelegate = new Delegates(intProfile, dsn);
            oClass = new Classes(intProfile, dsn);
            oServer = new Servers(0, dsn);
            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            if (Request.QueryString["rrid"] != null && Request.QueryString["rrid"] != "")
            {
                lblResourceWorkflow.Text = Request.QueryString["rrid"];
                int intResourceWorkflow = Int32.Parse(Request.QueryString["rrid"]);
                int intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);
                DataSet ds = oResourceRequest.Get(intResourceParent);
                intRequest = Int32.Parse(ds.Tables[0].Rows[0]["requestid"].ToString());
                intItem = Int32.Parse(ds.Tables[0].Rows[0]["itemid"].ToString());
                intNumber = Int32.Parse(ds.Tables[0].Rows[0]["number"].ToString());
                // Workflow start
                bool boolComplete = (oResourceRequest.GetWorkflow(intResourceWorkflow, "status") == "3");
                int intUser = Int32.Parse(oResourceRequest.GetWorkflow(intResourceWorkflow, "userid"));
                txtCustom.Text = oResourceRequest.GetWorkflow(intResourceWorkflow, "name");
                double dblAllocated = double.Parse(oResourceRequest.GetWorkflow(intResourceWorkflow, "allocated"));
                // Workflow end
                int intService = Int32.Parse(ds.Tables[0].Rows[0]["serviceid"].ToString());
                lblService.Text = oService.Get(intService, "name");
                int intApp = oRequestItem.GetItemApplication(intItem);

                double dblUsed = oResourceRequest.GetWorkflowUsed(intResourceWorkflow);
                lblUpdated.Text = oResourceRequest.GetUpdated(intResourceParent);
                dblUsed = (dblUsed / dblAllocated) * 100;
                if (Request.QueryString["save"] != null && Request.QueryString["save"] != "")
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "saved", "<script type=\"text/javascript\">alert('Information Saved Successfully');<" + "/" + "script>");
                if (Request.QueryString["status"] != null && Request.QueryString["status"] != "")
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "statusd", "<script type=\"text/javascript\">alert('Weekly Status has been Added');<" + "/" + "script>");
                intProject = oRequest.GetProjectNumber(intRequest);
                hdnTab.Value = "D";
                panWorkload.Visible = true;
                LoadStatus(intResourceWorkflow);
                bool boolDone = LoadInformation(intResourceWorkflow);
                if (boolDone == true)
                {
                    if (boolComplete == false)
                    {
                        oFunction.ConfigureToolButton(btnComplete, "/images/tool_complete");
                        btnComplete.Attributes.Add("onclick", "return ValidateDropDown('" + ddlSuccess.ClientID + "','Please select a status') && confirm('Are you sure you want to mark this as completed and remove it from your workload?');");
                    }
                    else
                    {
                        oFunction.ConfigureToolButton(btnComplete, "/images/tool_complete");
                        btnComplete.Attributes.Add("onclick", "alert('This task has already been marked COMPLETE. You can close this window.');return false;");
                    }
                    imgSuccess.ImageUrl = "/images/green_arrow.gif";
                    btnSave.ImageUrl = "/images/tool_save.gif";
                    btnSave.Enabled = false;
                }
                else
                {
                    btnComplete.ImageUrl = "/images/tool_complete_dbl.gif";
                    btnComplete.Enabled = false;
                    oFunction.ConfigureToolButton(btnSave, "/images/tool_save");
                }

                btnDenied.Attributes.Add("onclick", "return CloseWindow();");
                oFunction.ConfigureToolButton(btnSave, "/images/tool_save");
                oFunction.ConfigureToolButton(btnPrint, "/images/tool_print");
                btnPrint.Attributes.Add("onclick", "return PrintWindow();");
                oFunction.ConfigureToolButton(btnClose, "/images/tool_close");
                btnClose.Attributes.Add("onclick", "return ExitWindow();");
                btnSave.Attributes.Add("onclick", "return ValidateStatus('" + ddlStatus.ClientID + "','" + txtIssues.ClientID + "');");
                // 6/1/2009 - Load ReadOnly View
                if (oResourceRequest.CanUpdate(intProfile, intResourceWorkflow, false) == false)
                {
                    oFunction.ConfigureToolButtonRO(btnSave, btnComplete);
                    //panDenied.Visible = true;
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
        private bool LoadInformation(int _request_workflow)
        {
            if (intProject > 0)
            {
                lblName.Text = oProject.Get(intProject, "name");
                lblNumber.Text = oProject.Get(intProject, "number");
                lblType.Text = "Project";
            }
            else
            {
                lblName.Text = oResourceRequest.GetWorkflow(_request_workflow, "name");
                lblNumber.Text = "CVT" + intRequest.ToString();
                lblType.Text = "Task";
            }
            bool boolDone = false;
            DataSet ds = oOnDemandTasks.GetVMWareII(intRequest, intItem, intNumber);
            if (ds.Tables[0].Rows.Count > 0)
            {
                Forecast oForecast = new Forecast(intProfile, dsn);
                int intAnswer = Int32.Parse(ds.Tables[0].Rows[0]["answerid"].ToString());
                if (oForecast.IsHARoom(intAnswer) == true)
                {
                    DataSet dsServer = oServer.GetAnswer(intAnswer);
                    foreach (DataRow drServer in dsServer.Tables[0].Rows)
                    {
                        int intHA = Int32.Parse(drServer["id"].ToString());
                        DataSet dsHA = oServer.GetHA(intHA);
                        if (dsHA.Tables[0].Rows.Count > 0)
                        {
                            if (lblHA.Text != "")
                                lblHA.Text += "<br/>";
                            lblHA.Text += oServer.GetName(intHA, true) + " = " + oServer.GetName(Int32.Parse(dsHA.Tables[0].Rows[0]["serverid_ha"].ToString()), true);
                            panHA.Visible = true;
                        }
                    }
                }
                lblAnswer.Text = intAnswer.ToString();
                btnView.Attributes.Add("onclick", "return OpenWindow('FORECAST_EQUIPMENT','?id=" + intAnswer.ToString() + "');");
                btnBirth.Attributes.Add("onclick", "return OpenWindow('PDF_BIRTH','?id=" + intAnswer.ToString() + "');");
                btnSC.Attributes.Add("onclick", "return OpenWindow('PDF_BIRTH','?id=" + intAnswer.ToString() + "');");
                lblView.Text = oOnDemandTasks.GetBody(intAnswer, intImplementorDistributed, intImplementorMidrange);
                int intModel = Int32.Parse(ds.Tables[0].Rows[0]["modelid"].ToString());
                ModelsProperties oModelsProperties = new ModelsProperties(intProfile, dsn);
                intModel = Int32.Parse(oModelsProperties.Get(intModel, "modelid"));
                Models oModel = new Models(intProfile, dsn);
                int intType = Int32.Parse(oModel.Get(intModel, "typeid"));
                Types oType = new Types(intProfile, dsn);
                string strExecute = oType.Get(intType, "forecast_execution_path");
                if (strExecute != "")
                    btnExecute.Attributes.Add("onclick", "return OpenWindow('FORECAST_EXECUTE','" + strExecute + "?id=" + intAnswer.ToString() + "');");
                chk1.Checked = (ds.Tables[0].Rows[0]["chk1"].ToString() == "1");
                chk3.Checked = (ds.Tables[0].Rows[0]["chk3"].ToString() == "1");
                chk4.Checked = (ds.Tables[0].Rows[0]["chk4"].ToString() == "1");
                chk5.Checked = (ds.Tables[0].Rows[0]["chk5"].ToString() == "1");
                chk6.Checked = (ds.Tables[0].Rows[0]["chk6"].ToString() == "1");
                chk7.Checked = (ds.Tables[0].Rows[0]["chk7"].ToString() == "1");
                chk8.Checked = (ds.Tables[0].Rows[0]["chk8"].ToString() == "1");
                chk9.Checked = (ds.Tables[0].Rows[0]["chk9"].ToString() == "1");
                chk10.Checked = (ds.Tables[0].Rows[0]["chk10"].ToString() == "1");
                chk11.Checked = (ds.Tables[0].Rows[0]["chk11"].ToString() == "1");
                int intNotifications = Int32.Parse(ds.Tables[0].Rows[0]["notifications"].ToString());
                int intClass = Int32.Parse(oForecast.GetAnswer(intAnswer, "classid"));
               
                if (oClass.Get(intClass, "prod") != "1")
                {
                  
                    chk9.Checked = true;
                    img9.ImageUrl = "/images/cancel.gif";
                    chk10.Checked = true;
                    img10.ImageUrl = "/images/cancel.gif";
                    chk11.Checked = true;
                    img11.ImageUrl = "/images/cancel.gif";
                }
                boolDone = (chk1.Checked && chk3.Checked && chk4.Checked && chk5.Checked && chk6.Checked && chk7.Checked && chk8.Checked && chk9.Checked && chk10.Checked && chk11.Checked);
              
                img1.ImageUrl = "/images/green_arrow.gif";
                if (chk1.Checked == true)
                {
                    btnExecute.Enabled = false;
                    img1.ImageUrl = "/images/check.gif";
                    img3.ImageUrl = "/images/green_arrow.gif";
                    chk3.Enabled = true;
                    Servers oServer = new Servers(intProfile, dsn);
                    DataSet dsAnswer = oServer.GetAnswer(intAnswer);
                    foreach (DataRow drAnswer in dsAnswer.Tables[0].Rows)
                    {
                        if (lbl1.Text != "")
                            lbl1.Text += "<br/>";
                        int intServer = Int32.Parse(drAnswer["id"].ToString());
                        int intServerName = Int32.Parse(drAnswer["nameid"].ToString());
                        ServerName oServerName = new ServerName(0, dsn);
                        string strName = oServer.GetName(intServer, boolUsePNCNaming);
                        lbl1.Text += "Server Name: " + strName + "<br/>";
                        Zeus oZeus = new Zeus(intProfile, dsnZeus);
                        DataSet dsZeus = oZeus.GetBuildServer(intServer);
                        if (dsZeus.Tables[0].Rows.Count > 0)
                        {
                            lbl1.Text += "Serial Number: " + dsZeus.Tables[0].Rows[0]["serial"].ToString() + "<br/>";
                            lbl1.Text += "Asset Tag: " + dsZeus.Tables[0].Rows[0]["asset"].ToString() + "<br/>";
                        }
                        int intDomain = Int32.Parse(drAnswer["domainid"].ToString());
                        Domains oDomain = new Domains(intProfile, dsn);
                        boolMove = (oDomain.Get(intDomain, "move") == "1");
                        if (boolMove == true)
                            lbl1.Text += "DHCP Address: " + drAnswer["dhcp"].ToString() + "<br/>";
                        int intIPAddress = 0;
                        IPAddresses oIPAddresses = new IPAddresses(0, dsnIP, dsn);
                        lbl1.Text += "Assigned IP Address: " + oServer.GetIPs(intServer, 1, 0, 0, dsnIP, "", "") + "<br/>";
                        lbl1.Text += "Final IP Address: " + oServer.GetIPs(intServer, 0, 1, 0, dsnIP, "", "") + "<br/>";
                        Asset oAsset = new Asset(intProfile, dsnAsset);
                        VMWare oVMWare = new VMWare(intProfile, dsn);
                        int intAsset = 0;
                        if (drAnswer["assetid"].ToString() != "")
                            intAsset = Int32.Parse(drAnswer["assetid"].ToString());
                        if (intAsset > 0)
                        {
                            DataSet dsGuest = oVMWare.GetGuest(strName);
                            if (dsGuest.Tables[0].Rows.Count > 0)
                            {
                                int intHost = Int32.Parse(dsGuest.Tables[0].Rows[0]["hostid"].ToString());
                                int intCluster = Int32.Parse(oVMWare.GetHost(intHost, "clusterid"));
                                int intFolder = Int32.Parse(oVMWare.GetCluster(intCluster, "folderid"));
                                int intDataCenter = Int32.Parse(oVMWare.GetFolder(intFolder, "datacenterid"));
                                lbl1.Text += "DataCenter: " + oVMWare.GetDatacenter(intDataCenter, "name") + "<br/>";
                                lbl1.Text += "Host: " + oVMWare.GetHost(intHost, "name") + "<br/>";
                                int intDatastore = Int32.Parse(dsGuest.Tables[0].Rows[0]["datastoreid"].ToString());
                                lbl1.Text += "DataStore: " + oVMWare.GetDatastore(intDatastore, "name") + "<br/>";
                                int intPool = Int32.Parse(dsGuest.Tables[0].Rows[0]["poolid"].ToString());
                                //lbl1.Text += "Pool: " + oVMWare.GetPool(intPool, "name") + "<br/>";
                                int intVlan = Int32.Parse(dsGuest.Tables[0].Rows[0]["vlanid"].ToString());
                                lbl1.Text += "VLAN: " + oVMWare.GetVlan(intVlan, "name") + "<br/>";
                            }
                        }
                        Storage oStorage = new Storage(intProfile, dsn);
                        int intCluster2 = Int32.Parse(drAnswer["clusterid"].ToString());
                        int intCSMConfig2 = Int32.Parse(drAnswer["csmconfigid"].ToString());
                        int intNumber2 = Int32.Parse(drAnswer["number"].ToString());
                        DataSet dsLuns = oStorage.GetLuns(intAnswer, 0, intCluster2, intCSMConfig2, intNumber2);
                        StringBuilder sbStorage = new StringBuilder();
                        int intRow = 0;
                        bool boolOverride = (oForecast.GetAnswer(intAnswer, "storage_override") == "1");
                        foreach (DataRow drLun in dsLuns.Tables[0].Rows)
                        {
                            intRow++;
                            sbStorage.Append("<tr>");
                            sbStorage.Append("<td>");
                            sbStorage.Append(intRow.ToString());
                            sbStorage.Append("</td>");
                            string strLetter = drLun["letter"].ToString();
                            if (strLetter == "")
                            {
                                if (drLun["driveid"].ToString() == "-1000")
                                    strLetter = "E";
                                else if (drLun["driveid"].ToString() == "-100")
                                    strLetter = "F";
                                else if (drLun["driveid"].ToString() == "-10")
                                    strLetter = "P";
                                else if (drLun["driveid"].ToString() == "-1")
                                    strLetter = "Q";
                            }
                            if ((boolOverride == true && drLun["driveid"].ToString() == "0") || oForecast.IsOSMidrange(intAnswer) == true)
                            {
                                sbStorage.Append("<td>");
                                sbStorage.Append(drLun["path"].ToString());
                                sbStorage.Append("</td>");
                            }
                            else
                            {
                                sbStorage.Append("<td>");
                                sbStorage.Append(strLetter);
                                sbStorage.Append(":");
                                sbStorage.Append(drLun["path"].ToString());
                                sbStorage.Append("</td>");
                            }
                            sbStorage.Append("<td>");
                            sbStorage.Append(drLun["performance"].ToString());
                            sbStorage.Append("</td>");
                            sbStorage.Append("<td>");
                            sbStorage.Append(drLun["size"].ToString());
                            sbStorage.Append(" GB</td>");
                            sbStorage.Append("<td class=\"required\">");
                            sbStorage.Append(drLun["actual_size"].ToString() == "-1" ? "Pending" : drLun["actual_size"].ToString() + " GB");
                            sbStorage.Append("</td>");
                            sbStorage.Append("<td>");
                            sbStorage.Append(drLun["size_qa"].ToString());
                            sbStorage.Append(" GB</td>");
                            sbStorage.Append("<td class=\"required\">");
                            sbStorage.Append(drLun["actual_size_qa"].ToString() == "-1" ? "Pending" : drLun["actual_size_qa"].ToString() + " GB");
                            sbStorage.Append("</td>");
                            sbStorage.Append("<td>");
                            sbStorage.Append(drLun["size_test"].ToString());
                            sbStorage.Append(" GB</td>");
                            sbStorage.Append("<td class=\"required\">");
                            sbStorage.Append(drLun["actual_size_test"].ToString() == "-1" ? "Pending" : drLun["actual_size_test"].ToString() + " GB");
                            sbStorage.Append("</td>");
                            sbStorage.Append("<td>");
                            sbStorage.Append(drLun["replicated"].ToString() == "0" ? "No" : "Yes");
                            sbStorage.Append("</td>");
                            sbStorage.Append("<td class=\"required\">");
                            sbStorage.Append(drLun["actual_replicated"].ToString() == "-1" ? "Pending" : (drLun["actual_replicated"].ToString() == "0" ? "No" : "Yes"));
                            sbStorage.Append("</td>");
                            sbStorage.Append("<td>");
                            sbStorage.Append(drLun["high_availability"].ToString() == "0" ? "No" : "Yes (" + drLun["size"].ToString() + " GB)");
                            sbStorage.Append("</td>");
                            sbStorage.Append("<td class=\"required\">");
                            sbStorage.Append(drLun["actual_high_availability"].ToString() == "-1" ? "Pending" : (drLun["actual_high_availability"].ToString() == "0" ? "No" : "Yes (" + drLun["actual_size"].ToString() + " GB)"));
                            sbStorage.Append("</td>");
                            sbStorage.Append("</tr>");
                            DataSet dsPoints = oStorage.GetMountPoints(Int32.Parse(drLun["id"].ToString()));
                            int intPoint = 0;
                            foreach (DataRow drPoint in dsPoints.Tables[0].Rows)
                            {
                                intRow++;
                                intPoint++;
                                sbStorage.Append("<tr>");
                                sbStorage.Append("<td>");
                                sbStorage.Append(intRow.ToString());
                                sbStorage.Append("</td>");
                                if (oForecast.IsOSMidrange(intAnswer) == true)
                                {
                                    sbStorage.Append("<td>");
                                    sbStorage.Append(drPoint["path"].ToString());
                                    sbStorage.Append("</td>");
                                }
                                else
                                {
                                    sbStorage.Append("<td>");
                                    sbStorage.Append(strLetter);
                                    sbStorage.Append(":\\SH");
                                    sbStorage.Append(drLun["driveid"].ToString());
                                    sbStorage.Append("VOL");
                                    sbStorage.Append(intPoint < 10 ? "0" : "");
                                    sbStorage.Append(intPoint.ToString());
                                    sbStorage.Append("</td>");
                                }
                                sbStorage.Append("<td>");
                                sbStorage.Append(drPoint["performance"].ToString());
                                sbStorage.Append("</td>");
                                sbStorage.Append("<td>");
                                sbStorage.Append(drPoint["size"].ToString());
                                sbStorage.Append(" GB</td>");
                                sbStorage.Append("<td class=\"required\">");
                                sbStorage.Append(drPoint["actual_size"].ToString() == "-1" ? "Pending" : drPoint["actual_size"].ToString() + " GB");
                                sbStorage.Append("</td>");
                                sbStorage.Append("<td>");
                                sbStorage.Append(drPoint["size_qa"].ToString());
                                sbStorage.Append(" GB</td>");
                                sbStorage.Append("<td class=\"required\">");
                                sbStorage.Append(drPoint["actual_size_qa"].ToString() == "-1" ? "Pending" : drPoint["actual_size_qa"].ToString() + " GB");
                                sbStorage.Append("</td>");
                                sbStorage.Append("<td>");
                                sbStorage.Append(drPoint["size_test"].ToString());
                                sbStorage.Append(" GB</td>");
                                sbStorage.Append("<td class=\"required\">");
                                sbStorage.Append(drPoint["actual_size_test"].ToString() == "-1" ? "Pending" : drPoint["actual_size_test"].ToString() + " GB");
                                sbStorage.Append("</td>");
                                sbStorage.Append("<td>");
                                sbStorage.Append(drPoint["replicated"].ToString() == "0" ? "No" : "Yes");
                                sbStorage.Append("</td>");
                                sbStorage.Append("<td class=\"required\">");
                                sbStorage.Append(drPoint["actual_replicated"].ToString() == "-1" ? "Pending" : (drPoint["actual_replicated"].ToString() == "0" ? "No" : "Yes"));
                                sbStorage.Append("</td>");
                                sbStorage.Append("<td>");
                                sbStorage.Append(drPoint["high_availability"].ToString() == "0" ? "No" : "Yes (" + drPoint["size"].ToString() + " GB)");
                                sbStorage.Append("</td>");
                                sbStorage.Append("<td class=\"required\">");
                                sbStorage.Append(drPoint["actual_high_availability"].ToString() == "-1" ? "Pending" : (drPoint["actual_high_availability"].ToString() == "0" ? "No" : "Yes (" + drPoint["actual_size"].ToString() + " GB)"));
                                sbStorage.Append("</td>");
                                sbStorage.Append("</tr>");
                            }
                        }
                        if (sbStorage.ToString() != "")
                        {
                            sbStorage.Insert(0, "<tr class=\"bold\"><td></td><td>Path</td><td>Performance</td><td>Requested<br/>Size in Prod</td><td>Actual<br/>Size in Prod</td><td>Requested<br/>Size in QA</td><td>Actual<br/>Size in QA</td><td>Requested<br/>Size in Test</td><td>Actual<br/>Size in Test</td><td>Requested<br/>Replication</td><td>Actual<br/>Replication</td><td>Requested<br/>High Availability</td><td>Actual<br/>High Availability</td></tr>");
                            sbStorage.Insert(0, "<table width=\"100%\" cellpadding=\"2\" cellspacing=\"1\" border=\"0\">");
                            sbStorage.Append("</table>");
                        }
                        lbl1.Text += sbStorage.ToString();
                    }
                   
                }
                if (chk3.Checked == true)
                {
                    img3.ImageUrl = "/images/check.gif";
                    img4.ImageUrl = "/images/green_arrow.gif";
                    chk4.Enabled = true;
                }
                if (chk4.Checked == true)
                {
                    img4.ImageUrl = "/images/check.gif";
                    if (boolMove == true)
                    {
                        img5.ImageUrl = "/images/green_arrow.gif";
                        chk5.Enabled = true;
                    }
                    else
                    {
                        chk5.Checked = true;
                        img7.ImageUrl = "/images/green_arrow.gif";
                        chk7.Enabled = true;
                    }
                }
                if (chk5.Checked == true)
                {
                    if (boolMove == true)
                        img5.ImageUrl = "/images/check.gif";
                    else
                        img5.ImageUrl = "/images/cancel.gif";
                    if (boolMove == true)
                    {
                        img6.ImageUrl = "/images/green_arrow.gif";
                        chk6.Enabled = true;
                    }
                }
                if (chk6.Checked == true)
                {
                    if (boolMove == true)
                        img6.ImageUrl = "/images/check.gif";
                    else
                        img6.ImageUrl = "/images/cancel.gif";
                    if (boolMove == true)
                    {
                        img7.ImageUrl = "/images/green_arrow.gif";
                        chk7.Enabled = true;
                    }
                }
                if (chk7.Checked == true)
                {
                    img7.ImageUrl = "/images/check.gif";
                    img8.ImageUrl = "/images/green_arrow.gif";
                    chk8.Enabled = true;
                }
                if (chk8.Checked == true)
                {
                    img8.ImageUrl = "/images/check.gif";
                    img9.ImageUrl = "/images/green_arrow.gif";
                    chk9.Enabled = true;
                }
                if (chk9.Checked == true)
                {
                    img9.ImageUrl = "/images/check.gif";
                    img10.ImageUrl = "/images/green_arrow.gif";
                    chk10.Enabled = true;
                }
                if (chk10.Checked == true)
                {
                    img10.ImageUrl = "/images/check.gif";
                    img11.ImageUrl = "/images/green_arrow.gif";
                    chk11.Enabled = true;
                }
                if (chk11.Checked == true)
                {
                    img11.ImageUrl = "/images/check.gif";
                }
            }
            if (Request.QueryString["div"] != null)
            {
                switch (Request.QueryString["div"])
                {
                    case "E":
                        boolExecution = true;
                        break;
                }
            }
            if (boolDetails == false && boolExecution == false)
                boolDetails = true;
            return boolDone;
        }
        protected void btnSave_Click(Object Sender, EventArgs e)
        {
            int intResourceWorkflow = Int32.Parse(lblResourceWorkflow.Text);
            int intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);
            if (ddlStatus.SelectedIndex > -1 && txtIssues.Text.Trim() != "")
                oResourceRequest.AddStatus(intResourceWorkflow, Int32.Parse(ddlStatus.SelectedItem.Value), txtIssues.Text);
            oResourceRequest.UpdateWorkflowName(intResourceWorkflow, txtCustom.Text, intProfile);
            double dblHours = 0.00;
            dblHours += (chk1.Checked ? dbl1 : 0.00);
            dblHours += (chk3.Checked ? dbl3 : 0.00);
            dblHours += (chk4.Checked ? dbl4 : 0.00);
            dblHours += (chk5.Checked ? dbl5 : 0.00);
            dblHours += (chk6.Checked ? dbl6 : 0.00);
            dblHours += (chk7.Checked ? dbl7 : 0.00);
            dblHours += (chk8.Checked ? dbl8 : 0.00);
            double dblUsed = oResourceRequest.GetWorkflowUsed(intResourceWorkflow);
            dblHours = (dblHours - dblUsed);
            if (dblHours > 0.00)
                oResourceRequest.UpdateWorkflowHours(intResourceWorkflow, dblHours);
            oOnDemandTasks.UpdateVMWareII(intRequest, intItem, intNumber, (chk1.Checked ? 1 : 0), (chk3.Checked ? 1 : 0), (chk4.Checked ? 1 : 0), (chk5.Checked || boolMove == false ? 1 : 0), (chk6.Checked || boolMove == false ? 1 : 0), (chk7.Checked ? 1 : 0), (chk8.Checked ? 1 : 0), (chk9.Checked ? 1 : 0), (chk10.Checked ? 1 : 0), (chk11.Checked ? 1 : 0));
            Response.Redirect(Request.Path + "?rrid=" + intResourceWorkflow.ToString() + "&div=E&save=true");
        }
        protected void btnComplete_Click(Object Sender, EventArgs e)
        {
            int intResourceWorkflow = Int32.Parse(lblResourceWorkflow.Text);
            int intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);
            oResourceRequest.UpdateWorkflowStatus(intResourceWorkflow, 3, true);
            oOnDemandTasks.AddSuccess(intResourceParent, "VMware", Int32.Parse(ddlSuccess.SelectedItem.Value), txtComments.Text);
            oOnDemandTasks.UpdateVMWareIIComplete(intRequest, intItem, intNumber);
            Forecast oForecast = new Forecast(intProfile, dsn);
            oForecast.UpdateAnswerFinished(Int32.Parse(lblAnswer.Text));
            oResourceRequest.CloseWorkflow(intResourceWorkflow, intEnvironment, 0, dsnServiceEditor, true, intResourceRequestApprove, intAssignPage, intViewPage, dsnAsset, dsnIP);
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "reload", "<script type=\"text/javascript\">RefreshOpeningWindow();window.close();<" + "/" + "script>");
        }
    }
}