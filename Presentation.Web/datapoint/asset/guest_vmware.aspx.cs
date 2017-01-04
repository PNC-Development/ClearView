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

namespace NCC.ClearView.Presentation.Web
{
    public partial class guest_vmware : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
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
            oAsset = new Asset(intProfile, dsnAsset);
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
                        Master.Page.Title = "DataPoint | VMware (" + strHeader + ")";
                        lblHeaderSub.Text = "Provides all the information about a VMware guest...";
                        int intMenuTab = 0;
                        if (Request.QueryString["menu_tab"] != null && Request.QueryString["menu_tab"] != "")
                            intMenuTab = Int32.Parse(Request.QueryString["menu_tab"]);
                        Tab oTab = new Tab(hdnTab.ClientID, intMenuTab, "divMenu1", true, false);
                        oTab.AddTab("Asset Information", "");
                        oTab.AddTab("Host Information", "");
                        oTab.AddTab("Resource Dependencies", "");
                        oTab.AddTab("Provisioning Information", "");
                        //oTab.AddTab("Network Adapter Configuration", "");
                        strMenuTab1 = oTab.GetTabs();

                        if (!IsPostBack)
                        {
                            //DataSet dsAssets = oServer.GetAsset(intAsset);
                            //foreach (DataRow drAsset in dsAssets.Tables[0].Rows)
                            //{
                            //    if (drAsset["latest"].ToString() == "1")
                            //    {
                            //        intAsset = Int32.Parse(drAsset["assetid"].ToString());
                            //        intAssetClass = Int32.Parse(drAsset["classid"].ToString());
                            //        intAssetEnv = Int32.Parse(drAsset["environmentid"].ToString());
                            //        break;
                            //    }
                            //}

                            // Asset Information
                            oDataPoint.LoadTextBox(txtPlatformSerial, intProfile, null, "", lblPlatformSerial, fldPlatformSerial, "VMWARE_SERIAL", strSerial, "", false, true);
                            oDataPoint.LoadTextBox(txtPlatformAsset, intProfile, null, "", lblPlatformAsset, fldPlatformAsset, "VMWARE_ASSET", strAsset, "", false, true);

                            int intAssetAttribute = 0;
                            Int32.TryParse(oAsset.Get(intAsset, "asset_attribute"), out intAssetAttribute);
                            oDataPoint.LoadDropDown(ddlAssetAttribute, intProfile, null, "", lblAssetAttribute, fldAssetAttribute, "ASSET_ATTRIBUTE", "Name", "AttributeId", oAsset.getAssetAttributes(null, "", 1), intAssetAttribute, true, false, false);
                            oDataPoint.LoadTextBox(txtAssetAttributeComment, intProfile, null, "", lblAssetAttributeComment, fldAssetAttributeComment, "ASSET_ATTRIBUTE_COMMENT", oAsset.getAssetAttributesComments(intAsset), "", false, true);
                            ddlAssetAttribute.Attributes.Add("onclick", "return SetControlsForAssetAttributes()");

                            ddlPlatform.Attributes.Add("onchange", "PopulatePlatformTypes('" + ddlPlatform.ClientID + "','" + ddlPlatformType.ClientID + "','" + ddlPlatformModel.ClientID + "','" + ddlPlatformModelProperty.ClientID + "');ResetDropDownHidden('" + hdnModel.ClientID + "');");
                            ddlPlatformType.Attributes.Add("onchange", "PopulatePlatformModels('" + ddlPlatformType.ClientID + "','" + ddlPlatformModel.ClientID + "','" + ddlPlatformModelProperty.ClientID + "');ResetDropDownHidden('" + hdnModel.ClientID + "');");
                            ddlPlatformModel.Attributes.Add("onchange", "PopulatePlatformModelProperties('" + ddlPlatformModel.ClientID + "','" + ddlPlatformModelProperty.ClientID + "');ResetDropDownHidden('" + hdnModel.ClientID + "');");
                            ddlPlatformModelProperty.Attributes.Add("onchange", "UpdateDropDownHidden('" + ddlPlatformModelProperty.ClientID + "','" + hdnModel.ClientID + "');");
                            int intModel = 0;
                            Int32.TryParse(oAsset.Get(intAsset, "modelid"), out intModel);

                            hdnModel.Value = intModel.ToString();
                            int intModelParent = 0;
                            Int32.TryParse(oModelsProperties.Get(intModel, "modelid"), out intModelParent);
                            int intType = oModel.GetType(intModelParent);
                            int intPlatform = oType.GetPlatform(intType);
                            oDataPoint.LoadDropDown(ddlPlatform, intProfile, null, "", lblPlatform, fldPlatform, "VMWARE_PLATFORM", "name", "platformid", oPlatform.Gets(1), intPlatform, false, false, true);
                            oDataPoint.LoadDropDown(ddlPlatformType, intProfile, null, "", lblPlatformType, fldPlatformType, "VMWARE_TYPE", "name", "id", oType.Gets(intPlatform, 1), intType, false, false, true);
                            oDataPoint.LoadDropDown(ddlPlatformModel, intProfile, null, "", lblPlatformModel, fldPlatformModel, "VMWARE_MODEL", "name", "id", oModel.Gets(intType, 1), intModelParent, false, false, true);
                            oDataPoint.LoadDropDown(ddlPlatformModelProperty, intProfile, null, "", lblPlatformModelProperty, fldPlatformModelProperty, "VMWARE_MODEL_PROP", "name", "id", oModelsProperties.GetModels(0, intModelParent, 1), intModel, false, false, true);

                            // Get Asset
                            DataSet dsAsset = oAsset.GetGuest(intAsset);
                            if (dsAsset.Tables[0].Rows.Count > 0)
                            {
                                oDataPoint.LoadTextBoxDeviceName(txtName, btnName, null, true, hdnPNC, intProfile, btnNameLookup, "/datapoint/asset/datapoint_asset_search.aspx?t=name&q=" + oFunction.encryptQueryString(dsAsset.Tables[0].Rows[0]["name"].ToString()), lblName, fldName, "VMWARE_NAME", dsAsset.Tables[0].Rows[0]["name"].ToString(), "", false, false);
                                if (txtName.Text != "")
                                    lblHeader.Text += "&nbsp;&nbsp;&nbsp;[" + txtName.Text + "]";
                                lblStatus.Text = dsAsset.Tables[0].Rows[0]["statusname"].ToString();
                                int intClass = Int32.Parse(dsAsset.Tables[0].Rows[0]["classid"].ToString());
                                int intEnv = Int32.Parse(dsAsset.Tables[0].Rows[0]["environmentid"].ToString());
                                hdnEnvironment.Value = intEnv.ToString();
                                oDataPoint.LoadDropDown(ddlPlatformClass, intProfile, null, "", lblPlatformClass, fldPlatformClass, "VMWARE_CLASS", "name", "id", oClass.Gets(1), intClass, false, false, true);
                                oDataPoint.LoadDropDown(ddlPlatformEnvironment, intProfile, null, "", lblPlatformEnvironment, fldPlatformEnvironment, "VMWARE_ENVIRONMENT", "name", "id", oClass.GetEnvironment(intClass, 0), intEnv, false, false, true);
                                ddlStatus.SelectedValue = dsAsset.Tables[0].Rows[0]["status"].ToString();
                                ddlStatus.Enabled = (intAssetAttribute == (int)AssetAttribute.Ok);
                                panStatus.Visible = (ddlStatus.Enabled == false);
                            }
                            else
                                Response.Redirect("/datapoint/asset/datapoint_asset_search.aspx");

                            // Get Original Detail
                            VMWare oVMWare = new VMWare(intProfile, dsn);
                            DataSet dsGuest = oVMWare.GetGuest(dsAsset.Tables[0].Rows[0]["name"].ToString());
                            if (dsGuest.Tables[0].Rows.Count > 0)
                            {
                                panVMWare.Visible = true;
                                DataRow drGuest = dsGuest.Tables[0].Rows[0];
                                int intDatastore = Int32.Parse(drGuest["datastoreid"].ToString());
                                lblBuildDataStore.Text = oVMWare.GetDatastore(intDatastore, "name");
                                int intHost = Int32.Parse(drGuest["hostid"].ToString());
                                lblBuildHost.Text = oVMWare.GetHost(intHost, "name");
                                int intCluster = Int32.Parse(oVMWare.GetHost(intHost, "clusterid"));
                                lblBuildCluster.Text = oVMWare.GetCluster(intCluster, "name");
                                int intFolder = Int32.Parse(oVMWare.GetCluster(intCluster, "folderid"));
                                lblBuildFolder.Text = oVMWare.GetFolder(intFolder, "name");
                                int intDataCenter = Int32.Parse(oVMWare.GetFolder(intFolder, "datacenterid"));
                                lblBuildDataCenter.Text = oVMWare.GetDatacenter(intDataCenter, "name");
                                int intVirtualCenter = Int32.Parse(oVMWare.GetDatacenter(intDataCenter, "virtualcenterid"));
                                lblBuildVirtualCenter.Text = oVMWare.GetVirtualCenter(intVirtualCenter, "name");

                                // Get Host
                                if (Request.Cookies["host"] != null && Request.Cookies["host"].Value != "")
                                {
                                    string strHost = "";
                                    string strFind = txtName.Text;
                                    //strFind = "ohcleapp103d";
                                    DateTime datStart = DateTime.Parse(Request.Cookies["host"].Value);
                                    Response.Cookies["host"].Value = "";
                                    //VMWare oVMWare = new VMWare(intProfile, dsn);
                                    DataSet dsVirtualCenter = oVMWare.GetVirtualCenters(1);
                                    foreach (DataRow drVirtualCenter in dsVirtualCenter.Tables[0].Rows)
                                    {
                                        intVirtualCenter = Int32.Parse(drVirtualCenter["id"].ToString());
                                        string strVirtualCenter = drVirtualCenter["name"].ToString();
                                        string strVirtualCenterURL = drVirtualCenter["url"].ToString();
                                        int intVirtualCenterEnv = Int32.Parse(drVirtualCenter["environment"].ToString());
                                        DataSet dsDataCenter = oVMWare.GetDatacenters(intVirtualCenter, 1);
                                        foreach (DataRow drDataCenter in dsDataCenter.Tables[0].Rows)
                                        {
                                            intDataCenter = Int32.Parse(drDataCenter["id"].ToString());
                                            string strDataCenter = drDataCenter["name"].ToString();
                                            string strConnect = oVMWare.ConnectDEBUG(strVirtualCenterURL, intVirtualCenterEnv, strDataCenter);
                                            VimService _service = oVMWare.GetService();
                                            ServiceContent _sic = oVMWare.GetSic();
                                            try
                                            {
                                                ManagedObjectReference oVM = oVMWare.GetVM(strFind);
                                                GuestInfo ginfo = (GuestInfo)oVMWare.getObjectProperty(oVM, "guest");
                                                lblGuestName.Text = ginfo.guestFullName;
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
                                                VirtualMachineConfigInfo vminfo = (VirtualMachineConfigInfo)oVMWare.getObjectProperty(oVM, "config");
                                                VirtualMachineSummary oVMSummary = (VirtualMachineSummary)oVMWare.getObjectProperty(oVM, "summary");
                                                VirtualMachineConfigSummary oVMConfig = oVMSummary.config;
                                                lblRAM.Text = oVMConfig.memorySizeMB.ToString();
                                                lblCPUs.Text = oVMConfig.numCpu.ToString();
                                                lblPath.Text = oVMConfig.vmPathName;
                                                VirtualMachineRuntimeInfo oVMRuntime = oVMSummary.runtime;
                                                ManagedObjectReference oVMHost = oVMRuntime.host;
                                                strHost = (string)oVMWare.getObjectProperty(oVMHost, "name");
                                                if (strHost.IndexOf(".") > -1)
                                                    strHost = strHost.Substring(0, strHost.IndexOf("."));
                                                lblVirtualCenter.Text = strVirtualCenter;
                                                lblDataCenter.Text = strDataCenter;
                                                break;
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
                                        }
                                    }
                                    if (strHost != "")
                                        oDataPoint.LoadTextBox(txtHostName, intProfile, btnHostName, "/datapoint/asset/server.aspx?t=name&q=" + oFunction.encryptQueryString(strHost), lblHostName, fldHostName, "VMWARE_HOST", strHost, "", false, false);
                                    else
                                        panHostNo.Visible = true;
                                    TimeSpan oSpan = DateTime.Now.Subtract(datStart);
                                    btnHostQuery.Enabled = false;
                                    btnHostQuery.Text = "Query Time: " + oSpan.TotalSeconds.ToString("0") + " seconds...";
                                }
                                else
                                {
                                    txtHostName.Visible = false;
                                    lblHostName.Text = "---";
                                    lblDataCenter.Text = "---";
                                    lblVirtualCenter.Text = "---";
                                }
                                oDataPoint.LoadPanel(panHostQuery, intProfile, fldHostQuery, "VMWARE_HOST_QUERY");
                            }
                            else
                            {
                                Solaris oSolaris = new Solaris(intProfile, dsn);
                                DataSet dsServers = oServer.GetAssetsAsset(intAsset);
                                foreach (DataRow drServer in dsServers.Tables[0].Rows)
                                {
                                    DataSet dsSVE = oSolaris.GetSVEGuest(Int32.Parse(drServer["serverid"].ToString()));
                                    if (dsSVE.Tables[0].Rows.Count > 0)
                                    {
                                        panSVE.Visible = true;
                                        int intCluster = Int32.Parse(dsSVE.Tables[0].Rows[0]["clusterid"].ToString());
                                        lblSVECluster.Text = oSolaris.GetSVECluster(intCluster, "name");
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
                            oDataPoint.LoadPanel(panProvisioning, intProfile, fldProvisioning, "VMWARE_STATUS");
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
                        intID = Int32.Parse(ds.Tables[0].Rows[0]["id"].ToString());
                        Response.Redirect(Request.Path + "?t=" + Request.QueryString["t"] + "&q=" + Request.QueryString["q"] + "&id=" + oFunction.encryptQueryString(intID.ToString()));
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
                ddlPlatformClass.Attributes.Add("onchange", "PopulateEnvironments('" + ddlPlatformClass.ClientID + "','" + ddlPlatformEnvironment.ClientID + "',0);");
                ddlPlatformEnvironment.Attributes.Add("onchange", "UpdateDropDownHidden('" + ddlPlatformEnvironment.ClientID + "','" + hdnEnvironment.ClientID + "');");
                btnHostQuery.Attributes.Add("onclick", "ProcessButton(this,'Querying... please be patient...','225') && ProcessControlButton();");
            }
            else
                panDenied.Visible = true;
        }
        protected void btnHostQuery_Click(Object Sender, EventArgs e)
        {
            Response.Cookies["host"].Value = DateTime.Now.ToString();
            Response.Redirect(Request.Path + "?t=" + Request.QueryString["t"] + "&q=" + Request.QueryString["q"] + "&id=" + oFunction.encryptQueryString(intID.ToString()) + "&menu_tab=2");
        }
        protected void btnNew_Click(Object Sender, EventArgs e)
        {
            Response.Redirect("/datapoint/asset/datapoint_asset_search.aspx");
        }
        protected void btnSave_Click(Object Sender, EventArgs e)
        {
            int intSave = Save();
            Response.Redirect(Request.Path + "?t=" + Request.QueryString["t"] + "&q=" + Request.QueryString["q"] + "&id=" + oFunction.encryptQueryString(intID.ToString()) + "&" + (intSave > 0 ? "save" : "error") + "=" + intSave.ToString());
        }
        protected void btnSaveClose_Click(Object Sender, EventArgs e)
        {
            int intSave = Save();
            Response.Redirect(Request.Path + "?t=" + Request.QueryString["t"] + "&q=" + Request.QueryString["q"] + "&id=" + oFunction.encryptQueryString(intID.ToString()) + "&" + (intSave > 0 ? "close" : "error") + "=" + intSave.ToString());
        }
        protected int Save()
        {
            oAsset.Update(intAsset, Int32.Parse(Request.Form[hdnModel.UniqueID]), txtPlatformSerial.Text, txtPlatformAsset.Text, Int32.Parse(ddlAssetAttribute.SelectedValue));
            oAsset.addAssetAttributesForAsset(intAsset, Int32.Parse(ddlAssetAttribute.SelectedValue), txtAssetAttributeComment.Text.Trim(), intProfile);

            //int intReturn = oServer.DeviceName_Update(intAsset, txtName.Text, intProfile, txtPlatformSerial.Text, dsnAsset, Request.Form[hdnPNC.UniqueID]);
            int intReturn = 1;
            if (intReturn > 0)
                oAsset.UpdateGuest(intAsset, txtName.Text, Int32.Parse(ddlStatus.SelectedItem.Value), intProfile, DateTime.Now, Int32.Parse(ddlPlatformClass.SelectedItem.Value), Int32.Parse(Request.Form[hdnEnvironment.UniqueID]));
            return intReturn;
        }
    }
}
