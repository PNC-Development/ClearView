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
    public partial class ad_server : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        
        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            AD oAD = new AD(0, dsn, intEnvironment);
            string strName = "";
            if (Request.QueryString["c"] != null)
            {
                SearchResultCollection oResults = oAD.ComputerSearch(Request.QueryString["c"]);
                if (oResults.Count == 0)
                    Response.Write("NONE");
                else
                {
                    if (oResults.Count == 1)
                    {
                        if (oResults[0].Properties.Contains("cn") == true)
                            strName = oResults[0].GetDirectoryEntry().Properties["cn"].Value.ToString();
                        Response.Write(strName + "_" + strName + "_" + strName);
                    }
                    else
                        Response.Write("MULTIPLE");
                }
            }
            else
                Response.Write("");
            Response.End();
        }
    }
}
