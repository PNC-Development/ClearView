<%@ Page Language="C#" MasterPageFile="~/clearview.Master" AutoEventWireup="true" CodeBehind="interior.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.interior" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" Runat="Server">
<%=strPageRefresh %>
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
        <td colspan="2" height="26" background="/images/button_back.gif">
            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td><asp:PlaceHolder ID="PH1" runat="server" /></td>
                    <td align="right"></td>
                </tr>
            </table>
        </td>
    </tr>
    <tr height="1"> 
        <td colspan="2" bgcolor="#E9E9E9"><asp:PlaceHolder ID="PHDown" runat="server" /></td>
    </tr>
    <tr> 
        <td colspan="2" bgcolor="#E9E9E9">
            <table width="98%" height="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                <tr height="1">
                    <td align="left" valign="top"><img src="/images/spacer.gif" width="180" height="1" /></td>
                    <td align="left" valign="top"><img src="/images/spacer.gif" width="12" height="1" /></td>
                    <td align="left" valign="top">&nbsp;</td>
                </tr>
                <tr> 
                    <td width="180" align="left" valign="top">
                        <asp:PlaceHolder ID="PH2" runat="server" />
                    </td>
                    <td width="12" align="left" valign="top">&nbsp;</td>
                    <td width="100%" align="left" valign="top">
                        <asp:PlaceHolder ID="PH3" runat="server" />
                        <p>&nbsp;</p>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
<script type="text/javascript">EnablePostBack('<%=strPage%>','<%=Request.Path%>');</script>
</asp:Content>