using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Text;
using NCC.ClearView.Application.Core.ClearViewWS;
using System.Threading;
using System.DirectoryServices;
using System.IO;

namespace NCC.ClearView.Application.Core
{
    public class ServerDecommission
    {
        private string dsn = "";
        private int user = 0;
        private SqlParameter[] arParams;

        private Services oService;
        private ServiceRequests oServiceRequest;
        private ServiceDetails oServiceDetail;
        private ServerName oServerName;

        public ServerDecommission(int _user, string _dsn)
        {
            user = _user;
            dsn = _dsn;
        }

        public void InitiateDecom(int intServerid, int intModel, string strName, int intRequest, int intItem, int intNumber,
                                  int intIsSAN, int intIsLTM,
                                  int intAssignPage, int intViewPage,
                                  int intEnvironment,
                                  int intIMDecommServiceId,
                                  string dsnServiceEditor, string dsnAsset, string dsnIP, string strDSMADMC, bool MissedFix)
        {
            // *** CALLED DURING THE DESTROY PHASE OR DURING MANUAL DECOM PROCESS
            int intOS = 0;
            int intServerNameId = 0;
            int intForecastAnswerid = 0;

            bool boolIsServerVMWare = false;
            bool boolIsServerHasBackup = false;
            bool boolIsPNCServer = false;
            bool boolIsDistributed = false;

            oService = new Services(user, dsn);
            oServiceRequest = new ServiceRequests(user, dsn);
            oServiceDetail = new ServiceDetails(user, dsn);
            oServerName = new ServerName(user, dsn);

            Servers oServers = new Servers(user, dsn);
            DataSet dsServer = oServers.Get(intServerid);
            ModelsProperties oModelsProperties = new ModelsProperties(user, dsn);
            OperatingSystems oOperatingSystem = new OperatingSystems(user, dsn);
            Forecast oForecast = new Forecast(user, dsn);
            ResourceRequest oResourceRequest = new ResourceRequest(user, dsn);
            PNCTasks oPNCTask = new PNCTasks(user, dsn);
            Storage oStorage = new Storage(user, dsn);
            TSM oTSM = new TSM(user, dsn);
            Log oLog = new Log(user, dsn);

            if (dsServer.Tables[0].Rows.Count > 0)
            {
                if (intModel == 0 && dsServer.Tables[0].Rows[0]["modelid"] != DBNull.Value)
                    intModel = Int32.Parse(dsServer.Tables[0].Rows[0]["modelid"].ToString());
                if (dsServer.Tables[0].Rows[0]["osid"] != DBNull.Value)
                    intOS = Int32.Parse(dsServer.Tables[0].Rows[0]["osid"].ToString());

                boolIsPNCServer = (dsServer.Tables[0].Rows[0]["PNC"] != DBNull.Value ? (dsServer.Tables[0].Rows[0]["PNC"].ToString() == "1" ? true : false) : false);
                boolIsServerHasBackup = (Int32.Parse(dsServer.Tables[0].Rows[0]["tsm_schedule"].ToString()) > 0 ? true : false);
                if (dsServer.Tables[0].Rows[0]["nameid"] != DBNull.Value)
                    intServerNameId = Int32.Parse(dsServer.Tables[0].Rows[0]["nameid"].ToString());
                if (dsServer.Tables[0].Rows[0]["answerid"] != DBNull.Value)
                    intForecastAnswerid = Int32.Parse(dsServer.Tables[0].Rows[0]["answerid"].ToString());
                boolIsDistributed = ((intOS > 0 && oOperatingSystem.IsDistributed(intOS)) || (oForecast.IsOSDistributed(intForecastAnswerid)));

                if (intIsLTM == 0 && oForecast.IsHACSM(intForecastAnswerid) == true)
                    intIsLTM = 1;
            }

            if (intModel > 0 && oModelsProperties.IsTypeVMware(intModel) == true)
                boolIsServerVMWare = true;

            // Release server name
            if (intServerNameId > 0)
            {
                if (boolIsPNCServer == true)
                {
                    //Release the server name for PNC
                    strName = oServerName.GetNameFactory(intServerNameId, 0);
                    oServerName.UpdateFactory(intServerNameId, 1);
                }
                else
                {
                    //Release the server name for NCB
                    strName = oServerName.GetName(intServerNameId, 0);
                    oServerName.Update(intServerNameId, 1);
                }
                oLog.AddEvent(strName, "InitiateDecom", "The server name has been released", LoggingType.Information);
            }

            if (oStorage.GetStorageDW(strName).Tables[0].Rows.Count > 0)
                intIsSAN = 1;
            else if (intIsSAN == 0 && boolIsServerVMWare == false && strName.Trim() != "")
            {
                if (oForecast.IsStorage(intForecastAnswerid) == true && oForecast.GetAnswer(intForecastAnswerid, "storage") == "1")
                    intIsSAN = 1;
            }

            DataSet dsDecom = oPNCTask.Gets((boolIsPNCServer ? 1 : 0), (boolIsPNCServer ? 0 : 1), (boolIsDistributed ? 1 : 0), (boolIsDistributed ? 0 : 1), 1, 1);
            bool boolIM = true;
            foreach (DataRow drDecom in dsDecom.Tables[0].Rows)
            {
                bool boolProcess = false;
                if (drDecom["storage"].ToString() == "1")
                {
                    // Generate SAN Task if storage exists
                    if (intIsSAN == 1)
                        boolProcess = true;
                }
                else if (drDecom["if_ltm_install"].ToString() == "1")
                {
                    // Generate WAN Task if CSM exists
                    if (intIsLTM == 1)
                        boolProcess = true;
                }
                else if (drDecom["tsm"].ToString() == "1")
                {
                    // Generate TSM Task if backup exists
                    if (boolIsServerHasBackup == true && strDSMADMC == "")
                        boolProcess = true;
                }
                else
                {
                    // If not SAN, WAN, or LTM
                    boolProcess = true;
                }

                if (boolProcess == true)
                {
                    oLog.AddEvent(strName, "InitiateDecom", drDecom["name"].ToString() + " task to be generated", LoggingType.Debug);
                    int intService = Int32.Parse(drDecom["serviceid"].ToString());
                    if (oResourceRequest.GetAllService(intRequest, intService, intNumber).Tables[0].Rows.Count == 0)
                    {
                        boolIM = false;
                        int intServiceItemId = oService.GetItemId(intService);
                        double dblServiceHours = oServiceDetail.GetHours(intService, 1);
                        int intResource = oServiceRequest.AddRequest(intRequest, intServiceItemId, intService, 1, dblServiceHours, 2, intNumber, dsnServiceEditor);
                        oServiceRequest.NotifyTeamLead(intServiceItemId, intResource, intAssignPage, intViewPage, intEnvironment, "", dsnServiceEditor, dsnAsset, dsnIP, 0);
                        oLog.AddEvent(strName, "InitiateDecom", drDecom["name"].ToString() + " sent", LoggingType.Debug);
                    }
                }
            }

            if (boolIM == true)
            {
                oLog.AddEvent(strName, "InitiateDecom", "All decommission tasks have been completed", LoggingType.Information);
                // No other tasks to submit, so submit IM request
                VerifyDecomCompletionAndNofity(intRequest, intItem, intNumber,
                                    intAssignPage, intViewPage, intEnvironment,
                                    intIMDecommServiceId,
                                    dsnServiceEditor, dsnAsset, dsnIP);
            }

            if (intServerid > 0)
            {
                // Set ASSET to Decommission
                Asset oAsset = new Asset(0, dsnAsset);
                DataSet dsAsset = oServers.GetAssets(intServerid);
                oLog.AddEvent(strName, "InitiateDecom", "Setting " + dsAsset.Tables[0].Rows.Count.ToString() + " to decommissioned", LoggingType.Information);
                foreach (DataRow drAsset in dsAsset.Tables[0].Rows)
                {
                    int intAsset = Int32.Parse(drAsset["assetid"].ToString());
                    int intStatus = 0;
                    if (Int32.TryParse(oAsset.GetStatus(intAsset, "status"), out intStatus) == true)
                    {
                        if (intStatus != (int)AssetStatus.Decommissioned && intStatus != (int)AssetStatus.Disposed)
                        {
                            if (drAsset["dr"].ToString() == "1")
                                oAsset.AddStatus(intAsset, strName + "-DR", (int)AssetStatus.Decommissioned, -999, DateTime.Now);
                            else if (drAsset["latest"].ToString() == "1")
                                oAsset.AddStatus(intAsset, strName, (int)AssetStatus.Decommissioned, -999, DateTime.Now);
                            oLog.AddEvent(strName, "InitiateDecom", "Setting Asset " + oAsset.Get(intAsset, "serial") + " to Decommissioned", LoggingType.Information);
                        }
                    }
                }

                Functions oFunction = new Functions(user, dsn, intEnvironment);
                string strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_WARNING");

                if (MissedFix == false)
                {
                    // Release IP Address(es)
                    IPAddresses oIPAddresses = new IPAddresses(0, dsnIP, dsn);
                    DataSet dsIP = oServers.GetIP(intServerid, 0, 0, 0, 0);
                    oLog.AddEvent(strName, "InitiateDecom", "Releasing " + dsIP.Tables[0].Rows.Count.ToString() + " IP addresses", LoggingType.Information);
                    foreach (DataRow drIP in dsIP.Tables[0].Rows)
                    {
                        int intIPAssign = Int32.Parse(drIP["ipAddressID"].ToString());
                        oIPAddresses.UpdateAvailable(intIPAssign, 1);
                        oLog.AddEvent(strName, "InitiateDecom", "Releasing IP Address " + oIPAddresses.GetName(intIPAssign, 0), LoggingType.Information);
                        // Notify
                        if (intIPAssign > 0)
                        {
                            bool boolAvailable = (oIPAddresses.Get(intIPAssign, "available") == "1");
                            int intNetwork = 0;
                            Int32.TryParse(oIPAddresses.Get(intIPAssign, "networkid"), out intNetwork);
                            string strNotify = "";
                            if (intNetwork > 0)
                                strNotify = oIPAddresses.GetNetwork(intNetwork, "notify");
                            else
                                strNotify = oIPAddresses.GetNetworkNotifications(intIPAssign);
                            // 9/6/2011: Changed to notify on every IP address.
                            //if (boolAvailable == true && strNotify != "")
                            // 1/29/14, per Peacock (CVT106817) - no longer notify on every IP address
                            //if (boolAvailable == true)
                            strNotify = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DECOM_IP").Trim();
                            if (boolAvailable == true && strNotify != "")
                            {
                                // Send notification(s)
                                strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_IPADDRESS");
                                oFunction.SendEmail("IP Decommission", strNotify, "", strEMailIdsBCC, "IP Decommission", oIPAddresses.GetName(intIPAssign, 0), false, true);
                                oLog.AddEvent(strName, "InitiateDecom", "IP Notification sent for " + oIPAddresses.GetName(intIPAssign, 0) + " to (" + strNotify + ")", LoggingType.Information);
                            }
                            else
                                oLog.AddEvent(strName, "InitiateDecom", "IP Notification skipped for " + oIPAddresses.GetName(intIPAssign, 0) + " (A = " + boolAvailable.ToString() + ", N = " + strNotify + ")", LoggingType.Information);
                        }
                    }

                    strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_WARNING");
                    // Unregister TSM
                    DataSet dsTSM = oServers.GetTSM(strName);
                    if (strDSMADMC != "" && dsTSM.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow drTSM in dsTSM.Tables[0].Rows)
                        {
                            oLog.AddEvent(strName, "InitiateDecom", "Decommission from TSM starting...", LoggingType.Information);

                            // Add Decom Information for retrieval later
                            oTSM.AddDecom(drTSM["name1"].ToString(), drTSM["server"].ToString(), drTSM["port"].ToString(), drTSM["domain"].ToString(), drTSM["schedule"].ToString(), drTSM["contacts"].ToString());

                            // Perform Decommission
                            DateTime _now = DateTime.Now;
                            string strNow = _now.Day.ToString() + _now.Month.ToString() + _now.Year.ToString() + _now.Hour.ToString() + _now.Minute.ToString() + _now.Second.ToString() + _now.Millisecond.ToString();
                            string strFile = strDSMADMC + strName + "_" + strNow;
                            StreamWriter oMacro = new StreamWriter(strFile + ".mac");
                            // Update Contact
                            string strContacts = drTSM["contacts"].ToString().Trim();
                            if (strContacts != "")
                                strContacts += " /";
                            strContacts += "DECOM_" + _now.ToString("yyyyMMdd");     // FORMAT = 20101225 for 12/25/2010
                            oMacro.WriteLine("UPDATE NODE " + drTSM["name1"].ToString() + " CONTACT=\"" + strContacts + "\"");
                            oMacro.WriteLine("DEL ASSOC " + drTSM["domain"].ToString() + " " + drTSM["schedule"].ToString() + " " + drTSM["name1"].ToString());
                            oMacro.Flush();
                            oMacro.Close();
                            StreamWriter oBatch = new StreamWriter(strFile + ".bat");
                            // FORMAT: "E:\Program Files\Tivoli\TSM\baclient\dsmadmc.exe" -tcps=BKPSRV-OC-T2 -tcpp=1500 -id=clearview -password=clearview macro /full/pathname/macro_filename > /full/pathname/outputfile.txt
                            string strScript = "\"" + strDSMADMC + "dsmadmc.exe\" -tcps=" + drTSM["server"].ToString() + " -tcpp=" + drTSM["port"].ToString() + " -id=clearview -password=clearview macro \"" + strFile + ".mac\" > \"" + strFile + ".txt\"";
                            //oEventLog.WriteEntry(String.Format(strName + ": " + "TSM Registration Script: " + strScript), EventLogEntryType.FailureAudit);
                            oBatch.WriteLine(strScript);
                            oBatch.Flush();
                            oBatch.Close();

                            System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo("cmd.exe");
                            info.WorkingDirectory = strDSMADMC;
                            info.Arguments = "/c \"" + strFile + ".bat\"";
                            System.Diagnostics.Process proc = System.Diagnostics.Process.Start(info);
                            bool boolTimeout = false;
                            int intTimeoutDefault = (2 * 60 * 1000);    // 2 minutes
                            proc.WaitForExit(intTimeoutDefault);
                            if (proc.HasExited == false)
                            {
                                proc.Kill();
                                boolTimeout = true;
                            }
                            proc.Close();
                            if (boolTimeout == false)
                            {
                                bool boolContent = false;
                                for (int ii = 0; ii < 10 && boolContent == false; ii++)
                                {
                                    if (File.Exists(strFile + ".txt") == true)
                                    {
                                        string strContent = "";
                                        try
                                        {
                                            StreamReader oReader = new StreamReader(strFile + ".txt");
                                            strContent = oReader.ReadToEnd();
                                            if (strContent != "")
                                            {
                                                boolContent = true;
                                                oReader.Close();
                                                oLog.AddEvent(strName, "InitiateDecom", "Decommission from TSM completed", LoggingType.Information);
                                                // Delete Files
                                                if (strContent.Contains("Highest return code was 0") || strContent.Contains("Highest return code was 10"))
                                                {
                                                    if (File.Exists(strFile + ".bat") == true)
                                                        File.Delete(strFile + ".bat");
                                                    if (File.Exists(strFile + ".mac") == true)
                                                        File.Delete(strFile + ".mac");
                                                    if (File.Exists(strFile + ".txt") == true)
                                                        File.Delete(strFile + ".txt");
                                                }
                                                else
                                                {
                                                    strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_ERROR");
                                                    oFunction.SendEmail("ERROR: TSM Decommission Script Error", strEMailIdsBCC, "", "", "ERROR: TSM Decommission Script Error", "<p><b>An error occurred when attempting to auto-decommission server " + strName + " in TSM...</b></p><p>Output:" + strContent + "</p>", true, false);
                                                }
                                                //break;
                                            }
                                            else
                                            {
                                                oReader.Close();
                                                Thread.Sleep(5000);
                                            }
                                        }
                                        catch
                                        {
                                            Thread.Sleep(5000);
                                        }
                                    }
                                    else
                                    {
                                        Thread.Sleep(5000);
                                    }
                                }
                                if (boolContent == false)
                                {
                                    strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_ERROR");
                                    oFunction.SendEmail("ERROR: TSM Decommission Error", strEMailIdsBCC, "", "", "ERROR: TSM Decommission Error", "<p><b>An error occurred when attempting to auto-decommission server " + strName + " in TSM...</b></p><p>Script:" + strFile + ".vbs" + "</p>", true, false);
                                }
                            }
                            else
                            {
                                strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_ERROR");
                                oFunction.SendEmail("ERROR: TSM Decommission Timeout", strEMailIdsBCC, "", "", "ERROR: TSM Decommission Timeout", "<p><b>A timeout occurred when attempting to auto-decommission server " + strName + " in TSM...</b></p><p>Script:" + strFile + ".vbs" + "</p>", true, false);
                            }
                        }
                    }
                    else
                        oLog.AddEvent(strName, "InitiateDecom", "Skipping TSM Decommission", LoggingType.Information);

                    Variables oVariable = new Variables(intEnvironment);
                    System.Net.NetworkCredential oCredentials = new System.Net.NetworkCredential(oVariable.ADUser(), oVariable.ADPassword(), oVariable.Domain());
                    ClearViewWebServices oWebService = new ClearViewWebServices();
                    oWebService.Timeout = Timeout.Infinite;
                    oWebService.Credentials = oCredentials;
                    oWebService.Url = oVariable.WebServiceURL();

                    // Remove from Avamar Grid
                    Avamar oAvamar = new Avamar(0, dsn);
                    DataSet dsGroups = oAvamar.GetDecoms(strName);
                    if (dsGroups.Tables[0].Rows.Count > 0)
                    {
                        AvamarRegistration oAvamarRegistration = new AvamarRegistration(0, dsn);
                        string client = dsGroups.Tables[0].Rows[0]["client"].ToString();
                        string grid = dsGroups.Tables[0].Rows[0]["grid"].ToString();
                        string domain = dsGroups.Tables[0].Rows[0]["domain"].ToString();
                        oLog.AddEvent(strName, "InitiateDecom", "Retiring client from the avamar grid (" + grid + ")", LoggingType.Information);
                        AvamarReturnType retire = oAvamarRegistration.API(oWebService.DeleteAvamarClient(grid, domain, client));
                        if (retire.Error == false)
                            oLog.AddEvent(strName, "InitiateDecom", "Client has been retired from the grid (" + grid + ")", LoggingType.Information);
                        else
                            oLog.AddEvent(strName, "InitiateDecom", "There was a problem retiring the client from the grid (" + grid + ") - " + retire.Message, LoggingType.Error);
                    }
                    else
                        oLog.AddEvent(strName, "InitiateDecom", "This server was not decommissioned using the ClearView automated process", LoggingType.Information);


                    strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_WARNING");

                    // Delete from DNS
                    string strWebServiceResult = "N / A";
                    Settings oSetting = new Settings(0, dsn);

                    // 3: <name>-backup
                    if (boolIsPNCServer == true)
                    {
                        if (oSetting.IsDNS_QIP() == true)
                        {
                            try
                            {
                                strWebServiceResult = oWebService.DeleteDNSforPNC("", strName + "-BACKUP", user, true);
                            }
                            catch (Exception eDNS)
                            {
                                strWebServiceResult = eDNS.Message;
                            }

                            if (strWebServiceResult != "SUCCESS" && strWebServiceResult != "***NOTFOUND")
                                oFunction.SendEmail("Decommission Result - DNS", strEMailIdsBCC, "", "", "Decommission Result - DNS", "<p>Here are the results of the automated decommission from PNC DNS via QIP for " + strName + "-BACKUP" + "...</p><p>" + strWebServiceResult + "</p>", true, false);
                            oLog.AddEvent(strName + "-BACKUP", "InitiateDecom", "DNS Record deleted from QIP ~ " + strWebServiceResult, LoggingType.Information);
                        }
                    }
                    else
                        oLog.AddEvent(strName + "-BACKUP", "InitiateDecom", "DNS Record for QIP skipped since it is not a PNC server", LoggingType.Information);

                    if (oSetting.IsDNS_Bluecat() == true && CheckBluecat(strName + "-BACKUP", oWebService, oLog) == true)
                    {
                        try
                        {
                            strWebServiceResult = oWebService.DeleteBluecatDNS("", strName + "-BACKUP", false, true);
                        }
                        catch (Exception eDNS)
                        {
                            strWebServiceResult = eDNS.Message;
                        }

                        if (strWebServiceResult != "SUCCESS" && strWebServiceResult != "***NOTFOUND")
                            oFunction.SendEmail("Decommission Result - DNS", strEMailIdsBCC, "", "", "Decommission Result - DNS", "<p>Here are the results of the automated decommission from PNC DNS via BlueCat for " + strName + "-BACKUP" + "...</p><p>" + strWebServiceResult + "</p>", true, false);
                        oLog.AddEvent(strName + "-BACKUP", "InitiateDecom", "DNS Record deleted from BlueCat ~ " + strWebServiceResult, LoggingType.Information);
                    }


                    // 2: <name>-rm
                    if (boolIsPNCServer == true)
                    {
                        if (oSetting.IsDNS_QIP() == true)
                        {
                            try
                            {
                                strWebServiceResult = oWebService.DeleteDNSforPNC("", strName + "-RM", user, true);
                            }
                            catch (Exception eDNS)
                            {
                                strWebServiceResult = eDNS.Message;
                            }

                            if (strWebServiceResult != "SUCCESS" && strWebServiceResult != "***NOTFOUND")
                                oFunction.SendEmail("Decommission Result - DNS", strEMailIdsBCC, "", "", "Decommission Result - DNS", "<p>Here are the results of the automated decommission from PNC DNS via QIP for " + strName + "-RM" + "...</p><p>" + strWebServiceResult + "</p>", true, false);
                            oLog.AddEvent(strName + "-RM", "InitiateDecom", "DNS Record deleted from QIP ~ " + strWebServiceResult, LoggingType.Information);
                        }
                    }
                    else
                        oLog.AddEvent(strName + "-RM", "InitiateDecom", "DNS Record for QIP skipped since it is not a PNC server", LoggingType.Information);

                    if (oSetting.IsDNS_Bluecat() == true && CheckBluecat(strName + "-RM", oWebService, oLog) == true)
                    {
                        try
                        {
                            strWebServiceResult = oWebService.DeleteBluecatDNS("", strName + "-RM", false, true);
                        }
                        catch (Exception eDNS)
                        {
                            strWebServiceResult = eDNS.Message;
                        }

                        if (strWebServiceResult != "SUCCESS" && strWebServiceResult != "***NOTFOUND")
                            oFunction.SendEmail("Decommission Result - DNS", strEMailIdsBCC, "", "", "Decommission Result - DNS", "<p>Here are the results of the automated decommission from PNC DNS via BlueCat for " + strName + "-RM" + "...</p><p>" + strWebServiceResult + "</p>", true, false);
                        oLog.AddEvent(strName + "-RM", "InitiateDecom", "DNS Record deleted from BlueCat ~ " + strWebServiceResult, LoggingType.Information);
                    }


                    // 1: <name>
                    if (boolIsPNCServer == true)
                    {
                        if (oSetting.IsDNS_QIP() == true)
                        {
                            try
                            {
                                strWebServiceResult = oWebService.DeleteDNSforPNC("", strName, user, true);
                            }
                            catch (Exception eDNS)
                            {
                                strWebServiceResult = eDNS.Message;
                            }

                            if (strWebServiceResult != "SUCCESS" && strWebServiceResult != "***NOTFOUND")
                                oFunction.SendEmail("Decommission Result - DNS", strEMailIdsBCC, "", "", "Decommission Result - DNS", "<p>Here are the results of the automated decommission from PNC DNS via QIP for " + strName + "...</p><p>" + strWebServiceResult + "</p>", true, false);
                            oLog.AddEvent(strName, "InitiateDecom", "DNS Record deleted from QIP ~ " + strWebServiceResult, LoggingType.Information);
                        }
                    }
                    else
                        oLog.AddEvent(strName, "InitiateDecom", "DNS Record for QIP skipped since it is not a PNC server", LoggingType.Information);

                    if (oSetting.IsDNS_Bluecat() == true && CheckBluecat(strName, oWebService, oLog) == true)
                    {
                        try
                        {
                            strWebServiceResult = oWebService.DeleteBluecatDNS("", strName, false, true);
                        }
                        catch (Exception eDNS)
                        {
                            strWebServiceResult = eDNS.Message;
                        }

                        if (strWebServiceResult != "SUCCESS" && strWebServiceResult != "***NOTFOUND")
                            oFunction.SendEmail("Decommission Result - DNS", strEMailIdsBCC, "", "", "Decommission Result - DNS", "<p>Here are the results of the automated decommission from PNC DNS via BlueCat for " + strName + "...</p><p>" + strWebServiceResult + "</p>", true, false);
                        oLog.AddEvent(strName, "InitiateDecom", "DNS Record deleted from BlueCat ~ " + strWebServiceResult, LoggingType.Information);
                    }


                    // Remove computer object
                    oLog.AddEvent(strName, "InitiateDecom", "Removing computer object", LoggingType.Debug);
                    Domains oDomain = new Domains(0, dsn);
                    int intDomain = 0;
                    int intDomainEnvironment = 0;
                    Int32.TryParse(dsServer.Tables[0].Rows[0]["domainid"].ToString(), out intDomain);
                    Int32.TryParse(oDomain.Get(intDomain, "environment"), out intDomainEnvironment);
                    if (intDomainEnvironment == 0)
                        intDomainEnvironment = 999;
                    AD oAD = new AD(0, dsn, intDomainEnvironment);
                    string strAD = "";
                    try
                    {
                        SearchResultCollection oComputers = oAD.ComputerSearch(strName);
                        if (oComputers.Count == 1)
                            strAD = oAD.Delete(oComputers[0].GetDirectoryEntry());
                        else if (oComputers.Count > 1)
                            strAD = "More than one (1) computer objects were found in PNC Active Directory";

                    }
                    catch (Exception eAD)
                    {
                        strAD = "ERROR: " + eAD.Message + " (Source: " + eAD.Source + ") (Stack Trace: " + eAD.StackTrace + ")";
                    }
                    if (strAD != "")
                    {
                        //strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_ERROR");
                        //oFunction.SendEmail("Decommission Error - Active Directory", strEMailIdsBCC, "", "", "Decommission Error - Active Directory", "<p>Here are the results of the automated decommission from (" + oDomain.Get(intDomain, "name") + ") Active Directory for " + strName + "...</p><p>" + strAD + "</p><p>This message is not good...the computer object was NOT removed...</p>", true, false);
                        oLog.AddEvent(strName, "InitiateDecom", "There was a problem removing the computer object", LoggingType.Error);
                    }
                    else
                    {
                        strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_ACTIVE_DIRECTORY");
                        oFunction.SendEmail("Decommission Success - Active Directory", strEMailIdsBCC, "", "", "Decommission Success - Active Directory", "<p>The automated decommission from (" + oDomain.Get(intDomain, "name") + ") Active Directory for " + strName + " was successful!!</p>", true, false);
                        oLog.AddEvent(strName, "InitiateDecom", "Computer object removed successfully", LoggingType.Information);
                    }
                }
            }
        }

