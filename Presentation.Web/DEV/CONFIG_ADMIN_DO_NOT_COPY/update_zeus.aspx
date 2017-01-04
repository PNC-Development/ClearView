<%@ Page Language="C#" Debug="false" %>
<%@ Import Namespace="System.Data.OleDb" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System" %>
<script runat="server">
    private string dsnZeus = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ZeusDSN"]].ConnectionString;
    private void Page_Load()
    {
    }
    private void btnLoad1_Click(Object Sender, EventArgs e)
    {
        DataSet ds = SqlHelper.ExecuteDataset(dsnZeus, CommandType.Text, "SELECT * FROM cv_zeus_builds WHERE id > 1188");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            string strMAC = dr["macaddress"].ToString();
            while (strMAC.Contains(":") == true)
                strMAC = strMAC.Replace(":", "");
            SqlHelper.ExecuteNonQuery(dsnZeus, CommandType.Text, "UPDATE cv_zeus_builds SET ipaddress = '" + strMAC + "' WHERE id = " + dr["id"].ToString());
        }
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
                <td colspan="2" class="header">Import Servers</td>
            </tr>
            <tr>
                <td colspan="2">&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td><asp:Button ID="btnLoad1" runat="server" CssClass="default" Width="150" Text="Load" OnClick="btnLoad1_Click" /></td>
            </tr>
        </table>
</form>
</body>
</html>