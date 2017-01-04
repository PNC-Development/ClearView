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
    public partial class asset_types : BasePage
    {
   
    private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
    protected Types oType;
    protected Platforms oPlatform;
    protected int intProfile;
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cookies["loginreferrer"].Value = "/admin/asset/asset_types.aspx";
        Response.Cookies["loginreferrer"].Expires = DateTime.Now.AddDays(30);
        if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
            intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
        else
            Response.Redirect("/admin/login.aspx");
        oType = new Types(intProfile, dsn);
        oPlatform = new Platforms(intProfile, dsn);
        if (!IsPostBack)
        {
            Load();
            btnOrder.Attributes.Add("onclick", "return OpenWindow('SUPPORTORDER','" + hdnParent.ClientID + "','" + hdnOrder.ClientID + "&type=A_TYPE" + "',false,400,400);");
            btnParent.Attributes.Add("onclick", "return OpenWindow('PLATFORMBROWSER','" + hdnParent.ClientID + "','&control=" + hdnParent.ClientID + "&controltext=" + lblParent.ClientID + "',false,400,600);");
            btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this item?');");
            btnConfiguration.Attributes.Add("onclick", "return OpenWindow('FILEBROWSER','" + txtConfiguration.ClientID + "','',false,400,600);");
            btnDeploy.Attributes.Add("onclick", "return OpenWindow('FILEBROWSER','" + txtDeploy.ClientID + "','',false,400,600);");
            btnDesignExecution.Attributes.Add("onclick", "return OpenWindow('FILEBROWSER','" + txtDesignExecution.ClientID + "','',false,400,600);");
            btnForecastExecution.Attributes.Add("onclick", "return OpenWindow('FILEBROWSER','" + txtForecastExecution.ClientID + "','',false,400,600);");
            btnOndemandExecution.Attributes.Add("onclick", "return OpenWindow('FILEBROWSER','" + txtOndemandExecution.ClientID + "','',false,400,600);");
            btnOndemandJavaWindow.Attributes.Add("onclick", "return OpenWindow('FILEBROWSER','" + txtOndemandSteps.ClientID + "','',false,400,600);");
            btnCopy.Attributes.Add("onclick", "setCookie(\"configuration\", document.getElementById('" + txtConfiguration.ClientID + "').value, 1); setCookie(\"execution\", document.getElementById('" + txtForecastExecution.ClientID + "').value, 1); return false;");
            btnPaste.Attributes.Add("onclick", "document.getElementById('" + txtConfiguration.ClientID + "').value = getCookie(\"configuration\"); document.getElementById('" + txtForecastExecution.ClientID + "').value = getCookie(\"execution\"); return false;");
            btnCancel.Attributes.Add("onclick", "return Cancel();");
        }
    }
    private void Load()
    {
        DataSet ds = oPlatform.Gets(1);
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            TreeNode oNode = new TreeNode();
            oNode.Text = dr["name"].ToString();
            oNode.ToolTip = dr["name"].ToString();
            oNode.ImageUrl = "/images/folder.gif";
            oNode.SelectAction = TreeNodeSelectAction.Expand;
            oTreeview.Nodes.Add(oNode);
            Load(Int32.Parse(dr["platformid"].ToString()), oNode);
            TreeNode oNew = new TreeNode();
            oNew.Text = "&nbsp;Add Type";
            oNew.ToolTip = "Add Type";
            oNew.ImageUrl = "/images/green_right.gif";
            oNew.NavigateUrl = "javascript:Add('" + dr["platformid"].ToString() + "','" + oPlatform.GetName(Int32.Parse(dr["platformid"].ToString())) + "');";
            oNode.ChildNodes.Add(oNew);
        }
    }
    private void Load(int _parent, TreeNode oParent)
    {
        DataSet ds = oType.Gets(_parent, 0);
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            TreeNode oNode = new TreeNode();
            oNode.Text = dr["name"].ToString();
            oNode.ToolTip = dr["name"].ToString();
            if (dr["enabled"].ToString() == "1")
                oNode.ImageUrl = "/images/check.gif";
            else
                oNode.ImageUrl = "/images/cancel.gif";
            oNode.SelectAction = TreeNodeSelectAction.Expand;
            oNode.NavigateUrl = "javascript:Edit('" + dr["id"].ToString() + "','" + dr["name"].ToString() + "','" + dr["platformid"].ToString() + "','" + oPlatform.GetName(Int32.Parse(dr["platformid"].ToString())) + "','" + dr["configuration_path"].ToString() + "','" + dr["asset_deploy_path"].ToString() + "','" + dr["design_execution_path"].ToString() + "','" + dr["forecast_execution_path"].ToString() + "','" + dr["ondemand_execution_path"].ToString() + "','" + dr["ondemand_steps_path"].ToString() + "','" + dr["inventory_warning"].ToString() + "','" + dr["inventory_critical"].ToString() + "','" + dr["enabled"].ToString() + "');";
            oParent.ChildNodes.Add(oNode);
        }
        oTreeview.ExpandDepth = 1;
        oTreeview.Attributes.Add("oncontextmenu", "return false;");
    }
        protected void btnAdd_Click(Object Sender, EventArgs e)
    {
        int intParent = Int32.Parse(Request.Form[hdnParent.UniqueID]);
        if (Request.Form[hdnId.UniqueID] == "0")
            oType.Add(intParent, txtName.Text, txtConfiguration.Text, txtDeploy.Text, txtDesignExecution.Text, txtForecastExecution.Text, txtOndemandExecution.Text, txtOndemandSteps.Text, Int32.Parse(txtWarning.Text), Int32.Parse(txtCritical.Text), (oType.Gets(intParent, 0).Tables[0].Rows.Count + 1), (chkEnabled.Checked ? 1 : 0));
        else
            oType.Update(Int32.Parse(Request.Form[hdnId.UniqueID]), intParent, txtName.Text, txtConfiguration.Text, txtDeploy.Text, txtDesignExecution.Text, txtForecastExecution.Text, txtOndemandExecution.Text, txtOndemandSteps.Text, Int32.Parse(txtWarning.Text), Int32.Parse(txtCritical.Text), (chkEnabled.Checked ? 1 : 0));
        if (Request.Form[hdnOrder.UniqueID] != "")
        {
            string strOrder = Request.Form[hdnOrder.UniqueID];
            int intCount = 0;
            while (strOrder != "")
            {
                intCount++;
                int intId = Int32.Parse(strOrder.Substring(0, strOrder.IndexOf("&")));
                strOrder = strOrder.Substring(strOrder.IndexOf("&") + 1);
                oType.UpdateOrder(intId, intCount);
            }
        }
        Response.Redirect(Request.Path);
    }
        protected void btnDelete_Click(Object Sender, EventArgs e)
    {
        oType.Delete(Int32.Parse(Request.Form[hdnId.UniqueID]));
        Response.Redirect(Request.Path);
    }

    }
}
