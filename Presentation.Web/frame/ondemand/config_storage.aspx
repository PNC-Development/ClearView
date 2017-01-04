<%@ Page Language="C#" MasterPageFile="~/window.Master" AutoEventWireup="true" CodeBehind="config_storage.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.config_storage" Title="Untitled Page" %>
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
        var boolWarning = null;
        var oTexts = document.getElementsByName("ValidTextbox0");
        for (var ii=0; ii<oTexts.length && boolReturn == true; ii++) {
            if (oTexts[ii].disabled == false) {
                if (oTexts[ii].value == "" || isInt(oTexts[ii].value) == false || parseInt(oTexts[ii].value) <= 0) {
                    boolReturn = false;
                    alert('Please enter a valid WHOLE number (no decimals) between 1 and 750');
                    oTexts[ii].focus();
                }
                else if (parseInt(oTexts[ii].value) > 750) {
                    boolWarning = oTexts[ii];
                }
            }
        }
        if (boolWarning != null) {
            if (confirm('One or more of the amounts entered are outside the standard permitted range for SAN data LUNs\nPermitted Range: 1 GB - 750 GB\n\nNOTE: This is acceptable for SQL Log LUNs\n\nAre you sure you want to continue?') == false) {
                boolReturn = false;
                boolWarning.focus();
            }
        }
        return boolReturn;
    }
    function UpdateDDL(oDDL, oNumber) {
        oNumber = document.getElementById(oNumber);
        if (oNumber.value != oDDL.options[oDDL.selectedIndex].value)
        {
            boolStoreChange = true;
            oNumber.value = oDDL.options[oDDL.selectedIndex].value;
        }
    }
    function UpdateText(oText, oNumber) {
        oNumber = document.getElementById(oNumber);
        if (oNumber.value != oText.value)
        {
            boolStoreChange = true;
            oNumber.value = oText.value;
        }
    }
    function UpdateTextPath(oText, oHidden) {
        oHidden = document.getElementById(oHidden);
        if (oHidden.value != oText.value)
        {
            boolStoreChange = true;
            oHidden.value = oText.value;
        }
    }
    function BlurPath(oText, oHidden, strPath) {
        oHidden = document.getElementById(oHidden);
        if (oText.value == "") {
            oText.value = strPath;
            oHidden.value = oText.value;
        }
    }
    function EnsureDatabase(oYes, oNo, oSize, oQA, oTest, oNonYes, oNonNo, oNon, oNonQA, oNonTest, oLargest, oTempDBYes, oTempDBNo, oTempDB) {
        oYes = document.getElementById(oYes);
        oNo = document.getElementById(oNo);
        if (oYes.checked == false && oNo.checked == false) {
            alert('Please select if this server has locally attached storage'); 
            oYes.focus();
            return false;
        }
        else {
            if (oYes.checked == true) {
                if (ValidateNumber0(oSize, 'Please enter a valid value for the database size') == false || ValidateNumber0(oQA, 'Please enter a valid value for the database size in QA') == false || ValidateNumber0(oTest, 'Please enter a valid value for the database size in TEST') == false)
                    return false;
                else {
                    if (oLargest != null) {
                        if (ValidateNumber0(oLargest, 'Please enter a valid whole number percentage for the total space of the largest Table and/or Index') == false)
                            return false;
                    }
                    oTempDBYes = document.getElementById(oTempDBYes);
                    oTempDBNo = document.getElementById(oTempDBNo);
                    if (oTempDBYes.checked == false && oTempDBNo.checked == false) {
                        alert('Please select an answer to if you are using a non-standard TempDB size'); 
                        oTempDBYes.focus();
                        return false;
                    }
                    else if (oTempDBYes.checked == true && oTempDB != null) {
                        if (ValidateNumber0(oTempDB, 'Please enter a valid number for the amount of storage of TempDB') == false)
                            return false;
                    }
                    oNonYes = document.getElementById(oNonYes);
                    oNonNo = document.getElementById(oNonNo);
                    if (oNonYes.checked == false && oNonNo.checked == false) {
                        alert('Please select an answer to if the same instance will store non-database data'); 
                        oNonYes.focus();
                        return false;
                    }
                    else if (oNonYes.checked == true) {
                        if (ValidateNumber0(oNon, 'Please enter a valid number for the amount of storage to store non-database data') == false || ValidateNumber0(oNonQA, 'Please enter a valid number for the amount of storage to store non-database data in QA') == false || ValidateNumber0(oNonTest, 'Please enter a valid number for the amount of storage to store non-database data in TEST') == false)
                            return false;
                    }
                }
            }
        }
        return true;
    }
    function CatchClose() {
        window.onbeforeunload = CatchClose2;
    }
    function CatchClose2() {
        if (boolStoreChange == true)
            return "WARNING: You have made changes without saving!";
    }
