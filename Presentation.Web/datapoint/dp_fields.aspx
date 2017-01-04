<%@ Page Language="C#" MasterPageFile="~/frame.Master" AutoEventWireup="true" CodeBehind="dp_fields.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.dp_fields" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" Runat="Server">
<script type="text/javascript">
</script>
            <table width="100%" height="100%" cellpadding="2" cellspacing="0" border="0">
                <tr height="1">
                    <td class="frame">&nbsp;DataPoint Fields</td>
                    <td class="frame" align="right"><a href="javascript:void(0);" onclick="parent.HidePanel();"><img src="/images/close.gif" border="0" title="Close"></a></td>
                </tr>
                <tr>
                    <td colspan="2">
                        <table width="100%" height="100%" cellpadding="2" cellspacing="0" border="0">
	                        <tr height="1">
	                            <td nowrap><b>KEY:</b></td>
	                            <td width="100%"><%=Request.QueryString["key"] %></td>
	                        </tr>
	                        <tr height="1">
	                            <td nowrap><b>User:</b></td>
	                            <td width="100%">
                                    <table cellpadding="0" cellspacing="0" border="0">
                                        <tr>
                                            <td><asp:TextBox ID="txtUser" runat="server" Width="400" CssClass="default" /></td>
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
	                        <tr height="1">
	                            <td nowrap></td>
	                            <td width="100%"><asp:Button ID="btnAdd" runat="server" CssClass="default" Width="75" Text="Add" OnClick="btnAdd_Click" /></td>
	                        </tr>
	                        <tr height="1">
	                            <td colspan="2" class="default">&nbsp;</td>
	                        </tr>
	                        <tr height="1">
	                            <td colspan="2" class="default"><b>Users with Edit Capability:</b></td>
	                        </tr>
                            <tr>
                                <td colspan="2" valign="top" class="default">
                                    <asp:ListBox ID="lstCurrent" runat="server" Width="475" Rows="35" CssClass="default" />
                                </td>
                            </tr>
	                        <tr height="1">
	                            <td colspan="2" class="default"><asp:Button ID="btnRemove" runat="server" CssClass="default" Width="75" Text="Delete" OnClick="btnRemove_Click" /></td>
	                        </tr>
                        </table>
                    </td>
                </tr>
            </table>
<asp:HiddenField ID="hdnUser" runat="server" />
</asp:Content>
