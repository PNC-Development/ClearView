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
    public partial class frame_domain_class_environment : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intProfile;
        protected Domains oDomain;
        protected Classes oClass;
        protected Environments oEnvironment;
        protected int intDomain;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Reload();
            oDomain = new Domains(intProfile, dsn);
            oClass = new Classes(intProfile, dsn);
            oEnvironment = new Environments(intProfile, dsn);
            btnClose.Attributes.Add("onclick", "return HidePanel();");
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                intDomain = Int32.Parse(Request.QueryString["id"]);
            if (!IsPostBack)
            {
                LoadClasses();
                if (intDomain > 0)
                    lblName.Text = oDomain.Get(intDomain, "name");
            }
        }
        private void LoadClasses()
        {
            DataSet ds = oClass.Gets(1);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["name"].ToString();
                oNode.ToolTip = dr["name"].ToString();
                oNode.Value = dr["id"].ToString();
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                oTree.Nodes.Add(oNode);
                LoadEnvironments(oNode, Int32.Parse(dr["id"].ToString()));
            }
        }
        private void LoadEnvironments(TreeNode oParent, int _classid)
        {
            DataSet dsOther = oDomain.GetClassEnvironment(intDomain);
            DataSet ds = oClass.GetEnvironment(_classid, 0);
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
                    if (_classid.ToString() == drOther["classid"].ToString() && dr["id"].ToString() == drOther["environmentid"].ToString())
                        oNode.Checked = true;
                }
                oParent.ChildNodes.Add(oNode);
            }
        }
        protected void btnSave_Click(Object Sender, EventArgs e)
        {
            oDomain.DeleteClassEnvironment(intDomain);
            foreach (TreeNode oNode in oTree.Nodes)
                SaveEnvironment(oNode, Int32.Parse(oNode.Value));
            Reload();
        }
        private void SaveEnvironment(TreeNode oParent, int _classid)
        {
            foreach (TreeNode oNode in oParent.ChildNodes)
            {
                if (oNode.Checked == true)
                    oDomain.AddClassEnvironment(intDomain, _classid, Int32.Parse(oNode.Value));
            }
        }
        private void Reload()
        {
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "reload", "<script type=\"text/javascript\">window.top.location.reload();<" + "/" + "script>");
        }
    }
}
