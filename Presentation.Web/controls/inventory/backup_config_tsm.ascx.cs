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
using System.IO;
using NCC.ClearView.Presentation.Web.Custom;

namespace NCC.ClearView.Presentation.Web
{
    public partial class backup_config_tsm : System.Web.UI.UserControl
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected Pages oPage;
        protected Users oUser;
        protected Servers oServer;
        protected TSM oTSM;
        protected Mnemonic oMnemonic;
        protected Locations oLocation;
        protected Resiliency oResiliency;
        protected Functions oFunctions;
        protected int intProfile;
        protected int intApplication = 0;
        protected int intPage = 0;
        protected int intServer = 0;
        protected int intDomain = 0;
        protected int intSchedule = 0;
        protected int intMnemonic = 0;
        protected string strLocation = "";
        protected string strMenuTab1 = "";
        private string strSpacer = "&nbsp;&nbsp;&gt;&gt;&nbsp;&nbsp;";
        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oPage = new Pages(intProfile, dsn);
            oUser = new Users(intProfile, dsn);
            oServer = new Servers(intProfile, dsn);
            oTSM = new TSM(intProfile, dsn);
            oMnemonic = new Mnemonic(intProfile, dsn);
            oLocation = new Locations(intProfile, dsn);
            oResiliency = new Resiliency(intProfile, dsn);
            oFunctions = new Functions(intProfile, dsn, intEnvironment);
            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            if (Request.QueryString["saved"] != null)
                lblMessage.Text = oFunctions.BuildBox("/images/ico_check.gif", "Information Saved", "Your change has been saved successfully!", "box_green header");
            if (Request.QueryString["deleted"] != null)
                lblMessage.Text = oFunctions.BuildBox("/images/ico_check.gif", "Record Deleted", "The record has been deleted successfully!", "box_green header");
            if (Request.QueryString["server"] != null && Request.QueryString["server"] != "")
            {
                intServer = Int32.Parse(Request.QueryString["server"]);
                lblCrumbs.Text += "<a href=\"" + oPage.GetFullLink(intPage) + FormURL("") + "\">Back to Home</a>";
                if (Request.QueryString["mnemonic"] != null && Request.QueryString["mnemonic"] != "")
                {
                    panMnemonic.Visible = true;
                    Int32.TryParse(Request.QueryString["mnemonic"], out intMnemonic);
                    if (!IsPostBack)
                    {
                        DataSet ds = oTSM.GetMnemonic(intMnemonic);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            int intMnemonicID = Int32.Parse(ds.Tables[0].Rows[0]["mnemonicid"].ToString());
                            hdnMnemonic.Value = intMnemonicID.ToString();
                            txtMnemonic.Text = oMnemonic.Get(intMnemonicID, "factory_code") + " - " + oMnemonic.Get(intMnemonicID, "name");
                            chkMnemonicEnabled.Checked = (ds.Tables[0].Rows[0]["enabled"].ToString() == "1");
                            btnMnemonicUpdate.Visible = true;
                            btnMnemonicUpdate.Attributes.Add("onclick", "return ValidateHidden0('" + hdnMnemonic.ClientID + "','" + txtMnemonic.ClientID + "','Please enter a mnemonic\\n\\n(Start typing and a list will be presented...)')" +
                                " && ProcessButton(this)" +
                                ";");
                            btnMnemonicDelete.Visible = true;
                            btnMnemonicDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this item') && ProcessButton(this);");
                        }
                        else
                        {
                            btnMnemonicAdd.Visible = true;
                            btnMnemonicAdd.Attributes.Add("onclick", "return ValidateHidden0('" + hdnMnemonic.ClientID + "','" + txtMnemonic.ClientID + "','Please enter a mnemonic\\n\\n(Start typing and a list will be presented...)')" +
                                " && ProcessButton(this)" +
                                ";");
                        }
                        btnMnemonicCancel.Attributes.Add("onclick", "return ProcessButton(this);");
                    }
                }
                else if (Request.QueryString["domain"] != null && Request.QueryString["domain"] != "")
                {
                    intDomain = Int32.Parse(Request.QueryString["domain"]);
                    lblCrumbs.Text += strSpacer + "<a href=\"" + oPage.GetFullLink(intPage) + FormURL("server=" + intServer.ToString()) + "\">" + oTSM.Get(intServer, "name") + "</a>";
                    if (Request.QueryString["schedule"] != null && Request.QueryString["schedule"] != "")
                    {
                        intSchedule = Int32.Parse(Request.QueryString["schedule"]);
                        lblCrumbs.Text += strSpacer + "<a href=\"" + oPage.GetFullLink(intPage) + FormURL("server=" + intServer.ToString() + "&domain=" + intDomain.ToString()) + "\">" + oTSM.GetDomain(intDomain, "name") + "</a>";
                        panSchedule.Visible = true;
                        if (!IsPostBack)
                        {
                            DataSet ds = oTSM.GetSchedule(intSchedule);
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                lblCrumbs.Text += strSpacer + oTSM.GetSchedule(intSchedule, "name");
                                txtScheduleName.Text = ds.Tables[0].Rows[0]["name"].ToString();
                                chkScheduleEngineering.Checked = (ds.Tables[0].Rows[0]["engineering"].ToString() == "1");
                                chkScheduleTest.Checked = (ds.Tables[0].Rows[0]["test"].ToString() == "1");
                                chkScheduleQA.Checked = (ds.Tables[0].Rows[0]["qa"].ToString() == "1");
                                chkScheduleProduction.Checked = (ds.Tables[0].Rows[0]["prod"].ToString() == "1");
                                chkScheduleWindows.Checked = (ds.Tables[0].Rows[0]["windows"].ToString() == "1");
                                chkScheduleUnix.Checked = (ds.Tables[0].Rows[0]["unix"].ToString() == "1");
                                chkScheduleDaily.Checked = (ds.Tables[0].Rows[0]["daily"].ToString() == "1");
                                chkScheduleWeekly.Checked = (ds.Tables[0].Rows[0]["weekly"].ToString() == "1");
                                if (chkScheduleWeekly.Checked == true)
                                    divScheduleWeekly.Style["display"] = "inline";
                                ddlScheduleResiliency.SelectedValue = ds.Tables[0].Rows[0]["resiliencyid"].ToString();
                                chkScheduleWeeklySunday.Checked = (ds.Tables[0].Rows[0]["sunday"].ToString() == "1");
                                chkScheduleWeeklyMonday.Checked = (ds.Tables[0].Rows[0]["monday"].ToString() == "1");
                                chkScheduleWeeklyTuesday.Checked = (ds.Tables[0].Rows[0]["tuesday"].ToString() == "1");
                                chkScheduleWeeklyWednesday.Checked = (ds.Tables[0].Rows[0]["wednesday"].ToString() == "1");
                                chkScheduleWeeklyThursday.Checked = (ds.Tables[0].Rows[0]["thursday"].ToString() == "1");
                                chkScheduleWeeklyFriday.Checked = (ds.Tables[0].Rows[0]["friday"].ToString() == "1");
                                chkScheduleWeeklySaturday.Checked = (ds.Tables[0].Rows[0]["saturday"].ToString() == "1");
                                chkScheduleMonthly.Checked = (ds.Tables[0].Rows[0]["monthly"].ToString() == "1");
                                chk1200AM.Checked = (ds.Tables[0].Rows[0]["AM1200"].ToString() == "1");
                                chk1230AM.Checked = (ds.Tables[0].Rows[0]["AM1230"].ToString() == "1");
                                chk100AM.Checked = (ds.Tables[0].Rows[0]["AM100"].ToString() == "1");
                                chk130AM.Checked = (ds.Tables[0].Rows[0]["AM130"].ToString() == "1");
                                chk200AM.Checked = (ds.Tables[0].Rows[0]["AM200"].ToString() == "1");
                                chk230AM.Checked = (ds.Tables[0].Rows[0]["AM230"].ToString() == "1");
                                chk300AM.Checked = (ds.Tables[0].Rows[0]["AM300"].ToString() == "1");
                                chk330AM.Checked = (ds.Tables[0].Rows[0]["AM330"].ToString() == "1");
                                chk400AM.Checked = (ds.Tables[0].Rows[0]["AM400"].ToString() == "1");
                                chk430AM.Checked = (ds.Tables[0].Rows[0]["AM430"].ToString() == "1");
                                chk500AM.Checked = (ds.Tables[0].Rows[0]["AM500"].ToString() == "1");
                                chk530AM.Checked = (ds.Tables[0].Rows[0]["AM530"].ToString() == "1");
                                chk600AM.Checked = (ds.Tables[0].Rows[0]["AM600"].ToString() == "1");
                                chk630AM.Checked = (ds.Tables[0].Rows[0]["AM630"].ToString() == "1");
                                chk700AM.Checked = (ds.Tables[0].Rows[0]["AM700"].ToString() == "1");
                                chk730AM.Checked = (ds.Tables[0].Rows[0]["AM730"].ToString() == "1");
                                chk800AM.Checked = (ds.Tables[0].Rows[0]["AM800"].ToString() == "1");
                                chk830AM.Checked = (ds.Tables[0].Rows[0]["AM830"].ToString() == "1");
                                chk900AM.Checked = (ds.Tables[0].Rows[0]["AM900"].ToString() == "1");
                                chk930AM.Checked = (ds.Tables[0].Rows[0]["AM930"].ToString() == "1");
                                chk1000AM.Checked = (ds.Tables[0].Rows[0]["AM1000"].ToString() == "1");
                                chk1030AM.Checked = (ds.Tables[0].Rows[0]["AM1030"].ToString() == "1");
                                chk1100AM.Checked = (ds.Tables[0].Rows[0]["AM1100"].ToString() == "1");
                                chk1130AM.Checked = (ds.Tables[0].Rows[0]["AM1130"].ToString() == "1");
                                chk1200PM.Checked = (ds.Tables[0].Rows[0]["PM1200"].ToString() == "1");
                                chk1230PM.Checked = (ds.Tables[0].Rows[0]["PM1230"].ToString() == "1");
                                chk100PM.Checked = (ds.Tables[0].Rows[0]["PM100"].ToString() == "1");
                                chk130PM.Checked = (ds.Tables[0].Rows[0]["PM130"].ToString() == "1");
                                chk200PM.Checked = (ds.Tables[0].Rows[0]["PM200"].ToString() == "1");
                                chk230PM.Checked = (ds.Tables[0].Rows[0]["PM230"].ToString() == "1");
                                chk300PM.Checked = (ds.Tables[0].Rows[0]["PM300"].ToString() == "1");
                                chk330PM.Checked = (ds.Tables[0].Rows[0]["PM330"].ToString() == "1");
                                chk400PM.Checked = (ds.Tables[0].Rows[0]["PM400"].ToString() == "1");
                                chk430PM.Checked = (ds.Tables[0].Rows[0]["PM430"].ToString() == "1");
                                chk500PM.Checked = (ds.Tables[0].Rows[0]["PM500"].ToString() == "1");
                                chk530PM.Checked = (ds.Tables[0].Rows[0]["PM530"].ToString() == "1");
                                chk600PM.Checked = (ds.Tables[0].Rows[0]["PM600"].ToString() == "1");
                                chk630PM.Checked = (ds.Tables[0].Rows[0]["PM630"].ToString() == "1");
                                chk700PM.Checked = (ds.Tables[0].Rows[0]["PM700"].ToString() == "1");
                                chk730PM.Checked = (ds.Tables[0].Rows[0]["PM730"].ToString() == "1");
                                chk800PM.Checked = (ds.Tables[0].Rows[0]["PM800"].ToString() == "1");
                                chk830PM.Checked = (ds.Tables[0].Rows[0]["PM830"].ToString() == "1");
                                chk900PM.Checked = (ds.Tables[0].Rows[0]["PM900"].ToString() == "1");
                                chk930PM.Checked = (ds.Tables[0].Rows[0]["PM930"].ToString() == "1");
                                chk1000PM.Checked = (ds.Tables[0].Rows[0]["PM1000"].ToString() == "1");
                                chk1030PM.Checked = (ds.Tables[0].Rows[0]["PM1030"].ToString() == "1");
                                chk1100PM.Checked = (ds.Tables[0].Rows[0]["PM1100"].ToString() == "1");
                                chk1130PM.Checked = (ds.Tables[0].Rows[0]["PM1130"].ToString() == "1");
                                chkScheduleEnabled.Checked = (ds.Tables[0].Rows[0]["enabled"].ToString() == "1");
                                btnScheduleUpdate.Visible = true;
                                btnScheduleUpdate.Attributes.Add("onclick", "return ValidateText('" + txtScheduleName.ClientID + "','Please enter a name for this schedule')" +
                                " && ProcessButton(this)" +
                                    ";");
                                btnScheduleDelete.Visible = true;
                                btnScheduleDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this schedule')" +
                                    " && ProcessButton(this)" +
                                    ";");
                            }
                            else
                            {
                                btnScheduleAdd.Visible = true;
                                btnScheduleAdd.Attributes.Add("onclick", "return ValidateText('" + txtScheduleName.ClientID + "','Please enter a name for this schedule')" +
                                " && ProcessButton(this)" +
                                    ";");
                            }
                            btnScheduleCancel.Attributes.Add("onclick", "return ProcessButton(this);");
                            chkScheduleWeekly.Attributes.Add("onclick", "ShowHideDiv2('" + divScheduleWeekly.ClientID + "');");
                        }
                    }
                    else
                    {
                        panDomain.Visible = true;
                        DataSet dsSchedules = oTSM.GetSchedules(intDomain, 0);
                        DataView dvSchedules = dsSchedules.Tables[0].DefaultView;
                        if (Request.QueryString["sort"] != null)
                            dvSchedules.Sort = Request.QueryString["sort"];
                        else
                            dvSchedules.Sort = "name";
                        rptSchedules.DataSource = dvSchedules;
                        rptSchedules.DataBind();
                        lblSchedules.Visible = (rptSchedules.Items.Count == 0);
                        if (!IsPostBack)
                        {
                            LoadLists();
                            DataSet ds = oTSM.GetDomain(intDomain);
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                lblCrumbs.Text += strSpacer + oTSM.GetDomain(intDomain, "name");
                                txtDomainName.Text = ds.Tables[0].Rows[0]["name"].ToString();
                                chkDomainEngineering.Checked = (ds.Tables[0].Rows[0]["engineering"].ToString() == "1");
                                chkDomainTest.Checked = (ds.Tables[0].Rows[0]["test"].ToString() == "1");
                                chkDomainQA.Checked = (ds.Tables[0].Rows[0]["qa"].ToString() == "1");
                                chkDomainProduction.Checked = (ds.Tables[0].Rows[0]["prod"].ToString() == "1");
                                chkDomainWindows.Checked = (ds.Tables[0].Rows[0]["windows"].ToString() == "1");
                                chkDomainUnix.Checked = (ds.Tables[0].Rows[0]["unix"].ToString() == "1");
                                ddlDomainResiliency.SelectedValue = ds.Tables[0].Rows[0]["resiliencyid"].ToString();
                                chkDomainEnabled.Checked = (ds.Tables[0].Rows[0]["enabled"].ToString() == "1");
                                btnDomainUpdate.Visible = true;
                                btnDomainUpdate.Attributes.Add("onclick", "return ValidateText('" + txtDomainName.ClientID + "','Please enter a name for this domain')" +
                                " && ValidateDropDown('" + ddlDomainResiliency.ClientID + "','Please select a Resiliency')" +
                                " && ProcessButton(this)" +
                                    ";");
                                btnDomainDelete.Visible = true;
                                btnDomainDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this domain')" +
                                    " && ProcessButton(this)" +
                                    ";");
                            }
                            else
                            {
                                btnDomainAdd.Visible = true;
                                btnDomainAdd.Attributes.Add("onclick", "return ValidateText('" + txtDomainName.ClientID + "','Please enter a name for this domain')" +
                                " && ValidateDropDown('" + ddlDomainResiliency.ClientID + "','Please select a Resiliency')" +
                                " && ProcessButton(this)" +
                                    ";");
                                btnAddSchedule.Enabled = false;
                            }
                            btnDomainCancel.Attributes.Add("onclick", "return ProcessButton(this);");
                        }
                    }
                }
                else
                {
                    panServer.Visible = true;
                    DataSet dsDomains = oTSM.GetDomains(intServer, 0);
                    DataView dvDomains = dsDomains.Tables[0].DefaultView;
                    if (Request.QueryString["sort"] != null)
                        dvDomains.Sort = Request.QueryString["sort"];
                    else
                        dvDomains.Sort = "name";
                    rptDomains.DataSource = dvDomains;
                    rptDomains.DataBind();
                    lblDomains.Visible = (rptDomains.Items.Count == 0);
                    // Mnemonics
                    rptMnemonics.DataSource = oTSM.GetMnemonics(intServer, 0);
                    rptMnemonics.DataBind();
                    lblMnemonics.Visible = (rptMnemonics.Items.Count == 0);
                    if (!IsPostBack)
                    {
                        int intLocation = Int32.Parse(ConfigurationManager.AppSettings["OPSLocationID"]);
                        DataSet ds = oTSM.Get(intServer);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            lblCrumbs.Text += strSpacer + oTSM.Get(intServer, "name");
                            txtServerName.Text = ds.Tables[0].Rows[0]["name"].ToString();
                            txtServerPort.Text = ds.Tables[0].Rows[0]["port"].ToString();
                            Int32.TryParse(ds.Tables[0].Rows[0]["addressid"].ToString(), out intLocation);
                            txtServerPath.Text = ds.Tables[0].Rows[0]["path"].ToString();
                            if (txtServerPath.Text != "")
                                hypServerPath.NavigateUrl = txtServerPath.Text;
                            else
                                hypServerPath.Enabled = false;
                            chkServerEnabled.Checked = (ds.Tables[0].Rows[0]["enabled"].ToString() == "1");
                            btnServerUpdate.Visible = true;
                            btnServerUpdate.Attributes.Add("onclick", "return ValidateText('" + txtServerName.ClientID + "','Please enter a name for this server')" +
                                " && ValidateNumber0('" + txtServerPort.ClientID + "','Please enter a port number')" +
                                " && ProcessButton(this)" +
                                ";");
                            btnServerDelete.Visible = true;
                            btnServerDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this server')" +
                                " && ProcessButton(this)" +
                                ";");
                        }
                        else
                        {
                            btnServerAdd.Visible = true;
                            btnServerAdd.Attributes.Add("onclick", "return ValidateText('" + txtServerName.ClientID + "','Please enter a name for this server')" +
                                " && ValidateNumber0('" + txtServerPort.ClientID + "','Please enter a port number')" +
                                " && ProcessButton(this)" +
                                ";");
                            btnAddDomain.Enabled = false;
                            btnAddMnemonic.Enabled = false;
                        }
                        btnServerCancel.Attributes.Add("onclick", "return ProcessButton(this);");
                        strLocation = oLocation.LoadDDL("ddlState", "ddlCity", "ddlAddress", hdnLocation.ClientID, intLocation, true, "ddlCommon");
                        hdnLocation.Value = intLocation.ToString();
                    }
                    int intMenuTab = 0;
                    if (Request.QueryString["menu_tab"] != null && Request.QueryString["menu_tab"] != "")
                        intMenuTab = Int32.Parse(Request.QueryString["menu_tab"]);
                    Tab oTab = new Tab(hdnTab.ClientID, intMenuTab, "divMenu1", true, false);
                    oTab.AddTab("Domains", "");
                    oTab.AddTab("Associated Mnemonics", "");
                    strMenuTab1 = oTab.GetTabs();
                }
            }
            else
            {
                panServers.Visible = true;
                DataSet dsTSM = oTSM.Gets(1, 0, 0);
                DataView dvTSM = dsTSM.Tables[0].DefaultView;
                if (Request.QueryString["sort"] != null)
                    dvTSM.Sort = Request.QueryString["sort"];
                else
                    dvTSM.Sort = "name";
                rptServers.DataSource = dvTSM;
                rptServers.DataBind();
                lblServers.Visible = (rptServers.Items.Count == 0);
            }
            Variables oVariable = new Variables(intEnvironment);
            txtMnemonic.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'500','195','" + divMnemonic.ClientID + "','" + lstMnemonic.ClientID + "','" + hdnMnemonic.ClientID + "','" + oVariable.URL() + "/frame/ajax/ajax_mnemonics.aspx',2);");
            lstMnemonic.Attributes.Add("ondblclick", "AJAXClickRow();");
            btnAddDomain.Attributes.Add("onclick", "return ProcessButton(this);");
            btnAddSchedule.Attributes.Add("onclick", "return ProcessButton(this);");
            btnAddServer.Attributes.Add("onclick", "return ProcessButton(this);");
        }
        protected void LoadLists()
        {
            ddlScheduleResiliency.DataValueField = "id";
            ddlScheduleResiliency.DataTextField = "name";
            ddlScheduleResiliency.DataSource = oResiliency.Gets(1);
            ddlScheduleResiliency.DataBind();
            ddlScheduleResiliency.Items.Insert(0, new ListItem("-- SELECT --", "0"));
            ddlDomainResiliency.DataValueField = "id";
            ddlDomainResiliency.DataTextField = "name";
            ddlDomainResiliency.DataSource = oResiliency.Gets(1);
            ddlDomainResiliency.DataBind();
            ddlDomainResiliency.Items.Insert(0, new ListItem("-- SELECT --", "0"));
        }
        protected void btnOrder_Click(Object Sender, EventArgs e)
        {
            LinkButton oOrder = (LinkButton)Sender;
            string strOrder = "";
            if (Request.QueryString["sort"] != null)
            {
                if (Request.QueryString["sort"] == oOrder.CommandArgument)
                    strOrder = oOrder.CommandArgument + " DESC";
            }
            if (strOrder == "")
                strOrder = oOrder.CommandArgument;
            string strRedirect = "";
            strRedirect += BuildURL("id", strRedirect);
            strRedirect += BuildURL("tid", strRedirect);
            strRedirect += BuildURL("server", strRedirect);
            strRedirect += BuildURL("domain", strRedirect);
            strRedirect += BuildURL("schedule", strRedirect);
            if (strRedirect == "")
                strRedirect = "?";
            else
                strRedirect += "&";
            Response.Redirect(oPage.GetFullLink(intPage) + strRedirect + "sort=" + strOrder);
        }
        protected void btnAddServer_Click(Object Sender, EventArgs e)
        {
            Redirect("server=0");
        }
        protected void btnServerAdd_Click(Object Sender, EventArgs e)
        {
            oTSM.Add(txtServerName.Text, Int32.Parse(txtServerPort.Text), Int32.Parse(Request.Form[hdnLocation.UniqueID]), SaveFile(), 1, 0, oTSM.Gets(1, 0, 1).Tables[0].Rows.Count + 1, (chkServerEnabled.Checked ? 1 : 0));
            Redirect("saved=true");
        }
        protected void btnServerUpdate_Click(Object Sender, EventArgs e)
        {
            oTSM.Update(intServer, txtServerName.Text, Int32.Parse(txtServerPort.Text), Int32.Parse(Request.Form[hdnLocation.UniqueID]), SaveFile(), 1, 0, (chkServerEnabled.Checked ? 1 : 0));
            Redirect("server=" + intServer.ToString() + "&saved=true");
        }
        protected string SaveFile()
        {
            string strPath = txtServerPath.Text;
            if (txtFile.FileName != "" && txtFile.PostedFile != null)
            {
                Variables oVariable = new Variables(intEnvironment);
                string strDirectory = oVariable.DocumentsFolder() + "tsm";
                if (Directory.Exists(strDirectory) == false)
                    Directory.CreateDirectory(strDirectory);
                string strFile = txtFile.PostedFile.FileName.Trim();
                string strFileName = strFile.Substring(strFile.LastIndexOf("\\") + 1);
                string strExtension = txtFile.FileName;
                strExtension = strExtension.Substring(strExtension.LastIndexOf("."));
                DateTime _now = DateTime.Now;
                string strNow = _now.Day.ToString() + _now.Month.ToString() + _now.Year.ToString() + _now.Hour.ToString() + _now.Minute.ToString() + _now.Second.ToString() + _now.Millisecond.ToString();
                if (strPath != "")
                {
                    try
                    {
                        File.Delete(strPath);
                    }
                    catch { }
                }
                strPath = strDirectory + "\\" + strNow + strExtension;
                txtFile.PostedFile.SaveAs(strPath);
            }
            return strPath;
        }
        protected void btnServerDelete_Click(Object Sender, EventArgs e)
        {
            oTSM.Delete(intServer);
            Redirect("deleted=true");
        }
        protected void btnServerCancel_Click(Object Sender, EventArgs e)
        {
            Redirect("");
        }
        protected void btnAddDomain_Click(Object Sender, EventArgs e)
        {
            Redirect("server=" + intServer.ToString() + "&domain=0");
        }
        protected void btnDomainAdd_Click(Object Sender, EventArgs e)
        {
            int intResiliency = 0;
            if (ddlDomainResiliency.SelectedItem != null)
                Int32.TryParse(ddlDomainResiliency.SelectedItem.Value, out intResiliency);
            oTSM.AddDomain(intServer, txtDomainName.Text, (chkDomainEngineering.Checked ? 1 : 0), (chkDomainTest.Checked ? 1 : 0), (chkDomainQA.Checked ? 1 : 0), (chkDomainProduction.Checked ? 1 : 0), (chkDomainWindows.Checked ? 1 : 0), (chkDomainUnix.Checked ? 1 : 0), intResiliency, oTSM.GetDomains(intServer, 1).Tables[0].Rows.Count + 1, (chkDomainEnabled.Checked ? 1 : 0));
            Redirect("server=" + intServer.ToString() + "&saved=true");
        }
        protected void btnDomainUpdate_Click(Object Sender, EventArgs e)
        {
            int intResiliency = 0;
            if (ddlDomainResiliency.SelectedItem != null)
                Int32.TryParse(ddlDomainResiliency.SelectedItem.Value, out intResiliency);
            oTSM.UpdateDomain(intDomain, intServer, txtDomainName.Text, (chkDomainEngineering.Checked ? 1 : 0), (chkDomainTest.Checked ? 1 : 0), (chkDomainQA.Checked ? 1 : 0), (chkDomainProduction.Checked ? 1 : 0), (chkDomainWindows.Checked ? 1 : 0), (chkDomainUnix.Checked ? 1 : 0), intResiliency, (chkDomainEnabled.Checked ? 1 : 0));
            Redirect("server=" + intServer.ToString() + "&domain=" + intDomain.ToString() + "&saved=true");
        }
        protected void btnDomainDelete_Click(Object Sender, EventArgs e)
        {
            oTSM.DeleteDomain(intDomain);
            Redirect("server=" + intServer.ToString() + "&deleted=true");
        }
        protected void btnDomainCancel_Click(Object Sender, EventArgs e)
        {
            Redirect("server=" + intServer.ToString());
        }
        protected void btnAddMnemonic_Click(Object Sender, EventArgs e)
        {
            Redirect("server=" + intServer.ToString() + "&mnemonic=0");
        }
        protected void btnMnemonicAdd_Click(Object Sender, EventArgs e)
        {
            int intMnemonicID = 0;
            Int32.TryParse(Request.Form[hdnMnemonic.UniqueID], out intMnemonicID);
            if (intMnemonicID > 0)
                oTSM.AddMnemonic(intServer, intMnemonicID, (chkMnemonicEnabled.Checked ? 1 : 0));
            Redirect("server=" + intServer.ToString() + "&menu_tab=2" + "&saved=true");
        }
        protected void btnMnemonicUpdate_Click(Object Sender, EventArgs e)
        {
            int intMnemonicID = 0;
            Int32.TryParse(Request.Form[hdnMnemonic.UniqueID], out intMnemonicID);
            oTSM.UpdateMnemonic(intMnemonic, intServer, intMnemonicID, (chkMnemonicEnabled.Checked ? 1 : 0));
            Redirect("server=" + intServer.ToString() + "&mnemonic=" + intMnemonic.ToString() + "&saved=true");
        }
        protected void btnMnemonicDelete_Click(Object Sender, EventArgs e)
        {
            oTSM.DeleteMnemonic(intMnemonic);
            Redirect("server=" + intServer.ToString() + "&menu_tab=2" + "&delete=true");
        }
        protected void btnMnemonicCancel_Click(Object Sender, EventArgs e)
        {
            Redirect("server=" + intServer.ToString() + "&menu_tab=2");
        }
        protected void btnAddSchedule_Click(Object Sender, EventArgs e)
        {
            Redirect("server=" + intServer.ToString() + "&domain=" + intDomain.ToString() + "&schedule=0");
        }
        protected void btnScheduleAdd_Click(Object Sender, EventArgs e)
        {
            int intResiliency = 0;
            if (ddlScheduleResiliency.SelectedItem != null)
                Int32.TryParse(ddlScheduleResiliency.SelectedItem.Value, out intResiliency);
            oTSM.AddSchedule(intDomain, txtScheduleName.Text, (chkScheduleEngineering.Checked ? 1 : 0), (chkScheduleTest.Checked ? 1 : 0), (chkScheduleQA.Checked ? 1 : 0), (chkScheduleProduction.Checked ? 1 : 0), (chkScheduleWindows.Checked ? 1 : 0), (chkScheduleUnix.Checked ? 1 : 0), (chkScheduleDaily.Checked ? 1 : 0), (chkScheduleWeekly.Checked ? 1 : 0), intResiliency, (chkScheduleWeeklySunday.Checked ? 1 : 0), (chkScheduleWeeklyMonday.Checked ? 1 : 0), (chkScheduleWeeklyTuesday.Checked ? 1 : 0), (chkScheduleWeeklyWednesday.Checked ? 1 : 0), (chkScheduleWeeklyThursday.Checked ? 1 : 0), (chkScheduleWeeklyFriday.Checked ? 1 : 0), (chkScheduleWeeklySaturday.Checked ? 1 : 0), (chkScheduleMonthly.Checked ? 1 : 0), (chk1200AM.Checked ? 1 : 0), (chk1230AM.Checked ? 1 : 0), (chk100AM.Checked ? 1 : 0), (chk130AM.Checked ? 1 : 0), (chk200AM.Checked ? 1 : 0), (chk230AM.Checked ? 1 : 0), (chk300AM.Checked ? 1 : 0), (chk330AM.Checked ? 1 : 0), (chk400AM.Checked ? 1 : 0), (chk430AM.Checked ? 1 : 0), (chk500AM.Checked ? 1 : 0), (chk530AM.Checked ? 1 : 0), (chk600AM.Checked ? 1 : 0), (chk630AM.Checked ? 1 : 0), (chk700AM.Checked ? 1 : 0), (chk730AM.Checked ? 1 : 0), (chk800AM.Checked ? 1 : 0), (chk830AM.Checked ? 1 : 0), (chk900AM.Checked ? 1 : 0), (chk930AM.Checked ? 1 : 0), (chk1000AM.Checked ? 1 : 0), (chk1030AM.Checked ? 1 : 0), (chk1100AM.Checked ? 1 : 0), (chk1130AM.Checked ? 1 : 0), (chk1200PM.Checked ? 1 : 0), (chk1230PM.Checked ? 1 : 0), (chk100PM.Checked ? 1 : 0), (chk130PM.Checked ? 1 : 0), (chk200PM.Checked ? 1 : 0), (chk230PM.Checked ? 1 : 0), (chk300PM.Checked ? 1 : 0), (chk330PM.Checked ? 1 : 0), (chk400PM.Checked ? 1 : 0), (chk430PM.Checked ? 1 : 0), (chk500PM.Checked ? 1 : 0), (chk530PM.Checked ? 1 : 0), (chk600PM.Checked ? 1 : 0), (chk630PM.Checked ? 1 : 0), (chk700PM.Checked ? 1 : 0), (chk730PM.Checked ? 1 : 0), (chk800PM.Checked ? 1 : 0), (chk830PM.Checked ? 1 : 0), (chk900PM.Checked ? 1 : 0), (chk930PM.Checked ? 1 : 0), (chk1000PM.Checked ? 1 : 0), (chk1030PM.Checked ? 1 : 0), (chk1100PM.Checked ? 1 : 0), (chk1130PM.Checked ? 1 : 0), oTSM.GetSchedules(intDomain, 1).Tables[0].Rows.Count + 1, (chkScheduleEnabled.Checked ? 1 : 0));
            Redirect("server=" + intServer.ToString() + "&domain=" + intDomain.ToString() + "&saved=true");
        }
        protected void btnScheduleUpdate_Click(Object Sender, EventArgs e)
        {
            int intResiliency = 0;
            if (ddlScheduleResiliency.SelectedItem != null)
                Int32.TryParse(ddlScheduleResiliency.SelectedItem.Value, out intResiliency);
            oTSM.UpdateSchedule(intSchedule, intDomain, txtScheduleName.Text, (chkScheduleEngineering.Checked ? 1 : 0), (chkScheduleTest.Checked ? 1 : 0), (chkScheduleQA.Checked ? 1 : 0), (chkScheduleProduction.Checked ? 1 : 0), (chkScheduleWindows.Checked ? 1 : 0), (chkScheduleUnix.Checked ? 1 : 0), (chkScheduleDaily.Checked ? 1 : 0), (chkScheduleWeekly.Checked ? 1 : 0), intResiliency, (chkScheduleWeeklySunday.Checked ? 1 : 0), (chkScheduleWeeklyMonday.Checked ? 1 : 0), (chkScheduleWeeklyTuesday.Checked ? 1 : 0), (chkScheduleWeeklyWednesday.Checked ? 1 : 0), (chkScheduleWeeklyThursday.Checked ? 1 : 0), (chkScheduleWeeklyFriday.Checked ? 1 : 0), (chkScheduleWeeklySaturday.Checked ? 1 : 0), (chkScheduleMonthly.Checked ? 1 : 0), (chk1200AM.Checked ? 1 : 0), (chk1230AM.Checked ? 1 : 0), (chk100AM.Checked ? 1 : 0), (chk130AM.Checked ? 1 : 0), (chk200AM.Checked ? 1 : 0), (chk230AM.Checked ? 1 : 0), (chk300AM.Checked ? 1 : 0), (chk330AM.Checked ? 1 : 0), (chk400AM.Checked ? 1 : 0), (chk430AM.Checked ? 1 : 0), (chk500AM.Checked ? 1 : 0), (chk530AM.Checked ? 1 : 0), (chk600AM.Checked ? 1 : 0), (chk630AM.Checked ? 1 : 0), (chk700AM.Checked ? 1 : 0), (chk730AM.Checked ? 1 : 0), (chk800AM.Checked ? 1 : 0), (chk830AM.Checked ? 1 : 0), (chk900AM.Checked ? 1 : 0), (chk930AM.Checked ? 1 : 0), (chk1000AM.Checked ? 1 : 0), (chk1030AM.Checked ? 1 : 0), (chk1100AM.Checked ? 1 : 0), (chk1130AM.Checked ? 1 : 0), (chk1200PM.Checked ? 1 : 0), (chk1230PM.Checked ? 1 : 0), (chk100PM.Checked ? 1 : 0), (chk130PM.Checked ? 1 : 0), (chk200PM.Checked ? 1 : 0), (chk230PM.Checked ? 1 : 0), (chk300PM.Checked ? 1 : 0), (chk330PM.Checked ? 1 : 0), (chk400PM.Checked ? 1 : 0), (chk430PM.Checked ? 1 : 0), (chk500PM.Checked ? 1 : 0), (chk530PM.Checked ? 1 : 0), (chk600PM.Checked ? 1 : 0), (chk630PM.Checked ? 1 : 0), (chk700PM.Checked ? 1 : 0), (chk730PM.Checked ? 1 : 0), (chk800PM.Checked ? 1 : 0), (chk830PM.Checked ? 1 : 0), (chk900PM.Checked ? 1 : 0), (chk930PM.Checked ? 1 : 0), (chk1000PM.Checked ? 1 : 0), (chk1030PM.Checked ? 1 : 0), (chk1100PM.Checked ? 1 : 0), (chk1130PM.Checked ? 1 : 0), (chkScheduleEnabled.Checked ? 1 : 0));
            Redirect("server=" + intServer.ToString() + "&domain=" + intDomain.ToString() + "&schedule=" + intSchedule.ToString() + "&saved=true");
        }
        protected void btnScheduleDelete_Click(Object Sender, EventArgs e)
        {
            oTSM.DeleteSchedule(intSchedule);
            Redirect("server=" + intServer.ToString() + "&domain=" + intDomain.ToString() + "&deleted=true");
        }
        protected void btnScheduleCancel_Click(Object Sender, EventArgs e)
        {
            Redirect("server=" + intServer.ToString() + "&domain=" + intDomain.ToString());
        }

        protected void Redirect(string _additional_querystring)
        {
            Response.Redirect(FormURL(_additional_querystring));
        }
        protected string FormURL(string _additional_querystring)
        {
            string strRedirect = "";
            strRedirect += BuildURL("id", strRedirect);
            strRedirect += BuildURL("tid", strRedirect);
            if (_additional_querystring != "")
            {
                if (strRedirect == "")
                    _additional_querystring = "?" + _additional_querystring;
                else
                    _additional_querystring = "&" + _additional_querystring;
            }
            return oPage.GetFullLink(intPage) + strRedirect + _additional_querystring;
        }
        protected string BuildURL(string _value, string _url)
        {
            string strReturn = "";
            if (Request.QueryString[_value] != null)
            {
                if (_url == "")
                    strReturn = "?" + _value + "=" + Request.QueryString[_value];
                else
                    strReturn = "&" + _value + "=" + Request.QueryString[_value];
            }
            return strReturn;
        }
    }
}