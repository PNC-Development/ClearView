<%@ Page Language="C#" %>
<%@ Import Namespace="PAObjectsLib" %>
<script runat="server">
    private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
    private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
    private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
    private string dsnServiceEditor = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ServiceEditorDSN"]].ConnectionString;
    private int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
    protected int intAssignPage = Int32.Parse(ConfigurationManager.AppSettings["AssignWorkload"]);
    protected int intViewPage = Int32.Parse(ConfigurationManager.AppSettings["ViewWorkload"]);
    private Variables oVariable;

    protected void Page_Load(object sender, EventArgs e)
    {
        oVariable = new Variables(intEnvironment);
    }
    protected void btnGo_Click(object sender, EventArgs e)
    {
        ServerDecommission oDecom = new ServerDecommission(0, dsn);
        String[] Servers = txtServers.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
        foreach (string Server in Servers)
        {
            Response.Write(Server + "...");
            try
            {
                DataSet ds = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "select * from cv_WM_decommission_server where servername = '" + Server + "' and deleted = 0");
                if (ds.Tables[0].Rows.Count == 1)
                {
                    int ServerID = Int32.Parse(ds.Tables[0].Rows[0]["serverid"].ToString());
                    int RequestID = Int32.Parse(ds.Tables[0].Rows[0]["requestid"].ToString());
                    int ItemID = Int32.Parse(ds.Tables[0].Rows[0]["itemid"].ToString());
                    int Number = Int32.Parse(ds.Tables[0].Rows[0]["number"].ToString());
                    oDecom.InitiateAssetReDeployOrDisposeServiceRequest(ServerID, RequestID, Number, dsnAsset, dsnServiceEditor, intEnvironment, dsnIP, intAssignPage, intViewPage);
                    Response.Write("Done!");
                }
                else
                    Response.Write("Multiple Records = "+ ds.Tables[0].Rows.Count.ToString());
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
                if (ex.InnerException != null)
                    Response.Write(" (" + ex.InnerException.Message + ")");
            }
            Response.Write("<br/>");
        }
    }

</script>
<script type="text/javascript">
</script>
<html>
<head>
<title>LOAD</title>
<link rel="stylesheet" type="text/css" href="/css/default.css" />
<script src="/javascript/default.js"type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:TextBox ID="txtServers" runat="server" TextMode="MultiLine" Rows="10" Width="600" />
        <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="btnGo_Click" />
    </div>
    </form>
</body>
</html>