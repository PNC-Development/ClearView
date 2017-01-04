<%@ Page Language="C#" MasterPageFile="~/frame.Master" AutoEventWireup="true" CodeBehind="zeus.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.zeus" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" runat="server">
    <table width="100%" height="100%" cellpadding="2" cellspacing="0" border="0">
        <tr>
            <td colspan="2" valign="top" class="default">
                <div style="height:100%; width:695; overflow:auto; background-color:#FFFFFF">
                    <table width="100%" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
		                <asp:Repeater id="rptZeus" runat="server">
			                <ItemTemplate>
				                <tr>
                                    <td nowrap><img src='/images/<%# DataBinder.Eval(Container.DataItem, "error").ToString() == "1" ? "cancel" : "check" %>.gif' border='0' align='absmiddle' /></td>
                                    <td nowrap class="bold"><%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "created").ToString()).ToString() %></td>
					                <td width="100%" class="smalldefault"><%# DataBinder.Eval(Container.DataItem, "message") %></td>
				                </tr>
			                </ItemTemplate>
		                </asp:Repeater>
                        <tr>
                            <td colspan="3">
                                <asp:Label ID="lblZeus" runat="server" Visible="false" Text="<img src='/images/alert.gif' border='0' align='absmiddle'> There is no information about this build" />
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
