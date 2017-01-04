<%@ Page Language="C#" MasterPageFile="~/frame.Master" AutoEventWireup="true" CodeBehind="locations.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.locations" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" runat="server">
<script type="text/javascript">
    function Select(strValue, oHidden, strName, oName) {
        oHidden = document.getElementById(oHidden);
        oName = document.getElementById(oName);
        oHidden.value = strValue;
        oName.innerText = strName;
    }
    function Reset(strValue, oHidden, strName, oName) {
        oHidden = document.getElementById(oHidden);
        oName = document.getElementById(oName);
        oHidden.value = strValue;
        oName.innerText = strName;
        return false;
    }
    function Update(oHidden, strControl, oName, strControlText) {
        oHidden = document.getElementById(oHidden);
        oName = document.getElementById(oName);
        window.parent.UpdateLocation(oHidden.value, strControl, oName.value, strControlText);
        window.parent.HidePanel();
        return false;
    }
</script>
<div id="divTree" runat="server" style="height:100%; width:100%; overflow:auto;">
<table width="100%" height="100%" cellpadding="2" cellspacing="0" border="0">
    <tr height="1">
        <td colspan="2">Location:&nbsp;<asp:TextBox ID="txtName" runat="server" CssClass="error" Width="80%" ReadOnly="true" /></td>
    </tr>
    <tr>
        <td colspan="2" valign="top" class="default">
            <div style="height:100%; overflow:auto; background-color:#FFFFFF">
            <asp:TreeView ID="oTreeview" runat="server" ShowLines="true" NodeIndent="10">
                <NodeStyle CssClass="default" />
            </asp:TreeView>
            </div>
        </td>
    </tr>
    <tr height="1">
        <td colspan="2"><hr size="1" noshade /></td>
    </tr>
    <tr height="1">
        <td><asp:Button ID="btnNew" runat="server" Text="New Location" Width="125" CssClass="default" OnClick="btnNew_Click" /></td>
        <td align="right"><asp:Button ID="btnSave" runat="server" Text="OK" Width="75" CssClass="default" /> <asp:Button ID="btnClose" runat="server" Text="Cancel" Width="75" CssClass="default" /></td>
    </tr>
</table>
</div>
<asp:Panel ID="panNew" runat="server" Visible="false">
    <table width="100%" height="100%" cellpadding="2" cellspacing="0" border="0">
        <tr height="1">
            <td class="frame" nowrap>&nbsp;Location Browser</td>
            <td class="frame" align="right"><a href="javascript:void(0);" onclick="parent.HidePanel();"><img src="/images/close.gif" border="0" title="Close"></a></td>
        </tr>
        <tr height="1">
            <td colspan="2">&nbsp;</td>
        </tr>
        <tr height="1">
            <td colspan="2">Please complete the following fields to create a new location.</td>
        </tr>
        <tr height="1">
            <td colspan="2">&nbsp;</td>
        </tr>
        <tr height="1">
            <td>State:</td>
            <td><asp:TextBox ID="txtState" runat="server" CssClass="default" Width="50" MaxLength="2" /></td>
        </tr>
        <tr height="1">
            <td></td>
            <td class="footer">(Example: OH)</td>
        </tr>
        <tr height="1">
            <td>City:</td>
            <td><asp:TextBox ID="txtCity" runat="server" CssClass="default" Width="100" MaxLength="50" /></td>
        </tr>
        <tr height="1">
            <td></td>
            <td class="footer">(Example: Cleveland)</td>
        </tr>
        <tr height="1">
            <td>Address:</td>
            <td><asp:TextBox ID="txtAddress" runat="server" CssClass="default" Width="200" MaxLength="50" /></td>
        </tr>
        <tr>
            <td></td>
            <td class="footer" valign="top">(Example: 4100 West 150th Street)</td>
        </tr>
        <tr height="1">
            <td colspan="2"><hr size="1" noshade /></td>
        </tr>
        <tr height="1">
            <td colspan="2" align="right"><asp:Button ID="btnSaveNew" runat="server" Text="OK" Width="75" CssClass="default" OnClick="btnSaveNew_Click" /> <asp:Button ID="btnCancel" runat="server" Text="Cancel" Width="75" CssClass="default" OnClick="btnCancel_Click" /></td>
        </tr>
    </table>
</asp:Panel>
<asp:Label ID="lblId" runat="server" Visible="false" />
<input type="hidden" runat="server" id="hdnId" />
</asp:Content>
