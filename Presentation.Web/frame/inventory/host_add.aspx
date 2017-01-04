<%@ Page Language="C#" MasterPageFile="~/frame.Master" AutoEventWireup="true" CodeBehind="host_add.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.host_add" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" Runat="Server">
<script type="text/javascript">
    function EnableMaintenance() {
        var oClass = document.getElementById('<%=ddlClass.ClientID %>');
        var oMaintenance = document.getElementById('<%=ddlMaintenance.ClientID %>');
        if (oClass.options[oClass.selectedIndex].text.toUpperCase() == "PRODUCTION" || oClass.options[oClass.selectedIndex].text.toUpperCase() == "TEST") {
            oMaintenance.disabled = false;
            oMaintenance.options[0].text = "-- SELECT --";
        }
        else {
            oMaintenance.disabled = true;
            oMaintenance.options[0].text = "-- NONE --";
            oMaintenance.selectedIndex = 0;
        }
    }
    function EnsureMaintenance(oObject, strAlert) {
        var oClass = document.getElementById('<%=ddlClass.ClientID %>');
        if (oClass.options[oClass.selectedIndex].text.toUpperCase() == "PRODUCTION") {
            if (ValidateDropDown(oObject, strAlert) == false)
                return false;
        }
        return true;
    }
</script>
<table width="100%" cellpadding="4" cellspacing="0" border="0">
    <tr>
        <td colspan="2" class="header">Create a Host</td>
    </tr>
    <tr>
        <td nowrap>Location:</td>
        <td colspan="2">
            <table cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td><asp:DropDownList ID="ddlLocation" runat="server" CssClass="default" Width="300" AutoPostBack="true" OnSelectedIndexChanged="ddlLocation_Change" /></td>
                    <td class="bold">
                        <div id="divLocation" runat="server" style="display:none">
                            <img src="/images/spacer.gif" border="0" width="20" height="1" /><img src="/images/please_wait.gif" border="0" align="absmiddle" /> Please Wait...
                        </div>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td nowrap>Host Type:</td>
        <td width="100%"><asp:DropDownList ID="ddlHost" runat="server" CssClass="default" Width="300" /></td>
    </tr>
    <tr>
        <td nowrap>Nickname:</td>
        <td width="100%"><asp:TextBox ID="txtName" runat="server" CssClass="default" Width="300" MaxLength="50" /></td>
    </tr>
    <tr>
        <td nowrap>Class:<font class="required">&nbsp;*</font></td>
        <td width="100%"><asp:DropDownList ID="ddlClass" CssClass="default" runat="server" Width="300" /></td>
    </tr>
    <tr>
        <td nowrap>Environment:<font class="required">&nbsp;*</font></td>
        <td width="100%">
            <asp:DropDownList ID="ddlEnvironment" CssClass="default" runat="server" Width="300" Enabled="false" >
                <asp:ListItem Value="-- Please select a Class --" />
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td nowrap>Maintenance Window:<font class="required">&nbsp;*</font></td>
        <td width="100%"><asp:DropDownList ID="ddlMaintenance" CssClass="default" runat="server" Enabled="false" /></td>
    </tr>
    <tr>
        <td nowrap>Commitment Date:<font class="required">&nbsp;*</font></td>
        <td width="100%"><asp:TextBox ID="txtDate" runat="server" CssClass="default" Width="80" MaxLength="10" /> <asp:ImageButton ID="imgDate" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
    </tr>
    <tr>
        <td nowrap>Quantity:<font class="required">&nbsp;*</font></td>
        <td width="100%"><asp:TextBox ID="txtQuantity" runat="server" CssClass="default" Width="80" MaxLength="10" /></td>
    </tr>
    <tr>
        <td nowrap>Confidence Level:<font class="required">&nbsp;*</font></td>
        <td width="100%"><asp:DropDownList ID="ddlConfidence" runat="server" CssClass="default" /></td>
    </tr>
    <tr>
        <td nowrap>&nbsp;</td>
        <td width="100%">
            <asp:Button ID="btnAddHost" runat="server" CssClass="default" Width="125" Text="Order Host" OnClick="btnAddHost_Click" />
            <asp:Button ID="btnUpdateHost" runat="server" CssClass="default" Width="125" Text="Update Host" OnClick="btnUpdateHost_Click" />
        </td>
    </tr>
</table>
<input type="hidden" id="hdnEnvironment" runat="server" />
</asp:Content>
