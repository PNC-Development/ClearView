<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserProfileUpdate.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.UserProfileUpdate" %>

<table class="fullWidth" style="padding:2px 2px 2px 2px;">
    <tr>
        <td colspan="4">
            <table class="fullWidth">
                <tr>
                    <td rowspan="2" class="center"><img src="/images/profile.gif" alt="Profile Icon" /></td>
                    <td class="header fullWidth bottom">Update Information</td>
                </tr>
                <tr>
                    <td class="fullWidth top">To keep ClearView up to date, we recommend updating your user information every three (3) months. Please verify the following information and click <b>Update</b> to save your information.</td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td colspan="4">&nbsp;</td>
    </tr>
    <tr> 
        <td style="padding-left:10px;">Username:</td>
        <td><asp:textbox ID="txtUser" runat="server" Width="150" MaxLength="30"/></td>
        <td class="right top" rowspan="5">
            <table>
                <tr>
                    <td class="center"><asp:Image ID="imgPicture" runat="server" Width="90" Height="90" /></td>
                </tr>
                <tr>
                    <td class="center" style="padding-top:5px;"><asp:LinkButton ID="btnPicture" runat="server" Text="Change Photo" /></td>
                </tr>
            </table>
        </td>
        <td class="right" rowspan="4"><img src="/images/spacer.gif" style="border:none;width:10px;height:1px;" /></td>
    </tr>
    <tr id="rowPNC" runat="server" visible="false"> 
        <td style="padding-left:10px;">PNC ID:</td>
        <td><asp:textbox ID="txtPNC" runat="server" Width="150" MaxLength="30"/></td>
    </tr>
    <tr> 
        <td style="padding-left:10px;">First name:</td>
        <td><asp:textbox ID="txtFirst" runat="server" Width="200" MaxLength="100"/></td>
    </tr>
    <tr> 
        <td style="padding-left:10px;">Last name:</td>
        <td><asp:textbox ID="txtLast" runat="server" Width="200" MaxLength="100"/></td>
    </tr>
    <tr id="rowManager" runat="server" visible="false"> 
        <td style="padding-left:10px;">Manager:</td>
        <td>
            <table>
                <tr>
                    <td><asp:TextBox ID="txtManager" runat="server" Width="300" /></td>
                </tr>
                <tr>
                    <td>
                        <div id="divAJAX" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                            <asp:ListBox ID="lstAJAX" runat="server" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="footer"><b>NOTE:</b> To select your manager, start typing the LAN ID, PID, last name or first name of your manger. A list will appear from which you will need to select the name of your manager.</td>
                </tr>
                <tr>
                    <td class="footer"><b>NOTE:</b> If you do not see your manager in the list, please use the SUPPORT button (located in the top right of this page) to submit a new support ticket.</td>
                </tr>
            </table>
            <input type="hidden" id="hdnAJAXValue" name="hdnAJAXValue" />
        </td>
    </tr>
    <tr> 
        <td style="padding-left:10px;">Pager:</td>
        <td colspan="3"><asp:textbox ID="txtPagers" runat="server" Width="100" MaxLength="30"/> @ <asp:dropdownlist ID="ddlUserAt" runat="server"/></td>
    </tr>
    <tr> 
        <td style="padding-left:10px;">Phone:</td>
        <td colspan="3"><asp:textbox ID="txtPhone" runat="server" Width="100" MaxLength="15"/></td>
    </tr>
    <tr> 
        <td class="top" style="padding-left:10px;">Special Skills:</td>
        <td colspan="3"><asp:textbox ID="txtSkills" runat="server" Width="400" Rows="10" TextMode="MultiLine"/></td>
    </tr>
    <tr> 
        <td>&nbsp;</td>
        <td colspan="3">
            <asp:button ID="btnUpdate" runat="server" Text="Update" Width="75" OnClick="btnUpdate_Click"  />&nbsp;
            <asp:button ID="btnCancel" runat="server" Text="Skip" Width="75" OnClick="btnCancel_Click" />
        </td>
    </tr>
    <tr>
        <td colspan="4">&nbsp;</td>
    </tr>
    <tr>
        <td colspan="4" class="center"><img src="/images/alert.gif" alt="Alert" /> <b>NOTE:</b> You can update your information at any time by clicking on the <b>Settings</b> module within ClearView.</td>
    </tr>
    <tr>
        <td>&nbsp;</td> 
        <td colspan="3">
            <asp:Label ID="lblType" runat="server" CssClass="cmerror" />
            <asp:HiddenField ID="hdnManager" runat="server" />
        </td>
    </tr>
</table>