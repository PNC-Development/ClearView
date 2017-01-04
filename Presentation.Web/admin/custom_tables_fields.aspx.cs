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
    public partial class custom_tables_fields : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected Field oField;
        protected int intProfile;
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cookies["loginreferrer"].Value = "/admin/custom_tables_fields.aspx";
            Response.Cookies["loginreferrer"].Expires = DateTime.Now.AddDays(30);
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Response.Redirect("/admin/login.aspx");
            oField = new Field(intProfile, dsn);
            if (!IsPostBack)
            {
                Load();
                btnOrder.Attributes.Add("onclick", "return OpenWindow('SUPPORTORDER','" + hdnParent.ClientID + "','" + hdnOrder.ClientID + "&type=FIELD" + "',false,400,400);");
                btnParent.Attributes.Add("onclick", "return OpenWindow('TABLEBROWSER','" + hdnParent.ClientID + "','&control=" + hdnParent.ClientID + "&controltext=" + lblParent.ClientID + "',false,400,600);");
                btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this item?');");
                btnCancel.Attributes.Add("onclick", "return Cancel();");
                txtFieldName.Attributes.Add("onkeypress", "return DatabaseName();");
            }
        }
        private void Load()
        {
            DataSet ds = oField.GetTables(1);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["tablename"].ToString();
                oNode.ToolTip = dr["tablename"].ToString();
                oNode.ImageUrl = "/images/folder.gif";
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                oTreeview.Nodes.Add(oNode);
                LoadFields(Int32.Parse(dr["id"].ToString()), oNode);
                TreeNode oNew = new TreeNode();
                oNew.Text = "&nbsp;Add Field";
                oNew.ToolTip = "Add Field";
                oNew.ImageUrl = "/images/green_right.gif";
                oNew.NavigateUrl = "javascript:Add('" + dr["id"].ToString() + "','" + oField.GetTable(Int32.Parse(dr["id"].ToString()), "tablename") + "');";
                oNode.ChildNodes.Add(oNew);
            }
        }
        private void LoadFields(int _tableid, TreeNode oParent)
        {
            DataSet ds = oField.Gets(_tableid, 0);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["fieldname"].ToString();
                oNode.ToolTip = dr["fieldname"].ToString();
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                oNode.NavigateUrl = "javascript:Edit('" + dr["id"].ToString() + "','" + dr["fieldname"].ToString() + "','" + dr["name"].ToString() + "','" + dr["datatype"].ToString() + "','" + dr["join_table"].ToString() + "','" + dr["join_on"].ToString() + "','" + dr["join_field"].ToString() + "','" + dr["tableid"].ToString() + "','" + oField.GetTable(Int32.Parse(dr["tableid"].ToString()), "tablename") + "','" + dr["hidden"].ToString() + "','" + dr["enabled"].ToString() + "');";
                oParent.ChildNodes.Add(oNode);
            }
            oTreeview.ExpandDepth = 0;
            oTreeview.Attributes.Add("oncontextmenu", "return false;");
        }
        protected void btnAdd_Click(Object Sender, EventArgs e)
        {
            int intParent = Int32.Parse(Request.Form[hdnParent.UniqueID]);
            if (Request.Form[hdnId.UniqueID] == "0")
                oField.Add(intParent, txtFieldName.Text, txtName.Text, ddlType.SelectedItem.Value, txtJoinTable.Text, txtJoinOn.Text, txtJoinField.Text, (chkHidden.Checked ? 1 : 0), (oField.Gets(intParent, 1).Tables[0].Rows.Count + 1), (chkEnabled.Checked ? 1 : 0));
            else
                oField.Update(Int32.Parse(Request.Form[hdnId.UniqueID]), intParent, txtFieldName.Text, txtName.Text, ddlType.SelectedItem.Value, txtJoinTable.Text, txtJoinOn.Text, txtJoinField.Text, (chkHidden.Checked ? 1 : 0), (chkEnabled.Checked ? 1 : 0));
            if (Request.Form[hdnOrder.UniqueID] != "")
            {
                string strOrder = Request.Form[hdnOrder.UniqueID];
                int intCount = 0;
                while (strOrder != "")
                {
                    intCount++;
                    int intId = Int32.Parse(strOrder.Substring(0, strOrder.IndexOf("&")));
                    strOrder = strOrder.Substring(strOrder.IndexOf("&") + 1);
                    oField.UpdateOrder(intId, intCount);
                }
            }
            Response.Redirect(Request.Path);
        }
        protected void btnDelete_Click(Object Sender, EventArgs e)
        {
            oField.Delete(Int32.Parse(Request.Form[hdnId.UniqueID]));
            Response.Redirect(Request.Path);
        }
    }
}
