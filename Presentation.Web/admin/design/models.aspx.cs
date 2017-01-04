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
    public partial class design_models : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected Design oDesign;
        protected ModelsProperties oModelsProperties;
        protected Locations oLocation;
        protected Classes oClass;
        protected Environments oEnvironment;
        protected int intProfile;
        protected int intID = 0;
        protected string strDisabled = "";
        protected string strMenuTab1 = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cookies["loginreferrer"].Value = Request.Path;
            Response.Cookies["loginreferrer"].Expires = DateTime.Now.AddDays(30);
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Response.Redirect("/admin/login.aspx");
            oDesign = new Design(intProfile, dsn);
            oModelsProperties = new ModelsProperties(intProfile, dsn);
            oLocation = new Locations(intProfile, dsn);
            oClass = new Classes(intProfile, dsn);
            oEnvironment = new Environments(intProfile, dsn);
            if (!IsPostBack)
                LoadList();
            if (Request.QueryString["id"] == null)
                LoopRepeater();
            else
            {
                panAdd.Visible = true;
                intID = Int32.Parse(Request.QueryString["id"]);
                if (intID > 0)
                {
                    if (!IsPostBack)
                    {
                        DataSet ds = oDesign.GetModel(intID);
                        hdnId.Value = intID.ToString();
                        txtName.Text = ds.Tables[0].Rows[0]["name"].ToString();
                        ddlModel.Text = ds.Tables[0].Rows[0]["modelid"].ToString();
                        lblName.Text = txtName.Text;
                        txtCores.Text = ds.Tables[0].Rows[0]["cores"].ToString();
                        txtRAM.Text = ds.Tables[0].Rows[0]["ram"].ToString();
                        chkEnabled.Checked = (ds.Tables[0].Rows[0]["enabled"].ToString() == "1");
                        btnAdd.Text = "Save";
                        btnAddBack.Text = "Save & Return";

                        // Load Phase Restrictions
                        LoadAddresses(oTreeInventory);
                    }
                }
                else
                {
                    if (!IsPostBack)
                    {
                        txtCores.Text = "0";
                        txtRAM.Text = "0";
                    }
                    strDisabled = "<p><img src=\"/images/bigAlert.gif\" border=\"0\" align=\"absmiddle\"/> <b>This section is only available when updating an existing item</b></p>";
                    btnOrder.Enabled = false;
                    btnDelete.Enabled = false;
                }
            }
            if (panAdd.Visible == true)
            {
                int intMenuTab = 0;
                if (Request.QueryString["menu_tab"] != null && Request.QueryString["menu_tab"] != "")
                    intMenuTab = Int32.Parse(Request.QueryString["menu_tab"]);
                Tab oTab = new Tab("", intMenuTab, "divMenu1", true, false);
                oTab.AddTab("Properties", "");
                oTab.AddTab("Out of Inventory", "");
                strMenuTab1 = oTab.GetTabs();
            }
            btnOrder.Attributes.Add("onclick", "return OpenWindow('SUPPORTORDER','" + hdnId.ClientID + "','" + hdnOrder.ClientID + "&type=D_MODELS" + "',false,400,400);");
            btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this item?');");
        }
        private void LoadList()
        {
            ddlModel.DataTextField = "name";
            ddlModel.DataValueField = "id";
            ddlModel.DataSource = oModelsProperties.Gets(1, 1);
            ddlModel.DataBind();
            ddlModel.Items.Insert(0, new ListItem("-- SELECT --", "0"));
        }
        private void LoadAddresses(TreeView oTree)
        {
            DataSet ds = oLocation.GetAddressCommon();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["commonname"].ToString();
                oNode.ImageUrl = "/images/folder.gif";
                oNode.Value = dr["id"].ToString();
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                oTree.Nodes.Add(oNode);
                LoadClasses(oNode, Int32.Parse(dr["id"].ToString()));
            }
            oTree.ExpandDepth = 1;
            oTree.Attributes.Add("oncontextmenu", "return false;");
        }
        private void LoadClasses(TreeNode oParent, int _addressid)
        {
            DataSet ds = oClass.GetForecasts(1);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["name"].ToString();
                oNode.ToolTip = dr["name"].ToString();
                oNode.Value = dr["id"].ToString();
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                bool boolEnvironment = LoadEnvironments(oNode, Int32.Parse(dr["id"].ToString()), _addressid);
                oNode.ImageUrl = (boolEnvironment ? "/images/cancel.gif" : "/images/check.gif");
                oParent.ChildNodes.Add(oNode);
            }
        }
        private bool LoadEnvironments(TreeNode oParent, int _classid, int _addressid)
        {
            bool boolEnvironment = false;
            DataSet ds = oClass.GetEnvironment(_classid, 1);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["name"].ToString();
                oNode.ToolTip = dr["name"].ToString();
                oNode.Value = dr["id"].ToString();
                oNode.SelectAction = TreeNodeSelectAction.None;
                oNode.ShowCheckBox = true;
                if (oDesign.IsModelInventory(intID, _addressid, _classid, Int32.Parse(dr["id"].ToString())) == false)
                {
                    boolEnvironment = true;
                    oNode.Checked = true;
                }
                oParent.ChildNodes.Add(oNode);
            }
            return boolEnvironment;
        }
        private void LoopRepeater()
        {
            panView.Visible = true;
            DataSet ds = oDesign.GetModels(0);
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
        protected void btnAdd_Click(Object Sender, EventArgs e)
        {
            Save();
            Response.Redirect(Request.Path + "?id=" + intID.ToString() + "&save=true");
        }
        protected void btnAddBack_Click(Object Sender, EventArgs e)
        {
            Save();
            Response.Redirect(Request.Path);
        }
        private void Save()
        {
            int intCores = 0;
            Int32.TryParse(txtCores.Text, out intCores);
            int intRAM = 0;
            Int32.TryParse(txtRAM.Text, out intRAM);
            if (intID == 0)
                oDesign.AddModel(txtName.Text, Int32.Parse(ddlModel.SelectedItem.Value), intCores, intRAM, (oDesign.GetModels(0).Tables[0].Rows.Count + 1), (chkEnabled.Checked ? 1 : 0));
            else
                oDesign.UpdateModel(intID, txtName.Text, Int32.Parse(ddlModel.SelectedItem.Value), intCores, intRAM, (chkEnabled.Checked ? 1 : 0));
            if (Request.Form[hdnOrder.UniqueID] != "")
            {
                string strOrder = Request.Form[hdnOrder.UniqueID];
                int intCount = 0;
                while (strOrder != "")
                {
                    intCount++;
                    int intId = Int32.Parse(strOrder.Substring(0, strOrder.IndexOf("&")));
                    strOrder = strOrder.Substring(strOrder.IndexOf("&") + 1);
                    oDesign.UpdateModelOrder(intId, intCount);
                }
            }
            // Save Inventory
            oDesign.DeleteModelInventory(intID);
            foreach (TreeNode oNodeA in oTreeInventory.Nodes)
            {
                foreach (TreeNode oNodeC in oNodeA.ChildNodes)
                {
                    foreach (TreeNode oNodeE in oNodeC.ChildNodes)
                    {
                        if (oNodeE.Checked == true)
                            oDesign.AddModelInventory(intID, Int32.Parse(oNodeA.Value), Int32.Parse(oNodeC.Value), Int32.Parse(oNodeE.Value), (oNodeE.Checked ? 1 : 0));
                    }
                }
            }
        }
        protected void btnEnable_Click(Object Sender, ImageClickEventArgs e)
        {
            ImageButton oButton = (ImageButton)Sender;
            oDesign.EnableModel(Int32.Parse(oButton.CommandArgument), (oButton.ImageUrl == "/admin/images/enabled.gif" ? 0 : 1));
            Response.Redirect(Request.Path);
        }
        protected void btnDeleteLink_Click(Object Sender, ImageClickEventArgs e)
        {
            ImageButton oButton = (ImageButton)Sender;
            oDesign.DeleteModel(Int32.Parse(oButton.CommandArgument));
            Response.Redirect(Request.Path);
        }
        protected void btnCancel_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path);
        }
        protected void btnDelete_Click(Object Sender, EventArgs e)
        {
            oDesign.DeleteModel(intID);
            Response.Redirect(Request.Path);
        }
    }
}
