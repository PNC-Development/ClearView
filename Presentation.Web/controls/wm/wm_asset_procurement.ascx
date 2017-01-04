<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wm_asset_procurement.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.wm_asset_procurement" %>


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


    function ValidatePurchaseOrderStatus()
    {
        var oddlPurchaseOrderStatus = document.getElementById('<%=ddlPurchaseOrderStatus.ClientID %>');
        var oddlVendorOrderStatus = document.getElementById('<%=ddlVendorOrderStatus.ClientID %>');
        
        if (oddlPurchaseOrderStatus != null) 
        {   
            if (ValidateText('<%=txtPurchaseOrderNumber.ClientID %>','Please enter a valid purchase order number') == false)
                return false;
            if (oddlPurchaseOrderStatus.options[oddlPurchaseOrderStatus.selectedIndex].value == "3" || 
                oddlPurchaseOrderStatus.options[oddlPurchaseOrderStatus.selectedIndex].value == "4")
            {
                // Rejected or Skipped
                if (document.getElementById('<%=txtPurchaseOrderComments.ClientID %>').value =="")
                    return ValidateText('<%=txtPurchaseOrderComments.ClientID %>', 'Please enter a comments');
            }
            
            if (oddlPurchaseOrderStatus.options[oddlPurchaseOrderStatus.selectedIndex].value == "2" )
            {
                // Approved
                if (ValidateDate('<%=txtPurchaseOrderDate.ClientID %>','Please enter a valid purchase order date') == false)
                    return false;
                 var txtApprovedQuantity = document.getElementById('<%=txtApprovedQuantity.ClientID %>');
                 var txtQuantity = document.getElementById('<%=txtQuantity.ClientID %>');
                if (ValidateNumber0(txtApprovedQuantity,'Please enter a valid quantity') == true) {
                    if (parseInt(txtApprovedQuantity.value) > parseInt(txtQuantity.value)) {
                        alert('The approved quantity must be less than the procure quantity');
                        txtApprovedQuantity.focus();
                        return false;
                    }
                }
                if (ValidateDate('<%=txtApprovedOn.ClientID %>','Please enter a valid approval date') == false)
                    return false;
                    
                if (ValidateText('<%=txtQuoteNumber.ClientID %>','Please enter a valid quote number') == false)
                    return false;
                if (ValidateDate('<%=txtQuoteDate.ClientID %>','Please enter a valid quote date') == false)
                    return false;
                if (ValidateNumber0('<%=txtSystemPrice.ClientID %>','Please enter a valid system price') == false)
                    return false;
                if (ValidateNumber0('<%=txtPurchaseOrderPrice.ClientID %>','Please enter a valid group total price') == false)
                    return false;
                
                if (ValidateDropDown('<%=ddlVendorOrderStatus.ClientID %>','Please select an order status') == false)
                    return false;
                if (ValidateText('<%=txtVendorTrackingNumber.ClientID %>','Please enter a valid tracking number') == false)
                    return false;
                if (ValidateDate('<%=txtVendorOrderDate.ClientID %>','Please enter a valid expected arrival date') == false)
                    return false;
                if (ValidateHidden0('<%=hdnAttentionTo.ClientID %>','<%=txtAttentionTo.ClientID %>','Please enter the LAN ID for the Attention To field') == false)
                    return false;
             }
             if (oddlVendorOrderStatus != null) 
            { 
                if (oddlVendorOrderStatus.options[oddlVendorOrderStatus.selectedIndex].value == "1" )
                {
                     if (document.getElementById('<%=txtVendorOrderDate.ClientID %>').value=="")
                        return ValidateDate('<%=txtVendorOrderDate.ClientID %>','Please enter valid vendor order date');

                     if (document.getElementById('<%=txtVendorTrackingNumber.ClientID %>').value=="")
                        return ValidateText('<%=txtVendorTrackingNumber.ClientID %>','Please enter vendor tracking number');

                        
                     
                 }
            }
             return true;
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
		                                            <asp:Panel ID="pnlOrderReqComments" runat="server" Visible="true"  Width="100%"  ScrollBars="Auto" >
		                                            <fieldset>
		                                                <legend class="tableheader"><b>Comments</b></legend>
		                                                <table width="100%" cellpadding="4" cellspacing="3" border="0">
                                                            <tr valign="top" align="left">
                                                                <td width="100%">
                                                                    <asp:TextBox ID="txtOrderReqComments" runat="server" CssClass="default" Width="98%" TextMode="MultiLine" Rows="2"  />
                                                                </td>
                                                            </tr> 
                                                            <tr valign="top" align="left">
                                                                <td width="100%">
                                                                    <asp:Button ID="btnAddOrderReqComments" runat="server" CssClass="default" Text="Add Comments" OnClick ="btnAddOrderReqComments_Click" Visible="true"/>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                               <td  width="100%"> 
                                                                  <asp:Panel ID="pnlOrderReqCommentsList" runat="server" Visible="true"  Width="100%" height="100%" ScrollBars="Auto" >
                                                                            <asp:DataList ID="dlOrderReqComments" runat="server" CellPadding="3" CellSpacing="1" Width="100%" OnItemDataBound="dlOrderReqComments_ItemDataBound" >
                                                                                <HeaderTemplate>
                                                                                    <tr bgcolor="#EEEEEE">
                                                                                        <td align="left" width="80%">
                                                                                            <asp:Label ID="lblOrderReqHeaderComments" runat="server" CssClass="default" Text="<b>Comments</b>"  />
                                                                                        </td>
                                                                                        <td align="left" width="10%">
                                                                                            <asp:Label ID="lblOrderReqHeaderUpdatedBy" runat="server" CssClass="default" Text="<b>Updated By</b>"  />
                                                                                        </td>
                                                                                        <td align="left" width="90%">
                                                                                            <asp:Label ID="lblOrderReqHeaderLastUpdated" runat="server" CssClass="default" Text="<b>Last Updated </b>"  />
                                                                                        </td>
                                                                                        <td align="left" width="0%">
                                                                                        </td>
                                                                                      </tr>
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <tr>
                                                                                        <td align="left" >
                                                                                            <asp:Label ID="lblOrderReqComments" runat="server" CssClass="default" />
                                                                                        </td>
                                                                                         <td align="left"  nowrap>
                                                                                            <asp:Label ID="lblOrderReqUpdatedBy" runat="server" CssClass="default" />
                                                                                        </td>
                                                                                        <td align="left"  nowrap>
                                                                                            <asp:Label ID="lblOrderReqLastUpdated" runat="server" CssClass="default" />
                                                                                        </td>
                                                                                        <td align="left" >
                                                                                            [<asp:LinkButton ID="lnkbtnOrderReqDelete" runat="server" OnClick="lnkbtnOrderReqDelete_Click" Text="Delete" />]
                                                                                        </td>
                                                                                    </tr>
                                                                                </ItemTemplate>
                                                                                <AlternatingItemTemplate>
                                                                                    <tr bgcolor="#F6F6F6">
                                                                                        <td align="left" >
                                                                                            <asp:Label ID="lblOrderReqComments" runat="server" CssClass="default" />
                                                                                        </td>
                                                                                         <td align="left"  nowrap>
                                                                                            <asp:Label ID="lblOrderReqUpdatedBy" runat="server" CssClass="default" />
                                                                                        </td>
                                                                                        <td align="left"  nowrap>
                                                                                            <asp:Label ID="lblOrderReqLastUpdated" runat="server" CssClass="default" />
                                                                                        </td>
                                                                                        <td align="left" >
                                                                                            [<asp:LinkButton ID="lnkbtnOrderReqDelete" runat="server" OnClick="lnkbtnOrderReqDelete_Click" Text="Delete" />]
                                                                                        </td>
                                                                                    </tr>
                                                                                </AlternatingItemTemplate>
                                                                            </asp:DataList>
                                                                  </asp:Panel>
                                                               </td>
                                                            </tr>
                                                            <tr>
                                                               <td ><asp:Label ID="lblOrderReqCommentsNoComments" runat="server" CssClass="default" Visible="false" Text="<img src='/images/spacer.gif' border='0' height='1' width='25' /><img src='/images/alert.gif' border='0' align='absmiddle'> There are no items" /></td>
                                                            </tr>
                                                         </table>
                                                     </fieldset>
                                                    </asp:Panel>
                                                    <br />
		                                             <asp:Panel ID="pnlExecution" runat="server" Visible="true">
		                                               <table width="100%" cellpadding="4" cellspacing="3" border="0">
                                                            <tr valign="top">
                                                                <td colspan="2" class="bold">Project Information:</td>
                                                            </tr>
                                                            <tr valign="top">
                                                                <td ><asp:Label ID="lblProjectNumber" runat="server" CssClass="default" Text="Project Number :" /></td>
                                                                <td >
                                                                    <asp:TextBox ID="txtProjectNumber" runat="server" CssClass="default" Width="150" /> 
                                                                    <asp:Button ID="btnProject" runat="server" CssClass="default" Width="150" Text="Load Project Information" OnClick="btnProject_Click" /> 
                                                                    <asp:HiddenField ID="hdnProjectId" runat="server" />
                                                                </td>
                                                            </tr>
                                                            <tr valign="top">
                                                                <td ><asp:Label ID="lblProjectName" runat="server" CssClass="default" Text="Project Name :" /></td>
                                                                <td >
                                                                    <asp:TextBox ID="txtProjectName" runat="server" CssClass="default" Width="400" Enabled="false" />
                                                                </td>
                                                            </tr>
                                                            <tr valign="top">
                                                                <td ><asp:Label ID="lblProjectManager" runat="server" CssClass="default" Text="Project Manager :" /></td>
                                                                <td >
                                                                    <table cellpadding="0" cellspacing="0" border="0">
                                                                        <tr>
                                                                            <td><asp:TextBox ID="txtProjectManager" runat="server" Width="250" CssClass="default" /></td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <div id="divProjectManager" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                                                                                    <asp:ListBox ID="lstProjectManager" runat="server" CssClass="default" />
                                                                                </div>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr valign="top">
                                                                <td ><asp:Label ID="lblNickName" runat="server" CssClass="default" Text="Nick Name :" /></td>
                                                                <td >
                                                                    <asp:TextBox ID="txtNickName" runat="server" CssClass="default" Width="300" />
                                                                    <asp:HiddenField ID="hdnOrderId" runat="server" />
                                                                </td>
                                                            </tr>
                                                            
                                                            <tr valign="top">
                                                                <td colspan="2" class="bold">&nbsp;</td>
                                                            </tr>
                                                            <tr valign="top">
                                                                <td colspan="2" class="bold">Asset Information:</td>
                                                            </tr>
                                                            <tr valign="top">
                                                                <td ><asp:Label ID="lblModel" runat="server" CssClass="default" Text="Model:" /></td>
                                                                <td ><asp:TextBox ID="txtModel" runat="server" CssClass="default" Width="400" Enabled="false" />
                                                                     <asp:HiddenField ID="hdnModel" runat="server" />
                                                                </td>
                                                            </tr>
                                                            <tr valign="top">
                                                                <td ><asp:Label ID="lblQuantity" runat="server" CssClass="default" Text="Requested Quantity:" /></td>
                                                                <td ><asp:TextBox ID="txtQuantity" runat="server" CssClass="default" Width="50" MaxLength="10" Enabled="false" /></td>
                                                            </tr>
                                                            <tr valign="top" style="display:none">
                                                                <td ><asp:Label ID="lblProcureQuantity" runat="server" CssClass="default" Text="Procure Quantity:<font class='required'>&nbsp;*</font>" /></td>
                                                                <td ><asp:TextBox ID="txtProcureQuantity" runat="server" CssClass="default" Width="200" MaxLength="10" Enabled="false" /></td>
                                                            </tr>
                                                            <tr valign="top" style="display:none">
                                                                <td ><asp:Label ID="lblReDeployQuantity" runat="server" CssClass="default" Text="Skip Quantity:<font class='required'>&nbsp;*</font>" /></td>
                                                                <td ><asp:TextBox ID="txtReDeployQuantity" runat="server" CssClass="default" Width="200" MaxLength="10" Enabled="false" /></td>
                                                            </tr>
                                                            <tr valign="top" style="display:none">
                                                                <td ><asp:Label ID="lblReturnedQuantity" runat="server" CssClass="default" Text="Returned Quantity:<font class='required'>&nbsp;*</font>" /></td>
                                                                <td ><asp:TextBox ID="txtReturnedQuantity" runat="server" CssClass="default" Width="200" MaxLength="10" Enabled="false" /></td>
                                                            </tr>
                                                            
                                                            <tr valign="top">
                                                                <td colspan="2" class="bold">&nbsp;</td>
                                                            </tr>
                                                            <tr valign="top">
                                                                <td colspan="2" class="bold">Internal Company Purchase Order Information:</td>
                                                            </tr>
                                                            <tr valign="top">
                                                                <td ><asp:Label ID="lblPurchaseOrderNumber" runat="server" CssClass="default" Text="Internal Company Purchase Order No.:<font class='required'>&nbsp;*</font>" /></td>
                                                                <td ><asp:TextBox ID="txtPurchaseOrderNumber" runat="server" CssClass="default" Width="200" MaxLength="50" /></td>
                                                            </tr>
                                                            <tr valign="top">
                                                                <td ><asp:Label ID="lblPurchaseOrderStatus" runat="server" CssClass="default" Text ="Internal Company Purchase Order Status:<font class='required'>&nbsp;*</font>" /></td>
                                                                <td > <asp:DropDownList ID="ddlPurchaseOrderStatus" runat="server" CssClass="default" Width="200" /></td>
                                                            </tr>
                                                            <tr valign="top">
                                                                <td ><asp:Label ID="lblPurchaseOrderDate" runat="server" CssClass="default"  Text="Internal Company Purchase Order Date :<font class='required'>&nbsp;*</font>" /></td>
                                                                <td ><asp:TextBox ID="txtPurchaseOrderDate" runat="server" CssClass="default" Width="100" MaxLength="10" />
                                                                    <asp:ImageButton ID="imgBtnPurchaseOrderDate" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" />
                                                                </td>
                                                            </tr> 
                                                            <tr valign="top">
                                                                <td ><asp:Label ID="lblApprovedQuantity" runat="server" CssClass="default" Text="Approved Quantity:<font class='required'>&nbsp;*</font>" /></td>
                                                                <td ><asp:TextBox ID="txtApprovedQuantity" runat="server" CssClass="default" Width="50" MaxLength="10" /></td>
                                                            </tr>
                                                            <tr valign="top">
                                                                <td ><asp:Label ID="lblApprovedOn" runat="server" CssClass="default"  Text="Approved On :<font class='required'>&nbsp;*</font>" /></td>
                                                                <td ><asp:TextBox ID="txtApprovedOn" runat="server" CssClass="default" Width="100" MaxLength="10" />
                                                                    <asp:ImageButton ID="imgApprovedOn" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" />
                                                                </td>
                                                            </tr> 
                                                            <tr valign="top">
                                                                <td ><asp:Label ID="lblPurchaseOrderUpload" runat="server" CssClass="default"  Text="Internal Company Purchase Order Upload :" /></td>
                                                                <td ><asp:Hyperlink ID="hypPurchaseOrderUpload" runat="server" CssClass="default" Target="_blank"  />
                                                                </td>
                                                            </tr> 
                                                            <tr valign="top">
                                                                <td ></td>
                                                                <td ><asp:FileUpload ID="filPurchaseOrderUpload" runat="server" Width="500" />
                                                                </td>
                                                            </tr> 
                                                            
                                                            
                                                            <tr valign="top">
                                                                <td colspan="2" class="bold">&nbsp;</td>
                                                            </tr>
                                                            <tr valign="top">
                                                                <td colspan="2" class="bold">Manufacture Purchase Order Information:</td>
                                                            </tr>
                                                            <tr valign="top">
                                                                <td ><asp:Label ID="lblQuoteNumber" runat="server" CssClass="default" Text="Manufacture Order Number :<font class='required'>&nbsp;*</font>"/></td>
                                                                <td ><asp:TextBox ID="txtQuoteNumber" runat="server" CssClass="default" Width="200" MaxLength="50"   /></td>
                                                            </tr>  
                                                            <tr valign="top">
                                                                <td ><asp:Label ID="lblQuoteDate" runat="server" CssClass="default"  Text="Manufacture Order Date :<font class='required'>&nbsp;*</font>" /></td>
                                                                <td ><asp:TextBox ID="txtQuoteDate" runat="server" CssClass="default" Width="100" MaxLength="10" />
                                                                    <asp:ImageButton ID="imgQuoteDate" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" />
                                                                </td>
                                                            </tr> 
                                                            <tr valign="top">
                                                                <td ><asp:Label ID="lblWarrantyDate" runat="server" CssClass="default"  Text="Manufacture Warranty Expiration Date :" /></td>
                                                                <td ><asp:TextBox ID="txtWarrantyDate" runat="server" CssClass="default" Width="100" MaxLength="10" Enabled="false" /></td>
                                                            </tr> 
                                                            <tr valign="top">
                                                                <td ><asp:Label ID="lblSystemPrice" runat="server" CssClass="default" Text="System Price :<font class='required'>&nbsp;*</font>"/></td>
                                                                <td >$<asp:TextBox ID="txtSystemPrice" runat="server" CssClass="default" Width="100"   /> (Cost of each item individually, without sales tax)</td>
                                                            </tr>  
                                                            <tr valign="top">
                                                                <td ><asp:Label ID="lblPurchaseOrderPrice" runat="server" CssClass="default" Text="Group Total :<font class='required'>&nbsp;*</font>"/></td>
                                                                <td >$<asp:TextBox ID="txtPurchaseOrderPrice" runat="server" CssClass="default" Width="100"   /> (Collective cost of all items without sales tax)</td>
                                                            </tr>  
                                                            <tr valign="top">
                                                                <td ><asp:Label ID="lblSalesTax" runat="server" CssClass="default" Text="Sales Tax :"/></td>
                                                                <td >$<asp:TextBox ID="txtSalesTax" runat="server" CssClass="default" Width="100"   /> (Sales tax of each item)</td>
                                                            </tr>  
                                                            <tr valign="top">
                                                                <td ><asp:Label ID="lblManufacturerQuoteUpload" runat="server" CssClass="default"  Text="Manufacturer Order Upload :" /></td>
                                                                <td ><asp:Hyperlink ID="hypManufacturerQuoteUpload" runat="server" CssClass="default" Target="_blank"  />
                                                                </td>
                                                            </tr> 
                                                            <tr valign="top">
                                                                <td ></td>
                                                                <td ><asp:FileUpload ID="filManufacturerQuoteUpload" runat="server" Width="500" />
                                                                </td>
                                                            </tr> 
                                                          <!-- Upload Manufacturer Quote (file upload) -->
                                                            
                                                            
                                                            <tr valign="top">
                                                                <td colspan="2" class="bold">&nbsp;</td>
                                                            </tr>
                                                            <tr valign="top">
                                                                <td colspan="2" class="bold">Shipping Information:</td>
                                                            </tr>
                                                            <tr valign="top">
                                                                <td ><asp:Label ID="lblVendorOrderStaus" runat="server" CssClass="default" Text ="Order Status :<font class='required'>&nbsp;*</font>" /></td>
                                                                <td > <asp:DropDownList ID="ddlVendorOrderStatus" runat="server" CssClass="default" Width="200" /></td>
                                                            </tr>
                                                             <tr valign="top">
                                                                <td ><asp:Label ID="lblVendorTrackingNumber" runat="server" CssClass="default" Text="Tracking Number :<font class='required'>&nbsp;*</font>" /></td>
                                                                <td ><asp:TextBox ID="txtVendorTrackingNumber" runat="server" CssClass="default" Width="200" MaxLength="200" /></td>
                                                            </tr>  
                                                            <tr valign="top">
                                                               <td ><asp:Label ID="lblVendorOrderDate" runat="server" CssClass="default" Text="Expected Arrival Date :<font class='required'>&nbsp;*</font>" /></td>
                                                               <td ><asp:TextBox ID="txtVendorOrderDate" runat="server" CssClass="default" Width="100" MaxLength="10" />
                                                                    <asp:ImageButton ID="imgBtnVendorOrderDate" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" />
                                                               </td>
                                                            </tr>
                                                            <tr valign="top">
                                                               <td ><asp:Label ID="lblAttentionTo" runat="server" CssClass="default" Text="Attention To :<font class='required'>&nbsp;*</font>" /></td>
                                                               <td >
                                                                    <table cellpadding="0" cellspacing="0" border="0">
                                                                        <tr>
                                                                            <td><asp:TextBox ID="txtAttentionTo" runat="server" Width="250" CssClass="default" /></td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <div id="divAttentionTo" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                                                                                    <asp:ListBox ID="lstAttentionTo" runat="server" CssClass="default" />
                                                                                </div>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                               </td>
                                                            </tr>


                                                            <tr valign="top">
                                                                <td colspan="2" class="bold">&nbsp;</td>
                                                            </tr>
                                                            <tr valign="top">
                                                                <td ><asp:Label ID="lblPurchaseOrderComments" runat="server" CssClass="default" Text ="Comments :" /></td>
                                                                <td ><asp:TextBox ID="txtPurchaseOrderComments" runat="server" CssClass="default" Width="500" TextMode="MultiLine" Rows="3" /></td>
                                                            </tr>   
                                                       </table>
                                                       
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
<asp:HiddenField ID="hdnAttentionTo" runat="server" />
<asp:HiddenField ID="hdnProjectManager" runat="server" />
