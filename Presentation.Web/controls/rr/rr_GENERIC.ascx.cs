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
    public partial class rr_GENERIC : System.Web.UI.UserControl
    {

        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intViewRequest = Int32.Parse(ConfigurationManager.AppSettings["ViewRequest"]);
        protected Pages oPage;
        protected RequestItems oRequestItem;
        protected ServiceRequests oServiceRequest;
        protected Applications oApplication;
        protected ServiceEditor oServiceEditor;
        protected Customized oCustomized;
        protected Requests oRequest;
        protected Projects oProject;

        protected int intProfile;
        protected int intPage = 0;
        protected int intApplication = 0;
        protected string strCustom = "";
        protected string strHidden = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oPage = new Pages(intProfile, dsn);
            oRequestItem = new RequestItems(intProfile, dsn);
            oServiceRequest = new ServiceRequests(intProfile, dsn);
            oApplication = new Applications(intProfile, dsn);
            oServiceEditor = new ServiceEditor(intProfile, dsn);
            oCustomized = new Customized(intProfile, dsn);
            oRequest = new Requests(intProfile, dsn);
            oProject = new Projects(intProfile, dsn);
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
                imgStart.Attributes.Add("onclick", "return ShowCalendar('" + txtStart.ClientID + "');");
                imgEnd.Attributes.Add("onclick", "return ShowCalendar('" + txtEnd.ClientID + "');");
                chkExpedite_Yes.Attributes.Add("onclick", "Expedite(this, '" + chkExpedite_No.ClientID + "');");
                // Custom Loads
                int intItem = Int32.Parse(lblItem.Text);
                int intApp = oRequestItem.GetItemApplication(intItem);
                string strDeliverable = oApplication.Get(intApp, "deliverables_doc");
                if (strDeliverable != "")
                {
                    panDeliverable.Visible = true;
                    btnDeliverable.Attributes.Add("onclick", "return OpenWindow('NEW_WINDOW','" + strDeliverable + "');");
                }
                int intWorkingDays = oApplication.GetLead(intApp, 3);
                if (intWorkingDays > 0)
                {
                    oApplication.AssignPriority(intApp, radPriority, lblDeliverable.ClientID, txtEnd.ClientID, hdnEnd.ClientID);
                    lblDeliverable.Text = intWorkingDays.ToString();
                    txtEnd.Text = DateTime.Today.AddDays(intWorkingDays).ToShortDateString();
                    hdnEnd.Value = DateTime.Today.AddDays(intWorkingDays).ToShortDateString();
                }
                btnDocuments.Attributes.Add("onclick", "return OpenWindow('DOCUMENTS_SECURE','?rid=" + Request.QueryString["rid"] + "');");
            }
            btnCancel1.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this service request?');");
        }
        private void LoadValues()
        {
            int intRequest = Int32.Parse(Request.QueryString["rid"]);
            DataSet dsItems;
            int intForm = 0;
            if (Request.QueryString["formid"] != null && Request.QueryString["formid"] != "")
            {
                intForm = Int32.Parse(Request.QueryString["formid"]);
                dsItems = oRequestItem.GetForm(intRequest, intForm);
            }
            else
                dsItems = oRequestItem.GetForms(intRequest);
            if (dsItems.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow drItem in dsItems.Tables[0].Rows)
                {
                    if (drItem["done"].ToString() == "-1" || intForm > 0)
                    {
                        lblItem.Text = drItem["itemid"].ToString();
                        lblNumber.Text = drItem["number"].ToString();
                        lblService.Text = drItem["serviceid"].ToString();
                        break;
                    }
                }
            }
        }
        private void LoadRequest()
        {
            int intRequest = Int32.Parse(Request.QueryString["rid"]);
            int intProject = oRequest.GetProjectNumber(intRequest);
            //DataSet dsRequest = oCustomized.GetGenericRequests(intRequest);
            //if (dsRequest.Tables[0].Rows.Count > 0)
            //{
            //    radPriority.SelectedValue = dsRequest.Tables[0].Rows[0]["priority"].ToString();
            //    txtStatement.Text = dsRequest.Tables[0].Rows[0]["statement"].ToString();
            //    bool boolExpedite = (dsRequest.Tables[0].Rows[0]["expedite"].ToString() == "1");
            //    chkExpedite_Yes.Checked = boolExpedite;
            //    chkExpedite_No.Checked = !boolExpedite;
            //    txtStart.Text = DateTime.Parse(dsRequest.Tables[0].Rows[0]["start_date"].ToString()).ToShortDateString();
            //    txtEnd.Text = DateTime.Parse(dsRequest.Tables[0].Rows[0]["end_date"].ToString()).ToShortDateString();
            //}
            //else
            //{
            radPriority.SelectedValue = "3";
            int intApproved = (oProject.IsApproved(intProject) ? 1 : 0);
            ProjectRequest oProjectRequest = new ProjectRequest(intProfile, dsn);
            DataSet dsProjectRequest = oProjectRequest.GetProject(intProject);
            int intProjectRequest = 0;
            if (dsProjectRequest.Tables[0].Rows.Count > 0 && intApproved == 1)
            {
                intProjectRequest = Int32.Parse(dsProjectRequest.Tables[0].Rows[0]["requestid"].ToString());
                txtStatement.Text = oRequest.Get(intRequest, "description");
                txtStart.Text = DateTime.Parse(oRequest.Get(intRequest, "start_date")).ToShortDateString();
                int intRequestor = Int32.Parse(oRequest.Get(intProjectRequest, "userid"));
                DataSet dsProject = oProjectRequest.GetResources(intProjectRequest);
            }
            //}

            string strRequired = "";
            //DataSet ds = oServiceEditor.GetConfig(Int32.Parse(lblItem.Text));
            //panCustom.Visible = (ds.Tables[0].Rows.Count > 0);
            //foreach (DataRow dr in ds.Tables[0].Rows)
            //{
            //    string strTable = dr["tablename"].ToString().ToUpper();
            //    string strValue = "";
            //    switch (strTable)
            //    {
            //        case "CV_GEN_COMPONENT":
            //            strCustom += "<tr>";
            //            strCustom += "<td nowrap>Component:<font class=\"required\">&nbsp;*</font></td>";
            //            strCustom += "<td width=\"100%\">";
            //            strCustom += "<select name=\"" + strTable + "\" id=\"" + strTable + "\" class=\"default\" style=\"width:200px;\" onchange=\"UpdateDDL('" + strTable + "');\">";
            //            strCustom += "<option value=\"-- SELECT --\">-- SELECT --</option>";
            //            strCustom += "<option value=\"Hardware\">Hardware</option>";
            //            strCustom += "<option value=\"Software\">Software</option>";
            //            strCustom += "<option value=\"Performance\">Performance</option>";
            //            strCustom += "<option value=\"Other...\">Other...</option>";
            //            strCustom += "</select>";
            //            strCustom += "</td>";
            //            strCustom += "</tr>";
            //            strHidden += "<input type=\"hidden\" name=\"hdnGEN_" + strTable + "\" id=\"hdnGEN_" + strTable + "\" value=\"" + strValue + "\" />";
            //            strRequired += " && ValidateHidden('hdnGEN_" + strTable + "','" + strTable + "','Please select a component')";
            //            break;
            //        case "CV_GEN_REASON":
            //            strCustom += "<tr>";
            //            strCustom += "<td nowrap>Reason:<font class=\"required\">&nbsp;*</font></td>";
            //            strCustom += "<td width=\"100%\">";
            //            strCustom += "<select name=\"" + strTable + "\" id=\"" + strTable + "\" class=\"default\" style=\"width:200px;\" onchange=\"UpdateDDL('" + strTable + "');\">";
            //            strCustom += "<option value=\"-- SELECT --\">-- SELECT --</option>";
            //            strCustom += "<option value=\"Vendor Advisory\">Vendor Advisory</option>";
            //            strCustom += "<option value=\"Break-Fix\">Break-Fix</option>";
            //            strCustom += "<option value=\"Best Practice\">Best Practice</option>";
            //            strCustom += "<option value=\"Certification\">Certification</option>";
            //            strCustom += "<option value=\"Decommission\">Decommission</option>";
            //            strCustom += "<option value=\"Capacity\">Capacity</option>";
            //            strCustom += "<option value=\"Rebuild\">Rebuild</option>";
            //            strCustom += "<option value=\"Facility\">Facility</option>";
            //            strCustom += "<option value=\"Remediation\">Remediation</option>";
            //            strCustom += "<option value=\"Maintenance\">Maintenance</option>";
            //            strCustom += "<option value=\"Health Check\">Health Check</option>";
            //            strCustom += "<option value=\"Other...\">Other...</option>";
            //            strCustom += "</select>";
            //            strCustom += "</td>";
            //            strCustom += "</tr>";
            //            strHidden += "<input type=\"hidden\" name=\"hdnGEN_" + strTable + "\" id=\"hdnGEN_" + strTable + "\" value=\"" + strValue + "\" />";
            //            strRequired += " && ValidateHidden('hdnGEN_" + strTable + "','" + strTable + "','Please select a reason')";
            //            break;
            //    }
            //}
            btnNext.Attributes.Add("onclick", "return ValidateText('" + txtStatement.ClientID + "','Please enter a statement of work')" +
                " && ValidateDate('" + txtStart.ClientID + "','Please enter a valid estimated start date')" +
                " && ValidateDate('" + txtEnd.ClientID + "','Please enter a valid estimated end date')" +
                " && ValidateDateHidden('" + txtEnd.ClientID + "','" + hdnEnd.ClientID + "','NOTE: Based on the selected priority of this request, your end date is invalid.\\n\\n')" +
                " && ValidateDates('" + txtStart.ClientID + "','" + txtEnd.ClientID + "','The estimated start date must occur before the estimated end date')" +
                strRequired +
                ";");
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
            int intExpedite = 0;
            if (chkExpedite_Yes.Checked == true)
                intExpedite = 1;
            oCustomized.AddGeneric(intRequest, intItem, intNumber, Int32.Parse(radPriority.SelectedItem.Value), txtStatement.Text, DateTime.Parse(txtStart.Text), DateTime.Parse(txtEnd.Text), intExpedite);
            foreach (string strForm in Request.Form)
            {
                if (strForm.StartsWith("hdnGEN_"))
                {
                    string strTable = strForm;
                    strTable = strTable.Substring(7);
                    //switch (strTable)
                    //{
                    //    case "CV_GEN_COMPONENT":
                    //        oServiceEditor.GEN_AddComponent(intRequest, intItem, intNumber, Request.Form[strForm]);
                    //        break;
                    //    case "CV_GEN_REASON":
                    //        oServiceEditor.GEN_AddReason(intRequest, intItem, intNumber, Request.Form[strForm]);
                    //        break;
                    //}
                }
            }

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