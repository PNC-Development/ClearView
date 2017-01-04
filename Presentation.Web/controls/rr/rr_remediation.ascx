<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="rr_remediation.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.rr_remediation" %>


<div id="divShow" runat="server" style="display:inline">
<table width="100%" border="0" cellSpacing="2" cellPadding="4" class="default">
    <tr>
        <td nowrap>Working Sponsor:<font class="required">&nbsp;*</font></td>
        <td width="100%">
            <table cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td><asp:TextBox ID="txtWorking" runat="server" Width="300" CssClass="default" /></td>
                </tr>
                <tr>
                    <td>
                        <div id="divWorking" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                            <asp:ListBox ID="lstWorking" runat="server" CssClass="default" />
                        </div>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td nowrap>Reason:<font class="required">&nbsp;*</font></td>
        <td width="100%">
            <asp:DropDownList ID="ddlReason" runat="server" CssClass="default">
                <asp:ListItem Value="-- SELECT --" />
                <asp:ListItem Value="Vendor Advisory" />
                <asp:ListItem Value="Break-Fix" />
                <asp:ListItem Value="Best Practice" />
                <asp:ListItem Value="Certification" />
                <asp:ListItem Value="Decommission" />
                <asp:ListItem Value="Capacity" />
                <asp:ListItem Value="Rebuild" />
                <asp:ListItem Value="Facility" />
                <asp:ListItem Value="Remediation" />
                <asp:ListItem Value="Maintenance" />
                <asp:ListItem Value="Health Check" />
                <asp:ListItem Value="Other..." />
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td nowrap>Component:<font class="required">&nbsp;*</font></td>
        <td width="100%">
            <asp:DropDownList ID="ddlComponent" runat="server" CssClass="default">
                <asp:ListItem Value="-- SELECT --" />
                <asp:ListItem Value="Hardware" />
                <asp:ListItem Value="Software" />
                <asp:ListItem Value="Performance" />
                <asp:ListItem Value="Other..." />
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td nowrap>Estimated Funding Cost:<font class="required">&nbsp;*</font></td>
        <td width="100%">
            <asp:DropDownList ID="ddlFunding" runat="server" CssClass="default" Visible="false">
                <asp:ListItem Value="-- SELECT --" />
                <asp:ListItem Value="$0 - No Funding Required" />
                <asp:ListItem Value="$1 - $500" />
                <asp:ListItem Value="$500 - $1000" />
                <asp:ListItem Value="$1000 - $5000" />
            </asp:DropDownList>
            <asp:Label ID="lblFunding" runat="server" CssClass="default" Visible="false"/>
        </td>
    </tr>
    <tr>
        <td nowrap>Priority:<font class="required">&nbsp;*</font></td>
        <td width="100%">
            <table cellpadding="2" cellspacing="0" border="0">
                <tr>
                    <td>
                        <asp:RadioButtonList ID="radPriority" runat="server" CssClass="default" RepeatDirection="Horizontal">
                            <asp:ListItem Value="1" />
                            <asp:ListItem Value="2" />
                            <asp:ListItem Value="3" />
                            <asp:ListItem Value="4" />
                            <asp:ListItem Value="5" />
                        </asp:RadioButtonList>
                    </td>
                    <td>&nbsp;</td>
                    <td class="error">[<asp:Label ID="lblDeliverable" runat="server" CssClass="error" /> days]</td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td nowrap>&nbsp;</td>
        <td width="100%" class="footer">(Highest --> Lowest)</td>
    </tr>
    <tr>
        <td nowrap>Require Technical Project Manager:<font class="required">&nbsp;*</font></td>
        <td width="100%">
            <asp:RadioButton ID="radTPM_Yes" runat="server" Text="Yes" CssClass="default" GroupName="TPM" />&nbsp;
            <asp:RadioButton ID="radTPM_No" runat="server" Text="No" CssClass="default" GroupName="TPM" />
        </td>
    </tr>
    <asp:Panel ID="panPM" runat="server" Visible="false">
    <tr>
        <td nowrap><img src="/images/spacer.gif" border="0" width="20" height="1" /><img src="/images/downright.gif" border="0" align="absmiddle" /> <asp:Label ID="lblPM" runat="server" CssClass="default" /></td>
        <td width="100%"><asp:Label ID="lblPMName" runat="server" CssClass="error" /></td>
    </tr>
    </asp:Panel>
    <tr>
        <td nowrap valign="top">Statement of Work:<font class="required">&nbsp;*</font></td>
        <td width="100%" valign="top"><asp:TextBox ID="txtStatement" runat="server" CssClass="default" TextMode="MultiLine" Rows="5" Width="500" /></td>
    </tr>
    <tr>
        <td nowrap>Number of Devices:<font class="required">&nbsp;*</font></td>
        <td width="100%"><asp:TextBox ID="txtDevices" runat="server" CssClass="default" Width="100" MaxLength="10" /></td>
    </tr>
    <tr>
        <td nowrap>Number of Hours:<font class="required">&nbsp;*</font></td>
        <td width="100%"><asp:TextBox ID="txtHours" runat="server" CssClass="default" Width="100" MaxLength="10" /></td>
    </tr>
    <tr>
        <td nowrap>Articles / Documentation:</td>
        <td width="100%"><img src="/images/upload.gif" border="0" align="absmiddle" /> <asp:LinkButton ID="btnDocuments" runat="server" Text="Click Here to Upload" /></td>
    </tr>
    <tr>
        <td nowrap>Estimated Start Date:<font class="required">&nbsp;*</font></td>
        <td width="100%"><asp:TextBox ID="txtStart" runat="server" CssClass="default" Width="100" MaxLength="10" /> <asp:ImageButton ID="imgStart" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
    </tr>
    <tr>
        <td nowrap>Estimated End Date:<font class="required">&nbsp;*</font></td>
        <td width="100%"><asp:TextBox ID="txtEnd" runat="server" CssClass="default" Width="100" MaxLength="10" /> <asp:ImageButton ID="imgEnd" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
    </tr>
    <tr>
        <td colspan="2" class="greentableheader">Change Control (Optional)</td>
    </tr>
    <tr>
        <td nowrap>Number:</td>
        <td width="100%"><asp:TextBox ID="txtNumber" runat="server" CssClass="default" Width="150" MaxLength="15" /></td>
    </tr>
    <tr>
        <td nowrap>Date:</td>
        <td width="100%"><asp:TextBox ID="txtDate" runat="server" CssClass="default" Width="100" MaxLength="10" /> <asp:ImageButton ID="imgDate" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
    </tr>
    <tr>
        <td nowrap>Time:</td>
        <td width="100%"><asp:TextBox ID="txtTime" runat="server" CssClass="default" Width="100" MaxLength="10" /></td>
    </tr>
    <tr>
        <td nowrap valign="top">Comments:</td>
        <td width="100%" valign="top"><asp:TextBox ID="txtChange" runat="server" CssClass="default" TextMode="MultiLine" Rows="5" Width="500" /></td>
    </tr>
    <tr>
        <td colspan="2"><hr size="1" noshade /></td>
    </tr>
    <tr>
        <td colspan="2">
            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td>
                        <table cellpadding="4" cellspacing="4" border="0">
                            <tr>
                                <td><asp:Button ID="btnBack" runat="server" OnClick="btnBack_Click" CssClass="default" Text="<<  Back" Width="100" /></td>
                                <td>
                                    <asp:Panel ID="panDeliverable" runat="server" Visible="false">
                                        <asp:Button ID="btnDeliverable" runat="server" CssClass="default" Text="Service Deliverables" Width="150" />
                                    </asp:Panel>
                                </td>
                                <td><asp:Button ID="btnNext" runat="server" OnClick="btnNext_Click" CssClass="default" Text="Next  >>" Width="100" /></td>
                            </tr>
                        </table>
                    </td>
                    <td align="right">
                        <table cellpadding="4" cellspacing="4" border="0">
                            <tr>
                                <td><asp:Button ID="btnCancel1" runat="server" OnClick="btnCancel_Click" CssClass="default" Text="Cancel Request" Width="125" /></td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
