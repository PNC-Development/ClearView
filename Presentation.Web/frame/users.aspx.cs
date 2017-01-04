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
using System.Text;

namespace NCC.ClearView.Presentation.Web
{
    public partial class users : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["u"] != null && Request.QueryString["u"] == "GET")
            {
                XmlDocument oDoc = new XmlDocument();
                oDoc.Load(Request.InputStream);
                string strUser = oDoc.ChildNodes[0].ChildNodes[0].InnerText;
                bool boolPNC = false;
                bool boolNCB = false;
                if (oDoc.ChildNodes[0].ChildNodes[1] != null)
                {
                    boolPNC = (oDoc.ChildNodes[0].ChildNodes[1].InnerText == "PNC");
                    boolNCB = (oDoc.ChildNodes[0].ChildNodes[1].InnerText == "NCB");
                }

                Response.ContentType = "application/xml";
                Response.Write("<values>");
                StringBuilder strUsers = LoadUsers(strUser, boolPNC, boolNCB);
                if (strUsers.ToString() == "")
                    strUsers = LoadUsers(Server.UrlDecode(strUser), boolPNC, boolNCB);
                Response.Write(strUsers.ToString());
                Response.Write("</values>");
                Response.End();
            }
        }
        protected StringBuilder LoadUsers(string strUser, bool boolPNC, bool boolNCB)
        {
            Users oUser = new Users(0, dsn);
            StringBuilder strReturn = new StringBuilder();
            if (boolPNC == true)
            {
                DataSet ds = oUser.Gets(strUser);
                foreach (DataRow dr in ds.Tables[0].Rows)
                    strReturn.Append("<value>" + dr["userid"].ToString() + "</value><text>" + dr["username"].ToString() + " (" + dr["pnc_id"].ToString() + ")" + "</text>");
            }
            else
            {
                DataSet ds = oUser.Gets(strUser, (boolNCB ? 1 : 0));
                foreach (DataRow dr in ds.Tables[0].Rows)
                    strReturn.Append("<value>" + dr["userid"].ToString() + "</value><text>" + dr["username"].ToString() + " (" + dr["xid"].ToString() + ")" + "</text>");
            }
            return strReturn;
        }
    }
}
