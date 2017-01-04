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
using System.Xml;
using System.DirectoryServices;

namespace NCC.ClearView.Presentation.Web
{
    public partial class ajax_accenture : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected string strResponse = "";
        protected int intCount = 0;
        protected DataTable oTable;
        protected AD oAD;
        protected Users oUser;
        protected bool boolFound = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            oAD = new AD(0, dsn, intEnvironment);
            oUser = new Users(0, dsn);
            if (Request.QueryString["u"] != null && Request.QueryString["u"] == "GET")
            {
                XmlDocument oDoc = new XmlDocument();
                oDoc.Load(Request.InputStream);
                strResponse += "<values>";
                int intUser = Int32.Parse(Server.UrlDecode(oDoc.ChildNodes[0].ChildNodes[0].InnerText));
                string strUser = oUser.GetName(intUser);
                bool boolAccenture = IsAccenture(strUser);
                string strUsers = Server.UrlDecode(oDoc.ChildNodes[0].ChildNodes[1].InnerText);
                bool boolAccentureUser = boolAccenture;
                while (strUsers != "" && boolAccentureUser == boolAccenture)
                {
                    string strCheck = strUsers.Substring(0, strUsers.IndexOf(";"));
                    boolAccentureUser = IsAccenture(strCheck);
                    strUsers = strUsers.Substring(strUsers.IndexOf(";") + 1);
                }
                strResponse += "</values>";
                Response.ContentType = "text/xml";
                Response.Write("<values><value>" + (boolAccentureUser == boolAccenture ? "1" : "0") + "</value></values>");
                Response.End();
            }
        }
        private bool IsAccenture(string strUser)
        {
            string strDisplay = GetDisplay(strUser);
            bool boolAccenture = (strDisplay.Contains("(ACN)") || strDisplay.Contains("(Accenture)"));
            return boolAccenture;
        }
        private string GetDisplay(string strUser)
        {
            string strDisplay = "";
            DirectoryEntry oEntry = oAD.UserSearch(strUser);
            if (oEntry != null)
            {
                if (oEntry.Properties.Contains("displayname") == true)
                    strDisplay = oEntry.Properties["displayname"].Value.ToString();
            }
            return strDisplay;
        }
    }
}
