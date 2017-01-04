<%@ Page Language="C#" Debug="false" EnableEventValidation="false" ValidateRequest="false"%>
<script runat="server">
    private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
    private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
    private int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);

    protected void Page_Load(object sender, EventArgs e)
    {
        Models oModel = new Models(0, dsn);
        Solaris oSolaris = new Solaris(0, dsn);
        string strMAC = "";
        int _boot_groupid = 4;
        string strUsername = oModel.GetBootGroup(_boot_groupid, "username");
        string strPassword = oModel.GetBootGroup(_boot_groupid, "password");
        string strExpects = oModel.GetBootGroup(_boot_groupid, "regular");
        SshShell oSSHshell = new SshShell("10.83.193.202", strUsername, strPassword);
        oSSHshell.RemoveTerminalEmulationCharacters = true;
        oSSHshell.Connect();
        if (oSSHshell.Connected == true && oSSHshell.ShellOpened == true)
        {
            // Wait for "sc>"
            string strBanner = oSSHshell.Expect(strExpects);
            // Send Command : showsc sys_enetaddr
            oSSHshell.WriteLine("show /HOST macaddress");
            // Wait for "sc>"
            strMAC = oSSHshell.Expect(strExpects);
            strMAC = oSolaris.ParseOutput(strMAC, "macaddress = ", Environment.NewLine);
        }
        oSSHshell.Close();
        Response.Write(strMAC);
    }
</script>
<html>
<body leftmargin="0" topmargin="0">
<form id="Form1" runat="server">
</form>
</body>
</html>