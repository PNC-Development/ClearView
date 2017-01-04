<%@ Page Language="C#" MasterPageFile="~/clearview.Master" AutoEventWireup="true" CodeBehind="updateUserProfile.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.updateUserProfile" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" Runat="Server">
<script type="text/javascript">
</script>
<table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0">
    <tr height="1"> 
        <td bgcolor="#007253"><img src="/images/header_wide.aspx" border="0" /></td>
        <td bgcolor="#007253" align="right"><asp:PlaceHolder ID="PH4" runat="server" /></td>
    </tr>
    <tr bgcolor="#000000" height="1"> 
        <td colspan="2" height="26" background="/images/button_back.gif">
            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td></td>
                    <td align="right"></td>
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
<table width="100%" cellpadding="0" cellspacing="0" border="0">
    <tr>
        <td align="center">
<table width="700" cellpadding="0" cellspacing="0" border="0">
    <tr>
        <td><img src="/images/table_topleft.gif" border="0" width="5" height="26"></td>
        <td nowrap background="/images/table_top.gif" class="greentableheader" width="100%">Please complete the following...</td>
        <td><img src="/images/table_topright.gif" border="0" width="5" height="26"></td>
    </tr>
    <tr>
        <td background="/images/table_left.gif"><img src="/images/table_left.gif" width="5" height="10"></td>
        <td width="100%" bgcolor="#FFFFFF">
            <table width="100%" cellpadding="2" cellspacing="2" border="0">
                <tr>
                    <td colspan="4">
                        <table width="100%" cellpadding="0" cellspacing="5" border="0">
                            <tr>
                                <td rowspan="2"><img src="/images/profile.gif" border="0" align="absmiddle" /></td>
                                <td class="header" width="100%" valign="bottom">Update Information</td>
                            </tr>
                            <tr>
                                <td width="100%" valign="top">To keep ClearView up to date, we recommend updating your user information every three (3) months. Please verify the following information and click <b>Update</b> to save your information.</td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">&nbsp;</td>
                </tr>
                <tr> 
                    <td class="default" width="100px">Username:</td>
                    <td><asp:textbox ID="txtUser" CssClass="default" runat="server" Width="150" MaxLength="30"/></td>
                    <td align="right" rowspan="5" valign="top">
                        <table cellpadding="0" cellspacing="5" border="0">
                            <tr>
                                <td align="center"><asp:Image ID="imgPicture" runat="server" Width="90" Height="90" /></td>
                            </tr>
                            <tr>
                                <td align="center"><asp:LinkButton ID="btnPicture" runat="server" Text="Change Photo" /></td>
                            </tr>
                        </table>
                    </td>
                    <td align="right" rowspan="4"><img src="/images/spacer.gif" border="0" width="10" height="1" /></td>
                </tr>
                <tr id="rowPNC" runat="server" visible="true"> 
                    <td class="default" width="100px">PNC ID:</td>
                    <td><asp:textbox ID="txtPNC" CssClass="default" runat="server" Width="150" MaxLength="30"/></td>
                </tr>
                <tr> 
                    <td class="default" width="100px">First name:</td>
                    <td><asp:textbox ID="txtFirst" CssClass="default" runat="server" Width="200" MaxLength="100"/></td>
                </tr>
                <tr> 
                    <td class="default" width="100px">Last name:</td>
                    <td><asp:textbox ID="txtLast" CssClass="default" runat="server" Width="200" MaxLength="100"/></td>
                </tr>
                <asp:Panel ID="panManager" runat="server" Visible="false">
                <tr> 
                    <td class="default">Manager:</td>
                    <td>
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td><asp:TextBox ID="txtManager" runat="server" Width="300" CssClass="default" /></td>
                            </tr>
                            <tr>
                            <td>
                            <div id="divAJAX" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                                <asp:ListBox ID="lstAJAX" runat="server" CssClass="default" />
                            </div>
                            </td>
                            </tr>
                        </table>
                        <input type="hidden" id="hdnAJAXValue" name="hdnAJAXValue" />
                    </td>
                 </tr>
                <tr> 
                    <td class="default"></td>
                    <td class="footer" colspan="3"><b>NOTE:</b> To select your manager, start typing the LAN ID, PID, last name or first name of your manger. A list will appear from which you will need to select the name of your manager.</td>
                </tr>
                <tr> 
                    <td class="default"></td>
                    <td class="footer" colspan="3"><b>NOTE:</b> If you do not see your manager in the list, please use the SUPPORT button (located in the top right of this page) to submit a new support ticket.</td>
                </tr>
                </asp:Panel>
                <tr> 
                    <td class="default">Pager:</td>
                    <td colspan="3"><asp:textbox ID="txtPagers" CssClass="default" runat="server" Width="100" MaxLength="30"/> @ <asp:dropdownlist ID="ddlUserAt" CssClass="default" runat="server"/></td>
                </tr>
                <tr> 
                    <td class="default">Phone:</td>
                    <td colspan="3"><asp:textbox ID="txtPhone" CssClass="default" runat="server" Width="100" MaxLength="15"/></td>
                </tr>
                <tr> 
                    <td class="default" valign="top">Special Skills:</td>
                    <td colspan="3"><asp:textbox ID="txtSkills" CssClass="default" runat="server" Width="400" Rows="10" TextMode="MultiLine"/></td>
                </tr>
                <tr> 
                    <td>&nbsp;</td>
                    <td colspan="3">
                        <asp:button ID="btnUpdate" CssClass="default" runat="server" Text="Update" Width="75" OnClick="btnUpdate_Click"  />&nbsp;
                        <asp:button ID="btnCancel" CssClass="default" runat="server" Text="Skip" Width="75" OnClick="btnCancel_Click" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4">&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="4" align="center"><img src="/images/alert.gif" border="0" align="absmiddle" /> <b>NOTE:</b> You can update your information at any time by clicking on the <b>Settings</b> module within ClearView.</td>
                </tr>
                <tr>
                    <td>&nbsp;</td> 
                    <td colspan="3">
                        <asp:Label ID="lblType" runat="server" CssClass="cmerror" />
                        <asp:HiddenField ID="hdnManager" runat="server" />
                    </td>
                </tr>
                </table>
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
        </td>
    </tr>
</table>
</asp:Content>