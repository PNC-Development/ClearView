<%@ Control Language="C#" %>
<script runat="server">
    private DataSet ds;
    private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
    private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
    private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
    private string dsnServiceEditor = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ServiceEditorDSN"]].ConnectionString;
    private int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
    private string strVirtual = ConfigurationManager.AppSettings["VirtualGatekeeper"];
    private int intAssignPage = Int32.Parse(ConfigurationManager.AppSettings["AssignWorkload"]);
    private int intResourceRequestApprove = Int32.Parse(ConfigurationManager.AppSettings["ResourceRequestApprove"]);
    private int intViewPage = Int32.Parse(ConfigurationManager.AppSettings["ViewWorkload"]);
    private int intMyWork = Int32.Parse(ConfigurationManager.AppSettings["MyWork"]);
    private string strBCC = ConfigurationManager.AppSettings["WorkflowBCC"];
    private int intProfile;
    private Projects oProject;
    private Functions oFunction;
    private Users oUser;
    private Pages oPage;
    private ResourceRequest oResourceRequest;
    private RequestItems oRequestItem;
    private Requests oRequest;
    private Services oService;
    private ServiceRequests oServiceRequest;
    private RequestFields oRequestField;
    private Applications oApplication;
    private ServiceDetails oServiceDetail;
    private Delegates oDelegate;
    private Documents oDocument;
    private OnDemandTasks oOnDemandTasks;
    private int intApplication = 0;
    private int intPage = 0;
    private int intProject = 0;
    private bool boolDetails = false;
    private bool boolExecution = false;
    private bool boolChange = false;
    private bool boolDocuments = false;
    private int intRequest = 0;
    private int intItem = 0;
    private int intService = 0;
    private int intNumber = 0;
    private bool boolCheckboxes = false;
    private string strCheckboxes = "";
    private bool boolJoined = false;
    private void Page_Load()
    {
        intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
        oProject = new Projects(intProfile, dsn);
        oFunction = new Functions(intProfile, dsn, intEnvironment);
        oUser = new Users(intProfile, dsn);
        oPage = new Pages(intProfile, dsn);
        oResourceRequest = new ResourceRequest(intProfile, dsn);
        oRequestItem = new RequestItems(intProfile, dsn);
        oRequest = new Requests(intProfile, dsn);
        oService = new Services(intProfile, dsn);
        oServiceRequest = new ServiceRequests(intProfile, dsn);
        oRequestField = new RequestFields(intProfile, dsn);
        oApplication = new Applications(intProfile, dsn);
        oServiceDetail = new ServiceDetails(intProfile, dsn);
        oDelegate = new Delegates(intProfile, dsn);
        oDocument = new Documents(intProfile, dsn);
        oOnDemandTasks = new OnDemandTasks(intProfile, dsn);
        if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
            intApplication = Int32.Parse(Request.QueryString["applicationid"]);
        if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
            intPage = Int32.Parse(Request.QueryString["pageid"]);
        if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
            intApplication = Int32.Parse(Request.Cookies["application"].Value);
        if (Request.QueryString["rrid"] != null && Request.QueryString["rrid"] != "")
        {
            // Start Workflow Change
            lblResourceWorkflow.Text = Request.QueryString["rrid"];
            int intResourceWorkflow = Int32.Parse(Request.QueryString["rrid"]);
            int intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);
            ds = oResourceRequest.Get(intResourceParent);
            // End Workflow Change
            intRequest = Int32.Parse(ds.Tables[0].Rows[0]["requestid"].ToString());
            lblRequestedBy.Text = oUser.GetFullName(oRequest.GetUser(intRequest));
            lblRequestedOn.Text = DateTime.Parse(oRequest.Get(intRequest, "created")).ToString();
            lblDescription.Text = oRequest.Get(intRequest, "description");
            if (lblDescription.Text == "")
                lblDescription.Text = "<i>No information</i>";
            intItem = Int32.Parse(ds.Tables[0].Rows[0]["itemid"].ToString());
            intNumber = Int32.Parse(ds.Tables[0].Rows[0]["number"].ToString());
            // Start Workflow Change
            bool boolComplete = (oResourceRequest.GetWorkflow(intResourceWorkflow, "status") == "3");
            int intUser = Int32.Parse(oResourceRequest.GetWorkflow(intResourceWorkflow, "userid"));
            txtCustom.Text = oResourceRequest.GetWorkflow(intResourceWorkflow, "name");
            double dblAllocated = double.Parse(oResourceRequest.GetWorkflow(intResourceWorkflow, "allocated"));
            boolJoined = (oResourceRequest.GetWorkflow(intResourceWorkflow, "joined") == "1");
            // End Workflow Change
            intService = Int32.Parse(ds.Tables[0].Rows[0]["serviceid"].ToString());
            lblService.Text = oService.Get(intService, "name");
            int intApp = oRequestItem.GetItemApplication(intItem);

            if (Request.QueryString["save"] != null && Request.QueryString["save"] != "")
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "saved", "<script type=\"text/javascript\">alert('Information Saved Successfully');<" + "/" + "script>");
            if (Request.QueryString["status"] != null && Request.QueryString["status"] != "")
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "statusd", "<script type=\"text/javascript\">alert('Weekly Status has been Added');<" + "/" + "script>");
            if (!IsPostBack)
            {
                double dblUsed = oResourceRequest.GetWorkflowUsed(intResourceWorkflow);
                lblUpdated.Text = oResourceRequest.GetUpdated(intResourceParent);
                if (dblAllocated == dblUsed)
                {
                    if (boolComplete == false)
                    {
                        oFunction.ConfigureToolButton(btnComplete, "/images/tool_complete");
                        btnComplete.Attributes.Add("onclick", "return confirm('Are you sure you want to mark this as completed and remove it from your workload?');");
                    }
                    else
                    {
                        oFunction.ConfigureToolButton(btnComplete, "/images/tool_complete");
                        btnComplete.Attributes.Add("onclick", "alert('This task has already been marked COMPLETE. You can close this window.');return false;");
                    }
                }
                else
                {
                    btnComplete.ImageUrl = "/images/tool_complete_dbl.gif";
                    btnComplete.Enabled = false;
                }
                if (oService.Get(intService, "sla") != "")
                {
                    oFunction.ConfigureToolButton(btnSLA, "/images/tool_sla");
                    int intDays = oResourceRequest.GetSLA(intResourceParent);
                    if (intDays < 1)
                        btnSLA.Style["border"] = "solid 2px #FF0000";
                    else if (intDays < 3)
                        btnSLA.Style["border"] = "solid 2px #FF9999";
                    btnSLA.Attributes.Add("onclick", "return OpenWindow('RESOURCE_REQUEST_SLA','?rrid=" + intResourceParent.ToString() + "');");
                }
                else
                {
                    btnSLA.ImageUrl = "/images/tool_sla_dbl.gif";
                    btnSLA.Enabled = false;
                }
                oFunction.ConfigureToolButton(btnEmail, "/images/tool_email");
                btnEmail.Attributes.Add("onclick", "return OpenWindow('RESOURCE_REQUEST_EMAIL','?rrid=" + intResourceWorkflow.ToString() + "&type=GENERIC');");
                dblUsed = (dblUsed / dblAllocated) * 100;
                strCheckboxes = oServiceDetail.LoadCheckboxes(intRequest, intItem, intNumber, intResourceWorkflow, intService);
                if (oService.Get(intService, "tasks") != "1" || strCheckboxes == "")
                {
                    if (oService.Get(intService, "no_slider") == "1")
                    {
                        panNoSlider.Visible = true;
                        btnComplete.ImageUrl = "/images/tool_complete.gif";
                        btnComplete.Enabled = true;
                        btnComplete.Attributes.Add("onclick", "return confirm('Are you sure you want to mark this as completed and remove it from your workload?');");
                        btnComplete.CommandArgument = "FAST";
                    }
                    else
                    {
                        panSlider.Visible = true;
                        sldHours._StartPercent = dblUsed.ToString();
                        sldHours._TotalHours = dblAllocated.ToString();
                    }
                }
                else
                    panCheckboxes.Visible = true;
                intProject = oRequest.GetProjectNumber(intRequest);
                hdnTab.Value = "D";
                panWorkload.Visible = true;
                LoadStatus(intResourceWorkflow);
                LoadChange(intResourceWorkflow);
                LoadInformation(intResourceWorkflow);
                chkDescription.Checked = (Request.QueryString["doc"] != null);
                lblDocuments.Text = oDocument.GetDocuments_Request(intRequest, 0, Request.PhysicalApplicationPath, 1, (Request.QueryString["doc"] != null));
                // GetDocuments(string _physical, int _projectid, int _requestid, int _userid, int _security, bool _show_description, bool _mine)
                //lblDocuments.Text = oDocument.GetDocuments(Request.PhysicalApplicationPath, 0, intRequest, 0, 1, (Request.QueryString["doc"] != null), false);

                btnDenied.Attributes.Add("onclick", "return CloseWindow();");
                oFunction.ConfigureToolButton(btnSave, "/images/tool_save");
                oFunction.ConfigureToolButton(btnPrint, "/images/tool_print");
                btnPrint.Attributes.Add("onclick", "return PrintWindow();");
                oFunction.ConfigureToolButton(btnClose, "/images/tool_close");
                btnClose.Attributes.Add("onclick", "return ExitWindow();");
                btnSave.Attributes.Add("onclick", "return ValidateStatus('" + ddlStatus.ClientID + "','" + txtComments.ClientID + "');");
                btnChange.Attributes.Add("onclick", "return ValidateText('" + txtNumber.ClientID + "','Please enter a change control number')" +
                    " && ValidateDate('" + txtDate.ClientID + "','Please enter a valid implementation date')" +
                    " && ValidateTime('" + txtTime.ClientID + "','Please enter a valid implementation time')" +
                    ";");
                imgDate.Attributes.Add("onclick", "return ShowCalendar('" + txtDate.ClientID + "');");
                // 6/1/2009 - Load ReadOnly View
                if (oResourceRequest.CanUpdate(intProfile, intResourceWorkflow, false) == false)
                {
                    oFunction.ConfigureToolButtonRO(btnSave, btnComplete);
                    //panDenied.Visible = true;
                }
            }
        }
        else
            panDenied.Visible = true;
    }
    private void LoadStatus(int _resourceid)
    {
        DataSet dsStatus = oResourceRequest.GetStatuss(_resourceid);
        rptStatus.DataSource = dsStatus;
        rptStatus.DataBind();
        lblNoStatus.Visible = (rptStatus.Items.Count == 0);
        double dblTotalStatus = 0.00;
        foreach (RepeaterItem ri in rptStatus.Items)
        {
            Label _status = (Label)ri.FindControl("lblStatus");
            double dblStatus = double.Parse(_status.Text);
            if (dblTotalStatus == 0.00)
                dblTotalStatus = dblStatus;
            _status.Text = oResourceRequest.GetStatus(dblStatus, 50, 15);
        }
        lblStatus.Text = oResourceRequest.GetStatus(dblTotalStatus, 50, 15);
    }
    private void LoadChange(int _resourceid)
    {
        DataSet dsChange = oResourceRequest.GetChangeControls(_resourceid);
        rptChange.DataSource = dsChange;
        rptChange.DataBind();
        lblNoChange.Visible = (rptChange.Items.Count == 0);
        foreach (RepeaterItem ri in rptChange.Items)
        {
            LinkButton _delete = (LinkButton)ri.FindControl("btnDeleteChange");
            _delete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this change control?');");
        }
    }
    private void LoadInformation(int _request)
    {
        lblView.Text = oRequestField.GetBodyWorkflow(_request, dsnServiceEditor, intEnvironment, dsnAsset, dsnIP);
        if (intProject > 0)
        {
            lblName.Text = oProject.Get(intProject, "name");
            lblNumber.Text = oProject.Get(intProject, "number");
            lblType.Text = "Project";
        }
        else
        {
            lblName.Text = oResourceRequest.GetWorkflow(_request, "name");
            //lblName.Text = oResourceRequest.GetWorkflow(_request, "name");
            lblNumber.Text = "CVT" + intRequest.ToString();
            lblType.Text = "Task";
        }
        if (Request.QueryString["div"] != null)
        {
            switch (Request.QueryString["div"])
            {
                case "E":
                    boolExecution = true;
                    break;
                case "C":
                    boolChange = true;
                    break;
                case "D":
                    boolDocuments = true;
                    break;
            }
        }
        if (boolDetails == false && boolExecution == false && boolChange == false && boolDocuments == false)
            boolDetails = true;
    }
    private void btnStatus_Click(Object Sender, EventArgs e)
    {
        int intResourceWorkflow = Int32.Parse(lblResourceWorkflow.Text);
        int intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);
        oResourceRequest.AddStatus(intResourceWorkflow, Int32.Parse(ddlStatus.SelectedItem.Value), txtComments.Text);
        Response.Redirect(Request.Path + "?rrid=" + intResourceWorkflow.ToString() + "&div=E&status=true");
    }
    private void btnChange_Click(Object Sender, EventArgs e)
    {
        int intResourceWorkflow = Int32.Parse(lblResourceWorkflow.Text);
        int intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);
        oResourceRequest.AddChangeControl(intResourceWorkflow, txtNumber.Text, DateTime.Parse(txtDate.Text + " " + txtTime.Text), txtChange.Text);
        Response.Redirect(Request.Path + "?rrid=" + intResourceWorkflow.ToString() + "&div=C&save=true");
    }
    private void btnDeleteChange_Click(Object Sender, EventArgs e)
    {
        LinkButton oButton = (LinkButton)Sender;
        oResourceRequest.DeleteChangeControl(Int32.Parse(oButton.CommandArgument));
        Response.Redirect(Request.Path + "?rrid=" + lblResourceWorkflow.Text + "&div=C");
    }
    private void chkDescription_Change(Object Sender, EventArgs e)
    {
        if (chkDescription.Checked == true)
            Response.Redirect(Request.Path + "?rrid=" + Request.QueryString["rrid"] + "&doc=true&div=D");
        else
            Response.Redirect(Request.Path + "?rrid=" + Request.QueryString["rrid"] + "&div=D");
    }
    private void btnSave_Click(Object Sender, EventArgs e)
    {
        int intResourceWorkflow = Int32.Parse(lblResourceWorkflow.Text);
        int intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);
        if (ddlStatus.SelectedIndex > -1 && txtComments.Text.Trim() != "")
            oResourceRequest.AddStatus(intResourceWorkflow, Int32.Parse(ddlStatus.SelectedItem.Value), txtComments.Text);
        if (panNoSlider.Visible == false)
        {
            double dblHours = 0.00;
            if (panSlider.Visible == true)
            {
                if (Request.Form["hdnHours"] != null && Request.Form["hdnHours"] != "")
                    dblHours = double.Parse(Request.Form["hdnHours"]);
                double dblUsed = oResourceRequest.GetWorkflowUsed(intResourceWorkflow);
                dblHours = (dblHours - dblUsed);
                if (dblHours > 0.00)
                    oResourceRequest.UpdateWorkflowHours(intResourceWorkflow, dblHours);
            }
            else
            {
                oServiceDetail.UpdateCheckboxes(Request, intResourceWorkflow, intRequest, intItem, intNumber);
                double dblAllocated = oResourceRequest.GetDetailsHoursUsed(intRequest, intItem, intNumber, intResourceWorkflow, false);
                oResourceRequest.UpdateWorkflowAllocated(intResourceWorkflow, dblAllocated);
            }
        }
        oResourceRequest.UpdateWorkflowName(intResourceWorkflow, txtCustom.Text, intProfile);
        Response.Redirect(Request.Path + "?rrid=" + intResourceWorkflow.ToString() + "&div=E&save=true");
    }
    private void btnComplete_Click(Object Sender, EventArgs e)
    {
        int intResourceWorkflow = Int32.Parse(lblResourceWorkflow.Text);
        int intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);
        if (panNoSlider.Visible == true)
        {
            double dblAllocated = double.Parse(oResourceRequest.GetWorkflow(intResourceWorkflow, "allocated"));
            oResourceRequest.UpdateWorkflowHoursOverwrite(intResourceWorkflow, dblAllocated);
        }
        int intAnswer = 0;
        int intModel = 0;
        DataSet ds = oOnDemandTasks.GetServerOther(intRequest, intService, intNumber);
        if (ds.Tables[0].Rows.Count > 0)
        {
            intAnswer = Int32.Parse(ds.Tables[0].Rows[0]["answerid"].ToString());
            intModel = Int32.Parse(ds.Tables[0].Rows[0]["modelid"].ToString());
        }
        //Generate Security Task
        Forecast oForecast = new Forecast(0, dsn);
        bool boolCluster = false;
        if (oForecast.IsHACluster(intAnswer) == true)
            boolCluster = true;
        if (boolCluster == true)
        {
            // send Cluster
            int intImplementorUser = 0;
            int intImplementorResource = 0;
            DataSet dsImplementor = oOnDemandTasks.GetPending(intAnswer);
            if (dsImplementor.Tables[0].Rows.Count > 0)
            {
                intImplementorResource = Int32.Parse(dsImplementor.Tables[0].Rows[0]["resourceid"].ToString());
                intImplementorUser = Int32.Parse(oResourceRequest.GetWorkflow(intImplementorResource, "userid"));
            }
            if (intImplementorUser > 0)
            {
                Variables oVariable = new Variables(intEnvironment);
                int intServiceId = Int32.Parse(ConfigurationManager.AppSettings["SERVICEID_PNC_SECURITY"]);
                int intServiceItemId = oService.GetItemId(intServiceId);
                int intServiceNumber = 1;
                oOnDemandTasks.AddServerOther(intRequest, intServiceId, intServiceNumber, intAnswer, intModel);
                double dblServiceHours = oServiceDetail.GetHours(intServiceId, 1);
                int intResource = oServiceRequest.AddRequest(intRequest, intServiceItemId, intServiceId, 1, dblServiceHours, 2, intServiceNumber, dsnServiceEditor);
                // Comment the following...
                //oServiceRequest.NotifyTeamLead(intServiceItemId, intResource, intAssignPage, intViewPage, intEnvironment, strBCC, "", dsnServiceEditor, dsnAsset, dsnIP);
                // Include the following...
                int intResourceWorkflowNEW = oResourceRequest.AddWorkflow(intResource, 0, oResourceRequest.Get(intResource, "name"), intImplementorUser, Int32.Parse(oResourceRequest.Get(intResource, "devices")), double.Parse(oResourceRequest.Get(intResource, "allocated")), 2, 1);
                string strService = oService.GetName(intService);
                if (intService == 0)
                    strService = oRequestItem.GetItemName(intItem);
                int intApp = oRequestItem.GetItemApplication(intItem);
                string strDefault = oUser.GetApplicationUrl(intImplementorUser, intViewPage);
                if (strDefault == "")
                    oFunction.SendEmail("Request Assignment: " + strService, oUser.GetName(intImplementorUser), "", strBCC, "Request Assignment: " + strService, "<p><b>The following request has been automatically assigned to you</b><p><p>" + oResourceRequest.GetWorkflowSummary(intResourceWorkflowNEW, intEnvironment, dsnServiceEditor, dsnAsset, dsnIP) + "</p>", true, false);
                else
                {
                    if (intProject > 0)
                        oFunction.SendEmail("Request Assignment: " + strService, oUser.GetName(intImplementorUser), "", strBCC, "Request Assignment: " + strService, "<p><b>The following request has been automatically assigned to you</b><p><p><a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=/" + strDefault + oPage.GetFullLink(intViewPage) + "?pid=" + intProject.ToString() + "\" target=\"_blank\">Click here to review your new assignment.</a></p><p>" + oResourceRequest.GetWorkflowSummary(intResourceWorkflowNEW, intEnvironment, dsnServiceEditor, dsnAsset, dsnIP) + "</p>" + oVariable.EmailFooter(), true, false);
                    else
                        oFunction.SendEmail("Request Assignment: " + strService, oUser.GetName(intImplementorUser), "", strBCC, "Request Assignment: " + strService, "<p><b>The following request has been automatically assigned to you</b><p><p><a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=/frame/resource_request.aspx?rrid=" + intResourceWorkflowNEW.ToString() + "\" target=\"_blank\">Click here to review your new assignment.</a></p><p>" + oResourceRequest.GetWorkflowSummary(intResourceWorkflowNEW, intEnvironment, dsnServiceEditor, dsnAsset, dsnIP) + "</p>" + oVariable.EmailFooter(), true, false);
                }
                oProject.Update(intProject, 2);
                oResourceRequest.UpdateAccepted(intResource, 1);
                oResourceRequest.UpdateAssignedBy(intResource, -1000);
            }
        }

        bool boolSQL = false;
        Servers oServer = new Servers(0, dsn);
        DataSet dsServers = oServer.GetAnswer(intAnswer);
        foreach (DataRow drServer in dsServers.Tables[0].Rows)
        {
            int intServer = Int32.Parse(drServer["id"].ToString());
            DataSet dsComponents = oServer.GetComponents(intServer);
            foreach (DataRow drComponent in dsComponents.Tables[0].Rows)
            {
                boolSQL = (drComponent["code"].ToString() == "SQL");
                if (boolSQL == true)
                    break;
            }
            if (boolSQL == true)
                break;
        }
        if (boolSQL == true)
        {
            // send SQL
        }
        
        
        
        //int intServiceId = Int32.Parse(ConfigurationManager.AppSettings["SERVICEID_PNC_SECURITY"]);
        //int intServiceItemId = oService.GetItemId(intServiceId);
        //int intServiceNumber = 1;
        //oOnDemandTasks.AddServerOther(intRequest, intServiceId, intServiceNumber, intAnswer, intModel);
        //double dblServiceHours = oServiceDetail.GetHours(intServiceId, 1);
        //int intResource = oServiceRequest.AddRequest(intRequest, intServiceItemId, intServiceId, 1, dblServiceHours, 2, intServiceNumber, dsnServiceEditor);
        //oServiceRequest.NotifyTeamLead(intServiceItemId, intResource, intAssignPage, intViewPage, intEnvironment, strBCC, "", dsnServiceEditor, dsnAsset, dsnIP);
        //End of Generate Security Task
        oResourceRequest.UpdateWorkflowStatus(intResourceWorkflow, 3, true);
        oResourceRequest.CloseWorkflow(intResourceWorkflow, intEnvironment, strBCC, 0, dsnServiceEditor, true, intResourceRequestApprove, intAssignPage, intViewPage, dsnAsset, dsnIP);
        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "reload", "<script type=\"text/javascript\">if(window.opener!=null)window.opener.navigate('" + oPage.GetFullLink(intMyWork) + "');window.close();<" + "/" + "script>");
    }
