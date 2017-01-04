<%@ Page Language="C#" Debug="true" %>
<script runat="server">
    private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
    private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
    private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
    private string dsnServiceEditor = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ServiceEditorDSN"]].ConnectionString;
    private int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
    Servers oServer;
    Forecast oForecast;
    OperatingSystems oOperatingSystem;
    VMWare oVMWare;
    IPAddresses oIPAddresses;
    OnDemand oOnDemand;
    ServerName oServerName;
    int intRebuildStep = 7;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        oServer = new Servers(0, dsn);
        oForecast = new Forecast(0, dsn);
        oOperatingSystem = new OperatingSystems(0, dsn);
        oVMWare = new VMWare(0, dsn);
        oIPAddresses = new IPAddresses(0, dsnIP, dsn);
        oOnDemand = new OnDemand(0, dsn);
        oServerName = new ServerName(0, dsn);

        lblError.Text = "";
        lblHA.CssClass = "default";
        txtName.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnLoad.ClientID + "').click();return false;}} else {return true}; ");
    }
    protected void btnLoad_Click(Object Sender, EventArgs e)
    {
        chkDatastore.Visible = false;
        chkVLAN.Visible = false;
        
        string strName = txtName.Text.Trim().ToUpper();
        txtName.Text = strName;
        DataSet dsServer = oServer.Get(strName);
        if (dsServer.Tables[0].Rows.Count == 1)
        {
            DataRow drServer = dsServer.Tables[0].Rows[0];
            DataSet dsGuest = oVMWare.GetGuest(strName);
            if (dsGuest.Tables[0].Rows.Count == 1)
            {
                DataRow drGuest = dsGuest.Tables[0].Rows[0];
                panContinue.Visible = true;
                int intID = Int32.Parse(drServer["id"].ToString());
                lblID.Text = intID.ToString();
                int intOS = Int32.Parse(drServer["osid"].ToString());
                lblOS.Text = oOperatingSystem.Get(intOS, "name");
                int intStep = Int32.Parse(drServer["step"].ToString());
                btnRebuild.Enabled = (drServer["rebuilding"].ToString() == "1");
                int intAnswer = Int32.Parse(drServer["answerid"].ToString());
                if (oForecast.IsHANone(intAnswer) == true)
                {
                    lblHA.Text = "YES";
                    DataSet dsAnswer = oServer.GetAnswer(intAnswer);
                    if (dsAnswer.Tables[0].Rows.Count > 1)
                    {
                        lblHA.CssClass = "bold";
                        lblHA.Text += " = ";
                        foreach (DataRow drAnswer in dsAnswer.Tables[0].Rows)
                        {
                            int intServer = Int32.Parse(drAnswer["id"].ToString());
                            if (intServer != intID)
                                lblHA.Text += oServer.GetName(intServer, true) + ", ";
                        }
                    }
                    else
                    {
                        lblHA.Text += " (alone)";
                    }
                }
                else
                    lblHA.Text = "Not Applicable";
                int intRebuilding = Int32.Parse(drServer["rebuilding"].ToString());
                if (intRebuilding > 0)
                    lblStatus.Text = "REBUILDING...Step # " + intStep.ToString();
                else if (intStep == 999)
                    lblStatus.Text = "Completed on " + drServer["build_completed"].ToString();
                else
                    lblStatus.Text = "Step # " + intStep.ToString();

                int intHost = Int32.Parse(drGuest["hostid"].ToString());
                int intCluster = Int32.Parse(oVMWare.GetHost(intHost, "clusterid"));
                int intDatastore = Int32.Parse(drGuest["datastoreid"].ToString());
                int intVlan = Int32.Parse(drGuest["vlanid"].ToString());

                ddlCluster.DataValueField = "id";
                ddlCluster.DataTextField = "name";
                ddlCluster.DataSource = GetClusters();
                ddlCluster.DataBind();
                ddlCluster.SelectedValue = intCluster.ToString();
                txtDatastore.Text = oVMWare.GetDatastore(intDatastore, "name");
                txtVLAN.Text = oVMWare.GetVlan(intVlan, "name");

                txtIP1_1.Text = "";
                txtIP1_2.Text = "";
                txtIP1_3.Text = "";
                txtIP1_4.Text = "";
                txtIP2_1.Text = "";
                txtIP2_2.Text = "";
                txtIP2_3.Text = "";
                txtIP2_4.Text = "";
                txtIP3_1.Text = "";
                txtIP3_2.Text = "";
                txtIP3_3.Text = "";
                txtIP3_4.Text = "";
                lblIP1.Text = "";
                lblIP2.Text = "";
                lblIP3.Text = "";
                DataSet dsIP = oServer.GetIP(intID, 0, 0, 0, 0);
                foreach (DataRow drIP in dsIP.Tables[0].Rows)
                {
                    if (drIP["final"].ToString() == "1")
                    {
                        int intIP = Int32.Parse(drIP["ipaddressid"].ToString());
                        if (txtIP1_1.Text == "")
                            LoadIP(intIP, txtIP1_1, txtIP1_2, txtIP1_3, txtIP1_4, lblIP1);
                        else if (txtIP2_1.Text == "")
                            LoadIP(intIP, txtIP2_1, txtIP2_2, txtIP2_3, txtIP2_4, lblIP2);
                        else if (txtIP3_1.Text == "")
                            LoadIP(intIP, txtIP3_1, txtIP3_2, txtIP3_3, txtIP3_4, lblIP3);
                    }
                }
                
                // Error(s)
                panError.Visible = false;
                DataSet dsError = oServer.GetErrors(intID);
                foreach (DataRow drError in dsError.Tables[0].Rows)
                {
                    if (drError["fixed"].ToString() == "" && drError["deleted"].ToString() == "0")
                    {
                        panError.Visible = true;
                        lblError2.Text = drError["reason"].ToString();
                    }
                }
                
            }
            else
            {
                panContinue.Visible = false;
                lblError.Text = "There are " + dsGuest.Tables[0].Rows.Count.ToString() + " VMWARE GUEST (cv_vmware_guests) records with that name.  Unable to continue.";
            }
        }
        else
        {
            panContinue.Visible = false;
            lblError.Text = "There are " + dsServer.Tables[0].Rows.Count.ToString() + " SERVER records (cv_servers) with that name.  Unable to continue.";
        }
    }
    private DataSet GetClusters()
    {
        return SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT * FROM cv_vmware_cluster WHERE enabled = 1 and deleted = 0 ORDER BY name");
    }
    private void LoadIP(int _id, TextBox _ip1, TextBox _ip2, TextBox _ip3, TextBox _ip4, Label _lblIP)
    {
        _lblIP.Text = _id.ToString();
        if (_id > 0)
        {
            string strIP = oIPAddresses.GetName(_id, 0);
            int intIP1 = Int32.Parse(strIP.Substring(0, strIP.IndexOf(".")));
            strIP = strIP.Substring(strIP.IndexOf(".") + 1);
            int intIP2 = Int32.Parse(strIP.Substring(0, strIP.IndexOf(".")));
            strIP = strIP.Substring(strIP.IndexOf(".") + 1);
            int intIP3 = Int32.Parse(strIP.Substring(0, strIP.IndexOf(".")));
            strIP = strIP.Substring(strIP.IndexOf(".") + 1);
            int intIP4 = Int32.Parse(strIP);
            _ip1.Text = intIP1.ToString();
            _ip2.Text = intIP2.ToString();
            _ip3.Text = intIP3.ToString();
            _ip4.Text = intIP4.ToString();
        }
        else
        {
            _ip1.Text = "0";
            _ip2.Text = "0";
            _ip3.Text = "0";
            _ip4.Text = "0";
        }
    }
    protected void btnSave_Click(Object Sender, EventArgs e)
    {
        Save();
    }
    private void CompareIP(int _id, string _ip, string _ip1, string _ip2, string _ip3, string _ip4) 
    {
        if (_ip != "")
        {
            int intIP = Int32.Parse(_ip);
            string strIP = oIPAddresses.GetName(intIP, 0);
            int intIP1 = Int32.Parse(strIP.Substring(0, strIP.IndexOf(".")));
            strIP = strIP.Substring(strIP.IndexOf(".") + 1);
            int intIP2 = Int32.Parse(strIP.Substring(0, strIP.IndexOf(".")));
            strIP = strIP.Substring(strIP.IndexOf(".") + 1);
            int intIP3 = Int32.Parse(strIP.Substring(0, strIP.IndexOf(".")));
            strIP = strIP.Substring(strIP.IndexOf(".") + 1);
            int intIP4 = Int32.Parse(strIP);

            if (_ip1 == "" || _ip2 == "" || _ip3 == "" || _ip4 == "")
                oServer.DeleteIP(_id, intIP, dsnIP);
            else
            {
                int intIPNew1 = Int32.Parse(_ip1);
                int intIPNew2 = Int32.Parse(_ip2);
                int intIPNew3 = Int32.Parse(_ip3);
                int intIPNew4 = Int32.Parse(_ip4);

                if (intIP1 != intIPNew1 || intIP2 != intIPNew2 || intIP3 != intIPNew3 || intIP4 != intIPNew4)
                    SqlHelper.ExecuteNonQuery(dsnIP, CommandType.Text, "UPDATE cv_ip_addresses SET add1 = " + intIPNew1.ToString() + ", add2 = " + intIPNew2.ToString() + ", add3 = " + intIPNew3.ToString() + ", add4 = " + intIPNew4.ToString() + " WHERE id = " + intIP.ToString());
            }
        }
        else if (_ip1 != "" || _ip2 != "" || _ip3 != "" || _ip4 != "")
        {
            int intIP = oIPAddresses.Add(0, Int32.Parse(_ip1), Int32.Parse(_ip2), Int32.Parse(_ip3), Int32.Parse(_ip4), 0);
            oServer.AddIP(_id, intIP, 0, 1, 0, 0);
        }
    }
    private int GetVLAN(int _clusterid, string _name, bool _create)
    {
        DataSet ds = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT * FROM cv_vmware_vlan WHERE clusterid = " + _clusterid.ToString() + " AND name = '" + _name + "' AND enabled = 1 AND deleted = 0");
        if (ds.Tables[0].Rows.Count > 0)
            return Int32.Parse(ds.Tables[0].Rows[0]["id"].ToString());
        else if (_create == true)
        {
            oVMWare.AddVlan(_clusterid, _name, 1);
            return GetVLAN(_clusterid, _name, false);
        }
        else
            return 0;
    }
    private int GetDatastore(int _clusterid, string _name, bool _create)
    {
        DataSet ds = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT * FROM cv_vmware_datastore WHERE clusterid = " + _clusterid.ToString() + " AND name = '" + _name + "' AND enabled = 1 AND deleted = 0");
        if (ds.Tables[0].Rows.Count > 0)
            return Int32.Parse(ds.Tables[0].Rows[0]["id"].ToString());
        else if (_create == true)
        {
            oVMWare.AddDatastore(_clusterid, _name, 100, 1, 0, 1);
            return GetDatastore(_clusterid, _name, false);
        }
        else
            return 0;
    }
    
    private void btnCancel_Click(Object Sender, EventArgs e) 
    {
        panBuild.Visible = false;
        panRebuild.Visible = false;
        panContinue.Visible = true;
    }
    private void btnRebuild_Click(Object Sender, EventArgs e)
    {
        panContinue.Visible = false;
        panRebuild.Visible = true;
    }
    private void btnRebuild2_Click(Object Sender, EventArgs e)
    {
        // Reset current build
        int intID = Int32.Parse(lblID.Text);
        oServer.UpdateRebuilding(intID, 0);
        oServer.UpdateStep(intID, 999);
        DataSet dsServer = SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT * FROM cv_ondemand_steps_done_server WHERE serverid = " + intID.ToString() + " AND step < 0");
        foreach (DataRow drServer in dsServer.Tables[0].Rows)
        {
            int intStepNegative = Int32.Parse(drServer["step"].ToString());
            int intStep = intStepNegative * -1;
            SqlHelper.ExecuteNonQuery(dsn, CommandType.Text, "DELETE FROM cv_ondemand_steps_done_server WHERE serverid = " + intID.ToString() + " AND step = " + intStep.ToString());
            SqlHelper.ExecuteNonQuery(dsn, CommandType.Text, "UPDATE cv_ondemand_steps_done_server SET step = " + intStep.ToString() + " WHERE serverid = " + intID.ToString() + " AND step = " + intStepNegative.ToString());
            lblError.Text = "<img src='/images/ico_check.gif' border='0' align='absmiddle'/> Build has been Reset!";
        }
        
        // Start the new build
        StartBuild();
    }
    private void btnBuild_Click(Object Sender, EventArgs e)
    {
        panBuild.Visible = false;
        // Start the new build
        StartBuild();
    }
    private void btnSaveStart_Click(Object Sender, EventArgs e)
    {
        if (Save() == true)
            panBuild.Visible = true;
    }
    private void btnError_Click(Object Sender, EventArgs e)
    {
        int intID = Int32.Parse(lblID.Text);
        SqlHelper.ExecuteNonQuery(dsn, CommandType.Text, "UPDATE cv_servers_errors SET deleted = 1 WHERE serverid = " + intID.ToString() + " AND deleted = 0 AND fixed IS NULL");
        panError.Visible = false;
    }

    private bool Save()
    {
        int intID = Int32.Parse(lblID.Text);
        int intCluster = Int32.Parse(ddlCluster.SelectedItem.Value);
        DataSet dsHosts = oVMWare.GetHosts(intCluster, 1);
        int intHost = Int32.Parse(dsHosts.Tables[0].Rows[0]["id"].ToString());
        int intDatastore = GetDatastore(intCluster, txtDatastore.Text, (chkDatastore.Visible && chkDatastore.Checked));
        chkDatastore.Visible = (intDatastore == 0);
        int intVLAN = GetVLAN(intCluster, txtVLAN.Text, (chkVLAN.Visible && chkVLAN.Checked));
        chkVLAN.Visible = (intVLAN == 0);
        if (intDatastore > 0 && intVLAN > 0)
        {
            // Save IPs
            CompareIP(intID, lblIP1.Text, txtIP1_1.Text, txtIP1_2.Text, txtIP1_3.Text, txtIP1_4.Text);
            CompareIP(intID, lblIP2.Text, txtIP2_1.Text, txtIP2_2.Text, txtIP2_3.Text, txtIP2_4.Text);
            CompareIP(intID, lblIP3.Text, txtIP3_1.Text, txtIP3_2.Text, txtIP3_3.Text, txtIP3_4.Text);

            oVMWare.DeleteGuest(txtName.Text);
            SqlHelper.ExecuteNonQuery(dsn, CommandType.Text, "insert into cv_vmware_guests select top 1 " + intHost.ToString() + ", classid, environmentid, addressid, " + intDatastore.ToString() + ", " + intVLAN.ToString() + ", poolid, name, allocated, 'N/A', vim, 0, created, modified, 0 FROM cv_vmware_guests WHERE name = '" + txtName.Text + "' order by ID desc");
            lblError.Text = "<img src='/images/ico_check.gif' border='0' align='absmiddle'/> Data Saved!";
            panContinue.Visible = false;
            return true;
        }
        else
        {
            lblError.Text = "Please confirm the information below...";
            return false;
        }
    }
    private void StartBuild()
    {
        int intID = Int32.Parse(lblID.Text);
        oServer.UpdateStep(intID, intRebuildStep);
        oOnDemand.UpdateStepDoneServerRedo(intID, intRebuildStep);
        oServer.UpdateRebuilding(intID, 1);
        /*
        // Delete Audits
        oAudit.DeleteServer(intServer);
        // Set installs back
        DataSet dsInstalls = oServerName.GetComponentDetailSelected(intServer, 0);
        foreach (DataRow drInstall in dsInstalls.Tables[0].Rows)
            oServerName.UpdateComponentDetailSelected(intServer, Int32.Parse(drInstall["detailid"].ToString()), -2);
        */
        
        // Clear Errors
        SqlHelper.ExecuteNonQuery(dsn, CommandType.Text, "UPDATE cv_servers_errors SET deleted = 1 WHERE serverid = " + intID.ToString() + " AND deleted = 0 AND fixed IS NULL");
        
        lblError.Text = "<img src='/images/ico_check.gif' border='0' align='absmiddle'/> Build Started!";
        panBuild.Visible = false;
        panRebuild.Visible = false;
    }
