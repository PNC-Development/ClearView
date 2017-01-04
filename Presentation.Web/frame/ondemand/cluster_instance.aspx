<%@ Page Language="C#" MasterPageFile="~/window.Master" AutoEventWireup="true" CodeBehind="cluster_instance.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.cluster_instance" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" runat="server">
<script type="text/javascript">
    var boolStoreChange = false;
    function EnsureTextbox() {
        boolStoreChange = false;
        var boolReturn = true;
        var oTexts = document.getElementsByName("ValidTextbox");
        var arrIllegal = new Array("","/","/USR","/VAR","/TMP","/HOME","/OPT","/OPT/PATROL","/EXPORT","/BOOT","/USR/LOCAL");
        for (var ii=0; ii<oTexts.length && boolReturn == true; ii++) {
            if (oTexts[ii].disabled == false) {
                if (oTexts[ii].value.charAt(0) == '/') {
                    for (var jj = 0; jj < arrIllegal.length; jj++) {
                        if (arrIllegal[jj].toUpperCase() == oTexts[ii].value.toUpperCase()) {
                            boolReturn = false;
                            alert(arrIllegal[jj].toUpperCase() + ' is an invalid path');
                            oTexts[ii].focus();
                        }
                    }
                }
                else {
                    boolReturn = false;
                    alert('The path must start with \"/\"');
                    oTexts[ii].focus();
                }
            }
        }
        if (boolReturn == true) {
            var arrPaths = new Array(<%=strPaths %>);
            for (var jj = 0; jj < arrPaths.length; jj++) {
                var oFound = false;
                for (var ii=0; ii<oTexts.length && boolReturn == true; ii++) {
                    if (oTexts[ii].disabled == false) {
                        if (arrPaths[jj].toUpperCase() == oTexts[ii].value.toUpperCase()) {
                            if (oFound == false)
                                oFound = true;
                            else {
                                boolReturn = false;
                                alert(arrPaths[jj].toUpperCase() + ' is already configured');
                                oTexts[ii].focus();
                            }
                        }
                    }
                }
            }
        }
        return boolReturn;
    }
    
     
     function EnsureValidText() {
        boolStoreChange = false;
        var boolReturn = true;
        var oTexts = document.getElementsByName("ValidTextbox");
        for (var ii=0; ii<oTexts.length && boolReturn == true; ii++) {
            if (oTexts[ii].disabled == false) {
                if (oTexts[ii].value == "") {
                    boolReturn = false;
                    alert('Please enter a valid path');
                    oTexts[ii].focus();
                }
            }
        }
        return boolReturn;
    }
    
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
    function UpdateTextPath(oText, oHidden) {
        boolStoreChange = true;
        oHidden = document.getElementById(oHidden);
        oHidden.value = oText.value;
    }
    function BlurPath(oText, oHidden, strPath) {
        oHidden = document.getElementById(oHidden);
        if (oText.value == "") {
            oText.value = strPath;
            oHidden.value = oText.value;
        }
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
                    <td rowspan="2"><img src="/images/cluster_instance.gif" border="0" align="middle" /></td>
                    <td class="header" width="100%" valign="bottom">Cluster Instance Configuration</td>
                </tr>
                <tr>
                    <td width="100%" valign="top">Using this page, you can configure your cluster instance.</td>
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
                        <table width="100%" cellpadding="4" cellspacing="0" border="0">
                            <tr>
                                <td colspan="3">Nickname:</td>
                            </tr>
                            <tr>
                                <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
                                <td><asp:TextBox ID="txtName" runat="server" CssClass="default" Width="200" MaxLength="50" /></td>
                                <td width="100%" align="right"><asp:Button ID="btnAdd" runat="server" CssClass="default" Width="150" Text="Add Mount Point" OnClick="btnAdd_Click" /></td>
                            </tr>
                        </table>
                        <br />
                        <table width="100%" cellpadding="3" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                            <tr bgcolor="#EEEEEE">
                                <td><b><u>Lun #:</u></b></td>
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
                                <td class="required">* = Required Field</td>
                                <td align ="center"><asp:Button ID="btnStorageOverride" runat="server" Text="Storage Override" CssClass="default" Width="120" /> </td>
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
<asp:Label ID="lblLun" runat="server" Visible="false" />
</asp:Content>
