<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="IM_server_supply_demand.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.IM_server_supply_demand" %>


<script type="text/javascript">


    var old_onload = window.onload;
    window.onload = new function() 
    {
        addDOMLoadEvent(function() 
        {
            NewLoadEventIMSupplyAndDemand();
        });
    }; 

    function NewLoadEventIMSupplyAndDemand() 
    {
        CheckWarningAndCritical();
    }
    function CheckWarningAndCritical()
    {
        
        var chkWarningAndCritial = document.getElementById('<%=chkWarningAndCritial.ClientID%>');

        var ddlGroup1 = document.getElementById('<%=ddlGroup1.ClientID%>');
        var ddlGroup2 = document.getElementById('<%=ddlGroup2.ClientID%>');
        var ddlGroup3 = document.getElementById('<%=ddlGroup3.ClientID%>');
        var ddlGroup4 = document.getElementById('<%=ddlGroup4.ClientID%>');

        var lstLocations = document.getElementById('<%=lstLocations.ClientID%>');
        var lstClasses = document.getElementById('<%=lstClasses.ClientID%>');
        var lstEnvironments = document.getElementById('<%=lstEnvironments.ClientID%>');
        var lstModelTypes = document.getElementById('<%=lstModelTypes.ClientID%>');

        if(lstLocations != null) lstLocations.disabled=false;
        if(lstClasses != null) lstClasses.disabled=false;
        if(lstEnvironments != null) lstEnvironments.disabled=false;
        if(lstModelTypes != null) lstModelTypes.disabled=false;
        if(ddlGroup1 != null) ddlGroup1.disabled=false;
        if(ddlGroup2 != null) ddlGroup2.disabled=false;
        if(ddlGroup3 != null) ddlGroup3.disabled=false;
        if(ddlGroup4 != null) ddlGroup4.disabled=false;
      
        if (chkWarningAndCritial != null)
        {
                if(chkWarningAndCritial.checked==true)
                {
                    if(lstLocations != null)lstLocations.disabled=true;
                    if(lstClasses != null) lstClasses.disabled=true;
                    if(lstEnvironments != null) lstEnvironments.disabled=true;
                    if(lstModelTypes != null) lstModelTypes.disabled=true;
                if(ddlGroup1!=null)
                {
                    for(i=0;i<ddlGroup1.length;i++)
                    {  
                        if (ddlGroup1.options[i].value=="ClassId")
                        {
                            ddlGroup1.options[i].selected=true;
                            ddlGroup1.disabled=true;
                        }
                    }
                }

                if(ddlGroup2!=null)
                {
                    ddlGroup2.options[0].selected=true
                    ddlGroup2.disabled=true;
                }
                if(ddlGroup3!=null)
                {
                    ddlGroup3.options[0].selected=true
                    ddlGroup3.disabled=true;
                }
                if(ddlGroup4!=null)
                {
                    ddlGroup4.options[0].selected=true
                    ddlGroup4.disabled=true;
                }
                }
        }
    }
    
