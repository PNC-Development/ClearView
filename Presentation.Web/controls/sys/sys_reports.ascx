<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="sys_reports.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.sys_reports" %>


<script type="text/javascript">
	var strAboutUrl = "";
	var strReportUrl = "";
	var oResizeTD = null;
	var boolResize = null;
	var intResizeWidth = null;
	var oLine = null;
	var oLineCover = null;
	var oFrameCover = null;
    function Loader()
    {
	    boolResize = false;
	    oLineCover = window.top.document.getElementById('divLineCover');
	    oFrameCover = window.top.document.getElementById('frmLiveCover');
	    oLine = window.top.document.getElementById('divLine');
        var strLeftNavSize = GetCookie("reportNavSize");
        if (strLeftNavSize != "")
        {
            intResizeWidth = parseInt(strLeftNavSize);
		    DoResize();
        }
    }
	function ShowResize() {
		window.top.document.body.scroll = "NO";
		oLineCover.style.posLeft = 0;
		oLineCover.style.display = "inline";
		oLineCover.style.posTop = window.top.document.body.scrollTop;
		oLineCover.style.width = '100%';
		oLineCover.style.height = '100%';
		DHTMLHelp(oLineCover, oFrameCover);
	    oLine.style.posLeft = event.clientX;
	    oLine.style.posTop = 0;
	    oLine.style.display = 'inline';
	    document.attachEvent("onmousemove", MoveLine);
	    document.attachEvent("onmouseup", EndResize);
	    boolResize = true;
	    MoveLine();
	}
	function DefaultSize() {
        intResizeWidth = 250;
	    document.detachEvent("onmouseup", EndResize);
	    document.detachEvent("onmousemove", MoveLine);
	    boolResize = false;
        var oTD = window.top.document.getElementById("tdResize");
	    oTD.width = intResizeWidth;
	    oLine.style.display = "none";
	    oLineCover.style.display = "none";
	    oFrameCover.style.display = "none";
	    SetCookie("reportNavSize", intResizeWidth);
	}
    function MoveLine() {
        if (boolResize == true) {
	        intResizeWidth = event.clientX - 3;
	        if (intResizeWidth < 200)
		        intResizeWidth = 200;
	        var intWindowWidth = parseInt(window.top.document.body.clientWidth);
	        if (intResizeWidth > intWindowWidth - 400)
	            intResizeWidth = intWindowWidth - 400
	        oLine.style.posLeft =  intResizeWidth;
        }
    }
    function EndResize() {
	    document.detachEvent("onmouseup", EndResize);
	    document.detachEvent("onmousemove", MoveLine);
	    if (boolResize == true) {
		    boolResize = false;
		    DoResize();
	    }
    }
    function EndResizeMouseLeave() {
	    document.detachEvent("onmouseup", EndResize);
	    document.detachEvent("onmousemove", MoveLine);
	    if (boolResize == true) {
		    boolResize = false;
		    DoResize();
	    }
    }
    function DoResize() {
        var oTD = window.top.document.getElementById("tdResize");
	    var oHolder = document.getElementById("holder");
        var intHolder = parseInt(findPosX(oHolder));
	    oTD.width = intResizeWidth - intHolder;
	    oLine.style.display = "none";
	    oLineCover.style.display = "none";
	    oFrameCover.style.display = "none";
	    SetCookie("reportNavSize", intResizeWidth);
    }
	function NewMaximize(strUrl) {
		if (strReportUrl == "" && strUrl == "")
		    alert('Please select a report');
		else {
		    if (strUrl == "")
		        window.open("/frame/loading.htm?referrer=" + strReportUrl,"_blank","height=600,width=800,menubar=no,resizable=yes,scrollbars=no,status=no,toolbar=no");
	        else
		        window.open("/frame/loading.htm?referrer=" + strUrl,"_blank","height=600,width=800,menubar=no,resizable=yes,scrollbars=no,status=no,toolbar=no");
	    }
		return false;
	}
	function SelectReport(strUrl, oFrame, strReport, oHidden) {
		oFrame = document.getElementById(oFrame);
		oFrame.contentWindow.navigate(strUrl);
		strReportUrl = strUrl;
		if (strReport != "")
		    strAboutUrl = "?id=" + strReport;
	    else
		    strAboutUrl = "";
		oHidden = document.getElementById(oHidden);
		oHidden.value = strReport;
	}
	function ShowAbout() {
		if (strAboutUrl == "")
			alert('Please select a report!');
		else
			OpenWindow("REPORT_ABOUT", strAboutUrl);
	}
	function CheckReport(oHidden, strConfirm) {
		oHidden = document.getElementById(oHidden);
		if (oHidden.value == "") {
		    alert('Please select a report');
		    return false;
		}
		else {
		    if (strConfirm == "")
		        return true;
		    else
		        return confirm(strConfirm);
		}
	}
