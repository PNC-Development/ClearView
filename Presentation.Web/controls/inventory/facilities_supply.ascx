<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="facilities_supply.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.facilities_supply" %>

<script type="text/javascript">
</script>
<asp:Panel ID="panLocations" runat="server" Visible="false">
    <table width="100%" cellpadding="4" cellspacing="0" border="0" align="center" style="border:solid 1px #CCCCCC">
        <tr bgcolor="#EEEEEE">
            <td width="40%" nowrap><b>Data Center</b></td>
            <td width="15%" nowrap><b>Rooms</b></td>
            <td width="15%" nowrap><b>Zones</b></td>
            <td width="15%" nowrap><b>Racks</b></td>
            <td width="15%" nowrap><b>Status</b></td>
        </tr>
        <asp:Repeater ID="rptLocations" runat="server">
            <ItemTemplate>
                <tr class='<%# DataBinder.Eval(Container.DataItem, "enabled").ToString() == "1" ? "default" : "component_unavailable" %>' onmouseover="CellRowOver(this);" onmouseout="CellRowOut(this);" onclick="if(event.srcElement.tagName != 'IMG') { LoadWait();window.navigate('<%# FormURL("l=" + DataBinder.Eval(Container.DataItem, "id")) %>'); }">
                    <td><%# DataBinder.Eval(Container.DataItem, "commonname") %></td>
                    <td><%# DataBinder.Eval(Container.DataItem, "rooms") %></td>      
                    <td><%# DataBinder.Eval(Container.DataItem, "zones") %></td>      
                    <td><%# DataBinder.Eval(Container.DataItem, "racks") %></td>      
                    <td><%# DataBinder.Eval(Container.DataItem, "enabled").ToString() == "1" ? "Available" : "Disabled" %></td>
                </tr>
            </ItemTemplate>
        </asp:repeater>
        <tr>
            <td colspan="10"><asp:Label ID="lblLocations" runat="server" CssClass="error" Visible="false" Text="<img src='/images/bigAlert.gif' border='0' align='absmiddle'> There are no locations" /></td>
        </tr>
    </table>
    <br /><br />
    <asp:Button ID="btnAddLocation" runat="server" CssClass="default" Width="75" Text="Add New" OnClick="btnAddLocation_Click" OnClientClick="ProcessButton(this) && LoadWait();" />
</asp:Panel>

