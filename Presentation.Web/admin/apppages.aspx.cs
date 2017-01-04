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
    public partial class apppages : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected Pages oPage;
        protected Applications oApplication;
        protected Templates oTemplate;
        protected AppPages oAppPage;
        protected int intProfile;
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cookies["loginreferrer"].Value = "/admin/apppages.aspx";
            Response.Cookies["loginreferrer"].Expires = DateTime.Now.AddDays(30);
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Response.Redirect("/admin/login.aspx");
            oPage = new Pages(intProfile, dsn);
            oApplication = new Applications(intProfile, dsn);
            oTemplate = new Templates(intProfile, dsn);
            oAppPage = new AppPages(intProfile, dsn);
            if (!IsPostBack)
                LoadApplications(0);
        }
        private void LoadApplications(int _parent)
        {
            DataSet ds = oApplication.Gets(_parent, 1);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["name"].ToString();
                oNode.ToolTip = dr["name"].ToString();
                oNode.ImageUrl = "/images/folder.gif";
                //            oNode.SelectAction = TreeNodeSelectAction.Expand;
                oNode.NavigateUrl = "javascript:Edit('" + dr["applicationid"].ToString() + "','" + _parent.ToString() + "');";
                oTreeview.Nodes.Add(oNode);
                LoadApplications(Int32.Parse(dr["applicationid"].ToString()));
                //LoadPages(oNode, 0, Int32.Parse(dr["applicationid"].ToString()));
                //TreeNode oNew = new TreeNode();
                //oNew.Text = "&nbsp;Modify Pages";
                //oNew.ToolTip = "Modify Pages";
                //oNew.ImageUrl = "/images/green_right.gif";
                //oNew.NavigateUrl = "javascript:Edit('" + dr["applicationid"].ToString() + "','" + _parent.ToString() + "');";
                //oNode.ChildNodes.Add(oNew);
            }
            oTreeview.ExpandDepth = 0;
            oTreeview.Attributes.Add("oncontextmenu", "return false;");
        }
        private void LoadPages(TreeNode oParent, int _parent, int _application)
        {
            DataSet ds = oAppPage.Gets(_application, _parent, 0, 1);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["title"].ToString();
                oNode.ToolTip = dr["title"].ToString();
                oNode.SelectAction = TreeNodeSelectAction.None;
                oParent.ChildNodes.Add(oNode);
                LoadPages(oNode, Int32.Parse(dr["pageid"].ToString()), _application);
            }
        }
    }
}
