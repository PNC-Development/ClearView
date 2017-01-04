<%@ Page Language="C#" MasterPageFile="~/window.Master" AutoEventWireup="true" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" runat="server">
<script runat="server">
    private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
    private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
    private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
    protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
    protected double dblCompressionPercentage = double.Parse(ConfigurationManager.AppSettings["SQL2008_COMPRESSION_PERCENTAGE"]);
    protected double dblTempDBOverhead = double.Parse(ConfigurationManager.AppSettings["SQL2008_TEMPDB_OVERHEAD"]);
    protected int intForecastPage = Int32.Parse(ConfigurationManager.AppSettings["ForecastEdit"]);
    protected int intProfile;
    protected Forecast oForecast;
    protected Classes oClass;
    protected Pages oPage;
    protected Mnemonic oMnemonic;
    protected CostCenter oCostCenter;
    protected Confidence oConfidence;
    protected Users oUser;
    protected Functions oFunction;
    protected Variables oVariable;
    protected Holidays oHoliday;

    protected int intForecast;
    protected int intID;
    protected string strSpacer = "45";
    protected string strLock = "/images/docshare.gif";
    //protected string strApproval = "/images/hold.gif";
    protected string strApproval = "/images/required.gif";
    protected string strHighlight = "#FFEE99";
    protected string strAppend = "";
    protected string strGridBackup = "";
    protected string strGridBackupSun = "";
    protected string strGridBackupMon = "";
    protected string strGridBackupTue = "";
    protected string strGridBackupWed = "";
    protected string strGridBackupThu = "";
    protected string strGridBackupFri = "";
    protected string strGridBackupSat = "";
    private int intGridBackups = 0;
    protected string strGridMaintenance = "";
    protected string strGridMaintenanceSun = "";
    protected string strGridMaintenanceMon = "";
    protected string strGridMaintenanceTue = "";
    protected string strGridMaintenanceWed = "";
    protected string strGridMaintenanceThu = "";
    protected string strGridMaintenanceFri = "";
    protected string strGridMaintenanceSat = "";
    private int intGridMaintenances = 0;
    private double dblSLA = 10.00;
    private string strValidation = "";
    protected bool boolWindows = false;
    protected bool boolDatabase = false;
    protected bool boolSQL = false;
    protected bool boolCluster = false;
    protected bool boolLoadBalance = false;
    protected bool boolReview = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
        oForecast = new Forecast(intProfile, dsn);
        oClass = new Classes(intProfile, dsn);
        oPage = new Pages(intProfile, dsn);
        oMnemonic = new Mnemonic(intProfile, dsn);
        oCostCenter = new CostCenter(intProfile, dsn);
        oConfidence = new Confidence(intProfile, dsn);
        oUser = new Users(intProfile, dsn);
        oFunction = new Functions(intProfile, dsn, intEnvironment);
        oHoliday = new Holidays(intProfile, dsn);
        oVariable = new Variables(intEnvironment);

        if (Request.QueryString["parent"] != null)
            Int32.TryParse(Request.QueryString["parent"], out intForecast);
        if (Request.QueryString["id"] != null)
            Int32.TryParse(Request.QueryString["id"], out intID);
        Page.Title = "ClearView Design Builder";
        
        if (!IsPostBack)
        {
            if (intID == 0)
                LoadPanel("2.1.5");
            else
            {
                LoadPanel(Get(intID, "step"));
                Int32.TryParse(Get(intID, "forecastid"), out intForecast);
                lblID.Text = "&nbsp;(#" + intID.ToString() + ")";
            }

            txtMnemonic.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'300','195','" + divMnemonic.ClientID + "','" + lstMnemonic.ClientID + "','" + hdnMnemonic.ClientID + "','" + oVariable.URL() + "/frame/ajax/ajax_mnemonics.aspx',2);");
            lstMnemonic.Attributes.Add("ondblclick", "AJAXClickRow();");
            txtCostCenter.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'200','195','" + divCostCenter.ClientID + "','" + lstCostCenter.ClientID + "','" + hdnCostCenter.ClientID + "','" + oVariable.URL() + "/frame/ajax/ajax_cost_centers.aspx',5);");
            lstCostCenter.Attributes.Add("ondblclick", "AJAXClickRow();");
            txtSI.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'300','195','" + divSI.ClientID + "','" + lstSI.ClientID + "','" + hdnSI.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
            lstSI.Attributes.Add("ondblclick", "AJAXClickRow();");
            txtUser.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'300','195','" + divAJAX.ClientID + "','" + lstAJAX.ClientID + "','" + hdnUser.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2,null,'PNC');");
            lstAJAX.Attributes.Add("ondblclick", "AJAXClickRow();");
            
            if (Request.QueryString["saved"] != null)
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "saved", "<script type=\"text/javascript\">if(window.opener!=null)window.opener.navigate('" + oPage.GetFullLink(intForecastPage) + "?id=" + intForecast.ToString() + "');<" + "/" + "script>");
            if (Request.QueryString["duplicate"] != null && Request.QueryString["duplicate"] != "")
                Page.ClientScript.RegisterStartupScript(typeof(Page), "duplicate", "<script type=\"text/javascript\">alert('The account " + Request.QueryString["duplicate"] + " has already been configured');<" + "/" + "script>");
            if (Request.QueryString["conflict"] != null && Request.QueryString["conflict"] != "")
                Page.ClientScript.RegisterStartupScript(typeof(Page), "conflict", "<script type=\"text/javascript\">alert('The account " + Request.QueryString["duplicate"] + " has already been configured');<" + "/" + "script>");

            btnNext.Attributes.Add("onclick", "return ProcessButton(this);");
            btnBack.Attributes.Add("onclick", "return ProcessButton(this);");
            btnUpdate.Attributes.Add("onclick", "return ProcessButton(this);");
            btnSubmit.Attributes.Add("onclick", "return ProcessButton(this);");
            btnHelpAccounts.Attributes.Add("onclick", "ShowHideDiv2('" + hlpAccounts.ClientID + "');return false;");
            //btnHelpAccounts.Attributes.Add("onclick", "return OpenWindow('DOCUMENTS_SECURE','?pid=" + intProject.ToString() + "');");
            imgCalendar.Attributes.Add("onclick", "return ShowCalendar('" + txtDate.ClientID + "');");
            btnAddAccount.Attributes.Add("onclick", "return ValidateHidden('" + hdnUser.ClientID + "','" + txtUser.ClientID + "','Enter a username, first name or last name') && ValidateDropDown('" + ddlPermission.ClientID + "','Select a permission') && ProcessButton(this);");
        }
    }
    private void LoadLists(int _classid)
    {
        ddlClass.DataTextField = "name";
        ddlClass.DataValueField = "id";
        ddlClass.DataSource = oClass.GetForecasts(1);
        ddlClass.DataBind();
        ddlClass.Items.Insert(0, new ListItem("-- SELECT --", "0"));
        if (_classid > 0)
        {
            ddlClass.SelectedValue = _classid.ToString();
            ddlLocation.DataTextField = "name";
            ddlLocation.DataValueField = "id";
            ddlLocation.DataSource = oClass.GetEnvironment(_classid, 1);
            ddlLocation.DataBind();
            ddlLocation.Items.Insert(0, new ListItem("-- SELECT --", "0"));
        }

        ddlConfidence.DataValueField = "id";
        ddlConfidence.DataTextField = "name";
        ddlConfidence.DataSource = oConfidence.Gets(1);
        ddlConfidence.DataBind();

        ddlStorageDrive.DataValueField = "id";
        ddlStorageDrive.DataTextField = "path";
        ddlStorageDrive.DataSource = GetDrives();
        ddlStorageDrive.DataBind();
    }
    private void Save()
    {
        int intLocation = 0;
        if (ddlLocation.SelectedIndex > -1)
            Int32.TryParse(ddlLocation.SelectedItem.Value, out intLocation);

        SqlParameter[] arParams;
        switch (lblProcedureNumber.Text)
        {
            case "2.1.5":
                int intMnemonic = 0;
                Int32.TryParse(Request.Form[hdnMnemonic.UniqueID], out intMnemonic);
                int intCost = 0;
                Int32.TryParse(Request.Form[hdnCostCenter.UniqueID], out intCost);
                int intSI = 0;
                Int32.TryParse(Request.Form[hdnSI.UniqueID], out intSI);
                if (intID == 0)
                {
                    arParams = new SqlParameter[5];
                    arParams[0] = new SqlParameter("@forecastid", intForecast);
                    arParams[1] = new SqlParameter("@mnemonicid", intMnemonic);
                    arParams[2] = new SqlParameter("@costid", intCost);
                    arParams[3] = new SqlParameter("@si", intSI);
                    arParams[4] = new SqlParameter("@id", SqlDbType.Int);
                    arParams[4].Direction = ParameterDirection.Output;
                    SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addAccenture", arParams);
                    intID = Int32.Parse(arParams[4].Value.ToString());
                }
                else
                {
                    arParams = new SqlParameter[4];
                    arParams[0] = new SqlParameter("@id", intID);
                    arParams[1] = new SqlParameter("@mnemonicid", intMnemonic);
                    arParams[2] = new SqlParameter("@costid", intCost);
                    arParams[3] = new SqlParameter("@si", intSI);
                    SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateAccenture", arParams);
                }
                break;
            case "2.1.7":
                int intClass = 0;
                Int32.TryParse(ddlClass.SelectedItem.Value, out intClass);
                arParams = new SqlParameter[2];
                arParams[0] = new SqlParameter("@id", intID);
                arParams[1] = new SqlParameter("@classid", intClass);
                SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateAccenture", arParams);
                break;
            case "2.1.8":
                arParams = new SqlParameter[6];
                arParams[0] = new SqlParameter("@id", intID);
                arParams[1] = new SqlParameter("@database", (chkDatabase.Checked ? 1 : 0));
                arParams[2] = new SqlParameter("@web", (chkWeb.Checked ? 1 : 0));
                arParams[3] = new SqlParameter("@oracle", (radOracle.Checked ? 1 : 0));
                arParams[4] = new SqlParameter("@sql", (radSQL.Checked ? 1 : 0));
                arParams[5] = new SqlParameter("@other_db", (radOther.Checked ? 1 : 0));
                SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateAccenture", arParams);
                break;
            case "2.1.10":
                arParams = new SqlParameter[2];
                arParams[0] = new SqlParameter("@id", intID);
                arParams[1] = new SqlParameter("@os", ddlOS.SelectedItem.Value);
                SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateAccenture", arParams);
                break;
            case "2.1.11":
                arParams = new SqlParameter[2];
                arParams[0] = new SqlParameter("@id", intID);
                arParams[1] = new SqlParameter("@size", ddlSize.SelectedItem.Value);
                SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateAccenture", arParams);
                break;
            case "2.1.12":
                arParams = new SqlParameter[3];
                arParams[0] = new SqlParameter("@id", intID);
                arParams[1] = new SqlParameter("@backup_frequency", (radBackupDaily.Checked ? "D" : (radBackupWeekly.Checked ? "W" : (radBackupMonthly.Checked ? "M" : ""))));
                //arParams[2] = new SqlParameter("@backup_window", (radBackupDay.Checked ? "D" : (radBackupNight.Checked ? "N" : "")));
                SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateAccenture", arParams);
                // Schedule
                arParams = new SqlParameter[8];
                arParams[0] = new SqlParameter("@designid", intID);
                arParams[1] = new SqlParameter("@sun", GetGrid("hdnBackup", 1));
                arParams[2] = new SqlParameter("@mon", GetGrid("hdnBackup", 2));
                arParams[3] = new SqlParameter("@tue", GetGrid("hdnBackup", 3));
                arParams[4] = new SqlParameter("@wed", GetGrid("hdnBackup", 4));
                arParams[5] = new SqlParameter("@thu", GetGrid("hdnBackup", 5));
                arParams[6] = new SqlParameter("@fri", GetGrid("hdnBackup", 6));
                arParams[7] = new SqlParameter("@sat", GetGrid("hdnBackup", 7));
                SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateAccentureBackup", arParams);
                break;
            case "2.1.14":
                arParams = new SqlParameter[8];
                arParams[0] = new SqlParameter("@designid", intID);
                arParams[1] = new SqlParameter("@sun", GetGrid("hdnMaintenance", 1));
                arParams[2] = new SqlParameter("@mon", GetGrid("hdnMaintenance", 2));
                arParams[3] = new SqlParameter("@tue", GetGrid("hdnMaintenance", 3));
                arParams[4] = new SqlParameter("@wed", GetGrid("hdnMaintenance", 4));
                arParams[5] = new SqlParameter("@thu", GetGrid("hdnMaintenance", 5));
                arParams[6] = new SqlParameter("@fri", GetGrid("hdnMaintenance", 6));
                arParams[7] = new SqlParameter("@sat", GetGrid("hdnMaintenance", 7));
                SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateAccentureMaintenance", arParams);
                break;
            case "2.1.15":
                arParams = new SqlParameter[2];
                arParams[0] = new SqlParameter("@id", intID);
                arParams[1] = new SqlParameter("@mainframe", (radMainframe_Yes.Checked ? 1 : 0));
                SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateAccenture", arParams);
                break;
            case "2.1.16":
                arParams = new SqlParameter[2];
                arParams[0] = new SqlParameter("@id", intID);
                arParams[1] = new SqlParameter("@environmentid", intLocation);
                SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateAccenture", arParams);
                break;
            case "2.1.17":
                int intInstances = 0;
                Int32.TryParse(txtInstances.Text, out intInstances);
                int intNodes = 0;
                Int32.TryParse(txtNodes.Text, out intNodes);
                int intQuorum = 0;
                Int32.TryParse(txtQuorum.Text, out intQuorum);
                arParams = new SqlParameter[9];
                arParams[0] = new SqlParameter("@id", intID);
                arParams[1] = new SqlParameter("@ha", (radHA_Yes.Checked ? 1 : 0));
                arParams[2] = new SqlParameter("@ha_type", (radHA.SelectedIndex > -1 ? radHA.SelectedItem.Value : ""));
                arParams[3] = new SqlParameter("@cluster_type", (radCluster.SelectedIndex > -1 ? radCluster.SelectedItem.Value : ""));
                arParams[4] = new SqlParameter("@instances", intInstances);
                arParams[5] = new SqlParameter("@nodes", intNodes);
                arParams[6] = new SqlParameter("@quorum", intQuorum);
                arParams[7] = new SqlParameter("@middleware", (radMiddlewareYes.Checked ? 1 : 0));
                arParams[8] = new SqlParameter("@application", (radApplicationYes.Checked ? 1 : 0));
                SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateAccenture", arParams);
                break;
            case "2.1.20":
                StringBuilder strSpecial = new StringBuilder();
                foreach (ListItem li in chkSpecial.Items)
                {
                    if (li.Selected == true)
                    {
                        strSpecial.Append(li.Value);
                        strSpecial.Append(";");
                    }
                }
                arParams = new SqlParameter[2];
                arParams[0] = new SqlParameter("@id", intID);
                arParams[1] = new SqlParameter("@special", strSpecial.ToString());
                SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateAccenture", arParams);
                break;
            case "2.1.21":
                arParams = new SqlParameter[4];
                arParams[0] = new SqlParameter("@id", intID);
                arParams[1] = new SqlParameter("@software", 1);
                arParams[2] = new SqlParameter("@ndm", (chkNDM.Checked ? 1 : 0));
                arParams[3] = new SqlParameter("@ca7", (chkCA7.Checked ? 1 : 0));
                SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateAccenture", arParams);
                break;
            case "2.1.23":
                int intStorage = 0;
                Int32.TryParse(txtStorage.Text, out intStorage);
                arParams = new SqlParameter[4];
                arParams[0] = new SqlParameter("@id", intID);
                arParams[1] = new SqlParameter("@storage", (radStorageYes.Checked ? 1 : 0));
                arParams[2] = new SqlParameter("@persistent", (radPersistent.Checked ? 1 : 0));
                arParams[3] = new SqlParameter("@non_persistent", intStorage);
                SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateAccenture", arParams);
                break;
            case "2.1.24":
                int intCount = 0;
                Int32.TryParse(ddlCount.SelectedItem.Value, out intCount);
                arParams = new SqlParameter[2];
                arParams[0] = new SqlParameter("@id", intID);
                arParams[1] = new SqlParameter("@quantity", intCount);
                SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateAccenture", arParams);
                break;
            case "2.1.25":
                arParams = new SqlParameter[3];
                arParams[0] = new SqlParameter("@id", intID);
                arParams[1] = new SqlParameter("@accounts", 1);
                arParams[2] = new SqlParameter("@account_results", (chkNotify.Checked ? 1 : 0));
                SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateAccenture", arParams);
                break;
            case "2.1.26":
                arParams = new SqlParameter[3];
                arParams[0] = new SqlParameter("@id", intID);
                arParams[1] = new SqlParameter("@setCommitment", "1");
                arParams[2] = new SqlParameter("@commitment", (txtDate.Text == "" ? SqlDateTime.Null : DateTime.Parse(txtDate.Text)));
                SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateAccenture", arParams);
                break;
            case "2.1.27":
                int intConfidence = 0;
                Int32.TryParse(ddlConfidence.SelectedItem.Value, out intConfidence);
                arParams = new SqlParameter[2];
                arParams[0] = new SqlParameter("@id", intID);
                arParams[1] = new SqlParameter("@confidenceid", intConfidence);
                SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateAccenture", arParams);
                break;
        }
        
        // Set global values
        if (radSQL.Checked)
        {
            // Set to Windows
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", intID);
            arParams[1] = new SqlParameter("@os", "W");
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateAccenture", arParams);
        }
        if (ddlLocation.Items.Count == 2)
        {
            ddlLocation.SelectedIndex = 1;
            Int32.TryParse(ddlLocation.SelectedItem.Value, out intLocation);
            arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@id", intID);
            arParams[1] = new SqlParameter("@environmentid", intLocation);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateAccenture", arParams);
        }
    }
    private void Navigate(bool boolBackwards)
    {
        switch (lblProcedureNumber.Text)
        {
            case "2.1.5":
                if (boolBackwards)
                { // Do nothing
                }
                else
                    UpdateStep("2.1.7");
                break;
            case "2.1.7":
                UpdateStep(boolBackwards ? "2.1.5" : "2.1.8");
                break;
            case "2.1.8":
                if (boolBackwards)
                    UpdateStep("2.1.7");
                else
                {
                    if (radSQL.Checked == true)
                        UpdateStep("2.1.11");
                    else
                        UpdateStep("2.1.10");
                }
                break;
            case "2.1.10":
                UpdateStep(boolBackwards ? "2.1.8" : "2.1.11");
                break;
            case "2.1.11":
                if (boolBackwards)
                {
                    if (radSQL.Checked == true)
                        UpdateStep("2.1.8");
                    else
                        UpdateStep("2.1.10");
                }
                else
                    UpdateStep("2.1.12");
                break;
            case "2.1.12":
                if (boolBackwards)
                    UpdateStep("2.1.11");
                else
                {
                    if (chkDatabase.Checked)
                        UpdateStep("2.1.14");
                    else
                        UpdateStep("2.1.13");
                }
                break;
            case "2.1.13":
                UpdateStep(boolBackwards ? "2.1.12" : "2.1.14");
                break;
            case "2.1.14":
                if (boolBackwards)
                {
                    if (chkDatabase.Checked)
                        UpdateStep("2.1.12");
                    else
                        UpdateStep("2.1.13");
                }
                else
                    UpdateStep("2.1.15");
                break;
            case "2.1.15":
                if (boolBackwards)
                    UpdateStep("2.1.14");
                else
                {
                    if (lblEnvironment.Text.ToUpper().Contains("DEV") == true)
                        UpdateStep("2.1.17");
                    else
                        UpdateStep("2.1.16");
                }
                break;
            case "2.1.16":
                UpdateStep(boolBackwards ? "2.1.15" : "2.1.17");
                break;
            case "2.1.17":
                if (boolBackwards)
                {
                    if (lblEnvironment.Text.ToUpper().Contains("DEV") == true)
                        UpdateStep("2.1.15");
                    else
                        UpdateStep("2.1.16");
                }
                else
                    UpdateStep("2.1.20");
                // Fix NEXT (add screens)
                break;
            case "2.1.20":
                UpdateStep(boolBackwards ? "2.1.17" : "2.1.21");
                break;
            case "2.1.21":
                UpdateStep(boolBackwards ? "2.1.20" : "2.1.23");
                break;
            case "2.1.23":
                UpdateStep(boolBackwards ? "2.1.21" : "2.1.24");
                break;
            case "2.1.24":
                UpdateStep(boolBackwards ? "2.1.23" : "2.1.25");
                break;
            case "2.1.25":
                UpdateStep(boolBackwards ? "2.1.24" : "2.1.26");
                break;
            case "2.1.26":
                UpdateStep(boolBackwards ? "2.1.25" : "2.1.27");
                break;
            case "2.1.27":
                UpdateStep(boolBackwards ? "2.1.26" : "2.2");
                break;
            case "2.3":
                if (boolBackwards)
                    UpdateStep("2.2");
                break;
        }
    }
    private void UpdateStep(string strStep)
    {
        // First Save all the information.
        SqlParameter[] arParams = new SqlParameter[2];
        arParams[0] = new SqlParameter("@id", intID);
        arParams[1] = new SqlParameter("@step", strStep);
        SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateAccenture", arParams);
        Response.Redirect(Request.Path + "?id=" + intID.ToString() + "&saved=true");
    }
    private void LoadPanel(string strStep)
    {
        if (strStep != "")
        {
            lblSubHeader.Text = "Welcome to Design Builder! Complete the following questions...";
            LoadInformation();
            panRequirements.Visible = true;

            // Individual Changes
            switch (strStep)
            {
                case "2.1.5":
                    LoadStep(trMnemonic, imgMnemonic, lnkMnemonic, lblMnemonic);
                    panMnemonic.Visible = true;
                    btnBack.Enabled = false;
                    break;
                case "2.1.7":
                    LoadStep(trEnvironment, imgEnvironment, lnkEnvironment, lblEnvironment);
                    panEnvironment.Visible = true;
                    break;
                case "2.1.8":
                    LoadStep(trServerType, imgServerType, lnkServerType, lblServerType);
                    panServerType.Visible = true;
                    break;
                case "2.1.10":
                    LoadStep(trOS, imgOS, lnkOS, lblOS);
                    panOS.Visible = true;
                    break;
                case "2.1.11":
                    LoadStep(trServerSize, imgServerSize, lnkServerSize, lblServerSize);
                    panServerSize.Visible = true;
                    break;
                case "2.1.12":
                    LoadStep(trBackup, imgBackup, lnkBackup, lblBackup);
                    panBackup.Visible = true;
                    break;
                case "2.1.13":
                    LoadStep(trExclusions, imgExclusions, lnkExclusions, lblExclusions);
                    panExclusions.Visible = true;
                    rptExclusions.DataSource = GetExclusions();
                    rptExclusions.DataBind();
                    foreach (RepeaterItem ri in rptExclusions.Items)
                    {
                        LinkButton _delete = (LinkButton)ri.FindControl("btnDeleteExclusion");
                        _delete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this exclusion?');");
                    }
                    if (rptExclusions.Items.Count == 0)
                        lblExclusion.Visible = true;
                    break;
                case "2.1.14":
                    LoadStep(trMaintenance, imgMaintenance, lnkMaintenance, lblMaintenance);
                    panMaintenance.Visible = true;
                    break;
                case "2.1.15":
                    LoadStep(trMainframe, imgMainframe, lnkMainframe, lblMainframe);
                    panMainframe.Visible = true;
                    break;
                case "2.1.16":
                    LoadStep(trLocation, imgLocation, lnkLocation, lblLocation);
                    panLocation.Visible = true;
                    break;
                case "2.1.17":
                    LoadStep(trHA, imgHA, lnkHA, lblHA);
                    panHA.Visible = true;
                    break;
                case "2.1.20":
                    LoadStep(trSpecial, imgSpecial, lnkSpecial, lblSpecial);
                    panSpecial.Visible = true;
                    break;
                case "2.1.21":
                    LoadStep(trSoftware, imgSoftware, lnkSoftware, lblSoftware);
                    panSoftware.Visible = true;
                    break;
                case "2.1.23":
                    LoadStep(trStorage, imgStorage, lnkStorage, lblStorage);
                    panStorage.Visible = true;
                    break;
                case "2.1.24":
                    LoadStep(trCount, imgCount, lnkCount, lblCount);
                    panCount.Visible = true;
                    break;
                case "2.1.25":
                    LoadStep(trAccounts, imgAccounts, lnkAccounts, lblAccounts);
                    panAccounts.Visible = true;
                    panHelpAccounts.Visible = true;
                    rptAccounts.DataSource = GetAccounts();
                    rptAccounts.DataBind();
                    foreach (RepeaterItem ri in rptAccounts.Items)
                    {
                        LinkButton _delete = (LinkButton)ri.FindControl("btnDeleteAccount");
                        _delete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this account?');");
                        string strPermissions = "";
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
                    break;
                case "2.1.26":
                    LoadStep(trDate, imgDate, lnkDate, lblDate);
                    panDate.Visible = true;
                    break;
                case "2.1.27":
                    LoadStep(trConfidence, imgConfidence, lnkConfidence, lblConfidence);
                    panConfidence.Visible = true;
                    break;
                case "2.2":
                    panRequirements.Visible = false;
                    panSummary.Visible = true;
                    btnNext.Visible = false;
                    btnBack.Visible = false;
                    btnUpdate.Visible = false; 
                    break;
                case "2.3":
                    btnNext.Enabled = false;
                    break;
            }
            lblProcedureNumber.Text = strStep;
        }
    }
    private void LoadInformation()
    {
        bool boolValid = true;
        DataSet ds = Get(intID);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            string strDataCenter = "";
            string strDataCenterWhy = "";
            
            // Load Mnemonic
            int intMnemonic = 0;
            Int32.TryParse(dr["mnemonicid"].ToString(), out intMnemonic);
            lblValMnemonic.CssClass = "default";
            if (intMnemonic > 0)
                txtMnemonic.Text = oMnemonic.Get(intMnemonic, "factory_code") + " - " + oMnemonic.Get(intMnemonic, "name");
            else
            {
                lblValMnemonic.CssClass = "reddefault";
                boolValid = false;
            }
            hdnMnemonic.Value = intMnemonic.ToString();
            lblMnemonic.Text = oMnemonic.Get(intMnemonic, "factory_code");
            int intCost = 0;
            Int32.TryParse(dr["costid"].ToString(), out intCost);
            if (intCost > 0)
                txtCostCenter.Text = oCostCenter.Get(intCost, "name");
            hdnCostCenter.Value = intCost.ToString();
            int intSI = 0;
            Int32.TryParse(dr["si"].ToString(), out intSI);
            if (intSI > 0)
                txtSI.Text = oUser.GetFullName(intSI) + " (" + oUser.GetName(intSI) + ")";
            hdnSI.Value = intSI.ToString();

            // Load DR
            string strResRating = "";
            if (intMnemonic > 0)
                strResRating = oMnemonic.Get(intMnemonic, "ResRating");
            lblRTO.Text = strResRating;
            lblSummaryMnemonic.Text = oMnemonic.Get(intMnemonic, "name") + " (" + lblMnemonic.Text + "), RTO: " + strResRating;
            if (strResRating.ToUpper().Contains("HOURS") == true)
                strResRating = strResRating.ToUpper().Replace("HOURS", "");
            strResRating = strResRating.Trim();
            int intResRating = 0;
            Int32.TryParse(strResRating, out intResRating);
            if (intResRating < 48)
            {
                radBackupDaily.Checked = true;
                lblBackup.Text = "Daily";
                radBackupWeekly.Enabled = false;
                radBackupMonthly.Enabled = false;
            }
            else
            {
                radBackupDaily.Enabled = false;
            }
            radBackupDaily.ToolTip = intResRating.ToString() + " Hours";

            // Load Class
            int intClass = 0;
            Int32.TryParse(dr["classid"].ToString(), out intClass);
            LoadLists(intClass);
            
            string strClass = oClass.Get(intClass, "name");
            bool boolDev = strClass.ToUpper().Contains("DEV");
            bool boolTest = strClass.ToUpper().Contains("TEST");
            bool boolQA = strClass.ToUpper().Contains("QA");
            bool boolProd = strClass.ToUpper().Contains("PROD");

            if (boolDev || boolTest)
            {
                strDataCenter = "Dalton Data Center";
                strDataCenterWhy = "All DEVELOPMENT and TEST servers will be provisioned in the Dalton Data Center";
            }

            if (lblMnemonic.Text != "")
                imgMnemonic.ImageUrl = "/images/check.gif";

            lblEnvironment.Text = strClass;
            if (strClass != "")
            {
                lblEnvironment.Text = strClass;
                imgEnvironment.ImageUrl = "/images/check.gif";
            }

            // Load Server Type
            boolDatabase = (dr["database"].ToString() == "1");
            chkDatabase.Checked = boolDatabase;
            panDatabase.Visible = boolDatabase;
            if (boolDatabase == true)
            {
                lnkExclusions.Enabled = false;
                imgExclusions.ImageUrl = strLock;
                lblExclusions.Text = "<i>N / A</i>";
                imgExclusions.ToolTip = "Database servers do not permit backup exclusions";
            }
            else
            {
                DataSet dsExclusions = GetExclusions();
                int intExclusions = dsExclusions.Tables[0].Rows.Count;
                lblExclusions.Text = intExclusions.ToString();
            }
            lblSummaryExclusion.Text = lblExclusions.Text + " Exclusion(s)";
            bool boolOracle = (dr["oracle"].ToString() == "1");
            radOracle.Checked = boolOracle;
            boolSQL = (dr["sql"].ToString() == "1");
            radSQL.Checked = boolSQL;
            bool boolOtherDB = (dr["other_db"].ToString() == "1");
            radOther.Checked = boolOtherDB;
            bool boolWeb = (dr["web"].ToString() == "1");
            chkWeb.Checked = boolWeb;
            boolWindows = (dr["os"].ToString() == "W");
            bool boolLinux = (dr["os"].ToString() == "L");
            bool boolSolaris = (dr["os"].ToString() == "S");
            bool boolAIX = (dr["os"].ToString() == "A");
            bool boolOtherOS = (dr["os"].ToString() == "O");

            string strDatabase = "";
            imgServerType.ImageUrl = "/images/check.gif";

            if (boolDatabase == false)
            {
                if (boolWeb)
                    lblSummaryServerType.Text = "Web";
                else
                    lblSummaryServerType.Text = "Application";
            }
            else
            {
                lblSummaryServerType.Text = "Database";
                if (boolSQL || boolOracle || boolOtherDB)
                {
                    if (boolSQL == true)
                    {
                        strDatabase = "SQL";
                        boolWindows = true;
                        lnkOS.Enabled = false;
                        imgOS.ImageUrl = strLock;
                        imgOS.ToolTip = "SQL Requires Microsoft Windows";
                    }
                    if (boolOracle == true)
                        strDatabase = "Oracle";
                    if (boolOtherDB == true)
                    {
                        strDatabase = "Other";
                        LoadApproval(imgServerType, lblServerType, "");
                    }
                    lblSummaryServerType.Text += " (" + strDatabase + ")";
                }
                if (boolWeb)
                    lblSummaryServerType.Text += " + Web";
            }
            lblServerType.Text = (boolDatabase ? (boolWeb ? strDatabase + " Database + Web" : strDatabase + " Database") : (boolWeb ? "Web" : "Application"));



            // Size
            ddlSize.Items.Clear();
            if (boolWindows || boolLinux || boolSolaris || boolAIX || boolOtherOS)
            {
                if (imgOS.ImageUrl != strLock)
                    imgOS.ImageUrl = "/images/check.gif";
                if (boolWindows)
                {
                    lblOS.Text = "Windows 2008 64bit Standard Edition";
                    ddlOS.SelectedValue = "W";
                    ddlSize.Items.Add(new ListItem("Small - VMware Guest (1 Core, 2GB RAM)", "S"));
                    ddlSize.Items.Add(new ListItem("Medium - VMware Guest (2 Cores, 4GB RAM)", "M"));
                    ddlSize.Items.Add(new ListItem("Large - VMware Guest (4 Cores, 8GB RAM)", "L"));
                }
                else if (boolLinux)
                {
                    lblOS.Text = "Linux RedHat 5 Update 7";
                    ddlOS.SelectedValue = "L";
                    ddlSize.Items.Add(new ListItem("Small - VMware Guest (1 Core, 2GB RAM)", "S"));
                    ddlSize.Items.Add(new ListItem("Medium - VMware Guest (2 Cores, 4GB RAM)", "M"));
                    ddlSize.Items.Add(new ListItem("Large - VMware Guest (4 Cores, 8GB RAM)", "L"));
                }
                else if (boolSolaris)
                {
                    lblOS.Text = "Solaris 10";
                    ddlOS.SelectedValue = "S";
                    ddlSize.Items.Add(new ListItem("Small - SUN Virtual (8 threads, 5GB RAM)", "S"));
                    ddlSize.Items.Add(new ListItem("Medium - SUN Virtual (12 threads, 8GB RAM)", "M"));
                    ddlSize.Items.Add(new ListItem("Large - SUN Virtual (16 threads, 12GB RAM)", "L"));
                }
                else if (boolAIX)
                {
                    lblOS.Text = "AIX 6";
                    ddlOS.SelectedValue = "A";
                    ddlSize.Items.Add(new ListItem("Small - Virtual (1 core, 8GB RAM)", "S"));
                    ddlSize.Items.Add(new ListItem("Medium - Virtual (8 core, 16GB RAM)", "M"));
                    ddlSize.Items.Add(new ListItem("Large - Virtual (16 core, 32GB RAM)", "L"));
                }
                else if (boolOtherOS)
                {
                    lblOS.Text = "Other";
                    ddlOS.SelectedValue = "O";
                    LoadApproval(imgOS, lblOS, "");
                    ddlSize.Items.Add(new ListItem("Small", "S"));
                    ddlSize.Items.Add(new ListItem("Medium", "M"));
                    ddlSize.Items.Add(new ListItem("Large", "L"));
                }
            }
            else
                imgOS.ImageUrl = "/images/spacer.gif";
            ddlSize.Items.Insert(0, new ListItem("-- SELECT --", "0"));

            string strSize = dr["size"].ToString();
            if (imgServerSize.ImageUrl != strLock && strSize != "")
            {
                ddlSize.SelectedValue = strSize;
                if (strSize != "0")
                {
                    imgServerSize.ImageUrl = "/images/check.gif";
                    lblServerSize.Text = (strSize == "L" ? "Large" : (strSize == "M" ? "Medium" : (strSize == "S" ? "Small" : "Unknown")));
                }
            }
            else
                trSummarySize.Visible = false;

            // Backup
            lblValBackup.CssClass = "default";
            DataSet dsBackup = GetBackup(intID);
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
                strTemp.Append(LoadGrid(strGridBackupSun, 1, ref intGridBackups));
                strTemp.Append(LoadGrid(strGridBackupMon, 2, ref intGridBackups));
                strTemp.Append(LoadGrid(strGridBackupTue, 3, ref intGridBackups));
                strTemp.Append(LoadGrid(strGridBackupWed, 4, ref intGridBackups));
                strTemp.Append(LoadGrid(strGridBackupThu, 5, ref intGridBackups));
                strTemp.Append(LoadGrid(strGridBackupFri, 6, ref intGridBackups));
                strTemp.Append(LoadGrid(strGridBackupSat, 7, ref intGridBackups));
                strGridBackup = strTemp.ToString();
            }
            else
            {
                strGridBackupSun = "111111111111111111111111";
                strGridBackupMon = "111111111111111111111111";
                strGridBackupTue = "111111111111111111111111";
                strGridBackupWed = "111111111111111111111111";
                strGridBackupThu = "111111111111111111111111";
                strGridBackupFri = "111111111111111111111111";
                strGridBackupSat = "111111111111111111111111";
                lblValBackup.CssClass = "reddefault";
                boolValid = false;
            }
            
            string strBackupFrequency = dr["backup_frequency"].ToString();
            //string strBackupWindow = dr["backup_window"].ToString();
            if (imgBackup.ImageUrl != strLock)
            {
                if (strBackupFrequency != "")
                {
                    imgBackup.ImageUrl = "/images/check.gif";
                    if (strBackupFrequency == "D")
                    {
                        radBackupDaily.Checked = true;
                        lblBackup.Text = "Daily";
                    }
                    else if (strBackupFrequency == "W")
                    {
                        radBackupWeekly.Checked = true;
                        lblBackup.Text = "Weekly";
                    }
                    else if (strBackupFrequency == "M")
                    {
                        radBackupMonthly.Checked = true;
                        lblBackup.Text = "Monthly";
                    }
                    /*
                    if (strBackupWindow == "D")
                    {
                        radBackupDay.Checked = true;
                        lblBackup.Text += " / Day";
                    }
                    else if (strBackupWindow == "N")
                    {
                        radBackupNight.Checked = true;
                        lblBackup.Text += " / Night";
                    }
                    */
                }
            }
            else
                trSummaryBackup.Visible = false;
            lblSummaryBackup.Text = lblBackup.Text;

            // MAINTENANCE WINDOW
            DataSet dsMaintenance = GetMaintenance(intID);
            if (dsMaintenance.Tables[0].Rows.Count > 0)
            {
                imgMaintenance.ImageUrl = "/images/check.gif";
                
                DataRow drMaintenance = dsMaintenance.Tables[0].Rows[0];
                strGridMaintenanceSun = drMaintenance["sun"].ToString();
                strGridMaintenanceMon = drMaintenance["mon"].ToString();
                strGridMaintenanceTue = drMaintenance["tue"].ToString();
                strGridMaintenanceWed = drMaintenance["wed"].ToString();
                strGridMaintenanceThu = drMaintenance["thu"].ToString();
                strGridMaintenanceFri = drMaintenance["fri"].ToString();
                strGridMaintenanceSat = drMaintenance["sat"].ToString();

                StringBuilder strTemp = new StringBuilder();
                strTemp.Append(LoadGrid(strGridMaintenanceSun, 1, ref intGridMaintenances));
                strTemp.Append(LoadGrid(strGridMaintenanceMon, 2, ref intGridMaintenances));
                strTemp.Append(LoadGrid(strGridMaintenanceTue, 3, ref intGridMaintenances));
                strTemp.Append(LoadGrid(strGridMaintenanceWed, 4, ref intGridMaintenances));
                strTemp.Append(LoadGrid(strGridMaintenanceThu, 5, ref intGridMaintenances));
                strTemp.Append(LoadGrid(strGridMaintenanceFri, 6, ref intGridMaintenances));
                strTemp.Append(LoadGrid(strGridMaintenanceSat, 7, ref intGridMaintenances));
                strGridMaintenance = strTemp.ToString();

                lblMaintenance.Text = intGridMaintenances.ToString() + " Conflict(s)";
            }
            else
            {
                strGridMaintenanceSun = "111111111111111111111111";
                strGridMaintenanceMon = "111111111111111111111111";
                strGridMaintenanceTue = "111111111111111111111111";
                strGridMaintenanceWed = "111111111111111111111111";
                strGridMaintenanceThu = "111111111111111111111111";
                strGridMaintenanceFri = "111111111111111111111111";
                strGridMaintenanceSat = "111111111111111111111111";
            }

            // MAINFRAME
            bool boolMainframe = (dr["mainframe"].ToString() == "1");
            if (imgMainframe.ImageUrl != strLock)
            {
                imgMainframe.ImageUrl = "/images/check.gif";
                lblMainframe.Text = (boolMainframe ? "Yes" : "No");
                if (boolMainframe && (boolProd || boolQA))
                {
                    radMainframe_Yes.Checked = true;
                    strDataCenter = "Summit Data Center";
                    strDataCenterWhy = "All QA / PROD servers with a dependency on communications with the Mainframe will be deployed to the Summit Data Center";
                }
                else
                {
                    radMainframe_No.Checked = true;
                }
            }
            else
                trSummaryMainframe.Visible = false;
            lblSummaryMainframe.Text = lblMainframe.Text;

            // LOCATION
            int intEnv = 0;
            Int32.TryParse(dr["environmentid"].ToString(), out intEnv);
            ddlLocation.SelectedValue = intEnv.ToString();
            if (ddlLocation.Items.Count == 2)
            {
                // Lock
                ddlLocation.SelectedIndex = 1;
                lnkLocation.Enabled = false;
                imgLocation.ImageUrl = strLock;
                lblLocation.Text = ddlLocation.SelectedItem.Text;
            }
            else
            {
                if (intEnv > 0)
                {
                    imgLocation.ImageUrl = "/images/check.gif";
                    lblLocation.Text = ddlLocation.SelectedItem.Text;
                }
            }

            // High Availability
            bool boolHA = (dr["ha"].ToString() == "1");
            if (imgHA.ImageUrl != strLock)
            {
                imgHA.ImageUrl = "/images/check.gif";
                lblHA.Text = (boolHA ? "Yes" : "No");
                if (boolHA)
                {
                    radHA_Yes.Checked = true;
                    panHADetail.Visible = true;
                    string strHA = dr["ha_type"].ToString();
                    if (strHA != "")
                    {
                        radHA.SelectedValue = strHA;
                        lblHA.Text = strHA;
                    }
                    boolCluster = (strHA.ToUpper().Contains("CLUSTER") && strHA.ToUpper().Contains("NON-CLUSTER") == false);
                    if (boolCluster)
                    {
                        string strClusterType = dr["cluster_type"].ToString();
                        if (strClusterType != "")
                        {
                            radCluster.SelectedValue = strClusterType;
                            lblHA.Text += " (" + strClusterType + ")";
                        }
                    }
                    boolLoadBalance = (strHA.ToUpper().Contains("LOAD-BALANC") || strHA.ToUpper().Contains("LOAD BALANC"));
                    LoadHA();
                    txtInstances.Text = dr["instances"].ToString();
                    txtNodes.Text = dr["nodes"].ToString();
                    txtQuorum.Text = dr["quorum"].ToString();
                    if (dr["middleware"].ToString() == "1")
                        radMiddlewareYes.Checked = true;
                    else
                        radMiddlewareNo.Checked = true;
                    if (dr["application"].ToString() == "1")
                        radApplicationYes.Checked = true;
                    else
                        radApplicationNo.Checked = true;
                }
                else
                    radHA_No.Checked = true;
            }
            else
                trSummaryHA.Visible = false;
            lblSummaryHA.Text = lblHA.Text;

            // Software
            if (dr["software"].ToString() == "1")
            {
                imgSoftware.ImageUrl = "/images/check.gif";
                if (chkNDM.Checked && chkCA7.Checked)
                    lblSoftware.Text = "NDM + CA7";
                else if (chkNDM.Checked)
                    lblSoftware.Text = "NDM";
                else if (chkCA7.Checked)
                    lblSoftware.Text = "CA7";
                else
                    lblSoftware.Text = "None";
            }
            
            // Special Hardware
            lblSpecial.Text = "";
            string[] strSpecials = dr["special"].ToString().Split(new char[] { ';' });
            for (int ii = 0; ii < strSpecials.Length; ii++)
            {
                if (strSpecials[ii].Trim() != "")
                {
                    if (lblSpecial.Text != "")
                        lblSpecial.Text += ", ";
                    lblSpecial.Text += strSpecials[ii].Trim();
                    foreach (ListItem li in chkSpecial.Items)
                    {
                        if (li.Text == strSpecials[ii].Trim())
                        {
                            li.Selected = true;
                            break;
                        }
                    }
                }
            }
            lblSummarySpecial.Text = lblSpecial.Text;
            if (lblSpecial.Text == "")
                lblSpecial.Text = "---";
            else
            {
                if (lblSpecial.Text == "None")
                    imgSpecial.ImageUrl = "/images/check.gif";
                else
                    LoadApproval(imgSpecial, lblSpecial, "");
            }
            
            // Storage
            bool boolStorage = (dr["storage"].ToString() == "1");
            bool boolPersistent = (dr["persistent"].ToString() == "1");
            int intStorage = 0;
            Int32.TryParse(dr["non_persistent"].ToString(), out intStorage);
            bool boolLUNs = (Request.QueryString["luns"] != null);
            if (boolStorage || boolLUNs)
            {
                lblStorage.Text = "Yes";
                radStorageYes.Checked = true;
                panStorageYes.Visible = true;
                if (boolPersistent == true || boolLUNs)
                {
                    radPersistent.Checked = true;
                    panPersistent.Visible = true;
                    if (boolDatabase && boolSQL)
                    {
                        panStorageDatabase.Visible = true;
                        btnGenerate.Visible = true;
                    }
                    else
                        panStorageLUNs.Visible = true;
                    intStorage = LoadStorage();
                    lblStorage.Text = "Persistent, " + intStorage.ToString() + " GB";
                }
                else
                {
                    radPersistentNon.Checked = true;
                    panPersistentNon.Visible = true;
                    txtStorage.Text = intStorage.ToString();
                    lblStorage.Text = "Non-Persistent, " + intStorage.ToString() + " GB";
                }
                if (intStorage > 0)
                    imgStorage.ImageUrl = "/images/check.gif";
            }
            else
            {
                radStroageNo.Checked = true;
                imgStorage.ImageUrl = "/images/check.gif";
                lblStorage.Text = "No";
            }

            // Accounts
            bool boolAccounts = (dr["accounts"].ToString() == "1");
            bool boolNotify = (dr["account_results"].ToString() == "1");
            DataSet dsAccounts = GetAccounts();
            int intAccounts = dsAccounts.Tables[0].Rows.Count;
            lblValAccounts.CssClass = "default";
            if (boolAccounts)
            {
                imgAccounts.ImageUrl = "/images/check.gif";
                lblAccounts.Text = intAccounts.ToString();
            }
            else
            {
                lblValAccounts.CssClass = "reddefault";
                boolValid = false;
            }
          
            chkNotify.Checked = boolNotify;
            lblSummaryAccounts.Text = "<a href=\"javascript:void(0);\" onclick=\"OpenAccountWindow(" + intID.ToString() + ");\">" + intAccounts.ToString() + " Accounts</a>";
            
            // Quantity
            int intCount = 0;
            Int32.TryParse(dr["quantity"].ToString(), out intCount);
            lblValQuantity.CssClass = "default";
            if (intCount > 0)
            {
                imgCount.ImageUrl = "/images/check.gif";
                ddlCount.SelectedValue = intCount.ToString();
                if (intCount == 16)
                {
                    lblCount.Text = "15+";
                    LoadApproval(imgCount, lblCount, "");
                }
                else
                    lblCount.Text = intCount.ToString();
            }
            else
            {
                lblValQuantity.CssClass = "reddefault";
                boolValid = false;
            }
            lblSummaryQuantity.Text = lblCount.Text;
            
            // Build Date
            string strDate = dr["commitment"].ToString();
            string strTarget = "";
            lblValDate.CssClass = "default";
            if (strDate != "")
            {
                DateTime datDate = DateTime.Now;
                if (DateTime.TryParse(strDate, out datDate) == true)
                {
                    txtDate.Text = datDate.ToShortDateString();
                    lblDate.Text = txtDate.Text;
                    imgDate.ImageUrl = "/images/check.gif";
                    // Target completion date = 2 weeks later than commitment
                    DateTime datTarget = oHoliday.GetDays(dblSLA, datDate);
                    strTarget = datTarget.ToShortDateString();
                }
            }
            else
            {
                lblValDate.CssClass = "reddefault";
                boolValid = false;
            }
            lblSummaryDate.Text = lblDate.Text;
            if (strTarget != "")
                lblSummaryTarget.Text = strTarget + "&nbsp;&nbsp;&nbsp;&nbsp;(<a href=\"javascript:void(0);\" onclick=\"alert('" + strTarget + " is " + dblSLA.ToString() + " business days from your build date (" + lblSummaryDate.Text + ").\\n\\nBusiness days exclude weekends and holidays.');\">How is this calculated?</a>)";
            else
                lblSummaryTarget.Text = "<i>Missing Build Date...<i>";

            int intConfidence = 0;
            Int32.TryParse(dr["confidenceid"].ToString(), out intConfidence);
            if (intConfidence > 0)
            {
                imgConfidence.ImageUrl = "/images/check.gif";
                ddlConfidence.SelectedValue = intConfidence.ToString();
                lblConfidence.Text = ddlConfidence.SelectedItem.Text;
                btnNext.Visible = false;
                btnBack.Visible = false;
                btnUpdate.Visible = true;
            }
            lblSummaryConfidence.Text = lblConfidence.Text;

            // Pick DataCenter based on environment
            if (strDataCenter == "")
            {
                imgDataCenter.Attributes.Add("onclick", "alert('The datacenter will be either the Cleveland Data Center or the Summit Data Center.\\n\\nThis will be decided during execution. It is based on the available inventory at that time.');return false;");
                lblDataCenter.Text = "Cleveland or Summit";
            }
            else
            {
                imgDataCenter.Attributes.Add("onclick", "alert('Based on the following criteria, your server will be located at the " + strDataCenter + ".\\n\\n--> " + strDataCenterWhy + "');return false;");
                lblDataCenter.Text = strDataCenter;
            }

            lblSummarySolution.Text = "VMware Guest Version 3.5";
            lblSummaryOS.Text = lblOS.Text;
            if (boolPersistent == true)
                lblSummaryStorage.Text = "<a href=\"javascript:void(0);\" onclick=\"OpenStorageWindow(" + intID.ToString() + ");\">" + lblStorage.Text + "</a>";
            else
                lblSummaryStorage.Text = lblStorage.Text;
            lblSummaryLocation.Text = lblEnvironment.Text + ", " + lblLocation.Text + " at " + strDataCenter;
        }

    }
    private void LoadApproval(System.Web.UI.WebControls.Image _image, Label _label, string _text)
    {
        boolReview = true;
        _image.ImageUrl = strApproval;
        _image.ToolTip = (_text == "" ? "Requires Approval" : _text);
    }
    protected void chkDatabase_Change(Object Sender, EventArgs e)
    {
        panDatabase.Visible = chkDatabase.Checked;
    }
    protected void chkCA7_Change(Object Sender, EventArgs e)
    {
        panCA7.Visible = chkCA7.Checked;
    }
    protected void radHA_Change(Object Sender, EventArgs e)
    {
        LoadHA();
    }
    private void LoadHA()
    {
        string strHA = radHA.SelectedItem.Value;
        boolCluster = (strHA.ToUpper().Contains("CLUSTER") && strHA.ToUpper().Contains("NON-CLUSTER") == false);
        boolLoadBalance = (strHA.ToUpper().Contains("LOAD-BALANC") || strHA.ToUpper().Contains("LOAD BALANC"));
        panCluster.Visible = false;
        panLoadBalanced.Visible = false;
        
        if (boolLoadBalance)
        {
            panLoadBalanced.Visible = true;
            panLoadBalancedApplication.Visible = (lblServerType.Text.ToUpper().Contains("WEB"));
        }
        if (boolCluster)
        {
            panCluster.Visible = true;
            if (lblOS.Text.ToUpper().Contains("LINUX") == true)
                radCluster.Items[1].Enabled = true;
            else
            {
                radCluster.Items[1].Enabled = false;
                radCluster.Items[0].Selected = true;
            }
        }
    }
    private void LoadStep(HtmlTableRow _row, System.Web.UI.WebControls.Image _image, LinkButton _link, Label _label)
    {
        _row.BgColor = strHighlight;
        _image.ImageUrl = "/images/green_arrow.gif";
        _link.CssClass = "bold";
        if (_label.Text == "---")
            _label.Text = "";
        if (strValidation != "")
            _link.Attributes.Add("onclick", "return " + strValidation);
    }
    protected void btnBack_Click(Object Sender, EventArgs e)
    {
        Save();
        Navigate(true);
    }
    protected void btnNext_Click(Object Sender, EventArgs e)
    {
        Save();
        Navigate(false);
    }
    protected void btnUpdate_Click(Object Sender, EventArgs e)
    {
        Save();
        UpdateStep("2.2");
    }
    protected void btnSubmit_Click(Object Sender, EventArgs e)
    {
        if (radComplete.Checked == true)
            UpdateStep("2.3");
        else if (radChange.Checked == true)
            UpdateStep("2.1.5");
        else if (radException.Checked == true)
        {
            Page.ClientScript.RegisterStartupScript(typeof(Page), "approval", "<script type=\"text/javascript\">confirm('Placeholder for approval...');<" + "/" + "script>");
            //panApproval.Visible = true;
        }
        else
            Page.ClientScript.RegisterStartupScript(typeof(Page), "selection", "<script type=\"text/javascript\">alert('Make a selection from the choices at the bottom of the screen');<" + "/" + "script>");
    }
    
    protected void radStorage_Change(Object Sender, EventArgs e)
    {
        panStorageYes.Visible = radStorageYes.Checked;
    }
    protected void radHADetail_Change(Object Sender, EventArgs e)
    {
        panHADetail.Visible = radHA_Yes.Checked;
    }
    protected void radPersistent_Change(Object Sender, EventArgs e)
    {
        panPersistent.Visible = false;
        panPersistentNon.Visible = false;

        if (radPersistent.Checked == true)
        {
            panPersistent.Visible = true;
            if (lblServerType.Text.Contains("SQL"))
            {
                panStorageDatabase.Visible = true;
                btnGenerate.Visible = true;
            }
            else
                panStorageLUNs.Visible = true;
            LoadStorage();
        }
        if (radPersistentNon.Checked == true)
            panPersistentNon.Visible = true;
    }
    private string GetGrid(string _hidden, int _day)
    {
        StringBuilder strReturn = new StringBuilder();
        strReturn.Append(GetGrid(_hidden, _day, "12:00 AM", strAppend));
        strReturn.Append(GetGrid(_hidden, _day, "1:00 AM", strAppend));
        strReturn.Append(GetGrid(_hidden, _day, "2:00 AM", strAppend));
        strReturn.Append(GetGrid(_hidden, _day, "3:00 AM", strAppend));
        strReturn.Append(GetGrid(_hidden, _day, "4:00 AM", strAppend));
        strReturn.Append(GetGrid(_hidden, _day, "5:00 AM", strAppend));
        strReturn.Append(GetGrid(_hidden, _day, "6:00 AM", strAppend));
        strReturn.Append(GetGrid(_hidden, _day, "7:00 AM", strAppend));
        strReturn.Append(GetGrid(_hidden, _day, "8:00 AM", strAppend));
        strReturn.Append(GetGrid(_hidden, _day, "9:00 AM", strAppend));
        strReturn.Append(GetGrid(_hidden, _day, "10:00 AM", strAppend));
        strReturn.Append(GetGrid(_hidden, _day, "11:00 AM", strAppend));
        strReturn.Append(GetGrid(_hidden, _day, "12:00 PM", strAppend));
        strReturn.Append(GetGrid(_hidden, _day, "1:00 PM", strAppend));
        strReturn.Append(GetGrid(_hidden, _day, "2:00 PM", strAppend));
        strReturn.Append(GetGrid(_hidden, _day, "3:00 PM", strAppend));
        strReturn.Append(GetGrid(_hidden, _day, "4:00 PM", strAppend));
        strReturn.Append(GetGrid(_hidden, _day, "5:00 PM", strAppend));
        strReturn.Append(GetGrid(_hidden, _day, "6:00 PM", strAppend));
        strReturn.Append(GetGrid(_hidden, _day, "7:00 PM", strAppend));
        strReturn.Append(GetGrid(_hidden, _day, "8:00 PM", strAppend));
        strReturn.Append(GetGrid(_hidden, _day, "9:00 PM", strAppend));
        strReturn.Append(GetGrid(_hidden, _day, "10:00 PM", strAppend));
        strReturn.Append(GetGrid(_hidden, _day, "11:00 PM", strAppend));
        return strReturn.ToString();
    }
    private string GetGrid(string _hidden, int _day, string _time, string _append)
    {
        string strReturn = "1"; // unchecked
        string strHidden = Request.Form[_hidden];
        string[] strSelections = strHidden.Split(new char[] { '&' });
        foreach (string strSelection in strSelections)
        {
            string[] strValues = strSelection.Split(new char[] { '_' });
            int intDay = 0;
            if (Int32.TryParse(strValues[0], out intDay) && intDay == _day)
            {
                // Days match...check time
                if (_time == strValues[1])
                {
                    // Times match...return selection
                    strReturn = strValues[2];
                    break;
                }
            }
        }
        return strReturn + _append;
    }
    private string LoadGrid(string _value, int _day, ref int _counter)
    {
        StringBuilder strReturn = new StringBuilder();
        strReturn.Append(LoadGrid(_value, _day, 0, "12:00 AM", ref _counter));
        strReturn.Append(LoadGrid(_value, _day, 1, "1:00 AM", ref _counter));
        strReturn.Append(LoadGrid(_value, _day, 2, "2:00 AM", ref _counter));
        strReturn.Append(LoadGrid(_value, _day, 3, "3:00 AM", ref _counter));
        strReturn.Append(LoadGrid(_value, _day, 4, "4:00 AM", ref _counter));
        strReturn.Append(LoadGrid(_value, _day, 5, "5:00 AM", ref _counter));
        strReturn.Append(LoadGrid(_value, _day, 6, "6:00 AM", ref _counter));
        strReturn.Append(LoadGrid(_value, _day, 7, "7:00 AM", ref _counter));
        strReturn.Append(LoadGrid(_value, _day, 8, "8:00 AM", ref _counter));
        strReturn.Append(LoadGrid(_value, _day, 9, "9:00 AM", ref _counter));
        strReturn.Append(LoadGrid(_value, _day, 10, "10:00 AM", ref _counter));
        strReturn.Append(LoadGrid(_value, _day, 11, "11:00 AM", ref _counter));
        strReturn.Append(LoadGrid(_value, _day, 12, "12:00 PM", ref _counter));
        strReturn.Append(LoadGrid(_value, _day, 13, "1:00 PM", ref _counter));
        strReturn.Append(LoadGrid(_value, _day, 14, "2:00 PM", ref _counter));
        strReturn.Append(LoadGrid(_value, _day, 15, "3:00 PM", ref _counter));
        strReturn.Append(LoadGrid(_value, _day, 16, "4:00 PM", ref _counter));
        strReturn.Append(LoadGrid(_value, _day, 17, "5:00 PM", ref _counter));
        strReturn.Append(LoadGrid(_value, _day, 18, "6:00 PM", ref _counter));
        strReturn.Append(LoadGrid(_value, _day, 19, "7:00 PM", ref _counter));
        strReturn.Append(LoadGrid(_value, _day, 20, "8:00 PM", ref _counter));
        strReturn.Append(LoadGrid(_value, _day, 21, "9:00 PM", ref _counter));
        strReturn.Append(LoadGrid(_value, _day, 22, "10:00 PM", ref _counter));
        strReturn.Append(LoadGrid(_value, _day, 23, "11:00 PM", ref _counter));
        return strReturn.ToString();
    }
    private string LoadGrid(string _value, int _day, int _index, string _time, ref int _counter)
    {
        if (_value[_index] == '0')
        {
            _counter++;
            return _day.ToString() + "_" + _time + "_0&";
        }
        else
            return "";
    }
    protected int LoadStorage()
    {
        DataSet dsStorage = GetStorage();
        // First Check to be sure database is correct
        bool boolStorageExists = false;
        int intTotal = 0;
        foreach (DataRow drStorage in dsStorage.Tables[0].Rows)
        {
            int intDrive = 0;
            if (Int32.TryParse(drStorage["driveid"].ToString(), out intDrive) == true)
            {
                boolStorageExists = true;
                if (intDrive > 0)
                {
                    int intDB = -1;
                    if (drStorage["db"].ToString() == "1" && boolDatabase == true && boolSQL == false)
                        intDB = 1;
                    if (drStorage["db"].ToString() == "0" && boolDatabase == true && boolSQL == true)
                        intDB = 0;
                    if (intDB >= 0)
                    {
                        DeleteLuns(intDB);
                        Response.Redirect(Request.Path + "?id=" + intID.ToString() + "&cleanup=true");
                    }
                }
            }
            int intTemp = 0;
            Int32.TryParse(drStorage["size"].ToString(), out intTemp);
            intTotal += intTemp;
        }
        if (boolStorageExists == true && boolDatabase && boolSQL)
        {
            panStorageDatabase.Visible = false;
            btnGenerate.Visible = false;
            btnReset.Visible = true;
            panStorageLUNs.Visible = true;
        }
        rptStorage.DataSource = dsStorage;
        rptStorage.DataBind();
        foreach (RepeaterItem ri in rptStorage.Items)
        {
            ImageButton _delete = (ImageButton)ri.FindControl("btnStorageDelete");
            _delete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this LUN?');");
            CheckBox _shared = (CheckBox)ri.FindControl("chkStorageSize");
            _shared.Checked = (_shared.Text == "1");
            _shared.Text = "";
        }
        if (lblOS.Text.ToUpper().Contains("WINDOWS"))
        {
            trStorageApp.Visible = true;
            DataSet dsApp = GetStorage(-1000);
            if (dsApp.Tables[0].Rows.Count > 0)
            {
                if (!IsPostBack || txtStorageSizeE.Text == "")
                {
                    int intTemp = 0;
                    if (Int32.TryParse(dsApp.Tables[0].Rows[0]["size"].ToString(), out intTemp) == true)
                    {
                        txtStorageSizeE.Text = intTemp.ToString();
                        intTotal += intTemp;
                    }
                }
                btnStorageSaveSizeE.CommandName = dsApp.Tables[0].Rows[0]["id"].ToString();
            }
        }
        return intTotal;
    }
    protected void radTempDB_Change(Object Sender, EventArgs e)
    {
        panTempDB.Visible = radTempDBYes.Checked;
    }
    protected void radDatabaseNon_Change(Object Sender, EventArgs e)
    {
        panDatabaseNon.Visible = radDatabaseNonYes.Checked;
    }
    protected void lnkRefresh_Click(Object Sender, EventArgs e)
    {
        LoadPanel(lblProcedureNumber.Text);
    }
    protected void lnkCrumbs_Click(Object Sender, EventArgs e)
    {
        LinkButton oButton = (LinkButton)Sender;
        UpdateStep(oButton.CommandArgument);
    }
    public DataSet Get(int _id)
    {
        SqlParameter[] arParams = new SqlParameter[1];
        arParams[0] = new SqlParameter("@id", _id);
        return SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT * FROM cv_accenture WHERE id = @id AND deleted = 0", arParams);
    }
    public string Get(int _id, string _column)
    {
        DataSet ds = Get(_id);
        if (ds.Tables[0].Rows.Count > 0)
            return ds.Tables[0].Rows[0][_column].ToString();
        else
            return "";
    }
    
    // Backup
    public DataSet GetBackup(int _id)
    {
        SqlParameter[] arParams = new SqlParameter[1];
        arParams[0] = new SqlParameter("@id", _id);
        return SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT * FROM cv_accenture_backup WHERE designid = @id AND deleted = 0", arParams);
    }

    // Maintenance
    public DataSet GetMaintenance(int _id)
    {
        SqlParameter[] arParams = new SqlParameter[1];
        arParams[0] = new SqlParameter("@id", _id);
        return SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT * FROM cv_accenture_maintenance WHERE designid = @id AND deleted = 0", arParams);
    }

    // EXCLUSIONS
    public DataSet GetExclusions()
    {
        SqlParameter[] arParams = new SqlParameter[1];
        arParams[0] = new SqlParameter("@designid", intID);
        return SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT * FROM cv_accenture_exclusions WHERE designid = @designid AND deleted = 0", arParams);
    }
    protected void btnAddExclusion_Click(Object Sender, EventArgs e)
    {
        SqlParameter[] arParams = new SqlParameter[2];
        arParams[0] = new SqlParameter("@designid", intID);
        arParams[1] = new SqlParameter("@path", txtPath.Text);
        SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addAccentureExclusion", arParams);
        Response.Redirect(Request.Path + "?id=" + Request.QueryString["id"]);
    }
    protected void btnDeleteExclusion_Click(Object Sender, EventArgs e)
    {
        LinkButton oDelete = (LinkButton)Sender;
        SqlParameter[] arParams = new SqlParameter[1];
        arParams[0] = new SqlParameter("@id", Int32.Parse(oDelete.CommandArgument));
        SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteAccentureExclusion", arParams);
        Response.Redirect(Request.Path + "?id=" + Request.QueryString["id"]);
    }
    
    // ACCOUNTS
    public DataSet GetAccounts()
    {
        SqlParameter[] arParams = new SqlParameter[1];
        arParams[0] = new SqlParameter("@designid", intID);
        return SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT * FROM cv_accenture_accounts WHERE designid = @designid AND deleted = 0", arParams);
    }
    protected void btnAddAccount_Click(Object Sender, EventArgs e)
    {
        int intUser = 0;
        Int32.TryParse(Request.Form[hdnUser.UniqueID], out intUser);
        bool boolDuplicate = false;
        DataSet dsAccount = GetAccounts();
        foreach (DataRow drAccount in dsAccount.Tables[0].Rows)
        {
            if (drAccount["userid"].ToString() == intUser.ToString())
            {
                boolDuplicate = true;
                break;
            }
        }
        if (boolDuplicate == false)
        {
            SqlParameter[] arParams = new SqlParameter[4];
            arParams[0] = new SqlParameter("@designid", intID);
            arParams[1] = new SqlParameter("@userid", intUser);
            arParams[2] = new SqlParameter("@access", ddlPermission.SelectedItem.Value);
            arParams[3] = new SqlParameter("@remote", (chkRemoteDesktop.Checked ? 1 : 0));
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addAccentureAccount", arParams);
            Response.Redirect(Request.Path + "?id=" + Request.QueryString["id"]);
        }
        else
        {
            Response.Redirect(Request.Path + "?id=" + Request.QueryString["id"] + "&duplicate=" + oUser.GetName(intUser));
        }
    }
    protected void btnDeleteAccount_Click(Object Sender, EventArgs e)
    {
        LinkButton oDelete = (LinkButton)Sender;
        SqlParameter[] arParams = new SqlParameter[1];
        arParams[0] = new SqlParameter("@id", Int32.Parse(oDelete.CommandArgument));
        SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteAccentureAccount", arParams);
        Response.Redirect(Request.Path + "?id=" + Request.QueryString["id"]);
    }


    // SOFTWARE
    public DataSet GetSoftware()
    {
        SqlParameter[] arParams = new SqlParameter[1];
        arParams[0] = new SqlParameter("@designid", intID);
        return SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT * FROM cv_accenture_software WHERE designid = @designid AND deleted = 0", arParams);
    }


    // STORAGE
    public DataSet GetStorage()
    {
        SqlParameter[] arParams = new SqlParameter[1];
        arParams[0] = new SqlParameter("@designid", intID);
        return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getAccentureStorages", arParams);
    }
    public DataSet GetStorage(int intDrive)
    {
        SqlParameter[] arParams = new SqlParameter[2];
        arParams[0] = new SqlParameter("@designid", intID);
        arParams[1] = new SqlParameter("@driveid", intDrive);
        return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getAccentureStorage", arParams);
    }
    protected void btnStorageDriveAdd_Click(Object Sender, EventArgs e)
    {
        trStorageDrive.Visible = true;
        txtStorageSizeNew.Text = "0";
        trStorageDriveNew.Visible = false;
    }
    protected void btnStorageSave_Click(Object Sender, ImageClickEventArgs e)
    {
        ImageButton oButton = (ImageButton)Sender;
        if (oButton.CommandArgument == "APP")
        {
            if (oButton.CommandName == "")
                AddLun(-1000, "", txtStorageSizeE.Text, false, false);
            else
                UpdateLun(Int32.Parse(oButton.CommandName), "", txtStorageSizeE.Text, false);
        }
        else if (oButton.CommandArgument == "NEW")
        {
            if (oButton.CommandName == "")
                AddLun(Int32.Parse(ddlStorageDrive.SelectedItem.Value), txtStoragePathNew.Text, txtStorageSizeNew.Text, chkStorageSizeNew.Checked, boolSQL);
            else
                UpdateLun(Int32.Parse(oButton.CommandName), txtStoragePathNew.Text, txtStorageSizeNew.Text, chkStorageSizeNew.Checked);
        }
        else if (oButton.CommandArgument == "SAVE")
        {
            foreach (RepeaterItem ri in rptStorage.Items)
            {
                ImageButton oSave = (ImageButton)ri.FindControl("btnStorageSave");
                if (oButton == oSave)
                {
                    TextBox oPath = (TextBox)ri.FindControl("txtStoragePath");
                    TextBox oSize = (TextBox)ri.FindControl("txtStorageSize");
                    CheckBox oShared = (CheckBox)ri.FindControl("chkStorageSize");
                    UpdateLun(Int32.Parse(oButton.CommandName), oPath.Text, oSize.Text, oShared.Checked);
                    break;
                }
            }
        }
        else if (oButton.CommandArgument == "DELETE")
        {
            SqlParameter[] arParams = new SqlParameter[1];
            arParams[0] = new SqlParameter("@id", Int32.Parse(oButton.CommandName));
            SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteAccentureStorage", arParams);
        }
        else
        {
            // CANCEL
        }
        Response.Redirect(Request.Path + "?id=" + Request.QueryString["id"] + "&luns=true");
    }
    protected void AddLun(int _driveid, string _path, string _size, bool _shared, bool _db)
    {
        int intSize = 0;
        Int32.TryParse(_size, out intSize);
        SqlParameter[] arParams = new SqlParameter[6];
        arParams[0] = new SqlParameter("@designid", intID);
        arParams[1] = new SqlParameter("@driveid", _driveid);
        arParams[2] = new SqlParameter("@path", _path);
        arParams[3] = new SqlParameter("@size", intSize);
        arParams[4] = new SqlParameter("@shared", (_shared ? 1 : 0));
        arParams[5] = new SqlParameter("@db", (_db ? 1 : 0));
        SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_addAccentureStorage", arParams);
    }
    protected void UpdateLun(int _id, string _path, string _size, bool _shared)
    {
        int intSize = 0;
        Int32.TryParse(_size, out intSize);
        SqlParameter[] arParams = new SqlParameter[4];
        arParams[0] = new SqlParameter("@id", _id);
        arParams[1] = new SqlParameter("@path", _path);
        arParams[2] = new SqlParameter("@size", intSize);
        arParams[3] = new SqlParameter("@shared", (_shared ? 1 : 0));
        SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_updateAccentureStorage", arParams);
    }
    protected void DeleteLuns(int _db)
    {
        SqlParameter[] arParams = new SqlParameter[2];
        arParams[0] = new SqlParameter("@designid", intID);
        arParams[1] = new SqlParameter("@db", _db);
        SqlHelper.ExecuteNonQuery(dsn, CommandType.StoredProcedure, "pr_deleteAccentureStorages", arParams);
    }
    
    public DataSet GetDrives()
    {
        return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getStorageDriveLetters");
    }
    private void btnGenerate_Click(Object Sender, EventArgs e)
    {
        double dblSize = double.Parse(txtDatabaseSize.Text);
        int intNon = (radDatabaseNonYes.Checked ? 1 : 0);
        double dblNon = 0.00;
        if (intNon == 1)
            dblNon = double.Parse(txtDatabaseNon.Text);
        double dblPercent = double.Parse(txtDatabasePercent.Text);
        double dblTempDB = 0.00;
        double.TryParse(txtDatabaseTemp.Text, out dblTempDB);
        AddLunSQLPNC(dblSize, dblNon, dblPercent, dblTempDB, dblCompressionPercentage, dblTempDBOverhead, true);
        Response.Redirect(Request.Path + "?id=" + Request.QueryString["id"]);
    }
    private void btnReset_Click(Object Sender, EventArgs e)
    {
        DeleteLuns(1);
        Response.Redirect(Request.Path + "?id=" + Request.QueryString["id"]);
    }
    public void AddLunSQLPNC(double _size, double _non, double _percent, double _tempDB, double _compressionPercentage, double _tempDBOverhead, bool _2008)
    {
        double dblLunMin = 500.00;
        double dblLunMax = 750.00;

        string strClass = lblEnvironment.Text.ToUpper();
        bool boolDev = strClass.Contains("DEV");
        bool boolTest = strClass.Contains("TEST");
        bool boolQA = strClass.Contains("QA");
        bool boolProd = strClass.Contains("PROD");

        Forecast oForecast = new Forecast(0, dsn);
        Classes oClass = new Classes(0, dsn);
        _percent = (_percent / 100.00);
        double dblLUN = 0.00;

        double dblOverallSize = (_size * (1.00 + _percent + 0.05));
        double dblDividend = dblOverallSize / dblLunMax;
        dblDividend = Math.Ceiling(dblDividend);
        //Response.Write(dblDividend.ToString("0") + "<br/>");
        double dblEachLUN = (dblOverallSize / dblDividend);
        //Response.Write(dblEachLUN.ToString() + "<br/>");
        dblEachLUN = RoundUp(dblEachLUN);
        dblOverallSize = (dblEachLUN * dblDividend);
        //Response.Write(dblEachLUN.ToString() + " x " + dblDividend.ToString() + " = " + dblOverallSize.ToString() + "<br/>");


        int intDrive = GetNextDrive();
        // R:
        dblLUN = 1.00;
        AddLun(intDrive, "", dblLUN.ToString(), true, true);


        // R:\Production
        dblLUN = 10.00;
        AddLun(intDrive, "\\Production", dblLUN.ToString(), true, true);


        if (boolCluster)
        {
            // R:\Production\OEM
            dblLUN = 10.00;
            AddLun(intDrive, "\\Production\\oracle_oem", dblLUN.ToString(), true, true);
        }


        // R:\Production\Database\SQL01 ***
        dblLUN = dblOverallSize;
        double dblLUN_Database = Minimum(dblLUN);
        for (double dblStart = 1.00; dblStart <= dblDividend; dblStart += 1.00)
        {
            string strBefore = "";
            if (dblStart.ToString().Length == 1)
                strBefore = "0";
            AddLun(intDrive, "\\Production\\Database\\SQL" + strBefore + dblStart.ToString(), Minimum(dblEachLUN).ToString(), true, true);
        }


        // R:\Production\Logs
        dblLUN = dblOverallSize;
        if (dblLUN > 2000.00)
            dblLUN = 2000.00;   // 2 TB is maximum for LOGS
        double dblLUN_Logs = Minimum(dblLUN);
        AddLun(intDrive, "\\Production\\Logs", Minimum(dblLUN).ToString(), true, true);


        // R:\Production\TempDB
        //dblLUN = (_tempDB * _tempDBOverhead);
        if (_tempDB == 0.00)
        {
            // Calcualate tempDB dynamically
            _tempDB = (_size * .02);                                // 2% of database size
            _tempDB += (_size * _percent);                          // Plus x% of largest table
        }
        dblLUN = RoundUp(_tempDB + (_tempDB * _tempDBOverhead));    // Plus OverHead value
        AddLun(intDrive, "\\Production\\TempDB", Minimum(dblLUN).ToString(), true, true);


        // R:\Production\Backups\SQL01*
        double dblBackups = dblOverallSize;
        if (_2008 == true)
            dblBackups = ((dblLUN_Logs + dblLUN_Database) * _compressionPercentage);
        else
            dblBackups = (dblLUN_Logs + dblLUN_Database);
        if (dblBackups < dblLunMax)
        {
            // Save backups to one single LUN
            AddLun(intDrive, "\\Production\\Backups\\SQL01", Minimum(dblBackups).ToString(), true, true);
        }
        else
        {
            if (dblBackups == dblLUN_Database)
            {
                // Same configuration as Database LUNs
                for (double dblStart = 1.00; dblStart <= (dblDividend * 1.00); dblStart += 1.00)
                {
                    string strBefore = "";
                    if (dblStart.ToString().Length == 1)
                        strBefore = "0";
                    AddLun(intDrive, "\\Production\\Backups\\SQL" + strBefore + dblStart.ToString(), Minimum(dblEachLUN).ToString(), true, true);
                }
            }
            else if (dblBackups == (dblLUN_Database * 2.00))
            {
                // Double the configuration of Database LUNs
                for (double dblStart = 1.00; dblStart <= (dblDividend * 2.00); dblStart += 1.00)
                {
                    string strBefore = "";
                    if (dblStart.ToString().Length == 1)
                        strBefore = "0";
                    AddLun(intDrive, "\\Production\\Backups\\SQL" + strBefore + dblStart.ToString(), Minimum(dblEachLUN).ToString(), true, true);
                }
            }
            else
            {
                // Neither of the above, split them out
                double dblDividendB = dblBackups / dblLunMax;
                dblDividendB = Math.Ceiling(dblDividendB);
                double dblEachLUNBackup = (dblBackups / dblDividendB);
                dblEachLUNBackup = RoundUp(dblEachLUNBackup);
                dblBackups = (dblEachLUNBackup * dblDividendB);

                for (double dblStart = 1.00; dblStart <= dblDividendB; dblStart += 1.00)
                {
                    string strBefore = "";
                    if (dblStart.ToString().Length == 1)
                        strBefore = "0";
                    AddLun(intDrive, "\\Production\\Backups\\SQL" + strBefore + dblStart.ToString(), Minimum(dblEachLUNBackup).ToString(), true, true);
                }
            }
        }


        // R:\AppData
        if (_non > 0.00)
            AddLun(intDrive, "\\AppData", Minimum(_non).ToString(), false, true);
    }
    private double RoundUp(double _size)
    {
        _size = Math.Ceiling(_size);
        while (_size % 5.00 != 0.00)
            _size += 1.00;
        return _size;
    }
    private double Minimum(double _size)
    {
        if (_size > 0.00 && _size < 10.00)
            _size = 10.00;
        return Math.Ceiling(_size);
    }
    private int GetNextDrive()
    {
        int intReturn = 0;
        DataSet dsStorage = GetStorage();
        // First Check to be sure database is correct
        foreach (DataRow drStorage in dsStorage.Tables[0].Rows)
        {
            int intDrive = 0;
            if (Int32.TryParse(drStorage["driveid"].ToString(), out intDrive) == true)
            {
                if (intDrive > intReturn)
                    intReturn = intDrive;
            }
        }
        return intReturn + 1;
    }
    
    
