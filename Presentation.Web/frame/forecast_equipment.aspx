<%@ Page Language="C#" MasterPageFile="~/window.Master" AutoEventWireup="true" CodeBehind="forecast_equipment.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.forecast_equipment" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" runat="server">
    <script type="text/javascript">
    function ShowUnlockCode() {
        var divShow = document.getElementById('divShow');
        var divHide = document.getElementById('divHide');
        divShow.style.display = "inline";
        divHide.style.display = "none";
    }
    function CheckRefresh(strFore, strProj) {
        var strParent = window.opener.location.toString();
        if (strParent.indexOf("?pid=") > -1)
        {
            if (window.opener != null)
                window.opener.navigate(strProj + "&div=S");
        }
        else {
            if (window.opener != null)
                window.opener.navigate(strFore);
        }
    }
    </script>
<asp:Panel ID="panDenied" runat="server" Visible="false">
    <table width="100%" height="100%" cellpadding="4" cellspacing="2" border="0">
    <tr>
        <td align="center" bgcolor="#CCCCCC">
    <table cellpadding="5" cellspacing="5" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
        <tr>
            <td rowspan="5" valign="top"><img src="/images/hugeAlert.gif" border="0" align="absmiddle" /></td>
            <td class="header" valign="bottom">Access Denied</td>
        </tr>
        <tr>
            <td valign="top">You do not have access to the design builder module.</td>
        </tr>
        <tr>
            <td valign="top">To see that status of this design, you must do the following...</td>
        </tr>
        <tr>
            <td valign="top">
                <ul>
                    <li>Click &quot;Datapoint&quot; from the main navigation menu.</li><br /><br />
                    <li>Select &quot;Service Search&quot;.</li><br /><br />
                    <li>Change the &quot;Search Type&quot; to Design.</li><br /><br />
                    <li>Enter your Design ID and click &quot;Search&quot;.</li>
                </ul>
            </td>
        </tr>
        <tr>
            <td valign="top">For your convenience, a direct link is provided by <asp:HyperLink ID="hypDesign" runat="server" CssClass="default" Text="clicking here" />.</td>
        </tr>
    </table>
        </td>
    </tr>
</asp:Panel>
<asp:Panel ID="panAllow" runat="server" Visible="false">
    <asp:Panel ID="panStep" runat="server" Visible="false">
        <table width="100%" cellpadding="4" cellspacing="2" border="0">
            <tr>
                <td colspan="2">
                    <table width="100%" cellpadding="0" cellspacing="5" border="0">
                        <tr>
                            <td rowspan="2"><asp:Image ID="imgStep" runat="Server" ImageAlign="AbsMiddle" /></td>
                            <td class="header" width="100%" valign="bottom"><asp:Label ID="lblTitle" runat="server" CssClass="header" /></td>
                        </tr>
                        <tr>
                            <td width="100%" valign="top"><asp:Label ID="lblSubTitle" runat="server" CssClass="default" /></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2"><asp:PlaceHolder ID="PHStep" runat="server" /></td>
            </tr>
            <asp:Panel ID="panOverride" runat="server" Visible="false">
            <tr>
                <td colspan="2">&nbsp;</td>
            </tr>
            <tr>
                <td colspan="2" class="bold" align="right"><img src="/images/alert.gif" border="0" align="absmiddle" /> Selection Matrix Override Mode</td>
            </tr>
            </asp:Panel>
        </table>
    </asp:Panel>
    <asp:Panel ID="panSteps" runat="server" Visible="false">
        <table width="100%" cellpadding="0" cellspacing="5" border="0">
            <tr>
                <td rowspan="2"><img src="/images/forecast2.gif" border="0" align="absmiddle" /></td>
                <td class="header" width="100%" valign="bottom">Design Builder</td>
            </tr>
            <tr>
                <td width="100%" valign="top">You have finished building the design. If you wish to change this design, select the section from the list below.</td>
            </tr>
        </table>
        <br />
        <%=strSteps %>
        <a id="Unlock"></a>
        <table width="100%" cellpadding="0" cellspacing="5" border="0">
            <tr>
                <td align="center">
                    <asp:Panel ID="panHundred" runat="server" Visible="false">
                    <table cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
                        <tr>
                            <td align="center" class="redheader"><img src="/images/bigAlert.gif" border="0" align="absmiddle" /> You cannot edit a design that has a confidence level of 100%.</td>
                        </tr>
                        <tr id="divHide">
                            <td align="center"><img src="/images/green_right.gif" border="0" align="absmiddle" /> <a href="#Unlock" onclick="ShowUnlockCode();">Click Here to Unlock this Design</a></td>
                        </tr>
                        <tr>
                            <td>
                                <div id="divShow" style="display:none">
                                    <table cellpadding="4" cellspacing="0" border="0">
                                        <tr>
                                            <td nowrap>Workstation IP:</td>
                                            <td><asp:Label ID="lblConfidenceUnlock" runat="server" CssClass="default" /></td>
                                        </tr>
                                        <tr>
                                            <td nowrap>Unlock Code:</td>
                                            <td><asp:TextBox ID="txtConfidenceUnlock" runat="server" CssClass="default" Width="400" /></td>
                                        </tr>
                                        <tr>
                                            <td nowrap valign="top">Reason:</td>
                                            <td><asp:TextBox ID="txtConfidenceReason" runat="server" CssClass="default" Width="400" TextMode="MultiLine" Rows="5" /></td>
                                        </tr>
                                        <tr>
                                            <td nowrap>&nbsp;</td>
                                            <td><asp:Button ID="btnConfidenceUnlock" runat="server" CssClass="default" Width="100" Text="Unlock" OnClick="btnConfidenceUnlock_Click" /></td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" class="reddefault"><b>NOTE:</b> Unlocking a design will automatically change the confidence level from 100% to 80%.</td>
                        </tr>
                    </table>
                    </asp:Panel>
                    <asp:Panel ID="panLocked" runat="server" Visible="false">
                    <table cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
                        <tr>
                            <td align="center" class="redheader"><img src="/images/lock.gif" border="0" align="absmiddle" /> You cannot edit a design that is currently being executed.</td>
                        </tr>
                        <tr>
                            <td>This design is currently being executed by ClearView and is not available for modifications.</td>
                        </tr>
                        <tr>
                            <td>If this design needs to be corrected, please contact your ClearView administrator <b>immediately</b>.</td>
                        </tr>
                        <tr>
                            <td>You will need to reference design builder number <b><%=intID %></b> to unlock this item.</td>
                        </tr>
                    </table>
                    </asp:Panel>
                </td>
            </tr>
        </table>
        <br />        
        <table width="100%" cellpadding="4" cellspacing="2" border="0">
            <tr>
                <td><hr size="1" noshade /></td>
            </tr>
            <tr>
                <td align="right"><asp:Button ID="btnClose" runat="server" Text="Close" CssClass="default" Width="75" /></td>
            </tr>
        </table>
    </asp:Panel>
</asp:Panel>
</asp:Content>
