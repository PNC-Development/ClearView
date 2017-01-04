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
            string strName = "ohcinapp4200";
            ManagedObjectReference vmRef = oVMWare.GetVM(strName);
            GuestInfo ginfo = (GuestInfo)oVMWare.getObjectProperty(vmRef, "guest");
            VirtualMachineConfigInfo vminfo = (VirtualMachineConfigInfo)oVMWare.getObjectProperty(vmRef, "config");

            VirtualMachineBootOptions oBootOptions = new VirtualMachineBootOptions();
            oBootOptions.bootDelay = 10000;
            oBootOptions.bootDelaySpecified = true;
            VirtualMachineConfigSpec _cs_boot_options = new VirtualMachineConfigSpec();
            _cs_boot_options.bootOptions = oBootOptions;
            ManagedObjectReference _task_boot_options = _service.ReconfigVM_Task(vmRef, _cs_boot_options);
            TaskInfo _info_boot_options = (TaskInfo)oVMWare.getObjectProperty(_task_boot_options, "info");
            while (_info_boot_options.state == TaskInfoState.running)
                _info_boot_options = (TaskInfo)oVMWare.getObjectProperty(_task_boot_options, "info");
            string strResult = "NONE";
            if (_info_boot_options.state == TaskInfoState.success)
                strResult = "SUCCESS";
            else
                strResult = "ERROR";
            Response.Write(strResult);
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