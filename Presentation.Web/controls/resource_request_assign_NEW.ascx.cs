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
    public partial class resource_request_assign_NEW : System.Web.UI.UserControl
    {

        private DataSet ds;
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        private string dsnServiceEditor = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ServiceEditorDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intViewPage = Int32.Parse(ConfigurationManager.AppSettings["ViewWorkload"]);
        protected int intViewRequest = Int32.Parse(ConfigurationManager.AppSettings["ViewRequest"]);
        protected int intViewResourceRequest = Int32.Parse(ConfigurationManager.AppSettings["ResourceRequest"]);
        protected int intAssignPage = Int32.Parse(ConfigurationManager.AppSettings["AssignWorkload"]);
        protected int intSearchPage = Int32.Parse(ConfigurationManager.AppSettings["SearchPage"]);
        protected int intImplementorDistributed = Int32.Parse(ConfigurationManager.AppSettings["ITEMID_IMPLEMENTOR_DISTRIBUTED"]);
        protected int intImplementorMidrange = Int32.Parse(ConfigurationManager.AppSettings["ITEMID_IMPLEMENTOR_MIDRANGE"]);
        protected int intDesignBuilder = Int32.Parse(ConfigurationManager.AppSettings["ForecastEdit"]);
       
        protected int intProfile;
        protected ProjectRequest oProjectRequest;
        protected ResourceRequest oResourceRequest;
        protected ProjectNumber oProjectNumber;
        protected Pages oPage;
        protected Users oUser;
        protected RequestItems oRequestItem;
        protected Applications oApplication;
        protected Requests oRequest;
        protected Variables oVariable;
        protected Services oService;
        protected Delegates oDelegate;
        protected StatusLevels oStatusLevel;
        protected ServiceDetails oServiceDetail;
        protected Log oLog;
        protected int intApplication = 0;
        protected int intPage = 0;
        protected string strSummary = "Project Information Not Available";
        protected string strAvailable = "";
        private string strEMailIdsBCC = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oProjectRequest = new ProjectRequest(intProfile, dsn);
            oResourceRequest = new ResourceRequest(intProfile, dsn);
            oProjectNumber = new ProjectNumber(intProfile, dsn);
            oPage = new Pages(intProfile, dsn);
            oUser = new Users(intProfile, dsn);
            oRequestItem = new RequestItems(intProfile, dsn);
            oApplication = new Applications(intProfile, dsn);
            oRequest = new Requests(intProfile, dsn);
            oVariable = new Variables(intEnvironment);
            oService = new Services(intProfile, dsn);
            oDelegate = new Delegates(intProfile, dsn);
            oStatusLevel = new StatusLevels();
            oServiceDetail = new ServiceDetails(intProfile, dsn);
            oLog = new Log(intProfile, dsn);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.QueryString["action"] != null && Request.QueryString["action"] != "")
                panFinish.Visible = true;
            else
            {
                if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                    intApplication = Int32.Parse(Request.QueryString["applicationid"]);
                if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                    intApplication = Int32.Parse(Request.Cookies["application"].Value);
                if (!IsPostBack)
                {
                    if (Request.QueryString["rrid"] != null && Request.QueryString["rrid"] != "")
                    {
                        lblResourceParent.Text = Request.QueryString["rrid"];
                        int intResourceParent = Int32.Parse(lblResourceParent.Text);
                        ds = oResourceRequest.Get(intResourceParent);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            int intItem = Int32.Parse(ds.Tables[0].Rows[0]["itemid"].ToString());
                            int intService = Int32.Parse(ds.Tables[0].Rows[0]["serviceid"].ToString());
                            int intRequest = Int32.Parse(ds.Tables[0].Rows[0]["requestid"].ToString());
                            int intNumber = Int32.Parse(ds.Tables[0].Rows[0]["number"].ToString());
                            int intProject = Int32.Parse(oRequest.Get(intRequest, "projectid"));
                            int intApp = oRequestItem.GetItemApplication(intItem);
                            int intForecast = 0;
                            Forecast oForecast = new Forecast(intProfile, dsn);
                            DataSet dsForecast = oForecast.GetProject(intProject);
                            if (dsForecast.Tables[0].Rows.Count > 0)
                                intForecast = Int32.Parse(dsForecast.Tables[0].Rows[0]["id"].ToString());
                            if (intForecast > 0)
                                btnView.Attributes.Add("oncontextmenu", "return OpenWindow('NEW_WINDOW','" + oPage.GetFullLink(intDesignBuilder) + "?id=" + intForecast.ToString() + "');");
                            else
                                btnView.Attributes.Add("oncontextmenu", "alert('There is no design for this project');return false;");
                            if (ds.Tables[0].Rows[0]["solo"].ToString() == "0")
                            {
                                btnView.Attributes.Add("onclick", "return OpenWindow('PRINTER_FRIENDLY','?page=" + intViewRequest.ToString() + "&rid=" + intRequest.ToString() + "');");
                                btnView.Text = "Click here to view the project details";
                                lblType.Text = "Project Request";
                            }
                            else
                            {
                                btnView.Attributes.Add("onclick", "return OpenWindow('PRINTER_FRIENDLY','?page=" + intViewResourceRequest.ToString() + "&rrid=" + intResourceParent.ToString() + "');");
                                if (oApplication.Get(intApp, "tpm") == "1")
                                    btnView.Text = "Click here to view the project details";
                                else
                                    btnView.Text = "Click here to view this request";
                                lblType.Text = "Service Request";
                            }
                            btnAssignments.Attributes.Add("onclick", "return OpenWindow('PRINTER_FRIENDLY','?page=" + intSearchPage.ToString() + "&pid=" + intProject + "&aid=" + intApp.ToString() + "&sort=userid');");
                            lblItem.Text = intItem.ToString();
                            lblService.Text = oService.GetName(intService);
                            lblServiceID.Text = intService.ToString();
                            if (oApplication.Get(intApp, "request_items") == "1")
                                lblGroup.Text = oApplication.GetName(intApp) + " | " + oRequestItem.GetItemName(intItem);
                            else
                                lblGroup.Text = oApplication.GetName(intApp);
                            int intUser = Int32.Parse(oRequest.Get(intRequest, "userid"));
                            int intAppManager = oApplication.GetManager(intApp);
                            if (ds.Tables[0].Rows[0]["accepted"].ToString() == "-1" && oApplication.Get(intApp, "tpm") == "1")
                            {
                                if (intUser == intProfile || oDelegate.Get(intUser, intProfile) > 0)
                                {
                                    panRequest.Visible = true;
                                    panAssignSingle.Visible = true;
                                    txtHours.Text = double.Parse(ds.Tables[0].Rows[0]["allocated"].ToString()).ToString("F");
                                    lblHoursOverall.Text = double.Parse(ds.Tables[0].Rows[0]["allocated"].ToString()).ToString("F");
                                    txtQuantityOverall.Text = ds.Tables[0].Rows[0]["devices"].ToString();
                                    txtQuantityOverall.Enabled = false;
                                    txtQuantity.Text = txtQuantityOverall.Text;
                                    txtQuantity.Enabled = false;
                                    lblHours.Visible = true;
                                    lblHours.Text = txtHours.Text;
                                    lblStatus.Text = oStatusLevel.HTML(Int32.Parse(ds.Tables[0].Rows[0]["status"].ToString()));
                                    txtQuantity.Text = ds.Tables[0].Rows[0]["devices"].ToString();
                                    btnAssign.Enabled = (txtQuantity.Text != "0");
                                    lblAssignBy.Text = oUser.GetFullName(intProfile);
                                    strSummary = oResourceRequest.GetBodyOverallFix(intResourceParent, 0, intEnvironment, false);
                                }
                                else
                                {
                                    Response.Write("no rights");
                                    panDenied.Visible = true;
                                }
                                btnSubmit.Attributes.Add("onclick", "return ValidateNumber0('" + txtHours.ClientID + "','Please enter a valid number for the hours allocated') && ValidateHidden('" + hdnManager.ClientID + "','" + txtManager.ClientID + "','Please enter a valid LAN ID') && ProcessButton(this);");
                            }
                            else
                            {
                                if (oUser.IsAdmin(intProfile) || oService.IsManager(intService, intProfile) || (oApplication.IsManager(intApp, intProfile) && oApplication.Get(intApp, "disable_manager") != "1"))
                                {
                                    panRequest.Visible = true;
                                    panAssignMultiple.Visible = true;
                                    btnSubmit.Enabled = (ds.Tables[0].Rows[0]["assigned"].ToString() == "");
                                    if (ds.Tables[0].Rows[0]["status"].ToString() == "5")
                                    {
                                        radHold.Checked = true;
                                        divChoice.Style["display"] = "inline";
                                        divHold.Style["display"] = "inline";
                                        txtHold.Text = ds.Tables[0].Rows[0]["reason"].ToString();
                                    }
                                    else
                                    {
                                        divChoice.Style["display"] = (ds.Tables[0].Rows[0]["accepted"].ToString() == "0" ? "inline" : "none");
                                        if (Request.QueryString["assign"] != null)
                                        {
                                            divAccept.Style["display"] = "inline";
                                            radAccept.Checked = true;
                                        }
                                        else
                                            divAccept.Style["display"] = (ds.Tables[0].Rows[0]["accepted"].ToString() == "1" ? "inline" : "none");
                                    }
                                    panDelete.Visible = (intItem == intImplementorDistributed || intItem == intImplementorMidrange);
                                    btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this request');");
                                    txtHours.Text = double.Parse(ds.Tables[0].Rows[0]["allocated"].ToString()).ToString("F");
                                    lblHoursOverall.Text = double.Parse(ds.Tables[0].Rows[0]["allocated"].ToString()).ToString("F");
                                    txtQuantityOverall.Text = ds.Tables[0].Rows[0]["devices"].ToString();
                                    if (oApplication.Get(intApp, "tpm") == "1")
                                    {
                                        lblHours.Visible = true;
                                        lblHours.Text = txtHours.Text;
                                        txtQuantityOverall.Enabled = false;
                                        txtQuantity.Enabled = false;
                                        btnAssign.Attributes.Add("onclick", "return ValidateDropDown('" + ddlUser.ClientID + "','Please select a technician')" +
                                            " && ProcessButton(this)" +
                                            ";");
                                    }
                                    else if (oServiceDetail.Gets(intService, 1).Tables[0].Rows.Count > 0)
                                    {
                                        lblHours.Visible = true;
                                        lblHours.Text = "TBD ";
                                        btnWhy.Visible = true;
                                        btnAssign.Attributes.Add("onclick", "return ValidateDropDown('" + ddlUser.ClientID + "','Please select a technician')" +
                                            " && ValidateNumber0('" + txtQuantity.ClientID + "','Please enter the quantity')" +
                                            " && ProcessButton(this)" +
                                            ";");
                                    }
                                    else if (oService.Get(intService, "disable_hours") == "1")
                                    {
                                        lblHours.Visible = true;
                                        lblHours.Text = txtHours.Text;
                                        btnAssign.Attributes.Add("onclick", "return ValidateDropDown('" + ddlUser.ClientID + "','Please select a technician')" +
                                            " && ValidateNumber0('" + txtQuantity.ClientID + "','Please enter the quantity')" +
                                            " && ProcessButton(this)" +
                                            ";");
                                    }
                                    else
                                    {
                                        txtHours.Visible = true;
                                        btnAssign.Attributes.Add("onclick", "return ValidateDropDown('" + ddlUser.ClientID + "','Please select a technician')" +
                                            " && ValidateNumber0('" + txtQuantity.ClientID + "','Please enter the quantity')" +
                                            " && ValidateNumber0('" + txtHours.ClientID + "','Please enter the hours')" +
                                            " && ProcessButton(this)" +
                                            ";");
                                    }
                                    btnUpdate.Attributes.Add("onclick", "return ValidateNumber0('" + txtQuantityOverall.ClientID + "') && ProcessButton(this);");
                                    DataSet dsAssigned = oResourceRequest.GetWorkflowsAssign(intResourceParent);
                                    int intQuantity = Int32.Parse(ds.Tables[0].Rows[0]["devices"].ToString());
                                    foreach (DataRow drAssigned in dsAssigned.Tables[0].Rows)
                                        intQuantity = intQuantity - Int32.Parse(drAssigned["devices"].ToString());
                                    txtQuantity.Text = intQuantity.ToString();
                                    btnAssign.Enabled = (txtQuantity.Text != "0");
                                    lblStatus.Text = oStatusLevel.HTML(Int32.Parse(ds.Tables[0].Rows[0]["status"].ToString()));
                                    lblAssigned.Visible = !(btnSubmit.Enabled);
                                    string strAssign = "";
                                    DataSet dsManager = oService.GetUser(intService, 1);
                                    foreach (DataRow drManager in dsManager.Tables[0].Rows)
                                    {
                                        if (strAssign != "")
                                            strAssign += ", ";
                                        strAssign += oUser.GetFullName(Int32.Parse(drManager["userid"].ToString()));
                                    }
                                    lblAssignBy.Text = strAssign;
                                    DataSet dsReports = oUser.GetManagerReports(intAppManager, intRequest, intService, intNumber);
                                    DataTable dtReports = dsReports.Tables[0].Copy();
                                    dtReports.Clear();
                                    DataSet dsAssignment = oService.GetUser(intService, -100);
                                    foreach (DataRow drAssignment in dsAssignment.Tables[0].Rows)
                                    {
                                        int intAssignment = Int32.Parse(drAssignment["userid"].ToString());
                                        DataSet dsAssignees = oUser.GetManagerReports(intAssignment, intRequest, intService, intNumber);
                                        foreach (DataRow drAssignee in dsAssignees.Tables[0].Rows)
                                            dtReports.ImportRow(drAssignee);
                                    }
                                    if (dtReports.Rows.Count > 0)
                                    {
                                        // Use custom assignment list
                                        // First, remove duplicates
                                        Hashtable hTable = new Hashtable();
                                        ArrayList duplicateList = new ArrayList();
                                        foreach (DataRow drow in dtReports.Rows)
                                        {
                                            if (hTable.Contains(drow["userid"]))
                                                duplicateList.Add(drow);
                                            else
                                                hTable.Add(drow["userid"], string.Empty);
                                        }
                                        foreach (DataRow dRow in duplicateList)
                                            dtReports.Rows.Remove(dRow);

                                        // Next, sort by username
                                        dtReports.DefaultView.Sort = "username";

                                        // Now add to dataset
                                        dsReports = new DataSet();
                                        dsReports.Tables.Add(dtReports.DefaultView.ToTable());
                                    }
                                    LoadList(dsReports, intApp);
                                    LoadAvailable(dsReports, intApp);
                                    rptAssign.DataSource = dsAssigned;
                                    rptAssign.DataBind();
                                    foreach (RepeaterItem ri in rptAssign.Items)
                                        ((LinkButton)ri.FindControl("btnDeleteAssign")).Attributes.Add("onclick", "return confirm('Are you sure you want to delete this assignment?');");
                                    lblAssign.Visible = (rptAssign.Items.Count == 0);
                                    strSummary = oResourceRequest.GetBodyOverallFix(intResourceParent, 0, intEnvironment, false);
                                    btnSubmit.Attributes.Add("onclick", "return ValidateChoice('" + divChoice.ClientID + "','" + radAccept.ClientID + "','" + radReject.ClientID + "','" + radHold.ClientID + "'," + dsAssigned.Tables[0].Rows.Count.ToString() + ") && ProcessButton(this);");
                                }
                                else
                                {
                                    Response.Write("no delegate");
                                    panDenied.Visible = true;
                                }
                            }
                        }
                        else
                            panDenied.Visible = true;
                    }
                    else
                        panDenied.Visible = true;
                }
            }
            btnClose.Attributes.Add("onclick", "return CloseWindow();");
            lnkAvailable.Attributes.Add("onclick", "return ShowHideAvailable('" + divAvailable.ClientID + "');");
            radAccept.Attributes.Add("onclick", "ShowHideDiv('" + divAccept.ClientID + "','inline');ShowHideDiv('" + divReject.ClientID + "','none');ShowHideDiv('" + divHold.ClientID + "','none');");
            radReject.Attributes.Add("onclick", "ShowHideDiv('" + divReject.ClientID + "','inline');ShowHideDiv('" + divAccept.ClientID + "','none');ShowHideDiv('" + divHold.ClientID + "','none');");
            radHold.Attributes.Add("onclick", "ShowHideDiv('" + divHold.ClientID + "','inline');ShowHideDiv('" + divAccept.ClientID + "','none');ShowHideDiv('" + divReject.ClientID + "','none');");
            txtManager.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'300','195','" + divManager.ClientID + "','" + lstManager.ClientID + "','" + hdnManager.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
            lstManager.Attributes.Add("ondblclick", "AJAXClickRow();");
            btnWhy.Attributes.Add("onclick", "return ShowHideAvailable('" + divWhy.ClientID + "');");
        }
        private void LoadList(DataSet _dsReports, int _app)
        {
            ddlUser.DataValueField = "userid";
            ddlUser.DataTextField = "username";
            ddlUser.DataSource = _dsReports;
            ddlUser.DataBind();
            ddlUser.Items.Insert(0, new ListItem("-- SELECT --", "0"));
        }
        private void LoadAvailable(DataSet _dsReports, int _app)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("name", System.Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("hours", System.Type.GetType("System.Double")));
            dt.Columns.Add(new DataColumn("graph", System.Type.GetType("System.Double")));
            foreach (DataRow dr in _dsReports.Tables[0].Rows)
            {
                int intUser = Int32.Parse(dr["userid"].ToString());
                //DataSet dsAss = oResourceRequest.GetWorkflowAssigned(intUser, 2);
                //double dblTotal = 0;
                //foreach (DataRow drAss in dsAss.Tables[0].Rows)
                //{
                //    int intId = Int32.Parse(drAss["id"].ToString());
                //    double dblAllocated = double.Parse(drAss["allocated"].ToString());
                //    double dblUsed = oResourceRequest.GetWorkflowUsed(intId);
                //    dblTotal += dblAllocated - dblUsed;
                //}
                double dblAllocated = 0;
                double.TryParse(dr["allocated"].ToString(), out dblAllocated);
                double dblUsed = 0;
                double.TryParse(dr["used"].ToString(), out dblUsed);
                double dblTotal = dblAllocated - dblUsed;

                DataRow drRow = dt.NewRow();
                drRow["name"] = oUser.GetFullName(intUser);
                drRow["hours"] = dblTotal;
                dt.Rows.Add(drRow);
            }

            ds = new DataSet();
            ds.Tables.Add(dt);
            double dblMax = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                double dblHours = double.Parse(dr["hours"].ToString());
                if (dblMax < dblHours)
                    dblMax = dblHours;
            }
            if (dblMax > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    double dblHours = double.Parse(dr["hours"].ToString());
                    dblHours = dblHours / dblMax;
                    dr["graph"] = dblHours * 400;
                }
            }
            rptAvailable.DataSource = ds;
            rptAvailable.DataBind();
        }
        protected void btnSubmit_Click(Object Sender, EventArgs e)
        {
            int intResourceParent = Int32.Parse(lblResourceParent.Text);
            ds = oResourceRequest.Get(intResourceParent);
            if (ds.Tables[0].Rows.Count > 0)
            {
                int intRequest = Int32.Parse(ds.Tables[0].Rows[0]["requestid"].ToString());
                int intItem = Int32.Parse(ds.Tables[0].Rows[0]["itemid"].ToString());
                int intService = Int32.Parse(ds.Tables[0].Rows[0]["serviceid"].ToString());
                int intNumber = Int32.Parse(ds.Tables[0].Rows[0]["number"].ToString());
                string strCVT = "CVT" + intRequest.ToString() + "-" + intService.ToString() + "-" + intNumber.ToString();
                string strService = oService.GetName(intService);
                if (intService == 0)
                    strService = oRequestItem.GetItemName(intItem);
                int intApp = oRequestItem.GetItemApplication(intItem);
                int intProject = Int32.Parse(oRequest.Get(intRequest, "projectid"));
                Projects oProject = new Projects(intProfile, dsn);
                ProjectsPending oProjectsPending = new ProjectsPending(intProfile, dsn, intEnvironment);
                Functions oFunction = new Functions(intProfile, dsn, intEnvironment);
                Variables oVariable = new Variables(intEnvironment);
                RequestFields oRequestField = new RequestFields(intProfile, dsn);
                int intRequester = Int32.Parse(oRequest.Get(intRequest, "userid"));
                int intPC = 0;
                int intIE = 0;
                if (intProject > 0)
                {
                    if (oProject.Get(intProject, "lead") != "")
                        intPC = Int32.Parse(oProject.Get(intProject, "lead"));
                    if (oProject.Get(intProject, "engineer") != "")
                        intIE = Int32.Parse(oProject.Get(intProject, "engineer"));
                }
                else
                {
                    try
                    {
                        intPC = Int32.Parse(oProjectsPending.GetRequest(intRequest, "lead"));
                        intIE = Int32.Parse(oProjectsPending.GetRequest(intRequest, "engineer"));
                    }
                    catch { }
                }
                string strCC = "";
                if (intPC > 0)
                    strCC += oUser.GetName(intPC) + ";";
                if (intIE > 0)
                    strCC += oUser.GetName(intIE) + ";";
                if (radReject.Checked == true)
                {
                    string strComments = "";
                    if (txtComments.Text.Trim() != "")
                        strComments = "<p>The following comments were added:<br/>" + txtComments.Text + "</p>";
                    string strDefault = oUser.GetApplicationUrl(intRequester, intAssignPage);
                    strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_REQUEST_ASSIGNMENT,EMAILGRP_REQUEST_STATUS");
                    if (strDefault == "" || oApplication.Get(intApp, "tpm") != "1")
                        oFunction.SendEmail("Request REJECTED: " + strService, oUser.GetName(intRequester), strCC, strEMailIdsBCC, "Request REJECTED: " + strService, "<p><b>The following request has been rejected by " + oUser.GetFullName(intProfile) + "</b><p><p>" + oResourceRequest.GetSummary(intResourceParent, 0, intEnvironment, dsnServiceEditor, dsnAsset, dsnIP) + "</p>" + strComments, true, false);
                    else
                        oFunction.SendEmail("Request REJECTED: " + strService, oUser.GetName(intRequester), strCC, strEMailIdsBCC, "Request REJECTED: " + strService, "<p><b>The following request has been rejected by " + oUser.GetFullName(intProfile) + "</b><p><p>" + oResourceRequest.GetSummary(intResourceParent, 0, intEnvironment, dsnServiceEditor, dsnAsset, dsnIP) + "</p>" + strComments + "<p><a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=/" + strDefault + oPage.GetFullLink(intAssignPage) + "?rrid=" + intResourceParent.ToString() + "\" target=\"_blank\">Click here to assign a new project manager to your request.</a></p>", true, false);
                    oResourceRequest.UpdateAccepted(intResourceParent, -1);
                    oResourceRequest.UpdateReason(intResourceParent, txtComments.Text);
                }
                else if (radHold.Checked == true)
                {
                    oResourceRequest.UpdateStatusOverall(intResourceParent, 5);
                    oResourceRequest.UpdateReason(intResourceParent, txtHold.Text);
                }
                else
                {
                    if (oApplication.Get(intApp, "tpm") != "1" && oProject.Get(intProject, "number") == "")
                        oProject.Update(intProject, oProjectNumber.New());
                    oProject.Update(intProject, 2);
                    bool boolSolo = (oResourceRequest.Get(intResourceParent, "solo") == "1");
                    int intAssigned = 0;
                    bool boolRejected = (oResourceRequest.Get(intResourceParent, "accepted") == "-1");
                    ProjectRequest oProjectRequest = new ProjectRequest(intProfile, dsn);
                    string strNotify = "";
                    string strSpacerRow = "<tr><td colspan=\"3\"><img src=\"" + oVariable.ImageURL() + "/images/spacer.gif\" border=\"0\" width=\"1\" height=\"7\" /></td></tr>";
                    if (boolRejected == true)
                    {
                        oResourceRequest.UpdateRejected(intResourceParent, 1);
                        intAssigned = Int32.Parse(Request.Form[hdnManager.UniqueID]);
                        int intResourceWorkflow = oResourceRequest.AddWorkflow(intResourceParent, 0, oResourceRequest.Get(intResourceParent, "name"), intAssigned, Int32.Parse(txtQuantity.Text), double.Parse(txtHours.Text), 2, 0);
                        oLog.AddEvent(intRequest.ToString(), strCVT, "Request assigned by " + oUser.GetFullNameWithLanID(intProfile) + " to " + oUser.GetFullNameWithLanID(intAssigned), LoggingType.Debug);
                        string strDefault = oUser.GetApplicationUrl(intAssigned, intViewPage);
                        if (boolSolo == false)
                        {
                            strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_REQUEST_ASSIGNMENT");
                            // ADD PM
                            if (strDefault == "")
                                oFunction.SendEmail("Request Assignment", oUser.GetName(intAssigned), "", strEMailIdsBCC, "Request Assignment", "<p><b>The following project has been assigned to you by " + oUser.GetFullName(intProfile) + "</b><p><p>" + oProjectRequest.GetBody(intRequest, intEnvironment, true) + "</p>", true, false);
                            else
                            {
                                if (intProject > 0)
                                    oFunction.SendEmail("Request Assignment", oUser.GetName(intAssigned), "", strEMailIdsBCC, "Request Assignment", "<p><b>The following project has been assigned to you by " + oUser.GetFullName(intProfile) + "</b><p><p>" + oProjectRequest.GetBody(intRequest, intEnvironment, true) + "</p><p><a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=/" + strDefault + oPage.GetFullLink(intViewPage) + "?pid=" + intProject.ToString() + "\" target=\"_blank\">Click here to review your new assignment.</a></p>", true, false);
                                else
                                    oFunction.SendEmail("Request Assignment", oUser.GetName(intAssigned), "", strEMailIdsBCC, "Request Assignment", "<p><b>The following project has been assigned to you by " + oUser.GetFullName(intProfile) + "</b><p><p>" + oProjectRequest.GetBody(intRequest, intEnvironment, true) + "</p><p><a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=/frame/resource_request.aspx?rrid=" + intResourceWorkflow.ToString() + "\" target=\"_blank\">Click here to review your new assignment.</a></p>", true, false);
                            }
                            string strExecutive = oProject.Get(intProject, "executive");
                            string strWorking = oProject.Get(intProject, "working");
                            string strActivity = "<tr><td><b>Resource:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>" + oUser.GetFullName(intAssigned) + "</td></tr>";
                            strActivity += strSpacerRow;
                            strActivity += "<tr><td><b>Service:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>Project Coordinator</td></tr>";
                            strActivity = "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"" + oVariable.DefaultFontStyle() + "\">" + strActivity + "</table>";
                            string strDeliverable = oApplication.Get(intApp, "deliverables_doc");
                            if (strDeliverable.Trim() != "")
                                strDeliverable = "<p><a href=\"" + oVariable.URL() + strDeliverable + "\">Click here to view the service deliverables</a></p>";
                            if (oService.Get(intService, "notify_client") != "0")
                            {
                                strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_REQUEST_ASSIGNMENT");
                                oFunction.SendEmail("Request Assignment", oUser.GetName(intRequester), strExecutive + ";" + strWorking + ";" + strCC, strEMailIdsBCC, "Request Assignment", "<p><b>A resource has been assigned to the following project...</b><p><p>" + oProjectRequest.GetBody(intRequest, intEnvironment, true) + "</p><p>" + strActivity + "</p>" + strDeliverable + strNotify, true, false);
                            }
                        }
                        else
                        {
                            strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_ERROR");
                            oFunction.SendEmail("ERROR: Request Assignment", strEMailIdsBCC, "", "", "ERROR: Request Assignment", "<p><b>A REJECTED resource request with a SOLO status of 1 has been assigned to " + oUser.GetName(intAssigned) + "...</b><p><p>" + oProjectRequest.GetBody(intRequest, intEnvironment, true) + "</p>", true, false);
                        }
                    }
                    else
                    {
                        oResourceRequest.UpdateAccepted(intResourceParent, 1);
                        if (boolSolo == true)
                        {
                            string strResources = "";
                            DataSet dsAssigned = oResourceRequest.GetWorkflowsAssign(intResourceParent);
                            foreach (DataRow drAssigned in dsAssigned.Tables[0].Rows)
                            {
                                intAssigned = Int32.Parse(drAssigned["userid"].ToString());
                                int intQuantity = Int32.Parse(drAssigned["devices"].ToString());
                                double dblHours = double.Parse(drAssigned["allocated"].ToString());
                                int intResourceWorkflow = oResourceRequest.AddWorkflow(intResourceParent, 0, oResourceRequest.Get(intResourceParent, "name"), intAssigned, intQuantity, dblHours, 2, 0);
                                oLog.AddEvent(intRequest.ToString(), strCVT, "Request assigned by " + oUser.GetFullNameWithLanID(intProfile) + " to " + oUser.GetFullNameWithLanID(intAssigned), LoggingType.Debug);
                                oResourceRequest.UpdateWorkflowStatus(intResourceWorkflow, 2, true);
                                if (intItem != intImplementorDistributed && intItem != intImplementorMidrange)
                                {
                                    //if (oProject.Get(intProject, "number").StartsWith("CV") == false)
                                    //    strNotify = "<p><span style=\"color:#0000FF\"><b>PROJECT COORDINATOR:</b> Please allocate the hours listed above for each resource in Clarity.</span></p>";
                                    string strDefault = oUser.GetApplicationUrl(intAssigned, intViewPage);
                                    strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_REQUEST_ASSIGNMENT");
                                    if (strDefault == "")
                                        oFunction.SendEmail("Request Assignment: " + strService, oUser.GetName(intAssigned), "", strEMailIdsBCC, "Request Assignment: " + strService, "<p><b>The following request has been assigned to you by " + oUser.GetFullName(intProfile) + "</b><p><p>" + oResourceRequest.GetWorkflowSummary(intResourceWorkflow, intEnvironment, dsnServiceEditor, dsnAsset, dsnIP) + "</p>", true, false);
                                    else
                                    {
                                        if (intProject > 0)
                                            oFunction.SendEmail("Request Assignment: " + strService, oUser.GetName(intAssigned), "", strEMailIdsBCC, "Request Assignment: " + strService, "<p><b>The following request has been assigned to you by " + oUser.GetFullName(intProfile) + "</b><p><p>" + oResourceRequest.GetWorkflowSummary(intResourceWorkflow, intEnvironment, dsnServiceEditor, dsnAsset, dsnIP) + "</p><p><a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=/" + strDefault + oPage.GetFullLink(intViewPage) + "?pid=" + intProject.ToString() + "\" target=\"_blank\">Click here to review your new assignment.</a></p>", true, false);
                                        else
                                            oFunction.SendEmail("Request Assignment: " + strService, oUser.GetName(intAssigned), "", strEMailIdsBCC, "Request Assignment: " + strService, "<p><b>The following request has been assigned to you by " + oUser.GetFullName(intProfile) + "</b><p><p>" + oResourceRequest.GetWorkflowSummary(intResourceWorkflow, intEnvironment, dsnServiceEditor, dsnAsset, dsnIP) + "</p><p><a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=/frame/resource_request.aspx?rrid=" + intResourceWorkflow.ToString() + "\" target=\"_blank\">Click here to review your new assignment.</a></p>", true, false);
                                    }
                                    if (strResources != "")
                                        strResources += ", ";
                                    strResources += oUser.GetFullName(intAssigned);
                                }
                            }
                            string strActivity = "<tr><td><b>Resource(s):</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>" + strResources + "</td></tr>";
                            strActivity += strSpacerRow;
                            strActivity += "<tr><td><b>Service:</b></td><td>&nbsp;&nbsp;&nbsp;</td><td>" + strService + "</td></tr>";
                            strActivity = "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"" + oVariable.DefaultFontStyle() + "\">" + strActivity + "</table>";
                            string strDeliverable = oApplication.Get(intApp, "deliverables_doc");
                            strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_REQUEST_ASSIGNMENT");
                            if (strDeliverable.Trim() != "")
                                strDeliverable = "<p><a href=\"" + oVariable.URL() + strDeliverable + "\">Click here to view the service deliverables</a></p>";
                            if (oService.Get(intService, "notify_client") != "0")
                                oFunction.SendEmail("Request Assignment: " + strService, oUser.GetName(intRequester), strCC, strEMailIdsBCC, "Request Assignment: " + strService, "<p><b>The following request has been assigned...</b><p><p>" + oResourceRequest.GetSummary(intResourceParent, 0, intEnvironment, dsnServiceEditor, dsnAsset, dsnIP) + "</p><p>" + strActivity + "</p>" + strDeliverable + strNotify, true, false);
                        }
                        else
                        {
                            strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_ERROR");
                            oFunction.SendEmail("ERROR: Request Assignment", strEMailIdsBCC, "", "", "ERROR: Request Assignment", "<p><b>An ACCEPTED resource request with a SOLO status of 0 has been assigned... RRID = " + intResourceParent.ToString() + "</b></p>", true, false);
                        }
                    }
                    oResourceRequest.UpdateStatusOverall(intResourceParent, 2);
                    oResourceRequest.UpdateAssignedBy(intResourceParent, intProfile);
                }
            }
            Response.Redirect(oPage.GetFullLink(intPage) + "?action=finish");
        }
        protected void btnDelete_Click(Object Sender, EventArgs e)
        {
            int intResourceParent = Int32.Parse(lblResourceParent.Text);
            oResourceRequest.Delete(intResourceParent);
            Response.Redirect(oPage.GetFullLink(intPage) + "?action=finish");
        }
        protected void btnAssign_Click(Object Sender, EventArgs e)
        {
            int intResourceParent = Int32.Parse(lblResourceParent.Text);
            double dblHours = 0.00;
            if (txtHours.Visible == true)
                dblHours = double.Parse(txtHours.Text);
            else
            {
                int intService = Int32.Parse(lblServiceID.Text);
                double dblQuantity = double.Parse(txtQuantity.Text);
                dblHours = oServiceDetail.GetHours(intService, dblQuantity);
            }
            oResourceRequest.AddWorkflowAssign(intResourceParent, Int32.Parse(ddlUser.SelectedItem.Value), Int32.Parse(txtQuantity.Text), dblHours);
            Response.Redirect(oPage.GetFullLink(intPage) + "?rrid=" + intResourceParent.ToString() + "&assign=true");
        }
        protected void btnUpdate_Click(Object Sender, EventArgs e)
        {
            int intResourceParent = Int32.Parse(lblResourceParent.Text);
            int intService = Int32.Parse(lblServiceID.Text);
            double dblQuantity = double.Parse(txtQuantityOverall.Text);
            double dblHours = oServiceDetail.GetHours(intService, dblQuantity);
            oResourceRequest.UpdateDevices(intResourceParent, Int32.Parse(txtQuantityOverall.Text), dblHours);
            Response.Redirect(oPage.GetFullLink(intPage) + "?rrid=" + intResourceParent.ToString() + "&assign=true");
        }
        protected void btnDeleteAssign_Click(Object Sender, EventArgs e)
        {
            int intResourceParent = Int32.Parse(lblResourceParent.Text);
            LinkButton oButton = (LinkButton)Sender;
            oResourceRequest.DeleteWorkflowAssign(Int32.Parse(oButton.CommandArgument));
            Response.Redirect(oPage.GetFullLink(intPage) + "?rrid=" + intResourceParent.ToString() + "&assign=true");
        }
        protected void btnFinish_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(oPage.GetFullLinkRelated(intPage));
        }
    }
}