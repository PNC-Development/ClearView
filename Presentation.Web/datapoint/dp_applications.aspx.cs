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
    public partial class dp_applications : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intProfile = 0;
        protected DataPoint oDataPoint;
        protected Users oUser;
        protected string strKey = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            if (Request.Cookies["profileid"] != null && Request.Cookies["profileid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oUser = new Users(intProfile, dsn);
            oDataPoint = new DataPoint(intProfile, dsn);
            if (oUser.IsAdmin(intProfile) == true && Request.QueryString["key"] != null)
            {
                strKey = Request.QueryString["key"];
                if (!IsPostBack)
                    LoadList();
            }
            else
            {
                btnAdd.Enabled = false;
                btnRemove.Enabled = false;
            }
        }
        private void LoadList()
        {
            Applications oApplication = new Applications(intProfile, dsn);
            ddlApplications.DataValueField = "applicationid";
            ddlApplications.DataTextField = "name";
            ddlApplications.DataSource = oApplication.Gets(1);
            ddlApplications.DataBind();
            ddlApplications.Items.Insert(0, new ListItem("-- SELECT --", "0"));
            lstCurrent.DataValueField = "id";
            lstCurrent.DataTextField = "name";
            lstCurrent.DataSource = oDataPoint.GetPagePermission(strKey);
            lstCurrent.DataBind();
            for (int ii = 0; ii < lstCurrent.Items.Count; ii++)
            {
                for (int jj = 0; jj < ddlApplications.Items.Count; jj++)
                {
                    if (lstCurrent.Items[ii].Text == ddlApplications.Items[jj].Text)
                    {
                        ddlApplications.Items.Remove(ddlApplications.Items[jj]);
                        jj--;
                    }
                }
            }
        }
        protected void btnAdd_Click(Object Sender, EventArgs e)
        {
            oDataPoint.AddPagePermission(Int32.Parse(ddlApplications.SelectedItem.Value), strKey);
            Response.Redirect(Request.Path + "?key=" + strKey + "&save=true");
        }
        protected void btnRemove_Click(Object Sender, EventArgs e)
        {
            if (lstCurrent.SelectedIndex > -1)
            {
                oDataPoint.DeletePagePermission(Int32.Parse(lstCurrent.Items[lstCurrent.SelectedIndex].Value));
                Response.Redirect(Request.Path + "?key=" + strKey + "&delete=true");
            }
            else
                Response.Redirect(Request.Path + "?key=" + strKey);
        }
    }
}
