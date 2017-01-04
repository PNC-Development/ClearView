<%@ Page Language="C#" MasterPageFile="~/frame.Master" AutoEventWireup="true" CodeBehind="design_approvers.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.design_approvers" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" Runat="Server">
<script type="text/javascript">
</script>
<table width="100%" cellpadding="0" cellspacing="5" border="0">
    <tr>
        <td rowspan="2"><img src="/images/user_add.gif" border="0" align="absmiddle" /></td>
        <td class="header" valign="bottom" width="100%">Design Approvers</td>
    </tr>
    <tr>
        <td valign="top" width="100%">Add a list of people to approve this design.</td>
    </tr>
</table>
<table width="550" cellpadding="5" cellspacing="3" border="0">
    <tr>
        <td colspan="2">How do you want the approval process to work?</td>
    </tr>
    <tr>
        <td colspan="2">
            <table width="500" cellpadding="3" cellspacing="0" border="0">
                <tr>
                    <td><asp:RadioButton ID="radAny" runat="server" CssClass="default" Text="Any of the approvers approving the design will start the build" GroupName="radApprovalOption" /></td>
                </tr>
                <tr>
                    <td><asp:RadioButton ID="radAll" runat="server" CssClass="default" Text="All of the approvers must approve the design before the build can start" GroupName="radApprovalOption" /></td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td nowrap>Approver:</td>
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
        </td>
    </tr>
    <tr>
        <td></td>
        <td><asp:Button ID="btnAdd" runat="server" CssClass="default" Width="100" Text="Add Approver" OnClick="btnAdd_Click" /></td>
    </tr>
    <tr>
        <td colspan="2">
            <table width="100%" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #CCCCCC; background-color:#FFFFFF">
                <tr bgcolor="#EEEEEE">
                    <td colspan="2"><b>Current List of Approvers</b></td>
                </tr>
                <asp:repeater ID="rptApprovers" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td><%# DataBinder.Eval(Container.DataItem, "username") %></td>
                            <td align="right">[<asp:LinkButton ID="btnDelete" runat="server" CssClass="default" Text="Delete" OnClick="btnDelete_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' />]</td>
                        </tr>
                    </ItemTemplate>
                </asp:repeater>
                <tr>
                    <td colspan="2">
                        <asp:Label ID="lblApprovers" runat="server" CssClass="default" Visible="false" Text="<img src='/images/alert.gif' border='0' align='absmiddle'> There are no approvers..." />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <table width="100%" cellpadding="5" cellspacing="3" border="0">
                <tr>
                    <td><asp:Button ID="btnSubmit" runat="server" CssClass="default" Width="175" Text="Submit & Queue for Build" OnClick="btnSubmit_Click" /></td>
                </tr>
            </table>
        </td>
    </tr>
</table>
<asp:HiddenField ID="hdnUser" runat="server" />
</asp:Content>
