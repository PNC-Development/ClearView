<%@ Page Language="C#" MasterPageFile="~/frame.Master" AutoEventWireup="true" Codebehind="index.aspx.cs"
    Inherits="NCC.ClearView.Presentation.Web.datapointIndex" %>

<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" runat="Server">

    <script type="text/javascript">
    </script>

    <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0">
        <tr height="1">
            <td bgcolor="#007253">
                <img src="/images/header_wide.aspx?t=Welcome to DataPoint" border="0" /></td>
            <td bgcolor="#007253" align="right">
                <asp:PlaceHolder ID="PH4" runat="server" />
            </td>
        </tr>
        <tr bgcolor="#000000" height="1">
            <td colspan="2" height="26" background="/images/button_back.gif">
                <table width="100%" cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td>
                            <asp:PlaceHolder ID="PH1" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2" bgcolor="#E9E9E9">
                <table width="98%" height="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                    <tr height="1">
                        <td align="left" valign="top">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td align="left" valign="top">
                            <asp:PlaceHolder ID="PH3" runat="server" />
                            <p>
                                &nbsp;</p>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
