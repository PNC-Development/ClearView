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
using System.Net.Mail;

namespace NCC.ClearView.Presentation.Web
{
    public partial class email : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);

        protected string strTitle = ConfigurationManager.AppSettings["appTitle"];
        private Variables oVariable;
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = strTitle;
            lblTitle.Text = "ClearView Email Page";
            oVariable = new Variables(intEnvironment);
            if (!IsPostBack)
            {
                Users oUser = new Users(0, dsn);
                string strUser = Request.ServerVariables["LOGON_USER"];
                strUser = strUser.Substring(strUser.LastIndexOf("\\") + 1);
                if (strUser.Trim().ToUpper() == "XSXH33T" || strUser.Trim().ToUpper() == "PT43054")
                    btnSend.Enabled = true;
                else
                {
                    btnSend.Enabled = false;
                    lblError.Text += " [" + strUser.Trim().ToUpper() + "]";
                    lblError.Visible = true;
                }
                txtSMTP.Text = oVariable.SmtpServer();
                btnSend.Attributes.Add("onclick", "return ValidateText('" + txtFrom.ClientID + "','Please enter a FROM LAN ID') && ValidateText('" + txtTo.ClientID + "','Please enter a TO LAN ID') && ValidateText('" + txtSubject.ClientID + "','Please enter a subject') && ValidateText('" + txtMessage.ClientID + "','Please enter a message');");
                btnFinish.Attributes.Add("onclick", "return CloseWindow();");
            }
            if (Request.QueryString["sent"] != null)
                panFinish.Visible = true;
            else
                panForm.Visible = true;
        }
        public void btnSend_Click(Object Sender, EventArgs e)
        {
            Users oUser = new Users(0, dsn);
            string strFrom = txtFrom.Text;
            string strTo = txtTo.Text;
            if (chkSettings.Checked)
            {
                Functions oFunction = new Functions(0, dsn, intEnvironment);
                oFunction.SendEmail("Test Email", strTo, "", "", txtSubject.Text, txtMessage.Text, false, !chkHTML.Checked);
            }
            else
            {
                if (chkFrom.Checked == true)
                    strFrom = oUser.GetEmail(strFrom, intEnvironment);
                if (chkTo.Checked == true)
                    strTo = oUser.GetEmail(strTo, intEnvironment);
                MailMessage oMessage = new MailMessage(strFrom, strTo, txtSubject.Text, txtMessage.Text);
                if (chkHTML.Checked == true)
                    oMessage.IsBodyHtml = true;
                SmtpClient oClient = new SmtpClient(txtSMTP.Text);
                oClient.Send(oMessage);
            }
            Response.Redirect(Request.Path + "?sent=true");
        }

        protected void chkSettings_CheckedChanged(object sender, EventArgs e)
        {
            trSettings.Visible = !chkSettings.Checked;
        }
    }
}
