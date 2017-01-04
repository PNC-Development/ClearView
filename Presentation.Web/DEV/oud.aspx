<%@ Page Language="C#" Debug="true" %>
<%@ Import Namespace="NCC.ClearView.Application.Core.Proteus" %>
<%@ Import Namespace="Novell.Directory.Ldap" %>

<script runat="server">
    private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
    private int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
    private Variables oVariable;
    private Functions oFunction;

    protected void Page_Load(object sender, EventArgs e)
    {
        oVariable = new Variables(999);
        oFunction = new Functions(0, dsn, intEnvironment);
    }
    protected void btnManager_Click(object sender, EventArgs e)
    {
        SearchResultCollection results = oFunction.eDirectory("pncmanagerid", "pt43039");
        Response.Write(results.GetType().ToString() == null);
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
        <p><asp:Button ID="btnManager" runat="server" Text="Manager" OnClick="btnManager_Click" /></p>
</form>
</body>
</html>