using NCC.ClearView.Application.Core;
using NCC.ClearView.Application.Core.ClearViewWS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Xml;

namespace Application.Services.Tasks
{
    public class Storage
    {
        private string dsn { get; set; }
        private int EnvironmentID { get; set; }
        private string strResults { get; set; }
        private EventLog oEventLog { get; set; }
        private bool Debug { get; set; }
        protected Starter Starter { get; set; }

        // Constructor - load members
        public Storage(string _dsn, int _environment, EventLog _log, bool _debug, Starter _starter)
        {
            dsn = _dsn;
            EnvironmentID = _environment;
            oEventLog = _log;
            Debug = _debug;
            Starter = _starter;
        }

        public void NonShared()
        {
            //// Initiate Timer
            //int intTimeout = 10;    // minutes for all registrations
            //if (Debug)
            //    oEventLog.WriteEntry(String.Format("Starting Avamar Registration Thread."), EventLogEntryType.Information);
            //Timeout timeout = new Timeout(TimeoutType.Minutes, intTimeout, oEventLog, Debug);
            //ThreadStart tTimeoutStart = new ThreadStart(timeout.Begin);
            //Thread tTimeout = new Thread(tTimeoutStart);
            //tTimeout.Start();

            try
            {
                this.Starter.NonSharedStorage = true;

                // Setup Classes
                Servers oServer = new Servers(0, dsn);
                OperatingSystems oOperatingSystem = new OperatingSystems(0, dsn);
                Log oLog = new Log(0, dsn);

                DataSet dsNew = oServer.GetStorageConfigured();
                if (dsNew.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow drNew in dsNew.Tables[0].Rows)
                    {
                        int intServer = Int32.Parse(drNew["id"].ToString());
                        int intOS = Int32.Parse(drNew["osid"].ToString());
                        int intAnswer = Int32.Parse(drNew["answerid"].ToString());
                        int intNumber = Int32.Parse(drNew["number"].ToString());
                        string Name = drNew["servername"].ToString();
                        string IP = drNew["ipaddress"].ToString();

                        if (oOperatingSystem.IsWindows(intOS) || oOperatingSystem.IsWindows2008(intOS))
                        {
                            // First, check to make sure it's available (pinging) in DNS
                            //bool InDNS = false;
                            //Ping Ping = new Ping();
                            //string PingStatus = "";
                            //try
                            //{
                            //    PingReply Reply = Ping.Send(Name);
                            //    PingStatus = Reply.Status.ToString().ToUpper();
                            //    if (PingStatus == "SUCCESS")
                            //    {
                            //        InDNS = true;
                            //        break;
                            //    }
                            //}
                            //catch { }

                            //if (InDNS)
                            //{
                            // Serverprocessing.ps1 -AnswerID 26622 -ServerNumber 1 –Environment "Albert_Dev" –IPAddressToConnect  "10.24.240.205" – ConfigureNonSharedStorage -Log
                            string command = "Serverprocessing.ps1 -AnswerID " + intAnswer.ToString() + " -ServerNumber " + intNumber.ToString() + " –Environment \"" + this.Starter.ScriptEnvironment + "\" –IPAddressToConnect  \"" + IP + "\" – ConfigureNonSharedStorage -Log";
                            oLog.AddEvent(intAnswer, Name, "Non-shared storage", "Starting automated script (" + command + ")...", LoggingType.Debug);

                            string error = "";
                            try
                            {
                                List<PowershellParameter> powershell = new List<PowershellParameter>();
                                Powershell oPowershell = new Powershell();
                                powershell.Add(new PowershellParameter("AnswerID", intAnswer.ToString()));
                                powershell.Add(new PowershellParameter("ServerNumber", intNumber.ToString()));
                                powershell.Add(new PowershellParameter("Environment", this.Starter.ScriptEnvironment));
                                powershell.Add(new PowershellParameter("IPAddressToConnect", IP));
                                powershell.Add(new PowershellParameter("ConfigureNonSharedStorage", null));
                                powershell.Add(new PowershellParameter("Log", null));
                                List<PowershellParameter> results = oPowershell.Execute(this.Starter.strScripts + "\\Serverprocessing.ps1", powershell, oLog, Name);
                                oLog.AddEvent(intAnswer, Name, "Non-shared storage", "Powershell script completed!", LoggingType.Debug);
                                bool PowerShellError = false;
                                foreach (PowershellParameter result in results)
                                {
                                    oLog.AddEvent(intAnswer, Name, "Non-shared storage", "PSOBJECT: " + result.Name + " = " + result.Value, LoggingType.Information);
                                    if (result.Name == "ResultCode" && result.Value.ToString() != "0")
                                        PowerShellError = true;
                                    else if (result.Name == "Message" && PowerShellError)
                                        error = result.Value.ToString();
                                }
                            }
                            catch (Exception exPowershell)
                            {
                                error = exPowershell.Message;
                            }

                            if (String.IsNullOrEmpty(error))
                            {
                                oServer.UpdateStorageConfigured(intServer, DateTime.Now.ToString());
                            }
                            else
                            {
                                oLog.AddEvent(intAnswer, Name, "", error, LoggingType.Error);
                                oServer.AddError(0, 0, 0, intServer, 99991, error);
                            }
                            //}
                            //else
                            //    oLog.AddEvent(intAnswer, Name, "Non-shared storage", "DNS is not registered yet...", LoggingType.Debug);
                        }
                        else
                            oServer.UpdateStorageConfigured(intServer, DateTime.Now.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message + " ~ (Source: " + ex.Source + ") (Stack Trace: " + ex.StackTrace + ")";
                oEventLog.WriteEntry(error, EventLogEntryType.Error);
            }
            finally
            {
                this.Starter.NonSharedStorage = false;
                //timeout.StopIt = true;  // Kill timeout thread.
            }
        }
    }
}
