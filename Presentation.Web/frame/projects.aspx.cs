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
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using NCC.ClearView.Application.Core;

namespace NCC.ClearView.Presentation.Web
{
    public partial class projects : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["u"] != null && Request.QueryString["u"] == "GET")
            {
                Projects oProject = new Projects(0, dsn);
                XmlDocument oDoc = new XmlDocument();
                oDoc.Load(Request.InputStream);
                DataSet ds = oProject.GetAll(Server.UrlDecode(oDoc.FirstChild.InnerXml));
                Response.ContentType = "application/xml";
                Response.Write("<values>");
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string strNumber = dr["number"].ToString();
                    if (strNumber != "")
                        strNumber = " (" + strNumber + ")";
                    Response.Write("<value>" + dr["projectid"].ToString() + "</value><text>" + dr["name"].ToString() + strNumber + "</text>");
                }
                Response.Write("</values>");
                Response.End();
            }
        }
    }
}
