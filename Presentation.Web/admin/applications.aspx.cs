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
    public partial class applications : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected Applications oApplication;
        protected Users oUser;
        protected int intProfile;
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cookies["loginreferrer"].Value = "/admin/applications.aspx";
            Response.Cookies["loginreferrer"].Expires = DateTime.Now.AddDays(30);
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Response.Redirect("/admin/login.aspx");
            oApplication = new Applications(intProfile, dsn);
            oUser = new Users(intProfile, dsn);
            if (!IsPostBack)
            {
                LoopRepeater();
                btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this application?');");
                btnCancel.Attributes.Add("onclick", "return Cancel();");
                btnParent.Attributes.Add("onclick", "return OpenWindow('APPLICATIONBROWSER','" + hdnParent.ClientID + "','&control=" + hdnParent.ClientID + "&controltext=" + lblParent.ClientID + "',false,400,600);");
                btnDoc.Attributes.Add("onclick", "return OpenWindow('FILEBROWSER','" + txtDoc.ClientID + "','',false,400,600);");
                btnImage.Attributes.Add("onclick", "return OpenWindow('IMAGEPATH','','" + txtImage.ClientID + "',false,500,550);");
                btnReportGroups.Attributes.Add("onclick", "return OpenWindow('REPORTGROUPS','" + hdnId.ClientID + "','',false,'500',300);");
            }
            Variables oVariable = new Variables(intEnvironment);
            txtUser.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'300','195','" + divAJAX.ClientID + "','" + lstAJAX.ClientID + "','" + hdnUser.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
            lstAJAX.Attributes.Add("ondblclick", "AJAXClickRow();");
        }
        private void LoopRepeater()
        {
            DataSet ds = oApplication.Gets(0);
            DataView dv = ds.Tables[0].DefaultView;
            if (Request.QueryString["sort"] != null)
                dv.Sort = Request.QueryString["sort"].ToString();
            rptView.DataSource = dv;
            rptView.DataBind();
            foreach (RepeaterItem ri in rptView.Items)
            {
                ImageButton oDelete = (ImageButton)ri.FindControl("btnDelete");
                oDelete.Attributes.Add("onClick", "return confirm('Are you sure you want to delete this application?');");
                ImageButton oEnable = (ImageButton)ri.FindControl("btnEnable");
                if (oEnable.ImageUrl == "images/enabled.gif")
                {
                    oEnable.ToolTip = "Click to disable";
                    oEnable.Attributes.Add("onClick", "return confirm('Are you sure you want to disable this application?');");
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
                oApplication.Add(txtName.Text, txtUrl.Text, txtTitle.Text, txtOrg.Text, txtDescription.Text, txtImage.Text, Int32.Parse(Request.Form[hdnUser.UniqueID]), Int32.Parse(Request.Form[hdnParent.UniqueID]), Int32.Parse(txtPriority1.Text), Int32.Parse(txtPriority2.Text), (chkTPM.Checked ? 1 : 0), (chkDisable.Checked ? 1 : 0), (chkManager.Checked == true ? 1 : 0), (chkPlatform.Checked == true ? 1 : 0), txtDoc.Text, Int32.Parse(txtLead1.Text), Int32.Parse(txtLead2.Text), Int32.Parse(txtLead3.Text), Int32.Parse(txtLead4.Text), Int32.Parse(txtLead5.Text), (chkApproval.Checked ? 1 : 0), Int32.Parse(txtEmployees.Text), (chkServiceSearch.Checked ? 1 : 0), (chkReminders.Checked ? 1 : 0), (chkRequestItems.Checked ? 1 : 0), (chkDecom.Checked ? 1 : 0), (chkAdmin.Checked ? 1 : 0), (chkDNS.Checked ? 1 : 0), (chkEnabled.Checked == true ? 1 : 0));
            else
                oApplication.Update(Int32.Parse(Request.Form[hdnId.UniqueID]), txtName.Text, txtUrl.Text, txtTitle.Text, txtOrg.Text, txtDescription.Text, txtImage.Text, Int32.Parse(Request.Form[hdnUser.UniqueID]), Int32.Parse(Request.Form[hdnParent.UniqueID]), Int32.Parse(txtPriority1.Text), Int32.Parse(txtPriority2.Text), (chkTPM.Checked ? 1 : 0), (chkDisable.Checked ? 1 : 0), (chkManager.Checked == true ? 1 : 0), (chkPlatform.Checked == true ? 1 : 0), txtDoc.Text, Int32.Parse(txtLead1.Text), Int32.Parse(txtLead2.Text), Int32.Parse(txtLead3.Text), Int32.Parse(txtLead4.Text), Int32.Parse(txtLead5.Text), (chkApproval.Checked ? 1 : 0), Int32.Parse(txtEmployees.Text), (chkServiceSearch.Checked ? 1 : 0), (chkReminders.Checked ? 1 : 0), (chkRequestItems.Checked ? 1 : 0), (chkDecom.Checked ? 1 : 0), (chkAdmin.Checked ? 1 : 0), (chkDNS.Checked ? 1 : 0), (chkEnabled.Checked == true ? 1 : 0));
            Response.Redirect(Request.Path);
        }
        protected void btnEnable_Click(Object Sender, ImageClickEventArgs e)
        {
            ImageButton oButton = (ImageButton)Sender;
            oApplication.Enable(Int32.Parse(oButton.CommandArgument), (oButton.ImageUrl == "/admin/images/enabled.gif" ? 0 : 1));
            Response.Redirect(Request.Path);
        }
        protected void btnDeleteLink_Click(Object Sender, ImageClickEventArgs e)
        {
            ImageButton oButton = (ImageButton)Sender;
            oApplication.Delete(Int32.Parse(oButton.CommandArgument));
            Response.Redirect(Request.Path);
        }
        protected void btnDelete_Click(Object Sender, EventArgs e)
        {
            oApplication.Delete(Int32.Parse(Request.Form[hdnId.UniqueID]));
            Response.Redirect(Request.Path);
        }
    }
}
