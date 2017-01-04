<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="rr_service_editor.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.rr_service_editor" %>


<script type="text/javascript">
function UpdateDDL(strQuestion) {
    var oTemp = document.getElementById('hdnGEN_' + strQuestion);
    oTemp.value = strValue;
}
</script>
<table width="100%" border="0" cellSpacing="2" cellPadding="2" class="default">
    <tr>
        <td class="required">* = Required Field</td>
        <td align="right"><asp:LinkButton ID="btnDeliverable" runat="server" CssClass="default" Text="Service Deliverables" Visible="false" /></td>
    </tr>
</table>
<asp:Panel ID="pnlReqDenied" runat="server" CssClass="default" Visible="false">
    <table width="100%" cellpadding="0" cellspacing="5" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
        <tr>
            <td rowspan="2" valign="top"><img src="/images/bigAlert.gif" border="0" align="absmiddle" /></td>
            <td class="header" valign="bottom" width="100%"><asp:Label ID="lblReqDeny" runat="server" CssClass="header" Text="Your service request has been denied for the following reason..." /></td>
        </tr>
        <tr>
            <td valign="bottom">
                <table cellpadding="0" cellspacing="5" border="0">
                    <tr>
                        <td><asp:Label ID="lblReqDenyComment" runat="server" CssClass="default" Text="Comments : " /></td>
                        <td valign="top"><asp:Label ID="lblReqDenyCommentValue" runat="server" CssClass="default" Text="" /></td>
                    </tr>
                </table>
                <p class="biggerbold"><img src="/images/arrow_green_right.gif" border="0" align="absmiddle" /> Please modify the request and click the &quot;Update&quot; button</p>
            </td>
        </tr>
    </table>
    <br />
</asp:Panel>
<asp:Panel ID="pnlReqReturn" runat="server" CssClass="default" Visible="false">
    <table width="95%" cellpadding="0" cellspacing="5" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
        <tr>
            <td rowspan="2" valign="top"><img src="/images/hugeAlert.gif" border="0" align="absmiddle" /></td>
            <td class="header" valign="bottom" width="100%"><asp:Label ID="lblReqReturn" runat="server" CssClass="header" Text="This service request has been returned by " /><asp:Label ID="lblReqReturnedByValue" runat="server" CssClass="header" Text="User" /><asp:Label ID="lblReqReturnedId" runat="server" CssClass="default" Text="0" Visible="false" /></td>
        </tr>
        <tr>
            <td valign="bottom">
                <table cellpadding="0" cellspacing="5" border="0">
                    <tr>
                        <td class="bold">Reason / Comments:</td>
                    </tr>
                    <tr>
                        <td><asp:Label ID="lblReqReturnCommentValue" runat="server" CssClass="default" Text="" /></td>
                    </tr>
                </table>
                <table id="pnlReqReturn2" runat="server" visible="false" cellpadding="0" cellspacing="5" border="0">
                    <tr><td><hr size="1" noshade /></td></tr>
                    <tr>
                        <td><asp:Label ID="lblReqReturnedBy2" runat="server" CssClass="default" Text="Previously Returned By " /><asp:Label ID="lblReqReturnedByValue2" runat="server" CssClass="bold" Text="User" /> with the following comments:<asp:Label ID="lblReqReturnedId2" runat="server" CssClass="default" Text="0" Visible="false" /></td>
                    </tr>
                    <tr>
                        <td><asp:Label ID="lblReqReturnCommentValue2" runat="server" CssClass="default" Text="" /></td>
                    </tr>
                </table>
                <table cellpadding="0" cellspacing="5" border="0">
                    <tr>
                        <td><img src="/images/tack.gif" border="0" align="absmiddle" /></td>
                        <td class="greendefault">After making the necessary changes, click the &quot;Update&quot; button at the bottom of this page <b>OR</b> click the &quot;Cancel Request&quot; button to cancel this request.</td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <br />
