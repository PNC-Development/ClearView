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
using System.DirectoryServices;
using NCC.ClearView.Application.Core;

namespace NCC.ClearView.Presentation.Web.DEV
{
    public partial class eDirectory : System.Web.UI.Page
    {
        private int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private Variables oVariable;
        private Functions oFunction;

        protected void Page_Load(object sender, EventArgs e)
        {
            oVariable = new Variables(intEnvironment);
            oFunction = new Functions(0, dsn, intEnvironment);
        }
        protected void btnGo_Click(object sender, EventArgs e)
        {
            string LDAP = "LDAP://" + oVariable.eDirectoryHost() + ":636/ou=People,o=pnc";
            Response.Write(LDAP + "...<br/>");
            string User = oVariable.eDirectoryUsername();
            Response.Write(User + "...<br/>");
            AuthenticationTypes[] types = (AuthenticationTypes[])Enum.GetValues(typeof(AuthenticationTypes));
            foreach (AuthenticationTypes type in types)
            {
                Response.Write(type.ToString() + "...");
                try
                {
                    DirectorySearcher oSearcher = new DirectorySearcher(new DirectoryEntry(LDAP, oVariable.eDirectoryUsername(), oVariable.eDirectoryPassword(), type), "(cn=pt43054)");
                    oSearcher.FindAll();
                    Response.Write("OK!");
                }
                catch (Exception ex)
                {
                    Response.Write(ex.Message);
                    if (ex.InnerException != null)
                        Response.Write(" (" + ex.InnerException.Message + ")");
                }
                Response.Write("<br/>");
            }

            LDAP = "LDAP://" + oVariable.eDirectoryHost() + ":389/ou=People,o=pnc";
            Response.Write(LDAP + "...<br/>");
            Response.Write(User + "...<br/>");
            foreach (AuthenticationTypes type in types)
            {
                Response.Write(type.ToString() + "...");
                try
                {
                    DirectorySearcher oSearcher = new DirectorySearcher(new DirectoryEntry(LDAP, oVariable.eDirectoryUsername(), oVariable.eDirectoryPassword(), type), "(cn=pt43054)");
                    oSearcher.FindAll();
                    Response.Write("OK!");
                }
                catch (Exception ex)
                {
                    Response.Write(ex.Message);
                    if (ex.InnerException != null)
                        Response.Write(" (" + ex.InnerException.Message + ")");
                }
                Response.Write("<br/>");
            }
        }

    }
}
