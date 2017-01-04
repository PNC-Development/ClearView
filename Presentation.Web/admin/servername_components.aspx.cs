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
    public partial class servername_components : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected ServerName oServerName;
        protected int intProfile;
        protected int intID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cookies["loginreferrer"].Value = "/admin/servername_components.aspx";
            Response.Cookies["loginreferrer"].Expires = DateTime.Now.AddDays(30);
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Response.Redirect("/admin/login.aspx");
            oServerName = new ServerName(intProfile, dsn);
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
                    DataSet ds = oServerName.GetComponent(intID);
                    hdnId.Value = intID.ToString();
                    txtName.Text = ds.Tables[0].Rows[0]["name"].ToString();
                    txtCode.Text = ds.Tables[0].Rows[0]["code"].ToString();
                    txtFactoryCode.Text = ds.Tables[0].Rows[0]["factory_code"].ToString();
                    txtFactoryCodeSpecific.Text = ds.Tables[0].Rows[0]["factory_code_specific"].ToString();
                    txtZeusCode.Text = ds.Tables[0].Rows[0]["zeus_code"].ToString();
                    radIIS.Checked = (ds.Tables[0].Rows[0]["iis"].ToString() == "1");
                    radWeb.Checked = (ds.Tables[0].Rows[0]["web"].ToString() == "1");
                    radSQL.Checked = (ds.Tables[0].Rows[0]["sql"].ToString() == "1");
                    radDatabase.Checked = (ds.Tables[0].Rows[0]["dbase"].ToString() == "1");
                    chkResetSAN.Checked = (ds.Tables[0].Rows[0]["reset_storage"].ToString() == "1");
                    chkEnabled.Checked = (ds.Tables[0].Rows[0]["enabled"].ToString() == "1");
                }
            }
            btnOrder.Attributes.Add("onclick", "return OpenWindow('SUPPORTORDER','" + hdnId.ClientID + "','" + hdnOrder.ClientID + "&type=SERVERNAME_C" + "',false,400,400);");
            btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this item?');");
        }
        private void LoopRepeater()
        {
            panView.Visible = true;
            DataSet ds = oServerName.GetComponents(0);
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
                oServerName.AddComponent(txtName.Text, txtCode.Text, txtFactoryCode.Text, txtFactoryCodeSpecific.Text, txtZeusCode.Text, (radIIS.Checked ? 1 : 0), (radWeb.Checked ? 1 : 0), (radSQL.Checked ? 1 : 0), (radDatabase.Checked ? 1 : 0), (chkResetSAN.Checked ? 1 : 0), (oServerName.GetComponents(0).Tables[0].Rows.Count + 1), (chkEnabled.Checked ? 1 : 0));
            else
                oServerName.UpdateComponent(intID, txtName.Text, txtCode.Text, txtFactoryCode.Text, txtFactoryCodeSpecific.Text, txtZeusCode.Text, (radIIS.Checked ? 1 : 0), (radWeb.Checked ? 1 : 0), (radSQL.Checked ? 1 : 0), (radDatabase.Checked ? 1 : 0), (chkResetSAN.Checked ? 1 : 0), (chkEnabled.Checked ? 1 : 0));
            if (Request.Form[hdnOrder.UniqueID] != "")
            {
                string strOrder = Request.Form[hdnOrder.UniqueID];
                int intCount = 0;
                while (strOrder != "")
                {
                    intCount++;
                    int intId = Int32.Parse(strOrder.Substring(0, strOrder.IndexOf("&")));
                    strOrder = strOrder.Substring(strOrder.IndexOf("&") + 1);
                    oServerName.UpdateComponentOrder(intId, intCount);
                }
            }
            Response.Redirect(Request.Path);
        }
        protected void btnEnable_Click(Object Sender, ImageClickEventArgs e)
        {
            ImageButton oButton = (ImageButton)Sender;
            oServerName.EnableComponent(Int32.Parse(oButton.CommandArgument), (oButton.ImageUrl == "/admin/images/enabled.gif" ? 0 : 1));
            Response.Redirect(Request.Path);
        }
        protected void btnDeleteLink_Click(Object Sender, ImageClickEventArgs e)
        {
            ImageButton oButton = (ImageButton)Sender;
            oServerName.DeleteComponent(Int32.Parse(oButton.CommandArgument));
            Response.Redirect(Request.Path);
        }
        protected void btnCancel_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path);
        }
        protected void btnDelete_Click(Object Sender, EventArgs e)
        {
            oServerName.DeleteComponent(intID);
            Response.Redirect(Request.Path);
        }
    }
}
