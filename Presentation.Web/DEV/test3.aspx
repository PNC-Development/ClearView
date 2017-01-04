<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="test3.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.test3" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
<link rel="stylesheet" type="text/css" href="/css/default.css" />
<script src="/javascript/default.js"type="text/javascript"></script>
<script type="text/javascript" src="/javascript/both.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <p><asp:Button ID="btn1" runat="server" Text="Step 1" OnClick="btn1_Click" /></p>
        <p><asp:Button ID="btn2" runat="server" Text="Step 2" OnClick="btn2_Click" /></p>
        <p><asp:Button ID="btn3" runat="server" Text="Step 3" OnClick="btn3_Click" /></p>
        <p><asp:Button ID="btn4" runat="server" Text="Step 4" OnClick="btn4_Click" /></p>
    Done.
    </div>
    </form>
</body>
</html>
