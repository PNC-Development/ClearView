<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="fore_storage_new.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.fore_storage_new" %>
<script type="text/javascript">
    var oTotalProd = 0;
    var oTotalQA = 0;
    var oTotalTest = 0;
    var oTotalProdOS = 0;
    var oTotalQAOS = 0;
    var oTotalTestOS = 0;
    function ResetStorage() {
        oTotalProd = 0;
        oTotalQA = 0;
        oTotalTest = 0;
        oTotalProdOS = 0;
        oTotalQAOS = 0;
        oTotalTestOS = 0;
        return true;
    }
    function EnsureStorage(oRad, o1, o2, o3, oHeader) {
        oRad = document.getElementById(oRad);
        if (oRad.checked == true) {
            o1 = document.getElementById(o1);
            o2 = document.getElementById(o2);
            o3 = document.getElementById(o3);
            if (o1.checked == false && o2.checked == false && o3.checked == false) {
                alert(oHeader + '\n\n' + 'Please select a type of storage');
                return false;
            }
        }
        return true;
    }
    function EnsureStorageNew(oRad, oCheck, oTextReq, oQA, oTest, oProd, oNoRep, oDDLRep, oTextRep, oHA, oDDLHA, oTextHA, oHeader) {
        oRad = document.getElementById(oRad);
        oCheck = document.getElementById(oCheck);
        if (oRad.checked == true && oCheck.checked == true) {
            if (ValidateNumber0(oTextReq, oHeader + '\n\n' + 'Please enter a valid number for the total amount of storage') == false)
                return false;
            else if (document.getElementById(oTextReq) != null && isNumber(document.getElementById(oTextReq).value))
            {
                var iAmount = parseInt(document.getElementById(oTextReq).value);
                if (iAmount < <%=intStoragePerBladeApp %>) {
                    alert(oHeader + '\n\n' + 'The minimum amount of storage that can be configured is <%=intStoragePerBladeApp %> GB.');
                    return false;
                }
                else
                    oTotalProd = oTotalProd + iAmount;
            }
            if (ValidateNumber(oQA, oHeader + '\n\n' + 'Please enter a valid number for the total amount of storage in QA') == false)
                return false;
            else if (document.getElementById(oQA) != null && isNumber(document.getElementById(oQA).value))
            {
                var iAmount = parseInt(document.getElementById(oQA).value);
                if (iAmount < <%=intStoragePerBladeApp %>) {
                    alert(oHeader + '\n\n' + 'The minimum amount of storage that can be configured is <%=intStoragePerBladeApp %> GB.');
                    return false;
                }
                else
                    oTotalQA = oTotalQA + iAmount;
            }
            if (ValidateNumber(oTest, oHeader + '\n\n' + 'Please enter a valid number for the total amount of storage in TEST') == false)
                return false;
            else if (document.getElementById(oTest) != null && isNumber(document.getElementById(oTest).value))
            {
                var iAmount = parseInt(document.getElementById(oTest).value);
                if (iAmount < <%=intStoragePerBladeApp %>) {
                    alert(oHeader + '\n\n' + 'The minimum amount of storage that can be configured is <%=intStoragePerBladeApp %> GB.');
                    return false;
                }
                else
                    oTotalTest = oTotalTest + iAmount;
            }
            if (oNoRep == false) {
                oDDLRep = document.getElementById(oDDLRep);
                if (oDDLRep != null && oDDLRep.selectedIndex == (oDDLRep.length - 1)) {
                    if (ValidateNumber0(oTextRep, oHeader + '\n\n' + 'Please enter a valid number for the amount of storage to be replicated') == false)
                        return false;
                    if (ValidateNumberGreater(oTextRep, oTextReq, oHeader + '\n\n' + 'The amount of replicated storage cannot exceed the total amount of storage') == false)
                        return false;
                }
            }
            if (oProd == true && oHA == true) {
                oDDLHA = document.getElementById(oDDLHA);
                if (oDDLHA != null && oDDLHA.selectedIndex == (oDDLHA.length - 1)) {
                    if (ValidateNumber0(oTextHA, oHeader + '\n\n' + 'Please enter a valid number for the amount of high availability storage') == false)
                        return false;
                    if (ValidateNumberGreater(oTextHA, oTextReq, oHeader + '\n\n' + 'The amount of high availability storage cannot exceed the total amount of storage') == false)
                        return false;
                }
            }
        }
        return true;
    }
    function EnsureStorageNewOS(oRad, oCheck, oTextReq, oQA, oTest, oProd, oNoRep, oDDLRep, oTextRep, oHA, oDDLHA, oTextHA, oHeader) {
        oRad = document.getElementById(oRad);
        oCheck = document.getElementById(oCheck);
        if (oRad.checked == true && oCheck.checked == true) {
            if (document.getElementById(oTextReq) != null && isNumber(document.getElementById(oTextReq).value))
                oTotalProdOS = oTotalProdOS + parseInt(document.getElementById(oTextReq).value);
            if (document.getElementById(oQA) != null && isNumber(document.getElementById(oQA).value))
                oTotalQAOS = oTotalQAOS + parseInt(document.getElementById(oQA).value);
            if (document.getElementById(oTest) != null && isNumber(document.getElementById(oTest).value))
                oTotalTestOS = oTotalTestOS + parseInt(document.getElementById(oTest).value);
            if (oNoRep == false) {
                oDDLRep = document.getElementById(oDDLRep);
                if (oDDLRep != null && oDDLRep.selectedIndex == (oDDLRep.length - 1)) {
                    if (ValidateNumber0(oTextRep, oHeader + '\n\n' + 'Please enter a valid number for the amount of storage to be replicated') == false)
                        return false;
                    if (ValidateNumberGreater(oTextRep, oTextReq, oHeader + '\n\n' + 'The amount of replicated storage cannot exceed the total amount of storage') == false)
                        return false;
                }
            }
            if (oProd == true && oHA == true) {
                oDDLHA = document.getElementById(oDDLHA);
                if (oDDLHA != null && oDDLHA.selectedIndex == (oDDLHA.length - 1)) {
                    if (ValidateNumber0(oTextHA, oHeader + '\n\n' + 'Please enter a valid number for the amount of high availability storage') == false)
                        return false;
                    if (ValidateNumberGreater(oTextHA, oTextReq, oHeader + '\n\n' + 'The amount of high availability storage cannot exceed the total amount of storage') == false)
                        return false;
                }
            }
        }
        return true;
    }
    function EnsureStorageMinimum(oMinProd, oMinQA, oMinTest, oHeader) {
        if (oMinProd > 0 && oTotalProd < oMinProd) {
            alert(oHeader + '\n\n' + 'The selected model is a blade. Blade technology requires <%=intStoragePerBladeApp %> GB of storage per device.\n\nCurrently, you have only allocated ' + oTotalProd + ' GB of storage in PRODUCTION.\nYou need to allocate at least ' + oMinProd + ' GB of storage in PRODUCTION.');
            return false;
        }
        if (oMinQA > 0 && oTotalQA < oMinQA) {
            alert(oHeader + '\n\n' + 'The selected model is a blade. Blade technology requires <%=intStoragePerBladeApp %> GB of storage per device.\n\nCurrently, you have only allocated ' + oTotalQA + ' GB of storage in QA.\nYou need to allocate at least ' + oMinQA + ' GB of storage in QA.');
            return false;
        }
        if (oMinTest > 0 && oTotalTest < oMinTest) {
            alert(oHeader + '\n\n' + 'The selected model is a blade. Blade technology requires <%=intStoragePerBladeApp %> GB of storage per device.\n\nCurrently, you have only allocated ' + oTotalTest + ' GB of storage in TEST.\nYou need to allocate at least ' + oMinTest + ' GB of storage in TEST.');
            return false;
        }
        return true;
    }
    function EnsureStorageOS(oStorageProd, oStorageQA, oStorageTest, oHeader) {
        if (oStorageProd > 0 && oTotalProdOS != oStorageProd) {
            alert(oHeader + '\n\n' + 'The selected model is a blade. Blade technology requires <%=intStoragePerBladeOs %> GB of storage per device on the operating system volume.\n\nCurrently, you have allocated ' + oTotalProdOS + ' GB of storage in PRODUCTION.\nYou need to allocate exactly ' + oStorageProd + ' GB of storage in PRODUCTION.');
            return false;
        }
        if (oStorageQA > 0 && oTotalQAOS != oStorageQA) {
            alert(oHeader + '\n\n' + 'The selected model is a blade. Blade technology requires <%=intStoragePerBladeOs %> GB of storage per device on the operating system volume.\n\nCurrently, you have allocated ' + oTotalQAOS + ' GB of storage in QA.\nYou need to allocate exactly ' + oStorageQA + ' GB of storage in QA.');
            return false;
        }
        if (oStorageTest > 0 && oTotalTestOS != oStorageTest) {
            alert(oHeader + '\n\n' + 'The selected model is a blade. Blade technology requires <%=intStoragePerBladeOs %> GB of storage per device on the operating system volume.\n\nCurrently, you have allocated ' + oTotalTestOS + ' GB of storage in TEST.\nYou need to allocate exactly ' + oStorageTest + ' GB of storage in TEST.');
            return false;
        }
        return true;
    }
    function StorageSpinnerUp(oText, intValue) {
        oText = document.getElementById(oText);
        var iValue = 0;
        if (oText.value != "")
            iValue = parseInt(oText.value);
        if (iValue < <%=intStorageOS %>) {
            iValue = iValue + intValue;
            oText.value = iValue;
        }
        return false;
    }
    function StorageSpinnerDn(oText, intValue) {
        oText = document.getElementById(oText);
        var iValue = 0;
        if (oText.value != "")
            iValue = parseInt(oText.value);
        if (iValue > 0) {
            iValue = iValue - intValue;
            oText.value = iValue;
        }
        return false;
    }
