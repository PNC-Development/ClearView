<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="fore_new_cfi.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.fore_new_cfi" %>
<script type="text/javascript">
    function EditForecast(strID) {
        OpenWindow('DESIGN_EQUIPMENT','?id=' + strID);
        return false;
    }
function ShowProjectInfo(oList) {
    OpenWindow('PROJECT_INFO', oList.options[oList.selectedIndex].value);
}
function abandon(strDate, strDesign) {
    alert('Since the Start Build Date (' + strDate + ') is more than thirty (30) days old and this design was never executed, ClearView marked this design as being abandoned.\n\nNOTE: Prior to making this change, ClearView attempted to contact the requestor to update the Start Build Date on fifteen (15) separate occasions...but never received a response.\n\nTo have this design unlocked, please notify your ClearView administrator with an updated Start Build Date and the ID of this design (' + strDesign + ').');
}
</script>
<table width="100%" cellpadding="0" cellspacing="0" border="0" class="default">
    <tr>
        <td><img src="/images/table_topleft.gif" border="0" width="5" height="26"></td>
        <td nowrap background="/images/table_top.gif" class="greentableheader" width="100%"><asp:Label ID="lblTitle" runat="server" CssClass="greentableheader" /></td>
        <td><img src="/images/table_topright.gif" border="0" width="5" height="26"></td>
    </tr>
    <tr>
        <td background="/images/table_left.gif"><img src="/images/table_left.gif" width="5" height="10"></td>
        <td width="100%" bgcolor="#FFFFFF">
            <br />
            <asp:Panel ID="panAllow" runat="server" Visible="false">
            <table width="100%" cellpadding="0" cellspacing="5" border="0">
                <tr>
                    <td rowspan="2"><img src="/images/forecast2.gif" border="0" align="absmiddle" /></td>
                    <td class="header" width="100%" valign="bottom">Design Builder</td>
                </tr>
                <tr>
                    <td width="100%" valign="top">The design builder module is used to plan and design application requirements.</td>
                </tr>
            </table>
                <asp:Panel ID="panPending" runat="server" Visible="false">
	                <asp:Panel ID="panPendingChoose" runat="server" Visible="false">
                    <table width="100%" cellpadding="4" cellspacing="3" border="0" class="default">
                        <tr>
                            <td class="bold">Please select a project to associate with this design.</td>
                            <td align="right"><b>Order By: </b><asp:LinkButton ID="btnOrderName" runat="server" CommandArgument="namenumber" OnClick="btnOrder_Click" Text="Project Name" /> | <asp:LinkButton ID="btnOrderNumber" runat="server" CommandArgument="numbername" OnClick="btnOrder_Click" Text="Project Number" /></td>
                        </tr>
                <tr>
                    <td class="footer"><b>NOTE:</b> If you do not see your project, choose the &quot;<span style="color:#DD0000">-- PROJECT NOT LISTED --</span>&quot; option...</td>
                    <td align="right"><img src="/images/search_icon.gif" border="0" align="absmiddle" /> <a href="javascript:void(0);" onclick="ShowHideDiv2('trQuickFind');">Show Quick Find Filter</a></td>
                </tr>
                <tr id="trQuickFind" style="display:none">
                    <td colspan="2" align="center">
                        <table cellpadding="4" cellspacing="3" border="0" class="default" style="border:solid 1px #999999" bgcolor="#f9f9f9">
                            <tr>
                                <td nowrap><img src="/images/search_icon.gif" border="0" align="absmiddle" /> <b>Quick Find Filter:</b></td>
                                <td nowrap>Project Number:</td>
                                <td><asp:TextBox ID="txtSearchNumber" runat="server" CssClass="default" Width="100" MaxLength="20" /></td>
                                <td><img src="/images/spacer.gif" border="0" width="10" height="1" /></td>
                                <td nowrap class="header">-- OR --</td>
                                <td><img src="/images/spacer.gif" border="0" width="10" height="1" /></td>
                                <td nowrap>Project Name:</td>
                                <td><asp:TextBox ID="txtSearchName" runat="server" CssClass="default" Width="200" MaxLength="35" /></td>
                                <td><img src="/images/spacer.gif" border="0" width="10" height="1" /></td>
                                <td><asp:Button ID="btnSearch" runat="server" CssClass="default" Width="75" Text="Search" /></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                        <tr>
                            <td colspan="2"><asp:ListBox ID="lstProjects" runat="server" CssClass="default" Width="100%" Rows="25" /></td>
                        </tr>
                        <tr>
                            <td colspan="2"><hr size="1" noshade /></td>
                        </tr>
                        <tr>
                            <td nowrap class="required"></td>
                            <td align="right"><asp:Button ID="btnProjectSelect" runat="server" CssClass="default" Text="Submit" Width="75" OnClick="btnProjectSelect_Click" /></td>
                        </tr>
                    </table>
	                </asp:Panel>
	                <asp:Panel ID="panPendingNew" runat="server" Visible="false">
                    <table width="100%" cellpadding="4" cellspacing="3" border="0" class="default">
                        <tr>
                            <td colspan="2" class="bold">Please fill in the related project information using the following form.</td>
                        </tr>
                        <tr>
                            <td nowrap>Project Number:</td>
                            <td width="100%"><asp:TextBox ID="txtNumber" runat="server" CssClass="default" Width="150" MaxLength="30" /> <span class="required">(Not Required)</span></td>
                        </tr>
                        <tr>
                            <td nowrap>Project Name:<font class="required">&nbsp;*</font></td>
                            <td width="100%"><asp:TextBox ID="txtName" runat="server" CssClass="default" Width="300" MaxLength="50" /></td>
                        </tr>
                        <tr>
                            <td nowrap>Investment Class:<font class="required">&nbsp;*</font></td>
                            <td width="100%">
                                <asp:DropDownList ID="ddlBaseDisc" runat="server" CssClass="default">
                                    <asp:ListItem Value="0" Text="-- SELECT --" />
                                    <asp:ListItem Value="Acquisitions & Divestitures" />
                                    <asp:ListItem Value="Baseline" />
                                    <asp:ListItem Value="Client Billable/Contractual" />
                                    <asp:ListItem Value="Discretionary Project" />
                                    <asp:ListItem Value="Efficiency Initiatives" />
                                    <asp:ListItem Value="General Management & Administration" />
                                    <asp:ListItem Value="Client Implementations & Conversions" />
                                    <asp:ListItem Value="Non-Billable FTE" />
                                    <asp:ListItem Value="Application Support" />
                                    <asp:ListItem Value="Non-Technology" />
                                    <asp:ListItem Value="Regulatory & Compliance" />
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td nowrap>Sponsoring Portfolio:<font class="required">&nbsp;*</font></td>
                            <td width="100%"><asp:DropDownList ID="ddlOrganization" runat="server" CssClass="default" Width="400" /></td>
                        </tr>
                        <tr>
                            <td nowrap>Segment:<font class="required">&nbsp;*</font></td>
                            <td width="100%">
                                <asp:DropDownList ID="ddlSegment" CssClass="default" runat="server" Width="400" Enabled="false" >
                                    <asp:ListItem Value="-- Please select a Sponsoring Portfolio --" />
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td nowrap>Project Manager:<font class="required">&nbsp;*</font></td>
                            <td width="100%"><asp:TextBox ID="txtManager" runat="server" CssClass="default" Width="300" MaxLength="100" /></td>
                        </tr>
                        <tr>
                            <td colspan="2"><hr size="1" noshade /></td>
                        </tr>
                        <tr>
                            <td nowrap class="required">* = Required Field</td>
                            <td align="right"><asp:Button ID="btnProjectNew" runat="server" CssClass="default" Text="Submit" Width="75" OnClick="btnProjectNew_Click" /></td>
                        </tr>
                    </table>
	                </asp:Panel>
                </asp:Panel>
                <asp:Panel ID="panSelected" runat="server" Visible="False">
                <table width="100%" cellpadding="4" cellspacing="3" border="0">
                    <tr>
                        <td width="50%" valign="top">
                            <table width="100%" cellpadding="4" cellspacing="3" border="0">
                                <tr>
                                    <td nowrap>Requestor:</td>
                                    <td width="100%"><asp:Label ID="lblRequestor" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td nowrap>Submission Date:</td>
                                    <td width="100%"><asp:Label ID="lblDate" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td nowrap>Project Name:</td>
                                    <td width="100%"><asp:Label ID="lblName" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td nowrap>Initiative Type:</td>
                                    <td width="100%"><asp:Label ID="lblBaseDisc" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td nowrap>Organization:</td>
                                    <td width="100%"><asp:Label ID="lblOrganization" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td nowrap>Segment:</td>
                                    <td width="100%"><asp:Label ID="lblSegment" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td nowrap>Project Number:</td>
                                    <td width="100%"><asp:Label ID="lblNumber" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr style="display:none">
                                    <td nowrap>PNC Project Number:</td>
                                    <td width="100%"><asp:TextBox ID="txtPNC" runat="server" CssClass="default" Width="100" MaxLength="12" /></td>
                                </tr>
                                <tr>
                                    <td nowrap>Project Status:</td>
                                    <td width="100%"><asp:Label ID="lblStatus" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td nowrap>Project Manager:</td>
                                    <td width="100%"><asp:Label ID="lblManager" runat="server" CssClass="default" /></td>
                                </tr>
                            </table>
                        </td>
                        <td width="50%" valign="top" align="center">
                            <table width="200" cellpadding="4" cellspacing="3" border="0" style='display:<%= boolDemo ? "none" : "inline" %>'>
                                <tr>
                                    <td class="biggerreddefault"><b>Design Implementor(s)</b></td>
                                </tr>
                                <tr>
                                    <td>
                                     <%=strMenuTab1%>
                                     <!-- 
                                        <table width="300" cellpadding="0" cellspacing="0" border="0">
                                            <tr>
                                                <%=strMenuTab1%>
                                                <td width="100%" background="/images/TabEmptyBackground.gif">&nbsp;</td>
                                            </tr>
                                        </table>
                                        -->
                                        <div id="divMenu1" style="width:300px; border:solid 1px #DDDDDD; border-top:none">
                                        <%=strDiv %>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td><a href="https://appservicesstd.wss.pncbank.com/sites/Collaboration/WIS/Digital Library/Virtualization Engineering/Server Name Standards v8.pdf" target="_blank"><img src="/images/icons/doc.gif" border="0" align="absmiddle" /><img src="/images/spacer.gif" border="0" width="5" height="1" />View the PNC Server Naming Standards</a></td>
                                </tr>
                                <tr style="display:none">
                                    <td><a href="http://cois.sharepoint.ntl-city.com/EPS/EPS_CS_Commn/Product%20Availability/Product%20Availability%20Matrix.pdf" target="_blank"><img src="/images/icons/doc.gif" border="0" align="absmiddle" /><img src="/images/spacer.gif" border="0" width="5" height="1" />View the Product Availability Matrix</a></td>
                                </tr>
                                <tr>
                                    <td><a href="http://eawiki/ArchitectureDocs/CtaWikiPortal" target="_blank"><img src="/images/icons/doc.gif" border="0" align="absmiddle" /><img src="/images/spacer.gif" border="0" width="5" height="1" />View the Reference Architecture</a></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2"><hr size="1" noshade /></td>
                    </tr>
                </table>
                <table width="100%" cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td>
                            <table cellpadding="4" cellspacing="0" border="0">
                                <tr>
                                    <td class="default"><b>View:</b></td>
                                    <td class="default"><asp:Label ID="lblFilter" runat="server" CssClass="default" /></td>
                                    <td><img src="/images/spacer.gif" border="0" width="5" height="1" /></td>
                                    <td>[<asp:LinkButton ID="btnView" runat="server" Text="Filter" />]</td>
                                    <td>[<asp:LinkButton ID="btnClear" runat="server" Text="Show All" OnClick="btnClear_Click" Enabled="false" />]</td>
                                </tr>
                            </table>
                        </td>
                        <td align="right">
                            <table cellpadding="4" cellspacing="0" border="0">
                                <tr>
                                    <td>Filter:</td>
                                    <td>
                                        <asp:DropDownList ID="ddlFilter" runat="server" CssClass="default">
                                            <asp:ListItem Value="all" Text="Show me everyones designs" />
                                            <asp:ListItem Value="mine" Text="Show me only my designs" />
                                            <asp:ListItem Value="active" Text="Show me only active designs" />
                                            <asp:ListItem Value="complete" Text="Show me only completed designs" />
                                        </asp:DropDownList>
                                    </td>
                                    <td><asp:ImageButton ID="btnFilter" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/search.gif" OnClick="btnFilter_Click" ToolTip="Apply Filter" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <br />
                <table width="100%" cellpadding="4" cellspacing="0" border="0" align="center"  style="border:solid 1px #CCCCCC">
                    <tr bgcolor="#EEEEEE">
                        <td valign="bottom"><b><u>Design&nbsp;ID:</u></b></td>
                        <td valign="bottom"><b><u>Nickname:</u></b></td>
                        <td valign="bottom"><b><u>OS:</u></b></td>
                        <!--
                        <td valign="bottom" align="center"><b><u>Platform:</u></b></td>
                        -->
                        <td valign="bottom" nowrap style='display:<%= boolDemo ? "none" : "inline" %>'><b><u>Model:</u></b></td>
                        <td valign="bottom" align="center"><b><u>QTY:</u></b></td>
                        <td valign="bottom" align="center" style='display:<%= boolDemo ? "inline" : "none" %>'><b><u>Acquisition<br />Costs:</u></b></td>
                        <td valign="bottom" align="center" style='display:<%= boolDemo ? "inline" : "none" %>'><b><u>Operational<br />Costs:</u></b></td>
                        <td valign="bottom" align="center"><b><u>Storage:</u></b></td>
                        <!--
                        <td valign="bottom" align="center"><b><u>Backup:</u></b></td>
                        <td valign="bottom" align="center"><b><u>AMP:</u></b></td>
                        <td valign="bottom" align="center"><b><u>HRs:</u></b></td>
                        -->
                        <td valign="bottom" align="center"><b><u>Commitment<br />Date:</u></b></td>
                        <td valign="bottom" align="center"><b><u>Confidence:</u></b></td>
                        <!--
                        <td valign="bottom" align="center"><b><u>Production<br />Live Date:</u></b></td>
                        -->
                        <td valign="bottom" align="center"><b><u>% Done:</u></b></td>
                        <td width="1"></td>
                    </tr>
                    <asp:repeater ID="rptAll" runat="server">
                        <ItemTemplate>
                            <tr<%# (Request.QueryString["highlight"] != null && Request.QueryString["highlight"] == DataBinder.Eval(Container.DataItem, "id").ToString() ? " bgcolor=\"#99FF99\"" : "")%>>
                                <asp:Label ID="lblId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "id") %>' />
                                <td valign="top"><%# DataBinder.Eval(Container.DataItem, "id") %></td>
                                <td valign="top"><%# (DataBinder.Eval(Container.DataItem, "name").ToString() == "" ? "---" : DataBinder.Eval(Container.DataItem, "name").ToString()) %></td>
                                <td valign="top"><asp:Label ID="lblOS" runat="server" CssClass="default" Text='<%# (DataBinder.Eval(Container.DataItem, "os").ToString() == "" ? "---" : DataBinder.Eval(Container.DataItem, "os").ToString()) %>' /></td>
                                <td valign="top" style='display:<%= boolDemo ? "none" : "inline" %>'><asp:Label ID="lblModel" runat="server" CssClass="default" Text='0' /></td>
                                <td valign="top" align="center" nowrap><asp:Label ID="lblQuantity" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "quantity") %>' /></td></td>
                                <td valign="top" align="right" nowrap style='display:<%= boolDemo ? "inline" : "none" %>'><asp:Label ID="lblAcquisition" runat="server" CssClass="default" /><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                                <td valign="top" align="right" nowrap style='display:<%= boolDemo ? "inline" : "none" %>'><asp:Label ID="lblOperational" runat="server" CssClass="default" /><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                                <td valign="top" align="right" nowrap><asp:Label ID="lblStorage" runat="server" CssClass="default" /><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                                <!--
                                <td valign="top" align="center" nowrap><a href="javascript:void(0);" onclick="OpenWindow('FORECAST_BACKUP','<%# DataBinder.Eval(Container.DataItem, "id") %>');"><img src="/images/backup.gif" border="0" align="absmiddle" /></a></td>
                                <td valign="top" align="right" nowrap><asp:Label ID="lblAmp" runat="server" CssClass="default" /><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                                <td valign="top" align="right" nowrap><asp:Label ID="lblHours" runat="server" CssClass="default" /><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                                -->
                                <td valign="top" align="center" nowrap><asp:Label ID="lblCommitment" runat="server" CssClass="default" Text='<%# (DataBinder.Eval(Container.DataItem, "commitment").ToString() == "" ? "" : DateTime.Parse(DataBinder.Eval(Container.DataItem, "commitment").ToString()).ToShortDateString()) %>' /></td>
                                <td valign="top" align="center"><%# DataBinder.Eval(Container.DataItem, "confidence") %></td>
                                <td valign="top" align="center" nowrap>
                                    <asp:Panel ID="panStep" runat="server" Visible="false">
                                        <asp:Label ID="lblStep" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "step") %>' />                                    </asp:Panel>
                                    <asp:Panel ID="panComplete" runat="server" Visible="false">
                                        <table cellpadding="0" cellspacing="0" border="0" class="design_built">
                                            <tr>
                                                <th><img src="/images/spacer.gif" border="0" width="1" height="5" /></th>
                                            </tr>
                                            <tr>
                                                <td><img src="/images/check_small.gif" border="0" align="absmiddle" />&nbsp;<asp:Label ID="lblComplete" runat="server" CssClass="default" Text='' /></td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    <asp:Panel ID="panAbandoned" runat="server" Visible="false">
                                        <table cellpadding="0" cellspacing="0" border="0" class="design_built">
                                            <tr>
                                                <th><img src="/images/spacer.gif" border="0" width="1" height="5" /></th>
                                            </tr>
                                            <tr>
                                                <td><img src="/images/alert_small.gif" border="0" align="absmiddle" />&nbsp;<asp:Label ID="lblAbandoned" runat="server" CssClass="default" Text='' /></td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    <asp:Panel ID="panScheduled" runat="server" Visible="false">
                                        <table cellpadding="0" cellspacing="0" border="0" class="design_scheduled">
                                            <tr>
                                                <th><img src="/images/spacer.gif" border="0" width="1" height="5" /></th>
                                            </tr>
                                            <tr>
                                                <td><img src="/images/clock_small.gif" border="0" align="absmiddle" />&nbsp;<asp:Label ID="lblScheduled" runat="server" CssClass="default" Text='' /></td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                                <td valign="top" align="right" width="1">
                                    [<asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' Text="Edit" />]&nbsp;&nbsp;[<asp:LinkButton ID="btnDelete" runat="server" OnClick="btnDelete_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' CommandName='<%# DataBinder.Eval(Container.DataItem, "answerid") %>' Text="Delete" />]
                        <!--
                                    &nbsp;&nbsp;[<asp:LinkButton ID="btnExecute" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' Text="Execute" />]
                        -->
                                </td>
                            </tr>
                        </ItemTemplate>
                        <AlternatingItemTemplate>
                            <tr<%# (Request.QueryString["highlight"] != null && Request.QueryString["highlight"] == DataBinder.Eval(Container.DataItem, "id").ToString() ? " bgcolor=\"#99FF99\"" : " bgcolor=\"#F6F6F6\"")%>>
                                <asp:Label ID="lblId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "id") %>' />
                                <td valign="top"><%# DataBinder.Eval(Container.DataItem, "id") %></td>
                                <td valign="top"><%# (DataBinder.Eval(Container.DataItem, "name").ToString() == "" ? "---" : DataBinder.Eval(Container.DataItem, "name").ToString()) %></td>
                                <td valign="top"><asp:Label ID="lblOS" runat="server" CssClass="default" Text='<%# (DataBinder.Eval(Container.DataItem, "os").ToString() == "" ? "---" : DataBinder.Eval(Container.DataItem, "os").ToString()) %>' /></td>
                                <td valign="top" style='display:<%= boolDemo ? "none" : "inline" %>'><asp:Label ID="lblModel" runat="server" CssClass="default" Text='0' /></td>
                                <td valign="top" align="center" nowrap><asp:Label ID="lblQuantity" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "quantity") %>' /></td></td>
                                <td valign="top" align="right" nowrap style='display:<%= boolDemo ? "inline" : "none" %>'><asp:Label ID="lblAcquisition" runat="server" CssClass="default" /><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                                <td valign="top" align="right" nowrap style='display:<%= boolDemo ? "inline" : "none" %>'><asp:Label ID="lblOperational" runat="server" CssClass="default" /><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                                <td valign="top" align="right" nowrap><asp:Label ID="lblStorage" runat="server" CssClass="default" /><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                                <!--
                                <td valign="top" align="center" nowrap><a href="javascript:void(0);" onclick="OpenWindow('FORECAST_BACKUP','<%# DataBinder.Eval(Container.DataItem, "id") %>');"><img src="/images/backup.gif" border="0" align="absmiddle" /></a></td>
                                <td valign="top" align="right" nowrap><asp:Label ID="lblAmp" runat="server" CssClass="default" /><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                                <td valign="top" align="right" nowrap><asp:Label ID="lblHours" runat="server" CssClass="default" /><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                                -->
                                <td valign="top" align="center" nowrap><asp:Label ID="lblCommitment" runat="server" CssClass="default" Text='<%# (DataBinder.Eval(Container.DataItem, "commitment").ToString() == "" ? "" : DateTime.Parse(DataBinder.Eval(Container.DataItem, "commitment").ToString()).ToShortDateString()) %>' /></td>
                                <td valign="top" align="center"><%# DataBinder.Eval(Container.DataItem, "confidence") %></td>
                                <td valign="top" align="center" nowrap>
                                    <asp:Panel ID="panStep" runat="server" Visible="false">
                                        <asp:Label ID="lblStep" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "step") %>' />                                    </asp:Panel>
                                    <asp:Panel ID="panComplete" runat="server" Visible="false">
                                        <table cellpadding="0" cellspacing="0" border="0" class="design_built">
                                            <tr>
                                                <th><img src="/images/spacer.gif" border="0" width="1" height="5" /></th>
                                            </tr>
                                            <tr>
                                                <td><img src="/images/check_small.gif" border="0" align="absmiddle" />&nbsp;<asp:Label ID="lblComplete" runat="server" CssClass="default" Text='' /></td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    <asp:Panel ID="panAbandoned" runat="server" Visible="false">
                                        <table cellpadding="0" cellspacing="0" border="0" class="design_built">
                                            <tr>
                                                <th><img src="/images/spacer.gif" border="0" width="1" height="5" /></th>
                                            </tr>
                                            <tr>
                                                <td><img src="/images/alert_small.gif" border="0" align="absmiddle" />&nbsp;<asp:Label ID="lblAbandoned" runat="server" CssClass="default" Text='' /></td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    <asp:Panel ID="panScheduled" runat="server" Visible="false">
                                        <table cellpadding="0" cellspacing="0" border="0" class="design_scheduled">
                                            <tr>
                                                <th><img src="/images/spacer.gif" border="0" width="1" height="5" /></th>
                                            </tr>
                                            <tr>
                                                <td><img src="/images/clock_small.gif" border="0" align="absmiddle" />&nbsp;<asp:Label ID="lblScheduled" runat="server" CssClass="default" Text='' /></td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                                <td valign="top" align="right" width="1">
                                    [<asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' Text="Edit" />]&nbsp;&nbsp;[<asp:LinkButton ID="btnDelete" runat="server" OnClick="btnDelete_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' CommandName='<%# DataBinder.Eval(Container.DataItem, "answerid") %>' Text="Delete" />]
                        <!--
                                    &nbsp;&nbsp;[<asp:LinkButton ID="btnExecute" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' Text="Execute" />]
                        -->
                                </td>
                            </tr>
                        </AlternatingItemTemplate>
                    </asp:repeater>
                    <tr>
                        <td colspan="12">
                            <asp:Label ID="lblNone" runat="server" CssClass="default" Visible="false" Text="<img src='/images/spacer.gif' border='0' height='1' width='25' /><img src='/images/alert.gif' border='0' align='absmiddle'> You have not added any equipment to this forecast" />
                        </td>
                    </tr>
                    <tr bgcolor="#EEEEEE">
                        <td valign="top"></td>
                        <td valign="top"></td>
                        <td valign="top"></td>
                        <td valign="top" style='display:<%= boolDemo ? "none" : "inline" %>'></td>
                        <td valign="top" align="center"><b><asp:Label ID="lblQuantityTotal" runat="server" CssClass="default" /></b></td>
                        <td valign="top" align="right" style='display:<%= boolDemo ? "inline" : "none" %>'><b><asp:Label ID="lblAcquisitionTotal" runat="server" CssClass="default" /></b><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                        <td valign="top" align="right" style='display:<%= boolDemo ? "inline" : "none" %>'><b><asp:Label ID="lblOperationalTotal" runat="server" CssClass="default" /></b><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                        <td valign="top" align="right"><b><asp:Label ID="lblStorageTotal" runat="server" CssClass="default" /></b><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                        <!--
                        <td valign="top"></td>
                        <td valign="top" align="right"><b><asp:Label ID="lblAmpTotal" runat="server" CssClass="default" /></b><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                        <td valign="top" align="right"><b><asp:Label ID="lblHoursTotal" runat="server" CssClass="default" /></b><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                        -->
                        <td valign="top"></td>
                        <td valign="top"></td>
                        <td valign="top"></td>
                        <td valign="top"></td>
                    </tr>
                </table>
                <table width="100%" cellpadding="4" cellspacing="3" border="0">
                    <tr height="1">
                        <td colspan="2"><img src="/images/spacer.gif" border="0" width="1" height="1" /></td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                <tr>
                                    <td><asp:Button ID="btnNew" runat="Server" CssClass="default" Width="125" Text="Design Environment" /> <asp:Button ID="btnPrint" runat="Server" CssClass="default" Width="125" Text="Print Design" /></td>
                                    <td align="right"><asp:Button ID="btnDeleteForecast" runat="Server" CssClass="default" Width="125" OnClick="btnDeleteForecast_Click" Text="Delete Design" Enabled="false" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                </asp:Panel>
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
<asp:Label ID="lblRequest" runat="server" Visible="false" />
<asp:HiddenField ID="hdnManager" runat="server" />
<asp:HiddenField ID="hdnLead" runat="server" />
<asp:HiddenField ID="hdnEngineer" runat="server" />
<asp:Label ID="lblProject" runat="server" Visible="false" />
<asp:HiddenField ID="hdnSegment" runat="server" />
