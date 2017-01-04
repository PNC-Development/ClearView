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

namespace NCC.ClearView.Presentation.Web
{
    public partial class test_vmware_boot_order : BasePage
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
            Servers oServer = new Servers(0, dsn);
            OnDemand oOnDemand = new OnDemand(0, dsn);
            IPAddresses oIPAddresses = new IPAddresses(0, dsnIP, dsn);
            oVMWare = new VMWare(0, dsn);
            string strConnect = oVMWare.ConnectDEBUG("https://ohcleutl4001/sdk", 3, "Dalton");
            if (strConnect == "")
            {
                VimService _service = oVMWare.GetService();
                ServiceContent _sic = oVMWare.GetSic();
                string strName = "txpv00001a";
                ManagedObjectReference _vm_boot_order = oVMWare.GetVM(strName);

                OptionValue oValue = new OptionValue();
                oValue.key = "bios.bootDeviceClasses";
                // Set the PXE boot
                oValue.value = "allow:net";
                // Remove the PXE boot
                oValue.value = "";
                VirtualMachineConfigSpec _cs_boot_order = new VirtualMachineConfigSpec();
                _cs_boot_order.extraConfig = new OptionValue[] { oValue };

                ManagedObjectReference _task_boot_order = _service.ReconfigVM_Task(_vm_boot_order, _cs_boot_order);
                TaskInfo _inf_boot_order = (TaskInfo)oVMWare.getObjectProperty(_task_boot_order, "info");
                while (_inf_boot_order.state == TaskInfoState.running)
                    _inf_boot_order = (TaskInfo)oVMWare.getObjectProperty(_task_boot_order, "info");
                if (_inf_boot_order.state == TaskInfoState.success)
                    strResult = "PXE Boot Forced";
                else
                    strResult = "PXE Boot NOT Forced";
                Response.Write(strResult);
            }
            else
                Response.Write("LOGIN error");
        }
    }
}
