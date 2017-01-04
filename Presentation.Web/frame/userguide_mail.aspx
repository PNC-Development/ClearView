<%@ Page Language="C#" MasterPageFile="~/frame.Master" AutoEventWireup="true" CodeBehind="userguide_mail.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.userguide_mail" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" Runat="Server">  
<script type="text/javascript">
var oXid = null;
var strXid ="";
var oValues = null;
window.onload = function Load() {
 oXid = document.getElementById('<%= hdnXid.ClientID %>');  
 oValues = document.getElementById('<%= hdnValues.ClientID %>');  
}

// Setup an array to hold our list
var RecipientArray = new Array();
var XidArray = new Array();
Array.prototype.contains = function (element) 
  {
          for (var i = 0; i < this.length; i++) 
       {             
              if (this[i] == element) 
              {                 
                      return true;
              }
        }
          return false;
  };   
  
 
// This function is what rebuilds the list. It also generates the hidden form fields
// that allow you to do your server side processing with the list.
function showRecipients() {
	var out = '';	
	 
	// loop over each item in the array to create the HTML code below.
	for (i = 0; i < RecipientArray.length; i++) {
		// create a link letting you remove items from the list, escape the list item, 
		// if link clicked then call removeRecipient to get rid of the item
		
		// This is the hidden field that sends the list back to the server. The name of 
		// the input ends in [] so that PHP will treat it like an array of items
		out += '<input name="sRecipientList[]" type="hidden" value="' + RecipientArray[i] +'"> ';
		// unescape the item for display			
		out += unescape(RecipientArray[i]);
		
		out += '<a href="javascript:removeRecipient(\'' + escape(RecipientArray[i]) +'\')"'; 
		// Do a confirm before deleting the item
		out += 'onClick="if(confirm(\'Are you sure you want to delete this recipient?\')) return true; else return false;">';
		out += ' (delete)</a> ';
		
		out += '<a href="javascript:editRecipient(\'' + escape(RecipientArray[i]) +'\')">'; 		 
		out += ' (edit)</a> ';
		
		
		// space out the list
		// with some more code you could also make this into an actual <ul> list
		out += ' <br>';		
		strXid = RecipientArray[i].substring(RecipientArray[i].indexOf("%28")).replace("%28","").replace("%29",""); // Replace "(" and ")" and get the LAN ID alone
		if(!XidArray.contains(strXid))
		  XidArray[XidArray.length]= strXid; 	
	  	 
	}
	oValues.value = XidArray.slice(0);
 

	// Here's the magic, in the code of your page you'll have a <span> (or other element) 
	// with a unique ID. What we do here is grab that element and change the contents of 
	// it to the html we just created above which is in the out variable.
	document.getElementById('<%= SpanRecipient.ClientID %>').innerHTML = out;
	document.getElementById('<%= hdnRecipient.ClientID %>').value = document.getElementById('<%= SpanRecipient.ClientID %>').innerText;
	
; 
	
}

// Adds an item to the array
function addRecipient(Recipient) {
	// if the item is not an empty string
	Recipient = document.getElementById(Recipient);		
		
	 
	if(Recipient.value != ''){
		// Add it to the array		
		RecipientArray[RecipientArray.length] = escape(Recipient.value);
		// Clear out the form field
		//document.categoryform.RecipientInput.value = '';
		Recipient.value ='';
		// Redisplay the list
		showRecipients();
	}
	else alert('Please enter a recipient');
		 
	document.getElementById('<%=txtTo.ClientID %>').focus();	 
	return false;
}

// Remove an item from the list
function removeRecipient(Recipient) {
	// temp array
	var Recipients = new Array();
	// Loop over current array
	for (i = 0; i < RecipientArray.length; i++) {
		// If the item doesn't match the one we're trying to delete then
		// add it to the temp array
		if (RecipientArray[i] != Recipient) {
			Recipients[Recipients.length] = RecipientArray[i];
		}
		else { 
		 if(RecipientArray[i] == Recipient)
		    XidArray.splice(i,1);
		}
	}
	// Set the item array to our temp array
	RecipientArray = Recipients;
	// Redisplay the list
	showRecipients();
}

function editRecipient(Recipient) {
	// temp array
	var Recipients = new Array();
	// Loop over current array
	for (i = 0; i < RecipientArray.length; i++) {
		// If the item doesn't match the one we're trying to delete then
		// add it to the temp array
		if (RecipientArray[i] == Recipient) {
			 document.getElementById('<%=txtTo.ClientID %>').value = unescape(Recipient);			 			  
			 document.getElementById('hdnPosition').value = i;
		}		 
		Recipients[Recipients.length] = RecipientArray[i];
		
	}
	// Set the item array to our temp array
	RecipientArray = Recipients;
	// Redisplay the list
	showRecipients();
}

function updateRecipient(Recipient,oHidden)  
{   
   Recipient = document.getElementById(Recipient);	
   oHidden = document.getElementById(oHidden);
   if(Recipient.value == '') 
     alert('Nothing to update');    
   else if(oHidden.value == '') 
     alert('Item not in list');  
   else
   {    
       RecipientArray[oHidden.value] = escape(Recipient.value);
       Recipient.value ='';
       strXid = RecipientArray[oHidden.value].substring(RecipientArray[oHidden.value].indexOf("%28")).replace("%28","").replace("%29","");
       XidArray[oHidden.value] = strXid;   
       showRecipients();
   }
   return false;
}
</script>
 <table width="100%" cellpadding="2" cellspacing="2" border="0">
    <tr> 
       <td class="default"><b>To:</b></td>              
       <td nowrap width="100%"><asp:TextBox ID="txtTo" runat="server" CssClass="default" Width="200" /> <asp:Button ID="btnAdd" runat="Server" CssClass="default" Width="75" Text="Add" /> <asp:Button ID="btnUpdate" runat="Server" CssClass="default" Width="75" Text="Update" /></td>                
    </tr>
    <tr>
       <td width="100%">
           <div id="divTo" runat="server" style="overflow: hidden; position: absolute; display: none;
                 background-color: #FFFFFF; border: solid 1px #CCCCCC">
                 <asp:ListBox ID="lstTo" runat="server" CssClass="default" />
           </div>
       </td>
    </tr>           
    <tr>
       <td class="default"><b>Comments:</b></td> 
       <td width="100%"><asp:TextBox ID="txtComments" runat="server" CssClass="default" TextMode="multiLine" Width="350" Rows="5" /></td> 
    </tr>
    <tr>
       <td class="greentableheader"><b>Recipient(s):</b></td>
    </tr>
    <tr>
       <td colspan="2">
          <table width="100%" cellpadding="2" cellspacing="0" border="0" style="border: dashed 1px #CCCCCC">
             <tr><td bgcolor="#EEEEEE" class="default"><b>Recipient Name</b></td>
             </tr> 
             <tr>
                <td width="100%" valign="top"><span id="SpanRecipient" runat="server" class="default"></span></td> 
             </tr>                                          
          </table>
       </td>                                
    </tr>   
    <tr><td>&nbsp;</td></tr>         
       <% = strAttachement %>         
    <tr>
       <td>&nbsp;</td> 
       <td align="right"><asp:Button ID="btnSend" runat="server" Text="Send" CssClass="default" Width="100" OnClick="btnSend_Click" /></td>
   </tr>   
 </table>
<asp:HiddenField ID="hdnTo" runat="server" /> 
<asp:HiddenField ID="hdnRecipient" runat="server" />
<asp:HiddenField ID="hdnXid" runat="server" />
<asp:HiddenField ID="hdnValues" runat="server" /> 
<input type="hidden" id="hdnPosition" />
</asp:Content>