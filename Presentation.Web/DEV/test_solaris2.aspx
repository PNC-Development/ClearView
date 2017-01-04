<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="test_solaris2.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.test_solaris2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
<link rel="stylesheet" type="text/css" href="/css/default.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <table cellpadding="5" cellspacing="5" border="0">
        <tr>
            <td>ILOM:</td>
            <td><asp:TextBox ID="txtILOM" runat="server" Text="10.249.237.148" /></td>
        </tr>
        <tr>
            <td>Wait For:</td>
            <td><asp:TextBox ID="txtWait" runat="server" Text="sc>" /></td>
        </tr>
        <tr>
            <td>Command 1:</td>
            <td><asp:TextBox ID="txtCommand1" runat="server" Text="showsc sys_enetaddr" /></td>
        </tr>
        <tr>
            <td>Command 2:</td>
            <td><asp:TextBox ID="txtCommand2" runat="server" Text="showpower" /></td>
        </tr>
        <tr>
            <td></td>
            <td>Reset Active Sessions : "restartssh -y"</td>
        </tr>
        <tr>
            <td></td>
            <td><asp:Button ID="btnCommand" runat="server" Text="Go" OnClick="btnCommand_Click" /></td>
        </tr>
    </table>
    
    </div>
    </form>
</body>
</html>
