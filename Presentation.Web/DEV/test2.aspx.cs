using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using NCC.ClearView.Application.Core;
using NCC.ClearView.Application.Core.ClearViewWS;
using System.Net;
using System.IO;
using Vim25Api;
using System.Reflection;
using System.ComponentModel;
using Microsoft.ApplicationBlocks.Data;
using System.Diagnostics;
using System.Data.OleDb;

namespace NCC.ClearView.Presentation.Web
{
    public partial class test2 : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        private string dsnZeus = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ZeusDSN"]].ConnectionString;
        private int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);


        protected void Page_Load(object sender, EventArgs e)
        {
            Variables oVariable = new Variables(999);
            Functions oFunction = new Functions(0, dsn, intEnvironment);
            string strServer = "WDCLV101W";

            string _location_of_psexec = @"C:\Temp\";
            int intTimeoutDefault = (1 * 20 * 1000);    // 20 seconds

            string strAdminUser = oVariable.Domain() + "\\" + oVariable.ADUser(); ;
            string strAdminPass = oVariable.ADPassword();

            ProcessStartInfo infoAudit = new ProcessStartInfo(_location_of_psexec + "psexec");
            infoAudit.WorkingDirectory = _location_of_psexec;
            // Windows 2008: Have to remove the "-i" parameter or else the script won't run (will come up with "the following helper dll cannot be loaded...XXXX.DLL" error)
            string strAuditArguments = "\\\\" + strServer + " -u " + strAdminUser + " -p " + strAdminPass + " -h cmd.exe /c hostname.exe";
            infoAudit.Arguments = strAuditArguments;
            infoAudit.UseShellExecute = false;
            infoAudit.RedirectStandardOutput = true;
            infoAudit.RedirectStandardError = true;

            Process procAudit = Process.Start(infoAudit);
            procAudit.WaitForExit(intTimeoutDefault);
            if (procAudit.HasExited == false)
                procAudit.Kill();
            Response.Write("PSEXEC Exited (" + procAudit.ExitCode.ToString() + ")" + "<br/>");

            string output = procAudit.StandardOutput.ReadToEnd();

            Response.Write("Result (" + output.Replace(Environment.NewLine, "") + ")");

            string error = procAudit.StandardError.ReadToEnd();

            procAudit.Close();
        }
    }
}
