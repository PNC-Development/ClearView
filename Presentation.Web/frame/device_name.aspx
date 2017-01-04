<%@ Page Language="C#" MasterPageFile="~/frame.Master" AutoEventWireup="true" CodeBehind="device_name.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.device_name" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" Runat="Server">
<script type="text/javascript">
    var OKtoChange = false;
    function UpdateNamingDDL(oDDL,oLabel,strType) {
        oLabel = document.getElementById(oLabel);
        oLabel.innerText = oDDL.options[oDDL.selectedIndex].value;
        UpdateNaming(strType);
    }
    function UpdateNamingMnemonic(oHidden,oText,oLabel,strType) {
        oHidden = document.getElementById(oHidden);
        if (trim(oHidden.value) == "" || trim(oHidden.value) == "0" || isNumber(oHidden.value) == false) {
        }
        else {
            oText = document.getElementById(oText);
            var strMnemonic = oText.value;
            if (strMnemonic.indexOf("-") > -1) {
                strMnemonic = trim(strMnemonic.substring(0, strMnemonic.indexOf("-")));
                oLabel = document.getElementById(oLabel);
                oLabel.innerText = strMnemonic;
                UpdateNaming(strType);
            }
            else
                alert('Could not retrieve Mnemonic');
        }
    }
    function UpdateNamingText(oText,oLabel,strType) {
        oLabel = document.getElementById(oLabel);
        oLabel.innerText = oText.value;
        UpdateNaming(strType);
    }
    function UpdateNaming(strType) {
        var oNew = document.getElementById('<%=lblNew.ClientID %>');
        var oName = document.getElementById('<%=lblName.ClientID %>');
        var oSubmit = document.getElementById('<%=btnSubmit.ClientID %>');
        var oReset = document.getElementById('<%=btnReset.ClientID %>');
        if (strType == "PNC")
        {
            var olblPNCOS = document.getElementById('<%=lblPNCOS.ClientID %>');
            var olblPNCLocation = document.getElementById('<%=lblPNCLocation.ClientID %>');
            var olblPNCMnemonic = document.getElementById('<%=lblPNCMnemonic.ClientID %>');
            var olblPNCEnvironment = document.getElementById('<%=lblPNCEnvironment.ClientID %>');
            var olblPNCSequence = document.getElementById('<%=lblPNCSequence.ClientID %>');
            var olblPNCFunction = document.getElementById('<%=lblPNCFunction.ClientID %>');
            var olblPNCSpecific = document.getElementById('<%=lblPNCSpecific.ClientID %>');
            oNew.innerText = olblPNCOS.innerText + olblPNCLocation.innerText + olblPNCMnemonic.innerText + olblPNCEnvironment.innerText + olblPNCSequence.innerText + olblPNCFunction.innerText + olblPNCSpecific.innerText;
        }
        if (strType == "NCB")
        {
            var olblNCBState = document.getElementById('<%=lblNCBState.ClientID %>');
            var olblNCBCity = document.getElementById('<%=lblNCBCity.ClientID %>');
            var olblNCBFunction = document.getElementById('<%=lblNCBFunction.ClientID %>');
            var olblNCBSiteCode = document.getElementById('<%=lblNCBSiteCode.ClientID %>');
            var olblNCBSequence = document.getElementById('<%=lblNCBSequence.ClientID %>');
            oNew.innerText = olblNCBState.innerText + olblNCBCity.innerText + olblNCBFunction.innerText + olblNCBSiteCode.innerText + olblNCBSequence.innerText;
        }
        if (strType == "WORKSTATION")
        {
            var olblWorkstationEnvironment = document.getElementById('<%=lblWorkstationEnvironment.ClientID %>');
            var olblWorkstationCode = document.getElementById('<%=lblWorkstationCode.ClientID %>');
            var olblWorkstationIdentifier = document.getElementById('<%=lblWorkstationIdentifier.ClientID %>');
            var olblWorkstationSequence = document.getElementById('<%=lblWorkstationSequence.ClientID %>');
            oNew.innerText = olblWorkstationEnvironment.innerText + olblWorkstationCode.innerText + olblWorkstationIdentifier.innerText + olblWorkstationSequence.innerText;
        }
        var oHidden = document.getElementById('<%=hdnName.ClientID %>');
        oHidden.value = oNew.innerText;
        if (oNew.innerText == oName.innerText) {
            oNew.className = "header";
            OKtoChange = false;
            //oSubmit.disabled = true;
            //oReset.disabled = true;
        }
        else {
            oNew.className = "redheader";
            OKtoChange = true;
            //oSubmit.disabled = false;
            //oReset.disabled = false;
            oSubmit.focus();
        }
    }
    function IsOKtoChange() {
        if (OKtoChange == false) {
            alert('You have not made any changes to this device name!\n\nPlease make a change before clicking the Submit button.');
            return false;
        }
        else
            return true;
    }
    function IsOKtoReset(oButton) {
        if ((OKtoChange == false) || (OKtoChange == true && confirm('WARNING: This will reset the device name to the original name / configuration!\n\nAre you sure you want to continue?') == true))
        {
            ProcessButton(oButton,'Resetting...','100');
            return true;
        }
        else
            return false;
    }
    
