using System;
using System.Collections.Generic;
using System.Text;
using Tamir.SharpSsh;
using Tamir.Streams;
using System.Threading;
using NCC.ClearView.Application.Core;
using System.IO;
using System.Data;
using System.Diagnostics;

namespace ClearViewAP_Physical
{
    public class SolarisBuild
    {
        private int intServer = 0;
        private string strName = "";
        private string strTo = "";
        private string strCC = "";
        private string strSerial = "";
        private string strAsset = "";
        private int intAsset = 0;
        private string strILO = "";
        private SshShell oSSHshell;
        private int intStep = 0;
        private string dsn = "";
        private string dsnAsset = "";
        private string strScripts;
        private int intEnvironment;
        private int intLogging;
        private bool boolSSHDebug;

        private DateTime datStart;
        private int intTimeout = 5;
        private int intTimeoutStep = 0;
        private string strSSH = "";
        private Servers oServer;
        private Models oModel;
        private ModelsProperties oModelsProperties;
        private int intModel = 0;
        private int intModelParent = 0;
        private int intModelBootGroup = 0;
        private string strModel = "";
        private Settings oSetting;
        private OnDemand oOnDemand;
        private Solaris oSolaris;
        private Log oEventLog;
        private Variables oVariable;
        private Functions oFunction;
        private int intRequest = 0;
        private int intService = 0;
        private int intResourceRequestApprove = 0;
        private int intAssignPage = 0;
        private int intViewPage = 0;
        private string dsnServiceEditor;
        private string dsnIP;
        private bool boolEmailError = false;
        private string strReturnToALOM = "";

        //private string strSSH_Carriage = "\r";
        //private string strSubSSH = "ssh\\";
        private int[] arProcessing;

        public SolarisBuild(int _serverid, string _name, Variables _variable, string _cc, string _serial, string _asset, int _assetid, string _ilo, int _step, string _dsn, string _dsn_asset, string _scripts, int _environment, int _logging, bool _debug, int _requestid, int _serviceid, int _resourcerequestapprove, int _assignpage, int _viewpage, string _dsn_ip, string _dsn_service_editor, bool _email_error)
		{
            intServer = _serverid;
            strName = _name;
            oVariable = _variable;
            strCC = _cc;
            strSerial = _serial;
            strAsset = _asset;
            intAsset = _assetid;
            strILO = _ilo;
            intStep = _step;
            dsn = _dsn;
            dsnAsset = _dsn_asset;
            strScripts = _scripts;
            intEnvironment = _environment;
            intLogging = _logging;
            boolSSHDebug = _debug;
            intRequest = _requestid;
            intService = _serviceid;
            intResourceRequestApprove = _resourcerequestapprove;
            intAssignPage = _assignpage;
            intViewPage = _viewpage;
            dsnServiceEditor = _dsn_service_editor;
            dsnIP = _dsn_ip;
            boolEmailError = _email_error;
        }

