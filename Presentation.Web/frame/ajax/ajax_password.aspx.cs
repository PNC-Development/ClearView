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
using System.Xml;
using NCC.ClearView.Application.Core;

namespace NCC.ClearView.Presentation.Web
{
    public partial class ajax_password : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["u"] != null)
            {
                string strPassword = Request.QueryString["u"];
                Functions oFunction = new Functions(0, dsn, intEnvironment);
                string strNewPassword = oFunction.GetPassword(strPassword);
                Response.ContentType = "application/xml";
                Response.Write("<values><value>" + (strNewPassword == "" ? strPassword : strNewPassword) + "</value></values>");
                Response.End();
            }
        }
    }
}
