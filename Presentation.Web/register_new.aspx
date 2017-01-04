<%@ Page Language="C#" MasterPageFile="~/clearview_new.Master" AutoEventWireup="true" CodeBehind="register_new.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.register_new" Title="Untitled Page" %>
<%@ Register Src="~/controls/sys/RegistrationForm.ascx" TagPrefix="clearview" TagName="Registration" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" Runat="Server">
<div id="thinOrangeBar"></div>
<div id="breadcrumbBar">
    <div class="leftColumn">
        <asp:PlaceHolder ID="PH1" runat="server" />
    </div>
    <div class="rightColumn">
    </div>
</div>
<table class="fullWidthAndHeight">
    <tr> 
        <td class="center">
            <table id="controlsContainer">
                <tr> 
                    <td>
                        <clearview:Registration ID="cvRegistration" runat="server" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
</asp:Content>