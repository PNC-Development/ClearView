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
using System.Net.NetworkInformation;
using Tamir.SharpSsh;

namespace NCC.ClearView.Presentation.Web
{
    public partial class physical : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        private string dsnZeus = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ZeusDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);

        protected int intDataPointAvailableAsset = Int32.Parse(ConfigurationManager.AppSettings["DATAPOINT_AVAILABLE_ASSET"]);
        protected DataPoint oDataPoint;
        protected Users oUser;
        protected Servers oServer;
        protected Asset oAsset;
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
        protected Resiliency oResiliency;
        protected Solaris oSolaris;
        protected Log oLog;
        protected int intProfile = 0;
        protected int intApplication = 0;
        protected int intAsset = 0;
        protected string strMenuTab1 = "";
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
            oAsset = new Asset(intProfile, dsnAsset, dsn);
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
            oResiliency = new Resiliency(intProfile, dsn);
            oSolaris = new Solaris(intProfile, dsn);
            oLog = new Log(intProfile, dsn);
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
                        lblError.Text = "Name does not exist";
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
                        Master.Page.Title = "DataPoint | Physical (" + strHeader + ")";
                        lblHeaderSub.Text = "Provides all the information about a physical server...";
                        int intMenuTab = 0;
                        if (Request.QueryString["menu_tab"] != null && Request.QueryString["menu_tab"] != "")
                            intMenuTab = Int32.Parse(Request.QueryString["menu_tab"]);
                        Tab oTab = new Tab(hdnTab.ClientID, intMenuTab, "divMenu1", true, false);
                        oTab.AddTab("Asset Information", "");
                        oTab.AddTab("Location Information", "");
                        oTab.AddTab("World Wide Port Names", "");
                        DataSet dsSwitchNetwork = oAsset.GetSwitchports(intAsset, SwitchPortType.Network);
                        if (dsSwitchNetwork.Tables[0].Rows.Count > 0)
                        {
                            if (dsSwitchNetwork.Tables[0].Rows[0]["nexus"].ToString() == "1")
                            {
                                oTab.AddTab("Nexus Port Configuration", "");
                                divNexus.Visible = true;
                                lblSwitchNetworkA.Text = dsSwitchNetwork.Tables[0].Rows[0]["name"].ToString();
                                lblInterfaceNetworkA.Text = dsSwitchNetwork.Tables[0].Rows[0]["blade"].ToString() + "/" + dsSwitchNetwork.Tables[0].Rows[0]["port"].ToString();
                                if (dsSwitchNetwork.Tables[0].Rows.Count > 1)
                                {
                                    lblSwitchNetworkB.Text = dsSwitchNetwork.Tables[0].Rows[1]["name"].ToString();
                                    lblInterfaceNetworkB.Text = dsSwitchNetwork.Tables[0].Rows[1]["blade"].ToString() + "/" + dsSwitchNetwork.Tables[0].Rows[1]["port"].ToString();
                                }

                                DataSet dsSwitchRemote = oAsset.GetSwitchports(intAsset, SwitchPortType.Remote);
                                if (dsSwitchRemote.Tables[0].Rows.Count > 0)
                                {
                                    panNexusRemote.Visible = true;
                                    lblSwitchRemote.Text = dsSwitchRemote.Tables[0].Rows[0]["name"].ToString();
                                    lblInterfaceRemote.Text = dsSwitchRemote.Tables[0].Rows[0]["blade"].ToString() + "/" + dsSwitchRemote.Tables[0].Rows[0]["port"].ToString();
                                }

                                DataSet dsSwitchBackup = oAsset.GetSwitchports(intAsset, SwitchPortType.Backup);
                                if (dsSwitchBackup.Tables[0].Rows.Count > 0)
                                {
                                    panNexusBackup.Visible = true;
                                    lblSwitchBackup.Text = dsSwitchBackup.Tables[0].Rows[0]["name"].ToString();
                                    lblInterfaceBackup.Text = dsSwitchBackup.Tables[0].Rows[0]["blade"].ToString() + "/" + dsSwitchBackup.Tables[0].Rows[0]["port"].ToString();
                                }

                                DataSet dsSwitchCluster = oAsset.GetSwitchports(intAsset, SwitchPortType.Cluster);
                                if (dsSwitchCluster.Tables[0].Rows.Count > 0)
                                {
                                    panNexusClusterA.Visible = true;
                                    lblSwitchClusterA.Text = dsSwitchCluster.Tables[0].Rows[0]["name"].ToString();
                                    lblInterfaceClusterA.Text = dsSwitchCluster.Tables[0].Rows[0]["blade"].ToString() + "/" + dsSwitchCluster.Tables[0].Rows[0]["port"].ToString();
                                    if (dsSwitchCluster.Tables[0].Rows.Count > 1)
                                    {
                                        panNexusClusterB.Visible = true;
                                        lblSwitchClusterB.Text = dsSwitchCluster.Tables[0].Rows[1]["name"].ToString();
                                        lblInterfaceClusterB.Text = dsSwitchCluster.Tables[0].Rows[1]["blade"].ToString() + "/" + dsSwitchCluster.Tables[0].Rows[1]["port"].ToString();
                                    }
                                }

                                oDataPoint.LoadPanel(panNexusConfiguration, intProfile, fldNexusConfiguration, "BLADE_NEXUS_CONFIG");
                                if (panNexusConfiguration.Visible == true)
                                {
                                    if (Request.Cookies["nexus"] != null && Request.Cookies["nexus"].Value != "")
                                    {
                                        DateTime datStart = DateTime.Parse(Request.Cookies["nexus"].Value);
                                        Response.Cookies["nexus"].Value = "";

                                        Variables oVariable = new Variables(intEnvironment);

                                        ShowNexus(lblSwitchNetworkA, lblInterfaceNetworkA, lblConfigNetworkA, oVariable, intAsset);
                                        ShowNexus(lblSwitchNetworkB, lblInterfaceNetworkB, lblConfigNetworkB, oVariable, intAsset);
                                        if (panNexusRemote.Visible == true)
                                            ShowNexus(lblSwitchRemote, lblInterfaceRemote, lblConfigRemote, oVariable, intAsset);
                                        if (panNexusBackup.Visible == true)
                                            ShowNexus(lblSwitchBackup, lblInterfaceBackup, lblConfigBackup, oVariable, intAsset);
                                        if (panNexusClusterA.Visible == true)
                                            ShowNexus(lblSwitchClusterA, lblInterfaceClusterA, lblConfigClusterA, oVariable, intAsset);
                                        if (panNexusClusterB.Visible == true)
                                            ShowNexus(lblSwitchClusterB, lblInterfaceClusterB, lblConfigClusterB, oVariable, intAsset);

                                        TimeSpan oSpan = DateTime.Now.Subtract(datStart);
                                        btnNexus.Enabled = false;
                                        btnNexus.Text = "Query Time: " + oSpan.TotalSeconds.ToString("0") + " seconds...";
                                    }
                                }
                                oDataPoint.LoadButton(btnNexusAdapter, intProfile, fldNexusAdapter, "NEXUS_NETWORK_ADAPTER", "return OpenWindow('ASSET_DEPLOY_SWITCHPORTs','" + intAsset.ToString() + "');");
                            }
                            else
                            {
                                oTab.AddTab("Network Adapter Configuration", "");
                                divNetworkAdapter.Visible = true;
                                lblNetworkSwitch1.Text = dsSwitchNetwork.Tables[0].Rows[0]["name"].ToString();
                                lblNetworkBlade1.Text = dsSwitchNetwork.Tables[0].Rows[0]["blade"].ToString();
                                lblNetworkPort1.Text = dsSwitchNetwork.Tables[0].Rows[0]["port"].ToString();
                                lblNetworkInterface1.Text = dsSwitchNetwork.Tables[0].Rows[0]["interface"].ToString();
                                if (dsSwitchNetwork.Tables[0].Rows.Count > 1)
                                {
                                    lblNetworkSwitch2.Text = dsSwitchNetwork.Tables[0].Rows[1]["name"].ToString();
                                    lblNetworkBlade2.Text = dsSwitchNetwork.Tables[0].Rows[1]["blade"].ToString();
                                    lblNetworkPort2.Text = dsSwitchNetwork.Tables[0].Rows[1]["port"].ToString();
                                    lblNetworkInterface2.Text = dsSwitchNetwork.Tables[0].Rows[1]["interface"].ToString();
                                }
                                oDataPoint.LoadButton(btnNetworkAdapter, intProfile, fldNetworkAdapter, "PHYSICAL_NETWORK_ADAPTER", "return OpenWindow('ASSET_DEPLOY_SWITCHPORTs','" + intAsset.ToString() + "');");
                            }
                        }
                        oTab.AddTab("Resource Dependencies", "");
                        oTab.AddTab("Provisioning History", "");
                        //oTab.AddTab("Network Adapter Configuration", "");
                        if (oUser.IsAdmin(intProfile) == true || oDataPoint.GetFieldPermission(intProfile, "PHYSICAL_ADMINISTRATOR") == true)
                        {
                            panOldLocationInfo.Visible = true;
                            oTab.AddTab("Administration", "");
                            panAdministration.Visible = true;
                            if (intMenuTab == 6)
                            {
                                if (Request.QueryString["macAdmin"] != null)
                                    lblAdministration.Text = "<img src='/images/bigError.gif' border='0' align='absmiddle'/> There was a problem getting the MAC address.";
                                else if (Request.QueryString["zeus"] != null)
                                    lblAdministration.Text = "<img src='/images/bigCheck.gif' border='0' align='absmiddle'/> Record has been added to the Zeus table.";
                                else if (Request.QueryString["altiris"] != null)
                                    lblAdministration.Text = "<img src='/images/bigCheck.gif' border='0' align='absmiddle'/> Record has been added to Altiris.";
                            }
                        }
                        strMenuTab1 = oTab.GetTabs();


                        if (!IsPostBack)
                        {
                            LoadList();

                            // Asset Information
                            oDataPoint.LoadTextBox(txtPlatformSerial, intProfile, null, "", lblPlatformSerial, fldPlatformSerial, "PHYSICAL_SERIAL", strSerial, "", false, true);
                            oDataPoint.LoadTextBox(txtPlatformAsset, intProfile, null, "", lblPlatformAsset, fldPlatformAsset, "PHYSICAL_ASSET", strAsset, "", false, true);
                            
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
                            int intPlatform = oType.GetPlatform(intType);
                            oDataPoint.LoadDropDown(ddlPlatform, intProfile, null, "", lblPlatform, fldPlatform, "PHYSICAL_PLATFORM", "name", "platformid", oPlatform.Gets(1), intPlatform, false, false, true);
                            oDataPoint.LoadDropDown(ddlPlatformType, intProfile, null, "", lblPlatformType, fldPlatformType, "PHYSICAL_TYPE", "name", "id", oType.Gets(intPlatform, 1), intType, false, false, true);
                            oDataPoint.LoadDropDown(ddlPlatformModel, intProfile, null, "", lblPlatformModel, fldPlatformModel, "PHYSICAL_MODEL", "name", "id", oModel.Gets(intType, 1), intModelParent, false, false, true);
                            oDataPoint.LoadDropDown(ddlPlatformModelProperty, intProfile, null, "", lblPlatformModelProperty, fldPlatformModelProperty, "PHYSICAL_MODEL_PROP", "name", "id", oModelsProperties.GetModels(0, intModelParent, 1), intModel, false, false, true);

                            // Get Asset
                            DataSet dsAsset = oAsset.GetServerOrBlade(intAsset);
                            if (dsAsset.Tables[0].Rows.Count > 0)
                            {
                                oDataPoint.LoadTextBoxDeviceName(txtName, btnName, null, true, hdnPNC, intProfile, btnNameLookup, "/datapoint/asset/datapoint_asset_search.aspx?t=name&q=" + oFunction.encryptQueryString(dsAsset.Tables[0].Rows[0]["name"].ToString()), lblName, fldName, "PHYSICAL_NAME", dsAsset.Tables[0].Rows[0]["name"].ToString(), "", false, false);
                                if (txtName.Text != "")
                                    lblHeader.Text += "&nbsp;&nbsp;&nbsp;[" + txtName.Text + "]";
                                lblStatus.Text = dsAsset.Tables[0].Rows[0]["statusname"].ToString();
                                int intClass = Int32.Parse(dsAsset.Tables[0].Rows[0]["classid"].ToString());
                                int intEnv = Int32.Parse(dsAsset.Tables[0].Rows[0]["environmentid"].ToString());
                                hdnEnvironment.Value = intEnv.ToString();
                               
                                oDataPoint.LoadDropDown(ddlPlatformClass, intProfile, null, "", lblPlatformClass, fldPlatformClass, "PHYSICAL_CLASS", "name", "id", oClass.Gets(1), intClass, false, false, true);
                                oDataPoint.LoadDropDown(ddlPlatformEnvironment, intProfile, null, "", lblPlatformEnvironment, fldPlatformEnvironment, "PHYSICAL_ENVIRONMENT", "name", "id", oClass.GetEnvironment(intClass, 0), intEnv, false, false, true);
                                if (dsAsset.Tables[0].Rows[0]["ilo"].ToString() != "")
                                    oDataPoint.LoadTextBox(txtPlatformILO, intProfile, btnILO, "https://" + dsAsset.Tables[0].Rows[0]["ilo"].ToString(), lblPlatformILO, fldPlatformILO, "PHYSICAL_ILO", dsAsset.Tables[0].Rows[0]["ilo"].ToString(), "", false, true);
                                else
                                    oDataPoint.LoadTextBox(txtPlatformILO, intProfile, null, "", lblPlatformILO, fldPlatformILO, "PHYSICAL_ILO", dsAsset.Tables[0].Rows[0]["ilo"].ToString(), "", false, true);
                                oDataPoint.LoadTextBox(txtPlatformDummy, intProfile, null, "", lblPlatformDummy, fldPlatformDummy, "PHYSICAL_DUMMY", dsAsset.Tables[0].Rows[0]["dummy_name"].ToString(), "", false, true);
                                oDataPoint.LoadTextBox(txtPlatformMAC, intProfile, null, "", lblPlatformMAC, fldPlatformMAC, "PHYSICAL_MAC", dsAsset.Tables[0].Rows[0]["macaddress"].ToString(), "", false, true);
                                if (Request.QueryString["mac"] != null && Request.QueryString["mac"] != "")
                                    txtPlatformMAC.Text = oFunction.decryptQueryString(Request.QueryString["mac"]);
                                if (Request.QueryString["time"] != null && Request.QueryString["time"] != "")
                                {
                                    btnMAC.Enabled = false;
                                    btnMAC.Text = Request.QueryString["time"] + " secs";
                                }
                                btnMAC.Visible = (oUser.IsAdmin(intProfile) == true && oModelsProperties.IsDell(intModel) == true);
                                oDataPoint.LoadTextBox(txtPlatformVLAN, intProfile, null, "", lblPlatformVLAN, fldPlatformVLAN, "PHYSICAL_VLAN", dsAsset.Tables[0].Rows[0]["vlan"].ToString(), "", false, true);
                                int intBuildNetwork = 0;
                                Int32.TryParse(dsAsset.Tables[0].Rows[0]["build_network_id"].ToString(), out intBuildNetwork);
                                oDataPoint.LoadDropDown(ddlPlatformBuildNetwork, intProfile, null, "", lblPlatformBuildNetwork, fldPlatformBuildNetwork, "PHYSICAL_BUILD_NETWORK", "name", "id", oSolaris.GetBuildNetworks(1), intBuildNetwork, false, false, false);
                                int intResiliency = 0;
                                Int32.TryParse(dsAsset.Tables[0].Rows[0]["resiliencyid"].ToString(), out intResiliency);
                                oDataPoint.LoadDropDown(ddlPlatformResiliency, intProfile, null, "", lblPlatformResiliency, fldPlatformResiliency, "PHYSICAL_RESILIENCY", "name", "id", oResiliency.Gets(1), intResiliency, false, false, true);
                                int intOperatingSystemGroup = 0;
                                Int32.TryParse(dsAsset.Tables[0].Rows[0]["operatingsystemgroupid"].ToString(), out intOperatingSystemGroup);
                                oDataPoint.LoadDropDown(ddlPlatformOperatingSystemGroup, intProfile, null, "", lblPlatformOperatingSystemGroup, fldPlatformOperatingSystemGroup, "PHYSICAL_OS_GROUP", "name", "id", oOperatingSystem.GetGroups(1), intOperatingSystemGroup, false, false, false);


                                lblOldlocation.Text = dsAsset.Tables[0].Rows[0]["OldLocation"].ToString();
                                lblOldRoom.Text = dsAsset.Tables[0].Rows[0]["OldRoom"].ToString();
                                lblOldRack.Text = dsAsset.Tables[0].Rows[0]["OldRack"].ToString();

                                txtLocation.Text = dsAsset.Tables[0].Rows[0]["Location"].ToString();
                                txtRoom.Text = dsAsset.Tables[0].Rows[0]["Room"].ToString();
                                txtZone.Text = dsAsset.Tables[0].Rows[0]["Zone"].ToString();
                                txtRack.Text = dsAsset.Tables[0].Rows[0]["Rack"].ToString();
                                txtRackPosition.Text = dsAsset.Tables[0].Rows[0]["Rackposition"].ToString();
                                oDataPoint.LoadTextBox(txtRackPosition, intProfile, null, "", lblRackPositionValue, fldLocation, "CHANGE_LOCATION", dsAsset.Tables[0].Rows[0]["rackposition"].ToString(), "", false, true);
                                hdnRackId.Value = dsAsset.Tables[0].Rows[0]["RackId"].ToString();

                                oDataPoint.LoadButton(btnSelectLocation, intProfile, fldLocation, "CHANGE_LOCATION",
                                                     "return LoadLocationRoomRack('" + "rack" + "','" + hdnRackId.ClientID + "', '" + txtLocation.ClientID + "','" + txtRoom.ClientID + "','" + txtZone.ClientID + "','" + txtRack.ClientID + "');");

                                ddlStatus.SelectedValue = dsAsset.Tables[0].Rows[0]["status"].ToString();
                                ddlStatus.Enabled = (intAssetAttribute == (int)AssetAttribute.Ok);
                                panStatus.Visible = (ddlStatus.Enabled == false);
                            }
                            else
                                Response.Redirect("/datapoint/asset/physical_deploy.aspx?t=serial&q=" + oFunction.encryptQueryString(strSerial) + "&id=" + oFunction.encryptQueryString(intAsset.ToString()) + "&r=0");

                            // Resource Dependencies
                            AssetOrder oAssetOrder = new AssetOrder(0, dsn, dsnAsset, intEnvironment);
                            Services oService = new Services(0, dsn);
                            ServiceRequests oServiceRequest = new ServiceRequests(0, dsn);
                            rptServiceRequests.DataSource = oAssetOrder.GetByAsset(intAsset, false);
                            rptServiceRequests.DataBind();
                            trServiceRequests.Visible = (rptServiceRequests.Items.Count == 0);
                            foreach (RepeaterItem ri in rptServiceRequests.Items)
                            {
                                Label lblServiceID = (Label)ri.FindControl("lblServiceID");
                                int intService = Int32.Parse(lblServiceID.Text);
                                Label lblDetails = (Label)ri.FindControl("lblDetails");
                                Label lblProgress = (Label)ri.FindControl("lblProgress");

                                if (lblProgress.Text == "")
                                    lblProgress.Text = "<i>Unavailable</i>";
                                else
                                {
                                    int intResource = Int32.Parse(lblProgress.Text);
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
                                    lblDetails.Text = "<a href=\"javascript:void(0);\" class=\"lookup\" onclick=\"OpenNewWindowMenu('/datapoint/service/resource.aspx?id=" + oFunction.encryptQueryString(intResource.ToString()) + "', '800', '600');\">" + lblDetails.Text + "</a>";
                                }
                            }

                            // Provioning History
                            rptHistory.DataSource = rptHistory.DataSource = SqlHelper.ExecuteDataset(dsnAsset, CommandType.Text, oDataPoint.AssetHistorySelect(intAsset));
                            rptHistory.DataBind();
                            lblHistory.Visible = (rptHistory.Items.Count == 0);
                            oDataPoint.LoadPanel(panProvisioning, intProfile, fldProvisioning, "PHYSICAL_STATUS");

                            // WWW
                            rptWWW.DataSource = oAsset.GetHBA(intAsset);
                            rptWWW.DataBind();
                            lblWWW.Visible = (rptWWW.Items.Count == 0);
                            oDataPoint.LoadButton(btnWWW, intProfile, fldWWW, "PHYSICAL_WWW", "return OpenWindow('ASSET_DEPLOY_HBAs','" + intAsset.ToString() + "');");

                            // Redeploy
                            oDataPoint.LoadButton(btnRedeploy, intProfile, fldRedeploy, "PHYSICAL_REDEPLOY");
                            btnRedeploy.Attributes.Add("onclick", "return confirm('Are you sure you want to redeploy this asset?') && confirm('WARNING! Redeploying an asset means that ALL the information regarding this asset will be erased.\\n\\nAre you sure you want to redeploy this asset?');");

                            // Asset Orders
                            DataSet dsOrders = oAssetOrder.GetByAsset(intAsset, true);
                            DataView dvOrders = dsOrders.Tables[0].DefaultView;
                            DateTime FilterDate = DateTime.Today.AddDays(-90);
                            dvOrders.RowFilter = String.Format("poweroff IS NULL AND modified >= #{0}#", FilterDate.ToString("MM/dd/yyyy"));
                            rptAssetOrders.DataSource = dvOrders;
                            rptAssetOrders.DataBind();
                            lblAssetOrders.Visible = (rptAssetOrders.Items.Count == 0);
                            foreach (RepeaterItem ri in rptAssetOrders.Items)
                            {
                                Label lblRequest = (Label)ri.FindControl("lblRequest");
                                lblRequest.Text = "<a href=\"javascript:void(0);\" class=\"lookup\" onclick=\"OpenNewWindowMenu('/datapoint/service/resource.aspx?id=" + oFunction.encryptQueryString(lblRequest.ToolTip) + "', '800', '600');\">" + lblRequest.Text + "</a>";
                                LinkButton btnDeleteOrder = (LinkButton)ri.FindControl("btnDeleteOrder");
                                Label lblStatus = (Label)ri.FindControl("lblStatus");
                                btnDeleteOrder.Enabled = (lblStatus.Text == "Active");
                                if (btnDeleteOrder.Enabled)
                                    btnDeleteOrder.Attributes.Add("onclick", "return confirm('Are you sure you want to cancel this order?');");
                            }

                            if (panAdministration.Visible)
                            {
                                // Load Asset Order information
                                int intOrderID = 0;
                                Int32.TryParse(oAsset.Get(intAsset, "orderid"), out intOrderID);
                                if (intOrderID > 0)
                                {
                                    lblAssetOrderID.Text = intOrderID.ToString();
                                    DataSet dsOrder = oAssetOrder.Get(intOrderID);
                                    if (dsOrder.Tables[0].Rows.Count > 0)
                                    {
                                        DataRow drOrder = dsOrder.Tables[0].Rows[0];
                                        int intOrderProject = 0;
                                        if (Int32.TryParse(drOrder["projectid"].ToString(), out intOrderProject) == true)
                                        {
                                            Projects oProject = new Projects(intProfile, dsn);
                                            if (intOrderProject > 0)
                                                lblAssetOrderProject.Text = oProject.GetName(intOrderProject) + " (" + oProject.Get(intOrderProject, "number") + ")";
                                            else
                                                lblAssetOrderProject.Text = "<i>Any Project</i>";
                                        }
                                        else
                                            lblAssetOrderProject.Text = "<i>N / A</i>";

                                        int intOrderResiliency = 0;
                                        if (Int32.TryParse(drOrder["resiliencyid"].ToString(), out intOrderResiliency) == true)
                                        {
                                            if (intOrderResiliency > 0)
                                                lblAssetOrderResiliency.Text = oResiliency.Get(intOrderResiliency, "name");
                                            else
                                                lblAssetOrderResiliency.Text = "<i>Any Resiliency</i>";
                                        }
                                        else
                                            lblAssetOrderResiliency.Text = "<i>N / A</i>";
                                        if (intOrderResiliency > 0 && lblAssetOrderResiliency.Text != ddlPlatformResiliency.SelectedItem.Text)
                                            lblAssetOrderResiliency.CssClass = "reddefault";

                                        int intOrderLocation = 0;
                                        if (Int32.TryParse(drOrder["rackid"].ToString(), out intOrderLocation) == true)
                                        {
                                            if (intOrderLocation > 0)
                                            {
                                                RacksNew oRackNew = new RacksNew(intProfile, dsn);
                                                lblAssetOrderLocation.Text = oRackNew.Get(intOrderLocation, "Location");
                                            }
                                            else
                                            {
                                                Int32.TryParse(drOrder["locationid"].ToString(), out intOrderLocation);
                                                if (intOrderLocation > 0)
                                                {
                                                    Locations oLocation = new Locations(intProfile, dsn);
                                                    lblAssetOrderLocation.Text = oLocation.GetFull(intOrderLocation);
                                                }
                                                else
                                                    lblAssetOrderLocation.Text = "<i>Any Data Center</i>";
                                            }
                                        }
                                        else
                                            lblAssetOrderLocation.Text = "<i>N / A</i>";
                                        if (intOrderLocation > 0 && lblAssetOrderLocation.Text != txtLocation.Text)
                                            lblAssetOrderLocation.CssClass = "reddefault";

                                        int intOrderModel = 0;
                                        if (Int32.TryParse(drOrder["modelid"].ToString(), out intOrderModel) == true)
                                        {
                                            if (intOrderModel > 0)
                                                lblAssetOrderModel.Text = oModelsProperties.Get(intOrderModel, "name");
                                            else
                                                lblAssetOrderModel.Text = "<i>Any Model</i>";
                                        }
                                        else
                                            lblAssetOrderModel.Text = "<i>N / A</i>";
                                        if (intOrderModel > 0 && lblAssetOrderModel.Text != ddlPlatformModelProperty.SelectedItem.Text)
                                            lblAssetOrderModel.CssClass = "reddefault";
                                    }
                                }
                            }
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
                    else
                        Response.Redirect("/datapoint/asset/datapoint_asset_search.aspx?multiple=true&t=" + Request.QueryString["t"] + "&q=" + Request.QueryString["q"]);
                }
                else
                    Response.Redirect("/datapoint/asset/datapoint_asset_search.aspx");
                btnClose.Attributes.Add("onclick", "window.close();return false;");
                btnPrint.Attributes.Add("onclick", "window.print();return false;");
                btnName.Attributes.Add("onclick", "return OpenWindow('DEVICE_NAME','?assetid=" + intAsset.ToString() + "');");
                btnSave.Attributes.Add("onclick", oDataPoint.LoadValidation("ProcessControlButton()"));
                btnSaveClose.Attributes.Add("onclick", oDataPoint.LoadValidation("ProcessControlButton()"));
                btnMAC.Attributes.Add("onclick", "return confirm('WARNING: This process could take a long time (up to 3 minutes).\\n\\nDuring this time, you should not close the window or click anywhere else on the page.\\n\\nAre you sure you want to continue?') && ProcessButton(this) && ProcessControlButton();");
                ddlPlatformClass.Attributes.Add("onchange", "PopulateEnvironments('" + ddlPlatformClass.ClientID + "','" + ddlPlatformEnvironment.ClientID + "',0);");
                ddlPlatformEnvironment.Attributes.Add("onchange", "UpdateDropDownHidden('" + ddlPlatformEnvironment.ClientID + "','" + hdnEnvironment.ClientID + "');");
                //ddlLocationState.Attributes.Add("onchange", "LoadLocationCity('" + ddlLocationCity.ClientID + "');LoadLocationAddress('" + ddlLocationAddress.ClientID + "');PopulateCitys('" + ddlLocationState.ClientID + "','" + ddlLocationCity.ClientID + "');ResetDropDownHidden('" + hdnAddress.ClientID + "');");
                //ddlLocationCity.Attributes.Add("onchange", "LoadLocationAddress('" + ddlLocationAddress.ClientID + "');PopulateAddresss('" + ddlLocationCity.ClientID + "','" + ddlLocationAddress.ClientID + "');ResetDropDownHidden('" + hdnAddress.ClientID + "');");
                //ddlLocationAddress.Attributes.Add("onchange", "UpdateDropDownHidden('" + ddlLocationAddress.ClientID + "','" + hdnAddress.ClientID + "');");
                btnNexus.Attributes.Add("onclick", "ProcessButton(this,'Querying... please be patient...','225') && ProcessControlButton();");
               
              
            }
            else
                panDenied.Visible = true;
        }
        private void LoadList()
        {
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

            //int intReturn = oServer.DeviceName_Update(intAsset, txtName.Text, intProfile, txtPlatformSerial.Text, dsnAsset, Request.Form[hdnPNC.UniqueID]);
            int intReturn = 1;
            if (intReturn > 0)
                oAsset.UpdateServer(intAsset, txtName.Text, Int32.Parse(ddlStatus.SelectedItem.Value), intProfile, DateTime.Now, Int32.Parse(ddlPlatformClass.SelectedItem.Value), Int32.Parse(Request.Form[hdnEnvironment.UniqueID]), Int32.Parse(hdnRackId.Value), txtRackPosition.Text, txtPlatformILO.Text, txtPlatformDummy.Text, txtPlatformMAC.Text, Int32.Parse(txtPlatformVLAN.Text), Int32.Parse(ddlPlatformBuildNetwork.SelectedItem.Value), Int32.Parse(ddlPlatformResiliency.SelectedItem.Value), Int32.Parse(ddlPlatformOperatingSystemGroup.SelectedItem.Value));
            return intReturn;
        }
        protected void btnRedeploy_Click(Object Sender, EventArgs e)
        {
            oAsset.DeleteServer(intAsset, true, intProfile);
            Response.Redirect("/datapoint/asset/datapoint_asset_search.aspx?t=serial&q=" + oFunction.encryptQueryString(oAsset.Get(intAsset, "serial")) + "&r=0");
        }
        protected void btnZeus_Click(Object Sender, EventArgs e)
        {
            Variables oVariable = new Variables(intEnvironment);
            string strMACAddress = oAsset.GetServerOrBlade(intAsset, "macaddress").ToUpper();
            if (strMACAddress == "")
                Response.Redirect(Request.Path + "?t=" + Request.QueryString["t"] + "&q=" + Request.QueryString["q"] + "&id=" + oFunction.encryptQueryString(intAsset.ToString()) + "&menu_tab=6&zeus=true&macAdmin=true");
            else
            {
                string strZeusMAC = oFunction.FormatMAC(strMACAddress, ":");
                Zeus oZeus = new Zeus(intProfile, dsnZeus);
                oZeus.AddBuild(0, 0, 0, txtPlatformSerial.Text, txtPlatformAsset.Text, txtName.Text, "", "", "", 0, "", "", intEnvironment, "", 0, strZeusMAC, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", 1);
                Response.Redirect(Request.Path + "?t=" + Request.QueryString["t"] + "&q=" + Request.QueryString["q"] + "&id=" + oFunction.encryptQueryString(intAsset.ToString()) + "&menu_tab=6&zeus=true");
            }
        }
        protected void btnAltiris_Click(Object Sender, EventArgs e)
        {
            Variables oVariable = new Variables(intEnvironment);
            string strMACAddress = oAsset.GetServerOrBlade(intAsset, "macaddress").ToUpper();
            if (strMACAddress == "")
                Response.Redirect(Request.Path + "?t=" + Request.QueryString["t"] + "&q=" + Request.QueryString["q"] + "&id=" + oFunction.encryptQueryString(intAsset.ToString()) + "&menu_tab=6&altiris=true&macAdmin=true");
            else
            {
                string strAltirisMAC = oFunction.FormatMAC(strMACAddress, "");
                System.Net.NetworkCredential oCredentials = new System.Net.NetworkCredential(oVariable.AltirisUsername(), oVariable.AltirisPassword(), oVariable.AltirisDomain());
                NCC.ClearView.Application.Core.AltirisWS_Computer.ComputerManagementService oComputer = new NCC.ClearView.Application.Core.AltirisWS_Computer.ComputerManagementService();
                oComputer.Credentials = oCredentials;
                oComputer.Url = ddlAltirisComputer.SelectedItem.Value;
                // Delete Computer Object if it Exists
                int intDeleteComputer = oComputer.GetComputerID(txtName.Text, 1);
                if (intDeleteComputer > 0)
                    oComputer.DeleteComputer(intDeleteComputer);
                // Add Computer Object
                int intComputer = oComputer.AddBasicVirtualComputer(-1, txtName.Text, txtPlatformAsset.Text, txtPlatformSerial.Text, strAltirisMAC, 2, "");
                if (ddlAltirisJob.SelectedItem.Value != "")
                {
                    // Assign Schedule
                    NCC.ClearView.Application.Core.altirisws.dsjob oJob = new NCC.ClearView.Application.Core.altirisws.dsjob();
                    oJob.Credentials = oCredentials;
                    oJob.Url = ddlAltirisSchedule.SelectedItem.Value;
                    oJob.ScheduleNow(txtName.Text, ddlAltirisJob.SelectedItem.Value);
                }
                Response.Redirect(Request.Path + "?t=" + Request.QueryString["t"] + "&q=" + Request.QueryString["q"] + "&id=" + oFunction.encryptQueryString(intAsset.ToString()) + "&menu_tab=6&altiris=true");
            }
        }
        protected void btnMAC_Click(Object Sender, EventArgs e)
        {
            Variables oVariable = new Variables(intEnvironment);
            DateTime datStart = DateTime.Now;
            string strMACAddress = oAsset.GetVirtualConnectMACAddress(intAsset, 0, intEnvironment, 1, oVariable.DocumentsFolder(), dsn, oFunction.GetSetupValuesByKey("RACADM_WEB").Tables[0].Rows[0]["value"].ToString(), oLog, txtName.Text);
            TimeSpan oSpan = DateTime.Now.Subtract(datStart);
            if (strMACAddress == "**ERROR**")
                Response.Redirect(Request.Path + "?t=" + Request.QueryString["t"] + "&q=" + Request.QueryString["q"] + "&id=" + oFunction.encryptQueryString(intAsset.ToString()) + "&mac=" + oFunction.encryptQueryString("ERROR") + "&time=" + oSpan.TotalSeconds.ToString("0"));
            else
                Response.Redirect(Request.Path + "?t=" + Request.QueryString["t"] + "&q=" + Request.QueryString["q"] + "&id=" + oFunction.encryptQueryString(intAsset.ToString()) + "&mac=" + oFunction.encryptQueryString(strMACAddress) + "&time=" + oSpan.TotalSeconds.ToString("0"));
        }
        protected void btnNexus_Click(Object Sender, EventArgs e)
        {
            Response.Cookies["nexus"].Value = DateTime.Now.ToString();
            Response.Redirect(Request.Path + "?t=" + Request.QueryString["t"] + "&q=" + Request.QueryString["q"] + "&id=" + oFunction.encryptQueryString(intAsset.ToString()) + "&menu_tab=4");
        }
        protected void btnDeleteOrder_Click(Object Sender, EventArgs e)
        {
            LinkButton oButton = (LinkButton)Sender;
            int intOrder = Int32.Parse(oButton.ToolTip);
            AssetOrder oAssetOrder = new AssetOrder(intProfile, dsn, dsnAsset, intEnvironment);
            ResourceRequest oResourceRequest = new ResourceRequest(intProfile, dsn);
            DataSet dsOrders = oAssetOrder.GetByAsset(intAsset, false);
            foreach (DataRow drOrders in dsOrders.Tables[0].Rows)
            {
                int intTemp = Int32.Parse(drOrders["orderid"].ToString());
                if (intOrder == intTemp)
                {
                    // Cancel Resource Requests
                    int intResource = 0;
                    if (Int32.TryParse(drOrders["resourceid"].ToString(), out intResource) == true)
                        oResourceRequest.UpdateStatusOverallWorkflow(intResource, (int)ResourceRequestStatus.Cancelled);
                    // Delete Order
                    oAssetOrder.DeleteOrder(intOrder);
                    // Delete Asset Order Asset Selection
                    oAssetOrder.DeleteAssetOrderAssetSelection(intOrder, intAsset);
                    // Set NewOrderID = 0
                    oAsset.updateNewOrderId(0, intAsset);
                    // Set AssetAttribute to OK (if Moving)
                    int intAttribute = 0;
                    Int32.TryParse(oAsset.Get(intAsset, "asset_attribute"), out intAttribute);
                    if (intAttribute == (int)AssetAttribute.Moving)
                    {
                        oAsset.Update(intAsset, (int)AssetAttribute.Ok);
                        oAsset.addAssetAttributesForAsset(intAsset, (int)AssetAttribute.Ok, "", intProfile);
                    }
                    break;
                }
            }
            Response.Redirect(Request.Path + "?t=" + Request.QueryString["t"] + "&q=" + Request.QueryString["q"] + "&id=" + oFunction.encryptQueryString(intAsset.ToString()) + "&menu_tab=6");
        }
        protected void ShowNexus(Label _switch, Label _interface, Label _config, Variables _variable, int _assetid)
        {
            if (_switch.Text != "" && _interface.Text != "")
            {
                Ping oPinger = new Ping();
                string strPingStatus = "";
                try
                {
                    PingReply oReply = oPinger.Send(_switch.Text);
                    strPingStatus = oReply.Status.ToString().ToUpper();
                }
                catch { }
                if (strPingStatus == "SUCCESS")
                {
                    try
                    {
                        SshShell oSSHshell = new SshShell(_switch.Text, _variable.NexusUsername(), _variable.NexusPassword());
                        oSSHshell.RemoveTerminalEmulationCharacters = true;
                        oSSHshell.Connect();
                        string strLogin = oAsset.GetDellSwitchportOutput(oSSHshell);
                        if (strLogin != "**INVALID**")
                        {
                            string strConfig = oAsset.GetDellSwitchportOutput(oSSHshell, _interface.Text, DellBladeSwitchportType.Config, _assetid);
                            _config.Text = strConfig.Replace(Environment.NewLine, "<br/>");
                        }
                        else
                            _config.Text = "There was a problem logging into the Switch";
                    }
                    catch (Exception exNexus)
                    {
                        _config.Text = "There was a problem connecting to the Switch - " + exNexus.Message;
                        Request.Cookies["nexus"].Value = "";
                    }
                }
                else
                    _config.Text = "There was a problem pinging the Switch";
            }
        }
    }
}
