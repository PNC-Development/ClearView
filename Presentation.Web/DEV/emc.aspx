<%@ Page Language="C#" Debug="false" EnableEventValidation="false" ValidateRequest="false"%>
<%@ Import Namespace="Tamir.Streams" %>

<script runat="server">
    private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
    private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
    private int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);

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
</script>
<html>
<body leftmargin="0" topmargin="0">
<form id="Form1" runat="server">
</form>
</body>
</html>