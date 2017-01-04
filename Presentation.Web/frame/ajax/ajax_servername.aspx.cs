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
    public partial class ajax_servername : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected string strResponse = "";
        protected int intCount = 0;
        protected ServerName oServerName;
        protected Classes oClass;
        protected bool boolFound = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            oServerName = new ServerName(0, dsn);
            oClass = new Classes(0, dsn);
            if (Request.QueryString["u"] != null && Request.QueryString["u"] == "GET")
            {
                XmlDocument oDoc = new XmlDocument();
                oDoc.Load(Request.InputStream);
                strResponse += "<values>";
                string strName = Server.UrlDecode(oDoc.ChildNodes[0].ChildNodes[0].InnerText);
                int intClass = Int32.Parse(Server.UrlDecode(oDoc.ChildNodes[0].ChildNodes[1].InnerText));
                int intName = 0;
                if (intClass == 0)
                {
                    intName = oServerName.GetNameFactory(strName);
                    if (intName == 0)
                        intName = oServerName.GetName(strName);
                }
                else
                {
                    if (oClass.Get(intClass, "pnc") == "1")
                        intName = oServerName.GetNameFactory(strName);
                    else
                        intName = oServerName.GetName(strName);
                }
                strResponse += "</values>";
                Response.ContentType = "text/xml";
                Response.Write("<values><value>" + intName.ToString() + "</value></values>");
                Response.End();
            }
        }
    }
}
