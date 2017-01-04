using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Net.Mail;
using Microsoft.ApplicationBlocks.ExceptionHandling;
using NCC.ClearView.Application.Core;

namespace NCC.ClearView.Presentation.Web
{
    public class BasePage : System.Web.UI.Page
    {
        /// <summary>
        /// Logs exceptions encountered by the ClearView application.
        /// </summary>
        /// <param name="exception"></param>
        protected string LogException(Exception exception)
        {
            // Add information about the current web request
            AddExceptionInformation(exception);

            // Create GUID for this exception (for tracking purposes)
            System.Guid exceptionGUID = System.Guid.NewGuid();

            // Log error
            LoggingExceptionHandler loggingExceptionHandler = new LoggingExceptionHandler();
            loggingExceptionHandler.HandleException(exception, exceptionGUID);

            // Email error
            string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
            int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
            Variables variables = new Variables(intEnvironment);
            Functions oFunction = new Functions(0, dsn, intEnvironment);
            string strEMailIdsTO = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DEVELOPER_ERROR");
            MessageExceptionHandler messageExceptionHandler = new MessageExceptionHandler(variables.FromEmail(), strEMailIdsTO, variables.SmtpServer());
            messageExceptionHandler.HandleException(exception, exceptionGUID);
            if (intEnvironment == (int)CurrentEnvironment.CORPDMN || intEnvironment == (int)CurrentEnvironment.PNCNT_PROD)
                messageExceptionHandler.Email();
            return messageExceptionHandler.Message;
        }

        /// <summary>
        /// Authenticates the current user.  If the user is not authenticated they are redirected to the login page.
        /// </summary>
        protected virtual void AuthenticateUser()
        {
            if (Request.Cookies["profileid"] == null || Request.Cookies["profileid"].Value == string.Empty)
            {
                Response.Redirect("~/login.aspx");
            }
        }

        /// <summary>
        /// Handles the OnError event to provide global exception handling for all ClearView web pages
        /// </summary>
        /// <param name="e"></param>
        protected override void OnError(EventArgs e)  
        {  
            base.OnError(e);  
            Exception exception = Server.GetLastError();

            string strMessage = LogException(exception);
            if (strMessage.ToUpper().Contains("VIEWSTATE MAC FAILED") == true)
                Response.Redirect("~/");
            else
            {
                int intEnvironment = int.Parse(ConfigurationManager.AppSettings["Environment"]);
                bool boolShowError = (ConfigurationManager.AppSettings["ERROR_SHOW"] == "1");
                if (boolShowError == false && (intEnvironment == (int)CurrentEnvironment.CORPDMN || intEnvironment == (int)CurrentEnvironment.PNCNT_PROD))
                    RedirectUser(exception);
            }
        }

        /// <summary>
        /// Redirects to appropriate error page
        /// </summary>
        /// <param name="exception">Exception ecnountered</param>
        private void RedirectUser(Exception exception)
        {
            Response.Redirect("~/Error.aspx");
        }

        /// <summary>
        /// Adds information to the current application exception
        /// </summary>
        /// <param name="exception">Exception to add data to</param>
        private void AddExceptionInformation(Exception exception)
        {
            try
            {
                HttpContext context = HttpContext.Current;
                string requestUrl = context.Request.Url.ToString();

                exception.Data.Add("Date", DateTime.Now.ToString());
                exception.Data.Add("URL", requestUrl);
                exception.Data.Add("Is HTTPS", context.Request.IsSecureConnection);
                exception.Data.Add("URL Referrer", context.Request.UrlReferrer);
                exception.Data.Add("Server Name", context.Request.ServerVariables["SERVER_NAME"]);
                exception.Data.Add("QueryString", context.Request.QueryString);
                exception.Data.Add("Platform", context.Request.Browser.Platform);
                exception.Data.Add("Is Crawler", context.Request.Browser.Crawler);
                exception.Data.Add("User Agent", context.Request.UserAgent);
                exception.Data.Add("Supports Cookies", context.Request.Browser.Cookies);
                exception.Data.Add("User IP", context.Request.UserHostAddress);
                exception.Data.Add("User Name", context.User.Identity.Name);
                exception.Data.Add("User Host Name", context.Request.UserHostName);
                exception.Data.Add("User is Authenticated", context.User.Identity.IsAuthenticated);
                exception.Data.Add("User Authentication Type", context.User.Identity.AuthenticationType);

                if (context.Request.Cookies != null)
                {
                    foreach (string cookie in context.Request.Cookies.AllKeys)
                    {
                        if (!exception.Data.Contains(cookie))
                        {
                            exception.Data.Add(cookie, context.Request.Cookies[cookie]);
                        }
                    }
                }
            }
            catch
            {
                // Unable to log exception since we are already logging an exception
            }
        }  
    }
}
