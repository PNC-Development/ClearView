<%@ Page Language="C#" MasterPageFile="~/frame.Master" AutoEventWireup="true" CodeBehind="service_task_order.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.service_task_order" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" runat="server">
<script type="text/javascript">
    function MoveOrderUp(ddl) {
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
			    UpdateOrder(ddl);
		    }
	    }
	    return false;
    }
    function MoveOrderDown(ddl) {
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
			    UpdateOrder(ddl);
		    }
	    }
	    return false;
    }
    function UpdateOrder(oDDL) {
	    var oHidden = document.getElementById("hdnUpdateOrder");
	    oHidden.value = "";
	    for (var ii=0; ii<oDDL.length; ii++) {
		    oHidden.value = oHidden.value + oDDL.options[ii].value + "&";
	    }
    }
    function Update(oHidden, strControl) {
        oHidden = document.getElementById(oHidden);
        window.top.UpdateWindow(oHidden.value, strControl, null, null);
        window.top.HidePanel();
        return false;
    }
</script>
    <table width="100%" cellpadding="4" cellspacing="3" border="0">
        <tr>
            <td align="center" colspan="2"><asp:ListBox ID="lstOrder" runat="server" Width="350" CssClass="default" Rows="20"/></td>
        </tr>
        <tr>
            <td align="center">
                <asp:ImageButton ID="imgOrderUp" runat="server" ImageUrl="/admin/images/up.gif" ToolTip="Move Up"/>
                &nbsp;&nbsp;&nbsp;&nbsp;
                <asp:ImageButton ID="imgOrderDown" runat="server" ImageUrl="/admin/images/dn.gif" ToolTip="Move Down"/>
            </td>
            <td align="right"><asp:Button ID="btnSave" runat="server" CssClass="default" Width="100" Text="Save" OnClick="btnSave_Click" /></td>
        </tr>
    </table>
<input id="hdnUpdateOrder" name="hdnUpdateOrder" type="hidden"/>
</asp:Content>
