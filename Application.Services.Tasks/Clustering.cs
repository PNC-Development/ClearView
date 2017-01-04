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
    public class Clustering
    {
        private string dsn { get; set; }
        private int EnvironmentID { get; set; }
        private string strResults { get; set; }
        private EventLog oEventLog { get; set; }
        private bool Debug { get; set; }
        protected Starter Starter { get; set; }

        // Constructor - load members
        public Clustering(string _dsn, int _environment, EventLog _log, bool _debug, Starter _starter)
        {
            dsn = _dsn;
            EnvironmentID = _environment;
            oEventLog = _log;
            Debug = _debug;
            Starter = _starter;
        }

        public void New()
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
                this.Starter.Clusters = true;

                // Setup Classes
                Servers oServer = new Servers(0, dsn);
                NCC.ClearView.Application.Core.Storage oStorage = new NCC.ClearView.Application.Core.Storage(0, dsn);
                Forecast oForecast = new Forecast(0, dsn);
                Cluster oCluster = new Cluster(0, dsn);
                Log oLog = new Log(0, dsn);

                DataSet dsNew = oCluster.GetClustering();
                if (dsNew.Tables[0].Rows.Count > 0)
                {

                    oLog.AddEvent("", "", "Get clustering (" + dsNew.Tables[0].Rows.Count.ToString() + ")", LoggingType.Debug);
                    foreach (DataRow drNew in dsNew.Tables[0].Rows)
                    {
                        string error = "";
                        int intAnswer = Int32.Parse(drNew["answerid"].ToString());
                        //if (oStorage.GetLunDisks(intAnswer).Tables[0].Rows.Count > 0)
                        //{
                        //    oLog.AddEvent(intAnswer, "CLUSTERING", "CLUSTERING", "Disks are there. Starting clustering...", LoggingType.Debug);
                        //    DataSet dsClusters = oServer.GetAnswerClusters(intAnswer);
                        //    foreach (DataRow drCluster in dsClusters.Tables[0].Rows)
                        //    {
                        //        int intClusterID = Int32.Parse(drCluster["clusterid"].ToString());
                        //        string strCluster = oCluster.Get(intClusterID, "name");
                        //        oLog.AddEvent(intAnswer, strCluster, "CLUSTERING", "Checking cluster in DNS...", LoggingType.Debug);
                        //        // First, check to make sure it's available (pinging) in DNS
                        //        bool InDNS = false;
                        //        Ping Ping = new Ping();
                        //        string PingStatus = "";
                        //        try
                        //        {
                        //            PingReply Reply = Ping.Send(strCluster);
                        //            PingStatus = Reply.Status.ToString().ToUpper();
                        //            if (PingStatus == "SUCCESS")
                        //            {
                        //                InDNS = true;
                        //                break;
                        //            }
                        //        }
                        //        catch { }

                        //        if (InDNS == false)
                        //        {
                        //            error = "The cluster name " + strCluster + " is not responding in DNS";
                        //            oLog.AddEvent(intAnswer, strCluster, "CLUSTERING", error, LoggingType.Error);
                        //            break;
                        //        }
                        //        else
                        //        {
                        //            oLog.AddEvent(intAnswer, strCluster, "CLUSTERING", "Cluster is responding in DNS. Now check instances...", LoggingType.Debug);
                        //            DataSet dsInstances = oCluster.GetInstances(intClusterID);
                        //            foreach (DataRow drInstance in dsInstances.Tables[0].Rows)
                        //            {
                        //                string strInstance = drInstance["name"].ToString();
                        //                oLog.AddEvent(intAnswer, strCluster, strInstance, "Checking instance " + strInstance + " in DNS...", LoggingType.Debug);
                        //                // First, check to make sure it's available (pinging) in DNS
                        //                InDNS = false;
                        //                Ping = new Ping();
                        //                PingStatus = "";
                        //                try
                        //                {
                        //                    PingReply Reply = Ping.Send(strInstance);
                        //                    PingStatus = Reply.Status.ToString().ToUpper();
                        //                    if (PingStatus == "SUCCESS")
                        //                    {
                        //                        InDNS = true;
                        //                        break;
                        //                    }
                        //                }
                        //                catch { }

                        //                if (InDNS == false)
                        //                {
                        //                    error = "The cluster instance name " + strInstance + " is not responding in DNS";
                        //                    oLog.AddEvent(intAnswer, strCluster, strInstance, error, LoggingType.Error);
                        //                    break;
                        //                }
                        //                else
                        //                    oLog.AddEvent(intAnswer, strCluster, strInstance, "The cluster instance name " + strInstance + " is responding in DNS.", LoggingType.Debug);
                        //            }
                        //        }
                        //    }

                        //    if (String.IsNullOrEmpty(error))
                        //    {
                        //        // Clusterprocessing.ps1 –AnswerID [answerid] –Environment [environment] –Log [true]
                        //        string command = "Clusterprocessing.ps1 -AnswerID " + intAnswer.ToString() + " -Environment \"" + this.Starter.ScriptEnvironment + "\" –Log";
                        //        oLog.AddEvent(intAnswer, "CLUSTERING", "CLUSTERING", "Executing automated clustering script (" + command + ")...", LoggingType.Debug);

                        //        try
                        //        {
                        //            List<PowershellParameter> powershell = new List<PowershellParameter>();
                        //            Powershell oPowershell = new Powershell();
                        //            powershell.Add(new PowershellParameter("AnswerID", intAnswer.ToString()));
                        //            powershell.Add(new PowershellParameter("Environment", this.Starter.ScriptEnvironment));
                        //            powershell.Add(new PowershellParameter("Log", null));
                        //            List<PowershellParameter> results = oPowershell.Execute(this.Starter.strScripts + "\\Clusterprocessing.ps1", powershell, oLog, intAnswer.ToString());
                        //            oLog.AddEvent(intAnswer, "CLUSTERING", "CLUSTERING", "Powershell script completed!", LoggingType.Debug);
                        //            bool PowerShellError = false;
                        //            foreach (PowershellParameter result in results)
                        //            {
                        //                oLog.AddEvent(intAnswer, "CLUSTERING", "CLUSTERING", "PSOBJECT: " + result.Name + " = " + result.Value, LoggingType.Information);
                        //                if (result.Name == "ResultCode" && result.Value.ToString() != "0")
                        //                    PowerShellError = true;
                        //                else if (result.Name == "Message" && PowerShellError)
                        //                    error = result.Value.ToString();
                        //            }
                        //        }
                        //        catch (Exception exPowershell)
                        //        {
                        //            error = exPowershell.Message;
                        //        }
                        //    }
                        //}
                        //else
                        //    error = "Windows - no disk error message c0000013";

                        if (String.IsNullOrEmpty(error))
                        {
                            oLog.AddEvent(intAnswer, "CLUSTERING", "CLUSTERING", "Done.", LoggingType.Debug);
                            oCluster.UpdateClusteringCompleted(intAnswer, "", DateTime.Now.ToString(), 0);
                        }
                        else
                        {
                            oLog.AddEvent(intAnswer, "CLUSTERING", "CLUSTERING", error, LoggingType.Error);
                            oCluster.UpdateClusteringCompleted(intAnswer, error, DateTime.Now.ToString(), 1);
                            oForecast.AddError(0, 0, 0, intAnswer, (int)NCC.ClearView.Application.Core.Forecast.ForecastAnswerErrorStep.Clustering, error);
                        }

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
                this.Starter.Clusters = false;
                //timeout.StopIt = true;  // Kill timeout thread.
            }
        }
    }
}
