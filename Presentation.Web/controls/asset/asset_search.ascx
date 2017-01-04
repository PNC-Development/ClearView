<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="asset_search.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.asset_search" %>
<script type="text/javascript">
    function ValidateSearch(oH, oN, oS, oA, oPC, oC, oP, oPD, oD) {
        oH = document.getElementById(oH);
        if (oH.value == "1") {
            oN = document.getElementById(oN);
            oS = document.getElementById(oS);
            oA = document.getElementById(oA);
            if (trim(oN.value) == "" && trim(oS.value) == "" && trim(oA.value) == "")
            {
                alert('Please enter a device name, serial number or asset tag');
                oN.focus();
                return false;
            }
        }
        if (oH.value == "2") {
            oPC = document.getElementById(oPC);
            oC = document.getElementById(oC);
            if (oPC.selectedIndex == 0)
            {
                alert('Please choose a platform');
                oPC.focus();
                return false;
            }
            else if (oC.selectedIndex == 0)
            {
                alert('Please choose a class');
                oC.focus();
                return false;
            }
        }
        if (oH.value == "3") {
            oP = document.getElementById(oP);
            if (oP.selectedIndex == 0)
            {
                alert('Please choose a platform');
                oP.focus();
                return false;
            }
        }
        if (oH.value == "4") {
            oPD = document.getElementById(oPD);
            oD = document.getElementById(oD);
            if (oPD.selectedIndex == 0)
            {
                alert('Please choose a platform');
                oPD.focus();
                return false;
            }
            else if (oD.selectedIndex == 0)
            {
                alert('Please choose a depot');
                oD.focus();
                return false;
            }
        }
        return true;
    }
    function View(strStatus, strUrl) {
        if(event.srcElement.tagName != 'A') {
            if (strStatus == "-1")
                alert('This device has been disposed and therefore, there is no additional information regarding this device');
            else
                window.navigate(strUrl);
        }
    }