</script>
<script type="text/javascript">
</script>
<html>
<head>
<title>VMware Server Rebuild</title>
<link rel="stylesheet" type="text/css" href="/css/default.css" />
</head>
<body leftmargin="0" topmargin="0">
<form id="Form1" runat="server">
    <table cellpadding="5" cellspacing="3">
        <tr>
            <td colspan="2"><asp:Label ID="lblError" runat="server" CssClass="header" /></td>
        </tr>
        <tr>
            <td>Server Name:</td>
            <td><asp:TextBox ID="txtName" runat="server" Width="200" /></td>
        </tr>
        <tr>
            <td></td>
            <td><asp:Button ID="btnLoad" runat="server" Text="Load VMware Guest" OnClick="btnLoad_Click" /></td>
        </tr>
    </table>
    <asp:Panel ID="panContinue" runat="server" Visible="false">
    <br />
    <table cellpadding="5" cellspacing="3">
        <tr>
            <td>Server ID:</td>
            <td colspan="2"><asp:Label ID="lblID" runat="server" /></td>
        </tr>
        <tr>
            <td>Operating System:</td>
            <td colspan="2"><asp:Label ID="lblOS" runat="server" /></td>
        </tr>
        <tr>
            <td>Load Balanced:</td>
            <td colspan="2"><asp:Label ID="lblHA" runat="server" /></td>
        </tr>
        <tr id="panError" runat="server" visible="false">
            <td>ERROR:</td>
            <td><asp:Label ID="lblError2" runat="server" CssClass="reddefault"/></td>
            <td><asp:Button ID="btnError" runat="server" Text="Clear Error" OnClick="btnError_Click" /></td>
        </tr>
        <tr>
            <td>Current Status:</td>
            <td><asp:Label ID="lblStatus" runat="server" /></td>
            <td><asp:Button ID="btnRebuild" runat="server" Text="Reset Rebuild" OnClick="btnRebuild_Click" Enabled="false" /></td>
        </tr>
        <tr>
            <td>Cluster:</td>
            <td colspan="2"><asp:DropDownList ID="ddlCluster" runat="server" Width="200" /></td>
        </tr>
        <tr>
            <td>VLAN:</td>
            <td><asp:TextBox ID="txtVLAN" runat="server" Width="200" /></td>
            <td><asp:CheckBox ID="chkVLAN" runat="server" CssClass="reddefault" Visible="false" Text="The VLAN does not exist in the database. Check here to create it." /></td>
        </tr>
        <tr>
            <td></td>
            <td colspan="2"><b>WARNING:</b> This field is <u>case sensitive</u> and MUST exist in the cluster for the rebuild to work.</td>
        </tr>
        <tr>
            <td>Datastore:</td>
            <td><asp:TextBox ID="txtDatastore" runat="server" Width="300" /></td>
            <td><asp:CheckBox ID="chkDatastore" runat="server" CssClass="reddefault" Visible="false" Text="The DATASTORE does not exist in the database. Check here to create it." /></td>
        </tr>
        <tr>
            <td></td>
            <td colspan="2"><b>WARNING:</b> This field is <u>case sensitive</u> and MUST exist in the cluster for the rebuild to work.</td>
        </tr>
        <tr>
            <td>IP Address # 1:</td>
            <td colspan="2"><asp:TextBox ID="txtIP1_1" CssClass="default" runat="server" Width="50" MaxLength="3"/> . <asp:TextBox ID="txtIP1_2" CssClass="default" runat="server" Width="50" MaxLength="3"/> . <asp:TextBox ID="txtIP1_3" CssClass="default" runat="server" Width="50" MaxLength="3"/> . <asp:TextBox ID="txtIP1_4" CssClass="default" runat="server" Width="50" MaxLength="3"/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblIP1" runat="server" /></td>
        </tr>
        <tr>
            <td>IP Address # 2:</td>
            <td colspan="2"><asp:TextBox ID="txtIP2_1" CssClass="default" runat="server" Width="50" MaxLength="3"/> . <asp:TextBox ID="txtIP2_2" CssClass="default" runat="server" Width="50" MaxLength="3"/> . <asp:TextBox ID="txtIP2_3" CssClass="default" runat="server" Width="50" MaxLength="3"/> . <asp:TextBox ID="txtIP2_4" CssClass="default" runat="server" Width="50" MaxLength="3"/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblIP2" runat="server" /></td>
        </tr>
        <tr>
            <td>IP Address # 3:</td>
            <td colspan="2"><asp:TextBox ID="txtIP3_1" CssClass="default" runat="server" Width="50" MaxLength="3"/> . <asp:TextBox ID="txtIP3_2" CssClass="default" runat="server" Width="50" MaxLength="3"/> . <asp:TextBox ID="txtIP3_3" CssClass="default" runat="server" Width="50" MaxLength="3"/> . <asp:TextBox ID="txtIP3_4" CssClass="default" runat="server" Width="50" MaxLength="3"/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblIP3" runat="server" /></td>
        </tr>
        <tr>
            <td></td>
            <td colspan="2">
                <asp:Button ID="btnSave" runat="server" Text="Save ONLY" OnClick="btnSave_Click" /> 
                <asp:Button ID="btnSaveStart" runat="server" Text="Save and Start Build" OnClick="btnSaveStart_Click" />
            </td>
        </tr>
    </table>
    </asp:Panel>

    <asp:Panel ID="panBuild" runat="server" Visible="false">
    <br />
    <table cellpadding="5" cellspacing="3">
        <tr>
            <td><b>NOTE:</b> Once you click this button, the build will start immediately...</td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="btnBuild" runat="server" Text="Rebuild" OnClick="btnBuild_Click" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
            </td>
        </tr>
    </table>
    </asp:Panel>

    <asp:Panel ID="panRebuild" runat="server" Visible="false">
    <br />
    <table cellpadding="5" cellspacing="3">
        <tr>
            <td class="header">Please Read....</td>
        </tr>
        <tr>
            <td>When resetting a rebuild, you need to make sure the VMware guest has been removed from Virtual Center.</td>
        </tr>
        <tr>
            <td><b>NOTE:</b> Once you click this button, the build will start immediately...</td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="btnRebuild2" runat="server" Text="Reset Rebuild" OnClick="btnRebuild2_Click" />
                <asp:Button ID="btnCancel2" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
            </td>
        </tr>
    </table>
    </asp:Panel>
</form>
</body>
</html>