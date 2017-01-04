<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucWorkflow.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.wucWorkflow" %>

 <asp:Panel ID="pnlRRinvolvement" runat="server" Visible="true">
     <table id="tblHeader" width="100%" cellpadding="4" cellspacing="2" border="0">
         <tr>
             <td class="header" width="100%" valign="bottom">Workflow / History</td>
         </tr>
     </table>
     <br />
    <table cellpadding="7" cellspacing="0" border="0">
        <asp:Literal ID="litWorkflow" runat="server" />
        <tr>
            <td colspan="10">
                <asp:Label ID="lblWorkflow" runat="server" CssClass="default" Visible="true" Text="<img src='/images/spacer.gif' border='0' height='1' width='25' /><img src='/images/hugeAlert.gif' border='0' align='absmiddle'> There is no information for this request." />
            </td>
        </tr>
    </table>
 </asp:Panel>