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
    public partial class service_task : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);

        protected ServiceDetails oServiceDetail;
        protected Services oService;
        protected ResourceRequest oResourceRequest;
        protected int intID = 0;
        protected int intService = 0;
        protected int intParent = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            oServiceDetail = new ServiceDetails(0, dsn);
            oService = new Services(0, dsn);
            oResourceRequest = new ResourceRequest(0, dsn);
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                intID = Int32.Parse(Request.QueryString["id"]);
            if (Request.QueryString["parent"] != null && Request.QueryString["parent"] != "")
                intParent = Int32.Parse(Request.QueryString["parent"]);
            if (Request.QueryString["service"] != null && Request.QueryString["service"] != "")
                intService = Int32.Parse(Request.QueryString["service"]);
            if (Request.QueryString["add"] != null && Request.QueryString["add"] != "")
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "add", "<script type=\"text/javascript\">window.parent.navigate(window.parent.location);<" + "/" + "script>");
            if (Request.QueryString["update"] != null && Request.QueryString["update"] != "")
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "update", "<script type=\"text/javascript\">window.parent.navigate(window.parent.location);<" + "/" + "script>");
            if (Request.QueryString["delete"] != null && Request.QueryString["delete"] != "")
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "delete", "<script type=\"text/javascript\">window.parent.navigate(window.parent.location);<" + "/" + "script>");
            if (!IsPostBack)
            {
                if (intID > 0)
                {
                    DataSet ds = oServiceDetail.Get(intID);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        lblService.Text = oService.GetName(intService);
                        lblParent.Text = (intParent == 0 ? "No Parent" : oServiceDetail.GetName(intParent));
                        txtName.Text = ds.Tables[0].Rows[0]["name"].ToString();
                        txtHours.Text = double.Parse(ds.Tables[0].Rows[0]["hours"].ToString()).ToString("F");
                        chkEnabled.Checked = (ds.Tables[0].Rows[0]["enabled"].ToString() == "1");
                        btnSave.Text = "Update";
                        btnSave.Enabled = true;
                        btnDelete.Enabled = true;
                        btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this task?');");
                    }
                }
                else if (intService > 0)
                {
                    lblService.Text = oService.GetName(intService);
                    lblParent.Text = (intParent == 0 ? "No Parent" : oServiceDetail.GetName(intParent));
                    btnSave.Text = "Add";
                    btnSave.Enabled = true;
                }
                btnSave.Attributes.Add("onclick", "return ValidateText('" + txtName.ClientID + "','Please enter a task name')" +
                    " && ValidateNumber('" + txtHours.ClientID + "','Please enter a valid number')" +
                    ";");
            }
        }
        protected void btnSave_Click(Object Sender, EventArgs e)
        {
            double dblHours = double.Parse(txtHours.Text);
            if (Request.Form["hdnMinutes"] == "1")
                dblHours = dblHours / 60.00;
            if (intID == 0)
            {
                intID = oServiceDetail.Add(intService, txtName.Text, intParent, dblHours, dblHours, 1, (oServiceDetail.Gets(intService, intParent, 0).Tables[0].Rows.Count + 1), (chkEnabled.Checked ? 1 : 0));
                Response.Redirect(Request.Path + "?id=" + intID.ToString() + "&add=true");
            }
            else
            {
                oServiceDetail.Update(intID, txtName.Text, intParent, dblHours, dblHours, 1, (chkEnabled.Checked ? 1 : 0));
                Response.Redirect(Request.Path + "?id=" + intID.ToString() + "&update=true");
            }
            oResourceRequest.UpdateWorkflowAllocated(intService);
        }
        protected void btnDelete_Click(Object Sender, EventArgs e)
        {
            oServiceDetail.Delete(intID);
            Response.Redirect(Request.Path + "?delete=true");
        }
    }
}
