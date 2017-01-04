<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="asset_order_shared_env.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.asset_order_shared_env" %>

<script type="text/javascript">

    var rblASEOrderType = null;
    var rblClusterTypes = null;
    var divFunctionsVMWARE = null;
    var divFunctionsSUN = null;
    var divFunctionsORACLE = null;
    var divFunctionsSQL = null;
    
    var trASEModel=null;
    var trASELocation=null;
    var trASEClass=null;
    var trASEEnvironment=null;
    var trASEStorageAmt=null;
    

    
    
    var trASEType= null;
    var trASEFunctions= null;
    var lblParent = null;
    var lblParentSelect= null;
    var lblSelectType= null;
    var lblAssetsSelectedCount=null;
    

    function OnTreeClick(evt) 
    { 

        var src = window.event != window.undefined ? window.event.srcElement : evt.target; 
        var isChkBoxClick = (src.tagName.toLowerCase() == "input" && src.type == "checkbox"); 
        //    alert(src.checked +" " + src.id + " " + src.name); 
        if (src.checked==true)
        {
            /* 1. Loop through all the other check box in treeview 
               2. Mark them un-checked if their ids doesn't match with the current src */
            var treeview = document.getElementById("<%=tvParent.ClientID%>");
            var inputElements = treeview.getElementsByTagName("input");
            if (treeview!=null) 
                for(i=0;i<inputElements.length;i++)
                {   if(inputElements[i].type == "checkbox" && inputElements[i].checked)
                    {  
                        //alert(" Current  : " + src.id  + "  Others "+inputElements[i].id);
                        if (inputElements[i].id !=src.id)
                            inputElements[i].checked = false;
                    }
                }   
         }

    }
    var old_onload = window.onload;
    window.onload = new function() 
    {
        addDOMLoadEvent(function() 
        {NewLoadEvent();});
        if (old_onload) old_onload();
    }; 

    
    function NewLoadEvent() {
        rblASEOrderType = document.getElementById('<%=rblASEOrderType.ClientID%>');
        rblClusterTypes = document.getElementById('<%=rblClusterTypes.ClientID%>');
    
        trASEModel = document.getElementById('<%=trASEModel.ClientID%>');
        trASELocation = document.getElementById('<%=trASELocation.ClientID%>');
        trASEClass = document.getElementById('<%=trASEClass.ClientID%>');
        trASEEnvironment = document.getElementById('<%=trASEEnvironment.ClientID%>');
        trASEStorageAmt = document.getElementById('<%=trASEStorageAmt.ClientID%>');
          
        trASEType = document.getElementById('<%=trASEType.ClientID%>');
        trASEFunctions = document.getElementById('<%=trASEFunctions.ClientID%>');
        
        divFunctionsVMWARE = document.getElementById('<%=divFunctionsVMWARE.ClientID%>');
        divFunctionsSUN = document.getElementById('<%=divFunctionsSUN.ClientID%>');
        divFunctionsORACLE = document.getElementById('<%=divFunctionsORACLE.ClientID%>');
        divFunctionsSQL = document.getElementById('<%=divFunctionsSQL.ClientID%>');
   
        lblParent = document.getElementById("<%=lblParent.ClientID%>");
        lblParentSelect = document.getElementById("<%=lblParentSelect.ClientID%>");
        lblSelectType = document.getElementById("<%=lblSelectType.ClientID%>");
        
        lblAssetsSelectedCount = document.getElementById("<%=lblAssetsSelectedCount.ClientID%>");
        
        DisplayFieldsForEnvSelection();
        DisplayFunctions() ;
     } 
    
    
   function validateTreeViewNodeSelection() 
   {
        var treeview = document.getElementById("<%=tvParent.ClientID%>");
        var inputElements = treeview.getElementsByTagName("input");
        var nodeselected=false;   
        
        if (treeview==null) 
            return true;
            
        for(i=0;i<inputElements.length;i++)
        {
            if(inputElements[i].type == "checkbox" && inputElements[i].checked)
            {   //alert('node selected');
                nodeselected =true;
            }
        }
        if (nodeselected == false)
        {
            alert('Please select parent');
            return false;
        }
        else
            return true;
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
    function DisplayFieldsForEnvSelection()//Display fields based on Env. Selection
    {
       var value = GetValue(rblASEOrderType);  // Get the selected value    
    
        trASEModel.style.display = "none";
        trASELocation.style.display = "none";
        trASEClass.style.display = "none";
        trASEEnvironment.style.display = "none";
        trASEEnvironment.style.display = "none";
        trASEType.style.display = "none";
        trASEStorageAmt.style.display = "none";
      
         var treeview = document.getElementById("<%=tvParent.ClientID%>");
         var treeviewNodes = treeview.getElementsByTagName("input");
         if (treeview!=null && treeviewNodes!=null) 
         {   
            if (treeviewNodes.length>0)
                lblParentSelect.style.display = "none";
            else
                lblParentSelect.style.display = "inline";
          }
    
         if(value == 1)  //Cluster
         {   
            trASEModel.style.display = "inline";
            trASELocation.style.display = "inline";
            trASEClass.style.display = "inline";
            trASEEnvironment.style.display = "inline";
            trASEType.style.display = "inline";
            trASEFunctions.style.display = "inline";
            lblParent.innerHTML ="Folder";
            lblParentSelect.innerHTML ="Select Folder";
        }
        else if (value == 2) //Host
        {
            trASEModel.style.display = "inline";
            trASELocation.style.display = "inline";
            trASEClass.style.display = "inline";
            trASEEnvironment.style.display = "inline";
            lblParent.innerHTML ="Cluster";
            lblParentSelect.innerHTML ="Select Cluster";
        }
         else if (value == 3) //Storage
         {
            trASEStorageAmt.style.display = "inline";
            lblParent.innerHTML ="Cluster";
            lblParentSelect.innerHTML ="Select Cluster";
         }
    }
    
    function DisplayFunctions() //Display functions based on Type Selection
    {
        var value = GetValue(rblClusterTypes);
        lblSelectType.style.display = "inline";
        
        divFunctionsVMWARE.style.display = "none";
        divFunctionsSUN.style.display = "none";
        divFunctionsORACLE.style.display = "none";
        divFunctionsSQL.style.display = "none";
        if (value != null || value>1)
           lblSelectType.style.display = "none";
            
        if (value==1) //VMWARE
            divFunctionsVMWARE.style.display = "inline";
        else if (value==2) //SUN
            divFunctionsSUN.style.display = "inline";
        else if (value==3) //ORACLE
            divFunctionsORACLE.style.display = "inline";
        else if (value==4) //SQL
             divFunctionsSQL.style.display = "inline";
    }
    
    function CheckUnCheckAll(chkAssetsSelectionSelectAll) //Check UnCheck Assets ALL
    {
       
        var pnlAssetList = document.getElementById("<%=pnlAssetsSelection.ClientID%>");
        var chkState = chkAssetsSelectionSelectAll.checked;  
        var inputElements = pnlAssetList.getElementsByTagName("input");
        var intAssetsSelectedCount=0;
        for(i=0;i<inputElements.length;i++)
        {  
            if(inputElements[i].type=='checkbox' && inputElements[i].name.indexOf('chkSelectAsset') != -1 && inputElements[i].disabled==false)
            {   
                inputElements[i].checked=chkState; 
            }
        }
        getSelectedAssetsCount();
    }
    
    function getSelectedAssetsCount() //Check UnCheck Assets
    {
        var pnlAssetList = document.getElementById("<%=pnlAssetsSelection.ClientID%>");
        var inputElements = pnlAssetList.getElementsByTagName("input");
        var intAssetsSelectedCount=0;
        for(i=0;i<inputElements.length;i++)
        {  
            if(inputElements[i].type=='checkbox' && inputElements[i].name.indexOf('chkSelectAsset') != -1 && inputElements[i].disabled==false)
            {   
                if (inputElements[i].checked==true)
                intAssetsSelectedCount=intAssetsSelectedCount+1;
            }
        }
        lblAssetsSelectedCount.innerHTML =intAssetsSelectedCount;
    }
    
    function validateAssetSelection()
    {
        if (lblAssetsSelectedCount.innerHTML=="0")
        {
         alert('Please select at least one Host Asset.');
            return false;
        }
        else
             return true;
    }
  
</script>
<script type="text/javascript" src="/javascript/treeview.js"></script>
<asp:Panel ID="pnlInfo" runat="server" Width="100%" Visible="false">
     <table width="100%" cellpadding="2" cellspacing="2" border="0">
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

<asp:Panel ID="pnlOrder" width="100%" runat="server" Visible="false">
                
<table width="100%" cellpadding="4" cellspacing="0" border="0">
    <tr>
        <td colspan="2" class="header">Order Environments(s)</td>
    </tr>
    <tr >
        <td ><asp:Label ID="lblOrderType" runat="server" CssClass="default" Text="Order Type:<font class='required'>&nbsp;*</font>" /></td>
        <td>
            <asp:RadioButtonList ID="rblASEOrderType" runat="server" CssClass="default" RepeatDirection="Horizontal">
                <asp:ListItem Value="1"  Text="Add Cluster"/>
                <asp:ListItem Value="2" Text="Add Host"/>
                <asp:ListItem Value="3" Text="Add Storage"/>
            </asp:RadioButtonList>
                                                                
         </td>
    </tr>
    <tr>
        <td ><asp:Label ID="lblDescription" runat="server" CssClass="default" Text="Description:<font class='required'>&nbsp;*</font>" /></td>
        <td><asp:TextBox ID="txtDescription" runat="server" CssClass="default" MaxLength="300" Width="300" /></td>
    </tr>
    <tr id="trASEModel" runat="server">
        <td ><asp:Label ID="lblModel" runat="server" CssClass="default" Text="Model:<font class='required'>&nbsp;*</font>" /></td>
        <td ><asp:DropDownList ID="ddlModel" runat="server" CssClass="default" Width="500"  /></td>
    </tr>
    <tr id="trASELocation" runat="server" >
        <td nowrap>
        <asp:Label ID="lblLocation" runat="server" CssClass="default" Text="Location :<font class='required'>&nbsp;*</font>" />
        </td><td width="100%"><%=strLocation %>
             <asp:HiddenField ID="hdnLocation" runat="server" Value="0" />
        </td>
    </tr>
    <tr id="trASEClass" runat="server">
        <td nowrap ><asp:Label ID="lblClass" runat="server" CssClass="default" Text="Class:<font class='required'>&nbsp;*</font>" /></td>
        <td width="100%"><asp:DropDownList ID="ddlClass" runat="server" CssClass="default" Width="300" /></td>
    </tr>
    <tr id="trASEEnvironment" runat="server">
        <td nowrap ><asp:Label ID="lblEnvironment" runat="server" CssClass="default" Text="Environment:<font class='required'>&nbsp;*</font>" /></td>
        <td width="100%"><asp:DropDownList ID="ddlEnvironment" runat="server" CssClass="default" Width="300" /></td>
    </tr>
     <tr id="trASEStorageAmt" runat="server">
        <td ><asp:Label ID="lblStorageAmt" runat="server" CssClass="default" Text="Storage Amount:<font class='required'>&nbsp;*</font>" /></td>
        <td><asp:TextBox ID="txtStorageAmt" runat="server" CssClass="default" MaxLength="20" Width="200" Text="0.00" />  GB</td>
    </tr>
    <tr>
        <td ><asp:Label ID="lblRequestedByDate" runat="server" CssClass="default"  Text="Requested By Date:<font class='required'>&nbsp;*</font>" /></td>
        <td ><asp:TextBox ID="txtRequestedByDate" runat="server" CssClass="default" Width="100" MaxLength="10" />
            <asp:ImageButton ID="imgbtnRequestedByDate" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" />
        </td>
    </tr> 
    <tr id="trASEType" runat="server" style="display:none">
        <td ><asp:Label ID="lblClusterType" runat="server" CssClass="default" Text="Cluster Type:<font class='required'>&nbsp;*</font>" /></td>
        <td>
             <asp:RadioButtonList ID="rblClusterTypes" runat="server" CssClass="default" RepeatDirection="Horizontal">
                <asp:ListItem Value="1"  Text="VMWARE"/>
                <asp:ListItem Value="2" Text="SUN"/>
                <asp:ListItem Value="3" Text="ORACLE"/>
                <asp:ListItem Value="4" Text="SQL"/>
            </asp:RadioButtonList>
         </td>
    </tr>
    <tr id="trASEFunctions" runat="server" style="display:none">
        <td ><asp:Label ID="lblFunctions" runat="server" CssClass="default" Text="Functions:<font class='required'>&nbsp;*</font>" /></td>
        <td id="tdFunctions" runat="server">
            <asp:Label ID="lblSelectType" runat="server" CssClass="note" Text="Select Type" />
            <div id="divFunctionsVMWARE" runat="server" style="display:none">
                <asp:CheckBoxList ID="chkFunctionVMWARE" runat="server" CssClass="default" RepeatColumns="5" RepeatDirection="Vertical" />
            </div>
            <div id="divFunctionsSUN" runat="server" style="display:none">
                <asp:CheckBox ID="chkFunctionContainer" runat="server" Text="Container"/>
                <asp:CheckBox ID="chkFunctionLDOM" runat="server" Text="LDOM"/>
           </div>
           <div id="divFunctionsORACLE" runat="server" style="display:none">
                <asp:CheckBox ID="chkFunctionOracle" runat="server" Text="ORACLE"/>
            </div>
            <div id="divFunctionsSQL" runat="server" style="display:none">
                <asp:CheckBox ID="chkFunctionSQL" runat="server" Text="SQL"/>
              </div>
        </td>
    </tr>
    <tr>
        <td nowrap>&nbsp;</td>
        <td width="100%">
            <asp:Button ID="btnViewSelection" runat="server" CssClass="default" Width="150" Text="View Selection Results" OnClick="btnViewSelection_Click" />
            &nbsp;&nbsp;&nbsp;
            <asp:Button ID="btnClearSelection" runat="server" CssClass="default" Width="150" Text="Clear Selection" OnClick="btnClearSelection_Click" />
            
        </td>
    </tr>
    <tr id="trASEParent" runat="server" valign="top" >
        <td ><asp:Label ID="lblParent" runat="server" CssClass="default" Text="Parent:<font class='required'>&nbsp;*</font>" /></td>
        <td>
           <div style="height:100%; width:100%; overflow:auto;">
                 <asp:Label ID="lblParentSelect" runat="server" CssClass="note" Text="Select Parent" />
                 <br />
                <asp:TreeView ID="tvParent" runat="server" CssClass="default" ShowLines="true" NodeIndent="30" >
                </asp:TreeView>
               
            </div>
         </td>
    </tr>
    
    <tr>
        <td colspan="2">&nbsp;</td>
    </tr>
    
    <tr>
        <td colspan="2">
            <asp:Panel ID="pnlAssetsAvailable" width="100%" runat="server" Visible="false">
                <table width="100%" cellpadding="4" cellspacing="0" border="1"  class="default">
                     <tr>
                        <td class="header" style="border-width:0">Hosts Asset(s) Available</td>
                        <td align="right" class="header" style="border-width:0">Total Asset(s) Selected : 
                            <asp:Label ID="lblAssetsSelectedCount" runat="server" CssClass="header" Text="0" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="border-width:1" >
                            <asp:Panel ID="pnlAssetsSelection" runat="server" Visible="true" Width="100%"  height="200" ScrollBars="Auto" >
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
                                          <td align="left" width="50%">
                                            <asp:Label ID="lblAssetsSelectionHeaderLocation" runat="server" Text="<b>Current Location</b>"  />
                                        </td>
                                        <td align="left" width="20%">
                                            <asp:Label ID="lblAssetsSelectionHeaderClass" runat="server" Text="<b>Current Class</b>"  />
                                        </td>
                                        <td align="left" width="20%">
                                            <asp:Label ID="lblAssetsSelectionHeaderEnvironment" runat="server" Text="<b>Current Environment</b>"  />
                                        </td>
                                    </tr>
                                </HeaderTemplate>
                                    <ItemTemplate>
                                    <tr class="default" valign="top">
                                        <td align="left">
                                            <asp:CheckBox ID="chkSelectAsset" runat="server" />
                                        </td>
                                        <td align="left" nowrap>
                                            <asp:Label ID="lblAssetsSelectionSerial" runat="server" />
                                            <asp:HiddenField ID="hdnAssetId" runat="server" />
                                        </td>
                                        <td align="left">
                                            <asp:Label ID="lblAssetsSelectionAssetTag" runat="server" />
                                        </td>
                                         <td align="left">
                                            <asp:Label ID="lblAssetsSelectionLocation" runat="server" />
                                        </td>
                                        <td align="left">
                                            <asp:Label ID="lblAssetsSelectionClass" runat="server" />
                                        </td>
                                        <td align="left">
                                            <asp:Label ID="lblAssetsSelectionEnvironment" runat="server" />
                                        </td>
                                    </tr>
                                </ItemTemplate>                     
                                    <AlternatingItemTemplate>
                                    <tr bgcolor="#F6F6F6" valign="top" class="default">
                                        <td align="left">
                                            <asp:CheckBox ID="chkSelectAsset" runat="server" />
                                        </td>
                                        <td align="left" >
                                            <asp:Label ID="lblAssetsSelectionSerial" runat="server" />
                                            <asp:HiddenField ID="hdnAssetId" runat="server" />
                                        </td>
                                        <td align="left">
                                            <asp:Label ID="lblAssetsSelectionAssetTag" runat="server" />
                                        </td>
                                         <td align="left">
                                            <asp:Label ID="lblAssetsSelectionLocation" runat="server" />
                                        </td>
                                        <td align="left">
                                            <asp:Label ID="lblAssetsSelectionClass" runat="server" />
                                        </td>
                                        <td align="left">
                                            <asp:Label ID="lblAssetsSelectionEnvironment" runat="server" />
                                        </td>
                                    </tr>
                                </AlternatingItemTemplate>
                                </asp:DataList> 
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </td>
    </tr>


    <tr>
        <td ></td>
        <td >
            <asp:Button ID="btnSave" runat="server" CssClass="default" Width="100" Text="Save" OnClick="btnSave_Click" /> &nbsp;&nbsp;&nbsp;
            <asp:Button ID="btnSubmitRequest" runat="server" CssClass="default" Width="100" Text="Submit Request" OnClick="btnSubmitRequest_Click" />
        </td>
    </tr>
    
   
</table>
<br />



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

<input type="hidden" id="hdnEnvironment" runat="server" Value="0"/>
 <asp:HiddenField ID="hdnParentId" runat="server" Value="0" />
<asp:HiddenField ID="hdnOrderId" runat="server" Value="0" />
<asp:HiddenField ID="hdnRequestId" runat="server" Value="0"/>
<asp:HiddenField ID="hdnItemId" runat="server" Value="0"/>
<asp:HiddenField ID="hdnNumber" runat="server" Value="0"/>


