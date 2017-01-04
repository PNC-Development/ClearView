<%@ Page Language="C#" Debug="false" %>
<script runat="server">
    private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
    private int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
    private string strQuestion = ConfigurationManager.AppSettings["FORECAST_RAM_QUESTIONS"];
    private Platforms oPlatform;
    private Types oType;
    private ModelsProperties oModelsProperties;
    private ServiceRequests oServiceRequest;
    private Forecast oForecast;
    private Pages oPage;
    private int intProfile;
    private int intApplication = 0;
    private int intPage = 0;
    protected string strDemand = "";
    private int intMax = 50;
    private int intMaxRAM = 50;
    private int intMaxSpace = 50;
    protected string strFilter = "";
    private int intGroup = 0;
    protected string strViews = "";
    protected string strParameters = "";
    private void Page_Load()
    {
        intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
        oPlatform = new Platforms(intProfile, dsn);
        oType = new Types(intProfile, dsn);
        oModelsProperties = new ModelsProperties(intProfile, dsn);
        oServiceRequest = new ServiceRequests(intProfile, dsn);
        oForecast = new Forecast(intProfile, dsn);
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
            intMaxRAM = Int32.Parse(oPlatform.Get(intPlatform, "max_inventory2"));
            intMaxSpace = Int32.Parse(oPlatform.Get(intPlatform, "max_inventory3"));
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
    private void btnGo_Click(Object Sender, EventArgs e)
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
        Response.Redirect(oPage.GetFullLink(intPage) + "?id=" + Request.QueryString["id"] + "&div=D" + strQuery + "&start=" + txtStart.Text + "&end=" + txtEnd.Text + "&g1=" + ddlGroup1.SelectedItem.Value + "&g2=" + ddlGroup2.SelectedItem.Value + "&g3=" + ddlGroup3.SelectedItem.Value + "&g4=" + ddlGroup4.SelectedItem.Value + "&g5=" + ddlGroup5.SelectedItem.Value);
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
    private void LoadGroups(int _platformid)
    {
        DataSet dsDemand = oForecast.GetAnswersModel(_platformid);
        DataTable dtDemand = dsDemand.Tables[0];
        if (Request.QueryString["g1"] != null && Request.QueryString["g1"] != "" && Request.QueryString["g1"] != "0")
            strDemand += ShowGroup(_platformid, dtDemand, 1, "");
        else
            strDemand += LoadGroup(_platformid, dtDemand, "");
    }
    private string ShowGroup(int _platformid, DataTable _demand, int _group_num, string _existing_filters)
    {
        string strGroup = "";
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
                    strGroup += "<tr><td><a href=\"javascript:void(0);\" onclick=\"ShowDetail('imgGroup_" + intGroup.ToString() + "','divGroup_" + intGroup.ToString() + "');\"><img id=\"imgGroup_" + intGroup.ToString() + "\" src=\"/images/biggerPlus.gif\" border=\"0\" align=\"absmiddle\" /></a></td><td width=\"100%\">" + oProject.Get(intID, "name") + "</td></tr>";
                    if (oNext == null || oNext.ToString() == "" || oNext.ToString() == "0")
                        strGroup += "<tr><td></td><td width=\"100%\" id=\"divGroup_" + intGroup.ToString() + "\" style=\"display:none\">" + LoadGroup(_platformid, _demand, _existing_filters + " AND projectid = " + drProject["projectid"].ToString()) + "</td></tr>";
                    else
                        strGroup += "<tr><td></td><td width=\"100%\" id=\"divGroup_" + intGroup.ToString() + "\" style=\"display:none\">" + ShowGroup(_platformid, _demand, _group_num, _existing_filters + " AND projectid = " + drProject["projectid"].ToString()) + "</td></tr>";
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
                    strGroup += "<tr><td><a href=\"javascript:void(0);\" onclick=\"ShowDetail('imgGroup_" + intGroup.ToString() + "','divGroup_" + intGroup.ToString() + "');\"><img id=\"imgGroup_" + intGroup.ToString() + "\" src=\"/images/biggerPlus.gif\" border=\"0\" align=\"absmiddle\" /></a></td><td width=\"100%\">" + drConfidence["name"].ToString() + "</td></tr>";
                    if (oNext == null || oNext.ToString() == "" || oNext.ToString() == "0")
                        strGroup += "<tr><td></td><td width=\"100%\" id=\"divGroup_" + intGroup.ToString() + "\" style=\"display:none\">" + LoadGroup(_platformid, _demand, _existing_filters + " AND confidenceid = " + drConfidence["id"].ToString()) + "</td></tr>";
                    else
                        strGroup += "<tr><td></td><td width=\"100%\" id=\"divGroup_" + intGroup.ToString() + "\" style=\"display:none\">" + ShowGroup(_platformid, _demand, _group_num, _existing_filters + " AND confidenceid = " + drConfidence["id"].ToString()) + "</td></tr>";
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
                    strGroup += "<tr><td><a href=\"javascript:void(0);\" onclick=\"ShowDetail('imgGroup_" + intGroup.ToString() + "','divGroup_" + intGroup.ToString() + "');\"><img id=\"imgGroup_" + intGroup.ToString() + "\" src=\"/images/biggerPlus.gif\" border=\"0\" align=\"absmiddle\" /></a></td><td width=\"100%\">" + drLocation["fullname"].ToString() + "</td></tr>";
                    if (oNext == null || oNext.ToString() == "" || oNext.ToString() == "0")
                        strGroup += "<tr><td></td><td width=\"100%\" id=\"divGroup_" + intGroup.ToString() + "\" style=\"display:none\">" + LoadGroup(_platformid, _demand, _existing_filters + " AND addressid = " + drLocation["id"].ToString()) + "</td></tr>";
                    else
                        strGroup += "<tr><td></td><td width=\"100%\" id=\"divGroup_" + intGroup.ToString() + "\" style=\"display:none\">" + ShowGroup(_platformid, _demand, _group_num, _existing_filters + " AND addressid = " + drLocation["id"].ToString()) + "</td></tr>";
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
                    strGroup += "<tr><td><a href=\"javascript:void(0);\" onclick=\"ShowDetail('imgGroup_" + intGroup.ToString() + "','divGroup_" + intGroup.ToString() + "');\"><img id=\"imgGroup_" + intGroup.ToString() + "\" src=\"/images/biggerPlus.gif\" border=\"0\" align=\"absmiddle\" /></a></td><td width=\"100%\">" + drClass["name"].ToString() + "</td></tr>";
                    if (oNext == null || oNext.ToString() == "" || oNext.ToString() == "0")
                        strGroup += "<tr><td></td><td width=\"100%\" id=\"divGroup_" + intGroup.ToString() + "\" style=\"display:none\">" + LoadGroup(_platformid, _demand, _existing_filters + " AND classid = " + drClass["id"].ToString()) + "</td></tr>";
                    else
                        strGroup += "<tr><td></td><td width=\"100%\" id=\"divGroup_" + intGroup.ToString() + "\" style=\"display:none\">" + ShowGroup(_platformid, _demand, _group_num, _existing_filters + " AND classid = " + drClass["id"].ToString()) + "</td></tr>";
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
                    strGroup += "<tr><td><a href=\"javascript:void(0);\" onclick=\"ShowDetail('imgGroup_" + intGroup.ToString() + "','divGroup_" + intGroup.ToString() + "');\"><img id=\"imgGroup_" + intGroup.ToString() + "\" src=\"/images/biggerPlus.gif\" border=\"0\" align=\"absmiddle\" /></a></td><td width=\"100%\">" + drEnvironment["name"].ToString() + "</td></tr>";
                    if (oNext == null || oNext.ToString() == "" || oNext.ToString() == "0")
                        strGroup += "<tr><td></td><td width=\"100%\" id=\"divGroup_" + intGroup.ToString() + "\" style=\"display:none\">" + LoadGroup(_platformid, _demand, _existing_filters + " AND environmentid = " + drEnvironment["id"].ToString()) + "</td></tr>";
                    else
                        strGroup += "<tr><td></td><td width=\"100%\" id=\"divGroup_" + intGroup.ToString() + "\" style=\"display:none\">" + ShowGroup(_platformid, _demand, _group_num, _existing_filters + " AND environmentid = " + drEnvironment["id"].ToString()) + "</td></tr>";
                }
                break;
        }
        if (strGroup != "")
            strGroup = "<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\" border=\"0\">" + strGroup + "</table>";
        return strGroup;
    }
    private string LoadGroup(int _platformid, DataTable _demand, string _additional_filter)
    {
        string strGroup = "";
        Functions oFunction = new Functions(intProfile, dsn, intEnvironment);
        DataSet dsTypes = oType.Gets(_platformid, 1);
        foreach (DataRow drType in dsTypes.Tables[0].Rows)
        {
            bool boolPhysical = false;
            bool boolBlade = false;
            bool boolOther = false;
            int intOldHost = -1;
            DataSet dsModels = oModelsProperties.GetTypes(1, Int32.Parse(drType["id"].ToString()), 1);
            foreach (DataRow drModel in dsModels.Tables[0].Rows)
            {
                boolOther = !boolOther;
                int intBlade = Int32.Parse(drModel["blade"].ToString());
                int intHost = -1;
                if (drModel["hostid"].ToString() != "")
                    intHost = Int32.Parse(drModel["hostid"].ToString());
                string strType = "Unknown";
                if (intBlade == 1)
                {
                    strType = "Blade";
                    if (boolBlade == false)
                    {
                        if (strGroup != "")
                            strGroup += "<tr><td colspan=\"4\"><span style=\"width:100%;border-bottom:1 dotted #999999;\"/></td></tr>";
                        strGroup += "<tr><td colspan=\"4\" class=\"bold\">" + drType["name"].ToString() + " | " + strType + "</td></tr>";
                        boolBlade = true;
                    }
                }
                else if (intHost == 0)
                {
                    strType = "Physical";
                    if (boolPhysical == false)
                    {
                        if (strGroup != "")
                            strGroup += "<tr><td colspan=\"4\"><p>&nbsp;</p></td></tr>";
                        strGroup += "<tr><td colspan=\"4\" class=\"bold\">" + drType["name"].ToString() + " | " + strType + "</td></tr>";
                        boolPhysical = true;
                    }
                }
                else if (intOldHost != intHost)
                {
                    strType = drModel["host"].ToString();
                    if (intHost > 0)
                    {
                        if (strGroup != "")
                            strGroup += "<tr><td colspan=\"4\"><span style=\"width:100%;border-bottom:1 dotted #999999;\"/></td></tr>";
                        strGroup += "<tr><td colspan=\"4\" class=\"bold\">" + drType["name"].ToString() + " | " + strType + "</td></tr>";
                        intOldHost = intHost;
                    }
                }
                int intModel = Int32.Parse(drModel["id"].ToString());
                DataRow[] drModels = _demand.Select("model = " + intModel.ToString() + strFilter + _additional_filter);
                int intDemand = 0;
                if (intHost == 0)
                {
                    foreach (DataRow dr in drModels)
                    {
                        intDemand += Int32.Parse(dr["quantity"].ToString());
                        intDemand += Int32.Parse(dr["recovery_number"].ToString());
                    }
                    strGroup += "<tr" + (boolOther ? " bgcolor=\"F6F6F6\"" : "") + " class=\"" + (intDemand > 0 ? "greendefault" : (intDemand < 0 ? "reddefault" : "default")) + "\"><td nowrap><img src=\"/images/spacer.gif\" border=\"0\" width=\"10\" height=\"1\"/></td><td nowrap>" + (intDemand == 0 ? drModel["name"].ToString() : "<a href=\"javascript:void(0);\" onclick=\"OpenWindow('INVENTORY_DEMAND','?model=" + drModel["id"].ToString() + "&filters=" + oFunction.encryptQueryString(strFilter + _additional_filter) + "');\">" + drModel["name"].ToString() + "</a>") + ":</td><td width=\"100%\">" + oServiceRequest.GetStatusBarFill(((double.Parse(intDemand.ToString()) / double.Parse(intMax.ToString())) * 100.00), "95", false, "#CC3300") + "</td><td nowrap>" + intDemand.ToString() + "</td></tr>";
                }
                else
                {
                    // RAM
                    double dblRAM = 0.0;
                    string[] strQuestions;
                    char[] strSplit = { ';' };
                    strQuestions = strQuestion.Split(strSplit);
                    for (int ii = 0; ii < strQuestions.Length; ii++)
                    {
                        if (strQuestions[ii].Trim() != "")
                        {
                            int intQuestion = Int32.Parse(strQuestions[ii].Trim());
                            foreach (DataRow drCount in drModels)
                            {
                                int intAnswer = Int32.Parse(drCount["id"].ToString());
                                DataSet dsAnswers = oForecast.GetAnswerPlatform(intAnswer, intQuestion);
                                foreach (DataRow drAnswer in dsAnswers.Tables[0].Rows)
                                {
                                    int intResponse = Int32.Parse(drAnswer["responseid"].ToString());
                                    dblRAM += double.Parse(oForecast.GetResponse(intResponse, "response"));
                                }
                            }
                        }
                    }
                    strGroup += "<tr" + (boolOther ? " bgcolor=\"F6F6F6\"" : "") + " class=\"" + (dblRAM > 0.00 ? "greendefault" : (dblRAM < 0.00 ? "reddefault" : "default")) + "\"><td nowrap><img src=\"/images/spacer.gif\" border=\"0\" width=\"10\" height=\"1\"/></td><td nowrap>RAM:</td><td width=\"100%\">" + oServiceRequest.GetStatusBarFill(((dblRAM / double.Parse(intMaxRAM.ToString())) * 100.00), "95", false, "#CC3300") + "</td><td nowrap>" + dblRAM.ToString() + " GB</td></tr>";
                    // Disk Space
                    double dblHDD = 0.0;
                    foreach (DataRow drCount in drModels)
                    {
                        if (drCount["high_total"].ToString() != "")
                            dblHDD += double.Parse(drCount["high_total"].ToString());
                        if (drCount["standard_total"].ToString() != "")
                            dblHDD += double.Parse(drCount["standard_total"].ToString());
                        if (drCount["low_total"].ToString() != "")
                            dblHDD += double.Parse(drCount["low_total"].ToString());
                        if (drCount["high_test"].ToString() != "")
                            dblHDD += double.Parse(drCount["high_test"].ToString());
                        if (drCount["standard_test"].ToString() != "")
                            dblHDD += double.Parse(drCount["standard_test"].ToString());
                        if (drCount["low_test"].ToString() != "")
                            dblHDD += double.Parse(drCount["low_test"].ToString());
                    }
                    strGroup += "<tr" + (boolOther ? " bgcolor=\"F6F6F6\"" : "") + " class=\"" + (dblHDD > 0.00 ? "greendefault" : (dblHDD < 0.00 ? "reddefault" : "default")) + "\"><td nowrap><img src=\"/images/spacer.gif\" border=\"0\" width=\"10\" height=\"1\"/></td><td nowrap>Disk Space:</td><td width=\"100%\">" + oServiceRequest.GetStatusBarFill(((dblHDD / double.Parse(intMaxSpace.ToString())) * 100.00), "95", false, "#CC3300") + "</td><td nowrap>" + dblHDD.ToString() + " GB</td></tr>";
                }
            }
        }
        if (strGroup != "")
            strGroup = "<table width=\"100%\" cellpadding=\"4\" cellspacing=\"0\" border=\"0\">" + strGroup + "</table>";
        return strGroup;
    }
    public DataTable SelectDistinct(string TableName, DataTable SourceTable, string FieldName)
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
</script>
<html>
<head>
<title>Inventory Manager</title>
<link rel="stylesheet" type="text/css" href="/css/default.css" />
<script src="/javascript/default.js"type="text/javascript"></script>
<script type="text/javascript">
</script>
</head>
<body leftmargin="0" topmargin="0">
<form id="Form1" runat="server">
<table width="100%" cellpadding="0" cellspacing="2" border="0">
    <tr>
        <td id="tdSideBar" valign="top" nowrap style="background-color:#F6F6F6">
            <table width="100%" cellpadding="3" cellspacing="2" border="0">
                <tr>
                    <td colspan="3">
                        <asp:Panel ID="panParameters" runat="server" Visible="false">
                            <table width="100%" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #404040" bgcolor="#FFFFFF">
                                <tr>
                                    <td nowrap><img src="/images/funnel.gif" border="0" align="absmiddle" /></td>
                                    <td width="100%" class="header">Applied Filters</td>
                                </tr>
                                <tr>
                                    <td colspan="2"><%=strParameters %></td>
                                </tr>
                            </table>
                            <br />
                            
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td nowrap><img src="/images/arrow_black_right.gif" border="0" /></td>
                    <td class="bigger" nowrap><b>Filters</b></td>
                    <td align="right"><asp:Button ID="btnGo2" runat="server" CssClass="default" Width="75" Text="Apply" OnClick="btnGo_Click" /></td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td valign="top">Projects:<br /><br /><asp:Button id="btnProjects" runat="server" Text="&lt; &gt;" CssClass="default" ToolTip="Maximize Listing" /> <asp:Button id="btnProjectsClear" runat="server" Text="X" Width="25" CssClass="default" ToolTip="Clear Selection(s)" /></td>
                    <td><asp:ListBox ID="lstProjects" runat="server" CssClass="smalldefault" Width="200" SelectionMode="Multiple" /> </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td valign="top">Confidence:<br /><br /><asp:Button id="btnConfidences" runat="server" Text="&lt; &gt;" CssClass="default" ToolTip="Maximize Listing" /> <asp:Button id="btnConfidencesClear" runat="server" Text="X" Width="25" CssClass="default" ToolTip="Clear Selection(s)" /></td>
                    <td><asp:ListBox ID="lstConfidences" runat="server" CssClass="smalldefault" Width="200" SelectionMode="Multiple" /> </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td valign="top">Location:<br /><br /><asp:Button id="btnLocations" runat="server" Text="&lt; &gt;" CssClass="default" ToolTip="Maximize Listing" /> <asp:Button id="btnLocationsClear" runat="server" Text="X" Width="25" CssClass="default" ToolTip="Clear Selection(s)" /></td>
                    <td><asp:ListBox ID="lstLocations" runat="server" CssClass="smalldefault" Width="200" SelectionMode="Multiple" /> </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td valign="top">Class:<br /><br /><asp:Button id="btnClasses" runat="server" Text="&lt; &gt;" CssClass="default" ToolTip="Maximize Listing" /> <asp:Button id="btnClassesClear" runat="server" Text="X" Width="25" CssClass="default" ToolTip="Clear Selection(s)" /></td>
                    <td><asp:ListBox ID="lstClasses" runat="server" CssClass="smalldefault" Width="200" SelectionMode="Multiple" /> </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td valign="top">Environment:<br /><br /><asp:Button id="btnEnvironments" runat="server" Text="&lt; &gt;" CssClass="default" ToolTip="Maximize Listing" /> <asp:Button id="btnEnvironmentsClear" runat="server" Text="X" Width="25" CssClass="default" ToolTip="Clear Selection(s)" /></td>
                    <td><asp:ListBox ID="lstEnvironments" runat="server" CssClass="smalldefault" Width="200" SelectionMode="Multiple" /> </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>Date Range:</td>
                    <td><asp:TextBox ID="txtStart" runat="server" CssClass="smalldefault" Width="70" MaxLength="10" /> <asp:ImageButton ID="imgStart" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /> - <asp:TextBox ID="txtEnd" runat="server" CssClass="smalldefault" Width="70" MaxLength="10" /> <asp:ImageButton ID="imgEnd" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
                </tr>
                <tr>
                    <td colspan="3"><img src="/images/spacer.gif" border="0" width="1" height="1" /></td>
                </tr>
                <tr>
                    <td nowrap><img src="/images/arrow_black_right.gif" border="0" /></td>
                    <td colspan="2" class="bigger" nowrap><b>Groups</b></td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>Group 1:</td>
                    <td><asp:DropDownList ID="ddlGroup1" runat="server" CssClass="smalldefault" Width="200" /> </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>Group 2:</td>
                    <td><asp:DropDownList ID="ddlGroup2" runat="server" CssClass="smalldefault" Width="200" /> </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>Group 3:</td>
                    <td><asp:DropDownList ID="ddlGroup3" runat="server" CssClass="smalldefault" Width="200" /> </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>Group 4:</td>
                    <td><asp:DropDownList ID="ddlGroup4" runat="server" CssClass="smalldefault" Width="200" /> </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>Group 5:</td>
                    <td><asp:DropDownList ID="ddlGroup5" runat="server" CssClass="smalldefault" Width="200" /> </td>
                </tr>
                <tr>
                    <td colspan="3"><img src="/images/spacer.gif" border="0" width="1" height="1" /></td>
                </tr>
                <tr>
                    <td colspan="3" align="right"><asp:Button ID="btnGo1" runat="server" CssClass="default" Width="75" Text="Apply" OnClick="btnGo_Click" /></td>
                </tr>
                <tr>
                    <td nowrap><img src="/images/arrow_black_right.gif" border="0" /></td>
                    <td colspan="2" class="bigger" nowrap><b>Saved Views</b></td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td colspan="2"><%=strViews %></td>
                </tr>
                <tr>
                    <td colspan="3"><img src="/images/spacer.gif" border="0" width="1" height="1" /></td>
                </tr>
            </table>
        </td>
        <td valign="top" style="background-color:#C6C6C6;border-right:1px solid #999999;width:6px;padding-top:350px;">
            <a href="javascript:void(0)" onclick="SideBar(this);"><img src="/images/sidebar_collapse.gif" border="0" alt="Collapse Sidebar"></a>
        </td>
        <td valign="top" width="100%" height="100%">
            <table width="100%" cellpadding="3" cellspacing="2" border="0">
                <tr>
                    <td><%=strDemand %></td>
                </tr>
            </table>
        </td>
    </tr>
</table>
<asp:HiddenField ID="hdnEnvironment" runat="server" />
</form>
</body>
</html>