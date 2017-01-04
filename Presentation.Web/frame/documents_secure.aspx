<%@ Page Language="C#" MasterPageFile="~/frame.Master" AutoEventWireup="true" CodeBehind="documents_secure.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.documents_secure" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" Runat="Server">
<table width="100%" height="100%" cellpadding="2" cellspacing="0" border="0">
    <tr height="1">
        <td colspan="2">
            <table width="100%" height="100%" cellpadding="2" cellspacing="0" border="0">
                <tr>
                    <td nowrap>Name:</td>
                    <td width="100%"><asp:TextBox ID="txtName" runat="server" CssClass="default" Width="300" MaxLength="100" /></td>
                </tr>
                <tr>
                    <td nowrap>Path:</td>
                    <td width="100%"><asp:FileUpload runat="server" ID="oFile" Width="500" CssClass="default" /></td>
                </tr>
                <tr>
                    <td nowrap valign="top">Description:</td>
                    <td width="100%"><asp:TextBox ID="txtDescription" runat="server" CssClass="default" Width="400" TextMode="MultiLine" Rows="5" /></td>
                </tr>
                <tr>
                    <td nowrap valign="top"><br />Security:</td>
                    <td width="100%">
                        <asp:RadioButtonList ID="radSecurity" runat="server" CssClass="default" RepeatDirection="vertical">
                            <asp:ListItem Value="1" Text="Public (Available to Everyone)" Selected />
                            <asp:ListItem Value="0" Text="Shared (Available to Selected Resources)" />
                            <asp:ListItem Value="-1" Text="Private (Not Available)" />
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td><asp:Button ID="btnUpload" runat="server" CssClass="default" Text="Upload" Width="75" OnClick="btnUpload_Click" /></td>
                </tr>
            </table>
        </td>
    </tr>
    <tr height="1">
        <td colspan="2">&nbsp;</td>
    </tr>
    <tr>
        <td colspan="2" valign="top">
            <table width="100%" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                <tr bgcolor="#EEEEEE">
	                <td><b>Name</b></td>
	                <td>&nbsp;</td>
	            </tr>
                <asp:Repeater id="rptDocuments" runat="server">
	                <ItemTemplate>
		                <tr>
			                <td width="100%"><asp:HyperLink runat="server" ID="lblName" Target="_blank" NavigateUrl='<%# DataBinder.Eval(Container.DataItem, "path") %>' Text='<%# DataBinder.Eval(Container.DataItem, "name") %>' ToolTip="Click to View this Document" /></td>
                            <td nowrap>[<asp:LinkButton ID="btnDelete" OnClick="btnDeleteLink_Click" runat="server" ToolTip="Delete" ImageAlign="AbsMiddle" Text="Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "documentid") %>' />]</td>
		                </tr>
	                </ItemTemplate>
                </asp:Repeater>
                <tr>
                    <td colspan="4">
                        <asp:Label ID="lblNone" runat="server" Visible="false" Text="<img src='/images/alert.gif' border='0' align='absmiddle'> No documents" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
<asp:Label ID="lblType" runat="server" Visible="false" />
<asp:Label ID="lblId" runat="server" Visible="false" />
<asp:Label ID="lblIid" runat="server" Visible="false" />
</asp:Content>
