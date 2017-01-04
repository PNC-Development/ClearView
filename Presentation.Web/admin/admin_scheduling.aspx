<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="admin_scheduling.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.admin_scheduling" %>

<script type="text/javascript">
    var oAdd = null;
    var oView = null;
    var oAddButton = null;
    var oDeleteButton = null;
    var oHidden = null;
    var oName = null;
    var oFac = null;
    var oNet = null;
    var oConf = null;
    var oPass = null;
    var oDate = null;
    var oStart = null;
    var oEnd = null;
    var oMax = null;
    var oLocation = null;
    function Edit(strId, strName, strFac, strNet, strConf, strPass, strDate, strStart, strEnd, strMax, strLocation) {
        oAdd.style.display = "inline";
        oView.style.display = "none";
        oAddButton.value = "Update";
        oDeleteButton.style.display = "inline";
        oHidden.value = strId;
        oName.value = strName;
        oFac.value = strFac;
        oNet.value = strNet;
        oConf.value = strConf;
        oPass.value = strPass;
        oDate.value = strDate;
        oStart.value = strStart;
        oEnd.value = strEnd;
        oMax.value = strMax;
        oLocation.value = strLocation;
    }
    function Add() {
        oAdd.style.display = "inline";
        oView.style.display = "none";
        oAddButton.value = "Add";
        oDeleteButton.style.display = "none";
        oHidden.value = "0";
        oName.value = "";
        oFac.value = "";
        oNet.value = "";
        oConf.value = "";
        oPass.value = "";
        oDate.value = "";
        oStart.value = "";
        oEnd.value = "";
        oMax.value = "";
        oLocation.value = "";
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
        oFac = document.getElementById('<%=txtFac.ClientID%>');
        oNet = document.getElementById('<%=txtNet.ClientID%>');
        oConf = document.getElementById('<%=txtConf.ClientID%>');
        oPass = document.getElementById('<%=txtPass.ClientID%>');
        oDate = document.getElementById('<%=txtDate.ClientID%>');
        oStart = document.getElementById('<%=txtStart.ClientID%>');
        oEnd = document.getElementById('<%=txtEnd.ClientID%>');
        oMax = document.getElementById('<%=txtMax.ClientID%>');
        oLocation = document.getElementById('<%=txtLocation.ClientID%>');
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
		    <td><b>Scheduling</b></td>
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
                                                <td width="33%" class="bold"><asp:linkbutton ID="lnkName" Text="Name" CommandArgument="event" CssClass="bold" OnClick="OrderView" runat="server"/></td>
                                                <td width="33%" class="bold"><asp:linkbutton ID="lnkHappens" Text="Date" CommandArgument="date_sch" CssClass="bold" OnClick="OrderView" runat="server"/></td>
                                                <td width="33%" class="bold"><asp:linkbutton ID="lnkModified" Text="Last Modified" CommandArgument="modified" CssClass="bold" OnClick="OrderView" runat="server"/></td>
                                            </tr>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr bgcolor="#EFEFEF" onmouseover="AdminMouseOver(this);" onmouseout="AdminMouseOut(this);" onclick="if(event.srcElement.tagName != 'INPUT')Edit('<%# DataBinder.Eval(Container.DataItem, "schd_id") %>','<%# DataBinder.Eval(Container.DataItem, "event") %>','<%# DataBinder.Eval(Container.DataItem, "facilitator") %>','<%# DataBinder.Eval(Container.DataItem, "netmeeting") %>','<%# DataBinder.Eval(Container.DataItem, "confline") %>','<%# DataBinder.Eval(Container.DataItem, "passcode") %>','<%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "date_sch").ToString()).ToShortDateString() %>','<%# DataBinder.Eval(Container.DataItem, "start_time") %>','<%# DataBinder.Eval(Container.DataItem, "end_time") %>','<%# DataBinder.Eval(Container.DataItem, "max_people") %>','<%# DataBinder.Eval(Container.DataItem, "location") %>');" class="default">
                                            <td width="5%" align="left">&nbsp;<asp:ImageButton ID="btnDelete" OnClick="btnDeleteLink_Click" runat="server" ToolTip="Delete" ImageAlign="AbsMiddle" ImageUrl="/admin/images/delete.gif" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "schd_id") %>' /></td>
                                            <td width="33%">&nbsp;<%# DataBinder.Eval(Container.DataItem, "event") %></td>
                                            <td width="33%">&nbsp;<%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "date_sch").ToString()).ToShortDateString()%></td>
                                            <td width="33%">&nbsp;<%# DataBinder.Eval(Container.DataItem, "modified") %></td>
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
                            <td><asp:textbox ID="txtName" CssClass="default" runat="server" Width="300" MaxLength="50"/></td>
                        </tr>
                        <tr> 
                            <td class="default" width="100px">Facilitator:</td>
                            <td><asp:textbox ID="txtFac" CssClass="default" runat="server" Width="300" MaxLength="50"/></td>
                        </tr>
                        <tr> 
                            <td class="default" width="100px">NetMeeting:</td>
                            <td><asp:textbox ID="txtNet" CssClass="default" runat="server" Width="300" MaxLength="30"/></td>
                        </tr>
                        <tr> 
                            <td class="default" width="100px">Conference Line:</td>
                            <td><asp:textbox ID="txtConf" CssClass="default" runat="server" Width="300" MaxLength="20"/></td>
                        </tr>
                        <tr> 
                            <td class="default" width="100px">Pass Code:</td>
                            <td><asp:textbox ID="txtPass" CssClass="default" runat="server" Width="300" MaxLength="10"/></td>
                        </tr>
                        <tr> 
                            <td class="default" width="100px">Date:</td>
                            <td><asp:textbox ID="txtDate" CssClass="default" runat="server" Width="100" MaxLength="10"/></td>
                        </tr>
                        <tr> 
                            <td class="default" width="100px">Start Time:</td>
                            <td><asp:textbox ID="txtStart" CssClass="default" runat="server" Width="100" MaxLength="15"/></td>
                        </tr>
                        <tr> 
                            <td class="default" width="100px">End Time:</td>
                            <td><asp:textbox ID="txtEnd" CssClass="default" runat="server" Width="100" MaxLength="15"/></td>
                        </tr>
                        <tr> 
                            <td class="default" width="100px">Max Registrations:</td>
                            <td><asp:textbox ID="txtMax" CssClass="default" runat="server" Width="100" MaxLength="10"/></td>
                        </tr>
                        <tr> 
                            <td class="default" width="100px">Location:</td>
                            <td><asp:textbox ID="txtLocation" CssClass="default" runat="server" Width="300" MaxLength="50"/></td>
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
</form>
</body>
</html>
