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
    public partial class frame_service_locations : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intProfile;
        protected Services oService;
        protected int intService;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
            {
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
                oService = new Services(intProfile, dsn);
                btnClose.Attributes.Add("onclick", "return HidePanel();");
                if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                    intService = Int32.Parse(Request.QueryString["id"]);
                if (!IsPostBack)
                {
                    LoadLocations();
                    if (intService > 0)
                        lblName.Text = oService.GetName(intService);
                }
            }
        }
        private void LoadLocations()
        {
            TreeNode oNode = new TreeNode();
            oNode.Text = "ClearView Services";
            oNode.Value = "0";
            oNode.ToolTip = "ClearView Services";
            oNode.SelectAction = TreeNodeSelectAction.None;
            oTree.Nodes.Add(oNode);
            LoadLocations(0, oNode);
            oTree.CollapseAll();
            oNode.Expand();
            oService.ExpandLocations(oNode, intService, 0);
            oNode.ShowCheckBox = false;
        }
        private void LoadLocations(int _parent, TreeNode oParent)
        {
            DataSet ds = oService.GetFolders(_parent, 0);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["name"].ToString();
                oNode.Value = dr["id"].ToString();
                oNode.ToolTip = dr["name"].ToString();
                oNode.SelectAction = TreeNodeSelectAction.None;
                oParent.ChildNodes.Add(oNode);
                LoadLocations(Int32.Parse(dr["id"].ToString()), oNode);
            }
        }
        protected void btnSave_Click(Object Sender, EventArgs e)
        {
            oService.DeleteFolders(intService);
            foreach (TreeNode oNode in oTree.Nodes)
                SaveLocation(oNode);
            Response.Redirect(Request.Url.PathAndQuery);
        }
        private void SaveLocation(TreeNode oParent)
        {
            if (oParent.Checked == true)
                oService.AddFolders(intService, Int32.Parse(oParent.Value), 0);
            foreach (TreeNode oNode in oParent.ChildNodes)
                SaveLocation(oNode);
        }
    }
}
