<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="application.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.application" %>

<script type="text/javascript">
    var oOldMnemonic = "";
    function AfterAJAXFunction() {
        // Find the mnemonic
        var oHidden = document.getElementById('<%=hdnMnemonic.ClientID %>');
        var strHidden = oHidden.value;
        if (oOldMnemonic == "" || oOldMnemonic != strHidden) {
            oOldMnemonic = strHidden;
            if (strHidden != "" && strHidden != "0")
                LookupMnemonic(oHidden.value);
            else {
                // Clear Mnemonic User Information
                oMnemonicTextbox.disabled = false;
            }
        }
    }
    
    function LookupMnemonic(strMnemonic) {
        // Client Contact
        PopulateMnemonic('<%=txtOwner.ClientID %>','<%=hdnOwner.ClientID %>',strMnemonic,'DMName','Departmental Manager');
        // Primary Contact
        PopulateMnemonic('<%=txtPrimary.ClientID %>','<%=hdnPrimary.ClientID %>',strMnemonic,'ATLName','Application Technical Lead');
        // Application Owner
        PopulateMnemonic('<%=txtAppOwner.ClientID %>','<%=hdnAppOwner.ClientID %>',strMnemonic,'AppOwner','Application Owner');
    }

    var oActiveXMnemonic = null;
    var oMnemonicTextbox = null;
    var oMnemonicHidden = null;
    var oMnemonicName = null;
    var intMnemonicTry = 0;
    var strMnemonicNames = null;
    function PopulateMnemonic(_textbox, _hidden, _mnemonic, _field, _name) {
        oMnemonicTextbox = document.getElementById(_textbox);
        oMnemonicHidden = document.getElementById(_hidden);
        oMnemonicName = _name;
        oActiveXMnemonic = new ActiveXObject("Microsoft.XMLHTTP");
        oActiveXMnemonic.onreadystatechange = PopulateMnemonic_a;
        oActiveXMnemonic.open("GET", "/frame/ajax/ajax_mnemonic.aspx?u=GET", false);
        oActiveXMnemonic.send("<ajax><value>" + _mnemonic + "</value><value>" + _field + "</value></ajax>");
    }
    function PopulateMnemonic_a() {
        if (oActiveXMnemonic.readyState == 4)
        {
            if (oActiveXMnemonic.status == 200) {
                var or = oActiveXMnemonic.responseXML.documentElement.childNodes;
                if (or[0].childNodes[0] != null)
                {
                    var strName = or[0].childNodes[0].text;
                    //alert(strName);
                    strMnemonicNames = new Array();
                    var intName = 0;
                    while (strName.indexOf(" ") > -1) {
                        strMnemonicNames[intName] = strName.substring(0, strName.indexOf(" "));
                        if (strMnemonicNames[intName] != "")
                            intName = intName + 1;
                        strName = strName.substring(strName.indexOf(" ") + 1);
                    }
                    strMnemonicNames[intName] = strName;
                    intMnemonicTry = 1;
                    PopulateMnemonicUser();
                }
                else
                    oMnemonicTextbox.focus();
            }
            else 
                alert('There was a problem getting the information');
        }
    }
    function PopulateMnemonicUser() {
        var strFirst = "";
        var strMiddle = "";
        var strLast = "";
        for(var ii=0; ii<strMnemonicNames.length; ii++) {
            if (strFirst == "")
                strFirst = strMnemonicNames[ii];
            else if (strMiddle == "")
                strMiddle = strMnemonicNames[ii];
            else
                strLast = strMnemonicNames[ii];
            //alert(strMnemonicNames[ii]);
        }
        oActiveXMnemonic = new ActiveXObject("Microsoft.XMLHTTP");
        oActiveXMnemonic.onreadystatechange = PopulateMnemonicUser_a;
        oActiveXMnemonic.open("GET", "/frame/users.aspx?u=GET", false);
        if (intMnemonicTry == 1)
            oActiveXMnemonic.send("<ajax><value>" + strLast + "</value></ajax>");
        else if (intMnemonicTry == 2)
            oActiveXMnemonic.send("<ajax><value>" + strFirst + "</value></ajax>");
        else if (intMnemonicTry == 3)
            oActiveXMnemonic.send("<ajax><value>" + strFirst + " " + strLast + "</value></ajax>");
        else if (intMnemonicTry == 4)
            oActiveXMnemonic.send("<ajax><value>" + strLast + " " + strFirst + "</value></ajax>");
        else if (intMnemonicTry == 5)
            oActiveXMnemonic.send("<ajax><value>" + strFirst + " " + strMiddle + " " + strLast + "</value></ajax>");
        else if (intMnemonicTry == 6)
            oActiveXMnemonic.send("<ajax><value>" + strMiddle + " " + strLast + "</value></ajax>");
        else if (intMnemonicTry == 7)
            oActiveXMnemonic.send("<ajax><value>" + strFirst + " " + strMiddle + "</value></ajax>");
        else {
            alert('A user could not be automatically selected for the field : ' + oMnemonicName + '\n\nPlease try to find the user ' + strFirst + " " + strMiddle + " " + strLast + ' on your own');
            oMnemonicTextbox.disabled = false;
            oMnemonicTextbox.focus();
        }
    }
    
    function PopulateMnemonicUser_a() {
        if (oActiveXMnemonic.readyState == 4)
        {
            if (oActiveXMnemonic.status == 200) {
                var or = oActiveXMnemonic.responseXML.documentElement.childNodes;
                
                //alert(or.length);
                if (or.length != 2) {
                    intMnemonicTry = intMnemonicTry + 1;
                    PopulateMnemonicUser();
                }
                else {
                    //alert(or[1].childNodes[0].text);
                    oMnemonicTextbox.value = or[1].childNodes[0].text;
                    oMnemonicTextbox.disabled = true;
                    //alert(or[0].childNodes[0].text);
                    oMnemonicHidden.value = or[0].childNodes[0].text;
                }
            }
            else 
                alert('There was a problem getting the information');
        }
    }
