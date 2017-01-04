<%@ Page Language="C#" Debug="false" EnableEventValidation="false" ValidateRequest="false" MasterPageFile="~/clearview.master" %>
<script runat="server">
    private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
    private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
    private int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
    private int intCount = 0;
    private string strResult = "";
    private string strScripts = @"C:\Scripts\clearview\";
    private Asset oAsset;
    protected void Page_Load(object sender, EventArgs e)
    {
        oAsset = new Asset(0, dsnAsset, dsn);
        int intAsset = 0;
        Int32.TryParse(Request.QueryString["a"], out intAsset);
        if (Request.QueryString["m"] != null)
            Response.Write(GetVirtualConnectMACAddress(intAsset, intEnvironment, 1, strScripts));
        else if (Request.QueryString["i"] != null)
            Response.Write(ExecuteVirtualConnectIP(intAsset, intEnvironment, "VLAN_57", 2, false, false, true));
    }
    public string GetVirtualConnectMACAddress(int _assetid, int _environment, int _nic, string _file_location)
    {
        string strReturn = "";
        string strProfile = GetVirtualConnectSetting(_assetid, "Server Profile", _environment);
        if (strProfile != "")
        {
            string strResults = ExecuteVirtualConnectHelper(_assetid, "show profile " + strProfile, _environment);
            try
            {
                string strResult = strResults.Substring(strResults.IndexOf("Ethernet Network Connections"));
                strResult = strResult.Substring(strResult.IndexOf("==\n") + 3);
                strResult = strResult.Substring(strResult.IndexOf("==\n") + 3);
                strResult = strResult.Substring(0, strResult.IndexOf("\nFC SAN Connections"));
                char[] strSplit = { '\n' };
                string[] strMACs = strResult.Split(strSplit);
                for (int ii = 0; ii < strMACs.Length; ii++)
                {
                    if (strMACs[ii].Trim() != "" && _nic == (ii + 1))
                    {
                        string strMAC = strMACs[ii].Trim();
                        while (strMAC.Contains("  ") == true)
                            strMAC = strMAC.Replace("  ", " ");
                        string strResultValue = strMAC;
                        string strNumber = strResultValue.Substring(0, strResultValue.IndexOf(" "));
                        strResultValue = strResultValue.Substring(strResultValue.IndexOf(" ") + 1);
                        strResultValue = strResultValue.Substring(strResultValue.IndexOf(" ") + 1);
                        strResultValue = strResultValue.Substring(strResultValue.IndexOf(" ") + 1);
                        strResultValue = strResultValue.Substring(strResultValue.IndexOf(" ") + 1);
                        if (strResultValue.IndexOf(" ") > -1)
                            strResultValue = strResultValue.Substring(0, strResultValue.IndexOf(" "));
                        strReturn = strResultValue;
                        break;
                    }
                }
                return strReturn;
            }
            catch
            {
                StreamWriter filMAC = File.CreateText(_file_location + _assetid.ToString() + ".txt");
                filMAC.Write(strResults);
                filMAC.Flush();
                filMAC.Close();
                return "**ERROR**";
            }
        }
        else
            return "**ERROR**";
    }
    public string ExecuteVirtualConnectIP(int _assetid, int _environment, string _vlan, int _nic, bool _PXE_enabled, bool _PXE_disabled, bool _PXE_UseBios)
    {
        string strProfile = GetVirtualConnectSetting(_assetid, "Server Profile", _environment);
        return ExecuteVirtualConnectHelper(_assetid, "set enet-connection " + strProfile + " " + _nic.ToString() + " Network=" + _vlan + (_PXE_enabled == true || _PXE_disabled == true || _PXE_UseBios == true ? ";set enet-connection " + strProfile + " " + _nic.ToString() + " PXE=" + (_PXE_enabled ? "enabled" : (_PXE_disabled ? "disabled" : "UseBios")) : ""), _environment); ;
    }
    public string GetVirtualConnectSetting(int _assetid, string _setting, int _environment)
    {
        int intSlot = Int32.Parse(oAsset.GetServerOrBlade(_assetid, "slot"));
        string strResults = ExecuteVirtualConnectHelper(_assetid, "show server enc0:" + intSlot.ToString(), _environment);
        return GetVirtualConnectSetting(strResults, _setting);
    }
    public string GetVirtualConnectSetting(string _results, string _setting)
    {
        string strReturn = "";
        if (_results != "")
        {
            while (_results.Contains(": ") == true)
                _results = _results.Replace(": ", ":");
            while (_results.Contains(" :") == true)
                _results = _results.Replace(" :", ":");
            if (_results.Contains(_setting + ":") == true)
                strReturn = _results.Substring(_results.IndexOf(_setting + ":") + _setting.Length + 1);
            //if (strReturn.Contains("\n") == true)
                strReturn = strReturn.Substring(0, strReturn.IndexOf("\n"));
        }
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
            try
            {
                strReturn = ExecuteVirtualConnect(strHost, oVariable.ILOUsername(), oVariable.ILOPassword(), _command);
            }
            catch { }
            if (strReturn != "")
                break;
        }
        if (strReturn != "")
            strReturn += " (" + strHost + ")";
        return strReturn;
    }
    public string ExecuteVirtualConnect(string _host, string _username, string _password, string _commands)
    {
        SshExec oSSH = new SshExec(_host, _username, _password);
        oSSH.Connect();
        string strReturn = "";
        char[] strSplit = { ';' };
        string[] strCommand = _commands.Split(strSplit);
        for (int ii = 0; ii < strCommand.Length; ii++)
        {
            if (strCommand[ii].Trim() != "")
            {
                string strVirtualConnect = "Host = " + _host + ", Command = " + strCommand[ii].Trim();
                string strReturnTemp = oSSH.RunCommand(strCommand[ii].Trim());
                // Virtual Connect Manager not found at this IP address. Please use IP Address: 10.249.236.122 ; 
                if (strReturnTemp.ToUpper().Contains("VIRTUAL CONNECT MANAGER NOT FOUND AT THIS IP ADDRESS") == true)
                {
                    strReturn = "";
                    break;
                }
                else
                    strReturn += strReturnTemp + ";";
            }
        }
        oSSH.Close();
        return strReturn;
    }
</script>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" Runat="Server">
</asp:Content>