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
    public partial class workstation_components_scripts : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected Workstations oWorkstation;
        protected int intProfile;
        protected int intID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cookies["loginreferrer"].Value = "/admin/workstation_components_scripts.aspx";
            Response.Cookies["loginreferrer"].Expires = DateTime.Now.AddDays(30);
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Response.Redirect("/admin/login.aspx");
            oWorkstation = new Workstations(intProfile, dsn);
            if (Request.QueryString["id"] == null)
            {
                if (Request.QueryString["parent"] == null)
                    LoadComponents();
                else
                {
                    panAdd.Visible = true;
                    int intParent = Int32.Parse(Request.QueryString["parent"]);
                    hdnParent.Value = intParent.ToString();
                    lblParent.Text = oWorkstation.GetComponent(intParent, "name");
                }
            }
            else
            {
                panAdd.Visible = true;
                intID = Int32.Parse(Request.QueryString["id"]);
                if (!IsPostBack)
                {
                    DataSet ds = oWorkstation.GetComponentScript(intID);
                    int intParent = Int32.Parse(ds.Tables[0].Rows[0]["componentid"].ToString());
                    hdnParent.Value = intParent.ToString();
                    lblParent.Text = oWorkstation.GetComponent(intParent, "name");
                    txtName.Text = ds.Tables[0].Rows[0]["name"].ToString();
                    txtScript.Text = ds.Tables[0].Rows[0]["script"].ToString();
                    chkEnabled.Checked = (ds.Tables[0].Rows[0]["enabled"].ToString() == "1");
                }
            }
            btnOrder.Attributes.Add("onclick", "return OpenWindow('SUPPORTORDER','" + hdnParent.ClientID + "','" + hdnOrder.ClientID + "&type=COMPONENT_SCRIPTS" + "',false,400,400);");
            btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this item?');");
        }
        private void LoadComponents()
        {
            panView.Visible = true;
            DataSet ds = oWorkstation.GetComponents(1);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["name"].ToString();
                oNode.ToolTip = dr["name"].ToString();
                oNode.ImageUrl = "/images/folder.gif";
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                oTreeview.Nodes.Add(oNode);
                Load(Int32.Parse(dr["id"].ToString()), oNode);
                TreeNode oNew = new TreeNode();
                oNew.Text = "&nbsp;Add Script";
                oNew.ToolTip = "Add Script";
                oNew.ImageUrl = "/images/green_right.gif";
                oNew.NavigateUrl = Request.Path + "?parent=" + dr["id"].ToString();
                oNode.ChildNodes.Add(oNew);
            }
            oTreeview.ExpandDepth = 0;
            oTreeview.Attributes.Add("oncontextmenu", "return false;");
        }
        private void Load(int _componentid, TreeNode oParent)
        {
            DataSet ds = oWorkstation.GetComponentScripts(_componentid, 0);
            int intCount = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                intCount++;
                TreeNode oNode = new TreeNode();
                oNode.Text = intCount.ToString() + ") " + dr["name"].ToString();
                oNode.ToolTip = intCount.ToString() + ") " + dr["name"].ToString();
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                oNode.NavigateUrl = Request.Path + "?id=" + dr["id"].ToString();
                oParent.ChildNodes.Add(oNode);
            }
        }
        protected void btnAdd_Click(Object Sender, EventArgs e)
        {
            int intComponent = Int32.Parse(Request.Form[hdnParent.UniqueID]);
            if (intID == 0)
                oWorkstation.AddComponentScript(intComponent, txtName.Text, txtScript.Text, oWorkstation.GetComponentScripts(intComponent, 0).Tables[0].Rows.Count + 1, (chkEnabled.Checked ? 1 : 0));
            else
                oWorkstation.UpdateComponentScript(intID, intComponent, txtName.Text, txtScript.Text, (chkEnabled.Checked ? 1 : 0));
            if (Request.Form[hdnOrder.UniqueID] != "")
            {
                string strOrder = Request.Form[hdnOrder.UniqueID];
                int intCount = 0;
                while (strOrder != "")
                {
                    intCount++;
                    int intId = Int32.Parse(strOrder.Substring(0, strOrder.IndexOf("&")));
                    strOrder = strOrder.Substring(strOrder.IndexOf("&") + 1);
                    oWorkstation.UpdateComponentScriptOrder(intId, intCount);
                }
            }
            Response.Redirect(Request.Path);
        }
        protected void btnCancel_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path);
        }
        protected void btnDelete_Click(Object Sender, EventArgs e)
        {
            oWorkstation.DeleteComponentScript(intID);
            Response.Redirect(Request.Path);
        }
    }
}
