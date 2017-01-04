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
    public partial class asset_models_network : BasePage
    {

    private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
    protected int intPlatform = Int32.Parse(ConfigurationManager.AppSettings["NetworkPlatformID"]);
    protected Types oType;
    protected Models oModel;
    protected ModelsProperties oModelsProperties;
    protected int intProfile;
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cookies["loginreferrer"].Value = "/admin/asset/asset_models_network.aspx";
        Response.Cookies["loginreferrer"].Expires = DateTime.Now.AddDays(30);
        if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
            intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
        else
            Response.Redirect("/admin/login.aspx");
        oType = new Types(intProfile, dsn);
        oModel = new Models(intProfile, dsn);
        oModelsProperties = new ModelsProperties(intProfile, dsn);
        if (!IsPostBack)
        {
            LoadTypes(intPlatform);
            btnParent.Attributes.Add("onclick", "return OpenWindow('MODELBROWSER','" + hdnParent.ClientID + "','&control=" + hdnParent.ClientID + "&controltext=" + lblParent.ClientID + "',false,400,600);");
            btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this item?');");
            btnCancel.Attributes.Add("onclick", "return Cancel();");
        }
    }
    private void LoadTypes(int _platformid)
    {
        DataSet ds = oType.Gets(_platformid, 0);
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            TreeNode oNode = new TreeNode();
            oNode.Text = dr["name"].ToString();
            oNode.ToolTip = dr["name"].ToString();
            oNode.ImageUrl = "/images/folder.gif";
            oNode.SelectAction = TreeNodeSelectAction.Expand;
            oTreeview.Nodes.Add(oNode);
            LoadModels(Int32.Parse(dr["id"].ToString()), oNode);
        }
    }
    private void LoadModels(int _typeid, TreeNode oParent)
    {
        DataSet ds = oModel.Gets(_typeid, 0);
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            TreeNode oNode = new TreeNode();
            oNode.Text = dr["name"].ToString();
            oNode.ToolTip = dr["name"].ToString();
            oNode.ImageUrl = "/images/folder.gif";
            oNode.SelectAction = TreeNodeSelectAction.Expand;
            oParent.ChildNodes.Add(oNode);
            LoadModelsNetwork(Int32.Parse(dr["id"].ToString()), oNode);
            TreeNode oNew = new TreeNode();
            oNew.Text = "&nbsp;Add Model";
            oNew.ToolTip = "Add Model";
            oNew.ImageUrl = "/images/green_right.gif";
            oNew.NavigateUrl = "javascript:Add('" + dr["id"].ToString() + "','" + oModel.Get(Int32.Parse(dr["id"].ToString()), "name") + "');";
            oNode.ChildNodes.Add(oNew);
        }
    }
    private void LoadModelsNetwork(int _modelid, TreeNode oParent)
    {
        DataSet ds = oModelsProperties.GetModels(0, _modelid, 0);
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            TreeNode oNode = new TreeNode();
            string strName = dr["name"].ToString();
            if (strName == "")
                strName = oModel.Get(_modelid, "name");
            oNode.Text = strName;
            oNode.ToolTip = strName;
            oNode.ImageUrl = "/images/folder.gif";
            oNode.SelectAction = TreeNodeSelectAction.Expand;
            oNode.NavigateUrl = "javascript:Edit('" + dr["id"].ToString() + "','" + dr["available"].ToString() + "','" + dr["replicate_times"].ToString() + "','" + dr["amp"].ToString() + "','" + dr["modelid"].ToString() + "','" + oModel.Get(Int32.Parse(dr["modelid"].ToString()), "name") + "','" + dr["network_ports"].ToString() + "','" + dr["enabled"].ToString() + "');";
            oParent.ChildNodes.Add(oNode);
        }
        oTreeview.ExpandDepth = 0;
        oTreeview.Attributes.Add("oncontextmenu", "return false;");
    }
    protected void btnAdd_Click(Object Sender, EventArgs e)
    {
        int intParent = Int32.Parse(Request.Form[hdnParent.UniqueID]);
        if (Request.Form[hdnId.UniqueID] == "0")
            oModelsProperties.AddNetwork(intParent, (chkAvailable.Checked ? 1 : 0), Int32.Parse(txtReplicate.Text), double.Parse(txtAmp.Text), Int32.Parse(txtPorts.Text), (chkEnabled.Checked ? 1 : 0));
        else
            oModelsProperties.UpdateNetwork(Int32.Parse(Request.Form[hdnId.UniqueID]), intParent, (chkAvailable.Checked ? 1 : 0), Int32.Parse(txtReplicate.Text), double.Parse(txtAmp.Text), Int32.Parse(txtPorts.Text), (chkEnabled.Checked ? 1 : 0));
        Response.Redirect(Request.Path);
    }
    protected void btnDelete_Click(Object Sender, EventArgs e)
    {
        oModelsProperties.Delete(Int32.Parse(Request.Form[hdnId.UniqueID]));
        Response.Redirect(Request.Path);
    }

    }
}
