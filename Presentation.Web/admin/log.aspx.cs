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
    public partial class log : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private NCC.ClearView.Application.Core.Log oLog;
        protected int intProfile;
        protected int intCount;
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cookies["loginreferrer"].Value = "/admin/log.aspx";
            Response.Cookies["loginreferrer"].Expires = DateTime.Now.AddDays(30);
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Response.Redirect("/admin/login.aspx");
            intCount = 1;
            oLog = new NCC.ClearView.Application.Core.Log(intProfile, dsn);
            if (!IsPostBack)
            {
                LoopRepeater();
                btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete these records?');");
                btnDeleteAll.Attributes.Add("onclick", "return confirm('Are you sure you want to delete ALL of the records?') && confirm('ARE YOU REALLY SURE YOU WANT TO CLEAR THE LOG?!?');");
            }
        }
        private void LoopRepeater()
        {
            DataSet ds = oLog.Get();
            DataView dv = ds.Tables[0].DefaultView;
            if (Request.QueryString["sort"] != null)
                dv.Sort = Request.QueryString["sort"].ToString();
            rptView.DataSource = dv;
            rptView.DataBind();
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
        protected void btnDelete_Click(Object Sender, EventArgs e)
        {
            oLog.Delete(Int32.Parse(txtDelete.Text));
            Response.Redirect(Request.Path);
        }
        protected void btnDeleteAll_Click(Object Sender, EventArgs e)
        {
            oLog.Delete();
            Response.Redirect(Request.Path);
        }
    }
}