</script>
<table width="100%" height="100%" cellpadding="0" cellspacing="0" border="0">
    <tr>
        <td valign="top">
            <div style="height:100%; overflow:auto">
            <table width="100%" cellpadding="4" cellspacing="0" border="0">
                <tr>
                    <td nowrap>Application Name:</td>
                    <td nowrap><asp:TextBox ID="txtName" runat="server" CssClass="default" Width="300" MaxLength="100" /></td>
                </tr>
                <tr id="panCode" runat="server" visible="false">
                    <td nowrap>Application Code:</td>
                    <td nowrap><asp:TextBox ID="txtCode" runat="server" CssClass="default" Width="100" MaxLength="3" /></td>
                </tr>
                <tr id="panMnemonic" runat="server" visible="false">
                    <td nowrap>Mnemonic:</td>
                    <td nowrap>
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td><asp:TextBox ID="txtMnemonic" runat="server" Width="500" CssClass="default" /></td>
                            </tr>
                            <tr>
                                <td>
                                    <div id="divMnemonic" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                                        <asp:ListBox ID="lstMnemonic" runat="server" CssClass="default" />
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr id="panCostCenter" runat="server" visible="false">
                    <td nowrap>Cost Center:</td>
                    <td nowrap>
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td><asp:TextBox ID="txtCostCenter" runat="server" Width="200" CssClass="default" /></td>
                            </tr>
                            <tr>
                                <td>
                                    <div id="divCostCenter" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                                        <asp:ListBox ID="lstCostCenter" runat="server" CssClass="default" />
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr id="panDR" runat="server" visible="false">
                    <td nowrap>DR Criticality:</td>
                    <td nowrap>
                        <asp:RadioButton ID="radHigh" runat="server" CssClass="default" Text="1 - High" GroupName="dr" />&nbsp;
                        <asp:RadioButton ID="radLow" runat="server" CssClass="default" Text="2 - Low" GroupName="dr" />&nbsp;
                    </td>
                </tr>
                <tr>
                    <td nowrap>Departmental Manager:</td>
                    <td nowrap>
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td><asp:TextBox ID="txtOwner" runat="server" Width="300" CssClass="default" /></td>
                            </tr>
                            <tr>
                                <td>
                                    <div id="divOwner" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                                        <asp:ListBox ID="lstOwner" runat="server" CssClass="default" />
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td nowrap>Application Technical Lead:</td>
                    <td nowrap>
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td><asp:TextBox ID="txtPrimary" runat="server" Width="300" CssClass="default" /></td>
                            </tr>
                            <tr>
                                <td>
                                    <div id="divPrimary" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                                        <asp:ListBox ID="lstPrimary" runat="server" CssClass="default" />
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr style="display:none">
                    <td nowrap>Administrative Contact:</td>
                    <td nowrap>
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td><asp:TextBox ID="txtSecondary" runat="server" Width="300" CssClass="default" /></td>
                            </tr>
                            <tr>
                                <td>
                                    <div id="divSecondary" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                                        <asp:ListBox ID="lstSecondary" runat="server" CssClass="default" />
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr id="panAppOwner" runat="server" visible="false">
                    <td nowrap>Application Owner:</td>
                    <td nowrap>
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td><asp:TextBox ID="txtAppOwner" runat="server" Width="300" CssClass="default" /></td>
                            </tr>
                            <tr>
                                <td>
                                    <div id="divAppOwner" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                                        <asp:ListBox ID="lstAppOwner" runat="server" CssClass="default" />
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr id="panEngineer" runat="server" visible="false">
                    <td nowrap>Network Engineer:</td>
                    <td nowrap>
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
                    <td nowrap></td>
                    <td nowrap><asp:LinkButton ID="btnManager" runat="server" Text="User Not Appearing in a List? Click Here." /></td>
                </tr>
            </table>
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
<asp:HiddenField ID="hdnOwner" runat="server" />
<asp:HiddenField ID="hdnPrimary" runat="server" />
<asp:HiddenField ID="hdnSecondary" runat="server" />
<asp:HiddenField ID="hdnAppOwner" runat="server" />
<asp:HiddenField ID="hdnEngineer" runat="server" />
<asp:HiddenField ID="hdnMnemonic" runat="server" />
<asp:HiddenField ID="hdnCostCenter" runat="server" />
