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
    public partial class cp_iis : System.Web.UI.UserControl
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
        protected string strVirtual = ConfigurationManager.AppSettings["VirtualGatekeeper"];
        protected int intWorkloadPage = Int32.Parse(ConfigurationManager.AppSettings["ViewWorkload"]);
        protected int intResourceRequest = Int32.Parse(ConfigurationManager.AppSettings["ResourceRequest"]);
        protected void Page_Load(object sender, EventArgs e)
        {
            RequestItems oRequestItem = new RequestItems(intProfile, dsn);
            RequestFields oRequestField = new RequestFields(intProfile, dsn);
            ServiceRequests oServiceRequest = new ServiceRequests(intProfile, dsn);
            Services oService = new Services(intProfile, dsn);
            ServiceDetails oServiceDetail = new ServiceDetails(intProfile, dsn);
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
                        string strResult = oService.GetName(intService) + " Completed";
                        string strError = oService.GetName(intService) + " Error";
                        // ********* BEGIN PROCESSING **************
                        Requests oRequest = new Requests(intProfile, dsn);
                        Functions oFunction = new Functions(intProfile, dsn, intEnvironment);
                        Variables oVariable = new Variables(intEnvironment);
                        Users oUser = new Users(intProfile, dsn);
                        Pages oPage = new Pages(intProfile, dsn);
                        if (oRequest.Get(intRequest, "description") == "")
                        {
                            Customized oCustomized = new Customized(intProfile, dsn);
                            DataSet dsStatement = oCustomized.GetIIS(intRequest, intItem, intNumber);
                            if (dsStatement.Tables[0].Rows.Count > 0)
                                oRequest.UpdateDescription(intRequest, dsStatement.Tables[0].Rows[0]["statement"].ToString());
                        }
                        int intDevices = 0;
                        double dblQuantity = 0.00;
                        DataSet ds = oService.GetSelected(intRequest, intService);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            dblQuantity = double.Parse(ds.Tables[0].Rows[0]["quantity"].ToString());
                            intDevices = Int32.Parse(ds.Tables[0].Rows[0]["quantity"].ToString());
                        }
                        double dblHours = oServiceDetail.GetHours(intService, dblQuantity);
                        int intResource = oServiceRequest.AddRequest(intRequest, intItem, intService, intDevices, dblHours, 2, intNumber, dsnServiceEditor);
                        if (oService.Get(intService, "typeid") == "3")
                        {
                        }
                        else if (oService.Get(intService, "typeid") == "2")
                        {
                        }
                        else
                        {
                            if (oServiceRequest.NotifyApproval(intResource, intResourceRequestApprove, intEnvironment, "", dsnServiceEditor) == false)
                                oServiceRequest.NotifyTeamLead(intItem, intResource, intAssignPage, intViewPage, intEnvironment,  "", dsnServiceEditor, dsnAsset, dsnIP, 0);
                        }
                        // ******** END PROCESSING **************
                        if (oService.Get(intService, "automate") == "1" && boolSuccess == true)
                            strDone += "<p><span class=\"biggerbold\"><img src=\"/images/bigCheck.gif\" border=\"0\" align=\"absmiddle\"/> " + strResult + "</span></p>";
                        else
                        {
                            if (boolSuccess == false)
                                strDone += "<p><span class=\"biggerbold\"><img src=\"/images/bigError.gif\" border=\"0\" align=\"absmiddle\"/> " + strError + "</span></p>";
                            else
                                strDone += "<p><span class=\"biggerbold\"><img src=\"/images/bigCheck.gif\" border=\"0\" align=\"absmiddle\"/> " + oService.GetName(intService) + " Submitted</span></p>";
                        }
                        oRequestItem.UpdateFormDone(intRequest, intItem, intNumber, 1);
                    }
                }
            }
        }
    }
}