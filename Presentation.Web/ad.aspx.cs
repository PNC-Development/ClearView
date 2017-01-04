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

namespace NCC.ClearView.Presentation.Web
{
    public partial class ad : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;

        protected Users oUser;
        protected void Page_Load(object sender, EventArgs e) 
        {
            AuthenticateUser();
            oUser = new Users(0, dsn);
            string strFName = "";
            string strLName = "";
            if (Request.QueryString["u"] != null)
            {
                DataSet ds = oUser.Gets(Request.QueryString["u"], 0);
                if (ds.Tables[0].Rows.Count == 0)
                    Response.Write("NONE");
                else
                {
                    if (ds.Tables[0].Rows.Count == 1)
                    {
                        string strId = ds.Tables[0].Rows[0]["userid"].ToString();
                        string strXId = ds.Tables[0].Rows[0]["xid"].ToString();
                        strFName = ds.Tables[0].Rows[0]["fname"].ToString();
                        strLName = ds.Tables[0].Rows[0]["lname"].ToString();
                        Response.Write(strId + "_" + strXId + "_" + strFName + " " + strLName);
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
