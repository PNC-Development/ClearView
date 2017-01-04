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
    public partial class servername_subapplications : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected ServerName oServerName;
        protected int intProfile;
        protected Solution oSolution;
        protected Classes oClass;
        protected IPAddresses oIPAddresses;
        protected int intID = 0;
        protected string strNetworks = "";
        protected string strDisabled = "";
        protected string strMenuTab1 = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cookies["loginreferrer"].Value = "/admin/servername_subapplications.aspx";
            Response.Cookies["loginreferrer"].Expires = DateTime.Now.AddDays(30);
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Response.Redirect("/admin/login.aspx");
            oServerName = new ServerName(intProfile, dsn);
            oSolution = new Solution(intProfile, dsn);
            oClass = new Classes(intProfile, dsn);
            oIPAddresses = new IPAddresses(intProfile, dsnIP, dsn);
            if (!IsPostBack)
                LoadLists();

            if (Request.QueryString["id"] == null)
            {
                if (Request.QueryString["parent"] == null)
                    LoadComponents(oTreeDetails);
                else
                {
                    panAdd.Visible = true;
                    ddlApplication.SelectedValue = Request.QueryString["parent"];
                    btnDelete.Enabled = false;
                    strDisabled = "<p><img src=\"/images/bigAlert.gif\" border=\"0\" align=\"absmiddle\"/> <b>This section is only available when updating an existing item</b></p>";
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
                    DataSet ds = oServerName.GetSubApplication(intID);
                    ddlApplication.SelectedValue = ds.Tables[0].Rows[0]["applicationid"].ToString();
                    txtName.Text = ds.Tables[0].Rows[0]["name"].ToString();
                    txtCode.Text = ds.Tables[0].Rows[0]["code"].ToString();
                    txtFactoryCode.Text = ds.Tables[0].Rows[0]["factory_code"].ToString();
                    txtFactoryCodeSpecific.Text = ds.Tables[0].Rows[0]["factory_code_specific"].ToString();
                    txtZEUSArrayConfig.Text = ds.Tables[0].Rows[0]["zeus_array_config"].ToString();
                    txtZEUSOs.Text = ds.Tables[0].Rows[0]["zeus_os"].ToString();
                    txtZEUSOsVersion.Text = ds.Tables[0].Rows[0]["zeus_os_version"].ToString();
                    txtZEUSBuildType.Text = ds.Tables[0].Rows[0]["zeus_build_type"].ToString();
                    txtADMoveLocation.Text = ds.Tables[0].Rows[0]["ad_move_location"].ToString();
                    chkPermitNoReplication.Checked = (ds.Tables[0].Rows[0]["permit_no_replication"].ToString() == "1");
                    ddlSolutionCodes.SelectedValue = ds.Tables[0].Rows[0]["SolutionCode"].ToString();
                    chkNetworks.Checked = (ds.Tables[0].Rows[0]["networks"].ToString() == "1");
                    chkEnabled.Checked = (ds.Tables[0].Rows[0]["enabled"].ToString() == "1");
                    btnAdd.Text = "Update";

                    if (chkNetworks.Checked == false)
                        strNetworks = "<p><img src=\"/images/bigAlert.gif\" border=\"0\" align=\"absmiddle\"/> <b>This section is only available if the <i>SPECIFY NETWORK RANGES(S)</i> flag is checked</b></p>";
                    else
                    {
                        LoadClasses(oTreeNetworks);
                        oTreeNetworks.Attributes.Add("onclick", "OnCheckBoxCheckChanged(event)");
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
                oTab.AddTab("Network Ranges", "");
                strMenuTab1 = oTab.GetTabs();
            }

            btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this item?');");
            Variables oVariable = new Variables(intEnvironment);
            btnOrder.Attributes.Add("onclick", "return OpenWindow('SUPPORTORDER','" + ddlApplication.ClientID + "','" + hdnOrder.ClientID + "&type=SERVERNAME_SUBA" + "',false,400,400);");
            /*
            if (!IsPostBack)
            {
                LoadList();
                Load();
                btnOrder.Attributes.Add("onclick", "return OpenWindow('SUPPORTORDER','" + ddlApplication.ClientID + "','" + hdnOrder.ClientID + "&type=SERVERNAME_SUBA" + "',false,400,400);");
                btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this item?');");
                btnCancel.Attributes.Add("onclick", "return Cancel();");


                LoadSolutionCodes();
            }
            */
        }
        private void LoadLists()
        {
            ddlApplication.DataTextField = "name";
            ddlApplication.DataValueField = "id";
            ddlApplication.DataSource = oServerName.GetApplications(1);
            ddlApplication.DataBind();
            ddlApplication.Items.Insert(0, new ListItem("-- SELECT --", "0"));

            ddlSolutionCodes.DataTextField = "code";
            ddlSolutionCodes.DataValueField = "id";
            ddlSolutionCodes.DataSource = oSolution.GetCodes(1);
            ddlSolutionCodes.DataBind();
            ddlSolutionCodes.Items.Insert(0, new ListItem("-- NONE --", "0"));
        }
        /*
        private void Load()
        {
            DataSet ds = oServerName.GetApplications(1);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["name"].ToString();
                oNode.ToolTip = dr["name"].ToString();
                oNode.ImageUrl = "/images/folder.gif";
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                oTreeview.Nodes.Add(oNode);
                LoadSubApplication(Int32.Parse(dr["id"].ToString()), oNode);
                TreeNode oNew = new TreeNode();
                oNew.Text = "&nbsp;Add SubApplication";
                oNew.ToolTip = "Add SubApplication";
                oNew.ImageUrl = "/images/green_right.gif";
                oNew.NavigateUrl = "javascript:Add('" + dr["id"].ToString() + "');";
                oNode.ChildNodes.Add(oNew);
            }
            oTreeview.ExpandDepth = 0;
            oTreeview.Attributes.Add("oncontextmenu", "return false;");
        }
        */
        private void LoadComponents(TreeView oTree)
        {
            panView.Visible = true;
            DataSet ds = oServerName.GetApplications(1);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["name"].ToString();
                oNode.ToolTip = dr["name"].ToString();
                oNode.ImageUrl = "/images/folder.gif";
                oNode.Value = dr["id"].ToString();
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                oTree.Nodes.Add(oNode);
                LoadDetails(Int32.Parse(dr["id"].ToString()), oNode);
                TreeNode oNew = new TreeNode();
                oNew.Text = "&nbsp;Add Sub-Application";
                oNew.ToolTip = "Add Sub-Application";
                oNew.ImageUrl = "/images/green_right.gif";
                oNew.NavigateUrl = Request.Path + "?parent=" + dr["id"].ToString();
                oNode.ChildNodes.Add(oNew);
            }
            oTree.ExpandDepth = 1;
            oTree.Attributes.Add("oncontextmenu", "return false;");
        }
        private void LoadDetails(int _parent, TreeNode oParent)
        {
            DataSet ds = oServerName.GetSubApplications(_parent, 0);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["name"].ToString();
                oNode.ToolTip = dr["name"].ToString();
                oNode.Value = dr["id"].ToString();
                if (dr["enabled"].ToString() == "1")
                    oNode.ImageUrl = "/images/check.gif";
                else
                    oNode.ImageUrl = "/images/cancel.gif";
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                oNode.NavigateUrl = Request.Path + "?id=" + dr["id"].ToString();
                if (intID.ToString() != dr["id"].ToString() || oNode.ShowCheckBox == false)
                    oParent.ChildNodes.Add(oNode);
            }
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
                oTree.Nodes.Add(oNode);
                LoadEnvironments(oNode, Int32.Parse(dr["id"].ToString()));
            }
            oTree.ExpandDepth = 2;
            oTree.Attributes.Add("oncontextmenu", "return false;");
        }
        private void LoadEnvironments(TreeNode oParent, int _classid)
        {
            DataSet ds = oClass.GetEnvironment(_classid, 0);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["name"].ToString();
                oNode.ToolTip = dr["name"].ToString();
                oNode.Value = dr["id"].ToString();
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                oParent.ChildNodes.Add(oNode);
                LoadAddress(_classid, Int32.Parse(dr["id"].ToString()), oNode);
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
            DataSet dsOther = oServerName.GetSubApplicationNetworks(intID);
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
        protected void btnAdd_Click(Object Sender, EventArgs e)
        {
            int intApplication = Int32.Parse(ddlApplication.SelectedItem.Value);
            string strName = txtName.Text.Trim();
            if (strName == "")
                strName = oServerName.GetSubApplication(intID, "name");
            if (intID == 0)
                intID = oServerName.AddSubApplication(intApplication, strName, txtCode.Text, txtFactoryCode.Text, txtFactoryCodeSpecific.Text, txtZEUSArrayConfig.Text, txtZEUSOs.Text, txtZEUSOsVersion.Text, txtZEUSBuildType.Text, txtADMoveLocation.Text, (chkPermitNoReplication.Checked ? 1 : 0), Int32.Parse(ddlSolutionCodes.SelectedItem.Value), (chkNetworks.Checked ? 1 : 0), 0, (chkEnabled.Checked ? 1 : 0));
            else
            {
                // Details
                oServerName.UpdateSubApplication(intID, intApplication, strName, txtCode.Text, txtFactoryCode.Text, txtFactoryCodeSpecific.Text, txtZEUSArrayConfig.Text, txtZEUSOs.Text, txtZEUSOsVersion.Text, txtZEUSBuildType.Text, txtADMoveLocation.Text, (chkPermitNoReplication.Checked ? 1 : 0), Int32.Parse(ddlSolutionCodes.SelectedItem.Value), (chkNetworks.Checked ? 1 : 0), (chkEnabled.Checked ? 1 : 0));
                if (Request.Form[hdnOrder.UniqueID] != "")
                {
                    string strOrder = Request.Form[hdnOrder.UniqueID];
                    int intCount = 0;
                    while (strOrder != "")
                    {
                        intCount++;
                        int intId = Int32.Parse(strOrder.Substring(0, strOrder.IndexOf("&")));
                        strOrder = strOrder.Substring(strOrder.IndexOf("&") + 1);
                        oServerName.UpdateSubApplicationOrder(intId, intCount);
                    }
                }
                // Networks
                oServerName.DeleteSubApplicationNetwork(intID);
                foreach (TreeNode oNodeC in oTreeNetworks.Nodes)
                    foreach (TreeNode oNodeE in oNodeC.ChildNodes)
                        foreach (TreeNode oNodeA in oNodeE.ChildNodes)
                            foreach (TreeNode oNodeV in oNodeA.ChildNodes)
                                SaveNetwork(oNodeV);
            }
            Response.Redirect(Request.Path + "?id=" + intID.ToString() + "&save=true");
        }
        private void SaveNetwork(TreeNode oParent)
        {
            foreach (TreeNode oNode in oParent.ChildNodes)
            {
                if (oNode.Checked == true)
                    oServerName.AddSubApplicationNetwork(intID, Int32.Parse(oNode.Value));
            }
        }
        protected void btnCancel_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path);
        }
        protected void btnDelete_Click(Object Sender, EventArgs e)
        {
            oServerName.DeleteSubApplication(intID);
            Response.Redirect(Request.Path);
        }
    }
}
