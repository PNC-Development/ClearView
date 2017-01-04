<%@ Page Language="C#" MasterPageFile="~/frame.Master" AutoEventWireup="true" CodeBehind="storage_override_code.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.storage_override_code" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" runat="server">
    <asp:Panel ID="panUnlock" runat="server" Visible="True">
            <table width="100%" cellpadding="0" cellspacing="5" border="0">
            <tr>
                <td align="center">
                    <asp:Panel ID="panStorageOverrideUnlock" runat="server" Visible="True">
                    <table cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
                        
                        <tr>
                            <td>
                                <div id="divShow" style="display:"inline">
                                    <table cellpadding="4" cellspacing="0" border="0">
                                        <tr>
                                            <td nowrap align ="right">Workstation IP:</td>
                                            <td align ="left"><asp:Label ID="lblStorageOverrideWorkStation" runat="server" CssClass="default" /></td>
                                        </tr>
                                        <tr>
                                            <td nowrap align="right">Override Code:</td>
                                            <td align ="left"><asp:TextBox ID="txtStorageOverrideUnlock" runat="server" CssClass="default" Width="400" /></td>
                                        </tr>
                                        <tr>
                                            <td nowrap></td> 
                                            <td><asp:Button ID="btnStorageOverrideUnlock" runat="server" CssClass="default" Width="100" Text="Unlock" OnClick="btnStorageOverrideUnlock_Click" /></td>
                                            
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" class="reddefault"><b>NOTE:</b> Overriding storage will delete the existing configuration.</td>
                        </tr>
                    </table>
                    </asp:Panel>
                </td>
            </tr>
            </table>
    </asp:Panel>
    <asp:HiddenField ID="hdnOverrideConfirmed" runat="server" />
</asp:Content>
