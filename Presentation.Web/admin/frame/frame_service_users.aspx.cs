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
    public partial class frame_service_users : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intProfile;
        protected Services oService;
       
        protected int intID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Reload();
            oService = new Services(intProfile, dsn);

            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
            {
                intID = Int32.Parse(Request.QueryString["id"]);
                if (!IsPostBack)
                {
                    lblName.Text = oService.GetName(intID);
                    rptItems.DataSource = oService.GetUsers(intID);
                    rptItems.DataBind();
                    lblNone.Visible = (rptItems.Items.Count == 0);
                    foreach (RepeaterItem ri in rptItems.Items)
                    {
                        LinkButton btnDelete = (LinkButton)ri.FindControl("btnDelete");
                        if (btnDelete.CommandArgument == "0")
                            btnDelete.Attributes.Add("onclick", "alert('You cannot delete this item from here. You have to change the \"Workflow Assignment\" option on the service administration page.');return false;");
                        else
                            btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this item?');");
                    }
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
            oService.AddUser(intID, Int32.Parse(Request.Form[hdnUser.UniqueID]), Int32.Parse(ddlLevel.SelectedItem.Value));
            Response.Redirect(Request.Path + "?id=" + intID.ToString() + "&save=true");
        }
        protected void btnDelete_Click(Object Sender, EventArgs e)
        {
            LinkButton oButton = (LinkButton)Sender;
            oService.DeleteUser(Int32.Parse(oButton.CommandArgument));
            Response.Redirect(Request.Path + "?id=" + intID.ToString() + "&delete=true");
        }
        private void Reload()
        {
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "reload", "<script type=\"text/javascript\">window.top.location.reload();<" + "/" + "script>");
        }
    }
}
