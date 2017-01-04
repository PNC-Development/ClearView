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
    public partial class weekly_status : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);

        protected DataSet dsStatus;
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
                    if (Request.QueryString["type"] == "t")
                    {
                        panTPMPC.Visible = true;
                        dsStatus = oResourceRequest.GetStatusTPM(intId);
                        if (dsStatus.Tables[0].Rows.Count > 0)
                        {
                            double dblScope = double.Parse(dsStatus.Tables[0].Rows[0]["scope"].ToString());
                            double dblTimeline = double.Parse(dsStatus.Tables[0].Rows[0]["timeline"].ToString());
                            double dblBudget = double.Parse(dsStatus.Tables[0].Rows[0]["budget"].ToString());
                            if (dblTimeline > 0.00)
                            {
                                panTPM.Visible = true;
                                lblScope.Text = oResourceRequest.GetStatus(dblScope, 75, 15);
                                lblTimeline.Text = oResourceRequest.GetStatus(dblTimeline, 75, 15);
                                lblBudget.Text = oResourceRequest.GetStatus(dblBudget, 75, 15);
                                lblScopeD.Text = DateTime.Parse(dsStatus.Tables[0].Rows[0]["datestamp"].ToString()).ToShortDateString();
                            }
                            else
                            {
                                panPC.Visible = true;
                                lblVariance.Text = oResourceRequest.GetStatus(dblScope, 75, 15);
                                lblVarianceD.Text = DateTime.Parse(dsStatus.Tables[0].Rows[0]["datestamp"].ToString()).ToShortDateString();
                            }
                            txtComments.Text = dsStatus.Tables[0].Rows[0]["comments"].ToString();
                            txtThis.Text = dsStatus.Tables[0].Rows[0]["thisweek"].ToString();
                            txtNext.Text = dsStatus.Tables[0].Rows[0]["nextweek"].ToString();
                            btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this item?');");
                            if (dblScope == 3.00 && dblTimeline == 3.00 && dblBudget == 3.00)
                            {
                                btnSubmit.Attributes.Add("onclick", "return ValidateText('" + txtThis.ClientID + "','Please enter some information for this weeks accomplishments')" +
                                    " && ValidateText('" + txtNext.ClientID + "','Please enter some information for next weeks accomplishments')" +
                                    ";");
                            }
                            else
                            {
                                btnSubmit.Attributes.Add("onclick", "return ValidateText('" + txtThis.ClientID + "','Please enter some information for this weeks accomplishments')" +
                                    " && ValidateText('" + txtNext.ClientID + "','Please enter some information for next weeks accomplishments')" +
                                    " && ValidateText('" + txtComments.ClientID + "','Please enter some comments')" +
                                    ";");
                            }
                        }
                    }
                    if (Request.QueryString["type"] == "r")
                    {
                        panResource.Visible = true;
                        dsStatus = oResourceRequest.GetStatus(intId);
                        if (dsStatus.Tables[0].Rows.Count > 0)
                        {
                            lblStatus.Text = oResourceRequest.GetStatus(double.Parse(dsStatus.Tables[0].Rows[0]["status"].ToString()), 75, 15);
                            lblComments2.Text = dsStatus.Tables[0].Rows[0]["comments"].ToString();
                        }
                    }
                }
            }
        }
        protected void btnSubmit_Click(Object Sender, EventArgs e)
        {
            int intId = Int32.Parse(Request.QueryString["id"]);
            oResourceRequest.UpdateStatusTPM(intId, txtComments.Text, txtThis.Text, txtNext.Text);
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "reload", "<script type=\"text/javascript\">window.top.location.reload();<" + "/" + "script>");
        }
        protected void btnDelete_Click(Object Sender, EventArgs e)
        {
            int intId = Int32.Parse(Request.QueryString["id"]);
            oResourceRequest.DeleteStatusTPM(intId);
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "reload", "<script type=\"text/javascript\">window.top.location.reload();<" + "/" + "script>");
        }
    }
}
