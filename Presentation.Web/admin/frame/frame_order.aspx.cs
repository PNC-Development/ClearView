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
    public partial class frame_order : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intProfile;
        protected Pages oPage;
        protected AppPages oAppPage;
        private DataSet ds;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Reload();
            oPage = new Pages(intProfile, dsn);
            oAppPage = new AppPages(intProfile, dsn);
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                lblId.Text = Request.QueryString["id"];
            string strControl = "";
            if (Request.QueryString["control"] != null)
                strControl = Request.QueryString["control"];
            btnSave.Attributes.Add("onclick", "return Update('hdnUpdateOrder','" + strControl + "');");
            btnClose.Attributes.Add("onclick", "return HidePanel();");
            imgOrderUp.Attributes.Add("onclick", "return MoveOrderUp(" + lstOrder.ClientID + ");");
            imgOrderDown.Attributes.Add("onclick", "return MoveOrderDown(" + lstOrder.ClientID + ");");
            LoadList();
        }
        private void LoadList()
        {
            int intParent = oPage.GetParent(Int32.Parse(lblId.Text));
            ds = oPage.Gets(intParent, 1);
            lstOrder.DataValueField = "pageid";
            lstOrder.DataTextField = "title";
            lstOrder.DataSource = ds;
            lstOrder.DataBind();
        }
        private void Reload()
        {
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "reload", "<script type=\"text/javascript\">window.top.location.reload();<" + "/" + "script>");
        }
    }
}
