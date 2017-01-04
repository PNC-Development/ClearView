<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="supply_component.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.supply_component" %>

<script type="text/javascript">
</script>
<br />
<table width="100%" height="100%" cellpadding="5" cellspacing="5" border="0">
    <tr height="1">
        <td nowrap>Component:</td>
        <td width="100%">
            <table cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td><asp:DropDownList ID="ddlTypes" runat="server" CssClass="default" Width="300" AutoPostBack="true" OnSelectedIndexChanged="ddlTypes_Change" /></td>
                    <td class="bold">
                        <div id="divTypes" runat="server" style="display:none">
                            <img src="/images/spacer.gif" border="0" width="20" height="1" /><img src="/images/please_wait.gif" border="0" align="absmiddle" /> Please Wait...
                        </div>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td colspan="2"><asp:PlaceHolder ID="phComponent" runat="server" /></td>
    </tr>
</table>
