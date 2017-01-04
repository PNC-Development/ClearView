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
    public partial class frame_build_locations_rdp_zones : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intProfile;
        protected BuildLocation oBuildLocation;
        protected Zones oZone;
        protected Locations oLocation;
        protected RoomsNew oRoom;
        protected int intAddress;
        protected int intBuildLocationRDP;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Reload();
            oBuildLocation = new BuildLocation(intProfile, dsn);
            oZone = new Zones(intProfile, dsn);
            oLocation = new Locations(intProfile, dsn);
            oRoom = new RoomsNew(intProfile, dsn);
            btnClose.Attributes.Add("onclick", "return HidePanel();");
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                intBuildLocationRDP = Int32.Parse(Request.QueryString["id"]);
            if (!IsPostBack)
            {
                if (intBuildLocationRDP > 0)
                {
                    Int32.TryParse(oBuildLocation.GetRDP(intBuildLocationRDP, "addressid"), out intAddress);
                    lblName.Text = oLocation.GetFull(intAddress);
                }
                LoadRooms();
            }
        }
        private void LoadRooms()
        {
            DataSet ds = oRoom.Gets(intAddress, 1);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["room"].ToString();
                oNode.ToolTip = dr["room"].ToString();
                oNode.Value = dr["roomid"].ToString();
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                oTree.Nodes.Add(oNode);
                Load(oNode, Int32.Parse(dr["roomid"].ToString()));
            }
        }
        private void Load(TreeNode oParent, int intRoom)
        {
            DataSet dsOther = oBuildLocation.GetRDPZone(intBuildLocationRDP);
            DataSet ds = oZone.Gets(intRoom, 1);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["zone"].ToString() + " (VLAN: " + dr["vlan"].ToString() + ")";
                oNode.ToolTip = dr["zone"].ToString();
                oNode.Value = dr["zoneid"].ToString();
                oNode.SelectAction = TreeNodeSelectAction.None;
                oNode.Checked = false;
                foreach (DataRow drOther in dsOther.Tables[0].Rows)
                {
                    if (dr["zoneid"].ToString() == drOther["zoneid"].ToString())
                        oNode.Checked = true;
                }
                if (oParent != null)
                    oParent.ChildNodes.Add(oNode);
                else
                    oTree.Nodes.Add(oNode);
            }
        }
        protected  void btnSave_Click(Object Sender, EventArgs e)
        {
            oBuildLocation.DeleteRDPZone(intBuildLocationRDP);
            foreach (TreeNode oNode in oTree.Nodes)
                SaveBuildLocation(oNode, Int32.Parse(oNode.Value));
            Reload();
        }
        private void SaveBuildLocation(TreeNode oParent, int _roomid)
        {
            foreach (TreeNode oNode in oParent.ChildNodes)
            {
                if (oNode.Checked == true)
                    oBuildLocation.AddRDPZone(intBuildLocationRDP, Int32.Parse(oNode.Value));
            }
        }
        private void Reload()
        {
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "reload", "<script type=\"text/javascript\">window.top.location.reload();<" + "/" + "script>");
        }
    }
}
