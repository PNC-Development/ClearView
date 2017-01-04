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
    public partial class wizard_steps : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected OnDemand oOnDemand;
        protected Types oType;
        protected Platforms oPlatform;
        protected int intProfile;
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cookies["loginreferrer"].Value = "/admin/forecast/wizard_steps.aspx";
            Response.Cookies["loginreferrer"].Expires = DateTime.Now.AddDays(30);
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Response.Redirect("/admin/login.aspx");
            oOnDemand = new OnDemand(intProfile, dsn);
            oType = new Types(intProfile, dsn);
            oPlatform = new Platforms(intProfile, dsn);
            if (!IsPostBack)
            {
                LoadPlatforms();
                btnOrder.Attributes.Add("onclick", "return OpenWindow('SUPPORTORDER','" + hdnParent.ClientID + "','" + hdnOrder.ClientID + "&type=OD_W_STEPS" + "',false,400,400);");
                btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this item?');");
                btnCancel.Attributes.Add("onclick", "return Cancel();");
                btnParent.Attributes.Add("onclick", "return OpenWindow('TYPEBROWSER','" + hdnParent.ClientID + "','&control=" + hdnParent.ClientID + "&controltext=" + lblParent.ClientID + "',false,400,600);");
                btnPath.Attributes.Add("onclick", "return OpenWindow('FILEBROWSER','" + txtPath.ClientID + "','',false,400,600);");
            }
        }
        private void LoadPlatforms()
        {
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
                oNew.NavigateUrl = "javascript:Add('" + dr["id"].ToString() + "','" + oType.Get(Int32.Parse(dr["id"].ToString()), "name") + "');";
                oNode.ChildNodes.Add(oNew);
            }
        }
        private void Load(int _typeid, TreeNode oParent)
        {
            DataSet ds = oOnDemand.GetWizardSteps(_typeid, 0);
            int intCount = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                intCount++;
                TreeNode oNode = new TreeNode();
                oNode.Text = intCount.ToString() + ") " + dr["name"].ToString();
                oNode.ToolTip = intCount.ToString() + ") " + dr["name"].ToString();
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                oNode.NavigateUrl = "javascript:Edit('" + dr["id"].ToString() + "','" + dr["typeid"].ToString() + "','" + oType.Get(Int32.Parse(dr["typeid"].ToString()), "name") + "','" + dr["name"].ToString() + "','" + dr["subtitle"].ToString() + "','" + dr["path"].ToString() + "','" + dr["show_cluster"].ToString() + "','" + dr["show_csm"].ToString() + "','" + dr["skip_cluster"].ToString() + "','" + dr["skip_csm"].ToString() + "','" + dr["enabled"].ToString() + "');";
                oParent.ChildNodes.Add(oNode);
            }
        }
        protected void btnAdd_Click(Object Sender, EventArgs e)
        {
            int intType = Int32.Parse(Request.Form[hdnParent.UniqueID]);
            if (Request.Form[hdnId.UniqueID] == "0")
                oOnDemand.AddWizardStep(intType, txtName.Text, txtSubtitle.Text, txtPath.Text, (chkShowCluster.Checked ? 1 : 0), (chkShowCSM.Checked ? 1 : 0), (chkSkipCluster.Checked ? 1 : 0), (chkSkipCSM.Checked ? 1 : 0), oOnDemand.GetWizardSteps(intType, 0).Tables[0].Rows.Count + 1, (chkEnabled.Checked ? 1 : 0));
            else
                oOnDemand.UpdateWizardStep(Int32.Parse(Request.Form[hdnId.UniqueID]), txtName.Text, txtSubtitle.Text, txtPath.Text, (chkShowCluster.Checked ? 1 : 0), (chkShowCSM.Checked ? 1 : 0), (chkSkipCluster.Checked ? 1 : 0), (chkSkipCSM.Checked ? 1 : 0), (chkEnabled.Checked ? 1 : 0));
            if (Request.Form[hdnOrder.UniqueID] != "")
            {
                string strOrder = Request.Form[hdnOrder.UniqueID];
                int intCount = 0;
                while (strOrder != "")
                {
                    intCount++;
                    int intId = Int32.Parse(strOrder.Substring(0, strOrder.IndexOf("&")));
                    strOrder = strOrder.Substring(strOrder.IndexOf("&") + 1);
                    oOnDemand.UpdateWizardStepOrder(intId, intCount);
                }
            }
            Response.Redirect(Request.Path);
        }
        protected  void btnDelete_Click(Object Sender, EventArgs e)
        {
            oOnDemand.DeleteWizardStep(Int32.Parse(Request.Form[hdnId.UniqueID]));
            Response.Redirect(Request.Path);
        }
    }
}
