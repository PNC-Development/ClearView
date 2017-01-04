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
using System.IO;
namespace NCC.ClearView.Presentation.Web
{
    public partial class wm_provisioning_support : System.Web.UI.UserControl
    {
        private DataSet ds;
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        private string dsnServiceEditor = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ServiceEditorDSN"]].ConnectionString;
        private string dsnZeus = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ZeusDSN"]].ConnectionString;

        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected string strVirtual = ConfigurationManager.AppSettings["VirtualGatekeeper"];
        protected int intAssignPage = Int32.Parse(ConfigurationManager.AppSettings["AssignWorkload"]);
        protected int intResourceRequestApprove = Int32.Parse(ConfigurationManager.AppSettings["ResourceRequestApprove"]);
        protected int intViewPage = Int32.Parse(ConfigurationManager.AppSettings["ViewWorkload"]);
        protected int intMyWork = Int32.Parse(ConfigurationManager.AppSettings["MyWork"]);
        protected int intProfile;
        protected Projects oProject;
        protected Functions oFunction;
        protected Users oUser;
        protected Pages oPage;
        protected ResourceRequest oResourceRequest;
        protected RequestItems oRequestItem;
        protected Requests oRequest;
        protected Services oService;
        protected ServiceRequests oServiceRequest;
        protected RequestFields oRequestField;
        protected Applications oApplication;
        protected ServiceDetails oServiceDetail;
        protected Delegates oDelegate;
        protected Documents oDocument;

        // For server Workstation Errors
        protected Servers oServer;
        protected Workstations oWorkstation;
        protected Zeus oZeus;
        protected Asset oAsset;
        protected OnDemand oOnDemand;
        protected Errors oError;
        protected Variables oVariable;


        protected int intApplication = 0;
        protected int intPage = 0;
        protected int intProject = 0;
        protected bool boolDetails = false;
        protected bool boolExecution = false;
        protected bool boolChange = false;
        protected bool boolDocuments = false;
        protected int intRequest = 0;
        protected int intService = 0;
        protected int intItem = 0;
        protected int intNumber = 0;
    
        protected bool boolCheckboxes = false;
        protected string strCheckboxes = "";
        protected bool boolJoined = false;
        protected bool boolServiceReturned = false;

        protected string strError = "";
        protected string strMenuTab1 = "";

        protected void Page_Load(object sender, EventArgs e)
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

            // For server Workstation Errors
            oServer = new Servers(intProfile, dsn);
            oWorkstation = new Workstations(intProfile, dsn);
            oZeus = new Zeus(intProfile, dsnZeus);
            oAsset = new Asset(0, dsnAsset);
            oOnDemand = new OnDemand(intProfile, dsn);
            oError = new Errors(intProfile, dsn);
            oVariable = new Variables(intEnvironment);

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
                lblRequestedOn.Text = DateTime.Parse(oResourceRequest.Get(intResourceParent, "created")).ToString();
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
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "statusd", "<script type=\"text/javascript\">alert('Status Updates has been Added');<" + "/" + "script>");
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
                    bool boolSLABreached = false;
                    if (oService.Get(intService, "sla") != "")
                    {
                        oFunction.ConfigureToolButton(btnSLA, "/images/tool_sla");
                        int intDays = oResourceRequest.GetSLA(intResourceParent);
                        if (intDays > -99999)
                        {
                            if (intDays < 1)
                                btnSLA.Style["border"] = "solid 2px #FF0000";
                            else if (intDays < 3)
                                btnSLA.Style["border"] = "solid 2px #FF9999";
                            boolSLABreached = (intDays < 0);
                            btnSLA.Attributes.Add("onclick", "return OpenWindow('RESOURCE_REQUEST_SLA','?rrid=" + intResourceParent.ToString() + "');");
                        }
                        else
                        {
                            btnSLA.ImageUrl = "/images/tool_sla_dbl.gif";
                            btnSLA.Enabled = false;
                        }
                    }
                    else
                    {
                        btnSLA.ImageUrl = "/images/tool_sla_dbl.gif";
                        btnSLA.Enabled = false;
                    }
                    oFunction.ConfigureToolButton(btnEmail, "/images/tool_email");
                    btnEmail.Attributes.Add("onclick", "return OpenWindow('RESOURCE_REQUEST_EMAIL','?rrid=" + intResourceWorkflow.ToString() + "&type=GENERIC');");
                    dblUsed = (dblUsed / dblAllocated) * 100;
                    
