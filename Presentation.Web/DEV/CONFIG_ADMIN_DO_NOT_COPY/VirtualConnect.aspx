<%@ Page Language="C#" Debug="false" %>
<%@ Import Namespace="System.Data.OleDb" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System" %>
<script runat="server">
    private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
    private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
    private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
    private int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
    private int intCount = 0;
    
    private void Page_Load()
    {
    }
    private void btnLoad1_Click(Object Sender, EventArgs e)
    {
        string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=F:\\VirtualConnectMgr.xls;Extended Properties=Excel 8.0;";
        OleDbDataAdapter myCommand = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", strConn);
        DataSet ds = new DataSet();
        myCommand.Fill(ds, "ExcelInfo");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            if (dr[0].ToString().Trim() == "")
                break;
            int intEnclosure = Int32.Parse(dr[0].ToString());
            string strIP = dr[10].ToString();
            SqlHelper.ExecuteNonQuery(dsnAsset, CommandType.Text, "INSERT INTO cva_enclosures_vc VALUES (" + intEnclosure.ToString() + ", '" + strIP + "', 1, 1, getdate(), getdate(), 0)");
            intCount++;
        }
        Response.Write("Done = " + intCount.ToString() + "<br/>");
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
                <td colspan="2" class="header">Import</td>
            </tr>
            <tr>
                <td colspan="2">&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td><asp:Button ID="btnLoad1" runat="server" CssClass="default" Width="150" Text="Go" OnClick="btnLoad1_Click" /></td>
            </tr>
        </table>
</form>
</body>
</html>