        public void VerifyDecomCompletionAndNofity(
                                  int intRequest, int intItem, int intNumber,
                                  int intAssignPage, int intViewPage,
                                  int intEnvironment,
                                  int intIMDecommServiceId,
                                  string dsnServiceEditor, string dsnAsset, string dsnIP)
        {

            // *** CALLED WHEN A TASK IS COMPLETED (OR ALL ARE COMPLETED FROM "InitiateDecom" FUNCTION)

            int intServer = 0;
            int intModel = 0;
            int intOS = 0;
            int intForecastAnswerid = 0;
            int intProfile = user;
            bool boolSANAttached = false;
            bool boolCSMAttached = false;
            bool boolIsPNCServer = false;
            bool boolIsServerHasBackup = false;
            bool boolIsDistributed = false;

            bool boolIsServerVMWare = false;
            bool boolWorkflowCompleted = true;
            Customized oCustomized = new Customized(intProfile, dsn);
            ResourceRequest oResourceRequest = new ResourceRequest(intProfile, dsn);
            ModelsProperties oModelsProperties = new ModelsProperties(intProfile, dsn);
            OperatingSystems oOperatingSystem = new OperatingSystems(user, dsn);
            Servers oServers = new Servers(intProfile, dsn);
            Forecast oForecast = new Forecast(user, dsn);
            PNCTasks oPNCTask = new PNCTasks(user, dsn);
            Storage oStorage = new Storage(user, dsn);
            Asset oAsset = new Asset(user, dsnAsset, dsn);
            oService = new Services(user, dsn);
            oServiceRequest = new ServiceRequests(user, dsn);
            oServiceDetail = new ServiceDetails(user, dsn);
            oServerName = new ServerName(user, dsn);
            Log oLog = new Log(user, dsn);

            DataSet dsDecom = oCustomized.GetDecommissionServer(intRequest, intItem, intNumber);
            if (dsDecom.Tables[0].Rows.Count > 0)
            {
                intServer = Int32.Parse(dsDecom.Tables[0].Rows[0]["serverid"].ToString());
                boolSANAttached = (dsDecom.Tables[0].Rows[0]["SAN"].ToString() == "1" ? true : false);
                boolCSMAttached = (dsDecom.Tables[0].Rows[0]["CSM"].ToString() == "1" ? true : false);
            }

            DataSet dsServer = oServers.Get(intServer);

            if (dsServer.Tables[0].Rows.Count > 0)
            {
                if (dsServer.Tables[0].Rows[0]["modelid"] != DBNull.Value)
                    intModel = Int32.Parse(dsServer.Tables[0].Rows[0]["modelid"].ToString());
                if (dsServer.Tables[0].Rows[0]["osid"] != DBNull.Value)
                    intOS = Int32.Parse(dsServer.Tables[0].Rows[0]["osid"].ToString());

                boolIsPNCServer = (dsServer.Tables[0].Rows[0]["PNC"] != DBNull.Value ? (dsServer.Tables[0].Rows[0]["PNC"].ToString() == "1" ? true : false) : false);
                boolIsServerHasBackup = (Int32.Parse(dsServer.Tables[0].Rows[0]["tsm_schedule"].ToString()) > 0 ? true : false);
                intForecastAnswerid = Int32.Parse(dsServer.Tables[0].Rows[0]["answerid"].ToString());
                boolIsDistributed = ((intOS > 0 && oOperatingSystem.IsDistributed(intOS)) || (oForecast.IsOSDistributed(intForecastAnswerid)));
            }


            string strName = "";
            if (intServer > 0)
            {
                if (intModel > 0 && oModelsProperties.IsTypeVMware(intModel) == true)
                    boolIsServerVMWare = true;
                strName = oServers.GetName(intServer, true);
                if (boolSANAttached == false && boolIsServerVMWare == false && strName.Trim() != "")
                    boolSANAttached = (oStorage.GetStorageDW(strName).Tables[0].Rows.Count > 0 || (oForecast.IsStorage(intForecastAnswerid) == true && oForecast.GetAnswer(intForecastAnswerid, "storage") == "1"));
            }

            DataSet dsTasks = oPNCTask.Gets((boolIsPNCServer ? 1 : 0), (boolIsPNCServer ? 0 : 1), (boolIsDistributed ? 1 : 0), (boolIsDistributed ? 0 : 1), 1, 1);
            oLog.AddEvent(strName, "VerifyDecomCompletionAndNofity", "There are " + dsTasks.Tables[0].Rows.Count.ToString() + " tasks associated with this decommission (ServerID = " + intServer.ToString() + ")", LoggingType.Debug);
            foreach (DataRow drTask in dsTasks.Tables[0].Rows)
            {
                if (boolWorkflowCompleted == false)
                {
                    oLog.AddEvent(strName, "VerifyDecomCompletionAndNofity", "DECOM TASK - BREAK!", LoggingType.Debug);
                    break;
                }

                DataSet dsVerifyServices;
                bool boolCheck = false;
                if (drTask["storage"].ToString() == "1")
                {
                    if (boolSANAttached == true)
                        boolCheck = true;
                    oLog.AddEvent(strName, "VerifyDecomCompletionAndNofity", "DECOM TASK - Checking storage : " + (boolCheck ? "Yes" : "No"), LoggingType.Debug);
                }
                else if (drTask["if_ltm_install"].ToString() == "1")
                {
                    if (boolCSMAttached == true)
                        boolCheck = true;
                    oLog.AddEvent(strName, "VerifyDecomCompletionAndNofity", "DECOM TASK - Checking LTM : " + (boolCheck ? "Yes" : "No"), LoggingType.Debug);
                }
                else if (drTask["tsm"].ToString() == "1")
                {
                    if (boolIsServerHasBackup == true)
                        boolCheck = true;
                    oLog.AddEvent(strName, "VerifyDecomCompletionAndNofity", "DECOM TASK - Checking TSM : " + (boolCheck ? "Yes" : "No"), LoggingType.Debug);
                }
                else
                {
                    boolCheck = true;
                }

                int intService = Int32.Parse(drTask["serviceid"].ToString());
                if (boolCheck == true)
                {
                    dsVerifyServices = oResourceRequest.GetAllService(intRequest, intService, intNumber);
                    oLog.AddEvent(strName, "VerifyDecomCompletionAndNofity", "DECOM TASK - There are " + dsVerifyServices.Tables[0].Rows.Count.ToString() + " records for service " + oService.GetName(intService), LoggingType.Debug);
                    if (dsVerifyServices.Tables[0].Rows.Count > 0)
                    {
                        if (dsVerifyServices.Tables[0].Rows[0]["status"].ToString() == "-2")
                            oLog.AddEvent(strName, "VerifyDecomCompletionAndNofity", "Workflow " + oService.GetName(intService) + " is cancelled", LoggingType.Debug);
                        else if (dsVerifyServices.Tables[0].Rows[0]["status"].ToString() == "3" || dsVerifyServices.Tables[0].Rows[0]["completed"] != DBNull.Value)
                            oLog.AddEvent(strName, "VerifyDecomCompletionAndNofity", "Workflow " + oService.GetName(intService) + " is complete", LoggingType.Debug);
                        else
                        {
                            boolWorkflowCompleted = false;
                            oLog.AddEvent(strName, "VerifyDecomCompletionAndNofity", "Workflow " + oService.GetName(intService) + " is incomplete", LoggingType.Debug);
                            break;
                        }
                    }
                    // Remove otherwise the decom will fail.  To be done - need to fix.
                    else
                    {
                        boolWorkflowCompleted = false;
                        oLog.AddEvent(strName, "VerifyDecomCompletionAndNofity", "Workflow " + oService.GetName(intService) + " is missing one or more tasks...regenerating", LoggingType.Debug);
                        bool MissedFix = true; // so that we don't cause an outage
                        DataSet dsDecommission = oAsset.GetDecommission(strName);
                        if (dsDecommission.Tables[0].Rows.Count > 0)
                            MissedFix = (dsDecommission.Tables[0].Rows[0]["missed_fix"].ToString() != "");
                        InitiateDecom(intServer, intModel, strName, intRequest, intItem, intNumber, (boolSANAttached ? 1 : 0), (boolCSMAttached ? 1 : 0), intAssignPage, intViewPage, intEnvironment, intIMDecommServiceId, dsnServiceEditor, dsnAsset, dsnIP, "", MissedFix);
                        break;
                    }
                }
                else
                    oLog.AddEvent(strName, "VerifyDecomCompletionAndNofity", "DECOM TASK - Skipping service " + oService.GetName(intService), LoggingType.Debug);
            }
            if (boolWorkflowCompleted == true)
            {
                oLog.AddEvent(strName, "VerifyDecomCompletionAndNofity", "All workflows have been completed", LoggingType.Information);
                //Generate IM Task -If not VMWARE    
                if (boolIsServerVMWare == false)
                {
                    InitiateAssetReDeployOrDisposeServiceRequest(intServer, intRequest, intNumber, dsnAsset, dsnServiceEditor, intEnvironment, dsnIP, intAssignPage, intViewPage);

                    //int intServiceItemId = oService.GetItemId(intIMDecommServiceId);
                    //double dblServiceHours = oServiceDetail.GetHours(intIMDecommServiceId, 1);
                    //oCustomized.AddDecommissionServerIM(intRequest, intServiceItemId, intNumber, intServer, 0, 0, 0, 0, 0, 0, 0);
                    //int intResource = oServiceRequest.AddRequest(intRequest, intServiceItemId, intIMDecommServiceId, 1, dblServiceHours, 2, intNumber, dsnServiceEditor);
                    //oServiceRequest.NotifyTeamLead(intServiceItemId, intResource, intAssignPage, intViewPage, intEnvironment,  "", dsnServiceEditor, dsnAsset, dsnIP, 0);

                    oLog.AddEvent(strName, "VerifyDecomCompletionAndNofity", "Redeploy / Dispose task generated", LoggingType.Information);
                }
            }
            else
                oLog.AddEvent(strName, "VerifyDecomCompletionAndNofity", "Workflows are incomplete", LoggingType.Debug);
        }

