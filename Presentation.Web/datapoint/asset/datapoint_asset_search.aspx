<%@ Page Language="C#" MasterPageFile="~/datapoint.Master" AutoEventWireup="True" CodeBehind="datapoint_asset_search.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.datapoint_asset_search" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" runat="server">
    <script type="text/javascript">
        function ChangeCookieSearch(oList, strCookie, oValue, oModel) {
            if (event.srcElement.tagName == "INPUT") {
                var oRadio = event.srcElement;
                SetCookie(strCookie, oRadio.value);
                oValue = document.getElementById(oValue);
                oModel = document.getElementById(oModel);
                if (oRadio.value == "deploy") {
                    oValue.style.display = "none";
                    oModel.style.display = "inline";
                }
                else {
                    oValue.style.display = "inline";
                    oModel.style.display = "none";
                }
            }
        }
        function EnsureAssetSearch(oSearch, oMnemonicText, oMnemonicHidden) {
            var oObject = document.getElementById(oMnemonicText);
            var boolHidden = false;
            try {
                oObject.focus();
            }
            catch (ex) {
                boolHidden = true;
            }
            if (boolHidden == true) {
                // Basic Search
                return ValidateTextDisabled(oSearch,'Please enter a value to search');
            }
            else {
                // Advanced Search
                if (ValidateHidden0(oMnemonicHidden,oMnemonicText,'') == false && ValidateTextDisabled(oSearch,'') == false) {
                    alert('Please enter a search criteria');
                    return false;
                }
            }
            return true;
        }
    </script>
    <asp:Panel ID="panAllow" runat="server" Visible="false">
        <table width="100%" cellpadding="0" cellspacing="5" border="0">
            <tr>
                <td rowspan="2"><img src="/images/assets.gif" border="0" align="absmiddle" /></td>
                <td class="header" width="100%" valign="bottom">Asset Search</td>
            </tr>
            <tr>
                <td width="100%" valign="top">Search on asset information related to servers and workstations.</td>
            </tr>
        </table>
        <table width="100%" cellpadding="4" cellspacing="3" border="0">
            <tr>
                <td nowrap><asp:Label ID="fldSearch" runat="server" CssClass="default" Text="Search Type:" /></td>
                <td width="100%"><asp:RadioButtonList ID="radSearch" runat="server" CssClass="default" RepeatDirection="Horizontal" /></td>
            </tr>
            <tr id="divValue" runat="server" style="display:none">
                <td nowrap>Search Value:</td>
                <td width="100%">
                    <table cellpadding="0" cellspacing="0" border="0">
                        <tr>
                            <td><asp:TextBox ID="txtSearch" runat="server" Width="250" CssClass="default" MaxLength="100" /></td>
                        </tr>
                        <tr>
                            <td>
                                <div id="divSearch" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                                    <asp:ListBox ID="lstSearch" runat="server" CssClass="default" />
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr id="divModel" runat="server" style="display:none">
                <td nowrap><asp:Label ID="fldModel" runat="server" CssClass="default" Text="Model:" /></td>
                <td width="100%"><asp:DropDownList ID="ddlModel" runat="server" CssClass="default" Width="250" /></td>
            </tr>
            <tr style='display:<%=boolAdvanced ? "inline" : "none" %>'>
                <td nowrap>Mnemonic:</td>
                <td width="100%">
                    <table cellpadding="0" cellspacing="0" border="0">
                        <tr>
                            <td><asp:TextBox ID="txtSearchMnemonic" runat="server" Width="500" CssClass="default" /></td>
                        </tr>
                        <tr>
                            <td>
                                <div id="divSearchMnemonic" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                                    <asp:ListBox ID="lstSearchMnemonic" runat="server" CssClass="default" />
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr style='display:<%=boolAdvanced ? "inline" : "none" %>'>
                <td nowrap>Build Date:</td>
                <td width="100%"><asp:TextBox ID="txtBuildStart" runat="server" CssClass="default" Width="100" MaxLength="10" />&nbsp;<asp:ImageButton ID="imgBuildStart" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" />&nbsp;-&nbsp;<asp:TextBox ID="txtBuildEnd" runat="server" CssClass="default" Width="100" MaxLength="10" />&nbsp;<asp:ImageButton ID="imgBuildEnd" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
            </tr>
            <tr style='display:<%=boolAdvanced ? "inline" : "none" %>'>
                <td nowrap></td>
                <td width="100%"><asp:CheckBox ID="chkDecommissions" runat="server" CssClass="default" Text="Include Decommissions" /></td>
            </tr>
            <tr>
                <td nowrap>Results per Page:</td>
                <td width="100%">
                    <asp:DropDownList ID="ddlSearch" runat="server" Width="75" CssClass="default">
                        <asp:ListItem Value="10" Text="10" />
                        <asp:ListItem Value="25" Text="25" Selected="True" />
                        <asp:ListItem Value="50" Text="50" />
                        <asp:ListItem Value="100" Text="100" />
                        <asp:ListItem Value="0" Text="All" />
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td nowrap class="required"></td>
                <td width="100%" colspan="2">
                    <table cellpadding="3" cellspacing="2" border="0">
                        <tr>
                            <td><asp:LinkButton ID="btnAdvanced" runat="server" Text="Advanced Search" OnClick="btnAdvanced_Click" /></td>
                            <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                            <td><asp:LinkButton ID="lnkSearch" runat="server" Text="Clear History" OnClick="lnkSearch_Click" Enabled="false" /></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td nowrap class="required"></td>
                <td width="100%"><asp:Button ID="btnSearch" Text="Search" Width="100" CssClass="default" runat="server" OnClick="btnSearch_Click" /> <asp:Button ID="btnNew" Text="Start Over" Width="100" CssClass="default" runat="server" OnClick="btnNew_Click" Visible="false" /></td>
            </tr>
        </table>
        <br />
        <asp:Panel ID="panResults" runat="server" Visible="false">
        <table width="100%" cellpadding="4" cellspacing="3" border="0">
            <tr>
                <td class="default"><asp:Label ID="lblRecords" runat="server" CssClass="bigger" /></td>
                <td class="default" align="right"><asp:LinkButton ID="btnBack" runat="server" Text="Previous Page" OnClick="btnBack_Click" />&nbsp;&nbsp;|&nbsp;&nbsp;<asp:LinkButton ID="btnNext" runat="server" Text="Next Page" OnClick="btnNext_Click" />&nbsp;&nbsp;</td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Panel ID="panName" runat="server" Visible="false">
                        <table width="100%" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                            <asp:repeater ID="rptName" runat="server">
                                <HeaderTemplate>
                                    <tr bgcolor="#EEEEEE">
                                        <td></td>
                                        <td>#</td>
                                        <td nowrap><b>Device Name</b></td>
                                        <td nowrap><b>Serial Number</b></td>
                                        <td nowrap><b>Asset Tag</b></td>
                                        <td nowrap><b>Model</b></td>
                                        <td nowrap align="center"><b>Status</b></td>
                                    </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                                        <td><%=intCounter++ %></td>
                                        <td width="20%" nowrap><a onclick="LoadWait();" href='/datapoint/asset/<%# DataBinder.Eval(Container.DataItem, "url") %>.aspx?t=<%#Request.QueryString["t"] %>&q=<%# oFunction.encryptQueryString(DataBinder.Eval(Container.DataItem, "servername").ToString()) %>&id=<%# oFunction.encryptQueryString(DataBinder.Eval(Container.DataItem, "id").ToString()) %>'><%# DataBinder.Eval(Container.DataItem, "servername") %></a></td>
                                        <td width="20%" nowrap title='<%# DataBinder.Eval(Container.DataItem, "assetid") %>'><%# DataBinder.Eval(Container.DataItem, "serial") %></td>
                                        <td width="20%" nowrap><%# DataBinder.Eval(Container.DataItem, "asset") %></td>
                                        <td width="20%" nowrap><%# DataBinder.Eval(Container.DataItem, "model") %></td>
                                        <td width="20%" nowrap align="center"><%# DataBinder.Eval(Container.DataItem, "status") %></td>
                                    </tr>
                                </ItemTemplate>
                                <AlternatingItemTemplate>
                                    <tr bgcolor="#F6F6F6">
                                        <td><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                                        <td><%=intCounter++%></td>
                                        <td width="20%" nowrap><a onclick="LoadWait();" href='/datapoint/asset/<%# DataBinder.Eval(Container.DataItem, "url") %>.aspx?t=<%#Request.QueryString["t"] %>&q=<%# oFunction.encryptQueryString(DataBinder.Eval(Container.DataItem, "servername").ToString()) %>&id=<%# oFunction.encryptQueryString(DataBinder.Eval(Container.DataItem, "id").ToString()) %>'><%# DataBinder.Eval(Container.DataItem, "servername") %></a></td>
                                        <td width="20%" nowrap title='<%# DataBinder.Eval(Container.DataItem, "assetid") %>'><%# DataBinder.Eval(Container.DataItem, "serial") %></td>
                                        <td width="20%" nowrap><%# DataBinder.Eval(Container.DataItem, "asset") %></td>
                                        <td width="20%" nowrap><%# DataBinder.Eval(Container.DataItem, "model") %></td>
                                        <td width="20%" nowrap align="center"><%# DataBinder.Eval(Container.DataItem, "status") %></td>
                                    </tr>
                                </AlternatingItemTemplate>
                            </asp:repeater>
                            <tr>
                                <td colspan="6"><asp:Label ID="lblName" runat="server" CssClass="default" Visible="false" Text="<img src='/images/spacer.gif' border='0' height='1' width='25' /><img src='/images/alert.gif' border='0' align='absmiddle'> No results found" /></td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="panSerial" runat="server" Visible="false">
                        <table width="100%" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                            <asp:repeater ID="rptSerial" runat="server">
                                <HeaderTemplate>
                                    <tr bgcolor="#EEEEEE">
                                        <td></td>
                                        <td>#</td>
                                        <td nowrap><b>Serial Number</b></td>
                                        <td nowrap><b>Asset Tag</b></td>
                                        <td nowrap><b>Model</b></td>
                                        <td nowrap align="center"><b>Status</b></td>
                                        <td nowrap><b>Device Name</b></td>
                                    </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                                        <td><%=intCounter++ %></td>
                                        <td width="20%" nowrap><a onclick="LoadWait();" href='/datapoint/asset/<%# DataBinder.Eval(Container.DataItem, "url") %>.aspx?t=<%#Request.QueryString["t"] %>&q=<%#Request.QueryString["q"] %>&id=<%# oFunction.encryptQueryString(DataBinder.Eval(Container.DataItem, "id").ToString()) %>'><%# DataBinder.Eval(Container.DataItem, "serial")%></a></td>
                                        <td width="20%" nowrap><%# DataBinder.Eval(Container.DataItem, "asset")%></td>
                                        <td width="20%" nowrap><%# DataBinder.Eval(Container.DataItem, "model")%></td>
                                        <td width="20%" nowrap align="center"><%# DataBinder.Eval(Container.DataItem, "status")%></td>
                                        <td width="20%" nowrap><%# DataBinder.Eval(Container.DataItem, "servername")%></td>
                                    </tr>
                                </ItemTemplate>
                                <AlternatingItemTemplate>
                                    <tr bgcolor="#F6F6F6">
                                        <td><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                                        <td><%=intCounter++%></td>
                                        <td width="20%" nowrap><a onclick="LoadWait();" href='/datapoint/asset/<%# DataBinder.Eval(Container.DataItem, "url") %>.aspx?t=<%#Request.QueryString["t"] %>&q=<%#Request.QueryString["q"] %>&id=<%# oFunction.encryptQueryString(DataBinder.Eval(Container.DataItem, "id").ToString()) %>'><%# DataBinder.Eval(Container.DataItem, "serial")%></a></td>
                                        <td width="20%" nowrap><%# DataBinder.Eval(Container.DataItem, "asset")%></td>
                                        <td width="20%" nowrap><%# DataBinder.Eval(Container.DataItem, "model")%></td>
                                        <td width="20%" nowrap align="center"><%# DataBinder.Eval(Container.DataItem, "status")%></td>
                                        <td width="20%" nowrap><%# DataBinder.Eval(Container.DataItem, "servername")%></td>
                                    </tr>
                                </AlternatingItemTemplate>
                            </asp:repeater>
                            <tr>
                                <td colspan="6"><asp:Label ID="lblSerial" runat="server" CssClass="default" Visible="false" Text="<img src='/images/spacer.gif' border='0' height='1' width='25' /><img src='/images/alert.gif' border='0' align='absmiddle'> No results found" /></td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="panTag" runat="server" Visible="false">
                        <table width="100%" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                            <asp:repeater ID="rptTag" runat="server">
                                <HeaderTemplate>
                                    <tr bgcolor="#EEEEEE">
                                        <td></td>
                                        <td>#</td>
                                        <td nowrap><b>Asset Tag</b></td>
                                        <td nowrap><b>Serial Number</b></td>
                                        <td nowrap><b>Model</b></td>
                                        <td nowrap align="center"><b>Status</b></td>
                                        <td nowrap><b>Device Name</b></td>
                                    </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                                        <td><%=intCounter++ %></td>
                                        <td width="20%" nowrap><a onclick="LoadWait();" href='/datapoint/asset/<%# DataBinder.Eval(Container.DataItem, "url") %>.aspx?t=<%#Request.QueryString["t"] %>&q=<%#Request.QueryString["q"] %>&id=<%# oFunction.encryptQueryString(DataBinder.Eval(Container.DataItem, "id").ToString()) %>'><%# DataBinder.Eval(Container.DataItem, "asset")%></a></td>
                                        <td width="20%" nowrap><%# DataBinder.Eval(Container.DataItem, "serial")%></td>
                                        <td width="20%" nowrap><%# DataBinder.Eval(Container.DataItem, "model")%></td>
                                        <td width="20%" nowrap align="center"><%# DataBinder.Eval(Container.DataItem, "status")%></td>
                                        <td width="20%" nowrap><%# DataBinder.Eval(Container.DataItem, "servername")%></td>
                                    </tr>
                                </ItemTemplate>
                                <AlternatingItemTemplate>
                                    <tr bgcolor="#F6F6F6">
                                        <td><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                                        <td><%=intCounter++%></td>
                                        <td width="20%" nowrap><a onclick="LoadWait();" href='/datapoint/asset/<%# DataBinder.Eval(Container.DataItem, "url") %>.aspx?t=<%#Request.QueryString["t"] %>&q=<%#Request.QueryString["q"] %>&id=<%# oFunction.encryptQueryString(DataBinder.Eval(Container.DataItem, "id").ToString()) %>'><%# DataBinder.Eval(Container.DataItem, "asset")%></a></td>
                                        <td width="20%" nowrap><%# DataBinder.Eval(Container.DataItem, "serial")%></td>
                                        <td width="20%" nowrap><%# DataBinder.Eval(Container.DataItem, "model")%></td>
                                        <td width="20%" nowrap align="center"><%# DataBinder.Eval(Container.DataItem, "status")%></td>
                                        <td width="20%" nowrap><%# DataBinder.Eval(Container.DataItem, "servername")%></td>
                                    </tr>
                                </AlternatingItemTemplate>
                            </asp:repeater>
                            <tr>
                                <td colspan="6"><asp:Label ID="lblTag" runat="server" CssClass="default" Visible="false" Text="<img src='/images/spacer.gif' border='0' height='1' width='25' /><img src='/images/alert.gif' border='0' align='absmiddle'> No results found" /></td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="panDeploy" runat="server" Visible="false">
                        <table width="100%" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                            <asp:repeater ID="rptDeploy" runat="server">
                                <HeaderTemplate>
                                    <tr bgcolor="#EEEEEE">
                                        <td></td>
                                        <td>#</td>
                                        <td nowrap><b>Serial Number</b></td>
                                        <td nowrap><b>Asset Tag</b></td>
                                        <td nowrap><b>Model</b></td>
                                        <td nowrap align="center"><b>Status</b></td>
                                        <td nowrap><b>Last Modified</b></td>
                                    </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                                        <td><%=intCounter++ %></td>
                                        <td width="20%" nowrap><a onclick="LoadWait();" href='/datapoint/asset/<%# DataBinder.Eval(Container.DataItem, "url") %>.aspx?t=<%#Request.QueryString["t"] %>&q=<%#Request.QueryString["q"] %>&id=<%# oFunction.encryptQueryString(DataBinder.Eval(Container.DataItem, "id").ToString()) %>'><%# DataBinder.Eval(Container.DataItem, "serial")%></a></td>
                                        <td width="20%" nowrap><%# DataBinder.Eval(Container.DataItem, "asset")%></td>
                                        <td width="20%" nowrap><%# DataBinder.Eval(Container.DataItem, "model")%></td>
                                        <td width="20%" nowrap align="center"><%# DataBinder.Eval(Container.DataItem, "status")%></td>
                                        <td width="20%" nowrap><%# DataBinder.Eval(Container.DataItem, "datestamp")%></td>
                                    </tr>
                                </ItemTemplate>
                                <AlternatingItemTemplate>
                                    <tr bgcolor="#F6F6F6">
                                        <td><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                                        <td><%=intCounter++%></td>
                                        <td width="20%" nowrap><a onclick="LoadWait();" href='/datapoint/asset/<%# DataBinder.Eval(Container.DataItem, "url") %>.aspx?t=<%#Request.QueryString["t"] %>&q=<%#Request.QueryString["q"] %>&id=<%# oFunction.encryptQueryString(DataBinder.Eval(Container.DataItem, "id").ToString()) %>'><%# DataBinder.Eval(Container.DataItem, "serial")%></a></td>
                                        <td width="20%" nowrap><%# DataBinder.Eval(Container.DataItem, "asset")%></td>
                                        <td width="20%" nowrap><%# DataBinder.Eval(Container.DataItem, "model")%></td>
                                        <td width="20%" nowrap align="center"><%# DataBinder.Eval(Container.DataItem, "status")%></td>
                                        <td width="20%" nowrap><%# DataBinder.Eval(Container.DataItem, "datestamp")%></td>
                                    </tr>
                                </AlternatingItemTemplate>
                            </asp:repeater>
                            <tr>
                                <td colspan="6"><asp:Label ID="lblDeploy" runat="server" CssClass="default" Visible="false" Text="<img src='/images/spacer.gif' border='0' height='1' width='25' /><img src='/images/alert.gif' border='0' align='absmiddle'> No results found" /></td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
        </table>
        </asp:Panel>
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
<asp:HiddenField ID="hdnSearchText" runat="server" />
<asp:HiddenField ID="hdnSearchType" runat="server" />
<asp:HiddenField ID="hdnSearchMnemonic" runat="server" />
</asp:Content>
