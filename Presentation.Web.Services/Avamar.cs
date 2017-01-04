using Renci.SshNet;
using Renci.SshNet.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Xml;

namespace Presentation.Web.Services
{
    public class Avamar : BaseClass
    {

        public Avamar(string strWebMethodName)
            : base(strWebMethodName)
        {
        }
        /// <summary>
        /// Connects and executes the provided command on the grid. Will automatically add the "--xml" paramater and return XML output.
        /// </summary>
        /// <param name="grid">The fully qualified name of the grid</param>
        /// <param name="command">The command to be executed (without the --xml parameter)</param>
        /// <returns></returns>
        public string RunCommand(string grid, string command)
        {
            return RunCommand(grid, command, true);
        }
        public string RunCommand(string grid, string command, bool appendXML)
        {
            string strMethodName = (new StackTrace(true)).GetFrame(0).GetMethod().Name;
            if (boolCanRead)
            {
                string privateKey = HttpContext.Current.Server.MapPath("~/cv_avamar.pvt");
                string user = oVariable.AvamarUsername();
                string xml = "";

                PrivateKeyFile key = new PrivateKeyFile(privateKey, oVariable.ADPassword());
                var connectionInfo = new PrivateKeyConnectionInfo(grid, user, key);
                using (var client = new SshClient(connectionInfo))
                {
                    try
                    {
                        client.Connect();
                        SshCommand output = client.RunCommand(command + (appendXML ? " --xml" : "") + " | tee -a .avamar_cv_command_$(date +%Y%m).log");
                        if (String.IsNullOrEmpty(output.Error) == false)
                            xml = output.Error;
                        else
                            xml = output.Result;
                        client.Disconnect();
                    }
                    catch (SshAuthenticationException ex)
                    {
                        xml = "Error Message: " + ex.Message + " on " + grid;
                    }
                }
                return xml;
            }
            else
            {
                oLog.WriteEntry(String.Format("\"" + WebMethodName + "\" Finished...Access Denied (" + strUser + ")"), EventLogEntryType.Information);
                return strERRORPrefix + "Access Denied (" + strUser + ")";
            }
        }
    }
}