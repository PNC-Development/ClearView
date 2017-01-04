<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="workstation_virtual_control.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.workstation_virtual_control" %>

<script type="text/javascript">
</script>
<asp:Panel ID="panBegin" runat="server" Visible="false">
<table width="100%" height="100%" cellpadding="4" cellspacing="2" border="0">
    <tr height="1">
        <td colspan="2"><b>You are now ready to start the Auto-Provisioning Process...</b></td>
    </tr>
    <tr height="1">
        <td nowrap><img src="/images/spacer.gif" border="0" width="50" height="1" /></td>
        <td width="100%">
            <table width="100%" cellpadding="2" cellspacing="2" border="0">
                <tr>
                    <td rowspan="2"><img src="/images/step_1.gif" border="0" align="absmiddle" /></td>
                    <td class="bigblue" width="100%" valign="bottom">Start the Build Now</td>
                </tr>
                <tr>
                    <td valign="top">Click <b>Start the Build</b> to start your auto-provisioning build.</td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td><asp:Button ID="btnStart" runat="server" OnClick="btnStart_Click" Text="Start the Build" CssClass="default" Width="125" /></td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td> <asp:Label ID="lblNotify" runat="server" CssClass="reddefault" Text="<b>BURN-IN PROCESS:</b> Upon clicking this button, the design implementor will be notified of your request." Visible="false" /></td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td colspan="2">&nbsp;</td>
    </tr>
</table>
</asp:Panel>
<asp:Panel ID="panStart" runat="server" Visible="false">
<table width="100%" height="100%" cellpadding="4" cellspacing="2" border="0">
    <tr height="1">
        <td valign="top">
                <%=strMenuTab1 %>
	            <div id="divMenu1"> 
	            <%=strDivs %>
	            </div> 
	    </td>
    </tr>
    <!-- 
    <tr height="1">
        <td valign="top"><%=strMenuTab1 %></td>
    </tr>
    <tr>
        <td valign="top"><%=strDivs %></td>
    </tr>
    -->
</table>
</asp:Panel>