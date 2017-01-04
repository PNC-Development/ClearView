using NCC.ClearView.Application.Core;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Xml;

namespace Presentation.Web.Services
{
    public class Powershell : BaseClass
    {

        public Powershell(string strWebMethodName)
            : base(strWebMethodName)
        {
        }
        /// <summary>
        /// Connects and executes the provided command on the grid. Will automatically add the "--xml" paramater and return XML output.
        /// </summary>
        /// <param name="grid">The fully qualified name of the grid</param>
        /// <param name="command">The command to be executed (without the --xml parameter)</param>
        /// <returns></returns>
        public string ServerProcessing(int intAnswer, int intNumber, string ScriptEnvironment, string strIP, string[] strServersDNS, string strMACAddress, string strName, string strSerial)
        {
            string strMethodName = (new StackTrace(true)).GetFrame(0).GetMethod().Name;
            if (boolCanRead)
            {
                string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
                Log oLog = new Log(0, dsn);

                string error = "";
                try
                {
                    List<PowershellParameter> powershell = new List<PowershellParameter>();
                    NCC.ClearView.Application.Core.Powershell oPowershell = new NCC.ClearView.Application.Core.Powershell();
                    powershell.Add(new PowershellParameter("AnswerID", intAnswer));
                    powershell.Add(new PowershellParameter("ServerNumber", intNumber));
                    powershell.Add(new PowershellParameter("Environment", ScriptEnvironment));
                    powershell.Add(new PowershellParameter("IPAddressToConnect", strIP));
                    powershell.Add(new PowershellParameter("ConfigureIPAddress", true));
                    powershell.Add(new PowershellParameter("ShutDownAfterNetworkConfiguration", true));
                    powershell.Add(new PowershellParameter("ConfigureDNS", true));
                    powershell.Add(new PowershellParameter("ConfigureDNSServers", true));
                    powershell.Add(new PowershellParameter("DNSServerAddressList", strServersDNS));
                    powershell.Add(new PowershellParameter("ConfigureDNSSuffix", true));
                    powershell.Add(new PowershellParameter("ConnectionSpecificSuffix", "pncbank.com"));
                    powershell.Add(new PowershellParameter("ConfigureDNSRegistration", true));
                    powershell.Add(new PowershellParameter("SetRegisterThisConnectionAddress", false));
                    powershell.Add(new PowershellParameter("SetUseSuffixWhenRegistering", false));
                    powershell.Add(new PowershellParameter("MACAddressToConfigure", strMACAddress));
                    powershell.Add(new PowershellParameter("Log", true));
                    List<PowershellParameter> results = oPowershell.Execute(HttpContext.Current.Server.MapPath("~/Serverprocessing.ps1"), powershell, oLog, strName.ToString());
                    oLog.AddEvent(intAnswer, strName, strSerial, "Powershell IP address / DNS script completed!", LoggingType.Debug);
                    bool PowerShellError = false;
                    foreach (PowershellParameter result in results)
                    {
                        oLog.AddEvent(intAnswer, strName, strSerial, "PSOBJECT: " + result.Name + " = " + result.Value, LoggingType.Information);
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
                return error;
            }
            else
            {
                oLog.WriteEntry(String.Format("\"" + WebMethodName + "\" Finished...Access Denied (" + strUser + ")"), EventLogEntryType.Information);
                return strERRORPrefix + "Access Denied (" + strUser + ")";
            }
        }
    }
}