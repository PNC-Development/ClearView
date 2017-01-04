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
    public partial class storage_demand : System.Web.UI.UserControl
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
        protected Classes oClass;
        protected BuildLocation oBuildLocation;
        protected int intProfile;
        protected int intApplication = 0;
        protected int intPage = 0;
        protected string strDemand = "";
        protected int intMax = 50;
        protected int intMaxPorts = 50;
        protected string strFilter = "";
        protected int intGroup = 0;
        protected string strViews = "";
        protected string strParameters = "";
        private double dblTotal = 0.00;
        private double dblTotalN = 0.00;
        private double dblTotalE = 0.00;
        protected bool boolShowGrowth = true;
        protected bool boolAppliedFilter = false;
        protected bool boolAppliedGroup = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oPlatform = new Platforms(intProfile, dsn);
            oType = new Types(intProfile, dsn);
            oModelsProperties = new ModelsProperties(intProfile, dsn);
            oServiceRequest = new ServiceRequests(intProfile, dsn);
            oDesign = new Design(intProfile, dsn);
            oPage = new Pages(intProfile, dsn);
            oClass = new Classes(intProfile, dsn);
            oBuildLocation = new BuildLocation(intProfile, dsn);
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
                    LoadLists();
                    LoadFilters();
                    LoadGroups(intPlatform);
                    //if (boolAppliedFilter == false && boolAppliedGroup == false && boolShowGrowth == true)
                    //    panGrowth.Visible = true;
                }
                btnProjects.Attributes.Add("onclick", "return MakeWider(this, '" + lstProjects.ClientID + "');");
                btnProjectsClear.Attributes.Add("onclick", "return ClearList('" + lstProjects.ClientID + "');");
                btnConfidences.Attributes.Add("onclick", "return MakeWider(this, '" + lstConfidences.ClientID + "');");
                btnConfidencesClear.Attributes.Add("onclick", "return ClearList('" + lstConfidences.ClientID + "');");
                imgStart.Attributes.Add("onclick", "return ShowCalendar('" + txtStart.ClientID + "');");
                imgEnd.Attributes.Add("onclick", "return ShowCalendar('" + txtEnd.ClientID + "');");
                btnGo1.Attributes.Add("onclick", "return ProcessButton(this);");
                btnGo2.Attributes.Add("onclick", "return ProcessButton(this);");
            }
        }
        private void LoadLists()
        {
            Projects oProject = new Projects(intProfile, dsn);
            lstProjects.DataValueField = "projectid";
            if (Request.QueryString["sort"] == null || Request.QueryString["sort"] == "")
                lstProjects.DataTextField = "namenumber";
            else
                lstProjects.DataTextField = Request.QueryString["sort"];
            lstProjects.DataSource = oProject.GetForecast();
            lstProjects.DataBind();
            lstProjects.Items.Insert(0, new ListItem("-- ALL --", "0"));
            LoadList("pid", "Project(s)", lstProjects, null);
            Confidence oConfidence = new Confidence(intProfile, dsn);
            lstConfidences.DataValueField = "id";
            lstConfidences.DataTextField = "name";
            lstConfidences.DataSource = oConfidence.Gets(1);
            lstConfidences.DataBind();
            lstConfidences.Items.Insert(0, new ListItem("-- ALL --", "0"));
            LoadList("xid", "Confidence(s)", lstConfidences, null);
            // Groups
            ddlGroup1.Items.Insert(0, new ListItem("Environment", "env"));
            ddlGroup1.Items.Insert(0, new ListItem("Class", "cla"));
            ddlGroup1.Items.Insert(0, new ListItem("Confidence", "con"));
            ddlGroup1.Items.Insert(0, new ListItem("Project", "pro"));
            ddlGroup1.Items.Insert(0, new ListItem("-- NONE --", "0"));
            ddlGroup2.Items.Insert(0, new ListItem("Environment", "env"));
            ddlGroup2.Items.Insert(0, new ListItem("Class", "cla"));
            ddlGroup2.Items.Insert(0, new ListItem("Confidence", "con"));
            ddlGroup2.Items.Insert(0, new ListItem("Project", "pro"));
            ddlGroup2.Items.Insert(0, new ListItem("-- NONE --", "0"));
            ddlGroup3.Items.Insert(0, new ListItem("Environment", "env"));
            ddlGroup3.Items.Insert(0, new ListItem("Class", "cla"));
            ddlGroup3.Items.Insert(0, new ListItem("Confidence", "con"));
            ddlGroup3.Items.Insert(0, new ListItem("Project", "pro"));
            ddlGroup3.Items.Insert(0, new ListItem("-- NONE --", "0"));
            ddlGroup4.Items.Insert(0, new ListItem("Environment", "env"));
            ddlGroup4.Items.Insert(0, new ListItem("Class", "cla"));
            ddlGroup4.Items.Insert(0, new ListItem("Confidence", "con"));
            ddlGroup4.Items.Insert(0, new ListItem("Project", "pro"));
            ddlGroup4.Items.Insert(0, new ListItem("-- NONE --", "0"));
            ddlGroup5.Items.Insert(0, new ListItem("Environment", "env"));
            ddlGroup5.Items.Insert(0, new ListItem("Class", "cla"));
            ddlGroup5.Items.Insert(0, new ListItem("Confidence", "con"));
            ddlGroup5.Items.Insert(0, new ListItem("Project", "pro"));
            ddlGroup5.Items.Insert(0, new ListItem("-- NONE --", "0"));
            // Views
            strViews = "";
            if (strViews == "")
                strViews = "<tr><td><img src=\"/images/cancel.gif\" border=\"0\" align=\"absmiddle\" /> You have no saved views</td></tr>";
            strViews = "<table cellpadding=\"3\" cellspacing=\"1\" border=\"0\">" + strViews + "</table>";
            // Load Dates and Groups
            txtStart.Text = Request.QueryString["start"];
            txtEnd.Text = Request.QueryString["end"];
            ddlGroup1.SelectedValue = Request.QueryString["g1"];
            ddlGroup2.SelectedValue = Request.QueryString["g2"];
            ddlGroup3.SelectedValue = Request.QueryString["g3"];
            ddlGroup4.SelectedValue = Request.QueryString["g4"];
            ddlGroup5.SelectedValue = Request.QueryString["g5"];
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
                StringBuilder sb = new StringBuilder(strParameters);

                bool boolAll = (strQuery == "0");
                if (boolAll == false)
                {
                    sb.Append("<tr><td><b>");
                    sb.Append(_name);
                    sb.Append(":</b></td></tr>");
                }
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
                                {
                                    sb.Append("<tr><td>&nbsp;&nbsp;&nbsp;&quot;");
                                    sb.Append(oList.Text);
                                    sb.Append("&quot;</td></tr>");
                                }
                                oList.Selected = true;
                                if (_hidden != null)
                                {
                                    _hidden.Value += oList.Value + ";";
                                }
                                break;
                            }
                        }
                    }
                }

                strParameters = sb.ToString();
            }
        }
        protected void btnGo_Click(Object Sender, EventArgs e)
        {
            Filter("");
        }
        private void Filter(string strSort)
        {
            StringBuilder sb = new StringBuilder();

            if (strSort == "" && Request.QueryString["sort"] != null)
            {
                sb.Append("&sort=");
                sb.Append(Request.QueryString["sort"]);
            }
            else
            {
                sb.Append("&sort=");
                sb.Append(strSort);
            }

            foreach (ListItem oList in lstProjects.Items)
            {
                if (oList.Selected == true)
                {
                    sb.Append("&pid=");
                    sb.Append(oList.Value);
                }
            }

            foreach (ListItem oList in lstConfidences.Items)
            {
                if (oList.Selected == true)
                {
                    sb.Append("&xid=");
                    sb.Append(oList.Value);
                }
            }

            Response.Redirect(string.Concat(oPage.GetFullLink(intPage), "?id=", Request.QueryString["id"], "&div=D", sb.ToString(), "&start=", txtStart.Text.Trim(), "&end=", txtEnd.Text.Trim(), "&g1=", ddlGroup1.SelectedItem.Value, "&g2=", ddlGroup2.SelectedItem.Value, "&g3=", ddlGroup3.SelectedItem.Value, "&g4=", ddlGroup4.SelectedItem.Value, "&g5=", ddlGroup5.SelectedItem.Value));
        }
        private void LoadFilters()
        {
            strFilter += LoadFilter("pid", "projectid");
            strFilter += LoadFilter("xid", "confidenceid");
            if (Request.QueryString["start"] != null && Request.QueryString["start"] != "")
                strFilter += " AND (implementation >= '" + Request.QueryString["start"] + "')";
            if (Request.QueryString["end"] != null && Request.QueryString["end"] != "")
                strFilter += " AND (implementation <= '" + Request.QueryString["end"] + "')";
            if (strFilter != "")
                boolAppliedFilter = true;
            hdnFilter.Value = strFilter;
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
                    if (_query == "cid")
                    {
                        int intClass = Int32.Parse(strQuerys[ii].Trim());
                        if (oClass.IsTestDev(intClass))
                        {
                            if (strMiniFilter != "")
                                strMiniFilter += " OR ";
                            strMiniFilter += "(test = 1)";
                        }
                    }
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
        private void LoadGroups(int _platformid)
        {
            DataSet dsDemand = oDesign.GetForecast();
            DataTable dtDemand = dsDemand.Tables[0];
            if (Request.QueryString["g1"] != null && Request.QueryString["g1"] != "" && Request.QueryString["g1"] != "0")
            {
                boolAppliedGroup = true;
                strDemand += ShowGroup(_platformid, dtDemand, 1, "");
            }
            else
                strDemand += LoadGroup(_platformid, dtDemand, "");
        }
        private string ShowGroup(int _platformid, DataTable _demand, int _group_num, string _existing_filters)
        {
            StringBuilder sbGroup = new StringBuilder();
            StringBuilder sbGroupTemp = new StringBuilder();
            string strQuery = Request.QueryString["g" + _group_num.ToString()];
            _group_num = _group_num + 1;
            object oNext = Request.QueryString["g" + _group_num.ToString()];

            switch (strQuery)
            {
                case "pro":
                    string strProject = LoadFilter("pid", "projectid");
                    if (strProject.StartsWith(" AND ") == true)
                        strProject = strProject.Substring(5);
                    Projects oProject = new Projects(intProfile, dsn);
                    DataSet dsProjects = oProject.GetForecast();
                    DataRow[] drProjects = dsProjects.Tables[0].Select(strProject);
                    foreach (DataRow drProject in drProjects)
                    {
                        dblTotal = 0.00;
                        dblTotalN = 0.00;
                        dblTotalE = 0.00;
                        int intID = Int32.Parse(drProject["projectid"].ToString());
                        intGroup++;
                        sbGroupTemp = new StringBuilder();
                        if (oNext == null || oNext.ToString() == "" || oNext.ToString() == "0")
                        {
                            sbGroupTemp.Append("<tr><td></td><td colspan=\"3\" width=\"100%\" id=\"divGroup_");
                            sbGroupTemp.Append(intGroup.ToString());
                            sbGroupTemp.Append("\" style=\"display:none\">");
                            sbGroupTemp.Append(LoadGroup(_platformid, _demand, _existing_filters + " AND projectid = " + drProject["projectid"].ToString()));
                            sbGroupTemp.Append("</td></tr>");
                        }
                        else
                        {
                            sbGroupTemp.Append("<tr><td></td><td colspan=\"3\" width=\"100%\" id=\"divGroup_");
                            sbGroupTemp.Append(intGroup.ToString());
                            sbGroupTemp.Append("\" style=\"display:none\">");
                            sbGroupTemp.Append(ShowGroup(_platformid, _demand, _group_num, _existing_filters + " AND projectid = " + drProject["projectid"].ToString()));
                            sbGroupTemp.Append("</td></tr>");
                        }
                        sbGroup.Append("<tr><td><a href=\"javascript:void(0);\" onclick=\"ShowDetail('imgGroup_");
                        sbGroup.Append(intGroup.ToString());
                        sbGroup.Append("','divGroup_");
                        sbGroup.Append(intGroup.ToString());
                        sbGroup.Append("');\"><img id=\"imgGroup_");
                        sbGroup.Append(intGroup.ToString());
                        sbGroup.Append("\" src=\"/images/biggerPlus.gif\" border=\"0\" align=\"absmiddle\" /></a></td><td nowrap>");
                        sbGroup.Append(oProject.Get(intID, "name"));
                        sbGroup.Append("</td><td width=\"100%\">");
                        sbGroup.Append(oServiceRequest.GetStatusBarFill(((double.Parse(dblTotal.ToString()) / double.Parse(intMax.ToString())) * 100.00), "95", false, "#CC3300"));
                        sbGroup.Append("</td><td nowrap title='");
                        sbGroup.Append(dblTotalN.ToString());
                        sbGroup.Append("GB, ");
                        sbGroup.Append(dblTotalE.ToString());
                        sbGroup.Append("GB'>");
                        sbGroup.Append(dblTotal.ToString());
                        sbGroup.Append(" GB</td></tr>");
                        sbGroup.Append(sbGroupTemp.ToString());
                    }
                    break;
                case "con":
                    string strConfidence = LoadFilter("xid", "id");
                    if (strConfidence.StartsWith(" AND ") == true)
                        strConfidence = strConfidence.Substring(5);
                    Confidence oConfidence = new Confidence(intProfile, dsn);
                    DataSet dsConfidences = oConfidence.Gets(1);
                    DataRow[] drConfidences = dsConfidences.Tables[0].Select(strConfidence);
                    foreach (DataRow drConfidence in drConfidences)
                    {
                        dblTotal = 0.00;
                        dblTotalN = 0.00;
                        dblTotalE = 0.00;
                        intGroup++;
                        sbGroupTemp = new StringBuilder();
                        if (oNext == null || oNext.ToString() == "" || oNext.ToString() == "0")
                        {
                            sbGroupTemp.Append("<tr><td></td><td colspan=\"3\" width=\"100%\" id=\"divGroup_");
                            sbGroupTemp.Append(intGroup.ToString());
                            sbGroupTemp.Append("\" style=\"display:none\">");
                            sbGroupTemp.Append(LoadGroup(_platformid, _demand, _existing_filters + " AND confidenceid = " + drConfidence["id"].ToString()));
                            sbGroupTemp.Append("</td></tr>");
                        }
                        else
                        {
                            sbGroupTemp.Append("<tr><td></td><td colspan=\"3\" width=\"100%\" id=\"divGroup_");
                            sbGroupTemp.Append(intGroup.ToString());
                            sbGroupTemp.Append("\" style=\"display:none\">");
                            sbGroupTemp.Append(ShowGroup(_platformid, _demand, _group_num, _existing_filters + " AND confidenceid = " + drConfidence["id"].ToString()));
                            sbGroupTemp.Append("</td></tr>");
                        }
                        sbGroup.Append("<tr><td><a href=\"javascript:void(0);\" onclick=\"ShowDetail('imgGroup_");
                        sbGroup.Append(intGroup.ToString());
                        sbGroup.Append("','divGroup_");
                        sbGroup.Append(intGroup.ToString());
                        sbGroup.Append("');\"><img id=\"imgGroup_");
                        sbGroup.Append(intGroup.ToString());
                        sbGroup.Append("\" src=\"/images/biggerPlus.gif\" border=\"0\" align=\"absmiddle\" /></a></td><td nowrap>");
                        sbGroup.Append(drConfidence["name"].ToString());
                        sbGroup.Append("</td><td width=\"100%\">");
                        sbGroup.Append(oServiceRequest.GetStatusBarFill(((double.Parse(dblTotal.ToString()) / double.Parse(intMax.ToString())) * 100.00), "95", false, "#CC3300"));
                        sbGroup.Append("</td><td nowrap title='");
                        sbGroup.Append(dblTotalN.ToString());
                        sbGroup.Append("GB, ");
                        sbGroup.Append(dblTotalE.ToString());
                        sbGroup.Append("GB'>" + dblTotal.ToString());
                        sbGroup.Append(" GB</td></tr>");
                        sbGroup.Append(sbGroupTemp.ToString());
                    }
                    break;
                case "loc":
                    string strLocation = LoadFilter("lid", "id");
                    if (strLocation.StartsWith(" AND ") == true)
                        strLocation = strLocation.Substring(5);
                    Locations oLocation = new Locations(intProfile, dsn);
                    DataSet dsLocations = oLocation.GetAddresss(1);
                    DataRow[] drLocations = dsLocations.Tables[0].Select(strLocation);
                    foreach (DataRow drLocation in drLocations)
                    {
                        dblTotal = 0.00;
                        dblTotalN = 0.00;
                        dblTotalE = 0.00;
                        intGroup++;
                        sbGroupTemp = new StringBuilder();
                        if (oNext == null || oNext.ToString() == "" || oNext.ToString() == "0")
                        {
                            sbGroupTemp.Append("<tr><td></td><td colspan=\"3\" width=\"100%\" id=\"divGroup_");
                            sbGroupTemp.Append(intGroup.ToString());
                            sbGroupTemp.Append("\" style=\"display:none\">");
                            sbGroupTemp.Append(LoadGroup(_platformid, _demand, _existing_filters + " AND addressid = " + drLocation["id"].ToString()));
                            sbGroupTemp.Append("</td></tr>");
                        }
                        else
                        {
                            sbGroupTemp.Append("<tr><td></td><td colspan=\"3\" width=\"100%\" id=\"divGroup_");
                            sbGroupTemp.Append(intGroup.ToString());
                            sbGroupTemp.Append("\" style=\"display:none\">");
                            sbGroupTemp.Append(ShowGroup(_platformid, _demand, _group_num, _existing_filters + " AND addressid = " + drLocation["id"].ToString()));
                            sbGroupTemp.Append("</td></tr>");
                        }
                        sbGroup.Append("<tr><td><a href=\"javascript:void(0);\" onclick=\"ShowDetail('imgGroup_");
                        sbGroup.Append(intGroup.ToString());
                        sbGroup.Append("','divGroup_");
                        sbGroup.Append(intGroup.ToString());
                        sbGroup.Append("');\"><img id=\"imgGroup_");
                        sbGroup.Append(intGroup.ToString());
                        sbGroup.Append("\" src=\"/images/biggerPlus.gif\" border=\"0\" align=\"absmiddle\" /></a></td><td nowrap>");
                        sbGroup.Append(drLocation["fullname"].ToString());
                        sbGroup.Append("</td><td width=\"100%\">");
                        sbGroup.Append(oServiceRequest.GetStatusBarFill(((double.Parse(dblTotal.ToString()) / double.Parse(intMax.ToString())) * 100.00), "95", false, "#CC3300"));
                        sbGroup.Append("</td><td nowrap title='");
                        sbGroup.Append(dblTotalN.ToString());
                        sbGroup.Append("GB, ");
                        sbGroup.Append(dblTotalE.ToString());
                        sbGroup.Append("GB'>");
                        sbGroup.Append(dblTotal.ToString());
                        sbGroup.Append(" GB</td></tr>");
                        sbGroup.Append(sbGroupTemp.ToString());
                    }
                    break;
                case "cla":
                    string strClass = LoadFilter("cid", "id");
                    if (strClass.StartsWith(" AND ") == true)
                        strClass = strClass.Substring(5);
                    Classes oClass = new Classes(intProfile, dsn);
                    DataSet dsClasses = oClass.Gets(1);
                    DataRow[] drClasses = dsClasses.Tables[0].Select(strClass);
                    foreach (DataRow drClass in drClasses)
                    {
                        dblTotal = 0.00;
                        dblTotalN = 0.00;
                        dblTotalE = 0.00;
                        intGroup++;
                        sbGroupTemp = new StringBuilder();
                        if (oNext == null || oNext.ToString() == "" || oNext.ToString() == "0")
                        {
                            sbGroupTemp.Append("<tr><td></td><td colspan=\"3\" width=\"100%\" id=\"divGroup_");
                            sbGroupTemp.Append(intGroup.ToString());
                            sbGroupTemp.Append("\" style=\"display:none\">");
                            sbGroupTemp.Append(LoadGroup(_platformid, _demand, _existing_filters + " AND classid = " + drClass["id"].ToString()));
                            sbGroupTemp.Append("</td></tr>");
                        }
                        else
                        {
                            sbGroupTemp.Append("<tr><td></td><td colspan=\"3\" width=\"100%\" id=\"divGroup_");
                            sbGroupTemp.Append(intGroup.ToString());
                            sbGroupTemp.Append("\" style=\"display:none\">");
                            sbGroupTemp.Append(ShowGroup(_platformid, _demand, _group_num, _existing_filters + " AND classid = " + drClass["id"].ToString()));
                            sbGroupTemp.Append("</td></tr>");
                        }
                        sbGroup.Append("<tr><td><a href=\"javascript:void(0);\" onclick=\"ShowDetail('imgGroup_");
                        sbGroup.Append(intGroup.ToString());
                        sbGroup.Append("','divGroup_");
                        sbGroup.Append(intGroup.ToString());
                        sbGroup.Append("');\"><img id=\"imgGroup_");
                        sbGroup.Append(intGroup.ToString());
                        sbGroup.Append("\" src=\"/images/biggerPlus.gif\" border=\"0\" align=\"absmiddle\" /></a></td><td nowrap>");
                        sbGroup.Append(drClass["name"].ToString());
                        sbGroup.Append("</td><td width=\"100%\">");
                        sbGroup.Append(oServiceRequest.GetStatusBarFill(((double.Parse(dblTotal.ToString()) / double.Parse(intMax.ToString())) * 100.00), "95", false, "#CC3300"));
                        sbGroup.Append("</td><td nowrap title='");
                        sbGroup.Append(dblTotalN.ToString());
                        sbGroup.Append("GB, ");
                        sbGroup.Append(dblTotalE.ToString());
                        sbGroup.Append("GB'>");
                        sbGroup.Append(dblTotal.ToString());
                        sbGroup.Append(" GB</td></tr>");
                        sbGroup.Append(sbGroupTemp.ToString());
                    }
                    break;
                case "env":
                    string strEnvironment = LoadFilter("eid", "id");
                    if (strEnvironment.StartsWith(" AND ") == true)
                        strEnvironment = strEnvironment.Substring(5);
                    Environments oEnvironment = new Environments(intProfile, dsn);
                    DataSet dsEnvironments = oEnvironment.Gets(1);
                    DataRow[] drEnvironments = dsEnvironments.Tables[0].Select(strEnvironment);
                    foreach (DataRow drEnvironment in drEnvironments)
                    {
                        dblTotal = 0.00;
                        dblTotalN = 0.00;
                        dblTotalE = 0.00;
                        intGroup++;
                        sbGroupTemp = new StringBuilder();
                        if (oNext == null || oNext.ToString() == "" || oNext.ToString() == "0")
                        {
                            sbGroupTemp.Append("<tr><td></td><td colspan=\"3\" width=\"100%\" id=\"divGroup_");
                            sbGroupTemp.Append(intGroup.ToString());
                            sbGroupTemp.Append("\" style=\"display:none\">");
                            sbGroupTemp.Append(LoadGroup(_platformid, _demand, _existing_filters + " AND environmentid = " + drEnvironment["id"].ToString()));
                            sbGroupTemp.Append("</td></tr>");
                        }
                        else
                        {
                            sbGroupTemp.Append("<tr><td></td><td colspan=\"3\" width=\"100%\" id=\"divGroup_");
                            sbGroupTemp.Append(intGroup.ToString());
                            sbGroupTemp.Append("\" style=\"display:none\">");
                            sbGroupTemp.Append(ShowGroup(_platformid, _demand, _group_num, _existing_filters + " AND environmentid = " + drEnvironment["id"].ToString()));
                            sbGroupTemp.Append("</td></tr>");
                        }
                        sbGroup.Append("<tr><td><a href=\"javascript:void(0);\" onclick=\"ShowDetail('imgGroup_");
                        sbGroup.Append(intGroup.ToString());
                        sbGroup.Append("','divGroup_");
                        sbGroup.Append(intGroup.ToString());
                        sbGroup.Append("');\"><img id=\"imgGroup_");
                        sbGroup.Append(intGroup.ToString());
                        sbGroup.Append("\" src=\"/images/biggerPlus.gif\" border=\"0\" align=\"absmiddle\" /></a></td><td nowrap>");
                        sbGroup.Append(drEnvironment["name"].ToString());
                        sbGroup.Append("</td><td width=\"100%\">");
                        sbGroup.Append(oServiceRequest.GetStatusBarFill(((double.Parse(dblTotal.ToString()) / double.Parse(intMax.ToString())) * 100.00), "95", false, "#CC3300"));
                        sbGroup.Append("</td><td nowrap title='");
                        sbGroup.Append(dblTotalN.ToString());
                        sbGroup.Append("GB, ");
                        sbGroup.Append(dblTotalE.ToString());
                        sbGroup.Append("GB'>");
                        sbGroup.Append(dblTotal.ToString());
                        sbGroup.Append(" GB</td></tr>");
                        sbGroup.Append(sbGroupTemp.ToString());
                    }
                    break;
            }

            if (sbGroup.ToString() != "")
            {
                sbGroup.Insert(0, "<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\" border=\"0\">");
                sbGroup.Append("</table>");
            }

            return sbGroup.ToString();
        }
        private string LoadGroup(int _platformid, DataTable _demand, string _additional_filter)
        {
            StringBuilder sb = new StringBuilder();
            double intHighProd = 0;
            double intStandardProd = 0;
            double intLowProd = 0;
            double intPortsProd = 0;
            double intHighQA = 0;
            double intStandardQA = 0;
            double intLowQA = 0;
            double intPortsQA = 0;
            double intHighTest4 = 0;
            double intStandardTest4 = 0;
            double intLowTest4 = 0;
            double intPortsTest4 = 0;

            double intHighTest9 = 0;
            double intStandardTest9 = 0;
            double intLowTest9 = 0;
            double intPortsTest9 = 0;
            double intHighRep = 0;
            double intStandardRep = 0;
            double intLowRep = 0;
            double intHighHA = 0;
            double intStandardHA = 0;
            double intLowHA = 0;
            double intPortsHA = 0;
            if (strFilter.StartsWith(" AND ") == true)
                strFilter = strFilter.Substring(5);
            if (strFilter == "" && _additional_filter.StartsWith(" AND ") == true)
                _additional_filter = _additional_filter.Substring(5);
            DataRow[] drReturn = _demand.Select(strFilter + _additional_filter);
            foreach (DataRow dr in drReturn)
            {
                int intClass = 0;
                Int32.TryParse(dr["classid"].ToString(), out intClass);
                int intEnv = 0;
                Int32.TryParse(dr["environmentid"].ToString(), out intEnv);
                int intAddress = 0;
                Int32.TryParse(dr["addressid"].ToString(), out intAddress);
                int intModel = 0;
                Int32.TryParse(dr["modelid"].ToString(), out intModel);
                int intParent = 0;
                if (intModel > 0)
                    intParent = Int32.Parse(oModelsProperties.Get(intModel, "modelid"));

                int quantity = 0;
                Int32.TryParse(dr["quantity"].ToString(), out quantity);
                int storage_shared = 0;
                Int32.TryParse(dr["storage_shared"].ToString(), out storage_shared);
                int storage_non_shared = 0;
                Int32.TryParse(dr["storage_non_shared"].ToString(), out storage_non_shared);
                DateTime commitment = DateTime.MinValue;
                double total = (storage_non_shared + storage_shared);
                storage_non_shared = storage_non_shared * quantity;

                dblTotalN += (storage_non_shared + storage_shared);

                //double dblReplicate = 0.00;
                //if (dr["replicate_times"].ToString() != "")
                //    dblReplicate = double.Parse(dr["replicate_times"].ToString());
                //if (intParent > 0)
                //{
                //    DataSet dsBuild = oBuildLocation.Gets(intClass, intEnv, intAddress, intParent);
                //    if (dsBuild.Tables[0].Rows.Count > 0)
                //        intAddress = Int32.Parse(dsBuild.Tables[0].Rows[0]["addressid"].ToString());
                //}
                //double intHP = 0;
                //double intSP = 0;
                //double intLP = 0;
                //double intHQ = 0;
                //double intSQ = 0;
                //double intLQ = 0;
                //double intHT = 0;
                //double intST = 0;
                //double intLT = 0;
                //double intHR = 0;
                //double intSR = 0;
                //double intLR = 0;
                //double intHA = 0;
                //double intSA = 0;
                //double intLA = 0;
                //int intAllocated = (intStoragePerBladeApp * Int32.Parse(dr["quantity"].ToString()));
                //// Production
                //intHP += double.Parse(dr["high_total"].ToString());
                //intSP += double.Parse(dr["standard_total"].ToString());
                //intLP += double.Parse(dr["low_total"].ToString());
                //// QA
                //intHQ += double.Parse(dr["high_qa"].ToString());
                //intSQ += double.Parse(dr["standard_qa"].ToString());
                //intLQ += double.Parse(dr["low_qa"].ToString());
                //// Test
                //intHT += double.Parse(dr["high_test"].ToString());
                //intST += double.Parse(dr["standard_test"].ToString());
                //intLT += double.Parse(dr["low_test"].ToString());
                //// Replication
                //intHR += double.Parse(dr["high_replicated"].ToString());
                //if (dr["high_replicated_os"].ToString() != "")
                //    intHR += double.Parse(dr["high_replicated_os"].ToString());
                //intSR += double.Parse(dr["standard_replicated"].ToString());
                //if (dr["standard_replicated_os"].ToString() != "")
                //    intSR += double.Parse(dr["standard_replicated_os"].ToString());
                //intLR += double.Parse(dr["low_replicated"].ToString());
                //if (dr["low_replicated_os"].ToString() != "")
                //    intLR += double.Parse(dr["low_replicated_os"].ToString());
                //// High Availability
                //intHA += double.Parse(dr["high_ha"].ToString());
                //if (dr["high_ha_os"].ToString() != "")
                //    intHA += double.Parse(dr["high_ha_os"].ToString());
                //intSA += double.Parse(dr["standard_ha"].ToString());
                //if (dr["standard_ha_os"].ToString() != "")
                //    intSA += double.Parse(dr["standard_ha_os"].ToString());
                //intLA += double.Parse(dr["low_ha"].ToString());
                //if (dr["low_ha_os"].ToString() != "")
                //    intLA += double.Parse(dr["low_ha_os"].ToString());
                //// Match up
                //intHighProd += intHP;
                //intStandardProd += intSP;
                //intLowProd += intLP;
                //if ((intHP + intSP + intLP) > 0)
                //    intPortsProd += double.Parse(dr["ports"].ToString());
                //intHighQA += intHQ;
                //intStandardQA += intSQ;
                //intLowQA += intLQ;
                //if ((intHQ + intSQ + intLQ) > 0)
                //    intPortsQA += double.Parse(dr["ports"].ToString());
                //if (intAddress == 715)      // OPs Center
                //{
                //    intHighTest4 += intHT;
                //    intStandardTest4 += intST;
                //    intLowTest4 += intLT;
                //    if ((intHT + intST + intLT) > 0)
                //        intPortsTest4 += double.Parse(dr["ports"].ToString());
                //}
                //if (intAddress == 696)      // Dalton
                //{
                //    intHighTest9 += intHT;
                //    intStandardTest9 += intST;
                //    intLowTest9 += intLT;
                //    if ((intHT + intST + intLT) > 0)
                //        intPortsTest9 += double.Parse(dr["ports"].ToString());
                //}
                //intHighRep += (intHR * dblReplicate);
                //intStandardRep += (intSR * dblReplicate);
                //intLowRep += (intLR * dblReplicate);
                //intHighHA += intHA;
                //intStandardHA += intSA;
                //intLowHA += intLA;
                //if ((intHA + intSA + intLA) > 0)
                //    intPortsHA += double.Parse(dr["ports"].ToString());
                //dblTotalN += intHP + intSP + intLP + intHQ + intSQ + intLQ + intHT + intST + intLT + (intHR * dblReplicate) + (intSR * dblReplicate) + (intLR * dblReplicate) + intHA + intSA + intLA;
            }

            // Add Storage Request Information
            if (boolAppliedFilter == false && boolAppliedGroup == false && boolShowGrowth == true)
            {
                Customized oCustomized = new Customized(0, dsn);
                DataSet dsCustomized = oCustomized.GetStorage3rdForecast();
                foreach (DataRow drCustomized in dsCustomized.Tables[0].Rows)
                {
                    double intHP = 0;
                    double intSP = 0;
                    double intLP = 0;
                    double intHQ = 0;
                    double intSQ = 0;
                    double intLQ = 0;
                    double intHT = 0;
                    double intST = 0;
                    double intLT = 0;
                    double dblReplicateTimes = (drCustomized["replicated"].ToString().ToUpper() == "YES" ? (drCustomized["fabric"].ToString().ToUpper() == "CISCO" ? 2.00 : 3.00) : 0.00);
                    double dblHA = (drCustomized["ha"].ToString().ToUpper() == "1" ? (drCustomized["fabric"].ToString().ToUpper() == "CISCO" ? 2.00 : 0.00) : 0.00);
                    string strPerformance = drCustomized["performance"].ToString().ToUpper();
                    int intAddress = Int32.Parse(drCustomized["addressid"].ToString());
                    if (drCustomized["prod"].ToString() == "1")
                    {
                        // Production
                        if (strPerformance.Contains("HIGH") == true)
                            intHP += double.Parse(drCustomized["amount"].ToString());
                        if (strPerformance.Contains("STANDARD") == true)
                            intSP += double.Parse(drCustomized["amount"].ToString());
                        if (strPerformance.Contains("LOW") == true)
                            intLP += double.Parse(drCustomized["amount"].ToString());
                    }
                    if (drCustomized["test"].ToString() == "1")
                    {
                        // Test
                        if (strPerformance.Contains("HIGH") == true)
                            intHT += double.Parse(drCustomized["amount"].ToString());
                        if (strPerformance.Contains("STANDARD") == true)
                            intST += double.Parse(drCustomized["amount"].ToString());
                        if (strPerformance.Contains("LOW") == true)
                            intLT += double.Parse(drCustomized["amount"].ToString());
                    }
                    if (drCustomized["qa"].ToString() == "1")
                    {
                        // QA
                        if (strPerformance.Contains("HIGH") == true)
                            intHQ += double.Parse(drCustomized["amount"].ToString());
                        if (strPerformance.Contains("STANDARD") == true)
                            intSQ += double.Parse(drCustomized["amount"].ToString());
                        if (strPerformance.Contains("LOW") == true)
                            intLQ += double.Parse(drCustomized["amount"].ToString());
                    }
                    // Match up
                    intHighProd += intHP + (intHP * dblHA);
                    intStandardProd += intSP + (intSP * dblHA);
                    intLowProd += intLP + (intLP * dblHA);

                    intHighRep += (intHP * dblReplicateTimes);
                    intStandardRep += (intSP * dblReplicateTimes);
                    intLowRep += (intLP * dblReplicateTimes);

                    intHighQA += intHQ;
                    intStandardQA += intSQ;
                    intLowQA += intLQ;

                    if (intAddress == 715)      // OPs Center
                    {
                        intHighTest4 += intHT;
                        intStandardTest4 += intST;
                        intLowTest4 += intLT;
                    }
                    if (intAddress == 696)      // Dalton
                    {
                        intHighTest9 += intHT;
                        intStandardTest9 += intST;
                        intLowTest9 += intLT;
                    }

                    dblTotalE += intHP + (intHP * dblHA) + intSP + (intSP * dblHA) + intLP + (intLP * dblHA) + intHQ + intSQ + intLQ + intHT + intST + intLT + (intHP * dblReplicateTimes) + (intSP * dblReplicateTimes) + (intLP * dblReplicateTimes);
                }
            }

            dblTotal = dblTotalN + dblTotalE;

            sb.Append("<tr><td colspan=\"4\" class=\"hugeheader\">4100 West 150th Street (Cleveland, Ohio)</td></tr>");
            sb.Append("<tr><td colspan=\"4\" class=\"header\">Production</td></tr>");
            sb.Append("<tr bgcolor=\"F6F6F6\" class=\"default\"><td nowrap><img src=\"/images/spacer.gif\" border=\"0\" width=\"10\" height=\"1\"/></td><td nowrap>");
            sb.Append(boolAppliedFilter == false && boolAppliedGroup == false && boolShowGrowth == true ? "<a href=\"/frame/inventory/storage3rd.aspx?prod=1&qa=0&test=0&add=715&high=1&stand=0&low=0&rep=0\" target=\"_blank\">High Performance:</a>" : "High Performance:");
            sb.Append("</td><td width=\"100%\">");
            sb.Append(oServiceRequest.GetStatusBarFill(((double.Parse(intHighProd.ToString()) / double.Parse(intMax.ToString())) * 100.00), "95", false, "#CC3300"));
            sb.Append("</td><td nowrap>");
            sb.Append(intHighProd.ToString());
            sb.Append(" GB</td></tr>");

            sb.Append("<tr bgcolor=\"F6F6F6\" class=\"default\"><td nowrap><img src=\"/images/spacer.gif\" border=\"0\" width=\"10\" height=\"1\"/></td><td nowrap>");
            sb.Append(boolAppliedFilter == false && boolAppliedGroup == false && boolShowGrowth == true ? "<a href=\"/frame/inventory/storage3rd.aspx?prod=1&qa=0&test=0&add=715&high=0&stand=1&low=0&rep=0\" target=\"_blank\">Standard Performance:</a>" : "Standard Performance:");
            sb.Append("</td><td width=\"100%\">");
            sb.Append(oServiceRequest.GetStatusBarFill(((double.Parse(intStandardProd.ToString()) / double.Parse(intMax.ToString())) * 100.00), "95", false, "#CC3300"));
            sb.Append("</td><td nowrap>");
            sb.Append(intStandardProd.ToString());
            sb.Append(" GB</td></tr>");
            sb.Append("<tr bgcolor=\"F6F6F6\" class=\"default\"><td nowrap><img src=\"/images/spacer.gif\" border=\"0\" width=\"10\" height=\"1\"/></td><td nowrap>");
            sb.Append(boolAppliedFilter == false && boolAppliedGroup == false && boolShowGrowth == true ? "<a href=\"/frame/inventory/storage3rd.aspx?prod=1&qa=0&test=0&add=715&high=0&stand=0&low=1&rep=0\" target=\"_blank\">Low Performance:</a>" : "Low Performance:");
            sb.Append("</td><td width=\"100%\">");
            sb.Append(oServiceRequest.GetStatusBarFill(((double.Parse(intLowProd.ToString()) / double.Parse(intMax.ToString())) * 100.00), "95", false, "#CC3300"));
            sb.Append("</td><td nowrap>");
            sb.Append(intLowProd.ToString());
            sb.Append(" GB</td></tr>");
            sb.Append("<tr bgcolor=\"F6F6F6\" class=\"default\"><td nowrap><img src=\"/images/spacer.gif\" border=\"0\" width=\"10\" height=\"1\"/></td><td nowrap>Ports:</td><td width=\"100%\">");
            sb.Append(oServiceRequest.GetStatusBarFill(((double.Parse(intPortsProd.ToString()) / double.Parse(intMaxPorts.ToString())) * 100.00), "95", false, "#CC3300"));
            sb.Append("</td><td nowrap>");
            sb.Append(intPortsProd.ToString());
            sb.Append("</td></tr>");
            sb.Append("<tr><td colspan=\"4\"><span style=\"width:100%;border-bottom:1 dotted #999999;\"/></td></tr>");

            sb.Append("<tr><td colspan=\"4\" class=\"header\">QA</td></tr>");

            sb.Append("<tr bgcolor=\"F6F6F6\" class=\"default\"><td nowrap><img src=\"/images/spacer.gif\" border=\"0\" width=\"10\" height=\"1\"/></td><td nowrap>");
            sb.Append(boolAppliedFilter == false && boolAppliedGroup == false && boolShowGrowth == true ? "<a href=\"/frame/inventory/storage3rd.aspx?prod=0&qa=1&test=0&add=715&high=1&stand=0&low=0&rep=0\" target=\"_blank\">High Performance:</a>" : "High Performance:");
            sb.Append("</td><td width=\"100%\">");
            sb.Append(oServiceRequest.GetStatusBarFill(((double.Parse(intHighQA.ToString()) / double.Parse(intMax.ToString())) * 100.00), "95", false, "#CC3300"));
            sb.Append("</td><td nowrap>");
            sb.Append(intHighQA.ToString());
            sb.Append(" GB</td></tr>");
            sb.Append("<tr bgcolor=\"F6F6F6\" class=\"default\"><td nowrap><img src=\"/images/spacer.gif\" border=\"0\" width=\"10\" height=\"1\"/></td><td nowrap>");
            sb.Append(boolAppliedFilter == false && boolAppliedGroup == false && boolShowGrowth == true ? "<a href=\"/frame/inventory/storage3rd.aspx?prod=0&qa=1&test=0&add=715&high=0&stand=1&low=0&rep=0\" target=\"_blank\">Standard Performance:</a>" : "Standard Performance:");
            sb.Append("</td><td width=\"100%\">");
            sb.Append(oServiceRequest.GetStatusBarFill(((double.Parse(intStandardQA.ToString()) / double.Parse(intMax.ToString())) * 100.00), "95", false, "#CC3300"));
            sb.Append("</td><td nowrap>");
            sb.Append(intStandardQA.ToString());
            sb.Append(" GB</td></tr>");
            sb.Append("<tr bgcolor=\"F6F6F6\" class=\"default\"><td nowrap><img src=\"/images/spacer.gif\" border=\"0\" width=\"10\" height=\"1\"/></td><td nowrap>");
            sb.Append(boolAppliedFilter == false && boolAppliedGroup == false && boolShowGrowth == true ? "<a href=\"/frame/inventory/storage3rd.aspx?prod=0&qa=1&test=0&add=715&high=0&stand=0&low=1&rep=0\" target=\"_blank\">Low Performance:</a>" : "Low Performance:");
            sb.Append("</td><td width=\"100%\">");
            sb.Append(oServiceRequest.GetStatusBarFill(((double.Parse(intLowQA.ToString()) / double.Parse(intMax.ToString())) * 100.00), "95", false, "#CC3300"));
            sb.Append("</td><td nowrap>");
            sb.Append(intLowQA.ToString());
            sb.Append(" GB</td></tr>");
            sb.Append("<tr bgcolor=\"F6F6F6\" class=\"default\"><td nowrap><img src=\"/images/spacer.gif\" border=\"0\" width=\"10\" height=\"1\"/></td><td nowrap>Ports:</td><td width=\"100%\">");
            sb.Append(oServiceRequest.GetStatusBarFill(((double.Parse(intPortsQA.ToString()) / double.Parse(intMaxPorts.ToString())) * 100.00), "95", false, "#CC3300"));
            sb.Append("</td><td nowrap>");
            sb.Append(intPortsQA.ToString());
            sb.Append("</td></tr>");
            sb.Append("<tr><td colspan=\"4\"><span style=\"width:100%;border-bottom:1 dotted #999999;\"/></td></tr>");

            sb.Append("<tr><td colspan=\"4\" class=\"header\">Test</td></tr>");

            sb.Append("<tr bgcolor=\"F6F6F6\" class=\"default\"><td nowrap><img src=\"/images/spacer.gif\" border=\"0\" width=\"10\" height=\"1\"/></td><td nowrap>");
            sb.Append(boolAppliedFilter == false && boolAppliedGroup == false && boolShowGrowth == true ? "<a href=\"/frame/inventory/storage3rd.aspx?prod=0&qa=0&test=1&add=715&high=1&stand=0&low=0&rep=0\" target=\"_blank\">High Performance:</a>" : "High Performance:");
            sb.Append("</td><td width=\"100%\">");
            sb.Append(oServiceRequest.GetStatusBarFill(((double.Parse(intHighTest4.ToString()) / double.Parse(intMax.ToString())) * 100.00), "95", false, "#CC3300"));
            sb.Append("</td><td nowrap>");
            sb.Append(intHighTest4.ToString());
            sb.Append(" GB</td></tr>");
            sb.Append("<tr bgcolor=\"F6F6F6\" class=\"default\"><td nowrap><img src=\"/images/spacer.gif\" border=\"0\" width=\"10\" height=\"1\"/></td><td nowrap>");
            sb.Append(boolAppliedFilter == false && boolAppliedGroup == false && boolShowGrowth == true ? "<a href=\"/frame/inventory/storage3rd.aspx?prod=0&qa=0&test=1&add=715&high=0&stand=1&low=0&rep=0\" target=\"_blank\">Standard Performance:</a>" : "Standard Performance:");
            sb.Append("</td><td width=\"100%\">");
            sb.Append(oServiceRequest.GetStatusBarFill(((double.Parse(intStandardTest4.ToString()) / double.Parse(intMax.ToString())) * 100.00), "95", false, "#CC3300"));
            sb.Append("</td><td nowrap>");
            sb.Append(intStandardTest4.ToString());
            sb.Append(" GB</td></tr>");
            sb.Append("<tr bgcolor=\"F6F6F6\" class=\"default\"><td nowrap><img src=\"/images/spacer.gif\" border=\"0\" width=\"10\" height=\"1\"/></td><td nowrap>");
            sb.Append(boolAppliedFilter == false && boolAppliedGroup == false && boolShowGrowth == true ? "<a href=\"/frame/inventory/storage3rd.aspx?prod=0&qa=0&test=1&add=715&high=0&stand=0&low=1&rep=0\" target=\"_blank\">Low Performance:</a>" : "Low Performance:");
            sb.Append("</td><td width=\"100%\">");
            sb.Append(oServiceRequest.GetStatusBarFill(((double.Parse(intLowTest4.ToString()) / double.Parse(intMax.ToString())) * 100.00), "95", false, "#CC3300"));
            sb.Append("</td><td nowrap>");
            sb.Append(intLowTest4.ToString());
            sb.Append(" GB</td></tr>");
            sb.Append("<tr bgcolor=\"F6F6F6\" class=\"default\"><td nowrap><img src=\"/images/spacer.gif\" border=\"0\" width=\"10\" height=\"1\"/></td><td nowrap>Ports:</td><td width=\"100%\">");
            sb.Append(oServiceRequest.GetStatusBarFill(((double.Parse(intPortsTest4.ToString()) / double.Parse(intMaxPorts.ToString())) * 100.00), "95", false, "#CC3300"));
            sb.Append("</td><td nowrap>");
            sb.Append(intPortsTest4.ToString());
            sb.Append("</td></tr>");

            sb.Append("<tr><td colspan=\"4\"><span style=\"width:100%;border-bottom:1 dotted #999999;\"/></td></tr>");

            sb.Append("<tr><td colspan=\"4\" class=\"header\">High Availability</td></tr>");

            sb.Append("<tr bgcolor=\"F6F6F6\" class=\"default\"><td nowrap><img src=\"/images/spacer.gif\" border=\"0\" width=\"10\" height=\"1\"/></td><td nowrap>");
            sb.Append(boolAppliedFilter == false && boolAppliedGroup == false && boolShowGrowth == true ? "<a href=\"/frame/inventory/storage3rd.aspx?prod=0&qa=0&test=1&add=715&high=1&stand=0&low=0&rep=0\" target=\"_blank\">High Performance:</a>" : "High Performance:");
            sb.Append("</td><td width=\"100%\">");
            sb.Append(oServiceRequest.GetStatusBarFill(((double.Parse(intHighHA.ToString()) / double.Parse(intMax.ToString())) * 100.00), "95", false, "#CC3300"));
            sb.Append("</td><td nowrap>");
            sb.Append(intHighHA.ToString());
            sb.Append(" GB</td></tr>");
            sb.Append("<tr bgcolor=\"F6F6F6\" class=\"default\"><td nowrap><img src=\"/images/spacer.gif\" border=\"0\" width=\"10\" height=\"1\"/></td><td nowrap>");
            sb.Append(boolAppliedFilter == false && boolAppliedGroup == false && boolShowGrowth == true ? "<a href=\"/frame/inventory/storage3rd.aspx?prod=0&qa=0&test=1&add=715&high=0&stand=1&low=0&rep=0\" target=\"_blank\">Standard Performance:</a>" : "Standard Performance:");
            sb.Append("</td><td width=\"100%\">");
            sb.Append(oServiceRequest.GetStatusBarFill(((double.Parse(intStandardHA.ToString()) / double.Parse(intMax.ToString())) * 100.00), "95", false, "#CC3300"));
            sb.Append("</td><td nowrap>");
            sb.Append(intStandardHA.ToString());
            sb.Append(" GB</td></tr>");
            sb.Append("<tr bgcolor=\"F6F6F6\" class=\"default\"><td nowrap><img src=\"/images/spacer.gif\" border=\"0\" width=\"10\" height=\"1\"/></td><td nowrap>");
            sb.Append(boolAppliedFilter == false && boolAppliedGroup == false && boolShowGrowth == true ? "<a href=\"/frame/inventory/storage3rd.aspx?prod=0&qa=0&test=1&add=715&high=0&stand=0&low=1&rep=0\" target=\"_blank\">Low Performance:</a>" : "Low Performance:");
            sb.Append("</td><td width=\"100%\">");
            sb.Append(oServiceRequest.GetStatusBarFill(((double.Parse(intLowHA.ToString()) / double.Parse(intMax.ToString())) * 100.00), "95", false, "#CC3300"));
            sb.Append("</td><td nowrap>");
            sb.Append(intLowHA.ToString());
            sb.Append(" GB</td></tr>");
            sb.Append("<tr bgcolor=\"F6F6F6\" class=\"default\"><td nowrap><img src=\"/images/spacer.gif\" border=\"0\" width=\"10\" height=\"1\"/></td><td nowrap>Ports:</td><td width=\"100%\">");
            sb.Append(oServiceRequest.GetStatusBarFill(((double.Parse(intPortsHA.ToString()) / double.Parse(intMaxPorts.ToString())) * 100.00), "95", false, "#CC3300"));
            sb.Append("</td><td nowrap>");
            sb.Append(intPortsHA.ToString());
            sb.Append("</td></tr>");

            sb.Append("<tr><td colspan=\"4\"><span style=\"width:100%;border-bottom:1 dotted #999999;\"/></td></tr>");

            sb.Append("<tr><td colspan=\"4\" class=\"header\">&nbsp;</td></tr>");

            sb.Append("<tr><td colspan=\"4\" class=\"hugeheader\">925 Dalton Street (Cincinnati, Ohio)</td></tr>");

            sb.Append("<tr><td colspan=\"4\" class=\"header\">Test</td></tr>");

            sb.Append("<tr bgcolor=\"F6F6F6\" class=\"default\"><td nowrap><img src=\"/images/spacer.gif\" border=\"0\" width=\"10\" height=\"1\"/></td><td nowrap>");
            sb.Append(boolAppliedFilter == false && boolAppliedGroup == false && boolShowGrowth == true ? "<a href=\"/frame/inventory/storage3rd.aspx?prod=0&qa=0&test=1&add=696&high=1&stand=0&low=0&rep=0\" target=\"_blank\">High Performance:</a>" : "High Performance:");
            sb.Append("</td><td width=\"100%\">");
            sb.Append(oServiceRequest.GetStatusBarFill(((double.Parse(intHighTest9.ToString()) / double.Parse(intMax.ToString())) * 100.00), "95", false, "#CC3300"));
            sb.Append("</td><td nowrap>");
            sb.Append(intHighTest9.ToString());
            sb.Append(" GB</td></tr>");
            sb.Append("<tr bgcolor=\"F6F6F6\" class=\"default\"><td nowrap><img src=\"/images/spacer.gif\" border=\"0\" width=\"10\" height=\"1\"/></td><td nowrap>");
            sb.Append(boolAppliedFilter == false && boolAppliedGroup == false && boolShowGrowth == true ? "<a href=\"/frame/inventory/storage3rd.aspx?prod=0&qa=0&test=1&add=696&high=0&stand=1&low=0&rep=0\" target=\"_blank\">Standard Performance:</a>" : "Standard Performance:");
            sb.Append("</td><td width=\"100%\">");
            sb.Append(oServiceRequest.GetStatusBarFill(((double.Parse(intStandardTest9.ToString()) / double.Parse(intMax.ToString())) * 100.00), "95", false, "#CC3300"));
            sb.Append("</td><td nowrap>");
            sb.Append(intStandardTest9.ToString());
            sb.Append(" GB</td></tr>");
            sb.Append("<tr bgcolor=\"F6F6F6\" class=\"default\"><td nowrap><img src=\"/images/spacer.gif\" border=\"0\" width=\"10\" height=\"1\"/></td><td nowrap>");
            sb.Append(boolAppliedFilter == false && boolAppliedGroup == false && boolShowGrowth == true ? "<a href=\"/frame/inventory/storage3rd.aspx?prod=0&qa=0&test=1&add=696&high=0&stand=0&low=1&rep=0\" target=\"_blank\">Low Performance:</a>" : "Low Performance:");
            sb.Append("</td><td width=\"100%\">");
            sb.Append(oServiceRequest.GetStatusBarFill(((double.Parse(intLowTest9.ToString()) / double.Parse(intMax.ToString())) * 100.00), "95", false, "#CC3300"));
            sb.Append("</td><td nowrap>");
            sb.Append(intLowTest9.ToString());
            sb.Append(" GB</td></tr>");
            sb.Append("<tr bgcolor=\"F6F6F6\" class=\"default\"><td nowrap><img src=\"/images/spacer.gif\" border=\"0\" width=\"10\" height=\"1\"/></td><td nowrap>Ports:</td><td width=\"100%\">");
            sb.Append(oServiceRequest.GetStatusBarFill(((double.Parse(intPortsTest9.ToString()) / double.Parse(intMaxPorts.ToString())) * 100.00), "95", false, "#CC3300"));
            sb.Append("</td><td nowrap>");
            sb.Append(intPortsTest9.ToString());
            sb.Append("</td></tr>");

            sb.Append("<tr><td colspan=\"4\" class=\"header\">Replication</td></tr>");

            sb.Append("<tr bgcolor=\"F6F6F6\" class=\"default\"><td nowrap><img src=\"/images/spacer.gif\" border=\"0\" width=\"10\" height=\"1\"/></td><td nowrap>");
            sb.Append(boolAppliedFilter == false && boolAppliedGroup == false && boolShowGrowth == true ? "<a href=\"/frame/inventory/storage3rd.aspx?prod=1&qa=0&test=0&add=715&high=1&stand=0&low=0&rep=1\" target=\"_blank\">High Performance:</a>" : "High Performance:");
            sb.Append("</td><td width=\"100%\">");
            sb.Append(oServiceRequest.GetStatusBarFill(((double.Parse(intHighRep.ToString()) / double.Parse(intMax.ToString())) * 100.00), "95", false, "#CC3300"));
            sb.Append("</td><td nowrap>");
            sb.Append(intHighRep.ToString());
            sb.Append(" GB</td></tr>");
            sb.Append("<tr bgcolor=\"F6F6F6\" class=\"default\"><td nowrap><img src=\"/images/spacer.gif\" border=\"0\" width=\"10\" height=\"1\"/></td><td nowrap>");
            sb.Append(boolAppliedFilter == false && boolAppliedGroup == false && boolShowGrowth == true ? "<a href=\"/frame/inventory/storage3rd.aspx?prod=1&qa=0&test=0&add=715&high=0&stand=1&low=0&rep=1\" target=\"_blank\">Standard Performance:</a>" : "Standard Performance:");
            sb.Append("</td><td width=\"100%\">");
            sb.Append(oServiceRequest.GetStatusBarFill(((double.Parse(intStandardRep.ToString()) / double.Parse(intMax.ToString())) * 100.00), "95", false, "#CC3300"));
            sb.Append("</td><td nowrap>");
            sb.Append(intStandardRep.ToString());
            sb.Append(" GB</td></tr>");
            sb.Append("<tr bgcolor=\"F6F6F6\" class=\"default\"><td nowrap><img src=\"/images/spacer.gif\" border=\"0\" width=\"10\" height=\"1\"/></td><td nowrap>");
            sb.Append(boolAppliedFilter == false && boolAppliedGroup == false && boolShowGrowth == true ? "<a href=\"/frame/inventory/storage3rd.aspx?prod=1&qa=0&test=0&add=715&high=0&stand=0&low=1&rep=1\" target=\"_blank\">Low Performance:</a>" : "Low Performance:");
            sb.Append("</td><td width=\"100%\">");
            sb.Append(oServiceRequest.GetStatusBarFill(((double.Parse(intLowRep.ToString()) / double.Parse(intMax.ToString())) * 100.00), "95", false, "#CC3300"));
            sb.Append("</td><td nowrap>");
            sb.Append(intLowRep.ToString());
            sb.Append(" GB</td></tr>");

            if (sb.ToString() != "")
            {
                sb.Insert(0, "<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\" border=\"0\" style=\"border:solid 1px #CCCCCC\">");
                sb.Append("</table>");
            }

            return sb.ToString();
        }
        protected void btnProjectOrderName_Click(Object Sender, EventArgs e)
        {
            Filter("namenumber");
        }
        protected void btnProjectOrderNumber_Click(Object Sender, EventArgs e)
        {
            Filter("numbername");
        }
        protected DataTable SelectDistinct(string TableName, DataTable SourceTable, string FieldName)
        {
            DataTable dt = new DataTable(TableName);
            dt.Columns.Add(FieldName, SourceTable.Columns[FieldName].DataType);

            object LastValue = null;
            foreach (DataRow dr in SourceTable.Select("", FieldName))
            {
                if (LastValue == null || !(ColumnEqual(LastValue, dr[FieldName])))
                {
                    LastValue = dr[FieldName];
                    dt.Rows.Add(new object[] { LastValue });
                }
            }
            return dt;
        }
        protected bool ColumnEqual(object A, object B)
        {
            if (A == DBNull.Value && B == DBNull.Value) //  both are DBNull.Value
                return true;
            if (A == DBNull.Value || B == DBNull.Value) //  only one is DBNull.Value
                return false;
            return (A.Equals(B));  // value type standard comparison
        }
    }
}