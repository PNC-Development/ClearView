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
using System.Text;

namespace NCC.ClearView.Presentation.Web
{
    public partial class config_storage : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected bool boolUseCSM = (ConfigurationManager.AppSettings["USE_CSM_EXECUTION"] == "1");
        protected int intUnder48A = Int32.Parse(ConfigurationManager.AppSettings["DR_HOUR_RESPONSE"]);
        protected int intUnder48Q = Int32.Parse(ConfigurationManager.AppSettings["DR_HOUR_QUESTION"]);
        protected double dblCompressionPercentage = double.Parse(ConfigurationManager.AppSettings["SQL2008_COMPRESSION_PERCENTAGE"]);
        protected double dblTempDBOverhead = double.Parse(ConfigurationManager.AppSettings["SQL2008_TEMPDB_OVERHEAD"]);
        protected int intProfile;
        protected OnDemand oOnDemand;
        protected Forecast oForecast;
        protected Classes oClass;
        protected CSMConfig oCSMConfig;
        protected Cluster oCluster;
        protected Requests oRequest;
        protected Servers oServer;
        protected ServerName oServerName;
        protected Storage oStorage;
        protected Models oModel;
        protected ModelsProperties oModelsProperties;
        protected OperatingSystems oOperatingSystem;
        protected int intAnswer = 0;
        protected int intCluster = 0;
        protected int intConfig = 0;
        protected int intNumber = 0;
        protected int intRequest = 0;
        protected string strSQL = "";
        protected string strHidden = "";
        protected bool boolProd = false;
        protected bool boolQA = false;
        protected bool boolTest = false;
        protected bool boolMidrange = false;
        protected string strPaths = "";
        protected bool boolUsePNC = true;
        protected string strSaveStorageAlert = "";
        protected string strSaveStorageValidate = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "ClearView Storage Configuration";
            AuthenticateUser();
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oOnDemand = new OnDemand(intProfile, dsn);
            oForecast = new Forecast(intProfile, dsn);
            oClass = new Classes(intProfile, dsn);
            oCSMConfig = new CSMConfig(intProfile, dsn);
            oCluster = new Cluster(intProfile, dsn);
            oRequest = new Requests(intProfile, dsn);
            oServer = new Servers(intProfile, dsn);
            oServerName = new ServerName(intProfile, dsn);
            oStorage = new Storage(intProfile, dsn);
            oModel = new Models(intProfile, dsn);
            oModelsProperties = new ModelsProperties(intProfile, dsn);
            oOperatingSystem = new OperatingSystems(intProfile, dsn);
            if (Request.QueryString["aid"] != null && Request.QueryString["aid"] != "")
                intAnswer = Int32.Parse(Request.QueryString["aid"]);
            if (Request.QueryString["clusterid"] != null && Request.QueryString["clusterid"] != "")
                intCluster = Int32.Parse(Request.QueryString["clusterid"]);
            if (Request.QueryString["csmid"] != null && Request.QueryString["csmid"] != "")
                intConfig = Int32.Parse(Request.QueryString["csmid"]);
            if (Request.QueryString["num"] != null && Request.QueryString["num"] != "")
                intNumber = Int32.Parse(Request.QueryString["num"]);
            if (Request.QueryString["save"] != null)
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "save", "<script type=\"text/javascript\">RefreshOpeningWindow();alert('Storage Saved Successfully');<" + "/" + "script>");
            if (Request.QueryString["saveClose"] != null)
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "save", "<script type=\"text/javascript\">RefreshOpeningWindow();window.close();<" + "/" + "script>");
            if (Request.QueryString["generate"] != null)
                panGenerate.Visible = true;
            int intServer = 0;

            btnStorageOverride.Attributes.Add("onclick", "return OpenWindow('STORAGE_OVERRIDE','" + intAnswer.ToString() + "')");

            int intDR = 0;
            string strNotes = "";
            if (intAnswer > 0)
            {
                Page.Title = "ClearView Storage Configuration | Design # " + intAnswer.ToString();
                StringBuilder sbSQL = new StringBuilder(strSQL);
                StringBuilder sbHidden = new StringBuilder(strHidden);
                DataSet ds = oForecast.GetAnswer(intAnswer);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    int intModel = oForecast.GetModel(intAnswer);
                    int intType = oModelsProperties.GetType(intModel);
                    int intDRForecast = Int32.Parse(oForecast.TotalDRCount(intAnswer, boolUseCSM).ToString());
                    int intDRCurrent = Int32.Parse(ds.Tables[0].Rows[0]["recovery_number"].ToString());
                    int intClass = Int32.Parse(ds.Tables[0].Rows[0]["classid"].ToString());
                    bool boolPNC = (oClass.Get(intClass, "pnc") == "1");
                    bool boolOverride = (ds.Tables[0].Rows[0]["storage_override"].ToString() == "1");
                    lblPNC.Text = (boolPNC ? "1" : "0");
                    bool boolUnder = false;
                    if (oClass.IsProd(intClass))
                    {
                        boolProd = true;
                        if (oForecast.GetAnswer(intAnswer, "test") == "1")
                            boolTest = true;
                        if (oForecast.GetAnswerPlatform(intAnswer, intUnder48Q, intUnder48A) == true)
                            boolUnder = true;
                    }
                    else if (oClass.IsQA(intClass))
                        boolQA = true;
                    else
                        boolTest = true;


                    bool boolHADisabled = (oModelsProperties.IsHighAvailability(intModel) == false);
                    intServer = Int32.Parse(ds.Tables[0].Rows[0]["quantity"].ToString()) - oForecast.TotalServerCount(intAnswer, boolUseCSM);
                    intDR = Int32.Parse(ds.Tables[0].Rows[0]["recovery_number"].ToString()) - oForecast.TotalDRCount(intAnswer, boolUseCSM);
                    intRequest = oForecast.GetRequestID(intAnswer, true);
                    bool boolDatabase = false;
                    bool boolSQL = false;
                    bool boolSQL2008 = false;
                    bool boolDisabled = false;
                    int intFDrive = -1;
                    int intDriveId = -1;

                    DataSet dsServer = oServer.Get(intAnswer, intConfig, intCluster, intNumber);
                    if (dsServer.Tables[0].Rows.Count > 0)
                    {
                        intServer = Int32.Parse(dsServer.Tables[0].Rows[0]["id"].ToString());
                        Page.Title = "ClearView Storage Configuration | Server # " + intServer.ToString();
                        int intOS = Int32.Parse(dsServer.Tables[0].Rows[0]["osid"].ToString());
                        boolMidrange = (oOperatingSystem.IsMidrange(intOS) == true);
                    }

                    if (!IsPostBack)
                    {

                        ////Delete the luns (if specified )
                        if (boolOverride == true)
                        {
                            if (Request.QueryString["dellun"] != null && Request.QueryString["dellun"] != "")
                            {
                                oStorage.DeleteLunByLunID(Int32.Parse(Request.QueryString["dellun"]));
                                Response.Redirect(Request.Path + "?aid=" + intAnswer + "&clusterid=" + intCluster + "&csmid=" + intConfig + "&num=" + intNumber + "&refresh=true");
                            }
                        }


                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            ds = oServer.Get(intAnswer, intConfig, intCluster, intNumber);
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                if (ds.Tables[0].Rows[0]["step"].ToString() != "0")
                                {
                                    btnSaveStorage.Visible = false;
                                    btnSaveCloseStorage.Visible = false;
                                    btnMount.Visible = false;
                                    btnDrive.Visible = false;
                                }
                                intFDrive = Int32.Parse(ds.Tables[0].Rows[0]["fdrive"].ToString());
                                lblId.Text = intServer.ToString();

                                //Incase of override set the drive =0
                                if (boolOverride == true)

                                    intDriveId = 0;
                                else
                                    if (boolPNC == true && boolUsePNC == true)
                                        intDriveId = -1000;
                                    else
                                        intDriveId = -100;

                                if (ds.Tables[0].Rows[0]["local_storage"].ToString() == "1")
                                {
                                    radYes.Checked = true;
                                    radNo.Text = "No (selecting this will RESET all your SAN configuration)";
                                    if (boolMidrange == false && oModelsProperties.IsStorageDE_FDriveMustBeOnSAN(intModel) == false && oModelsProperties.IsStorageDE_FDriveOnly(intModel) == false)
                                    {
                                        if (boolProd == false)
                                        {
                                            panFDrive.Visible = true;
                                            if (intFDrive == 1)
                                            {
                                                radFYes.Checked = true;
                                                panStorageYes.Visible = true;
                                            }
                                            else if (intFDrive == -1)
                                            {
                                                radFNo.Checked = true;
                                                panStorageYes.Visible = true;
                                            }
                                            else
                                            {
                                                btnSaveStorage.Enabled = false;
                                                btnSaveCloseStorage.Enabled = false;
                                            }
                                        }
                                        else
                                        {
                                            if (boolProd == true && boolUnder == true)
                                            {
                                                strNotes += " - Production builds with under 48 hours recovery require SAN storage" + "<br/>";
                                                radYes.Checked = true;
                                                radYes.ToolTip = "If production and under 48 hours recovery, the build type must be set to SANConnectedF";
                                                radYes.Enabled = false;
                                                radNo.Enabled = false;
                                                oServer.UpdateLocalStorage(intServer, 1);
                                                boolDisabled = true;
                                                if (intFDrive == 0)
                                                {
                                                    oStorage.AddLun(intAnswer, 0, intCluster, intConfig, intNumber, intDriveId, (boolProd ? 10.00 : 0.00), (boolQA ? 10.00 : 0.00), (boolTest ? 10.00 : 0.00));
                                                    oServer.UpdateFDrive(intServer, 1);
                                                }
                                                panFDrive.Visible = true;
                                                radFYes.Checked = true;
                                                radFYes.ToolTip = "If production and under 48 hours recovery, the build type must be set to SANConnectedF";
                                                radFYes.Enabled = false;
                                                radFNo.Enabled = false;
                                            }
                                            panStorageYes.Visible = true;
                                        }
                                    }
                                    else if (oModelsProperties.IsStorageDE_FDriveMustBeOnSAN(intModel) == true || oModelsProperties.IsStorageDE_FDriveOnly(intModel) == true)
                                    {
                                        strNotes += " - Blades / VMWare require SAN storage" + "<br/>";
                                        radYes.Checked = true;
                                        radYes.ToolTip = "Blades / VMWare require SAN storage";
                                        radYes.Enabled = false;
                                        radNo.Enabled = false;
                                        boolDisabled = true;
                                        panFDrive.Visible = true;
                                        radFYes.Checked = true;
                                        radFYes.ToolTip = "Blades / VMWare require SAN storage";
                                        radFYes.Enabled = false;
                                        radFNo.Enabled = false;
                                        DataSet dsF = oStorage.GetLun(intAnswer, 0, intCluster, intConfig, intNumber, intDriveId);

                                        if (dsF.Tables[0].Rows.Count == 0)
                                        {
                                            oStorage.AddLun(intAnswer, 0, intCluster, intConfig, intNumber, intDriveId, (boolProd ? 10.00 : 0.00), (boolQA ? 10.00 : 0.00), (boolTest ? 10.00 : 0.00));
                                            oServer.UpdateFDrive(intServer, 1);
                                            Response.Redirect(Request.Path + "?aid=" + intAnswer + "&clusterid=" + intCluster + "&csmid=" + intConfig + "&num=" + intNumber);
                                        }
                                        if (oModelsProperties.IsStorageDE_FDriveOnly(intModel) == true)
                                        {
                                            btnDrive.Enabled = false;
                                            btnMount.Enabled = false;
                                        }
                                        panStorageYes.Visible = true;
                                    }
                                    else
                                        panStorageYes.Visible = true;
                                }
                                else if ((boolProd == true && boolUnder == true && boolMidrange == false) || oModelsProperties.IsStorageDE_FDriveMustBeOnSAN(intModel) == true || oModelsProperties.IsStorageDE_FDriveOnly(intModel) == true)
                                {
                                    oServer.UpdateLocalStorage(intServer, 1);
                                    if ((oModelsProperties.IsStorageDE_FDriveMustBeOnSAN(intModel) == true || oModelsProperties.IsStorageDE_FDriveOnly(intModel) == true) && intFDrive == 0)
                                    {
                                        oStorage.AddLun(intAnswer, 0, intCluster, intConfig, intNumber, intDriveId, (boolProd ? 10.00 : 0.00), (boolQA ? 10.00 : 0.00), (boolTest ? 10.00 : 0.00));
                                        oServer.UpdateFDrive(intServer, 1);
                                    }
                                    Response.Redirect(Request.Path + "?aid=" + intAnswer + "&clusterid=" + intCluster + "&csmid=" + intConfig + "&num=" + intNumber);
                                }
                                if (ds.Tables[0].Rows[0]["local_storage"].ToString() == "-1")
                                {
                                    radNo.Checked = true;
                                    oServer.UpdateFDrive(intServer, 0);
                                }
                                ds = oServerName.GetComponentDetailSelected(intServer, 1);
                                foreach (DataRow dr in ds.Tables[0].Rows)
                                {
                                    if (dr["sql"].ToString() == "1" || dr["dbase"].ToString() == "1")
                                    {
                                        if (dr["sql"].ToString() == "1")
                                            boolSQL = true;
                                        boolDatabase = true;
                                        if (dr["name"].ToString().Contains("2008") == true)
                                            boolSQL2008 = true;
                                    }
                                }
                                lblDatabase.Text = (boolDatabase ? (boolSQL2008 ? "2008" : "1") : "0");
                                btnReset.Visible = (boolDatabase == true && boolOverride == false && boolMidrange == false && oModelsProperties.IsStorageDE_FDriveOnly(intModel) == false);
                                btnReset.Attributes.Add("onclick", "return confirm('WARNING: Resetting the database configuration will delete all existing storage configuration\\n\\nAre you sure you want to reset this configuration?');");

                                // if PNC and SQL, update the E drive to be at least 30 GB
                                if (boolPNC == true && boolDatabase == true && boolOverride == false && boolMidrange == false)
                                {
                                    // Make sure the E drive is 30 GB
                                    ds = oStorage.GetLun(intAnswer, 0, intCluster, intConfig, intNumber, intDriveId);
                                    bool boolCreate30 = false;
                                    if (ds.Tables[0].Rows.Count == 1)
                                    {
                                        double dblEProd = 0.00;
                                        double.TryParse(ds.Tables[0].Rows[0]["size"].ToString(), out dblEProd);
                                        double dblEQA = 0.00;
                                        double.TryParse(ds.Tables[0].Rows[0]["size_qa"].ToString(), out dblEQA);
                                        double dblETest = 0.00;
                                        double.TryParse(ds.Tables[0].Rows[0]["size_test"].ToString(), out dblETest);

                                        if (boolProd == true && dblEProd < 30.00)
                                            boolCreate30 = true;
                                        if (boolQA == true && dblEQA < 30.00)
                                            boolCreate30 = true;
                                        if (boolTest == true && dblETest < 30.00)
                                            boolCreate30 = true;
                                    }
                                    else
                                        boolCreate30 = true;

                                    if (boolCreate30 == true)
                                    {
                                        // Create the E drive as 30 GB (delete all prior E drive configuration)
                                        oStorage.DeleteLun(intAnswer, 0, intCluster, intConfig, intNumber, intDriveId);
                                        oStorage.AddLun(intAnswer, 0, intCluster, intConfig, intNumber, intDriveId, (boolProd ? 30.00 : 0.00), (boolQA ? 30.00 : 0.00), (boolTest ? 30.00 : 0.00));
                                        oServer.UpdateFDrive(intServer, 1);
                                        Response.Redirect(Request.Path + "?aid=" + intAnswer + "&clusterid=" + intCluster + "&csmid=" + intConfig + "&num=" + intNumber);
                                    }
                                }
                            }
                        }
                    }

                    bool boolShowLuns = false;
                    if (oModelsProperties.IsStorageDE_FDriveOnly(intModel) == true)
                    {
                        // For VMWare, get only the F drive.
                        ds = oStorage.GetLun(intAnswer, 0, intCluster, intConfig, intNumber, intDriveId);
                        boolShowLuns = true;
                    }
                    else
                    {

                        ds = oStorage.GetLuns(intAnswer, 0, intCluster, intConfig, intNumber);

                        if (boolDatabase == true && boolOverride == false && boolMidrange == false)
                        {
                            btnDrive.Enabled = false;
                            btnMount.Enabled = false;
                            if (intFDrive == 0 && panFDrive.Visible == true && boolDisabled == false)
                                strSaveStorageAlert = "Please select if you want your F Drive to be SAN Attached";
                            bool boolAlreadyAdded = false;
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                if (Int32.Parse(dr["driveid"].ToString()) > 0)
                                {
                                    boolAlreadyAdded = true;
                                    break;
                                }
                            }

                            if (boolAlreadyAdded == true)
                                boolShowLuns = true;
                            else
                            {
                                bool boolShowDB = false;
                                if (intCluster > 0)
                                {
                                    panSQL.Visible = true;
                                    if (oCluster.Get(intCluster, "sql") == "1")
                                    {
                                        radSQLYes.Checked = true;
                                        boolShowDB = true;
                                    }
                                    if (oCluster.Get(intCluster, "sql") == "-1")
                                    {
                                        radSQLNo.Checked = true;
                                        boolShowLuns = true;
                                        btnReset.Enabled = false;
                                    }
                                }
                                else
                                    boolShowDB = true;
                                if (boolShowDB == true)
                                {
                                    if (boolPNC == false)
                                    {
                                        panDatabase.Visible = true;
                                        radNonYes.Attributes.Add("onclick", "ShowHideDiv('" + divNon.ClientID + "','inline');");
                                        radNonNo.Attributes.Add("onclick", "ShowHideDiv('" + divNon.ClientID + "','none');");
                                        radTempDBYes.Attributes.Add("onclick", "ShowHideDiv('" + divTempDB.ClientID + "','inline');");
                                        radTempDBNo.Attributes.Add("onclick", "ShowHideDiv('" + divTempDB.ClientID + "','none');");
                                        if (boolProd == false)
                                        {
                                            txtSize.Text = "0";
                                            txtSize.Enabled = false;
                                            txtNon.Text = "0";
                                            txtNon.Enabled = false;
                                        }
                                        if (boolQA == false)
                                        {
                                            txtQA.Text = "0";
                                            txtQA.Enabled = false;
                                            txtNonQA.Text = "0";
                                            txtNonQA.Enabled = false;
                                        }
                                        if (boolTest == false)
                                        {
                                            txtTest.Text = "0";
                                            txtTest.Enabled = false;
                                            txtNonTest.Text = "0";
                                            txtNonTest.Enabled = false;
                                        }
                                        if (intFDrive != 0)
                                        {
                                            if (strSaveStorageValidate != "")
                                                strSaveStorageValidate += " && ";
                                            strSaveStorageValidate += "EnsureDatabase('" + radYes.ClientID + "','" + radNo.ClientID + "','" + txtSize.ClientID + "','" + txtQA.ClientID + "','" + txtTest.ClientID + "','" + radNonYes.ClientID + "','" + radNonNo.ClientID + "','" + txtNon.ClientID + "','" + txtNonQA.ClientID + "','" + txtNonTest.ClientID + "',null,null,null,null)";
                                        }
                                    }
                                    else
                                    {
                                        panDatabasePNC.Visible = true;
                                        radNonPNCYes.Attributes.Add("onclick", "ShowHideDiv('" + divNonPNC.ClientID + "','inline');");
                                        radNonPNCNo.Attributes.Add("onclick", "ShowHideDiv('" + divNonPNC.ClientID + "','none');");
                                        radTempDBYes.Attributes.Add("onclick", "ShowHideDiv('" + divTempDB.ClientID + "','inline');");
                                        radTempDBNo.Attributes.Add("onclick", "ShowHideDiv('" + divTempDB.ClientID + "','none');");
                                        if (intFDrive != 0)
                                        {
                                            if (strSaveStorageValidate != "")
                                                strSaveStorageValidate += " && ";
                                            strSaveStorageValidate += "EnsureDatabase('" + radYes.ClientID + "','" + radNo.ClientID + "','" + txtSizePNC.ClientID + "',null,null,'" + radNonPNCYes.ClientID + "','" + radNonPNCNo.ClientID + "','" + txtNonPNC.ClientID + "',null,null,'" + txtPercentPNC.ClientID + "','" + radTempDBYes.ClientID + "','" + radTempDBNo.ClientID + "','" + txtTempPNC.ClientID + "')";
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            boolShowLuns = true;
                        }
                    }

                    if (boolShowLuns == true)
                    {
                        if (intFDrive == 0 && panFDrive.Visible == true && boolDisabled == false)
                            strSaveStorageAlert = "Please select if you want your F Drive to be SAN Attached";
                        if (boolOverride == true)
                        {
                            btnDrive.Visible = false;
                            btnMount.Text = "Add a Lun";
                            btnMount.Enabled = true;
                            btnMount.CommandArgument = "ADDLUN";
                            btnStorageOverride.Visible = false;

                        }
                        else if (boolMidrange == true)
                        {
                            btnDrive.Visible = false;
                            btnMount.Text = "Add Filesystem";
                        }
                        panDatabaseNo.Visible = true;
                        string strDrive = "";
                        bool boolOther = false;
                        int intRow = 0;
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            // START CODING FOR OVERRIDE!!
                            int intDrive = Int32.Parse(dr["driveid"].ToString());
                            if (intDrive > 0 || intDrive == -100 || intDrive == -1000 || (boolOverride == true && intDrive == 0))
                            {
                                string strLetter = dr["letter"].ToString();
                                if (strLetter == "")
                                {
                                    if (dr["driveid"].ToString() == "-1000")
                                        strLetter = "E";
                                    else if (dr["driveid"].ToString() == "-100")
                                        strLetter = "F";
                                    else if (dr["driveid"].ToString() == "-10")
                                        strLetter = "P";
                                    else if (dr["driveid"].ToString() == "-1")
                                        strLetter = "Q";
                                }
                                if (intDrive > 0)
                                    strDrive = strLetter + ":";
                                lblLun.Text = dr["id"].ToString();
                                intRow++;
                                boolOther = !boolOther;
                                sbSQL.Append("<tr");
                                sbSQL.Append(boolOther ? " bgcolor=\"F6F6F6\"" : "");
                                sbSQL.Append(">");

                                if (boolOverride == true && boolMidrange == false)
                                {
                                    sbSQL.Append("<td valign=\"top\" align=\"right\">[<a href=\"");
                                    sbSQL.Append(Request.Path);
                                    sbSQL.Append("?aid=");
                                    sbSQL.Append(Request.QueryString["aid"]);
                                    sbSQL.Append("&clusterid=");
                                    sbSQL.Append(Request.QueryString["clusterid"]);
                                    sbSQL.Append("&csmid=");
                                    sbSQL.Append(Request.QueryString["csmid"]);
                                    sbSQL.Append("&num=");
                                    sbSQL.Append(Request.QueryString["num"]);
                                    sbSQL.Append("&storage=true&dellun=");
                                    sbSQL.Append(dr["id"].ToString());
                                    sbSQL.Append("\" onclick=\"return confirm('Are you sure you want to delete this lun ?');\" title=\"Delete\">Delete</a>]&nbsp;&nbsp;");
                                    sbSQL.Append(intRow);
                                    sbSQL.Append("&nbsp;&nbsp;&nbsp;</td>");
                                }
                                else
                                {
                                    sbSQL.Append("<td valign=\"top\" align=\"right\">");
                                    sbSQL.Append(intRow);
                                    sbSQL.Append("&nbsp;&nbsp;&nbsp;</td>");
                                }

                                string strPath = dr["path"].ToString();
                                if (strPath != "")
                                {
                                    if (strPaths != "")
                                        strPaths += ",";
                                    while (strPath != "" && strPath.EndsWith("\\") == true)
                                        strPath = strPath.Substring(0, strPath.Length - 1);
                                    strPaths += "\"" + strPath + "\"";
                                }
                                if (boolOverride == true)
                                {
                                    sbSQL.Append("<td valign=\"top\"><input type=\"text\" name=\"ValidTextbox\" class=\"default\" style=\"width:125px\" onblur=\"UpdateTextPath(this,'HDN_");
                                    sbSQL.Append(dr["id"].ToString());
                                    sbSQL.Append("_PATH');\" onblur=\"BlurPath(this,'HDN_");
                                    sbSQL.Append(dr["id"].ToString());
                                    sbSQL.Append("_PATH','");
                                    sbSQL.Append(strPath);
                                    sbSQL.Append("');\" value=\"");
                                    sbSQL.Append(strPath);
                                    sbSQL.Append("\" /></td>");
                                }
                                else if (boolMidrange == true)
                                {
                                    sbSQL.Append("<td valign=\"top\"><input type=\"text\" name=\"ValidTextbox\" class=\"default\" style=\"width:125px\" onblur=\"UpdateTextPath(this,'HDN_");
                                    sbSQL.Append(dr["id"].ToString());
                                    sbSQL.Append("_PATH');\" onblur=\"BlurPath(this,'HDN_");
                                    sbSQL.Append(dr["id"].ToString());
                                    sbSQL.Append("_PATH','");
                                    sbSQL.Append(strPath);
                                    sbSQL.Append("');\" value=\"");
                                    sbSQL.Append(strPath);
                                    sbSQL.Append("\" /></td>");
                                }
                                else
                                {
                                    sbSQL.Append("<td valign=\"top\">");
                                    sbSQL.Append(strLetter);
                                    sbSQL.Append(":");
                                    sbSQL.Append(strPath);
                                    sbSQL.Append("</td>");
                                }
                                sbSQL.Append("<td valign=\"top\">");
                                sbSQL.Append("<select class=\"default\" onchange=\"UpdateDDL(this,'HDN_");
                                sbSQL.Append(dr["id"].ToString());
                                sbSQL.Append("_PERFORMANCE');\" style=\"width:100px;\">");
                                sbSQL.Append("<option value=\"High\"");
                                sbSQL.Append(dr["performance"].ToString() == "High" ? " selected" : "");
                                sbSQL.Append(">High</option>");
                                sbSQL.Append("<option value=\"Standard\"");
                                sbSQL.Append(dr["performance"].ToString() == "Standard" ? " selected" : "");
                                sbSQL.Append(">Standard</option>");
                                sbSQL.Append("<option value=\"Low\"");
                                sbSQL.Append(dr["performance"].ToString() == "Low" ? " selected" : "");
                                sbSQL.Append(">Low</option>");
                                sbSQL.Append("</select>");
                                sbSQL.Append("</td>");
                                if (boolProd == true)
                                {
                                    sbSQL.Append("<td valign=\"top\"><input type=\"text\" name=\"ValidTextbox0\" class=\"default\" style=\"width:75px\" onblur=\"UpdateText(this,'HDN_");
                                    sbSQL.Append(dr["id"].ToString());
                                    sbSQL.Append("_SIZE');\" value=\"");
                                    sbSQL.Append(dr["size"].ToString());
                                    sbSQL.Append("\" />&nbsp;GB</td>");
                                }
                                else
                                {
                                    //sbSQL.Append("<td valign=\"top\"><input type=\"text\" name=\"ValidTextbox0\" class=\"default\" style=\"width:75px\" value=\"0\" disabled=\"disabled\" />&nbsp;GB</td>");
                                }
                                if (boolQA == true)
                                {
                                    sbSQL.Append("<td valign=\"top\"><input type=\"text\" name=\"ValidTextbox0\" class=\"default\" style=\"width:75px\" onblur=\"UpdateText(this,'HDN_");
                                    sbSQL.Append(dr["id"].ToString());
                                    sbSQL.Append("_SIZE_QA');\" value=\"");
                                    sbSQL.Append(dr["size_qa"].ToString());
                                    sbSQL.Append("\" />&nbsp;GB</td>");
                                }
                                else
                                {
                                    //sbSQL.Append("<td valign=\"top\"><input type=\"text\" name=\"ValidTextbox0\" class=\"default\" style=\"width:75px\" value=\"0\" disabled=\"disabled\" />&nbsp;GB</td>");
                                }
                                if (boolTest == true)
                                {
                                    sbSQL.Append("<td valign=\"top\"><input type=\"text\" name=\"ValidTextbox0\" class=\"default\" style=\"width:75px\" onblur=\"UpdateText(this,'HDN_");
                                    sbSQL.Append(dr["id"].ToString());
                                    sbSQL.Append("_SIZE_TEST');\" value=\"");
                                    sbSQL.Append(dr["size_test"].ToString());
                                    sbSQL.Append("\" />&nbsp;GB</td>");
                                }
                                else
                                {
                                    //sbSQL.Append("<td valign=\"top\"><input type=\"text\" name=\"ValidTextbox0\" class=\"default\" style=\"width:75px\" value=\"0\" disabled=\"disabled\" />&nbsp;GB</td>");
                                }
                                sbSQL.Append("<td valign=\"top\">");
                                if (boolProd == true && boolUnder == true)
                                {
                                    sbSQL.Append("<select class=\"default\" onchange=\"UpdateDDL(this,'HDN_");
                                    sbSQL.Append(dr["id"].ToString());
                                    sbSQL.Append("_REPLICATED');\" style=\"width:75px;\">");
                                    sbSQL.Append("<option value=\"Yes\"");
                                    sbSQL.Append(dr["replicated"].ToString() == "1" ? " selected" : "");
                                    sbSQL.Append(">Yes</option>");
                                    sbSQL.Append("<option value=\"No\"");
                                    sbSQL.Append(dr["replicated"].ToString() == "0" ? " selected" : "");
                                    sbSQL.Append(">No</option>");
                                    sbSQL.Append("</select>");
                                }
                                else
                                {
                                    sbSQL.Append("<select class=\"default\" disabled=\"disabled\" style=\"width:75px;\">");
                                    sbSQL.Append("<option value=\"No\">No</option>");
                                    sbSQL.Append("</select>");
                                }
                                sbSQL.Append("</td>");
                                sbSQL.Append("<td valign=\"top\">");
                                sbSQL.Append("<select class=\"default\" onchange=\"UpdateDDL(this,'HDN_");
                                sbSQL.Append(dr["id"].ToString());
                                sbSQL.Append("_HIGH_AVAILABILITY');\" style=\"width:75px;\"");
                                sbSQL.Append(boolHADisabled == true ? " disabled" : "");
                                sbSQL.Append(">");
                                sbSQL.Append("<option value=\"No\"");
                                sbSQL.Append(dr["high_availability"].ToString() == "0" ? " selected" : "");
                                sbSQL.Append(">No</option>");
                                sbSQL.Append("<option value=\"Yes\"");
                                sbSQL.Append(dr["high_availability"].ToString() == "1" ? " selected" : "");
                                sbSQL.Append(">Yes</option>");
                                sbSQL.Append("</select>");
                                sbSQL.Append("</td>");
                                sbSQL.Append("</tr>");

                                sbHidden.Append("<input type=\"hidden\" name=\"HDN__");
                                sbHidden.Append(dr["id"].ToString());
                                sbHidden.Append("\" id=\"HDN__");
                                sbHidden.Append(dr["id"].ToString());
                                sbHidden.Append("\" />");

                                sbHidden.Append("<input type=\"hidden\" name=\"HDN_");
                                sbHidden.Append(dr["id"].ToString());
                                sbHidden.Append("_PATH\" id=\"HDN_");
                                sbHidden.Append(dr["id"].ToString());
                                sbHidden.Append("_PATH\" value=\"");
                                sbHidden.Append(strPath);
                                sbHidden.Append("\" />");
                                sbHidden.Append("<input type=\"hidden\" name=\"HDN_");
                                sbHidden.Append(dr["id"].ToString());
                                sbHidden.Append("_PERFORMANCE\" id=\"HDN_");
                                sbHidden.Append(dr["id"].ToString());
                                sbHidden.Append("_PERFORMANCE\" value=\"");
                                sbHidden.Append(dr["performance"].ToString());
                                sbHidden.Append("\" />");
                                if (boolProd == true)
                                {
                                    sbHidden.Append("<input type=\"hidden\" name=\"HDN_");
                                    sbHidden.Append(dr["id"].ToString());
                                    sbHidden.Append("_SIZE\" id=\"HDN_");
                                    sbHidden.Append(dr["id"].ToString());
                                    sbHidden.Append("_SIZE\" value=\"");
                                    sbHidden.Append(dr["size"].ToString());
                                    sbHidden.Append("\" />");
                                }
                                else
                                {
                                    sbHidden.Append("<input type=\"hidden\" name=\"HDN_");
                                    sbHidden.Append(dr["id"].ToString());
                                    sbHidden.Append("_SIZE\" id=\"HDN_");
                                    sbHidden.Append(dr["id"].ToString());
                                    sbHidden.Append("_SIZE\" value=\"0\" />");
                                }
                                if (boolQA == true)
                                {
                                    sbHidden.Append("<input type=\"hidden\" name=\"HDN_");
                                    sbHidden.Append(dr["id"].ToString());
                                    sbHidden.Append("_SIZE_QA\" id=\"HDN_");
                                    sbHidden.Append(dr["id"].ToString());
                                    sbHidden.Append("_SIZE_QA\" value=\"");
                                    sbHidden.Append(dr["size_qa"].ToString());
                                    sbHidden.Append("\" />");
                                }
                                else
                                {
                                    sbHidden.Append("<input type=\"hidden\" name=\"HDN_");
                                    sbHidden.Append(dr["id"].ToString());
                                    sbHidden.Append("_SIZE_QA\" id=\"HDN_");
                                    sbHidden.Append(dr["id"].ToString());
                                    sbHidden.Append("_SIZE_QA\" value=\"0\" />");
                                }
                                if (boolTest == true)
                                {
                                    sbHidden.Append("<input type=\"hidden\" name=\"HDN_");
                                    sbHidden.Append(dr["id"].ToString());
                                    sbHidden.Append("_SIZE_TEST\" id=\"HDN_");
                                    sbHidden.Append(dr["id"].ToString());
                                    sbHidden.Append("_SIZE_TEST\" value=\"");
                                    sbHidden.Append(dr["size_test"].ToString());
                                    sbHidden.Append("\" />");
                                }
                                else
                                {
                                    sbHidden.Append("<input type=\"hidden\" name=\"HDN_");
                                    sbHidden.Append(dr["id"].ToString());
                                    sbHidden.Append("_SIZE_TEST\" id=\"HDN_");
                                    sbHidden.Append(dr["id"].ToString());
                                    sbHidden.Append("_SIZE_TEST\" value=\"0\" />");
                                }
                                if (boolProd == true && boolUnder == true)
                                {
                                    sbHidden.Append("<input type=\"hidden\" name=\"HDN_");
                                    sbHidden.Append(dr["id"].ToString());
                                    sbHidden.Append("_REPLICATED\" id=\"HDN_");
                                    sbHidden.Append(dr["id"].ToString());
                                    sbHidden.Append("_REPLICATED\" value=\"");
                                    sbHidden.Append(dr["replicated"].ToString() == "1" ? "Yes" : "No");
                                    sbHidden.Append("\" />");
                                }
                                else
                                {
                                    sbHidden.Append("<input type=\"hidden\" name=\"HDN_");
                                    sbHidden.Append(dr["id"].ToString());
                                    sbHidden.Append("_REPLICATED\" id=\"HDN_");
                                    sbHidden.Append(dr["id"].ToString());
                                    sbHidden.Append("_REPLICATED\" value=\"No\" />");
                                }
                                sbHidden.Append("<input type=\"hidden\" name=\"HDN_");
                                sbHidden.Append(dr["id"].ToString());
                                sbHidden.Append("_HIGH_AVAILABILITY\" id=\"HDN_");
                                sbHidden.Append(dr["id"].ToString());
                                sbHidden.Append("_HIGH_AVAILABILITY\" value=\"");
                                sbHidden.Append(dr["high_availability"].ToString() == "1" ? "Yes" : "No");
                                sbHidden.Append("\" />");
                                // Delete Mount Point (if there)
                                if (Request.QueryString["del"] != null && Request.QueryString["del"] != "")
                                    oStorage.DeleteMountPoint(Int32.Parse(Request.QueryString["del"]));
                                // Add Mount Points
                                DataSet dsPoints = oStorage.GetMountPoints(Int32.Parse(dr["id"].ToString()));
                                int intPoint = 0;
                                foreach (DataRow drPoint in dsPoints.Tables[0].Rows)
                                {
                                    intRow++;
                                    intPoint++;
                                    boolOther = !boolOther;
                                    sbSQL.Append("<tr");
                                    sbSQL.Append(boolOther ? " bgcolor=\"F6F6F6\"" : "");
                                    sbSQL.Append(">");
                                    sbSQL.Append("<td valign=\"top\" align=\"right\">[<a href=\"");
                                    sbSQL.Append(Request.Path);
                                    sbSQL.Append("?aid=");
                                    sbSQL.Append(Request.QueryString["aid"]);
                                    sbSQL.Append("&clusterid=");
                                    sbSQL.Append(Request.QueryString["clusterid"]);
                                    sbSQL.Append("&csmid=");
                                    sbSQL.Append(Request.QueryString["csmid"]);
                                    sbSQL.Append("&num=");
                                    sbSQL.Append(Request.QueryString["num"]);
                                    sbSQL.Append("&storage=true&del=");
                                    sbSQL.Append(drPoint["id"].ToString());
                                    sbSQL.Append("\" onclick=\"return confirm('Are you sure you want to delete this ");
                                    sbSQL.Append(boolMidrange ? "filesystem" : "mount point");
                                    sbSQL.Append("?');\" title=\"Delete\">Delete</a>]&nbsp;&nbsp;");
                                    sbSQL.Append(intRow);
                                    sbSQL.Append("&nbsp;&nbsp;&nbsp;</td>");
                                    strPath = drPoint["path"].ToString();
                                    if (strPath != "")
                                    {
                                        if (strPaths != "")
                                            strPaths += ",";
                                        while (strPath != "" && strPath.EndsWith("\\") == true)
                                            strPath = strPath.Substring(0, strPath.Length - 1);
                                        strPaths += "\"" + strPath + "\"";
                                    }
                                    if (boolMidrange == true)
                                    {
                                        sbSQL.Append("<td valign=\"top\"><input type=\"text\" name=\"ValidTextbox\" class=\"default\" style=\"width:125px\" onblur=\"UpdateTextPath(this,'MNT_");
                                        sbSQL.Append(drPoint["id"].ToString());
                                        sbSQL.Append("_PATH');\" onblur=\"BlurPath(this,'MNT_");
                                        sbSQL.Append(drPoint["id"].ToString());
                                        sbSQL.Append("_PATH','");
                                        sbSQL.Append(strPath);
                                        sbSQL.Append("');\" value=\"");
                                        sbSQL.Append(strPath);
                                        sbSQL.Append("\" /></td>");
                                    }
                                    else
                                    {
                                        sbSQL.Append("<td valign=\"top\">");
                                        sbSQL.Append(strLetter);
                                        sbSQL.Append(":\\SH");
                                        sbSQL.Append(dr["driveid"].ToString());
                                        sbSQL.Append("VOL");
                                        sbSQL.Append(intPoint < 10 ? "0" : "");
                                        sbSQL.Append(intPoint.ToString());
                                        sbSQL.Append("</td>");
                                    }
                                    sbSQL.Append("<td valign=\"top\">");
                                    sbSQL.Append("<select class=\"default\" onchange=\"UpdateDDL(this,'MNT_");
                                    sbSQL.Append(drPoint["id"].ToString());
                                    sbSQL.Append("_PERFORMANCE');\" style=\"width:100px;\">");
                                    sbSQL.Append("<option value=\"High\"");
                                    sbSQL.Append(drPoint["performance"].ToString() == "High" ? " selected" : "");
                                    sbSQL.Append(">High</option>");
                                    sbSQL.Append("<option value=\"Standard\"");
                                    sbSQL.Append(drPoint["performance"].ToString() == "Standard" ? " selected" : "");
                                    sbSQL.Append(">Standard</option>");
                                    sbSQL.Append("<option value=\"Low\"");
                                    sbSQL.Append(drPoint["performance"].ToString() == "Low" ? " selected" : "");
                                    sbSQL.Append(">Low</option>");
                                    sbSQL.Append("</select>");
                                    sbSQL.Append("</td>");
                                    if (boolProd == true)
                                    {
                                        sbSQL.Append("<td valign=\"top\"><input type=\"text\" name=\"ValidTextbox0\" class=\"default\" style=\"width:75px\" onblur=\"UpdateText(this,'MNT_");
                                        sbSQL.Append(drPoint["id"].ToString());
                                        sbSQL.Append("_SIZE');\" value=\"");
                                        sbSQL.Append(drPoint["size"].ToString());
                                        sbSQL.Append("\" />&nbsp;GB</td>");
                                    }
                                    else
                                    {
                                        //sbSQL.Append("<td valign=\"top\"><input type=\"text\" name=\"ValidTextbox0\" class=\"default\" style=\"width:75px\" value=\"0\" disabled=\"disabled\" />&nbsp;GB</td>");
                                    }
                                    if (boolQA == true)
                                    {
                                        sbSQL.Append("<td valign=\"top\"><input type=\"text\" name=\"ValidTextbox0\" class=\"default\" style=\"width:75px\" onblur=\"UpdateText(this,'MNT_");
                                        sbSQL.Append(drPoint["id"].ToString());
                                        sbSQL.Append("_SIZE_QA');\" value=\"");
                                        sbSQL.Append(drPoint["size_qa"].ToString());
                                        sbSQL.Append("\" />&nbsp;GB</td>");
                                    }
                                    else
                                    {
                                        //sbSQL.Append("<td valign=\"top\"><input type=\"text\" name=\"ValidTextbox0\" class=\"default\" style=\"width:75px\" value=\"0\" disabled=\"disabled\" />&nbsp;GB</td>");
                                    }
                                    if (boolTest == true)
                                    {
                                        sbSQL.Append("<td valign=\"top\"><input type=\"text\" name=\"ValidTextbox0\" class=\"default\" style=\"width:75px\" onblur=\"UpdateText(this,'MNT_");
                                        sbSQL.Append(drPoint["id"].ToString());
                                        sbSQL.Append("_SIZE_TEST');\" value=\"");
                                        sbSQL.Append(drPoint["size_test"].ToString());
                                        sbSQL.Append("\" />&nbsp;GB</td>");
                                    }
                                    else
                                    {
                                        //sbSQL.Append("<td valign=\"top\"><input type=\"text\" name=\"ValidTextbox0\" class=\"default\" style=\"width:75px\" value=\"0\" disabled=\"disabled\" />&nbsp;GB</td>");
                                    }
                                    sbSQL.Append("<td valign=\"top\">");
                                    if (boolProd == true && boolUnder == true)
                                    {
                                        sbSQL.Append("<select class=\"default\" onchange=\"UpdateDDL(this,'MNT_");
                                        sbSQL.Append(drPoint["id"].ToString());
                                        sbSQL.Append("_REPLICATED');\" style=\"width:75px;\">");
                                        sbSQL.Append("<option value=\"Yes\"");
                                        sbSQL.Append(drPoint["replicated"].ToString() == "1" ? " selected" : "");
                                        sbSQL.Append(">Yes</option>");
                                        sbSQL.Append("<option value=\"No\"");
                                        sbSQL.Append(drPoint["replicated"].ToString() == "0" ? " selected" : "");
                                        sbSQL.Append(">No</option>");
                                        sbSQL.Append("</select>");
                                    }
                                    else
                                    {
                                        sbSQL.Append("<select class=\"default\" disabled=\"disabled\" style=\"width:75px;\">");
                                        sbSQL.Append("<option value=\"No\">No</option>");
                                        sbSQL.Append("</select>");
                                    }
                                    sbSQL.Append("</td>");
                                    sbSQL.Append("<td valign=\"top\">");
                                    sbSQL.Append("<select class=\"default\" onchange=\"UpdateDDL(this,'MNT_");
                                    sbSQL.Append(drPoint["id"].ToString());
                                    sbSQL.Append("_HIGH_AVAILABILITY');\" style=\"width:75px;\"");
                                    sbSQL.Append(boolHADisabled == true ? " disabled" : "");
                                    sbSQL.Append(">");
                                    sbSQL.Append("<option value=\"No\"");
                                    sbSQL.Append(drPoint["high_availability"].ToString() == "0" ? " selected" : "");
                                    sbSQL.Append(">No</option>");
                                    sbSQL.Append("<option value=\"Yes\"");
                                    sbSQL.Append(drPoint["high_availability"].ToString() == "1" ? " selected" : "");
                                    sbSQL.Append(">Yes</option>");
                                    sbSQL.Append("</select>");
                                    sbSQL.Append("</td>");
                                    sbSQL.Append("</tr>");

                                    sbHidden.Append("<input type=\"hidden\" name=\"MNT__");
                                    sbHidden.Append(drPoint["id"].ToString());
                                    sbHidden.Append("\" id=\"MNT__");
                                    sbHidden.Append(drPoint["id"].ToString());
                                    sbHidden.Append("\" />");

                                    sbHidden.Append("<input type=\"hidden\" name=\"MNT_");
                                    sbHidden.Append(drPoint["id"].ToString());
                                    sbHidden.Append("_PATH\" id=\"MNT_");
                                    sbHidden.Append(drPoint["id"].ToString());
                                    sbHidden.Append("_PATH\" value=\"");
                                    sbHidden.Append(strPath);
                                    sbHidden.Append("\" />");
                                    sbHidden.Append("<input type=\"hidden\" name=\"MNT_");
                                    sbHidden.Append(drPoint["id"].ToString());
                                    sbHidden.Append("_PERFORMANCE\" id=\"MNT_");
                                    sbHidden.Append(drPoint["id"].ToString());
                                    sbHidden.Append("_PERFORMANCE\" value=\"");
                                    sbHidden.Append(drPoint["performance"].ToString());
                                    sbHidden.Append("\" />");
                                    if (boolProd == true)
                                    {
                                        sbHidden.Append("<input type=\"hidden\" name=\"MNT_");
                                        sbHidden.Append(drPoint["id"].ToString());
                                        sbHidden.Append("_SIZE\" id=\"MNT_");
                                        sbHidden.Append(drPoint["id"].ToString());
                                        sbHidden.Append("_SIZE\" value=\"");
                                        sbHidden.Append(drPoint["size"].ToString());
                                        sbHidden.Append("\" />");
                                    }
                                    else
                                    {
                                        sbHidden.Append("<input type=\"hidden\" name=\"MNT_");
                                        sbHidden.Append(drPoint["id"].ToString());
                                        sbHidden.Append("_SIZE\" id=\"MNT_");
                                        sbHidden.Append(drPoint["id"].ToString());
                                        sbHidden.Append("_SIZE\" value=\"0\" />");
                                    }
                                    if (boolQA == true)
                                    {
                                        sbHidden.Append("<input type=\"hidden\" name=\"MNT_");
                                        sbHidden.Append(drPoint["id"].ToString());
                                        sbHidden.Append("_SIZE_QA\" id=\"MNT_");
                                        sbHidden.Append(drPoint["id"].ToString());
                                        sbHidden.Append("_SIZE_QA\" value=\"");
                                        sbHidden.Append(drPoint["size_qa"].ToString());
                                        sbHidden.Append("\" />");
                                    }
                                    else
                                    {
                                        sbHidden.Append("<input type=\"hidden\" name=\"MNT_");
                                        sbHidden.Append(drPoint["id"].ToString());
                                        sbHidden.Append("_SIZE_QA\" id=\"MNT_");
                                        sbHidden.Append(drPoint["id"].ToString());
                                        sbHidden.Append("_SIZE_QA\" value=\"0\" />");
                                    }
                                    if (boolTest == true)
                                    {
                                        sbHidden.Append("<input type=\"hidden\" name=\"MNT_");
                                        sbHidden.Append(drPoint["id"].ToString());
                                        sbHidden.Append("_SIZE_TEST\" id=\"MNT_");
                                        sbHidden.Append(drPoint["id"].ToString());
                                        sbHidden.Append("_SIZE_TEST\" value=\"");
                                        sbHidden.Append(drPoint["size_test"].ToString());
                                        sbHidden.Append("\" />");
                                    }
                                    else
                                    {
                                        sbHidden.Append("<input type=\"hidden\" name=\"MNT_");
                                        sbHidden.Append(drPoint["id"].ToString());
                                        sbHidden.Append("_SIZE_TEST\" id=\"MNT_");
                                        sbHidden.Append(drPoint["id"].ToString());
                                        sbHidden.Append("_SIZE_TEST\" value=\"0\" />");
                                    }
                                    if (boolProd == true && boolUnder == true)
                                    {
                                        sbHidden.Append("<input type=\"hidden\" name=\"MNT_");
                                        sbHidden.Append(drPoint["id"].ToString());
                                        sbHidden.Append("_REPLICATED\" id=\"MNT_");
                                        sbHidden.Append(drPoint["id"].ToString());
                                        sbHidden.Append("_REPLICATED\" value=\"");
                                        sbHidden.Append(drPoint["replicated"].ToString() == "1" ? "Yes" : "No");
                                        sbHidden.Append("\" />");
                                    }
                                    else
                                    {
                                        sbHidden.Append("<input type=\"hidden\" name=\"MNT_");
                                        sbHidden.Append(drPoint["id"].ToString());
                                        sbHidden.Append("_REPLICATED\" id=\"MNT_");
                                        sbHidden.Append(drPoint["id"].ToString());
                                        sbHidden.Append("_REPLICATED\" value=\"No\" />");
                                    }
                                    sbHidden.Append("<input type=\"hidden\" name=\"MNT_");
                                    sbHidden.Append(drPoint["id"].ToString());
                                    sbHidden.Append("_HIGH_AVAILABILITY\" id=\"MNT_");
                                    sbHidden.Append(drPoint["id"].ToString());
                                    sbHidden.Append("_HIGH_AVAILABILITY\" value=\"");
                                    sbHidden.Append(drPoint["high_availability"].ToString() == "1" ? "Yes" : "No");
                                    sbHidden.Append("\" />");
                                }
                            }
                        }
                        if (strDrive == "" && boolOverride == false && boolMidrange == false)
                        {
                            btnDrive.CommandArgument = "ADD";
                            btnMount.Enabled = false;
                        }
                        else
                        {
                            if (boolOverride == true)
                                btnMount.Attributes.Add("onclick", "return EnsureValidText() && EnsureTextbox0();");
                            else
                                btnMount.Attributes.Add("onclick", "return EnsureTextbox() && EnsureTextbox0();");
                            btnDrive.CommandArgument = "REMOVE";
                            btnDrive.Text = "Remove the " + strDrive + " Drive";
                            btnDrive.Attributes.Add("onclick", "return confirm('WARNING: Removing the " + strDrive + " Drive will remove all mount point configurations associated with this drive.\\n\\nAre you sure you want to continue?');");
                        }
                    }
                    btnClose.Attributes.Add("onclick", "return window.close();");

                    if (boolOverride == true)
                    {
                        if (strSaveStorageValidate != "")
                            strSaveStorageValidate += " && ";
                        strSaveStorageValidate += "EnsureValidText() && EnsureTextbox0()";
                    }
                    else
                    {
                        if (strSaveStorageValidate != "")
                            strSaveStorageValidate += " && ";
                        strSaveStorageValidate += "EnsureTextbox() && EnsureTextbox0()";
                    }

                    if (boolProd == false || boolUnder == false)
                        strNotes += " - Replication only available for Production builds with under 48 hours recovery" + "<br/>";

                    if (boolSQL == true && boolSQL2008 == false)
                        strNotes += " - SQL 2005 has no compression available for the BACKUP LUN(s)" + "<br/>";

                }

                strSQL = sbSQL.ToString();
                strHidden = sbHidden.ToString();
            }
            else
            {
                btnSaveStorage.Enabled = false;
                btnSaveCloseStorage.Enabled = false;
            }

            if (strNotes != "")
            {
                lblNotes.Text = strNotes;
                panNotes.Visible = true;
            }

            if (strSaveStorageAlert != "")
            {
                btnSaveStorage.Attributes.Add("onclick", "alert('" + strSaveStorageAlert + "');return false;");
                btnSaveCloseStorage.Attributes.Add("onclick", "alert('" + strSaveStorageAlert + "');return false;");
            }
            else if (strSaveStorageValidate != "")
            {
                btnSaveStorage.Attributes.Add("onclick", "return " + strSaveStorageValidate + ";");
                btnSaveCloseStorage.Attributes.Add("onclick", "return " + strSaveStorageValidate + ";");
            }

            Page.ClientScript.RegisterStartupScript(typeof(Page), "loader", "<script type=\"text/javascript\">CatchClose();<" + "/" + "script>");
        }
        protected void radYes_Check(Object Sender, EventArgs e)
        {
            int intModel = oForecast.GetModel(intAnswer);
            DataSet ds = oStorage.GetLuns(intAnswer, 0, intCluster, intConfig, intNumber);
            bool boolAlreadyAdded = false;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (Int32.Parse(dr["driveid"].ToString()) > 0)
                {
                    boolAlreadyAdded = true;
                    break;
                }
            }
            if (boolAlreadyAdded == false)
            {
                if (lblDatabase.Text == "0" || boolMidrange == true)
                {
                    int intDrive = oStorage.GetNextLun(intAnswer, intCluster, intConfig, intNumber);
                    oStorage.AddLun(intAnswer, 0, intCluster, intConfig, intNumber, intDrive, (boolProd ? 1.00 : 0.00), (boolQA ? 1.00 : 0.00), (boolTest ? 1.00 : 0.00));
                }
            }
            ds = oServer.Get(intAnswer, intConfig, intCluster, intNumber);
            if (ds.Tables[0].Rows.Count > 0)
            {
                int intServer = Int32.Parse(ds.Tables[0].Rows[0]["id"].ToString());
                oServer.UpdateLocalStorage(intServer, 1);
            }
            Response.Redirect(Request.Path + "?aid=" + intAnswer + "&clusterid=" + intCluster + "&csmid=" + intConfig + "&num=" + intNumber + "&refresh=true");
        }
        protected void radNo_Check(Object Sender, EventArgs e)
        {
            oStorage.DeleteLuns(intAnswer, 0, intCluster, intConfig, intNumber);
            oServer.UpdateLocalStorage(Int32.Parse(lblId.Text), -1);
            DataSet ds = oServer.Get(intAnswer, intConfig, intCluster, intNumber);
            if (ds.Tables[0].Rows.Count > 0)
            {
                int intServer = Int32.Parse(ds.Tables[0].Rows[0]["id"].ToString());
                oServer.UpdateFDrive(intServer, -1);
            }
            Response.Redirect(Request.Path + "?aid=" + intAnswer + "&clusterid=" + intCluster + "&csmid=" + intConfig + "&num=" + intNumber + "&refresh=true");
        }
        protected void radFYes_Check(Object Sender, EventArgs e)
        {
            bool boolPNC = (lblPNC.Text == "1");
            if (boolPNC == true && boolUsePNC == true)
                oStorage.AddLun(intAnswer, 0, intCluster, intConfig, intNumber, -1000, (boolProd ? 10.00 : 0.00), (boolQA ? 10.00 : 0.00), (boolTest ? 10.00 : 0.00));
            else
                oStorage.AddLun(intAnswer, 0, intCluster, intConfig, intNumber, -100, (boolProd ? 10.00 : 0.00), (boolQA ? 10.00 : 0.00), (boolTest ? 10.00 : 0.00));
            DataSet ds = oServer.Get(intAnswer, intConfig, intCluster, intNumber);
            if (ds.Tables[0].Rows.Count > 0)
            {
                int intServer = Int32.Parse(ds.Tables[0].Rows[0]["id"].ToString());
                oServer.UpdateFDrive(intServer, 1);
            }
            Response.Redirect(Request.Path + "?aid=" + intAnswer + "&clusterid=" + intCluster + "&csmid=" + intConfig + "&num=" + intNumber);
        }
        protected void radFNo_Check(Object Sender, EventArgs e)
        {
            bool boolPNC = (lblPNC.Text == "1");
            if (boolPNC == true && boolUsePNC == true)
                oStorage.DeleteLun(intAnswer, 0, intCluster, intConfig, intNumber, -1000);
            else
                oStorage.DeleteLun(intAnswer, 0, intCluster, intConfig, intNumber, -100);
            DataSet ds = oServer.Get(intAnswer, intConfig, intCluster, intNumber);
            if (ds.Tables[0].Rows.Count > 0)
            {
                int intServer = Int32.Parse(ds.Tables[0].Rows[0]["id"].ToString());
                oServer.UpdateFDrive(intServer, -1);
            }
            Response.Redirect(Request.Path + "?aid=" + intAnswer + "&clusterid=" + intCluster + "&csmid=" + intConfig + "&num=" + intNumber);
        }
        protected void radSQLYes_Check(Object Sender, EventArgs e)
        {
            oCluster.UpdateSQL(intCluster, 1);
            Response.Redirect(Request.Path + "?aid=" + intAnswer + "&clusterid=" + intCluster + "&csmid=" + intConfig + "&num=" + intNumber);
        }
        protected void radSQLNo_Check(Object Sender, EventArgs e)
        {
            oCluster.UpdateSQL(intCluster, -1);
            Response.Redirect(Request.Path + "?aid=" + intAnswer + "&clusterid=" + intCluster + "&csmid=" + intConfig + "&num=" + intNumber);
        }
        protected void btnMount_Click(Object Sender, EventArgs e)
        {
            if (btnMount.CommandArgument != "ADDLUN") //In Case of Override
            {
                if (panDatabaseNo.Visible == true)
                {

                    foreach (string strForm in Request.Form)
                    {
                        if (strForm.StartsWith("HDN__") == true)
                        {
                            int intID = Int32.Parse(strForm.Substring(5));
                            oStorage.UpdateLun(intID, Request.Form["HDN_" + intID.ToString() + "_PATH"], Request.Form["HDN_" + intID.ToString() + "_PERFORMANCE"], double.Parse(Request.Form["HDN_" + intID.ToString() + "_SIZE"]), double.Parse(Request.Form["HDN_" + intID.ToString() + "_SIZE_QA"]), double.Parse(Request.Form["HDN_" + intID.ToString() + "_SIZE_TEST"]), (Request.Form["HDN_" + intID.ToString() + "_REPLICATED"] == "Yes" ? 1 : 0), (Request.Form["HDN_" + intID.ToString() + "_HIGH_AVAILABILITY"] == "Yes" ? 1 : 0));
                        }
                        if (strForm.StartsWith("MNT__") == true)
                        {
                            int intMountID = Int32.Parse(strForm.Substring(5));
                            oStorage.UpdateMountPoint(intMountID, Request.Form["MNT_" + intMountID.ToString() + "_PATH"], Request.Form["MNT_" + intMountID.ToString() + "_PERFORMANCE"], double.Parse(Request.Form["MNT_" + intMountID.ToString() + "_SIZE"]), double.Parse(Request.Form["MNT_" + intMountID.ToString() + "_SIZE_QA"]), double.Parse(Request.Form["MNT_" + intMountID.ToString() + "_SIZE_TEST"]), (Request.Form["MNT_" + intMountID.ToString() + "_REPLICATED"] == "Yes" ? 1 : 0), (Request.Form["MNT_" + intMountID.ToString() + "_HIGH_AVAILABILITY"] == "Yes" ? 1 : 0));
                        }
                    }
                }
                oStorage.AddMountPoint(Int32.Parse(lblLun.Text), "", "Standard", 0.00, 0.00, 0.00, 0, 0);
                Response.Redirect(Request.Path + "?aid=" + intAnswer + "&clusterid=" + intCluster + "&csmid=" + intConfig + "&num=" + intNumber + "&refresh=true");
            }
            else
            {
                //In Case of Override Add luns only
                foreach (string strForm in Request.Form)
                {
                    if (strForm.StartsWith("HDN__") == true)
                    {
                        int intID = Int32.Parse(strForm.Substring(5));
                        oStorage.UpdateLun(intID, Request.Form["HDN_" + intID.ToString() + "_PATH"], Request.Form["HDN_" + intID.ToString() + "_PERFORMANCE"], double.Parse(Request.Form["HDN_" + intID.ToString() + "_SIZE"]), double.Parse(Request.Form["HDN_" + intID.ToString() + "_SIZE_QA"]), double.Parse(Request.Form["HDN_" + intID.ToString() + "_SIZE_TEST"]), (Request.Form["HDN_" + intID.ToString() + "_REPLICATED"] == "Yes" ? 1 : 0), (Request.Form["HDN_" + intID.ToString() + "_HIGH_AVAILABILITY"] == "Yes" ? 1 : 0));
                    }
                }
                oStorage.AddLun(intAnswer, 0, intCluster, intConfig, intNumber, 0, (boolProd ? 1.00 : 0.00), (boolQA ? 1.00 : 0.00), (boolTest ? 1.00 : 0.00));
                Response.Redirect(Request.Path + "?aid=" + intAnswer + "&clusterid=" + intCluster + "&csmid=" + intConfig + "&num=" + intNumber + "&refresh=true");
            }
        }
        protected void btnDrive_Click(Object Sender, EventArgs e)
        {
            if (btnDrive.CommandArgument == "ADD")
            {
                int intDrive = oStorage.GetNextLun(intAnswer, intCluster, intConfig, intNumber);
                oStorage.AddLun(intAnswer, 0, intCluster, intConfig, intNumber, intDrive, (boolProd ? 1.00 : 0.00), (boolQA ? 1.00 : 0.00), (boolTest ? 1.00 : 0.00));
            }
            else
            {
                DataSet ds = oStorage.GetLuns(intAnswer, 0, intCluster, intConfig, intNumber);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    int intDrive = Int32.Parse(dr["driveid"].ToString());
                    if (intDrive > 0)
                    {
                        DataSet dsPoints = oStorage.GetMountPoints(Int32.Parse(dr["id"].ToString()));
                        foreach (DataRow drPoint in dsPoints.Tables[0].Rows)
                            oStorage.DeleteMountPoint(Int32.Parse(drPoint["id"].ToString()));
                        oStorage.DeleteLun(intAnswer, 0, intCluster, intConfig, intNumber, intDrive);
                    }
                }
            }
            Response.Redirect(Request.Path + "?aid=" + intAnswer + "&clusterid=" + intCluster + "&csmid=" + intConfig + "&num=" + intNumber + "&refresh=true");
        }



        protected void btnReset_Click(Object Sender, EventArgs e)
        {
            oStorage.DeleteLuns(intAnswer, 0, intCluster, intConfig, intNumber);
            Response.Redirect(Request.Path + "?aid=" + intAnswer + "&clusterid=" + intCluster + "&csmid=" + intConfig + "&num=" + intNumber + "&reset=true");
        }
        protected void btnSaveStorage_Click(Object Sender, EventArgs e)
        {
            Save(false);
        }
        protected void btnSaveCloseStorage_Click(Object Sender, EventArgs e)
        {
            Save(true);
        }
        protected void Save(bool _close)
        {
            if (radYes.Checked == true)
            {
                // PNC SQL Breakdown
                if (panDatabasePNC.Visible == true)
                {
                    DataSet ds = oStorage.GetLuns(intAnswer, 0, intCluster, intConfig, intNumber);
                    bool boolAlreadyAdded = false;
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (Int32.Parse(dr["driveid"].ToString()) > 0)
                        {
                            boolAlreadyAdded = true;
                            break;
                        }
                    }
                    if (boolAlreadyAdded == false)
                    {
                        double dblSize = double.Parse(txtSizePNC.Text);
                        int intNon = (radNonPNCYes.Checked ? 1 : 0);
                        double dblNon = 0.00;
                        if (intNon == 1)
                            dblNon = double.Parse(txtNonPNC.Text);
                        double dblPercent = double.Parse(txtPercentPNC.Text);
                        double dblTempDB = 0.00;
                        double.TryParse(txtTempPNC.Text, out dblTempDB);
                        oStorage.AddLunSQLPNC(intAnswer, 0, intCluster, intConfig, intNumber, dblSize, dblNon, dblPercent, dblTempDB, dblCompressionPercentage, dblTempDBOverhead, (lblDatabase.Text == "2008"));
                    }
                    Response.Redirect(Request.Path + "?aid=" + intAnswer + "&clusterid=" + intCluster + "&csmid=" + intConfig + "&num=" + intNumber + "&generate=true");
                }
                // NCB SQL Breakdown
                if (panDatabase.Visible == true)
                {
                    DataSet ds = oStorage.GetLuns(intAnswer, 0, intCluster, intConfig, intNumber);
                    bool boolAlreadyAdded = false;
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (Int32.Parse(dr["driveid"].ToString()) > 0)
                        {
                            boolAlreadyAdded = true;
                            break;
                        }
                    }
                    if (boolAlreadyAdded == false)
                    {
                        double dblSize = double.Parse(txtSize.Text);
                        double dblQA = double.Parse(txtQA.Text);
                        double dblTest = double.Parse(txtTest.Text);
                        int intNon = (radNonYes.Checked ? 1 : 0);
                        double dblNon = 0.00;
                        double dblNonQA = 0.00;
                        double dblNonTest = 0.00;
                        if (intNon == 1)
                        {
                            dblNon = double.Parse(txtNon.Text);
                            dblNonQA = double.Parse(txtNonQA.Text);
                            dblNonTest = double.Parse(txtNonTest.Text);
                        }
                        oStorage.AddLunSQL(intAnswer, 0, intCluster, intConfig, intNumber, dblSize, dblQA, dblTest, dblNon, dblNonQA, dblNonTest);
                    }
                    Response.Redirect(Request.Path + "?aid=" + intAnswer + "&clusterid=" + intCluster + "&csmid=" + intConfig + "&num=" + intNumber + "&generate=true");
                }
                if (panDatabaseNo.Visible == true)
                {
                    foreach (string strForm in Request.Form)
                    {
                        if (strForm.StartsWith("HDN__") == true)
                        {
                            int intID = Int32.Parse(strForm.Substring(5));
                            oStorage.UpdateLun(intID, Request.Form["HDN_" + intID.ToString() + "_PATH"], Request.Form["HDN_" + intID.ToString() + "_PERFORMANCE"], double.Parse(Request.Form["HDN_" + intID.ToString() + "_SIZE"]), double.Parse(Request.Form["HDN_" + intID.ToString() + "_SIZE_QA"]), double.Parse(Request.Form["HDN_" + intID.ToString() + "_SIZE_TEST"]), (Request.Form["HDN_" + intID.ToString() + "_REPLICATED"] == "Yes" ? 1 : 0), (Request.Form["HDN_" + intID.ToString() + "_HIGH_AVAILABILITY"] == "Yes" ? 1 : 0));
                        }
                        if (strForm.StartsWith("MNT__") == true)
                        {
                            int intMountID = Int32.Parse(strForm.Substring(5));
                            oStorage.UpdateMountPoint(intMountID, Request.Form["MNT_" + intMountID.ToString() + "_PATH"], Request.Form["MNT_" + intMountID.ToString() + "_PERFORMANCE"], double.Parse(Request.Form["MNT_" + intMountID.ToString() + "_SIZE"]), double.Parse(Request.Form["MNT_" + intMountID.ToString() + "_SIZE_QA"]), double.Parse(Request.Form["MNT_" + intMountID.ToString() + "_SIZE_TEST"]), (Request.Form["MNT_" + intMountID.ToString() + "_REPLICATED"] == "Yes" ? 1 : 0), (Request.Form["MNT_" + intMountID.ToString() + "_HIGH_AVAILABILITY"] == "Yes" ? 1 : 0));
                        }
                    }
                    if (intCluster > 0)
                        oCluster.UpdateAddInstance(intCluster, 1);
                    if (panFDrive.Visible == false)
                    {
                        DataSet ds = oServer.Get(intAnswer, intConfig, intCluster, intNumber);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            int intServer = Int32.Parse(ds.Tables[0].Rows[0]["id"].ToString());
                            if (oServer.Get(intServer, "fdrive") == "0")
                                oServer.UpdateFDrive(intServer, -1);
                        }
                    }
                    Response.Redirect(Request.Path + "?aid=" + intAnswer + "&clusterid=" + intCluster + "&csmid=" + intConfig + "&num=" + intNumber + (_close ? "&saveClose=true" : "&save=true"));
                }
            }
            else
            {
                if (intCluster > 0)
                    oCluster.UpdateAddInstance(intCluster, 1);
                Response.Redirect(Request.Path + "?aid=" + intAnswer + "&clusterid=" + intCluster + "&csmid=" + intConfig + "&num=" + intNumber + (_close ? "&saveClose=true" : "&save=true"));
            }
        }
    }
}
