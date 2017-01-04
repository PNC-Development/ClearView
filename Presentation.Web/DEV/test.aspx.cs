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
using System.DirectoryServices;
using NCC.ClearView.Application.Core.w08r2;
using System.Data.OleDb;
using System.Security.Principal;
using ActiveDs;
using System.Collections.Generic;

namespace NCC.ClearView.Presentation.Web
{
    public partial class test : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        private string dsnServiceEditor = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ServiceEditorDSN"]].ConnectionString;
        private int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intAssignPage = Int32.Parse(ConfigurationManager.AppSettings["AssignWorkload"]);
        protected int intViewPage = Int32.Parse(ConfigurationManager.AppSettings["ViewWorkload"]);
        protected int intResourceRequestApprove = Int32.Parse(ConfigurationManager.AppSettings["ResourceRequestApprove"]);
        protected int intCount = 0;
        private string strScripts = @"C:\Scripts\clearview\";
        private Asset oAsset;
        private Log oLog;

        protected void Page_Load(object sender, EventArgs e)
        {
            List<VMwareDisk> disks = Disks();
        }
        private List<VMwareDisk> Disks()
        {
            List<VMwareDisk> disks = new List<VMwareDisk>();

            VMWare oVMWare = new VMWare(0, dsn);
            string name = "WSCLV223A";
            DataSet dsGuest = oVMWare.GetGuest(name);
            if (dsGuest.Tables[0].Rows.Count > 0)
            {
                DataRow drGuest = dsGuest.Tables[0].Rows[0];
                int intDatastore = Int32.Parse(drGuest["datastoreid"].ToString());
                int intHost = Int32.Parse(drGuest["hostid"].ToString());
                int intCluster = Int32.Parse(oVMWare.GetHost(intHost, "clusterid"));
                int intFolder = Int32.Parse(oVMWare.GetCluster(intCluster, "folderid"));
                int intDataCenter = Int32.Parse(oVMWare.GetFolder(intFolder, "datacenterid"));
                string strDataCenter = oVMWare.GetDatacenter(intDataCenter, "name");
                int intVirtualCenter = Int32.Parse(oVMWare.GetDatacenter(intDataCenter, "virtualcenterid"));
                string strVirtualCenter = oVMWare.GetVirtualCenter(intVirtualCenter, "name");
                string strVirtualCenterURL = oVMWare.GetVirtualCenter(intVirtualCenter, "url");
                int intVirtualCenterEnv = Int32.Parse(oVMWare.GetVirtualCenter(intVirtualCenter, "environment"));
                string strConnect = oVMWare.ConnectDEBUG(strVirtualCenterURL, intVirtualCenterEnv, strDataCenter);
                VimService _service = oVMWare.GetService();
                ServiceContent _sic = oVMWare.GetSic();
                try
                {
                    ManagedObjectReference oVM = oVMWare.GetVM(name);
                    if (oVM != null)
                    {
                        VirtualMachineConfigInfo vminfo = (VirtualMachineConfigInfo)oVMWare.getObjectProperty(oVM, "config");
                        VirtualDevice[] devices = vminfo.hardware.device;

                        List<VMwareController> controllers = new List<VMwareController>();

                        foreach (VirtualDevice device in devices)
                        {
                            // Try to cast to Controller
                            try
                            {
                                VirtualController controller = (VirtualController)device;
                                VMwareController Controller = new VMwareController();
                                Controller.busNumber = controller.busNumber;
                                Controller.key = controller.key;
                                controllers.Add(Controller);
                            }
                            catch { }
                            // Try to cast to Disk
                            try
                            {
                                VirtualDisk disk = (VirtualDisk)device;
                                bool boolShared = false;
                                string strLunID = null;
                                try
                                {
                                    VirtualDiskFlatVer2BackingInfo backingNonShared = (VirtualDiskFlatVer2BackingInfo)disk.backing;
                                    boolShared = false;
                                    strLunID = "";
                                }
                                catch
                                {
                                    // Not flat disk, so don't worry about it
                                }
                                if (strLunID != null)
                                {
                                    VMwareDisk Disk = new VMwareDisk();
                                    Disk.controllerKey = disk.controllerKey;
                                    Disk.label = disk.deviceInfo.label;
                                    Disk.capacityInKB = disk.capacityInKB;
                                    Disk.unitNumber = disk.unitNumber;
                                    Disk.lunUuid = strLunID;
                                    Disk.Shared = boolShared;
                                    disks.Add(Disk);
                                }
                            }
                            catch { }
                        }

                        Storage oStorage = new Storage(0, dsn);
                        // Match up disks with controllers for bus numbers
                        foreach (VMwareDisk disk in disks)
                        {
                            disk.capacityInKB = ((disk.capacityInKB / 1024) / 1024);    // convert KB to GB
                            foreach (VMwareController controller in controllers)
                            {
                                if (disk.controllerKey == controller.key)
                                {
                                    disk.busNumber = controller.busNumber;
                                    disk.Name = disk.capacityInKB.ToString() + " GB - SCSI(" + disk.busNumber.ToString() + ":" + disk.unitNumber.ToString() + ") " + disk.label;

                                    oStorage.AddLunDisk(0, disk.busNumber, disk.unitNumber, disk.lunUuid);
                                    break;
                                }
                            }
                        }
                    }
                }
                catch { }
                finally
                {
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
            }

            return disks;
        }
    }
}
