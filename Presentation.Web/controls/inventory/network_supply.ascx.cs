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
    public partial class network_supply : System.Web.UI.UserControl
    {

        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intStoragePerBladeOs = Int32.Parse(ConfigurationManager.AppSettings["STORAGE_PER_BLADE_OS"]);
        protected int intStoragePerBladeApp = Int32.Parse(ConfigurationManager.AppSettings["STORAGE_PER_BLADE_APP"]);
        protected int intPlatformServer = Int32.Parse(ConfigurationManager.AppSettings["ServerPlatformID"]);
        protected Platforms oPlatform;
        protected Types oType;
        protected ModelsProperties oModelsProperties;
        protected ServiceRequests oServiceRequest;
        protected Forecast oForecast;
        protected Pages oPage;
        protected Classes oClass;
        protected Asset oAsset;
        protected int intProfile;
        protected int intApplication = 0;
        protected int intPage = 0;
        protected string strDemand = "";
        protected int intMax = 50;
        protected string strFilter = "";
        protected int intGroup = 0;
        protected string strViews = "";
        protected string strParameters = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oPlatform = new Platforms(intProfile, dsn);
            oType = new Types(intProfile, dsn);
            oModelsProperties = new ModelsProperties(intProfile, dsn);
            oServiceRequest = new ServiceRequests(intProfile, dsn);
            oForecast = new Forecast(intProfile, dsn);
            oPage = new Pages(intProfile, dsn);
            oClass = new Classes(intProfile, dsn);
            oAsset = new Asset(intProfile, dsnAsset);
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
            // Groups
            ddlGroup1.Items.Insert(0, new ListItem("Environment", "env"));
            ddlGroup1.Items.Insert(0, new ListItem("Class", "cla"));
            ddlGroup1.Items.Insert(0, new ListItem("Location", "loc"));
            ddlGroup1.Items.Insert(0, new ListItem("Confidence", "con"));
            ddlGroup1.Items.Insert(0, new ListItem("Project", "pro"));
            ddlGroup1.Items.Insert(0, new ListItem("-- NONE --", "0"));
            ddlGroup2.Items.Insert(0, new ListItem("Environment", "env"));
            ddlGroup2.Items.Insert(0, new ListItem("Class", "cla"));
            ddlGroup2.Items.Insert(0, new ListItem("Location", "loc"));
            ddlGroup2.Items.Insert(0, new ListItem("Confidence", "con"));
            ddlGroup2.Items.Insert(0, new ListItem("Project", "pro"));
            ddlGroup2.Items.Insert(0, new ListItem("-- NONE --", "0"));
            ddlGroup3.Items.Insert(0, new ListItem("Environment", "env"));
            ddlGroup3.Items.Insert(0, new ListItem("Class", "cla"));
            ddlGroup3.Items.Insert(0, new ListItem("Location", "loc"));
            ddlGroup3.Items.Insert(0, new ListItem("Confidence", "con"));
            ddlGroup3.Items.Insert(0, new ListItem("Project", "pro"));
            ddlGroup3.Items.Insert(0, new ListItem("-- NONE --", "0"));
            ddlGroup4.Items.Insert(0, new ListItem("Environment", "env"));
            ddlGroup4.Items.Insert(0, new ListItem("Class", "cla"));
            ddlGroup4.Items.Insert(0, new ListItem("Location", "loc"));
            ddlGroup4.Items.Insert(0, new ListItem("Confidence", "con"));
            ddlGroup4.Items.Insert(0, new ListItem("Project", "pro"));
            ddlGroup4.Items.Insert(0, new ListItem("-- NONE --", "0"));
            ddlGroup5.Items.Insert(0, new ListItem("Environment", "env"));
            ddlGroup5.Items.Insert(0, new ListItem("Class", "cla"));
            ddlGroup5.Items.Insert(0, new ListItem("Location", "loc"));
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
            Filter();
        }
        private void Filter()
        {
            StringBuilder sb = new StringBuilder();
            
            foreach (ListItem oList in lstProjects.Items)
            {
                if (oList.Selected == true)
                {
                    sb.Append("&pid=");
                    sb.Append(oList.Value);
                }
            }

            foreach (ListItem oList in lstLocations.Items)
            {
                if (oList.Selected == true)
                {
                    sb.Append("&lid=");
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

            foreach (ListItem oList in lstClasses.Items)
            {
                if (oList.Selected == true)
                {
                    sb.Append("&cid=");
                    sb.Append(oList.Value);
                }
            }

            string strEnvironment = Request.Form[hdnEnvironment.UniqueID];
            string[] strEnvironments;
            char[] strSplit = { ';' };
            strEnvironments = strEnvironment.Split(strSplit);

            for (int ii = 0; ii < strEnvironments.Length; ii++)
            {
                if (strEnvironments[ii].Trim() != "")
                {
                    sb.Append("&eid=");
                    sb.Append(strEnvironments[ii].Trim());
                }
            }

            Response.Redirect(oPage.GetFullLink(intPage) + "?id=" + Request.QueryString["id"] + "&div=S" + sb.ToString() + "&start=" + txtStart.Text + "&end=" + txtEnd.Text + "&g1=" + ddlGroup1.SelectedItem.Value + "&g2=" + ddlGroup2.SelectedItem.Value + "&g3=" + ddlGroup3.SelectedItem.Value + "&g4=" + ddlGroup4.SelectedItem.Value + "&g5=" + ddlGroup5.SelectedItem.Value);
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
            hdnFilter.Value = strFilter;
        }
        private string LoadFilter(string _query, string _name)
        {
            char[] strSplit = { ',' };
            string strQuery = Request.QueryString[_query];
            StringBuilder sb = new StringBuilder();
            string strReturn = "";

            if (strQuery != null && strQuery != "0")
            {
                string[] strQuerys;
                strQuerys = strQuery.Split(strSplit);
                for (int ii = 0; ii < strQuerys.Length; ii++)
                {
                    if (strQuerys[ii].Trim() != "")
                    {
                        if (sb.ToString() != "")
                        {
                            sb.Append(" OR ");
                        }

                        sb.Append(_name);
                        sb.Append(" = ");
                        sb.Append(strQuerys[ii].Trim());
                    }
                }

                strReturn += " AND (" + sb.ToString() + ")";
            }

            return strReturn;
        }
        private void LoadGroups(int _platformid)
        {
            if (Request.QueryString["g1"] != null && Request.QueryString["g1"] != "" && Request.QueryString["g1"] != "0")
                strDemand += ShowGroup(_platformid, 1, "");
            else
                strDemand += LoadGroup(_platformid, "");
        }
        private string ShowGroup(int _platformid, int _group_num, string _existing_filters)
        {
            StringBuilder sb = new StringBuilder();
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
                    DataSet dsProjects = oProject.GetActive();
                    DataRow[] drProjects = dsProjects.Tables[0].Select(strProject);
                    
                    foreach (DataRow drProject in drProjects)
                    {
                        int intID = Int32.Parse(drProject["projectid"].ToString());
                        intGroup++;
                        sb.Append("<tr><td><a href=\"javascript:void(0);\" onclick=\"ShowDetail('imgGroup_");
                        sb.Append(intGroup.ToString());
                        sb.Append("','divGroup_");
                        sb.Append(intGroup.ToString());
                        sb.Append("');\"><img id=\"imgGroup_");
                        sb.Append(intGroup.ToString());
                        sb.Append("\" src=\"/images/biggerPlus.gif\" border=\"0\" align=\"absmiddle\" /></a></td><td width=\"100%\">");
                        sb.Append(oProject.Get(intID, "name"));
                        sb.Append("</td></tr>");

                        if (oNext == null || oNext.ToString() == "" || oNext.ToString() == "0")
                        {
                            sb.Append("<tr><td></td><td width=\"100%\" id=\"divGroup_");
                            sb.Append(intGroup.ToString());
                            sb.Append("\" style=\"display:none\">");
                            sb.Append(LoadGroup(_platformid, _existing_filters + " AND projectid = " + drProject["projectid"].ToString()));
                            sb.Append("</td></tr>");
                        }
                        else
                        {
                            sb.Append("<tr><td></td><td width=\"100%\" id=\"divGroup_");
                            sb.Append(intGroup.ToString());
                            sb.Append("\" style=\"display:none\">");
                            sb.Append(ShowGroup(_platformid, _group_num, _existing_filters + " AND projectid = " + drProject["projectid"].ToString()));
                            sb.Append("</td></tr>");
                        }
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
                        intGroup++;
                        sb.Append("<tr><td><a href=\"javascript:void(0);\" onclick=\"ShowDetail('imgGroup_");
                        sb.Append(intGroup.ToString());
                        sb.Append("','divGroup_");
                        sb.Append(intGroup.ToString());
                        sb.Append("');\"><img id=\"imgGroup_");
                        sb.Append(intGroup.ToString());
                        sb.Append("\" src=\"/images/biggerPlus.gif\" border=\"0\" align=\"absmiddle\" /></a></td><td width=\"100%\">");
                        sb.Append(drConfidence["name"].ToString());
                        sb.Append("</td></tr>");

                        if (oNext == null || oNext.ToString() == "" || oNext.ToString() == "0")
                        {
                            sb.Append("<tr><td></td><td width=\"100%\" id=\"divGroup_");
                            sb.Append(intGroup.ToString());
                            sb.Append("\" style=\"display:none\">");
                            sb.Append(LoadGroup(_platformid, _existing_filters + " AND confidenceid = " + drConfidence["id"].ToString()));
                            sb.Append("</td></tr>");
                        }
                        else
                        {
                            sb.Append("<tr><td></td><td width=\"100%\" id=\"divGroup_");
                            sb.Append(intGroup.ToString());
                            sb.Append("\" style=\"display:none\">");
                            sb.Append(ShowGroup(_platformid, _group_num, _existing_filters + " AND confidenceid = " + drConfidence["id"].ToString()));
                            sb.Append("</td></tr>");
                        }
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
                        intGroup++;
                        sb.Append("<tr><td><a href=\"javascript:void(0);\" onclick=\"ShowDetail('imgGroup_");
                        sb.Append(intGroup.ToString());
                        sb.Append("','divGroup_");
                        sb.Append(intGroup.ToString());
                        sb.Append("');\"><img id=\"imgGroup_");
                        sb.Append(intGroup.ToString());
                        sb.Append("\" src=\"/images/biggerPlus.gif\" border=\"0\" align=\"absmiddle\" /></a></td><td width=\"100%\">");
                        sb.Append(drLocation["fullname"].ToString());
                        sb.Append("</td></tr>");

                        if (oNext == null || oNext.ToString() == "" || oNext.ToString() == "0")
                        {
                            sb.Append("<tr><td></td><td width=\"100%\" id=\"divGroup_");
                            sb.Append(intGroup.ToString());
                            sb.Append("\" style=\"display:none\">");
                            sb.Append(LoadGroup(_platformid, _existing_filters + " AND addressid = " + drLocation["id"].ToString()));
                            sb.Append("</td></tr>");
                        }
                        else
                        {
                            sb.Append("<tr><td></td><td width=\"100%\" id=\"divGroup_");
                            sb.Append(intGroup.ToString());
                            sb.Append("\" style=\"display:none\">");
                            sb.Append(ShowGroup(_platformid, _group_num, _existing_filters + " AND addressid = " + drLocation["id"].ToString()));
                            sb.Append("</td></tr>");
                        }
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
                        intGroup++;
                        sb.Append("<tr><td><a href=\"javascript:void(0);\" onclick=\"ShowDetail('imgGroup_");
                        sb.Append(intGroup.ToString());
                        sb.Append("','divGroup_");
                        sb.Append(intGroup.ToString());
                        sb.Append("');\"><img id=\"imgGroup_");
                        sb.Append(intGroup.ToString());
                        sb.Append("\" src=\"/images/biggerPlus.gif\" border=\"0\" align=\"absmiddle\" /></a></td><td width=\"100%\">");
                        sb.Append(drClass["name"].ToString());
                        sb.Append("</td></tr>");

                        if (oNext == null || oNext.ToString() == "" || oNext.ToString() == "0")
                        {
                            sb.Append("<tr><td></td><td width=\"100%\" id=\"divGroup_");
                            sb.Append(intGroup.ToString());
                            sb.Append("\" style=\"display:none\">");
                            sb.Append(LoadGroup(_platformid, _existing_filters + " AND classid = " + drClass["id"].ToString()));
                            sb.Append("</td></tr>");
                        }
                        else
                        {
                            sb.Append("<tr><td></td><td width=\"100%\" id=\"divGroup_");
                            sb.Append(intGroup.ToString());
                            sb.Append("\" style=\"display:none\">");
                            sb.Append(ShowGroup(_platformid, _group_num, _existing_filters + " AND classid = " + drClass["id"].ToString()));
                            sb.Append("</td></tr>");
                        }
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
                        intGroup++;
                        sb.Append("<tr><td><a href=\"javascript:void(0);\" onclick=\"ShowDetail('imgGroup_");
                        sb.Append(intGroup.ToString());
                        sb.Append("','divGroup_");
                        sb.Append(intGroup.ToString());
                        sb.Append("');\"><img id=\"imgGroup_");
                        sb.Append(intGroup.ToString());
                        sb.Append("\" src=\"/images/biggerPlus.gif\" border=\"0\" align=\"absmiddle\" /></a></td><td width=\"100%\">");
                        sb.Append(drEnvironment["name"].ToString());
                        sb.Append("</td></tr>");

                        if (oNext == null || oNext.ToString() == "" || oNext.ToString() == "0")
                        {
                            sb.Append("<tr><td></td><td width=\"100%\" id=\"divGroup_");
                            sb.Append(intGroup.ToString());
                            sb.Append("\" style=\"display:none\">");
                            sb.Append(LoadGroup(_platformid, _existing_filters + " AND environmentid = " + drEnvironment["id"].ToString()));
                            sb.Append("</td></tr>");
                        }
                        else
                        {
                            sb.Append("<tr><td></td><td width=\"100%\" id=\"divGroup_");
                            sb.Append(intGroup.ToString());
                            sb.Append("\" style=\"display:none\">");
                            sb.Append(ShowGroup(_platformid, _group_num, _existing_filters + " AND environmentid = " + drEnvironment["id"].ToString()));
                            sb.Append("</td></tr>");
                        }
                    }
                    break;
            }
            if (sb.ToString() != "")
            {
                return "<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\" border=\"0\">" + sb.ToString() + "</table>";
            }
            else
            {
                return sb.ToString();
            }
        }
        private string LoadGroup(int _platformid, string _additional_filter)
        {
            string strGroup = "";
            DataSet dsSupply = oAsset.GetSupplyNetwork();
            DataTable dtSupply = dsSupply.Tables[0];
            int intPorts = 0;
            if (strFilter.StartsWith(" AND ") == true)
                strFilter = strFilter.Substring(5);
            if (strFilter.StartsWith("AND ") == true)
                strFilter = strFilter.Substring(4);
            DataRow[] drModels = dtSupply.Select(strFilter + _additional_filter);
            foreach (DataRow dr in drModels)
            {
                if (dr["available_ports"].ToString() != "")
                    intPorts += Int32.Parse(dr["available_ports"].ToString());
            }
            strGroup += "<tr class=\"" + (intPorts > 0 ? "greendefault" : (intPorts < 0 ? "reddefault" : "default")) + "\"><td nowrap><img src=\"/images/spacer.gif\" border=\"0\" width=\"10\" height=\"1\"/></td><td nowrap>Ports:</td><td width=\"100%\">" + oServiceRequest.GetStatusBarFill(((double.Parse(intPorts.ToString()) / double.Parse(intMax.ToString())) * 100.00), "95", false, "#CC3300") + "</td><td nowrap>" + intPorts.ToString() + "</td></tr>";
            if (strGroup != "")
                strGroup = "<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\" border=\"0\">" + strGroup + "</table>";
            return strGroup;
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
        private bool ColumnEqual(object A, object B)
        {
            if (A == DBNull.Value && B == DBNull.Value) //  both are DBNull.Value
                return true;
            if (A == DBNull.Value || B == DBNull.Value) //  only one is DBNull.Value
                return false;
            return (A.Equals(B));  // value type standard comparison
        }
    }
}