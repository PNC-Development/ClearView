<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="asset_order.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.asset_order" %>

<script type="text/javascript">
 
    var trOrderType = null;
    var rblOrderType = null;
    var trDescription  = null;
    var txtDescription = null;
    var trModel = null;
    var hdnAssetCategoryId = null;
    var ddlModel = null;
    var trLocation = null;
    var trRoom = null;
    var trZone =null;
    var trRack = null;
    var hdnRackId = null;
    var trRackPos = null;
    var txtRackPos = null;
    var trResiliency = null;
    var ddlResiliency = null;
    var trOperatingSystemGroup = null;
    var ddlOperatingSystemGroup = null;
    var btnSelectLocation = null;
    var trEnclosure = null;
    var ddlEnclosure = null;
    var trEnclosureSlot = null;
    var txtEnclosureSlot = null;
    var trClass2 = null;
    var ddlClass = null;
    var trEnvironment = null;
    var ddlEnvironment = null;
    var hdnEnvironment = null;

    var trQuantity = null;
    var txtQuantity = null;
    
    var trSelection = null;
    var trRequestByDate = null;
    var txtRequestedByDate = null;
    var trClustered = null;
    var trSanAttached = null;
    var trBootLuns = null;
    var trSwitch1 = null;
    var trPort1 = null;
    var trSwitch2 = null;
    var trPort2 = null;

    var trAssetsSummary = null;
    var lblTotalAssetsSelected =null;
    var trAssetsSelection = null;
    var pnlAssetsSelection = null;
   
    var old_onload = window.onload;
    window.onload = new function() 
    {
        addDOMLoadEvent(function() {
            NewLoadEvent();
        });
    }; 

    function NewLoadEvent() {
        trOrderType = document.getElementById('<%=trOrderType.ClientID%>');
        rblOrderType = document.getElementById('<%=rblOrderType.ClientID%>');
        trDescription  = document.getElementById('<%=trDescription.ClientID%>');
        txtDescription = document.getElementById('<%=txtDescription.ClientID%>');
        trModel = document.getElementById('<%=trModel.ClientID%>');
        ddlModel = document.getElementById('<%=ddlModel.ClientID%>');
        hdnAssetCategoryId = document.getElementById('<%=hdnAssetCategoryId.ClientID%>');
        trLocation = document.getElementById('<%=trLocation.ClientID%>');
        trRoom = document.getElementById('<%=trRoom.ClientID%>');
        trZone = document.getElementById('<%=trZone.ClientID%>');
        trRack = document.getElementById('<%=trRack.ClientID%>');
        hdnRackId = document.getElementById('<%=hdnRackId.ClientID%>');
        trRackPos = document.getElementById('<%=trRackPos.ClientID%>');
        txtRackPos = document.getElementById('<%=txtRackPos.ClientID%>');
        trResiliency = document.getElementById('<%=trResiliency.ClientID%>');
        ddlResiliency = document.getElementById('<%=ddlResiliency.ClientID%>');
        trOperatingSystemGroup = document.getElementById('<%=trOperatingSystemGroup.ClientID%>');
        ddlOperatingSystemGroup = document.getElementById('<%=ddlOperatingSystemGroup.ClientID%>');
        btnSelectLocation = document.getElementById('<%=btnSelectLocation.ClientID%>');
        trEnclosure = document.getElementById('<%=trEnclosure.ClientID%>');
        ddlEnclosure = document.getElementById('<%=ddlEnclosure.ClientID%>');
        trEnclosureSlot = document.getElementById('<%=trEnclosureSlot.ClientID%>');
        txtEnclosureSlot = document.getElementById('<%=txtEnclosureSlot.ClientID%>');
        trClass2 = document.getElementById('<%=trClass.ClientID%>');
        ddlClass = document.getElementById('<%=ddlClass.ClientID%>');
        trEnvironment = document.getElementById('<%=trEnvironment.ClientID%>');
        ddlEnvironment = document.getElementById('<%=ddlEnvironment.ClientID%>');
        hdnEnvironment = document.getElementById('<%=hdnEnvironment.ClientID%>');
        trQuantity = document.getElementById('<%=trQuantity.ClientID%>');
        txtQuantity = document.getElementById('<%=txtQuantity.ClientID%>');
        trRequestByDate = document.getElementById('<%=trRequestByDate.ClientID%>');
        txtRequestedByDate = document.getElementById('<%=txtRequestedByDate.ClientID%>');
        trClustered = document.getElementById('<%=trClustered.ClientID%>');
        trSanAttached = document.getElementById('<%=trSanAttached.ClientID%>');
        trBootLuns = document.getElementById('<%=trBootLuns.ClientID%>');
        trSwitch1 = document.getElementById('<%=trSwitch1.ClientID%>');
        trPort1 = document.getElementById('<%=trPort1.ClientID%>');
        trSwitch2 = document.getElementById('<%=trSwitch2.ClientID%>');
        trPort2 = document.getElementById('<%=trPort2.ClientID%>');
        trSelection = document.getElementById('<%=trSelection.ClientID%>');
        trAssetsSummary = document.getElementById('<%=trAssetsSummary.ClientID%>');
        lblTotalAssetsSelected = document.getElementById('<%=lblTotalAssetsSelected.ClientID%>');

        trAssetsSelection = document.getElementById('<%=trAssetsSelection.ClientID%>');
        pnlAssetsSelection = document.getElementById('<%=pnlAssetsSelection.ClientID%>');


        setFieldsForOrderTypeAndModelSelection();
    }
    function setFieldsForOrderTypeAndModelSelection()
    {
     
        trModel.style.display="inline";
        trQuantity.style.display="inline";
        trRequestByDate.style.display = "inline";
        
        trLocation.style.display="none";   
        trRoom.style.display="none";
        trZone.style.display="none";
        trRack.style.display="none";
        trResiliency.style.display="none";
        trOperatingSystemGroup.style.display="none";
        trEnclosure.style.display="none";
        trEnclosureSlot.style.display="none";
        trClass2.style.display="none";
        trEnvironment.style.display="none";

        trClustered.style.display = "none";
        trSanAttached.style.display = "none";
        trBootLuns.style.display = "none";
        trSwitch1.style.display = "none";
        trPort1.style.display = "none";
        trSwitch2.style.display = "none";
        trPort2.style.display = "none";

        trSelection.style.display = "none";
        var orderType = GetValue(rblOrderType);
        if (hdnAssetCategoryId.value=="1")  //Physical Server
        {
            trLocation.style.display="inline";   
            trRoom.style.display="inline";
            trZone.style.display="inline";
            trRack.style.display="inline";
            trRackPos.style.display="inline";
            trResiliency.style.display="inline";
            trOperatingSystemGroup.style.display="inline";
            trClass2.style.display="inline";
            trEnvironment.style.display = "inline";
        }
        else if (hdnAssetCategoryId.value=="2") //Blade
        {
//            trLocation.style.display="inline";   
            trResiliency.style.display="inline";
            trOperatingSystemGroup.style.display="inline";
            trEnclosure.style.display="inline";
            trEnclosureSlot.style.display="inline";
            trClass2.style.display="inline";
            trEnvironment.style.display="inline";
        }
        else if (hdnAssetCategoryId.value=="3") //Enclosure
        {
            trLocation.style.display="inline";   
            trRoom.style.display="inline";
            trZone.style.display="inline";
            trRack.style.display="inline";
            trRackPos.style.display="inline";
            trResiliency.style.display="inline";
            trOperatingSystemGroup.style.display="inline";
        }
        else if (hdnAssetCategoryId.value=="4") //Rack
        {
            trLocation.style.display="inline";   
            trRoom.style.display="inline";
            trZone.style.display="inline";
        }
       
        //Base on Order Type
        if (orderType == "1") {
            trSelection.style.display = "none";
            trBootLuns.style.display = "inline";
            trSanAttached.style.display = "inline";
            trClustered.style.display = "inline";
            trSwitch1.style.display = "inline";
            trPort1.style.display = "inline";
            trSwitch2.style.display = "inline";
            trPort2.style.display = "inline";
        }
        else if (orderType == "2" || orderType == "3" || orderType == "4")
            trSelection.style.display = "inline";
   
        if (orderType=="4") //Distroy
        {
          trLocation.style.display="none";
          trRoom.style.display="none";
          trZone.style.display="none";
          trRack.style.display="none";
          trRackPos.style.display="none";
          trResiliency.style.display="none";
          trOperatingSystemGroup.style.display="none";
          trEnclosure.style.display="none";
          trEnclosureSlot.style.display="none";
          trClass2.style.display="none";
          trEnvironment.style.display="none";
        }
        
        //Clean up Values if display is none.        
        if (trLocation.style.display=="none") hdnRackId.value="0";
        if (trEnclosure.style.display=="none")ddlEnclosure.options[0].selected = true;
        if (trEnclosureSlot.style.display=="none") txtEnclosureSlot.value="0";
        if (trClass2.style.display=="none") ddlClass.options[0].selected = true;
        if (trEnvironment.style.display=="none")
        {  if( ddlEnvironment.options.length >0)
            ddlEnvironment.options[0].selected = true;
            hdnEnvironment.value="0";
        }
        if (trResiliency.style.display=="none")ddlResiliency.options[0].selected = true;
        if (trOperatingSystemGroup.style.display=="none")ddlOperatingSystemGroup.options[0].selected = true;
     }
   var oActiveXAssetCategory = null;
   
   function getAssetCategory() 
   {
        if (ddlModel.options[ddlModel.selectedIndex].value>0)
        {
            oActiveXAssetCategory = new ActiveXObject("Microsoft.XMLHTTP");
            oActiveXAssetCategory.onreadystatechange = getAssetCategory_a;
            oActiveXAssetCategory.open("GET", "/frame/ajax/ajax_asset_category.aspx?u=GET", false);
            oActiveXAssetCategory.send("<ajax><value>" + ddlModel.options[ddlModel.selectedIndex].value + "</value></ajax>");
        }
        else
        {  
            hdnAssetCategoryId.value="0";
        }
       return true;
    }
  function getAssetCategory_a() 
  {
    if (oActiveXAssetCategory.readyState == 4)
    {   
        if (oActiveXAssetCategory.status == 200) 
        {
            var or = oActiveXAssetCategory.responseXML.documentElement.childNodes;
            hdnAssetCategoryId.value=or[0].childNodes[0].text;
        }
        else 
        {
            hdnAssetCategoryId.value="0";
            alert('There was a problem getting the information');
        }
    }
  }
    
   function GetValue(radioList) //Get the radio button list selected value
   {
       var options = radioList.getElementsByTagName('input');
            for(i=0;i<options.length;i++)
            { var opt = options[i];
              if(opt.checked)
              {
                return opt.value;
              }
            }
    }
   
    
    
    function CheckUnCheckAll(chkAssetsSelectionSelectAll)
    {
       var chkState = chkAssetsSelectionSelectAll.checked;  
       var inputElements = pnlAssetsSelection.getElementsByTagName("input");
        for(i=0;i<inputElements.length;i++)
        {  
            if(inputElements[i].type=='checkbox' && inputElements[i].name.indexOf('chkSelectAsset') != -1 && inputElements[i].disabled==false)
            {   
            inputElements[i].checked=chkState; 
            }
        }
    }
    
    
    function  Validations(boolValidateQuality)
    {
    
        //Order Type
        var orderType = GetValue(rblOrderType);
        if (ValidateRadioList('<%=rblOrderType.ClientID %>','Please select Order type')==false) return false;
        if (ValidateText('<%=txtDescription.ClientID %>','Please enter a request description')==false) return false;
        if (ValidateDropDown('<%=ddlModel.ClientID %>','Please select a model')==false) return false;
        
        if (orderType !=4)
        {
            if (hdnAssetCategoryId.value=="1")  //Physical Server
            {   
                if (ValidateHidden0('<%=hdnRackId.ClientID %>','<%=btnSelectLocation.ClientID %>','Please select intended location')==false) return false;
                if (ValidateDropDown('<%=ddlClass.ClientID %>','Please select intended class')==false) return false;
                if (ValidateDropDown('<%=ddlEnvironment.ClientID %>','Please select intended environment')==false) return false;
            }
            else if (hdnAssetCategoryId.value=="2") //Blade
            {
//                if (ValidateHidden0('<%=hdnRackId.ClientID %>','<%=btnSelectLocation.ClientID %>','Please select intended location')==false) return false;
                if (ValidateDropDown('<%=ddlEnclosure.ClientID %>','Please select intended enclosure')==false) return false;
                if (ValidateNumber('<%=txtEnclosureSlot.ClientID%>','Please select enclosure slot')==false) return false;
                if (ValidateDropDown('<%=ddlClass.ClientID %>','Please select intended class')==false) return false;
                if (ValidateDropDown('<%=ddlEnvironment.ClientID %>','Please select intended environment')==false) return false;
            }
            else if (hdnAssetCategoryId.value=="3") //Enclosure
            {
                if (ValidateHidden0('<%=hdnRackId.ClientID %>','<%=btnSelectLocation.ClientID %>','Please select intended location')==false) return false;
                if (ValidateNumber0('<%= txtQuantity.ClientID%>','Please enter a valid quantity')==false) return false;
            }
            else if (hdnAssetCategoryId.value=="4") //Rack
            {
                if (ValidateHidden0('<%=hdnRackId.ClientID %>','<%=btnSelectLocation.ClientID %>','Please select intended location')==false) return false;
            }
       }
       
        if (ValidateNumber0('<%= txtQuantity.ClientID%>','Please enter a valid quantity')==false) return false;
        if (ValidateDate('<%=txtRequestedByDate.ClientID%>','Please enter requested by date')==false) return false;
      
        if (orderType != 1 && boolValidateQuality==true)  //Validated selected assets quantity
        { 
            if (txtQuantity.value!=lblTotalAssetsSelected.innerHTML)
            {
                alert('Please select the ' + txtQuantity.value +' asset(s) as per mentioned quantity');
                return false;
            }
        }
        
        return true;
    }
    
    
 
