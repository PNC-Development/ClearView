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
    private void Page_Load()
    {
        string strError = "";
        Servers oServer = new Servers(0, dsn);
        OnDemand oOnDemand = new OnDemand(0, dsn);
        IPAddresses oIPAddresses = new IPAddresses(0, dsnIP, dsn);
        oVMWare = new VMWare(0, dsn);
        string strConnect = oVMWare.ConnectDEBUG("https://ohcleutl4001/sdk", 3, "Dalton");
        if (strConnect == "")
        {
            VimService _service = oVMWare.GetService();
            ServiceContent _sic = oVMWare.GetSic();
            string strName = "wdhcm100a";
            string strVLAN = "vlan216net";
            ManagedObjectReference vmRef = oVMWare.GetVM(strName);
            GuestInfo ginfo = (GuestInfo)oVMWare.getObjectProperty(vmRef, "guest");
            VirtualMachineConfigInfo vminfo = (VirtualMachineConfigInfo)oVMWare.getObjectProperty(vmRef, "config");
            VirtualDevice[] test = vminfo.hardware.device;
            string strResult = "";
            for (int ii = 0; ii < test.Length; ii++)
            {
                if (test[ii].deviceInfo.label == "Network Adapter 1")
                {
                    VirtualEthernetCard nic = (VirtualEthernetCard)test[ii];

                    VirtualDeviceConfigSpec[] configspecarr = new VirtualDeviceConfigSpec[1];
                    VirtualEthernetCardNetworkBackingInfo vecnbi = new VirtualEthernetCardNetworkBackingInfo();
                    vecnbi.deviceName = strVLAN;
                    VirtualDeviceConfigSpec newethdevicespec = new VirtualDeviceConfigSpec();
                    newethdevicespec.device = nic;
                    nic.backing = vecnbi;
                    newethdevicespec.operation = VirtualDeviceConfigSpecOperation.edit;
                    newethdevicespec.operationSpecified = true;
                    configspecarr[0] = newethdevicespec;
                    VirtualMachineConfigSpec vmconfigspec = new VirtualMachineConfigSpec();
                    vmconfigspec.deviceChange = configspecarr;
                    ManagedObjectReference _task_net = _service.ReconfigVM_Task(vmRef, vmconfigspec);
                    TaskInfo _info_net = (TaskInfo)oVMWare.getObjectProperty(_task_net, "info");
                    while (_info_net.state == TaskInfoState.running)
                        _info_net = (TaskInfo)oVMWare.getObjectProperty(_task_net, "info");
                    if (_info_net.state == TaskInfoState.success)
                        strResult += "Network Adapter Created (VLAN: " + strVLAN + ")";
                    else
                        strResult += "Network Adapter Was Not Created";
                    
                    //// REMOVE
                    //VirtualDeviceConfigSpec[] deviceSpec = new VirtualDeviceConfigSpec[1];
                    //deviceSpec[0] = new VirtualDeviceConfigSpec();
                    //deviceSpec[0].operation = VirtualDeviceConfigSpecOperation.edit;
                    //deviceSpec[0].device = nic;
                    //deviceSpec[0].operationSpecified = true;
                    //VirtualMachineConfigSpec newSpec = new VirtualMachineConfigSpec();
                    //newSpec.deviceChange = deviceSpec;
                    //ManagedObjectReference _task_net2 = _service.ReconfigVM_Task(vmRef, newSpec);
                    //TaskInfo _info_net2 = (TaskInfo)oVMWare.getObjectProperty(_task_net2, "info");
                    //while (_info_net2.state == TaskInfoState.running)
                    //    _info_net2 = (TaskInfo)oVMWare.getObjectProperty(_task_net2, "info");
                    //if (_info_net2.state == TaskInfoState.success)
                    //    strResult += "Network Adapter Removed<br/>";
                    //else
                    //    strResult += "Network Adapter Was Not Removed<br/>";
                    
                    //// ADD
                    //VirtualDeviceConfigSpec[] configspecarr = configspecarr = new VirtualDeviceConfigSpec[1];
                    //VirtualEthernetCardNetworkBackingInfo vecnbi = new VirtualEthernetCardNetworkBackingInfo();
                    //vecnbi.deviceName = strVLAN;
                    //VirtualEthernetCard newethdev = new VirtualPCNet32();
                    //newethdev.backing = vecnbi;
                    //newethdev.key = 5000;
                    //VirtualDeviceConfigSpec newethdevicespec = new VirtualDeviceConfigSpec();
                    //newethdevicespec.device = newethdev;
                    //newethdevicespec.operation = VirtualDeviceConfigSpecOperation.add;
                    //newethdevicespec.operationSpecified = true;
                    //configspecarr[0] = newethdevicespec;
                    //VirtualMachineConfigSpec vmconfigspec = new VirtualMachineConfigSpec();
                    //vmconfigspec.deviceChange = configspecarr;
                    //ManagedObjectReference _task_net = _service.ReconfigVM_Task(vmRef, vmconfigspec);
                    //TaskInfo _info_net = (TaskInfo)oVMWare.getObjectProperty(_task_net, "info");
                    //while (_info_net.state == TaskInfoState.running)
                    //    _info_net = (TaskInfo)oVMWare.getObjectProperty(_task_net, "info");
                    //if (_info_net.state == TaskInfoState.success)
                    //    strResult += "Network Adapter Created (VLAN: " + strVLAN + ")";
                    //else
                    //    strResult += "Network Adapter Was Not Created";

                    Response.Write(strResult);
                    break;
                }
            }
        }
        else
            Response.Write("LOGIN error");
    }
</script>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" Runat="Server">
<script type="text/javascript">
</script>
done!
</asp:Content>