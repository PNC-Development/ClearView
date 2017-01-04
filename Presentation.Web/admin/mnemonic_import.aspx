<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="mnemonic_import.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.mnemonic_import" %>

<script type="text/javascript">
</script>
<html>
<head>
<title>LOAD</title>
<link rel="stylesheet" type="text/css" href="/css/default.css" />
</head>
<body leftmargin="0" topmargin="0">
<form id="Form1" runat="server">
        <table>
            <tr>
                <td colspan="2" class="header">Upload the file to the F: drive of OHCLEIIS1319 and call it <b>Mnemonic.xls</b>. Please make sure the name of the worksheet is called <b>Mnemonic</b> and that EVERYONE has read access to it.</td>
            </tr>
            <tr>
                <td colspan="2">&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td><asp:Button ID="btnLoad1" runat="server" CssClass="default" Width="150" Text="Import and Overwrite" OnClick="btnLoad1_Click" /></td>
            </tr>
        </table>
</form>
</body>
</html>