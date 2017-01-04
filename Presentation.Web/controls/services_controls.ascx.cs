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
    public partial class services_controls : System.Web.UI.UserControl
    {

        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected Pages oPage;
        protected int intProfile;
        protected int intPage = 0;
        protected int intApplication = 0;
        protected string strFavorites = "";
        protected Services oService;
        protected ServiceRequests oServiceRequest;
        protected Requests oRequest;
        protected Users oUser;
        protected bool boolWM = false;
        protected int intViewRequest = Int32.Parse(ConfigurationManager.AppSettings["ResourceRequest"]);
        protected void Page_Load(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder(strFavorites);
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oPage = new Pages(intProfile, dsn);
            oService = new Services(intProfile, dsn);
            oServiceRequest = new ServiceRequests(intProfile, dsn);
            oRequest = new Requests(intProfile, dsn);
            oUser = new Users(intProfile, dsn);
            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            lblTitle.Text = "My Favorite Services";
            bool boolFavorite = false;
            DataSet dsFavorite = oService.GetFavorites(intProfile);
            foreach (DataRow drFavorite in dsFavorite.Tables[0].Rows)
            {
                int intService = Int32.Parse(drFavorite["serviceid"].ToString());
                sb.Append("<tr style=\"background-color:");
                sb.Append(boolFavorite ? "#F6F6F6" : "#FFFFFF");
                sb.Append("\" id=\"trDept");
                sb.Append(drFavorite["serviceid"].ToString());
                sb.Append("\">");
                boolFavorite = !boolFavorite;
                if (oServiceRequest.GetTasks(intProfile, intService).Tables[0].Rows.Count == 0 || boolWM == false)
                {
                    sb.Append("<td nowrap><input type=\"checkbox\" onclick=\"HighlightCheckRow(this, 'trDept");
                    sb.Append(intService.ToString());
                    sb.Append("','");
                    sb.Append(intService.ToString());
                    sb.Append("','");
                    sb.Append(hdnService.ClientID);
                    sb.Append("',true);\"/></td>");
                    sb.Append("<td nowrap width=\"20%\"><a href=\"javascript:void(0);\" onclick=\"OpenWindow('SERVICES_DETAIL','?sid=");
                    sb.Append(intService.ToString());
                    sb.Append("');\">");
                    sb.Append(drFavorite["name"].ToString());
                    sb.Append("</a></td>");
                }
                else
                {
                    sb.Append("<td nowrap><input type=\"checkbox\" onclick=\"HighlightCheckRow(this, 'trDept");
                    sb.Append(intService.ToString());
                    sb.Append("','");
                    sb.Append(intService.ToString());
                    sb.Append("','");
                    sb.Append(hdnService.ClientID);
                    sb.Append("',false);\"/></td>");
                    sb.Append("<td nowrap width=\"20%\" class=\"default\"><a href=\"javascript:void(0);\" onclick=\"OpenWindow('SERVICES_DETAIL','?sid=");
                    sb.Append(intService.ToString());
                    sb.Append("');\">");
                    sb.Append(drFavorite["name"].ToString());
                    sb.Append("</a></td>");
                }

                sb.Append("<td width=\"60%\">");
                sb.Append(drFavorite["description"].ToString());
                sb.Append("</td>");
                int intItem = oService.GetItemId(intService);
                string strItem = "";
                DataSet dsManagers = oService.GetUser(intService, -1);
                foreach (DataRow drManager in dsManagers.Tables[0].Rows)
                {
                    if (strItem != "")
                        strItem += "<br/>";
                    strItem += "<a href=\"javascript:void(0);\" onclick=\"OpenWindow('PROFILE','?userid=" + drManager["userid"].ToString() + "');\"><img src=\"/images/user.gif\" border=\"0\" align=\"absmiddle\"/> " + oUser.GetFullName(Int32.Parse(drManager["userid"].ToString())) + "</a>";
                }
                if (strItem == "")
                {
                    // Check the people that get assigned
                    dsManagers = oService.GetUser(intService, 0);
                    foreach (DataRow drManager in dsManagers.Tables[0].Rows)
                    {
                        if (strItem != "")
                            strItem += ", ";
                        strItem += "<a href=\"javascript:void(0);\" onclick=\"OpenWindow('PROFILE','?userid=" + drManager["userid"].ToString() + "');\">" + oUser.GetFullName(Int32.Parse(drManager["userid"].ToString())) + "</a>";
                    }
                }
                sb.Append("<td width=\"20%\" nowrap>");
                sb.Append(strItem);
                sb.Append("</td>");
                sb.Append("</tr>");
            }
            if (sb.ToString() == "")
            {
                sb.Append("<tr><td colspan=\"4\"> You have not selected any favorites</td></tr>");
                btnFavorite.Enabled = false;
            }

            sb.Insert(0, "<tr bgcolor=\"#EEEEEE\"><td></td><td width=\"20%\"><b><u>Service:</u></b></td><td width=\"60%\"><b><u>Description:</u></b></td><td width=\"20%\" nowrap><b><u>Service Owner:</u></b></td></tr>");
            sb.Insert(0, "<table width=\"100%\" border=\"0\" cellpadding=\"4\" cellspacing=\"0\" style=\"border:solid 1px #CCCCCC\">");
            sb.Append("</table>");

            strFavorites = sb.ToString();
            btnFavorite.Attributes.Add("onclick", "return ValidateStringItems('" + hdnService.ClientID + "','Please select at least one service') && ProcessButton(this);");
        }
        protected void btnFavorite_Click(Object Sender, EventArgs e)
        {
            int intRequest = 0;
            if (intRequest == 0)
            {
                intRequest = oRequest.Add(-1, intProfile);
                oServiceRequest.Add(intRequest, 1, -2);
            }
            string strHidden = Request.Form[hdnService.UniqueID];
            while (strHidden != "")
            {
                string strField = strHidden.Substring(0, strHidden.IndexOf("&"));
                strHidden = strHidden.Substring(strHidden.IndexOf("&") + 1);
                string strFlag = strField.Substring(strField.IndexOf("_") + 1);
                strField = strField.Substring(0, strField.IndexOf("_"));
                if (strFlag == "1")
                    oService.AddSelected(intRequest, Int32.Parse(strField), 1);
            }
            oServiceRequest.Update(intRequest, -1);
            Response.Redirect(oPage.GetFullLink(intViewRequest) + "?rid=" + intRequest.ToString());
        }
    }
}