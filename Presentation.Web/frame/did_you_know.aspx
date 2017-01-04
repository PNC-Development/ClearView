<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="did_you_know.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.did_you_know" %>

<html>
<head>
<title>Did You Know</title>
<link rel="stylesheet" type="text/css" href="/css/default.css" />
<script src="/javascript/default.js"type="text/javascript"></script>
</head>
<body leftmargin="0" topmargin="0">
<form id="Form1" runat="server">
    <table width="300" height="135" cellpadding="0" cellspacing="0" border="0">
        <tr height="1">
            <td><img src="/images/table_topleft.gif" border="0" width="5" height="26"></td>
            <td nowrap background="/images/table_top.gif" class="greentableheader" width="100%">Did You Know?</td>
            <td nowrap background="/images/table_top.gif"><asp:Label ID="lblTip" runat="server" CssClass="default" />&nbsp;&nbsp;[<asp:LinkButton ID="btnNext" runat="server" Text="Next" OnClick="btnNext_Click" />]&nbsp;</td>
            <td><img src="/images/table_topright.gif" border="0" width="5" height="26"></td>
        </tr>
        <tr>
            <td background="/images/table_left.gif"><img src="/images/table_left.gif" width="5" height="10"></td>
            <td width="100%" height="100%" bgcolor="#FFFFFF" colspan="2">
                <div id="divKnow" style="padding:2; width:100%; height:100%; overflow:auto"><%=strKnow%></div>
            </td>
            <td background="/images/table_right.gif"><img src="/images/table_right.gif" width="5" height="10"></td>
        </tr>
        <tr height="1">
            <td><img src="/images/table_bottomLeft.gif" border="0" width="5" height="9"></td>
            <td colspan="2" width="100%" background="/images/table_bottom.gif"></td>
            <td><img src="/images/table_bottomRight.gif" border="0" width="5" height="9"></td>
        </tr>
    </table>
</form>
</body>
</html>