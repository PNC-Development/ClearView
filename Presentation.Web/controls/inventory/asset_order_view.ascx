<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="asset_order_view.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.asset_order_view" %>

<script type="text/javascript">

</script>

<asp:Panel ID="pnlHeader" runat="server" width="100%"  Visible="true">
    <table width="100%" cellpadding="0" cellspacing="0" border="0" class="default">
        <tr>
            <td><img src="/images/table_topleft.gif" border="0" width="5" height="26"></td>
            <td nowrap background="/images/table_top.gif" class="greentableheader" width="100%">
                <asp:Label ID="Label1" runat="server" CssClass="greentableheader" />
            </td>
            <td nowrap background="/images/table_top.gif" class="greentableheader"></td>
            <td><img src="/images/table_topright.gif" border="0" width="5" height="26"></td>
        </tr>
     </table>
</asp:Panel>

<asp:Panel ID="pnlAllow" runat="server" width="100%" Visible="true">
    <table width="100%" cellpadding="0" cellspacing="0" border="0" class="default">
        <tr>
            <td width="100%" bgcolor="#FFFFFF">    
                <fieldset>
                    <legend class="tableheader"><b>Filtering Options</b></legend>
                    <table width="100%" cellpadding="4" cellspacing="0" border="0">
                        <tr valign="top">
                            <td width="25%">
                                <asp:Label ID="lblStatus" runat="server" CssClass="default" Text="Status:" />
                            </td>
                            <td width="75%" >
                                <asp:ListBox ID="lstStatus" runat="server" CssClass="smalldefault" Width="300" SelectionMode="Multiple" /> 
                             </td>
                        </tr>   
                        <tr valign="top">
                            <td width="25%">
                                <asp:Label ID="lblModel" runat="server" CssClass="default" Text="Model:" />
                            </td>
                            <td width="75%" >
                                <asp:DropDownList ID="ddlModel" runat="server" CssClass="smalldefault" Width="300"  /> 
                             </td>
                        </tr>   
                        <tr valign="top">
                            <td width="25%">
                               <asp:Label ID="lblSubmittedDate" runat="server" CssClass="default" Text="Submitted Date Range:" />
                            </td>
                            <td width="75%" >
                                <asp:TextBox ID="txtStartDate" runat="server" CssClass="smalldefault" Width="70" MaxLength="10" /> 
                                <asp:ImageButton ID="imgStartDate" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /> - 
                                <asp:TextBox ID="txtEndDate" runat="server" CssClass="smalldefault" Width="70" MaxLength="10" /> 
                                <asp:ImageButton ID="imgEndDate" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" />
                             </td>
                        </tr>   
                        <tr valign="top">
                            <td width="25%">&nbsp;</td>
                            <td nowrap colspan="2" style ="text-align:left">
                                <asp:Button ID="btnApplyFilter" runat="server" CssClass="default" Width="100" Text="Apply Filter" OnClick="btnApplyFilter_Click" />
                            </td>
                        </tr>
                        
                    </table>
                </fieldset>
              </td>
        </tr>
    </table>
    
    <asp:Panel ID="pnlResults" runat="server" width="100%" Visible="true">
     <table width="98%" cellpadding="4" cellspacing="2" border="0">
        <tr>
            <td class="default">
                <asp:Label ID="lblRecords" runat="server" CssClass="bigger" /></td>
            <td id ="tdNavigation" class="default" align="right" runat="server" visible="false">
                <asp:LinkButton ID="btnPrevious" runat="server" Text="Previous Page" OnClick="btnPrevious_Click" />
                &nbsp;&nbsp;|&nbsp;&nbsp;
                <asp:LinkButton ID="btnNext" runat="server" Text="Next Page" OnClick="btnNext_Click" />&nbsp;&nbsp;</td>
        </tr>
    </table>
    <br />
    <asp:DataList ID="dlAssetOrders" runat="server" CellPadding="2" CellSpacing="1" Width="98%" OnItemDataBound="dlAssetOrders_ItemDataBound">
            <HeaderTemplate>
                <tr bgcolor="#EEEEEE">
                    <td align="left" width="0%">
                        <asp:LinkButton ID="lnkbtnHeaderOrderId" runat="server" CssClass="tableheader" Text="<b>Order#</b>" />
                    </td>
                    <td align="left" width="0%">
                        <asp:LinkButton ID="lnkbtnHeaderRequestId" runat="server" CssClass="tableheader" Text="<b>Request#</b>" />
                    </td>
                    <td align="left" width="10%">
                        <asp:LinkButton ID="lnkbtnHeaderNickName" runat="server" CssClass="tableheader" Text="<b>NickName</b>" />
                    </td>
                    <td align="left" width="10%">
                        <asp:LinkButton ID="lnkbtnHeaderModel" runat="server" CssClass="tableheader" Text="<b>Model</b>"
                            OnClick="btnOrder_Click" CommandArgument="ModelName" />
                    </td>
                    <td align="left" width="10%">
                        <asp:LinkButton ID="lnkbtnHeaderOrderType" runat="server" CssClass="tableheader" Text="<b>Procurement Type</b>"/>
                    </td>
                    <td align="left" width="0%">
                        <asp:LinkButton ID="lnkbtnHeaderQuantityRequested" runat="server" CssClass="tableheader" Text="<b>Requested Quantity</b>" />
                    </td>
                    <td align="left" width="20%" >
                        <asp:LinkButton ID="lnkbtnHeaderLocation" runat="server" CssClass="tableheader" Text="<b>Location</b>" 
                        OnClick="btnOrder_Click" CommandArgument="Location" />
                    </td>
                    <td align="left" width="10%" >
                        <asp:LinkButton ID="lnkbtnHeaderClass" runat="server" CssClass="tableheader" Text="<b>Class</b>" 
                            OnClick="btnOrder_Click" CommandArgument="Class"/>
                    </td>
                    <td align="left" width="10%">
                        <asp:LinkButton ID="lnkbtnHeaderEnvironment" runat="server" CssClass="tableheader" Text="<b>Environment</b>" 
                            OnClick="btnOrder_Click" CommandArgument="Environment"/>
                    </td>
                     <td align="left" width="10%" >
                        <asp:LinkButton ID="lnkbtnHeaderSubmittedDate" runat="server" CssClass="tableheader" Text="<b>Submitted Date</b>" />
                    </td>
                    <td align="left" width="10%" >
                        <asp:LinkButton ID="lnkbtnHeaderStatus" runat="server" CssClass="tableheader" Text="<b>Status</b>" />
                    </td>
                    <td align="left" width="10%">
                        <asp:LinkButton ID="lnkbtnHeaderLastUpdates" runat="server" CssClass="tableheader" Text="<b>Recent Updates</b>" />
                    </td>
                </tr>
            </HeaderTemplate>
            <ItemTemplate>
                <tr  valign="top" align="left" >
                    <td align="left">
                        <asp:LinkButton ID="lnkOrderId" runat="server" CssClass="lookup" OnClick="btnViewOrder_Click" />
                    </td>
                    <td align="left">
                        <asp:Label ID="lblRequestId" runat="server" CssClass="default" />
                    </td>
                    <td align="left">
                        <asp:Label ID="lblNickName" runat="server" CssClass="default" />
                    </td>
                    <td align="left" >
                        <asp:Label ID="lblModel" runat="server" CssClass="default" />
                    </td>
                    <td align="left" >
                        <asp:Label ID="lblOrderType" runat="server" CssClass="default" />
                    </td>
                    <td align="left" >
                        <asp:Label ID="lblQuantityRequested" runat="server" CssClass="default" />
                    </td>
                    <td align="left" >
                        <asp:Label ID="lblLocation" runat="server" CssClass="default" />
                    </td>
                    <td align="left" >
                        <asp:Label ID="lblClass" runat="server" CssClass="default" />
                    </td>
                    <td align="left" >
                        <asp:Label ID="lblEnvironment" runat="server" CssClass="default" />
                    </td>
                    <td align="left" >
                        <asp:Label ID="lblSubmittedDate" runat="server" CssClass="default" />
                    </td>
                    <td align="left" >
                        <asp:Label ID="lblStatus" runat="server" CssClass="default" />
                    </td>
                    <td align="left" >
                        <asp:Label ID="lblLastUpdates" runat="server" CssClass="default" />
                    </td>
                </tr>
            </ItemTemplate>
            <AlternatingItemTemplate>
                <tr bgcolor="#F6F6F6">
                    <td align="left">
                        <asp:LinkButton ID="lnkOrderId" runat="server" CssClass="lookup" OnClick="btnViewOrder_Click" />
                    </td>
                    <td align="left">
                        <asp:Label ID="lblRequestId" runat="server" CssClass="default" />
                    </td>
                    <td align="left">
                        <asp:Label ID="lblNickName" runat="server" CssClass="default" />
                    </td>
                    <td align="left" >
                        <asp:Label ID="lblModel" runat="server" CssClass="default" />
                    </td>
                    <td align="left" >
                        <asp:Label ID="lblOrderType" runat="server" CssClass="default" />
                    </td>
                    <td align="left" >
                        <asp:Label ID="lblQuantityRequested" runat="server" CssClass="default" />
                    </td>
                    <td align="left" >
                        <asp:Label ID="lblLocation" runat="server" CssClass="default" />
                    </td>
                    <td align="left" >
                        <asp:Label ID="lblClass" runat="server" CssClass="default" />
                    </td>
                    <td align="left" >
                        <asp:Label ID="lblEnvironment" runat="server" CssClass="default" />
                    </td>
                     <td align="left" >
                        <asp:Label ID="lblSubmittedDate" runat="server" CssClass="default" />
                    </td>
                    <td align="left" >
                        <asp:Label ID="lblStatus" runat="server" CssClass="default" />
                    </td>
                    <td align="left" >
                        <asp:Label ID="lblLastUpdates" runat="server" CssClass="default" />
                    </td>
                </tr>
            </AlternatingItemTemplate>
    </asp:DataList>
    <br />
    <hr />     
    </asp:Panel>

</asp:Panel>
<asp:Panel ID="pnlDenied" runat="server" width="100%" Visible="false">
    <br />
    <table width="100%" cellpadding="0" cellspacing="5" border="0">
        <tr width="100%">
            <td rowspan="2"><img id="imgError" src="/images/ico_error.gif" border="0" align="absmiddle"   /></td>
            <td class="header" width="100%" valign="bottom">Access Denied</td>
        </tr>
        <tr width="100%"><td width="100%" valign="top">You do not have sufficient permission to view this page.</td></tr>
        <tr><td colspan="2">&nbsp;</td></tr>
        <tr width="100%">
            <td></td>
            <td width="100%">If you think you should have rights to view it, please contact your ClearView administrator.</td>
        </tr>
    </table>
    <p>&nbsp;</p>
</asp:Panel>
    

<asp:HiddenField ID="hdnOrder" Value="0" runat="server" />
<asp:HiddenField ID="hdnOrderBy" Value="" runat="server" />
<asp:HiddenField ID="hdnPageNo" Value="1" runat="server" />
<asp:HiddenField ID="hdnRecsPerPage" Value="10" runat="server" />

