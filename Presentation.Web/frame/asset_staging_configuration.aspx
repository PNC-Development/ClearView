<%@ Page Language="C#" MasterPageFile="~/frame.Master" AutoEventWireup="True" CodeBehind="asset_staging_configuration.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.asset_staging_configuration" Title="Asset Staging and Configuration" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" runat="server">

    <script type="text/javascript">
        function ValidateInput() 
        {
        
        var hdnAssetCategoryId = document.getElementById('<%=hdnAssetCategoryId.ClientID %>');
        var oddl=null;
       
        if (hdnAssetCategoryId.value ==1 ||hdnAssetCategoryId.value ==2 ||hdnAssetCategoryId.value ==3 ||hdnAssetCategoryId.value ==4)
        {
        
            if (hdnAssetCategoryId.value ==4)
            {
                if (document.getElementById('<%=hdnZoneId.ClientID %>')!=null && 
                    document.getElementById('<%=hdnZoneId.ClientID %>').value !="")
                    if(ValidateHidden('<%=hdnZoneId.ClientID %>','<%=btnSelectLocation.ClientID %>' , 'Please select location staged')==false) return false;
                    
            }
            else
            {
                if (document.getElementById('<%=hdnRackId.ClientID %>')!=null && 
                    document.getElementById('<%=hdnRackId.ClientID %>').value !="")
                    if(ValidateHidden('<%=hdnRackId.ClientID %>','<%=btnSelectLocation.ClientID %>' , 'Please select location staged')==false) return false;
             }
          
            oddl=document.getElementById('<%=ddlEnclosure.ClientID %>');  
            if (oddl !=null && oddl.options[oddl.selectedIndex].value == "0" )
               if(ValidateDropDown('<%=ddlEnclosure.ClientID %>','Please select Enclosure')==false) return false;

            if (document.getElementById('<%=txtILO.ClientID %>')!=null && 
                document.getElementById('<%=txtILO.ClientID %>').value !="")
                  if( ValidateIPAddress('<%=txtILO.ClientID %>', 'Please enter valid Remote Management Address')==false) return false;   
            
            if (document.getElementById('<%=txtVLAN.ClientID %>')!=null && 
                document.getElementById('<%=txtVLAN.ClientID %>').value!="0" && 
                document.getElementById('<%=txtVLAN.ClientID %>').value!="-999")
                  if( ValidateNumber('<%=txtVLAN.ClientID %>', 'Please enter Original VLAN')==false) return false;  

            if (document.getElementById('<%=txtOAIP.ClientID %>')!=null && 
                document.getElementById('<%=txtOAIP.ClientID %>').value !="")
                return ValidateIPAddress('<%=txtOAIP.ClientID %>', 'Please enter Onboard Administrator IP Address');  
/*                
            if (document.getElementById('<%=ddlNetwork1.ClientID %>') != null && 
                    ValidateDropDown('<%=ddlNetwork1.ClientID %>','Please select a Switch')==false) 
               return false;
            if (document.getElementById('<%=hdnNetwork1Blade.ClientID %>') != null)
            {
                if (document.getElementById('<%=ddlNetwork1Blade.ClientID %>') != null && ValidateHidden0('<%=hdnNetwork1Blade.ClientID %>','<%=ddlNetwork1Blade.ClientID %>','Please select a Blade')==false) 
                   return false;
                if (document.getElementById('<%=txtNetwork1FexID.ClientID %>') != null && ValidateText('<%=txtNetwork1FexID.ClientID %>','Please enter a FEX ID')==false) 
                   return false;
            } 
                    
            if (document.getElementById('<%=hdnNetwork1Port.ClientID %>') != null && 
                    ValidateHidden0('<%=hdnNetwork1Port.ClientID %>','<%=ddlNetwork1Port.ClientID %>','Please select a Port')==false) 
               return false;
            if (document.getElementById('<%=txtNetwork1Interface.ClientID %>')!= null &&
                    ValidateText('<%=txtNetwork1Interface.ClientID %>', 'Please enter an interface')==false) 
                return false;

            if (document.getElementById('<%=ddlNetwork2.ClientID %>') != null && 
                    ValidateDropDown('<%=ddlNetwork2.ClientID %>','Please select a Switch')==false) 
               return false;
            if (document.getElementById('<%=hdnNetwork2Blade.ClientID %>') != null)
            {
                if (document.getElementById('<%=ddlNetwork2Blade.ClientID %>') != null && ValidateHidden0('<%=hdnNetwork2Blade.ClientID %>','<%=ddlNetwork2Blade.ClientID %>','Please select a Blade')==false) 
                   return false;
                if (document.getElementById('<%=txtNetwork2FexID.ClientID %>') != null && ValidateText('<%=txtNetwork2FexID.ClientID %>','Please enter a FEX ID')==false) 
                   return false;
            } 
            if (document.getElementById('<%=hdnNetwork2Port.ClientID %>') != null && 
                    ValidateHidden0('<%=hdnNetwork2Port.ClientID %>','<%=ddlNetwork2Port.ClientID %>','Please select a Port')==false) 
               return false;
            if (document.getElementById('<%=txtNetwork2Interface.ClientID %>')!= null &&
                    ValidateText('<%=txtNetwork2Interface.ClientID %>', 'Please enter an interface')==false) 
                return false;
*/            
            oddl=document.getElementById('<%=ddlResiliency.ClientID %>');  
            if (oddl !=null && oddl.options[oddl.selectedIndex].value == "0" )
               if(ValidateDropDown('<%=ddlResiliency.ClientID %>','Please select a Resiliency')==false) return false;

        }
        
        return true;    
   }
        
   
  
  
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
    
        <table width="100%" cellpadding="5" cellspacing="3" border="0">
            <tr>
                <td class="header" colspan="2">Asset Information&nbsp;&nbsp;<asp:Label ID="lblAssetID" runat="server" CssClass="default" /></td>
            </tr>
              <tr id="trDeviceName" runat="server" >
                <td width="20%"><asp:Label ID="lblDeviceName" runat="server" CssClass="default" Text="Device Name:" /></td>
                <td width="80%">
                    <asp:TextBox ID="txtDeviceName" CssClass="default" runat="server"  MaxLength="100" Width="400"  />
                </td>
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
            <tr id="trModel" runat="server" visible="false" >
                <td ><asp:Label ID="lblModel" runat="server" CssClass="default" Text="Model:" /></td>
                <td >
                    <asp:TextBox ID="txtModel" CssClass="lightdefault" runat="server"  Width="400" ReadOnly="true" />
                </td>
            </tr>
            <tr id="trModelProperty" runat="server" >
                <td ><asp:Label ID="lblModelProperty" runat="server" CssClass="default" Text="Model:" /></td>
                <td >
                    <asp:TextBox ID="txtModelProperty" CssClass="lightdefault" runat="server"  Width="400" ReadOnly="true" />
                    <asp:HiddenField ID="hdnAssetCategoryId" runat="server" />
                    <asp:HiddenField ID="hdnModelPropertyId" runat="server" />
                </td>
            </tr>
             <tr id="trLocation" runat="server" >
                <td >
                    <asp:Label ID="lblLocation" runat="server" CssClass="default" Text="Intended Location :<font class='required'>&nbsp;*</font>" />
                </td>
                <td >
                    <asp:TextBox ID="txtLocation" CssClass="lightdefault" runat="server"  Width="500" ReadOnly="true" />
                    <asp:HiddenField ID="hdnLocationId" runat="server" />
                    &nbsp
                    <asp:Button ID="btnSelectLocation" runat="server" Text="..." CssClass="default"  Width="25"   />
                    <br />
                    <asp:Label ID="lblIntendedLocation" runat="server" CssClass="note" Text="" />
                </td>
            </tr>

            <tr id="trRoom" runat="server"  >
                <td >
                    <asp:Label ID="lblRoom" runat="server" CssClass="default" Text="Intended Room:<font class='required'>&nbsp;*</font>" />
                </td>
                <td >
                    <asp:TextBox ID="txtRoom" CssClass="lightdefault" runat="server"  Width="250" ReadOnly="true" />
                    <asp:HiddenField ID="hdnRoomId" runat="server" />
                    <br />
                    <asp:Label ID="lblIntendedRoom" runat="server" CssClass="note" Text="" />
                </td>
            </tr>
            <tr id="trZone" runat="server"  >
                <td >
                    <asp:Label ID="lblZone" runat="server" CssClass="default" Text="Intended Zone:<font class='required'>&nbsp;*</font>" />
                </td>
                <td >
                    <asp:TextBox ID="txtZone" CssClass="lightdefault" runat="server"  Width="250" ReadOnly="true" />
                    <asp:HiddenField ID="hdnZoneId" runat="server" />
                     <br />
                    <asp:Label ID="lblIntendedZone" runat="server" CssClass="note" Text="" />
                </td>
             </tr>
             <tr id="trRack" runat="server"  >
                <td >
                    <asp:Label ID="lblRack" runat="server" CssClass="default" Text="Intended Rack:<font class='required'>&nbsp;*</font>" />
                </td>
                <td >
                    <asp:TextBox ID="txtRack" CssClass="lightdefault" runat="server"  Width="250" ReadOnly="true" />
                    <asp:HiddenField ID="hdnRackId" runat="server" />
                    <br />
                    <asp:Label ID="lblIntendedRack" runat="server" CssClass="note" Text="" />
                </td>                            
             </tr>

            
            <tr id="trRackPosition" runat="server" >
                <td nowrap><asp:Label ID="lblRackPosition" runat="server" CssClass="default"  Text="Rack Position:" /></td>
                <td >
                    <asp:TextBox ID="txtRackPosition" CssClass="default" runat="server"  MaxLength="10" Width="100" />
                    <br />
                    <asp:Label ID="lblIntendedRackPosition" runat="server" CssClass="note" Text="" />
                </td>
            </tr>
            <tr id="trClass" runat="server" >
                <td nowrap><asp:Label ID="lblClass" runat="server" CssClass="default" Text="Current Class:" /></td>
                <td ><asp:DropDownList ID="ddlClass" runat="server" CssClass="default" Width="400" /><br />
                <asp:Label ID="lblIntendedClass" runat="server" CssClass="note" Text="" /></td>
            </tr>
            <tr id="trEnvironment" runat="server" >
                <td nowrap><asp:Label ID="lblEnvironment" runat="server" CssClass="default" Text="Current Environment:" /></td>
                <td ><asp:DropDownList ID="ddlEnvironment" runat="server" CssClass="default" Width="400" /><br />
                <asp:Label ID="lblIntendedEnvironment" runat="server" CssClass="note" Text="" />
                </td>
            </tr>
            <tr id="trEnclosure" runat="server" >
                <td ><asp:Label ID="lblEnclosure" runat="server" CssClass="default" Text="Enclosure:" /></td>
                <td ><asp:DropDownList ID="ddlEnclosure" runat="server" CssClass="default" Width="400" /><br />
                    <asp:Label ID="lblIntendedEnclosure" runat="server" CssClass="note" Text="" />
                </td>
            </tr>
            <tr id="trSlot" runat="server" >
                <td ><asp:Label ID="lblSlot" runat="server" CssClass="default" Text="Slot:" /></td>
                <td><asp:TextBox ID="txtSlot" runat="server" CssClass="default" MaxLength="10" Width="100" /><br />
                    <asp:Label ID="lblIntendedSlot" runat="server" CssClass="note" Text="" />
                </td>
            </tr>
            <tr id="trSpare" runat="server" >
                <td ><asp:Label ID="lblSpare" runat="server" CssClass="default" Text="Spare:" /></td>
                <td ><asp:CheckBox ID="chkSpare" runat="server" CssClass="default" /></td>
            </tr>
            <tr  id="trILO" runat="server" >
                <td nowrap><asp:Label ID="lblILO" runat="server" CssClass="default" Text="Remote Management Address:"/></td>
                <td ><asp:TextBox ID="txtILO" runat="server" CssClass="default" MaxLength="50" Width="300"   />
                    &nbsp;&nbsp;&nbsp;<span class="footer">Examples: 11.11.1.111</span>
                </td>
            </tr>
            <tr id="trDummyName" runat="server" >
                <td nowrap><asp:Label ID="lblDummyName" runat="server" CssClass="default" Text="Dummy Name:" /></td>
                <td >
                    <asp:TextBox ID="txtDummyName" runat="server" CssClass="default" MaxLength="50" Width="300" />
                    &nbsp;&nbsp;&nbsp;<img src="/images/help.gif" border="0" align="absmiddle" /> <a href="javascript:void(0);" onclick="ShowHideDiv2('trDummyHelp');">Show Help</a>
                </td>
            </tr>
            <tr id="trDummyHelp" style="display:none">
                <td colspan="2">
                    <table cellpadding="3" cellspacing="3" border="1">
                        <tr>
                            <td align="left" class="bold"></td>
                            <td align="left" class="bold">Format:</td>
                            <td align="left" class="bold">Example:</td>
                        </tr>
                        <tr>
                            <td align="left" class="bold">Non-Blade</td>
                            <td align="left">Serial Number</td>
                            <td align="left">17QQPN1</td>
                        </tr>
                        <tr>
                            <td align="left" class="bold">HP Blades</td>
                            <td align="left">Enclosure + Slot</td>
                            <td align="left">CIBCO010AE305</td>
                        </tr>
                        <tr>
                            <td align="left" class="bold">DELL Half Height Blades</td>
                            <td align="left">Enclosure + &quot;_&quot; + Slot</td>
                            <td align="left">CMCCLEPR001_05</td>
                        </tr>
                        <tr>
                            <td align="left" class="bold">DELL Full Height Blades</td>
                            <td align="left">Enclosure + &quot;_&quot; + Top Slot + &quot;-&quot; Bottom Slot</td>
                            <td align="left">CMCCLEPR001_05-13</td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr id="trVLAN" runat="server" >
                <td nowrap><asp:Label ID="lblVLAN" runat="server" CssClass="default" Text="Original VLAN:" /></td>
                <td >
                    <asp:TextBox ID="txtVLAN" runat="server" CssClass="default" MaxLength="4" Width="50" />
                    &nbsp;&nbsp;&nbsp;<span class="footer">Nexus Attached = &quot;999&quot;, DHCP = &quot;-999&quot;</span>
                </td>
            </tr>
            <tr id="trBuildNetwork" runat="server" >
                <td nowrap><asp:Label ID="lblBuildNetwork" runat="server" CssClass="default" Text="Build Network:" /></td>
                <td ><asp:DropDownList ID="ddlBuildNetwork" runat="server" CssClass="default" Width="400" /></td>
            </tr>
            <tr id="trOAIP" runat="server" >
                    <td ><asp:Label ID="lblOAIP" runat="server" CssClass="default" Text="Onboard Administrator IP:" /></td>
                    <td ><asp:TextBox ID="txtOAIP" runat="server" CssClass="default" MaxLength="15" Width="100" />
                         &nbsp;&nbsp;&nbsp;<span class="footer">Examples: 11.11.1.111</span>
                    </td>
            </tr>
            <tr id="trResiliency" runat="server" >
                <td nowrap><asp:Label ID="lblResiliency" runat="server" CssClass="default" Text="Resiliency:" /></td>
                <td >
                    <asp:DropDownList ID="ddlResiliency" runat="server" CssClass="default" Width="400" />
                    <br />
                    <asp:Label ID="lblIntendedResiliency" runat="server" CssClass="note" Text="" />
                </td>
            </tr>
            <tr id="trOperatingSystemGroup" runat="server" >
                <td nowrap><asp:Label ID="lblOperatingSystemGroup" runat="server" CssClass="default" Text="Operating System Group:" /></td>
                <td >
                    <asp:DropDownList ID="ddlOperatingSystemGroup" runat="server" CssClass="default" Width="400" />
                    <br />
                    <asp:Label ID="lblIntendedOperatingSystemGroup" runat="server" CssClass="note" Text="" />
                </td>
            </tr>
            <tr>
                <td colspan="2"></td>
            </tr>
            <asp:Panel ID="pnlWWPortNames" runat="server"  Visible="true">
            <tr>
                <td colspan="2" class="framegreen">World Wide Port Names</td>
            </tr>
            <tr >
                <td><b>World Wide Port Name</b></td>
                <td>
                    <asp:TextBox ID="txtWWPortName" runat="server" CssClass="default" Width="200" MaxLength="100" />
                    <asp:Button ID="btnAddWWPortName" runat="server" CssClass="default" Text="Add" OnClick ="btnAddWWPortName_Click"/>
                </td>
            </tr>
            <tr>
                <td colspan="2"> 
                    <asp:Panel ID="pnlWWPortNamesList" runat="server" Visible="true"   height="100%" ScrollBars="Auto" >
                        <asp:DataList ID="dlWWPortNamesList" runat="server" CellPadding="3" CellSpacing="1" Width="100%" OnItemDataBound="dlWWPortNamesList_ItemDataBound" >
                            <HeaderTemplate>
                                <tr bgcolor="#EEEEEE">
                                    <td align="left" width="60%">
                                        <asp:Label ID="lblWWPortNamesListHeaderListName" runat="server" CssClass="default" Text="<b>World Wide Port Name</b>"  />
                                    </td>
                                    <td align="left" width="40%">
                                        <asp:Label ID="lblWWPortNamesListHeaderLastUpdated" runat="server" CssClass="default" Text="<b>Last Updated</b>"  />
                                    </td>
                                    <td align="left" width="0%">
                                    </td>
                                  </tr>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td align="left" >
                                        <asp:Label ID="lblWWPortNamesListName" runat="server" CssClass="default" />
                                    </td>
                                    <td align="left" >
                                        <asp:Label ID="lblWWPortNamesListLastUpdated" runat="server" CssClass="default" />
                                    </td>
                                    <td align="left" >
                                        [<asp:LinkButton ID="lnkbtnWWPortNamesListDelete" runat="server" OnClick="lnkbtnWWPortNamesListDelete_Click" Text="Delete" />]
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <AlternatingItemTemplate>
                                <tr bgcolor="#F6F6F6">
                                    <td align="left" >
                                        <asp:Label ID="lblWWPortNamesListName" runat="server" CssClass="default" />
                                    </td>
                                    <td align="left" >
                                        <asp:Label ID="lblWWPortNamesListLastUpdated" runat="server" CssClass="default" />
                                    </td>
                                     <td align="left" >
                                        [<asp:LinkButton ID="lnkbtnWWPortNamesListDelete" runat="server" OnClick="lnkbtnWWPortNamesListDelete_Click" Text="Delete" />]
                                    </td>
                                </tr>
                            </AlternatingItemTemplate>
                        </asp:DataList>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                 <td colspan="2"><asp:Label ID="lblWWPNamesNoItems" runat="server" CssClass="default" Visible="false" Text="<img src='/images/spacer.gif' border='0' height='1' width='25' /><img src='/images/alert.gif' border='0' align='absmiddle'> There are no World Wide Port Names" /></td>
            </tr>
            </asp:Panel>
            
            <asp:Panel ID="panSwitchesPending" runat="server"  Visible="true">
            <tr>
                <td colspan="2"></td>
            </tr>
            <tr>
                <td colspan="2" class="framegreen">SwitchPort Assignments</td>
            </tr>
            <tr>
                <td colspan="2">Please click &quot;Save&quot; to enable switchport assignments</td>
            </tr>
            </asp:Panel>
            
            <asp:Panel ID="panSwitches" runat="server"  Visible="true">
            <tr>
                <td colspan="2"></td>
            </tr>
            <tr>
                <td colspan="2" class="framegreen">SwitchPort Assignments</td>
            </tr>
            <tr>
                <td colspan="2">
                    <table cellpadding="3" cellspacing="2" border="0">
                        <tr>
                            <td></td>
                            <td><b>Switch</b></td>
                            <td><b>Blade / FEX ID</b></td>
                            <td><b>Port</b></td>
                            <td><b>Interface</b> (Ex: fa0/44)</td>
                        </tr>
                        <tr>
                            <td>Network # 1 <font class='required'>&nbsp;*</font></td>
                            <td><asp:DropDownList ID="ddlNetwork1" runat="server" Width="150" /></td>
                            <td>
                                <asp:DropDownList ID="ddlNetwork1Blade" runat="server" Width="150" />
                                <asp:TextBox ID="txtNetwork1FexID" runat="server" Width="150" MaxLength="50" />
                            </td>
                            <td><asp:DropDownList ID="ddlNetwork1Port" runat="server" Width="150" /></td>
                            <td>
                                <asp:TextBox ID="txtNetwork1Interface" runat="server" Width="100" MaxLength="20" Enabled="false" />
                                <asp:Label ID="lblNetwork1Interface" runat="server" Text="<i>Not Configured</i>" />
                            </td>
                        </tr>
                        <tr>
                            <td>Network # 2 <font class='required'>&nbsp;*</font></td>
                            <td><asp:DropDownList ID="ddlNetwork2" runat="server" Width="150" /></td>
                            <td>
                                <asp:DropDownList ID="ddlNetwork2Blade" runat="server" Width="150" />
                                <asp:TextBox ID="txtNetwork2FexID" runat="server" Width="150" MaxLength="50" />
                            </td>
                            <td><asp:DropDownList ID="ddlNetwork2Port" runat="server" Width="150" /></td>
                            <td>
                                <asp:TextBox ID="txtNetwork2Interface" runat="server" Width="100" MaxLength="20" Enabled="false" />
                                <asp:Label ID="lblNetwork2Interface" runat="server" Text="<i>Not Configured</i>" />
                            </td>
                        </tr>
                        <tr>
                            <td>Remote Mgmt <font class='required'>&nbsp;*</font></td>
                            <td><asp:DropDownList ID="ddlRemote" runat="server" Width="150" /></td>
                            <td>
                                <asp:DropDownList ID="ddlRemoteBlade" runat="server" Width="150" />
                                <asp:TextBox ID="txtRemoteFexID" runat="server" Width="150" MaxLength="50" />
                            </td>
                            <td><asp:DropDownList ID="ddlRemotePort" runat="server" Width="150" /></td>
                            <td>
                                <asp:TextBox ID="txtRemoteInterface" runat="server" Width="100" MaxLength="20" Enabled="false" />
                                <asp:Label ID="lblRemoteInterface" runat="server" Text="<i>Not Configured</i>" />
                            </td>
                        </tr>
                        <tr>
                            <td>Backup (Optional) </td>
                            <td><asp:DropDownList ID="ddlBackup" runat="server" Width="150" /></td>
                            <td>
                                <asp:DropDownList ID="ddlBackupBlade" runat="server" Width="150" />
                                <asp:TextBox ID="txtBackupFexID" runat="server" Width="150" MaxLength="50" />
                            </td>
                            <td><asp:DropDownList ID="ddlBackupPort" runat="server" Width="150" /></td>
                            <td>
                                <asp:TextBox ID="txtBackupInterface" runat="server" Width="100" MaxLength="20" Enabled="false" />
                                <asp:Label ID="lblBackupInterface" runat="server" Text="<i>Not Configured</i>" />
                            </td>
                        </tr>
                        <tr>
                            <td>Cluster # 1 </td>
                            <td><asp:DropDownList ID="ddlCluster1" runat="server" Width="150" /></td>
                            <td>
                                <asp:DropDownList ID="ddlCluster1Blade" runat="server" Width="150" />
                                <asp:TextBox ID="txtCluster1FexID" runat="server" Width="150" MaxLength="50" />
                            </td>
                            <td><asp:DropDownList ID="ddlCluster1Port" runat="server" Width="150" /></td>
                            <td>
                                <asp:TextBox ID="txtCluster1Interface" runat="server" Width="100" MaxLength="20" Enabled="false" />
                                <asp:Label ID="lblCluster1Interface" runat="server" Text="<i>Not Configured</i>" />
                            </td>
                        </tr>
                        <tr>
                            <td>Cluster # 2 </td>
                            <td><asp:DropDownList ID="ddlCluster2" runat="server" Width="150" /></td>
                            <td>
                                <asp:DropDownList ID="ddlCluster2Blade" runat="server" Width="150" />
                                <asp:TextBox ID="txtCluster2FexID" runat="server" Width="150" MaxLength="50" />
                            </td>
                            <td><asp:DropDownList ID="ddlCluster2Port" runat="server" Width="150" /></td>
                            <td>
                                <asp:TextBox ID="txtCluster2Interface" runat="server" Width="100" MaxLength="20" Enabled="false" />
                                <asp:Label ID="lblCluster2Interface" runat="server" Text="<i>Not Configured</i>" />
                            </td>
                        </tr>
                        <tr>
                            <td>Storage # 1</td>
                            <td><asp:DropDownList ID="ddlStorage1" runat="server" Width="150" /></td>
                            <td><asp:DropDownList ID="ddlStorage1Blade" runat="server" Width="150" /></td>
                            <td><asp:DropDownList ID="ddlStorage1Port" runat="server" Width="150" /></td>
                            <td><asp:TextBox ID="txtStorage1Interface" runat="server" Width="100" MaxLength="20" Enabled="false" /></td>
                        </tr>
                        <tr>
                            <td>Storage # 2</td>
                            <td><asp:DropDownList ID="ddlStorage2" runat="server" Width="150" /></td>
                            <td><asp:DropDownList ID="ddlStorage2Blade" runat="server" Width="150" /></td>
                            <td><asp:DropDownList ID="ddlStorage2Port" runat="server" Width="150" /></td>
                            <td><asp:TextBox ID="txtStorage2Interface" runat="server" Width="100" MaxLength="20" Enabled="false" /></td>
                        </tr>
                    </table>
                    <asp:HiddenField ID="hdnNetwork1Blade" runat="server" />
                    <asp:HiddenField ID="hdnNetwork1Port" runat="server" />
                    <asp:HiddenField ID="hdnNetwork2Blade" runat="server" />
                    <asp:HiddenField ID="hdnNetwork2Port" runat="server" />
                    <asp:HiddenField ID="hdnBackupBlade" runat="server" />
                    <asp:HiddenField ID="hdnBackupPort" runat="server" />
                    <asp:HiddenField ID="hdnRemoteBlade" runat="server" />
                    <asp:HiddenField ID="hdnRemotePort" runat="server" />
                    <asp:HiddenField ID="hdnCluster1Blade" runat="server" />
                    <asp:HiddenField ID="hdnCluster1Port" runat="server" />
                    <asp:HiddenField ID="hdnCluster2Blade" runat="server" />
                    <asp:HiddenField ID="hdnCluster2Port" runat="server" />
                    <asp:HiddenField ID="hdnStorage1Blade" runat="server" />
                    <asp:HiddenField ID="hdnStorage1Port" runat="server" />
                    <asp:HiddenField ID="hdnStorage2Blade" runat="server" />
                    <asp:HiddenField ID="hdnStorage2Port" runat="server" />
                </td>
            </tr>
            <tr>
                 <td colspan="2">
                    <table width="100%" cellpadding="0" cellspacing="5" border="0">
                        <tr>
                            <td rowspan="2"><img src="/images/tack.gif" border="0" align="absmiddle" /></td>
                            <td width="100%" valign="bottom"><b>NOTE:</b> The list of switches is populated based on the RACK (<asp:Label ID="lblSwitchRack" runat="server" />) of this device.</td>
                        </tr>
                        <tr>
                            <td width="100%" valign="top">If a switch does not exist in the list, please contact your ClearView administrator to have it added.</td>
                        </tr>
                    </table>
                </td>
            </tr>
            </asp:Panel>
            
            <tr>
                <td colspan="2"><hr /></td>
            </tr>
                <tr align="right">
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
<asp:HiddenField ID="hdnTab" runat="server" />
<asp:HiddenField ID="hdnOrderId" runat="server" />
<asp:HiddenField ID="hdnAssetId" runat="server" />
<asp:HiddenField ID="hdnEnvironment" runat="server" />

</asp:Content>
