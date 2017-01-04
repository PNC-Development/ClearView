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
using System.Text;

namespace NCC.ClearView.Presentation.Web
{
    public partial class fore_approve_cfi : System.Web.UI.UserControl
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        private string dsnServiceEditor = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ServiceEditorDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intLocation = Int32.Parse(ConfigurationManager.AppSettings["OPSLocationID"]);
        protected string strWorkflowBCC = ConfigurationManager.AppSettings["WorkflowBCC"];
        protected int intConfidenceUnlock = Int32.Parse(ConfigurationManager.AppSettings["CONFIDENCE_UNLOCK"]);
        protected int intAssignPage = Int32.Parse(ConfigurationManager.AppSettings["AssignWorkload"]);
        protected int intResourceRequestApprove = Int32.Parse(ConfigurationManager.AppSettings["ResourceRequestApprove"]);
        protected int intViewPage = Int32.Parse(ConfigurationManager.AppSettings["ViewWorkload"]);
        protected int intImplementorDistributedService = Int32.Parse(ConfigurationManager.AppSettings["SERVICEID_IMPLEMENTOR_DISTRIBUTED"]);
        protected int intImplementorMidrangeService = Int32.Parse(ConfigurationManager.AppSettings["SERVICEID_IMPLEMENTOR_MIDRANGE"]);
        protected int intWorkstationPlatform = Int32.Parse(ConfigurationManager.AppSettings["WorkstationPlatformID"]);
        protected int intProfile;
        protected Design oDesign;
        protected Pages oPage;
        protected Variables oVariable;
        protected Mnemonic oMnemonic;
        protected CostCenter oCostCenter;
        protected Classes oClass;
        protected Environments oEnvironment;
        protected Locations oLocation;
        protected OperatingSystems oOperatingSystem;
        protected Domains oDomain;
        protected ServerName oServerName;
        protected Holidays oHoliday;
        protected Users oUser;
        protected Platforms oPlatform;
        protected Types oType;
        protected Models oModel;
        protected ModelsProperties oModelsProperties;
        protected Functions oFunction;
        protected Log oLog;

        protected int intApplication = 0;
        protected int intPage = 0;
        protected int intID = 0;
        protected string strMenuTabApprove1 = "";
        protected string strMenuTabApprove2 = "";
        protected string strWindow = "111111111111111111111111";
        protected string strGridBackup = "";
        protected string strGridBackupTable = "";
        protected string strGridBackupSun = "";
        protected string strGridBackupMon = "";
        protected string strGridBackupTue = "";
        protected string strGridBackupWed = "";
        protected string strGridBackupThu = "";
        protected string strGridBackupFri = "";
        protected string strGridBackupSat = "";
        private int intGridBackups = 0;
        protected string strGridMaintenance = "";
        protected string strGridMaintenanceTable = "";
        protected string strGridMaintenanceSun = "";
        protected string strGridMaintenanceMon = "";
        protected string strGridMaintenanceTue = "";
        protected string strGridMaintenanceWed = "";
        protected string strGridMaintenanceThu = "";
        protected string strGridMaintenanceFri = "";
        protected string strGridMaintenanceSat = "";
        private int intGridMaintenances = 0;
        protected bool boolWindows = false;
        protected string strLocation = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oDesign = new Design(intProfile, dsn);
            oPage = new Pages(intProfile, dsn);
            oVariable = new Variables(intEnvironment);
            oMnemonic = new Mnemonic(intProfile, dsn);
            oCostCenter = new CostCenter(intProfile, dsn);
            oClass = new Classes(intProfile, dsn);
            oEnvironment = new Environments(intProfile, dsn);
            oLocation = new Locations(intProfile, dsn);
            oOperatingSystem = new OperatingSystems(intProfile, dsn);
            oDomain = new Domains(intProfile, dsn);
            oServerName = new ServerName(intProfile, dsn);
            oHoliday = new Holidays(intProfile, dsn);
            oUser = new Users(intProfile, dsn);
            oPlatform = new Platforms(intProfile, dsn);
            oType = new Types(intProfile, dsn);
            oModel = new Models(intProfile, dsn);
            oModelsProperties = new ModelsProperties(intProfile, dsn);
            oFunction = new Functions(intProfile, dsn, intEnvironment);
            oLog = new Log(intProfile, dsn);

