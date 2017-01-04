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
    private int intError = 0;
    private string strErrors = "";
    private string strErrorTable = "cv_errors";
    private string strErrorServerTable = "cv_servers_errors";
    
    private void Page_Load()
    {
        if (!IsPostBack)
        {
        }
    }
    private void btnImport_Click(Object sender, EventArgs e)
    {
        Servers oServer = new Servers(0, dsn);
        Forecast oForecast = new Forecast(0, dsn);
        int intCount = 0;
        int intTotal = 0;
        string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\\codes.xls;Extended Properties=Excel 8.0;";
        OleDbDataAdapter myCommand = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", strConn);
        DataSet ds = new DataSet();
        myCommand.Fill(ds, "ExcelInfo");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            string strName = dr[0].ToString().Trim();
            Response.Write("Trying..." + strName + "...");
            DataSet dsServer = oServer.Get(strName);
            if (dsServer.Tables[0].Rows.Count == 1)
            {
                int intAnswer = Int32.Parse(dsServer.Tables[0].Rows[0]["answerid"].ToString());
                Response.Write("Found...DesignID = " + intAnswer.ToString() + "...");
                string strCodeCurrent = dr[1].ToString().Trim();
                string strCodeNew = dr[2].ToString().Trim();
                string strCode = oForecast.GetAnswer(intAnswer, "appcode");
                if (strCode.ToUpper() == strCodeCurrent.ToUpper())
                {
                    Response.Write("Codes match...");
                    DataSet dsServers = oServer.GetAnswer(intAnswer);
                    if (dsServers.Tables[0].Rows.Count == 1)
                    {
                        Response.Write("Only one record...OK TO UPDATE...");
                        SqlHelper.ExecuteNonQuery(dsn, CommandType.Text, "UPDATE cv_forecast_answers SET appcode = '" + strCodeNew + "' WHERE id = " + intAnswer.ToString());
                        intCount = intCount + 1;
                        Response.Write("UPDATE DONE!!<br/>");
                    }
                    else
                        Response.Write("Error...Design Count = " + dsServers.Tables[0].Rows.Count.ToString() + "<br/>");
                }
                else
                    Response.Write("Error...Codes do not match (ClearView = " + strCode + ", Sheet = " + strCodeCurrent.ToUpper() + ")" + "<br/>");
            }
            else
                Response.Write("Error...Search Count = " + dsServer.Tables[0].Rows.Count.ToString() + "<br/>");
            intTotal++;
        }
        Response.Write("Total Modified: " + intCount.ToString() + " of " + intTotal.ToString() + "<br/>");
    }
</script>
<script type="text/javascript">
</script>
<html>
<head>
<title>LOAD</title>
<link rel="stylesheet" type="text/css" href="/css/default.css" />
</head>
<body>
<form id="Form1" runat="server">
    <table>
        <tr>
            <td>&nbsp;</td>
            <td><asp:Button ID="btnImport" runat="server" CssClass="default" Width="150" Text="Delete and Import" OnClick="btnImport_Click" /></td>
        </tr>
    </table>
</form>
</body>
</html>