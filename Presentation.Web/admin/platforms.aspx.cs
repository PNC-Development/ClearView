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
    public partial class platforms : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected Platforms oPlatform;
        protected Users oUser;
        protected ModelsProperties oModelsProperties;
        protected int intProfile;
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cookies["loginreferrer"].Value = "/admin/platforms.aspx";
            Response.Cookies["loginreferrer"].Expires = DateTime.Now.AddDays(30);
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Response.Redirect("/admin/login.aspx");
            oPlatform = new Platforms(intProfile, dsn);
            oUser = new Users(intProfile, dsn);
            oModelsProperties = new ModelsProperties(intProfile, dsn);
            if (!IsPostBack)
            {
                LoopRepeater();
                btnImage.Attributes.Add("onclick", "return OpenWindow('IMAGEPATH','','" + txtImage.ClientID + "',false,500,550);");
                btnDisplay.Attributes.Add("onclick", "return OpenWindow('SUPPORTORDER','" + hdnId.ClientID + "','" + hdnOrder.ClientID + "&type=PLAT" + "',false,400,400);");
                btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this platform?');");
                btnCancel.Attributes.Add("onclick", "return Cancel();");
                btnAction.Attributes.Add("onclick", "return OpenWindow('FILEBROWSER','" + txtAction.ClientID + "','',false,400,600);");
                btnDemand.Attributes.Add("onclick", "return OpenWindow('FILEBROWSER','" + txtDemand.ClientID + "','',false,400,600);");
                btnSupply.Attributes.Add("onclick", "return OpenWindow('FILEBROWSER','" + txtSupply.ClientID + "','',false,400,600);");
                btnOrder.Attributes.Add("onclick", "return OpenWindow('FILEBROWSER','" + txtOrder.ClientID + "','',false,400,600);");
                btnOrderView.Attributes.Add("onclick", "return OpenWindow('FILEBROWSER','" + txtOrderView.ClientID + "','',false,400,600);");
                btnAddForm.Attributes.Add("onclick", "return OpenWindow('FILEBROWSER','" + txtAdd.ClientID + "','',false,400,600);");
                btnSettings.Attributes.Add("onclick", "return OpenWindow('FILEBROWSER','" + txtSettings.ClientID + "','',false,400,600);");
                btnForm.Attributes.Add("onclick", "return OpenWindow('FILEBROWSER','" + txtForm.ClientID + "','',false,400,600);");
                btnAlert.Attributes.Add("onclick", "return OpenWindow('FILEBROWSER','" + txtAlert.ClientID + "','',false,400,600);");
                btnUsers.Attributes.Add("onclick", "return OpenWindow('PLATFORM_USERS','" + hdnId.ClientID + "','',false,'500',500);");
            }
            Variables oVariable = new Variables(intEnvironment);
            txtUser.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'300','195','" + divAJAX.ClientID + "','" + lstAJAX.ClientID + "','" + hdnUser.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
            lstAJAX.Attributes.Add("ondblclick", "AJAXClickRow();");
            txtManager.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'300','195','" + divManager.ClientID + "','" + lstManager.ClientID + "','" + hdnManager.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
            lstManager.Attributes.Add("ondblclick", "AJAXClickRow();");
        }
        private void LoopRepeater()
        {
            DataSet ds = oPlatform.Gets(0);
            DataView dv = ds.Tables[0].DefaultView;
            if (Request.QueryString["sort"] != null)
                dv.Sort = Request.QueryString["sort"].ToString();
            rptView.DataSource = dv;
            rptView.DataBind();
            foreach (RepeaterItem ri in rptView.Items)
            {
                ImageButton oDelete = (ImageButton)ri.FindControl("btnDelete");
                oDelete.Attributes.Add("onClick", "return confirm('Are you sure you want to delete this platform?');");
                ImageButton oEnable = (ImageButton)ri.FindControl("btnEnable");
                if (oEnable.ImageUrl == "images/enabled.gif")
                {
                    oEnable.ToolTip = "Click to disable";
                    oEnable.Attributes.Add("onClick", "return confirm('Are you sure you want to disable this platform?');");
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
        protected void btnAdd_Click(Object Sender, EventArgs e)
        {
            if (Request.Form[hdnId.UniqueID] == "0")
                oPlatform.Add(txtName.Text, Int32.Parse(Request.Form[hdnUser.UniqueID]), Int32.Parse(Request.Form[hdnManager.UniqueID]), txtImage.Text, txtBigImage.Text, (chkAsset.Checked ? 1 : 0), (chkForecast.Checked ? 1 : 0), (chkSystem.Checked ? 1 : 0), (chkInventory.Checked ? 1 : 0), txtAction.Text, txtDemand.Text, txtSupply.Text, txtOrder.Text, txtOrderView.Text.Trim(), txtAdd.Text, txtSettings.Text, txtForm.Text, txtAlert.Text, Int32.Parse(txtMax1.Text), Int32.Parse(txtMax2.Text), Int32.Parse(txtMax3.Text), (chkEnabled.Checked == true ? 1 : 0));
            else
                oPlatform.Update(Int32.Parse(Request.Form[hdnId.UniqueID]), txtName.Text, Int32.Parse(Request.Form[hdnUser.UniqueID]), Int32.Parse(Request.Form[hdnManager.UniqueID]), txtImage.Text, txtBigImage.Text, (chkAsset.Checked ? 1 : 0), (chkForecast.Checked ? 1 : 0), (chkSystem.Checked ? 1 : 0), (chkInventory.Checked ? 1 : 0), txtAction.Text, txtDemand.Text, txtSupply.Text, txtOrder.Text, txtOrderView.Text.Trim(), txtAdd.Text, txtSettings.Text, txtForm.Text, txtAlert.Text, Int32.Parse(txtMax1.Text), Int32.Parse(txtMax2.Text), Int32.Parse(txtMax3.Text), (chkEnabled.Checked == true ? 1 : 0));
            if (Request.Form[hdnOrder.UniqueID] != "")
            {
                string strOrder = Request.Form[hdnOrder.UniqueID];
                int intCount = 0;
                while (strOrder != "")
                {
                    intCount++;
                    int intId = Int32.Parse(strOrder.Substring(0, strOrder.IndexOf("&")));
                    strOrder = strOrder.Substring(strOrder.IndexOf("&") + 1);
                    oPlatform.UpdateOrder(intId, intCount);
                }
            }
            Response.Redirect(Request.Path);
        }
        protected void btnEnable_Click(Object Sender, ImageClickEventArgs e)
        {
            ImageButton oButton = (ImageButton)Sender;
            oPlatform.Enable(Int32.Parse(oButton.CommandArgument), (oButton.ImageUrl == "/admin/images/enabled.gif" ? 0 : 1));
            Response.Redirect(Request.Path);
        }
        protected void btnDeleteLink_Click(Object Sender, ImageClickEventArgs e)
        {
            ImageButton oButton = (ImageButton)Sender;
            oPlatform.Delete(Int32.Parse(oButton.CommandArgument));
            Response.Redirect(Request.Path);
        }
        protected void btnDelete_Click(Object Sender, EventArgs e)
        {
            oPlatform.Delete(Int32.Parse(Request.Form[hdnId.UniqueID]));
            Response.Redirect(Request.Path);
        }
    }
}
