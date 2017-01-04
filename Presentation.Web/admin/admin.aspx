<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="admin.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.admin" %>

<script type="text/javascript">
</script>
<html>
<head>
<title>Web Content Management Administration</title>
<link rel="stylesheet" type="text/css" href="/css/default.css" />
<script type="text/javascript" src="/javascript/global.js"></script>
<script type="text/javascript" src="/javascript/ajax.js"></script>
<script type="text/javascript" src="/javascript/default.js"></script>
<script type="text/javascript">
    function ValidateNexus(oName, oInterface, oSearch, oAccess, oTrunk, oShutdown, oVLANs) {
        if (ValidateText(oName, "Please enter a switch name / ip address") == false)
            return false;
        if (ValidateText(oInterface, "Please enter an interface") == false)
            return false;
        oSearch = document.getElementById(oSearch);
        oAccess = document.getElementById(oAccess);
        oTrunk = document.getElementById(oTrunk);
        oShutdown = document.getElementById(oShutdown);
        if (oSearch.checked == false && oAccess.checked == false && oTrunk.checked == false && oShutdown.checked == false)
        {
            alert('Please select a mode');
            return false;
        }
        else 
        {
            //var oVLAN = document.getElementById(oVLANs);
            if (oAccess.checked)
            {
                if (ValidateNumber0(oVLANs, "Please enter a valid VLAN") == false)
                    return false;
            }
            else if (oTrunk.checked)
            {
                if (ValidateText(oVLANs, "Please enter a VLAN range") == false)
                    return false;
            }
        }
        return true;
    }
