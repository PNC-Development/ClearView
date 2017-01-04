<%@ Page Language="C#" Debug="false" EnableEventValidation="false" ValidateRequest="false" %>
<script runat="server">
    private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
    private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
    private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
    private string dsnZeus = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ZeusDSN"]].ConnectionString;
    private int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
    private string strConfig = "";
    private string strHost = "";
    private int intPort = 0;
    private string strTicket = "";
    private bool boolUsePNCNaming = (ConfigurationManager.AppSettings["USE_PNC_NAMING"] == "1");
    protected void Page_Load(object sender, EventArgs e)
    {
        VMWare oVMWare = new VMWare(0, dsn);
        oVMWare.Connect("OHCLEIIS403T");
        VimService _service = oVMWare.GetService();
        ManagedObjectReference _virtualMachine = oVMWare.GetVM("OHCLEIIS403T");
        VirtualMachineMksTicket _virtualMachineMksTicket = _service.AcquireMksTicket(_virtualMachine);
        strConfig = _virtualMachineMksTicket.cfgFile;
        strHost = _virtualMachineMksTicket.host;
        intPort = _virtualMachineMksTicket.port;
        strTicket = _virtualMachineMksTicket.ticket;

        //QuickMksAxCtl oTest = new QuickMksAxCtl();
        //string strHost = "ohclexnv400d.tstctr.ntl-city.net";
        //string strConfig = "/vmfs/volumes/481a2c8c-e080b4a8-4552-0017a4770400/OHCLEIIS403T/OHCLEIIS403T.vmx";
        //string strTicket = "52cceee7-aecb-58a9-9dea-9d80ac3045cf";
        //oTest.VerifySSLCertificates = false;
        //oTest.Connect(strHost, 902, strConfig, strTicket, strTicket);
        //oTest.SendCAD();
    }
</script>
<html>
<head id="Head1" runat="server">
<title id="Title1" runat="server">National City</title>
<link rel="stylesheet" type="text/css" href="/css/default.css" />
<script src="/javascript/default.js"type="text/javascript"></script>
<script type="text/javascript" src="/javascript/ajax.js"></script>
<script type="text/javascript" src="/javascript/both.js"></script>
<script type="text/javascript">
function LoadPreview() {
    var mks = document.getElementById("mks");
    var oConfig = '<%=strConfig %>';
    var oHost = '<%=strHost %>';
    var oPort = <%=intPort %>;
    var oTicket = '<%=strTicket %>';
    alert(mks.connect(oHost, oPort, oConfig, oTicket, oTicket)); 
}
</script>
</head>
<body bgcolor="#F8F8F8" leftmargin="0" topmargin="0" onload="LoadPreview();">
<form id="Form1" runat="server">
<p>&nbsp;</p>
<object id="mks" classid="CLSID:338095E4-1806-4ba3-AB51-38A3179200E9" codebase='http://ohcleutl4001/ui/plugin/msie/vmware-mks.cab#version=2,1,0,0' width="100%" height="100%">
Your browser is not supported.
</object>
</form>
</body>
</html>