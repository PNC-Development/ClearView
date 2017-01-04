<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="platforms.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.platforms" %>


<script type="text/javascript">
    var oAdd = null;
    var oView = null;
    var oAddButton = null;
    var oDeleteButton = null;
    var oHidden = null;
    var oName = null;
    var oUser = null;
    var oUserId = null;
    var oManager = null;
    var oManagerId = null;
    var oImage = null;
    var oBigImage = null;
    var oAsset = null;
    var oForecast = null;
    var oSystem = null;
    var oInventory = null;
    var oAction = null;
    var oDemand = null;
    var oSupply = null;
    var oOrder = null;
    var oOrderView = null;
    var oAddForm = null;
    var oSettings = null;
    var oForm = null;
    var oAlert = null;
    var oMax1 = null;
    var oMax2 = null;
    var oMax3 = null;
    var oEnabled = null;
    function Edit(strId, strName, strUser, strUserId, strManager, strManagerId, strImage, strBigImage, strAsset, strForecast, strSystem, strInventory, strAction, strDemand, strSupply, strOrder,strOrderView, strAddForm, strSettings, strForm, strAlert, strMax1, strMax2, strMax3, strEnabled) {
        oAdd.style.display = "inline";
        oView.style.display = "none";
        oAddButton.value = "Update";
        oDeleteButton.style.display = "inline";
        oHidden.value = strId;
        oName.value = strName;
        oUser.value = unescape(strUser);
        oUserId.value = strUserId;
        oManager.value = unescape(strManager);
        oManagerId.value = strManagerId;
        oImage.value = strImage;
        oBigImage.value = strBigImage;
        oAsset.checked = (strAsset == "1");
        oForecast.checked = (strForecast == "1");
        oSystem.checked = (strSystem == "1");
        oInventory.checked = (strInventory == "1");
        oAction.value = strAction;
        oDemand.value = strDemand;
        oSupply.value = strSupply;
        oOrder.value = strOrder;
        oOrderView.value = strOrderView;
        oAddForm.value = strAddForm;
        oSettings.value = strSettings;
        oForm.value = strForm;
        oAlert.value = strAlert;
        oMax1.value = strMax1;
        oMax2.value = strMax2;
        oMax3.value = strMax3;
        oEnabled.checked = (strEnabled == "1");
    }
    function Add() {
        oAdd.style.display = "inline";
        oView.style.display = "none";
        oAddButton.value = "Add";
        oDeleteButton.style.display = "none";
        oHidden.value = "0";
        oName.value = "";
        oUser.value = "";
        oUserId.value = "0";
        oManager.value = "";
        oManagerId.value = "0";
        oImage.value = "";
        oBigImage.value = "";
        oAsset.checked = false;
        oForecast.checked = false;
        oSystem.checked = false;
        oInventory.checked = false;
        oAction.value = "";
        oDemand.value = "";
        oSupply.value = "";
        oOrder.value = "";
        oOrderView.value = "";
        oAddForm.value = "";
        oSettings.value = "";
        oForm.value = "";
        oAlert.value = "";
        oMax1.value = "100";
        oMax2.value = "0";
        oMax3.value = "0";
        oName.focus();
        oEnabled.checked = false;
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
        oUser = document.getElementById('<%=txtUser.ClientID%>');
        oUserId = document.getElementById('<%=hdnUser.ClientID%>');
        oManager = document.getElementById('<%=txtManager.ClientID%>');
        oManagerId = document.getElementById('<%=hdnManager.ClientID%>');
        oImage = document.getElementById('<%=txtImage.ClientID%>');
        oBigImage = document.getElementById('<%=txtBigImage.ClientID%>');
        oAsset = document.getElementById('<%=chkAsset.ClientID%>');
        oForecast = document.getElementById('<%=chkForecast.ClientID%>');
        oSystem = document.getElementById('<%=chkSystem.ClientID%>');
        oInventory = document.getElementById('<%=chkInventory.ClientID%>');
        oAction = document.getElementById('<%=txtAction.ClientID%>');
        oDemand = document.getElementById('<%=txtDemand.ClientID%>');
        oSupply = document.getElementById('<%=txtSupply.ClientID%>');
        oOrder = document.getElementById('<%=txtOrder.ClientID%>');
        oOrderView = document.getElementById('<%=txtOrderView.ClientID%>');
        oAddForm = document.getElementById('<%=txtAdd.ClientID%>');
        oSettings = document.getElementById('<%=txtSettings.ClientID%>');
        oForm = document.getElementById('<%=txtForm.ClientID%>');
        oAlert = document.getElementById('<%=txtAlert.ClientID%>');
        oMax1 = document.getElementById('<%=txtMax1.ClientID%>');
        oMax2 = document.getElementById('<%=txtMax2.ClientID%>');
        oMax3 = document.getElementById('<%=txtMax3.ClientID%>');
        oEnabled = document.getElementById('<%=chkEnabled.ClientID%>');
    }
