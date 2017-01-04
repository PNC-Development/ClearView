<%@ Page Language="C#" MasterPageFile="~/datapoint.Master" AutoEventWireup="true" CodeBehind="design.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.design" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" runat="server">
    <asp:Panel ID="panAllow" runat="server" Visible="false">
    <table id="cntrlHeader" runat="Server" width="100%" cellpadding="0" cellspacing="5" border="0">
        <tr id="cntrlButtons">
            <td rowspan="2"><img src="/images/workload48.gif" border="0" align="absmiddle" /></td>
            <td class="header" nowrap valign="bottom">Design ID <asp:Label ID="lblHeader" runat="server" CssClass="header" /></td>
            <td width="100%" rowspan="2" align="right">
                <table cellpadding="1" cellspacing="4" border="0">
                    <tr>
                        <td nowrap><asp:LinkButton ID="btnNew" runat="server" Text="<img src='/images/new-icon.gif' border='0' align='absmiddle' /><img src='/images/spacer.gif' border='0' width='5' height='1' />New Search" OnClick="btnNew_Click" /></td>
                        <td>|</td>
                        <td nowrap><asp:LinkButton ID="btnSave" runat="server" Text="<img src='/images/save-icon.gif' border='0' align='absmiddle' /><img src='/images/spacer.gif' border='0' width='5' height='1' />Save" OnClick="btnSave_Click" /></td>
                        <td>|</td>
                        <td nowrap><asp:LinkButton ID="btnSaveClose" runat="server" Text="<img src='/images/save-icon.gif' border='0' align='absmiddle' /><img src='/images/spacer.gif' border='0' width='5' height='1' />Save & Close" OnClick="btnSaveClose_Click" /></td>
                        <td>|</td>
                        <td nowrap><asp:LinkButton ID="btnPrint" runat="server" Text="<img src='/images/print-icon.gif' border='0' align='absmiddle' /><img src='/images/spacer.gif' border='0' width='5' height='1' />Print" /></td>
                        <td>|</td>
                        <td nowrap><asp:LinkButton ID="btnClose" runat="server" Text="<img src='/images/close-icon.gif' border='0' align='absmiddle' /><img src='/images/spacer.gif' border='0' width='5' height='1' />Close" /></td>
                    </tr>
                    <asp:Panel ID="panSave" runat="server" Visible="false">
                    <tr>
                        <td colspan="7" class="bigCheck" align="center"><img src="/images/bigCheck.gif" border="0" align="absmiddle" /> Update Successful</td>
                    </tr>
                    </asp:Panel>
                    <asp:Panel ID="panError" runat="server" Visible="false">
                    <tr>
                        <td colspan="7" class="bigError" align="center"><img src="/images/bigError.gif" border="0" align="absmiddle" /> <asp:Label ID="lblError" runat="server" /></td>
                    </tr>
                    </asp:Panel>
                </table>
            </td>
        </tr>
        <tr id="cntrlButtons2">
            <td nowrap valign="top"><asp:Label ID="lblHeaderSub" runat="server" CssClass="default" /></td>
        </tr>
        <tr id="cntrlProcessing" style="display:none">
            <td colspan="20">
                <table width="100%" cellpadding="0" cellspacing="5" border="0">
                    <tr>
                        <td rowspan="2"><img src="/images/saving.gif" border="0" align="absmiddle" /></td>
                        <td class="header" width="100%" valign="bottom">Processing...</td>
                    </tr>
                    <tr>
                        <td width="100%" valign="top">Please do not close this window while the page is processing.  Please be patient....</td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <br />
    <%=strMenuTab1 %>
    <table width="100%" height="100%" cellpadding="10" cellspacing="5" border="0">
        <tr>
            <td valign="top">
                <div id="divMenu1" class="tabbing">
                    <div style="display:none">
                        <table width="100%" cellpadding="5" cellspacing="2" border="0">
                            <tr>
                                <td class="header">Forecast Details <asp:Image ID="imgBIR" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/spacer.gif" /></td>
                            </tr>
                            <tr>
                                <td>
                                    <%=strMenuTabForecast1 %>
                                    <table width="100%" height="100%" cellpadding="10" cellspacing="5" border="0">
                                        <tr>
                                            <td valign="top">
                                                <div id="divMenuForecast1" class="tabbing">
                                                    <div style="display:none">
                                                        <!-- General -->
                                                        <%=strForecastGeneral %>
                                                    </div>
                                                    <div style="display:none">
                                                        <!-- Platform -->
                                                        <%=strForecastPlatform %>
                                                    </div>
                                                    <div style="display:none">
                                                        <!-- Storage -->
                                                        <asp:Panel ID="panForecastStorageOS" runat="server" Visible="false">
                                                        <p><span class="biggerbold">Operating System Volumes</span></p>
                                                        <p><%=strForecastStorageOS %></p>
                                                        <br /><br />
                                                        </asp:Panel>
                                                        <p><span class="biggerbold">Application / Data Volumes</span></p>
                                                        <p><%=strForecastStorage %></p>
                                                    </div>
                                                    <div style="display:none">
                                                        <!-- Backup -->
                                                        <%=strMenuTabBackup1 %>
                                                        <div id="divMenuBackup1">
                                                            <div style="display:none">
                                                                <br />
                                                                <table width="100%" cellpadding="4" cellspacing="3" border="0">
                                                                    <%=strForecastBackup%>
                                                                </table>
                                                            </div>
                                                            <div style="display:none">
                                                                <br />
                                                                <table width="500" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                                                                    <tr bgcolor="#EEEEEE">
                                                                       <td nowrap><b>File/Folder</b></td>
                                                                       <td nowrap></td>
                                                                    </tr>
                                                                    <asp:repeater ID="rptInclusions" runat="server">
                                                                        <ItemTemplate>
                                                                           <tr>
                                                                              <td><%# DataBinder.Eval(Container.DataItem, "path") %></td>                                                               
                                                                           </tr>
                                                                        </ItemTemplate>
                                                                    </asp:repeater>
                                                                    <tr>
                                                                       <td colspan="2">
                                                                          <asp:Label ID="lblNoneInclusions" runat="server" CssClass="default" Visible="false" Text="<img src='/images/alert.gif' border='0' align='absmiddle'> There are no inclusions..." />
                                                                       </td>
                                                                    </tr>                       
                                                                </table>
                                                            </div>
                                                            <div style="display:none">
                                                                <br />
                                                                <table width="500" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                                                                    <tr bgcolor="#EEEEEE">
                                                                       <td nowrap><b>File/Folder</b></td>
                                                                       <td nowrap></td>
                                                                    </tr>
                                                                    <asp:repeater ID="rptExclusions" runat="server">
                                                                        <ItemTemplate>
                                                                           <tr>
                                                                              <td><%# DataBinder.Eval(Container.DataItem, "path") %></td>                                                               
                                                                           </tr>
                                                                        </ItemTemplate>
                                                                    </asp:repeater>
                                                                    <tr>
                                                                       <td colspan="2">
                                                                          <asp:Label ID="lblNoneExclusions" runat="server" CssClass="default" Visible="false" Text="<img src='/images/alert.gif' border='0' align='absmiddle'> There are no exclusions..." />
                                                                       </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                            <div style="display:none">
                                                                <br />
                                                                <table width="500" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                                                                    <tr bgcolor="#EEEEEE">
                                                                       <td nowrap colspan="3"><b>File/Folder</b></td>
                                                                       <td nowrap></td>
                                                                    </tr>
                                                                    <asp:repeater ID="rptRetention" runat="server">
                                                                        <ItemTemplate>
                                                                           <tr>
                                                                              <td colspan="3"><%# DataBinder.Eval(Container.DataItem, "path") %></td>                                                               
                                                                           </tr>
                                                                           <tr>
                                                                              <td>&nbsp;&nbsp;&nbsp;</td>
                                                                              <td nowrap>First Archival:</td>
                                                                              <td width="100%"><%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "first").ToString()).ToShortDateString() %></td>
                                                                              <td></td>
                                                                           </tr>
                                                                           <tr>
                                                                              <td>&nbsp;&nbsp;&nbsp;</td>
                                                                              <td nowrap>Archive Period:</td>
                                                                              <td width="100%"><%# (DataBinder.Eval(Container.DataItem, "number").ToString() == "0" ? "" : DataBinder.Eval(Container.DataItem, "number") + " ") %><%# DataBinder.Eval(Container.DataItem, "type") %></td>
                                                                              <td></td>
                                                                           </tr>
                                                                           <tr>
                                                                              <td>&nbsp;&nbsp;&nbsp;</td>
                                                                              <td nowrap>Start Time:</td>
                                                                              <td width="100%"><%# DataBinder.Eval(Container.DataItem, "hour") %> <%# DataBinder.Eval(Container.DataItem, "switch") %></td>
                                                                              <td></td>
                                                                           </tr>
                                                                           <tr>
                                                                              <td>&nbsp;&nbsp;&nbsp;</td>
                                                                              <td nowrap>Frequency:</td>
                                                                              <td width="100%"><%# DataBinder.Eval(Container.DataItem, "occurence") %></td>
                                                                              <td></td>
                                                                           </tr>
                                                                        </ItemTemplate>
                                                                    </asp:repeater>
                                                                    <tr>
                                                                       <td colspan="6">
                                                                          <asp:Label ID="lblNoneRetention" runat="server" CssClass="default" Visible="false" Text="<img src='/images/alert.gif' border='0' align='absmiddle'> There are no archive requirements..." />
                                                                       </td>
                                                                    </tr>
                                                                </table>                                                               
                                                            </div>
                                                            <div style="display:none">
                                                                <br />
                                                                <table width="400" cellpadding="5" cellspacing="2" border="0" class="default">
                                                                    <tr>
                                                                       <td nowrap>Average Size of One Data File:</td>
                                                                       <td width="100%"><asp:Label ID="lblAverage" runat="server" CssClass="default" /></td>
                                                                    </tr>
                                                                    <tr>
                                                                       <td nowrap>Production Turnover Documentation Folder Name:</td>
                                                                       <td width="100%"><asp:Label ID="lblDocumentation" runat="server" CssClass="default" /></td>
                                                                    </tr>
                                                                    <tr>
                                                                       <td colspan="2">&nbsp;</td>
                                                                    </tr>
                                                                    <tr>
                                                                       <td colspan="2" class="bold">Client File System Data</td>                                                      
                                                                    </tr>
                                                                    <tr>
                                                                       <td nowrap>Percent Changed Daily:</td>
                                                                       <td width="100%"><asp:Label ID="lblCFPercent" runat="server" CssClass="default" /></td>                                                       
                                                                    </tr>
                                                                    <tr>
                                                                       <td nowrap>Compression Ratio:</td>
                                                                       <td width="100%"><asp:Label ID="lblCFCompression" runat="server" CssClass="default" /></td>                                                        
                                                                    </tr>
                                                                    <tr>
                                                                       <td nowrap>Average File Size:</td>
                                                                       <td width="100%"><asp:Label ID="lblCFAverage" runat="server" CssClass="default" /></td>                                                        
                                                                    </tr>
                                                                    <tr>
                                                                       <td nowrap>Backup Version Ratio:</td>
                                                                       <td width="100%"><asp:Label ID="lblCFBackup" runat="server" CssClass="default" /></td>                                                        
                                                                    </tr>
                                                                    <tr>
                                                                       <td nowrap>Archive Ratio:</td>
                                                                       <td width="100%"><asp:Label ID="lblCFArchive" runat="server" CssClass="default" /></td>                                                        
                                                                    </tr>
                                                                    <tr>
                                                                       <td nowrap>Backup Window (Hours):</td>
                                                                       <td width="100%"><asp:Label ID="lblCFWindow" runat="server" CssClass="default" /></td>                                                        
                                                                    </tr>
                                                                    <tr>
                                                                       <td nowrap>Backupsets:</td>
                                                                       <td width="100%"><asp:Label ID="lblCFSets" runat="server" CssClass="default" /></td>                                                        
                                                                    </tr>
                                                                    <tr>
                                                                       <td colspan="2">&nbsp;</td>
                                                                    </tr>
                                                                    <tr>
                                                                       <td colspan="2" class="bold">Client Database Data</td>                                                        
                                                                    </tr>
                                                                    <tr>
                                                                       <td nowrap>Database Type:</td>
                                                                       <td width="100%"><asp:Label ID="lblCDType" runat="server" CssClass="default" /></td>                                                        
                                                                    </tr>
                                                                    <tr>
                                                                       <td nowrap>Percent Changed Daily:</td>
                                                                       <td width="100%"><asp:Label ID="lblCDPercent" runat="server" CssClass="default" /></td>                                                        
                                                                    </tr>
                                                                    <tr>
                                                                       <td nowrap>Compression Ratio:</td>
                                                                       <td width="100%"><asp:Label ID="lblCDCompression" runat="server" CssClass="default" /></td>                                                        
                                                                    </tr>
                                                                    <tr>
                                                                       <td nowrap>Number of Backup Versions:</td>
                                                                       <td width="100%"><asp:Label ID="lblCDVersions" runat="server" CssClass="default" /></td>                                                        
                                                                    </tr>
                                                                    <tr>
                                                                       <td nowrap>Backup Window (Hours):</td>
                                                                       <td width="100%"><asp:Label ID="lblCDWindow" runat="server" CssClass="default" /></td>                                                        
                                                                    </tr>
                                                                    <tr>
                                                                       <td nowrap>Growth Factor:</td>
                                                                       <td width="100%"><asp:Label ID="lblCDGrowth" runat="server" CssClass="default" /></td>                                                        
                                                                    </tr>
                                                                    <tr>
                                                                       <td colspan="2">&nbsp;</td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </div>
    <asp:Panel ID="panCFI" runat="server" Visible="false">
                    <table width="100%" cellpadding="5" cellspacing="2" border="0">
                        <tr>
                            <td colspan="2">
                                <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                    <tr>
                                        <td class="bigger"><b>Backup Configuration</b></td>
                                        <td align="right"><b>B</b> = Backup Acceptable</td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td nowrap class="bold">Backup Frequency:</td>
                            <td width="100%"><asp:Label ID="lblFrequency" runat="server" /></td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table width="100%" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                                    <tr>
                                        <td></td>
                                        <td colspan="12" align="center" style="color: #FFF; background-color: #333"><b>AM</b></td>
                                        <td colspan="12" align="center" style="color: #000; background-color: #AAA"><b>PM</b></td>
                                    </tr>
                                    <tr bgcolor="#EEEEEE">
                                        <td></td>
                                        <td>12</td>
                                        <td>1</td>
                                        <td>2</td>
                                        <td>3</td>
                                        <td>4</td>
                                        <td>5</td>
                                        <td>6</td>
                                        <td>7</td>
                                        <td>8</td>
                                        <td>9</td>
                                        <td>10</td>
                                        <td>11</td>
                                        <td>12</td>
                                        <td>1</td>
                                        <td>2</td>
                                        <td>3</td>
                                        <td>4</td>
                                        <td>5</td>
                                        <td>6</td>
                                        <td>7</td>
                                        <td>8</td>
                                        <td>9</td>
                                        <td>10</td>
                                        <td>11</td>
                                    </tr>
                                    <%=strBackup.ToString()%>
                                </table>
                            </td>
                        </tr>
                    </table>
        <table width="100%" cellpadding="5" cellspacing="2" border="0">
            <tr>
                <td class="bigger" colspan="2"><b>Backup Exclusion Configuration</b></td>
            </tr>
            <tr>
                <td colspan="2">
                    <table width="100%" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                        <tr bgcolor="#EEEEEE">
                            <td><b><u>Path:</u></b></td>
                        </tr>
                        <asp:repeater ID="rptExclusionsCFI" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td valign="top" width="100%"><%# DataBinder.Eval(Container.DataItem, "path") %></td>
                                </tr>
                            </ItemTemplate>
                            <AlternatingItemTemplate>
                                <tr bgcolor="F6F6F6">
                                    <td valign="top" width="100%"><%# DataBinder.Eval(Container.DataItem, "path") %></td>
                                </tr>
                            </AlternatingItemTemplate>
                        </asp:repeater>
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="lblExclusion" runat="server" CssClass="default" Visible="false" Text="<img src='/images/spacer.gif' border='0' height='1' width='25' /><img src='/images/alert.gif' border='0' align='absmiddle'> There are no backup exclusions" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </asp:Panel>
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <asp:Panel ID="panDeleted" runat="server" Visible="false">
                            <tr>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="header"><img src="/images/bigError.gif" border="0" align="absmiddle" /> This design was deleted!</td>
                            </tr>
                            </asp:Panel>
                        </table>
                    </div>
                    <div style="display:none">
                        <table width="100%" cellpadding="5" cellspacing="2" border="0">
                            <tr>
                                <td class="header">Configuration Details</td>
                            </tr>
                            <tr>
                                <td>
                                    <%=strMenuTabConfig1 %>
                                    <table width="100%" height="100%" cellpadding="10" cellspacing="5" border="0">
                                        <tr>
                                            <td valign="top">
                                                <div id="divMenuConfig1" class="tabbing">
                                                    <div style="display:none">
                                                        <!-- Application -->
                                                        <%=strConfigApplication %>
                                                    </div>
                                                    <div style="display:none">
                                                        <!-- Device -->
                                                        <%=strMenuTabConfigDevices1%>
                                                        <table width="100%" height="100%" cellpadding="10" cellspacing="5" border="0">
                                                            <tr>
                                                                <td valign="top">
                                                                    <div id="divMenuConfigDevices1" class="tabbing">
                                                                        <%=strConfigDevice %>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                    <div style="display:none">
                                                        <!-- User -->
                                                        <%=strMenuTabConfigUsers1%>
                                                        <table width="100%" height="100%" cellpadding="10" cellspacing="5" border="0">
                                                            <tr>
                                                                <td valign="top">
                                                                    <div id="divMenuConfigUsers1" class="tabbing">
                                                                        <%=strConfigUser %>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                    <div style="display:none">
                                                        <!-- Storage -->
                                                        <%=strMenuTabConfigStorage1%>
                                                        <table width="100%" height="100%" cellpadding="10" cellspacing="5" border="0">
                                                            <tr>
                                                                <td valign="top">
                                                                    <div id="divMenuConfigStorage1" class="tabbing">
                                                                        <%=strConfigStorage %>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="display:none">
                        <table width="100%" cellpadding="5" cellspacing="2" border="0">
                            <tr>
                                <td class="header">Provisioning Status</td>
                                <td align="right">
                                    <asp:Button ID="btnDesign" runat="server" CssClass="default" Width="100" Text="View Design" />&nbsp;
                                    <asp:Button ID="btnProvisioning" runat="server" CssClass="default" Width="100" Text="View Execution" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <%=strMenuTabExecution1 %>
                                    <table width="100%" height="100%" cellpadding="10" cellspacing="5" border="0">
                                        <tr>
                                            <td valign="top">
                                                <div id="divMenuExecution1" class="tabbing">
                                                    <%=strExecution %>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="display:none">
                        <table width="100%" cellpadding="5" cellspacing="2" border="0">
                            <tr>
                                <td colspan="2" class="header">Administration</td>
                            </tr>
                            <tr>
                                <td colspan="2" align="center" class="smallalert">
                                    <table cellpadding="3" cellspacing="0" border="0">
                                        <tr>
                                            <td><img src="/images/alert.gif" border="0" align="absmiddle" /></td>
                                            <td> <b>NOTE:</b> When modifying the design, use the SAVE buttons at the bottom of each tab...DO NOT use the SAVE button at the top of this page.</td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td nowrap class="bold">Legacy Design ID:</td>
                                <td width="100%"><asp:Label ID="lblAnswer" runat="server" /></td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:PlaceHolder ID="phAdministration" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="display:none">
                        <table width="100%" cellpadding="5" cellspacing="2" border="0">
                            <tr>
                                <td class="header">Implementation Details</td>
                            </tr>
                            <tr>
                                <td>
                                    <%=strMenuTabImplementation1%>
                                    <table width="100%" height="100%" cellpadding="10" cellspacing="5" border="0">
                                        <tr>
                                            <td valign="top">
                                                <div id="divMenuImplementation1" class="tabbing">
                                                    <%=strImplementation %>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </td>
        </tr>
    </table>
    </asp:Panel>
    <asp:Panel ID="panDenied" runat="server" Visible="false">
        <br />
            <table width="100%" cellpadding="0" cellspacing="5" border="0">
                <tr>
                    <td rowspan="2"><img src="/images/ico_error.gif" border="0" align="absmiddle" /></td>
                    <td class="header" width="100%" valign="bottom">Access Denied</td>
                </tr>
                <tr>
                    <td width="100%" valign="top">You do not have sufficient permission to view this page.</td>
                </tr>
                <tr>
                    <td colspan="2">&nbsp;</td>
                </tr>
                <tr>
                    <td></td>
                    <td width="100%">If you think you should have rights to view it, please contact your ClearView administrator.</td>
                </tr>
            </table>
        <p>&nbsp;</p>
    </asp:Panel>
<asp:HiddenField ID="hdnTab" runat="server" />
</asp:Content>
