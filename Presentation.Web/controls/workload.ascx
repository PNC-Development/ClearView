<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="workload.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.workload" %>


<script type="text/javascript">
    function ChangeFrame(oCell, oShow, oHide1, oHide2, oHide3) {
        oShow = document.getElementById(oShow);
        oShow.style.display = "inline";
        oHide1 = document.getElementById(oHide1);
        oHide1.style.display = "none";
        oHide2 = document.getElementById(oHide2);
        oHide2.style.display = "none";
        oHide3 = document.getElementById(oHide3);
        oHide3.style.display = "none";
    	var oRow = oCell.parentElement;
	    for (var yy=0; yy<oRow.children.length; yy++) {
    		var oNot = oRow.getElementsByTagName("td").item(yy);
    		if (oNot.className == "cmbutton")
                oNot.style.border = "1px solid #94a6b5"
    	}
	    oCell.style.borderTop = "3px solid orange"
        oCell.style.borderBottom = "1px solid #FFFFFF"
    }
    function ShowRR(strID) {
        OpenWindow("RESOURCE_REQUEST", strID);
        return false;
    }
</script>
<table width="100%" cellpadding="0" cellspacing="0" border="0" class="default">
    <tr>
        <td><img src="/images/table_topleft.gif" border="0" width="5" height="26"></td>
        <td nowrap background="/images/table_top.gif" class="greentableheader" width="100%"><asp:label ID="lblTitle" runat="server" CssClass="greetableheader" />Workload Manager</td>
        <td><img src="/images/table_topright.gif" border="0" width="5" height="26"></td>
    </tr>
    <tr>
        <td background="/images/table_left.gif"><img src="/images/table_left.gif" width="5" height="10"></td>
        <td width="100%" bgcolor="#FFFFFF">
            <asp:Panel ID="panDenied" runat="server" Visible="false">
                <br />
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
            <asp:Panel ID="panConfigure" runat="server" Visible="false">
                <br />
                <table width="100%" cellpadding="3" cellspacing="2" border="0">
                    <tr>
                        <td colspan="2" class="hugeheader"><img src="/images/project_locked.gif" border="0" align="absmiddle" /> Project Not Configured</div></td>
                    </tr>
                    <tr>
                        <td colspan="2"><%=strProjectSummary %></td>
                    </tr>
                    <tr height="1">
                        <td colspan="2"><img src="/images/spacer.gif" border="0" height="1" width="1" /></td>
                    </tr>
                    <tr>
                        <td colspan="2"><asp:LinkButton ID="btnView" runat="server" Text="Click here to view the project details" /></td>
                    </tr>
                    <tr>
                        <td colspan="2"><hr size="1" noshade /></td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <table width="100%" cellpadding="3" cellspacing="2" border="0">
                                <tr>
                                    <td nowrap>Project Number:</td>
                                    <td width="100%"><asp:Label ID="lblNumber" runat="server" CssClass="default" Visible="false" /><asp:TextBox ID="txtNumber" runat="server" CssClass="default" Width="75" MaxLength="7" Visible="false" /></td>
                                </tr>
                                <asp:Panel ID="panLink" runat="server" Visible="False">
                                <tr>
                                    <td nowrap>&nbsp;</td>
                                    <td width="100%"><img src="/images/green_right.gif" border="0" align="absmiddle" />&nbsp;<asp:HyperLink ID="hypNumber" runat="server" Text="Click here to request a clarity number" /></td>
                                </tr>
                                </asp:Panel>
                                <tr>
                                    <td nowrap>&nbsp;</td>
                                    <td width="100%"><asp:Button ID="btnConfigure" runat="server" CssClass="default" Width="75" Text="Update" OnClick="btnConfigure_Click" /></td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="center" class="header"><asp:Label ID="lblDuplicate" runat="server" CssClass="header" Text="<br /><img src='/images/bigX.gif' border='0' align='absmiddle' /> Duplicate Clarity / Project Number" Visible="false" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="panControl" runat="server" Visible="false">
                <asp:PlaceHolder ID="phControl" runat="server" />
            </asp:Panel>
            <asp:Panel ID="panWorkload" runat="server" Visible="false">
                <br />
                <table width="100%" border="0" cellSpacing="0" cellPadding="0" class="default">
                    <tr>
                        <td class="default">
                            <table width="100%" border="0" cellSpacing="2" cellPadding="2" class="default">
                                <tr id="trCommunication" runat="server" visible="false">
                                    <td colspan="2" align="center" class="bigcheck">
                                        <img src='/images/bigCheck.gif' border='0' align='absmiddle' /> Communication Sent
                                    </td>
                                </tr>
                                <tr>
                                    <td width="33%" valign="top">
                                        <%=strSummary %>
                                        <br />
                                        <asp:LinkButton ID="btnViewPR" runat="server" Text="View Original Project Request" />
                                    </td>
                                    <td width="33%" valign="top">
                                        <asp:Panel ID="panProject" runat="server" Visible="false">
                                        <table cellpadding="3" cellspacing="2" border="0">
                                            <tr>
                                                <td><b>Project Coordinator:</b></td>
                                                <td><asp:Label ID="lblCoordinator" runat="server" CssClass="default" /></td>
                                            </tr>
                                            <tr>
                                                <td><b>Phone:</b></td>
                                                <td><asp:Label ID="lblPhone" runat="server" CssClass="default" /></td>
                                            </tr>
                                            <tr>
                                                <td><b>Email:</b></td>
                                                <td><asp:Label ID="lblEmail" runat="server" CssClass="default" /></td>
                                            </tr>
                                            <tr>
                                                <td><b>Mobile Number:</b></td>
                                                <td><asp:Label ID="lblMobileDevice" runat="server" CssClass="default" /></td>
                                            </tr>
                                            <tr>
                                                <td><b>Mobile Email:</b></td>
                                                <td><asp:Label ID="lblMobileEmail" runat="server" CssClass="default" /></td>
                                            </tr>
                                        </table>
                                        </asp:Panel>
                                    </td>
                                    <td width="34%" valign="top" align="right">
                                        <table cellpadding="2" cellspacing="2" border="0">
                                            <tr>
                                                <td rowspan="2"><asp:LinkButton ID="btnBack1" runat="server" Text="<img src='/images/bigArrowLeft.gif' border='0' align='absmiddle' />" OnClick="btnBack_Click" /></td>
                                                <td class="bigblue" valign="bottom"><asp:LinkButton ID="btnBack2" runat="server" CssClass="bigblue" Text="Go Back to My Work" OnClick="btnBack_Click" /></td>
                                            </tr>
                                            <tr>
                                                <td valign="top"><asp:LinkButton ID="btnBack3" runat="server" CssClass="default" Text="Click here to return to your workload manager" OnClick="btnBack_Click" /></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr height="16">
                        <td><img src="/images/spacer.gif" border="0" height="16" width="1" /></td>
                    </tr>
                    <tr>
                        <td>
                            <table width="100%" height="100%" border="0" cellSpacing="0" cellPadding="0" class="default">
                                <tr>
                                    <td class="cmbutton" style='<%=boolMine == true ? "display:inline;border-bottom:1px solid #FFFFFF;border-top:3px solid orange;" : "" %>' onclick="ChangeFrame(this,'divMine','divInvolvement','divDocuments','divMyDocuments');">My Tasks</td>
                                    <td class="cmbuttonspace">&nbsp;</td>
                                    <td class="cmbutton" style='<%=boolMyDocuments == true ? "display:inline;border-bottom:1px solid #FFFFFF;border-top:3px solid orange;" : "" %>' onclick="ChangeFrame(this,'divMyDocuments','divDocuments','divMine','divInvolvement');">My Documents</td>
                                    <td class="cmbuttonspace">&nbsp;</td>
                                    <td class="cmbutton" style='<%=boolDocuments == true ? "display:inline;border-bottom:1px solid #FFFFFF;border-top:3px solid orange;" : "" %>' onclick="ChangeFrame(this,'divDocuments','divMine','divInvolvement','divMyDocuments');">Project Documents</td>
                                </tr>
                                <tr>
                                    <td colspan="7" align="center" class="cmcontents">
                                        <table width="100%" height="100%" border="0" cellSpacing="2" cellPadding="2" class="default">
                                            <tr> 
                                                <td valign="top">
	                                                <div id="divMine"  style='<%=boolMine == true ? "display:inline" : "display:none" %>'>
	                                                    <br />
                                                        <table width="100%" cellpadding="0" cellspacing="5" border="0">
                                                            <tr>
                                                                <td rowspan="2" valign="top"><img src="/images/tasks.gif" border="0" align="absmiddle" /></td>
                                                                <td class="hugeheader" width="100%" valign="bottom">My Tasks</td>
                                                            </tr>
                                                            <tr>
                                                                <td width="100%" valign="top">Here you can find all the tasks that you are responsible for completing.</td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2" background="/images/lines.gif">&nbsp;</td>
                                                            </tr>
                                                        </table><br />
                                                        <table width="100%" cellpadding="2" cellspacing="1" border="0">
                                                            <tr>
                                                                <td>
                                                                    <table width="100%" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                                                                        <tr bgcolor="#EEEEEE">
                                                                            <td width="1"></td>
                                                                            <td width="1"></td>
                                                                            <td nowrap><b>Task Number</b></td>
                                                                            <td nowrap><b>Task Name</b></td>
                                                                            <td nowrap><b>Role</b></td>
                                                                            <td nowrap align="right"><b>Allocated</b></td>
                                                                            <td nowrap align="right"><b>Used</b></td>
                                                                            <td nowrap align="right"><b>Created</b></td>
                                                                            <td nowrap align="right"><b>Updated</b></td>
                                                                            <td nowrap align="right"><b>Completed</b></td>
                                                                            <td nowrap align="center"><b>Status</b></td>
                                                                        </tr>
                                                                        <asp:repeater ID="rptMine" runat="server">
                                                                            <ItemTemplate>
                                                                                <tr class='<%# (DataBinder.Eval(Container.DataItem, "new").ToString() == "1" ? "bold" : "default") %>' onmouseover='<%# DataBinder.Eval(Container.DataItem, "itemid").ToString() == "-1" ? "" : "CellRowOver(this);" %>' onmouseout='<%# DataBinder.Eval(Container.DataItem, "itemid").ToString() == "-1" ? "" : "CellRowOut(this);" %>' onclick='<%# DataBinder.Eval(Container.DataItem, "itemid").ToString() == "-1" ? "" : "ShowRR(\"" + DataBinder.Eval(Container.DataItem, "id") + "\",\"" + intPage.ToString() + "\");" %>'>
                                                                                    <asp:Label ID="lblId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "id") %>' />
                                                                                    <asp:Label ID="lblServiceId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "serviceid") %>' />
                                                                                    <asp:Label ID="lblUser" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "userid") %>' />
                                                                                    <td width="1"><asp:Image ID="imgDelegate" Visible="false" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/delegate.gif" /></td>
                                                                                    <td width="1" valign="top" align="right"><asp:Label ID="lblColor" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "green").ToString() + "_" + DataBinder.Eval(Container.DataItem, "yellow").ToString() + "_" + DataBinder.Eval(Container.DataItem, "red").ToString() %>' ToolTip='<%# DataBinder.Eval(Container.DataItem, "comments").ToString() %>' /></td>
                                                                                    <td><%# DataBinder.Eval(Container.DataItem, "number") %></td>
                                                                                    <td><asp:Label ID="lblName" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "name") %>' /></td>
                                                                                    <td><asp:Label ID="lblItem" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "itemid") %>' /></td>
                                                                                    <td align="right"><asp:Label ID="lblAllocated" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "allocated") %>' /></td>
                                                                                    <td align="right"><asp:Label ID="lblUsed" runat="server" CssClass="default" Text='' /></td>
                                                                                    <td align="right"><%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "created").ToString()).ToShortDateString() %></td>
                                                                                    <td align="right"><%# DataBinder.Eval(Container.DataItem, "modified").ToString() == "" ? "---" : DateTime.Parse(DataBinder.Eval(Container.DataItem, "modified").ToString()).ToShortDateString() %></td>
                                                                                    <td align="right"><asp:Label ID="lblPercent" runat="server" CssClass="default" Text='' /></td>
                                                                                    <td align="center"><asp:Label ID="lblStatus" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "status") %>' /></td>
                                                                                </tr>
                                                                            </ItemTemplate>
                                                                        </asp:repeater>
                                                                    <tr>
                                                                        <td colspan="9">
                                                                            <asp:Label ID="lblNoMine" runat="server" CssClass="default" Visible="false" Text="<img src='/images/alert.gif' border='0' align='absmiddle'> You have no tasks associated with this project" />
                                                                        </td>
                                                                    </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>&nbsp;</td>
                                                            </tr>
                                                            <tr>
                                                                <td><img src="/images/delegate.gif" border="0" align="absmiddle" /> = Covering for an Out of Office Buddy</td>
                                                            </tr>
                                                        </table>
                                                    </div>
	                                                <div id="divMyDocuments" style='<%=boolMyDocuments == true ? "display:inline" : "display:none" %>'>
	                                                    <br />
                                                        <table width="100%" cellpadding="0" cellspacing="5" border="0">
                                                            <tr>
                                                                <td rowspan="2" valign="top"><img src="/images/documents_mine.gif" border="0" align="absmiddle" /></td>
                                                                <td class="hugeheader" width="100%" valign="bottom">My Documents</td>
                                                            </tr>
                                                            <tr>
                                                                <td width="100%" valign="top">My Documents are documents that are only for you, or can be shared with others.  To add a document and configure the permissions, click the <b>upload</b> button.</td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2" background="/images/lines.gif">&nbsp;</td>
                                                            </tr>
                                                        </table><br />
                                                        <table width="100%" cellpadding="2" cellspacing="1" border="0">
                                                            <tr>
                                                                <td><asp:Button ID="btnDocuments" runat="server" Text="Upload" Width="100" CssClass="default" /></td>
                                                                <td align="right"><asp:CheckBox ID="chkMyDescription" runat="server" CssClass="default" Text="Show Descriptions" AutoPostBack="true" OnCheckedChanged="chkMyDescription_Change" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2"><asp:Label ID="lblMyDocuments" runat="server" CssClass="default" /></td>
                                                            </tr>
                                                        </table>
                                                        <p>&nbsp;</p>
	                                                </div>
	                                                <div id="divDocuments" style='<%=boolDocuments == true ? "display:inline" : "display:none" %>'>
	                                                    <br />
                                                        <table width="100%" cellpadding="0" cellspacing="5" border="0">
                                                            <tr>
                                                                <td rowspan="2" valign="top"><img src="/images/documents.gif" border="0" align="absmiddle" /></td>
                                                                <td class="hugeheader" width="100%" valign="bottom">Project Documents</td>
                                                            </tr>
                                                            <tr>
                                                                <td width="100%" valign="top">Project Documents are shared documents that can be viewed by other project resources.</td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2" background="/images/lines.gif">&nbsp;</td>
                                                            </tr>
                                                        </table><br />
                                                        <table width="100%" cellpadding="2" cellspacing="1" border="0">
                                                            <tr>
                                                                <td align="right"><asp:CheckBox ID="chkDescription" runat="server" CssClass="default" Text="Show Descriptions" AutoPostBack="true" OnCheckedChanged="chkDescription_Change" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td><asp:Label ID="lblDocuments" runat="server" CssClass="default" /></td>
                                                            </tr>
                                                        </table>
                                                        <p>&nbsp;</p>
	                                                </div>
	                                                <div id="divInvolvement"  style='<%=boolResource == true ? "display:inline" : "display:none" %>'>
	                                                    <br />
                                                        <table width="100%" cellpadding="0" cellspacing="5" border="0">
                                                            <tr>
                                                                <td rowspan="2" valign="top"><img src="/images/users40.gif" border="0" align="absmiddle" /></td>
                                                                <td class="hugeheader" width="100%" valign="bottom">Resource Involvement</td>
                                                            </tr>
                                                            <tr>
                                                                <td width="100%" valign="top">The following list shows all resources involved in this project. Send a Communication to someone by completing the form at the bottom.</td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2" background="/images/lines.gif">&nbsp;</td>
                                                            </tr>
                                                        </table><br />
                                                        <table width="100%" cellpadding="2" cellspacing="1" border="0">
                                                            <tr>
                                                                <td>
                                                                    <table width="100%" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                                                                        <tr bgcolor="#EEEEEE">
                                                                            <td nowrap><b>Resource</b></td>
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
                                                                                    <td><asp:Label ID="lblUser" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "userid") %>' /></td>
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
                                                                            <td colspan="2" class="greentableheader">Send Communication</td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td nowrap>Resource:</td>
                                                                            <td width="100%"><asp:DropDownList ID="ddlResource" runat="server" CssClass="default" Width="250" /></td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td nowrap>Communication:</td>
                                                                            <td width="100%">
                                                                                <asp:DropDownList ID="ddlCommunication" runat="server" CssClass="default" Width="250">
                                                                                    <asp:ListItem Value="-- SELECT --" />
                                                                                    <asp:ListItem Value="Email" />
                                                                                    <asp:ListItem Value="Pager" />
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td nowrap>Message:</td>
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
<asp:Label ID="lblProject" runat="server" Visible="false" />