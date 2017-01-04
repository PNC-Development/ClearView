<%@ Page Language="C#" MasterPageFile="~/window.Master" AutoEventWireup="true" CodeBehind="config_server_components.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.config_server_components" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" Runat="Server">
<script type="text/javascript">
    function MoveList(ddlFrom, ddlTo, oHidden, ddlUpdate) {
        ddlFrom = document.getElementById(ddlFrom);
        ddlTo = document.getElementById(ddlTo);
        ddlUpdate = document.getElementById(ddlUpdate);
	    if (ddlFrom.selectedIndex > -1) {
		    var oOption = document.createElement("OPTION");
		    ddlTo.add(oOption);
		    oOption.text = ddlFrom.options[ddlFrom.selectedIndex].text;
		    oOption.value = ddlFrom.options[ddlFrom.selectedIndex].value;
		    ddlFrom.remove(ddlFrom.selectedIndex);
		    ddlTo.selectedIndex = ddlTo.length - 1;
		    UpdateHidden(oHidden, ddlUpdate);
	    }
	    return false;
    }
    function UpdateHidden(oHidden, oControl) {
	    var oHidden = document.getElementById(oHidden);
	    oHidden.value = "";
	    for (var ii=0; ii<oControl.length; ii++) {
		    oHidden.value = oHidden.value + oControl.options[ii].value + "&";
	    }
    }
</script>
    <table cellpadding="4" cellspacing="0" border="0">
        <tr>
            <td valign="top">
                <table cellpadding="4" cellspacing="0" border="0">
                    <tr>
                        <td class="bold">Available Components:</td>
                    </tr>
                    <tr>
                        <td><asp:ListBox ID="lstAvailable" runat="server" CssClass="default" Width="325" Rows="15" SelectionMode="Single" /></td>
                    </tr>
                </table>
            </td>
            <td>
                <table cellpadding="4" cellspacing="0" border="0">
                    <tr>
                        <td><asp:ImageButton ID="btnAdd" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/arrowRight.gif" ToolTip="Add" OnClick="btnAdd_Click" /></td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td><asp:ImageButton ID="btnRemove" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/arrowLeft.gif" ToolTip="Remove" OnClick="btnRemove_Click" /></td>
                    </tr>
                </table>
            </td>
            <td valign="top">
                <table cellpadding="4" cellspacing="0" border="0">
                    <tr>
                        <td class="bold">Selected Components:</td>
                    </tr>
                    <tr>
                        <td><asp:ListBox ID="lstSelected" runat="server" CssClass="default" Width="325" Rows="15" SelectionMode="Single" /></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <br />
                <%=strMenuTab1 %>
                <div id="divMenu1">
                    <br />
                    <div style="display:none">
                        <div style="height:175px; overflow:auto;">
                            <asp:Table ID="tblPrerequisites" runat="server" CellPadding="3" CellSpacing="0">
                            </asp:Table>
                        </div>
                    </div>
                    <div style="display:none">
                        <div style="height:175px; overflow:auto;">
                            <asp:Table ID="tblExceptions" runat="server" CellPadding="3" CellSpacing="0">
                            </asp:Table>
                        </div>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
