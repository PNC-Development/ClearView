<%@ Page Language="C#" MasterPageFile="~/datapoint.Master" AutoEventWireup="true" CodeBehind="datapoint_resource.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.datapoint_resource" Title="Datapoint Resource" %>

<%@ Register Src="~/datapoint/controls/wucResourceInvolvement.ascx"
    TagName="wucResourceInvolvement" TagPrefix="ucResourceInvolvement" %>
    
<%@ Register Src="~/datapoint/controls/wucServiceProgression.ascx"
TagName="wucServiceProgression" TagPrefix="ucServiceProgression" %>

<%@ Register Src="~/controls/UserContactInfo.ascx"
TagName="wucUserContactInfo" TagPrefix="ucUserContactInfo" %>


<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" runat="server">
<asp:Panel ID="pnlAllow" runat="server" Visible="false">
    <asp:Panel ID="pnlResourceInfo" runat="server" Visible="true">
        <table width="95%" cellpadding="5" cellspacing="2" border="0">
            <tr>
                <td rowspan="2">
                    <img src="/images/users40.gif" border="0" align="absmiddle" /></td>
                <td class="header" nowrap align="left" valign="bottom" width="100%">
                    Resource Information :
                    <asp:Label ID="lblHeader" runat="server" CssClass="header" /></td>
            </tr>
            <tr>
                <td nowrap valign="top" colspan="2">
                    <asp:Label ID="lblHeaderSub" runat="server" CssClass="default" /></td>
            </tr>
            <tr>
                <td nowrap valign="top" colspan="2">
                    &nbsp;</td>
            </tr>
        </table>
        <asp:Panel ID="pnlTabs" runat="server" Visible="true">
            <%=strMenuTab1%>
            <div id="divMenu1" class="tabbing">
                <div id="divResourceInfo" style="width: 100%;">
                    <table width="95%" cellpadding="4" cellspacing="4" border="0" align="center">
                        <tr>
                            <td class="default" width="30%">
                                LAN ID
                            </td>
                            <td class="default" width="70%">
                                <asp:Label ID="lblLANId" runat="server" CssClass="default" Text="" /></td>
                        </tr>
                        <tr>
                            <td class="default" width="30%">
                                First name</td>
                            <td class="default" width="70%">
                                <asp:Label ID="lblFName" runat="server" CssClass="default" Text="" /></td>
                        </tr>
                        <tr>
                            <td class="default" width="30%">
                                Last name</td>
                            <td class="default" width="70%">
                                <asp:Label ID="lblLName" runat="server" CssClass="default" Text="" /></td>
                        </tr>
                        <tr>
                            <td class="default" width="30%">
                                Manager</td>
                            <td class="default" width="70%">
                                <asp:Label ID="lblManager" runat="server" CssClass="default" Text="" /></td>
                        </tr>
                        <tr>
                            <td class="default" width="30%">
                                Is Manager</td>
                            <td class="default" width="70%">
                                <asp:CheckBox ID="chkIsManager" runat="server" Checked="false" Enabled="false" /></td>
                        </tr>
                        <tr>
                            <td class="default" width="30%">
                                Is Board Member</td>
                            <td class="default" width="70%">
                                <asp:CheckBox ID="chkIsBoardMember" runat="server" Checked="true" Enabled="false" /></td>
                        </tr>
                        <tr>
                            <td class="default" width="30%">
                                Is Director</td>
                            <td class="default" width="70%">
                                <asp:CheckBox ID="chkIsDirector" runat="server" Checked="true" Enabled="false" /></td>
                        </tr>
                        <tr>
                            <td class="default" width="30%">
                                Pager</td>
                            <td class="default" width="70%">
                                <asp:Label ID="lblPager" runat="server" CssClass="default" Text="" /></td>
                        </tr>
                        <tr>
                            <td class="default" width="30%">
                                Phone</td>
                            <td class="default" width="70%">
                                <asp:Label ID="lblPhone" runat="server" CssClass="default" Text="" /></td>
                        </tr>
                        <tr>
                            <td class="default" width="30%">
                                Special Skills</td>
                            <td class="default" width="70%">
                                <asp:Label ID="lblSpecialSkills" runat="server" CssClass="default" Text="" /></td>
                        </tr>
                        <tr>
                            <td class="default" width="30%">
                                Vacation Days</td>
                            <td class="default" width="70%">
                                <asp:Label ID="lblVacationDays" runat="server" CssClass="default" Text="" /></td>
                        </tr>
                        <tr>
                            <td class="default" width="30%">
                                Multiple Applications</td>
                            <td class="default" width="70%">
                                <asp:CheckBox ID="chkMultipleApps" runat="server" Enabled="false" /></td>
                        </tr>
                        <tr>
                            <td class="default" width="30%">
                                Can Add Locations</td>
                            <td class="default" width="70%">
                                <asp:CheckBox ID="chkAddLocation" runat="server" Enabled="false" /></td>
                        </tr>
                        <tr>
                            <td class="default" width="30%">
                                Administrator</td>
                            <td class="default" width="70%">
                                <asp:CheckBox ID="chkAdmin" runat="server" Checked="true" Enabled="false" /></td>
                        </tr>
                        <tr>
                            <td class="default" width="30%">
                                Enabled:</td>
                            <td class="default" width="70%">
                                <asp:CheckBox ID="chkEnabled" runat="server" Checked="true" Enabled="false" /></td>
                        </tr>
                        <tr>
                            <td class="default" width="30%">
                                Picture:</td>
                            <td class="default" width="70%">
                                <asp:Image ID="imgPicture" runat="server" Width="90" Height="90" />
                            </td>
                        </tr>
                        <tr>
                            <td height="5" colspan="2">
                                <img src="images/spacer.gif" width="1" height="5" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divServices">
                   <ucServiceProgression:wucServiceProgression ID="ucServiceProgression" runat="server" />
                </div>
                <div id="divResourceInvolvement">
                    <ucResourceInvolvement:wucResourceInvolvement ID="ucResourceInvolvement" runat="server" />
                </div>
                <div id="divPersonalAssets">
                    <asp:Label ID="lblPersonalAssets" runat="server" CssClass="default" Text="Disabled" />
                </div>
                <div id="divDocuments">
                    <asp:Label ID="lblDocuments" runat="server" CssClass="default" Text="Disabled" />
                </div>
                <div id="divRaves">
                    <asp:Label ID="lblRaves" runat="server" CssClass="default" Text="Disabled" />
                </div>
                <div id="divEducationAndCerts">
                    <asp:Label ID="lblEducationAndCerts" runat="server" CssClass="default" Text="Disabled" />
                </div>
                <div id="divBlog">
                    <asp:Label ID="lblBlog" runat="server" CssClass="default" Text="Disabled" />
                </div>
                 <div id="divUserContactInfo">
                    <ucUserContactInfo:wucUserContactInfo ID="ucUserContactInfo" runat="server" />
                </div>
            </div>
            
        </asp:Panel>
    </asp:Panel>
    <asp:Panel ID="pnlDenied" runat="server" Visible="false">
        <br />
        <table width="100%" cellpadding="0" cellspacing="5" border="0">
            <tr>
                <td rowspan="2">
                    <img src="/images/ico_error.gif" border="0" align="absmiddle" id="IMG1" onclick="return IMG1_onclick()" /></td>
                <td class="header" width="100%" valign="bottom">
                    Access Denied</td>
            </tr>
            <tr>
                <td width="100%" valign="top">
                    You do not have sufficient permission to view this page.</td>
            </tr>
            <tr>
                <td colspan="2">
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                </td>
                <td width="100%">
                    If you think you should have rights to view it, please contact your ClearView administrator.</td>
            </tr>
        </table>
        <p>
            &nbsp;</p>
    </asp:Panel>
      <asp:HiddenField ID="hdnTab" runat="server" />
</asp:Panel>
</asp:Content>
