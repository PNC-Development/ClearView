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
    public partial class cp_server_rebuild : System.Web.UI.UserControl
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

                        Servers oServer = new Servers(intProfile, dsn);
                        OnDemand oOnDemand = new OnDemand(intProfile, dsn);
                        Audit oAudit = new Audit(intProfile, dsn);
                        ServerName oServerName = new ServerName(intProfile, dsn);
                        ModelsProperties oModelsProperties = new ModelsProperties(intProfile, dsn);
                        Asset oAsset = new Asset(intProfile, dsnAsset, dsn);
                        DataSet dsRebuild = oServer.GetRebuild(intRequest, intService, intNumber);
                        bool found = false;
                        foreach (DataRow drRebuild in dsRebuild.Tables[0].Rows)
                        {
                            found = true;

                            int intServer = Int32.Parse(drRebuild["serverid"].ToString());
                            string strName = oServer.GetName(intServer, true);
                            int intModel = 0;
                            int intRebuildStep = 0;
                            // Load General Information
                            int intAsset = 0;
                            DataSet dsAssets = oServer.GetAssets(intServer);
                            foreach (DataRow drAsset in dsAssets.Tables[0].Rows)
                            {
                                if (drAsset["latest"].ToString() == "1")
                                {
                                    intAsset = Int32.Parse(drAsset["assetid"].ToString());
                                    break;
                                }
                            }
                            if (intAsset > 0)
                            {
                                // Asset Information
                                intModel = Int32.Parse(oAsset.Get(intAsset, "modelid"));
                            }
                            if (oModelsProperties.IsTypeVMware(intModel) == false)
                                intRebuildStep = 6;
                            else
                                intRebuildStep = 7;

                            oServer.UpdateStep(intServer, intRebuildStep);
                            // Redo step...delete current step and update the other step
                            oOnDemand.UpdateStepDoneServerRedo(intServer, intRebuildStep);
                            oServer.DeleteSwitchports(intServer);   // Reconfigure switchports
                            oServer.UpdateRebuilding(intServer, 1);
                            // Delete Audits
                            oAudit.DeleteServer(intServer, 0);
                            oAudit.DeleteServer(intServer, 1);
                            // Set installs back
                            DataSet dsInstalls = oServerName.GetComponentDetailSelected(intServer, 0);
                            foreach (DataRow drInstall in dsInstalls.Tables[0].Rows)
                                oServerName.UpdateComponentDetailSelected(intServer, Int32.Parse(drInstall["detailid"].ToString()), -2);
                            int intStepSkipStart = 0;
                            int intStepSkipGoto = 0;
                            oServer.UpdateStepSkip(intServer, intStepSkipStart, intStepSkipGoto);
                            strResult += "<p>The server " + strName + " was successfully queued for rebuild</p>";
                            strError = "";
                            break;
                        }
                        if (found == false)
                        {
                            strError = "<p>There was a problem configuring the server for rebuild ~ Request not found.</p>";
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
    }
}