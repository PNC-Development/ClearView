<%@ Page Language="C#" Debug="true" %>
<%@ Import Namespace="PAObjectsLib" %>
<%@ Import Namespace="System.Collections.Generic" %>
<script runat="server">
    public enum XenConfigType
    {
        Add, Remove, MaintOn, MaintOff
    }
    public class XenConfig
    {
        private int _AddressID;
        public int AddressID
        {
            get { return _AddressID; }
            set { _AddressID = value; }
        }
        private string _MachineCatalogName;
        public string MachineCatalogName
        {
            get { return _MachineCatalogName; }
            set { _MachineCatalogName = value; }
        }
        private string _DesktopDeliveryGroup;
        public string DesktopDeliveryGroup
        {
            get { return _DesktopDeliveryGroup; }
            set { _DesktopDeliveryGroup = value; }
        }
        private string _ADGroup;
        public string ADGroup
        {
            get { return _ADGroup; }
            set { _ADGroup = value; }
        }
        private string[] _Controllers;
        public string[] Controllers
        {
            get { return _Controllers; }
            set { _Controllers = value; }
        }
        public XenConfig(int _AddressID, string _MachineCatalogName, string _DesktopDeliveryGroup, string _ADGroup, string[] _Controllers)
        {
            AddressID = _AddressID;
            MachineCatalogName = _MachineCatalogName;
            DesktopDeliveryGroup = _DesktopDeliveryGroup;
            ADGroup = _ADGroup;
            Controllers = _Controllers;
        }
    }

    private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
    private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
    private int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
    private VMWare oVMWare;
    protected void Page_Load(Object Sender, EventArgs e)
    {
        oVMWare = new VMWare(0, dsn);
        if (!IsPostBack)
        {
            ddlVirtualCenter.DataTextField = "name";
            ddlVirtualCenter.DataValueField = "url";
            ddlVirtualCenter.DataSource = oVMWare.GetVirtualCenters(1);
            ddlVirtualCenter.DataBind();
        }
    }
    private void btnGo_Click(Object Sender, EventArgs e)
    {
            VMWare oVMWare = new VMWare(0, dsn);
            List<XenConfig> XenConfigs = new List<XenConfig>();
            XenConfigs.Add(new XenConfig(715, "Windows 7 VDI CleOps", "Windows 7 VDI CleOps", "GSLcxiSP_CXI_Windows7_VDI_CleOps", new string[] { "WDCXI109A", "WDCXI110A" }));
            XenConfigs.Add(new XenConfig(1675, "Windows 7 VDI Summit", "Windows 7 VDI Summit", "GSLcxiSP_CXI_Windows7_VDI_Summit", new string[] { "WDCXI109A", "WDCXI110A" }));
            //XenConfigs.Add(new XenConfig(715, "Windows 7 VDI CleOps", "Windows 7 VDI CleOps", "GSLcxiSP_CXI_Windows7_VDI_CleOps", "@(\"WDCXI109A,WDCXI110A\")"));
            //XenConfigs.Add(new XenConfig(1675, "Windows 7 VDI Summit", "Windows 7 VDI Summit", "GSLcxiSP_CXI_Windows7_VDI_Summit", "@(\"WDCXI109A,WDCXI110A\")"));
            int addressid = Int32.Parse(ddlLocation.SelectedItem.Value);
            //XenConfig xenConfig = XenConfigs.Find(o => o.AddressID == addressid);
            XenConfig xenConfig = XenConfigs[0];
            foreach (XenConfig xen in XenConfigs)
            {
                if (xen.AddressID == addressid)
                {
                    xenConfig = xen;
                    break;
                }
            }
            switch (ddlAction.SelectedItem.Value)
            {
                case "A":
                    Response.Write(XenDesktop(txtName.Text, Request.PhysicalApplicationPath + "scripts", oVMWare, ddlVirtualCenter.SelectedItem.Value, xenConfig, XenConfigType.Add, "The workstation was successfully registered in Xen Desktop " + xenConfig.MachineCatalogName));
                    break;
                case "N":
                    Response.Write(XenDesktop(txtName.Text, Request.PhysicalApplicationPath + "scripts", oVMWare, ddlVirtualCenter.SelectedItem.Value, xenConfig, XenConfigType.MaintOn, "The workstation was successfully registered in Xen Desktop " + xenConfig.MachineCatalogName));
                    break;
                case "F":
                    Response.Write(XenDesktop(txtName.Text, Request.PhysicalApplicationPath + "scripts", oVMWare, ddlVirtualCenter.SelectedItem.Value, xenConfig, XenConfigType.MaintOff, "The workstation was successfully registered in Xen Desktop " + xenConfig.MachineCatalogName));
                    break;
                case "R":
                    Response.Write(XenDesktop(txtName.Text, Request.PhysicalApplicationPath + "scripts", oVMWare, ddlVirtualCenter.SelectedItem.Value, xenConfig, XenConfigType.Remove, "The workstation was successfully registered in Xen Desktop " + xenConfig.MachineCatalogName));
                    break;
            }
    }
    private string XenDesktop(string _name, string _scripts, VMWare oVMWare, string _virtual_center_url, XenConfig config, XenConfigType type, string _results)
    //private string XenDesktop(int _answerid, string _name, string _serial, string _scripts, VMWare oVMWare, string _virtual_center_url, XenConfig config, XenConfigType type, string _results)
    {
        string error = "";
        System.Collections.Generic.List<PowershellParameter> powershell = new List<PowershellParameter>();
        Powershell oPowershell = new Powershell();
        switch (type)
        {
            case XenConfigType.Add:
                //oLog.AddEvent(_answerid, _name, _serial, "Registering in Xen Desktop...(" + _scripts + "\\XenWorkstationCommands.ps1)", LoggingType.Debug);
                //ManagedObjectReference _vm_xen = oVMWare.GetVM(_name);
                //VirtualMachineConfigInfo _vminfo_xen = (VirtualMachineConfigInfo)oVMWare.getObjectProperty(_vm_xen, "config");
                //string UUID = _vminfo_xen.uuid;
                //string UUID = "420f4ed0-627e-f575-5ca4-c522de34c933";
                string UUID = txtUUID.Text;
                powershell.Add(new PowershellParameter("Add", null));
                powershell.Add(new PowershellParameter("WorkstationName", _name));
                powershell.Add(new PowershellParameter("DomainName", "PNCNT"));
                powershell.Add(new PowershellParameter("WorkstationUUID", UUID));
                powershell.Add(new PowershellParameter("VirtualCenterAddress", _virtual_center_url));
                powershell.Add(new PowershellParameter("GroupName", config.ADGroup));
                powershell.Add(new PowershellParameter("MachineCatalogueName", config.MachineCatalogName));
                powershell.Add(new PowershellParameter("DesktopGroupName", config.DesktopDeliveryGroup));
                powershell.Add(new PowershellParameter("ControllerNames", config.Controllers));
                break;
            case XenConfigType.MaintOn:
                //oLog.AddEvent(_answerid, _name, _serial, "Enabling maintenance mode in Xen Desktop...(" + _scripts + "\\XenWorkstationCommands.ps1)", LoggingType.Debug);
                powershell.Add(new PowershellParameter("TurnOnMaintenanceMode", null));
                powershell.Add(new PowershellParameter("WorkstationName", _name));
                powershell.Add(new PowershellParameter("DomainName", "PNCNT"));
                powershell.Add(new PowershellParameter("ControllerNames", config.Controllers));
                break;
            case XenConfigType.Remove:
                //oLog.AddEvent(_answerid, _name, _serial, "Removing machine from Xen Desktop...(" + _scripts + "\\XenWorkstationCommands.ps1)", LoggingType.Debug);
                powershell.Add(new PowershellParameter("Remove", null));
                powershell.Add(new PowershellParameter("WorkstationName", _name));
                powershell.Add(new PowershellParameter("DomainName", "PNCNT"));
                powershell.Add(new PowershellParameter("MachineCatalogueName", config.MachineCatalogName));
                powershell.Add(new PowershellParameter("DesktopGroupName", config.DesktopDeliveryGroup));
                powershell.Add(new PowershellParameter("ControllerNames", config.Controllers));
                break;
            case XenConfigType.MaintOff:
                //oLog.AddEvent(_answerid, _name, _serial, "Disabling maintenance mode in Xen Desktop...(" + _scripts + "\\XenWorkstationCommands.ps1)", LoggingType.Debug);
                powershell.Add(new PowershellParameter("TurnOffMaintenanceMode", null));
                powershell.Add(new PowershellParameter("WorkstationName", _name));
                powershell.Add(new PowershellParameter("DomainName", "PNCNT"));
                powershell.Add(new PowershellParameter("ControllerNames", config.Controllers));
                break;
        }
        List<List<PowershellParameter>> results = oPowershell.Execute(_scripts + "\\XenWorkstationCommands.ps1", powershell);
        bool PowerShellError = false;
        foreach (List<PowershellParameter> psobject in results)
        {
            foreach (PowershellParameter result in psobject)
            {
                //oLog.AddEvent(_name, _serial, "PSOBJECT: " + result.Name + " = " + result.Value, LoggingType.Information);
                if (result.Name == "ResultCode" && result.Value.ToString() != "0")
                    PowerShellError = true;
                else if (result.Name == "Message" && PowerShellError)
                    error = result.Value.ToString();
            }
        }
        //if (error == "")
        //    oLog.AddEvent(_answerid, _name, _serial, _results, LoggingType.Debug);
        return error;
    }
