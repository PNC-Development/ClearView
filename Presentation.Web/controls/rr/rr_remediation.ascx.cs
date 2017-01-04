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
    public partial class rr_remediation : System.Web.UI.UserControl
    {

        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intProjectRequestPage = Int32.Parse(ConfigurationManager.AppSettings["ProjectRequest"]);
        protected Pages oPage;
        protected RequestItems oRequestItem;
        protected Applications oApplication;
        protected Variables oVariable;
        protected RequestFields oRequestField;
        protected Services oService;
        protected Customized oCustomized;
        protected Requests oRequest;
        protected Projects oProject;
        protected ProjectsPending oProjectsPending;
        protected int intProfile;
        protected int intPage = 0;
        protected int intApplication = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oPage = new Pages(intProfile, dsn);
            oRequestItem = new RequestItems(intProfile, dsn);
            oApplication = new Applications(intProfile, dsn);
            oVariable = new Variables(intEnvironment);
            oRequestField = new RequestFields(intProfile, dsn);
            oService = new Services(intProfile, dsn);
            oCustomized = new Customized(intProfile, dsn);
            oRequest = new Requests(intProfile, dsn);
            oProject = new Projects(intProfile, dsn);
            oProjectsPending = new ProjectsPending(intProfile, dsn, intEnvironment);
            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            if (Request.QueryString["rid"] != "" && Request.QueryString["rid"] != null)
            {
                LoadValues();
                txtWorking.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'300','195','" + divWorking.ClientID + "','" + lstWorking.ClientID + "','" + hdnWorking.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
                lstWorking.Attributes.Add("ondblclick", "AJAXClickRow();");
                imgStart.Attributes.Add("onclick", "return ShowCalendar('" + txtStart.ClientID + "');");
                imgEnd.Attributes.Add("onclick", "return ShowCalendar('" + txtEnd.ClientID + "');");
                // Custom Loads
                int intItem = Int32.Parse(lblItem.Text);
                int intApp = oRequestItem.GetItemApplication(intItem);
                LoadRequest(intApp);
                string strDeliverable = oApplication.Get(intApp, "deliverables_doc");
                if (strDeliverable != "")
                {
                    panDeliverable.Visible = true;
                    btnDeliverable.Attributes.Add("onclick", "return OpenWindow('NEW_WINDOW','" + strDeliverable + "');");
                }
                oApplication.AssignPriority(intApp, radPriority, lblDeliverable.ClientID, txtEnd.ClientID, hdnEnd.ClientID);
                int intWorking = oApplication.GetLead(intApp, 3);
                lblDeliverable.Text = intWorking.ToString();
                txtEnd.Text = DateTime.Today.AddDays(intWorking).ToShortDateString();
                hdnEnd.Value = DateTime.Today.AddDays(intWorking).ToShortDateString();
                btnClose.Attributes.Add("onclick", "return CloseWindow();");
                btnBack2.Attributes.Add("onclick", "ShowHideDiv('" + divShow.ClientID + "','inline');ShowHideDiv('" + divHide.ClientID + "','none');return false;");
                btnDocuments.Attributes.Add("onclick", "return OpenWindow('DOCUMENTS_SECURE','?rid=" + Request.QueryString["rid"] + "');");
                imgDate.Attributes.Add("onclick", "return ShowCalendar('" + txtDate.ClientID + "');");
            }
            btnCancel1.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this service request?');");
        }
        private void LoadValues()
        {
            int intRequest = Int32.Parse(Request.QueryString["rid"]);
            DataSet dsItems = oRequestItem.GetForms(intRequest);
            if (dsItems.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow drItem in dsItems.Tables[0].Rows)
                {
                    if (drItem["done"].ToString() == "-1")
                    {
                        lblItem.Text = drItem["itemid"].ToString();
                        lblNumber.Text = drItem["number"].ToString();
                        break;
                    }
                }
            }
        }
        private void LoadRequest(int intApp)
        {
            Users oUser = new Users(intProfile, dsn);
            int intRequest = Int32.Parse(Request.QueryString["rid"]);
            int intProject = oRequest.GetProjectNumber(intRequest);
            int intWorking = 0;
            if (intProject > 0)
                intWorking = Int32.Parse(oProject.Get(intProject, "working"));
            else
            {
                string strWorking = oProjectsPending.GetRequest(intRequest, "working");
                if (strWorking != "")
                    intWorking = Int32.Parse(strWorking);
            }
            if (intWorking > 0)
                txtWorking.Text = oUser.GetFullName(intWorking) + " (" + oUser.GetName(intWorking) + ")";
            hdnWorking.Value = intWorking.ToString();
            DataSet dsRequest = oCustomized.GetRemediationRequests(intRequest);
            if (dsRequest.Tables[0].Rows.Count > 0)
            {
                ddlReason.SelectedValue = dsRequest.Tables[0].Rows[0]["reason"].ToString();
                ddlComponent.SelectedValue = dsRequest.Tables[0].Rows[0]["component"].ToString();
                ddlFunding.SelectedValue = dsRequest.Tables[0].Rows[0]["funding"].ToString();
                ddlFunding.Visible = true;
                radPriority.SelectedValue = dsRequest.Tables[0].Rows[0]["priority"].ToString();
                string strTPM = dsRequest.Tables[0].Rows[0]["tpm"].ToString();
                if (strTPM == "")
                    radTPM_No.Checked = true;
                txtStatement.Text = dsRequest.Tables[0].Rows[0]["statement"].ToString();
                txtDevices.Text = dsRequest.Tables[0].Rows[0]["devices"].ToString();
                txtHours.Text = dsRequest.Tables[0].Rows[0]["hours"].ToString();
                txtStart.Text = DateTime.Parse(dsRequest.Tables[0].Rows[0]["start_date"].ToString()).ToShortDateString();
                txtEnd.Text = DateTime.Parse(dsRequest.Tables[0].Rows[0]["end_date"].ToString()).ToShortDateString();
            }
            else
            {
                radPriority.SelectedValue = "3";
                int intApproved = (oProject.IsApproved(intProject) ? 1 : 0);
                ProjectRequest oProjectRequest = new ProjectRequest(intProfile, dsn);
                DataSet dsProjectRequest = oProjectRequest.GetProject(intProject);
                int intProjectRequest = 0;
                if (dsProjectRequest.Tables[0].Rows.Count > 0 && intApproved == 1)
                {
                    intProjectRequest = Int32.Parse(dsProjectRequest.Tables[0].Rows[0]["requestid"].ToString());
                    lblFunding.Text = dsProjectRequest.Tables[0].Rows[0]["expected_capital"].ToString();
                    lblFunding.Visible = true;
                    txtStatement.Text = oRequest.Get(intRequest, "description");
                    txtStart.Text = DateTime.Parse(oRequest.Get(intRequest, "start_date")).ToShortDateString();
                    btnNext.Attributes.Add("onclick", "return ValidateDropDown('" + ddlReason.ClientID + "','Please make a selection for the reason')" +
                        " && ValidateDropDown('" + ddlComponent.ClientID + "','Please make a selection for the component')" +
                        " && ValidateText('" + txtStatement.ClientID + "','Please enter a statement of work')" +
                        " && ValidateNumber('" + txtDevices.ClientID + "','Please enter a valid number of devices')" +
                        " && ValidateNumber0('" + txtHours.ClientID + "','Please enter a valid number of hours')" +
                        " && ValidateDate('" + txtStart.ClientID + "','Please enter a valid estimated start date')" +
                        " && ValidateDate('" + txtEnd.ClientID + "','Please enter a valid estimated end date')" +
                        " && ValidateDateHidden('" + txtEnd.ClientID + "','" + hdnEnd.ClientID + "','NOTE: Based on your priority, your end date cannot occur before a certain time.\\n\\n')" +
                        " && ValidateDates('" + txtStart.ClientID + "','" + txtEnd.ClientID + "','The estimated start date must occur before the estimated end date')" +
                        ";");
                    int intRequestor = Int32.Parse(oRequest.Get(intProjectRequest, "userid"));
                    DataSet dsProject = oProjectRequest.GetResources(intProjectRequest);
                    if (dsProject.Tables[0].Rows.Count > 0)
                    {
                        panPM.Visible = true;
                        int intItem = Int32.Parse(dsProject.Tables[0].Rows[0]["itemid"].ToString());
                        int intService = Int32.Parse(dsProject.Tables[0].Rows[0]["serviceid"].ToString());
                        int intAccepted = Int32.Parse(dsProject.Tables[0].Rows[0]["accepted"].ToString());
                        int intManager = Int32.Parse(dsProject.Tables[0].Rows[0]["userid"].ToString());
                        if (intAccepted == -1)
                            lblPM.Text = "<i>Pending Assignment</i><br><font class=\"footer\">(" + oUser.GetFullName(intRequestor) + ")</font>";
                        else if (intManager > 0)
                        {
                            lblPM.Text = oUser.GetFullName(intManager);
                            if (intItem == 0)
                                radTPM_No.Checked = true;
                            else
                                radTPM_Yes.Checked = true;
                            radTPM_No.Enabled = false;
                            radTPM_Yes.Enabled = false;
                        }
                        else if (intItem > 0)
                            lblPM.Text = "<i>Pending Assignment</i><br><font class=\"footer\">(" + oService.GetName(intService) + ")</font>";
                    }
                    else
                        lblPM.Text = "<i>Pending Technical Project Management Service Request</i>";
                }
                else
                {
                    ddlFunding.Visible = true;
                    radTPM_No.Checked = true;
                    btnNext.Attributes.Add("onclick", "return ValidateHidden('" + hdnWorking.ClientID + "','" + txtWorking.ClientID + "','Please enter the LAN ID of your working sponsor')" +
                        " && ValidateDropDown('" + ddlReason.ClientID + "','Please make a selection for the reason')" +
                        " && ValidateDropDown('" + ddlComponent.ClientID + "','Please make a selection for the component')" +
                        " && ValidateDropDown('" + ddlFunding.ClientID + "','Please make a selection for the estimated funding cost')" +
                        " && ValidateRadioButtons('" + radTPM_Yes.ClientID + "','" + radTPM_No.ClientID + "','Please select whether or not you require a technical project manager')" +
                        " && ValidateText('" + txtStatement.ClientID + "','Please enter a statement of work')" +
                        " && ValidateNumber('" + txtDevices.ClientID + "','Please enter a valid number of devices')" +
                        " && ValidateNumber0('" + txtHours.ClientID + "','Please enter a valid number of hours')" +
                        " && ValidateDate('" + txtStart.ClientID + "','Please enter a valid estimated start date')" +
                        " && ValidateDate('" + txtEnd.ClientID + "','Please enter a valid estimated end date')" +
                        " && ValidateDateHidden('" + txtEnd.ClientID + "','" + hdnEnd.ClientID + "','NOTE: Based on your priority, your end date cannot occur before a certain time.\\n\\n')" +
                        " && ValidateDates('" + txtStart.ClientID + "','" + txtEnd.ClientID + "','The estimated start date must occur before the estimated end date')" +
                        " && ValidateRemediation('" + intApproved.ToString() + "','" + ddlFunding.ClientID + "','" + radTPM_Yes.ClientID + "','" + txtHours.ClientID + "','" + divShow.ClientID + "','" + divHide.ClientID + "')" +
                        ";");
                }
            }
        }
        protected void lnkRequest_Click(Object Sender, EventArgs e)
        {
            Users oUser = new Users(intProfile, dsn);
            string strDefault = oUser.GetApplicationUrl(intProfile, intProjectRequestPage);
            int intRequest = Int32.Parse(Request.QueryString["rid"]);
            Requests oRequest = new Requests(intProfile, dsn);
            Response.Redirect(strDefault + oPage.GetFullLink(intProjectRequestPage) + "?pid=" + oRequest.Get(intRequest, "projectid"));
        }
        protected void btnBack_Click(Object Sender, EventArgs e)
        {
            int intRequest = Int32.Parse(Request.QueryString["rid"]);
            oRequestItem.UpdateForm(intRequest, false);
            Response.Redirect(oPage.GetFullLink(intPage) + "?rid=" + intRequest.ToString());
        }
        protected void btnNext_Click(Object Sender, EventArgs e)
        {
            int intRequest = Int32.Parse(Request.QueryString["rid"]);
            int intItem = Int32.Parse(lblItem.Text);
            int intNumber = Int32.Parse(lblNumber.Text);
            int intProject = oRequest.GetProjectNumber(intRequest);
            if (intProject > 0)
                oProject.Update(intProject, 0, 0, Int32.Parse(Request.Form[hdnWorking.UniqueID]), 0, 0, 0);
            else
                oProjectsPending.Update(intRequest, 0, 0, Int32.Parse(Request.Form[hdnWorking.UniqueID]), 0, 0, 0);
            oCustomized.AddRemediation(intRequest, intItem, intNumber, ddlReason.SelectedItem.Value, ddlComponent.SelectedItem.Value, ddlFunding.SelectedItem.Value, Int32.Parse(radPriority.SelectedItem.Value), 0, txtStatement.Text, Int32.Parse(txtDevices.Text), double.Parse(txtHours.Text), DateTime.Parse(txtStart.Text), DateTime.Parse(txtEnd.Text), txtNumber.Text, txtDate.Text, txtTime.Text, txtChange.Text);
            if (oRequest.Get(intRequest, "description") == "")
                oRequest.UpdateDescription(intRequest, txtStatement.Text);
            oRequest.UpdateStartDate(intRequest, DateTime.Parse(txtStart.Text));
            oRequest.UpdateEndDate(intRequest, DateTime.Parse(txtEnd.Text));
            oRequestItem.UpdateForm(intRequest, true);
            Response.Redirect(oPage.GetFullLink(intPage) + "?rid=" + intRequest.ToString());
        }
        protected void btnCancel_Click(Object Sender, EventArgs e)
        {
            int intRequest = Int32.Parse(Request.QueryString["rid"]);
            oRequest.Cancel(intRequest);
            Response.Redirect(oPage.GetFullLink(intPage));
        }
        private string LoadRequestItem(DataSet _dataset, int _nameid)
        {
            string strReturn = "";
            foreach (DataRow dr in _dataset.Tables[0].Rows)
            {
                if (Int32.Parse(dr["nameid"].ToString()) == _nameid)
                {
                    strReturn = dr["value"].ToString();
                    break;
                }
            }
            return strReturn;
        }
    }
}