</script>
<asp:Panel ID="panReport" runat="server" Visible="false">
<table width="100%" height="100%" cellpadding="0" cellspacing="0" border="0" class="default">
    <tr height="1">
        <td><img src="/images/table_topleft.gif" border="0" width="5" height="26"></td>
        <td nowrap background="/images/table_top.gif" class="greentableheader" width="100%"><asp:Label ID="lblTitle" runat="server" CssClass="greentableheader" /></td>
        <td nowrap background="/images/table_top.gif" class="greentableheader" nowrap><asp:TextBox ID="txtSearch" runat="server" CssClass="default" Width="200" /> <asp:Button ID="btnSearch" runat="server" CssClass="default" Width="75" Text="Search" OnClick="btnSearch_Click" /></td>
        <td><img src="/images/table_topright.gif" border="0" width="5" height="26"></td>
    </tr>
    <tr>
        <td background="/images/table_left.gif"><img src="/images/table_left.gif" width="5" height="10"></td>
        <td width="100%" colspan="2" bgcolor="#FFFFFF" valign="top">
<table width="100%" height="100%" cellpadding="0" cellspacing="0" border="0">
	<tr>
		<td width="2"><img id="holder" src="/images/spacer.gif" width="2" height="1" border="0" /></td>
		<td>
			<table id="tdResize" width="250" height="100%" cellpadding="0" cellspacing="2" border="0">
				<tr>
					<td height="100%" valign="top">
						<table width="100%" height="100%" cellpadding="2" cellspacing="0" border="0">
				            <tr>
					            <td class="framegreen">ClearView Reports</td>
				            </tr>
				            <tr>
					            <td height="100%" style="border:solid 1px #CCCCCC">
						            <div style="width:100%; height:100%; overflow:auto">
                                        <asp:TreeView ID="oTreeview" runat="server" ShowLines="true" NodeIndent="15">
                                            <NodeStyle CssClass="default" />
                                        </asp:TreeView>
						            </div>
					            </td>
				            </tr>
						</table>
					</td>
				</tr>
				<tr height="1">
					<td width="100%" valign="top">
						<table width="100%" cellpadding="2" cellspacing="0" border="0">
				            <tr>
					            <td class="framegreen">Actions</td>
				            </tr>
				            <tr>
					            <td style="border:solid 1px #CCCCCC">
						            <table width="100%" cellpadding="2" cellspacing="2" border="0">
							            <tr>
								            <td><img src="/images/order.gif" border="0" align="absmiddle" /></td>
								            <td width="100%"><asp:LinkButton ID="lnkOrder" runat="server" Text="Order a Report" OnClick="lnkOrder_Click" /></td>
							            </tr>
							            <tr>
								            <td><img src="/images/check.gif" border="0" align="absmiddle" /></td>
								            <td width="100%"><asp:LinkButton ID="lnkAdd" runat="server" Text="Add Report to Favorites" OnClick="lnkAdd_Click" /></td>
							            </tr>
							            <tr>
								            <td><img src="/images/delete.gif" border="0" align="absmiddle" /></td>
								            <td width="100%"><asp:LinkButton ID="lnkRemove" runat="server" Text="Remove Report from Favorites" OnClick="lnkRemove_Click" /></td>
							            </tr>
						            </table>
					            </td>
				            </tr>
						</table>
					</td>
				</tr>
			</table>
		</td>
		<td onmousedown="ShowResize()" ondblclick="DefaultSize()" style="cursor:e-resize" width="2"><img src="/images/spacer.gif" width="2" height="1" border="0" /></td>
		<td width="100%">
			<table width="100%" height="100%" cellpadding="0" cellspacing="2" border="0">
				<tr>
					<td valign="top">
						<table width="100%" height="100%" cellpadding="2" cellspacing="0" border="0">
							<tr height="1">
								<td class="framegreen">Preview</td>
								<td class="framegreen" align="right">
            						<a href="javascript:void(0);" onclick="ShowAbout();"><img src="/images/about.gif" border="0" align="absmiddle" title="About this Report"/></a> <asp:ImageButton ID="imgMaximize" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/maximize.gif" ToolTip="Maximize Window" /><div style="display:none"><a href="javascript:void(0);" onclick="NewMaximize();"><img id="imgPreview" src="/images/maximize.gif" border="0" align="absmiddle" title="Maximize Window"/></a></div>
								</td>
							</tr>
				            <tr>
					            <td colspan="2" style="border:solid 1px #CCCCCC"><iframe id="frmReport" frameborder="0" scrolling="no" width="100%" height="100%"></iframe></td>
				            </tr>
						</table>
					</td>
				</tr>
			</table>
		</td>
		<td width="2"><img src="/images/spacer.gif" width="2" height="1" border="0" /></td>
	</tr>
