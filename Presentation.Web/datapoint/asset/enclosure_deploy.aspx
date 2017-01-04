<%@ Page Language="C#" MasterPageFile="~/datapoint.Master" AutoEventWireup="true" CodeBehind="enclosure_deploy.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.enclosure_deploy" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" runat="server">

<script type="text/javascript">
        var ddlAssetAttribute = null;
        var txtAssetAttributeComment = null;
        window.onload = function Load() 
        {
             ddlAssetAttribute = document.getElementById('<%=ddlAssetAttribute.ClientID%>');
             txtAssetAttributeComment = document.getElementById('<%=txtAssetAttributeComment.ClientID%>');
             SetControlsForAssetAttributes();
        }
        function SetControlsForAssetAttributes()
        {
         if (ddlAssetAttribute != null) 
        {   
            if (ddlAssetAttribute.options[ddlAssetAttribute.selectedIndex].value != "0" )
                txtAssetAttributeComment.disabled=false;
            else
            {   txtAssetAttributeComment.value="";
                txtAssetAttributeComment.disabled=true;
            }
            
          }
          return false;      
            
        }
</script>

    <asp:Panel ID="panAllow" runat="server" Visible="false">
    <table width="100%" cellpadding="0" cellspacing="5" border="0">
        <tr>
            <td rowspan="2"><img src="/images/assets.gif" border="0" align="absmiddle" /></td>
            <td class="redheader" nowrap valign="bottom"><asp:Label ID="lblHeader" runat="server" CssClass="redheader" /></td>
            <td width="100%" rowspan="2" align="right">
                <table cellpadding="1" cellspacing="4" border="0">
                    <tr>
                        <td nowrap><asp:LinkButton ID="btnNew" runat="server" Text="<img src='/images/new-icon.gif' border='0' align='absmiddle' /><img src='/images/spacer.gif' border='0' width='5' height='1' />New Search" OnClick="btnNew_Click" /></td>
                        <td>|</td>
                        <td nowrap><asp:LinkButton ID="btnDeploy" runat="server" Text="<img src='/images/save-icon.gif' border='0' align='absmiddle' /><img src='/images/spacer.gif' border='0' width='5' height='1' />Deploy" OnClick="btnDeploy_Click" /></td>
                        <td>|</td>
                        <td nowrap><asp:LinkButton ID="btnPrint" runat="server" Text="<img src='/images/print-icon.gif' border='0' align='absmiddle' /><img src='/images/spacer.gif' border='0' width='5' height='1' />Print" /></td>
                        <td>|</td>
                        <td nowrap><asp:LinkButton ID="btnClose" runat="server" Text="<img src='/images/close-icon.gif' border='0' align='absmiddle' /><img src='/images/spacer.gif' border='0' width='5' height='1' />Close" /></td>
                    </tr>
                    <asp:Panel ID="panSave" runat="server" Visible="false">
                    <tr>
                        <td colspan="7" class="bigCheck" align="center"><img src="/images/bigCheck.gif" border="0" align="absmiddle" /> Deploy Successful</td>
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
    <%=strMenuTab1 %>
    <table width="100%" height="100%" cellpadding="10" cellspacing="5" border="0">
        <tr>
            <td valign="top">
                <div id="divMenu1" class="tabbing">
                    <div style="display:none">
                        <table width="100%" cellpadding="5" cellspacing="2" border="0">
                            <tr>
                                <td class="header" colspan="2">Asset Information&nbsp;&nbsp;<asp:Label ID="lblAssetID" runat="server" CssClass="default" /></td>
                            </tr>
                            <tr>
                                <td nowrap><asp:Label ID="fldName" runat="server" CssClass="default" Text="Device Name:" /></td>
                                <td width="100%"><asp:Label ID="lblName" runat="server" CssClass="default" /><asp:TextBox ID="txtName" runat="server" CssClass="default" MaxLength="100" Width="300" /></td>
                            </tr>
                            <tr>
                                <td nowrap><asp:Label ID="fldPlatformSerial" runat="server" CssClass="default" Text="Serial Number" /></td>
                                <td width="100%"><asp:Label ID="lblPlatformSerial" runat="server" CssClass="default" /><asp:TextBox ID="txtPlatformSerial" runat="server" CssClass="default" MaxLength="100" Width="400" /></td>
                            </tr>
                            <tr>
                                <td nowrap><asp:Label ID="fldPlatformAsset" runat="server" CssClass="default" Text="Asset Tag:" /></td>
                                <td width="100%"><asp:Label ID="lblPlatformAsset" runat="server" CssClass="default" /><asp:TextBox ID="txtPlatformAsset" runat="server" CssClass="default" MaxLength="100" Width="400" /></td>
                            </tr>
                            <tr>
                                <td nowrap><asp:Label ID="fldAssetAttribute" runat="server" CssClass="default" Text="Asset Attribute:" /></td>
                                <td width="100%"><asp:Label ID="lblAssetAttribute" runat="server" CssClass="default" /><asp:DropDownList ID="ddlAssetAttribute" runat="server" CssClass="default" Width="400" /></td>
                            </tr>
                             <tr>
                                <td nowrap><asp:Label ID="fldAssetAttributeComment" runat="server" CssClass="default" Text="Asset Attribute Comment:" /></td>
                                <td width="100%"><asp:Label ID="lblAssetAttributeComment" runat="server" CssClass="default" /><asp:TextBox ID="txtAssetAttributeComment" runat="server" CssClass="default" MaxLength="100" Width="400" TextMode="MultiLine" Rows="2"/></td>
                            </tr>
                            <tr>
                                <td nowrap><asp:Label ID="fldPlatform" runat="server" CssClass="default" Text="Platform:" /></td>
                                <td width="100%"><asp:Label ID="lblPlatform" runat="server" CssClass="default" /><asp:DropDownList ID="ddlPlatform" runat="server" CssClass="default" Width="400" /></td>
                            </tr>
                            <tr>
                                <td nowrap><asp:Label ID="fldPlatformType" runat="server" CssClass="default" Text="Type:" /></td>
                                <td width="100%"><asp:Label ID="lblPlatformType" runat="server" CssClass="default" /><asp:DropDownList ID="ddlPlatformType" runat="server" CssClass="default" Width="400" /></td>
                            </tr>
                            <tr>
                                <td nowrap><asp:Label ID="fldPlatformModel" runat="server" CssClass="default" Text="Model:" /></td>
                                <td width="100%"><asp:Label ID="lblPlatformModel" runat="server" CssClass="default" /><asp:DropDownList ID="ddlPlatformModel" runat="server" CssClass="default" Width="400" /></td>
                            </tr>
                            <tr>
                                <td nowrap><asp:Label ID="fldPlatformModelProperty" runat="server" CssClass="default" Text="Model Property:" /></td>
                                <td width="100%"><asp:Label ID="lblPlatformModelProperty" runat="server" CssClass="default" /><asp:DropDownList ID="ddlPlatformModelProperty" runat="server" CssClass="default" Width="400" /> <asp:Button ID="btnModel" runat="server" CssClass="default" Width="75" Text="Lookup" Visible="false" /></td>
                            </tr>
                            <tr>
                                <td nowrap><asp:Label ID="fldPlatformClass" runat="server" CssClass="default" Text="Class:" /></td>
                                <td width="100%"><asp:Label ID="lblPlatformClass" runat="server" CssClass="default" /><asp:DropDownList ID="ddlPlatformClass" runat="server" CssClass="default" Width="400" /></td>
                            </tr>
                            <tr>
                                <td nowrap><asp:Label ID="fldPlatformEnvironment" runat="server" CssClass="default" Text="Environment:" /></td>
                                <td width="100%"><asp:Label ID="lblPlatformEnvironment" runat="server" CssClass="default" /><asp:DropDownList ID="ddlPlatformEnvironment" runat="server" CssClass="default" Width="400" /></td>
                            </tr>
                            <tr>
                                <td nowrap><asp:Label ID="fldPlatformVLAN" runat="server" CssClass="default" Text="Original VLAN:" /></td>
                                <td width="100%"><asp:Label ID="lblPlatformVLAN" runat="server" CssClass="default" /><asp:TextBox ID="txtPlatformVLAN" runat="server" CssClass="default" MaxLength="4" Width="50" /></td>
                            </tr>
                            <tr>
                                <td nowrap><asp:Label ID="fldPlatformOA" runat="server" CssClass="default" Text="Onboard Administrator IP:" /></td>
                                <td width="100%"><asp:Label ID="lblPlatformOA" runat="server" CssClass="default" /><asp:TextBox ID="txtPlatformOA" runat="server" CssClass="default" MaxLength="50" Width="300" /></td>
                            </tr>
                            <tr>
                                <td nowrap><asp:Label ID="fldPlatformResiliency" runat="server" CssClass="default" Text="Resiliency:" /></td>
                                <td width="100%"><asp:Label ID="lblPlatformResiliency" runat="server" CssClass="default" /><asp:DropDownList ID="ddlPlatformResiliency" runat="server" CssClass="default" Width="400" /></td>
                            </tr>
                            <tr>
                                <td nowrap>Deployment Status:</td>
                                <td width="100%" class="required"><asp:DropDownList ID="ddlPlatformStatus" runat="server" CssClass="default" />&nbsp;&nbsp;Default: In Use</td>
                            </tr>
                        </table>
                    </div>
                    <div style="display:none">
                        <table id="tblLocation" width="100%" cellpadding="2" cellspacing="5" border="0" align="center">
                        <tr>
                            <td class="header" colspan="2"><asp:Label ID="fldLocation" runat="server" CssClass="header" Text="Location Information" /></td>
                        </tr>
                        <tr> 
                            <td nowrap>
                                <asp:Label ID="lblLocation" runat="server" CssClass="default" Text="Address :" />
                            </td>
                            <td width="100%">
                                <asp:TextBox ID="txtLocation" CssClass="lightdefault" runat="server"  Width="500" ReadOnly="true" />
                                <input type="hidden" id="hdnLocationId" runat="server" />
                                &nbsp
                                <asp:Button ID="btnSelectLocation" runat="server" Text="..." CssClass="default"  Width="25"   />
                            </td>
                        </tr>
                        
		                <tr> 
                            <td nowrap>
                                <asp:Label ID="lblRoom" runat="server" CssClass="default" Text="Room :" />
                            </td>
                            <td width="100%" >
                                <asp:TextBox ID="txtRoom" CssClass="lightdefault" runat="server"  Width="250" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr> 
                            <td nowrap>
                                <asp:Label ID="lblZone" runat="server" CssClass="default" Text="Zone :" />
                            </td>
                            <td width="100%" >
                                <asp:TextBox ID="txtZone" CssClass="lightdefault" runat="server"  Width="250" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td nowrap>
                                <asp:Label ID="lblRack" runat="server" CssClass="default" Text="Rack :" />
                            </td>
                            <td width="100%">
                                <asp:TextBox ID="txtRack" CssClass="lightdefault" runat="server"  Width="250" ReadOnly="true" />
                                <asp:HiddenField ID="hdnRackId" runat="server" />
                            </td>                            
                         </tr>
                         <tr>
                            <td nowrap >
                                <asp:Label ID="lblRackPosition" runat="server" CssClass="default" Text="Rack Position :" />
                            </td>                 
                            <td width="100%"">
                                <asp:TextBox ID="txtRackPosition" CssClass="default" runat="server"  MaxLength="10" Width="100" />
                                <asp:Label ID="lblRackPositionValue" runat="server" CssClass="default" />&nbsp;&nbsp;&nbsp;
                                <span class="footer">Examples: 10,&nbsp;&nbsp;10-11,&nbsp;&nbsp;10-12</span>
                            </td>
                         </tr>
                         <tr>
                            <td colspan="2">&nbsp;</td>
                         </tr>
                           <tr id="panOldLocationInfo" runat="server" visible="false">
                                <td colspan="2" >
                                    <table id="tblOldlocation" width="100%" cellpadding="0" cellspacing="5" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
                                        <tr>
                                            <td rowspan="3" valign="top"><img src="/images/hugeAlert.gif" border="0" align="absmiddle" /></td>
                                            <td class="header" width="100%" valign="bottom">Old Location Information</td>
                                        </tr>
                                        <tr>
                                            <td width="100%" valign="top">This information should be used as a reference and may be inaccurate.  This information can not be updated.</td>
                                        </tr>
                                        <tr>
                                            <td width="100%">
                                                <table cellpadding="3" cellspacing="2" border="0">
                                                    <tr>
                                                        <td valign="top">Location :</td>
                                                        <td width="100%" valign="top"><asp:Label ID="lblOldlocation" runat="server" CssClass="default" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="top">Room :</td>
                                                        <td width="100%" valign="top"><asp:Label ID="lblOldRoom" runat="server" CssClass="default" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="top">Rack :</td>
                                                        <td width="100%" valign="top"><asp:Label ID="lblOldRack" runat="server" CssClass="default" /></td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>

		        </table>
                    </div>
                    <div style="display:none">
                        <table width="100%" cellpadding="5" cellspacing="2" border="0">
                            <tr>
                                <td class="header" colspan="2">Virtual Connect IPs</td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <table width="100%" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                                        <asp:repeater ID="rptVirtualConnect" runat="server">
                                            <HeaderTemplate>
                                                <tr bgcolor="#EEEEEE">
                                                    <td></td>
                                                    <td nowrap><b>IP Address</b></td>
                                                    <td nowrap><b>Last Updated</b></td>
                                                </tr>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                                                    <td width="50%"><%# DataBinder.Eval(Container.DataItem, "virtual_connect")%></td>
                                                    <td width="50%"><%# DataBinder.Eval(Container.DataItem, "modified")%></td>
                                                </tr>
                                            </ItemTemplate>
                                            <AlternatingItemTemplate>
                                                <tr bgcolor="#F6F6F6">
                                                    <td><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                                                    <td width="50%"><%# DataBinder.Eval(Container.DataItem, "virtual_connect")%></td>
                                                    <td width="50%"><%# DataBinder.Eval(Container.DataItem, "modified")%></td>
                                                </tr>
                                            </AlternatingItemTemplate>
                                        </asp:repeater>
                                        <tr>
                                            <td colspan="5"><asp:Label ID="lblVirtualConnect" runat="server" CssClass="default" Visible="false" Text="<img src='/images/spacer.gif' border='0' height='1' width='25' /><img src='/images/alert.gif' border='0' align='absmiddle'> There are no virtual connect IPs" /></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2"><asp:Button ID="btnVCs" runat="server" CssClass="default" Width="100" Text="Manage" /></td>
                            </tr>
                        </table>
                    </div>
                    <div style="display:none">
                        <table width="100%" cellpadding="5" cellspacing="2" border="0">
                            <tr>
                                <td class="header" colspan="2">Resource Dependencies</td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <table width="100%" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                                        <asp:repeater ID="rptServiceRequests" runat="server">
                                            <HeaderTemplate>
                                                <tr bgcolor="#EEEEEE">
                                                    <td></td>
                                                    <td nowrap><b>Service Name</b></td>
                                                    <td nowrap><b>Progress</b></td>
                                                    <td nowrap><b>Submitted</b></td>
                                                    <td nowrap><b>Last Updated</b></td>
                                                </tr>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <asp:Label ID="lblServiceID" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem, "serviceid").ToString() %>' />
                                                    <td title='ResourceID: <%# DataBinder.Eval(Container.DataItem, "resourceid") %>'><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                                                    <td width="40%" nowrap title='Click here to view the details of this service'><asp:Label ID="lblDetails" runat="server" CssClass="default" Text='<%#DataBinder.Eval(Container.DataItem, "name").ToString() %>' /></td>
                                                    <td width="20%" nowrap><asp:Label ID="lblProgress" runat="server" CssClass="default" Text='<%#DataBinder.Eval(Container.DataItem, "resourceid").ToString() %>' /></td>
                                                    <td width="20%" nowrap><%# DataBinder.Eval(Container.DataItem, "submitted").ToString()%></td>
                                                    <td width="20%" nowrap><%# DataBinder.Eval(Container.DataItem, "modified").ToString()%></td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:repeater>
                                        <tr id="trServiceRequests" runat="server" visible="false">
                                            <td colspan="5"><img src="/images/spacer.gif" border="0" height="1" width="25" /><img src="/images/alert.gif" border="0" align="absmiddle"> There are no resource dependencies</td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="display:none">
                        <table width="100%" cellpadding="5" cellspacing="2" border="0">
                            <tr>
                                <td class="header" colspan="2">Provisioning History</td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <table width="100%" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                                        <asp:repeater ID="rptHistory" runat="server">
                                            <HeaderTemplate>
                                                <tr bgcolor="#EEEEEE">
                                                    <td></td>
                                                    <td nowrap><b>Status Changed To</b></td>
                                                    <td nowrap><b>Status Changed On</b></td>
                                                    <td nowrap><b>Status Changed By</b></td>
                                                    <td nowrap><b>Device Name</b></td>
                                                </tr>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                                                    <td width="25%"><%# DataBinder.Eval(Container.DataItem, "statusname")%></td>
                                                    <td width="25%"><%# DataBinder.Eval(Container.DataItem, "datestamp")%></td>
                                                    <td width="25%"><%# oUser.GetFullName(Int32.Parse(DataBinder.Eval(Container.DataItem, "userid").ToString())) %></td>
                                                    <td width="25%"><%# DataBinder.Eval(Container.DataItem, "name")%></td>
                                                </tr>
                                            </ItemTemplate>
                                            <AlternatingItemTemplate>
                                                <tr bgcolor="#F6F6F6">
                                                    <td><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                                                    <td width="25%"><%# DataBinder.Eval(Container.DataItem, "statusname")%></td>
                                                    <td width="25%"><%# DataBinder.Eval(Container.DataItem, "datestamp")%></td>
                                                    <td width="25%"><%# oUser.GetFullName(Int32.Parse(DataBinder.Eval(Container.DataItem, "userid").ToString())) %></td>
                                                    <td width="25%"><%# DataBinder.Eval(Container.DataItem, "name")%></td>
                                                </tr>
                                            </AlternatingItemTemplate>
                                        </asp:repeater>
                                        <tr>
                                            <td colspan="5"><asp:Label ID="lblHistory" runat="server" CssClass="default" Visible="false" Text="<img src='/images/spacer.gif' border='0' height='1' width='25' /><img src='/images/alert.gif' border='0' align='absmiddle'> There is no history of this asset" /></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </td>
        </tr>
    </table>
    </asp:Panel>
    <asp:Panel ID="panDenied" runat="server" Visible="false">
        <br />
            <table width="100%" cellpadding="0" cellspacing="5" border="0">
                <tr>
                    <td rowspan="2"><img src="/images/ico_error.gif" border="0" align="absmiddle" /></td>
                    <td class="header" width="100%" valign="bottom">Access Denied</td>
                </tr>
                <tr>
                    <td width="100%" valign="top">You do not have sufficient permission to view this page.</td>
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
<asp:HiddenField ID="hdnTab" runat="server" />
<asp:HiddenField ID="hdnModel" runat="server" />
<asp:HiddenField ID="hdnEnvironment" runat="server" />
<asp:HiddenField ID="hdnAddress" runat="server" />
</asp:Content>
