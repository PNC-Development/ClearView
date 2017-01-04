<%@ Page Language="C#" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" CodeBehind="Build.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.DEV.Build" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
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