            string strMHS = "";
            if (!IsPostBack && Request.QueryString["duplicate"] != null && Request.QueryString["duplicate"] != "")
                Page.ClientScript.RegisterStartupScript(typeof(Page), "duplicate", "<script type=\"text/javascript\">alert('The account " + Request.QueryString["duplicate"] + " has already been configured');<" + "/" + "script>");
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.QueryString["action"] != null && Request.QueryString["action"] != "")
                panFinish.Visible = true;
            else
            {
                if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                    intApplication = Int32.Parse(Request.QueryString["applicationid"]);
                if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                    intApplication = Int32.Parse(Request.Cookies["application"].Value);
                if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                {
                    string strID = Request.QueryString["id"];
                    strID = oFunction.decryptQueryString(strID);
                    if (strID != "")
                        intID = Int32.Parse(strID);
                    else
                        intID = Int32.Parse(Request.QueryString["id"]);
                }
                if (intID > 0 && oDesign.Get(intID).Tables[0].Rows.Count == 0)
                {
                    // Must be the Forecast Answer ID....
                    Int32.TryParse(oDesign.GetAnswer(intID, "id"), out intID);
                }
                if (!IsPostBack)
                {
                    if (intID > 0)
                    {
                        lblID.Text = intID.ToString();
                        bool boolShow = false;
                        bool boolShowException = false;
                        bool boolAdmin = false;
                        DataSet dsApprovals = oDesign.GetApprovalsUser(intProfile, intID);
                        if (oUser.IsAdmin(intProfile) && dsApprovals.Tables[0].Rows.Count == 0)
                            boolShow = boolShowException = boolAdmin = true;
                        else if (dsApprovals.Tables[0].Rows.Count > 0)
                        {
                            boolShow = true;
                            lblType.Text = dsApprovals.Tables[0].Rows[0]["type"].ToString();
                            lblTypeID.Text = dsApprovals.Tables[0].Rows[0]["typeid"].ToString();
                            if (lblType.Text == "S")
                            {
                                // Show licensing...not needed since nobody uses it.

                                // 
                                int intComponentDetailID = 0;
                                if (Int32.TryParse(dsApprovals.Tables[0].Rows[0]["componentid"].ToString(), out intComponentDetailID) == true)
                                {
                                    // Contains the ID of the component
                                    int intComponentID = 0;
                                    if (Int32.TryParse(oServerName.GetComponentDetail(intComponentDetailID, "componentid"), out intComponentID) == true)
                                    {
                                        if (oServerName.GetComponent(intComponentID, "iis") == "1")
                                        {
                                            trMHS.Visible = true;
                                            strMHS = " && ValidateRadioButtons('" + radMHSYes.ClientID + "','" + radMHSNo.ClientID + "','Please select whether or not the server(s) associated with this design will be hosted by MHS')";
                                        }
                                    }
                                }
                            }
                            int intException = 0;
                            if (Int32.TryParse(dsApprovals.Tables[0].Rows[0]["only_exceptions"].ToString(), out intException) == true)
                                boolShowException = (intException == 1);
                            lblUserID.Text = dsApprovals.Tables[0].Rows[0]["managerid"].ToString();
                        }

                        if (boolShow)
                        {
                            panException.Visible = boolShowException;

                            DataSet ds = oDesign.Get(intID);
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                panShow.Visible = true;
                                btnApprove.Enabled = (boolAdmin == false);
                                btnDeny.Enabled = (boolAdmin == false);

                                if (panException.Visible == true)
                                {
                                    // Lock it for override editing.
                                    oDesign.UpdateExceptionDone(intID, true);

                                    // Show Comments
                                    DataSet dsSubmitted = oDesign.GetSubmitted(intID);
                                    if (dsSubmitted.Tables[0].Rows.Count > 0)
                                    {
                                        if (dsSubmitted.Tables[0].Rows[0]["comments"].ToString() != "")
                                        {
                                            lblComments.Text = oFunction.FormatText(dsSubmitted.Tables[0].Rows[0]["comments"].ToString());
                                            txtExceptionID.Text = dsSubmitted.Tables[0].Rows[0]["exceptionID"].ToString();
                                            btnException.CommandArgument = dsSubmitted.Tables[0].Rows[0]["userid"].ToString();
                                        }
                                    }

                                    DataRow dr = ds.Tables[0].Rows[0];
                                    // Load information
                                    int intMnemonic = 0;
                                    if (Int32.TryParse(dr["mnemonicid"].ToString(), out intMnemonic) == true && intMnemonic > 0)
                                        lblMnemonic.Text = oMnemonic.Get(intMnemonic, "factory_code") + " - " + oMnemonic.Get(intMnemonic, "name");
                                    int intCostCenter = 0;
                                    if (Int32.TryParse(dr["costid"].ToString(), out intCostCenter) == true && intCostCenter > 0)
                                        lblCost.Text = oCostCenter.GetName(intCostCenter);
                                    int intClass = 0;
                                    Int32.TryParse(dr["classid"].ToString(), out intClass);
                                    ddlClass.DataTextField = "name";
                                    ddlClass.DataValueField = "id";
                                    ddlClass.DataSource = oClass.GetForecasts(1);
                                    ddlClass.DataBind();
                                    ddlClass.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                                    ddlClass.SelectedValue = intClass.ToString();
                                    int intEnv = 0;
                                    Int32.TryParse(dr["environmentid"].ToString(), out intEnv);
                                    ddlEnvironment.Enabled = true;
                                    ddlEnvironment.DataTextField = "name";
                                    ddlEnvironment.DataValueField = "id";
                                    ddlEnvironment.DataSource = oClass.GetEnvironment(intClass, 1);
                                    ddlEnvironment.DataBind();
                                    ddlEnvironment.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                                    ddlEnvironment.SelectedValue = intEnv.ToString();
                                    hdnEnvironment.Value = intEnv.ToString();
                                    /*
                                    chkTypeDatabase.Checked = (dr["database"].ToString() == "1");
                                    chkTypeWeb.Checked = (dr["web"].ToString() == "1");
                                    divDatabase.Style["display"] = (chkTypeDatabase.Checked ? "inline" : "none");
                                    if (dr["oracle"].ToString() == "1")
                                        ddlDatabase.SelectedValue = "O";
                                    else if (dr["sql"].ToString() == "1")
                                        ddlDatabase.SelectedValue = "S";
                                    else if (dr["other_db"].ToString() == "1")
                                        ddlDatabase.SelectedValue = "X";
                                    */
                                    int intOS = 0;
                                    Int32.TryParse(dr["osid"].ToString(), out intOS);
                                    ddlOS.DataValueField = "id";
                                    ddlOS.DataTextField = "name";
                                    ddlOS.DataSource = oOperatingSystem.Gets(0, 1);
                                    ddlOS.DataBind();
                                    ddlOS.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                                    ddlOS.SelectedValue = intOS.ToString();
                                    ddlSP.DataValueField = "id";
                                    ddlSP.DataTextField = "name";
                                    ddlSP.DataSource = oOperatingSystem.GetServicePack(intOS);
                                    ddlSP.DataBind();
                                    ddlSP.Items.Insert(0, new ListItem("-- INHERIT FROM STANDARD --", "0"));
                                    ddlSP.SelectedValue = dr["spid"].ToString();
                                    ddlDomain.DataValueField = "id";
                                    ddlDomain.DataTextField = "name";
                                    ddlDomain.DataSource = oDomain.GetClassEnvironment(intClass, intEnv);
                                    ddlDomain.DataBind();
                                    ddlDomain.Items.Insert(0, new ListItem("-- INHERIT FROM STANDARD --", "0"));
                                    ddlDomain.SelectedValue = dr["domainid"].ToString();
                                    if (dr["backup_frequency"].ToString() != "")
                                        ddlBackup.SelectedValue = dr["backup_frequency"].ToString();
                                    if (dr["dr"].ToString() != "")
                                        ddlDR.SelectedValue = dr["dr"].ToString();
                                    if (dr["ha"].ToString() == "1")
                                    {
                                        if (dr["ha_clustering"].ToString() == "1")
                                        {
                                            divInstances.Style["display"] = "inline";
                                            divQuorum.Style["display"] = "inline";

                                            if (dr["active_passive"].ToString() == "1")
                                                radHAClusterAP.Checked = true;
                                            if (dr["active_passive"].ToString() == "2")
                                                radHAClusterAA.Checked = true;
                                        }
                                        else if (dr["ha_load_balancing"].ToString() == "1")
                                        {
                                            radHALoadBalance.Checked = true;
                                            divLoadBalance.Style["display"] = "inline";

                                            if (dr["middleware"].ToString() == "1")
                                                ddlLoadBalance.SelectedValue = "M";
                                            else if (dr["web"].ToString() == "0")
                                                ddlLoadBalance.SelectedValue = "A";
                                            else if (dr["application"].ToString() == "1")
                                                ddlLoadBalance.SelectedValue = "WA";
                                            else
                                                ddlLoadBalance.SelectedValue = "W";
                                        }
                                    }
                                    else if (dr["ha"].ToString() == "0")
                                        radHANone.Checked = true;
                                    txtInstances.Text = dr["instances"].ToString();
                                    txtQuorum.Text = dr["quorum"].ToString();
                                    if (dr["commitment"].ToString() != "")
                                    {
                                        lblDate.Text = DateTime.Parse(dr["commitment"].ToString()).ToShortDateString();
                                        //txtDate.Text = DateTime.Parse(dr["commitment"].ToString()).ToShortDateString();
                                    }
                                    lblSpecial.Text = dr["special"].ToString();
                                    txtQuantity.Text = dr["quantity"].ToString();
                                    txtCores.Text = dr["cores"].ToString();
                                    txtRam.Text = dr["ram"].ToString();

                                    // Confidence
                                    DataSet dsConfidence = oDesign.GetResponsesOther(1, 0, 0, 1);
                                    if (dsConfidence.Tables[0].Rows.Count > 0)
                                    {
                                        lblConfidence.Text = dsConfidence.Tables[0].Rows[0]["response"].ToString();
                                        /*
                                        int intQuestion = Int32.Parse(dsConfidence.Tables[0].Rows[0]["questionid"].ToString());
                                        dsConfidence = oDesign.GetResponses(intQuestion, 1);
                                        ddlConfidence.DataTextField = "response";
                                        ddlConfidence.DataValueField = "related_value";
                                        ddlConfidence.DataSource = dsConfidence;
                                        ddlConfidence.DataBind();
                                        ddlConfidence.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                                        ddlConfidence.SelectedValue = dr["confidence"].ToString();
                                        */
                                    }

                                    int intApplicationID = 0;
                                    Int32.TryParse(dr["applicationid"].ToString(), out intApplicationID);
                                    ddlApplications.DataTextField = "name";
                                    ddlApplications.DataValueField = "id";
                                    ddlApplications.DataSource = oServerName.GetApplicationsForecast(1);
                                    ddlApplications.DataBind();
                                    ddlApplications.Items.Insert(0, new ListItem("-- NONE --", "0"));
                                    if (intApplicationID > 0)
                                    {
                                        ddlApplications.SelectedValue = intApplicationID.ToString();
                                        DataSet dsSubApplications = oServerName.GetSubApplications(intApplicationID, 1);
                                        if (dsSubApplications.Tables[0].Rows.Count > 0)
                                        {
                                            int intSubApplicationID = 0;
                                            Int32.TryParse(dr["subapplicationid"].ToString(), out intSubApplicationID);
                                            ddlSubApplications.DataTextField = "name";
                                            ddlSubApplications.DataValueField = "id";
                                            ddlSubApplications.DataSource = dsSubApplications;
                                            ddlSubApplications.DataBind();
                                            ddlSubApplications.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                                            divSubApplications.Style["display"] = "inline";
                                            ddlSubApplications.SelectedValue = intSubApplicationID.ToString();
                                            hdnSubApplication.Value = intSubApplicationID.ToString();
                                        }
                                    }

                                    // ACCOUNTS
                                    rptAccounts.DataSource = oDesign.GetAccounts(intID);
                                    rptAccounts.DataBind();
                                    foreach (RepeaterItem ri in rptAccounts.Items)
                                    {
                                        LinkButton _delete = (LinkButton)ri.FindControl("btnDeleteAccount");
                                        _delete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this account?');");
                                        Label _permissions = (Label)ri.FindControl("lblPermissions");
                                        switch (_permissions.Text)
                                        {
                                            case "0":
                                                _permissions.Text = "-----";
                                                break;
                                            case "D":
                                                _permissions.Text = "Developer";
                                                break;
                                            case "P":
                                                _permissions.Text = "Promoter";
                                                break;
                                            case "S":
                                                _permissions.Text = "AppSupport";
                                                break;
                                            case "U":
                                                _permissions.Text = "AppUsers";
                                                break;
                                        }
                                        if (_permissions.ToolTip == "1")
                                            _permissions.Text += " (R/D)";
                                    }
                                    if (rptAccounts.Items.Count == 0)
                                        lblNone.Visible = true;

                                    // SOFTWARE
                                    frmComponents.Attributes.Add("src", "/frame/ondemand/config_server_components.aspx?designid=" + intID.ToString());

                                    // STORAGE
                                    ddlStorageDriveNew.DataValueField = "id";
                                    ddlStorageDriveNew.DataTextField = "path";
                                    ddlStorageDriveNew.DataSource = oDesign.GetDrives();
                                    ddlStorageDriveNew.DataBind();
                                    LoadStorage(intID);

                                    // BACKUP
                                    DataSet dsBackup = oDesign.GetBackup(intID);
                                    if (dsBackup.Tables[0].Rows.Count > 0)
                                    {
                                        DataRow drBackup = dsBackup.Tables[0].Rows[0];
                                        strGridBackupSun = drBackup["sun"].ToString();
                                        strGridBackupMon = drBackup["mon"].ToString();
                                        strGridBackupTue = drBackup["tue"].ToString();
                                        strGridBackupWed = drBackup["wed"].ToString();
                                        strGridBackupThu = drBackup["thu"].ToString();
                                        strGridBackupFri = drBackup["fri"].ToString();
                                        strGridBackupSat = drBackup["sat"].ToString();

                                        StringBuilder strTemp = new StringBuilder();
                                        strTemp.Append(oDesign.LoadGrid(strGridBackupSun, 1, ref intGridBackups));
                                        strTemp.Append(oDesign.LoadGrid(strGridBackupMon, 2, ref intGridBackups));
                                        strTemp.Append(oDesign.LoadGrid(strGridBackupTue, 3, ref intGridBackups));
                                        strTemp.Append(oDesign.LoadGrid(strGridBackupWed, 4, ref intGridBackups));
                                        strTemp.Append(oDesign.LoadGrid(strGridBackupThu, 5, ref intGridBackups));
                                        strTemp.Append(oDesign.LoadGrid(strGridBackupFri, 6, ref intGridBackups));
                                        strTemp.Append(oDesign.LoadGrid(strGridBackupSat, 7, ref intGridBackups));
                                        strGridBackup = strTemp.ToString();
                                    }
                                    else
                                    {
                                        strGridBackupSun = strWindow;
                                        strGridBackupMon = strWindow;
                                        strGridBackupTue = strWindow;
                                        strGridBackupWed = strWindow;
                                        strGridBackupThu = strWindow;
                                        strGridBackupFri = strWindow;
                                        strGridBackupSat = strWindow;
                                    }
                                    strGridBackupTable = oDesign.LoadMatrix("Backup", "B", strGridBackupSun, strGridBackupMon, strGridBackupTue, strGridBackupWed, strGridBackupThu, strGridBackupFri, strGridBackupSat);

                                    // EXCLUSIONS
                                    rptExclusions.DataSource = oDesign.GetExclusions(intID);
                                    rptExclusions.DataBind();
                                    foreach (RepeaterItem ri in rptExclusions.Items)
                                    {
                                        LinkButton _delete = (LinkButton)ri.FindControl("btnDeleteExclusion");
                                        _delete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this exclusion?');");
                                    }
                                    if (rptExclusions.Items.Count == 0)
                                        lblExclusion.Visible = true;

                                    // MAINTENANCE
                                    DataSet dsMaintenance = oDesign.GetMaintenance(intID);
                                    if (dsMaintenance.Tables[0].Rows.Count > 0)
                                    {
                                        DataRow drMaintenance = dsMaintenance.Tables[0].Rows[0];
                                        strGridMaintenanceSun = drMaintenance["sun"].ToString();
                                        strGridMaintenanceMon = drMaintenance["mon"].ToString();
                                        strGridMaintenanceTue = drMaintenance["tue"].ToString();
                                        strGridMaintenanceWed = drMaintenance["wed"].ToString();
                                        strGridMaintenanceThu = drMaintenance["thu"].ToString();
                                        strGridMaintenanceFri = drMaintenance["fri"].ToString();
                                        strGridMaintenanceSat = drMaintenance["sat"].ToString();

                                        StringBuilder strTemp = new StringBuilder();
                                        strTemp.Append(oDesign.LoadGrid(strGridMaintenanceSun, 1, ref intGridMaintenances));
                                        strTemp.Append(oDesign.LoadGrid(strGridMaintenanceMon, 2, ref intGridMaintenances));
                                        strTemp.Append(oDesign.LoadGrid(strGridMaintenanceTue, 3, ref intGridMaintenances));
                                        strTemp.Append(oDesign.LoadGrid(strGridMaintenanceWed, 4, ref intGridMaintenances));
                                        strTemp.Append(oDesign.LoadGrid(strGridMaintenanceThu, 5, ref intGridMaintenances));
                                        strTemp.Append(oDesign.LoadGrid(strGridMaintenanceFri, 6, ref intGridMaintenances));
                                        strTemp.Append(oDesign.LoadGrid(strGridMaintenanceSat, 7, ref intGridMaintenances));
                                        strGridMaintenance = strTemp.ToString();
                                    }
                                    else
                                    {
                                        strGridMaintenanceSun = strWindow;
                                        strGridMaintenanceMon = strWindow;
                                        strGridMaintenanceTue = strWindow;
                                        strGridMaintenanceWed = strWindow;
                                        strGridMaintenanceThu = strWindow;
                                        strGridMaintenanceFri = strWindow;
                                        strGridMaintenanceSat = strWindow;
                                    }
                                    strGridMaintenanceTable = oDesign.LoadMatrix("Maintenance", "M", strGridMaintenanceSun, strGridMaintenanceMon, strGridMaintenanceTue, strGridMaintenanceWed, strGridMaintenanceThu, strGridMaintenanceFri, strGridMaintenanceSat);

                                    // MODEL
                                    ddlPlatform.Attributes.Add("onchange", "PopulatePlatformTypes('" + ddlPlatform.ClientID + "','" + ddlPlatformType.ClientID + "','" + ddlPlatformModel.ClientID + "','" + ddlPlatformModelProperty.ClientID + "');ResetDropDownHidden('" + hdnModel.ClientID + "');");
                                    int intModel = oDesign.GetModelProperty(intID);
                                    int intModelParent = 0;
                                    Int32.TryParse(oModelsProperties.Get(intModel, "modelid"), out intModelParent);
                                    int intType = oModel.GetType(intModelParent);
                                    int intPlatform = oType.GetPlatform(intType);
                                    hdnModel.Value = intModel.ToString();

                                    ddlPlatformModelProperty.DataValueField = "id";
                                    ddlPlatformModelProperty.DataTextField = "name";
                                    ddlPlatformModelProperty.DataSource = oModelsProperties.GetModels(0, intModelParent, 1);
                                    ddlPlatformModelProperty.DataBind();
                                    ddlPlatformModelProperty.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                                    ddlPlatformModelProperty.SelectedValue = intModel.ToString();
                                    ddlPlatformModel.DataValueField = "id";
                                    ddlPlatformModel.DataTextField = "name";
                                    ddlPlatformModel.DataSource = oModel.Gets(intType, 1);
                                    ddlPlatformModel.DataBind();
                                    ddlPlatformModel.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                                    ddlPlatformModel.SelectedValue = intModelParent.ToString();
                                    ddlPlatformType.DataValueField = "id";
                                    ddlPlatformType.DataTextField = "name";
                                    ddlPlatformType.DataSource = oType.Gets(intPlatform, 1);
                                    ddlPlatformType.DataBind();
                                    ddlPlatformType.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                                    ddlPlatformType.SelectedValue = intType.ToString();
                                    ddlPlatform.DataValueField = "platformid";
                                    ddlPlatform.DataTextField = "name";
                                    ddlPlatform.DataSource = oPlatform.Gets(1);
                                    ddlPlatform.DataBind();
                                    ddlPlatform.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                                    ddlPlatform.SelectedValue = intPlatform.ToString();

                                    int intAddress = 0;
                                    Int32.TryParse(dr["addressid"].ToString(), out intAddress);
                                    intLocation = intAddress;
                                    strLocation = oLocation.LoadDDL("ddlState", "ddlCity", "ddlAddress", hdnLocation.ClientID, intLocation, true, "ddlCommon", "Cleveland or Summit Data Center");
                                    hdnLocation.Value = intLocation.ToString();
                                }
                            }
                            else
                                panException.Visible = false;
                        }
                    }
                }
                if (panShow.Visible == true)
                {
                    DesignControl oControl = (DesignControl)LoadControl("/controls/fore/fore_design.ascx");
                    oControl.DesignId = intID;
                    phSummary.Controls.Add(oControl);
                }
                if (panException.Visible == true)
                {
                    int intMenuTab = 0;
                    if (Request.QueryString["menu_tab_approve"] != null && Request.QueryString["menu_tab_approve"] != "")
                        intMenuTab = Int32.Parse(Request.QueryString["menu_tab_approve"]);
                    LoadTab1(intMenuTab);

                    int intMenuTab2 = 0;
                    if (Request.QueryString["menu_tab_approve2"] != null && Request.QueryString["menu_tab_approve2"] != "")
                        intMenuTab2 = Int32.Parse(Request.QueryString["menu_tab_approve2"]);
                    LoadTab2(intMenuTab2);

                    ddlClass.Attributes.Add("onchange", "PopulateEnvironments('" + ddlClass.ClientID + "','" + ddlEnvironment.ClientID + "',1);");
                    ddlEnvironment.Attributes.Add("onchange", "UpdateDropDownHidden('" + ddlEnvironment.ClientID + "','" + hdnEnvironment.ClientID + "');");
                    ddlApplications.Attributes.Add("onchange", "PopulateSubApplications('" + ddlApplications.ClientID + "','" + ddlSubApplications.ClientID + "','" + divSubApplications.ClientID + "');");
                    ddlSubApplications.Attributes.Add("onchange", "UpdateDropDownHidden('" + ddlSubApplications.ClientID + "','" + hdnSubApplication.ClientID + "');");
                    //chkTypeDatabase.Attributes.Add("onclick", "ShowHideDivCheck('" + divDatabase.ClientID + "',this);");
                    //imgDate.Attributes.Add("onclick", "return ShowCalendarMin('" + txtDate.ClientID + "',10);");
                    //hdnDate.Value = oHoliday.GetDays(10.00, DateTime.Today).ToShortDateString();
                    radHAClusterAA.Attributes.Add("onclick", "ShowHideDiv('" + divInstances.ClientID + "','inline');ShowHideDiv('" + divQuorum.ClientID + "','inline');ShowHideDiv('" + divLoadBalance.ClientID + "','none');");
                    radHAClusterAP.Attributes.Add("onclick", "ShowHideDiv('" + divInstances.ClientID + "','inline');ShowHideDiv('" + divQuorum.ClientID + "','inline');ShowHideDiv('" + divLoadBalance.ClientID + "','none');");
                    radHALoadBalance.Attributes.Add("onclick", "ShowHideDiv('" + divInstances.ClientID + "','none');ShowHideDiv('" + divQuorum.ClientID + "','none');ShowHideDiv('" + divLoadBalance.ClientID + "','inline');");
                    radHANone.Attributes.Add("onclick", "ShowHideDiv('" + divInstances.ClientID + "','none');ShowHideDiv('" + divQuorum.ClientID + "','none');ShowHideDiv('" + divLoadBalance.ClientID + "','none');");
                    txtAccount.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'300','195','" + divAccount.ClientID + "','" + lstAccount.ClientID + "','" + hdnAccount.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2,null,'PNC');");
                    lstAccount.Attributes.Add("ondblclick", "AJAXClickRow();");
                    btnAddAccount.Attributes.Add("onclick", "return ValidateHidden('" + hdnAccount.ClientID + "','" + txtAccount.ClientID + "','Enter a username, first name or last name') && ValidateDropDown('" + ddlPermission.ClientID + "','Select a permission') && ProcessButton(this) && LoadWait();");
                    ddlGridBackup.Attributes.Add("onchange", "SetConflictConfig(this,'tblBackup','hdnBackup');");
                    ddlGridBackup.Attributes.Add("onmousewheel", "return false;");
                    btnAddExclusion.Attributes.Add("onclick", "return ValidateText('" + txtPath.ClientID + "','Please enter the path') && ProcessButton(this) && LoadWait();");
                    ddlGridMaintenance.Attributes.Add("onchange", "SetConflictConfig(this,'tblMaintenance','hdnMaintenance');");
                    ddlGridMaintenance.Attributes.Add("onmousewheel", "return false;");
                    ddlPlatform.Attributes.Add("onchange", "PopulatePlatformTypes('" + ddlPlatform.ClientID + "','" + ddlPlatformType.ClientID + "','" + ddlPlatformModel.ClientID + "','" + ddlPlatformModelProperty.ClientID + "');ResetDropDownHidden('" + hdnModel.ClientID + "');");
                    ddlPlatformType.Attributes.Add("onchange", "PopulatePlatformModels('" + ddlPlatformType.ClientID + "','" + ddlPlatformModel.ClientID + "','" + ddlPlatformModelProperty.ClientID + "');ResetDropDownHidden('" + hdnModel.ClientID + "');");
                    ddlPlatformModel.Attributes.Add("onchange", "PopulatePlatformModelProperties('" + ddlPlatformModel.ClientID + "','" + ddlPlatformModelProperty.ClientID + "');ResetDropDownHidden('" + hdnModel.ClientID + "');");
                    ddlPlatformModelProperty.Attributes.Add("onchange", "UpdateDropDownHidden('" + ddlPlatformModelProperty.ClientID + "','" + hdnModel.ClientID + "');");

                    btnSaveProperties.Attributes.Add("onclick", "return ValidateDropDown('" + ddlClass.ClientID + "','Please make a selection for the environment')" +
                        " && ValidateDropDown('" + ddlOS.ClientID + "','Please make a selection for the operating system')" +
                        " && ValidateDropDown('" + ddlEnvironment.ClientID + "','Please make a selection for the server location')" +
                        " && ValidateDropDown('" + ddlLoadBalance.ClientID + "','Please make a selection for the network tier')" +
                        " && ValidateNumber('" + txtInstances.ClientID + "','Please enter a valid number for the number of instances')" +
                        " && ValidateNumber('" + txtQuorum.ClientID + "','Please enter a valid number for the quorum drive')" +
                        " && ValidateNumber('" + txtQuantity.ClientID + "','Please enter a valid number for the server count')" +
                        " && ValidateNumber('" + txtCores.ClientID + "','Please enter a valid number for the number of cores')" +
                        " && ValidateNumber('" + txtRam.ClientID + "','Please enter a valid number for the amount of RAM (in GB)')" +
                        " && ProcessButton(this) && LoadWait()" +
                        ";");
                    btnSaveComponents.Attributes.Add("onclick", "return ProcessButton(this) && LoadWait();");
                    btnSaveBackup.Attributes.Add("onclick", "return ProcessButton(this) && LoadWait();");
                    btnSaveMaintenance.Attributes.Add("onclick", "return ProcessButton(this) && LoadWait();");
                    btnSaveHardware.Attributes.Add("onclick", "return ProcessButton(this) && LoadWait();");
                    btnSaveHardware.Attributes.Add("onclick", "return ValidateDropDown('" + ddlPlatform.ClientID + "','Please make a selection for the model platform')" +
                        " && ValidateDropDown('" + ddlPlatformType.ClientID + "','Please make a selection for the model type')" +
                        " && ValidateDropDown('" + ddlPlatformModel.ClientID + "','Please make a selection for the model')" +
                        " && ValidateDropDown('" + ddlPlatformModelProperty.ClientID + "','Please make a selection for the model property')" +
                        " && ProcessButton(this) && LoadWait()" +
                        ";");
                    btnException.Attributes.Add("onclick", "return (ValidateText('" + txtExceptionID.ClientID + "','') == false || ValidateNumber('" + txtExceptionID.ClientID + "','Enter a valid number'))" +
                        " && ProcessButton(this) && LoadWait()" +
                        ";");
                }
            }
            btnApprove.Attributes.Add("onclick", "return true " + strMHS + " && confirm('Are you sure you want to APPROVE this request?') && ProcessButton(this) && LoadWait();");
            btnDeny.Attributes.Add("onclick", "return ValidateText('" + txtComments.ClientID + "','Please enter a reason for the rejection of this request') && confirm('Are you sure you want to DENY this request?') && ProcessButton(this) && LoadWait();");
        }
        protected void btnException_Click(Object Sender, EventArgs e)
        {
            oDesign.AddSubmitted(intID, Int32.Parse(btnException.CommandArgument), lblComments.Text, txtExceptionID.Text);
            Redirect("&menu_tab_approve2=1");
        }
        protected void btnApprove_Click(Object Sender, EventArgs e)
        {
            int intAnswer = oDesign.Approve(intID, Int32.Parse(lblUserID.Text), false, intEnvironment, intImplementorDistributedService, intWorkstationPlatform, intImplementorMidrangeService, dsnAsset, dsnIP, dsnServiceEditor, intAssignPage, intResourceRequestApprove, intViewPage);
            switch (lblType.Text)
            {
                case "F":
                    oDesign.UpdateApproverFieldWorkflow(Int32.Parse(lblTypeID.Text), intProfile, 0, txtComments.Text);
                    break;
                case "G":
                    oDesign.UpdateApproverGroupWorkflow(Int32.Parse(lblTypeID.Text), intProfile, 0, txtComments.Text);
                    break;
                case "S":
                    oDesign.UpdateSoftwareComponent(Int32.Parse(lblTypeID.Text), intProfile, 0, txtComments.Text, (radMHSYes.Checked ? 1 : 0));
                    if (radMHSYes.Checked)
                    {
                        Servers oServer = new Servers(0, dsn);
                        DataSet dsServer = oServer.GetAnswer(intAnswer);
                        foreach (DataRow drServer in dsServer.Tables[0].Rows)
                            oServer.UpdateMHS(Int32.Parse(drServer["id"].ToString()), 1);
                    }
                    break;
            }
            // Set to 100% if not already.
            if (oDesign.IsLocked(intID) == false)
            {
                // Not 100%
                DataSet dsOther = oDesign.GetResponsesOther(1, 0, 0, 1);
                foreach (DataRow drOther in dsOther.Tables[0].Rows)
                {
                    int intResponse = Int32.Parse(drOther["id"].ToString());
                    int intQuestion = 0;
                    if (Int32.TryParse(oDesign.GetResponse(intResponse, "questionid"), out intQuestion) == true)
                    {
                        string strValue = drOther["related_value"].ToString();
                        string strField = drOther["related_field"].ToString();
                        string strFieldQuestion = oDesign.GetQuestion(intQuestion, "related_field");
                        if (strField != "")
                        {
                            if (oDesign.Get(intID, strField) != strValue)
                                oDesign.AddValue(intID, 0, 0, intQuestion, strField, strValue);
                        }
                        else if (strFieldQuestion != "")
                        {
                            if (oDesign.Get(intID, strFieldQuestion) != strValue)
                                oDesign.AddValue(intID, 0, 0, intQuestion, strFieldQuestion, strValue);
                        }
                    }
                }
            }
            Notify("APPROVED", oUser.GetFullNameWithLanID(intProfile));
            Response.Redirect(oPage.GetFullLink(intPage) + "?id=" + intID.ToString() + "&action=done");
        }
        protected void btnDeny_Click(Object Sender, EventArgs e)
        {
            if (panException.Visible == true)
                oDesign.UpdateExceptionDone(intID, false);
            switch (lblType.Text)
            {
                case "F":
                    oDesign.UpdateApproverFieldWorkflow(Int32.Parse(lblTypeID.Text), intProfile, 1, txtComments.Text);
                    break;
                case "G":
                    oDesign.UpdateApproverGroupWorkflow(Int32.Parse(lblTypeID.Text), intProfile, 1, txtComments.Text);
                    break;
                case "S":
                    oDesign.UpdateSoftwareComponent(Int32.Parse(lblTypeID.Text), intProfile, 1, txtComments.Text, 0);
                    break;
            }
            oDesign.Unlock(intID);
            Notify("DENIED", oUser.GetFullNameWithLanID(intProfile));
            Response.Redirect(oPage.GetFullLink(intPage) + "?id=" + intID.ToString() + "&action=done");
        }
        protected void Notify(string strStatus, string strBy)
        {
            int intRequestor = 0;
            DataSet dsSubmitted = oDesign.GetSubmitted(intID);
            if (dsSubmitted.Tables[0].Rows.Count > 0)
                Int32.TryParse(dsSubmitted.Tables[0].Rows[0]["userid"].ToString(), out intRequestor);
            if (intRequestor > 0)
            {
                string strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DESIGN_BUILDER");
                oLog.AddEvent(intID, "", "", strStatus + " by " + strBy + " with comments = " + txtComments.Text, LoggingType.Debug);
                oFunction.SendEmail("Design Approval (" + intID.ToString() + ")", oUser.GetName(intRequestor), "", strEMailIdsBCC, "Design Approval (#" + intID.ToString() + ")", "<p><b>This message is to inform you that the following design has been " + strStatus + " by " + strBy + ".</b></p>" + (txtComments.Text == "" ? "" : "<p>The following comments were added:<br/>" + txtComments.Text + "</p>") + "<p>" + oDesign.GetSummary(intID, intEnvironment) + "</p><p><a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=/frame/design.aspx?id=" + intID.ToString() + "\" target=\"_blank\">Click here to review this design.</a></p>", true, false);
            }
        }
        protected void btnFinish_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(oPage.GetFullLinkRelated(intPage));
        }


        // Properties
        protected void btnSaveProperties_Click(Object Sender, EventArgs e)
        {
            string strLoadBalance = "";
            if (ddlLoadBalance.SelectedIndex > -1)
                strLoadBalance = ddlLoadBalance.SelectedItem.Value;
            int intDR = 0;
            Int32.TryParse(ddlDR.SelectedItem.Value, out intDR);
            int intEnv = 0;
            Int32.TryParse(Request.Form[hdnEnvironment.UniqueID], out intEnv);
            int intInstances = 0;
            Int32.TryParse(txtInstances.Text, out intInstances);
            int intQuorum = 0;
            Int32.TryParse(txtQuorum.Text, out intQuorum);
            int intQuantity = 0;
            Int32.TryParse(txtQuantity.Text, out intQuantity);
            int intCores = 0;
            Int32.TryParse(txtCores.Text, out intCores);
            int intRAM = 0;
            Int32.TryParse(txtRam.Text, out intRAM);
            int intApp = 0;
            int intSubApp = 0;
            if (ddlApplications.SelectedIndex > -1)
            {
                Int32.TryParse(ddlApplications.SelectedItem.Value, out intApp);
                Int32.TryParse(Request.Form[hdnSubApplication.UniqueID], out intSubApp);
            }
            oDesign.Update(intID, Int32.Parse(ddlClass.SelectedItem.Value), Int32.Parse(ddlOS.SelectedItem.Value), Int32.Parse(ddlSP.SelectedItem.Value), Int32.Parse(ddlDomain.SelectedItem.Value), ddlBackup.SelectedItem.Value, intDR, intEnv, (radHANone.Checked ? 0 : 1), (radHAClusterAA.Checked || radHAClusterAP.Checked ? 1 : 0), (radHALoadBalance.Checked ? 1 : 0), (radHAClusterAP.Checked ? 1 : (radHAClusterAA.Checked ? 2 : 0)), intInstances, intQuorum, (strLoadBalance == "M" ? 1 : 0), (strLoadBalance == "A" || strLoadBalance == "WA" ? 1 : 0), intQuantity, null, null, intApp, intSubApp, (intCores == 0 ? 1 : intCores), (intRAM == 0 ? 1 : intRAM));
            Redirect("&menu_tab_approve2=1");
        }

        // Accounts
        protected void btnAddAccount_Click(Object Sender, EventArgs e)
        {
            int intAccount = 0;
            if (Int32.TryParse(Request.Form[hdnAccount.UniqueID], out intAccount) == true)
            {
                if (oDesign.AddAccount(intID, intAccount, ddlPermission.SelectedItem.Value, (chkRemoteDesktop.Checked ? 1 : 0)) == true)
                    Redirect("&menu_tab_approve2=2");
                else
                    Redirect("&menu_tab_approve2=2&duplicate=" + oUser.GetName(intAccount));
            }
        }
        protected void btnDeleteAccount_Click(Object Sender, EventArgs e)
        {
            LinkButton oDelete = (LinkButton)Sender;
            oDesign.DeleteAccount(Int32.Parse(oDelete.CommandArgument));
            Redirect("&menu_tab_approve2=2");
        }

        // Software
        protected void btnSaveComponents_Click(Object Sender, EventArgs e)
        {
            if (Request.Form["hdnComponents"] != null)
            {
                string strComponents = Request.Form["hdnComponents"];
                if (strComponents != "")
                {
                    oDesign.DeleteSoftwareComponents(intID, false);
                    while (strComponents != "")
                    {
                        int intDetail = Int32.Parse(strComponents.Substring(0, strComponents.IndexOf("&")));
                        if (intDetail > 0)
                            oDesign.AddSoftwareComponent(intID, intDetail, 0);
                        strComponents = strComponents.Substring(strComponents.IndexOf("&") + 1);
                    }
                }
            }
            Redirect("&menu_tab_approve2=3");
        }

        // Storage
        private void LoadStorage(int _designid)
        {
            boolWindows = oDesign.IsWindows(_designid);
            DataSet dsStorage = oDesign.GetStorageDrives(_designid);
            DataSet dsDrives = oDesign.GetDrives();
            rptStorage.DataSource = dsStorage;
            rptStorage.DataBind();
            foreach (RepeaterItem ri in rptStorage.Items)
            {
                ImageButton _delete = (ImageButton)ri.FindControl("btnStorageDelete");
                _delete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this LUN?') && LoadWait();");
                ImageButton _save = (ImageButton)ri.FindControl("btnStorageSave");
                _save.Attributes.Add("onclick", "return LoadWait();");
                CheckBox _shared = (CheckBox)ri.FindControl("chkStorageSize");
                _shared.Checked = (_shared.Text == "1");
                _shared.Text = "";
                TextBox _size = (TextBox)ri.FindControl("txtStorageSize");
                TextBox _path = (TextBox)ri.FindControl("txtStoragePath");
                _size.Attributes.Add("onblur", "ChangeLUN('" + _size.Text + "','" + _path.Text + "','" + _size.ClientID + "','" + _path.ClientID + "');");
                _path.Attributes.Add("onblur", "ChangeLUN('" + _size.Text + "','" + _path.Text + "','" + _size.ClientID + "','" + _path.ClientID + "');");
                DropDownList _drive = (DropDownList)ri.FindControl("ddlStorageDrive");
                if (!IsPostBack || _drive.Items.Count == 0)
                {
                    _drive.DataValueField = "id";
                    _drive.DataTextField = "path";
                    _drive.DataSource = dsDrives;
                    _drive.DataBind();
                }
                _drive.SelectedValue = oDesign.GetStorageID(Int32.Parse(_save.CommandName), "driveid");
            }
            if (boolWindows)
            {
                trStorageApp.Visible = true;
                DataSet dsApp = oDesign.GetStorageDrive(_designid, -1000);
                if (dsApp.Tables[0].Rows.Count > 0)
                {
                    if (!IsPostBack || txtStorageSizeE.Text == "")
                    {
                        int intTemp = 0;
                        if (Int32.TryParse(dsApp.Tables[0].Rows[0]["size"].ToString(), out intTemp) == true)
                        {
                            txtStorageSizeE.Text = intTemp.ToString();
                            txtStorageSizeE.Attributes.Add("onblur", "ChangeLUN('" + txtStorageSizeE.Text + "','','" + txtStorageSizeE.ClientID + "',null);");
                        }
                    }
                    btnStorageSaveSizeE.CommandName = dsApp.Tables[0].Rows[0]["id"].ToString();
                }
                btnStorageSaveSizeE.Attributes.Add("onclick", "return LoadWait();");
            }
            btnStorageSaveNew.Attributes.Add("onclick", "return LoadWait();");
            btnStorageDeleteNew.Attributes.Add("onclick", "return LoadWait();");
            btnStorageDriveAdd.Attributes.Add("onclick", "return LoadWait();");
        }
        protected void btnStorageDriveAdd_Click(Object Sender, EventArgs e)
        {
            boolWindows = oDesign.IsWindows(intID);
            trStorageDrive.Visible = true;
            tdStorageDrive.Visible = boolWindows;
            txtStorageSizeNew.Text = "0";
            trStorageDriveNew.Visible = false;
            LoadTab1(2);
            LoadTab2(4);
        }
        protected void btnStorageSave_Click(Object Sender, ImageClickEventArgs e)
        {
            ImageButton oButton = (ImageButton)Sender;
            if (oButton.CommandArgument == "APP")
            {
                if (oButton.CommandName == "")
                    oDesign.AddLun(intID, -1000, "", txtStorageSizeE.Text, false);
                else
                    oDesign.UpdateLun(Int32.Parse(oButton.CommandName), -1000, "", txtStorageSizeE.Text, false);
            }
            else if (oButton.CommandArgument == "NEW")
            {
                int intDrive = 0;
                if (ddlStorageDriveNew.Items.Count > 0)
                    Int32.TryParse(ddlStorageDriveNew.SelectedItem.Value, out intDrive);
                if (oButton.CommandName == "")
                    oDesign.AddLun(intID, intDrive, txtStoragePathNew.Text, txtStorageSizeNew.Text, chkStorageSizeNew.Checked);
                else
                    oDesign.UpdateLun(Int32.Parse(oButton.CommandName), intDrive, txtStoragePathNew.Text, txtStorageSizeNew.Text, chkStorageSizeNew.Checked);
                trStorageDrive.Visible = false;
                trStorageDriveNew.Visible = true;
            }
            else if (oButton.CommandArgument == "SAVE")
            {
                foreach (RepeaterItem ri in rptStorage.Items)
                {
                    ImageButton oSave = (ImageButton)ri.FindControl("btnStorageSave");
                    if (oButton.CommandName == oSave.CommandName)
                    {
                        DropDownList oDrive = (DropDownList)ri.FindControl("ddlStorageDrive");
                        int intDrive = 0;
                        if (oDrive.Items.Count > 0)
                            Int32.TryParse(oDrive.SelectedItem.Value, out intDrive);
                        TextBox oPath = (TextBox)ri.FindControl("txtStoragePath");
                        TextBox oSize = (TextBox)ri.FindControl("txtStorageSize");
                        CheckBox oShared = (CheckBox)ri.FindControl("chkStorageSize");
                        oDesign.UpdateLun(Int32.Parse(oButton.CommandName), intDrive, oPath.Text, oSize.Text, oShared.Checked);
                        break;
                    }
                }
            }
            else if (oButton.CommandArgument == "DELETE")
            {
                oDesign.DeleteLun(Int32.Parse(oButton.CommandName));
            }
            else
            {
                // CANCEL the insert
                trStorageDrive.Visible = false;
                trStorageDriveNew.Visible = true;
            }
            // Refresh the storage repeater
            LoadStorage(intID);
            LoadTab1(2);
            LoadTab2(4);
        }

        // Backup
        protected void btnSaveBackup_Click(Object Sender, EventArgs e)
        {
            oDesign.AddBackup(intID, Request);
            Redirect("&menu_tab_approve2=5");
        }

        // Backup Exclusions
        protected void btnAddExclusion_Click(Object Sender, EventArgs e)
        {
            oDesign.AddExclusion(intID, txtPath.Text);
            Redirect("&menu_tab_approve2=6");
        }
        protected void btnDeleteExclusion_Click(Object Sender, EventArgs e)
        {
            LinkButton oDelete = (LinkButton)Sender;
            oDesign.DeleteExclusions(Int32.Parse(oDelete.CommandArgument));
            Redirect("&menu_tab_approve2=6");
        }

        // Maintenance
        protected void btnSaveMaintenance_Click(Object Sender, EventArgs e)
        {
            oDesign.AddMaintenance(intID, Request);
            Redirect("&menu_tab_approve2=7");
        }

        // Hardware
        protected void btnSaveHardware_Click(Object Sender, EventArgs e)
        {
            oDesign.Update(intID, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, Int32.Parse(Request.Form[hdnLocation.UniqueID]), Int32.Parse(Request.Form[hdnModel.UniqueID]), null, null, null, null);
            Redirect("&menu_tab_approve2=8");
        }
        
        private void LoadTab1(int _menu_tab)
        {
            Tab oTab = new Tab("", _menu_tab, "divMenuApprove1", true, false);
            oTab.AddTab("Design Summary", "");
            oTab.AddTab("Exception Configuration", "");
            strMenuTabApprove1 = oTab.GetTabs();
        }
        private void LoadTab2(int _menu_tab)
        {
            Tab oTab2 = new Tab("", _menu_tab, "divMenuApprove2", true, false);
            oTab2.AddTab("Properties", "");
            oTab2.AddTab("Accounts", "");
            oTab2.AddTab("Software", "");
            oTab2.AddTab("Storage", "");
            oTab2.AddTab("Backup Window", "");
            oTab2.AddTab("Backup Exclusion(s)", "");
            oTab2.AddTab("Maintenance Window", "");
            oTab2.AddTab("Hardware", "");
            strMenuTabApprove2 = oTab2.GetTabs();
        }
        private void Redirect(string _additional)
        {
            if (intPage > 0)
                Response.Redirect(oPage.GetFullLink(intPage) + "?id=" + intID.ToString() + "&menu_tab_approve=2" + _additional);
            else
                Response.Redirect(Request.Path + "?id=" + Request.QueryString["id"] + "&menu_tab_approve=2" + _additional);
        }
    }
}