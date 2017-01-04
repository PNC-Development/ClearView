<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="server_order.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.server_order" %>

<asp:Panel ID="panOrder" runat="server" Visible="true">
<table width="100%" cellpadding="4" cellspacing="0" border="0">
    <tr>
        <td colspan="2" class="header">Order a Device</td>
    </tr>
    <tr>
        <td colspan="2">Place an order for a device then add the order to ClearView.</td>
    </tr>
    <tr>
        <td nowrap>Nickname:</td>
        <td width="100%"><asp:TextBox ID="txtOrderName" runat="server" CssClass="default" Width="300" MaxLength="50" /></td>
    </tr>
    <tr>
        <td nowrap>Tracking Number:<font class="required">&nbsp;*</font></td>
        <td width="100%"><asp:TextBox ID="txtOrderTracking" runat="server" CssClass="default" Width="300" MaxLength="50" /></td>
    </tr>
    <tr>
        <td nowrap>Model:<font class="required">&nbsp;*</font></td>
        <td width="100%"><asp:DropDownList ID="ddlOrderModel" runat="server" CssClass="default" Width="300" /></td>
    </tr>
    <tr>
        <td nowrap>Location:<font class="required">&nbsp;*</font></td>
        <td width="100%"><asp:TextBox ID="txtOrderAddress" CssClass="lightdefault" runat="server" Text="" Width="400" ReadOnly="true" />&nbsp;&nbsp;<asp:Button ID="btnOrderAddress" runat="server" CssClass="default" Width="25" Text="..." /> <span class="lightdefault"><img src="/images/hand_left.gif" border="0" align="absmiddle" /> Click here to select a value</span></td>
    </tr>
    <tr>
        <td nowrap>Class:<font class="required">&nbsp;*</font></td>
        <td width="100%"><asp:DropDownList ID="ddlOrderClass" CssClass="default" runat="server" Width="300" /></td>
    </tr>
    <tr>
        <td nowrap>Environment:<font class="required">&nbsp;*</font></td>
        <td width="100%">
            <asp:DropDownList ID="ddlOrderEnvironment" CssClass="default" runat="server" Width="300" Enabled="false" >
                <asp:ListItem Value="-- Please select a Class --" />
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td nowrap>Quantity:<font class="required">&nbsp;*</font></td>
        <td width="100%"><asp:TextBox ID="txtOrderQuantity" runat="server" CssClass="default" Width="80" MaxLength="10" /></td>
    </tr>
    <tr>
        <td nowrap>Order Date:<font class="required">&nbsp;*</font></td>
        <td width="100%"><asp:TextBox ID="txtOrderDate" runat="server" CssClass="default" Width="80" MaxLength="10" /> <asp:ImageButton ID="imgOrderDate" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
    </tr>
    <asp:Panel ID="panOrderUpdate" runat="server" Visible="false">
    <tr>
        <td nowrap>Order Status:</td>
        <td width="100%">
            <asp:DropDownList ID="ddlOrderStatus" runat="server" CssClass="default">
                <asp:ListItem Value="0" Text="Pending" />
                <asp:ListItem Value="1" Text="Approved" />
                <asp:ListItem Value="-1" Text="Rejected" />
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td nowrap valign="top">Comments:</td>
        <td width="100%"><asp:TextBox ID="txtOrderComments" runat="server" CssClass="default" Width="600" TextMode="MultiLine" Rows="6" /></td>
    </tr>
    </asp:Panel>
    <tr>
        <td nowrap>&nbsp;</td>
        <td width="100%">
            <asp:Button ID="btnAddOrder" runat="server" CssClass="default" Width="125" Text="Place Order" OnClick="btnAddOrder_Click" />
            <asp:Button ID="btnUpdateOrder" runat="server" CssClass="default" Width="125" Text="Update Order" OnClick="btnUpdateOrder_Click" />
        </td>
    </tr>
