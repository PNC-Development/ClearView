<%@ Page Language="C#" MasterPageFile="~/window.Master" AutoEventWireup="true" CodeBehind="config_server_f_drive.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.config_server_f_drive" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" runat="server">
<script type="text/javascript">
    function UpdateDDL(oDDL, oNumber) {
        oNumber = document.getElementById(oNumber);
        oNumber.value = oDDL.options[oDDL.selectedIndex].value;
    }
    function UpdateText(oText, oNumber) {
        oNumber = document.getElementById(oNumber);
        if (oText.value == "" || isNumber(oText.value) == false)
            oText.value = 0;
        oNumber.value = oText.value;
    }
</script>
<table width="100%" height="100%" cellpadding="4" cellspacing="2" border="0">
    <tr height="1">
        <td colspan="2">
            <table width="100%" cellpadding="0" cellspacing="5" border="0">
                <tr>
                    <td rowspan="2"><img src="/images/cluster_quorum.gif" border="0" align="middle" /></td>
                    <td class="header" width="100%" valign="bottom">F:\ Drive Configuration</td>
                </tr>
                <tr>
                    <td width="100%" valign="top">Using this page, you can configure the F:\ Drive of your server.</td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <table width="100%" height="100%" cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td valign="top">
                        <table width="100%" cellpadding="3" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                            <tr bgcolor="#EEEEEE">
                                <td><b><u>Lun Number:</u></b></td>
                                <td><b><u>Path:</u></b></td>
                                <td><b><u>Performance:</u></b></td>
                                <td><b><u>Amount:</u></b></td>
                                <td><b><u>Amount in Test:</u></b></td>
                                <td><b><u>Replicated:</u></b></td>
                                <td><b><u>High Availability:</u></b></td>
                            </tr>
                            <%=strSQL %>
                            <%=strHidden %>
                        </table>
                    </td>
                </tr>
                <tr height="1">
                    <td>
                        <table width="100%" cellpadding="4" cellspacing="2" border="0">
                            <tr>
                                <td colspan="3"><hr size="1" noshade /></td>
                            </tr>
                            <tr>
                                <td class="required"></td>
                                <td align="center">
                                </td>
                                <td align="right">
                                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="default" Width="75" OnClick="btnSave_Click" /> 
                                    <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="default" Width="75" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
</asp:Content>
