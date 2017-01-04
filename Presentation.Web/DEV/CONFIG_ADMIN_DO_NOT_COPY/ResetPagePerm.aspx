<%@ Page Language="C#" Debug="false" %>
<%@ Import Namespace="System.Data.OleDb" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System" %>
<script runat="server">
    private int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
    private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
    private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
    private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
    private int intCount = 0;
    private void Page_Load()
    {
        intCount = 0;
    }
    private void btnLoad1_Click(Object Sender, EventArgs e)
    {
        AppPages oAppPage = new AppPages(0, dsn);
        DataSet dsPages = oAppPage.Gets(Int32.Parse(txtApp.Text), 0, 0, 1);
        Applications oApplication = new Applications(0, dsn);
        DataSet ds = oApplication.Gets(1);
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            int intApplication = Int32.Parse(dr["applicationid"].ToString());
            if (oAppPage.Gets(intApplication, 0, 0, 1).Tables[0].Rows.Count == 0)
            {
                foreach (DataRow drPage in dsPages.Tables[0].Rows)
                    oAppPage.Add(Int32.Parse(drPage["pageid"].ToString()), intApplication);
            }
            intCount++;
        }
        Response.Write("Count = " + intCount.ToString() + "<br>");
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
                <td colspan="2" class="header">Reset Page Permissions</td>
            </tr>
            <tr>
                <td colspan="2">&nbsp;</td>
            </tr>
            <tr>
                <td>Application:</td>
                <td><asp:TextBox ID="txtApp" runat="server" CssClass="default" Width="100" /></td>
            </tr>
            <tr>
                <td colspan="2">&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td><asp:Button ID="btnLoad1" runat="server" CssClass="default" Width="150" Text="Run" OnClick="btnLoad1_Click" /></td>
            </tr>
        </table>
</form>
</body>
</html>