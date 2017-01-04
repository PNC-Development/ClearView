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
        ModelsProperties oModelsProperties = new ModelsProperties(0, dsn);
        Asset oAsset = new Asset(0, dsnAsset);
        string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=F:\\VirtualConnectMgr.xls;Extended Properties=Excel 8.0;";
        OleDbDataAdapter myCommand = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", strConn);
        DataSet ds = new DataSet();
        myCommand.Fill(ds, "ExcelInfo");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            if (dr[0].ToString().Trim() == "")
                break;
            int intAsset = Int32.Parse(dr[0].ToString().Trim());
            string strOA = StripHTTP(dr[3].ToString().Trim());
            SqlHelper.ExecuteNonQuery(dsnAsset, CommandType.Text, "UPDATE cva_enclosures SET oa_ip = '" + strOA + "' WHERE assetid = " + intAsset.ToString() + " AND deleted = 0");
            string strVC1 = StripHTTP(dr[4].ToString().Trim());
            if (strVC1 != "")
                SqlHelper.ExecuteNonQuery(dsnAsset, CommandType.Text, "INSERT INTO cva_enclosures_vc VALUES (" + intAsset.ToString() + ", '" + strVC1 + "', 1, 1, getdate(), getdate(), 0)");
            string strVC2 = StripHTTP(dr[5].ToString().Trim());
            if (strVC2 != "")
                SqlHelper.ExecuteNonQuery(dsnAsset, CommandType.Text, "INSERT INTO cva_enclosures_vc VALUES (" + intAsset.ToString() + ", '" + strVC2 + "', 2, 1, getdate(), getdate(), 0)");
            string strVC3 = StripHTTP(dr[6].ToString().Trim());
            if (strVC3 != "")
                SqlHelper.ExecuteNonQuery(dsnAsset, CommandType.Text, "INSERT INTO cva_enclosures_vc VALUES (" + intAsset.ToString() + ", '" + strVC3 + "', 3, 1, getdate(), getdate(), 0)");
            string strVC4 = StripHTTP(dr[7].ToString().Trim());
            if (strVC4 != "")
                SqlHelper.ExecuteNonQuery(dsnAsset, CommandType.Text, "INSERT INTO cva_enclosures_vc VALUES (" + intAsset.ToString() + ", '" + strVC4 + "', 4, 1, getdate(), getdate(), 0)");
            string strVC5 = StripHTTP(dr[8].ToString().Trim());
            if (strVC5 != "")
                SqlHelper.ExecuteNonQuery(dsnAsset, CommandType.Text, "INSERT INTO cva_enclosures_vc VALUES (" + intAsset.ToString() + ", '" + strVC5 + "', 5, 1, getdate(), getdate(), 0)");
            intCount++;
        }
        Response.Write("Done = " + intCount.ToString() + "<br/>");
    }
    private string StripHTTP(string _return)
    {
        string strReturn = _return;
        if (strReturn != "")
        {
            strReturn = strReturn.Substring(strReturn.IndexOf("//") + 2);
            while (strReturn.Contains("/") == true)
                strReturn = strReturn.Replace("/", "");
        }
        return strReturn.Trim();
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
                <td><asp:Button ID="btnLoad1" runat="server" CssClass="default" Width="150" Text="Check Completed Designs" OnClick="btnLoad1_Click" /></td>
            </tr>
        </table>
</form>
</body>
</html>