<asp:Panel ID="panLocation" runat="server" Visible="false">
    <asp:Label ID="lblLocationCityID" runat="server" Visible="false" />
    <table width="100%" cellpadding="4" cellspacing="3" border="0">
        <tr>
            <td nowrap>Name:</td>
            <td width="100%"><asp:TextBox ID="txtLocationName" runat="server" CssClass="default" Width="300" MaxLength="50" /></td>
        </tr>
        <tr>
            <td nowrap>Address:</td>
            <td width="100%"><asp:TextBox ID="txtLocationAddress" runat="server" CssClass="default" Width="400" MaxLength="100" /></td>
        </tr>
        <tr>
            <td nowrap>Server Name Code:</td>
            <td width="100%"><asp:TextBox ID="txtLocationCode" runat="server" CssClass="default" Width="50" MaxLength="5" /></td>
        </tr>
        <tr>
            <td nowrap>Storage Available:</td>
            <td width="100%"><asp:CheckBox ID="chkLocationStorage" runat="server" CssClass="default" /></td>
        </tr>
        <tr>
            <td nowrap>Backup Available:</td>
            <td width="100%"><asp:CheckBox ID="chkLocationTSM" runat="server" CssClass="default" /></td>
        </tr>
        <tr>
            <td nowrap>Location:</td>
            <td width="100%">
                <asp:CheckBox ID="chkLocationProd" runat="server" CssClass="default" Text="Production" />
                <asp:CheckBox ID="chkLocationQA" runat="server" CssClass="default" Text="QA" />
                <asp:CheckBox ID="chkLocationTest" runat="server" CssClass="default" Text="Test" />
                <asp:CheckBox ID="chkLocationDR" runat="server" CssClass="default" Text="Disaster Recovery" />
            </td>
        </tr>
        <tr>
            <td nowrap>Building Code:</td>
            <td width="100%"><asp:TextBox ID="txtLocationBuildingCode" runat="server" CssClass="default" Width="200" MaxLength="20" /></td>
        </tr>
        <tr>
            <td nowrap>Assign Build IP:</td>
            <td width="100%"><asp:CheckBox ID="chkLocationAssignIP" runat="server" CssClass="default" /></td>
        </tr>
        <tr>
            <td nowrap>Available:</td>
            <td width="100%"><asp:CheckBox ID="chkLocationAvailable" runat="server" CssClass="default" /></td>
        </tr>
        <tr>
            <td nowrap></td>
            <td width="100%">
                <asp:Button ID="btnLocationAdd" runat="server" CssClass="default" Width="100" Text="Add" OnClick="btnLocationAdd_Click" Visible="false" />
                <asp:Button ID="btnLocationUpdate" runat="server" CssClass="default" Width="100" Text="Update" OnClick="btnLocationUpdate_Click" Visible="false" />
                &nbsp;<asp:Button ID="btnLocationDelete" runat="server" CssClass="default" Width="100" Text="Delete" OnClick="btnLocationDelete_Click" Visible="false" />
                &nbsp;<asp:Button ID="btnLocationCancel" runat="server" CssClass="default" Width="100" Text="Cancel" OnClick="btnLocationCancel_Click" />
            </td>
        </tr>
    </table>
    <br /><br />
    <span class="header">Rooms</span>
    <br /><br />
    <table width="100%" cellpadding="4" cellspacing="0" border="0" align="center" style="border:solid 1px #CCCCCC">
        <tr bgcolor="#EEEEEE">
            <td width="55%" nowrap><b>Room</b></td>
            <td width="15%" nowrap><b>Zones</b></td>
            <td width="15%" nowrap><b>Racks</b></td>
            <td width="15%" nowrap><b>Status</b></td>
        </tr>
    <asp:repeater ID="rptRooms" runat="server">
        <ItemTemplate>
                <tr class='<%# DataBinder.Eval(Container.DataItem, "enabled").ToString() == "1" ? "default" : "component_unavailable" %>' onmouseover="CellRowOver(this);" onmouseout="CellRowOut(this);" onclick="if(event.srcElement.tagName != 'IMG') { LoadWait();window.navigate('<%# FormURL("l=" + Request.QueryString["l"] + "&ro=" + DataBinder.Eval(Container.DataItem, "id")) %>'); }">
                    <td><%# DataBinder.Eval(Container.DataItem, "name") %></td>
                    <td><%# DataBinder.Eval(Container.DataItem, "zones") %></td>      
                    <td><%# DataBinder.Eval(Container.DataItem, "racks") %></td>      
                    <td><%# DataBinder.Eval(Container.DataItem, "enabled").ToString() == "1" ? "Available" : "Disabled" %></td>
                </tr>
        </ItemTemplate>
    </asp:repeater>
        <tr>
            <td colspan="10"><asp:Label ID="lblRooms" runat="server" CssClass="error" Visible="false" Text="<img src='/images/bigAlert.gif' border='0' align='absmiddle'> There are no rooms for this location" /></td>
        </tr>
    </table>
    <br /><br />
    <asp:Button ID="btnAddRoom" runat="server" CssClass="default" Width="75" Text="Add New" OnClick="btnAddRoom_Click" OnClientClick="ProcessButton(this) && LoadWait();" />
</asp:Panel>

