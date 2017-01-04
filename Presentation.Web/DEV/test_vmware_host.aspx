<%@ Page Language="C#" Debug="false" EnableEventValidation="false" ValidateRequest="false" MasterPageFile="~/clearview.master" %>
<%@ Import Namespace="System.Security.Cryptography.X509Certificates" %>
<script runat="server">
    private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
    private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
    private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
    private string dsnZeus = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ZeusDSN"]].ConnectionString;
    private int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
    private int intVMWare = Int32.Parse(ConfigurationManager.AppSettings["VMWareModelID"]);
    private VMWare oVMWare;
    private VimService _service;
    private ServiceContent _sic;
    private void Page_Load()
    {
        string strError = "";
        Servers oServer = new Servers(0, dsn);
        OnDemand oOnDemand = new OnDemand(0, dsn);
        IPAddresses oIPAddresses = new IPAddresses(0, dsnIP, dsn);
        oVMWare = new VMWare(0, dsn);
        //string strConnect = oVMWare.ConnectDEBUG("https://ohcleutl4001/sdk", 3, "Cleveland Ops");
        string strConnect = oVMWare.ConnectDEBUG("https://ohclesql4005/sdk", 3, "Test");
        if (strConnect == "")
        {
            _service = oVMWare.GetService();
            _sic = oVMWare.GetSic();
            ManagedObjectReference datacenterRef = oVMWare.GetDataCenter();
            ManagedObjectReference hostFolderRef = (ManagedObjectReference)oVMWare.getObjectProperty(datacenterRef, "hostFolder");
            ManagedObjectReference rootFolder = (ManagedObjectReference)oVMWare.getObjectProperty(datacenterRef, "parent");
            int intCluster = 1;
            //string strCluster = oVMWare.GetCluster(intCluster, "name");
            //string strCluster = "ohclexcv4004";
            string strCluster = "test";
            string strHostName = "10.34.34.243";
            string strHostIP = "10.34.34.243";
            string strHostIP2 = "10.34.34.242";
            string strHostIP2Gateway = "10.34.34.1";
            string[] nics1 = { "vmnic0", "vmnic1" };
            string[] nics2 = { "vmnic2", "vmnic3" };
            bool boolError = false;
            
            ManagedObjectReference clusterRef = oVMWare.GetCluster(strCluster);
            ManagedObjectReference resourcePoolRootRef = (ManagedObjectReference)oVMWare.getObjectProperty(clusterRef, "resourcePool");
            // ****** 1st thing - Add Host (BEGIN)
            HostConnectSpec oSpec = new HostConnectSpec();
            oSpec.force = true;
            oSpec.hostName = strHostName;
            oSpec.userName = "root";
            oSpec.password = "vm1234";
            ManagedObjectReference _task1 = _service.AddStandaloneHost_Task(hostFolderRef, oSpec, null, true);
            boolError = WaitForExit(_task1, "ERROR 1");
            // ****** 1st thing - Add Host (END)

            if (boolError == false)
            {
                ManagedObjectReference hostRefConfig = GetMOR(strHostName, "COMPUTERESOURCE");
                ManagedObjectReference[] hostRefs = (ManagedObjectReference[])oVMWare.getObjectProperty(hostRefConfig, "host");
                ManagedObjectReference hostRef = hostRefs[0];
                HostConfigManager hostConfig = (HostConfigManager)oVMWare.getObjectProperty(hostRef, "configManager");
                HostListSummary hostSummary = (HostListSummary)oVMWare.getObjectProperty(hostRef, "summary");
                ManagedObjectReference networkSystem = hostConfig.networkSystem;
                ManagedObjectReference vmotionSystem = hostConfig.vmotionSystem;
                HostVirtualSwitchBeaconConfig beaconConfig = new HostVirtualSwitchBeaconConfig();
                beaconConfig.interval = 1;
                if (hostSummary.runtime.inMaintenanceMode == false)
                {
                    ManagedObjectReference _task0 = _service.EnterMaintenanceMode_Task(hostRef, 0, false, false);
                    boolError = WaitForExit(_task0, "ERROR 2");
                }

                // ****** 1st thing - remove VM Network Port Group (BEGIN)
                try
                {
                    _service.RemovePortGroup(networkSystem, "VM Network");
                }
                catch { }
                // ****** 1st thing - remove VM Network Port Group (END)

                if (boolError == false)
                {
                    // ****** 2nd thing - reconfigure vSwitch0 to add the vmNic1 and teaming (BEGIN)
                    HostVirtualSwitchSpec vswitch0Spec = new HostVirtualSwitchSpec();
                    vswitch0Spec.numPorts = 64;
                    HostVirtualSwitchBondBridge vswitch0Bridge = new HostVirtualSwitchBondBridge();
                    vswitch0Bridge.beacon = beaconConfig;
                    vswitch0Bridge.nicDevice = nics1;
                    vswitch0Spec.bridge = vswitch0Bridge;
                    vswitch0Spec.policy = GetHostNetworkPolicy(nics1);
                    _service.UpdateVirtualSwitch(networkSystem, "vSwitch0", vswitch0Spec);
                    // ****** 2nd thing - reconfigure vSwitch0 to add the vmNic1 and teaming (END)
                }

                if (boolError == false)
                {
                    // ****** 3rd thing - Add Configure VMotion (BEGIN)
                    HostPortGroupSpec oHostPortGroupSpec = new HostPortGroupSpec();
                    oHostPortGroupSpec.name = "VMkernel";
                    oHostPortGroupSpec.vlanId = 0;
                    oHostPortGroupSpec.vswitchName = "vSwitch0";
                    oHostPortGroupSpec.policy = new HostNetworkPolicy();
                    _service.AddPortGroup(networkSystem, oHostPortGroupSpec);

                    HostVirtualNicSpec oHostVirtualNicSpec = new HostVirtualNicSpec();
                    HostIpConfig oHostIpConfig = new HostIpConfig();
                    oHostIpConfig.dhcp = false;
                    oHostIpConfig.ipAddress = strHostIP2;
                    oHostIpConfig.subnetMask = "255.255.255.0";
                    oHostVirtualNicSpec.ip = oHostIpConfig;
                    _service.AddVirtualNic(networkSystem, "VMkernel", oHostVirtualNicSpec);

                    HostNetworkInfo networkInfo = (HostNetworkInfo)oVMWare.getObjectProperty(networkSystem, "networkInfo");
                    HostVirtualNic[] hostVirtualNics = networkInfo.vnic;
                    HostVirtualNic hostVirtualNic = hostVirtualNics[0];
                    _service.SelectVnic(vmotionSystem, hostVirtualNic.device);
                    
                    HostIpRouteConfig oHostIpRouteConfig = new HostIpRouteConfig();
                    oHostIpRouteConfig.defaultGateway = strHostIP2Gateway;
                    _service.UpdateIpRouteConfig(networkSystem, oHostIpRouteConfig);
                    // ****** 3rd thing - Configure VMotion (END)
                }

                if (boolError == false)
                {
                    // ****** 4th thing - Add vSwitch1 (BEGIN)
                    HostVirtualSwitchSpec vswitch1Spec = new HostVirtualSwitchSpec();
                    vswitch1Spec.numPorts = 64;
                    HostVirtualSwitchBondBridge vswitch1Bridge = new HostVirtualSwitchBondBridge();
                    vswitch1Bridge.beacon = beaconConfig;
                    vswitch1Bridge.nicDevice = nics2;
                    vswitch1Spec.bridge = vswitch1Bridge;
                    vswitch1Spec.policy = GetHostNetworkPolicy(nics2);
                    _service.AddVirtualSwitch(networkSystem, "vSwitch1", vswitch1Spec);
                    // ****** 4th thing - Add vSwitch1 (END)
                }

                if (boolError == false)
                {
                    // ****** 5th thing - Add Port Groups to vSwitch1 (BEGIN)
                    DataSet ds = oVMWare.GetVlanAssociationsCluster(intCluster);
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        HostPortGroupSpec oHostPortGroupSpecSWITCH1 = new HostPortGroupSpec();
                        oHostPortGroupSpecSWITCH1.name = dr["name"].ToString();
                        oHostPortGroupSpecSWITCH1.vlanId = Int32.Parse(dr["vlan"].ToString());
                        oHostPortGroupSpecSWITCH1.vswitchName = "vSwitch1";
                        oHostPortGroupSpecSWITCH1.policy = GetHostNetworkPolicy(nics2);
                        _service.AddPortGroup(networkSystem, oHostPortGroupSpecSWITCH1);
                    }
                    // ****** 5th thing - Add Port Groups to vSwitch1 (END)
                }

                if (boolError == false)
                {
                    // ****** 6th thing - Pull out of maintenance mode and join cluster (BEGIN)
                    ManagedObjectReference _task10 = _service.MoveHostInto_Task(clusterRef, hostRef, resourcePoolRootRef);
                    boolError = WaitForExit(_task10, "ERROR 10");
                    HostListSummary hostSummaryEnd = (HostListSummary)oVMWare.getObjectProperty(hostRef, "summary");
                    if (hostSummaryEnd.runtime.inMaintenanceMode == true)
                    {
                        ManagedObjectReference _task11 = _service.ExitMaintenanceMode_Task(hostRef, 0);
                        boolError = WaitForExit(_task11, "ERROR 11");
                    }
                    // ****** 6th thing - Pull out of maintenance mode and join cluster (END)
                }
            }

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
    }
    public bool WaitForExit(ManagedObjectReference _task, string _error)
    {
        bool boolError = false;
        TaskInfo _info = (TaskInfo)oVMWare.getObjectProperty(_task, "info");
        while (_info.state == TaskInfoState.running)
            _info = (TaskInfo)oVMWare.getObjectProperty(_task, "info");
        if (_info.state == TaskInfoState.error)
        {
            Response.Write(_error);
            boolError = true;
        }
        return boolError;
    }

    public HostNetworkPolicy GetHostNetworkPolicy(string[] _nics)
    {
        HostNetworkPolicy networkPolicy = new HostNetworkPolicy();
        HostNicOrderPolicy oHostNicOrderPolicy = new HostNicOrderPolicy();
        oHostNicOrderPolicy.activeNic = _nics;
        HostNicFailureCriteria oHostNicFailureCriteria = new HostNicFailureCriteria();
        oHostNicFailureCriteria.checkBeacon = false;
        oHostNicFailureCriteria.checkBeaconSpecified = true;
        oHostNicFailureCriteria.checkDuplex = false;
        oHostNicFailureCriteria.checkDuplexSpecified = true;
        oHostNicFailureCriteria.checkErrorPercent = false;
        oHostNicFailureCriteria.checkErrorPercentSpecified = true;
        oHostNicFailureCriteria.checkSpeed = "minimum";
        oHostNicFailureCriteria.fullDuplex = false;
        oHostNicFailureCriteria.fullDuplexSpecified = true;
        oHostNicFailureCriteria.percentage = 0;
        oHostNicFailureCriteria.percentageSpecified = true;
        oHostNicFailureCriteria.speed = 10;
        oHostNicFailureCriteria.speedSpecified = true;
        HostNicTeamingPolicy oHostNicTeamingPolicy = new HostNicTeamingPolicy();
        oHostNicTeamingPolicy.notifySwitches = true;
        oHostNicTeamingPolicy.notifySwitchesSpecified = true;
        oHostNicTeamingPolicy.reversePolicy = true;
        oHostNicTeamingPolicy.reversePolicySpecified = true;
        oHostNicTeamingPolicy.rollingOrder = false;
        oHostNicTeamingPolicy.rollingOrderSpecified = true;
        oHostNicTeamingPolicy.policy = "failover_explicit";
        oHostNicTeamingPolicy.nicOrder = oHostNicOrderPolicy;
        oHostNicTeamingPolicy.failureCriteria = oHostNicFailureCriteria;
        HostNetOffloadCapabilities oHostNetOffloadCapabilities = new HostNetOffloadCapabilities();
        oHostNetOffloadCapabilities.csumOffload = true;
        oHostNetOffloadCapabilities.csumOffloadSpecified = true;
        oHostNetOffloadCapabilities.tcpSegmentation = true;
        oHostNetOffloadCapabilities.tcpSegmentationSpecified = true;
        oHostNetOffloadCapabilities.zeroCopyXmit = true;
        oHostNetOffloadCapabilities.zeroCopyXmitSpecified = true;
        HostNetworkSecurityPolicy oHostNetworkSecurityPolicy = new HostNetworkSecurityPolicy();
        oHostNetworkSecurityPolicy.allowPromiscuous = false;
        oHostNetworkSecurityPolicy.allowPromiscuousSpecified = true;
        oHostNetworkSecurityPolicy.forgedTransmits = true;
        oHostNetworkSecurityPolicy.forgedTransmitsSpecified = true;
        oHostNetworkSecurityPolicy.macChanges = true;
        oHostNetworkSecurityPolicy.macChangesSpecified = true;
        HostNetworkTrafficShapingPolicy oHostNetworkTrafficShapingPolicy = new HostNetworkTrafficShapingPolicy();
        oHostNetworkTrafficShapingPolicy.enabled = false;
        oHostNetworkTrafficShapingPolicy.enabledSpecified = true;
        networkPolicy.nicTeaming = oHostNicTeamingPolicy;
        networkPolicy.offloadPolicy = oHostNetOffloadCapabilities;
        networkPolicy.security = oHostNetworkSecurityPolicy;
        networkPolicy.shapingPolicy = oHostNetworkTrafficShapingPolicy;
        return networkPolicy;
    }
    public ManagedObjectReference GetMOR(string _name, string _type)
    {
        ManagedObjectReference datacenterRef = oVMWare.GetDataCenter();
        ManagedObjectReference returnRef = new ManagedObjectReference();
        // Get Other objects
        TraversalSpec dc2hf = new TraversalSpec();
        dc2hf.type = "Datacenter";
        dc2hf.name = "dc2hf";
        dc2hf.path = "hostFolder";
        dc2hf.skip = false;
        dc2hf.skipSpecified = true;
        TraversalSpec cr2h = new TraversalSpec();
        cr2h.type = "ComputeResource";
        cr2h.name = "cr2h";
        cr2h.path = "host";
        cr2h.skip = false;
        TraversalSpec ccr2h = new TraversalSpec();
        ccr2h.type = "ClusterComputeResource";
        ccr2h.name = "ccr2h";
        ccr2h.path = "host";
        ccr2h.skip = false;
        TraversalSpec f2ce = new TraversalSpec();
        f2ce.type = "Folder";
        f2ce.name = "f2ce";
        f2ce.path = "childEntity";
        f2ce.skip = false;
        dc2hf.skipSpecified = true;
        //Then setup your recursion with the potential "depth" you need. For example a DataCenter only needs to go to a Folder. A folder though needs to be able to goto another folder, a Datacenter, a ComputeResource and a ClusterComputeResource 
        dc2hf.selectSet = new SelectionSpec[] { new SelectionSpec() };
        dc2hf.selectSet[0].name = "f2ce";
        f2ce.selectSet = new SelectionSpec[] { new SelectionSpec(), new SelectionSpec(), new SelectionSpec(), new SelectionSpec() };
        f2ce.selectSet[0].name = "f2ce";
        f2ce.selectSet[1].name = "cr2h";
        f2ce.selectSet[2].name = "dc2hf";
        f2ce.selectSet[3].name = "ccr2h";
        //Then combine your TrversalSpecs into an array
        TraversalSpec[] tsa = new TraversalSpec[] { f2ce, cr2h, dc2hf, ccr2h };
        //Define your properties. Name for this example, but you could do any properties or All
        PropertySpec[] psa = new PropertySpec[] { new PropertySpec() };
        psa[0].all = false;
        psa[0].allSpecified = true;
        psa[0].pathSet = new string[] { "name" };
        psa[0].type = "ManagedEntity";
        //Then combine your traversal spec and your propertyspec into your "object collector"
        PropertyFilterSpec pfs = new PropertyFilterSpec();
        pfs.propSet = psa;
        pfs.objectSet = new ObjectSpec[] { new ObjectSpec() };
        pfs.objectSet[0].obj = _sic.rootFolder;
        pfs.objectSet[0].skip = false;
        pfs.objectSet[0].selectSet = tsa;
        //Run it through VC
        ObjectContent[] oca = _service.RetrieveProperties(_sic.propertyCollector, new PropertyFilterSpec[] { pfs });
        foreach (ObjectContent oc in oca)
        {
            if (oc.obj.type.ToUpper() == _type && ((string)oVMWare.getObjectProperty(oc.obj, "name")).ToUpper() == _name.ToUpper())
                returnRef = oc.obj;
        }
        return returnRef;
    }
</script>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" Runat="Server">
<script type="text/javascript">
</script>
done!
</asp:Content>