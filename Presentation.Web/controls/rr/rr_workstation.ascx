<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="rr_workstation.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.rr_workstation" %>


<table width="100%" border="0" cellSpacing="2" cellPadding="4" class="default">
    <tr>
        <td nowrap>Project Coordinator:<font class="required">&nbsp;*</font></td>
        <td width="100%">
            <table cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td><asp:TextBox ID="txtManager" runat="server" Width="300" CssClass="default" /></td>
                </tr>
                <tr>
                    <td>
                        <div id="divManager" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                            <asp:ListBox ID="lstManager" runat="server" CssClass="default" />
                        </div>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td nowrap>Integration Engineer / Technical Lead:<font class="required">&nbsp;*</font></td>
        <td width="100%">
            <table cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td><asp:TextBox ID="txtEngineer" runat="server" Width="300" CssClass="default" /></td>
                </tr>
                <tr>
                    <td>
                        <div id="divEngineer" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                            <asp:ListBox ID="lstEngineer" runat="server" CssClass="default" />
                        </div>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td nowrap>Reason:<font class="required">&nbsp;*</font></td>
        <td width="100%">
            <asp:DropDownList ID="ddlReason" runat="server" CssClass="default">
                <asp:ListItem Value="-- SELECT --" />
                <asp:ListItem Value="New Project" />
                <asp:ListItem Value="Break-Fix" />
                <asp:ListItem Value="Certification" />
                <asp:ListItem Value="Decommission" />
                <asp:ListItem Value="Rebuild" />
                <asp:ListItem Value="Other..." />
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td nowrap valign="top">Special Instructions:</td>
        <td width="100%" valign="top"><asp:TextBox ID="txtStatement" runat="server" CssClass="default" TextMode="MultiLine" Rows="5" Width="250" /></td>
    </tr>
    <tr>
        <td nowrap>Articles / Documentation:</td>
        <td width="100%"><img src="/images/upload.gif" border="0" align="absmiddle" /> <asp:LinkButton ID="btnDocuments" runat="server" Text="Click Here to Upload" /></td>
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
<asp:Label ID="lblItem" runat="server" Visible="false" />
<asp:Label ID="lblNumber" runat="server" Visible="false" />
<asp:Label ID="lblService" runat="server" Visible="false" />
<asp:HiddenField ID="hdnLead" runat="server" />
<asp:HiddenField ID="hdnTechnical" runat="server" />