<asp:Panel ID="panRoom" runat="server" Visible="false">
    <table width="100%" cellpadding="4" cellspacing="3" border="0">
        <tr>
            <td nowrap>Location:</td>
            <td width="100%"><asp:Label ID="lblRoomLocation" runat="server" CssClass="default" /></td>
        </tr>
        <tr>
            <td nowrap>Name:</td>
            <td width="100%"><asp:TextBox ID="txtRoomName" runat="server" CssClass="default" Width="400" MaxLength="100" /></td>
        </tr>
        <tr>
            <td nowrap>Description:</td>
            <td width="100%"><asp:TextBox ID="txtRoomDescription" runat="server" CssClass="default" Width="400" TextMode="MultiLine" Rows="5" /></td>
        </tr>
        <tr>
            <td nowrap>Available:</td>
            <td width="100%"><asp:CheckBox ID="chkRoomAvailable" runat="server" CssClass="default" /></td>
        </tr>
        <tr>
            <td nowrap></td>
            <td width="100%">
                <asp:Button ID="btnRoomAdd" runat="server" CssClass="default" Width="100" Text="Add" OnClick="btnRoomAdd_Click" Visible="false" />
                <asp:Button ID="btnRoomUpdate" runat="server" CssClass="default" Width="100" Text="Update" OnClick="btnRoomUpdate_Click" Visible="false" />
                &nbsp;<asp:Button ID="btnRoomDelete" runat="server" CssClass="default" Width="100" Text="Delete" OnClick="btnRoomDelete_Click" Visible="false" />
                &nbsp;<asp:Button ID="btnRoomCancel" runat="server" CssClass="default" Width="100" Text="Cancel" OnClick="btnRoomCancel_Click" />
            </td>
        </tr>
    </table>
    <br /><br />
    <span class="header">Zones</span>
    <br /><br />
    <table width="100%" cellpadding="4" cellspacing="0" border="0" align="center" style="border:solid 1px #CCCCCC">
        <tr bgcolor="#EEEEEE">
            <td width="70%" nowrap><b>Zone</b></td>
            <td width="15%" nowrap><b>Racks</b></td>
            <td width="15%" nowrap><b>Status</b></td>
        </tr>
    <asp:repeater ID="rptZones" runat="server">
        <ItemTemplate>
                <tr class='<%# DataBinder.Eval(Container.DataItem, "enabled").ToString() == "1" ? "default" : "component_unavailable" %>' onmouseover="CellRowOver(this);" onmouseout="CellRowOut(this);" onclick="if(event.srcElement.tagName != 'IMG') { LoadWait();window.navigate('<%# FormURL("ro=" + Request.QueryString["ro"] + "&z=" + DataBinder.Eval(Container.DataItem, "id")) %>'); }">
                    <td><%# DataBinder.Eval(Container.DataItem, "name") %></td>
                    <td><%# DataBinder.Eval(Container.DataItem, "racks") %></td>      
                    <td><%# DataBinder.Eval(Container.DataItem, "enabled").ToString() == "1" ? "Available" : "Disabled" %></td>
                </tr>
        </ItemTemplate>
    </asp:repeater>
        <tr>
            <td colspan="10"><asp:Label ID="lblZones" runat="server" CssClass="error" Visible="false" Text="<img src='/images/bigAlert.gif' border='0' align='absmiddle'> There are no zones for this room" /></td>
        </tr>
    </table>
    <br /><br />
    <asp:Button ID="btnAddZone" runat="server" CssClass="default" Width="75" Text="Add New" OnClick="btnAddZone_Click" OnClientClick="ProcessButton(this) && LoadWait();" />
</asp:Panel>

