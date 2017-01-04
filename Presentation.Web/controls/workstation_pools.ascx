<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="workstation_pools.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.workstation_pools" %>


<script type="text/javascript">
    function ChangeFrame(oCell, oShow) {
        var oTable = document.getElementById("tblDivs");
        var oDIVs = oTable.getElementsByTagName("DIV");
        for(var ii=0;ii<oDIVs.length;ii++){
            oDIVs[ii].style.display = "none"
        }
        oShow = document.getElementById(oShow);
        oShow.style.display = "inline";
        var oTable = document.getElementById("tblTabs");
        var oTDs = oTable.getElementsByTagName("TD");
        for(var ii=0;ii<oTDs.length;ii++){
    		if (oTDs[ii].className == "cmbutton")
                oTDs[ii].style.border = "1px solid #94a6b5"
        }
	    oCell.style.borderTop = "3px solid orange"
        oCell.style.borderBottom = "1px solid #FFFFFF"
    }
    function MoveList(ddlFrom, ddlTo, oHidden, ddlUpdate) {
        ddlFrom = document.getElementById(ddlFrom);
        ddlTo = document.getElementById(ddlTo);
        ddlUpdate = document.getElementById(ddlUpdate);
	    if (ddlFrom.selectedIndex > -1) {
		    var oOption = document.createElement("OPTION");
		    ddlTo.add(oOption);
		    oOption.text = ddlFrom.options[ddlFrom.selectedIndex].text;
		    oOption.value = ddlFrom.options[ddlFrom.selectedIndex].value;
		    ddlFrom.remove(ddlFrom.selectedIndex);
		    ddlTo.selectedIndex = ddlTo.length - 1;
		    UpdateHidden(oHidden, ddlUpdate);
	    }
	    return false;
    }
    function UpdateHidden(oHidden, oControl) {
	    var oHidden = document.getElementById(oHidden);
	    oHidden.value = "";
	    for (var ii=0; ii<oControl.length; ii++) {
    		oHidden.value = oHidden.value + oControl.options[ii].value + "_" + ii + "&";
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
                    <td rowspan="2"><img src="/images/user_add.gif" border="0" align="absmiddle" /></td>
                    <td class="header" width="100%" valign="bottom">Workstation Pools</td>
                </tr>
                <tr>
                    <td width="100%" valign="top">Workstation pools are used to provide multiple people access to the same workstations.</td>
                </tr>
            </table>
            <asp:Panel ID="panPools" runat="server" Visible="false">
                <br />
                <table width="100%" cellpadding="4" cellspacing="0" border="0" align="center" style="border:solid 1px #CCCCCC">
                    <tr bgcolor="#EEEEEE">
                        <td width="20%" nowrap><b>Name</b></td>
                        <td width="50%"><b>Description</b></td>
                        <td width="20%" nowrap><b>Last Modified</b></td>
                        <td width="10%" nowrap align="center"><b>Workstations</b></td>
                        <td nowrap align="center"><b>Enabled</b></td>
                    </tr>
                <asp:repeater ID="rptPools" runat="server">
                    <ItemTemplate>
                        <tr onmouseover="CellRowOver(this);" onmouseout="CellRowOut(this);" onclick="window.navigate('<%# oPage.GetFullLink(intPage) + "?id=" + DataBinder.Eval(Container.DataItem, "id") %>');">
                            <td width="20%" nowrap><%# DataBinder.Eval(Container.DataItem, "name") %></td>
                            <td width="50%"><%# DataBinder.Eval(Container.DataItem, "description") %></td>
                            <td width="20%" nowrap><%# oUser.GetFullName(Int32.Parse(DataBinder.Eval(Container.DataItem, "modifiedby").ToString())) %> @ <%# DataBinder.Eval(Container.DataItem, "modified") %></td>
                            <td width="10%" nowrap align="center"><asp:Label ID="lblWorkstations" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "id") %>' /></td>
                            <td nowrap align="center"><img src="/images/<%# DataBinder.Eval(Container.DataItem, "enabled").ToString() == "1" ? "check" : "cancel" %>.gif" border="0" align="absmiddle" /></td>
                        </tr>
                    </ItemTemplate>
                </asp:repeater>
                    <tr>
                        <td colspan="6"><asp:Label ID="lblPools" runat="server" CssClass="error" Visible="false" Text="<img src='/images/bigAlert.gif' border='0' align='absmiddle'> You do not have any workstation pools configured" /></td>
                    </tr>
                </table>
                <br /><br /><br />
                <table width="100%" cellpadding="4" cellspacing="3" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9" >
                    <tr>
                        <td class="bigred">Create a Virtual Workstation</td>
                    </tr>
                    <tr>
                        <td>Click the <b>Create</b> button to begin the process of creating your virtual workstation.</td>
                    </tr>
                    <tr>
                        <td><asp:Button ID="btnCreate" runat="server" CssClass="default" Width="100" Text="Create" OnClick="btnCreate_Click" /></td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="panPool" runat="server" Visible="false">
                <br />
                <asp:LinkButton ID="lnkBack" runat="server" Text="<img src='/images/back.gif' border='0' align='absmiddle' /><img src='/images/spacer.gif' border='0' width='10' height='1'/>Back to Workstation Pool List" OnClick="btnBack_Click" />
                <br /><br />
                <%=strMenuTab1 %>
                
                <!--      
                 <table cellpadding="0" cellspacing="0" border="0">
                     <tr>
                         <td><img src="/images/TabEmptyBackground.gif" /></td>
                         <td><img src="/images/TabOnLeftCap.gif" /></td>
                         <td nowrap background="/images/TabOnBackground.gif"><a href="javascript:void(0);" onclick="ChangeTab(this,'divTab1',null,null,false);" class="tabheader">Pool Configurtion</a></td>
                         <td><img src="/images/TabOnRightCap.gif" /></td>
                         <td><img src="/images/TabOffLeftCap.gif" /></td>
                         <td nowrap background="/images/TabOffBackground.gif"><a href="javascript:void(0);" onclick="ChangeTab(this,'divTab2',null,null,false);" class="tabheader">Workstation History</a></td>
                         <td><img src="/images/TabOffRightCap.gif" /></td>
                         <td><img src="/images/TabOffLeftCap.gif" /></td>
                         <td nowrap background="/images/TabOffBackground.gif"><a href="javascript:void(0);" onclick="ChangeTab(this,'divTab3',null,null,false);" class="tabheader">Workstations Currently Available</a></td>
                         <td><img src="/images/TabOffRightCap.gif" /></td>
                         <td><img src="/images/TabOffLeftCap.gif" /></td>
                         <td nowrap background="/images/TabOffBackground.gif"><a href="javascript:void(0);" onclick="ChangeTab(this,'divTab4',null,null,false);" class="tabheader">Subscribed Users</a></td>
                         <td><img src="/images/TabOffRightCap.gif" /></td>
                         <td width="100%" background="/images/TabEmptyBackground.gif">&nbsp;</td>
                     </tr>
                 </table> -->
                <div id="divMenu1">
                <div style="display:inline">
                    <br />
                    <table width="100%" cellpadding="4" cellspacing="3" border="0">
                        <asp:Panel ID="panSave" runat="server" Visible="false">
                        <tr>
                            <td colspan="2" class="bigcheck" align="center"><img src="/images/ico_check.gif" border="0" align="absmiddle" /> Save Successful</td>
                        </tr>
                        </asp:Panel>
                        <tr>
                            <td nowrap>Name:</td>
                            <td width="100%"><asp:TextBox ID="txtName" runat="server" CssClass="default" Width="300" MaxLength="50" ReadOnly="true" />&nbsp;&nbsp;&nbsp;(Please contact a ClearView administrator to modify the name of the pool)</td>
                        </tr>
                        <tr>
                            <td nowrap>Description:</td>
                            <td width="100%"><asp:TextBox ID="txtDescription" runat="server" CssClass="default" Width="500" TextMode="MultiLine" Rows="6" /></td>
                        </tr>
                        <tr>
                            <td nowrap>Primary Contact:</td>
                            <td width="100%">
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <tr>
                                        <td><asp:TextBox ID="txtContact1" runat="server" Width="250" CssClass="default" /></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div id="divContact1" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                                                <asp:ListBox ID="lstContact1" runat="server" CssClass="default" />
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td nowrap>Secondary Contact:</td>
                            <td width="100%">
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <tr>
                                        <td><asp:TextBox ID="txtContact2" runat="server" Width="250" CssClass="default" /></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div id="divContact2" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                                                <asp:ListBox ID="lstContact2" runat="server" CssClass="default" />
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td nowrap>Enabled:</td>
                            <td width="100%"><asp:CheckBox ID="chkEnabled" runat="server" CssClass="default" /></td>
                        </tr>
                        <tr>
                            <td colspan="2">&nbsp;</td>
                        </tr>
                        <tr>
                            <td nowrap>&nbsp;</td>
                            <td width="100%" class="bold">Workstations in this Pool</td>
                        </tr>
                        <tr>
                            <td nowrap>&nbsp;</td>
                            <td width="100%">
                                <table cellpadding="2" cellspacing="0" border="0">
                                    <tr>
                                        <td class="default">Selected:</td>
                                        <td class="default" colspan="3">&nbsp;</td>
                                        <td class="default">Available:</td>
                                    </tr>
                                    <tr>
                                        <td><asp:ListBox ID="lstCurrent" runat="server" Width="300" CssClass="default" Rows="20" /></td>
                                        <td>&nbsp;</td>
                                        <td>
                                            <asp:ImageButton ID="btnAdd" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/lt.gif" ToolTip="Add" /><br /><br />
                                            <asp:ImageButton ID="btnRemove" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/rt.gif" ToolTip="Remove" />
                                        </td>
                                        <td>&nbsp;</td>
                                        <td><asp:ListBox ID="lstAvailable" runat="server" Width="300" CssClass="default" Rows="20" /></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td nowrap>&nbsp;</td>
                            <td width="100%"><asp:Button ID="btnUpdate" runat="server" CssClass="default" Width="100" Text="Save" OnClick="btnUpdate_Click" /> <asp:Button ID="btnBack" runat="server" CssClass="default" Width="100" Text="Cancel" OnClick="btnBack_Click" /></td>
                        </tr>
                    </table>
                </div>
                <div style="display:none">
                    <br />
                    <table width="100%" cellpadding="4" cellspacing="0" border="0" align="center" style="border:solid 1px #CCCCCC">
                        <tr bgcolor="#EEEEEE">
                            <td width="30%" nowrap><b>Workstation</b></td>
                            <td width="30%" nowrap><b>Description</b></td>
                            <td width="20%" nowrap><b>Checked Out</b></td>
                            <td width="20%" nowrap><b>Checked In</b></td>
                        </tr>
                    <asp:repeater ID="rptHistory" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td width="30%" nowrap><%# DataBinder.Eval(Container.DataItem, "name") %></td>
                                <td width="30%" nowrap><%# DataBinder.Eval(Container.DataItem, "username") %> (<%# DataBinder.Eval(Container.DataItem, "xid") %>)</td>
                                <td width="20%" nowrap><%# DataBinder.Eval(Container.DataItem, "checkedout") %></td>
                                <td width="20%" nowrap><%# DataBinder.Eval(Container.DataItem, "checkedin") %></td>
                            </tr>
                        </ItemTemplate>
                    </asp:repeater>
                        <tr>
                            <td colspan="4"><asp:Label ID="lblHistory" runat="server" CssClass="error" Visible="false" Text="<img src='/images/bigAlert.gif' border='0' align='absmiddle'> There is no history" /></td>
                        </tr>
                    </table>
                </div>
                <div  style="display:none">
                    <br />
                    <table width="100%" cellpadding="4" cellspacing="0" border="0" align="center" style="border:solid 1px #CCCCCC">
                        <tr bgcolor="#EEEEEE">
                            <td nowrap><b>Workstation</b></td>
                        </tr>
                    <asp:repeater ID="rptAvailable" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td nowrap><%# DataBinder.Eval(Container.DataItem, "name") %></td>
                            </tr>
                        </ItemTemplate>
                    </asp:repeater>
                        <tr>
                            <td><asp:Label ID="lblAvailable" runat="server" CssClass="error" Visible="false" Text="<img src='/images/bigAlert.gif' border='0' align='absmiddle'> There are no workstations available" /></td>
                        </tr>
                    </table>
                </div>
                <div  style="display:none">
                    <br />
                    <table width="100%" cellpadding="4" cellspacing="0" border="0" align="center" style="border:solid 1px #CCCCCC">
                        <tr bgcolor="#EEEEEE">
                            <td nowrap><b>User</b></td>
                        </tr>
                        <%=strSubscribers %>
                    </table>
                    <br />
                    <br />
                    <img src="/images/bigInfo.gif" border="0" align="absmiddle" /> <b>NOTE:</b> To add a user to this pool, please complete and submit a LAN access form. [<a href="#">Click here to download</a>]
                </div>
                </div>
            </asp:Panel>
            <asp:Panel ID="panCreate" runat="server" Visible="false">
                <table width="100%" border="0" cellSpacing="2" cellPadding="4" class="default">
                    <tr>
                        <td colspan="2">
                            <table width="100%" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
                                <tr>
                                    <td nowrap><img src="/images/bigAlert.gif" border="0" align="absmiddle" /></td>
                                    <td width="100%"><b>NOTE:</b> You cannot attach a physical device to a virtual workstation. If your initiative requires special hardware, do not choose a virtual workstation.</td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td nowrap>Location:<font class="required">&nbsp;*</font></td>
                        <td width="100%"><asp:TextBox ID="txtParent" CssClass="lightdefault" runat="server" Text="" Width="400" ReadOnly="true" /></td>
                    </tr>
                    <tr>
                        <td nowrap>Quantity:<font class="required">&nbsp;*</font></td>
                        <td width="100%"><asp:TextBox ID="txtQuantity" runat="server" CssClass="default" Width="80" MaxLength="10" /> <span class="footer">NOTE: You can request up to five (5) workstations per day</span></td>
                    </tr>
                    <tr>
                        <td nowrap>RAM:<font class="required">&nbsp;*</font></td>
                        <td width="100%"><asp:DropDownList ID="ddlRam" runat="server" CssClass="default" Width="300" /> GB</td>
                    </tr>
                    <tr>
                        <td nowrap>Operating System:<font class="required">&nbsp;*</font></td>
                        <td width="100%"><asp:DropDownList ID="ddlOS" runat="server" CssClass="default" Width="300" /></td>
                    </tr>
                    <tr>
                        <td nowrap>CPUs:<font class="required">&nbsp;*</font></td>
                        <td width="100%"><asp:DropDownList ID="ddlCPU" runat="server" CssClass="default" Width="300" /></td>
                    </tr>
                    <tr>
                        <td nowrap>Hard Drive:<font class="required">&nbsp;*</font></td>
                        <td width="100%"><asp:DropDownList ID="ddlHardDrive" runat="server" CssClass="default" Width="300" /> GB</td>
                    </tr>
                    <tr>
                        <td nowrap>&nbsp;</td>
                        <td width="100%"><asp:Button ID="btnContinue" runat="server" CssClass="default" Width="100" Text="Continue" OnClick="btnContinue_Click" /> <asp:Button ID="btnCancel" runat="server" CssClass="default" Width="100" Text="Cancel" OnClick="btnBack_Click" /></td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="panExecute" runat="server" Visible="false">
                <br />
                <table width="100%" cellpadding="4" cellspacing="3" border="0">
                    <tr>
                        <td class="biggerbold"><img src="/images/bigCheck.gif" border="0" align="absmiddle" /> Ready to Execute!</td>
                    </tr>
                    <tr>
                        <td>Your request for a virtual workstation has been submitted successfully and you are ready to execute your build.  Click <b>Execute</b> to finalize your configuration and start your build.</td>
                    </tr>
                    <tr>
                        <td><asp:Button ID="btnExecute" runat="server" CssClass="default" Width="100" Text="Execute" /></td>
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
<input type="hidden" id="hdnParent" runat="server" />
<asp:HiddenField ID="hdnContact1" runat="server" />
<asp:HiddenField ID="hdnContact2" runat="server" />
<asp:HiddenField ID="hdnWorkstations" runat="server" />