</asp:Panel>
<table width="100%" border="0" cellSpacing="2" cellPadding="2" class="default">
    <asp:Panel ID="panTitle" runat="server" Visible="false">
    <tr>
        <td colspan="2"><asp:Label ID="lblTitleName" runat="server" CssClass="default" Text="Please enter a title for this request" />:<span class="required">&nbsp;*</span></td>
    </tr>
    <tr>
        <td><img src="/images/spacer.gif" border="0" width="50" height="1"/></td>
        <td width="100%"><asp:TextBox ID="txtTitle" runat="server" CssClass="default" MaxLength="100" Width="500" /></td>
    </tr>
    </asp:Panel>
        <asp:Panel ID="panStatement" runat="server" Visible="false">
        <tr>
            <td colspan="2">&nbsp;</td>
        </tr>
        <tr>
            <td colspan="2">Please enter your statement of work:<span class="required">&nbsp;*</span></td>
        </tr>
        <tr>
            <td><img src="/images/spacer.gif" border="0" width="50" height="1"/></td>
            <td width="100%"><asp:TextBox ID="txtStatement" runat="server" CssClass="default" TextMode="MultiLine" Rows="8" Width="80%" /></td>
        </tr>
        </asp:Panel>
    <asp:Panel ID="panHide" runat="server" Visible="false">
        <tr>
            <td colspan="2">&nbsp;</td>
        </tr>
        <tr>
            <td colspan="2">Please select your priority:<span class="required">&nbsp;*</span></td>
        </tr>
        <tr>
            <td><img src="/images/spacer.gif" border="0" width="50" height="1"/></td>
            <td width="100%">
                <asp:RadioButtonList ID="radPriority" runat="server" CssClass="default" RepeatDirection="Horizontal">
                    <asp:ListItem Value="1" />
                    <asp:ListItem Value="2" />
                    <asp:ListItem Value="3" />
                    <asp:ListItem Value="4" />
                    <asp:ListItem Value="5" />
                </asp:RadioButtonList>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="footer">Highest&nbsp;&nbsp;<img src="/images/small_arrow_right.gif" border="0" align="absmiddle" />&nbsp;&nbsp;Lowest</span>
            </td>
        </tr>
        <asp:Panel ID="panDeliverable" runat="server" Visible="false">
        <tr>
            <td colspan="2">&nbsp;</td>
        </tr>
        <tr>
            <td colspan="2">Based on your priority, the service level agreement is:</td>
        </tr>
        <tr>
            <td><img src="/images/spacer.gif" border="0" width="50" height="1"/></td>
            <td width="100%"><b><asp:Label ID="lblDeliverable" runat="server" CssClass="bold" /> days</b></td>
        </tr>
        </asp:Panel>
    </asp:Panel>
</table>
<asp:Panel ID="panForm" runat="server" CssClass="default">
    <%=strForm %>
</asp:Panel>
<table width="100%" border="0" cellSpacing="2" cellPadding="2" class="default">
    <asp:Panel ID="panExpedite" runat="server" Visible="false">
    <tr>
        <td colspan="2">&nbsp;</td>
    </tr>
    <tr>
        <td colspan="2">Please select if you would like to expedite this request:<span class="required">&nbsp;*</span></td>
    </tr>
    <tr>
        <td><img src="/images/spacer.gif" border="0" width="50" height="1"/></td>
        <td width="100%">
            <asp:RadioButton ID="radExpediteYes" runat="server" Text="Yes" CssClass="default" GroupName="expedite" />&nbsp;
            <asp:RadioButton ID="radExpediteNo" runat="server" Text="No" CssClass="default" GroupName="expedite" />
        </td>
    </tr>
    </asp:Panel>
    <asp:Panel ID="panUpload" runat="server" Visible="false">
    <tr>
        <td colspan="2">&nbsp;</td>
    </tr>
    <tr>
        <td colspan="2">Click the following button to upload and articles / documentation associated with this request:</td>
    </tr>
    <tr>
        <td><img src="/images/spacer.gif" border="0" width="50" height="1"/></td>
        <td width="100%"><asp:Button ID="btnDocuments" runat="server" Text="Click to Upload" Width="125" CssClass="default" /></td>
    </tr>
    </asp:Panel>
</table>
<table width="100%" border="0" cellSpacing="2" cellPadding="4" class="default">
    <tr>
        <td colspan="2"><hr size="1" noshade /></td>
    </tr>
    <tr>
        <td>
            <asp:Panel ID="panNavigation" runat="server" Visible="false">
                <asp:Button ID="btnBack" runat="server" OnClick="btnBack_Click" CssClass="default" Text="<<  Back" Width="100" /> 
                <asp:Button ID="btnNext" runat="server" OnClick="btnNext_Click" CssClass="default" Text="Next  >>" Width="100" /> 
            </asp:Panel>
            <asp:Panel ID="panUpdate" runat="server" Visible="false">
                <asp:Button ID="btnUpdate" runat="server" OnClick="btnUpdate_Click" CssClass="default" Text="Update" Width="100" /> 
                <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" CssClass="default" Text="Cancel" Width="100" /> 
            </asp:Panel>
        </td>
        <td align="right">
            <asp:Button ID="btnCancelR" runat="server" OnClick="btnCancelR_Click" CssClass="default" Text="Cancel Request" Width="125" />
        </td>
    </tr>
</table>

<asp:Label ID="lblItem" runat="server" Visible="false" />
<asp:Label ID="lblNumber" runat="server" Visible="false" />
<asp:Label ID="lblService" runat="server" Visible="false" />
<asp:Label ID="lblReturned" runat="server" Visible="false" />
<asp:Label ID="lblPRequest" runat="server" Visible="false" />
<asp:HiddenField ID="hdnEnd" runat="server" />