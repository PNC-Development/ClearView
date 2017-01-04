<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="sys_returned.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.sys_returned" %>


<script type="text/javascript">
    function ViewResource(strUrl, strID, strNew) {
        if (strNew == "0")
            window.navigate(strUrl);
        else
            OpenWindow("RESOURCE_REQUEST", strID);
    }
</script>
<asp:Panel ID="panHide" runat="server" Visible="true">
<table width="100%" cellpadding="0" cellspacing="0" border="0" class="default">
    <tr>
        <td><img src="/images/table_topleft.gif" border="0" width="5" height="26"></td>
        <td nowrap background="/images/table_top.gif" class="greentableheader" width="100%"><asp:Label ID="lblTitle" runat="server" CssClass="greentableheader" /></td>
        <td nowrap background="/images/table_top.gif" class="greentableheader"></td>
        </td>
        <td><img src="/images/table_topright.gif" border="0" width="5" height="26"></td>
    </tr>
    <tr>
        <td background="/images/table_left.gif"><img src="/images/table_left.gif" width="5" height="10"></td>
        <td width="100%" bgcolor="#FFFFFF" colspan="2">
            <br />
            <table width="100%" cellpadding="0" cellspacing="5" border="0">
                <tr>
                    <td rowspan="2"><img src="/images/hugeAlert.gif" border="0" align="absmiddle" /></td>
                    <td class="header" width="100%" valign="bottom">Returned Service Request(s)</td>
                </tr>
                <tr>
                    <td width="100%" valign="top">The following service request(s) are waiting for you to respond...</td>
                </tr>
            </table>
            <br />
            <table width="100%" cellpadding="5" cellspacing="0" border="0" align="center" style="border:solid 1px #CCCCCC">
            <asp:repeater ID="rptView" runat="server">
                <HeaderTemplate>
                    <tr bgcolor="#EEEEEE">
                        <td class="tableheader"><b>Number</b></td>
                        <td class="tableheader"><b>Title</b></td>
                        <td class="tableheader"><b>Service</b></td>
                        <td class="tableheader"><b>Submitted</b></td>
                        <td class="tableheader"><b>Returned On</b></td>
                        <td class="tableheader"><b>Returned By</b></td>
                    </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class="default" onmouseover="CellRowOver(this);" onmouseout="CellRowOut(this);" onclick="ViewResource('<%# strRedirect + "?rid=" + DataBinder.Eval(Container.DataItem, "requestid") + "&formid=" + DataBinder.Eval(Container.DataItem, "serviceid") + "&num=" + DataBinder.Eval(Container.DataItem, "number") + "&returned=true"  %>','<%#DataBinder.Eval(Container.DataItem, "RRWID")%>','<%#DataBinder.Eval(Container.DataItem, "ReturningToPreServiceWF")%>');">
                        <td nowrap valign="top"><%# DataBinder.Eval(Container.DataItem, "cvt") %></td>
                        <td width="100%" valign="top"><%# DataBinder.Eval(Container.DataItem, "name") %></td>
                        <td nowrap valign="top"><%# DataBinder.Eval(Container.DataItem, "service") %></td>
                        <td nowrap valign="top"><%# DataBinder.Eval(Container.DataItem, "submitted") %></td>
                        <td nowrap valign="top"><%# DataBinder.Eval(Container.DataItem, "returned") %></td>
                        <td nowrap valign="top"><%# DataBinder.Eval(Container.DataItem, "returned_by")%></td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr class="default" bgcolor="#F6F6F6" onmouseover="CellRowOver(this);" onmouseout="CellRowOut(this);" onclick="ViewResource('<%# strRedirect + "?rid=" + DataBinder.Eval(Container.DataItem, "requestid") + "&formid=" + DataBinder.Eval(Container.DataItem, "serviceid") + "&num=" + DataBinder.Eval(Container.DataItem, "number") + "&returned=true"  %>','<%#DataBinder.Eval(Container.DataItem, "RRWID")%>','<%#DataBinder.Eval(Container.DataItem, "ReturningToPreServiceWF")%>');">
                        <td nowrap valign="top"><%# DataBinder.Eval(Container.DataItem, "cvt") %></td>
                        <td width="100%" valign="top"><%# DataBinder.Eval(Container.DataItem, "name") %></td>
                        <td nowrap valign="top"><%# DataBinder.Eval(Container.DataItem, "service") %></td>
                        <td nowrap valign="top"><%# DataBinder.Eval(Container.DataItem, "submitted") %></td>
                        <td nowrap valign="top"><%# DataBinder.Eval(Container.DataItem, "returned") %></td>
                        <td nowrap valign="top"><%# DataBinder.Eval(Container.DataItem, "returned_by")%></td>
                    </tr>
                </AlternatingItemTemplate>
            </asp:repeater>
            </table>
                <table cellpadding="0" cellspacing="5" border="0">
                    <tr>
                        <td><img src="/images/tack.gif" border="0" align="absmiddle" /></td>
                        <td class="greendefault"><b>NOTE:</b> You can hide this list at any time by unchecking the &quot;Show Service Request Returns&quot; option in <a href="/Settings/default.aspx">My Settings</a>.</td>
                    </tr>
                </table>
            <p>&nbsp;</p>
        </td>
        <td background="/images/table_right.gif"><img src="/images/table_right.gif" width="5" height="10"></td>
    </tr>
    <tr>
        <td><img src="/images/table_bottomLeft.gif" border="0" width="5" height="9"></td>
        <td width="100%" background="/images/table_bottom.gif" colspan="2"></td>
        <td><img src="/images/table_bottomRight.gif" border="0" width="5" height="9"></td>
    </tr>
</table>
<br />
</asp:Panel>