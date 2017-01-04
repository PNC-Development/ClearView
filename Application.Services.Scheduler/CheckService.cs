using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using NCC.ClearView.Application.Core;

namespace Application.Services.Scheduler
{
    public class CheckService
    {
        public string name { get; set; }
        public DataSet builds { get; set; }
        public string[] steps { get; set; }
        public int restart_hours { get; set; }
        public int start_timeout { get; set; }
        public string column { get; set; }
        public string dsn { get; set; }
        public int environment { get; set; }
        public int logging { get; set; }
        public EventLog oEventLog { get; set; }
        public Log oLog { get; set; }

        public CheckService(string _name, DataSet _builds, string[] _steps, string _column, int _restart_hours, int _start_timeout, string _dsn, int _environment, int _logging, EventLog _EventLog)
        {
            name = _name;
            builds = _builds;
            steps = _steps;
            column = _column;
            restart_hours = _restart_hours;
            start_timeout = _start_timeout;
            dsn = _dsn;
            environment = _environment;
            logging = _logging;
            oEventLog = _EventLog;
            oLog = new Log(0, dsn);
        }
        public void Begin()
        {
            // Run Audit Scripts in Multi-Threaded Fashion
            ThreadStart oThreadStart = new ThreadStart(Check);
            Thread oThread = new Thread(oThreadStart);
            oThread.Start();
        }
        private void Check()
        {
            Functions oFunction = new Functions(0, dsn, environment);
            try
            {
                List<string> restart = new List<string>();

                ServiceController service = new ServiceController(name);
                bool ServiceIsInstalled = false;
                try
                {
                    string ServiceName = service.DisplayName;
                    ServiceIsInstalled = true;
                }
                catch (InvalidOperationException) { }
                finally
                {
                    service.Close();
                }
                if (ServiceIsInstalled)
                {
                    if (service.Status == ServiceControllerStatus.Stopped)
                    {
                        LogIt("Service \"" + name + "\" is stopped. Starting...", false);
                        restart.Add("0");
                    }
                    else
                    {
                        if (logging >= 2)
                            LogIt("Checking " + builds.Tables[0].Rows.Count.ToString() + " record(s)", false);
                        foreach (DataRow build in builds.Tables[0].Rows)
                        {
                            bool exclude = false;
                            foreach (string step in steps)
                            {
                                if (build["step"].ToString() == step)
                                {
                                    exclude = true;
                                    break;
                                }
                            }
                            if (exclude == false)
                            {
                                // Step is not excluded.  See if the TimeSpan is out of acceptable range
                                DateTime started = DateTime.Parse(build["started"].ToString());
                                TimeSpan difference = DateTime.Now.Subtract(started);
                                if (difference.TotalHours >= restart_hours)
                                {
                                    LogIt(build[column].ToString() + " - The timespan of " + difference.TotalHours.ToString() + " hour(s) has exceeded the threshold (" + restart_hours.ToString() + ")", false);
                                    restart.Add(build[column].ToString());
                                }
                            }
                        }
                    }

                    if (restart.Count > 0)
                    {
                        // Restart
                        LogIt("Recylcling service \"" + name + "\" (" + start_timeout.ToString() + " milliseconds)", false);
                        int millisec1 = Environment.TickCount;
                        if (service.Status != ServiceControllerStatus.Stopped)
                        {
                            TimeSpan Time = TimeSpan.FromMilliseconds(start_timeout);

                            service.Stop();
                            service.WaitForStatus(ServiceControllerStatus.Stopped, Time);
                            LogIt("Service \"" + name + "\" has been stopped. Now restarting...", false);
                        }

                        // count the rest of the timeout
                        int millisec2 = Environment.TickCount;
                        TimeSpan Time2 = TimeSpan.FromMilliseconds(start_timeout - (millisec2 - millisec1));

                        service.Start();
                        service.WaitForStatus(ServiceControllerStatus.Running, Time2);
                        LogIt("Service \"" + name + "\" restart completed", false);

                        string strEMails = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_ERROR");
                        StringBuilder strBuilds = new StringBuilder();
                        if (restart.Count > 1 || restart[0] != "0")
                        {
                            foreach (string device in restart)
                                strBuilds.Append(device + System.Environment.NewLine);
                            oFunction.SendEmail("Restart", strEMails, "", "", name + " Restarted!", "This message is to inform you that the " + name + " service was successfully restarted due to lack of activity" + System.Environment.NewLine + System.Environment.NewLine + "The following build(s) will need to be fixed since they are currently in a hung state:" + System.Environment.NewLine + strBuilds.ToString(), false, true);
                        }
                        else
                            oFunction.SendEmail("Restart", strEMails, "", "", name + " Started!", "This message is to inform you that the " + name + " service was in a stopped state and was successfully started" + System.Environment.NewLine + System.Environment.NewLine + "You should check the build queue to make sure there are no builds in a hung state", false, true);
                    }
                }
                else if (logging >= 0)
                    LogIt("Service \"" + name + "\" is not installed", false);
            }
            catch (Exception ex)
            {
                try
                {
                    string strError = "Restart Service (" + name + "): " + "(Error Message: " + ex.Message + ") ~ (Source: " + ex.Source + ") (Stack Trace: " + ex.StackTrace + ") [" + System.Environment.UserName + "]";
                    LogIt(strError, true);
                    oFunction.SendEmail("Restart", "stephen.healy@pnc.com", "", "", name + " could not restart!", "This message is to inform you that the " + name + " service encountered an error while restarting" + System.Environment.NewLine + System.Environment.NewLine + strError, false, true);
                }
                catch { }
            }
        }
        private void LogIt(string message, bool error)
        {
            if (logging >= 0)
                oEventLog.WriteEntry(message, (error ? EventLogEntryType.Error : EventLogEntryType.Information));
            oLog.AddEvent("RESTART", name, message, (error ? LoggingType.Error : LoggingType.Information));
        }
    }
}
