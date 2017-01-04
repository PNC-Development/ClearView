<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="workstations_virtual.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.workstations_virtual" %>


<script type="text/javascript">
    function OpenOnDemandDevice(intAnswer, intNum) {
        return OpenWindow('ONDEMAND_VIRTUALWORKSTATION','?aid=' + intAnswer + '&num=' + intNum);
    }
</script>
<table width="100%" height="100%" cellpadding="0" cellspacing="0" border="0">
    <tr>
        <td valign="top">
            <div style="height:100%; overflow:auto">
            <table width="100%" cellpadding="3" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                <tr bgcolor="#EEEEEE">
                    <td width="33%"><b><u>Model:</u></b></td>
                    <td width="33%"><b><u>Nickname:</u></b></td>
                    <td width="33%"><b><u>Count:</u></b></td>
                    <td nowrap align="center"><b><u>Device Config:</u></b></td>
                </tr>
                <%=strDevices %>
            </table>
            </div>
        </td>
    </tr>
    <tr height="1">
        <td>
            <table width="100%" cellpadding="4" cellspacing="0" border="0">
                <tr>
                    <td align="center" class="bigger">
                        <asp:Panel ID="panInvalid" runat="server" Visible="false">
                            <img src="/images/bigAlert.gif" border="0" align="absmiddle" /> <b>Invalid Configuration - Please Correct to Continue</b>
                        </asp:Panel>
                        <asp:Panel ID="panValid" runat="server" Visible="false">
                            <img src="/images/bigCheck.gif" border="0" align="absmiddle" /> <b>Valid Configuration - Ready to Start!</b>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
            <table width="100%" cellpadding="4" cellspacing="2" border="0">
                <tr>
                    <td colspan="3"><hr size="1" noshade /></td>
                </tr>
                <tr>
                    <td class="required">* = Required Field</td>
                    <td align="center">
                        <asp:Panel ID="panNavigation" runat="server" Visible="false">
                            <asp:Button ID="btnBack" runat="server" OnClick="btnBack_Click" Text="Back" CssClass="default" Width="75" /> <asp:Button ID="btnNext" runat="server" OnClick="btnNext_Click" Text="Next" CssClass="default" Width="75" />
                        </asp:Panel>
                        <asp:Panel ID="panUpdate" runat="server" Visible="false">
                            <asp:Button ID="btnUpdate" runat="server" OnClick="btnUpdate_Click" Text="Update" CssClass="default" Width="75" /> <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" Text="Cancel" CssClass="default" Width="75" />
                        </asp:Panel>
                    </td>
                    <td align="right"><asp:Button ID="btnClose" runat="server" Text="Finish Later" CssClass="default" Width="125" /></td>
                </tr>
            </table>
        </td>
    </tr>
</table>
