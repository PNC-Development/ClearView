<%@ Page Language="C#" MasterPageFile="~/frame.Master" AutoEventWireup="true" CodeBehind="pcr_csrc_approvals.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.pcr_csrc_approvals" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" Runat="Server"> 
<script type="text/javascript">
 function chkHidden(chk,oHidden)
 {   
   var chk = document.getElementById(chk);
   var oHidden = document.getElementById(oHidden);  
   oHidden.value = chk.checked;
   return true;
 }
</script>
 <asp:Panel ID="panRoute" runat="server" Visible="false">
   <!-- <div id="divRoute" runat="server"> -->
    <table cellpadding="3" cellspacing="2" border="0" class="default" width="100%">             
     <tr>
        <td class="greenheader"><b>Working Sponsor:</b></td>
        <td><asp:Label ID="lblW" runat="server" CssClass="default" /></td>
     </tr>
     <tr>
        <td class="greenheader"><b>Executive Sponsor:</b></td>
        <td><asp:Label ID="lblE" runat="server" CssClass="default" /></td>
     </tr>
     <tr>
        <td class="greenheader"><b>Director:</b></td>
        <td>
            <table cellpadding="0" cellspacing="0" border="0">
              <tr>
                 <td><asp:TextBox ID="txtD" runat="server" Width="200" CssClass="default" /></td>
              </tr>
              <tr>
                 <td>
                     <div id="divD" runat="server" style="overflow: hidden; position: absolute; display: none;
                                      background-color: #FFFFFF; border: solid 1px #CCCCCC">
                          <asp:ListBox ID="lstD" runat="server" CssClass="default" />
                     </div>
                 </td>
              </tr>
            </table>
        </td>
     </tr>
     <tr>
        <td class="greenheader"> <b>CC:</b></td>
        <td>                
            <table cellpadding="0" cellspacing="0" border="0">
              <tr>
                 <td><asp:TextBox ID="txtC" runat="server" Width="200" CssClass="default" /></td>
              </tr>
              <tr>
                 <td>
                      <div id="divC" runat="server" style="overflow: hidden; position: absolute; display: none;
                                      background-color: #FFFFFF; border: solid 1px #CCCCCC">
                                      <asp:ListBox ID="lstC" runat="server" CssClass="default" />
                      </div>
                 </td>
              </tr>
            </table>
        </td>
     </tr>
     <%= strAttachement %>
     <tr>
        <td>&nbsp;</td>
        <td colspan="2"><asp:Button ID="btnRoute" CssClass="default" runat="server" Text="Route PCR" Width="100" OnClick="btnRoute_Click" /></td>
     </tr>
  </table>
 <!-- </div>-->
 </asp:Panel> 
 
 <asp:Panel ID="panView" runat="server" Visible="false">
  <!--<div id="divView" runat="server" style="display:none">-->
   <table width="100%" cellpadding="4" cellspacing="0" border="0" class="default">
        <tr bgcolor="#EEEEEE">
           <td><b>Approver</b></td>
           <td><b>Role</b></td>
           <td><b>Status</b></td>
           <td><b>Comments</b></td>
        </tr>    
    <asp:Repeater ID="rptView" runat="server">
      <ItemTemplate>
        <tr>
           <td nowrap valign="top"><%# oUser.GetFullName(Int32.Parse(DataBinder.Eval(Container.DataItem,"userid").ToString())) %></td>
           <td nowrap valign="top"><%# GetRole(Int32.Parse(DataBinder.Eval(Container.DataItem,"step").ToString())) %></td>
           <td nowrap valign="top"><%# GetStatus(Int32.Parse(DataBinder.Eval(Container.DataItem,"status").ToString())) %></td>
           <td nowrap valign="top"><%# DataBinder.Eval(Container.DataItem, "comments") == DBNull.Value ? "---" : DataBinder.Eval(Container.DataItem, "comments") %></td>   
        </tr> 
      </ItemTemplate> 
    </asp:Repeater>                        
   </table>
 <!--</div> -->
 </asp:Panel>             

   <asp:Panel ID="panNoView" runat="server" Visible="false">
   <img src='/images/alert.gif' border='0' align='absmiddle'>  This <%= strView %> has not been routed for approval ...
   </asp:Panel>   
<asp:HiddenField ID="hdnD" runat="server" /> 
<asp:HiddenField ID="hdnC" runat="server" />
</asp:Content>

 
