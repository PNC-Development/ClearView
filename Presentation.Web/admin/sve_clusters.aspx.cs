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
    public partial class sve_clusters : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected Solaris oSolaris;
        protected int intProfile;
        protected Classes oClass;
        protected IPAddresses oIPAddresses;
        protected Servers oServer;
        protected Locations oLocation;
        protected Resiliency oResiliency;
        protected int intID = 0;
        protected string strNetworks = "";
        protected string strDisabled = "";
        protected string strMenuTab1 = "";
        protected string strLocation = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cookies["loginreferrer"].Value = "/admin/sve_clusters.aspx";
            Response.Cookies["loginreferrer"].Expires = DateTime.Now.AddDays(30);
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Response.Redirect("/admin/login.aspx");
            oSolaris = new Solaris(intProfile, dsn);
            oClass = new Classes(intProfile, dsn);
            oIPAddresses = new IPAddresses(intProfile, dsnIP, dsn);
            oServer = new Servers(intProfile, dsn);
            oLocation = new Locations(intProfile, dsn);
            oResiliency = new Resiliency(intProfile, dsn);
            if (!IsPostBack)
                LoadLists();

            if (Request.QueryString["id"] == null)
            {
                if (Request.QueryString["add"] == null)
                    LoopRepeater();
                else
                    panAdd.Visible = true;
            }
            else
            {
                if (Request.QueryString["save"] != null)
                    panSave.Visible = true;
                panAdd.Visible = true;
                intID = Int32.Parse(Request.QueryString["id"]);
                if (intID > 0)
                {
                    if (!IsPostBack)
                    {
                        DataSet ds = oSolaris.GetSVECluster(intID);
                        txtName.Text = ds.Tables[0].Rows[0]["name"].ToString();
                        chkDatabase.Checked = (ds.Tables[0].Rows[0]["db"].ToString() == "1");
                        int intClass = 0;
                        if (Int32.TryParse(ds.Tables[0].Rows[0]["classid"].ToString(), out intClass) == true)
                            ddlClass.SelectedValue = intClass.ToString();
                        ddlResiliency.SelectedValue = ds.Tables[0].Rows[0]["resiliencyid"].ToString();
                        chkAvailable.Checked = (ds.Tables[0].Rows[0]["available"].ToString() == "1");
                        chkNetworks.Checked = (ds.Tables[0].Rows[0]["networks"].ToString() == "1");
                        txtComments.Text = ds.Tables[0].Rows[0]["comments"].ToString();
                        chkStorage.Checked = (ds.Tables[0].Rows[0]["storage_allocated"].ToString() == "1");
                        chkTrunking.Checked = (ds.Tables[0].Rows[0]["trunking"].ToString() == "1");
                        chkEnabled.Checked = (ds.Tables[0].Rows[0]["enabled"].ToString() == "1");
                        btnAdd.Text = "Update";

                        // Load Networks
                        if (chkNetworks.Checked == false)
                            strNetworks = "<p><img src=\"/images/bigAlert.gif\" border=\"0\" align=\"absmiddle\"/> <b>This section is only available if the <i>SPECIFY NETWORK RANGES(S)</i> flag is checked</b></p>";
                        else
                        {
                            LoadClasses(oTreeNetworks, intClass);
                            oTreeNetworks.Attributes.Add("onclick", "OnCheckBoxCheckChanged(event)");
                        }
                        LoadLocations();
                        // Load Hosts
                        rptHosts.DataSource = oServer.GetSVEClusters(intID);
                        rptHosts.DataBind();
                    }
                }
                else
                {
                    btnDelete.Enabled = false;
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
                oTab.AddTab("Locations", "");
                oTab.AddTab("Current Hosts", "");
                strMenuTab1 = oTab.GetTabs();
            }

            btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this item?');");
            strLocation = oLocation.LoadDDL("ddlState", "ddlCity", "ddlAddress", hdnLocation.ClientID, 0, true, "ddlCommon");
        }
        private void LoadLists()
        {
            ddlClass.DataTextField = "name";
            ddlClass.DataValueField = "id";
            ddlClass.DataSource = oClass.GetForecasts(1);
            ddlClass.DataBind();
            ddlClass.Items.Insert(0, new ListItem("-- SELECT --", "0"));
            ddlResiliency.DataValueField = "id";
            ddlResiliency.DataTextField = "name";
            ddlResiliency.DataSource = oResiliency.Gets(1);
            ddlResiliency.DataBind();
            ddlResiliency.Items.Insert(0, new ListItem("-- SELECT --", "0"));
        }
        private void LoopRepeater()
        {
            panView.Visible = true;
            DataSet ds = oSolaris.GetSVEClusters(-1, 0, -1, 0);
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
        protected void btnNew_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path + "?id=0");
        }

        private void LoadClasses(TreeView oTree, int intClass)
        {
            DataSet ds = oClass.Gets(1);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (dr["id"].ToString() == intClass.ToString())
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
            DataSet dsOther = oSolaris.GetSVENetworks(intID);
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
        private void LoadLocations()
        {
            rptLocations.DataSource = oSolaris.GetSVELocations(intID);
            rptLocations.DataBind();
            lblLocations.Visible = (rptLocations.Items.Count == 0);
            foreach (RepeaterItem ri in rptLocations.Items)
                ((ImageButton)ri.FindControl("btnDeleteLocation")).Attributes.Add("onclick", "return confirm('Are you sure you want to delete this location?');");
        }
        protected void btnAdd_Click(Object Sender, EventArgs e)
        {
            string strName = txtName.Text.Trim();
            if (intID == 0)
                intID = oSolaris.AddSVECluster(strName, (chkDatabase.Checked ? 1 : 0), Int32.Parse(ddlClass.SelectedItem.Value), Int32.Parse(ddlResiliency.SelectedItem.Value), (chkNetworks.Checked ? 1 : 0), (chkAvailable.Checked ? 1 : 0), txtComments.Text, (chkStorage.Checked ? 1 : 0), (chkTrunking.Checked ? 1 : 0), (chkEnabled.Checked ? 1 : 0));
            else
            {
                // Details
                oSolaris.UpdateSVECluster(intID, strName, (chkDatabase.Checked ? 1 : 0), Int32.Parse(ddlClass.SelectedItem.Value), Int32.Parse(ddlResiliency.SelectedItem.Value), (chkNetworks.Checked ? 1 : 0), (chkAvailable.Checked ? 1 : 0), txtComments.Text, (chkStorage.Checked ? 1 : 0), (chkTrunking.Checked ? 1 : 0), (chkEnabled.Checked ? 1 : 0));
                // Networks
                oSolaris.DeleteSVENetwork(intID);
                foreach (TreeNode oNodeC in oTreeNetworks.Nodes)
                    foreach (TreeNode oNodeE in oNodeC.ChildNodes)
                        foreach (TreeNode oNodeA in oNodeE.ChildNodes)
                            foreach (TreeNode oNodeV in oNodeA.ChildNodes)
                                SaveNetwork(oNodeV);
            }
            Response.Redirect(Request.Path + "?id=" + intID.ToString() + "&save=true");
        }
        protected void btnLocation_Click(Object Sender, EventArgs e)
        {
            oSolaris.AddSVELocation(intID, Int32.Parse(Request.Form[hdnLocation.UniqueID]));
            Response.Redirect(Request.Path + "?id=" + intID.ToString() + "&menu_tab=2");
        }
        protected void btnDeleteLocation_Click(Object Sender, ImageClickEventArgs e)
        {
            ImageButton oButton = (ImageButton)Sender;
            oSolaris.DeleteSVELocation(Int32.Parse(oButton.CommandArgument));
            Response.Redirect(Request.Path + "?id=" + intID.ToString() + "&menu_tab=2");
        }
        private void SaveNetwork(TreeNode oParent)
        {
            foreach (TreeNode oNode in oParent.ChildNodes)
            {
                if (oNode.Checked == true)
                    oSolaris.AddSVENetwork(intID, Int32.Parse(oNode.Value));
            }
        }
        protected void btnEnable_Click(Object Sender, ImageClickEventArgs e)
        {
            ImageButton oButton = (ImageButton)Sender;
            oSolaris.EnableSVECluster(Int32.Parse(oButton.CommandArgument), (oButton.ImageUrl == "/admin/images/enabled.gif" ? 0 : 1));
            Response.Redirect(Request.Path);
        }
        protected void btnDeleteLink_Click(Object Sender, ImageClickEventArgs e)
        {
            ImageButton oButton = (ImageButton)Sender;
            oSolaris.DeleteSVECluster(Int32.Parse(oButton.CommandArgument));
            Response.Redirect(Request.Path);
        }
        protected void btnCancel_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path);
        }
        protected void btnDelete_Click(Object Sender, EventArgs e)
        {
            oSolaris.DeleteSVECluster(intID);
            Response.Redirect(Request.Path);
        }
    }
}