</script>
</head>
<body topmargin="0" leftmargin="0">
<form id="Form1" runat="server">
        <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
        <td height="100%" valign="top">
            <table width="98%" cellpadding="3" cellspacing="2" border="0" align="center">
		        <tr><td colspan="2"><a name="bluecat">&nbsp;</a></td></tr>
		        <tr> 
		            <td colspan="2"><b>BlueCat DNS</b>&nbsp;&nbsp;(Domain = pncbank.com)</td>
		        </tr>
		        <tr>
		            <td colspan="2"><img src="/images/bigAlert.gif" border="0" align="absmiddle" /><b>WARNING:</b> Make sure you know what you are doing before you click anything...this will modify production BlueCat DNS settings!</td>
		        </tr>
		        <tr>
		            <td nowrap>IP Address:</td>
		            <td width="100%"><asp:TextBox ID="txtBlueCatIP" runat="server" CssClass="default" Width="300" /></td>
		        </tr>
		        <tr>
		            <td nowrap>Name:</td>
		            <td width="100%"><asp:TextBox ID="txtBlueCatName" runat="server" CssClass="default" Width="300" /></td>
		        </tr>
		        <tr>
		            <td nowrap>Description:</td>
		            <td width="100%"><asp:TextBox ID="txtBlueCatDescription" runat="server" CssClass="default" Width="300" /></td>
		        </tr>
		        <tr>
		            <td nowrap></td>
		            <td width="100%"><asp:CheckBox ID="chkBlueCatDelete" runat="server" CssClass="default" Text="Delete Staging?" /></td>
		        </tr>
		        <tr>
		            <td colspan="2">
		                <asp:Button ID="btnBlueCatDNSCreate" runat="server" CssClass="default" Width="75" OnClick="btnBlueCatDNSCreate_Click" Text="Create" />&nbsp;&nbsp;
		                <asp:Button ID="btnBlueCatDNSUpdate" runat="server" CssClass="default" Width="75" OnClick="btnBlueCatDNSUpdate_Click" Text="Update" />&nbsp;&nbsp;
		                <asp:Button ID="btnBlueCatDNSDelete" runat="server" CssClass="default" Width="75" OnClick="btnBlueCatDNSDelete_Click" Text="Delete" />&nbsp;&nbsp;
		                <asp:Button ID="btnBlueCatDNSSearch" runat="server" CssClass="default" Width="75" OnClick="btnBlueCatDNSSearch_Click" Text="Search" />&nbsp;&nbsp;
		            </td>
		        </tr>
		        <tr>
		            <td colspan="2"><asp:Label ID="lblBlueCat" runat="server" CssClass="default" /></td>
		        </tr>
		        <tr><td colspan="2"><a name="qip">&nbsp;</a></td></tr>
		        <tr> 
		            <td colspan="2"><b>QIP DNS</b>&nbsp;&nbsp;(Only for Object Types = Server, Domain = pncbank.com)</td>
		        </tr>
		        <tr>
		            <td colspan="2"><img src="/images/bigAlert.gif" border="0" align="absmiddle" /><b>WARNING:</b> Make sure you know what you are doing before you click anything...this will modify production QIP DNS settings!</td>
		        </tr>
		        <tr>
		            <td nowrap>IP Address:</td>
		            <td width="100%"><asp:TextBox ID="txtQIPDNSIP" runat="server" CssClass="default" Width="300" /></td>
		        </tr>
		        <tr>
		            <td nowrap>Name:</td>
		            <td width="100%"><asp:TextBox ID="txtQIPDNSName" runat="server" CssClass="default" Width="300" /></td>
		        </tr>
                <tr>
                    <td nowrap>Is Alias:</td>
                    <td width="100%"><asp:CheckBox ID="chkQIPDNSAlias" runat="server" CssClass="default" /></td>
                </tr>
		        <tr>
		            <td colspan="2">
		                <asp:Button ID="btnQIPDNSCreate" runat="server" CssClass="default" Width="75" OnClick="btnQIPDNSCreate_Click" Text="Create" />&nbsp;&nbsp;
		                <asp:Button ID="btnQIPDNSUpdate" runat="server" CssClass="default" Width="75" OnClick="btnQIPDNSUpdate_Click" Text="Update" />&nbsp;&nbsp;
		                <asp:Button ID="btnQIPDNSDelete" runat="server" CssClass="default" Width="75" OnClick="btnQIPDNSDelete_Click" Text="Delete" />&nbsp;&nbsp;
		                <asp:Button ID="btnQIPDNSSearch" runat="server" CssClass="default" Width="75" OnClick="btnQIPDNSSearch_Click" Text="Search" />&nbsp;&nbsp;
		            </td>
		        </tr>
		        <tr>
		            <td colspan="2"><asp:Label ID="lblQIPDNSResult" runat="server" CssClass="default" /></td>
		        </tr>
		        <tr><td colspan="2"><a name="dns_import">&nbsp;</a></td></tr>
		        <tr> 
		            <td colspan="2"><b>Import DNS</b></td>
		        </tr>
                <tr>
                    <td><a href="/admin/imports/templates/dns.xls" target="_blank">File:</a></td>
                    <td><asp:FileUpload ID="filImportDNS" runat="server" Width="450" CssClass="default" /></td>
                </tr>
		        <tr>
		            <td colspan="2"><asp:Button ID="btnImportDNS" runat="server" CssClass="default" Width="75" OnClick="btnImportDNS_Click" Text="Go" /></td>
		        </tr>
		        <tr>
		            <td colspan="2"><asp:Label ID="lblImportDNS" runat="server" CssClass="default" /></td>
		        </tr>
		        <tr><td colspan="2"><a name="nexus">&nbsp;</a></td></tr>
		        <tr> 
		            <td colspan="2"><b>Nexus Switch Modification</b></td>
		        </tr>
		        <tr>
		            <td colspan="2"><img src="/images/bigAlert.gif" border="0" align="absmiddle" /><b>WARNING:</b> Make sure you know what you are doing before you click anything...this will modify production SWITCH settings!</td>
		        </tr>
		        <tr>
		            <td nowrap>Switch:</td>
		            <td width="100%"><asp:TextBox ID="txtNexusSwitch" runat="server" CssClass="default" Width="200" MaxLength="50" /> [Ex: P-YB19-ZH06A-2]</td>
		        </tr>
		        <tr>
		            <td nowrap>Interface:</td>
		            <td width="100%"><asp:TextBox ID="txtNexusInterface" runat="server" CssClass="default" Width="100" MaxLength="20" /> [Ex: 102/1/32]</td>
		        </tr>
                <tr>
                    <td nowrap>Mode:</td>
                    <td width="100%">
                        <asp:RadioButton ID="radNexusSearch" runat="server" CssClass="default" Text="Search" GroupName="NexusMode" />
                        <asp:RadioButton ID="radNexusAccess" runat="server" CssClass="default" Text="Access" GroupName="NexusMode" />
                        <asp:RadioButton ID="radNexusTrunk" runat="server" CssClass="default" Text="Trunk" GroupName="NexusMode" />
                        <asp:RadioButton ID="radNexusShutdown" runat="server" CssClass="default" Text="Shutdown" GroupName="NexusMode" />
                    </td>
                </tr>
		        <tr>
                    <td nowrap></td>
		            <td width="100%"><asp:CheckBox ID="chkNexus" runat="server" CssClass="default" Text="Ignore Connected Warning" /></td>
		        </tr>
		        <tr>
                    <td nowrap>VLAN(s):</td>
		            <td width="100%"><asp:TextBox ID="txtNexusVLANs" runat="server" CssClass="default" Width="300" /> [Separate by comma, no spaces]</td>
		        </tr>
		        <tr>
                    <td nowrap>Native VLAN:</td>
		            <td width="100%"><asp:TextBox ID="txtNexusNative" runat="server" CssClass="default" Width="300" /> [Set to 0 to remove from config. Leave blank to skip]</td>
		        </tr>
		        <tr>
		            <td nowrap>Description:</td>
		            <td width="100%"><asp:TextBox ID="txtNexusDescription" runat="server" CssClass="default" Width="300" MaxLength="50" /></td>
		        </tr>
		        <tr>
		            <td colspan="2"><asp:Button ID="btnNexus" runat="server" CssClass="default" Width="75" OnClick="btnNexus_Click" Text="Change" /></td>
		        </tr>
		        <tr>
		            <td colspan="2"><asp:TextBox ID="txtNexus" runat="server" CssClass="default" Width="500" Rows="10" TextMode="MultiLine" Visible="false" /></td>
		        </tr>
		        <tr><td colspan="2">&nbsp;</td></tr>
		        <tr> 
		            <td colspan="2"><b>Decrypt</b></td>
		        </tr>
		        <tr>
		            <td nowrap>Value:</td>
		            <td width="100%"><asp:TextBox ID="txtDecrypt1" runat="server" CssClass="default" Width="300" MaxLength="100" /></td>
		        </tr>
		        <tr>
		            <td nowrap>passPhrase:</td>
		            <td width="100%"><asp:TextBox ID="txtDecrypt2" runat="server" CssClass="default" Width="100" MaxLength="20" /></td>
		        </tr>
		        <tr>
		            <td colspan="2"><asp:Button ID="btnDecrypt" runat="server" CssClass="default" Width="75" OnClick="btnDecrypt_Click" Text="Go" /></td>
		        </tr>
		        <tr>
		            <td colspan="2"><asp:Label ID="lblDecrypt" runat="server" CssClass="default" /></td>
		        </tr>
		        <tr><td colspan="2">&nbsp;</td></tr>
		        <tr> 
		            <td colspan="2"><b>Decrypt Querystring</b></td>
		        </tr>
		        <tr>
		            <td nowrap>Value:</td>
		            <td width="100%"><asp:TextBox ID="txtDecryptQ" runat="server" CssClass="default" Width="300" MaxLength="100" /></td>
		        </tr>
		        <tr>
		            <td colspan="2"><asp:Button ID="btnDecryptQ" runat="server" CssClass="default" Width="75" OnClick="btnDecryptQ_Click" Text="Go" /></td>
		        </tr>
		        <tr>
		            <td colspan="2"><asp:Label ID="lblDecryptQ" runat="server" CssClass="default" /></td>
		        </tr>
		        <tr><td colspan="2"><a name="import_project">&nbsp;</a></td></tr>
		        <tr> 
		            <td colspan="2"><b>Clarity Project Upload</b></td>
		        </tr>
                <tr>
                    <td>
                        <!--
                        <a href="/admin/imports/templates/projects.xls" target="_blank">File:</a>
                        -->
                        File:
                    </td>
                    <td><asp:FileUpload ID="filImportProject" runat="server" Width="450" CssClass="default" /></td>
                </tr>
		        <tr>
		            <td colspan="2"><img src="/images/bigAlert.gif" border="0" align="absmiddle" /><b>WARNING:</b> This file needs to be in the folling format...</td>
		        </tr>
		        <tr>
		            <td colspan="2">
		                <ul>
		                    <li> - First sheet is called &quot;Project List1&quot;</li>
		                    <li> - Column # 1 is &quot;ID&quot; (used to search for project)</li>
		                    <li> - Column # 2 is &quot;Project&quot; (will override the existing project name)</li>
		                    <li> - Column # 3 is &quot;Project Manager&quot; (will override the existing project manager)</li>
		                    <li> - Column # 4 is &quot;Created Date&quot; (will override the created date of the project)</li>
		                    <li> - Column # 5 is &quot;Approved Flag&quot; (only Approved records will be imported)</li>
		                    <li> - Column # 6 is &quot;Work Status&quot;</li>
		                </ul>
		                <p>Work Approved Flag = Yes, No</p>
		                <p>Work Status = Active (Requested), Cancelled, Complete, On Hold</p>
		            </td>
		        </tr>
		        <tr>
		            <td colspan="2"><asp:Button ID="btnProjectImport" runat="server" CssClass="default" Width="75" OnClick="btnProjectImport_Click" Text="Import" /> <asp:Button ID="btnProjectExport" runat="server" CssClass="default" Width="75" OnClick="btnProjectExport_Click" Text="Export" /></td>
		        </tr>
		        <tr>
		            <td colspan="2"><asp:Label ID="lblImportProject" runat="server" CssClass="default" /></td>
		        </tr>
		        <tr><td colspan="2"><a name="import_cost">&nbsp;</a></td></tr>
		        <tr> 
		            <td colspan="2"><b>Import Cost Centers</b></td>
		        </tr>
                <tr>
                    <td>File:</td>
                    <td><asp:FileUpload ID="filImportCostCenter" runat="server" Width="450" CssClass="default" /></td>
                </tr>
		        <tr>
		            <td colspan="2"><img src="/images/bigAlert.gif" border="0" align="absmiddle" /><b>WARNING:</b> This file needs to be in the folling format...</td>
		        </tr>
		        <tr>
		            <td colspan="2">
		                <ul>
		                    <li> - Delimited with the tilde character (&quot;~&quote;)</li>
		                    <li> - Column # 1 is &quot;PeopleSoftStatus&quot; (Not Used)</li>
		                    <li> - Column # 2 is &quot;GLCompCostCenter&quot;</li>
		                    <li> - Column # 3 is &quot;LOB&quot; (Not Used)</li>
		                    <li> - Column # 4 is &quot;BusinessSegment&quot; (Not Used)</li>
		                    <li> - Column # 5 is &quot;CostCenterDescription&quot; (Not Used)</li>
		                    <li> - Column # 6 is &quot;Lvl1&quot; (Not Used)</li>
		                    <li> - Column # 7 is &quot;Lvl2&quot; (Not Used)</li>
		                    <li> - Column # 8 is &quot;Lvl3&quot; (Not Used)</li>
		                    <li> - Column # 9 is &quot;Lvl4&quot; (Not Used)</li>
		                    <li> - Column # 10 is &quot;Lvl5&quot; (Not Used)</li>
		                    <li> - Column # 11 is &quot;Lvl6&quot; (Not Used)</li>
		                    <li> - Column # 12 is &quot;Lvl7&quot; (Not Used)</li>
		                    <li> - Column # 13 is &quot;Lvl8&quot; (Not Used)</li>
		                    <li> - Column # 14 is &quot;CompanyCode&quot;</li>
		                    <li> - Column # 15 is &quot;CompanyCostCenter&quot;</li>
		                    <li> - The rest of the columns are not used....</li>
		                </ul>
		            </td>
		        </tr>
		        <tr>
		            <td colspan="2"><asp:Button ID="btnImportCostCenter" runat="server" CssClass="default" Width="75" OnClick="btnImportCostCenter_Click" Text="Import" /></td>
		        </tr>
		        <tr>
		            <td colspan="2"><asp:Label ID="lblImportCostCenter" runat="server" CssClass="default" /></td>
		        </tr>
		        <tr><td colspan="2"><a name="associate_assets">&nbsp;</a></td></tr>
		        <tr> 
		            <td colspan="2"><b>Associate Assets / Names (for up to 20 servers)</b></td>
		        </tr>
		        <tr>
		            <td nowrap>Design ID:</td>
		            <td width="100%"><asp:TextBox ID="txtAssociateDesign" runat="server" CssClass="default" Width="100" MaxLength="10" /></td>
		        </tr>
		        <tr>
		            <td colspan="2"><asp:Button ID="btnAssociateDesign" runat="server" CssClass="default" Width="75" OnClick="btnAssociateDesign_Click" Text="Lookup" /></td>
		        </tr>
                <tr>
                    <td colspan="2"><asp:Label ID="lblAssociateDesignResult" runat="server" /></td>
                </tr>
		        <tr id="panAssociateDesign" runat="server" visible="false">
		            <td colspan="2">
		                <table cellpadding="5" cellspacing="2" border="0">
		                    <tr>
		                        <td>
                                    <table cellpadding="5" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                                        <tr bgcolor="#EEEEEE">
                                            <td><b>#</b></td>
                                            <td><b>Serial # of <asp:Label ID="lblAssociateDesignClass" runat="server" /> Asset:</b></td>
                                            <td><b><asp:Label ID="lblAssociateDesignDR" runat="server" /></b></td>
                                            <td><b>Server Name:</b></td>
                                            <td></td>
                                        </tr>
                                        <tr style='display:<%= (intAssociateDesignDevice <= intAssociateDesignDevices ? "inline" : "none")%>'>
                                            <td>Server # <%=intAssociateDesignDevice++ %></td>
                                            <td><asp:TextBox ID="txtAssociateDesignProd1" runat="server" Width="150" MaxLength="50" /></td>
                                            <td><asp:TextBox ID="txtAssociateDesignDR1" runat="server" Width="150" MaxLength="50" /></td>
                                            <td><asp:TextBox ID="txtAssociateDesignName1" runat="server" Width="150" MaxLength="50" /></td>
                                            <td><asp:Label ID="lblAssociateDesign1" runat="server" /></td>
                                        </tr>
                                        <tr style='display:<%= (intAssociateDesignDevice <= intAssociateDesignDevices ? "inline" : "none")%>'>
                                            <td>Server # <%=intAssociateDesignDevice++ %></td>
                                            <td><asp:TextBox ID="txtAssociateDesignProd2" runat="server" Width="150" MaxLength="50" /></td>
                                            <td><asp:TextBox ID="txtAssociateDesignDR2" runat="server" Width="150" MaxLength="50" /></td>
                                            <td><asp:TextBox ID="txtAssociateDesignName2" runat="server" Width="150" MaxLength="50" /></td>
                                            <td><asp:Label ID="lblAssociateDesign2" runat="server" /></td>
                                        </tr>
                                        <tr style='display:<%= (intAssociateDesignDevice <= intAssociateDesignDevices ? "inline" : "none")%>'>
                                            <td>Server # <%=intAssociateDesignDevice++ %></td>
                                            <td><asp:TextBox ID="txtAssociateDesignProd3" runat="server" Width="150" MaxLength="50" /></td>
                                            <td><asp:TextBox ID="txtAssociateDesignDR3" runat="server" Width="150" MaxLength="50" /></td>
                                            <td><asp:TextBox ID="txtAssociateDesignName3" runat="server" Width="150" MaxLength="50" /></td>
                                            <td><asp:Label ID="lblAssociateDesign3" runat="server" /></td>
                                        </tr>
                                        <tr style='display:<%= (intAssociateDesignDevice <= intAssociateDesignDevices ? "inline" : "none")%>'>
                                            <td>Server # <%=intAssociateDesignDevice++ %></td>
                                            <td><asp:TextBox ID="txtAssociateDesignProd4" runat="server" Width="150" MaxLength="50" /></td>
                                            <td><asp:TextBox ID="txtAssociateDesignDR4" runat="server" Width="150" MaxLength="50" /></td>
                                            <td><asp:TextBox ID="txtAssociateDesignName4" runat="server" Width="150" MaxLength="50" /></td>
                                            <td><asp:Label ID="lblAssociateDesign4" runat="server" /></td>
                                        </tr>
                                        <tr style='display:<%= (intAssociateDesignDevice <= intAssociateDesignDevices ? "inline" : "none")%>'>
                                            <td>Server # <%=intAssociateDesignDevice++ %></td>
                                            <td><asp:TextBox ID="txtAssociateDesignProd5" runat="server" Width="150" MaxLength="50" /></td>
                                            <td><asp:TextBox ID="txtAssociateDesignDR5" runat="server" Width="150" MaxLength="50" /></td>
                                            <td><asp:TextBox ID="txtAssociateDesignName5" runat="server" Width="150" MaxLength="50" /></td>
                                            <td><asp:Label ID="lblAssociateDesign5" runat="server" /></td>
                                        </tr>
                                        <tr style='display:<%= (intAssociateDesignDevice <= intAssociateDesignDevices ? "inline" : "none")%>'>
                                            <td>Server # <%=intAssociateDesignDevice++ %></td>
                                            <td><asp:TextBox ID="txtAssociateDesignProd6" runat="server" Width="150" MaxLength="50" /></td>
                                            <td><asp:TextBox ID="txtAssociateDesignDR6" runat="server" Width="150" MaxLength="50" /></td>
                                            <td><asp:TextBox ID="txtAssociateDesignName6" runat="server" Width="150" MaxLength="50" /></td>
                                            <td><asp:Label ID="lblAssociateDesign6" runat="server" /></td>
                                        </tr>
                                        <tr style='display:<%= (intAssociateDesignDevice <= intAssociateDesignDevices ? "inline" : "none")%>'>
                                            <td>Server # <%=intAssociateDesignDevice++ %></td>
                                            <td><asp:TextBox ID="txtAssociateDesignProd7" runat="server" Width="150" MaxLength="50" /></td>
                                            <td><asp:TextBox ID="txtAssociateDesignDR7" runat="server" Width="150" MaxLength="50" /></td>
                                            <td><asp:TextBox ID="txtAssociateDesignName7" runat="server" Width="150" MaxLength="50" /></td>
                                            <td><asp:Label ID="lblAssociateDesign7" runat="server" /></td>
                                        </tr>
                                        <tr style='display:<%= (intAssociateDesignDevice <= intAssociateDesignDevices ? "inline" : "none")%>'>
                                            <td>Server # <%=intAssociateDesignDevice++ %></td>
                                            <td><asp:TextBox ID="txtAssociateDesignProd8" runat="server" Width="150" MaxLength="50" /></td>
                                            <td><asp:TextBox ID="txtAssociateDesignDR8" runat="server" Width="150" MaxLength="50" /></td>
                                            <td><asp:TextBox ID="txtAssociateDesignName8" runat="server" Width="150" MaxLength="50" /></td>
                                            <td><asp:Label ID="lblAssociateDesign8" runat="server" /></td>
                                        </tr>
                                        <tr style='display:<%= (intAssociateDesignDevice <= intAssociateDesignDevices ? "inline" : "none")%>'>
                                            <td>Server # <%=intAssociateDesignDevice++ %></td>
                                            <td><asp:TextBox ID="txtAssociateDesignProd9" runat="server" Width="150" MaxLength="50" /></td>
                                            <td><asp:TextBox ID="txtAssociateDesignDR9" runat="server" Width="150" MaxLength="50" /></td>
                                            <td><asp:TextBox ID="txtAssociateDesignName9" runat="server" Width="150" MaxLength="50" /></td>
                                            <td><asp:Label ID="lblAssociateDesign9" runat="server" /></td>
                                        </tr>
                                        <tr style='display:<%= (intAssociateDesignDevice <= intAssociateDesignDevices ? "inline" : "none")%>'>
                                            <td>Server # <%=intAssociateDesignDevice++ %></td>
                                            <td><asp:TextBox ID="txtAssociateDesignProd10" runat="server" Width="150" MaxLength="50" /></td>
                                            <td><asp:TextBox ID="txtAssociateDesignDR10" runat="server" Width="150" MaxLength="50" /></td>
                                            <td><asp:TextBox ID="txtAssociateDesignName10" runat="server" Width="150" MaxLength="50" /></td>
                                            <td><asp:Label ID="lblAssociateDesign10" runat="server" /></td>
                                        </tr>
                                        <tr style='display:<%= (intAssociateDesignDevice <= intAssociateDesignDevices ? "inline" : "none")%>'>
                                            <td>Server # <%=intAssociateDesignDevice++ %></td>
                                            <td><asp:TextBox ID="txtAssociateDesignProd11" runat="server" Width="150" MaxLength="50" /></td>
                                            <td><asp:TextBox ID="txtAssociateDesignDR11" runat="server" Width="150" MaxLength="50" /></td>
                                            <td><asp:TextBox ID="txtAssociateDesignName11" runat="server" Width="150" MaxLength="50" /></td>
                                            <td><asp:Label ID="lblAssociateDesign11" runat="server" /></td>
                                        </tr>
                                        <tr style='display:<%= (intAssociateDesignDevice <= intAssociateDesignDevices ? "inline" : "none")%>'>
                                            <td>Server # <%=intAssociateDesignDevice++ %></td>
                                            <td><asp:TextBox ID="txtAssociateDesignProd12" runat="server" Width="150" MaxLength="50" /></td>
                                            <td><asp:TextBox ID="txtAssociateDesignDR12" runat="server" Width="150" MaxLength="50" /></td>
                                            <td><asp:TextBox ID="txtAssociateDesignName12" runat="server" Width="150" MaxLength="50" /></td>
                                            <td><asp:Label ID="lblAssociateDesign12" runat="server" /></td>
                                        </tr>
                                        <tr style='display:<%= (intAssociateDesignDevice <= intAssociateDesignDevices ? "inline" : "none")%>'>
                                            <td>Server # <%=intAssociateDesignDevice++ %></td>
                                            <td><asp:TextBox ID="txtAssociateDesignProd13" runat="server" Width="150" MaxLength="50" /></td>
                                            <td><asp:TextBox ID="txtAssociateDesignDR13" runat="server" Width="150" MaxLength="50" /></td>
                                            <td><asp:TextBox ID="txtAssociateDesignName13" runat="server" Width="150" MaxLength="50" /></td>
                                            <td><asp:Label ID="lblAssociateDesign13" runat="server" /></td>
                                        </tr>
                                        <tr style='display:<%= (intAssociateDesignDevice <= intAssociateDesignDevices ? "inline" : "none")%>'>
                                            <td>Server # <%=intAssociateDesignDevice++ %></td>
                                            <td><asp:TextBox ID="txtAssociateDesignProd14" runat="server" Width="150" MaxLength="50" /></td>
                                            <td><asp:TextBox ID="txtAssociateDesignDR14" runat="server" Width="150" MaxLength="50" /></td>
                                            <td><asp:TextBox ID="txtAssociateDesignName14" runat="server" Width="150" MaxLength="50" /></td>
                                            <td><asp:Label ID="lblAssociateDesign14" runat="server" /></td>
                                        </tr>
                                        <tr style='display:<%= (intAssociateDesignDevice <= intAssociateDesignDevices ? "inline" : "none")%>'>
                                            <td>Server # <%=intAssociateDesignDevice++ %></td>
                                            <td><asp:TextBox ID="txtAssociateDesignProd15" runat="server" Width="150" MaxLength="50" /></td>
                                            <td><asp:TextBox ID="txtAssociateDesignDR15" runat="server" Width="150" MaxLength="50" /></td>
                                            <td><asp:TextBox ID="txtAssociateDesignName15" runat="server" Width="150" MaxLength="50" /></td>
                                            <td><asp:Label ID="lblAssociateDesign15" runat="server" /></td>
                                        </tr>
                                        <tr style='display:<%= (intAssociateDesignDevice <= intAssociateDesignDevices ? "inline" : "none")%>'>
                                            <td>Server # <%=intAssociateDesignDevice++ %></td>
                                            <td><asp:TextBox ID="txtAssociateDesignProd16" runat="server" Width="150" MaxLength="50" /></td>
                                            <td><asp:TextBox ID="txtAssociateDesignDR16" runat="server" Width="150" MaxLength="50" /></td>
                                            <td><asp:TextBox ID="txtAssociateDesignName16" runat="server" Width="150" MaxLength="50" /></td>
                                            <td><asp:Label ID="lblAssociateDesign16" runat="server" /></td>
                                        </tr>
                                        <tr style='display:<%= (intAssociateDesignDevice <= intAssociateDesignDevices ? "inline" : "none")%>'>
                                            <td>Server # <%=intAssociateDesignDevice++ %></td>
                                            <td><asp:TextBox ID="txtAssociateDesignProd17" runat="server" Width="150" MaxLength="50" /></td>
                                            <td><asp:TextBox ID="txtAssociateDesignDR17" runat="server" Width="150" MaxLength="50" /></td>
                                            <td><asp:TextBox ID="txtAssociateDesignName17" runat="server" Width="150" MaxLength="50" /></td>
                                            <td><asp:Label ID="lblAssociateDesign17" runat="server" /></td>
                                        </tr>
                                        <tr style='display:<%= (intAssociateDesignDevice <= intAssociateDesignDevices ? "inline" : "none")%>'>
                                            <td>Server # <%=intAssociateDesignDevice++ %></td>
                                            <td><asp:TextBox ID="txtAssociateDesignProd18" runat="server" Width="150" MaxLength="50" /></td>
                                            <td><asp:TextBox ID="txtAssociateDesignDR18" runat="server" Width="150" MaxLength="50" /></td>
                                            <td><asp:TextBox ID="txtAssociateDesignName18" runat="server" Width="150" MaxLength="50" /></td>
                                            <td><asp:Label ID="lblAssociateDesign18" runat="server" /></td>
                                        </tr>
                                        <tr style='display:<%= (intAssociateDesignDevice <= intAssociateDesignDevices ? "inline" : "none")%>'>
                                            <td>Server # <%=intAssociateDesignDevice++ %></td>
                                            <td><asp:TextBox ID="txtAssociateDesignProd19" runat="server" Width="150" MaxLength="50" /></td>
                                            <td><asp:TextBox ID="txtAssociateDesignDR19" runat="server" Width="150" MaxLength="50" /></td>
                                            <td><asp:TextBox ID="txtAssociateDesignName19" runat="server" Width="150" MaxLength="50" /></td>
                                            <td><asp:Label ID="lblAssociateDesign19" runat="server" /></td>
                                        </tr>
                                        <tr style='display:<%= (intAssociateDesignDevice <= intAssociateDesignDevices ? "inline" : "none")%>'>
                                            <td>Server # <%=intAssociateDesignDevice++ %></td>
                                            <td><asp:TextBox ID="txtAssociateDesignProd20" runat="server" Width="150" MaxLength="50" /></td>
                                            <td><asp:TextBox ID="txtAssociateDesignDR20" runat="server" Width="150" MaxLength="50" /></td>
                                            <td><asp:TextBox ID="txtAssociateDesignName20" runat="server" Width="150" MaxLength="50" /></td>
                                            <td><asp:Label ID="lblAssociateDesign20" runat="server" /></td>
                                        </tr>
                                    </table>
		                        </td>
		                    </tr>
		                    <tr>
		                        <td><asp:Button ID="btnAssociateDesign2" runat="server" CssClass="default" Width="75" OnClick="btnAssociateDesign2_Click" Text="Associate" /></td>
		                    </tr>
		                </table>
		            </td>
		        </tr>
		        <tr>
		            <td colspan="2"><asp:Label ID="lblAssociateDesign" runat="server" CssClass="default" /></td>
		        </tr>
		        <tr><td colspan="2"><a name="decommission">&nbsp;</a></td></tr>
		        <tr> 
		            <td colspan="2"><b>Set Decommission</b></td>
		        </tr>
		        <tr>
		            <td nowrap>Servers:</td>
		            <td width="100%"><asp:TextBox ID="txtDecomServers" runat="server" CssClass="default" Width="200" TextMode="MultiLine" Rows="5" /></td>
		        </tr>
		        <tr>
		            <td nowrap>Reason:</td>
		            <td width="100%"><asp:TextBox ID="txtDecomReason" runat="server" CssClass="default" Width="200" TextMode="MultiLine" Rows="5" /></td>
		        </tr>
		        <tr>
		            <td nowrap>Change Control:</td>
		            <td width="100%"><asp:TextBox ID="txtDecomPTM" runat="server" CssClass="default" Width="100" MaxLength="10" /></td>
		        </tr>
		        <tr>
		            <td nowrap>Requestor:</td>
		            <td width="100%">
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td><asp:TextBox ID="txtUser" runat="server" Width="300" CssClass="default" /></td>
                            </tr>
                            <tr>
                                <td>
                                    <div id="divAJAX" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                                        <asp:ListBox ID="lstAJAX" runat="server" CssClass="default" />
                                    </div>
                                </td>
                            </tr>
                        </table>
                        <input type="hidden" id="hdnAJAXValue" name="hdnAJAXValue" />
		            </td>
		        </tr>
		        <tr>
		            <td nowrap>Date:</td>
		            <td width="100%"><asp:TextBox ID="txtDecomDate" runat="server" CssClass="default" Width="100" MaxLength="10" /></td>
		        </tr>
		        <tr>
		            <td nowrap>Status:</td>
		            <td width="100%">
		                <asp:RadioButton ID="radDecomDecom" runat="server" Text="Decommissioned" GroupName="radDecom" Checked="true" />
		                <asp:RadioButton ID="radDecomDestroy" runat="server" Text="Destroyed" GroupName="radDecom" />
		            </td>
		        </tr>
		        <tr>
		            <td colspan="2"><asp:Button ID="btnDecommission" runat="server" CssClass="default" Width="75" OnClick="btnDecommission_Click" Text="Go" /></td>
		        </tr>
                <tr>
                    <td colspan="2"><asp:Label ID="lblDecommission" runat="server" /></td>
                </tr>
		        <tr><td colspan="2"><a name="decommission_search">&nbsp;</a></td></tr>
		        <tr> 
		            <td colspan="2"><b>Decommission Search</b></td>
		        </tr>
		        <tr>
		            <td nowrap>Server Name:</td>
		            <td width="100%"><asp:TextBox ID="txtDecommissionSearch" runat="server" CssClass="default" Width="200" /></td>
		        </tr>
		        <tr>
		            <td colspan="2"><asp:Button ID="btnDecommissionSearch" runat="server" CssClass="default" Width="75" OnClick="btnDecommissionSearch_Click" Text="Go" /></td>
		        </tr>
                <tr id="panDecommissionSearch" runat="server" visible="false">
                    <td colspan="2">
                        <table cellpadding="7" border="1">
                            <tr>
                                <td class="bold">Type:</td>
                                <td><asp:Label ID="lblDecomType" runat="server" /></td>
                            </tr>
                            <tr>
                                <td class="bold">RequestID:</td>
                                <td><asp:Label ID="lblDecomRequestID" runat="server" /></td>
                            </tr>
                            <tr>
                                <td class="bold">Submitter:</td>
                                <td><asp:Label ID="lblDecomSubmitter" runat="server" /></td>
                            </tr>
                            <tr>
                                <td class="bold">Power Off:</td>
                                <td><asp:Label ID="lblDecomPoweroff" runat="server" /></td>
                            </tr>
                            <tr>
                                <td class="bold" valign="top">Reason:</td>
                                <td><asp:Label ID="lblDecomReason" runat="server" /></td>
                            </tr>
                            <tr>
                                <td class="bold">Change #:</td>
                                <td><asp:Label ID="lblDecomChange" runat="server" /></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="2"><asp:Label ID="lblDecommissionSearch" runat="server" /></td>
                </tr>
		        <tr><td colspan="2"><a name="remove_dr">&nbsp;</a></td></tr>
		        <tr> 
		            <td colspan="2"><b>Clear DR for Design</b></td>
		        </tr>
		        <tr>
		            <td nowrap>Design ID:</td>
		            <td width="100%"><asp:TextBox ID="txtClearDR" runat="server" CssClass="default" Width="100" MaxLength="10" /></td>
		        </tr>
		        <tr>
		            <td colspan="2"><asp:Button ID="btnClearDR" runat="server" CssClass="default" Width="75" OnClick="btnClearDR_Click" Text="Go" /></td>
		        </tr>
                <tr>
                    <td colspan="2"><asp:Label ID="lblClearDR" runat="server" /></td>
                </tr>
		        <tr><td colspan="2">&nbsp;</td></tr>
		        <tr> 
		            <td colspan="2"><b>Setup DEV / TEST Environments</b></td>
		        </tr>
		        <tr>
		            <td nowrap>User ID (for Services):</td>
		            <td width="100%"><asp:TextBox ID="txtDevTestUser" runat="server" CssClass="default" Width="100" MaxLength="10" /></td>
		        </tr>
		        <tr>
		            <td nowrap>Application ID (for assignment):</td>
		            <td width="100%"><asp:TextBox ID="txtDevTestApp" runat="server" CssClass="default" Width="100" MaxLength="10" /></td>
		        </tr>
		        <tr>
		            <td colspan="2"><asp:Button ID="btnDevTest" runat="server" CssClass="default" Width="75" OnClick="btnDevTest_Click" Text="Go" /></td>
		        </tr>
		        <tr>
		            <td colspan="2"><asp:Label ID="lblDevTest" runat="server" CssClass="default" /></td>
		        </tr>
		        <tr><td colspan="2">&nbsp;</td></tr>
		        <tr> 
		            <td colspan="2"><b>Initialize Missing PNC Pre-Production Online Tasks</b></td>
		        </tr>
		        <tr>
		            <td colspan="2">
		                <asp:Button ID="btnPreProdGet" runat="server" CssClass="default" Width="75" OnClick="btnPreProdGet_Click" Text="Find" /> 
		                <asp:Button ID="btnPreProdSelect" runat="server" CssClass="default" Width="75" OnClick="btnPreProdSelect_Click" Text="Select All" />
		            </td>
		        </tr>
		        <tr>
		            <td colspan="2"><asp:ListBox ID="lstPreProdGet" runat="server" Width="400" Rows="10" CssClass="default" SelectionMode="Multiple" /></td>
		        </tr>
		        <tr>
		            <td colspan="2"><asp:Button ID="btnPreProdGo" runat="server" CssClass="default" Width="75" OnClick="btnPreProdGo_Click" Text="Execute" Enabled="false" /></td>
		        </tr>
		        <tr>
		            <td colspan="2"><asp:Label ID="lblPreProdGet" runat="server" CssClass="default" /></td>
		        </tr>
		        <tr><td colspan="2">&nbsp;</td></tr>
		        <tr> 
		            <td colspan="2"><b>Reset Virtual Workstation</b></td>
		        </tr>
		        <tr>
		            <td colspan="2"><img src="/images/bigAlert.gif" border="0" align="absmiddle" /><b>WARNING:</b> Make sure you know what you are doing before you click anything...this action cannot be undone!</td>
		        </tr>
		        <tr>
		            <td nowrap>Workstation ID:</td>
		            <td width="100%"><asp:TextBox ID="txtID" runat="server" CssClass="default" Width="100" MaxLength="10" /></td>
		        </tr>
		        <tr>
		            <td colspan="2"><asp:Button ID="btnGo" runat="server" CssClass="default" Width="75" OnClick="btnGo_Click" Text="Go" /></td>
		        </tr>
		        <tr>
		            <td colspan="2"><asp:Label ID="lblDone" runat="server" CssClass="default" /></td>
		        </tr>
		        <tr><td colspan="2"><a name="anchoring_itemid_change">&nbsp;</a></td></tr>
		        <tr> 
		            <td colspan="2"><b>Change Service ITEM ID</b></td>
		        </tr>
		        <tr>
		            <td nowrap>Service ID:</td>
		            <td width="100%"><asp:TextBox ID="txtServiceID" runat="server" CssClass="default" Width="100" MaxLength="10" /></td>
		        </tr>
		        <tr>
		            <td nowrap>Item ID (OLD):</td>
		            <td width="100%"><asp:TextBox ID="txtServiceItemID_OLD" runat="server" CssClass="default" Width="100" MaxLength="10" /></td>
		        </tr>
		        <tr>
		            <td nowrap>Item ID (NEW):</td>
		            <td width="100%"><asp:TextBox ID="txtServiceItemID_NEW" runat="server" CssClass="default" Width="100" MaxLength="10" /></td>
		        </tr>
		        <tr>
		            <td colspan="2"><asp:Button ID="btnServiceItemID" runat="server" CssClass="default" Width="75" OnClick="btnServiceItemID_Click" Text="Go" /></td>
		        </tr>
		        <tr>
		            <td colspan="2"><asp:Label ID="lblServiceItemID" runat="server" CssClass="default" /></td>
		        </tr>
		        <tr><td colspan="2">&nbsp;</td></tr>
		        <tr> 
		            <td colspan="2"><b>Get Model from Answerid</b></td>
		        </tr>
		        <tr>
		            <td nowrap>Answer ID:</td>
		            <td width="100%"><asp:TextBox ID="txtAnswer" runat="server" CssClass="default" Width="100" MaxLength="10" /></td>
		        </tr>
		        <tr>
		            <td colspan="2"><asp:Button ID="btnAnswer" runat="server" CssClass="default" Width="75" OnClick="btnAnswer_Click" Text="Go" /></td>
		        </tr>
		        <tr>
		            <td colspan="2"><asp:Label ID="lblAnswer" runat="server" CssClass="default" /></td>
		        </tr>
		        <tr><td colspan="2">&nbsp;</td></tr>
		        <tr> 
		            <td colspan="2"><b>Copy ServiceEditor SERVICEID to Another Service</b></td>
		        </tr>
		        <tr>
		            <td nowrap>Service ID From:</td>
		            <td width="100%"><asp:TextBox ID="txtServiceIDFrom" runat="server" CssClass="default" Width="100" MaxLength="10" /></td>
		        </tr>
		        <tr>
		            <td nowrap>Service ID To:</td>
		            <td width="100%"><asp:TextBox ID="txtServiceIDTo" runat="server" CssClass="default" Width="100" MaxLength="10" /></td>
		        </tr>
		        <tr>
		            <td colspan="2"><asp:CheckBox ID="chkServiceID" runat="server" CssClass="default" Text="Delete existing configuration" /></td>
		        </tr>
		        <tr>
		            <td colspan="2"><asp:Button ID="btnServiceID" runat="server" CssClass="default" Width="75" OnClick="btnServiceID_Click" Text="Go" /></td>
		        </tr>
		        <tr>
		            <td colspan="2"><asp:Label ID="lblServiceID" runat="server" CssClass="default" /></td>
		        </tr>
		        <tr><td colspan="2">&nbsp;</td></tr>
		        <tr> 
		            <td colspan="2"><b>Generate and Assign SAN Form</b></td>
		        </tr>
		        <tr>
		            <td nowrap>AnswerID:</td>
		            <td width="100%"><asp:TextBox ID="txtAnswerSAN" runat="server" CssClass="default" Width="100" MaxLength="10" /></td>
		        </tr>
		        <tr>
		            <td colspan="2"><asp:CheckBox ID="chkAnswerSAN" runat="server" CssClass="default" Text="Production?" /></td>
		        </tr>
		        <tr>
		            <td colspan="2"><asp:Button ID="btnAnswerSAN" runat="server" CssClass="default" Width="75" OnClick="btnAnswerSAN_Click" Text="Go" /></td>
		        </tr>
		        <tr>
		            <td colspan="2"><asp:Label ID="lblAnswerSAN" runat="server" CssClass="default" /></td>
		        </tr>
		        <tr><td colspan="2">&nbsp;</td></tr>
		        <tr> 
		            <td colspan="2"><b>Generate and Assign TSM Form</b></td>
		        </tr>
		        <tr>
		            <td nowrap>AnswerID:</td>
		            <td width="100%"><asp:TextBox ID="txtAnswerTSM" runat="server" CssClass="default" Width="100" MaxLength="10" /></td>
		        </tr>
		        <tr>
		            <td colspan="2"><asp:Button ID="btnAnswerTSM" runat="server" CssClass="default" Width="75" OnClick="btnAnswerTSM_Click" Text="Go" /></td>
		        </tr>
		        <tr>
		            <td colspan="2"><asp:Label ID="lblAnswerTSM" runat="server" CssClass="default" /></td>
		        </tr>
		        <tr><td colspan="2">&nbsp;</td></tr>
		        <tr> 
		            <td colspan="2"><b>Get Production / DR Assets</b></td>
		        </tr>
		        <tr>
		            <td nowrap>Server Name:</td>
		            <td width="100%"><asp:TextBox ID="txtProdDRAsset" runat="server" CssClass="default" Width="200" MaxLength="20" /></td>
		        </tr>
		        <tr>
		            <td colspan="2"><asp:CheckBox ID="chkProdDRAsset" runat="server" CssClass="default" Text="Release Build/Test?" /></td>
		        </tr>
		        <tr>
		            <td colspan="2"><asp:CheckBox ID="chkProdDRAssetOnly" runat="server" CssClass="default" Text="DR Only (380s Only)?" /></td>
		        </tr>
		        <tr>
		            <td colspan="2"><asp:Button ID="btnProdDRAsset" runat="server" CssClass="default" Width="75" OnClick="btnProdDRAsset_Click" Text="Go" /></td>
		        </tr>
		        <tr>
		            <td colspan="2"><asp:Label ID="lblProdDRAsset" runat="server" CssClass="default" /></td>
		        </tr>
		        <tr><td colspan="2">&nbsp;</td></tr>
		        <tr> 
		            <td colspan="2"><b>Fix Resource Request / Resource Request Workflow Complete Status</b></td>
		        </tr>
		        <tr>
		            <td colspan="2"><asp:Button ID="btnRR" runat="server" CssClass="default" Width="75" OnClick="btnRR_Click" Text="Go" /></td>
		        </tr>
		        <tr>
		            <td colspan="2"><asp:Label ID="lblRR" runat="server" CssClass="default" /></td>
		        </tr>
		        <tr><td colspan="2">&nbsp;</td></tr>
		        <tr> 
		            <td colspan="2"><b>Fix Invalid Auto-Provisioning Services</b></td>
		        </tr>
		        <tr>
		            <td colspan="2"><asp:Button ID="btnFix" runat="server" CssClass="default" Width="75" OnClick="btnFix_Click" Text="Go" /></td>
		        </tr>
		        <tr>
		            <td colspan="2"><asp:Label ID="lblFix" runat="server" CssClass="default" /></td>
		        </tr>
		        <tr><td colspan="2">&nbsp;</td></tr>
		        <tr> 
		            <td colspan="2"><b>Check for Duplicate Physical / Blade</b></td>
		        </tr>
		        <tr>
		            <td colspan="2">
		                <asp:RadioButtonList ID="radDuplicate" runat="server" CssClass="default" RepeatDirection="Horizontal" >
		                    <asp:ListItem Value="FIRST" Text="Leave First" />
		                    <asp:ListItem Value="LAST" Text="Leave Last" />
		                </asp:RadioButtonList>
		            </td>
		        </tr>
		        <tr>
		            <td colspan="2"><asp:Button ID="btnDuplicate" runat="server" CssClass="default" Width="75" OnClick="btnDuplicate_Click" Text="Go" /></td>
		        </tr>
		        <tr>
		            <td colspan="2"><asp:Label ID="lblDuplicate" runat="server" CssClass="default" /></td>
		        </tr>
		        <tr><td colspan="2">&nbsp;</td></tr>
		        <tr> 
		            <td colspan="2"><b>Import User Configuration</b></td>
		        </tr>
                <tr>
                    <td><a href="/admin/imports/templates/users.xls" target="_blank">File:</a></td>
                    <td><asp:FileUpload ID="filImportUser" runat="server" Width="450" CssClass="default" /></td>
                </tr>
		        <tr>
		            <td colspan="2"><asp:Button ID="btnImportUser" runat="server" CssClass="default" Width="75" OnClick="btnImportUser_Click" Text="Go" /></td>
		        </tr>
		        <tr>
		            <td colspan="2"><asp:Label ID="lblImportUser" runat="server" CssClass="default" /></td>
		        </tr>
		        <tr><td colspan="2"><a name="decoms">&nbsp;</a></td></tr>
		        <tr> 
		            <td colspan="2"><b>Import Server Decommissions</b></td>
		        </tr>
                <tr>
                    <td><a href="/admin/imports/templates/decoms.xls" target="_blank">File:</a></td>
                    <td><asp:FileUpload ID="filImportDecom" runat="server" Width="450" CssClass="default" /></td>
                </tr>
		        <tr>
		            <td>Decom Service ID:</td>
		            <td><asp:TextBox ID="txtImportDecom" runat="server" CssClass="default" Width="100" MaxLength="5" Text="789" /></td>
		        </tr>
		        <tr>
		            <td colspan="2"><asp:Button ID="btnImportDecom" runat="server" CssClass="default" Width="75" OnClick="btnImportDecom_Click" Text="Go" /></td>
		        </tr>
		        <tr>
		            <td colspan="2"><asp:Label ID="lblImportDecom" runat="server" CssClass="default" /></td>
		        </tr>
		        <tr><td colspan="2">&nbsp;</td></tr>
		        <tr> 
		            <td colspan="2"><b>Add UserID to ALL DataPoint Fields</b></td>
		        </tr>
		        <tr>
		            <td nowrap>User ID:</td>
		            <td width="100%"><asp:TextBox ID="txtDataPoint" runat="server" CssClass="default" Width="100" MaxLength="10" /></td>
		        </tr>
		        <tr>
		            <td colspan="2"><asp:Button ID="btnDataPoint" runat="server" CssClass="default" Width="75" OnClick="btnDataPoint_Click" Text="Go" /></td>
		        </tr>
		        <tr>
		            <td colspan="2"><asp:Label ID="lblDataPoint" runat="server" CssClass="default" /></td>
		        </tr>
		        <tr><td colspan="2">&nbsp;</td></tr>
		        <tr> 
		            <td colspan="2"><b>Notify Implementor of Execution</b></td>
		        </tr>
		        <tr>
		            <td nowrap>Design ID:</td>
		            <td width="100%"><asp:TextBox ID="txtNotifyImplementor" runat="server" CssClass="default" Width="100" MaxLength="10" /></td>
		        </tr>
		        <tr>
		            <td colspan="2"><asp:Button ID="btnNotifyImplementor" runat="server" CssClass="default" Width="75" OnClick="btnNotifyImplementor_Click" Text="Go" /></td>
		        </tr>
		        <tr>
		            <td colspan="2"><asp:Label ID="lblNotifyImplementor" runat="server" CssClass="default" /></td>
		        </tr>
		        <tr><td colspan="2">&nbsp;</td></tr>
		        <tr> 
		            <td colspan="2"><b>Kick off PNC DNS Request</b></td>
		        </tr>
		        <tr>
		            <td nowrap>Design ID:</td>
		            <td width="100%"><asp:TextBox ID="txtPNCDNS" runat="server" CssClass="default" Width="100" MaxLength="10" /></td>
		        </tr>
		        <tr>
		            <td colspan="2"><asp:Button ID="btnPNCDNS" runat="server" CssClass="default" Width="75" OnClick="btnPNCDNS_Click" Text="Go" /></td>
		        </tr>
		        <tr>
		            <td colspan="2"><asp:Label ID="lblPNCDNS" runat="server" CssClass="default" /></td>
		        </tr>
		        <tr><td colspan="2"><a name="IDs">&nbsp;</a></td></tr>
		        <tr> 
		            <td colspan="2"><b>Get NCC ID or PNC ID</b></td>
		        </tr>
		        <tr>
		            <td nowrap>ID:</td>
		            <td width="100%"><asp:TextBox ID="txtGetID" runat="server" CssClass="default" Width="100" MaxLength="10" /></td>
		        </tr>
		        <tr>
		            <td colspan="2"><asp:Button ID="btnGetID" runat="server" CssClass="default" Width="75" OnClick="btnGetID_Click" Text="Go" /></td>
		        </tr>
		        <tr>
		            <td colspan="2"><asp:Label ID="lblGetID" runat="server" CssClass="default" /></td>
		        </tr>
		        <tr><td colspan="2">&nbsp;</td></tr>
		        <tr> 
		            <td colspan="2"><b>Switchport Modification</b>&nbsp;&nbsp;(Only for IOS switches)</td>
		        </tr>
		        <tr>
		            <td nowrap>Switch:</td>
		            <td width="100%"><asp:TextBox ID="txtSwitchName" runat="server" CssClass="default" Width="300" /> [fully qualified name] Ex: clelabsrvsw07.tstctr.ntl-city.net</td>
		        </tr>
		        <tr>
		            <td nowrap>Serial #:</td>
		            <td width="100%"><asp:TextBox ID="txtSwitchSerial" runat="server" CssClass="default" Width="300" /> Used for validation</td>
		        </tr>
		        <tr>
		            <td nowrap>Port:</td>
		            <td width="100%"><asp:TextBox ID="txtSwitchPort" runat="server" CssClass="default" Width="100" /> Ex: Gi4/11</td>
		        </tr>
		        <tr>
		            <td nowrap>VLAN:</td>
		            <td width="100%"><asp:TextBox ID="txtSwitchVLAN" runat="server" CssClass="default" Width="100" /> Ex: 215</td>
		        </tr>
                <tr>
                    <td nowrap>Show Debug:</td>
                    <td width="100%"><asp:CheckBox ID="chkSwitch" runat="server" CssClass="default" /></td>
                </tr>
		        <tr>
		            <td colspan="2"><asp:Button ID="btnSwitch" runat="server" CssClass="default" Width="75" OnClick="btnSwitch_Click" Text="Change" /></td>
		        </tr>
		        <tr>
		            <td colspan="2"><asp:TextBox ID="txtSwitchResult" runat="server" CssClass="default" Width="500" Rows="10" TextMode="MultiLine" Visible="false" /></td>
		        </tr>
		        <tr><td colspan="2">&nbsp;</td></tr>
		        <tr> 
		            <td colspan="2"><b>Initiate PNC Task</b></td>
		        </tr>
		        <tr>
		            <td nowrap>Design ID:</td>
		            <td width="100%"><asp:TextBox ID="txtAudit" runat="server" CssClass="default" Width="300" MaxLength="100" /></td>
		        </tr>
		        <tr>
		            <td nowrap>Service ID:</td>
		            <td width="100%"><asp:TextBox ID="txtAuditService" runat="server" CssClass="default" Width="300" MaxLength="100" /></td>
		        </tr>
                <tr>
                    <td nowrap></td>
                    <td width="100%"><asp:CheckBox ID="chkAudit" runat="server" CssClass="default" Text="Assign to Implementor" /></td>
                </tr>
		        <tr>
		            <td colspan="2"><asp:Button ID="btnAudit" runat="server" CssClass="default" Width="75" OnClick="btnAudit_Click" Text="Go" /></td>
		        </tr>
		        <tr>
		            <td colspan="2"><asp:Label ID="lblAudit" runat="server" CssClass="default" /></td>
		        </tr>
		        <tr><td colspan="2">&nbsp;</td></tr>
		        <tr> 
		            <td colspan="2"><b>Close out all Pre-Production / Post Build Tasks</b></td>
		        </tr>
		        <tr>
		            <td nowrap>Design ID:</td>
		            <td width="100%"><asp:TextBox ID="txtPreProd" runat="server" CssClass="default" Width="100" MaxLength="10" /></td>
		        </tr>
		        <tr>
		            <td colspan="2"><asp:Button ID="btnPreProd" runat="server" CssClass="default" Width="75" OnClick="btnPreProd_Click" Text="Go" /></td>
		        </tr>
		        <tr>
		            <td colspan="2"><asp:Label ID="lblPreProd" runat="server" CssClass="default" /></td>
		        </tr>
		        <tr><td colspan="2"><a name="SetupAppDocs">&nbsp;</a></td></tr>
		        <tr> 
		            <td colspan="2"><b>Setup Application Documents</b></td>
		        </tr>
		        <tr>
		            <td nowrap>Application ID:</td>
		            <td width="100%"><asp:TextBox ID="txtSetupAppDocs" runat="server" CssClass="default" Width="100" MaxLength="10" /></td>
		        </tr>
		        <tr>
		            <td colspan="2"><asp:Button ID="btnSetupAppDocs" runat="server" CssClass="default" Width="75" OnClick="btnSetupAppDocs_Click" Text="Go" /></td>
		        </tr>
                <tr>
                    <td colspan="2"><asp:Label ID="lblSetupAppDocs" runat="server" /></td>
                </tr>
		        <tr><td colspan="2"><a name="DatabaseFieldData">&nbsp;</a></td></tr>
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
		        <tr><td colspan="2">&nbsp;</td></tr>
		        <tr> 
		            <td colspan="2"><b>Update TEXT Field</b></td>
		        </tr>
		        <tr>
		            <td nowrap>DSN:</td>
		            <td width="100%">
	                    <asp:DropDownList ID="ddlTEXT" runat="server" CssClass="default">
	                        <asp:ListItem Value="devDSN" />
	                        <asp:ListItem Value="devDSNAsset" />
	                        <asp:ListItem Value="devDSNip" />
	                        <asp:ListItem Value="devDSNService" />
	                        <asp:ListItem Value="devDSNServiceEditor" />
	                        <asp:ListItem Value="testDSN" />
	                        <asp:ListItem Value="testDSNAsset" />
	                        <asp:ListItem Value="testDSNip" />
	                        <asp:ListItem Value="testDSNService" />
	                        <asp:ListItem Value="testDSNServiceEditor" />
	                        <asp:ListItem Value="prodDSN" />
	                        <asp:ListItem Value="prodDSNAsset" />
	                        <asp:ListItem Value="prodDSNip" />
	                        <asp:ListItem Value="prodDSNService" />
	                        <asp:ListItem Value="prodDSNServiceEditor" />
	                    </asp:DropDownList>
		            </td>
		        </tr>
		        <tr>
		            <td nowrap>Text:</td>
		            <td width="100%"><asp:TextBox ID="txtTEXTValue" runat="server" CssClass="default" TextMode="MultiLine" Rows="8" Width="400" /></td>
		        </tr>
		        <tr>
		            <td colspan="2"><img src="/images/bigAlert.gif" border="0" align="absmiddle" /><b>NOTE:</b> Prior to executing, make sure the stored procedure <b>xxx_UpdateTextValue</b> exists and is configured to receive this value.</td>
		        </tr>
		        <tr>
		            <td colspan="2"><asp:Button ID="btnTEXT" runat="server" CssClass="default" Width="75" OnClick="btnTEXT_Click" Text="Go" /></td>
		        </tr>
		        <tr>
		            <td colspan="2"><asp:Label ID="lblTEXT" runat="server" CssClass="default" /></td>
		        </tr>
		        <tr><td colspan="2"><a name="settings">&nbsp;</a></td></tr>
		        <tr> 
		            <td colspan="2"><b>Update Settings</b></td>
		        </tr>
		        <tr>
		            <td nowrap>Freeze Start:</td>
		            <td width="100%"><asp:TextBox ID="txtUpdateSettingsFreezeStart" runat="server" CssClass="default" Width="100" MaxLength="10" /></td>
		        </tr>
		        <tr>
		            <td nowrap>Freeze End:</td>
		            <td width="100%"><asp:TextBox ID="txtUpdateSettingsFreezeEnd" runat="server" CssClass="default" Width="100" MaxLength="10" /></td>
		        </tr>
		        <tr>
		            <td nowrap>Freeze Skip RequestID:</td>
		            <td width="100%">CVT<asp:TextBox ID="txtUpdateSettingsFreezeSkip" runat="server" CssClass="default" Width="100" MaxLength="10" /> (Process during the freeze)</td>
		        </tr>
		        <tr>
		            <td nowrap>Decom Override RequestID:</td>
		            <td width="100%">CVT<asp:TextBox ID="txtUpdateSettingsDecom" runat="server" CssClass="default" Width="100" MaxLength="10" /> (Process during the day)</td>
		        </tr>
		        <tr>
		            <td nowrap>Destroy Override RequestID:</td>
		            <td width="100%">CVT<asp:TextBox ID="txtUpdateSettingsDestroy" runat="server" CssClass="default" Width="100" MaxLength="10" /> (Process during the day)</td>
		        </tr>
		        <tr>
		            <td colspan="2"><asp:Button ID="btnUpdateSettings" runat="server" CssClass="default" Width="75" OnClick="btnUpdateSettings_Click" Text="Go" /></td>
		        </tr>
		        <tr>
		            <td colspan="2"><asp:Label ID="lblUpdateSettings" runat="server" CssClass="default" /></td>
		        </tr>
            </table>
        </td>
        </tr>
        </table>
</form>
</body>
</html>
