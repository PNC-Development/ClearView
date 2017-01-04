<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wm_tpm.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.wm_tpm" %>

<script type="text/javascript">     

// Setup an array to hold our list
var AccomplishmentArray = new Array();

// This function is what rebuilds the list. It also generates the hidden form fields
// that allow you to do your server side processing with the list.
function showAccomplishments() {
	var out = '';
	
	// loop over each item in the array to create the HTML code below.
	for (i = 0; i < AccomplishmentArray.length; i++) {
		// create a link letting you remove items from the list, escape the list item, 
		// if link clicked then call removeAccomplishment to get rid of the item
		out += '<a href="javascript:removeAccomplishment(\'' + escape(AccomplishmentArray[i]) +'\')"'; 
		// Do a confirm before deleting the item
		out += 'onClick="if(confirm(\'Are you sure you want to delete this item?\')) return true; else return false;">';
		out += '(delete)</a> ';
		
		out += '<a href="javascript:editAccomplishment(\'' + escape(AccomplishmentArray[i]) +'\')">'; 		 
		out += '(edit)</a> ';
		
		// This is the hidden field that sends the list back to the server. The name of 
		// the input ends in [] so that PHP will treat it like an array of items
		out += '<input name="sAccomplishmentList[]" type="hidden" value="' + AccomplishmentArray[i] +'"> ';
		// unescape the item for display			
		out += unescape(AccomplishmentArray[i]);
		// space out the list
		// with some more code you could also make this into an actual <ul> list
		out += ' <br>';
	}	

	// Here's the magic, in the code of your page you'll have a <span> (or other element) 
	// with a unique ID. What we do here is grab that element and change the contents of 
	// it to the html we just created above which is in the out variable.
	document.getElementById('<%= AccSpan.ClientID %>').innerHTML = out;
	document.getElementById('<%= hdnAcc.ClientID %>').value = document.getElementById('<%= AccSpan.ClientID %>').innerText;
	
}

// Adds an item to the array
function addAccomplishment(Accomplishment) {
	// if the item is not an empty string
	Accomplishment = document.getElementById(Accomplishment);		
		
	if(Accomplishment.value != ''){
		// Add it to the array		
		AccomplishmentArray[AccomplishmentArray.length] = escape(Accomplishment.value);
		// Clear out the form field
		//document.categoryform.AccomplishmentInput.value = '';
		Accomplishment.value ='';
		// Redisplay the list
		showAccomplishments();
	}
	else alert('Please enter a text');
	
	return false;
}

// Remove an item from the list
function removeAccomplishment(Accomplishment) {
	// temp array
	var Accomplishments = new Array();
	// Loop over current array
	for (i = 0; i < AccomplishmentArray.length; i++) {
		// If the item doesn't match the one we're trying to delete then
		// add it to the temp array
		if (AccomplishmentArray[i] != Accomplishment) {
			Accomplishments[Accomplishments.length] = AccomplishmentArray[i];
		}
	}
	// Set the item array to our temp array
	AccomplishmentArray = Accomplishments;
	// Redisplay the list
	showAccomplishments();
}

function editAccomplishment(Accomplishment) {
	// temp array
	var Accomplishments = new Array();
	// Loop over current array
	for (i = 0; i < AccomplishmentArray.length; i++) {
		// If the item doesn't match the one we're trying to delete then
		// add it to the temp array
		if (AccomplishmentArray[i] == Accomplishment) {
			 document.getElementById('<%=txtAccomplishment.ClientID %>').value = unescape(Accomplishment);			 			  
			 document.getElementById('hdnAccomplishment').value = i;
		}		 
		Accomplishments[Accomplishments.length] = AccomplishmentArray[i];
		
	}
	// Set the item array to our temp array
	AccomplishmentArray = Accomplishments;
	// Redisplay the list
	showAccomplishments();
}

