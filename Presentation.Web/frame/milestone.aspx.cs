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
    public partial class milestone : BasePage
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
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
            {
                if (!IsPostBack)
                {
                    int intId = Int32.Parse(Request.QueryString["id"]);
                    DataSet ds = oResourceRequest.GetMilestone(intId);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        txtApproved.Text = DateTime.Parse(ds.Tables[0].Rows[0]["approved"].ToString()).ToShortDateString();
                        txtForecasted.Text = DateTime.Parse(ds.Tables[0].Rows[0]["forecasted"].ToString()).ToShortDateString();
                        chkComplete.Checked = (ds.Tables[0].Rows[0]["complete"].ToString() == "1");
                        txtMilestone.Text = ds.Tables[0].Rows[0]["milestone"].ToString();
                        txtDescription.Text = ds.Tables[0].Rows[0]["description"].ToString();
                        btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this item?');");
                        btnSubmit.Attributes.Add("onclick", "return ValidateDate('" + txtApproved.ClientID + "','Please enter a valid date') && ValidateDate('" + txtForecasted.ClientID + "','Please enter a valid date') && ValidateText('" + txtMilestone.ClientID + "','Please enter a milestone');");
                        imgApproved.Attributes.Add("onclick", "return OpenCalendar('" + txtApproved.ClientID + "');");
                        imgForecasted.Attributes.Add("onclick", "return OpenCalendar('" + txtForecasted.ClientID + "');");
                    }
                }
            }
        }
        protected void btnSubmit_Click(Object Sender, EventArgs e)
        {
            int intId = Int32.Parse(Request.QueryString["id"]);
            oResourceRequest.UpdateMilestone(intId, DateTime.Parse(txtApproved.Text), DateTime.Parse(txtForecasted.Text), (chkComplete.Checked ? 1 : 0), txtMilestone.Text, txtDescription.Text);
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "reload", "<script type=\"text/javascript\">window.top.location.reload();<" + "/" + "script>");
        }
        protected void btnDelete_Click(Object Sender, EventArgs e)
        {
            int intId = Int32.Parse(Request.QueryString["id"]);
            oResourceRequest.DeleteMilestone(intId);
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "reload", "<script type=\"text/javascript\">window.top.location.reload();<" + "/" + "script>");
        }
    }
}
