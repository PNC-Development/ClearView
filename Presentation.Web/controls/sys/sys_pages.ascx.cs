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
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Text;

namespace NCC.ClearView.Presentation.Web
{
    public partial class sys_pages : System.Web.UI.UserControl
    {
        private DataSet ds;
        protected Pages oPage;
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intProfile;
        protected string strMenu;
        protected int intPage = 0;
        protected int intApplication = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder(strMenu);
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oPage = new Pages(intProfile, dsn);
            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            string strTitle = oPage.Get(intPage, "title");
            lblTitle.Text = strTitle;
            ds = oPage.Gets(intApplication, intProfile, intPage, 1, 1);
            if (ds.Tables[0].Rows.Count == 0)
            {
                sb.Insert(0, "<tr height=\"5\"><td><img src=\"/images/spacer.gif\" border=\"0\" height=\"5\" width=\"1\" /></td></tr>");
                sb.Append("<tr><td class=\"default\"><img src=\"/images/spacer.gif\" border=\"0\" width=\"15\" height=\"1\" /><img src=\"/images/bigInfo.gif\" border=\"0\" align=\"absmiddle\" /> There are no pages available under <b>");
                sb.Append(strTitle);
                sb.Append("</b></td></tr>");
            }
            else
            {
                int intColumns = 2;
                int intCount = intColumns;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (intCount == intColumns)
                    {
                        if (sb.ToString() != "")
                        {
                            sb.Append("</tr>");
                        }
                        sb.Append("<tr>");
                        intCount = 0;
                    }
                    intCount++;
                    string strTotal = "";
                    int intChild = Int32.Parse(dr["pageid"].ToString());
                    if (oPage.Get(intChild, "sproc") != "")
                    {
                        DataSet dsTotal = oPage.GetTotal(intProfile, oPage.Get(intChild, "sproc"));
                        strTotal = " (" + dsTotal.Tables[0].Rows.Count + ")";
                    }
                    strTitle = oPage.Get(intChild, "title");
                    string strDescription = oPage.Get(intChild, "description");
                    if (strDescription == "")
                    {
                        strDescription = "<em>No Additional Information</em>";
                    }
                    sb.Append("<td><img src=\"/images/spacer.gif\" border=\"0\" width=\"15\" height=\"1\" /></td><td colspan=\"2\" width=\"50%\"><table width=\"100%\" cellpadding=\"0\" cellspacing=\"5\" border=\"0\"><tr><td rowspan=\"2\"><img src=\"/images/ico_check.gif\" border=\"0\" align=\"absmiddle\" /></td><td class=\"header\" width=\"100%\" valign=\"bottom\"><a ");
                    sb.Append(oPage.GetHref(intChild));
                    sb.Append("\" title=\"");
                    sb.Append(strTitle);
                    sb.Append("\" class=\"navigation\">");
                    sb.Append(strTitle);
                    sb.Append(strTotal);
                    sb.Append("</a></td></tr><tr><td width=\"100%\" valign=\"top\">");
                    sb.Append(strDescription);
                    sb.Append("</td></tr></table></td>");
                }
                if (intCount < intColumns)
                {
                    while (intCount < intColumns)
                    {
                        sb.Append("<td colspan=\"3\" width=\"50%\">&nbsp;</td>");
                        intCount++;
                    }
                    sb.Append("</tr>");
                }
                sb.Insert(0, "<tr height=\"5\"><td colspan=\"6\"><img src=\"/images/spacer.gif\" border=\"0\" height=\"5\" width=\"1\" /></td></tr>");
            }

            if (sb.ToString() != "")
            {
                sb.Insert(0, "<table width=\"100%\" cellpadding=\"2\" cellspacing=\"1\" border=\"0\">");
                sb.Append("</table>");
            }

            strMenu = sb.ToString();
        }
    }
}