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
    public partial class frame_document_browser : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intProfile;
        protected int intApplication;
        protected Variables oVariable;
        protected Customized oCustomized;
        protected Services oService;
        protected Applications oApplication;
        protected Users oUser;
        private TreeNode oSelected;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Reload();
            oVariable = new Variables(intEnvironment);
            oCustomized = new Customized(intProfile, dsn);
            oService = new Services(intProfile, dsn);
            oApplication = new Applications(intProfile, dsn);
            oUser = new Users(intProfile, dsn);
            Int32.TryParse(Request.QueryString["applicationid"], out intApplication);
            if (intApplication > 0)
                litApplication.Text = oApplication.GetName(intApplication);

            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                lblId.Text = Request.QueryString["id"];
            string strControl = "";
            string strControlText = "";
            if (Request.QueryString["control"] != null)
                strControl = Request.QueryString["control"];
            if (Request.QueryString["controltext"] != null)
                strControlText = Request.QueryString["controltext"];
            btnNone.Attributes.Add("onclick", "return Reset(0,'" + hdnId.ClientID + "','No Document','" + txtName.ClientID + "');");
            btnSave.Attributes.Add("onclick", "return Update('" + hdnId.ClientID + "','" + strControl + "','" + txtName.ClientID + "','" + strControlText + "');");
            btnClose.Attributes.Add("onclick", "return HidePanel();");
            LoadTree();
        }
        private void LoadTree()
        {
            PopulateTree(oVariable.DocumentsFolder() + "department\\" + intApplication.ToString(), oTreeview, null);
            oTreeview.CollapseAll();
            if (oSelected == null)
                txtName.Text = "No Document";
            else
            {
                while (oSelected != null)
                {
                    oSelected.Expand();
                    oSelected = oSelected.Parent;
                }
            }
        }
        private void PopulateTree(string _path, TreeView oTree, TreeNode oParent)
        {
            //DataSet ds = oCustomized.GetDocumentRepositoryUser(intProfile, intApplication, oVariable.DocumentsFolder() + "user\\" + intProfile.ToString());
            DataSet ds = oCustomized.GetDocumentRepositoryApplication(intApplication, _path);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["name"].ToString();
                oNode.ToolTip = dr["name"].ToString();
                if (dr["type"].ToString() == "Folder")
                {
                    oNode.SelectAction = TreeNodeSelectAction.Expand;
                    PopulateTree(dr["path"].ToString(), oTree, oNode);
                }
                else
                {
                    oNode.NavigateUrl = "javascript:Select(" + dr["id"].ToString() + ",'" + hdnId.ClientID + "','" + dr["name"].ToString() + "','" + txtName.ClientID + "');";
                    if (dr["id"].ToString() == lblId.Text)
                    {
                        oSelected = oNode;
                        txtName.Text = dr["name"].ToString();
                    }
                }
                if (oParent == null)
                    oTree.Nodes.Add(oNode);
                else
                    oParent.ChildNodes.Add(oNode);
                //LoadItems(Int32.Parse(dr["applicationid"].ToString()), oNode);
            }
        }
        //private void LoadItems(int _applicationid, TreeNode oParent)
        //{
        //    DataSet ds = oRequestItem.GetItems(_applicationid, 0, 1);
        //    foreach (DataRow dr in ds.Tables[0].Rows)
        //    {
        //        TreeNode oNode = new TreeNode();
        //        oNode.Text = dr["name"].ToString();
        //        oNode.ToolTip = dr["name"].ToString();
        //        oNode.NavigateUrl = "javascript:Select(" + dr["itemid"].ToString() + ",'" + hdnId.ClientID + "','" + dr["name"].ToString() + "','" + txtName.ClientID + "');";
        //        oParent.ChildNodes.Add(oNode);
        //        if (dr["itemid"].ToString() == lblId.Text)
        //        {
        //            oSelected = oNode;
        //            txtName.Text = dr["name"].ToString();
        //        }
        //    }
        //}
        private void Reload()
        {
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "reload", "<script type=\"text/javascript\">window.top.location.reload();<" + "/" + "script>");
        }
    }
}
