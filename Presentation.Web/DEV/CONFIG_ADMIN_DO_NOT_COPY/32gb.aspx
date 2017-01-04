<%@ Page Language="C#" Debug="false" %>
<%@ Import Namespace="System.Data.OleDb" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System" %>
<script runat="server">
    private int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
    private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
    private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
    private int intDRHourQuestion = Int32.Parse(ConfigurationManager.AppSettings["DR_HOUR_QUESTION"]);
    private int intDRHourResponse = Int32.Parse(ConfigurationManager.AppSettings["DR_HOUR_RESPONSE"]);
    private int intDRRecoveryQuestion = Int32.Parse(ConfigurationManager.AppSettings["DR_RECOVERY_QUESTION"]);
    private int intDRRecoveryResponse = Int32.Parse(ConfigurationManager.AppSettings["DR_RECOVERY_RESPONSE"]);
    private int intCount = 0;
    private int intSkip = 0;
    private void Page_Load()
    {
        intCount = 0;
        intSkip = 0;
    }
    private void btnLoad1_Click(Object Sender, EventArgs e)
    {
        Forecast oForecast = new Forecast(0, dsn);
        Classes oClass = new Classes(0, dsn);
        ModelsProperties oModelsProperties = new ModelsProperties(0, dsn);
        Asset oAsset = new Asset(0, dsnAsset);
        string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=F:\\32GBblades.xls;Extended Properties=Excel 8.0;";
        OleDbDataAdapter myCommand = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", strConn);
        DataSet ds = new DataSet();
        myCommand.Fill(ds, "ExcelInfo");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            if (dr[0].ToString().Trim() == "")
                break;
            int intAsset = Int32.Parse(dr[0].ToString().Trim());
            if (dr[4].ToString().Trim() == "1")
            {
                if (oAsset.Get(intAsset, "modelid") == "395")
                {
                    SqlHelper.ExecuteNonQuery(dsnAsset, CommandType.Text, "UPDATE cva_assets SET modelid = 424 WHERE id = " + intAsset.ToString());
                    intCount++;
                }
                else
                    intSkip++;
            }
        }
        Response.Write("intCount = " + intCount.ToString() + "<br/>");
        Response.Write("intSkip = " + intSkip.ToString() + "<br/>");
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
                <td><asp:Button ID="btnLoad1" runat="server" CssClass="default" Width="150" Text="go" OnClick="btnLoad1_Click" /></td>
            </tr>
        </table>
</form>
</body>
</html>