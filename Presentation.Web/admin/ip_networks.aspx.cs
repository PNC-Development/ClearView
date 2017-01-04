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
    public partial class ip_networks : BasePage
    {
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected IPAddresses oIPAddresses;
        protected Environments oEnvironment;
        protected Classes oClass;
        protected int intProfile;
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cookies["loginreferrer"].Value = "/admin/ip_networks.aspx";
            Response.Cookies["loginreferrer"].Expires = DateTime.Now.AddDays(30);
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Response.Redirect("/admin/login.aspx");
            oIPAddresses = new IPAddresses(intProfile, dsnIP, dsn);
            oEnvironment = new Environments(intProfile, dsn);
            oClass = new Classes(intProfile, dsn);
            if (!IsPostBack)
            {
                LoadClasses();
                btnParent.Attributes.Add("onclick", "return OpenWindow('IPVLANBROWSER','" + hdnParent.ClientID + "','&control=" + hdnParent.ClientID + "&controltext=" + lblParent.ClientID + "',false,400,600);");
                btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this item?');");
                btnCancel.Attributes.Add("onclick", "return Cancel();");
                txtMin1.Attributes.Add("onkeyup", "CopyTextBox('" + txtMin1.ClientID + "','" + txtMax1.ClientID + "');");
                txtMin2.Attributes.Add("onkeyup", "CopyTextBox('" + txtMin2.ClientID + "','" + txtMax2.ClientID + "');");
                txtMin3.Attributes.Add("onkeyup", "CopyTextBox('" + txtMin3.ClientID + "','" + txtMax3.ClientID + "');");
                btnRelations.Attributes.Add("onclick", "return OpenWindow('NETWORKSRELATED','" + hdnId.ClientID + "','',false,'500',500);");
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
                oTreeview.Nodes.Add(oNode);
                LoadEnvironments(Int32.Parse(dr["id"].ToString()), oNode);
            }
            oTreeview.ExpandDepth = 1;
            oTreeview.Attributes.Add("oncontextmenu", "return false;");
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
                oParent.ChildNodes.Add(oNode);
                LoadAddress(_parent, Int32.Parse(dr["id"].ToString()), oNode);
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
                oNode.ImageUrl = "/images/folder.gif";
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                oParent.ChildNodes.Add(oNode);
                LoadVlans(_classid, _environmentid, Int32.Parse(dr["id"].ToString()), oNode);
            }
        }
        private void LoadVlans(int _classid, int _environmentid, int _addressid, TreeNode oParent)
        {
            DataSet ds = oIPAddresses.GetVlansAddress(_classid, _environmentid, _addressid, 0);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["vlan"].ToString();
                oNode.ToolTip = dr["vlan"].ToString();
                oNode.ImageUrl = "/images/folder.gif";
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                oParent.ChildNodes.Add(oNode);
                Load(Int32.Parse(dr["id"].ToString()), oNode);
                TreeNode oNew = new TreeNode();
                oNew.Text = "&nbsp;Add Network";
                oNew.ToolTip = "Add Network";
                oNew.ImageUrl = "/images/green_right.gif";
                oNew.NavigateUrl = "javascript:Add('" + dr["id"].ToString() + "','" + oIPAddresses.GetVlan(Int32.Parse(dr["id"].ToString()), "vlan") + "');";
                oNode.ChildNodes.Add(oNew);
            }
        }
        private void Load(int _parent, TreeNode oParent)
        {
            DataSet ds = oIPAddresses.GetNetworks(_parent, 0);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = oIPAddresses.GetNetworkName(Int32.Parse(dr["id"].ToString()));
                oNode.ToolTip = oIPAddresses.GetNetworkName(Int32.Parse(dr["id"].ToString()));
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                oNode.NavigateUrl = "javascript:Edit('" + dr["id"].ToString() + "','" + dr["vlanid"].ToString() + "','" + oIPAddresses.GetVlan(Int32.Parse(dr["vlanid"].ToString()), "vlan") + "','" + dr["add1"].ToString() + "','" + dr["add2"].ToString() + "','" + dr["add3"].ToString() + "','" + dr["min4"].ToString() + "','" + dr["max4"].ToString() + "','" + dr["mask"].ToString() + "','" + dr["gateway"].ToString() + "','" + dr["starting"].ToString() + "','" + dr["maximum"].ToString() + "','" + dr["reverse"].ToString() + "','" + dr["routable"].ToString() + "','" + dr["notify"].ToString() + "','" + dr["only_apps"].ToString() + "','" + dr["only_components"].ToString() + "','" + dr["description"].ToString() + "','" + dr["enabled"].ToString() + "');";
                oParent.ChildNodes.Add(oNode);
            }
        }
        protected void btnAdd_Click(Object Sender, EventArgs e)
        {
            int intParent = Int32.Parse(Request.Form[hdnParent.UniqueID]);
            if (Request.Form[hdnId.UniqueID] == "0")
                oIPAddresses.AddNetwork(intParent, Int32.Parse(txtMin1.Text), Int32.Parse(txtMin2.Text), Int32.Parse(txtMin3.Text), Int32.Parse(txtMin4.Text), Int32.Parse(txtMax4.Text), txtMask.Text, txtGateway.Text, Int32.Parse(txtStart.Text), Int32.Parse(txtMaximum.Text), (chkReverse.Checked ? 1 : 0), (chkRoutable.Checked ? 1 : 0), txtNotify.Text, (chkOnlyApps.Checked ? 1 : 0), (chkOnlyComponennts.Checked ? 1 : 0), txtDescription.Text,(chkEnabled.Checked ? 1 : 0));
            else
                oIPAddresses.UpdateNetwork(Int32.Parse(Request.Form[hdnId.UniqueID]), intParent, Int32.Parse(txtMin1.Text), Int32.Parse(txtMin2.Text), Int32.Parse(txtMin3.Text), Int32.Parse(txtMin4.Text), Int32.Parse(txtMax4.Text), txtMask.Text, txtGateway.Text, Int32.Parse(txtStart.Text), Int32.Parse(txtMaximum.Text), (chkReverse.Checked ? 1 : 0), (chkRoutable.Checked ? 1 : 0), txtNotify.Text, (chkOnlyApps.Checked ? 1 : 0), (chkOnlyComponennts.Checked ? 1 : 0), txtDescription.Text, (chkEnabled.Checked ? 1 : 0));
            Response.Redirect(Request.Path);
        }
        protected void btnDelete_Click(Object Sender, EventArgs e)
        {
            oIPAddresses.DeleteNetwork(Int32.Parse(Request.Form[hdnId.UniqueID]));
            Response.Redirect(Request.Path);
        }
    }
}
