<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="rr_tpm.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.rr_tpm" %>



<table width="100%" border="0" cellSpacing="2" cellPadding="4" class="default">
    <tr>
        <td colspan="2" class="header"><asp:Label ID="lblHeader" runat="server" CssClass="header" /></td>
    </tr>
    <tr>
        <td nowrap>Executive Sponsor:<font class="required">&nbsp;*</font></td>
        <td width="100%">
            <table cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td><asp:TextBox ID="txtExecutive" runat="server" Width="300" CssClass="default" /></td>
                </tr>
                <tr>
                    <td>
                        <div id="divExecutive" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                            <asp:ListBox ID="lstExecutive" runat="server" CssClass="default" />
                        </div>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td nowrap>Working Sponsor:<font class="required">&nbsp;*</font></td>
        <td width="100%">
            <table cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td><asp:TextBox ID="txtWorking" runat="server" Width="300" CssClass="default" /></td>
                </tr>
                <tr>
                    <td>
                        <div id="divWorking" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                            <asp:ListBox ID="lstWorking" runat="server" CssClass="default" />
                        </div>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td nowrap>Priority:<font class="required">&nbsp;*</font></td>
        <td width="100%">
            <asp:RadioButtonList ID="ddlPriority" runat="server" CssClass="default" RepeatDirection="Horizontal">
                <asp:ListItem Value="1" Text="1 (High)" />
                <asp:ListItem Value="2" />
                <asp:ListItem Value="3" Selected="True" />
                <asp:ListItem Value="4" />
                <asp:ListItem Value="5" Text="5 (Low)" />
            </asp:RadioButtonList>
        </td>
    </tr>
    <tr>
        <td nowrap valign="top">Statement of Work:<font class="required">&nbsp;*</font></td>
        <td width="100%" valign="top"><asp:TextBox ID="txtStatement" runat="server" CssClass="default" TextMode="MultiLine" Rows="5" Width="250" /></td>
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
<asp:HiddenField ID="hdnWorking" runat="server" />
<asp:HiddenField ID="hdnExecutive" runat="server" />