//===============================================================================
// Microsoft Patterns & Practices Enterprise Library
// Excerpt from the Exception Handling Logging Block
//===============================================================================

using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.ApplicationBlocks.Logging;

namespace Microsoft.ApplicationBlocks.ExceptionHandling
{
    /// <summary>
    /// Represents an <see cref="IExceptionHandler"/> that formats
    /// the exception into a log message and sends it to
    /// the Enterprise Library Logging Block.
    /// </summary>	
    public class LoggingExceptionHandler : IExceptionHandler
    {
        #region Member Variables
        private LogWriter logWriter;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingExceptionHandler"/> class
        /// </summary>
        public LoggingExceptionHandler()
        {
            this.LogWriter = new LogWriter();
        }
        #endregion

        #region Properties
        /// <summary>
        /// The Log Writer used to log the exception
        /// </summary>
        public LogWriter LogWriter
        {
            get { return logWriter; }
            set { logWriter = value; }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// <para>Handles the specified object by formatting it and writing to the configured log.</para>
        /// </summary>
        /// <param name="exception">The exception to handle.</param>        
        /// <param name="handlingInstanceId">The unique ID attached to the handling chain for this handling instance.</param>
        public void HandleException(Exception exception, Guid handlingInstanceId)
        {
            string message = CreateMessage(exception, handlingInstanceId);
            
            this.LogWriter.Write(message, EventLogEntryType.Error);
        }
        #endregion

        #region Private Methods
        private string CreateMessage(Exception exception, Guid handlingInstanceID)
        {
            string formattedException;

            try
            {
                ExceptionFormatter formatter = new ExceptionFormatter(exception, handlingInstanceID);
                formatter.Format();
                formattedException = formatter.FormattedException;
            }
            catch
            {
                formattedException = exception.ToString();
            }

            return formattedException;
        }
        #endregion
    }
}