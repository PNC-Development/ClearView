<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="rr_GENERIC.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.rr_GENERIC" %>


<script type="text/javascript">
function UpdateDDL(strQuestion) {
    var oTemp = document.getElementById('hdnGEN_' + strQuestion);
    oTemp.value = strValue;
}
</script>
<table width="100%" border="0" cellSpacing="2" cellPadding="4" class="default">
    <tr>
        <td nowrap valign="top">Statement of Work:<font class="required">&nbsp;*</font></td>
        <td width="100%" valign="top"><asp:TextBox ID="txtStatement" runat="server" CssClass="default" TextMode="MultiLine" Rows="8" Width="80%" /></td>
    </tr>
    <tr>
        <td nowrap>Service Level Agreement:</td>
        <td width="100%" class="footer"><b><asp:Label ID="lblDeliverable" runat="server" CssClass="bold" /> days</b> (based on priority)</td>
    </tr>
    <tr>
        <td nowrap>Priority:<font class="required">&nbsp;*</font></td>
        <td width="100%">
            <asp:RadioButtonList ID="radPriority" runat="server" CssClass="default" RepeatDirection="Horizontal">
                <asp:ListItem Value="1" />
                <asp:ListItem Value="2" />
                <asp:ListItem Value="3" />
                <asp:ListItem Value="4" />
                <asp:ListItem Value="5" />
            </asp:RadioButtonList>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="footer">Highest&nbsp;&nbsp;<img src="/images/small_arrow_right.gif" border="0" align="absmiddle" />&nbsp;&nbsp;Lowest</span>
        </td>
    </tr>
    <tr>
        <td nowrap>Estimated Start Date:<font class="required">&nbsp;*</font></td>
        <td width="100%"><asp:TextBox ID="txtStart" runat="server" CssClass="default" Width="100" MaxLength="10" /> <asp:ImageButton ID="imgStart" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
    </tr>
    <tr>
        <td nowrap>Estimated End Date:<font class="required">&nbsp;*</font></td>
        <td width="100%"><asp:TextBox ID="txtEnd" runat="server" CssClass="default" Width="100" MaxLength="10" /> <asp:ImageButton ID="imgEnd" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
    </tr>
    <tr>
        <td nowrap>Expedite this Request:<font class="required">&nbsp;*</font></td>
        <td width="100%">
            <asp:RadioButton ID="chkExpedite_Yes" runat="server" Text="Yes" CssClass="default" GroupName="TPM" />&nbsp;
            <asp:RadioButton ID="chkExpedite_No" runat="server" Text="No" CssClass="default" GroupName="TPM" />
        </td>
    </tr>
    <tr>
        <td colspan="2"><img src="/images/spacer.gif" border="0" height="1" width="1" /></td>
    </tr>
    <tr>
        <td nowrap>Articles / Documentation:</td>
        <td width="100%"><asp:Button ID="btnDocuments" runat="server" Text="Click to Upload" Width="125" CssClass="default" /></td>
    </tr>
    <asp:Panel ID="panCustom" runat="server" CssClass="default">
        <tr>
            <td colspan="2"><span style="width:100%;border-bottom:1 dotted #CCCCCC;"/></td>
        </tr>
        <tr>
            <td colspan="2"><%=strCustom %></td>
        </tr>
    </asp:Panel>
    <tr>
        <td colspan="2"><hr size="1" noshade /></td>
    </tr>
    <tr>
        <td colspan="2">
            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td>
                        <table cellpadding="4" cellspacing="4" border="0">
                            <tr>
                                <td><asp:Button ID="btnBack" runat="server" OnClick="btnBack_Click" CssClass="default" Text="<<  Back" Width="100" /></td>
                                <td>
                                    <asp:Panel ID="panDeliverable" runat="server" Visible="false">
                                        <asp:Button ID="btnDeliverable" runat="server" CssClass="default" Text="Service Deliverables" Width="150" />
                                    </asp:Panel>
                                </td>
                                <td><asp:Button ID="btnNext" runat="server" OnClick="btnNext_Click" CssClass="default" Text="Next  >>" Width="100" /></td>
                            </tr>
                        </table>
                    </td>
                    <td align="right">
                        <table cellpadding="4" cellspacing="4" border="0">
                            <tr>
                                <td><asp:Button ID="btnCancel1" runat="server" OnClick="btnCancel_Click" CssClass="default" Text="Cancel Request" Width="125" /></td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
</asp:Panel>
<asp:Label ID="lblItem" runat="server" Visible="false" />
<asp:Label ID="lblNumber" runat="server" Visible="false" />
<asp:Label ID="lblService" runat="server" Visible="false" />
<asp:Label ID="lblPRequest" runat="server" Visible="false" />
<asp:HiddenField ID="hdnEnd" runat="server" />