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
    public partial class workstation_components : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected Workstations oWorkstation;
        protected int intProfile;
        protected int intID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cookies["loginreferrer"].Value = "/admin/workstation_components.aspx";
            Response.Cookies["loginreferrer"].Expires = DateTime.Now.AddDays(30);
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Response.Redirect("/admin/login.aspx");
            oWorkstation = new Workstations(intProfile, dsn);
            if (Request.QueryString["id"] == null)
            {
                if (Request.QueryString["add"] == null)
                    LoopRepeater();
                else
                    panAdd.Visible = true;
            }
            else
            {
                panAdd.Visible = true;
                intID = Int32.Parse(Request.QueryString["id"]);
                if (intID > 0 && !IsPostBack)
                {
                    DataSet ds = oWorkstation.GetComponent(intID);
                    txtName.Text = ds.Tables[0].Rows[0]["name"].ToString();
                    txtZEUSBuildType.Text = ds.Tables[0].Rows[0]["zeus_build_type"].ToString();
                    txtADMoveLocation.Text = ds.Tables[0].Rows[0]["ad_move_location"].ToString();
                    chkSMSInstall.Checked = (ds.Tables[0].Rows[0]["sms_install"].ToString() == "1");
                    txtScript.Text = ds.Tables[0].Rows[0]["script"].ToString();
                    txtWorkstationGroup.Text = ds.Tables[0].Rows[0]["workstation_group"].ToString();
                    txtUserGroup.Text = ds.Tables[0].Rows[0]["user_group"].ToString();
                    txtNotifications.Text = ds.Tables[0].Rows[0]["notifications"].ToString();
                    chkEnabled.Checked = (ds.Tables[0].Rows[0]["enabled"].ToString() == "1");
                }
            }
            btnOrder.Attributes.Add("onclick", "return OpenWindow('SUPPORTORDER','" + hdnId.ClientID + "','" + hdnOrder.ClientID + "&type=WORKSTATION_C" + "',false,400,400);");
            btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this item?');");
            btnOS.Attributes.Add("onclick", "return OpenWindow('WORKSTATION_COMPONENTS','" + hdnId.ClientID + "','',false,'500',300);");
        }
        private void LoopRepeater()
        {
            panView.Visible = true;
            DataSet ds = oWorkstation.GetComponents(0);
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
        protected void OrderView(Object Sender, EventArgs e)
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
        protected void btnNew_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path + "?id=0");
        }
        protected void btnAdd_Click(Object Sender, EventArgs e)
        {
            if (intID == 0)
                oWorkstation.AddComponent(txtName.Text, txtZEUSBuildType.Text, txtADMoveLocation.Text, (chkSMSInstall.Checked ? 1 : 0), txtScript.Text, txtWorkstationGroup.Text, txtUserGroup.Text, txtNotifications.Text, (oWorkstation.GetComponents(0).Tables[0].Rows.Count + 1), (chkEnabled.Checked ? 1 : 0));
            else
                oWorkstation.UpdateComponent(intID, txtName.Text, txtZEUSBuildType.Text, txtADMoveLocation.Text, (chkSMSInstall.Checked ? 1 : 0), txtScript.Text, txtWorkstationGroup.Text, txtUserGroup.Text, txtNotifications.Text, (chkEnabled.Checked ? 1 : 0));
            if (Request.Form[hdnOrder.UniqueID] != "")
            {
                string strOrder = Request.Form[hdnOrder.UniqueID];
                int intCount = 0;
                while (strOrder != "")
                {
                    intCount++;
                    int intId = Int32.Parse(strOrder.Substring(0, strOrder.IndexOf("&")));
                    strOrder = strOrder.Substring(strOrder.IndexOf("&") + 1);
                    oWorkstation.UpdateComponentOrder(intId, intCount);
                }
            }
            Response.Redirect(Request.Path);
        }
        protected void btnEnable_Click(Object Sender, ImageClickEventArgs e)
        {
            ImageButton oButton = (ImageButton)Sender;
            oWorkstation.EnableComponent(Int32.Parse(oButton.CommandArgument), (oButton.ImageUrl == "/admin/images/enabled.gif" ? 0 : 1));
            Response.Redirect(Request.Path);
        }
        protected void btnDeleteLink_Click(Object Sender, ImageClickEventArgs e)
        {
            ImageButton oButton = (ImageButton)Sender;
            oWorkstation.DeleteComponent(Int32.Parse(oButton.CommandArgument));
            Response.Redirect(Request.Path);
        }
        protected void btnCancel_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path);
        }
        protected void btnDelete_Click(Object Sender, EventArgs e)
        {
            oWorkstation.DeleteComponent(intID);
            Response.Redirect(Request.Path);
        }
    }
}
