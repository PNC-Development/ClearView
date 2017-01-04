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
using PAObjectsLib;
using Vim25Api;
using System.Reflection;
using NCC.ClearView.Application.Core.ClearViewWS;
using NCC.ClearView.Application.Core.w08r2;

namespace NCC.ClearView.Presentation.Web
{
    public partial class test_vmware : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        private string dsnServiceEditor = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ServiceEditorDSN"]].ConnectionString;
        private int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        private VMWare oVMWare;

        protected void Page_Load(object sender, EventArgs e)
        {
            string strResult = "";
            string strError = "";
            Servers oServer = new Servers(0, dsn);
            OnDemand oOnDemand = new OnDemand(0, dsn);
            IPAddresses oIPAddresses = new IPAddresses(0, dsnIP, dsn);
            oVMWare = new VMWare(0, dsn);

            string strName = "healytest2";
            string strCluster = "CDALVMTEST01";
            string strDatastore1 = "dt01-ibm-lun1";
            string strDatastore2 = "";
            string strVLAN = "VLAN52";
            string strPortGroupName = "dvPortGroup255";
            string strMACAddress = "";
            string strResourcePool = "";
            string strMDTos = "WABEx64";
            string strMDTtask = "W2K8R2_ENT";
            string strMDTbuildDB = "ServerShare";
            double dblDriveC = 27.5;
            double dblDrive2 = 2.5;
            string strVMguestOS = "winNetEnterprise64Guest";

            intEnvironment = 999;
            string strConnect = oVMWare.ConnectDEBUG("https://wdsvt100a/sdk", intEnvironment, "PNC");
            if (Request.QueryString["old"] != null)
            {
                strName = "healytest";
                strCluster = "ohcinxcv4003";
                strDatastore1 = "CINDSSVCN40063";
                strVLAN = "vlan250net";
                intEnvironment = 3;
                strConnect = oVMWare.ConnectDEBUG("https://ohcinutl4003/sdk", intEnvironment, "Dalton");
            }
            if (Request.QueryString["w"] != null)
            {
                strName = "healytest2";
                strCluster = "CLEVDTLAB01";
                strDatastore1 = "VDItest";
                strDatastore2 = "pagefile01";
                strPortGroupName = "dvPortGroup";
                strResourcePool = "VDI";
                strMDTos = "DesktopWABEx86";
                strMDTtask = "VDIXP";
                strMDTbuildDB = "DesktopDeploymentShare";
                strVMguestOS = "windows7_64Guest";
                strConnect = oVMWare.ConnectDEBUG("https://vwsvt102/sdk", intEnvironment, "PNC-TestLab");
            }
            if (Request.QueryString["t"] != null)
            {
                strName = "healyTest2012";
                strCluster = "CLESVTLAB01";
                dblDriveC = 45.00;
                strDatastore1 = "CTVXN00008";
                strDatastore2 = "CTVXN00008";
                dblDrive2 = 10.00;
                strPortGroupName = "dvPortGroup5";  // DHCP enabled
                strMDTos = "WABEx64";
                strMDTtask = "W2K8R2_STD";
                strMDTbuildDB = "ServerShare";
                strVMguestOS = "windows8Server64Guest";
                strConnect = oVMWare.ConnectDEBUG("https://wcsvt013a.pnceng.pvt/sdk", (int)CurrentEnvironment.PNCENG, "PNC");
            }
            Variables oVariable = new Variables(intEnvironment);
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

                int intStep = 0;
                Int32.TryParse(Request.QueryString["s"], out intStep);

                string strUUID = "";
                ManagedObjectReference oComputer = null;
                if (intStep == 100 || intStep == 1)
                {
                    if (oComputer != null && oComputer.Value != "")
                    {
                        // Destroy computer
                        ManagedObjectReference _task_power = _service.Destroy_Task(oComputer);
                        TaskInfo _info_power = (TaskInfo)oVMWare.getObjectProperty(_task_power, "info");
                        while (_info_power.state == TaskInfoState.running)
                            _info_power = (TaskInfo)oVMWare.getObjectProperty(_task_power, "info");
                        if (_info_power.state == TaskInfoState.success)
                            strResult += "Virtual Machine " + strName.ToUpper() + " Destroyed<br/>";
                        else
                            strError = "Virtual Machine was not destroyed";
                    }
                    if (strError == "")
                    {
                        // Create computer
                        oConfig.annotation = "Blah, Blah, Blah" + Environment.NewLine + "Next Line";
                        oConfig.guestId = strVMguestOS;
                        oConfig.firmware = "efi";
                        string strRamConfig = "2048";
                        oConfig.memoryMB = long.Parse(strRamConfig);
                        oConfig.memoryMBSpecified = true;
                        int intCpuConfig = 1;
                        oConfig.numCPUs = intCpuConfig;
                        oConfig.numCPUsSpecified = true;
                        oConfig.name = strName.ToLower();
                        oConfig.files = new VirtualMachineFileInfo();
                        oConfig.files.vmPathName = "[" + strDatastore1 + "] " + strName.ToLower() + "/" + strName.ToLower() + ".vmx";

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
                    }
                }

                if (intStep > 1)
                    oComputer = oVMWare.GetVM(strName);

                if ((intStep == 100 || intStep == 2) && strError == "")
                {
                    // 2) SCSI Controller
                    VirtualMachineConfigSpec _cs_scsi = new VirtualMachineConfigSpec();
                    VirtualDeviceConfigSpec controlVMSpec = Controller(true);
                    _cs_scsi.deviceChange = new VirtualDeviceConfigSpec[] { controlVMSpec };
                    ManagedObjectReference _task_scsi = _service.ReconfigVM_Task(oComputer, _cs_scsi);
                    TaskInfo _inf_scsi = (TaskInfo)oVMWare.getObjectProperty(_task_scsi, "info");
                    while (_inf_scsi.state == TaskInfoState.running)
                        _inf_scsi = (TaskInfo)oVMWare.getObjectProperty(_task_scsi, "info");
                    if (_inf_scsi.state == TaskInfoState.success)
                        strResult += "SCSI Controller Created<br/>";
                    else
                        strError = "SCSI Controller Was Not Created";
                }

                if ((intStep == 100 || intStep == 3) && strError == "")
                {
                    // 3) Create Hard Disk 1 
                    VirtualMachineConfigSpec _cs_hdd1 = new VirtualMachineConfigSpec();
                    dblDriveC = dblDriveC * 1024.00 * 1024.00;
                    VirtualDeviceConfigSpec diskVMSpec1 = Disk(oVMWare, strName, strDatastore1, dblDriveC.ToString(), 0, "");    // 10485760 KB = 10 GB = 10 x 1024 x 1024
                    _cs_hdd1.deviceChange = new VirtualDeviceConfigSpec[] { diskVMSpec1 };
                    ManagedObjectReference _task_hdd1 = _service.ReconfigVM_Task(oComputer, _cs_hdd1);
                    TaskInfo _info_hdd1 = (TaskInfo)oVMWare.getObjectProperty(_task_hdd1, "info");
                    while (_info_hdd1.state == TaskInfoState.running)
                        _info_hdd1 = (TaskInfo)oVMWare.getObjectProperty(_task_hdd1, "info");
                    if (_info_hdd1.state == TaskInfoState.success)
                        strResult += "Hard Drive Created (" + dblDriveC.ToString() + ")<br/>";
                    else
                        strError = "Hard Drive Was Not Created";
                }

                if ((intStep == 100 || intStep == 33) && strError == "")
                {
                    if (strDatastore2 != "")
                    {
                        // 33) Create Hard Disk 2
                        VirtualMachineConfigSpec _cs_hdd1 = new VirtualMachineConfigSpec();
                        dblDrive2 = dblDrive2 * 1024.00 * 1024.00;
                        VirtualDeviceConfigSpec diskVMSpec1 = Disk(oVMWare, strName, strDatastore2, dblDrive2.ToString(), 1, (strDatastore1 == strDatastore2 ? "_1" : ""));    // 10485760 KB = 10 GB = 10 x 1024 x 1024
                        _cs_hdd1.deviceChange = new VirtualDeviceConfigSpec[] { diskVMSpec1 };
                        ManagedObjectReference _task_hdd1 = _service.ReconfigVM_Task(oComputer, _cs_hdd1);
                        TaskInfo _info_hdd1 = (TaskInfo)oVMWare.getObjectProperty(_task_hdd1, "info");
                        while (_info_hdd1.state == TaskInfoState.running)
                            _info_hdd1 = (TaskInfo)oVMWare.getObjectProperty(_task_hdd1, "info");
                        if (_info_hdd1.state == TaskInfoState.success)
                            strResult += "Hard Drive # 2 Created (" + dblDrive2.ToString() + ")<br/>";
                        else
                            strError = "Hard Drive # 2 Was Not Created";
                    }
                }

                if ((intStep == 100 || intStep == 4) && strError == "")
                {
                    // 4) Create Network Adapter
                    bool boolISVmware4 = true;
                    if (boolISVmware4 == false)
                    {
                        VirtualDeviceConfigSpec[] configspecarr = configspecarr = new VirtualDeviceConfigSpec[1];
                        VirtualEthernetCardNetworkBackingInfo vecnbi = new VirtualEthernetCardNetworkBackingInfo();
                        vecnbi.deviceName = strVLAN;
                        VirtualEthernetCard newethdev;
                        //newethdev = new VirtualE1000();
                        newethdev = new VirtualVmxnet3();
                        //      ******** OTHER WAY = newethdev = new VirtualPCNet32();
                        newethdev.backing = vecnbi;
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
                            strResult += "Network Adapter Created<br/>";
                        else
                            strError = "Network Adapter Was Not Created";
                    }
                    else
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
                }
                if ((intStep == 100 || intStep == 5) && strError == "")
                {
                    VirtualMachineConfigSpec _cs_swap = new VirtualMachineConfigSpec();
                    _cs_swap.swapPlacement = "hostLocal";
                    ManagedObjectReference _task_swap = _service.ReconfigVM_Task(oComputer, _cs_swap);
                    TaskInfo _info_swap = (TaskInfo)oVMWare.getObjectProperty(_task_swap, "info");
                    while (_info_swap.state == TaskInfoState.running)
                        _info_swap = (TaskInfo)oVMWare.getObjectProperty(_task_swap, "info");
                    if (_info_swap.state == TaskInfoState.success)
                        strResult += "Swap File Configured<br/>";
                    else
                        strError = "Swap File Was Not Configured";
                    /*
                    // 5) Attach Floppy Drive
                    VirtualMachineConfigSpec _cs_floppy = new VirtualMachineConfigSpec();
                    VirtualDeviceConfigSpec _dcs_floppy = new VirtualDeviceConfigSpec();
                    _dcs_floppy.operation = VirtualDeviceConfigSpecOperation.add;
                    _dcs_floppy.operationSpecified = true;
                    VirtualDeviceConnectInfo _ci_floppy = new VirtualDeviceConnectInfo();
                    _ci_floppy.startConnected = false;
                    VirtualFloppy floppy = new VirtualFloppy();
                    VirtualFloppyRemoteDeviceBackingInfo floppyBack = new VirtualFloppyRemoteDeviceBackingInfo();
                    floppyBack.deviceName = "";
                    floppy.backing = floppyBack;
                    floppy.key = 8000;
                    floppy.controllerKey = 400;
                    floppy.controllerKeySpecified = true;
                    floppy.connectable = _ci_floppy;
                    _dcs_floppy.device = floppy;
                    _cs_floppy.deviceChange = new VirtualDeviceConfigSpec[] { _dcs_floppy };
                    ManagedObjectReference _task_floppy = _service.ReconfigVM_Task(oComputer, _cs_floppy);
                    TaskInfo _info_floppy = (TaskInfo)oVMWare.getObjectProperty(_task_floppy, "info");
                    while (_info_floppy.state == TaskInfoState.running)
                        _info_floppy = (TaskInfo)oVMWare.getObjectProperty(_task_floppy, "info");
                    if (_info_floppy.state == TaskInfoState.success)
                        strResult += "Floppy Drive Created<br/>";
                    else
                        strError = "Floppy Drive Was Not Created";
                    */
                    
                }

                if ((intStep == 100 || intStep == 6) && strError == "")
                {
                    // 6) Attach CD-ROM Drive
                    VirtualMachineConfigSpec _cs_cd = new VirtualMachineConfigSpec();
                    VirtualDeviceConfigSpec _dcs_cd = new VirtualDeviceConfigSpec();
                    _dcs_cd.operation = VirtualDeviceConfigSpecOperation.add;
                    _dcs_cd.operationSpecified = true;
                    VirtualDeviceConnectInfo _ci_cd = new VirtualDeviceConnectInfo();
                    _ci_cd.startConnected = false;
                    VirtualCdrom cd = new VirtualCdrom();
                    VirtualCdromRemotePassthroughBackingInfo cdBack = new VirtualCdromRemotePassthroughBackingInfo();
                    cdBack.exclusive = false;
                    cdBack.deviceName = "";
                    cd.backing = cdBack;
                    cd.key = 3000;
                    cd.controllerKey = 200;
                    cd.controllerKeySpecified = true;
                    cd.connectable = _ci_cd;
                    _dcs_cd.device = cd;
                    _cs_cd.deviceChange = new VirtualDeviceConfigSpec[] { _dcs_cd };
                    ManagedObjectReference _task_cd = _service.ReconfigVM_Task(oComputer, _cs_cd);
                    TaskInfo _info_cd = (TaskInfo)oVMWare.getObjectProperty(_task_cd, "info");
                    while (_info_cd.state == TaskInfoState.running)
                        _info_cd = (TaskInfo)oVMWare.getObjectProperty(_task_cd, "info");
                    if (_info_cd.state == TaskInfoState.success)
                        strResult += "CD-ROM Was Created<br/>";
                    else
                        strError = "CD-ROM Was Not Created";
                }

                if ((intStep == 100 || intStep == 7) && strError == "")
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

                if ((intStep == 100 || intStep == 8) && strError == "")
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

                if ((intStep == 100 || intStep == 9) && strError == "")
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

                if ((intStep == 100 || intStep == 10) && strError == "")
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
        public VirtualDeviceConfigSpec Controller(bool Is2012)
        {
            VirtualSCSIController scsi = new VirtualSCSIController();
            if (Is2012)
                scsi = new VirtualLsiLogicSASController();
            else
                scsi = new VirtualLsiLogicController();
            VirtualDeviceConnectInfo ci = new VirtualDeviceConnectInfo();
            ci.startConnected = false;
            scsi.key = 1000;
            scsi.controllerKey = 100;
            scsi.controllerKeySpecified = true;
            scsi.busNumber = 0;
            scsi.hotAddRemove = true;
            scsi.hotAddRemoveSpecified = true;
            scsi.scsiCtlrUnitNumber = 7;
            scsi.scsiCtlrUnitNumberSpecified = true;
            scsi.sharedBus = VirtualSCSISharing.noSharing;
            scsi.unitNumber = 2;
            scsi.unitNumberSpecified = true;
            scsi.connectable = ci;
            VirtualDeviceConfigSpec dcs = new VirtualDeviceConfigSpec();
            dcs.device = scsi;
            dcs.operation = VirtualDeviceConfigSpecOperation.add;
            dcs.operationSpecified = true;
            return dcs;
        }
        public VirtualDeviceConfigSpec Disk(VMWare oVMWare, string _name, string _datastore, string _size, int _unit_number, string _suffix)
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
            disk.controllerKey = 1000;
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
