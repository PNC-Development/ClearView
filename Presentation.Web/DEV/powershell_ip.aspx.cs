using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NCC.ClearView.Application.Core;

namespace NCC.ClearView.Presentation.Web.DEV
{
    public partial class powershell_ip : System.Web.UI.Page
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            List<string> dns = new List<string>();
            dns.Add("1.2.3.4");
            dns.Add("5.6.7.8");
            string strScripts = Server.MapPath("/scripts/");
            Log oLog = new Log(0, dsn);
            string strName = "WSCLV221AZ";
            int intAnswer = 26397;
            string strSerial = "";
            string strError = "";

            // Executing IP address / DNS script (Serverprocessing.ps1 –AnswerID 26397 –ServerNumber 1 –Environment "Test" –IPAddressToConnect "10.24.49.214"
            // –ConfigureIPAddress true –ShutdownAfterNetworkConfiguration  true –ConfigureDNS true –ConfigureDNSServers true –DNSServerAddressList System.String[] –ConfigureDNSSuffix –ConnectionSpecificSuffix "pncbank.com" –ConfigureDNSRegistration true –SetRegisterThisConnectionAddrress false –SetUseSuffixWhenRegistering false –MACAddressToConfigure "00:50:56:93:16:5b" -Log)...
            try
            {
                List<PowershellParameter> powershell = new List<PowershellParameter>();
                Powershell oPowershell = new Powershell();
                powershell.Add(new PowershellParameter("AnswerID", intAnswer));
                powershell.Add(new PowershellParameter("ServerNumber", 1));
                powershell.Add(new PowershellParameter("Environment", "Test"));
                powershell.Add(new PowershellParameter("IPAddressToConnect", "10.24.49.214"));
                powershell.Add(new PowershellParameter("ConfigureIPAddress", true));
                powershell.Add(new PowershellParameter("ShutDownAfterNetworkConfiguration", true));
                powershell.Add(new PowershellParameter("ConfigureDNS", true));
                powershell.Add(new PowershellParameter("ConfigureDNSServers", true));
                powershell.Add(new PowershellParameter("DNSServerAddressList", dns.ToArray()));
                powershell.Add(new PowershellParameter("ConfigureDNSSuffix", true));
                powershell.Add(new PowershellParameter("ConnectionSpecificSuffix", "pncbank.com"));
                powershell.Add(new PowershellParameter("ConfigureDNSRegistration", true));
                powershell.Add(new PowershellParameter("SetRegisterThisConnectionAddrress", false));
                powershell.Add(new PowershellParameter("SetUseSuffixWhenRegistering", false));
                powershell.Add(new PowershellParameter("MACAddressToConfigure", "00:50:56:93:16:5b"));
                powershell.Add(new PowershellParameter("Log", null));
                List<PowershellParameter> results = oPowershell.Execute(strScripts + "\\Serverprocessing.ps1", powershell, oLog, strName.ToString());
                oLog.AddEvent(intAnswer, strName, strSerial, "Powershell IP address / DNS script completed!", LoggingType.Debug);
                bool PowerShellError = false;
                foreach (PowershellParameter result in results)
                {
                    oLog.AddEvent(intAnswer, strName, strSerial, "PSOBJECT: " + result.Name + " = " + result.Value, LoggingType.Information);
                    if (result.Name == "ResultCode" && result.Value.ToString() != "0")
                        PowerShellError = true;
                    else if (result.Name == "Message" && PowerShellError)
                        strError = result.Value.ToString();
                }
            }
            catch (Exception exPowershell)
            {
                strError = exPowershell.Message;
            }

            Response.Write("Error = " + strError);
        }

    }
}