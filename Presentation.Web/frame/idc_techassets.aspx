<%@ Page Language="C#" MasterPageFile="~/frame.Master" AutoEventWireup="true" CodeBehind="idc_techassets.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.idc_techassets" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" runat="server">
<script type="text/javascript">
 function redirect(strMsg)
 {
    alert(strMsg);
    var strLocation="";    
    if(window.top.location.href.match("&div=I") == null)
    {  
       strLocation = window.top.location.href+"&div=I";  
       strLocation = strLocation.replace(/&save=true/,"");
       window.top.navigate(strLocation);              
    }    
    else 
            
      window.top.navigate(window.top.location);
    
 }
</script>
    <table width="100%" cellpadding="2" cellspacing="2" border="0">                            
        <tr>
            <td colspan="2" class="header" align="center">IDC Asset Types</td> 
        </tr>
        <tr>
            <td class="bold" nowrap>Asset Type: <font class="required">*</font></td>
            <td width="100%"><asp:DropDownList ID="drpAssetType" runat="server" CssClass="default"></asp:DropDownList></td>
        </tr>  
        <tr>
            <td class="bold" nowrap>Sale Status: <font class="required">*</font></td>
            <td width="100%">
              <asp:DropDownList ID="drpSaleStatus" runat="server" CssClass="default">
                <asp:ListItem>-- SELECT --</asp:ListItem> 
                <asp:ListItem>Sold</asp:ListItem>
                <asp:ListItem>Pending</asp:ListItem>
                <asp:ListItem>Lost Sale</asp:ListItem>                                                                      
              </asp:DropDownList> 
            </td>    
        </tr>
        <tr>
            <td class="bold" nowrap>Last Modified:</td> 
            <td width="100%"><asp:Label ID="lblModified" runat="server" CssClass="default"></asp:Label></td>
        </tr>
        <tr>
            <td class="bold" nowrap>Last Updated:</td>
            <td width="100%"><asp:Label ID="lblUpdated" runat="server" CssClass="default"></asp:Label></td> 
        </tr> 
        <tr>   
            <td></td>                             
            <td align="left"><asp:Button ID="btnAdd" runat="server" CssClass="default" Width="100" Text="Add Asset" OnCommand="AddAsset"/>
             <asp:Button ID="btnUpdate" runat="server" CssClass="default" Width="100" Text="Update Asset" OnCommand="UpdateAsset" Visible="false"/>
            </td>                               
        </tr>
    </table>
<asp:Label ID="lblType" runat="server" Visible="false" />
<asp:Label ID="lblProject" runat="server" Visible="false" />
<asp:HiddenField ID ="hdnAsset_Typeid" runat="server" Visible="false" />
</asp:Content>
