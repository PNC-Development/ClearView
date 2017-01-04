<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucProjectFinancials.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.wucProjectFinancials" %>

<table id="tblHeader" width="95%" cellpadding="4" cellspacing="2" border="0">
    <tr>
        <td class="header" width="100%" valign="bottom">
            Project Financials</td>
    </tr>
</table>


<asp:Panel ID="pnlProjectFinancials" runat="server" Visible="false">
    <table width="95%" cellpadding="5" cellspacing="0" border="1px">
        <tr>
            <td align="left" class="header">
                <b>Discovery:</b></td>
            <td align="right" class="greenheader">
                <b>Approved C1 & PCRs</b></td>
            <td align="right" class="greenheader">
                <b>Actuals to Date</b></td>
            <td align="right" class="greenheader">
                <b>Estimate to Complete</b></td>
            <td align="right" class="greenheader">
                <b>Current Forecast</b></td>
            <td align="right" class="greenheader">
                <b>Variance $</b></td>
            <td align="right" class="greenheader">
                <b>Variance %</b></td>
        </tr>
        <tr>
            <td align="left">
                Internal Labor:</td>
            <td align="right">
                <asp:Label ID="lblDisILApprovedC1PCRs" runat="server" CssClass="default" /></td>
            <td align="right">
                <asp:Label ID="lblDisILActualsToDate" runat="server" CssClass="default" />
            </td>
            <td align="right">
                <asp:Label ID="lblDisILEstimateToComplete" runat="server" CssClass="default" />
            </td>
            <td align="right">
                <asp:Label ID="lblDisILCurrentForecast" runat="server" CssClass="default" />
            </td>
            <td align="right">
                <asp:Label ID="lblDisILVariance" runat="server" CssClass="default" />
            </td>
            <td align="right">
                <asp:Label ID="lblDisILVariancePercent" runat="server" CssClass="default" />
            </td>
        </tr>
        <tr>
            <td align="left">
                External Labor:</td>
            <td align="right">
                <asp:Label ID="lblDisELApprovedC1PCRs" runat="server" CssClass="default" /></td>
            <td align="right">
                <asp:Label ID="lblDisELActualsToDate" runat="server" CssClass="default" />
            </td>
            <td align="right">
                <asp:Label ID="lblDisELEstimateToComplete" runat="server" CssClass="default" />
            </td>
            <td align="right">
                <asp:Label ID="lblDisELCurrentForecast" runat="server" CssClass="default" />
            </td>
            <td align="right">
                <asp:Label ID="lblDisELVariance" runat="server" CssClass="default" />
            </td>
            <td align="right">
                <asp:Label ID="lblDisELVariancePercent" runat="server" CssClass="default" />
            </td>
        </tr>
        <tr>
            <td align="left">
                HW/SW/One Time Cost</td>
            <td align="right">
                <asp:Label ID="lblDisHSApprovedC1PCRs" runat="server" CssClass="default" /></td>
            <td align="right">
                <asp:Label ID="lblDisHSActualsToDate" runat="server" CssClass="default" />
            </td>
            <td align="right">
                <asp:Label ID="lblDisHSEstimateToComplete" runat="server" CssClass="default" />
            </td>
            <td align="right">
                <asp:Label ID="lblDisHSCurrentForecast" runat="server" CssClass="default" />
            </td>
            <td align="right">
                <asp:Label ID="lblDisHSVariance" runat="server" CssClass="default" />
            </td>
            <td align="right">
                <asp:Label ID="lblDisHSVariancePercent" runat="server" CssClass="default" />
            </td>
        </tr>
        <tr style="background: #E9E9E9;">
            <td align="left">
                <b>PHASE TOTALS</b></td>
            <td align="right">
                <asp:Label ID="lblDisApprovedC1PCRs" runat="server" CssClass="default" /></td>
            <td align="right">
                <asp:Label ID="lblDisActualsToDate" runat="server" CssClass="default" />
            </td>
            <td align="right">
                <asp:Label ID="lblDisEstimateToComplete" runat="server" CssClass="default" />
            </td>
            <td align="right">
                <asp:Label ID="lblDisCurrentForecast" runat="server" CssClass="default" />
            </td>
            <td align="right">
                <asp:Label ID="lblDisVariance" runat="server" CssClass="default" />
            </td>
            <td align="right">
                <asp:Label ID="lblDisVariancePercent" runat="server" CssClass="default" />
            </td>
        </tr>
        <tr>
            <td align="left" style="border-style: none;">
                &nbsp;</td>
            <td align="right" style="border-style: none;">
            </td>
            <td align="right" style="border-style: none;">
            </td>
            <td align="right" style="border-style: none;">
            </td>
            <td align="right" style="border-style: none;">
            </td>
            <td align="right" style="border-style: none;">
            </td>
            <td align="right" style="border-style: none;">
            </td>
        </tr>
        <tr>
            <td align="left" class="header">
                <b>Planning:</b></td>
            <td align="right" class="greenheader">
                <b>Approved C1 & PCRs</b></td>
            <td align="right" class="greenheader">
                <b>Actuals to Date</b></td>
            <td align="right" class="greenheader">
                <b>Estimate to Complete</b></td>
            <td align="right" class="greenheader">
                <b>Current Forecast</b></td>
            <td align="right" class="greenheader">
                <b>Variance $</b></td>
            <td align="right" class="greenheader">
                <b>Variance %</b></td>
        </tr>
        <tr>
            <td align="left">
                Internal Labor:</td>
            <td align="right">
                <asp:Label ID="lblPlanILApprovedC1PCRs" runat="server" CssClass="default" /></td>
            <td align="right">
                <asp:Label ID="lblPlanILActualsToDate" runat="server" CssClass="default" />
            </td>
            <td align="right">
                <asp:Label ID="lblPlanILEstimateToComplete" runat="server" CssClass="default" />
            </td>
            <td align="right">
                <asp:Label ID="lblPlanILCurrentForecast" runat="server" CssClass="default" />
            </td>
            <td align="right">
                <asp:Label ID="lblPlanILVariance" runat="server" CssClass="default" />
            </td>
            <td align="right">
                <asp:Label ID="lblPlanILVariancePercent" runat="server" CssClass="default" />
            </td>
        </tr>
        <tr>
            <td align="left">
                External Labor:</td>
            <td align="right">
                <asp:Label ID="lblPlanELApprovedC1PCRs" runat="server" CssClass="default" /></td>
            <td align="right">
                <asp:Label ID="lblPlanELActualsToDate" runat="server" CssClass="default" />
            </td>
            <td align="right">
                <asp:Label ID="lblPlanELEstimateToComplete" runat="server" CssClass="default" />
            </td>
            <td align="right">
                <asp:Label ID="lblPlanELCurrentForecast" runat="server" CssClass="default" />
            </td>
            <td align="right">
                <asp:Label ID="lblPlanELVariance" runat="server" CssClass="default" />
            </td>
            <td align="right">
                <asp:Label ID="lblPlanELVariancePercent" runat="server" CssClass="default" />
            </td>
        </tr>
        <tr>
            <td align="left">
                HW/SW/One Time Cost</td>
            <td align="right">
                <asp:Label ID="lblPlanHSApprovedC1PCRs" runat="server" CssClass="default" /></td>
            <td align="right">
                <asp:Label ID="lblPlanHSActualsToDate" runat="server" CssClass="default" />
            </td>
            <td align="right">
                <asp:Label ID="lblPlanHSEstimateToComplete" runat="server" CssClass="default" />
            </td>
            <td align="right">
                <asp:Label ID="lblPlanHSCurrentForecast" runat="server" CssClass="default" />
            </td>
            <td align="right">
                <asp:Label ID="lblPlanHSVariance" runat="server" CssClass="default" />
            </td>
            <td align="right">
                <asp:Label ID="lblPlanHSVariancePercent" runat="server" CssClass="default" />
            </td>
        </tr>
        <tr style="background: #E9E9E9;">
            <td align="left">
                <b>PHASE TOTALS</b></td>
            <td align="right">
                <asp:Label ID="lblPlanApprovedC1PCRs" runat="server" CssClass="default" /></td>
            <td align="right">
                <asp:Label ID="lblPlanActualsToDate" runat="server" CssClass="default" />
            </td>
            <td align="right">
                <asp:Label ID="lblPlanEstimateToComplete" runat="server" CssClass="default" />
            </td>
            <td align="right">
                <asp:Label ID="lblPlanCurrentForecast" runat="server" CssClass="default" />
            </td>
            <td align="right">
                <asp:Label ID="lblPlanVariance" runat="server" CssClass="default" />
            </td>
            <td align="right">
                <asp:Label ID="lblPlanVariancePercent" runat="server" CssClass="default" />
            </td>
        </tr>
        <tr>
            <td align="left" style="border-style: none;">
                &nbsp;</td>
            <td align="right" style="border-style: none;">
            </td>
            <td align="right" style="border-style: none;">
            </td>
            <td align="right" style="border-style: none;">
            </td>
            <td align="right" style="border-style: none;">
            </td>
            <td align="right" style="border-style: none;">
            </td>
            <td align="right" style="border-style: none;">
            </td>
        </tr>
        <tr>
            <td align="left" class="header">
                <b>Execution:</b></td>
            <td align="right" class="greenheader">
                <b>Approved C1 & PCRs</b></td>
            <td align="right" class="greenheader">
                <b>Actuals to Date</b></td>
            <td align="right" class="greenheader">
                <b>Estimate to Complete</b></td>
            <td align="right" class="greenheader">
                <b>Current Forecast</b></td>
            <td align="right" class="greenheader">
                <b>Variance $</b></td>
            <td align="right" class="greenheader">
                <b>Variance %</b></td>
        </tr>
        <tr>
            <td align="left">
                Internal Labor:</td>
            <td align="right">
                <asp:Label ID="lblExecILApprovedC1PCRs" runat="server" CssClass="default" /></td>
            <td align="right">
                <asp:Label ID="lblExecILActualsToDate" runat="server" CssClass="default" />
            </td>
            <td align="right">
                <asp:Label ID="lblExecILEstimateToComplete" runat="server" CssClass="default" />
            </td>
            <td align="right">
                <asp:Label ID="lblExecILCurrentForecast" runat="server" CssClass="default" />
            </td>
            <td align="right">
                <asp:Label ID="lblExecILVariance" runat="server" CssClass="default" />
            </td>
            <td align="right">
                <asp:Label ID="lblExecILVariancePercent" runat="server" CssClass="default" />
            </td>
        </tr>
        <tr>
            <td align="left">
                External Labor:</td>
            <td align="right">
                <asp:Label ID="lblExecELApprovedC1PCRs" runat="server" CssClass="default" /></td>
            <td align="right">
                <asp:Label ID="lblExecELActualsToDate" runat="server" CssClass="default" />
            </td>
            <td align="right">
                <asp:Label ID="lblExecELEstimateToComplete" runat="server" CssClass="default" />
            </td>
            <td align="right">
                <asp:Label ID="lblExecELCurrentForecast" runat="server" CssClass="default" />
            </td>
            <td align="right">
                <asp:Label ID="lblExecELVariance" runat="server" CssClass="default" />
            </td>
            <td align="right">
                <asp:Label ID="lblExecELVariancePercent" runat="server" CssClass="default" />
            </td>
        </tr>
        <tr>
            <td align="left">
                HW/SW/One Time Cost</td>
            <td align="right">
                <asp:Label ID="lblExecHSApprovedC1PCRs" runat="server" CssClass="default" /></td>
            <td align="right">
                <asp:Label ID="lblExecHSActualsToDate" runat="server" CssClass="default" />
            </td>
            <td align="right">
                <asp:Label ID="lblExecHSEstimateToComplete" runat="server" CssClass="default" />
            </td>
            <td align="right">
                <asp:Label ID="lblExecHSCurrentForecast" runat="server" CssClass="default" />
            </td>
            <td align="right">
                <asp:Label ID="lblExecHSVariance" runat="server" CssClass="default" />
            </td>
            <td align="right">
                <asp:Label ID="lblExecHSVariancePercent" runat="server" CssClass="default" />
            </td>
        </tr>
        <tr style="background: #E9E9E9;">
            <td align="left">
                <b>PHASE TOTALS</b></td>
            <td align="right">
                <asp:Label ID="lblExecApprovedC1PCRs" runat="server" CssClass="default" /></td>
            <td align="right">
                <asp:Label ID="lblExecActualsToDate" runat="server" CssClass="default" />
            </td>
            <td align="right">
                <asp:Label ID="lblExecEstimateToComplete" runat="server" CssClass="default" />
            </td>
            <td align="right">
                <asp:Label ID="lblExecCurrentForecast" runat="server" CssClass="default" />
            </td>
            <td align="right">
                <asp:Label ID="lblExecVariance" runat="server" CssClass="default" />
            </td>
            <td align="right">
                <asp:Label ID="lblExecVariancePercent" runat="server" CssClass="default" />
            </td>
        </tr>
        <tr>
            <td align="left" style="border-style: none;">
                &nbsp;</td>
            <td align="right" style="border-style: none;">
            </td>
            <td align="right" style="border-style: none;">
            </td>
            <td align="right" style="border-style: none;">
            </td>
            <td align="right" style="border-style: none;">
            </td>
            <td align="right" style="border-style: none;">
            </td>
            <td align="right" style="border-style: none;">
            </td>
        </tr>
        <tr>
            <td align="left" class="header">
                <b>Closing:</b></td>
            <td align="right" class="greenheader">
                <b>Approved C1 & PCRs</b></td>
            <td align="right" class="greenheader">
                <b>Actuals to Date</b></td>
            <td align="right" class="greenheader">
                <b>Estimate to Complete</b></td>
            <td align="right" class="greenheader">
                <b>Current Forecast</b></td>
            <td align="right" class="greenheader">
                <b>Variance $</b></td>
            <td align="right" class="greenheader">
                <b>Variance %</b></td>
        </tr>
        <tr>
            <td align="left">
                Internal Labor:</td>
            <td align="right">
                <asp:Label ID="lblCloseILApprovedC1PCRs" runat="server" CssClass="default" /></td>
            <td align="right">
                <asp:Label ID="lblCloseILActualsToDate" runat="server" CssClass="default" />
            </td>
            <td align="right">
                <asp:Label ID="lblCloseILEstimateToComplete" runat="server" CssClass="default" />
            </td>
            <td align="right">
                <asp:Label ID="lblCloseILCurrentForecast" runat="server" CssClass="default" />
            </td>
            <td align="right">
                <asp:Label ID="lblCloseILVariance" runat="server" CssClass="default" />
            </td>
            <td align="right">
                <asp:Label ID="lblCloseILVariancePercent" runat="server" CssClass="default" />
            </td>
        </tr>
        <tr>
            <td align="left">
                External Labor:</td>
            <td align="right">
                <asp:Label ID="lblCloseELApprovedC1PCRs" runat="server" CssClass="default" /></td>
            <td align="right">
                <asp:Label ID="lblCloseELActualsToDate" runat="server" CssClass="default" />
            </td>
            <td align="right">
                <asp:Label ID="lblCloseELEstimateToComplete" runat="server" CssClass="default" />
            </td>
            <td align="right">
                <asp:Label ID="lblCloseELCurrentForecast" runat="server" CssClass="default" />
            </td>
            <td align="right">
                <asp:Label ID="lblCloseELVariance" runat="server" CssClass="default" />
            </td>
            <td align="right">
                <asp:Label ID="lblCloseELVariancePercent" runat="server" CssClass="default" />
            </td>
        </tr>
        <tr>
            <td align="left">
                HW/SW/One Time Cost</td>
            <td align="right">
                <asp:Label ID="lblCloseHSApprovedC1PCRs" runat="server" CssClass="default" /></td>
            <td align="right">
                <asp:Label ID="lblCloseHSActualsToDate" runat="server" CssClass="default" />
            </td>
            <td align="right">
                <asp:Label ID="lblCloseHSEstimateToComplete" runat="server" CssClass="default" />
            </td>
            <td align="right">
                <asp:Label ID="lblCloseHSCurrentForecast" runat="server" CssClass="default" />
            </td>
            <td align="right">
                <asp:Label ID="lblCloseHSVariance" runat="server" CssClass="default" />
            </td>
            <td align="right">
                <asp:Label ID="lblCloseHSVariancePercent" runat="server" CssClass="default" />
            </td>
        </tr>
        <tr style="background: #E9E9E9;">
            <td align="left">
                <b>PHASE TOTALS</b></td>
            <td align="right">
                <asp:Label ID="lblCloseApprovedC1PCRs" runat="server" CssClass="default" /></td>
            <td align="right">
                <asp:Label ID="lblCloseActualsToDate" runat="server" CssClass="default" />
            </td>
            <td align="right">
                <asp:Label ID="lblCloseEstimateToComplete" runat="server" CssClass="default" />
            </td>
            <td align="right">
                <asp:Label ID="lblCloseCurrentForecast" runat="server" CssClass="default" />
            </td>
            <td align="right">
                <asp:Label ID="lblCloseVariance" runat="server" CssClass="default" />
            </td>
            <td align="right">
                <asp:Label ID="lblCloseVariancePercent" runat="server" CssClass="default" />
            </td>
        </tr>
        <tr>
            <td align="left" style="border-style: none;">
                &nbsp;</td>
            <td align="right" style="border-style: none;">
            </td>
            <td align="right" style="border-style: none;">
            </td>
            <td align="right" style="border-style: none;">
            </td>
            <td align="right" style="border-style: none;">
            </td>
            <td align="right" style="border-style: none;">
            </td>
            <td align="right" style="border-style: none;">
            </td>
        </tr>
        <tr>
            <td align="left" class="header">
                <b>Project Totals</b></td>
            <td align="right" class="greenheader">
                <b>Approved C1 & PCRs</b></td>
            <td align="right" class="greenheader">
                <b>Actuals to Date</b></td>
            <td align="right" class="greenheader">
                <b>Estimate to Complete</b></td>
            <td align="right" class="greenheader">
                <b>Current Forecast</b></td>
            <td align="right" class="greenheader">
                <b>Variance $</b></td>
            <td align="right" class="greenheader">
                <b>Variance %</b></td>
        </tr>
        <tr>
            <td align="left">
                Internal Labor:</td>
            <td align="right">
                <asp:Label ID="lblILApprovedC1PCRs" runat="server" CssClass="default" /></td>
            <td align="right">
                <asp:Label ID="lblILActualsToDate" runat="server" CssClass="default" />
            </td>
            <td align="right">
                <asp:Label ID="lblILEstimateToComplete" runat="server" CssClass="default" />
            </td>
            <td align="right">
                <asp:Label ID="lblILCurrentForecast" runat="server" CssClass="default" />
            </td>
            <td align="right">
                <asp:Label ID="lblILVariance" runat="server" CssClass="default" />
            </td>
            <td align="right">
                <asp:Label ID="lblILVariancePercent" runat="server" CssClass="default" />
            </td>
        </tr>
        <tr>
            <td align="left">
                External Labor:</td>
            <td align="right">
                <asp:Label ID="lblELApprovedC1PCRs" runat="server" CssClass="default" /></td>
            <td align="right">
                <asp:Label ID="lblELActualsToDate" runat="server" CssClass="default" />
            </td>
            <td align="right">
                <asp:Label ID="lblELEstimateToComplete" runat="server" CssClass="default" />
            </td>
            <td align="right">
                <asp:Label ID="lblELCurrentForecast" runat="server" CssClass="default" />
            </td>
            <td align="right">
                <asp:Label ID="lblELVariance" runat="server" CssClass="default" />
            </td>
            <td align="right">
                <asp:Label ID="lblELVariancePercent" runat="server" CssClass="default" />
            </td>
        </tr>
        <tr>
            <td align="left">
                HW/SW/One Time Cost</td>
            <td align="right">
                <asp:Label ID="lblHSApprovedC1PCRs" runat="server" CssClass="default" /></td>
            <td align="right">
                <asp:Label ID="lblHSActualsToDate" runat="server" CssClass="default" />
            </td>
            <td align="right">
                <asp:Label ID="lblHSEstimateToComplete" runat="server" CssClass="default" />
            </td>
            <td align="right">
                <asp:Label ID="lblHSCurrentForecast" runat="server" CssClass="default" />
            </td>
            <td align="right">
                <asp:Label ID="lblHSVariance" runat="server" CssClass="default" />
            </td>
            <td align="right">
                <asp:Label ID="lblHSVariancePercent" runat="server" CssClass="default" />
            </td>
        </tr>
        <tr style="background: #E9E9E9;">
            <td align="left">
                <b>TOTALS</b></td>
            <td align="right">
                <asp:Label ID="lblApprovedC1PCRs" runat="server" CssClass="default" /></td>
            <td align="right">
                <asp:Label ID="lblActualsToDate" runat="server" CssClass="default" />
            </td>
            <td align="right">
                <asp:Label ID="lblEstimateToComplete" runat="server" CssClass="default" />
            </td>
            <td align="right">
                <asp:Label ID="lblCurrentForecast" runat="server" CssClass="default" />
            </td>
            <td align="right">
                <asp:Label ID="lblVariance" runat="server" CssClass="default" />
            </td>
            <td align="right">
                <asp:Label ID="lblVariancePercent" runat="server" CssClass="default" />
            </td>
        </tr>
    </table>
</asp:Panel>


<table id="tblNoProjectFinancials" width="95%" cellpadding="4" cellspacing="2" border="0">
    <tr>
        <td colspan="5">
            <asp:Label ID="lblNoProjectFinancials" runat="server" CssClass="default" Visible="false"
                Text="<img src='/images/spacer.gif' border='0' height='1' width='25' /><img src='/images/alert.gif' border='0' align='absmiddle'> No project financial information found." /></td>
    </tr>
</table>