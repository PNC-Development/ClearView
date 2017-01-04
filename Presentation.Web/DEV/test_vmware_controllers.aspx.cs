using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using NCC.ClearView.Application.Core;
using System.Net;
using System.IO;
using Vim25Api;
using NCC.ClearView.Application.Core.w08r2;

namespace NCC.ClearView.Presentation.Web
{
    // http://localhost:4412/dev/test_vmware_controllers.aspx?build=true

    public partial class test_vmware_controllers : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        private string dsnZeus = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ZeusDSN"]].ConnectionString;
        private int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        private int intVMWare = Int32.Parse(ConfigurationManager.AppSettings["VMWareModelID"]);
        private VMWare oVMWare;
        protected void Page_Load(object sender, EventArgs e)
        {
            string strResult = "";
            string strError = "";
            Servers oServer = new Servers(0, dsn);
            OnDemand oOnDemand = new OnDemand(0, dsn);
            IPAddresses oIPAddresses = new IPAddresses(0, dsnIP, dsn);
            oVMWare = new VMWare(0, dsn);
            Variables oVariable = new Variables(999);

            string strName = "healyTest";
            string strCluster = "CLESVTLAB01";
            double dblDriveC = 60.00;
            string strDatastore = "CTVXN00007";
            double dblDrive2 = 10.00;
            double dblDrive3 = 10.00;
            string strPortGroupName = "dvPortGroup5";  // DHCP enabled
            string strMDTos = "WABEx64";
            string strMDTtask = "W2K8R2_STD";
            string strMDTbuildDB = "ServerShare";
            string strVMguestOS = "windows7Server64Guest";
            string strMACAddress = "";
            string strResourcePool = "";

            string strConnect = oVMWare.ConnectDEBUG("https://vwsvt102/sdk", 999, "PNC-TestLab");
            if (strConnect == "")
            {
                VimService _service = oVMWare.GetService();
                ServiceContent _sic = oVMWare.GetSic();
                ManagedObjectReference datacenterRef = oVMWare.GetDataCenter();
                ManagedObjectReference vmFolderRef = oVMWare.GetVMFolder(datacenterRef);
                ManagedObjectReference clusterRef = oVMWare.GetCluster(strCluster);
                ManagedObjectReference resourcePoolRootRef = (ManagedObjectReference)oVMWare.getObjectProperty(clusterRef, "resourcePool");
                if (strResourcePool != "")
                    resourcePoolRootRef = oVMWare.GetResourcePool(clusterRef, strResourcePool);
                VirtualMachineConfigSpec oConfig = new VirtualMachineConfigSpec();

                // Create computer
                ManagedObjectReference oComputer = null;
                oConfig.annotation = "Blah, Blah, Blah" + Environment.NewLine + "Next Line";
                oConfig.guestId = strVMguestOS;
                string strRamConfig = "2048";
                oConfig.memoryMB = long.Parse(strRamConfig);
                oConfig.memoryMBSpecified = true;
                int intCpuConfig = 1;
                oConfig.numCPUs = intCpuConfig;
                oConfig.numCPUsSpecified = true;
                oConfig.name = strName.ToLower();
                oConfig.files = new VirtualMachineFileInfo();
                oConfig.files.vmPathName = "[" + strDatastore + "] " + strName.ToLower() + "/" + strName.ToLower() + ".vmx";

                ManagedObjectReference _task = _service.CreateVM_Task(vmFolderRef, oConfig, resourcePoolRootRef, null);
                TaskInfo oInfo = (TaskInfo)oVMWare.getObjectProperty(_task, "info");
                while (oInfo.state == TaskInfoState.running)
                    oInfo = (TaskInfo)oVMWare.getObjectProperty(_task, "info");
                if (oInfo.state == TaskInfoState.success)
                {
                    oComputer = (ManagedObjectReference)oInfo.result;
                    VirtualMachineConfigInfo _temp = (VirtualMachineConfigInfo)oVMWare.getObjectProperty(oComputer, "config");
                    strResult += "Virtual Machine " + strName.ToUpper() + " Created (" + _temp.uuid + ")<br/>";
                }
                else
                    strError = "Virtual Machine was not created";


                if (strError == "")
                {
                    // 2) SCSI Controller 1
                    VirtualMachineConfigSpec _cs_scsi = new VirtualMachineConfigSpec();
                    VirtualDeviceConfigSpec controlVMSpec = Controller(0, 2, 1000);
                    _cs_scsi.deviceChange = new VirtualDeviceConfigSpec[] { controlVMSpec };
                    ManagedObjectReference _task_scsi = _service.ReconfigVM_Task(oComputer, _cs_scsi);
                    TaskInfo _inf_scsi = (TaskInfo)oVMWare.getObjectProperty(_task_scsi, "info");
                    while (_inf_scsi.state == TaskInfoState.running)
                        _inf_scsi = (TaskInfo)oVMWare.getObjectProperty(_task_scsi, "info");
                    if (_inf_scsi.state == TaskInfoState.success)
                        strResult += "SCSI Controller # 1 Created<br/>";
                    else
                        strError = "SCSI Controller # 1 Was Not Created";
                }


                if (strError == "")
                {
                    // 3) Create Hard Disk 1 
                    VirtualMachineConfigSpec _cs_hdd1 = new VirtualMachineConfigSpec();
                    dblDriveC = dblDriveC * 1024.00 * 1024.00;
                    VirtualDeviceConfigSpec diskVMSpec1 = Disk(oVMWare, strName, strDatastore, dblDriveC.ToString(), 0, 1000, "");
                    _cs_hdd1.deviceChange = new VirtualDeviceConfigSpec[] { diskVMSpec1 };
                    ManagedObjectReference _task_hdd1 = _service.ReconfigVM_Task(oComputer, _cs_hdd1);
                    TaskInfo _info_hdd1 = (TaskInfo)oVMWare.getObjectProperty(_task_hdd1, "info");
                    while (_info_hdd1.state == TaskInfoState.running)
                        _info_hdd1 = (TaskInfo)oVMWare.getObjectProperty(_task_hdd1, "info");
                    if (_info_hdd1.state == TaskInfoState.success)
                        strResult += "Hard Drive # 1 Created (" + dblDriveC.ToString() + ")<br/>";
                    else
                        strError = "Hard Drive # 1 Was Not Created";
                }


                if (strError == "")
                {
                    // 4) Create Hard Disk 2 
                    VirtualMachineConfigSpec _cs_hdd1 = new VirtualMachineConfigSpec();
                    dblDrive2 = dblDrive2 * 1024.00 * 1024.00;
                    VirtualDeviceConfigSpec diskVMSpec1 = Disk(oVMWare, strName, strDatastore, dblDrive2.ToString(), 1, 1000, "_2");
                    _cs_hdd1.deviceChange = new VirtualDeviceConfigSpec[] { diskVMSpec1 };
                    ManagedObjectReference _task_hdd1 = _service.ReconfigVM_Task(oComputer, _cs_hdd1);
                    TaskInfo _info_hdd1 = (TaskInfo)oVMWare.getObjectProperty(_task_hdd1, "info");
                    while (_info_hdd1.state == TaskInfoState.running)
                        _info_hdd1 = (TaskInfo)oVMWare.getObjectProperty(_task_hdd1, "info");
                    if (_info_hdd1.state == TaskInfoState.success)
                        strResult += "Hard Drive # 2 Created (" + dblDriveC.ToString() + ")<br/>";
                    else
                        strError = "Hard Drive # 2 Was Not Created";
                }


                if (strError == "")
                {
                    // 5) SCSI Controller 2
                    VirtualMachineConfigSpec _cs_scsi = new VirtualMachineConfigSpec();
                    VirtualDeviceConfigSpec controlVMSpec = Controller(1, 3, 1001);
                    _cs_scsi.deviceChange = new VirtualDeviceConfigSpec[] { controlVMSpec };
                    ManagedObjectReference _task_scsi = _service.ReconfigVM_Task(oComputer, _cs_scsi);
                    TaskInfo _inf_scsi = (TaskInfo)oVMWare.getObjectProperty(_task_scsi, "info");
                    while (_inf_scsi.state == TaskInfoState.running)
                        _inf_scsi = (TaskInfo)oVMWare.getObjectProperty(_task_scsi, "info");
                    if (_inf_scsi.state == TaskInfoState.success)
                        strResult += "SCSI Controller # 1 Created<br/>";
                    else
                        strError = "SCSI Controller # 1 Was Not Created";
                }


                if (strError == "")
                {
                    // 6) Create Hard Disk 3 
                    VirtualMachineConfigSpec _cs_hdd1 = new VirtualMachineConfigSpec();
                    dblDrive3 = dblDrive3 * 1024.00 * 1024.00;
                    VirtualDeviceConfigSpec diskVMSpec1 = Disk(oVMWare, strName, strDatastore, dblDrive3.ToString(), 0, 1001, "_3");
                    _cs_hdd1.deviceChange = new VirtualDeviceConfigSpec[] { diskVMSpec1 };
                    ManagedObjectReference _task_hdd1 = _service.ReconfigVM_Task(oComputer, _cs_hdd1);
                    TaskInfo _info_hdd1 = (TaskInfo)oVMWare.getObjectProperty(_task_hdd1, "info");
                    while (_info_hdd1.state == TaskInfoState.running)
                        _info_hdd1 = (TaskInfo)oVMWare.getObjectProperty(_task_hdd1, "info");
                    if (_info_hdd1.state == TaskInfoState.success)
                        strResult += "Hard Drive # 3 Created (" + dblDriveC.ToString() + ")<br/>";
                    else
                        strError = "Hard Drive # 3 Was Not Created";
                }

                if (String.IsNullOrEmpty(Request.QueryString["build"]) == false)
                {
                    if (strError == "")
                    {
                        bool boolCompleted = false;
                        string strPortGroupKey = "";
                        ManagedObjectReference[] oNetworks = (ManagedObjectReference[])oVMWare.getObjectProperty(datacenterRef, "network");
                        foreach (ManagedObjectReference oNetwork in oNetworks)
                        {
                            if (boolCompleted == true)
                                break;
                            try
                            {
                                if (strPortGroupName == "" || strPortGroupName == oVMWare.getObjectProperty(oNetwork, "name").ToString())
                                {
                                    object oPortConfig = oVMWare.getObjectProperty(oNetwork, "config");
                                    if (oPortConfig != null)
                                    {
                                        DVPortgroupConfigInfo oPort = (DVPortgroupConfigInfo)oPortConfig;
                                        if (oPort.key != strPortGroupKey)
                                        {
                                            strPortGroupKey = oPort.key;
                                            ManagedObjectReference oSwitch = oPort.distributedVirtualSwitch;
                                            string strSwitchUUID = (string)oVMWare.getObjectProperty(oSwitch, "uuid");
                                            Response.Write("Trying..." + strPortGroupKey + "(" + strSwitchUUID + ")" + "<br/>");

                                            VirtualDeviceConfigSpec[] configspecarr = configspecarr = new VirtualDeviceConfigSpec[1];
                                            VirtualEthernetCardDistributedVirtualPortBackingInfo vecdvpbi = new VirtualEthernetCardDistributedVirtualPortBackingInfo();
                                            DistributedVirtualSwitchPortConnection connection = new DistributedVirtualSwitchPortConnection();
                                            connection.portgroupKey = strPortGroupKey;
                                            connection.switchUuid = strSwitchUUID;
                                            vecdvpbi.port = connection;
                                            //VirtualEthernetCard newethdev = new VirtualE1000();
                                            VirtualEthernetCard newethdev = new VirtualVmxnet3();
                                            //      ******** OTHER WAY = newethdev = new VirtualPCNet32();
                                            newethdev.backing = vecdvpbi;
                                            newethdev.key = 5000;
                                            VirtualDeviceConfigSpec newethdevicespec = new VirtualDeviceConfigSpec();
                                            newethdevicespec.device = newethdev;
                                            newethdevicespec.operation = VirtualDeviceConfigSpecOperation.add;
                                            newethdevicespec.operationSpecified = true;
                                            configspecarr[0] = newethdevicespec;
                                            VirtualMachineConfigSpec vmconfigspec = new VirtualMachineConfigSpec();
                                            vmconfigspec.deviceChange = configspecarr;
                                            ManagedObjectReference _task_net = _service.ReconfigVM_Task(oComputer, vmconfigspec);
                                            TaskInfo _info_net = (TaskInfo)oVMWare.getObjectProperty(_task_net, "info");
                                            while (_info_net.state == TaskInfoState.running)
                                                _info_net = (TaskInfo)oVMWare.getObjectProperty(_task_net, "info");
                                            if (_info_net.state == TaskInfoState.success)
                                            {
                                                strResult += "Network Adapter Created<br/>";
                                                boolCompleted = true;
                                            }
                                            //break;
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                // Only hits here if it is not a "DistributedVirtualPortgroup" (meaning it is a standard NETWORK object)
                            }
                        }
                        if (boolCompleted == false)
                            strError = "Network Adapter Was Not Created ~ Could not find a port group";
                    }

                    if (strError == "")
                    {
                        // 7) Boot Delay
                        VirtualMachineBootOptions oBootOptions = new VirtualMachineBootOptions();
                        oBootOptions.bootDelay = 10000;
                        oBootOptions.bootDelaySpecified = true;
                        VirtualMachineConfigSpec _cs_boot_options = new VirtualMachineConfigSpec();
                        _cs_boot_options.bootOptions = oBootOptions;
                        ManagedObjectReference _task_boot_options = _service.ReconfigVM_Task(oComputer, _cs_boot_options);
                        TaskInfo _info_boot_options = (TaskInfo)oVMWare.getObjectProperty(_task_boot_options, "info");
                        while (_info_boot_options.state == TaskInfoState.running)
                            _info_boot_options = (TaskInfo)oVMWare.getObjectProperty(_task_boot_options, "info");
                        if (_info_boot_options.state == TaskInfoState.success)
                            strResult += "Boot delay changed to 10 seconds<br/>";
                        else
                            strError = "Boot delay NOT changed";
                    }

                    if (strError == "")
                    {
                        // 8) Get MAC Address
                        VirtualMachineConfigInfo _vminfo = (VirtualMachineConfigInfo)oVMWare.getObjectProperty(oComputer, "config");
                        VirtualDevice[] _device = _vminfo.hardware.device;
                        for (int ii = 0; ii < _device.Length; ii++)
                        {
                            // 4/29/2009: Change to only one NIC for PNC
                            if (_device[ii].deviceInfo.label.ToUpper() == "NETWORK ADAPTER 1")
                            {
                                VirtualEthernetCard nic = (VirtualEthernetCard)_device[ii];
                                strMACAddress = nic.macAddress;
                                break;
                            }
                        }
                        if (strMACAddress != "")
                            strResult += "MAC Address = " + strMACAddress + "<br/>";
                        else
                            strError = "No MAC Address";
                    }

                    if (strError == "")
                    {
                        // 9) Configure WebService
                        System.Net.NetworkCredential oCredentials = new System.Net.NetworkCredential(oVariable.ADUser(), oVariable.ADPassword(), oVariable.Domain());
                        BuildSubmit oMDT = new BuildSubmit();
                        oMDT.Credentials = oCredentials;
                        oMDT.Url = "http://wcrdp100a.pnceng.pvt/wabebuild/BuildSubmit.asmx";
                        string[] strExtendedMDT = new string[] { "PNCBACKUPSOFTWARE:NONE", "IISInstall:NO", "HWConfig:DEFAULT", "ESMInstall:NO", "ClearViewInstall:YES", "Teamed2:DEFAULT", "HIDSInstall:NO" };
                        string strExtendedMDTs = "";
                        foreach (string extendedMDT in strExtendedMDT)
                        {
                            if (strExtendedMDTs != "")
                                strExtendedMDTs += ", ";
                            strExtendedMDTs += extendedMDT;
                        }
                        string strOutput = oMDT.automatedBuild2(strName.ToUpper(), strMACAddress, strMDTos, "provision", "PNCNT", strMDTtask, strMDTbuildDB, strExtendedMDT);
                        strResult += "WebService Configured " + strOutput + "<br/>";
                    }

                    if (strError == "")
                    {
                        // 10) Power On
                        GuestInfo ginfo_power = (GuestInfo)oVMWare.getObjectProperty(oComputer, "guest");
                        if (ginfo_power.guestState.ToUpper() == "NOTRUNNING" || ginfo_power.guestState.ToUpper() == "UNKNOWN")
                        {
                            ManagedObjectReference _task_power = _service.PowerOnVM_Task(oComputer, null);
                            TaskInfo _info_power = (TaskInfo)oVMWare.getObjectProperty(_task_power, "info");
                            while (_info_power.state == TaskInfoState.running)
                                _info_power = (TaskInfo)oVMWare.getObjectProperty(_task_power, "info");
                            if (_info_power.state == TaskInfoState.success)
                                strResult += "Virtual Machine Powered On";
                            else
                                strError = "Virtual Machine Was Not Powered On";
                        }
                        else
                            strResult += "Virtual Machine Was Already Powered On (" + ginfo_power.guestState + ")";
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

                Response.Write("RESULT(s): " + strResult);
                Response.Write("ERROR: " + strError);
            }
            else
                Response.Write("LOGIN error");
        }


        public VirtualDeviceConfigSpec Controller(int _bus_number, int _unit_number, int _controller_key)
        {
            VirtualLsiLogicController scsi = new VirtualLsiLogicController();
            VirtualDeviceConnectInfo ci = new VirtualDeviceConnectInfo();
            ci.startConnected = false;
            scsi.key = _controller_key;
            scsi.controllerKey = 100;
            scsi.controllerKeySpecified = true;
            scsi.busNumber = _bus_number;
            scsi.hotAddRemove = true;
            scsi.hotAddRemoveSpecified = true;
            scsi.scsiCtlrUnitNumber = 7;
            scsi.scsiCtlrUnitNumberSpecified = true;
            scsi.sharedBus = VirtualSCSISharing.noSharing;
            scsi.unitNumber = _unit_number;
            scsi.unitNumberSpecified = true;
            scsi.connectable = ci;
            VirtualDeviceConfigSpec dcs = new VirtualDeviceConfigSpec();
            dcs.device = scsi;
            dcs.operation = VirtualDeviceConfigSpecOperation.add;
            dcs.operationSpecified = true;
            return dcs;
        }
        public VirtualDeviceConfigSpec Disk(VMWare oVMWare, string _name, string _datastore, string _size, int _unit_number, int _controller_key, string _suffix)
        {
            // Get Datastores
            ManagedObjectReference datastoreRef = null;
            PropertySpec[] psDCarray = new PropertySpec[] { new PropertySpec() };
            psDCarray[0].all = false;
            psDCarray[0].allSpecified = true;
            psDCarray[0].pathSet = new string[] { "datastore" };
            psDCarray[0].type = "Datacenter";
            PropertyFilterSpec spec = new PropertyFilterSpec();
            spec.propSet = psDCarray;
            spec.objectSet = new ObjectSpec[] { new ObjectSpec() };
            spec.objectSet[0].obj = oVMWare.GetDataCenter();
            spec.objectSet[0].skip = true;
            ObjectContent[] ocary = oVMWare.GetService().RetrieveProperties(oVMWare.GetSic().propertyCollector, new PropertyFilterSpec[] { spec });
            ManagedObjectReference[] datastores = null;
            foreach (ObjectContent oc in ocary)
                datastores = (ManagedObjectReference[])oc.propSet[0].val;
            if (datastores != null)
                datastoreRef = datastores[0];

            // Create disk
            VirtualDisk disk = new VirtualDisk();
            disk.key = 2000;
            disk.controllerKey = _controller_key;  
            disk.controllerKeySpecified = true;
            disk.unitNumber = _unit_number;
            disk.unitNumberSpecified = true;
            VirtualDiskFlatVer2BackingInfo diskBack = new VirtualDiskFlatVer2BackingInfo();
            diskBack.diskMode = "persistent";
            diskBack.fileName = "[" + _datastore + "] " + _name.ToLower() + "/" + _name.ToLower() + _suffix + ".vmdk";
            diskBack.datastore = datastoreRef;
            diskBack.thinProvisioned = false;
            diskBack.thinProvisionedSpecified = true;
            diskBack.writeThrough = false;
            diskBack.writeThroughSpecified = true;
            disk.backing = diskBack;
            disk.capacityInKB = long.Parse(_size);
            VirtualDeviceConfigSpec dcs = new VirtualDeviceConfigSpec();
            dcs.device = disk;
            dcs.fileOperation = VirtualDeviceConfigSpecFileOperation.create;
            dcs.fileOperationSpecified = true;
            dcs.operation = VirtualDeviceConfigSpecOperation.add;
            dcs.operationSpecified = true;
            return dcs;
        }
    }
}
