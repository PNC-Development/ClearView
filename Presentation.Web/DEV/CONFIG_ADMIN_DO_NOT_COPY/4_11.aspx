<%@ Page Language="C#" Debug="false" %>
<%@ Import Namespace="System.Data.OleDb" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System" %>
<script runat="server">
    private int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
    private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
    private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
    private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
    private int intDNSService = Int32.Parse(ConfigurationManager.AppSettings["SERVICEID_PNC_DNS"]);
    private int intSecurityService = Int32.Parse(ConfigurationManager.AppSettings["SERVICEID_PNC_SECURITY"]);
    private int intCount = 0;
    private int intError = 0;
    private void Page_Load()
    {
        intCount = 0;
        intError = 0;
    }
    private void btnLoad1_Click(Object Sender, EventArgs e)
    {
        DataSet ds = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "select * from cv_ondemand_tasks_server_pnc_dns");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            SqlHelper.ExecuteNonQuery(dsn, CommandType.Text, "INSERT INTO cv_ondemand_tasks_server_other VALUES (" + dr["requestid"].ToString() + ", " + intDNSService.ToString() + ", " + dr["number"].ToString() + ", " + dr["answerid"].ToString() + ", " + dr["modelid"].ToString() + ", " + dr["chk1"].ToString() + ", " + (dr["modified"].ToString() == "" ? "null" : "'" + dr["modified"].ToString() + "'") + ", " + (dr["completed"].ToString() == "" ? "null" : "'" + dr["completed"].ToString() + "'") + ", " + dr["deleted"].ToString() + ")");
            intCount++;
        }
        ds = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "select * from cv_ondemand_tasks_server_pnc_security");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            SqlHelper.ExecuteNonQuery(dsn, CommandType.Text, "INSERT INTO cv_ondemand_tasks_server_other VALUES (" + dr["requestid"].ToString() + ", " + intSecurityService.ToString() + ", " + dr["number"].ToString() + ", " + dr["answerid"].ToString() + ", " + dr["modelid"].ToString() + ", " + dr["chk1"].ToString() + ", " + (dr["modified"].ToString() == "" ? "null" : "'" + dr["modified"].ToString() + "'") + ", " + (dr["completed"].ToString() == "" ? "null" : "'" + dr["completed"].ToString() + "'") + ", " + dr["deleted"].ToString() + ")");
            intCount++;
        }
        Response.Write("Done = " + intCount.ToString() + "<br/>");
    }
    private void btnLoad2_Click(Object Sender, EventArgs e)
    {
        SqlHelper.ExecuteNonQuery(dsn, CommandType.Text, "UPDATE cv_resource_requests SET devices = 1 WHERE devices = 0");
        SqlHelper.ExecuteNonQuery(dsn, CommandType.Text, "UPDATE cv_resource_requests_workflow SET devices = 1 WHERE devices = 0");
        Response.Write("Done!!!<br/>");
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
                <td colspan="2" class="header">4.10</td>
            </tr>
            <tr>
                <td colspan="2">&nbsp;</td>
            </tr>
            <tr>
                <td colspan="2">Click this after adding cv_ondemand_tasks_server_other table and before deleting cv_ondemand_tasks_server_pnc_dns AND cv_ondemand_tasks_server_pnc_security tables</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td><asp:Button ID="btnLoad1" runat="server" CssClass="default" Width="150" Text="Tasks" OnClick="btnLoad1_Click" /></td>
            </tr>
            <tr>
                <td colspan="2">&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td><asp:Button ID="btnLoad2" runat="server" CssClass="default" Width="150" Text="RR Devices" OnClick="btnLoad2_Click" /></td>
            </tr>
        </table>
</form>
</body>
</html>