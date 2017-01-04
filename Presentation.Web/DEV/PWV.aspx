<%@ Page Language="C#" %>
<%@ Import Namespace="PAObjectsLib" %>
<script runat="server">
    private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
    private int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);

    protected void Page_Load(Object Sender, EventArgs e)
    {
        AD oAD = new AD(0, dsn, intEnvironment);
        
        //oAD.CreateServer("AAAHEALY", "Description", "Notes", "OU=OUc_Special,OU=OUc_Srvs,OU=OUc_DmnCptrs,");

        if (oAD.Search("AAHEALY") == null)
            oAD.CreateUser("AAHEALY", "Steve", "Healy", "Abcd1234", "DisplayName", "Description", "Notes", "OU=OUc_InfoClients,", false, true);
        else
        {
            DirectoryEntry oEntry = oAD.Search("AAHEALY");
            Response.Write(oAD.SetCannotChangePassword(oEntry, false).ToString());
        }
        /*
        int intClusterID = 179;
        if (Request.QueryString["id"] != null)
            intClusterID = Int32.Parse(Request.QueryString["id"]);
        int intAnswerID = 11667;
        int intClass = 7;
        Servers oServer = new Servers(0, dsn);
        Classes oClass = new Classes(0, dsn);
        Cluster oCluster = new Cluster(0, dsn);

        string strClusterName = "";
        string strClusterNameSuffix = "";
        string strClusterAppInstance = "";
        DataSet dsClusters = oServer.GetClusters(intClusterID);
        string strClusterClass = "T";
        if (oClass.IsProd(intClass))
            strClusterClass = "P";
        else if (oClass.IsQA(intClass))
            strClusterClass = "Q";
        // Example: WDRDP103DZ and WDRDP104DZ and WDRDP105DZ...will only get the first two (03 and 04)
        ArrayList oArray = new ArrayList(2);
        for (int ii = 0; ii < 2; ii++)
        {
            int intClusterServerName = Int32.Parse(dsClusters.Tables[0].Rows[ii]["id"].ToString());
            string strClusterServerName = oServer.GetName(intClusterServerName, true);
            oArray.Add(strClusterServerName.ToUpper());
        }
        oArray.Sort();
        foreach (string strClusterServerName in oArray)
        {
            Response.Write("Server Name = " + strClusterServerName + "<br/>");
            if (strClusterName == "")
            {
                strClusterName = strClusterServerName.Substring(0, 8);
                strClusterNameSuffix = strClusterServerName.Substring(8, 1);
            }
            else
            {
                strClusterName += strClusterServerName.Substring(6, 2);
                strClusterAppInstance = strClusterServerName.Substring(0, 8);
            }
        }
        strClusterName += strClusterNameSuffix.ToLower();
        Response.Write("Cluster Name = " + strClusterName + "<br/>");
        //oCluster.UpdateName(intClusterID, strClusterName);
        string strClusterServiceAccount = "XA" + strClusterName;
        Response.Write("Cluster Service Account = " + strClusterServiceAccount + "<br/>");
        string strClusterPassword = strClusterName.Substring(5, 6).ToLower() + "$";
        Response.Write("Password = " + strClusterPassword + "<br/>");
        Response.Write("App Instance = " + strClusterAppInstance + "<br/>");
         * */
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