<%@ Page Language="C#" Debug="false" %>
<%@ Import Namespace="System.Data.OleDb" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System" %>
<script runat="server">
    private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
    private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
    private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
    private int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
    private TSM oTSM;
    private int intServer = 0;
    private int intDomain = 0;
    private int intSched = 0;
    
    private void Page_Load()
    {
        oTSM = new TSM(0, dsn);
    }
    private void btnLoad1_Click(Object Sender, EventArgs e)
    {
        string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=F:\\TSM.xls;Extended Properties=Excel 8.0;";
        OleDbDataAdapter myCommand = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", strConn);
        DataSet ds = new DataSet();
        myCommand.Fill(ds, "ExcelInfo");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            if (dr[0].ToString().Trim() == "")
                break;
            int _server = GetServer(dr[0].ToString().Trim());
            int _domain = GetDomain(_server, dr[1].ToString().Trim());
            int _sched = GetSchedule(_domain, dr[2].ToString().Trim());
            
        }
        Response.Write("intServer = " + intServer.ToString() + "<br/>");
        Response.Write("intDomain = " + intDomain.ToString() + "<br/>");
        Response.Write("intSched = " + intSched.ToString() + "<br/>");
    }
    private int GetServer(string _name)
    {
        object o = SqlHelper.ExecuteScalar(dsn, CommandType.Text, "SELECT id FROM cv_tsm WHERE name = '" + _name + "'");
        if (o == null || o.ToString() == "")
        {
            oTSM.Add(_name, 0, 0, 1);
            intServer++;
            return GetServer(_name);
        }
        else
            return Int32.Parse(o.ToString());
    }
    private int GetDomain(int _tsm, string _name)
    {
        object o = SqlHelper.ExecuteScalar(dsn, CommandType.Text, "SELECT id FROM cv_tsm_domains WHERE name = '" + _name + "' AND tsm = " + _tsm.ToString());
        if (o == null || o.ToString() == "")
        {
            oTSM.AddDomain(_tsm, _name, 0, 1);
            intDomain++;
            return GetDomain(_tsm, _name);
        }
        else
            return Int32.Parse(o.ToString());
    }
    private int GetSchedule(int _domain, string _name)
    {
        object o = SqlHelper.ExecuteScalar(dsn, CommandType.Text, "SELECT id FROM cv_tsm_schedules WHERE name = '" + _name + "' AND domain = " + _domain.ToString());
        if (o == null || o.ToString() == "")
        {
            oTSM.AddSchedule(_domain, _name, 0, 1);
            intSched++;
            return GetSchedule(_domain, _name);
        }
        else
            return Int32.Parse(o.ToString());
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
                <td colspan="2" class="header">TSM Import</td>
            </tr>
            <tr>
                <td colspan="2">&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td><asp:Button ID="btnLoad1" runat="server" CssClass="default" Width="150" Text="Import TSM.xls" OnClick="btnLoad1_Click" /></td>
            </tr>
        </table>
</form>
</body>
</html>