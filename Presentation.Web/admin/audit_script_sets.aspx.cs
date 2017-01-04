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
using NCC.ClearView.Presentation.Web.Custom;

namespace NCC.ClearView.Presentation.Web
{
    public partial class audit_script_sets : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intPlatform = Int32.Parse(ConfigurationManager.AppSettings["ServerPlatformID"]);
        protected Audit oAudit;
        protected Classes oClass;
        protected Environments oEnvironment;
        protected Types oType;
        protected Models oModel;
        protected ModelsProperties oModelsProperties;
        protected OperatingSystems oOperatingSystem;
        protected ServicePacks oServicePack;
        protected Locations oLocation;
        protected int intProfile;
        protected int intID = 0;
        protected string strModels = "";
        protected string strDisabled = "";
        protected string strMenuTab1 = "";
        protected string strLocation = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cookies["loginreferrer"].Value = "/admin/audit_script_sets.aspx";
            Response.Cookies["loginreferrer"].Expires = DateTime.Now.AddDays(30);
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Response.Redirect("/admin/login.aspx");

            oAudit = new Audit(intProfile, dsn);
            oClass = new Classes(intProfile, dsn);
            oEnvironment = new Environments(intProfile, dsn);
            oType = new Types(intProfile, dsn);
            oModel = new Models(intProfile, dsn);
            oModelsProperties = new ModelsProperties(intProfile, dsn);
            oOperatingSystem = new OperatingSystems(intProfile, dsn);
            oServicePack = new ServicePacks(intProfile, dsn);
            oLocation = new Locations(intProfile, dsn);

            if (!IsPostBack)
                LoadLists();

            if (Request.QueryString["id"] == null)
                LoopRepeater();
            else
            {
                intID = Int32.Parse(Request.QueryString["id"]);
                if (intID == 0)
                {
                    panAdd.Visible = true;
                    btnDelete.Enabled = false;
                    strDisabled = "<p><img src=\"/images/bigAlert.gif\" border=\"0\" align=\"absmiddle\"/> <b>This section is only available when updating an existing item</b></p>";
                }
                else
                {
                    if (Request.QueryString["save"] != null)
                        panSave.Visible = true;
                    panAdd.Visible = true;
                    if (!IsPostBack)
                    {
                        DataSet ds = oAudit.GetScriptSet(intID);
                        txtName.Text = ds.Tables[0].Rows[0]["name"].ToString();
                        chkModels.Checked = (ds.Tables[0].Rows[0]["models"].ToString() == "1");
                        chkSAN.Checked = (ds.Tables[0].Rows[0]["san"].ToString() == "1");
                        chkCluster.Checked = (ds.Tables[0].Rows[0]["cluster"].ToString() == "1");
                        chkMIS.Checked = (ds.Tables[0].Rows[0]["mis"].ToString() == "1");
                        chkEnabled.Checked = (ds.Tables[0].Rows[0]["enabled"].ToString() == "1");
                        btnAdd.Text = "Update";

                        LoadClasses(oTreeClassEnvironment);
                        oTreeClassEnvironment.Attributes.Add("onclick", "OnCheckBoxCheckChanged(event)");
                        if (chkModels.Checked == false)
                            strModels = "<p><img src=\"/images/bigAlert.gif\" border=\"0\" align=\"absmiddle\"/> <b>This section is only available if the <i>SPECIFY INDIVIDUAL MODEL(S)</i> flag is checked</b></p>";
                        else
                        {
                            LoadTypes(oTreeModels);
                            oTreeModels.Attributes.Add("onclick", "OnCheckBoxCheckChanged(event)");
                        }
                        LoadOperatingSystems(oTreeOperatingSystemServicePack);
                        LoadLocations();
                        panSteps.Visible = true;
                        frmSteps.Attributes.Add("src", "/admin/audit_script_set_details.aspx?parent=" + intID.ToString());
                    }
                }
            }

