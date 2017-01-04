<%@ Page Language="C#" Debug="false" %>
<%@ Import Namespace="System.Data.OleDb" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System" %>
<script runat="server">
    private int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
    private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
    private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
    private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
    private int intCount = 0;
    private void Page_Load()
    {
        intCount = 0;
    }
    private void btnLoad_Click(Object Sender, EventArgs e)
    {
        DataSet dsProject = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT * FROM cv_projects WHERE deleted = 0 AND status = 2");
        foreach (DataRow drProject in dsProject.Tables[0].Rows)
        {
            bool boolClose = true;
            DataSet ds = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT cv_users.fname + ' ' + cv_users.lname AS username, cv_resource_requests.*, cv_service_requests.id AS SRID, cv_project_requests.id AS PRID FROM cv_resource_requests INNER JOIN cv_requests ON cv_resource_requests.requestid = cv_requests.requestid AND cv_requests.deleted = 0 AND cv_requests.projectid = " + drProject["projectid"].ToString() + " INNER JOIN cv_users ON cv_resource_requests.userid = cv_users.userid AND cv_users.deleted = 0 LEFT OUTER JOIN cv_service_requests ON cv_resource_requests.requestid = cv_service_requests.requestid AND cv_service_requests.deleted = 0 LEFT OUTER JOIN cv_project_requests ON cv_resource_requests.requestid = cv_project_requests.requestid AND cv_project_requests.deleted = 0");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (dr["status"].ToString() != "3")
                    boolClose = false;
            }
            if (boolClose == true)
            {
                SqlHelper.ExecuteNonQuery(dsn, CommandType.Text, "UPDATE cv_projects SET status = 3, completed = getdate() WHERE projectid = " + drProject["projectid"].ToString());
                intCount++;
            }
        }
        Response.Write("Projects Closed = " + intCount.ToString() + "<br>");
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
                <td colspan="2">&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td><asp:Button ID="btnLoad" runat="server" CssClass="default" Width="150" Text="Go" OnClick="btnLoad_Click" /></td>
            </tr>
        </table>
</form>
</body>
</html>