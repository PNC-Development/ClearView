<%@ Page Language="C#" Debug="false" %>
<%@ Import Namespace="System.Data.OleDb" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System" %>
<script runat="server">
    private int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
    private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
    private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
    private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
    private int intCount1 = 0;
    private int intCount2 = 0;
    private void Page_Load()
    {
    }
    private void btnGo_Click(Object Sender, EventArgs e)
    {
        string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=F:\\privateIPs.xls;Extended Properties=Excel 8.0;";

        Servers oServer = new Servers(0, dsn);
        Locations oLocation = new Locations(0, dsn);
        Classes oClass = new Classes(0, dsn);
        Environments oEnvironment = new Environments(0, dsn);
        IPAddresses oIPAddresses = new IPAddresses(0, dsnIP, dsn);
        
        // Update Used Addresses
        OleDbDataAdapter myCommand1 = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", strConn);
        DataSet ds1 = new DataSet();
        myCommand1.Fill(ds1, "ExcelInfo");
        foreach (DataRow dr in ds1.Tables[0].Rows)
        {
            if (dr[0].ToString().Trim() == "")
                break;
            int intVLAN = Int32.Parse(dr[0].ToString().Trim());
            int intAddress = Int32.Parse(dr[1].ToString().Trim());
            int intClass = Int32.Parse(dr[2].ToString().Trim());
            int intEnv = Int32.Parse(dr[3].ToString().Trim());
            int intAdd1 = Int32.Parse(dr[4].ToString().Trim());
            int intAdd2 = Int32.Parse(dr[5].ToString().Trim());
            int intAdd3 = Int32.Parse(dr[6].ToString().Trim());
            int intMin = Int32.Parse(dr[7].ToString().Trim());
            int intMax = Int32.Parse(dr[8].ToString().Trim());
            int intStart = Int32.Parse(dr[9].ToString().Trim());
            int intAdd4 = Int32.Parse(dr[10].ToString().Trim());
            string strName = dr[11].ToString().Trim();
            Response.Write(strName + "...");
            DataSet ds = oServer.Get(strName);
            if (ds.Tables[0].Rows.Count == 1)
            {
                Response.Write("Server Found...");
                int intServer = Int32.Parse(ds.Tables[0].Rows[0]["id"].ToString());
                DataSet dsIPs = oServer.GetIP(intServer, 0, 0, 1, 0);
                if (dsIPs.Tables[0].Rows.Count == 0)
                {
                    Response.Write("No IPs...");
                    int intVLANID = oIPAddresses.GetVlan(intVLAN, intClass, intEnv, intAddress);
                    if (intVLANID == 0)
                    {
                        Response.Write("Creating VLAN...");
                        oIPAddresses.AddVlan(intVLAN, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, "ImportedPrivate", "", intClass, intEnv, intAddress, 0, 1);
                        intVLANID = oIPAddresses.GetVlan(intVLAN, intClass, intEnv, intAddress);
                    }
                    if (intVLANID == 0)
                        Response.Write("Failed!");
                    else
                    {
                        int intNetwork = oIPAddresses.GetNetwork(intVLAN, intClass, intEnv, intAddress, intAdd1, intAdd2, intAdd3, intAdd4);
                        if (intNetwork == 0)
                        {
                            Response.Write("Creating Network...");
                            oIPAddresses.AddNetwork(intVLANID, intAdd1, intAdd2, intAdd3, intMin, intMax, "255.255.255.248", "", intStart, 8, 0, 0, "", 0, 0, "Private Cluster Address(es): " + oLocation.GetAddress(intAddress, "commonname") + ", " + oClass.Get(intClass, "name") + " - " + oEnvironment.Get(intEnv, "name"), 1);
                            intNetwork = oIPAddresses.GetNetwork(intVLAN, intClass, intEnv, intAddress, intAdd1, intAdd2, intAdd3, intAdd4);
                        }
                        if (intNetwork == 0)
                            Response.Write("Failed!");
                        else
                        {
                            oIPAddresses.UpdateNetworkCluster(intNetwork, 1);
                            Response.Write("Adding IP Address...");
                            int intIP = oIPAddresses.Add(intNetwork, intAdd1, intAdd2, intAdd3, intAdd4, -999);
                            oServer.AddIP(intServer, intIP, 0, 0, 1, 0);
                            Response.Write("Done!");
                        }
                    }
                }
                else
                    Response.Write("IPs Already Added! (" + dsIPs.Tables[0].Rows.Count.ToString() + ")");
            }
            else
                Response.Write("Server Not Found! (" + ds.Tables[0].Rows.Count.ToString() + ")");
            Response.Write("<br/>");
        }
        
    }
 </script>
<script type="text/javascript">
</script>
<html>
<head>
<title>IMPORT FUNCTIONS</title>
<link rel="stylesheet" type="text/css" href="/css/default.css" />
</head>
<body leftmargin="0" topmargin="0">
<form id="Form1" runat="server">
    <table>
        <tr>
            <td colspan="2" class="header">Import</td>
        </tr>
        <tr>
            <td colspan="2">&nbsp;</td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td><asp:Button ID="btnGo" runat="server" CssClass="default" Width="150" Text="Import Sheet" OnClick="btnGo_Click" /></td>
        </tr>
    </table>
</form>
</body>
</html>