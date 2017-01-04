<%@ Page Language="C#" %>
<%@ Import Namespace="PAObjectsLib" %>
<script runat="server">
    private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
    private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
    private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
    private string dsnServiceEditor = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ServiceEditorDSN"]].ConnectionString;
    private int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
    private Variables oVariable;

    protected void Page_Load(object sender, EventArgs e)
    {
        oVariable = new Variables(intEnvironment);
    }
    protected void btnGo_Click(object sender, EventArgs e)
    {
        Response.Write("Start = " + DateTime.Now.ToString() + "...<br/>");
        Pages oPage = new Pages(0, dsn);
        int intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
        DataSet dsTotal = oPage.GetTotal(intProfile, "pr_getServiceRequestsMine");
        Response.Write("Start = " + DateTime.Now.ToString() + "...<br/>");
    }

</script>
<script type="text/javascript">
</script>
<html>
<head>
<title>LOAD</title>
<link rel="stylesheet" type="text/css" href="/css/default.css" />
<script src="/javascript/default.js"type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="btnGo_Click" />
    </div>
    </form>
</body>
</html>