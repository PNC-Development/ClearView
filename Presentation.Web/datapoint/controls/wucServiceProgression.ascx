<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucServiceProgression.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.wucServiceProgression" %>

 <asp:Panel ID="pnlServiceProgression" runat="server" Visible="true">
     <table id="tblHeader" width="100%" cellpadding="4" cellspacing="2" border="0">
         <tr>
             <td class="header" width="100%" valign="bottom">
                 Service Progression</td>
         </tr>
     </table>
    <table width="100%" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
        <tr bgcolor="#EEEEEE">
            <td><b><u>Request #:</u></b></td>
            <td><b><u>Service Name:</u></b></td>
            <td><b><u>Requested By:</u></b></td>
            <td><b><u>Assigned By:</u></b></td>
            <td><b><u>Assigned To:</u></b></td>
            <td><b><u>Submitted:</u></b></td>
            <td><b><u>Last Updated:</u></b></td>
            <td><b><u>Progress:</u></b></td>
            <td><b><u>Status:</u></b></td>
        </tr>
        <asp:repeater ID="rptServices" runat="server">
            <ItemTemplate>
                <tr>
                    <asp:Label ID="lblAutomate" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "automate") %>' />
                    <asp:Label ID="lblRRID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "ResourceRequestID") %>' />
                    <asp:Label ID="lblOnDemand" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "OnDemand") %>' />
                    <asp:Label ID="lblServiceID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "serviceid") %>' />
                    <asp:Label ID="lblRequestID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "requestid") %>' />
                    <td valign="top" nowrap><asp:Label ID="lblRequest" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "ReqServiceNumber") %>' ToolTip='<%# DataBinder.Eval(Container.DataItem, "RequestNumber") %>' /></td>
                    <td valign="top"><asp:Label ID="lblService" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "ServiceName") %>' ToolTip='<%# DataBinder.Eval(Container.DataItem, "ServiceId") %>' /></td>
                    <td valign="top"><asp:Label ID="lblRequestedBy" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "RequestorName").ToString() == "" ? "" : DataBinder.Eval(Container.DataItem, "RequestorName") + " (" +  DataBinder.Eval(Container.DataItem, "RequestorXID") + ")" %>' ToolTip='<%# DataBinder.Eval(Container.DataItem, "userid") %>' /></td>
                    <td valign="top"><asp:Label ID="lblAssignedBy" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "UserAssignedByName").ToString() == "" ? "" : DataBinder.Eval(Container.DataItem, "UserAssignedByName") + " (" +  DataBinder.Eval(Container.DataItem, "UserAssignedByXID") + ")" %>' ToolTip='<%# DataBinder.Eval(Container.DataItem, "AssignedBy") %>' /></td>
                    <td valign="top"><asp:Label ID="lblAssignedTo" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "UserAssignedToName").ToString() == "" ? "" : DataBinder.Eval(Container.DataItem, "UserAssignedToName") + " (" +  DataBinder.Eval(Container.DataItem, "UserAssignedToXID") + ")" %>' ToolTip='<%# DataBinder.Eval(Container.DataItem, "AssignedTo") %>' /></td>
                    <td valign="top"><%# DataBinder.Eval(Container.DataItem, "RequestSubmitted").ToString() == "" ? "" : DateTime.Parse(DataBinder.Eval(Container.DataItem, "RequestSubmitted").ToString()).ToShortDateString() %></td>
                    <td valign="top"><%# DataBinder.Eval(Container.DataItem, "RequestModified").ToString() == "" ? "" : DateTime.Parse(DataBinder.Eval(Container.DataItem, "RequestModified").ToString()).ToShortDateString() %></td>
                    <td valign="top" width="125"><asp:Label ID="lblProgress" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "ServiceName") %>' /></td>
                    <td valign="top"><%# DataBinder.Eval(Container.DataItem, "ServiceStatusName") %></td>
                </tr>
            </ItemTemplate>
            <AlternatingItemTemplate>
                <tr bgcolor="F6F6F6">
                    <asp:Label ID="lblAutomate" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "automate") %>' />
                    <asp:Label ID="lblRRID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "ResourceRequestID") %>' />
                    <asp:Label ID="lblOnDemand" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "OnDemand") %>' />
                    <asp:Label ID="lblServiceID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "serviceid") %>' />
                    <asp:Label ID="lblRequestID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "requestid") %>' />
                    <td valign="top" nowrap><asp:Label ID="lblRequest" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "ReqServiceNumber") %>' ToolTip='<%# DataBinder.Eval(Container.DataItem, "RequestNumber") %>' /></td>
                    <td valign="top"><asp:Label ID="lblService" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "ServiceName") %>' ToolTip='<%# DataBinder.Eval(Container.DataItem, "ServiceId") %>' /></td>
                    <td valign="top"><asp:Label ID="lblRequestedBy" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "RequestorName").ToString() == "" ? "" : DataBinder.Eval(Container.DataItem, "RequestorName") + " (" +  DataBinder.Eval(Container.DataItem, "RequestorXID") + ")" %>' ToolTip='<%# DataBinder.Eval(Container.DataItem, "userid") %>' /></td>
                    <td valign="top"><asp:Label ID="lblAssignedBy" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "UserAssignedByName").ToString() == "" ? "" : DataBinder.Eval(Container.DataItem, "UserAssignedByName") + " (" +  DataBinder.Eval(Container.DataItem, "UserAssignedByXID") + ")" %>' ToolTip='<%# DataBinder.Eval(Container.DataItem, "AssignedBy") %>' /></td>
                    <td valign="top"><asp:Label ID="lblAssignedTo" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "UserAssignedToName").ToString() == "" ? "" : DataBinder.Eval(Container.DataItem, "UserAssignedToName") + " (" +  DataBinder.Eval(Container.DataItem, "UserAssignedToXID") + ")" %>' ToolTip='<%# DataBinder.Eval(Container.DataItem, "AssignedTo") %>' /></td>
                    <td valign="top"><%# DataBinder.Eval(Container.DataItem, "RequestSubmitted").ToString() == "" ? "" : DateTime.Parse(DataBinder.Eval(Container.DataItem, "RequestSubmitted").ToString()).ToShortDateString() %></td>
                    <td valign="top"><%# DataBinder.Eval(Container.DataItem, "RequestModified").ToString() == "" ? "" : DateTime.Parse(DataBinder.Eval(Container.DataItem, "RequestModified").ToString()).ToShortDateString() %></td>
                    <td valign="top" width="125"><asp:Label ID="lblProgress" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "ServiceName") %>' /></td>
                    <td valign="top"><%# DataBinder.Eval(Container.DataItem, "ServiceStatusName") %></td>
                </tr>
            </AlternatingItemTemplate>
        </asp:repeater>
        <tr>
            <td colspan="10">
                <asp:Label ID="lblServices" runat="server" CssClass="default" Visible="false" Text="<img src='/images/spacer.gif' border='0' height='1' width='25' /><img src='/images/alert.gif' border='0' align='absmiddle'> There are no services related to this project" />
            </td>
        </tr>
    </table>
 </asp:Panel>
 
<asp:HiddenField ID="hdnOrder" Value="0" runat="server" />
<asp:HiddenField ID="hdnOrderBy" value ="" runat="server" />
<asp:HiddenField ID="hdnPageNo" Value="1" runat="server" />
<asp:HiddenField ID="hdnRecsPerPage" Value="0" runat="server" />