        public bool CheckBluecat(string _name, ClearViewWebServices _service, Log _log)
        {
            string strCheckIP = _service.SearchBluecatDNS("", _name);
            if (strCheckIP.StartsWith("***") == false)
            {
                _log.AddEvent(_name, "", "The Name " + _name + " points to " + strCheckIP, LoggingType.Debug);
                // Name is unique
                string strCheckName = _service.SearchBluecatDNS(strCheckIP, "");
                if (strCheckName.StartsWith("***") == false)
                {
                    _log.AddEvent(_name, "", "The IP Address " + strCheckIP + " points to " + strCheckName, LoggingType.Debug);
                    // IP is unique
                    //if (_name.Trim().ToUpper() == strCheckName.Trim().ToUpper())
                    if (strCheckName.Trim().ToUpper().Contains(_name.Trim().ToUpper()) == true)
                    {
                        _log.AddEvent(_name, "", "The name " + _name + " is found in " + strCheckName + ". OK to decom.", LoggingType.Debug);
                        return true;
                    }
                    else
                    {
                        _log.AddEvent(_name, "", "The name " + _name + " is NOT found in " + strCheckName + ". Skipping Decom.", LoggingType.Error);
                        return false;
                    }
                }
                else
                {
                    _log.AddEvent(_name, "", "There was a problem resolving the IP Address " + strCheckIP + " in DNS ~ " + strCheckName, LoggingType.Error);
                    return false;
                }
            }
            else
            {
                _log.AddEvent(_name, "", "There was a problem resolving the Name " + _name + " in DNS ~ " + strCheckIP, LoggingType.Error);
                return false;
            }
        }
        public string GetBody(int _requestid, int _number, string _dsnAsset, int _environment)
        {
            Functions oFunction = new Functions(0, dsn, _environment);
            Asset oAsset = new Asset(0, _dsnAsset, dsn);
            DataSet ds = oAsset.GetDecommission(_requestid, _number, 0);
            Users oUser = new Users(0, dsn);
            Customized oCustomized = new Customized(0, dsn);
            Requests oRequest = new Requests(0, dsn);
            int intUser = oRequest.GetUser(_requestid);
            StringBuilder sbDetails = new StringBuilder();
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                int intAsset = Int32.Parse(dr["assetid"].ToString());
                int intItem = Int32.Parse(dr["itemid"].ToString());
                string strResult = "";
                DataSet dsResults = oRequest.GetResult(_requestid, intItem, _number);
                foreach (DataRow drResult in dsResults.Tables[0].Rows)
                {
                    if (strResult != "")
                        strResult += "<br/>";
                    strResult += drResult["result"].ToString();
                }
                sbDetails.Append("<tr><td>Device Name: </td><td><a href=\"/datapoint/asset/datapoint_asset_search.aspx?t=name&q=" + oFunction.encryptQueryString(dr["name"].ToString()) + "\" target=\"_blank\">" + dr["name"].ToString() + "</a></td></tr>");
                sbDetails.Append("<tr><td>Serial Number: </td><td><a href=\"/datapoint/asset/datapoint_asset_search.aspx?t=serial&q=" + oFunction.encryptQueryString(oAsset.Get(intAsset, "serial")) + "\" target=\"_blank\">" + oAsset.Get(intAsset, "serial") + "</a></td></tr>");
                sbDetails.Append("<tr><td>Requested By: </td><td>" + oUser.GetFullName(intUser) + "</td></tr>");
                DataSet dsC = oCustomized.GetDecommissionServer(_requestid, intItem, _number);
                if (dsC.Tables[0].Rows.Count > 0)
                    sbDetails.Append("<tr><td>Change Control: </td><td>" + dsC.Tables[0].Rows[0]["change"].ToString() + "</td></tr>");
                sbDetails.Append("<tr><td>Reason: </td><td>" + dr["reason"].ToString() + "</td></tr>");
                sbDetails.Append("<tr><td>Error: </td><td>" + strResult + "</td></tr>");
                sbDetails.Append("<tr><td>Power Off Date: </td><td>" + dr["decom"].ToString() + "</td></tr>");
                sbDetails.Append("<tr><td>Powered Off On: </td><td>" + dr["turnedoff"].ToString() + "</td></tr>");
                sbDetails.Append("<tr><td>To Be Destroyed On: </td><td>" + dr["destroy"].ToString() + "</td></tr>");
            }
            return sbDetails.ToString();
        }


        public void InitiateAssetReDeployOrDisposeServiceRequest(int _serverId, int _request, int _number, string _dsnAsset, string _dsnServiceEditor, int _environment, string _dsnIP, int _assign_page, int _view_page)
        {
            Servers oServer = new Servers(user, dsn);
            Log oLog = new Log(user, dsn);
            Asset oAsset = new Asset(user, _dsnAsset, dsn);
            Models oModel = new Models(user, dsn);
            ModelsProperties oModelsProperties = new ModelsProperties(user, dsn);
            Locations oLocation = new Locations(user, dsn);
            ResourceRequest oResourceRequest = new ResourceRequest(user, dsn);
            int intDRLocation = 0;
            AssetOrder oAssetOrder = new AssetOrder(user, dsn, _dsnAsset, _environment);
            int intNextServiceItemID = 0;
            string name = oServer.GetName(_serverId, true);

            DataSet dsServer = oServer.Get(_serverId);
            if (dsServer.Tables[0].Rows.Count > 0)
            {
                DataRow drServer = dsServer.Tables[0].Rows[0];
                intDRLocation = oLocation.GetAddressDR();

                DataSet dsServerAssets = oServer.GetAssets(_serverId);
                foreach (DataRow drServerAsset in dsServerAssets.Tables[0].Rows)
                {
                    string serial = drServerAsset["serial"].ToString();
                    oLog.AddEvent(name, serial, "Redeploy / Dispose starting", LoggingType.Debug);
                    if (drServerAsset["latest"].ToString() == "1" || drServerAsset["dr"].ToString() == "1")
                    {
                        int intAssetId = Int32.Parse(drServerAsset["AssetId"].ToString());
                        int intModelPropertyId = Int32.Parse(oAsset.Get(intAssetId, "ModelId"));
                        int intModelId = Int32.Parse(oModelsProperties.Get(intModelPropertyId, "ModelId"));
                        int intDispose = Int32.Parse(oModel.Get(intModelId, "destroy"));
                        int intLocation = (drServerAsset["dr"].ToString() == "1" ? intDRLocation : Int32.Parse(drServer["addressid"].ToString()));
                        int intOrderId = oAssetOrder.AddOrderId(user);
                        int intNextRequestNumber = oResourceRequest.GetNumber(_request);
                        //int intNextRequestNumber = _number;


                        int intBFS = 0;
                        if (oModelsProperties.Get(intModelPropertyId, "type_blade") == "1"
                        || oModelsProperties.Get(intModelPropertyId, "storage_db_boot_san_windows") == "1"
                        || oModelsProperties.Get(intModelPropertyId, "storage_db_boot_san_unix") == "1")
                            intBFS = 1;

                        oAssetOrder.AddRemoveAssetOrderAssetSelection(intOrderId, intAssetId, user, 1);

                        int intRoom = 0;
                        Int32.TryParse(oAsset.GetServerOrBlade(intAssetId, "roomid"), out intRoom);
                        int intZone = 0;
                        Int32.TryParse(oAsset.GetServerOrBlade(intAssetId, "ZoneId"), out intZone);
                        int intRack = 0;
                        Int32.TryParse(oAsset.GetServerOrBlade(intAssetId, "rackid"), out intRack);
                        string strRackPosition = oAsset.GetServerOrBlade(intAssetId, "rackposition");
                        int intResiliency = 0;
                        Int32.TryParse(oAsset.GetServerOrBlade(intAssetId, "resiliencyid"), out intResiliency);
                        int intOperatingSystemGroup = 0;
                        Int32.TryParse(oAsset.GetServerOrBlade(intAssetId, "OperatingSystemGroupId"), out intOperatingSystemGroup);
                        int intEnclosure = 0;
                        Int32.TryParse(oAsset.GetServerOrBlade(intAssetId, "enclosureid"), out intEnclosure);
                        int intEncSlot = 0;
                        Int32.TryParse(oAsset.GetServerOrBlade(intAssetId, "slot"), out intEncSlot);


                        oAssetOrder.AddOrder(intOrderId,
                                          _request, intNextServiceItemID, intNextRequestNumber,
                                          (intDispose == 1 ? (int)AssetOrderType.Dispose : (int)AssetOrderType.ReDeploy),
                                          (intDispose == 1 ? "Decommissioned Asset Dispose" : "Decommissioned Asset Redeploy"),
                                          intModelPropertyId,
                                          intLocation,
                                          intRoom,
                                          intZone,
                                          intRack,
                                          strRackPosition,
                                          intResiliency,
                                          intOperatingSystemGroup,
                                          Int32.Parse(drServer["classid"].ToString()),
                                          Int32.Parse(drServer["environmentid"].ToString()),
                                          intEnclosure,
                                          intEncSlot,
                                          1,
                                          0,
                                          (intDispose == 1 ? 0 : 1), 0,
                                          DateTime.Now.AddDays(10), 0, intBFS, intBFS, 0, "", 0, "", 1, user);

                        oAssetOrder.InitiateNextServiceRequestOrCompleteRequest(intOrderId, intNextRequestNumber, 0, true, _dsnServiceEditor, _assign_page, _view_page, _dsnAsset, _dsnIP);
                        oLog.AddEvent(name, serial, "Order # " + intOrderId.ToString() + " generated", LoggingType.Debug);

                        // oServiceRequest.NotifyTeamLead(intNextServiceItemID, intResource, intAssignPage, intViewPage, intEnvironment, "", dsnServiceEditor, dsnAsset, dsnIP, 0);
                    }
                    else
                        oLog.AddEvent(name, serial, "Redeploy / Dispose skipped for this asset", LoggingType.Debug);

                }
            }
        }

    }
}
