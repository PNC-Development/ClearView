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
    public partial class sys_will_down : System.Web.UI.UserControl
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected Applications oApplication;
        protected Pages oPage;
        protected Users oUser;
        protected Settings oSetting;
        protected Variables oVariable;
        protected int intProfile = 0;
        protected int intPage = 0;
        protected int intApplication = 0;
        protected string strError = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            oApplication = new Applications(0, dsn);
            oPage = new Pages(0, dsn);
            oUser = new Users(0, dsn);
            oSetting = new Settings(0, dsn);
            oVariable = new Variables(intEnvironment);
            if (Request.Cookies["profileid"] != null && Request.Cookies["profileid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            string strDown = "";
            try
            {
                strDown = oSetting.Get("down");
                strError = oSetting.Get("error");
            }
            catch (Exception ex)
            {
                Response.Cookies["app_error"].Value = ex.Message;
                Response.Redirect("/error.aspx");
            }
            if (strDown == "0")
            {
                panDown.Visible = true;
                string strDate = oSetting.Get("maintenance");
                if (strDate == "")
                    Response.Redirect("/index.aspx");
                else
                {
                    try
                    {
                        DateTime _date = DateTime.Parse(strDate);
                        lblDate.Text = _date.ToLongDateString();
                        lblTime.Text = _date.ToLongTimeString();
                    }
                    catch { }
                }
            }
            if (oUser.IsAdmin(intProfile) == true && oApplication.IsAdmin(intApplication) == true && strError != "")
                panError.Visible = true;
        }

        protected void btnError_Click(Object Sender, EventArgs e)
        {
            oSetting.UpdateError("");
            string strRedirect = "";
            foreach (string strQuery in Request.QueryString.AllKeys)
            {
                if (strQuery != "pageid")
                {
                    strRedirect += (strRedirect == "" ? "?" : "&");
                    strRedirect += strQuery;
                    strRedirect += "=";
                    strRedirect += Request.QueryString[strQuery];
                }
            }
            Response.Redirect(oPage.GetFullLink(intPage) + strRedirect);
        }
    }
}