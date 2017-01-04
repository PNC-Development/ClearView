<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="whats_new.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.whats_new" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
<title>ClearView Administration</title>
<link rel="stylesheet" type="text/css" href="/css/default.css" />
<script type="text/javascript" src="/javascript/global.js"></script>
<script type="text/javascript">
    function Edit(strId) {
        var strURL = '<%=Request.Path %>';
        window.navigate(strURL + "?id=" + strId);
    }
</script>
</head>
<body style="margin-top:0; margin-left:0" >
<form id="Form1" runat="server">
    <table width="100%" border="0" cellspacing="0" cellpadding="0" style="text-align:left ;height:100%">
    <tr>
    <td height="100%">
        <div style="height:100%; overflow:auto">
                <table width="98%" cellpadding="0" cellspacing="0" border="0" style="text-align:left">
		                <tr><td colspan="2">&nbsp;</td></tr>
		                <tr> 
		                    <td><b>What's New</b></td>
		                    <td align="right"><asp:LinkButton ID="btnNew" runat="server" Text="Add New" OnClick="btnNew_Click" /></td>
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
                                                        <table width="100%" cellpadding="3" cellspacing="0" border="0" style="text-align:left">
                                                            <tr bgcolor="#CCCCCC">
                                                                <td class="bold">&nbsp;</td>
                                                                <td width="50%" class="bold"><asp:linkbutton ID="lnkName" Text="Name" CommandArgument="title" CssClass="bold" OnClick="OrderView" runat="server"/></td>
                                                                <td width="50%" class="bold"><asp:linkbutton ID="lnkModified" Text="Last Modified" CommandArgument="modified" CssClass="bold" OnClick="OrderView" runat="server"/></td>
                                                                <td class="bold" align="center"><asp:linkbutton ID="lnkEnabled" Text="Enabled" CommandArgument="enabled" CssClass="bold" OnClick="OrderView" runat="server"/></td>
                                                            </tr>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <tr bgcolor="#EFEFEF" onmouseover="AdminMouseOver(this);" onmouseout="AdminMouseOut(this);" onclick="if(event.srcElement.tagName != 'INPUT')Edit('<%# DataBinder.Eval(Container.DataItem, "id") %>');" class="default">
                                                            <td width="5%" align="left">&nbsp;<asp:ImageButton ID="btnDelete" OnClick="btnDeleteLink_Click" runat="server" ToolTip="Delete" ImageAlign="AbsMiddle" ImageUrl="/admin/images/delete.gif" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' /></td>
                                                            <td width="50%">&nbsp;<%# DataBinder.Eval(Container.DataItem, "title") %></td>
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
                                    <table width="95%" cellpadding="3" cellspacing="0" border="0" style="text-align:left">
                                        <tr> 
                                            <td class="default" width="100px">Title:</td>
                                            <td><asp:textbox ID="txtTitle" CssClass="default" runat="server" Width="90%" MaxLength="100"/></td>
                                        </tr>
                                        <tr> 
                                            <td class="default" width="100px">Description:</td>
                                            <td><asp:textbox ID="txtDescription" CssClass="default" runat="server" Width="90%" TextMode="MultiLine" Rows="15" /></td>
                                        </tr>
                                        <tr> 
                                            <td class="default" width="100px">Attachment:</td>
                                            <td><asp:textbox ID="txtAttachment" CssClass="default" runat="server" Width="90%" MaxLength="100"/> <asp:button ID="btnBrowse" runat="server" Text="..." Width="25" CssClass="default"/></td>
                                        </tr>
                                         <tr> 
                                            <td class="default" width="100px">Version:</td>
                                            <td><asp:textbox ID="txtVersion" CssClass="default" runat="server" Width="90%" MaxLength="25"/></td>
                                        </tr>
                                         <tr> 
                                            <td class="default" width="100px">Category:</td>
                                            <td><asp:textbox ID="txtCategory" CssClass="default" runat="server" Width="90%" Text="News" Enabled="false" MaxLength="100"/></td>
                                        </tr>
                                        <tr> 
                                            <td class="default">Enabled:</td>
                                            <td><asp:CheckBox ID="chkEnabled" runat="server" Checked="true" /></td>
                                        </tr>
                                        <tr> 
                                            <td class="cmdefault">&nbsp;</td>
                                            <td><asp:Button ID="btnOrder" runat="server" Text="Change Order" Width="150" CssClass="default" /></td>
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
    <input type="hidden" id="hdnOrder" runat="server" />
</form>
</body>
</html>
