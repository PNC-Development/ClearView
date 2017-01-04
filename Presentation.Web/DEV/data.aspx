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
    }
    public void btnDatabaseFieldData_Click(Object Sender, EventArgs e)
    {
        string strDSN = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings[ddlDatabaseFieldData.SelectedItem.Value]].ConnectionString;
        bool boolError = false;
        DataSet dsT = SqlHelper.ExecuteDataset(strDSN, CommandType.Text, "select name from sys.tables order by name");
        foreach (DataRow drT in dsT.Tables[0].Rows)
        {
            if (boolError)
                break;
            string strTable = drT["name"].ToString();
            DataSet dsS = SqlHelper.ExecuteDataset(strDSN, CommandType.Text, "select c.name, c.max_length AS length, c.collation_name AS collation_c, t.name AS type, t.collation_name AS collation_t from sys.columns c LEFT OUTER JOIN sys.types t ON t.system_type_id = c.system_type_id AND t.user_type_id <= 255 where c.object_id =object_id('" + strTable + "')");
            foreach (DataRow drS in dsS.Tables[0].Rows)
            {
                if (boolError)
                    break;
                string strField = drS["name"].ToString().ToUpper();
                string strSQL = txtDatabaseFieldData.Text;
                string strType = drS["type"].ToString().ToUpper();
                SqlParameter[] arParamsData = new SqlParameter[1];
                bool boolInt = false;
                int intTemp = 0;
                Int32.TryParse(strSQL, out intTemp);
                bool boolDatetime = false;
                DateTime datToday = DateTime.Today;
                try { datToday = DateTime.Parse(strSQL); }
                catch {datToday = DateTime.Today; }
                bool boolText = false;
                if (strType == "DATETIME" || strType == "VARCHAR" || strType == "CHAR" || strType == "NVARCHAR" || strType == "NCHAR" || strType == "TEXT")
                {
                    if (strSQL.Contains("'") == true)
                        arParamsData[0] = new SqlParameter("@value", strSQL.Replace("'", "''"));
                    else
                        arParamsData[0] = new SqlParameter("@value", strSQL);

                    if (strType == "DATETIME")
                    {
                        boolDatetime = true;
                        arParamsData[0].SqlDbType = SqlDbType.DateTime;
                    }
                    else if (strType == "VARCHAR")
                        arParamsData[0].SqlDbType = SqlDbType.VarChar;
                    else if (strType == "CHAR")
                        arParamsData[0].SqlDbType = SqlDbType.Char;
                    else if (strType == "NVARCHAR")
                        arParamsData[0].SqlDbType = SqlDbType.NVarChar;
                    else if (strType == "NCHAR")
                        arParamsData[0].SqlDbType = SqlDbType.NChar;
                    else if (strType == "TEXT")
                    {
                        boolText = true;
                        arParamsData[0].SqlDbType = SqlDbType.VarChar;
                    }
                }
                else
                {
                    boolInt = true;
                    arParamsData[0] = new SqlParameter("@value", strSQL);
                    if (strType == "BIGINT")
                        arParamsData[0].SqlDbType = SqlDbType.BigInt;
                    else
                        arParamsData[0].SqlDbType = SqlDbType.Int;
                }
                if ((intTemp != 0 || boolInt == false) && (datToday != DateTime.Today || boolDatetime == false))
                {
                    string strExecute = "SELECT * FROM " + strTable + " WHERE [" + strField + "] " + (boolInt == false && boolDatetime == false ? "LIKE '%' + " : " = ") + " @value" + (boolInt == false && boolDatetime == false ? " + '%'" : "");
                    try
                    {
                        //Response.Write(strExecute + ", value:" + strSQL + ", type:" + strType);
                        DataSet ds = SqlHelper.ExecuteDataset(strDSN, CommandType.Text, strExecute, arParamsData);
                        if (ds.Tables[0].Rows.Count > 0)
                            lblDatabaseFieldData.Text += strTable + "." + strField + " = " + ds.Tables[0].Rows.Count.ToString() + " records<br/>";
                    }
                    catch (Exception ex)
                    {
                        Response.Write(ex.Message + "<br/>");
                        Response.Write(strExecute + ", value:" + strSQL + ", dbtype:" + arParamsData[0].SqlDbType.ToString() + ", type:" + strType + "<br/>");
                        boolError = true;
                    }
                }
            }
        }
        Page.ClientScript.RegisterStartupScript(typeof(Page), "anchoring", "<script type=\"text/javascript\">location.href='#DatabaseFieldData'<" + "/" + "script>");
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
		            <td colspan="2"><b>Get Database Fields by Record Data</b></td>
		        </tr>
	            <tr>
	                <td>DSN:</td>
	                <td>
	                    <asp:DropDownList ID="ddlDatabaseFieldData" runat="server" CssClass="default">
	                        <asp:ListItem Value="DSN" />
	                        <asp:ListItem Value="AssetDSN" />
	                        <asp:ListItem Value="IpDSN" />
	                        <asp:ListItem Value="ServiceDSN" />
	                        <asp:ListItem Value="ServiceEditorDSN" />
	                        <asp:ListItem Value="ZeusDSN" />
	                        <asp:ListItem Value="ReportingDSN" />
	                        <asp:ListItem Value="ClearViewDWDSN" />
	                    </asp:DropDownList>
	                </td>
	            </tr>
		        <tr>
		            <td nowrap>Data:</td>
		            <td width="100%"><asp:TextBox ID="txtDatabaseFieldData" runat="server" CssClass="default" Width="200" MaxLength="100" /></td>
		        </tr>
		        <tr>
		            <td colspan="2"><asp:Button ID="btnDatabaseFieldData" runat="server" CssClass="default" Width="75" OnClick="btnDatabaseFieldData_Click" Text="Go" /></td>
		        </tr>
		        <tr>
		            <td colspan="2"><asp:Label ID="lblDatabaseFieldData" runat="server" CssClass="default" /></td>
		        </tr>
		    </table>
</form>
</body>
</html>