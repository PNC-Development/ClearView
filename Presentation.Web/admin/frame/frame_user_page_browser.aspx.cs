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
    public partial class frame_user_page_browser : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intProfile;
        protected Users oUser;
        protected Pages oPage;
        protected int intUser = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Reload();
            oUser = new Users(intProfile, dsn);
            oPage = new Pages(intProfile, dsn);
            btnClose.Attributes.Add("onclick", "return HidePanel();");
            if (Request.QueryString["userid"] != null && Request.QueryString["userid"] != "")
                intUser = Int32.Parse(Request.QueryString["userid"]);
            if (!IsPostBack)
            {
                LoadPages(null, 0, intUser);
                if (intUser > 0)
                    lblName.Text = oUser.GetFullName(intUser);
                else
                    btnSave.Enabled = false;
            }
        }
        private void LoadPages(TreeNode oParent, int _parent, int _userid)
        {
            DataSet dsPages = oUser.GetPages(_userid, _parent, 0, 1);
            DataSet ds = oPage.Gets(_parent, 1);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["title"].ToString();
                oNode.ToolTip = dr["title"].ToString();
                oNode.Value = dr["pageid"].ToString();
                oNode.SelectAction = TreeNodeSelectAction.None;
                oNode.Checked = false;
                foreach (DataRow drPage in dsPages.Tables[0].Rows)
                {
                    if (dr["pageid"].ToString() == drPage["pageid"].ToString())
                        oNode.Checked = true;
                }
                if (oParent != null)
                    oParent.ChildNodes.Add(oNode);
                else
                    oTree.Nodes.Add(oNode);
                LoadPages(oNode, Int32.Parse(dr["pageid"].ToString()), _userid);
            }
        }
        protected void btnSave_Click(Object Sender, EventArgs e)
        {
            oUser.DeletePage(intUser);
            foreach (TreeNode oNode in oTree.Nodes)
            {
                if (oNode.Checked == true)
                    oUser.AddPage(Int32.Parse(oNode.Value), intUser);
                SavePages(oNode);
            }
            Reload();
        }
        private void SavePages(TreeNode oParent)
        {
            foreach (TreeNode oNode in oParent.ChildNodes)
            {
                if (oNode.Checked == true)
                    oUser.AddPage(Int32.Parse(oNode.Value), intUser);
                SavePages(oNode);
            }
        }
        private void Reload()
        {
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "reload", "<script type=\"text/javascript\">window.top.location.reload();<" + "/" + "script>");
        }
    }
}
