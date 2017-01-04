<%@ Page Language="C#" %>
<%@ Import Namespace="Microsoft.ApplicationBlocks.Data" %>
<script runat="server">
    private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
    private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
    private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
    private string dsnServiceEditor = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ServiceEditorDSN"]].ConnectionString;
    private int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
    protected int intImplementorDistributed = Int32.Parse(ConfigurationManager.AppSettings["ITEMID_IMPLEMENTOR_DISTRIBUTED"]);
    protected int intImplementorMidrange = Int32.Parse(ConfigurationManager.AppSettings["ITEMID_IMPLEMENTOR_MIDRANGE"]);
    protected int intWorkstationPlatform = Int32.Parse(ConfigurationManager.AppSettings["WorkstationPlatformID"]);
    protected int intUnder48A = Int32.Parse(ConfigurationManager.AppSettings["DR_HOUR_RESPONSE"]);
    protected int intUnder48Q = Int32.Parse(ConfigurationManager.AppSettings["DR_HOUR_QUESTION"]);
    protected int intAssignPage = Int32.Parse(ConfigurationManager.AppSettings["AssignWorkload"]);
    protected int intViewPage = Int32.Parse(ConfigurationManager.AppSettings["ViewWorkload"]);
    protected int intServiceStorage = Int32.Parse(ConfigurationManager.AppSettings["SERVICEID_SERVER_DECOMMISSION_STORAGE"]);
    protected int intCount = 0;
    protected int intCount2 = 0;
    protected void Page_Load(Object Sender, EventArgs e)
    {
        //intCount = 0;
        //intCount2 = 0;
        //DateTime _now = DateTime.Now;
        //Response.Write("ToUniversalTime = " + _now.ToUniversalTime() + "<br/>");
        //Response.Write("ToBinary = " + _now.ToBinary() + "<br/>");
        //Response.Write("ToFileTime = " + _now.ToFileTime() + "<br/>");
        //Response.Write("ToFileTimeUtc = " + _now.ToFileTimeUtc() + "<br/>");
        //Response.Write("ToLocalTime = " + _now.ToLocalTime() + "<br/>");
        //Response.Write("ToOADate = " + _now.ToOADate() + "<br/>");
        //Response.Write("ToString = " + _now.ToString("yyyyMMdd") + "<br/>");

        /*
        VMWare oVMWare = new VMWare(0, dsn);
        //string strConnect = oVMWare.ConnectDEBUG("https://wcsvt300a/sdk", 999, "PNC");
        string strConnect = oVMWare.ConnectDEBUG("https://ohcleutl4001/sdk", 3, "Dalton");
        if (strConnect == "")
        {
            VimService _service = oVMWare.GetService();
            ServiceContent _sic = oVMWare.GetSic();
            ManagedObjectReference datacenterRef = oVMWare.GetDataCenter();
            ManagedObjectReference vmFolderRef = oVMWare.GetVMFolder(datacenterRef);
            ManagedObjectReference[] oDatastores = oVMWare.GetDatastores("ohcinxcv4007");
            foreach (ManagedObjectReference oDataStore in oDatastores)
            {
                DatastoreSummary oSummary = (DatastoreSummary)oVMWare.getObjectProperty(oDataStore, "summary");
                try
                {
                    Permission[] oPermission = (Permission[])oVMWare.getObjectProperty(oDataStore, "permission");
                    Response.Write(oSummary.name + " = OK (" + oPermission.Length + ")" + "<br/>");
                }
                catch (Exception ex)
                {
                    Response.Write(oSummary.name + " = ERROR (" + ex.Message + ")" + "<br/>");
                }
            }
            // Logout
            if (_service != null)
            {
                _service.Abort();
                if (_service.Container != null)
                    _service.Container.Dispose();
                try
                {
                    _service.Logout(_sic.sessionManager);
                }
                catch { }
                _service.Dispose();
                _service = null;
                _sic = null;
            }
        }
        else
            Response.Write("LOGIN error");
        */
    }
    protected void btnLoad1_Click(Object Sender, EventArgs e)
    {
        DataSet ds = SqlHelper.ExecuteDataset(dsnAsset, CommandType.StoredProcedure, "pr_456");
        ResourceRequest oResourceRequest = new ResourceRequest(0, dsn);
        Services oService = new Services(0, dsn);
        ServiceDetails oServiceDetail = new ServiceDetails(0, dsn);
        ServiceRequests oServiceRequest = new ServiceRequests(0, dsn);
        int intCounter = 0;
        int intUntil = 0;
        Int32.TryParse(Request.QueryString["count"], out intUntil);
        if (intUntil == 0)
            intUntil = 100;
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            intCounter++;
            if (intCounter <= intUntil)
            {
                int intRequest = Int32.Parse(dr["requestid"].ToString());
                int intNumber = Int32.Parse(dr["number"].ToString());
                if (oResourceRequest.GetAllService(intRequest, intServiceStorage, intNumber).Tables[0].Rows.Count == 0)
                {
                    int intServiceItemId = oService.GetItemId(intServiceStorage);
                    double dblServiceHours = oServiceDetail.GetHours(intServiceStorage, 1);
                    int intResource = oServiceRequest.AddRequest(intRequest, intServiceItemId, intServiceStorage, 1, dblServiceHours, 2, intNumber, dsnServiceEditor);
                    oServiceRequest.NotifyTeamLead(intServiceItemId, intResource, intAssignPage, intViewPage, intEnvironment, "", dsnServiceEditor, dsnAsset, dsnIP, 0);
                    intCount++;
                }
                else
                    intCount2++;
            }
        }
        Response.Write("Done = " + intCount.ToString() + "<br/>");
        Response.Write("Duplicate = " + intCount2.ToString() + "<br/>");
        Response.Write("Until = " + intUntil.ToString() + "<br/>");
    }
    protected void btnLoad2_Click(Object Sender, EventArgs e)
    {
        ResourceRequest oResourceRequest = new ResourceRequest(0, dsn);
        ServiceRequests oServiceRequest = new ServiceRequests(0, dsn);
        Functions oFunction = new Functions(0, dsn, intEnvironment);
        string strDone = "";
        DataSet ds = SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "xxxStorage");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            int intRequest = Int32.Parse(dr["requestid"].ToString());
            int intService = 832;
            int intNumber = Int32.Parse(dr["number"].ToString());
            DateTime datCutoff = DateTime.Parse("11/12/2010");
            DateTime datDestroy = DateTime.Parse(dr["destroyed"].ToString());
            int intServer = Int32.Parse(dr["serverid"].ToString());
            int intAnswer = Int32.Parse(dr["answerid"].ToString());
            string strName = dr["servername"].ToString();
            if (datDestroy < datCutoff)
            {
                int intAssigned = 15507;
                int intUser = 47687;
                DateTime datAssigned = datDestroy.AddMinutes(69.00);
                DateTime datCompleted = datDestroy.AddMinutes(1027.00);

                // Resource Request
                int intRR = oResourceRequest.Add(intRequest, 37, intService, intNumber, "", 1, 8.00, 3, 1, 1, 1);
                SqlHelper.ExecuteNonQuery(dsn, CommandType.Text, "UPDATE cv_resource_requests SET assignedby = " + intAssigned.ToString() + ", assigned = '" + datAssigned.ToString() + "', created = '" + datDestroy.ToString() + "' WHERE id = " + intRR.ToString());
                // Resource Request Workflow
                int intRRW = oResourceRequest.AddWorkflow(intRR, 0, "", intUser, 1, 8.00, 3, 0);
                SqlHelper.ExecuteNonQuery(dsn, CommandType.Text, "UPDATE cv_resource_requests_workflow SET used = 8.00, modifiedby = " + intUser.ToString() + ", created = '" + datDestroy.ToString() + "', modified = '" + datCompleted.ToString() + "', completed = '" + datCompleted.ToString() + "' WHERE id = " + intRRW.ToString());
                // Resource Request Workflow Hours
                oResourceRequest.UpdateWorkflowHours(intRRW);
                SqlHelper.ExecuteNonQuery(dsn, CommandType.Text, "UPDATE cv_resource_requests_hours SET modified = '" + datCompleted.ToString() + "' WHERE parent = " + intRRW.ToString());
                // Servers
                SqlHelper.ExecuteNonQuery(dsn, CommandType.Text, "UPDATE cv_servers SET reclaimed_storage = 1 WHERE id = " + intServer.ToString());
                SqlHelper.ExecuteNonQuery(dsn, CommandType.Text, "UPDATE cv_resource_requests SET modified = '" + datCompleted.ToString() + "', completed = '" + datCompleted.ToString() + "' WHERE id = " + intRR.ToString());
                intCount++;
                strDone += "Server = " + strName + ", Decom = " + datDestroy.ToString() + ", RR = " + intRR.ToString() + ", RRW = " + intRRW.ToString() + Environment.NewLine;
            }
            else
            {
                int intResource = oServiceRequest.AddRequest(intRequest, 37, intService, 1, 8.00, 2, intNumber, dsnServiceEditor);
                SqlHelper.ExecuteNonQuery(dsn, CommandType.Text, "UPDATE cv_resource_requests SET number = " + intNumber.ToString() + " WHERE id = " + intResource.ToString());
                oServiceRequest.NotifyTeamLead(37, intResource, intAssignPage, intViewPage, intEnvironment, "", dsnServiceEditor, dsnAsset, dsnIP, 0);
                SqlHelper.ExecuteNonQuery(dsnAsset, CommandType.Text, "UPDATE cva_decommissions SET running = 2 WHERE name = '" + strName + "'");
                intCount++;
                strDone += "Server = " + strName + ", Decom = " + datDestroy.ToString() + ", RR = " + intResource.ToString() + ", Running = 2 WHERE name = '" + strName + "'" + Environment.NewLine;
            }
        }
        Response.Write(intCount.ToString());
        oFunction.SendEmail("Decom Fixes", "stephen.healy@pnc.com", "", "", "Decom Fixes", strDone, false, true);
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
                <td><asp:Button ID="btnLoad1" runat="server" CssClass="default" Width="150" Text="Storage Fix (1)" Enabled="false" OnClick="btnLoad1_Click" /></td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td><asp:Button ID="btnLoad2" runat="server" CssClass="default" Width="150" Text="Storage Fix (2)" Enabled="true" OnClick="btnLoad2_Click" /></td>
            </tr>
        </table>
</form>
</body>
</html>