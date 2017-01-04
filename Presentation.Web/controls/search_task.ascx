<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="search_task.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.search_task" %>


<script type="text/javascript">
    function ValidateSearch(oH, oNA, oNU, oBy) {
        oH = document.getElementById(oH);
        if (oH.value == "1") {
            oNA = document.getElementById(oNA);
            oNU = document.getElementById(oNU);
            oBy = document.getElementById(oBy);
            if (trim(oNA.value) == "" && trim(oNU.value) == "" && oBy.value == 0)
            {
                alert('Please enter a task name, task number or submitter');
                oNA.focus();
                return false;
            }
        }
        return true;
    }
    function ViewWorkload(strUrl) {
        if(event.srcElement.tagName != 'A')
            window.navigate(strUrl);
    }
    function ShowDetail(oImg, oDiv) {
        oImg = document.getElementById(oImg);
        oDiv = document.getElementById(oDiv);
        if (oDiv.style.display == 'inline') {
            oDiv.style.display = 'none';
            SwapImage(oImg, '/images/biggerPlus.gif');
        }
        else {
            oDiv.style.display = 'inline';
            SwapImage(oImg, '/images/biggerMinus.gif');
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
            <asp:Panel ID="panSearch" runat="server" Visible="false">
            <%=strMenuTab1 %>
            <div id="divMenu1"> 
            <!-- 
                <table cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td><img src="/images/TabEmptyBackground.gif"></td>
                        <td><img src="/images/TabOnLeftCap.gif"></td>
                        <td nowrap background="/images/TabOnBackground.gif"><a href="javascript:void(0);" onclick="ChangeTab(this,'divTab1','<%=hdnType.ClientID%>','O',true);" class="tabheader">Overall Search</a></td>
                        <td><img src="/images/TabOnRightCap.gif"></td>
                        <td><img src="/images/TabOffLeftCap.gif"></td>
                        <td nowrap background="/images/TabOffBackground.gif"><a href="javascript:void(0);" onclick="ChangeTab(this,'divTab2','<%=hdnType.ClientID%>','P',true);" class="tabheader">Search By Department</a></td>
                        <td><img src="/images/TabOffRightCap.gif"></td>
                        <td><img src="/images/TabOffLeftCap.gif"></td>
                        <td nowrap background="/images/TabOffBackground.gif"><a href="javascript:void(0);" onclick="ChangeTab(this,'divTab3','<%=hdnType.ClientID%>','G',true);" class="tabheader">Search By Group</a></td>
                        <td><img src="/images/TabOffRightCap.gif"></td>
                        <td><img src="/images/TabOffLeftCap.gif"></td>
                        <td nowrap background="/images/TabOffBackground.gif"><a href="javascript:void(0);" onclick="ChangeTab(this,'divTab4','<%=hdnType.ClientID%>','L',true);" class="tabheader">Search By Team Lead</a></td>
                        <td><img src="/images/TabOffRightCap.gif"></td>
                        <td><img src="/images/TabOffLeftCap.gif"></td>
                        <td nowrap background="/images/TabOffBackground.gif"><a href="javascript:void(0);" onclick="ChangeTab(this,'divTab5','<%=hdnType.ClientID%>','T',true);" class="tabheader">Search By Resource</a></td>
                        <td><img src="/images/TabOffRightCap.gif"></td>
                        <td width="100%" background="/images/TabEmptyBackground.gif">&nbsp;</td>
                    </tr>
                </table>
                -->
             
                <div style="display:none">
                    <br />
                    <table width="100%" cellpadding="0" cellspacing="5" border="0">
                        <tr>
                            <td rowspan="2"><img src="/images/bigSearch.gif" border="0" align="absmiddle" /></td>
                            <td class="header" width="100%" valign="bottom"> Task Search</td>
                        </tr>
                        <tr>
                            <td width="100%" valign="top">Searches all tasks in the system, regardless of your department.</td>
                        </tr>
                    </table>
                    <table width="100%" cellpadding="4" cellspacing="3" border="0">
                        <tr>
                            <td nowrap>Task Name:</td>
                            <td width="100%"><asp:TextBox ID="txtName" runat="server" CssClass="default" Width="300" /></td>
                        </tr>
                        <tr>
                            <td nowrap>Task Number:</td>
                            <td width="100%"><asp:TextBox ID="txtNumber" runat="server" CssClass="default" Width="200" /></td>
                        </tr>
                        <tr>
                            <td nowrap>Status:</td>
                            <td width="100%">
                                <asp:DropDownList ID="ddlOStatus" runat="server" CssClass="default" Width="200">
                                    <asp:ListItem Value="-2" Text="Cancelled" />
                                    <asp:ListItem Value="2" Text="Active" />
                                    <asp:ListItem Value="3" Text="Completed" />
                                    <asp:ListItem Value="5" Text="On Hold" />
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td nowrap>Submitted By:</td>
                            <td width="100%">
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <tr>
                                        <td><asp:TextBox ID="txtSubmittedBy" runat="server" Width="300" CssClass="default" /></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div id="divSubmittedBy" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                                                <asp:ListBox ID="lstSubmittedBy" runat="server" CssClass="default" />
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td nowrap>Submitted On:</td>
                            <td width="100%"><asp:TextBox ID="txtOStart" runat="server" CssClass="default" Width="100" MaxLength="10" /> <asp:ImageButton ID="imgOStart" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /> - <asp:TextBox ID="txtOEnd" runat="server" CssClass="default" Width="100" MaxLength="10" /> <asp:ImageButton ID="imgOEnd" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
                        </tr>
                        <tr>
                            <td colspan="2">&nbsp;</td>
                        </tr>
                    </table>
                </div>
                <div style="display:none">
                    <br />
                    <table width="100%" cellpadding="0" cellspacing="5" border="0">
                        <tr>
                            <td rowspan="2"><img src="/images/bigSearch.gif" border="0" align="absmiddle" /></td>
                            <td class="header" width="100%" valign="bottom"> Task Search By Department</td>
                        </tr>
                        <tr>
                            <td width="100%" valign="top">Searches all tasks in the system in which you or someone in your department is working.</td>
                        </tr>
                    </table>
                    <table width="100%" cellpadding="4" cellspacing="3" border="0">
                        <tr>
                            <td nowrap>Department:</td>
                            <td width="100%">
                                <asp:DropDownList ID="ddlDepartment" runat="server" CssClass="default" Width="300" />
                            </td>
                        </tr>
                        <tr>
                            <td nowrap>Status:</td>
                            <td width="100%">
                                <asp:DropDownList ID="ddlDStatus" runat="server" CssClass="default" Width="150">
                                    <asp:ListItem Value="-2" Text="Cancelled" />
                                    <asp:ListItem Value="2" Text="Active" />
                                    <asp:ListItem Value="3" Text="Completed" />
                                    <asp:ListItem Value="5" Text="On Hold" />
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">&nbsp;</td>
                        </tr>
                    </table>
                </div>
                <div style="display:none">
                    <br />
                    <table width="100%" cellpadding="0" cellspacing="5" border="0">
                        <tr>
                            <td rowspan="2"><img src="/images/bigSearch.gif" border="0" align="absmiddle" /></td>
                            <td class="header" width="100%" valign="bottom"> Task Search By Group</td>
                        </tr>
                        <tr>
                            <td width="100%" valign="top">Searches all tasks in the system in which a group in your department is working.</td>
                        </tr>
                    </table>
                    <table width="100%" cellpadding="4" cellspacing="3" border="0">
                        <tr>
                            <td nowrap>Group:</td>
                            <td width="100%">
                                <asp:DropDownList ID="ddlGroup" runat="server" CssClass="default" Width="300" />
                            </td>
                        </tr>
                        <tr>
                            <td nowrap>Status:</td>
                            <td width="100%">
                                <asp:DropDownList ID="ddlGStatus" runat="server" CssClass="default" Width="150">
                                    <asp:ListItem Value="-2" Text="Cancelled" />
                                    <asp:ListItem Value="2" Text="Active" />
                                    <asp:ListItem Value="3" Text="Completed" />
                                    <asp:ListItem Value="5" Text="On Hold" />
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">&nbsp;</td>
                        </tr>
                    </table>
                </div>
                <div style="display:none">
                    <br />
                    <table width="100%" cellpadding="0" cellspacing="5" border="0">
                        <tr>
                            <td rowspan="2"><img src="/images/bigSearch.gif" border="0" align="absmiddle" /></td>
                            <td class="header" width="100%" valign="bottom"> Task Search By Team Lead</td>
                        </tr>
                        <tr>
                            <td width="100%" valign="top">Searches all tasks in the system assigned by a team lead, or being performed by a resource under a team lead.</td>
                        </tr>
                    </table>
                    <table width="100%" cellpadding="4" cellspacing="3" border="0">
                        <tr>
                            <td nowrap>Team Lead:</td>
                            <td width="100%">
                                <asp:DropDownList ID="ddlLead" runat="server" CssClass="default" Width="300" />
                            </td>
                        </tr>
                        <tr>
                            <td nowrap>Status:</td>
                            <td width="100%">
                                <asp:DropDownList ID="ddlLStatus" runat="server" CssClass="default" Width="150">
                                    <asp:ListItem Value="-2" Text="Cancelled" />
                                    <asp:ListItem Value="2" Text="Active" />
                                    <asp:ListItem Value="3" Text="Completed" />
                                    <asp:ListItem Value="5" Text="On Hold" />
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">&nbsp;</td>
                        </tr>
                    </table>
                </div>
                <div style="display:none">
                    <br />
                    <table width="100%" cellpadding="0" cellspacing="5" border="0">
                        <tr>
                            <td rowspan="2"><img src="/images/bigSearch.gif" border="0" align="absmiddle" /></td>
                            <td class="header" width="100%" valign="bottom"> Task Search By Resource</td>
                        </tr>
                        <tr>
                            <td width="100%" valign="top">Searches all tasks in the system in which a resource in your department is working.</td>
                        </tr>
                    </table>
                    <table width="100%" cellpadding="4" cellspacing="3" border="0">
                        <tr>
                            <td nowrap>Resource:</td>
                            <td width="100%"><asp:DropDownList ID="ddlTechnician" runat="server" CssClass="default" Width="300" /></td>
                        </tr>
                        <tr>
                            <td nowrap>Status:</td>
                            <td width="100%">
                                <asp:DropDownList ID="ddlTStatus" runat="server" CssClass="default" Width="150">
                                    <asp:ListItem Value="-2" Text="Cancelled" />
                                    <asp:ListItem Value="2" Text="Active" />
                                    <asp:ListItem Value="3" Text="Completed" />
                                    <asp:ListItem Value="5" Text="On Hold" />
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">&nbsp;</td>
                        </tr>
                    </table>
                </div>
                <table width="100%" cellpadding="4" cellspacing="3" border="0">
                    <tr>
                        <td><hr size="1" noshade /></td>
                    </tr>
                    <tr>
                        <td align="right"><asp:Button ID="btnSearch" runat="server" CssClass="default" Text="Search" Width="75" OnClick="btnSearch_Click" /></td>
                    </tr>
                </table>
                </div>
            </asp:Panel>
            <asp:Panel ID="panResults" runat="server" Visible="false">
                <table width="100%" cellpadding="4" cellspacing="3" border="0">
                    <tr>
                        <td class="header"><img src="/images/bigSearch.gif" border="0" align="absmiddle" /> Search Results</td>
                        <td align="right"><img src="/images/check.gif" border="0" align="absmiddle" /> <asp:HyperLink ID="hypClear" runat="server" Text="Start a New Search" /></td>
                    </tr>
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
                            <asp:repeater ID="rptView" runat="server">
                                <HeaderTemplate>
                                    <tr bgcolor="#EEEEEE">
                                        <td><asp:LinkButton ID="btnOrderName" runat="server" CssClass="tableheader" Text="<b>Name:</b>" OnClick="btnOrder_Click" CommandArgument="name" /></td>
                                        <td><asp:LinkButton ID="btnOrderNumber" runat="server" CssClass="tableheader" Text="<b>Number:</b>" OnClick="btnOrder_Click" CommandArgument="number" /></td>
                                        <td class="tableheader"><b>Status:</b></td>
                                        <td class="tableheader"><b>Requestor:</b></td>
                                        <td class="tableheader"><b>Last Updated:</b></td>
                                        <td class="tableheader"><b>Progress:</b></td>
                                    </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr class="default" onmouseover="CellRowOver(this);" onmouseout="CellRowOut(this);" onclick="ViewWorkload('<%# DataBinder.Eval(Container.DataItem, "query") %>');">
                                        <td nowrap valign="top"><%# DataBinder.Eval(Container.DataItem, "name")%></td>
                                        <td nowrap valign="top"><asp:label ID="lblNumber" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "number") %>' /></td>
                                        <td nowrap valign="top"><asp:label ID="lblStatus" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "status") %>' /></td>
                                        <td nowrap valign="top"><asp:label ID="lblRequestor" runat="server" CssClass="default" Text='' /></td>
                                        <td nowrap valign="top"><asp:label ID="lblUpdated" runat="server" CssClass="default" Text='' /></td>
                                        <td nowrap valign="top"><asp:label ID="lblProgress" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "query") %>' /></td>
                                    </tr>
                                </ItemTemplate>
                                <AlternatingItemTemplate>
                                    <tr class="default" bgcolor="#F6F6F6" onmouseover="CellRowOver(this);" onmouseout="CellRowOut(this);" onclick="ViewWorkload('<%# DataBinder.Eval(Container.DataItem, "query") %>');">
                                        <td nowrap valign="top"><%# DataBinder.Eval(Container.DataItem, "name")%></td>
                                        <td nowrap valign="top"><asp:label ID="lblNumber" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "number") %>' /></td>
                                        <td nowrap valign="top"><asp:label ID="lblStatus" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "status") %>' /></td>
                                        <td nowrap valign="top"><asp:label ID="lblRequestor" runat="server" CssClass="default" Text='' /></td>
                                        <td nowrap valign="top"><asp:label ID="lblUpdated" runat="server" CssClass="default" Text='' /></td>
                                        <td nowrap valign="top"><asp:label ID="lblProgress" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "query") %>' /></td>
                                    </tr>
                                </AlternatingItemTemplate>
                            </asp:repeater>
                            <tr>
                                <td colspan="2"><asp:Label ID="lblNone" runat="server" CssClass="error" Visible="false" Text="<img src='/images/alert.gif' border='0' align='absmiddle'> There are no items" /></td>
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
            <asp:Panel ID="panResult" runat="server" Visible="false">
                <table width="100%" cellpadding="0" cellspacing="5" border="0">
                    <tr>
                        <td rowspan="2"><img src="/images/ProjectResults.gif" border="0" align="absmiddle" /></td>
                        <td class="header" width="100%" valign="bottom"><asp:Label ID="lblTaskName" runat="server" CssClass="header" /></td>
                    </tr>
                    <tr>
                        <td width="100%" valign="top">The following shows all of the information collected about the current task.</td>
                    </tr>
                </table>
                <br />
                <table cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td><img src="/images/TabOnLeftCap.gif"></td>
                        <td nowrap background="/images/TabOnBackground.gif"><a href="javascript:void(0);" onclick="ChangeTab(this,'divTab10',null,null,true);" class="tabheader">Details</a></td>
                        <td><img src="/images/TabOnRightCap.gif"></td>
                        <td><img src="/images/TabOffLeftCap.gif"></td>
                        <td nowrap background="/images/TabOffBackground.gif"><a href="javascript:void(0);" onclick="ChangeTab(this,'divTab11',null,null,true);" class="tabheader">Priority</a></td>
                        <td><img src="/images/TabOffRightCap.gif"></td>
                        <td><img src="/images/TabOffLeftCap.gif"></td>
                        <td nowrap background="/images/TabOffBackground.gif"><a href="javascript:void(0);" onclick="ChangeTab(this,'divTab12',null,null,true);" class="tabheader">Documents</a></td>
                        <td><img src="/images/TabOffRightCap.gif"></td>
                        <td><img src="/images/TabOffLeftCap.gif"></td>
                        <td nowrap background="/images/TabOffBackground.gif"><a href="javascript:void(0);" onclick="ChangeTab(this,'divTab13',null,null,true);" class="tabheader">History</a></td>
                        <td><img src="/images/TabOffRightCap.gif"></td>
                        <td><img src="/images/TabOffLeftCap.gif"></td>
                        <td nowrap background="/images/TabOffBackground.gif"><a href="javascript:void(0);" onclick="ChangeTab(this,'divTab14',null,null,true);" class="tabheader">Service Progress</a></td>
                        <td><img src="/images/TabOffRightCap.gif"></td>
                        <td><img src="/images/TabOffLeftCap.gif"></td>
                        <td nowrap background="/images/TabOffBackground.gif"><a href="javascript:void(0);" onclick="ChangeTab(this,'divTab15',null,null,true);" class="tabheader">Resource Involvement</a></td>
                        <td><img src="/images/TabOffRightCap.gif"></td>
                        <td width="100%" background="/images/TabEmptyBackground.gif">&nbsp;</td>
                    </tr>
                </table>
