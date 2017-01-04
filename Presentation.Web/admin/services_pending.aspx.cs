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
    public partial class services_pending : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected Services oService;
        protected ServiceEditor oServiceEditor;
        protected RequestItems oRequestItem;
        protected Applications oApplication;
        protected Users oUser;
        protected int intProfile;
        protected int intService;
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cookies["loginreferrer"].Value = "/admin/services_pending.aspx";
            Response.Cookies["loginreferrer"].Expires = DateTime.Now.AddDays(30);
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Response.Redirect("/admin/login.aspx");
            oService = new Services(intProfile, dsn);
            oServiceEditor = new ServiceEditor(intProfile, dsn);
            oRequestItem = new RequestItems(intProfile, dsn);
            oApplication = new Applications(intProfile, dsn);
            oUser = new Users(intProfile, dsn);
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
            {
                intService = Int32.Parse(Request.QueryString["id"]);
                panView.Visible = true;
                if (!IsPostBack)
                {
                    LoadServices(0);
                    lblName.Text = oService.GetName(intService);
                    int intItem = oService.GetItemId(intService);
                    lblItem.Text = oRequestItem.GetItemName(intItem);
                    lblApplication.Text = oApplication.GetName(oRequestItem.GetItemApplication(intItem));
                    DataSet ds = oService.GetUser(intService, -1);
                    if (ds.Tables[0].Rows.Count > 0)
                        lblUser.Text = oUser.GetFullName(Int32.Parse(ds.Tables[0].Rows[0]["userid"].ToString()));
                }
            }
            else
            {
                panAll.Visible = true;
                rptView.DataSource = oService.GetPending();
                rptView.DataBind();
            }
            btnSave.Attributes.Add("onclick", "return confirm('Are you sure you want to save this service?');");
            btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this service?');");
        }
        private void LoadServices(int _parent)
        {
            TreeNode oNode = new TreeNode();
            oNode.Text = "ClearView Services";
            oNode.Value = "0";
            oNode.ToolTip = "ClearView Services";
            oNode.SelectAction = TreeNodeSelectAction.None;
            oTreeview.Nodes.Add(oNode);
            LoadServices(0, oNode);
            oTreeview.ExpandDepth = 1;
            oTreeview.Attributes.Add("oncontextmenu", "return false;");
        }
        private void LoadServices(int _parent, TreeNode oParent)
        {
            DataSet ds = oService.GetFolders(_parent, 1);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["name"].ToString();
                oNode.Value = dr["id"].ToString();
                oNode.ToolTip = dr["name"].ToString();
                oNode.SelectAction = TreeNodeSelectAction.None;
                oParent.ChildNodes.Add(oNode);
                LoadServices(Int32.Parse(dr["id"].ToString()), oNode);
            }
        }
        protected void btnSave_Click(Object Sender, EventArgs e)
        {
            foreach (TreeNode oNode in oTreeview.Nodes)
                SaveLocation(oNode);
            oService.UpdateStep(intService, 0);
            Response.Redirect(Request.Path);
        }
        private void SaveLocation(TreeNode oParent)
        {
            if (oParent.Checked == true)
                oService.AddFolders(intService, Int32.Parse(oParent.Value), 0);
            foreach (TreeNode oNode in oParent.ChildNodes)
                SaveLocation(oNode);
        }
        protected void btnDelete_Click(Object Sender, EventArgs e)
        {
            oService.Delete(intService);
            Response.Redirect(Request.Path);
        }
        protected void btnCancel_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path);
        }
    }
}
