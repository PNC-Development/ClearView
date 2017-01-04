<%@ Page Language="C#" MasterPageFile="~/frame.Master" AutoEventWireup="true" CodeBehind="location_room_rack.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.location_room_rack" Title="Location Details" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" runat="server">
<script type="text/javascript">

//var oParentWindow = window.parent;


function UpdateLocationRoomRack() {

    var oParentWindow=null;
    oParentWindow=window.opener;
    if (oParentWindow==null)
            oParentWindow=window.parent;

    //alert(oParentWindow);
     var selectionType= '<%=Request.QueryString["type"] %>';
     selectionType=selectionType.toLowerCase();
     var trState= document.getElementById('<%=trState.ClientID%>');
     var trCity = document.getElementById('<%=trCity.ClientID%>');
     var trLocation = document.getElementById('<%=trLocation.ClientID%>');
     var trRoom = document.getElementById('<%=trRoom.ClientID%>');
     var trZone = document.getElementById('<%=trZone.ClientID%>');
     var trRack = document.getElementById('<%=trRack.ClientID%>');
     
     var chkCommonLocation= document.getElementById('<%=chkCommonLocation.ClientID%>');
     var ddlState = document.getElementById('<%=ddlState.ClientID%>');
     var ddlCity = document.getElementById('<%=ddlCity.ClientID%>');
     var ddlLocation = document.getElementById('<%=ddlLocation.ClientID%>');
     var ddlRoom = document.getElementById('<%=ddlRoom.ClientID%>');
     var ddlZone = document.getElementById('<%=ddlZone.ClientID%>');
     var ddlRack = document.getElementById('<%=ddlRack.ClientID%>');
    
    
    //Parent Selection Id Control(Location Or RoomId Or RackId)    
     var oParentCtrlHdnId = '<%=lblParentCtrlHdnId.Text %>';
     var oParentCtrlHdnId =oParentWindow.document.getElementById(oParentCtrlHdnId)
    if (selectionType=="location")
    {
        var strLocation=ddlLocation.options[ddlLocation.selectedIndex].text +" (" +
                        ddlCity.options[ddlCity.selectedIndex].text +", " +
                        ddlState.options[ddlState.selectedIndex].text +" )";
        
        var oParentCtrlLocation = '<%=lblParentCtrlLocation.Text %>';
        var oParentCtrlLocation =oParentWindow.document.getElementById(oParentCtrlLocation)

        if (oParentCtrlLocation!=null) 
        {   if (oParentCtrlLocation.type == "text")
                oParentCtrlLocation.value=strLocation;
            else
                oParentCtrlLocation.innerHTML=strLocation;
        }
        
        if (oParentCtrlHdnId!=null) 
            oParentCtrlHdnId.value=ddlLocation.options[ddlLocation.selectedIndex].value;


    }

    else if (selectionType=="room")
    { 
        var strLocation=ddlLocation.options[ddlLocation.selectedIndex].text +" (" +
                        ddlCity.options[ddlCity.selectedIndex].text +", " +
                        ddlState.options[ddlState.selectedIndex].text +" )";
        var strRoom=ddlRoom.options[ddlRoom.selectedIndex].text;
        
        var oParentCtrlLocation = '<%=lblParentCtrlLocation.Text %>';
        var oParentCtrlLocation =oParentWindow.document.getElementById(oParentCtrlLocation)
        if (oParentCtrlLocation!=null) 
        {   if (oParentCtrlLocation.type == "text")
                oParentCtrlLocation.value=strLocation;
            else
                oParentCtrlLocation.innerHTML=strLocation;
        }
        
        var oParentCtrlRoom = '<%=lblParentCtrlRoom.Text %>';
        var oParentCtrlRoom =oParentWindow.document.getElementById(oParentCtrlRoom)
        if (oParentCtrlRoom!=null) 
        {   if (oParentCtrlRoom.type == "text")
                oParentCtrlRoom.value=strRoom;
            else
                oParentCtrlRoom.innerHTML=strRoom;
        }
        
        if (oParentCtrlHdnId!=null) 
            oParentCtrlHdnId.value=ddlRoom.options[ddlRoom.selectedIndex].value;
    }
    else if (selectionType=="zone")
    {
        var strLocation=ddlLocation.options[ddlLocation.selectedIndex].text +" (" +
                        ddlCity.options[ddlCity.selectedIndex].text +", " +
                        ddlState.options[ddlState.selectedIndex].text +" )";
        var strRoom=ddlRoom.options[ddlRoom.selectedIndex].text;
        var strZone=ddlZone.options[ddlZone.selectedIndex].text;
        
        var oParentCtrlLocation = '<%=lblParentCtrlLocation.Text %>';
        var oParentCtrlLocation =oParentWindow.document.getElementById(oParentCtrlLocation)
        if (oParentCtrlLocation!=null) 
        {   if (oParentCtrlLocation.type == "text")
                oParentCtrlLocation.value=strLocation;
            else
                oParentCtrlLocation.innerHTML=strLocation;
        }

        var oParentCtrlRoom = '<%=lblParentCtrlRoom.Text %>';
        var oParentCtrlRoom =oParentWindow.document.getElementById(oParentCtrlRoom)
        if (oParentCtrlRoom!=null) 
        {   if (oParentCtrlRoom.type == "text")
                oParentCtrlRoom.value=strRoom;
            else
                oParentCtrlRoom.innerHTML=strRoom;
        }
        
        
        var oParentCtrlZone = '<%=lblParentCtrlZone.Text %>';
        var oParentCtrlZone =oParentWindow.document.getElementById(oParentCtrlZone)
        if (oParentCtrlZone!=null) 
        {   if (oParentCtrlZone.type == "text")
                oParentCtrlZone.value=strZone;
            else
                oParentCtrlZone.innerHTML=strZone;
        }

        if (oParentCtrlHdnId!=null) 
            oParentCtrlHdnId.value=ddlZone.options[ddlZone.selectedIndex].value;
      
    }
    else if (selectionType=="rack")
    {
        var strLocation=ddlLocation.options[ddlLocation.selectedIndex].text +" (" +
                        ddlCity.options[ddlCity.selectedIndex].text +", " +
                        ddlState.options[ddlState.selectedIndex].text +" )";
        var strRoom=ddlRoom.options[ddlRoom.selectedIndex].text;
        var strZone=ddlZone.options[ddlZone.selectedIndex].text;
        var strRack=ddlRack.options[ddlRack.selectedIndex].text;

        var oParentCtrlLocation = '<%=lblParentCtrlLocation.Text %>';
        var oParentCtrlLocation =oParentWindow.document.getElementById(oParentCtrlLocation)
        if (oParentCtrlLocation!=null) 
        {
            if (oParentCtrlLocation.type == "text")
                oParentCtrlLocation.value=strLocation;
            else
                oParentCtrlLocation.innerHTML=strLocation;
        }

        var oParentCtrlRoom = '<%=lblParentCtrlRoom.Text %>';
        var oParentCtrlRoom =oParentWindow.document.getElementById(oParentCtrlRoom)
        if (oParentCtrlRoom!=null) 
        {
           if (oParentCtrlRoom.type == "text")
                oParentCtrlRoom.value=strRoom;
           else
                oParentCtrlRoom.innerHTML=strRoom;
        }
        
        var oParentCtrlZone = '<%=lblParentCtrlZone.Text %>';
        var oParentCtrlZone =oParentWindow.document.getElementById(oParentCtrlZone)
        if (oParentCtrlZone!=null) 
        {   if (oParentCtrlZone.type == "text")
                oParentCtrlZone.value=strZone;
            else
                oParentCtrlZone.innerHTML=strZone;
        }
            

        var oParentCtrlRack = '<%=lblParentCtrlRack.Text %>';
        var oParentCtrlRack =oParentWindow.document.getElementById(oParentCtrlRack)
        if (oParentCtrlRack!=null) 
        {   if (oParentCtrlRack.type == "text")
                oParentCtrlRack.value=strRack;
            else
                oParentCtrlRack.innerHTML=strRack;
        }

        if (oParentCtrlHdnId!=null) 
            oParentCtrlHdnId.value=ddlRack.options[ddlRack.selectedIndex].value;

    }
    if (window.opener == null) { parent.HidePanel(); } else { window.close(); }

}
</script>

