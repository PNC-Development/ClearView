<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frame_project_request_qa.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.frame_project_request_qa" %>

<script type="text/javascript">
  var oHidden1 = null;
  var oHidden2 = null;   
  var strParent = null;
   
  function Edit(strOrganizationId, strQuestionId,strParent) {    
         oHidden1.value = strParent +"&" + strOrganizationId;       
    }
    
    function MoveOrderUp(ddl) {
	    if (ddl.selectedIndex > -1) {
		    var oldIndex = ddl.selectedIndex - 1;
		    if (oldIndex > -1) {

			    var oldValue = ddl.options[ddl.selectedIndex].value;
			    var oldText = ddl.options[ddl.selectedIndex].text;
			    ddl.options[ddl.selectedIndex].value = ddl.options[ddl.selectedIndex - 1].value;
			    ddl.options[ddl.selectedIndex].text = ddl.options[ddl.selectedIndex - 1].text;
			    ddl.options[ddl.selectedIndex - 1].value = oldValue;
			    ddl.options[ddl.selectedIndex - 1].text = oldText;
			    ddl.selectedIndex = oldIndex;
			    UpdateOrder(ddl);
		    }
		    else
			    alert('You cannot move this item');
	    }
	    else
		    alert('Please select an item to move');
	    return false;
    }
    function MoveOrderDown(ddl) {
	    if (ddl.selectedIndex > -1) {
		    var oldIndex = ddl.selectedIndex + 1;
		    if (oldIndex < ddl.length) {
			    var oldValue = ddl.options[ddl.selectedIndex].value;
			    var oldText = ddl.options[ddl.selectedIndex].text;
			    ddl.options[ddl.selectedIndex].value = ddl.options[ddl.selectedIndex + 1].value;
			    ddl.options[ddl.selectedIndex].text = ddl.options[ddl.selectedIndex + 1].text;
			    ddl.options[ddl.selectedIndex + 1].value = oldValue;
			    ddl.options[ddl.selectedIndex + 1].text = oldText;
			    ddl.selectedIndex = oldIndex;
			    UpdateOrder(ddl);
		    }
		    else
			    alert('You cannot move this item');
	    }
	    else
		    alert('Please select an item to move');
	    return false;
    }
    function UpdateOrder(oDDL) {
	    var oHidden = document.getElementById("hdnUpdateOrder");
	    oHidden.value = "";
	    for (var ii=0; ii<oDDL.length; ii++) {
		    oHidden.value = oHidden.value + oDDL.options[ii].value + "&";
	    }
    }
    function Update(oHidden, strControl) {
        oHidden = document.getElementById(oHidden);
        window.top.UpdateWindow(oHidden.value, strControl, null, null);
        window.top.HidePanel();
        return false;
    }
    function Load()
    {   
        oHidden1 = document.getElementById('<%=hdnParent.ClientID %>');          
        oHidden2 = document.getElementById('hdnOrganizationID');         
    }

    function OnTreeClick(evt)
    {           
        var src = window.event != window.undefined ? window.event.srcElement : evt.target;               
        var nodeClick = src.tagName.toLowerCase() == "a";        
        var imgClick = src.tagName.toLowerCase() == "img";   
        var strReplace ="";
                 
        if(imgClick)        
         strParent = src.alt.match("Base") || src.alt.match("Discretionary");                                   
        var question = '<%= intQuestion %>';
        var orgid = null;
         if(nodeClick)
        {  
           var nodeText = src.innerText;           
           var nodeValue = GetNodeValue(src);      
           alert("Text: "+nodeText + "," + "Value: " + nodeValue);      
          
        }
        var nodeClick = src.tagName.toLowerCase() == "input"; 
        if(nodeClick && src.checked == true) 
        {   oHidden1.value +=strParent + question + src.nextSibling.title +"&";
            orgid = src.nextSibling.title;
        }
        else if(nodeClick && src.checked == false) 
         {
            strReplace = oHidden1.value.match(strParent + question + src.nextSibling.title);
            alert(strReplace);
            oHidden1.value = oHidden1.replace(/strReplace.value/,'');
         }
     
        alert(oHidden1.value);
      //  return false; //uncomment this if you do not want postback on node click
    }
    function GetNodeValue(node)
    {
    
        var nodeValue = "";
        var nodePath = node.href.substring(node.href.indexOf(",")+2,node.href.length-2);
        var nodeValues = nodePath.split("\\");
        if(nodeValues.length > 1)
        nodeValue = nodeValues[nodeValues.length - 1];
        else
        nodeValue = nodeValues[0].substr(1);

        return nodeValue;
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
		            <td><b>Order</b></td>
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
            <table width="100%" height="100%" border="0" cellSpacing="2" cellPadding="2" bgcolor="#c8cfdd">
	            <tr>	                	
	                <td valign="top" class="default">
                        <asp:TreeView ID="oTreeview" runat="server" ShowCheckBoxes="Leaf" ShowLines="true" NodeIndent="35" >
                             <NodeStyle CssClass="default" />
                        </asp:TreeView>
                    </td>               
	            </tr>
	            <tr height="1">
	                <td><hr size="1" noshade /></td>
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
</form>
</body>
</html>

