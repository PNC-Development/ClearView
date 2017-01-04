<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="asset_decommission.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.asset_decommission" %>
<script type="text/javascript">
</script>
<table width="100%" cellpadding="0" cellspacing="0" border="0" class="default">
    <tr>
        <td><img src="/images/table_topleft.gif" border="0" width="5" height="26"></td>
        <td nowrap background="/images/table_top.gif" class="greentableheader" width="100%"><asp:Label ID="lblTitle" runat="server" CssClass="greentableheader" /></td>
        <td><img src="/images/table_topright.gif" border="0" width="5" height="26"></td>
    </tr>
    <tr>
        <td background="/images/table_left.gif"><img src="/images/table_left.gif" width="5" height="10"></td>
        <td width="100%" bgcolor="#FFFFFF">
            <br />
            <table width="100%" cellpadding="0" cellspacing="5" border="0">
                <tr>
                    <td rowspan="2"><img src="/images/asset_decommission.gif" border="0" align="absmiddle" /></td>
                    <td class="header" width="100%" valign="bottom">Asset Decommission</td>
                </tr>
                <tr>
                    <td width="100%" valign="top">Decommissioning an asset means you are removing it from a working environment.</td>
                </tr>
            </table>
            <table width="100%" cellpadding="4" cellspacing="3" border="0">
                <tr>
                    <td nowrap>Platform:<font class="required">&nbsp;*</font></td>
                    <td width="100%">
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td><asp:DropDownList ID="ddlPlatform" runat="server" CssClass="default" Width="300" AutoPostBack="true" OnSelectedIndexChanged="ddlPlatform_Change" /></td>
                                <td class="bold">
                                    <div id="divWait" runat="server" style="display:none">
                                        <img src="/images/spacer.gif" border="0" width="20" height="1" /><img src="/images/please_wait.gif" border="0" align="absmiddle" /> Please Wait...
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <asp:Panel ID="panTypes" runat="server" Visible="false">
                <tr>
                    <td nowrap>Asset Type:<font class="required">&nbsp;*</font></td>
                    <td width="100%">
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td><asp:DropDownList ID="ddlTypes" runat="server" CssClass="default" Width="300" AutoPostBack="true" OnSelectedIndexChanged="ddlTypes_Change" /></td>
                                <td class="bold">
                                    <div id="divWait2" runat="server" style="display:none">
                                        <img src="/images/spacer.gif" border="0" width="20" height="1" /><img src="/images/please_wait.gif" border="0" align="absmiddle" /> Please Wait...
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                </asp:Panel>
                <asp:Panel ID="panPath" runat="server" Visible="false">
                    <asp:PlaceHolder ID="PHControl" runat="server" />
                </asp:Panel>
                <asp:Panel ID="panNoPath" runat="server" Visible="false">
                <tr>
                    <td colspan="2" align="center"><img src="/images/alert.gif" border="0" align="absmiddle" /> Asset information has not been configured for this platform...</td>
                </tr>
                </asp:Panel>
            </table>
            <asp:Panel ID="panAll" runat="server" Visible="false">
            <table width="100%" cellpadding="4" cellspacing="0" border="0" align="center"  style="border:solid 1px #CCCCCC">
                <tr bgcolor="#EEEEEE">
                    <td><asp:LinkButton ID="btnOrderID" runat="server" CssClass="tableheader" Text="<b>ID</b>" OnClick="btnOrder_Click" CommandArgument="id" /></td>
                    <td><asp:LinkButton ID="btnOrderName" runat="server" CssClass="tableheader" Text="<b>Device Name</b>" OnClick="btnOrder_Click" CommandArgument="name" /></td>
                    <td><asp:LinkButton ID="btnOrderSerial" runat="server" CssClass="tableheader" Text="<b>Serial Number</b>" OnClick="btnOrder_Click" CommandArgument="serial" /></td>
                    <td><asp:LinkButton ID="btnOrderAsset" runat="server" CssClass="tableheader" Text="<b>Asset Tag</b>" OnClick="btnOrder_Click" CommandArgument="asset" /></td>
                    <td><asp:LinkButton ID="btnOrderModel" runat="server" CssClass="tableheader" Text="<b>Model</b>" OnClick="btnOrder_Click" CommandArgument="model" /></td>
                    <td><asp:LinkButton ID="btnOrderBy" runat="server" CssClass="tableheader" Text="<b>User</b>" OnClick="btnOrder_Click" CommandArgument="username" /></td>
                    <td><asp:LinkButton ID="btnOrderOn" runat="server" CssClass="tableheader" Text="<b>Commissioned</b>" OnClick="btnOrder_Click" CommandArgument="datestamp" /></td>
                </tr>
                <asp:repeater ID="rptAll" runat="server">
                    <ItemTemplate>
                        <tr onmouseover="CellRowOver(this);" onmouseout="CellRowOut(this);"  onclick="window.navigate('<%# oPage.GetFullLink(intPage) + "?pid=" + intPlatform.ToString() + "&tid=" + intType.ToString() + "&id=" + DataBinder.Eval(Container.DataItem, "id") %>');">
                            <td valign="top"><%# DataBinder.Eval(Container.DataItem, "id")%></td>
                            <td valign="top"><%# DataBinder.Eval(Container.DataItem, "name") %></td>
                            <td valign="top"><%# DataBinder.Eval(Container.DataItem, "serial") %></td>
                            <td valign="top"><%# DataBinder.Eval(Container.DataItem, "asset") %></td>
                            <td valign="top"><%# DataBinder.Eval(Container.DataItem, "model") %></td>
                            <td valign="top"><%# DataBinder.Eval(Container.DataItem, "username") %></td>
                            <td valign="top"><%# (DataBinder.Eval(Container.DataItem, "datestamp").ToString() == "" ? "---" : DateTime.Parse(DataBinder.Eval(Container.DataItem, "datestamp").ToString()).ToShortDateString()) %></td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr bgcolor="F6F6F6" onmouseover="CellRowOver(this);" onmouseout="CellRowOut(this);"  onclick="window.navigate('<%# oPage.GetFullLink(intPage) + "?pid=" + intPlatform.ToString() + "&tid=" + intType.ToString() + "&id=" + DataBinder.Eval(Container.DataItem, "id") %>');">
                            <td valign="top"><%# DataBinder.Eval(Container.DataItem, "id")%></td>
                            <td valign="top"><%# DataBinder.Eval(Container.DataItem, "name") %></td>
                            <td valign="top"><%# DataBinder.Eval(Container.DataItem, "serial") %></td>
                            <td valign="top"><%# DataBinder.Eval(Container.DataItem, "asset") %></td>
                            <td valign="top"><%# DataBinder.Eval(Container.DataItem, "model") %></td>
                            <td valign="top"><%# DataBinder.Eval(Container.DataItem, "username") %></td>
                            <td valign="top"><%# (DataBinder.Eval(Container.DataItem, "datestamp").ToString() == "" ? "---" : DateTime.Parse(DataBinder.Eval(Container.DataItem, "datestamp").ToString()).ToShortDateString()) %></td>
                        </tr>
                    </AlternatingItemTemplate>
                </asp:repeater>
                <tr>
                    <td colspan="7">
                        <asp:Label ID="lblNone" runat="server" CssClass="default" Visible="false" Text="<img src='/images/spacer.gif' border='0' height='1' width='25' /><img src='/images/alert.gif' border='0' align='absmiddle'> There are no items" />
                    </td>
                </tr>
            </table>
            </asp:Panel>
            <p>&nbsp;</p>
        </td>
        <td background="/images/table_right.gif"><img src="/images/table_right.gif" width="5" height="10"></td>
    </tr>
    <tr>
        <td><img src="/images/table_bottomLeft.gif" border="0" width="5" height="9"></td>
        <td width="100%" background="/images/table_bottom.gif"></td>
        <td><img src="/images/table_bottomRight.gif" border="0" width="5" height="9"></td>
    </tr>
</table>
