<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="fore_approve_cfi.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.fore_approve_cfi" %>
<script type="text/javascript" src="/javascript/design.js"></script>
<script type="text/javascript">
</script>
<table width="100%" cellpadding="0" cellspacing="0" border="0" class="default">
    <tr>
        <td><img src="/images/table_topleft.gif" border="0" width="5" height="26"></td>
        <td nowrap background="/images/table_top.gif" class="greentableheader" width="100%">Design Approval (# <asp:Label ID="lblID" runat="server" CssClass="greentableheader" />)</td>
        <td><img src="/images/table_topright.gif" border="0" width="5" height="26"></td>
    </tr>
    <tr>
        <td background="/images/table_left.gif"><img src="/images/table_left.gif" width="5" height="10"></td>
        <td width="100%" bgcolor="#FFFFFF">
            <br />
            <asp:Panel ID="panShow" runat="server" Visible="false">
                <%=strMenuTabApprove1 %>
                <div id="divMenuApprove1">
                    <br />
                    <div style="display:inline">
                        <asp:PlaceHolder ID="phSummary" runat="server" />
                        <table width="100%" cellpadding="4" cellspacing="3" border="0">
                            <tr>
                                <td colspan="2">&nbsp;</td>
                            </tr>
                            <tr>
                                <td colspan="2" class="biggerbold"><a href="javascript:void(0);" onclick="OpenNewWindow('/frame/design_print.aspx?id=<%=intID.ToString() %>',750,600)" oncontextmenu="OpenNewWindow('/frame/design.aspx?id=<%=intID.ToString() %>',750,600)"><img src='/images/project_request.gif' border='0' align='absmiddle' /><img src='/images/spacer.gif' border='0' width='5' height='1' />View the Complete Design</a></td>
                            </tr>
                            <tr>
                                <td colspan="2">Comments:&nbsp;&nbsp;(required if you deny this request)</td>
                            </tr>
                            <tr>
                                <td colspan="2"><asp:TextBox ID="txtComments" runat="server" CssClass="default" Width="90%" Rows="10" TextMode="multiLine" /></td>
                            </tr>
                            <tr id="trMHS" runat="server" visible="false">
                                <td colspan="2">
                                    <table border="0">
                                        <tr>
                                            <td>Will the server(s) associated with this design be hosted by MHS?</td>
                                            <td>
                                                <asp:RadioButton ID="radMHSYes" runat="server" Text="Yes" GroupName="radMHS" /> 
                                                <asp:RadioButton ID="radMHSNo" runat="server" Text="No" GroupName="radMHS" /> 
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2"><hr size="1" noshade /></td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Button ID="btnApprove" runat="server" CssClass="default" Width="75" Text="Approve" OnClick="btnApprove_Click" /> 
                                    <asp:Button ID="btnDeny" runat="server" CssClass="default" Width="75" Text="Deny" OnClick="btnDeny_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="display:none">
                        <table cellpadding="5" cellspacing="0" border="0">
                            <tr>
                                <td colspan="2" class="bold">The following comments were provided to explain the need for the exception:</td>
                            </tr>
                            <tr>
                                <td valign="top"><img src="/images/icon_answer.gif" border="0" align="absmiddle" /></td>
                                <td valign="top" width="100%"><asp:Label ID="lblComments" runat="server" /></td>
                            </tr>
                            <tr>
                                <td colspan="2"><b>Exception ID:</b>&nbsp;<asp:TextBox ID="txtExceptionID" runat="server" Width="100" MaxLength="20" />&nbsp;&nbsp;<asp:Button ID="btnException" runat=server Text="Update" OnClick="btnException_Click" /></td>
                            </tr>
                            <tr>
                                <td colspan="2" align="center" class="smallalert">
                                    <table cellpadding="0" cellspacing="0" border="0">
                                        <tr>
                                            <td><img src="/images/alert.gif" border="0" align="absmiddle" /></td>
                                            <td> <b>NOTE:</b> You must save each tab in which you make changes.  Typically, there is a button at the bottom of each tab.</td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Panel ID="panException" runat="server" Visible="false">
                                    <%=strMenuTabApprove2%>
                                    <div id="divMenuApprove2">
                                        <br />
                                        <div style="display:none">
                                            <table width="100%" cellpadding="4" cellspacing="3" border="0">
                                                <tr>
                                                    <td valign="top" nowrap>Mnemonic:</td>
                                                    <td valign="top" width="100%"><asp:Label ID="lblMnemonic" runat="server" /></td>
                                                </tr>
                                                <tr>
                                                    <td valign="top" nowrap>Cost Center:</td>
                                                    <td valign="top" width="100%"><asp:Label ID="lblCost" runat="server" /></td>
                                                </tr>
                                                <tr>
                                                    <td valign="top" nowrap>Environment:</td>
                                                    <td valign="top" width="100%"><asp:DropDownList ID="ddlClass" CssClass="default" runat="server" Width="300" /></td>
                                                </tr>
                                                <tr>
                                                    <td valign="top" nowrap>Operating System:</td>
                                                    <td valign="top" width="100%"><asp:DropDownList ID="ddlOS" CssClass="default" runat="server" Width="300" /></td>
                                                </tr>
                                                <tr>
                                                    <td valign="top" nowrap>Service Pack:</td>
                                                    <td valign="top" width="100%"><asp:DropDownList ID="ddlSP" CssClass="default" runat="server" Width="300" /></td>
                                                </tr>
                                                <tr>
                                                    <td valign="top" nowrap>Domain:</td>
                                                    <td valign="top" width="100%">
                                                        <asp:DropDownList ID="ddlDomain" CssClass="default" runat="server" Width="300" />
                                                        &nbsp;&nbsp;&nbsp;&nbsp;(Based on Class + Environment)
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign="top" nowrap>Backup Window:</td>
                                                    <td valign="top" width="100%">
                                                        <asp:DropDownList ID="ddlBackup" runat="server" Width="300">
                                                            <asp:ListItem Text="-- None --" Value="" />
                                                            <asp:ListItem Text="Daily" Value="D" />
                                                            <asp:ListItem Text="Weekly" Value="W" />
                                                            <asp:ListItem Text="Monthly" Value="M" />
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign="top" nowrap>DR:</td>
                                                    <td valign="top" width="100%">
                                                        <asp:DropDownList ID="ddlDR" runat="server" Width="300">
                                                            <asp:ListItem Text="-- None --" Value="-1" />
                                                            <asp:ListItem Text="Under 48 Hours" Value="1" />
                                                            <asp:ListItem Text="48 Hours or More" Value="0" />
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign="top" nowrap>Server Location:</td>
                                                    <td valign="top" width="100%">
                                                        <asp:DropDownList ID="ddlEnvironment" CssClass="default" runat="server" Width="300" Enabled="false" >
                                                            <asp:ListItem Value="-- Please select an Environment --" />
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign="top" nowrap>High Availability:</td>
                                                    <td valign="top" width="100%">
                                                        <asp:RadioButton ID="radHAClusterAP" runat="server" Text="Clustering (Active / Passive)" GroupName="HA" /><br />
                                                        <asp:RadioButton ID="radHAClusterAA" runat="server" Text="Clustering (Active / Active)" GroupName="HA" /><br />
                                                        <asp:RadioButton ID="radHALoadBalance" runat="server" Text="Network Load Balancing" GroupName="HA" /><br />
                                                        <asp:RadioButton ID="radHANone" runat="server" Text="None" GroupName="HA" /><br />
                                                    </td>
                                                </tr>
                                                <tr id="divInstances" runat="server" style="display:none">
                                                    <td valign="top" nowrap>Cluster Instances:</td>
                                                    <td valign="top" width="100%"><asp:TextBox ID="txtInstances" runat="server" Width="75" /></td>
                                                </tr>
                                                <tr id="divQuorum" runat="server" style="display:none">
                                                    <td valign="top" nowrap>Cluster Quorum Drive:</td>
                                                    <td valign="top" width="100%"><asp:TextBox ID="txtQuorum" runat="server" Width="75" /> GB</td>
                                                </tr>
                                                <tr id="divLoadBalance" runat="server" style="display:none">
                                                    <td valign="top" nowrap>Network Tier:</td>
                                                    <td valign="top" width="100%">
                                                        <asp:DropDownList ID="ddlLoadBalance" runat="server" Width="300">
                                                            <asp:ListItem Text="-- SELECT --" Value="0" />
                                                            <asp:ListItem Text="Middleware" Value="M" />
                                                            <asp:ListItem Text="Web Tier" Value="W" />
                                                            <asp:ListItem Text="App Tier" Value="A" />
                                                            <asp:ListItem Text="Web / App Tier" Value="WA" />
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign="top" nowrap>Special Hardware:</td>
                                                    <td valign="top" width="100%"><asp:Label ID="lblSpecial" runat="server" /></td>
                                                </tr>
                                                <tr>
                                                    <td valign="top" nowrap>Server Count:</td>
                                                    <td valign="top" width="100%"><asp:TextBox ID="txtQuantity" runat="server" Width="75" /></td>
                                                </tr>
                                                <tr>
                                                    <td valign="top" nowrap>Number of Cores:</td>
                                                    <td valign="top" width="100%"><asp:TextBox ID="txtCores" runat="server" Width="75" /></td>
                                                </tr>
                                                <tr>
                                                    <td valign="top" nowrap>Amount of RAM:</td>
                                                    <td valign="top" width="100%"><asp:TextBox ID="txtRam" runat="server" Width="75" /> (GB)</td>
                                                </tr>
                                                <tr>
                                                    <td valign="top" nowrap>Build Date:</td>
                                                    <td valign="top" width="100%"><asp:Label ID="lblDate" runat="server" /></td>
                                                </tr>
                                                <tr>
                                                    <td valign="top" nowrap>Confidence Level:</td>
                                                    <td valign="top" width="100%"><asp:Label ID="lblConfidence" runat="server" /></td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" class="header">*** Optional Configuration ***</td>
                                                </tr>
                                                <tr>
                                                    <td valign="top" nowrap>Infrastructure Component:</td>
                                                    <td valign="top" width="100%"><asp:DropDownList ID="ddlApplications" runat="server" CssClass="default" Width="300" />&nbsp;&nbsp;&nbsp;<img src="/images/alert.gif" border="0" align="absmiddle" /> <b>NOTE:</b> NOT required.</td>
                                                </tr>
                                                <tr id="divSubApplications" runat="server" style="display:none">
                                                    <td valign="top" nowrap>Infrastructure Role:</td>
                                                    <td valign="top" width="100%"><asp:DropDownList ID="ddlSubApplications" runat="server" CssClass="default" Width="300" /></td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td valign="top" nowrap></td>
                                                    <td valign="top" width="100%"><asp:Button ID="btnSaveProperties" runat="server" Text="Save Properties" Width="125" OnClick="btnSaveProperties_Click" /></td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div style="display:none">
                                            <table width="100%" height="100%" cellpadding="0" cellspacing="0" border="0">
                                                <tr height="1">
                                                    <td><img src="/images/table_topleft.gif" border="0" width="5" height="26"></td>
                                                    <td nowrap background="/images/table_top.gif" class="greentableheader" width="100%">New Account</td>
                                                    <td><img src="/images/table_topright.gif" border="0" width="5" height="26"></td>
                                                </tr>
                                                <tr>
                                                    <td background="/images/table_left.gif"><img src="/images/table_left.gif" width="5" height="10"></td>
                                                    <td width="100%" bgcolor="#FFFFFF" valign="top">
                                                        <table width="100%" border="0" cellspacing="4" cellpadding="4">
                                                            <tr>
                                                                <td nowrap>User:</td>
                                                                <td width="100%">
                                                                    <table cellpadding="0" cellspacing="0" border="0">
                                                                        <tr>
                                                                            <td><asp:TextBox ID="txtAccount" runat="server" Width="300" CssClass="default" /></td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <div id="divAccount" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                                                                                    <asp:ListBox ID="lstAccount" runat="server" CssClass="default" />
                                                                                </div>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td nowrap class="footer">&nbsp;</td>
                                                                <td class="footer">(Enter a valid LAN ID, First Name, or Last Name)</td>
                                                            </tr>
                                                            <tr>
                                                                <td></td>
                                                                <td nowrap><asp:LinkButton ID="btnManager" runat="server" Text="User Not Appearing in the List? Click Here." /></td>
                                                            </tr>
                                                            <tr>
                                                                <td>Permission:</td>
                                                                <td nowrap>
                                                                    <asp:DropDownList ID="ddlPermission" runat="server">
                                                                        <asp:ListItem Value="0" Text="-- SELECT --" />
                                                                        <asp:ListItem Value="D" Text="Developer" />
                                                                        <asp:ListItem Value="P" Text="Promoter" />
                                                                        <asp:ListItem Value="S" Text="AppSupport" />
                                                                        <asp:ListItem Value="U" Text="AppUsers" />
                                                                    </asp:DropDownList>&nbsp;&nbsp;&nbsp;
                                                                    <asp:CheckBox ID="chkRemoteDesktop" runat="server" Text="With Remote Desktop" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td></td>
                                                                <td nowrap></td>
                                                            </tr>
                                                            <tr>
                                                                <td></td>
                                                                <td nowrap><asp:Button ID="btnAddAccount" runat="server" CssClass="default" Width="125" OnClick="btnAddAccount_Click" Text="Add Account" /></td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td background="/images/table_right.gif"><img src="/images/table_right.gif" width="5" height="10"></td>
                                                </tr>
                                                <tr height="1">
                                                    <td><img src="/images/table_bottomLeft.gif" border="0" width="5" height="9"></td>
                                                    <td width="100%" background="/images/table_bottom.gif"></td>
                                                    <td><img src="/images/table_bottomRight.gif" border="0" width="5" height="9"></td>
                                                </tr>
                                            </table>
                                            <br />
                                            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                                <tr>
                                                    <td><img src="/images/table_topleft.gif" border="0" width="5" height="26"></td>
                                                    <td nowrap background="/images/table_top.gif" class="greentableheader" width="100%">Current Account Requests</td>
                                                    <td><img src="/images/table_topright.gif" border="0" width="5" height="26"></td>
                                                </tr>
                                                <tr>
                                                    <td background="/images/table_left.gif"><img src="/images/table_left.gif" width="5" height="10"></td>
                                                    <td width="100%" bgcolor="#FFFFFF">
                                                        <div style="height:100%; width:100%; overflow:auto">
                                                            <table width="100%" cellpadding="4" cellspacing="0" border="0" align="center">
                                                                <tr>
                                                                    <td><b><u>User:</u></b></td>
                                                                    <td><b><u>Permission:</u></b></td>
                                                                    <td></td>
                                                                </tr>
                                                                <asp:repeater ID="rptAccounts" runat="server">
                                                                    <ItemTemplate>
                                                                        <tr>
                                                                            <td valign="top"><%# oUser.GetFullName(Int32.Parse(DataBinder.Eval(Container.DataItem, "userid").ToString())) %> (<%# oUser.GetName(Int32.Parse(DataBinder.Eval(Container.DataItem, "userid").ToString()))%>)</td>
                                                                            <td valign="top"><asp:Label ID="lblPermissions" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "access") %>' ToolTip='<%# DataBinder.Eval(Container.DataItem, "remote") %>' /></td>
                                                                            <td valign="top" align="right"><asp:LinkButton ID="btnDeleteAccount" runat="server" Text="Delete" OnClick="btnDeleteAccount_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' /></td>
                                                                        </tr>
                                                                    </ItemTemplate>
                                                                    <AlternatingItemTemplate>
                                                                        <tr bgcolor="F6F6F6">
                                                                            <td valign="top"><%# oUser.GetFullName(Int32.Parse(DataBinder.Eval(Container.DataItem, "userid").ToString()))%> (<%# oUser.GetName(Int32.Parse(DataBinder.Eval(Container.DataItem, "userid").ToString()))%>)</td>
                                                                            <td valign="top"><asp:Label ID="lblPermissions" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "access") %>' ToolTip='<%# DataBinder.Eval(Container.DataItem, "remote") %>' /></td>
                                                                            <td valign="top" align="right"><asp:LinkButton ID="btnDeleteAccount" runat="server" Text="Delete" OnClick="btnDeleteAccount_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' /></td>
                                                                        </tr>
                                                                    </AlternatingItemTemplate>
                                                                </asp:repeater>
                                                                <tr>
                                                                    <td colspan="2">
                                                                        <asp:Label ID="lblNone" runat="server" CssClass="default" Visible="false" Text="<img src='/images/spacer.gif' border='0' height='1' width='25' /><img src='/images/alert.gif' border='0' align='absmiddle'> You have not added any accounts to this request" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </td>
                                                    <td background="/images/table_right.gif"><img src="/images/table_right.gif" width="5" height="10"></td>
                                                </tr>
                                                <tr>
                                                    <td><img src="/images/table_bottomLeft.gif" border="0" width="5" height="9"></td>
                                                    <td width="100%" background="/images/table_bottom.gif"></td>
                                                    <td><img src="/images/table_bottomRight.gif" border="0" width="5" height="9"></td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div style="display:none">
                                            <table cellpadding="4" cellspacing="0" border="0">
                                                <tr>
                                                    <td>
                                                        <iframe id="frmComponents" runat="server" frameborder="1" scrolling="no" width="730" height="450" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="100%"><asp:Button ID="btnSaveComponents" runat="server" Text="Save Components" Width="125" OnClick="btnSaveComponents_Click" /></td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div style="display:none">
                                            <table width="100%" cellpadding="4" cellspacing="0" border="0" align="center" style="border:solid 1px #CCCCCC">
                                                <tr bgcolor="#EEEEEE">
                                                    <td style='display:<%=boolWindows ? "inline" : "none" %>' nowrap><b><u>Drive:</u></b></td>
                                                    <td style='display:<%=boolWindows ? "inline" : "none" %>' width="100%"><b><u>Mount Point:</u></b></td>
                                                    <td style='display:<%=boolWindows == false ? "inline" : "none" %>' width="100%"><b><u>Filesystem:</u></b></td>
                                                    <td nowrap><b><u>Shared:</u></b></td>
                                                    <td nowrap><b><u>Size:</u></b></td>
                                                    <td nowrap></td>
                                                </tr>
                                                <tr id="trStorageOSWindows" runat="server" visible="false">
                                                    <td nowrap>C:\,D:\</td>
                                                    <td width="100%">&nbsp;&nbsp;<span class="footer">(Reserved: Operating System)</span></td>
                                                    <td nowrap><input type="checkbox" disabled="disabled" /></td>
                                                    <td nowrap><asp:TextBox ID="txtStorageSizeOS" runat="server" Width="50" MaxLength="10" /> GB</td>
                                                    <td nowrap>
                                                        <asp:ImageButton ID="btnStorageSaveOS" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/save_icon.jpg" CommandArgument="APP" OnClick="btnStorageSave_Click" /> 
                                                        <img src="/images/spacer.gif" border="0" height="16" width="16" />
                                                    </td>
                                                </tr>
                                                <tr id="trStorageApp" runat="server" visible="false">
                                                    <td nowrap>E:**</td>
                                                    <td width="100%">&nbsp;&nbsp;<span class="footer">(Reserved: Application Drive)</span></td>
                                                    <td nowrap><input type="checkbox" disabled="disabled" /></td>
                                                    <td nowrap><asp:TextBox ID="txtStorageSizeE" runat="server" Width="50" MaxLength="10" /> GB</td>
                                                    <td nowrap>
                                                        <asp:ImageButton ID="btnStorageSaveSizeE" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/save_icon.jpg" CommandArgument="APP" OnClick="btnStorageSave_Click" /> 
                                                        <img src="/images/spacer.gif" border="0" height="16" width="16" />
                                                    </td>
                                                </tr>
                                                <asp:repeater ID="rptStorage" runat="server">
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td style='display:<%=boolWindows ? "inline" : "none" %>' valign="top" nowrap><asp:DropDownList ID="ddlStorageDrive" runat="server" /></td>
                                                            <td valign="top" width="100%"><asp:TextBox ID="txtStoragePath" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "path") %>' Width="200" /></td>
                                                            <td valign="top" nowrap><asp:CheckBox ID="chkStorageSize" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "shared") %>' /></td>
                                                            <td valign="top" nowrap><asp:TextBox ID="txtStorageSize" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "size") %>' Width="50" MaxLength="10" /> GB</td>
                                                            <td valign="top" nowrap align="right">
                                                                <asp:ImageButton ID="btnStorageSave" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/save_icon.jpg" CommandArgument="SAVE" CommandName='<%# DataBinder.Eval(Container.DataItem, "id") %>' OnClick="btnStorageSave_Click" /> 
                                                                <asp:ImageButton ID="btnStorageDelete" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/cancel.gif" CommandArgument="DELETE" CommandName='<%# DataBinder.Eval(Container.DataItem, "id") %>' OnClick="btnStorageSave_Click" /> 
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:repeater>
                                                <tr id="trStorageDrive" runat="server" visible="false">
                                                    <td id="tdStorageDrive" nowrap><asp:DropDownList ID="ddlStorageDriveNew" runat="server" /></td>
                                                    <td width="100%"><asp:TextBox ID="txtStoragePathNew" runat="server" Width="200" /></td>
                                                    <td nowrap><asp:CheckBox ID="chkStorageSizeNew" runat="server" /></td>
                                                    <td nowrap><asp:TextBox ID="txtStorageSizeNew" runat="server" Width="50" MaxLength="10" /> GB</td>
                                                    <td nowrap>
                                                        <asp:ImageButton ID="btnStorageSaveNew" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/save_icon.jpg" CommandArgument="NEW" OnClick="btnStorageSave_Click" /> 
                                                        <asp:ImageButton ID="btnStorageDeleteNew" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/cancel.gif" CommandArgument="CANCEL" OnClick="btnStorageSave_Click" /> 
                                                    </td>
                                                </tr>
                                                <tr id="trStorageDriveNew" runat="server">
                                                    <td colspan="10">
                                                        <asp:LinkButton ID="btnStorageDriveAdd" runat="server" Text="<img src='/images/green_arrow.gif' border='0' align='absmiddle'/> Add More Storage" OnClick="btnStorageDriveAdd_Click" />
                                                    </td>
                                                </tr>
                                            </table>
                                            <div align="right" style='display:<%=boolWindows ? "inline" : "none" %>'>** = If you do not require an application drive, set it to zero (0) GB.</div>
                                        </div>
                                        <div style="display:none">
                                            <table cellpadding="4" cellspacing="0" border="0">
                                                <tr>
                                                    <td>
                                                        <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                                            <tr>
                                                                <td valign="top" nowarp><b>Pre-Configured Options:</b></td>
                                                            </tr>
                                                            <tr>
                                                                <td valign="top" nowarp>
                                                                    <asp:DropDownList ID="ddlGridBackup" runat="server">
                                                                        <asp:ListItem Value="0" Text="-- MANUAL --" />
                                                                        <asp:ListItem Value="S" Text="Standard Backup Window" />
                                                                        <asp:ListItem Value="E" Text="Evening Only" />
                                                                        <asp:ListItem Value="M" Text="Early Morning" />
                                                                        <asp:ListItem Value="D" Text="During the Business Day" />
                                                                        <asp:ListItem Value="W" Text="Weekends Only" />
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td width="100%">
                                                        <%=strGridBackupTable %>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="100%"><asp:Button ID="btnSaveBackup" runat="server" Text="Save Backup Window" Width="150" OnClick="btnSaveBackup_Click" /></td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div style="display:none">
                                            <table cellpadding="4" cellspacing="0" border="0">
                                                <tr>
                                                    <td nowrap>Path:</td>
                                                    <td width="100%"><asp:TextBox ID="txtPath" runat="server" Width="300" CssClass="default" /></td>
                                                </tr>
                                                <tr>
                                                    <td></td>
                                                    <td nowrap><asp:Button ID="btnAddExclusion" runat="server" CssClass="default" Width="125" OnClick="btnAddExclusion_Click" Text="Add Exclusion" /></td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <table width="400" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                                                            <tr bgcolor="#EEEEEE">
                                                                <td><b><u>Path:</u></b></td>
                                                                <td></td>
                                                            </tr>
                                                            <asp:repeater ID="rptExclusions" runat="server">
                                                                <ItemTemplate>
                                                                    <tr>
                                                                        <td valign="top" width="100%"><%# DataBinder.Eval(Container.DataItem, "path") %></td>
                                                                        <td valign="top" align="right"><asp:LinkButton ID="btnDeleteExclusion" runat="server" Text="Delete" OnClick="btnDeleteExclusion_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' /></td>
                                                                    </tr>
                                                                </ItemTemplate>
                                                                <AlternatingItemTemplate>
                                                                    <tr bgcolor="F6F6F6">
                                                                        <td valign="top" width="100%"><%# DataBinder.Eval(Container.DataItem, "path") %></td>
                                                                        <td valign="top" align="right"><asp:LinkButton ID="btnDeleteExclusion" runat="server" Text="Delete" OnClick="btnDeleteExclusion_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' /></td>
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
                                        </div>
                                        <div style="display:none">
                                            <table cellpadding="4" cellspacing="0" border="0">
                                                <tr>
                                                    <td>
                                                        <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                                            <tr>
                                                                <td valign="top" nowarp><b>Pre-Configured Options:</b></td>
                                                            </tr>
                                                            <tr>
                                                                <td valign="top" nowarp>
                                                                    <asp:DropDownList ID="ddlGridMaintenance" runat="server">
                                                                        <asp:ListItem Value="0" Text="-- MANUAL --" />
                                                                        <asp:ListItem Value="S" Text="Standard Maintenance Window" />
                                                                        <asp:ListItem Value="E" Text="Evening Only" />
                                                                        <asp:ListItem Value="M" Text="Early Morning" />
                                                                        <asp:ListItem Value="D" Text="During the Business Day" />
                                                                        <asp:ListItem Value="W" Text="Weekends Only" />
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td width="100%">
                                                        <%=strGridMaintenanceTable %>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="100%"><asp:Button ID="btnSaveMaintenance" runat="server" Text="Save Maintenance Window" Width="175" OnClick="btnSaveMaintenance_Click" /></td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div style="display:none">
                                            <table width="100%" cellpadding="4" cellspacing="3" border="0">
                                                <tr>
                                                    <td colspan="2" class="header">Change Assigned Model</td>
                                                </tr>
                                                <tr>
                                                    <td valign="top" nowrap>Model Platform:</td>
                                                    <td valign="top" width="100%"><asp:DropDownList ID="ddlPlatform" runat="server" CssClass="default" Width="400" /></td>
                                                </tr>
                                                <tr>
                                                    <td valign="top" nowrap>Model Type:</td>
                                                    <td valign="top" width="100%"><asp:DropDownList ID="ddlPlatformType" runat="server" CssClass="default" Width="400" /></td>
                                                </tr>
                                                <tr>
                                                    <td valign="top" nowrap>Model:</td>
                                                    <td valign="top" width="100%"><asp:DropDownList ID="ddlPlatformModel" runat="server" CssClass="default" Width="400" /></td>
                                                </tr>
                                                <tr>
                                                    <td valign="top" nowrap>Model Property:</td>
                                                    <td valign="top" width="100%"><asp:DropDownList ID="ddlPlatformModelProperty" runat="server" CssClass="default" Width="400" /></td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" class="header">Change Assigned Location</td>
                                                </tr>
                                                <tr>
                                                    <td valign="top" nowrap>Build Location:</td>
                                                    <td valign="top" width="100%"><%=strLocation %></td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td valign="top" nowrap></td>
                                                    <td valign="top" width="100%"><asp:Button ID="btnSaveHardware" runat="server" Text="Save Hardware" Width="125" OnClick="btnSaveHardware_Click" /></td>
                                                </tr>
                                            </table>
                                        </div>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </asp:Panel>
            <asp:Panel ID="panFinish" runat="server" Visible="false">
                <table width="100%" cellpadding="2" cellspacing="2" border="0">
                    <tr>
                        <td><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                        <td class="header"><img src="/images/bigCheck.gif" border="0" align="absmiddle" /> Record Updated</td>
                    </tr>
                    <tr><td colspan="2">&nbsp;</td></tr>
                    <tr>
                        <td><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                        <td>Your information has been saved successfully.</td>
                    </tr>
                    <tr><td colspan="2">&nbsp;</td></tr>
                    <tr>
                        <td colspan="2"><hr size="1" noshade /></td>
                    </tr>
                    <tr>
                        <td colspan="2" align="right">
                            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                <tr>
                                    <td class="footer"></td>
                                    <td align="right"><asp:Button ID="btnFinish" runat="server" CssClass="default" Width="75" Text="Finish" OnClick="btnFinish_Click" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <p>&nbsp;</p>
        </td>
        <td background="/images/table_right.gif"><img src="/images/table_right.gif" width="5" height="10"></td>
    </tr>
    <tr>
        <td><img src="/images/table_bottomLeft.gif" border="0" width="5" height="9"></td>
        <td width="100%" background="/images/table_bottom.gif"></td>
        <td><img src="/images/table_bottomRight.gif" border="0" width="5" height="9"></td>
    </tr>
</table>
<asp:Label ID="lblType" runat="server" Visible="false" />
<asp:Label ID="lblUserID" runat="server" Visible="false" />
<asp:Label ID="lblTypeID" runat="server" Visible="false" />
<asp:HiddenField ID="hdnAccount" runat="server" />
<asp:HiddenField ID="hdnEnvironment" runat="server" />
<asp:HiddenField ID="hdnSubApplication" runat="server" />
<asp:HiddenField ID="hdnDate" runat="server" />
<asp:HiddenField ID="hdnModel" runat="server" />
<asp:HiddenField ID="hdnLocation" runat="server" />
<input type="hidden" id="hdnBackup" name="hdnBackup" value='<%=strGridBackup %>' />
<input type="hidden" id="hdnMaintenance" name="hdnMaintenance" value='<%=strGridMaintenance %>' />
<input type="hidden" id="hdnComponents" name="hdnComponents" />
