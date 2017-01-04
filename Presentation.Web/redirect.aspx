<%@ Page Language="C#" MasterPageFile="~/clearview.Master" AutoEventWireup="true" CodeBehind="redirect.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.redirect" Title="Untitled Page" %>
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
                    <td colspan="2" align="right" class="whitedefault"><DIV id=thinOrangeBar><asp:label ID="lblName" runat="server" CssClass="whitedefault" />&nbsp;&nbsp;</DIV></td>
                </tr>
            </table>
        </td>
    </tr>
    <tr bgcolor="#000000" height="1"> 
        <td colspan="2" height="26" background="/images/button_back.gif" colspan="5"><asp:PlaceHolder ID="PH1" runat="server" /></td>
    </tr>
    <tr> 
        <td colspan="2" bgcolor="E9E9E9">
            <table width="98%" height="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                <tr height="1">
                    <td align="left" valign="top">&nbsp;</td>
                </tr>
                <tr> 
                    <td align="left" valign="top">
                        <asp:PlaceHolder ID="PH3" runat="server" />
                        <p>&nbsp;</p>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
</asp:Content>