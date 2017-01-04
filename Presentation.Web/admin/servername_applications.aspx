<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="servername_applications.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.servername_applications" %>


<script type="text/javascript">
    var oAdd = null;
    var oView = null;
    var oAddButton = null;
    var oDeleteButton = null;
    var oHidden = null;
    var oName = null;
    var oCode = null;
    var oFactoryCode = null;
    var oFactoryCodeSpecific = null;
    var oZEUSArrayConfig = null;
    var oZEUSOs = null;
    var oZEUSOsVersion = null;
    var oZEUSBuildType = null;
    var oADMoveLocation = null;
    var oForecast = null;
    var oPermitNoReplication = null;
    var oSolutionCode =null;
    var oEnabled = null;
    function Edit(strId, strName, strCode, strFactoryCode, strFactoryCodeSpecific, strZEUSArrayConfig, strZEUSOs, strZEUSOsVersion, strZEUSBuildType, strADMoveLocation, strForecast, strPermitNoReplication, strSolutionCode, strEnabled) {
        oAdd.style.display = "inline";
        oView.style.display = "none";
        oAddButton.value = "Update";
        oDeleteButton.style.display = "inline";
        oHidden.value = strId;
        oName.value = strName;
        oCode.value = strCode;
        oFactoryCode.value = strFactoryCode;
        oFactoryCodeSpecific.value = strFactoryCodeSpecific;
        oZEUSArrayConfig.value = strZEUSArrayConfig;
        oZEUSOs.value = strZEUSOs;
        oZEUSOsVersion.value = strZEUSOsVersion;
        oZEUSBuildType.value = strZEUSBuildType;
        oADMoveLocation.value = strADMoveLocation;
        oForecast.checked = (strForecast == "1");
        oPermitNoReplication.checked = (strPermitNoReplication == "1");
        oEnabled.checked = (strEnabled == "1");
        oSolutionCode.selectedIndex = 0;
               for (var i=0; i<oSolutionCode.length; i++) 
               {
                    if (oSolutionCode.options[i].value == strSolutionCode)
                    oSolutionCode.selectedIndex = i;
                }
    }
    function Add() {
        oAdd.style.display = "inline";
        oView.style.display = "none";
        oAddButton.value = "Add";
        oDeleteButton.style.display = "none";
        oHidden.value = "0";
        oName.value = "";
        oCode.value = "";
        oFactoryCode.value = "";
        oFactoryCodeSpecific.value = "";
        oZEUSArrayConfig.value = "";
        oZEUSOs.value = "";
        oZEUSOsVersion.value = "";
        oZEUSBuildType.value = "";
        oADMoveLocation.value = "";
        oForecast.checked = false;
        oPermitNoReplication.checked = false;
        oSolutionCode.selectedIndex = 0;
        oEnabled.checked = true;
        oName.focus();
    }
    function Cancel() {
        oAdd.style.display = "none";
        oView.style.display = "inline";
        return false;
    }
    function Load() {
        oAdd = document.getElementById('<%=divAdd.ClientID%>');
        oView = document.getElementById('<%=divView.ClientID%>');
        oAddButton = document.getElementById('<%=btnAdd.ClientID%>');
        oDeleteButton = document.getElementById('<%=btnDelete.ClientID%>');
        oHidden = document.getElementById('<%=hdnId.ClientID%>');
        oName = document.getElementById('<%=txtName.ClientID%>');
        oCode = document.getElementById('<%=txtCode.ClientID%>');
        oFactoryCode = document.getElementById('<%=txtFactoryCode.ClientID%>');
        oFactoryCodeSpecific = document.getElementById('<%=txtFactoryCodeSpecific.ClientID%>');
        oZEUSArrayConfig = document.getElementById('<%=txtZEUSArrayConfig.ClientID%>');
        oZEUSOs = document.getElementById('<%=txtZEUSOs.ClientID%>');
        oZEUSOsVersion = document.getElementById('<%=txtZEUSOsVersion.ClientID%>');
        oZEUSBuildType = document.getElementById('<%=txtZEUSBuildType.ClientID%>');
        oADMoveLocation = document.getElementById('<%=txtADMoveLocation.ClientID%>');
        oForecast = document.getElementById('<%=chkForecast.ClientID%>');
        oPermitNoReplication = document.getElementById('<%=chkPermitNoReplication.ClientID%>');
        oSolutionCode = document.getElementById('<%=ddlSolutionCodes.ClientID%>');
        oEnabled = document.getElementById('<%=chkEnabled.ClientID%>');
    }
