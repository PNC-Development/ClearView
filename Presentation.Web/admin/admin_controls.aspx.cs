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
    public partial class admin_controls : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected Controls oControl;
        protected int intProfile;
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cookies["loginreferrer"].Value = "/admin/admin_controls.aspx";
            Response.Cookies["loginreferrer"].Expires = DateTime.Now.AddDays(30);
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Response.Redirect("/admin/login.aspx");
            oControl = new Controls(intProfile, dsn);
            if (!IsPostBack)
            {
                LoopRepeater();
                btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this control?');");
                btnBrowse.Attributes.Add("onclick", "return OpenWindow('FILEBROWSER','" + txtPath.ClientID + "','',false,400,600);");
                btnCancel.Attributes.Add("onclick", "return Cancel();");
            }
        }
        private void LoopRepeater()
        {
            DataSet ds = oControl.Gets(0, 1);
            DataView dv = ds.Tables[0].DefaultView;
            if (Request.QueryString["sort"] != null)
                dv.Sort = Request.QueryString["sort"].ToString();
            rptView.DataSource = dv;
            rptView.DataBind();
            foreach (RepeaterItem ri in rptView.Items)
            {
                ImageButton oDelete = (ImageButton)ri.FindControl("btnDelete");
                oDelete.Attributes.Add("onClick", "return confirm('Are you sure you want to delete this control?');");
                ImageButton oEnable = (ImageButton)ri.FindControl("btnEnable");
                if (oEnable.ImageUrl == "images/enabled.gif")
                {
                    oEnable.ToolTip = "Click to disable";
                    oEnable.Attributes.Add("onClick", "return confirm('Are you sure you want to disable this control?');");
                }
                else
                    oEnable.ToolTip = "Click to enable";
            }
        }
        protected void OrderView(Object Sender, EventArgs e)
        {
            LinkButton oButton = (LinkButton)Sender;
            string strSort;
            if (Request.QueryString["sort"] == null)
                strSort = oButton.CommandArgument + " ASC";
            else
                if (Request.QueryString["sort"].ToString() == (oButton.CommandArgument + " ASC"))
                    strSort = oButton.CommandArgument + " DESC";
                else
                    strSort = oButton.CommandArgument + " ASC";
            Response.Redirect(Request.Path + "?sort=" + strSort);
        }
        protected void btnAdd_Click(Object Sender, EventArgs e)
        {
            if (Request.Form[hdnId.UniqueID] == "0")
                oControl.Add(txtName.Text, txtDescription.Text, txtPath.Text, (chkSuper.Checked == true ? 1 : 0), (chkEnabled.Checked == true ? 1 : 0));
            else
                oControl.Update(Int32.Parse(Request.Form[hdnId.UniqueID]), txtName.Text, txtDescription.Text, txtPath.Text, (chkSuper.Checked == true ? 1 : 0), (chkEnabled.Checked == true ? 1 : 0));
            Response.Redirect(Request.Path);
        }
        protected void btnEnable_Click(Object Sender, ImageClickEventArgs e)
        {
            ImageButton oEnable = (ImageButton)Sender;
            oControl.Enable(Int32.Parse(oEnable.CommandArgument), (oEnable.ImageUrl == "images/enabled.gif" ? 0 : 1));
            Response.Redirect(Request.Path);
        }
        protected void btnDeleteLink_Click(Object Sender, EventArgs e)
        {
            ImageButton oDelete = (ImageButton)Sender;
            oControl.Delete(Int32.Parse(oDelete.CommandArgument));
            Response.Redirect(Request.Path);
        }
        protected void btnDelete_Click(Object Sender, EventArgs e)
        {
            oControl.Delete(Int32.Parse(Request.Form[hdnId.UniqueID]));
            Response.Redirect(Request.Path);
        }
    }
}
