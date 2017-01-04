<%@ Page Language="C#" Debug="true" %>
<%@ Import Namespace="NCC.ClearView.Application.Core.Proteus" %>
<script runat="server">
    private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
    private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
    private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
    private string dsnServiceEditor = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ServiceEditorDSN"]].ConnectionString;
    private int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);

    protected void Page_Load(object sender, EventArgs e)
    {
        Variables oVariable = new Variables(intEnvironment);
        System.Net.NetworkCredential oCredentialsDNS = new System.Net.NetworkCredential(oVariable.ADUser(), oVariable.ADPassword(), oVariable.Domain());
        ProteusAPI oProteusAPI = new ProteusAPI();
        oProteusAPI.CookieContainer = new CookieContainer();
        oProteusAPI.Credentials = oCredentialsDNS;
        oProteusAPI.Url = oVariable.BlueCatWebService(dsn);

        string cfg_proteus_api_configuration = oVariable.BlueCatConfiguration();
        string cfg_proteus_api_view = oVariable.BlueCatView();
        string strStaging = "IP Address held for server staging via Ciora, Healy, Whelan";

        //string strName = "bobpeacock.pncbank.com";
        string strName = strStaging;
        string strIP = "10.33.4.11";
        string strMAC = "00-11-22-33-44-99";
        bool boolCreate = false;
        bool boolDelete = false;

        if (Request.QueryString["n"] != null)
            strName = Request.QueryString["n"];
        if (Request.QueryString["i"] != null)
            strIP = Request.QueryString["i"];
        if (Request.QueryString["m"] != null)
            strMAC = Request.QueryString["m"];
        if (Request.QueryString["c"] != null)
            boolCreate = true;
        if (Request.QueryString["d"] != null)
            boolDelete = true;
        
        // Login
        oProteusAPI.login("admin", "admin");


        APIEntity oConfiguration = oProteusAPI.getEntityByName(0, cfg_proteus_api_configuration, "Configuration");
        APIEntity oView = oProteusAPI.getEntityByName(oConfiguration.id, cfg_proteus_api_view, "View");

        /*
        APIEntity[] oBlocks = oProteusAPI.getEntities(oConfiguration.id, "IP4Block", 0, 1000);
        long ID = 0;
        foreach (APIEntity oBlock in oBlocks)
        {
            if (ID > 0)
                break;
            if (oBlock.properties != null && oBlock.properties.Contains("10.0.0") == true)
            {
                APIEntity[] oBlock2s = oProteusAPI.getEntities(oBlock.id, "IP4Block", 0, 1000);
                if (oBlock2s != null)
                {
                    foreach (APIEntity oBlock2 in oBlock2s)
                    {
                        if (ID > 0)
                            break;
                        if (oBlock2.properties != null && oBlock2.properties.Contains("10.33.0") == true)
                        {
                            APIEntity[] oNetworks = oProteusAPI.getEntities(oBlock2.id, "IP4Network", 0, 1000);
                            if (oNetworks != null)
                            {
                                foreach (APIEntity oNetwork in oNetworks)
                                {
                                    if (ID > 0)
                                        break;
                                    if (oNetwork.properties != null && oNetwork.properties.Contains("10.33.4") == true)
                                    {
                                        
                                        APIEntity oAddress = oProteusAPI.getEntityByName(oNetwork.id, "healytest.pncbank.com", "IP4Address");
                                        if (oAddress.id > 0)
                                        {
                                            ID = oAddress.id;
                                        }
                                        
                                        APIEntity oHost = oProteusAPI.getEntityByName(oNetwork.id, "healytest.pncbank.com", "HostRecord");
                                        if (oHost.id > 0)
                                        {
                                            ID = oHost.id;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        */

        //string strOutput = oProteusAPI.getSystemInfo();

        //APIEntity[] oTest = oProteusAPI.searchByObjectTypes("healytest", "HostRecord", 0, 10);
        //if (oTest.Length > 0)
        //{
        //    oProteusAPI.delete(oTest[0].id);
        //}
        
        
          // 10.12.36.0
         
        APIEntity[] oSearchIP = oProteusAPI.searchByCategory(strIP, "IP4_OBJECTS", 0, 1000);
        Response.Write(oSearchIP.Length.ToString() + " record(s) for the IP Address " + strIP + "<br/>");

        //APIEntity[] oSearchName = oProteusAPI.searchByCategory(strName, "IP4_OBJECTS", 0, 1000);
        //Response.Write(oSearchName.Length.ToString() + " record(s) for the Name " + strName + "<br/>");

        //if (oSearchName.Length > 0)
        //{
        //    APIEntity oEntity = oSearchName[0];
        //    APIEntity[] oEntities = oProteusAPI.getLinkedEntities(oEntity.id, "HostRecord", 0, 100);
        //    string[] strProperties = oEntity.properties.Split('|');

        //    if (oSearchIP.Length > 0)
        //    {
        //        APIEntity oEntity2 = oSearchIP[0];
        //        APIEntity[] oEntities2 = oProteusAPI.getLinkedEntities(oEntity2.id, "HostRecord", 0, 100);
        //        Response.Write("Address = " + GetBluecatProperty(oEntity2, "address"));
        //    }

        //    if (boolDelete == true)
        //    {
        //        // Delete
        //        oProteusAPI.delete(oEntity.id);
        //    }

        //    if (strName == strStaging && boolCreate == true)
        //    {
        //        string properties = "NWS=clearview|requestor=clearview|modified-by=xacview|modified=" + DateTime.Now.ToString() + "|name=" + strName + "|";
        //        oProteusAPI.assignIP4Address(oConfiguration.id, strIP, "", "", "MAKE_STATIC", properties);
        //    }
        //}
        //else if (oSearchIP.Length > 0)
        //{
            if (oSearchIP.Length == 1)
            {
                Response.Write("Found 1" + "<br/>");
                if (oSearchIP[0] != null && oSearchIP[0].name != null && oSearchIP[0].name.ToUpper().Contains("STAGING") == false)
                {
                    Response.Write("Valid" + "<br/>");
                    //oReturn.Entity = oSearch[0];
                    //oReturn.Count = 1;
                }
                APIEntity[] oEntities = oProteusAPI.getLinkedEntities(oSearchIP[0].id, "HostRecord", 0, 100);
            }
            APIEntity oEntity = oSearchIP[0];
            /*
            oEntity.name = strName;
            string properties = "NWS=clearview|requestor=clearview|modified-by=xacview|modified=" + DateTime.Now.ToString() + "|name=" + strName + "|";
            oEntity.properties = properties;
            oProteusAPI.update(oEntity);
            oProteusAPI.addHostRecord(oView.id, strName, strIP, -1, "");    // no properties for HostRecord (only IP Record)
            */
        //}
        //else if (boolCreate == true)
        //{
        //    string hostInfo = strName + "," + oView.id + ",true,false";
        //    string properties = "NWS=clearview|requestor=clearview|modified-by=xacview|modified=" + DateTime.Now.ToString() + "|name=" + strName + "|";
        //    //oProteusAPI.assignIP4Address(oConfiguration.id, strIP, strMAC, hostInfo, "MAKE_STATIC", properties);
        //    oProteusAPI.addHostRecord(oView.id, strName, strIP, -1, properties);

        //}
        
         
         
        
        
        // Log out
        oProteusAPI.logout();

    }
    public string GetBluecatProperty(APIEntity _entity, string _property)
    {
        string strReturn = "";
        string[] _properties = _entity.properties.Split('|');
        foreach (string strProperty in _properties)
        {
            if (strProperty.ToUpper().StartsWith(_property.ToUpper()) == true)
            {
                if (strProperty.Contains("=") == true)
                {
                    strReturn = strProperty.Substring(strProperty.IndexOf("=") + 1);
                    break;
                }
            }
        }
        return strReturn;
    }
</script>
<script type="text/javascript">
</script>
<html>
<head>
<title>LOAD</title>
<link rel="stylesheet" type="text/css" href="/css/default.css" />
</head>
<body leftmargin="0" topmargin="0">
<form id="Form1" runat="server">
</form>
</body>
</html>