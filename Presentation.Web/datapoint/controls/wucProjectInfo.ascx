<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucProjectInfo.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.wucProjectInfo" %>
<table id="tblHeader" width="95%" cellpadding="4" cellspacing="2" border="0">
    <tr>
        <td class="header" width="100%" valign="bottom">
            Project Details</td>
    </tr>
</table>
<asp:Panel ID="pnlProjectInfo" runat="server" Visible="false">
 <table id="tblProjectDetails" width="95%" cellpadding="4" cellspacing="2" border="1px">
     <tr>
         <td align="left" valign="top" width="30%">
             Project #</td>
         <td align="left" valign="top" width="70%">
             <asp:Label ID="lblProjectNumber" runat="server" CssClass="default" />
         </td>
     </tr>
     <tr>
         <td align="left" valign="top" width="30%">
             Project Name</td>
         <td align="left" valign="top" width="70%">
             <asp:Label ID="lblProjectName" runat="server" CssClass="default" />
         </td>
     </tr>
      <tr>
         <td align="left" valign="top" width="30%">
             Project Manager</td>
         <td align="left" valign="top" width="70%">
             <asp:Label ID="lblProjectManager" runat="server" CssClass="default" />
         </td>
     </tr>
     <tr>
         <td align="left" valign="top" width="30%">
             Project Type</td>
         <td align="left" valign="top" width="70%">
             <asp:Label ID="lblProjectType" runat="server" CssClass="default" />
         </td>
     </tr>
      <tr>
         <td align="left" valign="top" width="30%">
             Project Description</td>
         <td align="left" valign="top" width="70%">
             <asp:Label ID="lblProjectDesc" runat="server" CssClass="default" />
         </td>
     </tr>
       <tr>
         <td align="left" valign="top" width="30%">
             Project Status</td>
         <td align="left" valign="top" width="70%">
             <asp:Label ID="lblProjectStatus" runat="server" CssClass="default" />
         </td>
     </tr>
       <tr>
         <td align="left" valign="top" width="30%">
             Executive Sponsor</td>
         <td align="left" valign="top" width="70%">
             <asp:Label ID="lblExecutiveSponsor" runat="server" CssClass="default" />
         </td>
     </tr>
     <tr>
         <td align="left" valign="top" width="30%">
             Working Sponsor</td>
         <td align="left" valign="top" width="70%">
             <asp:Label ID="lblWorkingSponsor" runat="server" CssClass="default" />
         </td>
     </tr>
     <tr>
         <td align="left" valign="top" width="30%">
             Organization</td>
         <td align="left" valign="top" width="70%">
             <asp:Label ID="lblOrganization" runat="server" CssClass="default" />
         </td>
     </tr>
     <tr>
         <td align="left" valign="top" width="30%">
             Segment</td>
         <td align="left" valign="top" width="70%">
             <asp:Label ID="lblSegment" runat="server" CssClass="default" />
         </td>
     </tr>
      <tr>
         <td align="left" valign="top" width="30%">
             Project Initiated On</td>
         <td align="left" valign="top" width="70%">
             <asp:Label ID="lblProjectInitiatedOn" runat="server" CssClass="default" />
         </td>
     </tr>
 </table>
 
 </asp:Panel>
 <table id="tblNoProjectInfo" width="95%" cellpadding="4" cellspacing="2" border="0">
    <tr>
        <td colspan="5">
            <asp:Label ID="lblNoProjectInfo" runat="server" CssClass="default" Visible="false"
                Text="<img src='/images/spacer.gif' border='0' height='1' width='25' /><img src='/images/alert.gif' border='0' align='absmiddle'> No project information found." /></td>
    </tr>
</table>