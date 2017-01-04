<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="fore_design.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.fore_design" %>
<script type="text/javascript">
</script>
<table width="100%" cellpadding="5" cellspacing="0" border="0" bgcolor="#f9f9f9">
    <tr>
        <td nowrap class="bold"><asp:Label ID="lblValProjectName" runat="server" Text="Project Name:" /></td>
        <td width="50%"><asp:Label ID="lblSummaryProjectName" runat="server" /></td>
        <td nowrap class="bold"><asp:Label ID="lblValProjectNumber" runat="server" Text="Project Number:" /></td>
        <td width="50%"><asp:Label ID="lblSummaryProjectNumber" runat="server" /></td>
    </tr>
    <tr id="trSubmitted" runat="server" visible="false">
        <td nowrap class="bold"><asp:Label ID="lblValSubmittedBy" runat="server" Text="Requested By:" /></td>
        <td width="50%"><asp:Label ID="lblSummarySubmittedBy" runat="server" /></td>
        <td nowrap class="bold"><asp:Label ID="lblValSubmittedOn" runat="server" Text="Requested On:" /></td>
        <td width="50%"><asp:Label ID="lblSummarySubmittedOn" runat="server" /></td>
    </tr>
    <tr>
        <td nowrap class="bold"><asp:Label ID="lblValMnemonic" runat="server" Text="Mnemonic:" /></td>
        <td width="50%"><asp:Label ID="lblSummaryMnemonic" runat="server" /></td>
        <td nowrap class="bold"><asp:Label ID="lblValQuantity" runat="server" Text="Quantity:" /></td>
        <td width="50%"><asp:Label ID="lblSummaryQuantity" runat="server" /></td>
    </tr>
    <tr>
        <td nowrap class="bold"><asp:Label ID="lblValServerType" runat="server" Text="Platform Type:" /></td>
        <td width="50%"><asp:Label ID="lblSummaryServerType" runat="server" /></td>
        <td nowrap class="bold"><asp:Label ID="lblValStorage" runat="server" Text="Storage:" /></td>
        <td width="50%"><asp:Label ID="lblSummaryStorage" runat="server" /></td>
    </tr>
    <tr>
        <td nowrap class="bold"><asp:Label ID="lblValSize" runat="server" Text="Platform Size:" /></td>
        <td width="50%"><asp:Label ID="lblSummarySize" runat="server" /></td>
        <td nowrap class="bold"><asp:Label ID="lblValHA" runat="server" Text="High Availability:" /></td>
        <td width="50%"><asp:Label ID="lblSummaryHA" runat="server" /></td>
    </tr>
    <tr>
        <td nowrap class="bold"><asp:Label ID="lblValOS" runat="server" Text="Operating System:" /></td>
        <td width="50%"><asp:Label ID="lblSummaryOS" runat="server" /></td>
        <td nowrap class="bold"><asp:Label ID="lblValSpecial" runat="server" Text="Platform Boot Type:" /></td>
        <td width="50%"><asp:Label ID="lblSummaryBootType" runat="server" /></td>
    </tr>
    <tr>
        <td nowrap class="bold"><asp:Label ID="lblValDate" runat="server" Text="Commitment Date:" /></td>
        <td width="50%"><asp:Label ID="lblSummaryDate" runat="server" /></td>
        <td nowrap class="bold"><asp:Label ID="lblValConfidence" runat="server" Text="Confidence Level:" /></td>
        <td width="50%"><asp:Label ID="lblSummaryConfidence" runat="server" />&nbsp;&nbsp;&nbsp;<asp:LinkButton ID="btnUnlock" runat="server" Text="Unlock" Visible="false" /></td>
    </tr>
    <tr>
        <td nowrap class="bold"><asp:Label ID="lblValLocation" runat="server" Text="Location:" /></td>
        <td width="100%" colspan="3"><asp:Label ID="lblSummaryLocation" runat="server" /></td>
    </tr>
    <tr id="divLocation" style="display:none">
        <td colspan="4" width="100%">
            <table width="100%" cellpadding="5" cellspacing="0" border="0">
                <tr>
                    <td>Location is based upon current availability and the reference architecture.</td>
                </tr>
                <tr>
                    <td>
                        <ul>
                            <li style="padding:1px">QA and Production designs will reside in either the Cleveland Operations Center or Summit</li>
                            <li style="padding:1px">Test designs will reside in Dalton (Cincinnati)</li>
                        </ul>
                    </td>
                </tr>
                <tr>
                    <td>If you require a specific location for your application or if your application requiremenets dictate that your infrastructure reside in a certain data center due to co-location or functionality reasons, select the &quot;This does not fit my needs - I need an exception&quot; option below to request an exception.</td>
                </tr>
            </table>
        </td>
    </tr>
    <tr style='display:<%=boolDemo ? "none" : "inline"%>'>
        <td colspan="4" width="100%"><img src="/images/spacer.gif" border="0" height="1" width="1" /></td>
    </tr>
    <tr style='display:<%=boolDemo ? "none" : "inline"%>'>
        <td colspan="4" class="biggerbold">The following solution has been selected:</td>
    </tr>
    <tr style='display:<%=boolDemo ? "none" : "inline"%>' bgcolor='<%=strHighlight %>'>
        <td nowrap class="bold">Solution Determined:</td>
        <td colspan="3" width="100%"><asp:Label ID="lblSummarySolution" runat="server" /></td>
    </tr>
    <tr runat="server" visible="false">
        <td nowrap class="bold">Target Completion Date:</td>
        <td colspan="3" width="100%"><asp:Label ID="lblSummaryTarget" runat="server" /></td>
    </tr>
    <tr>
        <td colspan="4" width="100%"><img src="/images/spacer.gif" border="0" height="1" width="1" /></td>
    </tr>
    <tr id="trException1" runat="server" visible="false">
        <td width="100%" colspan="4" class="bold">The following comments were provided to explain the need for the exception:</td>
    </tr>
    <tr id="trException2" runat="server" visible="false">
        <td width="100%" colspan="4">
            <table cellpadding="2" cellspacing="2" border="0">
                <tr>
                    <td valign="top"><img src="/images/icon_answer.gif" border="0" align="absmiddle" /></td>
                    <td valign="top"><asp:Label ID="lblException" runat="server" /></td>
                </tr>
            </table>
        </td>
    </tr>
    <tr id="trException3" runat="server" visible="false">
        <td nowrap class="bold">Exception ID:</td>
        <td colspan="3" width="100%"><asp:Label ID="lblExceptionID" runat="server" /></td>
    </tr>
</table>
