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
    public partial class frame_image : BasePage
    {
        string strStartDir = "/images";
        string strControl;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["align"] != null)
            {
                try { ddlAlignment.SelectedValue = Request.QueryString["align"].ToString(); }
                catch (Exception exception)
                {
                    LogException(exception);
                }
            }

            strControl = "";
            if (Request.QueryString["control"] != null)
                strControl = Request.QueryString["control"];
            if (Request.Form["hdnFile"] != null && Request.Form["hdnFile"] != "")
            {
                string strDelete = Request.Form["hdnFile"].ToString().Replace("/", "\\");
                System.IO.FileInfo oFileDelete = new System.IO.FileInfo(Request.PhysicalApplicationPath + strDelete.Substring(1));
                if (oFileDelete.Exists == true)
                    oFileDelete.Delete();
                Response.Redirect(Request.Path + "?type=" + Request.QueryString["type"] + (strControl == "" ? "" : "&control=" + strControl));
            }
            btnInsert.Attributes.Add("onclick", "return btnInsert_Click('" + strControl + "')");
            btnDelete.Attributes.Add("onclick", "return btnDelete_Click();");
            btnClose.Attributes.Add("onclick", "return window.top.HidePanel();");
            LoadTree();
        }

        private void LoadTree()
        {
            PopulateTree(strStartDir, oTreeview, null);
            oTreeview.CollapseAll();
        }
        private void PopulateTree(string strFolder, TreeView oTree, TreeNode oParent)
        {
            DirectoryInfo oDir = new DirectoryInfo(Request.PhysicalApplicationPath + strFolder);
            DirectoryInfo[] oDirs = oDir.GetDirectories();
            TreeNode oNode;
            foreach (DirectoryInfo oInfo in oDirs)
            {
                oNode = new TreeNode();
                oNode.Text = oInfo.Name;
                //            oNode.NavigateUrl = "javascript:FolderClick('" + strFolder + "/" + oInfo.Name + "');";
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
                if (oFile.Extension == ".bmp" || oFile.Extension == ".gif" || oFile.Extension == ".jpg" || oFile.Extension == ".jpeg" || oFile.Extension == ".png" || oFile.Extension == ".tif")
                {
                    oNode = new TreeNode();
                    oNode.Text = oFile.Name;
                    oNode.NavigateUrl = "javascript:ImageClick('" + strFolder + "/" + oFile.Name + "');";
                    if (oParent == null)
                        oTree.Nodes.Add(oNode);
                    else
                        oParent.ChildNodes.Add(oNode);
                }
            }
        }

        protected  void btnUpload_Click(Object Sender, EventArgs e)
        {
            if (oUpload.FileName != "" && oUpload.PostedFile != null)
            {
                try
                {
                    oUpload.PostedFile.SaveAs(Request.PhysicalApplicationPath + strStartDir.Replace("/", "\\") + "\\" + oUpload.FileName);
                    Response.Redirect(Request.Path + "?type=" + Request.QueryString["type"] + (strControl == "" ? "" : "&control=" + strControl) + "&src=" + strStartDir + "/" + oUpload.FileName);
                }
                catch { }
            }
        }    
    }

}