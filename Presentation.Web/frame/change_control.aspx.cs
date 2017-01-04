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
    public partial class change_control : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);

        protected int intProfile;
        protected ResourceRequest oResourceRequest;
        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oResourceRequest = new ResourceRequest(intProfile, dsn);
            if (Request.QueryString["save"] != null)
                lblUpdate.Visible = true;
            if (!IsPostBack)
            {
                if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                {
                    int intId = Int32.Parse(Request.QueryString["id"]);
                    DataSet ds = oResourceRequest.GetChangeControl(intId);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        int intResourceWorkflow = Int32.Parse(ds.Tables[0].Rows[0]["parent"].ToString());
                        int intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);
                        int intUser = Int32.Parse(oResourceRequest.GetWorkflow(intResourceWorkflow, "userid"));
                        int intRequest = Int32.Parse(oResourceRequest.Get(intResourceParent, "requestid"));
                        Requests oRequest = new Requests(intProfile, dsn);
                        ServiceRequests oServiceRequest = new ServiceRequests(intProfile, dsn);
                        Projects oProject = new Projects(intProfile, dsn);
                        Users oUser = new Users(intProfile, dsn);
                        int intProject = oRequest.GetProjectNumber(intRequest);
                        if (intProject > 0)
                        {
                            panProject.Visible = true;
                            lblName.Text = oProject.Get(intProject, "name");
                            lblNumber.Text = oProject.Get(intProject, "number");
                        }
                        else
                        {
                            panTask.Visible = true;
                            lblTName.Text = oResourceRequest.GetWorkflow(intResourceWorkflow, "name");
                            lblTNumber.Text = "CVT" + intRequest.ToString();
                        }
                        lblImplementor.Text = oUser.GetFullName(intUser);
                        string strComments = ds.Tables[0].Rows[0]["comments"].ToString().Trim();
                        if (intProfile == intUser)
                        {
                            txtChange.Visible = true;
                            txtDate.Visible = true;
                            txtTime.Visible = true;
                            btnSubmit.Visible = true;
                            btnDelete.Visible = true;
                            txtComments.Visible = true;
                            txtChange.Text = ds.Tables[0].Rows[0]["number"].ToString();
                            txtDate.Text = DateTime.Parse(ds.Tables[0].Rows[0]["implementation"].ToString()).ToShortDateString();
                            txtTime.Text = DateTime.Parse(ds.Tables[0].Rows[0]["implementation"].ToString()).ToShortTimeString();
                            txtComments.Text = strComments;
                            btnSubmit.Attributes.Add("onclick", "return ValidateText('" + txtChange.ClientID + "','Please enter a change control number')" +
                                " && ValidateDate('" + txtDate.ClientID + "','Please enter a valid implementation date')" +
                                " && ValidateTime('" + txtTime.ClientID + "','Please enter a valid implementation time')" +
                                ";");
                            btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this change control?');");
                        }
                        else
                        {
                            lblChange.Visible = true;
                            lblDate.Visible = true;
                            lblTime.Visible = true;
                            panComments.Visible = true;
                            lblChange.Text = ds.Tables[0].Rows[0]["number"].ToString();
                            lblDate.Text = DateTime.Parse(ds.Tables[0].Rows[0]["implementation"].ToString()).ToLongDateString();
                            lblTime.Text = DateTime.Parse(ds.Tables[0].Rows[0]["implementation"].ToString()).ToLongTimeString();
                            if (strComments == "")
                                strComments = "<i>No additional information</i>";
                            lblComments.Text = strComments;
                        }
                        btnToday.Text += DateTime.Parse(ds.Tables[0].Rows[0]["implementation"].ToString()).ToShortDateString();
                    }
                }
            }
        }
        protected void btnSubmit_Click(Object Sender, EventArgs e)
        {
            int intId = Int32.Parse(Request.QueryString["id"]);
            oResourceRequest.UpdateChangeControl(intId, txtChange.Text, DateTime.Parse(txtDate.Text + " " + txtTime.Text), txtComments.Text);
            Response.Redirect(Request.Path + "?id=" + Request.QueryString["id"] + "&save=true");
        }
        protected void btnDelete_Click(Object Sender, EventArgs e)
        {
            int intId = Int32.Parse(Request.QueryString["id"]);
            oResourceRequest.DeleteChangeControl(intId);
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "reload", "<script type=\"text/javascript\">window.top.location.reload();<" + "/" + "script>");
        }
        protected void btnToday_Click(Object Sender, EventArgs e)
        {
            int intId = Int32.Parse(Request.QueryString["id"]);
            DataSet ds = oResourceRequest.GetChangeControl(intId);
            Response.Redirect("/frame/change_controls.aspx?d=" + DateTime.Parse(ds.Tables[0].Rows[0]["implementation"].ToString()).ToShortDateString());
        }
    }
}