</script>


<asp:Panel ID="pnlInfo" runat="server" Visible="false" width="100%">
     <table width="98%" cellpadding="2" cellspacing="2" border="0">
        <tr>
            <td><asp:Label ID="lblInfo" runat="server" Text="" /></td>
            <td align="right"><asp:Button ID="btnNewRequest" runat="server" CssClass="default" Width="100" Text="New Request" OnClick="btnNewRequest_Click" /></td>
        </tr>
        <tr><td colspan="2">&nbsp;</td></tr>
        <tr>
            <td colspan="2"><hr size="1" noshade /></td>
        </tr>
    </table>
</asp:Panel>

<asp:Panel ID="pnlOrder" runat="server" width="100%" Visible="false">
                
<table width="98%" cellpadding="4" cellspacing="0" border="0" >
    <tr>
        <td colspan="2" class="header">Order Asset(s)</td>
    </tr>
     <tr id="trOrderType" runat="server">
        <td width="15%" ><asp:Label ID="lblOrderType" runat="server" CssClass="default" Text="Order Type:<font class='required'>&nbsp;*</font>" /></td>
        <td>
            <asp:RadioButtonList ID="rblOrderType" runat="server" CssClass="default" RepeatDirection="Horizontal" >
                <asp:ListItem Value="1"  Text="Asset(s) Procure"/>
                <asp:ListItem Value="2" Text="Asset(s) Re Deploy"/>
                <asp:ListItem Value="3" Text="Asset(s) Movement"/>
                <asp:ListItem Value="4" Text="Asset(s) Dispose"/>
            </asp:RadioButtonList>
            
         </td>
    </tr>
    <tr id="trRequest" runat="server" visible="false" >
        <td ><asp:Label ID="lblRequest" runat="server" CssClass="default" Text="RequestID:<font class='required'>&nbsp;*</font>" /></td>
        <td><asp:Label ID="lblRequestID" runat="server" CssClass="default" /></td>
    </tr>
    <tr id="trDescription" runat="server" >
        <td ><asp:Label ID="lblDescription" runat="server" CssClass="default" Text="Description:<font class='required'>&nbsp;*</font>" /></td>
        <td><asp:TextBox ID="txtDescription" runat="server" CssClass="default" MaxLength="300" Width="300" /></td>
    </tr>
    <tr id="trModel" runat="server" >
        <td ><asp:Label ID="lblModel" runat="server" CssClass="default" Text="Model:<font class='required'>&nbsp;*</font>" /></td>
        <td ><asp:DropDownList ID="ddlModel" runat="server" CssClass="default" Width="500"  /><br /><b>NOTE:</b> If a model does not exist, it is because the AVAILABLE flag is not set.</td>
    </tr>
    <tr id="trLocation" runat="server" style="display:none" >
        <td >
            <asp:Label ID="lblLocation" runat="server" CssClass="default" Text="Intended Location :<font class='required'>&nbsp;*</font>" />
        </td>
        <td >
            <asp:TextBox ID="txtLocation" CssClass="lightdefault" runat="server"  Width="500" ReadOnly="true" />

            <asp:HiddenField ID="hdnLocationId" runat="server" />
            &nbsp
            <asp:Button ID="btnSelectLocation" runat="server" Text="..." CssClass="default"  Width="25"   />
        </td>
    </tr>

    <tr id="trRoom" runat="server" style="display:none" >
        <td >
            <asp:Label ID="lblRoom" runat="server" CssClass="default" Text="Intended Room:<font class='required'>&nbsp;*</font>" />
        </td>
        <td >
            <asp:TextBox ID="txtRoom" CssClass="lightdefault" runat="server"  Width="250" ReadOnly="true" />
            <asp:HiddenField ID="hdnRoomId" runat="server" />
        </td>
    </tr>
    <tr id="trZone" runat="server" style="display:none" >
        <td >
            
            <asp:Label ID="lblZone" runat="server" CssClass="default" Text="Intended Zone:<font class='required'>&nbsp;*</font>" />
        </td>
        <td >
            <asp:TextBox ID="txtZone" CssClass="lightdefault" runat="server"  Width="250" ReadOnly="true" />    
            <asp:HiddenField ID="hdnZoneId" runat="server" />

        </td>
     </tr>
     <tr id="trRack" runat="server" style="display:none" >
        <td >
            <asp:Label ID="lblRack" runat="server" CssClass="default" Text="Intended Rack:<font class='required'>&nbsp;*</font>" />
        </td>
        <td >
            <asp:TextBox ID="txtRack" CssClass="lightdefault" runat="server"  Width="250" ReadOnly="true" />
            <asp:HiddenField ID="hdnRackId" runat="server" />
        </td>                            
     </tr>
     <tr id="trRackPos" runat="server" style="display:none" >
        <td >
            <asp:Label ID="lblRackPos" runat="server" CssClass="default" Text="Intended Rack Position:<font class='required'>&nbsp;*</font>" />
        </td>
        <td >
            <asp:TextBox ID="txtRackPos" CssClass="lightdefault" runat="server"  Width="100" MaxLength="10" />
            <asp:HiddenField ID="hddnRackPos" runat="server" />
        </td>                            
     </tr>
    <tr id="trResiliency" runat="server" style="display:none">
        <td ><asp:Label ID="lblResiliency" runat="server" CssClass="default" Text="Intended Resiliency:<font class='required'>&nbsp;*</font>" /></td>
        <td ><asp:DropDownList ID="ddlResiliency" runat="server" CssClass="default" Width="500" /></td>
    </tr>
    <tr id="trOperatingSystemGroup" runat="server" style="display:none">
        <td ><asp:Label ID="lblOperatingSystemGroup" runat="server" CssClass="default" Text="Operating System Group:<font class='required'>&nbsp;*</font>" /></td>
        <td ><asp:DropDownList ID="ddlOperatingSystemGroup" runat="server" CssClass="default" Width="500" /></td>
    </tr>
    <tr id="trEnclosure" runat="server" style="display:none">
        <td ><asp:Label ID="lblEnclosure" runat="server" CssClass="default" Text="Intended Enclosure:<font class='required'>&nbsp;*</font>" /></td>
        <td ><asp:DropDownList ID="ddlEnclosure" runat="server" CssClass="default" Width="500" /></td>
    </tr>
    <tr id="trEnclosureSlot" runat="server" style="display:inline">
        <td ><asp:Label ID="lblEnclosureSlot" runat="server" CssClass="default" Text="Intended Enclosure Slot:<font class='required'>&nbsp;*</font>" /></td>
        <td><asp:TextBox ID="txtEnclosureSlot" runat="server" CssClass="default" MaxLength="5" Width="80" /></td>
    </tr>
    <tr id="trClass" runat="server" style="display:inline">
        <td ><asp:Label ID="lblClass" runat="server" CssClass="default" Text="Intended Class:<font class='required'>&nbsp;*</font>" /></td>
        <td ><asp:DropDownList ID="ddlClass" runat="server" CssClass="default" Width="300" /></td>
    </tr>
     <tr id="trEnvironment" runat="server" style="display:none">
        <td ><asp:Label ID="lblEnvironment" runat="server" CssClass="default" Text="Intended Environment:<font class='required'>&nbsp;*</font>" /></td>
        <td ><asp:DropDownList ID="ddlEnvironment" runat="server" CssClass="default" Width="300" /></td>
    </tr>
    <tr id="trQuantity" runat="server" style="display:none">
        <td ><asp:Label ID="lblQuantity" runat="server" CssClass="default" Text="Quantity:<font class='required'>&nbsp;*</font>" /></td>
        <td><asp:TextBox ID="txtQuantity" runat="server" CssClass="default" MaxLength="10" Width="80" /></td>
    </tr>
    <tr id="trRequestByDate" runat="server" style="display:none">
        <td ><asp:Label ID="lblRequestedByDate" runat="server" CssClass="default"  Text="Requested By Date:<font class='required'>&nbsp;*</font>" /></td>
        <td ><asp:TextBox ID="txtRequestedByDate" runat="server" CssClass="default" Width="100" MaxLength="10" />
            <asp:ImageButton ID="imgbtnRequestedByDate" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" />
        </td>
    </tr> 
    <tr id="trClustered" runat="server" style="display:none">
        <td ><asp:Label ID="lblClustered" runat="server" CssClass="default" Text="Is Clustered:<font class='required'>&nbsp;*</font>" /></td>
        <td><asp:CheckBox ID="chkClustered" runat="server" CssClass="default" Text="(Check this box if these assets are part of a cluster)" /></td>
    </tr>
    <tr id="trSanAttached" runat="server" style="display:none">
        <td ><asp:Label ID="lblSanAttached" runat="server" CssClass="default" Text="Is SAN Attached:<font class='required'>&nbsp;*</font>" /></td>
        <td><asp:CheckBox ID="chkSanAttached" runat="server" CssClass="default" Text="(Check this box if these assets should be attached to SAN)" /></td>
    </tr>
    <tr id="trBootLuns" runat="server" style="display:none">
        <td ><asp:Label ID="lblBootLuns" runat="server" CssClass="default" Text="Has Boot Luns:<font class='required'>&nbsp;*</font>" /></td>
        <td><asp:CheckBox ID="chkBootLuns" runat="server" CssClass="default" Text="(Check this box if these assets should have boot luns assigned)" /></td>
    </tr>
     <tr id="trSwitch1" runat="server" style="display:none">
        <td ><asp:Label ID="lblSwitch1" runat="server" CssClass="default" Text="Switch # 1:<font class='required'>&nbsp;*</font>" /></td>
        <td ><asp:DropDownList ID="ddlSwitch1" runat="server" CssClass="default" Width="300" /></td>
    </tr>
    <tr id="trPort1" runat="server" style="display:none">
        <td ><asp:Label ID="lblPort1" runat="server" CssClass="default" Text="Port # 1:<font class='required'>&nbsp;*</font>" /></td>
        <td><asp:TextBox ID="txtPort1" runat="server" CssClass="default" MaxLength="10" Width="80" /> (Format: 101/1/28)</td>
    </tr>
     <tr id="trSwitch2" runat="server" style="display:none">
        <td ><asp:Label ID="lblSwitch2" runat="server" CssClass="default" Text="Switch # 2:<font class='required'>&nbsp;*</font>" /></td>
        <td ><asp:DropDownList ID="ddlSwitch2" runat="server" CssClass="default" Width="300" /></td>
    </tr>
    <tr id="trPort2" runat="server" style="display:none">
        <td ><asp:Label ID="lblPort2" runat="server" CssClass="default" Text="Port # 2:<font class='required'>&nbsp;*</font>" /></td>
        <td><asp:TextBox ID="txtPort2" runat="server" CssClass="default" MaxLength="10" Width="80" /> (Format: 111/1/28)</td>
    </tr>
    
   
    <tr id="trSelection" runat="server" style="display:none">
        <td nowrap>&nbsp;</td>
        <td >
            <asp:Button ID="btnViewSelection" runat="server" CssClass="default" Width="150" Text="Select Assets" OnClick="btnViewSelection_Click" />
            &nbsp;&nbsp;&nbsp;
            <asp:Button ID="btnClearSelection" runat="server" CssClass="default" Width="150" Text="Clear Selection" OnClick="btnClearSelection_Click" />
            
        </td>
    </tr>
   
    
     <tr id="trAssetsSummary" runat="server" style="display:none">
        <td colspan="2">
            <table width="100%" cellpadding="4" cellspacing="0" border="1"  class="default">
                <tr><td class="header" style="border-width:0">Asset(s) Summary</td>
                    <td align="right" class="header" style="border-width:0">Total Asset(s) Selected : 
                        <asp:Label ID="lblTotalAssetsSelected" runat="server" CssClass="header" Text="0" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="border-width:1" >
                    <asp:Panel ID="pnlAssetsSummary" runat="server" Visible="true"  Width="100%" height="150" ScrollBars="Auto"   >
                        <asp:DataList ID="dlAssetsSummary" runat="server" CellPadding="2" CellSpacing="2" Width="100%" OnItemDataBound="dlAssetsSummary_ItemDataBound" OnItemCommand="dlAssetsSummary_Command">
                            <HeaderTemplate>
                                <tr bgcolor="#EEEEEE" valign="top" class="default">
                                    <td align="left" width="30%">
                                        <asp:Label ID="lblAssetsSummaryHeaderLocation" runat="server" Text="<b>Current Location</b>"  />
                                    </td>
                                    <td align="left" width="10%"  style="display: <%=hdnAssetCategoryId.Value=="1" ||hdnAssetCategoryId.Value=="2"?"inline":"none" %>">
                                        <asp:Label ID="lblAssetsSummaryHeaderClass" runat="server" Text="<b>Current Class</b>"  />
                                    </td>
                                    <td align="left" width="10%"  style="display: <%=hdnAssetCategoryId.Value=="1" ||hdnAssetCategoryId.Value=="2"?"inline":"none" %>">
                                        <asp:Label ID="lblAssetsSummaryHeaderEnv" runat="server" Text="<b>Current Environment</b>"  />
                                    </td>
                                    <td align="left" width="10%">
                                        <asp:Label ID="lblAssetsSummaryHeaderCount" runat="server" Text="<b>Total Assets #</b>"  />
                                    </td>
                                    <td align="left" width="10%">
                                        <asp:Label ID="lblAssetsSummaryHeaderSelected" runat="server" Text="<b>Selected #</b>"  />
                                    </td>
                                    <td align="left" width="10%">
                                        <asp:Label ID="lblAssetsSummaryHeaderLocationSelect" runat="server" Text=""  />
                                    </td>
                                </tr>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr class="default" valign="top">
                                    <td align="left">
                                        <asp:Label ID="lblAssetsSummaryLocation" runat="server" />
                                        <asp:HiddenField ID="hdnAssetsAvailableLocationId" runat="server" />
                                    </td>
                                    <td align="left"  style="display: <%=hdnAssetCategoryId.Value=="1" ||hdnAssetCategoryId.Value=="2"?"inline":"none" %>">
                                        <asp:Label ID="lblAssetsSummaryClass" runat="server" />
                                        <asp:HiddenField ID="hdnAssetsAvailableClassId" runat="server" />
                                    </td>
                                    <td align="left"  style="display: <%=hdnAssetCategoryId.Value=="1" ||hdnAssetCategoryId.Value=="2"?"inline":"none" %>">
                                        <asp:Label ID="lblAssetsSummaryEnviorment" runat="server" />
                                        <asp:HiddenField ID="hdnAssetsAvailableEnvironmentId" runat="server" />
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="lblAssetsSummaryCount" runat="server" />
                                    </td>
                                    <td align="left" >
                                        <asp:Label ID="lblAssetsSummarySelected" runat="server" />
                                    </td>
                                    <td align="left" >
                                        <asp:LinkButton ID="lnkbtnAssetsAvailableLocationSelect" runat="server"  Text="Select" CssClass="lookup" /> 
                                    </td>
                                </tr>
                            </ItemTemplate>   
                            <AlternatingItemTemplate>
                                <tr bgcolor="#F6F6F6" valign="top" class="default">
                                    <td align="left">
                                        <asp:Label ID="lblAssetsSummaryLocation" runat="server" />
                                        <asp:HiddenField ID="hdnAssetsAvailableLocationId" runat="server" />
                                    </td>
                                    <td align="left"  style="display: <%=hdnAssetCategoryId.Value=="1" ||hdnAssetCategoryId.Value=="2"?"inline":"none" %>">
                                        <asp:Label ID="lblAssetsSummaryClass" runat="server"  />
                                        <asp:HiddenField ID="hdnAssetsAvailableClassId" runat="server" />
                                    </td>
                                    <td align="left"  style="display: <%=hdnAssetCategoryId.Value=="1" ||hdnAssetCategoryId.Value=="2"?"inline":"none" %>">
                                        <asp:Label ID="lblAssetsSummaryEnviorment" runat="server" />
                                        <asp:HiddenField ID="hdnAssetsAvailableEnvironmentId" runat="server" />
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="lblAssetsSummaryCount" runat="server"/>
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="lblAssetsSummarySelected" runat="server" />
                                    </td>
                                    <td align="left" >
                                        <asp:LinkButton ID="lnkbtnAssetsAvailableLocationSelect" runat="server"  Text="Select" CssClass="lookup" /> 
                                    </td>
                                </tr>
                            </AlternatingItemTemplate>
                             <SelectedItemTemplate>
                                <tr bgcolor="#FFF68F" valign="top" class="default">
                                    <td align="left">
                                        <asp:Label ID="lblAssetsSummaryLocation" runat="server"  />
                                        <asp:HiddenField ID="hdnAssetsAvailableLocationId" runat="server" />
                                    </td>
                                    <td align="left"  style="display: <%=hdnAssetCategoryId.Value=="1" ||hdnAssetCategoryId.Value=="2"?"inline":"none" %>">
                                        <asp:Label ID="lblAssetsSummaryClass" runat="server"  />
                                        <asp:HiddenField ID="hdnAssetsAvailableClassId" runat="server" />
                                    </td>
                                    <td align="left"  style="display: <%=hdnAssetCategoryId.Value=="1" ||hdnAssetCategoryId.Value=="2"?"inline":"none" %>">
                                        <asp:Label ID="lblAssetsSummaryEnviorment" runat="server"  />
                                        <asp:HiddenField ID="hdnAssetsAvailableEnvironmentId" runat="server" />
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="lblAssetsSummaryCount" runat="server"  />
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="lblAssetsSummarySelected" runat="server"   />
                                    </td>
                                    <td align="left" >
                                        <asp:LinkButton ID="lnkbtnAssetsAvailableLocationSelect" runat="server"  Text="Select" CssClass="lookup" /> 
                                    </td>
                                </tr>
                            </SelectedItemTemplate>
                            
                        </asp:DataList>
                    </asp:Panel>
                    </td>
                </tr>
            </table>
        </td>
     </tr>
     <tr id="trAssetsSelection" runat="server" style="display:none" >
        <td colspan="2">
                <table width="100%" cellpadding="4" cellspacing="0" border="1"  class="default">
                    <tr>
                        <td class="header" style="border-width:0">Asset(s) Selected : 
                       
                        </td>
                        <td align="right" style="border-width:0">
                        <asp:Button ID="btnSaveSelection" runat="server" CssClass="default" Width="100" Text="Save Selection" OnClick="btnSaveSelection_Click" />
                        </td>
                    </tr>
                     <tr>
                        <td colspan="2" style="border-width:0"> Search :
                           <asp:TextBox ID="txtSearch" runat="server" CssClass="default" MaxLength="300" Width="300"  />
                            <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="default" Width="100"  />
                        </td>
                     </tr>
                    <tr><td colspan="2" style="border-width:1" >
                        <asp:Panel ID="pnlAssetsSelection" runat="server" Visible="true"  Width="100%" height="200" ScrollBars="Auto" >
                            <asp:DataList ID="dlAssetsSelection" runat="server" CellPadding="2" CellSpacing="2" Width="100%" OnItemDataBound="dlAssetsSelection_ItemDataBound" >
                                    <HeaderTemplate>
                                        <tr bgcolor="#EEEEEE" valign="top" class="default">
                                            <td align="left" width="0%">
                                                <input type="checkbox" id="chkAssetsSelectionSelectAll" onclick="CheckUnCheckAll(this);" runat="server" name="chkAssetsSelectionSelectAll">
                                            </td>
                                            <td align="left" width="10%">
                                                <asp:Label ID="lblAssetsSelectionHeaderSerial" runat="server"  Text="<b>Serial</b>"  />
                                            </td>
                                            <td align="left" width="10%">
                                                <asp:Label ID="lblAssetsSelectionHeaderAssetTag" runat="server" Text="<b>AssetTag</b>"  />
                                            </td>
                                            <td align="left" width="10%">
                                                <asp:Label ID="lblAssetsSelectionHeaderStatus" runat="server" Text="<b>Asset Status</b>"  />
                                            </td>
                                              <td align="left" width="30%">
                                                <asp:Label ID="lblAssetsSelectionHeaderLocation" runat="server" Text="<b>Current Location</b>"  />
                                            </td>
                                            <td align="left" width="10%" style="display: <%=hdnAssetCategoryId.Value=="1" ||hdnAssetCategoryId.Value=="2"?"inline":"none" %>">
                                                <asp:Label ID="lblAssetsSelectionHeaderClass" runat="server" Text="<b>Current Class</b>"  />
                                            </td>
                                            <td align="left" width="10%" style="display: <%=hdnAssetCategoryId.Value=="1" ||hdnAssetCategoryId.Value=="2"?"inline":"none" %>">
                                                <asp:Label ID="lblAssetsSelectionHeaderEnvironment" runat="server" Text="<b>Current Environment</b>"  />
                                            </td>
                                            <td align="left" width="10%" style="display: <%=hdnAssetCategoryId.Value=="2"?"inline":"none" %>">
                                                <asp:Label ID="lblAssetsSelectionHeaderEnclosure" runat="server"  Text="<b>Current Enclosure</b>"  />
                                            </td>
                                            <td align="left" width="10%" style="display: <%=hdnAssetCategoryId.Value=="2"?"inline":"none" %>">
                                                <asp:Label ID="lblAssetsSelectionHeaderEnclosureslot" runat="server"  Text="<b>Current Enclosure Slot</b>"  />
                                            </td>
                                            <td align="left" width="10%">
                                                <asp:Label ID="lblAssetsSelectionHeaderRoom" runat="server"  Text="<b>Current Room</b>"  />
                                            </td>
                                            <td align="left" width="10%" style="display: <%=hdnAssetCategoryId.Value!="4"?"inline":"none" %>">
                                                <asp:Label ID="lblAssetsSelectionHeaderRack" runat="server"  Text="<b>Current Rack</b>"  />
                                            </td>
                                        </tr>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr class="default" valign="top">
                                            <td align="left">
                                                <asp:CheckBox ID="chkSelectAsset" runat="server" />
                                            </td>
                                            <td align="left">
                                                <asp:Label ID="lblAssetsSelectionSerial" runat="server" />
                                                <asp:HiddenField ID="hdnAssetId" runat="server" />
                                            </td>
                                            <td align="left">
                                                <asp:Label ID="lblAssetsSelectionAssetTag" runat="server" />
                                            </td>
                                            <td align="left">
                                                <asp:Label ID="lblAssetsSelectionAssetStatus" runat="server" />
                                            </td>
                                             <td align="left">
                                                <asp:Label ID="lblAssetsSelectionLocation" runat="server" />
                                            </td>
                                            <td align="left"  style="display: <%=hdnAssetCategoryId.Value=="1" ||hdnAssetCategoryId.Value=="2"?"inline":"none" %>">
                                                <asp:Label ID="lblAssetsSelectionClass" runat="server" />
                                            </td>
                                            <td align="left"  style="display: <%=hdnAssetCategoryId.Value=="1" ||hdnAssetCategoryId.Value=="2"?"inline":"none" %>">
                                                <asp:Label ID="lblAssetsSelectionEnvironment" runat="server" />
                                            </td>
                                            <td align="left" nowrap  style="display: <%=hdnAssetCategoryId.Value=="2"?"inline":"none" %>">
                                                <asp:Label ID="lblAssetsSelectionEnclosure" runat="server" />
                                            </td>
                                            <td align="left" nowrap  style="display: <%=hdnAssetCategoryId.Value=="2"?"inline":"none" %>">
                                                <asp:Label ID="lblAssetsSelectionEnclosureSlot" runat="server" />
                                            </td>
                                            <td align="left" nowrap>
                                                <asp:Label ID="lblAssetsSelectionRoom" runat="server" />
                                            </td>
                                            <td align="left" nowrap style="display: <%=hdnAssetCategoryId.Value!="4"?"inline":"none" %>">
                                                <asp:Label ID="lblAssetsSelectionRack" runat="server" />
                                            </td>
                                        </tr>
                                    </ItemTemplate>                     
                                    <AlternatingItemTemplate>
                                        <tr bgcolor="#F6F6F6" valign="top" class="default">
                                            <td align="left">
                                                <asp:CheckBox ID="chkSelectAsset" runat="server" />
                                            </td>
                                            <td align="left">
                                                <asp:Label ID="lblAssetsSelectionSerial" runat="server" />
                                                <asp:HiddenField ID="hdnAssetId" runat="server" />
                                            </td>
                                            <td align="left">
                                                <asp:Label ID="lblAssetsSelectionAssetTag" runat="server" />
                                            </td>
                                            <td align="left">
                                                <asp:Label ID="lblAssetsSelectionAssetStatus" runat="server" />
                                            </td>
                                             <td align="left">
                                                <asp:Label ID="lblAssetsSelectionLocation" runat="server" />
                                            </td>
                                            <td align="left"  style="display: <%=hdnAssetCategoryId.Value=="1" ||hdnAssetCategoryId.Value=="2"?"inline":"none" %>">
                                                <asp:Label ID="lblAssetsSelectionClass" runat="server" />
                                            </td>
                                            <td align="left"  style="display: <%=hdnAssetCategoryId.Value=="1" ||hdnAssetCategoryId.Value=="2"?"inline":"none" %>">
                                                <asp:Label ID="lblAssetsSelectionEnvironment" runat="server"/>
                                            </td>
                                            <td align="left" nowrap  style="display: <%=hdnAssetCategoryId.Value=="2"?"inline":"none" %>">
                                                <asp:Label ID="lblAssetsSelectionEnclosure" runat="server" />
                                            </td>
                                            <td align="left" nowrap  style="display: <%=hdnAssetCategoryId.Value=="2"?"inline":"none" %>">
                                                <asp:Label ID="lblAssetsSelectionEnclosureSlot" runat="server" />
                                            </td>
                                            <td align="left" nowrap>
                                                <asp:Label ID="lblAssetsSelectionRoom" runat="server" />
                                            </td>
                                            <td align="left" nowrap style="display: <%=hdnAssetCategoryId.Value!="4"?"inline":"none" %>">
                                                <asp:Label ID="lblAssetsSelectionRack" runat="server" />
                                            </td>
                                        </tr>
                                    </AlternatingItemTemplate>
                            </asp:DataList>
            
                        </asp:Panel>
                    </td></tr>
                </table>

        </td>
    </tr>
     <tr id="trReqComment" runat="server">
        <td ><asp:Label ID="lblOrderReqComments" runat="server" CssClass="default"  Text="Comments" /></td>
        <td ><asp:TextBox ID="txtOrderReqComments" runat="server" CssClass="default" Width="500" TextMode="MultiLine" Rows="2"  />
             <asp:Button ID="btnAddOrderReqComments" runat="server" CssClass="default" Text="Add" OnClick ="btnAddOrderReqComments_Click" Visible="false"/>
        </td>
    </tr> 
     <tr id="trReqCommentList" runat="server">
        <td colspan="2" width="100%">
            <asp:Panel ID="pnlOrderReqCommentsList" runat="server" Visible="true"  Width="100%" height="100%" ScrollBars="Auto" >
                <asp:DataList ID="dlOrderReqComments" runat="server" CellPadding="3" CellSpacing="1" Width="100%" OnItemDataBound="dlOrderReqComments_ItemDataBound" >
                    <HeaderTemplate>
                        <tr bgcolor="#EEEEEE">
                            <td align="left" width="80%"><asp:Label ID="lblOrderReqHeaderComments" runat="server" CssClass="default" Text="<b>Comments</b>"  /></td>
                            <td align="left" width="10%"><asp:Label ID="lblOrderReqHeaderUpdatedBy" runat="server" CssClass="default" Text="<b>Updated By</b>"  /></td>
                            <td align="left" width="90%"><asp:Label ID="lblOrderReqHeaderLastUpdated" runat="server" CssClass="default" Text="<b>Last Updated </b>"  /></td>
                        <td align="left" width="0%"></td>
                        </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td align="left" ><asp:Label ID="lblOrderReqComments" runat="server" CssClass="default" /></td>
                            <td align="left" ><asp:Label ID="lblOrderReqUpdatedBy" runat="server" CssClass="default" /></td>
                            <td align="left" ><asp:Label ID="lblOrderReqLastUpdated" runat="server" CssClass="default" /></td>
                            <td align="left" >[<asp:LinkButton ID="lnkbtnOrderReqDelete" runat="server" OnClick="lnkbtnOrderReqDelete_Click" Text="Delete" />]</td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr bgcolor="#F6F6F6">
                            <td align="left" ><asp:Label ID="lblOrderReqComments" runat="server" CssClass="default" /></td>
                            <td align="left" ><asp:Label ID="lblOrderReqUpdatedBy" runat="server" CssClass="default" /></td>
                            <td align="left" ><asp:Label ID="lblOrderReqLastUpdated" runat="server" CssClass="default" /></td>
                            <td align="left" >[<asp:LinkButton ID="lnkbtnOrderReqDelete" runat="server" OnClick="lnkbtnOrderReqDelete_Click" Text="Delete" />]</td>
                        </tr>
                    </AlternatingItemTemplate>
                </asp:DataList>
            </asp:Panel>
        </td>
    </tr>
     <tr>
        <td colspan="2" ><asp:Label ID="lblOrderReqCommentsNoComments" runat="server" CssClass="default" Visible="false" Text="<img src='/images/spacer.gif' border='0' height='1' width='25' /><img src='/images/alert.gif' border='0' align='absmiddle'> There are no items" /></td>
     </tr>
    <tr>
        <td nowrap>&nbsp;</td>
        <td width="100%">
            <asp:Button ID="btnSave" runat="server" CssClass="default" Width="100" Text="Save" OnClick="btnSave_Click"/> &nbsp;&nbsp;&nbsp;
            <asp:Button ID="btnSubmitRequest" runat="server" CssClass="default" Width="100" Text="Submit Request" OnClick="btnSubmitRequest_Click" /> &nbsp;&nbsp;&nbsp;
            <asp:Button ID="btnUpdate" runat="server" CssClass="default" Width="100" Text="Update" OnClick="btnUpdate_Click" Visible="false"/>
        </td>
    </tr>
