<%@ Page Language="C#" MasterPageFile="~/datapoint.Master" AutoEventWireup="true" CodeBehind="datapoint_project_search.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.datapoint_project_search" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" runat="server">


    <script type="text/javascript">
   
    function ValidateBasicSearchControls()
    {
        var boolIsValid=false;
        var strMsg ="";
        var otxtProjectNumber = document.getElementById('<%=txtProjectNumber.ClientID %>');
        var otxtProjectName = document.getElementById('<%=txtProjectName.ClientID %>');
        var oddlStatus = document.getElementById('<%=ddlStatus.ClientID %>');
        if (otxtProjectNumber != null && trim(otxtProjectNumber.value) != "") 
            boolIsValid =true;
        if (otxtProjectName != null && trim(otxtProjectName.value) != "") 
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
    function ValidateAdvanceSearchControls()
    {
        var boolIsValid=false;
        var strMsg ="";
        var otxtProjectNumber = document.getElementById('<%=txtProjectNumber.ClientID %>');
        var otxtProjectName = document.getElementById('<%=txtProjectName.ClientID %>');
        var oddlStatus = document.getElementById('<%=ddlStatus.ClientID %>');
        var otxtProjectManager = document.getElementById('<%=txtProjectManager.ClientID %>');
        var ohdnProjectManager = document.getElementById('<%=hdnProjectManager.ClientID %>');
        var oddlOrganization = document.getElementById('<%=ddlOrganization.ClientID %>');
        var otxtCreatedAfter = document.getElementById('<%=txtCreatedAfter.ClientID %>');
        var otxtCreatedBefore = document.getElementById('<%=txtCreatedBefore.ClientID %>');
        var otxtModifiedAfter = document.getElementById('<%=txtModifiedAfter.ClientID %>');
        var otxtModifiedBefore = document.getElementById('<%=txtModifiedBefore.ClientID %>');
        var otxtCompletedAfter = document.getElementById('<%=txtCompletedAfter.ClientID %>');
        var otxtCompletedBefore = document.getElementById('<%=txtCompletedBefore.ClientID %>');
        
        if (otxtProjectNumber != null && trim(otxtProjectNumber.value) != "") 
            boolIsValid =true;
        if (otxtProjectName != null && trim(otxtProjectName.value) != "") 
            boolIsValid =true; 
        if (oddlStatus.selectedIndex != 0)
             boolIsValid =true; 
        if (otxtProjectManager != null && trim(otxtProjectManager.value) != "") 
            boolIsValid =true; 
        else
            ohdnProjectManager.value="";
         if (oddlOrganization.selectedIndex != 0)
             boolIsValid =true; 
            
        if (otxtCreatedAfter != null && trim(otxtCreatedAfter.value) != "")
            if (isDate(otxtCreatedAfter.value)==true)
                boolIsValid =true; 
            else
            {   SetFocus(otxtCreatedAfter);
                return false;
            }
          
        if (otxtCreatedBefore != null && trim(otxtCreatedBefore.value) != "")
            if (isDate(otxtCreatedBefore.value)==true)
                boolIsValid =true; 
            else
            {   SetFocus(otxtCreatedBefore);
                return false;
            }
        if (otxtModifiedAfter != null && trim(otxtModifiedAfter.value) != "")
            if (isDate(otxtModifiedAfter.value)==true)
                boolIsValid =true; 
            else
            {   SetFocus(otxtModifiedAfter);
                return false;
            } 
        if (otxtModifiedBefore != null && trim(otxtModifiedBefore.value) != "")
            if (isDate(otxtModifiedBefore.value)==true)
                boolIsValid =true; 
            else
            {   SetFocus(otxtModifiedBefore);
                return false;
            } 
          
        if (otxtCompletedAfter != null && trim(otxtCompletedAfter.value) != "")
            if (isDate(otxtCompletedAfter.value)==true)
                boolIsValid =true; 
            else
            {   SetFocus(otxtCompletedAfter);
                return false;
            } 
            
        if (otxtCompletedBefore != null && trim(otxtCompletedBefore.value) != "")
            if (isDate(otxtCompletedAfter.value)==true)
                boolIsValid =true; 
            else
            {   SetFocus(otxtCompletedBefore);
                return false;
            } 
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
        var otxtProjectNumber = document.getElementById('<%=txtProjectNumber.ClientID %>');
        var otxtProjectName = document.getElementById('<%=txtProjectName.ClientID %>');
        var oddlStatus = document.getElementById('<%=ddlStatus.ClientID %>');
        var otxtProjectManager = document.getElementById('<%=txtProjectManager.ClientID %>');
        var ohdnProjectManager = document.getElementById('<%=hdnProjectManager.ClientID %>');
        var oddlOrganization = document.getElementById('<%=ddlOrganization.ClientID %>');
        var otxtCreatedAfter = document.getElementById('<%=txtCreatedAfter.ClientID %>');
        var otxtCreatedBefore = document.getElementById('<%=txtCreatedBefore.ClientID %>');
        var otxtModifiedAfter = document.getElementById('<%=txtModifiedAfter.ClientID %>');
        var otxtModifiedBefore = document.getElementById('<%=txtModifiedBefore.ClientID %>');
        var otxtCompletedAfter = document.getElementById('<%=txtCompletedAfter.ClientID %>');
        var otxtCompletedBefore = document.getElementById('<%=txtCompletedBefore.ClientID %>');
        
        if (otxtProjectNumber != null) otxtProjectNumber.value = ""; 
        if (otxtProjectName != null) otxtProjectName.value = ""; 
        if (oddlStatus != null) oddlStatus.selectedIndex = 0;  
        if (otxtProjectManager != null) otxtProjectManager.value = "";
        if (ohdnProjectManager != null) ohdnProjectManager.value = "";
        if (oddlOrganization != null) oddlOrganization.selectedIndex = 0;  
        if (otxtCreatedAfter != null) otxtCreatedAfter.value = "";
        if (otxtCreatedBefore != null) otxtCreatedBefore.value = "";
        if (otxtModifiedAfter != null) otxtModifiedAfter.value = "";
        if (otxtModifiedBefore != null) otxtModifiedBefore.value = "";
        if (otxtCompletedAfter != null) otxtCompletedAfter.value = "";
        if (otxtCompletedBefore != null) otxtCompletedBefore.value = "";
       
     }

    </script>

    <asp:Panel ID="pnlAllow" runat="server" Visible="false">
        <table width="100%" cellpadding="0" cellspacing="5" border="0">
            <tr>
                <td rowspan="2">
                    <img src="/images/project.gif" border="0" align="absmiddle" /></td>
                <td class="header" width="100%" valign="bottom">
                    Project Search</td>
            </tr>
            <tr>
                <td width="100%" valign="top">
                    Search on project information.</td>
            </tr>
        </table>
        <table width="100%" cellpadding="4" cellspacing="2" border="0">
            <asp:Panel ID="pnlBasicSearch" runat="server" Visible="false">
                <tr>
                    <td width="20%" align="left">
                        Project Id</td>
                    <td width="30%" align="left">
                        <asp:TextBox ID="txtProjectNumber" runat="server" Width="250" CssClass="default" /></td>
                    <td width="20%" align="left">
                        Project Name</td>
                    <td width="30%" align="left">
                        <asp:TextBox ID="txtProjectName" runat="server" Width="250" CssClass="default" /></td>
                </tr>
            </asp:Panel>
            <asp:Panel ID="pnlAdvancedSearch" runat="server" Visible="false">
                <tr>
                    <td width="20%" align="left">
                        Status</td>
                    <td width="30%" align="left">
                        <asp:DropDownList ID="ddlStatus" runat="server" Width="250" CssClass="default" /></td>
                    <td width="20%" align="left">
                    </td>
                    <td width="30%" align="left">
                    </td>
                </tr>
                <tr>
                    <td width="20%" align="left">
                        Project Manager</td>
                    <td width="30%" align="left">
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td>
                                    <asp:HiddenField ID="hdnProjectManager" runat="server" />
                                    <asp:TextBox ID="txtProjectManager" runat="server" Width="250" CssClass="default" /></td>
                            </tr>
                            <tr>
                                <td>
                                    <div id="divProjectManager" runat="server" style="overflow: hidden; position: absolute;
                                        display: none; background-color: #FFFFFF; border: solid 1px #CCCCCC">
                                        <asp:ListBox ID="lstProjectManager" runat="server" CssClass="default" />
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td width="20%" align="left">
                        Sponsoring Organization</td>
                    <td width="30%" align="left">
                        <asp:DropDownList ID="ddlOrganization" runat="server" Width="250" CssClass="default" />
                    </td>
                </tr>
                <tr>
                    <td width="20%" align="left">
                        Created After:</td>
                    <td width="30%" align="left">
                        <asp:TextBox ID="txtCreatedAfter" runat="server" Width="100" CssClass="default" />
                        <asp:ImageButton ID="imgOpenedAfterDate" runat="server" CssClass="default" ImageUrl="/images/calendar.gif"
                            ImageAlign="AbsMiddle" />
                    </td>
                    <td width="20%" align="left">
                        Created Before:</td>
                    <td width="30%" align="left">
                        <asp:TextBox ID="txtCreatedBefore" runat="server" Width="100" CssClass="default" />
                        <asp:ImageButton ID="imgOpenedBeforeDate" runat="server" CssClass="default" ImageUrl="/images/calendar.gif"
                            ImageAlign="AbsMiddle" />
                    </td>
                </tr>
                <tr>
                    <td width="20%" align="left">
                        Modified After:</td>
                    <td width="30%" align="left">
                        <asp:TextBox ID="txtModifiedAfter" runat="server" Width="100" CssClass="default" />
                        <asp:ImageButton ID="imgUpdatedAfterDate" runat="server" CssClass="default" ImageUrl="/images/calendar.gif"
                            ImageAlign="AbsMiddle" />
                    </td>
                    <td width="20%" align="left">
                        Modified Before:</td>
                    <td width="30%" align="left">
                        <asp:TextBox ID="txtModifiedBefore" runat="server" Width="100" CssClass="default" />
                        <asp:ImageButton ID="imgUpdatedBeforeDate" runat="server" CssClass="default" ImageUrl="/images/calendar.gif"
                            ImageAlign="AbsMiddle" />
                    </td>
                </tr>
                <tr>
                    <td width="20%" align="left">
                        Completed After:</td>
                    <td width="30%" align="left">
                        <asp:TextBox ID="txtCompletedAfter" runat="server" Width="100" CssClass="default" />
                        <asp:ImageButton ID="imgClosedAfterDate" runat="server" CssClass="default" ImageUrl="/images/calendar.gif"
                            ImageAlign="AbsMiddle" />
                    </td>
                    <td width="20%" align="left">
                        Completed Before:</td>
                    <td width="30%" align="left">
                        <asp:TextBox ID="txtCompletedBefore" runat="server" Width="100" CssClass="default" />
                        <asp:ImageButton ID="imgClosedBeforeDate" runat="server" CssClass="default" ImageUrl="/images/calendar.gif"
                            ImageAlign="AbsMiddle" />
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
            <asp:DataList ID="dlProjects" runat="server" CellPadding="2" CellSpacing="1" Width="95%"
                OnItemDataBound="dlProjects_ItemDataBound">
                <HeaderTemplate>
                    <tr bgcolor="#EEEEEE">
                        <td align="left" width="10%">
                            <asp:LinkButton ID="lnkbtnHeaderProjectNo" runat="server" CssClass="tableheader"
                                Text="<b>Project #</b>" OnClick="btnOrder_Click" CommandArgument="ProjectNumber" />
                        </td>
                        <td align="left" width="30%">
                            <asp:LinkButton ID="lnkbtnHeaderProjectName" runat="server" CssClass="tableheader"
                                Text="<b>Project Name</b>" OnClick="btnOrder_Click" CommandArgument="ProjectName" />
                        </td>
                         <td align="left" width="20%">
                            <asp:LinkButton ID="lnkbtnSponsoringOrg" runat="server" CssClass="tableheader"
                                Text="<b>Sponsoring Organization</b>" OnClick="btnOrder_Click" CommandArgument="ProjectOrgName" />
                        </td>
                        <td align="left" width="10%">
                            <asp:LinkButton ID="lnkbtnHeaderProjectType" runat="server" CssClass="tableheader"
                                Text="<b>Project Type</b>" OnClick="btnOrder_Click" CommandArgument="ProjectType" />
                        </td>
                        <td align="left" width="20%">
                            <asp:LinkButton ID="lnkbtnHeaderProjectManager" runat="server" CssClass="tableheader"
                                Text="<b>Project Manager</b>" OnClick="btnOrder_Click" CommandArgument="ProjectManager" />
                        </td>
                        <td align="left" width="10%">
                            <asp:LinkButton ID="lnkbtnHeaderProjectStatus" runat="server" CssClass="tableheader"
                                Text="<b>Status</b>" OnClick="btnOrder_Click" CommandArgument="Project Status" />
                        </td>
                    </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td align="left" width="10%">
                            <asp:Label ID="lblProjectNo" runat="server" CssClass="default" />
                        </td>
                        <td align="left" width="30%">
                            <asp:LinkButton ID="lnkProjectName" runat="server" CssClass="lookup" />
                        </td>
                        <td align="left" width="20%">
                            <asp:Label ID="lblSponsoringOrg" runat="server" CssClass="default" />
                        </td>
                        <td align="left" width="10%">
                            <asp:Label ID="lblProjectType" runat="server" CssClass="default" />
                        </td>
                        <td align="left" width="20%">
                            <asp:LinkButton ID="lnkProjectManager" runat="server" CssClass="lookup" />
                        </td>
                        <td align="left" width="10%">
                            <asp:Label ID="lblProjectStatus" runat="server" CssClass="default" />
                        </td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr bgcolor="#F6F6F6">
                        <td align="left" width="10%">
                            <asp:Label ID="lblProjectNo" runat="server" CssClass="default" />
                        </td>
                        <td align="left" width="30%">
                            <asp:LinkButton ID="lnkProjectName" runat="server" CssClass="lookup" />
                        </td>
                        <td align="left" width="20%">
                              <asp:Label ID="lblSponsoringOrg" runat="server" CssClass="default" />
                        </td>
                        <td align="left" width="10%">
                            <asp:Label ID="lblProjectType" runat="server" CssClass="default" />
                        </td>
                        <td align="left" width="20%">
                            <asp:LinkButton ID="lnkProjectManager" runat="server" CssClass="lookup" />
                        </td>
                        <td align="left" width="10%">
                            <asp:Label ID="lblProjectStatus" runat="server" CssClass="default" />
                        </td>
                    </tr>
                </AlternatingItemTemplate>
            </asp:DataList>
             <table width="95%" cellpadding="4" cellspacing="2" border="0">
                <tr><td class="default"><hr /></td></tr>
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
<asp:HiddenField ID="hdnOrderBy" value ="" runat="server" />
<asp:HiddenField ID="hdnPageNo" Value="1" runat="server" />
<asp:HiddenField ID="hdnRecsPerPage" Value="0" runat="server" />
</asp:Content>