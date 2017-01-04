<%@ Page Language="C#" MasterPageFile="~/frame.Master" AutoEventWireup="true" CodeBehind="asset_wm_service_tasks.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.asset_wm_service_tasks" Title="Asset Burn-In and Deployment" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" runat="server">

    <script type="text/javascript">
     
  
    </script>
    
    <asp:Panel ID="pnlAllow" runat="server" Visible="true">
        <table width="100%" cellpadding="0" cellspacing="5" border="0">
            <tr>
                <td rowspan="2"><img src="/images/assets.gif" border="0" align="absmiddle" /></td>
                <td class="header" nowrap valign="bottom"><asp:Label ID="lblHeader" runat="server" CssClass="header" /></td>
                <td width="100%" rowspan="2" align="right">
                    <table cellpadding="1" cellspacing="4" border="0">
                        <asp:Panel ID="panSave" runat="server" Visible="false">
                        <tr>
                            <td colspan="7" class="bigCheck" align="center"><img src="/images/bigCheck.gif" border="0" align="absmiddle" /> Update Successful</td>
                        </tr>
                        </asp:Panel>
                        <asp:Panel ID="panError" runat="server" Visible="false">
                        <tr>
                            <td colspan="7" class="bigError" align="center"><img src="/images/bigError.gif" border="0" align="absmiddle" /> <asp:Label ID="lblError" runat="server" /></td>
                        </tr>
                        </asp:Panel>
                    </table>
                </td>
            </tr>
            <tr>
                <td nowrap valign="top"><asp:Label ID="lblHeaderSub" runat="server" CssClass="default" /></td>
            </tr>
        </table>
        <br />
    
        <table width="100%" cellpadding="5" cellspacing="3" border="0" style="border-collapse:collapse">
            <tr>
                <td class="header" colspan="2">Asset Information&nbsp;&nbsp;<asp:Label ID="lblAssetID" runat="server" CssClass="default" /></td>
            </tr>
                        
            <tr id="trAssetSerial" runat="server"   >
                <td ><asp:Label ID="lblAssetSerial" runat="server" CssClass="default" Text="Serial #:" /></td>
                <td >
                    <asp:TextBox ID="txtAssetSerial" CssClass="lightdefault" runat="server"  Width="400" ReadOnly="true" />
                </td>
            </tr>
            <tr id="trAssetTag" runat="server" >
                <td ><asp:Label ID="lblAssetTag" runat="server" CssClass="default" Text="Asset Tag:" /></td>
                <td >
                    <asp:TextBox ID="txtAssetTag" CssClass="lightdefault" runat="server"  Width="400" ReadOnly="true" />
                </td>
            </tr>
            <tr id="trHeader" runat="server" >
                <td class="header" colspan="2">Workload Manager Tasks&nbsp;&nbsp;</td>
            </tr>
            <tr>
                <td colspan="2"><asp:Label ID="lblNote" runat="server" CssClass="note" Text="Incase you have a dependency on service request to complete a task, you can add such request as associated request." /></td>
            </tr>
            <tr>
                <td colspan="2"><hr /></td>
            </tr>
            <tr>
                <td id ="tdWMServiceTasks" colspan="2" runat="server" width="100%" >
                     <asp:Panel ID="pnlWMServiceTasks" runat="server" Visible="true"   width="100%"  ScrollBars="Auto" >
                        <asp:DataList ID="dlWMServiceTaskList" runat="server" CellPadding="3" CellSpacing="1" Width="100%" OnItemDataBound="dlWMServiceTaskList_ItemDataBound" OnItemCommand="dlWMServiceTaskList_Command">
                            <HeaderTemplate>
                                <tr bgcolor="#EEEEEE">
                                    <td align="left" >
                                        <asp:Label ID="lblWMServiceTaskListHeaderSelect" runat="server" CssClass="default" Text="<b></b>"  />
                                    </td>
                                    <td align="left" width="100%">
                                        <asp:Label ID="lblWMServiceTaskListHeaderTask" runat="server" CssClass="default" Text="<b>Tasks</b>"  />
                                    </td>
                                </tr>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr valign="top" style="border-bottom:1px solid #CECECE">
                                    <td align="left" style="border-bottom:1px solid #CECECE" >
                                         <asp:CheckBox ID="chkSelectTask" runat="server" />
                                         <asp:HiddenField ID="hdnTaskStatusId" runat="server" />
                                          <asp:HiddenField ID="hdnTaskId" runat="server" />
                                    </td>
                                    <td align="left" style="border-bottom:1px solid #CECECE">
                                        <asp:Label ID="lblWMServiceTaskListTask" runat="server" CssClass="default" Width="300" /> &nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Label ID="lblWMServiceTaskListRequestAdd" runat="server" CssClass="default" Text="Associated Request# :" />
                                        <asp:TextBox ID="txtRequestNo" runat="server" CssClass="default" MaxLength="50" Width="75"  ToolTip="Examples: CVT12345" />
                                        <asp:Button ID="btnWMServiceTaskReqAdd" runat="server" CssClass="default" Text="Add"  CommandName="ADDREQUEST" />
                                        <br />
                                            <asp:DataList ID="dlWMServiceTaskReqList" runat="server" CellPadding="3" CellSpacing="1" Width="100%" OnItemDataBound="dlWMServiceTaskReqList_ItemDataBound" OnItemCommand="dlWMServiceTaskReqList_Command" >
                                                <HeaderTemplate>
                                                    <tr bgcolor="#EEEEEE" valign="top" >
                                                        <td align="left" >
                                                            
                                                            <asp:Label ID="lblWMServiceTaskReqListHeaderReq" runat="server" CssClass="smalldefault" Text="<b>Request#</b>"  />
                                                        </td>
                                                        <td align="left" width="30%">
                                                            <asp:Label ID="lblWMServiceTaskReqListHeaderReqName" runat="server" CssClass="smalldefault" Text="<b>Request Name</b>"  />
                                                        </td>
                                                         <td align="left" width="20%">
                                                            <asp:Label ID="lblWMServiceTaskReqListHeaderResource" runat="server" CssClass="smalldefault" Text="<b>Resource</b>"  />
                                                        </td>
                                                        <td align="left" width="20%">
                                                            <asp:Label ID="lblWMServiceTaskReqListHeaderStatus" runat="server" CssClass="smalldefault" Text="<b>Status</b>"  />
                                                        </td>
                                                        <td align="left" width="20%">
                                                            <asp:Label ID="lblWMServiceTaskReqListHeaderLastUpdated" runat="server" CssClass="smalldefault" Text="<b>Last Updated</b>"  />
                                                        </td>
                                                         <td align="left" nowrap>
                                                            
                                                        </td>
                                                    </tr>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <tr bordercolor ="red">
                                                        <td align="left" nowrap bordercolor ="red">
                                                            <asp:HiddenField ID="hdnTaskReqStatusId" runat="server" />
                                                            <asp:HiddenField ID="hdnTaskRequestId" runat="server" />
                                                            <asp:LinkButton ID="lnkbtnAssetBnDTaskReqNo" runat="server"  CssClass="lookup"/>
                                                            
                                                        </td>
                                                        <td align="left" >
                                                            <asp:Label ID="lblWMServiceTaskReqListReqName" runat="server" CssClass="smalldefault" />
                                                        </td>
                                                        <td align="left" >
                                                            <asp:Label ID="lblWMServiceTaskReqListResource" runat="server" CssClass="smalldefault" />
                                                        </td>
                                                        <td align="left" >
                                                            <asp:Label ID="lblWMServiceTaskReqListReqStatus" runat="server" CssClass="smalldefault" />
                                                        </td>
                                                        <td align="left" >
                                                            <asp:Label ID="lblWMServiceTaskReqListLastUpdated" runat="server" CssClass="smalldefault" />
                                                        </td>
                                                        <td align="left" >
                                                            <asp:LinkButton ID="lnkbtnWMServiceTaskReqListRemove" runat="server"  Text="Remove" ToolTip="Remove Request"  CssClass="lookup" CommandName="REMOVEREQUEST"/>
                                                        </td>
                                                        
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:DataList>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:DataList>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td colspan="2"><hr /></td>
            </tr>
                <tr align="left">
                <td colspan="2">
                <asp:Button ID="btnSave" runat="server" CssClass="default" Text="Save" OnClick ="btnSave_Click" /> &nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnSaveAndClose" runat="server" CssClass="default" Text="Save And Close" OnClick ="btnSaveAndClose_Click" /> &nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnClose" runat="server" CssClass="default" Text="Close" OnClick ="btnClose_Click" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="pnlDenied" runat="server" Visible="false">
        <br />
            <table width="100%" cellpadding="0" cellspacing="5" border="0">
                <tr>
                    <td rowspan="2"><img src="/images/ico_error.gif" border="0" align="absmiddle" /></td>
                    <td class="header" width="100%" valign="bottom">Access Denied</td>
                </tr>
                <tr>
                    <td width="100%" valign="top">You do not have sufficient permission to view this page Or error occurred while processing your request.</td>
                </tr>
                <tr>
                    <td colspan="2">&nbsp;</td>
                </tr>
                <tr>
                    <td></td>
                    <td width="100%">If you think you should have rights to view it, please contact your ClearView administrator.</td>
                </tr>
            </table>
        <p>&nbsp;</p>
    </asp:Panel>

<asp:HiddenField ID="hdnRequestId" runat="server" />
<asp:HiddenField ID="hdnServiceId" runat="server" />
<asp:HiddenField ID="hdnItemId" runat="server" />
<asp:HiddenField ID="hdnNumber" runat="server" />
<asp:HiddenField ID="hdnAssetId" runat="server" />


</asp:Content>
