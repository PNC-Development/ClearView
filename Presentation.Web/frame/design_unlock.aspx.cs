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
using System.Text;

namespace NCC.ClearView.Presentation.Web
{
    public partial class design_unlock : System.Web.UI.Page
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intProfile;
        protected Design oDesign;
        protected Users oUser;
        protected Functions oFunction;
        protected int intID;
        protected bool boolWindows = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oDesign = new Design(intProfile, dsn);
            oUser = new Users(intProfile, dsn);
            oFunction = new Functions(intProfile, dsn, intEnvironment);

            if (Request.QueryString["id"] != null)
                Int32.TryParse(Request.QueryString["id"], out intID);
            if (Request.QueryString["unlock"] != null)
                Page.ClientScript.RegisterStartupScript(typeof(Page), "unlock", "<script type=\"text/javascript\">alert('Invalid Unlock Code\\n\\nPlease contact your ClearView administrator to acquire a valid code');<" + "/" + "script>");
            if (Request.QueryString["unlocked"] != null)
                Page.ClientScript.RegisterStartupScript(typeof(Page), "unlocked", "<script type=\"text/javascript\">window.parent.navigate(window.parent.location);window.parent.HidePanel();<" + "/" + "script>");
            
            if (intID > 0)
            {
                lblConfidenceUnlock.Text = Request.ServerVariables["REMOTE_HOST"];
                btnConfidenceUnlock.Attributes.Add("onclick", "return ValidateText('" + txtConfidenceUnlock.ClientID + "','Please enter an unlock code') && ValidateText('" + txtConfidenceReason.ClientID + "','Please enter a reason for unlocking this design');");
            }
        }
        protected void btnConfidenceUnlock_Click(Object Sender, EventArgs e)
        {
            Encryption oEncryption = new Encryption();

            string strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DESIGN_BUILDER");

            string strDate = DateTime.Today.Month.ToString() + DateTime.Today.Day.ToString() + DateTime.Today.Year.ToString();
            if (txtConfidenceUnlock.Text == oEncryption.Encrypt(lblConfidenceUnlock.Text + strDate, "UNLOCK_100"))
            {
                oDesign.Unlock(intID);
                Response.Redirect(Request.Path + "?id=" + intID.ToString() + "&unlocked=true");
            }
            else
                Response.Redirect(Request.Path + "?id=" + intID.ToString() + "&unlock=true");
        }
    }
}
