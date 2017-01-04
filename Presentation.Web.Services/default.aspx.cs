using NCC.ClearView.Application.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace Presentation.Web.Services
{
    public partial class _default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnAvamarDomains_Click(object sender, EventArgs e)
        {
            string host = "yb19-avr01-01.test.pncbank.com";
            WebServiceAPI.ClearViewWebServices ws = new WebServiceAPI.ClearViewWebServices();
            ws.Url = "http://localhost:55030/ClearViewWebServices.asmx";
            string strError = "";

            //string result = ws.GetAvamarDomains(host);
            try
            {
                string result = ws.GetAvamarGrid("YB62-AVR02-01.pncbank.com");
                if (!string.IsNullOrEmpty(result) && result.TrimStart().StartsWith("<"))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(result);
                    XmlNodeList xnList = doc.SelectNodes("/CLIOutput/Data/Row");
                    foreach (XmlNode xn in xnList)
                    {
                        Response.Write(xn["Name"].InnerText + "<br/>");
                    }
                }
                else
                    Response.Write(result + "<br/>");
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                    ex = ex.InnerException;
                strError = ex.Message;
            }
        }

        protected bool boolCanRead = false;
        protected bool boolCanWrite = false;
        protected string strERRORPrefix = "***";
        protected string strERRORNotFound = "";
        protected string strUser = "NA";
        public EventLog oLog;
        protected string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected Variables oVariable;
        protected string WebMethodName { get; set; }
        protected void btnLog_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Write("1" + "<br/>");
                WebMethodName = "CreateDNSforPNC";
                Response.Write("1" + "<br/>");
                strERRORNotFound = strERRORPrefix + "NOTFOUND";
                Response.Write("1" + "<br/>");
                strUser = System.Environment.UserName;
                Response.Write("1" + "<br/>");
                WebServices oWebService = new WebServices(0, dsn);
                Response.Write(strUser + "<br/>");
                Users oUser = new Users(0, dsn);
                Response.Write(oUser.GetId(strUser).ToString() + "<br/>");
                boolCanRead = (oWebService.CanRead(WebMethodName, oUser.GetId(strUser)));
                Response.Write("1" + "<br/>");
                boolCanWrite = (oWebService.CanWrite(WebMethodName, oUser.GetId(strUser)));
                Response.Write("1" + "<br/>");
                if (HttpContext.Current.Request.IsLocal == false)
                {
                    if (EventLog.SourceExists("ClearView") == false)
                    {
                        EventLog.CreateEventSource("ClearView", "ClearView");
                        EventLog.WriteEntry("Application", String.Format("ClearView EventLog Created"), EventLogEntryType.Information);
                    }
                    oLog = new EventLog();
                    oLog.Source = "ClearView";
                    oLog.ModifyOverflowPolicy(OverflowAction.OverwriteAsNeeded, 0);
                    oLog.MaximumKilobytes = long.Parse("16384");
                }
                else
                {
                    // Just write to local application log for debugging
                    oLog = new EventLog();
                    oLog.Source = "Application";
                    // Also, allow reading and writing
                    boolCanRead = true;
                    boolCanWrite = true;
                }

                Response.Write("1" + "<br/>");
                oVariable = new Variables(intEnvironment);
                Response.Write("No Problem!");
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        protected void btnServiceNow_Click(object sender, EventArgs e)
        {
            string URL = "https://webtest-itsm.pncbank.com";
            WebServiceAPI.ClearViewWebServices ws = new WebServiceAPI.ClearViewWebServices();
            ws.Url = "http://localhost:64919/ClearViewWebServices.asmx";

            string sequence = "0009";
            string domain = "pncbank.com";
            string name = "HEALY" + sequence;
            string serial = "HEALYSN" + sequence;
            string os = "Windows 2008 R2 Standard";
            string model = "VMware, Inc. VMware Virtual Platform";
            string manufacturer = "VMware, Inc.";

            // optional
            DateTime installed = DateTime.Now;
            string u_data_center = "YB19";
            string u_location = "CLEOPS";

            string result = ws.CreateServiceNowServer(URL, domain, "127.127.0.1", manufacturer, "CLV", model, name, os, serial, ServiceNowClasses.Windows, ServiceNowEnvironments.Development, true, installed, u_location, u_data_center, "_clearview_user", "_clearview_user");
            if (String.IsNullOrEmpty(result) == false)
                Response.Write(result);
            else
                Response.Write("Success");
        }

        protected void btnServiceNow2_Click(object sender, EventArgs e)
        {
            string URL = "https://webtest-itsm.pncbank.com";
            WebServiceAPI.ClearViewWebServices ws = new WebServiceAPI.ClearViewWebServices();
            ws.Url = "http://localhost:64919/ClearViewWebServices.asmx";

            string result = ws.GetServiceNowServer(URL, "_clearview_user", "_clearview_user", "HEALY0001");
            if (String.IsNullOrEmpty(result) == false)
                Response.Write(result);
            else
                Response.Write("Success");
        }

        protected void btnServiceNowDecom_Click(object sender, EventArgs e)
        {
            string URL = "https://webtest-itsm.pncbank.com";
            WebServiceAPI.ClearViewWebServices ws = new WebServiceAPI.ClearViewWebServices();
            ws.Url = "http://localhost:64919/ClearViewWebServices.asmx";
            string result = ws.DecomServiceNowServer(URL, "_clearview_user", "_clearview_user", "HEALY0005");
            if (String.IsNullOrEmpty(result) == false)
                Response.Write(result);
            else
                Response.Write("Success");
        }

        protected void btnServiceNowRecom_Click(object sender, EventArgs e)
        {
            string URL = "https://webtest-itsm.pncbank.com";
            WebServiceAPI.ClearViewWebServices ws = new WebServiceAPI.ClearViewWebServices();
            ws.Url = "http://localhost:64919/ClearViewWebServices.asmx";
            string result = ws.RecomServiceNowServer(URL, "_clearview_user", "_clearview_user", "HEALY0005");
            if (String.IsNullOrEmpty(result) == false)
                Response.Write(result);
            else
                Response.Write("Success");
        }

        protected void btnServiceNowIncidentGet_Click(object sender, EventArgs e)
        {
            if (false)
            {
                // QA Service Now
                string URL = "https://webqa-itsm.pncbank.com";
                WebServiceAPI.ClearViewWebServices ws = new WebServiceAPI.ClearViewWebServices();
                ws.Url = "http://localhost:64919/ClearViewWebServices.asmx";
                int ErrorID = 178255;
                if (String.IsNullOrEmpty(Request.QueryString["id"]) == false)
                    Int32.TryParse(Request.QueryString["id"], out ErrorID);
                string result = ws.GetServiceNowIncident(URL, "_clearview_user", "_clearview_userQA", ErrorID, "WORKSTATION");
                if (String.IsNullOrEmpty(result) == false)
                    Response.Write(result);
                else
                    Response.Write("Success");
            }
            else
            {
                // Production Service Now
                oVariable = new Variables(999);
                WebServiceAPI.ClearViewWebServices oServiceNow = new WebServiceAPI.ClearViewWebServices();
                if (true)
                {
                    // Production Web Service
                    System.Net.NetworkCredential oCredentials = new System.Net.NetworkCredential(oVariable.ADUser(), oVariable.ADPassword(), oVariable.Domain());
                    oServiceNow.Timeout = Timeout.Infinite;
                    oServiceNow.Credentials = oCredentials;
                    oServiceNow.Url = oVariable.WebServiceURL();
                }
                else  // Local Web Service
                    oServiceNow.Url = "http://localhost:55030/ClearViewWebServices.asmx";

                string url = "https://itsm.pncbank.com";
                bool workstation = false;
                int errorid = 29002;
                //string data = oServiceNow.GetServiceNowIncident(url, oVariable.ServiceNowUsername(), oVariable.ServiceNowPassword(), errorid, (workstation ? "WORKSTATION" : "SERVER"));
                string data = oServiceNow.GetServiceNowIncidentNumber(url, oVariable.ServiceNowUsername(), oVariable.ServiceNowPassword(), "INC2381530");
                if (String.IsNullOrEmpty(data) == false)
                    Response.Write(data);
                else
                    Response.Write("Success");
            }
        }
    }
}