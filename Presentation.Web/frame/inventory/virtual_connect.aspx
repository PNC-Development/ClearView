<%@ Page Language="C#" MasterPageFile="~/frame.Master" AutoEventWireup="true" CodeBehind="virtual_connect.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.virtual_connect" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" Runat="Server">
<script type="text/javascript">
    function MoveOrderUp(oList, oHidden) {
	    if (oList.selectedIndex > -1) {
		    var oldIndex = oList.selectedIndex - 1;
		    if (oldIndex > -1) {

			    var oldValue = oList.options[oList.selectedIndex].value;
			    var oldText = oList.options[oList.selectedIndex].text;
			    oList.options[oList.selectedIndex].value = oList.options[oList.selectedIndex - 1].value;
			    oList.options[oList.selectedIndex].text = oList.options[oList.selectedIndex - 1].text;
			    oList.options[oList.selectedIndex - 1].value = oldValue;
			    oList.options[oList.selectedIndex - 1].text = oldText;
			    oList.selectedIndex = oldIndex;
			    UpdateOrder(oList, oHidden);
		    }
	    }
	    return false;
    }
    function MoveOrderDown(oList, oHidden) {
	    if (oList.selectedIndex > -1) {
		    var oldIndex = oList.selectedIndex + 1;
		    if (oldIndex < oList.length) {
			    var oldValue = oList.options[oList.selectedIndex].value;
			    var oldText = oList.options[oList.selectedIndex].text;
			    oList.options[oList.selectedIndex].value = oList.options[oList.selectedIndex + 1].value;
			    oList.options[oList.selectedIndex].text = oList.options[oList.selectedIndex + 1].text;
			    oList.options[oList.selectedIndex + 1].value = oldValue;
			    oList.options[oList.selectedIndex + 1].text = oldText;
			    oList.selectedIndex = oldIndex;
			    UpdateOrder(oList, oHidden);
		    }
	    }
	    return false;
    }
    function UpdateOrder(oList, oHidden) {
	    oHidden = document.getElementById(oHidden);
	    oHidden.value = "";
	    for (var ii=0; ii<oList.length; ii++)
		    oHidden.value = oHidden.value + oList.options[ii].value + "&";
    }
    function MoveOut(oList) {
	    if (oList.selectedIndex > -1) {
	        window.navigate('<%=Request.Path %>' + '?id=<%=Request.QueryString["id"] %>' + '&did=' + oList.options[oList.selectedIndex].value);
	    }
        return false;
    }
    function EditVC(oList) {
	    if (oList.selectedIndex > -1) {
	        window.navigate('<%=Request.Path %>' + '?id=<%=Request.QueryString["id"] %>' + '&edit=' + oList.options[oList.selectedIndex].value);
	    }
        return false;
    }
</script>
<table width="100%" cellpadding="4" cellspacing="3" border="0">
    <tr>
        <td colspan="2" align="center"><a href="javascript:void(0);" onclick="parent.HidePanel();"><img src='/images/close-icon.gif' border='0' align='absmiddle' /><img src='/images/spacer.gif' border='0' width='5' height='1' />Close Window</a></td>
    </tr>
    <tr>
        <td nowrap>Tracking Number:</td>
        <td width="100%"><asp:Label ID="lblTracking" runat="server" CssClass="default" /></td>
    </tr>
    <tr>
        <td nowrap>Serial Number:</td>
        <td width="100%"><asp:Label ID="lblSerial" runat="server" CssClass="default" /></td>
    </tr>
    <tr>
        <td nowrap>Asset Tag:</td>
        <td width="100%"><asp:Label ID="lblAsset" runat="server" CssClass="default" /></td>
    </tr>
    <tr>
        <td nowrap>Model:</td>
        <td width="100%"><asp:Label ID="lblModel" runat="server" CssClass="default" /></td>
    </tr>
    <tr>
        <td colspan="2">&nbsp;</td>
    </tr>
    <tr>
        <td colspan="2" class="bold">Add a Virtual Connect</td>
    </tr>
    <tr>
        <td nowrap>Virtual Connect IP:</td>
        <td width="100%"><asp:TextBox ID="txtVirtualConnect" runat="server" CssClass="default" Width="200" MaxLength="15" /></td>
    </tr>
    <tr>
        <td nowrap></td>
        <td width="100%"><asp:Button ID="btnAdd" runat="server" CssClass="default" Width="75" Text="Add" OnClick="btnAdd_Click" /> <asp:Button ID="btnCancel" runat="server" CssClass="default" Width="75" Text="Cancel" OnClick="btnCancel_Click" Visible="false" /></td>
    </tr>
    <tr>
        <td colspan="2" class="bold">Current Virtual Connect IPs</td>
    </tr>
    <tr>
        <td colspan="2">
            <table cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td class="default"><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                    <td colspan="3"><asp:ListBox ID="lstCurrent" runat="server" Width="350" CssClass="default" Rows="10" /></td>
                </tr>
                <tr>
                    <td class="default"><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                    <td class="default" align="right"><asp:ImageButton ID="imgOrderUp" runat="server" ImageUrl="/images/up.gif" ToolTip="Move Up"/></td>
                    <td class="default" align="center"><asp:ImageButton ID="imgRemove" runat="server" ImageUrl="/images/dl.gif" ToolTip="Remove" /></td>
                    <td class="default"><asp:ImageButton ID="imgOrderDown" runat="server" ImageUrl="/images/dn.gif" ToolTip="Move Down"/></td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td nowrap></td>
        <td width="100%"><asp:Button ID="btnSave" runat="server" CssClass="default" Width="100" Text="Save Order" OnClick="btnSave_Click" /></td>
    </tr>
</table>
<input type="hidden" runat="server" id="hdnOrder" />
</asp:Content>
