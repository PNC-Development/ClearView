using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using NCC.ClearView.Application.Core;
using System.Collections.Generic;
using System.Net;
using NCC.ClearView.Application.Core.w08r2;
using System.Threading;

namespace NCC.ClearView.Presentation.Web.DEV
{
    public partial class Build : System.Web.UI.Page
    {
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
        //#region ClassDeclarations
        //protected class cBuildLocation
        //{
        //    private string name;
        //    public string Name
        //    {
        //        get
        //        {
        //            return name;
        //        }
        //        set
        //        {
        //            name = value;
        //        }
        //    }
        //    private string key;
        //    public string Key
        //    {
        //        get
        //        {
        //            return key;
        //        }
        //        set
        //        {
        //            key = value;
        //        }
        //    }
        //    private string ws;
        //    public string WS
        //    {
        //        get
        //        {
        //            return ws;
        //        }
        //        set
        //        {
        //            ws = value;
        //        }
        //    }
        //    private string source;
        //    public string Source
        //    {
        //        get
        //        {
        //            return source;
        //        }
        //        set
        //        {
        //            source = value;
        //        }
        //    }

        //    public cBuildLocation(string _name, string _key, string _ws, string _source)
        //    {
        //        Name = _name;
        //        Key = _key;
        //        WS = _ws;
        //        Source = _source;
        //    }
        //}
        //#endregion

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
        //protected List<cBuildLocation> cBuildLocations;
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
                string strIPMask = txtIPServerMask.Text.Trim();
                string strIPGateway = txtIPServerGateway.Text.Trim();
                string strIPBackup = txtIPBackup.Text.Trim();
                string strIPBackupVlan = txtIPBackupVLAN.Text.Trim();
                string strIPBackupMask = txtIPBackupMask.Text.Trim();
                string strIPBackupGateway = txtIPBackupGateway.Text.Trim();

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
                        strError = oZeus.AddBuild(intID, intID, intID, strSerial, strAsset, strName, strArrayConfig, oOperatingSystem.Get(intOS, "zeus_os"), oOperatingSystem.Get(intOS, "zeus_os_version"), 0, oOperatingSystem.Get(intOS, "zeus_build_type"), ddlDomainWindows.SelectedItem.Value, intEnvironment, "SERVER", 0, strMAC1, strMAC2, strIP, strIPVlan, strIPMask, strIPGateway, strIPBackup, strIPBackupVlan, strIPBackupMask, strIPBackupGateway, "", "", "", "", ddlMDT.SelectedItem.Value, "", 1);
                        if (strError == "")
                        {
                            // Add Applications
                            foreach (ListItem li in chkComponentsWindows.Items)
                            {
                                if (li.Selected)
                                    oZeus.AddApp(strSerial, li.Value, "");
                            }
                            boolMDT = true;
                        }
                    }
                    else if (boolOSLinux)
                    {
                        strError = oZeus.AddBuild(intID, intID, intID, strSerial, strAsset, strName, strArrayConfig, oOperatingSystem.Get(intOS, "name"), "", 0, "", ddlDomainLinux.SelectedItem.Value, intEnvironment, "SERVER", 0, strMAC1, strMAC2, strIP, strIPVlan, strIPMask, strIPGateway, strIPBackup, strIPBackupVlan, strIPBackupMask, strIPBackupGateway, "", "", "", "", ddlMDT.SelectedItem.Value, "", 1);
                        if (strError == "")
                        {
                            // Add Applications
                            foreach (ListItem li in chkComponentsLinux.Items)
                            {
                                if (li.Selected)
                                    oZeus.AddApp(strSerial, li.Value, "");
                            }
                            boolMDT = true;
                        }
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

                        strError = oZeus.AddBuild(intID, intID, intID, strSerial, strAsset, strName.ToLower(), strArrayConfig, oOperatingSystem.Get(intOS, "zeus_os"), oOperatingSystem.Get(intOS, "zeus_os_version"), 0, strZeusBuildTypeSolaris, strSolarisDomain, intEnvironment, strSource, 0, strSolarisMAC, "", strIP, strIPVlan, strIPMask, strIPGateway, strIPBackup, strIPBackupVlan, strIPBackupMask, strIPBackupGateway, ddlInterface1.SelectedItem.Value, txtMPIP1.Text, ddlInterface2.SelectedItem.Value, txtMPIP2.Text, strSolarisBuildFlags, oVariable.SolarisFlags(strJumpstartCGI, strSolarisBuildNetwork, strJumpstartBuildType, false), 1);

                        if (strError == "")
                        {
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


                if (strError == "")
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
                lblIPmask.Text = drSearch["ipaddress_subnet"].ToString();
                lblIPgateway.Text = drSearch["ipaddress_gateway"].ToString();
                lblBackupIP.Text = drSearch["ipbackup"].ToString();
                lblBackupIPvlan.Text = drSearch["ipbackup_vlan"].ToString();
                lblBackupIPmask.Text = drSearch["ipbackup_subnet"].ToString();
                lblBackupIPgateway.Text = drSearch["ipbackup_gateway"].ToString();
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
                        string[] strFlags = dsSearch.Tables[0].Rows[0]["flags"].ToString().Split(new char[] {','});
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
    }
}
