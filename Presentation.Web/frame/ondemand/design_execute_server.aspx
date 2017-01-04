<%@ Page Language="C#" MasterPageFile="~/window.Master" AutoEventWireup="true" CodeBehind="design_execute_server.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.design_execute_server" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" runat="server">
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
    <table width="100%" height="100%" cellpadding="4" cellspacing="2" border="0">
        <tr height="1">
            <td>
                <table width="100%" cellpadding="1" cellspacing="1" border="0">
                    <tr>
                        <td>
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
                        <td>
                            <table width="100%" cellpadding="0" cellspacing="5" border="0" style="border:solid 1px #70BA77; background-color:#EEF7EF">
                                <tr>
                                    <td nowrap class="bigger">Device Count:</td>
                                    <td nowrap width="25%" class="default"><b><asp:Label ID="lblCurrentCount" runat="server" CssClass="bigger" /> / <asp:Label ID="lblForecastCount" runat="server" CssClass="bigger" /></b></td>
                                    <td nowrap class="bigger">Recovery Count:</td>
                                    <td nowrap width="25%" class="default"><b><asp:Label ID="lblCurrentDR" runat="server" CssClass="bigger" /> / <asp:Label ID="lblForecastDR" runat="server" CssClass="bigger" /></b></td>
                                    <td nowrap class="bigger">HA Count:</td>
                                    <td nowrap width="25%" class="default"><b><asp:Label ID="lblCurrentHA" runat="server" CssClass="bigger" /> / <asp:Label ID="lblForecastHA" runat="server" CssClass="bigger" /></b></td>
                                    <td nowrap class="bigger">Storage:</td>
                                    <td nowrap width="25%" class="default"><b><asp:Label ID="lblCurrentStorage" runat="server" CssClass="bigger" /> / <asp:Label ID="lblForecastStorage" runat="server" CssClass="bigger" /></b></td>
                                    <td nowrap><asp:Button ID="btnDetails" runat="server" CssClass="default" Width="125" Text="Storage Details" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="divDetails" runat="server" style="display:none">
                        <td>
                            <table width="100%" cellpadding="0" cellspacing="5" border="0" style="border:solid 1px #EE2C2C; background-color:#F7EFEE">
                                <tr>
                                    <td nowrap width="10%" class="bigger"><b>TEST</b>&nbsp;<img src="/images/arrow_black_right.gif" border="0" align="absmiddle" /></td>
                                    <td nowrap class="bigger">High Perform:</td>
                                    <td nowrap width="30%" class="default"><b><asp:Label ID="lblCurrentStorageHighT" runat="server" CssClass="bigger" /> / <asp:Label ID="lblForecastStorageHighT" runat="server" CssClass="bigger" /></b></td>
                                    <td nowrap class="bigger">Standard Perform:</td>
                                    <td nowrap width="30%" class="default"><b><asp:Label ID="lblCurrentStorageStandardT" runat="server" CssClass="bigger" /> / <asp:Label ID="lblForecastStorageStandardT" runat="server" CssClass="bigger" /></b></td>
                                    <td nowrap class="bigger">Low Perform:</td>
                                    <td nowrap width="30%" class="default"><b><asp:Label ID="lblCurrentStorageLowT" runat="server" CssClass="bigger" /> / <asp:Label ID="lblForecastStorageLowT" runat="server" CssClass="bigger" /></b></td>
                                </tr>
                            </table>
                            <br />
                            <table width="100%" cellpadding="0" cellspacing="5" border="0" style="border:solid 1px #EE2C2C; background-color:#F7EFEE">
                                <tr>
                                    <td nowrap width="10%" class="bigger"><b>QA</b>&nbsp;<img src="/images/arrow_black_right.gif" border="0" align="absmiddle" /></td>
                                    <td nowrap class="bigger">High Perform:</td>
                                    <td nowrap width="30%" class="default"><b><asp:Label ID="lblCurrentStorageHighQ" runat="server" CssClass="bigger" /> / <asp:Label ID="lblForecastStorageHighQ" runat="server" CssClass="bigger" /></b></td>
                                    <td nowrap class="bigger">Standard Perform:</td>
                                    <td nowrap width="30%" class="default"><b><asp:Label ID="lblCurrentStorageStandardQ" runat="server" CssClass="bigger" /> / <asp:Label ID="lblForecastStorageStandardQ" runat="server" CssClass="bigger" /></b></td>
                                    <td nowrap class="bigger">Low Perform:</td>
                                    <td nowrap width="30%" class="default"><b><asp:Label ID="lblCurrentStorageLowQ" runat="server" CssClass="bigger" /> / <asp:Label ID="lblForecastStorageLowQ" runat="server" CssClass="bigger" /></b></td>
                                </tr>
                            </table>
                            <br />
                            <table width="100%" cellpadding="0" cellspacing="5" border="0" style="border:solid 1px #EE2C2C; background-color:#F7EFEE">
                                <tr>
                                    <td nowrap width="10%" class="bigger"><b>PROD</b>&nbsp;<img src="/images/arrow_black_right.gif" border="0" align="absmiddle" /></td>
                                    <td nowrap class="bigger">High Perform:</td>
                                    <td nowrap width="30%" class="default"><b><asp:Label ID="lblCurrentStorageHighP" runat="server" CssClass="bigger" /> / <asp:Label ID="lblForecastStorageHighP" runat="server" CssClass="bigger" /></b></td>
                                    <td nowrap class="bigger">Standard Perform:</td>
                                    <td nowrap width="30%" class="default"><b><asp:Label ID="lblCurrentStorageStandardP" runat="server" CssClass="bigger" /> / <asp:Label ID="lblForecastStorageStandardP" runat="server" CssClass="bigger" /></b></td>
                                    <td nowrap class="bigger">Low Perform:</td>
                                    <td nowrap width="30%" class="default"><b><asp:Label ID="lblCurrentStorageLowP" runat="server" CssClass="bigger" /> / <asp:Label ID="lblForecastStorageLowP" runat="server" CssClass="bigger" /></b></td>
                                </tr>
                            </table>
                            <br />
                            <table width="100%" cellpadding="0" cellspacing="5" border="0" style="border:solid 1px #EE2C2C; background-color:#F7EFEE">
                                <tr>
                                    <td nowrap width="10%" class="bigger"><b>HA</b>&nbsp;<img src="/images/arrow_black_right.gif" border="0" align="absmiddle" /></td>
                                    <td nowrap class="bigger">High Perform:</td>
                                    <td nowrap width="30%" class="default"><b><asp:Label ID="lblCurrentStorageHighH" runat="server" CssClass="bigger" /> / <asp:Label ID="lblForecastStorageHighH" runat="server" CssClass="bigger" /></b></td>
                                    <td nowrap class="bigger">Standard Perform:</td>
                                    <td nowrap width="30%" class="default"><b><asp:Label ID="lblCurrentStorageStandardH" runat="server" CssClass="bigger" /> / <asp:Label ID="lblForecastStorageStandardH" runat="server" CssClass="bigger" /></b></td>
                                    <td nowrap class="bigger">Low Perform:</td>
                                    <td nowrap width="30%" class="default"><b><asp:Label ID="lblCurrentStorageLowH" runat="server" CssClass="bigger" /> / <asp:Label ID="lblForecastStorageLowH" runat="server" CssClass="bigger" /></b></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <table width="100%" cellpadding="0" cellspacing="5" border="0">
                    <tr>
                        <td class="bigger"><b>Progress:</b></td>
                        <td width="90%"><%=strProgress %></td>
                    </tr>
                </table>
            </td>
        </tr>
        <asp:Panel ID="panConfidence" runat="server" Visible="false">
        <tr>
            <td>
                <table width="100%" cellpadding="0" cellspacing="5" border="0">
                    <tr>
                        <td rowspan="2"><img src="/images/ico_error.gif" border="0" align="absmiddle" /></td>
                        <td class="header" width="100%" valign="bottom">Design Confidence Not 100%</td>
                    </tr>
                    <tr>
                        <td width="100%" valign="top">You cannot execute this design because the confidence level is not set to 100%.</td>
                    </tr>
                </table>
            </td>
        </tr>
        </asp:Panel>
        <asp:Panel ID="panOverridePending" runat="server" Visible="false">
        <tr>
            <td>
                <table width="100%" cellpadding="0" cellspacing="5" border="0">
                    <tr>
                        <td rowspan="2"><img src="/images/ico_error.gif" border="0" align="absmiddle" /></td>
                        <td class="header" width="100%" valign="bottom">The Override of this Design is still Pending</td>
                    </tr>
                    <tr>
                        <td width="100%" valign="top">You cannot execute this design because the override is still under review.</td>
                    </tr>
                </table>
            </td>
        </tr>
        </asp:Panel>
        <asp:Panel ID="panOverrideReject" runat="server" Visible="false">
        <tr>
            <td>
                <table width="100%" cellpadding="0" cellspacing="5" border="0">
                    <tr>
                        <td rowspan="2"><img src="/images/ico_error.gif" border="0" align="absmiddle" /></td>
                        <td class="header" width="100%" valign="bottom">The Override of this Design was Rejected</td>
                    </tr>
                    <tr>
                        <td width="100%" valign="top">You cannot execute this design because the override was rejected by the design review board. Please close this window and choose &quot;No&quot; to the override option on the first page of the design to continue.</td>
                    </tr>
                </table>
            </td>
        </tr>
        </asp:Panel>
        <asp:Panel ID="panStep" runat="server" Visible="false">
        <tr>
            <td><asp:PlaceHolder ID="PHStep" runat="server" /></td>
        </tr>
        </asp:Panel>
        <asp:Panel ID="panBlade" runat="server" Visible="false">
        <tr>
            <td valign="top">
                <br />
                <table width="100%" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
                    <tr>
                        <td nowrap><img src="/images/bigAlert.gif" border="0" align="absmiddle" /></td>
                        <td width="100%">The provisioned model is an HP Blade. Blade technology requires at least 25 GB storage allocated for each device. Your current quantity is <asp:Label ID="lblQuantity" runat="server" CssClass="bold" /> meaning you are required to allocate at least <asp:Label ID="lblMinimum" runat="server" CssClass="bold" /> GB storage.</td>
                    </tr>
                    <tr>
                        <td nowrap></td>
                        <td width="100%">Currently, you have only allocated a total of <asp:Label ID="lblBladeStorage" runat="server" CssClass="bold" /> GB Storage.</td>
                    </tr>
                    <tr>
                        <td nowrap></td>
                        <td width="100%">Please click <b>Close</b> to return to the design builder. Then click [Edit] to fix your storage configuration.</td>
                    </tr>
                    <tr>
                        <td colspan="2">&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center"><asp:Button ID="btnClose" runat="server" CssClass="default" Width="125" Text="Close" /></td>
                    </tr>
                </table>
            </td>
        </tr>
        </asp:Panel>
    </table>
</asp:Panel>
</asp:Content>
