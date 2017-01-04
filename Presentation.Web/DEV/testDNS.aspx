<%@ Page Language="C#" Debug="false" EnableEventValidation="false" ValidateRequest="false"%>
<script runat="server">
    private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
    private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
    private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
    private int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
    protected int intCount = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
    }
    public void btnCreate_Click(Object Sender, EventArgs e)
    {
        Variables oVariable = new Variables(intEnvironment);
        System.Net.NetworkCredential oCredentials = new System.Net.NetworkCredential(oVariable.ADUser(), oVariable.ADPassword(), oVariable.Domain());
        ClearViewWebServices oWS = new ClearViewWebServices();
        oWS.Credentials = oCredentials;
        oWS.Url = "http://ohcleiis104s/clearviewwebservices.asmx";
        //oWS.Url = oVariable.WebServiceURL();
        Response.Write(oWS.CreateDNSforPNC(txtIP.Text, txtName.Text, "Server", "", oVariable.DNS_Domain(), oVariable.DNS_NameService(), oVariable.DNS_DynamicDNSUpdate(), 14781, true));
    }
    public void btnUpdate_Click(Object Sender, EventArgs e)
    {
        Variables oVariable = new Variables(intEnvironment);
        System.Net.NetworkCredential oCredentials = new System.Net.NetworkCredential(oVariable.ADUser(), oVariable.ADPassword(), oVariable.Domain());
        ClearViewWebServices oWS = new ClearViewWebServices();
        oWS.Credentials = oCredentials;
        oWS.Url = "http://ohcleiis104s/clearviewwebservices.asmx";
        //oWS.Url = oVariable.WebServiceURL();
        Response.Write(oWS.UpdateDNSforPNC(txtIP.Text, txtName.Text, "Server", "", oVariable.DNS_Domain(), oVariable.DNS_NameService(), oVariable.DNS_DynamicDNSUpdate(), 14781, true));
    }
    public void btnDelete_Click(Object Sender, EventArgs e)
    {
        Variables oVariable = new Variables(intEnvironment);
        System.Net.NetworkCredential oCredentials = new System.Net.NetworkCredential(oVariable.ADUser(), oVariable.ADPassword(), oVariable.Domain());
        ClearViewWebServices oWS = new ClearViewWebServices();
        oWS.Credentials = oCredentials;
        oWS.Url = "http://ohcleiis104s/clearviewwebservices.asmx";
        //oWS.Url = oVariable.WebServiceURL();
        Response.Write(oWS.DeleteDNSforPNC(txtIP.Text, txtName.Text, 14781, true));
    }
    public void btnSearch_Click(Object Sender, EventArgs e)
    {
        Variables oVariable = new Variables(intEnvironment);
        System.Net.NetworkCredential oCredentials = new System.Net.NetworkCredential(oVariable.ADUser(), oVariable.ADPassword(), oVariable.Domain());
        ClearViewWebServices oWS = new ClearViewWebServices();
        oWS.Credentials = oCredentials;
        oWS.Url = "http://ohcleiis104s/clearviewwebservices.asmx";
        //oWS.Url = oVariable.WebServiceURL();
        Response.Write(oWS.SearchDNSforPNC(txtIP.Text, txtName.Text, true));
    }
</script>
<html>
<head id="Head1" runat="server">
<title id="Title1" runat="server">National City</title>
<link rel="stylesheet" type="text/css" href="/css/default.css" />
<script src="/javascript/default.js"type="text/javascript"></script>
<script type="text/javascript" src="/javascript/ajax.js"></script>
<script type="text/javascript" src="/javascript/both.js"></script>
</head>
<body leftmargin="0" topmargin="0">
<form id="Form1" runat="server">
    <table border="0">
        <tr>
            <td>IP Address:</td>
            <td><asp:TextBox ID="txtIP" runat="server" CssClass="default" /></td>
        </tr>
        <tr>
            <td>Name:</td>
            <td><asp:TextBox ID="txtName" runat="server" CssClass="default" /></td>
        </tr>
        <tr>
            <td></td>
            <td><asp:Button ID="btnCreate" runat="server" CssClass="default" Text="Create" Width="75" OnClick="btnCreate_Click" /></td>
        </tr>
        <tr>
            <td></td>
            <td><asp:Button ID="btnUpdate" runat="server" CssClass="default" Text="Update" Width="75" OnClick="btnUpdate_Click" /></td>
        </tr>
        <tr>
            <td></td>
            <td><asp:Button ID="btnDelete" runat="server" CssClass="default" Text="Delete" Width="75" OnClick="btnDelete_Click" /></td>
        </tr>
        <tr>
            <td></td>
            <td><asp:Button ID="btnSearch" runat="server" CssClass="default" Text="Search" Width="75" OnClick="btnSearch_Click" /></td>
        </tr>
    </table>
</form>
</body>
</html>