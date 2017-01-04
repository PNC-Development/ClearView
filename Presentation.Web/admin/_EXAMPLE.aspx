<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="_EXAMPLE.aspx.cs" Inherits="NCC.ClearView.Presentation.Web._EXAMPLE" %>



<html>
<head>
<title>ClearView Administration</title>
<link rel="stylesheet" type="text/css" href="/css/default.css" />
<script type="text/javascript" src="/javascript/global.js"></script>
</head>
<body topmargin="0" leftmargin="0">
<form id="Form1" runat="server">
        <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
        <td height="100%">
        <div style="height:100%; overflow:auto">
<table width="98%" cellpadding="0" cellspacing="0" border="0" align="center">
		<tr><td colspan="2">&nbsp;</td></tr>
		<tr> 
		    <td><b>Audit Scripts</b></td>
		    <td align="right"><asp:LinkButton id="btnNew" runat="server" Text="Add New" OnClick="btnNew_Click" /></td>
		</tr>
		<tr><td colspan="2">&nbsp;</td></tr>
		<tr>
		    <td colspan="2" align="center">
                <asp:Panel ID="panView" runat="server" Visible="false">
                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <asp:repeater ID="rptView" runat="server">
                                    <HeaderTemplate>
                                        <table width="100%" cellpadding="3" cellspacing="0" border="0" align="center">
                                            <tr bgcolor="#CCCCCC">
                                                <td class="bold">&nbsp;</td>
                                                <td width="50%" class="bold"><asp:linkbutton ID="lnkName" Text="Name" CommandArgument="name" CssClass="bold" OnClick="OrderView" runat="server"/></td>
                                                <td width="50%" class="bold"><asp:linkbutton ID="lnkModified" Text="Last Modified" CommandArgument="modified" CssClass="bold" OnClick="OrderView" runat="server"/></td>
                                                <td class="bold" align="center"><asp:linkbutton ID="lnkEnabled" Text="Enabled" CommandArgument="enabled" CssClass="bold" OnClick="OrderView" runat="server"/></td>
                                            </tr>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr bgcolor="#EFEFEF" onmouseover="AdminMouseOver(this);" onmouseout="AdminMouseOut(this);" onclick="window.navigate('<%# Request.Path + "?id=" + DataBinder.Eval(Container.DataItem, "id")%>');" class="default">
                                            <td width="5%" align="left">&nbsp;<asp:ImageButton ID="btnDelete" OnClick="btnDeleteLink_Click" runat="server" ToolTip="Delete" ImageAlign="AbsMiddle" ImageUrl="/admin/images/delete.gif" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' /></td>
                                            <td width="50%">&nbsp;<%# DataBinder.Eval(Container.DataItem, "name") %></td>
                                            <td width="50%">&nbsp;<%# DataBinder.Eval(Container.DataItem, "modified") %></td>
                                            <td align="center"><asp:ImageButton ID="btnEnable" runat="server" ImageAlign="AbsMiddle" ImageUrl='<%# (DataBinder.Eval(Container.DataItem, "enabled").ToString() == "1") ? "/admin/images/enabled.gif" : "/admin/images/disabled.gif" %>' CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' OnClick="btnEnable_Click" /></td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </table>
                                    </FooterTemplate>
                                </asp:repeater>
                            </td>
                        </tr>
                    </table>
               </asp:Panel>

                <asp:Panel id="panAdd" runat="server" Visible="false">
                    <table width="95%" cellpadding="3" cellspacing="0" border="0" align="center">
                        <tr> 
                            <td class="default">Name:</td>
                            <td><asp:textbox ID="txtName" CssClass="default" runat="server" Width="300" MaxLength="100"/></td>
                        </tr>
                        <tr> 
                            <td class="default">Hardcoded Script:</td>
                            <td>
                                <asp:DropDownList ID="ddlHardcode" CssClass="default" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlHardcode_Change">
                                    <asp:ListItem Text="-- None --" Value="" />
                                    <asp:ListItem Text="ESM" Value="ESM" />
                                </asp:DropDownList>
                                &nbsp;(Optional)
                            </td>
                        </tr>
                        <tr> 
                            <td class="default" valign="top"><br />Execution:</td>
                            <td>
                                <table cellpadding="3" cellspacing="2" border="0">
                                    <tr>
                                        <td><asp:RadioButton ID="radLocal" runat="server" CssClass="default" Text="Local = Script is copied to target server and executed" GroupName="Run" /></td>
                                    </tr>
                                    <tr>
                                        <td><asp:RadioButton ID="radRemote" runat="server" CssClass="default" Text="Remote = Script is executed from ClearView server" GroupName="Run" /></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr> 
                            <td class="default">Language:</td>
                            <td><asp:DropDownList ID="ddlLanguage" CssClass="default" runat="server"/></td>
                        </tr>
                        <tr> 
                            <td class="default">Path:</td>
                            <td><asp:Label ID="lblPath" CssClass="default" runat="server"/></td>
                        </tr>
                        <tr> 
                            <td class="default">Upload:</td>
                            <td><asp:FileUpload ID="txtFile" runat="server" CssClass="default" Width="500" Height="18" /></td>
                        </tr>
                        <tr> 
                            <td class="default">Parameters:</td>
                            <td><asp:textbox ID="txtParameters" CssClass="default" runat="server" Width="300" MaxLength="100"/></td>
                        </tr>
                        <tr> 
                            <td class="default"></td>
                            <td>
                                <table cellpadding="3" cellspacing="2" border="0">
                                    <tr>
                                        <td colspan="2">Here is a list of popular parameters...if you are referencing any of these, be sure to use the proper variable.</td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">An example is if you are running script.exe and passing the &quot;Server Name&quot; into the script, the parameters should be &quot;%s&quot;.</td>
                                    </tr>
                                    <tr>
                                        <td nowrap><b>Reference</b></td>
                                        <td width="100%"><b>Parameter</b></td>
                                    </tr>
                                    <tr>
                                        <td nowrap>Server Name</td>
                                        <td>%s</td>
                                    </tr>
                                    <tr>
                                        <td nowrap>MHS Flag (1 = Yes, 0 = No)</td>
                                        <td>%mhs</td>
                                    </tr>
                                    <tr>
                                        <td nowrap>Audit ID</td>
                                        <td>%id</td>
                                    </tr>
                                    <tr>
                                        <td nowrap>IP Address</td>
                                        <td>%i</td>
                                    </tr>
                                    <tr>
                                        <td nowrap>TSM Flag (1 = Yes, 0 = No)</td>
                                        <td>%t</td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr> 
                            <td class="default">Timeout:</td>
                            <td><asp:textbox ID="txtTimeout" CssClass="default" runat="server" Width="100" MaxLength="10"/> [in minutes...set to 0 for UNLIMITED TIMEOUT]</td>
                        </tr>
                        <tr> 
                            <td class="default">Enabled:</td>
                            <td><asp:CheckBox ID="chkEnabled" runat="server" Checked="true" /></td>
                        </tr>
                        <tr><td height="5" colspan="2">&nbsp;</td></tr>
                        <tr> 
                            <td>&nbsp;</td>
                            <td>
                                <asp:button ID="btnAdd" CssClass="default" runat="server" Text="Add" Width="75" OnClick="btnAdd_Click" />
                                <asp:button ID="btnDelete" CssClass="default" runat="server" Text="Delete" Width="75" OnClick="btnDelete_Click" />
                                <asp:button ID="btnCancel" CssClass="default" runat="server" Text="Cancel" Width="75" OnClick="btnCancel_Click" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
    </table>
        </div>
        </td>
        </tr>
        </table>
    <input type="hidden" id="hdnId" runat="server" />
</form>
</body>
</html>
