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
    public partial class cp_server_decommission_New : System.Web.UI.UserControl
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
                        string strResult = oService.GetName(intService) + " Completed";
                        string strError = oService.GetName(intService) + " Error";
                        // ********* BEGIN PROCESSING **************
                        bool boolIsServerVMWare = false;
                        bool boolIsNotManual = false;
                        int intServer = 0;
                        int intModel = 0;

                        Customized oCustomized = new Customized(intProfile, dsn);
                        DataSet dsDecomServer= oCustomized.GetDecommissionServer(intRequest,intItem,intNumber);
                        if (dsDecomServer.Tables[0].Rows.Count > 0)
                        {
                            intServer = Int32.Parse(dsDecomServer.Tables[0].Rows[0]["serverid"].ToString());
                            DateTime datPower = DateTime.Parse(dsDecomServer.Tables[0].Rows[0]["poweroff"].ToString());
                            string strPowerNew = dsDecomServer.Tables[0].Rows[0]["poweroff_new"].ToString();
                            string strName = dsDecomServer.Tables[0].Rows[0]["servername"].ToString();
                            Asset oAsset = new Asset(intProfile, dsnAsset);
                            Servers oServers = new Servers(intProfile, dsn);
                            DataSet dsAssets = oServers.GetAssets(intServer);

                            if (intServer > 0)
                            {
                                DataSet dsServer = oServers.Get(intServer);
                                ModelsProperties oModelsProperties = new ModelsProperties(intProfile, dsn);
                                if (dsServer.Tables[0].Rows[0]["modelid"] != DBNull.Value)
                                    intModel = Int32.Parse(dsServer.Tables[0].Rows[0]["modelid"].ToString());
                                if (intModel > 0 && oModelsProperties.IsTypeVMware(intModel) == true)
                                    boolIsServerVMWare = true;
                                boolIsNotManual = true;
                            }
                            // VMWARE
                            if (boolIsNotManual == true)
                            {
                                bool boolUnique = true;
                                foreach (DataRow drAsset in dsAssets.Tables[0].Rows)
                                {
                                    if (drAsset["latest"].ToString() == "1" || drAsset["dr"].ToString() == "1")
                                    {
                                        int intAsset = Int32.Parse(drAsset["assetid"].ToString());
                                        boolUnique = oAsset.AddDecommission(intRequest, intItem, intNumber, intAsset, intProfile, dsDecomServer.Tables[0].Rows[0]["reason"].ToString(), datPower, strName + (drAsset["dr"].ToString() == "1" ? "-DR" : ""), (drAsset["dr"].ToString() == "1" ? 1 : 0), strPowerNew);
                                        if (boolUnique == false)
                                            break;
                                    }
                                }
                                if (boolUnique == true || strPowerNew != "")
                                {
                                    oAsset.UpdateDecommission(intRequest, intItem, intNumber, 1);
                                    strResult += "<p>The server " + strName + " was successfully queued for decommission on " + (strPowerNew != "" ? strPowerNew : datPower.ToShortDateString()) + "</p>";
                                    strError = "";
                                }
                                else
                                {
                                    strError = "<p>The server " + strName + " has already been queued for decommission</p>";
                                }
                                //Send service center notification
                                //"Assignment: Complete and Close"
                                # region "Send Service Center Notification"
                                ResourceRequest oResourceRequest = new ResourceRequest(intProfile, dsn);
                                DataSet dsRR = oResourceRequest.GetResourceRequest(intRequest, intItem, intNumber);
                                int intRRId = 0;
                                if (dsRR.Tables[0].Rows.Count > 0)
                                    intRRId = Int32.Parse(dsRR.Tables[0].Rows[0]["id"].ToString());

                                //int intServerDecommServiceID = Int32.Parse(ConfigurationManager.AppSettings["SERVICEID_SERVER_DECOMMISSION"]);
                                //if (intService == intServerDecommServiceID)
                                //{
                                //    SendServiceCenterNotification(intRequest, intItem, intService, intNumber, 0);
                                //}
                                #endregion

                            }
                            else
                            {
                                // MANUAL (non-automated)
                                double dblHours = 0.00;
                                int intDevices = 1;
                                Field oField = new Field(intProfile, dsn);

                                strResult += "<p>The server decommission request was submitted for " + strName + ".</p>";
                                strError = "";

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
                                int intResource = oServiceRequest.AddRequest(intRequest, intItem, intService, intDevices, dblHours, 2, intNumber, dsnServiceEditor);
                                if (oServiceRequest.NotifyApproval(intResource, intResourceRequestApprove, intEnvironment, "", dsnServiceEditor) == false)
                                    oServiceRequest.NotifyTeamLead(intItem, intResource, intAssignPage, intViewPage, intEnvironment, "", dsnServiceEditor, dsnAsset, dsnIP, 0);

                                oRequest.AddResult(intRequest, intItem, intNumber, "Server Decommission", strError, strResult, intEnvironment, (oService.Get(intService, "notify_client") == "1"), oUser.GetName(intProfile));
                            }
                        }
                        else
                        {
                            strResult = "";
                            dsDecomServer = oCustomized.GetDecommissionServerDeleted(intRequest, intItem, intNumber);
                            string strName = "";
                            if (dsDecomServer.Tables[0].Rows.Count > 0)
                                strName = dsDecomServer.Tables[0].Rows[0]["servername"].ToString();
                            if (strName == "")
                                strError = "<p>One or more of the servers you attempted to decommission have already been queued for decommission</p>";
                            else
                                strError = "<p>The server " + strName + " has already been queued for decommission</p>";
                            oRequestItem.DeleteForms(intRequest, intService, intNumber);
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
                        oRequestItem.UpdateFormDone(intRequest, intItem, intNumber, 1,(boolIsNotManual?1:0));
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