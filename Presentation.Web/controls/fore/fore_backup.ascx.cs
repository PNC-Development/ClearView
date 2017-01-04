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
    public partial class fore_backup : System.Web.UI.UserControl
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intUnder48A = Int32.Parse(ConfigurationManager.AppSettings["DR_HOUR_RESPONSE"]);
        protected int intUnder48Q = Int32.Parse(ConfigurationManager.AppSettings["DR_HOUR_QUESTION"]);
        protected int intSharedPlatform = Int32.Parse(ConfigurationManager.AppSettings["SharedPlatformID"]);
        protected int intProfile;
        protected Forecast oForecast;
        protected Platforms oPlatform;
        protected ModelsProperties oModelsProperties;
        protected Locations oLocation;
        protected Classes oClass;
        protected int intForecast;
        protected int intID = 0;

        // Vijay code - start
        protected bool boolBackupInclusion = true;
        protected bool boolBackupExclusion = false;
        protected bool boolArchiveRequirement = false;
        protected bool boolAdditionalConfiguration = false;
        // Vijay code - end


        protected string strMenuTab1 = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oForecast = new Forecast(intProfile, dsn);
            oPlatform = new Platforms(intProfile, dsn);
            oModelsProperties = new ModelsProperties(intProfile, dsn);
            oLocation = new Locations(intProfile, dsn);
            oClass = new Classes(intProfile, dsn);


            //Menus
            int intMenuTab = 0;
            if (Request.QueryString["menu_tab"] != null && Request.QueryString["menu_tab"] != "")
                intMenuTab = Int32.Parse(Request.QueryString["menu_tab"]);
            Tab oTab = new Tab("", intMenuTab, "divMenu1", true, false);
            //Tab oTab = new Tab(hdnType.ClientID, intMenuTab, "divMenu1", true, false);
            oTab.AddTab("Backup Inclusions", "");
            oTab.AddTab("Backup Exclusions", "");
            oTab.AddTab("Archive Requirements", "");
            oTab.AddTab("Additional Configuration", "");
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
            if (!IsPostBack)
                LoadList();
            if (intID > 0)
            {
                DataSet ds = oForecast.GetAnswer(intID);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    int intClass = Int32.Parse(ds.Tables[0].Rows[0]["classid"].ToString());
                    int intAddress = Int32.Parse(ds.Tables[0].Rows[0]["addressid"].ToString());
                  
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
                    intForecast = Int32.Parse(ds.Tables[0].Rows[0]["forecastid"].ToString());
                    DataSet dsSteps = oForecast.GetSteps(intPlatform, 1);
                    if (dsSteps.Tables[0].Rows.Count == intStep)
                        btnNext.Text = "Finish";
                    if (intStep == 0 || intStep == 1)
                        btnBack.Enabled = false;
                    int intModel = oForecast.GetModel(intID);
                    if (intModel > 0)
                    {
                        panContinue.Visible = true;
                        Types oType = new Types(intProfile, dsn);
                        int intType = oModelsProperties.GetType(intModel);
                        if (oType.GetPlatform(intType) == intSharedPlatform)
                        {
                            panShared.Visible = true;
                            rptRetention2.DataSource = oForecast.GetBackupRetentions(intID);
                            rptRetention2.DataBind();
                            foreach (RepeaterItem ri in rptRetention2.Items)
                                ((LinkButton)ri.FindControl("btnDeleteRetention")).Attributes.Add("onclick", "return confirm('Are you sure you want to delete this retention requirement?');");
                            lblNoneRetention2.Visible = (rptRetention2.Items.Count == 0);
                            btnAddRetention2.Attributes.Add("onclick", "return OpenWindow('BACKUP_RETENTION','?id=" + intID.ToString() + "');");
                        }
                        else
                        {
                            panNonShared.Visible = true;
                            bool boolProduction = oClass.IsProd(Int32.Parse(ds.Tables[0].Rows[0]["classid"].ToString()));
                            bool boolNone = (ds.Tables[0].Rows[0]["backup"].ToString() == "-2");
                            if (boolProduction == true)
                            {
                                if (boolNone == true)
                                    divYes.Style["display"] = "inline";
                                //if (oLocation.GetAddress(intAddress, "common") == "1")
                                //{
                                //    // Only Data Centers require backups
                                //    radNo.Enabled = false;
                                //    radNo.ToolTip = "All production bound servers in a data center require a backup";
                                //}
                            }
                            if (!IsPostBack)
                            {
                                if (Request.QueryString["child"] != null)
                                {
                                    radYes.Checked = true;
                                    divYes.Style["display"] = "inline";
                                    if (Request.QueryString["daily"] != null)
                                        chkDaily.Checked = true;
                                    if (Request.QueryString["weekly"] != null)
                                    {
                                        chkWeekly.Checked = true;
                                        divWeekly.Style["display"] = "inline";
                                        ddlWeekly.Enabled = true;
                                        ddlWeekly.SelectedIndex = Int32.Parse(Request.QueryString["week"]);
                                    }
                                    if (Request.QueryString["monthly"] != null)
                                        chkMonthly.Checked = true;
                                    ddlTimeHour.Enabled = true;
                                    ddlTimeHour.SelectedIndex = Int32.Parse(Request.QueryString["hour"]);
                                    ddlTimeSwitch.Enabled = true;
                                    ddlTimeSwitch.SelectedIndex = Int32.Parse(Request.QueryString["switch"]);
                                    txtDate.Text = Request.QueryString["date"];
                                    ddlLocation.SelectedIndex = Int32.Parse(Request.QueryString["location"]);

                                }
                                else if (ds.Tables[0].Rows[0]["backup"].ToString() == "1")
                                {
                                    radYes.Checked = true;
                                    ds = oForecast.GetBackup(intID);
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        divYes.Style["display"] = "inline";
                                        if (ds.Tables[0].Rows[0]["daily"].ToString() == "1")
                                            chkDaily.Checked = true;
                                        if (ds.Tables[0].Rows[0]["weekly"].ToString() == "1")
                                        {
                                            chkWeekly.Checked = true;
                                            divWeekly.Style["display"] = "inline";
                                            if (ds.Tables[0].Rows[0]["weekly_day"].ToString() != "None")
                                            {
                                                ddlWeekly.Enabled = true;
                                                ddlWeekly.SelectedValue = ds.Tables[0].Rows[0]["weekly_day"].ToString();
                                            }
                                        }
                                        if (ds.Tables[0].Rows[0]["monthly"].ToString() == "1")
                                        {
                                            chkMonthly.Checked = true;
                                            //divMonthly.Style["display"] = "inline";
                                            //if (ds.Tables[0].Rows[0]["monthly_day"].ToString() != "None")
                                            //{
                                            //    ddlMonthlyDay.Enabled = true;
                                            //    ddlMonthlyDays.Enabled = true;
                                            //    ddlMonthlyDay.SelectedValue = ds.Tables[0].Rows[0]["monthly_day"].ToString();
                                            //    ddlMonthlyDays.SelectedValue = ds.Tables[0].Rows[0]["monthly_days"].ToString();
                                            //}
                                        }
                                        if (ds.Tables[0].Rows[0]["time"].ToString() == "1")
                                        {
                                            ddlTimeHour.Enabled = true;
                                            ddlTimeSwitch.Enabled = true;
                                            ddlTimeHour.SelectedValue = ds.Tables[0].Rows[0]["time_hour"].ToString();
                                            ddlTimeSwitch.SelectedValue = ds.Tables[0].Rows[0]["time_switch"].ToString();
                                        }
                                        txtDate.Text = ds.Tables[0].Rows[0]["start_date"].ToString();
                                        ddlLocation.SelectedValue = ds.Tables[0].Rows[0]["recoveryid"].ToString();
                                        txtCFPercent.Text = ds.Tables[0].Rows[0]["cf_percent"].ToString();
                                        txtCFCompression.Text = ds.Tables[0].Rows[0]["cf_compression"].ToString();
                                        ddlCFAverage.SelectedValue = ds.Tables[0].Rows[0]["cf_average"].ToString();
                                        txtCFBackup.Text = ds.Tables[0].Rows[0]["cf_backup"].ToString();
                                        txtCFArchive.Text = ds.Tables[0].Rows[0]["cf_archive"].ToString();
                                        txtCFWindow.Text = ds.Tables[0].Rows[0]["cf_window"].ToString();
                                        txtCFSets.Text = ds.Tables[0].Rows[0]["cf_sets"].ToString();
                                        ddlCDType.SelectedValue = ds.Tables[0].Rows[0]["cd_type"].ToString();
                                        txtCDPercent.Text = ds.Tables[0].Rows[0]["cd_percent"].ToString();
                                        txtCDCompression.Text = ds.Tables[0].Rows[0]["cd_compression"].ToString();
                                        txtCDVersions.Text = ds.Tables[0].Rows[0]["cd_versions"].ToString();
                                        txtCDWindow.Text = ds.Tables[0].Rows[0]["cd_window"].ToString();
                                        txtCDGrowth.Text = ds.Tables[0].Rows[0]["cd_growth"].ToString();
                                    }
                                }
                                else if (ds.Tables[0].Rows[0]["backup"].ToString() == "-1")
                                    radLater.Checked = true;
                                else if (ds.Tables[0].Rows[0]["backup"].ToString() == "0")
                                    radNo.Checked = true;
                                else if (boolProduction == true && boolNone == true)
                                    radYes.Checked = true;

                                rptExclusions.DataSource = oForecast.GetBackupExclusions(intID);
                                rptExclusions.DataBind();

                                foreach (RepeaterItem ri in rptExclusions.Items)
                                    ((LinkButton)ri.FindControl("btnDeleteExclusion")).Attributes.Add("onclick", "return confirm('Are you sure you want to delete this exclusion?');");
                                lblNoneExclusions.Visible = (rptExclusions.Items.Count == 0);

                                // Vijay code - start
                                rptInclusions.DataSource = oForecast.GetBackupInclusions(intID);
                                rptInclusions.DataBind();

                                foreach (RepeaterItem ri in rptInclusions.Items)
                                    ((LinkButton)ri.FindControl("btnDeleteInclusion")).Attributes.Add("onclick", "return confirm('Are you sure you want to delete this inclusion?');");
                                lblNoneInclusions.Visible = (rptInclusions.Items.Count == 0);
                                // Vijay code - end

                                rptRetention.DataSource = oForecast.GetBackupRetentions(intID);
                                rptRetention.DataBind();
                                foreach (RepeaterItem ri in rptRetention.Items)
                                    ((LinkButton)ri.FindControl("btnDeleteRetention")).Attributes.Add("onclick", "return confirm('Are you sure you want to delete this archive requirement?');");
                                lblNoneRetention.Visible = (rptRetention.Items.Count == 0);
                            }
                            if (boolProduction == true)
                                radNo.Attributes.Add("onclick", "ShowHideDiv('" + divNo.ClientID + "','inline');ShowHideDiv('" + divYes.ClientID + "','none');");
                            else
                                radNo.Attributes.Add("onclick", "ShowHideDiv('" + divYes.ClientID + "','none');");
                            radLater.Attributes.Add("onclick", "ShowHideDiv('" + divYes.ClientID + "','none');ShowHideDiv('" + divNo.ClientID + "','none');");
                            radYes.Attributes.Add("onclick", "ShowHideDiv('" + divYes.ClientID + "','inline');ShowHideDiv('" + divNo.ClientID + "','none');");

                            // Vijay code - start
                            chkDaily.Attributes.Add("onclick", "ForeBackCheck(this);");
                            chkWeekly.Attributes.Add("onclick", "ForeBackCheck(this);");
                            chkMonthly.Attributes.Add("onclick", "ForeBackCheck(this);");
                            // Vijay code - end                        

                            btnNext.Attributes.Add("onclick", "return EnsureBackup('" + radYes.ClientID + "','" + chkDaily.ClientID + "','" + chkWeekly.ClientID + "','" + ddlWeekly.ClientID + "','" + chkMonthly.ClientID + "','" + ddlMonthlyDay.ClientID + "','" + ddlMonthlyDays.ClientID + "','" + ddlTimeHour.ClientID + "','" + ddlTimeSwitch.ClientID + "','" + txtDate.ClientID + "','" + ddlLocation.ClientID + "');");
                            btnUpdate.Attributes.Add("onclick", "return EnsureBackup('" + radYes.ClientID + "','" + chkDaily.ClientID + "','" + chkWeekly.ClientID + "','" + ddlWeekly.ClientID + "','" + chkMonthly.ClientID + "','" + ddlMonthlyDay.ClientID + "','" + ddlMonthlyDays.ClientID + "','" + ddlTimeHour.ClientID + "','" + ddlTimeSwitch.ClientID + "','" + txtDate.ClientID + "','" + ddlLocation.ClientID + "');");
                            btnClose.Attributes.Add("onclick", "return window.close();");
                            btnAddExclusion.Attributes.Add("onclick", "return OpenWindow('BACKUP_EXCLUSION','?id=" + intID.ToString() + "');");
                            btnAddInclusion.Attributes.Add("onclick", "return OpenWindow('BACKUP_INCLUSION','?id=" + intID.ToString() + "');");
                            btnAddRetention.Attributes.Add("onclick", "return OpenWindow('BACKUP_RETENTION','?id=" + intID.ToString() + "');");
                            imgDate.Attributes.Add("onclick", "return ShowCalendar('" + txtDate.ClientID + "');");
                        }
                    }
                    else
                        panStop.Visible = true;
                }
            }
        }
        private void LoadList()
        {
            ddlLocation.DataTextField = "commonname";
            ddlLocation.DataValueField = "id";
            //ddlLocation.DataSource = oRecoveryLocations.Gets(1);
            ddlLocation.DataSource = oLocation.GetAddressRecovery();
            ddlLocation.DataBind();
            ddlLocation.Items.Insert(0, new ListItem("-- SELECT --", "0"));
        }
        protected int Save()
        {
            oForecast.DeleteBackup(intID);
            if (radYes.Checked == true)
            {
                string strWeekly = chkWeekly.Checked ? ddlWeekly.SelectedItem.Value : "0";
                string strMonthlyDay = ddlMonthlyDay.SelectedItem.Value;
                string strMonthlyDays = ddlMonthlyDays.SelectedItem.Value;
                oForecast.AddBackup(intID, (chkDaily.Checked ? 1 : 0), (chkWeekly.Checked ? 1 : 0), strWeekly, (chkMonthly.Checked ? 1 : 0), strMonthlyDay, strMonthlyDays, 1, ddlTimeHour.SelectedItem.Value, ddlTimeSwitch.SelectedItem.Value, txtDate.Text, Int32.Parse(ddlLocation.SelectedItem.Value), txtCFPercent.Text, txtCFCompression.Text, ddlCFAverage.SelectedItem.Value, txtCFBackup.Text, txtCFArchive.Text, txtCFWindow.Text, txtCFSets.Text, ddlCDType.SelectedItem.Value, txtCDPercent.Text, txtCDCompression.Text, txtCDVersions.Text, txtCDWindow.Text, txtCDGrowth.Text, (txtAverage.Text == "" ? 0 : Int32.Parse(txtAverage.Text)), txtDocumentation.Text);
            }
            // Check to see if step is done
            int intBackup = 0;
            if (radLater.Checked == true)
                intBackup = -1;
            else if (radYes.Checked == true)
                intBackup = 1;
            oForecast.UpdateBackup(intID, intBackup);
            return (radNo.Checked == true || radYes.Checked == true || panShared.Visible == true ? 1 : 0);
        }
        protected void btnBack_Click(Object Sender, EventArgs e)
        {
            oForecast.UpdateAnswerStep(intID, -1, -1);
            Response.Redirect(Request.Path + "?id=" + intID.ToString());
        }
        protected void btnNext_Click(Object Sender, EventArgs e)
        {
            int intDone = Save();
            oForecast.UpdateAnswerStep(intID, 1, intDone);
            Response.Redirect(Request.Path + "?id=" + intID.ToString() + "&save=true");
        }

        protected void btnUpdate_Click(Object Sender, EventArgs e)
        {
            int intDone = Save();
            int intStep = Int32.Parse(Request.QueryString["step"]);
            string strAlert = oForecast.AddStepDone(intID, intStep, intDone);
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "save", "<script type=\"text/javascript\">" + strAlert + "window.navigate('" + Request.Path + "?id=" + intID.ToString() + "&save=true');<" + "/" + "script>");
        }
        protected void btnCancel_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path + "?id=" + intID.ToString());
        }
        protected void btnDeleteExclusion_Click(Object Sender, EventArgs e)
        {
            LinkButton oButton = (LinkButton)Sender;
            oForecast.DeleteBackupExclusion(Int32.Parse(oButton.CommandArgument));
            Response.Redirect(Request.Path + "?id=" + intID.ToString() + "&child=true" + LoadQueryString() + "&menu_tab=2");
        }
        // Vijay code - start
        protected void btnDeleteInclusion_Click(Object Sender, EventArgs e)
        {
            LinkButton oButton = (LinkButton)Sender;
            oForecast.DeleteBackupInclusion(Int32.Parse(oButton.CommandArgument));
            Response.Redirect(Request.Path + "?id=" + intID.ToString() + "&child=true" + LoadQueryString() + "&menu_tab=1");
        }
        // Vijay code - end
        protected void btnDeleteRetention_Click(Object Sender, EventArgs e)
        {
            LinkButton oButton = (LinkButton)Sender;
            oForecast.DeleteBackupRetention(Int32.Parse(oButton.CommandArgument));
            Response.Redirect(Request.Path + "?id=" + intID.ToString() + "&child=true" + LoadQueryString() + "&menu_tab=3");
        }
        private string LoadQueryString()
        {
            string strURL = "";
            if (panNonShared.Visible == true)
            {
                if (chkDaily.Checked == true)
                    strURL += "&daily=true";
                if (chkWeekly.Checked == true)
                {
                    strURL += "&weekly=true";
                    strURL += "&week=" + ddlWeekly.SelectedIndex.ToString();
                }
                if (chkMonthly.Checked == true)
                    strURL += "&monthly=true";
                strURL += "&hour=" + ddlTimeHour.SelectedIndex.ToString();
                strURL += "&switch=" + ddlTimeSwitch.SelectedIndex.ToString();
                // Vijay code - start
                strURL += "&date=" + txtDate.Text;
                strURL += "&location=" + ddlLocation.SelectedIndex.ToString();
                // Vijay code - end
            }
            return strURL;
        }
    }
}