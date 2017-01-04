<%@ Page Language="C#" %>
<%@ Import Namespace="System.Data" %>
<script runat="server">
    private int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
    private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
    private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
    private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
    private int intCount = 0;
    private void Page_Load()
    {
    }
    private void btnGo_Click(Object Sender, EventArgs e)
    {
        Variables oVariable = new Variables(intEnvironment);
        System.Net.NetworkCredential oCredentials = new System.Net.NetworkCredential(oVariable.ADUser(), oVariable.ADPassword(), oVariable.Domain());
        ClearViewWebServices oWS = new ClearViewWebServices();
        oWS.Timeout = Int32.Parse(ConfigurationManager.AppSettings["WS_TIMEOUT"]);
        oWS.Credentials = oCredentials;
        oWS.Url = oVariable.WebServiceURL();
        lblBlueCat.Text = "<img src='/images/bigcheck.gif' border='0' align='absmiddle'/> " + oWS.SearchBluecatDNS(txtBlueCatIP.Text, txtBlueCatName.Text);
        Page.ClientScript.RegisterStartupScript(typeof(Page), "anchoring_dns", "<script type=\"text/javascript\">location.href='#bluecat'<" + "/" + "script>");
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
    <table>
        <tr>
            <td>&nbsp;</td>
            <td><asp:Button ID="btnGo" runat="server" CssClass="default" Width="150" Text="Execute" OnClick="btnGo_Click" /></td>
        </tr>
    </table>
</form>
</body>
</html>