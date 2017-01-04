<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wm_shared_env_add_storage.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.wm_shared_env_add_storage" %>
<%--<%@ Register Src="~/controls/wm/wm_Status_Updates.ascx"
TagName="wucWMStatusUpdates" TagPrefix="ucWMStatusUpdates" %>--%>

<script type="text/javascript">
     function ChangeFrame(oCell, oShow, oHide1, oHide2, oHide3,oHide4, oHidden, strHidden) 
      {
        oShow = document.getElementById(oShow);
        oShow.style.display = "inline";
        oHide1 = document.getElementById(oHide1);
        oHide1.style.display = "none";
        oHide2 = document.getElementById(oHide2);
        oHide2.style.display = "none";
        oHide3 = document.getElementById(oHide3);
        oHide3.style.display = "none";
        oHide4 = document.getElementById(oHide4);
        oHide4.style.display = "none";
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
    
    function setControlsForReturnedRequest()
    {
       
        var obtnSave = document.getElementById('<%=btnSave.ClientID %>');
        if (obtnSave != null)
            obtnSave.disabled=true;
        var obtnPrint = document.getElementById('<%=btnPrint.ClientID %>');
        if (obtnPrint != null)
            obtnPrint.disabled=true;
        var obtnEmail = document.getElementById('<%=btnEmail.ClientID %>');
        if (obtnEmail != null)
            obtnEmail.disabled=true;
        var obtnComplete = document.getElementById('<%= btnComplete.ClientID %>');
        if (obtnComplete != null)
            obtnComplete.disabled=true;
        var obtnReturn = document.getElementById('<%=btnReturn.ClientID %>');
        if (obtnReturn != null)
            obtnReturn.disabled=true;
            
        RefreshOpeningWindow();
        window.close();
    }
    
    function SaveAndRefreshWindow()
    {
     var obtnSave = document.getElementById('<%=btnSave.ClientID %>');
     if (obtnSave != null)
        obtnSave.click();
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
                        <td><asp:ImageButton ID="btnReturn" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/tool_return.gif" OnClick="btnReturn_Click" /></td>
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
                <asp:Panel ID="pnlReqReturn" runat="server" CssClass="default" Visible="false">
                    <table width="100%" cellSpacing="2" cellPadding="2" class="default" style="border:solid 1px #999999" bgcolor="#f9f9f9">
                        <tr>
                            <td colspan="2" style="width:20%"><asp:Label ID="lblReqReturn" runat="server" CssClass="redheader" Text="You service request is returned for below mentioned reason.  Please correct the request and submit again " /></td>
                           
                        </tr>
                        <tr>
                            <td style="width:20%"><asp:Label ID="lblReqReturnedBy" runat="server" CssClass="bold" Text="Request Returned By : " /></td>
                            <td style="width:78%"><asp:Label ID="lblReqReturnedByValue" runat="server" CssClass="bold" Text="User" />
                            <asp:Label ID="lblReqReturnedId" runat="server" CssClass="bold" Text="0" Visible="false" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width:20%"><asp:Label ID="lblReqReturnComment" runat="server" CssClass="bold" Text="Comments : " /></td>
                            <td style="width:78%"><asp:Label ID="lblReqReturnCommentValue" runat="server" CssClass="bold" Text="" /></td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td class="default">
                <table id="tblRequestDetails" width="100%" border="0" cellSpacing="2" cellPadding="2" class="default">
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
                        <td class="cmbutton" style='<%=boolDetails == true ? "display:inline;border-bottom:1px solid #FFFFFF;border-top:3px solid orange;" : "" %>' onclick="ChangeFrame(this,'divDetails','divExecution','divStatusUpdates','divChange','divDocuments','<%=hdnTab.ClientID %>','D');">Request Details</td>
                        <td class="cmbuttonspace">&nbsp;</td>
                        <td class="cmbutton" style='<%=boolExecution == true ? "display:inline;border-bottom:1px solid #FFFFFF;border-top:3px solid orange;" : "" %>' onclick="ChangeFrame(this,'divExecution','divDetails','divStatusUpdates','divChange','divDocuments','<%=hdnTab.ClientID %>','E');">Execution</td>
                        <td class="cmbuttonspace">&nbsp;</td>
                        <td class="cmbutton" style='<%=boolStatusUpdates == true ? "display:inline;border-bottom:1px solid #FFFFFF;border-top:3px solid orange;" : "" %>' onclick="ChangeFrame(this,'divStatusUpdates','divDetails','divExecution','divChange','divDocuments','<%=hdnTab.ClientID %>','S');">Status Updates</td>
                        <td class="cmbuttonspace">&nbsp;</td>
                        <td class="cmbutton" style='<%=boolChange == true ? "display:inline;border-bottom:1px solid #FFFFFF;border-top:3px solid orange;" : "" %>' onclick="ChangeFrame(this,'divChange','divDetails','divExecution','divStatusUpdates','divDocuments','<%=hdnTab.ClientID %>','C');">Change Controls</td>
                        <td class="cmbuttonspace">&nbsp;</td>
                        <td class="cmbutton" style='<%=boolDocuments == true ? "display:inline;border-bottom:1px solid #FFFFFF;border-top:3px solid orange;" : "" %>' onclick="ChangeFrame(this,'divDocuments','divDetails','divExecution','divStatusUpdates','divChange','<%=hdnTab.ClientID %>','D');">Attached Files</td>
                        
                    </tr>
                        <tr>
                            <td colspan="9" align="center" class="cmcontents">
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
	                                          <asp:Panel ID="pnlExecution" runat="server" width="100%" Visible="true">
                                                <asp:HiddenField ID="hdnOrderId" runat="server" />
                                                <table width="100%" cellpadding="2" cellspacing="2" border="0">
                                                    <tr valign="top">
                                                        <td>
                                                                 <asp:Panel ID="pnlWMServiceTasks" runat="server" Visible="true"   width="100%"  ScrollBars="Auto" >
                                                                    <asp:DataList ID="dlWMServiceTaskList" runat="server" CellPadding="3" CellSpacing="1" Width="100%" OnItemDataBound="dlWMServiceTaskList_ItemDataBound" OnItemCommand="dlWMServiceTaskList_Command">
                                                                        <HeaderTemplate>
                                                                            <tr bgcolor="#EEEEEE">
                                                                                <td align="left" >
                                                                                    <asp:Label ID="lblWMServiceTaskListHeaderSelect" runat="server" CssClass="default" Text="<b></b>"  />
                                                                                </td>
                                                                                <td align="left" width="100%">
                                                                                    <asp:Label ID="lblWMServiceTaskListHeaderTask" runat="server" CssClass="default" Text="<b>Tasks</b>"  />
                                                                                </td>
                                                                            </tr>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <tr valign="top" style="border-bottom:1px solid #CECECE">
                                                                                <td align="left" style="border-bottom:1px solid #CECECE" >
                                                                                     <asp:CheckBox ID="chkSelectTask" runat="server" />
                                                                                     <asp:HiddenField ID="hdnTaskStatusId" runat="server" />
                                                                                      <asp:HiddenField ID="hdnTaskId" runat="server" />
                                                                                </td>
                                                                                <td align="left" style="border-bottom:1px solid #CECECE">
                                                                                    <asp:Label ID="lblWMServiceTaskListTask" runat="server" CssClass="default" Width="300" /> &nbsp;&nbsp;&nbsp;&nbsp;
                                                                                    <asp:Label ID="lblWMServiceTaskListRequestAdd" runat="server" CssClass="default" Text="Associated Request# :" />
                                                                                    <asp:TextBox ID="txtRequestNo" runat="server" CssClass="default" MaxLength="50" Width="75"  ToolTip="Examples: CVT12345" />
                                                                                    <asp:Button ID="btnWMServiceTaskReqAdd" runat="server" CssClass="default" Text="Add"  CommandName="ADDREQUEST" />
                                                                                    <br />
                                                                                        <asp:DataList ID="dlWMServiceTaskReqList" runat="server" CellPadding="3" CellSpacing="1" Width="100%" OnItemDataBound="dlWMServiceTaskReqList_ItemDataBound" OnItemCommand="dlWMServiceTaskReqList_Command" >
                                                                                            <HeaderTemplate>
                                                                                                <tr bgcolor="#EEEEEE" valign="top" >
                                                                                                    <td align="left" >
                                                                                                        
                                                                                                        <asp:Label ID="lblWMServiceTaskReqListHeaderReq" runat="server" CssClass="smalldefault" Text="<b>Request#</b>"  />
                                                                                                    </td>
                                                                                                    <td align="left" width="30%">
                                                                                                        <asp:Label ID="lblWMServiceTaskReqListHeaderReqName" runat="server" CssClass="smalldefault" Text="<b>Request Name</b>"  />
                                                                                                    </td>
                                                                                                     <td align="left" width="20%">
                                                                                                        <asp:Label ID="lblWMServiceTaskReqListHeaderResource" runat="server" CssClass="smalldefault" Text="<b>Resource</b>"  />
                                                                                                    </td>
                                                                                                    <td align="left" width="20%">
                                                                                                        <asp:Label ID="lblWMServiceTaskReqListHeaderStatus" runat="server" CssClass="smalldefault" Text="<b>Status</b>"  />
                                                                                                    </td>
                                                                                                    <td align="left" width="20%">
                                                                                                        <asp:Label ID="lblWMServiceTaskReqListHeaderLastUpdated" runat="server" CssClass="smalldefault" Text="<b>Last Updated</b>"  />
                                                                                                    </td>
                                                                                                     <td align="left" nowrap>
                                                                                                        
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </HeaderTemplate>
                                                                                            <ItemTemplate>
                                                                                                <tr bordercolor ="red">
                                                                                                    <td align="left" nowrap bordercolor ="red">
                                                                                                        <asp:HiddenField ID="hdnTaskReqStatusId" runat="server" />
                                                                                                        <asp:HiddenField ID="hdnTaskRequestId" runat="server" />
                                                                                                        <asp:LinkButton ID="lnkbtnAssetBnDTaskReqNo" runat="server"  CssClass="lookup"/>
                                                                                                        
                                                                                                    </td>
                                                                                                    <td align="left" >
                                                                                                        <asp:Label ID="lblWMServiceTaskReqListReqName" runat="server" CssClass="smalldefault" />
                                                                                                    </td>
                                                                                                    <td align="left" >
                                                                                                        <asp:Label ID="lblWMServiceTaskReqListResource" runat="server" CssClass="smalldefault" />
                                                                                                    </td>
                                                                                                    <td align="left" >
                                                                                                        <asp:Label ID="lblWMServiceTaskReqListReqStatus" runat="server" CssClass="smalldefault" />
                                                                                                    </td>
                                                                                                    <td align="left" >
                                                                                                        <asp:Label ID="lblWMServiceTaskReqListLastUpdated" runat="server" CssClass="smalldefault" />
                                                                                                    </td>
                                                                                                    <td align="left" >
                                                                                                        <asp:LinkButton ID="lnkbtnWMServiceTaskReqListRemove" runat="server"  Text="Remove" ToolTip="Remove Request"  CssClass="lookup" CommandName="REMOVEREQUEST"/>
                                                                                                    </td>
                                                                                                    
                                                                                                </tr>
                                                                                            </ItemTemplate>
                                                                                        </asp:DataList>
                                                                                </td>
                                                                            </tr>
                                                                        </ItemTemplate>
                                                                    </asp:DataList>
                                                                </asp:Panel>
                                                        </td>
                                                    </tr>
                                                </table>
                                               </asp:Panel>
                                               <br />
                                               <asp:Panel ID="pnlDataStore" runat="server" Visible="true"  Width="100%"  ScrollBars="Auto" >
		                                            <fieldset>
		                                                <legend class="tableheader"><b>DataStore</b></legend>
		                                                <table width="100%" cellpadding="4" cellspacing="3" border="0">
                                                            <tr> 
                                                                <td class="default" width="100px">Name:</td>
                                                                <td><asp:textbox ID="txtDataStoreName" CssClass="default" runat="server" Width="200" MaxLength="50"/>&nbsp;&nbsp;&nbsp;<span class="reddefault"><b>NOTE:</b> This field IS CASE SENSITIVE.</span></td>
                                                            </tr>
                                                            <tr id="trDataStoreStroageType" runat ="server" visible="false"> 
                                                                <td class="default" width="100px">Storage Type:</td>
                                                                <td>
                                                                    <asp:DropDownList ID="ddlDataStoreSelectionStorageType" CssClass="default" runat="server">
                                                                        <asp:ListItem Value="0" Text="--Select--" />
                                                                        <asp:ListItem Value="1" Text="Low Performance" />
                                                                        <asp:ListItem Value="10" Text="Standard Performance" />
                                                                        <asp:ListItem Value="100" Text="High Performance" />
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr id="trDataStoreStroageReplicated" runat ="server" visible="false"> 
                                                                <td class="default">Replicated:</td>
                                                                <td><asp:CheckBox ID="chkDataStoreReplicated" runat="server" Checked="false" /></td>
                                                            </tr>
                                                            <tr id="trDataStoreStroageEnabled" runat ="server" visible="false"> 
                                                                <td class="default">Enabled:</td>
                                                                <td><asp:CheckBox ID="chkDataStoreEnabled" runat="server" Checked="true" /></td>
                                                            </tr>
                                                            <tr> 
                                                                <td class="default"></td>
                                                                <td><asp:button ID="btnAddDataStoreSelection" CssClass="default" runat="server" Text="Add" Width="75" OnClick="btnAddDataStoreSelection_Click" /></td>
                                                            </tr>
                                                            <tr>
                                                               <td colspan="4" width="100%"> 
                                                                  <asp:Panel ID="pnlDataStoreList" runat="server" Visible="true"  Width="100%" height="100%" ScrollBars="Auto" >
                                                                            <asp:DataList ID="dlDataStoreSelection" runat="server" CellPadding="3" CellSpacing="1" Width="100%" OnItemDataBound="dlDataStoreSelection_ItemDataBound" OnItemCommand="dlDataStoreSelection_Command"  >
                                                                                <HeaderTemplate>
                                                                                    <tr bgcolor="#EEEEEE" align="left">
                                                                                        <td align="left" width="80%">
                                                                                            <asp:Label ID="lblDataStoreHeaderName" runat="server" CssClass="default" Text="<b>DataStore</b>"  />
                                                                                        </td>
                                                                                        <td align="left" style="display:none" width="10%">
                                                                                            <asp:Label ID="lblDataStoreHeaderStorageType" runat="server" CssClass="default" Text="<b>Storage Type</b>"  />
                                                                                        </td>
                                                                                        <td align="left" style="display:none" width="10%">
                                                                                            <asp:Label ID="lblDataStoreHeaderReplicated" runat="server" CssClass="default" Text="<b>Replicated</b>"  />
                                                                                        </td>
                                                                                        <td align="left" width="0%">
                                                                                        </td>
                                                                                      </tr>
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <tr align="left">
                                                                                        <td >
                                                                                            <asp:Label ID="lblDataStoreName" runat="server" CssClass="default" />
                                                                                        </td>
                                                                                         <td style="display:none">
                                                                                            <asp:Label ID="lblDataStoreStorageType" runat="server" CssClass="default" />
                                                                                        </td>
                                                                                        <td style="display:none">
                                                                                            <asp:Label ID="lblDataStoreReplicated" runat="server" CssClass="default" />
                                                                                        </td>
                                                                                        <td align="left" nowrap>
                                                                                            [<asp:LinkButton ID="lnkbtnDataStoreDelete" runat="server" Text="Delete" />]
                                                                                        </td>
                                                                                    </tr>
                                                                                </ItemTemplate>
                                                                            </asp:DataList>
                                                                  </asp:Panel>
                                                               </td>
                                                            </tr>
                                                            <tr>
                                                               <td ><asp:Label ID="lblDataStoreNoItems" runat="server" CssClass="default" Visible="false" Text="<img src='/images/spacer.gif' border='0' height='1' width='25' /><img src='/images/alert.gif' border='0' align='absmiddle'> There are no items" /></td>
                                                            </tr>
                                                         </table>
                                                     </fieldset>
                                                    </asp:Panel>
	                                           <br />
		                                    </div>
		                                    <div id="divStatusUpdates" style='<%=boolStatusUpdates == true ? "display:inline" : "display:none" %>'>
		                                        <br />
                                                <table width="100%" cellpadding="4" cellspacing="3" border="0">
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
                                                        <td colspan="2"><asp:Button ID="btnStatus" runat="server" CssClass="default" Width="150" Text="Submit Status" OnClick="btnStatus_Click" /></td>
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
	                                     <%--   <div id="divStatusUpdates" style='<%=boolStatusUpdates == true ? "display:inline" : "display:none" %>'>
	                                           <br />
	                                                <ucWMStatusUpdates:wucWMStatusUpdates ID="ucWMStatusUpdates" runat="server" />
                                              
	                                        </div>--%>
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
