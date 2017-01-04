<%@ Page Language="C#" Debug="false" %>
<%@ Import Namespace="System.Data.OleDb" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System" %>
<script runat="server">
    private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
    private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
    private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
    private int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
    private int intCountIP;
    private void Page_Load()
    {
        intCountIP = 0;
    }
    private void btnLoad2_Click(Object Sender, EventArgs e)
    {
        ResourceRequest oResourceRequest = new ResourceRequest(0, dsn);
        DataSet ds = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "select * FROM cv_ondemand_tasks_pending WHERE deleted = 0");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            int intWorkflowResource = Int32.Parse(dr["resourceid"].ToString());
            int intWorkflowParent = oResourceRequest.GetWorkflowParent(intWorkflowResource);
            DataSet dsWorkflow = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "select * from cv_resource_requests_workflow WHERE id = " + intWorkflowResource.ToString() + " AND deleted = 0");
            if (dsWorkflow.Tables[0].Rows.Count > 0)
                SqlHelper.ExecuteNonQuery(dsn, CommandType.Text, "UPDATE cv_resource_requests SET assignedby = -999, assigned = getdate() WHERE id = " + intWorkflowResource.ToString());
            else
                Response.Write("Not Found...WorkflowID = " + intWorkflowResource.ToString() + "<br/>");
            
        }
        Response.Write("Done IP = " + intCountIP.ToString() + "<br/>");
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
                <td><asp:Button ID="btnLoad2" runat="server" CssClass="default" Width="150" Text="IPs & Storage" OnClick="btnLoad2_Click" /></td>
            </tr>
        </table>
</form>
</body>
</html>