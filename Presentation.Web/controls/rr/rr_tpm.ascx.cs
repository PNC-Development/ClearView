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
    public partial class rr_tpm : System.Web.UI.UserControl
    {

        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intViewRequest = Int32.Parse(ConfigurationManager.AppSettings["ViewRequest"]);
        protected Pages oPage;
        protected RequestItems oRequestItem;
        protected ServiceRequests oServiceRequest;
        protected Applications oApplication;
        protected Variables oVariable;
        protected RequestFields oRequestField;
        protected Services oService;
        protected Users oUser;
        protected Customized oCustomized;
        protected Requests oRequest;
        protected ResourceRequest oResourceRequest;
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
            oServiceRequest = new ServiceRequests(intProfile, dsn);
            oApplication = new Applications(intProfile, dsn);
            oVariable = new Variables(intEnvironment);
            oRequestField = new RequestFields(intProfile, dsn);
            oService = new Services(intProfile, dsn);
            oUser = new Users(intProfile, dsn);
            oCustomized = new Customized(intProfile, dsn);
            oRequest = new Requests(intProfile, dsn);
            oResourceRequest = new ResourceRequest(intProfile, dsn);
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
                LoadRequest();
                btnNext.Attributes.Add("onclick", "return ValidateHidden('" + hdnExecutive.ClientID + "','" + txtExecutive.ClientID + "','Please enter the LAN ID of your executive sponsor')" +
                    " && ValidateHidden('" + hdnWorking.ClientID + "','" + txtWorking.ClientID + "','Please enter the LAN ID of your working sponsor')" +
                    " && ValidateText('" + txtStatement.ClientID + "','Please enter a statement of work')" +
                    " && ValidateDate('" + txtStart.ClientID + "','Please enter a valid estimated start date')" +
                    " && ValidateDate('" + txtEnd.ClientID + "','Please enter a valid estimated end date')" +
                    " && ValidateDates('" + txtStart.ClientID + "','" + txtEnd.ClientID + "','The estimated start date must occur before the estimated end date')" +
                    ";");
                txtExecutive.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'300','195','" + divExecutive.ClientID + "','" + lstExecutive.ClientID + "','" + hdnExecutive.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
                lstExecutive.Attributes.Add("ondblclick", "AJAXClickRow();");
                txtWorking.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'300','195','" + divWorking.ClientID + "','" + lstWorking.ClientID + "','" + hdnWorking.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
                lstWorking.Attributes.Add("ondblclick", "AJAXClickRow();");
                imgStart.Attributes.Add("onclick", "return ShowCalendar('" + txtStart.ClientID + "');");
                imgEnd.Attributes.Add("onclick", "return ShowCalendar('" + txtEnd.ClientID + "');");
                // Custom Loads
                int intItem = Int32.Parse(lblItem.Text);
                int intApp = oRequestItem.GetItemApplication(intItem);
                string strDeliverable = oApplication.Get(intApp, "deliverables_doc");
                if (strDeliverable != "")
                {
                    panDeliverable.Visible = true;
                    btnDeliverable.Attributes.Add("onclick", "return OpenWindow('NEW_WINDOW','" + strDeliverable + "');");
                }
            }
            btnCancel1.Attributes.Add("onclick", "return confirm('Are you sure you want to cancel this service request?');");
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
        protected void LoadRequest()
        {
            int intRequest = Int32.Parse(Request.QueryString["rid"]);
            int intProject = oRequest.GetProjectNumber(intRequest);
            int intApproved = (oProject.IsApproved(intProject) ? 1 : 0);
            int intExecutive = 0;
            int intWorking = 0;
            if (intProject > 0)
            {
                intExecutive = Int32.Parse(oProject.Get(intProject, "executive"));
                intWorking = Int32.Parse(oProject.Get(intProject, "working"));
            }
            else
            {
                string strExecutive = oProjectsPending.GetRequest(intRequest, "executive");
                if (strExecutive != "")
                    intExecutive = Int32.Parse(strExecutive);
                string strWorking = oProjectsPending.GetRequest(intRequest, "working");
                if (strWorking != "")
                    intWorking = Int32.Parse(strWorking);
            }
            if (intExecutive > 0)
                txtExecutive.Text = oUser.GetFullName(intExecutive) + " (" + oUser.GetName(intExecutive) + ")";
            hdnExecutive.Value = intExecutive.ToString();
            if (intWorking > 0)
                txtWorking.Text = oUser.GetFullName(intWorking) + " (" + oUser.GetName(intWorking) + ")";
            hdnWorking.Value = intWorking.ToString();
            ProjectRequest oProjectRequest = new ProjectRequest(intProfile, dsn);
            DataSet dsProjectRequest = oProjectRequest.GetProject(intProject);
            if (dsProjectRequest.Tables[0].Rows.Count > 0 && intApproved == 1)
            {
                int intProjectRequest = Int32.Parse(dsProjectRequest.Tables[0].Rows[0]["requestid"].ToString());
                txtStatement.Text = oRequest.Get(intProjectRequest, "description");
                txtStart.Text = DateTime.Parse(oRequest.Get(intProjectRequest, "start_date")).ToShortDateString();
                txtEnd.Text = DateTime.Parse(oRequest.Get(intProjectRequest, "end_date")).ToShortDateString();
            }
            else
            {
                DataSet dsProject = oProject.Get(intProject);
                Organizations oOrganization = new Organizations(intProfile, dsn);
                if (dsProject.Tables[0].Rows.Count > 0)
                {
                    int intItem = Int32.Parse(lblItem.Text);
                    DataSet ds = oCustomized.GetTPMServiceRequests(intProject, intItem);
                    if (ds.Tables[0].Rows.Count > 0)
                        LoadPrevious(ds);
                    else
                    {
                        int intApp = oRequestItem.GetItemApplication(intItem);
                        ds = oCustomized.GetTPMServiceRequestsApplication(intProject, intApp);
                        if (ds.Tables[0].Rows.Count > 0)
                            LoadPrevious(ds);
                    }
                }
            }
        }
        protected void LoadPrevious(DataSet ds)
        {
            txtStatement.Text = ds.Tables[0].Rows[0]["statement"].ToString();
            txtStart.Text = ds.Tables[0].Rows[0]["start_date"].ToString();
            txtEnd.Text = ds.Tables[0].Rows[0]["end_date"].ToString();
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
                oProject.Update(intProject, 0, Int32.Parse(Request.Form[hdnExecutive.UniqueID]), Int32.Parse(Request.Form[hdnWorking.UniqueID]), 0, 0, 0);
            else
                oProjectsPending.Update(intRequest, 0, Int32.Parse(Request.Form[hdnExecutive.UniqueID]), Int32.Parse(Request.Form[hdnWorking.UniqueID]), 0, 0, 0);
            oCustomized.AddTPM(intRequest, intItem, intNumber, Int32.Parse(ddlPriority.SelectedItem.Value), txtStatement.Text, DateTime.Parse(txtStart.Text), DateTime.Parse(txtEnd.Text));
            oRequest.UpdateStartDate(intRequest, DateTime.Parse(txtStart.Text));
            oRequest.UpdateEndDate(intRequest, DateTime.Parse(txtEnd.Text));
            oRequestItem.UpdateForm(intRequest, true);
            Response.Redirect(oPage.GetFullLink(intPage) + "?rid=" + intRequest.ToString());
        }
        protected void btnCancel_Click(Object Sender, EventArgs e)
        {
            int intRequest = Int32.Parse(Request.QueryString["rid"]);
            ServiceRequests oServiceRequest = new ServiceRequests(intProfile, dsn);
            Requests oRequest = new Requests(intProfile, dsn);
            Projects oProject = new Projects(intProfile, dsn);
            int intProject = oRequest.GetProjectNumber(intRequest);
            DataSet ds = oRequest.Gets(intProject);
            if (ds.Tables[0].Rows.Count == 0)
                oProject.Delete(intProject);
            Response.Redirect(oPage.GetFullLink(intPage));
        }
    }
}