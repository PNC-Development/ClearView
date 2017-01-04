<%@ Page Language="C#" Debug="true" %>
<%@ Import Namespace="NCC.ClearView.Application.Core.Proteus" %>
<%@ Import Namespace="NCC.ClearView.Application.Core.w08r2" %>
<%@ Import Namespace="System.Threading" %>

<script runat="server">
    public enum ValidationRestriction
    {
        Basic = 0,
        Numeric = 1,
        IPAddress = 2,
        MACAddress = 3
    }
    public enum Validation
    {
        Required = 0,
        Optional = 1
    }
    
    private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
    private string dsnZeus = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ZeusDSN"]].ConnectionString;
    protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
    protected bool boolOSWindows = false;
    protected bool boolOSLinux = false;
    protected bool boolOSSolaris = false;
    protected bool boolOSAIX = false;
    protected bool boolOSOther = false;
    protected bool boolTypeVirtual = false;
    protected bool boolTypePhysical = false;
    protected string strSpacer = "<img src='/images/spacer.gif' border='0' width='50' height='1' />";
    protected int intID = -999;

    protected OperatingSystems oOperatingSystem;
    protected Log oLog;
    protected Zeus oZeus;
    protected Functions oFunction;
    protected Variables oVariable;

    protected void Page_Load(object sender, EventArgs e)
    {
        oOperatingSystem = new OperatingSystems(0, dsn);
        oLog = new Log(0, dsn);
        oZeus = new Zeus(0, dsnZeus);
        oFunction = new Functions(0, dsn, intEnvironment);
        oVariable = new Variables(intEnvironment);

        panInformation.Style["visibility"] = "hidden";
        if (!IsPostBack)
        {
            LoadLists();
            txtSearch.Focus();
        }

    }
    protected void LoadLists()
    {
        DataSet dsOS = oOperatingSystem.Gets(0, 1);
        DataView dvOS = dsOS.Tables[0].DefaultView;
        dvOS.RowFilter = "factory_code <> ''";
        LoadList(dvOS, ddlOS, "select");
        //LoadList(cBuildLocations, ddlMDT, "select", "name", "key");
    }
    protected void ddlOS_Change(object sender, EventArgs e)
    {
        RefreshForm();
    }
    protected void radVirtual_Change(object sender, EventArgs e)
    {
        RefreshForm();
    }
    protected void btnRegister_Click(object sender, EventArgs e)
    {
        RefreshForm();
        ulErrors.Controls.Clear();
        bool Valid = true;

        Valid = AddError(txtName, "Enter a server name", Valid, ValidationRestriction.Basic, Validation.Required, ulErrors, panError);
        Valid = AddError(ddlBackup, "Select the backup software", Valid, ulErrors, panError);
        Valid = AddError(txtIPServer, "Enter a valid server IP address", Valid, ValidationRestriction.IPAddress, Validation.Optional, ulErrors, panError);
        Valid = AddError(txtIPServerVLAN, txtIPServer, "Enter the VLAN of the server IP address", Valid, ValidationRestriction.Numeric, ulErrors, panError);
        Valid = AddError(txtIPServerMask, txtIPServer, "Enter the subnet mask of the server IP address", Valid, ValidationRestriction.IPAddress, ulErrors, panError);
        Valid = AddError(txtIPServerGateway, txtIPServer, "Enter the gateway of the server IP address", Valid, ValidationRestriction.IPAddress, ulErrors, panError);
        Valid = AddError(txtIPBackup, "Enter a valid bakup IP address", Valid, ValidationRestriction.IPAddress, Validation.Optional, ulErrors, panError);
        Valid = AddError(txtIPBackupVLAN, txtIPBackup, "Enter the VLAN of the backup IP address", Valid, ValidationRestriction.Numeric, ulErrors, panError);
        Valid = AddError(txtIPBackupMask, txtIPBackup, "Enter the subnet mask of the backup IP address", Valid, ValidationRestriction.IPAddress, ulErrors, panError);
        Valid = AddError(txtIPBackupGateway, txtIPBackup, "Enter the gateway of the backup IP address", Valid, ValidationRestriction.IPAddress, ulErrors, panError);
        Valid = AddError(ddlOS, "Select the operating system", Valid, ulErrors, panError);

        if (boolOSWindows || boolOSLinux)
            Valid = AddError(ddlMDT, "Select the build location", Valid, ulErrors, panError);
        if (boolOSWindows)
            Valid = AddError(ddlDomainWindows, "Select the domain", Valid, ulErrors, panError);
        if (boolOSLinux)
            Valid = AddError(ddlDomainLinux, "Select the domain", Valid, ulErrors, panError);
        if (boolOSWindows || boolOSLinux || boolOSSolaris)
            Valid = AddError(new RadioButton[] { radVirtualYes, radVirtualNo }, "Choose if this is a virtual machine", Valid, ulErrors, panError);
        if (boolTypeVirtual)
        {
            if (boolOSWindows || boolOSLinux)
                Valid = AddError(txtMac, "Enter the MAC address of the server", Valid, ValidationRestriction.MACAddress, Validation.Required, ulErrors, panError);
        }
        if (boolTypePhysical)
        {
            Valid = AddError(txtSerial, "Enter the serial number", Valid, ValidationRestriction.Basic, Validation.Required, ulErrors, panError);
            Valid = AddError(txtMacPrimary, "Enter the primary MAC address of the server", Valid, ValidationRestriction.MACAddress, Validation.Required, ulErrors, panError);
            if (boolOSSolaris)
            {
                Valid = AddError(ddlBuildType, "Select the build type", Valid, ulErrors, panError);
                Valid = AddError(ddlDomainSolaris, "Select the domain", Valid, ulErrors, panError);
                Valid = AddError(ddlSource, "Select the source", Valid, ulErrors, panError);
                Valid = AddError(new RadioButton[] { radBootLocal, radBootSan }, "Select the boot type", Valid, ulErrors, panError);
                Valid = AddError(new RadioButton[] { radEcomYes, radEcomNo }, "Choose if this is going into the E-commerce environment", Valid, ulErrors, panError);
                Valid = AddError(ddlBuildNetwork, "Select the build network", Valid, ulErrors, panError);
                Valid = AddError(ddlCGI, "Select the CGI script location", Valid, ulErrors, panError);
                Valid = AddError(ddlInterface1, "Select interface # 1", Valid, ulErrors, panError);
                Valid = AddError(txtMPIP1, "Enter a valid MP IP address # 1", Valid, ValidationRestriction.IPAddress, Validation.Required, ulErrors, panError);
                Valid = AddError(txtMPIP1VLAN, txtMPIP1, "Enter the VLAN of MP IP address # 1", Valid, ValidationRestriction.Numeric, ulErrors, panError);
                Valid = AddError(ddlInterface2, "Select interface # 2", Valid, ulErrors, panError);
                Valid = AddError(txtMPIP2, "Enter a valid MP IP address # 2", Valid, ValidationRestriction.IPAddress, Validation.Required, ulErrors, panError);
                Valid = AddError(txtMPIP2VLAN, txtMPIP2, "Enter the VLAN of MP IP address # 2", Valid, ValidationRestriction.Numeric, ulErrors, panError);
            }
        }


        if (Valid)
        {
            string strName = txtName.Text.Trim();
            string strBackup = ddlBackup.SelectedValue;
            string strSerial = txtSerial.Text.Trim();
            string strAsset = txtAsset.Text.Trim();
            int intOS = ParseInt(ddlOS.SelectedItem.Value);
            string strIP = txtIPServer.Text.Trim();
            string strIPVlan = txtIPServerVLAN.Text.Trim();
            string strIPBackup = txtIPBackup.Text.Trim();
            string strIPBackupVlan = txtIPBackupVLAN.Text.Trim();

            string strArrayConfig = "PNCBASIC30";
            if (radVirtualYes.Checked)
                strArrayConfig = "BASIC";

            string strMAC1 = txtMacPrimary.Text;
            string strMAC2 = txtMacSecondary.Text;
            if (boolTypeVirtual)
            {
                strMAC1 = txtMac.Text;
                strMAC2 = "";
            }

            string strError = "";
            try
            {
                bool boolMDT = false;
                if (boolOSWindows)
                {
                    oZeus.AddBuild(intID, intID, intID, strSerial, strAsset, strName, strArrayConfig, oOperatingSystem.Get(intOS, "zeus_os"), oOperatingSystem.Get(intOS, "zeus_os_version"), 0, oOperatingSystem.Get(intOS, "zeus_build_type"), ddlDomainWindows.SelectedItem.Value, intEnvironment, "SERVER", 0, strMAC1, strMAC2, strIP, strIPVlan, strIPBackup, strIPBackupVlan, "", "", "", "", ddlMDT.SelectedItem.Value, "", 1);
                    // Add Applications
                    foreach (ListItem li in chkComponentsWindows.Items)
                    {
                        if (li.Selected)
                            oZeus.AddApp(strSerial, li.Value, "");
                    }
                    boolMDT = true;
                }
                else if (boolOSLinux)
                {
                    oZeus.AddBuild(intID, intID, intID, strSerial, strAsset, strName, strArrayConfig, oOperatingSystem.Get(intOS, "name"), "", 0, "", ddlDomainLinux.SelectedItem.Value, intEnvironment, "SERVER", 0, strMAC1, strMAC2, strIP, strIPVlan, strIPBackup, strIPBackupVlan, "", "", "", "", ddlMDT.SelectedItem.Value, "", 1);
                    // Add Applications
                    foreach (ListItem li in chkComponentsLinux.Items)
                    {
                        if (li.Selected)
                            oZeus.AddApp(strSerial, li.Value, "");
                    }
                    boolMDT = true;
                }
                else if (boolOSSolaris)
                {
                    string strZeusBuildTypeSolaris = ddlBuildType.SelectedItem.Value;
                    string strSolarisDomain = ddlDomainSolaris.SelectedItem.Value;
                    string strSolarisBuildFlags = (radBootLocal.Checked ? "P,T,D" : "P");
                    if (radEcomYes.Checked)
                        strSolarisBuildFlags += ",E";
                    string strSource = ddlSource.SelectedItem.Value;
                    string strSolarisMAC = oFunction.FormatMAC(strMAC1, ":");
                    string strJumpstartCGI = ddlCGI.SelectedItem.Value;
                    string strSolarisBuildNetwork = ddlBuildNetwork.SelectedItem.Value;
                    string strJumpstartBuildType = "dhcp";

                    oZeus.AddBuild(intID, intID, intID, strSerial, strAsset, strName.ToLower(), strArrayConfig, oOperatingSystem.Get(intOS, "zeus_os"), oOperatingSystem.Get(intOS, "zeus_os_version"), 0, strZeusBuildTypeSolaris, strSolarisDomain, intEnvironment, strSource, 0, strSolarisMAC, "", strIP, strIPVlan, strIPBackup, strIPBackupVlan, ddlInterface1.SelectedItem.Value, txtMPIP1.Text, ddlInterface2.SelectedItem.Value, txtMPIP2.Text, strSolarisBuildFlags, oVariable.SolarisFlags(strJumpstartCGI, strSolarisBuildNetwork, strJumpstartBuildType, false), 1);

                    // Register
                    string strJumpstartURL = "http://" + strJumpstartCGI + "/cgi-bin/jumpstart.cgi?1010,MACADDR=" + strSolarisMAC + ",create_host_info";
                    oLog.AddEvent(intID, strName, strSerial, "Beginning HOST.INFO registration on jumpstart URL = " + strJumpstartURL, LoggingType.Information);
                    if (RunCGI(strJumpstartURL) == true)
                    {
                        oLog.AddEvent(intID, strName, strSerial, "Success!! HOST.INFO registration on jumpstart URL = " + strJumpstartURL, LoggingType.Information);
                    }
                    else
                    {
                        strError = "CREATE_HOST_INFO registration ~ Jumpstart URL = " + strJumpstartURL + " failed.";
                        oLog.AddEvent(intID, strName, strSerial, "Failure!! HOST.INFO registration on jumpstart URL = " + strJumpstartURL, LoggingType.Error);
                    }
                    strJumpstartURL = "http://" + strJumpstartCGI + "/cgi-bin/jumpstart.cgi?1010,MACADDR=" + strSolarisMAC + ",add_client";
                    oLog.AddEvent(intID, strName, strSerial, "Beginning Client Configuration on jumpstart URL = " + strJumpstartURL, LoggingType.Information);
                    if (RunCGI(strJumpstartURL) == true)
                    {
                        oLog.AddEvent(intID, strName, strSerial, "Success!! Client Configuration on jumpstart URL = " + strJumpstartURL, LoggingType.Information);
                    }
                    else
                    {
                        strError = "ADD_CLIENT registration ~ Jumpstart URL = " + strJumpstartURL + " failed.";
                        oLog.AddEvent(intID, strName, strSerial, "Failure!! Client Configuration on jumpstart URL = " + strJumpstartURL, LoggingType.Error);
                    }
                }
                else if (boolOSAIX)
                {
                    strError = "AIX is unavailable at this time.";
                }
                else
                {
                    // Other
                    strError = "The selected operating system is unavailable at this time.";
                }

                if (boolMDT)
                {
                    // Register MDT
                    string strRDPMDTWebService = ddlMDT.SelectedItem.Value;
                    System.Net.NetworkCredential oCredentials = new System.Net.NetworkCredential(oVariable.ADUser(), oVariable.ADPassword(), oVariable.Domain());
                    BuildSubmit oMDT = new BuildSubmit();
                    oMDT.Credentials = oCredentials;
                    oMDT.Url = strRDPMDTWebService;
                    string strMDTMAC1 = oFunction.FormatMAC(strMAC1, ":");
                    oLog.AddEvent(intID, strName, strSerial, "Running ForceCleanup MDT on " + strRDPMDTWebService + " (" + strName + ", " + strMDTMAC1 + ", " + "ServerShare" + ")", LoggingType.Information);
                    oMDT.ForceCleanup(strName, strMDTMAC1, "ServerShare");
                    string strMDTMAC2 = "";
                    if (strMAC2 != "")
                    {
                        strMDTMAC2 = oFunction.FormatMAC(strMAC2, ":");
                        oLog.AddEvent(intID, strName, strSerial, "Waiting 60 seconds for Active Directory to Sychronize...", LoggingType.Debug);
                        Thread.Sleep(60000); // Wait 60 seconds
                        oLog.AddEvent(intID, strName, strSerial, "Running ForceCleanup MDT on " + strRDPMDTWebService + " (" + strName + ", " + strMDTMAC2 + ", " + "ServerShare" + ")", LoggingType.Information);
                        oMDT.ForceCleanup(strName, strMDTMAC2, "ServerShare");
                    }
                    string[] strExtendedMDT1 = new string[] { "PNCBACKUPSOFTWARE:" + strBackup, "IISInstall:" + (chkIIS.Checked ? "YES" : "NO"), "HWConfig:" + (radBootLocal.Checked ? "DellLD" : "DEFAULT"), "ESMInstall:" + (chkESM.Checked ? "YES" : "NO") + ", ClearViewInstall:YES", "Teamed2:" + strMDTMAC2, "HIDSInstall:" + (chkHIDS.Checked ? "YES" : "NO") };
                    string strExtendedMDTs1 = "";
                    foreach (string extendedMDT in strExtendedMDT1)
                    {
                        if (strExtendedMDTs1 != "")
                            strExtendedMDTs1 += ", ";
                        strExtendedMDTs1 += extendedMDT;
                    }
                    string strBootEnvironment = oOperatingSystem.Get(intOS, "boot_environment");
                    string strTaskSequence = oOperatingSystem.Get(intOS, "task_sequence");
                    string strDomain = ddlDomainWindows.SelectedItem.Value;
                    oLog.AddEvent(intID, strName, strSerial, "Registering MDT on " + strRDPMDTWebService + ", automatedBuild2 (" + strName + ", " + strMDTMAC1 + ", " + strBootEnvironment + ", " + "provision" + ", " + strDomain + ", " + strTaskSequence + ", " + "ServerShare" + ", ExtendedValues=" + strExtendedMDTs1 + ")", LoggingType.Information);
                    oMDT.automatedBuild2(strName, strMDTMAC1, strBootEnvironment, "provision", strDomain, strTaskSequence, "ServerShare", strExtendedMDT1);
                    if (strMDTMAC2 != "")
                    {
                        string[] strExtendedMDT2 = new string[] { "PNCBACKUPSOFTWARE:" + strBackup, "IISInstall:" + (chkIIS.Checked ? "YES" : "NO"), "HWConfig:" + (radBootLocal.Checked ? "DellLD" : "DEFAULT"), "ESMInstall:" + (chkESM.Checked ? "YES" : "NO") + ", ClearViewInstall:YES", "Teamed2:" + strMDTMAC1, "HIDSInstall:" + (chkHIDS.Checked ? "YES" : "NO") };
                        string strExtendedMDTs2 = "";
                        foreach (string extendedMDT in strExtendedMDT2)
                        {
                            if (strExtendedMDTs2 != "")
                                strExtendedMDTs2 += ", ";
                            strExtendedMDTs2 += extendedMDT;
                        }
                        oLog.AddEvent(intID, strName, strSerial, "Waiting 60 seconds for Active Directory to Sychronize...", LoggingType.Debug);
                        Thread.Sleep(60000); // Wait 60 seconds
                        oLog.AddEvent(intID, strName, strSerial, "Registering MDT on " + strRDPMDTWebService + ", automatedBuild2 (" + strName + ", " + strMDTMAC2 + ", " + strBootEnvironment + ", " + "provision" + ", " + strDomain + ", " + strTaskSequence + ", " + "ServerShare" + ", ExtendedValues=" + strExtendedMDTs2 + ")", LoggingType.Information);
                        oMDT.automatedBuild2(strName, strMDTMAC1, strBootEnvironment, "provision", strDomain, strTaskSequence, "ServerShare", strExtendedMDT2);
                    }
                    oLog.AddEvent(intID, strName, strSerial, "MDT has been configured", LoggingType.Information);
                }

            }
            catch (Exception ex)
            {
                strError = ex.Message;
            }


            if (strError != "")
            {
                panSuccess.Visible = true;
                litName.Text = txtName.Text.ToUpper();
            }
            else
                AddError(strError, ulErrors2, panError2);
        }
        // ArrayConfig
        //      PNCBASIC30      = Windows / Linux Blade, All Solaris
        //      BASIC           = VMware
        //      SANCONNECTEDF   = Rackmount (not just BFS?)
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        Response.Redirect(Request.Path);
    }
    protected DataSet Search()
    {
        ClearMessages();
        ulSearch.Controls.Clear();
        bool Valid = true;

        Valid = AddError(txtSearch, "Enter a server name", Valid, ValidationRestriction.Basic, Validation.Required, ulSearch, panSearch);

        if (Valid)
        {
            DataSet dsSearch = oZeus.GetBuildServer(txtSearch.Text);
            if (dsSearch.Tables[0].Rows.Count < 1)
                Valid = AddError("Server not found. Try again...", ulSearch, panSearch);
            else if (dsSearch.Tables[0].Rows.Count > 1)
                Valid = AddError(dsSearch.Tables[0].Rows.Count.ToString() + "records returned. Try again...", ulSearch, panSearch);
            else
                return dsSearch;
        }
        return null;
    }
    protected void btnStatus_Click(object sender, EventArgs e)
    {
        DataSet dsSearch = Search();
        if (dsSearch != null)
        {
            panInformation.Style["visibility"] = "visible";
            DataRow drSearch = dsSearch.Tables[0].Rows[0];
            lblName.Text = drSearch["name"].ToString();
            lblSerial.Text = drSearch["serial"].ToString();
            lblAsset.Text = drSearch["asset"].ToString();
            lblArrayConfig.Text = drSearch["array_config"].ToString();
            lblOS.Text = drSearch["os"].ToString();
            lblVersion.Text = drSearch["os_version"].ToString();
            lblServicePack.Text = drSearch["sp"].ToString();
            lblBuildType.Text = drSearch["build_type"].ToString();
            lblDomain.Text = drSearch["domain"].ToString();
            lblSource.Text = drSearch["source"].ToString();
            lblDHCP.Text = drSearch["dhcp"].ToString();
            btnDHCP.Attributes.Add("onclick", "window.prompt(\"Copy to clipboard: Ctrl+C, Enter\", \"" + lblDHCP.Text + "\");return false;");
            btnDHCP.Visible = (lblDHCP.Text != "");
            lblMAC1.Text = drSearch["macaddress"].ToString();
            lblMAC2.Text = drSearch["macaddress2"].ToString();
            lblIP.Text = drSearch["ipaddress"].ToString();
            lblIPvlan.Text = drSearch["ipaddress_vlan"].ToString();
            //lblIPmask.Text = drSearch["ipaddress_subnet"].ToString();
            //lblIPgateway.Text = drSearch["ipaddress_gateway"].ToString();
            lblBackupIP.Text = drSearch["ipbackup"].ToString();
            lblBackupIPvlan.Text = drSearch["ipbackup_vlan"].ToString();
            //lblBackupIPmask.Text = drSearch["ipbackup_subnet"].ToString();
            //lblBackupIPgateway.Text = drSearch["ipbackup_gateway"].ToString();
            lblInterface1.Text = drSearch["interface1"].ToString();
            lblMPIP1.Text = drSearch["mpipaddress1"].ToString();
            lblInterface2.Text = drSearch["interface2"].ToString();
            lblMPIP2.Text = drSearch["mpipaddress2"].ToString();
            lblBuildFlags.Text = drSearch["build_flags"].ToString();
            lblFlags.Text = drSearch["flags"].ToString();
            lblRegistered.Text = drSearch["created"].ToString();

            if (drSearch["dhcp"].ToString() != "")
            {
                lblStatus.Text = "The server build has <b>completed</b>.";
                btnCleanup.Enabled = true;
            }
            else
                lblStatus.Text = "The server is currently <b>building</b>.";
        }
    }
    protected void btnCleanup_Click(object sender, EventArgs e)
    {
        DataSet dsSearch = Search();
        if (dsSearch != null)
        {
            string strName = dsSearch.Tables[0].Rows[0]["name"].ToString();
            string strSerial = dsSearch.Tables[0].Rows[0]["serial"].ToString();
            string strAsset = dsSearch.Tables[0].Rows[0]["asset"].ToString();

            string strError = "";
            try
            {
                bool boolMDT = false;
                if (boolOSWindows)
                {
                    oZeus.DeleteBuildName(strName);
                    boolMDT = true;
                }
                else if (boolOSLinux)
                {
                    oZeus.DeleteBuildName(strName);
                    boolMDT = true;
                }
                else if (boolOSSolaris)
                {
                    string strSolarisMAC = dsSearch.Tables[0].Rows[0]["macaddress"].ToString();
                    string[] strFlags = dsSearch.Tables[0].Rows[0]["flags"].ToString().Split(new char[] { ',' });
                    string strJumpstartCGI = "";
                    foreach (string strFlag in strFlags)
                    {
                        if (strFlag.StartsWith("BUILD_SERVER") && strFlag.Contains(":"))
                        {
                            strJumpstartCGI = strFlag.Substring(strFlag.IndexOf(":") + 1);
                            break;
                        }
                    }
                    if (strJumpstartCGI != "")
                    {
                        string strJumpstartURL = "http://" + strJumpstartCGI + "/cgi-bin/jumpstart.cgi?1010,MACADDR=" + strSolarisMAC + ",remove_client";
                        oLog.AddEvent(intID, strName, strSerial, "Beginning Client Removal on jumpstart URL = " + strJumpstartURL, LoggingType.Information);
                        if (RunCGI(strJumpstartURL) == true)
                        {
                            oLog.AddEvent(intID, strName, strSerial, "Success!! Client Removal on jumpstart URL = " + strJumpstartURL, LoggingType.Information);
                        }
                        else
                        {
                            strError = "REMOVE_CLIENT registration ~ Jumpstart URL = " + strJumpstartURL + " failed.";
                            oLog.AddEvent(intID, strName, strSerial, "Failure!! Client Removal on jumpstart URL = " + strJumpstartURL, LoggingType.Error);
                        }
                    }
                    else
                        strError = "Invalid Build Flags: " + dsSearch.Tables[0].Rows[0]["flags"].ToString();
                }
                else
                {
                    // Other
                }

                if (boolMDT)
                {
                    string strMACAddress1 = dsSearch.Tables[0].Rows[0]["macaddress"].ToString();
                    string strMACAddress2 = dsSearch.Tables[0].Rows[0]["macaddress2"].ToString();
                    string strRDPMDTWebService = dsSearch.Tables[0].Rows[0]["build_flags"].ToString();

                    System.Net.NetworkCredential oCredentials = new System.Net.NetworkCredential(oVariable.ADUser(), oVariable.ADPassword(), oVariable.Domain());
                    BuildSubmit oMDT = new BuildSubmit();
                    oMDT.Credentials = oCredentials;
                    oMDT.Url = strRDPMDTWebService;
                    oLog.AddEvent(intID, strName, strSerial, "Cleaning MDT on " + strRDPMDTWebService + " (" + strName + ", " + strMACAddress1 + ", " + "ServerShare" + ")", LoggingType.Information);
                    oMDT.Cleanup(strName, strMACAddress1, "ServerShare");
                    if (strMACAddress2 != "")
                    {
                        oLog.AddEvent(intID, strName, strSerial, "Waiting 60 seconds for Active Directory to Sychronize...", LoggingType.Debug);
                        Thread.Sleep(60000); // Wait 60 seconds
                        oLog.AddEvent(intID, strName, strSerial, "Cleaning MDT on " + strRDPMDTWebService + " (" + strName + ", " + strMACAddress2 + ", " + "ServerShare" + ")", LoggingType.Information);
                        oMDT.Cleanup(strName, strMACAddress2, "ServerShare");
                    }
                    oLog.AddEvent(intID, strName, strSerial, "MDT has been cleared", LoggingType.Information);
                }
            }
            catch (Exception ex)
            {
                strError = ex.Message;
            }

            if (strError != "")
            {
                panCleanup.Visible = true;
                litCleanup.Text = txtName.Text.ToUpper();
            }
            else
                AddError(strError, ulSearchError, panSearchError);
        }
    }
    protected void ClearMessages()
    {
        panError.Visible = false;
        panError2.Visible = false;
        panSearch.Visible = false;
        panSearchError.Visible = false;
        btnCleanup.Enabled = false;
    }
    protected void RefreshForm()
    {
        ClearMessages();
        int intOS = ParseInt(ddlOS.SelectedItem.Value);
        boolOSWindows = oOperatingSystem.IsWindows(intOS) || oOperatingSystem.IsWindows2008(intOS);
        boolOSLinux = oOperatingSystem.IsLinux(intOS);
        boolOSSolaris = oOperatingSystem.IsSolaris(intOS);
        boolOSAIX = oOperatingSystem.IsAix(intOS);
        boolOSOther = (intOS > 0 && boolOSWindows == false && boolOSLinux == false && boolOSSolaris == false && boolOSAIX == false);

        boolTypeVirtual = radVirtualYes.Checked;
        boolTypePhysical = radVirtualNo.Checked;

        btnRegister.Enabled = !(boolOSOther || (boolOSSolaris && boolTypeVirtual));
        Page.ClientScript.RegisterStartupScript(typeof(Page), "tabbing", "ChangeTab(2);", true);
    }





    private bool RunCGI(string _url)
    {
        bool boolReturn = false;
        HttpWebRequest oWebRequest1 = (HttpWebRequest)WebRequest.Create(_url);
        try
        {
            HttpWebResponse oWebResponse1 = (HttpWebResponse)oWebRequest1.GetResponse();
            oWebResponse1.Close();
            boolReturn = true;
        }
        catch { }
        return boolReturn;
    }
    protected bool AddError(TextBox _control, string _message, bool _error, ValidationRestriction _restrict, Validation _type, HtmlGenericControl ul, Panel _panel)
    {
        _control.CssClass = _control.CssClass.Replace(" invalid", "");
        bool error = false;

        string strValue = _control.Text.Trim();
        switch (_restrict)
        {
            case ValidationRestriction.Basic:
                if (_type == Validation.Required)
                    error = (strValue == "");
                break;
            case ValidationRestriction.Numeric:
                if (_type == Validation.Required && strValue == "")
                    error = true;
                else if (strValue != "" && ParseInt(strValue) < 0)
                    error = true;
                break;
            case ValidationRestriction.IPAddress:
                if (_type == Validation.Required && strValue == "")
                    error = true;
                else if (strValue != "")
                {
                    if (strValue.Contains(".") == false)
                        error = true;
                    else
                    {
                        string[] strValues = strValue.Split(new char[] { '.' });
                        if (strValues.Length != 4)
                            error = true;
                        else
                        {
                            int add1 = ParseInt(strValues[0]);
                            int add2 = ParseInt(strValues[1]);
                            int add3 = ParseInt(strValues[2]);
                            int add4 = ParseInt(strValues[3]);
                            if ((add1 < 1 || add1 > 255)
                                || (add2 < 0 || add2 > 255)
                                || (add3 < 0 || add3 > 255)
                                || (add4 < 0 || add4 > 255)
                                )
                                error = true;
                        }
                    }
                }
                break;
            case ValidationRestriction.MACAddress:
                if (_type == Validation.Required && strValue == "")
                    error = true;
                else if (strValue != "")
                {
                    if (strValue.Contains(":") == false)
                        error = true;
                    else
                    {
                        string[] strValues = strValue.Split(new char[] { ':' });
                        if (strValues.Length != 6)
                            error = true;
                        else
                        {
                            foreach (string _value in strValues)
                            {
                                if (_value.Length != 2)
                                {
                                    error = true;
                                    break;
                                }
                            }
                        }
                    }
                }
                break;
        }
        if (error == true)
        {
            _control.CssClass += " invalid";
            _error = AddError(_control, _message, ul, _panel);
        }
        else
            _control.Text = strValue;
        return _error;
    }
    protected bool AddError(TextBox _control, TextBox _control_if_not_empty, string _message, bool _error, ValidationRestriction _restrict, HtmlGenericControl ul, Panel _panel)
    {
        _control.CssClass = _control.CssClass.Replace(" invalid", "");
        if (_control_if_not_empty.Text.Trim() != "")
            return AddError(_control, _message, _error, _restrict, Validation.Required, ul, _panel);
        else
            return _error;
    }
    protected bool AddError(RadioButton[] _controls, string _message, bool _error, HtmlGenericControl ul, Panel _panel)
    {
        bool error = true;
        foreach (RadioButton _control in _controls)
        {
            if (_control.Checked)
            {
                error = false;
                break;
            }
        }
        if (error)
        {
            foreach (RadioButton _control in _controls)
                //Page.ClientScript.RegisterStartupScript(typeof(Page), "saved", "RadioError('" + _control.ClientID + "');");
                _control.Attributes.Add("class", " invalid");
            _error = AddError(_controls[0], _message, ul, _panel);
        }
        return _error;
    }
    protected bool AddError(DropDownList _control, string _message, bool _error, HtmlGenericControl ul, Panel _panel)
    {
        _control.CssClass = _control.CssClass.Replace(" invalid", "");
        if (_control.SelectedValue == "0")
        {
            _control.CssClass += " invalid";
            _error = AddError(_control, _message, ul, _panel);
        }
        return _error;
    }



    protected bool AddError(Control _control, string _message, HtmlGenericControl ul, Panel _panel)
    {
        return AddError("<a href=\"javascript:void(0);\" onclick=\"Focus('" + _control.ClientID + "');\">" + _message + "</a>", ul, _panel);
    }
    protected bool AddError(string _message, HtmlGenericControl ul, Panel _panel)
    {
        _panel.Visible = true;
        HtmlGenericControl li = new HtmlGenericControl("li");
        li.InnerHtml = _message;
        ul.Controls.Add(li);
        return false;
    }
    protected void LoadList(object _source, DropDownList _control, string _default)
    {
        LoadList(_source, _control, _default, "name", "id");
    }
    protected void LoadList(object _source, DropDownList _control, string _default, string _text, string _value)
    {
        _control.DataTextField = _text;
        _control.DataValueField = _value;
        _control.DataSource = _source;
        _control.DataBind();
        if (_default != "")
            _control.Items.Insert(0, new ListItem("-- " + _default + " --", "0"));
    }
    protected int ParseInt(string _value)
    {
        int _return = 0;
        if (Int32.TryParse(_value, out _return))
            return _return;
        else
            return -1;
    }
