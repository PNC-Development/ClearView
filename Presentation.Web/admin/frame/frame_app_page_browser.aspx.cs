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
    public partial class frame_app_page_browser : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intProfile;
        protected Applications oApplication;
        protected Pages oPage;
        protected AppPages oAppPage;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Reload();
            oApplication = new Applications(intProfile, dsn);
            oPage = new Pages(intProfile, dsn);
            oAppPage = new AppPages(intProfile, dsn);
            btnClose.Attributes.Add("onclick", "return HidePanel();");
            if (!IsPostBack)
            {
                int intApplication = 0;
                if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                {
                    lblApplication.Text = Request.QueryString["applicationid"];
                    intApplication = Int32.Parse(Request.QueryString["applicationid"]);
                }
                LoadPages(null, 0, intApplication);
                if (intApplication > 0)
                    lblName.Text = oApplication.GetName(intApplication);
                else
                    lblName.Text = "GLOBAL ADD";
            }
        }
        private void LoadPages(TreeNode oParent, int _parent, int _application)
        {
            DataSet dsPages = oAppPage.Gets(_application, _parent, 0, 1);
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
                LoadPages(oNode, Int32.Parse(dr["pageid"].ToString()), _application);
            }
        }
        protected void btnSave_Click(Object Sender, EventArgs e)
        {
            int intApplication = Int32.Parse(lblApplication.Text);
            if (intApplication > 0)
            {
                oAppPage.Delete(intApplication);
                foreach (TreeNode oNode in oTree.Nodes)
                {
                    if (oNode.Checked == true)
                        oAppPage.Add(Int32.Parse(oNode.Value), intApplication);
                    SavePages(oNode);
                }
            }
            else
            {
                DataSet ds = oApplication.Gets(1);
                foreach (TreeNode oNode in oTree.Nodes)
                {
                    if (oNode.Checked == true)
                    {
                        oAppPage.DeletePage(Int32.Parse(oNode.Value));
                        foreach (DataRow dr in ds.Tables[0].Rows)
                            oAppPage.Add(Int32.Parse(oNode.Value), Int32.Parse(dr["applicationid"].ToString()));
                    }
                    SavePages(oNode);
                }
            }
            Reload();
        }
        private void SavePages(TreeNode oParent)
        {
            int intApplication = Int32.Parse(lblApplication.Text);
            if (intApplication > 0)
            {
                foreach (TreeNode oNode in oParent.ChildNodes)
                {
                    if (oNode.Checked == true)
                        oAppPage.Add(Int32.Parse(oNode.Value), intApplication);
                    SavePages(oNode);
                }
            }
            else
            {
                DataSet ds = oApplication.Gets(1);
                foreach (TreeNode oNode in oParent.ChildNodes)
                {
                    if (oNode.Checked == true)
                    {
                        oAppPage.DeletePage(Int32.Parse(oNode.Value));
                        foreach (DataRow dr in ds.Tables[0].Rows)
                            oAppPage.Add(Int32.Parse(oNode.Value), Int32.Parse(dr["applicationid"].ToString()));
                    }
                    SavePages(oNode);
                }
            }
        }
        private void Reload()
        {
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "reload", "<script type=\"text/javascript\">window.top.location.reload();<" + "/" + "script>");
        }
        protected void btnParent_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path + "?applicationid=" + lblApplication.Text);
        }
    }
}
