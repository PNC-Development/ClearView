using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using System.Data.SqlClient;
using System.Timers;
using Microsoft.ApplicationBlocks.Data;
using System.Threading;
using NCC.ClearView.Application.Core;
using System.Net.NetworkInformation;

namespace ClearViewZeus
{
    public partial class Service : ServiceBase
    {
        private System.Timers.Timer oTimer = null;
        private double dblInterval;
        private string dsn;
        private string dsnServiceEditor;
        private string dsnZeus;
        private string dsnRemote;
        private int intEnvironment; 
        private int intProd;
        private int intWorkstationInstallStep;
        private int intLogging;
        private EventLog oLog;
        private string strScripts = "E:\\APPS\\CLV\\ClearViewZeus\\";
        private string strEMailIdsBCC = "";
        public Service()
        {
            InitializeComponent();
            try
            {
                if (EventLog.SourceExists("ClearView") == false)
                {
                    EventLog.CreateEventSource("ClearView", "ClearView");
                    EventLog.WriteEntry(String.Format("ClearView EventLog Created"), EventLogEntryType.Information);
                }
                oLog = new EventLog();
                oLog.Source = "ClearView";
                oLog.ModifyOverflowPolicy(OverflowAction.OverwriteAsNeeded, 0);
                oLog.MaximumKilobytes = long.Parse("16384");
                DataSet ds = new DataSet();
                ds.ReadXml(strScripts + "config.xml");
                dblInterval = Convert.ToDouble(ds.Tables[0].Rows[0]["interval"].ToString());
                intEnvironment = Int32.Parse(ds.Tables[0].Rows[0]["environment"].ToString());
                string strDSN = ds.Tables[0].Rows[0]["DSN"].ToString();
                string strDSNServiceEditor = ds.Tables[0].Rows[0]["ServiceEditorDSN"].ToString();
                string strDSNZeus = ds.Tables[0].Rows[0]["ZeusDSN"].ToString();
                string strDSNRemote = ds.Tables[0].Rows[0]["RemoteDSN"].ToString();
                dsn = ds.Tables[0].Rows[0][strDSN].ToString();
                dsnServiceEditor = ds.Tables[0].Rows[0][strDSNServiceEditor].ToString();
                dsnZeus = ds.Tables[0].Rows[0][strDSNZeus].ToString();
                dsnRemote = ds.Tables[0].Rows[0][strDSNRemote].ToString();
                intProd = Int32.Parse(ds.Tables[0].Rows[0]["prod"].ToString());
                intWorkstationInstallStep = Int32.Parse(ds.Tables[0].Rows[0]["workstation_install_step"].ToString());
                intLogging = Int32.Parse(ds.Tables[0].Rows[0]["logging"].ToString());
                oTimer = new System.Timers.Timer(dblInterval);
                oTimer.Elapsed += new ElapsedEventHandler(this.ServiceTimer_Tick);
            }
            catch
            {
                EventLog.WriteEntry(String.Format("ClearView Zeus Service initialization has failed - INVALID XML FILE"), EventLogEntryType.Error);
            }
        }

        protected override void OnStart(string[] args)
        {
            oLog.WriteEntry(String.Format("ClearView Zeus Service started."), EventLogEntryType.Information);
            oTimer.AutoReset = true;
            oTimer.Enabled = true;
            oTimer.Start();
        }

        protected override void OnStop()
        {
            oLog.WriteEntry(String.Format("ClearView Zeus Service stopped."), EventLogEntryType.Information);
            oTimer.AutoReset = false;
            oTimer.Enabled = false;
        }

