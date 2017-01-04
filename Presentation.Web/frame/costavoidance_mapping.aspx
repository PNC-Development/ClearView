<%@ Page Language="C#" MasterPageFile="~/frame.Master" AutoEventWireup="true" CodeBehind="costavoidance_mapping.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.costavoidance_mapping" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" runat="server">
<script type="text/javascript">
var ddlCategory = null;
var ddlItem = null;
var oAmount = null;
var oHidden1 = null;
var oHidden2 = null;
var oAdd = null;
 function Edit(strId,strCategoryId,strItemId) {   
     oAdd.value = "Update";         
     ddlCategory.selectedIndex = strCategoryId;
     ddlItem.selectedIndex = strItemId;
     oHidden1.value = strId;     
     oHidden2.value = "Update";
     AjaxGetItemAmount(ddlItem,oAmount,ddlItem.selectedIndex);         
 }
 window.onload = function Load() {  
      ddlCategory = document.getElementById('<%= drpCategory.ClientID %>');
      ddlItem = document.getElementById('<%= drpItem.ClientID %>');
      oAmount = document.getElementById('<%= lblAmt.ClientID %>');
      oHidden1 = document.getElementById('<%= hdnId.ClientID %>');
      oHidden2 = document.getElementById('<%= hdnVal.ClientID %>');
      oAdd = document.getElementById('<%= btnAdd.ClientID %>');
 }
 
 
</script>
<table width="100%" cellpadding="3" cellspacing="0" border="0" align="center" class="default">
   <tr height="1">
      <td class="frame" nowrap>&nbsp;Category/Item Mapping</td>
      <td class="frame" align="right"><a href="javascript:void(0);" onclick="parent.HidePanel();"><img src="/images/close.gif" border="0" title="Close"></a></td>
   </tr>
   <tr>
     <td colspan="2">     
      <table width="100%" cellpadding="2" cellspacing="2" border="0">
          <tr>
              <td nowrap>Category:<font class="required">&nbsp;*</font></td>                  
              <td width="100%"><asp:DropDownList ID="drpCategory" runat="server" CssClass="default" Width="100" /></td>                  
          </tr>
          <tr>
              <td nowrap>Item:<font class="required">&nbsp;*</font></td>                  
              <td width="100%"><asp:DropDownList ID="drpItem" runat="server" CssClass="default" Width="100" /> <asp:Label ID="lblAmt" CssClass="default" runat="server" /></td>                           
          </tr>
          <tr>
              <td>&nbsp;</td>                  
              <td><asp:Button ID="btnAdd" runat="server" CssClass="default" Width="75" Text="Add" OnClick="btnAdd_Click" /> </td>                  
          </tr>
          <tr>
              <td colspan="2">
                  <table width="100%" cellpadding="3" cellspacing="0" border="0" align="left" style="border: solid 1px #CCCCCC">
                      <asp:Repeater ID="rptView" runat="server">
                          <HeaderTemplate>
                              <tr bgcolor="#EEEEEE">
                                  <td><b>Category</b></td>
                                  <td><b>Item</b></td>                                      
                                  <td><b>Amount</b></td> 
                                  <td width="1"></td>                                     
                              </tr>
                          </HeaderTemplate>
                          <ItemTemplate>
                              <tr class="default">
                                  <td valign="top" nowrap><%# oCustomized.GetCategory(Int32.Parse(DataBinder.Eval(Container.DataItem, "categoryid").ToString()),"name") %></td>                                
                                  <td valign="top" nowrap><%# oCustomized.GetItem(Int32.Parse(DataBinder.Eval(Container.DataItem, "itemid").ToString()),"name")  %></td>                            
                                  <td valign="top" nowrap><%# String.Format("{0:C}", Double.Parse(oCustomized.GetItem(Int32.Parse(DataBinder.Eval(Container.DataItem, "itemid").ToString()), "amount")))%></td>                           
                                  <td valign="top" align="right" width="1">
                                    <asp:Panel ID="panEdit" runat="server" Visible="false">
                                       [<a href="javascript:void(0);" onclick="Edit('<%# DataBinder.Eval(Container.DataItem, "id") %>','<%# DataBinder.Eval(Container.DataItem, "categoryid") %>','<%# DataBinder.Eval(Container.DataItem, "itemid") %>');">Edit</a>]&nbsp;&nbsp;[<asp:LinkButton ID="btnDelete" runat="server" OnClick="btnDelete_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' Text="Delete" />]
                                    </asp:Panel>                                
                                  </td>
                              </tr>
                          </ItemTemplate>
                      </asp:Repeater>
                  </table>
              </td>
          </tr>
          <tr>
              <td colspan="7">
                  <asp:Label ID="lblNone" runat="server" CssClass="default" Visible="false" Text="<img src='/images/alert.gif' border='0' align='absmiddle'> There are (0) items ..." />
              </td>
          </tr>
      </table>            
     </td>
   </tr>       
</table>
<input type="hidden" id="hdnId" runat="server" /> 
<input type="hidden" id="hdnVal" runat="server" />
</asp:Content>
