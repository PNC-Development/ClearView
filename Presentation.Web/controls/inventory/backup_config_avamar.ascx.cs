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
    public partial class backup_config_avamar : System.Web.UI.UserControl
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected Pages oPage;
        protected Users oUser;
        protected Servers oServer;
        protected Avamar oAvamar;
        protected Locations oLocation;
        protected Classes oClass;
        protected Resiliency oResiliency;
        protected OperatingSystems oOperatingSystem;
        protected ServerName oServerName;
        protected Functions oFunctions;
        protected int intProfile;
        protected int intApplication = 0;
        protected int intPage = 0;
        protected int intGrid = 0;
        protected int intDomain = 0;
        protected int intParent = 0;
        protected int intLocation = 0;
        protected int intEnv = 0;
        protected int intGroup = 0;
        protected string strLocation = "";
        protected Tab oTab = null;
        protected string strMenuTab1 = "";
        private string strSpacer = "&nbsp;&nbsp;&gt;&gt;&nbsp;&nbsp;";
        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oPage = new Pages(intProfile, dsn);
            oUser = new Users(intProfile, dsn);
            oServer = new Servers(intProfile, dsn);
            oAvamar = new Avamar(intProfile, dsn);
            oLocation = new Locations(intProfile, dsn);
            oClass = new Classes(intProfile, dsn);
            oResiliency = new Resiliency(intProfile, dsn);
            oOperatingSystem = new OperatingSystems(intProfile, dsn);
            oServerName = new ServerName(intProfile, dsn);
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
            int intMenuTab = 0;
            if (Request.QueryString["menu_tab"] != null && Request.QueryString["menu_tab"] != "")
                intMenuTab = Int32.Parse(Request.QueryString["menu_tab"]);
            int intAddressID = 0;
            if (Request.QueryString["grid"] != null && Request.QueryString["grid"] != "")
            {
                intGrid = Int32.Parse(Request.QueryString["grid"]);
                lblCrumbs.Text += "<a href=\"" + oPage.GetFullLink(intPage) + FormURL("") + "\">Back to Home</a>";
                if (Request.QueryString["domain"] != null && Request.QueryString["domain"] != "")
                {
                    intDomain = Int32.Parse(Request.QueryString["domain"]);
                    lblCrumbs.Text += strSpacer + "<a href=\"" + oPage.GetFullLink(intPage) + FormURL("grid=" + intGrid.ToString()) + "\">" + oAvamar.GetGrid(intGrid, "name") + "</a>";
                    panDomain.Visible = true;
                    if (!IsPostBack)
                    {
                        chkDomainResiliency.DataValueField = "id";
                        chkDomainResiliency.DataTextField = "name";
                        chkDomainResiliency.DataSource = oResiliency.Gets(1);
                        chkDomainResiliency.DataBind();

                        chkDomainApplication.DataValueField = "id";
                        chkDomainApplication.DataTextField = "name";
                        chkDomainApplication.DataSource = oServerName.GetComponents(1);
                        chkDomainApplication.DataBind();

                        ddlDomainParent.DataValueField = "id";
                        ddlDomainParent.DataTextField = "FQDN";
                        ddlDomainParent.DataSource = oAvamar.GetDomainsGrid(intGrid, 0, 1);
                        ddlDomainParent.DataBind();
                        

                        DataSet ds = oAvamar.GetDomain(intDomain);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            txtDomainName.Text = ds.Tables[0].Rows[0]["name"].ToString();
                            if (ds.Tables[0].Rows[0]["root"].ToString() == "1")
                            {
                                txtDomainName.Enabled = false;
                                txtDomainName.Text = "/ (ROOT)";
                                ddlDomainParent.Enabled = false;
                                btnDomainUpdate.Attributes.Add("onclick", "return ProcessButton(this) && LoadWait();");
                            }
                            else
                            {
                                foreach (ListItem domain in ddlDomainParent.Items)
                                {
                                    if (domain.Value == ds.Tables[0].Rows[0]["id"].ToString())
                                    {
                                        ddlDomainParent.Items.Remove(domain);
                                        break;
                                    }
                                }
                                btnDomainUpdate.Attributes.Add("onclick", "return ValidateText('" + txtDomainName.ClientID + "','Please enter a name for this domain')" +
                                " && ProcessButton(this) && LoadWait()" +
                                    ";");
                            }
                            lblCrumbs.Text += strSpacer + txtDomainName.Text;
                            ddlDomainParent.SelectedValue = ds.Tables[0].Rows[0]["domainid"].ToString();
                            chkDomainCatchAll.Checked = (ds.Tables[0].Rows[0]["catchall"].ToString() == "1");
                            chkDomainEnabled.Checked = (ds.Tables[0].Rows[0]["enabled"].ToString() == "1");
                            btnDomainUpdate.Visible = true;
                            btnDomainDelete.Visible = true;
                            btnDomainDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this domain')" +
                                " && ProcessButton(this) && LoadWait()" +
                                ";");

                            // Resiliencies
                            DataSet dsResiliencies = oAvamar.GetDomainResiliencys(intDomain);
                            foreach (DataRow drResiliency in dsResiliencies.Tables[0].Rows)
                            {
                                foreach (ListItem item in chkDomainResiliency.Items)
                                {
                                    if (drResiliency["resiliencyid"].ToString() == item.Value)
                                    {
                                        item.Selected = true;
                                        break;
                                    }
                                }
                            }
                            // Applications
                            DataSet dsApplications = oAvamar.GetDomainApplications(intDomain);
                            foreach (DataRow drApplication in dsApplications.Tables[0].Rows)
                            {
                                foreach (ListItem item in chkDomainApplication.Items)
                                {
                                    if (drApplication["applicationid"].ToString() == item.Value)
                                    {
                                        item.Selected = true;
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            btnDomainAdd.Visible = true;
                            btnDomainAdd.Attributes.Add("onclick", "return ValidateText('" + txtDomainName.ClientID + "','Please enter a name for this domain')" +
                            " && ProcessButton(this) && LoadWait()" +
                                ";");
                        }
                        btnDomainCancel.Attributes.Add("onclick", "return ProcessButton(this) && LoadWait();");
                    }
                }
                else if (String.IsNullOrEmpty(Request.QueryString["group"]) == false)
                {
                    intGroup = Int32.Parse(Request.QueryString["group"]);
                    lblCrumbs.Text += strSpacer + "<a href=\"" + oPage.GetFullLink(intPage) + FormURL("grid=" + intGrid.ToString()) + "\">" + oAvamar.GetGrid(intGrid, "name") + "</a>";
                    panGroup.Visible = true;
                    if (!IsPostBack)
                    {
                        ddlGroupDomain.DataValueField = "id";
                        ddlGroupDomain.DataTextField = "FQDN";
                        ddlGroupDomain.DataSource = oAvamar.GetDomainsGrid(intGrid, 1, 1);
                        ddlGroupDomain.DataBind();

                        chkGroupOS.DataValueField = "id";
                        chkGroupOS.DataTextField = "name";
                        chkGroupOS.DataSource = oOperatingSystem.GetStandard(0);
                        chkGroupOS.DataBind();

                        DataSet ds = oAvamar.GetGroup(intGroup);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            txtGroupName.Text = ds.Tables[0].Rows[0]["name"].ToString();
                            lblCrumbs.Text += strSpacer + txtGroupName.Text;
                            chkGroupDaily.Checked = (ds.Tables[0].Rows[0]["daily"].ToString() == "1");
                            chkGroupWeekly.Checked = (ds.Tables[0].Rows[0]["weekly"].ToString() == "1");
                            if (chkGroupWeekly.Checked == true)
                                divGroupWeekly.Style["display"] = "inline";
                            chkGroupWeeklySunday.Checked = (ds.Tables[0].Rows[0]["sunday"].ToString() == "1");
                            chkGroupWeeklyMonday.Checked = (ds.Tables[0].Rows[0]["monday"].ToString() == "1");
                            chkGroupWeeklyTuesday.Checked = (ds.Tables[0].Rows[0]["tuesday"].ToString() == "1");
                            chkGroupWeeklyWednesday.Checked = (ds.Tables[0].Rows[0]["wednesday"].ToString() == "1");
                            chkGroupWeeklyThursday.Checked = (ds.Tables[0].Rows[0]["thursday"].ToString() == "1");
                            chkGroupWeeklyFriday.Checked = (ds.Tables[0].Rows[0]["friday"].ToString() == "1");
                            chkGroupWeeklySaturday.Checked = (ds.Tables[0].Rows[0]["saturday"].ToString() == "1");
                            chkGroupMonthly.Checked = (ds.Tables[0].Rows[0]["monthly"].ToString() == "1");
                            if (chkGroupMonthly.Checked == true)
                                divGroupMonthly.Style["display"] = "inline";
                            ddlGroupMonthly.SelectedValue = ds.Tables[0].Rows[0]["day"].ToString();
                            chk1200AM.Checked = (ds.Tables[0].Rows[0]["AM1200"].ToString() == "1");
                            chk100AM.Checked = (ds.Tables[0].Rows[0]["AM100"].ToString() == "1");
                            chk200AM.Checked = (ds.Tables[0].Rows[0]["AM200"].ToString() == "1");
                            chk300AM.Checked = (ds.Tables[0].Rows[0]["AM300"].ToString() == "1");
                            chk400AM.Checked = (ds.Tables[0].Rows[0]["AM400"].ToString() == "1");
                            chk500AM.Checked = (ds.Tables[0].Rows[0]["AM500"].ToString() == "1");
                            chk600AM.Checked = (ds.Tables[0].Rows[0]["AM600"].ToString() == "1");
                            chk700AM.Checked = (ds.Tables[0].Rows[0]["AM700"].ToString() == "1");
                            chk800AM.Checked = (ds.Tables[0].Rows[0]["AM800"].ToString() == "1");
                            chk900AM.Checked = (ds.Tables[0].Rows[0]["AM900"].ToString() == "1");
                            chk1000AM.Checked = (ds.Tables[0].Rows[0]["AM1000"].ToString() == "1");
                            chk1100AM.Checked = (ds.Tables[0].Rows[0]["AM1100"].ToString() == "1");
                            chk1200PM.Checked = (ds.Tables[0].Rows[0]["PM1200"].ToString() == "1");
                            chk100PM.Checked = (ds.Tables[0].Rows[0]["PM100"].ToString() == "1");
                            chk200PM.Checked = (ds.Tables[0].Rows[0]["PM200"].ToString() == "1");
                            chk300PM.Checked = (ds.Tables[0].Rows[0]["PM300"].ToString() == "1");
                            chk400PM.Checked = (ds.Tables[0].Rows[0]["PM400"].ToString() == "1");
                            chk500PM.Checked = (ds.Tables[0].Rows[0]["PM500"].ToString() == "1");
                            chk600PM.Checked = (ds.Tables[0].Rows[0]["PM600"].ToString() == "1");
                            chk700PM.Checked = (ds.Tables[0].Rows[0]["PM700"].ToString() == "1");
                            chk800PM.Checked = (ds.Tables[0].Rows[0]["PM800"].ToString() == "1");
                            chk900PM.Checked = (ds.Tables[0].Rows[0]["PM900"].ToString() == "1");
                            chk1000PM.Checked = (ds.Tables[0].Rows[0]["PM1000"].ToString() == "1");
                            chk1100PM.Checked = (ds.Tables[0].Rows[0]["PM1100"].ToString() == "1");
                            txtGroupThreshold.Text = ds.Tables[0].Rows[0]["threshold"].ToString();
                            txtGroupMaximum.Text = ds.Tables[0].Rows[0]["maximum"].ToString();
                            chkGroupClustering.Checked = (ds.Tables[0].Rows[0]["clustering"].ToString() == "1");
                            chkGroupEnabled.Checked = (ds.Tables[0].Rows[0]["enabled"].ToString() == "1");
                            btnGroupUpdate.Visible = true;
                            btnGroupUpdate.Attributes.Add("onclick", "return ValidateText('" + txtGroupName.ClientID + "','Please enter a name for this group')" +
                            " && ProcessButton(this) && LoadWait()" +
                                ";");
                            btnGroupDelete.Visible = true;
                            btnGroupDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this group')" +
                                " && ProcessButton(this) && LoadWait()" +
                                ";");

                            // Operating Systems
                            DataSet dsOperatingSystems = oAvamar.GetGroupOperatingSystems(intGroup);
                            foreach (DataRow drOperatingSystem in dsOperatingSystems.Tables[0].Rows)
                            {
                                foreach (ListItem item in chkGroupOS.Items)
                                {
                                    if (drOperatingSystem["osid"].ToString() == item.Value)
                                    {
                                        item.Selected = true;
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            btnGroupAdd.Visible = true;
                            btnGroupAdd.Attributes.Add("onclick", "return ValidateText('" + txtGroupName.ClientID + "','Please enter a name for this group')" +
                            " && ProcessButton(this) && LoadWait()" +
                                ";");
                        }
                        btnGroupCancel.Attributes.Add("onclick", "return ProcessButton(this) && LoadWait();");
                        chkGroupDaily.Attributes.Add("onclick", "ShowHideDiv('" + divGroupWeekly.ClientID + "','none');ShowHideDiv('" + divGroupMonthly.ClientID + "','none');");
                        chkGroupWeekly.Attributes.Add("onclick", "ShowHideDiv('" + divGroupWeekly.ClientID + "','inline');ShowHideDiv('" + divGroupMonthly.ClientID + "','none');");
                        chkGroupMonthly.Attributes.Add("onclick", "ShowHideDiv('" + divGroupWeekly.ClientID + "','none');ShowHideDiv('" + divGroupMonthly.ClientID + "','inline');");
                    }
                }
                else if (String.IsNullOrEmpty(Request.QueryString["location"]) == false)
                {
                    intLocation = Int32.Parse(Request.QueryString["location"]);
                    lblCrumbs.Text += strSpacer + "<a href=\"" + oPage.GetFullLink(intPage) + FormURL("grid=" + intGrid.ToString()) + "\">" + oAvamar.GetGrid(intGrid, "name") + "</a>";
                    panLocation.Visible = true;
                    if (!IsPostBack)
                    {
                        DataSet ds = oAvamar.GetLocation(intLocation);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            intAddressID = Int32.Parse(ds.Tables[0].Rows[0]["addressid"].ToString());
                            lblCrumbs.Text += strSpacer + oLocation.GetFull(intAddressID);
                            chkLocationEnabled.Checked = (ds.Tables[0].Rows[0]["enabled"].ToString() == "1");
                            btnLocationUpdate.Visible = true;
                            btnLocationUpdate.Attributes.Add("onclick", "return ValidateHidden0('" + hdnLocation.ClientID + "','ddlCommonLocation','Please select a location')" +
                            " && ProcessButton(this) && LoadWait()" +
                                ";");
                            btnLocationDelete.Visible = true;
                            btnLocationDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this location')" +
                                " && ProcessButton(this) && LoadWait()" +
                                ";");
                        }
                        else
                        {
                            btnLocationAdd.Visible = true;
                            btnLocationAdd.Attributes.Add("onclick", "return ValidateHidden0('" + hdnLocation.ClientID + "','ddlCommonLocation','Please select a location')" +
                            " && ProcessButton(this) && LoadWait()" +
                                ";");
                        }
                        btnLocationCancel.Attributes.Add("onclick", "return ProcessButton(this) && LoadWait();");
                        strLocation = oLocation.LoadDDL("ddlStateLocation", "ddlCityLocation", "ddlAddressLocation", hdnLocation.ClientID, intAddressID, true, "ddlCommonLocation");
                    }
                }
                else if (String.IsNullOrEmpty(Request.QueryString["environment"]) == false)
                {
                    intEnv = Int32.Parse(Request.QueryString["environment"]);
                    lblCrumbs.Text += strSpacer + "<a href=\"" + oPage.GetFullLink(intPage) + FormURL("grid=" + intGrid.ToString()) + "\">" + oAvamar.GetGrid(intGrid, "name") + "</a>";
                    panEnvironment.Visible = true;
                    if (!IsPostBack)
                    {
                        ddlClass.DataTextField = "name";
                        ddlClass.DataValueField = "id";
                        ddlClass.DataSource = oClass.GetForecasts(1);
                        ddlClass.DataBind();
                        ddlClass.Items.Insert(0, new ListItem("-- SELECT --", "0"));

                        DataSet ds = oAvamar.GetEnvironment(intEnv);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            int intClassID = Int32.Parse(ds.Tables[0].Rows[0]["classid"].ToString());
                            try
                            {
                                ddlClass.SelectedValue = intClassID.ToString();
                            }
                            catch { }
                            if (ddlClass.SelectedIndex == 0)
                            {
                                // Class is not available, add it.
                                ddlClass.Items.Add(new ListItem(oClass.Get(intClassID, "name") + " *", intClassID.ToString()));
                                ddlClass.SelectedValue = intClassID.ToString();
                            }
                            int intEnvironmentID = Int32.Parse(ds.Tables[0].Rows[0]["environmentid"].ToString());
                            hdnEnvironment.Value = intEnvironmentID.ToString();
                            ddlEnvironment.DataTextField = "name";
                            ddlEnvironment.DataValueField = "id";
                            ddlEnvironment.DataSource = oClass.GetEnvironment(intClassID, 1);
                            ddlEnvironment.DataBind();
                            ddlEnvironment.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                            ddlEnvironment.Enabled = true;
                            ddlEnvironment.SelectedValue = intEnvironmentID.ToString();
                            chkEnvironmentEnabled.Checked = (ds.Tables[0].Rows[0]["enabled"].ToString() == "1");
                            btnEnvironmentUpdate.Visible = true;
                            btnEnvironmentUpdate.Attributes.Add("onclick", "return ValidateDropDown('" + ddlClass.ClientID + "','Please select a class')" +
                            " && ValidateHidden0('" + hdnEnvironment.ClientID + "','" + ddlEnvironment.ClientID + "','Please select an environment')" +
                            " && ProcessButton(this) && LoadWait()" +
                                ";");
                            btnEnvironmentDelete.Visible = true;
                            btnEnvironmentDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this location')" +
                                " && ProcessButton(this) && LoadWait()" +
                                ";");
                        }
                        else
                        {
                            btnEnvironmentAdd.Visible = true;
                            btnEnvironmentAdd.Attributes.Add("onclick", "return ValidateDropDown('" + ddlClass.ClientID + "','Please select a class')" +
                            " && ValidateHidden0('" + hdnEnvironment.ClientID + "','" + ddlEnvironment.ClientID + "','Please select an environment')" +
                            " && ProcessButton(this) && LoadWait()" +
                                ";");
                        }
                        btnEnvironmentCancel.Attributes.Add("onclick", "return ProcessButton(this) && LoadWait();");
                        ddlClass.Attributes.Add("onchange", "PopulateEnvironments('" + ddlClass.ClientID + "','" + ddlEnvironment.ClientID + "',1);");
                        ddlEnvironment.Attributes.Add("onchange", "UpdateDropDownHidden('" + ddlEnvironment.ClientID + "','" + hdnEnvironment.ClientID + "');");
                    }
                }
                else
                {
                    panGrid.Visible = true;
                    if (!IsPostBack)
                    {
                        //Menus
                        oTab = new Tab(hdnType.ClientID, intMenuTab, "divMenu1", true, false);
                        oTab.AddTab("Domains", "");
                        DataSet dsDomains = oAvamar.GetDomainsGrid(intGrid, 0, 0);
                        DataView dvDomains = dsDomains.Tables[0].DefaultView;
                        if (Request.QueryString["sort"] != null)
                            dvDomains.Sort = Request.QueryString["sort"];
                        else
                            dvDomains.Sort = "name";
                        rptDomains.DataSource = dvDomains;
                        rptDomains.DataBind();
                        lblDomains.Visible = (rptDomains.Items.Count == 0);

                        oTab.AddTab("Groups", "");
                        DataSet dsGroups = oAvamar.GetGroupsGrid(intGrid, 0);
                        DataView dvGroups = dsGroups.Tables[0].DefaultView;
                        if (Request.QueryString["sort"] != null)
                            dvGroups.Sort = Request.QueryString["sort"];
                        else
                            dvGroups.Sort = "name";
                        rptGroups.DataSource = dvGroups;
                        rptGroups.DataBind();
                        lblGroups.Visible = (rptGroups.Items.Count == 0);

                        oTab.AddTab("Locations", "");
                        DataSet dsLocations = oAvamar.GetLocations(intGrid, 0);
                        DataView dvLocations = dsLocations.Tables[0].DefaultView;
                        if (Request.QueryString["sort"] != null)
                            dvLocations.Sort = Request.QueryString["sort"];
                        else
                            dvLocations.Sort = "addressid";
                        rptLocations.DataSource = dvLocations;
                        rptLocations.DataBind();
                        lblLocations.Visible = (rptLocations.Items.Count == 0);

                        oTab.AddTab("Environments", "");
                        DataSet dsEnvironments = oAvamar.GetEnvironments(intGrid, 0);
                        DataView dvEnvironments = dsEnvironments.Tables[0].DefaultView;
                        if (Request.QueryString["sort"] != null)
                            dvEnvironments.Sort = Request.QueryString["sort"];
                        else
                            dvEnvironments.Sort = "classid";
                        rptEnvironments.DataSource = dvEnvironments;
                        rptEnvironments.DataBind();
                        lblEnvironments.Visible = (rptEnvironments.Items.Count == 0);

                        strMenuTab1 = oTab.GetTabs();

                        DataSet ds = oAvamar.GetGrid(intGrid);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            txtGridName.Text = ds.Tables[0].Rows[0]["name"].ToString();
                            lblCrumbs.Text += strSpacer + txtGridName.Text;
                            txtGridThreshold.Text = ds.Tables[0].Rows[0]["threshold"].ToString();
                            txtGridMaximum.Text = ds.Tables[0].Rows[0]["maximum"].ToString();
                            chkGridEnabled.Checked = (ds.Tables[0].Rows[0]["enabled"].ToString() == "1");
                            btnGridUpdate.Visible = true;
                            btnGridUpdate.Attributes.Add("onclick", "return ValidateText('" + txtGridName.ClientID + "','Please enter a name for this grid')" +
                                " && ProcessButton(this) && LoadWait()" +
                                ";");
                            btnGridDelete.Visible = true;
                            btnGridDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this server')" +
                                " && ProcessButton(this) && LoadWait()" +
                                ";");
                        }
                        else
                        {
                            btnGridAdd.Visible = true;
                            btnGridAdd.Attributes.Add("onclick", "return ValidateText('" + txtGridName.ClientID + "','Please enter a name for this grid')" +
                                " && ProcessButton(this) && LoadWait()" +
                                ";");
                            btnAddDomain.Enabled = false;
                            btnAddLocation.Enabled = false;
                            btnAddEnvironment.Enabled = false;
                            btnAddGroup.Enabled = false;
                        }
                        btnGridCancel.Attributes.Add("onclick", "return ProcessButton(this) && LoadWait();");
                        strLocation = oLocation.LoadDDL("ddlStateServer", "ddlCityServer", "ddlAddressServer", hdnLocation.ClientID, intAddressID, true, "ddlCommonServer");
                    }
                }
            }
            else
            {
                panGrids.Visible = true;
                DataSet dsGrids = oAvamar.GetGrids(0);
                DataView dvGrids = dsGrids.Tables[0].DefaultView;
                if (Request.QueryString["sort"] != null)
                    dvGrids.Sort = Request.QueryString["sort"];
                else
                    dvGrids.Sort = "name";
                rptGrids.DataSource = dvGrids;
                rptGrids.DataBind();
                lblGrids.Visible = (rptGrids.Items.Count == 0);
            }
            hdnLocation.Value = intAddressID.ToString();
            btnAddDomain.Attributes.Add("onclick", "return ProcessButton(this) && LoadWait();");
            btnAddLocation.Attributes.Add("onclick", "return ProcessButton(this) && LoadWait();");
            btnAddEnvironment.Attributes.Add("onclick", "return ProcessButton(this) && LoadWait();");
            btnAddGroup.Attributes.Add("onclick", "return ProcessButton(this) && LoadWait();");
            btnAddGrid.Attributes.Add("onclick", "return ProcessButton(this) && LoadWait();");
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
            strRedirect += BuildURL("grid", strRedirect);
            strRedirect += BuildURL("domain", strRedirect);
            strRedirect += BuildURL("group", strRedirect);
            if (strRedirect == "")
                strRedirect = "?";
            else
                strRedirect += "&";
            Response.Redirect(oPage.GetFullLink(intPage) + strRedirect + "sort=" + strOrder);
        }
        protected void btnAddGrid_Click(Object Sender, EventArgs e)
        {
            Redirect("grid=0");
        }
        protected void btnGridAdd_Click(Object Sender, EventArgs e)
        {
            int intThreshold = 0;
            Int32.TryParse(txtGridThreshold.Text, out intThreshold);
            int intMaximum = 0;
            Int32.TryParse(txtGridMaximum.Text, out intMaximum);
            intGrid = oAvamar.AddGrid(txtGridName.Text, 0, intThreshold, intMaximum, (chkGridEnabled.Checked ? 1 : 0));
            // By default, add the root domain.
            intDomain = oAvamar.AddDomain(intGrid, 0, "", 1, 1, 0, 1);
            Redirect("saved=true");
        }
        protected void btnGridUpdate_Click(Object Sender, EventArgs e)
        {
            int intThreshold = 0;
            Int32.TryParse(txtGridThreshold.Text, out intThreshold);
            int intMaximum = 0;
            Int32.TryParse(txtGridMaximum.Text, out intMaximum);
            oAvamar.UpdateGrid(intGrid, txtGridName.Text, intThreshold, intMaximum, (chkGridEnabled.Checked ? 1 : 0));
            Redirect("grid=" + intGrid.ToString() + "&saved=true");
        }
        protected void btnGridDelete_Click(Object Sender, EventArgs e)
        {
            oAvamar.DeleteGrid(intGrid);
            Redirect("deleted=true");
        }
        protected void btnGridCancel_Click(Object Sender, EventArgs e)
        {
            Redirect("");
        }

        // SERVER -> LOCATION
        protected void btnAddLocation_Click(object sender, EventArgs e)
        {
            Redirect("grid=" + intGrid.ToString() + "&location=0");
        }
        protected void btnLocationAdd_Click(object sender, EventArgs e)
        {
            int intAddressID = 0;
            if (String.IsNullOrEmpty(Request.Form[hdnLocation.UniqueID]) == false)
                Int32.TryParse(Request.Form[hdnLocation.UniqueID], out intAddressID);
            oAvamar.AddLocation(intGrid, intAddressID, (chkLocationEnabled.Checked ? 1 : 0));
            Redirect("grid=" + intGrid.ToString() + "&saved=true");
        }
        protected void btnLocationUpdate_Click(object sender, EventArgs e)
        {
            int intAddressID = 0;
            if (String.IsNullOrEmpty(Request.Form[hdnLocation.UniqueID]) == false)
                Int32.TryParse(Request.Form[hdnLocation.UniqueID], out intAddressID);
            oAvamar.UpdateLocation(intLocation, intGrid, intAddressID, (chkLocationEnabled.Checked ? 1 : 0));
            Redirect("grid=" + intGrid.ToString() + "&location=" + intLocation.ToString() + "&saved=true");
        }
        protected void btnLocationDelete_Click(object sender, EventArgs e)
        {
            oAvamar.DeleteLocation(intLocation);
            Redirect("grid=" + intGrid.ToString() + "&deleted=true");
        }
        protected void btnLocationCancel_Click(object sender, EventArgs e)
        {
            Redirect("grid=" + intGrid.ToString());
        }

        // SERVER -> ENVIRONMENT
        protected void btnAddEnvironment_Click(object sender, EventArgs e)
        {
            Redirect("grid=" + intGrid.ToString() + "&environment=0");
        }
        protected void btnEnvironmentAdd_Click(object sender, EventArgs e)
        {
            int intEnvironmentID = 0;
            if (String.IsNullOrEmpty(Request.Form[hdnEnvironment.UniqueID]) == false)
                Int32.TryParse(Request.Form[hdnEnvironment.UniqueID], out intEnvironmentID);
            oAvamar.AddEnvironment(intGrid, Int32.Parse(ddlClass.SelectedItem.Value), intEnvironmentID, (chkEnvironmentEnabled.Checked ? 1 : 0));
            Redirect("grid=" + intGrid.ToString() + "&saved=true");
        }
        protected void btnEnvironmentUpdate_Click(object sender, EventArgs e)
        {
            int intEnvironmentID = 0;
            if (String.IsNullOrEmpty(Request.Form[hdnEnvironment.UniqueID]) == false)
                Int32.TryParse(Request.Form[hdnEnvironment.UniqueID], out intEnvironmentID);
            oAvamar.UpdateEnvironment(intEnv, intGrid, Int32.Parse(ddlClass.SelectedItem.Value), intEnvironmentID, (chkEnvironmentEnabled.Checked ? 1 : 0));
            Redirect("grid=" + intGrid.ToString() + "&saved=true");
        }
        protected void btnEnvironmentDelete_Click(object sender, EventArgs e)
        {
            oAvamar.DeleteEnvironment(intLocation);
            Redirect("grid=" + intGrid.ToString() + "&deleted=true");
        }
        protected void btnEnvironmentCancel_Click(object sender, EventArgs e)
        {
            Redirect("grid=" + intGrid.ToString());
        }

        protected void btnAddDomain_Click(Object Sender, EventArgs e)
        {
            Redirect("grid=" + intGrid.ToString() + "&domain=0");
        }
        protected void btnDomainAdd_Click(Object Sender, EventArgs e)
        {
            intDomain = oAvamar.AddDomain(intGrid, Int32.Parse(ddlDomainParent.SelectedItem.Value), txtDomainName.Text, 0, (chkDomainCatchAll.Checked ? 1 : 0), 0, (chkDomainEnabled.Checked ? 1 : 0));
            SaveDomain();
            Redirect("grid=" + intGrid.ToString() + "&saved=true");
        }
        protected void btnDomainUpdate_Click(Object Sender, EventArgs e)
        {
            int intParent = Int32.Parse(ddlDomainParent.SelectedItem.Value);
            if (ddlDomainParent.Enabled == false)
                intParent = 0;
            string strName = txtDomainName.Text;
            if (txtDomainName.Enabled == false)
                strName = "";
            oAvamar.UpdateDomain(intDomain, intGrid, intParent, strName, (chkDomainCatchAll.Checked ? 1 : 0), (chkDomainEnabled.Checked ? 1 : 0));
            SaveDomain();
            Redirect("grid=" + intGrid.ToString() + "&domain=" + intDomain.ToString() + "&saved=true");
        }
        private void SaveDomain()
        {
            // Resiliencies
            oAvamar.DeleteDomainResiliency(intDomain);
            foreach (ListItem item in chkDomainResiliency.Items)
            {
                if (item.Selected)
                    oAvamar.AddDomainResiliency(intDomain, Int32.Parse(item.Value));
            }
            // Applications
            oAvamar.DeleteDomainApplication(intDomain);
            foreach (ListItem item in chkDomainApplication.Items)
            {
                if (item.Selected)
                    oAvamar.AddDomainApplication(intDomain, Int32.Parse(item.Value));
            }
        }
        protected void btnDomainDelete_Click(Object Sender, EventArgs e)
        {
            oAvamar.DeleteDomain(intDomain);
            Redirect("grid=" + intGrid.ToString() + "&deleted=true");
        }
        protected void btnDomainCancel_Click(Object Sender, EventArgs e)
        {
            Redirect("grid=" + intGrid.ToString());
        }
        protected void btnAddGroup_Click(Object Sender, EventArgs e)
        {
            Redirect("grid=" + intGrid.ToString() + "&group=0");
        }
        protected void btnGroupAdd_Click(Object Sender, EventArgs e)
        {
            int intThreshold = 0;
            Int32.TryParse(txtGroupThreshold.Text, out intThreshold);
            int intMaximum = 0;
            Int32.TryParse(txtGroupMaximum.Text, out intMaximum);
            intGroup = oAvamar.AddGroup(Int32.Parse(ddlGroupDomain.SelectedItem.Value), txtGroupName.Text, (chkGroupDaily.Checked ? 1 : 0), (chkGroupWeekly.Checked ? 1 : 0), (chkGroupWeeklySunday.Checked ? 1 : 0), (chkGroupWeeklyMonday.Checked ? 1 : 0), (chkGroupWeeklyTuesday.Checked ? 1 : 0), (chkGroupWeeklyWednesday.Checked ? 1 : 0), (chkGroupWeeklyThursday.Checked ? 1 : 0), (chkGroupWeeklyFriday.Checked ? 1 : 0), (chkGroupWeeklySaturday.Checked ? 1 : 0), (chkGroupMonthly.Checked ? 1 : 0), Int32.Parse(ddlGroupMonthly.SelectedItem.Value), (chk1200AM.Checked ? 1 : 0), 0, (chk100AM.Checked ? 1 : 0), 0, (chk200AM.Checked ? 1 : 0), 0, (chk300AM.Checked ? 1 : 0), 0, (chk400AM.Checked ? 1 : 0), 0, (chk500AM.Checked ? 1 : 0), 0, (chk600AM.Checked ? 1 : 0), 0, (chk700AM.Checked ? 1 : 0), 0, (chk800AM.Checked ? 1 : 0), 0, (chk900AM.Checked ? 1 : 0), 0, (chk1000AM.Checked ? 1 : 0), 0, (chk1100AM.Checked ? 1 : 0), 0, (chk1200PM.Checked ? 1 : 0), 0, (chk100PM.Checked ? 1 : 0), 0, (chk200PM.Checked ? 1 : 0), 0, (chk300PM.Checked ? 1 : 0), 0, (chk400PM.Checked ? 1 : 0), 0, (chk500PM.Checked ? 1 : 0), 0, (chk600PM.Checked ? 1 : 0), 0, (chk700PM.Checked ? 1 : 0), 0, (chk800PM.Checked ? 1 : 0), 0, (chk900PM.Checked ? 1 : 0), 0, (chk1000PM.Checked ? 1 : 0), 0, (chk1100PM.Checked ? 1 : 0), 0, 0, intThreshold, intMaximum, (chkGroupClustering.Checked ? 1 : 0), (chkGroupEnabled.Checked ? 1 : 0));
            SaveGroup();
            Redirect("grid=" + intGrid.ToString() + "&saved=true");
        }
        protected void btnGroupUpdate_Click(Object Sender, EventArgs e)
        {
            int intThreshold = 0;
            Int32.TryParse(txtGroupThreshold.Text, out intThreshold);
            int intMaximum = 0;
            Int32.TryParse(txtGroupMaximum.Text, out intMaximum);
            oAvamar.UpdateGroup(intGroup, Int32.Parse(ddlGroupDomain.SelectedItem.Value), txtGroupName.Text, (chkGroupDaily.Checked ? 1 : 0), (chkGroupWeekly.Checked ? 1 : 0), (chkGroupWeeklySunday.Checked ? 1 : 0), (chkGroupWeeklyMonday.Checked ? 1 : 0), (chkGroupWeeklyTuesday.Checked ? 1 : 0), (chkGroupWeeklyWednesday.Checked ? 1 : 0), (chkGroupWeeklyThursday.Checked ? 1 : 0), (chkGroupWeeklyFriday.Checked ? 1 : 0), (chkGroupWeeklySaturday.Checked ? 1 : 0), (chkGroupMonthly.Checked ? 1 : 0), Int32.Parse(ddlGroupMonthly.SelectedItem.Value), (chk1200AM.Checked ? 1 : 0), 0, (chk100AM.Checked ? 1 : 0), 0, (chk200AM.Checked ? 1 : 0), 0, (chk300AM.Checked ? 1 : 0), 0, (chk400AM.Checked ? 1 : 0), 0, (chk500AM.Checked ? 1 : 0), 0, (chk600AM.Checked ? 1 : 0), 0, (chk700AM.Checked ? 1 : 0), 0, (chk800AM.Checked ? 1 : 0), 0, (chk900AM.Checked ? 1 : 0), 0, (chk1000AM.Checked ? 1 : 0), 0, (chk1100AM.Checked ? 1 : 0), 0, (chk1200PM.Checked ? 1 : 0), 0, (chk100PM.Checked ? 1 : 0), 0, (chk200PM.Checked ? 1 : 0), 0, (chk300PM.Checked ? 1 : 0), 0, (chk400PM.Checked ? 1 : 0), 0, (chk500PM.Checked ? 1 : 0), 0, (chk600PM.Checked ? 1 : 0), 0, (chk700PM.Checked ? 1 : 0), 0, (chk800PM.Checked ? 1 : 0), 0, (chk900PM.Checked ? 1 : 0), 0, (chk1000PM.Checked ? 1 : 0), 0, (chk1100PM.Checked ? 1 : 0), 0, intThreshold, intMaximum, (chkGroupClustering.Checked ? 1 : 0), (chkGroupEnabled.Checked ? 1 : 0));
            SaveGroup();
            Redirect("grid=" + intGrid.ToString() + "&group=" + intGroup.ToString() + "&saved=true");
        }
        private void SaveGroup()
        {
            // Operating Systems
            oAvamar.DeleteGroupOperatingSystem(intGroup);
            foreach (ListItem item in chkGroupOS.Items)
            {
                if (item.Selected)
                    oAvamar.AddGroupOperatingSystem(intGroup, Int32.Parse(item.Value));
            }
        }
        protected void btnGroupDelete_Click(Object Sender, EventArgs e)
        {
            oAvamar.DeleteGroup(intGroup);
            Redirect("grid=" + intGrid.ToString() + "&deleted=true");
        }
        protected void btnGroupCancel_Click(Object Sender, EventArgs e)
        {
            Redirect("grid=" + intGrid.ToString());
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
            strRedirect += BuildURL("div", strRedirect);
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