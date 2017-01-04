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
    public partial class dp_fields : BasePage
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
                Variables oVariable = new Variables(intEnvironment);
                txtUser.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'400','195','" + divUser.ClientID + "','" + lstUser.ClientID + "','" + hdnUser.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
                lstUser.Attributes.Add("ondblclick", "AJAXClickRow();");
            }
            else
            {
                btnAdd.Enabled = false;
                btnRemove.Enabled = false;
            }
        }
        private void LoadList()
        {
            lstCurrent.DataValueField = "id";
            lstCurrent.DataTextField = "name";
            lstCurrent.DataSource = oDataPoint.GetFieldPermission(strKey);
            lstCurrent.DataBind();
        }
        protected void btnAdd_Click(Object Sender, EventArgs e)
        {
            if (Request.Form[hdnUser.UniqueID] != "")
            {
                oDataPoint.AddFieldPermission(Int32.Parse(Request.Form[hdnUser.UniqueID]), strKey);
                Response.Redirect(Request.Path + "?key=" + strKey + "&save=true");
            }
            else
                Response.Redirect(Request.Path + "?key=" + strKey);
        }
        protected void btnRemove_Click(Object Sender, EventArgs e)
        {
            if (lstCurrent.SelectedIndex > -1)
            {
                oDataPoint.DeleteFieldPermission(Int32.Parse(lstCurrent.Items[lstCurrent.SelectedIndex].Value));
                Response.Redirect(Request.Path + "?key=" + strKey + "&delete=true");
            }
            else
                Response.Redirect(Request.Path + "?key=" + strKey);
        }
    }
}
