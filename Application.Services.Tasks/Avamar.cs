using NCC.ClearView.Application.Core;
using NCC.ClearView.Application.Core.ClearViewWS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml;

namespace Application.Services.Tasks
{
    public class Avamar
    {
        private string dsn { get; set; }
        private int EnvironmentID { get; set; }
        private string strResults { get; set; }
        private EventLog oEventLog { get; set; }
        private bool Debug { get; set; }
        protected Starter Starter { get; set; }

        // Constructor - load members
        public Avamar(string _dsn, int _environment, EventLog _log, bool _debug, Starter _starter)
        {
            dsn = _dsn;
            EnvironmentID = _environment;
            oEventLog = _log;
            Debug = _debug;
            Starter = _starter;
        }

        public void Registrations()
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
                this.Starter.Registrations = true;
                AvamarRegistration oAvamarRegistration = new AvamarRegistration(0, dsn);
                oAvamarRegistration.Registrations(EnvironmentID, this.Starter.strScripts, this.Starter.dsnAsset, this.Starter.dsnServiceEditor, this.Starter.dsnIP, this.Starter.intViewPage, this.Starter.intAssignPage);
            }
            catch (Exception ex)
            {
                string error = ex.Message + " ~ (Source: " + ex.Source + ") (Stack Trace: " + ex.StackTrace + ")";
                oEventLog.WriteEntry(error, EventLogEntryType.Error);
            }
            finally
            {
                this.Starter.Registrations = false;
                //timeout.StopIt = true;  // Kill timeout thread.
            }
        }

        public void Activations()
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
                this.Starter.Activations = true;
                AvamarActivation oAvamarActivation = new AvamarActivation(0, dsn);
                oAvamarActivation.Activations(EnvironmentID);
            }
            catch (Exception ex)
            {
                string error = ex.Message + " ~ (Source: " + ex.Source + ") (Stack Trace: " + ex.StackTrace + ")";
                oEventLog.WriteEntry(error, EventLogEntryType.Error);
            }
            finally
            {
                this.Starter.Activations = false;
                //timeout.StopIt = true;  // Kill timeout thread.
            }
        }

        public void Backups()
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
                this.Starter.Backups = true;
                AvamarBackup oAvamarBackup = new AvamarBackup(0, dsn);
                oAvamarBackup.Backups(EnvironmentID);
            }
            catch (Exception ex)
            {
                string error = ex.Message + " ~ (Source: " + ex.Source + ") (Stack Trace: " + ex.StackTrace + ")";
                oEventLog.WriteEntry(error, EventLogEntryType.Error);
            }
            finally
            {
                this.Starter.Backups = false;
                //timeout.StopIt = true;  // Kill timeout thread.
            }
        }
    }
}
