using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using NCC.ClearView.Application.Core;
using System.IO;
using System.Data;
using System.Diagnostics;
using System.Net.NetworkInformation;

namespace ClearViewAP_VMware
{
    public class AuditThread
    {
        private int intServer = 0;
        private string strName = "";
        private string strSerial = "";
        private int intIP = 0;
        private bool boolOKtoAssignIP = false;
        private string strIP = "";
        private int intClass = 0;
        private int intEnv = 0;
        private int intModel = 0;
        private int intOS = 0;
        private int intSP = 0;
        private int intAddress = 0;
        private bool boolIsSAN = false;
        private bool boolIsCluster = false;
        private bool boolIsTSM = false;

        private int intStep = 0;
        private int intRequest = 0;
        private int intService = 0;
        private int intResourceRequestApprove = 0;
        private int intAssignPage = 0;
        private int intViewPage = 0;
        private string strScripts;
        private string strSub;
        private string strAdminUser;
        private string strAdminPass;
        private int intEnvironment;
        private int intLogging;
        private string dsn = "";
        private string dsnAsset = "";
        private string dsnIP;
        private string dsnServiceEditor;
        private bool boolDeleteFiles = false;
        private bool boolMultiThreaded = false;
        private Servers oServer;
        private Audit oAudit;
        private Log oLog;
        private OnDemand oOnDemand;
        private OperatingSystems oOperatingSystem;
        private IPAddresses oIPAddresses;
        private Variables oVariable;
        private Functions oFunction;
        private Service oCalling;
        private bool boolMIS = false;

        public AuditThread(int _serverid, string _name, string _serial, int _ipaddressid, bool _ok_to_assign_ip, string _ip, int _classid, int _environmentid, int _modelid, int _osid, int _spid, int _addressid, bool _is_san, bool _is_cluster, bool _is_tsm, int _step, int _requestid, int _serviceid, int _resourcerequestapprove, int _assignpage, int _viewpage, string _scripts, string _sub, string _admin_user, string _admin_pass, int _environment, int _logging, string _dsn, string _dsn_asset, string _dsn_ip, string _dsn_service_editor, bool _delete_files, bool _multi_threaded, Service _calling, bool _mis)
		{
            intServer = _serverid;
            strName = _name;
            strSerial = _serial;
            intIP = _ipaddressid;
            boolOKtoAssignIP = _ok_to_assign_ip;
            strIP = _ip;
            intClass = _classid;
            intEnv = _environmentid;
            intModel = _modelid;
            intOS = _osid;
            intSP = _spid;
            intAddress = _addressid;
            boolIsSAN = _is_san;
            boolIsCluster = _is_cluster;
            boolIsTSM = _is_tsm;

            intStep = _step;
            intRequest = _requestid;
            intService = _serviceid;
            intResourceRequestApprove = _resourcerequestapprove;
            intViewPage = _viewpage;
            intAssignPage = _step;
            strScripts = _scripts;
            strSub = _sub;
            strAdminUser = _admin_user;
            strAdminPass = _admin_pass;
            intEnvironment = _environment;
            intLogging = _logging;
            dsn = _dsn;
            dsnAsset = _dsn_asset;
            dsnIP = _dsn_ip;
            dsnServiceEditor = _dsn_service_editor;
            boolDeleteFiles = _delete_files;
            boolMultiThreaded = _multi_threaded;
            oCalling = _calling;
            boolMIS = _mis;
        }

        public void Begin()
        {
            oServer = new Servers(0, dsn);
            oAudit = new Audit(0, dsn);
            oLog = new Log(0, dsn);
            oOnDemand = new OnDemand(0, dsn);
            oOperatingSystem = new OperatingSystems(0, dsn);
            oIPAddresses = new IPAddresses(0, dsnIP, dsn);
            oVariable = new Variables(intEnvironment);
            oFunction = new Functions(0, dsn, intEnvironment);

            if (boolMultiThreaded == true)
            {
                // Run Audit Scripts in Multi-Threaded Fashion
                ThreadStart oThreadStart = new ThreadStart(RunAudits);
                Thread oThread = new Thread(oThreadStart);
                oThread.Start();
            }
            else
            {
                // Run Audit Scripts in Single-Threaded Fashion
                RunAudits();
            }
        }

