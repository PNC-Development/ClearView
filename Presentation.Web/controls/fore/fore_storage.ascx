<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="fore_storage.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.fore_storage" %>
<script type="text/javascript">
</script>
<asp:Panel ID="panBlade" runat="server" Visible="false">
    <table width="100%" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
        <tr>
            <td nowrap><img src="/images/bigAlert.gif" border="0" align="absmiddle" /></td>
            <td width="100%">The provisioned model requires Boot from SAN (BFS). This technology requires exactly <%=intStoragePerBladeOs %> GB to be configured on the Operating System volume for each device. In addition, it requires at least <%=intStoragePerBladeApp %> GB storage allocated for each device on the Application / Data volume. Your current quantity is <asp:Label ID="lblQuantity" runat="server" CssClass="bold" /> meaning you are required to allocate exactly <asp:Label ID="lblMinimumOS" runat="server" CssClass="bold" /> GB storage on the Operating System volume and at least <asp:Label ID="lblMinimumApp" runat="server" CssClass="bold" /> GB storage on the Application / Data volume.</td>
        </tr>
    </table>
    <br />
</asp:Panel>
<asp:Panel ID="panContinue" runat="server" Visible="false">
    <asp:PlaceHolder ID="PHStorage" runat="server" />
</asp:Panel>
<asp:Panel ID="panStop" runat="server" Visible="false">
    <table width="100%" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
        <tr>
            <td nowrap><img src="/images/bigError.gif" border="0" align="absmiddle" /></td>
            <td width="100%">
                <asp:Label ID="lblValidModelSelectionMsg" runat="server" CssClass="default" Text="You cannot configure your storage until a valid model has been selected. Please contact your design implementor if you have questions." />
            </td>
        </tr>
        <tr>
            <td nowrap></td>
            <td width="100%"><asp:Button ID="btnBack" runat="server" OnClick="btnBack_Click" Text="Back" CssClass="default" Width="75" /></td>
        </tr>
    </table>
</asp:Panel>
<asp:Panel ID="panReset" runat="server" Visible="false">
    <table width="100%" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
        <tr>
            <td nowrap><img src="/images/bigAlert.gif" border="0" align="absmiddle" /></td>
            <td width="100%">
                <asp:Label ID="lblUnableMakeSelection" runat="server" CssClass="default" Text="ClearView is unable to make a selection for your design. However, it appears that you have already configured storage for this design and this configuration could interfere with ClearView's selection matrix." />
            </td>
        </tr>
        <tr>
            <td nowrap></td>
            <td width="100%">
                <asp:Label ID="lblRecommendationResetStorage" runat="server" CssClass="default" Text="It is recommended that you reset the storage for this design by clicking the &quot;Reset Storage&quot; button." />
            </td>
        </tr>
        <tr>
            <td nowrap></td>
            <td width="100%"><asp:Button ID="btnReset" runat="server" Text="Reset Storage" CssClass="default" Width="125" /></td>
        </tr>
    </table>
</asp:Panel>