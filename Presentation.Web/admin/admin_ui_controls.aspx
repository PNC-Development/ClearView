<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="admin_ui_controls.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.admin_ui_controls" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Web Content Management Administration</title>
    <link rel="stylesheet" type="text/css" href="/css/default.css" />
    <script type="text/javascript" src="/javascript/global.js"></script>
    <script type="text/javascript" src="/javascript/default.js"></script>
    <script type="text/javascript">  
    
    function validateUserInputs() 
    {
        var oddlDataType = document.getElementById('<%=ddlDataType.ClientID %>');
        var otxtValRangeFrom = document.getElementById('<%=txtValRangeFrom.ClientID %>');
        var otxtValRangeTo = document.getElementById('<%=txtValRangeTo.ClientID %>');
        
        
        if (otxtValRangeFrom.value!= ""  || otxtValRangeTo.value!= "")
        {    
            
            if (otxtValRangeFrom.value== ""  || otxtValRangeTo.value== "")
            {
                alert("You have enter only validation-Range From or Range To value, Please enter both the values for validation range");
                return false;
            }
            
            if (oddlDataType.selectedIndex == "0")
            {
                alert("Please select the control data type to add range validation");
                return false;
             }
             else
             { 
                    //Validation for selected data type
                    //1 -String,2 -Integer,3-Double,4-Currency,5-Date     

                if (oddlDataType.options[oddlDataType.selectedIndex].value == "2")
                {
                    var ValidChars = "0123456789"; 
                    var Char;
                    var str;
                    str=otxtValRangeFrom.value;
                   
                    for (ii = 0; ii < str.length; ii++) 
                    {
                        Char = str.charAt(ii);
                        if (ValidChars.indexOf(Char) == -1)
                        {  alert("As you selected integer data type,Please enter valid integer values for validation-Range From and Range To");
                           return false;
                        }
                    }
                    str=otxtValRangeTo.value;
                    
                    for (ii = 0; ii < str.length; ii++) 
                    {
                        Char = str.charAt(ii);
                        if (ValidChars.indexOf(Char) == -1)
                        {   alert("As you selected integer data type,Please enter valid integer values for validation-Range From and Range To");
                           return false;
                         }
                       // otxtValRangeTo.focus()
                    }
                                   
                }
                if (oddlDataType.options[oddlDataType.selectedIndex].value == "3"||oddlDataType.options[oddlDataType.selectedIndex].value == "4")
                {
                    if(isNumber(otxtValRangeFrom.value) == false || isNumber(otxtValRangeTo.value) == false)
                    {
                    alert("As you selected double or currency data type,Please enter valid numeric values for validation-Range From and Range To");
                    return false;
                    }                    
                }
               
                if (oddlDataType.options[oddlDataType.selectedIndex].value == "5")
                {
                    if(isDate(otxtValRangeFrom.value) == false || isDate(otxtValRangeTo.value) == false)
                    {
                    alert("As you selected date data type,Please enter valid date for validation-Range From and Range To");
                    return false;
                    }                    
                }

                
             }
         }
         return true;
        
    }
    </script>
