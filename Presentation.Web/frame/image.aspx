<%@ Page Language="C#" MasterPageFile="~/frame.Master" AutoEventWireup="true" CodeBehind="image.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.image" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" runat="server">
<script type="text/javascript">
    function StartPic432() {
        setTimeout("WaitPic432()",1000);
    }
    function WaitPic432() {
        var oFile = document.getElementById('<%=fileUpload.ClientID %>');
        if (oFile.value != "") 
        {
            var oPic = document.getElementById('<%=imgNew.ClientID %>');
            oPic.src = oFile.value;
        }
        setTimeout("WaitPic432()",1000);
    }
</script>
    <table width="100%" height="100%" cellpadding="2" cellspacing="0" border="0">
        <tr>
            <td valign="top">
                <table width="100%" cellpadding="0" cellspacing="5" border="0">
                    <tr>
                        <td rowspan="2"><img src="/images/camera.gif" border="0" align="middle" /></td>
                        <td class="header" width="100%" valign="bottom">Upload your Picture</td>
                    </tr>
                    <tr>
                        <td width="100%" valign="top">Upload your custom picture of yourself.</td>
                    </tr>
                </table>
                <table width="100%" cellpadding="2" cellspacing="2" border="0">
                    <tr>
                        <td align="center">Current Picture:</td>
                        <td align="center"></td>
                        <td align="center">New Picture Preview:</td>
                    </tr>
                    <tr>
                        <td align="center"><asp:Image ID="imgPicture" runat="server" Width="90" Height="90" /></td>
                        <td align="center"><img src="/images/arrow_right.gif" border="0" align="absmiddle" /></td>
                        <td align="center"><asp:Image ID="imgNew" runat="server" Width="90" Height="90" /></td>
                    </tr>
                    <tr>
                        <td colspan="3"><b>NOTE:</b> Please make sure this picture is professional. All inappropriate pictures will be deleted and replaced with a generic picture and future modifications will be revoked.</td>
                    </tr>
                    <tr>
                        <td colspan="3">New Picture Path:</td>
                    </tr>
                    <tr>
                        <td colspan="3"><asp:FileUpload id="fileUpload" runat="server" CssClass="default" Width="100%" /></td>
                    </tr>
                    <tr>
                        <td colspan="3"><asp:Button ID="btnSave" runat="server" CssClass="default" Width="100" Text="Save" OnClick="btnSave_Click" /></td>
                    </tr>
                    <tr>
                        <td colspan="3"><b>NOTE:</b> After uploading your new picture, you might have to refresh the page for your picture to appear. If it still fails to appear, try to delete your temporary internet files and refresh again.</td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
