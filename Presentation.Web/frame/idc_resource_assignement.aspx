<%@ Page Language="C#" MasterPageFile="~/frame.Master" AutoEventWireup="true" CodeBehind="idc_resource_assignement.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.idc_resource_assignement" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" runat="server">
<script type="text/javascript">
 function redirect(strMsg)
 {
    alert(strMsg);
    var strLocation="";
    if(window.top.location.href.match("&div=I") == null)
    {  
       strLocation = window.top.location.href+"&div=I";  
       window.top.navigate(strLocation);              
    }    
    else 
      window.top.navigate(window.top.location);
    
 }
</script>
    <table width="100%" cellpadding="2" cellspacing="2" border="0">                            
        <tr>
            <td colspan="2" class="header" align="center">IDC Resource Assignement</td> 
        </tr>
        <tr>
            <td class="bold" nowrap>Resource Required: <font class="required">*</font></td>
            <td width="100%"><asp:DropDownList ID="drpResourceRequired" runat="server" CssClass="default"></asp:DropDownList></td>
        </tr>  
        <tr>
            <td class="bold" nowrap>Requested By: <font class="required">*</font></td>
            <td width="100%">
               <table cellpadding="0" cellspacing="0" border="0">
                 <tr>
                    <td><asp:TextBox ID="txtRequestedBy" runat="server" Width="250" CssClass="default" /></td>
                 </tr>
                  <tr>
                    <td>
                        <div id="divAJAX" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                            <asp:ListBox ID="lstAJAX" runat="server" CssClass="default" />
                        </div>
                    </td>
                </tr>
               </table>
                <input type="hidden" id="hdnAJAXValue" name="hdnAJAXValue" />
            </td>    
        </tr>                           
        <tr>
            <td class="bold" nowrap>Date Requested: <font class="required">*</font></td> 
            <td width="100%"><asp:TextBox ID="txtDateRequested" runat="server" CssClass="default"></asp:TextBox> <asp:ImageButton ID="imgDateRequested" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
        </tr>
        <tr>
            <td class="bold" nowrap>Fulfillment Date: <font class="required">*</font></td> 
            <td width="100%"><asp:TextBox ID="txtFulfillDate" runat="server" CssClass="default"></asp:TextBox> <asp:ImageButton ID="imgFulfillDate" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
        </tr> 
        <tr>
            <td class="bold" nowrap>Resource Assigned: <font class="required">*</font></td> 
            <td width="100%">
              <table cellpadding="0" cellspacing="0" border="0">
                 <tr>
                    <td><asp:TextBox ID="txtResourceAssigned" runat="server" CssClass="default" Width="250"></asp:TextBox></td>
                 </tr>
                 <tr>
                    <td>
                        <div id="divAJAX2" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                            <asp:ListBox ID="lstAJAX2" runat="server" CssClass="default" />
                        </div>
                    </td>
                 </tr>
              </table>
            </td>                               
        </tr> 
          <tr>
            <td class="bold" nowrap>Status: <font class="required">*</font></td> 
            <td width="100%"><asp:TextBox ID="txtStatus" runat="server" CssClass="default" TextMode="MultiLine" Height="60px" Width="50%"></asp:TextBox> </td>
        </tr> 
        <tr>  
        <td></td>                              
            <td align="left"><asp:Button ID="btnAdd" runat="server" CssClass="default" Width="100" Text="Add Resource" OnClick="btnAdd_Click"/>
             <asp:Button ID="btnUpdate" runat="server" CssClass="default" Width="130" Text="Update Resource" OnCommand="btnUpdate_Click" Visible="false"/>
            </td>                               
        </tr>
    </table>
<asp:Label ID="lblType" runat="server" Visible="false" />
<asp:Label ID="lblProject" runat="server" Visible="false" />
<asp:HiddenField ID="hdnManager" runat="server" />
</asp:Content>
