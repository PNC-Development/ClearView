<%@ Page Language="C#" MasterPageFile="~/window.Master" AutoEventWireup="true" CodeBehind="cluster_quorum.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.cluster_quorum" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" runat="server">
<script type="text/javascript">
    var boolStoreChange = false;
    function EnsureTextbox0() {
        boolStoreChange = false;
        var boolReturn = true;
        var oTexts = document.getElementsByName("ValidTextbox0");
        for (var ii=0; ii<oTexts.length && boolReturn == true; ii++) {
            if (oTexts[ii].disabled == false) {
                if (oTexts[ii].value == "" || isNumber(oTexts[ii].value) == false || parseInt(oTexts[ii].value) <= 0) {
                    boolReturn = false;
                    alert('Please enter a valid number');
                    oTexts[ii].focus();
                }
            }
        }
        return boolReturn;
    }
    function UpdateDDL(oDDL, oNumber) {
        boolStoreChange = true;
        oNumber = document.getElementById(oNumber);
        oNumber.value = oDDL.options[oDDL.selectedIndex].value;
    }
    function UpdateText(oText, oNumber) {
        boolStoreChange = true;
        oNumber = document.getElementById(oNumber);
        oNumber.value = oText.value;
    }
    function CatchClose() {
        window.onbeforeunload = CatchClose2;
    }
    function CatchClose2() {
        if (boolStoreChange == true)
            return "WARNING: You have made changes without saving!";
    }
</script>
<table width="100%" height="100%" cellpadding="4" cellspacing="2" border="0">
    <tr height="1">
        <td colspan="2">
            <table width="100%" cellpadding="0" cellspacing="5" border="0">
                <tr>
                    <td rowspan="2"><img src="/images/cluster_quorum.gif" border="0" align="middle" /></td>
                    <td class="header" width="100%" valign="bottom">Cluster Quorum Configuration</td>
                </tr>
                <tr>
                    <td width="100%" valign="top">Using this page, you can configure the quorum of your cluster.</td>
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
                                <td><b><u>Lun Number:</u></b></td>
                                <td><b><u>Path:</u></b></td>
                                <td><b><u>Performance:</u></b></td>
                                <td><b><u>Amount in Prod:</u></b></td>
                                <td><b><u>Amount in QA:</u></b></td>
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
        </td>
    </tr>
</table>
</asp:Content>
