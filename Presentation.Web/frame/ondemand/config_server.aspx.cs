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
    public partial class config_server : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);

        protected bool boolUseCSM = (ConfigurationManager.AppSettings["USE_CSM_EXECUTION"] == "1");
        protected int intOSQuestion = Int32.Parse(ConfigurationManager.AppSettings["ForecastOSQuestionID"]);
        protected int intSoftwarePageID = Int32.Parse(ConfigurationManager.AppSettings["SOFTWARE_COMPONENT_PAGEID"]);
        protected int intProfile;
        protected OnDemand oOnDemand;
        protected Forecast oForecast;
        protected Classes oClass;
        protected CSMConfig oCSMConfig;
        protected Requests oRequest;
        protected Servers oServer;
        protected Storage oStorage;
        protected Models oModel;
        protected ModelsProperties oModelsProperties;
        protected VMWare oVMWare;
        protected ConsistencyGroups oConsistencyGroup;
        protected ServerName oServerName;
        protected Users oUser;
        protected OperatingSystems oOperatingSystem;
        protected int intAnswer = 0;
        protected int intCluster = 0;
        protected int intConfig = 0;
        protected int intNumber = 0;
        protected int intRequest = 0;
        protected int intOS = 0;
        protected string strSQL = "";
        protected string strHidden = "";
        protected string strMenuTab1 = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oOnDemand = new OnDemand(intProfile, dsn);
            oForecast = new Forecast(intProfile, dsn);
            oClass = new Classes(intProfile, dsn);
            oCSMConfig = new CSMConfig(intProfile, dsn);
            oRequest = new Requests(intProfile, dsn);
            oServer = new Servers(intProfile, dsn);
            oStorage = new Storage(intProfile, dsn);
            oModel = new Models(intProfile, dsn);
            oModelsProperties = new ModelsProperties(intProfile, dsn);
            oVMWare = new VMWare(intProfile, dsn);
            oConsistencyGroup = new ConsistencyGroups(intProfile, dsn);
            oServerName = new ServerName(intProfile, dsn);
            oUser = new Users(intProfile, dsn);
            oOperatingSystem = new OperatingSystems(intProfile, dsn);
            if (Request.QueryString["aid"] != null && Request.QueryString["aid"] != "")
                intAnswer = Int32.Parse(Request.QueryString["aid"]);
            if (Request.QueryString["clusterid"] != null && Request.QueryString["clusterid"] != "")
                intCluster = Int32.Parse(Request.QueryString["clusterid"]);
            if (Request.QueryString["csmid"] != null && Request.QueryString["csmid"] != "")
                intConfig = Int32.Parse(Request.QueryString["csmid"]);
            if (Request.QueryString["num"] != null && Request.QueryString["num"] != "")
                intNumber = Int32.Parse(Request.QueryString["num"]);
            int intMenuTab = 0;
            if (Request.QueryString["menu_tab"] != null && Request.QueryString["menu_tab"] != "")
                intMenuTab = Int32.Parse(Request.QueryString["menu_tab"]);
            if (Request.QueryString["refresh"] != null)
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "refresh", "<script type=\"text/javascript\">RefreshOpeningWindow();<" + "/" + "script>");
            if (Request.QueryString["save"] != null)
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "save", "<script type=\"text/javascript\">RefreshOpeningWindow();window.close();<" + "/" + "script>");
            else if (Request.QueryString["required"] != null)
            {
                if (Request.QueryString["dba"] != null)
                {
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "dba", "<script type=\"text/javascript\">RefreshOpeningWindow();alert('Please enter a database administrator');<" + "/" + "script>");
                    intMenuTab = 1;
                }
            }
            int intServer = 0;
            int intDR = 0;
            if (intAnswer > 0)
            {
                Page.Title = "ClearView Configure Device | Design # " + intAnswer.ToString();
                DataSet ds = oForecast.GetAnswer(intAnswer);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Tab oTab = new Tab("", intMenuTab, "divMenu1", true, false);
                    oTab.AddTab("1.) General Information", "");
                    oTab.AddTab("2.) Software Components", "");
                    strMenuTab1 = oTab.GetTabs();

                    int intModel = oForecast.GetModel(intAnswer);
                    int intType = oModelsProperties.GetType(intModel);
                    int intDRForecast = Int32.Parse(oForecast.TotalDRCount(intAnswer, boolUseCSM).ToString());
                    int intDRCurrent = Int32.Parse(ds.Tables[0].Rows[0]["recovery_number"].ToString());
                    int intHAForecast = Int32.Parse(oForecast.TotalHACount(intAnswer, boolUseCSM).ToString());
                    int intHACurrent = Int32.Parse(ds.Tables[0].Rows[0]["ha"].ToString());
                    bool boolHADisabled = (oModelsProperties.IsHighAvailability(intModel) == false);
                    bool boolHA = oForecast.IsHARoom(intAnswer);
                    int _classid = Int32.Parse(ds.Tables[0].Rows[0]["classid"].ToString());
                    if (oClass.Get(_classid, "prod") == "1")
                    {
                        if (oModelsProperties.IsStorageDE_FDriveMustBeOnSAN(intModel) == true)
                        {
                            radDRNo.Checked = false;
                            radDRYes.Checked = true;
                            radDRNo.Enabled = false;
                            radDRYes.Enabled = false;
                            divDR1.Style["display"] = "inline";
                            divDR2.Style["display"] = "inline";
                        }
                        if (ds.Tables[0].Rows[0]["test"].ToString() == "1")
                            panTest.Visible = true;
                    }
                    if (boolHA == false)
                    {
                        radHANo.Checked = true;
                        radHAYes.Checked = false;
                        radHANo.Enabled = false;
                        radHAYes.Enabled = false;
                    }
                    if (intCluster > 0)
                    {
                        panCluster.Visible = false;
                        radHANo.Checked = true;
                        radHAYes.Checked = false;
                        radHANo.Enabled = false;
                        radHAYes.Enabled = false;
                        radDRNo.Checked = true;
                        radDRYes.Checked = false;
                        radDRNo.Enabled = false;
                        radDRYes.Enabled = false;
                        divDR1.Style["display"] = "none";
                        divDR2.Style["display"] = "none";
                    }
                    int _environmentid = Int32.Parse(ds.Tables[0].Rows[0]["environmentid"].ToString());
                    intServer = Int32.Parse(ds.Tables[0].Rows[0]["quantity"].ToString()) - oForecast.TotalServerCount(intAnswer, boolUseCSM);
                    intDR = Int32.Parse(ds.Tables[0].Rows[0]["recovery_number"].ToString()) - oForecast.TotalDRCount(intAnswer, boolUseCSM);
                    intRequest = oForecast.GetRequestID(intAnswer, true);
                    if (ds.Tables[0].Rows[0]["applicationid"].ToString() == "0")
                        panInfrastructure.Visible = true;
                    if (!IsPostBack)
                    {
                        LoadLists(_classid, _environmentid);
                        int intDBA = 0;
                        ds = oServer.Get(intAnswer, intConfig, intCluster, intNumber);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            panComponents.Visible = true;
                            frmComponents.Attributes.Add("src", "/frame/ondemand/config_server_components.aspx" + Request.Url.Query);
                            intServer = Int32.Parse(ds.Tables[0].Rows[0]["id"].ToString());
                            Page.Title = "ClearView Configure Device | Server # " + intServer.ToString();
                            if (ds.Tables[0].Rows[0]["step"].ToString() != "0")
                                btnSaveConfig.Visible = false;
                            lblId.Text = intServer.ToString();
                            Int32.TryParse(ds.Tables[0].Rows[0]["osid"].ToString(), out intOS);
                            if (intOS > 0 && ddlOS.SelectedValue == "0")
                                ddlOS.SelectedValue = intOS.ToString();
                            int intSP = 0;
                            Int32.TryParse(ds.Tables[0].Rows[0]["spid"].ToString(), out intSP);
                            ddlServicePack.SelectedValue = intSP.ToString();
                            ddlMaintenance.SelectedValue = intSP.ToString();
                            ddlTemplate.SelectedValue = ds.Tables[0].Rows[0]["templateid"].ToString();
                            ddlDomain.SelectedValue = ds.Tables[0].Rows[0]["domainid"].ToString();
                            ddlTestDomain.SelectedValue = ds.Tables[0].Rows[0]["test_domainid"].ToString();
                            intDBA = Int32.Parse(ds.Tables[0].Rows[0]["dba"].ToString());
                            if (ds.Tables[0].Rows[0]["infrastructure"].ToString() == "1")
                                radInfrastructureYes.Checked = true;
                            else
                                radInfrastructureNo.Checked = true;
                            if (ds.Tables[0].Rows[0]["ha"].ToString() == "1")
                                radHAYes.Checked = true;
                            else
                                radHANo.Checked = true;
                            if (ds.Tables[0].Rows[0]["dr"].ToString() == "1")
                            {
                                radDRYes.Checked = true;
                                divDR1.Style["display"] = "inline";
                                if (ds.Tables[0].Rows[0]["dr_exist"].ToString() == "1")
                                {
                                    radExistYes.Checked = true;
                                    divExist.Style["display"] = "inline";
                                    txtRecovery.Text = ds.Tables[0].Rows[0]["dr_name"].ToString();
                                }
                                else
                                    radExistNo.Checked = true;
                                divDR2.Style["display"] = "inline";
                                if (ds.Tables[0].Rows[0]["dr_consistency"].ToString() == "1")
                                {
                                    radConsistencyYes.Checked = true;
                                    divConsistency1.Style["display"] = "inline";
                                    divConsistency2.Style["display"] = "inline";
                                    int intConsistency = Int32.Parse(ds.Tables[0].Rows[0]["dr_consistencyid"].ToString());
                                    hdnConsistencyGroup.Value = intConsistency.ToString();
                                    txtConsistencyGroup.Text = oConsistencyGroup.Get(intConsistency, "name");
                                }
                                else
                                    radConsistencyNo.Checked = true;
                            }
                            else
                                radDRNo.Checked = true;

                            // Check selected items
                            DataSet dsSelected = oServerName.GetComponentDetailSelected(intServer, 1);
                            foreach (DataRow drSelected in dsSelected.Tables[0].Rows)
                            {
                                if (drSelected["sql"].ToString() == "1" || drSelected["dbase"].ToString() == "1")
                                {
                                    divDBA.Style["display"] = "inline";
                                    if (intDBA > 0)
                                    {
                                        txtUser.Text = oUser.GetFullName(intDBA) + " (" + oUser.GetName(intDBA) + ")";
                                        hdnUser.Value = intDBA.ToString();
                                    }
                                }
                            }
                        }
                        else
                            panComponentsNo.Visible = true;
                    }
                    if (intDRCurrent < intDRForecast || (intDRCurrent == intDRForecast && radDRYes.Checked == false))
                    {
                        radDRNo.Checked = true;
                        radDRYes.Checked = false;
                        radDRNo.Enabled = false;
                        radDRYes.Enabled = false;
                        divDR1.Style["display"] = "none";
                        divDR2.Style["display"] = "none";
                    }
                    if (intHACurrent < intHAForecast || (intHACurrent == intHAForecast && radHAYes.Checked == false))
                    {
                        radHANo.Checked = true;
                        radHAYes.Checked = false;
                        radHANo.Enabled = false;
                        radHAYes.Enabled = false;
                    }
                    btnClose.Attributes.Add("onclick", "return window.close();");

                    if (oModelsProperties.IsConfigServicePack(intModel) == true || oOperatingSystem.IsWindows(intOS) == true || oOperatingSystem.IsWindows2008(intOS) == true)
                        panSP.Visible = true;
                    if (oModelsProperties.IsConfigVMWareTemplate(intModel) == true)
                        panTemplate.Visible = true;
                    if (oModelsProperties.IsConfigMaintenanceLevel(intModel) == true)
                        panMaintenance.Visible = true;

                    string strInfrastructure = "";
                    if (panInfrastructure.Visible == true)
                        strInfrastructure = " && ValidateRadioButtons('" + radInfrastructureYes.ClientID + "','" + radInfrastructureNo.ClientID + "','Please choose if this server is an infrastructure server')";
                    string strTemplate = "";
                    if (panTemplate.Visible == true)
                        strTemplate = " && ValidateDropDown('" + ddlTemplate.ClientID + "','Please select a template')";
                    string strSP = "";
                    if (panSP.Visible == true)
                        strSP = " && ValidateDropDown('" + ddlServicePack.ClientID + "','Please select a service pack')";
                    if (panMaintenance.Visible == true)
                        strSP = " && ValidateDropDown('" + ddlMaintenance.ClientID + "','Please select a maintenance level')";
                    string strHA = "";
                    string strDR = "";
                    if (panCluster.Visible == true)
                    {
                        strHA = " && ValidateRadioButtons('" + radHAYes.ClientID + "','" + radHANo.ClientID + "','Please choose if this server has an HA counterpart')";
                        strDR = " && EnsureDR('" + radDRYes.ClientID + "','" + radDRNo.ClientID + "','" + radExistYes.ClientID + "','" + radExistNo.ClientID + "','" + txtRecovery.ClientID + "','" + radConsistencyYes.ClientID + "','" + radConsistencyNo.ClientID + "','" + hdnConsistencyGroup.ClientID + "')";
                    }
                    btnSaveConfig.Attributes.Add("onclick", "return ValidateDropDown('" + ddlDomain.ClientID + "','Please select a domain')" +
                        " && ValidateHiddenDisabled('" + hdnUser.ClientID + "','" + txtUser.ClientID + "','Please enter a username, first name or last name')" +
                        strSP +
                        strTemplate +
                        " && ValidateDropDown('" + ddlTestDomain.ClientID + "','Please select a domain')" +
                        strInfrastructure +
                        strHA +
                        strDR +
                        ";");
                }
            }
            else
                btnSaveConfig.Enabled = false;
            radDRYes.Attributes.Add("onclick", "ShowHideDiv('" + divDR1.ClientID + "','inline');ShowHideDiv('" + divDR2.ClientID + "','inline');ShowHideDivCheck('" + divExist.ClientID + "',document.getElementById('" + radExistYes.ClientID + "'));ShowHideDivCheck('" + divConsistency1.ClientID + "',document.getElementById('" + radConsistencyYes.ClientID + "'));ShowHideDivCheck('" + divConsistency2.ClientID + "',document.getElementById('" + radConsistencyYes.ClientID + "'));");
            radDRNo.Attributes.Add("onclick", "ShowHideDiv('" + divDR1.ClientID + "','none');ShowHideDiv('" + divDR2.ClientID + "','none');ShowHideDiv('" + divExist.ClientID + "','none');ShowHideDiv('" + divConsistency1.ClientID + "','none');ShowHideDiv('" + divConsistency2.ClientID + "','none');");
            radExistYes.Attributes.Add("onclick", "ShowHideDiv('" + divExist.ClientID + "','inline');");
            radExistNo.Attributes.Add("onclick", "ShowHideDiv('" + divExist.ClientID + "','none');");
            radConsistencyYes.Attributes.Add("onclick", "ShowHideDiv('" + divConsistency1.ClientID + "','inline');ShowHideDiv('" + divConsistency2.ClientID + "','inline');");
            radConsistencyNo.Attributes.Add("onclick", "ShowHideDiv('" + divConsistency1.ClientID + "','none');ShowHideDiv('" + divConsistency2.ClientID + "','none');");
            btnConsistencyServer.Attributes.Add("onclick", "return ShowConsistency('CONSISTENCY_SERVER','?id=0','" + txtConsistencyGroup.ClientID + "','" + hdnConsistencyGroup.ClientID + "');");
            btnConsistencyName.Attributes.Add("onclick", "return ShowConsistency('CONSISTENCY_SELECT','?id=0','" + txtConsistencyGroup.ClientID + "','" + hdnConsistencyGroup.ClientID + "');");
            btnConsistencyNew.Attributes.Add("onclick", "return ShowConsistency('CONSISTENCY_NEW','?id=0','" + txtConsistencyGroup.ClientID + "','" + hdnConsistencyGroup.ClientID + "');");
            Variables oVariable = new Variables(intEnvironment);
            txtUser.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'250','195','" + divAJAX.ClientID + "','" + lstAJAX.ClientID + "','" + hdnUser.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
            lstAJAX.Attributes.Add("ondblclick", "AJAXClickRow();");
        }
        private void LoadLists(int _classid, int _environmentid)
        {
            ddlOS.DataValueField = "id";
            ddlOS.DataTextField = "name";
            ddlOS.DataSource = oOperatingSystem.Gets(0, 1);
            ddlOS.DataBind();
            ddlOS.Items.Insert(0, new ListItem("-- SELECT --", "0"));
            bool boolEnabled = false;
            if (intOS == 0)
            {
                string strOS = oForecast.GetAnswerPlatformResponse(intAnswer, intOSQuestion);
                if (strOS != "")
                {
                    foreach (ListItem oItem in ddlOS.Items)
                    {
                        if (oItem.Text == strOS)
                        {
                            intOS = Int32.Parse(oItem.Value);
                            break;
                        }
                    }
                }
                if (intOS == 0 && Request.QueryString["osid"] != null && Request.QueryString["osid"] != "")
                {
                    intOS = Int32.Parse(Request.QueryString["osid"]);
                    boolEnabled = true;
                }
            }
            if (intOS > 0)
            {
                ddlOS.SelectedValue = intOS.ToString();
                ddlOS.Enabled = boolEnabled;
                ddlServicePack.DataValueField = "id";
                ddlServicePack.DataTextField = "name";
                ddlServicePack.DataSource = oOperatingSystem.GetServicePack(intOS);
                ddlServicePack.DataBind();
                ddlServicePack.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                ddlMaintenance.DataValueField = "id";
                ddlMaintenance.DataTextField = "name";
                ddlMaintenance.DataSource = oOperatingSystem.GetServicePack(intOS);
                ddlMaintenance.DataBind();
                ddlMaintenance.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                ddlTemplate.DataValueField = "id";
                ddlTemplate.DataTextField = "name";
                ddlTemplate.DataSource = oVMWare.GetTemplateClassEnvironment(_classid, _environmentid);
                ddlTemplate.DataBind();
                ddlTemplate.Items.Insert(0, new ListItem("-- SELECT --", "0"));
            }
            else
            {
                ServicePacks oServicePack = new ServicePacks(intProfile, dsn);
                ddlServicePack.DataValueField = "id";
                ddlServicePack.DataTextField = "name";
                ddlServicePack.DataSource = oServicePack.Gets(1);
                ddlServicePack.DataBind();
                ddlServicePack.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                ddlMaintenance.DataValueField = "id";
                ddlMaintenance.DataTextField = "name";
                ddlMaintenance.DataSource = oServicePack.Gets(1);
                ddlMaintenance.DataBind();
                ddlMaintenance.Items.Insert(0, new ListItem("-- SELECT --", "0"));
            }
            Domains oDomain = new Domains(intProfile, dsn);
            ddlDomain.DataValueField = "id";
            ddlDomain.DataTextField = "name";
            ddlDomain.DataSource = oDomain.GetClassEnvironment(_classid, _environmentid);
            ddlDomain.DataBind();
            ddlDomain.Items.Insert(0, new ListItem("-- SELECT --", "0"));
            ddlTestDomain.DataValueField = "id";
            ddlTestDomain.DataTextField = "name";
            ddlTestDomain.DataSource = oDomain.GetsTest(1);
            ddlTestDomain.DataBind();
            ddlTestDomain.Items.Insert(0, new ListItem("-- SELECT --", "0"));
        }
        protected void ddlOS_Change(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path + "?aid=" + Request.QueryString["aid"] + "&clusterid=" + Request.QueryString["clusterid"] + "&csmid=" + Request.QueryString["csmid"] + "&num=" + Request.QueryString["num"] + "&osid=" + ddlOS.SelectedItem.Value);
        }
        protected void btnSaveConfig_Click(Object Sender, EventArgs e)
        {
            int intConfigured = (ddlOS.SelectedIndex > 0 && (panSP.Visible == false || ddlServicePack.SelectedIndex > 0) && (panMaintenance.Visible == false || ddlMaintenance.SelectedIndex > 0) && (panTemplate.Visible == false || ddlTemplate.SelectedIndex > 0) && ddlDomain.SelectedIndex > 0 && (panTest.Visible == false || ddlTestDomain.SelectedIndex > 0) && (panInfrastructure.Visible == false || radInfrastructureYes.Checked == true || radInfrastructureNo.Checked == true) && (radDRYes.Checked == true || radDRNo.Checked == true) ? 1 : 0);
            int intServer = 0;
            int intDBA = 0;
            if (Request.Form[hdnUser.UniqueID] != "")
                intDBA = Int32.Parse(Request.Form[hdnUser.UniqueID]);
            int intSP = 0;
            if (Int32.Parse(ddlServicePack.SelectedItem.Value) > 0)
                intSP = Int32.Parse(ddlServicePack.SelectedItem.Value);
            else if (Int32.Parse(ddlMaintenance.SelectedItem.Value) > 0)
                intSP = Int32.Parse(ddlMaintenance.SelectedItem.Value);
            int intClass = Int32.Parse(oForecast.GetAnswer(intAnswer, "classid"));
            bool boolPNC = (oClass.Get(intClass, "pnc") == "1");
            if (lblId.Text != "")
            {
                intServer = Int32.Parse(lblId.Text);
                oServer.Update(intServer, Int32.Parse(ddlOS.SelectedItem.Value), intSP, Int32.Parse(ddlTemplate.SelectedItem.Value), Int32.Parse(ddlDomain.SelectedItem.Value), Int32.Parse(ddlTestDomain.SelectedItem.Value), (radInfrastructureYes.Checked ? 1 : 0), (radHAYes.Checked ? 1 : 0), (radDRYes.Checked ? 1 : 0), (radDRYes.Checked && radExistYes.Checked ? 1 : 0), (radDRYes.Checked && radExistYes.Checked ? txtRecovery.Text : ""), (radDRYes.Checked && radConsistencyYes.Checked ? 1 : 0), (radDRYes.Checked && radConsistencyYes.Checked ? Int32.Parse(Request.Form[hdnConsistencyGroup.UniqueID]) : 0), intConfigured, intDBA, (boolPNC == true ? 1 : 0));
            }
            else
            {
                bool boolFDrive = (oForecast.IsDRUnder48(intAnswer, false));
                intServer = oServer.Add(intRequest, intAnswer, 0, intConfig, intCluster, intNumber, Int32.Parse(ddlOS.SelectedItem.Value), intSP, Int32.Parse(ddlTemplate.SelectedItem.Value), Int32.Parse(ddlDomain.SelectedItem.Value), Int32.Parse(ddlTestDomain.SelectedItem.Value), (radInfrastructureYes.Checked ? 1 : 0), (radHAYes.Checked ? 1 : 0), (radDRYes.Checked ? 1 : 0), (radDRYes.Checked && radExistYes.Checked ? 1 : 0), (radDRYes.Checked && radExistYes.Checked ? txtRecovery.Text : ""), (radDRYes.Checked && radConsistencyYes.Checked ? 1 : 0), (radDRYes.Checked && radConsistencyYes.Checked ? Int32.Parse(Request.Form[hdnConsistencyGroup.UniqueID]) : 0), intConfigured, (oForecast.IsStorage(intAnswer) ? 0 : -1), 0, (boolFDrive ? 0 : -1), intDBA, (boolPNC == true ? 1 : 0), 0, (boolPNC == true ? 0 : -100));
                if (intCluster > 0)
                {
                    Cluster oCluster = new Cluster(0, dsn);
                    oCluster.UpdateNonShared(intCluster, 1);
                }
                // Change configured to 0 so that when loading the components, the pre-selected stuff will appear (only when first loading...not after reloading a saved one)
                oServer.UpdateConfigured(intServer, 0);
            }
            // Add Break Fix Name
            string strName = oForecast.GetAnswer(intAnswer, "nameid");
            if (strName != "" && strName != "0")
                oServer.UpdateServerNamed(intServer, Int32.Parse(strName));

            bool boolReset = false;
            DataSet dsOld = oServerName.GetComponentDetailSelected(intServer, 1);

            // Save Components
            if (Request.Form["hdnComponents"] != null)
            {
                string strComponents = Request.Form["hdnComponents"];
                //if (strComponents != "")
                //{
                    oServerName.DeleteComponentDetailSelected(intServer);
                    while (strComponents != "")
                    {
                        int intDetail = Int32.Parse(strComponents.Substring(0, strComponents.IndexOf("&")));
                        if (intDetail > 0)
                        {
                            oServerName.AddComponentDetailSelected(intServer, intDetail, 0, true);
                            oServerName.AddComponentDetailPrerequisites(intServer, intDetail, true);
                        }
                        strComponents = strComponents.Substring(strComponents.IndexOf("&") + 1);
                    }
                //}
            }

            // Save Application Prerequisites
            int intApplication = Int32.Parse(oForecast.GetAnswer(intAnswer, "applicationid"));
            DataSet dsInclude = oServerName.GetComponentDetailSelectedRelated(intApplication, 1);
            foreach (DataRow drInclude in dsInclude.Tables[0].Rows)
            {
                int intDetail = Int32.Parse(drInclude["detailid"].ToString());
                oServerName.AddComponentDetailSelected(intServer, intDetail, -1, true);
                oServerName.AddComponentDetailPrerequisites(intServer, intDetail, true);
            }

            // Send to software component approver(s)
            oServerName.SendComponentNotification(intAnswer, intEnvironment,  dsnAsset, dsnIP, intSoftwarePageID);

            DataSet dsNew = oServerName.GetComponentDetailSelected(intServer, 1);
            // Check components and erase if necessary
            foreach (DataRow drOld in dsOld.Tables[0].Rows)
            {
                bool boolFound = false;
                foreach (DataRow drNew in dsNew.Tables[0].Rows)
                {
                    if (drOld["detailid"].ToString() == drNew["detailid"].ToString())
                    {
                        boolFound = true;
                        break;
                    }
                }
                if (boolFound == false)
                {
                    if (drOld["reset_storage"].ToString() == "1")
                    {
                        boolReset = true;
                        break;
                    }
                }
            }
            if (boolReset == false)
            {
                foreach (DataRow drNew in dsNew.Tables[0].Rows)
                {
                    bool boolFound = false;
                    foreach (DataRow drOld in dsOld.Tables[0].Rows)
                    {
                        if (drNew["detailid"].ToString() == drOld["detailid"].ToString())
                        {
                            boolFound = true;
                            break;
                        }
                    }
                    if (boolFound == false)
                    {
                        if (drNew["reset_storage"].ToString() == "1")
                        {
                            boolReset = true;
                            break;
                        }
                    }
                }
            }
            if (boolReset == true)
            {
                oStorage.DeleteLuns(intAnswer, 0, intCluster, intConfig, intNumber);
                oServer.UpdateLocalStorage(intServer, 0);
                if (intCluster > 0)
                {
                    Cluster oCluster = new Cluster(intProfile, dsn);
                    oCluster.UpdateAddInstance(intCluster, 0);
                }
                if (oServer.Get(intServer, "fdrive") == "-1" || oServer.Get(intServer, "fdrive") == "1")
                    oServer.UpdateFDrive(intServer, 0);
            }

            // Check if DBA is required
            bool boolDBA = false;
            foreach (DataRow drNew in dsNew.Tables[0].Rows)
            {
                if (drNew["sql"].ToString() == "1" || drNew["dbase"].ToString() == "1")
                {
                    boolDBA = true;
                    break;
                }
            }
            
            // Redirect
            if (panComponentsNo.Visible == true)
                Response.Redirect(Request.Url.PathAndQuery + "&menu_tab=2&refresh=true");
            else
            {
                if (boolDBA == true && intDBA == 0)
                {
                    oServer.UpdateConfigured(intServer, 0);
                    Response.Redirect(Request.Url.PathAndQuery + "&required=true&dba=true");
                }
                else
                    Response.Redirect(Request.Url.PathAndQuery + "&save=true");
            }
        }
    }
}
