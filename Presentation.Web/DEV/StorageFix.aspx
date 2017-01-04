<%@ Page Language="C#" %>
<%@ Import Namespace="Microsoft.ApplicationBlocks.Data" %>
<script runat="server">
    private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
    private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
    private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
    private string dsnServiceEditor = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ServiceEditorDSN"]].ConnectionString;
    private string dsnZeus = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ZeusDSN"]].ConnectionString;
    private int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
    protected int intAssignPage = Int32.Parse(ConfigurationManager.AppSettings["AssignWorkload"]);
    protected int intViewPage = Int32.Parse(ConfigurationManager.AppSettings["ViewWorkload"]);
    protected int intTotal = 0;

    protected void Page_Load(Object Sender, EventArgs e)
    {
        Int32.TryParse(Request.QueryString["t"], out intTotal);
        if (!IsPostBack)
        {
            if (intTotal == 0)
            {
                Response.Write("Missing T querystring paramater (for how many records to process)...");
                btnGo.Enabled = false;
            }
            else
                Response.Write("Will execute " + intTotal.ToString() + " records...<br/>");
        }
    }
    protected void btnGo_Click(Object Sender, EventArgs e)
    {
        Services oService = new Services(0, dsn);
        ServiceDetails oServiceDetail = new ServiceDetails(0, dsn);
        ServiceRequests oServiceRequest = new ServiceRequests(0, dsn);
        ResourceRequest oResourceRequest = new ResourceRequest(0, dsn);
        int intCount = 0;
        
        DataSet ds = SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "xxxStorage");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            if (intCount >= intTotal)
                break;
            else
            {
                string strName = dr["servername"].ToString();
                int intRequest = Int32.Parse(dr["requestid"].ToString());
                int intService = 832;
                int intNumber = Int32.Parse(dr["number"].ToString());
                DateTime datDestroyed = DateTime.Parse(dr["destroyed"].ToString());
                if (oResourceRequest.GetAllService(intRequest, intService, intNumber).Tables[0].Rows.Count == 0)
                {
                    int intServiceItemId = oService.GetItemId(intService);
                    double dblServiceHours = oServiceDetail.GetHours(intService, 1);
                    int intResource = oServiceRequest.AddRequest(intRequest, intServiceItemId, intService, 1, dblServiceHours, 2, intNumber, dsnServiceEditor);
                    oServiceRequest.NotifyTeamLead(intServiceItemId, intResource, intAssignPage, intViewPage, intEnvironment, "", dsnServiceEditor, dsnAsset, dsnIP, 0);
                    Response.Write("Sent notification for " + strName + ", destroyed on " + datDestroyed.ToString() + "<br/>");
                    intCount++;
                }
            }
        }
        Response.Write("intCount = " + intCount.ToString() + "<br/>");
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
                <td><asp:Button ID="btnGo" runat="server" CssClass="default" Width="150" Text="Go" OnClick="btnGo_Click" /></td>
            </tr>
        </table>
</form>
</body>
</html>