<asp:Panel ID="panZone" runat="server" Visible="false">
    <table width="100%" cellpadding="4" cellspacing="3" border="0">
        <tr>
            <td nowrap>Room:</td>
            <td width="100%"><asp:Label ID="lblZoneRoom" runat="server" CssClass="default" /></td>
        </tr>
        <tr>
            <td nowrap>Name:</td>
            <td width="100%"><asp:TextBox ID="txtZoneName" runat="server" CssClass="default" Width="400" MaxLength="100" /></td>
        </tr>
        <tr>
            <td nowrap>Description:</td>
            <td width="100%"><asp:TextBox ID="txtZoneDescription" runat="server" CssClass="default" Width="400" TextMode="MultiLine" Rows="5" /></td>
        </tr>
        <tr>
            <td nowrap>Available:</td>
            <td width="100%"><asp:CheckBox ID="chkZoneAvailable" runat="server" CssClass="default" /></td>
        </tr>
        <tr>
            <td nowrap></td>
            <td width="100%">
                <asp:Button ID="btnZoneAdd" runat="server" CssClass="default" Width="100" Text="Add" OnClick="btnZoneAdd_Click" Visible="false" />
                <asp:Button ID="btnZoneUpdate" runat="server" CssClass="default" Width="100" Text="Update" OnClick="btnZoneUpdate_Click" Visible="false" />
                &nbsp;<asp:Button ID="btnZoneDelete" runat="server" CssClass="default" Width="100" Text="Delete" OnClick="btnZoneDelete_Click" Visible="false" />
                &nbsp;<asp:Button ID="btnZoneCancel" runat="server" CssClass="default" Width="100" Text="Cancel" OnClick="btnZoneCancel_Click" />
            </td>
        </tr>
    </table>
    <br /><br />
    <span class="header">Racks</span>
    <br /><br />
    <table width="100%" cellpadding="4" cellspacing="0" border="0" align="center" style="border:solid 1px #CCCCCC">
        <tr bgcolor="#EEEEEE">
            <td width="85%" nowrap><b>Rack</b></td>
            <td width="15%" nowrap><b>Status</b></td>
        </tr>
    <asp:repeater ID="rptRacks" runat="server">
        <ItemTemplate>
                <tr class='<%# DataBinder.Eval(Container.DataItem, "enabled").ToString() == "1" ? "default" : "component_unavailable" %>' onmouseover="CellRowOver(this);" onmouseout="CellRowOut(this);" onclick="if(event.srcElement.tagName != 'IMG') { LoadWait();window.navigate('<%# FormURL("z=" + Request.QueryString["z"] + "&ra=" + DataBinder.Eval(Container.DataItem, "id")) %>'); }">
                    <td><%# DataBinder.Eval(Container.DataItem, "name") %></td>
                    <td><%# DataBinder.Eval(Container.DataItem, "enabled").ToString() == "1" ? "Available" : "Disabled" %></td>
                </tr>
        </ItemTemplate>
    </asp:repeater>
        <tr>
            <td colspan="10"><asp:Label ID="lblRacks" runat="server" CssClass="error" Visible="false" Text="<img src='/images/bigAlert.gif' border='0' align='absmiddle'> There are no racks for this zone" /></td>
        </tr>
    </table>
    <br /><br />
    <asp:Button ID="btnAddRack" runat="server" CssClass="default" Width="75" Text="Add New" OnClick="btnAddRack_Click" OnClientClick="ProcessButton(this) && LoadWait();" />
</asp:Panel>

<asp:Panel ID="panRack" runat="server" Visible="false">
    <table width="100%" cellpadding="4" cellspacing="3" border="0">
        <tr>
            <td nowrap>Zone:</td>
            <td width="100%"><asp:Label ID="lblRackZone" runat="server" CssClass="default" /></td>
        </tr>
        <tr>
            <td nowrap>Name:</td>
            <td width="100%"><asp:TextBox ID="txtRackName" runat="server" CssClass="default" Width="400" MaxLength="100" /></td>
        </tr>
        <tr>
            <td nowrap>Max U:</td>
            <td width="100%"><asp:TextBox ID="txtRackU" runat="server" CssClass="default" Width="400" MaxLength="100" /></td>
        </tr>
        <tr>
            <td nowrap>Max AMP:</td>
            <td width="100%"><asp:TextBox ID="txtRackAmp" runat="server" CssClass="default" Width="400" MaxLength="100" /></td>
        </tr>
        <tr>
            <td nowrap>Description:</td>
            <td width="100%"><asp:TextBox ID="txtRackDescription" runat="server" CssClass="default" Width="400" TextMode="MultiLine" Rows="5" /></td>
        </tr>
        <tr>
            <td nowrap>Available:</td>
            <td width="100%"><asp:CheckBox ID="chkRackAvailable" runat="server" CssClass="default" /></td>
        </tr>
        <tr>
            <td nowrap></td>
            <td width="100%">
                <asp:Button ID="btnRackAdd" runat="server" CssClass="default" Width="100" Text="Add" OnClick="btnRackAdd_Click" Visible="false" />
                <asp:Button ID="btnRackUpdate" runat="server" CssClass="default" Width="100" Text="Update" OnClick="btnRackUpdate_Click" Visible="false" />
                &nbsp;<asp:Button ID="btnRackDelete" runat="server" CssClass="default" Width="100" Text="Delete" OnClick="btnRackDelete_Click" Visible="false" />
                &nbsp;<asp:Button ID="btnRackCancel" runat="server" CssClass="default" Width="100" Text="Cancel" OnClick="btnRackCancel_Click" />
            </td>
        </tr>
    </table>
</asp:Panel>
