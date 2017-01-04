<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wm_physical.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.wm_physical" %>


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
                                    <td nowrap><b>Last Updated:</b></td>
                                    <td><asp:Label ID="lblUpdated" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td nowrap><b>Nickname:</b></td>
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
            </td>
        </tr>
        <tr height="16">
            <td><img src="/images/spacer.gif" border="0" height="16" width="1" /></td>
        </tr>
        <tr>
            <td>
                    <table width="100%" height="100%" border="0" cellSpacing="0" cellPadding="0" class="default">
                        <tr>
                            <td class="cmbutton" style='<%=boolDetails == true ? "display:inline;border-bottom:1px solid #FFFFFF;border-top:3px solid orange;" : "" %>' onclick="ChangeFrame(this,'divDetails','divExecution','<%=hdnTab.ClientID %>','D');">Request Details</td>
                            <td class="cmbuttonspace">&nbsp;</td>
                            <td class="cmbutton" style='<%=boolExecution == true ? "display:inline;border-bottom:1px solid #FFFFFF;border-top:3px solid orange;" : "" %>' onclick="ChangeFrame(this,'divExecution','divDetails','<%=hdnTab.ClientID %>','E');">Execution</td>
                        </tr>
                        <tr>
                            <td colspan="3" align="center" class="cmcontents">
                                <table width="100%" height="100%" border="0" cellSpacing="2" cellPadding="2" class="default">
                                    <tr> 
                                        <td valign="top">
		                                    <div id="divDetails" style='<%=boolDetails == true ? "display:inline" : "display:none" %>'>
		                                        <br />
                                                <table width="100%" cellpadding="4" cellspacing="3" border="0">
                                                    <tr>
                                                        <td><img src="/images/check.gif" border="0" align="absmiddle" /> <asp:LinkButton ID="btnView" runat="server" Text="Click Here to View the Original Design" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td><img src="/images/icons/pdf.gif" border="0" align="absmiddle" /> <asp:LinkButton ID="btnBirth" runat="server" Text="Click Here to View the Birth Certificate" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td class="greentableheader">Request Details</td>
                                                    </tr>
                                                    <tr>
                                                        <td><asp:Label ID="lblView" runat="server" CssClass="default" /></td>
                                                    </tr>
                                                </table>
                                                <p>&nbsp;</p>
		                                    </div>
		                                    <div id="divExecution" style='<%=boolExecution == true ? "display:inline" : "display:none" %>'>
		                                        <br />
                                                <table width="100%" cellpadding="4" cellspacing="3" border="0">
                                                    <asp:Panel ID="panAdditional" runat="server" Visible="false">
                                                    <tr>
                                                        <td nowrap></td>
                                                        <td nowrap><img src="/images/alert.gif" border="0" align="absmiddle" /></td>
                                                        <td width="100%">
                                                            <table cellpadding="2" cellspacing="2" border="0">
                                                                <tr>
                                                                    <td valign="top"><b>Additional Name(s):</b></td>
                                                                    <td valign="top"><asp:Label ID="lblAdditional" runat="server" CssClass="default" /></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    </asp:Panel>
                                                    <asp:Panel ID="panHA" runat="server" Visible="false">
                                                    <tr>
                                                        <td nowrap></td>
                                                        <td nowrap><img src="/images/alert.gif" border="0" align="absmiddle" /></td>
                                                        <td width="100%">
                                                            <table cellpadding="2" cellspacing="2" border="0">
                                                                <tr>
                                                                    <td valign="top"><b>High Availability:</b></td>
                                                                    <td valign="top"><asp:Label ID="lblHA" runat="server" CssClass="default" /></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    </asp:Panel>
                                                    <tr>
                                                        <td nowrap><asp:CheckBox ID="chk1" runat="server" CssClass="default" /></td>
                                                        <td nowrap><asp:Image ID="img1" runat="server" ImageAlign="absMiddle" ImageUrl="/images/spacer.gif" /></td>
                                                        <td width="100%">Execute Request: <asp:Button ID="btnExecute" runat="server" CssClass="default" Text="Execute" Width="125" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap></td>
                                                        <td nowrap></td>
                                                        <td width="100%"><asp:Label ID="lbl1" runat="server" CssClass="default" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap><asp:CheckBox ID="chk3" runat="server" CssClass="default" Enabled="false" /></td>
                                                        <td nowrap><asp:Image ID="img3" runat="server" ImageAlign="absMiddle" ImageUrl="/images/spacer.gif" /></td>
                                                        <td width="100%">Update Service Center Request&nbsp;&nbsp;&nbsp;[<asp:LinkButton ID="btnSC" runat="server" Text="Click Here for Details" />]</td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap><asp:CheckBox ID="chk4" runat="server" CssClass="default" Enabled="false" /></td>
                                                        <td nowrap><asp:Image ID="img4" runat="server" ImageAlign="absMiddle" ImageUrl="/images/spacer.gif" /></td>
                                                        <td width="100%">Validate Auto-Provisioning Process</td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap><asp:CheckBox ID="chk5" runat="server" CssClass="default" Enabled="false" /></td>
                                                        <td nowrap><asp:Image ID="img5" runat="server" ImageAlign="absMiddle" ImageUrl="/images/spacer.gif" /></td>
                                                        <td width="100%">Reserve Ecom / OffShore Dev Server (if applicable)</td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap></td>
                                                        <td nowrap></td>
                                                        <td width="100%"><asp:Label ID="lbl5" runat="server" CssClass="default" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap><asp:CheckBox ID="chk6" runat="server" CssClass="default" Enabled="false" /></td>
                                                        <td nowrap><asp:Image ID="img6" runat="server" ImageAlign="absMiddle" ImageUrl="/images/spacer.gif" /></td>
                                                        <td width="100%">Move the image to Ecom / OffShore Dev & configure (if applicable)</td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap></td>
                                                        <td nowrap></td>
                                                        <td width="100%"><asp:Label ID="lbl6" runat="server" CssClass="default" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap><asp:CheckBox ID="chk7" runat="server" CssClass="default" Enabled="false" /></td>
                                                        <td nowrap><asp:Image ID="img7" runat="server" ImageAlign="absMiddle" ImageUrl="/images/spacer.gif" /></td>
                                                        <td width="100%">Allocate Storage (if applicable) <span class="required"> - <b>NOTE:</b> Be sure to contact the storage team to make sure they are done.</span></td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap><asp:CheckBox ID="chk8" runat="server" CssClass="default" Enabled="false" /></td>
                                                        <td nowrap><asp:Image ID="img8" runat="server" ImageAlign="absMiddle" ImageUrl="/images/spacer.gif" /></td>
                                                        <td width="100%">Configure CSM (if applicable)</td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap><asp:CheckBox ID="chk9" runat="server" CssClass="default" Enabled="false" /></td>
                                                        <td nowrap><asp:Image ID="img9" runat="server" ImageAlign="absMiddle" ImageUrl="/images/spacer.gif" /></td>
                                                        <td width="100%">Configure Storage and Clustering (if applicable)</td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap></td>
                                                        <td nowrap></td>
                                                        <td width="100%"><asp:Label ID="lbl9" runat="server" CssClass="default" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap><asp:CheckBox ID="chk10" runat="server" CssClass="default" Enabled="false" /></td>
                                                        <td nowrap><asp:Image ID="img10" runat="server" ImageAlign="absMiddle" ImageUrl="/images/spacer.gif" /></td>
                                                        <td width="100%">Install Applications (if F Drive has been replaced)</td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap></td>
                                                        <td nowrap></td>
                                                        <td width="100%"><asp:Label ID="lbl10" runat="server" CssClass="default" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap><asp:CheckBox ID="chk11" runat="server" CssClass="default" Enabled="false" /></td>
                                                        <td nowrap><asp:Image ID="img11" runat="server" ImageAlign="absMiddle" ImageUrl="/images/spacer.gif" /></td>
                                                        <td width="100%">Label device(s) with name</td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap><asp:CheckBox ID="chk12" runat="server" CssClass="default" Enabled="false" /></td>
                                                        <td nowrap><asp:Image ID="img12" runat="server" ImageAlign="absMiddle" ImageUrl="/images/spacer.gif" /></td>
                                                        <td width="100%">Reserve Production Asset: <asp:Button ID="btnProduction" runat="server" CssClass="default" Text="Reserve" Width="125" OnClick="btnProduction_Click" /></td></td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap></td>
                                                        <td nowrap></td>
                                                        <td width="100%"><asp:Label ID="lbl12" runat="server" CssClass="default" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap><asp:CheckBox ID="chk13" runat="server" CssClass="default" Enabled="false" /></td>
                                                        <td nowrap><asp:Image ID="img13" runat="server" ImageAlign="absMiddle" ImageUrl="/images/spacer.gif" /></td>
                                                        <td width="100%">Open P2P Service Center Request (if in test longer than two hours)</td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap><asp:CheckBox ID="chk14" runat="server" CssClass="default" Enabled="false" /></td>
                                                        <td nowrap><asp:Image ID="img14" runat="server" ImageAlign="absMiddle" ImageUrl="/images/spacer.gif" /></td>
                                                        <td width="100%">Move Servers to Production</td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap><asp:CheckBox ID="chk15" runat="server" CssClass="default" Enabled="false" /></td>
                                                        <td nowrap><asp:Image ID="img15" runat="server" ImageAlign="absMiddle" ImageUrl="/images/spacer.gif" /></td>
                                                        <td width="100%">Release Build/Test Servers: <asp:Button ID="btnRelease" runat="server" CssClass="default" Text="Release" Width="125" OnClick="btnRelease_Click" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3">&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap></td>
                                                        <td nowrap><asp:Image ID="imgSuccess" runat="server" ImageAlign="absMiddle" ImageUrl="/images/spacer.gif" /></td>
                                                        <td width="100%" class="bold">Auto-Provisioning Survey</td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap></td>
                                                        <td nowrap></td>
                                                        <td width="100%">
                                                            <table cellpadding="2" cellspacing="2" border="0">
                                                                <tr>
                                                                    <td>Did the auto-provisioning tool work?</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:DropDownList ID="ddlSuccess" runat="server" CssClass="default">
                                                                            <asp:ListItem Value="-- SELECT --" />
                                                                            <asp:ListItem Value="1" Text="Yes (no problems)" />
                                                                            <asp:ListItem Value="0" Text="Yes (minor problems)" />
                                                                            <asp:ListItem Value="-1" Text="Yes (major problems)" />
                                                                            <asp:ListItem Value="-2" Text="No (could not complete)" />
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td valign="top">If there were problems, please explain:</td>
                                                                </tr>
                                                                <tr>
                                                                    <td><asp:TextBox ID="txtComments" runat="server" CssClass="default" Width="600" Rows="12" TextMode="multiline" /></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <br />
                                                <table width="100%" cellpadding="4" cellspacing="3" border="0">
                                                    <tr>
                                                        <td colspan="2" class="greentableheader">Weekly Status</td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap>Status:</td>
                                                        <td width="100%">
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
                                                        <td colspan="2"><asp:TextBox ID="txtIssues" runat="server" CssClass="default" Width="100%" TextMode="MultiLine" Rows="13" /></td>
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
                                                                            <td nowrap><a href="javascript:void(0);" onclick="ShowStatus('<%# DataBinder.Eval(Container.DataItem, "id") %>','r');"><%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "modified").ToString()).ToShortDateString() %>&nbsp;<%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "modified").ToString()).ToShortTimeString() %></a></td>
                                                                            <td nowrap>&nbsp;</td>
                                                                            <td nowrap valign="top"><asp:Label ID="lblStatus" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "status") %>' /></td>
                                                                            <td nowrap>&nbsp;</td>
                                                                            <td width="100%" valign="top">
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
<asp:Label ID="lblAnswer" runat="server" Visible="false" />
<asp:HiddenField ID="hdnTab" runat="server" />