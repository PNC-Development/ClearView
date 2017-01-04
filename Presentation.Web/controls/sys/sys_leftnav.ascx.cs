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
    public partial class sys_leftnav : System.Web.UI.UserControl
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected Applications oApplication;
        protected Pages oPage;
        protected PageControls oPageControl;
        protected int intProfile = 0;
        protected int intApplication = 0;
        protected int intPage = 0;
        protected string strMenu = "";
        protected int intCount = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oApplication = new Applications(intProfile, dsn);
            oPage = new Pages(intProfile, dsn);
            oPageControl = new PageControls(intProfile, dsn);
            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            AddPages(intApplication);
            if (strMenu != "")
                strMenu = "<div id=\"mainscroll\"><table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\" class=\"default\">" + strMenu + "</table></div>";
        }

        protected void AddPages(int _application)
        {
            DataSet ds = oPage.Gets(_application, intProfile, 0, 1, 1);
            int _parent = oPage.GetParent(intPage);
            StringBuilder sb = new StringBuilder(strMenu);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                int _page = Int32.Parse(dr["pageid"].ToString());
                string strShow = "none";
                string strSubMenu = (_page == intPage || _page == _parent ? AddPages(_page, _application) : "");
                if ((_page == intPage && strSubMenu != "") || (_parent == _page && strSubMenu != "") || (_page == oPage.GetParent(_parent) && strSubMenu != ""))
                {
                    strShow = "inline";
                }

                intCount++;
                sb.Append("<tr>");
                sb.Append("<td><img src=\"/images/table_topLeft.gif\" border=\"0\" width=\"5\" height=\"26\"></td>");

                string strHelp = oPage.Get(_page, "tooltip");
                if (strHelp != "")
                {
                    strHelp = " title=\"" + strHelp + "\"";
                }

                sb.Append("<td nowrap background=\"/images/table_top.gif\" width=\"100%\"");
                sb.Append(strHelp);
                sb.Append("><img src=\"");
                sb.Append(dr["navimage"].ToString() == "" ? "/images/spacer.gif" : dr["navimage"].ToString());
                sb.Append("\" border=\"0\" align=\"absmiddle\" width=\"24\" height=\"24\" /> <a ");
                sb.Append(oPage.GetHref(_page));
                sb.Append(" class=\"greentableheader\">");
                sb.Append(oPage.Get(_page, "menutitle"));
                sb.Append("</a></td>");
                sb.Append("<td><img src=\"/images/table_topRight.gif\" border=\"0\" width=\"5\" height=\"26\"></td>");
                sb.Append("</tr>");
                sb.Append("<tr id=\"div");
                sb.Append(intCount.ToString());
                sb.Append("t\" style=\"display:");
                sb.Append(strShow);
                sb.Append("\">");
                sb.Append("<td background=\"/images/table_left.gif\"><img src=\"/images/table_left.gif\" width=\"5\" height=\"10\"></td>");
                sb.Append("<td width=\"100%\" bgcolor=\"#FFFFFF\">");
                sb.Append(strSubMenu);
                sb.Append("</td>");
                sb.Append("<td background=\"/images/table_right.gif\"><img src=\"/images/table_right.gif\" width=\"5\" height=\"10\"></td>");
                sb.Append("</tr>");
                sb.Append("<tr id=\"div");
                sb.Append(intCount.ToString());
                sb.Append("b\" style=\"display:");
                sb.Append(strShow);
                sb.Append("\">");
                sb.Append("<td><img src=\"/images/table_bottomLeft.gif\" border=\"0\" width=\"5\" height=\"9\"></td>");
                sb.Append("<td width=\"100%\" background=\"/images/table_bottom.gif\"></td>");
                sb.Append("<td><img src=\"/images/table_bottomRight.gif\" border=\"0\" width=\"5\" height=\"9\"></td>");
                sb.Append("</tr>");
                sb.Append("<tr><td colspan=\"3\" height=\"5\"><img src=\"/images/spacer.gif\" border=\"0\" height=\"5\" width=\"1\"></td></tr>");
            }

            strMenu = sb.ToString();
        }
        protected string AddPages(int _parent, int _application)
        {
            StringBuilder sb = new StringBuilder();

            DataSet ds = oPage.Gets(_application, intProfile, _parent, 1, 1);
            int intRow = 0;
            int intTotal = ds.Tables[0].Rows.Count;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                intRow++;
                if (intRow == 1)
                {
                    sb.Append("<tr>");
                    sb.Append("<td height=\"5\" colspan=\"3\"><img src=\"/images/spacer.gif\" width=\"1\" height=\"5\"></td>");
                    sb.Append("</tr>");
                }
                int intChild = Int32.Parse(dr["pageid"].ToString());
                string strHelp = oPage.Get(intChild, "tooltip");
                if (strHelp != "")
                    strHelp = " title=\"" + strHelp + "\"";
                sb.Append("<tr");
                sb.Append(strHelp);
                sb.Append(">");
                sb.Append("<td><img src=\"/images/spacer.gif\" width=\"10\" height=\"5\"></td>");
                //sb.Append("<td valign=\"top\">&nbsp;<img src=\"/images/menu.gif\" border=\"0\" align=\"absmiddle\"/></td>");

                string strTotal = "";
                if (oPage.Get(intChild, "sproc") != "")
                {
                    DataSet dsTotal = oPage.GetTotal(intProfile, oPage.Get(intChild, "sproc"));
                    if (dsTotal.Tables[0].Rows.Count > 0)
                        strTotal = "<span class=\"leftnavp\"> (" + dsTotal.Tables[0].Rows.Count + ")</span>";
                }

                sb.Append("<td valign=\"top\" width=\"100%\"><a ");
                sb.Append(oPage.GetHref(intChild));
                sb.Append(" class=\"leftnav\">");
                sb.Append(oPage.Get(intChild, "menutitle"));
                sb.Append(strTotal);
                sb.Append("</a></td>");
                sb.Append("</tr>");

                if (intRow < intTotal)
                {
                    sb.Append("<tr>");
                    sb.Append("<td height=\"10\" colspan=\"3\" background=\"/images/gray_dot.gif\"><img src=\"/images/gray_dot.gif\" width=\"1\" height=\"10\"></td>");
                    sb.Append("</tr>");
                }
            }
            if (sb.ToString() != "")
            {
                sb.Insert(0, "<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\">");
                sb.Append("</table>");
            }

            return sb.ToString();
        }
    }
}