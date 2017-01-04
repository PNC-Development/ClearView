<%@ Page Language="C#" Debug="false" %>
<%@ Import Namespace="System.Data.OleDb" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System" %>
<script runat="server">
    private int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
    private int intCount = 0;
    private string strError = "";
    private void Page_Load()
    {
        intCount = 0;
    }
    private void btnLoad1_Click(Object Sender, EventArgs e)
    {
        string dsnTest = ConfigurationManager.ConnectionStrings["testDSN"].ConnectionString;
        ServerName oServerName = new ServerName(0, dsnTest);
        DataSet ds = SqlHelper.ExecuteDataset(dsnTest, CommandType.Text, "SELECT * FROM cv_zeus_builds");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            DataSet dsCheck = SqlHelper.ExecuteDataset(dsnTest, CommandType.Text, "SELECT * FROM cv_servernames WHERE prefix1 = '" + dr["prefix1"].ToString() + "' AND prefix2 = '" + dr["prefix2"].ToString() + "' AND sitecode = '" + dr["sitecode"].ToString() + "' AND name1 = '" + dr["name1"].ToString() + "' AND name2 = '" + dr["name2"].ToString() + "' AND deleted = 0");
            if (dsCheck.Tables[0].Rows.Count == 0)
            {
                oServerName.Add(Int32.Parse(dr["codeid"].ToString()), dr["prefix1"].ToString(), dr["prefix2"].ToString(), dr["sitecode"].ToString(), dr["name1"].ToString(), dr["name2"].ToString(), Int32.Parse(dr["userid"].ToString()), dr["name"].ToString(), Int32.Parse(dr["available"].ToString()));
                intCount++;
            }
        }
        Response.Write("Done = " + intCount.ToString());
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