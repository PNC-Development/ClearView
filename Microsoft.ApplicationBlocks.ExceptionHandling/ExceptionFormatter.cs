//===============================================================================
// Microsoft Patterns & Practices Enterprise Library
// Excerpt from the Exception Handling Logging Block
//===============================================================================

using System;
using System.Collections;
using System.Collections.Specialized;
using System.Globalization;
using System.Reflection;
using System.Security;
using System.Security.Principal;
using System.Threading;
using System.Text;

namespace Microsoft.ApplicationBlocks.ExceptionHandling
{
    /// <summary>
    /// The formatter provides functionality for formatting <see cref="Exception"/> objects.
    /// </summary>	
    public class ExceptionFormatter
    {
        #region Member Variables
        private Exception exception;
        private string formattedException;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionFormatter"/> class with an <see cref="Exception"/> to format.
        /// </summary>
        /// <param name="exception">The <see cref="Exception"/> object to format.</param>
        public ExceptionFormatter(Exception exception, Guid handlingInstanceID)
        {
            if (exception == null)
            {
                throw new ArgumentNullException("exception");
            }

            this.Exception = exception;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the <see cref="Exception"/> to format.
        /// </summary>
        /// <value>
        /// The <see cref="Exception"/> to format.
        /// </value>
        public Exception Exception
        {
            get { return this.exception; }
            set { this.exception = value; }
        }

        /// <summary>
        /// The formatted exception message
        /// </summary>
        public string FormattedException
        {
            get { return formattedException; }
            set { formattedException = value; }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Formats the <see cref="Exception"/> into the underlying stream.
        /// </summary>
        public virtual void Format()
        {
            WriteExceptionInformation(Exception);
            WriteBaseExceptionInformation();
            WriteInnerExceptionInformation(Exception.InnerException);    
        }
        #endregion

        #region Private Methods
        private void WriteExceptionInformation(Exception exception)
        {
            StringBuilder sb = new StringBuilder(FormattedException);

            sb.Append(exception.Message);
            sb.Append(System.Environment.NewLine);
            

            if (exception.HelpLink != null && exception.HelpLink != string.Empty)
            {
                sb.Append("For more information see: ");
                sb.Append(exception.HelpLink);
                sb.Append(System.Environment.NewLine);
            }

            sb.Append(System.Environment.NewLine);

            sb.Append("The name of the application or the object that caused the error: ");
            sb.Append(exception.Source);
            sb.Append(System.Environment.NewLine);

            sb.Append("Method that threw the current exception: ");
            sb.Append(exception.TargetSite);
            sb.Append(System.Environment.NewLine);

            if (exception.Data != null && exception.Data.Count > 0)
            {
                foreach (object key in exception.Data.Keys)
                {
                    sb.Append(key);
                    sb.Append(": ");
                    sb.Append(exception.Data[key]);
                    sb.Append(System.Environment.NewLine);
                }
            }

            sb.Append(System.Environment.NewLine);

            sb.Append("Stack trace: ");
            sb.Append(System.Environment.NewLine);
            sb.Append(exception.StackTrace);
            sb.Append(System.Environment.NewLine);

            sb.Append(System.Environment.NewLine);

            this.FormattedException = sb.ToString();
        }

        private void WriteBaseExceptionInformation()
        {
            StringBuilder sb = new StringBuilder(FormattedException);

            sb.Append("Machine Name: ");
            sb.Append(GetMachineName());
            sb.Append(System.Environment.NewLine);

            sb.Append("Executing Assembly: ");
            sb.Append(GetExecutingAssembly());
            sb.Append(System.Environment.NewLine);

            sb.Append("Application Domain Name: ");
            sb.Append(GetCurrentDomain());
            sb.Append(System.Environment.NewLine);

            sb.Append("Thread Identity: ");
            sb.Append(GetThreadIdentity());
            sb.Append(System.Environment.NewLine);

            sb.Append("WindowsIdentity: ");
            sb.Append(GetWindowsIdentity());
            sb.Append(System.Environment.NewLine);
            
            sb.Append(System.Environment.NewLine);

            this.FormattedException = sb.ToString();
        }

        private void WriteInnerExceptionInformation(Exception exception)
        {
            if (exception != null)
            {
                WriteExceptionInformation(exception);
                WriteInnerExceptionInformation(exception.InnerException);
            }
        }

        private string GetMachineName()
        {
            string machineName = String.Empty;

            try
            {
                machineName = Environment.MachineName;
            }
            catch (SecurityException)
            {
                machineName = "N/A - Permission Denied";
            }

            return machineName;
        }

        private string GetExecutingAssembly()
        {
            string executingAssembly = String.Empty;

            try
            {
                executingAssembly = Assembly.GetExecutingAssembly().FullName;
            }
            catch (Exception)
            {
                executingAssembly = "N/A - Exception encountered";
            }

            return executingAssembly;
        }

        private string GetCurrentDomain()
        {
            string domain = String.Empty;

            try
            {
                domain = AppDomain.CurrentDomain.FriendlyName;
            }
            catch (Exception)
            {
                domain = "N/A - Exception encountered";
            }

            return domain;
        }

        private string GetThreadIdentity()
        {
            string identity = String.Empty;

            try
            {
                identity = Thread.CurrentPrincipal.Identity.Name;
            }
            catch (Exception)
            {
                identity = "N/A - Exception encountered";
            }

            return identity;
        }

        private string GetWindowsIdentity()
        {
            string windowsIdentity = String.Empty;

            try
            {
                windowsIdentity = WindowsIdentity.GetCurrent().Name;
            }
            catch (SecurityException)
            {
                windowsIdentity = "N/A - PermissionDenied";
            }

            return windowsIdentity;
        }
        #endregion
    }
}