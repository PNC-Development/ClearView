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
using Microsoft.ApplicationBlocks.Data;
using System.Data.OleDb;
using System.IO;
using NCC.ClearView.Application.Core.ClearViewWS;
using System.Text;
using Tamir.SharpSsh;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Data.SqlClient;

namespace NCC.ClearView.Presentation.Web
{
    public partial class admin : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        private string dsnRemote = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["RemoteDSN"]].ConnectionString;
        private string dsnZeus = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ZeusDSN"]].ConnectionString;
        private string dsnServiceEditor = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ServiceEditorDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intModelVirtual = Int32.Parse(ConfigurationManager.AppSettings["VirtualWorkstationModelID"]);
        protected int intModelVMware = Int32.Parse(ConfigurationManager.AppSettings["VMwareWorkstationModelID"]);
        protected int intAssignPage = Int32.Parse(ConfigurationManager.AppSettings["AssignWorkload"]);
        protected int intViewPage = Int32.Parse(ConfigurationManager.AppSettings["ViewWorkload"]);
        protected int intResourceRequestApprove = Int32.Parse(ConfigurationManager.AppSettings["ResourceRequestApprove"]);
        protected int intStorageItem = Int32.Parse(ConfigurationManager.AppSettings["ITEMID_STORAGE"]);
        protected int intBackupItem = Int32.Parse(ConfigurationManager.AppSettings["ITEMID_BACKUP"]);
        protected int intStorageService = Int32.Parse(ConfigurationManager.AppSettings["SERVICEID_STORAGE"]);
        protected int intBackupService = Int32.Parse(ConfigurationManager.AppSettings["SERVICEID_BACKUP"]);
        protected int intImplementorDistributed = Int32.Parse(ConfigurationManager.AppSettings["ITEMID_IMPLEMENTOR_DISTRIBUTED"]);
        protected int intImplementorMidrange = Int32.Parse(ConfigurationManager.AppSettings["ITEMID_IMPLEMENTOR_MIDRANGE"]);
        protected int intWorkstationPlatform = Int32.Parse(ConfigurationManager.AppSettings["WorkstationPlatformID"]);
        protected int intUnder48A = Int32.Parse(ConfigurationManager.AppSettings["DR_HOUR_RESPONSE"]);
        protected int intUnder48Q = Int32.Parse(ConfigurationManager.AppSettings["DR_HOUR_QUESTION"]);
        protected int intEngineering = Int32.Parse(ConfigurationManager.AppSettings["EngineeringClassID"]);
        protected int intOrganizationDefault = Int32.Parse(ConfigurationManager.AppSettings["EPS_ORGANIZATION"]);
        protected bool boolUsePNCNaming = (ConfigurationManager.AppSettings["USE_PNC_NAMING"] == "1");
        protected string strHA = ConfigurationManager.AppSettings["FORECAST_HIGH_AVAILABILITY"];
        protected string strTIDs = ConfigurationManager.AppSettings["TEST_IDS"];
        protected string strResults = "";
        protected int intCount = 0;
        protected int intProfile = 0;
        protected bool boolSwitchDebug = (ConfigurationManager.AppSettings["SWITCHPORT_DEBUG"] == "1");
        protected bool boolSwitchEmptyMac = (ConfigurationManager.AppSettings["SWITCHPORT_EMPTY_MAC"] == "1");
        protected bool boolSwitchArpLookup = (ConfigurationManager.AppSettings["SWITCHPORT_NO_ARP_LOOKUP"] == "1");
        protected int intAssociateDesignDevice = 1;
        protected int intAssociateDesignDevices = 0;
        protected Settings oSetting;

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cookies["loginreferrer"].Value = "/admin/admin.aspx";
            Response.Cookies["loginreferrer"].Expires = DateTime.Now.AddDays(30);
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Response.Redirect("/admin/login.aspx");
            btnGo.Attributes.Add("onclick", "return confirm('This function will modify production data and cannot be reversed!\\n\\nAre you sure you want to continue?') && ProcessButton(this);");

            Variables oVariable = new Variables(intEnvironment);
            oSetting = new Settings(0, dsn);

            if (Request.QueryString["export"] != null)
            {
                HttpContext context = HttpContext.Current;
                context.Response.Clear();
                context.Response.ContentType = "text/csv";
                //context.Response.ContentType = "application/vnd.ms-excel";
                DateTime _now = DateTime.Now;
                string strNow = _now.Day.ToString() + _now.Month.ToString() + _now.Year.ToString() + _now.Hour.ToString() + _now.Minute.ToString() + _now.Second.ToString() + _now.Millisecond.ToString();
                context.Response.AddHeader("Content-Disposition", "attachment; filename=" + strNow + ".csv");
                Projects oProject = new Projects(intProfile, dsn);
                DataSet dsProject = oProject.GetActive();
                foreach (DataRow drProject in dsProject.Tables[0].Rows)
                {
                    context.Response.Write(drProject["number"].ToString());
                    context.Response.Write("\t");
                    context.Response.Write(drProject["name"].ToString());
                    context.Response.Write("\t");
                    context.Response.Write(drProject["manager"].ToString());
                    context.Response.Write(Environment.NewLine);
                }
                context.Response.End();
            }
            else
            {
                if (!IsPostBack)
                {
                    txtDevTestUser.Text = "14781";
                    txtDevTestApp.Text = "117";
                    txtUpdateSettingsFreezeStart.Text = oSetting.Get("freeze_start");
                    txtUpdateSettingsFreezeEnd.Text = oSetting.Get("freeze_end");
                    txtUpdateSettingsFreezeSkip.Text = oSetting.Get("freeze_skip_requestid");
                    txtUpdateSettingsDecom.Text = oSetting.Get("decom_override_requestid");
                    txtUpdateSettingsDestroy.Text = oSetting.Get("destroy_override_requestid");
                }
                btnNexus.Attributes.Add("onclick", "return ValidateNexus('" + txtNexusSwitch.ClientID + "','" + txtNexusInterface.ClientID + "','" + radNexusSearch.ClientID + "','" + radNexusAccess.ClientID + "','" + radNexusTrunk.ClientID + "','" + radNexusShutdown.ClientID + "','" + txtNexusVLANs.ClientID + "') && ProcessButton(this);");
                btnProjectImport.Attributes.Add("onclick", "return ProcessButton(this);");
                btnImportCostCenter.Attributes.Add("onclick", "return confirm('WARNING: Due to the high number of cost centers, it could take up to 10 minutes to complete this import.\\n\\nYou should not close the browser or navigate away from the page during the import.\\n\\nAre you sure you want to continue?') && ProcessButton(this);");
                txtUser.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'300','195','" + divAJAX.ClientID + "','" + lstAJAX.ClientID + "','hdnAJAXValue','" + oVariable.URL() + "/frame/users.aspx',2);");
                lstAJAX.Attributes.Add("ondblclick", "AJAXClickRow();");
                btnDecommission.Attributes.Add("onclick", "return ValidateHidden0('hdnAJAXValue','" + txtUser.ClientID + "','Please select the requestor / client') && confirm('WARNING: You are about to alter production data! This action CANNOT be undone!\\n\\nAre you sure you want to continue?');");
            }
        }
        protected void btnDevTest_Click(Object Sender, EventArgs e)
        {
            if (intEnvironment == 1 || intEnvironment == 2 || intEnvironment == 3)
            {
                // Update all services to point to Josh (or administrator)
                SqlHelper.ExecuteNonQuery(dsn, CommandType.Text, "UPDATE cv_services_users SET userid = " + txtDevTestUser.Text + " WHERE deleted = 0");
                // Update all groups to point to the II application (for assignment)
                SqlHelper.ExecuteNonQuery(dsn, CommandType.Text, "UPDATE cv_request_items SET applicationid = " + txtDevTestApp.Text + " WHERE deleted = 0");
                // Grant only specific users the ability to login to dev / test
                char[] strSplit = { ';' };
                string[] strTID = strTIDs.Split(strSplit);
                for (int ii = 0; ii < strTID.Length; ii++)
                {
                    if (strTID[ii].Trim() != "")
                    {
                        string strID = strTID[ii].Trim();
                        string strXID = strID.Substring(0, strID.IndexOf(","));
                        strID = strID.Substring(strID.IndexOf(",") + 1);
                        if (strXID != "")
                        {
                            // ID exists...just change the LAN ID to TID
                            SqlHelper.ExecuteNonQuery(dsn, CommandType.Text, "UPDATE cv_users SET xid = '" + strID + "', modified = getdate() WHERE xid = '" + strXID + "'");
                        }
                        else
                        {
                            // ID does not exist, create it and give permissions to application
                            Users oUser = new Users(0, dsn);
                            int intManager = Int32.Parse(txtDevTestUser.Text);
                            oUser.Add(strID, strID, "Generic", "Account", intManager, 0, 0, 0, "", 0, "", "", 0, 0, 0, 0, 1);
                            int intUser = oUser.GetId(strID);
                            // Load Manager's Role(s)
                            NCC.ClearView.Application.Core.Roles oRole = new NCC.ClearView.Application.Core.Roles(0, dsn);
                            DataSet dsRoles = oRole.Gets(intManager);
                            foreach (DataRow drRole in dsRoles.Tables[0].Rows)
                            {
                                int intApp = Int32.Parse(drRole["applicationid"].ToString());
                                oRole.Add(intUser, oRole.Get(intManager, intApp));
                            }
                        }
                    }
                }
                lblDevTest.Text = "<img src='/images/bigcheck.gif' border='0' align='absmiddle'/> <b>Finished!</b> ClearView configured the environment";
            }
            else
                lblDevTest.Text = "<img src='/images/bigerror.gif' border='0' align='absmiddle'/> <b>Error!</b> ClearView can only configure Dev / Test environments (you are in production)";
        }
        protected void btnPreProdGet_Click(Object Sender, EventArgs e)
        {
            lstPreProdGet.Items.Clear();
            PNCTasks oPNCTasks = new PNCTasks(0, dsn);
            Servers oServer = new Servers(0, dsn);
            DataSet ds = oPNCTasks.GetStepsDesigns();
            foreach (DataRow dr in ds.Tables[1].Rows)
            {
                int intAnswer = Int32.Parse(dr["answerid"].ToString());
                DataSet dsServers = oServer.GetAnswer(intAnswer);
                string strNames = "";
                foreach (DataRow drServer in dsServers.Tables[0].Rows)
                {
                    if (strNames != "")
                        strNames += ", ";
                    strNames += oServer.GetName(Int32.Parse(drServer["id"].ToString()), true);
                }
                lstPreProdGet.Items.Add(new ListItem(intAnswer.ToString() + " [" + dsServers.Tables[0].Rows.Count.ToString() + "] (" + strNames + ")", intAnswer.ToString()));
            }
            btnPreProdSelect.Enabled = (lstPreProdGet.Items.Count > 0);
            btnPreProdGo.Enabled = (lstPreProdGet.Items.Count > 0);
        }
        protected void btnPreProdSelect_Click(Object Sender, EventArgs e)
        {
            foreach (ListItem lstItem in lstPreProdGet.Items)
                lstItem.Selected = true;
        }
        protected void btnPreProdGo_Click(Object Sender, EventArgs e)
        {
            int intApplicationCitrix = Int32.Parse(ConfigurationManager.AppSettings["APPLICATIONID_CITRIX"]);
            int intServiceCSM = Int32.Parse(ConfigurationManager.AppSettings["SERVICEID_CSM"]);
            Servers oServer = new Servers(0, dsn);
            foreach (ListItem lstItem in lstPreProdGet.Items)
            {
                if (lstItem.Selected == true)
                {
                    int intAnswer = Int32.Parse(lstItem.Value);
                    DataSet dsServers = oServer.GetAnswer(intAnswer);
                    foreach (DataRow drServer in dsServers.Tables[0].Rows)
                    {
                        int intServer = Int32.Parse(drServer["id"].ToString());
                        oServer.GetExecution(intServer, intEnvironment, dsnAsset, dsnIP, dsnServiceEditor, intAssignPage, intViewPage);
                    }
                    //lstPreProdGet.Items.Remove(lstItem);
                    lblPreProdGet.Text += lstItem.Text + "<br/>";
                }
            }
            btnPreProdGo.Enabled = false;
        }
        protected void btnGo_Click(Object Sender, EventArgs e)
        {
            Forecast oForecast = new Forecast(0, dsn);
            Workstations oWorkstation = new Workstations(0, dsn);
            int intID = Int32.Parse(txtID.Text);
            DataSet ds = oWorkstation.GetVirtual(intID);
            if (ds.Tables[0].Rows.Count > 0)
            {
                string strSummary = "";
                int intRequest = Int32.Parse(ds.Tables[0].Rows[0]["requestid"].ToString());
                int intAnswer = Int32.Parse(ds.Tables[0].Rows[0]["answerid"].ToString());
                oWorkstation.AssignHost(intID, dsnRemote, dsnAsset, intEnvironment, dsnZeus);
                ModelsProperties oModelsProperties = new ModelsProperties(0, dsn);
                Types oType = new Types(0, dsn);
                int intType = oModelsProperties.GetType(intModelVirtual);
                string strExecute = oType.Get(intType, "forecast_execution_path");
                if (strExecute != "")
                    strSummary = "<a href=\"javascript:void(0);\" title=\"Click here to execute this service\" onclick=\"OpenWindow('FORECAST_EXECUTE','" + strExecute + "?id=" + intAnswer.ToString() + "');\">View the Status of this Build</a>";
                else
                    strSummary = "<a href=\"javascript:void(0);\" title=\"Click here to execute this service\" onclick=\"alert('Execution has not been configured for asset type " + oType.Get(intType, "name") + "');\">View the Status of this Build</a>";
                lblDone.Text = "<img src='/images/bigcheck.gif' border='0' align='absmiddle'/> <b>Finished!</b> ClearView reset the workstation" + "<p>" + strSummary + "</p>";
            }
            else
                lblDone.Text = "<img src='/images/bigerror.gif' border='0' align='absmiddle'/> <b>Error!</b> ClearView could not find the workstation";
        }
        protected void btnServiceItemID_Click(Object Sender, EventArgs e)
        {
            Services oService = new Services(0, dsn);
            int intServiceID = 0;
            Int32.TryParse(txtServiceID.Text, out intServiceID);
            int intItemID_OLD = 0;
            Int32.TryParse(txtServiceItemID_OLD.Text, out intItemID_OLD);
            int intItemID_NEW = 0;
            Int32.TryParse(txtServiceItemID_NEW.Text, out intItemID_NEW);
            if ((intServiceID > 0 || intItemID_OLD > 0) && intItemID_NEW > 0)
            {
                oService.Update(intServiceID, intItemID_NEW, dsnAsset);
                lblServiceItemID.Text = "<img src='/images/bigcheck.gif' border='0' align='absmiddle'/> <b>Finished!</b> ClearView changed the ItemID";
            }
            else
                lblServiceItemID.Text = "<img src='/images/bigError.gif' border='0' align='absmiddle'/> <b>Invalid Data!</b> You must enter a ServiceID/OLD ItemID and a NEW ItemID";
            Page.ClientScript.RegisterStartupScript(typeof(Page), "anchoring_itemid_change", "<script type=\"text/javascript\">location.href='#anchoring_itemid_change'<" + "/" + "script>");
        }
        protected void btnAnswer_Click(Object Sender, EventArgs e)
        {
            Forecast oForecast = new Forecast(0, dsn);
            ModelsProperties oModelsProperties = new ModelsProperties(0, dsn);
            Models oModels = new Models(0, dsn);
            ServerName oServerName = new ServerName(0, dsn);
            int intID = Int32.Parse(txtAnswer.Text);
            int intModelAsset = oForecast.GetModelAsset(intID);
            lblAnswer.Text = "Server Record(s): " + intModelAsset.ToString() + " [" + oModelsProperties.Get(intModelAsset, "name") + "]<br/>";
            int intModel = oForecast.GetModel(intID);
            lblAnswer.Text += "Selection Matrix: " + intModel.ToString() + " [" + oModelsProperties.Get(intModel, "name") + "]<br/>";
            int intModelParent = 0;
            if (Int32.TryParse(oModelsProperties.Get(intModel, "modelid"), out intModelParent))
            {
                if (oModels.Get(intModelParent, "sale") == "1")
                {
                }
                else   // Find reason for lost sale
                {
                    // Application or SubApplicationID specified
                    int intForecastApplicationSub = 0;
                    Int32.TryParse(oForecast.GetAnswer(intID, "subapplicationid"), out intForecastApplicationSub);
                    int intSolutionCode = 0;
                    if (intForecastApplicationSub > 0)
                        Int32.TryParse(oServerName.GetSubApplication(intForecastApplicationSub, "SolutionCode"), out intSolutionCode);
                    if (intSolutionCode > 0)
                        lblAnswer.Text += "*** Model was selected because the application / server type [" + oServerName.GetSubApplication(intForecastApplicationSub, "name") + "] was chosen<br/>";
                    else
                    {
                        int intForecastApplication = 0;
                        Int32.TryParse(oForecast.GetAnswer(intID, "applicationid"), out intForecastApplication);
                        if (intForecastApplicationSub > 0)
                            Int32.TryParse(oServerName.GetApplication(intForecastApplication, "SolutionCode"), out intSolutionCode);
                        if (intSolutionCode > 0)
                            lblAnswer.Text += "*** Model was selected because the sub-application / server role [" + oServerName.GetApplication(intForecastApplication, "name") + "] was chosen<br/>";
                    }
                    if (intSolutionCode == 0)
                    {
                    }
                }
            }
        }
        protected void btnServiceID_Click(Object Sender, EventArgs e)
        {
            // Copy one service (editor enabled) to another
            ServiceEditor oServiceEditor = new ServiceEditor(0, dsnServiceEditor);
            Services oService = new Services(0, dsn);
            int intIDFrom = Int32.Parse(txtServiceIDFrom.Text);
            int intIDTo = Int32.Parse(txtServiceIDTo.Text);
            if (chkServiceID.Checked == true)
            {
                DataSet dsD = oServiceEditor.GetConfigs(intIDTo, 0);
                foreach (DataRow dr in dsD.Tables[0].Rows)
                {
                    int intConfig = Int32.Parse(dr["id"].ToString());
                    int intDBField = Int32.Parse(dr["dbfield"].ToString());
                    // Values
                    oServiceEditor.DeleteConfigValues(intConfig);
                    // Affects
                    oServiceEditor.DeleteConfigAffects(intConfig);
                    // Workflow Shares
                    oServiceEditor.DeleteConfigWorkflowShared(intDBField);

                    oServiceEditor.DeleteConfig(intConfig);
                }
                // Workflows
                oServiceEditor.DeleteConfigWorkflows(intIDFrom, intIDTo);
                // Service Workflows
                DataSet dsWorkflowsD = oService.GetWorkflows(intIDTo);
                foreach (DataRow drWorkflow in dsWorkflowsD.Tables[0].Rows)
                    oService.DeleteWorkflow(Int32.Parse(drWorkflow["id"].ToString()));
                DataSet dsWorkflowsReceiveD = oService.GetWorkflowsReceive(intIDTo);
                foreach (DataRow drWorkflow in dsWorkflowsReceiveD.Tables[0].Rows)
                    oService.DeleteWorkflow(Int32.Parse(drWorkflow["id"].ToString()));

                oServiceEditor.AlterTable(intIDTo);
            }
            DataTable tblConfigs = new DataTable();
            DataColumn tblConfigs1 = new DataColumn();
            tblConfigs1.DataType = System.Type.GetType("System.Int32");
            tblConfigs1.ColumnName = "config1";
            tblConfigs.Columns.Add(tblConfigs1);
            DataColumn tblConfigs2 = new DataColumn();
            tblConfigs2.DataType = System.Type.GetType("System.Int32");
            tblConfigs2.ColumnName = "config2";
            tblConfigs.Columns.Add(tblConfigs2);
            DataRow tblConfigRow;

            DataSet ds = oServiceEditor.GetConfigs(intIDFrom, 0);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                int intConfigOLD = Int32.Parse(dr["id"].ToString());
                int intField = Int32.Parse(dr["fieldid"].ToString());
                int intWM = Int32.Parse(dr["wm"].ToString());
                int intDBField = Int32.Parse(dr["dbfield"].ToString());
                int intLength = Int32.Parse(dr["length"].ToString());
                int intWidth = Int32.Parse(dr["width"].ToString());
                int intHeight = Int32.Parse(dr["height"].ToString());
                int intChecked = Int32.Parse(dr["checked"].ToString());
                int intDirection = Int32.Parse(dr["direction"].ToString());
                int intMultiple = Int32.Parse(dr["multiple"].ToString());
                int intRequired = Int32.Parse(dr["required"].ToString());
                int intDisplay = Int32.Parse(dr["display"].ToString());
                int intConfigNew = oServiceEditor.AddConfig(intIDTo, intField, intWM, dr["question"].ToString(), intLength, intWidth, intHeight, intChecked, intDirection, intMultiple, dr["tip"].ToString(), intRequired, dr["required_text"].ToString(), dr["url"].ToString(), dr["other_text"].ToString(), intDisplay, 1);
                // Values
                DataSet dsValues = oServiceEditor.GetConfigValues(intConfigOLD);
                foreach (DataRow drValue in dsValues.Tables[0].Rows)
                    oServiceEditor.AddConfigValue(intConfigNew, drValue["value"].ToString(), Int32.Parse(drValue["display"].ToString()));
                // Workflow Shares
                DataSet dsShared = oServiceEditor.GetConfigsWorkflowShared(intDBField);
                if (dsShared.Tables[0].Rows.Count > 0)
                    oServiceEditor.AddConfigWorkflowShared(Int32.Parse(oServiceEditor.GetConfig(intConfigNew, "dbfield")));
                // Workflows
                DataSet dsWorkflow = oServiceEditor.GetConfigWorkflows(intConfigOLD);
                foreach (DataRow drWorkflow in dsWorkflow.Tables[0].Rows)
                {
                    if (Int32.Parse(drWorkflow["serviceid"].ToString()) == intIDFrom)
                        oServiceEditor.AddConfigWorkflow(intIDTo, Int32.Parse(drWorkflow["nextservice"].ToString()), intConfigNew, Int32.Parse(drWorkflow["editable"].ToString()));
                    if (Int32.Parse(drWorkflow["nextservice"].ToString()) == intIDFrom)
                        oServiceEditor.AddConfigWorkflow(Int32.Parse(drWorkflow["serviceid"].ToString()), intIDTo, intConfigNew, Int32.Parse(drWorkflow["editable"].ToString()));
                }
                tblConfigRow = tblConfigs.NewRow();
                tblConfigRow["config1"] = intConfigOLD;
                tblConfigRow["config2"] = intConfigNew;
                tblConfigs.Rows.Add(tblConfigRow);
            }
            // Affects
            foreach (DataRow drConfig in tblConfigs.Rows)
            {
                int intConfigOld = Int32.Parse(drConfig["config1"].ToString());
                int intConfigNew = Int32.Parse(drConfig["config2"].ToString());
                DataSet dsAffects = oServiceEditor.GetConfigAffectsConfig(intConfigOld);
                foreach (DataRow drAffect in dsAffects.Tables[0].Rows)
                {
                    int intValueOld = Int32.Parse(drAffect["valueid"].ToString());
                    string strValue = oServiceEditor.GetConfigValue(intValueOld, "value");
                    int intValueNew = 0;
                    DataSet dsValues = oServiceEditor.GetConfigValues(intConfigNew);
                    foreach (DataRow drValue in dsValues.Tables[0].Rows)
                    {
                        if (drValue["value"].ToString() == strValue)
                        {
                            intValueNew = Int32.Parse(drValue["id"].ToString());
                            break;
                        }
                    }
                    if (intValueNew > 0)
                        oServiceEditor.AddConfigAffect(intConfigNew, intValueNew);
                }
            }
            // Service Workflows
            DataSet dsWorkflows = oService.GetWorkflows(intIDFrom);
            foreach (DataRow drWorkflow in dsWorkflows.Tables[0].Rows)
                oService.AddWorkflow(intIDTo, Int32.Parse(drWorkflow["nextservice"].ToString()), Int32.Parse(drWorkflow["display"].ToString()));
            DataSet dsWorkflowsReceive = oService.GetWorkflowsReceive(intIDFrom);
            foreach (DataRow drWorkflow in dsWorkflowsReceive.Tables[0].Rows)
                oService.AddWorkflow(Int32.Parse(drWorkflow["serviceid"].ToString()), intIDTo, Int32.Parse(drWorkflow["display"].ToString()));
            // Config Workflows
            DataSet dsConfigWorkflows = oServiceEditor.GetConfigWorkflowsNext(intIDFrom);
            foreach (DataRow drConfigWorkflow in dsConfigWorkflows.Tables[0].Rows)
                oServiceEditor.AddConfigWorkflow(Int32.Parse(drConfigWorkflow["serviceid"].ToString()), intIDTo, Int32.Parse(drConfigWorkflow["configid"].ToString()), Int32.Parse(drConfigWorkflow["editable"].ToString()));
            // Settings
            oService.UpdateEditorWorkflow(intIDTo, Int32.Parse(oService.Get(intIDFrom, "sametime")), Int32.Parse(oService.Get(intIDFrom, "workflow_connect")), Int32.Parse(oService.Get(intIDFrom, "same_technician")));
            oService.UpdateEditorWorkflow(intIDTo, Int32.Parse(oService.Get(intIDFrom, "workflow")), oService.Get(intIDFrom, "workflow_title"));
            oServiceEditor.AlterTable(intIDTo);
            lblServiceID.Text = "<img src='/images/bigcheck.gif' border='0' align='absmiddle'/> <b>Finished!</b> ClearView finished the copy successfully";
        }
        protected void btnAnswerSAN_Click(Object Sender, EventArgs e)
        {
            Forecast oForecast = new Forecast(0, dsn);
            Users oUser = new Users(0, dsn);
            ResourceRequest oResourceRequest = new ResourceRequest(0, dsn);
            OnDemandTasks oOnDemandTasks = new OnDemandTasks(0, dsn);
            ServiceRequests oServiceRequest = new ServiceRequests(0, dsn);
            ModelsProperties oModelsProperties = new ModelsProperties(0, dsn);
            int intID = Int32.Parse(txtAnswerSAN.Text);
            int intModel = oForecast.GetModel(intID);
            Servers oServer = new Servers(0, dsn);
            int intQuantity = oServer.GetAnswer(intID).Tables[0].Rows.Count;
            if (oForecast.IsStorage(intID) == true && oForecast.GetAnswer(intID, "storage") == "1" && oModelsProperties.IsVMwareVirtual(intModel) == false)
            {
                // Send to Storage
                int intRequest = oForecast.GetRequestID(intID, true);
                int intStorageNumber = oResourceRequest.GetNumber(intRequest, intStorageItem);
                oOnDemandTasks.AddServerStorage(intRequest, intStorageItem, intStorageNumber, intID, (chkAnswerSAN.Checked ? 1 : 0), intModel);
                int intStorage = oResourceRequest.Add(intRequest, intStorageItem, intStorageService, intStorageNumber, (chkAnswerSAN.Checked ? "Move to Production (Storage)" : "Auto-Provisioning Task (Storage)"), intQuantity, 0.00, 2, 1, 1, 1);
                if (oServiceRequest.NotifyApproval(intStorage, intResourceRequestApprove, intEnvironment, "", dsnServiceEditor) == false)
                    oServiceRequest.NotifyTeamLead(intStorageItem, intStorage, intAssignPage, intViewPage, intEnvironment,  "", dsnServiceEditor, dsnAsset, dsnIP, 0);
                lblAnswerSAN.Text = "<img src='/images/bigcheck.gif' border='0' align='absmiddle'/> <b>Finished!</b> ClearView finished the form";
            }
            else
                lblAnswerSAN.Text = "<img src='/images/bigerror.gif' border='0' align='absmiddle'/> <b>Error!</b> ClearView could not generate the form since no storage was requested";
        }
        protected void btnAnswerTSM_Click(Object Sender, EventArgs e)
        {
            Forecast oForecast = new Forecast(0, dsn);
            Users oUser = new Users(0, dsn);
            ResourceRequest oResourceRequest = new ResourceRequest(0, dsn);
            OnDemandTasks oOnDemandTasks = new OnDemandTasks(0, dsn);
            ServiceRequests oServiceRequest = new ServiceRequests(0, dsn);
            int intID = Int32.Parse(txtAnswerTSM.Text);
            Servers oServer = new Servers(0, dsn);
            int intQuantity = oServer.GetAnswer(intID).Tables[0].Rows.Count;
            if (oForecast.GetAnswer(intID, "backup") == "1")
            {
                // Send to Backup
                int intRequest = oForecast.GetRequestID(intID, true);
                int intModel = oForecast.GetModel(intID);
                int intBackupNumber = oResourceRequest.GetNumber(intRequest, intBackupItem);
                oOnDemandTasks.AddServerBackup(intRequest, intBackupItem, intBackupNumber, intID, intModel);
                int intBackup = oResourceRequest.Add(intRequest, intBackupItem, intBackupService, intBackupNumber, "Auto-Provisioning Task (Backup)", intQuantity, 0.00, 2, 1, 1, 1);
                if (oServiceRequest.NotifyApproval(intBackup, intResourceRequestApprove, intEnvironment, "", dsnServiceEditor) == false)
                    oServiceRequest.NotifyTeamLead(intBackupItem, intBackup, intAssignPage, intViewPage, intEnvironment,  "", dsnServiceEditor, dsnAsset, dsnIP, 0);
                lblAnswerTSM.Text = "<img src='/images/bigcheck.gif' border='0' align='absmiddle'/> <b>Finished!</b> ClearView finished the form";
            }
            else
                lblAnswerTSM.Text = "<img src='/images/bigerror.gif' border='0' align='absmiddle'/> <b>Error!</b> ClearView could not generate the form since no backup was requested";
        }
        protected void SendForm(int _answerid, bool _san, bool _tsm)
        {
            PDFs oPDF = new PDFs(dsn, dsnAsset, dsnIP, intEnvironment);
            oPDF.CreateDocuments(_answerid, _san, _tsm, null, true, true, true, false, boolUsePNCNaming, true);
        }
        protected void btnProdDRAsset_Click(Object Sender, EventArgs e)
        {
            lblProdDRAsset.Text = "";
            Functions oFunction = new Functions(0, dsn, intEnvironment);
            Variables oVariable = new Variables(intEnvironment);
            string strServer = txtProdDRAsset.Text;
            Forecast oForecast = new Forecast(0, dsn);
            Asset oAsset = new Asset(0, dsnAsset);
            Servers oServer = new Servers(0, dsn);
            OnDemandTasks oOnDemandTasks = new OnDemandTasks(0, dsn);
            ResourceRequest oResourceRequest = new ResourceRequest(0, dsn);
            ModelsProperties oModelsProperties = new ModelsProperties(0, dsn);
            Resiliency oResiliency = new Resiliency(0, dsn);
            DataSet dsServer = oServer.Get(strServer, false);
            if (dsServer.Tables[0].Rows.Count == 1)
            {
                int intAnswer = 0;
                if (dsServer.Tables[0].Rows[0]["answerid"].ToString() != "")
                    intAnswer = Int32.Parse(dsServer.Tables[0].Rows[0]["answerid"].ToString());
                int intOS = 0;
                if (dsServer.Tables[0].Rows[0]["osid"].ToString() != "")
                    intOS = Int32.Parse(dsServer.Tables[0].Rows[0]["osid"].ToString());
                int intServer = Int32.Parse(dsServer.Tables[0].Rows[0]["id"].ToString());
                if (intAnswer > 0)
                {
                    int intRecovery = 0;
                    if (oForecast.GetAnswer(intAnswer, "recovery_number") != "")
                        intRecovery = Int32.Parse(oForecast.GetAnswer(intAnswer, "recovery_number"));
                    int intMnemonic = 0;
                    if (oForecast.GetAnswer(intAnswer, "mnemonicid") != "")
                        intMnemonic = Int32.Parse(oForecast.GetAnswer(intAnswer, "mnemonicid"));
                    bool boolBIR = (oForecast.GetAnswer(intAnswer, "resiliency") == "1");    // Related to the Business Resiliency Effort (use old data center strategy)
                    int intResiliency = oResiliency.GetIDFromMnemonic(intMnemonic, boolBIR, intAnswer);
                    DataSet dsModel = oOnDemandTasks.GetModel(intAnswer);
                    if (dsModel.Tables[0].Rows.Count > 0)
                    {
                        bool boolBlade = false;
                        switch (dsModel.Tables[0].Rows[0]["type"].ToString())
                        {
                            case "BLADE":
                                boolBlade = true;
                                break;
                        }
                        int intClass = Int32.Parse(oForecast.GetAnswer(intAnswer, "classid"));
                        int intEnv = Int32.Parse(oForecast.GetAnswer(intAnswer, "environmentid"));
                        int intAddress = Int32.Parse(oForecast.GetAnswer(intAnswer, "addressid"));
                        string strName = oServer.GetName(intServer, boolUsePNCNaming);
                        int intModel = Int32.Parse(dsServer.Tables[0].Rows[0]["modelid"].ToString());
                        int intAssetDR = 0;
                        bool boolSuccess = false;
                        if (chkProdDRAssetOnly.Checked == true)
                        {
                            if (dsServer.Tables[0].Rows[0]["dr"].ToString() == "1" && dsServer.Tables[0].Rows[0]["dr_exist"].ToString() != "1")
                            {
                                DataSet dsAssets = oAsset.GetServerOrBladeAvailableDR(intEnv, intModel, 0, oModelsProperties.IsDell(intModel), null);
                                if (dsAssets.Tables[0].Rows.Count > 0)
                                    intAssetDR = Int32.Parse(dsAssets.Tables[0].Rows[0]["id"].ToString());
                                if (intAssetDR > 0)
                                {
                                    lblProdDRAsset.Text += "<img src='/images/bigcheck.gif' border='0' align='absmiddle'/> <b>Finished!</b> ClearView assigned DR asset " + oAsset.Get(intAssetDR, "serial") + " (ASSETID: " + intAssetDR.ToString() + ")<br/>";
                                    oAsset.AddStatus(intAssetDR, strName + "-DR", (int)AssetStatus.InUse, -999, DateTime.Now);
                                    oServer.AddAsset(intServer, intAssetDR, intClass, intEnv, 0, 1);
                                    boolSuccess = true;
                                }
                                else
                                    lblProdDRAsset.Text += "<img src='/images/bigerror.gif' border='0' align='absmiddle'/> <b>Error!</b> Could Not Find DR Asset (Env:" + intEnv.ToString() + ", ModelID:" + intModel.ToString() + ")<br/>";
                            }
                            else
                            {
                                lblProdDRAsset.Text += "<img src='/images/bigAlert.gif' border='0' align='absmiddle'/> <b>Warning:</b> DR not selected<br/>";
                                boolSuccess = true;
                            }
                        }
                        else
                        {
                            bool boolGetDR = (intRecovery > 0 && (boolBlade == true || (dsServer.Tables[0].Rows[0]["dr"].ToString() == "1" && dsServer.Tables[0].Rows[0]["dr_exist"].ToString() != "1")));
                            List<int> lstAssets = oAsset.GetServerOrBladeAvailable(intClass, intEnv, intAddress, intModel, intAnswer, dsn, strHA, false, 0, intResiliency, intOS, "", boolGetDR, oModelsProperties.IsDell(intModel), dsnServiceEditor);
                            int intAsset = lstAssets[0];
                            if (intAsset > 0)
                            {
                                if (boolGetDR == true)
                                    intAssetDR = lstAssets[1];
                                else
                                    intAssetDR = -1;
                            }

                            if (intAsset > 0 && intAssetDR != 0)
                            {
                                lblProdDRAsset.Text += "<img src='/images/bigcheck.gif' border='0' align='absmiddle'/> <b>Finished!</b> ClearView assigned PROD asset " + oAsset.Get(intAsset, "serial") + " (ASSETID: " + intAsset.ToString() + ")<br/>";
                                oAsset.AddStatus(intAsset, strName, (int)AssetStatus.InUse, -999, DateTime.Now);
                                oServer.AddAsset(intServer, intAsset, intClass, intEnv, 0, 0);
                                if (intAssetDR > 0)
                                {
                                    lblProdDRAsset.Text += "<img src='/images/bigcheck.gif' border='0' align='absmiddle'/> <b>Finished!</b> ClearView assigned DR asset " + oAsset.Get(intAssetDR, "serial") + " (ASSETID: " + intAssetDR.ToString() + ")<br/>";
                                    oAsset.AddStatus(intAssetDR, strName + "-DR", (int)AssetStatus.InUse, -999, DateTime.Now);
                                    oServer.AddAsset(intServer, intAssetDR, intClass, intEnv, 0, 1);
                                }
                                else
                                    lblProdDRAsset.Text += "<img src='/images/bigAlert.gif' border='0' align='absmiddle'/> <b>Warning:</b> DR not selected<br/>";
                                boolSuccess = true;
                            }
                            else
                            {
                                if (intAsset > 0)
                                {
                                    lblProdDRAsset.Text += "<img src='/images/bigcheck.gif' border='0' align='absmiddle'/> <b>Finished!</b> ClearView found PROD asset " + oAsset.Get(intAsset, "serial") + " (ASSETID: " + intAsset.ToString() + ") but did not assign it<br/>";
                                    boolSuccess = true;
                                }
                                else
                                    lblProdDRAsset.Text += "<img src='/images/bigerror.gif' border='0' align='absmiddle'/> <b>Error!</b> Could Not Find PROD Asset<br/>";
                                if (intAssetDR > 0)
                                {
                                    lblProdDRAsset.Text += "<img src='/images/bigcheck.gif' border='0' align='absmiddle'/> <b>Finished!</b> ClearView found DR asset " + oAsset.Get(intAssetDR, "serial") + " (ASSETID: " + intAssetDR.ToString() + ") but did not assign it<br/>";
                                    boolSuccess = true;
                                }
                                else
                                    lblProdDRAsset.Text += "<img src='/images/bigerror.gif' border='0' align='absmiddle'/> <b>Error!</b> Could Not Find DR Asset<br/>";
                            }
                        }
                        if (boolSuccess == true)
                        {
                            //DataSet dsPending = oOnDemandTasks.GetPending(intAnswer);
                            //if (dsPending.Tables[0].Rows.Count > 0)
                            //{
                            //    int intWorkflowResource = Int32.Parse(dsPending.Tables[0].Rows[0]["resourceid"].ToString());
                            //    int intWorkflowParent = oResourceRequest.GetWorkflowParent(intWorkflowResource);
                            //    int intRequest = 0;
                            //    int intItem = 0;
                            //    int intNumber = 0;
                            //    DataSet dsResource = oResourceRequest.Get(intWorkflowParent);
                            //    if (dsResource.Tables[0].Rows.Count > 0)
                            //    {
                            //        intRequest = Int32.Parse(dsResource.Tables[0].Rows[0]["requestid"].ToString());
                            //        intItem = Int32.Parse(dsResource.Tables[0].Rows[0]["itemid"].ToString());
                            //        intNumber = Int32.Parse(dsResource.Tables[0].Rows[0]["number"].ToString());

                            //        oOnDemandTasks.UpdateGenericIIProd(intRequest, intItem, intNumber);
                            //    }
                            //}
                        }
                    }
                    else
                        lblProdDRAsset.Text += "<img src='/images/bigerror.gif' border='0' align='absmiddle'/> <b>Error!</b> Could Not Find an EXECUTED OnDemand Task (ANSWERID: " + intAnswer.ToString() + ")<br/>";

                    // Release if selected
                    if (chkProdDRAsset.Checked == true)
                    {
                        int intClass = Int32.Parse(oForecast.GetAnswer(intAnswer, "classid"));
                        int intEnv = Int32.Parse(oForecast.GetAnswer(intAnswer, "environmentid"));
                        string strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_ALERT");
                        oFunction.SendEmail("!!! Releasing Build Assets !!!", strEMailIdsBCC, "", "", "!!! Releasing Build Assets !!!", "The build assets for ANSWERID " + intAnswer.ToString() + " (BLADE)", true, false);
                        DataSet dsAssets = oServer.GetAssetsNot(intAnswer, intClass, intEnv);
                        foreach (DataRow drAsset in dsAssets.Tables[0].Rows)
                        {
                            int intNewServer = Int32.Parse(drAsset["id"].ToString());
                            if (intNewServer == intServer)
                            {
                                int intAsset = Int32.Parse(drAsset["assetid"].ToString());
                                oAsset.AddStatus(intAsset, "", (int)AssetStatus.Available, -999, DateTime.Now);
                                //oServer.DeleteAsset(intServer, intAsset);
                                oServer.DeleteIP(intServer, 1, 0, 0, 0, dsnIP);
                            }
                        }
                    }
                }
                else
                    lblProdDRAsset.Text += "<img src='/images/bigerror.gif' border='0' align='absmiddle'/> <b>Error!</b> Could Not Find AnswerID<br/>";
            }
            else
                lblProdDRAsset.Text += "<img src='/images/bigerror.gif' border='0' align='absmiddle'/> <b>Error!</b> Could Not Find a Server Named " + strServer + "<br/>";
        }
        protected void btnRR_Click(Object Sender, EventArgs e)
        {
            DataSet ds = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT * FROM cv_resource_requests WHERE deleted = 0");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                int intID = Int32.Parse(dr["id"].ToString());
                DataSet dsR = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT * FROM cv_resource_requests_workflow WHERE deleted = 0 AND parent = " + intID.ToString());
                if (dsR.Tables[0].Rows.Count > 0)
                {
                    bool bool_2 = true;
                    bool bool2 = true;
                    bool bool3 = true;
                    bool bool5 = true;
                    bool bool7 = true;
                    bool bool10 = true;
                    foreach (DataRow drR in dsR.Tables[0].Rows)
                    {
                        if (drR["status"].ToString() != "-2")
                            bool_2 = false;
                        if (drR["status"].ToString() != "2" && drR["status"].ToString() != "1")
                            bool2 = false;
                        if (drR["status"].ToString() != "3")
                            bool3 = false;
                        if (drR["status"].ToString() != "5")
                            bool5 = false;
                        if (drR["status"].ToString() != "7")
                            bool7 = false;
                        if (drR["status"].ToString() != "10")
                            bool10 = false;
                    }
                    if (bool_2 == true)
                    {
                        SqlHelper.ExecuteNonQuery(dsn, CommandType.Text, "UPDATE cv_resource_requests SET status = -2, completed = null WHERE id = " + intID.ToString() + " AND status <> -2");
                        intCount++;
                    }
                    else if (bool2 == true)
                    {
                        SqlHelper.ExecuteNonQuery(dsn, CommandType.Text, "UPDATE cv_resource_requests SET status = 2, completed = null WHERE id = " + intID.ToString() + " AND status <> 2");
                        intCount++;
                    }
                    else if (bool3 == true)
                    {
                        SqlHelper.ExecuteNonQuery(dsn, CommandType.Text, "UPDATE cv_resource_requests SET status = 3, completed = getdate() WHERE id = " + intID.ToString() + " AND status <> 3");
                        intCount++;
                    }
                    else if (bool5 == true)
                    {
                        SqlHelper.ExecuteNonQuery(dsn, CommandType.Text, "UPDATE cv_resource_requests SET status = 5, completed = null WHERE id = " + intID.ToString() + " AND status <> 5");
                        intCount++;
                    }
                    else if (bool7 == true)
                    {
                        SqlHelper.ExecuteNonQuery(dsn, CommandType.Text, "UPDATE cv_resource_requests SET status = 7, completed = null WHERE id = " + intID.ToString() + " AND status <> 7");
                        intCount++;
                    }
                    else if (bool10 == true)
                    {
                        SqlHelper.ExecuteNonQuery(dsn, CommandType.Text, "UPDATE cv_resource_requests SET status = 10, completed = null WHERE id = " + intID.ToString() + " AND status <> 10");
                        intCount++;
                    }
                }
            }
            lblRR.Text = "<img src='/images/bigcheck.gif' border='0' align='absmiddle'/> <b>Finished!</b> ClearView updated " + intCount.ToString() + " records";
        }
        protected void btnFix_Click(Object Sender, EventArgs e)
        {
            //DataSet ds = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "select distinct id from cv_forecast_answers where deleted = 0 AND completed is null AND id in (SELECT answerid FROM cv_ondemand_tasks_generic_ii WHERE deleted = 0) order by id");
            //foreach (DataRow dr in ds.Tables[0].Rows)
            //{
            //    int intAnswer = Int32.Parse(dr["id"].ToString());
            //    Forecast oForecast = new Forecast(0, dsn);
            //    int intForecast = Int32.Parse(oForecast.GetAnswer(intAnswer, "forecastid"));
            //    int intRequest = Int32.Parse(oForecast.Get(intForecast, "requestid"));
            //    ServiceRequests oServiceRequest = new ServiceRequests(0, dsn);
            //    if (oServiceRequest.Get(intRequest).Tables[0].Rows.Count == 0)
            //        oServiceRequest.Add(intRequest, 1, 1);
            //    Requests oRequest = new Requests(0, dsn);
            //    int intProject = oRequest.GetProjectNumber(intRequest);
            //    ResourceRequest oResourceRequest = new ResourceRequest(0, dsn);
            //    int intImplementorOLD = 0;
            //    DataSet dsResource = oResourceRequest.GetProjectItem(intProject, intImplementorDistributed);
            //    if (oForecast.GetPlatformDistributed(intAnswer, intWorkstationPlatform) == false)
            //        dsResource = oResourceRequest.GetProjectItem(intProject, intImplementorMidrange);
            //    if (dsResource.Tables[0].Rows.Count > 0)
            //        intImplementorOLD = (dsResource.Tables[0].Rows[0]["userid"].ToString() == "" ? 0 : Int32.Parse(dsResource.Tables[0].Rows[0]["userid"].ToString()));
            //    if (intImplementorOLD > 0)
            //    {
            //        bool boolAssign = true;
            //        OnDemandTasks oOnDemandTasks = new OnDemandTasks(0, dsn);
            //        if (oOnDemandTasks.GetModel(intAnswer).Tables[0].Rows.Count > 0)
            //            boolAssign = false;
            //        // Get Task from Pending
            //        DataSet dsPending = oOnDemandTasks.GetPending(intAnswer);
            //        if (dsPending.Tables[0].Rows.Count > 0)
            //        {
            //            int intModel = oForecast.GetModel(intAnswer);
            //            int intResourceWorkflow = Int32.Parse(dsPending.Tables[0].Rows[0]["resourceid"].ToString());
            //            int intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);
            //            int intNumberExecute = Int32.Parse(oResourceRequest.Get(intResourceParent, "number"));
            //            Solution oSolution = new Solution(0, dsn);
            //            DataSet dsSolution = oSolution.GetCodeModel(intModel);
            //            int intServiceExecute = Int32.Parse(dsSolution.Tables[0].Rows[0]["serviceid"].ToString());
            //            Services oService = new Services(0, dsn);
            //            int intItemExecute = oService.GetItemId(intServiceExecute);
            //            int intQuantityExecute = Int32.Parse(oForecast.GetAnswer(intAnswer, "quantity"));
            //            ModelsProperties oModelsProperties = new ModelsProperties(0, dsn);
            //            int intType = oModelsProperties.GetType(intModel);
                       
            //            int intImplementorNEW = Int32.Parse(oResourceRequest.GetWorkflow(intResourceWorkflow, "userid"));
            //            int intServiceExecuteOLD = Int32.Parse(oResourceRequest.Get(intResourceParent, "serviceid"));
            //            int intItemExecuteOLD = Int32.Parse(oResourceRequest.Get(intResourceParent, "itemid"));
            //            if (boolAssign == false && (intImplementorOLD != intImplementorNEW || intServiceExecuteOLD != intServiceExecute || intItemExecuteOLD != intItemExecute))
            //                boolAssign = true;
            //            if (boolAssign == true)
            //            {
            //                intCount++;
            //                oOnDemandTasks.DeleteAll(intAnswer);
            //                //oOnDemandTasks.AddII(intRequest, intItemExecute, intNumberExecute, intAnswer, intModel);
            //                if (oModelsProperties.IsNotExecutable(intModel) == true)
            //                {
            //                    oOnDemandTasks.AddGenericII(intRequest, intItemExecute, intNumberExecute, intAnswer, intModel);
            //                }
            //                else
            //                {
            //                    if (oModelsProperties.IsTypeBlade(intModel) == true)
            //                    {
            //                        oOnDemandTasks.AddBladeII(intRequest, intItemExecute, intNumberExecute, intAnswer, intModel);
            //                    }
            //                    else if (oModelsProperties.IsTypePhysical(intModel) == true)
            //                    {
            //                        oOnDemandTasks.AddPhysicalII(intRequest, intItemExecute, intNumberExecute, intAnswer, intModel);
            //                    }
            //                    else if (oModelsProperties.IsTypeVMware(intModel) == true)
            //                    {
            //                        oOnDemandTasks.AddVMWareII(intRequest, intItemExecute, intNumberExecute, intAnswer, intModel);
            //                    }
            //                    else
            //                    {
            //                        Functions oFunction = new Functions(0, dsn, intEnvironment);
            //                        Variables oVariable = new Variables(intEnvironment);
            //                        string strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_ERROR");
            //                        oFunction.SendEmail("Auto-Provisioning Assignment Problem", strEMailIdsBCC, "", "", "Auto-Provisioning Assignment Problem", "<p>An auto-provisioning assignment was clicked, but there was a problem assigning a resource.</p><p>AnswerID: " + intAnswer.ToString() + "</p>", true, false);
            //                    }
            //                }
            //                oResourceRequest.UpdateAccepted(intResourceParent, 1);
            //                oResourceRequest.UpdateWorkflowAssigned(intResourceWorkflow, intImplementorNEW);
            //                oResourceRequest.UpdateItemAndService(intResourceParent, intItemExecute, intServiceExecute);
            //                oResourceRequest.UpdateWorkflowName(intResourceWorkflow, oForecast.GetAnswer(intAnswer, "name") + " [" + DateTime.Parse(oForecast.GetAnswer(intAnswer, "implementation")).ToShortDateString() + "]", 0);
            //            }
            //        }
            //    }
            //}
            //lblFix.Text = "<img src='/images/bigcheck.gif' border='0' align='absmiddle'/> <b>Finished!</b> ClearView updated " + intCount.ToString() + " records";
            lblFix.Text = "<img src='/images/bigAlert.gif' border='0' align='absmiddle'/> <b>Disabled!</b> ClearView updated 0 records";
        }

        protected void btnDuplicate_Click(Object Sender, EventArgs e)
        {
            string strDuplicate = "";
            // Check for duplicate asset in CVA_ASSETS
            DataSet dsSerial = SqlHelper.ExecuteDataset(dsnAsset, CommandType.Text, "SELECT DISTINCT cva_assets.serial FROM cva_assets INNER JOIN cva_server ON cva_assets.id = cva_server.assetid AND cva_server.deleted = 0 WHERE cva_assets.deleted = 0 AND cva_assets.serial IS NOT NULL AND cva_assets.serial <> '' UNION ALL SELECT cva_assets.serial FROM cva_assets INNER JOIN cva_blades ON cva_assets.id = cva_blades.assetid AND cva_blades.deleted = 0 WHERE cva_assets.deleted = 0 AND cva_assets.serial IS NOT NULL AND cva_assets.serial <> ''");
            foreach (DataRow drSerial in dsSerial.Tables[0].Rows)
            {
                DataSet dsAsset = SqlHelper.ExecuteDataset(dsnAsset, CommandType.Text, "SELECT * FROM cva_assets WHERE deleted = 0 AND serial = '" + drSerial["serial"].ToString() + "'");
                foreach (DataRow drAsset in dsAsset.Tables[0].Rows)
                {
                    DataSet dsAssetS = SqlHelper.ExecuteDataset(dsnAsset, CommandType.Text, "SELECT * FROM cva_server WHERE deleted = 0 AND assetid = " + drAsset["id"].ToString());
                    DataSet dsAssetB = SqlHelper.ExecuteDataset(dsnAsset, CommandType.Text, "SELECT * FROM cva_blades WHERE deleted = 0 AND assetid = " + drAsset["id"].ToString());
                    if (dsAssetS.Tables[0].Rows.Count == 0 && dsAssetB.Tables[0].Rows.Count == 0)
                    {
                        SqlHelper.ExecuteNonQuery(dsnAsset, CommandType.Text, "UPDATE cva_assets SET deleted = 1 WHERE deleted = 0 AND id = " + drAsset["id"].ToString());
                        strDuplicate += "Deleted Duplicate ASSETID " + drAsset["id"].ToString() + " from CVA_ASSETS table<br/>";
                    }
                }
            }
            // Check for duplicate asset in CVA_SERVERS and CVA_BLADES
            DataSet ds = SqlHelper.ExecuteDataset(dsnAsset, CommandType.Text, "SELECT DISTINCT cva_assets.id FROM cva_assets INNER JOIN cva_server ON cva_assets.id = cva_server.assetid AND cva_server.deleted = 0 WHERE cva_assets.deleted = 0 UNION ALL SELECT cva_assets.id FROM cva_assets INNER JOIN cva_blades ON cva_assets.id = cva_blades.assetid AND cva_blades.deleted = 0 WHERE cva_assets.deleted = 0");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                int intAsset = Int32.Parse(dr["id"].ToString());
                // CVA_SERVER
                DataSet dsServer = SqlHelper.ExecuteDataset(dsnAsset, CommandType.Text, "SELECT * FROM cva_server WHERE deleted = 0 AND assetid = " + intAsset.ToString());
                if (radDuplicate.SelectedItem.Value == "FIRST")
                {
                    int intServer = 0;
                    foreach (DataRow drServer in dsServer.Tables[0].Rows)
                    {
                        intServer++;
                        if (intServer > 1)
                        {
                            SqlHelper.ExecuteNonQuery(dsnAsset, CommandType.Text, "UPDATE cva_server SET deleted = 1 WHERE deleted = 0 AND id = " + drServer["id"].ToString());
                            strDuplicate += "Deleted Duplicate ASSETID " + drServer["assetid"].ToString() + " from CVA_SERVER table<br/>";
                        }
                    }
                }
                else
                {
                    for (int ii = 0; ii < dsServer.Tables[0].Rows.Count - 1; ii++)
                    {
                        SqlHelper.ExecuteNonQuery(dsnAsset, CommandType.Text, "UPDATE cva_server SET deleted = 1 WHERE deleted = 0 AND id = " + dsServer.Tables[0].Rows[ii]["id"].ToString());
                        strDuplicate += "Deleted Duplicate ASSETID " + dsServer.Tables[0].Rows[ii]["assetid"].ToString() + " from CVA_SERVER table<br/>";
                    }
                }
                // CVA_BLADES
                DataSet dsBlade = SqlHelper.ExecuteDataset(dsnAsset, CommandType.Text, "SELECT * FROM cva_blades WHERE deleted = 0 AND assetid = " + intAsset.ToString());
                if (radDuplicate.SelectedItem.Value == "FIRST")
                {
                    int intBlade = 0;
                    foreach (DataRow drBlade in dsBlade.Tables[0].Rows)
                    {
                        intBlade++;
                        if (intBlade > 1)
                        {
                            SqlHelper.ExecuteNonQuery(dsnAsset, CommandType.Text, "UPDATE cva_blades SET deleted = 1 WHERE deleted = 0 AND id = " + drBlade["id"].ToString());
                            strDuplicate += "Deleted Duplicate ASSETID " + drBlade["assetid"].ToString() + " from CVA_BLADES table<br/>";
                        }
                    }
                }
                else
                {
                    for (int ii = 0; ii < dsBlade.Tables[0].Rows.Count - 1; ii++)
                    {
                        SqlHelper.ExecuteNonQuery(dsnAsset, CommandType.Text, "UPDATE cva_blades SET deleted = 1 WHERE deleted = 0 AND id = " + dsBlade.Tables[0].Rows[ii]["id"].ToString());
                        strDuplicate += "Deleted Duplicate ASSETID " + dsBlade.Tables[0].Rows[ii]["assetid"].ToString() + " from CVA_BLADES table<br/>";
                    }
                }
            }
            lblDuplicate.Text = strDuplicate;
        }
        protected void btnImportUser_Click(Object Sender, EventArgs e)
        {
            bool boolDelete = false;
            string strFile = "";
            if (filImportUser.PostedFile != null)
            {
                Users oUser = new Users(0, dsn);
                DateTime _now = DateTime.Now;
                string strNow = _now.Day.ToString() + _now.Month.ToString() + _now.Year.ToString() + _now.Hour.ToString() + _now.Minute.ToString() + _now.Second.ToString() + _now.Millisecond.ToString();
                strFile = Request.PhysicalApplicationPath + "\\admin\\imports\\" + strNow + ".xls";
                filImportUser.PostedFile.SaveAs(strFile);
                string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strFile + ";Extended Properties=Excel 8.0;";
                OleDbDataAdapter myCommand = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", strConn);
                DataSet ds = new DataSet();
                myCommand.Fill(ds, "ExcelInfo");
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr[0].ToString().Trim() == "")
                        break;
                    int intManager = oUser.GetId(dr[0].ToString().Trim());
                    int intUser = oUser.GetId(dr[1].ToString().Trim());
                    oUser.Update(intUser, intManager);
                    intCount++;
                }
                if (File.Exists(strFile) == true)
                {
                    boolDelete = true;
                    File.Delete(strFile);
                }
            }
            lblImportUser.Text = "<img src='/images/bigcheck.gif' border='0' align='absmiddle'/> <b>Finished!</b> ClearView updated " + intCount.ToString() + " users<br/>";
            if (boolDelete == true)
                lblImportUser.Text += "<img src='/images/bigcheck.gif' border='0' align='absmiddle'/> ClearView deleted the file " + strFile;
            else
                lblImportUser.Text += "<img src='/images/bigerror.gif' border='0' align='absmiddle'/> ClearView DID NOT delete the file " + strFile;
        }
        protected void btnImportDecom_Click(Object Sender, EventArgs e)
        {
            bool boolDelete = false;
            int intService = Int32.Parse(txtImportDecom.Text);
            string strImportDecom = "";
            string strFile = "";
            if (filImportDecom.PostedFile != null)
            {
                Users oUser = new Users(0, dsn);
                Customized oCustomized = new Customized(0, dsn);
                Services oService = new Services(0, dsn);
                Requests oRequest = new Requests(0, dsn);
                ServiceRequests oServiceRequest = new ServiceRequests(0, dsn);
                RequestItems oRequestItem = new RequestItems(0, dsn);
                Asset oAsset = new Asset(0, dsnAsset);
                Servers oServers = new Servers(0, dsn);
                ModelsProperties oModelsProperties = new ModelsProperties(0, dsn);
                ResourceRequest oResourceRequest = new ResourceRequest(0, dsn);

                DateTime _now = DateTime.Now;
                string strNow = _now.Day.ToString() + _now.Month.ToString() + _now.Year.ToString() + _now.Hour.ToString() + _now.Minute.ToString() + _now.Second.ToString() + _now.Millisecond.ToString();
                strFile = Request.PhysicalApplicationPath + "\\admin\\imports\\" + strNow + ".xls";
                filImportDecom.PostedFile.SaveAs(strFile);
                string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strFile + ";Extended Properties=Excel 8.0;";
                OleDbDataAdapter myCommand = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", strConn);
                DataSet ds = new DataSet();
                myCommand.Fill(ds, "ExcelInfo");
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr[0].ToString().Trim() == "")
                        break;
                    string strName = dr[0].ToString().Trim();
                    strImportDecom += strName.ToUpper() + "......";
                    string strUser = dr[1].ToString().Trim();
                    int intUser = oUser.GetId(strUser);
                    if (intUser > 0)
                    {
                        string strOff = dr[2].ToString().Trim();
                        try
                        {
                            DateTime datOff = DateTime.Parse(strOff);
                            string strChange = dr[3].ToString().Trim();
                            string strReason = dr[4].ToString().Trim();

                            // Lookup server
                            DataSet dsServer = oServers.GetDecommission(strName);
                            if (dsServer.Tables[0].Rows.Count == 1)
                            {
                                int intServer = Int32.Parse(dsServer.Tables[0].Rows[0]["id"].ToString());
                                int intAsset = 0;
                                int intAssetDR = 0;
                                DataSet dsAsset = oServers.GetAssets(intServer);
                                foreach (DataRow drAsset in dsAsset.Tables[0].Rows)
                                {
                                    if (drAsset["latest"].ToString() == "1")
                                        intAsset = Int32.Parse(drAsset["assetid"].ToString());
                                    if (drAsset["dr"].ToString() == "1")
                                        intAssetDR = Int32.Parse(drAsset["assetid"].ToString());
                                }
                                if (intAsset > 0)
                                {
                                    int intClass = 0;
                                    int intEnv = 0;
                                    int intAddress = 0;
                                    try
                                    {
                                        intClass = Int32.Parse(oAsset.GetServerOrBlade(intAsset, "classid"));
                                        intEnv = Int32.Parse(oAsset.GetServerOrBlade(intAsset, "environmentid"));
                                        intAddress = Int32.Parse(oAsset.GetServerOrBlade(intAsset, "addressid"));
                                    }
                                    catch
                                    {
                                        DataSet dsCatch = oServers.GetAsset(intAsset);
                                        if (dsCatch.Tables[0].Rows.Count > 0)
                                        {
                                            intClass = Int32.Parse(dsCatch.Tables[0].Rows[0]["classid"].ToString());
                                            intEnv = Int32.Parse(dsCatch.Tables[0].Rows[0]["environmentid"].ToString());
                                            intAddress = Int32.Parse(dsCatch.Tables[0].Rows[0]["addressid"].ToString());
                                        }
                                    }
                                    int intModel = 0;
                                    if (oAsset.Get(intAsset, "modelid") != "")
                                        intModel = Int32.Parse(oAsset.Get(intAsset, "modelid"));
                                    if (intModel > 0)
                                    {
                                        string strSerial = oAsset.Get(intAsset, "serial");
                                        string strSerialDR = "";
                                        if (intAssetDR > 0)
                                            strSerialDR = oAsset.Get(intAssetDR, "serial");

                                        #region RR
                                        int intRequest = oRequest.Add(-1, intUser);
                                        oServiceRequest.Add(intRequest, 1, 1);
                                        int intItem = oService.GetItemId(intService);
                                        int intNumber = 1;
                                        // Add Services
                                        oService.UpdateSelected(intRequest, intService, intNumber, 1);
                                        // Load Forms        
                                        oRequestItem.AddForm(intRequest, intItem, intService, intNumber);
                                        oCustomized.AddDecommissionServer(intRequest, intItem, intNumber, strName, intServer, datOff, strChange, strReason, intClass, intEnv, intAddress, intModel, strSerial, strSerialDR, 0, "", "", "");
                                        oRequestItem.UpdateForm(intRequest, true);
                                        #endregion

                                        #region CP
                                        oServers.UpdateDecommissioned(intServer, datOff.ToString());
                                        //if (oModelsProperties.IsTypeVMware(intModel) == true)
                                        //    boolIsServerVMWare = true;
                                        bool boolUnique = oAsset.AddDecommission(intRequest, intItem, intNumber, intAsset, intUser, strReason, datOff, strName, 0, "");
                                        if (boolUnique == false)
                                            strImportDecom += "Duplicate (Non-DR)!";
                                        else
                                        {
                                            bool boolDone = false;
                                            oServers.UpdateAssetDecom(intServer, intAsset, datOff.ToString());
                                            if (intAssetDR > 0)
                                            {
                                                boolUnique = oAsset.AddDecommission(intRequest, intItem, intNumber, intAssetDR, intUser, strReason, datOff, strName + "-DR", 1, "");
                                                if (boolUnique == false)
                                                    strImportDecom += "Duplicate (DR)!";
                                                else
                                                {
                                                    oServers.UpdateAssetDecom(intServer, intAssetDR, datOff.ToString());
                                                    boolDone = true;
                                                }
                                            }
                                            else
                                                boolDone = true;

                                            if (boolDone == true)
                                            {
                                                oAsset.UpdateDecommission(intRequest, intItem, intNumber, 1);
                                                strImportDecom += "OK [CVT" + intRequest.ToString() + "]!";
                                                intCount++;
                                            }
                                        }
                                        #endregion
                                    }
                                    else
                                        strImportDecom += "The modelID of assetID [" + intAsset.ToString() + "] is not valid!";
                                }
                                else
                                    strImportDecom += "Could not find an associated asset record (assetid = 0)!";
                            }
                            else
                                strImportDecom += "Could not find a server record (serverid = 0)!";
                        }
                        catch
                        {
                            strImportDecom += "Power off date [" + strOff + "] is not a valid datetime!";
                        }
                    }
                    else
                    {
                        strImportDecom += "User [" + strUser + "] does not exist!";
                    }
                    strImportDecom += "<br/>";
                }
                if (File.Exists(strFile) == true)
                {
                    boolDelete = true;
                    File.Delete(strFile);
                }
            }
            lblImportDecom.Text = "<img src='/images/bigcheck.gif' border='0' align='absmiddle'/> <b>Finished!</b> ClearView added " + intCount.ToString() + " decommissions<br/>";
            lblImportDecom.Text += strImportDecom;
            if (boolDelete == true)
                lblImportDecom.Text += "<img src='/images/bigcheck.gif' border='0' align='absmiddle'/> ClearView deleted the import file " + strFile;
            else
                lblImportDecom.Text += "<img src='/images/bigerror.gif' border='0' align='absmiddle'/> ClearView DID NOT delete the import file " + strFile;
            Page.ClientScript.RegisterStartupScript(typeof(Page), "anchoring", "<script type=\"text/javascript\">location.href='#decoms'<" + "/" + "script>");
        }
        protected void btnDataPoint_Click(Object Sender, EventArgs e)
        {
            DataPoint oDataPoint = new DataPoint(0, dsn);
            int intUser = Int32.Parse(txtDataPoint.Text);
            DataSet ds = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT DISTINCT [key] FROM cv_datapoint_fields");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (oDataPoint.GetFieldPermission(intUser, dr["key"].ToString()) == false)
                {
                    oDataPoint.AddFieldPermission(intUser, dr["key"].ToString());
                    intCount++;
                }
            }
            lblDataPoint.Text = "<img src='/images/bigcheck.gif' border='0' align='absmiddle'/> ClearView added the user to " + intCount.ToString() + " fields";
        }
        protected void btnNotifyImplementor_Click(Object Sender, EventArgs e)
        {
            Forecast oForecast = new Forecast(0, dsn);
            int intAnswer = Int32.Parse(txtNotifyImplementor.Text);
            int intModel = oForecast.GetModelAsset(intAnswer);
            if (intModel == 0)
                intModel = oForecast.GetModel(intAnswer);
            bool boolNotify = oForecast.NotifyImplementor(intAnswer, intModel, intImplementorDistributed, intWorkstationPlatform, intImplementorMidrange, intEnvironment, -999, dsnAsset, dsnIP);
            if (boolNotify == true)
                lblNotifyImplementor.Text = "<img src='/images/bigcheck.gif' border='0' align='absmiddle'/> Implementor notified";
            else
                lblNotifyImplementor.Text = "<img src='/images/bigalert.gif' border='0' align='absmiddle'/> Implementor already notified";
        }
        public void btnPNCDNS_Click(Object Sender, EventArgs e)
        {
            OnDemandTasks oOnDemandTasks = new OnDemandTasks(0, dsn);
            ResourceRequest oResourceRequest = new ResourceRequest(0, dsn);
            ServiceRequests oServiceRequest = new ServiceRequests(0, dsn);
            Services oService = new Services(0, dsn);
            ServiceDetails oServiceDetail = new ServiceDetails(0, dsn);
            Servers oServer = new Servers(0, dsn);
            Forecast oForecast = new Forecast(0, dsn);
            int intAnswer = Int32.Parse(txtPNCDNS.Text);
            int intModel = 0;
            int intRequest = oForecast.GetRequestID(intAnswer, true);
            DataSet ds = oServer.GetAnswer(intAnswer);
            if (ds.Tables[0].Rows.Count > 0)
            {
                intModel = Int32.Parse(ds.Tables[0].Rows[0]["id"].ToString());
                intRequest = Int32.Parse(ds.Tables[0].Rows[0]["requestid"].ToString());
            }
            int intDNSService = Int32.Parse(ConfigurationManager.AppSettings["SERVICEID_PNC_DNS"]);
            bool boolAssignNew = true;
            if (oOnDemandTasks.GetServerOther(intDNSService, intAnswer).Tables[0].Rows.Count > 0)
                boolAssignNew = false;
            if (boolAssignNew == true)
            {
                int intDNSItem = oService.GetItemId(intDNSService);
                int intDNSNumber = oResourceRequest.GetNumber(intRequest, intDNSItem);
                oOnDemandTasks.AddServerOther(intRequest, intDNSService, intDNSNumber, intAnswer, intModel);
                double dblDNSHours = oServiceDetail.GetHours(intDNSService, 1);
                int intDNS = oResourceRequest.Add(intRequest, intDNSItem, intDNSService, intDNSNumber, "Auto-Provisioning Task (DNS)", 1, dblDNSHours, 2, 1, 1, 1);
                if (oServiceRequest.NotifyApproval(intDNS, intResourceRequestApprove, intEnvironment, "", dsnServiceEditor) == false)
                    oServiceRequest.NotifyTeamLead(intDNSItem, intDNS, intAssignPage, intViewPage, intEnvironment,  "", dsnServiceEditor, dsnAsset, dsnIP, 0);
                lblPNCDNS.Text = "<img src='/images/bigcheck.gif' border='0' align='absmiddle'/> Request generated";
            }
            else
                lblPNCDNS.Text = "<img src='/images/bigalert.gif' border='0' align='absmiddle'/> Request already generated";
        }
        public void btnGetID_Click(Object Sender, EventArgs e)
        {
            AD oAD = new AD(0, dsn, intEnvironment);    // CORPDMN
            bool boolPNC = false;
            string strID = txtGetID.Text.Trim().ToUpper();
            SearchResultCollection oResults = oAD.Search(strID, "sAMAccountName");
            if (oResults.Count == 0)
            {
                boolPNC = true;
                oResults = oAD.Search(strID, "extensionattribute10");
            }

            if (oResults.Count == 1)
            {
                SearchResult oResult = oResults[0];
                if (boolPNC == false)
                {
                    if (oResult.Properties.Contains("sAMAccountName") == true)
                        strID = oResult.GetDirectoryEntry().Properties["sAMAccountName"].Value.ToString();
                    if (oResult.Properties.Contains("extensionattribute10") == true)
                    {
                        string strPNCID = oResult.GetDirectoryEntry().Properties["extensionattribute10"].Value.ToString();
                        lblGetID.Text = "The NCC ID you entered (" + strID + ") has a PNC ID of " + strPNCID + ".";
                    }
                    else
                        lblGetID.Text = "The NCC ID you entered (" + strID + ") does not have a PNC ID attribute configured.";
                }
                else
                {
                    if (oResult.Properties.Contains("sAMAccountName") == true)
                    {
                        string strXID = oResult.GetDirectoryEntry().Properties["sAMAccountName"].Value.ToString();
                        lblGetID.Text = "The PNC ID you entered (" + strID + ") has a NCC ID of " + strXID + ".";
                    }
                    else
                        lblGetID.Text = "The PNC ID you entered (" + strID + ") does not have a NCC ID attribute configured.";
                }
            }
            else if (oResults.Count > 1)
            {
                lblGetID.Text = "There were " + oResults.Count.ToString() + " accounts found for the account " + strID + ". Please try again.";
            }
            else
                lblGetID.Text = "<img src='/images/bigalert.gif' border='0' align='absmiddle'/> ID not found";
            Page.ClientScript.RegisterStartupScript(typeof(Page), "anchoring_ids", "<script type=\"text/javascript\">location.href='#IDs'<" + "/" + "script>");
        }
        
        public void btnDecrypt_Click(Object Sender, EventArgs e)
        {
            Encryption oEncrypt = new Encryption();
            lblDecrypt.Text = oEncrypt.Decrypt(txtDecrypt1.Text, txtDecrypt2.Text);
        }
        public void btnDecryptQ_Click(Object Sender, EventArgs e)
        {
            Functions oFunction = new Functions(0, dsn, intEnvironment);
            lblDecryptQ.Text = oFunction.decryptQueryString(txtDecryptQ.Text);
        }
        public void btnQIPDNSCreate_Click(Object Sender, EventArgs e)
        {
            Variables oVariable = new Variables(intEnvironment);
            System.Net.NetworkCredential oCredentials = new System.Net.NetworkCredential(oVariable.ADUser(), oVariable.ADPassword(), oVariable.Domain());
            ClearViewWebServices oWS = new ClearViewWebServices();
            oWS.Timeout = Int32.Parse(ConfigurationManager.AppSettings["WS_TIMEOUT"]);
            oWS.Credentials = oCredentials;
            oWS.Url = oVariable.WebServiceURL();
            lblQIPDNSResult.Text = "<img src='/images/bigcheck.gif' border='0' align='absmiddle'/> " + oWS.CreateDNSforPNC(txtQIPDNSIP.Text, txtQIPDNSName.Text, "Server", "", oVariable.DNS_Domain(), oVariable.DNS_NameService(), oVariable.DNS_DynamicDNSUpdate(), -999, 0, true);
            Page.ClientScript.RegisterStartupScript(typeof(Page), "anchoring_dns", "<script type=\"text/javascript\">location.href='#qip'<" + "/" + "script>");
        }
        public void btnQIPDNSUpdate_Click(Object Sender, EventArgs e)
        {
            Variables oVariable = new Variables(intEnvironment);
            System.Net.NetworkCredential oCredentials = new System.Net.NetworkCredential(oVariable.ADUser(), oVariable.ADPassword(), oVariable.Domain());
            ClearViewWebServices oWS = new ClearViewWebServices();
            oWS.Timeout = Int32.Parse(ConfigurationManager.AppSettings["WS_TIMEOUT"]);
            oWS.Credentials = oCredentials;
            oWS.Url = oVariable.WebServiceURL();
            lblQIPDNSResult.Text = "<img src='/images/bigcheck.gif' border='0' align='absmiddle'/> " + oWS.UpdateDNSforPNC(txtQIPDNSIP.Text, txtQIPDNSName.Text, "Server", "", oVariable.DNS_Domain(), oVariable.DNS_NameService(), oVariable.DNS_DynamicDNSUpdate(), -999, 0, true);
            Page.ClientScript.RegisterStartupScript(typeof(Page), "anchoring_dns", "<script type=\"text/javascript\">location.href='#qip'<" + "/" + "script>");
        }
        public void btnQIPDNSDelete_Click(Object Sender, EventArgs e)
        {
            Variables oVariable = new Variables(intEnvironment);
            System.Net.NetworkCredential oCredentials = new System.Net.NetworkCredential(oVariable.ADUser(), oVariable.ADPassword(), oVariable.Domain());
            ClearViewWebServices oWS = new ClearViewWebServices();
            oWS.Timeout = Int32.Parse(ConfigurationManager.AppSettings["WS_TIMEOUT"]);
            oWS.Credentials = oCredentials;
            oWS.Url = oVariable.WebServiceURL();
            lblQIPDNSResult.Text = "<img src='/images/bigcheck.gif' border='0' align='absmiddle'/> " + oWS.DeleteDNSforPNC(txtQIPDNSIP.Text, txtQIPDNSName.Text, -999, true);
            Page.ClientScript.RegisterStartupScript(typeof(Page), "anchoring_dns", "<script type=\"text/javascript\">location.href='#qip'<" + "/" + "script>");
        }
        public void btnQIPDNSSearch_Click(Object Sender, EventArgs e)
        {
            Variables oVariable = new Variables(intEnvironment);
            System.Net.NetworkCredential oCredentials = new System.Net.NetworkCredential(oVariable.ADUser(), oVariable.ADPassword(), oVariable.Domain());
            ClearViewWebServices oWS = new ClearViewWebServices();
            oWS.Timeout = Int32.Parse(ConfigurationManager.AppSettings["WS_TIMEOUT"]);
            oWS.Credentials = oCredentials;
            oWS.Url = oVariable.WebServiceURL();
            lblQIPDNSResult.Text = "<img src='/images/bigcheck.gif' border='0' align='absmiddle'/> " + oWS.SearchDNSforPNC(txtQIPDNSIP.Text, txtQIPDNSName.Text, chkQIPDNSAlias.Checked, true);
            Page.ClientScript.RegisterStartupScript(typeof(Page), "anchoring_dns", "<script type=\"text/javascript\">location.href='#qip'<" + "/" + "script>");
        }
        public void btnBlueCatDNSCreate_Click(Object Sender, EventArgs e)
        {
            Variables oVariable = new Variables(intEnvironment);
            System.Net.NetworkCredential oCredentials = new System.Net.NetworkCredential(oVariable.ADUser(), oVariable.ADPassword(), oVariable.Domain());
            ClearViewWebServices oWS = new ClearViewWebServices();
            oWS.Timeout = Int32.Parse(ConfigurationManager.AppSettings["WS_TIMEOUT"]);
            oWS.Credentials = oCredentials;
            oWS.Url = oVariable.WebServiceURL();

            lblBlueCat.Text = "<img src='/images/bigcheck.gif' border='0' align='absmiddle'/> " + oWS.CreateBluecatDNS(txtBlueCatIP.Text, txtBlueCatName.Text, txtBlueCatDescription.Text, "");
            Page.ClientScript.RegisterStartupScript(typeof(Page), "anchoring_dns", "<script type=\"text/javascript\">location.href='#bluecat'<" + "/" + "script>");
        }
        public void btnBlueCatDNSUpdate_Click(Object Sender, EventArgs e)
        {
            Variables oVariable = new Variables(intEnvironment);
            System.Net.NetworkCredential oCredentials = new System.Net.NetworkCredential(oVariable.ADUser(), oVariable.ADPassword(), oVariable.Domain());
            ClearViewWebServices oWS = new ClearViewWebServices();
            oWS.Timeout = Int32.Parse(ConfigurationManager.AppSettings["WS_TIMEOUT"]);
            oWS.Credentials = oCredentials;
            oWS.Url = oVariable.WebServiceURL();
            lblBlueCat.Text = "<img src='/images/bigcheck.gif' border='0' align='absmiddle'/> " + oWS.UpdateBluecatDNS(txtBlueCatIP.Text, txtBlueCatName.Text, txtBlueCatDescription.Text, "");
            Page.ClientScript.RegisterStartupScript(typeof(Page), "anchoring_dns", "<script type=\"text/javascript\">location.href='#bluecat'<" + "/" + "script>");
        }
        public void btnBlueCatDNSDelete_Click(Object Sender, EventArgs e)
        {
            Variables oVariable = new Variables(intEnvironment);
            System.Net.NetworkCredential oCredentials = new System.Net.NetworkCredential(oVariable.ADUser(), oVariable.ADPassword(), oVariable.Domain());
            ClearViewWebServices oWS = new ClearViewWebServices();
            oWS.Timeout = Int32.Parse(ConfigurationManager.AppSettings["WS_TIMEOUT"]);
            oWS.Credentials = oCredentials;
            oWS.Url = oVariable.WebServiceURL();
            lblBlueCat.Text = "<img src='/images/bigcheck.gif' border='0' align='absmiddle'/> " + oWS.DeleteBluecatDNS(txtBlueCatIP.Text, txtBlueCatName.Text, chkBlueCatDelete.Checked, false);
            Page.ClientScript.RegisterStartupScript(typeof(Page), "anchoring_dns", "<script type=\"text/javascript\">location.href='#bluecat'<" + "/" + "script>");
        }
        public void btnBlueCatDNSSearch_Click(Object Sender, EventArgs e)
        {
            Variables oVariable = new Variables(intEnvironment);
            System.Net.NetworkCredential oCredentials = new System.Net.NetworkCredential(oVariable.ADUser(), oVariable.ADPassword(), oVariable.Domain());
            ClearViewWebServices oWS = new ClearViewWebServices();
            oWS.Timeout = Int32.Parse(ConfigurationManager.AppSettings["WS_TIMEOUT"]);
            oWS.Credentials = oCredentials;
            oWS.Url = oVariable.WebServiceURL();
            lblBlueCat.Text = "<img src='/images/bigcheck.gif' border='0' align='absmiddle'/> " + oWS.SearchBluecatDNS(txtBlueCatIP.Text, txtBlueCatName.Text);
            Page.ClientScript.RegisterStartupScript(typeof(Page), "anchoring_dns", "<script type=\"text/javascript\">location.href='#bluecat'<" + "/" + "script>");
        }
        protected void btnImportDNS_Click(Object Sender, EventArgs e)
        {
            bool boolDelete = false;
            string strFile = "";
            StringBuilder oResult = new StringBuilder();
            if (filImportDNS.PostedFile != null && filImportDNS.PostedFile.FileName != "")
            {
                Variables oVariable = new Variables(intEnvironment);
                bool boolDNS_QIP = oSetting.IsDNS_QIP();
                bool boolDNS_Bluecat = oSetting.IsDNS_Bluecat();
                System.Net.NetworkCredential oCredentials = new System.Net.NetworkCredential(oVariable.ADUser(), oVariable.ADPassword(), oVariable.Domain());
                ClearViewWebServices oWS = new ClearViewWebServices();
                oWS.Timeout = 600000;   // 10 minutes
                oWS.Credentials = oCredentials;
                oWS.Url = oVariable.WebServiceURL();

                DateTime _now = DateTime.Now;
                string strNow = _now.Day.ToString() + _now.Month.ToString() + _now.Year.ToString() + _now.Hour.ToString() + _now.Minute.ToString() + _now.Second.ToString() + _now.Millisecond.ToString();
                strFile = Request.PhysicalApplicationPath + "\\admin\\imports\\" + strNow + ".xls";
                filImportDNS.PostedFile.SaveAs(strFile);
                string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strFile + ";Extended Properties=Excel 8.0;";
                OleDbDataAdapter myCommand = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", strConn);
                DataSet ds = new DataSet();
                myCommand.Fill(ds, "ExcelInfo");
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr[0].ToString().Trim() == "")
                        break;
                    string strName = dr[0].ToString().Trim();
                    string strIP = dr[1].ToString().Trim();
                    bool boolOverwrite = (dr[2].ToString().Trim().ToUpper() == "YES");
                    oResult.Append(strName);
                    oResult.Append(", ");
                    oResult.Append(strIP);
                    oResult.Append(" = ");

                    string strResult = "";
                    if (boolDNS_QIP == true)
                    {
                        strResult = oWS.CreateDNSforPNC(strIP, strName, "Server", "", oVariable.DNS_Domain(), oVariable.DNS_NameService(), oVariable.DNS_DynamicDNSUpdate(), -999, 0, true);
                        if (strResult.StartsWith("***CONFLICT") == true && boolOverwrite)
                            strResult = oWS.UpdateDNSforPNC(strIP, strName, "Server", "", oVariable.DNS_Domain(), oVariable.DNS_NameService(), oVariable.DNS_DynamicDNSUpdate(), -999, 0, true);
                    }
                    if (boolDNS_Bluecat == true)
                    {
                        strResult = oWS.CreateBluecatDNS(strIP, strName, strName, "");
                        if (strResult.StartsWith("***CONFLICT") == true && boolOverwrite)
                            strResult = oWS.UpdateBluecatDNS(strIP, strName, strName, "");
                    }

                    if (strResult == "SUCCESS")
                        oResult.Append("SUCCESS");
                    else if (strResult.StartsWith("***DUPLICATE") == true)
                        oResult.Append("DUPLICATE");
                    else if (strResult.StartsWith("***CONFLICT") == true)
                        oResult.Append("CONFLICT (Overwrite = " + boolOverwrite.ToString() + ")");
                    else
                    {
                        oResult.Append("ERROR: ");
                        oResult.Append(strResult);
                    }
                    oResult.Append("<br/>");
                    intCount++;
                }
                if (File.Exists(strFile) == true)
                {
                    boolDelete = true;
                    File.Delete(strFile);
                }
                lblImportDNS.Text = "<img src='/images/bigcheck.gif' border='0' align='absmiddle'/> <b>Finished!</b> ClearView imported " + intCount.ToString() + " records<br/><p>" + oResult.ToString() + "</p>";
                if (boolDelete == true)
                    lblImportDNS.Text += "<img src='/images/bigcheck.gif' border='0' align='absmiddle'/> ClearView deleted the file " + strFile;
                else
                    lblImportDNS.Text += "<img src='/images/bigerror.gif' border='0' align='absmiddle'/> ClearView DID NOT delete the file " + strFile;
            }
            else
                lblImportDNS.Text = "<img src='/images/bigerror.gif' border='0' align='absmiddle'/> <b>Error!</b> Please select a file";
            Page.ClientScript.RegisterStartupScript(typeof(Page), "anchoring_dns", "<script type=\"text/javascript\">location.href='#dns_import'<" + "/" + "script>");
        }
        public void btnSwitch_Click(Object Sender, EventArgs e)
        {
            Functions oFunction = new Functions(0, dsn, intEnvironment);
            txtSwitchResult.Text = oFunction.ChangeVLAN(txtSwitchName.Text, txtSwitchSerial.Text, "", txtSwitchPort.Text, txtSwitchVLAN.Text, true, boolSwitchEmptyMac, boolSwitchArpLookup, chkSwitch.Checked, true);
            txtSwitchResult.Visible = true;
        }
        public void btnNexus_Click(Object Sender, EventArgs e)
        {
            Variables oVariable = new Variables(intEnvironment);
            Asset oAsset = new Asset(intProfile, dsnAsset, dsn);
            SshShell oSSHshell = new SshShell(txtNexusSwitch.Text, oVariable.NexusUsername(), oVariable.NexusPassword());
            oSSHshell.RemoveTerminalEmulationCharacters = true;
            oSSHshell.Connect();
            string strLogin = oAsset.GetDellSwitchportOutput(oSSHshell);
            if (strLogin == "**INVALID**")
                txtNexus.Text = "Invalid Login";
            else
            {
                if (radNexusSearch.Checked == true)
                    txtNexus.Text = oAsset.GetDellSwitchportOutput(oSSHshell, txtNexusInterface.Text, DellBladeSwitchportType.Config, 0);
                else
                {
                    string strError = oAsset.ChangeDellSwitchport(oSSHshell, txtNexusInterface.Text, (radNexusAccess.Checked ? DellBladeSwitchportMode.Access : (radNexusTrunk.Checked ? DellBladeSwitchportMode.Trunk : DellBladeSwitchportMode.Shutdown)), txtNexusVLANs.Text, (txtNexusNative.Text == "0" ? null : txtNexusNative.Text), txtNexusDescription.Text, chkNexus.Checked, 0);
                    if (strError == "")
                        txtNexus.Text = "SUCCESS!" + Environment.NewLine + oAsset.GetDellSwitchportOutput(oSSHshell, txtNexusInterface.Text, DellBladeSwitchportType.Config, 0);
                    else
                        txtNexus.Text = strError;
                }
            }
            txtNexus.Visible = true;
            Page.ClientScript.RegisterStartupScript(typeof(Page), "anchoring_dns", "<script type=\"text/javascript\">location.href='#nexus'<" + "/" + "script>");
        }
        protected void btnProjectImport_Click(Object Sender, EventArgs e)
        {
            bool boolDelete = false;
            string strFile = "";
            StringBuilder oResult = new StringBuilder();
            if (filImportProject.PostedFile != null && filImportProject.PostedFile.FileName != "")
            {
                if (filImportProject.PostedFile.FileName.ToUpper().EndsWith("CSV"))
                {
                    Projects oProject = new Projects(0, dsn);
                    Forecast oForecast = new Forecast(0, dsn);
                    Requests oRequest = new Requests(0, dsn);
                    // Queue for import (status = 999)
                    oProject.Import();
                    DateTime _now = DateTime.Now;
                    string strNow = _now.Day.ToString() + _now.Month.ToString() + _now.Year.ToString() + _now.Hour.ToString() + _now.Minute.ToString() + _now.Second.ToString() + _now.Millisecond.ToString();
                    strFile = Request.PhysicalApplicationPath + "\\admin\\imports\\" + strNow + ".csv";
                    filImportProject.PostedFile.SaveAs(strFile);
                    using (StreamReader ProjectImport = new StreamReader(File.OpenRead(strFile)))
                    {
                        while (!ProjectImport.EndOfStream)
                        {
                            string import = ProjectImport.ReadLine();
                            string[] fields = import.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                            string strNumber = fields[0].ToString().Trim();     // Project Number
                            if (String.IsNullOrEmpty(strNumber) == false && strNumber.ToUpper() != "ID")
                            {
                                string strName = fields[1].ToString().Trim();       // Project Name
                                string strManager = fields[2].ToString().Trim();    // Project Manager
                                strManager += ", " + fields[3].ToString().Trim();
                                string strDate = fields[4].ToString().Trim();       // Created Date
                                string strStatus = fields[5].ToString().Trim();     // Approved [Yes, No]
                                if (strStatus.ToUpper() == "YES")
                                {
                                    strStatus = fields[6].ToString().Trim();        // Status [Active (Requested), Cancelled, Complete, On Hold]
                                    int intStatus = 2;  // Active
                                    if (strStatus.ToUpper() == "CANCELLED")
                                        intStatus = -2;
                                    else if (strStatus.ToUpper() == "COMPLETE")
                                        intStatus = 3;
                                    else if (strStatus.ToUpper() == "ON HOLD")
                                        intStatus = 5;

                                    oResult.Append(strName);
                                    oResult.Append(", ");
                                    oResult.Append(strNumber);
                                    oResult.Append(", ");
                                    oResult.Append(strManager);
                                    oResult.Append(", ");
                                    oResult.Append(strStatus);
                                    oResult.Append(" = ");
                                    int intProject = 0;
                                    DataSet dsProject = oProject.Get(strNumber.Trim());
                                    if (dsProject.Tables[0].Rows.Count > 0)
                                    {
                                        // Update information
                                        DataRow drProject = dsProject.Tables[0].Rows[0];
                                        intProject = Int32.Parse(drProject["projectid"].ToString());
                                        if (drProject["name"].ToString() != strName || drProject["manager"].ToString() != strManager || drProject["created"].ToString() != strDate)
                                        {
                                            oResult.Append("updated");
                                            intCount++;
                                        }
                                    }
                                    else
                                    {
                                        oResult.Append("added");
                                        intCount++;
                                    }
                                    intProject = oProject.Import(intProject, strNumber, strName, strManager, strDate, intStatus);
                                    oResult.Append("<br/>");
                                }
                            }
                        }
                    }

                    // Check those that were not in the list (status = 999)
                    DataSet ds = oProject.Imports();
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        int intProject = Int32.Parse(dr["projectid"].ToString());
                        bool boolClosed = false;
                        DateTime datLatest = DateTime.MinValue;
                        if (oProject.IsTest(intProject) == false)
                        {
                            DataSet dsForecast = oForecast.GetProject(intProject);
                            foreach (DataRow drForecast in dsForecast.Tables[0].Rows)
                            {
                                int intForecast = Int32.Parse(drForecast["id"].ToString());
                                DataSet dsAnswer = oForecast.GetAnswers(intForecast);
                                foreach (DataRow drAnswer in dsAnswer.Tables[0].Rows)
                                {
                                    DateTime datCreated = DateTime.Parse(drAnswer["created"].ToString());
                                    if (datCreated > datLatest)
                                        datLatest = datCreated;
                                }
                            }
                            TimeSpan oSpan = DateTime.Now.Subtract(datLatest);
                            if (dsForecast.Tables[0].Rows.Count == 0 || oSpan.Days > 730)
                            {
                                // Last activity was more than 2 years ago - check requests
                                DataSet dsRequest = oRequest.Gets(intProject);
                                foreach (DataRow drRequest in dsRequest.Tables[0].Rows)
                                {
                                    DateTime datCreated = DateTime.Parse(drRequest["created"].ToString());
                                    if (datCreated > datLatest)
                                        datLatest = datCreated;
                                }
                                oSpan = DateTime.Now.Subtract(datLatest);
                                if (dsRequest.Tables[0].Rows.Count == 0 || oSpan.Days > 730)
                                {
                                    // Mark completed.
                                    boolClosed = true;
                                }
                            }
                        }
                        if (boolClosed == true)
                        {
                            oProject.Update(intProject, 3);
                            oResult.Append(dr["name"].ToString());
                            oResult.Append(", ");
                            oResult.Append(dr["number"].ToString());
                            oResult.Append(", ");
                            oResult.Append(datLatest.ToString());
                            oResult.Append(" = <b>*** CLOSED ****</b>");
                            oResult.Append("<br/>");
                        }
                        else
                            oProject.Update(intProject, 2);
                    }
                    if (File.Exists(strFile) == true)
                    {
                        boolDelete = true;
                        File.Delete(strFile);
                    }
                    lblImportProject.Text = "<img src='/images/bigcheck.gif' border='0' align='absmiddle'/> <b>Finished!</b> ClearView imported " + intCount.ToString() + " records<br/><p>" + oResult.ToString() + "</p>";
                    if (boolDelete == true)
                        lblImportProject.Text += "<img src='/images/bigcheck.gif' border='0' align='absmiddle'/> ClearView deleted the file " + strFile;
                    else
                        lblImportProject.Text += "<img src='/images/bigerror.gif' border='0' align='absmiddle'/> ClearView DID NOT delete the file " + strFile;
                }
                else
                    lblImportProject.Text = "<img src='/images/bigerror.gif' border='0' align='absmiddle'/> <b>Error!</b> Please select a CSV file";
            }
            else
                lblImportProject.Text = "<img src='/images/bigerror.gif' border='0' align='absmiddle'/> <b>Error!</b> Please select a CSV file";
            Page.ClientScript.RegisterStartupScript(typeof(Page), "anchoring_projects", "<script type=\"text/javascript\">location.href='#import_project'<" + "/" + "script>");
        }
        protected void btnProjectExport_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path + "?export=true");
        }
        protected void btnImportCostCenter_Click(Object Sender, EventArgs e)
        {
            bool boolDelete = false;
            string strFile = "";
            StringBuilder oResult = new StringBuilder();
            if (filImportCostCenter.PostedFile != null && filImportCostCenter.PostedFile.FileName != "")
            {
                CostCenter oCostCenter = new CostCenter(0, dsn);
                oCostCenter.Delete();
                DateTime _now = DateTime.Now;
                string strNow = _now.Day.ToString() + _now.Month.ToString() + _now.Year.ToString() + _now.Hour.ToString() + _now.Minute.ToString() + _now.Second.ToString() + _now.Millisecond.ToString();
                strFile = Request.PhysicalApplicationPath + "\\admin\\imports\\" + strNow + ".txt";
                filImportCostCenter.PostedFile.SaveAs(strFile);
                //StreamReader oRead = File.OpenText(strFile);
                StreamReader oRead = new StreamReader(strFile);
                while (oRead.Peek() != -1)
                {
                    string strLine = oRead.ReadLine();
                    string[] strSplit = strLine.Split(new char[] { '~' });
                    oCostCenter.Add(strSplit[1], strSplit[12], strSplit[13]);
                    intCount++;
                    //if (intCount > 100)
                        //break;
                }
                oRead.Close();
                if (File.Exists(strFile) == true)
                {
                    boolDelete = true;
                    File.Delete(strFile);
                }
                lblImportCostCenter.Text = "<img src='/images/bigcheck.gif' border='0' align='absmiddle'/> <b>Finished!</b> ClearView imported " + intCount.ToString() + " records<br/><p>" + oResult.ToString() + "</p>";
                if (boolDelete == true)
                    lblImportCostCenter.Text += "<img src='/images/bigcheck.gif' border='0' align='absmiddle'/> ClearView deleted the file " + strFile;
                else
                    lblImportCostCenter.Text += "<img src='/images/bigerror.gif' border='0' align='absmiddle'/> ClearView DID NOT delete the file " + strFile;
            }
            else
                lblImportCostCenter.Text = "<img src='/images/bigerror.gif' border='0' align='absmiddle'/> <b>Error!</b> Please select a file";
            Page.ClientScript.RegisterStartupScript(typeof(Page), "anchoring_costs", "<script type=\"text/javascript\">location.href='#import_cost'<" + "/" + "script>");
        }
        protected void btnAssociateDesign_Click(Object Sender, EventArgs e)
        {
            Forecast oForecast = new Forecast(0, dsn);
            Servers oServer = new Servers(0, dsn);
            Classes oClass = new Classes(0, dsn);

            panAssociateDesign.Visible = true;
            btnAssociateDesign2.Enabled = true;

            int intAnswer = Int32.Parse(txtAssociateDesign.Text);
            int intClass = 0;
            if (Int32.TryParse(oForecast.GetAnswer(intAnswer, "classid"), out intClass) == true)
            {
                bool boolProd = (oClass.IsProd(intClass));
                bool boolQA = (oClass.IsQA(intClass));
                bool boolTest = (oClass.IsTest(intClass));
                bool boolDR = (oClass.IsDR(intClass));
                bool boolDev = (boolProd == false && boolQA == false && boolTest == false && boolDR == false);

                lblAssociateDesignClass.Text = (boolProd ? "PROD" : (boolQA ? "QA" : (boolTest ? "TEST" : (boolDev ? "DEV" : (boolDR ? "DR" : "Unknown")))));
                lblAssociateDesignDR.Text = (boolDR ? "" : "Serial # of DR Asset:");

                //if (oForecast.GetAnswer(intAnswer, "executed") == "")
                //{
                    DataSet dsServer = oServer.GetAnswer(intAnswer);
                    intAssociateDesignDevices = dsServer.Tables[0].Rows.Count;
                    if (intAssociateDesignDevices > 0)
                    {
                        lblAssociateDesignResult.Text = "If you would like, you can associate the following " + intAssociateDesignDevices.ToString() + " record(s)...";
                        int intAssociateDesignDeviceCount = 0;
                        foreach (DataRow drServer in dsServer.Tables[0].Rows)
                        {
                            intAssociateDesignDeviceCount++;
                            int intServer = Int32.Parse(drServer["id"].ToString());
                            if (intAssociateDesignDeviceCount == 1)
                                LoadAssociateDesign(intServer, txtAssociateDesignProd1, txtAssociateDesignDR1, txtAssociateDesignName1, lblAssociateDesign1, boolProd);
                            else if (intAssociateDesignDeviceCount == 2)
                                LoadAssociateDesign(intServer, txtAssociateDesignProd2, txtAssociateDesignDR2, txtAssociateDesignName2, lblAssociateDesign2, boolProd);
                            else if (intAssociateDesignDeviceCount == 3)
                                LoadAssociateDesign(intServer, txtAssociateDesignProd3, txtAssociateDesignDR3, txtAssociateDesignName3, lblAssociateDesign3, boolProd);
                            else if (intAssociateDesignDeviceCount == 4)
                                LoadAssociateDesign(intServer, txtAssociateDesignProd4, txtAssociateDesignDR4, txtAssociateDesignName4, lblAssociateDesign4, boolProd);
                            else if (intAssociateDesignDeviceCount == 5)
                                LoadAssociateDesign(intServer, txtAssociateDesignProd5, txtAssociateDesignDR5, txtAssociateDesignName5, lblAssociateDesign5, boolProd);
                            else if (intAssociateDesignDeviceCount == 6)
                                LoadAssociateDesign(intServer, txtAssociateDesignProd6, txtAssociateDesignDR6, txtAssociateDesignName6, lblAssociateDesign6, boolProd);
                            else if (intAssociateDesignDeviceCount == 7)
                                LoadAssociateDesign(intServer, txtAssociateDesignProd7, txtAssociateDesignDR7, txtAssociateDesignName7, lblAssociateDesign7, boolProd);
                            else if (intAssociateDesignDeviceCount == 8)
                                LoadAssociateDesign(intServer, txtAssociateDesignProd8, txtAssociateDesignDR8, txtAssociateDesignName8, lblAssociateDesign8, boolProd);
                            else if (intAssociateDesignDeviceCount == 9)
                                LoadAssociateDesign(intServer, txtAssociateDesignProd9, txtAssociateDesignDR9, txtAssociateDesignName9, lblAssociateDesign9, boolProd);
                            else if (intAssociateDesignDeviceCount == 10)
                                LoadAssociateDesign(intServer, txtAssociateDesignProd10, txtAssociateDesignDR10, txtAssociateDesignName10, lblAssociateDesign10, boolProd);
                            else if (intAssociateDesignDeviceCount == 11)
                                LoadAssociateDesign(intServer, txtAssociateDesignProd11, txtAssociateDesignDR11, txtAssociateDesignName11, lblAssociateDesign11, boolProd);
                            else if (intAssociateDesignDeviceCount == 12)
                                LoadAssociateDesign(intServer, txtAssociateDesignProd12, txtAssociateDesignDR12, txtAssociateDesignName12, lblAssociateDesign12, boolProd);
                            else if (intAssociateDesignDeviceCount == 13)
                                LoadAssociateDesign(intServer, txtAssociateDesignProd13, txtAssociateDesignDR13, txtAssociateDesignName13, lblAssociateDesign13, boolProd);
                            else if (intAssociateDesignDeviceCount == 14)
                                LoadAssociateDesign(intServer, txtAssociateDesignProd14, txtAssociateDesignDR14, txtAssociateDesignName14, lblAssociateDesign14, boolProd);
                            else if (intAssociateDesignDeviceCount == 15)
                                LoadAssociateDesign(intServer, txtAssociateDesignProd15, txtAssociateDesignDR15, txtAssociateDesignName15, lblAssociateDesign15, boolProd);
                            else if (intAssociateDesignDeviceCount == 16)
                                LoadAssociateDesign(intServer, txtAssociateDesignProd16, txtAssociateDesignDR16, txtAssociateDesignName16, lblAssociateDesign16, boolProd);
                            else if (intAssociateDesignDeviceCount == 17)
                                LoadAssociateDesign(intServer, txtAssociateDesignProd17, txtAssociateDesignDR17, txtAssociateDesignName17, lblAssociateDesign17, boolProd);
                            else if (intAssociateDesignDeviceCount == 18)
                                LoadAssociateDesign(intServer, txtAssociateDesignProd18, txtAssociateDesignDR18, txtAssociateDesignName18, lblAssociateDesign18, boolProd);
                            else if (intAssociateDesignDeviceCount == 19)
                                LoadAssociateDesign(intServer, txtAssociateDesignProd19, txtAssociateDesignDR19, txtAssociateDesignName19, lblAssociateDesign19, boolProd);
                            else if (intAssociateDesignDeviceCount == 20)
                                LoadAssociateDesign(intServer, txtAssociateDesignProd20, txtAssociateDesignDR20, txtAssociateDesignName20, lblAssociateDesign20, boolProd);

                        }
                    }
                    else
                    {
                        lblAssociateDesignResult.Text = "There are no servers associated with that design";
                        btnAssociateDesign2.Enabled = false;
                    }
                //}
                //else
                //{
                //    lblAssociateDesignResult.Text = "This design has already been executed";
                //    btnAssociateDesign2.Enabled = false;
                //}
            }
            else
            {
                lblAssociateDesignResult.Text = "Invalid Design ID";
                btnAssociateDesign2.Enabled = false;
            }
            Page.ClientScript.RegisterStartupScript(typeof(Page), "anchoring_associate_assets", "<script type=\"text/javascript\">location.href='#associate_assets'<" + "/" + "script>");
        }
        private void LoadAssociateDesign(int _serverid, TextBox _prod, TextBox _dr, TextBox _name, Label _result, bool _is_prod)
        {
            Servers oServer = new Servers(0, dsn);
            Asset oAsset = new Asset(0, dsnAsset, dsn);
            // Lookup Assets
            int intAssetLatest = 0;
            int intAssetDR = 0;
            DataSet dsAssets = oServer.GetAssets(_serverid);
            foreach (DataRow drAsset in dsAssets.Tables[0].Rows)
            {
                if (drAsset["latest"].ToString() == "1")
                    intAssetLatest = Int32.Parse(drAsset["assetid"].ToString());
                else if (drAsset["dr"].ToString() == "1")
                    intAssetDR = Int32.Parse(drAsset["assetid"].ToString());
            }
            if (Int32.Parse(oServer.Get(_serverid, "step")) > 1)
            {
                btnAssociateDesign2.Enabled = false;
                btnAssociateDesign2.Width = Unit.Pixel(175);
                btnAssociateDesign2.Text = "Past Assignment Step";
            }
            else
            {
                btnAssociateDesign2.Enabled = true;
                btnAssociateDesign2.Width = Unit.Pixel(75);
                btnAssociateDesign2.Text = "Associate";
            }
            // Lookup Name
            _prod.Text = oAsset.Get(intAssetLatest, "serial");
            if (_is_prod)
                _dr.Text = oAsset.Get(intAssetDR, "serial");
            else
            {
                _dr.Enabled = false;
                _dr.Text = "N / A";
            }
            _name.Text = oServer.GetName(_serverid, true);
            _result.Text = "";
        }
        protected void btnAssociateDesign2_Click(Object Sender, EventArgs e) 
        {
            Servers oServer = new Servers(0, dsn);
            int intAnswer = Int32.Parse(txtAssociateDesign.Text);
            DataSet dsServer = oServer.GetAnswer(intAnswer);
            intAssociateDesignDevices = dsServer.Tables[0].Rows.Count;
            int intAssociateDesignDeviceCount = 0;
            foreach (DataRow drServer in dsServer.Tables[0].Rows)
            {
                intAssociateDesignDeviceCount++;
                int intServer = Int32.Parse(drServer["id"].ToString());
                if (intAssociateDesignDeviceCount == 1)
                    SaveAssociateDesign(intServer, txtAssociateDesignProd1, txtAssociateDesignDR1, txtAssociateDesignName1, lblAssociateDesign1);
                else if (intAssociateDesignDeviceCount == 2)
                    SaveAssociateDesign(intServer, txtAssociateDesignProd2, txtAssociateDesignDR2, txtAssociateDesignName2, lblAssociateDesign2);
                else if (intAssociateDesignDeviceCount == 3)
                    SaveAssociateDesign(intServer, txtAssociateDesignProd3, txtAssociateDesignDR3, txtAssociateDesignName3, lblAssociateDesign3);
                else if (intAssociateDesignDeviceCount == 4)
                    SaveAssociateDesign(intServer, txtAssociateDesignProd4, txtAssociateDesignDR4, txtAssociateDesignName4, lblAssociateDesign4);
                else if (intAssociateDesignDeviceCount == 5)
                    SaveAssociateDesign(intServer, txtAssociateDesignProd5, txtAssociateDesignDR5, txtAssociateDesignName5, lblAssociateDesign5);
                else if (intAssociateDesignDeviceCount == 6)
                    SaveAssociateDesign(intServer, txtAssociateDesignProd6, txtAssociateDesignDR6, txtAssociateDesignName6, lblAssociateDesign6);
                else if (intAssociateDesignDeviceCount == 7)
                    SaveAssociateDesign(intServer, txtAssociateDesignProd7, txtAssociateDesignDR7, txtAssociateDesignName7, lblAssociateDesign7);
                else if (intAssociateDesignDeviceCount == 8)
                    SaveAssociateDesign(intServer, txtAssociateDesignProd8, txtAssociateDesignDR8, txtAssociateDesignName8, lblAssociateDesign8);
                else if (intAssociateDesignDeviceCount == 9)
                    SaveAssociateDesign(intServer, txtAssociateDesignProd9, txtAssociateDesignDR9, txtAssociateDesignName9, lblAssociateDesign9);
                else if (intAssociateDesignDeviceCount == 10)
                    SaveAssociateDesign(intServer, txtAssociateDesignProd10, txtAssociateDesignDR10, txtAssociateDesignName10, lblAssociateDesign10);
                else if (intAssociateDesignDeviceCount == 11)
                    SaveAssociateDesign(intServer, txtAssociateDesignProd11, txtAssociateDesignDR11, txtAssociateDesignName11, lblAssociateDesign11);
                else if (intAssociateDesignDeviceCount == 12)
                    SaveAssociateDesign(intServer, txtAssociateDesignProd12, txtAssociateDesignDR12, txtAssociateDesignName12, lblAssociateDesign12);
                else if (intAssociateDesignDeviceCount == 13)
                    SaveAssociateDesign(intServer, txtAssociateDesignProd13, txtAssociateDesignDR13, txtAssociateDesignName13, lblAssociateDesign13);
                else if (intAssociateDesignDeviceCount == 14)
                    SaveAssociateDesign(intServer, txtAssociateDesignProd14, txtAssociateDesignDR14, txtAssociateDesignName14, lblAssociateDesign14);
                else if (intAssociateDesignDeviceCount == 15)
                    SaveAssociateDesign(intServer, txtAssociateDesignProd15, txtAssociateDesignDR15, txtAssociateDesignName15, lblAssociateDesign15);
                else if (intAssociateDesignDeviceCount == 16)
                    SaveAssociateDesign(intServer, txtAssociateDesignProd16, txtAssociateDesignDR16, txtAssociateDesignName16, lblAssociateDesign16);
                else if (intAssociateDesignDeviceCount == 17)
                    SaveAssociateDesign(intServer, txtAssociateDesignProd17, txtAssociateDesignDR17, txtAssociateDesignName17, lblAssociateDesign17);
                else if (intAssociateDesignDeviceCount == 18)
                    SaveAssociateDesign(intServer, txtAssociateDesignProd18, txtAssociateDesignDR18, txtAssociateDesignName18, lblAssociateDesign18);
                else if (intAssociateDesignDeviceCount == 19)
                    SaveAssociateDesign(intServer, txtAssociateDesignProd19, txtAssociateDesignDR19, txtAssociateDesignName19, lblAssociateDesign19);
                else if (intAssociateDesignDeviceCount == 20)
                    SaveAssociateDesign(intServer, txtAssociateDesignProd20, txtAssociateDesignDR20, txtAssociateDesignName20, lblAssociateDesign20);
            }
            Page.ClientScript.RegisterStartupScript(typeof(Page), "anchoring_associate_assets", "<script type=\"text/javascript\">location.href='#associate_assets'<" + "/" + "script>");
        }
        private void SaveAssociateDesign(int _serverid, TextBox _prod, TextBox _dr, TextBox _name, Label _result)
        {
            Servers oServer = new Servers(0, dsn);
            ServerName oServerName = new ServerName(0, dsn);
            Forecast oForecast = new Forecast(0, dsn);
            Asset oAsset = new Asset(0, dsnAsset, dsn);
            int intAnswer = Int32.Parse(txtAssociateDesign.Text);
            int intAssetClass = Int32.Parse(oForecast.GetAnswer(intAnswer, "classid"));
            int intAssetEnv = Int32.Parse(oForecast.GetAnswer(intAnswer, "environmentid"));
            // Lookup Assets
            int intAssetLatest = 0;
            int intAssetDR = 0;
            DataSet dsAssets = oServer.GetAssets(_serverid);
            foreach (DataRow drAsset in dsAssets.Tables[0].Rows)
            {
                if (drAsset["latest"].ToString() == "1")
                    intAssetLatest = Int32.Parse(drAsset["assetid"].ToString());
                else if (drAsset["dr"].ToString() == "1")
                    intAssetDR = Int32.Parse(drAsset["assetid"].ToString());
            }
            _result.Text = "";

            string strProd = _prod.Text.Trim().ToUpper();
            if (strProd != "")
            {
                DataSet dsAsset = oAsset.Get(strProd);
                if (dsAsset.Tables[0].Rows.Count == 1)
                {
                    int intAsset = Int32.Parse(dsAsset.Tables[0].Rows[0]["id"].ToString());
                    if (intAsset != intAssetLatest)
                    {
                        if (dsAsset.Tables[0].Rows[0]["status"].ToString() == "2")
                        {
                            oServer.AddAsset(_serverid, intAsset, intAssetClass, intAssetEnv, 0, 0);
                            oAsset.AddStatus(intAsset, "Pre-Configured for Design # " + intAnswer.ToString(), (int)AssetStatus.Reserved, -999, DateTime.Now);
                        }
                        else
                        {
                            _result.Text = "The asset " + strProd + " is not set to AVAILABLE...";
                            return;
                        }
                    }
                }
                else
                {
                    _result.Text = "The asset " + strProd + " returned " + dsAsset.Tables[0].Rows.Count.ToString() + " record(s)...";
                    return;
                }
            }
            string strDR = _dr.Text.Trim().ToUpper();
            if (strDR != "" && strDR != "N / A")
            {
                DataSet dsAsset = oAsset.Get(strDR);
                if (dsAsset.Tables[0].Rows.Count == 1)
                {
                    int intAsset = Int32.Parse(dsAsset.Tables[0].Rows[0]["id"].ToString());
                    if (intAsset != intAssetDR)
                    {
                        if (dsAsset.Tables[0].Rows[0]["status"].ToString() == "2")
                        {
                            oServer.AddAsset(_serverid, intAsset, intAssetClass, intAssetEnv, 0, 1);
                            oAsset.AddStatus(intAsset, "Pre-Configured for Design # " + intAnswer.ToString(), (int)AssetStatus.Reserved, -999, DateTime.Now);
                        }
                        else
                        {
                            _result.Text = "The asset " + strDR + " is not set to AVAILABLE...";
                            return;
                        }
                    }
                }
                else
                {
                    _result.Text = "The asset " + strDR + " returned " + dsAsset.Tables[0].Rows.Count.ToString() + " record(s)...";
                    return;
                }
            }
            string strName = _name.Text.Trim().ToUpper();
            if (strName != "")
            {
                int intName = 0;
                if (strName.Length == 12)
                {
                    // National City
                    intName = oServerName.GetName(strName);
                    if (intName == 0)
                    {
                        string _prefix1 = strName.Substring(0, 5);
                        string _prefix2 = strName.Substring(5, 3);
                        string _sitecode = strName.Substring(8, 2);
                        string _name1 = strName.Substring(10, 1);
                        string _name2 = strName.Substring(11, 1);
                        //_result.Text = _prefix1 + "," + _prefix2 + "," + _sitecode + "," + _name1 + "," + _name2;
                        intName = oServerName.Add(0, _prefix1, _prefix2, _sitecode, _name1, _name2, -999, "AdminConfigure", 0);
                    }
                }
                else
                {
                    // PNC
                    intName = oServerName.GetNameFactory(strName);
                    if (intName == 0)
                    {
                        string _os = strName.Substring(0, 1);
                        string _location = strName.Substring(1, 1);
                        string _mnemonic = strName.Substring(2, 3);
                        string _environment = strName.Substring(5, 1);
                        string _name1 = strName.Substring(6, 1);
                        string _name2 = strName.Substring(7, 1);
                        string _func = strName.Substring(8, 1);
                        string _specific = "";
                        if (strName.Length > 9)
                            _specific = strName.Substring(9, 1);
                        //_result.Text = _os + "," + _location + "," + _mnemonic + "," + _environment + "," + _name1 + "," + _name2 + "," + _func + "," + _specific;
                        intName = oServerName.AddFactory(_os, _location, _mnemonic, _environment, _name1, _name2, _func, _specific, -999, "AdminConfigure", 0);
                    }
                }
                if (intName > 0)
                    oServer.UpdateServerNamed(_serverid, intName);
                else
                    _result.Text = "There was a problem assigning the name " + strName.ToUpper() + " to the asset)...";
            }

            _result.CssClass = "default";
            if (_result.Text != "")
                _result.CssClass = "reddefault";

            if (_result.Text == "" && (strProd != "" || (strDR != "" && strDR != "N / A") || strName != ""))
                _result.Text = "SUCCESS!!";
        }
        protected void btnDecommissionSearch_Click(Object Sender, EventArgs e)
        {
            Functions oFunction = new Functions(0, dsn, intEnvironment);
            Servers oServer = new Servers(0, dsn);
            DataSet ds = oServer.GetDecommissionAll(txtDecommissionSearch.Text);
            panDecommissionSearch.Visible = false;
            lblDecommissionSearch.Text = "";
            if (ds.Tables[0].Rows.Count > 0)
            {
                lblDecomType.Text = ds.Tables[0].Rows[0]["type"].ToString();
                if (ds.Tables[0].Rows[0]["number"].ToString() != "0")
                    lblDecomRequestID.Text = ds.Tables[0].Rows[0]["requestid"].ToString();
                else
                    lblDecomRequestID.Text = "Entered in CMS";
                lblDecomSubmitter.Text = ds.Tables[0].Rows[0]["submitter"].ToString();
                lblDecomPoweroff.Text = ds.Tables[0].Rows[0]["poweroff"].ToString();
                lblDecomReason.Text = oFunction.FormatText(ds.Tables[0].Rows[0]["reason"].ToString());
                lblDecomChange.Text = ds.Tables[0].Rows[0]["change"].ToString();
                panDecommissionSearch.Visible = true;
            }
            else
                lblDecommissionSearch.Text += "<img src='/images/bigerror.gif' border='0' align='absmiddle'/> No decommission record";
            Page.ClientScript.RegisterStartupScript(typeof(Page), "decommission_search", "<script type=\"text/javascript\">location.href='#decommission_search'<" + "/" + "script>");
        }
        protected void btnDecommission_Click(Object Sender, EventArgs e)
        {
            Asset oAsset = new Asset(0, dsnAsset, dsn);
            Servers oServer = new Servers(0, dsn);
            Customized oCustomized = new Customized(0, dsn);
            Requests oRequest = new Requests(0, dsn);
            DateTime datDate = DateTime.Now;
            int intUser = 0;
            if (DateTime.TryParse(txtDecomDate.Text, out datDate) == true && Int32.TryParse(Request.Form["hdnAJAXValue"], out intUser) == true && intUser > 0)
            {
                int intRequest = oRequest.Add(0, intUser);
                string[] servers = txtDecomServers.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string server in servers)
                {
                    if (String.IsNullOrEmpty(server) == false)
                    {
                        lblDecommission.Text += server + "...";
                        DataSet ds = oServer.GetDecommission(server);
                        if (ds.Tables[0].Rows.Count == 1)
                        {
                            int intServer = Int32.Parse(ds.Tables[0].Rows[0]["id"].ToString());
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
                                lblDecommission.Text += "asset record found, ";
                                oAsset.AddStatus(intAsset, server.ToUpper(), (radDecomDecom.Checked ? -1 : -10), intUser, datDate);
                                if (intAssetDR > 0)
                                    oAsset.AddStatus(intAssetDR, server.ToUpper() + "-DR", (radDecomDecom.Checked ? -1 : -10), intUser, datDate);
                                lblDecommission.Text += "updated...";
                            }
                            else
                                lblDecommission.Text += "NO asset record found...";

                            lblDecommission.Text += "server record found, ";
                            oServer.UpdateDecommissioned(intServer, datDate.ToShortDateString());
                            oServer.UpdateAssetDecom(intServer, intAsset, datDate.ToShortDateString());
                            oCustomized.AddDecommissionServer(intRequest, 0, 0, server, intServer, datDate, txtDecomPTM.Text, txtDecomReason.Text, 0, 0, 0, 0, oAsset.Get(intAsset, "serial"), oAsset.Get(intAssetDR, "serial"), 0, "", "", "");
                            if (intAssetDR > 0)
                                oServer.UpdateAssetDecom(intServer, intAssetDR, datDate.ToShortDateString());
                            lblDecommission.Text += "updated<br/>";
                        }
                    }
                }
            }
            lblDecommission.Text += "<img src='/images/bigcheck.gif' border='0' align='absmiddle'/> Finished with decommissions";
            Page.ClientScript.RegisterStartupScript(typeof(Page), "decommission", "<script type=\"text/javascript\">location.href='#decommission'<" + "/" + "script>");
        }
        protected void btnClearDR_Click(Object Sender, EventArgs e)
        {
            int intAnswer = Int32.Parse(txtClearDR.Text);
            Forecast oForecast = new Forecast(0, dsn);
            oForecast.UpdateAnswerRecovery(intAnswer, 0);
            lblClearDR.Text += "<img src='/images/bigcheck.gif' border='0' align='absmiddle'/> ClearView cleared the DR flag for Design ID " + intAnswer.ToString();
            Page.ClientScript.RegisterStartupScript(typeof(Page), "anchoring_dns", "<script type=\"text/javascript\">location.href='#remove_dr'<" + "/" + "script>");
        }
        protected void btnAudit_Click(Object Sender, EventArgs e)
        {
            int intAnswer = Int32.Parse(txtAudit.Text);
            int intService = Int32.Parse(txtAuditService.Text);
            Servers oServer = new Servers(0, dsn);

            DataSet dsServer = oServer.GetAnswer(intAnswer);
            bool boolAudit = true;
            foreach (DataRow drServer in dsServer.Tables[0].Rows)
            {
                if (drServer["step"].ToString() != "999" || drServer["dns_auto"].ToString() != "1" || drServer["dns_auto"].ToString() != "1")
                {
                    boolAudit = false;
                    break;
                }
            }
            if (boolAudit == true)
            {
                Forecast oForecast = new Forecast(0, dsn);
                OnDemandTasks oOnDemandTasks = new OnDemandTasks(0, dsn);
                Services oService = new Services(0, dsn);
                ResourceRequest oResourceRequest = new ResourceRequest(0, dsn);
                ServiceDetails oServiceDetail = new ServiceDetails(0, dsn);
                ServiceRequests oServiceRequest = new ServiceRequests(0, dsn);
                Users oUser = new Users(0, dsn);
                int intModel = oForecast.GetModelAsset(intAnswer);
                if (intModel == 0)
                    intModel = oForecast.GetModel(intAnswer);
                int intRequest = oForecast.GetRequestID(intAnswer, true);
                int intImplementorUser = 0;
                string strImplementor = "";
                if (chkAudit.Checked == true)
                {
                    //DataSet dsImplementor = oOnDemandTasks.GetPending(intAnswer);
                    //if (dsImplementor.Tables[0].Rows.Count > 0)
                    //{
                    //    int intImplementorResource = Int32.Parse(dsImplementor.Tables[0].Rows[0]["resourceid"].ToString());
                    //    intImplementorUser = Int32.Parse(oResourceRequest.GetWorkflow(intImplementorResource, "userid"));
                    //    strImplementor = oUser.GetName(intImplementorUser);
                    //}
                }
                bool boolAssignNew = true;
                if (oOnDemandTasks.GetServerOther(intService, intAnswer).Tables[0].Rows.Count > 0)
                    boolAssignNew = false;
                if (boolAssignNew == true)
                {
                    int intItem = oService.GetItemId(intService);
                    int intNumber = oResourceRequest.GetNumber(intRequest, intItem);
                    oOnDemandTasks.AddServerOther(intRequest, intService, intNumber, intAnswer, intModel);
                    double dblServiceHours = oServiceDetail.GetHours(intService, 1);
                    int intRR = oServiceRequest.AddRequest(intRequest, intItem, intService, 1, dblServiceHours, 2, intNumber, dsnServiceEditor);
                    oServiceRequest.NotifyTeamLead(intItem, intRR, intAssignPage, intViewPage, intEnvironment,  strImplementor, dsnServiceEditor, dsnAsset, dsnIP, intImplementorUser);
                    lblAudit.Text = "<img src='/images/bigcheck.gif' border='0' align='absmiddle'/> Service generated";
                }
                else
                    lblAudit.Text = "<img src='/images/bigalert.gif' border='0' align='absmiddle'/> Service already generated";
            }
            else
                lblAudit.Text = "<img src='/images/bigerror.gif' border='0' align='absmiddle'/> Either one or more of the servers in this design are not completed, not a PNC build, or the DNS auto flag is not set to 1 (meaning the DNS did not complete successfully)";
        }
        public void btnPreProd_Click(Object Sender, EventArgs e)
        {
            int intAnswer = Int32.Parse(txtPreProd.Text);
            Servers oServer = new Servers(0, dsn);
            PNCTasks oPNCTask = new PNCTasks(0, dsn);
            OnDemandTasks oOnDemandTask = new OnDemandTasks(0, dsn);
            ResourceRequest oResourceRequest = new ResourceRequest(0, dsn);

            DataSet dsServers = oServer.GetAnswer(intAnswer);
            bool boolReady = true;
            foreach (DataRow drServer in dsServers.Tables[0].Rows)
            {
                if (drServer["build_ready"].ToString() == "")
                {
                    boolReady = false;
                    break;
                }
            }
            if (boolReady == false)
            {
                // Set build ready flag
                foreach (DataRow drServer in dsServers.Tables[0].Rows)
                    oServer.UpdateBuildReady(Int32.Parse(drServer["id"].ToString()), DateTime.Now.ToString(), false);

                // Update all open tasks to completed
                DataSet dsRR = oPNCTask.GetAnswer(intAnswer);
                foreach (DataRow drRR in dsRR.Tables[0].Rows)
                {
                    if (drRR["completed"].ToString() != "")
                    {
                        int intRequest = 0;
                        int intItem = 0;
                        int intService = 0;
                        int intNumber = 0;
                        int intRR = Int32.Parse(drRR["rrid"].ToString());
                        DataSet dsResourceRequest = oResourceRequest.Get(intRR);
                        if (dsResourceRequest.Tables[0].Rows.Count > 0) 
                        {
                            Int32.TryParse(dsResourceRequest.Tables[0].Rows[0]["requestid"].ToString(), out intRequest);
                            Int32.TryParse(dsResourceRequest.Tables[0].Rows[0]["itemid"].ToString(), out intItem);
                            Int32.TryParse(dsResourceRequest.Tables[0].Rows[0]["serviceid"].ToString(), out intService);
                            Int32.TryParse(dsResourceRequest.Tables[0].Rows[0]["number"].ToString(), out intNumber);
                        }
                        int intRRW = Int32.Parse(drRR["rrwid"].ToString());
                        switch (drRR["type"].ToString())
                        {
                            case "OTHER":
                                oOnDemandTask.UpdateServerOtherComplete(intRequest, intService, intNumber);
                                oResourceRequest.UpdateWorkflowStatus(intRRW, 3, true);
                                break;
                            case "BACKUP":
                                oOnDemandTask.UpdateServerBackupComplete(intRequest, intItem, intNumber);
                                oResourceRequest.UpdateWorkflowStatus(intRRW, 3, true);
                                break;
                            case "STORAGE":
                                oOnDemandTask.UpdateServerStorageComplete(intRequest, intItem, intNumber);
                                oResourceRequest.UpdateWorkflowStatus(intRRW, 3, true);
                                break;
                        }
                    }
                }
                lblPreProd.Text = "<img src='/images/bigcheck.gif' border='0' align='absmiddle'/> Finished.";
            }
            else
                lblPreProd.Text = "<img src='/images/bigalert.gif' border='0' align='absmiddle'/> Build Ready Flag already set for all servers in this design";
        }
        public void btnDatabaseFieldData_Click(Object Sender, EventArgs e)
        {
            string strDSN = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings[ddlDatabaseFieldData.SelectedItem.Value]].ConnectionString;
            bool boolError = false;
            DataSet dsT = SqlHelper.ExecuteDataset(strDSN, CommandType.Text, "select name from sys.tables order by name");
            foreach (DataRow drT in dsT.Tables[0].Rows)
            {
                if (boolError)
                    break;
                string strTable = drT["name"].ToString();
                DataSet dsS = SqlHelper.ExecuteDataset(strDSN, CommandType.Text, "select c.name, c.max_length AS length, c.collation_name AS collation_c, t.name AS type, t.collation_name AS collation_t from sys.columns c LEFT OUTER JOIN sys.types t ON t.system_type_id = c.system_type_id AND t.user_type_id <= 255 where c.object_id =object_id('" + strTable + "')");
                foreach (DataRow drS in dsS.Tables[0].Rows)
                {
                    if (boolError)
                        break;
                    string strField = drS["name"].ToString().ToUpper();
                    string strSQL = txtDatabaseFieldData.Text;
                    string strType = drS["type"].ToString().ToUpper();
                    SqlParameter[] arParamsData = new SqlParameter[1];
                    bool boolInt = false;
                    int intTemp = 0;
                    Int32.TryParse(strSQL, out intTemp);
                    bool boolDatetime = false;
                    DateTime datToday = DateTime.Today;
                    try { datToday = DateTime.Parse(strSQL); }
                    catch { datToday = DateTime.Today; }
                    bool boolText = false;
                    if (strType == "DATETIME" || strType == "VARCHAR" || strType == "CHAR" || strType == "NVARCHAR" || strType == "NCHAR" || strType == "TEXT")
                    {
                        if (strSQL.Contains("'") == true)
                            arParamsData[0] = new SqlParameter("@value", strSQL.Replace("'", "''"));
                        else
                            arParamsData[0] = new SqlParameter("@value", strSQL);

                        if (strType == "DATETIME")
                        {
                            boolDatetime = true;
                            arParamsData[0].SqlDbType = SqlDbType.DateTime;
                        }
                        else if (strType == "VARCHAR")
                            arParamsData[0].SqlDbType = SqlDbType.VarChar;
                        else if (strType == "CHAR")
                            arParamsData[0].SqlDbType = SqlDbType.Char;
                        else if (strType == "NVARCHAR")
                            arParamsData[0].SqlDbType = SqlDbType.NVarChar;
                        else if (strType == "NCHAR")
                            arParamsData[0].SqlDbType = SqlDbType.NChar;
                        else if (strType == "TEXT")
                        {
                            boolText = true;
                            arParamsData[0].SqlDbType = SqlDbType.VarChar;
                        }
                    }
                    else
                    {
                        boolInt = true;
                        arParamsData[0] = new SqlParameter("@value", strSQL);
                        if (strType == "BIGINT")
                            arParamsData[0].SqlDbType = SqlDbType.BigInt;
                        else
                            arParamsData[0].SqlDbType = SqlDbType.Int;
                    }
                    if ((intTemp != 0 || boolInt == false) && (datToday != DateTime.Today || boolDatetime == false))
                    {
                        string strExecute = "SELECT * FROM " + strTable + " WHERE [" + strField + "] " + (boolInt == false && boolDatetime == false ? "LIKE '%' + " : " = ") + " @value" + (boolInt == false && boolDatetime == false ? " + '%'" : "");
                        try
                        {
                            //Response.Write(strExecute + ", value:" + strSQL + ", type:" + strType);
                            DataSet ds = SqlHelper.ExecuteDataset(strDSN, CommandType.Text, strExecute, arParamsData);
                            if (ds.Tables[0].Rows.Count > 0)
                                lblDatabaseFieldData.Text += strTable + "." + strField + " = " + ds.Tables[0].Rows.Count.ToString() + " records<br/>";
                        }
                        catch (Exception ex)
                        {
                            Response.Write(ex.Message + "<br/>");
                            Response.Write(strExecute + ", value:" + strSQL + ", dbtype:" + arParamsData[0].SqlDbType.ToString() + ", type:" + strType + "<br/>");
                            boolError = true;
                        }
                    }
                }
            }
            Page.ClientScript.RegisterStartupScript(typeof(Page), "anchoring", "<script type=\"text/javascript\">location.href='#DatabaseFieldData'<" + "/" + "script>");
        }
        protected void btnSetupAppDocs_Click(Object Sender, EventArgs e)
        {
            int intApplicationID = Int32.Parse(txtSetupAppDocs.Text);
            Functions oFunction = new Functions(0, dsn, intEnvironment);
            oFunction.ExecuteNonQuery("UPDATE cv_document_repository SET deleted = -999 WHERE applicationid = " + intApplicationID.ToString() + " and deleted = 0");
            Variables oVariable = new Variables(intEnvironment);
            int intFiles = LoadApplicationDocuments(oVariable.DocumentsFolder() + "department\\" + intApplicationID.ToString() + "\\", intApplicationID);
            lblSetupAppDocs.Text += "<img src='/images/bigcheck.gif' border='0' align='absmiddle'/> ClearView added a total of " + intFiles.ToString() + " files...";
            Page.ClientScript.RegisterStartupScript(typeof(Page), "anchoring_dns", "<script type=\"text/javascript\">location.href='#SetupAppDocs'<" + "/" + "script>");
        }
        protected int LoadApplicationDocuments(string strPath, int intApplicationID)
        {
            int intCount = 0;
            if (Directory.Exists(strPath))
            {
                Customized oCustomized = new Customized(0, dsn);

                // Directories
                DirectoryInfo oDirs = new DirectoryInfo(strPath);
                foreach (DirectoryInfo _directory in oDirs.GetDirectories())
                {
                    oCustomized.AddDocumentRepository(intApplicationID, -999, _directory.Name, "Folder", 1, _directory.FullName, _directory.Parent.FullName, 0);
                    intCount += LoadApplicationDocuments(_directory.FullName, intApplicationID);
                }

                // Files
                foreach (FileInfo _file in oDirs.GetFiles())
                {
                    oCustomized.AddDocumentRepository(intApplicationID, -999, _file.Name, _file.Extension, 1, _file.FullName, _file.Directory.FullName, (int)_file.Length);
                    intCount++;
                }
            }
            return intCount;
        }
        public void btnTEXT_Click(Object Sender, EventArgs e)
        {
            Functions oFunction = new Functions(0, ConfigurationManager.ConnectionStrings[ddlTEXT.SelectedItem.Value].ConnectionString, intEnvironment);
            try
            {
                oFunction.ExecuteSQL(txtTEXTValue.Text);
                lblTEXT.Text = "<img src='/images/bigcheck.gif' border='0' align='absmiddle'/> Success.";
            }
            catch (Exception exTEXT)
            {
                lblTEXT.Text = "<img src='/images/bigalert.gif' border='0' align='absmiddle'/> " + exTEXT.Message;
            }
        }
        protected void btnUpdateSettings_Click(Object Sender, EventArgs e)
        {
            Functions oFunction = new Functions(0, dsn, intEnvironment);

            if (String.IsNullOrEmpty(txtUpdateSettingsFreezeStart.Text))
                oFunction.ExecuteNonQuery("UPDATE cv_setting SET freeze_start = NULL");
            else
                oFunction.ExecuteNonQuery("UPDATE cv_setting SET freeze_start = '" + txtUpdateSettingsFreezeStart.Text + "'");

            if (String.IsNullOrEmpty(txtUpdateSettingsFreezeEnd.Text))
                oFunction.ExecuteNonQuery("UPDATE cv_setting SET freeze_end = NULL");
            else
                oFunction.ExecuteNonQuery("UPDATE cv_setting SET freeze_end = '" + txtUpdateSettingsFreezeEnd.Text + "'");

            if (String.IsNullOrEmpty(txtUpdateSettingsFreezeSkip.Text))
                oFunction.ExecuteNonQuery("UPDATE cv_setting SET freeze_skip_requestid = NULL");
            else
                oFunction.ExecuteNonQuery("UPDATE cv_setting SET freeze_skip_requestid = " + txtUpdateSettingsFreezeSkip.Text);

            if (String.IsNullOrEmpty(txtUpdateSettingsDecom.Text))
                oFunction.ExecuteNonQuery("UPDATE cv_setting SET decom_override_requestid = NULL");
            else
                oFunction.ExecuteNonQuery("UPDATE cv_setting SET decom_override_requestid = " + txtUpdateSettingsDecom.Text);

            if (String.IsNullOrEmpty(txtUpdateSettingsDestroy.Text))
                oFunction.ExecuteNonQuery("UPDATE cv_setting SET destroy_override_requestid = NULL");
            else
                oFunction.ExecuteNonQuery("UPDATE cv_setting SET destroy_override_requestid = " + txtUpdateSettingsDestroy.Text);

            lblUpdateSettings.Text = "<img src='/images/bigcheck.gif' border='0' align='absmiddle'/> <b>Finished!</b> ClearView updated the settings";
            Page.ClientScript.RegisterStartupScript(typeof(Page), "anchoring_costs", "<script type=\"text/javascript\">location.href='#settings'<" + "/" + "script>");
        }
        
    }
}