            if (panAdd.Visible == true)
            {
                int intMenuTab = 0;
                if (Request.QueryString["menu_tab"] != null && Request.QueryString["menu_tab"] != "")
                    intMenuTab = Int32.Parse(Request.QueryString["menu_tab"]);
                Tab oTab = new Tab("", intMenuTab, "divMenu1", true, false);
                oTab.AddTab("Details", "");
                oTab.AddTab("Class / Environments", "");
                oTab.AddTab("Models", "");
                oTab.AddTab("Operating Systems / Service Packs", "");
                oTab.AddTab("Locations", "");
                oTab.AddTab("Scripts", "");
                strMenuTab1 = oTab.GetTabs();
            }

            btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this item?');");
            strLocation = oLocation.LoadDDL("ddlState", "ddlCity", "ddlAddress", hdnLocation.ClientID, 0, true, "ddlCommon");
            btnLocation.Attributes.Add("onclick", "return EnsureHidden('" + hdnLocation.ClientID + "','ddlState','Please select a location')" +
                ";");
        }
        private void LoadLists()
        {
        }
        private void LoopRepeater()
        {
            panView.Visible = true;
            DataSet ds = oAudit.GetScriptSets(0);
            DataView dv = ds.Tables[0].DefaultView;
            if (Request.QueryString["sort"] != null)
                dv.Sort = Request.QueryString["sort"].ToString();
            rptView.DataSource = dv;
            rptView.DataBind();
            foreach (RepeaterItem ri in rptView.Items)
            {
                ImageButton oDelete = (ImageButton)ri.FindControl("btnDelete");
                oDelete.Attributes.Add("onClick", "return confirm('Are you sure you want to delete this item?');");
                ImageButton oEnable = (ImageButton)ri.FindControl("btnEnable");
                if (oEnable.ImageUrl == "/admin/images/enabled.gif")
                {
                    oEnable.ToolTip = "Click to disable";
                    oEnable.Attributes.Add("onClick", "return confirm('Are you sure you want to disable this item?');");
                }
                else
                    oEnable.ToolTip = "Click to enable";
            }
        }
        protected void OrderView(Object Sender, EventArgs e)
        {
            LinkButton oButton = (LinkButton)Sender;
            string strSort;
            if (Request.QueryString["sort"] == null)
                strSort = oButton.CommandArgument + " ASC";
            else
                if (Request.QueryString["sort"].ToString() == (oButton.CommandArgument + " ASC"))
                    strSort = oButton.CommandArgument + " DESC";
                else
                    strSort = oButton.CommandArgument + " ASC";
            Response.Redirect(Request.Path + "?sort=" + strSort);
        }
        private void LoadClasses(TreeView oTree)
        {
            DataSet ds = oClass.Gets(1);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["name"].ToString();
                oNode.ToolTip = dr["name"].ToString();
                oNode.Value = dr["id"].ToString();
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                oNode.ShowCheckBox = true;
                oTree.Nodes.Add(oNode);
                LoadEnvironments(oNode, Int32.Parse(dr["id"].ToString()));
            }
            oTree.ExpandDepth = 1;
            oTree.Attributes.Add("oncontextmenu", "return false;");
        }
        private void LoadEnvironments(TreeNode oParent, int _classid)
        {
            DataSet dsOther = oAudit.GetScriptSetCEs(intID);
            DataSet ds = oClass.GetEnvironment(_classid, 0);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["name"].ToString();
                oNode.ToolTip = dr["name"].ToString();
                oNode.Value = dr["id"].ToString();
                oNode.SelectAction = TreeNodeSelectAction.None;
                oNode.ShowCheckBox = true;
                oNode.Checked = false;
                foreach (DataRow drOther in dsOther.Tables[0].Rows)
                {
                    if (_classid.ToString() == drOther["classid"].ToString() && dr["id"].ToString() == drOther["environmentid"].ToString())
                        oNode.Checked = true;
                }
                oParent.ChildNodes.Add(oNode);
            }
        }
        private void LoadTypes(TreeView oTree)
        {
            DataSet ds = oType.Gets(intPlatform, 0);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["name"].ToString();
                oNode.ToolTip = dr["name"].ToString();
                oNode.Value = dr["id"].ToString();
                oNode.ImageUrl = "/images/folder.gif";
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                oNode.ShowCheckBox = true;
                oTree.Nodes.Add(oNode);
                LoadModels(Int32.Parse(dr["id"].ToString()), oNode);
            }
            oTree.ExpandDepth = 2;
            oTree.Attributes.Add("oncontextmenu", "return false;");
        }
        private void LoadModels(int _typeid, TreeNode oParent)
        {
            DataSet ds = oModel.Gets(_typeid, 0);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["name"].ToString();
                oNode.ToolTip = dr["name"].ToString();
                oNode.Value = dr["id"].ToString();
                oNode.ImageUrl = "/images/folder.gif";
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                oNode.ShowCheckBox = true;
                oParent.ChildNodes.Add(oNode);
                LoadModelProperties(Int32.Parse(dr["id"].ToString()), oNode);
            }
        }
        private void LoadModelProperties(int _modelid, TreeNode oParent)
        {
            DataSet dsOther = oAudit.GetScriptSetModels(intID);
            DataSet ds = oModelsProperties.GetModels(0, _modelid, 0);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                string strName = dr["name"].ToString();
                if (strName == "")
                    strName = dr["modelname"].ToString();
                oNode.Text = strName;
                oNode.ToolTip = strName;
                oNode.Value = dr["id"].ToString();
                oNode.SelectAction = TreeNodeSelectAction.None;
                oNode.ShowCheckBox = true;
                oNode.Checked = false;
                foreach (DataRow drOther in dsOther.Tables[0].Rows)
                {
                    if (dr["id"].ToString() == drOther["modelid"].ToString())
                        oNode.Checked = true;
                }
                oParent.ChildNodes.Add(oNode);
            }
        }
        private void LoadOperatingSystems(TreeView oTree)
        {
            DataSet ds = oOperatingSystem.Gets(1);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["name"].ToString();
                oNode.ToolTip = dr["name"].ToString();
                oNode.Value = dr["id"].ToString();
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                oTree.Nodes.Add(oNode);
                LoadServicePacks(oNode, Int32.Parse(dr["id"].ToString()));
            }
            oTree.ExpandDepth = 1;
            oTree.Attributes.Add("oncontextmenu", "return false;");
        }
        private void LoadServicePacks(TreeNode oParent, int _osid)
        {
            DataSet dsOther = oAudit.GetScriptSetOsSps(intID);
            DataSet ds = oOperatingSystem.GetServicePack(_osid);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["name"].ToString();
                oNode.ToolTip = dr["name"].ToString();
                oNode.Value = dr["id"].ToString();
                oNode.SelectAction = TreeNodeSelectAction.None;
                oNode.ShowCheckBox = true;
                oNode.Checked = false;
                foreach (DataRow drOther in dsOther.Tables[0].Rows)
                {
                    if (_osid.ToString() == drOther["osid"].ToString() && dr["id"].ToString() == drOther["spid"].ToString())
                        oNode.Checked = true;
                }
                oParent.ChildNodes.Add(oNode);
            }
        }
        private void LoadLocations()
        {
            rptLocations.DataSource = oAudit.GetScriptSetLocations(intID);
            rptLocations.DataBind();
            lblLocations.Visible = (rptLocations.Items.Count == 0);
            foreach (RepeaterItem ri in rptLocations.Items)
                ((ImageButton)ri.FindControl("btnDeleteLocation")).Attributes.Add("onclick", "return confirm('Are you sure you want to delete this location?');");
        }
        protected void btnNew_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path + "?id=0");
        }
        protected void btnAdd_Click(Object Sender, EventArgs e)
        {
            if (intID == 0)
                intID = oAudit.AddScriptSet(txtName.Text, (chkModels.Checked ? 1 : 0), (chkSAN.Checked ? 1 : 0), (chkCluster.Checked ? 1 : 0), (chkMIS.Checked ? 1 : 0), (chkEnabled.Checked ? 1 : 0));
            else
            {
                // Details
                oAudit.UpdateScriptSet(intID, txtName.Text, (chkModels.Checked ? 1 : 0), (chkSAN.Checked ? 1 : 0), (chkCluster.Checked ? 1 : 0), (chkMIS.Checked ? 1 : 0), (chkEnabled.Checked ? 1 : 0));
                // Class / Environments
                oAudit.DeleteScriptSetCE(intID);
                foreach (TreeNode oNode in oTreeClassEnvironment.Nodes)
                    SaveClassEnvironment(oNode, Int32.Parse(oNode.Value));
                // Models
                oAudit.DeleteScriptSetModel(intID);
                foreach (TreeNode oNodeT in oTreeModels.Nodes)
                    foreach (TreeNode oNodeM in oNodeT.ChildNodes)
                        SaveModel(oNodeM);
                // Operating Systems / Service Packs
                oAudit.DeleteScriptSetOsSp(intID);
                foreach (TreeNode oNode in oTreeOperatingSystemServicePack.Nodes)
                    SaveOsSp(oNode, Int32.Parse(oNode.Value));
            }
            Response.Redirect(Request.Path + "?id=" + intID.ToString() + "&save=true");
        }
        protected void btnLocation_Click(Object Sender, EventArgs e)
        {
            oAudit.AddScriptSetLocation(intID, Int32.Parse(Request.Form[hdnLocation.UniqueID]));
            Response.Redirect(Request.Path + "?id=" + intID.ToString() + "&menu_tab=5");
        }
        protected void btnDeleteLocation_Click(Object Sender, ImageClickEventArgs e)
        {
            ImageButton oButton = (ImageButton)Sender;
            oAudit.DeleteScriptSetLocation(Int32.Parse(oButton.CommandArgument));
            Response.Redirect(Request.Path + "?id=" + intID.ToString() + "&menu_tab=5");
        }
        private void SaveClassEnvironment(TreeNode oParent, int _classid)
        {
            foreach (TreeNode oNode in oParent.ChildNodes)
            {
                if (oNode.Checked == true)
                    oAudit.AddScriptSetCE(intID, _classid, Int32.Parse(oNode.Value));
            }
        }
        private void SaveModel(TreeNode oParent)
        {
            foreach (TreeNode oNode in oParent.ChildNodes)
            {
                if (oNode.Checked == true)
                    oAudit.AddScriptSetModel(intID, Int32.Parse(oNode.Value));
            }
        }
        private void SaveOsSp(TreeNode oParent, int _osid)
        {
            foreach (TreeNode oNode in oParent.ChildNodes)
            {
                if (oNode.Checked == true)
                    oAudit.AddScriptSetOsSp(intID, _osid, Int32.Parse(oNode.Value));
            }
        }
        protected void btnEnable_Click(Object Sender, ImageClickEventArgs e)
        {
            ImageButton oButton = (ImageButton)Sender;
            oAudit.EnableScriptSet(Int32.Parse(oButton.CommandArgument), (oButton.ImageUrl == "/admin/images/enabled.gif" ? 0 : 1));
            Response.Redirect(Request.Path);
        }
        protected void btnDeleteLink_Click(Object Sender, ImageClickEventArgs e)
        {
            ImageButton oButton = (ImageButton)Sender;
            oAudit.DeleteScriptSet(Int32.Parse(oButton.CommandArgument));
            Response.Redirect(Request.Path);
        }
        protected void btnCancel_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path);
        }
        protected void btnDelete_Click(Object Sender, EventArgs e)
        {
            oAudit.DeleteScriptSet(intID);
            Response.Redirect(Request.Path);
        }
    }
}
