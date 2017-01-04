<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="resource_request_search.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.resource_request_search" %>


<script type="text/javascript">
function ShowDetail(oImg, oDiv) {
    oImg = document.getElementById(oImg);
    oDiv = document.getElementById(oDiv);
    if (oDiv.style.display == 'inline') {
        oDiv.style.display = 'none';
        SwapImage(oImg, '/images/plus.gif');
    }
    else {
        oDiv.style.display = 'inline';
        SwapImage(oImg, '/images/minus.gif');
    }
}
</script>
<table width="100%" cellpadding="0" cellspacing="0" border="0" class="default">
    <tr>
        <td><img src="/images/table_topleft.gif" border="0" width="5" height="26"></td>
        <td width="100%" background="/images/table_top.gif" class="greentableheader"><asp:Label ID="lblTitle" runat="server" CssClass="greentableheader" /></td>
        <td><img src="/images/table_topright.gif" border="0" width="5" height="26"></td>
    </tr>
    <tr>
        <td background="/images/table_left.gif"><img src="/images/table_left.gif" width="5" height="10"></td>
        <td width="100%" bgcolor="#FFFFFF">
            <br />
            <asp:Panel ID="panSearch" runat="server" Visible="false">
                <table width="100%" cellpadding="4" cellspacing="3" border="0" class="default">
                    <tr>
                        <td colspan="2" class="hugeheader"><img src="/images/bigSearch.gif" border="0" align="absmiddle" /> Service Search</td>
                    </tr>
                    <tr>
                        <td nowrap><b>Department:</b></td>
                        <td width="100%">
                            <asp:DropDownList ID="ddlDepartments" runat="server" CssClass="default" AutoPostBack="true" OnSelectedIndexChanged="ddlDepartments_Change" Width="400"  />
                        </td>
                    </tr>
                    <asp:Panel ID="panCategory" runat="server" Visible="false">
                    <tr>
                        <td nowrap><b>Category:</b></td>
                        <td width="100%">
                            <asp:DropDownList ID="ddlGroups" runat="server" CssClass="default" AutoPostBack="true" OnSelectedIndexChanged="ddlGroups_Change" Width="400"  />
                        </td>
                    </tr>
                    </asp:Panel>
                    <tr>
                        <td nowrap valign="top"><b>Service(s):</b></td>
                        <td width="100%">
                            <asp:ListBox ID="lstServices" runat="server" CssClass="default" Rows="10" Width="500" SelectionMode="Multiple" />
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td class="required">Use CTRL or SHIFT keys to select multiple services</td>
                    </tr>
                    <tr>
                        <td colspan="2">&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="2"><hr size="1" noshade /></td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <table width="100%" cellpadding="0" cellspacing="0" border="0" class="default">
                                <tr>
                                    <td><asp:HyperLink ID="hypDeliverable" runat="server" Text="<img src='/images/tack.gif' border='0' align='absmiddle' />View service deliverables" Target="_blank" /></td>
                                    <td align="right"><asp:Button ID="btnSearch" runat="server" CssClass="default" Text="Search" Width="75" OnClick="btnSearch_Click" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="panResult" runat="server" Visible="false">
                <table width="100%" cellpadding="4" cellspacing="3" border="0">
                    <tr>
                        <td class="hugeheader"><img src="/images/bigSearch.gif" border="0" align="absmiddle" /> Search Results</td>
                        <td align="right"><img src="/images/check.gif" border="0" align="absmiddle" /> <asp:HyperLink ID="hypClear" runat="server" Text="Start a New Search" /></td>
                    </tr>
                    <tr>
                        <td colspan="2"><%=strView %></td>
                    </tr>
                    <tr>
                        <td><asp:LinkButton ID="btnPrinter" runat="server" Text="<img src='/images/bigPrint.gif' border='0' align='absmiddle'/><img src='/images/spacer.gif' border='0' width='10' height='20' align='absmiddle'/>Printer-friendly version" /></td>
                        <td align="right"><asp:LinkButton ID="btnPDF" runat="server" Text="<img src='/images/bigPDF.gif' border='0' align='absmiddle'/><img src='/images/spacer.gif' border='0' width='10' height='20' align='absmiddle'/>Export to PDF" /></td>
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
<asp:Label ID="lblSearch" runat="server" Visible="false" />
<input type="hidden" id="hdnType" runat="server" />
<asp:Label ID="lblPage" runat="server" Visible="false" />
<asp:Label ID="lblSort" runat="server" Visible="false" />