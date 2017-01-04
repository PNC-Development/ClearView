<%@ Page Language="C#" %>
<%@ Import Namespace="PAObjectsLib" %>
<script runat="server">
    private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
    private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
    private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
    private string dsnServiceEditor = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ServiceEditorDSN"]].ConnectionString;
    private int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
    private string strEnd = "";
    protected void Page_Load(Object Sender, EventArgs e)
    {
    }
    private void btnGo_Click(Object Sender, EventArgs e)
    {
        Response.Write("USE ClearView<br/>");
        Database(dsn, "ClearView");
        Response.Write("USE ClearViewAsset<br/>");
        Database(dsnAsset, "ClearViewAsset");
        Response.Write("USE ClearViewIP<br/>");
        Database(dsnIP, "ClearViewIP");
        Response.Write("USE ClearViewServiceEditor<br/>");
        Database(dsnServiceEditor, "ClearViewServiceEditor");
        Response.Write("USE ClearView<br/>");
        Response.Write(strEnd + "<br/>");
    }
    private void Database(string _dsn, string _database)
    {
        DataSet dsTD = SqlHelper.ExecuteDataset(_dsn, CommandType.Text, "select name from sys.tables order by name");
        foreach (DataRow drTD in dsTD.Tables[0].Rows)
        {
            bool boolRequest = false;
            bool boolService = false;
            bool boolItem = false;
            bool boolNumber = false;
            DataSet dsD = SqlHelper.ExecuteDataset(_dsn, CommandType.Text, "select c.name, c.max_length AS length, t.name AS type from sys.columns c LEFT OUTER JOIN sys.types t ON t.system_type_id = c.system_type_id AND t.user_type_id <= 255 where c.object_id =object_id('" + drTD["name"].ToString() + "')");
            foreach (DataRow drD in dsD.Tables[0].Rows)
            {
                if (drD["name"].ToString().ToUpper() == "REQUESTID")
                    boolRequest = true;
                else if (drD["name"].ToString().ToUpper() == "ITEMID")
                    boolItem = true;
                else if (drD["name"].ToString().ToUpper() == "NUMBER")
                    boolNumber = true;
                else if (drD["name"].ToString().ToUpper() == "SERVICEID")
                    boolService = true;
                
                if (boolRequest && boolItem && boolNumber && boolService)
                    break;
            }
            if (boolRequest && boolItem && boolNumber && boolService == false)
            {
                Response.Write("UPDATE " + drTD["name"].ToString() + " SET itemid = @itemid_NEW FROM " + drTD["name"].ToString() + " cvR INNER JOIN ClearView.dbo.cv_resource_requests cvRR ON cvR.requestid = cvRR.requestid AND cvR.itemid = @itemid_OLD AND cvR.number = cvRR.number" + "<br/>");
                Response.Write("UPDATE " + drTD["name"].ToString() + " SET itemid = @itemid_NEW FROM " + drTD["name"].ToString() + " cvR INNER JOIN ClearView.dbo.cv_request_forms cvRR ON cvR.requestid = cvRR.requestid AND cvR.itemid = @itemid_OLD AND cvR.number = cvRR.number" + "<br/>");
            }
            else if (boolRequest && boolItem && boolNumber && boolService)
                strEnd += "UPDATE " + drTD["name"].ToString() + " SET itemid = @itemid_NEW WHERE serviceid = @serviceid AND itemid = @itemid_OLD" + "<br/>";
        }
    }
    
</script>
<script type="text/javascript">
</script>
<html>
<head>
<title>LOAD</title>
<link rel="stylesheet" type="text/css" href="/css/default.css" />
<script src="/javascript/default.js"type="text/javascript"></script>
</head>
<body leftmargin="0" topmargin="0">
        <form id="Form1" runat="server">
            <table>
                <tr>
                    <td>&nbsp;</td>
                    <td><asp:Button ID="btnGo" runat="server" CssClass="default" Width="150" Text="Execute" OnClick="btnGo_Click" /></td>
                </tr>
            </table>
        </form>
</body>
</html>