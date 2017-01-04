<%@ Page Language="C#" MasterPageFile="~/frame.Master" AutoEventWireup="true" CodeBehind="production.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.production" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" runat="server">
   <table cellpadding="2" cellspacing="2" class="default" width="450">
      <tr>
         <td nowrap valign="top">When is this server going to production ?<font class="required">&nbsp;*</font></td>
         <td width="100%" nowrap><asp:TextBox ID="txtProdDate" runat="server" CssClass="default" Width="100" /> <asp:ImageButton ID="imgProdDate" runat="server" CssClass="default" ImageUrl="/images/calendar.gif" ImageAlign="AbsMiddle" /></td>
      </tr>                 
      <tr>                
         <td></td>       
         <td width="100%"><asp:Button ID="btnUpdate" runat="server" CssClass="default" Text="Update" Width="85" OnClick="btnUpdate_Click" /></td>
      </tr>                  
   </table>
</asp:Content>
