<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wm_server_decommission_IM.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.wm_server_decommission_IM" %>


<script type="text/javascript">
    function ChangeFrame(oCell, oShow, oHide1, oHide2, oHide3, oHidden, strHidden) {
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
	    oCell.style.borderTop = "3px solid #6A8359"
        oCell.style.borderBottom = "1px solid #FFFFFF"
        oHidden = document.getElementById(oHidden);
        oHidden.value = strHidden;
    }
    
    function setControls()
    {
       
        var opnlDestroy = document.getElementById('<%=pnlDestroy.ClientID %>');
        var opnlRedeploy = document.getElementById('<%=pnlRedeploy.ClientID %>');
        
        var oRadioDestroy = document.getElementById('<%=radDestroy.ClientID %>');
        var oRadioRedeploy = document.getElementById('<%=radRedeploy.ClientID %>');
        
        
        var ochkUnrack = document.getElementById('<%=chkUnrack.ClientID %>');
        var ochkWipeDrives = document.getElementById('<%=chkWipeDrives.ClientID %>');
        var ochkDispose = document.getElementById('<%=chkDispose.ClientID %>');
        
        var ochkVerifyModel = document.getElementById('<%=chkVerifyModel.ClientID %>');
        var ochkMoveServerToDeploy = document.getElementById('<%=chkMoveServerToDeploy.ClientID %>');
        
        if((oRadioDestroy.checked ==false) && (oRadioRedeploy.checked==false))
        {
            
            ochkUnrack.checked=false;
            ochkWipeDrives.checked=false;
            ochkDispose.checked=false;
            ochkVerifyModel.checked=false;
            ochkMoveServerToDeploy.checked=false;

            ochkUnrack.disabled=true;
            ochkWipeDrives.disabled=true;
            ochkDispose.disabled=true;
            ochkVerifyModel.disabled=true;
            ochkMoveServerToDeploy.disabled=true;
        }
        
        if(oRadioDestroy.checked)
        {   //opnlRedeploy.style.display =  "none";
            //opnlDestroy.style.display =  "inline";
           
            //ochkUnrack.checked=false;
            //ochkWipeDrives.checked=false;
            //ochkDispose.checked=false;
            ochkVerifyModel.checked=false;
            ochkMoveServerToDeploy.checked=false;

            ochkUnrack.disabled=false;
            ochkWipeDrives.disabled=false;
            ochkDispose.disabled=false;
            ochkVerifyModel.disabled=true;
            ochkMoveServerToDeploy.disabled=true;
            
        }   
        if (oRadioRedeploy.checked)
        { 
            ochkUnrack.checked=false;
            ochkWipeDrives.checked=false;
            ochkDispose.checked=false;
            //ochkVerifyModel.checked=false;
           //ochkMoveServerToDeploy.checked=false;

            ochkUnrack.disabled=true;
            ochkWipeDrives.disabled=true;
            ochkDispose.disabled=true;
            ochkVerifyModel.disabled=false;
            ochkMoveServerToDeploy.disabled=false;
            
        }      
    }

   
    function validationServerOptions()
    {
        var oRadioDestroy = document.getElementById('<%=radDestroy.ClientID %>');
        var oRadioRedeploy = document.getElementById('<%=radRedeploy.ClientID %>');
        
        var ochkUnrack = document.getElementById('<%=chkUnrack.ClientID %>');
        var ochkWipeDrives = document.getElementById('<%=chkWipeDrives.ClientID %>');
        var ochkDispose = document.getElementById('<%=chkDispose.ClientID %>');
        
        var ochkVerifyModel = document.getElementById('<%=chkVerifyModel.ClientID %>');
        var ochkMoveServerToDeploy = document.getElementById('<%=chkMoveServerToDeploy.ClientID %>');
        
        if (oRadioDestroy.checked ==false && oRadioRedeploy.checked==false)
        {
             alert('Please select the option Destroyed/Redeployed');
             return false;
        }
        if(oRadioDestroy.checked)
        {
            if(ochkUnrack.checked==false ||ochkUnrack.checked==false || ochkUnrack.checked==false)
            {
                alert('Please complete the all the server destroy tasks');
                return false;
             }
        }
        if(oRadioRedeploy.checked)
        {
            if(ochkVerifyModel.checked==false ||ochkMoveServerToDeploy.checked==false)
            {
                alert('Please complete the all the server redeploy tasks');
                return false;
             }
        }
        
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
                        <td><asp:ImageButton ID="btnEmail" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/tool_email.gif" /></td>
                        <td><asp:ImageButton ID="btnSLA" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/tool_sla.gif" /></td>
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
                    <tr>
                        <td colspan="2">
                            <table border="0" cellpadding="3" cellspacing="2">
                                <tr>
                                    <td><b>Original Request Details:</b>&nbsp;&nbsp;<asp:Label ID="lblDescription" runat="server" CssClass="default" /></td>
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
                            <td class="cmbutton" style='<%=boolDetails == true ? "display:inline;border-bottom:1px solid #FFFFFF;border-top:3px solid orange;" : "" %>' onclick="ChangeFrame(this,'divDetails','divExecution','divChange','divDocuments','<%=hdnTab.ClientID %>','D');">Request Details</td>
                            <td class="cmbuttonspace">&nbsp;</td>
                            <td class="cmbutton" style='<%=boolExecution == true ? "display:inline;border-bottom:1px solid #FFFFFF;border-top:3px solid orange;" : "" %>' onclick="ChangeFrame(this,'divExecution','divDetails','divChange','divDocuments','<%=hdnTab.ClientID %>','E');">Execution</td>
                            <td class="cmbuttonspace">&nbsp;</td>
                            <td class="cmbutton" style='<%=boolChange == true ? "display:inline;border-bottom:1px solid #FFFFFF;border-top:3px solid orange;" : "" %>' onclick="ChangeFrame(this,'divChange','divDetails','divExecution','divDocuments','<%=hdnTab.ClientID %>','C');">Change Controls</td>
                            <td class="cmbuttonspace">&nbsp;</td>
                            <td class="cmbutton" style='<%=boolDocuments == true ? "display:inline;border-bottom:1px solid #FFFFFF;border-top:3px solid orange;" : "" %>' onclick="ChangeFrame(this,'divDocuments','divDetails','divExecution','divChange','<%=hdnTab.ClientID %>','D');">Attached Files</td>
                        </tr>
                        <tr>
                            <td colspan="7" align="center" class="cmcontents">
                                <table width="100%" height="100%" border="0" cellSpacing="2" cellPadding="2" class="default">
                                    <tr> 
                                        <td valign="top">
		                                    <div id="divDetails" style='<%=boolDetails == true ? "display:inline" : "display:none" %>'>
		                                        <br />
                                                <table width="100%" cellpadding="4" cellspacing="3" border="0">
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
                                                    <tr>
                                                        <td colspan="2" class="greentableheader">Required Tasks</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <table cellpadding="2" cellspacing="1" border="0">
                                                                <tr>
                                                                    <td colspan="2">What should happen to this server?</td>
                                                                </tr>
                                                                <tr>
                                                                <td><img src="/images/spacer.gif" border="0" width="50" height="1" /></td>
                                                                </tr> 
                                                                <tr>
                                                                <td width="100%" align ="left" >
                                                                    <asp:RadioButton ID="radDestroy" runat="server" CssClass="default" Text="Dispose" GroupName="DECOMMISSION" /><br />
                                                                    <asp:RadioButton ID="radRedeploy" runat="server" CssClass="default" Text="Redeploy" GroupName="DECOMMISSION" /><br />
                                                                 </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2">&nbsp;</td>
                                                                </tr>
                                                                <asp:Panel id="pnlDestroy" runat="server" style="display:inline">
                                                                <tr>
                                                                    <td colspan="2">If Dispose :</td>
                                                                </tr>
                                                                <tr>
                                                                    <td width="100%" align ="left">
                                                                        <table cellpadding="2" cellspacing="2" border="0">
                                                                            <tr>
                                                                                <td><asp:CheckBox ID="chkUnrack" runat="server" CssClass="default" Text="Unrack the server and remove cables" /></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td><asp:CheckBox ID="chkWipeDrives" runat="server" CssClass="default" Text="Wipe drives" /></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td><asp:CheckBox ID="chkDispose" runat="server" CssClass="default" Text="Dispose Server(server marked destroyed)" /></td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                </asp:Panel>
                                                                 <tr>
                                                                    <td colspan="2">&nbsp;</td>
                                                                </tr>
                                                                <asp:Panel id="pnlRedeploy" runat="server" style="display:inline">
                                                                <tr>
                                                                    <td colspan="2">If Redeploy :</td>
                                                                </tr>
                                                                <tr>
                                                                    <td width="100%">
                                                                        <table cellpadding="2" cellspacing="2" border="0">
                                                                            <tr>
                                                                                <td><asp:CheckBox ID="chkVerifyModel" runat="server" CssClass="default" Text="Verify Server model and hardware configuration" /></td>
                                                                                <td>[<asp:HyperLink ID="btnVerifyModel" runat="server" Target="_blank" Text="Click here to view this asset" />]</td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="2"><asp:CheckBox ID="chkMoveServerToDeploy" runat="server" CssClass="default" Text="Move Server to deploy screen" /></td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                </asp:Panel>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" class="greentableheader">Status Updates</td>
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
                                                                </tr>
                                                                <asp:repeater ID="rptStatus" runat="server">
                                                                    <ItemTemplate>
                                                                        <tr class="default">
                                                                            <td nowrap><a href="javascript:void(0);" onclick="ShowStatus('<%# DataBinder.Eval(Container.DataItem, "id") %>','r');"><%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "modified").ToString()).ToShortDateString() %>&nbsp;<%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "modified").ToString()).ToShortTimeString() %></a></td>
                                                                            <td nowrap>&nbsp;</td>
                                                                            <td width="100%"><asp:Label ID="lblStatus" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "status") %>' /></td>
                                                                        </tr>
                                                                    </ItemTemplate>
                                                                </asp:repeater>
                                                            <tr>
                                                                <td colspan="3">
                                                                    <asp:Label ID="lblNoStatus" runat="server" CssClass="default" Visible="false" Text="<img src='/images/alert.gif' border='0' align='absmiddle'> There are no updates" />
                                                                </td>
                                                            </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
		                                    </div>
		                                    <div id="divChange" style='<%=boolChange == true ? "display:inline" : "display:none" %>'>
		                                        <br />
                                                <table width="100%" cellpadding="4" cellspacing="3" border="0">
                                                    <tr>
                                                        <td colspan="2" class="greentableheader">Change Controls</td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap>Number:</td>
                                                        <td width="100%"><asp:TextBox ID="txtNumber" runat="server" CssClass="default" Width="150" MaxLength="15" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap>Date:</td>
                                                        <td width="100%"><asp:TextBox ID="txtDate" runat="server" CssClass="default" Width="100" MaxLength="10" /> <asp:ImageButton ID="imgDate" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap>Time:</td>
                                                        <td width="100%"><asp:TextBox ID="txtTime" runat="server" CssClass="default" Width="100" MaxLength="10" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2"><b>Comments:</b></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2"><asp:TextBox ID="txtChange" runat="server" CssClass="default" Width="500" TextMode="MultiLine" Rows="7" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2"><asp:Button ID="btnChange" runat="server" CssClass="default" Width="150" Text="Submit Change" OnClick="btnChange_Click" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <table width="100%" cellpadding="3" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                                                                <tr bgcolor="#EEEEEE">
                                                                    <td nowrap><b>Number</b></td>
                                                                    <td nowrap><b>Date</b></td>
                                                                    <td nowrap>&nbsp;</td>
                                                                </tr>
                                                                <asp:repeater ID="rptChange" runat="server">
                                                                    <ItemTemplate>
                                                                        <tr class="default">
                                                                            <td nowrap><a href="javascript:void(0);" onclick="ShowChange('<%# DataBinder.Eval(Container.DataItem, "changeid") %>');"><%# DataBinder.Eval(Container.DataItem, "number") %></a></td>
                                                                            <td nowrap><%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "implementation").ToString()).ToLongDateString() %> @ <%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "implementation").ToString()).ToLongTimeString() %></td>
                                                                            <td align="right">[<asp:LinkButton ID="btnDeleteChange" runat="server" Text="Delete" OnClick="btnDeleteChange_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "changeid") %>' />]</td>
                                                                        </tr>
                                                                    </ItemTemplate>
                                                                </asp:repeater>
                                                            <tr>
                                                                <td colspan="3">
                                                                    <asp:Label ID="lblNoChange" runat="server" CssClass="default" Visible="false" Text="<img src='/images/alert.gif' border='0' align='absmiddle'> There are no change controls" />
                                                                </td>
                                                            </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
		                                    </div>
		                                    <div id="divDocuments" style='<%=boolDocuments == true ? "display:inline" : "display:none" %>'>
		                                        <br />
                                                <table width="100%" cellpadding="4" cellspacing="3" border="0">
                                                    <tr>
                                                        <td class="greentableheader">Attached Files</td>
                                                        <td align="right"><asp:CheckBox ID="chkDescription" runat="server" CssClass="default" Text="Show Descriptions" AutoPostBack="true" OnCheckedChanged="chkDescription_Change" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2"><asp:Label ID="lblDocuments" runat="server" CssClass="default" /></td>
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
<asp:HiddenField ID="hdnTab" runat="server" />