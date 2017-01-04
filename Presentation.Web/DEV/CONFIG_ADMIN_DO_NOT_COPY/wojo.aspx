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
        intCount = 0;
    }
    private void btnLoad1_Click(Object Sender, EventArgs e)
    {
        Servers oServer = new Servers(0, dsn);
        IPAddresses oIPAddresses = new IPAddresses(0, dsnIP, dsn);
        string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=F:\\wojo.xls;Extended Properties=Excel 8.0;";
        OleDbDataAdapter myCommand = new OleDbDataAdapter("SELECT DISTINCT Source FROM [Sheet1$]", strConn);
        DataSet ds = new DataSet();
        myCommand.Fill(ds, "ExcelInfo");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            if (dr[0].ToString().Trim() == "")
                break;
            string strIP = dr[0].ToString().Trim();
            Response.Write("Searching for <b>" + strIP + "</b>...<br/>");
            int intIP1 = Int32.Parse(strIP.Substring(0, strIP.IndexOf(".")));
            strIP = strIP.Substring(strIP.IndexOf(".") + 1);
            int intIP2 = Int32.Parse(strIP.Substring(0, strIP.IndexOf(".")));
            strIP = strIP.Substring(strIP.IndexOf(".") + 1);
            int intIP3 = Int32.Parse(strIP.Substring(0, strIP.IndexOf(".")));
            strIP = strIP.Substring(strIP.IndexOf(".") + 1);
            int intIP4 = Int32.Parse(strIP);
            DataSet dsIP = oIPAddresses.Get(intIP1, intIP2, intIP3, intIP4);
            if (dsIP.Tables[0].Rows.Count == 0)
                Response.Write(".....Could not find any addresses<br/>");
            else if (dsIP.Tables[0].Rows.Count > 1)
                Response.Write(".....Found multiple addresses<br/>");
            else
            {
                Response.Write(".....FOUND 1 ADDRESS...<br/>");
                int intIP = Int32.Parse(dsIP.Tables[0].Rows[0]["id"].ToString());
                DataSet dsServers = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT serverid FROM cv_servers_ips WHERE ipaddressid = " + intIP.ToString() + " AND deleted = 0");
                if (dsServers.Tables[0].Rows.Count == 0)
                    Response.Write(".....Could not find any servers<br/>");
                else if (dsServers.Tables[0].Rows.Count > 1)
                    Response.Write(".....Found multiple servers<br/>");
                else
                {
                    Response.Write(".....FOUND 1 SERVER...<br/>");
                    int intServer = Int32.Parse(dsServers.Tables[0].Rows[0]["serverid"].ToString());
                    Response.Write(".....<b>Server Name:</b>" + oServer.GetName(intServer, true) + "<br/>");
                }
            }
            Response.Write("<br/>");
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
                <td colspan="2" class="header">Code Push Updates</td>
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