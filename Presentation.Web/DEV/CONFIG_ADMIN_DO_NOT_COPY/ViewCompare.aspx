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
    private void btnLoad1_Click(Object Sender, EventArgs e)
    {
        string strResponse = "";
        string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=F:\\Excel.xls;Extended Properties=Excel 8.0;";
        OleDbDataAdapter oCorp = new OleDbDataAdapter("SELECT * FROM [Corp$]", strConn);
        DataSet dsCorp = new DataSet();
        oCorp.Fill(dsCorp, "ExcelInfo");
        OleDbDataAdapter oBranch = new OleDbDataAdapter("SELECT * FROM [Branch$]", strConn);
        DataSet dsBranch = new DataSet();
        oBranch.Fill(dsBranch, "ExcelInfo");
        DataSet ds = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT [Device Name] AS name, [Commission Date], Model, ServerType, Year, Month, CommissionMonth, Environment, OperatingSystem, [Current Class] AS cclass, [Intended Class] AS iclass, NccBuildDate, [Retire Date], FullName, [Stock Location], [Amp Rating], Assignment, State, Address1, City, Workgroup, nccSerialNo AS serial, AssetTag, NccRoom, NccRack, [Mounting Pos Start], mPrice, Value, [HR Site Code], CASE WHEN vw_AssetCenter_ServerAsset_Details.ServerType = 'Server' THEN 'Distributed' ELSE vw_AssetCenter_ServerAsset_Details.ServerType END AS 'Platform', CASE WHEN [HR Site Code] = '/OH005/' THEN 'Cleveland Ops' WHEN [HR Site Code] = '/OH937/' THEN 'Cincinnati' WHEN [HR Site Code] = '/OH198/' THEN 'Highland Hills' WHEN [HR Site Code] = '/OH388/' THEN 'Miamisburg' WHEN [HR Site Code] IS NULL OR [HR Site Code] LIKE '/%/' THEN 'Other' ELSE [HR Site Code] END AS Location FROM vw_AssetCenter_ServerAsset_Details WHERE (Model NOT LIKE '%virtual%') AND ([Commission Date] IS NOT NULL)");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            string strName = dr["name"].ToString().Trim().ToUpper();
            string strSerial = dr["serial"].ToString().Trim().ToUpper();
            string strModel = dr["model"].ToString().Trim().ToUpper();
            string strCurrent = dr["cclass"].ToString().Trim().ToUpper();
            string strIntended = dr["iclass"].ToString().Trim().ToUpper();
            bool boolFound = false;
            foreach (DataRow drCorp in dsCorp.Tables[0].Rows)
            {
                if (drCorp[1].ToString().Trim().ToUpper() == strSerial)
                {
                    boolFound = true;
                    break;
                }
            }
            if (boolFound == false)
            {
                foreach (DataRow drBranch in dsBranch.Tables[0].Rows)
                {
                    if (drBranch[1].ToString().Trim().ToUpper() == strSerial)
                    {
                        boolFound = true;
                        break;
                    }
                }
                if (boolFound == false)
                {
                    strResponse += "<tr>";
                    strResponse += "<td>" + strSerial + "</td>";
                    strResponse += "<td>" + strName + "</td>";
                    strResponse += "<td>" + strModel + "</td>";
                    strResponse += "<td>" + strCurrent + "</td>";
                    strResponse += "<td>" + strIntended + "</td>";
                    strResponse += "</tr>";
                    intCount++;
                }
                else if (chkOnly.Checked == false)
                {
                    strResponse += "<tr>";
                    strResponse += "<td>" + strSerial + "</td>";
                    strResponse += "<td colspan=\"4\">FOUND in Branch!!</td>";
                    strResponse += "</tr>";
                }
            }
            else if (chkOnly.Checked == false)
            {
                strResponse += "<tr>";
                strResponse += "<td>" + strSerial + "</td>";
                strResponse += "<td colspan=\"4\">FOUND in Corp!!</td>";
                strResponse += "</tr>";
            }
        }
        Variables oVariable = new Variables(intEnvironment);
        strResponse += "<tr><td colspan=\"5\"><b>Total:</b> " + intCount.ToString() + "</td></tr>";
        if (strResponse != "")
            strResponse = "<table border=\"0\" style=\"" + oVariable.DefaultFontStyle() + "\"><tr><td><b>Serial Number</b></td><td><b>Device Name</b></td><td><b>Model</b></td><td><b>Current Class</b></td><td><b>Intended Class</b></td></tr>" + strResponse + "</table>";
        Response.Write(strResponse);
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
                <td colspan="2"><asp:CheckBox ID="chkOnly" runat="server" CssClass="default" Text="Show Only those that do not exist in excel document" /></td>
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