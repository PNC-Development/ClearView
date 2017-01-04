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
    public partial class test_emc : BasePage
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
            string strUsername = "admin";
            string strPassword = "nccSAN03";
            //string strExpects = "sc>|[y/n]|} ok|return to ALOM";
            string strExpects = "#";
            SshShell oSSHshell = new SshShell("10.49.254.229", strUsername, strPassword);
            oSSHshell.RemoveTerminalEmulationCharacters = true;
            oSSHshell.Connect();
            CombinedStream oSSHstream = (CombinedStream)(oSSHshell.GetStream());
            if (oSSHshell.Connected == true && oSSHshell.ShellOpened == true)
            {
                //int bt = oSSHstream.ReadByte();
                string strOutput = oSSHshell.Expect(strExpects);
                Response.Write(strOutput);
            }
            oSSHshell.Close();
        }

    }
}
