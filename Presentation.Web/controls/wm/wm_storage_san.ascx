<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wm_storage_san.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.wm_storage_san" %>

<script type="text/javascript">
    function ChangeFrame(oCell, oShow, oHide1, oHidden, strHidden) {
        oShow = document.getElementById(oShow);
        oShow.style.display = "inline";
        oHide1 = document.getElementById(oHide1);
        oHide1.style.display = "none";
    	var oRow = oCell.parentElement;
	    for (var yy=0; yy<oRow.children.length; yy++) {
    		var oNot = oRow.getElementsByTagName("td").item(yy);
    		if (oNot.className == "cmbutton")
                oNot.style.border = "1px solid #94a6b5"
    	}
	    oCell.style.borderTop = "3px solid #6A8359"
        oCell.style.borderBottom = "1px solid #FFFFFF"
        oHidden = document.getElementById(oHidden);
        oHidden.value = strHidden;
    }
</script>
<asp:Panel ID="panWorkload" runat="server" Visible="false">
    <table width="100%" border="0" cellSpacing="0" cellPadding="0" class="default">    
        <tr>
            <td>
                <table width="100%" border="0" cellSpacing="0" cellPadding="0">
                    <tr>
                        <td><asp:ImageButton ID="btnSave" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/tool_save.gif" OnClick="btnSave_Click" /></td>
                        <td><asp:ImageButton ID="btnPrint" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/tool_print.gif" /></td>
                        <td><asp:ImageButton ID="btnComplete" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/tool_complete.gif" OnClick="btnComplete_Click" /></td>
                        <td width="100%">
                            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                <tr>
                                    <td><img src="/images/tool_left.gif" border="0" width="6" height="26"></td>
                                    <td nowrap background="/images/tool_back.gif" width="100%"><img src="/images/tool_back.gif" border="0" width="2" height="26"></td>
                                    <td><img src="/images/tool_right.gif" border="0" width="6" height="26"></td>
                                </tr>
                            </table>
                        </td>
                        <td><asp:ImageButton ID="btnClose" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/tool_close.gif" /></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="default">
                <table width="100%" border="0" cellSpacing="2" cellPadding="2" class="default">
                    <tr>
                        <td valign="top">
                            <table border="0" cellpadding="3" cellspacing="2">
                                <tr>
                                    <td nowrap><b>Name:</b></td>
                                    <td><asp:Label ID="lblName" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td nowrap><b>Number:</b></td>
                                    <td><asp:Label ID="lblNumber" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td nowrap><b>Type:</b></td>
                                    <td><asp:Label ID="lblType" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td nowrap><b>Service:</b></td>
                                    <td><asp:Label ID="lblService" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td nowrap><b>Requested By:</b></td>
                                    <td><asp:Label ID="lblRequestedBy" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td nowrap><b>Requested On:</b></td>
                                    <td><asp:Label ID="lblRequestedOn" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td nowrap><b>Last Updated:</b></td>
                                    <td><asp:Label ID="lblUpdated" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td nowrap><b>Custom Task Name:</b></td>
                                    <td><asp:TextBox ID="txtCustom" runat="server" CssClass="default" Width="300" MaxLength="100" /></td>
                                </tr>
                            </table>
                        </td>
                        <td valign="top" align="right">
                            <table border="0" cellpadding="3" cellspacing="2">
                                <tr>
                                    <td colspan="2"><iframe src="/frame/did_you_know.aspx" frameborder="0" scrolling="no" width="300" height="135"></iframe></td>
                                </tr>
                                <tr>
                                    <td nowrap><b>Overall Status:</b></td>
                                    <td><asp:Label ID="lblStatus" runat="server" CssClass="default" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <asp:Panel ID="panComplete" runat="server" Visible="false">
                    <table border="0" cellSpacing="2" cellPadding="2" style="border:solid 1px #999999" bgcolor="#f9f9f9">
                        <tr>
                            <td class="bigreddefault"><img src="/images/bigAlert.gif" border="0" align="absmiddle" /> <b>NOTE: Don't forget to click the <img src="/images/tool_complete.gif" border="0" align="absmiddle" /> button to finish this task!!!</b></td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
        <tr height="16">
            <td><img src="/images/spacer.gif" border="0" height="16" width="1" /></td>
        </tr>
        <tr>
            <td>
                <table width="100%" cellpadding="4" cellspacing="3" border="0">
                    <tr>
                        <td><hr size="1" noshade /></td>
                    </tr>
                </table>
                <table width="100%" cellpadding="4" cellspacing="3" border="0">
                    <tr>
                        <td>Finished Configuring Storage: <b>Total Amount</b> = <asp:TextBox ID="txtAmount" runat="server" CssClass="default" Width="100" MaxLength="20" /> GB</td>
                    </tr>
                    <tr>
                        <td><%=strDetails %></td>
                    </tr>
                </table>
            </td>
        </tr>
         <tr>
            <td>
                 <table width="100%" cellpadding="4" cellspacing="3" border="0">
                        <tr>
                            <td colspan="2">&nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="2" class="greentableheader">Status Updates</td>
                        </tr>
                        <tr>
                            <td nowrap>Status:</td>
                            <td style="width: 98%">
                                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="default" >
                                    <asp:ListItem Text="-- SELECT --" Value="0" />
                                    <asp:ListItem Text="Red" Value="1" />
                                    <asp:ListItem Text="Yellow" Value="2" />
                                    <asp:ListItem Text="Green" Value="3" />
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2"><b>Comments / Issues:</b></td>
                        </tr>
                        <tr>
                            <td colspan="2"><asp:TextBox ID="txtComments" runat="server" CssClass="default" Width="100%" TextMode="MultiLine" Rows="13" /></td>
                        </tr>
                        <tr>
                            <td colspan="2">&nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table width="100%" cellpadding="3" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                                    <tr bgcolor="#EEEEEE">
                                        <td nowrap><b>Date</b></td>
                                        <td nowrap>&nbsp;</td>
                                        <td nowrap><b>Status</b></td>
                                        <td nowrap>&nbsp;</td>
                                        <td nowrap><b>Comments</b></td>
                                    </tr>
                                    <asp:repeater ID="rptStatus" runat="server">
                                        <ItemTemplate>
                                            <tr class="default">
                                                <td nowrap valign="top"><a href="javascript:void(0);" onclick="ShowStatus('<%# DataBinder.Eval(Container.DataItem, "id") %>','r');"><%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "modified").ToString()).ToShortDateString() %>&nbsp;<%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "modified").ToString()).ToShortTimeString() %></a></td>
                                                <td nowrap>&nbsp;</td>
                                                <td nowrap valign="top"><asp:Label ID="lblStatus" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "status") %>' /></td>
                                                <td nowrap>&nbsp;</td>
                                                <td width="100%" valign="top" >
                                                    <div id="div200_<%# DataBinder.Eval(Container.DataItem, "id") %>" style="display:inline">
                                                        <%# (DataBinder.Eval(Container.DataItem, "comments").ToString().Length > 200 ? DataBinder.Eval(Container.DataItem, "comments").ToString().Substring(0, 200) + " ...&nbsp;&nbsp;&nbsp;[<a href=\"javascript:void(0);\" onclick=\"ShowHideDiv('div200_" + DataBinder.Eval(Container.DataItem, "id") + "', 'none');ShowHideDiv('divMore_" + DataBinder.Eval(Container.DataItem, "id") + "', 'inline');\">More</a>]" : DataBinder.Eval(Container.DataItem, "comments").ToString())%>
                                                    </div>
                                                    <div id="divMore_<%# DataBinder.Eval(Container.DataItem, "id") %>" style="display:none">
                                                        <%# DataBinder.Eval(Container.DataItem, "comments").ToString() + "&nbsp;&nbsp;&nbsp;[<a href=\"javascript:void(0);\" onclick=\"ShowHideDiv('div200_" + DataBinder.Eval(Container.DataItem, "id") + "', 'inline');ShowHideDiv('divMore_" + DataBinder.Eval(Container.DataItem, "id") + "', 'none');\">Hide</a>]"%>
                                                    </div>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:repeater>
                                    <tr>
                                        <td colspan="5">
                                            <asp:Label ID="lblNoStatus" runat="server" CssClass="default" Visible="false" Text="<img src='/images/alert.gif' border='0' align='absmiddle'> There are no updates" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
            </td>
        </tr>
    </table>
</asp:Panel>
<asp:Panel ID="panDenied" runat="server" Visible="false">
    <table width="100%" cellpadding="2" cellspacing="2" border="0">
        <tr>
            <td><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
            <td class="header"><img src="/images/bigX.gif" border="0" align="absmiddle" /> Access Denied</td>
        </tr>
        <tr><td colspan="2">&nbsp;</td></tr>
        <tr>
            <td><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
            <td>You do not have rights to view this item.</td>
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
                        <td align="right"><asp:Button ID="btnDenied" runat="server" CssClass="default" Width="75" Text="Close" /></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Panel>
<asp:Label ID="lblResourceWorkflow" runat="server" Visible="false" />
<asp:Label ID="lblMidrange" runat="server" Visible="false" />
<asp:HiddenField ID="hdnTab" runat="server" />
<asp:HiddenField ID="hdnDBA" runat="server" />
