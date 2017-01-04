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
    public partial class powershell : System.Web.UI.Page
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);

        protected void Page_Load(object sender, EventArgs e)
        {
        }
        public string PowerShell(string _location_of_psexec, string _file, string _parameters, int _timeout_in_minutes)
        {
            Response.Write(_file + "<br/>");

            // Convert seconds to milliseconds
            _timeout_in_minutes = (_timeout_in_minutes * 60 * 1000);
            ProcessStartInfo infoAudit = new ProcessStartInfo("powershell.exe");
            infoAudit.WorkingDirectory = _location_of_psexec;
            // Windows 2008: Have to remove the "-i" parameter or else the script won't run (will come up with "the following helper dll cannot be loaded...XXXX.DLL" error)
            string strAuditArguments = "-file " + _file + " " + _parameters;
            infoAudit.Arguments = strAuditArguments;
            infoAudit.UseShellExecute = false;
            infoAudit.RedirectStandardOutput = true;
            infoAudit.RedirectStandardError = true;

            Process procAudit = Process.Start(infoAudit);
            procAudit.WaitForExit(_timeout_in_minutes);
            if (procAudit.HasExited == false)
            {
                Response.Write("Timeout" + "<br/>");
                procAudit.Kill();
            }
            string output = procAudit.ExitCode.ToString();
            Response.Write("Exit Code : " + output + "<br/>");

            //Servers oServer = new Servers(user, dsn);
            //oServer.AddOutput(_serverid, _read_output_type, output);

            return output;
        }

        protected void btnPSEXEC_Click(object sender, EventArgs e)
        {
            Variables oVariable = new Variables(999);
            string strAdminUser = oVariable.Domain() + "\\" + oVariable.ADUser();
            string strAdminPass = oVariable.ADPassword();
            PowerShell(Request.PhysicalApplicationPath + "scripts\\", "test.ps1", "-path C:", 1);
        }

        protected void btnDLL_Click(object sender, EventArgs e)
        {
            Powershell oPowershell = new Powershell();
            Log oLog = new Log(0, dsn);
            List<PowershellParameter> powershell = new List<PowershellParameter>();
            powershell.Add(new PowershellParameter("path", "C:"));
            List<PowershellParameter> results = oPowershell.Execute(Request.PhysicalApplicationPath + "scripts\\test.ps1", powershell, oLog, "DEV");
            foreach (PowershellParameter result in results)
            {
                Response.Write(result.Name + " = " + result.Value + "<br/>");
            }
        }

    }
}