</div>
<div id="divHide" runat="server" style="display:none">
    <table width="100%" cellpadding="2" cellspacing="2" border="0">
        <tr>
            <td><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
            <td class="header"><img src="/images/bigAlert.gif" border="0" align="absmiddle" /> Project Request Required</td>
        </tr>
        <tr><td colspan="2">&nbsp;</td></tr>
        <tr>
            <td><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
            <td>
                <p>This resource request has failed validation.  A project request must be completed for a resource to be assigned for one or more of the following reasons:</p>
                <ul>
                    <li>More than 40 Labor Hours</li>
                    <li>More than $1,000.00</li>
                    <li>Requires a Technical Project Manager</li>
                </ul>
            </td>
        </tr>
        <tr>
            <td><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
            <td><asp:LinkButton ID="lnkRequest" runat="server" Text="Click here to complete a project request." OnClick="lnkRequest_Click" /></td>
        </tr>
        <tr><td colspan="2">&nbsp;</td></tr>
        <tr>
            <td colspan="2"><hr size="1" noshade /></td>
        </tr>
        <tr>
            <td colspan="2" align="right">
                <table width="100%" cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td class="footer"></td>
                        <td align="right"><asp:LinkButton ID="btnBack2" runat="server" Text="<img src='/images/bigGreenArrowLeft.gif' border='0' align='absmiddle'/><img src='/images/spacer.gif' border='0' width='10' height='20' align='absmiddle'/>Back" /><img src="/images/spacer.gif" border="0" width="25" height="1" /><asp:LinkButton ID="btnClose" runat="server" Text="<img src='/images/bigError.gif' border='0' align='absmiddle'/><img src='/images/spacer.gif' border='0' width='10' height='20' align='absmiddle'/>Cancel" /></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
<asp:Label ID="lblItem" runat="server" Visible="false" />
<asp:Label ID="lblNumber" runat="server" Visible="false" />
<asp:HiddenField ID="hdnWorking" runat="server" />
<asp:HiddenField ID="hdnEnd" runat="server" />