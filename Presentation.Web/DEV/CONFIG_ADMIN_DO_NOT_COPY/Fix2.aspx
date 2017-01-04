<%@ Page Language="C#" Debug="false" %>
<%@ Import Namespace="System.Data.OleDb" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System" %>
<script runat="server">
    private int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
    private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
    private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
    private int intBlade = 0;
    private int intBladeSkip = 0;
    private int intBladeDup = 0;
    private int intPhysical = 0;
    private int intPhysicalSkip = 0;
    private int intPhysicalDup = 0;
    private int intEnclosure = 0;
    private int intEnclosureSkip = 0;
    private int intEnclosureDup = 0;
    private void Page_Load()
    {
        intBlade = 0;
        intBladeSkip = 0;
        intBladeDup = 0;
        intEnclosure = 0;
        intEnclosureSkip = 0;
        intEnclosureDup = 0;
        intPhysical = 0;
        intPhysicalSkip = 0;
        intPhysicalDup = 0;
    }
    private void btnLoad1_Click(Object Sender, EventArgs e)
    {
        ModelsProperties oModelsProperties = new ModelsProperties(0, dsn);
        Asset oAsset = new Asset(0, dsnAsset);
        string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=F:\\fix2.xls;Extended Properties=Excel 8.0;";
        OleDbDataAdapter myCommand = new OleDbDataAdapter("SELECT * FROM [380$]", strConn);
        DataSet ds = new DataSet();
        myCommand.Fill(ds, "ExcelInfo");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            if (dr[0].ToString().Trim() == "")
                break;
            string strSerial = dr[0].ToString().Trim();
            string strAsset = dr[1].ToString().Trim();
            int intAddress = Int32.Parse(dr[2].ToString().Trim());
            string strRoom = dr[3].ToString().Trim();
            int intAsset = 0;
            DataSet dsSerial = oAsset.Get(dr[0].ToString().Trim());
            if (dsSerial.Tables[0].Rows.Count > 0)
                intAsset = Int32.Parse(dsSerial.Tables[0].Rows[0]["id"].ToString());
            int intRoom = 0;
            DataSet dsRoom = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT * FROM cv_rooms WHERE name = '" + strRoom + "'");
            if (dsRoom.Tables[0].Rows.Count > 0)
                intRoom = Int32.Parse(dsRoom.Tables[0].Rows[0]["id"].ToString());
            DataSet dsDuplicate = SqlHelper.ExecuteDataset(dsnAsset, CommandType.Text, "SELECT * FROM cva_server WHERE assetid = " + intAsset.ToString());
            if (dsDuplicate.Tables[0].Rows.Count > 0)
                intPhysicalDup++;
            else
            {
                if (intAsset > 0)
                {
                    oAsset.AddServer(intAsset, (int)AssetStatus.Arrived, -999, DateTime.Now, 1, 1, intAddress, intRoom, 0, "N/A", "", "", "", 0);
                    intPhysical++;
                }
                else
                    intPhysicalSkip++;
            }
        }
        Response.Write("Physical (Done) = " + intPhysical.ToString() + "<br/>");
        Response.Write("Physical (Skip) = " + intPhysicalSkip.ToString() + "<br/>");
        Response.Write("Physical (Dup) = " + intPhysicalDup.ToString() + "<br/>");
    }
    private void btnLoad2_Click(Object Sender, EventArgs e)
    {
        ModelsProperties oModelsProperties = new ModelsProperties(0, dsn);
        Asset oAsset = new Asset(0, dsnAsset);
        string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=F:\\fix2.xls;Extended Properties=Excel 8.0;";
        OleDbDataAdapter myCommand = new OleDbDataAdapter("SELECT * FROM [Enclosure$]", strConn);
        DataSet ds = new DataSet();
        myCommand.Fill(ds, "ExcelInfo");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            if (dr[0].ToString().Trim() == "")
                break;
            string strSerial = dr[0].ToString().Trim();
            string strAsset = dr[1].ToString().Trim();
            int intAddress = Int32.Parse(dr[2].ToString().Trim());
            string strRoom = dr[3].ToString().Trim();
            int intAsset = 0;
            DataSet dsSerial = oAsset.Get(dr[0].ToString().Trim());
            if (dsSerial.Tables[0].Rows.Count > 0)
                intAsset = Int32.Parse(dsSerial.Tables[0].Rows[0]["id"].ToString());
            int intRoom = 0;
            DataSet dsRoom = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT * FROM cv_rooms WHERE name = '" + strRoom + "'");
            if (dsRoom.Tables[0].Rows.Count > 0)
                intRoom = Int32.Parse(dsRoom.Tables[0].Rows[0]["id"].ToString());
            DataSet dsDuplicate = SqlHelper.ExecuteDataset(dsnAsset, CommandType.Text, "SELECT * FROM cva_enclosures WHERE assetid = " + intAsset.ToString());
            if (dsDuplicate.Tables[0].Rows.Count > 0)
                intEnclosureDup++;
            else
            {
                if (intAsset > 0)
                {
                    oAsset.AddEnclosure(intAsset, strSerial, (int)AssetStatus.Reserved, -999, DateTime.Now, 1, 1, intAddress, intRoom, 0, "", 0, "");
                    intEnclosure++;
                }
                else
                    intEnclosureSkip++;
            }
        }
        Response.Write("Enclosure (Done) = " + intEnclosure.ToString() + "<br/>");
        Response.Write("Enclosure (Skip) = " + intEnclosureSkip.ToString() + "<br/>");
        Response.Write("Enclosure (Dup) = " + intEnclosureDup.ToString() + "<br/>");
    }
    private void btnLoad3_Click(Object Sender, EventArgs e)
    {
        ModelsProperties oModelsProperties = new ModelsProperties(0, dsn);
        Asset oAsset = new Asset(0, dsnAsset);
        string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=F:\\fix2.xls;Extended Properties=Excel 8.0;";
        OleDbDataAdapter myCommand = new OleDbDataAdapter("SELECT * FROM [460$]", strConn);
        DataSet ds = new DataSet();
        myCommand.Fill(ds, "ExcelInfo");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            if (dr[0].ToString().Trim() == "")
                break;
            string strSerial = dr[0].ToString().Trim();
            string strAsset = dr[1].ToString().Trim();
            int intAddress = Int32.Parse(dr[2].ToString().Trim());
            string strRoom = dr[3].ToString().Trim();
            string strEnclosure = dr[4].ToString().Trim();
            int intAsset = 0;
            DataSet dsSerial = oAsset.Get(dr[0].ToString().Trim());
            if (dsSerial.Tables[0].Rows.Count > 0)
                intAsset = Int32.Parse(dsSerial.Tables[0].Rows[0]["id"].ToString());
            int intEnclosure = 0;
            DataSet dsEnclosure = oAsset.Get(strEnclosure);
            if (dsEnclosure.Tables[0].Rows.Count > 0)
                intEnclosure = Int32.Parse(dsEnclosure.Tables[0].Rows[0]["id"].ToString());
            int intRoom = 0;
            DataSet dsRoom = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT * FROM cv_rooms WHERE name = '" + strRoom + "'");
            if (dsRoom.Tables[0].Rows.Count > 0)
                intRoom = Int32.Parse(dsRoom.Tables[0].Rows[0]["id"].ToString());
            DataSet dsDuplicate = SqlHelper.ExecuteDataset(dsnAsset, CommandType.Text, "SELECT * FROM cva_blades WHERE assetid = " + intAsset.ToString());
            if (dsDuplicate.Tables[0].Rows.Count > 0)
                intBladeDup++;
            else
            {
                if (intAsset > 0)
                {
                    oAsset.AddBlade(intAsset, (int)AssetStatus.Arrived, -999, DateTime.Now, intEnclosure, "", "", "", 0, 0, 0);
                    intBlade++;
                }
                else
                    intBladeSkip++;
            }
        }
        Response.Write("Blade (Done) = " + intBlade.ToString() + "<br/>");
        Response.Write("Blade (Skip) = " + intBladeSkip.ToString() + "<br/>");
        Response.Write("Blade (Dup) = " + intBladeDup.ToString() + "<br/>");
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
                <td colspan="2" class="header">Fix</td>
            </tr>
            <tr>
                <td colspan="2">&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td><asp:Button ID="btnLoad1" runat="server" CssClass="default" Width="150" Text="380" OnClick="btnLoad1_Click" /></td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td><asp:Button ID="btnLoad2" runat="server" CssClass="default" Width="150" Text="Enclosure" OnClick="btnLoad2_Click" /></td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td><asp:Button ID="btnLoad3" runat="server" CssClass="default" Width="150" Text="Blade" OnClick="btnLoad3_Click" /></td>
            </tr>
        </table>
</form>
</body>
</html>