        private void RunAudits()
        {
            oLog.AddEvent(strName, strSerial, "Current Audit Count = " + oCalling.AuditCount.ToString(), LoggingType.Debug);
            oCalling.AuditCount = oCalling.AuditCount + 1;
            oLog.AddEvent(strName, strSerial, "New Audit Count = " + oCalling.AuditCount.ToString(), LoggingType.Debug);

            //oAudit.DeleteServer(intServer);
            oLog.AddEvent(strName, strSerial, "Querying for audit scripts... (SERVERID = " + intServer.ToString() + ", CLASSID = " + intClass.ToString() + ", ENVIRONMENTID = " + intEnv.ToString() + ", MODELID = " + intModel.ToString() + ", OSID = " + intOS.ToString() + ", SPID = " + intSP.ToString() + ", ADDRESSID = " + intAddress.ToString() + ", SAN = " + (boolIsSAN ? "1" : "0") + ", CLUSTER = " + (boolIsCluster ? "1" : "0") + ")", LoggingType.Information);
            DataSet dsAudit = oAudit.GetServerScripts(intServer, intClass, intEnv, intModel, intOS, intSP, intAddress, boolIsSAN, boolIsCluster, boolMIS, true);
            oLog.AddEvent(strName, strSerial, "There are " + dsAudit.Tables[0].Rows.Count.ToString() + " TOTAL audit script(s) to run (in " + (boolMultiThreaded ? "multi" : "single") + "-threaded mode)", LoggingType.Information);

            string strResult = "";
            string strError = "";
            int intAuditIDError = 0;

            if (dsAudit.Tables[0].Rows.Count == 0)
                strResult = "There are no audit scripts to run (" + dsAudit.Tables[0].Rows.Count.ToString() + ")";
            else
            {
                string strAuditResult = "";
                string strAuditError = "";
                string strAuditIP = strIP;
                if (intIP > 0 && boolOKtoAssignIP == true)
                    strAuditIP = oIPAddresses.GetName(intIP, 0);
                dsAudit = oAudit.GetServerScripts(intServer, intClass, intEnv, intModel, intOS, intSP, intAddress, boolIsSAN, boolIsCluster, boolMIS, false);
                oLog.AddEvent(strName, strSerial, "There are " + dsAudit.Tables[0].Rows.Count.ToString() + " PENDING audit script(s) to run", LoggingType.Information);
                foreach (DataRow drAudit in dsAudit.Tables[0].Rows)
                {
                    int intScript = Int32.Parse(drAudit["scriptid"].ToString());
                    int intScriptSet = Int32.Parse(drAudit["scriptsetid"].ToString());
                    int intScriptDetail = Int32.Parse(drAudit["detailid"].ToString());
                    string strAuditName = drAudit["name"].ToString();
                    string strHardcode = drAudit["hardcode"].ToString();
                    string strEXE = drAudit["exe"].ToString();
                    string strExtension = drAudit["extension"].ToString();
                    string strPath = drAudit["path"].ToString();    // The path to the script
                    int intAuditTimeout = Int32.Parse(drAudit["timeout"].ToString());
                    bool boolRemote = (drAudit["local"].ToString() == "0"); // REBOOT is NULL so set to boolRemote = false for those
                    DateTime _now = DateTime.Now;
                    string strNow = _now.Day.ToString() + _now.Month.ToString() + _now.Year.ToString() + _now.Hour.ToString() + _now.Minute.ToString() + _now.Second.ToString() + _now.Millisecond.ToString();
                    string strAuditScriptPath = strScripts + strSub + intServer.ToString() + "_" + strNow + "_audit_";
                    int intAuditReturn = (int)AuditStatus.Running;

                    // Delete all data from TEST and PROD
                    //oAudit.DeleteServer(intServer);
                    //oAudit.DeleteServerDetailRemote(intServer);

                    // Add data to TEST and PROD
                    int intAuditID = oAudit.AddServer(intServer, intScriptSet, intScript, boolMIS, AuditStatus.Running);
                    int intMHS = 0;
                    Int32.TryParse(oServer.Get(intServer, "mhs"), out intMHS);
                    string strParameters = oAudit.GetScriptParameters(drAudit["parameters"].ToString(), intAuditID, strName, strAuditIP, (boolIsTSM ? 1 : 0), intMHS);

                    bool boolHardcode = false;
                    if (strHardcode != "")
                    {
                        if (strHardcode == "ESM")
                        {
                            boolHardcode = true;
                            oLog.AddEvent(strName, strSerial, "ESM script encountered...", LoggingType.Information);
                            strAuditName = "Audit Script # " + intScriptDetail.ToString() + "(ESM)";
                            intAuditTimeout = 10;    // 10 minutes

                            string strAuditScriptHelper = strAuditScriptPath + "esm.esm";
                            string strAuditScriptHelperOut = strAuditScriptPath + "esm.txt";
                            if (File.Exists(strAuditScriptHelperOut) == true)
                                File.Delete(strAuditScriptHelperOut);
                            StreamWriter oAuditBatch = new StreamWriter(strAuditScriptHelper);
                            string strESM = "run job -v esmjob -a \"" + strName.ToUpper() + ".pncbank.com\" -m registry \"PNC SSCP 2011.v2\" \"Windows " + (oOperatingSystem.IsWindows2008(intOS) ? "2008" : "2003") + " Agents\"";
                            oLog.AddEvent(strName, strSerial, "ESM JOB = " + strESM, LoggingType.Information);
                            oAuditBatch.WriteLine(strESM);
                            oAuditBatch.WriteLine("sleep -j %esmjob%");
                            string strESMout = "view custom -o \"" + strAuditScriptHelperOut + "\" \"PNC SSCP 2011.v2\" \"" + strName.ToUpper() + ".pncbank.com\" registry %esmjob% code.vc";
                            oLog.AddEvent(strName, strSerial, "ESM OUT = " + strESM, LoggingType.Information);
                            oAuditBatch.WriteLine(strESMout);
                            oAuditBatch.Flush();
                            oAuditBatch.Close();

                            string strAuditScript = strAuditScriptPath + "esm.bat";
                            StreamWriter oAuditWriter = new StreamWriter(strAuditScript);
                            oAuditWriter.WriteLine("CD D:\\ESM\\bin\\w8s-ix64");
                            oAuditWriter.WriteLine("D:");
                            string strScript = "esmc -m " + (intAddress == 715 ? "wcesm300a" : (intAddress == 696 ? "wdesm100a" : (intAddress == 1675 ? "vwesm301" : "UNKNOWN")));
                            oLog.AddEvent(strName, strSerial, "ESM Script = " + strScript, LoggingType.Information);
                            oAuditWriter.WriteLine(strScript + " -U xacview -P Abcd1234 -b \"" + strAuditScriptHelper + "\"");
                            oAuditWriter.Flush();
                            oAuditWriter.Close();
                            intAuditReturn = oFunction.ExecuteVBScript(intServer, true, true, strAuditName, strName, strSerial, strAuditIP, strAuditScript, strAuditScriptPath, "ESM", "%windir%\\system32\\cmd.exe /c", "OPTIONS\\CV_AUDIT_REBOOT", "VBS", "", strScripts, strAdminUser, strAdminPass, intAuditTimeout, (oOperatingSystem.IsWindows2008(intOS) == false), false, intLogging, boolDeleteFiles);

                            AuditStatus oAuditStatusTemp = (AuditStatus)intAuditReturn;
                            if (oAuditStatusTemp == AuditStatus.Success || oAuditStatusTemp == AuditStatus.Warning)
                            {
                                // Check the output file
                                intAuditReturn = (int)AuditStatus.Error;
                                string strContent = "";
                                for (int ii = 0; ii < 10; ii++)
                                {
                                    if (File.Exists(strAuditScriptHelperOut) == true)
                                    {
                                        oLog.AddEvent(strName, strSerial, "ESM output file " + strAuditScriptHelperOut + " exists...reading...", LoggingType.Information);
                                        StreamReader oReader = new StreamReader(strAuditScriptHelperOut);
                                        try
                                        {
                                            strContent = oReader.ReadToEnd();
                                            if (strContent == "0")
                                            {
                                                intAuditReturn = (int)AuditStatus.Success;
                                                if (File.Exists(strAuditScriptHelperOut) == true)
                                                    File.Delete(strAuditScriptHelperOut);
                                            }
                                            else if (strContent == "1")
                                                intAuditReturn = (int)AuditStatus.Warning;
                                            else
                                                intAuditReturn = (int)AuditStatus.Error;
                                            oReader.Close();
                                        }
                                        catch
                                        {
                                            if (intLogging > 1)
                                                oLog.AddEvent(strName, strSerial, "Cannot open ESM output file " + strAuditScriptHelperOut + "...waiting 5 seconds...", LoggingType.Information);
                                            oReader.Close();
                                            Thread.Sleep(5000);
                                        }
                                    }
                                    else
                                    {
                                        if (intLogging > 1)
                                            oLog.AddEvent(strName, strSerial, "ESM output file " + strAuditScriptHelperOut + " does not exist...waiting 5 seconds...", LoggingType.Information);
                                        Thread.Sleep(5000);
                                    }
                                }
                            }
                            else
                            {
                                // Just let it continue...it will throw error later.
                                oLog.AddEvent(strName, strSerial, "ESM return status = " + intAuditReturn.ToString(), LoggingType.Error);
                            }
                        }
                    }

                    if (boolHardcode == false)
                    {
                        if (boolRemote == true)
                        {
                            // Script is executed from the ClearView server (on the clearview server)
                            strAuditName = "Audit Script # " + intScriptDetail.ToString() + "(" + strAuditName + ")";
                            intAuditReturn = oFunction.ExecuteVBScript(intServer, true, false, strAuditName, strName, strSerial, strAuditIP, strPath, strAuditScriptPath, "Script" + intScriptDetail.ToString() + "_", strEXE, "OPTIONS\\CV_AUDIT_SCRIPT_" + intScriptDetail.ToString() + "_", strExtension, strParameters, strScripts, strAdminUser, strAdminPass, intAuditTimeout, (oOperatingSystem.IsWindows2008(intOS) == false), false, intLogging, boolDeleteFiles);
                        }
                        else
                        {
                            // Majority of scripts should be here...
                            // Copy the script to the target server and run on that server. (REBOOT)
                            if (intScript == 0)
                            {
                                oLog.AddEvent(strName, strSerial, "Reboot script encountered...rebooting...", LoggingType.Information);
                                // Here is the reboot...
                                strAuditName = "Audit Script # " + intScriptDetail.ToString() + "(Reboot)";
                                //   1.) Create the VBS for rebooting the computer
                                string strAuditScript = strAuditScriptPath + "reboot.vbs";
                                StreamWriter oAuditWriter = new StreamWriter(strAuditScript);
                                oAuditWriter.WriteLine("Set OpSysSet = GetObject(\"winmgmts:{impersonationLevel=impersonate,(Shutdown)}\").ExecQuery(\"select * from Win32_OperatingSystem where Primary=true\")");
                                oAuditWriter.WriteLine("For Each OpSys In OpSysSet");
                                oAuditWriter.WriteLine("Debug \"OpSys.Win32Shutdown return: \" & OpSys.Win32Shutdown(6)");  // 6 = Forced Reboot
                                oAuditWriter.WriteLine("Next");
                                oAuditWriter.Flush();
                                oAuditWriter.Close();
                                intAuditReturn = oFunction.ExecuteVBScript(intServer, false, true, strAuditName, strName, strSerial, strAuditIP, strAuditScript, strAuditScriptPath, "Reboot", "%windir%\\system32\\wscript.exe", "OPTIONS\\CV_AUDIT_REBOOT", "VBS", "", strScripts, strAdminUser, strAdminPass, intAuditTimeout, (oOperatingSystem.IsWindows2008(intOS) == false), true, intLogging, boolDeleteFiles);

                                // Wait for server to reboot
                                oLog.AddEvent(strName, strSerial, "Waiting for device to shutdown...", LoggingType.Information);
                                string strRebootDown = "";
                                bool boolRebootDown = false;
                                for (int ii = 0; ii < 60 && boolRebootDown == false; ii++)
                                {
                                    Ping oPingDown = new Ping();
                                    try
                                    {
                                        PingReply oReplyDown = oPingDown.Send(strAuditIP);
                                        strRebootDown = oReplyDown.Status.ToString().ToUpper();
                                    }
                                    catch { }
                                    boolRebootDown = (strRebootDown != "SUCCESS");
                                    if (boolRebootDown == false)
                                    {
                                        int intRebootDownLeft = (60 - ii);
                                        oLog.AddEvent(strName, strSerial, "Device still powering down...waiting 3 seconds (Reply = " + strRebootDown + ") (" + intRebootDownLeft.ToString() + " tries left)", LoggingType.Debug);
                                        Thread.Sleep(3000);
                                    }
                                }

                                if (boolRebootDown == false)
                                {
                                    oLog.AddEvent(strName, strSerial, "The device is still powered on.  Trying with increased timeout...", LoggingType.Information);
                                    // Let's try increasing the timeout...maybe it was taking a while connecting to the server and we killed it too quickly
                                    intAuditReturn = oFunction.ExecuteVBScript(intServer, false, true, strAuditName, strName, strSerial, strAuditIP, strAuditScript, strAuditScriptPath, "Reboot", "%windir%\\system32\\wscript.exe", "OPTIONS\\CV_AUDIT_REBOOT", "VBS", "", strScripts, strAdminUser, strAdminPass, intAuditTimeout, (oOperatingSystem.IsWindows2008(intOS) == false), false, intLogging, boolDeleteFiles);

                                    // Wait for server to reboot
                                    oLog.AddEvent(strName, strSerial, "Waiting for device to shutdown (2)...", LoggingType.Information);
                                    strRebootDown = "";
                                    boolRebootDown = false;
                                    for (int ii = 0; ii < 60 && boolRebootDown == false; ii++)
                                    {
                                        Ping oPingDown = new Ping();
                                        try
                                        {
                                            PingReply oReplyDown = oPingDown.Send(strAuditIP);
                                            strRebootDown = oReplyDown.Status.ToString().ToUpper();
                                        }
                                        catch { }
                                        boolRebootDown = (strRebootDown != "SUCCESS");
                                        if (boolRebootDown == false)
                                        {
                                            int intRebootDownLeft = (60 - ii);
                                            oLog.AddEvent(strName, strSerial, "Device still powering down (2)...waiting 3 seconds (Reply = " + strRebootDown + ") (" + intRebootDownLeft.ToString() + " tries left)", LoggingType.Debug);
                                            Thread.Sleep(3000);
                                        }
                                    }
                                }

                                // At this point, the server should be down...
                                if (boolRebootDown == true)
                                {
                                    oLog.AddEvent(strName, strSerial, "Shutdown has finished (" + strRebootDown + ")...now waiting for device to come back up...", LoggingType.Information);
                                    // Wait for server to come back up
                                    string strRebootUp = "";
                                    bool boolRebootUp = false;
                                    for (int jj = 0; jj < 60 && boolRebootUp == false; jj++)
                                    {
                                        Ping oPingUp = new Ping();
                                        try
                                        {
                                            PingReply oReplyUp = oPingUp.Send(strAuditIP);
                                            strRebootUp = oReplyUp.Status.ToString().ToUpper();
                                        }
                                        catch { }
                                        boolRebootUp = (strRebootUp == "SUCCESS");
                                        if (boolRebootUp == false)
                                        {
                                            int intRebootUpLeft = (60 - jj);
                                            oLog.AddEvent(strName, strSerial, "Device still powering on...waiting 3 seconds (Reply = " + strRebootUp + ") (" + intRebootUpLeft.ToString() + " tries left)", LoggingType.Debug);
                                            Thread.Sleep(3000);
                                        }
                                    }
                                    // At this point, the server should be back on...
                                    if (boolRebootUp == true)
                                    {
                                        oLog.AddEvent(strName, strSerial, "Device is back online (" + strRebootUp + ")...reboot script completed", LoggingType.Information);
                                        // Clear the flag to continue audits...
                                        intAuditReturn = (int)AuditStatus.Success;
                                    }
                                    else
                                    {
                                        oLog.AddEvent(strName, strSerial, "The IP address (" + strAuditIP + ") is NOT pinging..." + strRebootUp, LoggingType.Error);
                                        intAuditReturn = (int)AuditStatus.Error;
                                        strAuditName += "...power on";
                                    }
                                }
                                else
                                {
                                    oLog.AddEvent(strName, strSerial, "The IP address (" + strAuditIP + ") is STILL pinging..." + strRebootDown, LoggingType.Error);
                                    intAuditReturn = (int)AuditStatus.Error;
                                    strAuditName += "...power off";
                                }
                            }
                            else
                            {
                                strAuditName = "Audit Script # " + intScriptDetail.ToString() + "(" + strAuditName + ")";
                                intAuditReturn = oFunction.ExecuteVBScript(intServer, false, false, strAuditName, strName, strSerial, strAuditIP, strPath, strAuditScriptPath, "Script" + intScriptDetail.ToString() + "_", strEXE, "OPTIONS\\CV_AUDIT_SCRIPT_" + intScriptDetail.ToString() + "_", strExtension, strParameters, strScripts, strAdminUser, strAdminPass, intAuditTimeout, (oOperatingSystem.IsWindows2008(intOS) == false), false, intLogging, boolDeleteFiles);
                            }
                        }
                    }

                    AuditStatus oAuditStatus = (AuditStatus)intAuditReturn;
                    if (oAuditStatus == AuditStatus.Success || oAuditStatus == AuditStatus.Warning)
                    {
                        oAudit.UpdateServer(intAuditID, oAuditStatus, DateTime.Now.ToString());
                        string strAuditTemp = "Finished Audit: " + strAuditName + " (" + (oAuditStatus == AuditStatus.Success ? " Success" : "Warning") + ")<br/>";
                        oLog.AddEvent(strName, strSerial, strAuditTemp, LoggingType.Information);
                        strAuditResult += strAuditTemp;
                    }
                    else
                    {
                        intAuditIDError = intAuditID;
                        oAudit.UpdateServer(intAuditID, oAuditStatus, "");
                        if (oAuditStatus == AuditStatus.Error)
                            strAuditError = "Failed Audit: " + strAuditName;
                        else if (oAuditStatus == AuditStatus.TimedOut)
                            strAuditError = "Failed Audit (Timeout): " + strAuditName;
                        else if (oAuditStatus == AuditStatus.NetUseError)
                            strAuditError = "Failed Audit (NET USE ERROR): " + strAuditName;
                        else
                            strAuditError = "Unexpected Audit Error (" + intAuditReturn.ToString() + "): " + strAuditName;
                        if (boolMIS == false)
                            oOnDemand.UpdateStepDoneServerResult(intServer, intStep, strAuditError + "<br/>", true);
                        else
                            oServer.UpdateMISAudits(intServer, DateTime.Now.ToString());
                        break;
                    }
                }
                if (strAuditError == "")
                {
                    string strAuditTemp = "Audit scripts completed successfully";
                    oLog.AddEvent(strName, strSerial, strAuditTemp, LoggingType.Information);
                    strResult = strAuditResult + strAuditTemp;
                }
                else
                {
                    //boolAuditError = true;
                    strError = strAuditResult + strAuditError;
                    //oLog.AddEvent(strName, strSerial, strError, LoggingType.Error);
                }
            }
            oCalling.AuditCount = oCalling.AuditCount - 1;
            AddResult(strResult, strError, intAuditIDError);
        }
        private void AddResult(string strResult, string strError, int intAuditIDError)
        {
            if (strError == "")
            {
                if (boolMIS == false)
                {
                    //oServer.NextStep(intServer);
                    //oOnDemand.UpdateStepDoneServer(intServer, intStep, strResult, 0, false, false);

                    // Update to prevent it being kicked off again
                    oServer.UpdateStep(intServer, intStep + 1);
                    // Set the done status
                    oOnDemand.UpdateStepDoneServer(intServer, intStep, strResult, 0, false, false);
                    // Go back to current step
                    oServer.UpdateStep(intServer, intStep);
                    // Push through to next step
                    oServer.NextStep(intServer);
                    intStep++;
                }
                else
                    oServer.UpdateMISAudits(intServer, DateTime.Now.ToString());
            }
            else
            {
                oLog.AddEvent(strName, strSerial, strError, LoggingType.Error);
                if (boolMIS == false)
                    oOnDemand.UpdateStepDoneServer(intServer, intStep, strError, 1, false, false);
                else
                    oServer.UpdateMISAudits(intServer, DateTime.Now.ToString());

                // Generic Error Request
                Services oService = new Services(0, dsn);
                ResourceRequest oResourceRequest = new ResourceRequest(0, dsn);
                ServiceRequests oServiceRequest = new ServiceRequests(0, dsn);
                int intServerAuditErrorService = intService;
                int intServerAuditErrorItem = oService.GetItemId(intServerAuditErrorService);
                int intServerAuditErrorNumber = oResourceRequest.GetNumber(intRequest, intServerAuditErrorItem);
                oAudit.AddError(intRequest, intServerAuditErrorService, intServerAuditErrorNumber, intAuditIDError, intStep, boolMIS);
                int intError = oServer.AddError(intRequest, intServerAuditErrorItem, intServerAuditErrorNumber, intServer, intStep, strError);
                int intServerAuditError = oResourceRequest.Add(intRequest, intServerAuditErrorItem, intServerAuditErrorService, intServerAuditErrorNumber, "Server Audit Exception (" + strName + ")", 1, 0.00, 2, 1, 1, 1);
                if (oServiceRequest.NotifyApproval(intServerAuditError, intResourceRequestApprove, intEnvironment, "", dsnServiceEditor) == false)
                    oServiceRequest.NotifyTeamLead(intServerAuditErrorItem, intServerAuditError, intAssignPage, intViewPage, intEnvironment, "", dsnServiceEditor, dsnAsset, dsnIP, 0);
            }
        }
    }
}
