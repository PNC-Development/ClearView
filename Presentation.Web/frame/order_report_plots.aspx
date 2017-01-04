<%@ Page Language="C#" MasterPageFile="~/frame.Master" AutoEventWireup="true" CodeBehind="order_report_plots.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.order_report_plots" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" runat="server">
<script type="text/javascript">
var oName1 = null;
var oName2 = null;
var oFormula = null;
var oType = null;
var oAdd1 = null;
var oAdd2 = null;
var oHidden1 = null;
var oHidden2 = null;

function EditField(strId,strName,strType){
  oName1.value = strName;
  oAdd1.value = "Update";
  for (var ii=0; ii<oType.length; ii++) {
     if (oType.options[ii].value == strType) {
        oType.selectedIndex = ii;
        break; 
     }
  }
  oHidden1.value = strId;
  oHidden2.value = "Update";
}

function EditCalc(strId,strName,strFormula){
  oFormula.value = strFormula;
  oAdd2.value = "Update";
  for (var ii=0; ii<oName2.length; ii++) {
     if (oName2.options[ii].text == strName) {
        oName2.selectedIndex = ii;
        break; 
     }
  }
  oHidden1.value = strId;
  oHidden2.value = "Update";
}
window.onload = function Load() {
    oName1 = document.getElementById('<%= txtName.ClientID %>');
    oName2 = document.getElementById('<%= drpName.ClientID %>');
    oFormula = document.getElementById('<%= txtFormula.ClientID %>');
    oType = document.getElementById('<%= drpType.ClientID %>');
    oAdd1 =  document.getElementById('<%= btnAdd.ClientID %>');
    oAdd2 =  document.getElementById('<%= btnAddFormula.ClientID %>');
    oHidden1 = document.getElementById('<%= hdnId.ClientID %>');
    oHidden2 = document.getElementById('<%= hdnVal.ClientID %>');
} 
</script> 
<asp:Panel ID="panField" runat="server" Visible="false">
    <table cellpadding="2" cellspacing="2" border="0" width="100%">
        <tr>
            <td align="center" colspan="2"><a href="javascript:void(0);" onclick="parent.HidePanel();"><img src='/images/close-icon.gif' border='0' align='absmiddle' /><img src='/images/spacer.gif' border='0' width='5' height='1' />Close Window</a></td>
        </tr>
        <tr>
            <td class="default" nowrap>Name:</td>
            <td align="left"><asp:TextBox ID="txtName" runat="server" CssClass="default" Width="250" /></td>                
        </tr>     
        <tr>
            <td class="default" nowrap>Type:</td>
            <td align="left">
                <asp:DropDownList ID="drpType" runat="server" CssClass="default" Width="150">
                    <asp:ListItem Text="-- SELECT --" Value="-- SELECT --" Selected="True"></asp:ListItem>                     
                    <asp:ListItem Text="Numeric" Value="Numeric"></asp:ListItem>
                    <asp:ListItem Text="AlphaNumeric" Value="AlphaNumeric"></asp:ListItem>
                    <asp:ListItem Text="Floating Point" Value="Floating Point"></asp:ListItem>
                    <asp:ListItem Text="Currency" Value="Currency"></asp:ListItem>
                    <asp:ListItem Text="Boolean" Value="Boolean"></asp:ListItem>
                    <asp:ListItem Text="Date" Value="Date"></asp:ListItem>              
                </asp:DropDownList>             
            </td>         
        </tr>
        <tr>
            <td></td>
            <td><asp:Button ID="btnAdd" runat="server" CssClass="default" Text="Add" Width="100" OnClick="btnAdd_Click"/></td>
        </tr>
        <tr>
            <td colspan="2">&nbsp;</td>
        </tr>
        <tr>
            <td colspan="2" class="bold">Data Field Mappings</td>
        </tr>
        <tr>            
            <td colspan="2">
                <table id="tblPlot" width="100%" cellpadding="2" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                    <tr bgcolor="#eeeeee">
                        <td nowrap style="text-decoration: underline"><b>Field Name</b></td>
                        <td nowrap style="text-decoration: underline"><b>Field Type</b></td>
                        <td width="1"></td>
                    </tr>                
                    <asp:Repeater ID="rptPlot" runat="server" EnableViewState="true">
                        <ItemTemplate>
                            <tr>
                               <td valign="top" width="50%"><asp:Label ID="lblFieldName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Name") %>'></asp:Label></td>
                               <td valign="top" width="50%"><asp:Label ID="lblFieldType" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Type") %>'></asp:Label></td>
                               <td valign="top" align="right" width="1">
                                   <asp:Panel ID="panEdit" runat="server" Visible="false">
                                       [<a href="javascript:void(0);" onclick="EditField('<%# DataBinder.Eval(Container.DataItem, "id") %>','<%# DataBinder.Eval(Container.DataItem, "Name") %>','<%# DataBinder.Eval(Container.DataItem, "Type") %>');">Edit</a>]&nbsp;&nbsp;[<asp:LinkButton ID="btnDelete" runat="server" OnClick="btnDelete_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' Text="Delete" />]
                                   </asp:Panel>                             
                               </td>
                            </tr>
                        </ItemTemplate>                  
                    </asp:Repeater>            
                    <tr>                
                        <td colspan="7" align="left"><asp:Label ID="lblNoFields" runat="server" CssClass="default" Visible="false" Text="<img src='/images/alert.gif' border='0' align='absmiddle'> There are 0 items..." /></td>
                    </tr>                       
                </table>     
            </td>       
        </tr> 
    </table>
 </asp:Panel>   
<asp:Panel ID="panFormula" runat="server" Visible="false">
    <table cellpadding="2" cellspacing="2" border="0" width="100%">
        <tr>
            <td align="center" colspan="2"><a href="javascript:void(0);" onclick="parent.HidePanel();"><img src='/images/close-icon.gif' border='0' align='absmiddle' /><img src='/images/spacer.gif' border='0' width='5' height='1' />Close Window</a></td></td>
        </tr>
        <tr>
            <td class="default">Name:</td>
            <td align="left"><asp:DropDownList ID="drpName" runat="server" CssClass="default" Width="100"></asp:DropDownList></td>                
        </tr>     
        <tr>
            <td class="default">Formula:</td>
            <td align="left"><asp:TextBox ID="txtFormula" runat="server" CssClass="default" Width="250" TextMode="MultiLine" Rows="5"></asp:TextBox></td>         
        </tr>
        <tr>
            <td></td>
            <td><asp:Button ID="btnAddFormula" runat="server" CssClass="default" Text="Add" Width="100" OnClick="btnAdd_Click"/></td>
        </tr>
        <tr>
            <td colspan="2">&nbsp;</td>
        </tr>
        <tr>
            <td colspan="2" class="bold">Data Field/Formula Mapping</td>
        </tr>
        <tr>            
            <td colspan="2">
                <table width="100%" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                    <tr bgcolor="#eeeeee">
                        <td nowrap style="text-decoration: underline"><b>Field Name</b></td>
                        <td nowrap style="text-decoration: underline"><b>Formula</b></td> 
                        <td width="1">                    
                    </tr>                
                    <asp:Repeater ID="rptFormula" runat="server" EnableViewState="true">
                        <ItemTemplate>
                            <tr>
                                <td valign="top" width="50%"><asp:Label ID="lblFieldName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Name") %>'></asp:Label></td>
                                <td valign="top" width="50%"><asp:Label ID="lblFieldFormula" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Formula") %>'></asp:Label></td>
                                <td valign="top" align="right" width="1">
                                   <asp:Panel ID="panEdit" runat="server" Visible="false">
                                       [<a href="javascript:void(0);" onclick="EditCalc('<%# DataBinder.Eval(Container.DataItem, "id") %>','<%# DataBinder.Eval(Container.DataItem, "name") %>','<%# DataBinder.Eval(Container.DataItem, "formula") %>');">Edit</a>]&nbsp;&nbsp;[<asp:LinkButton ID="btnDelete" runat="server" OnClick="btnDelete_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' Text="Delete" />]
                                   </asp:Panel>                             
                                </td>
                            </tr>
                        </ItemTemplate>                  
                    </asp:Repeater>            
                    <tr>                
                        <td colspan="7"><asp:Label ID="lblNoFormula" runat="server" CssClass="default" Visible="false" Text="<img src='/images/alert.gif' border='0' align='absmiddle'> There are 0 items..." /></td>
                    </tr>                       
                </table>     
            </td>       
        </tr> 
    </table>
</asp:Panel>   
<input type="hidden" id="hdnId" runat="server" />
<input type="hidden" id="hdnVal" runat="server" />
</asp:Content>
