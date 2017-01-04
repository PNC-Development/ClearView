<%@ Page Language="C#" Debug="false" %>
<%@ Import Namespace="System.Data.OleDb" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System" %>
<script runat="server">
    private int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
    private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
    private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
    private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
    private int intCount = 0;
    private string strError = "";
    private void Page_Load()
    {
        intCount = 0;
    }
    private void btnLoad1_Click(Object Sender, EventArgs e)
    {
        Response.Write("<p>SPREADSHEET != DATABASE</p>");
        Asset oAsset = new Asset(0, dsnAsset);
        Rooms oRoom = new Rooms(0, dsn);
        Racks oRack = new Racks(0, dsn);
        string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=F:\\Greg.xls;Extended Properties=Excel 8.0;";
        DataSet ds = new DataSet();
        OleDbDataAdapter myCommand = new OleDbDataAdapter("SELECT * FROM [" + radType.SelectedItem.Value + "$]", strConn);
        myCommand.Fill(ds, "ExcelInfo");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            string strModel = "";
            string strSerial = "";
            string strAsset = "";
            string strStatus = "";
            string strRoom = "";
            string strRack = "";
            string strRackPos = "";
            string strEnclosure = "";
            string strBay = "";
            string strDummy = "";
            string strILO = "";
            string strClass = "";
            string strEnvironment = "";
            bool boolBlade = false;
            switch (radType.SelectedItem.Value)
            {
                case "380G5 Test":
                    strModel = dr[0].ToString().Trim();
                    strSerial = dr[1].ToString().Trim();
                    strAsset = dr[2].ToString().Trim();
                    strStatus = dr[3].ToString().Trim();
                    strRoom = dr[8].ToString().Trim();
                    strRack = dr[9].ToString().Trim();
                    strRackPos = dr[10].ToString().Trim();
                    strDummy = dr[1].ToString().Trim();
                    strILO = dr[14].ToString().Trim();
                    strClass = "Test";
                    strEnvironment = "Core";
                    boolBlade = false;
                    break;
                case "380G5 Prod":
                    strModel = dr[0].ToString().Trim();
                    strSerial = dr[1].ToString().Trim();
                    strAsset = dr[2].ToString().Trim();
                    strStatus = dr[3].ToString().Trim();
                    strRoom = dr[8].ToString().Trim();
                    strRack = dr[9].ToString().Trim();
                    strRackPos = dr[10].ToString().Trim();
                    strDummy = dr[1].ToString().Trim();
                    strILO = dr[14].ToString().Trim();
                    strClass = "Production";
                    strEnvironment = "Core";
                    boolBlade = false;
                    break;
                case "460C Test":
                    strModel = dr[0].ToString().Trim();
                    strSerial = dr[1].ToString().Trim();
                    strAsset = dr[2].ToString().Trim();
                    strStatus = dr[3].ToString().Trim();
                    strRoom = dr[7].ToString().Trim();
                    strRack = dr[8].ToString().Trim();
                    strEnclosure = dr[9].ToString().Trim();
                    strBay = dr[10].ToString().Trim();
                    strDummy = dr[11].ToString().Trim();
                    strILO = dr[14].ToString().Trim();
                    strClass = "Test";
                    strEnvironment = "Core";
                    boolBlade = true;
                    break;
                case "460C Prod E":
                    strModel = dr[0].ToString().Trim();
                    strSerial = dr[1].ToString().Trim();
                    strAsset = dr[2].ToString().Trim();
                    strStatus = dr[3].ToString().Trim();
                    strRoom = dr[7].ToString().Trim();
                    strRack = dr[8].ToString().Trim();
                    strEnclosure = dr[9].ToString().Trim();
                    strBay = dr[10].ToString().Trim();
                    strDummy = dr[11].ToString().Trim();
                    strILO = dr[14].ToString().Trim();
                    strClass = "Production";
                    strEnvironment = "Core";
                    boolBlade = true;
                    break;
                case "460C Prod H":
                    strModel = dr[0].ToString().Trim();
                    strSerial = dr[1].ToString().Trim();
                    strAsset = dr[2].ToString().Trim();
                    strStatus = dr[3].ToString().Trim();
                    strRoom = dr[7].ToString().Trim();
                    strRack = dr[8].ToString().Trim();
                    strEnclosure = dr[9].ToString().Trim();
                    strBay = dr[10].ToString().Trim();
                    strDummy = dr[11].ToString().Trim();
                    strILO = dr[14].ToString().Trim();
                    strClass = "Production";
                    strEnvironment = "Core";
                    boolBlade = true;
                    break;
            }

            if (strSerial != "")
            {
                int intModel = GetModel(strModel);
                DataSet dsAsset = oAsset.Get(strSerial);
                if (dsAsset.Tables[0].Rows.Count == 0)
                    strError += " ERROR: No Asset Found<br/>";
                else if (dsAsset.Tables[0].Rows.Count > 1)
                    strError += " ERROR: Duplicate Asset Found<br/>";
                else
                {
                    int intAsset = Int32.Parse(dsAsset.Tables[0].Rows[0]["id"].ToString());
                    if (intModel != Int32.Parse(dsAsset.Tables[0].Rows[0]["modelid"].ToString()))
                        strError += " ERROR: Models do not match (" + intModel.ToString() + " != " + Int32.Parse(dsAsset.Tables[0].Rows[0]["modelid"].ToString()) + ")<br/>";
                    if (strAsset != dsAsset.Tables[0].Rows[0]["asset"].ToString())
                    {
                        strError += " ERROR: Asset Tags do not match (" + strAsset + " != " + dsAsset.Tables[0].Rows[0]["asset"].ToString() + ")<br/>";
                        if (chkAsset.Checked == true)
                            oAsset.Update(intAsset, strAsset);
                    }
                    if (oAsset.GetServerOrBlade(intAsset).Tables[0].Rows.Count == 0)
                    {
                        strError += " ERROR: Asset has not been deployed<br/>";
                        if (chkDeploy.Checked == true)
                        {
                            if (boolBlade == true)
                                oAsset.AddBlade(intAsset, GetStatus(strStatus), -100, DateTime.Now, GetEnclosure(strEnclosure), strILO, strDummy, "", 0, Int32.Parse(strBay), 0);
                            else
                                oAsset.AddServer(intAsset, GetStatus(strStatus), -100, DateTime.Now, GetClass(strClass), GetEnvironment(strEnvironment), 715, GetRoom(strRoom), GetRack(strRack), strRackPos, strILO, strDummy, "", 0);
                        }
                    }
                    else
                    {
                        if (strRoom != oAsset.GetServerOrBlade(intAsset, "room"))
                        {
                            strError += " ERROR: Rooms do not match (" + strRoom + " != " + oAsset.GetServerOrBlade(intAsset, "room") + ")<br/>";
                            if (chkRoom.Checked == true)
                                UpdateAsset(intAsset, "roomid", GetRoom(strRoom).ToString(), (boolBlade ? strEnclosure : ""));
                        }
                        if (strRack != oAsset.GetServerOrBlade(intAsset, "rack"))
                        {
                            strError += " ERROR: Racks do not match (" + strRack + " != " + oAsset.GetServerOrBlade(intAsset, "rack") + ")<br/>";
                            if (chkRack.Checked == true)
                                UpdateAsset(intAsset, "rackid", GetRack(strRack).ToString(), (boolBlade ? strEnclosure : ""));
                        }
                        if (boolBlade == false)
                        {
                            if (strRackPos != oAsset.GetServerOrBlade(intAsset, "rackposition"))
                            {
                                strError += " ERROR: Rack Positions do not match (" + strRackPos + " != " + oAsset.GetServerOrBlade(intAsset, "rackposition") + ")<br/>";
                                if (chkRackPos.Checked == true)
                                    UpdateAsset(intAsset, "rackposition", strRackPos, (boolBlade ? strEnclosure : ""));
                            }
                        }
                        else
                        {
                            if (GetEnclosure(strEnclosure).ToString() != oAsset.GetServerOrBlade(intAsset, "enclosureid"))
                            {
                                strError += " ERROR: Enclosures do not match (" + GetEnclosure(strEnclosure).ToString() + " != " + oAsset.GetServerOrBlade(intAsset, "enclosureid") + ")<br/>";
                                if (chkEnclosure.Checked == true)
                                    UpdateAsset(intAsset, "enclosureid", GetEnclosure(strEnclosure).ToString(), (boolBlade ? strEnclosure : ""));
                            }
                            if (strBay != oAsset.GetServerOrBlade(intAsset, "slot"))
                            {
                                strError += " ERROR: Enclosure Bays do not match (" + strBay + " != " + oAsset.GetServerOrBlade(intAsset, "slot") + ")<br/>";
                                if (chkEnclosureBay.Checked == true)
                                    UpdateAsset(intAsset, "slot", strBay, (boolBlade ? strEnclosure : ""));
                            }
                        }
                        if (strILO != oAsset.GetServerOrBlade(intAsset, "ilo"))
                        {
                            strError += " ERROR: ILOs do not match (" + strILO + " != " + oAsset.GetServerOrBlade(intAsset, "ilo") + ")<br/>";
                            if (chkILO.Checked == true)
                                UpdateAsset(intAsset, "ilo", strILO, (boolBlade ? strEnclosure : ""));
                        }
                        if (strDummy != oAsset.GetServerOrBlade(intAsset, "dummy_name"))
                        {
                            strError += " ERROR: Dummy Names do not match (" + strDummy + " != " + oAsset.GetServerOrBlade(intAsset, "dummy_name") + ")<br/>";
                            if (chkDummy.Checked == true)
                                UpdateAsset(intAsset, "dummy_name", strDummy, (boolBlade ? strEnclosure : ""));
                        }
                        if (strClass != oAsset.GetServerOrBlade(intAsset, "class"))
                        {
                            strError += " ERROR: Classes do not match (" + strClass + " != " + oAsset.GetServerOrBlade(intAsset, "class") + ")<br/>";
                            if (chkClass.Checked == true)
                                UpdateAsset(intAsset, "classid", GetClass(strClass).ToString(), (boolBlade ? strEnclosure : ""));
                        }
                        if (strEnvironment != oAsset.GetServerOrBlade(intAsset, "environment"))
                        {
                            strError += " ERROR: Environments do not match (" + strEnvironment + " != " + oAsset.GetServerOrBlade(intAsset, "environment") + ")<br/>";
                            if (chkEnvironment.Checked == true)
                                UpdateAsset(intAsset, "environmentid", GetEnvironment(strEnvironment).ToString(), (boolBlade ? strEnclosure : ""));
                        }
                    }
                    DataSet dsStatus = oAsset.GetStatus(intAsset);
                    if (dsStatus.Tables[0].Rows.Count > 0)
                    {
                        string strName = dsStatus.Tables[0].Rows[0]["name"].ToString();
                        int intUser = Int32.Parse(dsStatus.Tables[0].Rows[0]["userid"].ToString());
                        int intStatus = Int32.Parse(dsStatus.Tables[0].Rows[0]["status"].ToString());
                        if (GetStatus(strStatus) != intStatus)
                        {
                            strError += " ERROR: Status does not match (" + GetStatus(strStatus).ToString() + " != " + intStatus.ToString() + ")<br/>";
                            if (chkStatus.Checked == true)
                                oAsset.AddStatus(intAsset, strName, GetStatus(strStatus), intUser, DateTime.Now);
                        }
                    }
                    else
                        strError += " ERROR: No Status<br/>";
                }
                if (strError != "")
                    Response.Write("<p><b>" + strSerial + " [" + strModel + "]</b><br/>" + strError + "DONE WITH " + strSerial + "<br/></p>");
                strError = "";
                intCount++;
            }
        }
        Response.Write("Done = " + intCount.ToString());
    }
    public void UpdateAsset(int intAsset, string strColumn, string strValue, string strEnclosure)
    {
        string strScript = "";
        bool boolInt = false;
        try
        {
            int intTemp = Int32.Parse(strValue);
            boolInt = true;
        }
        catch { }
        if (strEnclosure != "")
        {
            DataSet dsBlade = SqlHelper.ExecuteDataset(dsnAsset, CommandType.Text, "SELECT TOP 1 * FROM cva_blades WHERE deleted = 0");
            bool boolFound = false;
            foreach (DataColumn dc in dsBlade.Tables[0].Columns)
            {
                if (dc.ColumnName.ToUpper() == strColumn.ToUpper())
                {
                    boolFound = true;
                    break;
                }
            }
            if (boolFound == true)
                strScript = "UPDATE cva_blades SET " + strColumn + " = " + (boolInt ? strValue : "'" + strValue + "'") + " WHERE assetid = " + intAsset.ToString() + " AND deleted = 0";
            else
                strScript = "UPDATE cva_enclosures SET " + strColumn + " = " + (boolInt ? strValue : "'" + strValue + "'") + " WHERE assetid = " + GetEnclosure(strEnclosure).ToString() + " AND deleted = 0";
        }
        else
            strScript = "UPDATE cva_server SET " + strColumn + " = " + (boolInt ? strValue : "'" + strValue + "'") + " WHERE assetid = " + intAsset.ToString() + " AND deleted = 0";
        SqlHelper.ExecuteNonQuery(dsnAsset, CommandType.Text, strScript);
        strError += " <b>SCRIPT:</b> " + strScript + "<br/>";
    }
    public int GetModel(string strName)
    {
        object o = SqlHelper.ExecuteScalar(dsn, CommandType.Text, "SELECT id FROM cv_models_property WHERE name = '" + strName + "' AND enabled = 1 AND deleted = 0");
        if (o == null || o.ToString() == "")
            return 0;
        else
            return Int32.Parse(o.ToString());
    }
    public int GetEnclosure(string strName)
    {
        DataSet dsAssets = SqlHelper.ExecuteDataset(dsnAsset, CommandType.Text, "SELECT assetid FROM cva_status WHERE name = '" + strName + "' AND deleted = 0");
        if (dsAssets.Tables[0].Rows.Count == 0)
            return 0;
        else
            return Int32.Parse(dsAssets.Tables[0].Rows[0]["assetid"].ToString());
    }
    public int GetStatus(string strName)
    {
        int intReturn = 999;
        switch (strName.ToUpper())
        {
            case "IN STOCK":
                intReturn = 1;
                break;
            case "AVAILABLE":
                intReturn = 2;
                break;
            case "IN USE":
                intReturn = 10;
                break;
            case "ARRIVED":
                intReturn = 0;
                break;
            case "RESERVED":
                intReturn = 100;
                break;
        }
        return intReturn;
    }
    private int GetRack(string _name)
    {
        DataSet ds = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT * FROM cv_racks WHERE name = '" + _name + "' AND enabled = 1 AND deleted = 0");
        if (ds.Tables[0].Rows.Count == 0)
            return 0;
        else
            return Int32.Parse(ds.Tables[0].Rows[0]["id"].ToString());
    }
    private int GetRoom(string _name)
    {
        DataSet ds = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT * FROM cv_rooms WHERE name = '" + _name + "' AND enabled = 1 AND deleted = 0");
        if (ds.Tables[0].Rows.Count == 0)
            return 0;
        else
            return Int32.Parse(ds.Tables[0].Rows[0]["id"].ToString());
    }
    private int GetClass(string _name)
    {
        DataSet ds = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT * FROM cv_classs WHERE name = '" + _name + "' AND enabled = 1 AND deleted = 0");
        if (ds.Tables[0].Rows.Count == 0)
            return 0;
        else
            return Int32.Parse(ds.Tables[0].Rows[0]["id"].ToString());
    }
    private int GetEnvironment(string _name)
    {
        DataSet ds = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT * FROM cv_environment WHERE name = '" + _name + "' AND enabled = 1 AND deleted = 0");
        if (ds.Tables[0].Rows.Count == 0)
            return 0;
        else
            return Int32.Parse(ds.Tables[0].Rows[0]["id"].ToString());
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
                <td colspan="2" class="header">Import Servers</td>
            </tr>
            <tr>
                <td colspan="2">&nbsp;</td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:RadioButtonList ID="radType" runat="server" CssClass="default">
                        <asp:ListItem Value="380G5 Test" />
                        <asp:ListItem Value="380G5 Prod" />
                        <asp:ListItem Value="460C Test" />
                        <asp:ListItem Value="460C Prod E" />
                        <asp:ListItem Value="460C Prod H" />
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td colspan="2">&nbsp;</td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:CheckBox ID="chkAsset" runat="server" CssClass="default" Text="Update Asset Tag" /><br />
                    <asp:CheckBox ID="chkRoom" runat="server" CssClass="default" Text="Update Room" /><br />
                    <asp:CheckBox ID="chkRack" runat="server" CssClass="default" Text="Update Rack" /><br />
                    <asp:CheckBox ID="chkRackPos" runat="server" CssClass="default" Text="Update Rack Pos" /><br />
                    <asp:CheckBox ID="chkEnclosure" runat="server" CssClass="default" Text="Update Enclosure" /><br />
                    <asp:CheckBox ID="chkEnclosureBay" runat="server" CssClass="default" Text="Update Enclosure Bay" /><br />
                    <asp:CheckBox ID="chkILO" runat="server" CssClass="default" Text="Update ILO" /><br />
                    <asp:CheckBox ID="chkDummy" runat="server" CssClass="default" Text="Update Dummy" /><br />
                    <asp:CheckBox ID="chkClass" runat="server" CssClass="default" Text="Update Class" /><br />
                    <asp:CheckBox ID="chkEnvironment" runat="server" CssClass="default" Text="Update Environment" /><br />
                    <asp:CheckBox ID="chkStatus" runat="server" CssClass="default" Text="Update Status" /><br />
                    <asp:CheckBox ID="chkDeploy" runat="server" CssClass="default" Text="Deploy Device" /><br />
                </td>
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