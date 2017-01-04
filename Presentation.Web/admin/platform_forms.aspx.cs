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
    public partial class platform_forms : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected Platforms oPlatform;
        protected int intProfile;
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cookies["loginreferrer"].Value = "/admin/platform_forms.aspx";
            Response.Cookies["loginreferrer"].Expires = DateTime.Now.AddDays(30);
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Response.Redirect("/admin/login.aspx");
            oPlatform = new Platforms(intProfile, dsn);
            if (!IsPostBack)
            {
                Load();
                btnOrder.Attributes.Add("onclick", "return OpenWindow('SUPPORTORDER','" + hdnParent.ClientID + "','" + hdnOrder.ClientID + "&type=PLATFORM_FORM" + "',false,400,400);");
                btnParent.Attributes.Add("onclick", "return OpenWindow('PLATFORMBROWSER','" + hdnParent.ClientID + "','&control=" + hdnParent.ClientID + "&controltext=" + lblParent.ClientID + "',false,400,600);");
                btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this item?');");
                btnImage.Attributes.Add("onclick", "return OpenWindow('IMAGEPATH','','" + txtImage.ClientID + "',false,500,550);");
                btnPath.Attributes.Add("onclick", "return OpenWindow('FILEBROWSER','" + txtPath.ClientID + "','',false,400,600);");
                btnCancel.Attributes.Add("onclick", "return Cancel();");
            }
        }
        private void Load()
        {
            DataSet ds = oPlatform.Gets(1);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["name"].ToString();
                oNode.ToolTip = dr["name"].ToString();
                oNode.ImageUrl = "/images/folder.gif";
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                oTreeview.Nodes.Add(oNode);
                Load(Int32.Parse(dr["platformid"].ToString()), oNode);
                TreeNode oNew = new TreeNode();
                oNew.Text = "&nbsp;Add Form";
                oNew.ToolTip = "Add Form";
                oNew.ImageUrl = "/images/green_right.gif";
                oNew.NavigateUrl = "javascript:Add('" + dr["platformid"].ToString() + "','" + oPlatform.GetName(Int32.Parse(dr["platformid"].ToString())) + "');";
                oNode.ChildNodes.Add(oNew);
            }
        }
        private void Load(int _parent, TreeNode oParent)
        {
            DataSet ds = oPlatform.GetForms(_parent, 0);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["name"].ToString();
                oNode.ToolTip = dr["name"].ToString();
                oNode.ImageUrl = "/images/folder.gif";
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                oNode.NavigateUrl = "javascript:Edit('" + dr["id"].ToString() + "','" + oPlatform.GetName(Int32.Parse(dr["platformid"].ToString())) + "','" + _parent.ToString() + "','" + dr["name"].ToString() + "','" + dr["path"].ToString() + "','" + dr["max1"].ToString() + "','" + dr["max2"].ToString() + "','" + dr["max3"].ToString() + "','" + dr["max4"].ToString() + "','" + dr["max5"].ToString() + "','" + dr["enabled"].ToString() + "');";
                oParent.ChildNodes.Add(oNode);
            }
            oTreeview.ExpandDepth = 1;
            oTreeview.Attributes.Add("oncontextmenu", "return false;");
        }
        protected void btnAdd_Click(Object Sender, EventArgs e)
        {
            int intParent = Int32.Parse(Request.Form[hdnParent.UniqueID]);
            if (Request.Form[hdnId.UniqueID] == "0")
                oPlatform.AddForm(intParent, txtName.Text, txtDescription.Text, txtImage.Text, txtPath.Text, Int32.Parse(txtMax1.Text), Int32.Parse(txtMax2.Text), Int32.Parse(txtMax3.Text), Int32.Parse(txtMax4.Text), Int32.Parse(txtMax5.Text), (oPlatform.GetForms(intParent, 0).Tables[0].Rows.Count + 1), (chkEnabled.Checked ? 1 : 0));
            else
                oPlatform.UpdateForm(Int32.Parse(Request.Form[hdnId.UniqueID]), intParent, txtName.Text, txtDescription.Text, txtImage.Text, txtPath.Text, Int32.Parse(txtMax1.Text), Int32.Parse(txtMax2.Text), Int32.Parse(txtMax3.Text), Int32.Parse(txtMax4.Text), Int32.Parse(txtMax5.Text), (chkEnabled.Checked ? 1 : 0));
            if (Request.Form[hdnOrder.UniqueID] != "")
            {
                string strOrder = Request.Form[hdnOrder.UniqueID];
                int intCount = 0;
                while (strOrder != "")
                {
                    intCount++;
                    int intId = Int32.Parse(strOrder.Substring(0, strOrder.IndexOf("&")));
                    strOrder = strOrder.Substring(strOrder.IndexOf("&") + 1);
                    oPlatform.UpdateFormOrder(intId, intCount);
                }
            }
            Response.Redirect(Request.Path);
        }
        protected void btnDelete_Click(Object Sender, EventArgs e)
        {
            oPlatform.DeleteForm(Int32.Parse(Request.Form[hdnId.UniqueID]));
            Response.Redirect(Request.Path);
        }
    }
}
