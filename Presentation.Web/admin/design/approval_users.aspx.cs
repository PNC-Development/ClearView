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
    public partial class design_approval_users : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intProfile;
        protected Design oDesign;
       
        protected int intID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Reload();
            oDesign = new Design(intProfile, dsn);
            if (Request.QueryString["save"] != null && Request.QueryString["save"] != "")
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "save", "<script type=\"text/javascript\">alert('User Added Successfully');<" + "/" + "script>");
            if (Request.QueryString["delete"] != null && Request.QueryString["delete"] != "")
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "delete", "<script type=\"text/javascript\">alert('User Deleted Successfully');<" + "/" + "script>");
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
            {
                intID = Int32.Parse(Request.QueryString["id"]);
                if (!IsPostBack)
                {
                    lblName.Text = oDesign.GetApprovalGroup(intID, "name");
                    rptItems.DataSource = oDesign.GetApprovalUsers(intID);
                    rptItems.DataBind();
                    lblNone.Visible = (rptItems.Items.Count == 0);
                    foreach (RepeaterItem ri in rptItems.Items)
                        ((LinkButton)ri.FindControl("btnDelete")).Attributes.Add("onclick", "return confirm('Are you sure you want to delete this item?');");
                }
                btnClose.Attributes.Add("onclick", "return HidePanel();");
                btnAdd.Attributes.Add("onclick", "return EnsureHidden('" + hdnUser.ClientID + "','" + txtUser.ClientID + "','Please enter the LAN ID of the user')" +
                    ";");
                Variables oVariable = new Variables(intEnvironment);
                txtUser.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'300','195','" + divAJAX.ClientID + "','" + lstAJAX.ClientID + "','" + hdnUser.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
                lstAJAX.Attributes.Add("ondblclick", "AJAXClickRow();");
            }
        }
        protected void btnAdd_Click(Object Sender, EventArgs e)
        {
            oDesign.AddApprovalUser(intID, Int32.Parse(Request.Form[hdnUser.UniqueID]));
            Response.Redirect(Request.Path + "?id=" + intID.ToString() + "&save=true");
        }
        protected void btnDelete_Click(Object Sender, EventArgs e)
        {
            LinkButton oButton = (LinkButton)Sender;
            oDesign.DeleteApprovalUser(Int32.Parse(oButton.CommandArgument));
            Response.Redirect(Request.Path + "?id=" + intID.ToString() + "&delete=true");
        }
        private void Reload()
        {
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "reload", "<script type=\"text/javascript\">window.top.location.reload();<" + "/" + "script>");
        }
    }
}
