<%@ Page Language="C#" %>
<%@ Import Namespace="PAObjectsLib" %>
<script runat="server">
    private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
    private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
    private int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
    private int intDefaultSwitchTimeout = 1000;
    private int intDefaultSwitchPort = 23;
    private int intSwitchMaxLoops = 100;
    private string strSwitchBreak = "\r\n";
    private string strSwitchReturn = Environment.NewLine;
    private bool _debug = true;
    protected void Page_Load(Object Sender, EventArgs e)
    {
        Variables oVariable = new Variables(999);
        Asset oAsset = new Asset(0, dsnAsset, dsn);
        string strHost = "10.49.254.12";
        string strUser = "clearview";
        string strPass = "Clearvi3w";
        string strInterface = "102/1/4";
        string strReturn = "";
        if (Request.QueryString["host"] != null)
        {
            strHost = Request.QueryString["host"];
            strUser = oVariable.NexusUsername();
            strPass = oVariable.NexusPassword();
        }
        if (Request.QueryString["interface"] != null)
            strInterface = Request.QueryString["interface"];
        SshShell oSSHshell = new SshShell(strHost, strUser, strPass);
        oSSHshell.RemoveTerminalEmulationCharacters = true;
        oSSHshell.Connect();
        string strLogin = GetDellSwitchportOutput(oSSHshell);
        if (strLogin == "**INVALID**")
        {
            txtResults.Text = "Invalid Login";
        }
        else
        {
            //txtResults.Text = oAsset.GetDellSwitchportOutput(oSSHshell, strInterface, DellBladeSwitchportType.Config, 0);
            //txtResults.Text = GetDellSwitchportOutput(oSSHshell, strInterface, DellBladeSwitchportType.Config);
            txtResults.Text = GetDellSwitchportOutput(oSSHshell, strInterface, DellBladeSwitchportType.Mode);
            //WriteDellSwitchport(oSSHshell, "show run int eth 101/1/20");
            //txtResults.Text = GetDellSwitchportOutput(oSSHshell);
            //txtResults.Text += oAsset.ChangeDellSwitchport(oSSHshell, strInterface, DellBladeSwitchportMode.Access, "2515", "test healy for SSH - ok to delete", true, 0);
            //txtResults.Text += oAsset.GetDellSwitchportOutput(oSSHshell, strInterface, DellBladeSwitchportType.Config, 0);
        }
        oSSHshell.Close();
        

        /*
        TelnetConnection telnet = oAsset.LoginDellSwitchport(strHost, strUser, strPass);
        if (telnet == null)
        {
            txtResults.Text = "Invalid Login";
        }
        else
        {
            //if (_debug == true)
            //    strReturn += "DEBUG: " + strLogin + strSwitchReturn;

            telnet.WriteLine("show run int eth 101/1/32");
            if (_debug == true)
                strReturn += "DEBUG: " + ReadOutput(telnet, true) + strSwitchReturn;

            strReturn += oAsset.ChangeDellSwitchport(telnet, "101/1/32", DellBladeSwitchportMode.Trunk, new string[] { "1","2" }, "healy") + strSwitchReturn;

            telnet.WriteLine("show run int eth 101/1/32");
            if (_debug == true)
                strReturn += "DEBUG: " + ReadOutput(telnet, true) + strSwitchReturn;

            //
            //strReturn += oAsset.GetDellSwitchportOutput(telnet, "101/1/21", DellBladeSwitchportType.VLANs);
            
            //telnet.WriteLine("show int eth 101/1/21 br");
            //string strTest = ReadOutput(telnet, true);
            //strTest = strTest.Substring(strTest.IndexOf("Eth101/1/21"));

            //if (_debug == true)
            //    strReturn += "DEBUG: " + GetOutput(strTest, "VLAN") + strSwitchReturn;

            //if (_debug == true)
            //    strReturn += "DEBUG: " + GetOutput(strTest, "MODE") + strSwitchReturn;

            //if (_debug == true)
            //    strReturn += "DEBUG: " + GetOutput(strTest, "STATUS") + strSwitchReturn;

            //if (_debug == true)
            //    strReturn += "DEBUG: " + strTest + strSwitchReturn;

            telnet.WriteLine("exit");
            if (_debug == true)
                strReturn += "DEBUG: " + ReadOutput(telnet, true) + strSwitchReturn;
            txtResults.Text = strReturn;
        }
        */
    }
    public string GetDellSwitchportOutput(SshShell oSSHshell)
    {
        string strRead = oSSHshell.Expect("#");
        if (strRead != "")
        {
            if (strRead.ToUpper().Contains("LOGIN INCORRECT") || strRead.ToUpper().Contains("ACCESS DENIED"))
            {
                // Set return value to "**INVALID**" and return
                strRead = "**INVALID**";
            }
        }
        return strRead;
    }
    public string WriteDellSwitchport(SshShell oSSHshell, string _command, int _pounds)
    {
        oSSHshell.WriteLine(_command);
        string strReturn = GetDellSwitchportOutput(oSSHshell);
        for (int ii = 1; ii < _pounds; ii++)
            strReturn += GetDellSwitchportOutput(oSSHshell);
        return strReturn;
    }
    public string GetDellSwitchportOutput(SshShell oSSHshell, string _interface, DellBladeSwitchportType _type)
    {
        string strReturn = "";

        if (_type == DellBladeSwitchportType.VLANs || _type == DellBladeSwitchportType.Description || _type == DellBladeSwitchportType.Config)
        {
            // show run int eth 101/1/32
            string strOutput = WriteDellSwitchport(oSSHshell, "show run int eth " + _interface, 1);
            if (strOutput.ToUpper().Contains("ETHERNET" + _interface) == true)
            {
                // Get from start of output
                strOutput = strOutput.Substring(strOutput.ToUpper().LastIndexOf("ETHERNET" + _interface));
                // Only get first line of output
                if (strOutput.Contains(Environment.NewLine + Environment.NewLine) == true)
                    strOutput = strOutput.Substring(0, strOutput.IndexOf(Environment.NewLine + Environment.NewLine));
                // Split into array (by the single spaces)
                string[] strOutputs = Regex.Split(strOutput, Environment.NewLine);
                // Replace all extra spaces
                while (strOutput.Contains("  ") == true)
                    strOutput = strOutput.Replace("  ", " ");
                // Get the value
                switch (_type)
                {
                    case DellBladeSwitchportType.Config:
                        strReturn = strOutput;
                        break;
                    case DellBladeSwitchportType.VLANs:
                        for (int ii = 0; ii < strOutputs.Length; ii++)
                        {
                            string strLine = strOutputs[ii].Trim();
                            string strTrunk = "SWITCHPORT TRUNK ALLOWED VLAN";
                            string strAccess = "SWITCHPORT ACCESS VLAN";
                            if (strLine.StartsWith(strTrunk))
                            {
                                strReturn = strLine.Substring(strTrunk.Length + 1).Trim();
                                break;
                            }
                            else if (strLine.StartsWith(strAccess))
                            {
                                strReturn = strLine.Substring(strAccess.Length + 1).Trim();
                                break;
                            }
                        }
                        break;
                    case DellBladeSwitchportType.Description:
                        for (int ii = 0; ii < strOutputs.Length; ii++)
                        {
                            string strLine = strOutputs[ii].Trim();
                            string strDescription = "DESCRIPTION";
                            if (strLine.StartsWith(strDescription))
                            {
                                strReturn = strLine.Substring(strDescription.Length + 1).Trim();
                                break;
                            }
                        }
                        break;
                }
            }
        }
        else
        {
            // show int eth 101/1/32 br
            string strOutput = WriteDellSwitchport(oSSHshell, "show int eth " + _interface + " br", 2);
            string strTemp = strOutput;
            if (strOutput.ToUpper().Contains("ETH" + _interface) == true)
            {
                // Get from start of output
                strOutput = strOutput.Substring(strOutput.ToUpper().IndexOf("ETH" + _interface));
                // Only get first line of output
                if (strOutput.Contains(Environment.NewLine) == true)
                    strOutput = strOutput.Substring(0, strOutput.IndexOf(Environment.NewLine));
                // Replace all extra spaces
                while (strOutput.Contains("  ") == true)
                    strOutput = strOutput.Replace("  ", " ");
                // Split into array (by the single spaces)
                string[] strOutputs = strOutput.Split(new char[] { ' ' });
                // Get the value
                switch (_type)
                {
                    case DellBladeSwitchportType.VLAN:
                        strReturn = strOutputs[1] + " (" + strTemp + ")";
                        break;
                    case DellBladeSwitchportType.Mode:
                        strReturn = strOutputs[3] + " (" + strTemp + ")";
                        break;
                    case DellBladeSwitchportType.Status:
                        strReturn = strOutputs[4] + " (" + strTemp + ")";
                        break;
                }
            }
            else
                strReturn = "ERROR: " + strOutput;
        }
        return strReturn;
    }
    private string ReadOutput(TelnetConnection oTelnet, bool boolCheck)
    {
        string strReturn = "";
        string strRead = "";
        bool boolAlready = false;
        int intCount = 0;
        while (boolCheck == true && oTelnet.IsConnected == true && intCount < intSwitchMaxLoops)
        {
            intCount++;
            strRead = oTelnet.Read();
            if (strRead != "")
                boolAlready = true;
            if (strRead == "" && boolAlready == true)
                break;
            strReturn += strRead;
            
        }
        return strReturn;
    }
    private string GetOutput(string _results, string _type)
    {
        string strReturn = "";
        if (_results.Contains(Environment.NewLine) == true)
            _results = _results.Substring(0, _results.IndexOf(Environment.NewLine));
        while (_results.Contains("  ") == true)
            _results = _results.Replace("  ", " ");
        string[] strResults = _results.Split(new char[] { ' ' });
        switch (_type)
        {
            case "VLAN":
                strReturn = strResults[1];
                break;
            case "MODE":
                strReturn = strResults[3];
                break;
            case "STATUS":
                strReturn = strResults[4];
                break;
        }
        return strReturn;
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
<asp:TextBox ID="txtResults" runat="server" Width="800" TextMode="MultiLine" Rows="50" />
</form>
</body>
</html>