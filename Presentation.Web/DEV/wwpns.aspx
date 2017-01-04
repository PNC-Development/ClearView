<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wwpns.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.wwpns" %>

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
        <p>Serial : <asp:TextBox ID="txtSerial" runat="server" Width="100" /></p>
        <p>----- OR ------</p>
        <p>DRAC : <asp:TextBox ID="txtDRAC" runat="server" Width="100" /></p>
        <p><asp:Button ID="btn1" runat="server" Text="Step 1" OnClick="btn1_Click" /></p>
    </div>
    </form>
</body>
</html>
