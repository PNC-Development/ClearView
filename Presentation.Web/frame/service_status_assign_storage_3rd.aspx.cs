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
    public partial class service_status_assign_storage_3rd : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;

        protected ResourceRequest oResourceRequest;
        protected Requests oRequest;
        protected Services oService;
        protected Customized oCustomized;
        protected string strSpacerRow = "<tr><td colspan=\"3\"><img src=\"/images/spacer.gif\" border=\"0\" width=\"1\" height=\"7\" /></td></tr>";
        protected string strView = "";
        protected string strTitle = "Unavailable";
        protected int intCount = 0;
        protected int intProfile;
        protected int intServiceSAN = Int32.Parse(ConfigurationManager.AppSettings["SERVICEID_SERVER_GROWTH_SAN"]);
        protected int intServiceDist = Int32.Parse(ConfigurationManager.AppSettings["SERVICEID_SERVER_GROWTH_DISTRIBUTED"]);
        protected int intServiceMid = Int32.Parse(ConfigurationManager.AppSettings["SERVICEID_SERVER_GROWTH_MIDRANGE"]);
        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oResourceRequest = new ResourceRequest(intProfile, dsn);
            oRequest = new Requests(intProfile, dsn);
            oService = new Services(intProfile, dsn);
            oCustomized = new Customized(intProfile, dsn);
            if (Request.QueryString["rid"] != null && Request.QueryString["rid"] != "" && Request.QueryString["sid"] != null && Request.QueryString["sid"] != "" && Request.QueryString["sidn"] != null && Request.QueryString["sidn"] != "")
            {
                int intRequest = Int32.Parse(Request.QueryString["rid"]);
                int intService = Int32.Parse(Request.QueryString["sid"]);
                int intNumber = Int32.Parse(Request.QueryString["sidn"]);
                if (oRequest.GetUser(intRequest) == intProfile)
                {
                    DataSet ds = oCustomized.GetStorage3rdFlow1(intRequest, oService.GetItemId(intService), intNumber);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        strView += GetSummary(intRequest, intService, intNumber);
                        if (ds.Tables[0].Rows[0]["number2"].ToString() != "0")
                            strView += GetSummary(intRequest, intServiceSAN, Int32.Parse(ds.Tables[0].Rows[0]["number2"].ToString()));
                        if (ds.Tables[0].Rows[0]["number3"].ToString() != "0")
                            strView += GetSummary(intRequest, intServiceDist, Int32.Parse(ds.Tables[0].Rows[0]["number3"].ToString()));
                        if (ds.Tables[0].Rows[0]["number3"].ToString() != "0")
                            strView += GetSummary(intRequest, intServiceMid, Int32.Parse(ds.Tables[0].Rows[0]["number3"].ToString()));
                    }
                }
                if (strView != "")
                    strView = "<table width=\"100%\" cellpadding=\"3\" cellspacing=\"2\" border=\"0\"><tr><td></td><td><b><u>Technician:</u></b></td><td><b><u>Department:</u></b></td><td align=\"center\"><b><u>Status:</u></b></td><td><b><u>Progress:</u></b></td><td></td></tr>" + strView + "</table>";
                else
                    strView = "Service Information Unavailable";
                strTitle = oService.GetName(intService);
            }
        }
        private string GetSummary(int _requestid, int _serviceid, int _number)
        {
            Users oUser = new Users(intProfile, dsn);
            ServiceRequests oServiceRequest = new ServiceRequests(intProfile, dsn);
            RequestItems oRequestItem = new RequestItems(intProfile, dsn);
            Applications oApplication = new Applications(intProfile, dsn);
            StringBuilder sbSummary = new StringBuilder();
            DataSet ds = oResourceRequest.GetRequestService(_requestid, _serviceid, _number);
            int intCount = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                intCount++;
                int intResource = Int32.Parse(dr["id"].ToString());
                double dblAllocated = double.Parse(dr["allocated"].ToString());
                double dblUsed = oResourceRequest.GetWorkflowUsed(intResource);
                DataSet dsStatus = oResourceRequest.GetStatuss(intResource);
                string strStatus = "";
                StringBuilder sbStatuses = new StringBuilder();
                bool boolChange = false;
                foreach (DataRow drStatus in dsStatus.Tables[0].Rows)
                {
                    double dblStatus = double.Parse(drStatus["status"].ToString());
                    strStatus = oResourceRequest.GetStatus(dblStatus, 30, 15);
                    if (boolChange == true)
                    {
                        sbStatuses.Append("<tr>");
                    }
                    else
                    {
                        sbStatuses.Append("<tr bgcolor=\"#F6F6F6\">");
                    }
                    sbStatuses.Append("<td nowrap valign=\"top\" align=\"center\">");
                    sbStatuses.Append(oResourceRequest.GetStatus(dblStatus, 30, 10));
                    sbStatuses.Append("</td>");
                    sbStatuses.Append("<td nowrap valign=\"top\" align=\"center\">");
                    sbStatuses.Append(DateTime.Parse(drStatus["modified"].ToString()).ToShortDateString());
                    sbStatuses.Append("</td>");
                    sbStatuses.Append("<td width=\"100%\" valign=\"top\">");
                    sbStatuses.Append(drStatus["comments"].ToString());
                    sbStatuses.Append("</td>");
                    sbStatuses.Append("</tr>");
                    boolChange = !boolChange;
                }
                if (sbStatuses.ToString() != "")
                {
                    sbStatuses.Insert(0, "<table width=\"100%\" cellpadding=\"3\" cellspacing=\"0\" border=\"0\" style=\"border:solid 1px #CCCCCC\"><tr bgcolor=\"#EEEEEE\"><td>&nbsp;</td><td><b><u>Date:</u></b></td><td><b><u>Comments:</u></b></td></tr>");
                    sbStatuses.Append("</table>");
                }
                double dblProgress = 0.00;
                if (dblAllocated > 0.00)
                {
                    dblProgress = (dblUsed / dblAllocated) * 100.00;
                }
                sbSummary.Append("<tr>");
                sbSummary.Append("<td><img src=\"/images/resource_people.gif\" border=\"0\" align=\"absmiddle\"></td>");
                int intUser = Int32.Parse(dr["userid"].ToString());
                sbSummary.Append("<td>");
                sbSummary.Append(intUser > 0 ? oUser.GetFullName(intUser) : "Pending Assignment");
                sbSummary.Append("</td>");
                sbSummary.Append("<td>");
                sbSummary.Append(oApplication.GetName(oRequestItem.GetItemApplication(Int32.Parse(dr["itemid"].ToString()))));
                sbSummary.Append("</td>");
                sbSummary.Append("<td nowrap align=\"center\">");
                sbSummary.Append(intUser > 0 ? strStatus : "---");
                sbSummary.Append("</td>");
                sbSummary.Append("<td nowrap width=\"100\">");
                sbSummary.Append(intUser > 0 ? oServiceRequest.GetStatusBar(dblProgress, "100", "14", true) : "---");
                sbSummary.Append("</td>");
                sbSummary.Append("<td width=\"1\" nowrap><a href=\"javascript:void(0);\" onclick=\"ShowHide('div");
                sbSummary.Append(intCount.ToString());
                sbSummary.Append("');\">[Details]</a></td>");
                sbSummary.Append("</tr>");
                sbSummary.Append("<tr>");
                sbSummary.Append("<td colspan=\"6\">");
                sbSummary.Append("<div id=\"div");
                sbSummary.Append(intCount.ToString());
                sbSummary.Append("\" style=\"display:none\">");
                sbSummary.Append(sbStatuses.ToString());
                sbSummary.Append("</div>");
                sbSummary.Append("</td>");
                sbSummary.Append("</tr>");
            }
            return sbSummary.ToString();
        }
    }
}
