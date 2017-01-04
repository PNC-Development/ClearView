<%@ Page Language="C#" %>
<%@ Import Namespace="Microsoft.ApplicationBlocks.Data" %>
<script runat="server">
    private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
    private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
    private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
    private string dsnZeus = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ZeusDSN"]].ConnectionString;
    private int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
    private ServerName oServerName;
    private Servers oServer;
    private Forecast oForecast;
    private Classes oClass;
    private Environments oEnvironment;
    private OperatingSystems oOperatingSystem;
    private ServicePacks oServicePack;
    private int intOK = 0;
    private int intInvalid = 0;
    private int intMissing1 = 0;
    private int intMissing2 = 0;

    protected void Page_Load(Object Sender, EventArgs e)
    {
        int intAsset = 12508;
        Asset oAsset = new Asset(0, dsnAsset, dsn);
        Response.Write("POWER = " + oAsset.GetVirtualConnectSetting(intAsset, "Power", intEnvironment));
        //Response.Write("MAC = " + oAsset.GetVirtualConnectMACAddress(23995, intEnvironment, 1, strScripts));
    }
    protected void btnLoad1_Click(Object Sender, EventArgs e)
    {
        oServer = new Servers(0, dsn);
        oForecast = new Forecast(0, dsn);
        string dsnDW = "data source=OHCLEIIS4333;uid=cvuser;password=l1ama;database=ClearViewDW";
        DataSet ds = SqlHelper.ExecuteDataset(dsnDW, CommandType.Text, "Select * from vwAssetsServers WHERE ASSETID NOT IN (Select ASSETID FROM dbo.vwServers  WHERE AssetStatusId=10) AND assetstatusid=10 and class <> 'NCB - Disaster Recovery'");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            int intAsset = Int32.Parse(dr["assetid"].ToString());
            DataSet dsAsset = SqlHelper.ExecuteDataset(dsnAsset, CommandType.Text, "SELECT * FROM cva_status WHERE assetid = " + intAsset.ToString() + " AND deleted = 0");
            if (dsAsset.Tables[0].Rows.Count == 1)
            {
                string strName = dsAsset.Tables[0].Rows[0]["name"].ToString();
                if (strName.Trim() != "")
                {
                    if (strName.IndexOf("(") > -1)
                        strName = strName.Substring(0, strName.IndexOf("("));
                    if (strName.IndexOf("-") > -1)
                        strName = strName.Substring(0, strName.IndexOf("-"));
                    DataSet dsName = oServer.Get(strName.Trim());
                    int intServer = 0;
                    bool boolContinue = true;
                    if (dsName.Tables[0].Rows.Count > 1)
                    {
                        foreach (DataRow drName in dsName.Tables[0].Rows)
                        {
                            int intServerOLD = intServer;
                            if (Int32.TryParse(dsName.Tables[0].Rows[0]["id"].ToString(), out intServer) == true)
                            {
                                if (intServerOLD > 0 && intServerOLD != intServer)
                                {
                                    boolContinue = false;
                                    break;
                                }
                            }

                        }
                    }
                    else if (dsName.Tables[0].Rows.Count < 1)
                        boolContinue = false;
                    if (boolContinue == true)
                    {
                        Int32.TryParse(dsName.Tables[0].Rows[0]["id"].ToString(), out intServer);
                        if (intServer > 0)
                        {
                            int intAnswer = 0;
                            Int32.TryParse(dsName.Tables[0].Rows[0]["answerid"].ToString(), out intAnswer);
                            int intClass = 0;
                            Int32.TryParse(oForecast.GetAnswer(intAnswer, "classid"), out intClass);
                            int intEnv = 0;
                            Int32.TryParse(oForecast.GetAnswer(intAnswer, "environmentid"), out intEnv);
                            if (intClass == 0)
                                intClass = 1;
                            if (intEnv == 0)
                                intEnv = 1;
                            if (intClass > 0 && intEnv > 0)
                            {
                                Response.Write("ADD = " + intServer.ToString() + ", " + intAnswer.ToString() + ", " + intClass.ToString() + ", " + intEnv.ToString() + "<br/>");
                                oServer.AddAsset(intServer, intAsset, intClass, intEnv, 0, 0);
                                intOK++;
                            }
                            else
                            {
                                Response.Write(".....INVALID = " + intServer.ToString() + ", " + intAnswer.ToString() + ", " + intClass.ToString() + ", " + intEnv.ToString() + " (" + strName + ")<br/>");
                                intInvalid++;
                            }
                        }
                    }
                    if (boolContinue == false || intServer == 0)
                    {
                        Response.Write("...MISSING = Found " + (intServer > 0 ? dsName.Tables[0].Rows.Count.ToString() : "-1") + " SERVER record(s) for name = " + strName + "<br/>");
                        intMissing1++;
                    }
                }
            }
            else
            {
                Response.Write("...MISSING = Did not find 1 STATUS record for assetid = " + intAsset.ToString() + "<br/>");
                intMissing2++;
            }
        }
        Response.Write("intOK = " + intOK.ToString() + "<br/>");
        Response.Write("intInvalid = " + intInvalid.ToString() + "<br/>");
        Response.Write("intMissing1 = " + intMissing1.ToString() + "<br/>");
        Response.Write("intMissing2 = " + intMissing2.ToString() + "<br/>");
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
        <table>
            <tr>
                <td colspan="2" class="header">Code Push Updates</td>
            </tr>
            <tr>
                <td colspan="2">&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td><asp:Button ID="btnLoad1" runat="server" CssClass="default" Width="150" Text="Change Workflow Data" OnClick="btnLoad1_Click" /></td>
            </tr>
        </table>
</form>
</body>
</html>