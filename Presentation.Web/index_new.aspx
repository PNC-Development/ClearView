<%@ Page Language="C#" MasterPageFile="~/clearview_new.Master" AutoEventWireup="true" CodeBehind="index_new.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.index_new" Title="Untitled Page" %>
<%@ Register Src="~/controls/sys/TopNavMenu.ascx" TagPrefix="clearview" TagName="NavBar" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" Runat="Server">
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
        <td><asp:PlaceHolder ID="PHDown" runat="server" /></td>
    </tr>
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
<script type="text/javascript">EnablePostBack('<%=strPage%>','<%=Request.Path%>');</script>
</asp:Content>