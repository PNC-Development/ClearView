<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="asset_enclosures.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.asset_enclosures" %>

<script type="text/javascript">
    var oAdd = null;
    var oView = null;
    var oHidden = null;
    var oName = null;
    var oEnabled = null;
    function Edit(strId, strName, strClass, strEnvironment, strLocation, strRoom, strRack, strDR) {
        oAdd.style.display = "inline";
        oView.style.display = "none";
        oHidden.value = strId;
        oName.innerText = strName;
        oClass.innerText = strClass;
        oEnvironment.innerText = strEnvironment;
        oLocation.innerText = strLocation;
        oRoom.innerText = strRoom;
        oRack.innerText = strRack;
        oDR.selectedIndex = 0;
        for (var ii=0; ii<oDR.length; ii++) {
            if (oDR.options[ii].text == strDR)
                oDR.selectedIndex = ii;
        }
    }
    function Cancel() {
        oAdd.style.display = "none";
        oView.style.display = "inline";
        return false;
    }
    function Load() {
        oAdd = document.getElementById('<%=divAdd.ClientID%>');
        oView = document.getElementById('<%=divView.ClientID%>');
        oHidden = document.getElementById('<%=hdnId.ClientID%>');
        oName = document.getElementById('<%=lblName.ClientID%>');
        oClass = document.getElementById('<%=lblClass.ClientID%>');
        oEnvironment = document.getElementById('<%=lblEnvironment.ClientID%>');
        oLocation = document.getElementById('<%=lblLocation.ClientID%>');
        oRoom = document.getElementById('<%=lblRoom.ClientID%>');
        oRack = document.getElementById('<%=lblRack.ClientID%>');
        oDR = document.getElementById('<%=ddlDR.ClientID%>');
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
		    <td><b>Associate DR Enclosures</b></td>
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
                                                <td nowrap class="bold"><asp:linkbutton ID="lnkName" Text="Name (PROD)" CommandArgument="name" CssClass="bold" OnClick="OrderView" runat="server"/></td>
                                                <td nowrap class="bold">Name (DR)</td>
                                                <td nowrap class="bold">Class</td>
                                                <td nowrap class="bold">Environment</td>
                                                <td nowrap class="bold">Location</td>
                                                <td nowrap class="bold">Room</td>
                                                <td nowrap class="bold">Rack</td>
                                                <td nowrap class="bold"><asp:linkbutton ID="lnkModified" Text="Last Modified" CommandArgument="modified" CssClass="bold" OnClick="OrderView" runat="server"/></td>
                                            </tr>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr bgcolor="#EFEFEF" onmouseover="AdminMouseOver(this);" onmouseout="AdminMouseOut(this);" onclick="if(event.srcElement.tagName != 'INPUT')Edit('<%# DataBinder.Eval(Container.DataItem, "id") %>','<%# DataBinder.Eval(Container.DataItem, "name") %>','<%# DataBinder.Eval(Container.DataItem, "class") %>','<%# DataBinder.Eval(Container.DataItem, "environment") %>','<%# DataBinder.Eval(Container.DataItem, "location") %>','<%# DataBinder.Eval(Container.DataItem, "room") %>','<%# DataBinder.Eval(Container.DataItem, "rack") %>','<%# DataBinder.Eval(Container.DataItem, "drname") %>');" class="default">
                                            <td nowrap>&nbsp;<%# DataBinder.Eval(Container.DataItem, "name") %></td>
                                            <td nowrap>&nbsp;<%# DataBinder.Eval(Container.DataItem, "drname")%></td>
                                            <td nowrap>&nbsp;<%# DataBinder.Eval(Container.DataItem, "class") %></td>
                                            <td nowrap>&nbsp;<%# DataBinder.Eval(Container.DataItem, "environment") %></td>
                                            <td nowrap>&nbsp;<%# DataBinder.Eval(Container.DataItem, "location") %></td>
                                            <td nowrap>&nbsp;<%# DataBinder.Eval(Container.DataItem, "room") %></td>
                                            <td nowrap>&nbsp;<%# DataBinder.Eval(Container.DataItem, "rack") %></td>
                                            <td nowrap>&nbsp;<%# DataBinder.Eval(Container.DataItem, "modified") %></td>
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
                            <td><asp:Label ID="lblName" runat="server" CssClass="default" /></td>
                        </tr>
                        <tr> 
                            <td class="default" width="100px">DR Counterpart:</td>
                            <td><asp:dropdownlist ID="ddlDR" CssClass="default" runat="server"/></td>
                        </tr>
                        <tr> 
                            <td class="default" width="100px">Class:</td>
                            <td><asp:Label ID="lblClass" runat="server" CssClass="default" /></td>
                        </tr>
                        <tr> 
                            <td class="default" width="100px">Environment:</td>
                            <td><asp:Label ID="lblEnvironment" runat="server" CssClass="default" /></td>
                        </tr>
                        <tr> 
                            <td class="default" width="100px">Location:</td>
                            <td><asp:Label ID="lblLocation" runat="server" CssClass="default" /></td>
                        </tr>
                        <tr> 
                            <td class="default" width="100px">Room:</td>
                            <td><asp:Label ID="lblRoom" runat="server" CssClass="default" /></td>
                        </tr>
                        <tr> 
                            <td class="default" width="100px">Rack:</td>
                            <td><asp:Label ID="lblRack" runat="server" CssClass="default" /></td>
                        </tr>
                        <tr><td height="5" colspan="2">&nbsp;</td></tr>
                        <tr> 
                            <td>&nbsp;</td>
                            <td>
                                <asp:button ID="btnSave" CssClass="default" runat="server" Text="Save" Width="75" OnClick="btnSave_Click" />
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
</form>
</body>
</html>

