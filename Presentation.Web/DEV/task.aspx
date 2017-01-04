<%@ Page Language="C#" %>
<%@ Import Namespace="System.Data" %>
<script runat="server">
    private int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
    private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
    private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
    private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
    private string dsnServiceEditor = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ServiceEditorDSN"]].ConnectionString;
    protected int intAssignPage = Int32.Parse(ConfigurationManager.AppSettings["AssignWorkload"]);
    protected int intViewPage = Int32.Parse(ConfigurationManager.AppSettings["ViewWorkload"]);
    private int intCount = 0;
    private void Page_Load()
    {
    }
    private void btnGo_Click(Object Sender, EventArgs e)
    {
        Forecast oForecast = new Forecast(0, dsn);
        PNCTasks oPNCTask = new PNCTasks(0, dsn);
        
        int intServiceID = Int32.Parse(txtService.Text);
        DataSet ds = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT TOP 1 * FROM cv_ondemand_tasks_server_other WHERE (serviceid = " + intServiceID.ToString() + ") AND (deleted = 0) AND (completed IS NULL) AND (answerid IN (SELECT id FROM cv_forecast_answers WHERE (executed IS NOT NULL) AND (completed IS NOT NULL) AND (finished IS NULL) AND (deleted = 0)))");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            int intAnswer = Int32.Parse(dr["answerid"].ToString());
            int intRequest = oForecast.GetRequestID(intAnswer, true);
            int intModel = oForecast.GetModelAsset(intAnswer);
            if (intModel == 0)
                intModel = oForecast.GetModel(intAnswer);
            oPNCTask.InitiateNextStep(intRequest, intAnswer, intModel, intEnvironment, intAssignPage, intViewPage, dsnAsset, dsnIP, dsnServiceEditor);
        }
    }
</script>
<script type="text/javascript">
</script>
<html>
    <head>
        <title>After Removing a Pre-Prod Task, run this to cleanup and send Birth Certificates, tasks, etc...</title>
        <link rel="stylesheet" type="text/css" href="/css/default.css" />
    </head>
    <body leftmargin="0" topmargin="0">
        <form id="Form1" runat="server">
            <table>
                <tr>
                    <td>ServiceID:</td>
                    <td><asp:TextBox ID="txtService" runat="server" CssClass="default" /></td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td><asp:Button ID="btnGo" runat="server" CssClass="default" Width="150" Text="Execute" OnClick="btnGo_Click" /></td>
                </tr>
            </table>
        </form>
    </body>
</html>