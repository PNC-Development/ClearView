<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="rr_virtual_workstation_vmware.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.rr_virtual_workstation_vmware" %>


<script type="text/javascript">
</script>
<asp:Panel ID="pnlReqDenied" runat="server" CssClass="default" Visible="false">
    <table width="100%" cellpadding="0" cellspacing="5" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
        <tr>
            <td rowspan="2" valign="top"><img src="/images/bigAlert.gif" border="0" align="absmiddle" /></td>
            <td class="header" valign="bottom" width="100%"><asp:Label ID="lblReqDeny" runat="server" CssClass="header" Text="Your service request has been denied for the following reason..." /></td>
        </tr>
        <tr>
            <td valign="bottom">
                <table cellpadding="0" cellspacing="5" border="0">
                    <tr>
                        <td><asp:Label ID="lblReqDenyComment" runat="server" CssClass="default" Text="Comments : " /></td>
                        <td valign="top"><asp:Label ID="lblReqDenyCommentValue" runat="server" CssClass="default" Text="" /></td>
                    </tr>
                </table>
                <p class="biggerbold"><img src="/images/arrow_green_right.gif" border="0" align="absmiddle" /> Please modify the request and click the &quot;Update&quot; button</p>
            </td>
        </tr>
    </table>
    <br />
</asp:Panel>
<table width="100%" border="0" cellSpacing="2" cellPadding="4" class="default">
    <tr>
        <td colspan="2">
            <table width="100%" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
                <tr>
                    <td nowrap><img src="/images/bigAlert.gif" border="0" align="absmiddle" /></td>
                    <td width="100%"><b>NOTE:</b> You cannot attach a physical device to a virtual workstation. If your initiative requires special hardware, do not choose a virtual workstation.</td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td class="required">* = Required Field</td>
        <td align="right"><asp:LinkButton ID="btnDeliverable" runat="server" CssClass="default" Text="Service Deliverables" Visible="false" /></td>
    </tr>
    <tr>
        <td nowrap>Nickname:</td>
        <td width="100%"><asp:TextBox ID="txtName" runat="server" CssClass="default" Width="200" MaxLength="50" /></td>
    </tr>
    <tr>
        <td nowrap>Location:<font class="required">&nbsp;*</font></td>
        <td width="100%">
            <asp:DropDownList ID="ddlLocation" runat="server" CssClass="default">
                <asp:ListItem Value="1675" Text="Summit Data Center" />
                <asp:ListItem Value="715" Text="Cleveland Data Center" />
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td nowrap>Class:<font class="required">&nbsp;*</font></td>
        <td width="100%"><asp:DropDownList ID="ddlClass" CssClass="default" runat="server" Width="300" /></td>
    </tr>
    <tr>
        <td nowrap>Quantity:<font class="required">&nbsp;*</font></td>
        <td width="100%"><asp:TextBox ID="txtQuantity" runat="server" CssClass="default" Width="80" MaxLength="10" /></td>
    </tr>
    <tr>
        <td nowrap>Operating System:<font class="required">&nbsp;*</font></td>
        <td width="100%"><asp:DropDownList ID="ddlOS" runat="server" CssClass="default" Width="300" AutoPostBack="true" OnSelectedIndexChanged="ddlOS_Change" /></td>
    </tr>
    <tr>
        <td nowrap>Employee Type:<font class="required">&nbsp;*</font></td>
        <td width="100%">
            <asp:RadioButtonList ID="radEmployee" runat="Server" RepeatDirection="Horizontal">
                <asp:ListItem Text="Internal User (PNC Employee, Onshore Contractor)" Value="1" />
                <asp:ListItem Text="External User (Offshore User)" Value="0" />
            </asp:RadioButtonList>
        </td>
    </tr>
    <tr>
        <td nowrap></td>
        <td width="100%"><asp:CheckBox ID="chkDR" runat="server" Text="This workstation will be used as part of the DR testing" /></td>
    </tr>
    <tr>
        <td nowrap>RAM:<font class="required">&nbsp;*</font></td>
        <td width="100%">
            <asp:DropDownList ID="ddlRam" runat="server" CssClass="default" Width="300" Enabled="false" >
                <asp:ListItem Value="-- Select an Operating System --" />
            </asp:DropDownList> GB
        </td>
    </tr>
    <tr>
        <td nowrap>CPUs:<font class="required">&nbsp;*</font></td>
        <td width="100%">
            <asp:DropDownList ID="ddlCPU" runat="server" CssClass="default" Width="300" Enabled="false" >
                <asp:ListItem Value="-- Select an Operating System --" />
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td nowrap>Hard Drive:<font class="required">&nbsp;*</font></td>
        <td width="100%">
            <asp:DropDownList ID="ddlHardDrive" runat="server" CssClass="default" Width="300" Enabled="false" >
                <asp:ListItem Value="-- Select an Operating System --" />
            </asp:DropDownList> GB
        </td>
    </tr>
</table>
<table width="100%" border="0" cellSpacing="2" cellPadding="4" class="default">
    <tr>
        <td colspan="2"><hr size="1" noshade /></td>
    </tr>
    <tr>
        <td>
            <asp:Panel ID="panNavigation" runat="server" Visible="false">
                <asp:Button ID="btnBack" runat="server" OnClick="btnBack_Click" CssClass="default" Text="<<  Back" Width="100" /> 
                <asp:Button ID="btnNext" runat="server" OnClick="btnNext_Click" CssClass="default" Text="Next  >>" Width="100" /> 
            </asp:Panel>
            <asp:Panel ID="panUpdate" runat="server" Visible="false">
                <asp:Button ID="btnUpdate" runat="server" OnClick="btnUpdate_Click" CssClass="default" Text="Update" Width="100" /> 
                <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" CssClass="default" Text="Cancel" Width="100" /> 
            </asp:Panel>
        </td>
        <td align="right">
            <asp:Button ID="btnCancelR" runat="server" OnClick="btnCancelR_Click" CssClass="default" Text="Cancel Request" Width="125" />
        </td>
    </tr>
</table>
<asp:Label ID="lblItem" runat="server" Visible="false" />
<asp:Label ID="lblNumber" runat="server" Visible="false" />
<asp:Label ID="lblService" runat="server" Visible="false" />
