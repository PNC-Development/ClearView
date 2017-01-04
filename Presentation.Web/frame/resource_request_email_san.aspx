<%@ Page Language="C#" MasterPageFile="~/frame.Master" AutoEventWireup="true" CodeBehind="resource_request_email_san.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.resource_request_email_san" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" runat="server">
<table width="100%" height="100%" border="0" cellSpacing="2" cellPadding="4" class="default">
    <tr height="1">
        <td nowrap><b>Recipient:</b></td>
        <td width="100%">
            <table cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td><asp:TextBox ID="txtEmployee" runat="server" Width="300" CssClass="default" /></td>
                </tr>
                <tr>
                    <td>
                        <div id="divEmployee" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                            <asp:ListBox ID="lstEmployee" runat="server" CssClass="default" />
                        </div>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <asp:Panel ID="panHide" runat="server" Visible="false">
    <tr height="1">
        <td nowrap></td>
        <td width="100%"><asp:CheckBox ID="chkStatus" runat="server" CssClass="default" Text="Include Weekly Status" /></td>
    </tr>
    <tr height="1">
        <td nowrap></td>
        <td width="100%"><asp:CheckBox ID="chkAttachments" runat="server" CssClass="default" Text="Include Attachments" /></td>
    </tr>
    </asp:Panel>
    <tr>
        <td colspan="2">&nbsp;</td>
    </tr>
    <tr height="1">
        <td colspan="2"><hr size="1" noshade /></td>
    </tr>
    <tr height="1">
        <td colspan="2" align="right"><asp:LinkButton ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" Text="<img src='/images/bigCheckBox.gif' border='0' align='absmiddle'/><img src='/images/spacer.gif' border='0' width='10' height='20' align='absmiddle'/>Send" /></td>
    </tr>
</table>
<asp:HiddenField ID="hdnEmployee" runat="server" />
</asp:Content>
