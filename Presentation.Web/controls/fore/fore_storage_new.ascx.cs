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

namespace NCC.ClearView.Presentation.Web
{
    public partial class fore_storage_new : System.Web.UI.UserControl
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);

        protected int intStoragePerBladeOs = Int32.Parse(ConfigurationManager.AppSettings["STORAGE_PER_BLADE_OS"]);
        protected int intStoragePerBladeApp = Int32.Parse(ConfigurationManager.AppSettings["STORAGE_PER_BLADE_APP"]);
        protected int intProfile;
        protected Forecast oForecast;
        protected Platforms oPlatform;
        protected Models oModel;
        protected ModelsProperties oModelsProperties;
        protected Locations oLocation;
        protected Classes oClass;
        protected int intForecast;
        protected int intID = 0;
        protected int intStorageOS = 0;
        protected string strMenuTab1 = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oForecast = new Forecast(intProfile, dsn);
            oPlatform = new Platforms(intProfile, dsn);
            oModel = new Models(intProfile, dsn);
            oModelsProperties = new ModelsProperties(intProfile, dsn);
            oLocation = new Locations(intProfile, dsn);
            oClass = new Classes(intProfile, dsn);

            //Menus
            int intMenuTab = 0;
            if (Request.QueryString["menu_tab"] != null && Request.QueryString["menu_tab"] != "")
                intMenuTab = Int32.Parse(Request.QueryString["menu_tab"]);
            Tab oTab = new Tab("", intMenuTab, "divMenu1", true, false);
            //Tab oTab = new Tab(hdnType.ClientID, intMenuTab, "divMenu1", true, false);

            oTab.AddTab("Operating System Volumes", "");
            oTab.AddTab("Application / Data Volumes", "");
            strMenuTab1 = oTab.GetTabs();

            //End Menus


            if (Request.QueryString["parent"] != null && Request.QueryString["parent"] != "")
                intForecast = Int32.Parse(Request.QueryString["parent"]);
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                intID = Int32.Parse(Request.QueryString["id"]);
            if (Request.QueryString["step"] != null && Request.QueryString["step"] != "")
                panUpdate.Visible = true;
            else
                panNavigation.Visible = true;
            int intMinimumApp = 0;
            bool boolTest = false;
            bool boolProduction = false;
            bool boolQA = false;
            bool boolNone = false;
            bool boolRequired = false;
            bool boolNoReplication = false;
            bool boolHA = false;
           
            if (intID > 0)
            {
                DataSet ds = oForecast.GetAnswer(intID);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    bool boolHundred = false;
                    int intConfidence = Int32.Parse(ds.Tables[0].Rows[0]["confidenceid"].ToString());
                    if (intConfidence > 0)
                    {
                        Confidence oConfidence = new Confidence(intProfile, dsn);
                        string strConfidence = oConfidence.Get(intConfidence, "name");
                        if (strConfidence.Contains("100%") == true)
                            boolHundred = true;
                    }
                    if (boolHundred == true)
                    {
                        panUpdate.Visible = false;
                        panNavigation.Visible = false;
                        btnHundred.Visible = true;
                    }
                    int intPlatform = Int32.Parse(ds.Tables[0].Rows[0]["platformid"].ToString());
                    int intStep = Int32.Parse(ds.Tables[0].Rows[0]["step"].ToString());
                    boolTest = (ds.Tables[0].Rows[0]["test"].ToString() == "1");
                    int intQuantity = Int32.Parse(ds.Tables[0].Rows[0]["quantity"].ToString());
                    int intAddress = Int32.Parse(ds.Tables[0].Rows[0]["addressid"].ToString());
                    intForecast = Int32.Parse(ds.Tables[0].Rows[0]["forecastid"].ToString());
                    DataSet dsSteps = oForecast.GetSteps(intPlatform, 1);
                    if (dsSteps.Tables[0].Rows.Count == intStep)
                        btnNext.Text = "Finish";
                    if (intStep == 0 || intStep == 1)
                        btnBack.Enabled = false;
                    if (oLocation.GetAddress(intAddress, "storage") != "1")
                    {
                        radNo.Checked = true;
                        radYes.Enabled = false;
                        radYes.ToolTip = "Location " + oLocation.GetFull(intAddress) + " is not configured for storage";
                    }
                    else
                    {
                        boolProduction = oClass.IsProd(Int32.Parse(ds.Tables[0].Rows[0]["classid"].ToString()));
                        boolQA = oClass.IsQA(Int32.Parse(ds.Tables[0].Rows[0]["classid"].ToString()));
                        boolNone = (ds.Tables[0].Rows[0]["storage"].ToString() == "-2");
                        if (oForecast.IsHACluster(intID) == true)
                        {
                            boolRequired = true;
                            radNo.ToolTip = "Storage Required for Clusters";
                        }
                        boolNoReplication = oForecast.IsDROver48(intID, false);
                        boolHA = oForecast.IsHARoom(intID);
                        // Get Model
                        int intModel = oForecast.GetModel(intID);
                        if (oModelsProperties.IsStorageDE_FDriveMustBeOnSAN(intModel) == true)
                        {
                            boolRequired = true;
                            radNo.ToolTip = "Storage Required for F: Drive";
                        }
                        if (oModelsProperties.IsStorageDB_BootSANWindows(intModel) == true)
                        {
                            boolRequired = true;
                            radNo.ToolTip = "Storage Required for Boot from SAN";
                            intStorageOS = intQuantity * intStoragePerBladeOs;
                            intMinimumApp = intQuantity * intStoragePerBladeApp;
                        }
                        if (boolRequired == true)
                        {
                            radNo.Enabled = false;
                            if (boolNone == false)
                                divYes.Style["display"] = "inline";
                        }
                        if (!IsPostBack)
                        {
                            if (boolProduction == true)
                            {
                                panHighProduction.Visible = true;
                                panStandardProduction.Visible = true;
                                panLowProduction.Visible = true;
                                panHighOSProduction.Visible = true;
                                panStandardOSProduction.Visible = true;
                                panLowOSProduction.Visible = true;
                            }
                            if (boolQA == true)
                            {
                                panHighQA.Visible = true;
                                panStandardQA.Visible = true;
                                panLowQA.Visible = true;
                                panHighOSQA.Visible = true;
                                panStandardOSQA.Visible = true;
                                panLowOSQA.Visible = true;
                            }
                            if ((boolProduction == false && boolQA == false) || (boolProduction == true && boolTest == true))
                            {
                                panHighTest.Visible = true;
                                panStandardTest.Visible = true;
                                panLowTest.Visible = true;
                                panHighOSTest.Visible = true;
                                panStandardOSTest.Visible = true;
                                panLowOSTest.Visible = true;
                            }
                            if (boolNoReplication == false)     // If Replication = true
                            {
                                panHighReplication.Visible = true;
                                ddlHighReplicated.Enabled = false;
                                ddlHighReplicated.SelectedValue = "Yes";
                                ddlHighReplicated.ToolTip = "Under 48 Hours requires Replication";
                                divHighReplicated.Style["display"] = "inline";
                                panStandardReplication.Visible = true;
                                ddlStandardReplicated.Enabled = false;
                                ddlStandardReplicated.SelectedValue = "Yes";
                                ddlStandardReplicated.ToolTip = "Under 48 Hours requires Replication";
                                divStandardReplicated.Style["display"] = "inline";
                                panLowReplication.Visible = true;
                                ddlLowReplicated.Enabled = false;
                                ddlLowReplicated.SelectedValue = "Yes";
                                ddlLowReplicated.ToolTip = "Under 48 Hours requires Replication";
                                divLowReplicated.Style["display"] = "inline";
                                panHighOSReplication.Visible = true;
                                ddlHighOSReplicated.Enabled = false;
                                ddlHighOSReplicated.SelectedValue = "Yes";
                                ddlHighOSReplicated.ToolTip = "Under 48 Hours requires Replication";
                                divHighOSReplicated.Style["display"] = "inline";
                                panStandardOSReplication.Visible = true;
                                ddlStandardOSReplicated.Enabled = false;
                                ddlStandardOSReplicated.SelectedValue = "Yes";
                                ddlStandardOSReplicated.ToolTip = "Under 48 Hours requires Replication";
                                divStandardOSReplicated.Style["display"] = "inline";
                                panLowOSReplication.Visible = true;
                                ddlLowOSReplicated.Enabled = false;
                                ddlLowOSReplicated.SelectedValue = "Yes";
                                ddlLowOSReplicated.ToolTip = "Under 48 Hours requires Replication";
                                divLowOSReplicated.Style["display"] = "inline";
                            }
                            if (ds.Tables[0].Rows[0]["storage"].ToString() == "1")
                            {
                                radYes.Checked = true;
                                // OS Volumes
                                ds = oForecast.GetStorageOS(intID);
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    divYes.Style["display"] = "inline";
                                    if (ds.Tables[0].Rows[0]["high"].ToString() == "1")
                                    {
                                        chkHighOS.Checked = true;
                                        divHighOS.Style["display"] = "inline";
                                        txtHighOSRequire.Text = ds.Tables[0].Rows[0]["high_total"].ToString();
                                        txtHighOSRequireQA.Text = ds.Tables[0].Rows[0]["high_qa"].ToString();
                                        txtHighOSRequireTest.Text = ds.Tables[0].Rows[0]["high_test"].ToString();
                                        double dblHighOSReplicated = double.Parse(ds.Tables[0].Rows[0]["high_replicated"].ToString());
                                        if (dblHighOSReplicated > 0.00)
                                        {
                                            divHighOSReplicated.Style["display"] = "inline";
                                            txtHighOSReplicated.Text = dblHighOSReplicated.ToString();
                                            ddlHighOSReplicated.SelectedValue = "Yes";
                                        }
                                        ddlHighOSLevel.SelectedValue = ds.Tables[0].Rows[0]["high_level"].ToString();
                                        double dblHighOSHA = double.Parse(ds.Tables[0].Rows[0]["high_ha"].ToString());
                                        if (dblHighOSHA > 0.00)
                                        {
                                            divHighOSLevel.Style["display"] = "inline";
                                            txtHighOSLevel.Text = dblHighOSHA.ToString();
                                        }
                                    }
                                    if (ds.Tables[0].Rows[0]["standard"].ToString() == "1")
                                    {
                                        chkStandardOS.Checked = true;
                                        divStandardOS.Style["display"] = "inline";
                                        txtStandardOSRequire.Text = ds.Tables[0].Rows[0]["standard_total"].ToString();
                                        txtStandardOSRequireQA.Text = ds.Tables[0].Rows[0]["standard_qa"].ToString();
                                        txtStandardOSRequireTest.Text = ds.Tables[0].Rows[0]["standard_test"].ToString();
                                        double dblStandardOSReplicated = double.Parse(ds.Tables[0].Rows[0]["standard_replicated"].ToString());
                                        if (dblStandardOSReplicated > 0.00)
                                        {
                                            divStandardOSReplicated.Style["display"] = "inline";
                                            txtStandardOSReplicated.Text = dblStandardOSReplicated.ToString();
                                            ddlStandardOSReplicated.SelectedValue = "Yes";
                                        }
                                        ddlStandardOSLevel.SelectedValue = ds.Tables[0].Rows[0]["standard_level"].ToString();
                                        double dblStandardOSHA = double.Parse(ds.Tables[0].Rows[0]["standard_ha"].ToString());
                                        if (dblStandardOSHA > 0.00)
                                        {
                                            divStandardOSLevel.Style["display"] = "inline";
                                            txtStandardOSLevel.Text = dblStandardOSHA.ToString();
                                        }
                                    }
                                    if (ds.Tables[0].Rows[0]["low"].ToString() == "1")
                                    {
                                        chkLowOS.Checked = true;
                                        divLowOS.Style["display"] = "inline";
                                        txtLowOSRequire.Text = ds.Tables[0].Rows[0]["low_total"].ToString();
                                        txtLowOSRequireQA.Text = ds.Tables[0].Rows[0]["low_qa"].ToString();
                                        txtLowOSRequireTest.Text = ds.Tables[0].Rows[0]["low_test"].ToString();
                                        double dblLowOSReplicated = double.Parse(ds.Tables[0].Rows[0]["low_replicated"].ToString());
                                        if (dblLowOSReplicated > 0.00)
                                        {
                                            divLowOSReplicated.Style["display"] = "inline";
                                            txtLowOSReplicated.Text = dblLowOSReplicated.ToString();
                                            ddlLowOSReplicated.SelectedValue = "Yes";
                                        }
                                        ddlLowOSLevel.SelectedValue = ds.Tables[0].Rows[0]["low_level"].ToString();
                                        double dblLowOSHA = double.Parse(ds.Tables[0].Rows[0]["low_ha"].ToString());
                                        if (dblLowOSHA > 0.00)
                                        {
                                            divLowOSLevel.Style["display"] = "inline";
                                            txtLowOSLevel.Text = dblLowOSHA.ToString();
                                        }
                                    }
                                }
                                // Application / Data Volumes
                                ds = oForecast.GetStorage(intID);
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    divYes.Style["display"] = "inline";
                                    if (ds.Tables[0].Rows[0]["high"].ToString() == "1")
                                    {
                                        chkHigh.Checked = true;
                                        divHigh.Style["display"] = "inline";
                                        txtHighRequire.Text = ds.Tables[0].Rows[0]["high_total"].ToString();
                                        txtHighRequireQA.Text = ds.Tables[0].Rows[0]["high_qa"].ToString();
                                        txtHighRequireTest.Text = ds.Tables[0].Rows[0]["high_test"].ToString();
                                        double dblHighReplicated = double.Parse(ds.Tables[0].Rows[0]["high_replicated"].ToString());
                                        if (dblHighReplicated > 0.00)
                                        {
                                            divHighReplicated.Style["display"] = "inline";
                                            txtHighReplicated.Text = dblHighReplicated.ToString();
                                            ddlHighReplicated.SelectedValue = "Yes";
                                        }
                                        ddlHighLevel.SelectedValue = ds.Tables[0].Rows[0]["high_level"].ToString();
                                        double dblHighHA = double.Parse(ds.Tables[0].Rows[0]["high_ha"].ToString());
                                        if (dblHighHA > 0.00)
                                        {
                                            divHighLevel.Style["display"] = "inline";
                                            txtHighLevel.Text = dblHighHA.ToString();
                                        }
                                    }
                                    if (ds.Tables[0].Rows[0]["standard"].ToString() == "1")
                                    {
                                        chkStandard.Checked = true;
                                        divStandard.Style["display"] = "inline";
                                        txtStandardRequire.Text = ds.Tables[0].Rows[0]["standard_total"].ToString();
                                        txtStandardRequireQA.Text = ds.Tables[0].Rows[0]["standard_qa"].ToString();
                                        txtStandardRequireTest.Text = ds.Tables[0].Rows[0]["standard_test"].ToString();
                                        double dblStandardReplicated = double.Parse(ds.Tables[0].Rows[0]["standard_replicated"].ToString());
                                        if (dblStandardReplicated > 0.00)
                                        {
                                            divStandardReplicated.Style["display"] = "inline";
                                            txtStandardReplicated.Text = dblStandardReplicated.ToString();
                                            ddlStandardReplicated.SelectedValue = "Yes";
                                        }
                                        ddlStandardLevel.SelectedValue = ds.Tables[0].Rows[0]["standard_level"].ToString();
                                        double dblStandardHA = double.Parse(ds.Tables[0].Rows[0]["standard_ha"].ToString());
                                        if (dblStandardHA > 0.00)
                                        {
                                            divStandardLevel.Style["display"] = "inline";
                                            txtStandardLevel.Text = dblStandardHA.ToString();
                                        }
                                    }
                                    if (ds.Tables[0].Rows[0]["low"].ToString() == "1")
                                    {
                                        chkLow.Checked = true;
                                        divLow.Style["display"] = "inline";
                                        txtLowRequire.Text = ds.Tables[0].Rows[0]["low_total"].ToString();
                                        txtLowRequireQA.Text = ds.Tables[0].Rows[0]["low_qa"].ToString();
                                        txtLowRequireTest.Text = ds.Tables[0].Rows[0]["low_test"].ToString();
                                        double dblLowReplicated = double.Parse(ds.Tables[0].Rows[0]["low_replicated"].ToString());
                                        if (dblLowReplicated > 0.00)
                                        {
                                            divLowReplicated.Style["display"] = "inline";
                                            txtLowReplicated.Text = dblLowReplicated.ToString();
                                            ddlLowReplicated.SelectedValue = "Yes";
                                        }
                                        ddlLowLevel.SelectedValue = ds.Tables[0].Rows[0]["low_level"].ToString();
                                        double dblLowHA = double.Parse(ds.Tables[0].Rows[0]["low_ha"].ToString());
                                        if (dblLowHA > 0.00)
                                        {
                                            divLowLevel.Style["display"] = "inline";
                                            txtLowLevel.Text = dblLowHA.ToString();
                                        }
                                    }
                                }
                            }
                            else if (ds.Tables[0].Rows[0]["storage"].ToString() == "-1")
                                radLater.Checked = true;
                            else if (boolRequired == true && boolNone == false)
                            {
                                radYes.Checked = true;
                                if (oModelsProperties.IsStorageDB_BootSANWindows(intModel) == true)
                                {
                                    chkStandardOS.Checked = true;
                                    divStandardOS.Style["display"] = "inline";
                                    txtStandardOSRequire.Text = intStorageOS.ToString();
                                    txtStandardOSRequireQA.Text = intStorageOS.ToString();
                                    txtStandardOSRequireTest.Text = intStorageOS.ToString();
                                    chkStandard.Checked = true;
                                    divStandard.Style["display"] = "inline";
                                    txtStandardRequire.Text = intMinimumApp.ToString();
                                    txtStandardRequireQA.Text = intMinimumApp.ToString();
                                    txtStandardRequireTest.Text = intMinimumApp.ToString();
                                }
                            }
                            else
                                radNo.Checked = true;
                            if (boolHA == false)
                            {
                                ddlHighLevel.SelectedValue = "Standard";
                                ddlHighLevel.Enabled = false;
                                ddlStandardLevel.SelectedValue = "Standard";
                                ddlStandardLevel.Enabled = false;
                                ddlLowLevel.SelectedValue = "Standard";
                                ddlLowLevel.Enabled = false;
                                ddlHighOSLevel.SelectedValue = "Standard";
                                ddlHighOSLevel.Enabled = false;
                                ddlStandardOSLevel.SelectedValue = "Standard";
                                ddlStandardOSLevel.Enabled = false;
                                ddlLowOSLevel.SelectedValue = "Standard";
                                ddlLowOSLevel.Enabled = false;
                            }
                        }
                        chkHighOS.Attributes.Add("onclick", "ShowHideDivCheck('" + divHighOS.ClientID + "',this);");
                        ddlHighOSReplicated.Attributes.Add("onchange", "ShowHideDivDDL('" + divHighOSReplicated.ClientID + "',this,'Yes');");
                        ddlHighOSLevel.Attributes.Add("onchange", "ShowHideDivDDL('" + divHighOSLevel.ClientID + "',this,'High');");
                        chkStandardOS.Attributes.Add("onclick", "ShowHideDivCheck('" + divStandardOS.ClientID + "',this);");
                        ddlStandardOSReplicated.Attributes.Add("onchange", "ShowHideDivDDL('" + divStandardOSReplicated.ClientID + "',this,'Yes');");
                        ddlStandardOSLevel.Attributes.Add("onchange", "ShowHideDivDDL('" + divStandardOSLevel.ClientID + "',this,'High');");
                        chkLowOS.Attributes.Add("onclick", "ShowHideDivCheck('" + divLowOS.ClientID + "',this);");
                        ddlLowOSReplicated.Attributes.Add("onchange", "ShowHideDivDDL('" + divLowOSReplicated.ClientID + "',this,'Yes');");
                        ddlLowOSLevel.Attributes.Add("onchange", "ShowHideDivDDL('" + divLowOSLevel.ClientID + "',this,'High');");
                        imgHighOSRequireUp.Attributes.Add("onclick", "return StorageSpinnerUp('" + txtHighOSRequire.ClientID + "'," + intStoragePerBladeOs + ");");
                        imgHighOSRequireDn.Attributes.Add("onclick", "return StorageSpinnerDn('" + txtHighOSRequire.ClientID + "'," + intStoragePerBladeOs + ");");
                        imgHighOSRequireQAUp.Attributes.Add("onclick", "return StorageSpinnerUp('" + txtHighOSRequireQA.ClientID + "'," + intStoragePerBladeOs + ");");
                        imgHighOSRequireQADn.Attributes.Add("onclick", "return StorageSpinnerDn('" + txtHighOSRequireQA.ClientID + "'," + intStoragePerBladeOs + ");");
                        imgHighOSRequireTestUp.Attributes.Add("onclick", "return StorageSpinnerUp('" + txtHighOSRequireTest.ClientID + "'," + intStoragePerBladeOs + ");");
                        imgHighOSRequireTestDn.Attributes.Add("onclick", "return StorageSpinnerDn('" + txtHighOSRequireTest.ClientID + "'," + intStoragePerBladeOs + ");");
                        imgStandardOSRequireUp.Attributes.Add("onclick", "return StorageSpinnerUp('" + txtStandardOSRequire.ClientID + "'," + intStoragePerBladeOs + ");");
                        imgStandardOSRequireDn.Attributes.Add("onclick", "return StorageSpinnerDn('" + txtStandardOSRequire.ClientID + "'," + intStoragePerBladeOs + ");");
                        imgStandardOSRequireQAUp.Attributes.Add("onclick", "return StorageSpinnerUp('" + txtStandardOSRequireQA.ClientID + "'," + intStoragePerBladeOs + ");");
                        imgStandardOSRequireQADn.Attributes.Add("onclick", "return StorageSpinnerDn('" + txtStandardOSRequireQA.ClientID + "'," + intStoragePerBladeOs + ");");
                        imgStandardOSRequireTestUp.Attributes.Add("onclick", "return StorageSpinnerUp('" + txtStandardOSRequireTest.ClientID + "'," + intStoragePerBladeOs + ");");
                        imgStandardOSRequireTestDn.Attributes.Add("onclick", "return StorageSpinnerDn('" + txtStandardOSRequireTest.ClientID + "'," + intStoragePerBladeOs + ");");
                        imgLowOSRequireUp.Attributes.Add("onclick", "return StorageSpinnerUp('" + txtLowOSRequire.ClientID + "'," + intStoragePerBladeOs + ");");
                        imgLowOSRequireDn.Attributes.Add("onclick", "return StorageSpinnerDn('" + txtLowOSRequire.ClientID + "'," + intStoragePerBladeOs + ");");
                        imgLowOSRequireQAUp.Attributes.Add("onclick", "return StorageSpinnerUp('" + txtLowOSRequireQA.ClientID + "'," + intStoragePerBladeOs + ");");
                        imgLowOSRequireQADn.Attributes.Add("onclick", "return StorageSpinnerDn('" + txtLowOSRequireQA.ClientID + "'," + intStoragePerBladeOs + ");");
                        imgLowOSRequireTestUp.Attributes.Add("onclick", "return StorageSpinnerUp('" + txtLowOSRequireTest.ClientID + "'," + intStoragePerBladeOs + ");");
                        imgLowOSRequireTestDn.Attributes.Add("onclick", "return StorageSpinnerDn('" + txtLowOSRequireTest.ClientID + "'," + intStoragePerBladeOs + ");");
                        txtHighOSRequire.Attributes.Add("readonly", "readonly");
                        txtHighOSRequireTest.Attributes.Add("readonly", "readonly");
                        txtStandardOSRequire.Attributes.Add("readonly", "readonly");
                        txtStandardOSRequireTest.Attributes.Add("readonly", "readonly");
                        txtLowOSRequire.Attributes.Add("readonly", "readonly");
                        txtLowOSRequireTest.Attributes.Add("readonly", "readonly");
                        chkHigh.Attributes.Add("onclick", "ShowHideDivCheck('" + divHigh.ClientID + "',this);");
                        ddlHighReplicated.Attributes.Add("onchange", "ShowHideDivDDL('" + divHighReplicated.ClientID + "',this,'Yes');");
                        ddlHighLevel.Attributes.Add("onchange", "ShowHideDivDDL('" + divHighLevel.ClientID + "',this,'High');");
                        chkStandard.Attributes.Add("onclick", "ShowHideDivCheck('" + divStandard.ClientID + "',this);");
                        ddlStandardReplicated.Attributes.Add("onchange", "ShowHideDivDDL('" + divStandardReplicated.ClientID + "',this,'Yes');");
                        ddlStandardLevel.Attributes.Add("onchange", "ShowHideDivDDL('" + divStandardLevel.ClientID + "',this,'High');");
                        chkLow.Attributes.Add("onclick", "ShowHideDivCheck('" + divLow.ClientID + "',this);");
                        ddlLowReplicated.Attributes.Add("onchange", "ShowHideDivDDL('" + divLowReplicated.ClientID + "',this,'Yes');");
                        ddlLowLevel.Attributes.Add("onchange", "ShowHideDivDDL('" + divLowLevel.ClientID + "',this,'High');");
                        string strOSLabel = "**** OPERATING SYSTEM VOLUMES ****";
                        string strAppLabel = "**** APPLICATION / DATA VOLUMES ****";
                        btnNext.Attributes.Add("onclick", "return ResetStorage()" +
                            " && EnsureStorage('" + radYes.ClientID + "','" + chkHighOS.ClientID + "','" + chkStandardOS.ClientID + "','" + chkLowOS.ClientID + "','" + strOSLabel + "')" +
                            " && EnsureStorageNewOS('" + radYes.ClientID + "','" + chkHighOS.ClientID + "','" + txtHighOSRequire.ClientID + "','" + txtHighOSRequireQA.ClientID + "','" + txtHighOSRequireTest.ClientID + "'," + (boolProduction ? "true" : "false") + "," + (boolNoReplication ? "true" : "false") + ",'" + ddlHighOSReplicated.ClientID + "','" + txtHighOSReplicated.ClientID + "'," + (boolHA ? "true" : "false") + ",'" + ddlHighOSLevel.ClientID + "','" + txtHighOSLevel.ClientID + "','" + strOSLabel + "')" +
                            " && EnsureStorageNewOS('" + radYes.ClientID + "','" + chkStandardOS.ClientID + "','" + txtStandardOSRequire.ClientID + "','" + txtStandardOSRequireQA.ClientID + "','" + txtStandardOSRequireTest.ClientID + "'," + (boolProduction ? "true" : "false") + "," + (boolNoReplication ? "true" : "false") + ",'" + ddlStandardOSReplicated.ClientID + "','" + txtStandardOSReplicated.ClientID + "'," + (boolHA ? "true" : "false") + ",'" + ddlStandardOSLevel.ClientID + "','" + txtStandardOSLevel.ClientID + "','" + strOSLabel + "')" +
                            " && EnsureStorageNewOS('" + radYes.ClientID + "','" + chkLowOS.ClientID + "','" + txtLowOSRequire.ClientID + "','" + txtLowOSRequireQA.ClientID + "','" + txtLowOSRequireTest.ClientID + "'," + (boolProduction ? "true" : "false") + "," + (boolNoReplication ? "true" : "false") + ",'" + ddlLowOSReplicated.ClientID + "','" + txtLowOSReplicated.ClientID + "'," + (boolHA ? "true" : "false") + ",'" + ddlLowOSLevel.ClientID + "','" + txtLowOSLevel.ClientID + "','" + strOSLabel + "')" +
                            " && EnsureStorageOS(" + (boolProduction ? intStorageOS.ToString() : "0") + "," + (boolQA ? intStorageOS.ToString() : "0") + "," + ((boolProduction == false && boolQA == false) || (boolProduction == true && boolTest == true) ? intStorageOS.ToString() : "0") + ",'" + strOSLabel + "')" +
                            " && EnsureStorage('" + radYes.ClientID + "','" + chkHigh.ClientID + "','" + chkStandard.ClientID + "','" + chkLow.ClientID + "','" + strAppLabel + "')" +
                            " && EnsureStorageNew('" + radYes.ClientID + "','" + chkHigh.ClientID + "','" + txtHighRequire.ClientID + "','" + txtHighRequireQA.ClientID + "','" + txtHighRequireTest.ClientID + "'," + (boolProduction ? "true" : "false") + "," + (boolNoReplication ? "true" : "false") + ",'" + ddlHighReplicated.ClientID + "','" + txtHighReplicated.ClientID + "'," + (boolHA ? "true" : "false") + ",'" + ddlHighLevel.ClientID + "','" + txtHighLevel.ClientID + "','" + strAppLabel + "')" +
                            " && EnsureStorageNew('" + radYes.ClientID + "','" + chkStandard.ClientID + "','" + txtStandardRequire.ClientID + "','" + txtStandardRequireQA.ClientID + "','" + txtStandardRequireTest.ClientID + "'," + (boolProduction ? "true" : "false") + "," + (boolNoReplication ? "true" : "false") + ",'" + ddlStandardReplicated.ClientID + "','" + txtStandardReplicated.ClientID + "'," + (boolHA ? "true" : "false") + ",'" + ddlStandardLevel.ClientID + "','" + txtStandardLevel.ClientID + "','" + strAppLabel + "')" +
                            " && EnsureStorageNew('" + radYes.ClientID + "','" + chkLow.ClientID + "','" + txtLowRequire.ClientID + "','" + txtLowRequireQA.ClientID + "','" + txtLowRequireTest.ClientID + "'," + (boolProduction ? "true" : "false") + "," + (boolNoReplication ? "true" : "false") + ",'" + ddlLowReplicated.ClientID + "','" + txtLowReplicated.ClientID + "'," + (boolHA ? "true" : "false") + ",'" + ddlLowLevel.ClientID + "','" + txtLowLevel.ClientID + "','" + strAppLabel + "')" +
                            " && EnsureStorageMinimum(" + (boolProduction ? intMinimumApp.ToString() : "0") + "," + (boolQA ? intMinimumApp.ToString() : "0") + "," + ((boolProduction == false && boolQA == false) || (boolProduction == true && boolTest == true) ? intMinimumApp.ToString() : "0") + ",'" + strAppLabel + "')" +
                            ";");
                        btnUpdate.Attributes.Add("onclick", "return ResetStorage()" +
                            " && EnsureStorage('" + radYes.ClientID + "','" + chkHighOS.ClientID + "','" + chkStandardOS.ClientID + "','" + chkLowOS.ClientID + "','" + strOSLabel + "')" +
                            " && EnsureStorageNewOS('" + radYes.ClientID + "','" + chkHighOS.ClientID + "','" + txtHighOSRequire.ClientID + "','" + txtHighOSRequireQA.ClientID + "','" + txtHighOSRequireTest.ClientID + "'," + (boolProduction ? "true" : "false") + "," + (boolNoReplication ? "true" : "false") + ",'" + ddlHighOSReplicated.ClientID + "','" + txtHighOSReplicated.ClientID + "'," + (boolHA ? "true" : "false") + ",'" + ddlHighOSLevel.ClientID + "','" + txtHighOSLevel.ClientID + "','" + strOSLabel + "')" +
                            " && EnsureStorageNewOS('" + radYes.ClientID + "','" + chkStandardOS.ClientID + "','" + txtStandardOSRequire.ClientID + "','" + txtStandardOSRequireQA.ClientID + "','" + txtStandardOSRequireTest.ClientID + "'," + (boolProduction ? "true" : "false") + "," + (boolNoReplication ? "true" : "false") + ",'" + ddlStandardOSReplicated.ClientID + "','" + txtStandardOSReplicated.ClientID + "'," + (boolHA ? "true" : "false") + ",'" + ddlStandardOSLevel.ClientID + "','" + txtStandardOSLevel.ClientID + "','" + strOSLabel + "')" +
                            " && EnsureStorageNewOS('" + radYes.ClientID + "','" + chkLowOS.ClientID + "','" + txtLowOSRequire.ClientID + "','" + txtLowOSRequireQA.ClientID + "','" + txtLowOSRequireTest.ClientID + "'," + (boolProduction ? "true" : "false") + "," + (boolNoReplication ? "true" : "false") + ",'" + ddlLowOSReplicated.ClientID + "','" + txtLowOSReplicated.ClientID + "'," + (boolHA ? "true" : "false") + ",'" + ddlLowOSLevel.ClientID + "','" + txtLowOSLevel.ClientID + "','" + strOSLabel + "')" +
                            " && EnsureStorageOS(" + (boolProduction ? intStorageOS.ToString() : "0") + "," + (boolQA ? intStorageOS.ToString() : "0") + "," + ((boolProduction == false && boolQA == false) || (boolProduction == true && boolTest == true) ? intStorageOS.ToString() : "0") + ",'" + strOSLabel + "')" +
                            " && EnsureStorage('" + radYes.ClientID + "','" + chkHigh.ClientID + "','" + chkStandard.ClientID + "','" + chkLow.ClientID + "','" + strAppLabel + "')" +
                            " && EnsureStorageNew('" + radYes.ClientID + "','" + chkHigh.ClientID + "','" + txtHighRequire.ClientID + "','" + txtHighRequireQA.ClientID + "','" + txtHighRequireTest.ClientID + "'," + (boolProduction ? "true" : "false") + "," + (boolNoReplication ? "true" : "false") + ",'" + ddlHighReplicated.ClientID + "','" + txtHighReplicated.ClientID + "'," + (boolHA ? "true" : "false") + ",'" + ddlHighLevel.ClientID + "','" + txtHighLevel.ClientID + "','" + strAppLabel + "')" +
                            " && EnsureStorageNew('" + radYes.ClientID + "','" + chkStandard.ClientID + "','" + txtStandardRequire.ClientID + "','" + txtStandardRequireQA.ClientID + "','" + txtStandardRequireTest.ClientID + "'," + (boolProduction ? "true" : "false") + "," + (boolNoReplication ? "true" : "false") + ",'" + ddlStandardReplicated.ClientID + "','" + txtStandardReplicated.ClientID + "'," + (boolHA ? "true" : "false") + ",'" + ddlStandardLevel.ClientID + "','" + txtStandardLevel.ClientID + "','" + strAppLabel + "')" +
                            " && EnsureStorageNew('" + radYes.ClientID + "','" + chkLow.ClientID + "','" + txtLowRequire.ClientID + "','" + txtLowRequireQA.ClientID + "','" + txtLowRequireTest.ClientID + "'," + (boolProduction ? "true" : "false") + "," + (boolNoReplication ? "true" : "false") + ",'" + ddlLowReplicated.ClientID + "','" + txtLowReplicated.ClientID + "'," + (boolHA ? "true" : "false") + ",'" + ddlLowLevel.ClientID + "','" + txtLowLevel.ClientID + "','" + strAppLabel + "')" +
                            " && EnsureStorageMinimum(" + (boolProduction ? intMinimumApp.ToString() : "0") + "," + (boolQA ? intMinimumApp.ToString() : "0") + "," + ((boolProduction == false && boolQA == false) || (boolProduction == true && boolTest == true) ? intMinimumApp.ToString() : "0") + ",'" + strAppLabel + "')" +
                            ";");
                    }
                }
                else
                {
                    divYes.Style["display"] = "inline";
                    chkStandardOS.Checked = true;
                    divStandardOS.Style["display"] = "inline";
                    if (boolProduction == true)
                        txtStandardOSRequire.Text = intStorageOS.ToString();
                    if (boolQA == true)
                        txtStandardOSRequireQA.Text = intStorageOS.ToString();
                    if ((boolProduction == false && boolQA == false) || (boolProduction == true && boolTest == true))
                        txtStandardOSRequireTest.Text = intStorageOS.ToString();
                }
            }
            radLater.Attributes.Add("onclick", "ShowHideDiv('" + divYes.ClientID + "','none');");
            radNo.Attributes.Add("onclick", "ShowHideDiv('" + divYes.ClientID + "','none');");
            radYes.Attributes.Add("onclick", "ShowHideDiv('" + divYes.ClientID + "','inline');");
            btnClose.Attributes.Add("onclick", "return window.close();");
        }
        protected int Save()
        {
            oForecast.DeleteStorageOS(intID);
            oForecast.DeleteStorage(intID);
            if (radYes.Checked == true)
            {
                // OS Volumes
                int intHighOS = (chkHighOS.Checked ? 1 : 0);
                double dblHighOSTotal = 0.00;
                double dblHighOSQA = 0.00;
                double dblHighOSTest = 0.00;
                double dblHighOSReplicated = 0.00;
                string strHighOSLevel = "";
                double dblHighOSLevel = 0.00;
                if (intHighOS == 1)
                {
                    if (panHighOSProduction.Visible == true)
                    {
                        dblHighOSTotal = double.Parse(txtHighOSRequire.Text);
                        if (panHighOSReplication.Visible == true && ddlHighOSReplicated.SelectedIndex == (ddlHighOSReplicated.Items.Count - 1))
                            dblHighOSReplicated = double.Parse(txtHighOSReplicated.Text);
                        strHighOSLevel = ddlHighOSLevel.SelectedItem.Value;
                        if (ddlHighOSLevel.SelectedIndex == (ddlHighOSLevel.Items.Count - 1))
                            dblHighOSLevel = double.Parse(txtHighOSLevel.Text);
                    }
                    if (panHighOSQA.Visible == true)
                        dblHighOSQA = double.Parse(txtHighOSRequireQA.Text);
                    if (panHighOSTest.Visible == true)
                        dblHighOSTest = double.Parse(txtHighOSRequireTest.Text);
                }
                int intStandardOS = (chkStandardOS.Checked ? 1 : 0);
                double dblStandardOSTotal = 0.00;
                double dblStandardOSQA = 0.00;
                double dblStandardOSTest = 0.00;
                double dblStandardOSReplicated = 0.00;
                string strStandardOSLevel = "";
                double dblStandardOSLevel = 0.00;
                if (intStandardOS == 1)
                {
                    if (panStandardOSProduction.Visible == true)
                    {
                        dblStandardOSTotal = double.Parse(txtStandardOSRequire.Text);
                        if (panStandardOSReplication.Visible == true && ddlStandardOSReplicated.SelectedIndex == (ddlStandardOSReplicated.Items.Count - 1))
                            dblStandardOSReplicated = double.Parse(txtStandardOSReplicated.Text);
                        strStandardOSLevel = ddlStandardOSLevel.SelectedItem.Value;
                        if (ddlStandardOSLevel.SelectedIndex == (ddlStandardOSLevel.Items.Count - 1))
                            dblStandardOSLevel = double.Parse(txtStandardOSLevel.Text);
                    }
                    if (panStandardOSQA.Visible == true)
                        dblStandardOSQA = double.Parse(txtStandardOSRequireQA.Text);
                    if (panStandardOSTest.Visible == true)
                        dblStandardOSTest = double.Parse(txtStandardOSRequireTest.Text);
                }
                int intLowOS = (chkLowOS.Checked ? 1 : 0);
                double dblLowOSTotal = 0.00;
                double dblLowOSQA = 0.00;
                double dblLowOSTest = 0.00;
                double dblLowOSReplicated = 0.00;
                string strLowOSLevel = "";
                double dblLowOSLevel = 0.00;
                if (intLowOS == 1)
                {
                    if (panLowOSProduction.Visible == true)
                    {
                        dblLowOSTotal = double.Parse(txtLowOSRequire.Text);
                        if (panLowOSReplication.Visible == true && ddlLowOSReplicated.SelectedIndex == (ddlLowOSReplicated.Items.Count - 1))
                            dblLowOSReplicated = double.Parse(txtLowOSReplicated.Text);
                        strLowOSLevel = ddlLowOSLevel.SelectedItem.Value;
                        if (ddlLowOSLevel.SelectedIndex == (ddlLowOSLevel.Items.Count - 1))
                            dblLowOSLevel = double.Parse(txtLowOSLevel.Text);
                    }
                    if (panLowOSQA.Visible == true)
                        dblLowOSQA = double.Parse(txtLowOSRequireQA.Text);
                    if (panLowOSTest.Visible == true)
                        dblLowOSTest = double.Parse(txtLowOSRequireTest.Text);
                }
                oForecast.AddStorageOS(intID, intHighOS, dblHighOSTotal, dblHighOSQA, dblHighOSTest, dblHighOSReplicated, strHighOSLevel, dblHighOSLevel, intStandardOS, dblStandardOSTotal, dblStandardOSQA, dblStandardOSTest, dblStandardOSReplicated, strStandardOSLevel, dblStandardOSLevel, intLowOS, dblLowOSTotal, dblLowOSQA, dblLowOSTest, dblLowOSReplicated, strLowOSLevel, dblLowOSLevel);
                // Application / Data Volumes
                int intHigh = (chkHigh.Checked ? 1 : 0);
                double dblHighTotal = 0.00;
                double dblHighQA = 0.00;
                double dblHighTest = 0.00;
                double dblHighReplicated = 0.00;
                string strHighLevel = "";
                double dblHighLevel = 0.00;
                if (intHigh == 1)
                {
                    if (panHighProduction.Visible == true)
                    {
                        dblHighTotal = double.Parse(txtHighRequire.Text);
                        if (panHighReplication.Visible == true && ddlHighReplicated.SelectedIndex == (ddlHighReplicated.Items.Count - 1))
                            dblHighReplicated = double.Parse(txtHighReplicated.Text);
                        strHighLevel = ddlHighLevel.SelectedItem.Value;
                        if (ddlHighLevel.SelectedIndex == (ddlHighLevel.Items.Count - 1))
                            dblHighLevel = double.Parse(txtHighLevel.Text);
                    }
                    if (panHighQA.Visible == true)
                        dblHighQA = double.Parse(txtHighRequireQA.Text);
                    if (panHighTest.Visible == true)
                        dblHighTest = double.Parse(txtHighRequireTest.Text);
                }
                int intStandard = (chkStandard.Checked ? 1 : 0);
                double dblStandardTotal = 0.00;
                double dblStandardQA = 0.00;
                double dblStandardTest = 0.00;
                double dblStandardReplicated = 0.00;
                string strStandardLevel = "";
                double dblStandardLevel = 0.00;
                if (intStandard == 1)
                {
                    if (panStandardProduction.Visible == true)
                    {
                        dblStandardTotal = double.Parse(txtStandardRequire.Text);
                        if (panStandardReplication.Visible == true && ddlStandardReplicated.SelectedIndex == (ddlStandardReplicated.Items.Count - 1))
                            dblStandardReplicated = double.Parse(txtStandardReplicated.Text);
                        strStandardLevel = ddlStandardLevel.SelectedItem.Value;
                        if (ddlStandardLevel.SelectedIndex == (ddlStandardLevel.Items.Count - 1))
                            dblStandardLevel = double.Parse(txtStandardLevel.Text);
                    }
                    if (panStandardQA.Visible == true)
                        dblStandardQA = double.Parse(txtStandardRequireQA.Text);
                    if (panStandardTest.Visible == true)
                        dblStandardTest = double.Parse(txtStandardRequireTest.Text);
                }
                int intLow = (chkLow.Checked ? 1 : 0);
                double dblLowTotal = 0.00;
                double dblLowQA = 0.00;
                double dblLowTest = 0.00;
                double dblLowReplicated = 0.00;
                string strLowLevel = "";
                double dblLowLevel = 0.00;
                if (intLow == 1)
                {
                    if (panLowProduction.Visible == true)
                    {
                        dblLowTotal = double.Parse(txtLowRequire.Text);
                        if (panLowReplication.Visible == true && ddlLowReplicated.SelectedIndex == (ddlLowReplicated.Items.Count - 1))
                            dblLowReplicated = double.Parse(txtLowReplicated.Text);
                        strLowLevel = ddlLowLevel.SelectedItem.Value;
                        if (ddlLowLevel.SelectedIndex == (ddlLowLevel.Items.Count - 1))
                            dblLowLevel = double.Parse(txtLowLevel.Text);
                    }
                    if (panLowQA.Visible == true)
                        dblLowQA = double.Parse(txtLowRequireQA.Text);
                    if (panLowTest.Visible == true)
                        dblLowTest = double.Parse(txtLowRequireTest.Text);
                }
                oForecast.AddStorage(intID, intHigh, dblHighTotal, dblHighQA, dblHighTest, dblHighReplicated, strHighLevel, dblHighLevel, intStandard, dblStandardTotal, dblStandardQA, dblStandardTest, dblStandardReplicated, strStandardLevel, dblStandardLevel, intLow, dblLowTotal, dblLowQA, dblLowTest, dblLowReplicated, strLowLevel, dblLowLevel);
            }
            int intStorage = 0;
            if (radLater.Checked == true)
                intStorage = -1;
            else if (radYes.Checked == true)
                intStorage = 1;
            oForecast.UpdateStorage(intID, intStorage);
            return (radNo.Checked == true || radYes.Checked == true ? 1 : 0);
        }
        protected void btnBack_Click(Object Sender, EventArgs e)
        {
            oForecast.UpdateAnswerStep(intID, -1, -1);
            Response.Redirect(Request.Path + "?id=" + intID.ToString());
        }
        protected void btnNext_Click(Object Sender, EventArgs e)
        {
            int intDone = 0;
            //if (radNo.Checked == true)
            //    intDone = 1;
            //else
            intDone = Save();
            oForecast.UpdateAnswerStep(intID, 1, intDone);
            Response.Redirect(Request.Path + "?id=" + intID.ToString() + "&save=true");
        }
        protected void btnUpdate_Click(Object Sender, EventArgs e)
        {
            int intDone = 0;
            if (radNo.Checked == true)
                intDone = 1;
            else
                intDone = Save();
            int intStep = Int32.Parse(Request.QueryString["step"]);
            string strAlert = oForecast.AddStepDone(intID, intStep, intDone);
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "save", "<script type=\"text/javascript\">" + strAlert + "window.navigate('" + Request.Path + "?id=" + intID.ToString() + "&save=true');<" + "/" + "script>");
        }
        protected void btnCancel_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path + "?id=" + intID.ToString());
        }
    }
}