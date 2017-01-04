<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ad.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.DEV.ad" EnableSessionState="True" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:AdRotator id="adRotator" runat="server" AdvertisementFile="ads.xml" Target="_blank" KeywordFilter="banner"></asp:AdRotator>
    </div>
    </form>
</body>
</html>
