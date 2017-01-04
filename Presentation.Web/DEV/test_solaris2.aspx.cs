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
using System.Net;
using System.IO;
using PAObjectsLib;
using Vim25Api;
using System.Reflection;
using NCC.ClearView.Application.Core.ClearViewWS;
using Tamir.SharpSsh;
using Tamir.Streams;
using System.Text;

namespace NCC.ClearView.Presentation.Web
{
    public partial class test_solaris2 : BasePage
    {
        protected int[] arProcessing = new int[4] { 45, 47, 92, 124 };   // 92 = \, 124 = |, 47 = /, 45 = -
        private string strSSH_Carriage = "\r";
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        private Log oLog;

        protected void Page_Load(object sender, EventArgs e)
        {
            oLog = new Log(0, dsn);
        }

        protected void btnCommand_Click(object Sender, EventArgs e)
        {
            string strSSH = "";
            string strILO = txtILOM.Text;
            string strUser = "admin";
            string strPass = "Abcd1234";

            string strWriteSSH = "";
            // Execute next command
            if (strWriteSSH == "")
                strWriteSSH = txtCommand1.Text;
            //else if (strWriteSSH == txtCommand1.Text)
            //    strWriteSSH = txtCommand2.Text;
            //else
            //    break;

            Functions oFunction = new Functions(0, dsn, 1);

            Asset oAsset = new Asset(0, dsnAsset);
            Response.Write(oAsset.GetVirtualConnectMACAddress(14487, 0, intEnvironment, 1, "C:\\", dsn, oFunction.GetSetupValuesByKey("RACADM_WEB").Tables[0].Rows[0]["value"].ToString(), oLog, ""));
            return;

            SshShell oSSHshell = new SshShell(strILO, strUser, strPass);
            oSSHshell.RemoveTerminalEmulationCharacters = true;
            oSSHshell.Connect();
            //CombinedStream oSSHstream = (CombinedStream)(oSSHshell.GetStream());

            // Write 3
            string strExpects = @"sc>|\[y/n\]|\{0\} ok|return to ALOM";
            if (oSSHshell.Connected == true && oSSHshell.ShellOpened == true)
            {
                // Wait for "sc>"
                string strBanner = oSSHshell.Expect(strExpects);
                // Send Command : showsc sys_enetaddr
                oSSHshell.WriteLine("showsc sys_enetaddr");
                // Wait for "sc>"
                string strMAC = oSSHshell.Expect(strExpects);
                strMAC = ParseOutput(strMAC, "showsc sys_enetaddr", Environment.NewLine);
                Response.Write(strMAC + "<br/>");


                // Send Command : poweron
                oSSHshell.WriteLine("poweron");
                // Wait for "sc>"
                string strPower = oSSHshell.Expect(strExpects);
                // Send Command : console -f
                oSSHshell.WriteLine("console -f");
                string strConsole = "";
                // Wait for "[y/n]" OR "return to ALOM"
                string strPrompt = oSSHshell.Expect(strExpects);
                if (strPrompt.Contains("[y/n]") == true)
                {
                    // ***  The console is already in use, need to take control
                    // Send Command : y
                    oSSHshell.WriteLine("y");
//                    // Send Command : <RETURN>
//                    oSSHshell.WriteLine("");
                    // Wait for "return to ALOM"
                    strConsole = ReplaceGarbageChars(oSSHshell.Expect(strExpects));
                }
                else
                {
                    // ***  The console was not in use, "{0} ok" was hit...store
                    strConsole = ReplaceGarbageChars(strPrompt);
                    strPrompt = "";
                }

                string strReturnToALOM = ParseOutput(strConsole, "Enter", "to return to ALOM");
                string strSetEnv = "";
                string strBuild = "";
                if (false)
                {
                    // Send Command : setenv local-mac-address? false
                    oSSHshell.WriteLine("setenv local-mac-address? false");
                    // Wait for "{0} ok"
                    strSetEnv = oSSHshell.Expect(strExpects);
                    // Send Command : boot net:dhcp - install
                    oSSHshell.WriteLine("boot net:dhcp - install");
                    // Wait for "{0} ok"
                    strBuild = oSSHshell.Expect(strExpects);
                }
                // Send Command : <strReturnToALOM>     // Either .. OR #.
                oSSHshell.WriteLine(strReturnToALOM);
                // Wait for "sc>"
                string strReturn = oSSHshell.Expect(strExpects);


                strSSH += strBanner + strPower + strPrompt + strConsole + strSetEnv + strBuild + strReturn;
            }

            Response.Write(strSSH);
            oSSHshell.Close();

        }

        private string ReplaceGarbageChars(string _replace)
        {
            StringBuilder strReturn = new StringBuilder();
            string strReplace = _replace;
            for (int ii = 0; ii < strReplace.Length; ii++)
            {
                char chrSSH = strReplace[ii];
                if ((int)chrSSH == 8)   // 8 = backspace
                {
                    // Hit a backspace, this means it is processing.  Check previous character.
                    char prevSSH = strReplace[ii -1];
                    if (IsGarbageChar((int)prevSSH) == true)
                        strReturn.Remove(strReturn.Length - 1, 1);
                }
                else
                    strReturn.Append(chrSSH.ToString());
            }
            return strReturn.ToString();
        }

        private bool IsGarbageChar(int intChar)
        {
            // This function will remove the garbage / processing characters from the output (such as -\|/-\|/)
            bool boolGarbage = false;
            for (int ii = 0; ii < arProcessing.Length; ii++)
            {
                if (intChar == arProcessing[ii])
                {
                    boolGarbage = true;
                    break;
                }
            }
            return boolGarbage;
        }

        private string ParseOutput(string _output, string _start, string _end)
        {
            if (_output.Contains(_start) == true)
            {
                string strBeginning = _output.Substring(_output.IndexOf(_start) + _start.Length).Trim();
                if (strBeginning.Contains(_end) == true)
                {
                    strBeginning = strBeginning.Substring(0, strBeginning.IndexOf(_end)).Trim();
                    return strBeginning.Trim();
                }
                else
                    return "";
            }
            else
                return "";
        }
    }
}
