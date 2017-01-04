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
    public partial class location_address : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected Locations oLocation;
        protected int intProfile;
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cookies["loginreferrer"].Value = "/admin/location_address.aspx";
            Response.Cookies["loginreferrer"].Expires = DateTime.Now.AddDays(30);
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Response.Redirect("/admin/login.aspx");
            oLocation = new Locations(intProfile, dsn);
            if (!IsPostBack)
            {
                Load();
                btnOrder.Attributes.Add("onclick", "return OpenWindow('SUPPORTORDER','" + hdnParent.ClientID + "','" + hdnOrder.ClientID + "&type=LOCATION_A" + "',false,400,400);");
                btnParent.Attributes.Add("onclick", "return OpenWindow('LOCATION_C_BROWSER','" + hdnParent.ClientID + "','&control=" + hdnParent.ClientID + "&controltext=" + lblParent.ClientID + "',false,400,600);");
                btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this item?');");
                btnCancel.Attributes.Add("onclick", "return Cancel();");
            }
        }
        private void Load()
        {
            DataSet ds = oLocation.GetStates(1);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["name"].ToString();
                oNode.ToolTip = dr["name"].ToString();
                oNode.ImageUrl = "/images/folder.gif";
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                oTreeview.Nodes.Add(oNode);
                LoadCity(Int32.Parse(dr["id"].ToString()), oNode);
            }
            oTreeview.ExpandDepth = 0;
            oTreeview.Attributes.Add("oncontextmenu", "return false;");
        }
        private void LoadCity(int _parent, TreeNode oParent)
        {
            DataSet ds = oLocation.GetCitys(_parent, 1);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["name"].ToString();
                oNode.ToolTip = dr["name"].ToString();
                oNode.ImageUrl = "/images/folder.gif";
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                oParent.ChildNodes.Add(oNode);
                LoadAddress(Int32.Parse(dr["id"].ToString()), oNode);
                TreeNode oNew = new TreeNode();
                oNew.Text = "&nbsp;Add Address";
                oNew.ToolTip = "Add Address";
                oNew.ImageUrl = "/images/green_right.gif";
                oNew.NavigateUrl = "javascript:Add('" + dr["id"].ToString() + "','" + oLocation.GetCity(Int32.Parse(dr["id"].ToString()), "name") + "');";
                oNode.ChildNodes.Add(oNew);
            }
        }
        private void LoadAddress(int _parent, TreeNode oParent)
        {
            DataSet ds = oLocation.GetAddresss(_parent, 0);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["name"].ToString();
                oNode.ToolTip = dr["name"].ToString();
                oNode.ImageUrl = "/images/folder.gif";
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                oNode.NavigateUrl = "javascript:Edit('" + dr["id"].ToString() + "','" + dr["name"].ToString() + "','" + dr["factory_code"].ToString() + "','" + dr["cityid"].ToString() + "','" + oLocation.GetCity(Int32.Parse(dr["cityid"].ToString()), "name") + "','" + dr["common"].ToString() + "','" + dr["commonname"].ToString() + "','" + dr["storage"].ToString() + "','" + dr["tsm"].ToString() + "','" + dr["dr"].ToString() + "','" + dr["prod"].ToString() + "','" + dr["qa"].ToString() + "','" + dr["test"].ToString() + "','" + dr["offsite_build"].ToString() + "','" + dr["manual_build"].ToString() + "','" + dr["building_code"].ToString() + "','" + dr["service_now"].ToString() + "','" + dr["recovery"].ToString() + "','" + dr["vmware_ipaddress"].ToString() + "','" + dr["enabled"].ToString() + "');";
                oParent.ChildNodes.Add(oNode);
            }
        }
        protected void btnAdd_Click(Object Sender, EventArgs e)
        {
            int intParent = Int32.Parse(Request.Form[hdnParent.UniqueID]);
            if (Request.Form[hdnId.UniqueID] == "0")
                oLocation.AddAddress(intParent, txtName.Text, txtCode.Text, (chkCommon.Checked ? 1 : 0), txtCommonName.Text, (chkStorage.Checked ? 1 : 0), (chkTSM.Checked ? 1 : 0), (chkDR.Checked ? 1 : 0), (radTypeOffsite.Checked ? 1 : 0), (radTypeManual.Checked ? 1 : 0), txtBuildingCode.Text, txtServiceNow.Text, (chkRecovery.Checked ? 1 : 0), (chkVMwareIP.Checked ? 1 : 0), (chkProd.Checked ? 1 : 0), (chkQA.Checked ? 1 : 0), (chkTest.Checked ? 1 : 0), (chkEnabled.Checked ? 1 : 0));
            else
                oLocation.UpdateAddress(Int32.Parse(Request.Form[hdnId.UniqueID]), intParent, txtName.Text, txtCode.Text, (chkCommon.Checked ? 1 : 0), txtCommonName.Text, (chkStorage.Checked ? 1 : 0), (chkTSM.Checked ? 1 : 0), (chkDR.Checked ? 1 : 0), (radTypeOffsite.Checked ? 1 : 0), (radTypeManual.Checked ? 1 : 0), txtBuildingCode.Text, txtServiceNow.Text, (chkRecovery.Checked ? 1 : 0), (chkVMwareIP.Checked ? 1 : 0), (chkProd.Checked ? 1 : 0), (chkQA.Checked ? 1 : 0), (chkTest.Checked ? 1 : 0), (chkEnabled.Checked ? 1 : 0));
            if (Request.Form[hdnOrder.UniqueID] != "")
            {
                string strOrder = Request.Form[hdnOrder.UniqueID];
                int intCount = 0;
                while (strOrder != "")
                {
                    intCount++;
                    int intId = Int32.Parse(strOrder.Substring(0, strOrder.IndexOf("&")));
                    strOrder = strOrder.Substring(strOrder.IndexOf("&") + 1);
                    oLocation.UpdateAddressOrder(intId, intCount);
                }
            }
            Response.Redirect(Request.Path);
        }
        protected void btnDelete_Click(Object Sender, EventArgs e)
        {
            oLocation.DeleteAddress(Int32.Parse(Request.Form[hdnId.UniqueID]));
            Response.Redirect(Request.Path);
        }
    }
}
