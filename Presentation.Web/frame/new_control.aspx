<%@ Page Language="C#" MasterPageFile="~/frame.Master" AutoEventWireup="true" CodeBehind="new_control.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.new_control" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" runat="server">
<script type="text/javascript">
    function MoveOrderUp(ddl, oHidden) {
	    if (ddl.selectedIndex > -1) {
		    var oldIndex = ddl.selectedIndex - 1;
		    if (oldIndex > -1) {
			    var oldValue = ddl.options[ddl.selectedIndex].value;
			    var oldText = ddl.options[ddl.selectedIndex].text;
			    ddl.options[ddl.selectedIndex].value = ddl.options[ddl.selectedIndex - 1].value;
			    ddl.options[ddl.selectedIndex].text = ddl.options[ddl.selectedIndex - 1].text;
			    ddl.options[ddl.selectedIndex - 1].value = oldValue;
			    ddl.options[ddl.selectedIndex - 1].text = oldText;
			    ddl.selectedIndex = oldIndex;
			    UpdateOrder(ddl, oHidden);
		    }
	    }
	    return false;
    }
    function MoveOrderDown(ddl, oHidden) {
	    if (ddl.selectedIndex > -1) {
		    var oldIndex = ddl.selectedIndex + 1;
		    if (oldIndex < ddl.length) {
			    var oldValue = ddl.options[ddl.selectedIndex].value;
			    var oldText = ddl.options[ddl.selectedIndex].text;
			    ddl.options[ddl.selectedIndex].value = ddl.options[ddl.selectedIndex + 1].value;
			    ddl.options[ddl.selectedIndex].text = ddl.options[ddl.selectedIndex + 1].text;
			    ddl.options[ddl.selectedIndex + 1].value = oldValue;
			    ddl.options[ddl.selectedIndex + 1].text = oldText;
			    ddl.selectedIndex = oldIndex;
			    UpdateOrder(ddl, oHidden);
		    }
	    }
	    return false;
    }
    function MoveOrderOut(ddl, oHidden) {
        if (ddl.selectedIndex > -1) {
	        var oldIndex = ddl.selectedIndex;
	        ddl.remove(ddl.selectedIndex);
	        ddl.selectedIndex = oldIndex + 1;
		    UpdateOrder(ddl, oHidden);
        }
        return false;
    }
    function MoveOrderIn(ddl, oHidden, oText) {
		var oOption = document.createElement("OPTION");
		oText = document.getElementById(oText);
		ddl.add(oOption);
		oOption.text = oText.value;
		oOption.value = oText.value;
		ddl.selectedIndex = ddl.length - 1;
	    UpdateOrder(ddl, oHidden);
	    oText.value = "";
	    oText.focus();
    	return false;
	}
    function UpdateOrder(oDDL, oHidden) {
	    oHidden = document.getElementById(oHidden);
	    oHidden.value = "";
	    for (var ii=0; ii<oDDL.length; ii++) {
		    oHidden.value = oHidden.value + oDDL.options[ii].value + ";";
	    }
    }
</script>
<asp:Panel ID="panFields" Height="100%" runat="server" Visible="false">
    <div style="width:100%; height:100%; overflow:auto">
        <%=strFields %>
    </div>
