<%@ Page Language="C#" Debug="false" %>
<%@ Import Namespace="System.Data.OleDb" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System" %>
<script runat="server">
    private int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
    private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
    private void Page_Load()
    {
    }
    private void btnImport_Click(Object Sender, EventArgs e)
    {
        int intDummy = 0;
        ServerName oServerName = new ServerName(0, dsn);
        string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=F:\\names.xls;Extended Properties=Excel 8.0;";
        OleDbDataAdapter myCommand = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", strConn);
        DataSet ds = new DataSet();
        myCommand.Fill(ds, "ExcelInfo");
        int intCount = 0;
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            intCount++;
            if (intCount == 7160)
                break;
            string strName = dr[0].ToString().Trim().ToUpper();
            if (strName != "" && strName.Length == 12)
            {
                string strName1 = strName.Substring(0, 5);
                strName = strName.Substring(5);
                string strName2 = strName.Substring(0, 3);
                strName = strName.Substring(3);
                string strName3 = strName.Substring(0, 2);
                strName = strName.Substring(2);
                string strName4 = strName.Substring(0, 1);
                strName = strName.Substring(1);
                int intName = 0;
                DataSet dsNames = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT * FROM cv_servernames WHERE prefix1 = '" + strName1 + "' AND prefix2 = '" + strName2 + "' AND sitecode = '" + strName3 + "' AND name1 = '" + strName4 + "' AND name2 = '" + strName + "' AND deleted = 0");
                if (dsNames.Tables[0].Rows.Count == 0)
                {
                    oServerName.Add(0, strName1, strName2, strName3, strName4, strName, 0, "ASSETCENTER", 0);
                    intDummy++;
                }
            }
        }
        Response.Write("NAMES: " + intDummy.ToString() + "<br/>");
    }
</script>
<script type="text/javascript">
</script>
<html>
<head>
<title>IMPORT Assets</title>
<link rel="stylesheet" type="text/css" href="/css/default.css" />
</head>
<body leftmargin="0" topmargin="0">
<form id="Form1" runat="server">
        <table>
            <tr>
                <td colspan="2" class="header">Import Assets</td>
            </tr>
            <tr>
                <td colspan="2">&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td><asp:Button ID="btnImport" runat="server" CssClass="default" Width="150" Text="Import Assets" OnClick="btnImport_Click" /></td>
            </tr>
        </table>
</form>
</body>
</html>