<%@ Page Language="C#" Debug="false" %>
<%@ Import Namespace="System.Data.OleDb" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System" %>
<script runat="server">
    private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
    private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
    private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
    private int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
    private int intCount = 0;
    private void Page_Load()
    {
        intCount = 0;
    }
    private void btnLoad1_Click(Object Sender, EventArgs e)
    {
        Services oService = new Services(0, dsn);
        ResourceRequest oResourceRequest = new ResourceRequest(0, dsn);
        ServiceRequests oServiceRequest = new ServiceRequests(0, dsn);
        DataSet ds = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "select * from cv_resource_requests");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            int intID = Int32.Parse(dr["id"].ToString());
            //string strName = dr["name"].ToString();
            //if (strName == "")
            //{
            //    string strRequest = dr["requestid"].ToString();
            //    if (strRequest != "")
            //    {
            //        int intRequest = Int32.Parse(strRequest);
            //        strName = oServiceRequest.Get(intRequest, "name");
            //    }
            //}
            //int intNew = oResourceRequest.AddWorkflow(intID, 0, strName, Int32.Parse(dr["userid"].ToString()), Int32.Parse(dr["devices"].ToString()), double.Parse(dr["allocated"].ToString()), Int32.Parse(dr["status"].ToString()), 0);
            //SqlHelper.ExecuteNonQuery(dsn, CommandType.Text, "update cv_resource_requests_workflow set used = " + dr["used"].ToString() + ", modifiedby = " + dr["userid"].ToString() + ", created = " + (dr["created"].ToString() == "" ? "NULL" : "'" + dr["created"].ToString() + "'") + ", modified = " + (dr["updated"].ToString() == "" ? "NULL" : "'" + dr["updated"].ToString() + "'") + ", completed = " + (dr["completed"].ToString() == "" ? "NULL" : "'" + dr["completed"].ToString() + "'") + " where id = " + intNew.ToString());
            int intNew = (int)SqlHelper.ExecuteScalar(dsn, CommandType.Text, "SELECT id FROM cv_resource_requests_workflow WHERE parent = " + intID.ToString());
            //// Hours
            //SqlHelper.ExecuteNonQuery(dsn, CommandType.Text, "update cv_resource_requests_hours set parent = " + intNew.ToString() + " where parent = " + intID.ToString());
            //// Pending Execution
            //SqlHelper.ExecuteNonQuery(dsn, CommandType.Text, "update cv_ondemand_tasks_pending set resourceid = " + intNew.ToString() + " where resourceid = " + intID.ToString());
            //// Change Controls
            //SqlHelper.ExecuteNonQuery(dsn, CommandType.Text, "update cv_resource_request_change_controls set parent = " + intNew.ToString() + " where parent = " + intID.ToString());
            //// Milestones
            //SqlHelper.ExecuteNonQuery(dsn, CommandType.Text, "update cv_resource_request_milestones set parent = " + intNew.ToString() + " where parent = " + intID.ToString());
            //// Status (and TPM)
            //SqlHelper.ExecuteNonQuery(dsn, CommandType.Text, "update cv_resource_request_update set parent = " + intNew.ToString() + " where parent = " + intID.ToString());
            //SqlHelper.ExecuteNonQuery(dsn, CommandType.Text, "update cv_resource_request_update_tpm set parent = " + intNew.ToString() + " where parent = " + intID.ToString());
            //// PRC
            //SqlHelper.ExecuteNonQuery(dsn, CommandType.Text, "update cv_resource_request_prc set parent = " + intNew.ToString() + " where parent = " + intID.ToString());
            //// Details
            try
            {
                SqlHelper.ExecuteNonQuery(dsn, CommandType.Text, "update cv_resource_requests_details set resourceid = " + intNew.ToString() + " where requestid = " + oResourceRequest.Get(intID, "requestid") + " AND itemid = " + oResourceRequest.Get(intID, "itemid") + " AND number = " + oResourceRequest.Get(intID, "number"));
                intCount++;
            }
            catch { }
        }
        Response.Write("Done = " + intCount.ToString() + "<br/>");
    }
    private void btnLoad2_Click(Object Sender, EventArgs e)
    {
        DataSet ds = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "select * from cv_storage_3rd");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            int intID = Int32.Parse(dr["id"].ToString());
            string strClass = dr["class"].ToString();
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
            }
            SqlHelper.ExecuteNonQuery(dsn, CommandType.Text, "UPDATE cv_storage_3rd SET class = '" + strNew + "' WHERE class = '" + strClass + "' AND id = " + intID.ToString());
            intCount++;
        }
        Response.Write("Done = " + intCount.ToString() + "<br/>");
    }
    private void btnLoad3_Click(Object Sender, EventArgs e)
    {
        Asset oAsset = new Asset(0, dsnAsset);
        DataSet ds = SqlHelper.ExecuteDataset(dsnAsset, CommandType.Text, "select * from cva_enclosures");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            int intAsset = Int32.Parse(dr["assetid"].ToString());
            string strVC = dr["oa_ip"].ToString();
            oAsset.AddEnclosureVC(intAsset, strVC, 1, 1);
            intCount++;
        }
        Response.Write("Done = " + intCount.ToString() + "<br/>");
    }
    private void btnLoad4_Click(Object Sender, EventArgs e)
    {
        ResourceRequest oResourceRequest = new ResourceRequest(0, dsn);
        ServiceRequests oServiceRequest = new ServiceRequests(0, dsn);
        DataSet ds = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "select * from cv_resource_requests_workflow where name is null or name = ''");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            int intParent = Int32.Parse(dr["parent"].ToString());
            string strName = oResourceRequest.Get(intParent, "name");
            if (strName == "") 
            {
                string strRequest = oResourceRequest.Get(intParent, "requestid");
                if (strRequest != "")
                {
                    int intRequest = Int32.Parse(strRequest);
                    strName = oServiceRequest.Get(intRequest, "name");
                }
            }
            if (strName != "")
            {
                oResourceRequest.UpdateWorkflowName(Int32.Parse(dr["id"].ToString()), strName, 0);
                intCount++;
            }
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
                <td colspan="2" class="header">Code Push Updates</td>
            </tr>
            <tr>
                <td colspan="2">&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td><asp:Button ID="btnLoad1" runat="server" CssClass="default" Width="150" Text="Change Workflow Data" OnClick="btnLoad1_Click" /></td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td><asp:Button ID="btnLoad2" runat="server" CssClass="default" Width="150" Text="Fix 3rd Storage" OnClick="btnLoad2_Click" /></td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td><asp:Button ID="btnLoad3" runat="server" CssClass="default" Width="150" Text="Swap Virtual Connects" OnClick="btnLoad3_Click" /></td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td><asp:Button ID="btnLoad4" runat="server" CssClass="default" Width="150" Text="Update RR Names" OnClick="btnLoad4_Click" /></td>
            </tr>
        </table>
</form>
</body>
</html>