</table>
<asp:Panel ID="panOrders" runat="server" Visible="true">
<br />
<table width="100%" cellpadding="4" cellspacing="0" border="0">
    <tr>
        <td class="header">Current Orders</td>
    </tr>
    <tr>
        <td>Devices that have been ordered.</td>
    </tr>
    <tr>
        <td>
            <table width="100%" cellpadding="3" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                <tr bgcolor="#EEEEEE">
                    <td><b><u>Name:</u></b></td>
                    <td><b><u>Tracking Number:</u></b></td>
                    <td><b><u>Model:</u></b></td>
                    <td><b><u>Ordered:</u></b></td>
                    <td align="center"><b><u>Quantity:</u></b></td>
                    <td align="center"><b><u>Received:</u></b></td>
                    <td align="center"><b><u>Status:</u></b></td>
                    <td><b><u></u></b></td>
                </tr>
                <asp:repeater ID="rptOrders" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td><%# DataBinder.Eval(Container.DataItem, "name") %></td>
                            <td><%# DataBinder.Eval(Container.DataItem, "tracking") %></td>
                            <td><%# DataBinder.Eval(Container.DataItem, "modelname") %></td>
                            <td><%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "ordered").ToString()).ToShortDateString()%></td>
                            <td align="center"><asp:Label ID="lblQuantity" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "quantity") %>' /></td>
                            <td align="center"><asp:Label ID="lblReceived" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "received") %>' /></td>
                            <td align="center"><img src='/images/<%# (DataBinder.Eval(Container.DataItem, "status").ToString() == "1" ? "check" : (DataBinder.Eval(Container.DataItem, "status").ToString() == "-1" ? "cancel" : "pending")) %>.gif' border='0' align='absmiddle' /></td>
                            <td align="right">
                                <asp:Panel ID="panEdit" runat="server" Visible="false">
                                    <asp:Button ID="btnEditOrder" runat="server" CssClass="default" Width="100" OnClick="btnEditOrder_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' Text="Edit Order" /> <asp:Button ID="btnDeleteOrder" runat="server" CssClass="default" Width="100" OnClick="btnDeleteOrder_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' Text="Cancel Order" />
                                </asp:Panel>
                                <asp:Panel ID="panRemove" runat="server" Visible="false">
                                    <asp:Button ID="btnRemoveOrder" runat="server" CssClass="default" Width="100" OnClick="btnRemoveOrder_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' Text="Remove Order" Enabled="false" />
                                </asp:Panel>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:repeater>
                <tr>
                    <td colspan="7" class="default">
                        <asp:Label ID="lblNoOrders" runat="server" Visible="false" Text="<img src='/images/spacer.gif' border='0' height='1' width='25' /><img src='/images/alert.gif' border='0' align='absmiddle'> There are no orders" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
<p><span style="width:100%;border-bottom:1 dotted #999999;">&nbsp;</span></p>
</asp:Panel>
</asp:Panel>
<asp:Panel ID="panHost" runat="server" Visible="true">
<asp:Panel ID="panHosts" runat="server" Visible="true">
<br />
<table width="100%" cellpadding="4" cellspacing="0" border="0">
    <tr>
        <td class="header">Current Hosts</td>
        <td align="right"><asp:Button ID="btnAddHost" runat="server" CssClass="default" Width="125" Text="Add Host" /></td>
    </tr>
    <tr>
        <td colspan="2">
            <table width="100%" cellpadding="3" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                <tr bgcolor="#EEEEEE">
                    <td><b><u>Name:</u></b></td>
                    <td><b><u>Host Type:</u></b></td>
                    <td><b><u>Device Name:</u></b></td>
                    <td><b><u>Model:</u></b></td>
                    <td><b><u>Commitment Date:</u></b></td>
                    <td align="center"><b><u>Status:</u></b></td>
                    <td><b><u></u></b></td>
                </tr>
                <asp:repeater ID="rptHosts" runat="server">
                    <ItemTemplate>
                        <asp:Label ID="lblAnswer" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "id") %>' />
                        <asp:Label ID="lblHost" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "hostid") %>' />
                        <tr>
                            <asp:Label ID="lblRequestId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "requestid") %>' />
                            <asp:Label ID="lblComplete" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "completed") %>' />
                            <td><%# DataBinder.Eval(Container.DataItem, "name") %></td>
                            <td><%# DataBinder.Eval(Container.DataItem, "host") %></td>
                            <td><%# DataBinder.Eval(Container.DataItem, "device") %></td>
                            <td><%# DataBinder.Eval(Container.DataItem, "model") %></td>
                            <td align="center"><asp:Label ID="lblCommitment" runat="server" CssClass="default" Text='<%# (DataBinder.Eval(Container.DataItem, "implementation").ToString() == "" ? "" : DateTime.Parse(DataBinder.Eval(Container.DataItem, "implementation").ToString()).ToShortDateString()) %>' /></td>
                            <td align="center"><%# "<img src=\"/images/" + (DataBinder.Eval(Container.DataItem, "completed").ToString() == "" ? "pending" : "check") + ".gif\" border=\"0\" align=\"absmiddle\"/>" %></td>
                            <td align="right"><asp:Button ID="btnEdit" runat="server" CssClass="default" Width="75" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' Text="Edit" />&nbsp;<asp:Button ID="btnDelete" runat="server" CssClass="default" Width="75" OnClick="btnDelete_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' Text="Delete" />&nbsp;<asp:Button ID="btnExecute" runat="server" CssClass="default" Width="75" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' Text="Execute" />&nbsp;<asp:Button ID="btnConfigure" runat="server" CssClass="default" Width="75" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "assetid") %>' Text="Configure" /></td>
                        </tr>
                    </ItemTemplate>
                </asp:repeater>
                <tr>
                    <td colspan="6" class="default">
                        <asp:Label ID="lblNone" runat="server" Visible="false" Text="<img src='/images/spacer.gif' border='0' height='1' width='25' /><img src='/images/alert.gif' border='0' align='absmiddle'> There are no hosts" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
</asp:Panel>
</asp:Panel>
<input type="hidden" id="hdnOrderAddress" runat="server" />
<input type="hidden" id="hdnOrderEnvironment" runat="server" />
