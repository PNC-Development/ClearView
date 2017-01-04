<%@ Page Language="C#" MasterPageFile="~/frame.Master" AutoEventWireup="true" CodeBehind="prioritization_question.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.prioritization_question" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" Runat="Server"> 
<script type="text/javascript">
 function chkHidden(chk,oHidden)
 {   
   var chk = document.getElementById(chk);
   var oHidden = document.getElementById(oHidden);  
   oHidden.value = chk.checked;
   return true;
 }
</script>
<table width="100%" cellpadding="3" cellspacing="0" border="0" align="center">
        <tr>     
            <td class="default">Question:<font class="required">&nbsp;*</font></td>
            <td><asp:TextBox ID="txtQuestion" CssClass="default" runat="server" Width="250" Rows="5" TextMode="MultiLine" /></td>            
         
        </tr>
        <tr>
            <td class="default">Class:</td>
            <td><asp:DropDownList ID="drpClass" runat="server" Width="250" CssClass="default" /> </td>
        </tr>
        <tr>
            <td class="default">Required:</td>
            <td><asp:CheckBox ID="chkRequired" runat="server" Checked="true" /></td>
        </tr>
        <tr> 
            <td class="default">Enabled:</td>
            <td><asp:CheckBox ID="chkEnabled" runat="server" Checked="true"/></td>
        </tr>
        <tr>
            <td height="5" colspan="2"><img src="images/spacer.gif" width="1" height="5" /></td>
        </tr>   
        <tr>
            <td>&nbsp;</td>
            <td>
                <asp:Button ID="btnUpdate" CssClass="default" runat="server" Text="Update" Width="75" OnClick="btnUpdate_Click" />
                <asp:Button ID="btnDelete" CssClass="default" runat="server" Text="Delete" Width="75" OnClick="btnDelete_Click" Visible="false" />                                  
            </td>
        </tr>
    </table>  
 <asp:HiddenField ID="hdnEnabled" runat="server" />
 <asp:HiddenField ID="hdnRequired" runat="server" /> 
 <asp:HiddenField ID="hdnID" runat="server" />  
</asp:Content>

 
