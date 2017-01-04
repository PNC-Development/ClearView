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
using Tamir.SharpSsh;
using Tamir.Streams;
using System.Text;

namespace NCC.ClearView.Presentation.Web.DEV
{
    public partial class ssh : System.Web.UI.Page
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);

        protected void Page_Load(object sender, EventArgs e)
        {
            Models oModel = new Models(0, dsn);
            Solaris oSolaris = new Solaris(0, dsn);
            Variables oVariables = new Variables((int)CurrentEnvironment.PNCNT_QA);
            string strMAC = "";
            string strUsername = oVariables.NexusUsername();
            string strPassword = oVariables.NexusPassword();
            string strExpects = "#";
            string strLine = "show int eth 102/1/24 br";

            //SshShell oSSHshell = new SshShell("P-PRDC-ZA08A-1", strUsername, strPassword);
            //oSSHshell.RemoveTerminalEmulationCharacters = true;
            //oSSHshell.Connect();
            //if (oSSHshell.Connected == true && oSSHshell.ShellOpened == true)
            //{
            //    // Wait for "sc>"
            //    string strBanner = oSSHshell.Expect(strExpects);
            //    // Send Command : showsc sys_enetaddr
            //    oSSHshell.WriteLine(strLine);
            //    //WriteLine(oSSHshell, strLine);
            //    // Wait for "sc>"
            //    strMAC = oSSHshell.Expect(strExpects);
            //    //strMAC = oSolaris.ParseOutput(strMAC, "macaddress = ", Environment.NewLine);
            //}
            //oSSHshell.Close();
            //Response.Write("<p>" + strMAC + "</p>");

            //SshExec oSSH = new SshExec("P-PRDC-ZA08A-1", strUsername, strPassword);
            //oSSH.Connect();
            //string strMAC2 = oSSH.RunCommand(strLine);
            //oSSH.Close();
            //Response.Write("<p>" + strMAC2 + "</p>");

            //SshExec oSSHa = new SshExec("10.49.254.229", "admin", "nccSAN03");
            //oSSHa.Connect();
            //string strResult = oSSHa.RunCommand("config t");
            //Response.Write(strResult);
            ////Response.Write(ExecuteSSH("config t", oSSHa));
            //oSSHa.Close();

            string strName = "HEALYTEST";
            StringBuilder strSAN = new StringBuilder();
            SshShell oSSHshellb = new SshShell("10.49.254.230", "admin", "nccSAN03");
            oSSHshellb.RemoveTerminalEmulationCharacters = true;
            oSSHshellb.Connect();
            if (oSSHshellb.Connected == true && oSSHshellb.ShellOpened == true)
            {
                string strBanner = oSSHshellb.Expect("#");
                strSAN.Append("10.49.254.230...");
                strSAN.Append(ExecuteSSH("config t", oSSHshellb));
                strSAN.Append(ExecuteSSH("device-alias database", oSSHshellb));
                strSAN.Append(ExecuteSSH("device-alias name " + strName + "b pwwn 50:06:0b:00:00:c3:5a:36", oSSHshellb));
                strSAN.Append(ExecuteSSH("exit", oSSHshellb));
                strSAN.Append(ExecuteSSH("device-alias commit", oSSHshellb));
                System.Threading.Thread.Sleep(20000);   // wait 20 seconds
                strSAN.Append(ExecuteSSH("zoneset name eng_cert vsan 1101", oSSHshellb));
                strSAN.Append(ExecuteSSH("zone name " + strName + "b_vmax0425_01h1", oSSHshellb));
                strSAN.Append(ExecuteSSH("member device-alias " + strName + "b", oSSHshellb));
                strSAN.Append(ExecuteSSH("member device-alias vmax0425_01h1", oSSHshellb));
                strSAN.Append(ExecuteSSH("exit", oSSHshellb));
                strSAN.Append(ExecuteSSH("member " + strName + "b_vmax0425_01h1", oSSHshellb));
                strSAN.Append(ExecuteSSH("exit", oSSHshellb));
                strSAN.Append(ExecuteSSH("zoneset activate name eng_cert vsan 1101", oSSHshellb));
                System.Threading.Thread.Sleep(10000);   // wait 10 seconds
                strSAN.Append(ExecuteSSH("zone commit vsan 1101", oSSHshellb));
                System.Threading.Thread.Sleep(20000);   // wait 20 seconds
                strSAN.Append(ExecuteSSH("end", oSSHshellb));
                strSAN.Append(ExecuteSSH("exit", oSSHshellb));
            }
            oSSHshellb.Close();
            Response.Write("<p>" + strSAN + "</p>");

        }
        protected string WriteLine(SshShell _shell, string _line)
        {
            CombinedStream oStream = (CombinedStream)(_shell.GetStream());
            string strReturn = "";
            for (int ii = 0; ii < _line.Length; ii++)
            {
                //oStream.Write(_line[ii].ToString());
                _shell.Write(_line[ii].ToString());
                System.Threading.Thread.Sleep(200); // sleep for a split second
                int intRead = oStream.ReadByte();
                strReturn += _shell.GetStream();
            }
            return strReturn;
        }
        private string ExecuteSSH(string _command, SshShell _shell)
        {
            _shell.WriteLine(_command);
            string strReturn = _shell.Expect("#");
            return strReturn + "<br/>";
        }
        private string ExecuteSSH(string _command, SshExec _ssh)
        {
            return _command + " -> " + _ssh.RunCommand(_command) + "<br/>";
        }
    }
}
