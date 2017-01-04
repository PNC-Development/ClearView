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
using System.IO;

namespace NCC.ClearView.Presentation.Web
{
    public partial class asset_models : BasePage
    {
   
    private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
    protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
    protected Models oModel;
    protected Types oType;
    protected Platforms oPlatform;
    protected Host oHost;
    protected Classes oClass;
    protected Solaris oSolaris;
    protected Variables oVariable;
    protected int intProfile;
    protected int intModel = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cookies["loginreferrer"].Value = "/admin/asset/asset_models.aspx";
        Response.Cookies["loginreferrer"].Expires = DateTime.Now.AddDays(30);
        if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
            intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
        else
            Response.Redirect("/admin/login.aspx");
        oModel = new Models(intProfile, dsn);
        oType = new Types(intProfile, dsn);
        oPlatform = new Platforms(intProfile, dsn);
        oHost = new Host(intProfile, dsn);
        oClass = new Classes(intProfile, dsn);
        oSolaris = new Solaris(intProfile, dsn);
        oVariable = new Variables(intEnvironment);
        if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
        {
            panOne.Visible = true;
            intModel = Int32.Parse(Request.QueryString["id"]);
            lblModel.Text = oModel.Get(intModel, "name") + " Reservations";
            LoadClasses();
            treModel.ExpandDepth = 0;
            treModel.Attributes.Add("oncontextmenu", "return false;");
        }
        else
        {
            panAll.Visible = true;
            if (!IsPostBack)
            {
                LoadList();
                Load();
                btnOrder.Attributes.Add("onclick", "return OpenWindow('SUPPORTORDER','" + hdnParent.ClientID + "','" + hdnOrder.ClientID + "&type=A_MODEL" + "',false,400,400);");
                btnParent.Attributes.Add("onclick", "return OpenWindow('TYPEBROWSER','" + hdnParent.ClientID + "','&control=" + hdnParent.ClientID + "&controltext=" + lblParent.ClientID + "',false,400,600);");
                btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this item?');");
                btnCancel.Attributes.Add("onclick", "return Cancel();");
            }
        }
    }
    private void LoadList()
    {
        ddlHost.DataTextField = "name";
        ddlHost.DataValueField = "id";
        ddlHost.DataSource = oHost.Gets(1);
        ddlHost.DataBind();
        ddlHost.Items.Insert(0, new ListItem("-- NONE --", "0"));

        ddlParentModel.DataTextField = "name";
        ddlParentModel.DataValueField = "id";
        ddlParentModel.DataSource = oModel.Gets(1);
        ddlParentModel.DataBind();
        ddlParentModel.Items.Insert(0, new ListItem("-- NONE --", "0"));

        ddlSolarisInterface1.DataTextField = "name";
        ddlSolarisInterface1.DataValueField = "id";
        ddlSolarisInterface1.DataSource = oSolaris.GetInterfaces(1);
        ddlSolarisInterface1.DataBind();
        ddlSolarisInterface1.Items.Insert(0, new ListItem("-- NONE --", "0"));

        ddlSolarisInterface2.DataTextField = "name";
        ddlSolarisInterface2.DataValueField = "id";
        ddlSolarisInterface2.DataSource = oSolaris.GetInterfaces(1);
        ddlSolarisInterface2.DataBind();
        ddlSolarisInterface2.Items.Insert(0, new ListItem("-- NONE --", "0"));

        ddlSolarisBuildType.DataTextField = "name";
        ddlSolarisBuildType.DataValueField = "id";
        ddlSolarisBuildType.DataSource = oSolaris.GetBuildTypes(1);
        ddlSolarisBuildType.DataBind();
        ddlSolarisBuildType.Items.Insert(0, new ListItem("-- NONE --", "0"));

        ddlBootGroup.DataTextField = "name";
        ddlBootGroup.DataValueField = "id";
        ddlBootGroup.DataSource = oModel.GetBootGroups(1);
        ddlBootGroup.DataBind();
        ddlBootGroup.Items.Insert(0, new ListItem("-- NONE --", "0"));
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
            LoadTypes(Int32.Parse(dr["platformid"].ToString()), oNode);
        }
        oTreeview.ExpandDepth = 0;
        oTreeview.Attributes.Add("oncontextmenu", "return false;");
    }
    private void LoadTypes(int _platformid, TreeNode oParent)
    {
        DataSet ds = oType.Gets(_platformid, 0);
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            TreeNode oNode = new TreeNode();
            oNode.Text = dr["name"].ToString();
            oNode.ToolTip = dr["name"].ToString();
            oNode.ImageUrl = "/images/folder.gif";
            oNode.SelectAction = TreeNodeSelectAction.Expand;
            oParent.ChildNodes.Add(oNode);
            LoadModels(Int32.Parse(dr["id"].ToString()), oNode);
            TreeNode oNew = new TreeNode();
            oNew.Text = "&nbsp;Add Model";
            oNew.ToolTip = "Add Model";
            oNew.ImageUrl = "/images/green_right.gif";
            oNew.NavigateUrl = "javascript:Add('" + dr["id"].ToString() + "','" + oType.Get(Int32.Parse(dr["id"].ToString()), "name") + "');";
            oNode.ChildNodes.Add(oNew);
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
            oNode.NavigateUrl = "javascript:Edit('" + dr["id"].ToString() + "','" + dr["name"].ToString() + "','" + dr["make"].ToString() + "','" + dr["typeid"].ToString() + "','" + oType.Get(Int32.Parse(dr["typeid"].ToString()), "name") + "','" + dr["pdf"].ToString().Replace("\\", "\\\\") + "','" + dr["sale"].ToString() + "','" + dr["grouping"].ToString() + "','" + dr["hostid"].ToString() + "','" + dr["ParentModel"].ToString() + "','" + dr["Slots"].ToString() + "','" + dr["Us"].ToString() + "','" + dr["destroy"].ToString() + "','" + dr["solaris_interfaceid1"].ToString() + "','" + dr["solaris_interfaceid2"].ToString() + "','" + dr["solaris_build_typeid"].ToString() + "','" + dr["boot_groupid"].ToString() + "','" + dr["powerdown_prod"].ToString() + "','" + dr["powerdown_test"].ToString() + "','" + dr["factory_code"].ToString() + "','" + dr["factory_code_specific"].ToString() + "','" + dr["enabled"].ToString() + "');";
            oParent.ChildNodes.Add(oNode);
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
            oNode.ImageUrl = "/images/folder.gif";
            oNode.SelectAction = TreeNodeSelectAction.Expand;
            treModel.Nodes.Add(oNode);
            LoadEnvironments(Int32.Parse(dr["id"].ToString()), oNode);
        }
    }
    private void LoadEnvironments(int _parent, TreeNode oParent)
    {
        DataSet ds = oClass.GetEnvironment(_parent, 0);
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            TreeNode oNode = new TreeNode();
            oNode.Text = dr["name"].ToString();
            oNode.ToolTip = dr["name"].ToString();
            oNode.ImageUrl = "/images/folder.gif";
            oNode.SelectAction = TreeNodeSelectAction.Expand;
            oNode.NavigateUrl = "javascript:Open('" + intModel.ToString() + "','" + _parent.ToString() + "','" + dr["id"].ToString() + "');";
            oParent.ChildNodes.Add(oNode);
        }
    }
    protected void btnAdd_Click(Object Sender, EventArgs e)
    {
        int intParent = Int32.Parse(Request.Form[hdnParent.UniqueID]);
        int intID = Int32.Parse(Request.Form[hdnId.UniqueID]);
        string strPDF = oModel.Get(intID, "pdf");
        if (txtPDF.FileName != "" && txtPDF.PostedFile != null)
        {
            string strDirectory = oVariable.DocumentsFolder() + "pdfs";
            if (Directory.Exists(strDirectory) == false)
                Directory.CreateDirectory(strDirectory);
            string strFile = txtPDF.PostedFile.FileName.Trim();
            string strFileName = strFile.Substring(strFile.LastIndexOf("\\") + 1);
            if (File.Exists(strDirectory + "\\" + strFileName) == true)
            {
                int intFileCount = 1;
                string strFileCount = strFileName + ".VERSION_" + intFileCount.ToString();
                while (File.Exists(strDirectory + "\\" + strFileCount) == true)
                {
                    intFileCount++;
                    strFileCount = strFileName + ".VERSION_" + intFileCount.ToString();
                }
                File.Move(strDirectory + "\\" + strFileName, strDirectory + "\\" + strFileCount);
            }
            strPDF = strDirectory + "\\" + strFileName;
            txtPDF.PostedFile.SaveAs(strPDF);
        }
        if (intID == 0)
            oModel.Add(intParent, txtName.Text, "", txtMake.Text, strPDF, (chkSale.Checked ? 1 : 0), Int32.Parse(ddlGrouping.SelectedItem.Value), Int32.Parse(ddlHost.SelectedItem.Value), (chkDestroy.Checked ? 1 : 0), Int32.Parse(ddlParentModel.SelectedItem.Value), (txtSlots.Text.Trim() != "" ? Int32.Parse(txtSlots.Text) : 0), (txtUs.Text.Trim() != "" ? Int32.Parse(txtUs.Text) : 0), Int32.Parse(ddlSolarisInterface1.SelectedItem.Value), Int32.Parse(ddlSolarisInterface2.SelectedItem.Value), Int32.Parse(ddlSolarisBuildType.SelectedItem.Value), Int32.Parse(ddlBootGroup.SelectedItem.Value), Int32.Parse(txtPowerDownProd.Text), Int32.Parse(txtPowerDownTest.Text), txtFactoryCode.Text, txtFactoryCodeSpecific.Text, (oModel.Gets(intParent).Tables[0].Rows.Count + 1), (chkEnabled.Checked ? 1 : 0));
        else
            oModel.Update(intID, intParent, txtName.Text, "", txtMake.Text, strPDF, (chkSale.Checked ? 1 : 0), Int32.Parse(ddlGrouping.SelectedItem.Value), Int32.Parse(ddlHost.SelectedItem.Value), (chkDestroy.Checked ? 1 : 0), Int32.Parse(ddlParentModel.SelectedItem.Value), (txtSlots.Text.Trim() != "" ? Int32.Parse(txtSlots.Text) : 0), (txtUs.Text.Trim() != "" ? Int32.Parse(txtUs.Text) : 0), Int32.Parse(ddlSolarisInterface1.SelectedItem.Value), Int32.Parse(ddlSolarisInterface2.SelectedItem.Value), Int32.Parse(ddlSolarisBuildType.SelectedItem.Value), Int32.Parse(ddlBootGroup.SelectedItem.Value), Int32.Parse(txtPowerDownProd.Text), Int32.Parse(txtPowerDownTest.Text), txtFactoryCode.Text, txtFactoryCodeSpecific.Text, (chkEnabled.Checked ? 1 : 0));
        if (Request.Form[hdnOrder.UniqueID] != "")
        {
            string strOrder = Request.Form[hdnOrder.UniqueID];
            int intCount = 0;
            while (strOrder != "")
            {
                intCount++;
                int intId = Int32.Parse(strOrder.Substring(0, strOrder.IndexOf("&")));
                strOrder = strOrder.Substring(strOrder.IndexOf("&") + 1);
                oModel.UpdateOrder(intId, intCount);
            }
        }
        Response.Redirect(Request.Path);
    }
        protected void btnDelete_Click(Object Sender, EventArgs e)
    {
        oModel.Delete(Int32.Parse(Request.Form[hdnId.UniqueID]));
        Response.Redirect(Request.Path);
    }
        protected void btnReservations_Click(Object Sender, EventArgs e)
    {
        Response.Redirect(Request.Path + "?id=" + Request.Form[hdnId.UniqueID]);
    }

    }
}
