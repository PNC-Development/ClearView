<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="asset_category_deployment_config.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.asset_category_deployment_config" %>



<script type="text/javascript">
   
</script>
<html>
<head>
<title>ClearView Administration</title>
<link rel="stylesheet" type="text/css" href="/css/default.css" />
<script type="text/javascript" src="/javascript/global.js"></script>
</head>
<body topmargin="0" leftmargin="0" >
<form id="Form1" runat="server">
        <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
        <td height="100%">
        <div style="height:100%; overflow:auto">
            <table width="98%" cellpadding="0" cellspacing="0" border="0" align="center">
		        <tr><td colspan="2">&nbsp;</td></tr>
		        <tr> 
		            <td colspan="2"><b>Asset Category : Deployment Configurations</b></td>
		        </tr>
		        <tr><td colspan="2">&nbsp;</td></tr>
		        <tr>
		            <td colspan="2" align="center">
                    
                        <table width="98%" cellpadding="3" cellspacing="0" border="0" align="center">
                            <tr> 
                                <td class="default" nowrap >Asset Category:</td>
                                <td align="left" width ="100%" >
                                    <asp:dropdownlist ID="ddlAssetCategory" CssClass="default" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlAssetCategory_SelectedIndexChanged"/>
                                 </td>
                            </tr>
                            <tr><td colspan="2">&nbsp;</td></tr>
                            <tr>
                                <td colspan ="2" class="default"><b>Deployment Config:</b></td>
                            </tr>
                             <tr valign="top" align="left">
                                <td class="default" ><b>Add Service:</b>  </td>
                                <td>
                                    <asp:TextBox ID="txtService" CssClass="lightdefault" runat="server"  Width="400" ReadOnly="true" />
                                    <input type="hidden" id="hdnServiceId" runat="server" />
                                    <asp:Button ID="btnService" runat="server" CssClass="default" Width="25" Text="..." />
                                    <asp:Button ID="btnAddService" runat="server" CssClass="default" Width="100" Text="Add Service" OnClick="btnAddService_Click" />
                                </td>
                            </tr>
                            <tr > 
                                <td  colspan ="2" class="default" style="border :1px solid #CECECE">
                                    <asp:Panel ID="pnlDeploymentConfig" runat="server" Visible="true" Width="100%"   ScrollBars="Auto" >
                                         <asp:DataList ID="dlConfigSteps" runat="server" CellSpacing="1" Width="100%" OnItemDataBound="dlConfigSteps_ItemDataBound" OnItemCommand="dlConfigSteps_Command">
                                            <HeaderTemplate>
                                                <tr align="left" valign="top" bgcolor="#CCCCCC">
                                                    <td ><asp:ImageButton ID="imgbtnConfigStepDelete" runat="server" ToolTip="Delete" ImageAlign="AbsMiddle" ImageUrl="/admin/images/delete.gif"  /></td>
                                                    <td >
                                                        <asp:Label ID="lblConfigStepHeaderService" runat="server"  CssClass="bold" Text="Service"  />
                                                    </td>
                                                    <td >
                                                        <asp:Label ID="lblConfigStepHeaderProcure" runat="server"  CssClass="bold" Text="Procure"  />
                                                    </td>
                                                    <td >
                                                        <asp:Label ID="lblConfigStepHeaderReDeploy" runat="server"  CssClass="bold" Text="Re-Deploy"  />
                                                    </td>
                                                    <td >
                                                        <asp:Label ID="lblConfigStepHeaderMovement" runat="server"  CssClass="bold" Text="Movement"  />
                                                    </td>
                                                    <td >
                                                        <asp:Label ID="lblConfigStepHeaderDispose" runat="server"  CssClass="bold" Text="Dispose"  />
                                                    </td>
                                                    <td >
                                                        <asp:Label ID="lblConfigStepHeaderStatusChange" runat="server"  CssClass="bold" Text="Asset Status </br> Change Applicable"  />
                                                    </td>
                                                    <td >
                                                        <asp:Label ID="lblConfigStepHeaderStatusIn" runat="server"  CssClass="bold" Text="Asset Status In"  />
                                                    </td>
                                                    <td >
                                                        <asp:Label ID="lblConfigStepHeaderStatusOut" runat="server"  CssClass="bold" Text="Asset Status Out"  />
                                                    </td>
                                                      <td >
                                                        <asp:Label ID="lblConfigStepHeaderCustomName" runat="server"  CssClass="bold" Text="Custom Name"  />
                                                    </td>
                                                    <td >
                                                        <asp:Label ID="lblConfigStepHeaderEnabled" runat="server"  CssClass="bold" Text="Enabled"  />
                                                    </td>
                                                </tr>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr align="left" valign="top">
                                                    <td ><asp:ImageButton ID="imgbtnConfigStepDelete" runat="server" ToolTip="Delete" ImageAlign="AbsMiddle" ImageUrl="/admin/images/delete.gif"  /></td>
                                                    <td>
                                                    
                                                        <asp:Label ID="lblConfigStepService" runat="server"  CssClass="default" Text="Service"  />
                                                        <asp:HiddenField ID="hdnConfigStepServiceId" runat="server" />
                                                        <asp:HiddenField ID="hdnConfigStepId" runat="server" />
                                                    </td>
                                                    <td><asp:CheckBox ID="chkConfigStepProcure" runat="server" CssClass="default" /></td>
                                                    <td><asp:CheckBox ID="chkConfigStepReDeploy" runat="server" CssClass="default" /></td>
                                                    <td><asp:CheckBox ID="chkConfigStepMovement" runat="server" CssClass="default" /></td>
                                                    <td><asp:CheckBox ID="chkConfigStepDispose" runat="server" CssClass="default" /></td>
                                                    <td><asp:CheckBox ID="chkConfigStepStatusChange" runat="server" CssClass="default" /></td>
                                                    <td><asp:dropdownlist ID="ddlConfigStepAssetStatusIn" runat="server" CssClass="default" /></td>
                                                    <td><asp:dropdownlist ID="ddlConfigStepAssetStatusOut" runat="server" CssClass="default" /></td>
                                                    <td><asp:TextBox ID="txtConfigStepCustomName"  runat="server" CssClass="default" /></td>
                                                    <td><asp:CheckBox ID="chkConfigStepEnabled" runat="server" CssClass="default" /></td>
                                                </tr>
                                            </ItemTemplate>
                                            <AlternatingItemTemplate>
                                                <tr align="left" valign="top" bgcolor="#F6F6F6">
                                                    <td ><asp:ImageButton ID="imgbtnConfigStepDelete" runat="server" ToolTip="Delete" ImageAlign="AbsMiddle" ImageUrl="/admin/images/delete.gif"  /></td>
                                                    <td>
                                                        <asp:Label ID="lblConfigStepService" runat="server"  CssClass="default" Text="Service"  />
                                                        <asp:HiddenField ID="hdnConfigStepServiceId" runat="server" />
                                                        <asp:HiddenField ID="hdnConfigStepId" runat="server" />
                                                    </td>
                                                    <td><asp:CheckBox ID="chkConfigStepProcure" runat="server" CssClass="default" /></td>
                                                    <td><asp:CheckBox ID="chkConfigStepReDeploy" runat="server" CssClass="default" /></td>
                                                    <td><asp:CheckBox ID="chkConfigStepMovement" runat="server" CssClass="default" /></td>
                                                    <td><asp:CheckBox ID="chkConfigStepDispose" runat="server" CssClass="default" /></td>
                                                    <td><asp:CheckBox ID="chkConfigStepStatusChange" runat="server" CssClass="default" /></td>
                                                    <td><asp:dropdownlist ID="ddlConfigStepAssetStatusIn" runat="server" CssClass="default" /></td>
                                                    <td><asp:dropdownlist ID="ddlConfigStepAssetStatusOut" runat="server" CssClass="default" /></td>
                                                    <td><asp:TextBox ID="txtConfigStepCustomName"  runat="server" CssClass="default" /></td>
                                                    <td><asp:CheckBox ID="chkConfigStepEnabled" runat="server" CssClass="default" /></td>
                                                </tr>
                                            </AlternatingItemTemplate>
                                        </asp:DataList>
                                        <br />
                                        <asp:Label ID="lblNoResults" runat="server" CssClass="default" Text="No Results Found..." />
                                    </asp:Panel>  
                                </td>
                            </tr>
                            
                            <tr><td height="5" colspan="2">&nbsp;</td></tr>
                            <tr> 
                                <td colspan ="2">
                                    <asp:button ID="btnUpdate" CssClass="default" runat="server" Text="Save" Width="75" OnClick="btnUpdate_Click" />
                                </td>
                            </tr>
                        </table>
                    
                </td>
            </tr>
            </table>
        </div>
        </td>
        </tr>
        </table>
    <input type="hidden" id="hdnId" runat="server" />
    <input type="hidden" id="hdnOrder" runat="server" />
</form>
</body>
</html>
