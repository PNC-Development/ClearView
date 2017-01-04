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
    public partial class storage_override_code : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);

        protected int intID = 0;
        protected int intProfile = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                intID = Int32.Parse(Request.QueryString["id"]);

            lblStorageOverrideWorkStation.Text = Request.ServerVariables["REMOTE_HOST"];

            if (!Page.IsPostBack)
            {
                hdnOverrideConfirmed.Value = "false";
            }

            btnStorageOverrideUnlock.Attributes.Add("onclick", "return ValidateText('" + txtStorageOverrideUnlock.ClientID + "','Please enter an unlock code.');");
        }
        protected void btnStorageOverrideUnlock_Click(object sender, EventArgs e)
        {
            Encryption oEncryption = new Encryption();
            Users oUser = new Users(0, dsn);
            Functions oFunction = new Functions(0, dsn, intEnvironment);
            Forecast oForecast = new Forecast(0, dsn);
            string strDate = DateTime.Today.Month.ToString() + DateTime.Today.Day.ToString() + DateTime.Today.Year.ToString();

            // Get a ClientScriptManager reference from the Page class.
            ClientScriptManager cs = Page.ClientScript;
            Type cstype = this.GetType();
            string csname = "ButtonClickScript";


            if (hdnOverrideConfirmed.Value == "false")
            {
                if (txtStorageOverrideUnlock.Text == oEncryption.Encrypt(lblStorageOverrideWorkStation.Text + strDate, "STORAGEOVERRIDE_1000"))
                //if (txtStorageOverrideUnlock.Text == "1234")
                {
                    // Check to see if the client script is already registered.
                    if (!cs.IsStartupScriptRegistered(cstype, csname))
                    {
                        StringBuilder cstext = new StringBuilder();
                        cstext.Append("<script defer type=text/javascript>");
                        cstext.Append(@"
								function confirmDelete() {				
								if (confirm('The current storage configuration for this design will need to be deleted to access storage override administration. \n\n Are you sure you want to continue ?'))
								{
									document.getElementById('" + hdnOverrideConfirmed.ClientID + @"').value = 'true';
									document.getElementById('" + btnStorageOverrideUnlock.ClientID + @"').click(); // submit the form with a click on the delete button
								} 
							}
							confirmDelete();");
                        cstext.Append("<" + "/script>");
                        cs.RegisterStartupScript(cstype, csname, cstext.ToString(), false);
                    }
                }
                else
                {
                    //Unlock code is invalid
                    //Response.Redirect(Request.Path + "?id=" + intID.ToString() + "&unlock=invalid");
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "unlock", "<script type=\"text/javascript\">alert('Invalid Override Code.\\n\\nPlease contact your ClearView administrator to acquire a valid code.');<" + "/" + "script>");
                }
            }
            else
            {
                //Unlock code is valid and got the user confirmation.
                oForecast.UpdateAnswerStorageOverride(intID, 1, intProfile);
                oForecast.DeleteStorageForOverride(intID);
                //Response.Redirect(Request.Path + "?id=" + intID.ToString() + "&unlock=valid");
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "unlock", "<script type=\"text/javascript\">window.parent.navigate(window.parent.location);<" + "/" + "script>");
            }

        }
    }
}
