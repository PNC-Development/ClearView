<%@ Page Language="C#" MasterPageFile="~/frame.Master" AutoEventWireup="true" CodeBehind="service_task.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.service_task" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" runat="server">
<script type="text/javascript">
    function ChangeMinutes(oText, boolMinute) {
        oText = document.getElementById(oText);
        var oHour = document.getElementById("tdHour");
        var oMinute = document.getElementById("tdMin");
        var oHidden = document.getElementById("hdnMinutes");
        var dblValue = 0.00;
        if (isNumber(oText.value) == true)
            dblValue = parseFloat(oText.value);
        if (boolMinute == true) {
            oHidden.value = "1";
            dblValue = dblValue * 60.00;
            oText.value = dblValue.toFixed(2);
            oHour.style.border = "outset 2px #FFFFFF";
            oMinute.style.border = "inset 2px #FFFFFF";
        }
        else {
            oHidden.value = "0";
            dblValue = dblValue / 60.00;
            oText.value = dblValue.toFixed(2);
            oHour.style.border = "inset 2px #FFFFFF";
            oMinute.style.border = "outset 2px #FFFFFF";
        }
    }
</script>
    <table width="100%" cellpadding="4" cellspacing="3" border="0">
        <tr>
            <td nowrap>Service Name:</td>
            <td width="100%"><asp:Label ID="lblService" runat="server" CssClass="default" /></td>
        </tr>
        <asp:Panel ID="panParent" runat="server" Visible="false">
        <tr>
            <td nowrap>Parent Task:</td>
            <td width="100%"><asp:Label ID="lblParent" runat="server" CssClass="default" /></td>
        </tr>
        </asp:Panel>
        <tr>
            <td nowrap>Task Name:</td>
            <td width="100%"><asp:TextBox ID="txtName" runat="server" CssClass="default" Width="300" MaxLength="300" /></td>
        </tr>
        <tr>
            <td nowrap>Time Allocated:</td>
            <td width="100%">
                <table cellpadding="2" cellspacing="1" border="0">
                    <tr>
                        <td nowrap><asp:TextBox ID="txtHours" runat="server" CssClass="default" Width="80" MaxLength="8" Text="0" /></td>
                        <td>&nbsp;</td>
                        <td id="tdHour" nowrap style="border:inset 2px #FFFFFF; cursor:hand" title="Change value to HOURS" onclick="ChangeMinutes('<%=txtHours.ClientID %>',false);">Hours</td>
                        <td id="tdMin" nowrap style="border:outset 2px #FFFFFF; cursor:hand" title="Change value to MINUTES" onclick="ChangeMinutes('<%=txtHours.ClientID %>',true);">Minutes</td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td nowrap>Enabled:</td>
            <td width="100%"><asp:CheckBox ID="chkEnabled" runat="server" CssClass="default" Checked="true" /></td>
        </tr>
        <tr>
            <td nowrap>&nbsp;</td>
            <td width="100%">
                <asp:Button ID="btnSave" runat="server" CssClass="default" Width="100" Text="Save" OnClick="btnSave_Click" Enabled="false" /> 
                <asp:Button ID="btnDelete" runat="server" CssClass="default" Width="100" Text="Delete" OnClick="btnDelete_Click" Enabled="false" /> 
            </td>
        </tr>
    </table>
<input type="hidden" id="hdnMinutes" name="hdnMinutes" value="0" />
</asp:Content>
