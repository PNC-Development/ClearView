<%@ Page Language="C#" MasterPageFile="~/clearview_new.Master" AutoEventWireup="true"
    Codebehind="error_new.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.error_new" Title="Untitled Page" %>
<%@ Register Src="~/controls/sys/SystemError.ascx" TagPrefix="clearview" TagName="Error" %>
<%@ Register Src="~/controls/sys/TopNavMenu.ascx" TagPrefix="clearview" TagName="NavBar" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" runat="Server">
<clearview:NavBar ID="navBar" runat="server" />
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
                        <clearview:Error ID="error" runat="server" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>    
</asp:Content>
