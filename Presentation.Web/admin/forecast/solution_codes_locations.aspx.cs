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
    public partial class solution_codes_locations : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intLocation = Int32.Parse(ConfigurationManager.AppSettings["OPSLocationID"]);
        protected int intPlatform = Int32.Parse(ConfigurationManager.AppSettings["ServerPlatformID"]);
        protected Locations oLocation;
        protected Solution oSolution;
        protected Environments oEnvironment;
        protected Classes oClass;
        protected int intProfile;
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cookies["loginreferrer"].Value = "/admin/forecast/solution_codes_locations.aspx";
            Response.Cookies["loginreferrer"].Expires = DateTime.Now.AddDays(30);
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Response.Redirect("/admin/login.aspx");
            oLocation = new Locations(intProfile, dsn);
            oSolution = new Solution(intProfile, dsn);
            oEnvironment = new Environments(intProfile, dsn);
            oClass = new Classes(intProfile, dsn);
            if (!IsPostBack)
            {
                LoadClasses();
                LoadLists();
                btnParent.Attributes.Add("onclick", "return OpenWindow('LOCATION_BROWSER','" + hdnParent.ClientID + "','&control=" + hdnParent.ClientID + "&controltext=" + lblParent.ClientID + "',false,400,600);");
                btnCancel.Attributes.Add("onclick", "return Cancel();");
                ddlClass.Attributes.Add("onchange", "PopulateEnvironments('" + ddlClass.ClientID + "','" + ddlEnvironment.ClientID + "',0);");
                ddlEnvironment.Attributes.Add("onchange", "UpdateDropDownHidden('" + ddlEnvironment.ClientID + "','" + hdnEnvironment.ClientID + "');");
            }
        }
        private void LoadLists()
        {
            ddlClass.DataTextField = "name";
            ddlClass.DataValueField = "id";
            ddlClass.DataSource = oClass.Gets(1);
            ddlClass.DataBind();
            ddlClass.Items.Insert(0, new ListItem("-- SELECT --", "0"));
            lstCode.DataTextField = "code";
            lstCode.DataValueField = "id";
            lstCode.DataSource = oSolution.GetCodes(1);
            lstCode.DataBind();
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
                TreeNode oNew = new TreeNode();
                oNew.Text = "&nbsp;Add Location";
                oNew.ToolTip = "Add Location";
                oNew.ImageUrl = "/images/green_right.gif";
                oNew.NavigateUrl = "javascript:Add('" + _parent.ToString() + "','" + dr["id"].ToString() + "');";
                oNode.ChildNodes.Add(oNew);
            }
        }
        private void LoadAddress(int _classid, int _environmentid, TreeNode oParent)
        {
            DataSet ds = oSolution.GetLocation(_classid, _environmentid);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["name"].ToString();
                oNode.ToolTip = dr["name"].ToString();
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                DataSet dsCodes = oSolution.GetLocation(_classid, _environmentid, Int32.Parse(dr["id"].ToString()));
                string strCodes = "";
                foreach (DataRow drCode in dsCodes.Tables[0].Rows)
                    strCodes += drCode["codeid"].ToString() + ";";
                oNode.NavigateUrl = "javascript:Edit('0','" + _classid.ToString() + "','" + _environmentid.ToString() + "','" + dr["id"].ToString() + "','" + dr["name"].ToString() + "','" + strCodes + "');";
                oParent.ChildNodes.Add(oNode);
            }
        }
        protected  void btnAdd_Click(Object Sender, EventArgs e)
        {
            int intClass = Int32.Parse(ddlClass.SelectedItem.Value);
            int intEnv = Int32.Parse(Request.Form[hdnEnvironment.UniqueID]);
            int intAddress = Int32.Parse(Request.Form[hdnParent.UniqueID]);
            oSolution.DeleteLocations(intClass, intEnv, intAddress);
            foreach (ListItem oList in lstCode.Items)
            {
                if (oList.Selected == true)
                    oSolution.AddLocation(intClass, intEnv, intAddress, Int32.Parse(oList.Value));
            }
            Response.Redirect(Request.Path);
        }
    }
}
