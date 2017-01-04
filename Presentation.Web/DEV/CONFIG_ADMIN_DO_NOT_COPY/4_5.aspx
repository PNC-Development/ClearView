<%@ Page Language="C#" Debug="false" %>
<%@ Import Namespace="System.Data.OleDb" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System" %>
<script runat="server">
    private int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
    private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
    private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
    private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
    private int intCount = 0;
    private int intError = 0;
    private void Page_Load()
    {
        intCount = 0;
        intError = 0;
    }
    private void btnLoad1_Click(Object Sender, EventArgs e)
    {
        Field oField = new Field(0, dsn);
        SqlHelper.ExecuteNonQuery(dsn, CommandType.Text, "DELETE FROM cv_tables_fields_permissions WHERE deleted = 1");
        SqlHelper.ExecuteNonQuery(dsn, CommandType.Text, "UPDATE cv_tables_fields_permissions SET deleted = -1 WHERE deleted = 0");
        DataSet ds = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT * FROM cv_tables_fields_permissions");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            int intItem = Int32.Parse(dr["serviceid"].ToString());
            int intTable = Int32.Parse(dr["tableid"].ToString());
            DataSet dsService = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT * FROM cv_services WHERE itemid = " + intItem.ToString());
            foreach (DataRow drService in dsService.Tables[0].Rows)
            {
                int intService = Int32.Parse(drService["serviceid"].ToString());
                oField.AddPermission2(intService, intTable);
            }
            intCount++;
        }
        SqlHelper.ExecuteNonQuery(dsn, CommandType.Text, "DELETE FROM cv_tables_fields_permissions WHERE deleted = -1");
        Response.Write("Done = " + intCount.ToString() + "<br/>");
    }
    private void btnLoad2_Click(Object Sender, EventArgs e)
    {
        SqlHelper.ExecuteNonQuery(dsn, CommandType.Text, "UPDATE cv_resource_requests_details SET resourceid = 0 WHERE resourceid in (SELECT id FROM cv_resource_requests_workflow WHERE joined = 1)");
        Response.Write("Done!<br/>");
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
                <td colspan="2" class="header">4.5</td>
            </tr>
            <tr>
                <td colspan="2">&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td><asp:Button ID="btnLoad1" runat="server" CssClass="default" Width="150" Text="Table Perms" OnClick="btnLoad1_Click" /></td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td><asp:Button ID="btnLoad2" runat="server" CssClass="default" Width="150" Text="RR Details" OnClick="btnLoad2_Click" /></td>
            </tr>
        </table>
</form>
</body>
</html>