</script>
<table width="100%" cellpadding="0" cellspacing="2" border="0">
    <tr>
        <td id="tdSideBar" valign="top" nowrap style="background-color:#F6F6F6">
            <table width="100%" cellpadding="3" cellspacing="2" border="0">
                <tr>
                    <td colspan="3">
                        <asp:Panel ID="panParameters" runat="server" Visible="false">
                            <table width="100%" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #404040" bgcolor="#FFFFFF">
                                <tr>
                                    <td nowrap><img src="/images/funnel.gif" border="0" align="absmiddle" /></td>
                                    <td width="100%" class="header">Applied Filters</td>
                                </tr>
                                <tr>
                                    <td colspan="2"><%--<%=strParameters %>--%></td>
                                </tr>
                            </table>
                            <br />
                            
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td nowrap><img src="/images/arrow_black_right.gif" border="0" /></td>
                    <td class="bigger" nowrap><b>Filters</b></td>
                    <td align="right"><asp:Button ID="btnGo2" runat="server" CssClass="default" Width="75" Text="Apply" OnClick="btnGo_Click" /></td>
                </tr>
                 <tr>
                    <td>&nbsp;</td>
                    <td valign="top"><b>Warning And Critical:</b></td>
                    <td><asp:CheckBox ID="chkWarningAndCritial" runat="server" onclick="CheckWarningAndCritical();"/></td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td valign="top">Location:<br /><br /><asp:Button id="btnLocations" runat="server" Text="&lt; &gt;" CssClass="default" ToolTip="Maximize Listing" /> <asp:Button id="btnLocationsClear" runat="server" Text="X" Width="25" CssClass="default" ToolTip="Clear Selection(s)" /></td>
                    <td><asp:ListBox ID="lstLocations" runat="server" CssClass="smalldefault" Width="200" SelectionMode="Multiple" /> </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td valign="top">Class:<br /><br /><asp:Button id="btnClasses" runat="server" Text="&lt; &gt;" CssClass="default" ToolTip="Maximize Listing" /> <asp:Button id="btnClassesClear" runat="server" Text="X" Width="25" CssClass="default" ToolTip="Clear Selection(s)" /></td>
                    <td><asp:ListBox ID="lstClasses" runat="server" CssClass="smalldefault" Width="200" SelectionMode="Multiple" /> </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td valign="top">Environment:<br /><br /><asp:Button id="btnEnvironments" runat="server" Text="&lt; &gt;" CssClass="default" ToolTip="Maximize Listing" /> <asp:Button id="btnEnvironmentsClear" runat="server" Text="X" Width="25" CssClass="default" ToolTip="Clear Selection(s)" /></td>
                    <td><asp:ListBox ID="lstEnvironments" runat="server" CssClass="smalldefault" Width="200" SelectionMode="Multiple" /> </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td valign="top">Model Type:<br /><br /><asp:Button id="btnModelTypes" runat="server" Text="&lt; &gt;" CssClass="default" ToolTip="Maximize Listing" /> <asp:Button id="btnModelTypeClear" runat="server" Text="X" Width="25" CssClass="default" ToolTip="Clear Selection(s)" /></td>
                    <td><asp:ListBox ID="lstModelTypes" runat="server" CssClass="smalldefault" Width="200" SelectionMode="Multiple" /> </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>Group 1:</td>
                    <td><asp:DropDownList ID="ddlGroup1" runat="server" CssClass="smalldefault" Width="200" /> </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>Group 2:</td>
                    <td><asp:DropDownList ID="ddlGroup2" runat="server" CssClass="smalldefault" Width="200" /> </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>Group 3:</td>
                    <td><asp:DropDownList ID="ddlGroup3" runat="server" CssClass="smalldefault" Width="200" /> </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>Group 4:</td>
                    <td><asp:DropDownList ID="ddlGroup4" runat="server" CssClass="smalldefault" Width="200" /> </td>
                </tr>
               <tr>
                    <td colspan="3"><img src="/images/spacer.gif" border="0" width="1" height="1" /></td>
                </tr>
                <tr>
                    <td colspan="3" align="right"><asp:Button ID="btnGo1" runat="server" CssClass="default" Width="75" Text="Apply" OnClick="btnGo_Click" /></td>
                </tr>
               
                <tr>
                    <td colspan="3"><img src="/images/spacer.gif" border="0" width="1" height="1" /></td>
                </tr>
            </table>
        </td>
        <td valign="top" style="background-color:#C6C6C6;border-right:1px solid #999999;width:6px;padding-top:350px;">
            <a href="javascript:void(0)" onclick="SideBar(this);"><img src="/images/sidebar_collapse.gif" border="0" alt="Collapse Sidebar"></a>
        </td>
        <td valign="top" width="100%" height="100%">
            <table width="100%" cellpadding="3" cellspacing="2" border="0">
                <tr>
                    <td><%=strSupplyAndDemand %></td>
                </tr>
            </table>
        </td>
    </tr>
</table>
<asp:DataList ID="dlDetails" runat="server" OnItemDataBound="dlDetails_ItemDataBound" Width="100%">  
        <HeaderTemplate>
            <tr bgcolor="#EEEEEE" valign="top" class="default" >
                <td align="left" width="50%">
                    <asp:Label ID="lblHeaderModel" runat="server" Text="<b>Model</b>"  />
                </td>
                <td align="left" width="20%" >
                    <asp:Label ID="lblHeaderSupply" runat="server" Text="<b>Supply</b>"  />
                </td>
                 <td align="left" width="5%" >
                    <asp:Label ID="lblHeaderSupplyNo" runat="server" Text="<b></b>"  />
                </td>
                 <td align="left" width="20%" >
                    <asp:Label ID="lblHeaderDemand" runat="server" Text="<b>Demand</b>"  />
                </td>
                <td align="left" width="5%" >
                    <asp:Label ID="lblHeaderDemandNo" runat="server" Text="<b></b>"  />
                </td>
            </tr>
        </HeaderTemplate>
                            
           <ItemTemplate>  
                <tr id="Tr1" class="default" valign="top" align="left" runat="server" >
                    <td>
                        <asp:Label ID="lblInfo" runat="server"/>
                        <asp:Label ID="lblModel" runat="server"/>
                    </td>
                    <td><asp:Label ID="lblSupply" runat="server"/></td>
                    <td><asp:Label ID="lblSupplyNo" runat="server"/></td>
                    <td><asp:Label ID="lblDemand" runat="server"/> </td>
                    <td><asp:Label ID="lblDemandNo" runat="server"/></td>
                    
                     
                </tr>
            </ItemTemplate>  
           <AlternatingItemTemplate>
                <tr id="Tr2" bgcolor="#F6F6F6" class="default" valign="top" align="left" runat="server">
                     <td>
                        <asp:Label ID="lblInfo" runat="server"/>
                        <asp:Label ID="lblModel" runat="server"/>
                    </td>
                    <td><asp:Label ID="lblSupply" runat="server"/></td>
                    <td><asp:Label ID="lblSupplyNo" runat="server"/></td>
                    <td><asp:Label ID="lblDemand" runat="server"/> </td>
                    <td><asp:Label ID="lblDemandNo" runat="server"/></td>
                </tr>
           </AlternatingItemTemplate>
     </asp:DataList>  
<asp:HiddenField ID="hdnEnvironment" runat="server" />