</script>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>OS Build Server Page</title>
    <link rel="stylesheet" type="text/css" href="/css/default.css" />
    <link rel="stylesheet" type="text/css" href="/css/build.css" />
    <link rel="stylesheet" type="text/css" href="/css/jquery-ui.css" />
    <script type="text/javascript" src="/javascript/jquery.min.js"></script>
    <script type="text/javascript" src="/javascript/jquery.ui.min.js"></script>
    <script type="text/javascript">
        jQuery(document).ready(function () {
            $('#<%=ddlOS.ClientID %>').change(function(e) {
                Wait();
            });
            $('#<%=radVirtualYes.ClientID %>:not(:checked)').click(function(e) {
                Wait();
            });
            $('#<%=radVirtualNo.ClientID %>:not(:checked)').click(function(e) {
                Wait();
            });
            $('#<%=txtSearch.ClientID %>').keydown(function (e) {
                if (e.keyCode && e.keyCode == '13') {
                    __doPostBack('<%=btnStatus.UniqueID%>', '');
                    return false;
                } else {
                    return true;
                }
            });
//            $('#<%=txtSearch.ClientID %>').keypress(function(e) {
//                alert(e.which);
//                if(e.which == 13) {
//                    $(this).blur();
//                    $('#<%=btnStatus.ClientID %>').focus().click();
//                }
//            });
            $( "#tabs" ).tabs();
            WaitHide();
        });
        function Wait(Button) {
            if (Button == null || Button.disabled == false) {
                setTimeout(function() { Wait2(Button); }, 100)
                return true;
            }
        }
        function Wait2(Button) {
            $("#ss_blackOutBox").show();
            $("#ss_blackOut").show();
            return true;
        }
        function WaitHide() {
            $("#ss_blackOutBox").hide();
            $("#ss_blackOut").hide();
        }
        function Focus(oObj) {
            //$('#' + oObj).focus().animate({border: '1px solid #779ccc', backgroundColor: '#f0f7ff'}, "slow").delay(500).animate({border: '1px solid #779ccc', backgroundColor: '#f0f7ff'}, "slow");
            if ($('#' + oObj).prop("type") == "radio") {   // radio
                $('#' + oObj).focus().parent().parent().find('input').each(function(index, value) {
                    $(this).parent().switchClass( " invalid", " invalidH", 500 ).delay(500).switchClass( " invalidH", " invalid", 500 );
                });
            }
            else
                $('#' + oObj).focus().switchClass( " invalid", " invalidH", 500 ).delay(500).switchClass( " invalidH", " invalid", 500 );
            return true;
        }
        function ChangeTab(_tab) {
            window.location = window.location + "#tabs-" + _tab;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div id="Wrapper">
            <div id="ss_blackOut"></div>
            <div id="ss_blackOutBox"></div>
            
            <div id="contentWrapper">
                <div id="tabs">
                    <ul>
                        <li><a href="#tabs-1">Status / Cleanup Existing Build</a></li>
                        <li><a href="#tabs-2">Register a New Build</a></li>
                    </ul>
                <div id="tabs-1">
                    <div class="staticWrapper">
                        <h1>
                            <p>OS Build Page</p>
                            <p><span>Version 1.0</span></p>
                            <div class="Buttons">
                                <asp:LinkButton ID="btnStatus" runat="server" Text="Search Build" OnClick="btnStatus_Click" OnClientClick="Wait(this);" />
                                <asp:LinkButton ID="btnReset2" runat="server" Text="Reset Form" OnClick="btnReset_Click" OnClientClick="Wait();" />
                                <asp:LinkButton ID="btnCleanup" runat="server" Text="Cleanup Build" OnClick="btnCleanup_Click" Enabled="false" OnClientClick="Wait(this);" />
                            </div>
                            <asp:Panel ID="panSearch" runat="server" Visible="false" CssClass="error">
                                <h4>The following fields need to be corrected:</h4>
                                <ul runat="server" id="ulSearch">
                                </ul>
                            </asp:Panel>
                            <asp:Panel ID="panCleanup" runat="server" Visible="false" CssClass="success">
                                <h1><p>Success</p></h1>
                                <h4>The server &quot;<asp:Literal ID="litCleanup" runat="server" />&quot; was successfully cleaned.</h4>
                            </asp:Panel>
                            <asp:Panel ID="panSearchError" runat="server" Visible="false" CssClass="error">
                                <h4>There was a problem processing this request:</h4>
                                <ul runat="server" id="ulSearchError">
                                </ul>
                            </asp:Panel>
                        </h1>
                    </div>
                    
                    
                    <p> <!-- Server name -->
                        <h4>
                            Enter the server name: 
                            <span>(Required)</span>
                        </h4>
                        <h5>
                            <asp:TextBox ID="txtSearch" runat="server" Width="200" MaxLength="30" />
                        </h5>
                    </p>
                    <asp:Panel ID="panInformation" runat="server">
                        <p>&nbsp;</p>
                        <table class="info">
                            <tr>
                                <td>Status:</td>
                                <td><asp:Label ID="lblStatus" runat="server" /></td>
                            </tr>
                            <tr>
                                <td>DHCP address:</td>
                                <td><asp:Label ID="lblDHCP" runat="server" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:LinkButton ID="btnDHCP" runat="server" Text="[Copy to Clipboard]" /></td>
                            </tr>
                            <tr>
                                <td>Registered:</td>
                                <td><asp:Label ID="lblRegistered" runat="server" /></td>
                            </tr>
                            <tr style='display:<%=lblName.Text == "" ? "none" : "inline" %>'>
                                <td>Name:</td>
                                <td><asp:Label ID="lblName" runat="server" /></td>
                            </tr>
                            <tr>
                                <td>Serial Number:</td>
                                <td><asp:Label ID="lblSerial" runat="server" /></td>
                            </tr>
                            <tr>
                                <td>Asset Tag:</td>
                                <td><asp:Label ID="lblAsset" runat="server" /></td>
                            </tr>
                            <tr>
                                <td>Array Config:</td>
                                <td><asp:Label ID="lblArrayConfig" runat="server" /></td>
                            </tr>
                            <tr>
                                <td>Operating System:</td>
                                <td><asp:Label ID="lblOS" runat="server" /></td>
                            </tr>
                            <tr>
                                <td>Operating System Version:</td>
                                <td><asp:Label ID="lblVersion" runat="server" /></td>
                            </tr>
                            <tr>
                                <td>Service Pack:</td>
                                <td><asp:Label ID="lblServicePack" runat="server" /></td>
                            </tr>
                            <tr>
                                <td>Build Type:</td>
                                <td><asp:Label ID="lblBuildType" runat="server" /></td>
                            </tr>
                            <tr>
                                <td>Domain:</td>
                                <td><asp:Label ID="lblDomain" runat="server" /></td>
                            </tr>
                            <tr>
                                <td>Source:</td>
                                <td><asp:Label ID="lblSource" runat="server" /></td>
                            </tr>
                            <tr>
                                <td>MAC address (primary):</td>
                                <td><asp:Label ID="lblMAC1" runat="server" /></td>
                            </tr>
                            <tr>
                                <td>MAC address (secondary):</td>
                                <td><asp:Label ID="lblMAC2" runat="server" /></td>
                            </tr>
                            <tr>
                                <td>IP address (server):</td>
                                <td><asp:Label ID="lblIP" runat="server" /></td>
                            </tr>
                            <tr>
                                <td>IP address (server) VLAN:</td>
                                <td><asp:Label ID="lblIPvlan" runat="server" /></td>
                            </tr>
                            <tr>
                                <td>IP address (server) Subnet Mask:</td>
                                <td><asp:Label ID="lblIPmask" runat="server" /></td>
                            </tr>
                            <tr>
                                <td>IP address (server) Gateway:</td>
                                <td><asp:Label ID="lblIPgateway" runat="server" /></td>
                            </tr>
                            <tr>
                                <td>IP address (backup):</td>
                                <td><asp:Label ID="lblBackupIP" runat="server" /></td>
                            </tr>
                            <tr>
                                <td>IP address (backup) VLAN:</td>
                                <td><asp:Label ID="lblBackupIPvlan" runat="server" /></td>
                            </tr>
                            <tr>
                                <td>IP address (backup) Subnet Mask:</td>
                                <td><asp:Label ID="lblBackupIPmask" runat="server" /></td>
                            </tr>
                            <tr>
                                <td>IP address (server) Gateway:</td>
                                <td><asp:Label ID="lblBackupIPgateway" runat="server" /></td>
                            </tr>
                            <tr>
                                <td>Interface # 1:</td>
                                <td><asp:Label ID="lblInterface1" runat="server" /></td>
                            </tr>
                            <tr>
                                <td>MP IP address # 1:</td>
                                <td><asp:Label ID="lblMPIP1" runat="server" /></td>
                            </tr>
                            <tr>
                                <td>Interface # 2:</td>
                                <td><asp:Label ID="lblInterface2" runat="server" /></td>
                            </tr>
                            <tr>
                                <td>MP IP address # 2:</td>
                                <td><asp:Label ID="lblMPIP2" runat="server" /></td>
                            </tr>
                            <tr>
                                <td>Build Flags:</td>
                                <td><asp:Label ID="lblBuildFlags" runat="server" /></td>
                            </tr>
                            <tr>
                                <td>Flags:</td>
                                <td><asp:Label ID="lblFlags" runat="server" /></td>
                            </tr>
                        </table>
                    </asp:Panel>
                </div>
                <div id="tabs-2">
                    <div class="staticWrapper">
                        <h1>
                            <p>OS Build Page</p>
                            <p><span>Version 1.0</span></p>
                            <div class="Buttons">
                                <asp:LinkButton ID="btnRegister" runat="server" Text="Register Build" OnClick="btnRegister_Click" OnClientClick="Wait(this);" />
                                <asp:LinkButton ID="btnReset" runat="server" Text="Reset Form" OnClick="btnReset_Click" OnClientClick="return confirm('WARNING: You will lose all of the information if you continue.\n\nAre you sure you want to reset the page?') && Wait();" />
                                <asp:LinkButton ID="btnPrint" runat="server" Text="Print Form" OnClientClick="window.print();return false;" />
                            </div>
                            <asp:Panel ID="panError" runat="server" Visible="false" CssClass="error">
                                <h4>The following fields need to be corrected:</h4>
                                <ul runat="server" id="ulErrors">
                                </ul>
                            </asp:Panel>
                            <asp:Panel ID="panSuccess" runat="server" Visible="false" CssClass="success">
                                <h1><p>Success</p></h1>
                                <h4>The server &quot;<asp:Literal ID="litName" runat="server" />&quot; was successfully registered and is ready to build.</h4>
                            </asp:Panel>
                            <asp:Panel ID="panError2" runat="server" Visible="false" CssClass="error">
                                <h4>There was a problem processing this request:</h4>
                                <ul runat="server" id="ulErrors2">
                                </ul>
                            </asp:Panel>
                        </h1>
                    </div>



                    <!-- *********************************** -->
                    <!-- **********   ALL   **************** -->
                    <!-- *********************************** -->
                    <p> <!-- Server name -->
                        <h4>
                            Enter the server name: 
                            <span>(Required)</span>
                        </h4>
                        <h5>
                            <asp:TextBox ID="txtName" runat="server" Width="200" MaxLength="30" />
                        </h5>
                    </p>
                    <p> <!-- Backup Software -->
                        <h4>
                            Select the backup software: 
                            <span>(Required)</span>
                        </h4>
                        <h5>
                            <asp:DropDownList ID="ddlBackup" runat="server">
                                <asp:ListItem Text="-- none --" Value="0" />
                                <asp:ListItem Value="Avamar" />
                                <asp:ListItem Value="TSM" />
                                <asp:ListItem Value="Legato" />
                            </asp:DropDownList>
                        </h5>
                    </p>
                    <p> <!-- IP Address (Server) -->
                        <h4>
                            Enter the server IP address: 
                        </h4>
                        <h5>
                            <asp:TextBox ID="txtIPServer" runat="server" Width="100" MaxLength="15" />
                            <span>(Format: 000.000.000.000)</span>
                        </h5>
                    </p>
                    <p> <!-- VLAN (Server) -->
                        <h4>
                            Enter the VLAN of the server IP address: 
                            <span>(Required if IP address specified)</span>
                        </h4>
                        <h5>
                            <asp:TextBox ID="txtIPServerVLAN" runat="server" Width="75" MaxLength="10" />
                            <span>(Numeric values ONLY)</span>
                        </h5>
                    </p>
                    <p> <!-- Subnet Mask (Server) -->
                        <h4>
                            Enter the subnet mask of the server IP address: 
                            <span>(Required if IP address specified)</span>
                        </h4>
                        <h5>
                            <asp:TextBox ID="txtIPServerMask" runat="server" Width="100" MaxLength="15" />
                            <span>(Format: 000.000.000.000)</span>
                        </h5>
                    </p>
                    <p> <!-- Gateway (Server) -->
                        <h4>
                            Enter the gateway of the server IP address: 
                            <span>(Required if IP address specified)</span>
                        </h4>
                        <h5>
                            <asp:TextBox ID="txtIPServerGateway" runat="server" Width="100" MaxLength="15" />
                            <span>(Format: 000.000.000.000)</span>
                        </h5>
                    </p>
                    <p> <!-- IP Address (Backup) -->
                        <h4>
                            Enter the backup IP address: 
                        </h4>
                        <h5>
                            <asp:TextBox ID="txtIPBackup" runat="server" Width="100" MaxLength="15" />
                            <span>(Format: 000.000.000.000)</span>
                        </h5>
                    </p>
                    <p> <!-- VLAN (Backup) -->
                        <h4>
                            Enter the VLAN of the backup IP address: 
                            <span>(Required if IP address specified)</span>
                        </h4>
                        <h5>
                            <asp:TextBox ID="txtIPBackupVLAN" runat="server" Width="75" MaxLength="10" />
                        </h5>
                    </p>
                    <p> <!-- Subnet Mask (Server) -->
                        <h4>
                            Enter the subnet mask of the server IP address: 
                        </h4>
                        <h5>
                            <asp:TextBox ID="txtIPBackupMask" runat="server" Width="100" MaxLength="15" />
                            <span>(Format: 000.000.000.000)</span>
                        </h5>
                    </p>
                    <p> <!-- Gateway (Backup) -->
                        <h4>
                            Enter the gateway of the backup IP address: 
                        </h4>
                        <h5>
                            <asp:TextBox ID="txtIPBackupGateway" runat="server" Width="100" MaxLength="15" />
                            <span>(Format: 000.000.000.000)</span>
                        </h5>
                    </p>
                    <p> <!-- Operating System -->
                        <h4>
                            Select the operating system: 
                            <span>(Required)</span>
                        </h4>
                        <h5>
                            <asp:DropDownList ID="ddlOS" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlOS_Change" />

                            <!-- *********************************** -->
                            <!-- ****** WINDOWS + LINUX ************ -->
                            <!-- *********************************** -->
                            <div style='display:<%=(boolOSWindows || boolOSLinux) ? "inline" : "none" %>'>
                                <p> <!-- Build location -->
                                    <h4>
                                        Select the build location: 
                                        <span>(Required)</span>
                                    </h4>
                                    <h5>
                                        <asp:DropDownList ID="ddlMDT" runat="server">
                                            <asp:ListItem Text="-- select --" Value="0" />
                                            <asp:ListItem Text="Cleveland" Value="http://clevelandbuild.pncbank.com/wabebuild/BuildSubmit.asmx" />
                                            <asp:ListItem Text="Dalton" Value="http://daltonbuild.pncbank.com/wabebuild/BuildSubmit.asmx" />
                                            <asp:ListItem Text="Summit" Value="http://summitbuild.pncbank.com/wabebuild/BuildSubmit.asmx" />
                                            <asp:ListItem Text="Firstside" Value="http://daltonbuild.pncbank.com/wabebuild/BuildSubmit.asmx" Enabled="false" />
                                        </asp:DropDownList>
                                    </h5>
                                </p>
                            </div>


                            <!-- *********************************** -->
                            <!-- ********** WINDOWS **************** -->
                            <!-- *********************************** -->
                            <div style='display:<%=boolOSWindows ? "inline" : "none" %>'>
                                <p> <!-- Domain -->
                                    <h4>
                                        Select the domain: 
                                        <span>(Required)</span>
                                    </h4>
                                    <h5>
                                        <asp:DropDownList ID="ddlDomainWindows" runat="server">
                                            <asp:ListItem Text="-- select --" Value="0" />
                                            <asp:ListItem Value="PNCNT" />
                                            <asp:ListItem Value="PNCADSTEST" />
                                        </asp:DropDownList>
                                    </h5>
                                </p>
                            </div>
                            <div style='display:<%=boolOSWindows && false ? "inline" : "none" %>'>
                                <p> <!-- Components -->
                                    <h4>
                                        Select components: 
                                        <span>(Select all that apply)</span>
                                    </h4>
                                    <h5>
                                        <asp:CheckBoxList ID="chkComponentsWindows" runat="server" RepeatColumns="1" RepeatDirection="Vertical" RepeatLayout="Table">
                                            <asp:ListItem Text="Internet Information Services (IIS)" Value="IIS" />
                                            <asp:ListItem Text="SQL 2008" Value="SQL" />
                                        </asp:CheckBoxList>
                                        <span><br />(This will not actually install anything)</span>
                                    </h5>
                                </p>
                            </div>
                            <div style='display:<%=boolOSWindows|| boolOSLinux ? "inline" : "none" %>'>
                                <p> <!-- Components -->
                                    <h4>
                                        Select components: 
                                        <span>(Select all that apply)</span>
                                    </h4>
                                    <h5>
                                        <asp:CheckBox ID="chkHIDS" runat="server" Text="HIDS" /><br />
                                        <asp:CheckBox ID="chkESM" runat="server" Text="ESM" /><br />
                                        <asp:CheckBox ID="chkIIS" runat="server" Text="Internet Information Services (IIS)" />
                                    </h5>
                                </p>
                            </div>


                            <!-- *********************************** -->
                            <!-- **********  LINUX  **************** -->
                            <!-- *********************************** -->
                            <div style='display:<%=boolOSLinux ? "inline" : "none" %>'>
                                <p> <!-- Domain -->
                                    <h4>
                                        Select the domain: 
                                        <span>(Required)</span>
                                    </h4>
                                    <h5>
                                        <asp:DropDownList ID="ddlDomainLinux" runat="server">
                                            <asp:ListItem Text="-- select --" Value="0" />
                                            <asp:ListItem Value="PNCNT" />
                                            <asp:ListItem Value="PNCADSTEST" />
                                            <asp:ListItem Value="PNCETECH" />
                                        </asp:DropDownList>
                                    </h5>
                                </p>
                            </div>
                            <div style='display:<%=boolOSLinux ? "inline" : "none" %>'>
                                <p> <!-- Components -->
                                    <h4>
                                        Select components: 
                                        <span>(Select all that apply)</span>
                                    </h4>
                                    <h5>
                                        <asp:CheckBoxList ID="chkComponentsLinux" runat="server" RepeatColumns="1" RepeatDirection="Vertical" RepeatLayout="Table">
                                            <asp:ListItem Text="Apache" Value="APACHE" />
                                            <asp:ListItem Text="Covalent Apache" Value="COVALENT_APACHE" />
                                            <asp:ListItem Text="DB2 Connect" Value="DB2CONNECT" />
                                            <asp:ListItem Text="Jboss 4.3" Value="JBOSS43" />
                                            <asp:ListItem Text="Jboss 5.1" Value="JBOSS51" />
                                            <asp:ListItem Text="JBoss Apache" Value="APACHE" />
                                            <asp:ListItem Text="JBoss SOA 5" Value="JBOSSSOA5" />
                                            <asp:ListItem Text="MySQL" Value="MYSQL" />
                                            <asp:ListItem Text="Oracle" Value="ORACLE" />
                                            <asp:ListItem Text="Weblogic 11g" Value="WEBLOGIC10" />
                                            <asp:ListItem Text="WebSphere 6.1" Value="WEBSPHERE61" />
                                            <asp:ListItem Text="WebSphere 7.0" Value="WEBSPHERE70" />
                                        </asp:CheckBoxList>
                                    </h5>
                                </p>
                            </div>
                            
                            
                            
                            
                            <div style='display:<%=boolOSWindows || boolOSLinux || boolOSSolaris ? "inline" : "none" %>'>
                                <p> <!-- Virtual? -->
                                    <h4>
                                        Is this a virtual machine?
                                        <span>(Required)</span>
                                    </h4>
                                    <h5>
                                        <asp:RadioButton ID="radVirtualYes" runat="server" Text="Yes" GroupName="Virtual" AutoPostBack="true" OnCheckedChanged="radVirtual_Change" />
                                        <asp:RadioButton ID="radVirtualNo" runat="server" Text="No" GroupName="Virtual" AutoPostBack="true" OnCheckedChanged="radVirtual_Change" />

                                        <div style='display:<%=boolTypeVirtual && boolOSSolaris ? "inline" : "none" %>'>
                                            <p> <!-- VMWARE -->
                                                <h6>
                                                    <img src="/images/alert.gif" alt="" align="absmiddle" /> Solaris virtual machines should not be registered using this page.  Contact your manager for more information.
                                                </h6>
                                            </p>
                                        </div>

                                        <div style='display:<%=boolTypeVirtual && (boolOSWindows || boolOSLinux) ? "inline" : "none" %>'>
                                            <p> <!-- MAC address  (Primary) -->
                                                <h4>
                                                    Enter the MAC address: 
                                                    <span>(Required)</span>
                                                </h4>
                                                <h5>
                                                    <asp:TextBox ID="txtMac" runat="server" Width="200" MaxLength="30" />
                                                    <span>(Format: A1:B2:C3:D4:E5:F6)</span>
                                                </h5>
                                            </p>
                                        </div>
                                        <div style='display:<%=boolTypePhysical ? "inline" : "none" %>'>
                                            <p> <!-- Serial number -->
                                                <h4>
                                                    Enter the serial number: 
                                                    <span>(Required)</span>
                                                </h4>
                                                <h5>
                                                    <asp:TextBox ID="txtSerial" runat="server" Width="200" MaxLength="30" />
                                                </h5>
                                            </p>
                                        </div>
                                        <div style='display:<%=boolTypePhysical ? "inline" : "none" %>'>
                                            <p> <!-- Asset Tag -->
                                                <h4>
                                                    Enter the asset tag: 
                                                </h4>
                                                <h5>
                                                    <asp:TextBox ID="txtAsset" runat="server" Width="200" MaxLength="30" />
                                                </h5>
                                            </p>
                                        </div>
                                        <div style='display:<%=boolTypePhysical ? "inline" : "none" %>'>
                                            <p> <!-- MAC address  (Primary) -->
                                                <h4>
                                                    Enter the MAC address (Primary): 
                                                    <span>(Required)</span>
                                                </h4>
                                                <h5>
                                                    <asp:TextBox ID="txtMacPrimary" runat="server" Width="200" MaxLength="30" />
                                                    <span>(Format: A1:B2:C3:D4:E5:F6)</span>
                                                </h5>
                                            </p>
                                        </div>
                                        <div style='display:<%=boolTypePhysical ? "inline" : "none" %>'>
                                            <p> <!-- MAC address  (Secondary) -->
                                                <h4>
                                                    Enter the MAC address (Secondary): 
                                                </h4>
                                                <h5>
                                                    <asp:TextBox ID="txtMacSecondary" runat="server" Width="200" MaxLength="30" />
                                                    <span>(Format: A1:B2:C3:D4:E5:F6)</span>
                                                </h5>
                                            </p>
                                        </div>







                                        <!-- *********************************** -->
                                        <!-- ********** SOLARIS **************** -->
                                        <!-- *********************************** -->
                                        <div style='display:<%=boolTypePhysical && boolOSSolaris ? "inline" : "none" %>'>
                                            <p> <!-- Build Type -->
                                                <h4>
                                                    Select the build type:
                                                    <span>(Required)</span>
                                                </h4>
                                                <h5>
                                                    <asp:DropDownList ID="ddlBuildType" runat="server">
                                                        <asp:ListItem Text="-- select --" Value="0" />
                                                        <asp:ListItem Value="sun4u" />
                                                        <asp:ListItem Value="sun4v" />
                                                    </asp:DropDownList>
                                                </h5>
                                            </p>
                                        </div>
                                        <div style='display:<%=boolTypePhysical && boolOSSolaris ? "inline" : "none" %>'>
                                            <p> <!-- Domain -->
                                                <h4>
                                                    Select the domain:
                                                    <span>(Required)</span>
                                                </h4>
                                                <h5>
                                                    <asp:DropDownList ID="ddlDomainSolaris" runat="server">
                                                        <asp:ListItem Text="-- select --" Value="0" />
                                                        <asp:ListItem Value="PNC_DR_HD" />
                                                        <asp:ListItem Value="PNC_PROD_HD" />
                                                        <asp:ListItem Value="PNC_QA_HD" />
                                                        <asp:ListItem Value="PNC_TEST_HD" />
                                                    </asp:DropDownList>
                                                </h5>
                                            </p>
                                        </div>
                                        <div style='display:<%= boolTypePhysical && boolOSSolaris ? "inline" : "none" %>'>
                                            <p> <!-- Ecom? -->
                                                <h4>
                                                    Is the server going into the E-commerce environment:
                                                </h4>
                                                <h5>
                                                    <asp:RadioButton ID="radEcomYes" runat="server" Text="Yes" GroupName="Ecom" ToolTip=",E" />
                                                    <asp:RadioButton ID="radEcomNo" runat="server" Text="No" GroupName="Ecom" ToolTip="" />
                                                </h5>
                                            </p>
                                        </div>
                                        <div style='display:<%=boolTypePhysical && boolOSSolaris ? "inline" : "none" %>'>
                                            <p> <!-- Source -->
                                                <h4>
                                                    Select the source:
                                                    <span>(Required)</span>
                                                </h4>
                                                <h5>
                                                    <asp:DropDownList ID="ddlSource" runat="server">
                                                        <asp:ListItem Text="-- select --" Value="0" />
                                                        <asp:ListItem Value="CLEVELAND" />
                                                        <asp:ListItem Value="DALTON" />
                                                        <asp:ListItem Value="PITTSBURGH" />
                                                    </asp:DropDownList>
                                                </h5>
                                            </p>
                                        </div>
                                        <div style='display:<%=boolTypePhysical && (boolOSSolaris || boolOSWindows) ? "inline" : "none" %>'>
                                            <p> <!-- Flags -->
                                                <h4>
                                                    Select the boot type:
                                                    <span>(Required)</span>
                                                </h4>
                                                <h5>
                                                    <asp:RadioButton ID="radBootSan" runat="server" Text="Boot From SAN" GroupName="BFS" ToolTip="P" />
                                                    <asp:RadioButton ID="radBootLocal" runat="server" Text="Boot Local" GroupName="BFS" ToolTip="P,T,D" />
                                                </h5>
                                            </p>
                                        </div>
                                        <div style='display:<%=boolTypePhysical && boolOSSolaris ? "inline" : "none" %>'>
                                            <p> <!-- Build Network -->
                                                <h4>
                                                    Select the build network:
                                                    <span>(Required)</span>
                                                </h4>
                                                <h5>
                                                    <asp:DropDownList ID="ddlBuildNetwork" runat="server">
                                                        <asp:ListItem Text="-- select --" Value="0" />
                                                        <asp:ListItem Value="10.24.113.0" />
                                                        <asp:ListItem Value="10.24.113.0" />
                                                        <asp:ListItem Value="10.24.176.0" />
                                                        <asp:ListItem Value="10.24.240.0" />
                                                        <asp:ListItem Value="10.24.48.0" />
                                                        <asp:ListItem Value="10.24.49.0" />
                                                        <asp:ListItem Value="10.48.54.0" />
                                                        <asp:ListItem Value="10.48.56.0" />
                                                        <asp:ListItem Value="10.48.57.0" />
                                                        <asp:ListItem Value="10.48.58.0" />
                                                        <asp:ListItem Value="10.48.59.0" />
                                                        <asp:ListItem Value="10.62.250.0" />
                                                        <asp:ListItem Value="10.62.251.0" />
                                                        <asp:ListItem Value="10.62.252.0" />
                                                        <asp:ListItem Value="10.62.253.0" />
                                                    </asp:DropDownList>
                                                </h5>
                                            </p>
                                        </div>
                                        <div style='display:<%=boolTypePhysical && boolOSSolaris ? "inline" : "none" %>'>
                                            <p> <!-- CGI Script location -->
                                                <h4>
                                                    Select the CGI script location:
                                                    <span>(Required)</span>
                                                </h4>
                                                <h5>
                                                    <asp:DropDownList ID="ddlCGI" runat="server">
                                                        <asp:ListItem Text="-- select --" Value="0" />
                                                        <asp:ListItem Value="10.22.84.140:80" />
                                                        <asp:ListItem Value="10.24.48.12:80" />
                                                        <asp:ListItem Value="10.24.61.254:80" />
                                                        <asp:ListItem Value="10.48.59.251:80" />
                                                        <asp:ListItem Value="10.62.253.250:80" />
                                                    </asp:DropDownList>
                                                </h5>
                                            </p>
                                        </div>
                                        <div style='display:<%=boolTypePhysical && boolOSSolaris ? "inline" : "none" %>'>
                                            <p> <!-- Interface 1 -->
                                                <h4>
                                                    Select interface # 1:
                                                    <span>(Required)</span>
                                                </h4>
                                                <h5>
                                                    <asp:DropDownList ID="ddlInterface1" runat="server">
                                                        <asp:ListItem Text="-- select --" Value="0" />
                                                        <asp:ListItem Value="bge0" />
                                                        <asp:ListItem Value="bge1" />
                                                        <asp:ListItem Value="e1000g0" />
                                                        <asp:ListItem Value="e1000g1" />
                                                        <asp:ListItem Value="e1000g2" />
                                                        <asp:ListItem Value="e1000g3" />
                                                        <asp:ListItem Value="e1000g4" />
                                                        <asp:ListItem Value="e1000g5" />
                                                        <asp:ListItem Value="igb0" />
                                                        <asp:ListItem Value="igb1" />
                                                        <asp:ListItem Value="ixgbe1" />
                                                        <asp:ListItem Value="ixgbe2" />
                                                        <asp:ListItem Value="ixgbe3" />
                                                        <asp:ListItem Value="ixgbe4" />
                                                        <asp:ListItem Value="nxge0" />
                                                        <asp:ListItem Value="nxge4" />
                                                        <asp:ListItem Value="nxge6" />
                                                    </asp:DropDownList>
                                                </h5>
                                            </p>
                                        </div>
                                        <div style='display:<%=boolTypePhysical && boolOSSolaris ? "inline" : "none" %>'>
                                            <p> <!-- MPIP Address #1 -->
                                                <h4>
                                                    Enter MP IP address # 1: 
                                                    <span>(Required)</span>
                                                </h4>
                                                <h5>
                                                    <asp:TextBox ID="txtMPIP1" runat="server" Width="100" MaxLength="15" />
                                                    <span>(Format: 000.000.000.000)</span>
                                                </h5>
                                            </p>
                                        </div>
                                        <div style='display:<%=boolTypePhysical && boolOSSolaris ? "inline" : "none" %>'>
                                            <p> <!-- VLAN (MPIP Address #1) -->
                                                <h4>
                                                    Enter the VLAN of MP IP address # 1: 
                                                    <span>(Required if MP IP address # 1 specified)</span>
                                                </h4>
                                                <h5>
                                                    <asp:TextBox ID="txtMPIP1VLAN" runat="server" Width="75" MaxLength="10" />
                                                </h5>
                                            </p>
                                        </div>
                                        <div style='display:<%=boolTypePhysical && boolOSSolaris ? "inline" : "none" %>'>
                                            <p> <!-- Interface 2 -->
                                                <h4>
                                                    Select interface # 2:
                                                </h4>
                                                <h5>
                                                    <asp:DropDownList ID="ddlInterface2" runat="server">
                                                        <asp:ListItem Text="-- select --" Value="0" />
                                                        <asp:ListItem Value="bge0" />
                                                        <asp:ListItem Value="bge1" />
                                                        <asp:ListItem Value="e1000g0" />
                                                        <asp:ListItem Value="e1000g1" />
                                                        <asp:ListItem Value="e1000g2" />
                                                        <asp:ListItem Value="e1000g3" />
                                                        <asp:ListItem Value="e1000g4" />
                                                        <asp:ListItem Value="e1000g5" />
                                                        <asp:ListItem Value="igb0" />
                                                        <asp:ListItem Value="igb1" />
                                                        <asp:ListItem Value="ixgbe1" />
                                                        <asp:ListItem Value="ixgbe2" />
                                                        <asp:ListItem Value="ixgbe3" />
                                                        <asp:ListItem Value="ixgbe4" />
                                                        <asp:ListItem Value="nxge0" />
                                                        <asp:ListItem Value="nxge4" />
                                                        <asp:ListItem Value="nxge6" />
                                                    </asp:DropDownList>
                                                </h5>
                                            </p>
                                        </div>
                                        <div style='display:<%=boolTypePhysical && boolOSSolaris ? "inline" : "none" %>'>
                                            <p> <!-- MPIP Address #2 -->
                                                <h4>
                                                    Enter MP IP address # 2: 
                                                </h4>
                                                <h5>
                                                    <asp:TextBox ID="txtMPIP2" runat="server" Width="100" MaxLength="15" />
                                                    <span>(Format: 000.000.000.000)</span>
                                                </h5>
                                            </p>
                                        </div>
                                        <div style='display:<%=boolTypePhysical && boolOSSolaris ? "inline" : "none" %>'>
                                            <p> <!-- VLAN (MPIP Address #2) -->
                                                <h4>
                                                    Enter the VLAN of MP IP address # 2: 
                                                    <span>(Required if MP IP address # 2 specified)</span>
                                                </h4>
                                                <h5>
                                                    <asp:TextBox ID="txtMPIP2VLAN" runat="server" Width="75" MaxLength="10" />
                                                </h5>
                                            </p>
                                        </div>
                                    </h5>
                                </p>
                            </div>


                            
                            
                        </h5>
                    </p>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
