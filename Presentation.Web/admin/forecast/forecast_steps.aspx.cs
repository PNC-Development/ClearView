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
    public partial class forecast_steps : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected Platforms oPlatform;
        protected Forecast oForecast;
        protected int intProfile;
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cookies["loginreferrer"].Value = "/admin/forecast/forecast_steps.aspx";
            Response.Cookies["loginreferrer"].Expires = DateTime.Now.AddDays(30);
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Response.Redirect("/admin/login.aspx");
            oPlatform = new Platforms(intProfile, dsn);
            oForecast = new Forecast(intProfile, dsn);
            if (!IsPostBack)
            {
                Load();
                btnOrder.Attributes.Add("onclick", "return OpenWindow('SUPPORTORDER','" + hdnParent.ClientID + "','" + hdnOrder.ClientID + "&type=F_STEPS" + "',false,400,400);");
                btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this item?');");
                btnCancel.Attributes.Add("onclick", "return Cancel();");
                btnParent.Attributes.Add("onclick", "return OpenWindow('PLATFORMBROWSER','" + hdnParent.ClientID + "','&control=" + hdnParent.ClientID + "&controltext=" + lblParent.ClientID + "',false,400,600);");
                btnPath.Attributes.Add("onclick", "return OpenWindow('FILEBROWSER','" + txtPath.ClientID + "','',false,400,600);");
                btnOverride.Attributes.Add("onclick", "return OpenWindow('FILEBROWSER','" + txtOverride.ClientID + "','',false,400,600);");
                btnReset.Attributes.Add("onclick", "return OpenWindow('FORECASTSTEPRESET','" + hdnId.ClientID + "','',false,'500',500);");
                btnImage.Attributes.Add("onclick", "return OpenWindow('IMAGEPATH','','" + txtImage.ClientID + "',false,500,550);");
            }
        }
        private void Load()
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
                Load(Int32.Parse(dr["platformid"].ToString()), oNode);
                TreeNode oNew = new TreeNode();
                oNew.Text = "&nbsp;Add Step";
                oNew.ToolTip = "Add Step";
                oNew.ImageUrl = "/images/green_right.gif";
                oNew.NavigateUrl = "javascript:Add('" + dr["platformid"].ToString() + "','" + oPlatform.GetName(Int32.Parse(dr["platformid"].ToString())) + "');";
                oNode.ChildNodes.Add(oNew);
            }
        }
        private void Load(int _parent, TreeNode oParent)
        {
            DataSet ds = oForecast.GetSteps(_parent, 0);
            int intCount = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                intCount++;
                TreeNode oNode = new TreeNode();
                oNode.Text = intCount.ToString() + ") " + dr["name"].ToString();
                oNode.ToolTip = intCount.ToString() + ") " + dr["name"].ToString();
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                oNode.NavigateUrl = "javascript:Edit('" + dr["id"].ToString() + "','" + dr["platformid"].ToString() + "','" + oPlatform.GetName(Int32.Parse(dr["platformid"].ToString())) + "','" + dr["name"].ToString() + "','" + dr["subtitle"].ToString() + "','" + dr["path"].ToString() + "','" + dr["override_path"].ToString() + "','" + dr["image_path"].ToString() + "','" + dr["additional"].ToString() + "','" + dr["enabled"].ToString() + "');";
                oParent.ChildNodes.Add(oNode);
            }
            oTreeview.ExpandDepth = 1;
            oTreeview.Attributes.Add("oncontextmenu", "return false;");
        }
        protected void btnAdd_Click(Object Sender, EventArgs e)
        {
            int intPlatform = Int32.Parse(Request.Form[hdnParent.UniqueID]);
            if (Request.Form[hdnId.UniqueID] == "0")
                oForecast.AddStep(intPlatform, txtName.Text, txtSubtitle.Text, txtPath.Text, txtOverride.Text, txtImage.Text, (chkAdditional.Checked ? 1 : 0), oForecast.GetSteps(intPlatform, 0).Tables[0].Rows.Count + 1, (chkEnabled.Checked ? 1 : 0));
            else
                oForecast.UpdateStep(Int32.Parse(Request.Form[hdnId.UniqueID]), txtName.Text, txtSubtitle.Text, txtPath.Text, txtOverride.Text, txtImage.Text, (chkAdditional.Checked ? 1 : 0), (chkEnabled.Checked ? 1 : 0));
            if (Request.Form[hdnOrder.UniqueID] != "")
            {
                string strOrder = Request.Form[hdnOrder.UniqueID];
                int intCount = 0;
                while (strOrder != "")
                {
                    intCount++;
                    int intId = Int32.Parse(strOrder.Substring(0, strOrder.IndexOf("&")));
                    strOrder = strOrder.Substring(strOrder.IndexOf("&") + 1);
                    oForecast.UpdateStepOrder(intId, intCount);
                }
            }
            Response.Redirect(Request.Path);
        }
        protected void btnDelete_Click(Object Sender, EventArgs e)
        {
            oForecast.DeleteStep(Int32.Parse(Request.Form[hdnId.UniqueID]));
            Response.Redirect(Request.Path);
        }
    }
}