<asp:Panel ID="pnlLocationRoomRackInfo" runat="server" Visible="true"  Width="100%">
<fieldset>
    <legend class="tableheader"><b>Location Details</b></legend>
<table id="tblLocation" width="100%" cellpadding="0" cellspacing="3" border="0" runat="server">
    <tr id="trCommonLocation" runat="server"> 
        <td colspan="2" width="100%">
            <asp:CheckBox ID="chkCommonLocation" Text="Show Common Locations" AutoPostBack=true CssClass="default" runat="server" OnCheckedChanged="chkCommonLocation_CheckedChanged" />
        </td>
    </tr>
    <tr id="trState" runat="server"> 
        <td nowrap width="30%">State :</td>
        <td width="70%"><asp:dropdownlist ID="ddlState" AutoPostBack=true CssClass="default" runat="server" Width="400" OnSelectedIndexChanged="ddlState_SelectedIndexChanged"/></td>
    </tr>
    <tr id="trCity" runat="server"> 
        <td nowrap width="30%">City :</td>
        <td width="70%"><asp:dropdownlist ID="ddlCity" AutoPostBack=true CssClass="default" runat="server" Width="400" OnSelectedIndexChanged="ddlCity_SelectedIndexChanged"/></td>
    </tr>
    <tr id="trLocation" runat="server"> 
        <td nowrap width="30%">Location :</td>
        <td width="70%"><asp:dropdownlist ID="ddlLocation" AutoPostBack=true CssClass="default" runat="server" Width="400" OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged"/></td>
    </tr>
    <tr id="trRoom" runat="server"> 
        <td nowrap width="30%">Room :</td>
        <td width="70%"><asp:dropdownlist ID="ddlRoom" AutoPostBack=true CssClass="default" runat="server" Width="400" OnSelectedIndexChanged="ddlRoom_SelectedIndexChanged"/></td>
    </tr>
    <tr id="trZone" runat="server"> 
        <td nowrap width="30%">Zone :</td>
        <td width="70%"><asp:dropdownlist ID="ddlZone" AutoPostBack=true CssClass="default" runat="server" Width="400" OnSelectedIndexChanged="ddlZone_SelectedIndexChanged"/></td>
    </tr>
    <tr id="trRack" runat="server"> 
        <td nowrap width="30%">Rack :</td>
        <td width="70%"><asp:dropdownlist ID="ddlRack" AutoPostBack=true CssClass="default" runat="server" Width="400" /></td>
    </tr>
</table>
</fieldset>
<br />

<asp:Label ID="lblParentCtrlHdnId" runat="Server" Visible="false" />
<asp:Label ID="lblParentCtrlLocation" runat="Server" Visible="false" />
<asp:Label ID="lblParentCtrlRoom" runat="Server" Visible="false" />
<asp:Label ID="lblParentCtrlZone" runat="Server" Visible="false" />
<asp:Label ID="lblParentCtrlRack" runat="Server" Visible="false" />
<asp:Button ID="btnSave" runat="server" CssClass="default" Width="100" Text="Select" OnClick="btnSave_Click" /></asp:Panel>


</asp:Content>
