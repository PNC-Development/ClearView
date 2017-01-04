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
    public partial class permissions : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected Permissions oPermission;
        protected Applications oApplication;
        protected int intProfile;
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cookies["loginreferrer"].Value = "/admin/permissions.aspx";
            Response.Cookies["loginreferrer"].Expires = DateTime.Now.AddDays(30);
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Response.Redirect("/admin/login.aspx");
            oPermission = new Permissions(intProfile, dsn);
            oApplication = new Applications(intProfile, dsn);
            if (!IsPostBack)
                LoadApplications(0);
            if (Request.QueryString["expand"] != null)
            {
                btnExpand.Text = "Minimize All";
                oTreeview.ExpandAll();
            }
            else
            {
                btnExpand.Text = "Expand All";
                oTreeview.ExpandDepth = 0;
            }
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
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                oTreeview.Nodes.Add(oNode);
                LoadApplications(Int32.Parse(dr["applicationid"].ToString()));
                LoadSub(oNode, Int32.Parse(dr["applicationid"].ToString()));
                TreeNode oNew = new TreeNode();
                oNew.Text = "&nbsp;Add Group(s)";
                oNew.ToolTip = "Add Group(s)";
                oNew.ImageUrl = "/images/green_right.gif";
                oNew.NavigateUrl = "javascript:Add('" + dr["applicationid"].ToString() + "');";
                oNode.ChildNodes.Add(oNew);
            }
            oTreeview.Attributes.Add("oncontextmenu", "return false;");
        }
        private void LoadSub(TreeNode oParent, int _application)
        {
            DataSet ds = oPermission.Gets(_application);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["groupname"].ToString();
                oNode.ToolTip = dr["groupname"].ToString();
                oNode.NavigateUrl = "javascript:Edit('" + dr["permissionid"].ToString() + "');";
                oParent.ChildNodes.Add(oNode);
            }
        }
        protected  void btnExpand_Click(Object Sender, EventArgs e)
        {
            if (Request.QueryString["expand"] != null)
                Response.Redirect(Request.Path);
            else
                Response.Redirect(Request.Path + "?expand=true");
        }
    }
}