</script>
<table width="100%" cellpadding="0" cellspacing="0" border="0" class="default">
    <tr>
        <td><img src="/images/table_topleft.gif" border="0" width="5" height="26"></td>
        <td width="100%" background="/images/table_top.gif" class="greentableheader"><asp:Label ID="lblTitle" runat="server" CssClass="greentableheader" /></td>
        <td><img src="/images/table_topright.gif" border="0" width="5" height="26"></td>
    </tr>
    <tr>
        <td background="/images/table_left.gif"><img src="/images/table_left.gif" width="5" height="10"></td>
        <td width="100%" bgcolor="#FFFFFF">
            <br />
            <table width="100%" cellpadding="0" cellspacing="5" border="0">
                <tr>
                    <td rowspan="2"><img src="/images/asset_search.gif" border="0" align="absmiddle" /></td>
                    <td class="header" width="100%" valign="bottom">Asset Search</td>
                </tr>
                <tr>
                    <td width="100%" valign="top">Use asset search to find and update an asset owned by National City.</td>
                </tr>
            </table>
            <asp:Panel ID="panSearch" runat="server" Visible="false">
                <%=strMenuTab1 %>
                <!-- 
                <table cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td><img src="/images/TabEmptyBackground.gif"></td>
                        <%=strMenuTab1 %>
                        <td width="100%" background="/images/TabEmptyBackground.gif">&nbsp;</td>
                    </tr>
                </table>-->
               <div id="divMenu1"> 
                <div style="display:none">
                    <br />
                    <table width="100%" cellpadding="4" cellspacing="3" border="0">
                        <tr>
                            <td colspan="2" class="required"><b>NOTE:</b> Searches Checked In, Commissioned, and Decommissioned Devices</td>
                        </tr>
                        <tr>
                            <td nowrap>Device Name:</td>
                            <td width="100%"><asp:TextBox ID="txtName" runat="server" CssClass="default" Width="400" MaxLength="100" /></td>
                        </tr>
                        <tr>
                            <td nowrap>Serial Number:</td>
                            <td width="100%"><asp:TextBox ID="txtSerial" runat="server" CssClass="default" Width="400" MaxLength="50" /></td>
                        </tr>
                        <tr>
                            <td nowrap>Asset Tag:</td>
                            <td width="100%"><asp:TextBox ID="txtAsset" runat="server" CssClass="default" Width="400" MaxLength="20" /></td>
                        </tr>
                    </table>
                </div>
                <div style="display:none">
                    <br />
                    <table width="100%" cellpadding="4" cellspacing="3" border="0">
                        <tr>
                            <td colspan="2" class="required"><b>NOTE:</b> Searches Commissioned Devices Only</td>
                        </tr>
                        <tr>
                            <td nowrap>Platform:</td>
                            <td width="100%">
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <tr>
                                        <td><asp:DropDownList ID="ddlPlatformClass" runat="server" CssClass="default" Width="400" AutoPostBack="true" OnSelectedIndexChanged="ddlPlatformClass_Change" /></td>
                                        <td class="bold">
                                            <div id="divPlatformClass" runat="server" style="display:none">
                                                <img src="/images/spacer.gif" border="0" width="20" height="1" /><img src="/images/please_wait.gif" border="0" align="absmiddle" /> Please Wait...
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td nowrap>Class:</td>
                            <td width="100%">
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <tr>
                                        <td><asp:DropDownList ID="ddlClass" runat="server" CssClass="default" Width="400" AutoPostBack="true" OnSelectedIndexChanged="ddlClass_Change" /></td>
                                        <td class="bold">
                                            <div id="divWaitClass" runat="server" style="display:none">
                                                <img src="/images/spacer.gif" border="0" width="20" height="1" /><img src="/images/please_wait.gif" border="0" align="absmiddle" /> Please Wait...
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td nowrap>Environment:</td>
                            <td width="100%"><asp:DropDownList ID="ddlEnvironment" runat="server" CssClass="default" Width="400" /></td>
                        </tr>
                    </table>
                </div>
                <div style="display:none">
                    <br />
                    <table width="100%" cellpadding="4" cellspacing="3" border="0">
                        <tr>
                            <td colspan="2" class="required"><b>NOTE:</b> Searches Checked In, Commissioned, and Decommissioned Devices</td>
                        </tr>
                        <tr>
                            <td nowrap>Platform:</td>
                            <td width="100%">
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <tr>
                                        <td><asp:DropDownList ID="ddlPlatform" runat="server" CssClass="default" Width="400" AutoPostBack="true" OnSelectedIndexChanged="ddlPlatform_Change" /></td>
                                        <td class="bold">
                                            <div id="divWaitPlatform" runat="server" style="display:none">
                                                <img src="/images/spacer.gif" border="0" width="20" height="1" /><img src="/images/please_wait.gif" border="0" align="absmiddle" /> Please Wait...
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td nowrap>Asset Type:</td>
                            <td width="100%">
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <tr>
                                        <td><asp:DropDownList ID="ddlTypes" runat="server" CssClass="default" Width="400" AutoPostBack="true" OnSelectedIndexChanged="ddlTypes_Change" /></td>
                                        <td class="bold">
                                            <div id="divWaitType" runat="server" style="display:none">
                                                <img src="/images/spacer.gif" border="0" width="20" height="1" /><img src="/images/please_wait.gif" border="0" align="absmiddle" /> Please Wait...
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td nowrap>Model:</td>
                            <td width="100%"><asp:DropDownList ID="ddlModel" runat="server" CssClass="default" Width="400" /></td>
                        </tr>
                    </table>
                </div>
                <div style="display:none">
                    <br />
                    <table width="100%" cellpadding="4" cellspacing="3" border="0">
                        <tr>
                            <td colspan="2" class="required"><b>NOTE:</b> Searches Checked In Devices Only</td>
                        </tr>
                        <tr>
                            <td nowrap>Platform:</td>
                            <td width="100%">
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <tr>
                                        <td><asp:DropDownList ID="ddlPlatformDepot" runat="server" CssClass="default" Width="400" AutoPostBack="true" OnSelectedIndexChanged="ddlPlatformDepot_Change" /></td>
                                        <td class="bold">
                                            <div id="divPlatformDepot" runat="server" style="display:none">
                                                <img src="/images/spacer.gif" border="0" width="20" height="1" /><img src="/images/please_wait.gif" border="0" align="absmiddle" /> Please Wait...
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td nowrap>Depot Location:</td>
                            <td width="100%"><asp:DropDownList ID="ddlDepot" runat="server" CssClass="default" Width="400" /></td>
                        </tr>
                    </table>
                </div>
                </div> 
                <table width="100%" cellpadding="4" cellspacing="3" border="0">
                    <tr>
                        <td colspan="2"><hr size="1" noshade /></td>
                    </tr>
                    <tr>
                        <td><asp:Label ID="lblCount" runat="server" CssClass="bold" /></td>
                        <td align="right"><asp:Button ID="btnSearch" runat="server" CssClass="default" Text="Search" Width="75" OnClick="btnSearch_Click" /></td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="panResult" runat="server" Visible="false">
                <table width="100%" cellpadding="4" cellspacing="3" border="0">
                    <tr>
                        <td colspan="2">
                            <table width="100%" cellpadding="3" cellspacing="0" border="0">
                                <tr>
                                    <td>
                                        <b>Page <asp:TextBox ID="txtPage" runat="server" CssClass="default" Width="25" /> of <asp:Label ID="lblPages" runat="server" /> <asp:ImageButton ID="btnPage" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/search.gif" OnClick="btnPage_Click" ToolTip="Go to this page" /></b>
                                        <b><asp:Label ID="lblRecords" runat="server" Visible="false" /></b>
                                    </td>
                                    <td align="right"><asp:Label ID="lblTopPaging" runat="server" /></td>
                                </tr>
                            </table>
                            <br />
                            <table width="100%" cellpadding="3" cellspacing="0" border="0" align="center" style="border:solid 1px #CCCCCC">
                                <tr bgcolor="#EEEEEE">
                                    <td><asp:LinkButton ID="btnOrderID" runat="server" CssClass="tableheader" Text="<b>ID</b>" OnClick="btnOrder_Click" CommandArgument="id" /></td>
                                    <td><asp:LinkButton ID="btnOrderName" runat="server" CssClass="tableheader" Text="<b>Device Name</b>" OnClick="btnOrder_Click" CommandArgument="name" /></td>
                                    <td><asp:LinkButton ID="btnOrderSerial" runat="server" CssClass="tableheader" Text="<b>Serial Number</b>" OnClick="btnOrder_Click" CommandArgument="serial" /></td>
                                    <td><asp:LinkButton ID="btnOrderAsset" runat="server" CssClass="tableheader" Text="<b>Asset Tag</b>" OnClick="btnOrder_Click" CommandArgument="asset" /></td>
                                    <td><asp:LinkButton ID="btnOrderPlatform" runat="server" CssClass="tableheader" Text="<b>Platform</b>" OnClick="btnOrder_Click" CommandArgument="platform" /></td>
                                    <td><asp:LinkButton ID="btnOrderType" runat="server" CssClass="tableheader" Text="<b>Type</b>" OnClick="btnOrder_Click" CommandArgument="type" /></td>
                                    <td><asp:LinkButton ID="btnOrderModel" runat="server" CssClass="tableheader" Text="<b>Model</b>" OnClick="btnOrder_Click" CommandArgument="model" /></td>
                                    <td><asp:LinkButton ID="btnOrderStatus" runat="server" CssClass="tableheader" Text="<b>Status</b>" OnClick="btnOrder_Click" CommandArgument="statusname" /></td>
                                </tr>
                                <asp:repeater ID="rptView" runat="server">
                                    <ItemTemplate>
                                        <tr onmouseover="CellRowOver(this);" onmouseout="CellRowOut(this);" onclick="View('<%# DataBinder.Eval(Container.DataItem, "status")%>','<%# "?sid=" + Request.QueryString["sid"] + "&id=" + DataBinder.Eval(Container.DataItem, "id") %>');">
                                            <td valign="top"><%# DataBinder.Eval(Container.DataItem, "id")%></td>
                                            <td valign="top"><%# DataBinder.Eval(Container.DataItem, "name") %></td>
                                            <td valign="top"><%# DataBinder.Eval(Container.DataItem, "serial") %></td>
                                            <td valign="top"><%# DataBinder.Eval(Container.DataItem, "asset") %></td>
                                            <td valign="top"><%# DataBinder.Eval(Container.DataItem, "platform") %></td>
                                            <td valign="top"><%# DataBinder.Eval(Container.DataItem, "type") %></td>
                                            <td valign="top"><%# DataBinder.Eval(Container.DataItem, "model") %></td>
                                            <td valign="top"><%# DataBinder.Eval(Container.DataItem, "statusname")%></td>
                                        </tr>
                                    </ItemTemplate>
                                    <AlternatingItemTemplate>
                                        <tr bgcolor="F6F6F6" onmouseover="CellRowOver(this);" onmouseout="CellRowOut(this);" onclick="View('<%# DataBinder.Eval(Container.DataItem, "status")%>','<%# "?sid=" + Request.QueryString["sid"] + "&id=" + DataBinder.Eval(Container.DataItem, "id") %>');">
                                            <td valign="top"><%# DataBinder.Eval(Container.DataItem, "id")%></td>
                                            <td valign="top"><%# DataBinder.Eval(Container.DataItem, "name") %></td>
                                            <td valign="top"><%# DataBinder.Eval(Container.DataItem, "serial") %></td>
                                            <td valign="top"><%# DataBinder.Eval(Container.DataItem, "asset") %></td>
                                            <td valign="top"><%# DataBinder.Eval(Container.DataItem, "platform") %></td>
                                            <td valign="top"><%# DataBinder.Eval(Container.DataItem, "type") %></td>
                                            <td valign="top"><%# DataBinder.Eval(Container.DataItem, "model") %></td>
                                            <td valign="top"><%# DataBinder.Eval(Container.DataItem, "statusname") %></td>
                                        </tr>
                                    </AlternatingItemTemplate>
                                </asp:repeater>
                                <tr>
                                    <td colspan="8">
                                        <asp:Label ID="lblNone" runat="server" CssClass="default" Visible="false" Text="<img src='/images/spacer.gif' border='0' height='1' width='25' /><img src='/images/alert.gif' border='0' align='absmiddle'> There are no items" />
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <table width="100%" cellpadding="3" cellspacing="0" border="0">
                                <tr>
                                    <td></td>
                                    <td align="right"><asp:Label ID="lblBottomPaging" runat="server" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td><b>Search Criteria:</b></td>
                        <td valign="top" rowspan="2" align="right"><asp:LinkButton ID="btnPrint" runat="server" Text="<img src='/images/bigPrint.gif' border='0' align='absmiddle' /><img src='/images/spacer.gif' border='0' width='5' height=1' />Click here for printer-friendly results" /></td>
                    </tr>
                    <tr>
                        <td valign="top"><asp:Label ID="lblResults" runat="server" CssClass="default" /></td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="panPath" runat="server" Visible="false">
            <table width="100%" cellpadding="4" cellspacing="3" border="0">
                <asp:PlaceHolder ID="PHControl" runat="server" />
            </table>
            </asp:Panel>
            <asp:Panel ID="panNoPath" runat="server" Visible="false">
            <table width="100%" cellpadding="0" cellspacing="5" border="0">
                <tr>
                    <td align="center"><img src="/images/alert.gif" border="0" align="absmiddle" /> Asset information has not been configured for this platform...</td>
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
                                    <td align="right"><asp:Button ID="btnClose" runat="server" CssClass="default" Width="75" Text="Close" /></td>
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
<asp:Label ID="lblSearch" runat="server" Visible="false" />
<input type="hidden" id="hdnType" runat="server" />
<asp:Label ID="lblPage" runat="server" Visible="false" />
<asp:Label ID="lblSort" runat="server" Visible="false" />
