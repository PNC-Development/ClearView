using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using System.Data.SqlClient;
using System.Timers;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using Microsoft.VirtualServer.Interop;
using System.Security.Principal;
using System.Security.Permissions;
using Microsoft.ApplicationBlocks.Data;
using NCC.ClearView.Application.Core;
using System.Threading;
using System.DirectoryServices;
using ActiveDs;

namespace ClearViewVirtualWorkstation
{
    public enum RpcAuthnLevel
    {
        Default = 0,
        None,
        Connect,
        Call,
        Pkt,
        PktIntegrity,
        PktPrivacy
    }

    public enum RpcImpLevel
    {
        Default = 0,
        Anonymous,
        Identify,
        Impersonate = 3,
        Delegate
    }

    public enum EoAuthnCap
    {
        None = 0x00,
        MutualAuth = 0x01,
        StaticCloaking = 0x20,
        DynamicCloaking = 0x40,
        AnyAuthority = 0x80,
        MakeFullSIC = 0x100,
        Default = 0x800,
        SecureRefs = 0x02,
        AccessControl = 0x04,
        AppID = 0x08,
        Dynamic = 0x10,
        RequireFullSIC = 0x200,
        AutoImpersonate = 0x400,
        NoCustomMarshal = 0x2000,
        DisableAAA = 0x1000
    }
    public partial class Service : ServiceBase
    {
        [DllImport("Ole32.dll",
            ExactSpelling = true,
            EntryPoint = "CoInitializeSecurity",
            CallingConvention = CallingConvention.StdCall,
            SetLastError = false,
            PreserveSig = false)]
        private static extern void CoInitializeSecurity(
            IntPtr pVoid,
            int cAuthSvc,
            IntPtr asAuthSvc,
            IntPtr pReserved1,
            uint dwAuthnLevel,
            uint dwImpLevel,
            IntPtr pAuthList,
            uint dwCapabilities,
            IntPtr pReserved3);
        [DllImport("advapi32.dll")]
        public static extern int LogonUserA(String lpszUserName,
            String lpszDomain,
            String lpszPassword,
            int dwLogonType,
            int dwLogonProvider,
            ref IntPtr phToken);


        private System.Timers.Timer oTimer = null;
        private double dblInterval;
        private string dsnRemote;
        private string strNetwork;
        private int intLogging;
        private bool boolProduction;
        private EventLog oLog;
        private string strScripts = "E:\\APPS\\CLV\\ClearViewVirtualWorkstation\\";
        private Workstations oWorkstation;
        public Service()
        {
            InitializeComponent();
            CoInitializeSecurity(IntPtr.Zero,
                -1,
                IntPtr.Zero,
                IntPtr.Zero,
                (uint)RpcAuthnLevel.None,
                (uint)RpcImpLevel.Impersonate,
                IntPtr.Zero,
                (uint)EoAuthnCap.None,
                IntPtr.Zero);
            try
            {
                if (EventLog.SourceExists("ClearView") == false)
                {
                    EventLog.CreateEventSource("ClearView", "ClearView");
                    EventLog.WriteEntry(String.Format("ClearView EventLog Created"), EventLogEntryType.Information);
                }
                oLog = new EventLog();
                oLog.Source = "ClearView";
                oLog.ModifyOverflowPolicy(OverflowAction.OverwriteAsNeeded, 0);
                oLog.MaximumKilobytes = long.Parse("16384");
                DataSet ds = new DataSet();
                ds.ReadXml(strScripts + "config.xml");
                dblInterval = Convert.ToDouble(ds.Tables[0].Rows[0]["interval"].ToString());
                dsnRemote = ds.Tables[0].Rows[0]["remoteDSN"].ToString();
                strNetwork = ds.Tables[0].Rows[0]["network"].ToString();
                intLogging = Int32.Parse(ds.Tables[0].Rows[0]["logging"].ToString());
                boolProduction = (ds.Tables[0].Rows[0]["prod"].ToString() == "1");
                oWorkstation = new Workstations(0, dsnRemote);
                oTimer = new System.Timers.Timer(dblInterval);
                oTimer.Elapsed += new ElapsedEventHandler(this.ServiceTimer_Tick);
            }
            catch
            {
                EventLog.WriteEntry(String.Format("ClearView Virtual Workstation Service initialization has failed - INVALID XML FILE"), EventLogEntryType.Error);
            }
        }