        private void ServiceTimer_Tick(object sender, System.Timers.ElapsedEventArgs e)
        {
            oTimer.Stop();
            // *********************************************
            // ************  START Processing  *************
            // *********************************************
            if (intLogging > 1)
                oLog.WriteEntry(String.Format("ClearView Zeus Service TICK."), EventLogEntryType.Information);
            try
            {
                ThreadStart oJob = new ThreadStart(ServiceTick);
                Thread oThreadJob = new Thread(oJob);
                oThreadJob.Start();
            }
            catch (Exception ex)
            {
                oLog.WriteEntry(String.Format(ex.Message), EventLogEntryType.Error);
            }
            // *******************************************
            // ************  END Processing  *************
            // *******************************************
            oTimer.Start();
        }
        private void ServiceTick()
        {
            try
            {
                // Cleanup ZEUS table
                Zeus oZeus = new Zeus(0, dsnZeus);
                oZeus.DeleteResults();
                AD oAD = new AD(0, dsn, intEnvironment);
                Functions oFunction = new Functions(0, dsn, intEnvironment);
                Variables oVariable = new Variables(intEnvironment);
                Log oLog = new Log(0, dsn);
                // Check Servers for Zeus
                Servers oServer = new Servers(0, dsn);
                ServerName oServerName = new ServerName(0, dsn);
                DataSet ds = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT * FROM cv_servers WHERE dhcp = '0' AND deleted = 0 OR dhcp = 'SUCCESS' AND deleted = 0");
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    int intServer = Int32.Parse(dr["id"].ToString());
                    DataSet dsZeus = SqlHelper.ExecuteDataset(dsnZeus, CommandType.Text, "SELECT * FROM cv_zeus_builds WHERE serverid = " + intServer.ToString() + " AND dhcp IS NOT NULL AND deleted = 0");
                    if (dsZeus.Tables[0].Rows.Count > 0)
                    {
                        DataRow drZeus = dsZeus.Tables[0].Rows[0];
                        oLog.AddEvent(drZeus["name"].ToString(), drZeus["serial"].ToString(), "The DHCP address " + drZeus["dhcp"].ToString() + " was found in the BUILD table", LoggingType.Information);
                        SqlHelper.ExecuteNonQuery(dsn, CommandType.Text, "UPDATE cv_servers SET dhcp = '" + drZeus["dhcp"].ToString() + "', modified = getdate() WHERE id = " + intServer.ToString());
                        object o = SqlHelper.ExecuteScalar(dsn, CommandType.Text, "SELECT dhcp FROM cv_servers WHERE id = " + intServer.ToString());
                        if (o != null && o.ToString() != "" && o.ToString() != "0" && o.ToString() != "SUCCESS")
                        {
                            SqlHelper.ExecuteNonQuery(dsnZeus, CommandType.Text, "UPDATE cv_zeus_builds SET deleted = 1, modified = getdate() WHERE id = " + drZeus["id"].ToString());
                            SqlHelper.ExecuteNonQuery(dsnZeus, CommandType.Text, "UPDATE cv_zeus_builds SET deleted = 10, modified = getdate() WHERE deleted = 0 AND serial = '" + drZeus["serial"].ToString() + "'");
                            oLog.AddEvent(drZeus["name"].ToString(), drZeus["serial"].ToString(), "The DHCP address was updated and the BUILD record was deleted", LoggingType.Information);
                        }
                        // Check for Errors
                        DataSet dsError = oServer.GetErrors(intServer);
                        foreach (DataRow drError in dsError.Tables[0].Rows)
                        {
                            if (drError["fixed"].ToString() == "")
                            {
                                oServer.UpdateError(intServer, Int32.Parse(drError["step"].ToString()), 0, 0, true, "");
                                oLog.AddEvent(drZeus["name"].ToString(), drZeus["serial"].ToString(), "The error has been cleared and the build is now ready to continue", LoggingType.Information);
                            }
                        }
                    }
                }
                // Check VMware workstations for Zeus
                Workstations oWorkstation = new Workstations(0, dsn);
                ds = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT * FROM cv_workstation_virtual WHERE dhcp = '0' AND vmware = 1 AND deleted = 0 OR dhcp = 'SUCCESS' AND vmware = 1 AND deleted = 0");
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    int intWorkstation = Int32.Parse(dr["id"].ToString());
                    DataSet dsZeus = SqlHelper.ExecuteDataset(dsnZeus, CommandType.Text, "SELECT * FROM cv_zeus_builds WHERE vmware_workstationid = " + intWorkstation.ToString() + " AND dhcp IS NOT NULL AND deleted = 0");
                    if (dsZeus.Tables[0].Rows.Count > 0)
                    {
                        DataRow drZeus = dsZeus.Tables[0].Rows[0];
                        oLog.AddEvent(drZeus["name"].ToString(), drZeus["serial"].ToString(), "The DHCP address " + drZeus["dhcp"].ToString() + " was found in the BUILD table", LoggingType.Information);
                        SqlHelper.ExecuteNonQuery(dsn, CommandType.Text, "UPDATE cv_workstation_virtual SET dhcp = '" + drZeus["dhcp"].ToString() + "', modified = getdate() WHERE id = " + intWorkstation.ToString());
                        object o = SqlHelper.ExecuteScalar(dsn, CommandType.Text, "SELECT dhcp FROM cv_workstation_virtual WHERE id = " + intWorkstation.ToString());
                        if (o != null && o.ToString() != "" && o.ToString() != "0" && o.ToString() != "SUCCESS")
                        {
                            SqlHelper.ExecuteNonQuery(dsnZeus, CommandType.Text, "UPDATE cv_zeus_builds SET deleted = 1, modified = getdate() WHERE id = " + drZeus["id"].ToString());
                            oLog.AddEvent(drZeus["name"].ToString(), drZeus["serial"].ToString(), "The DHCP address was updated and the BUILD record was deleted", LoggingType.Information);
                        }
                        // Check for Errors
                        DataSet dsError = oWorkstation.GetVirtualErrors(intWorkstation);
                        foreach (DataRow drError in dsError.Tables[0].Rows)
                        {
                            if (drError["fixed"].ToString() == "")
                            {
                                oWorkstation.UpdateVirtualError(intWorkstation, Int32.Parse(drError["step"].ToString()), 0, 0);
                                oLog.AddEvent(drZeus["name"].ToString(), drZeus["serial"].ToString(), "The error has been cleared and the build is now ready to continue", LoggingType.Information);
                            }
                        }
                    }
                }
                /*
                // Check Virtual Workstations for Zeus
                Workstations oWorkstation = new Workstations(0, dsn);
                Forecast oForecast = new Forecast(0, dsn);
                Classes oClass = new Classes(0, dsn);
                Workstations oRemote = new Workstations(0, dsnRemote);
                ds = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT * FROM cv_workstation_virtual WHERE dhcp = '0' AND vmware = 0 AND deleted = 0 OR dhcp = 'SUCCESS' AND vmware = 0 AND deleted = 0");
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    int intWorkstation = Int32.Parse(dr["id"].ToString());
                    int intRemote = Int32.Parse(dr["remoteid"].ToString());
                    DataSet dsZeus = SqlHelper.ExecuteDataset(dsnZeus, CommandType.Text, "SELECT * FROM cv_zeus_builds WHERE workstationid = " + intWorkstation.ToString() + " AND dhcp IS NOT NULL AND deleted = 0");
                    if (dsZeus.Tables[0].Rows.Count > 0)
                    {
                        SqlHelper.ExecuteNonQuery(dsn, CommandType.Text, "UPDATE cv_workstation_virtual SET dhcp = '" + dsZeus.Tables[0].Rows[0]["dhcp"].ToString() + "', modified = getdate() WHERE id = " + intWorkstation.ToString());
                        oRemote.NextRemoteVirtual(intRemote);
                        object o = SqlHelper.ExecuteScalar(dsn, CommandType.Text, "SELECT dhcp FROM cv_workstation_virtual WHERE id = " + intWorkstation.ToString());
                        if (o != null && o.ToString() != "" && o.ToString() != "0" && o.ToString() != "SUCCESS")
                            SqlHelper.ExecuteNonQuery(dsnZeus, CommandType.Text, "UPDATE cv_zeus_builds SET deleted = 1, modified = getdate() WHERE id = " + dsZeus.Tables[0].Rows[0]["id"].ToString());
                    }
                    else if (dr["dhcp"].ToString() == "0")
                    {
                        DateTime datModified = DateTime.Parse(dr["modified"].ToString());
                        DateTime _now = DateTime.Now;
                        TimeSpan oSpan = _now.Subtract(datModified);
                        if (oSpan.Hours > 6)
                        {
                            string strWorkstation = oWorkstation.GetName(Int32.Parse(dr["nameid"].ToString()));
                            Ping oPing = new Ping();
                            string strStatus = "";
                            try
                            {
                                PingReply oReply = oPing.Send(strWorkstation);
                                strStatus = oReply.Status.ToString().ToUpper();
                                if (strStatus == "SUCCESS")
                                    strStatus = Convert.ToString(oReply.Address);
                                else
                                    strStatus = "";
                            }
                            catch { }
                            if (strStatus != "")
                            {
                                if (intLogging > 0)
                                    oLog.WriteEntry(String.Format("PING SUCCESS: " + strWorkstation), EventLogEntryType.Information);
                                dsZeus = SqlHelper.ExecuteDataset(dsnZeus, CommandType.Text, "SELECT * FROM cv_zeus_builds WHERE name = '" + strWorkstation + "' AND dhcp IS NULL AND deleted = 0");
                                if (dsZeus.Tables[0].Rows.Count > 0)
                                    SqlHelper.ExecuteNonQuery(dsnZeus, CommandType.Text, "UPDATE cv_zeus_builds SET dhcp = '" + strStatus + "', modified = getdate() WHERE id = " + dsZeus.Tables[0].Rows[0]["id"].ToString());
                            }
                            else if (intLogging > 0)
                                oLog.WriteEntry(String.Format("PING UNSUCCESSFUL: " + strWorkstation), EventLogEntryType.Warning);
                        }
                    }
                }
                // Sync Workstations
                ds = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT * FROM cv_workstation_virtual WHERE deleted = 0 AND vmware = 0 AND completed IS NULL");
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    int intWorkstation = Int32.Parse(dr["id"].ToString());
                    int intRemote = Int32.Parse(dr["remoteid"].ToString());
                    object o = SqlHelper.ExecuteScalar(dsnRemote, CommandType.Text, "SELECT step FROM cv_virtual_workstations WHERE id = " + intRemote.ToString());
                    if (o != null && o.ToString() != "")
                        SqlHelper.ExecuteDataset(dsn, CommandType.Text, "UPDATE cv_workstation_virtual SET step = " + o.ToString() + " WHERE id = " + intWorkstation.ToString());
                }
                // Check Workstations for Installed Applications
                ds = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT * FROM cv_workstation_virtual WHERE step = " + intWorkstationInstallStep.ToString() + " AND vmware = 0 AND deleted = 0");
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    int intWorkstation = Int32.Parse(dr["id"].ToString());
                    int intRemote = Int32.Parse(dr["remoteid"].ToString());
                    string strName = oWorkstation.GetName(Int32.Parse(dr["nameid"].ToString()));
                    OnDemand oOnDemand = new OnDemand(0, dsn);
                    DataSet dsComponents = oWorkstation.GetComponentsSelected(intWorkstation);
                    if (dsComponents.Tables[0].Rows.Count == 0)
                        oOnDemand.UpdateStepDoneWorkstation(intWorkstation, intWorkstationInstallStep, "No components to install", 0, false, false);
                    else
                    {
                        foreach (DataRow drComponent in dsComponents.Tables[0].Rows)
                        {
                            int intComponent = Int32.Parse(drComponent["componentid"].ToString());
                            if (drComponent["user_group"].ToString() != "")
                            {
                                oAD.JoinGroup(strName, drComponent["user_group"].ToString());
                                strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_ALERT");
                                oFunction.SendEmail("Virtual Workstation Component Installation", drComponent["notifications"].ToString(), "", strEMailIdsBCC, "Virtual Workstation Component Installation", "<p><b>This message is to inform you that the following component(s) were configured for workstation " + strName + "</b></p><p> - " + drComponent["name"].ToString() + "</p>", true, false);
                                oOnDemand.UpdateStepDoneWorkstation(intWorkstation, intWorkstationInstallStep, "Successfully installed " + drComponent["name"].ToString() + "<br/>", 0, true, false);
                            }
                            else
                                oOnDemand.UpdateStepDoneWorkstation(intWorkstation, intWorkstationInstallStep, "Unable to install " + drComponent["name"].ToString() + " (currently only configured for user group installations)<br/>", 0, true, false);
                            oWorkstation.UpdateComponents(intWorkstation, intComponent, 2);
                        }
                    }
                    oRemote.NextRemoteVirtual(intRemote);
                }
                if (intProd == 1)
                {
                    // Check Workstations for completion
                    Users oUser = new Users(0, dsn);
                    ds = SqlHelper.ExecuteDataset(dsnRemote, CommandType.Text, "SELECT * FROM cv_virtual_workstations WHERE completed IS NOT NULL AND deleted = 0");
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        int intAnswer = Int32.Parse(dr["answerid"].ToString());
                        string strName = dr["name"].ToString();
                        string strHost = dr["hostname"].ToString();
                        string strEmail = "";
                        string strCC = "";
                        DataSet dsNotify = SqlHelper.ExecuteDataset(dsnRemote, CommandType.Text, "SELECT * FROM cv_virtual_workstations_notify WHERE name = '" + strName + "'");
                        foreach (DataRow drNotify in dsNotify.Tables[0].Rows)
                            strEmail += drNotify["xid"].ToString() + ";";
                        string strAccounts = "";
                        DataSet dsUsers = SqlHelper.ExecuteDataset(dsnRemote, CommandType.Text, "SELECT * FROM cv_virtual_workstations_accounts WHERE name = '" + strName + "'");
                        foreach (DataRow drUser in dsUsers.Tables[0].Rows)
                        {
                            if (drUser["admin"].ToString() == "1")
                                strAccounts += oUser.GetFullName(drUser["xid"].ToString()) + " (Administrator)<br/>";
                            else if (drUser["remote"].ToString() == "1")
                                strAccounts += oUser.GetFullName(drUser["xid"].ToString()) + " (Remote Access)<br/>";
                        }
                        int intClass = Int32.Parse(oForecast.GetAnswer(intAnswer, "classid"));
                        if (oClass.IsProd(intClass))
                            strCC = oVariable.NotifyWorkstationProd();
                        if (intLogging > 0)
                            oLog.WriteEntry(String.Format("Attempting to send email for " + strName), EventLogEntryType.Information);
                        if (strAccounts == "")
                            strAccounts = "Accounts were not requested at this time. To obtain access to this workstation, please fill out a LAN Access Form";
                        string strKnowledge = "http://knova.ntl-city.com/selfservice/documentLink.do?externalID=KN10319";
                        if (strKnowledge != "")
                        {
                            strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_PROVISIONING_ALERT,EMAILGRP_WORKSTATION");
                            oFunction.SendEmail("Virtual Workstation Notification: " + strName, strEmail, strCC, strEMailIdsBCC, "Virtual Workstation Notification: " + strName, "<p><b>This message is to inform you that the workstation " + strName + " has been auto-provisioned successfully!</b><p><p>This workstation was created on host <b>" + strHost + "</b>.</p><p>As requested, the following users have been granted rights to this workstation:<br/>" + strAccounts + "</p><p>If you are having problems connecting to your virtual workstation, <a href=\"" + strKnowledge + "\" target=\"_blank\"/>please click here</a> to view a helpful knowledge base article published by the support team.</p><p><b>NOTE:</b> This is automated email sent from the ClearView Auto-Provisioning tool. Please do not respond to this message.</p><p>If you would like to learn more about this process, <a href=\"" + oVariable.URL() + "/info.htm\" target=\"_blank\"/>please click here</a>.</p>", true, false);
                        }
                        else
                        {
                            strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_PROVISIONING_ALERT");
                            oFunction.SendEmail("Virtual Workstation Notification: " + strName, strEmail, strCC, strEMailIdsBCC, "Virtual Workstation Notification: " + strName, "<p><b>This message is to inform you that the workstation " + strName + " has been auto-provisioned successfully!</b><p><p>This workstation was created on host <b>" + strHost + "</b>.</p><p>As requested, the following users have been granted rights to this workstation:<br/>" + strAccounts + "</p><p><b>NOTE:</b> This is automated email sent from the ClearView Auto-Provisioning tool. Please do not respond to this message.</p><p>If you would like to learn more about this process, <a href=\"" + oVariable.URL() + "/info.htm\" target=\"_blank\"/>please click here</a>.</p>", true, false);
                        }
                        if (intLogging > 0)
                            oLog.WriteEntry(String.Format("Deleting remote workstation ID " + dr["id"].ToString()), EventLogEntryType.Information);
                        SqlHelper.ExecuteNonQuery(dsnRemote, CommandType.Text, "UPDATE cv_virtual_workstations SET deleted = 1 WHERE id = " + dr["id"].ToString() + " AND deleted = 0");
                    }
                }
                */
            }
            catch (Exception ex)
            {
                string strError = "Zeus Service: " + "(Error Message: " + ex.Message + ") ~ (Source: " + ex.Source + ") (Stack Trace: " + ex.StackTrace + ") [" + System.Environment.UserName + "]";
                oLog.WriteEntry(strError, EventLogEntryType.Error);
                SystemError(strError);
                oLog.WriteEntry(String.Format(ex.Message), EventLogEntryType.Error);
            }
        }
        private void SystemError(string _error)
        {
            try
            {
                Settings oSetting = new Settings(0, dsn);
                oSetting.SystemError(0, 0, 0, _error, 0, 0, false, null, intEnvironment, "");
            }
            catch (Exception exSystemError)
            {
                // If database is offline, it will cause a fatal error AND stop the service.  Stop that from happening by using a catch here.
                oLog.WriteEntry("The database is currently offline.  Error message = " + exSystemError.Message, EventLogEntryType.Error);
            }
        }

        //private void InstallTick()
        //{
        //    try
        //    {
        //        Workstations oWorkstation = new Workstations(0, dsn);
        //        Functions oFunction = new Functions(0, dsn, intEnvironment);
        //        Variables oVariable = new Variables(intEnvironment);
        //        DataSet dsInstalls = oWorkstation.GetComponents();
        //        if (dsInstalls.Tables[0].Rows.Count > 0)
        //        {
        //            int intWorkstation = Int32.Parse(dsInstalls.Tables[0].Rows[0]["workstationid"].ToString());
        //            int intRemote = Int32.Parse(dsInstalls.Tables[0].Rows[0]["remoteid"].ToString());
        //            string strDHCP = dsInstalls.Tables[0].Rows[0]["dhcp"].ToString();
        //            string strName = oWorkstation.GetName(Int32.Parse(dsInstalls.Tables[0].Rows[0]["nameid"].ToString()));
        //            if (strDHCP == "" || strDHCP == "0")
        //                strName = "";
        //            else if (strDHCP != "SUCCESS")
        //                strName = strDHCP;
        //            if (strName != "")
        //            {
        //                DataSet dsActive = oWorkstation.GetComponentsActive(intWorkstation);
        //                if (dsActive.Tables[0].Rows.Count == 0)
        //                {
        //                    if (dsInstalls.Tables[0].Rows[0]["sms_install"].ToString() == "1")
        //                    {
        //                        DataSet dsWorkstation = oWorkstation.GetVirtual(intWorkstation);
        //                        int intAnswer = Int32.Parse(dsWorkstation.Tables[0].Rows[0]["answerid"].ToString());
        //                        int intComponent = Int32.Parse(dsInstalls.Tables[0].Rows[0]["componentid"].ToString());
        //                        oWorkstation.UpdateComponent(intWorkstation, intComponent, 0);
        //                        DataSet dsScripts = oServer.GetComponentScripts(intComponent, 1);
        //                        DateTime _now = DateTime.Now;
        //                        string strNow = _now.Day.ToString() + _now.Month.ToString() + _now.Year.ToString() + _now.Hour.ToString() + _now.Minute.ToString() + _now.Second.ToString() + _now.Millisecond.ToString();
        //                        string strBatch1 = strScripts + strSub + intAnswer.ToString() + "_" + strNow + "_install_1.bat";
        //                        // ********** START : CHANGED CODE ON 7/31/2008 TO BATCH FILE COPY *******************
        //                        // 1st part - create BAT file to copy to server (install_1.bat)
        //                        StreamWriter oWriter1 = new StreamWriter(strBatch1);
        //                        foreach (DataRow drScript in dsScripts.Tables[0].Rows)
        //                            oWriter1.WriteLine(oFunction.ProcessLine(drScript["script"].ToString(), dsServer.Tables[0].Rows[0]));
        //                        oWriter1.Flush();
        //                        oWriter1.Close();
        //                        // 2nd part - create BAT file to do the copy (install_2.bat)
        //                        string strBatch2 = strScripts + strSub + intAnswer.ToString() + "_" + strNow + "_install_2.bat";
        //                        StreamWriter oWriter2 = new StreamWriter(strBatch2);
        //                        oWriter2.WriteLine("F:");
        //                        oWriter2.WriteLine("cd " + strScripts + strSub);
        //                        oWriter2.WriteLine("net use \\\\" + strIP + "\\C$ /user:onevoice 4AdminW03");
        //                        oWriter2.WriteLine("mkdir \\\\" + strIP + "\\C$\\OPTIONS");
        //                        oWriter2.WriteLine("copy " + strBatch1 + " \\\\" + strIP + "\\C$\\OPTIONS\\CV_INSTALL_" + strNow + ".BAT");
        //                        oWriter2.Flush();
        //                        oWriter2.Close();
        //                        // 3rd part - run the batch file to perform copy
        //                        string strFile1 = strScripts + strSub + intAnswer.ToString() + "_" + strNow + "_install_1.vbs";
        //                        StreamWriter oWriter3 = new StreamWriter(strFile1);
        //                        oWriter3.WriteLine("Dim objShell");
        //                        oWriter3.WriteLine("Set objShell = CreateObject(\"WScript.Shell\")");
        //                        oWriter3.WriteLine("objShell.Run(\"cmd.exe /c " + strBatch2 + "\")");
        //                        oWriter3.WriteLine("Set objShell = Nothing");
        //                        oWriter3.Flush();
        //                        oWriter3.Close();
        //                        ILaunchScript oScript1 = new SimpleLaunchWsh(strFile1, "", true, 5) as ILaunchScript;
        //                        oScript1.Launch();
        //                        // 4th part - file has been copied, do the PSEXEC to install application
        //                        System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo(strScripts + "psexec");
        //                        info.WorkingDirectory = strScripts;
        //                        info.Arguments = "\\\\" + strIP + " -u onevoice -p 4AdminW03 -e -i cmd.exe /c C:\\OPTIONS\\CV_INSTALL_" + strNow + ".BAT";
        //                        System.Diagnostics.Process proc = System.Diagnostics.Process.Start(info);
        //                        proc.WaitForExit();
        //                        proc.Close();
        //                        // 5th part - create BAT file to delete the copy (install_3.bat)
        //                        string strBatch3 = strScripts + strSub + intAnswer.ToString() + "_" + strNow + "_install_3.bat";
        //                        StreamWriter oWriter4 = new StreamWriter(strBatch3);
        //                        //oWriter4.WriteLine("del \\\\" + strIP + "\\C$\\OPTIONS\\CV_INSTALL_" + strNow + ".BAT");
        //                        oWriter4.WriteLine("net use \\\\" + strIP + "\\C$ /dele");
        //                        oWriter4.Flush();
        //                        oWriter4.Close();
        //                        // 6th part - run the batch file to perform copy
        //                        string strFile2 = strScripts + strSub + intAnswer.ToString() + "_" + strNow + "_install_2.vbs";
        //                        StreamWriter oWriter5 = new StreamWriter(strFile2);
        //                        oWriter5.WriteLine("Dim objShell");
        //                        oWriter5.WriteLine("Set objShell = CreateObject(\"WScript.Shell\")");
        //                        oWriter5.WriteLine("objShell.Run(\"cmd.exe /c " + strBatch3 + "\")");
        //                        oWriter5.WriteLine("Set objShell = Nothing");
        //                        oWriter5.Flush();
        //                        oWriter5.Close();
        //                        ILaunchScript oScript2 = new SimpleLaunchWsh(strFile2, "", true, 5) as ILaunchScript;
        //                        oScript2.Launch();
        //                        // ********** END : CHANGED CODE ON 7/31/2008 TO BATCH FILE COPY *******************
        //                        //StreamWriter oWriter = new StreamWriter(strFile);
        //                        //foreach (DataRow drScript in dsScripts.Tables[0].Rows)
        //                        //    oWriter.WriteLine(oFunction.ProcessLine(drScript["script"].ToString(), dsServer.Tables[0].Rows[0]));
        //                        //oWriter.Flush();
        //                        //oWriter.Close();
        //                        //System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo(strScripts + "psexec");
        //                        //info.WorkingDirectory = strScripts;
        //                        //info.Arguments = oFunction.ProcessLine(dsInstalls.Tables[0].Rows[0]["script"].ToString(), dsServer.Tables[0].Rows[0]) + " " + strFile;
        //                        //if (intLogging > 0)
        //                        //    oLog.WriteEntry("PSEXEC Script = " + oFunction.ProcessLine(dsInstalls.Tables[0].Rows[0]["script"].ToString(), dsServer.Tables[0].Rows[0]) + " " + strFile, EventLogEntryType.SuccessAudit);
        //                        //System.Diagnostics.Process proc = System.Diagnostics.Process.Start(info);
        //                        //proc.WaitForExit();
        //                        //proc.Close();
        //                        oWorkstation.UpdateComponent(intWorkstation, intComponent, 1);
        //                    }
        //                    else
        //                    {
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        oLog.WriteEntry(String.Format(ex.Message), EventLogEntryType.Error);
        //    }
        //}

    }
}
