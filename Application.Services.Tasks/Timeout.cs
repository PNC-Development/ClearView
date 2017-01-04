using NCC.ClearView.Application.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace Application.Services.Tasks
{
    public enum TimeoutType
    {
        Hours = 3600,
        Minutes = 60,
        Seconds = 1
    }
    public class Timeout
    {
        private int seconds { get; set; }
        private EventLog oEventLog { get; set; }
        public bool StopIt { get; set; }
        private DateTime started { get; set; }
        private bool Debug { get; set; }
        // Constructor - load members
        public Timeout(TimeoutType type, int value, EventLog _log, bool _debug)
        {
            seconds = (value * (int)type);
            oEventLog = _log;
            StopIt = false;
            started = DateTime.Now;
            Debug = _debug;
        }

        public void Begin()
        {
            if (Debug)
                oEventLog.WriteEntry(String.Format("Starting TimeOut Thread."), EventLogEntryType.Information);
            while (!StopIt)
            {
                TimeSpan span = DateTime.Now.Subtract(started);
                if (span.TotalSeconds > seconds)
                {
                    // Timed out.
                    StopIt = true;
                    oEventLog.WriteEntry("Timed Out (" + seconds.ToString() + "). Stopping...", EventLogEntryType.Information);
                }
                Thread.Sleep(1000); // Wait one second.
            }
            if (Debug)
                oEventLog.WriteEntry("TimeOut thread exited.", EventLogEntryType.Information);
        }
    }
}
