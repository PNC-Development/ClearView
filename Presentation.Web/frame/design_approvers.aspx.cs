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
using System.DirectoryServices;

namespace NCC.ClearView.Presentation.Web
{
    public partial class design_approvers : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intProfile;
        protected int intAnswer;

        protected void Page_Load(object sender, EventArgs e)
        {
            Int32.TryParse(Request.Cookies["profileid"].Value, out intProfile);
            Int32.TryParse(Request.QueryString["id"], out intAnswer);
            if (!IsPostBack)
            {
                lblApprovers.Visible = (rptApprovers.Items.Count == 0);
            }

            Variables oVariable = new Variables(intEnvironment);
            txtUser.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'300','195','" + divUser.ClientID + "','" + lstUser.ClientID + "','" + hdnUser.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
            lstUser.Attributes.Add("ondblclick", "AJAXClickRow();");

            btnAdd.Attributes.Add("onclick", "return ValidateHidden0('" + hdnUser.ClientID + "','" + txtUser.ClientID + "','Please enter the LAN ID of your approver');");
            if (rptApprovers.Items.Count > 0)
                btnSubmit.Attributes.Add("onclick", "return ValidateRadioButtons('" + radAny.ClientID + "','" + radAll.ClientID + "','Please select how you want the approval process to work');");
            else
                btnSubmit.Attributes.Add("onclick", "alert('Please enter at least one approver');return false;");
        }
        protected void btnAdd_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path + "?id=" + intAnswer.ToString());
        }
        protected void btnSubmit_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path + "?id=" + intAnswer.ToString());
        }
        protected void btnDelete_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path + "?id=" + intAnswer.ToString());
        }
    }
}
