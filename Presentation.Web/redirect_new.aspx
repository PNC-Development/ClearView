<%@ Page Language="C#" MasterPageFile="~/clearview_new.Master" AutoEventWireup="true" CodeBehind="redirect_new.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.redirect_new" Title="Untitled Page" %>

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
                        <asp:PlaceHolder ID="PH3" runat="server" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
</asp:Content>