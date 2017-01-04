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
using System.IO;

namespace NCC.ClearView.Presentation.Web
{
    public partial class frame_file_browser : BasePage
    {
        protected int intProfile;
        protected string strLocation;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Reload();
            strLocation = "";
            if (Request.QueryString["location"] != null && Request.QueryString["location"] != "")
                strLocation = Request.QueryString["location"];
            if (strLocation == "")
                strLocation = Request.PhysicalApplicationPath;
            else
                strLocation = Server.MapPath(strLocation);
            LoadTree();
            btnClose.Attributes.Add("onclick", "return window.top.HidePanel();");
            string strControl = "";
            if (Request.QueryString["control"] != null)
                strControl = Request.QueryString["control"];
            btnSave.Attributes.Add("onclick", "return Update('" + hdnId.ClientID + "','" + strControl + "');");
        }
        private void LoadTree()
        {
            PopulateTree("", oTreeview, null);
            oTreeview.CollapseAll();
        }
        private void PopulateTree(string strFolder, TreeView oTree, TreeNode oParent)
        {
            DirectoryInfo oDir = new DirectoryInfo(strLocation + strFolder);
            DirectoryInfo[] oDirs = oDir.GetDirectories();
            TreeNode oNode;
            foreach (DirectoryInfo oInfo in oDirs)
            {
                oNode = new TreeNode();
                oNode.Text = oInfo.Name;
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                if (oParent == null)
                    oTree.Nodes.Add(oNode);
                else
                    oParent.ChildNodes.Add(oNode);
                PopulateTree(strFolder + "/" + oInfo.Name, oTree, oNode);
            }
            System.IO.FileInfo[] oFiles = oDir.GetFiles();
            foreach (System.IO.FileInfo oFile in oFiles)
            {
                oNode = new TreeNode();
                oNode.Text = oFile.Name;
                if (strLocation == Request.PhysicalApplicationPath)
                    oNode.NavigateUrl = "javascript:Select('" + strFolder + "/" + oFile.Name + "','" + hdnId.ClientID + "','" + txtName.ClientID + "');";
                else
                    oNode.NavigateUrl = "javascript:Select('" + strFolder + "/" + oFile.Name + "','" + hdnId.ClientID + "','" + txtName.ClientID + "');";
                if (oParent == null)
                    oTree.Nodes.Add(oNode);
                else
                    oParent.ChildNodes.Add(oNode);
            }
        }
        private void Reload()
        {
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "reload", "<script type=\"text/javascript\">window.top.location.reload();<" + "/" + "script>");
        }
    }
}