        public void Begin()
        {
            datStart = DateTime.Now;
            arProcessing = new int[4] { 45, 47, 92, 124 };   // 92 = \, 124 = |, 47 = /, 45 = -
            strSSH = "";
            oModel = new Models(0, dsn);
            oModelsProperties = new ModelsProperties(0, dsn);
            oServer = new Servers(0, dsn);
            oSetting = new Settings(0, dsn);
            oOnDemand = new OnDemand(0, dsn);
            oSolaris = new Solaris(0, dsn);
            oEventLog = new Log(0, dsn);
            oFunction = new Functions(0, dsn, intEnvironment);
            strTo = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_PROVISIONING_SUPPORT");
            intModel = Int32.Parse(oServer.Get(intServer, "modelid"));
            if (intModel > 0)
            {
                strModel = oModelsProperties.Get(intModel, "name");
                Int32.TryParse(oModelsProperties.Get(intModel, "modelid"), out intModelParent);
                if (intModelParent > 0)
                    Int32.TryParse(oModel.Get(intModelParent, "boot_groupid"), out intModelBootGroup);
            }

            if (intModelBootGroup > 0) 
            {
                string strUsername = oModel.GetBootGroup(intModelBootGroup, "username");
                string strPassword = oModel.GetBootGroup(intModelBootGroup, "password");
                strReturnToALOM = oModel.GetBootGroup(intModelBootGroup, "return_to_alom");
                oEventLog.AddEvent(strName, strSerial, "Connecting to " + strILO + "... (U:" + strUsername + ", P:****)", LoggingType.Information);
                oSSHshell = new SshShell(strILO, strUsername, strPassword);
                oSSHshell.RemoveTerminalEmulationCharacters = true;
                oSSHshell.Connect();
                oEventLog.AddEvent(strName, strSerial, "Connected to " + strILO + "...sending commands...", LoggingType.Information);

                ThreadStart oReadingDoneSSH = new ThreadStart(ReadingDoneSSH);
                Thread oJobReadingDoneSSH = new Thread(oReadingDoneSSH);
                oJobReadingDoneSSH.Start();

                ReadingSSH();
            }
            else
                AddResult("The boot group of the model has not been configured ~ (ModelPropertyID = " + intModel.ToString() + ") (ModelID = " + intModelParent.ToString() + ") (ModelBootGroupID = " + intModelBootGroup.ToString() + ")");
        }

