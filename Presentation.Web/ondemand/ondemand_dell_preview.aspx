<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ondemand_dell_preview.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.ondemand_dell_preview" %>

<html>
<head>
<title>Loading...</title>
<link href="/css/default.css" type="text/css" rel="stylesheet" />
<script type="text/javascript">
    var urlDRAC = "https://<%=strILO %>";
    var urlDRAClogin = urlDRAC + "/data/login";
    var oActiveXByPass;
    function LoadPage() {
		var strUrl = window.location.toString();
		if (strUrl.indexOf("bypass=") > -1)
		    LoadPreview();
		else
		    LoadSecurity();
    }
    
    
    function LoadSecurity() {
        alert('**** DELL VIRTUAL CONSOLE PREVIEW WINDOW ****\n\nIn order to preview the DELL virtual console, you must accept all of the following security warnings\n\nPlease click YES to the following messages to continue');
        oActiveXByPass = new ActiveXObject("Microsoft.XMLHTTP");
        oActiveXByPass.onreadystatechange = LoadSecurity_callback;
        oActiveXByPass.open("GET", urlDRAClogin, false);
        oActiveXByPass.send();
    }
    function LoadSecurity_callback() {
        if (oActiveXByPass.readyState == 4)
        {
            if (oActiveXByPass.status == 200) {
                //alert('done with security');
                window.navigate(window.location.toString() + "&bypass=true");
            }
            else 
                alert('There was a problem getting the information');
        }
    }
    
    function LoadPreview() {
        var oPass = GetPassword('<%=strPass %>');
        //var postData = 'user=' + specialEncode('<%=strUser %>') + '&password=' + specialEncode('<%=strPass %>');
        var postData = 'user=' + specialEncode('<%=strUser %>') + '&password=' + specialEncode(oPass);
        sendPost(urlDRAClogin, postData, loginRequestChange, urlDRAC);
    }
</script>
<script type="text/javascript" src="/javascript/dell.js"></script>
<script type="text/javascript" src="/javascript/both.js"></script>
</head>
<body leftmargin="0" topmargin="0" onload="LoadPage();">
<form id="Form1" runat="server">
<table width="100%" height="100%" cellpadding="0" cellspacing="0" border="0" bgcolor="#000000">
    <tr>
        <td>
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td align="center" class="whiteheader"><p>Preview Loading</p></td>
                </tr>
                <tr><td>&nbsp;</td></tr>
                <tr><td>&nbsp;</td></tr>
                <tr>
                    <td align="center" class="header"><img src="/images/wait.gif" border="0"/></td>
                </tr>
                <tr><td>&nbsp;</td></tr>
                <tr><td>&nbsp;</td></tr>
                <tr>
                    <td align="center" class="whitedefault">This may take a minute ...</td>
                </tr>
                <tr><td>&nbsp;</td></tr>
                <tr><td>&nbsp;</td></tr>
            </table>
        </td>
    </tr>
</table>
</form>
</body>
</html>