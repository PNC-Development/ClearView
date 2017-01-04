<%@ Page Language="C#" MasterPageFile="~/frame.Master" AutoEventWireup="true" CodeBehind="forecast_filter.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.forecast_filter" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" runat="server">
<script type="text/javascript">
    function Update(oc, oe) {
        oc = document.getElementById(oc);
        oe = document.getElementById(oe);
        if (oc.selectedIndex == 0) {
            alert('Please select a class');
            oc.focus();
        }
        else if (oe.selectedIndex == 0) {
            alert('Please select an environment');
            oe.focus();
        }
        else {
            window.parent.ForecastFilter(oc.options[oc.selectedIndex].value, oe.options[oe.selectedIndex].value, window.parent.location.toString());
            window.parent.HidePanel();
        }
        return false;
    }
</script>
<table width="100%" cellpadding="4" cellspacing="3" border="0">
    <tr>
        <td nowrap>Class:</td>
        <td width="100%">
            <table cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td><asp:DropDownList ID="ddlClass" runat="server" CssClass="default" Width="200" AutoPostBack="true" OnSelectedIndexChanged="ddlClass_Change" /></td>
                    <td class="bold">
                        <div id="divWait" runat="server" style="display:none">
                            <img src="/images/spacer.gif" border="0" width="20" height="1" /><img src="/images/please_wait.gif" border="0" align="absmiddle" /> Please Wait...
                        </div>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td nowrap>Environment:</td>
        <td width="100%"><asp:DropDownList ID="ddlEnvironment" runat="server" CssClass="default" Width="200" /></td>
    </tr>
    <tr>
        <td colspan="2"><hr size="1" noshade /></td>
    </tr>
    <tr>
        <td colspan="2" align="right"><asp:Button ID="btnSave" runat="server" Text="OK" Width="75" CssClass="default" /> <asp:Button ID="btnClose" runat="server" Text="Cancel" Width="75" CssClass="default" /></td>
    </tr>
</table>
</asp:Content>
