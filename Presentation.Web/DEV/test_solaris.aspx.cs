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

namespace NCC.ClearView.Presentation.Web
{
    public partial class test_solaris : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        private string dsnServiceEditor = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ServiceEditorDSN"]].ConnectionString;
        private int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intCount = 0;
        protected int[] arProcessing = new int[5] { 8, 45, 47, 92, 124 };   // 8 = backspace, 92 = \, 124 = |, 47 = /, 45 = -
        private string strSSH_Carriage = "\r";

        protected void Page_Load(object sender, EventArgs e)
        {
            string strSSH = "";
            string strILO = "10.249.237.148";
            if (Request.QueryString["ilo"] != null)
                strILO = "10.249.237.144";

            Variables oVariable = new Variables(intEnvironment);
            int intLogging = 0;
            byte[] byt;
            string str;

            Models oModel = new Models(0, dsn);
            ModelsProperties oModelsProperties = new ModelsProperties(0, dsn);
            Servers oServer = new Servers(0, dsn);
            Settings oSetting = new Settings(0, dsn);
            OnDemand oOnDemand = new OnDemand(0, dsn);
            Log oEventLog = new Log(0, dsn);
            SshShell oSSHshell = new SshShell(strILO, oVariable.SolarisUsername(), oVariable.SolarisPassword());
            oSSHshell.RemoveTerminalEmulationCharacters = true;
            oSSHshell.Connect();
            Response.Write("Connected to " + strILO + "...sending commands..." + "<br/>");
            CombinedStream oSSHstream = (CombinedStream)(oSSHshell.GetStream());

            int intStep = 1;

            if (Request.QueryString["none"] == null)
            {
                byt = new byte[100];
                str = "" + strSSH_Carriage;
                byt = System.Text.ASCIIEncoding.ASCII.GetBytes(str);
                if (oSSHshell.Connected == true && oSSHshell.ShellOpened == true)
                {
                    oSSHstream.Write(byt);
                }
            }

            int bt = 0;
            int intMinutePrevious = 0;
            bool boolProcessing = false;
            while (bt != -1 && oSSHshell.Connected == true && oSSHshell.ShellOpened == true)
            {
                bt = oSSHstream.ReadByte();
                // Strip the processing cursor -\|/-\|/ from the output
                if (bt == 8)    // 8 = backspace
                {
                    // Check to see if previous characters were a processing character as well
                    char chrSSH = strSSH[strSSH.Length - 1];
                    int intSymbol = (int)chrSSH;
                    while (IsGarbageChar(intSymbol) == true)
                    {
                        if (intLogging > 1)
                            Response.Write("The symbol [" + chrSSH.ToString() + "] is a garbage character and must be removed" + "<br/>");
                        strSSH = strSSH.Substring(0, strSSH.Length - 1);
                        chrSSH = strSSH[strSSH.Length - 1];
                        intSymbol = (int)chrSSH;
                    }
                    // Set processing to true to exclude future characters
                    boolProcessing = true;
                }
                if (boolProcessing == true && IsGarbageChar(bt) == false)
                    boolProcessing = false;
                if (boolProcessing == false)
                    strSSH += (char)bt;

                string strReadSSH = "";
                string strWriteSSH = "";
                switch (intStep)
                {
                    case 1:
                        strReadSSH = "-sc>";
                        strWriteSSH = "poweron";
                        break;
                    case 2:
                        strReadSSH = "-sc>";
                        strWriteSSH = "showpower";
                        break;
                    case 3:
                        strReadSSH = "-sc>";
                        break;
                }


                if (strReadSSH != "" && strSSH.EndsWith(strReadSSH) == true)
                {
                    try
                    {
                        Response.Write("SSH output ends with [" + strReadSSH + "] : " + strSSH + "<br/>");
                    }
                    catch { }

                    if (intStep == 3)
                        break;
                    // Execute next command
                    byt = new byte[100];
                    str = strWriteSSH + strSSH_Carriage;
                    byt = System.Text.ASCIIEncoding.ASCII.GetBytes(str);
                    if (oSSHshell.Connected == true && oSSHshell.ShellOpened == true)
                    {
                        try
                        {
                            Response.Write("Sending command [" + strWriteSSH + "] : " + strSSH + "<br/>");
                        }
                        catch { }
                        oSSHstream.Write(byt);
                    }
                    intStep++;

                }
                else
                {
                }
            }
            Response.Write(strSSH);
            oSSHstream.Close();
            oSSHshell.Close();
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

    }
}
