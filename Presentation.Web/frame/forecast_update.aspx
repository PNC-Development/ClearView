<%@ Page Language="C#" MasterPageFile="~/window.Master" AutoEventWireup="true" CodeBehind="forecast_update.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.forecast_update" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" Runat="Server"> 
<script type="text/javascript">   
  var trValid = null;
  var trDate = null;
  var trComplete = null;
  var txtDate =  null;
  
  window.onload = function Load() {     
     trValid = document.getElementById('trValid');
     trDate = document.getElementById('trDate');  
     trComplete = document.getElementById('trComplete');  
     txtDate = document.getElementById('<%= txtCommitmentDate.ClientID %>');               
  }
  function RadioButtonListClick(rbl)
  {
     rbl = document.getElementById(rbl); 
     var value = GetValue(rbl); // Get the selected value in the radiobutton list
     if(rbl.id.match('rblComplete') !=null) {  // User selects (Yes/No) option in complete radiobutton list 
        trValid.style.display = value == 0? "inline":"none";  // Show valid radiobutton list only if user chooses 0 (No) option in complete radiobutton
        trDate.style.display = "none"; // Hide commitment date calendar           
        txtDate.value ="";     // Clear out date value if any
        ResetInput(document.getElementById('<%= rblValid.ClientID %>')); // Reset input for valid radiobutton list       
     }
     else { // User selects (Yes/No) option in valid radiobutton list
        trDate.style.display = value == 1? "inline":"none"; // Show commitment date calendar only if user chooses 1 (Yes) in valid radiobutton list     
     }      
  }
  
  function GetValue(radioList){
    var options = radioList.getElementsByTagName('input');
    for(i=0;i<options.length;i++){
      var opt = options[i];
      if(opt.checked){
        return opt.value;
      }
    }
  }
  
  function ResetInput(radioList){
    var options = radioList.getElementsByTagName('input');
    for(i=0;i<options.length;i++){
      var opt = options[i];
      if(opt.checked) opt.checked = false;
    }
  }
  
  /*
   Working of function ValidateStatusUpdate
   ===============================================
   
   If user chooses Yes from complete radiobutton hide the valid radiobutton(if visible) and clear out the commitment
   date field and stop the process.
   else
   {
     Display valid radiobutton.
     If user chooses Yes then display Commitment date calendar.
     else
     {
        Hide commitment date calendar and also clear out the date value if any.                        
     }      
   }   
  
  */
  
  function ValidateStatusUpdate(rblComplete,rblValid)
  {
     var validated = false;
     if(ValidateRadioList(rblComplete,'Please check if design is complete or not')) { // Validate complete radiobutton list
       rblComplete = document.getElementById(rblComplete);          
       var value = GetValue(rblComplete);  // Get the selected value       
       if(value == 0) {  // If Design not complete, show Valid radiobutton
         validated = ValidateRadioList(rblValid,'Please check if design is valid or not'); // Validate valid radiobutton list            
         if(validated) {  // Both 'complete' and 'valid' radiobuttons  are validated, Proceed      
            rblValid = document.getElementById(rblValid);
            value = GetValue(rblValid);  // Get the selected value from 'valid' radiobutton list             
            if(value == 1)  // If valid design               
               validated = ValidateDate('<%= txtCommitmentDate.ClientID %>','Please enter or select a valid commitment date');// Commitment date is already made visible; validate the selected commitment date          
            else {  // invalid design
               validated = true; // Set validation flag to valid                               
               txtDate.value ="";  // Clear out date value if any 
            } // if(value == 1)   
         }// if(validated)
       }// if(value == 0)
       else
           validated = true; // Set validation valid
     } // if(ValidateRadioList(rblComplete,'Please check if design is complete or not'))        
     return validated;      
  }

</script>
  <table width="100%" cellpadding="0" cellspacing="2" border="0" align="center">  
    <tr>
       <td rowspan="2"><img src="/images/forecast2.gif" border="0" align="absmiddle" /></td>
       <td class="header" width="100%" valign="bottom">Design Details</td>
    </tr>
    <tr>
       <td>&nbsp;</td>
    </tr> 
    <tr>
       <td colspan="2"><%= strHTML %></td>
    </tr>  
    <tr>
       <td colspan="2"><hr size="1" noshade /></td>
    </tr>
    <tr>
       <td colspan="2" align="center" class="bigcheck">
          <asp:Label ID="lblSaved" runat="server" Text="" Visible="false" />
       </td>
    </tr>
    <asp:Panel ID="panUpdate" runat="server">                    
    <tr>
       <td colspan="2">
          <table width="100%" cellpadding="0" cellspacing="3" border="0">
             <tr>
                 <td rowspan="2"><img src="/images/status.gif" border="0" align="absmiddle" /></td>
                 <td class="header" width="100%" valign="bottom">Design Status Update</td>
             </tr>
             <tr>
                <td width="100%" valign="top" class="note">The commitment date for this design is outdated by more than 30 days. Please answer the following question(s) to update the design status.</td>
             </tr>              
             <tr>
                 <td colspan="2">                 
                    <table width="100%" cellpadding="2" cellspacing="0" border="0">
                       <tr>
                          <td nowrap><b>Is this design complete (i.e.Did you receive the hardware) ?:</b></td>
                          <td width="100%">
                            <asp:RadioButtonList ID="rblComplete" runat="server" RepeatDirection="Horizontal" CssClass="default">
                               <asp:ListItem Text="Yes" Value="1" /> 
                               <asp:ListItem Text="No"  Value="0" /> 
                            </asp:RadioButtonList>
                          </td>                         
                       </tr>
                       <tr id="trValid" style="display:none">
                          <td nowrap><b>Is this design still valid?:</b></td> 
                          <td width="100%">  
                            <asp:RadioButtonList ID="rblValid" runat="server" RepeatDirection="Horizontal" CssClass="default">
                               <asp:ListItem Text="Yes" Value="1" /> 
                               <asp:ListItem Text="No"  Value="0" /> 
                            </asp:RadioButtonList>                    
                          </td>
                       </tr>                                          
                       <tr id="trDate" style="display:none">
                          <td nowrap><b>New Commitment Date:</b></td> 
                          <td width="100%"><asp:TextBox ID="txtCommitmentDate" runat="server" CssClass="default" Width="100"/> <asp:ImageButton ID="imgCommitmentDate" runat="server" CssClass="default" ImageUrl="/images/calendar.gif" ImageAlign="AbsMiddle" /></td> 
                       </tr>    
                       <tr>
                          <td nowrap><b>Comments:</b></td>
                          <td width="100%"><asp:TextBox ID="txtComments" runat="server" Width="400" Rows="5" TextMode="MultiLine" CssClass="default" /></td>
                       </tr>                         
                       <tr>
                          <td colspan="2" width="100%" align="right"><asp:Button ID="btnSave" runat="server" Text="Save" CssClass="default" Width="75" OnClick="btnSave_Click" /></td>
                       </tr>  
                    </table>
                 </td>                
             </tr>                         
          </table>         
       </td>     
    </tr>   
    </asp:Panel>    
  </table>     
</asp:Content>