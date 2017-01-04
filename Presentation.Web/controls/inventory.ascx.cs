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
using System.Text;
namespace NCC.ClearView.Presentation.Web
{
    public partial class inventory : System.Web.UI.UserControl
    {

        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected Pages oPage;
        protected Platforms oPlatform;
        protected int intProfile;
        protected int intApplication = 0;
        protected int intPage = 0;
        protected bool boolAction = false;
        protected bool boolDemand = false;
        protected bool boolSupply = false;
        protected bool boolOrder = false;
        protected bool boolAdd = false;
        protected bool boolSettings = false;
        protected bool boolForms = false;
        protected bool boolAlert = false;
        protected string strPlatforms = "";
        protected string strActionForm = "";
        protected string strDemandForm = "";
        protected string strSupplyForm = "";
        protected string strOrderForm = "";
        protected string strAddForm = "";
        protected string strSettingsForm = "";
        protected string strAlertForm = "";
        protected string strFormForm = "";
        protected string strTabs = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oPage = new Pages(intProfile, dsn);
            oPlatform = new Platforms(intProfile, dsn);
            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            lblTitle.Text = oPage.Get(intPage, "title");
            int intPlatform = 0;
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                intPlatform = Int32.Parse(Request.QueryString["id"]);
            // Load Platforms
            DataSet ds = oPlatform.GetInventorys(intProfile, 1);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (intPlatform == Int32.Parse(dr["platformid"].ToString()))
                    strPlatforms += "<td><img src=\"/images/arrow_red.gif\" border=\"0\" align=\"absmiddle\" /></td><td><a class=\"greentableheader\" onclick=\"LoadWait();\" href=\"" + oPage.GetFullLink(intPage) + "?id=" + dr["platformid"].ToString() + "\">" + dr["name"].ToString() + "</a></td>";
                else
                    strPlatforms += "<td><img src=\"/images/arrow_green.gif\" border=\"0\" align=\"absmiddle\" /></td><td><a class=\"greentableheader\" onclick=\"LoadWait();\" href=\"" + oPage.GetFullLink(intPage) + "?id=" + dr["platformid"].ToString() + "\">" + dr["name"].ToString() + "</a></td>";
            }
            strPlatforms = "<table cellpadding=\"3\" cellspacing=\"1\" border=\"0\">" + strPlatforms + "</table>";
            if (intPlatform > 0)
            {
                DataSet dsForms = oPlatform.GetForms(intPlatform, 1);
                foreach (DataRow drForm in dsForms.Tables[0].Rows)
                {
                    StringBuilder sb = new StringBuilder(strTabs);
                    string strImage = drForm["image"].ToString();
                    if (strImage == "")
                    {
                        strImage = "<img src=\"/images/bigCheck.gif\" border=\"0\" align=\"absmiddle\" />";
                    }

                    sb.Append("<tr><td onclick=\"OpenWindow('NEW_WINDOW','");
                    sb.Append(drForm["path"].ToString());
                    sb.Append("?id=");
                    sb.Append(intPlatform.ToString());
                    sb.Append("');\" style=\"cursor:hand\"><table width=\"100%\" cellpadding=\"0\" cellspacing=\"5\" border=\"0\">");
                    sb.Append("<tr>");
                    sb.Append("<td rowspan=\"2\">");
                    sb.Append(strImage);
                    sb.Append("</td>");
                    sb.Append("<td class=\"bold\" width=\"100%\" valign=\"bottom\">");
                    sb.Append(drForm["name"].ToString());
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td width=\"100%\" valign=\"top\">");
                    sb.Append(drForm["description"].ToString());
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    sb.Append("</table></td></tr>");

                    strTabs = sb.ToString();
                }
            }
        }
    }
}