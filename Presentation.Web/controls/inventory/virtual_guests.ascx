<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="virtual_guests.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.virtual_guests" %>



<script type="text/javascript">
   function CheckSelectedGuests(strAlert) 
   {
        var treeview = document.getElementById("<%=tvVirtualHostnGuests.ClientID%>");
        var inputElements = treeview.getElementsByTagName("input");
        var nodeselected=false;   
        for(i=0;i<inputElements.length;i++)
        {
            if(inputElements[i].type == "checkbox" && inputElements[i].checked)
            {   //alert('node selected');
                nodeselected =true;
            }
        }
        if (nodeselected == false)
        {
            alert('Please select at least one guest');
            return false;
        }
        else
        {
            //Guest are selected - get the user confirmation.
            if (strAlert != "")
                strAlert = strAlert + "\n\n";
            strAlert += "Are you sure you want to continue?"
            if (confirm(strAlert) == false)
                return false;
        }
        return true;
    }
    
    function expandcollapseTreeNodes(treeViewId)
    {
        var obtnExpandCollapseTree = document.getElementById('<%=btnExpandCollapseTree.ClientID %>');
        if (obtnExpandCollapseTree.value == "Expand")
        {
            expandAll(treeViewId);
            obtnExpandCollapseTree.value="Collapse";
           
         }
        else
          {  
            collapseAll(treeViewId);
            obtnExpandCollapseTree.value="Expand";
          }
         return false;
    }
    
    function expandAll(treeViewId)
    {
         var treeView = document.getElementById(treeViewId);
         var treeLinks = treeView.getElementsByTagName("a");
         var j = true;
         for(i=0;i<treeLinks.length;i++)
         {
              if(treeLinks[i].firstChild.tagName == "IMG")
              {
                var node = treeLinks[i];
                var level = parseInt(treeLinks[i].id.substr(treeLinks[i].id.length - 1),10);
                var childContainer = document.getElementById(treeLinks[i].id + "Nodes");
                
               if(j)
                {
                    if(childContainer.style.display == "none")
                    TreeView_ToggleNode(eval(treeViewId +"_Data"),level,node,'r',childContainer);
                    j = false;
                }
                else
                {
                    if(childContainer.style.display == "none")
                    TreeView_ToggleNode(eval(treeViewId +"_Data"),level,node,'l',childContainer);
                }
              }
          }
   }              
   
   function collapseAll(treeViewId)
    {
         var treeView = document.getElementById(treeViewId);
         var treeLinks = treeView.getElementsByTagName("a");
         var j = true;
         for(i=0;i<treeLinks.length;i++)
         {
              if(treeLinks[i].firstChild.tagName == "IMG")
              {
                var node = treeLinks[i];
                var level = parseInt(treeLinks[i].id.substr(treeLinks[i].id.length - 1),10);
                var childContainer = document.getElementById(treeLinks[i].id + "Nodes");
                
               if(j)
                {
                    if(childContainer.style.display == "block")
                    TreeView_ToggleNode(eval(treeViewId +"_Data"),level,node,'r',childContainer);
                    j = false;
                }
                else
                {
                    if(childContainer.style.display == "block")
                    TreeView_ToggleNode(eval(treeViewId +"_Data"),level,node,'l',childContainer);
                }
              }
          }
   }     


</script>



<table width="100%" cellpadding="4" cellspacing="0" border="0">
    <tr>
        <td colspan="2" class="header">Virtual Guests</td>
    </tr>
        <tr>
            <td colspan="2" class="header">
                <asp:Button ID="btnExpandCollapseTree" runat="server" CssClass="default" Width="100" Text="Expand"  />
            </td>
        </tr>
    <tr>
         <td >
          <asp:TreeView ID="tvVirtualHostnGuests" runat="server" CssClass="default" ShowLines="true" NodeIndent="30" ShowCheckBoxes="Leaf" NodeWrap="True" EnableViewState="False">
             
          </asp:TreeView>
          </td>
    </tr>
    <tr>
        <td colspan="3"><hr size="1" noshade /></td>
    </tr>
    </table> 
    <table width="100%" cellpadding="4" cellspacing="0" border="0">
    <tr> 
          <td  align="right"  style="width: 30%; white-space: nowrap;">
            <asp:Label ID="lblMoveGuest" runat="server" CssClass="default">Move Guest:</asp:Label>
          </td>
          <td align="left"><asp:DropDownList ID="ddlHost" runat="server" CssClass="default"></asp:DropDownList>
          <asp:Button ID="btnMoveGuest" runat="server" CssClass="default" Width="100" Text="Go" OnClick="btnMoveGuest_Click"  />
         
          </td>
    </tr>
    <tr>
          <td  align="right"  style="width: 30%; white-space: nowrap;">
            <asp:Label ID="lblDecommissionGuests" runat="server" CssClass="default">Decommission Guest(s):</asp:Label>
          </td>
          <td align="left">
            <asp:Button ID="btnDecommissionGuests" runat="server" CssClass="default" Width="100" Text="Go" OnClick="btnDecommissionGuests_Click"  />
          </td>
    </tr>
    <tr>
          <td  align="right"  style="width: 30%; white-space: nowrap;">
            <asp:Label ID="lblDeleteWOdecommissionGuest" runat="server" CssClass="default">Delete without Decommission:</asp:Label>
          </td>
          <td align="left">
            <asp:Button ID="btnDeleteWOdecommissionGuest" runat="server" CssClass="default" Width="100" Text="Go" OnClick="btnDeleteWOdecommissionGuest_Click" />
          </td>
    </tr>
    <%--<tr>
        <td></td>
        <td colspan="2"><asp:Button ID="btnSave" runat="server" CssClass="default" Width="100" Text="Save" OnClick="btnSave_Click" /></td>
    </tr>--%>
    
</table>