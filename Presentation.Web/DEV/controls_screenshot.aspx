<%@ Page Language="C#" MasterPageFile="~/window.Master" AutoEventWireup="true" CodeBehind="controls_screenshot.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.DEV.controls_screenshot" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" runat="server">
<table cellpadding-"10" cellspacing="30" border="0">
    <tr>
        <td>
            <asp:TextBox ID="TextBox1" runat="server" Width="300" Text="Example 1" />
        </td>
    </tr>
    <tr>
        <td>
            <asp:TextBox ID="TextBox2" runat="server" Width="300" Text="Example 1" TextMode="MultiLine" Rows="5" />
        </td>
    </tr>
    <tr>
        <td>
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
        <td>
            <asp:TextBox ID="TextBox3" runat="server" CssClass="default" Width="100" MaxLength="10" Text="12/31/2013" /> <asp:ImageButton ID="imgDate" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" />
        </td>
    </tr>
    <tr>
        <td>
            <asp:HyperLink ID="HyperLink1" runat="server" CssClass="default" Text="Example 1" NavigateUrl="http://www.pnc.com" />
        </td>
    </tr>
    <tr>
        <td>
            <asp:CheckBox ID="CheckBox1" runat="server" CssClass="default" Text="Example 1" />
        </td>
    </tr>
    <tr>
        <td>
            <asp:CheckBoxList ID="CheckBoxList1" runat="server" CssClass="default" RepeatColumns="1" RepeatDirection="Vertical" >
                <asp:ListItem Value="Example 1" />
                <asp:ListItem Value="Example 2" />
                <asp:ListItem Value="Example 3" />
            </asp:CheckBoxList>
        </td>
    </tr>
    <tr>
        <td>
            <asp:RadioButtonList ID="RadioButtonList1" runat="server" CssClass="default" RepeatColumns="1" RepeatDirection="Vertical" >
                <asp:ListItem Value="Example 1" />
                <asp:ListItem Value="Example 2" />
                <asp:ListItem Value="Example 3" />
            </asp:RadioButtonList>
        </td>
    </tr>
    <tr>
        <td>
            <asp:DropDownList ID="DropDownList1" runat="server" CssClass="default" Width="300" >
                <asp:ListItem Value="Example 1" />
                <asp:ListItem Value="Example 2" />
                <asp:ListItem Value="Example 3" />
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td>
            <asp:ListBox ID="ListBox1" runat="server" CssClass="default" Width="300" >
                <asp:ListItem Value="Example 1" />
                <asp:ListItem Value="Example 2" />
                <asp:ListItem Value="Example 3" />
            </asp:ListBox>
        </td>
    </tr>
    <tr>
        <td>
            <table cellpadding="2" cellspacing="0" border="0">
                <tr>
                    <td colspan="2"><asp:TextBox ID="TextBox4" runat="server" Width="300" Text="Example 3" /></td>
                </tr>
                <tr>
                    <td><asp:Button ID="btnAdd" runat="server" Text="Add to List" Width="75" /></td>
                    <td align="right"><asp:Button ID="btnRemove" runat="server" Text="Remove from List" Width="115" /></td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:ListBox ID="ListBox2" runat="server" CssClass="default" Width="300" >
                            <asp:ListItem Value="Example 1" />
                            <asp:ListItem Value="Example 2" Selected="True" />
                            <asp:ListItem Value="Example 3" />
                            <asp:ListItem Value="Example 4" />
                        </asp:ListBox>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td>
            <asp:FileUpload ID="FileUpload1" runat="server" Width="350" Text="Example 1" CssClass="default" Height="18" />
        </td>
    </tr>
    <tr>
        <td>
            <table cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td><asp:TextBox ID="txtServer" runat="server" Width="200" CssClass="default" /></td>
                </tr>
                <tr>
                    <td>
                        <div id="divServer" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                            <asp:ListBox ID="lstServer" runat="server" CssClass="default" />
                        </div>
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hdnServer" runat="server" />
        </td>
    </tr>
    <tr>
        <td>
            <table cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td><asp:TextBox ID="txtMnemonic" runat="server" Width="300" CssClass="default" /></td>
                </tr>
                <tr>
                    <td>
                        <div id="divMnemonic" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                            <asp:ListBox ID="lstMnemonic" runat="server" CssClass="default" />
                        </div>
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hdnMnemonic" runat="server" />
        </td>
    </tr>
    <tr>
        <td>
            <asp:CheckBox ID="chkAgree" runat="server" Text="I agree that I have read the <a href='http://www.google.com'>terms and conditions</a>" />
        </td>
    </tr>
</table>

</asp:Content>
