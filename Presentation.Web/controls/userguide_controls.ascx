<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="userguide_controls.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.userguide_controls" %>


<script type="text/javascript">
  var oHidden1 = null;   
     
window.onload = function Load() {
   oHidden1 = document.getElementById('hdnModule');              
}
 function ValidateTreeNodeSelection() {
 
    if(oHidden1.value == "") {
      alert('Please select at least one user guide');
      return false;
    }
    else
    return OpenWindow('USER_GUIDE','?ids='+document.getElementById('hdnModule').value);    
    
 }

 function OnTreeClick(evt)  {    
    var src = window.event != window.undefined ? window.event.srcElement : evt.target;               
    var nodeClick = src.tagName.toLowerCase() == "a";        
    var imgClick = src.tagName.toLowerCase() == "img";   
    var nodeClick = src.tagName.toLowerCase() == "input"; 
    if (nodeClick) 
    {
        if (src.checked == true)
            oHidden1.value += src.nextSibling.title +":";             
         else
            oHidden1.value = oHidden1.value.replace(src.nextSibling.title+":","");          
    }           
       // return false; //uncomment this if you do not want postback on node click
  }    
</script>
<table width="100%" cellpadding="0" cellspacing="0" border="0" class="default">
    <tr>
        <td><img src="/images/table_topleft.gif" border="0" width="5" height="26"></td>
        <td nowrap background="/images/table_top.gif" class="greentableheader" width="100%"><asp:Label ID="Label1" runat="server" CssClass="greentableheader" Text="User Guides" /></td>
        <td><img src="/images/table_topright.gif" border="0" width="5" height="26"></td>
    </tr>
    <tr>
        <td background="/images/table_left.gif"><img src="/images/table_left.gif" width="5" height="10"></td>      
        <td bgcolor="#FFFFFF" valign="top">
            <br />              
              <table width="100%" cellpadding="2" cellspacing="2" border="0">               
                <tr>
                    <td valign="top" class="default" nowrap><asp:Button ID="btnSend" runat="server" CssClass="default" Text="Email Selected Guide(s)" Width="200" /></td> 
                </tr>
                <tr>                     
                    <td>
                      <asp:TreeView ID="oTreeview" runat="server" CssClass="default" ShowCheckBoxes="Leaf" ShowLines="true" NodeIndent="35">
                         <NodeStyle  CssClass="default" />
                      </asp:TreeView>
                    </td>
                </tr>                                  
              </table>             
        </td>
        <td background="/images/table_right.gif"><img src="/images/table_right.gif" width="5" height="10"></td>
    </tr>
    <tr>
        <td><img src="/images/table_bottomLeft.gif" border="0" width="5" height="9"></td>
        <td width="100%" background="/images/table_bottom.gif"></td>
        <td><img src="/images/table_bottomRight.gif" border="0" width="5" height="9"></td>
    </tr>
</table>
<input type="hidden" id="hdnModule" />