</script>
<script type="text/javascript">
    function OpenAccountWindow(strID) {
        ShowPanel('/frame/loading.htm?referrer=/frame/design_accounts.aspx?id=' + strID,500,400);
    }
    function OpenStorageWindow(strID) {
        ShowPanel('/frame/loading.htm?referrer=/frame/design_storage.aspx?id=' + strID,500,400);
    }
    function SetConflict2(_check, _hidden, _day, _time) {
        if (_check.className == "selectGridCheck")
            _check.className = "selectGridCancel";
        else
            _check.className = "selectGridCheck";
    }
    function SetConflict(_check, _hidden, _day, _time) {
        var intAvailable = 0;
        if (_check.className == "selectGridCheck") {
            _check.className = "selectGridCancel";
            intAvailable = 0;
        }
        else {
            _check.className = "selectGridCheck";
            intAvailable = 1;
        }
        _hidden = document.getElementById(_hidden);
        _hidden.value = UpdateStringItems(_hidden.value, _day + "_" + _time, intAvailable);
    }
    function SetConflictsDay(_a, _hidden, _day) {
        var oCell = _a.parentElement;
        var oRow = oCell.parentElement;
        var oTable = oRow.parentElement;
        //alert(oTable.childNodes.length);
	    for(var ii=1; ii<oTable.childNodes.length; ii++)
            oTable.childNodes[ii].childNodes[_day].firstChild.click();
    }
    function SetConflictsTime(_a, _hidden, _time) {
        var oCell = _a.parentElement;
        var oRow = oCell.parentElement;
	    for(var ii=0; ii<oRow.childNodes.length; ii++)
	        oRow.childNodes[ii].firstChild.click();
    }
    
