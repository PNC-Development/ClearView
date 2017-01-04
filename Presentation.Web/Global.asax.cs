using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using NCC.ClearView.Application.Core;

namespace NCC.ClearView.Presentation.Web
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
        }

        protected void Application_End(object sender, EventArgs e)
        {

        }

        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected string strPage;
        private ArrayList oArray;
        protected Pages oPage;
        protected Applications oApplication;
        protected Settings oSetting;
        protected int intProfile = 0;
        protected bool boolMaintenance = (ConfigurationManager.AppSettings["Maintenance"] == "1");
        protected void Application_BeginRequest(Object sender, EventArgs e)
        {
            if (boolMaintenance)
                Response.Redirect("~/down.htm");

            HttpContext oContext = HttpContext.Current;
            if (Request.Cookies["profileid"] != null && Request.Cookies["profileid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            string strSuffix = "/default.aspx";
            strPage = "";
            int intNext = 0;
            string strPath = oContext.Server.UrlDecode(oContext.Request.Url.PathAndQuery);
            string strQuery = oContext.Server.UrlDecode(oContext.Request.Path);
            string strVariables = "";
            string strApplication = "";
            foreach (string strName in Request.QueryString)
                strVariables += "&" + strName + "=" + Request.QueryString[strName];
            if (strPath.StartsWith("/") == true)
                strPath = strPath.Substring(1);
            if (strQuery.EndsWith(strSuffix))
            {
                oPage = new Pages(0, dsn);
                oApplication = new Applications(0, dsn);
                oArray = new ArrayList();
                oSetting = new Settings(0, dsn);
                if (oSetting.Get("down") != "1")
                {
                    if (strQuery.IndexOf(strSuffix) > -1)
                        strQuery = strQuery.Substring(0, strQuery.IndexOf(strSuffix));
                    if (strQuery.StartsWith("/") == true)
                        strQuery = strQuery.Substring(1);
                    char[] splitter = { '/' };
                    oArray.AddRange(strQuery.Split(splitter));
                    DataSet dsApps = oApplication.Gets(1);
                    foreach (DataRow dr in dsApps.Tables[0].Rows)
                    {
                        if (dr["url"].ToString().ToUpper() == oArray[0].ToString().ToUpper())
                        {
                            Response.Cookies["application"].Value = dr["applicationid"].ToString();
                            Request.Cookies["application"].Value = dr["applicationid"].ToString();
                            intNext = 1;
                            strApplication = dr["applicationid"].ToString();
                            break;
                        }
                    }
                    if (strPage == "")
                    {
                        if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                        {
                            if (oArray.Count > intNext)
                                BuildPath(Request.Cookies["application"].Value, "0", intNext);
                            else
                                strPage = "/interior.aspx?applicationid=" + strApplication;
                            if (strPage == "")
                                oContext.RewritePath("/NotFound.aspx");
                            else
                                oContext.RewritePath(strPage + strVariables);
                        }
                        else
                            oContext.RewritePath("/index.aspx");
                    }
                    else
                        oContext.RewritePath(strPage + strVariables);
                }
                else
                    oContext.RewritePath("/index.aspx?down=true");
            }
        }
        private void BuildPath(string strApplication, string strParent, int intNext)
        {
            DataSet ds = oPage.Gets(Int32.Parse(strApplication), intProfile, Int32.Parse(strParent), 0, 1);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (dr["parent"].ToString() == strParent && dr["urltitle"].ToString().ToUpper() == oArray[intNext].ToString().ToUpper())
                {
                    if (intNext < oArray.Count - 1)
                        BuildPath(strApplication, dr["pageid"].ToString(), intNext + 1);
                    else
                    {
                        strPage = dr["path"].ToString() + "?pageid=" + dr["pageid"].ToString();
                        //                    strPage = dr["path"].ToString() + "?pageid=" + dr["pageid"].ToString() + "&apppageid=" + dr["apppageid"].ToString();
                        break;
                    }
                }
            }
        }
    }
}