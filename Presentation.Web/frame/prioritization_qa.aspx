<%@ Page Language="C#" MasterPageFile="~/frame.Master" AutoEventWireup="true" CodeBehind="prioritization_qa.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.prioritization_qa" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" Runat="Server">
<script type="text/javascript">
  var oHidden1 = null;
  var oHidden2 = null;   
  var strParent = null;
  
  function Edit(strOrganizationId, strQuestionId,strParent) {    
         oHidden1.value = strParent +"&" + strOrganizationId;       
    }   

  function Load()  {   
        oHidden1 = document.getElementById('<%=hdnParent.ClientID %>');          
        oHidden2 = document.getElementById('hdnOrganizationID');         
    }
    
</script>
<table  width="100%" height="100%" border="0" cellSpacing="0" cellPadding="0" align="center">
    <tr>
        <td colspan="2">
            <div style="height:100%; width:100%; overflow:auto;">
            <table width="100%" height="100%" border="0" cellSpacing="2" cellPadding="2" bgcolor="#ffffff">
	            <tr>	                	
	                <td valign="top" class="default">
                        <asp:TreeView ID="oTreeview" runat="server" ShowCheckBoxes="Leaf" ShowLines="true" NodeIndent="35" >
                             <NodeStyle CssClass="default" />
                        </asp:TreeView>
                    </td>               
	            </tr>
            </table>
            </div>
        </td>
    </tr>
    <tr height="1">
        <td colspan="2"><hr size="1" noshade /></td>
    </tr>
    <tr height="1">
        <td align="right" bgcolor="#ffffff" colspan="2">
            <asp:Button ID="btnSave" runat="server" Text="Copy" Width="75" CssClass="default" OnClick="btnSave_Click"/>              
        </td>
    </tr>      
</table>
<asp:Label ID="lblId" runat="server" Visible="false" />
<asp:Label ID="lblType" runat="server" Visible="false" />
<input type="hidden" runat="server" id="hdnId" />
<input id="hdnUpdateOrder" name="hdnUpdateOrder" type="hidden"/>
<input id="hdnParent" name="hdnParent" type="hidden" runat="server" />
<input id="hdnOrganizationID" name="hdnOrganizationID" type="hidden"/>
</asp:Content>