function updateAccomplishment(Accomplishment,oHidden)  
{   
   Accomplishment = document.getElementById(Accomplishment);	
   oHidden = document.getElementById(oHidden);
   if(Accomplishment.value == '') 
     alert('Nothing to update');    
   else if(oHidden.value == '') 
     alert('Item not in list');  
   else
   {    
       AccomplishmentArray[oHidden.value] = escape(Accomplishment.value);
       Accomplishment.value ='';
       showAccomplishments();
   }
   return false;
}
// This function let's people hit the enter key while in a text
// input field without the form submiting and will add the 
// item they are typing in to the list of items
function stopEnter(){
	if(document.categoryform.AccomplishmentInput.value == ''){
		return true;
	}else{
		addAccomplishment(document.categoryform.AccomplishmentInput.value);
		return false;	
	}			
}	
// ValidateChecks  -  Validates PCR(s)/CSRC(s) selection(s) for routing (CSRC/PCR Tab)
// ValidateChecks2 -  Validates Closure form for notification (Close Tab)
// ValidateChecks3 -  Validates PCR(s)/CSRC(s) selection(s) for updating status (CSRC/PCR Tab)

    function ValidateChecks(oControl,work,exec,oHidden,type)  {             
        var oControl = document.getElementsByName(oControl); 
        var valid = false;
         
        if(oControl.length == 0)
        {
           alert('There are no '+ type +'(s) to route!! Please add a '+ type +' and click on \'Route for Approval\'');
           return false;
        }
        
        for (var ii=0; ii<oControl.length; ii++) {
	    if(oControl[ii].checked)
	  	   valid = true; 
	    }   
	    if(!valid) 
	     {
	       alert('Please select atleast one '+ type +' to initiate the approval process!!');
	       return false;
	     }    
	  else
	     return OpenWindow('PCR/CSRC_APPROVALS','?work='+work+'&exec='+exec+'&checks='+document.getElementById(oHidden).value+'&route='+type);
    }
    
    function ValidateChecks2(oControl,work,exec,oHidden)
    {
       
        var oControl = document.getElementsByName(oControl); 
        var valid = false;
         
        if(oControl.length == 0)
        {
           alert('There are no PDF(s) to route!! Please select a PDF and click on \'Route for Notification\'');
           return false;
        }
        
        for (var ii=0; ii<oControl.length; ii++) {
	    if(oControl[ii].checked)	      
	  	   valid = true; 	  	 
	    }   
	    if(!valid) 
	     {
	       alert('Please select atleast one PDF to initiate the notification!!');
	       return false;
	     }    
	  else
	      return OpenWindow('PCR/CSRC_APPROVALS','?work='+work+'&exec='+exec+'&checks='+document.getElementById(oHidden).value+'&route=Closure');
      
    }
    
    function ReceiveServerData(arg, context)
    {       
        //window.top.navigate(window.top.location);
        window.top.location.replace(window.top.location);
    }

    
     function ValidateChecks3(oControl,oHidden)  {                   
        
        if(arr2.length == 0)
         {
          alert('Please select a status for the PCR(s)!!');
          return false;
          }
                
        for (var ii=0; ii<arr2.length; ii++) {     
	       CallServer(arr2[ii],''); // Client Callback script to update status of selected PCR(s). arr2 contains list of id's and statuses of PCR records    	     
	   }    
	   // return OpenWindow('PCR/CSRC_APPROVALS','?work='+work+'&exec='+exec+'&checks='+document.getElementById(oHidden).value+'&route='+type);
    }     
        
    function UpdateHidden2(oControl,oHidden) {  
     
      var oControl = document.getElementsByName(oControl);         
	  var oHidden = document.getElementById(oHidden);		 	  
	  oHidden.value ="";
	  for (var ii=0; ii<oControl.length; ii++) {
	    if(oControl[ii].checked)
	  	  oHidden.value = oHidden.value + oControl[ii].value + "+";	 		 
	  }       
	 
   }
   
  var arr2 = new Array();  
    
  Array.prototype.contains = function (element) 
  {
          for (var i = 0; i < this.length; i++) 
       {
           var id = this[i].substring(0,this[i].indexOf(':'));        
              if (id == element) 
              {
                      return true;
              }
        }
          return false;
  };  
  
  Array.prototype.find = function (element) 
  {
        var pos = -1; 
          for (var i = 0; i < this.length; i++) 
          {
           var id = this[i].substring(0,this[i].indexOf(':'));        
              if (id == element) 
              {       
                 pos = i; 
              }
          }
          return pos;
  }; 
      
      
    function UpdateHidden3(oControl,oHidden,oValue) {                 
    
      var oControl = document.getElementById(oControl);         
	  var oHidden = document.getElementById(oHidden);
	  var oStatus = document.getElementById(oStatus);	
	 
	      var strValue = oValue+ ":"+oControl.options[oControl.selectedIndex].value ;
	      oHidden.value ="";  
	      oHidden.value = oHidden.value + strValue + "+";		   
	      if(!arr2.contains(oValue))
            arr2[arr2.length] = oHidden.value; 	      
      
	      for(var ii=0; ii < arr2.length;ii++)
           {           
                var id = arr2[ii].substring(0,arr2[ii].indexOf(':'));
                var val = arr2[ii].substring(arr2[ii].indexOf(':')+1,arr2[ii].indexOf('+'));          
                if(id == oValue && val != oControl.options[oControl.selectedIndex].value)              
                   arr2.splice(ii,1,oHidden.value);                               
            }	   
	   //    alert("array result: "+arr2.sort());   	 	     	    	  
   }  
  

  
  function CalcTotal(oControl1,oControl2,oControl3,oControl4,oTotal)
  {
    var oControl1 = document.getElementById(oControl1);
    var oControl2 = document.getElementById(oControl2);
    var oControl3 = document.getElementById(oControl3);
    var oControl4 = document.getElementById(oControl4);
    var oTotal = document.getElementById(oTotal); 
    
         
    oControl1.value = trim(oControl1.value) == "" ? 0.00 :parseFloat(oControl1.value); 
    oControl2.value = trim(oControl2.value) == "" ? 0.00 :parseFloat(oControl2.value); 
    oControl3.value = trim(oControl3.value) == "" ? 0.00 :parseFloat(oControl3.value); 
    oControl4.value = trim(oControl4.value) == "" ? 0.00 :parseFloat(oControl4.value); 
    
    oTotal.value = oTotal.value == "" ? 0.00: parseFloat(oTotal.value);        
    oTotal.value = parseFloat(oControl1.value)+parseFloat(oControl2.value)+parseFloat(oControl3.value)+parseFloat(oControl4.value);    
    
  
    return false;
  }  
    
    function ChangeFrame3(oCell, oShow, oHidden, strHidden) {
        var oTable = document.getElementById("tblDivs");
        var oDIVs = oTable.getElementsByTagName("DIV");
        for(var ii=0;ii<oDIVs.length;ii++){
            if (oDIVs[ii].getAttribute("id") != "")
                oDIVs[ii].style.display = "none"
        }
        oShow = document.getElementById(oShow);
        oShow.style.display = "inline";
        var oTable = document.getElementById("tblTabs");
        var oTDs = oTable.getElementsByTagName("TD");
        for(var ii=0;ii<oTDs.length;ii++){
    		if (oTDs[ii].className == "cmbuttontop")
                oTDs[ii].style.borderTop = "1px solid #94a6b5"
    		if (oTDs[ii].className == "cmbutton")
                oTDs[ii].style.border = "1px solid #94a6b5"
        }
	    oCell.style.borderTop = "3px solid #6A8359"
        oCell.style.borderBottom = "1px solid #FFFFFF"
        oHidden = document.getElementById(oHidden);
        oHidden.value = strHidden;
    }
    function ChangeFrame2(oCell, oShow, oHide1, oHide2, oHide3, oHide4, oHide5, oHidden, strHidden) {
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
        oHide5 = document.getElementById(oHide5);
        oHide5.style.display = "none";
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
    var keepDescription = null;
    var newDescription = null;
    function DescriptionDown(oText) {
        if (keepDescription == null)
        {
            keepDescription = oText.value;
            newDescription = keepDescription;
        }
    }
    function DescriptionUp(oText) {
        if (oText.value.indexOf(keepDescription) == 0) {
            newDescription = oText.value;
        }
        else {
            alert('NOTE: You cannot change the description. You can only add additional information.');
            oText.value = newDescription;
        }
    }
    function CheckCSRC(o1,o2,o3,o4,oDiv) {
        o1 = document.getElementById(o1);
        o2 = document.getElementById(o2);
        o3 = document.getElementById(o3);
        o4 = document.getElementById(o4);
        oDiv = document.getElementById(oDiv);
        if (o1.checked == true || o2.checked == true || o3.checked == true || o4.checked == true)
            oDiv.style.display = "inline";
        else
            oDiv.style.display = "none";
    }
    function ValidateCSRC(chkD,chkP,chkE,chkC,SD,ED,ID,ExD,HD,SP,EP,IP,ExP,HP,SE,EE,IE,ExE,HE,SC,EC,IC,ExC,HC) {
        chkD = document.getElementById(chkD);
        chkP = document.getElementById(chkP);
        chkE = document.getElementById(chkE);
        chkC = document.getElementById(chkC);
        if (chkD.checked == false && chkP.checked == false && chkE.checked == false && chkC.checked == false) {
            alert('You must select at least on phase');
            return false;
        }
        else {           
            if ((chkD.checked == false || (chkD.checked == true && ValidateDate(SD,'Please enter a valid start date') && ValidateDate(ED,'Please enter a valid end date') && ValidateNumber(ID,'Please enter a valid cost') && ValidateNumber(ExD,'Please enter a valid cost') && ValidateNumber(HD,'Please enter a valid cost')))
                     && (chkP.checked == false || (chkP.checked == true && ValidateDate(SP,'Please enter a valid start date') && ValidateDate(EP,'Please enter a valid end date') && ValidateNumber(IP,'Please enter a valid cost') && ValidateNumber(ExP,'Please enter a valid cost') && ValidateNumber(HP,'Please enter a valid cost')))
                     && (chkE.checked == false || (chkE.checked == true && ValidateDate(SE,'Please enter a valid start date') && ValidateDate(EE,'Please enter a valid end date') && ValidateNumber(IE,'Please enter a valid cost') && ValidateNumber(ExE,'Please enter a valid cost') && ValidateNumber(HE,'Please enter a valid cost')))
                     && (chkC.checked == false || (chkC.checked == true && ValidateDate(SC,'Please enter a valid start date') && ValidateDate(EC,'Please enter a valid end date') && ValidateNumber(IC,'Please enter a valid cost') && ValidateNumber(ExC,'Please enter a valid cost') && ValidateNumber(HC,'Please enter a valid cost'))))
                return true;
            else
                return false;
        }
        return true;
    }
    function ValidatePCR(chkScope,chkSched,chkSD,DS,DE,chkSP,PS,PE,chkSE,ES,EE,chkSC,CS,CE,chkF,chkFD,D,chkFP,P,chkFE,E,chkFC,C) {
        chkScope = document.getElementById(chkScope);
        chkSched = document.getElementById(chkSched);
        chkF = document.getElementById(chkF);
        if (chkScope.checked == false && chkSched.checked == false && chkF.checked == false) {
            alert('Please select at least one cause for the change: Scope, Schedule and/or Financial');
            return false;
        }
        var oStop = false;
        if (chkSched.checked == true) {
            chkSD = document.getElementById(chkSD);
            chkSP = document.getElementById(chkSP);
            chkSE = document.getElementById(chkSE);
            chkSC = document.getElementById(chkSC);
            if (chkSD.checked == false && chkSP.checked == false && chkSE.checked == false && chkSC.checked == false) {
                oStop = true;
                alert('Please select at least one phase to change');
            }
            else if ((chkSD.checked == false || (chkSD.checked == true && ValidateDate(DS,'Please enter a valid start date') && ValidateDate(DE,'Please enter a valid end date')))
                    && (chkSP.checked == false || (chkSP.checked == true && ValidateDate(PS,'Please enter a valid start date') && ValidateDate(PE,'Please enter a valid end date')))
                    && (chkSE.checked == false || (chkSE.checked == true && ValidateDate(ES,'Please enter a valid start date') && ValidateDate(EE,'Please enter a valid end date')))
                    && (chkSC.checked == false || (chkSC.checked == true && ValidateDate(CS,'Please enter a valid start date') && ValidateDate(CE,'Please enter a valid end date'))))
                oStop = false;
            else
                oStop = true;
        }
        if (oStop == true)
            return false;
        if (chkF.checked == true) {
            chkFD = document.getElementById(chkFD);
            chkFP = document.getElementById(chkFP);
            chkFE = document.getElementById(chkFE);
            chkFC = document.getElementById(chkFC);
            if (chkFD.checked == false && chkFP.checked == false && chkFE.checked == false && chkFC.checked == false) {
                oStop = true;
                alert('Please select at least one phase to change');
            }
            else if ((chkFD.checked == false || (chkFD.checked == true && ValidateNumber(D,'Please enter a valid dollar amount')))
                    && (chkFP.checked == false || (chkFP.checked == true && ValidateNumber(P,'Please enter a valid dollar amount')))
                    && (chkFE.checked == false || (chkFE.checked == true && ValidateNumber(E,'Please enter a valid dollar amount')))
                    && (chkFC.checked == false || (chkFC.checked == true && ValidateNumber(C,'Please enter a valid dollar amount'))))
                oStop = false;
             else
                oStop = true;
        }
        if (oStop == true)
            return false;         
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
		oHidden.value = oHidden.value + oControl.options[ii].value + "&";
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
                        <td><asp:ImageButton ID="btnHold" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/tool_hold.gif" OnClick="btnHold_Click" /></td>
                        <td><asp:ImageButton ID="btnCancel" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/tool_cancel.gif" OnClick="btnCancel_Click" /></td>
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
                        <td><asp:ImageButton ID="btnClose" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/tool_close.gif" Visible="false" /></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="default">
                <table width="100%" border="0" cellSpacing="2" cellPadding="2" class="default">
                    <tr>
                        <td valign="top">
                            <table width="100%" border="0" cellSpacing="2" cellPadding="2" class="default">
                                <tr>
                                    <td nowrap><b>Project Name:</b></td>
                                    <td width="100%"><asp:Label ID="lblName" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td nowrap><b>Clarity Number:</b></td>
                                    <td width="100%"><asp:Label ID="lblNumber" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td nowrap><b>Sponsoring Portfolio:</b></td>
                                    <td width="100%"><asp:Label ID="lblPortfolio" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td nowrap><b>Segment:</b></td>
                                    <td width="100%"><asp:Label ID="lblSegment" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td nowrap><b>Executive Sponsor:</b></td>
                                    <td width="100%"><asp:Label ID="lblExecutive" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td nowrap><b>Working Sponsor:</b></td>
                                    <td width="100%"><asp:Label ID="lblWorking" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td nowrap><b>Submitted By:</b></td>
                                    <td width="100%"><asp:Label ID="lblSubmittedBy" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td nowrap><b>Date Submitted:</b></td>
                                    <td width="100%"><asp:Label ID="lblSubmitted" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr height="1">
                                    <td colspan="2"><img src="/images/spacer.gif" border="0" height="1" width="1" /></td>
                                </tr>
                                <tr>
                                    <td colspan="2"><asp:LinkButton ID="btnView" runat="server" /></td>
                                </tr>
                            </table>
                        </td>
                        <td valign="top">
                            <table width="100%" border="0" cellSpacing="2" cellPadding="2" class="default">
                                <tr>
                                    <td nowrap><b>Approved End Date:</b></td>
                                    <td width="100%"><asp:Label ID="lblEndDate" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td nowrap><b>Total Approved:</b></td>
                                    <td width="100%">$<asp:Label ID="lblTotalApproved" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td nowrap><b>Total Actual:</b></td>
                                    <td width="100%">$<asp:Label ID="lblTotalActual" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td nowrap><b>Variance:</b></td>
                                    <td width="100%">$<asp:Label ID="lblTotalVariance" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td nowrap><b>Project Status:</b></td>
                                    <td width="100%"><asp:Label ID="lblProjectStatus" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td nowrap><b>Overall Priority:</b></td>
                                    <td width="100%"><asp:Label ID="lblPriority" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td nowrap><b>Overall Status:</b></td>
                                    <td width="100%"><asp:Label ID="lblStatus" runat="server" CssClass="default" /></td>
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
            <td>
                <table id="tblDivs" width="100%" height="100%" border="0" cellSpacing="0" cellPadding="0" class="default">
                    <tr>
                        <td>
                            <table id="tblTabs" width="100%" border="0" cellspacing="0" cellpadding="0" class="default">
                                <tr>
                                    <td>
                                        <table width="100%" border="0" cellspacing="0" cellpadding="0" class="default">
                                            <tr>
                                                <td class="cmbuttontop" style='<%=boolDetails == true ? "display:inline;border-bottom:1px solid #FFFFFF;border-top:3px solid #6A8359;" : "" %>' onclick="ChangeFrame3(this,'divDetails','<%=hdnTab.ClientID %>','D');">Project Details</td>
                                                <td class="cmbuttonspacetop">&nbsp;</td>
                                                <td class="cmbuttontop" style='<%=boolFinancials == true ? "display:inline;border-bottom:1px solid #FFFFFF;border-top:3px solid #6A8359;" : "" %>' onclick="ChangeFrame3(this,'divFinancials','<%=hdnTab.ClientID %>','F');">Financials / Schedule</td>
                                                <td class="cmbuttonspacetop">&nbsp;</td>
                                                <td class="cmbuttontop" style='<%=boolClose == true ? "display:inline;border-bottom:1px solid #FFFFFF;border-top:3px solid #6A8359;" : "" %>' onclick="ChangeFrame3(this,'divClose','<%=hdnTab.ClientID %>','C');">Close</td>
<!--
                                                <td class="cmbuttontopdisabled">Close</td>
-->
                                                <td class="cmbuttonspacetop">&nbsp;</td>
                                                <td class="cmbuttontop" style='<%=boolStatus == true ? "display:inline;border-bottom:1px solid #FFFFFF;border-top:3px solid #6A8359;" : "" %>' onclick="ChangeFrame3(this,'divStatus','<%=hdnTab.ClientID %>','S');">Status Updates</td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table width="100%" border="0" cellspacing="0" cellpadding="0" class="default">
                                            <tr>
                                                <td class="cmbuttonspaceleft">&nbsp;</td>
<!--
                                                <td class="cmbuttondisabled">CSRC/PCR Information</td>
-->
                                                <td class="cmbutton" style='<%=boolInformation == true ? "display:inline;border-bottom:1px solid #FFFFFF;border-top:3px solid #6A8359;" : "" %>' onclick="ChangeFrame3(this,'divInformation','<%=hdnTab.ClientID %>','I');">CSRC/PCR Information</td>
                                                <td class="cmbuttonspace">&nbsp;</td>
                                                <td class="cmbutton" style='<%=boolMilestones == true ? "display:inline;border-bottom:1px solid #FFFFFF;border-top:3px solid #6A8359;" : "" %>' onclick="ChangeFrame3(this,'divMilestones','<%=hdnTab.ClientID %>','L');">Milestones</td>
                                                <td class="cmbuttonspace">&nbsp;</td>
                                                <td class="cmbutton" style='<%=boolResource == true ? "display:inline;border-bottom:1px solid #FFFFFF;border-top:3px solid #6A8359;" : "" %>' onclick="ChangeFrame3(this,'divInvolvement','<%=hdnTab.ClientID %>','R');">Resource Involvement</td>
                                                <td class="cmbuttonspaceright">&nbsp;</td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" class="cmcontents">
                            <table width="100%" height="100%" border="0" cellSpacing="2" cellPadding="2" class="default">
                                <tr> 
                                    <td valign="top">
		                                    <div id="divDetails" style='<%=boolDetails == true ? "display:inline" : "display:none" %>'>
		                                        <br /><br /><br />
                                                <table width="100%" cellpadding="3" cellspacing="2" border="0" class="default">
                                                    <tr>
                                                        <td nowrap style="color:#404040"><b>Project Name:</b></td>
                                                        <td width="100%"><asp:TextBox ID="txtProjectName1" runat="server" CssClass="default" Width="300" MaxLength="35" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap style="color:#404040"><b>Nickname:</b></td>
                                                        <td width="100%"><asp:TextBox ID="txtCustomName" runat="server" CssClass="default" Width="300" MaxLength="100" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap style="color:#404040"><b>Clarity Number:</b></td>
                                                        <td width="100%"><asp:TextBox ID="txtProjectNumber" runat="server" CssClass="default" Width="100" MaxLength="12" /> <img src="/images/question_red.gif" border="0" align="absmiddle" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap><b>Executive Sponsor:</b></td>
                                                        <td width="100%">
                                                            <table cellpadding="0" cellspacing="0" border="0">
                                                                <tr>
                                                                    <td><asp:TextBox ID="txtExecutive" runat="server" Width="300" CssClass="default" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <div id="divExecutive" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                                                                            <asp:ListBox ID="lstExecutive" runat="server" CssClass="default" />
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap><b>Working Sponsor:</b></td>
                                                        <td width="100%">
                                                            <table cellpadding="0" cellspacing="0" border="0">
                                                                <tr>
                                                                    <td><asp:TextBox ID="txtWorking" runat="server" Width="300" CssClass="default" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <div id="divWorking" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                                                                            <asp:ListBox ID="lstWorking" runat="server" CssClass="default" />
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap><b>Sponsoring Portfolio:</b></td>
                                                        <td width="100%"><asp:DropDownList ID="ddlPortfolio" runat="server" CssClass="default" Width="200" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap><b>Segment:</b></td>
                                                        <td width="100%">
                                                            <asp:DropDownList ID="ddlSegment" CssClass="default" runat="server" Width="300" Enabled="false" >
                                                                <asp:ListItem Value="-- Please select a Sponsoring Portfolio --" />
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap valign="top"><b>Project Description:</b></td>
                                                        <td width="100%"><asp:TextBox ID="txtDescription" runat="server" CssClass="default" Width="400" TextMode="MultiLine" Rows="7" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap><b>Date TPM/PC Assigned:</b></td>
                                                        <td width="100%"><asp:Label ID="lblAssigned" runat="server" CssClass="default" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap><b>Estimated Start Date:</b></td>
                                                        <td width="100%"><asp:Label ID="lblStart" runat="server" CssClass="default" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap><b>Estimated End Date:</b></td>
                                                        <td width="100%"><asp:Label ID="lblEnd" runat="server" CssClass="default" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="top"><br /><b>Cost Center(s):</b></td>
                                                        <td>
                                                            <table cellpadding="2" cellspacing="0" border="0">
                                                                <tr>
                                                                    <td class="default">Selected:</td>
                                                                    <td class="default" colspan="3">&nbsp;</td>
                                                                    <td class="default">Available:</td>
                                                                </tr>
                                                                <tr>
                                                                    <td><asp:ListBox ID="lstCostsCurrent" runat="server" Width="175" CssClass="default" Rows="5" /></td>
                                                                    <td>&nbsp;</td>
                                                                    <td>
                                                                        <asp:ImageButton ID="btnCostAdd" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/lt.gif" ToolTip="Add" /><br /><br />
                                                                        <asp:ImageButton ID="btnCostRemove" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/rt.gif" ToolTip="Remove" />
                                                                    </td>
                                                                    <td>&nbsp;</td>
                                                                    <td><asp:ListBox ID="lstCostsAvailable" runat="server" Width="175" CssClass="default" Rows="5" /></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap><b>Project Central SharePoint URL:</b></td>
                                                        <td width="100%">
                                                            <table cellpadding="0" cellspacing="0" border="0">
                                                                <tr>
                                                                    <td><asp:TextBox ID="txtSharepoint" runat="server" CssClass="default" Width="400" /></td>
                                                                    <td>&nbsp;</td>
                                                                    <td><asp:HyperLink ID="hypSharepoint" runat="server" ImageUrl="/images/search.gif" Target="_blank" /></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" class="hugeheader">My Documents</td>
                                                    </tr>
                                                    <tr>
                                                        <td><asp:Button ID="btnDocuments" runat="server" Text="Upload" Width="75" CssClass="default" /></td>
                                                        <td align="right"><asp:CheckBox ID="chkMyDescription" runat="server" CssClass="default" Text="Show Descriptions" AutoPostBack="true" OnCheckedChanged="chkMyDescription_Change" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2"><asp:Label ID="lblMyDocuments" runat="server" CssClass="default" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" class="hugeheader">Project Documents</td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right" colspan="2"><asp:CheckBox ID="chkDescription" runat="server" CssClass="default" Text="Show Descriptions" AutoPostBack="true" OnCheckedChanged="chkDescription_Change" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2"><asp:Label ID="lblDocuments" runat="server" CssClass="default" /></td>
                                                    </tr>
                                                </table>
		                                    </div>
		                                    <div id="divFinancials" style='<%=boolFinancials == true ? "display:inline" : "display:none" %>'>
		                                        <br /><br /><br />
                                                <table width="100%" cellpadding="5" cellspacing="0" border="0">
                                                    <tr>
                                                        <td nowrap><b>Project Name:</b></td>
                                                        <td width="100%"><asp:TextBox ID="txtProjectName2" runat="server" CssClass="default" Width="300" MaxLength="35" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap><b>Project Status:</b></td>
                                                        <td width="100%">
                                                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="default" Width="100">
                                                                <asp:ListItem Value="2" Text="Active" />
                                                                <asp:ListItem Value="5" Text="Hold" />
                                                                <asp:ListItem Value="-2" Text="Cancelled" />
                                                                <asp:ListItem Value="3" Text="Closed" />
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap><b>Current PMM Phase:</b></td>
                                                        <td width="100%">
                                                            <asp:DropDownList ID="ddlPPM" runat="server" CssClass="default" Width="100">
                                                                <asp:ListItem Value="Idea" />
                                                                <asp:ListItem Value="Discovery" />
                                                                <asp:ListItem Value="Planning" />
                                                                <asp:ListItem Value="Execution" />
                                                                <asp:ListItem Value="Closing" />
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <table cellpadding="0" cellspacing="0" border="0">
                                                                <tr>
                                                                    <td class="hugeheader">Project Schedule:</td>
                                                                    <td>&nbsp;&nbsp;&nbsp;</td>
                                                                    <td>[<a href="/help/ScheduleFinancialsFormula.pdf" target="_blank">Schedule and Financials Formula</a>]</td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>                                                     
                                                    <tr>
                                                        <td colspan="2">
                                                            <table width="90%" cellpadding="5" cellspacing="0" border="0" style="display:inline">
                                                                <tr>
                                                                    <td bgcolor="#E9E9E9" style="border-bottom:solid 1px #999999;border-top:solid 1px #999999;border-left:solid 1px #999999">&nbsp;</td>
                                                                    <td align="center" colspan="3" bgcolor="#E9E9E9" style="border-bottom:solid 1px #999999;border-top:solid 1px #999999"><b>Approved C1 and PCRs Dates</b></td>
                                                                    <td align="center" colspan="3" bgcolor="#E9E9E9" style="border-bottom:solid 1px #999999;border-top:solid 1px #999999"><b>Actual/Forecast Dates</b></td>
                                                                    <td align="center" rowspan="2" bgcolor="#E9E9E9" class="default" style="border-top:solid 1px #999999"><b>Variance<br /># Days</b></td>
                                                                    <td align="center" rowspan="2" bgcolor="#E9E9E9" class="default" style="border-top:solid 1px #999999;border-right:solid 1px #999999"><b>Variance<br />%</b></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>&nbsp;</td>
                                                                    <td align="center" class="greenheader"><b>Start Date</b></td>
                                                                    <td>&nbsp;</td>
                                                                    <td align="center" class="greenheader"><b>End Date</b></td>
                                                                    <td align="center" class="greenheader"><b>Start Date</b></td>
                                                                    <td>&nbsp;</td>
                                                                    <td align="center" class="greenheader" style="border-right:solid 1px #999999"><b>End Date</b></td>
                                                                </tr>
                                                                <tr>
                                                                    <td nowrap>Discovery Phase:</td>
                                                                    <td align="center" nowrap><asp:TextBox ID="txtAppSD" runat="server" CssClass="default" Width="80" MaxLength="10" /> <asp:ImageButton ID="imgtxtAppSD" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
                                                                    <td align="center" width="1">-</td>
                                                                    <td align="center"><asp:TextBox ID="txtAppED" runat="server" CssClass="default" Width="80" MaxLength="10" /> <asp:ImageButton ID="imgtxtAppED" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
                                                                    <td align="center" nowrap><asp:TextBox ID="txtStartD" runat="server" CssClass="default" Width="80" MaxLength="10" /> <asp:ImageButton ID="imgtxtStartD" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
                                                                    <td align="center" width="1">-</td>
                                                                    <td align="center" style="border-right:solid 1px #999999"><asp:TextBox ID="txtEndD" runat="server" CssClass="default" Width="80" MaxLength="10" /> <asp:ImageButton ID="imgtxtEndD" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
                                                                    <td bgcolor="#E9E9E9" align="center"><asp:Label ID="lblVarianceDaysD" runat="server" CssClass="default" /></td>
                                                                    <td bgcolor="#E9E9E9" align="center" style="border-right:solid 1px #999999"><asp:Label ID="lblVariancePercentD" runat="server" CssClass="default" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td nowrap>Planning Phase:</td>
                                                                    <td align="center" nowrap><asp:TextBox ID="txtAppSP" runat="server" CssClass="default" Width="80" MaxLength="10" /> <asp:ImageButton ID="imgtxtAppSP" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
                                                                    <td align="center" width="1">-</td>
                                                                    <td align="center"><asp:TextBox ID="txtAppEP" runat="server" CssClass="default" Width="80" MaxLength="10" /> <asp:ImageButton ID="imgtxtAppEP" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
                                                                    <td align="center" nowrap><asp:TextBox ID="txtStartP" runat="server" CssClass="default" Width="80" MaxLength="10" /> <asp:ImageButton ID="imgtxtStartP" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
                                                                    <td align="center" width="1">-</td>
                                                                    <td align="center" style="border-right:solid 1px #999999"><asp:TextBox ID="txtEndP" runat="server" CssClass="default" Width="80" MaxLength="10" /> <asp:ImageButton ID="imgtxtEndP" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
                                                                    <td bgcolor="#E9E9E9" align="center"><asp:Label ID="lblVarianceDaysP" runat="server" CssClass="default" /></td>
                                                                    <td bgcolor="#E9E9E9" align="center" style="border-right:solid 1px #999999"><asp:Label ID="lblVariancePercentP" runat="server" CssClass="default" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td nowrap>Execution Phase:</td>
                                                                    <td align="center" nowrap><asp:TextBox ID="txtAppSE" runat="server" CssClass="default" Width="80" MaxLength="10" /> <asp:ImageButton ID="imgtxtAppSE" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
                                                                    <td align="center" width="1">-</td>
                                                                    <td align="center"><asp:TextBox ID="txtAppEE" runat="server" CssClass="default" Width="80" MaxLength="10" /> <asp:ImageButton ID="imgtxtAppEE" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
                                                                    <td align="center" nowrap><asp:TextBox ID="txtStartE" runat="server" CssClass="default" Width="80" MaxLength="10" /> <asp:ImageButton ID="imgtxtStartE" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
                                                                    <td align="center" width="1">-</td>
                                                                    <td align="center" style="border-right:solid 1px #999999"><asp:TextBox ID="txtEndE" runat="server" CssClass="default" Width="80" MaxLength="10" /> <asp:ImageButton ID="imgtxtEndE" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
                                                                    <td bgcolor="#E9E9E9" align="center"><asp:Label ID="lblVarianceDaysE" runat="server" CssClass="default" /></td>
                                                                    <td bgcolor="#E9E9E9" align="center" style="border-right:solid 1px #999999"><asp:Label ID="lblVariancePercentE" runat="server" CssClass="default" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td nowrap>Close Phase:</td>
                                                                    <td align="center" nowrap><asp:TextBox ID="txtAppSC" runat="server" CssClass="default" Width="80" MaxLength="10" /> <asp:ImageButton ID="imgtxtAppSC" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
                                                                    <td align="center" width="1">-</td>
                                                                    <td align="center"><asp:TextBox ID="txtAppEC" runat="server" CssClass="default" Width="80" MaxLength="10" /> <asp:ImageButton ID="imgtxtAppEC" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
                                                                    <td align="center" nowrap><asp:TextBox ID="txtStartC" runat="server" CssClass="default" Width="80" MaxLength="10" /> <asp:ImageButton ID="imgtxtStartC" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
                                                                    <td align="center" width="1">-</td>
                                                                    <td align="center" style="border-right:solid 1px #999999"><asp:TextBox ID="txtEndC" runat="server" CssClass="default" Width="80" MaxLength="10" /> <asp:ImageButton ID="imgtxtEndC" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
                                                                    <td bgcolor="#E9E9E9" align="center"><asp:Label ID="lblVarianceDaysC" runat="server" CssClass="default" /></td>
                                                                    <td bgcolor="#E9E9E9" align="center" style="border-right:solid 1px #999999"><asp:Label ID="lblVariancePercentC" runat="server" CssClass="default" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td bgcolor="#E9E9E9" style="border-bottom:solid 1px #999999;border-top:solid 1px #999999;border-left:solid 1px #999999" class="default"><b>PROJECT TOTALS:</b></td>
                                                                    <td bgcolor="#E9E9E9" align="center" style="border-top:solid 1px #999999;border-bottom:solid 1px #999999"><asp:Label ID="lblApprovedStart" runat="server" CssClass="defaultbold" /></td>
                                                                    <td bgcolor="#E9E9E9" align="center" style="border-top:solid 1px #999999;border-bottom:solid 1px #999999">-</td>
                                                                    <td bgcolor="#E9E9E9" align="center" style="border-top:solid 1px #999999;border-bottom:solid 1px #999999"><asp:Label ID="lblApprovedEnd" runat="server" CssClass="defaultbold" /></td>
                                                                    <td bgcolor="#E9E9E9" align="center" style="border-top:solid 1px #999999;border-bottom:solid 1px #999999"><asp:Label ID="lblProjectStart" runat="server" CssClass="defaultbold" /></td>
                                                                    <td bgcolor="#E9E9E9" align="center" style="border-top:solid 1px #999999;border-bottom:solid 1px #999999">-</td>
                                                                    <td bgcolor="#E9E9E9" align="center" style="border-top:solid 1px #999999;border-bottom:solid 1px #999999"><asp:Label ID="lblProjectEnd" runat="server" CssClass="defaultbold" /></td>
                                                                    <td bgcolor="#E9E9E9" align="center" style="border-bottom:solid 1px #999999"><asp:Label ID="lblVarianceDays" runat="server" CssClass="defaultbold" /></td>
                                                                    <td bgcolor="#E9E9E9" align="center" style="border-bottom:solid 1px #999999;border-right:solid 1px #999999"><asp:Label ID="lblVariancePercent" runat="server" CssClass="defaultbold" /></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                                                <tr>
                                                                    <td class="hugeheader">Project Financials:</td>
                                                                    <td align="right"><asp:CheckBox ID="chkFinancials" runat="server" CssClass="default" Text="Exclude Financial Information" /></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <table width="100%" cellpadding="5" cellspacing="0" border="0">
                                                                <tr>
                                                                    <td align="left" class="header"><b>Discovery:</b></td>
                                                                    <td align="center" class="greenheader"><b>Approved C1 & PCRs</b></td>
                                                                    <td align="center" class="greenheader"><b>Actuals to Date</b></td>
                                                                    <td align="center" class="greenheader"><b>Estimate to Complete</b></td>
                                                                    <td bgcolor="#E9E9E9" class="default" align="center" style="border-left:solid 1px #999999;border-top:solid 1px #999999"><b>Current Forecast</b></td>
                                                                    <td bgcolor="#E9E9E9" class="default" align="center" style="border-top:solid 1px #999999"><b>Variance $</b></td>
                                                                    <td bgcolor="#E9E9E9" class="default" align="center" style="border-top:solid 1px #999999;border-right:solid 1px #999999"><b>Variance %</b></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>Internal Labor:</td>
                                                                    <td align="center"><asp:TextBox ID="txtAppID" runat="server" CssClass="default" Width="80" MaxLength="16" /> <asp:ImageButton ID="imgtxtAppID" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calculator.gif" /></td>
                                                                    <td align="center"><asp:TextBox ID="txtActID" runat="server" CssClass="default" Width="80" MaxLength="16" /> <asp:ImageButton ID="imgtxtActID" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calculator.gif" /></td>
                                                                    <td align="center"><asp:TextBox ID="txtEstID" runat="server" CssClass="default" Width="80" MaxLength="16" /> <asp:ImageButton ID="imgtxtEstID" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calculator.gif" /></td>
                                                                    <td bgcolor="#E9E9E9" align="center" style="border-left:solid 1px #999999"><asp:Label ID="lblForeDI" runat="server" CssClass="default" /></td>
                                                                    <td bgcolor="#E9E9E9" align="center"><asp:Label ID="lblVarDDI" runat="server" CssClass="default" /></td>
                                                                    <td bgcolor="#E9E9E9" align="center" style="border-right:solid 1px #999999"><asp:Label ID="lblVarPDI" runat="server" CssClass="default" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>External Labor:</td>
                                                                    <td align="center"><asp:TextBox ID="txtAppExD" runat="server" CssClass="default" Width="80" MaxLength="16" /> <asp:ImageButton ID="imgtxtAppExD" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calculator.gif" /></td>
                                                                    <td align="center"><asp:TextBox ID="txtActED" runat="server" CssClass="default" Width="80" MaxLength="16" /> <asp:ImageButton ID="imgtxtActED" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calculator.gif" /></td>
                                                                    <td align="center"><asp:TextBox ID="txtEstED" runat="server" CssClass="default" Width="80" MaxLength="16" /> <asp:ImageButton ID="imgtxtEstED" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calculator.gif" /></td>
                                                                    <td bgcolor="#E9E9E9" align="center" style="border-left:solid 1px #999999"><asp:Label ID="lblForeDE" runat="server" CssClass="default" /></td>
                                                                    <td bgcolor="#E9E9E9" align="center"><asp:Label ID="lblVarDDE" runat="server" CssClass="default" /></td>
                                                                    <td bgcolor="#E9E9E9" align="center" style="border-right:solid 1px #999999"><asp:Label ID="lblVarPDE" runat="server" CssClass="default" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>HW/SW/One Time Cost:</td>
                                                                    <td align="center"><asp:TextBox ID="txtAppHD" runat="server" CssClass="default" Width="80" MaxLength="16" /> <asp:ImageButton ID="imgtxtAppHD" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calculator.gif" /></td>
                                                                    <td align="center"><asp:TextBox ID="txtActHD" runat="server" CssClass="default" Width="80" MaxLength="16" /> <asp:ImageButton ID="imgtxtActHD" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calculator.gif" /></td>
                                                                    <td align="center"><asp:TextBox ID="txtEstHD" runat="server" CssClass="default" Width="80" MaxLength="16" /> <asp:ImageButton ID="imgtxtEstHD" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calculator.gif" /></td>
                                                                    <td bgcolor="#E9E9E9" align="center" style="border-left:solid 1px #999999"><asp:Label ID="lblForeDH" runat="server" CssClass="default" /></td>
                                                                    <td bgcolor="#E9E9E9" align="center"><asp:Label ID="lblVarDDH" runat="server" CssClass="default" /></td>
                                                                    <td bgcolor="#E9E9E9" align="center" style="border-right:solid 1px #999999"><asp:Label ID="lblVarPDH" runat="server" CssClass="default" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td bgcolor="#E9E9E9" style="border-top:solid 1px #999999;border-left:solid 1px #999999" class="default"><b>PHASE TOTALS:</b></td>
                                                                    <td bgcolor="#E9E9E9" align="center" style="border-top:solid 1px #999999"><asp:Label ID="lblAppD" runat="server" CssClass="defaultbold" /></td>
                                                                    <td bgcolor="#E9E9E9" align="center" style="border-top:solid 1px #999999"><asp:Label ID="lblActD" runat="server" CssClass="defaultbold" /></td>
                                                                    <td bgcolor="#E9E9E9" align="center" style="border-top:solid 1px #999999"><asp:Label ID="lblEstimateD" runat="server" CssClass="defaultbold" /></td>
                                                                    <td bgcolor="#E9E9E9" align="center"><asp:Label ID="lblForeD" runat="server" CssClass="defaultbold" /></td>
                                                                    <td bgcolor="#E9E9E9" align="center"><asp:Label ID="lblVarDD" runat="server" CssClass="defaultbold" /></td>
                                                                    <td bgcolor="#E9E9E9" align="center" style="border-right:solid 1px #999999"><asp:Label ID="lblVarPD" runat="server" CssClass="defaultbold" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="4" style="border-top:solid 1px #999999"><img src="/images/spacer.gif" border="0" width="1" height="1" /></td>
                                                                    <td colspan="3" style="border-bottom:solid 1px #999999;border-top:solid 1px #999999"><img src="/images/spacer.gif" border="0" width="1" height="1" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" class="header"><b>Planning:</b></td>
                                                                    <td align="center" class="greenheader"><b>Approved C1 & PCRs</b></td>
                                                                    <td align="center" class="greenheader"><b>Actuals to Date</b></td>
                                                                    <td align="center" class="greenheader"><b>Estimate to Complete</b></td>
                                                                    <td bgcolor="#E9E9E9" class="default" align="center" style="border-left:solid 1px #999999"><b>Current Forecast</b></td>
                                                                    <td bgcolor="#E9E9E9" class="default" align="center"><b>Variance $</b></td>
                                                                    <td bgcolor="#E9E9E9" class="default" align="center" style="border-right:solid 1px #999999"><b>Variance %</b></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>Internal Labor:</td>
                                                                    <td align="center"><asp:TextBox ID="txtAppIP" runat="server" CssClass="default" Width="80" MaxLength="16" /> <asp:ImageButton ID="imgtxtAppIP" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calculator.gif" /></td>
                                                                    <td align="center"><asp:TextBox ID="txtActIP" runat="server" CssClass="default" Width="80" MaxLength="16" /> <asp:ImageButton ID="imgtxtActIP" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calculator.gif" /></td>
                                                                    <td align="center"><asp:TextBox ID="txtEstIP" runat="server" CssClass="default" Width="80" MaxLength="16" /> <asp:ImageButton ID="imgtxtEstIP" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calculator.gif" /></td>
                                                                    <td bgcolor="#E9E9E9" align="center" style="border-left:solid 1px #999999"><asp:Label ID="lblForePI" runat="server" CssClass="default" /></td>
                                                                    <td bgcolor="#E9E9E9" align="center"><asp:Label ID="lblVarDPI" runat="server" CssClass="default" /></td>
                                                                    <td bgcolor="#E9E9E9" align="center" style="border-right:solid 1px #999999"><asp:Label ID="lblVarPPI" runat="server" CssClass="default" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>External Labor:</td>
                                                                    <td align="center"><asp:TextBox ID="txtAppExp" runat="server" CssClass="default" Width="80" MaxLength="16" /> <asp:ImageButton ID="imgtxtAppExp" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calculator.gif" /></td>
                                                                    <td align="center"><asp:TextBox ID="txtActEP" runat="server" CssClass="default" Width="80" MaxLength="16" /> <asp:ImageButton ID="imgtxtActEP" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calculator.gif" /></td>
                                                                    <td align="center"><asp:TextBox ID="txtEstEP" runat="server" CssClass="default" Width="80" MaxLength="16" /> <asp:ImageButton ID="imgtxtEstEP" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calculator.gif" /></td>
                                                                    <td bgcolor="#E9E9E9" align="center" style="border-left:solid 1px #999999"><asp:Label ID="lblForePE" runat="server" CssClass="default" /></td>
                                                                    <td bgcolor="#E9E9E9" align="center"><asp:Label ID="lblVarDPE" runat="server" CssClass="default" /></td>
                                                                    <td bgcolor="#E9E9E9" align="center" style="border-right:solid 1px #999999"><asp:Label ID="lblVarPPE" runat="server" CssClass="default" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>HW/SW/One Time Cost:</td>
                                                                    <td align="center"><asp:TextBox ID="txtAppHP" runat="server" CssClass="default" Width="80" MaxLength="16" /> <asp:ImageButton ID="imgtxtAppHP" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calculator.gif" /></td>
                                                                    <td align="center"><asp:TextBox ID="txtActHP" runat="server" CssClass="default" Width="80" MaxLength="16" /> <asp:ImageButton ID="imgtxtActHP" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calculator.gif" /></td>
                                                                    <td align="center"><asp:TextBox ID="txtEstHP" runat="server" CssClass="default" Width="80" MaxLength="16" /> <asp:ImageButton ID="imgtxtEstHP" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calculator.gif" /></td>
                                                                    <td bgcolor="#E9E9E9" align="center" style="border-left:solid 1px #999999"><asp:Label ID="lblForePH" runat="server" CssClass="default" /></td>
                                                                    <td bgcolor="#E9E9E9" align="center"><asp:Label ID="lblVarDPH" runat="server" CssClass="default" /></td>
                                                                    <td bgcolor="#E9E9E9" align="center" style="border-right:solid 1px #999999"><asp:Label ID="lblVarPPH" runat="server" CssClass="default" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td bgcolor="#E9E9E9" style="border-top:solid 1px #999999;border-left:solid 1px #999999" class="default"><b>PHASE TOTALS:</b></td>
                                                                    <td bgcolor="#E9E9E9" align="center" style="border-top:solid 1px #999999"><asp:Label ID="lblAppP" runat="server" CssClass="defaultbold" /></td>
                                                                    <td bgcolor="#E9E9E9" align="center" style="border-top:solid 1px #999999"><asp:Label ID="lblActP" runat="server" CssClass="defaultbold" /></td>
                                                                    <td bgcolor="#E9E9E9" align="center" style="border-top:solid 1px #999999"><asp:Label ID="lblEstimateP" runat="server" CssClass="defaultbold" /></td>
                                                                    <td bgcolor="#E9E9E9" align="center"><asp:Label ID="lblForeP" runat="server" CssClass="defaultbold" /></td>
                                                                    <td bgcolor="#E9E9E9" align="center"><asp:Label ID="lblVarDP" runat="server" CssClass="defaultbold" /></td>
                                                                    <td bgcolor="#E9E9E9" align="center" style="border-right:solid 1px #999999"><asp:Label ID="lblVarPP" runat="server" CssClass="defaultbold" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="4" style="border-top:solid 1px #999999"><img src="/images/spacer.gif" border="0" width="1" height="1" /></td>
                                                                    <td colspan="3" style="border-bottom:solid 1px #999999;border-top:solid 1px #999999"><img src="/images/spacer.gif" border="0" width="1" height="1" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" class="header"><b>Execution:</b></td>
                                                                    <td align="center" class="greenheader"><b>Approved C1 & PCRs</b></td>
                                                                    <td align="center" class="greenheader"><b>Actuals to Date</b></td>
                                                                    <td align="center" class="greenheader"><b>Estimate to Complete</b></td>
                                                                    <td bgcolor="#E9E9E9" class="default" align="center" style="border-left:solid 1px #999999"><b>Current Forecast</b></td>
                                                                    <td bgcolor="#E9E9E9" class="default" align="center"><b>Variance $</b></td>
                                                                    <td bgcolor="#E9E9E9" style="border-right:solid 1px #999999" class="default" align="center"><b>Variance %</b></td>
                                                                </tr>
                                                               <tr>
                                                                    <td>Internal Labor:</td>
                                                                    <td align="center"><asp:TextBox ID="txtAppIE" runat="server" CssClass="default" Width="80" MaxLength="16" /> <asp:ImageButton ID="imgtxtAppIE" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calculator.gif" /></td>
                                                                    <td align="center"><asp:TextBox ID="txtActIE" runat="server" CssClass="default" Width="80" MaxLength="16" /> <asp:ImageButton ID="imgtxtActIE" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calculator.gif" /></td>
                                                                    <td align="center"><asp:TextBox ID="txtEstIE" runat="server" CssClass="default" Width="80" MaxLength="16" /> <asp:ImageButton ID="imgtxtEstIE" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calculator.gif" /></td>
                                                                    <td bgcolor="#E9E9E9" align="center" style="border-left:solid 1px #999999"><asp:Label ID="lblForeEI" runat="server" CssClass="default" /></td>
                                                                    <td bgcolor="#E9E9E9" align="center"><asp:Label ID="lblVarDEI" runat="server" CssClass="default" /></td>
                                                                    <td bgcolor="#E9E9E9" align="center" style="border-right:solid 1px #999999"><asp:Label ID="lblVarPEI" runat="server" CssClass="default" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>External Labor:</td>
                                                                    <td align="center"><asp:TextBox ID="txtAppExE" runat="server" CssClass="default" Width="80" MaxLength="16" /> <asp:ImageButton ID="imgtxtAppExE" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calculator.gif" /></td>
                                                                    <td align="center"><asp:TextBox ID="txtActEE" runat="server" CssClass="default" Width="80" MaxLength="16" /> <asp:ImageButton ID="imgtxtActEE" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calculator.gif" /></td>
                                                                    <td align="center"><asp:TextBox ID="txtEstEE" runat="server" CssClass="default" Width="80" MaxLength="16" /> <asp:ImageButton ID="imgtxtEstEE" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calculator.gif" /></td>
                                                                    <td bgcolor="#E9E9E9" align="center" style="border-left:solid 1px #999999"><asp:Label ID="lblForeEE" runat="server" CssClass="default" /></td>
                                                                    <td bgcolor="#E9E9E9" align="center"><asp:Label ID="lblVarDEE" runat="server" CssClass="default" /></td>
                                                                    <td bgcolor="#E9E9E9" align="center" style="border-right:solid 1px #999999"><asp:Label ID="lblVarPEE" runat="server" CssClass="default" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>HW/SW/One Time Cost:</td>
                                                                    <td align="center"><asp:TextBox ID="txtAppHE" runat="server" CssClass="default" Width="80" MaxLength="16" /> <asp:ImageButton ID="imgtxtAppHE" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calculator.gif" /></td>
                                                                    <td align="center"><asp:TextBox ID="txtActHE" runat="server" CssClass="default" Width="80" MaxLength="16" /> <asp:ImageButton ID="imgtxtActHE" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calculator.gif" /></td>
                                                                    <td align="center"><asp:TextBox ID="txtEstHE" runat="server" CssClass="default" Width="80" MaxLength="16" /> <asp:ImageButton ID="imgtxtEstHE" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calculator.gif" /></td>
                                                                    <td bgcolor="#E9E9E9" align="center" style="border-left:solid 1px #999999"><asp:Label ID="lblForeEH" runat="server" CssClass="default" /></td>
                                                                    <td bgcolor="#E9E9E9" align="center"><asp:Label ID="lblVarDEH" runat="server" CssClass="default" /></td>
                                                                    <td bgcolor="#E9E9E9" align="center" style="border-right:solid 1px #999999"><asp:Label ID="lblVarPEH" runat="server" CssClass="default" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td bgcolor="#E9E9E9" style="border-top:solid 1px #999999;border-left:solid 1px #999999" class="default"><b>PHASE TOTALS:</b></td>
                                                                    <td bgcolor="#E9E9E9" align="center" style="border-top:solid 1px #999999"><asp:Label ID="lblAppE" runat="server" CssClass="defaultbold" /></td>
                                                                    <td bgcolor="#E9E9E9" align="center" style="border-top:solid 1px #999999"><asp:Label ID="lblActE" runat="server" CssClass="defaultbold" /></td>
                                                                    <td bgcolor="#E9E9E9" align="center" style="border-top:solid 1px #999999"><asp:Label ID="lblEstimateE" runat="server" CssClass="defaultbold" /></td>
                                                                    <td bgcolor="#E9E9E9" align="center"><asp:Label ID="lblForeE" runat="server" CssClass="defaultbold" /></td>
                                                                    <td bgcolor="#E9E9E9" align="center"><asp:Label ID="lblVarDE" runat="server" CssClass="defaultbold" /></td>
                                                                    <td bgcolor="#E9E9E9" align="center" style="border-right:solid 1px #999999"><asp:Label ID="lblVarPE" runat="server" CssClass="defaultbold" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="4" style="border-top:solid 1px #999999"><img src="/images/spacer.gif" border="0" width="1" height="1" /></td>
                                                                    <td colspan="3" style="border-bottom:solid 1px #999999;border-top:solid 1px #999999"><img src="/images/spacer.gif" border="0" width="1" height="1" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" class="header"><b>Closing:</b></td>
                                                                    <td align="center" class="greenheader"><b>Approved C1 & PCRs</b></td>
                                                                    <td align="center" class="greenheader"><b>Actuals to Date</b></td>
                                                                    <td align="center" class="greenheader"><b>Estimate to Complete</b></td>
                                                                    <td bgcolor="#E9E9E9" class="default" align="center" style="border-left:solid 1px #999999"><b>Current Forecast</b></td>
                                                                    <td bgcolor="#E9E9E9" class="default" align="center"><b>Variance $</b></td>
                                                                    <td bgcolor="#E9E9E9" class="default" align="center" style="border-right:solid 1px #999999"><b>Variance %</b></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>Internal Labor:</td>
                                                                    <td align="center"><asp:TextBox ID="txtAppIC" runat="server" CssClass="default" Width="80" MaxLength="16" /> <asp:ImageButton ID="imgtxtAppIC" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calculator.gif" /></td>
                                                                    <td align="center"><asp:TextBox ID="txtActIC" runat="server" CssClass="default" Width="80" MaxLength="16" /> <asp:ImageButton ID="imgtxtActIC" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calculator.gif" /></td>
                                                                    <td align="center"><asp:TextBox ID="txtEstIC" runat="server" CssClass="default" Width="80" MaxLength="16" /> <asp:ImageButton ID="imgtxtEstIC" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calculator.gif" /></td>
                                                                    <td bgcolor="#E9E9E9" align="center" style="border-left:solid 1px #999999"><asp:Label ID="lblForeCI" runat="server" CssClass="default" /></td>
                                                                    <td bgcolor="#E9E9E9" align="center"><asp:Label ID="lblVarDCI" runat="server" CssClass="default" /></td>
                                                                    <td bgcolor="#E9E9E9" align="center" style="border-right:solid 1px #999999"><asp:Label ID="lblVarPCI" runat="server" CssClass="default" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>External Labor:</td>
                                                                    <td align="center"><asp:TextBox ID="txtAppExC" runat="server" CssClass="default" Width="80" MaxLength="16" /> <asp:ImageButton ID="imgtxtAppExC" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calculator.gif" /></td>
                                                                    <td align="center"><asp:TextBox ID="txtActEC" runat="server" CssClass="default" Width="80" MaxLength="16" /> <asp:ImageButton ID="imgtxtActEC" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calculator.gif" /></td>
                                                                    <td align="center"><asp:TextBox ID="txtEstEC" runat="server" CssClass="default" Width="80" MaxLength="16" /> <asp:ImageButton ID="imgtxtEstEC" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calculator.gif" /></td>
                                                                    <td bgcolor="#E9E9E9" align="center" style="border-left:solid 1px #999999"><asp:Label ID="lblForeCE" runat="server" CssClass="default" /></td>
                                                                    <td bgcolor="#E9E9E9" align="center"><asp:Label ID="lblVarDCE" runat="server" CssClass="default" /></td>
                                                                    <td bgcolor="#E9E9E9" align="center" style="border-right:solid 1px #999999"><asp:Label ID="lblVarPCE" runat="server" CssClass="default" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>HW/SW/One Time Cost:</td>
                                                                    <td align="center"><asp:TextBox ID="txtAppHC" runat="server" CssClass="default" Width="80" MaxLength="16" /> <asp:ImageButton ID="imgtxtAppHC" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calculator.gif" /></td>
                                                                    <td align="center"><asp:TextBox ID="txtActHC" runat="server" CssClass="default" Width="80" MaxLength="16" /> <asp:ImageButton ID="imgtxtActHC" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calculator.gif" /></td>
                                                                    <td align="center"><asp:TextBox ID="txtEstHC" runat="server" CssClass="default" Width="80" MaxLength="16" /> <asp:ImageButton ID="imgtxtEstHC" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calculator.gif" /></td>
                                                                    <td bgcolor="#E9E9E9" align="center" style="border-left:solid 1px #999999"><asp:Label ID="lblForeCH" runat="server" CssClass="default" /></td>
                                                                    <td bgcolor="#E9E9E9" align="center"><asp:Label ID="lblVarDCH" runat="server" CssClass="default" /></td>
                                                                    <td bgcolor="#E9E9E9" align="center" style="border-right:solid 1px #999999"><asp:Label ID="lblVarPCH" runat="server" CssClass="default" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td bgcolor="#E9E9E9" style="border-top:solid 1px #999999;border-left:solid 1px #999999" class="default"><b>PHASE TOTALS:</b></td>
                                                                    <td bgcolor="#E9E9E9" align="center" style="border-top:solid 1px #999999"><asp:Label ID="lblAppC" runat="server" CssClass="defaultbold" /></td>
                                                                    <td bgcolor="#E9E9E9" align="center" style="border-top:solid 1px #999999"><asp:Label ID="lblActC" runat="server" CssClass="defaultbold" /></td>
                                                                    <td bgcolor="#E9E9E9" align="center" style="border-top:solid 1px #999999"><asp:Label ID="lblEstimateC" runat="server" CssClass="defaultbold" /></td>
                                                                    <td bgcolor="#E9E9E9" align="center"><asp:Label ID="lblForeC" runat="server" CssClass="defaultbold" /></td>
                                                                    <td bgcolor="#E9E9E9" align="center"><asp:Label ID="lblVarDC" runat="server" CssClass="defaultbold" /></td>
                                                                    <td bgcolor="#E9E9E9" align="center" style="border-right:solid 1px #999999"><asp:Label ID="lblVarPC" runat="server" CssClass="defaultbold" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="4" style="border-top:solid 1px #999999"><img src="/images/spacer.gif" border="0" width="1" height="1" /></td>
                                                                    <td colspan="3" style="border-top:solid 1px #999999"><img src="/images/spacer.gif" border="0" width="1" height="1" /></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" class="hugeheader">Project Totals:</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <table width="100%" cellpadding="5" cellspacing="0" border="0">
                                                                <tr>
                                                                    <td align="left" class="default"></td>
                                                                    <td align="center" class="greenheader"><b>Approved C1 & PCRs</b></td>
                                                                    <td align="center" class="greenheader"><b>Actuals to Date</b></td>
                                                                    <td align="center" class="greenheader"><b>Estimate to Complete</b></td>
                                                                    <td bgcolor="#E9E9E9" class="default" align="center" style="border-left:solid 1px #999999;border-top:solid 1px #999999"><b>Current Forecast</b></td>
                                                                    <td bgcolor="#E9E9E9" class="default" align="center" style="border-top:solid 1px #999999"><b>Variance $</b></td>
                                                                    <td bgcolor="#E9E9E9" class="default" align="center" style="border-top:solid 1px #999999;border-right:solid 1px #999999"><b>Variance %</b></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>Internal Labor:</td>
                                                                    <td align="center"><asp:Label ID="lblAppIn" runat="server" CssClass="default" /></td>
                                                                    <td align="center"><asp:Label ID="lblActIn" runat="server" CssClass="default" /></td>
                                                                    <td align="center"><asp:Label ID="lblEstIn" runat="server" CssClass="default" /></td>
                                                                    <td bgcolor="#E9E9E9" align="center" style="border-left:solid 1px #999999"><asp:Label ID="lblForeIn" runat="server" CssClass="default" /></td>
                                                                    <td bgcolor="#E9E9E9" align="center"><asp:Label ID="lblVarDIn" runat="server" CssClass="default" /></td>
                                                                    <td bgcolor="#E9E9E9" align="center" style="border-right:solid 1px #999999"><asp:Label ID="lblVarPIn" runat="server" CssClass="default" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>External Labor:</td>
                                                                    <td align="center"><asp:Label ID="lblAppEx" runat="server" CssClass="default" /></td>
                                                                    <td align="center"><asp:Label ID="lblActEx" runat="server" CssClass="default" /></td>
                                                                    <td align="center"><asp:Label ID="lblEstEx" runat="server" CssClass="default" /></td>
                                                                    <td bgcolor="#E9E9E9" align="center" style="border-left:solid 1px #999999"><asp:Label ID="lblForeEx" runat="server" CssClass="default" /></td>
                                                                    <td bgcolor="#E9E9E9" align="center"><asp:Label ID="lblVarDEx" runat="server" CssClass="default" /></td>
                                                                    <td bgcolor="#E9E9E9" align="center" style="border-right:solid 1px #999999"><asp:Label ID="lblVarPEx" runat="server" CssClass="default" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>HW/SW/One Time Cost:</td>
                                                                    <td align="center"><asp:Label ID="lblAppCl" runat="server" CssClass="default" /></td>
                                                                    <td align="center"><asp:Label ID="lblActCl" runat="server" CssClass="default" /></td>
                                                                    <td align="center"><asp:Label ID="lblEstCl" runat="server" CssClass="default" /></td>
                                                                    <td bgcolor="#E9E9E9" align="center" style="border-left:solid 1px #999999"><asp:Label ID="lblForeCl" runat="server" CssClass="default" /></td>
                                                                    <td bgcolor="#E9E9E9" align="center"><asp:Label ID="lblVarDCl" runat="server" CssClass="default" /></td>
                                                                    <td bgcolor="#E9E9E9" align="center" style="border-right:solid 1px #999999"><asp:Label ID="lblVarPCl" runat="server" CssClass="default" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td bgcolor="#E9E9E9" style="border-bottom:solid 1px #999999;border-top:solid 1px #999999;border-left:solid 1px #999999" class="default"><b>TOTALS:</b></td>
                                                                    <td bgcolor="#E9E9E9" align="center" style="border-bottom:solid 1px #999999;border-top:solid 1px #999999"><asp:Label ID="lblApp" runat="server" CssClass="defaultbold" /></td>
                                                                    <td bgcolor="#E9E9E9" align="center" style="border-bottom:solid 1px #999999;border-top:solid 1px #999999"><asp:Label ID="lblAct" runat="server" CssClass="defaultbold" /></td>
                                                                    <td bgcolor="#E9E9E9" align="center" style="border-bottom:solid 1px #999999;border-top:solid 1px #999999"><asp:Label ID="lblEstimate" runat="server" CssClass="defaultbold" /></td>
                                                                    <td bgcolor="#E9E9E9" align="center" style="border-bottom:solid 1px #999999"><asp:Label ID="lblFore" runat="server" CssClass="defaultbold" /></td>
                                                                    <td bgcolor="#E9E9E9" align="center" style="border-bottom:solid 1px #999999"><asp:Label ID="lblVarD" runat="server" CssClass="defaultbold" /></td>
                                                                    <td bgcolor="#E9E9E9" align="center" style="border-bottom:solid 1px #999999;border-right:solid 1px #999999"><asp:Label ID="lblVarP" runat="server" CssClass="defaultbold" /></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
		                                    </div>
		                                    <div id="divStatus" style='<%=boolStatus == true ? "display:inline" : "display:none" %>'>
		                                        <br /><br /><br />
                                                <table width="100%" cellpadding="5" cellspacing="0" border="0">
                                                    <tr>
                                                        <td width="50%" valign="top">
                                                            <table width="100%" cellpadding="5" cellspacing="0" border="0">
                                                                <tr>
                                                                    <td nowrap><b>Status Date:</b><font class="required">&nbsp;*</font></td>
                                                                    <td width="100%"><asp:TextBox ID="txtStatusDate" runat="server" CssClass="default" Width="100" MaxLength="10" /> <asp:ImageButton ID="imgStatusDate" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td nowrap><b>Scope Variance:</b><font class="required">&nbsp;*</font></td>
                                                                    <td width="100%">
                                                                        <asp:DropDownList ID="ddlScope" runat="server" CssClass="default" >
                                                                            <asp:ListItem Text="-- SELECT --" Value="0" />
                                                                            <asp:ListItem Text="Red" Value="1" />
                                                                            <asp:ListItem Text="Yellow" Value="2" />
                                                                            <asp:ListItem Text="Green" Value="3" />
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td nowrap><b>Timeline Variance:</b><font class="required">&nbsp;*</font></td>
                                                                    <td width="100%">
                                                                        <asp:DropDownList ID="ddlTimeline" runat="server" CssClass="default" >
                                                                            <asp:ListItem Text="-- SELECT --" Value="0" />
                                                                            <asp:ListItem Text="Red" Value="1" />
                                                                            <asp:ListItem Text="Yellow" Value="2" />
                                                                            <asp:ListItem Text="Green" Value="3" />
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td nowrap><b>Budget Variance:</b><font class="required">&nbsp;*</font></td>
                                                                    <td width="100%">
                                                                        <asp:DropDownList ID="ddlBudget" runat="server" CssClass="default" >
                                                                            <asp:ListItem Text="-- SELECT --" Value="0" />
                                                                            <asp:ListItem Text="Red" Value="1" />
                                                                            <asp:ListItem Text="Yellow" Value="2" />
                                                                            <asp:ListItem Text="Green" Value="3" />
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td nowrap><b>This Week's<br />Accomplishments:</b><font class="required">&nbsp;*</font></td>
                                                                    <td width="100%"><asp:TextBox ID="txtThisWeek" runat="server" CssClass="default" Width="90%" TextMode="MultiLine" Rows="5" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td nowrap><b>Next Week's<br />Accomplishments:</b><font class="required">&nbsp;*</font></td>
                                                                    <td width="100%"><asp:TextBox ID="txtNextWeek" runat="server" CssClass="default" Width="90%" TextMode="MultiLine" Rows="5" /></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td width="50%" valign="top">
                                                            <table width="100%" cellpadding="4" cellspacing="1" border="0">
                                                                <tr>
                                                                    <td><b>Comments / Issues:</b><font class="required">&nbsp;*</font></td>
                                                                </tr>
                                                                <tr>
                                                                    <td><asp:TextBox ID="txtComments" runat="server" CssClass="default" Width="90%" TextMode="MultiLine" Rows="17" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td><asp:Button ID="btnRetrieve" runat="server" CssClass="default" Text="Retrieve Latest" Width="150" OnClick="btnRetrieve_Click" /></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>                                                 
                                                <br /><br />
                                                <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                                    <tr>
                                                        <td>
                                                            <table width="100%" cellpadding="3" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                                                                <tr bgcolor="#EEEEEE">
                                                                    <td nowrap><b>Date</b></td>
                                                                    <td nowrap>&nbsp;</td>
                                                                    <td nowrap align="center"><b>Overall Status</b></td>
                                                                    <td></td>
                                                                    <td nowrap align="center"><b>Scope Status</b></td>
                                                                    <td nowrap align="center"><b>Timeline Status</b></td>
                                                                    <td nowrap align="center"><b>Budget Status</b></td>
                                                                </tr>
                                                                <asp:repeater ID="rptStatus" runat="server">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblScope" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "scope") %>' Visible="false" />
                                                                        <asp:Label ID="lblTimeline" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "timeline") %>' Visible="false" />
                                                                        <asp:Label ID="lblBudget" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "budget") %>' Visible="false" />
                                                                        <tr class="default">
                                                                            <td nowrap><a href="javascript:void(0);" onclick="ShowStatus('<%# DataBinder.Eval(Container.DataItem, "id") %>','t');"><%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "datestamp").ToString()).ToShortDateString() %></a></td>
                                                                            <td nowrap>&nbsp;</td>
                                                                            <td width="30%" align="center"><asp:Label ID="lblStatus" runat="server" CssClass="default" Text='' /></td>
                                                                            <td width="20%">&nbsp;</td>
                                                                            <td width="15%" align="center"><asp:Label ID="lblStatusScope" runat="server" CssClass="default" Text='' /></td>
                                                                            <td width="15%" align="center"><asp:Label ID="lblStatusTimeline" runat="server" CssClass="default" Text='' /></td>
                                                                            <td width="15%" align="center"><asp:Label ID="lblStatusBudget" runat="server" CssClass="default" Text='' /></td>
                                                                        </tr>
                                                                    </ItemTemplate>
                                                                </asp:repeater>
                                                            <tr>
                                                                <td colspan="7">
                                                                    <asp:Label ID="lblNoStatus" runat="server" CssClass="default" Visible="false" Text="<img src='/images/alert.gif' border='0' align='absmiddle'> There are no updates related to this project" />
                                                                </td>
                                                            </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
		                                    </div>
		                                    <div id="divInformation" style='<%=boolInformation == true ? "display:inline" : "display:none" %>'>
		                                        <br /><br /><br />
                                                <table width="100%" cellpadding="4" cellspacing="3" border="0" class="default">
                                                    <tr>
                                                        <td colspan="2" class="hugeheader">Capital Service Review Committee (CSRC) Form</td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap><b>NickName:</b></td>
                                                        <td><asp:TextBox ID="txtCSRCName" runat="server" CssClass="default" MaxLength="100" Width="150" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap><b>PMM Phase:</b></td>
                                                        <td width="100%">
                                                            <asp:CheckBox ID="chkDiscovery" Text="Discovery" runat="server" CssClass="default" /> 
                                                            <asp:CheckBox ID="chkPlanning" Text="Planning" runat="server" CssClass="default" /> 
                                                            <asp:CheckBox ID="chkExecution" Text="Execution" runat="server" CssClass="default" /> 
                                                            <asp:CheckBox ID="chkClosing" Text="Closing" runat="server" CssClass="default" />
                                                        </td>
                                                    </tr>
                                                    </tr>
                                                </table>
                                                <div id="divCSRC" runat="server" style="display:none">
                                                    <table width="100%" cellpadding="4" cellspacing="3" border="0" class="default">                                                     
                                                        <tr>
                                                            <td colspan="2">
                                                                <div id="divDiscovery" runat="server" style="display:none">
                                                                    <br />
                                                                    <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                                                        <tr>
                                                                            <td colspan="3" bgcolor="#6A8359" class="whitedefault"><div style="padding:3"><b>Discovery</b></div></td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="3" style="border-left:solid 1px #999999;border-right:solid 1px #999999;border-top:solid 1px #999999;">
                                                                                <table width="100%" cellpadding="4" cellspacing="3" border="0" class="default">
                                                                                    <tr>
                                                                                        <td nowrap><b>Phase Start Date:</b></td>
                                                                                        <td width="50%"><asp:TextBox ID="txtCSRCSD" runat="server" CssClass="default" Width="100" MaxLength="10" /> <asp:ImageButton ID="imgCSRCSD" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
                                                                                        <td nowrap><b>Internal Labor:</b></td>
                                                                                        <td width="50%"><asp:TextBox ID="txtCSRCID" runat="server" CssClass="default" Width="100" MaxLength="16" /> <asp:ImageButton ID="imgCSRCID" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calculator.gif" /></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td nowrap><b>Phase End Date:</b></td>
                                                                                        <td width="50%"><asp:TextBox ID="txtCSRCED" runat="server" CssClass="default" Width="100" MaxLength="10" /> <asp:ImageButton ID="imgCSRCED" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
                                                                                        <td nowrap><b>External Labor:</b></td>
                                                                                        <td width="50%"><asp:TextBox ID="txtCSRCExD" runat="server" CssClass="default" Width="100" MaxLength="16" /> <asp:ImageButton ID="imgCSRCExD" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calculator.gif" /></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td nowrap>&nbsp;</td>
                                                                                        <td width="50%">&nbsp;</td>
                                                                                        <td nowrap><b>HW/SW/One Time Cost:</b></td>
                                                                                        <td width="50%"><asp:TextBox ID="txtCSRCHD" runat="server" CssClass="default" Width="100" MaxLength="16" /> <asp:ImageButton ID="imgCSRCHD" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calculator.gif" /></td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td><img src="/images/shadow_box_l.gif" border="0" width="5" height="12"></td>
                                                                            <td width="100%" background="/images/shadow_box.gif"></td>
                                                                            <td><img src="/images/shadow_box_r.gif" border="0" width="5" height="12"></td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                                <div id="divPlanning" runat="server" style="display:none">
                                                                    <br />
                                                                    <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                                                        <tr>
                                                                            <td colspan="3" bgcolor="#6A8359" class="whitedefault"><div style="padding:3"><b>Planning</b></div></td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="3" style="border-left:solid 1px #999999;border-right:solid 1px #999999;border-top:solid 1px #999999;">
                                                                                <table width="100%" cellpadding="4" cellspacing="3" border="0" class="default">
                                                                                    <tr>
                                                                                        <td nowrap><b>Phase Start Date:</b></td>
                                                                                        <td width="50%"><asp:TextBox ID="txtCSRCSP" runat="server" CssClass="default" Width="100" MaxLength="10" /> <asp:ImageButton ID="imgCSRCSP" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
                                                                                        <td nowrap><b>Internal Labor:</b></td>
                                                                                        <td width="50%"><asp:TextBox ID="txtCSRCIP" runat="server" CssClass="default" Width="100" MaxLength="16" /> <asp:ImageButton ID="imgCSRCIP" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calculator.gif" /></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td nowrap><b>Phase End Date:</b></td>
                                                                                        <td width="50%"><asp:TextBox ID="txtCSRCEP" runat="server" CssClass="default" Width="100" MaxLength="10" /> <asp:ImageButton ID="imgCSRCEP" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
                                                                                        <td nowrap><b>External Labor:</b></td>
                                                                                        <td width="50%"><asp:TextBox ID="txtCSRCExP" runat="server" CssClass="default" Width="100" MaxLength="16" /> <asp:ImageButton ID="imgCSRCExP" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calculator.gif" /></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td nowrap>&nbsp;</td>
                                                                                        <td width="50%">&nbsp;</td>
                                                                                        <td nowrap><b>HW/SW/One Time Cost:</b></td>
                                                                                        <td width="50%"><asp:TextBox ID="txtCSRCHP" runat="server" CssClass="default" Width="100" MaxLength="16" /> <asp:ImageButton ID="imgCSRCHP" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calculator.gif" /></td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td><img src="/images/shadow_box_l.gif" border="0" width="5" height="12"></td>
                                                                            <td width="100%" background="/images/shadow_box.gif"></td>
                                                                            <td><img src="/images/shadow_box_r.gif" border="0" width="5" height="12"></td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                                <div id="divExecution" runat="server" style="display:none">
                                                                    <br />
                                                                    <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                                                        <tr>
                                                                            <td colspan="3" bgcolor="#6A8359" class="whitedefault"><div style="padding:3"><b>Execution</b></div></td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="3" style="border-left:solid 1px #999999;border-right:solid 1px #999999;border-top:solid 1px #999999;">
                                                                                <table width="100%" cellpadding="4" cellspacing="3" border="0" class="default">
                                                                                    <tr>
                                                                                        <td nowrap><b>Phase Start Date:</b></td>
                                                                                        <td width="50%"><asp:TextBox ID="txtCSRCSE" runat="server" CssClass="default" Width="100" MaxLength="10" /> <asp:ImageButton ID="imgCSRCSE" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
                                                                                        <td nowrap><b>Internal Labor:</b></td>
                                                                                        <td width="50%"><asp:TextBox ID="txtCSRCIE" runat="server" CssClass="default" Width="100" MaxLength="16" /> <asp:ImageButton ID="imgCSRCIE" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calculator.gif" /></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td nowrap><b>Phase End Date:</b></td>
                                                                                        <td width="50%"><asp:TextBox ID="txtCSRCEE" runat="server" CssClass="default" Width="100" MaxLength="10" /> <asp:ImageButton ID="imgCSRCEE" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
                                                                                        <td nowrap><b>External Labor:</b></td>
                                                                                        <td width="50%"><asp:TextBox ID="txtCSRCExE" runat="server" CssClass="default" Width="100" MaxLength="16" /> <asp:ImageButton ID="imgCSRCExE" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calculator.gif" /></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td nowrap>&nbsp;</td>
                                                                                        <td width="50%">&nbsp;</td>
                                                                                        <td nowrap><b>HW/SW/One Time Cost:</b></td>
                                                                                        <td width="50%"><asp:TextBox ID="txtCSRCHE" runat="server" CssClass="default" Width="100" MaxLength="16" /> <asp:ImageButton ID="imgCSRCHE" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calculator.gif" /></td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td><img src="/images/shadow_box_l.gif" border="0" width="5" height="12"></td>
                                                                            <td width="100%" background="/images/shadow_box.gif"></td>
                                                                            <td><img src="/images/shadow_box_r.gif" border="0" width="5" height="12"></td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                                <div id="divClosing" runat="server" style="display:none">
                                                                    <br />
                                                                    <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                                                        <tr>
                                                                            <td colspan="3" bgcolor="#6A8359" class="whitedefault"><div style="padding:3"><b>Closing</b></div></td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="3" style="border-left:solid 1px #999999;border-right:solid 1px #999999;border-top:solid 1px #999999;">
                                                                                <table width="100%" cellpadding="4" cellspacing="3" border="0" class="default">
                                                                                    <tr>
                                                                                        <td nowrap><b>Phase Start Date:</b></td>
                                                                                        <td width="50%"><asp:TextBox ID="txtCSRCSC" runat="server" CssClass="default" Width="100" MaxLength="10" /> <asp:ImageButton ID="imgCSRCSC" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
                                                                                        <td nowrap><b>Internal Labor:</b></td>
                                                                                        <td width="50%"><asp:TextBox ID="txtCSRCIC" runat="server" CssClass="default" Width="100" MaxLength="16" /> <asp:ImageButton ID="imgCSRCIC" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calculator.gif" /></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td nowrap><b>Phase End Date:</b></td>
                                                                                        <td width="50%"><asp:TextBox ID="txtCSRCEC" runat="server" CssClass="default" Width="100" MaxLength="10" /> <asp:ImageButton ID="imgCSRCEC" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
                                                                                        <td nowrap><b>External Labor:</b></td>
                                                                                        <td width="50%"><asp:TextBox ID="txtCSRCExC" runat="server" CssClass="default" Width="100" MaxLength="16" /> <asp:ImageButton ID="imgCSRCExC" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calculator.gif" /></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td nowrap>&nbsp;</td>
                                                                                        <td width="50%">&nbsp;</td>
                                                                                        <td nowrap><b>HW/SW/One Time Cost:</b></td>
                                                                                        <td width="50%"><asp:TextBox ID="txtCSRCHC" runat="server" CssClass="default" Width="100" MaxLength="16" /> <asp:ImageButton ID="imgCSRCHC" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calculator.gif" /></td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td><img src="/images/shadow_box_l.gif" border="0" width="5" height="12"></td>
                                                                            <td width="100%" background="/images/shadow_box.gif"></td>
                                                                            <td><img src="/images/shadow_box_r.gif" border="0" width="5" height="12"></td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </td>                                                            
                                                        </tr>    
                                                        <tr>                                                         
                                                           <td colspan="2" align="right"><asp:Button ID="btnSubmitCSRC" runat="server" CssClass="default" Text="Submit CSRC Request" Width="150" OnClick="btnSubmitCSRC_Click" /></td>
                                                        </tr>                                                                                               
                                                    </table>
                                                </div>
                                                <table width="100%" cellpadding="3" cellspacing="0" border="0" class="default">
                                                    <tr bgcolor="#EEEEEE">
                                                        <td><b>Nickname</b></td>
                                                        <td><b>PDF</b></td>
                                                        <td><b>Change Status</b></td>
                                                        <td><b>Approved/Denied Date</b></td>
                                                        <td><b>Route?</b></td>
                                                    </tr>
                                                    <%=strCSRC %>                                                  
                                                </table>
                                                <p><hr size="1" noshade /></p>
                                                <table width="100%" cellpadding="4" cellspacing="3" border="0" class="default">
                                                    <tr>
                                                        <td colspan="2" class="hugeheader">Project Change Request (PCR) Form</td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap><b>Show PCR Form:</b></td>
                                                        <td width="100%">
                                                            <asp:CheckBox ID="chkPCR" runat="server" CssClass="default" />
                                                        </td>
                                                    </tr>
                                                </table>
                                                <div id="divPCR" runat="server" style="display:none">
                                                    <table width="100%" cellpadding="4" cellspacing="3" border="0" class="default">
                                                        <tr>
                                                            <td colspan="2">
                                                                <table width="100%" cellpadding="0" cellspacing="0" border="0" class="default">
                                                                    <tr>
                                                                        <td colspan="3" bgcolor="#6A8359" class="whitedefault"><div style="padding:3"><b>Approved PCR Information</b></div></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="3" style="border-left:solid 1px #999999;border-right:solid 1px #999999;border-top:solid 1px #999999;">
                                                                            <table width="100%" cellpadding="4" cellspacing="3" border="0" class="default">
                                                                                <tr>
                                                                                    <td nowrap><b>NickName:</b></td>
                                                                                    <td width="100%"><asp:TextBox ID="txtPCRName" runat="server" CssClass="default" Width="150" MaxLength="100" /></td>
                                                                                </tr>  
                                                                                <tr>
                                                                                    <td nowrap><b>Scope:</b></td>
                                                                                    <td width="100%"><asp:CheckBox ID="chkScope" runat="server" CssClass="default" /></td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td colspan="3">
                                                                                        <div id="divScope" runat="server" style="display:none">
                                                                                            <table width="50%" cellpadding="0" cellspacing="0" border="0" class="default" style="border:dashed 1px #CCCCCC">
                                                                                                <tr>
                                                                                                    <td nowrap>Reason for Scope Change:</td>
                                                                                                    <td><asp:TextBox ID="txtScopeComments" runat="server" Width="400" CssClass="default" Rows="5" TextMode="MultiLine" /></td>
                                                                                                </tr> 
                                                                                            </table>                                                                                    
                                                                                        </div>  
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td nowrap><b>Schedule:</b></td>
                                                                                    <td width="100%"><asp:CheckBox ID="chkSchedule" runat="server" CssClass="default" /></td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td colspan="2">
                                                                                        <div id="divSchedule" runat="server" style="display:none">
                                                                                            <table width="100%" cellpadding="3" cellspacing="0" border="0" style="border:dashed 1px #CCCCCC">
                                                                                                <tr>
                                                                                                    <td class="greenheader"></td>
                                                                                                    <td class="greenheader"><b>Phase</b></td>
                                                                                                    <td class="greenheader"></td>
                                                                                                    <td class="greenheader" align="center"><b>Approved Dates</b></td>
                                                                                                    <td class="greenheader"></td>
                                                                                                    <td class="greenheader" align="center"><b>Modified Dates</b></td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td><asp:CheckBox ID="chkPCRScheduleD" runat="server" CssClass="default" /></td>
                                                                                                    <td>Discovery</td>
                                                                                                    <td align="center"><img src="/images/green_arrow.gif" border="0" align="absmiddle" /></td>
                                                                                                    <td align="center"><asp:Label ID="lblPCRScheduleD" runat="server" CssClass="default" /></td>
                                                                                                    <td align="center"><img src="/images/green_arrow.gif" border="0" align="absmiddle" /></td>
                                                                                                    <td align="center"><asp:TextBox ID="txtPCRScheduleDS" runat="server" CssClass="default" Width="100" MaxLength="10" /> <asp:ImageButton ID="imgPCRScheduleDS" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /> - <asp:TextBox ID="txtPCRScheduleDE" runat="server" CssClass="default" Width="100" MaxLength="10" /> <asp:ImageButton ID="imgPCRScheduleDE" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td><asp:CheckBox ID="chkPCRScheduleP" runat="server" CssClass="default" /></td>
                                                                                                    <td>Planning</td>
                                                                                                    <td align="center"><img src="/images/green_arrow.gif" border="0" align="absmiddle" /></td>
                                                                                                    <td align="center"><asp:Label ID="lblPCRScheduleP" runat="server" CssClass="default" /></td>
                                                                                                    <td align="center"><img src="/images/green_arrow.gif" border="0" align="absmiddle" /></td>
                                                                                                    <td align="center"><asp:TextBox ID="txtPCRSchedulePS" runat="server" CssClass="default" Width="100" MaxLength="10" /> <asp:ImageButton ID="imgPCRSchedulePS" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /> - <asp:TextBox ID="txtPCRSchedulePE" runat="server" CssClass="default" Width="100" MaxLength="10" /> <asp:ImageButton ID="imgPCRSchedulePE" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td><asp:CheckBox ID="chkPCRScheduleE" runat="server" CssClass="default" /></td>
                                                                                                    <td>Execution</td>
                                                                                                    <td align="center"><img src="/images/green_arrow.gif" border="0" align="absmiddle" /></td>
                                                                                                    <td align="center"><asp:Label ID="lblPCRScheduleE" runat="server" CssClass="default" /></td>
                                                                                                    <td align="center"><img src="/images/green_arrow.gif" border="0" align="absmiddle" /></td>
                                                                                                    <td align="center"><asp:TextBox ID="txtPCRScheduleES" runat="server" CssClass="default" Width="100" MaxLength="10" /> <asp:ImageButton ID="imgPCRScheduleES" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /> - <asp:TextBox ID="txtPCRScheduleEE" runat="server" CssClass="default" Width="100" MaxLength="10" /> <asp:ImageButton ID="imgPCRScheduleEE" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td><asp:CheckBox ID="chkPCRScheduleC" runat="server" CssClass="default" /></td>
                                                                                                    <td>Closing</td>
                                                                                                    <td align="center"><img src="/images/green_arrow.gif" border="0" align="absmiddle" /></td>
                                                                                                    <td align="center"><asp:Label ID="lblPCRScheduleC" runat="server" CssClass="default" /></td>
                                                                                                    <td align="center"><img src="/images/green_arrow.gif" border="0" align="absmiddle" /></td>
                                                                                                    <td align="center"><asp:TextBox ID="txtPCRScheduleCS" runat="server" CssClass="default" Width="100" MaxLength="10" /> <asp:ImageButton ID="imgPCRScheduleCS" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /> - <asp:TextBox ID="txtPCRScheduleCE" runat="server" CssClass="default" Width="100" MaxLength="10" /> <asp:ImageButton ID="imgPCRScheduleCE" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                   <td colspan="4">
                                                                                                     <table width="50%" cellpadding="2" cellspacing="0" border="0" class="default">
                                                                                                        <tr>
                                                                                                            <td nowrap>Reason for Schedule Change:</td>
                                                                                                            <td><asp:TextBox ID="txtScheduleComments" runat="server" Width="400" CssClass="default" Rows="5" TextMode="MultiLine" /></td>
                                                                                                        </tr> 
                                                                                                     </table>          
                                                                                                   </td>                                                                                                    
                                                                                                </tr>
                                                                                            </table>
                                                                                        </div>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td nowrap><b>Financial:</b></td>
                                                                                    <td width="100%"><asp:CheckBox ID="chkFinancial" runat="server" CssClass="default" /></td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td colspan="2">
                                                                                        <div id="divFinancial" runat="server" style="display:none">
                                                                                            <table width="100%" cellpadding="3" cellspacing="0" border="0" style="border:dashed 1px #CCCCCC">
                                                                                                <tr>
                                                                                                    <td class="greenheader"></td>
                                                                                                    <td class="greenheader"><b>Phase</b></td>
                                                                                                    <td class="greenheader"></td>
                                                                                                    <td class="greenheader" align="center"><b>Approved Financials</b></td>
                                                                                                    <td class="greenheader"></td>
                                                                                                    <td class="greenheader" align="center"><b>Modified Financials</b></td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td><asp:CheckBox ID="chkPCRFinancialD" runat="server" CssClass="default" /></td>
                                                                                                    <td>Discovery</td>
                                                                                                    <td align="center"><img src="/images/green_arrow.gif" border="0" align="absmiddle" /></td>
                                                                                                    <td align="center"><asp:Label ID="lblPCRFinancialD" runat="server" CssClass="default" /></td>
                                                                                                    <td align="center"><img src="/images/green_arrow.gif" border="0" align="absmiddle" /></td>
                                                                                                    <td align="center"><asp:TextBox ID="txtPCRFinancialD" runat="server" CssClass="default" Width="100" MaxLength="16" /> <asp:ImageButton ID="imgPCRFinancialD" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calculator.gif" /></td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td><asp:CheckBox ID="chkPCRFinancialP" runat="server" CssClass="default" /></td>
                                                                                                    <td>Planning</td>
                                                                                                    <td align="center"><img src="/images/green_arrow.gif" border="0" align="absmiddle" /></td>
                                                                                                    <td align="center"><asp:Label ID="lblPCRFinancialP" runat="server" CssClass="default" /></td>
                                                                                                    <td align="center"><img src="/images/green_arrow.gif" border="0" align="absmiddle" /></td>
                                                                                                    <td align="center"><asp:TextBox ID="txtPCRFinancialP" runat="server" CssClass="default" Width="100" MaxLength="16" /> <asp:ImageButton ID="imgPCRFinancialP" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calculator.gif" /></td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td><asp:CheckBox ID="chkPCRFinancialE" runat="server" CssClass="default" /></td>
                                                                                                    <td>Execution</td>
                                                                                                    <td align="center"><img src="/images/green_arrow.gif" border="0" align="absmiddle" /></td>
                                                                                                    <td align="center"><asp:Label ID="lblPCRFinancialE" runat="server" CssClass="default" /></td>
                                                                                                    <td align="center"><img src="/images/green_arrow.gif" border="0" align="absmiddle" /></td>
                                                                                                    <td align="center"><asp:TextBox ID="txtPCRFinancialE" runat="server" CssClass="default" Width="100" MaxLength="16" /> <asp:ImageButton ID="imgPCRFinancialE" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calculator.gif" /></td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td><asp:CheckBox ID="chkPCRFinancialC" runat="server" CssClass="default" /></td>
                                                                                                    <td>Closing</td>
                                                                                                    <td align="center"><img src="/images/green_arrow.gif" border="0" align="absmiddle" /></td>
                                                                                                    <td align="center"><asp:Label ID="lblPCRFinancialC" runat="server" CssClass="default" /></td>
                                                                                                    <td align="center"><img src="/images/green_arrow.gif" border="0" align="absmiddle" /></td>
                                                                                                    <td align="center"><asp:TextBox ID="txtPCRFinancialC" runat="server" CssClass="default" Width="100" MaxLength="16" /> <asp:ImageButton ID="imgPCRFinancialC" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calculator.gif" /></td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                   <td colspan="4">
                                                                                                     <table width="50%" cellpadding="2" cellspacing="0" border="0" class="default">
                                                                                                        <tr>
                                                                                                            <td nowrap>Reason for Financial Change:</td>
                                                                                                            <td><asp:TextBox ID="txtFinancialComments" runat="server" Width="400" CssClass="default" Rows="5" TextMode="MultiLine" /></td>
                                                                                                        </tr> 
                                                                                                     </table>          
                                                                                                   </td>                                                                                                    
                                                                                                </tr>
                                                                                            </table>
                                                                                        </div>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td nowrap><b>Reason for PCR:</b></td>
                                                                                    <td width="100%"><asp:CheckBox ID="chkReason" runat="server" CssClass="default" /></td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td colspan="2">
                                                                                        <div id="divReason" runat="server" style="display:none">
                                                                                        <asp:CheckBoxList ID="chkPCRReason" runat="server" CssClass="default" RepeatColumns="3" RepeatDirection="horizontal">
                                                                                            <asp:ListItem Value="Resource Constraints (internal and external)" />
                                                                                            <asp:ListItem Value="Estimates" />
                                                                                            <asp:ListItem Value="Scope change" />
                                                                                            <asp:ListItem Value="Vendor issues" />
                                                                                            <asp:ListItem Value="Technical issues" />
                                                                                            <asp:ListItem Value="Dependencies (other projects or other...)" />
                                                                                            <asp:ListItem Value="Unknown components" />
                                                                                            <asp:ListItem Value="Unknown resources" />
                                                                                            <asp:ListItem Value="IP/Audit requirements" />
                                                                                            <asp:ListItem Value="Prioritization change" />
                                                                                            <asp:ListItem Value="Sponsor mis-communication" />
                                                                                        </asp:CheckBoxList>
                                                                                          <table width="50%" cellpadding="2" cellspacing="0" border="0" class="default">
                                                                                              <tr>
                                                                                                 <td nowrap>Other:</td>
                                                                                                 <td><asp:TextBox ID="TextBox17" runat="server" Width="400" CssClass="default" Rows="5" TextMode="MultiLine" /></td>
                                                                                              </tr> 
                                                                                          </table>        
                                                                                        </div>
                                                                                    </td>
                                                                                </tr>                                                                                
                                                                                <tr>
                                                                                    <td colspan="2" align="right"><asp:Button ID="btnSubmitPCR" runat="server" CssClass="default" Width="150" Text="Submit PCR Request" OnClick="btnSubmitPCR_Click" /></td>
                                                                                </tr>                                                                                 
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td><img src="/images/shadow_box_l.gif" border="0" width="5" height="12"></td>
                                                                        <td width="100%" background="/images/shadow_box.gif"></td>
                                                                        <td><img src="/images/shadow_box_r.gif" border="0" width="5" height="12"></td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>                                                                                                        
                                                    </table>
    		                                    </div>
                                                <table width="100%" cellpadding="3" cellspacing="0" border="0" class="default">
                                                    <tr bgcolor="#EEEEEE">
                                                        <td valign="top" nowrap><b>Nickname</b></td>   
                                                        <td valign="top" nowrap><b>PDF</b></td>   
                                                        <td valign="top" nowrap><b>Submission Date</b></td>                                                   
                                                        <td valign="top" nowrap> <b>Change Status</b></td>
                                                        <td valign="top" nowrap><b>Approved/Denied Date</b></td>
                                                        <td align="center" valign="top" nowrap><b>Scope Change</b></td>
                                                        <td align="center" valign="top" nowrap><b>Schedule Change</b></td>
                                                        <td align="center" valign="top" nowrap><b>Financial Change</b></td>
                                                        <td align="center" valign="top" nowrap><b>Route?</b></td>
                                                    </tr>
                                                    <%=strPCR %> 
                                                    <tr>
                                                       <td colspan="10" align="right"><asp:Button ID="btnUpdatePCR" runat="Server" CssClass="default" Width="150" Text="Update PCR Status" /></td>                                                        
                                                    </tr>                                                  
                                                </table>
    		                                </div>
		                                    <div id="divClose" style='<%=boolClose == true ? "display:inline" : "display:none" %>'>
		                                        <br />
                                                <table width="100%" cellpadding="4" cellspacing="3" border="0" class="default">
                                                    <tr>
                                                        <td colspan="2">&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <table cellpadding="2" cellspacing="1" border="0" class="default">
                                                                <tr>
                                                                    <td nowrap><b>Closure Details</b></td>                                                                     
                                                                </tr>
                                                                <tr>
                                                                    <td class="default">                                                                                                                                                                                                      
                                                                      <p>
                                                                       This notification is to confirm the closure of Project "<%= lblName.Text %>". As of <font class="highlight"><%= lblProjectEnd.Text %></font> 
                                                                       Project "<%= lblName.Text %>" will be considered formally closed. The following has been accomplished: Click <a href="javascript:void(0);" onclick="ShowHideDiv2('<%= divAcc.ClientID %>');" class="link:hover">here</a> to add an accomplishment. 
                                                                      </p>                                                                                                                                             
                                                                    </td>
                                                                </tr>                                                                      
                                                                <tr>
                                                                    <td>
                                                                       <div id="divAcc" runat="server" style="display:none">
                                                                          <table width="100%" cellpadding="3" cellspacing="2" class="default" border="0">
                                                                              <tr>
                                                                                 <td colspan="2"><b>Accomplishment:</b></td>
                                                                              </tr>
                                                                              <tr>
                                                                                 <td colspan="2"><asp:TextBox ID="txtAccomplishment" runat="server" CssClass="default" Width="400" TextMode="MultiLine" Rows="5" /></td>                                                                                                                                                                                                                                                        
                                                                              </tr>
                                                                              <tr>
                                                                                 <td><asp:Button ID="btnAddAcc" runat="Server" CssClass="default" Width="75" Text="Add" /> <asp:Button ID="btnUpdateAcc" runat="Server" CssClass="default" Width="75" Text="Update" /></td>
                                                                              </tr>                                                                                                                                                          
                                                                              <tr>
                                                                                 <td><span id="AccSpan" runat="server" class="default"></span></td>
                                                                              </tr> 
                                                                              <tr>
                                                                                 <td colspan="2" align="right"></td>                                                                         
                                                                              </tr>
                                                                          </table>
                                                                       </div>
                                                                    </td>
                                                                </tr>                                                               
                                                                <tr>                                                                    
                                                                    <td class="default">                                                                                   
                                                                                                                                           
                                                                         Please contact <asp:TextBox ID="txtLead" runat="server" CssClass="default" Width="150" />
                                                                          <div id="divLead" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                                                                            <asp:ListBox ID="lstLead" runat="server" CssClass="default" />
                                                                          </div>                                                                                                                                       
                                                                         by <asp:TextBox ID="txtOneWeek" runat="server"  Width="80" MaxLength="16" CssClass="Default"/> <asp:ImageButton ID="img1Week" runat="server" ImageUrl="/images/calendar.gif" ImageAlign="AbsMiddle" />.
                                                                         if you identify any issues that could require the project to be re-opened.                                                                     
                                                                         
                                                                         You will be sent a Client Survey by <asp:TextBox ID="txtSurvey" runat="server" Width="80" MaxLength="16" CssClass="Default" /> <asp:ImageButton ID="imgSurvey" runat="server" ImageUrl="/images/calendar.gif" ImageAlign="AbsMiddle" />. This survey is
                                                                         administered as part of Technical Project Management's continuous improvement initiatives. We appreciate your participation and value your honest feedback.                                                                      
                                                                                                                                          
                                                                     </td>                                                                      
                                                                </tr>
                                                            </table>
                                                        </td>                                                                                                        
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <table cellpadding="3" cellspacing="2" border="0"  class="offsettable">
                                                                <tr>
                                                                    <td colspan="7" class="greentableheader">Project Financials:</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>&nbsp;</td>
                                                                    <td>&nbsp;</td>
                                                                    <td align="center"><b>Approved</b></td>
                                                                    <td>&nbsp;</td>
                                                                    <td align="center"><b>Actual</b></td>
                                                                    <td>&nbsp;</td>
                                                                    <td align="center"><b>Variance</b></td>
                                                                    <td>&nbsp;</td>
                                                                    <td align="center"><b>Variance(%)</b></td>                                                                    
                                                                </tr>
                                                                <tr>
                                                                    <td nowrap>Internal Labor</td>
                                                                    <td>&nbsp;</td>
                                                                    <td align="center">$<asp:Label ID="lblApprovedI" runat="server" CssClass="default" /></td>
                                                                    <td>&nbsp;</td>
                                                                    <td align="center">$<asp:Label ID="lblActualI" runat="server" CssClass="default" /></td>
                                                                    <td>&nbsp;</td>
                                                                    <td align="center">$<asp:Label ID="lblVarianceI" runat="server" CssClass="default" /></td>
                                                                    <td>&nbsp;</td>
                                                                    <td align="center"><%= lblVarPIn.Text %></td>
                                                                </tr>
                                                                <tr>
                                                                    <td nowrap>External Labor</td>
                                                                    <td>&nbsp;</td>
                                                                    <td align="center">$<asp:Label ID="lblApprovedE" runat="server" CssClass="default" /></td>
                                                                    <td>&nbsp;</td>
                                                                    <td align="center">$<asp:Label ID="lblActualE" runat="server" CssClass="default" /></td>
                                                                    <td>&nbsp;</td>
                                                                    <td align="center">$<asp:Label ID="lblVarianceE" runat="server" CssClass="default" /></td>
                                                                    <td>&nbsp;</td>
                                                                    <td align="center"><%= lblVarPEx.Text %></td>
                                                                </tr>
                                                                <tr>
                                                                    <td nowrap>Capital (HW / SW)</td>
                                                                    <td>&nbsp;</td>
                                                                    <td align="center">$<asp:Label ID="lblApprovedC" runat="server" CssClass="default" /></td>
                                                                    <td>&nbsp;</td>
                                                                    <td align="center">$<asp:Label ID="lblActualC" runat="server" CssClass="default" /></td>
                                                                    <td>&nbsp;</td>
                                                                    <td align="center">$<asp:Label ID="lblVarianceC" runat="server" CssClass="default" /></td>
                                                                    <td>&nbsp;</td>
                                                                    <td align="center"><%= lblVarPCl.Text %></td>
                                                                    
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="9" style="border-top:solid 1px #CCCCCC"><img src="/images/spacer.gif" border="0" width="1" height="1" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td nowrap align="right"><b>Total:</b></td>
                                                                    <td>&nbsp;</td>
                                                                    <td align="center"><b>$<asp:Label ID="lblApproved" runat="server" CssClass="default" /></b></td>
                                                                    <td>&nbsp;</td>
                                                                    <td align="center"><b>$<asp:Label ID="lblActual" runat="server" CssClass="default" /></b></td>
                                                                    <td>&nbsp;</td>
                                                                    <td align="center"><b>$<asp:Label ID="lblVariance" runat="server" CssClass="default" /></b></td>
                                                                    <td>&nbsp;</td>
                                                                    <td align="center"><b><%= lblVarP.Text %></b></td>                                                                   
                                                                   
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2"><b>Explanation of Variance:</b></td>
                                                    </tr>                                                    
                                                    <tr>
                                                        <td colspan="2"><asp:TextBox ID="txtVariance" runat="server" CssClass="default" Width="400" TextMode="MultiLine" Rows="5" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <table cellpadding="3" cellspacing="2" border="0"  class="offsettable">
                                                                <tr>
                                                                    <td colspan="7" class="greentableheader">Ongoing Financials:</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>&nbsp;</td>
                                                                    <td>&nbsp;</td>
                                                                    <td align="center"><b>Approved</b></td>                                                                     
                                                                </tr>
                                                                <tr>
                                                                    <td nowrap>1st Year Support</td>
                                                                    <td>&nbsp;</td>
                                                                    <td align="center">$ 
                                                                       <asp:TextBox ID="txtSupport" runat="server" CssClass="default"  Width="80" MaxLength="16" />
                                                                       <asp:ImageButton ID="imgtxtSupport" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calculator.gif" />
                                                                   </td>                                                                     
                                                                    <td>&nbsp;</td>                                                               
                                                                </tr>
                                                                <tr>
                                                                    <td nowrap>EPS Infrastructure Chargeback total (Annual)</td>
                                                                    <td>&nbsp;</td>
                                                                    <td align="center">$ 
                                                                       <asp:TextBox ID="txtEPSChargeback" runat="server" CssClass="default"  Width="80" MaxLength="16" />
                                                                       <asp:ImageButton ID="imgtxtEPSChargeback" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calculator.gif" /> 
                                                                    </td>              
                                                                    <td>&nbsp;</td>                                                                     
                                                                </tr>
                                                                <tr>
                                                                    <td nowrap>Software Maintenance</td>
                                                                    <td>&nbsp;</td>
                                                                    <td align="center">$
                                                                       <asp:TextBox ID="txtSwMaintenance" runat="server" CssClass="default"  Width="80" MaxLength="16" />
                                                                       <asp:ImageButton ID="imgtxtSwMaintenance" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calculator.gif" /> 
                                                                    </td>
                                                                    <td>&nbsp;</td>                                                                  
                                                                </tr>
                                                                <tr>
                                                                    <td nowrap>Transaction Costs</td>
                                                                    <td>&nbsp;</td>
                                                                    <td align="center">$ 
                                                                       <asp:TextBox ID="txtTransCosts" runat="server" CssClass="default"  Width="80" MaxLength="16" />
                                                                       <asp:ImageButton ID="imgtxtTransCosts" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calculator.gif" /> 
                                                                    </td>
                                                                    <td>&nbsp;</td>        
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="7" style="border-top:solid 1px #CCCCCC"><img src="/images/spacer.gif" border="0" width="1" height="1" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td nowrap align="right"><b>Total:</b></td>
                                                                    <td>&nbsp;</td>
                                                                    <td align="left"><b>$ <asp:TextBox ID="txtFinancialTotal" runat="server" CssClass="default" width="80" ReadOnly="True"/></b></td>                                                                                                                                                                                                                                                                                       
                                                                </tr>
                                                                <tr> 
                                                                    <td colspan="6" align="right"> <asp:Button ID="btnCalcTotal" runat="server" CssClass="default" Width="100" Text="Calculate Total" /></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                           <table width="90%" cellpadding="5" cellspacing="0" border="0" style="display:inline" class="offsettable">
                                                                <tr>
                                                                    <td colspan="7" class="greentableheader">Project Schedule:</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>&nbsp;</td>
                                                                    <td align="center" colspan="3"><b>Approved C1 and PCRs Dates</b></td>
                                                                    <td align="center" colspan="3"><b>Actual/Forecast Dates</b></td>
                                                                    <td align="center" rowspan="2"><b>Variance<br /># Days</b></td>
                                                                    <td align="center" rowspan="2"><b>Variance<br />%</b></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>&nbsp;</td>
                                                                    <td align="center"><b>Start Date</b></td>
                                                                    <td>&nbsp;</td>
                                                                    <td align="center"><b>End Date</b></td>
                                                                    <td align="center"><b>Start Date</b></td>
                                                                    <td>&nbsp;</td>
                                                                    <td align="center"><b>End Date</b></td>
                                                                </tr>
                                                                <tr>
                                                                    <td nowrap>Discovery Phase:</td>
                                                                    <td align="center" nowrap><%= txtAppSD.Text == "" ? "---" : txtAppSD.Text %></td>
                                                                    <td align="center" width="1">-</td>
                                                                    <td align="center"><%= txtAppED.Text == "" ? "---" : txtAppED.Text %></td>
                                                                    <td align="center" nowrap><%= txtStartD.Text == "" ? "---" : txtStartD.Text %></td>
                                                                    <td align="center" width="1">-</td>
                                                                    <td align="center"><%= txtEndD.Text == "" ? "---" : txtEndD.Text %></td>
                                                                    <td align="center"><%= lblVarianceDaysD.Text %></td>
                                                                    <td align="center"><%= lblVariancePercentD.Text %></td>
                                                                </tr>
                                                                <tr>
                                                                    <td nowrap>Planning Phase:</td>
                                                                    <td align="center" nowrap><%= txtAppSP.Text == "" ? "---" : txtAppSP.Text %></td>
                                                                    <td align="center" width="1">-</td>
                                                                    <td align="center"><%= txtAppEP.Text == "" ? "---" : txtAppEP.Text %></td>
                                                                    <td align="center" nowrap><%= txtStartP.Text == "" ? "---" : txtStartP.Text%></td>
                                                                    <td align="center" width="1">-</td>
                                                                    <td align="center"><%= txtEndP.Text == "" ? "---" : txtEndP.Text%></td>
                                                                    <td align="center"><%= lblVarianceDaysP.Text %></td>
                                                                    <td align="center"><%= lblVariancePercentP.Text %></td>
                                                                </tr>
                                                                <tr>
                                                                    <td nowrap>Execution Phase:</td>
                                                                    <td align="center" nowrap><%= txtAppSE.Text == "" ? "---" : txtAppSE.Text%></td>
                                                                    <td align="center" width="1">-</td>
                                                                    <td align="center"><%= txtAppEE.Text == "" ? "---" : txtAppEE.Text %></td>
                                                                    <td align="center" nowrap><%= txtStartE.Text == "" ? "---" : txtStartE.Text%></td>
                                                                    <td align="center" width="1">-</td>
                                                                    <td align="center"><%= txtEndE.Text == "" ? "---" : txtEndE.Text%></td>
                                                                    <td align="center"><%= lblVarianceDaysE.Text %></td>
                                                                    <td align="center"><%= lblVariancePercentE.Text %></td>
                                                                </tr>
                                                                <tr>
                                                                    <td nowrap>Close Phase:</td>
                                                                    <td align="center" nowrap><%= txtAppSC.Text == "" ? "---" : txtAppSC.Text %></td>
                                                                    <td align="center" width="1">-</td>
                                                                    <td align="center"><%= txtAppEC.Text == "" ? "---" : txtAppEC.Text %></td>
                                                                    <td align="center" nowrap><%= txtStartC.Text == "" ? "---" : txtStartC.Text %></td>
                                                                    <td align="center" width="1">-</td>
                                                                    <td align="center"><%= txtEndC.Text == "" ? "---" : txtEndC.Text %></td>
                                                                    <td align="center"><%= lblVarianceDaysC.Text %></td>
                                                                    <td align="center"><%= lblVariancePercentC.Text %></td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="9" style="border-top:solid 1px #CCCCCC"><img src="/images/spacer.gif" border="0" width="1" height="1" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="default"><b>PROJECT TOTALS:</b></td>
                                                                    <td align="center"><%= lblApprovedStart.Text %></td>
                                                                    <td align="center">-</td>
                                                                    <td align="center"><%= lblApprovedEnd.Text %></td>
                                                                    <td align="center"><%= lblProjectStart.Text %></td>
                                                                    <td align="center">-</td>
                                                                    <td align="center"><%= lblProjectEnd.Text %></td>
                                                                    <td align="center"><%= lblVarianceDays.Text%></td>
                                                                    <td align="center"><%= lblVariancePercent.Text%></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <!--
                                                    <tr>
                                                        <td colspan="2">
                                                            <table cellpadding="3" cellspacing="2" border="0"  class="offsettable">
                                                                <tr>
                                                                    <td colspan="7" class="greentableheader">Overall Schedule:</td>
                                                                </tr>
                                                                <tr>
                                                                    <td>&nbsp;</td>
                                                                    <td>&nbsp;</td>
                                                                    <td align="center"><b>Approved</b></td>
                                                                    <td>&nbsp;</td>
                                                                    <td align="center"><b>Actual</b></td>
                                                                    <td>&nbsp;</td>
                                                                    <td align="center"><b>Variance</b></td>
                                                                </tr>
                                                                <tr>
                                                                    <td nowrap>Start Date</td>
                                                                    <td>&nbsp;</td>
                                                                    <td align="center"><asp:Label ID="lblApprovedS" runat="server" CssClass="default" /></td>
                                                                    <td>&nbsp;</td>
                                                                    <td align="center"><asp:Label ID="lblActualS" runat="server" CssClass="default" /></td>
                                                                    <td>&nbsp;</td>
                                                                    <td align="center"><asp:Label ID="lblVarianceS" runat="server" CssClass="default" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td nowrap>End Date</td>
                                                                    <td>&nbsp;</td>
                                                                    <td align="center"><asp:Label ID="lblApprovedF" runat="server" CssClass="default" /></td>
                                                                    <td>&nbsp;</td>
                                                                    <td align="center"><asp:Label ID="lblActualF" runat="server" CssClass="default" /></td>
                                                                    <td>&nbsp;</td>
                                                                    <td align="center"><asp:Label ID="lblVarianceF" runat="server" CssClass="default" /></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    -->
                                                    <tr>
                                                        <td colspan="2"><b>Results Better than Expected:</b></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2"><asp:TextBox ID="txtBetter" runat="server" CssClass="default" Width="400" TextMode="MultiLine" Rows="5" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2"><b>Results Worse than Expected:</b></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2"><asp:TextBox ID="txtWorse" runat="server" CssClass="default" Width="400" TextMode="MultiLine" Rows="5" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2"><b>Lessons Learned:</b></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2"><asp:TextBox ID="txtLessons" runat="server" CssClass="default" Width="400" TextMode="MultiLine" Rows="5" /></td>
                                                    </tr>
                                                    <tr><td colspan="2">&nbsp;</td></tr>   
                                                    <tr>
                                                       <td colspan="2" align="right"><asp:Button ID="btnGenerateCloseForm" runat="server" Text="Generate Closure Form" CssClass="default" Width="150" OnClick="Generate_PDF" /></td>                                                                                                               
                                                    </tr>                                                
                                                    <tr>
                                                        <td colspan="2">
                                                           <table width="100%" cellpadding="2" cellspacing="0" border="0" class="default">
                                                              <tr bgcolor="EEEEEE">
                                                                  <td><b>PDF</b></td>
                                                                  <td><b>Modified Date</b></td>
                                                                  <td><b>Route?</b></td>
                                                              </tr>
                                                             <asp:Repeater ID="rptClosurePDF" runat="server">
                                                              <ItemTemplate>
                                                               <tr>
                                                                  <td valign="top" nowrap><a href="<%# oVariable.URL() +"/"+ DataBinder.Eval(Container.DataItem,"path") %>" target="_blank"><%# DataBinder.Eval(Container.DataItem,"path").ToString().Substring(DataBinder.Eval(Container.DataItem,"path").ToString().IndexOf("\\")+1) %></a></td>
                                                                  <td valign="top"><%# DataBinder.Eval(Container.DataItem,"modified") %></td>
                                                                  <td valign="top"><input type="checkbox" name="chkPDF[]" value="<%# DataBinder.Eval(Container.DataItem,"id")%>" onclick="UpdateHidden2('chkPDF[]','hdnClosurePDFCheck');" /></td>
                                                               </tr>
                                                              </ItemTemplate>                                                             
                                                             </asp:Repeater>                                                    
                                                             <tr> 
                                                                  <td colspan="2">
                                                                     <asp:Label ID="lblNoClosurePDF" runat="server" CssClass="default" Visible="false" Text="<img src='/images/alert.gif' border='0' align='absmiddle'> There are no PDF(s) generated ... " />
                                                                  </td>                                                                
                                                             </tr>                                                                                                                   
                                                           </table>
                                                        </td>
                                                    </tr>  
                                                    <tr>
                                                       <td colspan="2" align="right"><asp:Button ID="btnRouteClosurePDF" runat="server" Text="Route for Notification" CssClass="default" Width="150" /></td>                                                                                                               
                                                    </tr>                         
                                                  
                                                    
                                                </table>
		                                    </div>
                                            <div id="divInvolvement"  style='<%=boolResource == true ? "display:inline" : "display:none" %>'>
                                                <br />
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
                                                        <td colspan="2" class="greentableheader">Send Communication</td>
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
                                            </div>
		                                    <div id="divMilestones" style='<%=boolMilestones == true ? "display:inline" : "display:none" %>'>
		                                        <br />
                                                <table cellpadding="5" cellspacing="0" border="0">
                                                    <tr>
                                                        <td nowrap><b>Original Date:</b><font class="required">&nbsp;*</font></td>
                                                        <td nowrap><asp:TextBox ID="txtMilestoneApproved" runat="server" CssClass="default" Width="100" MaxLength="10" /> <asp:ImageButton ID="imgMilestoneApproved" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
                                                        <td align="right" valign="top" rowspan="2"><asp:CheckBox ID="chkComplete" runat="server" CssClass="bold" Text="Milestone Completed" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap><b>Forecasted Date:</b><font class="required">&nbsp;*</font></td>
                                                        <td nowrap><asp:TextBox ID="txtMilestoneForecasted" runat="server" CssClass="default" Width="100" MaxLength="10" /> <asp:ImageButton ID="imgMilestoneForecasted" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap><b>Milestone:</b><font class="required">&nbsp;*</font></td>
                                                        <td nowrap colspan="2"><asp:TextBox ID="txtMilestone" runat="server" CssClass="default" Width="400" MaxLength="100" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap valign="top"><b>Description:</b></td>
                                                        <td nowrap colspan="2"><asp:TextBox ID="txtDetail" runat="server" CssClass="default" Width="500" TextMode="MultiLine" Rows="7" /></td>
                                                    </tr>
                                                </table>
                                                <br /><br />
                                                <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                                    <tr>
                                                        <td>
                                                            <table width="100%" cellpadding="3" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                                                                <tr bgcolor="#EEEEEE">
                                                                    <td>&nbsp;</td>
                                                                    <td>&nbsp;</td>
                                                                    <td nowrap><b>Original Date</b></td>
                                                                    <td>&nbsp;</td>
                                                                    <td nowrap><b>Forecasted Date</b></td>
                                                                    <td>&nbsp;</td>
                                                                    <td nowrap><b>Milestone</b></td>
                                                                    <td nowrap><b>Completed</b></td>
                                                                </tr>
                                                                <asp:repeater ID="rptMilestones" runat="server">
                                                                    <ItemTemplate>
                                                                        <tr class="default">
                                                                            <td nowrap>[<a href="javascript:void(0);" onclick="OpenWindow('MILESTONE','<%# DataBinder.Eval(Container.DataItem, "id") %>');">Edit</a>]</td>
                                                                            <td>&nbsp;</td>
                                                                            <td nowrap><%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "approved").ToString()).ToShortDateString() %></td>
                                                                            <td>&nbsp;</td>
                                                                            <td nowrap><%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "forecasted").ToString()).ToShortDateString() %></td>
                                                                            <td>&nbsp;</td>
                                                                            <td width="100%"><%# DataBinder.Eval(Container.DataItem, "milestone") %></td>
                                                                            <td nowrap align="center"><img src='/images/<%# DataBinder.Eval(Container.DataItem, "complete").ToString() == "1" ? "check" : "cancel" %>.gif' border='0' align='absmiddle' /></td>
                                                                        </tr>
                                                                    </ItemTemplate>
                                                                </asp:repeater>
                                                            <tr>
                                                                <td colspan="8">
                                                                    <asp:Label ID="lblNoMilestone" runat="server" CssClass="default" Visible="false" Text="<img src='/images/alert.gif' border='0' align='absmiddle'> There are no milestones" />
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
<asp:Panel ID="panFinish" runat="server" Visible="false">
    <table width="100%" cellpadding="2" cellspacing="2" border="0">
        <tr>
            <td><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
            <td class="header"><img src="/images/bigCheck.gif" border="0" align="absmiddle" /> Record Updated</td>
        </tr>
        <tr><td colspan="2">&nbsp;</td></tr>
        <tr>
            <td><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
            <td>Your information has been saved successfully.</td>
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
                        <td align="right"><asp:Button ID="btnFinish" runat="server" CssClass="default" Width="75" Text="Finish" /></td>
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
<asp:HiddenField ID="hdnExecutive" runat="server" />
<asp:HiddenField ID="hdnWorking" runat="server" />
<asp:HiddenField ID="hdnCSRCD" runat="server" />
<asp:HiddenField ID="hdnPCRD" runat="server" />
<asp:HiddenField ID="hdnCSRCC" runat="server" />
<asp:HiddenField ID="hdnPCRC" runat="server" />
<asp:HiddenField ID="hdnTab" runat="server" />
<asp:HiddenField ID="hdnCosts" runat="server" />
<asp:HiddenField ID="hdnSegment" runat="server" />
<asp:HiddenField ID="hdnID" runat="server" />

 
<asp:HiddenField ID="hdnLead" runat="server" />
<asp:HiddenField ID="hdnD" runat="server" />
<asp:HiddenField ID="hdnCC" runat="server" />
<asp:HiddenField ID="hdnAcc" runat="server" />

<input type="hidden" id="hdnStatus" />  
<input type="hidden" id="hdnPCRCheck" />  
<input type="hidden" id="hdnCSRCCheck" />  
<input type="hidden" id="hdnClosurePDFCheck" />
<input type="hidden" id="hdnAccomplishment" /> 