</head>
<body style="margin-top:0; margin-left:0" >
    <form id="frmAdminUIControls" runat="server">
    <div style="height:100%; overflow:auto">
        <table id="tblSelect" width="98%" cellpadding="3" cellspacing="0" border="0"  style="text-align: center" runat="server">
            <tr><td colspan="2">&nbsp;</td></tr>
            <tr>
            <td style="width:20% ; text-align:left"><b>UI Controls</b></td>
            <td></td>
            </tr>
            <tr>
                <td style="width:20% ; text-align:left">
                    <asp:Label ID="lblSourceType" runat="server" CssClass="default" Text="Source Type" /></td>
                <td style="width:80% ; text-align:left">
                    <asp:DropDownList ID="ddlSourceType" runat="server" AutoPostBack="True" CssClass="default"
                        OnSelectedIndexChanged="ddlSourceType_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                
            </tr>
            <tr>
                <td style="width:20% ; text-align:left">
                    <asp:Label ID="lblSourceFile" runat="server" CssClass="default" Text="Source File" /></td>
                <td style="width:80% ; text-align:left">
                    <asp:DropDownList ID="ddlSourceFile" runat="server" AutoPostBack="True" CssClass="default"
                        OnSelectedIndexChanged="ddlSourceFile_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                
            </tr>
        </table>
        <asp:Panel ID="pnlNoResults" runat="server" Visible="false" Width="98%" >
              <table width="100%" cellpadding="3" cellspacing="0" border="0"  style="text-align: left" >
                 <tr><td  class="default"><hr /></td></tr>
             </table>
             <table id="tblNoRecords" width="100%" cellpadding="3" cellspacing="0" border="0"  style="text-align: left" runat="server">
                 <tr><td>  
                    <asp:Label ID="lblNoResults" runat="server" CssClass="default" Text="No Results Found..." />
                 </td></tr>
            </table>
        
         </asp:Panel>
        <asp:Panel ID="pnlResults" runat="server" Visible="false" Width="98%" ScrollBars ="Vertical">
             <table width="100%" cellpadding="3" cellspacing="0" border="0"  style="text-align: left" >
                 <tr><td  class="default"><hr /></td></tr>
             </table>
            <div id="divResults" runat="server" style="width:100%; height:250px"> <%--height:350px"--%>
                     <asp:DataList ID="dlUIControls" runat="server" CellPadding="2" CellSpacing="1"  Width="100%" 
                         OnItemDataBound="dlUIControls_ItemDataBound" OnSelectedIndexChanged="dlUIControls_SelectedIndexChanged" OnItemCommand="dlUIControls_Command">
                        <HeaderTemplate>
                            <tr style="background :#EEEEEE">
                                <td  style="width:10% ;  text-align:left;vertical-align :top" ></td>
                            
                               <td  style="width:20% ;  text-align:left;vertical-align :top" >
                                    <asp:LinkButton ID="lnkbtnLabelName" runat="server" CssClass="bold" Text="Label Name" ToolTip ="Label Name"/>
                                </td>
                                <td style="width:30% ; text-align:left;vertical-align :top">
                                    <asp:LinkButton ID="lnkbtnLabelText" runat="server" CssClass="bold" Text="Label Text" ToolTip ="Label Text"/>
                                </td>
                                <td style="width:20% ; text-align:left;vertical-align :top">
                                    <asp:LinkButton ID="lnkbtnShortName" runat="server" CssClass="bold" Text="Short Name" ToolTip ="Short Name(example: Project Name, Operating System)"/>
                                </td>
                                <td style="width:20% ; text-align:left;vertical-align :top">
                                    <asp:LinkButton ID="lnkbtnControlName" runat="server" CssClass="bold" Text="Control Name" ToolTip ="Control name associated with label" />
                                </td>
                            </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr style="background:none">
                                <td  style="width:10% ;  text-align:left;vertical-align :top" >
                                   <asp:LinkButton ID="lnkDLbtnSelectControl" runat="server" CssClass="bold" Text="Select" />
                               </td>
                               <td style="width:20% ; text-align:left;vertical-align :top">
                                   <asp:Label ID="lblDlLabelName" runat="server" CssClass="default" Text="" />
                                </td>
                                <td style="width:30% ; text-align:left;vertical-align :top">
                                    <asp:Label ID="lblDlLabelText" runat="server" CssClass="default" Text="" />
                                </td>
                                <td style="width:20% ; text-align:left;vertical-align :top">
                                    <asp:Label ID="lblDlShortName" runat="server" CssClass="default" Text="" />
                                </td>
                                <td style="width:20% ; text-align:left;vertical-align :top">
                                    <asp:Label ID="lblDlControlName" runat="server" CssClass="default" Text="" />
                                </td>
                            </tr>
                        </ItemTemplate>
                        <AlternatingItemTemplate >
                            <tr style="background:#F6F6F6">
                               <td  style="width:10% ;  text-align:left;vertical-align :top" >
                                   <asp:LinkButton ID="lnkDLbtnSelectControl" runat="server" CssClass="bold" Text="Select" />
                               </td>
                               <td style="width:20% ; text-align:left;vertical-align :top">
                                   <asp:Label ID="lblDlLabelName" runat="server" CssClass="default" Text="" />
                                </td>
                                <td style="width:30% ; text-align:left;vertical-align :top">
                                    <asp:Label ID="lblDlLabelText" runat="server" CssClass="default" Text="" />
                                </td>
                                <td style="width:20% ; text-align:left;vertical-align :top">
                                    <asp:Label ID="lblDlShortName" runat="server" CssClass="default" Text="" />
                                </td>
                                <td style="width:20% ; text-align:left;vertical-align :top">
                                    <asp:Label ID="lblDlControlName" runat="server" CssClass="default" Text="" />
                                </td>
                             </tr>
                        </AlternatingItemTemplate>
                        <SelectedItemTemplate >
                            <tr style="background:yellow">
                               <td  style="width:10% ;  text-align:left;vertical-align :top" >
                                   <asp:LinkButton ID="lnkDLbtnSelectControl" runat="server" CssClass="bold" Text="Select" />
                               </td>
                               <td style="width:20% ; text-align:left;vertical-align :top">
                                   <asp:Label ID="lblDlLabelName" runat="server" CssClass="default" Text="" />
                                </td>
                                <td style="width:30% ; text-align:left;vertical-align :top">
                                    <asp:Label ID="lblDlLabelText" runat="server" CssClass="default" Text="" />
                                </td>
                                <td style="width:20% ; text-align:left;vertical-align :top">
                                    <asp:Label ID="lblDlShortName" runat="server" CssClass="default" Text="" />
                                </td>
                                <td style="width:20% ; text-align:left;vertical-align :top">
                                    <asp:Label ID="lblDlControlName" runat="server" CssClass="default" Text="" />
                                </td>
                          </tr>
                        </SelectedItemTemplate>
                     </asp:DataList>
             </div>
        </asp:Panel>
        
        <asp:Panel ID="pnlControlDetails" runat="server" Visible="false" ScrollBars ="Auto" Width="98%" Height="50%" >
             <table width="100%" cellpadding="3" cellspacing="0" border="0"  style="text-align: left" >
                 <tr><td  class="default"><hr /></td></tr>
             </table>
            <table id="tblUpdates" width="100%" cellpadding="3" cellspacing="0" border="0"  style="text-align: center" runat="server">
                <tr>
                    <td colspan="4" style="width:100% ; text-align:left;vertical-align :top">
                        <asp:Label ID="lblControlDetails" runat="server" CssClass="bold" Text="Control Details" />
                    </td>
                    <td colspan="4" style="width:100% ; text-align:left;vertical-align :top">
                        &nbsp;
                    </td>
                </tr>
                <tr >
                    <td style="width:20% ; text-align:left;vertical-align :top">
                        <asp:Label ID="lblLabelName" runat="server" CssClass="default" Text="Label Name" />
                    </td>
                    <td style="width:30% ; text-align:left;vertical-align :top">
                        <asp:TextBox ID="txtControlId" runat="server" Width="0"  Visible ="false" CssClass="default" />
                        <asp:TextBox ID="txtLabelName" runat="server" Width="200" MaxLength="500" CssClass="default" ToolTip ="Label Name" />
                    </td>
                   <td style="width:20% ; text-align:left;vertical-align :top">
                        <asp:Label ID="lblLabelText" runat="server" CssClass="default" Text="Label Text" />
                    </td>
                    <td style="width:30% ; text-align:left;vertical-align :top">
                        <asp:TextBox ID="txtLabelText" runat="server" Width="200" MaxLength="500" CssClass="default" ToolTip ="Label Text"/>
                    </td>
                </tr>
                <tr>     
                    <td style="width:20% ; text-align:left;vertical-align :top; height: 24px;">
                        <asp:Label ID="lblShortName" runat="server" CssClass="default" Text="Short Name" />
                    </td>
                    <td style="width:30% ; text-align:left;vertical-align :top; height: 24px;">
                        <asp:TextBox ID="txtShortName" runat="server" Width="200" MaxLength="100" CssClass="default" ToolTip ="Short Name(example: Project Name, Operating System)"/>
                    </td>
                    <td style="width:20% ; text-align:left;vertical-align :top; height: 24px;">
                        <asp:Label ID="lblControlName" runat="server" CssClass="default" Text="Control Name" />
                    </td>
                    <td style="width:30% ; text-align:left;vertical-align :top; height: 24px;">
                        <asp:TextBox ID="txtControlName" runat="server" Width="200" MaxLength="500" CssClass="default" ToolTip ="Control name associated with label" />
                    </td>
                </tr>
                <tr>
                     <td style="width:20% ; text-align:left;vertical-align :top">
                        <asp:Label ID="lblToolTipText" runat="server" CssClass="default" Text="ToolTip Text" />
                    </td>
                     <td colspan="3" style="width:80% ; text-align:left;vertical-align :top">
                        <asp:TextBox ID="txtToolTipText" runat="server" Width="98%" CssClass="default" TextMode="MultiLine" Rows="2" ToolTip ="Tool tip text for control" />
                    </td>
                </tr>
                <tr>
                    <td style="width:20% ; text-align:left;vertical-align :top">
                        <asp:Label ID="lblHTMLHelp" runat="server" CssClass="default" Text="HTML Help" />
                    </td>
                     <td colspan="3" style="width:80% ; text-align:left;vertical-align :top">
                        <asp:TextBox ID="txtHTMLHelp" runat="server" Width="98%" CssClass="default" TextMode="MultiLine" Rows="3" ToolTip ="HTML help text for control"/>
                    </td>
                        
                </tr>
                <tr>
                    <td style="width:20% ; text-align:left;vertical-align :top">
                        <asp:Label ID="lblDataType" runat="server" CssClass="default" Text="Data Type" />
                    </td>
                    <td style="width:30% ; text-align:left;vertical-align :top">
                        <asp:DropDownList ID="ddlDataType" runat="server" AutoPostBack="True" CssClass="default" OnSelectedIndexChanged="ddlDataType_SelectedIndexChanged" ToolTip ="Control value data type(example string, integer,date), this is required if range validation is required">
                        </asp:DropDownList>
                    </td>
                     <td style="width:20% ; text-align:left;vertical-align :top">
                        <asp:Label ID="lblDisplayOrder" runat="server" CssClass="default" Text="Display Order" />
                    </td>
                     <td style="width:30% ; text-align:left;vertical-align :top">
                        <asp:TextBox ID="txtDisplayOrder" runat="server" Width="200" CssClass="default" ToolTip ="Optional : Enter display order of the control"/>
                    </td>
                 </tr>
                 </table>
                 <table id="tblValidation" width="100%" cellpadding="3" cellspacing="0" border="0"  style="text-align: center" runat="server">
                    <tr>
                         <td colspan="4" style="width:100% ; text-align:left;vertical-align :top">
                                <asp:Label ID="lblValidations" runat="server" CssClass="bold" Text="Validations" />
                         </td>
                    </tr> 
                    <tr>
                        <td style="width:20% ; text-align:left;vertical-align :top">
                            <asp:Label ID="lblValRequiredField" runat="server" CssClass="default" Text="Required Field" />
                        </td>
                         <td style="width:30% ; text-align:left;vertical-align :top">
                            <asp:CheckBox ID="chkValRequiredField" runat="server" CssClass="default" ToolTip ="Check if this is a required field"/>
                        </td>

                        <td style="width:20% ; text-align:left;vertical-align :top">
                            <asp:Label ID="lblValRegularExp" runat="server" CssClass="default" Text="Regular Expression" />
                        </td>
                        <td style="width:30% ; text-align:left;vertical-align :top">
                            <asp:DropDownList ID="ddlValRegularExp" runat="server" AutoPostBack="True" CssClass="default" OnSelectedIndexChanged="ddlValRegularExp_SelectedIndexChanged" ToolTip ="Specify, if Format expression for this control is required(example email address, SSN)">
                            </asp:DropDownList>
                        </td>
                    </tr> 
                    <tr>
                        <td style="width:20% ; text-align:left;vertical-align :top">
                        <asp:Label ID="lblValCompareControl" runat="server" CssClass="default" Text="Compare With Control" />
                        </td>
                        <td style="width:30% ; text-align:left;vertical-align :top">
                        <asp:TextBox ID="txtValCompareControl" runat="server" Width="200" MaxLength="500" CssClass="default" ToolTip ="Specify, if you want to compare value of this control with other controls(example for password reset, compare password control and retype password control values)" />
                        </td>
                        <td style="width:20% ; text-align:left;vertical-align :top">
                        </td>
                        <td style="width:30% ; text-align:left;vertical-align :top">
                        </td>
                    </tr>
                    <tr>
                        <td style="width:20% ; text-align:left;vertical-align :top">
                        <asp:Label ID="lblValMinLen" runat="server" CssClass="default" Text="Minimum Length"  />
                        </td>
                        <td style="width:30% ; text-align:left;vertical-align :top">
                        <asp:TextBox ID="txtValMinLen" runat="server" Width="200" MaxLength="3" CssClass="default" ToolTip ="Specify, if validation for minimum length characters for this control is required. By Default 0,means no validation required for minimum length " />
                        </td>
                        <td style="width:20% ; text-align:left;vertical-align :top">
                        <asp:Label ID="lblValMaxLen" runat="server" CssClass="default" Text="Maximum Length" />
                        </td>
                        <td style="width:30% ; text-align:left;vertical-align :top">
                        <asp:TextBox ID="txtValMaxLen" runat="server" Width="200" MaxLength="3" CssClass="default" ToolTip ="Specify, if validation for maximum length characters for this control is required. By Default 0,means no validation required for maximum length" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width:20% ; text-align:left;vertical-align :top">
                            <asp:Label ID="lblValMinValue" runat="server" CssClass="default" Text="Range From" />
                        </td>
                        <td style="width:30% ; text-align:left;vertical-align :top">
                            <asp:TextBox ID="txtValRangeFrom" runat="server" Width="200" MaxLength="500" CssClass="default" ToolTip ="Specify, if validation for minimum value for this control is required(example : Control value range from 10 to 20)"/>
                        </td>
                         <td style="width:20% ; text-align:left;vertical-align :top">
                            <asp:Label ID="lblValMaxValue" runat="server" CssClass="default" Text="Range To" />
                        </td>
                        <td style="width:30% ; text-align:left;vertical-align :top">
                            <asp:TextBox ID="txtValRangeTo" runat="server" Width="200" MaxLength="500" CssClass="default" ToolTip ="Specify, if validation for maximum value for this controls(example : Control value range from 10 to 20)"/>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:20% ; text-align:left;vertical-align :top">
                            <asp:Label ID="lblValClientSideFunName" runat="server" CssClass="default" Text="Client Side Validation Function Name"  />
                        </td>
                         <td style="width:30% ; text-align:left;vertical-align :top"> 
                            <asp:TextBox ID="txtValClientSideFunName" runat="server" Width="200" CssClass="default" ToolTip ="Specify, if client side validation function is required to validate this control(example : Javascript function validateText() )"/>
                        </td>
                        <td style="width:20% ; text-align:left;vertical-align :top">
                            <asp:Label ID="lblValServerSideFunName" runat="server" CssClass="default" Text="Server Side Validation Function Name" />
                        </td>
                         <td style="width:30% ; text-align:left;vertical-align :top">
                            <asp:TextBox ID="txtValServerSideFunName" runat="server" Width="200" CssClass="default" ToolTip ="Specify, if server side validation function is required to validate this control(example : function validateUser() )"/>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:20% ; text-align:left;vertical-align :top">
                        <asp:Label ID="lblValMsg" runat="server" CssClass="default" Text="Validation Message" />
                        </td>
                        <td colspan="3" style="width:80% ; text-align:left;vertical-align :top">
                        <asp:TextBox ID="txtValMsg" runat="server" Width="98%" CssClass="default" TextMode="MultiLine" Rows="2" ToolTip ="Specify, if validation is added to controls.(example : Please enter valid user id )" />
                        </td>
                    </tr>
                </table>
            <table id="tblAddUpdates" width="98%" cellpadding="3" cellspacing="0" border="0"  style="text-align: center" runat="server">
                <tr>
                    <td  style="width:100% ; text-align:right;vertical-align :top">
                            <asp:button ID="btnAdd" CssClass="default" runat="server" Text="Add" Width="75" Visible="false" OnClick="btnAdd_Click" />
                            <asp:button ID="btnUpdate" CssClass="default" runat="server" Text="Update" Width="75" Visible="true" OnClick="btnUpdate_Click" ToolTip ="Update"/>
                            <asp:button ID="btnCancel" CssClass="default" runat="server" Text="Cancel" Width="75" Visible="true" OnClick="btnCancel_Click" ToolTip ="Cancel"/>
                     </td>
                </tr>
            </table>
        </asp:Panel>
        
    </div>
    </form>
</body>
</html>
