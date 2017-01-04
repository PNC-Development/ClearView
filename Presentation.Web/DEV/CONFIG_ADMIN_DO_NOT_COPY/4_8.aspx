<%@ Page Language="C#" Debug="false" %>
<%@ Import Namespace="System.Data.OleDb" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System" %>
<script runat="server">
    private int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
    private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
    private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
    private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
    private int intCount = 0;
    private int intError = 0;
    private void Page_Load()
    {
        intCount = 0;
        intError = 0;
    }
    private void btnLoad1_Click(Object Sender, EventArgs e)
    {
        Classes oClass = new Classes(0, dsn);
        Environments oEnvironment = new Environments(0, dsn);
        DataSet ds = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "select * from cv_storage_3rd");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            int intID = Int32.Parse(dr["id"].ToString());
            string strClass = dr["classid"].ToString();
            string strEnv = dr["environmentid"].ToString();
            string strNew = "NONE";
            switch (strClass)
            {
                case "Development":
                    strNew = "NCB - Development";
                    break;
                case "Test":
                    strNew = "NCB - Test";
                    break;
                case "Production":
                    strNew = "NCB - Production";
                    break;
                case "Engineering":
                    strNew = "NCB - Engineering";
                    break;
                case "Disaster Recovery":
                    strNew = "NCB - Disaster Recovery";
                    break;
                default:
                    strNew = strClass;
                    break;
            }
            int intClass = 0;
            object oClassID = SqlHelper.ExecuteScalar(dsn, CommandType.Text, "SELECT id FROM cv_classs WHERE name = '" + strNew + "'");
            if (oClassID != null && oClassID.ToString() != "")
                intClass = Int32.Parse(oClassID.ToString());
            int intEnv = 0;
            object oEnvID = SqlHelper.ExecuteScalar(dsn, CommandType.Text, "SELECT id FROM cv_environment WHERE name = '" + strEnv + "'");
            if (oEnvID != null && oEnvID.ToString() != "")
                intEnv = Int32.Parse(oEnvID.ToString());

            int intAddress = 715;
            string strName = dr["servername"].ToString();
            if (strName.ToUpper().StartsWith("OHCIN") == true || strName.ToUpper().StartsWith("WD") == true)
                intAddress = 696;

            if (intClass == 0 || intEnv == 0)
            {
                if (strName.ToUpper().StartsWith("OHCIN") == true || strName.ToUpper().StartsWith("OHCLE") == true)
                {
                    if (strName.Length == 12)
                    {
                        string strLocation = strName.Substring(8, 1);
                        if (strLocation == "1")
                        {
                            intClass = 2;
                            intEnv = 1;
                        }
                        if (strLocation == "4")
                        {
                            intClass = 1;
                            intEnv = 1;
                        }
                    }
                }
                if (strName.ToUpper().StartsWith("WD") == true)
                {
                    if (strName.Length == 9)
                    {
                        string strLocation = strName.Substring(5, 1);
                    }
                }
            }
            SqlHelper.ExecuteNonQuery(dsn, CommandType.Text, "UPDATE cv_storage_3rd SET classid = '" + intClass.ToString() + "', environmentid = '" + intEnv.ToString() + "', addressid = '" + intAddress.ToString() + "' WHERE id = " + intID.ToString());
            intCount++;
        }
        Response.Write("Done = " + intCount.ToString() + "<br/>");
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
                <td colspan="2" class="header">4.8</td>
            </tr>
            <tr>
                <td colspan="2">&nbsp;</td>
            </tr>
            <tr>
                <td colspan="2">Click this before changing cv_storage_3rd fields</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td><asp:Button ID="btnLoad1" runat="server" CssClass="default" Width="150" Text="cv_storage_3rd" OnClick="btnLoad1_Click" /></td>
            </tr>
        </table>
</form>
</body>
</html>