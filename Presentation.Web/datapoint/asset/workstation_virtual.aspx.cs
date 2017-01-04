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

namespace NCC.ClearView.Presentation.Web
{
    public partial class workstation_virtual : BasePage
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
                        Master.Page.Title = "DataPoint | Virtual Workstation (" + strHeader + ")";
                        lblHeaderSub.Text = "Provides all the information about a Microsoft Virtual Workstation...";
                        int intMenuTab = 0;
                        if (Request.QueryString["menu_tab"] != null && Request.QueryString["menu_tab"] != "")
                            intMenuTab = Int32.Parse(Request.QueryString["menu_tab"]);
                        Tab oTab = new Tab(hdnTab.ClientID, intMenuTab, "divMenu1", true, false);
                        oTab.AddTab("Asset Information", "");
                        oTab.AddTab("Host Information", "");
                        oTab.AddTab("Account Information", "");
                        oTab.AddTab("Provisioning Information", "");
                        strMenuTab1 = oTab.GetTabs();

                        if (!IsPostBack)
                        {
                            // Asset Information
                            oDataPoint.LoadTextBox(txtPlatformSerial, intProfile, null, "", lblPlatformSerial, fldPlatformSerial, "WORKSTATION_VIRTUAL_SERIAL", strSerial, "", false, true);
                            oDataPoint.LoadTextBox(txtPlatformAsset, intProfile, null, "", lblPlatformAsset, fldPlatformAsset, "WORKSTATION_VIRTUAL_ASSET", strAsset, "", false, true);


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
                            oDataPoint.LoadDropDown(ddlPlatform, intProfile, null, "", lblPlatform, fldPlatform, "WORKSTATION_VIRTUAL_PLATFORM", "name", "platformid", oPlatform.Gets(1), intPlatform, false, false, true);
                            oDataPoint.LoadDropDown(ddlPlatformType, intProfile, null, "", lblPlatformType, fldPlatformType, "WORKSTATION_VIRTUAL_TYPE", "name", "id", oType.Gets(intPlatform, 1), intType, false, false, true);
                            oDataPoint.LoadDropDown(ddlPlatformModel, intProfile, null, "", lblPlatformModel, fldPlatformModel, "WORKSTATION_VIRTUAL_MODEL", "name", "id", oModel.Gets(intType, 1), intModelParent, false, false, true);
                            oDataPoint.LoadDropDown(ddlPlatformModelProperty, intProfile, null, "", lblPlatformModelProperty, fldPlatformModelProperty, "WORKSTATION_VIRTUAL_MODEL_PROP", "name", "id", oModelsProperties.GetModels(0, intModelParent, 1), intModel, false, false, true);

                            // Get Asset
                            DataSet dsAsset = oWorkstation.GetVirtualAsset(intAsset);
                            if (dsAsset.Tables[0].Rows.Count > 0)
                            {
                                oDataPoint.LoadTextBoxDeviceName(txtName, btnName, null, true, hdnPNC, intProfile, null, "", lblName, fldName, "WORKSTATION_VIRTUAL_NAME", dsAsset.Tables[0].Rows[0]["name"].ToString(), "", false, false);
                                if (txtName.Text != "")
                                    lblHeader.Text += "&nbsp;&nbsp;&nbsp;[" + txtName.Text + "]";
                                lblStatus.Text = dsAsset.Tables[0].Rows[0]["statusname"].ToString();
                                int intClass = Int32.Parse(dsAsset.Tables[0].Rows[0]["classid"].ToString());
                                int intEnv = Int32.Parse(dsAsset.Tables[0].Rows[0]["environmentid"].ToString());
                                hdnEnvironment.Value = intEnv.ToString();
                                oDataPoint.LoadDropDown(ddlPlatformClass, intProfile, null, "", lblPlatformClass, fldPlatformClass, "WORKSTATION_VIRTUAL_CLASS", "name", "id", oClass.Gets(1), intClass, false, false, true);
                                oDataPoint.LoadDropDown(ddlPlatformEnvironment, intProfile, null, "", lblPlatformEnvironment, fldPlatformEnvironment, "WORKSTATION_VIRTUAL_ENVIRONMENT", "name", "id", oClass.GetEnvironment(intClass, 0), intEnv, false, false, true);
                                ddlStatus.SelectedValue = dsAsset.Tables[0].Rows[0]["status"].ToString();
                                ddlStatus.Enabled = (intAssetAttribute == (int)AssetAttribute.Ok);
                                panStatus.Visible = (ddlStatus.Enabled == false);

                                int intOS = Int32.Parse(dsAsset.Tables[0].Rows[0]["osid"].ToString());
                                oDataPoint.LoadDropDown(ddlPlatformOS, intProfile, null, "", lblPlatformOS, fldPlatformOS, "WORKSTATION_VIRTUAL_OS", "name", "id", oOperatingSystem.Gets(1, 1), intOS, false, false, true);
                                oDataPoint.LoadDropDown(ddlPlatformServicePack, intProfile, null, "", lblPlatformServicePack, fldPlatformServicePack, "WORKSTATION_VIRTUAL_SP", "name", "id", oOperatingSystem.GetServicePack(intOS), Int32.Parse(dsAsset.Tables[0].Rows[0]["spid"].ToString()), false, false, true);
                                oDataPoint.LoadDropDown(ddlPlatformDomain, intProfile, null, "", lblPlatformDomain, fldPlatformDomain, "WORKSTATION_VIRTUAL_DOMAIN", "name", "id", oDomain.Gets(1), Int32.Parse(dsAsset.Tables[0].Rows[0]["domainid"].ToString()), false, false, true);
                                oDataPoint.LoadDropDown(ddlPlatformHDD, intProfile, null, "", lblPlatformHDD, fldPlatformHDD, "WORKSTATION_VIRTUAL_HDD", "name", "id", oVirtualHDD.GetVirtuals(1), Int32.Parse(dsAsset.Tables[0].Rows[0]["hddid"].ToString()), false, false, true);
                                oDataPoint.LoadDropDown(ddlPlatformRam, intProfile, null, "", lblPlatformRam, fldPlatformRam, "WORKSTATION_VIRTUAL_RAM", "name", "id", oVirtualRam.GetVirtuals(1), Int32.Parse(dsAsset.Tables[0].Rows[0]["ramid"].ToString()), false, false, true);
                                lblCPUs.Text = "1";
                            }
                            else
                                Response.Redirect("/datapoint/asset/datapoint_asset_search.aspx");

                            // Get Host
                            oDataPoint.LoadTextBox(txtHostName, intProfile, btnHostName, "/datapoint/asset/server.aspx?t=name&q=" + oFunction.encryptQueryString(dsAsset.Tables[0].Rows[0]["hostname"].ToString()), lblHostName, fldHostName, "WORKSTATION_VIRTUAL_HOST", dsAsset.Tables[0].Rows[0]["hostname"].ToString(), "", false, false);

                            // Account Information
                            DataSet dsAccounts = oWorkstation.GetAccountsVirtual(intAsset);
                            rptAccounts.DataSource = dsAccounts;
                            rptAccounts.DataBind();
                            lblNone.Visible = (rptAccounts.Items.Count == 0);

                            // Provioning History
                            rptHistory.DataSource = SqlHelper.ExecuteDataset(dsnAsset, CommandType.Text, oDataPoint.AssetHistorySelect(intAsset));
                            rptHistory.DataBind();
                            lblHistory.Visible = (rptHistory.Items.Count == 0);
                            oDataPoint.LoadPanel(panProvisioning, intProfile, fldProvisioning, "WORKSTATION_VIRTUAL_STATUS");
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
            }
            else
                panDenied.Visible = true;
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
                oAsset.UpdateGuest(intAsset, txtName.Text, Int32.Parse(ddlStatus.SelectedItem.Value), intProfile, DateTime.Now, Int32.Parse(ddlPlatformClass.SelectedItem.Value), Int32.Parse(Request.Form[hdnEnvironment.UniqueID]));
            return intReturn;
        }
    }
}