</asp:Panel>
<asp:Panel ID="panField" runat="server" Visible="false">
    <table width="100%" cellpadding="4" cellspacing="3" border="0">
        <tr>
            <td class="header"><asp:Label ID="lblField" runat="server" CssClass="header" /> Properties</td>
            <td align="right">
                <asp:Label ID="lblID" runat="server" CssClass="reddefault" Visible="false" />
                <asp:Button ID="btnBack" runat="server" CssClass="default" width="100" Text="<<  Back" OnClick="btnBack_Click" Visible="false" />
            </td>
        </tr>
    </table>
    <asp:Panel ID="panQuestion" runat="server" Visible="false">
        <table cellpadding="3" cellspacing="2" border="0">
            <tr>
                <td width="125" background="/images/spacer.gif"><asp:Literal ID="litQuestion" runat="server" Text="Question" />:<span class="required">&nbsp;*</span></td>
                <td><asp:TextBox ID="txtQuestion" runat="server" CssClass="default" Width="400" TextMode="MultiLine" Rows="3" /></td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="panValues" runat="server" Visible="false">
        <table cellpadding="3" cellspacing="2" border="0">
            <tr>
                <td valign="top" width="125" background="/images/spacer.gif">Values:<span class="required">&nbsp;*</span></td>
                <td rowspan="2">
                    <table cellpadding="2" cellspacing="0" border="0">
                        <tr>
                            <td><asp:TextBox ID="txtAdd" runat="server" CssClass="default" Width="350" /></td>
                            <td><asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="default" Width="40" /></td>
                        </tr>
                        <tr>
                            <td rowspan="3"><asp:ListBox ID="lstValues" runat="server" CssClass="default" Width="350" Rows="7" /></td>
                            <td valign="bottom"><asp:ImageButton ID="btnUp" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/up.gif" /></td>
                        </tr>
                        <tr>
                            <td valign="middle"><asp:ImageButton ID="btnOut" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/dl.gif" /></td>
                        </tr>
                        <tr>
                            <td valign="top"><asp:ImageButton ID="btnDown" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/dn.gif" /></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td valign="top" width="125" background="/images/spacer.gif"><b>NOTE:</b> To add a value, enter the text and click the &quot;Add&quot; button. Use the arrow buttons to re-order the values and the X button to delete.</td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="panText" runat="server" Visible="false">
        <table cellpadding="3" cellspacing="2" border="0">
            <tr>
                <td width="125" background="/images/spacer.gif"><asp:Literal ID="litText" runat="server" />:</td>
                <td colspan="2"><asp:TextBox ID="txtText" runat="server" CssClass="default" /></td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="panURL" runat="server" Visible="false">
        <table cellpadding="3" cellspacing="2" border="0">
            <tr>
                <td width="125" background="/images/spacer.gif">URL:</td>
                <td colspan="2"><asp:TextBox ID="txtURL" runat="server" CssClass="default" width="400" /></td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="panLength" runat="server" Visible="false">
        <table cellpadding="3" cellspacing="2" border="0">
            <tr>
                <td width="125" background="/images/spacer.gif">Max Characters:<span class="required">&nbsp;*</span></td>
                <td><asp:TextBox ID="txtLength" runat="server" CssClass="default" width="50" MaxLength="3" /></td>
                <td>(The maximum number of characters allowed in the control)</td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="panWidth" runat="server" Visible="false">
        <table cellpadding="3" cellspacing="2" border="0">
            <tr>
                <td width="125" background="/images/spacer.gif">Width:<span class="required">&nbsp;*</span></td>
                <td><asp:TextBox ID="txtWidth" runat="server" CssClass="default" width="50" MaxLength="3" /></td>
                <td>(The width of the control)</td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="panRows" runat="server" Visible="false">
        <table cellpadding="3" cellspacing="2" border="0">
            <tr>
                <td width="125" background="/images/spacer.gif">Rows:<span class="required">&nbsp;*</span></td>
                <td><asp:TextBox ID="txtRows" runat="server" CssClass="default" width="50" MaxLength="3"/></td>
                <td>(The number of rows for the control)</td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="panCheck" runat="server" Visible="false">
        <table cellpadding="3" cellspacing="2" border="0">
            <tr>
                <td width="125" background="/images/spacer.gif">Checked:<span class="required">&nbsp;*</span></td>
                <td>
                    <asp:RadioButton ID="radCheckYes" runat="server" CssClass="default" Text="Yes" GroupName="check" /> 
                    <asp:RadioButton ID="radCheckNo" runat="server" CssClass="default" Text="No" GroupName="check" /> 
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="panDirection" runat="server" Visible="false">
        <table cellpadding="3" cellspacing="2" border="0">
            <tr>
                <td width="125" background="/images/spacer.gif">Direction:<span class="required">&nbsp;*</span></td>
                <td>
                    <asp:RadioButton ID="radDirectionV" runat="server" CssClass="default" Text="Vertical" GroupName="direction" /> 
                    <asp:RadioButton ID="radDirectionH" runat="server" CssClass="default" Text="Horizontal" GroupName="direction" /> 
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="panMultiple" runat="server" Visible="false">
        <table cellpadding="3" cellspacing="2" border="0">
            <tr>
                <td width="125" background="/images/spacer.gif">Select Multiple:<span class="required">&nbsp;*</span></td>
                <td>
                    <asp:RadioButton ID="radMultipleYes" runat="server" CssClass="default" Text="Yes" GroupName="multiple" /> 
                    <asp:RadioButton ID="radMultipleNo" runat="server" CssClass="default" Text="No" GroupName="multiple" /> 
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="panLocation" runat="server" Visible="false">
        <table cellpadding="3" cellspacing="2" border="0">
            <tr>
                <td width="125" background="/images/spacer.gif">Default Location:<span class="required">&nbsp;*</span></td>
                <td>
                    <asp:DropDownList ID="ddlLocation" runat="server" Width="300" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="panRange" runat="server" Visible="false">
        <table cellpadding="3" cellspacing="2" border="0">
            <tr>
                <td width="125" background="/images/spacer.gif"></td>
                <td>Date must be at least <asp:TextBox ID="txtMinimum" runat="server" CssClass="default" width="50" MaxLength="3" /> days from current date</td>
            </tr>
            <tr>
                <td width="125" background="/images/spacer.gif"></td>
                <td class="footer">NOTE: Set the number of days to 0 to skip this setting</td>
            </tr>
            <tr>
                <td width="125" background="/images/spacer.gif"></td>
                <td>Date must be at most <asp:TextBox ID="txtMaximum" runat="server" CssClass="default" width="50" MaxLength="3" /> days from current date</td>
            </tr>
            <tr>
                <td width="125" background="/images/spacer.gif"></td>
                <td class="footer">NOTE: Set the number of days to 0 to skip this setting</td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="panRequired" runat="server" Visible="false">
        <table cellpadding="3" cellspacing="2" border="0">
            <tr>
                <td width="125" background="/images/spacer.gif">Required:<span class="required">&nbsp;*</span></td>
                <td>
                    <asp:RadioButton ID="radRequiredYes" runat="server" CssClass="default" Text="Yes" GroupName="required" /> 
                    <asp:RadioButton ID="radRequiredNo" runat="server" CssClass="default" Text="No" GroupName="required" /> 
                </td>
            </tr>
            <tr id="divRequired" runat="server" style="display:none">
                <td width="125" background="/images/spacer.gif">Required Text:<span class="required">&nbsp;*</span></td>
                <td><asp:TextBox ID="txtRequired" runat="server" CssClass="default" Width="400" TextMode="MultiLine" Rows="3" /></td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="panTip" runat="server" Visible="false">
        <table cellpadding="3" cellspacing="2" border="0">
            <tr>
                <td width="125" background="/images/spacer.gif">Help Text:</td>
                <td><asp:TextBox ID="txtTip" runat="server" CssClass="default" Width="400" TextMode="MultiLine" Rows="3" /></td>
            </tr>
            <tr>
                <td></td>
                <td><b>NOTE:</b> Help text is available my mousing over the specific control on your service request.</td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="panWrite" runat="server" Visible="false">
        <table cellpadding="3" cellspacing="2" border="0">
        <tr>
            <td width="125" background="/images/spacer.gif"></td>
            <td colspan="2"><asp:CheckBox ID="chkWrite" runat="server" CssClass="default" Text="The assigned resource can update this field (<i>default = unchecked</i>)" /></td>
        </tr>
        </table>
    </asp:Panel>
    <table width="550" cellpadding="3" cellspacing="2" border="0">
        <tr>
            <td width="125" background="/images/spacer.gif"></td>
            <td>
                <asp:Button ID="btnSave" runat="server" CssClass="default" width="100" Text="Save" OnClick="btnSave_Click" Enabled="false" /> 
                <asp:Button ID="btnDelete" runat="server" CssClass="default" width="100" Text="Delete" OnClick="btnDelete_Click" Enabled="false" /> 
            </td>
            <td align="right" class="required">* = Required Field</td>
        </tr>
    </table>
</asp:Panel>
<input type="hidden" id="hdnValues" runat="server" />
</asp:Content>
