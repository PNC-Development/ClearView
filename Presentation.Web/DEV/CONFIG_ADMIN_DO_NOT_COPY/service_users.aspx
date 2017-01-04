<%@ Page Language="C#" Debug="false" %>
<%@ Import Namespace="System.Data.OleDb" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System" %>
<script runat="server">
    private int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
    private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
    private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
    private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
    private string dsnServiceEditor = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ServiceEditorDSN"]].ConnectionString;
    private int intCount = 0;
    private int intError = 0;
    private void Page_Load()
    {
        intCount = 0;
        intError = 0;
    }
    private void btnLoad1_Click(Object Sender, EventArgs e)
    {
        AddServices(14752, 14748);
        Response.Write("count = " + intCount.ToString());
    }
    private void AddServices(int _userid_existing, int _userid_new)
    {
        AddServices(_userid_existing, _userid_new, 1);
        AddServices(_userid_existing, _userid_new, -1);
    }
    private void AddServices(int _userid_existing, int _userid_new, int _assign)
    {
        DataSet ds = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT DISTINCT serviceid FROM cv_services_users WHERE (serviceid IN (SELECT serviceid FROM cv_services_users AS cv_services_users_1 WHERE assign = " + _assign.ToString() + " AND (userid = " + _userid_existing.ToString() + "))) ORDER BY serviceid");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            int intService = Int32.Parse(dr["serviceid"].ToString());
            AddUser(intService, _userid_new, _assign);
            intCount++;
        }
    }
    private void AddUser(int _serviceid, int _userid_new, int _assign)
    {
        Services oService = new Services(0, dsn);
        DataSet dsUser = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT * FROM cv_services_users WHERE assign = " + _assign.ToString() + " AND userid = " + _userid_new.ToString() + " AND serviceid = " + _serviceid.ToString() + " AND deleted = 0");
        if (dsUser.Tables[0].Rows.Count == 0)
            oService.AddUser(_serviceid, _userid_new, _assign);
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
                <td colspan="2" class="header">Fix</td>
            </tr>
            <tr>
                <td colspan="2">&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td><asp:Button ID="btnLoad1" runat="server" CssClass="default" Width="150" Text="Load" OnClick="btnLoad1_Click" /></td>
            </tr>
        </table>
</form>
</body>
</html>