<%@ Page Language="C#" MasterPageFile="~/datapoint.Master" AutoEventWireup="true" CodeBehind="datapoint_people_search.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.datapoint_people_search" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" runat="server">

   <script type="text/javascript">
   
    
    function ValidateSearchControls()
    {
        var boolIsValid=false;
        var strMsg ="";
        var otxtFName = document.getElementById('<%=txtFName.ClientID %>');
        var otxtLName = document.getElementById('<%=txtLName.ClientID %>');
        var otxtLANID = document.getElementById('<%=txtLANID.ClientID %>');
        var oddlStatus = document.getElementById('<%=ddlStatus.ClientID %>');
        var otxtManager = document.getElementById('<%=txtManager.ClientID %>');
        var ohdnManager = document.getElementById('<%=hdnManager.ClientID %>');
        var oddlDepartment = document.getElementById('<%=ddlDepartment.ClientID %>');
        
        
        if (otxtFName != null && trim(otxtFName.value) != "") 
            boolIsValid =true;
        if (otxtLName != null && trim(otxtLName.value) != "") 
            boolIsValid =true; 
        if (otxtLANID != null && trim(otxtLANID.value) != "") 
            boolIsValid =true; 
        if (oddlStatus != null && oddlStatus.selectedIndex != 0)
             boolIsValid =true; 
        if (otxtManager != null && trim(otxtManager.value) != "") 
            boolIsValid =true; 
        else
        {  if (ohdnManager != null)
            ohdnManager.value="";
        }
         if (oddlDepartment !=null && oddlDepartment.selectedIndex != 0)
             boolIsValid =true; 
  
         //Return Message
          if (strMsg=="")
                strMsg="Please enter at least one search criteria.";
                
          if (boolIsValid==false) 
          {     alert(strMsg);
                return false;
          } 
           else
            return true;
                
    }
     function ClearSearchCriteria()
     {
        var otxtFName = document.getElementById('<%=txtFName.ClientID %>');
        var otxtLName = document.getElementById('<%=txtLName.ClientID %>');
        var otxtLANID = document.getElementById('<%=txtLANID.ClientID %>');
        var oddlStatus = document.getElementById('<%=ddlStatus.ClientID %>');
        var otxtManager = document.getElementById('<%=txtManager.ClientID %>');
        var ohdnManager = document.getElementById('<%=hdnManager.ClientID %>');
        var oddlDepartment = document.getElementById('<%=ddlDepartment.ClientID %>');
             
        if (otxtFName != null) otxtFName.value = ""; 
        if (otxtLName != null) otxtLName.value = ""; 
        if (otxtLANID != null) otxtLANID.value = ""; 
        if (oddlStatus != null) oddlStatus.selectedIndex = 0;  
        
        if (otxtManager != null) otxtManager.value = "";
        if (ohdnManager != null) ohdnManager.value = "";
        if (oddlDepartment != null) oddlDepartment.selectedIndex = 0;  
      
       
     }

    </script>


    <asp:Panel ID="pnlAllow" runat="server" Visible="true">
        <table width="100%" cellpadding="0" cellspacing="5" border="0">
            <tr>
                <td rowspan="2">
                    <img src="/images/users40.gif" border="0" align="absmiddle" /></td>
                <td class="header" width="100%" valign="bottom">
                    People Search</td>
            </tr>
            <tr>
                <td width="100%" valign="top">
                    Search on People information.</td>
            </tr>
        </table>
        <table width="100%" cellpadding="4" cellspacing="2" border="0">
            <asp:Panel ID="pnlBasicSearch" runat="server" Visible="false">
                <tr>
                    <td width="20%" align="left">
                        First Name</td>
                    <td width="30%" align="left">
                        <asp:TextBox ID="txtFName" runat="server" Width="250" CssClass="default" /></td>
                    <td width="20%" align="left">
                        Last Name
                    </td>
                    <td width="30%" align="left">
                        <asp:TextBox ID="txtLName" runat="server" Width="250" CssClass="default" /></td>
                </tr>
            </asp:Panel>
            <asp:Panel ID="pnlAdvancedSearch" runat="server" Visible="false">
                <tr>
                    <td width="20%" align="left">
                        LAN ID</td>
                    <td width="30%" align="left">
                        <asp:TextBox ID="txtLANID" runat="server" Width="250" CssClass="default" /></td>
                    <td width="20%" align="left">
                        Status</td>
                    <td width="30%" align="left">
                        <asp:DropDownList ID="ddlStatus" runat="server" Width="250" CssClass="default" /></td>
                </tr>
                <tr>
                    <td width="20%" align="left">
                        Manager</td>
                    <td width="30%" align="left">
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td>
                                    <asp:HiddenField ID="hdnManager" runat="server" />
                                    <asp:TextBox ID="txtManager" runat="server" Width="250" CssClass="default" /></td>
                            </tr>
                            <tr>
                                <td>
                                    <div id="divManager" runat="server" style="overflow: hidden; position: absolute;
                                        display: none; background-color: #FFFFFF; border: solid 1px #CCCCCC">
                                        <asp:ListBox ID="lstManager" runat="server" CssClass="default" />
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                  
                    <td width="20%" align="left">
                        Department</td>
                    <td width="30%" align="left">
                        <asp:DropDownList ID="ddlDepartment" runat="server" Width="250" CssClass="default" />
                    </td>
                  
                </tr>

            </asp:Panel>
            <asp:Panel ID="pnlCommon" runat="server" Visible="true">
                <tr>
                    <td width="20%">
                        Results per Page</td>
                    <td width="30%" align="left">
                        <asp:DropDownList ID="ddlResultsPerPage" runat="server" Width="75" CssClass="default">
                            <asp:ListItem Value="10" Text="10" />
                            <asp:ListItem Value="25" Text="25" Selected="True" />
                            <asp:ListItem Value="50" Text="50" />
                            <asp:ListItem Value="100" Text="100" />
                            <%--<asp:ListItem Value="0" Text="All" />--%>
                        </asp:DropDownList>
                    </td>
                    <td width="20%">
                    </td>
                    <td width="30%">
                    </td>
                </tr>
                <tr>
                    <td width="20%" class="required">
                    </td>
                    <td width="30%" colspan="2">
                        <table cellpadding="3" cellspacing="2" border="0">
                            <tr>
                                <td>
                                    <asp:LinkButton ID="lnkbtnBasicSearch" runat="server" Text="Basic Search" OnClick="lnkbtnBasicSearch_Click"
                                        Visible="false" />
                                    <asp:LinkButton ID="lnkbtnAdvancedSearch" runat="server" Text="Advanced Search" OnClick="lnkbtnAdvancedSearch_Click"
                                        Visible="true" /></td>
                                <td>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                <td>
                                    <asp:LinkButton ID="lnkbtnClearHistory" runat="server" Text="Clear Search" Enabled="true"
                                        OnClick="lnkbtnClearHistory_Click" /></td>
                            </tr>
                        </table>
                    </td>
                    <td width="20%">
                    </td>
                    <td width="30%">
                    </td>
                </tr>
                <tr>
                    <td width="20%" class="required">
                    </td>
                    <td width="30%" colspan="2">
                        <asp:Button ID="btnBasicSearch" Text="Search" Width="100" CssClass="default" runat="server"
                            OnClick="btnBasicSearch_Click" />
                        <asp:Button ID="btnAdvancedSearch" Text="Search" Width="100" CssClass="default" runat="server"
                            OnClick="btnAdvancedSearch_Click" Visible="false" />
                    </td>
                    <td width="20%">
                    </td>
                    <td width="30%">
                    </td>
                </tr>
            </asp:Panel>
        </table>
        <asp:Panel ID="pnlResults" runat="server" Visible="false">
            <table width="95%" cellpadding="4" cellspacing="2" border="0">
                <tr>
                    <td class="default">
                        <asp:Label ID="lblRecords" runat="server" CssClass="bigger" /></td>
                    <td class="default" align="right">
                        <asp:LinkButton ID="btnPrevious" runat="server" Text="Previous Page" OnClick="btnPrevious_Click" />&nbsp;&nbsp;|&nbsp;&nbsp;<asp:LinkButton
                            ID="btnNext" runat="server" Text="Next Page" OnClick="btnNext_Click" />&nbsp;&nbsp;</td>
                </tr>
            </table>
            <asp:DataList ID="dlPeople" runat="server" CellPadding="2" CellSpacing="1" Width="95%"
                OnItemDataBound="dlPeople_ItemDataBound">
                <HeaderTemplate>
                    <tr bgcolor="#EEEEEE">
                        <td align="left" width="10%">
                            <asp:LinkButton ID="lnkbtnHeaderPicture" runat="server" CssClass="tableheader" Text="" />
                        </td>
                        <td align="left" width="10%">
                            <asp:LinkButton ID="lnkbtnHeaderLANId" runat="server" CssClass="tableheader" Text="<b>LAN ID</b>"
                                OnClick="btnOrder_Click" CommandArgument="XID" />
                        </td>
                        <td align="left" width="10%">
                            <asp:LinkButton ID="lnkbtnHeaderFName" runat="server" CssClass="tableheader" Text="<b>First Name</b>"
                                OnClick="btnOrder_Click" CommandArgument="Fname" />
                        </td>
                        <td align="left" width="10%">
                            <asp:LinkButton ID="lnkbtnHeaderLName" runat="server" CssClass="tableheader" Text="<b>Last Name</b>"
                                OnClick="btnOrder_Click" CommandArgument="Lname" />
                        </td>
                        <td align="left" width="10%">
                            <asp:LinkButton ID="lnkbtnHeaderPhone" runat="server" CssClass="tableheader" Text="<b>Phone #</b>" />
                        </td>
                        <td align="left" width="20%">
                            <asp:LinkButton ID="lnkbtnHeaderEmail" runat="server" CssClass="tableheader" Text="<b>Email</b>" />
                        </td>
                        <td align="left" width="10%">
                            <asp:LinkButton ID="lnkbtnHeaderPager" runat="server" CssClass="tableheader" Text="<b>Pager</b>" />
                        </td>
                        <td align="left" width="10%">
                            <asp:LinkButton ID="lnkbtnHeaderDeparment" runat="server" CssClass="tableheader"
                                Text="<b>Department</b>" OnClick="btnOrder_Click" CommandArgument="ApplicationName" />
                        </td>
                        <td align="left" width="20%">
                            <asp:LinkButton ID="lnkbtnHeaderOutOfOffice" runat="server" CssClass="tableheader"
                                Text="<b>Current Office Status</b>" OnClick="btnOrder_Click" CommandArgument="OutOfOffice" />
                        </td>
                    </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td style='border-left: solid 0px "#FF0000"' valign="top" width="10%" rowspan="1">
                            <img id="imgResource" src='' runat="server" align='absmiddle' border='0' style='height: 90px;
                                width: 90px; border-width: 0px; border: solid 1px #999999;' /></td>
                        <td align="left" valign="top" width="10%">
                            <asp:LinkButton ID="lnkLANId" runat="server" CssClass="lookup" />
                        </td>
                        <td align="left" valign="top" width="10%">
                            <asp:Label ID="lblFName" runat="server" CssClass="default" />
                        </td>
                        <td align="left" valign="top" width="10%">
                            <asp:Label ID="lblLName" runat="server" CssClass="default" />
                        </td>
                        <td align="left" valign="top" width="10%">
                            <asp:Label ID="lblPhone" runat="server" CssClass="default" />
                        </td>
                        <td align="left" valign="top" width="20%">
                            <asp:Label ID="lblEmail" runat="server" CssClass="default" />
                        </td>
                        <td align="left" valign="top" width="10%">
                            <asp:Label ID="lblPager" runat="server" CssClass="default" />
                        </td>
                        <td align="left" valign="top" width="10%">
                            <asp:Label ID="lblDepartment" runat="server" CssClass="default" />
                        </td>
                        <td align="left" valign="top" width="20%">
                            <asp:Label ID="lblOutOfOffice" runat="server" CssClass="default" />
                        </td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr bgcolor="#F6F6F6">
                        <td style='border-left: solid 0px "#FF0000"' valign="top" width="10%" rowspan="1">
                            <img id="imgResource" src='' runat="server" align='absmiddle' border='0' style='height: 90px;
                                width: 90px; border-width: 0px; border: solid 1px #999999;' /></td>
                        <td align="left" valign="top" width="10%">
                            <asp:LinkButton ID="lnkLANId" runat="server" CssClass="lookup" />
                        </td>
                        <td align="left" valign="top" width="10%">
                            <asp:Label ID="lblFName" runat="server" CssClass="default" />
                        </td>
                        <td align="left" valign="top" width="10%">
                            <asp:Label ID="lblLName" runat="server" CssClass="default" />
                        </td>
                        <td align="left" valign="top" width="10%">
                            <asp:Label ID="lblPhone" runat="server" CssClass="default" />
                        </td>
                        <td align="left" valign="top" width="20%">
                            <asp:Label ID="lblEmail" runat="server" CssClass="default" />
                        </td>
                        <td align="left" valign="top" width="10%">
                            <asp:Label ID="lblPager" runat="server" CssClass="default" />
                        </td>
                        <td align="left" valign="top" width="10%">
                            <asp:Label ID="lblDepartment" runat="server" CssClass="default" />
                        </td>
                        <td align="left" valign="top" width="20%">
                            <asp:Label ID="lblOutOfOffice" runat="server" CssClass="default" />
                        </td>
                    </tr>
                </AlternatingItemTemplate>
            </asp:DataList>
            <table width="95%" cellpadding="4" cellspacing="2" border="0">
                <tr>
                    <td class="default">
                        <hr />
                    </td>
                </tr>
            </table>
            &nbsp;<br />
        </asp:Panel>
    </asp:Panel>
    <asp:Panel ID="pnlDenied" runat="server" Visible="false">
        <br />
        <table width="100%" cellpadding="0" cellspacing="5" border="0">
            <tr>
                <td rowspan="2">
                    <img src="/images/ico_error.gif" border="0" align="absmiddle" id="IMG1" onclick="return IMG1_onclick()" /></td>
                <td class="header" width="100%" valign="bottom">
                    Access Denied</td>
            </tr>
            <tr>
                <td width="100%" valign="top">
                    You do not have sufficient permission to view this page.</td>
            </tr>
            <tr>
                <td colspan="2">
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                </td>
                <td width="100%">
                    If you think you should have rights to view it, please contact your ClearView administrator.</td>
            </tr>
        </table>
        <p>
            &nbsp;</p>
    </asp:Panel>
    <asp:HiddenField ID="hdnOrder" Value="0" runat="server" />
    <asp:HiddenField ID="hdnOrderBy" Value="" runat="server" />
    <asp:HiddenField ID="hdnPageNo" Value="1" runat="server" />
    <asp:HiddenField ID="hdnRecsPerPage" Value="0" runat="server" />
</asp:Content>