</script>
<table width="550" cellpadding="0" cellspacing="5" border="0">
    <tr>
        <td rowspan="2"><img src="/images/user_add.gif" border="0" align="absmiddle" /></td>
        <td class="header" valign="bottom">Change a Device Name</td>
    </tr>
    <tr>
        <td valign="top">To change a device name, please answer the following questions...</td>
    </tr>
</table>
<table width="550" cellpadding="5" cellspacing="3" border="0">
    <tr>
        <td colspan="2">Do you want to clear the device name or change it?</td>
    </tr>
    <tr>
        <td nowrap>Name:</td>
        <td width="100%"><asp:Label ID="lblName" runat="server" CssClass="bold" /></td>
    </tr>
    <tr>
        <td colspan="2">
            <table width="500" cellpadding="3" cellspacing="0" border="0">
                <tr>
                    <td>
                        <asp:RadioButtonList ID="radClear" runat="server" CssClass="default" RepeatColumns="1" RepeatDirection="Vertical" AutoPostBack="true" OnSelectedIndexChanged="radClear_Change">
                            <asp:ListItem Value="Clear" Text="Clear the Device Name" />
                            <asp:ListItem Value="Change" Text="Change the Device Name" />
                        </asp:RadioButtonList>
                    </td>
                    <td align="right">
                        <table id="panID" runat="Server" visible="false" cellpadding="3" cellspacing="0" border="0" class="footer">
                            <tr>
                                <td>ID #</td>
                                <td><asp:Label ID="lblID" runat="server" CssClass="default" /></td>
                            </tr>
                            <tr>
                                <td>Name ID #</td>
                                <td><asp:Label ID="lblNameID" runat="server" CssClass="default" /></td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr id="panExists" runat="server" visible="false">
        <td colspan="2">
            <table cellpadding="3" cellspacing="2" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
                <tr>
                    <td rowspan="5" valign="top"><img src="/images/hugeAlert.gif" border="0" align="absmiddle" /></td>
                    <td class="header" valign="bottom"><asp:Label ID="lblAlready" runat="server" CssClass="header" /> Already Exists</td>
                </tr>
                <tr>
                    <td valign="top">The device name you entered already exists.</td>
                </tr>
                <tr>
                    <td valign="top">Are you sure you want to take this name from the other device?</td>
                </tr>
                <tr>
                    <td valign="top"><b>WARNING:</b> The other device will have no name associated with it.</td>
                </tr>
                <tr>
                    <td valign="top">
                        <table cellpadding="3" cellspacing="0" border="0">
                            <tr>
                                <td><asp:Button ID="btnAlreadyChange" runat="server" CssClass="default" Width="200" Text="Continue - Change Name" OnClick="btnAlreadyChange_Click" /></td>
                                <td><asp:Button ID="btnAlreadyCancel" runat="server" CssClass="default" Width="200" Text="Cancel - Discard Changes" OnClick="btnAlreadyCancel_Click" /></td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr id="panError" runat="server" visible="false">
        <td colspan="2">
            <table cellpadding="3" cellspacing="2" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
                <tr>
                    <td rowspan="3" valign="top"><img src="/images/bigError.gif" border="0" align="absmiddle" /></td>
                    <td class="header" valign="bottom">Unable to Change the Name</td>
                </tr>
                <tr>
                    <td valign="top"><asp:Label ID="lblError" runat="server" CssClass="default" /></td>
                </tr>
            </table>
        </td>
    </tr>
    <tr id="panClear" runat="server" visible="false">
        <td colspan="2">
            <table cellpadding="3" cellspacing="2" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
                <tr>
                    <td rowspan="3" valign="top"><img src="/images/hugeAlert.gif" border="0" align="absmiddle" /></td>
                    <td class="header" valign="bottom">WARNING - Please Read!</td>
                </tr>
                <tr>
                    <td valign="top">Clearing a device name will make the device unsearchable in ClearView and could impact reporting.</td>
                </tr>
                <tr>
                    <td valign="top">Click &quot;Submit&quot; if you are sure you want to clear this device name.</td>
                </tr>
            </table>
        </td>
    </tr>
    <tr id="panChange" runat="server" visible="false">
        <td colspan="2">
            <table cellpadding="3" cellspacing="0" border="0">
                <tr>
                    <td nowrap class="biggerbold">Type:</td>
                    <td>
                        <asp:DropDownList ID="ddlType" runat="server" CssClass="biggerbold" Enabled="false">
                            <asp:ListItem Value="-1" Text="Workstation" />
                            <asp:ListItem Value="0" Text="Server (National City)" />
                            <asp:ListItem Value="1" Text="Server (PNC Financial Services)" />
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr id="panPNC" runat="server" visible="false">
                    <td colspan="2">
                        <table cellpadding="5" cellspacing="0" border="0">
                            <tr>
                                <td><b>Option</b></td>
                                <td><b>Naming</b></td>
                                <td><b>Value</b></td>
                            </tr>
                            <tr>
                                <td>Operating System:</td>
                                <td><asp:Label ID="lblPNCOS" runat="server" CssClass="default" /></td>
                                <td><asp:DropDownList ID="ddlPNCOS" runat="server" CssClass="default" Width="250" /></td>
                            </tr>
                            <tr>
                                <td>Location:</td>
                                <td><asp:Label ID="lblPNCLocation" runat="server" CssClass="default" /></td>
                                <td><asp:DropDownList ID="ddlPNCLocation" runat="server" CssClass="default" /></td>
                            </tr>
                            <tr>
                                <td>Mnemonic:</td>
                                <td><asp:Label ID="lblPNCMnemonic" runat="server" CssClass="default" /></td>
                                <td>
                                    <table cellpadding="0" cellspacing="0" border="0">
                                        <tr>
                                            <td><asp:TextBox ID="txtPNCMnemonic" runat="server" Width="350" CssClass="default" /></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div id="divPNCMnemonic" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                                                    <asp:ListBox ID="lstPNCMnemonic" runat="server" CssClass="default" />
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                    <input type="hidden" id="hdnPNCMnemonic" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>Environment:</td>
                                <td><asp:Label ID="lblPNCEnvironment" runat="server" CssClass="default" /></td>
                                <td><asp:DropDownList ID="ddlPNCEnvironment" runat="server" CssClass="default" /></td>
                            </tr>
                            <tr>
                                <td>Sequence:</td>
                                <td><asp:Label ID="lblPNCSequence" runat="server" CssClass="default" /></td>
                                <td><asp:TextBox ID="txtPNCSequence" runat="server" CssClass="default" Width="50" MaxLength="2" /></td>
                            </tr>
                            <tr>
                                <td>Function:</td>
                                <td><asp:Label ID="lblPNCFunction" runat="server" CssClass="default" /></td>
                                <td>
                                    <asp:DropDownList ID="ddlPNCFunction" runat="server" CssClass="default">
                                        <asp:ListItem Value="C" Text="Domain Controller" />
                                        <asp:ListItem Value="D" Text="Database Server" />
                                        <asp:ListItem Value="W" Text="Web Server" />
                                        <asp:ListItem Value="M" Text="Mail Server" />
                                        <asp:ListItem Value="X" Text="Citrix Server" />
                                        <asp:ListItem Value="I" Text="Sametime Server" />
                                        <asp:ListItem Value="S" Text="SMTP Server" />
                                        <asp:ListItem Value="H" Text="Hub Server" />
                                        <asp:ListItem Value="U" Text="Utility / Notes Server" />
                                        <asp:ListItem Value="A" Text="Default - Application Server" />
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>Specific:</td>
                                <td><asp:Label ID="lblPNCSpecific" runat="server" CssClass="default" /></td>
                                <td>
                                    <asp:DropDownList ID="ddlPNCSpecific" runat="server" CssClass="default">
                                        <asp:ListItem Value="Z" Text="Cluster" />
                                        <asp:ListItem Value="B" Text="Mail Server Backup" />
                                        <asp:ListItem Value="F" Text="Forest Root Domain Controller" />
                                        <asp:ListItem Value="" Text="Default - None of the Above" />
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr id="panNCB" runat="server" visible="false">
                    <td colspan="2">
                        <table cellpadding="5" cellspacing="0" border="0">
                            <tr>
                                <td><b>Option</b></td>
                                <td><b>Naming</b></td>
                                <td><b>Value</b></td>
                            </tr>
                            <tr>
                                <td>State:</td>
                                <td><asp:Label ID="lblNCBState" runat="server" CssClass="default" /></td>
                                <td><asp:TextBox ID="txtNCBState" runat="server" CssClass="default" Width="50" MaxLength="2" /> <span class="footer">Example: OH</span></td>
                            </tr>
                            <tr>
                                <td>City:</td>
                                <td><asp:Label ID="lblNCBCity" runat="server" CssClass="default" /></td>
                                <td><asp:TextBox ID="txtNCBCity" runat="server" CssClass="default" Width="75" MaxLength="3" /> <span class="footer">Example: CLE</span></td>
                            </tr>
                            <tr>
                                <td>Function:</td>
                                <td><asp:Label ID="lblNCBFunction" runat="server" CssClass="default" /></td>
                                <td><asp:DropDownList ID="ddlNCBFunction" runat="server" CssClass="default" /></td>
                            </tr>
                            <tr>
                                <td>Site Code:</td>
                                <td><asp:Label ID="lblNCBSiteCode" runat="server" CssClass="default" /></td>
                                <td>
                                    <asp:DropDownList ID="ddlNCBSiteCode" runat="server" CssClass="default">
                                        <asp:ListItem Value="00" Text="00 : DR (Core & Inner Layer)" />
                                        <asp:ListItem Value="10" Text="10 : Production & QA (Core) / DR (Outer Layer)" />
                                        <asp:ListItem Value="11" Text="11 : Production (Core)" />
                                        <asp:ListItem Value="12" Text="12 : Production (Core)" />
                                        <asp:ListItem Value="13" Text="13 : Production (Core)" />
                                        <asp:ListItem Value="14" Text="14 : Production (Core)" />
                                        <asp:ListItem Value="15" Text="15 : Production (Core)" />
                                        <asp:ListItem Value="20" Text="20 : Production (Core)" />
                                        <asp:ListItem Value="40" Text="40 : Dev & Test (Core)" />
                                        <asp:ListItem Value="41" Text="41 : Dev & Test (Core)" />
                                        <asp:ListItem Value="42" Text="42 : Dev & Test (Core)" />
                                        <asp:ListItem Value="43" Text="43 : Dev & Test (Core)" />
                                        <asp:ListItem Value="45" Text="45 : Dev & Test (Core)" />
                                        <asp:ListItem Value="47" Text="47 : Dev (Offshore)" />
                                        <asp:ListItem Value="49" Text="49 : Test (Offshore)" />
                                        <asp:ListItem Value="50" Text="50 : Production (E-Commerce Outer)" />
                                        <asp:ListItem Value="51" Text="51 : Production (E-Commerce Inner)" />
                                        <asp:ListItem Value="52" Text="52 : Test (E-Commerce Outer)" />
                                        <asp:ListItem Value="53" Text="53 : Test (E-Commerce Inner)" />
                                        <asp:ListItem Value="54" Text="54 : Dev (E-Commerce Outer)" />
                                        <asp:ListItem Value="55" Text="55 : Dev (E-Commerce Inner)" />
                                        <asp:ListItem Value="56" Text="56 : DR (E-Commerce Outer)" />
                                        <asp:ListItem Value="90" Text="90 : Production (NCMC Core)" />
                                        <asp:ListItem Value="92" Text="92 : Production (NCMC E-Commerce)" />
                                        <asp:ListItem Value="94" Text="94 : Test (NCMC Core)" />
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>Sequence:</td>
                                <td><asp:Label ID="lblNCBSequence" runat="server" CssClass="default" /></td>
                                <td><asp:TextBox ID="txtNCBSequence" runat="server" CssClass="default" Width="50" MaxLength="2" /> <span class="footer">(00 - ZZ)</span></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr id="panWorkstation" runat="server" visible="false">
                    <td colspan="2">
                        <table cellpadding="5" cellspacing="0" border="0">
                            <tr>
                                <td><b>Option</b></td>
                                <td><b>Naming</b></td>
                                <td><b>Value</b></td>
                            </tr>
                            <tr>
                                <td>Environment:</td>
                                <td><asp:Label ID="lblWorkstationEnvironment" runat="server" CssClass="default" /></td>
                                <td>
                                    <asp:DropDownList ID="ddlWorkstationEnvironment" runat="server" CssClass="default">
                                        <asp:ListItem Value="W" Text="Windows : Production" />
                                        <asp:ListItem Value="T" Text="Windows : Test / Dev" />
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>Operating System:</td>
                                <td><asp:Label ID="lblWorkstationCode" runat="server" CssClass="default" /></td>
                                <td>
                                    <asp:DropDownList ID="ddlWorkstationCode" runat="server" CssClass="default">
                                        <asp:ListItem Value="XP" Text="Windows XP" />
                                        <asp:ListItem Value="2K" Text="Windows 2000" />
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>Identifier:</td>
                                <td><asp:Label ID="lblWorkstationIdentifier" runat="server" CssClass="default" Text="V" /></td>
                                <td>V</td>
                            </tr>
                            <tr>
                                <td>Sequence:</td>
                                <td><asp:Label ID="lblWorkstationSequence" runat="server" CssClass="default" /></td>
                                <td><asp:TextBox ID="txtWorkstationSequence" runat="server" CssClass="default" Width="100" MaxLength="6" /> <span class="footer">(000000 - ZZZZZZ)</span></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">&nbsp;</td>
                </tr>
                <tr>
                    <td nowrap><b>New Device Name:</b></td>
                    <td><asp:Label ID="lblNew" runat="server" CssClass="header" /></td>
                </tr>
            </table>
        </td>
    </tr>
    <tr id="panButtons" runat="server" visible="false">
        <td colspan="2">
            <table width="100%" cellpadding="5" cellspacing="3" border="0">
                <tr>
                    <td>
                        <asp:Button ID="btnSubmit" runat="server" CssClass="default" Width="75" Text="Submit" OnClick="btnSubmit_Click" /> 
                        <asp:Button ID="btnCancel" runat="server" CssClass="default" Width="75" Text="Cancel" OnClick="btnCancel_Click" />
                    </td>
                    <td align="right"><asp:Button ID="btnReset" runat="server" CssClass="default" Width="75" Text="Reset" OnClick="btnReset_Click" /></td>
                </tr>
            </table>
        </td>
    </tr>
</table>
<asp:HiddenField ID="hdnName" runat="server" />
</asp:Content>
