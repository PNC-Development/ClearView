<%@ Page Language="C#" Debug="false" EnableEventValidation="false" ValidateRequest="false" MasterPageFile="~/clearview.master" %>
<script runat="server">
    private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
    private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
    private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
    private string dsnZeus = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ZeusDSN"]].ConnectionString;
    private int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
    private int intVMWare = Int32.Parse(ConfigurationManager.AppSettings["VMWareModelID"]);
    private bool boolUsePNCNaming = (ConfigurationManager.AppSettings["USE_PNC_NAMING"] == "1");
    private VMWare oVMWare;
    private void Page_Load()
    {
        string strError = "";
        Servers oServer = new Servers(0, dsn);
        OnDemand oOnDemand = new OnDemand(0, dsn);
        IPAddresses oIPAddresses = new IPAddresses(0, dsnIP, dsn);
        oVMWare = new VMWare(0, dsn);
        Functions oFunction = new Functions(0, dsn, intEnvironment);
        int intServer = 188;
        string strName = "OHCLEUTL4121";
        string strConnect = oVMWare.Connect(strName);
        VimService _service = oVMWare.GetService();
        string strVLAN = "vlan161net";
        bool boolVlan = false;
        if (strVLAN != "" && boolVlan == true)
        {
            ManagedObjectReference _vm_net = oVMWare.GetVM(strName);
            VirtualDeviceConfigSpec[] configspecarr = configspecarr = new VirtualDeviceConfigSpec[1];
            VirtualEthernetCardNetworkBackingInfo vecnbi = new VirtualEthernetCardNetworkBackingInfo();
            vecnbi.deviceName = strVLAN;
            VirtualEthernetCard newethdev = new VirtualPCNet32();
            newethdev.backing = vecnbi;
            newethdev.key = 5000;
            Response.Write("1 - " + newethdev.macAddress + "<br/>");
            VirtualDeviceConfigSpec newethdevicespec = new VirtualDeviceConfigSpec();
            newethdevicespec.device = newethdev;
            newethdevicespec.operation = VirtualDeviceConfigSpecOperation.add;
            Response.Write("2 - " + newethdev.macAddress + "<br/>");
            newethdevicespec.operationSpecified = true;
            configspecarr[0] = newethdevicespec;
            VirtualMachineConfigSpec vmconfigspec = new VirtualMachineConfigSpec();
            vmconfigspec.deviceChange = configspecarr;
            Response.Write("3 - " + newethdev.macAddress + "<br/>");
            ManagedObjectReference _task_net = _service.ReconfigVM_Task(_vm_net, vmconfigspec);
            TaskInfo _info_net = (TaskInfo)oVMWare.getObjectProperty(_task_net, "info");
            while (_info_net.state == TaskInfoState.running)
                _info_net = (TaskInfo)oVMWare.getObjectProperty(_task_net, "info");
            if (_info_net.state == TaskInfoState.success)
                Response.Write("Done - " + newethdev.macAddress + "<br/>");
            else
                Response.Write("Error - " + newethdev.macAddress + "<br/>");
        }
        else
        {
            ManagedObjectReference vmRef = oVMWare.GetVM(strName);
            GuestInfo ginfo = (GuestInfo)oVMWare.getObjectProperty(vmRef, "guest");
            VirtualMachineConfigInfo vminfo = (VirtualMachineConfigInfo)oVMWare.getObjectProperty(vmRef, "config");
            VirtualDevice [] test = vminfo.hardware.device;
            for (int ii = 0; ii < test.Length; ii++)
            {
                if (test[ii].deviceInfo.label == "Network Adapter 1")
                {
                    VirtualEthernetCard nic = (VirtualEthernetCard)test[ii];
                    Response.Write("MAC = " + nic.macAddress);
                    break;
                }
            }
            //VM.config.hardware.device = array of devices, find VirtualEthernetCards ..then retrieve the macAddress property of each NIC device            
        }
            
    }
</script>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" Runat="Server">
<script type="text/javascript">
</script>
done!
</asp:Content>