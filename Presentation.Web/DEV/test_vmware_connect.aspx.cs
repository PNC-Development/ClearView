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

namespace NCC.ClearView.Presentation.Web
{
    public partial class test_vmware_connect : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        private string dsnServiceEditor = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ServiceEditorDSN"]].ConnectionString;
        private int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);

        private bool boolVMFound;
        VMWare oVMware;

        protected void Page_Load(object sender, EventArgs e)
        {
            oVMware = new VMWare(0, dsn);
        }

        protected void btnGo_Click(Object Sender, EventArgs e)
        {
            Log oLog = new Log(0, dsn);
            string name = "ldfpg100a";
            if (String.IsNullOrEmpty(Request.QueryString["n"]) == false)
                name = Request.QueryString["n"];

            boolVMFound = false;
            Response.Write(DateTime.Now.ToString() + "<br/>");
            string vc = "https://wssvt322a.pncbank.com/sdk";
            string dc = "PNC-Dalton";
            if (String.IsNullOrEmpty(Request.QueryString["dc"]) == false)
                dc = Request.QueryString["dc"];
            string strConnect = oVMware.ConnectDEBUG(vc, 999, dc);
            if (strConnect == "")
            {
                oLog.AddEvent(name, "DEBUG", "Connected to " + dc + " using " + vc, LoggingType.Debug);
                VimService _service = oVMware.GetService();
                ServiceContent _sic = oVMware.GetSic();
                oLog.AddEvent(name, "DEBUG", "Connection OK", LoggingType.Debug);
                ManagedObjectReference datacenterRef = oVMware.GetDataCenter();
                oLog.AddEvent(name, "DEBUG", "Connection to datacenterRef", LoggingType.Debug);
                ManagedObjectReference vmFolderRef = (ManagedObjectReference)getProperties(datacenterRef, "vmFolder");
                oLog.AddEvent(name, "DEBUG", "Connection to vmFolderRef", LoggingType.Debug);
                ManagedObjectReference[] vmList = (ManagedObjectReference[])getProperties(vmFolderRef, "childEntity");
                oLog.AddEvent(name, "DEBUG", "Connection to vmList (" + vmList.Length.ToString() + ")", LoggingType.Debug);
                for (int ii = 0; ii < vmList.Length; ii++)
                {
                    oLog.AddEvent(name, "DEBUG", "Checking " + vmList[ii].Value, LoggingType.Debug);
                    if (vmList[ii].type == "VirtualMachine")
                    {
                        oLog.AddEvent(name, "DEBUG", "Found! " + vmList[ii].Value, LoggingType.Debug);
                        try
                        {
                            VirtualMachineConfigInfo vmInfo = (VirtualMachineConfigInfo)getProperties(vmList[ii], "config");
                            oLog.AddEvent(name, "DEBUG", "vmInfo OK (" + vmInfo.uuid + ")", LoggingType.Debug);
                            Response.Write(vmInfo.name + " = " + vmInfo.uuid + "<br/>");
                        }
                        catch (Exception ex)
                        {
                            oLog.AddEvent(name, "DEBUG", "ERROR: " + ex.Message, LoggingType.Debug);
                            if (ex.InnerException != null)
                                oLog.AddEvent(name, "DEBUG", "ERROR: " + ex.InnerException.Message, LoggingType.Debug);
                        }
                        break;
                    }
                }
                oLog.AddEvent(name, "DEBUG", "Logging out", LoggingType.Debug);
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
                    oLog.AddEvent(name, "DEBUG", "Logged out", LoggingType.Debug);
                }
            }
            else
                oLog.AddEvent(name, "DEBUG", "Unable to connect - " + strConnect, LoggingType.Debug);
            Response.Write(DateTime.Now.ToString() + "<br/>");
        }
        public Object getProperties(ManagedObjectReference moRef, String propertyName)
        {
            PropertySpec pSpec = new PropertySpec();
            pSpec.type = moRef.type;
            pSpec.pathSet = new String[] { propertyName };

            ObjectSpec oSpec = new ObjectSpec();
            oSpec.obj = moRef;

            PropertyFilterSpec pfSpec = new PropertyFilterSpec();
            pfSpec.propSet = new PropertySpec[] { pSpec };
            pfSpec.objectSet = new ObjectSpec[] { oSpec };
            ObjectContent[] ocs = new ObjectContent[20];

            VimService _service = oVMware.GetService();
            ServiceContent _sic = oVMware.GetSic();
            ocs = _service.RetrieveProperties(_sic.propertyCollector, new PropertyFilterSpec[] { pfSpec });

            Object ret = new Object();
            for (int ii = 0; ocs != null && ii < ocs.Length; ++ii)
            {
                ObjectContent oc = ocs[ii];
                DynamicProperty[] dps = oc.propSet;
                for (int jj = 0; dps != null && jj < dps.Length; ++jj)
                {
                    DynamicProperty dp = dps[jj];
                    if (propertyName.Equals(dp.name))
                    {
                        ret = dp.val;
                        break;
                    }
                }
                if (ret != null)
                    break;
            }
            return ret;
        }

    }
}
