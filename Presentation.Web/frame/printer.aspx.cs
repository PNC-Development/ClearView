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
    public partial class printer : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        
        protected int intProfile;
        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            if (Request.QueryString["rid"] != null && Request.QueryString["rid"] != "")
            {
                Services oService = new Services(intProfile, dsn);
                Requests oRequest = new Requests(intProfile, dsn);
                int intRequest = Int32.Parse(Request.QueryString["rid"]);
                StringBuilder sb = new StringBuilder();
                if (intProfile.ToString() == oRequest.Get(intRequest, "userid"))
                {
                    DataSet ds = oService.GetSelected(intRequest);
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        int intService = Int32.Parse(dr["serviceid"].ToString());
                        int intNumber = Int32.Parse(dr["number"].ToString());
                        if (sb.ToString() != "")
                        {
                            sb.Append("<tr><td><hr size=\"1\" noshade/></td></tr>");
                        }
                        sb.Append("<tr><td class=\"header\">");
                        sb.Append(oService.GetName(intService));
                        sb.Append("</td></tr>");
                        sb.Append("<tr><td>");
                        sb.Append(GetSummary(intRequest, intService, intNumber));
                        sb.Append("</td></tr>");
                    }
                    lblSummary.Text = "<table width=\"100%\" cellpadding=\"3\" cellspacing=\"2\" border=\"0\">" + sb.ToString() + "</table>";
                }
            }
        }
        public string GetSummary(int _requestid, int _serviceid, int _number)
        {
            Users oUser = new Users(intProfile, dsn);
            ServiceRequests oServiceRequest = new ServiceRequests(intProfile, dsn);
            RequestItems oRequestItem = new RequestItems(intProfile, dsn);
            Services oService = new Services(intProfile, dsn);
            Applications oApplication = new Applications(intProfile, dsn);
            ResourceRequest oResourceRequest = new ResourceRequest(intProfile, dsn);
            StringBuilder sbSummary = new StringBuilder();
            DataSet ds = oResourceRequest.GetRequestService(_requestid, _serviceid, _number);
            int intCount = 0;
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    intCount++;
                    int intResource = Int32.Parse(dr["id"].ToString());
                    double dblAllocated = double.Parse(dr["allocated"].ToString());
                    double dblUsed = oResourceRequest.GetWorkflowUsed(intResource);
                    DataSet dsStatus = oResourceRequest.GetStatuss(intResource);
                    string strStatus = "";
                    StringBuilder sbStatuses = new StringBuilder();
                    foreach (DataRow drStatus in dsStatus.Tables[0].Rows)
                    {
                        double dblStatus = double.Parse(drStatus["status"].ToString());
                        strStatus = oResourceRequest.GetStatus(dblStatus, 30, 15);
                        sbStatuses.Append("<tr>");
                        sbStatuses.Append("<td nowrap valign=\"top\" align=\"center\">");
                        sbStatuses.Append(oResourceRequest.GetStatus(dblStatus, 30, 10));
                        sbStatuses.Append("</td>");
                        sbStatuses.Append("<td nowrap valign=\"top\" align=\"center\"><b>");
                        sbStatuses.Append(DateTime.Parse(drStatus["modified"].ToString()).ToShortDateString());
                        sbStatuses.Append("</b></td>");
                        sbStatuses.Append("<td width=\"100%\" valign=\"top\">");
                        sbStatuses.Append(drStatus["comments"].ToString());
                        sbStatuses.Append("</td>");
                        sbStatuses.Append("</tr>");
                    }
                    if (sbStatuses.ToString() != "")
                    {
                        sbStatuses.Insert(0, "<table width=\"100%\" cellpadding=\"3\" cellspacing=\"0\" border=\"0\" style=\"border:solid 1px #CCCCCC\"><tr bgcolor=\"#EEEEEE\"><td>&nbsp;</td><td><b>Date</b></td><td><b>Comments</b></td></tr>");
                        sbStatuses.Append("</table>");
                    }
                    double dblProgress = 0.00;
                    if (dblAllocated > 0.00)
                        dblProgress = (dblUsed / dblAllocated) * 100.00;
                    int intItem = Int32.Parse(dr["itemid"].ToString());
                    if (oService.Get(_serviceid, "automate") != "1")
                    {
                        sbSummary.Append("<tr>");
                        sbSummary.Append("<td><img src=\"/images/resource_people.gif\" border=\"0\" align=\"absmiddle\"></td>");
                        int intUser = Int32.Parse(dr["userid"].ToString());
                        sbSummary.Append("<td width=\"50%\"><a href=\"javascript:void(0);\">");
                        sbSummary.Append(intUser > 0 ? oUser.GetFullName(intUser) : "Pending Assignment");
                        sbSummary.Append("</a></td>");
                        sbSummary.Append("<td width=\"50%\">");
                        sbSummary.Append(oApplication.GetName(oRequestItem.GetItemApplication(intItem)));
                        sbSummary.Append("</td>");
                        sbSummary.Append("<td nowrap align=\"center\">");
                        sbSummary.Append(intUser > 0 ? strStatus : "---");
                        sbSummary.Append("</td>");
                        sbSummary.Append("<td nowrap>");
                        sbSummary.Append(oServiceRequest.GetStatusBar(dblProgress, "100", "14", true));
                        sbSummary.Append("</td>");
                        sbSummary.Append("</tr>");
                        sbSummary.Append("<tr>");
                        sbSummary.Append("<td colspan=\"5\">");
                        sbSummary.Append("<div id=\"div");
                        sbSummary.Append(intCount.ToString());
                        sbSummary.Append("\" style=\"display:inline\">");
                        sbSummary.Append(sbStatuses.ToString());
                        sbSummary.Append("</div>");
                        sbSummary.Append("</td>");
                        sbSummary.Append("</tr>");
                    }
                }
            }
            else
            {
                sbSummary.Append("<tr>");
                sbSummary.Append("<td><img src=\"/images/resource_people.gif\" border=\"0\" align=\"absmiddle\"></td>");
                sbSummary.Append("<td width=\"50%\"><a href=\"javascript:void(0);\">Automated Service</a></td>");
                sbSummary.Append("<td width=\"50%\">N / A</td>");
                sbSummary.Append("<td nowrap align=\"center\">---</td>");
                sbSummary.Append("<td nowrap>");
                sbSummary.Append(oServiceRequest.GetStatusBar(100.00, "100", "14", true));
                sbSummary.Append("</td>");
                sbSummary.Append("</tr>");
            }

            sbSummary.Insert(0, "<table width=\"100%\" cellpadding=\"3\" cellspacing=\"2\" border=\"0\"><tr><td></td><td><b><u>Technician:</u></b></td><td><b><u>Department:</u></b></td><td align=\"center\"><b><u>Status:</u></b></td><td><b><u>Progress:</u></b></td></tr>");
            sbSummary.Append("</table>");

            return sbSummary.ToString();
        }
    }
}
