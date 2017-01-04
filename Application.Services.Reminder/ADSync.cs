using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using NCC.ClearView.Application.Core;
using System.IO;
using System.Data;
using System.Diagnostics;
using System.DirectoryServices;

namespace ClearViewService
{
    public class ADSync
    {
        int intSleep = 0;
        double dblSleep = 0.00;
        bool boolUpdate = false;
        int intLog = 0;
        int intEnvironment = 0;
        private string dsn = "";
        private string dsnAsset = "";
        private string dsnIP;
        private string dsnServiceEditor;

        private Functions oFunction;
        private AD oAD;
        private EventLog oEventLog;

        public ADSync(double _sleep, bool _update, EventLog _event_log, int _log, int _environment, string _dsn, string _dsn_asset, string _dsn_ip, string _dsn_service_editor)
        {
            dblSleep = _sleep;
            boolUpdate = _update;
            oEventLog = _event_log;
            intLog = _log;
            intEnvironment = _environment;
            dsn = _dsn;
            dsnAsset = _dsn_asset;
            dsnIP = _dsn_ip;
            dsnServiceEditor = _dsn_service_editor;
        }

        public void Begin()
        {
            oAD = new AD(0, dsn, intEnvironment);
            oFunction = new Functions(0, dsn, intEnvironment);

            double dblWait = dblSleep * 60.00;  // Minutes to seconds
            dblWait = dblWait * 1000.00;        // Seconds to milliseconds
            intSleep = Int32.Parse(dblWait.ToString("0"));

            ThreadStart oThreadStart = new ThreadStart(Go);
            Thread oThread = new Thread(oThreadStart);
            oThread.Start();
        }

        private void Go()
        {
            bool boolError = false;
            try
            {
                // Write to Event Log
                if (intLog > 0)
                    oEventLog.WriteEntry("Active Directory Sync Started", EventLogEntryType.Information);

                // Run
                int intSync = oAD.Sync(dblSleep, boolUpdate);
                string strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_AD_SYNC");
                if (strEMailIdsBCC != "")
                    oFunction.SendEmail("Active Directory Sync", strEMailIdsBCC, "", "", "Active Directory Sync", "Here are the results of the Active Directory Sync..." + Environment.NewLine + Environment.NewLine + oAD.GetSync(intSync, "results"), true, true);

                // Write to Event Log
                if (intLog > 0)
                    oEventLog.WriteEntry("Active Directory Sync Finished...sleeping " + intSleep + " milliseconds...", EventLogEntryType.Information);
            }
            catch (Exception ex)
            {
                boolError = true;
                string strError = "AD Sync: " + "(Error Message: " + ex.Message + ") ~ (Source: " + ex.Source + ") (Stack Trace: " + ex.StackTrace + ") [" + System.Environment.UserName + "]";
                oEventLog.WriteEntry(strError, EventLogEntryType.Error);
                SystemError(strError);
            }

            if (boolError == false)
            {
                // Sleep
                Thread.Sleep(intSleep);
                // Re-run
                Go();
            }
        }

        private void SystemError(string _error)
        {
            SystemError(0, 0, _error, 0, 0);
        }
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
                oEventLog.WriteEntry("The database is currently offline.  Error message = " + exSystemError.Message, EventLogEntryType.Error);
            }
        }
    }
}