                    intProject = oRequest.GetProjectNumber(intRequest);
                    hdnTab.Value = "D";
                    panWorkload.Visible = true;
                    bool boolRed = LoadStatus(intResourceWorkflow);
                    if (boolRed == false && boolSLABreached == true)
                        btnComplete.Attributes.Add("onclick", "alert('NOTE: Your Service Level Agreement (SLA) has been breached!\\n\\nYou must provide a RED STATUS update with an explanation of why your SLA was breached for this request.\\n\\nOnce a RED STATUS update has been provided, you will be able to complete this request.');return false;");
                  
                    LoadLists();
                    LoadInformation(intResourceWorkflow);
                    chkDescription.Checked = (Request.QueryString["doc"] != null);

                    //Change Control and Documents
                    LoadChange(intResourceWorkflow);
                    lblDocuments.Text = oDocument.GetDocuments_Service(intRequest, intService, oVariable.DocumentsFolder(), 1, (Request.QueryString["doc"] != null));
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
                    if ((oResourceRequest.CanUpdate(intProfile, intResourceWorkflow, false) == false) || boolComplete == true)
                    {
                        oFunction.ConfigureToolButtonRO(btnSave, btnComplete);
                        pnlSolutions.Visible = false;
                        btnNoSolution.Enabled = false;
                        btnNewSolution.Enabled = false;
                    }

                   
                   btnReturn.Visible = false;
                   btnComplete.Visible = false;
                }

                
            }
            else
                panDenied.Visible = true;
            txtIncidentUser.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'300','195','" + divIncidentUser.ClientID + "','" + lstIncidentUser.ClientID + "','" + hdnIncidentUser.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
            lstIncidentUser.Attributes.Add("ondblclick", "AJAXClickRow();");
        }


        private void LoadLists()
        {
            ddlCauseCode.DataTextField = "name";
            ddlCauseCode.DataValueField = "id";
            ddlCauseCode.DataSource = oError.GetTypes(1);
            ddlCauseCode.DataBind();
            ddlCauseCode.Items.Insert(0, new ListItem("-- SELECT --", "0"));
            ddlCauseCode.Attributes.Add("onchange", "PopulateErrorType2s('" + ddlCauseCode.ClientID + "','" + ddlCauseType.ClientID + "');");
            ddlCauseType.Attributes.Add("onchange", "UpdateDropDownHidden('" + ddlCauseType.ClientID + "','" + hdnCauseType.ClientID + "');");

        }

        private bool LoadStatus(int _resourceid)
        {
            bool boolRed = false;
            DataSet dsStatus = oResourceRequest.GetStatuss(_resourceid);
            rptStatus.DataSource = dsStatus;
            rptStatus.DataBind();
            lblNoStatus.Visible = (rptStatus.Items.Count == 0);
            double dblTotalStatus = 0.00;
            foreach (RepeaterItem ri in rptStatus.Items)
            {
                Label _status = (Label)ri.FindControl("lblStatus");
                if (boolRed == false && _status.Text == "1")
                    boolRed = true;
                double dblStatus = double.Parse(_status.Text);
                if (dblTotalStatus == 0.00)
                    dblTotalStatus = dblStatus;
                _status.Text = oResourceRequest.GetStatus(dblStatus, 50, 15);
            }
            lblStatus.Text = oResourceRequest.GetStatus(dblTotalStatus, 50, 15);
            return boolRed;
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
            if (intService == Int32.Parse(ConfigurationManager.AppSettings["SERVICEID_PROVISIONINGSUPPORT_SERVER"]))//Server =1
            {
                panIncident.Visible = true;
                lblView.Text = oServer.GetErrorDetailsBody(intRequest, oService.GetItemId(intService), intNumber, intEnvironment);
                DataSet dsError = oServer.GetErrorByRequest(intRequest, oService.GetItemId(intService), intNumber);
                if (dsError.Tables[0].Rows.Count > 0)
                {
                    lblErrorId.Text = dsError.Tables[0].Rows[0]["id"].ToString();
                    lblServerId.Text = dsError.Tables[0].Rows[0]["serverid"].ToString();
                    int intServerId = Int32.Parse(lblServerId.Text);
                    txtIncident.Text = dsError.Tables[0].Rows[0]["incident"].ToString();
                    int intAssigned = 0;
                    string strAssigned = "";
                    if (Int32.TryParse(dsError.Tables[0].Rows[0]["assigned"].ToString(), out intAssigned))
                        strAssigned = oUser.GetFullNameWithLanID(intAssigned);
                    txtIncidentUser.Text = strAssigned;
                    hdnIncidentUser.Value = dsError.Tables[0].Rows[0]["assigned"].ToString();
                    LoadServerErrors();
                }
            }

            if (intService == Int32.Parse(ConfigurationManager.AppSettings["SERVICEID_PROVISIONINGSUPPORT_WORKSTATION"])) //Workstation =2
            {
                panIncident.Visible = true;
                lblView.Text = oWorkstation.GetVirtualErrorDetailsBody(intRequest, intNumber, intEnvironment);
                DataSet dsError = oWorkstation.GetVirtualErrorsByRequest(intRequest,intNumber  );
                if (dsError.Tables[0].Rows.Count > 0)
                {
                    lblErrorId.Text = dsError.Tables[0].Rows[0]["id"].ToString();
                    lblWorkstationId.Text = dsError.Tables[0].Rows[0]["workstationid"].ToString();
                    int intWorkstationId = Int32.Parse(lblWorkstationId.Text);
                    txtIncident.Text = dsError.Tables[0].Rows[0]["incident"].ToString();
                    int intAssigned = 0;
                    string strAssigned = "";
                    if (Int32.TryParse(dsError.Tables[0].Rows[0]["assigned"].ToString(), out intAssigned))
                        strAssigned = oUser.GetFullNameWithLanID(intAssigned);
                    txtIncidentUser.Text = strAssigned;
                    hdnIncidentUser.Value = dsError.Tables[0].Rows[0]["assigned"].ToString();
                    LoadWorkstationErrors();
                }
            }

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

        private void LoadServerErrors()
        {
            panError.Visible = true;
            DataSet ds = oServer.GetErrorByRequest(intRequest, oService.GetItemId(intService), intNumber);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
               

                int intAsset = 0;
                if (dr["assetid"].ToString() != "")
                    intAsset = Int32.Parse(dr["assetid"].ToString());

                lblAsset.Text = intAsset.ToString();
                lblStep.Text = dr["step"].ToString();
                lblError.Text = dr["reason"].ToString();

                if (dr["fixed"].ToString() != "")
                {
                    strError += "<tr class=\"deletedRow\">";
                    strError += "<td></td>";
                }
                else
                {
                    strError += "<tr>";
                    strError += "<td></td>";
                }
                if (intAsset == 0)
                    strError += "<td>" + dr["servername"].ToString() + "</td>";
                else
                {
                    string strILO = oAsset.GetServerOrBlade(intAsset, "ilo");
                    if (strILO != "")
                        strError += "<td><a href=\"https://" + strILO + "\" target=\"_blank\">" + dr["servername"].ToString() + "</a></td>";
                    else
                        strError += "<td>" + dr["servername"].ToString() + "</td>";
                }
                strError += "<td>" + dr["reason"].ToString() + "</td>";
                strError += "<td><a href=\"javascript:void(0);\" onclick=\"OpenNewWindowMenu('/datapoint/service/design.aspx?id=" + oFunction.encryptQueryString(dr["designid"].ToString()) + "','800','600')\">" + dr["designid"].ToString() + "</a></td>";
                if (String.IsNullOrEmpty(dr["incident"].ToString()))
                    strError += "<td>---</td>";
                else
                    strError += "<td>" + dr["incident"].ToString() + "</td>";
                strError += "<td>" + DateTime.Parse(dr["created"].ToString()).ToString() + "</td>";
                strError += "</tr>";
            }
            strError = "<table cellpadding=\"3\" cellspacing=\"2\" width=\"100%\" border=\"0\" style=\"border:solid 1px #CCCCCC\"><tr bgcolor=\"#EEEEEE\"><td><b></b></td><td><b>Server Name</b></td><td><b>Reason</b></td><td><b>DesignID</b></td><td><b>Incident#</b></td><td><b>Created</b></td></tr>" + strError + "</table>";

            //strError = "";

            Tab oTab = new Tab("", 0, "divMenu1", true, false);
            rptRelated.DataSource = oError.Gets(lblError.Text, 0);
            rptRelated.DataBind();
            int intTab = 0;
            foreach (RepeaterItem ri in rptRelated.Items)
            {
                intTab++;
                oTab.AddTab("Solution # " + intTab.ToString(), "");
                ((Button)ri.FindControl("btnSelectExistingSolution")).Attributes.Add("onclick", "return confirm('Are you sure you want to select this solution as the fix?');");
                Label lblAttach = (Label)ri.FindControl("lblAttach");
                Panel panAttach = (Panel)ri.FindControl("panAttach");
                if (lblAttach.Text != "")
                {
                    panAttach.Visible = true;
                    string strAttach = lblAttach.Text;
                    if (strAttach.Contains("\\") == true)
                        strAttach = strAttach.Substring(strAttach.LastIndexOf("\\") + 1);
                    lblAttach.Text = "<a href=\"" + lblAttach.Text + "\" target=\"_blank\">" + strAttach + "</a>";
                }
            }
            strMenuTab1 = oTab.GetTabs();
            trNone.Visible = (rptRelated.Items.Count == 0);

            btnApplyNewSolution.Attributes.Add("onclick", "return ValidateText('" + txtProblem.ClientID + "','Please enter the problem') && ValidateText('" + txtResolution.ClientID + "','Please enter the resolution') && ValidateDropDown('" + ddlCauseCode.ClientID + "','Please select a cause code') && confirm('Are you sure you want to mark this error as fixed?') && ProcessButton(this);");
            btnNewSolution.Attributes.Add("onclick", "ShowHideDiv('" + trSolution.ClientID + "','inline');return false;");
            btnNoSolution.Attributes.Add("onclick", "return confirm('Are you sure you do not want to associate this error with any solution?');");
        }

        private void LoadWorkstationErrors()
        {
            panError.Visible = true;
            DataSet ds = oWorkstation.GetVirtualErrorsByRequest(intRequest,intNumber);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                int intAsset = 0;
                if (dr["assetid"].ToString() != "")
                    intAsset = Int32.Parse(dr["assetid"].ToString());

                lblAsset.Text = intAsset.ToString();
                lblStep.Text = dr["step"].ToString();
                lblError.Text = dr["reason"].ToString();

                if (dr["fixed"].ToString() != "")
                {
                    strError += "<tr class=\"deletedRow\">";
                    strError += "<td></td>";
                }
                else
                {
                    strError += "<tr>";
                    strError += "<td></td>";
                }
                strError += "<td>" + dr["workstationname"].ToString() + "</td>";
                strError += "<td>" + dr["reason"].ToString() + "</td>";
                strError += "<td>" + oUser.GetFullName(dr["xid"].ToString()) + "</td>";
                strError += "<td>" + dr["name"].ToString() + "</td>";
                strError += "<td>" + DateTime.Parse(dr["created"].ToString()).ToString() + "</td>";
                strError += "</tr>";
            }
            strError = "<table cellpadding=\"3\" cellspacing=\"2\" width=\"100%\" border=\"0\" style=\"border:solid 1px #CCCCCC\"><tr bgcolor=\"#EEEEEE\"><td><b></b></td><td><b>Workstation Name</b></td><td><b>Reason</b></td><td><b>Requestor</b></td><td><b>Nickname</b></td><td><b>Created</b></td></tr>" + strError + "</table>";
            //strError = "";
            Tab oTab = new Tab("", 0, "divMenu1", true, false);
            rptRelated.DataSource = oError.Gets(lblError.Text, 0);
            rptRelated.DataBind();
            int intTab = 0;
            foreach (RepeaterItem ri in rptRelated.Items)
            {
                intTab++;
                oTab.AddTab("Solution # " + intTab.ToString(), "");
                ((Button)ri.FindControl("btnSelectExistingSolution")).Attributes.Add("onclick", "return confirm('Are you sure you want to select this solution as the fix?');");
                Label lblAttach = (Label)ri.FindControl("lblAttach");
                Panel panAttach = (Panel)ri.FindControl("panAttach");
                if (lblAttach.Text != "")
                {
                    panAttach.Visible = true;
                    string strAttach = lblAttach.Text;
                    if (strAttach.Contains("\\") == true)
                        strAttach = strAttach.Substring(strAttach.LastIndexOf("\\") + 1);
                    lblAttach.Text = "<a href=\"" + lblAttach.Text + "\" target=\"_blank\">" + strAttach + "</a>";
                }
            }
            strMenuTab1 = oTab.GetTabs();
            trNone.Visible = (rptRelated.Items.Count == 0);

            btnApplyNewSolution.Attributes.Add("onclick", "return ValidateText('" + txtProblem.ClientID + "','Please enter the problem') && ValidateText('" + txtResolution.ClientID + "','Please enter the resolution') && ValidateDropDown('" + ddlCauseCode.ClientID + "','Please select a cause code') && confirm('Are you sure you want to mark this error as fixed?') && ProcessButton(this);");
            btnNewSolution.Attributes.Add("onclick", "ShowHideDiv('" + trSolution.ClientID + "','inline');return false;");
            btnNoSolution.Attributes.Add("onclick", "return confirm('Are you sure you do not want to associate this error with any solution?');");
        }


        protected void btnStatus_Click(Object Sender, EventArgs e)
        {
            int intResourceWorkflow = Int32.Parse(lblResourceWorkflow.Text);
            int intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);
            oResourceRequest.AddStatus(intResourceWorkflow, Int32.Parse(ddlStatus.SelectedItem.Value), txtComments.Text, intProfile);
            Response.Redirect(Request.Path + "?rrid=" + intResourceWorkflow.ToString() + "&div=E&status=true");
        }
        protected void btnChange_Click(Object Sender, EventArgs e)
        {
            int intResourceWorkflow = Int32.Parse(lblResourceWorkflow.Text);
            int intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);
            oResourceRequest.AddChangeControl(intResourceWorkflow, txtNumber.Text, DateTime.Parse(txtDate.Text + " " + txtTime.Text), txtChange.Text);
            Response.Redirect(Request.Path + "?rrid=" + intResourceWorkflow.ToString() + "&div=C&save=true");
        }
        protected void btnDeleteChange_Click(Object Sender, EventArgs e)
        {
            LinkButton oButton = (LinkButton)Sender;
            oResourceRequest.DeleteChangeControl(Int32.Parse(oButton.CommandArgument));
            Response.Redirect(Request.Path + "?rrid=" + lblResourceWorkflow.Text + "&div=C");
        }
        protected void btnReturn_Click(Object Sender, EventArgs e)
        {
        }
        protected void chkDescription_Change(Object Sender, EventArgs e)
        {
            if (chkDescription.Checked == true)
                Response.Redirect(Request.Path + "?rrid=" + Request.QueryString["rrid"] + "&doc=true&div=D");
            else
                Response.Redirect(Request.Path + "?rrid=" + Request.QueryString["rrid"] + "&div=D");
        }
        protected void btnSave_Click(Object Sender, EventArgs e)
        {
            int intResourceWorkflow = Int32.Parse(lblResourceWorkflow.Text);
            int intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);
            if (ddlStatus.SelectedIndex > -1 && txtComments.Text.Trim() != "")
            {
                oResourceRequest.AddStatus(intResourceWorkflow, Int32.Parse(ddlStatus.SelectedItem.Value), txtComments.Text, intProfile);
                //CVT62149 Workload Manager Red Light Status =Hold
                if (ddlStatus.SelectedValue == "1") //Red
                    oResourceRequest.UpdateWorkflowStatus(intResourceWorkflow, 5, true);
                else
                    oResourceRequest.UpdateWorkflowStatus(intResourceWorkflow, 2, true); 
            }
           
            oServiceDetail.UpdateCheckboxes(Request, intResourceWorkflow, intRequest, intItem, intNumber);
            double dblAllocated = oResourceRequest.GetDetailsHoursUsed(intRequest, intItem, intNumber, intResourceWorkflow, false);
            oResourceRequest.UpdateWorkflowAllocated(intResourceWorkflow, dblAllocated);

            oResourceRequest.UpdateWorkflowName(intResourceWorkflow, txtCustom.Text, intProfile);
            Response.Redirect(Request.Path + "?rrid=" + intResourceWorkflow.ToString() + "&div=E&save=true");
        }
        protected void btnComplete_Click(Object Sender, EventArgs e)
        {
            CompletedWorkflow();
        }

        # region Server Workstation Error

        protected void btnApplyNewSolution_Click(Object Sender, EventArgs e)
        {
            // New Solution
            int intStep = Int32.Parse(lblStep.Text);
            int intAsset = Int32.Parse(lblAsset.Text);
            string strPath = "";
            if (txtFile.FileName != "" && txtFile.PostedFile != null)
            {
                string strDirectory = oVariable.DocumentsFolder() + "errors";
                if (Directory.Exists(strDirectory) == false)
                    Directory.CreateDirectory(strDirectory);
                string strFile = txtFile.PostedFile.FileName.Trim();
                string strFileName = strFile.Substring(strFile.LastIndexOf("\\") + 1);
                string strExtension = txtFile.FileName;
                strExtension = strExtension.Substring(strExtension.LastIndexOf("."));
                strPath = strDirectory + "\\" + strFileName;
                txtFile.PostedFile.SaveAs(strPath);
            }
            int intType = 0;
            Int32.TryParse(Request.Form[hdnCauseType.UniqueID], out intType);
            if (intType == 0)
                intType = Int32.Parse(ddlCauseCode.SelectedItem.Value);
            int intError = oError.Add(lblError.Text, txtProblem.Text, txtResolution.Text, intType, strPath, intProfile);
            FixError(intStep, intAsset, intError, intProfile);
            CompletedWorkflow();
        }

        protected void btnSelectExistingSolution_Click(Object Sender, EventArgs e)
        {
            // Existing
            int intStep = Int32.Parse(lblStep.Text);
            int intAsset = Int32.Parse(lblAsset.Text);
            Button oButton = (Button)Sender;
            int intError = Int32.Parse(oButton.CommandArgument);
            FixError(intStep, intAsset, intError, intProfile);
            CompletedWorkflow();
        }

        protected void btnNoSolution_Click(Object Sender, EventArgs e)
        {     
            /// Is not valid - will not be clicked.
            int intStep = Int32.Parse(lblStep.Text);
            int intAsset = Int32.Parse(lblAsset.Text);
            FixError(intStep, intAsset, 0, intProfile);
            CompletedWorkflow();
        }

        private void FixError(int intStep, int intAsset, int intError, int intUser)
        {
            
            if (intService == Int32.Parse(ConfigurationManager.AppSettings["SERVICEID_PROVISIONINGSUPPORT_SERVER"]))//Server =1
            {
                int intServerId = Int32.Parse(lblServerId.Text);
                oServer.UpdateError(intServerId, intStep, intError, intUser, true, dsnAsset);
                if (intAsset > 0)
                {
                    string strSerial = oAsset.Get(intAsset, "serial");
                    oZeus.UpdateResults(strSerial);
                }
            }
            else if (intService ==Int32.Parse(ConfigurationManager.AppSettings["SERVICEID_PROVISIONINGSUPPORT_WORKSTATION"])) //Workstation =2
            {
                int intWorkstationId = Int32.Parse(lblWorkstationId.Text);
                oWorkstation.UpdateVirtualError(intWorkstationId, intStep, intError, intUser);
                if (intAsset > 0)
                {
                    string strSerial = oAsset.Get(intAsset, "serial");
                    oZeus.UpdateResults(strSerial);
                }
            
            }
         
        }

        private void CompletedWorkflow()
        {

            int intResourceWorkflow = Int32.Parse(lblResourceWorkflow.Text);
            int intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);

            double dblAllocated = double.Parse(oResourceRequest.GetWorkflow(intResourceWorkflow, "allocated"));
            oResourceRequest.UpdateWorkflowHoursOverwrite(intResourceWorkflow, dblAllocated);

            // Add a green / completed status if there are no updates, OR the last status is not green
            DataSet dsStatus = oResourceRequest.GetStatuss(intResourceWorkflow);
            if (dsStatus.Tables[0].Rows.Count == 0 || dsStatus.Tables[0].Rows[0]["status"].ToString() != "3")
                oResourceRequest.AddStatus(intResourceWorkflow, 3, "Completed", intProfile);
            oResourceRequest.UpdateWorkflowStatus(intResourceWorkflow, 3, true);
            oResourceRequest.CloseWorkflow(intResourceWorkflow, intEnvironment, 0, dsnServiceEditor, true, intResourceRequestApprove, intAssignPage, intViewPage, dsnAsset, dsnIP);

            //If this service was returned then update the status of next service 
            if (boolServiceReturned == true)
            {
                DataSet dsRR = oResourceRequest.GetRequestService(intRequest, intService, intNumber);
                if (dsRR.Tables[0].Rows.Count > 0)
                {
                    int intRRId = Int32.Parse(dsRR.Tables[0].Rows[0]["parent"].ToString());

                    DataSet dsRRReturn = oResourceRequest.getResourceRequestReturn(intRRId, intService, intNumber, 1,0);
                    if (dsRRReturn.Tables[0].Rows.Count > 0)
                    {
                        int intNextRRId = Int32.Parse(dsRRReturn.Tables[0].Rows[0]["NextRRId"].ToString());
                        int intNextServiceId = Int32.Parse(dsRRReturn.Tables[0].Rows[0]["NextServiceId"].ToString());
                        int intNextNumber = Int32.Parse(dsRRReturn.Tables[0].Rows[0]["NextNumber"].ToString()); ;


                        oResourceRequest.UpdateStatusRequest(intNextRRId, 2);
                        DataSet dsRRWF = oResourceRequest.GetWorkflowsParent(intNextRRId);
                        foreach (DataRow dr in dsRRWF.Tables[0].Rows)
                        {
                            int intRRWFId = Int32.Parse(dr["id"].ToString());
                            oResourceRequest.UpdateWorkflowStatus(intRRWFId, 2, true);
                        }
                    }
                }
                oResourceRequest.updateResourceRequestReturnCompleted(Int32.Parse(lblReqReturnedId.Text));

            }
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "reload", "<script type=\"text/javascript\">RefreshOpeningWindow();window.close();<" + "/" + "script>");

        }
        #endregion

        protected void btnIncident_Click(object sender, EventArgs e)
        {
            int intErrorId = Int32.Parse(lblErrorId.Text);
            if (intService == Int32.Parse(ConfigurationManager.AppSettings["SERVICEID_PROVISIONINGSUPPORT_SERVER"]))//Server =1
                oServer.UpdateError(intErrorId, txtIncident.Text, Int32.Parse(Request.Form[hdnIncidentUser.UniqueID]));
            if (intService == Int32.Parse(ConfigurationManager.AppSettings["SERVICEID_PROVISIONINGSUPPORT_WORKSTATION"])) //Workstation =2
                oWorkstation.UpdateVirtualError(intErrorId, txtIncident.Text, Int32.Parse(Request.Form[hdnIncidentUser.UniqueID]));
            Response.Redirect(Request.Url.PathAndQuery);
        }
    }
}