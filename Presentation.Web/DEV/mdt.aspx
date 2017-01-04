<%@ Page Language="C#" Debug="false" %>
<%@ Import Namespace="System.Data.OleDb" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System" %>
<script runat="server">
    private int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
    private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
    private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
    private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
    private int intCount1 = 0;
    private int intCount2 = 0;
    private void Page_Load()
    {
    }
    private void btnGo_Click(Object Sender, EventArgs e)
    {
        Variables oVariable = new Variables(intEnvironment);
        System.Net.NetworkCredential oCredentials = new System.Net.NetworkCredential(oVariable.ADUser(), oVariable.ADPassword(), oVariable.Domain());
        NCC.ClearView.Application.Core.w08r2.BuildSubmit oMDT = new NCC.ClearView.Application.Core.w08r2.BuildSubmit();
        oMDT.Credentials = oCredentials;
        oMDT.Url = "http://daltonbuild-test.pncbank.com/wabebuild/BuildSubmit.asmx";
        //oMDT.Url = "http://daltonbuild.pncbank.com/wabebuild/BuildSubmit.asmx";

        string strName = "Healy20150527";
        string strMACAddress = "00:50:56:9b:32:70";
        string strBootEnvironment = "WABEx64";
        string strDomain = "PNCNT";
        string strTaskSequence = "W2K8R2_ENT";
        string strBackup = "Avamar";
        bool boolIIS = false;
        string strHIDs = "NO";

        string[] strExtendedMDT = new string[] { "PNCBACKUPSOFTWARE:" + strBackup, "IISInstall:" + (boolIIS ? "YES" : "NO"), "HWConfig:DEFAULT", "ESMInstall:YES", "ClearViewInstall:YES", "Teamed2:DEFAULT", "HIDSInstall:" + strHIDs };
        string strExtendedMDTs = "";
        foreach (string extendedMDT in strExtendedMDT)
        {
            if (strExtendedMDTs != "")
                strExtendedMDTs += ", ";
            strExtendedMDTs += extendedMDT;
        }
        string result = oMDT.automatedBuild2(strName, strMACAddress, strBootEnvironment, "provision", strDomain, strTaskSequence, "ServerShare", strExtendedMDT);
        
        //oMDT.ForceCleanup("TESTING", "abcd1234", "ServerShare");
        Response.Write("<br/>");
    }
</script>
<script type="text/javascript">
</script>
<html>
<head>
<title>Test</title>
<link rel="stylesheet" type="text/css" href="/css/default.css" />
</head>
<body leftmargin="0" topmargin="0">
<form id="Form1" runat="server">
    <table>
        <tr>
            <td colspan="2" class="header">Test</td>
        </tr>
        <tr>
            <td colspan="2">&nbsp;</td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td><asp:Button ID="btnGo" runat="server" CssClass="default" Width="150" Text="Go" OnClick="btnGo_Click" /></td>
        </tr>
    </table>
</form>
</body>
</html>