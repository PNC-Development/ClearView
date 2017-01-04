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
    public partial class request_items : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected RequestItems oRequestItem;
        protected Applications oApplication;
        protected Users oUser;
        protected Platforms oPlatform;
        protected int intProfile;
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cookies["loginreferrer"].Value = "/admin/request_items.aspx";
            Response.Cookies["loginreferrer"].Expires = DateTime.Now.AddDays(30);
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Response.Redirect("/admin/login.aspx");
            oRequestItem = new RequestItems(intProfile, dsn);
            oApplication = new Applications(intProfile, dsn);
            oUser = new Users(intProfile, dsn);
            oPlatform = new Platforms(intProfile, dsn);
            if (!IsPostBack)
            {
                LoadList();
                LoadApplications(0);
                btnOrder.Attributes.Add("onclick", "return OpenWindow('SUPPORTORDER','" + hdnParent.ClientID + "','" + hdnOrder.ClientID + "&type=ITEMS" + "',false,400,400);");
                btnImage.Attributes.Add("onclick", "return OpenWindow('IMAGEPATH','','" + txtImage.ClientID + "',false,500,550);");
                btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this resource request item?');");
                btnCancel.Attributes.Add("onclick", "return Cancel();");
                btnParent.Attributes.Add("onclick", "return OpenWindow('APPLICATIONBROWSER','" + hdnParent.ClientID + "','&control=" + hdnParent.ClientID + "&controltext=" + lblParent.ClientID + "',false,400,600);");
                btnTabs.Attributes.Add("onclick", "return OpenWindow('WORKLOAD_MGR_TAB','" + hdnId.ClientID + "','',false,400,400);");
                btnTaskTabs.Attributes.Add("onclick", "return OpenWindow('WORKLOAD_MGR_TAB','" + hdnId.ClientID + "','&tab=TT',false,400,400);");
            }
        }
        private void LoadList()
        {
            DataSet ds = oPlatform.GetSystems(1);
            ddlPlatform.DataTextField = "name";
            ddlPlatform.DataValueField = "platformid";
            ddlPlatform.DataSource = ds;
            ddlPlatform.DataBind();
            ddlPlatform.Items.Insert(0, new ListItem("-- NONE --", "0"));
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
                LoadItems(Int32.Parse(dr["applicationid"].ToString()), oNode);
                TreeNode oNew = new TreeNode();
                oNew.Text = "&nbsp;Add Item";
                oNew.ToolTip = "Add Item";
                oNew.ImageUrl = "/images/green_right.gif";
                oNew.NavigateUrl = "javascript:Add('" + dr["applicationid"].ToString() + "','" + oApplication.GetName(Int32.Parse(dr["applicationid"].ToString())) + "');";
                oNode.ChildNodes.Add(oNew);
            }
            oTreeview.ExpandDepth = 1;
            oTreeview.Attributes.Add("oncontextmenu", "return false;");
        }
        private void LoadItems(int _applicationid, TreeNode oParent)
        {
            DataSet ds = oRequestItem.GetItems(_applicationid, 0, 1);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["name"].ToString();
                oNode.ToolTip = dr["name"].ToString();
                oNode.NavigateUrl = "javascript:Edit('" + dr["itemid"].ToString() + "','" + dr["applicationid"].ToString() + "','" + oApplication.GetName(Int32.Parse(dr["applicationid"].ToString())) + "','" + dr["name"].ToString() + "','" + dr["service_title"].ToString() + "','" + dr["image"].ToString() + "','" + dr["platformid"].ToString() + "','" + dr["activity_type"].ToString() + "','" + dr["show"].ToString() + "','" + dr["enabled"].ToString() + "');";
                oParent.ChildNodes.Add(oNode);
            }
        }
        protected void btnAdd_Click(Object Sender, EventArgs e)
        {
            int intApp = Int32.Parse(Request.Form[hdnParent.UniqueID]);
            if (Request.Form[hdnId.UniqueID] == "0")
                oRequestItem.AddItem(intApp, txtName.Text, txtServiceTitle.Text, txtImage.Text, Int32.Parse(ddlPlatform.SelectedItem.Value), (chkActivity.Checked ? 1 : 0), (chkShow.Checked ? 1 : 0), oRequestItem.GetItems(intApp, 0, 1).Tables[0].Rows.Count + 1, (chkEnabled.Checked ? 1 : 0));
            else
                oRequestItem.UpdateItem(Int32.Parse(Request.Form[hdnId.UniqueID]), intApp, txtName.Text, txtServiceTitle.Text, txtImage.Text, Int32.Parse(ddlPlatform.SelectedItem.Value), (chkActivity.Checked ? 1 : 0), (chkShow.Checked ? 1 : 0), (chkEnabled.Checked ? 1 : 0));
            if (Request.Form[hdnOrder.UniqueID] != "")
            {
                string strOrder = Request.Form[hdnOrder.UniqueID];
                int intCount = 0;
                while (strOrder != "")
                {
                    intCount++;
                    int intId = Int32.Parse(strOrder.Substring(0, strOrder.IndexOf("&")));
                    strOrder = strOrder.Substring(strOrder.IndexOf("&") + 1);
                    oRequestItem.UpdateItemOrder(intId, intCount);
                }
            }
            Response.Redirect(Request.Path);
        }
        protected  void btnDelete_Click(Object Sender, EventArgs e)
        {
            oRequestItem.DeleteItem(Int32.Parse(Request.Form[hdnId.UniqueID]));
            Response.Redirect(Request.Path);
        }
    }
}
