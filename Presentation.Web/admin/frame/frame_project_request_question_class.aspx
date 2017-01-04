<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frame_project_request_question_class.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.frame_project_request_question_class" %>



<script type="text/javascript">
  var oHidden1 = null;
  var oHidden2 = null;   
  var strParent = null;
  var oHiddenTest = null;
   
  function Add(ddlClass,ddlWeight)  {        
    
    oHiddenTest.value =ddlClass.options[ddlClass.selectedIndex].value +"&" +ddlClass.options[ddlWeight.selectedIndex].value
//    var valid=true; 
//     if(ddlClass.selectedIndex > 0 && ddlWeight.selectedIndex > 0)           
//       oHiddenTest.value =ddlClass.options[ddlClass.selectedIndex].value +"&" +ddlClass.options[ddlWeight.selectedIndex].value
//     else
//     {
//        if(ddlClass.selectedIndex == 0)  alert('Please select a class');
//        if(ddlWeight.selectedIndex == 0) alert('Please select a weight');            
//        valid=false;
//     }   
//     return true;      
  } 
   
  function Load()
  {   
        oHidden1 = document.getElementById('<%=hdnParent.ClientID %>');          
        oHidden2 = document.getElementById('hdnOrganizationID');     
        oHiddenTest = document.getElementById('<%=hdnTest.ClientID %>');         
  }

</script>
<html>
<head>
<title>Web Content Management Administration</title>
<link rel="stylesheet" type="text/css" href="/css/default.css" />
<script type="text/javascript" src="/javascript/global.js"></script>  
</head>
<body topmargin="0" leftmargin="0" onload="Load();">
<form id="Form1" runat="server">
<table width="100%" height="100%" border="0" cellSpacing="0" cellPadding="0">
    <tr height="1">
        <td>
            <table width="100%" border="0" cellSpacing="2" cellPadding="2" bgcolor="#e6e9f0">
	            <tr bgcolor="#e6e9f0">
		            <td nowrap><b>Project Request Question Class</b></td>
	                <td align="right">
			            <a href="javascript:void(0);" onclick="parent.HidePanel();"><img src="/admin/images/close.gif" border="0" title="Close"></a>
	                </td>
	            </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td>  
          <div style="height:100%; width:100%; overflow:auto;"> 
          <table width="100%" height="100%" border="0" cellpadding="2" cellspacing="2" bgcolor="#c8cfdd">
                    <tr>
                        <td valign="top" class="default">
                            <asp:TreeView ID="oTreeview" runat="server" ShowLines="true"  ShowCheckBoxes="All" NodeIndent="35">
                                <NodeStyle CssClass="default" />
                            </asp:TreeView>
                        </td>
                    </tr>
            </table>              
          </div>        
        </td>
    </tr>
    <tr height="1">
        <td align="right" bgcolor="#c8cfdd">     
            <asp:Button ID="btnSave" runat="server" Text="Save" Width="75" CssClass="default" OnClick="btnSave_Click"/>         
            <asp:Button ID="btnClose" runat="server" Text="Close" Width="75" CssClass="default" />
        </td>
    </tr> 
      <tr><td height="5" bgcolor="#c8cfdd"><img src="images/spacer.gif" width="1" height="5" /></td></tr>    
</table>
<asp:Label ID="lblId" runat="server" Visible="false" />
<asp:Label ID="lblType" runat="server" Visible="false" />
<input type="hidden" runat="server" id="hdnId" />
<input id="hdnUpdateOrder" name="hdnUpdateOrder" type="hidden"/>
<input id="hdnParent" name="hdnParent" type="hidden" runat="server" />
<input id="hdnOrganizationID" name="hdnOrganizationID" type="hidden"/>
<input id="hdnTest" name="hdnTest" type="hidden" runat="server" />
</form>
</body>
</html>
