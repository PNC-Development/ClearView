<%@ Page Language="C#" Debug="false" %>
<%@ Import Namespace="System.Data.OleDb" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System" %>
<script runat="server">
    private int intEnvironment = 999;
    private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
    private Variables oVariable;
    private Functions oFunction;

    protected void Page_Load(object sender, EventArgs e)
    {
        oVariable = new Variables(intEnvironment);
        oFunction = new Functions(0, dsn, intEnvironment);
    }
    protected void btnGo_Click(object sender, EventArgs e)
    {
        string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\\temp\\workbook.xls;Extended Properties=Excel 8.0;";

        Services oServices = new Services(0, dsn);

        OleDbDataAdapter myCommand1 = new OleDbDataAdapter("SELECT * FROM [ALL$]", strConn);
        DataSet ds1 = new DataSet();
        myCommand1.Fill(ds1, "ExcelInfo");
        
        foreach (DataRow dr in ds1.Tables[0].Rows)
        {
            if (dr[0].ToString().Trim() == "")
                break;
            else
            {
                string folder = dr[0].ToString().Trim();
                if (folder.ToUpper().Trim() == "AUTOMATED")
                {
                    //txtResults.Text += DateTime.Now.ToString();
                }
                else
                {
                    string name = dr[4].ToString().Trim();
                    string actual = dr[5].ToString().Trim();
                    if (actual.ToUpper().Contains("ACTUAL") && actual.Contains(":"))
                        actual = actual.Substring(actual.IndexOf(":") + 1).Trim();
                    string desc = dr[6].ToString().Trim();
                    
                    SqlParameter[] arParams = new SqlParameter[2];
                    if (String.IsNullOrEmpty(actual) == false)
                        name = actual;
			        arParams[0] = new SqlParameter("@name", name);
			        arParams[1] = new SqlParameter("@desc", desc);
                    DataSet ds = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT * FROM cv_services WHERE name = @name", arParams);
                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        // try plural
                        arParams[0] = new SqlParameter("@name", name + "s");
                        ds = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT * FROM cv_services WHERE name = @name", arParams);
                        if (ds.Tables[0].Rows.Count == 0)
                        {
                            // try removing dashes
                            while (name.Contains("-"))
                                name = name.Replace("-", "");
                            arParams[0] = new SqlParameter("@name", name);
                            ds = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT * FROM cv_services WHERE name = @name", arParams);
                            if (ds.Tables[0].Rows.Count == 0)
                                txtResults.Text += "NOT FOUND! (name)";
                        }
                    }
                    
                    if (ds.Tables[0].Rows.Count > 1)
                    {
                        ds = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT * FROM cv_services WHERE name = @name AND CAST(description AS VARCHAR(MAX)) = @desc", arParams);
                        if (ds.Tables[0].Rows.Count == 0)
                            txtResults.Text += "NOT FOUND! (desc)";
                        else if (ds.Tables[0].Rows.Count > 1)
                            txtResults.Text += "TOO MANY RESULTS (" + ds.Tables[0].Rows.Count.ToString() + ")";
                    }

                    if (ds.Tables[0].Rows.Count == 1)
                    {
                        int ServiceID = Int32.Parse(ds.Tables[0].Rows[0]["serviceid"].ToString());
                        DataSet dsRR = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT * FROM cv_resource_requests WHERE serviceid = " + ServiceID + " AND deleted = 0 ORDER BY created DESC");
                        if (dsRR.Tables[0].Rows.Count > 0)
                        {
                            txtResults.Text += dsRR.Tables[0].Rows[0]["created"].ToString();
                            //txtResults.Text += dsRR.Tables[0].Rows.Count.ToString();
                            //break;
                        }
                        else
                        {
                            txtResults.Text += "Never";
                            //txtResults.Text += "0";
                        }
                    }
                }
                txtResults.Text += System.Environment.NewLine;
            }
        }
    }
 </script>
<script type="text/javascript">
</script>
<html>
<head>
<title>IMPORT FUNCTIONS</title>
<link rel="stylesheet" type="text/css" href="/css/default.css" />
</head>
<body leftmargin="0" topmargin="0">
<form id="Form1" runat="server">
    <p><asp:Button ID="btnGo" runat="server" Text="Go" OnClick="btnGo_Click" /></p>
    <p><asp:TextBox ID="txtResults" runat="server" TextMode="MultiLine" Rows="30" Width="800" /></p>
</form>
</body>
</html>