<!--            <div style="border:#007253 1px solid;border-top:none;padding:10px;width:100%;">-->
                <br />
                <div id="divTab10" style="display:inline">
                    <table width="100%" cellpadding="3" cellspacing="2" border="0">
                        <tr>
                            <td class="wrapper">Task Details</td>
                        </tr>
                        <tr>
                            <td><%=strDetails %></td>
                        </tr>
                    </table>
                </div>
                <div id="divTab11" style="display:none">
                    <table width="100%" cellpadding="3" cellspacing="2" border="0">
                        <tr>
                            <td><%=strPriority %></td>
                        </tr>
                    </table>
                </div>
                <div id="divTab12" style="display:none">
                    <table width="100%" cellpadding="3" cellspacing="2" border="0">
                        <tr>
                            <td><%=strDocuments %></td>
                        </tr>
                    </table>
                </div>
                <div id="divTab13" style="display:none">
                    <table width="100%" cellpadding="3" cellspacing="2" border="0">
                        <tr>
                            <td><img src='/images/alert.gif' border='0' align='absmiddle'> There is no available history</td>
                        </tr>
                    </table>
                </div>
                <div id="divTab14" style="display:none">
                    <table width="100%" cellpadding="3" cellspacing="2" border="0">
                        <tr>
                            <td colspan="2"><%=strProject %></td>
                        </tr>
                        <asp:Panel ID="panReview" runat="server" Visible="true">
                        <tr>
                            <td colspan="2"><hr size="1" noshade /></td>
                        </tr>
                        <tr>
                            <td><asp:LinkButton ID="btnExpand" runat="server" Text="Expand All" OnClick="btnExpand_Click" /></td>
                            <td align="right">Sort By: <asp:LinkButton ID="btnSortService" runat="server" Text="Service" CommandArgument="itemid" OnClick="btnSort_Click" /> | <asp:LinkButton ID="btnSortUser" runat="server" Text="Resource" CommandArgument="userid" OnClick="btnSort_Click" /></td>
                        </tr>
                        </asp:Panel>
                    </table>
                </div>
                <div id="divTab15" style="display:none">
                    <table width="100%" cellpadding="3" cellspacing="2" border="0">
                        <tr>
                            <td>
                                <table width="100%" cellpadding="3" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                                    <tr bgcolor="#EEEEEE">
                                        <td nowrap width="1">&nbsp;</td>
                                        <td nowrap><b>Technician</b></td>
                                        <td nowrap><b>Name</b></td>
                                        <td nowrap><b>Department</b></td>
                                        <td nowrap align="right"><b>Allocated</b></td>
                                        <td nowrap align="right"><b>Used</b></td>
                                        <td nowrap align="right"><b>Completed</b></td>
                                        <td nowrap align="center"><b>Status</b></td>
                                    </tr>
                                    <asp:repeater ID="rptInvolvement" runat="server">
                                        <ItemTemplate>
                                            <tr class="default">
                                                <asp:Label ID="lblId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "id") %>' />
                                                <td width="1"><asp:Label ID="lblImage" runat="server" CssClass="default" Text='' /></td>
                                                <td><asp:Label ID="lblUser" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "userid") %>' /></td>
                                                <td><%# DataBinder.Eval(Container.DataItem, "name") %></td>
                                                <td><asp:Label ID="lblItem" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "itemid") %>' /></td>
                                                <td align="right"><asp:Label ID="lblAllocated" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "allocated") %>' /></td>
                                                <td align="right"><asp:Label ID="lblUsed" runat="server" CssClass="default" Text='' /></td>
                                                <td align="right"><asp:Label ID="lblPercent" runat="server" CssClass="default" Text='' /></td>
                                                <td align="center"><asp:Label ID="lblStatus" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "status") %>' /></td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:repeater>
                                <tr>
                                    <td colspan="7">
                                        <asp:Label ID="lblNoInvolvement" runat="server" CssClass="default" Visible="false" Text="<img src='/images/alert.gif' border='0' align='absmiddle'> There are no resources involved in this project" />
                                    </td>
                                </tr>
                                </table>
                                <br /><br />
                                <table width="100%" cellpadding="3" cellspacing="2" border="0">
                                    <tr>
                                        <td colspan="2" class="wrapper">Send Communication</td>
                                    </tr>
                                    <tr>
                                        <td nowrap>Resource:<font class="required">&nbsp;*</font></td>
                                        <td width="100%"><asp:DropDownList ID="ddlResource" runat="server" CssClass="default" Width="250" /></td>
                                    </tr>
                                    <tr>
                                        <td nowrap>Communication:<font class="required">&nbsp;*</font></td>
                                        <td width="100%">
                                            <asp:DropDownList ID="ddlCommunication" runat="server" CssClass="default" Width="250">
                                                <asp:ListItem Value="-- SELECT --" />
                                                <asp:ListItem Value="Email" />
                                                <asp:ListItem Value="Pager" />
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td nowrap>Message:<font class="required">&nbsp;*</font></td>
                                        <td width="100%"><asp:TextBox ID="txtMessage" runat="server" CssClass="default" TextMode="MultiLine" Rows="8" Width="80%" /></td>
                                    </tr>
                                    <tr>
                                        <td nowrap>&nbsp;</td>
                                        <td width="100%"><asp:Button ID="btnMessage" runat="server" CssClass="default" Text="Send" Width="75" OnClick="btnMessage_Click" /></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
<!--            </div>-->
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
<asp:HiddenField ID="hdnSubmittedBy" runat="server" />
<asp:HiddenField ID="hdnSegment" runat="server" />
<asp:HiddenField ID="hdnSQL" runat="server" />
