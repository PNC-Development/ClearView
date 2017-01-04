<%@ Page Language="C#" MasterPageFile="~/clearview.Master" AutoEventWireup="true" CodeBehind="build_fixed.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.build_fixed" Title="Provisioning Issue" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" Runat="Server">
<script type="text/javascript">
</script>
<table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0">
    <tr height="1" style="display:none"> 
        <td bgcolor="#007253"><img src="/images/header_wide.aspx" border="0" /></td>
        <td bgcolor="#007253" align="right"><asp:PlaceHolder ID="PH4" runat="server" /></td>
    </tr>
    <tr height="1"> 
        <td colspan="2">
            <table width="100%" border="0" cellspacing="0" cellpadding="5">
                <tr style="background-color:#FFFFFF; display:inline">
                    <td><img src="/images/PNCHeaderLogo.gif" border="0" /></td>
                    <td align="right"><img src="/images/HeaderGradient.gif" border="0" /></td>
                </tr>
                <tr style="background-color:#FFFFFF; display:none">
                    <td background="/images/PNCLogoBack.gif" width="100%"><img src="/images/PNCLogo.gif" border="0" /></td>
                    <td align="right"><img src="/images/HeaderGradient.gif" border="0" /></td>
                </tr>
                <tr>
                    <td colspan="2" align="right" class="whitedefault"><DIV id=thinOrangeBar><asp:label ID="Label1" runat="server" CssClass="whitedefault" />&nbsp;&nbsp;</DIV></td>
                </tr>
            </table>
        </td>
    </tr>
    <tr> 
        <td colspan="2" bgcolor="#E9E9E9">
            <table width="98%" height="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                <tr height="1">
                    <td align="left" valign="top">&nbsp;</td>
                </tr>
                <tr> 
                    <td align="left" valign="top">
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
            <asp:Panel ID="panIssue" runat="server" Visible="false">
                                    <table cellpadding="0" cellspacing="5" border="0">
                                        <tr>
                                            <td rowspan="2"><img src="/images/ico_error.gif" border="0" align="absmiddle" /></td>
                                            <td class="header" valign="bottom">Provisioning Issue</td>
                                        </tr>
                                        <tr>
                                            <td valign="top">Please enter the fix for this issue...</td>
                                        </tr>
                                    </table>
                                    <table cellpadding="5" cellspacing="3" border="0">
                                        <tr>
                                            <td nowrap><asp:Label ID="lblLabel" runat="server" /></td>
                                            <td width="100%"><asp:Label ID="lblValue" runat="server" /></td>
                                        </tr>
                                        <tr>
                                            <td nowrap>Device Name:</td>
                                            <td width="100%"><asp:Label ID="lblName" runat="server" /></td>
                                        </tr>
                                        <tr>
                                            <td nowrap>Type of Device:</td>
                                            <td width="100%"><asp:Label ID="lblType" runat="server" /></td>
                                        </tr>
                                        <tr>
                                            <td nowrap>Date of Issue:</td>
                                            <td width="100%"><asp:Label ID="lblDate" runat="server" /></td>
                                        </tr>
                                        <tr>
                                            <td nowrap>Operating System:</td>
                                            <td width="100%"><asp:Label ID="lblOS" runat="server" /></td>
                                        </tr>
                                        <tr>
                                            <td nowrap>Details:</td>
                                            <td width="100%"><asp:Label ID="lblIssue" runat="server" CssClass="bold" /></td>
                                        </tr>
                                        <asp:Panel ID="panVMwareNo" runat="server" Visible="false">
                                        <tr>
                                            <td nowrap>Management Console:</td>
                                            <td width="100%"><asp:Label ID="lblConsole" runat="server" /></td>
                                        </tr>
                                        </asp:Panel>
                                        <asp:Panel ID="panVMware" runat="server" Visible="false">
                                        <tr>
                                            <td nowrap>Virtual Center Server:</td>
                                            <td width="100%"><asp:Label ID="lblVirtualCenter" runat="server" /></td>
                                        </tr>
                                        <tr>
                                            <td nowrap>DataCenter:</td>
                                            <td width="100%"><asp:Label ID="lblDataCenter" runat="server" /></td>
                                        </tr>
                                        <tr>
                                            <td nowrap>Folder:</td>
                                            <td width="100%"><asp:Label ID="lblFolder" runat="server" /></td>
                                        </tr>
                                        <tr>
                                            <td nowrap>Cluster:</td>
                                            <td width="100%"><asp:Label ID="lblCluster" runat="server" /></td>
                                        </tr>
                                        <tr>
                                            <td nowrap>DataStore:</td>
                                            <td width="100%"><asp:Label ID="lblDataStore" runat="server" /></td>
                                        </tr>
                                        </asp:Panel>
                                        <tr>
                                            <td colspan="2"><hr size="1" noshade /></td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">Is this a new issue, or a repeat of an existing issue?</td>
                                        </tr>
                                        <tr>
                                            <td nowrap></td>
                                            <td width="100%">
                                                <asp:RadioButton ID="radNew" runat="server" Text="New Issue" GroupName="Issue" /> 
                                                <asp:RadioButton ID="radExisting" runat="server" Text="Existing Issue" GroupName="Issue" /> 
                                            </td>
                                        </tr>
                                    </table>
                                    <br />
                                    <div id="divNew" runat="server" style="display:none">
                                        <table cellpadding="5" cellspacing="3" border="0">
                                            <tr>
                                                <td colspan="2"><b>Issue :</b> What caused the problem?</td>
                                            </tr>
                                            <tr>
                                                <td colspan="2"><asp:TextBox ID="txtIssue" runat="server" TextMode="MultiLine" Rows="8" Width="600" /></td>
                                            </tr>
                                            <tr>
                                                <td colspan="2"><b>Resolution :</b> Steps taken to fix the issue</td>
                                            </tr>
                                            <tr>
                                                <td colspan="2"><asp:TextBox ID="txtResolution" runat="server" TextMode="MultiLine" Rows="8" Width="600" /></td>
                                            </tr>
                                            <tr>
                                                <td colspan="2"><b>Case Code :</b> Select the component most responsible for the issue</td>
                                            </tr>
                                            <tr>
                                                <td colspan="2"><asp:DropDownList ID="ddlCode" runat="server" CssClass="default" Width="300" /></td>
                                            </tr>
                                            <tr>
                                                <td colspan="2"><b>Attachment :</b> (Optional)</td>
                                            </tr>
                                            <tr>
                                                <td colspan="2"><asp:FileUpload ID="txtFile" runat="server" CssClass="default" Width="600" Height="18" /></td>
                                            </tr>
                                            <tr>
                                                <td colspan="2"><asp:Button ID="btnFixed" runat="server" CssClass="default" Width="175" Text="Fixed, Continue the Build" OnClick="btnFixed_Click" /></td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div id="divExisting" runat="server" style="display:none">
                                        <table width="100%" cellpadding="4" cellspacing="3" border="0">
                                            <tr>
                                                <td>
                                                    <%=strMenuTab1 %>
                                                    <div id="divMenu1">
                                                        <br />
                                                        <asp:Repeater ID="rptRelated" runat="server">
                                                            <ItemTemplate>
                                                                <div style="display:none">
                                                                    <table width="100%" cellpadding="2" cellspacing="0" border="0" class="default">
                                                                        <tr>
                                                                            <td colspan="2">
                                                                                <table width="100%" cellpadding="4" cellspacing="3" border="0" style="border:solid 1px #CCCCCC">
                                                                                    <tr bgcolor="#EEEEEE">
                                                                                        <td class="bold">ISSUE - What caused the problem</td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td><%# oFunction.FormatText(DataBinder.Eval(Container.DataItem, "problem").ToString())%></td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="2">
                                                                                <table width="100%" cellpadding="4" cellspacing="3" border="0" style="border:solid 1px #CCCCCC">
                                                                                    <tr bgcolor="#EEEEEE">
                                                                                        <td class="bold">RESOLUTION - Steps taken to fix the issue</td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td><%# oFunction.FormatText(DataBinder.Eval(Container.DataItem, "resolution").ToString())%></td>
                                                                                    </tr>
                                                                                </table>
                                                                                <asp:Panel ID="panAttach" runat="server" Visible="false">
                                                                                    <table width="100%" cellpadding="3" cellspacing="2" border="0">
                                                                                        <tr>
                                                                                            <td><span style="width:100%;border-bottom:1 dotted #CCCCCC;"/></td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td><img src="/images/file.gif" border="0" align="absmiddle"/> <asp:Label ID="lblAttach" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem,"path") %>' /></td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </asp:Panel>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="2" align="right">Created by <b><%# oUser.GetFullName(Int32.Parse(DataBinder.Eval(Container.DataItem, "userid").ToString()))%></b> on <%# DataBinder.Eval(Container.DataItem,"modified") %></td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td><asp:Button ID="btnSelect" runat="server" CssClass="default" Width="175" Text="This Solution Fixed the Error" OnClick="btnSelect_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"id") %>' /></td>
                                                                            <td align="right"></td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr id="trNone" runat="server" visible="false">
                                                <td><img src='/images/alert.gif' border='0' align='absmiddle'> There are no related errors ... </td>
                                            </tr>
                                        </table>
                                    </div>

            </asp:Panel>
            <asp:Panel ID="panDenied" runat="server" Visible="false">
                <table width="100%" cellpadding="2" cellspacing="2" border="0">
                    <tr>
                        <td><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                        <td class="header"><img src="/images/bigX.gif" border="0" align="absmiddle" /> There was a problem retrieving the information</td>
                    </tr>
                    <tr><td colspan="2">&nbsp;</td></tr>
                    <tr>
                        <td><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                        <td><asp:Label ID="lblError" runat="server" /></td>
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
                                    <td align="right"><asp:Button ID="btnClose" runat="server" CssClass="default" Width="75" Text="Close" /></td>
                                </tr>
                            </table>
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
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
<asp:Label ID="lblAsset" runat="server" Visible="false" />
<asp:Label ID="lblStep" runat="server" Visible="false" />
</asp:Content>
