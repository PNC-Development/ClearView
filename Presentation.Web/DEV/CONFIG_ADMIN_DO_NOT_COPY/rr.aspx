<%@ Page Language="C#" Debug="false" %>
<%@ Import Namespace="System.Data.OleDb" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System" %>
<script runat="server">
    private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
    private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
    private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
    private int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
    private int intCount1 = 0;
    private int intCount2 = 0;
    private int intCount3 = 0;
    private void Page_Load()
    {
        intCount1 = 0;
        intCount2 = 0;
        intCount3 = 0;
    }
    private void btnLoad1_Click(Object Sender, EventArgs e)
    {
        Services oService = new Services(0, dsn);
        ResourceRequest oResourceRequest = new ResourceRequest(0, dsn);
        ServiceRequests oServiceRequest = new ServiceRequests(0, dsn);
        DataSet ds = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "select * from cv_resource_requests WHERE deleted = 0");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            int intID = Int32.Parse(dr["id"].ToString());
            int intCounter = 0;
            int intUser = 0;
            DataSet dsWorkflow = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "select * from cv_resource_requests_workflow WHERE parent = " + intID.ToString() + " AND deleted = 0");
            foreach (DataRow drWorkflow in dsWorkflow.Tables[0].Rows)
            {
                intCounter++;
                int intUserNew = Int32.Parse(drWorkflow["userid"].ToString());
                if (intUserNew != intUser)
                    intUser = intUserNew;
                else
                {
                    SqlHelper.ExecuteNonQuery(dsn, CommandType.Text, "UPDATE cv_resource_requests_workflow SET deleted = 1, completed = getdate() WHERE id = " + drWorkflow["id"].ToString());
                    intCount1++;
                }
            }
            if (intUser == 0 || intCounter == 0)
            {
                if (dr["assignedby"].ToString() != "0" || dr["assigned"].ToString() != "")
                {
                    SqlHelper.ExecuteNonQuery(dsn, CommandType.Text, "UPDATE cv_resource_requests SET assignedby = 0, assigned = null WHERE id = " + intID.ToString());
                    intCount2++;
                }
            }
            else
            {
                if (dr["assignedby"].ToString() == "0" || dr["assigned"].ToString() == "")
                {
                    SqlHelper.ExecuteNonQuery(dsn, CommandType.Text, "UPDATE cv_resource_requests SET assignedby = -999, assigned = getdate() WHERE id = " + intID.ToString());
                    intCount3++;
                }
            }
        }
        Response.Write("RR Workflow Deleted = " + intCount1.ToString() + "<br/>");
        Response.Write("RR Not Assigned = " + intCount2.ToString() + "<br/>");
        Response.Write("RR Assigned = " + intCount3.ToString() + "<br/>");
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