</script>
<html>
<head>
<title>ClearView Administration</title>
<link rel="stylesheet" type="text/css" href="/css/default.css" />
<script type="text/javascript" src="/javascript/global.js"></script>
<script type="text/javascript" src="/javascript/ajax.js"></script>
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
		    <td><b>Platforms</b></td>
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
                                        <tr bgcolor="#EFEFEF" onmouseover="AdminMouseOver(this);" onmouseout="AdminMouseOut(this);" onclick="if(event.srcElement.tagName != 'INPUT')Edit('<%# DataBinder.Eval(Container.DataItem, "platformid") %>','<%# DataBinder.Eval(Container.DataItem, "name") %>','<%# Server.UrlEncode(oUser.GetFullName(Int32.Parse(DataBinder.Eval(Container.DataItem, "userid").ToString()))) %>','<%# DataBinder.Eval(Container.DataItem, "userid") %>','<%# Server.UrlEncode(oUser.GetFullName(Int32.Parse(DataBinder.Eval(Container.DataItem, "managerid").ToString()))) %>','<%# DataBinder.Eval(Container.DataItem, "managerid") %>','<%# DataBinder.Eval(Container.DataItem, "image") %>','<%# DataBinder.Eval(Container.DataItem, "big_image") %>','<%# DataBinder.Eval(Container.DataItem, "asset") %>','<%# DataBinder.Eval(Container.DataItem, "forecast") %>','<%# DataBinder.Eval(Container.DataItem, "system") %>','<%# DataBinder.Eval(Container.DataItem, "inventory") %>','<%# DataBinder.Eval(Container.DataItem, "action_form") %>','<%# DataBinder.Eval(Container.DataItem, "demand_form") %>','<%# DataBinder.Eval(Container.DataItem, "supply_form") %>','<%# DataBinder.Eval(Container.DataItem, "order_form") %>','<%# DataBinder.Eval(Container.DataItem, "order_view_form") %>','<%# DataBinder.Eval(Container.DataItem, "add_form") %>','<%# DataBinder.Eval(Container.DataItem, "settings_form") %>','<%# DataBinder.Eval(Container.DataItem, "forms_form") %>','<%# DataBinder.Eval(Container.DataItem, "alert_form") %>','<%# DataBinder.Eval(Container.DataItem, "max_inventory1") %>','<%# DataBinder.Eval(Container.DataItem, "max_inventory2") %>','<%# DataBinder.Eval(Container.DataItem, "max_inventory3") %>','<%# DataBinder.Eval(Container.DataItem, "enabled") %>');" class="default">
                                            <td width="5%" align="left">&nbsp;<asp:ImageButton ID="btnDelete" OnClick="btnDeleteLink_Click" runat="server" ToolTip="Delete" ImageAlign="AbsMiddle" ImageUrl="/admin/images/delete.gif" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "platformid") %>' /></td>
                                            <td width="50%">&nbsp;<%# DataBinder.Eval(Container.DataItem, "name") %></td>
                                            <td width="50%">&nbsp;<%# DataBinder.Eval(Container.DataItem, "modified") %></td>
                                            <td align="center"><asp:ImageButton ID="btnEnable" runat="server" ImageAlign="AbsMiddle" ImageUrl='<%# (DataBinder.Eval(Container.DataItem, "enabled").ToString() == "1") ? "/admin/images/enabled.gif" : "/admin/images/disabled.gif" %>' CommandArgument='<%# DataBinder.Eval(Container.DataItem, "platformid") %>' OnClick="btnEnable_Click" /></td>
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
                            <td class="default">User:</td>
                            <td>
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
                            </td>
                        </tr>
                        <tr> 
                            <td class="default">Manager:</td>
                            <td>
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <tr>
                                        <td><asp:TextBox ID="txtManager" runat="server" Width="300" CssClass="default" /></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div id="divManager" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                                                <asp:ListBox ID="lstManager" runat="server" CssClass="default" />
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr> 
                            <td class="default">Image:</td>
                            <td><asp:textbox ID="txtImage" CssClass="default" runat="server" Width="300" MaxLength="100"/> <asp:Button ID="btnImage" runat="server" CssClass="default" Text="..." /></td>
                        </tr>
                        <tr> 
                            <td class="default">Big Image:</td>
                            <td><asp:textbox ID="txtBigImage" CssClass="default" runat="server" Width="300" MaxLength="100"/> <asp:Button ID="btnBigImage" runat="server" CssClass="default" Text="..." /></td>
                        </tr>
                        <tr> 
                            <td class="default">Include in System:</td>
                            <td><asp:CheckBox ID="chkSystem" runat="server" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Include in Forecast:</td>
                            <td><asp:CheckBox ID="chkForecast" runat="server" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Include in Inventory:</td>
                            <td><asp:CheckBox ID="chkInventory" runat="server" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Include in Asset:</td>
                            <td><asp:CheckBox ID="chkAsset" runat="server" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Action Form:</td>
                            <td><asp:textbox ID="txtAction" CssClass="default" runat="server" Width="400" MaxLength="100"/> <asp:button ID="btnAction" runat="server" Text="..." Width="25" CssClass="default"/></td>
                        </tr>
                        <tr> 
                            <td class="default">Demand Form:</td>
                            <td><asp:textbox ID="txtDemand" CssClass="default" runat="server" Width="400" MaxLength="100"/> <asp:button ID="btnDemand" runat="server" Text="..." Width="25" CssClass="default"/></td>
                        </tr>
                        <tr> 
                            <td class="default">Supply Form:</td>
                            <td><asp:textbox ID="txtSupply" CssClass="default" runat="server" Width="400" MaxLength="100"/> <asp:button ID="btnSupply" runat="server" Text="..." Width="25" CssClass="default"/></td>
                        </tr>
                        <tr> 
                            <td class="default">Order Form:</td>
                            <td><asp:textbox ID="txtOrder" CssClass="default" runat="server" Width="400" MaxLength="100"/> <asp:button ID="btnOrder" runat="server" Text="..." Width="25" CssClass="default"/></td>
                        </tr>
                         <tr> 
                            <td class="default">Order View Form:</td>
                            <td><asp:textbox ID="txtOrderView" CssClass="default" runat="server" Width="400" MaxLength="100"/> <asp:button ID="btnOrderView" runat="server" Text="..." Width="25" CssClass="default"/></td>
                        </tr>
                        <tr> 
                            <td class="default">Add Form:</td>
                            <td><asp:textbox ID="txtAdd" CssClass="default" runat="server" Width="400" MaxLength="100"/> <asp:button ID="btnAddForm" runat="server" Text="..." Width="25" CssClass="default"/></td>
                        </tr>
                        <tr> 
                            <td class="default">Settings Form:</td>
                            <td><asp:textbox ID="txtSettings" CssClass="default" runat="server" Width="400" MaxLength="100"/> <asp:button ID="btnSettings" runat="server" Text="..." Width="25" CssClass="default"/></td>
                        </tr>
                        <tr> 
                            <td class="default">Forms Form:</td>
                            <td><asp:textbox ID="txtForm" CssClass="default" runat="server" Width="400" MaxLength="100"/> <asp:button ID="btnForm" runat="server" Text="..." Width="25" CssClass="default"/></td>
                        </tr>
                        <tr> 
                            <td class="default">Alert Form:</td>
                            <td><asp:textbox ID="txtAlert" CssClass="default" runat="server" Width="400" MaxLength="100"/> <asp:button ID="btnAlert" runat="server" Text="..." Width="25" CssClass="default"/></td>
                        </tr>
                        <tr> 
                            <td class="default">Max Inventory 1:</td>
                            <td><asp:textbox ID="txtMax1" CssClass="default" runat="server" Width="100" MaxLength="5"/></td>
                        </tr>
                        <tr> 
                            <td class="default">Max Inventory 2:</td>
                            <td><asp:textbox ID="txtMax2" CssClass="default" runat="server" Width="100" MaxLength="5"/></td>
                        </tr>
                        <tr> 
                            <td class="default">Max Inventory 3:</td>
                            <td><asp:textbox ID="txtMax3" CssClass="default" runat="server" Width="100" MaxLength="5"/></td>
                        </tr>
                        <tr> 
                            <td class="default">Enabled:</td>
                            <td><asp:CheckBox ID="chkEnabled" runat="server" Checked="true" /></td>
                        </tr>
                        <tr> 
                            <td class="cmdefault">&nbsp;</td>
                            <td><asp:Button ID="btnUsers" runat="server" Text="Edit User Rights" Width="150" CssClass="default" /></td>
                        </tr>
                        <tr> 
                            <td class="cmdefault">&nbsp;</td>
                            <td><asp:Button ID="btnDisplay" runat="server" Text="Change Order" Width="150" CssClass="default" /></td>
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
    <asp:HiddenField ID="hdnUser" runat="server" />
    <asp:HiddenField ID="hdnManager" runat="server" />
</form>
</body>
</html>
