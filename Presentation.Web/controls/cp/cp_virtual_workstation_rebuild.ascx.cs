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
    public partial class cp_virtual_workstation_rebuild : System.Web.UI.UserControl
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnRemote = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["RemoteDSN"]].ConnectionString;
        private string dsnServiceEditor = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ServiceEditorDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intResourceRequestApprove = Int32.Parse(ConfigurationManager.AppSettings["ResourceRequestApprove"]);
        protected int intAssignPage = Int32.Parse(ConfigurationManager.AppSettings["AssignWorkload"]);
        protected int intViewPage = Int32.Parse(ConfigurationManager.AppSettings["ViewWorkload"]);
     
        protected int intProfile = 0;
        protected string strDone = "";
        private Functions oFunction;
        private Users oUser;
        private Requests oRequest;
        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            RequestItems oRequestItem = new RequestItems(intProfile, dsn);
            RequestFields oRequestField = new RequestFields(intProfile, dsn);
            ServiceRequests oServiceRequest = new ServiceRequests(intProfile, dsn);
            Services oService = new Services(intProfile, dsn);
            oFunction = new Functions(intProfile, dsn, intEnvironment);
            oUser = new Users(intProfile, dsn);
            oRequest = new Requests(intProfile, dsn);
   
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
                        //string strResult = oService.GetName(intService) + " Completed";
                        string strResult = "";
                        string strError = oService.GetName(intService) + " Error";
                        // ********* BEGIN PROCESSING **************

                        Workstations oWorkstation = new Workstations(intProfile, dsn);
                        DataSet dsRebuild = oWorkstation.GetVirtualRebuild(intRequest, intService, intNumber);
                        bool found = false;
                        foreach (DataRow drRebuild in dsRebuild.Tables[0].Rows)
                        {
                            if (drRebuild["cancelled"].ToString() == "")
                            {
                                found = true;
                                int intWorkstation = Int32.Parse(drRebuild["workstationid"].ToString());
                                int intName = Int32.Parse(drRebuild["nameid"].ToString());
                                oWorkstation.UpdateVirtualRebuild(intRequest, intService, intNumber);
                                string strName = oWorkstation.GetName(intName);
                                string strPower = drRebuild["scheduled"].ToString();
                                strResult += "<p>The workstation " + strName + " was successfully queued for rebuild on " + strPower + "</p>";
                                strError = "";
                                break;
                            }
                        }
                        if (found == false)
                        {
                            strError = "<p>There was a problem configuring the workstation for rebuild ~ Request not found.</p>";
                        }

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
                        oRequestItem.UpdateFormDone(intRequest, intItem, intNumber, 1, 1);
                    }
                }
            }
        }
        /*
        private void SendServiceCenterNotification(int intRequestId, int intItemId, int intNumberId, int intAssignedTo)
        {
            string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
            string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
            string dsnZeus = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ZeusDSN"]].ConnectionString;
            string dsnServiceEditor = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ServiceEditorDSN"]].ConnectionString;

            string strVirtual = ConfigurationManager.AppSettings["VirtualGatekeeper"];
            int intAssignPage = Int32.Parse(ConfigurationManager.AppSettings["AssignWorkload"]);
            int intViewPage = Int32.Parse(ConfigurationManager.AppSettings["ViewWorkload"]);
            int intResourceRequestApprove = Int32.Parse(ConfigurationManager.AppSettings["ResourceRequestApprove"]);
            int intMyWork = Int32.Parse(ConfigurationManager.AppSettings["MyWork"]);
            int intImplementorDistributed = Int32.Parse(ConfigurationManager.AppSettings["ITEMID_IMPLEMENTOR_DISTRIBUTED"]);
            int intImplementorMidrange = Int32.Parse(ConfigurationManager.AppSettings["ITEMID_IMPLEMENTOR_MIDRANGE"]);
            int intWorkstationPlatform = Int32.Parse(ConfigurationManager.AppSettings["WorkstationPlatformID"]);
            int intTest = Int32.Parse(ConfigurationManager.AppSettings["TestClassID"]);
            int intUnder48A = Int32.Parse(ConfigurationManager.AppSettings["DR_HOUR_RESPONSE"]);
            int intUnder48Q = Int32.Parse(ConfigurationManager.AppSettings["DR_HOUR_QUESTION"]);
            int intStorageItem = Int32.Parse(ConfigurationManager.AppSettings["ITEMID_STORAGE"]);
            int intBackupItem = Int32.Parse(ConfigurationManager.AppSettings["ITEMID_BACKUP"]);
            int intStorageService = Int32.Parse(ConfigurationManager.AppSettings["SERVICEID_STORAGE"]);
            int intBackupService = Int32.Parse(ConfigurationManager.AppSettings["SERVICEID_BACKUP"]);
            int intServerPlatformID = Int32.Parse(ConfigurationManager.AppSettings["ServerPlatformID"]);
            string strHA = ConfigurationManager.AppSettings["FORECAST_HIGH_AVAILABILITY"];
            string strServiceCenterXID = ConfigurationManager.AppSettings["ServiceCenterInputXID"];
            bool boolUsePNCNaming = (ConfigurationManager.AppSettings["USE_PNC_NAMING"] == "1");

            PDFs oPDF = new PDFs(dsn, dsnAsset, dsnIP, intEnvironment);
            oPDF.CreateServerDecommSCRequest(intRequestId, intItemId, intNumberId, intAssignedTo);

        }
        */
    }
}