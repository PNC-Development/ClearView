<%@ Page Language="C#" Debug="false" EnableEventValidation="false" ValidateRequest="false" MasterPageFile="~/clearview.master" %>
<script runat="server">
    private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
    private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
    private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
    private string dsnZeus = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ZeusDSN"]].ConnectionString;
    private int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
    private Asset oAsset;
    protected void Page_Load(object sender, EventArgs e)
    {
        oAsset = new Asset(0, dsnAsset);
        Variables oVariable = new Variables(intEnvironment);
        //int intAsset = 12960;
        int intAsset = 11485;
        int intSlot = 1;
        string strProfile = ExecuteVirtualConnectHelper(intAsset, "show server enc0:" + intSlot.ToString(), intEnvironment);
        Response.Write(strProfile);
        //string strResults = oAsset.ExecuteVirtualConnectHelper(12960, "show profile " + strProfile, intEnvironment);
        //Response.Write(strResults);
        //string strMAC = strResult.Substring(strResult.IndexOf("MAC Address"));
        //strMAC = strMAC.Substring(strMAC.IndexOf("1 "));
        //string strVLAN1 = strMAC.Substring(0, strMAC.IndexOf("\n"));
        //string strVLAN2 = strMAC.Substring(strVLAN1.Length + 1);
        //strVLAN2 = strVLAN2.Substring(0, strVLAN2.IndexOf("\n"));
        //strVLAN1 = strVLAN1.Trim();
        //strVLAN2 = strVLAN2.Trim();
        //strVLAN1 = strVLAN1.Substring(strVLAN1.LastIndexOf(" ") + 1);
        //strVLAN2 = strVLAN2.Substring(strVLAN2.LastIndexOf(" ") + 1);
        //Response.Write(strVLAN1 + "<br/>");
        //Response.Write(strVLAN2 + "<br/>");
        
        //string strCommand1 = oSSH.RunCommand("set enet-connection Profile_1 1 Network=VLAN_250");
        //Response.Write(strCommand1 + "<br/>");
        //string strCommand2 = oSSH.RunCommand("set enet-connection Profile_15 1 PXE=enabled");
        //Response.Write(strCommand2 + "<br/>");
        //oSSH.Close();
        //Response.Write(oAsset.ExecuteVirtualConnectIP(12783, intEnvironment, "VLAN_192", 2, false, false));
        //Response.Write(oAsset.GetVirtualConnectMACAddress(10351, intEnvironment, 1));

        //string strHost = "10.5.52.34";
        //string strResults = oAsset.ExecuteVirtualConnect(strHost, oVariable.ILOUsername(), oVariable.ILOPassword(), "show server enc0:" + intSlot.ToString());
        //Response.Write(GetVirtualConnectSetting(strResults, "Power"));

        //Response.Write(oAsset.ExecuteVirtualConnectIP(12082, intEnvironment, "VLAN_200", 1, false, false, false));
        
    }
    public string GetVirtualConnectSetting(string _results, string _setting)
    {
        while (_results.Contains(": ") == true)
            _results = _results.Replace(": ", ":");
        while (_results.Contains(" :") == true)
            _results = _results.Replace(" :", ":");
        string strReturn = _results.Substring(_results.IndexOf(_setting + ":") + _setting.Length + 1);
        strReturn = strReturn.Substring(0, strReturn.IndexOf("\n"));
        return strReturn;
    }
    public string ExecuteVirtualConnectHelper(int _assetid, string _command, int _environment)
    {
        Variables oVariable = new Variables(_environment);
        int intEnclosure = Int32.Parse(oAsset.GetServerOrBlade(_assetid, "enclosureid"));
        string strHost = "";
        string strReturn = "";
        DataSet dsVirtualConnect = oAsset.GetEnclosureVCs(intEnclosure, 1);
        foreach (DataRow drVirtualConnect in dsVirtualConnect.Tables[0].Rows)
        {
            strHost = drVirtualConnect["virtual_connect"].ToString();
            //try
            //{
                strReturn = oAsset.ExecuteVirtualConnect(strHost, oVariable.ILOUsername(), oVariable.ILOPassword(), _command);
            //}
            //catch { }
            if (strReturn != "")
                break;
        }
        if (strReturn != "")
            strReturn += " (" + strHost + ")";
        return strReturn;
    }
</script>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" Runat="Server">
<table cellpadding="2" cellspacing="2" border="0">
    <tr>
        <td style="border:solid 1px #CCCCCC">Image</td>
    </tr>
</table>

</asp:Content>