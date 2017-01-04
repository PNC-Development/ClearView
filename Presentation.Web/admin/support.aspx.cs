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
    public partial class support : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected Supports oSupport;
        protected int intProfile;
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cookies["loginreferrer"].Value = "/admin/support.aspx";
            Response.Cookies["loginreferrer"].Expires = DateTime.Now.AddDays(30);
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Response.Redirect("/admin/login.aspx");
            oSupport = new Supports(intProfile, dsn);
            if (!IsPostBack)
            {
                if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                {
                    panView.Visible = true;
                    DataSet ds = oSupport.Get(Int32.Parse(Request.QueryString["id"]));
                    lblName.Text = ds.Tables[0].Rows[0]["name"].ToString();
                    lblModule.Text = ds.Tables[0].Rows[0]["menutitle"].ToString();
                    lblDescription.Text = ds.Tables[0].Rows[0]["description"].ToString();
                    lblOn.Text = ds.Tables[0].Rows[0]["modified"].ToString();
                    lblBy.Text = ds.Tables[0].Rows[0]["username"].ToString();
                    txtComments.Text = ds.Tables[0].Rows[0]["comments"].ToString();
                    ddlStatus.SelectedValue = ds.Tables[0].Rows[0]["status"].ToString();
                }
                else
                {
                    panAll.Visible = true;
                    LoopRepeater();
                    btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this item?');");
                    btnCancel.Attributes.Add("onclick", "return Cancel();");
                }
            }
        }
        private void LoopRepeater()
        {
            DataSet ds = oSupport.GetsAdmin();
            DataView dv = ds.Tables[0].DefaultView;
            if (Request.QueryString["sort"] != null)
                dv.Sort = Request.QueryString["sort"].ToString();
            rptView.DataSource = dv;
            rptView.DataBind();
            foreach (RepeaterItem ri in rptView.Items)
            {
                ImageButton oDelete = (ImageButton)ri.FindControl("btnDelete");
                oDelete.Attributes.Add("onClick", "return confirm('Are you sure you want to delete this item?');");
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
            oSupport.Update(Int32.Parse(Request.QueryString["id"]), txtComments.Text, Int32.Parse(ddlStatus.SelectedItem.Value));
            Response.Redirect(Request.Path);
        }
        protected void btnEdit_Click(Object Sender, ImageClickEventArgs e)
        {
            ImageButton oButton = (ImageButton)Sender;
            Response.Redirect(Request.Path + "?id=" + oButton.CommandArgument);
        }
        protected void btnDeleteLink_Click(Object Sender, ImageClickEventArgs e)
        {
            ImageButton oButton = (ImageButton)Sender;
            oSupport.Delete(Int32.Parse(oButton.CommandArgument));
            Response.Redirect(Request.Path);
        }
        protected void btnDelete_Click(Object Sender, EventArgs e)
        {
            oSupport.Delete(Int32.Parse(Request.QueryString["id"]));
            Response.Redirect(Request.Path);
        }
        protected void btnCancel_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path);
        }
    }
}
