<%@ Page Language="C#" Debug="true" %>
<%@ Import Namespace="NCC.ClearView.Application.Core.Proteus" %>

<script runat="server">
    private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
    private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
    private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
    private string dsnServiceEditor = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ServiceEditorDSN"]].ConnectionString;
    private string dsnDW = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ClearViewDWDSN"]].ConnectionString;
    private int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);

    protected void Page_Load(object sender, EventArgs e)
    {
        string strDSN = dsnAsset;
        Page.ClientScript.RegisterStartupScript(typeof(Page), "dsn", "<script type=\"text/javascript\">alert('" + strDSN + "');<" + "/" + "script>");
        string strCollation = "SQL_Latin1_General_CP1_CI_AS";
        DataSet dsT = SqlHelper.ExecuteDataset(strDSN, CommandType.Text, "select name from sys.tables order by name");
        foreach (DataRow drT in dsT.Tables[0].Rows)
        {
            DataSet dsS = SqlHelper.ExecuteDataset(strDSN, CommandType.Text, "select c.name, c.max_length AS length, c.collation_name AS collation_c, t.name AS type, t.collation_name AS collation_t from sys.columns c LEFT OUTER JOIN sys.types t ON t.system_type_id = c.system_type_id AND t.user_type_id <= 255 where c.object_id =object_id('" + drT["name"].ToString() + "')");
            foreach (DataRow drS in dsS.Tables[0].Rows)
            {
                string strType = drS["type"].ToString().ToUpper();
                if (drS["collation_c"].ToString() != strCollation || drS["collation_t"].ToString() != strCollation)
                {
                    if (strType == "VARCHAR" || strType == "CHAR" || strType == "NVARCHAR" || strType == "NCHAR")
                        Response.Write("ALTER TABLE dbo." + drT["name"].ToString() + " ALTER COLUMN [" + drS["name"].ToString() + "] " + drS["type"].ToString() + "(" + (drS["length"].ToString() == "-1" ? "MAX" : drS["length"].ToString()) + ") COLLATE SQL_Latin1_General_CP1_CI_AS NULL  -- " + drS["collation_c"].ToString() + ", " + drS["collation_t"].ToString() + "<br/>");
                    else if (strType == "TEXT")
                        Response.Write("ALTER TABLE dbo." + drT["name"].ToString() + " ALTER COLUMN [" + drS["name"].ToString() + "] " + drS["type"].ToString() + " COLLATE SQL_Latin1_General_CP1_CI_AS NULL  -- " + drS["collation_c"].ToString() + ", " + drS["collation_t"].ToString() + "<br/>");
                }
            }
        }
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
<asp:Label ID="lblResult" runat="server" />
</form>
</body>
</html>