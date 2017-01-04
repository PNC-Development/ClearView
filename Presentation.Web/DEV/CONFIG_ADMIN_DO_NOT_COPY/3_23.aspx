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
    private void Page_Load()
    {
        intCount = 0;
        intError = 0;
    }
    private void btnLoad1_Click(Object Sender, EventArgs e)
    {
        OnDemandTasks oOnDemandTasks = new OnDemandTasks(0, dsn);
        DataSet ds = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "select * from cv_ondemand_tasks_physical_ii where deleted = 0");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            int intStep = 1;
            if (dr["chk15"].ToString() == "1")
                intStep = 999;
            else if (dr["chk14"].ToString() == "1" || dr["chk13"].ToString() == "1")
                intStep = 7;
            else if (dr["chk12"].ToString() == "1")
                intStep = 6;
            else if (dr["chk7"].ToString() == "1" || dr["chk8"].ToString() == "1" || dr["chk9"].ToString() == "1" || dr["chk10"].ToString() == "1" || dr["chk11"].ToString() == "1")
                intStep = 5;
            else if (dr["chk6"].ToString() == "1")
                intStep = 4;
            else if (dr["chk5"].ToString() == "1")
                intStep = 3;
            else if (dr["chk4"].ToString() == "1" || dr["chk3"].ToString() == "1" || dr["chk1"].ToString() == "1")
                intStep = 2;
            oOnDemandTasks.AddII(Int32.Parse(dr["requestid"].ToString()), Int32.Parse(dr["itemid"].ToString()), Int32.Parse(dr["number"].ToString()), Int32.Parse(dr["answerid"].ToString()), Int32.Parse(dr["modelid"].ToString()));
            oOnDemandTasks.UpdateIIStep(Int32.Parse(dr["requestid"].ToString()), Int32.Parse(dr["itemid"].ToString()), Int32.Parse(dr["number"].ToString()), intStep);
            intCount++;
        }
        Response.Write("Done = " + intCount.ToString() + "<br/>");
        Response.Write("Error = " + intError.ToString() + "<br/>");
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
                <td><asp:Button ID="btnLoad1" runat="server" CssClass="default" Width="150" Text="Merge II Workflow" OnClick="btnLoad1_Click" /></td>
            </tr>
        </table>
</form>
</body>
</html>