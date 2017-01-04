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
//using PAObjectsLib;
using Vim25Api;
using System.Reflection;
using Tamir.SharpSsh;
using System.Net.NetworkInformation;
using NCC.ClearView.Application.Core.ClearViewWS;

namespace NCC.ClearView.Presentation.Web
{
    public partial class test_vmware_find : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        private string dsnServiceEditor = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ServiceEditorDSN"]].ConnectionString;
        private int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);

        private bool boolVMFound;
        VMWare oVMware;
        protected VimService _service;
        protected ServiceContent _sic;

        protected void Page_Load(object sender, EventArgs e)
        {
            oVMware = new VMWare(0, dsn);
        }

        protected void btnGo_Click(Object Sender, EventArgs e)
        {
            boolVMFound = false;
            Response.Write(DateTime.Now.ToString() + "<br/>");

            Variables oVariable = new Variables(999);
            System.Net.NetworkCredential oCredentials = new System.Net.NetworkCredential(oVariable.ADUser(), oVariable.ADPassword(), oVariable.Domain());
            ClearViewWebServices oWS = new ClearViewWebServices();
            oWS.Timeout = Int32.Parse(ConfigurationManager.AppSettings["WS_TIMEOUT"]);
            oWS.Credentials = oCredentials;
            //oWS.Url = oVariable.WebServiceURL();
            oWS.Url = "http://localhost:55030/ClearViewWebServices.asmx";

            string path = oWS.GetVMwarePath(txtVM.Text, "http://localhost:53744/", "OpHAa0tdJBAdeJALK65pMptAyK2SFcismq7QZNB9rMd4Fuhp6K8Zqx8z5gdA8KIGsi1YLTV7E57alz5cDMW5escYrHVTKbaReGLyDtNmYauuA8oFka6vXNCafcGe8cwD5q5OYHJk8Kgd7RyttpIJeO4BeLrAWJoWN1zkcJRNAWwnGxcyPnUSDXxqwEJmqjyzvuzfsmqTH5jssQ4QI3UJYFs0DiYHaEyrz71l86fC7blINf8PK2OwxYKZcIMjxSTcI13uZTSLYsMXuUmykj7h0b3Lybjzu5eori9WmN00kdHflWFrvo9pUMmH7s7XKsq3oyFlUAlGth6XDYzKG1dg5ZDP6CZ4Qcq2WN1XquH5dC6NPzdj2wrSok7x30prKrZq0eaZ6LtluhdD309GzPbuVINMQC4AfpuVMPXLxnnrcfxghEFd0S25pFFsAxjDR3gfXr0ndxilCTrPjGFm70JwwzGgkElaeTKj8ttZNsaCKQCFoZi337G0");
            while (path.Contains("\""))
                path = path.Replace("\"", "");
            string[] paths = path.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
            if (paths.Length >= 3)
            {
                string strConnect = oVMware.ConnectDEBUG("https://" + paths[0] + ".pncbank.com/sdk", 999, "");
                if (strConnect == "")
                {
                    _service = oVMware.GetService();
                    _sic = oVMware.GetSic();
                    //ManagedObjectReference datacenterRef = oVMWare.GetDataCenter();
                    //ManagedObjectReference vmFolderRef = oVMWare.GetVMFolder(datacenterRef);
                    //ManagedObjectReference clusterRef = oVMWare.GetCluster(strCluster);
                    //ManagedObjectReference resourcePoolRootRef = (ManagedObjectReference)oVMWare.getObjectProperty(clusterRef, "resourcePool");
                    ManagedObjectReference oComputer = GetVM(txtVM.Text, paths);
                    if (oComputer != null)
                    {
                        VirtualMachineConfigInfo oInfo = (VirtualMachineConfigInfo)oVMware.getObjectProperty(oComputer, "config");
                        Response.Write(oInfo.uuid + "<br/>");
                    }
                    if (_service != null)
                    {
                        //ServiceContent _sic = oVMware.GetSic();
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
                        //oLog.AddEvent(intAnswer, strName, strSerial, "Logged out of VMware", LoggingType.Information);
                    }
                }
            }
            Response.Write(DateTime.Now.ToString() + "<br/>");
        }

        public ManagedObjectReference GetVM(string _name)
        {
            ManagedObjectReference datacenterRef = oVMware.GetDataCenter();
            ManagedObjectReference vmFolderRef = (ManagedObjectReference)oVMware.getObjectProperty(datacenterRef, "vmFolder");
            ManagedObjectReference[] vmList = (ManagedObjectReference[])oVMware.getObjectProperty(vmFolderRef, "childEntity");
            return GetVMs(_name, vmList, true);
        }
        public ManagedObjectReference GetVM(string _name, string[] paths)
        {
            ManagedObjectReference vmRef = null;
            ManagedObjectReference rootFolder = _sic.rootFolder;
            ManagedObjectReference[] datacenters = (ManagedObjectReference[])oVMware.getObjectProperty(rootFolder, "childEntity");
            foreach (ManagedObjectReference datacenter in datacenters)
            {
                if (paths[1] == datacenter.Value)
                {
                    ManagedObjectReference vmFolderRef = (ManagedObjectReference)oVMware.getObjectProperty(datacenter, "vmFolder");
                    vmRef = GetVMs(_name, paths, (ManagedObjectReference[])oVMware.getObjectProperty(vmFolderRef, "childEntity"), 2);
                    break;
                }
            }
            return vmRef;
        }
        public ManagedObjectReference GetVMs(string _name, string[] paths, ManagedObjectReference[] vms, int count)
        {
            ManagedObjectReference vmRef = null;
            foreach (ManagedObjectReference vm in vms)
            {
                if (vm.Value == paths[count])
                {
                    if (vm.type == "VirtualMachine")
                        vmRef = vm;
                    else
                        vmRef = GetVMs(_name, paths, (ManagedObjectReference[])oVMware.getObjectProperty(vm, "childEntity"), count + 1);
                    break;
                }
            }
            return vmRef;
        }
        public ManagedObjectReference GetVMs(string _name, ManagedObjectReference[] vmList, bool first)
        {
            ManagedObjectReference vmRef = null;
            for (int ii = 0; ii < vmList.Length; ii++)
            {
                if (boolVMFound == true)
                    break;
                if (vmList[ii].type == "VirtualMachine")
                {
                    //Object[] vmProps = oVMware.getProperties(vmList[ii], new String[] { "name", "config.name", "config.template" });
                    //if (((String)vmProps[0]).ToUpper() == _name.ToUpper())
                    //if (oVMware.getObjectProperty(vmList[ii], "name").ToString().ToUpper() == _name.ToUpper())
                    PropertySpec pSpec = new PropertySpec();
                    pSpec.type = vmList[ii].type;
                    pSpec.pathSet = new String[] { "name" };
                    ObjectSpec oSpec = new ObjectSpec();
                    oSpec.obj = vmList[ii];
                    PropertyFilterSpec pfSpec = new PropertyFilterSpec();
                    pfSpec.propSet = new PropertySpec[] { pSpec };
                    pfSpec.objectSet = new ObjectSpec[] { oSpec };

                    VimService _service = oVMware.GetService();
                    ServiceContent _sic = oVMware.GetSic();
                    ObjectContent[] ocs = new ObjectContent[20];
                    ocs = _service.RetrieveProperties(_sic.propertyCollector, new PropertyFilterSpec[] { pfSpec });
                    string strName = "";
                    if (ocs.Length == 1 && ocs[0].propSet.Length == 1)
                        strName = ocs[0].propSet[0].val.ToString();
                    if (strName.ToUpper() == _name.ToUpper())
                    {
                        boolVMFound = true;
                        vmRef = vmList[ii];
                        //UpdateGuestVIM(_name, vmRef.Value);
                        break;
                    }
                }
                //else if (vmList[ii].type == "Folder")
                //{
                //    if (chkGroups.Checked || (String.IsNullOrEmpty(txtGroup.Text) == false && oVMware.getObjectProperty(vmList[ii], "name").ToString().ToUpper() == txtGroup.Text.ToUpper()))
                //    {
                //        vmRef = GetVMs(_name, (ManagedObjectReference[])oVMware.getObjectProperty(vmList[ii], "childEntity"), false);
                //        if (boolVMFound == true)
                //            break;
                //    }
                //}
            }
            return vmRef;
        }
    }
}