</script>
<table cellpadding="2" cellspacing="1" border="0">
    <tr>
        <td colspan="2">Do you require SAN storage to be associated with this design?<font class="required">&nbsp;*</font></td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:RadioButton ID="radYes" runat="server" CssClass="default" Text="Yes" GroupName="storage" /> 
            <asp:RadioButton ID="radNo" runat="server" CssClass="default" Text="No" GroupName="storage" />
            <asp:RadioButton ID="radLater" runat="server" CssClass="default" Text="Ask Me Later" GroupName="storage" />
        </td>
    </tr>
</table>
<div id="divYes" runat="server" style="display:none">
    <br />
    <table width="100%" cellpadding="2" cellspacing="1" border="0">
        <tr>
            <td>
            <%=strMenuTab1 %>
                <!-- 
                <table cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td><img src="/images/TabOnLeftCap.gif"></td>
                        <td nowrap background="/images/TabOnBackground.gif"><a href="javascript:void(0);" onclick="ChangeTab(this,'divTab1',null,null,true);" class="tabheader">Operating System Volumes</a></td>
                        <td><img src="/images/TabOnRightCap.gif"></td>
                        <td><img src="/images/TabOffLeftCap.gif"></td>
                        <td nowrap background="/images/TabOffBackground.gif"><a href="javascript:void(0);" onclick="ChangeTab(this,'divTab2',null,null,true);" class="tabheader">Application / Data Volumes</a></td>
                        <td><img src="/images/TabOffRightCap.gif"></td>
                        <td width="100%" background="/images/TabEmptyBackground.gif">&nbsp;</td>
                    </tr>
                </table>
                -->
                <br />
                <div id="divMenu1"> 
                <div style="display:inline">
                    <table width="100%" cellpadding="1" cellspacing="1" border="0" class="default">
                        <tr>
                            <td>
                            <asp:Label ID="lblChkOSVolIfRequireStorage" runat="server" CssClass="default" Text="Check if you require any of the following storage:<font class='required'>&nbsp;*</font>" /></td>
                            <td align="right"></td>
                        </tr>
                    </table>
                    <table cellpadding="1" cellspacing="1" border="0" class="default">
                        <tr>
                            <td><asp:CheckBox ID="chkHighOS" runat="server" CssClass="default" Text="High Performance" GroupName="performance" /></td>
                        </tr>
                        <tr>
                            <td>
                                <div id="divHighOS" runat="server" style="display:none">
                                <asp:Panel ID="panHighOSProduction" runat="server" Visible="false">
                                <table cellpadding="2" cellspacing="3" border="0">
                                    <tr>
                                        <td align="right"><img src="/images/arrow_down_right.gif" border="0" align="absmiddle" /></td>
                                        <td>
                                        <asp:Label ID="lblOSVolHighPerfPRODStorageAmt" runat="server" CssClass="default" Text="<b>High Performance PROD Storage</b> - Please enter the total amount of storage you require in <b>PROD</b>:<font class='required'>&nbsp;*</font>" /></td>
                                    </tr>
                                    <tr>
                                        <td><img src="/images/spacer.gif" border="0" width="20" height="1" /></td>
                                        <td>
                                            <table cellpadding="1" cellspacing="2" border="0">
                                                <tr>
                                                    <td rowspan="2"><asp:TextBox ID="txtHighOSRequire" runat="server" CssClass="default" Width="75" MaxLength="5" Text="0" /></td>
                                                    <td style="border:solid 1px #CCCCCC"><asp:ImageButton ID="imgHighOSRequireUp" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/spinner_up_arrow.gif" /></td>
                                                    <td rowspan="3">GB</td>
                                                </tr>
                                                <tr>
                                                    <td style="border:solid 1px #CCCCCC"><asp:ImageButton ID="imgHighOSRequireDn" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/spinner_down_arrow.gif" /></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <asp:Panel ID="panHighOSReplication" runat="server" Visible="false">
                                <table cellpadding="2" cellspacing="3" border="0">
                                    <tr>
                                        <td align="right"><img src="/images/arrow_down_right.gif" border="0" align="absmiddle" /></td>
                                        <td colspan="2">
                                        <asp:Label ID="lblOSVolHighPerfReplication" runat="server" CssClass="default" Text="<b>High Performance Replication</b> - Will this storage be replicated?<font class='required'>&nbsp;*</font>" /></td>
                                    </tr>
                                    <tr>
                                        <td><img src="/images/spacer.gif" border="0" width="20" height="1" /></td>
                                        <td>
                                            <asp:DropDownList ID="ddlHighOSReplicated" runat="server" CssClass="default" Width="100">
                                                <asp:ListItem Value="No" />
                                                <asp:ListItem Value="Yes" />
                                            </asp:DropDownList>
                                        </td>
                                        <td><div id="divHighOSReplicated" runat="server" style="display:none">
                                        <img src="/images/green_right.gif" border="0" align="absmiddle" />
                                        <img src="/images/spacer.gif" border="0" width="10" height="1" />
                                        <asp:Label ID="lblOSVolHighPerfReplicationAmt" runat="server" CssClass="default" Text="Please enter the amount of storage to be replicated:" />
                                        <img src="/images/spacer.gif" border="0" width="5" height="1" />
                                        <asp:TextBox ID="txtHighOSReplicated" runat="server" CssClass="default" Width="75" MaxLength="5" /> GB</div></td>
                                    </tr>
                                </table>
                                <br />
                                </asp:Panel>
                                <table cellpadding="2" cellspacing="3" border="0">
                                    <tr>
                                        <td align="right"><img src="/images/arrow_down_right.gif" border="0" align="absmiddle" /></td>
                                        <td colspan="2">
                                        <asp:Label ID="lblOSVolHighPerfStorageAvailabilityLevel" runat="server" CssClass="default" Text="<b>High Performance Storage Availability Level</b> - What level of storage availability to you require?<font class='required'>&nbsp;*</font>" /></td>
                                    </tr>
                                    <tr>
                                        <td><img src="/images/spacer.gif" border="0" width="20" height="1" /></td>
                                        <td>
                                            <asp:DropDownList ID="ddlHighOSLevel" runat="server" CssClass="default" Width="100">
                                                <asp:ListItem Value="Standard" />
                                                <asp:ListItem Value="High" />
                                            </asp:DropDownList>
                                        </td>
                                        <td><div id="divHighOSLevel" runat="server" style="display:none">
                                        <img src="/images/green_right.gif" border="0" align="absmiddle" />
                                        <img src="/images/spacer.gif" border="0" width="10" height="1" />
                                        <asp:Label ID="lblOSVolHighPerfStorageAvailabilityLevelAmt" runat="server" CssClass="default" Text="Please enter the amount of storage to be replicated:" />
                                        <img src="/images/spacer.gif" border="0" width="5" height="1" />
                                        <asp:TextBox ID="txtHighOSLevel" runat="server" CssClass="default" Width="75" MaxLength="5" /> GB</div>
                                    </tr>
                                </table>
                                <br />
                                </asp:Panel>
                                <asp:Panel ID="panHighOSQA" runat="server" Visible="false">
                                <table cellpadding="2" cellspacing="3" border="0">
                                    <tr>
                                        <td align="right"><img src="/images/arrow_down_right.gif" border="0" align="absmiddle" /></td>
                                        <td>
                                        <asp:Label ID="lblOSVolHighPerfQAStorageAmt" runat="server" CssClass="default" Text="<b>High Performance QA Storage</b> - Please enter the total amount of storage you require in <b>QA</b>:<font class='required'>&nbsp;*</font>" /></td>
                                    </tr>
                                    <tr>
                                        <td><img src="/images/spacer.gif" border="0" width="20" height="1" /></td>
                                        <td>
                                            <table cellpadding="1" cellspacing="2" border="0">
                                                <tr>
                                                    <td rowspan="2"><asp:TextBox ID="txtHighOSRequireQA" runat="server" CssClass="default" Width="75" MaxLength="5" Text="0" /></td>
                                                    <td style="border:solid 1px #CCCCCC"><asp:ImageButton ID="imgHighOSRequireQAUp" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/spinner_up_arrow.gif" /></td>
                                                    <td rowspan="3">GB</td>
                                                </tr>
                                                <tr>
                                                    <td style="border:solid 1px #CCCCCC"><asp:ImageButton ID="imgHighOSRequireQADn" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/spinner_down_arrow.gif" /></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                </asp:Panel>
                                <asp:Panel ID="panHighOSTest" runat="server" Visible="false">
                                <table cellpadding="2" cellspacing="3" border="0">
                                    <tr>
                                        <td align="right"><img src="/images/arrow_down_right.gif" border="0" align="absmiddle" /></td>
                                        <td>
                                        <asp:Label ID="lblOSVolHighPerfTESTStorageAmt" runat="server" CssClass="default" Text="<b>High Performance TEST Storage</b> - Please enter the total amount of storage you require in <b>TEST</b>:<font class='required'>&nbsp;*</font>" /></td>
                                    </tr>
                                    <tr>
                                        <td><img src="/images/spacer.gif" border="0" width="20" height="1" /></td>
                                        <td>
                                            <table cellpadding="1" cellspacing="2" border="0">
                                                <tr>
                                                    <td rowspan="2"><asp:TextBox ID="txtHighOSRequireTest" runat="server" CssClass="default" Width="75" MaxLength="5" Text="0" /></td>
                                                    <td style="border:solid 1px #CCCCCC"><asp:ImageButton ID="imgHighOSRequireTestUp" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/spinner_up_arrow.gif" /></td>
                                                    <td rowspan="3">GB</td>
                                                </tr>
                                                <tr>
                                                    <td style="border:solid 1px #CCCCCC"><asp:ImageButton ID="imgHighOSRequireTestDn" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/spinner_down_arrow.gif" /></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                </asp:Panel>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td><asp:CheckBox ID="chkStandardOS" runat="server" CssClass="default" Text="Standard Performance" GroupName="performance" /></td>
                        </tr>
                        <tr>
                            <td>
                                <div id="divStandardOS" runat="server" style="display:none">
                                <asp:Panel ID="panStandardOSProduction" runat="server" Visible="false">
                                <table cellpadding="2" cellspacing="3" border="0">
                                    <tr>
                                        <td align="right"><img src="/images/arrow_down_right.gif" border="0" align="absmiddle" /></td>
                                        <td>
                                        <asp:Label ID="lblOSVolStandardPerfPRODStorageAmt" runat="server" CssClass="default" Text="<b>Standard Performance PROD Storage</b> - Please enter the total amount of storage you require in <b>PROD</b>:<font class='required'>&nbsp;*</font>" /></td>
                                    </tr>
                                    <tr>
                                        <td><img src="/images/spacer.gif" border="0" width="20" height="1" /></td>
                                        <td>
                                            <table cellpadding="1" cellspacing="2" border="0">
                                                <tr>
                                                    <td rowspan="2"><asp:TextBox ID="txtStandardOSRequire" runat="server" CssClass="default" Width="75" MaxLength="5" Text="0" /></td>
                                                    <td style="border:solid 1px #CCCCCC"><asp:ImageButton ID="imgStandardOSRequireUp" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/spinner_up_arrow.gif" /></td>
                                                    <td rowspan="3">GB</td>
                                                </tr>
                                                <tr>
                                                    <td style="border:solid 1px #CCCCCC"><asp:ImageButton ID="imgStandardOSRequireDn" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/spinner_down_arrow.gif" /></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <asp:Panel ID="panStandardOSReplication" runat="server" Visible="false">
                                <table cellpadding="2" cellspacing="3" border="0">
                                    <tr>
                                        <td align="right"><img src="/images/arrow_down_right.gif" border="0" align="absmiddle" /></td>
                                        <td colspan="2">
                                        <asp:Label ID="lblOSVolStandardPerfReplication" runat="server" CssClass="default" Text="<b>Standard Performance Replication</b> - Will this storage be replicated?<font class='required'>&nbsp;*</font>" /></td>
                                    </tr>
                                    <tr>
                                        <td><img src="/images/spacer.gif" border="0" width="20" height="1" /></td>
                                        <td>
                                            <asp:DropDownList ID="ddlStandardOSReplicated" runat="server" CssClass="default" Width="100">
                                                <asp:ListItem Value="No" />
                                                <asp:ListItem Value="Yes" />
                                            </asp:DropDownList>
                                        </td>
                                        <td><div id="divStandardOSReplicated" runat="server" style="display:none"><img src="/images/green_right.gif" border="0" align="absmiddle" /><img src="/images/spacer.gif" border="0" width="10" height="1" />Please enter the amount of storage to be replicated:<img src="/images/spacer.gif" border="0" width="5" height="1" /><asp:TextBox ID="txtStandardOSReplicated" runat="server" CssClass="default" Width="75" MaxLength="5" /> GB</div></td>
                                    </tr>
                                </table>
                                <br />
                                </asp:Panel>
                                <table cellpadding="2" cellspacing="3" border="0">
                                    <tr>
                                        <td align="right"><img src="/images/arrow_down_right.gif" border="0" align="absmiddle" /></td>
                                        <td colspan="2">
                                        <asp:Label ID="lblOSVolStandardPerfStorageAvailabilityLevel" runat="server" CssClass="default" Text="<b>Standard Performance Storage Availability Level</b> - What level of storage availability to you require?<font class='required'>&nbsp;*</font>" /></td>
                                    </tr>
                                    <tr>
                                        <td><img src="/images/spacer.gif" border="0" width="20" height="1" /></td>
                                        <td>
                                            <asp:DropDownList ID="ddlStandardOSLevel" runat="server" CssClass="default" Width="100">
                                                <asp:ListItem Value="Standard" />
                                                <asp:ListItem Value="High" />
                                            </asp:DropDownList>
                                        </td>
                                        <td><div id="divStandardOSLevel" runat="server" style="display:none">
                                        <img src="/images/green_right.gif" border="0" align="absmiddle" />
                                        <img src="/images/spacer.gif" border="0" width="10" height="1" />
                                        <asp:Label ID="lblOSVolStandardPerfStorageAvailabilityLevelAmt" runat="server" CssClass="default" Text="Please enter the amount of storage to be replicated:" />
                                        <img src="/images/spacer.gif" border="0" width="5" height="1" />
                                        <asp:TextBox ID="txtStandardOSLevel" runat="server" CssClass="default" Width="75" MaxLength="5" /> GB</div>
                                    </tr>
                                </table>
                                <br />
                                </asp:Panel>
                                <asp:Panel ID="panStandardOSQA" runat="server" Visible="false">
                                <table cellpadding="2" cellspacing="3" border="0">
                                    <tr>
                                        <td align="right"><img src="/images/arrow_down_right.gif" border="0" align="absmiddle" /></td>
                                        <td>
                                        <asp:Label ID="lblOSVolStandardPerfQAStorageAmt" runat="server" CssClass="default" Text="<b>Standard Performance QA Storage</b> - Please enter the total amount of storage you require in <b>QA</b>:<font class='required'>&nbsp;*</font>" /></td>
                                    </tr>
                                    <tr>
                                        <td><img src="/images/spacer.gif" border="0" width="20" height="1" /></td>
                                        <td>
                                            <table cellpadding="1" cellspacing="2" border="0">
                                                <tr>
                                                    <td rowspan="2"><asp:TextBox ID="txtStandardOSRequireQA" runat="server" CssClass="default" Width="75" MaxLength="5" Text="0" /></td>
                                                    <td style="border:solid 1px #CCCCCC"><asp:ImageButton ID="imgStandardOSRequireQAUp" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/spinner_up_arrow.gif" /></td>
                                                    <td rowspan="3">GB</td>
                                                </tr>
                                                <tr>
                                                    <td style="border:solid 1px #CCCCCC"><asp:ImageButton ID="imgStandardOSRequireQADn" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/spinner_down_arrow.gif" /></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                </asp:Panel>
                                <asp:Panel ID="panStandardOSTest" runat="server" Visible="false">
                                <table cellpadding="2" cellspacing="3" border="0">
                                    <tr>
                                        <td align="right"><img src="/images/arrow_down_right.gif" border="0" align="absmiddle" /></td>
                                        <td>
                                        <asp:Label ID="lblOSVolStandardPerfTESTStorageAmt" runat="server" CssClass="default" Text="<b>Standard Performance TEST Storage</b> - Please enter the total amount of storage you require in <b>TEST</b>:<font class='required'>&nbsp;*</font>" /></td>
                                    </tr>
                                    <tr>
                                        <td><img src="/images/spacer.gif" border="0" width="20" height="1" /></td>
                                        <td>
                                            <table cellpadding="1" cellspacing="2" border="0">
                                                <tr>
                                                    <td rowspan="2"><asp:TextBox ID="txtStandardOSRequireTest" runat="server" CssClass="default" Width="75" MaxLength="5" Text="0" /></td>
                                                    <td style="border:solid 1px #CCCCCC"><asp:ImageButton ID="imgStandardOSRequireTestUp" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/spinner_up_arrow.gif" /></td>
                                                    <td rowspan="3">GB</td>
                                                </tr>
                                                <tr>
                                                    <td style="border:solid 1px #CCCCCC"><asp:ImageButton ID="imgStandardOSRequireTestDn" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/spinner_down_arrow.gif" /></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                </asp:Panel>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td><asp:CheckBox ID="chkLowOS" runat="server" CssClass="default" Text="Low Performance" GroupName="performance" /></td>
                        </tr>
                        <tr>
                            <td>
                                <div id="divLowOS" runat="server" style="display:none">
                                <asp:Panel ID="panLowOSProduction" runat="server" Visible="false">
                                <table cellpadding="2" cellspacing="3" border="0">
                                    <tr>
                                        <td align="right"><img src="/images/arrow_down_right.gif" border="0" align="absmiddle" /></td>
                                        <td>
                                        <asp:Label ID="lblOSVolLowPerfPRODStorageAmt" runat="server" CssClass="default" Text="<b>Low Performance PROD Storage</b> - Please enter the total amount of storage you require in <b>PROD</b>:<font class='required'>&nbsp;*</font>" /></td>
                                    </tr>
                                    <tr>
                                        <td><img src="/images/spacer.gif" border="0" width="20" height="1" /></td>
                                        <td>
                                            <table cellpadding="1" cellspacing="2" border="0">
                                                <tr>
                                                    <td rowspan="2"><asp:TextBox ID="txtLowOSRequire" runat="server" CssClass="default" Width="75" MaxLength="5" Text="0" /></td>
                                                    <td style="border:solid 1px #CCCCCC"><asp:ImageButton ID="imgLowOSRequireUp" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/spinner_up_arrow.gif" /></td>
                                                    <td rowspan="3">GB</td>
                                                </tr>
                                                <tr>
                                                    <td style="border:solid 1px #CCCCCC"><asp:ImageButton ID="imgLowOSRequireDn" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/spinner_down_arrow.gif" /></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <asp:Panel ID="panLowOSReplication" runat="server" Visible="false">
                                <table cellpadding="2" cellspacing="3" border="0">
                                    <tr>
                                        <td align="right"><img src="/images/arrow_down_right.gif" border="0" align="absmiddle" /></td>
                                        <td colspan="2">
                                        <asp:Label ID="lblOSVolLowPerfReplication" runat="server" CssClass="default" Text="<b>Low Performance Replication</b> - Will this storage be replicated?<font class='required'>&nbsp;*</font>" /></td>
                                    </tr>
                                    <tr>
                                        <td><img src="/images/spacer.gif" border="0" width="20" height="1" /></td>
                                        <td>
                                            <asp:DropDownList ID="ddlLowOSReplicated" runat="server" CssClass="default" Width="100">
                                                <asp:ListItem Value="No" />
                                                <asp:ListItem Value="Yes" />
                                            </asp:DropDownList>
                                        </td>
                                        <td><div id="divLowOSReplicated" runat="server" style="display:none">
                                            <img src="/images/green_right.gif" border="0" align="absmiddle" />
                                            <img src="/images/spacer.gif" border="0" width="10" height="1" />
                                            <asp:Label ID="lblOSVolLowPerfReplicationAmt" runat="server" CssClass="default" Text="Please enter the amount of storage to be replicated:" />
                                            <img src="/images/spacer.gif" border="0" width="5" height="1" />
                                            <asp:TextBox ID="txtLowOSReplicated" runat="server" CssClass="default" Width="75" MaxLength="5" /> GB</div></td>
                                    </tr>
                                </table>
                                <br />
                                </asp:Panel>
                                <table cellpadding="2" cellspacing="3" border="0">
                                    <tr>
                                        <td align="right"><img src="/images/arrow_down_right.gif" border="0" align="absmiddle" /></td>
                                        <td colspan="2">
                                        <asp:Label ID="lblOSVolLowPerfStorageAvailabilityLevel" runat="server" CssClass="default" Text="<b>Low Performance Storage Availability Level</b> - What level of storage availability to you require?<font class='required'>&nbsp;*</font>" /></td>
                                    </tr>
                                    <tr>
                                        <td><img src="/images/spacer.gif" border="0" width="20" height="1" /></td>
                                        <td>
                                            <asp:DropDownList ID="ddlLowOSLevel" runat="server" CssClass="default" Width="100">
                                                <asp:ListItem Value="Standard" />
                                                <asp:ListItem Value="High" />
                                            </asp:DropDownList>
                                        </td>
                                        <td><div id="divLowOSLevel" runat="server" style="display:none">
                                        <img src="/images/green_right.gif" border="0" align="absmiddle" />
                                        <img src="/images/spacer.gif" border="0" width="10" height="1" />
                                        <asp:Label ID="lblOSVolLowPerfStorageAvailabilityLevelAmt" runat="server" CssClass="default" Text="Please enter the amount of storage to be replicated:" />
                                        <img src="/images/spacer.gif" border="0" width="5" height="1" />
                                        <asp:TextBox ID="txtLowOSLevel" runat="server" CssClass="default" Width="75" MaxLength="5" /> GB</div>
                                    </tr>
                                </table>
                                <br />
                                </asp:Panel>
                                <asp:Panel ID="panLowOSQA" runat="server" Visible="false">
                                <table cellpadding="2" cellspacing="3" border="0">
                                    <tr>
                                        <td align="right"><img src="/images/arrow_down_right.gif" border="0" align="absmiddle" /></td>
                                        <td>
                                        <asp:Label ID="lblOSVolLowPerfQAStorageAmt" runat="server" CssClass="default" Text="<b>Low Performance QA Storage</b> - Please enter the total amount of storage you require in <b>QA</b>:<font class='required'>&nbsp;*</font>" /></td>
                                    </tr>
                                    <tr>
                                        <td><img src="/images/spacer.gif" border="0" width="20" height="1" /></td>
                                        <td>
                                            <table cellpadding="1" cellspacing="2" border="0">
                                                <tr>
                                                    <td rowspan="2"><asp:TextBox ID="txtLowOSRequireQA" runat="server" CssClass="default" Width="75" MaxLength="5" Text="0" /></td>
                                                    <td style="border:solid 1px #CCCCCC"><asp:ImageButton ID="imgLowOSRequireQAUp" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/spinner_up_arrow.gif" /></td>
                                                    <td rowspan="3">GB</td>
                                                </tr>
                                                <tr>
                                                    <td style="border:solid 1px #CCCCCC"><asp:ImageButton ID="imgLowOSRequireQADn" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/spinner_down_arrow.gif" /></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                </asp:Panel>
                                <asp:Panel ID="panLowOSTest" runat="server" Visible="false">
                                <table cellpadding="2" cellspacing="3" border="0">
                                    <tr>
                                        <td align="right"><img src="/images/arrow_down_right.gif" border="0" align="absmiddle" /></td>
                                        <td>
                                        <asp:Label ID="lblOSVolLowPerfTESTStorageAmt" runat="server" CssClass="default" Text="<b>Low Performance TEST Storage</b> - Please enter the total amount of storage you require in <b>TEST</b>:<font class='required'>&nbsp;*</font>" /></td>
                                    </tr>
                                    <tr>
                                        <td><img src="/images/spacer.gif" border="0" width="20" height="1" /></td>
                                        <td>
                                            <table cellpadding="1" cellspacing="2" border="0">
                                                <tr>
                                                    <td rowspan="2"><asp:TextBox ID="txtLowOSRequireTest" runat="server" CssClass="default" Width="75" MaxLength="5" Text="0" /></td>
                                                    <td style="border:solid 1px #CCCCCC"><asp:ImageButton ID="imgLowOSRequireTestUp" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/spinner_up_arrow.gif" /></td>
                                                    <td rowspan="3">GB</td>
                                                </tr>
                                                <tr>
                                                    <td style="border:solid 1px #CCCCCC"><asp:ImageButton ID="imgLowOSRequireTestDn" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/spinner_down_arrow.gif" /></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                </asp:Panel>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
                <div style="display:none">
                    <table width="100%" cellpadding="1" cellspacing="1" border="0" class="default">
                        <tr>
                            <td>
                            <asp:Label ID="lblChkAppVolIfStorageReq" runat="server" CssClass="default" Text="Check if you require any of the following storage:<font class='required'>&nbsp;*</font>" /></td>
                            <td align="right"></td>
                        </tr>
                    </table>
                    <table cellpadding="1" cellspacing="1" border="0" class="default">
                        <tr>
                            <td><asp:CheckBox ID="chkHigh" runat="server" CssClass="default" Text="High Performance" GroupName="performance" /></td>
                        </tr>
                        <tr>
                            <td>
                                <div id="divHigh" runat="server" style="display:none">
                                <asp:Panel ID="panHighProduction" runat="server" Visible="false">
                                <table cellpadding="2" cellspacing="3" border="0">
                                    <tr>
                                        <td align="right"><img src="/images/arrow_down_right.gif" border="0" align="absmiddle" /></td>
                                        <td>
                                        <asp:Label ID="lblAppVolHighPerfPRODStorageAmt" runat="server" CssClass="default" Text="<b>High Performance PROD Storage</b> - Please enter the total amount of storage you require in <b>PROD</b>:<font class='required'>&nbsp;*</font>" /></td>
                                    </tr>
                                    <tr>
                                        <td><img src="/images/spacer.gif" border="0" width="20" height="1" /></td>
                                        <td><asp:TextBox ID="txtHighRequire" runat="server" CssClass="default" Width="75" MaxLength="5" /> GB</td>
                                    </tr>
                                </table>
                                <br />
                                <asp:Panel ID="panHighReplication" runat="server" Visible="false">
                                <table cellpadding="2" cellspacing="3" border="0">
                                    <tr>
                                        <td align="right"><img src="/images/arrow_down_right.gif" border="0" align="absmiddle" /></td>
                                        <td colspan="2">
                                        <asp:Label ID="lblAppVolHighPerfReplication" runat="server" CssClass="default" Text="<b>High Performance Replication</b> - Will this storage be replicated?<font class='required'>&nbsp;*</font>" /></td>
                                    </tr>
                                    <tr>
                                        <td><img src="/images/spacer.gif" border="0" width="20" height="1" /></td>
                                        <td>
                                            <asp:DropDownList ID="ddlHighReplicated" runat="server" CssClass="default" Width="100">
                                                <asp:ListItem Value="No" />
                                                <asp:ListItem Value="Yes" />
                                            </asp:DropDownList>
                                        </td>
                                        <td><div id="divHighReplicated" runat="server" style="display:none">
                                        <img src="/images/green_right.gif" border="0" align="absmiddle" />
                                        <img src="/images/spacer.gif" border="0" width="10" height="1" />
                                        <asp:Label ID="lblAppVolHighPerfReplicationAmt" runat="server" CssClass="default" Text="Please enter the amount of storage to be replicated:" />
                                        <img src="/images/spacer.gif" border="0" width="5" height="1" />
                                        <asp:TextBox ID="txtHighReplicated" runat="server" CssClass="default" Width="75" MaxLength="5" /> GB</div></td>
                                    </tr>
                                </table>
                                <br />
                                </asp:Panel>
                                <table cellpadding="2" cellspacing="3" border="0">
                                    <tr>
                                        <td align="right"><img src="/images/arrow_down_right.gif" border="0" align="absmiddle" /></td>
                                        <td colspan="2">
                                        <asp:Label ID="lblAppVolHighPerfStorageAvailabilityLevel" runat="server" CssClass="default" Text="<b>High Performance Storage Availability Level</b> - What level of storage availability to you require?<font class='required'>&nbsp;*</font>" /></td>
                                    </tr>
                                    <tr>
                                        <td><img src="/images/spacer.gif" border="0" width="20" height="1" /></td>
                                        <td>
                                            <asp:DropDownList ID="ddlHighLevel" runat="server" CssClass="default" Width="100">
                                                <asp:ListItem Value="Standard" />
                                                <asp:ListItem Value="High" />
                                            </asp:DropDownList>
                                        </td>
                                        <td><div id="divHighLevel" runat="server" style="display:none">
                                        <img src="/images/green_right.gif" border="0" align="absmiddle" />
                                        <img src="/images/spacer.gif" border="0" width="10" height="1" />
                                        <asp:Label ID="lblAppVolHighPerfStorageAvailabilityLevelAmt" runat="server" CssClass="default" Text="Please enter the amount of storage to be replicated:" />
                                        <img src="/images/spacer.gif" border="0" width="5" height="1" />
                                        <asp:TextBox ID="txtHighLevel" runat="server" CssClass="default" Width="75" MaxLength="5" /> GB</div>
                                    </tr>
                                </table>
                                <br />
                                </asp:Panel>
                                <asp:Panel ID="panHighQA" runat="server" Visible="false">
                                <table cellpadding="2" cellspacing="3" border="0">
                                    <tr>
                                        <td align="right"><img src="/images/arrow_down_right.gif" border="0" align="absmiddle" /></td>
                                        <td>
                                        <asp:Label ID="lblAppVolHighPerfQAStorageAmt" runat="server" CssClass="default" Text="<b>High Performance QA Storage</b> - Please enter the total amount of storage you require in <b>QA</b>:<font class='required'>&nbsp;*</font>" /></td>
                                    </tr>
                                    <tr>
                                        <td><img src="/images/spacer.gif" border="0" width="20" height="1" /></td>
                                        <td><asp:TextBox ID="txtHighRequireQA" runat="server" CssClass="default" Width="75" MaxLength="5" /> GB</td>
                                    </tr>
                                </table>
                                <br />
                                </asp:Panel>
                                <asp:Panel ID="panHighTest" runat="server" Visible="false">
                                <table cellpadding="2" cellspacing="3" border="0">
                                    <tr>
                                        <td align="right"><img src="/images/arrow_down_right.gif" border="0" align="absmiddle" /></td>
                                        <td>
                                        <asp:Label ID="lblAppVolHighPerfTESTStorageAmt" runat="server" CssClass="default" Text="<b>High Performance TEST Storage</b> - Please enter the total amount of storage you require in <b>TEST</b>:<font class='required'>&nbsp;*</font>" /></td>
                                    </tr>
                                    <tr>
                                        <td><img src="/images/spacer.gif" border="0" width="20" height="1" /></td>
                                        <td><asp:TextBox ID="txtHighRequireTest" runat="server" CssClass="default" Width="75" MaxLength="5" /> GB</td>
                                    </tr>
                                </table>
                                <br />
                                </asp:Panel>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td><asp:CheckBox ID="chkStandard" runat="server" CssClass="default" Text="Standard Performance" GroupName="performance" /></td>
                        </tr>
                        <tr>
                            <td>
                                <div id="divStandard" runat="server" style="display:none">
                                <asp:Panel ID="panStandardProduction" runat="server" Visible="false">
                                <table cellpadding="2" cellspacing="3" border="0">
                                    <tr>
                                        <td align="right"><img src="/images/arrow_down_right.gif" border="0" align="absmiddle" /></td>
                                        <td>
                                        <asp:Label ID="lblAppVolStandardPerfPRODStorageAmt" runat="server" CssClass="default" Text="<b>Standard Performance PROD Storage</b> - Please enter the total amount of storage you require in <b>PROD</b>:<font class='required'>&nbsp;*</font>" /></td>
                                    </tr>
                                    <tr>
                                        <td><img src="/images/spacer.gif" border="0" width="20" height="1" /></td>
                                        <td><asp:TextBox ID="txtStandardRequire" runat="server" CssClass="default" Width="75" MaxLength="5" /> GB</td>
                                    </tr>
                                </table>
                                <br />
                                <asp:Panel ID="panStandardReplication" runat="server" Visible="false">
                                <table cellpadding="2" cellspacing="3" border="0">
                                    <tr>
                                        <td align="right"><img src="/images/arrow_down_right.gif" border="0" align="absmiddle" /></td>
                                        <td colspan="2">
                                        <asp:Label ID="lblAppVolStandardPerfReplication" runat="server" CssClass="default" Text="<b>Standard Performance Replication</b> - Will this storage be replicated?<font class='required'>&nbsp;*</font>" /></td>
                                    </tr>
                                    <tr>
                                        <td><img src="/images/spacer.gif" border="0" width="20" height="1" /></td>
                                        <td>
                                            <asp:DropDownList ID="ddlStandardReplicated" runat="server" CssClass="default" Width="100">
                                                <asp:ListItem Value="No" />
                                                <asp:ListItem Value="Yes" />
                                            </asp:DropDownList>
                                        </td>
                                        <td><div id="divStandardReplicated" runat="server" style="display:none">
                                        <img src="/images/green_right.gif" border="0" align="absmiddle" />
                                        <img src="/images/spacer.gif" border="0" width="10" height="1" />
                                        <asp:Label ID="lblAppVolStandardPerfReplicationAmt" runat="server" CssClass="default" Text="Please enter the amount of storage to be replicated:" />
                                        <img src="/images/spacer.gif" border="0" width="5" height="1" />
                                        <asp:TextBox ID="txtStandardReplicated" runat="server" CssClass="default" Width="75" MaxLength="5" /> GB</div></td>
                                    </tr>
                                </table>
                                <br />
                                </asp:Panel>
                                <table cellpadding="2" cellspacing="3" border="0">
                                    <tr>
                                        <td align="right"><img src="/images/arrow_down_right.gif" border="0" align="absmiddle" /></td>
                                        <td colspan="2">
                                        <asp:Label ID="lblAppVolStandardPerfStorageAvailabilityLevel" runat="server" CssClass="default" Text="<b>Standard Performance Storage Availability Level</b> - What level of storage availability to you require?<font class='required'>&nbsp;*</font>" /></td>
                                    </tr>
                                    <tr>
                                        <td><img src="/images/spacer.gif" border="0" width="20" height="1" /></td>
                                        <td>
                                            <asp:DropDownList ID="ddlStandardLevel" runat="server" CssClass="default" Width="100">
                                                <asp:ListItem Value="Standard" />
                                                <asp:ListItem Value="High" />
                                            </asp:DropDownList>
                                        </td>
                                        <td><div id="divStandardLevel" runat="server" style="display:none">
                                        <img src="/images/green_right.gif" border="0" align="absmiddle" />
                                        <img src="/images/spacer.gif" border="0" width="10" height="1" />
                                        <asp:Label ID="lblAppVolStandardPerfStorageAvailabilityLevelAmt" runat="server" CssClass="default" Text="Please enter the amount of storage to be replicated:" />
                                        <img src="/images/spacer.gif" border="0" width="5" height="1" />
                                        <asp:TextBox ID="txtStandardLevel" runat="server" CssClass="default" Width="75" MaxLength="5" /> GB</div>
                                    </tr>
                                </table>
                                <br />
                                </asp:Panel>
                                <asp:Panel ID="panStandardQA" runat="server" Visible="false">
                                <table cellpadding="2" cellspacing="3" border="0">
                                    <tr>
                                        <td align="right"><img src="/images/arrow_down_right.gif" border="0" align="absmiddle" /></td>
                                        <td>
                                        <asp:Label ID="lblAppVolStandardPerfQAStorageAmt" runat="server" CssClass="default" Text="<b>Standard Performance QA Storage</b> - Please enter the total amount of storage you require in <b>QA</b>:<font class='required'>&nbsp;*</font>" /></td>
                                    </tr>
                                    <tr>
                                        <td><img src="/images/spacer.gif" border="0" width="20" height="1" /></td>
                                        <td><asp:TextBox ID="txtStandardRequireQA" runat="server" CssClass="default" Width="75" MaxLength="5" /> GB</td>
                                    </tr>
                                </table>
                                <br />
                                </asp:Panel>
                                <asp:Panel ID="panStandardTest" runat="server" Visible="false">
                                <table cellpadding="2" cellspacing="3" border="0">
                                    <tr>
                                        <td align="right"><img src="/images/arrow_down_right.gif" border="0" align="absmiddle" /></td>
                                        <td>
                                        <asp:Label ID="lblAppVolStandardPerfTESTStorageAmt" runat="server" CssClass="default" Text="<b>Standard Performance TEST Storage</b> - Please enter the total amount of storage you require in <b>TEST</b>:<font class='required'>&nbsp;*</font>" /></td>
                                    </tr>
                                    <tr>
                                        <td><img src="/images/spacer.gif" border="0" width="20" height="1" /></td>
                                        <td><asp:TextBox ID="txtStandardRequireTest" runat="server" CssClass="default" Width="75" MaxLength="5" /> GB</td>
                                    </tr>
                                </table>
                                <br />
                                </asp:Panel>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td><asp:CheckBox ID="chkLow" runat="server" CssClass="default" Text="Low Performance" GroupName="performance" /></td>
                        </tr>
                        <tr>
                            <td>
                                <div id="divLow" runat="server" style="display:none">
                                <asp:Panel ID="panLowProduction" runat="server" Visible="false">
                                <table cellpadding="2" cellspacing="3" border="0">
                                    <tr>
                                        <td align="right"><img src="/images/arrow_down_right.gif" border="0" align="absmiddle" /></td>
                                        <td>
                                        <asp:Label ID="lblAppVolLowPerfPRODStorageAmt" runat="server" CssClass="default" Text="<b>Low Performance PROD Storage</b> - Please enter the total amount of storage you require in <b>PROD</b>:<font class='required'>&nbsp;*</font>" /></td>
                                    </tr>
                                    <tr>
                                        <td><img src="/images/spacer.gif" border="0" width="20" height="1" /></td>
                                        <td><asp:TextBox ID="txtLowRequire" runat="server" CssClass="default" Width="75" MaxLength="5" /> GB</td>
                                    </tr>
                                </table>
                                <br />
                                <asp:Panel ID="panLowReplication" runat="server" Visible="false">
                                <table cellpadding="2" cellspacing="3" border="0">
                                    <tr>
                                        <td align="right"><img src="/images/arrow_down_right.gif" border="0" align="absmiddle" /></td>
                                        <td colspan="2">
                                        <asp:Label ID="lblAppVolLowPerfReplication" runat="server" CssClass="default" Text="<b>Low Performance Replication</b> - Will this storage be replicated?<font class='required'>&nbsp;*</font>" /></td>
                                    </tr>
                                    <tr>
                                        <td><img src="/images/spacer.gif" border="0" width="20" height="1" /></td>
                                        <td>
                                            <asp:DropDownList ID="ddlLowReplicated" runat="server" CssClass="default" Width="100">
                                                <asp:ListItem Value="No" />
                                                <asp:ListItem Value="Yes" />
                                            </asp:DropDownList>
                                        </td>
                                        <td><div id="divLowReplicated" runat="server" style="display:none">
                                        <img src="/images/green_right.gif" border="0" align="absmiddle" />
                                        <img src="/images/spacer.gif" border="0" width="10" height="1" />
                                        <asp:Label ID="lblAppVolLowPerfReplicationAmt" runat="server" CssClass="default" Text="Please enter the amount of storage to be replicated:" />
                                        <img src="/images/spacer.gif" border="0" width="5" height="1" />
                                        <asp:TextBox ID="txtLowReplicated" runat="server" CssClass="default" Width="75" MaxLength="5" /> GB</div></td>
                                    </tr>
                                </table>
                                <br />
                                </asp:Panel>
                                <table cellpadding="2" cellspacing="3" border="0">
                                    <tr>
                                        <td align="right"><img src="/images/arrow_down_right.gif" border="0" align="absmiddle" /></td>
                                        <td colspan="2">
                                        <asp:Label ID="lblAppVolLowPerfStorageAvailabilityLevel" runat="server" CssClass="default" Text="<b>Low Performance Storage Availability Level</b> - What level of storage availability to you require?<font class='required'>&nbsp;*</font>" /></td>
                                    </tr>
                                    <tr>
                                        <td><img src="/images/spacer.gif" border="0" width="20" height="1" /></td>
                                        <td>
                                            <asp:DropDownList ID="ddlLowLevel" runat="server" CssClass="default" Width="100">
                                                <asp:ListItem Value="Standard" />
                                                <asp:ListItem Value="High" />
                                            </asp:DropDownList>
                                        </td>
                                        <td><div id="divLowLevel" runat="server" style="display:none">
                                        <img src="/images/green_right.gif" border="0" align="absmiddle" />
                                        <img src="/images/spacer.gif" border="0" width="10" height="1" />
                                        <asp:Label ID="lblAppVolLowPerfStorageAvailabilityLevelAmt" runat="server" CssClass="default" Text="Please enter the amount of storage to be replicated:" />
                                        <img src="/images/spacer.gif" border="0" width="5" height="1" />
                                        <asp:TextBox ID="txtLowLevel" runat="server" CssClass="default" Width="75" MaxLength="5" /> GB</div>
                                    </tr>
                                </table>
                                <br />
                                </asp:Panel>
                                <asp:Panel ID="panLowQA" runat="server" Visible="false">
                                <table cellpadding="2" cellspacing="3" border="0">
                                    <tr>
                                        <td align="right"><img src="/images/arrow_down_right.gif" border="0" align="absmiddle" /></td>
                                        <td>
                                        <asp:Label ID="lblAppVolLowPerfQAStorageAmt" runat="server" CssClass="default" Text="<b>Low Performance QA Storage</b> - Please enter the total amount of storage you require in <b>QA</b>:<font class='required'>&nbsp;*</font>" /></td>
                                    </tr>
                                    <tr>
                                        <td><img src="/images/spacer.gif" border="0" width="20" height="1" /></td>
                                        <td><asp:TextBox ID="txtLowRequireQA" runat="server" CssClass="default" Width="75" MaxLength="5" /> GB</td>
                                    </tr>
                                </table>
                                <br />
                                </asp:Panel>
                                <asp:Panel ID="panLowTest" runat="server" Visible="false">
                                <table cellpadding="2" cellspacing="3" border="0">
                                    <tr>
                                        <td align="right"><img src="/images/arrow_down_right.gif" border="0" align="absmiddle" /></td>
                                        <td>
                                        <asp:Label ID="lblAppVolLowPerfTESTStorageAmt" runat="server" CssClass="default" Text="<b>Low Performance TEST Storage</b> - Please enter the total amount of storage you require in <b>TEST</b>:<font class='required'>&nbsp;*</font>" /></td>
                                    </tr>
                                    <tr>
                                        <td><img src="/images/spacer.gif" border="0" width="20" height="1" /></td>
                                        <td><asp:TextBox ID="txtLowRequireTest" runat="server" CssClass="default" Width="75" MaxLength="5" /> GB</td>
                                    </tr>
                                </table>
                                <br />
                                </asp:Panel>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
                </div> 
            </td>
        </tr>
    </table>
