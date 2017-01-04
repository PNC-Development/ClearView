<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RegistrationForm.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.RegistrationForm" %>
<asp:Panel ID="panRegistered" runat="server" Visible="false">
<table class="fullWidth">
    <tr>
        <td class="center header">
            <img src="/images/bigCheck.gif" alt="Big Check" /> Your LAN ID [<%=strUser %>] has already been registered in ClearView.
        </td>
    </tr>
    <tr>
        <td class="center header">
            <a href="/index.aspx">Click here to login.</a>
        </td>
    </tr>
</table>
</asp:Panel>
<asp:Panel ID="panNotRegisteredNone" runat="server" Visible="false">
<table class="fullWidth">
    <tr>
        <td class="center header">
            <img src="/images/bigAlert.gif" alt="Big Alert" /> The active directory query for account [<%=strUser %>] did not return any results.
        </td>
    </tr>
</table>
</asp:Panel>

<asp:Panel ID="panNotRegistered" runat="server" Visible="false">
<table class="fullWidth">
    <tr>
        <td colspan="2">Please complete the following form and click <b>Register</b> to finish your registration.</td>
    </tr>
    <tr>
        <td>LAN ID:<span class="required">&nbsp;*</span></td>
        <td class="fullWidth"><asp:TextBox ID="txtXID" runat="server" Width="150" Enabled="false" /></td>
    </tr>
    <tr>
        <td>First&nbsp;Name:<span class="required">&nbsp;*</span></td>
        <td class="fullWidth"><asp:TextBox ID="txtFirst" runat="server" Width="250" MaxLength="100" /></td>
    </tr>
    <tr>
        <td>Last&nbsp;Name:<span class="required">&nbsp;*</span></td>
        <td class="fullWidth"><asp:TextBox ID="txtLast" runat="server" Width="250" MaxLength="100" /></td>
    </tr>
    <tr>
        <td>Manager:<span class="required">&nbsp;*</span></td>
        <td class="fullWidth">
            <table cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td><asp:TextBox ID="txtManager" runat="server" Width="250" /></td>
                </tr>
                <tr>
                    <td>
                        <div id="divManager" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                            <asp:ListBox ID="lstManager" runat="server" />
                        </div>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td>Phone&nbsp;Number:</td>
        <td class="fullWidth"><asp:TextBox ID="txtPhone" runat="server" Width="150" MaxLength="20" /></td>
    </tr>
    <tr>
        <td>&nbsp;</td>
        <td class="fullWidth"><asp:Button ID="btnRegister" Text="Register" Width="100" runat="server" OnClick="btnRegister_Click" /></td>
    </tr>
    <tr>
        <td colspan="2" class="right required">* = Required Field</td>
    </tr>
</table>
</asp:Panel>

<asp:Panel ID="panFinish" runat="server" Visible="false">
<table class="fullWidth">
    <tr>
        <td class="center header">
            <img src="/images/bigCheck.gif" alt="Big Check" /> Your XID [<%=strUser %>] was successfully registered in ClearView.
        </td>
    </tr>
    <tr>
        <td class="center header">
            <a href="/index.aspx">Click here to login.</a>
        </td>
    </tr>
</table>
</asp:Panel>
<asp:HiddenField ID="hdnManager" runat="server" />