</script>
    <table width="100%" height="100%" cellpadding="0" cellspacing="0" border="0">
        <tr height="1">
            <td>
                <table width="100%" cellpadding="0" cellspacing="5" border="0">
                    <tr>
                        <td rowspan="2"><img src="/images/device_config.gif" border="0" align="middle" /></td>
                        <td class="header" width="100%" valign="bottom">Storage Configuration</td>
                    </tr>
                    <tr>
                        <td width="100%" valign="top">Please complete the following questions regarding SAN storage.</td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td valign="top">
                <div style="height:100%; overflow:auto">
                <table width="100%" cellpadding="4" cellspacing="0" border="0">
                    <tr>
                        <td nowrap>Will this server have non-shared SAN storage?<font class="required">&nbsp;*</font></td>
                        <td width="100%">
                            <asp:RadioButton ID="radYes" runat="server" CssClass="default" Text="Yes" GroupName="storage" OnCheckedChanged="radYes_Check" AutoPostBack="true" /> 
                            <asp:RadioButton ID="radNo" runat="server" CssClass="default" Text="No" GroupName="storage" OnCheckedChanged="radNo_Check" AutoPostBack="true" />
                        </td>
                    </tr>
                </table>
                <asp:Panel ID="panFDrive" runat="server" Visible="false">
                <table width="100%" cellpadding="4" cellspacing="0" border="0">
                    <tr>
                        <td nowrap>Do you want your Application Drive (F: or E:) to be SAN attached?<font class="required">&nbsp;*</font></td>
                        <td width="100%">
                            <asp:RadioButton ID="radFYes" runat="server" CssClass="default" Text="Yes" GroupName="fdrive" OnCheckedChanged="radFYes_Check" AutoPostBack="true" /> 
                            <asp:RadioButton ID="radFNo" runat="server" CssClass="default" Text="No" GroupName="fdrive" OnCheckedChanged="radFNo_Check" AutoPostBack="true" />
                        </td>
                    </tr>
                </table>
                </asp:Panel>
                <asp:Panel ID="panSQL" runat="server" Visible="false">
                <table width="100%" cellpadding="4" cellspacing="0" border="0">
                    <tr>
                        <td nowrap>Will these servers have non-shared SQL instances?<font class="required">&nbsp;*</font></td>
                        <td width="100%">
                            <asp:RadioButton ID="radSQLYes" runat="server" CssClass="default" Text="Yes" GroupName="sql" OnCheckedChanged="radSQLYes_Check" AutoPostBack="true" /> 
                            <asp:RadioButton ID="radSQLNo" runat="server" CssClass="default" Text="No" GroupName="sql" OnCheckedChanged="radSQLNo_Check" AutoPostBack="true" />
                        </td>
                    </tr>
                </table>
                </asp:Panel>
                <asp:Panel ID="panStorageYes" runat="server" Visible="false">
                <asp:Panel ID="panDatabase" runat="server" Visible="false">
                    <table width="100%" cellpadding="4" cellspacing="0" border="0">
                        <tr>
                            <td colspan="2" class="biggerbold">SQL Server Storage Calculator (NCC)</td>
                        </tr>
                        <tr>
                            <td colspan="2">Enter database size in PROD:</td>
                        </tr>
                        <tr>
                            <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
                            <td width="100%"><asp:TextBox ID="txtSize" runat="server" CssClass="default" Width="100" MaxLength="10" /> GB</td>
                        </tr>
                        <tr>
                            <td colspan="2">Enter database size in QA:</td>
                        </tr>
                        <tr>
                            <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
                            <td width="100%"><asp:TextBox ID="txtQA" runat="server" CssClass="default" Width="100" MaxLength="10" /> GB</td>
                        </tr>
                        <tr>
                            <td colspan="2">Enter database size in TEST:</td>
                        </tr>
                        <tr>
                            <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
                            <td width="100%"><asp:TextBox ID="txtTest" runat="server" CssClass="default" Width="100" MaxLength="10" /> GB</td>
                        </tr>
                        <tr>
                            <td colspan="2">Will you require to store non-database data on the same instance?</td>
                        </tr>
                        <tr>
                            <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
                            <td width="100%">
                                <asp:RadioButton ID="radNonYes" runat="server" CssClass="default" Text="Yes" GroupName="non" /> 
                                <asp:RadioButton ID="radNonNo" runat="server" CssClass="default" Text="No" GroupName="non" /> 
                            </td>
                        </tr>
                    </table>
                    <div id="divNon" runat="server" style="display:none">
                    <table width="100%" cellpadding="4" cellspacing="0" border="0">
                        <tr>
                            <td colspan="2">Enter amount of storage required to store non-database data in PROD:</td>
                        </tr>
                        <tr>
                            <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
                            <td width="100%"><asp:TextBox ID="txtNon" runat="server" CssClass="default" Width="100" MaxLength="10" /> GB</td>
                        </tr>
                        <tr>
                            <td colspan="2">Enter amount of storage required to store non-database data in QA:</td>
                        </tr>
                        <tr>
                            <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
                            <td width="100%"><asp:TextBox ID="txtNonQA" runat="server" CssClass="default" Width="100" MaxLength="10" /> GB</td>
                        </tr>
                        <tr>
                            <td colspan="2">Enter amount of storage required to store non-database data in TEST:</td>
                        </tr>
                        <tr>
                            <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
                            <td width="100%"><asp:TextBox ID="txtNonTest" runat="server" CssClass="default" Width="100" MaxLength="10" /> GB</td>
                        </tr>
                    </table>
                    </div>
                </asp:Panel>
                <asp:Panel ID="panDatabasePNC" runat="server" Visible="false">
                    <table width="100%" cellpadding="4" cellspacing="0" border="0">
                        <tr>
                            <td colspan="2" class="biggerbold">SQL Server Storage Calculator (PNC)</td>
                        </tr>
                        <tr>
                            <td colspan="2">Enter the total database size:</td>
                        </tr>
                        <tr>
                            <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
                            <td width="100%"><asp:TextBox ID="txtSizePNC" runat="server" CssClass="default" Width="100" MaxLength="10" /> GB</td>
                        </tr>
                        <tr>
                            <td colspan="2">What % of the total space is the largest Table and/or Index:</td>
                        </tr>
                        <tr>
                            <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
                            <td width="100%"><asp:TextBox ID="txtPercentPNC" runat="server" CssClass="default" Width="100" MaxLength="10" /> %&nbsp;&nbsp;&nbsp;<span class="footer">(0 - 100)</span></td>
                        </tr>
                        <tr>
                            <td colspan="2">Are you using a non-standard TempDB size?</td>
                        </tr>
                        <tr>
                            <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
                            <td width="100%">
                                <asp:RadioButton ID="radTempDBYes" runat="server" CssClass="default" Text="Yes" GroupName="TempDB" /> 
                                <asp:RadioButton ID="radTempDBNo" runat="server" CssClass="default" Text="No (Default)" GroupName="TempDB" Checked="true" /> 
                            </td>
                        </tr>
                    </table>
                    <div id="divTempDB" runat="server" style="display:none">
                    <table width="100%" cellpadding="4" cellspacing="0" border="0">
                        <tr>
                            <td colspan="2">Please enter your custom TempDB size:</td>
                        </tr>
                        <tr>
                            <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
                            <td width="100%"><asp:TextBox ID="txtTempPNC" runat="server" CssClass="default" Width="100" MaxLength="10" /> GB</td>
                        </tr>
                    </table>
                    </div>
                    <table width="100%" cellpadding="4" cellspacing="0" border="0">
                        <tr>
                            <td colspan="2">Will you require to store non-database data on the same instance?</td>
                        </tr>
                        <tr>
                            <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
                            <td width="100%">
                                <asp:RadioButton ID="radNonPNCYes" runat="server" CssClass="default" Text="Yes" GroupName="nonPNC" /> 
                                <asp:RadioButton ID="radNonPNCNo" runat="server" CssClass="default" Text="No (Default)" GroupName="nonPNC" /> 
                            </td>
                        </tr>
                    </table>
                    <div id="divNonPNC" runat="server" style="display:none">
                    <table width="100%" cellpadding="4" cellspacing="0" border="0">
                        <tr>
                            <td colspan="2">Enter amount of storage required to store non-database data:</td>
                        </tr>
                        <tr>
                            <td><img src="/images/spacer.gif" border="0" width="30" height="1" /></td>
                            <td width="100%"><asp:TextBox ID="txtNonPNC" runat="server" CssClass="default" Width="100" MaxLength="10" /> GB</td>
                        </tr>
                    </table>
                    </div>
                </asp:Panel>
                <asp:Panel ID="panDatabaseNo" runat="server" Visible="false">
                    <table width="100%" cellpadding="4" cellspacing="0" border="0">
                        <tr>
                            <td><asp:Button ID="btnReset" runat="server" CssClass="default" Width="200" Text="Reset Database Configuration" OnClick="btnReset_Click" Visible="false" /></td>
                            <td align="right">
                                <asp:Button ID="btnDrive" runat="server" CssClass="default" Width="150" Text="Add a New Drive" OnClick="btnDrive_Click" />
                                <asp:Button ID="btnMount" runat="server" CssClass="default" Width="150" Text="Add Mount Point" OnClick="btnMount_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Panel ID="panGenerate" runat="server" Visible="false">
                                    <table width="100%" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
                                        <tr>
                                            <td nowrap><img src="/images/bigCheck.gif" border="0" align="absmiddle" /></td>
                                            <td width="100%"><b>NOTE:</b> ClearView has auto-generated your database LUN assignments based on your information.</td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <table width="100%" cellpadding="3" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                        <tr bgcolor="#EEEEEE">
                            <td><b><u>Lun #:</u></b></td>
                            <td><b><u>Path:</u></b></td>
                            <td><b><u>Performance:</u></b></td>
                            <%= (boolProd == true ? "<td><b><u>Amount in Prod:</u></b></td>" : "") %>
                            <%= (boolQA == true ? "<td><b><u>Amount in QA:</u></b></td>" : "") %>
                            <%= (boolTest == true ? "<td><b><u>Amount in Test:</u></b></td>" : "") %>
                            <td><b><u>Replicated:</u></b></td>
                            <td><b><u>High Availability:</u></b></td>
                        </tr>
                        <%=strSQL %>
                        <%=strHidden %>
                    </table>
                    <br />
                    <asp:Panel ID="panNotes" runat="server" Visible="false">
                        <table width="100%" cellpadding="4" cellspacing="0" border="0">
                            <tr>
                                <td class="redheader">NOTES:</td>
                            </tr>
                            <tr>
                                <td><asp:Label ID="lblNotes" runat="server" CssClass="reddefault" /></td>
                            </tr>
                        </table>
                    </asp:Panel>
                </asp:Panel>
                </asp:Panel>
                </div>
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
                            <asp:Button ID="btnSaveStorage" runat="server" Text="Save" CssClass="default" Width="75" OnClick="btnSaveStorage_Click" /> 
                            <asp:Button ID="btnSaveCloseStorage" runat="server" Text="Save & Close" CssClass="default" Width="125" OnClick="btnSaveCloseStorage_Click" /> 
                            <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="default" Width="75" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
<asp:Label ID="lblId" runat="server" Visible="false" />
<asp:Label ID="lblLun" runat="server" Visible="false" />
<asp:Label ID="lblPNC" runat="server" Visible="false" />
<asp:Label ID="lblDatabase" runat="server" Visible="false" />
</asp:Content>