        protected override void OnStart(string[] args)
        {
            oLog.WriteEntry(String.Format("ClearView Virtual Workstation Service started."), EventLogEntryType.Information);
            oTimer.AutoReset = true;
            oTimer.Enabled = true;
            oTimer.Start();
        }

        protected override void OnStop()
        {
            oLog.WriteEntry(String.Format("ClearView Virtual Workstation Service stopped."), EventLogEntryType.Information);
            oTimer.AutoReset = false;
            oTimer.Enabled = false;
        }

        private void ServiceTimer_Tick(object sender, System.Timers.ElapsedEventArgs e)
        {
            oTimer.Stop();
            // *********************************************
            // ************  START Processing  *************
            // *********************************************
            if (intLogging > 1)
                oLog.WriteEntry(String.Format("ClearView Virtual Workstation Service TICK."), EventLogEntryType.Information);
            // 6/30/2008 - Disable Threading
            //ThreadStart oJob = new ThreadStart(ServiceTick);
            //Thread oThreadJob = new Thread(oJob);
            //oThreadJob.Start();
            ServiceTick();
            // *******************************************
            // ************  END Processing  *************
            // *******************************************
            oTimer.Start();
        }
        private void ServiceTick()
        {
            try 
            {
                string strServer = Environment.MachineName;
                DataSet ds = oWorkstation.GetRemoteVirtuals(strServer);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    int intID = Int32.Parse(dr["id"].ToString());
                    int intEnvironment = Int32.Parse(dr["environment"].ToString());
                    string strINI = @"\\citrixfile.ntl-city.com\flexcfg$\vwcfg\VW.INI";
                    if (intEnvironment != 4)
                        strINI = @"\\citrixfiletest.tstctr.ntl-city.net\flexcfg$\vwcfg\VW.INI";
                    string strImageScan = @"\\citrixfile.ntl-city.com\flexcfg$\vwcfg\BuildScripts\VWH_IMAGESCAN_SECURITY.vbs";
                    if (intEnvironment != 4)
                        strImageScan = @"\\citrixfiletest.tstctr.ntl-city.net\flexcfg$\vwcfg\BuildScripts\VWH_IMAGESCAN_SECURITY.vbs";
                    AD oAD = new AD(0, "", intEnvironment);
                    int intAnswer = Int32.Parse(dr["answerid"].ToString());
                    string strHost = dr["hostname"].ToString();
                    string strVirtualDir = dr["virtualdir"].ToString();
                    string strZipPath = dr["gzippath"].ToString();
                    string strImage = dr["image"].ToString();
                    string strName = dr["name"].ToString();
                    string strSerial = dr["serial"].ToString();
                    int intStep = Int32.Parse(dr["step"].ToString());
                    int intRam = Int32.Parse(dr["ram"].ToString());
                    string strManager = dr["manager"].ToString();
                    string strGroupRemote = "GSGu_WKS" + strName + "RemoteA";
                    string strGroupRemote2K = "GSGu_WKS" + strName + "Remote2K";
                    string strGroupAdmin = "GSGu_WKS" + strName + "Adm";
                    string strGroupW = "GSGw_" + strName;
                    string strResult = "";
                    string strVBS = strScripts + "scripts\\" + intID + ".vbs";
                    string strVBSImageScan = strScripts + "scripts\\" + intID + "_imagescan.vbs";
                    bool bool2000 = (strName.ToUpper().StartsWith("T2K") == true || strName.ToUpper().StartsWith("W2K") == true);
                    if (intStep > 0)
                    {
                        Type typeVSClass = typeof(VMVirtualServerClass);
                        Type typeDCOM = Type.GetTypeFromCLSID(typeVSClass.GUID, strHost, true);
                        object objDCOM = Activator.CreateInstance(typeDCOM);
                        VMVirtualServerClass oServer = (VMVirtualServerClass)System.Runtime.InteropServices.Marshal.CreateWrapperOfType(objDCOM, typeVSClass);
                        switch (intStep)
                        {
                            case -1:
                                // Rebuild Workstation
                                if (intLogging > 0)
                                    oLog.WriteEntry(String.Format("Starting rebuild of workstation " + strName), EventLogEntryType.Information);
                                VMVirtualMachine oMachineDown = oServer.FindVirtualMachine(strName);
                                if (oMachineDown.State != VMVMState.vmVMState_TurningOff && oMachineDown.State != VMVMState.vmVMState_TurnedOff)
                                    oMachineDown.TurnOff();
                                if (oMachineDown.State == VMVMState.vmVMState_TurnedOff)
                                {
                                    string strDirectory = strVirtualDir + "\\" + strName;
                                    Directory.Delete(strDirectory);
                                    oWorkstation.NextRemoteVirtual(intID);
                                    oWorkstation.NextRemoteVirtual(intID);
                                    oWorkstation.NextRemoteVirtual(intID);
                                    oWorkstation.NextRemoteVirtual(intID);
                                }
                                break;
                            case 1:     // Step 1 - Selecting Host
                                // Should be taken care of when adding remote workstation
                                break;
                            case 2:     // Step 2 - Configuring Accounts
                                // User intervention required
                                break;
                            case 3:     // Step 3 - Initiating Build
                                if (Directory.Exists(strVirtualDir + "\\" + strName) == false)
                                    Directory.CreateDirectory(strVirtualDir + "\\" + strName);
                                oWorkstation.NextRemoteVirtual(intID);
                                break;
                            case 4:     // Step 4 - Building VHD
                                if (strZipPath == "")
                                {
                                    Zip oZip = new Zip();
                                    oZip.ExtractZipFile(strImage, strVirtualDir + "\\" + strName);
                                    oWorkstation.NextRemoteVirtual(intID);
                                }
                                else
                                {
                                    if (intLogging > 0)
                                        oLog.WriteEntry(String.Format("Starting step to create VHD for " + strName), EventLogEntryType.Information);
                                    string strDirectory = strVirtualDir + "\\" + strName + "\\";
                                    string strFile = strDirectory + "DriveCXP.vhd";
                                    if (bool2000 == true)
                                        strFile = strDirectory + "DriveC2K.vhd";
                                    string strRename = strDirectory + "DriveCXP.rename";
                                    if (bool2000 == true)
                                        strRename = strDirectory + "DriveC2K.rename";
                                    if (Process.GetProcessesByName("gzip").GetLength(0) == 0)
                                    {
                                        // GZIP not running - check to see if we have already extracted this file
                                        if (File.Exists(strFile))
                                        {
                                            // File exists - let's move on
                                            oWorkstation.NextRemoteVirtual(intID);
                                        }
                                        else
                                        {
                                            // Add code to only run if between 9PM and 6AM
                                            if (boolProduction == false || DateTime.Now.Hour >= 21 || DateTime.Now.Hour <= 6)
                                            {
                                                if (intLogging > 0)
                                                    oLog.WriteEntry(String.Format("Beginning extraction of VHD for " + strName), EventLogEntryType.Information);
                                                // File does not exist, begin unzip
                                                StreamWriter oWriter = new StreamWriter(strVBS);
                                                oWriter.WriteLine("Dim objShell");
                                                oWriter.WriteLine("Set objShell = CreateObject(\"WScript.Shell\")");
                                                // FORMAT: objShell.Run("cmd.exe /c F:\WXP_SysPrep\gzip -d -k -c ""F:\WXP_SysPrep\WXP Pro SP2 CORPTEST.vhd.gz"" > R:\vsfiles\LUN01\WXP\TESTHEALY\DriveCXP.vhd")
                                                oWriter.WriteLine("objShell.Run(\"cmd.exe /c " + strZipPath + " -d -k -c \"\"" + strImage + "\"\" > " + strFile + "\")");
                                                oWriter.WriteLine("Set objShell = Nothing");
                                                oWriter.Flush();
                                                oWriter.Close();
                                                ILaunchScript oScript = new SimpleLaunchWsh(strVBS, "", true) as ILaunchScript;
                                                oScript.Launch();
                                                if (intLogging > 0)
                                                    oLog.WriteEntry(String.Format("FINISHED extraction of VHD for " + strName), EventLogEntryType.Information);
                                            }
                                            else if (intLogging > 0)
                                                    oLog.WriteEntry(String.Format("Waiting until 9PM to start extraction of VHD for " + strName), EventLogEntryType.Information);
                                        }
                                    }
                                    else if (intLogging > 0)
                                        oLog.WriteEntry(String.Format("GZIP already running - will not create VHD for " + strName), EventLogEntryType.Information);
                                }
                                break;
                            case 5:     // Step 5 - Creating Virtual Machine
                                oServer.CreateVirtualMachine(strName, strVirtualDir + "\\" + strName);
                                oWorkstation.NextRemoteVirtual(intID);
                                //    break;
                                //case 6:   // Step 6 - Modifying RAM
                                VMVirtualMachine oMachine = oServer.FindVirtualMachine(strName);
                                oMachine.Memory = intRam;
                                oMachine.ShutdownActionOnQuit = VMShutdownAction.vmShutdownAction_Shutdown;
                                oWorkstation.NextRemoteVirtual(intID);
                                //    break;
                                //case 7:   // Step 7 - Adding CD Drive
                                oMachine.AddDVDROMDrive(VMDriveBusType.vmDriveBusType_IDE, 1, 0);
                                oWorkstation.NextRemoteVirtual(intID);
                                //    break;
                                //case 8:   // Step 8 - Connecting Virtual Disks
                                VMSCSIController oSCSI = oMachine.AddSCSIController();
                                oSCSI.Configure(false, 7);
                                if (bool2000 == true)
                                    oMachine.AddHardDiskConnection(strVirtualDir + "\\" + strName + "\\DriveC2K.vhd", VMDriveBusType.vmDriveBusType_SCSI, 0, 0);
                                else
                                    oMachine.AddHardDiskConnection(strVirtualDir + "\\" + strName + "\\DriveCXP.vhd", VMDriveBusType.vmDriveBusType_SCSI, 0, 0);
                                oWorkstation.NextRemoteVirtual(intID);
                                break;
                            case 9:     // Step 9 - Configuring Network
                                VMVirtualMachine oMachineNetwork = oServer.FindVirtualMachine(strName);
                                foreach (VMNetworkAdapter oAdap in oMachineNetwork.NetworkAdapters)
                                {
                                    foreach (VMVirtualNetwork oNet in oServer.VirtualNetworks)
                                    {
                                        if (oNet.Name.ToUpper() == strNetwork.ToUpper())
                                            oAdap.AttachToVirtualNetwork(oNet);
                                    }
                                }
                                oWorkstation.NextRemoteVirtual(intID);
                                break;
                            case 10:    // Step 10 - Updating Serial Number
                                VMVirtualMachine oMachineSerial = oServer.FindVirtualMachine(strName);
                                oMachineSerial.BaseBoardSerialNumber = strSerial;
                                oMachineSerial.ChassisSerialNumber = strSerial;
                                oMachineSerial.BIOSSerialNumber = strSerial;
                                oWorkstation.NextRemoteVirtual(intID);
                                break;
                            case 11:    // Step 11 - Creating AD groups
                                // Create RemoteA group in OUg_Resources/OUc_WksMgmt (not for Windows 2000 workstations)
                                if (bool2000 == false)
                                {
                                    strResult = oAD.CreateGroup(strGroupRemote, "Remote access group for virtual workstation", "Requires approval from one of the following:" + Environment.NewLine + "               " + strManager, "OU=OUc_WksMgmt,OU=OUg_Resources,", "GG", "S");
                                    if (strResult.Trim() != "")
                                        oLog.WriteEntry(String.Format(strResult), EventLogEntryType.Warning);
                                    if (intLogging > 0)
                                        oLog.WriteEntry(String.Format(strResult), EventLogEntryType.Information);
                                }
                                else
                                {
                                    strResult = oAD.CreateGroup(strGroupRemote2K, "Remote access group for virtual workstation", "Requires approval from one of the following:" + Environment.NewLine + "               " + strManager, "OU=OUc_WksMgmt,OU=OUg_Resources,", "GG", "S");
                                    if (strResult.Trim() != "")
                                        oLog.WriteEntry(String.Format(strResult), EventLogEntryType.Warning);
                                    if (intLogging > 0)
                                        oLog.WriteEntry(String.Format(strResult), EventLogEntryType.Information);
                                }
                                if (intEnvironment != (int)CurrentEnvironment.CORPDMN && intEnvironment != (int)CurrentEnvironment.PNCNT_PROD) // Not Production
                                {
                                    // Create Adm group in OUc_Orgs/OUg_Admins
                                    strResult = oAD.CreateGroup(strGroupAdmin, "Virtual Workstation; " + strManager, "", "OU=OUg_Admins,OU=OUc_Orgs,", "GG", "S");
                                    if (strResult.Trim() != "")
                                        oLog.WriteEntry(String.Format(strResult), EventLogEntryType.Warning);
                                    if (intLogging > 0)
                                        oLog.WriteEntry(String.Format(strResult), EventLogEntryType.Information);
                                    // Create GSGw group in OUg_Resources
                                    strResult = oAD.CreateGroup(strGroupW, "Virtual Workstation; " + strManager, "", "OU=OUg_Resources,", "GG", "S");
                                    if (strResult.Trim() != "")
                                        oLog.WriteEntry(String.Format(strResult), EventLogEntryType.Warning);
                                    if (intLogging > 0)
                                        oLog.WriteEntry(String.Format(strResult), EventLogEntryType.Information);
                                    // Create Workstation Object in OUc_DmnCptrs/OUc_Wksts/OUw_Standard
                                    strResult = oAD.CreateWorkstation(strName, "Virtual Workstation; " + strManager, "", "OU=OUw_Standard,OU=OUc_Wksts,OU=OUc_DmnCptrs,");
                                    if (strResult.Trim() != "")
                                        oLog.WriteEntry(String.Format(strResult), EventLogEntryType.Warning);
                                    if (intLogging > 0)
                                        oLog.WriteEntry(String.Format(strResult), EventLogEntryType.Information);
                                    // Add workstation to GSGw group
                                    strResult = oAD.JoinGroup(strName, strGroupW, 0);
                                    if (strResult.Trim() != "")
                                        oLog.WriteEntry(String.Format(strResult), EventLogEntryType.Warning);
                                    if (intLogging > 0)
                                        oLog.WriteEntry(String.Format(strResult), EventLogEntryType.Information);
                                }
                                oWorkstation.NextRemoteVirtual(intID);
                                break;
                            case 12:
                                // Step 12 - Adding AD accounts
                                DataSet dsAccounts = oWorkstation.GetRemoteAccounts(strName);
                                foreach (DataRow drAccount in dsAccounts.Tables[0].Rows)
                                {
                                    string strID = drAccount["xid"].ToString();
                                    if (intEnvironment != (int)CurrentEnvironment.CORPDMN && intEnvironment != (int)CurrentEnvironment.PNCNT_PROD) // Not Production
                                    {
                                        strID = "E" + strID.Substring(1);
                                        if (oAD.Search(strID, false) == null)
                                        {
                                            strID = "T" + strID.Substring(1);
                                            if (oAD.Search(strID, false) == null)
                                            {
                                                strResult = oAD.CreateUser(strID, strID, strID, "Abcd1234", "", "", "");
                                                if (strResult.Trim() != "")
                                                    oLog.WriteEntry(String.Format(strResult), EventLogEntryType.Warning);
                                                if (intLogging > 0)
                                                    oLog.WriteEntry(String.Format(strResult), EventLogEntryType.Information);
                                            }
                                        }
                                        if (drAccount["admin"].ToString() == "1")
                                        {
                                            strResult = oAD.JoinGroup(strID, strGroupAdmin, 0);
                                            if (strResult.Trim() != "")
                                                oLog.WriteEntry(String.Format(strResult), EventLogEntryType.Warning);
                                            if (intLogging > 0)
                                                oLog.WriteEntry(String.Format(strResult), EventLogEntryType.Information);
                                        }
                                    }
                                    if (drAccount["remote"].ToString() == "1")
                                    {
                                        if (bool2000 == false)
                                        {
                                            strResult = oAD.JoinGroup(strID, strGroupRemote, 0);
                                            if (strResult.Trim() != "")
                                                oLog.WriteEntry(String.Format(strResult), EventLogEntryType.Warning);
                                            if (intLogging > 0)
                                                oLog.WriteEntry(String.Format(strResult), EventLogEntryType.Information);
                                        }
                                        else
                                        {
                                            strResult = oAD.JoinGroup(strID, strGroupRemote2K, 0);
                                            if (strResult.Trim() != "")
                                                oLog.WriteEntry(String.Format(strResult), EventLogEntryType.Warning);
                                            if (intLogging > 0)
                                                oLog.WriteEntry(String.Format(strResult), EventLogEntryType.Information);
                                        }
                                    }
                                }
                                oWorkstation.NextRemoteVirtual(intID);
                                break;
                            case 13:
                                // Step 13 - Starting Machine
                                VMVirtualMachine oMachineStart = oServer.FindVirtualMachine(strName);
                                oMachineStart.Startup();
                                oWorkstation.NextRemoteVirtual(intID);
                                // Update INI File
                                DataSet dsINI = oWorkstation.GetRemoteAccounts(strName);
                                foreach (DataRow drINI in dsINI.Tables[0].Rows)
                                {
                                    string strID = drINI["xid"].ToString();
                                    if (intEnvironment != (int)CurrentEnvironment.CORPDMN && intEnvironment != (int)CurrentEnvironment.PNCNT_PROD) // Not Production
                                    {
                                        strID = "E" + strID.Substring(1);
                                        if (oAD.Search(strID, false) == null)
                                            strID = "T" + strID.Substring(1);
                                    }
                                    if (drINI["remote"].ToString() == "1")
                                    {
                                        // FORMAT: EMJY13C=TXPVM001,XP,OHCLEVWH4004
                                        bool boolAdd = true;
                                        string strLine = strID + "=" + strName + "," + (bool2000 ? "2000" : "XP") + "," + strHost;
                                        StreamReader oINIr = new StreamReader(strINI);
                                        while (oINIr.Peek() != -1)
                                        {
                                            string strCompare = oINIr.ReadLine();
                                            if (strCompare == strLine)
                                            {
                                                boolAdd = false;
                                                break;
                                            }
                                        }
                                        oINIr.Close();
                                        if (boolAdd == true)
                                        {
                                            StreamWriter oINI = new StreamWriter(strINI, true);
                                            oINI.WriteLine(strLine);
                                            oINI.Flush();
                                            oINI.Close();
                                        }
                                    }
                                }
                                break;
                            case 14:
                                // Step 14 - ZEUS
                                break;
                            case 15:
                                // Step 15 - Installing Applications
                                break;
                            case 16:
                                // Step 16 - Complete Request
                                // Run ImageScan script
                                File.Copy(strImageScan, strVBSImageScan, true);
                                ILaunchScript oScriptImageScan = new SimpleLaunchWsh(strVBSImageScan, "", true) as ILaunchScript;
                                oScriptImageScan.Launch();
                                // Delete Files
                                if (File.Exists(strVBS) == true)
                                    File.Delete(strVBS);
                                if (File.Exists(strVBSImageScan) == true)
                                    File.Delete(strVBSImageScan);
                                // Reboot the workstation
                                VMVirtualMachine oMachineRestart = oServer.FindVirtualMachine(strName);
                                oMachineRestart.Reset();
                                oWorkstation.UpdateRemoteVirtualCompleted(intID);
                                break;
                        }
                    }
                }


                ds = oWorkstation.GetRemoteVirtualDecoms(strServer);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    int intID = Int32.Parse(dr["id"].ToString());
                    int intEnvironment = Int32.Parse(dr["environment"].ToString());
                    string strINI = @"\\citrixfile.ntl-city.com\flexcfg$\vwcfg\VW.INI";
                    if (intEnvironment != 4)
                        strINI = @"\\citrixfiletest.tstctr.ntl-city.net\flexcfg$\vwcfg\VW.INI";
                    string strINITemp = @"\\citrixfile.ntl-city.com\flexcfg$\vwcfg\VW_TEMP.INI";
                    if (intEnvironment != 4)
                        strINITemp = @"\\citrixfiletest.tstctr.ntl-city.net\flexcfg$\vwcfg\VW_TEMP.INI";
                    AD oAD = new AD(0, "", intEnvironment);
                    string strVirtualDir = dr["virtualdir"].ToString();
                    string strName = dr["name"].ToString();
                    string strGroupRemote = "GSGu_WKS" + strName + "RemoteA";
                    string strGroupRemote2K = "GSGu_WKS" + strName + "Remote2K";
                    string strGroupAdmin = "GSGu_WKS" + strName + "Adm";
                    string strGroupW = "GSGw_" + strName;
                    string strResult = "";
                    bool bool2000 = (strName.ToUpper().StartsWith("T2K") == true || strName.ToUpper().StartsWith("W2K") == true);
                    Type typeVSClass = typeof(VMVirtualServerClass);
                    Type typeDCOM = Type.GetTypeFromCLSID(typeVSClass.GUID, strServer, true);
                    object objDCOM = Activator.CreateInstance(typeDCOM);
                    VMVirtualServerClass oServer = (VMVirtualServerClass)System.Runtime.InteropServices.Marshal.CreateWrapperOfType(objDCOM, typeVSClass);
                    VMVirtualMachine oMachine = oServer.FindVirtualMachine(strName);
                    if (oMachine.State == VMVMState.vmVMState_TurnedOff)
                    {
                        // Remove Workstation
                        oServer.DeleteVirtualMachine(oMachine);
                        if (intLogging > 0)
                            oLog.WriteEntry("Workstation " + strName + " has been deleted", EventLogEntryType.Information);
                        // Delete Folder
                        DeleteDirectory(strVirtualDir + "\\" + strName);
                        if (intLogging > 0)
                            oLog.WriteEntry("The directory for workstation " + strName + " has been deleted", EventLogEntryType.Information);
                        // Delete AD Objects
                        DirectoryEntry oGroup;
                        if (bool2000 == false)
                        {
                            oGroup = oAD.GroupSearch(strGroupRemote);
                            if (oGroup != null) 
                            {
                                strResult = oAD.Delete(oGroup);
                                if (strResult.Trim() != "")
                                    oLog.WriteEntry(String.Format(strResult), EventLogEntryType.Warning);
                                if (intLogging > 0)
                                    oLog.WriteEntry(String.Format(strResult), EventLogEntryType.Information);
                            }
                        }
                        else
                        {
                            oGroup = oAD.GroupSearch(strGroupRemote2K);
                            if (oGroup != null)
                            {
                                strResult = oAD.Delete(oGroup);
                                if (strResult.Trim() != "")
                                    oLog.WriteEntry(String.Format(strResult), EventLogEntryType.Warning);
                                if (intLogging > 0)
                                    oLog.WriteEntry(String.Format(strResult), EventLogEntryType.Information);
                            }
                        }
                        if (intEnvironment != (int)CurrentEnvironment.CORPDMN && intEnvironment != (int)CurrentEnvironment.PNCNT_PROD) // Not Production
                        {
                            oGroup = oAD.GroupSearch(strGroupAdmin);
                            if (oGroup != null)
                            {
                                strResult = oAD.Delete(oGroup);
                                if (strResult.Trim() != "")
                                    oLog.WriteEntry(String.Format(strResult), EventLogEntryType.Warning);
                                if (intLogging > 0)
                                    oLog.WriteEntry(String.Format(strResult), EventLogEntryType.Information);
                            }
                            oGroup = oAD.GroupSearch(strGroupW);
                            if (oGroup != null)
                            {
                                strResult = oAD.Delete(oGroup);
                                if (strResult.Trim() != "")
                                    oLog.WriteEntry(String.Format(strResult), EventLogEntryType.Warning);
                                if (intLogging > 0)
                                    oLog.WriteEntry(String.Format(strResult), EventLogEntryType.Information);
                            }
                            SearchResultCollection oComputers = oAD.ComputerSearch(strName);
                            if (oComputers.Count > 0)
                            {
                                oGroup = oComputers[0].GetDirectoryEntry();
                                if (oGroup != null)
                                {
                                    strResult = oAD.Delete(oGroup);
                                    if (strResult.Trim() != "")
                                        oLog.WriteEntry(String.Format(strResult), EventLogEntryType.Warning);
                                    if (intLogging > 0)
                                        oLog.WriteEntry(String.Format(strResult), EventLogEntryType.Information);
                                }
                            }
                        }
                        if (intLogging > 0)
                            oLog.WriteEntry("The AD groups for workstation " + strName + " have been deleted", EventLogEntryType.Information);
                        // Update the INI File
                        string strCompare = "=" + strName + "," + (bool2000 ? "2000" : "XP") + "," + strServer;
                        StreamWriter oINI = new StreamWriter(strINITemp, true);
                        StreamReader oINIr = new StreamReader(strINI);
                        while (oINIr.Peek() != -1)
                        {
                            string strLine = oINIr.ReadLine();
                            if (strLine.EndsWith(strCompare) == false)
                                oINI.WriteLine(strLine);
                        }
                        oINIr.Close();
                        oINI.Flush();
                        oINI.Close();
                        if (File.Exists(strINI) == true)
                            File.Delete(strINI);
                        File.Copy(strINITemp, strINI, true);
                        if (File.Exists(strINITemp) == true)
                            File.Delete(strINITemp);
                        if (intLogging > 0)
                            oLog.WriteEntry("The INI file [" + strINI + "] was updated", EventLogEntryType.Information);
                        oWorkstation.UpdateRemoteVirtualDecomCompleted(intID);
                        if (intLogging > 0)
                            oLog.WriteEntry("The workstation " + strName + " has been decommissioned", EventLogEntryType.Information);
                    }
                    else if (oMachine.State != VMVMState.vmVMState_TurningOff)
                    {
                        oMachine.TurnOff();
                        if (intLogging > 0)
                            oLog.WriteEntry("The workstation " + strName + " is turning off", EventLogEntryType.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                oLog.WriteEntry(String.Format(ex.Message) + " [" + System.Environment.UserName + "]", EventLogEntryType.Error);
            }
        }
        private void DeleteDirectory(string strFolder)
        {
            DirectoryInfo oDir = new DirectoryInfo(strFolder);
            DirectoryInfo[] oDirs = oDir.GetDirectories();
            FileInfo[] oFiles = oDir.GetFiles();
            foreach (FileInfo oFile in oFiles)
                File.Delete(strFolder + "\\" + oFile.Name);
            foreach (DirectoryInfo oSubDir in oDirs)
                DeleteDirectory(strFolder + "\\" + oSubDir.Name);
            Directory.Delete(strFolder);
        }
    }
}
