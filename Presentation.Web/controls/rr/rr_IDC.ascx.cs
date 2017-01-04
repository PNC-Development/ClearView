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
    public partial class rr_IDC : System.Web.UI.UserControl
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
        protected Customized oCustom;
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
            oCustom = new Customized(intProfile, dsn);
            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            if (Request.QueryString["rid"] != "" && Request.QueryString["rid"] != null)
            {
                int intRequest = Int32.Parse(Request.QueryString["rid"]);
                LoadValues();
                // Vijay -- Added Validation Check
                btnNext.Attributes.Add("onclick", "return ValidateDates('" + txtStart.ClientID + "','" + txtEnd.ClientID + "','The estimated start date must occur before the estimated end date');");
                imgStart.Attributes.Add("onclick", "return ShowCalendar('" + txtStart.ClientID + "');");
                imgEnd.Attributes.Add("onclick", "return ShowCalendar('" + txtEnd.ClientID + "');");

                // Vijay -- Added onclick attributes to followup and date engaged fields
                imgDate.Attributes.Add("onclick", "return ShowCalendar('" + txtDateEngaged.ClientID + "');");
                imgFollowUpDate.Attributes.Add("onclick", "return ShowCalendar('" + txtFollowupDate.ClientID + "');");


                // Custom Loads
                int intItem = Int32.Parse(lblItem.Text);
                int intApp = oRequestItem.GetItemApplication(intItem);
                string strDeliverable = oApplication.Get(intApp, "deliverables_doc");
                if (strDeliverable != "")
                {
                    panDeliverable.Visible = true;
                    btnDeliverable.Attributes.Add("onclick", "return OpenWindow('NEW_WINDOW','" + strDeliverable + "');");
                }
                int intNumber = Int32.Parse(lblNumber.Text);
                rptAssets.DataSource = oCustom.GetTechAssets(intRequest, intItem, intNumber);
                rptAssets.DataBind();
                rptResource.DataSource = oCustom.GetResourceAssignments(intRequest, intItem, intNumber);
                rptResource.DataBind();
                lblNoAsset.Visible = rptAssets.Items.Count == 0;
                lblNoRes.Visible = rptResource.Items.Count == 0;
                foreach (RepeaterItem ri in rptAssets.Items)
                {
                    Panel panEdit = (Panel)ri.FindControl("panEditable");
                    LinkButton btnDelete = (LinkButton)panEdit.FindControl("btnDelete");

                    btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this asset?');");
                    panEdit.Visible = true;
                }
                if (IsPostBack)
                {
                    int intInvestigatedBy = 0;
                    if (Request.Form[hdnInvestigatedBy.UniqueID] != "")
                        intInvestigatedBy = Int32.Parse(Request.Form[hdnInvestigatedBy.UniqueID]);
                    int intIDCSPOC = 0;
                    if (Request.Form[hdnIDCSPOC.UniqueID] != "")
                        intIDCSPOC = Int32.Parse(Request.Form[hdnIDCSPOC.UniqueID]);
                    oCustomized.AddIDCDetails(intRequest, intItem, intNumber, drpInvestigated.SelectedItem.Text, intInvestigatedBy, txtFollowupDate.Text, txtDateEngaged.Text, drpPhase.SelectedItem.Text, drpEffortSize.SelectedItem.Text, drpInvolvement.SelectedItem.Text, drpEIT.SelectedItem.Text, drpProjectClass.SelectedItem.Text, drpEnterprise.SelectedItem.Text, drpNoInvolve.SelectedItem.Text, intIDCSPOC, txtComment.Text);
                    try
                    {
                        oRequest.UpdateStartDate(intRequest, DateTime.Parse(txtStart.Text));
                        oRequest.UpdateEndDate(intRequest, DateTime.Parse(txtEnd.Text));
                    }
                    catch { }
                }
                else
                {
                    DataSet dsInvestigation = oCustom.GetIDCDetails(intRequest, intItem, intNumber);
                    if (dsInvestigation.Tables[0].Rows.Count > 0)
                    {
                        int intSPOC = 0;
                        if (dsInvestigation.Tables[0].Rows[0]["idc_spoc"].ToString() != "0")
                        {
                            intSPOC = Int32.Parse(dsInvestigation.Tables[0].Rows[0]["idc_spoc"].ToString());
                            txtIDCSPOC.Text = oUser.GetFullName(intSPOC) + " (" + oUser.GetName(intSPOC) + ")";
                        }
                        hdnIDCSPOC.Value = intSPOC.ToString();
                        int intInvestigatedBy = 0;
                        if (dsInvestigation.Tables[0].Rows[0]["investigated_by"].ToString() != "0")
                        {
                            intInvestigatedBy = Int32.Parse(dsInvestigation.Tables[0].Rows[0]["investigated_by"].ToString());
                            txtInvestigatedBy.Text = oUser.GetFullName(intInvestigatedBy) + " (" + oUser.GetName(intInvestigatedBy) + ")";
                        }
                        hdnInvestigatedBy.Value = intInvestigatedBy.ToString();
                        drpInvestigated.SelectedValue = dsInvestigation.Tables[0].Rows[0]["investigated"].ToString();
                        if (dsInvestigation.Tables[0].Rows[0]["followup_date"].ToString() != "")
                            txtFollowupDate.Text = DateTime.Parse(dsInvestigation.Tables[0].Rows[0]["followup_date"].ToString()).ToShortDateString();
                        if (dsInvestigation.Tables[0].Rows[0]["date_engaged"].ToString() != "")
                            txtDateEngaged.Text = DateTime.Parse(dsInvestigation.Tables[0].Rows[0]["date_engaged"].ToString()).ToShortDateString();
                        drpPhase.SelectedValue = dsInvestigation.Tables[0].Rows[0]["phase_engaged"].ToString();
                        drpEffortSize.SelectedValue = dsInvestigation.Tables[0].Rows[0]["effort_size"].ToString();
                        drpInvolvement.SelectedValue = dsInvestigation.Tables[0].Rows[0]["involvement"].ToString();
                        if (drpInvolvement.SelectedItem.Value == "No")
                            divInvolvement.Style["display"] = "inline";
                        drpEIT.SelectedValue = dsInvestigation.Tables[0].Rows[0]["eit_testing"].ToString();
                        drpProjectClass.SelectedValue = dsInvestigation.Tables[0].Rows[0]["project_class"].ToString();
                        drpEnterprise.SelectedValue = dsInvestigation.Tables[0].Rows[0]["enterprise_release"].ToString();
                        drpNoInvolve.SelectedValue = dsInvestigation.Tables[0].Rows[0]["no_involve"].ToString();
                        txtComment.Text = dsInvestigation.Tables[0].Rows[0]["comments"].ToString();
                    }
                }



                foreach (RepeaterItem ri in rptResource.Items)
                {
                    int id = Int32.Parse(((Label)ri.FindControl("lblResID")).Text);
                    Label lblResourceType = (Label)ri.FindControl("lblResourceType");
                    Label lblRequestedDate = (Label)ri.FindControl("lblRequestedDate");
                    Label lblFulfillDate = (Label)ri.FindControl("lblFulfillDate");
                    lblRequestedDate.Text = Convert.ToDateTime(lblRequestedDate.Text).ToShortDateString();
                    lblFulfillDate.Text = Convert.ToDateTime(lblFulfillDate.Text).ToShortDateString();
                    lblResourceType.Text = oCustom.GetResourceTypeName(id, "name");
                    Panel panEdit = (Panel)ri.FindControl("panEditable");
                    LinkButton btnDelete = (LinkButton)panEdit.FindControl("btnDelete");
                    btnDelete.Attributes.Add("onclick", " return confirm('Are you sure you want to delete this resource?') ;");
                    panEdit.Visible = true;
                }
                btnAddRes.Attributes.Add("onclick", "return EditResource('" + intRequest + "','" + intItem + "','" + intNumber + "','0');");
                btnAddAsset.Attributes.Add("onclick", "return EditAsset('" + intRequest + "','" + intItem + "','" + intNumber + "','0');");
            }
            txtInvestigatedBy.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'200','195','" + divAJAX.ClientID + "','" + lstAJAX.ClientID + "','" + hdnInvestigatedBy.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
            lstAJAX.Attributes.Add("ondblclick", "AJAXClickRow();");
            txtIDCSPOC.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'200','195','" + divIDCSPOC.ClientID + "','" + lstIDCSPOC.ClientID + "','" + hdnIDCSPOC.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
            lstIDCSPOC.Attributes.Add("ondblclick", "AJAXClickRow();");
            drpInvolvement.Attributes.Add("onchange", "ShowInvolvementReason(this,'" + divInvolvement.ClientID + "');");
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
            // Vijay -- Added code to add a new record in cv_WM_IDC 
            int intInvestigatedBy = 0;
            if (Request.Form[hdnInvestigatedBy.UniqueID] != "")
                intInvestigatedBy = Int32.Parse(Request.Form[hdnInvestigatedBy.UniqueID]);
            int intIDCSPOC = 0;
            if (Request.Form[hdnIDCSPOC.UniqueID] != "")
                intIDCSPOC = Int32.Parse(Request.Form[hdnIDCSPOC.UniqueID]);
            oCustomized.AddIDCDetails(intRequest, intItem, intNumber, drpInvestigated.SelectedItem.Text, intInvestigatedBy, txtFollowupDate.Text, txtDateEngaged.Text, drpPhase.SelectedItem.Text, drpEffortSize.SelectedItem.Text, drpInvolvement.SelectedItem.Text, drpEIT.SelectedItem.Text, drpProjectClass.SelectedItem.Text, drpEnterprise.SelectedItem.Text, drpNoInvolve.SelectedItem.Text, intIDCSPOC, txtComment.Text);
            try
            {
                oRequest.UpdateStartDate(intRequest, DateTime.Parse(txtStart.Text));
                oRequest.UpdateEndDate(intRequest, DateTime.Parse(txtEnd.Text));
            }
            catch { }
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
        protected void btnDelete_Click(object Sender, EventArgs e)
        {
            LinkButton oButton = (LinkButton)Sender;
            int intId = Int32.Parse(oButton.CommandArgument);
            if (oButton.CommandName == "Asset")
                oCustom.DeleteTechAsset(intId);
            else
                oCustom.DeleteResourceAssignment(intId);
            Response.Redirect(oPage.GetFullLink(intPage) + "?rid=" + Request.QueryString["rid"]);
        }
    }
}