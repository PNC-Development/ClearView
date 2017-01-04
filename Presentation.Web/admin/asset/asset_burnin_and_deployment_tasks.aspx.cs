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
    public partial class asset_burnin_and_deployment_tasks : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected AssetBurnInAndDeploymentTasks oAssetBurnInAndDeploymentTasks;
        protected int intProfile;
        protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cookies["loginreferrer"].Value = "/admin/asset/asset_burnin_and_deployment_steps.aspx";
        Response.Cookies["loginreferrer"].Expires = DateTime.Now.AddDays(30);
        if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
            intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
        else
            Response.Redirect("/admin/login.aspx");
        oAssetBurnInAndDeploymentTasks = new AssetBurnInAndDeploymentTasks(intProfile, dsn);
        if (!IsPostBack)
        {
            LoopRepeater();
            btnOrder.Attributes.Add("onclick", "return OpenWindow('SUPPORTORDER','" + hdnId.ClientID + "','" + hdnOrder.ClientID + "&type=ASSET_BURNIN_AND_DEPLOYMENT" + "',false,400,400);");
            btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this item?');");
            btnCancel.Attributes.Add("onclick", "return Cancel();");
        }
    }
        private void LoopRepeater()
    {
        DataSet ds = oAssetBurnInAndDeploymentTasks.gets(0);
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
        public void OrderView(Object Sender, EventArgs e)
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
        protected  void btnAdd_Click(Object Sender, EventArgs e)
        {
            if (Request.Form[hdnId.UniqueID] == "0")
                oAssetBurnInAndDeploymentTasks.add(txtName.Text.Trim(), double.Parse(txtHours.Text), double.Parse(txtSLA.Text), (chkEnabled.Checked ? 1 : 0), intProfile);
            else
                oAssetBurnInAndDeploymentTasks.update(Int32.Parse(Request.Form[hdnId.UniqueID]), txtName.Text, double.Parse(txtHours.Text), double.Parse(txtSLA.Text), (chkEnabled.Checked ? 1 : 0), intProfile);
            if (Request.Form[hdnOrder.UniqueID] != "")
            {
                string strOrder = Request.Form[hdnOrder.UniqueID];
                int intCount = 0;
                while (strOrder != "")
                {
                    intCount++;
                    int intId = Int32.Parse(strOrder.Substring(0, strOrder.IndexOf("&")));
                    strOrder = strOrder.Substring(strOrder.IndexOf("&") + 1);
                    oAssetBurnInAndDeploymentTasks.updateOrder(intId, intCount,intProfile);
                }
            }
            Response.Redirect(Request.Path);
        }
        protected void btnEnable_Click(Object Sender, ImageClickEventArgs e)
        {
            ImageButton oButton = (ImageButton)Sender;
            oAssetBurnInAndDeploymentTasks.enable(Int32.Parse(oButton.CommandArgument), (oButton.ImageUrl == "/admin/images/enabled.gif" ? 0 : 1),intProfile);
            Response.Redirect(Request.Path);
        }
        protected void btnDeleteLink_Click(Object Sender, ImageClickEventArgs e)
        {
            ImageButton oButton = (ImageButton)Sender;
            oAssetBurnInAndDeploymentTasks.delete(Int32.Parse(oButton.CommandArgument),intProfile);
            Response.Redirect(Request.Path);
        }
        protected void btnDelete_Click(Object Sender, EventArgs e)
        {
            oAssetBurnInAndDeploymentTasks.delete(Int32.Parse(Request.Form[hdnId.UniqueID]),intProfile);
            Response.Redirect(Request.Path);
        }

    }
}