</table>
        </td>
        <td background="/images/table_right.gif"><img src="/images/table_right.gif" width="5" height="10"></td>
    </tr>
    <tr height="1">
        <td><img src="/images/table_bottomLeft.gif" border="0" width="5" height="9"></td>
        <td width="100%" colspan="2" background="/images/table_bottom.gif"></td>
        <td><img src="/images/table_bottomRight.gif" border="0" width="5" height="9"></td>
    </tr>
</table>
</asp:Panel>
<asp:Panel ID="panNoForm" runat="server" Visible="false">
<table width="100%" cellpadding="0" cellspacing="0" border="0">
    <tr height="1">
        <td><img src="/images/table_topleft.gif" border="0" width="5" height="26"></td>
        <td nowrap background="/images/table_top.gif" class="greentableheader" width="100%">Report Order</td>
        <td><img src="/images/table_topright.gif" border="0" width="5" height="26"></td>
    </tr>
    <tr>
        <td background="/images/table_left.gif"><img src="/images/table_left.gif" width="5" height="10"></td>
        <td width="100%" bgcolor="#FFFFFF" valign="top">
<asp:Panel ID="panOrder" runat="server" Visible="false">
<table width="100%" cellpadding="4" cellspacing="2" border="0">
	<tr>
		<td class="hugeheader"><img src="/images/contact.gif" border="0" align="absmiddle" /> <b>Order a Report</b></td>
	</tr>
    <tr>
        <td>Please enter your requirements for the report:</td>
    </tr>
    <tr>
        <td><asp:TextBox ID="txtOrder" runat="server" CssClass="default" TextMode="MultiLine" Rows="20" Width="100%" /></td>
    </tr>
	<tr>
	    <td>&nbsp;</td>
	</tr>
    <tr>
        <td align="right">
            <asp:Button ID="btnOrder" runat="server" CssClass="default" Text="Submit" Width="75" OnClick="btnOrder_Click" Enabled="false" /> 
            <asp:Button ID="btnCancel" runat="server" CssClass="default" Text="Cancel" Width="75" OnClick="btnCancel_Click" />
        </td>
    </tr>
