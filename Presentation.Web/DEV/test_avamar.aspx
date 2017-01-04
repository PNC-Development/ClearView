<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="test_avamar.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.test_avamar" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
<link rel="stylesheet" type="text/css" href="/css/default.css" />
</head>
<body>
    <form id="form1" runat="server">
        <p><asp:Button ID="btnWebService" runat="server" Text="Web Service" OnClick="btnWebService_Click" /></p>
        <p><asp:Button ID="btnRegister" runat="server" Text="Automated" OnClick="btnRegister_Click" /></p>
        <p><asp:Button ID="btnDecom" runat="server" Text="Decom" OnClick="btnDecom_Click" /></p>
        <p><asp:Button ID="btnRecom" runat="server" Text="Recom" OnClick="btnRecom_Click" /></p>
        <p><asp:Button ID="btnCSV" runat="server" Text="CSV" OnClick="btnCSV_Click" /></p>
        <p><asp:Button ID="btnActivated" runat="server" Text="Activated" OnClick="btnActivated_Click" /></p>
    </form>
</body>
</html>
