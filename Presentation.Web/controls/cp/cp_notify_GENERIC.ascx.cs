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
using Microsoft.ApplicationBlocks.Data;

namespace NCC.ClearView.Presentation.Web
{
    public partial class cp_notify_GENERIC : System.Web.UI.UserControl
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        private string dsnServiceEditor = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ServiceEditorDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intAssignPage = Int32.Parse(ConfigurationManager.AppSettings["AssignWorkload"]);
        protected int intViewPage = Int32.Parse(ConfigurationManager.AppSettings["ViewWorkload"]);
        protected int intResourceRequestApprove = Int32.Parse(ConfigurationManager.AppSettings["ResourceRequestApprove"]);
        protected string strDone = "";
        protected int intProfile = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            RequestItems oRequestItem = new RequestItems(intProfile, dsn);
            RequestFields oRequestField = new RequestFields(intProfile, dsn);
            ServiceRequests oServiceRequest = new ServiceRequests(intProfile, dsn);
            Services oService = new Services(intProfile, dsn);
            ResourceRequest oResourceRequest = new ResourceRequest(intProfile, dsn);
            Log oLog = new Log(intProfile, dsn);
            Users oUser = new Users(intProfile, dsn);
            int intRequest = Int32.Parse(Request.QueryString["rid"]);
            string strStatus = oServiceRequest.Get(intRequest, "checkout");
            DataSet dsItems = oRequestItem.GetForms(intRequest);
            int intItem = 0;
            int intService = 0;
            int intNumber = 0;
            if (dsItems.Tables[0].Rows.Count > 0)
            {
                bool boolBreak = false;
                foreach (DataRow drItem in dsItems.Tables[0].Rows)
                {
                    if (boolBreak == true)
                        break;
                    if (drItem["done"].ToString() == "0")
                    {
                        intItem = Int32.Parse(drItem["itemid"].ToString());
                        intService = Int32.Parse(drItem["serviceid"].ToString());
                        intNumber = Int32.Parse(drItem["number"].ToString());
                        boolBreak = true;
                    }
                    if (intItem > 0 && (strStatus == "1" || strStatus == "2"))
                    {
                        bool boolSuccess = true;
                        string strWorkflowTitle = oService.Get(intService, "workflow_title");
                        string strResult = (strWorkflowTitle == "" ? oService.GetName(intService) : strWorkflowTitle) + " Completed";
                        string strError = (strWorkflowTitle == "" ? oService.GetName(intService) : strWorkflowTitle) + " Error";
                        // ********* BEGIN PROCESSING **************
                        double dblHours = 0.00;
                        int intDevices = 1;
                        Field oField = new Field(intProfile, dsn);
                        string strTable = oField.GetTableName2(intService);
                        if (strTable != "")
                        {
                            DataSet ds = oField.GetTableServiceRequest(strTable, intRequest.ToString(), intItem.ToString(), intNumber.ToString());
                            if (ds.Tables[0].Columns.Contains("hours") == true)
                            {
                                foreach (DataRow dr in ds.Tables[0].Rows)
                                    dblHours += double.Parse(dr["hours"].ToString());
                            }
                            if (ds.Tables[0].Columns.Contains("devices") == true)
                            {
                                foreach (DataRow dr in ds.Tables[0].Rows)
                                    intDevices += Int32.Parse(dr["devices"].ToString());
                            }
                        }
                        if (oService.Get(intService, "quantity_is_device") == "1")
                        {
                            DataSet dsTemp = oService.GetSelected(intRequest, intService);
                            if (dsTemp.Tables[0].Rows.Count > 0)
                                intDevices = Int32.Parse(dsTemp.Tables[0].Rows[0]["quantity"].ToString());
                        }
                        string strCVT = "CVT" + intRequest.ToString() + "-" + intService.ToString() + "-" + intNumber.ToString();
                        int intResource = oServiceRequest.AddRequest(intRequest, intItem, intService, intDevices, dblHours, 2, intNumber, dsnServiceEditor);
                        oLog.AddEvent(intRequest.ToString(), strCVT, "Service request has been submitted by " + oUser.GetFullNameWithLanID(intProfile), LoggingType.Debug);
                        if (oServiceRequest.NotifyApproval(intResource, intResourceRequestApprove, intEnvironment, "", dsnServiceEditor) == false)
                        {
                            oLog.AddEvent(intRequest.ToString(), strCVT, "Service request has been fully approved", LoggingType.Debug);
                            oServiceRequest.NotifyTeamLead(intItem, intResource, intAssignPage, intViewPage, intEnvironment, "", dsnServiceEditor, dsnAsset, dsnIP, 0);
                            oLog.AddEvent(intRequest.ToString(), strCVT, "Service request NotifyTeamLead has completed.", LoggingType.Debug);
                        }
                        else
                            oLog.AddEvent(intRequest.ToString(), strCVT, "Service request has been sent for approval", LoggingType.Debug);
                        // ******** END PROCESSING **************
                        if (oService.Get(intService, "automate") == "1" && boolSuccess == true)
                            strDone += "<p><span class=\"biggerbold\"><img src=\"/images/bigCheck.gif\" border=\"0\" align=\"absmiddle\"/> " + strResult + "</span></p>";
                        else
                        {
                            if (boolSuccess == false)
                                strDone += "<p><span class=\"biggerbold\"><img src=\"/images/bigError.gif\" border=\"0\" align=\"absmiddle\"/> " + strError + "</span></p>";
                            else
                                strDone += "<p><span class=\"biggerbold\"><img src=\"/images/bigCheck.gif\" border=\"0\" align=\"absmiddle\"/> " + (strWorkflowTitle == "" ? oService.GetName(intService) : strWorkflowTitle) + " Submitted</span></p>";
                        }
                        oRequestItem.UpdateFormDone(intRequest, intItem, intNumber, 1);
                    }
                }
            }
        }
    }
}