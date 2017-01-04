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
    public partial class blade : BasePage
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
        protected Locations oLocation;
        protected Solaris oSolaris;
        protected Resiliency oResiliency;
        protected Log oLog;
        protected int intProfile = 0;
        protected int intApplication = 0;
        protected int intAsset = 0;
        protected string strMenuTab1 = "";
        private int intID = 0;
        private int intSaveMenuTab = 0;

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
            oLocation = new Locations(intProfile, dsn);
            oSolaris = new Solaris(intProfile, dsn);
            oResiliency = new Resiliency(intProfile, dsn);
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
                        int intModel = Int32.Parse(oAsset.Get(intAsset, "modelid"));

                        string strHeader = (strSerial.Length > 15 ? strSerial.Substring(0, 15) + "..." : strSerial);
                        lblHeader.Text = "&quot;" + strHeader.ToUpper() + "&quot;";
                        Master.Page.Title = "DataPoint | Blade (" + strHeader + ")";
                        lblHeaderSub.Text = "Provides all the information about a blade server...";
                        int intMenuTab = 0;
                        if (Request.QueryString["menu_tab"] != null && Request.QueryString["menu_tab"] != "")
                            intMenuTab = Int32.Parse(Request.QueryString["menu_tab"]);
                        Tab oTab = new Tab(hdnTab.ClientID, intMenuTab, "divMenu1", true, false);
                        oTab.AddTab("Asset Information", "");
                        oTab.AddTab("World Wide Port Names", "");
                        if (oModelsProperties.IsDell(intModel) == false)
                            oTab.AddTab("Network Adapter Configuration", "");
                        else
                            oTab.AddTab("Nexus Port Configuration", "");
                        oTab.AddTab("Resource Dependencies", "");
                        oTab.AddTab("Provisioning History", "");
                        if (oUser.IsAdmin(intProfile) == true || oDataPoint.GetFieldPermission(intProfile, "BLADE_ADMINISTRATOR") == true)
                        {
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
                            oDataPoint.LoadTextBox(txtPlatformSerial, intProfile, null, "", lblPlatformSerial, fldPlatformSerial, "BLADE_SERIAL", strSerial, "", false, true);
                            oDataPoint.LoadTextBox(txtPlatformAsset, intProfile, null, "", lblPlatformAsset, fldPlatformAsset, "BLADE_ASSET", strAsset, "", false, true);
                            
                            int intAssetAttribute = Int32.Parse(oAsset.Get(intAsset, "asset_attribute"));
                            oDataPoint.LoadDropDown(ddlAssetAttribute, intProfile, null, "", lblAssetAttribute, fldAssetAttribute, "ASSET_ATTRIBUTE", "Name", "AttributeId", oAsset.getAssetAttributes(null, "", 1), intAssetAttribute,true, false, false);
                            oDataPoint.LoadTextBox(txtAssetAttributeComment, intProfile, null, "", lblAssetAttributeComment, fldAssetAttributeComment, "ASSET_ATTRIBUTE_COMMENT", oAsset.getAssetAttributesComments(intAsset), "", false, true);
                            ddlAssetAttribute.Attributes.Add("onclick","return SetControlsForAssetAttributes()");

                            ddlPlatform.Attributes.Add("onchange", "PopulatePlatformTypes('" + ddlPlatform.ClientID + "','" + ddlPlatformType.ClientID + "','" + ddlPlatformModel.ClientID + "','" + ddlPlatformModelProperty.ClientID + "');ResetDropDownHidden('" + hdnModel.ClientID + "');");
                            ddlPlatformType.Attributes.Add("onchange", "PopulatePlatformModels('" + ddlPlatformType.ClientID + "','" + ddlPlatformModel.ClientID + "','" + ddlPlatformModelProperty.ClientID + "');ResetDropDownHidden('" + hdnModel.ClientID + "');");
                            ddlPlatformModel.Attributes.Add("onchange", "PopulatePlatformModelProperties('" + ddlPlatformModel.ClientID + "','" + ddlPlatformModelProperty.ClientID + "');ResetDropDownHidden('" + hdnModel.ClientID + "');");
                            ddlPlatformModelProperty.Attributes.Add("onchange", "UpdateDropDownHidden('" + ddlPlatformModelProperty.ClientID + "','" + hdnModel.ClientID + "');");

                            hdnModel.Value = intModel.ToString();
                            int intModelParent = Int32.Parse(oModelsProperties.Get(intModel, "modelid"));
                            int intType = oModel.GetType(intModelParent);
                            int intPlatform = oType.GetPlatform(intType);
                            oDataPoint.LoadDropDown(ddlPlatform, intProfile, null, "", lblPlatform, fldPlatform, "BLADE_PLATFORM", "name", "platformid", oPlatform.Gets(1), intPlatform, false, false, true);
                            oDataPoint.LoadDropDown(ddlPlatformType, intProfile, null, "", lblPlatformType, fldPlatformType, "BLADE_TYPE", "name", "id", oType.Gets(intPlatform, 1), intType, false, false, true);
                            oDataPoint.LoadDropDown(ddlPlatformModel, intProfile, null, "", lblPlatformModel, fldPlatformModel, "BLADE_MODEL", "name", "id", oModel.Gets(intType, 1), intModelParent, false, false, true);
                            oDataPoint.LoadDropDown(ddlPlatformModelProperty, intProfile, null, "", lblPlatformModelProperty, fldPlatformModelProperty, "BLADE_MODEL_PROP", "name", "id", oModelsProperties.GetModels(0, intModelParent, 1), intModel, false, false, true);

                            // Get Asset
                            int intEnclosure = 0;
                            int intSlot = 0;
                            DataSet dsAsset = oAsset.GetServerOrBlade(intAsset);
                            if (dsAsset.Tables[0].Rows.Count > 0)
                            {
                                oDataPoint.LoadTextBoxDeviceName(txtName, btnName, null, true, hdnPNC, intProfile, btnNameLookup, "/datapoint/asset/datapoint_asset_search.aspx?t=name&q=" + oFunction.encryptQueryString(dsAsset.Tables[0].Rows[0]["name"].ToString()), lblName, fldName, "BLADE_NAME", dsAsset.Tables[0].Rows[0]["name"].ToString(), "", false, false);
                                if (txtName.Text != "")
                                    lblHeader.Text += "&nbsp;&nbsp;&nbsp;[" + txtName.Text + "]";
                                lblStatus.Text = dsAsset.Tables[0].Rows[0]["statusname"].ToString();
                                intEnclosure = Int32.Parse(dsAsset.Tables[0].Rows[0]["enclosureid"].ToString());
                                Int32.TryParse(dsAsset.Tables[0].Rows[0]["slot"].ToString(), out intSlot);
                                int intClass = Int32.Parse(dsAsset.Tables[0].Rows[0]["classid"].ToString());
                                int intEnv = Int32.Parse(dsAsset.Tables[0].Rows[0]["environmentid"].ToString());
                                hdnEnvironment.Value = intEnv.ToString();
                                //int intAddress = Int32.Parse(dsAsset.Tables[0].Rows[0]["addressid"].ToString());
                                oDataPoint.LoadDropDown(ddlPlatformClass, intProfile, null, "", lblPlatformClass, fldPlatformClass, "BLADE_CLASS", "name", "id", oClass.Gets(1), intClass, false, false, true);
                                oDataPoint.LoadDropDown(ddlPlatformEnvironment, intProfile, null, "", lblPlatformEnvironment, fldPlatformEnvironment, "BLADE_ENVIRONMENT", "name", "id", oClass.GetEnvironment(intClass, 0), intEnv, false, false, true);
                                if (dsAsset.Tables[0].Rows[0]["ilo"].ToString() != "")
                                    oDataPoint.LoadTextBox(txtPlatformILO, intProfile, btnILO, "https://" + dsAsset.Tables[0].Rows[0]["ilo"].ToString(), lblPlatformILO, fldPlatformILO, "BLADE_ILO", dsAsset.Tables[0].Rows[0]["ilo"].ToString(), "", false, true);
                                else
                                    oDataPoint.LoadTextBox(txtPlatformILO, intProfile, null, "", lblPlatformILO, fldPlatformILO, "BLADE_ILO", dsAsset.Tables[0].Rows[0]["ilo"].ToString(), "", false, true);
                                oDataPoint.LoadTextBox(txtPlatformDummy, intProfile, null, "", lblPlatformDummy, fldPlatformDummy, "BLADE_DUMMY", dsAsset.Tables[0].Rows[0]["dummy_name"].ToString(), "", false, true);
                                oDataPoint.LoadTextBox(txtPlatformMAC, intProfile, null, "", lblPlatformMAC, fldPlatformMAC, "BLADE_MAC", dsAsset.Tables[0].Rows[0]["macaddress"].ToString(), "", false, true);
                                if (Request.QueryString["mac"] != null && Request.QueryString["mac"] != "")
                                    txtPlatformMAC.Text = oFunction.decryptQueryString(Request.QueryString["mac"]);
                                if (Request.QueryString["time"] != null && Request.QueryString["time"] != "")
                                {
                                    btnMAC.Enabled = false;
                                    btnMAC.Text = Request.QueryString["time"] + " secs";
                                }
                                btnMAC.Visible = (oUser.IsAdmin(intProfile) == true);
                                oDataPoint.LoadTextBox(txtPlatformVLAN, intProfile, null, "", lblPlatformVLAN, fldPlatformVLAN, "BLADE_VLAN", dsAsset.Tables[0].Rows[0]["vlan"].ToString(), "", false, true);
                                oDataPoint.LoadDropDown(ddlPlatformBuildNetwork, intProfile, null, "", lblPlatformBuildNetwork, fldPlatformBuildNetwork, "BLADE_BUILD_NETWORK", "name", "id", oSolaris.GetBuildNetworks(1), Int32.Parse(dsAsset.Tables[0].Rows[0]["build_network_id"].ToString()), false, false, false);
                                oDataPoint.LoadTextBox(txtEnclosureSlot, intProfile, null, "", lblEnclosureSlot, fldEnclosureSlot, "BLADE_SLOT", intSlot.ToString(), "", false, true);
                                lblFullHeight.Text = (oModelsProperties.IsFullHeight(intModel) ? " AND " + oAsset.GetBladeFullHeight(intAsset) : "");
                                oDataPoint.LoadCheckBox(chkEnclosureSpare, intProfile, null, "", lblEnclosureSpare, fldEnclosureSpare, "BLADE_SPARE", (dsAsset.Tables[0].Rows[0]["spare"].ToString() == "1"), "", false);
                                int intResiliency = 0;
                                Int32.TryParse(dsAsset.Tables[0].Rows[0]["resiliencyid"].ToString(), out intResiliency);
                                oDataPoint.LoadDropDown(ddlPlatformResiliency, intProfile, null, "", lblPlatformResiliency, fldPlatformResiliency, "BLADE_RESILIENCY", "name", "id", oResiliency.Gets(1), intResiliency, false, false, true);
                                int intOperatingSystemGroup = 0;
                                Int32.TryParse(dsAsset.Tables[0].Rows[0]["operatingsystemgroupid"].ToString(), out intOperatingSystemGroup);
                                oDataPoint.LoadDropDown(ddlPlatformOperatingSystemGroup, intProfile, null, "", lblPlatformOperatingSystemGroup, fldPlatformOperatingSystemGroup, "BLADE_OS_GROUP", "name", "id", oOperatingSystem.GetGroups(1), intOperatingSystemGroup, false, false, false);
                                oDataPoint.LoadDropDown(ddlEnclosure, intProfile, btnEnclosure, "/datapoint/asset/enclosure.aspx?t=serial&q=" + oFunction.encryptQueryString(oAsset.Get(intEnclosure, "serial")) + "&id=" + oFunction.encryptQueryString(intEnclosure.ToString()) + "&highlight=" + intAsset.ToString(), lblEnclosure, fldEnclosure, "BLADE_ENCLOSURE", "name", "id", oAsset.GetEnclosures((int)AssetStatus.InUse), intEnclosure, false, false, true);
                                oDataPoint.LoadTextBox(txtEnclosureSerial, intProfile, null, "", lblEnclosureSerial, fldEnclosureSerial, "BLADE_ENCLOSURE_SERIAL", oAsset.Get(intEnclosure, "serial"), "", true, false);
                                oDataPoint.LoadTextBox(txtEnclosureAsset, intProfile, null, "", lblEnclosureAsset, fldEnclosureAsset, "BLADE_ENCLOSURE_ASSET", oAsset.Get(intEnclosure, "asset"), "", true, false);

                                //DR Counterpart
                                int intDR = oAsset.GetDR(intAsset);
                                string strDR = oAsset.Get(intDR, "serial");
                                if (intDR > 0)
                                    oDataPoint.LoadTextBox(txtPlatformDRCounterPart, intProfile, btnPlatformDRCounterPart, "/datapoint/asset/datapoint_asset_search.aspx?t=serial&q=" + oFunction.encryptQueryString(strDR) + "&id=" + oFunction.encryptQueryString(intDR.ToString()), lblPlatformDRCounterPart, fldPlatformDRCounterPart, "BLADE_DR", strDR, "", true, false);
                                else
                                    oDataPoint.LoadTextBox(txtPlatformDRCounterPart, intProfile, null, "", lblPlatformDRCounterPart, fldPlatformDRCounterPart, "BLADE_DR", "", "", true, false);
                                
                                ddlStatus.SelectedValue = dsAsset.Tables[0].Rows[0]["status"].ToString();
                                ddlStatus.Enabled = (intAssetAttribute == (int)AssetAttribute.Ok);
                                panStatus.Visible = (ddlStatus.Enabled == false);
                            }
                            else
                                Response.Redirect("/datapoint/asset/blade_deploy.aspx?t=serial&q=" + oFunction.encryptQueryString(strSerial) + "&id=" + oFunction.encryptQueryString(intAsset.ToString()) + "&r=0");

                            if (oModelsProperties.IsDell(intModel) == false)
                            {
                                divNetworkAdapter.Visible = true;
                                // Get Network Adapter(s)
                                bool boolCanUpdateVirtualConnect = false;
                                oDataPoint.LoadPanel(panNetworkAdapterQuery, intProfile, fldNetworkAdapter, "BLADE_NETWORK_ADAPTER");
                                if (panNetworkAdapterQuery.Visible == true)
                                    boolCanUpdateVirtualConnect = true;
                                else
                                    panNetworkAdapterQuery.Visible = true;

                                if (Request.Cookies["networkAdapter"] != null && Request.Cookies["networkAdapter"].Value != "")
                                {
                                    panNetworkAdapterQuery.Visible = false;
                                    DateTime datStart = DateTime.Parse(Request.Cookies["networkAdapter"].Value);
                                    Response.Cookies["networkAdapter"].Value = "";
                                    DataTable tabNetworks = oAsset.GetVirtualConnectVLANs(intEnclosure, intEnvironment, "", dsn);
                                    // Load the DDL's with the available networks
                                    ddlAdapter1VLAN.DataValueField = "name";
                                    ddlAdapter1VLAN.DataTextField = "name";
                                    ddlAdapter1VLAN.DataSource = tabNetworks;
                                    ddlAdapter1VLAN.DataBind();
                                    //ddlAdapter1VLAN.Items.Insert(0, new ListItem("Unassigned", "<Unassigned>"));
                                    ddlAdapter1VLAN.Items.Insert(0, new ListItem("Unassigned", ""));
                                    ddlAdapter2VLAN.DataValueField = "name";
                                    ddlAdapter2VLAN.DataTextField = "name";
                                    ddlAdapter2VLAN.DataSource = tabNetworks;
                                    ddlAdapter2VLAN.DataBind();
                                    //ddlAdapter2VLAN.Items.Insert(0, new ListItem("Unassigned", "<Unassigned>"));
                                    ddlAdapter2VLAN.Items.Insert(0, new ListItem("Unassigned", ""));

                                    bool boolQueryError = false;
                                    if (intModelParent > 0)
                                    {
                                        if (oModel.Get(intModelParent, "grouping") == "2")
                                        {
                                            // Midrange
                                            boolQueryError = true;
                                        }
                                        else
                                        {
                                            // Distributed
                                            string strProfile = oAsset.GetVirtualConnectSetting(intAsset, "Server Profile", intEnvironment);
                                            lblProfile.Text = strProfile;
                                            AssetPowerStatus powStatus = oAsset.PowerStatus(intAsset, 0, intEnvironment, oFunction.GetSetupValuesByKey("RACADM_WEB").Tables[0].Rows[0]["value"].ToString());
                                            lblPower.Text = powStatus.ToString();
                                            if (strProfile != "")
                                            {
                                                string strResults = oAsset.ExecuteVirtualConnectHelper(intAsset, "show profile " + strProfile, intEnvironment);
                                                try
                                                {
                                                    string strResult = strResults.Substring(strResults.IndexOf("Ethernet Network Connections"));
                                                    strResult = strResult.Substring(strResult.IndexOf("==\n") + 3);
                                                    strResult = strResult.Substring(strResult.IndexOf("==\n") + 3);
                                                    strResult = strResult.Substring(0, strResult.IndexOf("\nFC SAN Connections"));
                                                    char[] strSplit = { '\n' };
                                                    string[] strMACs = strResult.Split(strSplit);
                                                    for (int ii = 0; ii < strMACs.Length; ii++)
                                                    {
                                                        if (strMACs[ii].Trim() != "")
                                                        {
                                                            string strMAC = strMACs[ii].Trim();
                                                            while (strMAC.Contains("  ") == true)
                                                                strMAC = strMAC.Replace("  ", " ");
                                                            string strResultValue = strMAC;
                                                            string strNumber = strResultValue.Substring(0, strResultValue.IndexOf(" "));
                                                            strResultValue = strResultValue.Substring(strResultValue.IndexOf(" ") + 1);
                                                            string strVLAN = strResultValue.Substring(0, strResultValue.IndexOf(" "));
                                                            strResultValue = strResultValue.Substring(strResultValue.IndexOf(" ") + 1);
                                                            string strStatus = strResultValue.Substring(0, strResultValue.IndexOf(" "));
                                                            strResultValue = strResultValue.Substring(strResultValue.IndexOf(" ") + 1);
                                                            string strPXE = strResultValue.Substring(0, strResultValue.IndexOf(" "));
                                                            strResultValue = strResultValue.Substring(strResultValue.IndexOf(" ") + 1);
                                                            // At this point, either the MAC address alone or the MAC address with information at the end exists.
                                                            if (strResultValue.IndexOf(" ") > -1)
                                                            {
                                                                // Get rid of the information at the end
                                                                strMAC = strResultValue.Substring(0, strResultValue.IndexOf(" "));
                                                            }
                                                            else
                                                            {
                                                                strMAC = strResultValue;
                                                                // Nothing to do since only the MAC address exists
                                                            }

                                                            if (ii == 0)
                                                            {
                                                                ddlAdapter1VLAN.Enabled = false;
                                                                try
                                                                {
                                                                    ddlAdapter1VLAN.SelectedValue = strVLAN;
                                                                }
                                                                catch { }
                                                                lblAdapter1Status.Text = strStatus;
                                                                ddlAdapter1PXE.Enabled = false;
                                                                ddlAdapter1PXE.SelectedValue = strPXE;
                                                                lblAdapter1MAC.Text = oFunction.FormatMAC(strMAC, ":");
                                                            }
                                                            else
                                                            {
                                                                ddlAdapter2VLAN.Enabled = false;
                                                                try
                                                                {
                                                                    ddlAdapter2VLAN.SelectedValue = strVLAN;
                                                                }
                                                                catch { }
                                                                lblAdapter2Status.Text = strStatus;
                                                                ddlAdapter2PXE.Enabled = false;
                                                                ddlAdapter2PXE.SelectedValue = strPXE;
                                                                lblAdapter2MAC.Text = oFunction.FormatMAC(strMAC, ":");
                                                            }
                                                        }
                                                    }
                                                }
                                                catch
                                                {
                                                    boolQueryError = true;
                                                }
                                            }
                                            else
                                                boolQueryError = true;
                                        }
                                    }
                                    if (boolQueryError == true)
                                    {
                                        // Check here for error
                                        panNetworkAdapterError.Visible = true;
                                    }
                                    else
                                    {
                                        panNetworkAdapterDone.Visible = true;
                                        TimeSpan oSpan = DateTime.Now.Subtract(datStart);
                                        btnNetworkAdapter.Enabled = false;
                                        btnNetworkAdapter.Text = "Query Time: " + oSpan.TotalSeconds.ToString("0") + " seconds...";
                                    }

                                    bool boolUpdatedSuccess = false;
                                    bool boolUpdatedError = false;
                                    if (Request.Cookies["networkAdapter1"] != null && Request.Cookies["networkAdapter1"].Value != "")
                                    {
                                        boolUpdatedSuccess = true;
                                        if (Request.Cookies["networkAdapter1"].Value == "Success")
                                            lblAdapter1Result.Text = "<img src='/images/check.gif' border='0' align='absmiddle'/>&nbsp;" + Request.Cookies["networkAdapter1"].Value;
                                        else
                                        {
                                            boolUpdatedError = true;
                                            lblAdapter1Result.Text = "<img src='/images/cancel.gif' border='0' align='absmiddle'/>&nbsp;" + Request.Cookies["networkAdapter1"].Value;
                                        }
                                        Response.Cookies["networkAdapter1"].Value = "";
                                    }
                                    if (Request.Cookies["networkAdapter2"] != null && Request.Cookies["networkAdapter2"].Value != "")
                                    {
                                        boolUpdatedSuccess = true;
                                        if (Request.Cookies["networkAdapter2"].Value == "Success")
                                            lblAdapter2Result.Text = "<img src='/images/check.gif' border='0' align='absmiddle'/>&nbsp;" + Request.Cookies["networkAdapter2"].Value;
                                        else
                                        {
                                            boolUpdatedError = true;
                                            lblAdapter2Result.Text = "<img src='/images/cancel.gif' border='0' align='absmiddle'/>&nbsp;" + Request.Cookies["networkAdapter2"].Value;
                                        }
                                        Response.Cookies["networkAdapter2"].Value = "";
                                    }
                                    if (boolUpdatedError == true)
                                        panNetworkAdapterError.Visible = true;
                                    else if (boolUpdatedSuccess == true)
                                        panNetworkAdapterSave.Visible = true;
                                }

                                if (boolCanUpdateVirtualConnect == true)
                                {
                                    chkAdpater1.Enabled = true;
                                    chkAdpater2.Enabled = true;
                                }
                            }
                            else
                            {
                                divNexus.Visible = true;
                                string strEnclosure = oAsset.GetStatus(intEnclosure, "name");
                                DataSet dsDellBlade = oAsset.GetDellBladeSwitchports(strEnclosure, intSlot);
                                if (dsDellBlade.Tables[0].Rows.Count == 1)
                                {
                                    DataRow drDellBlade = dsDellBlade.Tables[0].Rows[0];
                                    string strSwitchA = drDellBlade["switchA"].ToString();
                                    string strInterfaceA = drDellBlade["interfaceA"].ToString();
                                    string strSwitchB = drDellBlade["switchB"].ToString();
                                    string strInterfaceB = drDellBlade["interfaceB"].ToString();

                                    lblSwitchA.Text = strSwitchA;
                                    lblInterfaceA.Text = strInterfaceA;
                                    lblConfigA.Text = "";
                                    lblSwitchB.Text = strSwitchB;
                                    lblInterfaceB.Text = strInterfaceB;
                                    lblConfigB.Text = "";

                                    oDataPoint.LoadPanel(panNexusConfiguration, intProfile, fldNexusConfiguration, "BLADE_NEXUS_CONFIG");
                                    if (panNexusConfiguration.Visible == true)
                                    {
                                        if (Request.Cookies["nexus"] != null && Request.Cookies["nexus"].Value != "")
                                        {
                                            panNetworkAdapterQuery.Visible = false;
                                            DateTime datStart = DateTime.Parse(Request.Cookies["nexus"].Value);
                                            Response.Cookies["nexus"].Value = "";

                                            Variables oVariable = new Variables(intEnvironment);
                                            Ping oPingDellBlade = new Ping();
                                            string strPingStatusA = "";
                                            try
                                            {
                                                PingReply oReplyA = oPingDellBlade.Send(strSwitchA);
                                                strPingStatusA = oReplyA.Status.ToString().ToUpper();
                                            }
                                            catch { }
                                            if (strPingStatusA == "SUCCESS")
                                            {
                                                try
                                                {
                                                    SshShell oSSHshell = new SshShell(strSwitchA, oVariable.NexusUsername(), oVariable.NexusPassword());
                                                    oSSHshell.RemoveTerminalEmulationCharacters = true;
                                                    oSSHshell.Connect();
                                                    string strLogin = oAsset.GetDellSwitchportOutput(oSSHshell);
                                                    if (strLogin != "**INVALID**")
                                                    {
                                                        string strConfigA = oAsset.GetDellSwitchportOutput(oSSHshell, strInterfaceA, DellBladeSwitchportType.Config, intAsset);
                                                        lblConfigA.Text = strConfigA.Replace(Environment.NewLine, "<br/>");
                                                    }
                                                    else
                                                        lblConfigA.Text = "There was a problem logging into the Switch";
                                                }
                                                catch (Exception exNexus)
                                                {
                                                    lblConfigA.Text = "There was a problem connecting to the Switch - " + exNexus.Message;
                                                    Request.Cookies["nexus"].Value = "";
                                                }
                                            }
                                            else
                                                lblConfigA.Text = "There was a problem pinging the Switch";

                                            string strPingStatusB = "";
                                            try
                                            {
                                                PingReply oReplyB = oPingDellBlade.Send(strSwitchB);
                                                strPingStatusB = oReplyB.Status.ToString().ToUpper();
                                            }
                                            catch { }
                                            if (strPingStatusB == "SUCCESS")
                                            {
                                                try
                                                {
                                                    SshShell oSSHshell = new SshShell(strSwitchB, oVariable.NexusUsername(), oVariable.NexusPassword());
                                                    oSSHshell.RemoveTerminalEmulationCharacters = true;
                                                    oSSHshell.Connect();
                                                    string strLogin = oAsset.GetDellSwitchportOutput(oSSHshell);
                                                    if (strLogin != "**INVALID**")
                                                    {
                                                        string strConfigB = oAsset.GetDellSwitchportOutput(oSSHshell, strInterfaceB, DellBladeSwitchportType.Config, intAsset);
                                                        lblConfigB.Text = strConfigB.Replace(Environment.NewLine, "<br/>");
                                                    }
                                                    else
                                                        lblConfigB.Text = "There was a problem logging into the Switch";
                                                }
                                                catch (Exception exNexus)
                                                {
                                                    lblConfigA.Text = "There was a problem connecting to the Switch - " + exNexus.Message;
                                                    Request.Cookies["nexus"].Value = "";
                                                }
                                            }
                                            else
                                                lblConfigB.Text = "There was a problem pinging the Switch";

                                            TimeSpan oSpan = DateTime.Now.Subtract(datStart);
                                            btnNexus.Enabled = false;
                                            btnNexus.Text = "Query Time: " + oSpan.TotalSeconds.ToString("0") + " seconds...";
                                        }
                                    }
                                }
                            }

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
                            rptHistory.DataSource = oAsset.GetProvisioningHistory(oDataPoint.AssetHistorySelect(intAsset));
                            rptHistory.DataBind();
                            lblHistory.Visible = (rptHistory.Items.Count == 0);
                            oDataPoint.LoadPanel(panProvisioning, intProfile, fldProvisioning, "BLADE_STATUS");

                            // WWW
                            rptWWW.DataSource = oAsset.GetHBA(intAsset);
                            rptWWW.DataBind();
                            lblWWW.Visible = (rptWWW.Items.Count == 0);
                            oDataPoint.LoadButton(btnWWW, intProfile, fldWWW, "BLADE_WWW", "return OpenWindow('ASSET_DEPLOY_HBAs','" + intAsset.ToString() + "');");

                            // Redeploy
                            oDataPoint.LoadButton(btnRedeploy, intProfile, fldRedeploy, "BLADE_REDEPLOY");
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

                            if (panAdministration.Visible == true)
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
                                                    lblAssetOrderLocation.Text = oLocation.GetFull(intOrderLocation);
                                                else
                                                    lblAssetOrderLocation.Text = "<i>Any Data Center</i>";
                                            }
                                        }
                                        else
                                            lblAssetOrderLocation.Text = "<i>N / A</i>";
                                        
                                        int intEnclosureAddress = 0;
                                        if (intOrderLocation > 0 && Int32.TryParse(oAsset.GetEnclosure(intEnclosure, "addressid"), out intEnclosureAddress) == true)
                                        {
                                            if (lblAssetOrderLocation.Text != oLocation.GetFull(intEnclosureAddress))
                                                lblAssetOrderLocation.CssClass = "reddefault";
                                        }

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
                btnNetworkAdapter.Attributes.Add("onclick", "ProcessButton(this,'Querying... please be patient...','225') && ProcessControlButton();");
                btnNexus.Attributes.Add("onclick", "ProcessButton(this,'Querying... please be patient...','225') && ProcessControlButton();");
                chkAdpater1.Attributes.Add("onclick", "EnableDisableCheck(this,'" + ddlAdapter1VLAN.ClientID + "') && EnableDisableCheck(this,'" + ddlAdapter1PXE.ClientID + "');");
                chkAdpater2.Attributes.Add("onclick", "EnableDisableCheck(this,'" + ddlAdapter2VLAN.ClientID + "') && EnableDisableCheck(this,'" + ddlAdapter2PXE.ClientID + "');");
                btnZeus.Attributes.Add("onclick", "ProcessButton(this) && ProcessControlButton();");
                btnAltiris.Attributes.Add("onclick", "ProcessButton(this) && ProcessControlButton();");
            }
            else
                panDenied.Visible = true;
        }
        private void LoadList()
        {
            ddlPlatformBuildNetwork.DataValueField = "id";
            ddlPlatformBuildNetwork.DataTextField = "name";
            ddlPlatformBuildNetwork.DataSource = oSolaris.GetBuildNetworks(1);
            ddlPlatformBuildNetwork.DataBind();

        }
        protected void btnNew_Click(Object Sender, EventArgs e)
        {
            Response.Redirect("/datapoint/asset/datapoint_asset_search.aspx");
        }
        protected void btnSave_Click(Object Sender, EventArgs e)
        {
            int intSave = Save();
            Response.Redirect(Request.Path + "?t=" + Request.QueryString["t"] + "&q=" + Request.QueryString["q"] + "&id=" + oFunction.encryptQueryString(intAsset.ToString()) + "&" + (intSave > 0 ? "save" : "error") + "=" + intSave.ToString() + (intSaveMenuTab > 0 ? "&menu_tab=" + intSaveMenuTab.ToString() : ""));
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

            // Virtual Connect...
            if (chkAdpater1.Checked == true)
            {
                string strResultVC1 = oAsset.ExecuteVirtualConnectIP(intAsset, 0, intEnvironment, ddlAdapter1VLAN.SelectedItem.Value, 1, (ddlAdapter1PXE.SelectedItem.Value == "Enabled"), (ddlAdapter1PXE.SelectedItem.Value == "Disabled"), (ddlAdapter1PXE.SelectedItem.Value == "UseBIOS"), true);
                if (strResultVC1.Contains("ERROR") == true || strResultVC1 == "")
                    Response.Cookies["networkAdapter1"].Value = strResultVC1;
                else
                    Response.Cookies["networkAdapter1"].Value = "Success";
                Response.Cookies["networkAdapter"].Value = DateTime.Now.ToString();
                intSaveMenuTab = 3;
            }
            if (chkAdpater2.Checked == true)
            {
                string strResultVC2 = oAsset.ExecuteVirtualConnectIP(intAsset, 0, intEnvironment, ddlAdapter2VLAN.SelectedItem.Value, 2, (ddlAdapter2PXE.SelectedItem.Value == "Enabled"), (ddlAdapter2PXE.SelectedItem.Value == "Disabled"), (ddlAdapter2PXE.SelectedItem.Value == "UseBIOS"), true);
                if (strResultVC2.Contains("ERROR") == true || strResultVC2 == "")
                    Response.Cookies["networkAdapter2"].Value = strResultVC2;
                else
                    Response.Cookies["networkAdapter2"].Value = "Success";
                Response.Cookies["networkAdapter"].Value = DateTime.Now.ToString();
                intSaveMenuTab = 3;
            }

            //int intReturn = oServer.DeviceName_Update(intAsset, txtName.Text, intProfile, txtPlatformSerial.Text, dsnAsset, Request.Form[hdnPNC.UniqueID]);
            int intReturn = 1;
            if (intReturn > 0)
                oAsset.UpdateBlade(intAsset, txtName.Text, Int32.Parse(ddlStatus.SelectedItem.Value), intProfile, DateTime.Now, Int32.Parse(ddlEnclosure.SelectedItem.Value), Int32.Parse(ddlPlatformClass.SelectedItem.Value), Int32.Parse(Request.Form[hdnEnvironment.UniqueID]), txtPlatformILO.Text, txtPlatformDummy.Text, txtPlatformMAC.Text, Int32.Parse(txtPlatformVLAN.Text), Int32.Parse(ddlPlatformBuildNetwork.SelectedItem.Value), Int32.Parse(txtEnclosureSlot.Text), (chkEnclosureSpare.Checked ? 1 : 0), Int32.Parse(ddlPlatformResiliency.SelectedItem.Value), Int32.Parse(ddlPlatformOperatingSystemGroup.SelectedItem.Value));
            return intReturn;
        }
        protected void btnRedeploy_Click(Object Sender, EventArgs e)
        {
            oAsset.DeleteBlade(intAsset, true, intProfile);
            Response.Redirect("/datapoint/asset/datapoint_asset_search.aspx?t=serial&q=" + oFunction.encryptQueryString(oAsset.Get(intAsset, "serial")) + "&r=0");
        }
        protected void btnZeus_Click(Object Sender, EventArgs e)
        {
            Variables oVariable = new Variables(intEnvironment);
            string strMACAddress = oAsset.GetVirtualConnectMACAddress(intAsset, 0, intEnvironment, 1, oVariable.DocumentsFolder(), dsn, oFunction.GetSetupValuesByKey("RACADM_WEB").Tables[0].Rows[0]["value"].ToString(), oLog, txtName.Text);
            if (strMACAddress == "**ERROR**")
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
            string strMACAddress = oAsset.GetVirtualConnectMACAddress(intAsset, 0, intEnvironment, 1, oVariable.DocumentsFolder(), dsn, oFunction.GetSetupValuesByKey("RACADM_WEB").Tables[0].Rows[0]["value"].ToString(), oLog, txtName.Text);
            if (strMACAddress == "**ERROR**")
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
        protected void btnNetworkAdapter_Click(Object Sender, EventArgs e)
        {
            Response.Cookies["networkAdapter"].Value = DateTime.Now.ToString();
            Response.Redirect(Request.Path + "?t=" + Request.QueryString["t"] + "&q=" + Request.QueryString["q"] + "&id=" + oFunction.encryptQueryString(intAsset.ToString()) + "&menu_tab=3");
        }
        protected void btnNexus_Click(Object Sender, EventArgs e)
        {
            Response.Cookies["nexus"].Value = DateTime.Now.ToString();
            Response.Redirect(Request.Path + "?t=" + Request.QueryString["t"] + "&q=" + Request.QueryString["q"] + "&id=" + oFunction.encryptQueryString(intAsset.ToString()) + "&menu_tab=3");
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
    }
}
