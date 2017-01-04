<%@ Page Language="C#" %>
<%@ Import Namespace="PAObjectsLib" %>
<script runat="server">
    private int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
    private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
    private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
    private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
    private string dsnServiceEditor = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ServiceEditorDSN"]].ConnectionString;

    protected int intViewPage = Int32.Parse(ConfigurationManager.AppSettings["ViewWorkload"]);
    protected int intAssignPage = Int32.Parse(ConfigurationManager.AppSettings["AssignWorkload"]);
    protected int intIMDecommServiceId = Int32.Parse(ConfigurationManager.AppSettings["SERVICEID_SERVER_DECOMMISSION_IM"]); 
    
    private Variables oVariable;

    protected void Page_Load(object sender, EventArgs e)
    {
        oVariable = new Variables(intEnvironment);
    }
    protected void btnGo_Click(object sender, EventArgs e)
    {
        Forecast oForecast = new Forecast(0, dsn);
        ServerDecommission oServerDecommission = new ServerDecommission(0, dsn);
        string strDSMADMC = "";
        
        DataSet ds = SqlHelper.ExecuteDataset(dsn, CommandType.Text, txtScript.Text);
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            int intServer = Int32.Parse(dr["serverid"].ToString());
            int intModel = Int32.Parse(dr["modelid"].ToString());
            string strName = dr["name"].ToString();
            int intRequest = Int32.Parse(dr["requestid"].ToString());
            int intItem = Int32.Parse(dr["itemid"].ToString());
            int intNumber = Int32.Parse(dr["number"].ToString());
            int intAnswer = Int32.Parse(dr["answerid"].ToString());
            bool boolStorage = false;
            bool boolLTM = false;
            if (intAnswer > 0)
            {
                boolStorage = (oForecast.GetAnswer(intAnswer, "storage") == "1");
                boolLTM = oForecast.IsHACSM(intAnswer);
            }

            Response.Write(strName.ToString() + "...");
            if (radDecomTasks.Checked)
            {
                oServerDecommission.InitiateDecom(intServer, intModel, strName, intRequest, intItem, intNumber, (boolStorage ? 1 : 0), (boolLTM ? 1 : 0),
                                    intAssignPage, intViewPage, intEnvironment,
                                    intIMDecommServiceId,
                                    dsnServiceEditor, dsnAsset, dsnIP, strDSMADMC);
            }
            else
            {
                oServerDecommission.InitiateAssetReDeployOrDisposeServiceRequest(intServer, intRequest, intNumber, dsnAsset, dsnServiceEditor, intEnvironment, dsnIP, intAssignPage, intViewPage);
            }
            Response.Write("Done<br/>");
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
        <p><asp:TextBox ID="txtScript" runat="server" TextMode="MultiLine" Width="90%" Rows="20" /></p>
        <p><asp:Button ID="btnGo" runat="server" Text="Go" OnClick="btnGo_Click" Width="75" /></p>
        <p>
            <asp:RadioButton ID="radDecomTasks" runat="server" Text="Decom Tasks" GroupName="Decom" /><br />
            <asp:RadioButton ID="radDecomDispose" runat="server" Text="Dispose Tasks" GroupName="Decom" /><br />
        </p>
    </div>
    </form>
</body>
</html>