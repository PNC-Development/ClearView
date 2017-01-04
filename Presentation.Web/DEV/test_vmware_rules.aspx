<%@ Page Language="C#" Debug="false" EnableEventValidation="false" ValidateRequest="false" MasterPageFile="~/clearview.master" %>
<script runat="server">
    private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
    private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
    private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
    private string dsnZeus = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ZeusDSN"]].ConnectionString;
    private int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
    private int intVMWare = Int32.Parse(ConfigurationManager.AppSettings["VMWareModelID"]);
    private VMWare oVMWare;
    private void Page_Load()
    {
        string strError = "";
        Servers oServer = new Servers(0, dsn);
        OnDemand oOnDemand = new OnDemand(0, dsn);
        IPAddresses oIPAddresses = new IPAddresses(0, dsnIP, dsn);
        oVMWare = new VMWare(0, dsn);
        Functions oFunction = new Functions(0, dsn, intEnvironment);
        string strConnect = oVMWare.ConnectDEBUG("https://wcsvt300a/sdk", 4, "PNC");
        VimService _service = oVMWare.GetService();
        ManagedObjectReference clusterRef = oVMWare.GetCluster("ohclexcv100a");
        ClusterAntiAffinityRuleSpec oAff = new ClusterAntiAffinityRuleSpec();
        string strServers = "LCETH303A-LCETH304A";
        char[] strSplit = { '-' };
        string[] strServer = strServers.Split(strSplit);
        ManagedObjectReference[] vmRefs = new ManagedObjectReference[strServer.Length];
        for (int ii = 0; ii < strServer.Length; ii++)
        {
            if (strServer[ii].Trim() != "")
            {
                Response.Write("server # " + ii.ToString() + " = " + strServer[ii].Trim() + "<br/>");
                vmRefs[ii] = oVMWare.GetVM(strServer[ii].Trim());
                Response.Write("vmRefs # " + ii.ToString() + " = " + vmRefs[ii].ToString() + "<br/>");
            }
        }
        oAff.name = strServers;
        ClusterRuleSpec oRule = new ClusterRuleSpec();
        ArrayUpdateOperation oOperation = ArrayUpdateOperation.add;
        ClusterConfigInfo cinfo = (ClusterConfigInfo)oVMWare.getObjectProperty(clusterRef, "configuration");
        int intKey = 1;
        if (cinfo.rule != null)
        {
            ClusterRuleInfo[] rules = cinfo.rule;
            for (int ii = 0; ii < rules.Length; ii++)
            {
                if (rules[ii].key > intKey)
                    intKey = rules[ii].key;
                //ManagedObjectReference[] vmList = (ManagedObjectReference[])oVMWare.getObjectProperty(rules[ii], "vm");
                //foreach (ManagedObjectReference vmCheck in vmList) 
                //{
                //    Response.Write("RULE " + rules[ii].name + " = " + vmCheck.Value + "<br/>");
                ////    foreach (ManagedObjectReference vmCheck2 in vmRefs)
                ////    {
                ////    }
                //}
            }
        }
        intKey++;
        Response.Write("key = " + intKey.ToString() + "<br/>");
        oAff.key = intKey;
        oAff.keySpecified = true;
        oAff.enabled = true;
        oAff.enabledSpecified = true;
//        oAff.vm = vmRefs;

        oRule.operation = oOperation;
        oRule.info = oAff;
        ClusterConfigSpec oSpec = new ClusterConfigSpec();
        oSpec.rulesSpec = new ClusterRuleSpec[] { oRule };
        oAff.vm = vmRefs;
        ManagedObjectReference _task = _service.ReconfigureCluster_Task(clusterRef, oSpec, true);
        TaskInfo oInfo = (TaskInfo)oVMWare.getObjectProperty(_task, "info");
        while (oInfo.state == TaskInfoState.running)
            oInfo = (TaskInfo)oVMWare.getObjectProperty(_task, "info");
        if (oInfo.state == TaskInfoState.success)
            Response.Write("SUCCESS<br/>");
        else
            Response.Write("ERROR<br/>");
        Response.Write(strConnect);
        if (_service != null)
        {
            ServiceContent _sic = oVMWare.GetSic();
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
</script>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" Runat="Server">
<script type="text/javascript">
</script>
done!
</asp:Content>