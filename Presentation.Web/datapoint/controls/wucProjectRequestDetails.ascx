<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucProjectRequestDetails.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.wucProjectRequestDetails" %>
<table id="tblHeader" width="95%" cellpadding="4" cellspacing="2" border="0">
    <tr>
        <td class="header" width="100%" valign="bottom">
            Project Request Details</td>
    </tr>
</table>
<asp:Panel ID="pnlProjectRequest" runat="server" Visible="false">
    <table id="tblProjectReqDetails" width="95%" cellpadding="4" cellspacing="2" border="1px">
        <tr>
            <td align="left" valign="top" width="30%">
                Requested By</td>
            <td align="left" valign="top" width="70%">
                <asp:Label ID="lblRequestedBy" runat="server" CssClass="default" />
            </td>
        </tr>
        <tr>
            <td align="left" valign="top" width="30%">
                Requested On</td>
            <td align="left" valign="top" width="70%">
                <asp:Label ID="lblRequestedOnValue" runat="server" CssClass="default" />
            </td>
        </tr>
        <tr>
            <td align="left" valign="top" width="30%">
                Require a C1:</td>
            <td align="left" valign="top" width="70%">
                <asp:Label ID="lblC1" runat="server" CssClass="default" />
            </td>
        </tr>
        <tr>
            <td align="left" valign="top" width="30%">
                End Life:</td>
            <td align="left" valign="top" width="70%">
                <asp:Label ID="lblEndLife" runat="server" CssClass="default" />
                <br />
                <asp:Label ID="lblEndLifeDate" runat="server" Text="End Life Date :" Visible="false"
                    CssClass="default" />
                <asp:Label ID="lblEndLifeDateValue" runat="server" Text="" Visible="false" CssClass="default" />
            </td>
        </tr>
        <tr>
            <td align="left" valign="top" width="30%">
                Initiative Opportunity</td>
            <td align="left" valign="top" width="70%">
                <asp:Label ID="lblInitiativeOpportunity" runat="server" CssClass="default" />
            </td>
        </tr>
        <tr>
            <td align="left" valign="top" width="30%">
                Platform(s)</td>
            <td align="left" valign="top" width="70%">
                <asp:Label ID="lblPlatforms" runat="server" CssClass="default" />
            </td>
        </tr>
        <tr>
            <td align="left" valign="top" width="30%">
                Is this an Audit Requirement</td>
            <td align="left" valign="top" width="70%">
                <asp:Label ID="lblAuditReq" runat="server" CssClass="default" />
                <br />
                <asp:Label ID="lblAuditReqDate" runat="server" Text="Requirement Date : " Visible="false"
                    CssClass="default" />
                <asp:Label ID="lblAuditReqDateValue" runat="server" Text="" Visible="false" CssClass="default" />
            </td>
        </tr>
        <tr>
            <td align="left" valign="top" width="30%">
                Interdependency With Other Projects/Initiatives</td>
            <td align="left" valign="top" width="70%">
                <asp:Label ID="lblInterdependency" runat="server" CssClass="default" />
                <br />
                <asp:Label ID="lblProject" runat="server" Text="Project Name(s) : " Visible="false"
                    CssClass="default" />
                <asp:Label ID="lblProjectValues" runat="server" Text="" Visible="false" CssClass="default" />
            </td>
        </tr>
        <tr>
            <td align="left" valign="top" width="30%">
                Technology Capability</td>
            <td align="left" valign="top" width="70%">
                <asp:Label ID="lblTechnolgyCapability" runat="server" CssClass="default" />
            </td>
        </tr>
        <tr>
            <td align="left" valign="top" width="30%">
                Estimated Man Hours</td>
            <td align="left" valign="top" width="70%">
                <asp:Label ID="lblEstimatedManHours" runat="server" CssClass="default" />
            </td>
        </tr>
        <tr>
            <td align="left" valign="top" width="30%">
                Proposed Discovery Start Date</td>
            <td align="left" valign="top" width="70%">
                <asp:Label ID="lblDiscoveryStartDate" runat="server" CssClass="default" />
            </td>
        </tr>
        <tr>
            <td align="left" valign="top" width="30%">
                Expected Project Completion Date</td>
            <td align="left" valign="top" width="70%">
                <asp:Label ID="lblExpectedCompletionDate" runat="server" CssClass="default" />
            </td>
        </tr>
        <tr>
            <td align="left" valign="top" width="30%">
                Expected Capital Cost</td>
            <td align="left" valign="top" width="70%">
                <asp:Label ID="lblExpectedCapitalCost" runat="server" CssClass="default" />
            </td>
        </tr>
        <tr>
            <td align="left" valign="top" width="30%">
                Internal Labor</td>
            <td align="left" valign="top" width="70%">
                <asp:Label ID="lblInternalLabor" runat="server" CssClass="default" />
            </td>
        </tr>
        <tr>
            <td align="left" valign="top" width="30%">
                External Labor</td>
            <td align="left" valign="top" width="70%">
                <asp:Label ID="lblExternalLabor" runat="server" CssClass="default" />
            </td>
        </tr>
        <tr>
            <td align="left" valign="top" width="30%">
                Maintenance Cost Increase</td>
            <td align="left" valign="top" width="70%">
                <asp:Label ID="lblMaintenanceCostIncrease" runat="server" CssClass="default" />
            </td>
        </tr>
        <tr>
            <td align="left" valign="top" width="30%">
                Project Expenses</td>
            <td align="left" valign="top" width="70%">
                <asp:Label ID="lblProjectExpenses" runat="server" CssClass="default" />
            </td>
        </tr>
        <tr>
            <td align="left" valign="top" width="30%">
                Estimated Net Cost Avoidance</td>
            <td align="left" valign="top" width="70%">
                <asp:Label ID="lblEstimatedCostAvoidance" runat="server" CssClass="default" />
            </td>
        </tr>
        <tr>
            <td align="left" valign="top" width="30%">
                Estimated Net Cost Savings</td>
            <td align="left" valign="top" width="70%">
                <asp:Label ID="lblEstimatedCostSavings" runat="server" CssClass="default" />
            </td>
        </tr>
        <tr>
            <td align="left" valign="top" width="30%">
                Realized Cost Savings</td>
            <td align="left" valign="top" width="70%">
                <asp:Label ID="lblRealizedCostSavings" runat="server" CssClass="default" />
            </td>
        </tr>
        <tr>
            <td align="left" valign="top" width="30%">
                Business Impact Aviodance</td>
            <td align="left" valign="top" width="70%">
                <asp:Label ID="lblBusinessImpactAvoidance" runat="server" CssClass="default" />
            </td>
        </tr>
        <tr>
            <td align="left" valign="top" width="30%">
                Maintenance Cost Avoidance</td>
            <td align="left" valign="top" width="70%">
                <asp:Label ID="lblMaintenanceCostAvoidance" runat="server" CssClass="default" />
            </td>
        </tr>
        <tr>
            <td align="left" valign="top" width="30%">
                Asset Reusability</td>
            <td align="left" valign="top" width="70%">
                <asp:Label ID="lblAssetReusability" runat="server" CssClass="default" />
            </td>
        </tr>
        <tr>
            <td align="left" valign="top" width="30%">
                Internal Customer Impact</td>
            <td align="left" valign="top" width="70%">
                <asp:Label ID="lblInternalCustomerImpact" runat="server" CssClass="default" />
            </td>
        </tr>
        <tr>
            <td align="left" valign="top" width="30%">
                External Customer Impact</td>
            <td align="left" valign="top" width="70%">
                <asp:Label ID="lblExternalCustomerImpact" runat="server" CssClass="default" />
            </td>
        </tr>
        <tr>
            <td align="left" valign="top" width="30%">
                Business Impact</td>
            <td align="left" valign="top" width="70%">
                <asp:Label ID="lblBusinessImpact" runat="server" CssClass="default" />
            </td>
        </tr>
        <tr>
            <td align="left" valign="top" width="30%">
                Strategic Opportunity</td>
            <td align="left" valign="top" width="70%">
                <asp:Label ID="lblStrategicOpportunity" runat="server" CssClass="default" />
            </td>
        </tr>
        <tr>
            <td align="left" valign="top" width="30%">
                Acquisition / BIC</td>
            <td align="left" valign="top" width="70%">
                <asp:Label ID="lblAcquisitionBIC" runat="server" CssClass="default" />
            </td>
        </tr>
        <tr>
            <td align="left" valign="top" width="30%">
                Capabilities</td>
            <td align="left" valign="top" width="70%">
                <asp:Label ID="lblCapabilities" runat="server" CssClass="default" />
            </td>
        </tr>
        <tr>
            <td align="left" valign="top" width="30%">
                Technical Project Manager Requested</td>
            <td align="left" valign="top" width="70%">
                <asp:Label ID="lblTPMRequested" runat="server" CssClass="default" />
                <br />
                <asp:Label ID="lblServiceType" runat="server" Text="Service Type : " Visible="false"
                    CssClass="default" />
                <asp:Label ID="lblServiceTypeValue" runat="server" Text="" Visible="false" CssClass="default" />
                <br />
                <asp:Label ID="lblProjectLead" runat="server" Text="Project Lead : " Visible="false"
                    CssClass="default" />
                <asp:Label ID="lblProjectLeadValue" runat="server" Text="" Visible="false" CssClass="default" />
            </td>
        </tr>
    </table>
</asp:Panel>
<table id="tblNoProjectReqInfo" width="95%" cellpadding="4" cellspacing="2" border="0">
    <tr>
        <td colspan="5">
            <asp:Label ID="lblNoProjectReqInfo" runat="server" CssClass="default" Visible="false"
                Text="<img src='/images/spacer.gif' border='0' height='1' width='25' /><img src='/images/alert.gif' border='0' align='absmiddle'> No project request information found." /></td>
    </tr>
</table>
