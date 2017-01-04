<%@ Page Language="C#" MasterPageFile="~/frame.Master" AutoEventWireup="true" Codebehind="file_permissions.aspx.cs"
    Inherits="NCC.ClearView.Presentation.Web.file_permissions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" runat="Server">
    <table width="100%" cellpadding="2" cellspacing="0" border="0">
        <tr height="1">
            <td class="frame">
                &nbsp;Document Security</td>
            <td class="frame" align="right">
                <a href="javascript:window.print();" class="whitebold">[Print Page]</a></td>
            <td class="frame" align="right">
                <a href="javascript:void(0);" onclick="parent.HidePanel();">
                    <img src="/images/close.gif" border="0" title="Close"></a></td>
        </tr>
        <tr>
            <td colspan="3">
                <table width="100%" cellpadding="4" cellspacing="2" border="0">
                    <tr height="1">
                        <td nowrap>
                            <b>Document:</b></td>
                        <td width="100%">
                            <asp:Label ID="lblName" runat="server" CssClass="default" Visible="false" />
                            <asp:TextBox ID="txtName" runat="server" CssClass="default" Visible="false" Width="400"
                                MaxLength="100" />
                        </td>
                    </tr>
                    <asp:Panel ID="panEdit" runat="server" Visible="false">
                        <tr height="1">
                            <td nowrap valign="top">
                                <b>Description:</b></td>
                            <td width="100%">
                                <asp:TextBox ID="txtDescription" runat="server" CssClass="default" Width="400" TextMode="multiLine"
                                    Rows="5" /></td>
                        </tr>
                        <tr height="1">
                            <td nowrap>
                                <b>Security:</b></td>
                            <td width="100%">
                                <asp:DropDownList ID="ddlSecurity" runat="server" CssClass="default">
                                    <asp:ListItem Value="1" Text="Public (Available to Everyone)" />
                                    <asp:ListItem Value="0" Text="Shared (Available to Selected Resources)" />
                                    <asp:ListItem Value="-1" Text="Private (Not Available)" />
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr height="1">
                            <td>
                            </td>
                            <td>
                                <asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" Text="Save" Width="100"
                                    CssClass="default" />
                                <asp:Button ID="btnDelete" runat="server" OnClick="btnDelete_Click" Text="Delete"
                                    Width="100" CssClass="default" />
                            </td>
                        </tr>
                        <asp:Panel ID="panShared" runat="server" Visible="false">
                            <tr>
                                <td colspan="2">
                                    <hr size="1" noshade />
                                </td>
                            </tr>
                            <tr height="1">
                                <td nowrap>
                                    <b>User:</b></td>
                                <td width="100%">
                                    <table cellpadding="0" cellspacing="0" border="0">
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtUser" runat="server" Width="300" CssClass="default" /></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div id="divUser" runat="server" style="overflow: hidden; position: absolute; display: none;
                                                    background-color: #FFFFFF; border: solid 1px #CCCCCC">
                                                    <asp:ListBox ID="lstUser" runat="server" CssClass="default" />
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr height="1">
                                <td nowrap>
                                    <b>Security:</b></td>
                                <td width="100%">
                                    <asp:RadioButtonList ID="radSecurity" runat="server" CssClass="default" RepeatDirection="Horizontal">
                                        <asp:ListItem Value="1" Text="Viewer" Selected />
                                        <asp:ListItem Value="10" Text="Editor" />
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr height="1">
                                <td>
                                </td>
                                <td>
                                    <asp:Button ID="btnAdd" runat="server" OnClick="btnAdd_Click" Text="Add" Width="100"
                                        CssClass="default" /></td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <table width="100%" cellpadding="4" cellspacing="0" border="0" style="border: solid 1px #CCCCCC">
                                        <tr bgcolor="#EEEEEE">
                                            <td nowrap>
                                                <b>User</b></td>
                                            <td nowrap>
                                                <b>Security</b></td>
                                            <td>
                                            </td>
                                        </tr>
                                        <asp:Repeater ID="rptPermissions" runat="server">
                                            <ItemTemplate>
                                                <tr class="default">
                                                    <td>
                                                        <%# DataBinder.Eval(Container.DataItem, "username") %>
                                                    </td>
                                                    <td>
                                                        <%# (DataBinder.Eval(Container.DataItem, "security").ToString() == "1" ? "Viewer" : (DataBinder.Eval(Container.DataItem, "security").ToString() == "10" ? "Editor" : DataBinder.Eval(Container.DataItem, "security").ToString()))%>
                                                    </td>
                                                    <td align="right">
                                                        <asp:LinkButton ID="btnDeleteUser" runat="server" Text="Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "userid") %>'
                                                            OnClick="btnDeleteUser_Click" /></td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                        <tr>
                                            <td colspan="3">
                                                <asp:Label ID="lblNone" runat="server" CssClass="default" Visible="false" Text="<img src='/images/alert.gif' border='0' align='absmiddle'> There are no permissions assigned to this document" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </asp:Panel>
                    </asp:Panel>
                    <asp:Panel ID="panDenied" runat="server" Visible="false">
                        <tr>
                            <td colspan="2">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="2" align="center">
                                <img src="/images/error.gif" border="0" align="absmiddle" />
                                <b>Access Denied</b></td>
                        </tr>
                    </asp:Panel>
                </table>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="hdnUser" runat="server" />
</asp:Content>
