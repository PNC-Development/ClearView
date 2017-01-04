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
    public partial class test_vmware_guest_per_vlan : BasePage
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
            string strNetwork = "Desktop";
            if (string.IsNullOrEmpty(Request.QueryString["n"]) == false)
                strNetwork = Request.QueryString["n"];
            string strPortGroup = "dvPortGroupVLAN1000";
            if (string.IsNullOrEmpty(Request.QueryString["g"]) == false)
                strPortGroup = Request.QueryString["g"];

            Servers oServer = new Servers(0, dsn);
            OnDemand oOnDemand = new OnDemand(0, dsn);
            IPAddresses oIPAddresses = new IPAddresses(0, dsnIP, dsn);
            oVMWare = new VMWare(0, dsn);
            string strConnect = oVMWare.ConnectDEBUG("https://wssvt350a.pncbank.com/sdk", 999, "PNC-Pittsburgh");
            if (strConnect == "")
            {
                VimService _service = oVMWare.GetService();
                ServiceContent _sic = oVMWare.GetSic();
                ManagedObjectReference datacenterRef = oVMWare.GetDataCenter();
                ManagedObjectReference netFolderRef = (ManagedObjectReference)oVMWare.getObjectProperty(datacenterRef, "networkFolder");
                ManagedObjectReference[] netList = (ManagedObjectReference[])oVMWare.getObjectProperty(netFolderRef, "childEntity");
                ManagedObjectReference net = null;
                foreach (ManagedObjectReference network in netList)
                {
                    if (((string)oVMWare.getObjectProperty(network, "name")) == strNetwork)
                    {
                        net = network;
                        break;
                    }
                }

                if (net != null)
                {
                    ManagedObjectReference[] switchList = (ManagedObjectReference[])oVMWare.getObjectProperty(net, "childEntity");
                    foreach (ManagedObjectReference swit in switchList)
                    {
                        try
                        {
                            if (((string)oVMWare.getObjectProperty(swit, "name")) == strPortGroup)
                            {
                                DVPortgroupConfigInfo pgConfig = (DVPortgroupConfigInfo)oVMWare.getObjectProperty(swit, "config");
                                Response.Write("Num Ports = " + pgConfig.numPorts.ToString() + "<br/>");
                                ManagedObjectReference[] pgVM = (ManagedObjectReference[])oVMWare.getObjectProperty(swit, "vm");
                                Response.Write("Num Machines = " + pgVM.Length.ToString() + "<br/>");
                                int intAvailable = pgConfig.numPorts - pgVM.Length;
                                Response.Write("Ports Available = " + intAvailable.ToString() + "<br/>");
                                break;
                            }
                        }
                        catch { }
                    }
                }

                //    ManagedObjectReference[] oDatastores = oVMWare.GetDatastores("ohclexcv4004");
                //    foreach (ManagedObjectReference oDataStore in oDatastores)
                //    {
                //        DatastoreSummary oSummary = (DatastoreSummary)oVMWare.getObjectProperty(oDataStore, "summary");
                //        double oAvailable = double.Parse(oSummary.capacity.ToString());
                //        oAvailable = oAvailable / 1024.00;
                //        oAvailable = oAvailable / 1024.00;
                //        oAvailable = oAvailable / 1024.00;
                //        double oFree = double.Parse(oSummary.freeSpace.ToString());
                //        oFree = oFree / 1024.00;
                //        oFree = oFree / 1024.00;
                //        oFree = oFree / 1024.00;
                //        Response.Write(oSummary.name + " = " + oFree.ToString("F") + " GB / " + oAvailable.ToString("F") + " GB<br/>");
                //    }
            }
            else
                Response.Write("LOGIN error");
        }
    }
}
