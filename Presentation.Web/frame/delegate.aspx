<%@ Page Language="C#" MasterPageFile="~/frame.Master" AutoEventWireup="true" CodeBehind="delegate.aspx.cs" Inherits="NCC.ClearView.Presentation.Web._delegate" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" runat="server">
<table width="100%" cellpadding="4" cellspacing="3" border="0">
    <tr>
        <td nowrap>Delegate:</td>
        <td width="100%">
            <table cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td><asp:TextBox ID="txtDelegate" runat="server" Width="250" CssClass="default" /></td>
                </tr>
                <tr>
                    <td>
                        <div id="divDelegate" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                            <asp:ListBox ID="lstDelegate" runat="server" CssClass="default" />
                        </div>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td nowrap>Permission:</td>
        <td width="100%">
            <asp:DropDownList ID="ddlRights" runat="server" CssClass="default">
                <asp:ListItem Value="0" Text="-- SELECT --" />
                <asp:ListItem Value="1" Text="Out of Office Buddy" />
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td nowrap>&nbsp;</td>
        <td width="100%"><asp:Button ID="btnSave" runat="server" CssClass="default" Width="75" Text="Save" OnClick="btnSave_Click" />
    </tr>
</table>
<asp:HiddenField ID="hdnDelegate" runat="server" />
</asp:Content>
