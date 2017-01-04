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
using System.Globalization;
using Microsoft.ApplicationBlocks.Data;

namespace NCC.ClearView.Presentation.Web
{
    public partial class locations : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intProfile;
        protected Locations oLocation;
        protected Users oUser;
        protected TreeNode oSelected;
        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oLocation = new Locations(intProfile, dsn);
            oUser = new Users(intProfile, dsn);
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                lblId.Text = Request.QueryString["id"];
            string strControl = "";
            string strControlText = "";
            if (Request.QueryString["control"] != null)
                strControl = Request.QueryString["control"];
            if (Request.QueryString["controltext"] != null)
                strControlText = Request.QueryString["controltext"];
            btnClose.Attributes.Add("onclick", "return parent.HidePanel();");
            if (Request.QueryString["new"] != null && oUser.Get(intProfile, "add_location") == "1")
            {
                panNew.Visible = true;
                divTree.Style["display"] = "none";
                btnSaveNew.Attributes.Add("onclick", "return ValidateText('" + txtState.ClientID + "','Please enter a state') && ValidateText('" + txtCity.ClientID + "','Please enter a city') && ValidateText('" + txtAddress.ClientID + "','Please enter an address');");
            }
            else
            {
                btnNew.Enabled = (oUser.Get(intProfile, "add_location") == "1");
                LoadTree();
                btnSave.Attributes.Add("onclick", "return Update('" + hdnId.ClientID + "','" + strControl + "','" + txtName.ClientID + "','" + strControlText + "');");
            }
        }
        private void LoadTree()
        {
            PopulateTree();
            oTreeview.CollapseAll();
            if (oSelected == null)
                txtName.Text = "None";
            else
            {
                while (oSelected != null)
                {
                    oSelected.Expand();
                    oSelected = oSelected.Parent;
                }
            }
        }
        private void PopulateTree()
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
                LoadCities(Int32.Parse(dr["id"].ToString()), oNode);
            }
        }
        private void LoadCities(int _stateid, TreeNode oParent)
        {
            DataSet ds = oLocation.GetCitys(_stateid, 0);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["name"].ToString();
                oNode.ToolTip = dr["name"].ToString();
                oNode.ImageUrl = "/images/folder.gif";
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                oParent.ChildNodes.Add(oNode);
                LoadAddresses(Int32.Parse(dr["id"].ToString()), oNode);
            }
        }
        private void LoadAddresses(int _cityid, TreeNode oParent)
        {
            DataSet ds = oLocation.GetAddresss(_cityid, 0);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["name"].ToString();
                oNode.ToolTip = dr["name"].ToString();
                oNode.NavigateUrl = "javascript:Select(" + dr["id"].ToString() + ",'" + hdnId.ClientID + "','" + dr["fullname"].ToString() + "','" + txtName.ClientID + "');";
                oParent.ChildNodes.Add(oNode);
                if (dr["id"].ToString() == lblId.Text)
                {
                    oSelected = oNode;
                    txtName.Text = dr["fullname"].ToString();
                }
            }
            oTreeview.ExpandDepth = 2;
            oTreeview.Attributes.Add("oncontextmenu", "return false;");
        }
        protected void btnNew_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path + "?id=" + Request.QueryString["id"] + "&control=" + Request.QueryString["control"] + "&controltext=" + Request.QueryString["controltext"] + "&new=true");
        }
        protected void btnCancel_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path + "?id=" + Request.QueryString["id"] + "&control=" + Request.QueryString["control"] + "&controltext=" + Request.QueryString["controltext"]);
        }
        protected void btnSaveNew_Click(Object Sender, EventArgs e)
        {
            if (Request.QueryString["new"] != null)
            {
                TextInfo oText = new CultureInfo("en-US", false).TextInfo;
                string strState = oText.ToTitleCase(txtState.Text.ToLower());
                int intState = oLocation.GetState(strState);
                if (intState == 0)
                {
                    oLocation.AddState(strState, "", 1);
                    intState = oLocation.GetState(strState);
                }
                string strCity = oText.ToTitleCase(txtCity.Text.ToLower());
                int intCity = oLocation.GetCity(strCity);
                if (intCity == 0)
                {
                    oLocation.AddCity(intState, strCity, "", "", "", 1);
                    intCity = oLocation.GetCity(strCity);
                }
                string strAddress = oText.ToTitleCase(txtAddress.Text.ToLower());
                int intAddress = oLocation.GetAddress(strAddress);
                if (intAddress == 0)
                {
                    oLocation.AddAddress(intCity, strAddress, "", 0, "", 0, 0, 0, 0, 1, "", "", 0, 0, 0, 0, 0, 1);
                    intAddress = oLocation.GetAddress(strAddress);
                }
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "new_saved", "<script type=\"text/javascript\">alert('Location Added Successfully');window.parent.UpdateLocation('" + intAddress.ToString() + "','" + Request.QueryString["control"] + "','" + oLocation.GetFull(intAddress) + "','" + Request.QueryString["controltext"] + "');window.parent.HidePanel();<" + "/" + "script>");
            }
        }
    }
}
