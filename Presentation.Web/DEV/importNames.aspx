<%@ Page Language="C#" Debug="false" %>
<%@ Import Namespace="System.Data.OleDb" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System" %>
<script runat="server">
    private int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
    private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
    private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
    private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
    private int intCount1 = 0;
    private int intCount2 = 0;
    private void Page_Load()
    {
    }
    private void btnGo_Click(Object Sender, EventArgs e)
    {
        string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=F:\\import.xls;Extended Properties=Excel 8.0;";
        
        ServerName oServerName = new ServerName(0, dsn);
        OleDbDataAdapter myCommand1 = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", strConn);
        DataSet ds1 = new DataSet();
        myCommand1.Fill(ds1, "ExcelInfo");
        foreach (DataRow dr in ds1.Tables[0].Rows)
        {
            if (dr[0].ToString().Trim() == "")
                break;
            string strName = dr[1].ToString().Trim().ToUpper();
            if (strName.Length == 9 || strName.Length == 10) 
            {
                Response.Write(strName.ToUpper() + "......");
                string strOS = strName.Substring(0, 1);
                string strLocation = strName.Substring(1, 1);
                string strMnemonic = strName.Substring(2, 3);
                string strEnvironment = strName.Substring(5, 1);
                string strName1 = strName.Substring(6, 1);
                string strName2 = strName.Substring(7, 1);
                string strFunc = strName.Substring(8, 1);
                string strSpec = "";
                if (strName.Length == 10)
                    strSpec = strName.Substring(9, 1);
                oServerName.AddFactory(strOS, strLocation, strMnemonic, strEnvironment, strName1, strName2, strFunc, strSpec, -999, "MidlandImport", 0);
                Response.Write("Added");
                Response.Write("<br/>");
            }
        }
        
    }
 </script>
<script type="text/javascript">
</script>
<html>
<head>
<title>IMPORT FUNCTIONS</title>
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
            <td><asp:Button ID="btnGo" runat="server" CssClass="default" Width="150" Text="Import Sheet" OnClick="btnGo_Click" /></td>
        </tr>
    </table>
</form>
</body>
</html>