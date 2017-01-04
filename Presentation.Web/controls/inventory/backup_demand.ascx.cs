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
namespace NCC.ClearView.Presentation.Web
{
    public partial class backup_demand : System.Web.UI.UserControl
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intStoragePerBladeOs = Int32.Parse(ConfigurationManager.AppSettings["STORAGE_PER_BLADE_OS"]);
        protected int intStoragePerBladeApp = Int32.Parse(ConfigurationManager.AppSettings["STORAGE_PER_BLADE_APP"]);
        protected Platforms oPlatform;
        protected Types oType;
        protected ModelsProperties oModelsProperties;
        protected ServiceRequests oServiceRequest;
        protected Design oDesign;
        protected Pages oPage;
        protected int intProfile;
        protected int intApplication = 0;
        protected int intPage = 0;
        protected string strDemand = "";
        protected int intMax = 50;
        protected string strFilter = "";
        protected string strBackup = "";
        protected int intQuantity = 0;
        protected double dblTotal = 0.00;
        protected string strViews = "";
        protected string strParameters = "";
        DataSet dsDemand;

        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oPlatform = new Platforms(intProfile, dsn);
            oType = new Types(intProfile, dsn);
            oModelsProperties = new ModelsProperties(intProfile, dsn);
            oServiceRequest = new ServiceRequests(intProfile, dsn);
            oDesign = new Design(intProfile, dsn);
            oPage = new Pages(intProfile, dsn);
            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            int intPlatform = 0;
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                intPlatform = Int32.Parse(Request.QueryString["id"]);
            if (intPlatform > 0)
            {
                intMax = Int32.Parse(oPlatform.Get(intPlatform, "max_inventory1"));
                if (!IsPostBack)
                {
                    dsDemand = oDesign.GetForecast();
                    if (Request.QueryString["projectid"] != null && Request.QueryString["projectid"] != "")
                    {
                    }
                    else
                    {
                        DateTime _today = DateTime.Today;
                        if (Request.QueryString["change"] != null && Request.QueryString["change"] != "")
                            _today = DateTime.Parse(Request.QueryString["change"]);
                        panCalendar.Visible = true;
                        LoadLists();
                        LoadFilters();
                        calThis.VisibleDate = _today;
                        calThis.SelectedDate = _today;
                        ddlYear.SelectedValue = _today.Year.ToString();
                        ddlMonth.SelectedValue = _today.Month.ToString();
                    }
                }
                btnProjects.Attributes.Add("onclick", "return MakeWider(this, '" + lstProjects.ClientID + "');");
                btnProjectsClear.Attributes.Add("onclick", "return ClearList('" + lstProjects.ClientID + "');");
                btnClasses.Attributes.Add("onclick", "return MakeWider(this, '" + lstClasses.ClientID + "');");
                btnClassesClear.Attributes.Add("onclick", "return ClearList('" + lstClasses.ClientID + "');");
                btnConfidences.Attributes.Add("onclick", "return MakeWider(this, '" + lstConfidences.ClientID + "');");
                btnConfidencesClear.Attributes.Add("onclick", "return ClearList('" + lstConfidences.ClientID + "');");
                btnEnvironments.Attributes.Add("onclick", "return MakeWider(this, '" + lstEnvironments.ClientID + "');");
                btnEnvironmentsClear.Attributes.Add("onclick", "return ClearList('" + lstEnvironments.ClientID + "');");
                btnLocations.Attributes.Add("onclick", "return MakeWider(this, '" + lstLocations.ClientID + "');");
                btnLocationsClear.Attributes.Add("onclick", "return ClearList('" + lstLocations.ClientID + "');");
                lstClasses.Attributes.Add("onchange", "PopulateEnvironmentsList('" + lstClasses.ClientID + "','" + lstEnvironments.ClientID + "',0);");
                lstEnvironments.Attributes.Add("onchange", "UpdateListHidden('" + lstEnvironments.ClientID + "','" + hdnEnvironment.ClientID + "');");
                imgStart.Attributes.Add("onclick", "return ShowCalendar('" + txtStart.ClientID + "');");
                imgEnd.Attributes.Add("onclick", "return ShowCalendar('" + txtEnd.ClientID + "');");
            }
        }
        public void LoadLists()
        {
            Projects oProject = new Projects(intProfile, dsn);
            lstProjects.DataValueField = "projectid";
            lstProjects.DataTextField = "name";
            lstProjects.DataSource = oProject.GetActive();
            lstProjects.DataBind();
            lstProjects.Items.Insert(0, new ListItem("-- ALL --", "0"));
            LoadList("pid", "Project(s)", lstProjects, null);
            Locations oLocation = new Locations(intProfile, dsn);
            lstLocations.DataValueField = "id";
            lstLocations.DataTextField = "fullname";
            lstLocations.DataSource = oLocation.GetAddresssOrdered(1);
            lstLocations.DataBind();
            lstLocations.Items.Insert(0, new ListItem("-- ALL --", "0"));
            LoadList("lid", "Location(s)", lstLocations, null);
            Confidence oConfidence = new Confidence(intProfile, dsn);
            lstConfidences.DataValueField = "id";
            lstConfidences.DataTextField = "name";
            lstConfidences.DataSource = oConfidence.Gets(1);
            lstConfidences.DataBind();
            lstConfidences.Items.Insert(0, new ListItem("-- ALL --", "0"));
            LoadList("xid", "Confidence(s)", lstConfidences, null);
            Classes oClass = new Classes(intProfile, dsn);
            DataSet dsClasses = oClass.Gets(1);
            lstClasses.DataValueField = "id";
            lstClasses.DataTextField = "name";
            lstClasses.DataSource = dsClasses;
            lstClasses.DataBind();
            lstClasses.Items.Insert(0, new ListItem("-- ALL --", "0"));
            LoadList("cid", "Class(es)", lstClasses, null);
            if (Request.QueryString["cid"] != null)
            {
                string strValue = Request.QueryString["cid"];
                if (strValue == "0")
                {
                    foreach (DataRow drClass in dsClasses.Tables[0].Rows)
                        strValue += drClass["id"].ToString() + ",";
                }
                string[] strValues;
                char[] strSplit = { ',' };
                strValues = strValue.Split(strSplit);
                Functions oFunction = new Functions(intProfile, dsn, intEnvironment);
                strValue = oFunction.BuildXmlString("data", strValues);
                lstEnvironments.DataValueField = "id";
                lstEnvironments.DataTextField = "name";
                lstEnvironments.DataSource = oClass.GetEnvironments(strValue, 0);
                lstEnvironments.DataBind();
                lstEnvironments.Items.Insert(0, new ListItem("-- ALL --", "0"));
                LoadList("eid", "Environment(s)", lstEnvironments, hdnEnvironment);
            }
            else
            {
                lstEnvironments.Items.Insert(0, new ListItem("-- Please select a Class --", "0"));
                lstEnvironments.Enabled = false;
            }
            // Views
            strViews = "";
            if (strViews == "")
                strViews = "<tr><td><img src=\"/images/cancel.gif\" border=\"0\" align=\"absmiddle\" /> You have no saved views</td></tr>";
            strViews = "<table cellpadding=\"3\" cellspacing=\"1\" border=\"0\">" + strViews + "</table>";
            // Load Dates and Groups
            txtStart.Text = Request.QueryString["start"];
            txtEnd.Text = Request.QueryString["end"];
            if (strParameters != "")
            {
                strParameters = "<table cellpadding=\"3\" cellspacing=\"1\" border=\"0\">" + strParameters + "</table>";
                panParameters.Visible = true;
            }
        }
        private void LoadList(string _query, string _name, ListBox _box, HiddenField _hidden)
        {
            char[] strSplit = { ',' };
            string strQuery = Request.QueryString[_query];
            if (strQuery != null)
            {
                bool boolAll = (strQuery == "0");
                if (boolAll == false)
                    strParameters += "<tr><td><b>" + _name + ":</b></td></tr>";
                string[] strQuerys;
                strQuerys = strQuery.Split(strSplit);
                for (int ii = 0; ii < strQuerys.Length; ii++)
                {
                    if (strQuerys[ii].Trim() != "")
                    {
                        foreach (ListItem oList in _box.Items)
                        {
                            if (oList.Value == strQuerys[ii].Trim())
                            {
                                if (boolAll == false)
                                    strParameters += "<tr><td>&nbsp;&nbsp;&nbsp;&quot;" + oList.Text + "&quot;</td></tr>";
                                oList.Selected = true;
                                if (_hidden != null)
                                    _hidden.Value += oList.Value + ";";
                                break;
                            }
                        }
                    }
                }
            }
        }
        protected void btnGo_Click(Object Sender, EventArgs e)
        {
            Filter();
        }
        private void Filter()
        {
            string strQuery = "";
            foreach (ListItem oList in lstProjects.Items)
            {
                if (oList.Selected == true)
                    strQuery += "&pid=" + oList.Value;
            }
            foreach (ListItem oList in lstLocations.Items)
            {
                if (oList.Selected == true)
                    strQuery += "&lid=" + oList.Value;
            }
            foreach (ListItem oList in lstConfidences.Items)
            {
                if (oList.Selected == true)
                    strQuery += "&xid=" + oList.Value;
            }
            foreach (ListItem oList in lstClasses.Items)
            {
                if (oList.Selected == true)
                    strQuery += "&cid=" + oList.Value;
            }
            string strEnvironment = Request.Form[hdnEnvironment.UniqueID];
            string[] strEnvironments;
            char[] strSplit = { ';' };
            strEnvironments = strEnvironment.Split(strSplit);
            for (int ii = 0; ii < strEnvironments.Length; ii++)
            {
                if (strEnvironments[ii].Trim() != "")
                    strQuery += "&eid=" + strEnvironments[ii].Trim();
            }
            Response.Redirect(oPage.GetFullLink(intPage) + "?id=" + Request.QueryString["id"] + "&div=D" + strQuery + "&start=" + txtStart.Text + "&end=" + txtEnd.Text);
        }
        private void LoadFilters()
        {
            strFilter += LoadFilter("pid", "projectid");
            strFilter += LoadFilter("lid", "addressid");
            strFilter += LoadFilter("xid", "confidenceid");
            strFilter += LoadFilter("cid", "classid");
            strFilter += LoadFilter("eid", "environmentid");
            if (Request.QueryString["start"] != null && Request.QueryString["start"] != "")
                strFilter += " AND (implementation >= '" + Request.QueryString["start"] + "')";
            if (Request.QueryString["end"] != null && Request.QueryString["end"] != "")
                strFilter += " AND (implementation <= '" + Request.QueryString["end"] + "')";
        }
        private string LoadFilter(string _query, string _name)
        {
            char[] strSplit = { ',' };
            string strQuery = Request.QueryString[_query];
            string strMiniFilter = "";
            string strReturn = "";
            if (strQuery != null && strQuery != "0")
            {
                string[] strQuerys;
                strQuerys = strQuery.Split(strSplit);
                for (int ii = 0; ii < strQuerys.Length; ii++)
                {
                    if (strQuerys[ii].Trim() != "")
                    {
                        if (strMiniFilter != "")
                            strMiniFilter += " OR ";
                        strMiniFilter += _name + " = " + strQuerys[ii].Trim();
                    }
                }
                strReturn += " AND (" + strMiniFilter + ")";
            }
            return strReturn;
        }
        public void DayRender(Object Sender, DayRenderEventArgs e)
        {
            if (!IsPostBack)
            {
                e.Day.IsSelectable = false;
                if (e.Day.Date != calThis.SelectedDate)
                {
                    e.Cell.Attributes.Add("onmouseover", "CalendarOver(this);");
                    e.Cell.Attributes.Add("onmouseout", "CalendarOut(this);");
                }
                e.Cell.ToolTip = e.Day.Date.ToLongDateString();
                e.Cell.Text = "<table width=\"100%\" cellpadding=\"1\" cellspacing=\"0\" border=\"0\">";
                if (e.Day.IsOtherMonth == true)
                    e.Cell.Text += "<tr><td nowrap class=\"headergray\">" + e.Day.DayNumberText + "</td></tr>";
                else
                {
                    e.Cell.Text += "<tr><td nowrap class=\"header\">" + e.Day.DayNumberText + "</td></tr>";
                    int intCount = 0;
                    double dblAmount = 0.00;
                    if (strFilter.StartsWith(" AND ") == true)
                        strFilter = strFilter.Substring(5);
                    bool bold = false;
                    DataRow[] drReturn = dsDemand.Tables[0].Select(strFilter);
                    foreach (DataRow dr in drReturn)
                    {
                        int quantity = 0;
                        Int32.TryParse(dr["quantity"].ToString(), out quantity);
                        int storage_shared = 0;
                        Int32.TryParse(dr["storage_shared"].ToString(), out storage_shared);
                        int storage_non_shared = 0;
                        Int32.TryParse(dr["storage_non_shared"].ToString(), out storage_non_shared);
                        DateTime commitment = DateTime.MinValue;
                        double total = (storage_non_shared + storage_shared);
                        if (DateTime.TryParse(dr["commitment"].ToString(), out commitment) && commitment < e.Day.Date && total > 0.00)
                        {
                            storage_non_shared = storage_non_shared * quantity;
                            dblAmount += (storage_non_shared + storage_shared);
                            intCount += quantity;

                            if (commitment.AddDays(1) == e.Day.Date)    // yesterday
                            {
                                e.Cell.Text += "<tr><td align=\"right\" nowrap>+ " + total.ToString() + " GB (" + quantity.ToString() + ")</td></tr>";
                                bold = true;
                            }
                        }
                    }
                    double dblHidden = 0.00;
                    if (hdnAmount.Value != "")
                        dblHidden = double.Parse(hdnAmount.Value);
                    dblHidden += dblAmount;
                    hdnAmount.Value = dblHidden.ToString("N");
                    double dblBackups = double.Parse(intCount.ToString());
                    double dblProgress = dblBackups / double.Parse(intMax.ToString());
                    dblTotal = dblAmount;
                    //e.Cell.Text += "<tr><td align=\"right\" nowrap>" + oServiceRequest.GetStatusBar(dblProgress * 100, "75", "8", false) + "</td><td nowrap width=\"100%\">" + dblBackups.ToString() + "</td></tr>";
                    e.Cell.Text += "<tr><td align=\"right\" nowrap>" + (bold ? "<b>" : "") + dblAmount.ToString() + "&nbsp;GB" + (bold ? "</b>" : "") + "</td></tr>";
                }
                e.Cell.Text += "</table>";
            }
        }
        protected void btnChange_Click(Object Sender, ImageClickEventArgs e)
        {
            Response.Redirect(oPage.GetFullLink(intPage) + "?id=" + Request.QueryString["id"] + "&change=" + ddlMonth.SelectedItem.Value + "/1/" + ddlYear.SelectedItem.Value);
        }
        protected void btnBack_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(oPage.GetFullLink(intPage) + "?id=" + Request.QueryString["id"] + "&change=" + Request.QueryString["day"]);
        }
    }
}