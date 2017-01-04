<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="enhancement_new.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.enhancement_new" %>


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
            <asp:Panel ID="panEnhancement" runat="server" Visible="false">
                <table width="100%" cellpadding="0" cellspacing="5" border="0">
                    <tr>
                        <td rowspan="2"><img src="/images/bigHelp.gif" border="0" align="absmiddle" /></td>
                        <td class="header" width="100%" valign="bottom">Enhancement Request</td>
                    </tr>
                    <tr>
                        <td width="100%" valign="top">If you have an idea to enhance a current module, please select the module and complete the following form.</td>
                    </tr>
                </table>
                  <table width="100%" cellpadding="4" cellspacing="3" border="0">
                   <tr>
                        <td nowrap>Enhancement Title:<font class="required">&nbsp;*</font></td>
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
                        <td nowrap>Approximate number of users benefitted:</td>
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
                    <tr title="How urgent is your request? When do you want us to start working on this enhancement?">
                        <td nowrap>Estimated Start Date:<font class="required">&nbsp;*</font></td>
                        <td width="100%"><asp:TextBox id="txtStartDate" runat="server" CssClass="default" Width="100" /> <asp:ImageButton ID="imgStartDate" runat="server" CssClass="default" ImageUrl="/images/calendar.gif" ImageAlign="AbsMiddle" /></td>
                    </tr>
                    <tr title="When do you need this enhancement to be completed by?">
                        <td nowrap>Estimated End Date:<font class="required">&nbsp;*</font></td>
                        <td width="100%"><asp:TextBox id="txtEndDate" runat="server" CssClass="default" Width="100" /> <asp:ImageButton ID="imgEndDate" runat="server" CssClass="default" ImageUrl="/images/calendar.gif" ImageAlign="AbsMiddle" /></td>
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
                    <tr>
                        <td nowrap class="bold">Scheduled Release Date:</td>
                        <td width="100%"><asp:Label ID="lblRelease" runat="server" CssClass="default" /></td>
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
            </asp:Panel>
            <asp:Panel ID="panNoEnhancement" runat="server" Visible="false">
                <table width="100%" cellpadding="5" cellspacing="5" border="0">
                    <tr>
                        <td rowspan="2" valign="top"><img src="/images/hugeAlert.gif" border="0" align="absmiddle" /></td>
                        <td class="header" width="100%" valign="bottom">ClearView Enhancements - Please Read</td>
                    </tr>
                    <tr>
                        <td width="100%" valign="top">
                            <p>Clearview now offers support services using Service Manager. View the services in the <strong>/Application/Clearview</strong> folder under Service Manager. <a href="/ResourceRequest/NewRequest/default.aspx?rid=0&sid=179">Go There Now</a>.</p>
                            <p><b>NOTE:</b> Existing enhancements can still be monitored by using the &quot;My Enhancements&quot; queue.</p>
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
 