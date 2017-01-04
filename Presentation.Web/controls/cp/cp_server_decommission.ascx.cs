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
    public partial class cp_server_decommission : System.Web.UI.UserControl
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnRemote = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["RemoteDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intProfile = 0;
        protected string strDone = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            RequestItems oRequestItem = new RequestItems(intProfile, dsn);
            RequestFields oRequestField = new RequestFields(intProfile, dsn);
            ServiceRequests oServiceRequest = new ServiceRequests(intProfile, dsn);
            Services oService = new Services(intProfile, dsn);
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
                        Asset oAsset = new Asset(intProfile, dsnAsset);
                        Requests oRequest = new Requests(intProfile, dsn);
                        Users oUser = new Users(intProfile, dsn);
                        oAsset.UpdateDecommission(intRequest, intItem, intNumber, 1);
                        strResult += "<p>The server(s) were successfully queued for decommission</p>";
                        strError = "";
                        oRequest.AddResult(intRequest, intItem, intNumber, "Server Decommission", strError, strResult, intEnvironment, (oService.Get(intService, "notify_client") == "1"), oUser.GetName(intProfile));
                        if (strResult == "")
                            boolSuccess = false;
                        // ******** END PROCESSING **************
                        if (oService.Get(intService, "automate") == "1" && boolSuccess == true)
                            strDone += "<table border=\"0\"><tr><td valign=\"top\"><img src=\"/images/ico_check.gif\" border=\"0\" align=\"absmiddle\"/></td><td valign=\"top\" class=\"biggerbold\">" + strResult + "</td></tr></table>";
                        else
                        {
                            if (boolSuccess == false)
                                strDone += "<table border=\"0\"><tr><td valign=\"top\"><img src=\"/images/ico_error.gif\" border=\"0\" align=\"absmiddle\"/></td><td valign=\"top\" class=\"biggerbold\">" + strError + "</td></tr></table>";
                            else
                                strDone += "<table border=\"0\"><tr><td valign=\"top\"><img src=\"/images/ico_check.gif\" border=\"0\" align=\"absmiddle\"/></td><td valign=\"top\" class=\"biggerbold\">" + oService.GetName(intService) + " Submitted</td></tr></table>";
                        }
                        oRequestItem.UpdateFormDone(intRequest, intItem, intNumber, 1);
                    }
                }
            }
        }
    }
}