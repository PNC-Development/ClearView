<%@ Page Language="C#" MasterPageFile="~/frame.Master" AutoEventWireup="true" CodeBehind="new_user.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.new_user" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" Runat="Server">
<script type="text/javascript">
    function ADCheck(oCheck, strUserName, strPNC, oFname, oLname, oHidden) {
        oHidden = document.getElementById(oHidden);
        oFname = document.getElementById(oFname);
        oLname = document.getElementById(oLname);
        if (oCheck.checked == true && oFname.value == "") {
            oCheck.checked = false;
            alert('Please enter a first name');
            oFname.focus();
        }
        if (oCheck.checked == true && oLname.value == "") {
            oCheck.checked = false;
            alert('Please enter a last name');
            oLname.focus();
        }
        if (oCheck.checked == true) {
            oFname.disabled = true;
            oLname.disabled = true;
            oHidden.value += strUserName + "&" + strPNC + "&" + oFname.value + "&" + oLname.value + "&&";
        }
        else {
            oFname.disabled = false;
            oLname.disabled = false;
            var intFound = oHidden.value.indexOf(strUserName);
            var strBefore = oHidden.value.substring(0, intFound);
            var strAfter = oHidden.value;
            strAfter = oHidden.value.substring(oHidden.value.indexOf("&&", intFound) + 2);
            oHidden.value = strBefore + strAfter;
        }
    }
</script>
<table width="100%" cellpadding="0" cellspacing="5" border="0">
    <tr>
        <td rowspan="2"><img src="/images/user_add.gif" border="0" align="absmiddle" /></td>
        <td class="header" width="100%" valign="bottom">Register a New User</td>
    </tr>
    <tr>
        <td width="100%" valign="top">If a person does not exist in ClearView, use this page to register the user.</td>
    </tr>
</table>
<table width="100%" cellpadding="5" cellspacing="3" border="0">
    <tr>
        <td>Please enter the LAN ID of the user you want to add to ClearView...</td>
    </tr>
    <tr>
        <td class="footer"><b>NOTE:</b> The LAN ID can be either an X-ID (legacy National City) or a PNC ID (P-ID, XX-ID, etc.).</td>
    </tr>
    <tr>
        <td><asp:TextBox ID="txtXID" runat="server" CssClass="default" Width="200" /></td>
    </tr>
    <tr>
        <td><asp:Button ID="btnSubmit" runat="server" CssClass="default" Width="75" Text="Submit" OnClick="btnSubmit_Click" /></td>
    </tr>
    <tr>
        <td>&nbsp;</td>
    </tr>
    <tr>
        <td><asp:Label ID="lblResult" runat="server" CssClass="error" /></td>
    </tr>
    <tr>
        <td valign="top">
            <div style="width:100%; height:150; overflow:auto">
                <asp:Table ID="tblResults" runat="server" Width="100%" CellPadding="2" CellSpacing="0" CssClass="default" />
            </div>
        </td>
    </tr>
    <tr>
        <td><asp:Button ID="btnSave" runat="server" Text="Add Selected User(s)" CssClass="default" Width="200" OnClick="btnSave_Click" Visible="false" /></td>
    </tr>
</table>
<input type="hidden" id="hdnUsers" runat="server" />
</asp:Content>
