<%@ Page Language="C#" MasterPageFile="~/frame.Master" AutoEventWireup="true" Codebehind="file.aspx.cs"
    Inherits="NCC.ClearView.Presentation.Web.file" %>

<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" runat="Server">

    <script type="text/javascript">
    function SwapDesc(oHide, oShow) {
        document.getElementById(oHide).style.display = "none";
        document.getElementById(oShow).style.display = "inline";
    }
    function RCH(str) {
        alert(str);
        return false;
    }
    </script>

    <table width="100%" height="100%" cellpadding="1" cellspacing="1" border="0">
        <asp:Panel ID="panShow" runat="server" Visible="false">
            <tr height="1">
                <td valign="top">
                    <table width="100%" cellpadding="2" cellspacing="2" border="0">
                        <tr>
                            <td rowspan="6" valign="top">
                                <asp:Image ID="imgIcon" runat="server" ImageAlign="AbsMiddle" /></td>
                            <td colspan="2">
                                <asp:Label ID="lblTitle" runat="server" CssClass="header" /></td>
                        </tr>
                        <tr>
                            <td nowrap>
                                <b>Created On:</b></td>
                            <td width="100%">
                                <asp:Label ID="lblCreated" runat="server" CssClass="default" /></td>
                        </tr>
                        <tr>
                            <td nowrap>
                                <b>Last Updated:</b></td>
                            <td width="100%">
                                <asp:Label ID="lblUpdated" runat="server" CssClass="default" /></td>
                        </tr>
                        <tr>
                            <td nowrap>
                                <b>File Size:</b></td>
                            <td width="100%">
                                <asp:Label ID="lblSize" runat="server" CssClass="default" /></td>
                        </tr>
                        <tr>
                            <td nowrap>
                                <b>Security:</b></td>
                            <td width="100%">
                                <asp:Label ID="lblSecurity" runat="server" CssClass="default" /></td>
                        </tr>
                        <tr>
                            <td nowrap valign="top">
                                <b>Description:</b></td>
                            <td width="100%">
                                <asp:Panel ID="panDescription" runat="server" Visible="false">
                                    <div id="divDescNo">
                                        <a href="javascript:void(0);" onclick="SwapDesc('divDescNo','divDescYes');">[Click here
                                            to view the description]</a></div>
                                    <div id="divDescYes" style="display: none">
                                        <asp:Label ID="lblDescription" runat="server" CssClass="default" /><br />
                                        <a href="javascript:void(0);" onclick="SwapDesc('divDescYes','divDescNo');">[Hide Description]</a></div>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </td>
                <td valign="top" align="right">
                    <table cellpadding="2" cellspacing="2" border="0">
                        <tr>
                            <td>
                                <asp:Button ID="btnSave" runat="server" CssClass="default" Text="Save File" Width="125"
                                    OnClick="btnSave_Click" /></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btnPermissions" runat="server" CssClass="default" Text="Properties"
                                    Width="125" Enabled="false" /></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btnDelete" runat="server" CssClass="default" Text="Delete File" Width="125"
                                    OnClick="btnDelete_Click" Enabled="false" /></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btnClose" runat="server" CssClass="default" Text="Close Window" Width="125" /></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr height="1">
                <td colspan="2" style="border-bottom: dotted 1px #999999">
                    &nbsp;</td>
            </tr>
            <tr height="1">
                <td colspan="2" class="lightheader">
                    Preview:</td>
            </tr>
        </asp:Panel>
        <tr>
            <td colspan="2">
                <asp:Panel ID="panDenied" runat="server" Visible="false">
                    <table width="100%" height="100%" cellpadding="2" cellspacing="2" border="0">
                        <tr>
                            <td align="center" class="header">
                                <table>
                                    <tr>
                                        <td align="center" class="header">
                                            Access Denied</td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            You do not have access to this file</td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:Button ID="btnDenied" runat="server" CssClass="default" Text="Close Window"
                                                Width="100" /></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="panPreview" runat="server" Visible="false">
                    <table width="100%" height="100%" cellpadding="2" cellspacing="2" border="0">
                        <tr>
                            <td align="center">
                                <%=strPreview %>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
    </table>
</asp:Content>
