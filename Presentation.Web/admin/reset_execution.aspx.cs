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
    public partial class reset_execution : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        private string dsnZeus = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ZeusDSN"]].ConnectionString;
        private string dsnServiceEditor = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ServiceEditorDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intConfidenceUnlock = Int32.Parse(ConfigurationManager.AppSettings["CONFIDENCE_UNLOCK"]);
        protected int intAssignPage = Int32.Parse(ConfigurationManager.AppSettings["AssignWorkload"]);
        protected int intResourceRequestApprove = Int32.Parse(ConfigurationManager.AppSettings["ResourceRequestApprove"]);
        protected int intViewPage = Int32.Parse(ConfigurationManager.AppSettings["ViewWorkload"]);
        protected string strIARB = ConfigurationManager.AppSettings["IARB"];
        protected int intStorageService = Int32.Parse(ConfigurationManager.AppSettings["SERVICEID_STORAGE"]);
        protected int intBackupService = Int32.Parse(ConfigurationManager.AppSettings["SERVICEID_BACKUP"]);
        protected int intImplementorDistributedService = Int32.Parse(ConfigurationManager.AppSettings["SERVICEID_IMPLEMENTOR_DISTRIBUTED"]);
        protected int intImplementorMidrangeService = Int32.Parse(ConfigurationManager.AppSettings["SERVICEID_IMPLEMENTOR_MIDRANGE"]);
        protected int intWorkstationPlatform = Int32.Parse(ConfigurationManager.AppSettings["WorkstationPlatformID"]);
        protected int intProfile;
        protected string strError = "";
        protected string strResults = "";
        protected int intCount = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cookies["loginreferrer"].Value = "/admin/reset_execution.aspx";
            Response.Cookies["loginreferrer"].Expires = DateTime.Now.AddDays(30);
            if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
            else
                Response.Redirect("/admin/login.aspx");
            btnGo.Attributes.Add("onclick", "return confirm('This function will modify production data and cannot be reversed!\\n\\nAre you sure you want to continue?') && ProcessButton(this);");
        }
        protected void btnGo_Click(Object Sender, EventArgs e)
        {
            Design oDesign = new Design(0, dsn);
            Log oLog = new Log(0, dsn);
            Forecast oForecast = new Forecast(0, dsn);
            Users oUser = new Users(0, dsn);
            int intID = Int32.Parse(txtID.Text);
            DataSet dsDesign = oDesign.Get(intID);
            if (dsDesign.Tables[0].Rows.Count > 0)
                Int32.TryParse(dsDesign.Tables[0].Rows[0]["answerid"].ToString(), out intID);
            if (oForecast.GetAnswer(intID).Tables[0].Rows.Count > 0)
            {
                ResourceRequest oResourceRequest = new ResourceRequest(0, dsn);
                int intForecast = Int32.Parse(oForecast.GetAnswer(intID, "forecastid"));
                int intRequestExecute = oForecast.GetRequestID(intID, true);
                int intRequest = Int32.Parse(oForecast.Get(intForecast, "requestid"));
                Requests oRequest = new Requests(0, dsn);
                ServiceRequests oServiceRequest = new ServiceRequests(0, dsn);
                int intProject = oRequest.GetProjectNumber(intRequest);
                Variables oVariable = new Variables(intEnvironment);
                Functions oFunction = new Functions(0, dsn, intEnvironment);
                Projects oProject = new Projects(0, dsn);
                Pages oPage = new Pages(0, dsn);
                Servers oServer = new Servers(0, dsn);
                Cluster oCluster = new Cluster(0, dsn);
                ServerName oServerName = new ServerName(0, dsn);
                Asset oAsset = new Asset(0, dsnAsset);
                IPAddresses oIPAddresses = new IPAddresses(intProfile, dsnIP, dsn);
                OnDemand oOnDemand = new OnDemand(0, dsn);
                ModelsProperties oModelsProperties = new ModelsProperties(0, dsn);
                Types oType = new Types(0, dsn);
                Storage oStorage = new Storage(0, dsn);
                OnDemandTasks oOnDemandTasks = new OnDemandTasks(0, dsn);
                VMWare oVMWare = new VMWare(0, dsn);
                PNCTasks oPNCTasks = new PNCTasks(0, dsn);
                Zeus oZeus = new Zeus(intProfile, dsnZeus);

                DataSet dsServers = oServer.GetRequests(intRequestExecute, 0);

                if (chkTimes.Checked == true && (chkExitAP.Checked == true || chkConfig.Checked == true || chkUnlock.Checked == true))
                    strError = "You can not and should not reset times and alter the design at the same time.  RESET PROVISIONING TIMES can not be selected with any other selections.";
                else
                {
                    oLog.AddEvent(intID, "", "", "Design " + intID.ToString() + " reset by " + oUser.GetFullNameWithLanID(intProfile), LoggingType.Information);
                    oForecast.AddReset(intID, -999);
                    if (chkTimes.Checked == true)
                    {
                        DateTime datTimes = DateTime.Today;
                        if (DateTime.TryParse(txtTimes.Text, out datTimes) == true)
                        {
                            // Update Design Dates
                            int intExecutedBy = 0;
                            Int32.TryParse(oForecast.GetAnswer(intID, "executed_by"), out intExecutedBy);
                            if (intExecutedBy > 0)
                            {
                                if (chkTimesForecastExecuted.Checked == true)
                                    oForecast.UpdateAnswerExecuted(intID, datTimes.ToString(), intExecutedBy);
                                if (chkTimesForecastCompleted.Checked == true)
                                    oForecast.UpdateAnswerCompleted(intID, datTimes.ToString());
                                if (chkTimesTasksStart.Checked == true)
                                {
                                    // Reset all pre-production online tasks (including storage, backup, etc...)
                                    DataSet dsTasks = oPNCTasks.GetStepsDesign(intID, 0, 0);
                                    foreach (DataRow drTask in dsTasks.Tables[0].Rows)
                                    {
                                        int intRRID = 0;
                                        if (Int32.TryParse(drTask["id"].ToString(), out intRRID) == true)
                                        {
                                            // Reset the started and completed (if not null)
                                            oOnDemandTasks.UpdateTime(intRRID, datTimes, chkTimesTasksCompleted.Checked);
                                        }
                                    }
                                }
                                DataSet dsAnswer = oServer.GetAnswer(intID);
                                foreach (DataRow drServer in dsAnswer.Tables[0].Rows)
                                {
                                    int intServer = Int32.Parse(drServer["id"].ToString());
                                    int intModel = Int32.Parse(drServer["modelid"].ToString());
                                    int intType = oModelsProperties.GetType(intModel);
                                    DataSet dsSteps = oOnDemand.GetSteps(intType, 1);
                                    // Update Server Dates
                                    if (oServer.Get(intServer, "build_started") != "")
                                    {
                                        if (chkTimesServerStart.Checked == true)
                                            oServer.UpdateBuildStarted(intServer, datTimes.ToString(), true);
                                        if (chkTimesServerSteps.Checked == true)
                                        {
                                            int intStep = 0;
                                            foreach (DataRow drStep in dsSteps.Tables[0].Rows)
                                            {
                                                intStep++;
                                                oOnDemand.UpdateStepDoneServer(intServer, intStep, datTimes);
                                            }
                                        }
                                    }

                                    if (chkTimesServerComplete.Checked == true)
                                        oServer.UpdateBuildCompleted(intServer, datTimes.ToString());

                                    if (chkTimesServerReady.Checked == true)
                                        oServer.UpdateBuildReady(intServer, datTimes.ToString(), true);
                                }
                            }
                            else
                                strError = "The design has not been executed";
                        }
                        else
                            strError = "Invalid DateTime";
                    }
                    if (chkExitAP.Checked == true)
                    {
                        // Delete the Manual Build Task
                        DataSet dsManual = oOnDemandTasks.GetGenericII(intID);
                        if (dsManual.Tables[0].Rows.Count > 0)
                        {
                            int intRequestID = Int32.Parse(dsManual.Tables[0].Rows[0]["requestid"].ToString());
                            int intItem = Int32.Parse(dsManual.Tables[0].Rows[0]["itemid"].ToString());
                            int intNumber = Int32.Parse(dsManual.Tables[0].Rows[0]["number"].ToString());
                            oOnDemandTasks.DeleteGenericII(intID);
                            DataSet dsParent = oResourceRequest.Get(intRequestID, intItem, intNumber);
                            if (dsParent.Tables[0].Rows.Count > 0)
                            {
                                int intRRID = Int32.Parse(dsParent.Tables[0].Rows[0]["parent"].ToString());
                                oResourceRequest.Delete(intRRID);
                                int intRRWID = Int32.Parse(dsParent.Tables[0].Rows[0]["id"].ToString());
                                oResourceRequest.DeleteWorkflow(intRRWID);
                            }
                        }
                        // Reset automated servers
                        foreach (DataRow drServer in dsServers.Tables[0].Rows)
                        {
                            int intServer = Int32.Parse(drServer["id"].ToString());
                            int intAsset = 0;
                            Int32.TryParse(drServer["assetid"].ToString(), out intAsset);

                            // Clear MDT / Altiris
                            oServer.ClearMDT(intServer, dsnAsset, dsnZeus, intEnvironment);
                            // Clear ZEUS
                            if (intAsset > 0)
                                oZeus.DeleteBuild(oAsset.Get(intAsset, "serial"));
                            // Clear Steps
                            oServer.UpdateStep(intServer, 0);
                            oOnDemand.DeleteStepDoneServers(intServer, 1);
                            // Clear Dates
                            oServer.UpdateBuildStarted(intServer, "", true);
                            oServer.UpdateBuildCompleted(intServer, "");
                            oServer.UpdateBuildReady(intServer, "", true);
                            // Clear Errors
                            DataSet dsError = oServer.GetErrors(intServer);
                            foreach (DataRow drError in dsError.Tables[0].Rows)
                            {
                                if (drError["fixed"].ToString() == "")
                                    oServer.UpdateError(intServer, Int32.Parse(drError["step"].ToString()), 0, 0, true, dsnAsset);
                            }
                            // Clear VMware
                            oVMWare.DeleteGuest(oServer.GetName(intServer, true));
                            oServiceRequest.Delete(intRequestExecute);

                            if (chkExitAP_Name.Checked == true && drServer["nameid"].ToString() != "")
                            {
                                int intName = Int32.Parse(drServer["nameid"].ToString());
                                oServer.UpdateServerNamed(intServer, 0);
                                oServerName.UpdateFactory(intName, 1);
                            }
                            if (chkExitAP_IP.Checked == true)
                            {
                                oServer.DeleteIP(intServer, 0, 0, 0, 0, dsnIP);
                            }
                            if (chkExitAP_Asset.Checked == true && intAsset > 0)
                            {
                                oServer.DeleteAsset(intServer);
                                oAsset.AddStatus(intAsset, "", (int)AssetStatus.Available, -100, DateTime.Now);
                            }
                        }
                        // Clear the execution and completion dates
                        oForecast.UpdateAnswerExecution(intID, "");
                        oForecast.UpdateAnswerExecuted(intID, "", 0);
                        oForecast.UpdateAnswerCompleted(intID, "");

                        // Clear the new design builder.
                        if (dsDesign.Tables[0].Rows.Count > 0)
                        {
                            int intDesign = Int32.Parse(dsDesign.Tables[0].Rows[0]["id"].ToString());
                            oForecast.DeleteAnswer(intID);
                            foreach (DataRow drServer in dsServers.Tables[0].Rows)
                            {
                                int intServer = Int32.Parse(drServer["id"].ToString());
                                oServer.Delete(intServer);
                            }
                            oDesign.UpdateAnswerId(intDesign, 0);
                            if (chkExitAP_Approvals.Checked == true)
                            {
                                oDesign.DeleteApproverFieldWorkflow(intDesign);
                                oDesign.DeleteApproverGroupWorkflow(intDesign, 0);
                                oDesign.DeleteApproverGroupWorkflow(intDesign, 1);
                                oDesign.DeleteSoftwareComponents(intDesign, true);
                                if (chkExitAP_Submissions.Checked == false)
                                {
                                    // Reset back to original requestor.
                                    oDesign.Approve(intDesign, 0, (oDesign.Get(intDesign, "is_exception") == "1"), intEnvironment, intImplementorDistributedService, intWorkstationPlatform, intImplementorMidrangeService, dsnAsset, dsnIP, dsnServiceEditor, intAssignPage, intResourceRequestApprove, intViewPage);
                                }
                            }
                            if (chkExitAP_Submissions.Checked == true)
                                oDesign.DeleteSubmitted(intDesign);
                        }
                    }
                    if (chkConfig.Checked == true)
                    {
                        int intModel = oForecast.GetModel(intID);
                        int intType = oModelsProperties.GetType(intModel);
                        string strExecute = oType.Get(intType, "forecast_execution_path");
                        int intCount = 0;
                        DataSet dsWizard = oOnDemand.GetWizardStepsDoneBack(intID);
                        foreach (DataRow drWizard in dsWizard.Tables[0].Rows)
                        {
                            intCount++;
                            if (intCount != dsWizard.Tables[0].Rows.Count)
                                oOnDemand.DeleteWizardStepDone(intID, Int32.Parse(drWizard["step"].ToString()));
                        }
                        if (chkConfig_Reset.Checked == true)
                            oForecast.UpdateAnswer(intID, "", "", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
                        if (chkConfig_Device.Checked == true)
                        {
                            foreach (DataRow drServer in dsServers.Tables[0].Rows)
                            {
                                int intServer = Int32.Parse(drServer["id"].ToString());
                                int intCluster = Int32.Parse(drServer["clusterid"].ToString());
                                oCluster.Delete(intCluster, intID);
                                oServer.Delete(intServer);
                            }
                        }
                        if (chkConfig_Storage.Checked == true)
                            oStorage.DeleteLuns(intID);
                    }
                    if (chkUnlock.Checked == true)
                    {
                        if (chkUnlock_80.Checked == true)
                        {
                            if (oForecast.GetAnswer(intID, "override") == "-1")
                            {
                                // Send Email to iARB stating that they do not have to approve the design
                                string[] strEmail;
                                char[] strSplit = { ';' };
                                strEmail = strIARB.Split(strSplit);
                                for (int ii = 0; ii < strEmail.Length; ii++)
                                {
                                    if (strEmail[ii].Trim() != "")
                                    {
                                        string strAddress = strEmail[ii];
                                        string strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DESIGN_BUILDER");
                                        oFunction.SendEmail("Review Board Approval UNLOCKED", strAddress, "", strEMailIdsBCC, "Review Board Approval UNLOCKED", "<p><b>An overridden design has been unlocked and no longer requires your approval.</b><p><p>" + oForecast.GetAnswerBody(intID, intEnvironment, dsnAsset, dsnIP) + "</p>", true, false);
                                    }
                                }
                            }
                            oForecast.AddAnswerUnlock(intID, 0, "Reset by administrator at client's request");
                            oForecast.UpdateAnswerConfidence(intID, intConfidenceUnlock);
                        }
                        oForecast.DeleteReset(intID);
                        oForecast.AddReset(intID, intRequestExecute);
                        oForecast.UpdateAnswer(intID, 0);
                        DataSet dsPending = oOnDemandTasks.GetPending(intID);
                        if (dsPending.Tables[0].Rows.Count > 0)
                        {
                            int intResourceWorkflow = Int32.Parse(dsPending.Tables[0].Rows[0]["resourceid"].ToString());
                            int intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);
                            int intImplementor = Int32.Parse(oResourceRequest.GetWorkflow(intResourceWorkflow, "userid"));
                            oResourceRequest.UpdateItemAndService(intResourceParent, -1, -1);
                            oResourceRequest.UpdateWorkflowName(intResourceWorkflow, "Auto-Provisioning Task [Reset]", 0);
                            oOnDemandTasks.DeleteAll(intID);
                            if (intImplementor > 0)
                            {
                                string strDefault = oUser.GetApplicationUrl(intImplementor, intViewPage);
                                string strSpacerRow = "<tr><td colspan=\"3\"><img src=\"" + oVariable.ImageURL() + "/images/spacer.gif\" border=\"0\" width=\"1\" height=\"7\" /></td></tr>";
                                string strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DESIGN_BUILDER");
                                if (strDefault == "")
                                    oFunction.SendEmail("RESET: ClearView Auto-Provisioning Reset", oUser.GetName(intImplementor), "", strEMailIdsBCC, "RESET: ClearView Auto-Provisioning Reset", "<p><b>An auto-provisioning request has been reset. This request no longer requires immediate attention.</b><p><p>" + oForecast.GetAnswerBody(intID, intEnvironment, dsnAsset, dsnIP) + "</p>", true, false);
                                else
                                    oFunction.SendEmail("RESET: ClearView Auto-Provisioning Reset", oUser.GetName(intImplementor), "", strEMailIdsBCC, "RESET: ClearView Auto-Provisioning Reset", "<p><b>An auto-provisioning request has been reset. This request no longer requires immediate attention.</b><p><p>" + oForecast.GetAnswerBody(intID, intEnvironment, dsnAsset, dsnIP) + "</p><p><a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=/" + strDefault + oPage.GetFullLink(intViewPage) + "?pid=" + intProject.ToString() + "\" target=\"_blank\">Click here to review this project.</a></p>", true, false);
                            }
                        }
                    }
                }

                if (strError == "")
                    lblDone.Text = "<img src='/images/bigcheck.gif' border='0' align='absmiddle'/> <b>Finished!</b> ClearView reset the design";
                else
                    lblDone.Text = "<img src='/images/bigerror.gif' border='0' align='absmiddle'/> <b>Error!</b> " + strError;
            }
            else if (dsDesign.Tables[0].Rows.Count > 0)
            {
                int intDesign = Int32.Parse(dsDesign.Tables[0].Rows[0]["id"].ToString());
                oLog.AddEvent(intDesign, "", "", "Design " + intDesign.ToString() + " reset by " + oUser.GetFullNameWithLanID(intProfile), LoggingType.Information);
                oDesign.UpdateAnswerId(intDesign, 0);
                if (chkExitAP_Approvals.Checked == true)
                {
                    oDesign.DeleteApproverFieldWorkflow(intDesign);
                    oDesign.DeleteApproverGroupWorkflow(intDesign, 0);
                    oDesign.DeleteApproverGroupWorkflow(intDesign, 1);
                    oDesign.DeleteSoftwareComponents(intDesign, true);
                    if (chkExitAP_Submissions.Checked == false)
                    {
                        // Reset back to original requestor.
                        oDesign.Approve(intDesign, 0, (oDesign.Get(intDesign, "is_exception") == "1"), intEnvironment, intImplementorDistributedService, intWorkstationPlatform, intImplementorMidrangeService, dsnAsset, dsnIP, dsnServiceEditor, intAssignPage, intResourceRequestApprove, intViewPage);
                    }
                }
                if (chkExitAP_Submissions.Checked == true)
                    oDesign.DeleteSubmitted(intDesign);
                lblDone.Text = "<img src='/images/bigAlert.gif' border='0' align='absmiddle'/> <b>Done!</b> ClearView reset the new design builder";
            }
            else
                lblDone.Text = "<img src='/images/bigerror.gif' border='0' align='absmiddle'/> <b>Error!</b> ClearView could not find the design";
        }
    }
}
