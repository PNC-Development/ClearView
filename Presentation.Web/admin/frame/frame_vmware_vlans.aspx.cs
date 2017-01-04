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
    public partial class frame_vmware_vlans : BasePage
    {
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intLocation = Int32.Parse(ConfigurationManager.AppSettings["OPSLocationID"]);
        protected int intProfile;
        protected Classes oClass;
        protected Environments oEnvironment;
        protected Locations oLocation;
        protected IPAddresses oIPAddresses;
        protected VMWare oVMWare;
        protected int intID;
        protected int intParent = 0;
        protected string strLocation = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
            {
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
                oClass = new Classes(intProfile, dsn);
                oEnvironment = new Environments(intProfile, dsn);
                oLocation = new Locations(intProfile, dsn);
                oIPAddresses = new IPAddresses(intProfile, dsnIP, dsn);
                oVMWare = new VMWare(intProfile, dsn);
                btnClose.Attributes.Add("onclick", "return HidePanel();");
                if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                    intID = Int32.Parse(Request.QueryString["id"]);
                if (!IsPostBack)
                {
                    if (intID > 0)
                        lblName.Text = oVMWare.GetVlan(intID, "name");
                    LoadClasses();
                }
            }
            else
                btnSave.Enabled = false;
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
                oTree.Nodes.Add(oNode);
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
                oNode.Value = dr["id"].ToString();
                oNode.ToolTip = dr["vlan"].ToString();
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                oNode.ShowCheckBox = true;
                oNode.Checked = (oVMWare.GetVlanAssociation(intID, Int32.Parse(dr["id"].ToString())).Tables[0].Rows.Count > 0);
                oParent.ChildNodes.Add(oNode);
            }
        }
        protected  void btnSave_Click(Object Sender, EventArgs e)
        {
            oVMWare.DeleteVlanAssociation(intID);
            foreach (TreeNode oNode in oTree.Nodes)
                SaveEnvironment(oNode);
            Reload();
        }
        private void SaveEnvironment(TreeNode oParent)
        {
            foreach (TreeNode oNode in oParent.ChildNodes)
                SaveClass(oNode);
        }
        private void SaveClass(TreeNode oParent)
        {
            foreach (TreeNode oNode in oParent.ChildNodes)
                SaveLocation(oNode);
        }
        private void SaveLocation(TreeNode oParent)
        {
            foreach (TreeNode oNode in oParent.ChildNodes)
            {
                if (oNode.Checked == true)
                    oVMWare.AddVlanAssociation(intID, Int32.Parse(oNode.Value));
            }
        }
        private void Reload()
        {
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "reload", "<script type=\"text/javascript\">window.top.location.reload();<" + "/" + "script>");
        }
    }
}
