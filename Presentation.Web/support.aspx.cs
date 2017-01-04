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
    public partial class support1 : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        
        protected string strTitle = ConfigurationManager.AppSettings["appTitle"];
        protected string strTo = "xsxh33t";
        protected string strCC = "xjjc335";
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = strTitle;
            lblTitle.Text = "ClearView Support Page";
            if (!IsPostBack)
            {
                Users oUser = new Users(0, dsn);
                string strUser = Request.ServerVariables["LOGON_USER"];
                strUser = strUser.Substring(strUser.LastIndexOf("\\") + 1);
                try
                {
                    if (strUser != "")
                    {
                        int intUser = oUser.GetId(strUser);
                        if (intUser > 0)
                        {
                            txtID.Text = oUser.GetName(intUser);
                            txtName.Text = oUser.GetFullName(intUser);
                        }
                    }
                }
                catch
                {
                }
                btnSend.Attributes.Add("onclick", "return ValidateText('" + txtID.ClientID + "','Please enter your LAN ID') && ValidateText('" + txtName.ClientID + "','Please enter your name') && ValidateText('" + txtEmail.ClientID + "','Please enter your email address') && ValidateText('" + txtSubject.ClientID + "','Please enter a subject') && ValidateText('" + txtMessage.ClientID + "','Please enter a message');");
                btnFinish.Attributes.Add("onclick", "return CloseWindow();");
            }
            if (Request.QueryString["sent"] != null)
                panFinish.Visible = true;
            else
                panForm.Visible = true;
            Control oControl = (Control)LoadControl("/controls/sys/sys_rotator_header.ascx");
            PH4.Controls.Add(oControl);
        }
        protected void btnSend_Click(Object Sender, EventArgs e)
        {
            Functions oFunction = new Functions(0, dsn, intEnvironment);
            string strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_WARNING");
            oFunction.SendEmail("ClearView Support", strTo, strCC, strEMailIdsBCC, "CLEARVIEW SUPPORT ISSUE from " + txtName.Text + " (" + txtID.Text + ") at " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString(), "<p><b>" + txtSubject.Text + "</b></p><p>" + txtMessage.Text + "</p>", true, false);
            Response.Redirect(Request.Path + "?sent=true");
        }
    }
}
