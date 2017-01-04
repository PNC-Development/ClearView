<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="mnemonics.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.mnemonics" %>
</script>
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
		    <td><b>Mnemonics</b></td>
		    <td align="right">
		        <asp:LinkButton ID="btnImportRun" runat="server" CssClass="cmlink" Text="Import" OnClick="btnImportRun_Click" />&nbsp;&nbsp;&nbsp;
		        <asp:LinkButton ID="btnNew" runat="server" CssClass="cmlink" Text="Add New" OnClick="btnNew_Click" />
		    </td>
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
                                                <td width="5%" class="bold"><asp:linkbutton ID="lnkCode" Text="Code" CommandArgument="factory_code" CssClass="bold" OnClick="OrderView" runat="server"/></td>
                                                <td width="50%" class="bold"><asp:linkbutton ID="lnkName" Text="Name" CommandArgument="name" CssClass="bold" OnClick="OrderView" runat="server"/></td>
                                                <td width="20%" class="bold"><asp:linkbutton ID="lnkStatus" Text="Status" CommandArgument="status" CssClass="bold" OnClick="OrderView" runat="server"/></td>
                                                <td width="20%" class="bold"><asp:linkbutton ID="lnkModified" Text="Last Modified" CommandArgument="modified" CssClass="bold" OnClick="OrderView" runat="server"/></td>
                                                <td class="bold" align="center"><asp:linkbutton ID="lnkEnabled" Text="Enabled" CommandArgument="enabled" CssClass="bold" OnClick="OrderView" runat="server"/></td>
                                            </tr>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr bgcolor="#EFEFEF" onmouseover="AdminMouseOver(this);" onmouseout="AdminMouseOut(this);" onclick="if(event.srcElement.tagName != 'INPUT')window.navigate('<%# Request.Path + "?id=" + DataBinder.Eval(Container.DataItem, "id").ToString() %>');" class="default">
                                            <td width="5%" align="left">&nbsp;<asp:ImageButton ID="btnDelete" OnClick="btnDeleteLink_Click" runat="server" ToolTip="Delete" ImageAlign="AbsMiddle" ImageUrl="/admin/images/delete.gif" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' /></td>
                                            <td width="5%">&nbsp;<%# DataBinder.Eval(Container.DataItem, "factory_code") %></td>
                                            <td width="50%">&nbsp;<%# DataBinder.Eval(Container.DataItem, "name") %></td>
                                            <td width="20%">&nbsp;<%# DataBinder.Eval(Container.DataItem, "status") %></td>
                                            <td width="20%">&nbsp;<%# DataBinder.Eval(Container.DataItem, "modified") %></td>
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

                <asp:Panel ID="panAdd" runat="server" Visible="false">
                    <table width="95%" cellpadding="3" cellspacing="0" border="0" align="center">
                        <tr> 
                            <td class="default" width="100px">Name:</td>
                            <td><asp:textbox ID="txtName" CssClass="default" runat="server" Width="500" MaxLength="200"/></td>
                        </tr>
                        <tr> 
                            <td class="default" width="100px">Factory Code:</td>
                            <td><asp:textbox ID="txtFactoryCode" CssClass="default" runat="server" Width="100" MaxLength="10"/></td>
                        </tr>
                        <tr> 
                            <td class="default">Status:</td>
                            <td><asp:textbox ID="Status" CssClass="default" runat="server" Width="200" MaxLength="100"/></td>
                        </tr>
                        <tr> 
                            <td class="default">ResRating:</td>
                            <td><asp:textbox ID="ResRating" CssClass="default" runat="server" Width="200" MaxLength="100"/></td>
                        </tr>
                        <tr> 
                            <td class="default">DRRating:</td>
                            <td><asp:textbox ID="DRRating" CssClass="default" runat="server" Width="200" MaxLength="100"/></td>
                        </tr>
                        <tr> 
                            <td class="default">Infrastructure:</td>
                            <td><asp:textbox ID="Infrastructure" CssClass="default" runat="server" Width="200" MaxLength="100"/></td>
                        </tr>
                        <tr> 
                            <td class="default">CriticalityFactor:</td>
                            <td><asp:textbox ID="CriticalityFactor" CssClass="default" runat="server" Width="200" MaxLength="100"/></td>
                        </tr>
                        <tr> 
                            <td class="default">Platform:</td>
                            <td><asp:textbox ID="Platform" CssClass="default" runat="server" Width="200" MaxLength="100"/></td>
                        </tr>
                        <tr> 
                            <td class="default">CICS:</td>
                            <td><asp:textbox ID="CICS" CssClass="default" runat="server" Width="200" MaxLength="100"/></td>
                        </tr>
                        <tr> 
                            <td class="default">PagerLevel:</td>
                            <td><asp:textbox ID="PagerLevel" CssClass="default" runat="server" Width="200" MaxLength="100"/></td>
                        </tr>
                        <tr> 
                            <td class="default">ATLName:</td>
                            <td><asp:textbox ID="ATLName" CssClass="default" runat="server" Width="200" MaxLength="100"/></td>
                        </tr>
                        <tr> 
                            <td class="default">PMName:</td>
                            <td><asp:textbox ID="PMName" CssClass="default" runat="server" Width="200" MaxLength="100"/></td>
                        </tr>
                        <tr> 
                            <td class="default">FMName:</td>
                            <td><asp:textbox ID="FMName" CssClass="default" runat="server" Width="200" MaxLength="100"/></td>
                        </tr>
                        <tr> 
                            <td class="default">DMName:</td>
                            <td><asp:textbox ID="DMName" CssClass="default" runat="server" Width="200" MaxLength="100"/></td>
                        </tr>
                        <tr> 
                            <td class="default">CIO:</td>
                            <td><asp:textbox ID="CIO" CssClass="default" runat="server" Width="200" MaxLength="100"/></td>
                        </tr>
                        <tr> 
                            <td class="default">AppOwner:</td>
                            <td><asp:textbox ID="AppOwner" CssClass="default" runat="server" Width="200" MaxLength="100"/></td>
                        </tr>
                        <tr> 
                            <td class="default">AppLOBName:</td>
                            <td><asp:textbox ID="AppLOBName" CssClass="default" runat="server" Width="200" MaxLength="100"/></td>
                        </tr>
                        <tr> 
                            <td class="default">Segment1:</td>
                            <td><asp:textbox ID="Segment1" CssClass="default" runat="server" Width="200" MaxLength="100"/></td>
                        </tr>
                        <tr> 
                            <td class="default">RiskManager:</td>
                            <td><asp:textbox ID="RiskManager" CssClass="default" runat="server" Width="200" MaxLength="100"/></td>
                        </tr>
                        <tr> 
                            <td class="default">BRContact:</td>
                            <td><asp:textbox ID="BRContact" CssClass="default" runat="server" Width="200" MaxLength="100"/></td>
                        </tr>
                        <tr> 
                            <td class="default">AppRating:</td>
                            <td><asp:textbox ID="AppRating" CssClass="default" runat="server" Width="200" MaxLength="100"/></td>
                        </tr>
                        <tr> 
                            <td class="default">Source:</td>
                            <td><asp:textbox ID="Source" CssClass="default" runat="server" Width="200" MaxLength="100"/></td>
                        </tr>
                        <tr> 
                            <td class="default">OriginalApp:</td>
                            <td><asp:textbox ID="OriginalApp" CssClass="default" runat="server" Width="200" MaxLength="100"/></td>
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
                
                <asp:Panel ID="panImport" runat="server" Visible="false">
                    <table width="95%" cellpadding="3" cellspacing="0" border="0" align="center">
                        <tr> 
                            <td>
                                <asp:button ID="btnImport" CssClass="default" runat="server" Text="Import" Width="75" OnClick="btnImport_Click" />&nbsp;
                                <asp:button ID="btnCancel2" CssClass="default" runat="server" Text="Back" Width="75" OnClick="btnCancel_Click" />&nbsp;
                            </td>
                            <td align="right" class="bold">
                                <div id="divWait" runat="server" style="display:none">
                                    <img src="/images/spacer.gif" border="0" width="20" height="1" /><img src="/images/please_wait.gif" border="0" align="absmiddle" /> Please Wait...
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table width="100%" border="0" cellpadding="5" cellspacing="0">
                                    <tr>
                                        <td class="bold">Previous Imports</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:repeater ID="rptImport" runat="server">
                                                <HeaderTemplate>
                                                    <table width="100%" cellpadding="3" cellspacing="0" border="0" align="center">
                                                        <tr bgcolor="#CCCCCC">
                                                            <td width="10%" class="bold">ID</td>
                                                            <td width="90%" class="bold">Modified</td>
                                                        </tr>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <tr bgcolor="<%# DataBinder.Eval(Container.DataItem, "id").ToString() == Request.QueryString["import"] ? "#99FF99" : "#EFEFEF" %>" class="default">
                                                        <td width="10%">&nbsp;<%# "<a href=\"" + Request.Path + "?import=" + DataBinder.Eval(Container.DataItem, "id").ToString() + "\">" + DataBinder.Eval(Container.DataItem, "id") + "</a>"%></td>
                                                        <td width="90%">&nbsp;<%# DataBinder.Eval(Container.DataItem, "created") %></td>
                                                    </tr>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    </table>
                                                </FooterTemplate>
                                            </asp:repeater>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2"><asp:TextBox ID="txtImport" runat="server" CssClass="default" Width="100%" Rows="50" TextMode="multiLine" /></td>
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
</form>
</body>
</html>
