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
    public partial class order_report_plots : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;

        protected Pages oPage;
        protected Reports oReport;
        protected int intProfile;
        protected int intPage;
        protected string strCalc = "";
        protected int intRepId;
        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oPage = new Pages(intProfile, dsn);
            oReport = new Reports(intProfile, dsn);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.QueryString["calc"] != null && Request.QueryString["calc"] != "")
                strCalc = Request.QueryString["calc"];
            if (Request.QueryString["repid"] != null && Request.QueryString["repid"] != "")
                intRepId = Int32.Parse(Request.QueryString["repid"]);

            if (!IsPostBack)
            {
                if (strCalc.ToLower() == "y")
                {
                    panFormula.Visible = true;
                    drpName.DataSource = oReport.GetOrderReportDataFields(intRepId);
                    drpName.DataTextField = "name";
                    drpName.DataValueField = "id";
                    drpName.DataBind();
                    drpName.Items.Insert(0, "-- SELECT --");
                    rptFormula.DataSource = oReport.GetOrderReportCalculations(intRepId);
                    rptFormula.DataBind();
                    foreach (RepeaterItem item in rptFormula.Items)
                    {
                        Panel panEdit = item.FindControl("panEdit") as Panel;
                        panEdit.Visible = true;
                        LinkButton oDelete = (LinkButton)item.FindControl("btnDelete");
                        oDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this field?');");
                    }

                }
                else
                {
                    panField.Visible = true;
                    rptPlot.DataSource = oReport.GetOrderReportDataFields(intRepId);
                    rptPlot.DataBind();
                    foreach (RepeaterItem item in rptPlot.Items)
                    {
                        Panel panEdit = item.FindControl("panEdit") as Panel;
                        panEdit.Visible = true;
                        LinkButton oDelete = (LinkButton)item.FindControl("btnDelete");
                        oDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this field?');");
                    }
                }
            }
            lblNoFields.Visible = rptPlot.Items.Count == 0;
            lblNoFormula.Visible = rptFormula.Items.Count == 0;

            if (panField.Visible == true)
                btnAdd.Attributes.Add("onclick", "return ValidateText('" + txtName.ClientID + "','Please enter a field name') " +
                         "&& ValidateDropDown('" + drpType.ClientID + "','Please select a field type') " +
                         ";");
            else
                btnAddFormula.Attributes.Add("onclick", "return ValidateDropDown('" + drpName.ClientID + "','Please select a field name') " +
                         ";");

        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {

            if (Request.Form[hdnVal.UniqueID] == "Update")
            {
                if (strCalc.ToLower() == "y")
                    oReport.UpdateOrderReportCalculation(Int32.Parse(Request.Form[hdnId.UniqueID]), intRepId, Int32.Parse(drpName.SelectedValue), txtFormula.Text);
                else
                    oReport.UpdateOrderReportDataField(Int32.Parse(Request.Form[hdnId.UniqueID]), intRepId, txtName.Text, drpType.SelectedItem.Text);
            }
            else
            {
                if (strCalc.ToLower() == "y")
                    oReport.AddOrderReportCalculation(intRepId, Int32.Parse(drpName.SelectedValue), txtFormula.Text);
                else
                    oReport.AddOrderReportDataFields(intRepId, txtName.Text, drpType.SelectedItem.Text);

            }
            ClientScript.RegisterClientScriptBlock(typeof(Page), "redirect", "window.navigate(window.location)", true);
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            LinkButton oButton = (LinkButton)sender;
            int intId = Int32.Parse(oButton.CommandArgument);
            if (strCalc.ToLower() == "y")
                oReport.DeleteOrderReportCalculation(intId);
            else
                oReport.DeleteOrderReportDataField(intId);
            ClientScript.RegisterClientScriptBlock(typeof(Page), "redirect", "window.navigate(window.location)", true);
        }
    }
}
