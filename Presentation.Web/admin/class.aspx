<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="class.aspx.cs" Inherits="NCC.ClearView.Presentation.Web._class" %>

<script type="text/javascript">
    var oAdd = null;
    var oView = null;
    var oAddButton = null;
    var oDeleteButton = null;
    var oHidden = null;
    var oName = null;
    var oFactoryCode = null;
    var oForecast = null;
    var oWorkstationVMware = null;
    var oProd = null;
    var oQA = null;
    var oTest = null;
    var oDR = null;
    var oPNC = null;
    var oDomain = null;
    var oEnabled = null;
    function Edit(strId, strName, strFactoryCode, strForecast, strWorkstationVMware, strProd, strQA, strTest, strDR, strPNC, strDomain, strEnabled) {
        oAdd.style.display = "inline";
        oView.style.display = "none";
        oAddButton.value = "Update";
        oDeleteButton.style.display = "inline";
        oHidden.value = strId;
        oName.value = strName;
        oFactoryCode.value = strFactoryCode;
        oForecast.checked = (strForecast == "1");
        oWorkstationVMware.checked = (strWorkstationVMware == "1");
        oProd.checked = (strProd == "1");
        oQA.checked = (strQA == "1");
        oTest.checked = (strTest == "1");
        oDR.checked = (strDR == "1");
        oPNC.checked = (strPNC == "1");
        oDomain.value = strDomain;
        oEnabled.checked = (strEnabled == "1");
    }
    function Add() {
        oAdd.style.display = "inline";
        oView.style.display = "none";
        oAddButton.value = "Add";
        oDeleteButton.style.display = "none";
        oHidden.value = "0";
        oName.value = "";
        oFactoryCode.value = "";
        oForecast.checked = false;
        oWorkstationVMware.checked = false;
        oProd.checked = false;
        oQA.checked = false;
        oTest.checked = false;
        oDR.checked = false;
        oPNC.checked = false;
        oDomain.value = "";
        oEnabled.checked = false;
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
        oFactoryCode = document.getElementById('<%=txtFactoryCode.ClientID%>');
        oForecast = document.getElementById('<%=chkForecast.ClientID%>');
        oWorkstationVMware = document.getElementById('<%=chkWorkstationVMware.ClientID%>');
        oProd = document.getElementById('<%=chkProd.ClientID%>');
        oQA = document.getElementById('<%=chkQA.ClientID%>');
        oTest = document.getElementById('<%=chkTest.ClientID%>');
        oDR = document.getElementById('<%=chkDR.ClientID%>');
        oPNC = document.getElementById('<%=chkPNC.ClientID%>');
        oDomain = document.getElementById('<%=txtDomain.ClientID%>');
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
		    <td><b>Asset Classes</b></td>
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
                                        <tr bgcolor="#EFEFEF" onmouseover="AdminMouseOver(this);" onmouseout="AdminMouseOut(this);" onclick="if(event.srcElement.tagName != 'INPUT')Edit('<%# DataBinder.Eval(Container.DataItem, "id") %>','<%# DataBinder.Eval(Container.DataItem, "name") %>','<%# DataBinder.Eval(Container.DataItem, "factory_code") %>','<%# DataBinder.Eval(Container.DataItem, "forecast") %>','<%# DataBinder.Eval(Container.DataItem, "workstation_vmware") %>','<%# DataBinder.Eval(Container.DataItem, "prod") %>','<%# DataBinder.Eval(Container.DataItem, "qa") %>','<%# DataBinder.Eval(Container.DataItem, "test") %>','<%# DataBinder.Eval(Container.DataItem, "dr") %>','<%# DataBinder.Eval(Container.DataItem, "pnc") %>','<%# DataBinder.Eval(Container.DataItem, "domain") %>','<%# DataBinder.Eval(Container.DataItem, "enabled") %>');" class="default">
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
                            <td class="default" width="100px">Name:</td>
                            <td><asp:textbox ID="txtName" CssClass="default" runat="server" Width="300" MaxLength="100"/></td>
                        </tr>
                        <tr> 
                            <td class="default" width="100px">Factory Code:</td>
                            <td><asp:textbox ID="txtFactoryCode" CssClass="default" runat="server" Width="50" MaxLength="1"/></td>
                        </tr>
                        <tr> 
                            <td class="default">Include in Forecast:</td>
                            <td><asp:CheckBox ID="chkForecast" runat="server" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Available for VMware Workstations:</td>
                            <td><asp:CheckBox ID="chkWorkstationVMware" runat="server" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Production Class:</td>
                            <td><asp:CheckBox ID="chkProd" runat="server" /></td>
                        </tr>
                        <tr> 
                            <td class="default">QA Class:</td>
                            <td><asp:CheckBox ID="chkQA" runat="server" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Test Class:</td>
                            <td><asp:CheckBox ID="chkTest" runat="server" /></td>
                        </tr>
                        <tr> 
                            <td class="default">DR:</td>
                            <td><asp:CheckBox ID="chkDR" runat="server" /></td>
                        </tr>
                        <tr> 
                            <td class="default">PNC:</td>
                            <td><asp:CheckBox ID="chkPNC" runat="server" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Domain:</td>
                            <td><asp:TextBox ID="txtDomain" runat="server" CssClass="default" Width="200" MaxLength="50" /></td>
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
                            <td><asp:Button ID="btnJoins" runat="server" Text="Change Joins" Width="150" CssClass="default" /></td>
                        </tr>
                        <tr> 
                            <td class="cmdefault">&nbsp;</td>
                            <td><asp:Button ID="btnEnvironments" runat="server" Text="Change Environments" Width="150" CssClass="default" /></td>
                        </tr>
                        <tr> 
                            <td class="cmdefault">&nbsp;</td>
                            <td><asp:Button ID="btnAP" runat="server" Text="Auto-Provisionings" Width="150" CssClass="default" /></td>
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
