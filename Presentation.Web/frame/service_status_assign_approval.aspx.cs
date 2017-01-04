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
    public partial class service_status_assign_approval : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;

        protected ResourceRequest oResourceRequest;
        protected Requests oRequest;
        protected Services oService;
        protected string strSpacerRow = "<tr><td colspan=\"3\"><img src=\"/images/spacer.gif\" border=\"0\" width=\"1\" height=\"7\" /></td></tr>";
        protected string strView = "Service Information Unavailable";
        protected string strTitle = "Unavailable";
        protected int intCount = 0;
        protected int intProfile;

        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oResourceRequest = new ResourceRequest(intProfile, dsn);
            oRequest = new Requests(intProfile, dsn);
            oService = new Services(intProfile, dsn);
            if (Request.QueryString["rid"] != null && Request.QueryString["rid"] != "" && Request.QueryString["sid"] != null && Request.QueryString["sid"] != "" && Request.QueryString["sidn"] != null && Request.QueryString["sidn"] != "")
            {
                int intRequest = Int32.Parse(Request.QueryString["rid"]);
                int intService = Int32.Parse(Request.QueryString["sid"]);
                int intNumber = Int32.Parse(Request.QueryString["sidn"]);
                if (oRequest.GetUser(intRequest) == intProfile)
                    strView = GetSummary(intRequest, intService, intNumber);
                strTitle = oService.GetName(intService);
            }
        }
        private string GetSummary(int _requestid, int _serviceid, int _number)
        {
            Users oUser = new Users(intProfile, dsn);
            ServiceRequests oServiceRequest = new ServiceRequests(intProfile, dsn);
            RequestItems oRequestItem = new RequestItems(intProfile, dsn);
            Applications oApplication = new Applications(intProfile, dsn);
            StatusLevels oStatus = new StatusLevels();
            StringBuilder sbSummary = new StringBuilder();
            DataSet ds = oResourceRequest.GetRequestService(_requestid, _serviceid, _number);
            int intCount = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                intCount++;
                int intResource = Int32.Parse(dr["id"].ToString());
                int intItem = Int32.Parse(dr["itemid"].ToString());
                int intNumber = Int32.Parse(dr["number"].ToString());
                int intStatus = Int32.Parse(dr["status"].ToString());
                double dblAllocated = double.Parse(dr["allocated"].ToString());
                double dblProgress = 0.00;
                if (intStatus == 3)
                    dblProgress = 100.00;
                else
                {
                    double dblUsed = oResourceRequest.GetWorkflowUsed(intResource);
                    if (dblAllocated > 0.00)
                        dblProgress = (dblUsed / dblAllocated) * 100.00;
                }
                string strReason = "Unavailable";
                int intApproval = 0;
                DataSet dsReason = oResourceRequest.GetApproval(_requestid, intItem, intNumber);
                if (dsReason.Tables[0].Rows.Count > 0)
                {
                    strReason = dsReason.Tables[0].Rows[0]["reason"].ToString().Trim();
                    if (strReason == "")
                        strReason = "<i>No additional information...</i>";
                    intApproval = Int32.Parse(dsReason.Tables[0].Rows[0]["approval"].ToString());
                }
                string strStatus = oStatus.Name(intApproval);
                bool boolResult = (intApproval == 1);
                sbSummary.Append("<tr>");
                sbSummary.Append("<td width=\"1\"><img src=\"/images/resource_people.gif\" border=\"0\" align=\"absmiddle\"></td>");
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
                sbSummary.Append("_1');\">[Details]</a>");
                sbSummary.Append(boolResult ? "&nbsp;<a href=\"javascript:void(0);\" onclick=\"ShowHide('div" + intCount.ToString() + "_2');\">[Result]</a>" : "");
                sbSummary.Append("</td>");
                sbSummary.Append("</tr>");
                sbSummary.Append("<tr>");
                sbSummary.Append("<td colspan=\"6\">");
                sbSummary.Append("<div id=\"div");
                sbSummary.Append(intCount.ToString());
                sbSummary.Append("_1\" style=\"display:none\">");
                sbSummary.Append(strReason);
                sbSummary.Append("</div>");

                if (boolResult == true)
                {
                    string strResult = "";
                    DataSet dsResult = oRequest.GetResult(_requestid, intItem, intNumber);
                    foreach (DataRow drResult in dsResult.Tables[0].Rows)
                    {
                        strResult += drResult["result"].ToString();
                    }
                    sbSummary.Append("<div id=\"div");
                    sbSummary.Append(intCount.ToString());
                    sbSummary.Append("_2\" style=\"display:none\">");
                    sbSummary.Append(strResult);
                    sbSummary.Append("</div>");
                }
                sbSummary.Append("</td>");
                sbSummary.Append("</tr>");
            }

            sbSummary.Insert(0, "<table width=\"100%\" cellpadding=\"3\" cellspacing=\"2\" border=\"0\"><tr><td></td><td><b><u>Technician:</u></b></td><td><b><u>Department:</u></b></td><td align=\"center\"><b><u>Status:</u></b></td><td><b><u>Progress:</u></b></td><td></td></tr>");
            sbSummary.Append("</table>");

            return sbSummary.ToString();
        }
    }
}
