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
    public partial class frame_server_components_os : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intProfile;
        protected ServerName oServerName;
        protected OperatingSystems oOperatingSystems;
        protected int intComponent;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Reload();
            oServerName = new ServerName(intProfile, dsn);
            oOperatingSystems = new OperatingSystems(intProfile, dsn);
            btnClose.Attributes.Add("onclick", "return HidePanel();");
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                intComponent = Int32.Parse(Request.QueryString["id"]);
            if (!IsPostBack)
            {
                Load(null);
                if (intComponent > 0)
                    lblName.Text = oServerName.GetComponent(intComponent, "name");
            }
        }
        private void Load(TreeNode oParent)
        {
            DataSet dsOther = oServerName.GetComponentPermissions(intComponent);
            DataSet ds = oOperatingSystems.Gets(0, 1);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["name"].ToString();
                oNode.ToolTip = dr["name"].ToString();
                oNode.Value = dr["id"].ToString();
                oNode.SelectAction = TreeNodeSelectAction.None;
                oNode.Checked = false;
                foreach (DataRow drOther in dsOther.Tables[0].Rows)
                {
                    if (dr["id"].ToString() == drOther["id"].ToString())
                        oNode.Checked = true;
                }
                if (oParent != null)
                    oParent.ChildNodes.Add(oNode);
                else
                    oTree.Nodes.Add(oNode);
            }
        }
        protected  void btnSave_Click(Object Sender, EventArgs e)
        {
            oServerName.DeleteComponentPermission(intComponent);
            foreach (TreeNode oNode in oTree.Nodes)
            {
                if (oNode.Checked == true)
                    oServerName.AddComponentPermission(intComponent, Int32.Parse(oNode.Value));
            }
            Reload();
        }
        private void Reload()
        {
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "reload", "<script type=\"text/javascript\">window.top.location.reload();<" + "/" + "script>");
        }
    }
}
