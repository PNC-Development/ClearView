<%@ Page Language="C#" %>
<%@ Import Namespace="PAObjectsLib" %>
<script runat="server">
    protected void Page_Load(Object Sender, EventArgs e)
    {
        int intServer = 999999;

        string strILO = "";
        string strAdminUser = "";
        string strAdminPass = "";
        string strResultsFile = "";
        string strSlot = "";

        if (Request.QueryString["blade"] != null)
        {
            strILO = "10.49.90.231";
            strAdminUser = "pt43054";
            strAdminPass = "Pnc123$";
        }
        else if (Request.QueryString["710"] != null)
        {
            strILO = "10.49.66.190";
            strAdminUser = "root";
            strAdminPass = "calvin";
        }
        else if (Request.QueryString["intel"] != null)
        {
            strILO = "10.33.189.205";
            strAdminUser = "root";
            strAdminPass = "n0tc@lv1n";
            strSlot = "15";
        }
        else
        {
            strILO = "10.49.90.110";
            strAdminUser = "root";
            strAdminPass = "calvin";
        }

        string strCommand = "POWER";
        if (Request.QueryString["command"] != null)
            strCommand = Request.QueryString["command"];

        SshShell oSSHshell = new SshShell(strILO, strAdminUser, strAdminPass);
        oSSHshell.RemoveTerminalEmulationCharacters = true;
        oSSHshell.Connect();
        if (oSSHshell.Connected == true && oSSHshell.ShellOpened == true)
        {
            Response.Write("Logged into enclosure (" + strILO + ")" + "<br/>");
            /*
            strResultsFile = oSSHshell.Expect("$").ToUpper();
            Response.Write("Found (1) $ : " + strResultsFile + "<br/>");
            if (strResultsFile.Trim() == "")
            {
                Response.Write("...still waiting for the banner..." + "<br/>");
                strResultsFile = oSSHshell.Expect("$").ToUpper();
                Response.Write("Found (2) $ : " + strResultsFile + "<br/>");
            }
            // Got banner, now get the output
            Response.Write("Writing : " + "getflexaddr -i " + strSlot + "<br/>");
            oSSHshell.WriteLine("getflexaddr -i " + strSlot);
            Response.Write("Expecting (3) $" + "<br/>");
            strResultsFile = oSSHshell.Expect("$").ToUpper();
            Response.Write("Found (3) $ : " + strResultsFile + "<br/>");
            if (strResultsFile.ToUpper().Contains("GETFLEXADDR") == false)
            {
                Response.Write("Expecting (4) $" + "<br/>");
                strResultsFile += oSSHshell.Expect("$").ToUpper();
                Response.Write("Found (4) $ : " + strResultsFile + "<br/>");
                if (strResultsFile.ToUpper().Contains("GETFLEXADDR") == false)
                {
                    Response.Write("Expecting (5) $" + "<br/>");
                    strResultsFile = oSSHshell.Expect("$").ToUpper();
                    Response.Write("Found (5) $ : " + strResultsFile + "<br/>");
                }
            }
            strResultsFile = oSSHshell.Expect("$").ToUpper();
            Response.Write("Found (6) $ : " + strResultsFile + "<br/>");
             */
            while (strResultsFile.Contains("$") == false)
            {
                strResultsFile += oSSHshell.Expect("$").ToUpper();
                Response.Write("Found (banner) $ : " + strResultsFile + "<br/>");
            }
            strResultsFile = "";
            Response.Write("Writing : " + "getflexaddr -i " + strSlot + "<br/>");
            oSSHshell.WriteLine("getflexaddr -i " + strSlot);
            while (strResultsFile.Contains("$") == false)
            {
                strResultsFile += oSSHshell.Expect("$").ToUpper();
                Response.Write("Found $ : " + strResultsFile + "<br/>");
            }
        }
        else
            Response.Write("Disconnected from the OA" + "<br/>");

        
        /*
        DateTime _now = DateTime.Now;
        string strNow = _now.Day.ToString() + _now.Month.ToString() + _now.Year.ToString() + _now.Hour.ToString() + _now.Minute.ToString() + _now.Second.ToString() + _now.Millisecond.ToString();
        string strDirectory = @"C:\Downloads\Dell\te st\";
        int intTimeoutDefault = (2 * 60 * 1000);    // 2 minutes
        bool boolTimeout = false;
        int intReturn = 0;

        ProcessStartInfo processStartInfo = new ProcessStartInfo(@"C:\Downloads\Dell\cm d\" + "racadm.exe");
        processStartInfo.WorkingDirectory = @"C:\Downloads\Dell\cm d\";
        //processStartInfo.Arguments = "-r " + strILO + " -u " + strAdminUser + " -p " + strAdminPass + " getsysinfo -s";
        processStartInfo.Arguments = "-r " + strILO + " -u " + strAdminUser + " -p " + strAdminPass + " getflexaddr -i 15";
        processStartInfo.UseShellExecute = false;
        processStartInfo.RedirectStandardOutput = true;
        Process proc = Process.Start(processStartInfo);
        StreamReader outputReader = proc.StandardOutput;
        proc.WaitForExit(intTimeoutDefault);
        if (proc.HasExited == false)
        {
            proc.Kill();
            boolTimeout = true;
        }
        if (boolTimeout == false)
        {
            intReturn = proc.ExitCode;
            if (false)
            {
                string result = outputReader.ReadToEnd();
                result = result.Substring(result.IndexOf("System Information:"));
                char[] strSplit = { '\n' };
                string[] strResults = result.Split(strSplit);
                for (int ii = 0; ii < strResults.Length; ii++)
                {
                    if (strResults[ii].Trim() != "" && strResults[ii].ToUpper().StartsWith("POWER STATUS") == true)
                    {
                        string strLine = strResults[ii].Trim().ToUpper();
                        string strValue = strLine.Substring(strLine.IndexOf("=") + 1);
                        strValue = strValue.Trim();
                    }
                }
            }
            else
            {
                string result = outputReader.ReadToEnd();
                result = result.Substring(result.ToUpper().IndexOf("10 GBE XAUI"));
                char[] strSplit = { '\n' };
                string[] strResults = result.Split(strSplit);
                if (strResults.Length > 0)
                {
                    string strLine = strResults[0].Trim().ToUpper();
                    while (strLine.Contains("  ") == true)
                        strLine = strLine.Replace("  ", " ");
                    string strValue = strLine.Substring(strLine.LastIndexOf(" ") + 1);
                    if (strValue.Contains("(") == true)
                        strValue = strValue.Substring(0, strValue.IndexOf("("));
                }
            }
            
        }
        proc.Close();
        */
    }
</script>
<script type="text/javascript">
</script>
<html>
<head>
<title>LOAD</title>
<link rel="stylesheet" type="text/css" href="/css/default.css" />
</head>
<body leftmargin="0" topmargin="0">
<form id="Form1" runat="server">
</form>
</body>
</html>