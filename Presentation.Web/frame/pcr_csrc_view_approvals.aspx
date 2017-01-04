<%@ Page Language="C#" MasterPageFile="~/frame.Master" AutoEventWireup="true" CodeBehind="pcr_csrc_view_approvals.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.pcr_csrc_view_approvals" Title="Untitled Page" %>
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

 <div id="divRoute" runat="server" style="overflow:auto;">
    <table cellpadding="3" cellspacing="2" border="0" class="default">             
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
        <td><asp:Button ID="btnRoute" CssClass="default" runat="server" Text="Route PCR" Width="100" OnClick="btnRoute_Click" /></td>
     </tr>
  </table>
 </div>           
<asp:HiddenField ID="hdnD" runat="server" /> 
<asp:HiddenField ID="hdnC" runat="server" />
</asp:Content>

 
