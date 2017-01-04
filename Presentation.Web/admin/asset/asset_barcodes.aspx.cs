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
    public partial class asset_barcodes : BasePage
    {

    private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
    protected Platforms oPlatform;
    protected Types oType;
    protected Models oModel;
    protected ModelsProperties oModelsProperties;
    protected int intProfile;
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cookies["loginreferrer"].Value = "/admin/asset/asset_barcodes.aspx";
        Response.Cookies["loginreferrer"].Expires = DateTime.Now.AddDays(30);
        if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
            intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
        else
            Response.Redirect("/admin/login.aspx");
        oPlatform = new Platforms(intProfile, dsn);
        oType = new Types(intProfile, dsn);
        oModel = new Models(intProfile, dsn);
        oModelsProperties = new ModelsProperties(intProfile, dsn);
        LoadPlatforms();
        oTreeview.ExpandDepth = 0;
        oTreeview.Attributes.Add("oncontextmenu", "return false;");
    }
    private void LoadPlatforms()
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
            LoadTypes(Int32.Parse(dr["platformid"].ToString()), oNode);
        }
    }
    private void LoadTypes(int _platformid, TreeNode oParent)
    {
        DataSet ds = oType.Gets(_platformid, 1);
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            TreeNode oNode = new TreeNode();
            oNode.Text = dr["name"].ToString();
            oNode.ToolTip = dr["name"].ToString();
            oNode.SelectAction = TreeNodeSelectAction.Expand;
            oNode.NavigateUrl = "/admin/asset/asset_barcodes_view.aspx?id=" + dr["id"].ToString();
            oNode.Target = "_blank";
            oParent.ChildNodes.Add(oNode);
        }
    }

    }
}
