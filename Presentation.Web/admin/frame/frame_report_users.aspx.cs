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
    public partial class frame_report_users : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intProfile;
        protected Reports oReport;
        protected Users oUser;
        private DataSet ds;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Reload();
            oReport = new Reports(intProfile, dsn);
            oUser = new Users(intProfile, dsn);
            lblFinish.Visible = false;
            if (Request.QueryString["save"] != null && Request.QueryString["save"] != "")
                lblFinish.Visible = true;
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
            {
                if (!IsPostBack)
                {
                    lblId.Text = Request.QueryString["id"];
                    int intReport = Int32.Parse(lblId.Text);
                    lblName.Text = oReport.GetName(intReport);
                    LoadLists();
                    btnClose.Attributes.Add("onclick", "return HidePanel();");
                }
                Variables oVariable = new Variables(intEnvironment);
                txtUser.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'300','195','" + divAJAX.ClientID + "','" + lstAJAX.ClientID + "','" + hdnUser.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
                lstAJAX.Attributes.Add("ondblclick", "AJAXClickRow();");
            }
        }
        private void LoadLists()
        {
            int intReport = Int32.Parse(lblId.Text);
            ds = oReport.GetUsers(intReport);
            string strId = "0";
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (strId != dr["userid"].ToString())
                {
                    strId = dr["userid"].ToString();
                    lstCurrent.Items.Add(new ListItem(dr["username"].ToString(), dr["userid"].ToString()));
                }
            }
        }
        protected  void btnAdd_Click(Object Sender, EventArgs e)
        {
            int intReport = Int32.Parse(lblId.Text);
            oReport.AddUser(intReport, Int32.Parse(Request.Form[hdnUser.UniqueID]));
            Response.Redirect(Request.Path + "?id=" + intReport.ToString() + "&save=true");
        }
        protected void btnRemove_Click(Object Sender, EventArgs e)
        {
            int intReport = Int32.Parse(lblId.Text);
            if (lstCurrent.SelectedIndex > -1)
            {
                oReport.DeleteUser(intReport, Int32.Parse(lstCurrent.SelectedItem.Value));
                Response.Redirect(Request.Path + "?id=" + intReport.ToString() + "&save=true");
            }
            else
                Response.Redirect(Request.Path + "?id=" + intReport.ToString());
        }
        private void Reload()
        {
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "reload", "<script type=\"text/javascript\">window.top.location.reload();<" + "/" + "script>");
        }
    }
}
