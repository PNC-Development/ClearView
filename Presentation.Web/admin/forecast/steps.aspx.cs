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
    public partial class steps : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected OnDemand oOnDemand;
        protected Types oType;
        protected Platforms oPlatform;
        protected int intProfile;
        protected int intID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cookies["loginreferrer"].Value = "/admin/forecast/steps.aspx";
            Response.Cookies["loginreferrer"].Expires = DateTime.Now.AddDays(30);
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Response.Redirect("/admin/login.aspx");
            oOnDemand = new OnDemand(intProfile, dsn);
            oType = new Types(intProfile, dsn);
            oPlatform = new Platforms(intProfile, dsn);
            if (Request.QueryString["id"] == null)
            {
                if (Request.QueryString["parent"] == null)
                    LoadPlatforms();
                else
                {
                    panAdd.Visible = true;
                    int intParent = Int32.Parse(Request.QueryString["parent"]);
                    hdnParent.Value = intParent.ToString();
                    lblParent.Text = oType.Get(intParent, "name");
                }
            }
            else
            {
                panAdd.Visible = true;
                intID = Int32.Parse(Request.QueryString["id"]);
                if (!IsPostBack)
                {
                    DataSet ds = oOnDemand.GetStep(intID);
                    int intParent = Int32.Parse(ds.Tables[0].Rows[0]["typeid"].ToString());
                    hdnParent.Value = intParent.ToString();
                    lblParent.Text = oType.Get(intParent, "name");
                    txtName.Text = ds.Tables[0].Rows[0]["name"].ToString();
                    txtTitle.Text = ds.Tables[0].Rows[0]["title"].ToString();
                    txtPath.Text = ds.Tables[0].Rows[0]["path"].ToString();
                    txtScript.Text = ds.Tables[0].Rows[0]["script"].ToString();
                    txtScriptDone.Text = ds.Tables[0].Rows[0]["done"].ToString();
                    radZEUS.Checked = (ds.Tables[0].Rows[0]["zeus"].ToString() == "1");
                    radPower.Checked = (ds.Tables[0].Rows[0]["power"].ToString() == "1");
                    radAccounts.Checked = (ds.Tables[0].Rows[0]["accounts"].ToString() == "1");
                    radInstalls.Checked = (ds.Tables[0].Rows[0]["installs"].ToString() == "1");
                    radGroups.Checked = (ds.Tables[0].Rows[0]["groups"].ToString() == "1");
                    ddlType.SelectedValue = ds.Tables[0].Rows[0]["type"].ToString();
                    txtInteraction.Text = ds.Tables[0].Rows[0]["interact_path"].ToString();
                    chkResume.Checked = (ds.Tables[0].Rows[0]["resume_error"].ToString() == "1");
                    chkShowBuild.Checked = (ds.Tables[0].Rows[0]["show_build"].ToString() == "1");
                    chkEnabled.Checked = (ds.Tables[0].Rows[0]["enabled"].ToString() == "1");
                }
            }
            btnOrder.Attributes.Add("onclick", "return OpenWindow('SUPPORTORDER','" + hdnParent.ClientID + "','" + hdnOrder.ClientID + "&type=OD_STEPS" + "',false,400,400);");
            btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this item?');");
            btnParent.Attributes.Add("onclick", "return OpenWindow('TYPEBROWSER','" + hdnParent.ClientID + "','&control=" + hdnParent.ClientID + "&controltext=" + lblParent.ClientID + "',false,400,600);");
            btnPath.Attributes.Add("onclick", "return OpenWindow('FILEBROWSER','" + txtPath.ClientID + "','',false,400,600);");
            btnInteraction.Attributes.Add("onclick", "return OpenWindow('FILEBROWSER','" + txtInteraction.ClientID + "','',false,400,600);");
        }
        private void LoadPlatforms()
        {
            panView.Visible = true;
            DataSet ds = oPlatform.GetForecasts(1);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["name"].ToString();
                oNode.ToolTip = dr["name"].ToString();
                oNode.ImageUrl = "/images/folder.gif";
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                oTreeview.Nodes.Add(oNode);
                LoadTypes(Int32.Parse(dr["platformid"].ToString()), oNode);
            }
            oTreeview.ExpandDepth = 1;
            oTreeview.Attributes.Add("oncontextmenu", "return false;");
        }
        private void LoadTypes(int _platformid, TreeNode oParent)
        {
            DataSet ds = oType.Gets(_platformid, 1);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["name"].ToString();
                oNode.ToolTip = dr["name"].ToString();
                oNode.ImageUrl = "/images/folder.gif";
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                oParent.ChildNodes.Add(oNode);
                Load(Int32.Parse(dr["id"].ToString()), oNode);
                TreeNode oNew = new TreeNode();
                oNew.Text = "&nbsp;Add Step";
                oNew.ToolTip = "Add Step";
                oNew.ImageUrl = "/images/green_right.gif";
                oNew.NavigateUrl = Request.Path + "?parent=" + dr["id"].ToString();
                oNode.ChildNodes.Add(oNew);
            }
        }
        private void Load(int _typeid, TreeNode oParent)
        {
            DataSet ds = oOnDemand.GetSteps(_typeid, 0);
            int intCount = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                intCount++;
                TreeNode oNode = new TreeNode();
                oNode.Text = intCount.ToString() + ") " + dr["name"].ToString();
                oNode.ToolTip = intCount.ToString() + ") " + dr["name"].ToString();
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                oNode.NavigateUrl = Request.Path + "?id=" + dr["id"].ToString();
                oParent.ChildNodes.Add(oNode);
            }
        }
        protected void btnAdd_Click(Object Sender, EventArgs e)
        {
            int intType = Int32.Parse(Request.Form[hdnParent.UniqueID]);
            if (intID == 0)
                oOnDemand.AddStep(intType, txtName.Text, txtTitle.Text, txtPath.Text, txtScript.Text, txtScriptDone.Text, txtInteraction.Text, (radZEUS.Checked ? 1 : 0), (radPower.Checked ? 1 : 0), (radAccounts.Checked ? 1 : 0), (radInstalls.Checked ? 1 : 0), (radGroups.Checked ? 1 : 0), Int32.Parse(ddlType.SelectedItem.Value), (chkResume.Checked ? 1 : 0), (chkShowBuild.Checked ? 1 : 0), oOnDemand.GetSteps(intType, 0).Tables[0].Rows.Count + 1, (chkEnabled.Checked ? 1 : 0));
            else
                oOnDemand.UpdateStep(intID, txtName.Text, txtTitle.Text, txtPath.Text, txtScript.Text, txtScriptDone.Text, txtInteraction.Text, (radZEUS.Checked ? 1 : 0), (radPower.Checked ? 1 : 0), (radAccounts.Checked ? 1 : 0), (radInstalls.Checked ? 1 : 0), (radGroups.Checked ? 1 : 0), Int32.Parse(ddlType.SelectedItem.Value), (chkResume.Checked ? 1 : 0), (chkShowBuild.Checked ? 1 : 0), (chkEnabled.Checked ? 1 : 0));
            if (Request.Form[hdnOrder.UniqueID] != "")
            {
                string strOrder = Request.Form[hdnOrder.UniqueID];
                int intCount = 0;
                while (strOrder != "")
                {
                    intCount++;
                    int intId = Int32.Parse(strOrder.Substring(0, strOrder.IndexOf("&")));
                    strOrder = strOrder.Substring(strOrder.IndexOf("&") + 1);
                    oOnDemand.UpdateStepOrder(intId, intCount);
                }
            }
            Response.Redirect(Request.Path);
        }
        protected void btnCancel_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path);
        }
        protected void btnDelete_Click(Object Sender, EventArgs e)
        {
            oOnDemand.DeleteStep(intID);
            Response.Redirect(Request.Path);
        }
    }
}
