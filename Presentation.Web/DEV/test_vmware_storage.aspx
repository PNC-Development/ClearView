<%@ Page Language="C#" Debug="false" EnableEventValidation="false" ValidateRequest="false" MasterPageFile="~/clearview.master" %>
<%@ Import Namespace="System.Security.Cryptography.X509Certificates" %>
<script runat="server">
    private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
    private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
    private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
    private string dsnZeus = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ZeusDSN"]].ConnectionString;
    private int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
    private int intVMWare = Int32.Parse(ConfigurationManager.AppSettings["VMWareModelID"]);
    private VMWare oVMWare;
    private void Page_Load()
    {
        string strError = "";
        Servers oServer = new Servers(0, dsn);
        OnDemand oOnDemand = new OnDemand(0, dsn);
        IPAddresses oIPAddresses = new IPAddresses(0, dsnIP, dsn);
        oVMWare = new VMWare(0, dsn);
        string strConnect = oVMWare.ConnectDEBUG("https://ohcleutl4001/sdk", 3, "Cleveland Ops");
        if (strConnect == "")
        {
            VimService _service = oVMWare.GetService();
            ServiceContent _sic = oVMWare.GetSic();
            ManagedObjectReference[] oDatastores = oVMWare.GetDatastores("ohclexcv4004");
            foreach (ManagedObjectReference oDataStore in oDatastores)
            {
                DatastoreSummary oSummary = (DatastoreSummary)oVMWare.getObjectProperty(oDataStore, "summary");
                double oAvailable = double.Parse(oSummary.capacity.ToString());
                oAvailable = oAvailable / 1024.00;
                oAvailable = oAvailable / 1024.00;
                oAvailable = oAvailable / 1024.00;
                double oFree = double.Parse(oSummary.freeSpace.ToString());
                oFree = oFree / 1024.00;
                oFree = oFree / 1024.00;
                oFree = oFree / 1024.00;
                Response.Write(oSummary.name + " = " + oFree.ToString("F") + " GB / " + oAvailable.ToString("F") + " GB<br/>");
            }
        }
        else
            Response.Write("LOGIN error");
    }
</script>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" Runat="Server">
<script type="text/javascript">
</script>
done!
</asp:Content>