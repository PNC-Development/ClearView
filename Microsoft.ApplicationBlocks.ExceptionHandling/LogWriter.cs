//===============================================================================
// Microsoft Patterns & Practices Enterprise Library
// Excerpt from the Exception Handling Logging Block
//===============================================================================

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Security.Permissions;
using System.Threading;
using System.Text;

namespace Microsoft.ApplicationBlocks.Logging
{
    /// <summary>
    /// The Log Writer is used to to write messages to the event log and could be 
    /// expanded to include logging to files and databases.
    /// </summary>
    public class LogWriter
    {
        #region Member Variables
        private const string eventSourceName = "ClearView";
        private const string logName = "Application";
        #endregion

        #region Constructors
        /// <summary>
        /// Creates the default instance of the LogWrite class
        /// </summary>
        public LogWriter()
        {
            CreateEventSource();
        }
        #endregion

        #region Properties
        /// <summary>
        /// The name of the log the source's entries are written to
        /// </summary>
        public string LogName
        {
            get { return logName; }
        }

        /// <summary>
        /// The source name by which the application is registered on the local computer
        /// </summary>
        public string EventSourceName
        {
            get { return eventSourceName; }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Log message to event viewer
        /// </summary>
        /// <param name="messageToWrite">Message to write to event log</param>
        /// <param name="logEntryType">Specifies the event type</param>
        public void Write(string messageToWrite, EventLogEntryType logEntryType)
        {
            try
            {
                EventLog.WriteEntry(EventSourceName, messageToWrite, logEntryType);
            }
            catch (Exception ex)
            {
                // Security problem in DEV
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Creates an event source for the data fresh program (if it does not exist)
        /// </summary>
        private void CreateEventSource()
        {
            try
            {
                if (!EventLog.SourceExists(EventSourceName))
                {
                    EventLog.CreateEventSource(EventSourceName, LogName);
                }
            }
            catch (Exception ex)
            {
                // Security problem in DEV
            }
        }
        #endregion
    }
}
