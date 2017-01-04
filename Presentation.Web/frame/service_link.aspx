<%@ Page Language="C#" MasterPageFile="~/frame.Master" AutoEventWireup="true" CodeBehind="service_link.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.service_link" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" Runat="Server">
<script type="text/javascript">
</script>
<table width="100%" cellpadding="0" cellspacing="5" border="0">
    <tr>
        <td rowspan="2"><img src="/images/user_add.gif" border="0" align="absmiddle" /></td>
        <td class="header" width="100%" valign="bottom">Send Service Link</td>
    </tr>
    <tr>
        <td width="100%" valign="top">Enter the user and the link will be sent to them.</td>
    </tr>
</table>
<table width="100%" cellpadding="5" cellspacing="3" border="0">
    <tr>
        <td nowrap>Service:</td>
        <td width="100%"><asp:Label ID="lblService" runat="server" CssClass="default" /></td>
    </tr>
    <tr>
        <td nowrap>Location:</td>
        <td width="100%"><asp:Label ID="lblLocation" runat="server" CssClass="default" /></td>
    </tr>
    <tr>
        <td nowrap>User:</td>
        <td width="100%">
            <table cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td><asp:TextBox ID="txtUser" runat="server" Width="300" CssClass="default" /></td>
                </tr>
                <tr>
                    <td>
                        <div id="divUser" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                            <asp:ListBox ID="lstUser" runat="server" CssClass="default" />
                        </div>
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hdnUser" runat="server" />
        </td>
    </tr>
    <tr>
        <td nowrap class="footer">&nbsp;</td>
        <td class="footer">(Please enter a valid LAN ID, First Name, or Last Name)</td>
    </tr>
    <tr>
        <td></td>
        <td><asp:Button ID="btnSubmit" runat="server" CssClass="default" Width="75" Text="Submit" OnClick="btnSubmit_Click" /></td>
    </tr>
</table>
<input type="hidden" id="hdnUsers" runat="server" />
</asp:Content>