</script>
<script type="text/javascript">
    function ChangeFrame(oCell, oShow, oHide1, oHide2, oHide3, oHidden, strHidden) {
        oShow = document.getElementById(oShow);
        oShow.style.display = "inline";
        oHide1 = document.getElementById(oHide1);
        oHide1.style.display = "none";
        oHide2 = document.getElementById(oHide2);
        oHide2.style.display = "none";
        oHide3 = document.getElementById(oHide3);
        oHide3.style.display = "none";
    	var oRow = oCell.parentElement;
	    for (var yy=0; yy<oRow.children.length; yy++) {
    		var oNot = oRow.getElementsByTagName("td").item(yy);
    		if (oNot.className == "cmbutton")
                oNot.style.border = "1px solid #94a6b5"
    	}
	    oCell.style.borderTop = "3px solid #6A8359"
        oCell.style.borderBottom = "1px solid #FFFFFF"
        oHidden = document.getElementById(oHidden);
        oHidden.value = strHidden;
    }
</script>
<asp:Panel ID="panWorkload" runat="server" Visible="false">
    <table width="100%" border="0" cellSpacing="0" cellPadding="0" class="default">
        <tr>
            <td>
                <table width="100%" border="0" cellSpacing="0" cellPadding="0">
                    <tr>
                        <td><asp:ImageButton ID="btnSave" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/tool_save.gif" OnClick="btnSave_Click" /></td>
                        <td><asp:ImageButton ID="btnPrint" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/tool_print.gif" /></td>
                        <td><asp:ImageButton ID="btnEmail" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/tool_email.gif" /></td>
                        <td><asp:ImageButton ID="btnSLA" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/tool_sla.gif" /></td>
                        <td><asp:ImageButton ID="btnComplete" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/tool_complete.gif" OnClick="btnComplete_Click" /></td>
                        <td width="100%">
                            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                <tr>
                                    <td><img src="/images/tool_left.gif" border="0" width="6" height="26"></td>
                                    <td nowrap background="/images/tool_back.gif" width="100%"><img src="/images/tool_back.gif" border="0" width="2" height="26"></td>
                                    <td><img src="/images/tool_right.gif" border="0" width="6" height="26"></td>
                                </tr>
                            </table>
                        </td>
                        <td><asp:ImageButton ID="btnClose" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/tool_close.gif" /></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="default">
                <table width="100%" border="0" cellSpacing="2" cellPadding="2" class="default">
                    <tr>
                        <td valign="top">
                            <table border="0" cellpadding="3" cellspacing="2">
                                <tr>
                                    <td nowrap><b>Name:</b></td>
                                    <td><asp:Label ID="lblName" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td nowrap><b>Number:</b></td>
                                    <td><asp:Label ID="lblNumber" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td nowrap><b>Type:</b></td>
                                    <td><asp:Label ID="lblType" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td nowrap><b>Service:</b></td>
                                    <td><asp:Label ID="lblService" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td nowrap><b>Requested By:</b></td>
                                    <td><asp:Label ID="lblRequestedBy" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td nowrap><b>Requested On:</b></td>
                                    <td><asp:Label ID="lblRequestedOn" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td nowrap><b>Last Updated:</b></td>
                                    <td><asp:Label ID="lblUpdated" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td nowrap><b>Custom Task Name:</b></td>
                                    <td><asp:TextBox ID="txtCustom" runat="server" CssClass="default" Width="300" MaxLength="100" /></td>
                                </tr>
                            </table>
                        </td>
                        <td valign="top" align="right">
                            <table border="0" cellpadding="3" cellspacing="2">
                                <tr>
                                    <td colspan="2"><iframe src="/frame/did_you_know.aspx" frameborder="0" scrolling="no" width="300" height="135"></iframe></td>
                                </tr>
                                <tr>
                                    <td nowrap><b>Overall Status:</b></td>
                                    <td><asp:Label ID="lblStatus" runat="server" CssClass="default" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <table border="0" cellpadding="3" cellspacing="2">
                                <tr>
                                    <td><b>Original Request Details:</b>&nbsp;&nbsp;<asp:Label ID="lblDescription" runat="server" CssClass="default" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr height="16">
            <td><img src="/images/spacer.gif" border="0" height="16" width="1" /></td>
        </tr>
        <tr>
            <td>
                    <table width="100%" height="100%" border="0" cellSpacing="0" cellPadding="0" class="default">
                        <tr>
                            <td class="cmbutton" style='<%=boolDetails == true ? "display:inline;border-bottom:1px solid #FFFFFF;border-top:3px solid orange;" : "" %>' onclick="ChangeFrame(this,'divDetails','divExecution','divChange','divDocuments','<%=hdnTab.ClientID %>','D');">Request Details</td>
                            <td class="cmbuttonspace">&nbsp;</td>
                            <td class="cmbutton" style='<%=boolExecution == true ? "display:inline;border-bottom:1px solid #FFFFFF;border-top:3px solid orange;" : "" %>' onclick="ChangeFrame(this,'divExecution','divDetails','divChange','divDocuments','<%=hdnTab.ClientID %>','E');">Execution</td>
                            <td class="cmbuttonspace">&nbsp;</td>
                            <td class="cmbutton" style='<%=boolChange == true ? "display:inline;border-bottom:1px solid #FFFFFF;border-top:3px solid orange;" : "" %>' onclick="ChangeFrame(this,'divChange','divDetails','divExecution','divDocuments','<%=hdnTab.ClientID %>','C');">Change Controls</td>
                            <td class="cmbuttonspace">&nbsp;</td>
                            <td class="cmbutton" style='<%=boolDocuments == true ? "display:inline;border-bottom:1px solid #FFFFFF;border-top:3px solid orange;" : "" %>' onclick="ChangeFrame(this,'divDocuments','divDetails','divExecution','divChange','<%=hdnTab.ClientID %>','D');">Attached Files</td>
                        </tr>
                        <tr>
                            <td colspan="7" align="center" class="cmcontents">
                                <table width="100%" height="100%" border="0" cellSpacing="2" cellPadding="2" class="default">
                                    <tr> 
                                        <td valign="top">
		                                    <div id="divDetails" style='<%=boolDetails == true ? "display:inline" : "display:none" %>'>
		                                        <br />
                                                <table width="100%" cellpadding="4" cellspacing="3" border="0">
                                                    <tr>
                                                        <td class="greentableheader">Request Details</td>
                                                    </tr>
                                                    <tr>
                                                        <td><asp:Label ID="lblView" runat="server" CssClass="default" /></td>
                                                    </tr>
                                                </table>
                                                <p>&nbsp;</p>
		                                    </div>
		                                    <div id="divExecution" style='<%=boolExecution == true ? "display:inline" : "display:none" %>'>
		                                        <br />
                                                <table width="100%" cellpadding="4" cellspacing="3" border="0">
                                                    <asp:Panel ID="panSlider" runat="server" Visible="false">
                                                    <tr>
                                                        <td nowrap>Completed (%):</td>
                                                        <td width="100%"><SliderCC:Slider ID="sldHours" _ParentElement="divExecution" _Hidden="hdnHours" runat="server" _DivClass="default" _PercentClass="result" _Width="300" _ShowDiv="true" _RestrictLess="true" /> <asp:Label ID="lblHours" runat="server" CssClass="required" Visible="false" Text="No hours have been allocated for this initiative" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">&nbsp;</td>
                                                    </tr>
                                                    </asp:Panel>
                                                    <asp:Panel ID="panNoSlider" runat="server" Visible="false">
                                                    <tr>
                                                        <td colspan="2">
                                                            <table width="100%" cellpadding="0" cellspacing="5" border="0">
                                                                <tr>
                                                                    <td rowspan="2"><img src="/images/ico_check40.gif" border="0" align="absmiddle" /></td>
                                                                    <td class="header" width="100%" valign="bottom">Fast-Completion Enabled</td>
                                                                </tr>
                                                                <tr>
                                                                    <td width="100%" valign="top">With fast-completion, all you need to do is click the &quot;Complete&quot; button to close this request.</td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">&nbsp;</td>
                                                    </tr>
                                                    </asp:Panel>
                                                    <asp:Panel ID="panCheckboxes" runat="server" Visible="false">
                                                    <tr>
                                                        <td colspan="2" class="greentableheader">Required Tasks</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2"><%=strCheckboxes %></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">&nbsp;</td>
                                                    </tr>
                                                    </asp:Panel>
                                                    <tr>
                                                        <td colspan="2" class="greentableheader">Weekly Status</td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap>Status:</td>
                                                        <td width="100%">
                                                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="default" >
                                                                <asp:ListItem Text="-- SELECT --" Value="0" />
                                                                <asp:ListItem Text="Red" Value="1" />
                                                                <asp:ListItem Text="Yellow" Value="2" />
                                                                <asp:ListItem Text="Green" Value="3" />
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2"><b>Comments / Issues:</b></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2"><asp:TextBox ID="txtComments" runat="server" CssClass="default" Width="100%" TextMode="MultiLine" Rows="13" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <table width="100%" cellpadding="3" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                                                                <tr bgcolor="#EEEEEE">
                                                                    <td nowrap><b>Date</b></td>
                                                                    <td nowrap>&nbsp;</td>
                                                                    <td nowrap><b>Status</b></td>
                                                                </tr>
                                                                <asp:repeater ID="rptStatus" runat="server">
                                                                    <ItemTemplate>
                                                                        <tr class="default">
                                                                            <td nowrap><a href="javascript:void(0);" onclick="ShowStatus('<%# DataBinder.Eval(Container.DataItem, "id") %>','r');"><%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "modified").ToString()).ToShortDateString() %>&nbsp;<%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "modified").ToString()).ToShortTimeString() %></a></td>
                                                                            <td nowrap>&nbsp;</td>
                                                                            <td width="100%"><asp:Label ID="lblStatus" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "status") %>' /></td>
                                                                        </tr>
                                                                    </ItemTemplate>
                                                                </asp:repeater>
                                                            <tr>
                                                                <td colspan="3">
                                                                    <asp:Label ID="lblNoStatus" runat="server" CssClass="default" Visible="false" Text="<img src='/images/alert.gif' border='0' align='absmiddle'> There are no updates" />
                                                                </td>
                                                            </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
		                                    </div>
		                                    <div id="divChange" style='<%=boolChange == true ? "display:inline" : "display:none" %>'>
		                                        <br />
                                                <table width="100%" cellpadding="4" cellspacing="3" border="0">
                                                    <tr>
                                                        <td colspan="2" class="greentableheader">Change Controls</td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap>Number:</td>
                                                        <td width="100%"><asp:TextBox ID="txtNumber" runat="server" CssClass="default" Width="150" MaxLength="15" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap>Date:</td>
                                                        <td width="100%"><asp:TextBox ID="txtDate" runat="server" CssClass="default" Width="100" MaxLength="10" /> <asp:ImageButton ID="imgDate" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap>Time:</td>
                                                        <td width="100%"><asp:TextBox ID="txtTime" runat="server" CssClass="default" Width="100" MaxLength="10" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2"><b>Comments:</b></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2"><asp:TextBox ID="txtChange" runat="server" CssClass="default" Width="500" TextMode="MultiLine" Rows="7" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2"><asp:Button ID="btnChange" runat="server" CssClass="default" Width="150" Text="Submit Change" OnClick="btnChange_Click" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <table width="100%" cellpadding="3" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                                                                <tr bgcolor="#EEEEEE">
                                                                    <td nowrap><b>Number</b></td>
                                                                    <td nowrap><b>Date</b></td>
                                                                    <td nowrap>&nbsp;</td>
                                                                </tr>
                                                                <asp:repeater ID="rptChange" runat="server">
                                                                    <ItemTemplate>
                                                                        <tr class="default">
                                                                            <td nowrap><a href="javascript:void(0);" onclick="ShowChange('<%# DataBinder.Eval(Container.DataItem, "changeid") %>');"><%# DataBinder.Eval(Container.DataItem, "number") %></a></td>
                                                                            <td nowrap><%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "implementation").ToString()).ToLongDateString() %> @ <%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "implementation").ToString()).ToLongTimeString() %></td>
                                                                            <td align="right">[<asp:LinkButton ID="btnDeleteChange" runat="server" Text="Delete" OnClick="btnDeleteChange_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "changeid") %>' />]</td>
                                                                        </tr>
                                                                    </ItemTemplate>
                                                                </asp:repeater>
                                                            <tr>
                                                                <td colspan="3">
                                                                    <asp:Label ID="lblNoChange" runat="server" CssClass="default" Visible="false" Text="<img src='/images/alert.gif' border='0' align='absmiddle'> There are no change controls" />
                                                                </td>
                                                            </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
		                                    </div>
		                                    <div id="divDocuments" style='<%=boolDocuments == true ? "display:inline" : "display:none" %>'>
		                                        <br />
                                                <table width="100%" cellpadding="4" cellspacing="3" border="0">
                                                    <tr>
                                                        <td class="greentableheader">Attached Files</td>
                                                        <td align="right"><asp:CheckBox ID="chkDescription" runat="server" CssClass="default" Text="Show Descriptions" AutoPostBack="true" OnCheckedChanged="chkDescription_Change" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2"><asp:Label ID="lblDocuments" runat="server" CssClass="default" /></td>
                                                    </tr>
                                                </table>
		                                    </div>
		                                </td>
		                            </tr>
		                        </table>
		                    </td>
	                    </tr>
	                </table>
            </td>
        </tr>
    </table>
</asp:Panel>
<asp:Panel ID="panDenied" runat="server" Visible="false">
    <table width="100%" cellpadding="2" cellspacing="2" border="0">
        <tr>
            <td><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
            <td class="header"><img src="/images/bigX.gif" border="0" align="absmiddle" /> Access Denied</td>
        </tr>
        <tr><td colspan="2">&nbsp;</td></tr>
        <tr>
            <td><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
            <td>You do not have rights to view this item.</td>
        </tr>
        <tr><td colspan="2">&nbsp;</td></tr>
        <tr>
            <td colspan="2"><hr size="1" noshade /></td>
        </tr>
        <tr>
            <td colspan="2" align="right">
                <table width="100%" cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td class="footer"></td>
                        <td align="right"><asp:Button ID="btnDenied" runat="server" CssClass="default" Width="75" Text="Close" /></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Panel>
<asp:Label ID="lblResourceWorkflow" runat="server" Visible="false" />
<asp:HiddenField ID="hdnTab" runat="server" />