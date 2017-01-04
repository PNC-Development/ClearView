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
    public partial class folders : BasePage
    {
    private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
    protected VMWare oVMWare;
    protected Environments oEnvironment;
    protected Classes oClass;
    protected Locations oLocation;
    protected int intProfile;
    protected int intLocation = Int32.Parse(ConfigurationManager.AppSettings["OPSLocationID"]);
    protected string strLocation = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cookies["loginreferrer"].Value = "/admin/vmware/folders.aspx";
        Response.Cookies["loginreferrer"].Expires = DateTime.Now.AddDays(30);
        if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
            intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
        else
            Response.Redirect("/admin/login.aspx");
        oVMWare = new VMWare(intProfile, dsn);
        oEnvironment = new Environments(intProfile, dsn);
        oClass = new Classes(intProfile, dsn);
        oLocation = new Locations(intProfile, dsn);
        if (!IsPostBack)
        {
            LoadLists();
            LoadVirtualCenters();
            btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this item?');");
            btnCancel.Attributes.Add("onclick", "return Cancel();");
            btnLocations.Attributes.Add("onclick", "return OpenWindow('VMWARE_FOLDERS','" + hdnId.ClientID + "','',false,'650',500);");
            strLocation = oLocation.LoadDDL("ddlState", "ddlCity", "ddlAddress", hdnLocation.ClientID, intLocation, true, "ddlCommon");
            hdnLocation.Value = intLocation.ToString();
            ddlClass.Attributes.Add("onchange", "PopulateEnvironments('" + ddlClass.ClientID + "','" + ddlEnvironment.ClientID + "',1);");
            ddlEnvironment.Attributes.Add("onchange", "UpdateDropDownHidden('" + ddlEnvironment.ClientID + "','" + hdnEnvironment.ClientID + "');");
        }
    }
    private void LoadLists()
    {
        ddlClass.DataTextField = "name";
        ddlClass.DataValueField = "id";
        ddlClass.DataSource = oClass.Gets(1);
        ddlClass.DataBind();
        ddlClass.Items.Insert(0, new ListItem("-- SELECT --", "0"));
    }
    private void LoadVirtualCenters()
    {
        DataSet ds = oVMWare.GetVirtualCenters(1);
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            TreeNode oNode = new TreeNode();
            oNode.Text = dr["name"].ToString();
            oNode.ToolTip = dr["name"].ToString();
            oNode.ImageUrl = "/images/folder.gif";
            oNode.SelectAction = TreeNodeSelectAction.Expand;
            oTreeview.Nodes.Add(oNode);
            LoadDatacenter(Int32.Parse(dr["id"].ToString()), oNode);
        }
        oTreeview.ExpandDepth = 2;
        oTreeview.Attributes.Add("oncontextmenu", "return false;");
    }
    private void LoadDatacenter(int _parent, TreeNode oParent)
    {
        DataSet ds = oVMWare.GetDatacenters(_parent, 1);
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            ddlParent.Items.Insert(0, new ListItem(dr["name"].ToString(), dr["id"].ToString()));
            TreeNode oNode = new TreeNode();
            oNode.Text = dr["name"].ToString();
            oNode.ToolTip = dr["name"].ToString();
            oNode.ImageUrl = "/images/folder.gif";
            oNode.SelectAction = TreeNodeSelectAction.Expand;
            oParent.ChildNodes.Add(oNode);
            LoadFolder(Int32.Parse(dr["id"].ToString()), oNode);
            TreeNode oNew = new TreeNode();
            oNew.Text = "&nbsp;Add Folder";
            oNew.ToolTip = "Add Folder";
            oNew.ImageUrl = "/images/green_right.gif";
            oNew.NavigateUrl = "javascript:Add('" + dr["id"].ToString() + "');";
            oNode.ChildNodes.Add(oNew);
        }
    }
    private void LoadFolder(int _parent, TreeNode oParent)
    {
        DataSet ds = oVMWare.GetFolders(_parent, 0);
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            TreeNode oNode = new TreeNode();
            oNode.Text = dr["name"].ToString();
            oNode.ToolTip = dr["name"].ToString();
            oNode.SelectAction = TreeNodeSelectAction.Expand;
            int intAddress = 0;
            int intCity = 0;
            int intState = 0;
            if (Int32.TryParse(dr["addressid"].ToString(), out intAddress) == true)
                if (Int32.TryParse(oLocation.GetAddress(intAddress, "cityid"), out intCity) == true)
                    Int32.TryParse(oLocation.GetCity(intCity, "stateid"), out intState);
            oNode.NavigateUrl = "javascript:Edit('" + dr["id"].ToString() + "','" + dr["datacenterid"].ToString() + "','" + dr["name"].ToString() + "','" + dr["notification"].ToString() + "','" + dr["classid"].ToString() + "','" + dr["environmentid"].ToString() + "','" + intState.ToString() + "','" + intCity.ToString() + "','" + intAddress.ToString() + "','" + dr["enabled"].ToString() + "');";
            oParent.ChildNodes.Add(oNode);
        }
    }
    protected void btnAdd_Click(Object Sender, EventArgs e)
    {
        if (Request.Form[hdnId.UniqueID] == "0")
            oVMWare.AddFolder(Int32.Parse(ddlParent.SelectedItem.Value), txtName.Text, txtNotification.Text, Int32.Parse(ddlClass.SelectedItem.Value), Int32.Parse(Request.Form[hdnEnvironment.UniqueID]), Int32.Parse(Request.Form[hdnLocation.UniqueID]), (chkEnabled.Checked ? 1 : 0));
        else
            oVMWare.UpdateFolder(Int32.Parse(Request.Form[hdnId.UniqueID]), Int32.Parse(ddlParent.SelectedItem.Value), txtName.Text, txtNotification.Text, Int32.Parse(ddlClass.SelectedItem.Value), Int32.Parse(Request.Form[hdnEnvironment.UniqueID]), Int32.Parse(Request.Form[hdnLocation.UniqueID]), (chkEnabled.Checked ? 1 : 0));
        Response.Redirect(Request.Path);
    }
    protected void btnDelete_Click(Object Sender, EventArgs e)
    {
        oVMWare.DeleteFolder(Int32.Parse(Request.Form[hdnId.UniqueID]));
        Response.Redirect(Request.Path);
    }
    }
}
