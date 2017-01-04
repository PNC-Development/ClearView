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
    public partial class project_request_edit : System.Web.UI.UserControl
    {
        private DataSet ds;
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected bool boolDirector = (ConfigurationManager.AppSettings["DirectorApproval"] == "1");
        protected int intWorkflowPage = Int32.Parse(ConfigurationManager.AppSettings["WorkflowSuffix"]);
        protected int intViewRequest = Int32.Parse(ConfigurationManager.AppSettings["ViewRequest"]);
        protected int intAssignPage = Int32.Parse(ConfigurationManager.AppSettings["AssignWorkload"]);
        protected int intProfile;
        protected Platforms oPlatform;
        protected Organizations oOrganization;
        protected RequestItems oRequestItem;
        protected Services oService;
        protected ProjectRequest oProjectRequest;
        protected Projects oProject;
        protected Functions oFunction;
        protected Variables oVariable;
        protected Users oUser;
        protected ProjectRequest_Approval oApprove;
        protected Requests oRequest;
        protected ResourceRequest oResourceRequest;
        protected Pages oPage;
        protected StatusLevels oStatusLevel;
        protected Documents oDocument;
        protected Segment oSegment;
        protected int intApplication = 0;
        protected int intPage = 0;
        protected string strPriority = "";
        protected bool boolDetails = false;
        protected bool boolDocuments = false;
        protected bool boolDiscussion = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["rid"] != null && Request.QueryString["rid"] != "")
                lblRequest.Text = Request.QueryString["rid"];
            if (lblRequest.Text == "")
                lblRequest.Text = "0";
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oPlatform = new Platforms(intProfile, dsn);
            oOrganization = new Organizations(intProfile, dsn);
            oRequestItem = new RequestItems(intProfile, dsn);
            oService = new Services(intProfile, dsn);
            oProjectRequest = new ProjectRequest(intProfile, dsn);
            oProject = new Projects(intProfile, dsn);
            oFunction = new Functions(intProfile, dsn, intEnvironment);
            oVariable = new Variables(intEnvironment);
            oUser = new Users(intProfile, dsn);
            oApprove = new ProjectRequest_Approval(intProfile, dsn, intEnvironment);
            oRequest = new Requests(intProfile, dsn);
            oResourceRequest = new ResourceRequest(intProfile, dsn);
            oPage = new Pages(intProfile, dsn);
            oStatusLevel = new StatusLevels();
            oDocument = new Documents(intProfile, dsn);
            oSegment = new Segment(intProfile, dsn);
            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            lblDate.Text = DateTime.Now.ToLongDateString();
            lblName.Text = oUser.Get(intProfile, "fname") + " " + oUser.Get(intProfile, "lname");
            if (!IsPostBack)
            {
                lblTitle.Text = "Project Request";
                LoadLists();
                if (lblRequest.Text != "0")
                    LoadProperties(Int32.Parse(lblRequest.Text));
                else
                    panError.Visible = true;
            }
            if (Request.QueryString["div"] != null)
            {
                switch (Request.QueryString["div"])
                {
                    case "D":
                        boolDocuments = true;
                        break;
                    case "C":
                        boolDiscussion = true;
                        break;
                }
            }
            if (boolDetails == false && boolDocuments == false && boolDiscussion == false)
                boolDetails = true;
        }
        protected void LoadLists()
        {
            ds = oPlatform.GetSystems(1);
            lstPlatformsAvailable.DataValueField = "platformid";
            lstPlatformsAvailable.DataTextField = "name";
            lstPlatformsAvailable.DataSource = ds;
            lstPlatformsAvailable.DataBind();
            ds = oOrganization.Gets(1);
            ddlOrganization.DataValueField = "organizationid";
            ddlOrganization.DataTextField = "name";
            ddlOrganization.DataSource = ds;
            ddlOrganization.DataBind();
            ddlOrganization.Items.Insert(0, new ListItem("-- SELECT --", "0"));
            ds = oService.GetTPM();
            ddlTPM.DataValueField = "serviceid";
            ddlTPM.DataTextField = "name";
            ddlTPM.DataSource = ds;
            ddlTPM.DataBind();
            ddlTPM.Items.Insert(0, new ListItem("-- SELECT --", "0"));
        }
        protected void AddAttributes()
        {
            ds = oProjectRequest.GetPlatforms(Int32.Parse(lblRequest.Text));
            for (int ii = 0; ii < lstPlatformsAvailable.Items.Count; ii++)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr["platformid"].ToString() == lstPlatformsAvailable.Items[ii].Value)
                    {
                        lstPlatformsCurrent.Items.Add(lstPlatformsAvailable.Items[ii]);
                        lstPlatformsAvailable.Items.Remove(lstPlatformsAvailable.Items[ii]);
                        ii--;
                        break;
                    }
                }
            }
            ddlOrganization.Attributes.Add("onchange", "PopulateSegments('" + ddlOrganization.ClientID + "','" + ddlSegment.ClientID + "');");
            ddlSegment.Attributes.Add("onchange", "UpdateDropDownHidden('" + ddlSegment.ClientID + "','" + hdnSegment.ClientID + "');");
            imgRequirement.Attributes.Add("onclick", "return ShowCalendar('" + txtRequirement.ClientID + "');");
            imgEndLife.Attributes.Add("onclick", "return ShowCalendar('" + txtEndLife.ClientID + "');");
            imgStart.Attributes.Add("onclick", "return ShowCalendar('" + txtStart.ClientID + "');");
            imgCompletion.Attributes.Add("onclick", "return ShowCalendar('" + txtCompletion.ClientID + "');");
            txtExecutive.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'300','195','" + divExecutive.ClientID + "','" + lstExecutive.ClientID + "','" + hdnExecutive.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
            lstExecutive.Attributes.Add("ondblclick", "AJAXClickRow();");
            txtWorking.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'300','195','" + divWorking.ClientID + "','" + lstWorking.ClientID + "','" + hdnWorking.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
            lstWorking.Attributes.Add("ondblclick", "AJAXClickRow();");
            txtManager.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'300','195','" + divManager.ClientID + "','" + lstManager.ClientID + "','" + hdnManager.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
            lstManager.Attributes.Add("ondblclick", "AJAXClickRow();");
            btnPlatformAdd.Attributes.Add("onclick", "return MoveList('" + lstPlatformsAvailable.ClientID + "','" + lstPlatformsCurrent.ClientID + "','" + hdnPlatforms.ClientID + "','" + lstPlatformsCurrent.ClientID + "');");
            lstPlatformsAvailable.Attributes.Add("ondblclick", "return MoveList('" + lstPlatformsAvailable.ClientID + "','" + lstPlatformsCurrent.ClientID + "','" + hdnPlatforms.ClientID + "','" + lstPlatformsCurrent.ClientID + "');");
            btnPlatformRemove.Attributes.Add("onclick", "return MoveList('" + lstPlatformsCurrent.ClientID + "','" + lstPlatformsAvailable.ClientID + "','" + hdnPlatforms.ClientID + "','" + lstPlatformsCurrent.ClientID + "');");
            lstPlatformsCurrent.Attributes.Add("ondblclick", "return MoveList('" + lstPlatformsCurrent.ClientID + "','" + lstPlatformsAvailable.ClientID + "','" + hdnPlatforms.ClientID + "','" + lstPlatformsCurrent.ClientID + "');");
            chkRequirement.Attributes.Add("onclick", "ShowHideDivCheck('" + divRequirement.ClientID + "',this);");
            chkEndLife.Attributes.Add("onclick", "ShowHideDivCheck('" + divEndLife.ClientID + "',this);");
            ddlInterdependency.Attributes.Add("onclick", "ShowHideDivDropDown('" + divInterdependency.ClientID + "',this,2,3);");
            txtInitiative.Attributes.Add("onfocusin", "InitiativeIn(this);");
            txtInitiative.Attributes.Add("onfocusout", "InitiativeOut(this);");
            txtInitiative.Attributes.Add("onkeypress", "return CancelEnter();");
            txtCapability.Attributes.Add("onkeypress", "return CancelEnter();");
            btnShelf.Attributes.Add("onclick", "return confirm('Are you sure you want to put this project request on HOLD?');");
            btnCancel.Attributes.Add("onclick", "return confirm('Are you sure you want to CANCEL this project request?');");
            btnShelf2.Attributes.Add("onclick", "return confirm('Are you sure you want to put this project request on HOLD?');");
            btnCancel2.Attributes.Add("onclick", "return confirm('Are you sure you want to CANCEL this project request?');");
        }
        protected void LoadProperties(int _id)
        {
            ds = oProjectRequest.Get(_id);
            if (ds.Tables[0].Rows.Count > 0)
            {
                int intRequest = Int32.Parse(ds.Tables[0].Rows[0]["requestid"].ToString());
                DateTime _modified = DateTime.Parse(ds.Tables[0].Rows[0]["modified"].ToString() == "" ? ds.Tables[0].Rows[0]["created"].ToString() : ds.Tables[0].Rows[0]["modified"].ToString());
                DataSet dsRequest = oRequest.Get(_id);
                if (dsRequest.Tables[0].Rows.Count > 0)
                {
                    int intProject = Int32.Parse(dsRequest.Tables[0].Rows[0]["projectid"].ToString());
                    // DOCUMENTS
                    btnDocuments.Attributes.Add("onclick", "return OpenWindow('DOCUMENTS_SECURE','?pid=" + intProject.ToString() + "&PR=true');");
                    chkMyDescription.Checked = (Request.QueryString["mydoc"] != null);
                    lblMyDocuments.Text = oDocument.GetDocuments_Mine(intProject, intProfile, oVariable.DocumentsFolder(), -1, (Request.QueryString["mydoc"] != null));
                    // GetDocuments(string _physical, int _projectid, int _requestid, int _userid, int _security, bool _show_description, bool _mine)
                    //lblMyDocuments.Text = oDocument.GetDocuments(Request.PhysicalApplicationPath, intProject, 0, intProfile, -1, (Request.QueryString["mydoc"] != null), true);
                    DataSet dsProject = oProject.Get(intProject);
                    bool boolApproved = oProject.IsApproved(intProject);
                    int intStatus = Int32.Parse(dsProject.Tables[0].Rows[0]["status"].ToString());
                    btnInternal.Enabled = (intStatus == 0);
                    bool boolEdit = false;
                    chkNotify.Checked = (ds.Tables[0].Rows[0]["notify"].ToString() == "1");
                    if (oRequest.Allowed(_id, 0, 0, intProfile, false) == true)
                    {
                        panActions.Visible = true;
                        panActions2.Visible = true;
                        if (boolApproved == false && intStatus == 0)
                            boolEdit = true;
                    }
                    else
                        panInfo.Visible = true;
                    if (boolEdit == true)
                    {
                        panEdit.Visible = true;
                        lblRequest.Text = _id.ToString();
                        lblName.Text = oUser.GetFullName(oUser.GetName(Int32.Parse(dsRequest.Tables[0].Rows[0]["userid"].ToString())));
                        lblDate.Text = _modified.ToLongDateString() + " (" + _modified.ToLongTimeString() + ")";
                        txtProjectTask.Text = dsProject.Tables[0].Rows[0]["name"].ToString();
                        ddlBaseDisc.SelectedValue = dsProject.Tables[0].Rows[0]["bd"].ToString();
                        int intPortfolio = Int32.Parse(dsProject.Tables[0].Rows[0]["organization"].ToString());
                        ddlOrganization.SelectedValue = intPortfolio.ToString();
                        int intSegment = Int32.Parse(dsProject.Tables[0].Rows[0]["segmentid"].ToString());
                        hdnSegment.Value = intSegment.ToString();
                        ddlSegment.Enabled = true;
                        ddlSegment.DataTextField = "name";
                        ddlSegment.DataValueField = "id";
                        ddlSegment.DataSource = oSegment.Gets(intPortfolio, 1);
                        ddlSegment.DataBind();
                        ddlSegment.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                        ddlSegment.SelectedValue = intSegment.ToString();
                        txtClarityNumber.Text = dsProject.Tables[0].Rows[0]["number"].ToString();
                        lblStatus.Text = oStatusLevel.HTML(intStatus);
                        int intExecutive = Int32.Parse(oProject.Get(intProject, "executive"));
                        txtExecutive.Text = oUser.GetFullName(intExecutive) + " (" + oUser.GetName(intExecutive) + ")";
                        hdnExecutive.Value = intExecutive.ToString();
                        int intWorking = Int32.Parse(oProject.Get(intProject, "working"));
                        txtWorking.Text = oUser.GetFullName(intWorking) + " (" + oUser.GetName(intWorking) + ")";
                        hdnWorking.Value = intWorking.ToString();
                        chkC1.Checked = (ds.Tables[0].Rows[0]["c1"].ToString() == "1");
                        if (ds.Tables[0].Rows[0]["endlife"].ToString() == "1")
                        {
                            divEndLife.Style["display"] = "inline";
                            chkEndLife.Checked = true;
                        }
                        if (ds.Tables[0].Rows[0]["endlife_date"].ToString() != "")
                            txtEndLife.Text = DateTime.Parse(ds.Tables[0].Rows[0]["endlife_date"].ToString()).ToShortDateString();
                        txtInitiative.Text = dsRequest.Tables[0].Rows[0]["description"].ToString();
                        if (ds.Tables[0].Rows[0]["req_type"].ToString() == "1")
                        {
                            divRequirement.Style["display"] = "inline";
                            chkRequirement.Checked = true;
                        }
                        if (ds.Tables[0].Rows[0]["req_date"].ToString() != "")
                            txtRequirement.Text = DateTime.Parse(ds.Tables[0].Rows[0]["req_date"].ToString()).ToShortDateString();
                        SelectValue(ddlInterdependency, ds.Tables[0].Rows[0]["interdependency"].ToString());
                        if (ddlInterdependency.SelectedIndex > 1)
                            divInterdependency.Style["display"] = "inline";
                        txtInterdependency.Text = ds.Tables[0].Rows[0]["projects"].ToString();
                        txtCapability.Text = ds.Tables[0].Rows[0]["capability"].ToString();
                        txtHours.Text = ds.Tables[0].Rows[0]["man_hours"].ToString();
                        txtStart.Text = DateTime.Parse(dsRequest.Tables[0].Rows[0]["start_date"].ToString()).ToShortDateString();
                        txtCompletion.Text = DateTime.Parse(dsRequest.Tables[0].Rows[0]["end_date"].ToString()).ToShortDateString();
                        SelectValue(ddlCapital, ds.Tables[0].Rows[0]["expected_capital"].ToString());
                        SelectValue(ddlInternal, ds.Tables[0].Rows[0]["internal_labor"].ToString());
                        SelectValue(ddlExternal, ds.Tables[0].Rows[0]["external_labor"].ToString());
                        SelectValue(ddlMaintenance, ds.Tables[0].Rows[0]["maintenance_increase"].ToString());
                        SelectValue(ddlExpenses, ds.Tables[0].Rows[0]["project_expenses"].ToString());
                        SelectValue(ddlCostAvoidance, ds.Tables[0].Rows[0]["estimated_avoidance"].ToString());
                        SelectValue(ddlSavings, ds.Tables[0].Rows[0]["estimated_savings"].ToString());
                        SelectValue(ddlRealized, ds.Tables[0].Rows[0]["realized_savings"].ToString());
                        SelectValue(ddlBusinessAvoidance, ds.Tables[0].Rows[0]["business_avoidance"].ToString());
                        SelectValue(ddlMaintenanceAvoidance, ds.Tables[0].Rows[0]["maintenance_avoidance"].ToString());
                        SelectValue(ddlReusability, ds.Tables[0].Rows[0]["asset_reusability"].ToString());
                        SelectValue(ddlInternalImpact, ds.Tables[0].Rows[0]["internal_impact"].ToString());
                        SelectValue(ddlExternalImpact, ds.Tables[0].Rows[0]["external_impact"].ToString());
                        SelectValue(ddlImpact, ds.Tables[0].Rows[0]["business_impact"].ToString());
                        SelectValue(ddlStrategic, ds.Tables[0].Rows[0]["strategic_opportunity"].ToString());
                        SelectValue(ddlAcquisition, ds.Tables[0].Rows[0]["acquisition"].ToString());
                        SelectValue(ddlCapabilities, ds.Tables[0].Rows[0]["technology_capabilities"].ToString());
                        lblTPM.Text = (ds.Tables[0].Rows[0]["tpm"].ToString() == "1" ? "Yes" : "No");
                        dsProject = oProjectRequest.GetResources(_id);
                        if (dsProject.Tables[0].Rows.Count > 0)
                        {
                            int intItem = Int32.Parse(dsProject.Tables[0].Rows[0]["itemid"].ToString());
                            int intService = Int32.Parse(dsProject.Tables[0].Rows[0]["serviceid"].ToString());
                            int intManager = Int32.Parse(dsProject.Tables[0].Rows[0]["userid"].ToString());
                            lblResourceRequest.Text = dsProject.Tables[0].Rows[0]["id"].ToString();
                            lblTPM.Text = (intItem == 0 ? "No" : "Yes");
                            if (intItem > 0 && intManager == 0)
                            {
                                // TPM
                                panManagerTPM.Visible = true;
                                ddlTPM.SelectedValue = intService.ToString();
                                btnSave.Attributes.Add("onclick", "return ValidateHidden('" + hdnExecutive.ClientID + "','" + txtExecutive.ClientID + "','Please enter the LAN ID of your executive sponsor')" +
                                    " && ValidateHidden('" + hdnWorking.ClientID + "','" + txtWorking.ClientID + "','Please enter the LAN ID of your working sponsor')" +
                                    " && (document.getElementById('" + chkEndLife.ClientID + "').checked == false || (document.getElementById('" + chkEndLife.ClientID + "').checked == true && ValidateDate('" + txtEndLife.ClientID + "','Please enter a valid end life date')))" +
                                    " && ValidateText('" + txtInitiative.ClientID + "','Please enter the initiative opportunity')" +
                                    " && ValidateList('" + lstPlatformsCurrent.ClientID + "','Please select at least one platform')" +
                                    " && (document.getElementById('" + chkRequirement.ClientID + "').checked == false || (document.getElementById('" + chkRequirement.ClientID + "').checked == true && ValidateDate('" + txtRequirement.ClientID + "','Please enter a valid requirement date')))" +
                                    " && ValidateDropDown('" + ddlInterdependency.ClientID + "','Please make a selection for the interdependency with other projects / initiatives')" +
                                    " && ValidateText('" + txtCapability.ClientID + "','Please enter a capability')" +
                                    " && ValidateNumber('" + txtHours.ClientID + "','Please enter a valid number for the estimated man hours')" +
                                    " && ValidateDate('" + txtStart.ClientID + "','Please enter a valid proposed discovery start date')" +
                                    " && ValidateDate('" + txtCompletion.ClientID + "','Please enter a valid project completion date')" +
                                    " && ValidateDropDown('" + ddlCapital.ClientID + "','Please make a selection for the expected capital cost')" +
                                    " && ValidateDropDown('" + ddlInternal.ClientID + "','Please make a selection for the internal labor')" +
                                    " && ValidateDropDown('" + ddlExternal.ClientID + "','Please make a selection for the external labor')" +
                                    " && ValidateDropDown('" + ddlMaintenance.ClientID + "','Please make a selection for the maintenance cost increase')" +
                                    " && ValidateDropDown('" + ddlExpenses.ClientID + "','Please make a selection for the project expenses')" +
                                    " && ValidateDropDown('" + ddlCostAvoidance.ClientID + "','Please make a selection for the estimated net cost avoidance')" +
                                    " && ValidateDropDown('" + ddlSavings.ClientID + "','Please make a selection for the estimated net cost savings')" +
                                    " && ValidateDropDown('" + ddlRealized.ClientID + "','Please make a selection for the realized cost savings')" +
                                    " && ValidateDropDown('" + ddlBusinessAvoidance.ClientID + "','Please make a selection for the business impact avoidance')" +
                                    " && ValidateDropDown('" + ddlMaintenanceAvoidance.ClientID + "','Please make a selection for the maintenance cost avoidance')" +
                                    " && ValidateDropDown('" + ddlReusability.ClientID + "','Please make a selection for the asset reusability')" +
                                    " && ValidateDropDown('" + ddlInternalImpact.ClientID + "','Please make a selection for the internal customer impact')" +
                                    " && ValidateDropDown('" + ddlExternalImpact.ClientID + "','Please make a selection for the external customer impact')" +
                                    " && ValidateDropDown('" + ddlImpact.ClientID + "','Please make a selection for the external customer impact')" +
                                    " && ValidateDropDown('" + ddlStrategic.ClientID + "','Please make a selection for the strategic opportunity')" +
                                    " && ValidateDropDown('" + ddlAcquisition.ClientID + "','Please make a selection for the acquisition / BIC')" +
                                    " && ValidateDropDown('" + ddlCapabilities.ClientID + "','Please make a selection for the technology capabilities')" +
                                    " && ValidateDropDown('" + ddlTPM.ClientID + "','Please select a TPM Service Type')" +
                                    ";");
                            }
                            else
                            {
                                // PM
                                panManagerPM.Visible = true;
                                lblCoordinator.Text = intManager.ToString();
                                if (intManager > 0)
                                {
                                    txtManager.Text = oUser.GetFullName(intManager) + " (" + oUser.GetName(intManager) + ")";
                                    hdnManager.Value = intManager.ToString();
                                }
                                btnSave.Attributes.Add("onclick", "return ValidateHidden('" + hdnExecutive.ClientID + "','" + txtExecutive.ClientID + "','Please enter the LAN ID of your executive sponsor')" +
                                    " && ValidateHidden('" + hdnWorking.ClientID + "','" + txtWorking.ClientID + "','Please enter the LAN ID of your working sponsor')" +
                                    " && (document.getElementById('" + chkEndLife.ClientID + "').checked == false || (document.getElementById('" + chkEndLife.ClientID + "').checked == true && ValidateDate('" + txtEndLife.ClientID + "','Please enter a valid end life date')))" +
                                    " && ValidateText('" + txtInitiative.ClientID + "','Please enter the initiative opportunity')" +
                                    " && ValidateList('" + lstPlatformsCurrent.ClientID + "','Please select at least one platform')" +
                                    " && (document.getElementById('" + chkRequirement.ClientID + "').checked == false || (document.getElementById('" + chkRequirement.ClientID + "').checked == true && ValidateDate('" + txtRequirement.ClientID + "','Please enter a valid requirement date')))" +
                                    " && ValidateDropDown('" + ddlInterdependency.ClientID + "','Please make a selection for the interdependency with other projects / initiatives')" +
                                    " && ValidateText('" + txtCapability.ClientID + "','Please enter a capability')" +
                                    " && ValidateNumber('" + txtHours.ClientID + "','Please enter a valid number for the estimated man hours')" +
                                    " && ValidateDate('" + txtStart.ClientID + "','Please enter a valid proposed discovery start date')" +
                                    " && ValidateDate('" + txtCompletion.ClientID + "','Please enter a valid project completion date')" +
                                    " && ValidateDropDown('" + ddlCapital.ClientID + "','Please make a selection for the expected capital cost')" +
                                    " && ValidateDropDown('" + ddlInternal.ClientID + "','Please make a selection for the internal labor')" +
                                    " && ValidateDropDown('" + ddlExternal.ClientID + "','Please make a selection for the external labor')" +
                                    " && ValidateDropDown('" + ddlMaintenance.ClientID + "','Please make a selection for the maintenance cost increase')" +
                                    " && ValidateDropDown('" + ddlExpenses.ClientID + "','Please make a selection for the project expenses')" +
                                    " && ValidateDropDown('" + ddlCostAvoidance.ClientID + "','Please make a selection for the estimated net cost avoidance')" +
                                    " && ValidateDropDown('" + ddlSavings.ClientID + "','Please make a selection for the estimated net cost savings')" +
                                    " && ValidateDropDown('" + ddlRealized.ClientID + "','Please make a selection for the realized cost savings')" +
                                    " && ValidateDropDown('" + ddlBusinessAvoidance.ClientID + "','Please make a selection for the business impact avoidance')" +
                                    " && ValidateDropDown('" + ddlMaintenanceAvoidance.ClientID + "','Please make a selection for the maintenance cost avoidance')" +
                                    " && ValidateDropDown('" + ddlReusability.ClientID + "','Please make a selection for the asset reusability')" +
                                    " && ValidateDropDown('" + ddlInternalImpact.ClientID + "','Please make a selection for the internal customer impact')" +
                                    " && ValidateDropDown('" + ddlExternalImpact.ClientID + "','Please make a selection for the external customer impact')" +
                                    " && ValidateDropDown('" + ddlImpact.ClientID + "','Please make a selection for the external customer impact')" +
                                    " && ValidateDropDown('" + ddlStrategic.ClientID + "','Please make a selection for the strategic opportunity')" +
                                    " && ValidateDropDown('" + ddlAcquisition.ClientID + "','Please make a selection for the acquisition / BIC')" +
                                    " && ValidateDropDown('" + ddlCapabilities.ClientID + "','Please make a selection for the technology capabilities')" +
                                    ";");
                            }
                        }
                        AddAttributes();
                    }
                    else
                    {
                        panView.Visible = true;
                        lblPrinter.Visible = (Request.QueryString["page"] == null);
                        int intRequestor = Int32.Parse(dsRequest.Tables[0].Rows[0]["userid"].ToString());
                        lblName2.Text = oUser.GetFullName(oUser.GetName(intRequestor));
                        lblDate2.Text = _modified.ToLongDateString() + " (" + _modified.ToLongTimeString() + ")";
                        lblProjectTask2.Text = dsProject.Tables[0].Rows[0]["name"].ToString();
                        lblBaseDisc2.Text = dsProject.Tables[0].Rows[0]["bd"].ToString();
                        lblOrganization2.Text = oOrganization.GetName(Int32.Parse(dsProject.Tables[0].Rows[0]["organization"].ToString()));
                        lblSegment2.Text = oSegment.GetName(Int32.Parse(dsProject.Tables[0].Rows[0]["segmentid"].ToString()));
                        lblClarity2.Text = dsProject.Tables[0].Rows[0]["number"].ToString();
                        if (lblClarity2.Text == "")
                            lblClarity2.Text = "<i>To Be Determined...</i>";
                        lblStatus2.Text = oStatusLevel.HTML(intStatus);
                        lblExecutive2.Text = oUser.GetFullName(Int32.Parse(oProject.Get(intProject, "executive")));
                        lblWorking2.Text = oUser.GetFullName(Int32.Parse(oProject.Get(intProject, "working")));
                        lblC1.Text = (ds.Tables[0].Rows[0]["c1"].ToString() == "1" ? "Yes" : "No");
                        lblEndLife.Text = (ds.Tables[0].Rows[0]["endlife"].ToString() == "1" ? "Yes" : "No");
                        if (ds.Tables[0].Rows[0]["endlife_date"].ToString() != "")
                            lblEndLifeDate.Text = DateTime.Parse(ds.Tables[0].Rows[0]["endlife_date"].ToString()).ToShortDateString();
                        lblInitiative.Text = dsRequest.Tables[0].Rows[0]["description"].ToString();
                        lblRequirement.Text = (ds.Tables[0].Rows[0]["req_type"].ToString() == "1" ? "Yes" : "No");
                        if (ds.Tables[0].Rows[0]["req_date"].ToString() != "")
                            lblRequirementDate.Text = DateTime.Parse(ds.Tables[0].Rows[0]["req_date"].ToString()).ToShortDateString();
                        lblInterdependency.Text = ds.Tables[0].Rows[0]["interdependency"].ToString();
                        lblProjects.Text = ds.Tables[0].Rows[0]["projects"].ToString();
                        lblCapability.Text = ds.Tables[0].Rows[0]["capability"].ToString();
                        lblHours.Text = ds.Tables[0].Rows[0]["man_hours"].ToString();
                        lblStart.Text = DateTime.Parse(dsRequest.Tables[0].Rows[0]["start_date"].ToString()).ToShortDateString();
                        lblCompletion.Text = DateTime.Parse(dsRequest.Tables[0].Rows[0]["end_date"].ToString()).ToShortDateString();
                        lblCapital.Text = ds.Tables[0].Rows[0]["expected_capital"].ToString();
                        lblInternal.Text = ds.Tables[0].Rows[0]["internal_labor"].ToString();
                        lblExternal.Text = ds.Tables[0].Rows[0]["external_labor"].ToString();
                        lblMaintenance.Text = ds.Tables[0].Rows[0]["maintenance_increase"].ToString();
                        lblExpenses.Text = ds.Tables[0].Rows[0]["project_expenses"].ToString();
                        lblCostAvoidance.Text = ds.Tables[0].Rows[0]["estimated_avoidance"].ToString();
                        lblSavings.Text = ds.Tables[0].Rows[0]["estimated_savings"].ToString();
                        lblRealized.Text = ds.Tables[0].Rows[0]["realized_savings"].ToString();
                        lblBusinessAvoidance.Text = ds.Tables[0].Rows[0]["business_avoidance"].ToString();
                        lblMaintenanceAvoidance.Text = ds.Tables[0].Rows[0]["maintenance_avoidance"].ToString();
                        lblReusability.Text = ds.Tables[0].Rows[0]["asset_reusability"].ToString();
                        lblInternalImpact.Text = ds.Tables[0].Rows[0]["internal_impact"].ToString();
                        lblExternalImpact.Text = ds.Tables[0].Rows[0]["external_impact"].ToString();
                        lblImpact.Text = ds.Tables[0].Rows[0]["business_impact"].ToString();
                        lblStrategic.Text = ds.Tables[0].Rows[0]["strategic_opportunity"].ToString();
                        lblAcquisition.Text = ds.Tables[0].Rows[0]["acquisition"].ToString();
                        lblCapabilities.Text = ds.Tables[0].Rows[0]["technology_capabilities"].ToString();
                        lblTPM2.Text = (ds.Tables[0].Rows[0]["tpm"].ToString() == "1" ? "Yes" : "No");
                        ds = oProjectRequest.GetPlatforms(Int32.Parse(lblRequest.Text));
                        foreach (DataRow dr in ds.Tables[0].Rows)
                            lblPlatforms.Text += dr["name"].ToString() + "<br/>";
                        dsProject = oProjectRequest.GetResources(_id);
                        if (dsProject.Tables[0].Rows.Count > 0)
                        {
                            int intItem = Int32.Parse(dsProject.Tables[0].Rows[0]["itemid"].ToString());
                            int intService = Int32.Parse(dsProject.Tables[0].Rows[0]["serviceid"].ToString());
                            int intAccepted = Int32.Parse(dsProject.Tables[0].Rows[0]["accepted"].ToString());
                            int intManager = Int32.Parse(dsProject.Tables[0].Rows[0]["userid"].ToString());
                            if (intItem > 0)
                            {
                                lblTPMService.Text = oService.GetName(intService);
                                panTPM.Visible = true;
                            }
                            panPM.Visible = true;
                            if (intAccepted == -1)
                                lblPM.Text = "Pending Assignment (" + oUser.GetFullName(intRequestor) + ")";
                            else if (intManager > 0)
                                lblPM.Text = oUser.GetFullName(intManager);
                            else if (intItem > 0)
                                lblPM.Text = "Pending Assignment";
                        }
                    }
                }
                panShow.Visible = true;
                // PRIORITY
                strPriority = "<div align=\"center\">" + oProjectRequest.GetPriority(_id, intEnvironment) + "</div>";
                // DISCUSSION
                int intUser = Int32.Parse(oRequest.Get(_id, "userid"));
                if (intUser == intProfile)
                {
                    lblUser.Text = oUser.GetFullName(oUser.GetName(intProfile));
                    panDiscussion.Visible = true;
                }
                rptComments.DataSource = oProjectRequest.GetComments(_id);
                rptComments.DataBind();
                foreach (RepeaterItem ri in rptComments.Items)
                {
                    LinkButton oDelete = (LinkButton)ri.FindControl("btnDelete");
                    oDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this comment?');");
                }
                lblNoComments.Visible = (rptComments.Items.Count == 0);
                // HISTORY
                rptHistory.DataSource = oApprove.Get(_id);
                rptHistory.DataBind();
                foreach (RepeaterItem ri in rptHistory.Items)
                {
                    Label oStep = (Label)ri.FindControl("lblStep");
                    switch (oStep.Text)
                    {
                        case "1":
                            oStep.Text = "Manager";
                            break;
                        case "2":
                            oStep.Text = "Platform";
                            break;
                        case "3":
                            oStep.Text = "Board";
                            break;
                        case "4":
                            oStep.Text = "Director";
                            break;
                    }
                    Label oUserId = (Label)ri.FindControl("lblUserId");
                    intUser = Int32.Parse(oUserId.Text);
                    if (intUser > 0)
                        oUserId.Text = oUser.GetFullName(intUser);
                    else
                        oUserId.Text = "---";
                    Label oImage = (Label)ri.FindControl("lblImage");
                    Label oModified = (Label)ri.FindControl("lblModified");
                    Label oApproval = (Label)ri.FindControl("lblApproval");
                    switch (oApproval.Text)
                    {
                        case "-100":
                            oApproval.Text = "<span class=\"expedited\">EXPEDITED</span>";
                            oModified.Text = "Not Available";
                            oModified.CssClass = "pending";
                            break;
                        case "-10":
                            oApproval.Text = "<span class=\"waiting\">Waiting</span>";
                            break;
                        case "-1":
                            oApproval.Text = "<span class=\"denied\">Denied</span>";
                            break;
                        case "0":
                            oApproval.Text = "<span class=\"pending\">Pending</span>";
                            oImage.Text = "<img src=\"/images/green_right.gif\" border=\"0\" align=\"absmiddle\">&nbsp;";
                            break;
                        case "1":
                            oApproval.Text = "<span class=\"approved\">Approved</span>";
                            break;
                        case "10":
                            oApproval.Text = "<span class=\"shelved\">Shelved</span>";
                            break;
                        case "100":
                            if (oStep.Text == "Platform")
                                oApproval.Text = "<span class=\"pending\">Already Approved</span>";
                            else
                                oApproval.Text = "<span class=\"pending\">Majority Voted</span>";
                            oModified.Text = "Not Available";
                            oModified.CssClass = "pending";
                            break;
                    }
                }
            }
            else
                panError.Visible = true;
        }
        protected void chkMyDescription_Change(Object Sender, EventArgs e)
        {
            if (chkMyDescription.Checked == true)
                Response.Redirect(oPage.GetFullLink(intPage) + "?rid=" + lblRequest.Text + "&mydoc=true&div=D");
            else
                Response.Redirect(oPage.GetFullLink(intPage) + "?rid=" + lblRequest.Text + "&div=D");
        }
        protected void btnSave_Click(Object Sender, EventArgs e)
        {
            string strPlatforms = Request.Form[hdnPlatforms.UniqueID];
            double dblExpected = 0.00;
            double dblExpectedPercent = .2;
            double dblExpectedTotal = 5.00;
            double dblAvoidance = 0.00;
            double dblAvoidancePercent = .1666666667;
            double dblAvoidanceTotal = 5.00;
            double dblImpact = 0.00;
            double dblImpactPercent = .1666666667;
            double dblImpactTotal = 5.00;

            dblExpected += (dblExpectedPercent * double.Parse(ddlCapital.SelectedItem.Value));
            double dblInternal = double.Parse(ddlInternal.SelectedItem.Value);
            if (dblInternal < 0)
                dblInternal = 1.00;
            dblExpected += (dblExpectedPercent * dblInternal);
            double dblExternal = double.Parse(ddlExternal.SelectedItem.Value);
            if (dblExternal < 0)
                dblExternal = 1.00;
            dblExpected += (dblExpectedPercent * dblExternal);
            dblExpected += (dblExpectedPercent * double.Parse(ddlMaintenance.SelectedItem.Value));
            dblExpected += (dblExpectedPercent * double.Parse(ddlExpenses.SelectedItem.Value));
            dblExpected = dblExpected / dblExpectedTotal;

            dblAvoidance += (dblAvoidancePercent * double.Parse(ddlCostAvoidance.SelectedItem.Value));
            dblAvoidance += (dblAvoidancePercent * double.Parse(ddlSavings.SelectedItem.Value));
            dblAvoidance += (dblAvoidancePercent * double.Parse(ddlRealized.SelectedItem.Value));
            dblAvoidance += (dblAvoidancePercent * double.Parse(ddlBusinessAvoidance.SelectedItem.Value));
            dblAvoidance += (dblAvoidancePercent * double.Parse(ddlMaintenanceAvoidance.SelectedItem.Value));
            dblAvoidance += (dblAvoidancePercent * double.Parse(ddlReusability.SelectedItem.Value));
            dblAvoidance = dblAvoidance / dblAvoidanceTotal;

            dblImpact += (dblImpactPercent * double.Parse(ddlInternalImpact.SelectedItem.Value));
            dblImpact += (dblImpactPercent * double.Parse(ddlExternalImpact.SelectedItem.Value));
            dblImpact += (dblImpactPercent * double.Parse(ddlImpact.SelectedItem.Value));
            dblImpact += (dblImpactPercent * double.Parse(ddlStrategic.SelectedItem.Value));
            dblImpact += (dblImpactPercent * double.Parse(ddlAcquisition.SelectedItem.Value));
            dblImpact += (dblImpactPercent * double.Parse(ddlCapabilities.SelectedItem.Value));
            dblImpact = dblImpact / dblImpactTotal;
            bool boolResend = false;
            int _id = Int32.Parse(lblRequest.Text);
            ds = oProjectRequest.Get(_id);
            int intRequest = Int32.Parse(ds.Tables[0].Rows[0]["requestid"].ToString());
            int intProject = oRequest.GetProjectNumber(intRequest);
            DataSet dsRequest = oRequest.Get(_id);
            DataSet dsProject = new DataSet();
            if (intProject > 0)
                dsProject = oProject.Get(intProject);
            // Store data in variables to recover and compare
            string strName = dsProject.Tables[0].Rows[0]["name"].ToString();
            string strBD = dsProject.Tables[0].Rows[0]["bd"].ToString();
            string strOrg = dsProject.Tables[0].Rows[0]["organization"].ToString();
            string strNumber = dsProject.Tables[0].Rows[0]["number"].ToString();
            string strExecutive = dsProject.Tables[0].Rows[0]["executive"].ToString();
            string strWorking = dsProject.Tables[0].Rows[0]["working"].ToString();
            string strC1 = ds.Tables[0].Rows[0]["c1"].ToString();
            string strEndLife = ds.Tables[0].Rows[0]["endlife"].ToString();
            string strEndLifeDate = ds.Tables[0].Rows[0]["endlife_date"].ToString();
            if (strEndLifeDate != "")
                strEndLifeDate = DateTime.Parse(strEndLifeDate).ToShortDateString();
            string strInitiative = dsRequest.Tables[0].Rows[0]["description"].ToString();
            string strReqType = ds.Tables[0].Rows[0]["req_type"].ToString();
            string strReqDate = ds.Tables[0].Rows[0]["req_date"].ToString();
            if (strReqDate != "")
                strReqDate = DateTime.Parse(strReqDate).ToShortDateString();
            string strInterdependency = ds.Tables[0].Rows[0]["interdependency"].ToString();
            string strProjects = ds.Tables[0].Rows[0]["projects"].ToString();
            string strCapability = ds.Tables[0].Rows[0]["capability"].ToString();
            string strHours = ds.Tables[0].Rows[0]["man_hours"].ToString();
            string strStart = DateTime.Parse(dsRequest.Tables[0].Rows[0]["start_date"].ToString()).ToShortDateString();
            string strEnd = DateTime.Parse(dsRequest.Tables[0].Rows[0]["end_date"].ToString()).ToShortDateString();
            string strCapital = ds.Tables[0].Rows[0]["expected_capital"].ToString();
            string strInternal = ds.Tables[0].Rows[0]["internal_labor"].ToString();
            string strExternal = ds.Tables[0].Rows[0]["external_labor"].ToString();
            string strMaintenance = ds.Tables[0].Rows[0]["maintenance_increase"].ToString();
            string strExpenses = ds.Tables[0].Rows[0]["project_expenses"].ToString();
            string strAvoidance = ds.Tables[0].Rows[0]["estimated_avoidance"].ToString();
            string strSavings = ds.Tables[0].Rows[0]["estimated_savings"].ToString();
            string strRealized = ds.Tables[0].Rows[0]["realized_savings"].ToString();
            string strBusinessAviodance = ds.Tables[0].Rows[0]["business_avoidance"].ToString();
            string strMaintenanceAvoidance = ds.Tables[0].Rows[0]["maintenance_avoidance"].ToString();
            string strReusability = ds.Tables[0].Rows[0]["asset_reusability"].ToString();
            string strInternalImpact = ds.Tables[0].Rows[0]["internal_impact"].ToString();
            string strExternalImpact = ds.Tables[0].Rows[0]["external_impact"].ToString();
            string strImpact = ds.Tables[0].Rows[0]["business_impact"].ToString();
            string strStrategic = ds.Tables[0].Rows[0]["strategic_opportunity"].ToString();
            string strAcquisition = ds.Tables[0].Rows[0]["acquisition"].ToString();
            string strCapabilities = ds.Tables[0].Rows[0]["technology_capabilities"].ToString();
            // Update Project
            if (ddlBaseDisc.SelectedItem.Value != strBD || txtClarityNumber.Text != strNumber)
                boolResend = true;
            int intSegment = 0;
            if (Request.Form[hdnSegment.UniqueID] != "")
                intSegment = Int32.Parse(Request.Form[hdnSegment.UniqueID]);
            oProject.Update(intProject, txtProjectTask.Text, ddlBaseDisc.SelectedItem.Value, txtClarityNumber.Text, intProfile, Int32.Parse(ddlOrganization.SelectedItem.Value), intSegment, 0);
            // Update Request
            if (txtInitiative.Text != strInitiative)
                boolResend = true;
            oRequest.UpdateTask(intRequest, txtInitiative.Text, DateTime.Parse(txtStart.Text), DateTime.Parse(txtCompletion.Text));
            // Update Project Coordinator
            int intResourceWorkflow = Int32.Parse(lblResourceRequest.Text);
            int intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);
            string strManager = Request.Form[hdnManager.UniqueID];
            if (strManager != "")
            {
                int intManager = Int32.Parse(strManager);
                if (intManager.ToString() != lblCoordinator.Text)
                {
                    // Send notification to new project and old coordinators
                    oResourceRequest.UpdateAccepted(intResourceParent, 0);
                    oResourceRequest.UpdateWorkflowAssigned(intResourceWorkflow, intManager);
                }
            }
            else
            {
                if (ddlTPM.SelectedIndex > 0)
                {
                    int intOldItem = Int32.Parse(oResourceRequest.Get(intResourceParent, "itemid"));
                    int intNewService = Int32.Parse(ddlTPM.SelectedItem.Value);
                    int intNewItem = oService.GetItemId(intNewService);
                    oResourceRequest.UpdateItemAndService(intResourceParent, intNewItem, intNewService);
                    oResourceRequest.UpdateManager(intResourceParent, intRequest, intNewItem, 0, 0, 0, 0, 2);
                    ServiceRequests oServiceRequest = new ServiceRequests(intProfile, dsn);
                    //                oServiceRequest.NotifyTeamLead(intNewItem, intRRId, intAssignPage, intEnvironment, strWorkflowBCC);
                    //                oFunction.SendEmail("ClearView Project Request Update", oUser.GetName(oRequestItem.GetItemManager(intOldItem)), "", strWorkflowBCC, "ClearView Project Request Update", "<p><b>The Technical Project Management Service Type has been changed from &quot;" + oRequestItem.GetItemName(intOldItem) + "&quot; to &quot;" + oRequestItem.GetItemName(intNewItem) + "&quot; for following project request...</b></p><p>" + oProjectRequest.GetBody(_id, intEnvironment, true) + "</p>", true, false);
                }
            }
            // Update Sponsors
            int intExecutive = Int32.Parse(Request.Form[hdnExecutive.UniqueID]);
            if (intExecutive.ToString() != strExecutive)
            {
                // Send notification to new project and old exeuctive sponsor
                // NOTIFICATION
                // oFunction.SendEmail("ClearView Project Request Update", oUser.GetName(intExecutive), oUser.GetName(Int32.Parse(strExecutive)), strWorkflowBCC, "ClearView Project Request Update", "<p><b>The Executive Sponsor has been changed from " + oUser.GetFullName(Int32.Parse(strExecutive)) + " to " + oUser.GetFullName(intExecutive) + " for following project request...</b></p><p>" + oProjectRequest.GetBody(_id, intEnvironment, true) + "</p>", true, false);
                string strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_PROJECT");
                oFunction.SendEmail("ClearView Project Request Update", "", "", strEMailIdsBCC, "ClearView Project Request Update", "<p>TO: " + oUser.GetName(intExecutive) + "<br/>CC: " + oUser.GetName(Int32.Parse(strExecutive)) + "<b>The Executive Sponsor has been changed from " + oUser.GetFullName(Int32.Parse(strExecutive)) + " to " + oUser.GetFullName(intExecutive) + " for following project request...</b></p><p>" + oProjectRequest.GetBody(_id, intEnvironment, true) + "</p>", true, false);
            }
            int intWorking = Int32.Parse(Request.Form[hdnWorking.UniqueID]);
            if (intWorking.ToString() != strWorking)
            {
                // Send notification to new project and old working sponsor
                // NOTIFICATION
                // oFunction.SendEmail("ClearView Project Request Update", oUser.GetName(intWorking), oUser.GetName(Int32.Parse(strWorking)), strWorkflowBCC, "ClearView Project Request Update", "<p><b>The Working Sponsor has been changed from " + oUser.GetFullName(Int32.Parse(strWorking)) + " to " + oUser.GetFullName(intWorking) + " for following project request...</b></p><p>" + oProjectRequest.GetBody(_id, intEnvironment, true) + "</p>", true, false);
                string strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_PROJECT");
                oFunction.SendEmail("ClearView Project Request Update", "", "", strEMailIdsBCC, "ClearView Project Request Update", "<p>TO: " + oUser.GetName(intWorking) + "<br/>CC: " + oUser.GetName(Int32.Parse(strWorking)) + "<b>The Working Sponsor has been changed from " + oUser.GetFullName(Int32.Parse(strWorking)) + " to " + oUser.GetFullName(intWorking) + " for following project request...</b></p><p>" + oProjectRequest.GetBody(_id, intEnvironment, true) + "</p>", true, false);
            }
            oProject.Update(intProject, 0, Int32.Parse(Request.Form[hdnExecutive.UniqueID]), Int32.Parse(Request.Form[hdnWorking.UniqueID]), 0, 0, 0);
            if (intRequest > 0)
            {
                if ((chkC1.Checked ? "1" : "0") != strC1 || (chkEndLife.Checked ? "1" : "0") != strEndLife || (chkRequirement.Checked ? "1" : "0") != strReqType || strInterdependency != ddlInterdependency.SelectedItem.Text || txtHours.Text != strHours || strCapital != ddlCapital.SelectedItem.Text || strInternal != ddlInternal.SelectedItem.Text || strExternal != ddlExternal.SelectedItem.Text || strMaintenance != ddlMaintenance.SelectedItem.Text || strExpenses != ddlExpenses.SelectedItem.Text || strAvoidance != ddlCostAvoidance.SelectedItem.Text || strSavings != ddlSavings.SelectedItem.Text || strRealized != ddlRealized.SelectedItem.Text || strBusinessAviodance != ddlBusinessAvoidance.SelectedItem.Text || strMaintenanceAvoidance != ddlMaintenanceAvoidance.SelectedItem.Text || strReusability != ddlReusability.SelectedItem.Text || strInternalImpact != ddlInternalImpact.SelectedItem.Text || strExternalImpact != ddlExternalImpact.SelectedItem.Text || strImpact != ddlImpact.SelectedItem.Text || strStrategic != ddlStrategic.SelectedItem.Text || strAcquisition != ddlAcquisition.SelectedItem.Text || strCapabilities != ddlCapabilities.SelectedItem.Text)
                    boolResend = true;
                oProjectRequest.Update(intRequest, (chkRequirement.Checked ? 1 : 0), (chkRequirement.Checked ? txtRequirement.Text : ""), ddlInterdependency.SelectedItem.Text, txtInterdependency.Text, txtCapability.Text, Int32.Parse(txtHours.Text), ddlCapital.SelectedItem.Text, ddlInternal.SelectedItem.Text, ddlExternal.SelectedItem.Text, ddlMaintenance.SelectedItem.Text, ddlExpenses.SelectedItem.Text, ddlCostAvoidance.SelectedItem.Text, ddlSavings.SelectedItem.Text, ddlRealized.SelectedItem.Text, ddlBusinessAvoidance.SelectedItem.Text, ddlMaintenanceAvoidance.SelectedItem.Text, ddlReusability.SelectedItem.Text, ddlInternalImpact.SelectedItem.Text, ddlExternalImpact.SelectedItem.Text, ddlImpact.SelectedItem.Text, ddlStrategic.SelectedItem.Text, ddlAcquisition.SelectedItem.Text, ddlCapabilities.SelectedItem.Text, (chkC1.Checked ? 1 : 0), (chkEndLife.Checked ? 1 : 0), (chkEndLife.Checked ? txtEndLife.Text : ""), (lblTPM.Text == "Yes" ? 1 : 0));
                if (chkRequirement.Checked == true || chkEndLife.Checked == true)
                    oProjectRequest.AddPriority(intRequest, 1.00, 1.00, 1.00);
                else
                    oProjectRequest.AddPriority(intRequest, dblExpected, dblAvoidance, dblImpact);
                lblThanks.Text = "<b>Your request has been updated!!</b>";
            }
            if (strPlatforms != "")
            {
                oProjectRequest.DeletePlatforms(intRequest);
                while (strPlatforms != "")
                {
                    string strField = strPlatforms.Substring(0, strPlatforms.IndexOf("&"));
                    strPlatforms = strPlatforms.Substring(strPlatforms.IndexOf("&") + 1);
                    int intOrder = Int32.Parse(strField.Substring(strField.IndexOf("_") + 1));
                    strField = strField.Substring(0, strField.IndexOf("_"));
                    oProjectRequest.AddPlatform(intRequest, Int32.Parse(strField));
                }
            }
            if (boolResend == true)
            {
                oApprove.NewRequest(intRequest, intProfile, false, intWorkflowPage, boolDirector);
                lblResent.Visible = true;
            }
            lblRequest.Text = intRequest.ToString();
            panEdit.Visible = false;
            panShow.Visible = false;
            panFinish.Visible = true;
        }
        protected void SelectValue(DropDownList _ddl, string _value)
        {
            foreach (ListItem _item in _ddl.Items)
            {
                if (_item.Text == _value)
                {
                    _ddl.SelectedValue = _item.Value;
                    break;
                }
            }
        }
        protected void btnCancel_Click(Object Sender, EventArgs e)
        {
            lblAction.Text = "Project Request Cancelled";
            UpdateRequest(-2);
            lblSubAction.Text = "This project request has been cancelled.  An email has been distributed to all persons involved with this initiative.";
            panEdit.Visible = false;
            panShow.Visible = false;
            panAction.Visible = true;
        }
        protected void btnShelf_Click(Object Sender, EventArgs e)
        {
            lblAction.Text = "Project Request on Hold";
            UpdateRequest(5);
            lblSubAction.Text = "This project request has been put on hold.  An email has been distributed to all persons involved with this initiative.";
            panEdit.Visible = false;
            panShow.Visible = false;
            panAction.Visible = true;
        }
        protected void UpdateRequest(int _status)
        {
            string strStatus = "PUT ON HOLD";
            if (_status == -2)
                strStatus = "CANCELLED";
            int _id = Int32.Parse(lblRequest.Text);
            ds = oProjectRequest.Get(_id);
            int intRequest = Int32.Parse(ds.Tables[0].Rows[0]["requestid"].ToString());
            DataSet dsRequest = oRequest.Get(_id);
            int intProject = Int32.Parse(dsRequest.Tables[0].Rows[0]["projectid"].ToString());
            DataSet dsProject = oProject.Get(intProject);
            bool boolApproved = oProject.IsApproved(intProject);
            string strTo = "";
            string strUser = oUser.GetName(intProfile);
            if (strUser != "")
                strTo += strUser + ";";
            int intExecutive = Int32.Parse(oProject.Get(intProject, "executive"));
            if (intExecutive > 0)
                strTo += oUser.GetName(intExecutive) + ";";
            int intWorking = Int32.Parse(oProject.Get(intProject, "working"));
            if (intWorking > 0)
                strTo += oUser.GetName(intWorking) + ";";
            ds = oResourceRequest.GetWorkflowProjectAll(intProject);
            string strCC = "";
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (Int32.Parse(dr["userid"].ToString()) > 0 && Int32.Parse(dr["accepted"].ToString()) > -1 && Int32.Parse(dr["status"].ToString()) != (int)ResourceRequestStatus.Closed && Int32.Parse(dr["status"].ToString()) != (int)ResourceRequestStatus.Denied && Int32.Parse(dr["status"].ToString()) != (int)ResourceRequestStatus.Cancelled)
                {
                    strUser = oUser.GetName(Int32.Parse(dr["userid"].ToString()));
                    if (strUser != "")
                        strCC += strUser + ";";
                    oResourceRequest.UpdateWorkflowStatus(Int32.Parse(dr["id"].ToString()), _status, true);
                }
            }
            oProject.Update(intProject, _status);
            string strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_PROJECT");
            oFunction.SendEmail("ClearView Project Request " + strStatus, strTo, strCC, strEMailIdsBCC, "ClearView Project Request " + strStatus, "<p><b>The following project request has been " + strStatus + " by " + oUser.GetFullName(intProfile) + "...</b></p><p>" + oProjectRequest.GetBody(_id, intEnvironment, true) + "</p>", true, false);
        }
        protected void btnClose_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path + "/index.aspx");
        }
        protected void btnFinish_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path + "/index.aspx");
        }
        protected void btnInternal_Click(Object Sender, EventArgs e)
        {
            oProjectRequest.AddComment(Int32.Parse(lblRequest.Text), intProfile, txtInternal.Text, intEnvironment,  intViewRequest);
            Response.Redirect(oPage.GetFullLink(intPage) + "?rid=" + lblRequest.Text + "&div=C");
        }
        protected void btnDelete_Click(Object Sender, EventArgs e)
        {
            LinkButton oDelete = (LinkButton)Sender;
            oProjectRequest.DeleteComment(Int32.Parse(oDelete.CommandArgument));
            Response.Redirect(oPage.GetFullLink(intPage) + "?rid=" + lblRequest.Text + "&div=C");
        }
        protected void btnNotify_Click(Object Sender, EventArgs e)
        {
            oProjectRequest.Update(Int32.Parse(lblRequest.Text), (chkNotify.Checked ? 1 : 0));
            Response.Redirect(oPage.GetFullLink(intPage) + "?rid=" + lblRequest.Text + "&div=C");
        }
    }
}