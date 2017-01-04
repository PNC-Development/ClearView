<%@ Page Language="C#" MasterPageFile="~/clearview_new.Master" AutoEventWireup="true" CodeBehind="updateUserProfile_new.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.updateUserProfile_new" Title="Untitled Page" %>
<%@ Register Src="~/controls/sys/TopNavMenu.ascx" TagPrefix="clearview" TagName="NavBar" %>
<%@ Register Src="~/controls/sys/UserProfileUpdate.ascx" TagPrefix="clearview" TagName="UserProfileUpdate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" Runat="Server">
<clearview:NavBar ID="navBar" runat="server" />
<div id="thinOrangeBar"></div>
<div id="breadcrumbBar">
    <div class="rightColumn">
    </div>
</div>
<table class="fullWidthAndHeight">
    <tr> 
        <td class="center">
            <table id="controlsContainer">
                <tr> 
                    <td>
                        <clearview:UserProfileUpdate ID="cvUpdateProfile" runat="server" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>

</asp:Content>