</div>
<br />
<table width="100%" cellpadding="2" cellspacing="1" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
    <tr>
        <td class="bigred" colspan="2">Please read the following...</td>
    </tr>
    <tr>
        <td valign="top"><img src="/images/bigAlert.gif" border="0" align="absmiddle" /></td>
        <td>
            <table cellpadding="2" cellspacing="1" border="0" class="reddefault">
                <tr>
                    <td><b>If any storage is required, you need to specify the amount on this page.</b></td>
                </tr>
                <tr>
                    <td>The amounts should be accumulative for ALL the devices (10GB x 5 devices = 50GB total).</td>
                </tr>
                <tr>
                    <td>&nbsp;&nbsp;-&nbsp;Default VMware: 10GB per device</td>
                </tr>
                <tr>
                    <td>&nbsp;&nbsp;-&nbsp;Default Blade: 10GB per device</td>
                </tr>
                <tr>
                    <td>&nbsp;&nbsp;-&nbsp;Default Physical: 0GB per device</td>
                </tr>
            </table>
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
        <td align="right">
            <asp:Button ID="btnHundred" runat="server" OnClick="btnCancel_Click" Text="Back" CssClass="default" Width="75" Visible="false" /> 
            <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="default" Width="75" />
        </td>
    </tr>
</table>
