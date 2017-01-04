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
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;

namespace NCC.ClearView.Presentation.Web
{
    public partial class error_message : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intProfile = 0;
        protected string strError = "";

        protected void Page_Load()
        {
            Functions oFunction = new Functions(0, dsn, intEnvironment);
            Variables oVariable = new Variables(intEnvironment);
            Users oUser = new Users(0, dsn);

            if (Request.QueryString["error"] != null)
                strError = oFunction.decryptQueryString(Request.QueryString["error"]);
            if (String.IsNullOrEmpty(Request.QueryString["incident"]) == false)
            {
                string incident = Request.QueryString["incident"];
                //lblIncident.Text = lblIncident2.Text = ":&nbsp;<b>" + incident + "</b>";
                DataSet dsKey = oFunction.GetSetupValuesByKey("INCIDENTS");
                if (dsKey.Tables[0].Rows.Count > 0)
                {
                    string incidents = dsKey.Tables[0].Rows[0]["Value"].ToString();
                    StreamReader theReader = new StreamReader(incidents);
                    string theContents = theReader.ReadToEnd();
                    string[] theLines = theContents.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string theLine in theLines)
                    {
                        if (theLine.Contains(incident))
                        {
                            panIncident.Visible = true;
                            string[] theFields = theLine.Split(new char[] { ',' }, StringSplitOptions.None);
                            string person = theFields[5].Replace("\"", "");
                            if (String.IsNullOrEmpty(person))
                            {
                                string group = theFields[4].Replace("\"", "");
                                lblIncidentOwner.Text = group;
                            }
                            else
                                lblIncidentOwner.Text = person;
                            break;
                        }
                    }
                }
            }

            Int32.TryParse(Request.Cookies["profileid"].Value, out intProfile);
            trAdmin.Visible = oUser.IsAdmin(intProfile);

            hypWiki.NavigateUrl = oVariable.Community();
        }
    }
}
