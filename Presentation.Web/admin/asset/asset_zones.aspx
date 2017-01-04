<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="asset_zones.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.asset_zones" %>

<script type="text/javascript">
    var oAdd = null;
    var oView = null;
    var oAddButton = null;
    var oDeleteButton = null;
    var oHidden = null;
    var oName = null;
    var oSelectedLocation=null;
    var oSelectedLocationId=null;
    var oSelectedRoom=null;
    var oSelectedRoomId=null;
    var oLocation=null;
    var oRoom=null;
    var oVLan=null;
    var oDescription=null;
    var oEnabled = null;
    
    function Load() 
    {
        oAdd = document.getElementById('<%=divAdd.ClientID%>');
        oView = document.getElementById('<%=divView.ClientID%>');
        oAddButton = document.getElementById('<%=btnAdd.ClientID%>');
        oDeleteButton = document.getElementById('<%=btnDelete.ClientID%>');
        oHidden = document.getElementById('<%=hdnId.ClientID%>');
        oName = document.getElementById('<%=txtName.ClientID%>');
        
        oSelectedLocation = document.getElementById('<%=txtLocation.ClientID%>');
        oSelectedLocationId = document.getElementById('<%=hdnLocationId.ClientID%>');
        oSelectedRoomId = document.getElementById('<%=hdnRoomId.ClientID%>');
        oSelectedRoom = document.getElementById('<%=txtRoom.ClientID%>');
        oLocation = document.getElementById('<%=lblLocation.ClientID%>');
        oRoom = document.getElementById('<%=lblRoom.ClientID%>');
        oVLan=document.getElementById('<%=txtVLan.ClientID%>');
        oDescription = document.getElementById('<%=txtDescription.ClientID%>');
        oEnabled = document.getElementById('<%=chkEnabled.ClientID%>');
    }
    
    function Edit(strId, strName,strvLan, strDescription, strEnabled) {
        oAdd.style.display = "inline";
        oView.style.display = "none";
        oAddButton.value = "Update";
        oDeleteButton.style.display = "inline";
        oHidden.value = strId;
        oName.value = strName;
        oSelectedLocation = document.getElementById('<%=txtLocation.ClientID%>');
        oLocation.innerText = oSelectedLocation.value;
        oSelectedRoom = document.getElementById('<%=txtRoom.ClientID%>');
        oRoom.innerText =oSelectedRoom.value;
        oVLan.value=strvLan;
        oDescription.value = strDescription;
        oEnabled.checked = (strEnabled == "1");
    }
    function Add() 
    {
        if (ValidateHidden0('<%=hdnRoomId.ClientID %>','<%=btnSelectLocation.ClientID %>','Please select location')==false) 
        return false;
       else
       {
    
            oAdd.style.display = "inline";
            oView.style.display = "none";
            oAddButton.value = "Add";
            oDeleteButton.style.display = "none";
            oHidden.value = "0";
            oName.value = "";
            
            oSelectedLocation = document.getElementById('<%=txtLocation.ClientID%>');
            oLocation.innerText = oSelectedLocation.value;
            oSelectedRoom = document.getElementById('<%=txtRoom.ClientID%>');
            oRoom.innerText =oSelectedRoom.value;
            oVLan.value="0";
            oDescription.value ="";
            oEnabled.checked = false;
            oName.focus();
        }
    }
    function Cancel() {
        oAdd.style.display = "none";
        oView.style.display = "inline";
        return false;
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
		        <td colspan="2"><b>Asset Zones</b></td>
		    </tr>
		    <tr><td colspan="2">&nbsp;</td></tr>
		    <tr><td colspan="2">
		        		        <table id="tblLocation" width="100%" cellpadding="0" cellspacing="4" border="0" bgcolor="#f9f9f9" align="center">
                    <tr><td nowrap class="bold">Select Location</td>
                        <td width="100%" class="default"></td>
                    </tr>
		            <tr> 
                        <td nowrap class="bold">Location</td>
                        <td width="100%">
                            <asp:TextBox ID="txtLocation" CssClass="lightdefault" runat="server"  Width="500" ReadOnly="true" />
                            <input type="hidden" id="hdnLocationId" runat="server" />
                             &nbsp<asp:Button ID="btnSelectLocation" runat="server" Text="..." CssClass="default"  Width="25" OnClick="btnSelectLocation_Click"  />
                        </td>
                    </tr>
                    <tr> 
                        <td nowrap class="bold">Room</td>
                        <td width="100%">
                            <asp:TextBox ID="txtRoom" CssClass="lightdefault" runat="server"  Width="250" ReadOnly="true" />
                            <input type="hidden" id="hdnRoomId" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="bold"></td>
                        <td width="100%"></td>
                    </tr>

		        </table>

		    </td></tr>
		    <tr><td colspan="2"><hr /></td></tr>
		    
            <tr> 
                <td nowrap>                
                <asp:Button ID="btnGetZones" runat="server" Text="Show Zones" CssClass="default"  Width="150" OnClick="btnGetZones_Click" />
                </td>
                <td align="right"><a href="javascript:void(0);" onclick="Add();" class="cmlink" title="Click to Add New">Add New</a> &nbsp&nbsp</td>
            </tr>
		    <tr><td colspan="2">&nbsp</td></tr>
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
                                                <td width="20%" class="bold"><asp:linkbutton ID="lnkName" Text="Name" CommandArgument="Rack" CssClass="bold" OnClick="OrderView" runat="server"/></td>
                                                <td width="30%" class="bold"><asp:linkbutton ID="lnkRoom" Text="Room" CommandArgument="Room" CssClass="bold" OnClick="OrderView" runat="server"/></td>
                                                <td width="30%" class="bold"><asp:linkbutton ID="lnkDescription" Text="Description" CommandArgument="Description" CssClass="bold" OnClick="OrderView" runat="server"/></td>
                                                <td class="bold"><asp:linkbutton ID="lnkModified" Text="Last Modified" CommandArgument="modified" CssClass="bold" OnClick="OrderView" runat="server"/></td>
                                                <td class="bold" align="center"><asp:linkbutton ID="lnkEnabled" Text="Enabled" CommandArgument="enabled" CssClass="bold" OnClick="OrderView" runat="server"/></td>
                                            </tr>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr bgcolor="#EFEFEF" onmouseover="AdminMouseOver(this);" onmouseout="AdminMouseOut(this);" onclick="if(event.srcElement.tagName != 'INPUT')Edit('<%# DataBinder.Eval(Container.DataItem, "ZoneId") %>','<%# DataBinder.Eval(Container.DataItem, "Zone") %>','<%# DataBinder.Eval(Container.DataItem, "vLan") %>','<%# DataBinder.Eval(Container.DataItem, "description") %>','<%# DataBinder.Eval(Container.DataItem, "enabled") %>');" class="default">
                                            <td width="5%" align="left">&nbsp;<asp:ImageButton ID="btnDelete" OnClick="btnDeleteLink_Click" runat="server" ToolTip="Delete" ImageAlign="AbsMiddle" ImageUrl="/admin/images/delete.gif" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ZoneId") %>' /></td>
                                            <td >&nbsp;<%# DataBinder.Eval(Container.DataItem, "Zone") %></td>
                                            <td >&nbsp;<%# DataBinder.Eval(Container.DataItem, "Room") %></td>
                                            <td >&nbsp;<%# DataBinder.Eval(Container.DataItem, "Description") %></td>
                                            <td nowrap>&nbsp;<%# DataBinder.Eval(Container.DataItem, "modified") %></td>
                                            <td align="center"><asp:ImageButton ID="btnEnable" runat="server" ImageAlign="AbsMiddle" ImageUrl='<%# (DataBinder.Eval(Container.DataItem, "enabled").ToString() == "1") ? "/admin/images/enabled.gif" : "/admin/images/disabled.gif" %>' CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ZoneId") %>' OnClick="btnEnable_Click" /></td>
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
                            <td class="default">Location:</td>
                            <td width="100%">
                                <asp:label ID="lblLocation" CssClass="default" runat="server" />&nbsp;&nbsp;
                            </td>
                        </tr>
                        <tr> 
                            <td class="default">Room:</td>
                            <td width="100%">
                                <asp:label ID="lblRoom" CssClass="default" runat="server" />&nbsp;&nbsp;
                            </td>
                        </tr>
                        <tr> 
                            <td class="default" width="100px">vLAN:</td>
                            <td><asp:textbox ID="txtVLan" CssClass="default" runat="server" Width="50" MaxLength="20"/></td>
                        </tr>
                        <tr> 
                            <td class="default" width="100px">Description:</td>
                            <td><asp:textbox ID="txtDescription" CssClass="default" runat="server" Width="500" MaxLength="450"/></td>
                        </tr>
                        <tr> 
                            <td class="default">Enabled:</td>
                            <td><asp:CheckBox ID="chkEnabled" runat="server" Checked="true" /></td>
                        </tr>
                        <tr style="display :none">  
                            <td class="cmdefault">&nbsp;</td>
                            <td><asp:Button ID="btnOrder" runat="server" Text="Change Order" Width="150" CssClass="default" /></td>
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
    <input type="hidden" id="hdnAssetId" runat="server" value="" />
    
</form>
</body>
</html>
