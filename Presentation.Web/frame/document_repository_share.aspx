<%@ Page Language="C#" MasterPageFile="~/frame.Master" AutoEventWireup="true" CodeBehind="document_repository_share.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.document_repository_share" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" Runat="Server">
<script type="text/javascript">
var ddlSecurity=null;
var ddlApps = null;
var txtUser = null; 
var divApp = null;
var divUser = null;
var oHidden = null;

function ValidateShare()
{ 
    var validate = true;
    var type = '<%= lblType.Text.Replace(":","") %>'
    count = <%= intFolderCount %>
    if(type == "Folder" && count > 0) {
        validate= confirm('This action will be set to all subfolders and files under this folder!! Are you sure ?');
    }
    return validate;    
}

function ValidateAdd() {
    var validate = true;
    if(oHidden.value == "User") 
        validate = ValidateText(txtUser.id,'Please enter a LAN ID or user name');
    else if(oHidden.value == "Application")
        validate = ValidateRadioList(ddlApps.id,'Please select an application');           
    return validate;    
}

function DynamicShareType(rblShareType)
{
    var radioButtons = document.getElementById(rblShareType).getElementsByTagName("input");     
       for (var x = 0; x < radioButtons.length; x++) {
           if (radioButtons[x].checked) {      
                oHidden.value = radioButtons[x].value ;         
                if(radioButtons[x].value == "User") {             
                    divApp.style.display ="none";
                    divUser.style.display ="inline";                                         
                }    
                else  {                                      
                    divApp.style.display ="inline";
                    divUser.style.display ="none";                     
                }    
           }
       }   
}
 
 
 window.onload = function Load() {  
    ddlSecurity =  document.getElementById('<%= ddlSecurity.ClientID %>');
    ddlApps     =  document.getElementById('<%= drpApplications.ClientID %>');
    txtUser     =  document.getElementById('<%= txtUser.ClientID %>'); 
    divApp      =  document.getElementById('<%= divApp.ClientID %>');
    divUser     =  document.getElementById('<%= divUser2.ClientID %>'); 
    oHidden     =  document.getElementById('<%= hdnValue.ClientID %>')   
}
</script>
    <table width="100%" cellpadding="4" cellspacing="2" border="0">
        <tr height="1">
            <td nowrap><b><asp:Label ID="lblType" runat="server" CssClass="default" /></b></td>
            <td nowrap><asp:Label ID="lblName" runat="server" CssClass="default" /></td>
            <td width="100%">                                     
                <asp:TextBox ID="txtName" runat="server" CssClass="default" Visible="false" Width="400" MaxLength="100" />
            </td>
        </tr>
        <asp:Panel ID="panEdit" runat="server" Visible="false">                           
        <tr height="1">
            <td nowrap><b>Security:</b></td>
            <td width="100%">
                <asp:DropDownList ID="ddlSecurity" runat="server" CssClass="default" >
                    <asp:ListItem Value="1" Text="Public (No Restrictions)" />
                    <asp:ListItem Value="0" Text="Shared (Restricted to Users / Departments)" />
                    <asp:ListItem Value="-1" Text="Private (Restricted Access)" Selected="true" />
                </asp:DropDownList>
            </td>
        </tr>                          
        <tr height="1">
            <td></td>
            <td><asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" Text="Save" Width="100" CssClass="default" /></td>
        </tr>
        <asp:Panel ID="panShared" runat="server" Visible="false">
        <tr>
            <td colspan="2">&nbsp;</td>
        </tr>
        <tr height="1">
            <td colspan="2">
                <%=strMenuTab1 %>
                <br />
                <div id="divMenu1" class="tabbing">
                    <div style="display:none">
                        <table width="100%" cellpadding="4" cellspacing="0" border="0">
                            <tr>
                                <td nowrap><b>Share Type:</b></td>            
                                <td width="100%">                                    
                                    <asp:RadioButtonList ID="rblShareType" runat="Server" RepeatDirection="Horizontal">
                                      <asp:ListItem Value="User">User</asp:ListItem>
                                      <asp:ListItem Value="Application">Application</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>                                
                            </tr>                              
                            <tr id="divApp" runat="server" style="display:none">                                                                                                                                              
                                <td nowrap width="85"><b>Application: </b></td>
                                <td width="100%"><asp:DropDownList ID="drpApplications" runat="server" CssClass="default" /></td>                                                  
                            </tr>                                                            
                            <tr id="divUser2" runat="server" style="display:none">
                                <td nowrap width="85"><b>User: </b></td>
                                <td width="100%">
                                    <table cellpadding="0" cellspacing="0" border="0">
                                        <tr>
                                            <td><asp:TextBox ID="txtUser" runat="server" Width="300" CssClass="default" /></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div id="divUser" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                                                    <asp:ListBox ID="lstUser" runat="server" CssClass="default" />
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td nowrap><b>Security:</b></td>
                                <td width="100%">
                                    <asp:RadioButtonList ID="radSecurity" runat="server" CssClass="default" RepeatDirection="Horizontal">
                                        <asp:ListItem Value="1" Text="View" Selected="true" />
                                        <asp:ListItem Value="10" Text="Edit" Enabled="false" />
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td><asp:Button ID="btnAdd" runat="server" OnClick="btnAdd_Click" Text="Add" Width="100" CssClass="default" /></td>
                            </tr>
                        </table>
                    </div>
                    <div style="display:none">
                        <table width="100%" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                            <tr bgcolor="#EEEEEE">
                                <td nowrap><b>Security</b></td>
                                <td nowrap><b>Application</b></td>
                                <td></td>
                            </tr>
                            <asp:repeater ID="rptPermissionsApplications" runat="server">
                                <ItemTemplate>
                                    <tr class="default">
                                        <td nowrap><%# (DataBinder.Eval(Container.DataItem, "security").ToString() == "1" ? "Viewer" : (DataBinder.Eval(Container.DataItem, "security").ToString() == "10" ? "Editor" : DataBinder.Eval(Container.DataItem, "security").ToString()))%></td>
                                        <td width="100%"><%# DataBinder.Eval(Container.DataItem, "applicationid").ToString() =="0" ?"---":oApplication.Get(Int32.Parse(DataBinder.Eval(Container.DataItem, "applicationid").ToString()),"name") %></td>
                                        <td nowrap align="right"><asp:LinkButton ID="btnDeleteUser" runat="server" Text="Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' OnClick="btnDeleteUser_Click" /></td>                                                </tr>
                                </ItemTemplate>
                            </asp:repeater>
                        <tr>
                            <td colspan="3">
                                <asp:Label ID="lblPermissionsApplications" runat="server" CssClass="default" Visible="false" Text="<img src='/images/alert.gif' border='0' align='absmiddle'> This document/folder is not shared" />
                            </td>
                        </tr>
                        </table>
                    </div>
                    <div style="display:none">
                        <table width="100%" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                            <tr bgcolor="#EEEEEE">
                                <td nowrap><b>Security</b></td>
                                <td nowrap><b>User</b></td>
                                <td></td>
                            </tr>
                            <asp:repeater ID="rptPermissionsUsers" runat="server">
                                <ItemTemplate>
                                    <tr class="default">
                                        <td nowrap><%# (DataBinder.Eval(Container.DataItem, "security").ToString() == "1" ? "Viewer" : (DataBinder.Eval(Container.DataItem, "security").ToString() == "10" ? "Editor" : DataBinder.Eval(Container.DataItem, "security").ToString()))%></td>
                                        <td width="100%"><%# oUser.GetFullName(Int32.Parse(DataBinder.Eval(Container.DataItem, "profileid").ToString())) %></td>
                                        <td nowrap align="right"><asp:LinkButton ID="btnDeleteUser" runat="server" Text="Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' OnClick="btnDeleteUser_Click" /></td>                                                </tr>
                                </ItemTemplate>
                            </asp:repeater>
                        <tr>
                            <td colspan="3">
                                <asp:Label ID="lblPermissionsUsers" runat="server" CssClass="default" Visible="false" Text="<img src='/images/alert.gif' border='0' align='absmiddle'> This document/folder is not shared" />
                            </td>
                        </tr>
                        </table>
                    </div>
                </div>
            </td>
        </tr>
        </asp:Panel>
        </asp:Panel>
        <asp:Panel ID="panDenied" runat="server" Visible="false">
        <tr>
            <td colspan="2">&nbsp;</td>
        </tr>
        <tr>
            <td colspan="2" align="center"><img src="/images/error.gif" border="0" align="absmiddle" /> <b>Access Denied</b></td>
        </tr>
        </asp:Panel>
    </table>
<asp:HiddenField ID="hdnUser" runat="server" />
<asp:HiddenField ID="hdnShare" runat="server" /> 
<asp:HiddenField ID="hdnValue" runat="server" />
</asp:Content>
