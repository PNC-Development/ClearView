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
    public partial class rr_iis : System.Web.UI.UserControl
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
        protected string strTable = "";
        protected string strScript = "";
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
                txtManager.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'300','195','" + divManager.ClientID + "','" + lstManager.ClientID + "','" + hdnLead.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
                lstManager.Attributes.Add("ondblclick", "AJAXClickRow();");
                txtEngineer.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'300','195','" + divEngineer.ClientID + "','" + lstEngineer.ClientID + "','" + hdnTechnical.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
                lstEngineer.Attributes.Add("ondblclick", "AJAXClickRow();");
                imgStart.Attributes.Add("onclick", "return ShowCalendar('" + txtStart.ClientID + "');");
                imgEnd.Attributes.Add("onclick", "return ShowCalendar('" + txtEnd.ClientID + "');");
                chkExpedite_Yes.Attributes.Add("onclick", "Expedite(this, '" + chkExpedite_No.ClientID + "');");
                // Custom Loads
                int intItem = Int32.Parse(lblItem.Text);
                int intApp = oRequestItem.GetItemApplication(intItem);
                LoadRequest(intItem, intApp);
                string strDeliverable = oApplication.Get(intApp, "deliverables_doc");
                if (strDeliverable != "")
                {
                    panDeliverable.Visible = true;
                    btnDeliverable.Attributes.Add("onclick", "return OpenWindow('NEW_WINDOW','" + strDeliverable + "');");
                }
                btnDocuments.Attributes.Add("onclick", "return OpenWindow('DOCUMENTS_SECURE','?rid=" + Request.QueryString["rid"] + "');");
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
                        lblService.Text = drItem["serviceid"].ToString();
                        break;
                    }
                }
            }
        }
        private void LoadRequest(int intItem, int intApp)
        {
            int intRequest = Int32.Parse(Request.QueryString["rid"]);
            int intProject = oRequest.GetProjectNumber(intRequest);
            int intLead = 0;
            int intTechnical = 0;
            if (intProject > 0)
            {
                if (oProject.Get(intProject, "lead") != "")
                    intLead = Int32.Parse(oProject.Get(intProject, "lead"));
                if (oProject.Get(intProject, "technical") != "")
                    intTechnical = Int32.Parse(oProject.Get(intProject, "technical"));
            }
            else
            {
                try
                {
                    intLead = Int32.Parse(oProjectsPending.GetRequest(intRequest, "lead"));
                    intTechnical = Int32.Parse(oProjectsPending.GetRequest(intRequest, "technical"));
                }
                catch { }
            }
            Users oUser = new Users(intProfile, dsn);
            if (intLead > 0)
                txtManager.Text = oUser.GetFullName(intLead) + " (" + oUser.GetName(intLead) + ")";
            if (intTechnical > 0)
                txtEngineer.Text = oUser.GetFullName(intTechnical) + " (" + oUser.GetName(intTechnical) + ")";
            hdnLead.Value = intLead.ToString();
            hdnTechnical.Value = intTechnical.ToString();
            DataSet dsRequest = oCustomized.GetIISRequests(intRequest);
            if (dsRequest.Tables[0].Rows.Count > 0)
            {
                ddlReason.SelectedValue = dsRequest.Tables[0].Rows[0]["reason"].ToString();
                txtStatement.Text = dsRequest.Tables[0].Rows[0]["statement"].ToString();
                txtStart.Text = DateTime.Parse(dsRequest.Tables[0].Rows[0]["start_date"].ToString()).ToShortDateString();
                txtEnd.Text = DateTime.Parse(dsRequest.Tables[0].Rows[0]["end_date"].ToString()).ToShortDateString();
                bool boolExpedite = (dsRequest.Tables[0].Rows[0]["expedite"].ToString() == "1");
                chkExpedite_Yes.Checked = boolExpedite;
                chkExpedite_No.Checked = !boolExpedite;
            }
            btnNext.Attributes.Add("onclick", "return ValidateHidden0('" + hdnLead.ClientID + "','" + txtManager.ClientID + "','Please enter the LAN ID of your project coordinator')" +
                " && ValidateHidden0('" + hdnTechnical.ClientID + "','" + txtEngineer.ClientID + "','Please enter the LAN ID of your integration engineer')" +
                " && ValidateDropDown('" + ddlReason.ClientID + "','Please make a selection for the reason')" +
                " && ValidateDate('" + txtStart.ClientID + "','Please enter a valid estimated start date')" +
                " && ValidateDate('" + txtEnd.ClientID + "','Please enter a valid estimated end date')" +
                " && ValidateDates('" + txtStart.ClientID + "','" + txtEnd.ClientID + "','The estimated start date must occur before the estimated end date')" +
                ";");
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
                oProject.Update(intProject, Int32.Parse(Request.Form[hdnLead.UniqueID]), 0, 0, Int32.Parse(Request.Form[hdnTechnical.UniqueID]), 0, 0);
            else
                oProjectsPending.Update(intProject, Int32.Parse(Request.Form[hdnLead.UniqueID]), 0, 0, Int32.Parse(Request.Form[hdnTechnical.UniqueID]), 0, 0);
            int intExpedite = 0;
            if (chkExpedite_Yes.Checked == true)
                intExpedite = 1;
            oCustomized.AddIIS(intRequest, intItem, intNumber, ddlReason.SelectedItem.Value, txtStatement.Text, intExpedite, DateTime.Parse(txtStart.Text), DateTime.Parse(txtEnd.Text));
            oRequestItem.UpdateForm(intRequest, true);
            Response.Redirect(oPage.GetFullLink(intPage) + "?rid=" + intRequest.ToString());
        }
        protected void btnCancel_Click(Object Sender, EventArgs e)
        {
            int intRequest = Int32.Parse(Request.QueryString["rid"]);
            oRequest.Cancel(intRequest);
            Response.Redirect(oPage.GetFullLink(intPage));
        }
    }
}