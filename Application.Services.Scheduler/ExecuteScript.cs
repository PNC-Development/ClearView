using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using NCC.ClearView.Application.Core;

namespace Application.Services.Scheduler
{
    public class ExecuteScript
    {
        public int id { get; set; }
        public string name { get; set; }
        public string server { get; set; }
        public int credentials { get; set; }
        public string parameters { get; set; }
        public int timeout { get; set; }
        public bool privledges { get; set; }
        public bool interactive { get; set; }
        public string location { get; set; }
        public string dsn { get; set; }
        public int environment { get; set; }
        public int logging { get; set; }
        public EventLog oEventLog { get; set; }
        public NCC.ClearView.Application.Core.Scheduler oScheduler { get; set; }

        public ExecuteScript(int _id, string _name, string _server, int _credentials, string _parameters, int _timeout, bool _privledges, bool _interactive, string _location, string _dsn, int _environment, int _logging, EventLog _EventLog)
        {
            id = _id;
            name = _name;
            server = _server;
            credentials = _credentials;
            parameters = _parameters;
            timeout = _timeout;
            privledges = _privledges;
            interactive = _interactive;
            location = _location;
            dsn = _dsn;
            environment = _environment;
            logging = _logging;
            oEventLog = _EventLog;
        }
        public void Begin()
        {
            // Run Audit Scripts in Multi-Threaded Fashion
            ThreadStart oThreadStart = new ThreadStart(Script);
            Thread oThread = new Thread(oThreadStart);
            oThread.Start();
        }
        private void Script()
        {
            try
            {
                Variables oVariable = new Variables(environment);
                Functions oFunction = new Functions(0, dsn, environment);
                oScheduler = new NCC.ClearView.Application.Core.Scheduler(0, dsn);
                oScheduler.Status(id, SchedulerStatus.Running, DateTime.Now.ToString(), -1);

                if (String.IsNullOrEmpty(server) == false)
                    server = "\\\\" + server;
                Variables oCredentials = new Variables(credentials);
                string user = oCredentials.Domain() + "\\" + oCredentials.ADUser();
                string pass = oCredentials.ADPassword();
                int intTimeout = (timeout * 60 * 1000);     // convert to milliseconds
                //int intReturn = 0;
                if (interactive)
                    parameters = "-i " + parameters;
                if (privledges)
                    parameters = "-h " + parameters;

                bool boolTimeout = false;
                ProcessStartInfo infoPsExec = new ProcessStartInfo(location + "psexec");
                infoPsExec.WorkingDirectory = location;
                string command = server + " -u " + user + " -p {0} " + parameters;
                infoPsExec.Arguments = string.Format(command, pass);
                LogIt("Job \"" + name + "\" Starting PSEXEC: " + location + "psexec " + string.Format(command, "***"), false);
                Process procPsExec = Process.Start(infoPsExec);
                procPsExec.WaitForExit(intTimeout);
                if (procPsExec.HasExited == false)
                {
                    LogIt("Job \"" + name + "\" Timed Out after " + timeout.ToString() + " minute(s)...", true);
                    oScheduler.Status(id, SchedulerStatus.Waiting, DateTime.Now.ToString(), 1);
                    procPsExec.Kill();
                    boolTimeout = true;
                }
                else
                {
                    if (procPsExec.ExitCode == 0)   // 0 = Success
                    {
                        LogIt("Job \"" + name + "\" Exited (" + procPsExec.ExitCode.ToString() + ")", false);
                        oScheduler.Status(id, SchedulerStatus.Waiting, DateTime.Now.ToString(), 0);
                    }
                    else
                    {
                        LogIt("Job \"" + name + "\" Exited (" + procPsExec.ExitCode.ToString() + ")", true);
                        oScheduler.Status(id, SchedulerStatus.Waiting, DateTime.Now.ToString(), 1);
                    }
                }
                if (boolTimeout)
                {
                    string strEMails = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_ERROR");
                    oFunction.SendEmail("Timeout", strEMails, "", "", "SSIS Job \"" + name + "\" Timeout", "This message is to inform you that job \"" + name + "\" timed out after " + intTimeout.ToString() + "minute(s)", false, true);
                }
                //if (boolTimeout == false)
                //    intReturn = procPsExec.ExitCode;
                procPsExec.Close();
            }
            catch (Exception ex)
            {
                try
                {
                    string strError = "Scheduler Service (" + name + "): " + "(Error Message: " + ex.Message + ") ~ (Source: " + ex.Source + ") (Stack Trace: " + ex.StackTrace + ") [" + System.Environment.UserName + "]";
                    LogIt(strError, true);
                }
                catch
                {
                }
            }
        }
        private void LogIt(string message, bool error)
        {
            if (logging >= 0)
                oEventLog.WriteEntry(message, (error ? EventLogEntryType.Error : EventLogEntryType.Information));
            oScheduler.AddLog(id, message, (error ? 1 : 0));
        }
    }
}
