<%@ Page Language="C#" Debug="false" %>
<%@ Import Namespace="System.Data.OleDb" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System" %>
<script runat="server">
    private int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
    private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
    private int intCount = 0;
    private void Page_Load()
    {
        intCount = 0;
    }
    private void btnImport_Click(Object Sender, EventArgs e)
    {
        ServiceDetails oServiceDetail = new ServiceDetails(0, dsn);
        string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=F:\\Services.xls;Extended Properties=Excel 8.0;";
        OleDbDataAdapter myCommand = new OleDbDataAdapter("SELECT * FROM [Services$]", strConn);
        DataSet ds = new DataSet();
        myCommand.Fill(ds, "ExcelInfo");
        int intOldDetail = 0;
        DataSet dsDetails = oServiceDetail.Gets(0, 0, 1);
        int intDetailOrder = dsDetails.Tables[0].Rows.Count + 1;
        int intOrder = 0;
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            if (dr[0].ToString().Trim() == "")
                break;
            int intApplication = GetApplication(dr[0].ToString().Trim());
            int intItem = GetItem(dr[1].ToString().Trim(), intApplication);
            int intParent = 0;
            string strParent1 = dr[2].ToString().Trim();
            if (strParent1 != "")
                intParent = GetService(strParent1, "", intParent, intItem, 0);
            string strParent2 = dr[3].ToString().Trim();
            if (strParent2 != "")
                intParent = GetService(strParent2, "", intParent, intItem, 0);
            string strParent3 = dr[4].ToString().Trim();
            if (strParent3 != "")
                intParent = GetService(strParent3, "", intParent, intItem, 0);
            string strService = dr[5].ToString().Trim();
            int intService = GetService(strService, dr[6].ToString().Trim(), intParent, intItem, (dr[12].ToString().Trim().ToUpper() == "YES" ? 1 : 0));
            string strParentDetail = dr[7].ToString().Trim();
            if (strParentDetail == "")
                strParentDetail = strService;
            int intDetailParent = GetDetailParent(strParentDetail, intDetailOrder, intService);
            if (intOldDetail != intDetailParent)
            {
                intDetailOrder++;
                intOrder = 0;
                intOldDetail = intDetailParent;
            }
            intOrder++;
            string strDetail = dr[8].ToString().Trim();
            if (strDetail == "")
                strDetail = strService;
            int intDetail = GetDetail(strDetail, intDetailParent, intOrder, intService, double.Parse(dr[9].ToString().Trim()), double.Parse(dr[10].ToString().Trim()), (dr[11].ToString().Trim().ToUpper() == "YES" ? 1 : 0));
            intCount++;
        }
        Response.Write("Finished - " + intCount.ToString() + " Rows Affected");
    }
    private int GetApplication(string _name)
    {
        DataSet ds = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT * FROM cv_applications WHERE name = '" + _name + "' AND enabled = 1 AND deleted = 0");
        if (ds.Tables[0].Rows.Count == 0)
        {
            Applications oTemp = new Applications(0, dsn);
            string strURL = _name;
            while (strURL.IndexOf(" ") > -1)
                strURL = strURL.Replace(" ", "");
            oTemp.Add(_name, strURL, _name, _name, "", "", 0, 0, 0, 0, 0, 0, 0, 0, "", 0, 0, 0, 0, 0, 1, 2, 0, 0, 0, 1);
            return GetApplication(_name);
        }
        else
            return Int32.Parse(ds.Tables[0].Rows[0]["applicationid"].ToString());
    }
    private int GetItem(string _name, int _applicationid)
    {
        DataSet ds = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT * FROM cv_request_items WHERE name = '" + _name + "' AND enabled = 1 AND deleted = 0");
        if (ds.Tables[0].Rows.Count == 0)
        {
            RequestItems oTemp = new RequestItems(0, dsn);
            oTemp.AddItem(_applicationid, _name, _name, "", 0, 0, 0, 0, 1);
            return GetItem(_name, _applicationid);
        }
        else
            return Int32.Parse(ds.Tables[0].Rows[0]["itemid"].ToString());
    }
    private int GetService(string _name, string _desc, int _parent, int _itemid, int _project)
    {
        DataSet ds = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT * FROM cv_services WHERE name = '" + _name + "' AND enabled = 1 AND deleted = 0");
        if (ds.Tables[0].Rows.Count == 0)
        {
            Services oTemp = new Services(0, dsn);
            oTemp.Add(_name, _desc, "", _itemid, 1, 0, _project, -1, 0.00, 0.00, 0, 0, 0, 0, "", "", "", "", 0, 0, 0, 0, 0, 0, 1, 0, 0, "", 0, 0, 0, 0, 0, 0, "", 0, 0, 1);
            return GetService(_name, _desc, _parent, _itemid, _project);
        }
        else
            return Int32.Parse(ds.Tables[0].Rows[0]["serviceid"].ToString());
    }
    private int GetDetailParent(string _name, int _display, int _serviceid)
    {
        DataSet ds = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT * FROM cv_services_detail WHERE name = '" + _name + "' AND serviceid = " + _serviceid.ToString() + " AND enabled = 1 AND deleted = 0");
        if (ds.Tables[0].Rows.Count == 0)
        {
            ServiceDetails oServiceDetail = new ServiceDetails(0, dsn);
            oServiceDetail.Add(_serviceid, _name, 0, 0.00, 0.00, 0, _display, 1);
            int intDetail = GetDetailParent(_name, _display, _serviceid);
            return intDetail;
        }
        else
            return Int32.Parse(ds.Tables[0].Rows[0]["detailid"].ToString());
    }
    private int GetDetail(string _name, int _parent, int _display, int _serviceid, double _hours, double _additional, int _checkbox)
    {
        int intDetail = 0;
        DataSet ds = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT * FROM cv_services_detail WHERE serviceid = " + _serviceid.ToString() + " AND name = '" + _name + "' AND parent = " + _parent.ToString() + " AND display = " + _display.ToString() + " AND enabled = 1 AND deleted = 0");
        if (ds.Tables[0].Rows.Count > 0)
            return Int32.Parse(ds.Tables[0].Rows[0]["detailid"].ToString());
        else
        {
            ServiceDetails oServiceDetail = new ServiceDetails(0, dsn);
            oServiceDetail.Add(_serviceid, _name, _parent, _hours, _additional, _checkbox, _display, 1);
            return GetDetail(_name, _parent, _display, _serviceid, _hours, _additional, _checkbox);
        }
    }
</script>
<script type="text/javascript">
</script>
<html>
<head>
<title>IMPORT</title>
<link rel="stylesheet" type="text/css" href="/css/default.css" />
</head>
<body leftmargin="0" topmargin="0">
<form id="Form1" runat="server">
        <table>
            <tr>
                <td colspan="2" class="header">Import Services</td>
            </tr>
            <tr>
                <td colspan="2">&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td><asp:Button ID="btnImport" runat="server" CssClass="default" Width="150" Text="Import Services" OnClick="btnImport_Click" /></td>
            </tr>
        </table>
</form>
</body>
</html>