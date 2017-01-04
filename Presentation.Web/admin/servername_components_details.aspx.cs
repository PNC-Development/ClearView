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
    public partial class servername_components_details : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intPlatform = Int32.Parse(ConfigurationManager.AppSettings["ServerPlatformID"]);
        protected ServerName oServerName;
        protected Classes oClass;
        protected Environments oEnvironment;
        protected Types oType;
        protected Models oModel;
        protected ModelsProperties oModelsProperties;
        protected OperatingSystems oOperatingSystem;
        protected ServicePacks oServicePack;
        protected Users oUser;
        protected Zeus oZeus;
        protected Audit oAudit;
        protected IPAddresses oIPAddresses;
        protected int intProfile;
        protected int intID = 0;
        protected string strApprovals = "";
        protected string strModels = "";
        protected string strNetworks = "";
        protected string strDisabled = "";
        protected string strMenuTab1 = "";
        protected string strMenuTabComp = "";
        protected string strMenuTabApp = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cookies["loginreferrer"].Value = "/admin/servername_components_details.aspx";
            Response.Cookies["loginreferrer"].Expires = DateTime.Now.AddDays(30);
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Response.Redirect("/admin/login.aspx");

            oServerName = new ServerName(intProfile, dsn);
            oClass = new Classes(intProfile, dsn);
            oEnvironment = new Environments(intProfile, dsn);
            oType = new Types(intProfile, dsn);
            oModel = new Models(intProfile, dsn);
            oModelsProperties = new ModelsProperties(intProfile, dsn);
            oOperatingSystem = new OperatingSystems(intProfile, dsn);
            oServicePack = new ServicePacks(intProfile, dsn);
            oUser = new Users(intProfile, dsn);
            oZeus = new Zeus(intProfile, dsn);
            oAudit = new Audit(intProfile, dsn);
            oIPAddresses = new IPAddresses(intProfile, dsnIP, dsn);

            if (!IsPostBack)
                LoadLists();

            if (Request.QueryString["id"] == null)
            {
                if (Request.QueryString["parent"] == null)
                    LoadComponents(oTreeDetails, false, false);
                else
                {
                    panAdd.Visible = true;
                    ddlParent.SelectedValue = Request.QueryString["parent"];
                    btnDelete.Enabled = false;
                    strDisabled = "<p><img src=\"/images/bigAlert.gif\" border=\"0\" align=\"absmiddle\"/> <b>This section is only available when updating an existing item</b></p>";
                    btnUser.Enabled = false;
                }
            }
            else
            {
                if (Request.QueryString["save"] != null)
                    panSave.Visible = true;
                panAdd.Visible = true;
                intID = Int32.Parse(Request.QueryString["id"]);
                if (!IsPostBack)
                {
                    DataSet ds = oServerName.GetComponentDetail(intID);
                    ddlParent.SelectedValue = ds.Tables[0].Rows[0]["componentid"].ToString();
                    txtName.Text = ds.Tables[0].Rows[0]["name"].ToString();
                    ddlZEUSArrayConfig.SelectedValue = ds.Tables[0].Rows[0]["zeus_array_config_id"].ToString();
                    ddlZEUSBuildType.SelectedValue = ds.Tables[0].Rows[0]["zeus_build_type_id"].ToString();
                    chkApproval.Checked = (ds.Tables[0].Rows[0]["approval"].ToString() == "1");
                    chkModels.Checked = (ds.Tables[0].Rows[0]["models"].ToString() == "1");
                    chkNetworks.Checked = (ds.Tables[0].Rows[0]["networks"].ToString() == "1");
                    chkInstall.Checked = (ds.Tables[0].Rows[0]["install"].ToString() == "1");
                    chkMount.Checked = (ds.Tables[0].Rows[0]["mount"].ToString() == "1");
                    txtNetworkPath.Text = ds.Tables[0].Rows[0]["network_path"].ToString();
                    txtInstallPath.Text = ds.Tables[0].Rows[0]["install_path"].ToString();
                    txtAD.Text = ds.Tables[0].Rows[0]["ad_move_location"].ToString();
                    ddlScript.SelectedValue = ds.Tables[0].Rows[0]["scriptid"].ToString();
                    ddlEnvironment.SelectedValue = ds.Tables[0].Rows[0]["environment"].ToString();
                    chkEnabled.Checked = (ds.Tables[0].Rows[0]["enabled"].ToString() == "1");
                    btnAdd.Text = "Update";

                    LoadClasses(oTreeClassEnvironment, false);
                    oTreeClassEnvironment.Attributes.Add("onclick", "OnCheckBoxCheckChanged(event)");
                    if (chkModels.Checked == false)
                        strModels = "<p><img src=\"/images/bigAlert.gif\" border=\"0\" align=\"absmiddle\"/> <b>This section is only available if the <i>SPECIFY INDIVIDUAL MODEL(S)</i> flag is checked</b></p>";
                    else
                    {
                        LoadTypes(oTreeModels);
                        oTreeModels.Attributes.Add("onclick", "OnCheckBoxCheckChanged(event)");
                    }
                    LoadOperatingSystems(oTreeOperatingSystemServicePack);
                    if (chkApproval.Checked == false)
                    {
                        btnUser.Enabled = false;
                        strApprovals = "<p><img src=\"/images/bigAlert.gif\" border=\"0\" align=\"absmiddle\"/> <b>This section is only available if the <i>REQUIRES APPROVAL</i> flag is checked</b></p>";
                    }
                    else
                        LoadUsers();
                    LoadComponents(oTreeInclude, true, false);
                    oTreeInclude.Attributes.Add("onclick", "OnCheckBoxCheckChanged(event)");
                    LoadComponents(oTreeExclude, false, true);
                    oTreeExclude.Attributes.Add("onclick", "OnCheckBoxCheckChanged(event)");
                    LoadApplications(oTreeIncludeApp, true);
                    LoadApplications(oTreeExcludeApp, false);
                    if (chkNetworks.Checked == false)
                        strNetworks = "<p><img src=\"/images/bigAlert.gif\" border=\"0\" align=\"absmiddle\"/> <b>This section is only available if the <i>SPECIFY NETWORK RANGES(S)</i> flag is checked</b></p>";
                    else
                    {
                        LoadClasses(oTreeNetworks, true);
                        oTreeNetworks.Attributes.Add("onclick", "OnCheckBoxCheckChanged(event)");
                    }
                    panScripts.Visible = true;
                    frmScripts.Attributes.Add("src", "/admin/servername_components_scripts.aspx?detailid=" + intID.ToString());
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
                oTab.AddTab("Approvers", "");
                oTab.AddTab("Operating Systems / Service Packs", "");
                oTab.AddTab("Associated Components", "");
                int intMenuTabComp = 0;
                if (Request.QueryString["menu_tab_comp"] != null && Request.QueryString["menu_tab_comp"] != "")
                    intMenuTabComp = Int32.Parse(Request.QueryString["menu_tab_comp"]);
                Tab oTabComp = new Tab("", intMenuTabComp, "divMenuComp1", true, false);
                oTabComp.AddTab("Inclusions", "");
                oTabComp.AddTab("Exclusions", "");
                strMenuTabComp = oTabComp.GetTabs();
                oTab.AddTab("Associated Applications", "");
                int intMenuTabApp = 0;
                if (Request.QueryString["menu_tab_app"] != null && Request.QueryString["menu_tab_app"] != "")
                    intMenuTabApp = Int32.Parse(Request.QueryString["menu_tab_app"]);
                Tab oTabApp = new Tab("", intMenuTabApp, "divMenuApp1", true, false);
                oTabApp.AddTab("Inclusions", "");
                oTabApp.AddTab("Exclusions", "");
                strMenuTabApp = oTabApp.GetTabs();
                oTab.AddTab("Network Ranges", "");
                oTab.AddTab("Scripts", "");
                strMenuTab1 = oTab.GetTabs();
            }

            btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this item?');");
            Variables oVariable = new Variables(intEnvironment);
            txtUser.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'300','195','" + divUser.ClientID + "','" + lstUser.ClientID + "','" + hdnUser.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
            lstUser.Attributes.Add("ondblclick", "AJAXClickRow();");
            btnUser.Attributes.Add("onclick", "return EnsureHidden('" + hdnUser.ClientID + "','" + txtUser.ClientID + "','Please enter the LAN ID of the user')" +
                ";");
            btnArrayConfig.Attributes.Add("onclick", "return OpenWindow('SUPPORTORDER','" + hdnArrayConfigId.ClientID + "','" + hdnArrayConfigOrder.ClientID + "&type=ZEUS_ARRAY_CONFIGS" + "',false,400,400);");
            btnBuildType.Attributes.Add("onclick", "return OpenWindow('SUPPORTORDER','" + hdnBuildTypeId.ClientID + "','" + hdnBuildTypeOrder.ClientID + "&type=ZEUS_BUILD_TYPES" + "',false,400,400);");
        }
        private void LoadLists()
        {
            ddlParent.DataTextField = "name";
            ddlParent.DataValueField = "id";
            ddlParent.DataSource = oServerName.GetComponents(0);
            ddlParent.DataBind();
            ddlZEUSArrayConfig.DataTextField = "name";
            ddlZEUSArrayConfig.DataValueField = "id";
            ddlZEUSArrayConfig.DataSource = oZeus.GetArrayConfigs(1);
            ddlZEUSArrayConfig.DataBind();
            ddlZEUSArrayConfig.Items.Insert(0, new ListItem("-- NONE --", "0"));
            ddlZEUSBuildType.DataTextField = "name";
            ddlZEUSBuildType.DataValueField = "id";
            ddlZEUSBuildType.DataSource = oZeus.GetBuildTypes(1);
            ddlZEUSBuildType.DataBind();
            ddlZEUSBuildType.Items.Insert(0, new ListItem("-- NONE --", "0"));
            ddlScript.DataTextField = "name";
            ddlScript.DataValueField = "id";
            ddlScript.DataSource = oAudit.GetScripts(1);
            ddlScript.DataBind();
            ddlScript.Items.Insert(0, new ListItem("-- NONE --", "0"));
        }
        private void LoadComponents(TreeView oTree, bool boolInclude, bool boolExclude)
        {
            panView.Visible = true;
            DataSet ds = oServerName.GetComponents(1);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["name"].ToString();
                oNode.ToolTip = dr["name"].ToString();
                oNode.ImageUrl = "/images/folder.gif";
                oNode.Value = dr["id"].ToString();
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                if (boolInclude == true || boolExclude == true)
                    oNode.ShowCheckBox = true;
                oTree.Nodes.Add(oNode);
                LoadDetails(Int32.Parse(dr["id"].ToString()), oNode, boolInclude, boolExclude);
                if (boolInclude == false && boolExclude == false)
                {
                    TreeNode oNew = new TreeNode();
                    oNew.Text = "&nbsp;Add Detail";
                    oNew.ToolTip = "Add Detail";
                    oNew.ImageUrl = "/images/green_right.gif";
                    oNew.NavigateUrl = Request.Path + "?parent=" + dr["id"].ToString();
                    oNode.ChildNodes.Add(oNew);
                }
            }
            oTree.ExpandDepth = 1;
            oTree.Attributes.Add("oncontextmenu", "return false;");
        }
        private void LoadDetails(int _componentid, TreeNode oParent, bool boolInclude, bool boolExclude)
        {
            DataSet ds = oServerName.GetComponentDetails(_componentid, 0);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["name"].ToString();
                oNode.ToolTip = dr["name"].ToString();
                oNode.Value = dr["id"].ToString();
                if (boolInclude == false && boolExclude == false)
                {
                    if (dr["enabled"].ToString() == "1")
                        oNode.ImageUrl = "/images/check.gif";
                    else
                        oNode.ImageUrl = "/images/cancel.gif";
                    oNode.SelectAction = TreeNodeSelectAction.Expand;
                    oNode.NavigateUrl = Request.Path + "?id=" + dr["id"].ToString();
                }
                else
                {
                    DataSet dsOther = oServerName.GetComponentDetailRelateds(intID, (boolInclude ? 1 : 0));
                    oNode.ShowCheckBox = true;
                    oNode.Checked = false;
                    foreach (DataRow drOther in dsOther.Tables[0].Rows)
                    {
                        if (dr["id"].ToString() == drOther["relatedid"].ToString())
                            oNode.Checked = true;
                    }
                    oNode.SelectAction = TreeNodeSelectAction.None;
                }
                if (intID.ToString() != dr["id"].ToString() || oNode.ShowCheckBox == false)
                    oParent.ChildNodes.Add(oNode);
            }
        }
        private void LoadClasses(TreeView oTree, bool _network)
        {
            DataSet ds = oClass.Gets(1);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["name"].ToString();
                oNode.ToolTip = dr["name"].ToString();
                oNode.Value = dr["id"].ToString();
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                oNode.ShowCheckBox = (_network == false);
                oTree.Nodes.Add(oNode);
                LoadEnvironments(oNode, Int32.Parse(dr["id"].ToString()), _network);
            }
            oTree.ExpandDepth = (_network ? 2 : 1);
            oTree.Attributes.Add("oncontextmenu", "return false;");
        }
        private void LoadEnvironments(TreeNode oParent, int _classid, bool _network)
        {
            DataSet dsOther = oServerName.GetComponentDetailCEs(intID);
            DataSet ds = oClass.GetEnvironment(_classid, 0);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["name"].ToString();
                oNode.ToolTip = dr["name"].ToString();
                oNode.Value = dr["id"].ToString();
                oNode.SelectAction = (_network ? TreeNodeSelectAction.Expand : TreeNodeSelectAction.None);
                oNode.ShowCheckBox = (_network == false);
                oNode.Checked = false;
                foreach (DataRow drOther in dsOther.Tables[0].Rows)
                {
                    if (_classid.ToString() == drOther["classid"].ToString() && dr["id"].ToString() == drOther["environmentid"].ToString())
                        oNode.Checked = true;
                }
                oParent.ChildNodes.Add(oNode);
                if (_network && oNode.Checked)
                    LoadAddress(_classid, Int32.Parse(dr["id"].ToString()), oNode);
            }
            if (_network)
            {
                for (int ii = 0; ii < oParent.ChildNodes.Count; ii++)
                {
                    if (oParent.ChildNodes[ii].Checked == false)
                    {
                        oParent.ChildNodes.Remove(oParent.ChildNodes[ii]);
                        ii--;
                    }
                }
            }
        }
        private void LoadAddress(int _classid, int _environmentid, TreeNode oParent)
        {
            DataSet ds = oIPAddresses.GetVlansAddress(_classid, _environmentid, 0);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["name"].ToString();
                oNode.ToolTip = dr["name"].ToString();
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                oNode.ShowCheckBox = false;
                oParent.ChildNodes.Add(oNode);
                if (LoadVlans(_classid, _environmentid, Int32.Parse(dr["id"].ToString()), oNode) == true)
                    oNode.Expand();
            }
        }
        private bool LoadVlans(int _classid, int _environmentid, int _addressid, TreeNode oParent)
        {
            bool boolExpand = false;
            DataSet ds = oIPAddresses.GetVlansAddress(_classid, _environmentid, _addressid, 0);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["vlan"].ToString();
                oNode.ToolTip = dr["vlan"].ToString();
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                oNode.ShowCheckBox = false;
                oParent.ChildNodes.Add(oNode);
                if (LoadNetworks(Int32.Parse(dr["id"].ToString()), oNode) == true)
                {
                    oNode.Expand();
                    boolExpand = true;
                }
            }
            return boolExpand;
        }
        private bool LoadNetworks(int _parent, TreeNode oParent)
        {
            bool boolExpand = false;
            DataSet dsOther = oServerName.GetComponentDetailNetworks(intID);
            DataSet ds = oIPAddresses.GetNetworks(_parent, 0);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = oIPAddresses.GetNetworkName(Int32.Parse(dr["id"].ToString()));
                oNode.ToolTip = oIPAddresses.GetNetworkName(Int32.Parse(dr["id"].ToString()));
                oNode.Value = dr["id"].ToString();
                oNode.SelectAction = TreeNodeSelectAction.None;
                oNode.ShowCheckBox = true;
                oNode.Checked = false;
                foreach (DataRow drOther in dsOther.Tables[0].Rows)
                {
                    if (dr["id"].ToString() == drOther["networkid"].ToString())
                    {
                        oNode.Checked = true;
                        boolExpand = true;
                    }
                }
                oParent.ChildNodes.Add(oNode);
            }
            return boolExpand;
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
            DataSet dsOther = oServerName.GetComponentDetailModels(intID);
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
            DataSet dsOther = oServerName.GetComponentDetailOsSps(intID);
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
        private void LoadUsers()
        {
            rptUsers.DataSource = oServerName.GetComponentDetailUsers(intID, 0);
            rptUsers.DataBind();
            lblUsers.Visible = (rptUsers.Items.Count == 0);
            foreach (RepeaterItem ri in rptUsers.Items)
                ((ImageButton)ri.FindControl("btnDeleteLink")).Attributes.Add("onclick", "return confirm('Are you sure you want to delete this user?');");
        }
        private void LoadApplications(TreeView oTree, bool boolInclude)
        {
            DataSet dsOther = oServerName.GetComponentDetailRelatedApplications(intID, (boolInclude ? 1 : 0));
            DataSet ds = oServerName.GetApplications(1);
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
                    if (dr["id"].ToString() == drOther["applicationid"].ToString())
                        oNode.Checked = true;
                }
                oTree.Nodes.Add(oNode);
            }
            oTree.ExpandDepth = 0;
            oTree.Attributes.Add("oncontextmenu", "return false;");
        }
        protected void btnAdd_Click(Object Sender, EventArgs e)
        {
            int intComponent = Int32.Parse(ddlParent.SelectedItem.Value);
            string strName = txtName.Text.Trim();
            if (strName == "")
                strName = oServerName.GetComponent(intComponent, "name");
            if (intID == 0)
                intID = oServerName.AddComponentDetail(intComponent, strName, Int32.Parse(ddlZEUSArrayConfig.SelectedItem.Value), Int32.Parse(ddlZEUSBuildType.SelectedItem.Value), (chkApproval.Checked ? 1 : 0), (chkModels.Checked ? 1 : 0), (chkNetworks.Checked ? 1 : 0), (chkInstall.Checked ? 1 : 0), (chkMount.Checked ? 1 : 0), txtNetworkPath.Text, txtInstallPath.Text, txtAD.Text, Int32.Parse(ddlScript.SelectedItem.Value), Int32.Parse(ddlEnvironment.SelectedItem.Value), (chkEnabled.Checked ? 1 : 0));
            else
            {
                // Details
                oServerName.UpdateComponentDetail(intID, intComponent, strName, Int32.Parse(ddlZEUSArrayConfig.SelectedItem.Value), Int32.Parse(ddlZEUSBuildType.SelectedItem.Value), (chkApproval.Checked ? 1 : 0), (chkModels.Checked ? 1 : 0), (chkNetworks.Checked ? 1 : 0), (chkInstall.Checked ? 1 : 0), (chkMount.Checked ? 1 : 0), txtNetworkPath.Text, txtInstallPath.Text, txtAD.Text, Int32.Parse(ddlScript.SelectedItem.Value), Int32.Parse(ddlEnvironment.SelectedItem.Value), (chkEnabled.Checked ? 1 : 0));
                // Update Zeus Array Config Order
                if (Request.Form[hdnArrayConfigOrder.UniqueID] != "")
                {
                    string strOrder = Request.Form[hdnArrayConfigOrder.UniqueID];
                    int intCount = 0;
                    while (strOrder != "")
                    {
                        intCount++;
                        int intId = Int32.Parse(strOrder.Substring(0, strOrder.IndexOf("&")));
                        strOrder = strOrder.Substring(strOrder.IndexOf("&") + 1);
                        oZeus.UpdateArrayConfigOrder(intId, intCount);
                    }
                }
                // Update Zues Build Type Order
                if (Request.Form[hdnBuildTypeOrder.UniqueID] != "")
                {
                    string strOrder = Request.Form[hdnBuildTypeOrder.UniqueID];
                    int intCount = 0;
                    while (strOrder != "")
                    {
                        intCount++;
                        int intId = Int32.Parse(strOrder.Substring(0, strOrder.IndexOf("&")));
                        strOrder = strOrder.Substring(strOrder.IndexOf("&") + 1);
                        oZeus.UpdateArrayConfigOrder(intId, intCount);
                    }
                }
                // Class / Environments
                oServerName.DeleteComponentDetailCE(intID);
                foreach (TreeNode oNode in oTreeClassEnvironment.Nodes)
                    SaveClassEnvironment(oNode, Int32.Parse(oNode.Value));
                // Approvers
                // Models
                oServerName.DeleteComponentDetailModel(intID);
                foreach (TreeNode oNodeT in oTreeModels.Nodes)
                    foreach (TreeNode oNodeM in oNodeT.ChildNodes)
                        SaveModel(oNodeM);
                // Operating Systems / Service Packs
                oServerName.DeleteComponentDetailOsSp(intID);
                foreach (TreeNode oNode in oTreeOperatingSystemServicePack.Nodes)
                    SaveOsSp(oNode, Int32.Parse(oNode.Value));
                // Associated Components
                oServerName.DeleteComponentDetailRelated(intID, 0);
                foreach (TreeNode oNode in oTreeExclude.Nodes)
                    SaveRelated(oNode, 0);
                oServerName.DeleteComponentDetailRelated(intID, 1);
                foreach (TreeNode oNode in oTreeInclude.Nodes)
                    SaveRelated(oNode, 1);
                // Associated Applications
                oServerName.DeleteComponentDetailRelatedApplication(intID, 0);
                foreach (TreeNode oNode in oTreeExcludeApp.Nodes)
                    SaveRelatedApplication(oNode, 0);
                oServerName.DeleteComponentDetailRelatedApplication(intID, 1);
                foreach (TreeNode oNode in oTreeIncludeApp.Nodes)
                    SaveRelatedApplication(oNode, 1);
                // Networks
                oServerName.DeleteComponentDetailNetwork(intID);
                foreach (TreeNode oNodeC in oTreeNetworks.Nodes)
                    foreach (TreeNode oNodeE in oNodeC.ChildNodes)
                        foreach (TreeNode oNodeA in oNodeE.ChildNodes)
                            foreach (TreeNode oNodeV in oNodeA.ChildNodes)
                                SaveNetwork(oNodeV);
            }
            Response.Redirect(Request.Path + "?id=" + intID.ToString() + "&save=true");
        }
        private void SaveClassEnvironment(TreeNode oParent, int _classid)
        {
            foreach (TreeNode oNode in oParent.ChildNodes)
            {
                if (oNode.Checked == true)
                    oServerName.AddComponentDetailCE(intID, _classid, Int32.Parse(oNode.Value));
            }
        }
        private void SaveModel(TreeNode oParent)
        {
            foreach (TreeNode oNode in oParent.ChildNodes)
            {
                if (oNode.Checked == true)
                    oServerName.AddComponentDetailModel(intID, Int32.Parse(oNode.Value));
            }
        }
        private void SaveOsSp(TreeNode oParent, int _osid)
        {
            foreach (TreeNode oNode in oParent.ChildNodes)
            {
                if (oNode.Checked == true)
                    oServerName.AddComponentDetailOsSp(intID, _osid, Int32.Parse(oNode.Value));
            }
        }
        private void SaveRelated(TreeNode oParent, int _include)
        {
            foreach (TreeNode oNode in oParent.ChildNodes)
            {
                if (oNode.Checked == true)
                    oServerName.AddComponentDetailRelated(intID, Int32.Parse(oNode.Value), _include);
            }
        }
        private void SaveRelatedApplication(TreeNode oParent, int _include)
        {
            if (oParent.Checked == true)
                oServerName.AddComponentDetailRelatedApplication(intID, Int32.Parse(oParent.Value), _include);
        }
        private void SaveNetwork(TreeNode oParent)
        {
            foreach (TreeNode oNode in oParent.ChildNodes)
            {
                if (oNode.Checked == true)
                    oServerName.AddComponentDetailNetwork(intID, Int32.Parse(oNode.Value));
            }
        }
        protected void btnUser_Click(Object Sender, EventArgs e)
        {
            oServerName.AddComponentDetailUser(intID, Int32.Parse(Request.Form[hdnUser.UniqueID]), (chkEnabledUser.Checked ? 1 : 0));
            Response.Redirect(Request.Path + "?id=" + intID.ToString() + "&menu_tab=4");
        }
        protected void btnEnableLink_Click(Object Sender, ImageClickEventArgs e)
        {
            ImageButton oButton = (ImageButton)Sender;
            oServerName.EnableComponentDetailUser(Int32.Parse(oButton.CommandArgument), (oButton.ImageUrl == "/admin/images/enabled.gif" ? 0 : 1));
            Response.Redirect(Request.Path + "?id=" + intID.ToString() + "&menu_tab=4");
        }
        protected void btnDeleteLink_Click(Object Sender, ImageClickEventArgs e)
        {
            ImageButton oButton = (ImageButton)Sender;
            oServerName.DeleteComponentDetailUser(Int32.Parse(oButton.CommandArgument));
            Response.Redirect(Request.Path + "?id=" + intID.ToString() + "&menu_tab=4");
        }
        protected void btnCancel_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path);
        }
        protected void btnDelete_Click(Object Sender, EventArgs e)
        {
            oServerName.DeleteComponentDetail(intID);
            Response.Redirect(Request.Path);
        }
    }
}
