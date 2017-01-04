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
    public partial class service_folders : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected Services oService;
        protected Users oUser;
        protected int intProfile;
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cookies["loginreferrer"].Value = "/admin/service_folders.aspx";
            Response.Cookies["loginreferrer"].Expires = DateTime.Now.AddDays(30);
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Response.Redirect("/admin/login.aspx");
            oService = new Services(intProfile, dsn);
            oUser = new Users(intProfile, dsn);
            if (!IsPostBack)
            {
                LoadList();
                LoadServices(0);
                btnOrder.Attributes.Add("onclick", "return OpenWindow('SUPPORTORDER','" + hdnParent.ClientID + "','" + hdnOrder.ClientID + "&type=SERVICE_FOLDERS" + "',false,400,400);");
                btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this service?');");
                btnCancel.Attributes.Add("onclick", "return Cancel();");
                btnParent.Attributes.Add("onclick", "return OpenWindow('SERVICEFOLDERBROWSER','" + hdnParent.ClientID + "','&control=" + hdnParent.ClientID + "&controltext=" + lblParent.ClientID + "',false,400,600);");
            }
        }
        private void LoadList()
        {
        }
        private void LoadServices(int _parent)
        {
            TreeNode oNode = new TreeNode();
            oNode.Text = "ClearView Services";
            oNode.ToolTip = "ClearView Services";
            oNode.ImageUrl = "/images/folder.gif";
            oNode.SelectAction = TreeNodeSelectAction.Expand;
            oTreeview.Nodes.Add(oNode);
            LoadServices(0, oNode);
            TreeNode oNew = new TreeNode();
            oNew.Text = "&nbsp;Add Folder";
            oNew.ToolTip = "Add Folder";
            oNew.ImageUrl = "/images/green_right.gif";
            oNew.NavigateUrl = "javascript:Add('0','No Parent');";
            oNode.ChildNodes.Add(oNew);
        }
        private void LoadServices(int _parent, TreeNode oParent)
        {
            DataSet ds = oService.GetFolders(_parent, 0);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["name"].ToString();
                oNode.ToolTip = dr["name"].ToString();
                oNode.ImageUrl = "/images/folder.gif";
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                oNode.NavigateUrl = "javascript:Edit('" + dr["id"].ToString() + "','" + dr["name"].ToString() + "','" + dr["description"].ToString() + "','" + dr["image"].ToString() + "','" + dr["parent"].ToString() + "','" + oService.GetFolder(Int32.Parse(dr["parent"].ToString()), "name") + "','" + dr["userid"].ToString() + "','" + oUser.GetFullName(Int32.Parse(dr["userid"].ToString())) + "','" + dr["enabled"].ToString() + "');";
                oParent.ChildNodes.Add(oNode);
                LoadServices(Int32.Parse(dr["id"].ToString()), oNode);
                TreeNode oNew = new TreeNode();
                oNew.Text = "&nbsp;Add Folder";
                oNew.ToolTip = "Add Folder";
                oNew.ImageUrl = "/images/green_right.gif";
                oNew.NavigateUrl = "javascript:Add('" + dr["id"].ToString() + "','" + oService.GetFolder(Int32.Parse(dr["id"].ToString()), "name") + "');";
                oNode.ChildNodes.Add(oNew);
            }
            oTreeview.ExpandDepth = 1;
            oTreeview.Attributes.Add("oncontextmenu", "return false;");
        }
        protected void btnAdd_Click(Object Sender, EventArgs e)
        {
            int intParent = Int32.Parse(Request.Form[hdnParent.UniqueID]);
            if (Request.Form[hdnId.UniqueID] == "0")
                oService.AddFolder(txtName.Text, txtDescription.Text, txtImage.Text, intParent, Int32.Parse(Request.Form[hdnUser.UniqueID]), (oService.GetFolders(intParent, 0).Tables[0].Rows.Count + 1), (chkEnabled.Checked ? 1 : 0));
            else
                oService.UpdateFolder(Int32.Parse(Request.Form[hdnId.UniqueID]), txtName.Text, txtDescription.Text, txtImage.Text, intParent, Int32.Parse(Request.Form[hdnUser.UniqueID]), (chkEnabled.Checked ? 1 : 0));
            if (Request.Form[hdnOrder.UniqueID] != "")
            {
                string strOrder = Request.Form[hdnOrder.UniqueID];
                int intCount = 0;
                while (strOrder != "")
                {
                    intCount++;
                    int intId = Int32.Parse(strOrder.Substring(0, strOrder.IndexOf("&")));
                    strOrder = strOrder.Substring(strOrder.IndexOf("&") + 1);
                    oService.UpdateFolderOrder(intId, intCount);
                }
            }
            Response.Redirect(Request.Path);
        }
        protected void btnDelete_Click(Object Sender, EventArgs e)
        {
            oService.DeleteFolder(Int32.Parse(Request.Form[hdnId.UniqueID]));
            Response.Redirect(Request.Path);
        }
    }
}
