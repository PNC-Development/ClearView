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
    public partial class project_request_new : System.Web.UI.UserControl
    {

        private DataSet ds;
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected bool boolDirector = (ConfigurationManager.AppSettings["DirectorApproval"] == "1");
        protected int intWorkflowPage = Int32.Parse(ConfigurationManager.AppSettings["WorkflowSuffix"]);
        protected int intResourceRequestPage = Int32.Parse(ConfigurationManager.AppSettings["NewResourceRequest"]);
        protected string strWorkflowBCC = ConfigurationManager.AppSettings["WorkflowBCC"];
        protected int intProfile;
        protected Platforms oPlatform;
        protected Organizations oOrganization;
        protected ProjectRequest oProjectRequest;
        protected Requests oRequest;
        protected Projects oProject;
        protected Functions oFunction;
        protected Variables oVariable;
        protected Applications oApplication;
        protected Pages oPage;
        protected AppPages oAppPage;
        protected Users oUser;
        protected ProjectRequest_Approval oApprove;
        protected ResourceRequest oResourceRequest;
        protected RequestItems oRequestItem;
        protected Services oService;
        protected int intApplication = 0;
        protected int intPage = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            Users oUser = new Users(intProfile, dsn);
            oPlatform = new Platforms(intProfile, dsn);
            oOrganization = new Organizations(intProfile, dsn);
            oProjectRequest = new ProjectRequest(intProfile, dsn);
            oRequest = new Requests(intProfile, dsn);
            oProject = new Projects(intProfile, dsn);
            oFunction = new Functions(intProfile, dsn, intEnvironment);
            oVariable = new Variables(intEnvironment);
            oApplication = new Applications(intProfile, dsn);
            oPage = new Pages(intProfile, dsn);
            oAppPage = new AppPages(intProfile, dsn);
            oApprove = new ProjectRequest_Approval(intProfile, dsn, intEnvironment);
            oResourceRequest = new ResourceRequest(intProfile, dsn);
            oRequestItem = new RequestItems(intProfile, dsn);
            oService = new Services(intProfile, dsn);
            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            lblDate.Text = DateTime.Now.ToLongDateString();
            lblName.Text = oUser.Get(intProfile, "fname") + " " + oUser.Get(intProfile, "lname");
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
            radTPMYes.Attributes.Add("onclick", "ShowHideDiv('" + divTPMYes.ClientID + "','inline');ShowHideDiv('" + divTPMNo.ClientID + "','none');");
            radTPMNo.Attributes.Add("onclick", "ShowHideDiv('" + divTPMNo.ClientID + "','inline');ShowHideDiv('" + divTPMYes.ClientID + "','none');");
            ddlInterdependency.Attributes.Add("onclick", "ShowHideDivDropDown('" + divInterdependency.ClientID + "',this,2,3);");
            btnPName.Attributes.Add("onclick", "return ShowProjectInfo('" + txtProjectTask.ClientID + "','" + ddlBaseDisc.ClientID + "','" + ddlOrganization.ClientID + "','" + txtClarityNumber.ClientID + "','" + txtProjectTask.ClientID + "','PNAME_SEARCH_NOCV');");
            txtProjectTask.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnPName.ClientID + "').click();return false;}} else {return true}; ");
            btnPNumber.Attributes.Add("onclick", "return ShowProjectInfo('" + txtProjectTask.ClientID + "','" + ddlBaseDisc.ClientID + "','" + ddlOrganization.ClientID + "','" + txtClarityNumber.ClientID + "','" + txtClarityNumber.ClientID + "','PNUMBER_SEARCH_NOCV');");
            txtClarityNumber.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnPNumber.ClientID + "').click();return false;}} else {return true}; ");
            txtInitiative.Attributes.Add("onfocusin", "InitiativeIn(this);");
            txtInitiative.Attributes.Add("onfocusout", "InitiativeOut(this);");
            txtInitiative.Attributes.Add("onkeypress", "return CancelEnter();");
            txtCapability.Attributes.Add("onkeypress", "return CancelEnter();");
            lblInvalid.Visible = false;
            lblTitle.Text = "New Project Request";
            if (Request.QueryString["rid"] != null && Request.QueryString["rid"] != "")
            {
                panFinish.Visible = true;
                lblRequest.Text = Request.QueryString["rid"];
            }
            else if (Request.QueryString["pid"] != null && Request.QueryString["pid"] != "")
            {
                lblProject.Text = Request.QueryString["pid"];
                int intProject = Int32.Parse(lblProject.Text);
                txtInitiative.Text = "(Number of Devices) (Description of Project/Problem)";
                LoadLists();
                panForm.Visible = true;
                btnDocuments.Attributes.Add("onclick", "return OpenWindow('DOCUMENTS_SECURE','?pid=" + intProject.ToString() + "&PR=true');");
            }
            else
            {
                if (!IsPostBack)
                {
                    LoadLists();
                    panIntro.Visible = true;
                    txtProjectTask.Focus();
                }
            }
            btnSubmit.Attributes.Add("onclick", "return ValidateText('" + txtProjectTask.ClientID + "','Please enter a project or task name') && ValidateDropDown('" + ddlBaseDisc.ClientID + "','Please choose if this is a base or discretionary project') && ValidateDropDown('" + ddlOrganization.ClientID + "','Please choose the organization sponsoring this initiative') && (document.getElementById('" + ddlBaseDisc.ClientID + "').selectedIndex == 1 || ValidateText('" + txtClarityNumber.ClientID + "','Please enter a clarity number'));");
            btnSave.Attributes.Add("onclick", "return ValidateHidden('" + hdnExecutive.ClientID + "','" + txtExecutive.ClientID + "','Please enter the LAN ID of your executive sponsor')" +
                " && ValidateHidden('" + hdnWorking.ClientID + "','" + txtWorking.ClientID + "','Please enter the LAN ID of your working sponsor')" +
                " && (document.getElementById('" + chkEndLife.ClientID + "').checked == false || (document.getElementById('" + chkEndLife.ClientID + "').checked == true && ValidateDate('" + txtEndLife.ClientID + "','Please enter a valid end life date')))" +
                " && ValidateText('" + txtInitiative.ClientID + "','Please enter the initiative opportunity')" +
                " && EnsureInitiative('" + txtInitiative.ClientID + "')" +
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
                " && ValidateRadioButtons('" + radTPMYes.ClientID + "','" + radTPMNo.ClientID + "','Please select whether or not you require a technical project manager')" +
                " && (document.getElementById('" + radTPMYes.ClientID + "').checked == false || (document.getElementById('" + radTPMYes.ClientID + "').checked == true && ValidateDropDown('" + ddlTPM.ClientID + "','Please select a TPM Service Type')))" +
                " && (document.getElementById('" + radTPMNo.ClientID + "').checked == false || (document.getElementById('" + radTPMNo.ClientID + "').checked == true && ValidateHidden('" + hdnManager.ClientID + "','" + txtManager.ClientID + "','Please enter the LAN ID of the project coordinator')))" +
                ";");
            btnClose.Attributes.Add("onclick", "return CloseWindow();");
            btnDiscretionary.Attributes.Add("onclick", "return CloseWindow();");
        }
        private void LoadLists()
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
        protected void btnSubmit_Click(Object Sender, EventArgs e)
        {
            int intSegment = 0;
            if (Request.Form[hdnSegment.UniqueID] != "")
                intSegment = Int32.Parse(Request.Form[hdnSegment.UniqueID]);
            oUser = new Users(intProfile, dsn);
            if (ddlBaseDisc.SelectedItem.Text == "Base")
            {
                bool boolDuplicate = false;
                bool boolInvalid = false;
                DataSet dsRequest;
                if (txtClarityNumber.Text.Trim() != "")
                {
                    if (txtClarityNumber.Text.Trim().ToUpper().StartsWith("CV"))
                    {
                        lblInvalid.Visible = true;
                        boolInvalid = true;
                    }
                    ds = oProjectRequest.GetProjectNumber(txtClarityNumber.Text);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        lblDuplicate.Text = "A project request already exists with the clarity number <b>" + txtClarityNumber.Text + "</b>";
                        boolDuplicate = true;
                    }
                }
                if (boolDuplicate == false)
                {
                    ds = oProjectRequest.GetProjectName(txtProjectTask.Text);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        lblDuplicate.Text = "A project request already exists with the name <b>" + txtProjectTask.Text + "</b>";
                        boolDuplicate = true;
                    }
                }
                if (boolInvalid == false)
                {
                    if (boolDuplicate == false)
                    {
                        int intProject = oProject.Add(txtProjectTask.Text, ddlBaseDisc.SelectedItem.Text, txtClarityNumber.Text, intProfile, Int32.Parse(ddlOrganization.SelectedItem.Value), intSegment, 0);
                        Response.Redirect(oPage.GetFullLink(intPage) + "?pid=" + intProject.ToString());
                    }
                    else
                    {
                        int intRequest = Int32.Parse(ds.Tables[0].Rows[0]["requestid"].ToString());
                        int intProject = oRequest.GetProjectNumber(intRequest);
                        lblDetails.Text = "<table width=\"100%\" cellpadding=\"3\" cellspacing=\"2\" border=\"0\">";
                        lblDetails.Text += "<tr><td nowrap><b>Project Name:</b> </td><td width=\"100%\">" + oProject.Get(intProject, "name") + "</td></tr>";
                        dsRequest = oProjectRequest.GetProject(intProject);
                        lblDetails.Text += "<tr><td nowrap><b>Working Sponsor:</b> </td><td width=\"100%\">" + oUser.GetFullName(Int32.Parse(dsRequest.Tables[0].Rows[0]["working"].ToString())) + "</td></tr>";
                        lblDetails.Text += "<tr><td nowrap><b>Executive Sponsor:</b> </td><td width=\"100%\">" + oUser.GetFullName(Int32.Parse(dsRequest.Tables[0].Rows[0]["executive"].ToString())) + "</td></tr>";
                        lblDetails.Text += "<tr><td colspan=\"2\">&nbsp;</td></tr>";
                        lblDetails.Text += "<tr><td colspan=\"2\">Please contact the working or executive sponsor for details regarding this initiative.</td></tr>";
                        lblDetails.Text += "</table>";
                        panIntro.Visible = false;
                        panDuplicate.Visible = true;
                    }
                }
            }
            else
            {
                // Show Discretionary is not configured
                panIntro.Visible = false;
                panDiscretionary.Visible = true;
                lnkDiscretionary.NavigateUrl = oPage.GetFullLink(intResourceRequestPage);
                // Add A new service request
                int intProject = 0;
                if (txtClarityNumber.Text.Trim() != "")
                {
                    ds = oProject.Get(txtClarityNumber.Text);
                    if (ds.Tables[0].Rows.Count > 0)
                        intProject = Int32.Parse(ds.Tables[0].Rows[0]["projectid"].ToString());
                }
                if (intProject == 0)
                {
                    ds = oProject.GetName(txtProjectTask.Text);
                    if (ds.Tables[0].Rows.Count > 0)
                        intProject = Int32.Parse(ds.Tables[0].Rows[0]["projectid"].ToString());
                }
                if (intProject == 0)
                    intProject = oProject.Add(txtProjectTask.Text, ddlBaseDisc.SelectedItem.Text, txtClarityNumber.Text, intProfile, Int32.Parse(ddlOrganization.SelectedItem.Value), intSegment, 1);
                if (intProject > 0)
                {
                    int intRequest = oRequest.Add(intProject, intProfile);
                    ServiceRequests oServiceRequest = new ServiceRequests(intProfile, dsn);
                    oServiceRequest.Add(intRequest, 1, -2);
                    Response.Redirect(oPage.GetFullLink(intResourceRequestPage) + "?rid=" + intRequest);
                }
                // Send to Service Request Page
                Response.Redirect(oPage.GetFullLink(intResourceRequestPage));
            }
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
            int intProject = 0;
            if (lblProject.Text == "")
            {
                if (txtClarityNumber.Text.Trim() != "")
                {
                    ds = oProject.Get(txtClarityNumber.Text);
                    if (ds.Tables[0].Rows.Count > 0)
                        intProject = Int32.Parse(ds.Tables[0].Rows[0]["projectid"].ToString());
                }
                if (intProject == 0)
                {
                    ds = oProject.GetName(txtProjectTask.Text);
                    if (ds.Tables[0].Rows.Count > 0)
                        intProject = Int32.Parse(ds.Tables[0].Rows[0]["projectid"].ToString());
                }
                if (intProject == 0)
                {
                    int intSegment = 0;
                    if (Request.Form[hdnSegment.UniqueID] != "")
                        intSegment = Int32.Parse(Request.Form[hdnSegment.UniqueID]);
                    intProject = oProject.Add(txtProjectTask.Text, ddlBaseDisc.SelectedItem.Text, txtClarityNumber.Text, intProfile, Int32.Parse(ddlOrganization.SelectedItem.Value), intSegment, 0);
                }
            }
            else
            {
                intProject = Int32.Parse(lblProject.Text);
                oProject.Update(intProject, 0);
            }
            int intRequest = oRequest.AddTask(intProject, intProfile, txtInitiative.Text, DateTime.Parse(txtStart.Text), DateTime.Parse(txtCompletion.Text));
            oProject.Update(intProject, 0, Int32.Parse(Request.Form[hdnExecutive.UniqueID]), Int32.Parse(Request.Form[hdnWorking.UniqueID]), 0, 0, 0);
            oProjectRequest.Add(intRequest, (chkRequirement.Checked ? 1 : 0), (chkRequirement.Checked ? txtRequirement.Text : ""), ddlInterdependency.SelectedItem.Text, txtInterdependency.Text, txtCapability.Text, Int32.Parse(txtHours.Text), ddlCapital.SelectedItem.Text, ddlInternal.SelectedItem.Text, ddlExternal.SelectedItem.Text, ddlMaintenance.SelectedItem.Text, ddlExpenses.SelectedItem.Text, ddlCostAvoidance.SelectedItem.Text, ddlSavings.SelectedItem.Text, ddlRealized.SelectedItem.Text, ddlBusinessAvoidance.SelectedItem.Text, ddlMaintenanceAvoidance.SelectedItem.Text, ddlReusability.SelectedItem.Text, ddlInternalImpact.SelectedItem.Text, ddlExternalImpact.SelectedItem.Text, ddlImpact.SelectedItem.Text, ddlStrategic.SelectedItem.Text, ddlAcquisition.SelectedItem.Text, ddlCapabilities.SelectedItem.Text, (chkC1.Checked ? 1 : 0), (chkEndLife.Checked ? 1 : 0), (chkEndLife.Checked ? txtEndLife.Text : ""), (radTPMYes.Checked ? 1 : 0));
            if (radTPMNo.Checked == true)
            {
                int intWorkflowParent = oResourceRequest.Add(intRequest, 0, 0, 0, "", 0, 0.00, 2, 0, 0, 1);
                oResourceRequest.AddWorkflow(intWorkflowParent, 0, "", Int32.Parse(Request.Form[hdnManager.UniqueID]), 0, 0.00, 2, 0);
            }
            else
            {
                int intService = Int32.Parse(ddlTPM.SelectedItem.Value);
                int intItem = oService.GetItemId(intService);
                int intAccepted = (oService.Get(intService, "rejection") == "1" ? 0 : 1);
                oResourceRequest.Add(intRequest, intItem, intService, 0, "", 0, 0.00, 2, 0, intAccepted, 1);
            }
            if (chkRequirement.Checked == true || chkEndLife.Checked == true)
                oProjectRequest.AddPriority(intRequest, 1.00, 1.00, 1.00);
            else
                oProjectRequest.AddPriority(intRequest, dblExpected, dblAvoidance, dblImpact);
            while (strPlatforms != "")
            {
                string strField = strPlatforms.Substring(0, strPlatforms.IndexOf("&"));
                strPlatforms = strPlatforms.Substring(strPlatforms.IndexOf("&") + 1);
                int intOrder = Int32.Parse(strField.Substring(strField.IndexOf("_") + 1));
                strField = strField.Substring(0, strField.IndexOf("_"));
                oProjectRequest.AddPlatform(intRequest, Int32.Parse(strField));
            }
            oApprove.NewRequest(intRequest, intProfile, false, intWorkflowPage, boolDirector);
            Response.Redirect(oPage.GetFullLink(intPage) + "?rid=" + intRequest.ToString());
        }
        protected void btnClose_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path + "/index.aspx");
        }
        protected void btnFinish_Click(Object Sender, EventArgs e)
        {
            oProjectRequest.Update(Int32.Parse(lblRequest.Text), (chkNotify.Checked ? 1 : 0));
            Response.Redirect(Request.Path + "/index.aspx");
        }
    }
}