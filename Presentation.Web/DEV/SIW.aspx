<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SIW.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.DEV.SIW" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>SIW Export</title>
    <link rel="stylesheet" type="text/css" href="/css/default.css" />
    <style type="text/css">
        ul { list-style-type: none; }
        form { padding: 10px; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <p class="hugeheader">ClearView to SIW Export Feed</p>
        <p class="header">Valid QueryString Values (?type=xxx):</p>
        <p>
            <ul>
                <li><a href="?type=XML">?type=XML</a> : XML formatted</li>
                <li><a href="?type=CSV">?type=CSV</a> : Comma delimited</li>
                <li><a href="?type=TAB">?type=TAB</a> : Tab delimited</li>
                <li>&amp;output=file : (optional) Generates a file</li>
            </ul>
        </p>
        <p>Example: <a href='<%=Request.Path %>?type=CSV&output=file'><%=Request.Url.Scheme + "://" + Request.Url.Host + (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port) + Request.Path%>?type=CSV&amp;output=file</a></p>
        <p class="header">Column Names</p>
        <p>
            <ul>
                <asp:Literal ID="litColumns" runat="server" />
            </ul>
        </p>
    </form>
</body>
</html>