</table>




</asp:Panel>
<asp:Panel ID="pnlDenied" runat="server" width="100%" Visible="false">
    <br />
    <table width="100%" cellpadding="0" cellspacing="5" border="0">
        <tr width="100%">
            <td rowspan="2"><img id="imgError" src="/images/ico_error.gif" border="0" align="absmiddle"   /></td>
            <td class="header" width="100%" valign="bottom">Access Denied</td>
        </tr>
        <tr width="100%"><td width="100%" valign="top">You do not have sufficient permission to view this page.</td></tr>
        <tr><td colspan="2">&nbsp;</td></tr>
        <tr width="100%">
            <td></td>
            <td width="100%">If you think you should have rights to view it, please contact your ClearView administrator.</td>
        </tr>
    </table>
    <p>&nbsp;</p>
</asp:Panel>

<input type="hidden" id="hdnEnvironment" runat="server" />
<asp:HiddenField ID="hdnAssetCategoryId" Value ="0" runat="server" />
<asp:HiddenField ID="hdnOrderId" runat="server" />
<asp:HiddenField ID="hdnRequestId" runat="server" Value="0"/>
<asp:HiddenField ID="hdnItemId" runat="server" Value="0"/>
<asp:HiddenField ID="hdnNumber" runat="server" Value="0"/>

