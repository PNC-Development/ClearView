<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="server_errors.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.server_errors" %>



<script type="text/javascript">
    function ShowRR(strID) {
    	var o = event.srcElement;
	    if (o.tagName != "INPUT")
            OpenWindow("RESOURCE_REQUEST", strID);
        return false;
    }
    function FixError(strServer, strStep) {
        OpenWindow("FIX_SERVER_ERROR", "?id=" + strServer + "&step=" + strStep);
        return false;
    }
</script>
<br />
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
            <table width="100%" cellpadding="3" cellspacing="0" border="0">
                <tr>
                    <td>
                        <table width="100%" cellpadding="4" cellspacing="0" border="0" align="center">
                            <tr>
                                <td></td>
                                <td><b><u>Server Name:</u></b></td>
                                <td><b><u>Implementor:</u></b></td>
                                <td><b><u>Project Name:</u></b></td>
                                <td><b><u>Project Number:</u></b></td>
                                <td><b><u>Nickname:</u></b></td>
                                <td><b><u>Project Lead:</u></b></td>
                                <td><b><u>Integration Engineer:</u></b></td>
                                <td><b><u>Date:</u></b></td>
                            </tr>
                            <asp:repeater ID="rptView" runat="server">
                                <ItemTemplate>
                                    <tr class="default" onmouseover="CellRowOver(this);" onmouseout="CellRowOut(this);" onclick='<%# "ShowRR(\"" + DataBinder.Eval(Container.DataItem, "id") + "\",\"" + intPage.ToString() + "\");" %>'">
                                        <td><input type="button" onclick="FixError('<%# DataBinder.Eval(Container.DataItem, "serverid") %>','<%# DataBinder.Eval(Container.DataItem, "step") %>');" value="Fixed" class="default" /></td>
                                        <td><%# DataBinder.Eval(Container.DataItem, "servername") %></td>
                                        <td><%# DataBinder.Eval(Container.DataItem, "implementor") %></td>
                                        <td><%# DataBinder.Eval(Container.DataItem, "name") %></td>
                                        <td><%# DataBinder.Eval(Container.DataItem, "number") %></td>
                                        <td><%# DataBinder.Eval(Container.DataItem, "nickname") %></td>
                                        <td><%# DataBinder.Eval(Container.DataItem, "lead") %></td>
                                        <td><%# DataBinder.Eval(Container.DataItem, "engineer") %></td>
                                        <td><%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "implementation").ToString()).ToShortDateString()%></td>
                                    </tr>
                                </ItemTemplate>
                                <AlternatingItemTemplate>
                                    <tr class="default" bgcolor="#F6F6F6" onmouseover="CellRowOver(this);" onmouseout="CellRowOut(this);" onclick='<%# "ShowRR(\"" + DataBinder.Eval(Container.DataItem, "id") + "\",\"" + intPage.ToString() + "\");" %>'">
                                        <td><input type="button" onclick="FixError('<%# DataBinder.Eval(Container.DataItem, "serverid") %>','<%# DataBinder.Eval(Container.DataItem, "step") %>');" value="Fixed" class="default" /></td>
                                        <td><%# DataBinder.Eval(Container.DataItem, "servername") %></td>
                                        <td><%# DataBinder.Eval(Container.DataItem, "implementor") %></td>
                                        <td><%# DataBinder.Eval(Container.DataItem, "name") %></td>
                                        <td><%# DataBinder.Eval(Container.DataItem, "number") %></td>
                                        <td><%# DataBinder.Eval(Container.DataItem, "nickname") %></td>
                                        <td><%# DataBinder.Eval(Container.DataItem, "lead") %></td>
                                        <td><%# DataBinder.Eval(Container.DataItem, "engineer") %></td>
                                        <td><%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "implementation").ToString()).ToShortDateString()%></td>
                                    </tr>
                                </AlternatingItemTemplate>
                            </asp:repeater>
                            <tr>
                                <td colspan="4">
                                    <asp:Label ID="lblNone" runat="server" CssClass="default" Visible="false" Text="<img src='/images/spacer.gif' border='0' height='1' width='25' /><img src='/images/alert.gif' border='0' align='absmiddle'> No errors" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
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