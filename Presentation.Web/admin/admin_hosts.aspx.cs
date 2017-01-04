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
    public partial class admin_hosts : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;

        protected Host oHost;
        protected ModelsProperties oModelsProperties;
        protected Platforms oPlatform;
        protected int intProfile;
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cookies["loginreferrer"].Value = "/admin/admin_hosts.aspx";
            Response.Cookies["loginreferrer"].Expires = DateTime.Now.AddDays(30);
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Response.Redirect("/admin/login.aspx");
            oHost = new Host(intProfile, dsn);
            oModelsProperties = new ModelsProperties(intProfile, dsn);
            oPlatform = new Platforms(intProfile, dsn);
            if (!IsPostBack)
            {
                LoopRepeater();
                btnOrder.Attributes.Add("onclick", "return OpenWindow('SUPPORTORDER','" + hdnId.ClientID + "','" + hdnOrder.ClientID + "&type=HOST" + "',false,400,400);");
                btnBrowse.Attributes.Add("onclick", "return OpenWindow('FILEBROWSER','" + txtPath.ClientID + "','',false,400,600);");
                btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this item?');");
                btnCancel.Attributes.Add("onclick", "return Cancel();");
                btnParent.Attributes.Add("onclick", "return OpenWindow('SUBMODELBROWSER','" + hdnParent.ClientID + "','&control=" + hdnParent.ClientID + "&controltext=" + lblParent.ClientID + "',false,400,600);");
                btnPlatform.Attributes.Add("onclick", "return OpenWindow('PLATFORMBROWSER','" + hdnPlatform.ClientID + "','&control=" + hdnPlatform.ClientID + "&controltext=" + lblPlatform.ClientID + "',false,400,600);");
            }
        }
        private void LoopRepeater()
        {
            DataSet ds = oHost.Gets(0);
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
        protected void btnAdd_Click(Object Sender, EventArgs e)
        {
            int intModel = Int32.Parse(Request.Form[hdnParent.UniqueID]);
            int intPlatform = Int32.Parse(Request.Form[hdnPlatform.UniqueID]);
            if (Request.Form[hdnId.UniqueID] == "0")
                oHost.Add(txtName.Text, intModel, intPlatform, txtPath.Text, txtPrefix.Text, (chkStorage.Checked ? 1 : 0), (oHost.Gets(0).Tables[0].Rows.Count + 1), (chkEnabled.Checked ? 1 : 0));
            else
                oHost.Update(Int32.Parse(Request.Form[hdnId.UniqueID]), txtName.Text, intModel, intPlatform, txtPath.Text, txtPrefix.Text, (chkStorage.Checked ? 1 : 0), (chkEnabled.Checked ? 1 : 0));
            if (Request.Form[hdnOrder.UniqueID] != "")
            {
                string strOrder = Request.Form[hdnOrder.UniqueID];
                int intCount = 0;
                while (strOrder != "")
                {
                    intCount++;
                    int intId = Int32.Parse(strOrder.Substring(0, strOrder.IndexOf("&")));
                    strOrder = strOrder.Substring(strOrder.IndexOf("&") + 1);
                    oHost.UpdateOrder(intId, intCount);
                }
            }
            Response.Redirect(Request.Path);
        }
        protected void btnEnable_Click(Object Sender, ImageClickEventArgs e)
        {
            ImageButton oButton = (ImageButton)Sender;
            oHost.Enable(Int32.Parse(oButton.CommandArgument), (oButton.ImageUrl == "/admin/images/enabled.gif" ? 0 : 1));
            Response.Redirect(Request.Path);
        }
        protected void btnDeleteLink_Click(Object Sender, ImageClickEventArgs e)
        {
            ImageButton oButton = (ImageButton)Sender;
            oHost.Delete(Int32.Parse(oButton.CommandArgument));
            Response.Redirect(Request.Path);
        }
        protected void btnDelete_Click(Object Sender, EventArgs e)
        {
            oHost.Delete(Int32.Parse(Request.Form[hdnId.UniqueID]));
            Response.Redirect(Request.Path);
        }
    }
}