</script>
<script type="text/javascript">
</script>
<html>
<head>
<title>Xen Desktop</title>
<link rel="stylesheet" type="text/css" href="/css/default.css" />
</head>
<body leftmargin="0" topmargin="0">
<form id="Form1" runat="server">
    <table>
        <tr>
            <td colspan="2" class="header">Xen Desktop Registration</td>
        </tr>
        <tr>
            <td>Name:</td>
            <td><asp:TextBox ID="txtName" runat="server" Width="300" /></td>
        </tr>
        <tr>
            <td>UUID:</td>
            <td><asp:TextBox ID="txtUUID" runat="server" Width="300" /></td>
        </tr>
        <tr>
            <td>Virtual Center:</td>
            <td><asp:DropDownList ID="ddlVirtualCenter" runat="server" Width="300" /></td>
        </tr>
        <tr>
            <td>Location:</td>
            <td>
                <asp:DropDownList ID="ddlLocation" runat="server" Width="300">
                    <asp:ListItem Text="Cleveland" Value="715" />
                    <asp:ListItem Text="Summit" Value="1675" />
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>Action:</td>
            <td>
                <asp:DropDownList ID="ddlAction" runat="server" Width="300">
                    <asp:ListItem Text="Add" Value="A" />
                    <asp:ListItem Text="MaintOn" Value="N" />
                    <asp:ListItem Text="MaintOff" Value="F" />
                    <asp:ListItem Text="Remove" Value="R" />
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td><asp:Button ID="btnGo" runat="server" CssClass="default" Width="150" Text="Import Sheet" OnClick="btnGo_Click" /></td>
        </tr>
    </table>
</form>
</body>
</html>