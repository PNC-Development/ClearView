<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="powershell.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.DEV.powershell" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <p><asp:Button ID="btnPSEXEC" runat="server" CssClass="default" Width="150" Text="PowerShell PSEXEC" OnClick="btnPSEXEC_Click" /></p>
        <p><asp:Button ID="btnDLL" runat="server" CssClass="default" Width="150" Text="PowerShell DLL" OnClick="btnDLL_Click" /></p>
    </div>
    </form>
</body>
</html>