</table>
</asp:Panel>
<asp:Panel ID="panSearch" runat="server" Visible="false">
<table width="100%" cellpadding="4" cellspacing="2" border="0">
	<tr>
		<td class="hugeheader"><img src="/images/search_report.gif" border="0" align="absmiddle" /> <b>Search Results</b></td>
		<td align="right"><asp:LinkButton ID="btnBack" runat="server" OnClick="btnCancel_Click" Text="<img src='/images/arrow-left.gif' border='0' align='absmiddle'/><img src='/images/spacer.gif' border='0' width='10' height='20' align='absmiddle'/>Back to Reports" /></td>
	</tr>
	<tr>
	    <td>
		    <asp:TextBox ID="txtSearch2" runat="server" CssClass="default" Width="300" /> <asp:Button ID="btnSearch2" runat="server" CssClass="default" Width="75" Text="Search" OnClick="btnSearch2_Click" />
	    </td>
	    <td align="right"><asp:CheckBox ID="chkPreview" runat="server" CssClass="default" Text="Show Report Preview" OnCheckedChanged="chkPreview_Change" AutoPostBack="true" /></td>
	</tr>
	<tr>
	    <td colspan="2">
            <table width="100%" cellpadding="7" cellspacing="0" border="0" align="center" style="border:solid 1px #CCCCCC">
            <asp:Panel ID="panViewPreview" runat="server" Visible="false">
            <asp:repeater ID="rptViewPreview" runat="server">
                <HeaderTemplate>
                    <tr bgcolor="#EEEEEE">
                        <td></td>
                        <td><asp:LinkButton ID="btnOrderTitle" runat="server" CssClass="tableheader" Text="<b>Title</b>" OnClick="btnOrder_Click" CommandArgument="title" /></td>
                        <td><asp:LinkButton ID="btnOrderDescription" runat="server" CssClass="tableheader" Text="<b>Description</b>" OnClick="btnOrder_Click" CommandArgument="description" /></td>
                        <td><asp:LinkButton ID="btnOrderModified" runat="server" CssClass="tableheader" Text="<b>Modified</b>" OnClick="btnOrder_Click" CommandArgument="modified" /></td>
                    </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class="default" onmouseover="CellRowOver(this);" onmouseout="CellRowOut(this);" onclick="NewMaximize('/frame/loading.htm?referrer=/frame/report.aspx?r=<%# oFunction.encryptQueryString(DataBinder.Eval(Container.DataItem, "reportid").ToString()) %>');">
                        <td nowrap valign="top"><iframe src='/frame/report_preview.aspx?r=<%# oFunction.encryptQueryString(DataBinder.Eval(Container.DataItem, "reportid").ToString()) %>' width='150' height='100' frameborder='0' scrolling='no' style='border:solid 1px #CCCCCC'></iframe></td>
                        <td nowrap valign="top"><%# DataBinder.Eval(Container.DataItem, "title")%></td>
                        <td width="100%" valign="top"><%# DataBinder.Eval(Container.DataItem, "description")%></td>
                        <td nowrap valign="top"><%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "modified").ToString()).ToShortDateString() %></td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr class="default" bgcolor="#F6F6F6" onmouseover="CellRowOver(this);" onmouseout="CellRowOut(this);" onclick="NewMaximize('/frame/loading.htm?referrer=/frame/report.aspx?r=<%# oFunction.encryptQueryString(DataBinder.Eval(Container.DataItem, "reportid").ToString()) %>');">
                        <td nowrap valign="top"><iframe src='/frame/report_preview.aspx?r=<%# oFunction.encryptQueryString(DataBinder.Eval(Container.DataItem, "reportid").ToString()) %>' width='150' height='100' frameborder='0' scrolling='no' style='border:solid 1px #CCCCCC'></iframe></td>
                        <td nowrap valign="top"><%# DataBinder.Eval(Container.DataItem, "title")%></td>
                        <td width="100%" valign="top"><%# DataBinder.Eval(Container.DataItem, "description")%></td>
                        <td nowrap valign="top"><%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "modified").ToString()).ToShortDateString() %></td>
                    </tr>
                </AlternatingItemTemplate>
            </asp:repeater>
                <tr>
                    <td colspan="4"><asp:Label ID="lblViewPreview" runat="server" CssClass="default" Text="<img src='/images/bigAlert.gif' border='0' align='absmiddle' /> Your search has not returned any results..." /></td>
                </tr>
            </asp:Panel>
            <asp:Panel ID="panView" runat="server" Visible="false">
            <asp:repeater ID="rptView" runat="server">
                <HeaderTemplate>
                    <tr bgcolor="#EEEEEE">
                        <td><asp:LinkButton ID="btnOrderTitle" runat="server" CssClass="tableheader" Text="<b>Title</b>" OnClick="btnOrder_Click" CommandArgument="title" /></td>
                        <td><asp:LinkButton ID="btnOrderDescription" runat="server" CssClass="tableheader" Text="<b>Description</b>" OnClick="btnOrder_Click" CommandArgument="description" /></td>
                        <td><asp:LinkButton ID="btnOrderModified" runat="server" CssClass="tableheader" Text="<b>Modified</b>" OnClick="btnOrder_Click" CommandArgument="modified" /></td>
                    </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class="default" onmouseover="CellRowOver(this);" onmouseout="CellRowOut(this);" onclick="NewMaximize('/frame/loading.htm?referrer=/frame/report.aspx?r=<%# oFunction.encryptQueryString(DataBinder.Eval(Container.DataItem, "reportid").ToString()) %>');">
                        <td nowrap valign="top"><%# DataBinder.Eval(Container.DataItem, "title")%></td>
                        <td width="100%" valign="top"><%# DataBinder.Eval(Container.DataItem, "description")%></td>
                        <td nowrap valign="top"><%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "modified").ToString()).ToShortDateString() %></td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr class="default" bgcolor="#F6F6F6" onmouseover="CellRowOver(this);" onmouseout="CellRowOut(this);" onclick="NewMaximize('/frame/loading.htm?referrer=/frame/report.aspx?r=<%# oFunction.encryptQueryString(DataBinder.Eval(Container.DataItem, "reportid").ToString()) %>');">
                        <td nowrap valign="top"><%# DataBinder.Eval(Container.DataItem, "title")%></td>
                        <td width="100%" valign="top"><%# DataBinder.Eval(Container.DataItem, "description")%></td>
                        <td nowrap valign="top"><%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "modified").ToString()).ToShortDateString() %></td>
                    </tr>
                </AlternatingItemTemplate>
            </asp:repeater>
                <tr>
                    <td colspan="4"><asp:Label ID="lblView" runat="server" CssClass="default" Text="<img src='/images/bigAlert.gif' border='0' align='absmiddle' /> Your search has not returned any results..." /></td>
                </tr>
            </asp:Panel>
               
            </table>
	    </td>
	</tr>
</table>
</asp:Panel>
        </td>
        <td background="/images/table_right.gif"><img src="/images/table_right.gif" width="5" height="10"></td>
    </tr>
    <tr height="1">
        <td><img src="/images/table_bottomLeft.gif" border="0" width="5" height="9"></td>
        <td width="100%" background="/images/table_bottom.gif"></td>
        <td><img src="/images/table_bottomRight.gif" border="0" width="5" height="9"></td>
    </tr>
</table>
</asp:Panel>
<input type="hidden" id="hdnReport" runat="server" />