</script>
<table width="100%" height="100%" cellpadding="0" cellspacing="0" border="0">
    <tr height="1">
        <td>
            <table width="100%" cellpadding="0" cellspacing="5" border="0">
                <tr>
                    <td rowspan="2"><img src="/images/forecast2.gif" border="0" align="absmiddle" /></td>
                    <td class="header" width="100%" valign="bottom">ClearView Design Builder</td>
                </tr>
                <tr>
                    <td width="100%" valign="top"><asp:Label ID="lblSubHeader" runat="server" /></td>
                </tr>
            </table>
        </td>
        <td><asp:LinkButton ID="lblProcedureNumber" runat="server" CssClass="header" OnClick="lnkRefresh_Click" /></td>
    </tr>
    <tr height="1">
        <td colspan="2"><img src="/images/spacer.gif" border="0" width="1" height="1" /></td>
    </tr>
    <tr>
        <td colspan="2" valign="top">
            <asp:Panel ID="panRequirements" runat="server" Visible="false">
                <table height="100%" width="100%" cellpadding="0" cellspacing="5" border="0">
                    <tr>
                        <td valign="top"><img src="/images/spacer.gif" border="0" height="1" width="5" /></td>
                        <td valign="top" width="300">
                            <table width="100%" height="100%" cellpadding="0" cellspacing="0" border="0">
                                <tr height="1">
                                    <td><img src="/images/table_topleft.gif" border="0" width="5" height="26"></td>
                                    <td nowrap background="/images/table_top.gif" class="greentableheader" width="100%">Design Requirements<asp:Label ID="lblID" runat="server" /></td>
                                    <td><img src="/images/table_topright.gif" border="0" width="5" height="26"></td>
                                </tr>
                                <tr>
                                    <td background="/images/table_left.gif"><img src="/images/table_left.gif" width="5" height="10"></td>
                                    <td width="100%" bgcolor="#FFFFFF" valign="top">
                                        <table width="100%" cellpadding="4" cellspacing="0" border="0" bgcolor="#f9f9f9">
                                            <tr id="trMnemonic" runat="server">
                                                <td nowrap><asp:Image ID="imgMnemonic" runat="server" ImageUrl="/images/spacer.gif" /></td>
                                                <td nowrap><asp:LinkButton ID="lnkMnemonic" runat="server" Text="Mnemonic:" CommandArgument="2.1.5" OnClick="lnkCrumbs_Click" /></td>
                                                <td width="100%"><asp:Label ID="lblMnemonic" runat="server" Text="---" /></td>
                                            </tr>
                                            <tr id="trEnvironment" runat="server">
                                                <td nowrap><asp:Image ID="imgEnvironment" runat="server" ImageUrl="/images/spacer.gif" /></td>
                                                <td nowrap><asp:LinkButton ID="lnkEnvironment" runat="server" Text="Environment:" CommandArgument="2.1.7" OnClick="lnkCrumbs_Click" /></td>
                                                <td width="100%"><asp:Label ID="lblEnvironment" runat="server" Text="---" /></td>
                                            </tr>
                                            <tr id="trServerType" runat="server">
                                                <td nowrap><asp:Image ID="imgServerType" runat="server" ImageUrl="/images/spacer.gif" /></td>
                                                <td nowrap><asp:LinkButton ID="lnkServerType" runat="server" Text="Server Type:" CommandArgument="2.1.8" OnClick="lnkCrumbs_Click" /></td>
                                                <td width="100%"><asp:Label ID="lblServerType" runat="server" Text="---" /></td>
                                            </tr>
                                            <tr id="trOS" runat="server">
                                                <td nowrap><asp:Image ID="imgOS" runat="server" ImageUrl="/images/spacer.gif" /></td>
                                                <td nowrap><asp:LinkButton ID="lnkOS" runat="server" Text="OS:" CommandArgument="2.1.10" OnClick="lnkCrumbs_Click" /></td>
                                                <td width="100%"><asp:Label ID="lblOS" runat="server" Text="---" /></td>
                                            </tr>
                                            <tr id="trServerSize" runat="server">
                                                <td nowrap><asp:Image ID="imgServerSize" runat="server" ImageUrl="/images/spacer.gif" /></td>
                                                <td nowrap><asp:LinkButton ID="lnkServerSize" runat="server" Text="Server Size:" CommandArgument="2.1.11" OnClick="lnkCrumbs_Click" /></td>
                                                <td width="100%"><asp:Label ID="lblServerSize" runat="server" Text="---" /></td>
                                            </tr>
                                            <tr id="trBackup" runat="server">
                                                <td nowrap><asp:Image ID="imgBackup" runat="server" ImageUrl="/images/spacer.gif" /></td>
                                                <td nowrap><asp:LinkButton ID="lnkBackup" runat="server" Text="Backup:" CommandArgument="2.1.12" OnClick="lnkCrumbs_Click" /></td>
                                                <td width="100%"><asp:Label ID="lblBackup" runat="server" Text="---" /></td>
                                            </tr>
                                            <tr id="trExclusions" runat="server">
                                                <td nowrap><asp:Image ID="imgExclusions" runat="server" ImageUrl="/images/spacer.gif" /></td>
                                                <td nowrap><asp:LinkButton ID="lnkExclusions" runat="server" Text="Backup Exclusions:" CommandArgument="2.1.13" OnClick="lnkCrumbs_Click" /></td>
                                                <td width="100%"><asp:Label ID="lblExclusions" runat="server" Text="---" /></td>
                                            </tr>
                                            <tr id="trMaintenance" runat="server">
                                                <td nowrap><asp:Image ID="imgMaintenance" runat="server" ImageUrl="/images/spacer.gif" /></td>
                                                <td nowrap><asp:LinkButton ID="lnkMaintenance" runat="server" Text="Maintenance Window:" CommandArgument="2.1.14" OnClick="lnkCrumbs_Click" /></td>
                                                <td width="100%"><asp:Label ID="lblMaintenance" runat="server" Text="---" /></td>
                                            </tr>
                                            <tr id="trMainframe" runat="server">
                                                <td nowrap><asp:Image ID="imgMainframe" runat="server" ImageUrl="/images/spacer.gif" /></td>
                                                <td nowrap><asp:LinkButton ID="lnkMainframe" runat="server" Text="Mainframe:" CommandArgument="2.1.15" OnClick="lnkCrumbs_Click" /></td>
                                                <td width="100%"><asp:Label ID="lblMainframe" runat="server" Text="---" /></td>
                                            </tr>
                                            <tr id="trLocation" runat="server">
                                                <td nowrap><asp:Image ID="imgLocation" runat="server" ImageUrl="/images/spacer.gif" /></td>
                                                <td nowrap><asp:LinkButton ID="lnkLocation" runat="server" Text="Location:" CommandArgument="2.1.16" OnClick="lnkCrumbs_Click" /></td>
                                                <td width="100%"><asp:Label ID="lblLocation" runat="server" Text="---" /></td>
                                            </tr>
                                            <tr id="trHA" runat="server">
                                                <td nowrap><asp:Image ID="imgHA" runat="server" ImageUrl="/images/spacer.gif" /></td>
                                                <td nowrap><asp:LinkButton ID="lnkHA" runat="server" Text="High Availability:" CommandArgument="2.1.17" OnClick="lnkCrumbs_Click" /></td>
                                                <td width="100%"><asp:Label ID="lblHA" runat="server" Text="---" /></td>
                                            </tr>
                                            <tr id="trSpecial" runat="server">
                                                <td nowrap><asp:Image ID="imgSpecial" runat="server" ImageUrl="/images/spacer.gif" /></td>
                                                <td nowrap><asp:LinkButton ID="lnkSpecial" runat="server" Text="Special Hardware:" CommandArgument="2.1.20" OnClick="lnkCrumbs_Click" /></td>
                                                <td width="100%"><asp:Label ID="lblSpecial" runat="server" Text="---" /></td>
                                            </tr>
                                            <tr id="trSoftware" runat="server">
                                                <td nowrap><asp:Image ID="imgSoftware" runat="server" ImageUrl="/images/spacer.gif" /></td>
                                                <td nowrap><asp:LinkButton ID="lnkSoftware" runat="server" Text="Ancillary Software:" CommandArgument="2.1.21" OnClick="lnkCrumbs_Click" /></td>
                                                <td width="100%"><asp:Label ID="lblSoftware" runat="server" Text="---" /></td>
                                            </tr>
                                            <tr id="trStorage" runat="server">
                                                <td nowrap><asp:Image ID="imgStorage" runat="server" ImageUrl="/images/spacer.gif" /></td>
                                                <td nowrap><asp:LinkButton ID="lnkStorage" runat="server" Text="Storage:" CommandArgument="2.1.23" OnClick="lnkCrumbs_Click" /></td>
                                                <td width="100%"><asp:Label ID="lblStorage" runat="server" Text="---" /></td>
                                            </tr>
                                            <tr id="trCount" runat="server">
                                                <td nowrap><asp:Image ID="imgCount" runat="server" ImageUrl="/images/spacer.gif" /></td>
                                                <td nowrap><asp:LinkButton ID="lnkCount" runat="server" Text="Count:" CommandArgument="2.1.24" OnClick="lnkCrumbs_Click" /></td>
                                                <td width="100%"><asp:Label ID="lblCount" runat="server" Text="---" /></td>
                                            </tr>
                                            <tr id="trAccounts" runat="server">
                                                <td nowrap><asp:Image ID="imgAccounts" runat="server" ImageUrl="/images/spacer.gif" /></td>
                                                <td nowrap><asp:LinkButton ID="lnkAccounts" runat="server" Text="Accounts:" CommandArgument="2.1.25" OnClick="lnkCrumbs_Click" /></td>
                                                <td width="100%"><asp:Label ID="lblAccounts" runat="server" Text="---" /></td>
                                            </tr>
                                            <tr id="trDate" runat="server">
                                                <td nowrap><asp:Image ID="imgDate" runat="server" ImageUrl="/images/spacer.gif" /></td>
                                                <td nowrap><asp:LinkButton ID="lnkDate" runat="server" Text="Build Date:" CommandArgument="2.1.26" OnClick="lnkCrumbs_Click" /></td>
                                                <td width="100%"><asp:Label ID="lblDate" runat="server" Text="---" /></td>
                                            </tr>
                                            <tr id="trConfidence" runat="server">
                                                <td nowrap><asp:Image ID="imgConfidence" runat="server" ImageUrl="/images/spacer.gif" /></td>
                                                <td nowrap><asp:LinkButton ID="lnkConfidence" runat="server" Text="Confidence Level:" CommandArgument="2.1.27" OnClick="lnkCrumbs_Click" /></td>
                                                <td width="100%"><asp:Label ID="lblConfidence" runat="server" Text="---" /></td>
                                            </tr>
                                            <tr>
                                                <td colspan="3"><img src="/images/spacer.gif" border="0" width="1" height="1" /></td>
                                            </tr>
                                            <tr>
                                                <td nowrap><a href="javascript:void(0);" onclick="alert('The recovery time objective (RTO) is the duration of time and a service level within which a business process must be restored after a disaster (or disruption) in order to avoid unacceptable consequences associated with a break in business continuity.');"><img src="/images/comment.gif" border="0" /></a></td>
                                                <td nowrap>RTO:</td>
                                                <td width="100%"><asp:Label ID="lblRTO" runat="server" Text="---" /></td>
                                            </tr>
                                            <tr>
                                                <td nowrap><asp:ImageButton ID="imgDataCenter" runat="server" ImageUrl="/images/comment.gif" /></td>
                                                <td nowrap>Data Center:</td>
                                                <td width="100%"><asp:Label ID="lblDataCenter" runat="server" Text="---" /></td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td background="/images/table_right.gif"><img src="/images/table_right.gif" width="5" height="10"></td>
                                </tr>
                                <tr height="1">
                                    <td><img src="/images/table_bottomLeft.gif" border="0" width="5" height="9"></td>
                                    <td width="100%" background="/images/table_bottom.gif"></td>
                                    <td><img src="/images/table_bottomRight.gif" border="0" width="5" height="9"></td>
                                </tr>
                            </table>
                        </td>
                        <td valign="top">
                            <table width="100%" height="100%" cellpadding="0" cellspacing="0" border="0">
                                <tr>
                                    <td>
                                        <div style="height:100%; overflow:auto">
                                            <asp:Panel ID="panMnemonic" runat="server" Visible="false">
                                                <table cellpadding="4" cellspacing="0" border="0">
                                                    <tr>
                                                        <td colspan="2" class="biggestbold">Select Mnemonic</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">Enter the mnemonic of the project associated with this design.</td>
                                                    </tr>
                                                    <tr>
                                                        <td><img src="/images/spacer.gif" border="0" height="1" width='<%=strSpacer %>' /></td>
                                                        <td width="100%">
                                                            <table cellpadding="0" cellspacing="0" border="0">
                                                                <tr>
                                                                    <td><asp:TextBox ID="txtMnemonic" runat="server" Width="300" CssClass="default" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <div id="divMnemonic" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                                                                            <asp:ListBox ID="lstMnemonic" runat="server" CssClass="default" />
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <br />
                                                <table cellpadding="4" cellspacing="0" border="0">
                                                    <tr>
                                                        <td colspan="2">Enter the cost center associated with this request (Example: 0012345):</td>
                                                    </tr>
                                                    <tr>
                                                        <td><img src="/images/spacer.gif" border="0" height="1" width='<%=strSpacer %>' /></td>
                                                        <td width="100%">
                                                            <table cellpadding="0" cellspacing="0" border="0">
                                                                <tr>
                                                                    <td><asp:TextBox ID="txtCostCenter" runat="server" Width="200" CssClass="default" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <div id="divCostCenter" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                                                                            <asp:ListBox ID="lstCostCenter" runat="server" CssClass="default" />
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="footer">A list will appear as you begin to type...</td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <br />
                                                <table cellpadding="4" cellspacing="0" border="0">
                                                    <tr>
                                                        <td colspan="2">Enter the name of the Service Integrator working with you on this request:</td>
                                                    </tr>
                                                    <tr>
                                                        <td><img src="/images/spacer.gif" border="0" height="1" width='<%=strSpacer %>' /></td>
                                                        <td width="100%">
                                                            <table cellpadding="0" cellspacing="0" border="0">
                                                                <tr>
                                                                    <td><asp:TextBox ID="txtSI" runat="server" Width="300" CssClass="default" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <div id="divSI" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                                                                            <asp:ListBox ID="lstSI" runat="server" CssClass="default" />
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="footer">A list will appear as you begin to type...</td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                            
                                            
                                            <asp:Panel ID="panEnvironment" runat="server" Visible="false">
                                                <table cellpadding="4" cellspacing="0" border="0">
                                                    <tr>
                                                        <td colspan="2" class="biggestbold">Select Environment</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">Select the environment to be provisioned for this request.</td>
                                                    </tr>
                                                    <tr>
                                                        <td><img src="/images/spacer.gif" border="0" height="1" width='<%=strSpacer %>' /></td>
                                                        <td width="100%"><asp:DropDownList ID="ddlClass" CssClass="default" runat="server" /></td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                            
                                            
                                            <asp:Panel ID="panServerType" runat="server" Visible="false">
                                                <table cellpadding="4" cellspacing="0" border="0">
                                                    <tr>
                                                        <td colspan="2" class="biggestbold">Select Server Type</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">What type of server is this request for?</td>
                                                    </tr>
                                                    <tr>
                                                        <td><img src="/images/spacer.gif" border="0" height="1" width='<%=strSpacer %>' /></td>
                                                        <td width="100%">
                                                            <asp:CheckBox ID="chkDatabase" runat="server" Text="Database" AutoPostBack="true" OnCheckedChanged="chkDatabase_Change" /><br />
                                                            <asp:CheckBox ID="chkWeb" runat="server" Text="Web" />
                                                        </td>
                                                    </tr>
                                                </table>
                                                <br />
                                                <asp:Panel ID="panDatabase" runat="server" Visible="false">
                                                    <table cellpadding="4" cellspacing="0" border="0">
                                                        <tr>
                                                            <td colspan="2" class="biggestbold">Select Database Platform</td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">What Database Platform is required?</td>
                                                        </tr>
                                                        <tr>
                                                            <td><img src="/images/spacer.gif" border="0" height="1" width='<%=strSpacer %>' /></td>
                                                            <td width="100%">
                                                                <asp:RadioButton ID="radOracle" runat="server" Text="Oracle" GroupName="Database" /><br />
                                                                <asp:RadioButton ID="radSQL" runat="server" Text="SQL" GroupName="Database" /><br />
                                                                <asp:RadioButton ID="radOther" runat="server" Text="Other" GroupName="Database" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </asp:Panel>
                                            
                                            
                                            <asp:Panel ID="panOS" runat="server" Visible="false">
                                                <table cellpadding="4" cellspacing="0" border="0">
                                                    <tr>
                                                        <td colspan="2" class="biggestbold">Select Operating System</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">What Operating System is Required?</td>
                                                    </tr>
                                                    <tr>
                                                        <td><img src="/images/spacer.gif" border="0" height="1" width='<%=strSpacer %>' /></td>
                                                        <td width="100%">
                                                            <asp:DropDownList ID="ddlOS" CssClass="default" runat="server">
                                                                <asp:ListItem Value="0" Text="-- SELECT --" />
                                                                <asp:ListItem Value="W" Text="Windows" />
                                                                <asp:ListItem Value="L" Text="Linux" />
                                                                <asp:ListItem Value="S" Text="Solaris" />
                                                                <asp:ListItem Value="A" Text="AIX" />
                                                                <asp:ListItem Value="O" Text="Other" />
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                            
                                            
                                            <asp:Panel ID="panServerSize" runat="server" Visible="false">
                                                <table cellpadding="4" cellspacing="0" border="0">
                                                    <tr>
                                                        <td colspan="2" class="biggestbold">Select Server Size</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">Select your server size:</td>
                                                    </tr>
                                                    <tr>
                                                        <td><img src="/images/spacer.gif" border="0" height="1" width='<%=strSpacer %>' /></td>
                                                        <td width="100%"><asp:DropDownList ID="ddlSize" CssClass="default" runat="server" /></td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                            
                                            
                                            <asp:Panel ID="panBackup" runat="server" Visible="false">
                                                <table cellpadding="4" cellspacing="0" border="0">
                                                    <tr>
                                                        <td colspan="2" class="biggestbold">Select Backup Window</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">Select your backup frequency:</td>
                                                    </tr>
                                                    <tr>
                                                        <td><img src="/images/spacer.gif" border="0" height="1" width='<%=strSpacer %>' /></td>
                                                        <td width="100%">
                                                            <asp:RadioButton ID="radBackupDaily" runat="server" Text="Daily" GroupName="Backup" /><br />
                                                            <asp:RadioButton ID="radBackupWeekly" runat="server" Text="Weekly" GroupName="Backup" /><br />
                                                            <asp:RadioButton ID="radBackupMonthly" runat="server" Text="Monthly" GroupName="Backup" />
                                                        </td>
                                                    </tr>
                                                </table>
                                                <br />
                                                <table cellpadding="4" cellspacing="0" border="0">
                                                    <tr>
                                                        <td colspan="2"><b>NOTE:</b> The following table is an Exclusion Table for all times when a back-up is NOT acceptable. Select all conflicting times.</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" align="right">
                                                            <table>
                                                                <tr>
                                                                    <td><img src="/images/check.gif" border="0" /></td>
                                                                    <td> = Backup Acceptable</td>
                                                                </tr>
                                                                <tr>
                                                                    <td><img src="/images/cancel.gif" border="0" /></td>
                                                                    <td> = Backup Unacceptable</td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td><img src="/images/spacer.gif" border="0" height="1" width='<%=strSpacer %>' /></td>
                                                        <td width="100%">
                                                            <table cellpadding="6" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                                                                <tr bgcolor="#EEEEEE">
                                                                    <td>Time:</td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflictsDay(this,'hdnBackup',1);">Sun</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflictsDay(this,'hdnBackup',2);">Mon</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflictsDay(this,'hdnBackup',3);">Tue</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflictsDay(this,'hdnBackup',4);">Wed</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflictsDay(this,'hdnBackup',5);">Thu</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflictsDay(this,'hdnBackup',6);">Fri</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflictsDay(this,'hdnBackup',7);">Sat</a></td>
                                                                </tr>
                                                                <tr>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflictsTime(this,'hdnBackup','12:00 AM');">12:00 AM</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',1,'12:00 AM');" class='<%=strGridBackupSun[0] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',2,'12:00 AM');" class='<%=strGridBackupMon[0] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',3,'12:00 AM');" class='<%=strGridBackupTue[0] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',4,'12:00 AM');" class='<%=strGridBackupWed[0] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',5,'12:00 AM');" class='<%=strGridBackupThu[0] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',6,'12:00 AM');" class='<%=strGridBackupFri[0] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',7,'12:00 AM');" class='<%=strGridBackupSat[0] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                </tr>
                                                                <tr>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflictsTime(this,'hdnBackup','1:00 AM');">1:00 AM</td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',1,'1:00 AM');" class='<%=strGridBackupSun[1] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',2,'1:00 AM');" class='<%=strGridBackupMon[1] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',3,'1:00 AM');" class='<%=strGridBackupTue[1] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',4,'1:00 AM');" class='<%=strGridBackupWed[1] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',5,'1:00 AM');" class='<%=strGridBackupThu[1] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',6,'1:00 AM');" class='<%=strGridBackupFri[1] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',7,'1:00 AM');" class='<%=strGridBackupSat[1] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                </tr>
                                                                <tr>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflictsTime(this,'hdnBackup','2:00 AM');">2:00 AM</td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',1,'2:00 AM');" class='<%=strGridBackupSun[2] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',2,'2:00 AM');" class='<%=strGridBackupMon[2] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',3,'2:00 AM');" class='<%=strGridBackupTue[2] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',4,'2:00 AM');" class='<%=strGridBackupWed[2] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',5,'2:00 AM');" class='<%=strGridBackupThu[2] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',6,'2:00 AM');" class='<%=strGridBackupFri[2] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',7,'2:00 AM');" class='<%=strGridBackupSat[2] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                </tr>
                                                                <tr>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflictsTime(this,'hdnBackup','3:00 AM');">3:00 AM</td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',1,'3:00 AM');" class='<%=strGridBackupSun[3] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',2,'3:00 AM');" class='<%=strGridBackupMon[3] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',3,'3:00 AM');" class='<%=strGridBackupTue[3] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',4,'3:00 AM');" class='<%=strGridBackupWed[3] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',5,'3:00 AM');" class='<%=strGridBackupThu[3] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',6,'3:00 AM');" class='<%=strGridBackupFri[3] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',7,'3:00 AM');" class='<%=strGridBackupSat[3] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                </tr>
                                                                <tr>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflictsTime(this,'hdnBackup','4:00 AM');">4:00 AM</td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',1,'4:00 AM');" class='<%=strGridBackupSun[4] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',2,'4:00 AM');" class='<%=strGridBackupMon[4] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',3,'4:00 AM');" class='<%=strGridBackupTue[4] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',4,'4:00 AM');" class='<%=strGridBackupWed[4] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',5,'4:00 AM');" class='<%=strGridBackupThu[4] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',6,'4:00 AM');" class='<%=strGridBackupFri[4] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',7,'4:00 AM');" class='<%=strGridBackupSat[4] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                </tr>
                                                                <tr>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflictsTime(this,'hdnBackup','5:00 AM');">5:00 AM</td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',1,'5:00 AM');" class='<%=strGridBackupSun[5] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',2,'5:00 AM');" class='<%=strGridBackupMon[5] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',3,'5:00 AM');" class='<%=strGridBackupTue[5] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',4,'5:00 AM');" class='<%=strGridBackupWed[5] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',5,'5:00 AM');" class='<%=strGridBackupThu[5] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',6,'5:00 AM');" class='<%=strGridBackupFri[5] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',7,'5:00 AM');" class='<%=strGridBackupSat[5] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                </tr>
                                                                <tr>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflictsTime(this,'hdnBackup','6:00 AM');">6:00 AM</td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',1,'6:00 AM');" class='<%=strGridBackupSun[6] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',2,'6:00 AM');" class='<%=strGridBackupMon[6] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',3,'6:00 AM');" class='<%=strGridBackupTue[6] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',4,'6:00 AM');" class='<%=strGridBackupWed[6] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',5,'6:00 AM');" class='<%=strGridBackupThu[6] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',6,'6:00 AM');" class='<%=strGridBackupFri[6] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',7,'6:00 AM');" class='<%=strGridBackupSat[6] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                </tr>
                                                                <tr>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflictsTime(this,'hdnBackup','7:00 AM');">7:00 AM</td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',1,'7:00 AM');" class='<%=strGridBackupSun[7] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',2,'7:00 AM');" class='<%=strGridBackupMon[7] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',3,'7:00 AM');" class='<%=strGridBackupTue[7] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',4,'7:00 AM');" class='<%=strGridBackupWed[7] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',5,'7:00 AM');" class='<%=strGridBackupThu[7] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',6,'7:00 AM');" class='<%=strGridBackupFri[7] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',7,'7:00 AM');" class='<%=strGridBackupSat[7] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                </tr>
                                                                <tr>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflictsTime(this,'hdnBackup','8:00 AM');">8:00 AM</td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',1,'8:00 AM');" class='<%=strGridBackupSun[8] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',2,'8:00 AM');" class='<%=strGridBackupMon[8] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',3,'8:00 AM');" class='<%=strGridBackupTue[8] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',4,'8:00 AM');" class='<%=strGridBackupWed[8] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',5,'8:00 AM');" class='<%=strGridBackupThu[8] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',6,'8:00 AM');" class='<%=strGridBackupFri[8] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',7,'8:00 AM');" class='<%=strGridBackupSat[8] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                </tr>
                                                                <tr>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflictsTime(this,'hdnBackup','9:00 AM');">9:00 AM</td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',1,'9:00 AM');" class='<%=strGridBackupSun[9] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',2,'9:00 AM');" class='<%=strGridBackupMon[9] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',3,'9:00 AM');" class='<%=strGridBackupTue[9] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',4,'9:00 AM');" class='<%=strGridBackupWed[9] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',5,'9:00 AM');" class='<%=strGridBackupThu[9] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',6,'9:00 AM');" class='<%=strGridBackupFri[9] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',7,'9:00 AM');" class='<%=strGridBackupSat[9] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                </tr>
                                                                <tr>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflictsTime(this,'hdnBackup','10:00 AM');">10:00 AM</td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',1,'10:00 AM');" class='<%=strGridBackupSun[10] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',2,'10:00 AM');" class='<%=strGridBackupMon[10] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',3,'10:00 AM');" class='<%=strGridBackupTue[10] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',4,'10:00 AM');" class='<%=strGridBackupWed[10] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',5,'10:00 AM');" class='<%=strGridBackupThu[10] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',6,'10:00 AM');" class='<%=strGridBackupFri[10] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',7,'10:00 AM');" class='<%=strGridBackupSat[10] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                </tr>
                                                                <tr>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflictsTime(this,'hdnBackup','11:00 AM');">11:00 AM</td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',1,'11:00 AM');" class='<%=strGridBackupSun[11] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',2,'11:00 AM');" class='<%=strGridBackupMon[11] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',3,'11:00 AM');" class='<%=strGridBackupTue[11] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',4,'11:00 AM');" class='<%=strGridBackupWed[11] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',5,'11:00 AM');" class='<%=strGridBackupThu[11] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',6,'11:00 AM');" class='<%=strGridBackupFri[11] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',7,'11:00 AM');" class='<%=strGridBackupSat[11] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                </tr>
                                                                <tr>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflictsTime(this,'hdnBackup','12:00 PM');">12:00 PM</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',1,'12:00 PM');" class='<%=strGridBackupSun[12] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',2,'12:00 PM');" class='<%=strGridBackupMon[12] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',3,'12:00 PM');" class='<%=strGridBackupTue[12] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',4,'12:00 PM');" class='<%=strGridBackupWed[12] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',5,'12:00 PM');" class='<%=strGridBackupThu[12] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',6,'12:00 PM');" class='<%=strGridBackupFri[12] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',7,'12:00 PM');" class='<%=strGridBackupSat[12] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                </tr>
                                                                <tr>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflictsTime(this,'hdnBackup','1:00 PM');">1:00 PM</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',1,'1:00 PM');" class='<%=strGridBackupSun[13] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',2,'1:00 PM');" class='<%=strGridBackupMon[13] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',3,'1:00 PM');" class='<%=strGridBackupTue[13] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',4,'1:00 PM');" class='<%=strGridBackupWed[13] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',5,'1:00 PM');" class='<%=strGridBackupThu[13] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',6,'1:00 PM');" class='<%=strGridBackupFri[13] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',7,'1:00 PM');" class='<%=strGridBackupSat[13] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                </tr>
                                                                <tr>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflictsTime(this,'hdnBackup','2:00 PM');">2:00 PM</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',1,'2:00 PM');" class='<%=strGridBackupSun[14] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',2,'2:00 PM');" class='<%=strGridBackupMon[14] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',3,'2:00 PM');" class='<%=strGridBackupTue[14] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',4,'2:00 PM');" class='<%=strGridBackupWed[14] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',5,'2:00 PM');" class='<%=strGridBackupThu[14] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',6,'2:00 PM');" class='<%=strGridBackupFri[14] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',7,'2:00 PM');" class='<%=strGridBackupSat[14] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                </tr>
                                                                <tr>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflictsTime(this,'hdnBackup','3:00 PM');">3:00 PM</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',1,'3:00 PM');" class='<%=strGridBackupSun[15] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',2,'3:00 PM');" class='<%=strGridBackupMon[15] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',3,'3:00 PM');" class='<%=strGridBackupTue[15] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',4,'3:00 PM');" class='<%=strGridBackupWed[15] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',5,'3:00 PM');" class='<%=strGridBackupThu[15] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',6,'3:00 PM');" class='<%=strGridBackupFri[15] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',7,'3:00 PM');" class='<%=strGridBackupSat[15] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                </tr>
                                                                <tr>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflictsTime(this,'hdnBackup','4:00 PM');">4:00 PM</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',1,'4:00 PM');" class='<%=strGridBackupSun[16] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',2,'4:00 PM');" class='<%=strGridBackupMon[16] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',3,'4:00 PM');" class='<%=strGridBackupTue[16] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',4,'4:00 PM');" class='<%=strGridBackupWed[16] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',5,'4:00 PM');" class='<%=strGridBackupThu[16] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',6,'4:00 PM');" class='<%=strGridBackupFri[16] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',7,'4:00 PM');" class='<%=strGridBackupSat[16] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                </tr>
                                                                <tr>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflictsTime(this,'hdnBackup','5:00 PM');">5:00 PM</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',1,'5:00 PM');" class='<%=strGridBackupSun[17] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',2,'5:00 PM');" class='<%=strGridBackupMon[17] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',3,'5:00 PM');" class='<%=strGridBackupTue[17] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',4,'5:00 PM');" class='<%=strGridBackupWed[17] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',5,'5:00 PM');" class='<%=strGridBackupThu[17] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',6,'5:00 PM');" class='<%=strGridBackupFri[17] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',7,'5:00 PM');" class='<%=strGridBackupSat[17] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                </tr>
                                                                <tr>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflictsTime(this,'hdnBackup','6:00 PM');">6:00 PM</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',1,'6:00 PM');" class='<%=strGridBackupSun[18] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',2,'6:00 PM');" class='<%=strGridBackupMon[18] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',3,'6:00 PM');" class='<%=strGridBackupTue[18] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',4,'6:00 PM');" class='<%=strGridBackupWed[18] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',5,'6:00 PM');" class='<%=strGridBackupThu[18] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',6,'6:00 PM');" class='<%=strGridBackupFri[18] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',7,'6:00 PM');" class='<%=strGridBackupSat[18] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                </tr>
                                                                <tr>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflictsTime(this,'hdnBackup','7:00 PM');">7:00 PM</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',1,'7:00 PM');" class='<%=strGridBackupSun[19] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',2,'7:00 PM');" class='<%=strGridBackupMon[19] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',3,'7:00 PM');" class='<%=strGridBackupTue[19] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',4,'7:00 PM');" class='<%=strGridBackupWed[19] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',5,'7:00 PM');" class='<%=strGridBackupThu[19] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',6,'7:00 PM');" class='<%=strGridBackupFri[19] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',7,'7:00 PM');" class='<%=strGridBackupSat[19] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                </tr>
                                                                <tr>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflictsTime(this,'hdnBackup','8:00 PM');">8:00 PM</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',1,'8:00 PM');" class='<%=strGridBackupSun[20] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',2,'8:00 PM');" class='<%=strGridBackupMon[20] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',3,'8:00 PM');" class='<%=strGridBackupTue[20] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',4,'8:00 PM');" class='<%=strGridBackupWed[20] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',5,'8:00 PM');" class='<%=strGridBackupThu[20] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',6,'8:00 PM');" class='<%=strGridBackupFri[20] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',7,'8:00 PM');" class='<%=strGridBackupSat[20] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                </tr>
                                                                <tr>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflictsTime(this,'hdnBackup','9:00 PM');">9:00 PM</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',1,'9:00 PM');" class='<%=strGridBackupSun[21] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',2,'9:00 PM');" class='<%=strGridBackupMon[21] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',3,'9:00 PM');" class='<%=strGridBackupTue[21] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',4,'9:00 PM');" class='<%=strGridBackupWed[21] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',5,'9:00 PM');" class='<%=strGridBackupThu[21] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',6,'9:00 PM');" class='<%=strGridBackupFri[21] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',7,'9:00 PM');" class='<%=strGridBackupSat[21] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                </tr>
                                                                <tr>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflictsTime(this,'hdnBackup','10:00 PM');">10:00 PM</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',1,'10:00 PM');" class='<%=strGridBackupSun[22] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',2,'10:00 PM');" class='<%=strGridBackupMon[22] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',3,'10:00 PM');" class='<%=strGridBackupTue[22] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',4,'10:00 PM');" class='<%=strGridBackupWed[22] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',5,'10:00 PM');" class='<%=strGridBackupThu[22] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',6,'10:00 PM');" class='<%=strGridBackupFri[22] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',7,'10:00 PM');" class='<%=strGridBackupSat[22] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                </tr>
                                                                <tr>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflictsTime(this,'hdnBackup','11:00 PM');">11:00 PM</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',1,'11:00 PM');" class='<%=strGridBackupSun[23] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',2,'11:00 PM');" class='<%=strGridBackupMon[23] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',3,'11:00 PM');" class='<%=strGridBackupTue[23] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',4,'11:00 PM');" class='<%=strGridBackupWed[23] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',5,'11:00 PM');" class='<%=strGridBackupThu[23] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',6,'11:00 PM');" class='<%=strGridBackupFri[23] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnBackup',7,'11:00 PM');" class='<%=strGridBackupSat[23] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                            
                                            
                                            <asp:Panel ID="panExclusions" runat="server" Visible="false">
                                                <table cellpadding="4" cellspacing="0" border="0">
                                                    <tr>
                                                        <td colspan="2" class="biggestbold">Set Backup Exclusions</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">Specify (if) any exclusions that must be included in the backup procedures.</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">Enter specific files by using the complete file name and path (ex: C:\Application\Sub Directory\File.ext), whole directories may be included by not specifying the file name (ex: C:\Application\Sub Directory\)</td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap>Path:</td>
                                                        <td width="100%"><asp:TextBox ID="txtPath" runat="server" Width="300" CssClass="default" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td nowrap><asp:Button ID="btnAddExclusion" runat="server" CssClass="default" Width="125" OnClick="btnAddExclusion_Click" Text="Add Exclusion" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <table width="600" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                                                                <tr bgcolor="#EEEEEE">
                                                                    <td><b><u>Path:</u></b></td>
                                                                    <td></td>
                                                                </tr>
                                                                <asp:repeater ID="rptExclusions" runat="server">
                                                                    <ItemTemplate>
                                                                        <tr>
                                                                            <td valign="top" width="100%"><%# DataBinder.Eval(Container.DataItem, "path") %></td>
                                                                            <td valign="top" align="right"><asp:LinkButton ID="btnDeleteExclusion" runat="server" Text="Delete" OnClick="btnDeleteExclusion_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' /></td>
                                                                        </tr>
                                                                    </ItemTemplate>
                                                                    <AlternatingItemTemplate>
                                                                        <tr bgcolor="F6F6F6">
                                                                            <td valign="top" width="100%"><%# DataBinder.Eval(Container.DataItem, "path") %></td>
                                                                            <td valign="top" align="right"><asp:LinkButton ID="btnDeleteExclusion" runat="server" Text="Delete" OnClick="btnDeleteExclusion_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' /></td>
                                                                        </tr>
                                                                    </AlternatingItemTemplate>
                                                                </asp:repeater>
                                                                <tr>
                                                                    <td colspan="2">
                                                                        <asp:Label ID="lblExclusion" runat="server" CssClass="default" Visible="false" Text="<img src='/images/spacer.gif' border='0' height='1' width='25' /><img src='/images/alert.gif' border='0' align='absmiddle'> There are no backup exclusions" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                            
                                            
                                            <asp:Panel ID="panMaintenance" runat="server" Visible="false">
                                                <table cellpadding="4" cellspacing="0" border="0">
                                                    <tr>
                                                        <td colspan="2" class="biggestbold">Select Maintenance Window</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2"><b>NOTE:</b> The following table is an Exclusion Table for all times when routine maintenance is NOT acceptable. Select all conflicting times.</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" align="right">
                                                            <table>
                                                                <tr>
                                                                    <td><img src="/images/check.gif" border="0" /></td>
                                                                    <td> = Maintenance Acceptable</td>
                                                                </tr>
                                                                <tr>
                                                                    <td><img src="/images/cancel.gif" border="0" /></td>
                                                                    <td> = Maintenance Unacceptable</td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td><img src="/images/spacer.gif" border="0" height="1" width='<%=strSpacer %>' /></td>
                                                        <td width="100%">
                                                            <table cellpadding="6" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                                                                <tr bgcolor="#EEEEEE">
                                                                    <td>Time:</td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflictsDay(this,'hdnMaintenance',1);">Sun</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflictsDay(this,'hdnMaintenance',2);">Mon</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflictsDay(this,'hdnMaintenance',3);">Tue</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflictsDay(this,'hdnMaintenance',4);">Wed</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflictsDay(this,'hdnMaintenance',5);">Thu</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflictsDay(this,'hdnMaintenance',6);">Fri</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflictsDay(this,'hdnMaintenance',7);">Sat</a></td>
                                                                </tr>
                                                                <tr>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflictsTime(this,'hdnMaintenance','12:00 AM');">12:00 AM</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',1,'12:00 AM');" class='<%=strGridMaintenanceSun[0] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',2,'12:00 AM');" class='<%=strGridMaintenanceMon[0] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',3,'12:00 AM');" class='<%=strGridMaintenanceTue[0] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',4,'12:00 AM');" class='<%=strGridMaintenanceWed[0] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',5,'12:00 AM');" class='<%=strGridMaintenanceThu[0] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',6,'12:00 AM');" class='<%=strGridMaintenanceFri[0] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',7,'12:00 AM');" class='<%=strGridMaintenanceSat[0] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                </tr>
                                                                <tr>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflictsTime(this,'hdnMaintenance','1:00 AM');">1:00 AM</td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',1,'1:00 AM');" class='<%=strGridMaintenanceSun[1] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',2,'1:00 AM');" class='<%=strGridMaintenanceMon[1] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',3,'1:00 AM');" class='<%=strGridMaintenanceTue[1] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',4,'1:00 AM');" class='<%=strGridMaintenanceWed[1] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',5,'1:00 AM');" class='<%=strGridMaintenanceThu[1] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',6,'1:00 AM');" class='<%=strGridMaintenanceFri[1] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',7,'1:00 AM');" class='<%=strGridMaintenanceSat[1] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                </tr>
                                                                <tr>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflictsTime(this,'hdnMaintenance','2:00 AM');">2:00 AM</td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',1,'2:00 AM');" class='<%=strGridMaintenanceSun[2] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',2,'2:00 AM');" class='<%=strGridMaintenanceMon[2] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',3,'2:00 AM');" class='<%=strGridMaintenanceTue[2] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',4,'2:00 AM');" class='<%=strGridMaintenanceWed[2] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',5,'2:00 AM');" class='<%=strGridMaintenanceThu[2] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',6,'2:00 AM');" class='<%=strGridMaintenanceFri[2] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',7,'2:00 AM');" class='<%=strGridMaintenanceSat[2] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                </tr>
                                                                <tr>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflictsTime(this,'hdnMaintenance','3:00 AM');">3:00 AM</td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',1,'3:00 AM');" class='<%=strGridMaintenanceSun[3] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',2,'3:00 AM');" class='<%=strGridMaintenanceMon[3] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',3,'3:00 AM');" class='<%=strGridMaintenanceTue[3] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',4,'3:00 AM');" class='<%=strGridMaintenanceWed[3] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',5,'3:00 AM');" class='<%=strGridMaintenanceThu[3] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',6,'3:00 AM');" class='<%=strGridMaintenanceFri[3] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',7,'3:00 AM');" class='<%=strGridMaintenanceSat[3] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                </tr>
                                                                <tr>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflictsTime(this,'hdnMaintenance','4:00 AM');">4:00 AM</td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',1,'4:00 AM');" class='<%=strGridMaintenanceSun[4] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',2,'4:00 AM');" class='<%=strGridMaintenanceMon[4] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',3,'4:00 AM');" class='<%=strGridMaintenanceTue[4] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',4,'4:00 AM');" class='<%=strGridMaintenanceWed[4] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',5,'4:00 AM');" class='<%=strGridMaintenanceThu[4] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',6,'4:00 AM');" class='<%=strGridMaintenanceFri[4] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',7,'4:00 AM');" class='<%=strGridMaintenanceSat[4] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                </tr>
                                                                <tr>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflictsTime(this,'hdnMaintenance','5:00 AM');">5:00 AM</td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',1,'5:00 AM');" class='<%=strGridMaintenanceSun[5] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',2,'5:00 AM');" class='<%=strGridMaintenanceMon[5] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',3,'5:00 AM');" class='<%=strGridMaintenanceTue[5] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',4,'5:00 AM');" class='<%=strGridMaintenanceWed[5] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',5,'5:00 AM');" class='<%=strGridMaintenanceThu[5] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',6,'5:00 AM');" class='<%=strGridMaintenanceFri[5] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',7,'5:00 AM');" class='<%=strGridMaintenanceSat[5] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                </tr>
                                                                <tr>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflictsTime(this,'hdnMaintenance','6:00 AM');">6:00 AM</td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',1,'6:00 AM');" class='<%=strGridMaintenanceSun[6] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',2,'6:00 AM');" class='<%=strGridMaintenanceMon[6] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',3,'6:00 AM');" class='<%=strGridMaintenanceTue[6] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',4,'6:00 AM');" class='<%=strGridMaintenanceWed[6] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',5,'6:00 AM');" class='<%=strGridMaintenanceThu[6] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',6,'6:00 AM');" class='<%=strGridMaintenanceFri[6] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',7,'6:00 AM');" class='<%=strGridMaintenanceSat[6] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                </tr>
                                                                <tr>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflictsTime(this,'hdnMaintenance','7:00 AM');">7:00 AM</td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',1,'7:00 AM');" class='<%=strGridMaintenanceSun[7] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',2,'7:00 AM');" class='<%=strGridMaintenanceMon[7] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',3,'7:00 AM');" class='<%=strGridMaintenanceTue[7] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',4,'7:00 AM');" class='<%=strGridMaintenanceWed[7] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',5,'7:00 AM');" class='<%=strGridMaintenanceThu[7] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',6,'7:00 AM');" class='<%=strGridMaintenanceFri[7] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',7,'7:00 AM');" class='<%=strGridMaintenanceSat[7] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                </tr>
                                                                <tr>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflictsTime(this,'hdnMaintenance','8:00 AM');">8:00 AM</td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',1,'8:00 AM');" class='<%=strGridMaintenanceSun[8] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',2,'8:00 AM');" class='<%=strGridMaintenanceMon[8] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',3,'8:00 AM');" class='<%=strGridMaintenanceTue[8] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',4,'8:00 AM');" class='<%=strGridMaintenanceWed[8] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',5,'8:00 AM');" class='<%=strGridMaintenanceThu[8] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',6,'8:00 AM');" class='<%=strGridMaintenanceFri[8] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',7,'8:00 AM');" class='<%=strGridMaintenanceSat[8] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                </tr>
                                                                <tr>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflictsTime(this,'hdnMaintenance','9:00 AM');">9:00 AM</td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',1,'9:00 AM');" class='<%=strGridMaintenanceSun[9] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',2,'9:00 AM');" class='<%=strGridMaintenanceMon[9] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',3,'9:00 AM');" class='<%=strGridMaintenanceTue[9] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',4,'9:00 AM');" class='<%=strGridMaintenanceWed[9] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',5,'9:00 AM');" class='<%=strGridMaintenanceThu[9] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',6,'9:00 AM');" class='<%=strGridMaintenanceFri[9] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',7,'9:00 AM');" class='<%=strGridMaintenanceSat[9] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                </tr>
                                                                <tr>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflictsTime(this,'hdnMaintenance','10:00 AM');">10:00 AM</td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',1,'10:00 AM');" class='<%=strGridMaintenanceSun[10] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',2,'10:00 AM');" class='<%=strGridMaintenanceMon[10] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',3,'10:00 AM');" class='<%=strGridMaintenanceTue[10] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',4,'10:00 AM');" class='<%=strGridMaintenanceWed[10] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',5,'10:00 AM');" class='<%=strGridMaintenanceThu[10] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',6,'10:00 AM');" class='<%=strGridMaintenanceFri[10] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',7,'10:00 AM');" class='<%=strGridMaintenanceSat[10] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                </tr>
                                                                <tr>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflictsTime(this,'hdnMaintenance','11:00 AM');">11:00 AM</td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',1,'11:00 AM');" class='<%=strGridMaintenanceSun[11] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',2,'11:00 AM');" class='<%=strGridMaintenanceMon[11] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',3,'11:00 AM');" class='<%=strGridMaintenanceTue[11] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',4,'11:00 AM');" class='<%=strGridMaintenanceWed[11] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',5,'11:00 AM');" class='<%=strGridMaintenanceThu[11] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',6,'11:00 AM');" class='<%=strGridMaintenanceFri[11] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',7,'11:00 AM');" class='<%=strGridMaintenanceSat[11] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                </tr>
                                                                <tr>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflictsTime(this,'hdnMaintenance','12:00 PM');">12:00 PM</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',1,'12:00 PM');" class='<%=strGridMaintenanceSun[12] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',2,'12:00 PM');" class='<%=strGridMaintenanceMon[12] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',3,'12:00 PM');" class='<%=strGridMaintenanceTue[12] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',4,'12:00 PM');" class='<%=strGridMaintenanceWed[12] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',5,'12:00 PM');" class='<%=strGridMaintenanceThu[12] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',6,'12:00 PM');" class='<%=strGridMaintenanceFri[12] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',7,'12:00 PM');" class='<%=strGridMaintenanceSat[12] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                </tr>
                                                                <tr>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflictsTime(this,'hdnMaintenance','1:00 PM');">1:00 PM</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',1,'1:00 PM');" class='<%=strGridMaintenanceSun[13] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',2,'1:00 PM');" class='<%=strGridMaintenanceMon[13] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',3,'1:00 PM');" class='<%=strGridMaintenanceTue[13] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',4,'1:00 PM');" class='<%=strGridMaintenanceWed[13] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',5,'1:00 PM');" class='<%=strGridMaintenanceThu[13] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',6,'1:00 PM');" class='<%=strGridMaintenanceFri[13] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',7,'1:00 PM');" class='<%=strGridMaintenanceSat[13] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                </tr>
                                                                <tr>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflictsTime(this,'hdnMaintenance','2:00 PM');">2:00 PM</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',1,'2:00 PM');" class='<%=strGridMaintenanceSun[14] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',2,'2:00 PM');" class='<%=strGridMaintenanceMon[14] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',3,'2:00 PM');" class='<%=strGridMaintenanceTue[14] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',4,'2:00 PM');" class='<%=strGridMaintenanceWed[14] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',5,'2:00 PM');" class='<%=strGridMaintenanceThu[14] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',6,'2:00 PM');" class='<%=strGridMaintenanceFri[14] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',7,'2:00 PM');" class='<%=strGridMaintenanceSat[14] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                </tr>
                                                                <tr>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflictsTime(this,'hdnMaintenance','3:00 PM');">3:00 PM</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',1,'3:00 PM');" class='<%=strGridMaintenanceSun[15] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',2,'3:00 PM');" class='<%=strGridMaintenanceMon[15] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',3,'3:00 PM');" class='<%=strGridMaintenanceTue[15] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',4,'3:00 PM');" class='<%=strGridMaintenanceWed[15] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',5,'3:00 PM');" class='<%=strGridMaintenanceThu[15] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',6,'3:00 PM');" class='<%=strGridMaintenanceFri[15] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',7,'3:00 PM');" class='<%=strGridMaintenanceSat[15] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                </tr>
                                                                <tr>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflictsTime(this,'hdnMaintenance','4:00 PM');">4:00 PM</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',1,'4:00 PM');" class='<%=strGridMaintenanceSun[16] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',2,'4:00 PM');" class='<%=strGridMaintenanceMon[16] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',3,'4:00 PM');" class='<%=strGridMaintenanceTue[16] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',4,'4:00 PM');" class='<%=strGridMaintenanceWed[16] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',5,'4:00 PM');" class='<%=strGridMaintenanceThu[16] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',6,'4:00 PM');" class='<%=strGridMaintenanceFri[16] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',7,'4:00 PM');" class='<%=strGridMaintenanceSat[16] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                </tr>
                                                                <tr>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflictsTime(this,'hdnMaintenance','5:00 PM');">5:00 PM</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',1,'5:00 PM');" class='<%=strGridMaintenanceSun[17] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',2,'5:00 PM');" class='<%=strGridMaintenanceMon[17] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',3,'5:00 PM');" class='<%=strGridMaintenanceTue[17] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',4,'5:00 PM');" class='<%=strGridMaintenanceWed[17] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',5,'5:00 PM');" class='<%=strGridMaintenanceThu[17] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',6,'5:00 PM');" class='<%=strGridMaintenanceFri[17] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',7,'5:00 PM');" class='<%=strGridMaintenanceSat[17] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                </tr>
                                                                <tr>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflictsTime(this,'hdnMaintenance','6:00 PM');">6:00 PM</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',1,'6:00 PM');" class='<%=strGridMaintenanceSun[18] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',2,'6:00 PM');" class='<%=strGridMaintenanceMon[18] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',3,'6:00 PM');" class='<%=strGridMaintenanceTue[18] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',4,'6:00 PM');" class='<%=strGridMaintenanceWed[18] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',5,'6:00 PM');" class='<%=strGridMaintenanceThu[18] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',6,'6:00 PM');" class='<%=strGridMaintenanceFri[18] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',7,'6:00 PM');" class='<%=strGridMaintenanceSat[18] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                </tr>
                                                                <tr>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflictsTime(this,'hdnMaintenance','7:00 PM');">7:00 PM</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',1,'7:00 PM');" class='<%=strGridMaintenanceSun[19] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',2,'7:00 PM');" class='<%=strGridMaintenanceMon[19] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',3,'7:00 PM');" class='<%=strGridMaintenanceTue[19] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',4,'7:00 PM');" class='<%=strGridMaintenanceWed[19] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',5,'7:00 PM');" class='<%=strGridMaintenanceThu[19] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',6,'7:00 PM');" class='<%=strGridMaintenanceFri[19] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',7,'7:00 PM');" class='<%=strGridMaintenanceSat[19] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                </tr>
                                                                <tr>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflictsTime(this,'hdnMaintenance','8:00 PM');">8:00 PM</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',1,'8:00 PM');" class='<%=strGridMaintenanceSun[20] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',2,'8:00 PM');" class='<%=strGridMaintenanceMon[20] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',3,'8:00 PM');" class='<%=strGridMaintenanceTue[20] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',4,'8:00 PM');" class='<%=strGridMaintenanceWed[20] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',5,'8:00 PM');" class='<%=strGridMaintenanceThu[20] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',6,'8:00 PM');" class='<%=strGridMaintenanceFri[20] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',7,'8:00 PM');" class='<%=strGridMaintenanceSat[20] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                </tr>
                                                                <tr>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflictsTime(this,'hdnMaintenance','9:00 PM');">9:00 PM</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',1,'9:00 PM');" class='<%=strGridMaintenanceSun[21] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',2,'9:00 PM');" class='<%=strGridMaintenanceMon[21] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',3,'9:00 PM');" class='<%=strGridMaintenanceTue[21] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',4,'9:00 PM');" class='<%=strGridMaintenanceWed[21] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',5,'9:00 PM');" class='<%=strGridMaintenanceThu[21] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',6,'9:00 PM');" class='<%=strGridMaintenanceFri[21] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',7,'9:00 PM');" class='<%=strGridMaintenanceSat[21] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                </tr>
                                                                <tr>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflictsTime(this,'hdnMaintenance','10:00 PM');">10:00 PM</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',1,'10:00 PM');" class='<%=strGridMaintenanceSun[22] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',2,'10:00 PM');" class='<%=strGridMaintenanceMon[22] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',3,'10:00 PM');" class='<%=strGridMaintenanceTue[22] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',4,'10:00 PM');" class='<%=strGridMaintenanceWed[22] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',5,'10:00 PM');" class='<%=strGridMaintenanceThu[22] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',6,'10:00 PM');" class='<%=strGridMaintenanceFri[22] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',7,'10:00 PM');" class='<%=strGridMaintenanceSat[22] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                </tr>
                                                                <tr>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflictsTime(this,'hdnMaintenance','11:00 PM');">11:00 PM</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',1,'11:00 PM');" class='<%=strGridMaintenanceSun[23] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',2,'11:00 PM');" class='<%=strGridMaintenanceMon[23] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',3,'11:00 PM');" class='<%=strGridMaintenanceTue[23] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',4,'11:00 PM');" class='<%=strGridMaintenanceWed[23] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',5,'11:00 PM');" class='<%=strGridMaintenanceThu[23] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',6,'11:00 PM');" class='<%=strGridMaintenanceFri[23] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                    <td><a href="javascript:void(0);" onclick="SetConflict(this,'hdnMaintenance',7,'11:00 PM');" class='<%=strGridMaintenanceSat[23] == '1' ? "selectGridCheck" : "selectGridCancel" %>'</a></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                            
                                            
                                            <asp:Panel ID="panMainframe" runat="server" Visible="false">
                                                <table cellpadding="4" cellspacing="0" border="0">
                                                    <tr>
                                                        <td colspan="2" class="biggestbold">Mainframe Requirements Selection</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">Does the Server or Storage in this request require communications with the Mainframe?</td>
                                                    </tr>
                                                    <tr>
                                                        <td><img src="/images/spacer.gif" border="0" height="1" width='<%=strSpacer %>' /></td>
                                                        <td width="100%">
                                                            <asp:RadioButton ID="radMainframe_Yes" runat="server" Text="Yes" GroupName="Mainframe" /><br />
                                                            <asp:RadioButton ID="radMainframe_No" runat="server" Text="No" GroupName="Mainframe" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                            
                                            
                                            <asp:Panel ID="panLocation" runat="server" Visible="false">
                                                <table cellpadding="4" cellspacing="0" border="0">
                                                    <tr>
                                                        <td colspan="2" class="biggestbold">Select Server Location</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">Select the Server Location for this Request:</td>
                                                    </tr>
                                                    <tr>
                                                        <td><img src="/images/spacer.gif" border="0" height="1" width='<%=strSpacer %>' /></td>
                                                        <td width="100%"><asp:DropDownList ID="ddlLocation" CssClass="default" runat="server" /></td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                            
                                            
                                            <asp:Panel ID="panHA" runat="server" Visible="false">
                                                <table cellpadding="4" cellspacing="0" border="0">
                                                    <tr>
                                                        <td colspan="2" class="biggestbold">Select High Availability Requirements</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">Does the solution require high availability, i.e. load balancing?</td>
                                                    </tr>
                                                    <tr>
                                                        <td><img src="/images/spacer.gif" border="0" height="1" width='<%=strSpacer %>' /></td>
                                                        <td width="100%">
                                                            <asp:RadioButton ID="radHA_Yes" runat="server" Text="Yes" GroupName="Database" OnCheckedChanged="radHADetail_Change" AutoPostBack="true" /><br />
                                                            <asp:RadioButton ID="radHA_No" runat="server" Text="No" GroupName="Database" OnCheckedChanged="radHADetail_Change" AutoPostBack="true" />
                                                        </td>
                                                    </tr>
                                                </table>
                                                <asp:Panel ID="panHADetail" runat="server" Visible="false">
                                                    <br /><br />
                                                    <table cellpadding="4" cellspacing="0" border="0">
                                                        <tr>
                                                            <td colspan="2">Select the High Availability for this Request:</td>
                                                        </tr>
                                                        <tr>
                                                            <td><img src="/images/spacer.gif" border="0" height="1" width='<%=strSpacer %>' /></td>
                                                            <td width="100%">
                                                                <asp:RadioButtonList ID="radHA" CssClass="default" runat="server" AutoPostBack="true" OnSelectedIndexChanged="radHA_Change">
                                                                    <asp:ListItem Value="Clustered" />
                                                                    <asp:ListItem Value="Network Load Balancing" />
                                                                </asp:RadioButtonList>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <asp:Panel ID="panCluster" runat="server" Visible="false">
                                                        <br /><br />
                                                        <table cellpadding="4" cellspacing="0" border="0">
                                                            <tr>
                                                                <td colspan="2">Specify the clustering requirement type:</td>
                                                            </tr>
                                                            <tr>
                                                                <td><img src="/images/spacer.gif" border="0" height="1" width='<%=strSpacer %>' /></td>
                                                                <td width="100%">
                                                                    <asp:RadioButtonList ID="radCluster" runat="server">
                                                                        <asp:ListItem Text="Active / Passive" Value="A / P" />
                                                                        <asp:ListItem Text="Active / Active" Value="A / A" />
                                                                    </asp:RadioButtonList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">Enter the number of instances:</td>
                                                            </tr>
                                                            <tr>
                                                                <td><img src="/images/spacer.gif" border="0" height="1" width='<%=strSpacer %>' /></td>
                                                                <td width="100%"><asp:TextBox ID="txtInstances" CssClass="default" runat="server" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">Enter the number of nodes (per instance):</td>
                                                            </tr>
                                                            <tr>
                                                                <td><img src="/images/spacer.gif" border="0" height="1" width='<%=strSpacer %>' /></td>
                                                                <td width="100%"><asp:TextBox ID="txtNodes" CssClass="default" runat="server" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">Enter the size of the Quorum drive:</td>
                                                            </tr>
                                                            <tr>
                                                                <td><img src="/images/spacer.gif" border="0" height="1" width='<%=strSpacer %>' /></td>
                                                                <td width="100%"><asp:TextBox ID="txtQuorum" CssClass="default" runat="server" Width="50" MaxLength="10" /> GB</td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                    <asp:Panel ID="panLoadBalanced" runat="server" Visible="false">
                                                        <br /><br />
                                                        <table cellpadding="4" cellspacing="0" border="0">
                                                            <tr>
                                                                <td colspan="2">Will the application software installed on the server be used for middleware/messaging/protocol translation?</td>
                                                            </tr>
                                                            <tr>
                                                                <td><img src="/images/spacer.gif" border="0" height="1" width='<%=strSpacer %>' /></td>
                                                                <td width="100%">
                                                                    <asp:RadioButton ID="radMiddlewareYes" runat="server" Text="Yes" GroupName="Middleware" /><br />
                                                                    <asp:RadioButton ID="radMiddlewareNo" runat="server" Text="No" GroupName="Middleware" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <asp:Panel ID="panLoadBalancedApplication" runat="server" Visible="false">
                                                            <table cellpadding="4" cellspacing="0" border="0">
                                                                <tr>
                                                                    <td colspan="2">Will application tier software be running on the server(s)?</td>
                                                                </tr>
                                                                <tr>
                                                                    <td><img src="/images/spacer.gif" border="0" height="1" width='<%=strSpacer %>' /></td>
                                                                    <td width="100%">
                                                                        <asp:RadioButton ID="radApplicationYes" runat="server" Text="Yes" GroupName="Application" /><br />
                                                                        <asp:RadioButton ID="radApplicationNo" runat="server" Text="No" GroupName="Application" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                    </asp:Panel>
                                                </asp:Panel>
                                            </asp:Panel>
                                            
                                            
                                            <asp:Panel ID="panSpecial" runat="server" Visible="false">
                                                <table cellpadding="4" cellspacing="0" border="0">
                                                    <tr>
                                                        <td colspan="2" class="biggestbold">Select Special Hardware</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">Does this requirement have any special hardware requirements?</td>
                                                    </tr>
                                                    <tr>
                                                        <td><img src="/images/spacer.gif" border="0" height="1" width='<%=strSpacer %>' /></td>
                                                        <td width="100%">
                                                            <asp:CheckBoxList ID="chkSpecial" runat="server">
                                                                <asp:ListItem Value="None" />
                                                                <asp:ListItem Value="Special Hardware" />
                                                                <asp:ListItem Value="USB Dongles" />
                                                                <asp:ListItem Value="Special Network Requirements" />
                                                                <asp:ListItem Value="Other" />
                                                            </asp:CheckBoxList>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                            
                                            
                                            <asp:Panel ID="panSoftware" runat="server" Visible="false">
                                                <table cellpadding="4" cellspacing="0" border="0">
                                                    <tr>
                                                        <td colspan="2" class="biggestbold">Select Ancillary Software</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">Select if any of the following are necessary:</td>
                                                    </tr>
                                                    <tr>
                                                        <td><img src="/images/spacer.gif" border="0" height="1" width='<%=strSpacer %>' /></td>
                                                        <td width="100%">
                                                            <asp:CheckBox ID="chkNDM" runat="server" Text="NDM / Connect Direct" /><br />
                                                            <asp:CheckBox ID="chkCA7" runat="server" Text="CA7" AutoPostBack="true" OnCheckedChanged="chkCA7_Change" />
                                                        </td>
                                                    </tr>
                                                </table>
                                                <br />
                                                <asp:Panel ID="panCA7" runat="server" Visible="false">
                                                    <table cellpadding="4" cellspacing="0" border="0">
                                                    <tr>
                                                        <td colspan="2" class="biggestbold">CA7 Requirements</td>
                                                    </tr>
                                                        <tr>
                                                            <td colspan="2">PLACEHOLDER: CA7 Requirements</td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </asp:Panel>
                                            
                                            
                                            <asp:Panel ID="panStorage" runat="server" Visible="false">
                                                <table cellpadding="4" cellspacing="0" border="0">
                                                    <tr>
                                                        <td colspan="2" class="biggestbold">Storage Requirements</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">Does this request require any additional storage?</td>
                                                    </tr>
                                                    <tr>
                                                        <td><img src="/images/spacer.gif" border="0" height="1" width='<%=strSpacer %>' /></td>
                                                        <td width="100%">
                                                            <asp:RadioButton ID="radStorageYes" runat="server" Text="Yes" GroupName="Storage" OnCheckedChanged="radStorage_Change" AutoPostBack="true" /><br />
                                                            <asp:RadioButton ID="radStroageNo" runat="server" Text="No" GroupName="Storage" OnCheckedChanged="radStorage_Change" AutoPostBack="true" />
                                                        </td>
                                                    </tr>
                                                </table>
                                                <asp:Panel ID="panStorageYes" runat="server" Visible="false">
                                                    <br />
                                                    <table cellpadding="4" cellspacing="0" border="0">
                                                        <tr>
                                                            <td colspan="2">What type of storage do you require?</td>
                                                        </tr>
                                                        <tr>
                                                            <td><img src="/images/spacer.gif" border="0" height="1" width='<%=strSpacer %>' /></td>
                                                            <td width="100%">
                                                                <asp:RadioButton ID="radPersistent" runat="server" Text="Persistent Data Storage" GroupName="Persistent" OnCheckedChanged="radPersistent_Change" AutoPostBack="true" /><br />
                                                                <asp:RadioButton ID="radPersistentNon" runat="server" Text="Non-Persistent Data Storage" GroupName="Persistent" OnCheckedChanged="radPersistent_Change" AutoPostBack="true" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <asp:Panel ID="panPersistent" runat="server" Visible="false">
                                                        <br />
                                                        <asp:Panel ID="panStorageDatabase" runat="server" Visible="false">
                                                            <table width="100%" cellpadding="4" cellspacing="0" border="0">
                                                                <tr>
                                                                    <td colspan="2" class="biggerbold">SQL Server Storage Calculator</td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2">Enter the total database size:</td>
                                                                </tr>
                                                                <tr>
                                                                    <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
                                                                    <td width="100%"><asp:TextBox ID="txtDatabaseSize" runat="server" CssClass="default" Width="100" MaxLength="10" /> GB</td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2">What % of the total space is the largest Table and/or Index:</td>
                                                                </tr>
                                                                <tr>
                                                                    <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
                                                                    <td width="100%"><asp:TextBox ID="txtDatabasePercent" runat="server" CssClass="default" Width="100" MaxLength="10" /> %&nbsp;&nbsp;&nbsp;<span class="footer">(0 - 100)</span></td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2">Are you using a non-standard TempDB size?</td>
                                                                </tr>
                                                                <tr>
                                                                    <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
                                                                    <td width="100%">
                                                                        <asp:RadioButton ID="radTempDBYes" runat="server" CssClass="default" Text="Yes" GroupName="TempDB" AutoPostBack="true" OnCheckedChanged="radTempDB_Change" /> 
                                                                        <asp:RadioButton ID="radTempDBNo" runat="server" CssClass="default" Text="No (Default)" GroupName="TempDB" AutoPostBack="true" OnCheckedChanged="radTempDB_Change" Checked="true" /> 
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <asp:Panel ID="panTempDB" runat="server" Visible="false">
                                                                <table width="100%" cellpadding="4" cellspacing="0" border="0">
                                                                    <tr>
                                                                        <td colspan="2">Enter your custom TempDB size:</td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
                                                                        <td width="100%"><asp:TextBox ID="txtDatabaseTemp" runat="server" CssClass="default" Width="100" MaxLength="10" /> GB</td>
                                                                    </tr>
                                                                </table>
                                                            </asp:Panel>
                                                            <table width="100%" cellpadding="4" cellspacing="0" border="0">
                                                                <tr>
                                                                    <td colspan="2">Will you require to store non-database data on the same instance?</td>
                                                                </tr>
                                                                <tr>
                                                                    <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
                                                                    <td width="100%">
                                                                        <asp:RadioButton ID="radDatabaseNonYes" runat="server" CssClass="default" Text="Yes" GroupName="nonPNC" AutoPostBack="true" OnCheckedChanged="radDatabaseNon_Change" /> 
                                                                        <asp:RadioButton ID="radDatabaseNonNo" runat="server" CssClass="default" Text="No (Default)" GroupName="nonPNC" AutoPostBack="true" OnCheckedChanged="radDatabaseNon_Change" Checked="true" /> 
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <asp:Panel ID="panDatabaseNon" runat="server" Visible="false">
                                                                <table width="100%" cellpadding="4" cellspacing="0" border="0">
                                                                    <tr>
                                                                        <td colspan="2">Enter amount of storage required to store non-database data:</td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
                                                                        <td width="100%"><asp:TextBox ID="txtDatabaseNon" runat="server" CssClass="default" Width="100" MaxLength="10" /> GB</td>
                                                                    </tr>
                                                                </table>
                                                            </asp:Panel>
                                                        </asp:Panel>
                                                        
                                                        <asp:Panel ID="panStorageLUNs" runat="server" Visible="false">
                                                            <div style="height:100%; width:100%; overflow:auto">
                                                                <table width="100%" cellpadding="4" cellspacing="0" border="0" align="center" style="border:solid 1px #CCCCCC">
                                                                    <tr bgcolor="#EEEEEE">
                                                                        <td style='display:<%=boolWindows ? "inline" : "none" %>' nowrap><b><u>Drive:</u></b></td>
                                                                        <td style='display:<%=boolWindows ? "inline" : "none" %>' width="100%"><b><u>Mount Point:</u></b></td>
                                                                        <td style='display:<%=boolWindows == false ? "inline" : "none" %>' width="100%"><b><u>Filesystem:</u></b></td>
                                                                        <td nowrap><b><u>Shared:</u></b></td>
                                                                        <td nowrap><b><u>Size:</u></b></td>
                                                                        <td nowrap></td>
                                                                    </tr>
                                                                    <tr id="trStorageOSWindows" runat="server" visible="false">
                                                                        <td nowrap>C:\,D:\</td>
                                                                        <td width="100%">&nbsp;&nbsp;<span class="footer">(Reserved: Operating System)</span></td>
                                                                        <td nowrap><input type="checkbox" disabled="disabled" /></td>
                                                                        <td nowrap><asp:TextBox ID="TextBox1" runat="server" Width="50" MaxLength="10" /> GB</td>
                                                                        <td nowrap>
                                                                            <asp:ImageButton ID="ImageButton1" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/save_icon.jpg" CommandArgument="APP" OnClick="btnStorageSave_Click" /> 
                                                                            <img src="/images/spacer.gif" border="0" height="16" width="16" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trStorageApp" runat="server" visible="false">
                                                                        <td nowrap>E:\**</td>
                                                                        <td width="100%">&nbsp;&nbsp;<span class="footer">(Reserved: Application Drive)</span></td>
                                                                        <td nowrap><input type="checkbox" disabled="disabled" /></td>
                                                                        <td nowrap><asp:TextBox ID="txtStorageSizeE" runat="server" Width="50" MaxLength="10" /> GB</td>
                                                                        <td nowrap>
                                                                            <asp:ImageButton ID="btnStorageSaveSizeE" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/save_icon.jpg" CommandArgument="APP" OnClick="btnStorageSave_Click" /> 
                                                                            <img src="/images/spacer.gif" border="0" height="16" width="16" />
                                                                        </td>
                                                                    </tr>
                                                                    <asp:repeater ID="rptStorage" runat="server">
                                                                        <ItemTemplate>
                                                                            <tr>
                                                                                <td style='display:<%=boolWindows ? "inline" : "none" %>' valign="top" nowrap><%# DataBinder.Eval(Container.DataItem, "letter") %></td>
                                                                                <td valign="top" width="100%"><asp:TextBox ID="txtStoragePath" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "path") %>' Width="200" /></td>
                                                                                <td valign="top" nowrap><asp:CheckBox ID="chkStorageSize" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "shared") %>' /></td>
                                                                                <td valign="top" nowrap><asp:TextBox ID="txtStorageSize" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "size") %>' Width="50" MaxLength="10" /> GB</td>
                                                                                <td valign="top" nowrap align="right">
                                                                                    <asp:ImageButton ID="btnStorageSave" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/save_icon.jpg" CommandArgument="SAVE" CommandName='<%# DataBinder.Eval(Container.DataItem, "id") %>' OnClick="btnStorageSave_Click" /> 
                                                                                    <asp:ImageButton ID="btnStorageDelete" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/cancel.gif" CommandArgument="DELETE" CommandName='<%# DataBinder.Eval(Container.DataItem, "id") %>' OnClick="btnStorageSave_Click" /> 
                                                                                </td>
                                                                            </tr>
                                                                        </ItemTemplate>
                                                                    </asp:repeater>
                                                                    <tr id="trStorageDrive" runat="server" visible="false">
                                                                        <td style='display:<%=boolWindows ? "inline" : "none" %>' nowrap><asp:DropDownList ID="ddlStorageDrive" runat="server" /></td>
                                                                        <td width="100%"><asp:TextBox ID="txtStoragePathNew" runat="server" Width="200" /></td>
                                                                        <td nowrap><asp:CheckBox ID="chkStorageSizeNew" runat="server" /></td>
                                                                        <td nowrap><asp:TextBox ID="txtStorageSizeNew" runat="server" Width="50" MaxLength="10" /> GB</td>
                                                                        <td nowrap>
                                                                            <asp:ImageButton ID="btnStorageSaveNew" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/save_icon.jpg" CommandArgument="NEW" OnClick="btnStorageSave_Click" /> 
                                                                            <asp:ImageButton ID="btnStorageDeleteNew" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/cancel.gif" CommandArgument="CANCEL" OnClick="btnStorageSave_Click" /> 
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trStorageDriveNew" runat="server">
                                                                        <td colspan="10">
                                                                            <asp:LinkButton ID="btnStorageDriveAdd" runat="server" Text="<img src='/images/green_arrow.gif' border='0' align='absmiddle'/> Add More Storage" OnClick="btnStorageDriveAdd_Click" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                                <div align="right">** = If you do not require an application drive, set it to zero (0) GB.</div>
                                                            </div>
                                                        </asp:Panel>
                                                        <table width="100%" cellpadding="4" cellspacing="0" border="0">
                                                            <tr>
                                                                <td colspan="2">
                                                                    <asp:Button ID="btnGenerate" runat="server" Text="Generate Storage" Width="125" OnClick="btnGenerate_Click" Visible="false" />
                                                                    <asp:Button ID="btnReset" runat="server" Text="Reset Storage" Width="125" OnClick="btnReset_Click" Visible="false" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                    <asp:Panel ID="panPersistentNon" runat="server" Visible="false">
                                                        <br /><br />
                                                        <table cellpadding="4" cellspacing="0" border="0">
                                                            <tr>
                                                                <td colspan="2">Enter the amount of non-persistent storage:</td>
                                                            </tr>
                                                            <tr>
                                                                <td><img src="/images/spacer.gif" border="0" height="1" width='<%=strSpacer %>' /></td>
                                                                <td width="100%"><asp:TextBox ID="txtStorage" runat="server" Width="100" /> GB</td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                </asp:Panel>
                                            </asp:Panel>
                                            
                                            
                                            <asp:Panel ID="panCount" runat="server" Visible="false">
                                                <table cellpadding="4" cellspacing="0" border="0">
                                                    <tr>
                                                        <td colspan="2" class="biggestbold">Select Quantity</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">Select how many servers are required:</td>
                                                    </tr>
                                                    <tr>
                                                        <td><img src="/images/spacer.gif" border="0" height="1" width='<%=strSpacer %>' /></td>
                                                        <td width="100%">
                                                            <asp:DropDownList ID="ddlCount" CssClass="default" runat="server">
                                                                <asp:ListItem Value="-- SELECT --" />
                                                                <asp:ListItem Value="1" />
                                                                <asp:ListItem Value="2" />
                                                                <asp:ListItem Value="3" />
                                                                <asp:ListItem Value="4" />
                                                                <asp:ListItem Value="5" />
                                                                <asp:ListItem Value="6" />
                                                                <asp:ListItem Value="7" />
                                                                <asp:ListItem Value="8" />
                                                                <asp:ListItem Value="9" />
                                                                <asp:ListItem Value="10" />
                                                                <asp:ListItem Value="11" />
                                                                <asp:ListItem Value="12" />
                                                                <asp:ListItem Value="13" />
                                                                <asp:ListItem Value="14" />
                                                                <asp:ListItem Value="15" />
                                                                <asp:ListItem Value="16" Text="More than 15..." />
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                            
                                            
                                            <asp:Panel ID="panAccounts" runat="server" Visible="false">
                                                <table cellpadding="4" cellspacing="0" border="0">
                                                    <tr>
                                                        <td colspan="2" class="biggestbold">Specify User Accounts</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">Enter your account configuration:</td>
                                                    </tr>
                                                    <tr>
                                                        <td><img src="/images/spacer.gif" border="0" height="1" width="5" /></td>
                                                        <td width="100%">
                                                            <table width="100%" height="100%" cellpadding="0" cellspacing="0" border="0">
                                                                <tr height="1">
                                                                    <td><img src="/images/table_topleft.gif" border="0" width="5" height="26"></td>
                                                                    <td nowrap background="/images/table_top.gif" class="greentableheader" width="100%">New Account</td>
                                                                    <td><img src="/images/table_topright.gif" border="0" width="5" height="26"></td>
                                                                </tr>
                                                                <tr>
                                                                    <td background="/images/table_left.gif"><img src="/images/table_left.gif" width="5" height="10"></td>
                                                                    <td width="100%" bgcolor="#FFFFFF" valign="top">
                                                                        <table width="100%" border="0" cellspacing="4" cellpadding="4">
                                                                            <tr>
                                                                                <td nowrap>User:</td>
                                                                                <td width="100%">
                                                                                    <table cellpadding="0" cellspacing="0" border="0">
                                                                                        <tr>
                                                                                            <td><asp:TextBox ID="txtUser" runat="server" Width="300" CssClass="default" /></td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td>
                                                                                                <div id="divAJAX" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                                                                                                    <asp:ListBox ID="lstAJAX" runat="server" CssClass="default" />
                                                                                                </div>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td nowrap class="footer">&nbsp;</td>
                                                                                <td class="footer">(Enter a valid LAN ID, First Name, or Last Name)</td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td></td>
                                                                                <td nowrap><asp:LinkButton ID="btnManager" runat="server" Text="User Not Appearing in the List? Click Here." /></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>Permission:</td>
                                                                                <td nowrap>
                                                                                    <asp:DropDownList ID="ddlPermission" runat="server">
                                                                                        <asp:ListItem Value="0" Text="-- SELECT --" />
                                                                                        <asp:ListItem Value="D" Text="Developer" />
                                                                                        <asp:ListItem Value="P" Text="Promoter" />
                                                                                        <asp:ListItem Value="S" Text="AppSupport" />
                                                                                        <asp:ListItem Value="U" Text="AppUsers" />
                                                                                    </asp:DropDownList>&nbsp;&nbsp;&nbsp;
                                                                                    <asp:CheckBox ID="chkRemoteDesktop" runat="server" Text="With Remote Desktop" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td></td>
                                                                                <td nowrap></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td></td>
                                                                                <td nowrap><asp:Button ID="btnAddAccount" runat="server" CssClass="default" Width="125" OnClick="btnAddAccount_Click" Text="Add Account" /></td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                    <td background="/images/table_right.gif"><img src="/images/table_right.gif" width="5" height="10"></td>
                                                                </tr>
                                                                <tr height="1">
                                                                    <td><img src="/images/table_bottomLeft.gif" border="0" width="5" height="9"></td>
                                                                    <td width="100%" background="/images/table_bottom.gif"></td>
                                                                    <td><img src="/images/table_bottomRight.gif" border="0" width="5" height="9"></td>
                                                                </tr>
                                                            </table>
                                                            <br />
                                                            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                                                <tr>
                                                                    <td><img src="/images/table_topleft.gif" border="0" width="5" height="26"></td>
                                                                    <td nowrap background="/images/table_top.gif" class="greentableheader" width="100%">Current Account Requests</td>
                                                                    <td><img src="/images/table_topright.gif" border="0" width="5" height="26"></td>
                                                                </tr>
                                                                <tr>
                                                                    <td background="/images/table_left.gif"><img src="/images/table_left.gif" width="5" height="10"></td>
                                                                    <td width="100%" bgcolor="#FFFFFF">
                                                                        <div style="height:100%; width:100%; overflow:auto">
                                                                            <table width="100%" cellpadding="4" cellspacing="0" border="0" align="center">
                                                                                <tr>
                                                                                    <td><b><u>User:</u></b></td>
                                                                                    <td><b><u>Permission:</u></b></td>
                                                                                    <td></td>
                                                                                </tr>
                                                                                <asp:repeater ID="rptAccounts" runat="server">
                                                                                    <ItemTemplate>
                                                                                        <tr>
                                                                                            <td valign="top"><%# oUser.GetFullName(Int32.Parse(DataBinder.Eval(Container.DataItem, "userid").ToString())) %> (<%# oUser.GetName(Int32.Parse(DataBinder.Eval(Container.DataItem, "userid").ToString()))%>)</td>
                                                                                            <td valign="top"><asp:Label ID="lblPermissions" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "access") %>' ToolTip='<%# DataBinder.Eval(Container.DataItem, "remote") %>' /></td>
                                                                                            <td valign="top" align="right"><asp:LinkButton ID="btnDeleteAccount" runat="server" Text="Delete" OnClick="btnDeleteAccount_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' /></td>
                                                                                        </tr>
                                                                                    </ItemTemplate>
                                                                                    <AlternatingItemTemplate>
                                                                                        <tr bgcolor="F6F6F6">
                                                                                            <td valign="top"><%# oUser.GetFullName(Int32.Parse(DataBinder.Eval(Container.DataItem, "userid").ToString()))%> (<%# oUser.GetName(Int32.Parse(DataBinder.Eval(Container.DataItem, "userid").ToString()))%>)</td>
                                                                                            <td valign="top"><asp:Label ID="lblPermissions" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "access") %>' ToolTip='<%# DataBinder.Eval(Container.DataItem, "remote") %>' /></td>
                                                                                            <td valign="top" align="right"><asp:LinkButton ID="btnDeleteAccount" runat="server" Text="Delete" OnClick="btnDeleteAccount_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' /></td>
                                                                                        </tr>
                                                                                    </AlternatingItemTemplate>
                                                                                </asp:repeater>
                                                                                <tr>
                                                                                    <td colspan="2">
                                                                                        <asp:Label ID="lblNone" runat="server" CssClass="default" Visible="false" Text="<img src='/images/spacer.gif' border='0' height='1' width='25' /><img src='/images/alert.gif' border='0' align='absmiddle'> You have not added any accounts to this request" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </div>
                                                                    </td>
                                                                    <td background="/images/table_right.gif"><img src="/images/table_right.gif" width="5" height="10"></td>
                                                                </tr>
                                                                <tr>
                                                                    <td><img src="/images/table_bottomLeft.gif" border="0" width="5" height="9"></td>
                                                                    <td width="100%" background="/images/table_bottom.gif"></td>
                                                                    <td><img src="/images/table_bottomRight.gif" border="0" width="5" height="9"></td>
                                                                </tr>
                                                            </table>
                                                            <asp:CheckBox ID="chkNotify" runat="server" CssClass="default" Text="Email me the results of this account request" Checked="true" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                            
                                            
                                            <asp:Panel ID="panDate" runat="server" Visible="false">
                                                <table cellpadding="4" cellspacing="0" border="0">
                                                    <tr>
                                                        <td colspan="2" class="biggestbold">Set Build Date</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">What date does the server need to be ready for application configuration:</td>
                                                    </tr>
                                                    <tr>
                                                        <td><img src="/images/spacer.gif" border="0" height="1" width='<%=strSpacer %>' /></td>
                                                        <td width="100%"><asp:TextBox ID="txtDate" runat="server" Width="80" MaxLength="10" /> <asp:ImageButton ID="imgCalendar" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                            
                                            
                                            <asp:Panel ID="panConfidence" runat="server" Visible="false">
                                                <table cellpadding="4" cellspacing="0" border="0">
                                                    <tr>
                                                        <td colspan="2" class="biggestbold">Set Confidence Level</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">Indicate your level of confidence in the accuracy and validity of these recorded requirements:</td>
                                                    </tr>
                                                    <tr>
                                                        <td><img src="/images/spacer.gif" border="0" height="1" width='<%=strSpacer %>' /></td>
                                                        <td width="100%"><asp:DropDownList ID="ddlConfidence" CssClass="default" runat="server" /></td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                            
                                            
                                        </div>
                                    </td>
                                </tr>
                                <tr height="1">
                                    <td>
                                        <asp:Panel ID="panHelpAccounts" runat="server" Visible="false">
                                            <table width="100%" cellpadding="5" cellspacing="0" border="0">
                                                <tr id="hlpAccounts" runat="server" style="display:none">
                                                    <td>
                                                        <table border="0" cellspacing="2" cellpadding="2">
                                                            <tr>
                                                                <td class="bold" valign="top">Developer:</td>
                                                                <td>Require access for application code development and testing</td>
                                                            </tr>
                                                            <tr>
                                                                <td class="bold" valign="top">Promoter:</td>
                                                                <td>Responsible for promoting application code into production</td>
                                                            </tr>
                                                            <tr>
                                                                <td class="bold" valign="top">AppSupport:</td>
                                                                <td>Require access to support a business application (typically MIS but not limited to MIS)</td>
                                                            </tr>
                                                            <tr>
                                                                <td class="bold" valign="top">AppUser:</td>
                                                                <td>Require application user access</td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right"><img src="/images/help24.gif" border="0" align="absmiddle" /> <asp:LinkButton ID="btnHelpAccounts" runat="server" Text="Show Help" /></td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="panSummary" runat="server" Visible="false">
                <table height="100%" width="100%" cellpadding="0" cellspacing="0" border="0">
                    <tr height="1">
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td align="center">
                            <table width="95%" height="100%" cellpadding="0" cellspacing="0" border="0">
                                <tr height="1">
                                    <td><img src="/images/table_topleft.gif" border="0" width="5" height="26"></td>
                                    <td nowrap background="/images/table_top.gif" class="greentableheader" width="100%">Design Summary</td>
                                    <td><img src="/images/table_topright.gif" border="0" width="5" height="26"></td>
                                </tr>
                                <tr>
                                    <td background="/images/table_left.gif"><img src="/images/table_left.gif" width="5" height="10"></td>
                                    <td width="100%" bgcolor="#FFFFFF" valign="top">
                                        <div style="height:100%; overflow:auto">
                                            <table width="100%" cellpadding="5" cellspacing="0" border="0" bgcolor="#f9f9f9">
                                                <tr>
                                                    <td nowrap class="bold"><asp:Label ID="lblValMnemonic" runat="server" Text="Mnemonic:" /></td>
                                                    <td colspan="3" width="100%"><asp:Label ID="lblSummaryMnemonic" runat="server" /></td>
                                                </tr>
                                                <tr>
                                                    <td nowrap class="bold"><asp:Label ID="lblValLocation" runat="server" Text="Location:" /></td>
                                                    <td colspan="3" width="100%"><asp:Label ID="lblSummaryLocation" runat="server" /></td>
                                                </tr>
                                                <tr>
                                                    <td nowrap class="bold"><asp:Label ID="lblValServerType" runat="server" Text="Server Type:" /></td>
                                                    <td width="50%"><asp:Label ID="lblSummaryServerType" runat="server" /></td>
                                                    <td nowrap class="bold"><asp:Label ID="lblValQuantity" runat="server" Text="Quantity:" /></td>
                                                    <td width="50%"><asp:Label ID="lblSummaryQuantity" runat="server" /></td>
                                                </tr>
                                                <tr>
                                                    <td nowrap class="bold"><asp:Label ID="lblValOS" runat="server" Text="Operating System:" /></td>
                                                    <td width="50%"><asp:Label ID="lblSummaryOS" runat="server" /></td>
                                                    <td nowrap class="bold"><asp:Label ID="lblValStorage" runat="server" Text="Storage:" /></td>
                                                    <td width="50%"><asp:Label ID="lblSummaryStorage" runat="server" /></td>
                                                </tr>
                                                <tr>
                                                    <td nowrap class="bold"><asp:Label ID="lblValConfidence" runat="server" Text="Confidence:" /></td>
                                                    <td width="50%"><asp:Label ID="lblSummaryConfidence" runat="server" /></td>
                                                    <td nowrap class="bold"><asp:Label ID="lblValAccounts" runat="server" Text="Accounts:" /></td>
                                                    <td width="50%"><asp:Label ID="lblSummaryAccounts" runat="server" /></td>
                                                </tr>
                                                <tr id="trSummarySize" runat="server">
                                                    <td nowrap class="bold"><asp:Label ID="lblValSize" runat="server" Text="Size:" /></td>
                                                    <td width="50%"><asp:Label ID="lblSummarySize" runat="server" /></td>
                                                    <td nowrap class="bold"><asp:Label ID="lblValExclusion" runat="server" Text="Exclusions:" /></td>
                                                    <td width="50%"><asp:Label ID="lblSummaryExclusion" runat="server" /></td>
                                                </tr>
                                                <tr id="trSummaryHA" runat="server">
                                                    <td nowrap class="bold"><asp:Label ID="lblValHA" runat="server" Text="High Availability:" /></td>
                                                    <td colspan="3" width="100%"><asp:Label ID="lblSummaryHA" runat="server" /></td>
                                                </tr>
                                                <tr id="trSummaryBackup" runat="server">
                                                    <td nowrap class="bold"><asp:Label ID="lblValBackup" runat="server" Text="Backup:" /></td>
                                                    <td colspan="3" width="100%"><asp:Label ID="lblSummaryBackup" runat="server" /></td>
                                                </tr>
                                                <tr id="trSummaryMainframe" runat="server">
                                                    <td nowrap class="bold"><asp:Label ID="lblValMainframe" runat="server" Text="Mainframe:" /></td>
                                                    <td colspan="3" width="100%"><asp:Label ID="lblSummaryMainframe" runat="server" /></td>
                                                </tr>
                                                <tr>
                                                    <td nowrap class="bold"><asp:Label ID="lblValSpecial" runat="server" Text="Special Hardware:" /></td>
                                                    <td width="50%"><asp:Label ID="lblSummarySpecial" runat="server" /></td>
                                                    <td nowrap class="bold"><asp:Label ID="lblValDate" runat="server" Text="Build Date:" /></td>
                                                    <td width="50%"><asp:Label ID="lblSummaryDate" runat="server" /></td>
                                                </tr>
                                                </tr>
                                                <tr>
                                                    <td colspan="4" width="100%">&nbsp;</td>
                                                </tr>
                                <tr>
                                    <td colspan="4" class="biggerbold">Per the PNC reference architecture, the following solution has been selected:</td>
                                </tr>
                                                <tr bgcolor='<%=strHighlight %>'>
                                                    <td nowrap class="bold">Solution:</td>
                                                    <td colspan="3" width="100%"><asp:Label ID="lblSummarySolution" runat="server" /></td>
                                                </tr>
                                                <tr>
                                                    <td nowrap class="bold">Target Completion Date:</td>
                                                    <td colspan="3" width="100%"><asp:Label ID="lblSummaryTarget" runat="server" /></td>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                    <td background="/images/table_right.gif"><img src="/images/table_right.gif" width="5" height="10"></td>
                                </tr>
                                <tr height="1">
                                    <td><img src="/images/table_bottomLeft.gif" border="0" width="5" height="9"></td>
                                    <td width="100%" background="/images/table_bottom.gif"></td>
                                    <td><img src="/images/table_bottomRight.gif" border="0" width="5" height="9"></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr height="1">
                        <td align="center">
                            <table width="95%" cellpadding="0" cellspacing="0" border="0">
                                <tr>
                                    <td align="center">
                                        <table width="100%" cellpadding="0" cellspacing="5" border="0">
                                            <tr>
                                                <td colspan="2"><b>Attest this solution by selecting one of the following:</b></td>
                                            </tr>
                                            <tr>
                                                <td colspan="2"><asp:RadioButton ID="radComplete" runat="server" Text="Complete - I agree" GroupName="attest" /></td>
                                            </tr>
                                            <tr>
                                                <td colspan="2"><asp:RadioButton ID="radChange" runat="server" Text="Make Changes - I forgot something" GroupName="attest" /></td>
                                            </tr>
                                            <tr>
                                                <td colspan="2"><asp:RadioButton ID="radException" runat="server" Text="This does not fit my needs - I need an exception" GroupName="attest" /></td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td><asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" Text="Submit" CssClass="default" Width="75" /></td>
                                                <td align="right" class="biggerbold"><a href="javascript:void(0);" onclick="window.print();"><img src="/images/print-icon.gif" border="0" align="absmiddle" /><img src="/images/spacer.gif" border="0" width="5" height="1" />Print Design</a></td>
                                            </tr>
                                        </table>
                                        <asp:Panel ID="panApproval" runat="server" Visible="false">
                                            <br />        
                                            <table cellpadding="5" cellspacing="5" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
                                                <tr>
                                                    <td rowspan="5" valign="top"><img src="/images/bigHourGlass.gif" border="0" align="absmiddle" /></td>
                                                    <td class="header" valign="bottom">Pending Approval</td>
                                                </tr>
                                                <tr>
                                                    <td valign="top">Your exception is currently under review.  Please check back later...</td>
                                                </tr>
                                            </table>
                                            <br />
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </td>
    </tr>
    <tr height="1">
        <td colspan="2">
            <table width="100%" cellpadding="4" cellspacing="2" border="0">
                <tr>
                    <td colspan="3"><hr size="1" noshade /></td>
                </tr>
                <tr>
                    <td width="250" class="required">* = Required Field</td>
                    <td align="center">
                        <asp:Button ID="btnUpdate" runat="server" OnClick="btnUpdate_Click" Text="Update" CssClass="default" Width="75" Visible="false" /> 
                        <asp:Button ID="btnBack" runat="server" OnClick="btnBack_Click" Text="Back" CssClass="default" Width="75" /> 
                        <asp:Button ID="btnNext" runat="server" OnClick="btnNext_Click" Text="Next" CssClass="default" Width="75" />
                    </td>
                    <td width="250" align="right"><a href="http://eawiki/ArchitectureDocs/CtaWikiPortal"><img src="/images/file.gif" border="0" align="absmiddle" /> Reference Architecture</a>&nbsp;</td>
                </tr>
            </table>
        </td>
    </tr>
</table>
<asp:HiddenField ID="hdnUser" runat="server" />
<asp:HiddenField ID="hdnMnemonic" runat="server" />
<asp:HiddenField ID="hdnCostCenter" runat="server" />
<asp:HiddenField ID="hdnSI" runat="server" />
<input type="hidden" id="hdnBackup" name="hdnBackup" value='<%=strGridBackup %>' />
<input type="hidden" id="hdnMaintenance" name="hdnMaintenance" value='<%=strGridMaintenance %>' />
</asp:Content>
