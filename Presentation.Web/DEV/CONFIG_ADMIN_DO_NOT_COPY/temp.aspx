<%@ Page Language="C#" Debug="false" %>
<%@ Import Namespace="System.Data.OleDb" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System" %>
<script runat="server">
    private int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
    private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
    private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
    private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
    private string dsnServiceEditor = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ServiceEditorDSN"]].ConnectionString;
    private int intCount = 0;
    private int intError = 0;
    private void Page_Load()
    {
        intCount = 0;
        intError = 0;
    }
    private void btnLoad1_Click(Object Sender, EventArgs e)
    {
        ServerName oServerName = new ServerName(0, dsn);
        Servers oServer = new Servers(0, dsn);
        Asset oAsset = new Asset(0, dsnAsset);
        
        // Get duplicates from cv_servers table (check servernames in cv_servers.nameid and compare with cva_status.name)
        Response.Write("<p><b>Duplicates in cv_servers table (comparing to cva_status.name)</b></p>");
        DataSet ds = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT * FROM cv_servers WHERE deleted = 0 and nameid > 0");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            int intServer = Int32.Parse(dr["id"].ToString());
            bool boolPNC = (dr["pnc"].ToString() == "1");
            int intName = Int32.Parse(dr["nameid"].ToString());
            string strName = "";
            if (boolPNC == true)
                strName = oServerName.GetNameFactory(intName, 0);
            else
                strName = oServerName.GetName(intName, 0);
            if (strName != "")
            {
                DataSet dsAsset = SqlHelper.ExecuteDataset(dsnAsset, CommandType.Text, "SELECT cva_status.* FROM cva_status INNER JOIN cva_assets ON cva_status.assetid = cva_assets.id AND cva_assets.deleted = 0 WHERE cva_status.deleted = 0 and cva_status.name = '" + strName + "'");
                if (dsAsset.Tables[0].Rows.Count > 1)
                {
                    int intCount = 0;
                    Response.Write(strName + "<br/>");
                    foreach (DataRow drAsset in dsAsset.Tables[0].Rows)
                    {
                        intCount++;
                        int intAsset = Int32.Parse(drAsset["assetid"].ToString());
                        if (intAsset > 0 && oAsset.Get(intAsset, "serial").StartsWith("VMware") == true && intCount < dsAsset.Tables[0].Rows.Count)
                        {
                            Response.Write("DELETE<br/>");
                            oServer.DeleteAsset(intServer, intAsset);
                            oAsset.Delete(intAsset);
                        }
                        else
                        {
                            string strILO = oAsset.GetServerOrBlade(intAsset, "ilo");
                            if (strILO != "")
                                strILO = "<a href=\"https://" + strILO + "\" target=\"_blank\">" + strILO + "</a>";
                            Response.Write("&nbsp;&nbsp;-&nbsp;" + drAsset["name"].ToString() + " (serverID: " + intServer.ToString() + ", assetID: " + intAsset.ToString() + " = " + oAsset.Get(intAsset, "serial") + " = " + strILO + ")<br/>");
                        }
                    }
                    Response.Write("<br/>");
                }
            }
        }
        Response.Write("<p>&nbsp;</p>");


        // Get duplicates from cva_status table (check servernames in cva_status.name and compare with cv_servers.nameid)
        Response.Write("<p><b>Duplicates in cva_status table (comparing to cv_servers)</b></p>");
        int intMax = (int)SqlHelper.ExecuteScalar(dsn, CommandType.Text, "SELECT MAX(nameid) FROM cv_servers WHERE deleted = 0");
        for (int ii = 1; ii <= intMax; ii++)
        {
            DataSet dsName = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT * FROM cv_servers WHERE deleted = 0 and nameid = " + ii.ToString());
            if (dsName.Tables[0].Rows.Count > 1)
            {
                Response.Write("NameID: " + ii.ToString() + "<br/>");
                foreach (DataRow drName in dsName.Tables[0].Rows)
                {
                    int intServer = Int32.Parse(drName["id"].ToString());
                    bool boolPNC = (drName["pnc"].ToString() == "1");
                    int intName = Int32.Parse(drName["nameid"].ToString());
                    string strName = "";
                    if (boolPNC == true)
                        strName = oServerName.GetNameFactory(intName, 0);
                    else
                        strName = oServerName.GetName(intName, 0);
                    Response.Write("&nbsp;&nbsp;-&nbsp;" + strName + " (serverID: " + intServer.ToString() + ")<br/>");
                }
                Response.Write("<br/>");
            }
        }
        Response.Write("<p>&nbsp;</p>");
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
                <td colspan="2" class="header">Fix</td>
            </tr>
            <tr>
                <td colspan="2">&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td><asp:Button ID="btnLoad1" runat="server" CssClass="default" Width="150" Text="Load" OnClick="btnLoad1_Click" /></td>
            </tr>
        </table>
</form>
</body>
</html>