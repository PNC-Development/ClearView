<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="issue_new.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.issue_new" %>


<table width="100%" cellpadding="0" cellspacing="0" border="0" class="default">
    <tr>
        <td><img src="/images/table_topleft.gif" border="0" width="5" height="26"></td>
        <td nowrap background="/images/table_top.gif" class="greentableheader" width="100%"><asp:Label ID="lblTitle" runat="server" CssClass="greentableheader" /></td>
        <td><img src="/images/table_topright.gif" border="0" width="5" height="26"></td>
    </tr>
    <tr>
        <td background="/images/table_left.gif"><img src="/images/table_left.gif" width="5" height="10"></td>
        <td width="100%" bgcolor="#FFFFFF">
            <br /> 
            <asp:Panel ID="panIncident" runat="server" Visible="false">
                <table width="100%" cellpadding="0" cellspacing="5" border="0">
                    <tr>
                        <td rowspan="2"><img src="/images/bigHelp.gif" border="0" align="absmiddle" /></td>
                        <td class="header" width="100%" valign="bottom">Report an Issue</td>
                    </tr>
                    <tr>
                        <td width="100%" valign="top">If you are having problems with ClearView or have encountered an error, please report it here by completing the form.</td>
                    </tr>
                </table>
                <table width="100%" cellpadding="4" cellspacing="3" border="0">
                    <tr>
                        <td>Would you like to change the functionality of one or more parts of ClearView as part of this request?</td>
                    </tr>
                    <tr>
                        <td>
                            <asp:RadioButton ID="radYes" runat="server" Text="Yes" GroupName="Enhancement" /> 
                            <asp:RadioButton ID="radNo" runat="server" Text="No" GroupName="Enhancement" /> 
                        </td>
                    </tr>
                </table>
                <div id="divYes" runat="server" style="display:none">
                    <table width="100%" cellpadding="0" cellspacing="5" border="0">
                        <tr>
                            <td><img src="/images/bigAlert.gif" border="0" align="absmiddle" /></td>
                            <td width="100%">You should submit an <b>Enhancement</b> to request a change in the functionality of ClearView.  Submitting an <b>Issue</b> for an enhancment will most likely cause your request to be rejected or cancelled.</td>
                        </tr>
                    </table>
                </div>
                <div id="divNo" runat="server" style="display:none">
                  <table width="100%" cellpadding="4" cellspacing="3" border="0">
                   <tr>
                        <td nowrap>Title:<font class="required">&nbsp;*</font></td>
                        <td width="100%"><asp:TextBox ID="txtTitle" runat="server" CssClass="default" Width="600" MaxLength="100" /></td>
                    </tr>
                    <tr>
                        <td nowrap valign="top">Description:<font class="required">&nbsp;*</font></td>
                        <td width="100%"><asp:TextBox ID="txtDescription" runat="server" CssClass="default" Width="600" Rows="7" TextMode="MultiLine" /></td>
                    </tr>
                    <tr>
                        <td nowrap>Module:</td>
                        <td width="100%"><asp:DropDownList ID="drpModules" runat="server" CssClass="default" /></td>
                    </tr>
                    <tr>
                        <td nowrap>Approximate number of users affected:</td>
                        <td width="100%"><asp:TextBox ID="txtNumUsers" runat="server" CssClass="default" Width="50" MaxLength="100" /> (0 = none)</td>
                    </tr>
                    <tr>
                        <td nowrap>URL:</td>
                        <td width="100%"><asp:TextBox ID="txtURL" runat="server" CssClass="default" Width="500" MaxLength="100" /></td>
                    </tr>
                    <tr>
                        <td nowrap>Screenshot / Attachment:</td>
                        <td width="100%">
                            <asp:Panel ID="panUpload" runat="server" Visible="false">
                                <asp:FileUpload id="fileUpload" runat="server" CssClass="default" Width="600" />
                            </asp:Panel>
                            <asp:Panel ID="panUploaded" runat="server" Visible="false">
                                <asp:HyperLink id="hypUpload" runat="server" Target="_blank" Text="Click Here to View File" />&nbsp;&nbsp;&nbsp;<asp:Button ID="btnDeleteAttachment" runat="server" CssClass="default" Width="75" Text="Delete" OnClick="btnDeleteAttachment_Click" />
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td nowrap>Requested By:</td>
                        <td width="100%"><asp:Label ID="lblRequestBy" runat="server" CssClass="default" /></td>
                    </tr>
                    <tr>
                        <td nowrap>Requested On:</td>
                        <td width="100%"><asp:Label ID="lblRequestOn" runat="server" CssClass="default" /></td>
                    </tr>
                    <asp:Panel ID="panStatus" runat="server" Visible="false">
                    <tr>
                        <td nowrap bgcolor="#C1FFC1" class="bold">Current Status:</td>
                        <td width="100%"><asp:Label ID="lblStatus" runat="server" CssClass="default" /></td>
                    </tr> 
                    </asp:Panel>
                    <asp:Panel ID="panActive" runat="server" Visible="false">
                    <tr>
                        <td colspan="2"><hr size="1" noshade /></td>
                    </tr>
                    <tr>
                        <td nowrap class="required">* = Required Field</td>
                        <td width="100%" align="right"><asp:Button ID="btnSave" runat="server" CssClass="default" Text="Submit" Width="100" OnClick="btnSave_Click" Visible="false" /> <asp:Button ID="btnUpdate" runat="server" CssClass="default"  Text="Update" Width="100" OnClick="btnUpdate_Click" Visible="false" /></td>
                    </tr> 
                    </asp:Panel>
                    <asp:Panel ID="panMessage" runat="server" Visible="false">
                    <tr>
                       <td colspan="2">&nbsp;</td>
                    </tr>
                    <tr>
                       <td colspan="2">&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="2" class="wrapper">Message Thread:</td>
                    </tr>
                    <tr>
                        <td colspan="2" class="header"><asp:ImageButton ID="btnMessage" runat="server" ImageUrl="/images/post_reply.gif" ImageAlign="AbsMiddle" ToolTip="Click to Post a Reply" />&nbsp;&nbsp;&nbsp;<img src="/images/arrow_green_left.gif" border="0" align="absmiddle" />&nbsp;&nbsp;&nbsp;Click Here to Reply!</td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <div id="divMessage" runat="server" style="display:none">
                                <table width="100%" cellpadding="3" cellspacing="0" border="0">
                                    <tr>
                                       <td>Enter your response, upload an attachment (optional) and click <b>Send</b> to send your response:</td>
                                    </tr>
                                    <tr>
                                       <td><asp:TextBox ID="txtResponse" runat="server" CssClass="default" Width="600" Rows="7" TextMode="MultiLine" /></td>
                                    </tr>
                                    <tr>
                                        <td><asp:FileUpload id="oFile" runat="server" CssClass="default" Width="600" /></td>
                                    </tr>
                                    <tr>
                                       <td><asp:Button ID="btnResponse" runat="server" CssClass="default" Text="Send" Width="100" OnClick="btnResponse_Click" /></td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <tr>
                       <td colspan="2"><%= strMessages %></td>
                    </tr>         
                    </asp:Panel>
                </table>   
            </div>
            </asp:Panel>
            <asp:Panel ID="panNoIncident" runat="server" Visible="false">
                <table width="100%" cellpadding="5" cellspacing="5" border="0">
                    <tr>
                        <td rowspan="2" valign="top"><img src="/images/hugeAlert.gif" border="0" align="absmiddle" /></td>
                        <td class="header" width="100%" valign="bottom">Service Now - Create an Incident</td>
                    </tr>
                    <tr>
                        <td width="100%" valign="top">
                            <p>For new issues, submit a service now incident and use the assignment group <b>ClearView</b>.</p>
                            <p>If you cannot submit a service now incident, call the help desk and have them create one for you.</p>
                            <p><b>NOTE:</b> Existing issues can still be monitored by using the &quot;My Issues&quot; queue.</p>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <p>&nbsp;</p>
        </td>
        <td background="/images/table_right.gif"><img src="/images/table_right.gif" width="5" height="10"></td>
    </tr>
    <tr>
        <td><img src="/images/table_bottomLeft.gif" border="0" width="5" height="9"></td>
        <td width="100%" background="/images/table_bottom.gif"></td>
        <td><img src="/images/table_bottomRight.gif" border="0" width="5" height="9"></td>
    </tr>
</table>
 