</script>
<html>
<head>
<title>ClearView Administration</title>
<link rel="stylesheet" type="text/css" href="/css/default.css" />
<script type="text/javascript" src="/javascript/global.js"></script>
</head>
<body topmargin="0" leftmargin="0" onload="Load()">
<form id="Form1" runat="server">
        <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
        <td height="100%">
        <div style="height:100%; overflow:auto">
<table width="98%" cellpadding="0" cellspacing="0" border="0" align="center">
		<tr><td colspan="2">&nbsp;</td></tr>
		<tr> 
		    <td><b>Server Name Applications</b></td>
		    <td align="right"><a href="javascript:void(0);" onclick="Add();" class="cmlink" title="Click to Add New">Add New</a></td>
		</tr>
		<tr><td colspan="2">&nbsp;</td></tr>
		<tr>
		    <td colspan="2" align="center">
                <div ID="divView" runat="server">
                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <asp:repeater ID="rptView" runat="server">
                                    <HeaderTemplate>
                                        <table width="100%" cellpadding="3" cellspacing="0" border="0" align="center">
                                            <tr bgcolor="#CCCCCC">
                                                <td class="bold">&nbsp;</td>
                                                <td width="50%" class="bold"><asp:linkbutton ID="lnkName" Text="Name" CommandArgument="name" CssClass="bold" OnClick="OrderView" runat="server"/></td>
                                                <td width="50%" class="bold"><asp:linkbutton ID="lnkModified" Text="Last Modified" CommandArgument="modified" CssClass="bold" OnClick="OrderView" runat="server"/></td>
                                                <td class="bold" align="center"><asp:linkbutton ID="lnkEnabled" Text="Enabled" CommandArgument="enabled" CssClass="bold" OnClick="OrderView" runat="server"/></td>
                                            </tr>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr bgcolor="#EFEFEF" onmouseover="AdminMouseOver(this);" onmouseout="AdminMouseOut(this);" onclick="if(event.srcElement.tagName != 'INPUT')Edit('<%# DataBinder.Eval(Container.DataItem, "id") %>','<%# DataBinder.Eval(Container.DataItem, "name") %>','<%# DataBinder.Eval(Container.DataItem, "code") %>','<%# DataBinder.Eval(Container.DataItem, "factory_code") %>','<%# DataBinder.Eval(Container.DataItem, "factory_code_specific") %>','<%# DataBinder.Eval(Container.DataItem, "zeus_array_config") %>','<%# DataBinder.Eval(Container.DataItem, "zeus_os") %>','<%# DataBinder.Eval(Container.DataItem, "zeus_os_version") %>','<%# DataBinder.Eval(Container.DataItem, "zeus_build_type") %>','<%# DataBinder.Eval(Container.DataItem, "ad_move_location") %>','<%# DataBinder.Eval(Container.DataItem, "forecast") %>','<%# DataBinder.Eval(Container.DataItem, "permit_no_replication") %>','<%# DataBinder.Eval(Container.DataItem, "solutioncode") %>','<%# DataBinder.Eval(Container.DataItem, "enabled") %>');" class="default">
                                            <td width="5%" align="left">&nbsp;<asp:ImageButton ID="btnDelete" OnClick="btnDeleteLink_Click" runat="server" ToolTip="Delete" ImageAlign="AbsMiddle" ImageUrl="/admin/images/delete.gif" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' /></td>
                                            <td width="50%">&nbsp;<%# DataBinder.Eval(Container.DataItem, "name") %></td>
                                            <td width="50%">&nbsp;<%# DataBinder.Eval(Container.DataItem, "modified") %></td>
                                            <td align="center"><asp:ImageButton ID="btnEnable" runat="server" ImageAlign="AbsMiddle" ImageUrl='<%# (DataBinder.Eval(Container.DataItem, "enabled").ToString() == "1") ? "/admin/images/enabled.gif" : "/admin/images/disabled.gif" %>' CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' OnClick="btnEnable_Click" /></td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </table>
                                    </FooterTemplate>
                                </asp:repeater>
                            </td>
                        </tr>
                    </table>
                </div>

                <div id="divAdd" runat="server" style="display:none">
                    <table width="95%" cellpadding="3" cellspacing="0" border="0" align="center">
                        <tr> 
                            <td class="default">Name:</td>
                            <td><asp:textbox ID="txtName" CssClass="default" runat="server" Width="300" MaxLength="100"/></td>
                        </tr>
                        <tr> 
                            <td class="default">Code:</td>
                            <td><asp:textbox ID="txtCode" CssClass="default" runat="server" Width="100" MaxLength="3"/></td>
                        </tr>
                        <tr> 
                            <td class="default">Factory Code:</td>
                            <td><asp:textbox ID="txtFactoryCode" CssClass="default" runat="server" Width="75" MaxLength="2"/></td>
                        </tr>
                        <tr> 
                            <td class="default">Factory Code Specific:</td>
                            <td><asp:textbox ID="txtFactoryCodeSpecific" CssClass="default" runat="server" Width="75" MaxLength="2"/></td>
                        </tr>
                        <tr> 
                            <td class="default">ZEUS Array Config:</td>
                            <td><asp:textbox ID="txtZEUSArrayConfig" CssClass="default" runat="server" Width="100" MaxLength="20"/></td>
                        </tr>
                        <tr> 
                            <td class="default">ZEUS OS:</td>
                            <td><asp:textbox ID="txtZEUSOs" CssClass="default" runat="server" Width="100" MaxLength="20"/></td>
                        </tr>
                        <tr> 
                            <td class="default">ZEUS OS Version:</td>
                            <td><asp:textbox ID="txtZEUSOsVersion" CssClass="default" runat="server" Width="100" MaxLength="20"/></td>
                        </tr>
                        <tr> 
                            <td class="default">ZEUS Build Type:</td>
                            <td><asp:textbox ID="txtZEUSBuildType" CssClass="default" runat="server" Width="150" MaxLength="50"/></td>
                        </tr>
                        <tr> 
                            <td class="default">AD Move Location:</td>
                            <td><asp:textbox ID="txtADMoveLocation" CssClass="default" runat="server" Width="500" MaxLength="100" /></td>
                        </tr>
                        <tr> 
                            <td class="default"></td>
                            <td class="footer">EXAMPLE: OU=OUs_SQL,OU=OUc_Srvs,OU=OUc_DmnCptrs,</td>
                        </tr>
                        <tr> 
                            <td class="default">Include in Forecast:</td>
                            <td><asp:CheckBox ID="chkForecast" runat="server" Checked="true" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Permit No Replication:</td>
                            <td><asp:CheckBox ID="chkPermitNoReplication" runat="server" Checked="true" /></td>
                        </tr>
                         <tr>
                            <td class="default">Solution Codes:</td>
                            <td><asp:DropDownList ID="ddlSolutionCodes" CssClass="default" runat="server"/></td>
                        </tr>
                        <tr> 
                            <td class="default">Enabled:</td>
                            <td><asp:CheckBox ID="chkEnabled" runat="server" Checked="true" /></td>
                        </tr>
                        <tr> 
                            <td class="cmdefault">&nbsp;</td>
                            <td><asp:Button ID="btnOrder" runat="server" Text="Change Order" Width="150" CssClass="default" /></td>
                        </tr>
                        <tr> 
                            <td class="cmdefault">&nbsp;</td>
                            <td><asp:Button ID="btnOS" runat="server" Text="Change OSs" Width="150" CssClass="default" /></td>
                        </tr>
                       
                        <tr><td height="5" colspan="2">&nbsp;</td></tr>
                        <tr> 
                            <td>&nbsp;</td>
                            <td>
                                <asp:button ID="btnAdd" CssClass="default" runat="server" Text="Add" Width="75" OnClick="btnAdd_Click" />
                                <asp:button ID="btnDelete" CssClass="default" runat="server" Text="Delete" Width="75" OnClick="btnDelete_Click" />
                                <asp:button ID="btnCancel" CssClass="default" runat="server" Text="Cancel" Width="75" />
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
        </div>
        </td>
        </tr>
        </table>
    <input type="hidden" id="hdnId" runat="server" />
    <input type="hidden" id="hdnOrder" runat="server" />
</form>
</body>
</html>
