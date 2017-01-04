<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="reset_execution.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.reset_execution" %>



<script type="text/javascript">
</script>
<html>
<head>
<title>Web Content Management Administration</title>
<link rel="stylesheet" type="text/css" href="/css/default.css" />
<script type="text/javascript" src="/javascript/global.js"></script>
</head>
<body topmargin="0" leftmargin="0">
<form id="Form1" runat="server">
        <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
        <td height="100%" valign="top">
<table width="98%" cellpadding="0" cellspacing="0" border="0" align="center">
		<tr><td colspan="2">&nbsp;</td></tr>
		<tr> 
		    <td colspan="2"><b>Reset OnDemand Execution</b></td>
		</tr>
		<tr>
		    <td colspan="2">&nbsp;</td>
		</tr>
		<tr>
		    <td colspan="2"><img src="/images/bigAlert.gif" border="0" align="absmiddle" /><b>WARNING:</b> Make sure you know what you are doing before you click anything...this action cannot be undone!</td>
		</tr>
		<tr>
		    <td colspan="2">&nbsp;</td>
		</tr>
		<tr>
		    <td nowrap>Design Builder Number:</td>
		    <td width="100%"><asp:TextBox ID="txtID" runat="server" CssClass="default" Width="100" MaxLength="10" /></td>
		</tr>
		<tr>
		    <td colspan="2">
		        <table cellpadding="2" cellspacing="1" border="0">
		            <tr>
		                <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
		                <td></td>
		            </tr>
		            <tr>
		                <td colspan="2"><asp:CheckBox ID="chkTimes" runat="server" CssClass="bold" Text=" Reset Provisioning Times" /></td>
		            </tr>
		            <tr>
		                <td></td>
		                <td>New Date: <asp:TextBox ID="txtTimes" runat="server" CssClass="default" Width="150" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;(Example:&nbsp;&nbsp;&nbsp;<%=DateTime.Now.ToShortDateString() %> <b>-- OR --</b> <%=DateTime.Now.ToString() %>)</td>
		            </tr>
		            <tr>
		                <td></td>
		                <td>
		                    <table cellpadding="2" cellspacing="1" border="0">
		                        <tr>
		                            <td>Server:</td>
		                            <td>
		                                <asp:CheckBox ID="chkTimesServerStart" runat="server" Text="Started" /> 
		                                <asp:CheckBox ID="chkTimesServerComplete" runat="server" Text="Completed" /> 
		                                <asp:CheckBox ID="chkTimesServerReady" runat="server" Text="Build Ready" /> 
		                                <asp:CheckBox ID="chkTimesServerSteps" runat="server" Text="Provisioning Steps (Select Asset, Assign IP, etc...)" />
		                            </td>
		                        </tr>
		                        <tr>
		                            <td>Forecast / Design:</td>
		                            <td>
		                                <asp:CheckBox ID="chkTimesForecastExecuted" runat="server" Text="Executed" /> 
		                                <asp:CheckBox ID="chkTimesForecastCompleted" runat="server" Text="Completed" /> 
		                            </td>
		                        </tr>
		                        <tr>
		                            <td>Task / Request:</td>
		                            <td>
		                                <asp:CheckBox ID="chkTimesTasksStart" runat="server" Text="Started" /> 
		                                <asp:CheckBox ID="chkTimesTasksCompleted" runat="server" Text="Completed (WARNING: will mark complete if task is still open)" /> 
		                            </td>
		                        </tr>
		                    </table>
		                </td>
		            </tr>
		            <tr>
		                <td colspan="2">&nbsp;</td>
		            </tr>
		            <tr>
		                <td colspan="2"><asp:CheckBox ID="chkExitAP" runat="server" CssClass="bold" Text=" Exit out of OnDemand Auto-Provisioning" /></td>
		            </tr>
		            <tr>
		                <td></td>
		                <td> <b>NOTE:</b> Use this option (and this option alone) for resetting the new design builder.</td>
		            </tr>
		            <tr>
		                <td></td>
		                <td><asp:CheckBox ID="chkExitAP_Submissions" runat="server" CssClass="default" /> - Reset clients submission (new design builder ONLY)</td>
		            </tr>
		            <tr>
		                <td></td>
		                <td><asp:CheckBox ID="chkExitAP_Approvals" runat="server" CssClass="default" /> - Reset approvals (new design builder ONLY)</td>
		            </tr>
		            <tr>
		                <td></td>
		                <td><asp:CheckBox ID="chkExitAP_Name" runat="server" CssClass="default" /> - Release server name</td>
		            </tr>
		            <tr>
		                <td></td>
		                <td><asp:CheckBox ID="chkExitAP_IP" runat="server" CssClass="default" /> - Release IP Address</td>
		            </tr>
		            <tr>
		                <td></td>
		                <td><asp:CheckBox ID="chkExitAP_Asset" runat="server" CssClass="default" /> - Release Asset</td>
		            </tr>
		            <tr>
		                <td colspan="2">&nbsp;</td>
		            </tr>
		            <tr>
		                <td colspan="2" class="box_red"><img src="/images/bigError2.gif" border="0" align="absmiddle" /> <b>NOTE:</b> Only use the following options for the LEGACY DESIGN BUILDER!!!</td>
		            </tr>
		            <tr>
		                <td colspan="2"><asp:CheckBox ID="chkConfig" runat="server" CssClass="bold" Text=" Reset Configuration" /></td>
		            </tr>
		            <tr>
		                <td></td>
		                <td> - Returns to the device configuration page</td>
		            </tr>
		            <tr>
		                <td></td>
		                <td><asp:CheckBox ID="chkConfig_Reset" runat="server" CssClass="default" /> - Reset App Code / App Name / App Contacts</td>
		            </tr>
		            <tr>
		                <td></td>
		                <td><asp:CheckBox ID="chkConfig_Storage" runat="server" CssClass="default" /> - Delete Storage Configuration</td>
		            </tr>
		            <tr>
		                <td></td>
		                <td><asp:CheckBox ID="chkConfig_Device" runat="server" CssClass="default" /> - Delete Device Configuration</td>
		            </tr>
		            <tr>
		                <td colspan="2">&nbsp;</td>
		            </tr>
		            <tr>
		                <td colspan="2"><asp:CheckBox ID="chkUnlock" runat="server" CssClass="bold" Text=" Reset to Design Builder (Complete Unlock)" /></td>
		            </tr>
		            <tr>
		                <td></td>
		                <td> - Returns to the design builder page</td>
		            </tr>
		            <tr>
		                <td></td>
		                <td><asp:CheckBox ID="chkUnlock_80" runat="server" CssClass="default" /> - Reset to 80% Confidence Level</td>
		            </tr>
		        </table>
		    </td>
		</tr>
		<tr>
		    <td colspan="2">&nbsp;</td>
		</tr>
		<tr>
		    <td colspan="2"><asp:Button ID="btnGo" runat="server" CssClass="default" Width="75" OnClick="btnGo_Click" Text="Go" /></td>
		</tr>
		<tr>
		    <td colspan="2">&nbsp;</td>
		</tr>
		<tr>
		    <td colspan="2"><asp:Label ID="lblDone" runat="server" CssClass="default" /></td>
		</tr>
    </table>
        </td>
        </tr>
        </table>
</form>
</body>
</html>
