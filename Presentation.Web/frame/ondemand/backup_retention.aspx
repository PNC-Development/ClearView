<%@ Page Language="C#" MasterPageFile="~/frame.Master" AutoEventWireup="true" CodeBehind="backup_retention.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.backup_retention" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" runat="server">
<script type="text/javascript">
    function Reload() {
        var strParent = parent.location.toString();
        if (parent.location.toString().indexOf("&child") == -1)
            strParent += "&child=true";
        parent.navigate(strParent);
        parent.HidePanel();
    }
    function ChangeFrequency461(oDDL, oDiv) {
        oDiv = document.getElementById(oDiv);
        if (oDDL.selectedIndex > 0)
            oDiv.style.display = "inline";
        else
            oDiv.style.display = "none";
    }
    function CheckRetention461(oDDL, oText) {
        oDDL = document.getElementById(oDDL);
        if (oDDL.selectedIndex < 4)
            return ValidateNumber(oText, 'Please enter a valid number for the retention period');
        else
            return true;
    }
    function CheckFrequency461(oDDL, oText) {
        oDDL = document.getElementById(oDDL);
        if (oDDL.selectedIndex > 0)
            return ValidateText(oText, 'Please enter a valid frequency occurence');
        else
            return true;
    }
</script>
<table cellpadding="3" cellspacing="2" border="0">
    <tr>
        <td class="bold">File, Folder, File Space Path:</td>
    </tr>
    <tr>
        <td><asp:TextBox ID="txtPath" runat="server" CssClass="default" Width="450" MaxLength="200" /></td>
    </tr>
    <tr>
        <td class="footer">Example: C:\WINNT</td>
    </tr>
    <tr>
        <td class="bold">Date First Archive Required:</td>
    </tr>
    <tr>
        <td><asp:TextBox ID="txtFirst" runat="server" CssClass="default" Width="100" MaxLength="10" /> <asp:ImageButton ID="imgArchiveDate" runat="server" CssClass="default" ImageUrl="/images/calendar.gif" ImageAlign="AbsMiddle" /></td>                                 
    </tr>
    <tr>
        <td class="footer">Example: <%=DateTime.Today.ToShortDateString() %></td>
    </tr>
    <tr>
        <td class="bold">Total Archive Period:</td>
    </tr>
    <tr>
        <td>
            <asp:TextBox ID="txtRetention" runat="server" CssClass="default" Width="100" MaxLength="10" />&nbsp;
            <asp:DropDownList ID="ddlRetention" runat="server" CssClass="default">
                <asp:ListItem Value="Days" />
                <asp:ListItem Value="Weeks" />
                <asp:ListItem Value="Months" />
                <asp:ListItem Value="Years" />
                <asp:ListItem Value="Permanent" />
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td class="bold">Time of Backup:</td>
    </tr>
    <tr>
        <td>
            <asp:DropDownList ID="ddlHour" runat="server" CssClass="default">
                <asp:ListItem Value="12:00" />
                <asp:ListItem Value="1:00" />
                <asp:ListItem Value="2:00" />
                <asp:ListItem Value="3:00" />
                <asp:ListItem Value="4:00" />
                <asp:ListItem Value="5:00" />
                <asp:ListItem Value="6:00" />
                <asp:ListItem Value="7:00" />
                <asp:ListItem Value="8:00" />
                <asp:ListItem Value="9:00" />
                <asp:ListItem Value="10:00" />
                <asp:ListItem Value="11:00" />
            </asp:DropDownList>&nbsp;
            <asp:DropDownList ID="ddlSwitch" runat="server" CssClass="default">
                <asp:ListItem Value="AM" />
                <asp:ListItem Value="PM" />
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td class="bold">Frequency of Backup:</td>
    </tr>
    <tr>
        <td>
            <asp:DropDownList ID="ddlFrequency" runat="server" CssClass="default">
                <asp:ListItem Value="Daily" />
                <asp:ListItem Value="Weekly" />
                <asp:ListItem Value="Monthly" />
            </asp:DropDownList>
            <div id="divFrequency" runat="server" style="display:none">
                &nbsp;occurring on&nbsp;
                <asp:TextBox ID="txtFrequency" runat="server" CssClass="default" Width="200" MaxLength="50" />
            </div>
        </td>
    </tr>
    <tr>
        <td>&nbsp;</td>
    </tr>
    <tr>
        <td><asp:Button ID="btnAdd" runat="server" CssClass="default" Width="125" Text="Add Requirement" OnClick="btnAdd_Click" /></td>
    </tr>
</table>
</asp:Content>
