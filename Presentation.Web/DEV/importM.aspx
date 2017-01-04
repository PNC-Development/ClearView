<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="importM.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.DEV.importM" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>IMPORT MNEMONICS</title>
    <link rel="stylesheet" type="text/css" href="/css/default.css" />
</head>
<body>
    <form id="form1" runat="server">
    <table>
        <tr>
            <td colspan="2" class="header">Import</td>
        </tr>
        <tr>
            <td colspan="2"><asp:CheckBox ID="chkOne" runat="server" Text="Only One" /></td>
        </tr>
        <tr>
            <td colspan="2"><asp:CheckBox ID="chkUpdate" runat="server" Text="Update Contacts" /></td>
        </tr>
        <tr>
            <td colspan="2">&nbsp;</td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td><asp:Button ID="btnGo" runat="server" CssClass="default" Width="150" Text="Import Mnemonics" OnClick="btnGo_Click" /></td>
        </tr>
    </table>
    </form>
</body>
</html>
