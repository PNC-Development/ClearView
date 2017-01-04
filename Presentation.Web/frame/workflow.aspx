<%@ Page Language="C#" MasterPageFile="~/frame.Master" AutoEventWireup="true" CodeBehind="workflow.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.workflow" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" runat="server">
<script type="text/javascript">
    var oOldRadio = null;
    function UpdateWorkflow(oRadio, oHidden, oValue) {
        if (oOldRadio != null)
            oOldRadio.checked = false;
        oRadio.checked = true;
        oOldRadio = oRadio;
        oHidden = document.getElementById(oHidden);
        oHidden.value = oValue;
        return true;
    }
</script>
    <table width="100%" height="100%" cellpadding="2" cellspacing="0" border="0">
        <tr>
            <td height="100%" valign="top" colspan="2">
                <div style="height:100%; overflow-y:auto">
                    <asp:TreeView ID="treLocation" runat="server" CssClass="default" ShowLines="true" NodeIndent="30">
                    </asp:TreeView>
                </div>
            </td>
        </tr>
        <tr height="1">
            <td><asp:Button ID="btnSave" runat="server" CssClass="default" Width="75" Text="Save" OnClick="btnSave_Click" />
            <td align="right" nowrap><img src="/images/help24.gif" border="0" align="absmiddle" /> <a href="javascript:void(0);" onclick="alert('Only services that have been permitted to be connected for workflow are available to be selected.\n\nIf you are not seeing a service you are expecting to see, contact the service owner to have the workflow connection enabled.');">Why don't I see the service I'm looking for?</a></td>
        </tr>
    </table>
<asp:HiddenField ID="hdnService" runat="server" />
</asp:Content>
