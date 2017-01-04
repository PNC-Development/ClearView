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
using System.Net.Mail;

namespace Microsoft.ApplicationBlocks.ExceptionHandling
{
    /// <summary>
    /// Represents an <see cref="IExceptionHandler"/> that formats
    /// the exception into a log message and sends it to
    /// a specified email address.
    /// </summary>	
    public class MessageExceptionHandler : IExceptionHandler
    {
        #region Member Variables
        private string from;
        private string to;
        private string host;
        private string message;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingExceptionHandler"/> class
        /// </summary>
        /// <param name="fromAddress">The from address for this e-mail error message</param>
        /// <param name="toAddress">The address collection that contains the recipients of this e-mail error message</param>
        /// <param name="smtpHost">The name or IP address of the host used for SMTP transactions</param>
        public MessageExceptionHandler(string fromAddress, string toAddress, string smtpHost)
        {
            this.From = fromAddress;
            this.To = toAddress;
            this.Host = smtpHost;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the from address for this e-mail error message
        /// </summary>
        public string From
        {
            get { return from; }
            set { from = value; }
        }

        /// <summary>
        /// Gets or sets the from address for this e-mail error message
        /// </summary>
        public string Message
        {
            get { return message; }
            set { message = value; }
        }

        /// <summary>
        /// Gets or sets the address collection that contains the recipients of this e-mail error message
        /// </summary>
        public string To
        {
            get { return to; }
            set { to = value; }
        }

        /// <summary>
        /// Gets or sets the name or IP address of the host used for SMTP transactions
        /// </summary>
        public string Host
        {
            get { return host; }
            set { host = value; }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// <para>Handles the specified object by formatting it and writing to the configured email address.</para>
        /// </summary>
        /// <param name="exception">The exception to handle.</param>        
        /// <param name="handlingInstanceId">The unique ID attached to the handling chain for this handling instance.</param>
        public void HandleException(Exception exception, Guid handlingInstanceId)
        {
            this.Message = CreateMessage(exception, handlingInstanceId);
        }
        public void Email()
        {
            try
            {
                MailMessage mailMessage = new MailMessage();
                MailAddress oAddress = new MailAddress(this.From);
                mailMessage.From = oAddress;
                char[] strSplit = { ';' };
                string[] strEmail = this.To.Split(strSplit);
                for (int ii = 0; ii < strEmail.Length; ii++)
                {
                    if (strEmail[ii].Trim() != "")
                    {
                        string strAddress = strEmail[ii];
                        if (strAddress != "")
                        {
                            oAddress = new MailAddress(strAddress);
                            mailMessage.To.Add(oAddress);
                        }
                    }
                }
                mailMessage.Subject = "ClearView encountered an error";
                mailMessage.Body = this.Message;
                SmtpClient mailClient = new SmtpClient(this.Host);
                mailClient.Send(mailMessage);
            }
            catch { }
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