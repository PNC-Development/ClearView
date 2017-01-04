<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="forecast_acquisition_costs.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.forecast_acquisition_costs" %>


<script type="text/javascript">
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
		    <td><b>Acquisition Costs</b></td>
		    <td align="right"></td>
		</tr>
		<tr><td colspan="2">&nbsp;</td></tr>
		<tr>
		    <td colspan="2" align="center">
		        <asp:Panel ID="panAll" runat="server" Visible="false">
                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <asp:TreeView ID="oTreeview" runat="server" ShowLines="true" NodeIndent="35">
                                <NodeStyle CssClass="default" />
                            </asp:TreeView>
                        </td>
                    </tr>
                </table>
                </asp:Panel>

                <asp:Panel ID="panView" runat="server" Visible="false">
                    <table width="95%" cellpadding="3" cellspacing="0" border="0" align="center">
                        <tr>
                            <td class="default">Model:</td>
                            <td><asp:Label ID="lblName" runat="server" CssClass="default" /></td>
                        </tr>
                        <tr>
                            <td class="default">Line Item:</td>
                            <td><asp:DropDownList ID="ddlLineItem" CssClass="default" runat="server"/></td>
                        </tr>
                        <tr> 
                            <td class="default">Cost:</td>
                            <td><asp:textbox ID="txtCost" CssClass="default" runat="server" Width="125" MaxLength="15" /> Format: 13250.75</td>
                        </tr>
                        <tr> 
                            <td class="default">Production ONLY:</td>
                            <td><asp:CheckBox ID="chkProduction" runat="server" CssClass="default" /> Cost only associated with assets in production</td>
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
                                <asp:button ID="btnCancel" CssClass="default" runat="server" Text="Cancel" Width="75" OnClick="btnCancel_Click" />
                            </td>
                        </tr>
                    </table>
                    <p>&nbsp;</p>
                    <table width="100%" cellpadding="4" cellspacing="0" border="0" align="center"  style="border:solid 1px #CCCCCC">
                        <tr bgcolor="#EEEEEE">
                            <td></td>
                            <td><b><u>Line Item:</u></b></td>
                            <td><b><u>Cost:</u></b></td>
                            <td align="center"><b><u>Prod ONLY:</u></b></td>
                            <td align="right"></td>
                        </tr>
                        <asp:repeater ID="rptAll" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td valign="top">[<asp:LinkButton ID="btnEdit" runat="server" Text="Edit" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' OnClick="btnEdit_Click" />]</td>
                                    <td valign="top"><%# DataBinder.Eval(Container.DataItem, "name") %></td>
                                    <td valign="top"><%# double.Parse(DataBinder.Eval(Container.DataItem, "cost").ToString()).ToString("N") %></td>
                                    <td valign="top" align="center"><img src='<%# (DataBinder.Eval(Container.DataItem, "prod").ToString() == "1") ? "/images/go.gif" : "/images/cancel.gif" %>' align="absmiddle" border="0" /></td>
                                    <td valign="top" align="right">[<asp:LinkButton ID="btnDelete" runat="server" Text="Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' OnClick="btnDelete_Click" />]</td>
                                </tr>
                            </ItemTemplate>
                            <AlternatingItemTemplate>
                                <tr bgcolor="F6F6F6">
                                    <td valign="top">[<asp:LinkButton ID="btnEdit" runat="server" Text="Edit" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' OnClick="btnEdit_Click" />]</td>
                                    <td valign="top"><%# DataBinder.Eval(Container.DataItem, "name") %></td>
                                    <td valign="top"><%# double.Parse(DataBinder.Eval(Container.DataItem, "cost").ToString()).ToString("N") %></td>
                                    <td valign="top" align="center"><img src='<%# (DataBinder.Eval(Container.DataItem, "prod").ToString() == "1") ? "/images/go.gif" : "/images/cancel.gif" %>' align="absmiddle" border="0" /></td>
                                    <td valign="top" align="right">[<asp:LinkButton ID="btnDelete" runat="server" Text="Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' OnClick="btnDelete_Click" />]</td>
                                </tr>
                            </AlternatingItemTemplate>
                        </asp:repeater>
                        <tr>
                            <td colspan="5">
                                <asp:Label ID="lblNone" runat="server" CssClass="default" Visible="false" Text="<img src='/images/spacer.gif' border='0' height='1' width='25' /><img src='/images/alert.gif' border='0' align='absmiddle'> There are no items associated to this model" />
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