        private void ReadingSSH()
        {
            try
            {
                if (intServer > 0)
                {
                    string strRegularExpression = oModel.GetBootGroup(intModelBootGroup, "regular");
                    DataSet dsSSH = oModel.GetBootGroupStepNext(intModel, intServer);
                    bool boolCondition = false;
                    string strResultCondition = "";
                    string strResultSSH = "";
                    for (int ii = 0; ii < dsSSH.Tables[0].Rows.Count && oSSHshell.Connected == true && oSSHshell.ShellOpened == true; ii++)
                    {
                        string strWriteSSH = dsSSH.Tables[0].Rows[ii]["then_write"].ToString();
                        bool boolPower = (dsSSH.Tables[0].Rows[ii]["power"].ToString() == "1");
                        datStart = DateTime.Now;
                        intTimeoutStep = Int32.Parse(dsSSH.Tables[0].Rows[ii]["id"].ToString());
                        intTimeout = Int32.Parse(dsSSH.Tables[0].Rows[ii]["timeout"].ToString());
                        string strConditionSSH = dsSSH.Tables[0].Rows[ii]["wait_for"].ToString();

                        if (boolPower == true)
                        {
                            // Power on step completed, move to next step
                            AddResult("");
                            oEventLog.AddEvent(strName, strSerial, "POWER ON command sent...moving to next step", LoggingType.Information);
                        }

                        // Wait to execute next command
                        if (boolCondition == false)
                        {
                            DateTime datBegin = DateTime.Now;
                            oEventLog.AddEvent(strName, strSerial, "Waiting for response... (timeout = " + intTimeout.ToString() + " minutes) (Expression = " + strRegularExpression + ")", LoggingType.Information);
                            strResultSSH = ReplaceGarbageChars(oSSHshell.Expect(strRegularExpression));
                            strSSH += strResultSSH;
                            TimeSpan oSpan = DateTime.Now.Subtract(datBegin);
                            oEventLog.AddEvent(strName, strSerial, "Received Response... (took " + oSpan.TotalSeconds.ToString("0") + " seconds)", LoggingType.Information);

                            // **********************************************************************************************************************
                            // The following lines should be coded for specific models.  The point of the "strReturnToALOM" is to exit out of the 
                            // console once commands are sent, otherwise the console will show in use and the session might not close properly.
                            // **********************************************************************************************************************

                            // START: T6320 / T6340
                            if (strResultSSH.Contains("to return to ALOM") == true && strReturnToALOM == "")
                                strReturnToALOM = oSolaris.ParseOutput(strResultSSH, "Enter", "to return to ALOM");
                            // END: T6320 / T6340
                        }
                        else
                        {
                            strResultSSH = strResultCondition;
                            strResultCondition = "";
                        }

                        if (strConditionSSH == "" || strResultSSH.Contains(strConditionSSH) == true)
                        {
                            boolCondition = false;

                            if (oSSHshell.Connected == true && oSSHshell.ShellOpened == true)
                            {
                                // Execute next command
                                oEventLog.AddEvent(strName, strSerial, "Writing command : " + strWriteSSH, LoggingType.Information);
                                oSSHshell.WriteLine(strWriteSSH);
                                oEventLog.AddEvent(strName, strSerial, "Completed command : " + strWriteSSH, LoggingType.Information);
                            }
                            else
                                oEventLog.AddEvent(strName, strSerial, "oSSHshell has been closed...unable to write: " + strWriteSSH, LoggingType.Information);
                        }
                        else
                        {
                            // The conditional statement was not found, move to next step...
                            boolCondition = true;
                            strResultCondition = strResultSSH;
                            strResultSSH = "";
                        }

                        // Update database
                        oServer.AddOutput(intServer, strWriteSSH, strResultSSH);
                    }

                    // Either all commands are done, or the oSSHshell object was closed (due to error or timeout)
                    if (oSSHshell.Connected == true && oSSHshell.ShellOpened == true)
                    {
                        oEventLog.AddEvent(strName, strSerial, "Exited Step Configuration...waiting for last ouput...", LoggingType.Information);
                        // All commands are done - get the last output
                        strResultSSH = ReplaceGarbageChars(oSSHshell.Expect(strRegularExpression));
                        oEventLog.AddEvent(strName, strSerial, "Got Last Output", LoggingType.Information);
                        strSSH += strResultSSH;
                        // If applicable, run the "strReturnToALOM" command
                        if (strReturnToALOM != "")
                        {
                            if (oSSHshell.Connected == true && oSSHshell.ShellOpened == true)
                            {
                                oEventLog.AddEvent(strName, strSerial, "Writing command : " + strReturnToALOM, LoggingType.Information);
                                // Write command
                                oSSHshell.WriteLine(strReturnToALOM);
                                oEventLog.AddEvent(strName, strSerial, "Completed command : " + strReturnToALOM, LoggingType.Information);
                                // Update database
                                oServer.AddOutput(intServer, strReturnToALOM, strResultSSH);
                                // And get the "strReturnToALOM" output
                                oEventLog.AddEvent(strName, strSerial, "Waiting for return output... ", LoggingType.Information);
                                strResultSSH = ReplaceGarbageChars(oSSHshell.Expect(strRegularExpression));
                                oEventLog.AddEvent(strName, strSerial, "Got return output...", LoggingType.Information);
                                strSSH += strResultSSH;
                            }
                        }
                    }
                    else
                    {
                        // oSSHshell object was closed (due to error or timeout) - do nothing more...
                        oEventLog.AddEvent(strName, strSerial, "oSSHshell object was already closed... ", LoggingType.Information);
                    }

                    // Close the oSSHshell object
                    if (oSSHshell.Connected == true || oSSHshell.ShellOpened == true)
                    {
                        oSSHshell.Close();
                        oEventLog.AddEvent(strName, strSerial, "oSSHshell was closed... ", LoggingType.Information);
                    }

                    oEventLog.AddEvent(strName, strSerial, "Exited ReadingSSH Loop for SERVERID = " + intServer.ToString(), LoggingType.Information);
                }
            }
            catch (Exception ex)
            {
                string strError = "Physical Service (SolarisBuild - ReadingSSH): " + "(Error Message: " + ex.Message + ") (Source: " + ex.Source + ") (Stack Trace: " + ex.StackTrace + ") [" + System.Environment.UserName + "]";
                SystemError(intServer, intStep, strError, intAsset, intModel);
            }
        }
        private void ReadingDoneSSH()
        {
            try
            {
                bool boolTimeout = false;
                if (intServer > 0)
                {
                    string strDHCP = oServer.Get(intServer, "dhcp");
                    while (boolTimeout == false && oSSHshell.Connected == true && oSSHshell.ShellOpened == true && (strDHCP == "" || strDHCP == "0" || strDHCP == "SUCCESS"))
                    {
                        TimeSpan datSpan = DateTime.Now.Subtract(datStart);
                        if (intTimeout <= 0)
                            intTimeout = 180;   // Default: 4 hours = 60 x 3 = 180
                        int intMinutes = datSpan.Hours;
                        intMinutes = intMinutes * 60;
                        intMinutes += datSpan.Minutes;
                        boolTimeout = (intMinutes >= intTimeout);
                        int intSolarisDebug = 0;
                        Int32.TryParse(oSetting.Get("solaris_debug"), out intSolarisDebug);
                        if (boolSSHDebug == true && intSolarisDebug > 0)
                            oEventLog.AddEvent(strName, strSerial, "Solaris DEBUG: Checking SERVERID = " + intServer.ToString() + ", DHCP = " + strDHCP + ", timeout = " + intTimeout.ToString() + " <= " + intMinutes.ToString() + " [started = " + datStart.ToString() + "], boolTimeout = " + boolTimeout.ToString(), LoggingType.Information);
                        if (boolTimeout == true)
                        {
                            // Step has timed out, throw error and kill everything.
                            string strNow = datStart.Day.ToString() + datStart.Month.ToString() + datStart.Year.ToString() + datStart.Hour.ToString() + datStart.Minute.ToString() + datStart.Second.ToString() + datStart.Millisecond.ToString();
                            oServer.AddOutput(intServer, "BUILD_TIMEOUT_" + strNow, strSSH);
                            AddResult("The current step has reached its timeout ~ (Step = " + intTimeoutStep.ToString() + ") (Timeout = " + intTimeout.ToString() + " minutes)");
                        }

                        // Sleep 30 seconds before going again
                        Thread.Sleep(30000);
                        strDHCP = oServer.Get(intServer, "dhcp");
                    }

                    oEventLog.AddEvent(strName, strSerial, "Exited ReadingDoneSSH Loop for SERVERID = " + intServer.ToString() + ", DHCP = " + strDHCP, LoggingType.Information);

                    if (boolTimeout == false)
                    {
                        // DHCP has been set...completed with build...
                        oServer.AddOutput(intServer, "BUILD_SUCCESS", strSSH);
                        // AddResult("");   // No need to increment step since main thread will do that for us...just log the output.
                    }
                    
                    // Close the following to break the "ReadingSSH" thread (if still running)
                    if (oSSHshell.Connected == true || oSSHshell.ShellOpened == true)
                    {
                        oSSHshell.Close();
                        oEventLog.AddEvent(strName, strSerial, "oSSHshell was closed (error / timeout)... ", LoggingType.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                string strError = "Physical Service (SolarisBuild - ReadingDoneSSH): " + "(Error Message: " + ex.Message + ") (Source: " + ex.Source + ") (Stack Trace: " + ex.StackTrace + ") [" + System.Environment.UserName + "]";
                SystemError(intServer, intStep, strError, intAsset, intModel);
            }
        }
        private string ReplaceGarbageChars(string _replace)
        {
            StringBuilder strReturn = new StringBuilder();
            string strReplace = _replace;
            for (int ii = 0; ii < strReplace.Length; ii++)
            {
                char chrSSH = strReplace[ii];
                if ((int)chrSSH == 8)   // 8 = backspace
                {
                    // Hit a backspace, this means it is processing.  Check previous character.
                    char prevSSH = strReplace[ii - 1];
                    if (IsGarbageChar((int)prevSSH) == true)
                        strReturn.Remove(strReturn.Length - 1, 1);
                }
                else
                    strReturn.Append(chrSSH.ToString());
            }
            return strReturn.ToString();
        }
        private bool IsGarbageChar(int intChar)
        {
            // This function will remove the garbage / processing characters from the output (such as -\|/-\|/)
            bool boolGarbage = false;
            for (int ii = 0; ii < arProcessing.Length; ii++)
            {
                if (intChar == arProcessing[ii])
                {
                    boolGarbage = true;
                    break;
                }
            }
            return boolGarbage;
        }

        private void AddResult(string strError)
        {
            if (strError == "")
            {
                oOnDemand.UpdateStepDoneServer(intServer, intStep, oOnDemand.GetStep(intStep, "done"), 0, false, false);
                oServer.NextStep(intServer);
                intStep++;
            }
            else
            {
                oEventLog.AddEvent(strName, strSerial, strError, LoggingType.Error);
                //Functions oFunction = new Functions(0, dsn, intEnvironment);
                //Variables oVariable = new Variables(intEnvironment);
                oOnDemand.UpdateStepDoneServer(intServer, intStep, strError, 1, false, false);

                // Generic Error Request
                Services oService = new Services(0, dsn);
                ResourceRequest oResourceRequest = new ResourceRequest(0, dsn);
                ServiceRequests oServiceRequest = new ServiceRequests(0, dsn);
                int intProvisioningErrorService = intService;
                int intProvisioningErrorItem = oService.GetItemId(intProvisioningErrorService);
                int intProvisioningErrorNumber = oResourceRequest.GetNumber(intRequest, intProvisioningErrorItem);
                int intError = oServer.AddError(intRequest, intProvisioningErrorItem, intProvisioningErrorNumber, intServer, intStep, strError);
                int intProvisioningError = oResourceRequest.Add(intRequest, intProvisioningErrorItem, intProvisioningErrorService, intProvisioningErrorNumber, "Provisioning Error (" + strName + ")", 1, 0.00, 2, 1, 1, 1);
                if (oServiceRequest.NotifyApproval(intProvisioningError, intResourceRequestApprove, intEnvironment, "", dsnServiceEditor) == false)
                    oServiceRequest.NotifyTeamLead(intProvisioningErrorItem, intProvisioningError, intAssignPage, intViewPage, intEnvironment, "", dsnServiceEditor, dsnAsset, dsnIP, 0);
                if (boolEmailError == true)
                    oFunction.SendEmail("Auto-Provisioning ERROR: " + strName, strTo, strCC, "", "Auto-Provisioning ERROR: " + strName, "<p><b>This message is to inform you that the server " + strName + " has encountered an error and has been stopped!</b><p><p>Serial Number: " + strSerial.ToUpper() + "<br/>Asset Tag: " + strAsset.ToUpper() + "<br/>Model: " + strModel.ToUpper() + "<br/>Step #: " + intStep.ToString() + "<br/>Error: " + strError + "<br/>ILO: <a href=\"https://" + strILO + "\" target=\"_blank\">" + strILO + "</a></p><p>When this issue has been resolved, <a href=\"" + oVariable.URL() + "/admin/errors_server.aspx?id=" + intServer.ToString() + "\" target=\"_blank\">click here</a> to clear this error and continue with the build.</p>", true, false);
            }
        }
        //private void AddLogSSH(string _type)
        //{
        //    _type = oFunction.OnlyLettersAndNumbersFromString(_type);
        //    string strFolder = strScripts + strSubSSH;
        //    if (Directory.Exists(strFolder) == false)
        //        Directory.CreateDirectory(strFolder);

        //    string strFile = strFolder + intServer.ToString() + "_" + _type + ".txt";
        //    StreamWriter oWriterSSH = new StreamWriter(strFile);
        //    oWriterSSH.Write(strSSH);
        //    oWriterSSH.Flush();
        //    oWriterSSH.Close();
        //}

        private void SystemError(int _server, int _stepid, string _error, int _assetid, int _modelid)
        {
            try
            {
                Settings oSetting = new Settings(0, dsn);
                oSetting.SystemError(_server, 0, _stepid, _error, _assetid, _modelid, false, null, intEnvironment, dsnAsset);
            }
            catch (Exception exSystemError)
            {
                // If database is offline, it will cause a fatal error AND stop the service.  Stop that from happening by using a catch here.
            }
        }
    }
}
