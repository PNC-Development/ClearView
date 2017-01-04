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
    public partial class project_request_new_dynamic : System.Web.UI.UserControl
    {

        private DataSet ds;
        private DataSet dsResp;
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


        protected int intIndex;
        protected int intProject;
        protected int intQuestionId;
        protected int intResponseId;
        protected int intClassId;
        private HtmlGenericControl oHTMLcontrol;

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
            //ddlOrganization.Attributes.Add("onchange", "PopulateSegments('" + ddlOrganization.ClientID + "','" + ddlSegment.ClientID + "');");
            //ddlSegment.Attributes.Add("onchange", "UpdateDropDownHidden('" + ddlSegment.ClientID + "','" + hdnSegment.ClientID + "');");
            //imgRequirement.Attributes.Add("onclick", "return ShowCalendar('" + txtRequirement.ClientID + "');");
            //imgEndLife.Attributes.Add("onclick", "return ShowCalendar('" + txtEndLife.ClientID + "');");
            imgStart.Attributes.Add("onclick", "return ShowCalendar('" + txtStart.ClientID + "');");
            imgCompletion.Attributes.Add("onclick", "return ShowCalendar('" + txtCompletion.ClientID + "');");
            txtExecutive.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'300','195','" + divExecutive.ClientID + "','" + lstExecutive.ClientID + "','" + hdnExecutive.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
            lstExecutive.Attributes.Add("ondblclick", "AJAXClickRow();");
            txtWorking.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'300','195','" + divWorking.ClientID + "','" + lstWorking.ClientID + "','" + hdnWorking.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
            lstWorking.Attributes.Add("ondblclick", "AJAXClickRow();");
            //txtManager.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'300','195','" + divManager.ClientID + "','" + lstManager.ClientID + "','" + hdnManager.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
            //lstManager.Attributes.Add("ondblclick", "AJAXClickRow();");
            btnPlatformAdd.Attributes.Add("onclick", "return MoveList('" + lstPlatformsAvailable.ClientID + "','" + lstPlatformsCurrent.ClientID + "','" + hdnPlatforms.ClientID + "','" + lstPlatformsCurrent.ClientID + "');");
            lstPlatformsAvailable.Attributes.Add("ondblclick", "return MoveList('" + lstPlatformsAvailable.ClientID + "','" + lstPlatformsCurrent.ClientID + "','" + hdnPlatforms.ClientID + "','" + lstPlatformsCurrent.ClientID + "');");
            btnPlatformRemove.Attributes.Add("onclick", "return MoveList('" + lstPlatformsCurrent.ClientID + "','" + lstPlatformsAvailable.ClientID + "','" + hdnPlatforms.ClientID + "','" + lstPlatformsCurrent.ClientID + "');");
            lstPlatformsCurrent.Attributes.Add("ondblclick", "return MoveList('" + lstPlatformsCurrent.ClientID + "','" + lstPlatformsAvailable.ClientID + "','" + hdnPlatforms.ClientID + "','" + lstPlatformsCurrent.ClientID + "');");
            // chkRequirement.Attributes.Add("onclick", "ShowHideDivCheck('" + divRequirement.ClientID + "',this);");
            //chkEndLife.Attributes.Add("onclick", "ShowHideDivCheck('" + divEndLife.ClientID + "',this);");
            //radTPMYes.Attributes.Add("onclick", "ShowHideDiv('" + divTPMYes.ClientID + "','inline');ShowHideDiv('" + divTPMNo.ClientID + "','none');");
            //radTPMNo.Attributes.Add("onclick", "ShowHideDiv('" + divTPMNo.ClientID + "','inline');ShowHideDiv('" + divTPMYes.ClientID + "','none');");
            //ddlInterdependency.Attributes.Add("onclick", "ShowHideDivDropDown('" + divInterdependency.ClientID + "',this,2,3);");
            //btnPName.Attributes.Add("onclick", "return ShowProjectInfo('" + txtProjectTask.ClientID + "','" + ddlBaseDisc.ClientID + "','" + ddlOrganization.ClientID + "','" + txtClarityNumber.ClientID + "','" + txtProjectTask.ClientID + "','PNAME_SEARCH_NOCV');");
            //txtProjectTask.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnPName.ClientID + "').click();return false;}} else {return true}; ");
            //btnPNumber.Attributes.Add("onclick", "return ShowProjectInfo('" + txtProjectTask.ClientID + "','" + ddlBaseDisc.ClientID + "','" + ddlOrganization.ClientID + "','" + txtClarityNumber.ClientID + "','" + txtClarityNumber.ClientID + "','PNUMBER_SEARCH_NOCV');");
            //txtClarityNumber.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnPNumber.ClientID + "').click();return false;}} else {return true}; ");
            //txtInitiative.Attributes.Add("onfocusin", "InitiativeIn(this);");
            //txtInitiative.Attributes.Add("onfocusout", "InitiativeOut(this);");
            //txtInitiative.Attributes.Add("onkeypress", "return CancelEnter();");
            //txtCapability.Attributes.Add("onkeypress", "return CancelEnter();");
            //lblInvalid.Visible = false;
            lblTitle.Text = "New Project Request";
            if (Request.QueryString["rid"] != null && Request.QueryString["rid"] != "")
            {
                panFinish.Visible = true;
                lblRequest.Text = Request.QueryString["rid"];
            }
            else if (Request.QueryString["pid"] != null && Request.QueryString["pid"] != "")
            {
                lblProject.Text = Request.QueryString["pid"];
                intProject = Int32.Parse(lblProject.Text);
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
            //btnSubmit.Attributes.Add("onclick", "return ValidateText('" + txtProjectTask.ClientID + "','Please enter a project or task name') && ValidateDropDown('" + ddlBaseDisc.ClientID + "','Please choose if this is a base or Discretionary project') && ValidateDropDown('" + ddlOrganization.ClientID + "','Please choose the organization sponsoring this initiative') && (document.getElementById('" + ddlBaseDisc.ClientID + "').selectedIndex == 1 || ValidateText('" + txtClarityNumber.ClientID + "','Please enter a clarity number'));");
            btnSave.Attributes.Add("onclick", "return ValidateHidden('" + hdnExecutive.ClientID + "','" + txtExecutive.ClientID + "','Please enter the LAN ID of your executive sponsor')" +
                " && ValidateHidden('" + hdnWorking.ClientID + "','" + txtWorking.ClientID + "','Please enter the LAN ID of your working sponsor')" +
                " && ValidateText('" + txtInitiative.ClientID + "','Please enter the initiative opportunity')" +
                " && EnsureInitiative('" + txtInitiative.ClientID + "')" +
                " && ValidateList('" + lstPlatformsCurrent.ClientID + "','Please select at least one platform')" +
                " && ValidateDate('" + txtCompletion.ClientID + "','Please enter a valid project completion date')" +
                ";");
            btnClose.Attributes.Add("onclick", "return CloseWindow();");
            btnDiscretionary.Attributes.Add("onclick", "return CloseWindow();");


            // Vijay Code - Start
            ds = oProjectRequest.GetQAs();
            DataSet dsProj = oProject.Get(intProject);

            string strText = "";
            foreach (DataRow drprj in dsProj.Tables[0].Rows)
            {
                foreach (DataRow drqa in ds.Tables[0].Rows)
                {
                    if ((drprj["bd"].ToString() == drqa["bd"].ToString()) && (drprj["organization"].ToString() == drqa["organizationid"].ToString()))
                    {
                        // Response.Write(drqa["questionid"].ToString() + " " + drqa["deleted"].ToString()+"<br>");
                        int intQuestion = Int32.Parse(drqa["questionid"].ToString());
                        int intRequired = oProjectRequest.GetQuestion(intQuestion, "required") == "" ? 0 : Int32.Parse(oProjectRequest.GetQuestion(intQuestion, "required"));

                        oHTMLcontrol = new HtmlGenericControl();
                        TableRow tr = new TableRow();
                        TableCell td = new TableCell();

                        Label lbl = new Label();
                        lbl.Text = oProjectRequest.GetQuestion(intQuestion, "question");
                        strText = lbl.Text;
                        lbl.Width = Unit.Pixel(150);
                        td.Wrap = false;
                        td.CssClass = "default";
                        if (intRequired == 1) lbl.Text += " <font class=\"required\">*</font>";
                        td.Controls.Add(lbl);


                        tr.Controls.Add(td);
                        dsResp = oProjectRequest.GetResponses(intQuestion, 1);

                        DropDownList ddl = new DropDownList();
                        ddl.CssClass = "default";
                        ddl.Width = Unit.Pixel(400);
                        ddl.DataSource = dsResp;
                        ddl.DataTextField = "response";
                        ddl.DataValueField = "id";
                        ddl.DataBind();
                        ddl.Items.Insert(0, "--SELECT--");

                        td = new TableCell();
                        td.Controls.Add(ddl);
                        tr.Controls.Add(td);
                        oHTMLcontrol.Controls.Add(tr);
                        phTest.Controls.Add(oHTMLcontrol);

                        string attributes = btnSave.Attributes["onclick"].Replace(";", "");
                        if (intRequired == 1)
                        {
                            attributes += " && ValidateDropDown('" + ddl.UniqueID + "','Please make a selection for " + strText + "');";
                            btnSave.Attributes["onclick"] = attributes;
                        }

                        ddl.Attributes.Add("onchange", "UpdateHidden2('" + intQuestion + "','" + hdnResponseID.ClientID + "'," + ddl.ClientID + ");");
                        //hdnSubmissionID.Value += intQuestion.ToString() + ":" + ddl.SelectedValue + "<br>";                    

                    }
                }

            }

            // Vijay Code - End        
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
            //ddlTPM.DataValueField = "itemid";
            //ddlTPM.DataTextField = "service_title";
            //ddlTPM.DataSource = ds;
            //ddlTPM.DataBind();
            //ddlTPM.Items.Insert(0, new ListItem("-- SELECT --", "0"));
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
            // Vijay Code - Start
            Response.Write(hdnResponseID.Value + "<br>");
            string[] strSplit = hdnResponseID.Value.Split('&');
            // Vijay Code - End

            string strPlatforms = Request.Form[hdnPlatforms.UniqueID];
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

            // Vijay Code - Start
            ds = oProjectRequest.GetClasses(1);
            int intClassCount = ds.Tables[0].Rows.Count;
            double dblOverall = 0.00;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                double dblYY = 0.00;
                double dblTotal = 0.00;
                double dblWt = 0.00;

                DataSet dsQ = oProjectRequest.GetQuestionsByClass(Int32.Parse(dr["id"].ToString()));
                int intQuestionCount = dsQ.Tables[0].Rows.Count;
                if (intQuestionCount > 0) dblYY = (1.0 / intQuestionCount);
                foreach (DataRow drQ in dsQ.Tables[0].Rows)
                {
                    foreach (string str in strSplit)
                    {
                        string strTemp = str;
                        intIndex = str.IndexOf(":");
                        if (intIndex >= 1)
                        {
                            intQuestionId = Int32.Parse(strTemp.Substring(0, intIndex));
                            strTemp = strTemp.Substring(intIndex + 1);
                            intResponseId = Int32.Parse(strTemp);
                            intClassId = Int32.Parse(dr["id"].ToString());

                            if (drQ["questionid"].ToString() == intQuestionId.ToString())
                            {
                                dblWt = Double.Parse(oProjectRequest.GetResponseWeight(intResponseId, intQuestionId));
                                dblTotal += (dblYY * dblWt);
                                break;
                            }
                        }
                    }
                    //  Response.Write("# Questions: " + intQuestionCount + " Qid: " + intQuestionId + " RespId: " + intResponseId + " Class: " + intClassId + " " + " Wt: " + dblWt + " YY: " + dblYY + "<br>");

                }
                dblTotal = dblTotal / 5;
                dblOverall += dblTotal;
                oProjectRequest.AddWeightPriority(intRequest, Int32.Parse(dr["id"].ToString()), dblTotal);
                // Response.Write("Total_Wt: " + Double.Parse(dblTotal.ToString()).ToString("P") + "<br><br>");
            }
            dblOverall = dblOverall / intClassCount;
            oProjectRequest.AddWeightPriority(intRequest, 0, dblOverall);

            // Vijay Code - End         

            while (strPlatforms != "")
            {
                string strField = strPlatforms.Substring(0, strPlatforms.IndexOf("&"));
                strPlatforms = strPlatforms.Substring(strPlatforms.IndexOf("&") + 1);
                int intOrder = Int32.Parse(strField.Substring(strField.IndexOf("_") + 1));
                strField = strField.Substring(0, strField.IndexOf("_"));
                oProjectRequest.AddPlatform(intRequest, Int32.Parse(strField));
            }
            oApprove.NewRequest(intRequest, intProfile, false, intWorkflowPage, boolDirector);
            // Vijay Code - Start  
            foreach (string str in strSplit)
            {
                string strTemp = str;
                intIndex = str.IndexOf(":");
                if (intIndex >= 1)
                {
                    intQuestionId = Int32.Parse(strTemp.Substring(0, intIndex));
                    strTemp = strTemp.Substring(intIndex + 1);
                    intResponseId = Int32.Parse(strTemp);
                    oProjectRequest.AddSubmission(intRequest, intQuestionId, intResponseId);
                }
            }
            // Vijay Code - End

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