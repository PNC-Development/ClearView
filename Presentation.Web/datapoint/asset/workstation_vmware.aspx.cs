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
using NCC.ClearView.Presentation.Web.Custom;
using Microsoft.ApplicationBlocks.Data;
using Vim25Api;
using System.Diagnostics;

namespace NCC.ClearView.Presentation.Web
{
    public partial class workstation_vmware : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        private string dsnZeus = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ZeusDSN"]].ConnectionString;
        private string dsnServiceEditor = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ServiceEditorDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);

        protected int intDataPointAvailableAsset = Int32.Parse(ConfigurationManager.AppSettings["DATAPOINT_AVAILABLE_ASSET"]);
        protected int intServiceDecommission = Int32.Parse(ConfigurationManager.AppSettings["SERVICEID_SERVER_DECOMMISSION"]);
        protected DataPoint oDataPoint;
        protected Users oUser;
        protected Servers oServer;
        protected Asset oAsset;
        protected Workstations oWorkstation;
        protected Forecast oForecast;
        protected Platforms oPlatform;
        protected Types oType;
        protected Models oModel;
        protected ModelsProperties oModelsProperties;
        protected IPAddresses oIPAddresses;
        protected Functions oFunction;
        protected OperatingSystems oOperatingSystem;
        protected ServicePacks oServicePack;
        protected Classes oClass;
        protected Environments oEnvironment;
        protected Domains oDomain;
        protected VirtualHDD oVirtualHDD;
        protected VirtualRam oVirtualRam;
        protected Zeus oZeus;
        protected Log oLog;
        protected Services oService;
        protected ServiceRequests oServiceRequest;
        protected ResourceRequest oResourceRequest;
        protected int intProfile = 0;
        protected int intApplication = 0;
        protected int intAsset = 0;
        protected string strMenuTab1 = "";
        protected string strAdministration = "";
        private int intID = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            if (Request.Cookies["profileid"] != null && Request.Cookies["profileid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            oDataPoint = new DataPoint(intProfile, dsn);
            oUser = new Users(intProfile, dsn);
            oServer = new Servers(intProfile, dsn);
            oAsset = new Asset(intProfile, dsnAsset);
            oWorkstation = new Workstations(intProfile, dsn);
            oForecast = new Forecast(intProfile, dsn);
            oPlatform = new Platforms(intProfile, dsn);
            oType = new Types(intProfile, dsn);
            oModel = new Models(intProfile, dsn);
            oModelsProperties = new ModelsProperties(intProfile, dsn);
            oIPAddresses = new IPAddresses(intProfile, dsnIP, dsn);
            oFunction = new Functions(intProfile, dsn, intEnvironment);
            oOperatingSystem = new OperatingSystems(intProfile, dsn);
            oServicePack = new ServicePacks(intProfile, dsn);
            oClass = new Classes(intProfile, dsn);
            oEnvironment = new Environments(intProfile, dsn);
            oDomain = new Domains(intProfile, dsn);
            oVirtualHDD = new VirtualHDD(intProfile, dsn);
            oVirtualRam = new VirtualRam(intProfile, dsn);
            oZeus = new Zeus(intProfile, dsnZeus);
            oLog = new Log(intProfile, dsn);
            oService = new Services(intProfile, dsn);
            oServiceRequest = new ServiceRequests(intProfile, dsn);
            oResourceRequest = new ResourceRequest(intProfile, dsn);

            if (oUser.IsAdmin(intProfile) == true || (oDataPoint.GetPagePermission(intApplication, "ASSET") == true || intDataPointAvailableAsset == 1))
            {
                panAllow.Visible = true;
                if (Request.QueryString["save"] != null)
                    panSave.Visible = true;
                if (Request.QueryString["error"] != null)
                {
                    panError.Visible = true;
                    //      -100: More than one device name
                    //       -10: No device names
                    //        -5: Improper Name Format
                    //        -1: ServerID = 0
                    if (Request.QueryString["error"] == "-100")
                        lblError.Text = "More than one name";
                    else if (Request.QueryString["error"] == "-10")
                        lblError.Text = "User Cancelled Prompt";
                    else if (Request.QueryString["error"] == "-5")
                        lblError.Text = "Name is in Incorrect Format";
                    else if (Request.QueryString["error"] == "-1")
                        lblError.Text = "DeviceID = 0";
                    else
                        lblError.Text = "Generic Error";
                }
                Int32.TryParse(oFunction.decryptQueryString(Request.QueryString["id"]), out intID);
                if (Request.QueryString["close"] != null)
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "close", "<script type=\"text/javascript\">window.close();<" + "/" + "script>");
                else if (intID > 0)
                {
                    DataSet ds = oDataPoint.GetAsset(intID);
                    if (ds.Tables[0].Rows.Count == 1)
                    {
                        // Load General Information
                        intAsset = Int32.Parse(ds.Tables[0].Rows[0]["id"].ToString());
                        lblAssetID.Text = "#" + intAsset.ToString();
                        string strSerial = ds.Tables[0].Rows[0]["serial"].ToString();
                        string strAsset = ds.Tables[0].Rows[0]["asset"].ToString();

                        string strHeader = (strSerial.Length > 15 ? strSerial.Substring(0, 15) + "..." : strSerial);
                        lblHeader.Text = "&quot;" + strHeader.ToUpper() + "&quot;";
                        Master.Page.Title = "DataPoint | VMware Workstation (" + strHeader + ")";
                        lblHeaderSub.Text = "Provides all the information about a VMware Workstation...";
                        int intMenuTab = 0;
                        if (Request.QueryString["menu_tab"] != null && Request.QueryString["menu_tab"] != "")
                            intMenuTab = Int32.Parse(Request.QueryString["menu_tab"]);
                        Tab oTab = new Tab(hdnTab.ClientID, intMenuTab, "divMenu1", true, false);
                        oTab.AddTab("Asset Information", "");
                        oTab.AddTab("Guest Information", "");
                        oTab.AddTab("Account Information", "");
                        oTab.AddTab("Resource Dependencies", "");
                        oTab.AddTab("Provisioning Information", "");
                        if (oUser.IsAdmin(intProfile) == true || oDataPoint.GetFieldPermission(intProfile, "SERVER_ADMIN") == true)
                        {
                            oTab.AddTab("Administration", "");
                            panAdministration.Visible = true;
                        }
                        strMenuTab1 = oTab.GetTabs();

                        if (!IsPostBack)
                        {
                            // Asset Information
                            oDataPoint.LoadTextBox(txtPlatformSerial, intProfile, null, "", lblPlatformSerial, fldPlatformSerial, "WORKSTATION_VMWARE_SERIAL", strSerial, "", false, true);
                            oDataPoint.LoadTextBox(txtPlatformAsset, intProfile, null, "", lblPlatformAsset, fldPlatformAsset, "WORKSTATION_VMWARE_ASSET", strAsset, "", false, true);
                            
                            int intAssetAttribute = Int32.Parse(oAsset.Get(intAsset, "asset_attribute"));
                            oDataPoint.LoadDropDown(ddlAssetAttribute, intProfile, null, "", lblAssetAttribute, fldAssetAttribute, "ASSET_ATTRIBUTE", "Name", "AttributeId", oAsset.getAssetAttributes(null, "", 1), intAssetAttribute, true, false, false);
                            oDataPoint.LoadTextBox(txtAssetAttributeComment, intProfile, null, "", lblAssetAttributeComment, fldAssetAttributeComment, "ASSET_ATTRIBUTE_COMMENT", oAsset.getAssetAttributesComments(intAsset), "", false, true);
                            ddlAssetAttribute.Attributes.Add("onclick", "return SetControlsForAssetAttributes()");

                            ddlPlatform.Attributes.Add("onchange", "PopulatePlatformTypes('" + ddlPlatform.ClientID + "','" + ddlPlatformType.ClientID + "','" + ddlPlatformModel.ClientID + "','" + ddlPlatformModelProperty.ClientID + "');ResetDropDownHidden('" + hdnModel.ClientID + "');");
                            ddlPlatformType.Attributes.Add("onchange", "PopulatePlatformModels('" + ddlPlatformType.ClientID + "','" + ddlPlatformModel.ClientID + "','" + ddlPlatformModelProperty.ClientID + "');ResetDropDownHidden('" + hdnModel.ClientID + "');");
                            ddlPlatformModel.Attributes.Add("onchange", "PopulatePlatformModelProperties('" + ddlPlatformModel.ClientID + "','" + ddlPlatformModelProperty.ClientID + "');ResetDropDownHidden('" + hdnModel.ClientID + "');");
                            ddlPlatformModelProperty.Attributes.Add("onchange", "UpdateDropDownHidden('" + ddlPlatformModelProperty.ClientID + "','" + hdnModel.ClientID + "');");
                            int intModel = Int32.Parse(oAsset.Get(intAsset, "modelid"));

                            hdnModel.Value = intModel.ToString();
                            int intModelParent = Int32.Parse(oModelsProperties.Get(intModel, "modelid"));
                            int intType = oModel.GetType(intModelParent);
                            string strExecute = oType.Get(intType, "forecast_execution_path");
                            if (strExecute != "")
                            {
                                DataSet dsVirtual = oWorkstation.GetVirtualAsset(intAsset);
                                if (dsVirtual.Tables[0].Rows.Count > 0)
                                    btnExecute.Attributes.Add("onclick", "return OpenWindow('FORECAST_EXECUTE','" + strExecute + "?id=" + dsVirtual.Tables[0].Rows[0]["answerid"].ToString() + "');");
                                else
                                    btnExecute.Enabled = false;
                            }
                            else
                                btnExecute.Enabled = false;
                            int intPlatform = oType.GetPlatform(intType);
                            oDataPoint.LoadDropDown(ddlPlatform, intProfile, null, "", lblPlatform, fldPlatform, "WORKSTATION_VMWARE_PLATFORM", "name", "platformid", oPlatform.Gets(1), intPlatform, false, false, true);
                            oDataPoint.LoadDropDown(ddlPlatformType, intProfile, null, "", lblPlatformType, fldPlatformType, "WORKSTATION_VMWARE_TYPE", "name", "id", oType.Gets(intPlatform, 1), intType, false, false, true);
                            oDataPoint.LoadDropDown(ddlPlatformModel, intProfile, null, "", lblPlatformModel, fldPlatformModel, "WORKSTATION_VMWARE_MODEL", "name", "id", oModel.Gets(intType, 1), intModelParent, false, false, true);
                            oDataPoint.LoadDropDown(ddlPlatformModelProperty, intProfile, null, "", lblPlatformModelProperty, fldPlatformModelProperty, "WORKSTATION_VMWARE_MODEL_PROP", "name", "id", oModelsProperties.GetModels(0, intModelParent, 1), intModel, false, false, true);

                            // Get Asset
                            int intWorkstation = 0;
                            DataSet dsAsset = oWorkstation.GetVirtualAsset(intAsset);
                            if (dsAsset.Tables[0].Rows.Count > 0)
                            {
                                intWorkstation = Int32.Parse(dsAsset.Tables[0].Rows[0]["id"].ToString());
                                lblWorkstation.Text = intWorkstation.ToString();
                                txtStep.Text = dsAsset.Tables[0].Rows[0]["step"].ToString();
                                oDataPoint.LoadTextBoxDeviceName(txtName, btnName, null, true, hdnPNC, intProfile, null, "", lblName, fldName, "WORKSTATION_VMWARE_NAME", dsAsset.Tables[0].Rows[0]["name"].ToString(), "", false, false);
                                if (txtName.Text != "")
                                    lblHeader.Text += "&nbsp;&nbsp;&nbsp;[" + txtName.Text + "]";
                                lblStatus.Text = dsAsset.Tables[0].Rows[0]["statusname"].ToString();
                                // Administrative Functions
                                if (Request.QueryString["admin"] != null)
                                {
                                    if (Request.QueryString["result"] != null)
                                        strAdministration = "<tr><td>" + oFunction.decryptQueryString(Request.QueryString["result"]) + "</td></tr>";
                                    if (Request.QueryString["output"] != null)
                                    {
                                        DataSet dsOutput = oWorkstation.GetVirtualOutput(intWorkstation);
                                        foreach (DataRow drOutput in dsOutput.Tables[0].Rows)
                                        {
                                            strAdministration += "<tr><td><a href=\"javascript:void(0);\" onclick=\"ShowHideDiv2('div" + drOutput["id"].ToString() + "');\">" + drOutput["type"].ToString() + "</a></td></tr>";
                                            strAdministration += "<tr id=\"div" + drOutput["id"].ToString() + "\" style=\"display:none\"><td>" + oFunction.FormatText(drOutput["output"].ToString()) + "</td></tr>";
                                        }
                                        if (lblStatus.Text != "")
                                            strAdministration += "<tr><td>" + oLog.GetEvents(oLog.GetEventsByName(txtName.Text.ToUpper(), (int)LoggingType.Error), intEnvironment) + "</td></tr>";
                                    }
                                }
                                int intClass = Int32.Parse(dsAsset.Tables[0].Rows[0]["classid"].ToString());
                                int intEnv = Int32.Parse(dsAsset.Tables[0].Rows[0]["environmentid"].ToString());
                                hdnEnvironment.Value = intEnv.ToString();
                                oDataPoint.LoadDropDown(ddlPlatformClass, intProfile, null, "", lblPlatformClass, fldPlatformClass, "WORKSTATION_VMWARE_CLASS", "name", "id", oClass.GetWorkstationVMwares(1), intClass, false, false, true);
                                oDataPoint.LoadDropDown(ddlPlatformEnvironment, intProfile, null, "", lblPlatformEnvironment, fldPlatformEnvironment, "WORKSTATION_VMWARE_ENVIRONMENT", "name", "id", oClass.GetEnvironment(intClass, 0), intEnv, false, false, true);
                                ddlStatus.SelectedValue = dsAsset.Tables[0].Rows[0]["status"].ToString();
                                ddlStatus.Enabled = (intAssetAttribute == (int)AssetAttribute.Ok);
                                panStatus.Visible = (ddlStatus.Enabled == false);

                                int intOS = Int32.Parse(dsAsset.Tables[0].Rows[0]["osid"].ToString());
                                bool boolXP = (oOperatingSystem.Get(intOS, "code") == "XP");
                                bool boolWin7 = (oOperatingSystem.Get(intOS, "code") == "7");
                                oDataPoint.LoadDropDown(ddlPlatformOS, intProfile, null, "", lblPlatformOS, fldPlatformOS, "WORKSTATION_VMWARE_OS", "name", "id", oOperatingSystem.Gets(1, 1), intOS, false, false, true);
                                oDataPoint.LoadDropDown(ddlPlatformServicePack, intProfile, null, "", lblPlatformServicePack, fldPlatformServicePack, "WORKSTATION_VMWARE_SP", "name", "id", oOperatingSystem.GetServicePack(intOS), Int32.Parse(dsAsset.Tables[0].Rows[0]["spid"].ToString()), false, false, true);
                                oDataPoint.LoadDropDown(ddlPlatformDomain, intProfile, null, "", lblPlatformDomain, fldPlatformDomain, "WORKSTATION_VMWARE_DOMAIN", "name", "id", oDomain.Gets(1), Int32.Parse(dsAsset.Tables[0].Rows[0]["domainid"].ToString()), false, false, true);
                                oDataPoint.LoadDropDown(ddlPlatformHDD, intProfile, null, "", lblPlatformHDD, fldPlatformHDD, "WORKSTATION_VMWARE_HDD", "name", "id", oVirtualHDD.GetVMwares((boolXP ? 1 : 0), (boolWin7 ? 1 : 0), 1), Int32.Parse(dsAsset.Tables[0].Rows[0]["hddid"].ToString()), false, false, true);
                                oDataPoint.LoadDropDown(ddlPlatformRam, intProfile, null, "", lblPlatformRam, fldPlatformRam, "WORKSTATION_VMWARE_RAM", "name", "id", oVirtualRam.GetVMwares((boolXP ? 1 : 0), (boolWin7 ? 1 : 0), 1), Int32.Parse(dsAsset.Tables[0].Rows[0]["ramid"].ToString()), false, false, true);
                            }
                            else
                                Response.Redirect("/datapoint/asset/datapoint_asset_search.aspx");

                            // Get Host
                            oDataPoint.LoadTextBox(txtHostName, intProfile, btnHostName, "/datapoint/asset/server.aspx?t=name&q=" + oFunction.encryptQueryString(dsAsset.Tables[0].Rows[0]["hostname"].ToString()), lblHostName, fldHostName, "WORKSTATION_VMWARE_HOST", dsAsset.Tables[0].Rows[0]["hostname"].ToString(), "", false, false);
                            if (Request.Cookies["virtual_guest"] != null && Request.Cookies["virtual_guest"].Value != "")
                            {
                                string strHost = "";
                                string strFind = txtName.Text;
                                //strFind = "ohcleapp103d";
                                DateTime datStart = DateTime.Parse(Request.Cookies["virtual_guest"].Value);
                                Response.Cookies["virtual_guest"].Value = "";
                                VMWare oVMWare = new VMWare(intProfile, dsn);
                                string strConnect = oVMWare.Connect(strFind);
                                VimService _service = oVMWare.GetService();
                                ServiceContent _sic = oVMWare.GetSic();
                                try
                                {
                                    ManagedObjectReference oVM = oVMWare.GetVM(strFind);
                                    ManagedObjectReference oParent = (ManagedObjectReference)oVMWare.getObjectProperty(oVM, "parent");
                                    ManagedObjectReference oDataCenter = (ManagedObjectReference)oVMWare.getObjectProperty(oParent, "parent");
                                    string strDataCenter = (string)oVMWare.getObjectProperty(oDataCenter, "name");
                                    GuestInfo ginfo = (GuestInfo)oVMWare.getObjectProperty(oVM, "guest");
                                    lblGuestState.Text = ginfo.guestState;
                                    GuestNicInfo[] nInfo = ginfo.net;
                                    foreach (GuestNicInfo nic in nInfo)
                                    {
                                        string[] strIPAddresses = nic.ipAddress;
                                        foreach (string strIPAddress in strIPAddresses)
                                        {
                                            if (lblIPAddress.Text != "")
                                                lblIPAddress.Text += ", ";
                                            lblIPAddress.Text += strIPAddress;
                                        }
                                        if (lblMACAddress.Text != "")
                                            lblMACAddress.Text += ", ";
                                        lblMACAddress.Text += nic.macAddress;
                                        if (lblNetwork.Text != "")
                                            lblNetwork.Text += ", ";
                                        lblNetwork.Text += nic.network;
                                    }
                                    // HDD
                                    GuestDiskInfo[] oVMDisks = ginfo.disk;
                                    foreach (GuestDiskInfo oDisk in oVMDisks)
                                    {
                                        if (lblHDD.Text != "")
                                            lblHDD.Text += "<br/>";
                                        double dblCapacity = double.Parse(oDisk.capacity.ToString());
                                        dblCapacity = dblCapacity / 1024.00;
                                        dblCapacity = dblCapacity / 1024.00;
                                        dblCapacity = dblCapacity / 1024.00;
                                        double dblAvailable = double.Parse(oDisk.freeSpace.ToString());
                                        dblAvailable = dblAvailable / 1024.00;
                                        dblAvailable = dblAvailable / 1024.00;
                                        dblAvailable = dblAvailable / 1024.00;
                                        lblHDD.Text += oDisk.diskPath + "&nbsp;&nbsp;" + dblCapacity.ToString("F") + " GB (" + dblAvailable.ToString("F") + " GB available)";
                                    }
                                    // Status
                                    VirtualMachineSummary oVMSummary = (VirtualMachineSummary)oVMWare.getObjectProperty(oVM, "summary");
                                    ManagedEntityStatus oVMStatus = oVMSummary.overallStatus;

                                    //lblOverallStatus.Text = (string)oVMStatus;
                                    lblOverallStatus.Text = Enum.GetName(typeof(ManagedEntityStatus), (ManagedEntityStatus)oVMStatus);
                                    // RAM & CPUs & Host
                                    VirtualMachineConfigSummary oVMConfig = oVMSummary.config;
                                    lblRAM.Text = oVMConfig.memorySizeMB.ToString();
                                    lblCPUs.Text = oVMConfig.numCpu.ToString();
                                    VirtualMachineRuntimeInfo oVMRuntime = oVMSummary.runtime;
                                    ManagedObjectReference oVMHost = oVMRuntime.host;
                                    strHost = (string)oVMWare.getObjectProperty(oVMHost, "name");
                                    if (strHost.IndexOf(".") > -1)
                                        strHost = strHost.Substring(0, strHost.IndexOf("."));
                                    lblVirtualCenter.Text = oVMWare.VirtualCenter();
                                    lblDataCenter.Text = oVMWare.DataCenter();

                                }
                                catch { }
                                finally
                                {
                                    if (_service != null)
                                    {
                                        _service.Abort();
                                        if (_service.Container != null)
                                            _service.Container.Dispose();
                                        try
                                        {
                                            _service.Logout(_sic.sessionManager);
                                        }
                                        catch { }
                                        _service.Dispose();
                                        _service = null;
                                        _sic = null;
                                    }
                                }

                                if (strHost != "")
                                {
                                    panGuestYes.Visible = true;
                                    oDataPoint.LoadTextBox(txtHostName, intProfile, btnHostName, "/datapoint/asset/server.aspx?t=name&q=" + oFunction.encryptQueryString(strHost), lblHostName, fldHostName, "WORKSTATION_VMWARE_HOST", strHost, "", false, false);
                                }
                                else
                                    panGuestNo.Visible = true;
                                TimeSpan oSpan = DateTime.Now.Subtract(datStart);
                                btnGuestQuery.Enabled = false;
                                btnGuestQuery.Text = "Query Time: " + oSpan.TotalSeconds.ToString("0") + " seconds...";
                            }
                            oDataPoint.LoadPanel(panGuestQuery, intProfile, fldHostQuery, "WORKSTATION_VMWARE_HOST_QUERY");

                            // Account Information
                            DataSet dsAccounts = oWorkstation.GetAccountsVMware(intWorkstation);
                            rptAccounts.DataSource = dsAccounts;
                            rptAccounts.DataBind();
                            lblNone.Visible = (rptAccounts.Items.Count == 0);

                            // Resource Dependencies
                            rptServiceRequests.DataSource = ds.Tables[1];
                            rptServiceRequests.DataBind();
                            trServiceRequests.Visible = (rptServiceRequests.Items.Count == 0);
                            foreach (RepeaterItem ri in rptServiceRequests.Items)
                            {
                                Label lblRequestID = (Label)ri.FindControl("lblRequestID");
                                Label lblServiceID = (Label)ri.FindControl("lblServiceID");
                                Label lblNumber = (Label)ri.FindControl("lblNumber");
                                int intService = Int32.Parse(lblServiceID.Text);
                                Label lblDetails = (Label)ri.FindControl("lblDetails");
                                Label lblProgress = (Label)ri.FindControl("lblProgress");

                                if (lblProgress.Text == "")
                                    lblProgress.Text = "<i>Unavailable</i>";
                                else
                                {
                                    int intResource = Int32.Parse(lblProgress.Text);
                                    //lblProgress.Text = oResourceRequest.GetStatus(Int32.Parse(lblRequestID.Text), Int32.Parse(lblServiceID.Text), Int32.Parse(lblNumber.Text), true, true, dsnServiceEditor, dsnAsset, intServiceDecommission);
                                    lblProgress.Text = oResourceRequest.GetStatus(null, null, Int32.Parse(lblRequestID.Text), Int32.Parse(lblServiceID.Text), null, Int32.Parse(lblNumber.Text), true, dsnServiceEditor)[0].status;
                                    /*
                                    double dblAllocated = 0.00;
                                    double dblUsed = 0.00;
                                    int intStatus = 0;
                                    bool boolAssigned = false;
                                    DataSet dsResource = oDataPoint.GetServiceRequestResource(intResource);
                                    if (dsResource.Tables[0].Rows.Count > 0)
                                        Int32.TryParse(dsResource.Tables[0].Rows[0]["status"].ToString(), out intStatus);
                                    foreach (DataRow drResource in dsResource.Tables[1].Rows)
                                    {
                                        boolAssigned = true;
                                        dblAllocated += double.Parse(drResource["allocated"].ToString());
                                        dblUsed += double.Parse(drResource["used"].ToString());
                                        intStatus = Int32.Parse(drResource["status"].ToString());
                                    }
                                    if (intStatus == (int)ResourceRequestStatus.Closed)
                                        lblProgress.Text = oServiceRequest.GetStatusBar(100.00, "100", "12", true);
                                    else if (intStatus == (int)ResourceRequestStatus.Cancelled)
                                        lblProgress.Text = "Cancelled";
                                    else if (boolAssigned == false)
                                    {
                                        string strManager = "";
                                        DataSet dsManager = oService.GetUser(intService, 1);  // Managers
                                        foreach (DataRow drManager in dsManager.Tables[0].Rows)
                                        {
                                            if (strManager != "")
                                                strManager += "\\n";
                                            int intManager = Int32.Parse(drManager["userid"].ToString());
                                            strManager += " - " + oUser.GetFullName(intManager) + " [" + oUser.GetName(intManager) + "]";
                                        }
                                        lblProgress.Text = "<a href=\"javascript:void(0);\" class=\"lookup\" onclick=\"alert('This request is pending assignment by the following...\\n\\n" + strManager + "');\">Pending Assignment</a>";
                                    }
                                    else if (dblAllocated > 0.00)
                                        lblProgress.Text = oServiceRequest.GetStatusBar((dblUsed / dblAllocated) * 100.00, "100", "12", true);
                                    else
                                        lblProgress.Text = "<i>N / A</i>";
                                    */
                                    lblDetails.Text = "<a href=\"javascript:void(0);\" class=\"lookup\" onclick=\"OpenNewWindowMenu('/datapoint/service/resource.aspx?id=" + oFunction.encryptQueryString(intResource.ToString()) + "', '800', '600');\">" + lblDetails.Text + "</a>";
                                }
                            }

                            // Provioning History
                            rptHistory.DataSource = SqlHelper.ExecuteDataset(dsnAsset, CommandType.Text, oDataPoint.AssetHistorySelect(intAsset));
                            rptHistory.DataBind();
                            lblHistory.Visible = (rptHistory.Items.Count == 0);
                            oDataPoint.LoadPanel(panProvisioning, intProfile, fldProvisioning, "WORKSTATION_VMWARE_STATUS");
                        }
                    }
                    else
                    {
                        if (Request.QueryString["t"] != null && Request.QueryString["q"] != null)
                            Response.Redirect("/datapoint/asset/datapoint_asset_search.aspx?t=" + Request.QueryString["t"] + "&q=" + Request.QueryString["q"] + "&r=0");
                        else
                            Response.Redirect("/datapoint/asset/datapoint_asset_search.aspx");
                    }
                }
                else if (Request.QueryString["q"] != null && Request.QueryString["q"] != "")
                {
                    string strQuery = oFunction.decryptQueryString(Request.QueryString["q"]);
                    DataSet ds = oDataPoint.GetAssetName(strQuery, intID, 0, "", "", 0);
                    if (ds.Tables[0].Rows.Count == 1)
                    {
                        intAsset = Int32.Parse(ds.Tables[0].Rows[0]["assetid"].ToString());
                        Response.Redirect(Request.Path + "?t=" + Request.QueryString["t"] + "&q=" + Request.QueryString["q"] + "&id=" + oFunction.encryptQueryString(intAsset.ToString()));
                    }
                }
                else
                    Response.Redirect("/datapoint/asset/datapoint_asset_search.aspx");
                btnClose.Attributes.Add("onclick", "window.close();return false;");
                btnPrint.Attributes.Add("onclick", "window.print();return false;");
                btnName.Attributes.Add("onclick", "return OpenWindow('DEVICE_NAME','?assetid=" + intAsset.ToString() + "');");
                btnSave.Attributes.Add("onclick", oDataPoint.LoadValidation("ProcessControlButton()"));
                btnSaveClose.Attributes.Add("onclick", oDataPoint.LoadValidation("ProcessControlButton()"));
                ddlPlatformClass.Attributes.Add("onchange", "PopulateEnvironments('" + ddlPlatformClass.ClientID + "','" + ddlPlatformEnvironment.ClientID + "',0);");
                ddlPlatformEnvironment.Attributes.Add("onchange", "UpdateDropDownHidden('" + ddlPlatformEnvironment.ClientID + "','" + hdnEnvironment.ClientID + "');");
                btnGuestQuery.Attributes.Add("onclick", "ProcessButton(this,'Querying... please be patient...','225') && ProcessControlButton();");
                btnOutput.Attributes.Add("onclick", "return ProcessButton(this) && ProcessControlButton();");
            }
            else
                panDenied.Visible = true;
        }
        protected void btnGuestQuery_Click(Object Sender, EventArgs e)
        {
            Response.Cookies["virtual_guest"].Value = DateTime.Now.ToString();
            Response.Redirect(Request.Path + "?t=" + Request.QueryString["t"] + "&q=" + Request.QueryString["q"] + "&id=" + oFunction.encryptQueryString(intAsset.ToString()) + "&menu_tab=2");
        }
        protected void btnNew_Click(Object Sender, EventArgs e)
        {
            Response.Redirect("/datapoint/asset/datapoint_asset_search.aspx");
        }
        protected void btnSave_Click(Object Sender, EventArgs e)
        {
            int intSave = Save();
            Response.Redirect(Request.Path + "?t=" + Request.QueryString["t"] + "&q=" + Request.QueryString["q"] + "&id=" + oFunction.encryptQueryString(intAsset.ToString()) + "&" + (intSave > 0 ? "save" : "error") + "=" + intSave.ToString());
        }
        protected void btnSaveClose_Click(Object Sender, EventArgs e)
        {
            int intSave = Save();
            Response.Redirect(Request.Path + "?t=" + Request.QueryString["t"] + "&q=" + Request.QueryString["q"] + "&id=" + oFunction.encryptQueryString(intAsset.ToString()) + "&" + (intSave > 0 ? "close" : "error") + "=" + intSave.ToString());
        }
        protected int Save()
        {
            oAsset.Update(intAsset, Int32.Parse(Request.Form[hdnModel.UniqueID]), txtPlatformSerial.Text, txtPlatformAsset.Text, Int32.Parse(ddlAssetAttribute.SelectedValue));
            oAsset.addAssetAttributesForAsset(intAsset, Int32.Parse(ddlAssetAttribute.SelectedValue), txtAssetAttributeComment.Text.Trim(), intProfile);

            int intReturn = 1;
            if (intReturn > 0)
                oAsset.UpdateGuest(intAsset, txtName.Text, Int32.Parse(ddlStatus.SelectedItem.Value), intProfile, DateTime.Now, Int32.Parse(ddlPlatformClass.SelectedItem.Value), Int32.Parse(Request.Form[hdnEnvironment.UniqueID]));
            return intReturn;
        }
        protected void btnStep_Click(Object Sender, EventArgs e)
        {
            string strError = "";
            int intWorkstation = Int32.Parse(lblWorkstation.Text);
            if (chkStep.Checked == true)
            {
                int intAsset2 = 0;
                int intStep = 0;
                DataSet ds = oWorkstation.GetVirtualErrors(intWorkstation);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr["assetid"].ToString() != "")
                        intAsset2 = Int32.Parse(dr["assetid"].ToString());
                    if (dr["fixed"].ToString() == "")
                        intStep = Int32.Parse(dr["step"].ToString());
                }
                if (intWorkstation > 0)
                {
                    oWorkstation.UpdateVirtualError(intWorkstation, intStep, 0, 0);
                    if (intAsset2 > 0)
                    {
                        string strSerial = oAsset.Get(intAsset2, "serial");
                        oZeus.UpdateResults(strSerial);
                    }
                    else
                        strError = "Warning: Invalid AssetID";
                }
                else
                    strError = "Error: Invalid WorkstationID";
            }
            if (chkStepVMWare.Checked == true)
            {
                VMWare oVMWare = new VMWare(0, dsn);
                int intWorkstationName = 0;
                Int32.TryParse(oWorkstation.GetVirtual(intWorkstation, "nameid"), out intWorkstationName);
                oVMWare.DeleteGuest(oWorkstation.GetName(intWorkstationName));
            }
            oWorkstation.UpdateVirtualStep(intWorkstation, Int32.Parse(txtStep.Text));
            Response.Redirect(Request.Path + "?q=" + Request.QueryString["q"] + "&id=" + oFunction.encryptQueryString(intAsset.ToString()) + "&admin=true&menu_tab=6&result=" + oFunction.encryptQueryString((strError == "" ? "Success" : strError)));
        }
        protected void btnOutput_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path + "?q=" + Request.QueryString["q"] + "&id=" + oFunction.encryptQueryString(intAsset.ToString()) + "&admin=true&menu_tab=6&output=true");
        }
    }
}
