<%@ Page Language="C#" MasterPageFile="~/window.Master" AutoEventWireup="true" CodeBehind="cluster_instance_config.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.cluster_instance_config" Title="Untitled Page" %>
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
                    <td rowspan="2"><img src="/images/cluster_instance_quorum.gif" border="0" align="middle" /></td>
                    <td class="header" width="100%" valign="bottom">Cluster Instance Storage Configuration</td>
                </tr>
                <tr>
                    <td width="100%" valign="top">Using this page, you can configure the storage of an instance of your cluster.</td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:Panel ID="panView" runat="server" Visible="false">
            <table width="100%" height="100%" cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td valign="top">
                        <table width="100%" cellpadding="3" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                            <tr bgcolor="#EEEEEE">
                                <td><b><u>Lun #:</u></b></td>
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
            </asp:Panel>
            <asp:Panel ID="panDenied" runat="server" Visible="false">
                <table width="100%" cellpadding="2" cellspacing="2" border="0">
                    <tr>
                        <td><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                        <td class="header"><img src="/images/bigX.gif" border="0" align="absmiddle" /> Access Denied</td>
                    </tr>
                    <tr><td colspan="2">&nbsp;</td></tr>
                    <tr>
                        <td><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                        <td>You do not have rights to view this item.</td>
                    </tr>
                    <tr><td colspan="2">&nbsp;</td></tr>
                    <tr>
                        <td colspan="2"><hr size="1" noshade /></td>
                    </tr>
                    <tr>
                        <td colspan="2" align="right">
                            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                <tr>
                                    <td class="footer"></td>
                                    <td align="right"><asp:Button ID="btnDenied" runat="server" CssClass="default" Width="75" Text="Close" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </td>
    </tr>
</table>
</asp:Content>
