<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="error_message.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.error_message" %>

<html>
<head>
<title>Error Message</title>
<link href="/css/default.css" type="text/css" rel="stylesheet" />
<script src="/javascript/default.js"type="text/javascript"></script>
</head>
<body leftmargin="0" topmargin="0">
<form id="Form1" runat="server">
<div align="center">
<p>&nbsp;</p>
<table width="700" cellpadding="0" cellspacing="0" border="0">
    <tr>
        <td valign="top"><img src="/images/oops.jpg" border="0" align="absmiddle" /></td>
        <td valign="top">
            <table id="divDear" width="100%" border="0" cellspacing="5" cellpadding="5" style="font-size: 11px;">
                <tr>
                    <td class="hugeheader" align="center"> *********&nbsp;&nbsp;&nbsp; Please Read this Message &nbsp;&nbsp;&nbsp********* </td>
                </tr>
                <tr>
                    <td class="redheader">Your automated build has encountered an unexpected problem.</td>
                </tr>
                <tr>
                    <td>
                        While attempting to process your automated build, ClearView found that at least one infrastructure component is not working properly or is currently not available.
                    </td>
                </tr>
                <tr>
                    <td class="bold">What can I do to help?</td>
                </tr>
                <tr>
                    <td>
                        Nothing! ClearView has already engaged the appropriate support team to address the problem. 
                        <span id="panIncident" runat="server" visible="false">
                            You can follow the problem by referencing <asp:Label ID="lblIncident2" runat="server" /> and the assigned owner: <asp:Label ID="lblIncidentOwner" runat="server" />.
                        </span>
                        <div style="display:none">
                            Opening tickets and escalating to management will not resolve the issues any faster than they are already being resolved. 
                            In fact, your involvement significantly reduces our ability to address issues in a timely fashion.
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="bold">Some good advice:</td>
                </tr>
                <tr>
                    <td>As a general rule try to encourage your project to always plan 10 business days ahead. The 10 day business day rule is to compensate for unexpected infrastructure related issues like this one that may interfere with the automated build process such as hardware issues or a downed switch. This allows ample time for infrastructure support teams to resolve the issue. Although our average is 1 - 2 days, it is best to plan for unexpected delays.</td>
                </tr>
                <tr>
                    <td>Thanks,<br />ClearView Administrator</td>
                </tr>
                <tr>
                    <td>To learn more about this process, please <asp:HyperLink id="hypWiki" runat="server" Target="_blank">visit our Community</asp:HyperLink>.</td>
                </tr>
                <tr id="trAdmin" runat="server" visible="false">
                    <td>
                        <a href="javascript:void(0);" onclick="ShowHideDivs('divDetail','divDear');"><img src="/images/plus.gif" border="0" align="absmiddle" /> View Detail</a>
                        &nbsp;&nbsp;
                        <a href="javascript:void(0);" onclick="if (window.opener == null) { parent.HidePanel(); } else { window.close(); }"><img src='/images/close-icon.gif' border='0' align='absmiddle' /><img src='/images/spacer.gif' border='0' width='5' height='1' />Close Window</a>
                    </td>
                </tr>
            </table>
            <table id="divDetail" style="display:none" width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td class="header">Error Message Detail:</td>
                </tr>
                <tr><td>&nbsp;</td></tr>
                <tr>
                    <td><%=strError %></td>
                </tr>
                <tr><td>&nbsp;</td></tr>
                <tr>
                    <td><a href="javascript:void(0);" onclick="ShowHideDivs('divDetail','divDear');"><img src="/images/minus.gif" border="0" align="absmiddle" /> Hide Detail</a></td>
                </tr>
            </table>
        </td>
    </tr>
</table>
</div>
</form>
</body>
</html>