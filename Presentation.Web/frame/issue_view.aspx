<%@ Page Language="C#" MasterPageFile="~/frame.Master" AutoEventWireup="true" CodeBehind="issue_view.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.issue_view" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" runat="server">
  <table cellpadding="2" cellspacing="2" class="default">
       <tr>
        <td nowrap>Enhancement Title:<font class="required">&nbsp;*</font></td>
        <td width="100%"><asp:TextBox ID="txtTitle" runat="server" CssClass="default" Width="400" MaxLength="100" /></td>
    </tr>
    <tr>
        <td nowrap>Description:</td>
        <td width="100%"><asp:TextBox ID="txtDescription" runat="server" CssClass="default" Width="400" Rows="7" TextMode="MultiLine" /></td>
    </tr>
    <tr>
        <td nowrap>Module:</td>
        <td width="100%"><asp:DropDownList ID="drpModules" runat="server" CssClass="default" Width="250" /></td>
    </tr>
    <tr>
        <td nowrap>Approximate number of users benefitted:</td>
        <td width="100%"><asp:TextBox ID="txtNumUsers" runat="server" CssClass="default" Width="50" MaxLength="100" /> (0 = none)</td>
    </tr>
    <tr>
        <td nowrap>URL:</td>
        <td width="100%"><asp:TextBox ID="txtURL" runat="server" CssClass="default" Width="250" MaxLength="100" /></td>
    </tr>
    <tr>
        <td nowrap valign="top">Mockup Screenshot (If Applicable):</td>
        <td width="100%"><asp:FileUpload ID="fileUpload" runat="server" CssClass="default" Width="250" /></td>
    </tr>             
    <tr>
       <td colspan="2" align="right"><asp:Button ID="btnUpdate" runat="server" CssClass="default" Text="Update" Width="85" OnClick="btnUpdate_Click" /></td>
    </tr>                  
  </table>
    <asp:Label ID="lblPath" runat="server" Visible="false"></asp:Label>
</asp:Content>
