<%@ Page Language="C#" MasterPageFile="~/datapoint.Master" AutoEventWireup="true" CodeBehind="datapoint_service_search.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.datapoint_service_search" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" runat="server">

<script type="text/javascript">
    function ChangeCookieSearch(oList, strCookie) 
    {
        if (event.srcElement.tagName == "INPUT") 
        {
            var oRadio = event.srcElement;
            SetCookie(strCookie, oRadio.value);
        }
    }

    function ValidateBasicSearchControls()
    {
        var boolIsValid=false;
        var strMsg ="";
        var otxtSearchId = document.getElementById('<%=txtSearchId.ClientID %>');
        var otxtSearchName = document.getElementById('<%=txtSearchName.ClientID %>');
        if (otxtSearchId != null && trim(otxtSearchId.value) != "") 
            boolIsValid =true;
        if (otxtSearchName != null && trim(otxtSearchName.value) != "") 
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
    
    function ValidateReqAdvanceSearchControls()
    {
        var boolIsValid=false;
        var strMsg ="";
        var otxtSearchId = document.getElementById('<%=txtSearchId.ClientID %>');
        var otxtSearchName = document.getElementById('<%=txtSearchName.ClientID %>');
        var otxtReqRequestedBy = document.getElementById('<%=txtReqRequestedBy.ClientID %>');
        var ohdnReqRequestBy = document.getElementById('<%=hdnReqRequestBy.ClientID %>');
        var oddlReqStatus = document.getElementById('<%=ddlReqStatus.ClientID %>');
        
        var otxtReqAssignedTo = document.getElementById('<%=txtReqAssignedTo.ClientID %>');
        var ohdnReqAssignedTo = document.getElementById('<%=hdnReqAssignedTo.ClientID %>');
        
        var otxtReqAssignedBy = document.getElementById('<%=txtReqAssignedBy.ClientID %>');
        var ohdnReqAssignedBy = document.getElementById('<%=hdnReqAssignedBy.ClientID %>');
        
        var otxtReqProject = document.getElementById('<%=txtReqProject.ClientID %>');
        var ohdnReqProjectId = document.getElementById('<%=hdnReqProjectId.ClientID %>');
        
        var oddlReqAssingmentGrp = document.getElementById('<%=ddlReqAssingmentGrp.ClientID %>');
        var otxtReqCreatedAfter = document.getElementById('<%=txtReqCreatedAfter.ClientID %>');
        var otxtReqCreatedBefore = document.getElementById('<%=txtReqCreatedBefore.ClientID %>');
        var otxtReqModifiedAfter = document.getElementById('<%=txtReqModifiedAfter.ClientID %>');
        var otxtReqModifiedBefore = document.getElementById('<%=txtReqModifiedBefore.ClientID %>');
        var otxtReqCompletedAfter = document.getElementById('<%=txtReqCompletedAfter.ClientID %>');
        var otxtReqCompletedBefore = document.getElementById('<%=txtReqCompletedBefore.ClientID %>');
        
        if (otxtSearchId != null && trim(otxtSearchId.value) != "") 
            boolIsValid =true;
        if (otxtSearchName != null && trim(otxtSearchName.value) != "") 
            boolIsValid =true; 
            
        if (otxtReqRequestedBy != null && trim(otxtReqRequestedBy.value) != "") 
            boolIsValid =true; 
        else
            ohdnReqRequestBy.value="";
            
        if (oddlReqStatus.selectedIndex != 0)
             boolIsValid =true; 
        
        if (otxtReqAssignedTo != null && trim(otxtReqAssignedTo.value) != "") 
            boolIsValid =true; 
        else
            ohdnReqAssignedTo.value="";
        
        if (otxtReqAssignedBy != null && trim(otxtReqAssignedBy.value) != "") 
            boolIsValid =true; 
        else
            ohdnReqAssignedBy.value="";
                
        if (otxtReqProject != null && trim(otxtReqProject.value) != "") 
            boolIsValid =true; 
        else
            ohdnReqProjectId.value="";
        
        if (oddlReqAssingmentGrp.selectedIndex != 0)
             boolIsValid =true; 
            
        if (otxtReqCreatedAfter != null && trim(otxtReqCreatedAfter.value) != "")
            if (isDate(otxtReqCreatedAfter.value)==true)
                boolIsValid =true; 
            else
            {   SetFocus(otxtReqCreatedAfter);
                return false;
            }
          
        if (otxtReqCreatedBefore != null && trim(otxtReqCreatedBefore.value) != "")
            if (isDate(otxtReqCreatedBefore.value)==true)
                boolIsValid =true; 
            else
            {   SetFocus(otxtReqCreatedBefore);
                return false;
            }
        if (otxtReqModifiedAfter != null && trim(otxtReqModifiedAfter.value) != "")
            if (isDate(otxtReqModifiedAfter.value)==true)
                boolIsValid =true; 
            else
            {   SetFocus(otxtReqModifiedAfter);
                return false;
            } 
        if (otxtReqModifiedBefore != null && trim(otxtReqModifiedBefore.value) != "")
            if (isDate(otxtReqModifiedBefore.value)==true)
                boolIsValid =true; 
            else
            {   SetFocus(otxtReqModifiedBefore);
                return false;
            } 
          
        if (otxtReqCompletedAfter != null && trim(otxtReqCompletedAfter.value) != "")
            if (isDate(otxtReqCompletedAfter.value)==true)
                boolIsValid =true; 
            else
            {   SetFocus(otxtReqCompletedAfter);
                return false;
            } 
            
        if (otxtReqCompletedBefore != null && trim(otxtReqCompletedBefore.value) != "")
            if (isDate(otxtReqCompletedBefore.value)==true)
                boolIsValid =true; 
            else
            {   SetFocus(otxtReqCompletedBefore);
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
    function ValidateDesignAdvanceSearchControls()
    {
        var boolIsValid=false;
        var strMsg ="";
        var otxtSearchId = document.getElementById('<%=txtSearchId.ClientID %>');
        var otxtSearchName = document.getElementById('<%=txtSearchName.ClientID %>');
        var oddlDesignConfidenceLevel = document.getElementById('<%=ddlDesignConfidenceLevel.ClientID %>');
        var oddlDesignStatus = document.getElementById('<%=ddlDesignStatus.ClientID %>');
        var otxtDesignCreatedBy = document.getElementById('<%=txtDesignCreatedBy.ClientID %>');
        var ohdnDesignCreatedBy = document.getElementById('<%=hdnDesignCreatedBy.ClientID %>');
        
        var otxtDesignCreatedAfter = document.getElementById('<%=txtDesignCreatedAfter.ClientID %>');
        var otxtDesignCreatedBefore = document.getElementById('<%=txtDesignCreatedBefore.ClientID %>');
        var otxtDesignModifiedAfter = document.getElementById('<%=txtDesignModifiedAfter.ClientID %>');
        var otxtDesignModifiedBefore = document.getElementById('<%=txtDesignModifiedBefore.ClientID %>');
        var otxtDesignCompletedAfter = document.getElementById('<%=txtDesignCompletedAfter.ClientID %>');
        var otxtDesignCompletedBefore = document.getElementById('<%=txtDesignCompletedBefore.ClientID %>');
        
        if (otxtSearchId != null && trim(otxtSearchId.value) != "") 
            boolIsValid =true;
        if (otxtSearchName != null && trim(otxtSearchName.value) != "") 
            boolIsValid =true; 
        if (oddlDesignConfidenceLevel.selectedIndex != 0)
            boolIsValid =true;    
        if (oddlDesignStatus.selectedIndex != 0)
            boolIsValid =true; 
            
        if (otxtDesignCreatedBy != null && trim(otxtDesignCreatedBy.value) != "") 
            boolIsValid =true; 
        else
            ohdnDesignCreatedBy.value="";
        
        
            
        if (otxtDesignCreatedAfter != null && trim(otxtDesignCreatedAfter.value) != "")
            if (isDate(otxtDesignCreatedAfter.value)==true)
                boolIsValid =true; 
            else
            {   SetFocus(otxtDesignCreatedAfter);
                return false;
            }
          
        if (otxtDesignCreatedBefore != null && trim(otxtDesignCreatedBefore.value) != "")
            if (isDate(otxtDesignCreatedBefore.value)==true)
                boolIsValid =true; 
            else
            {   SetFocus(otxtDesignCreatedBefore);
                return false;
            }
        if (otxtDesignModifiedAfter != null && trim(otxtDesignModifiedAfter.value) != "")
            if (isDate(otxtDesignModifiedAfter.value)==true)
                boolIsValid =true; 
            else
            {   SetFocus(otxtDesignModifiedAfter);
                return false;
            } 
        if (otxtDesignModifiedBefore != null && trim(otxtDesignModifiedBefore.value) != "")
            if (isDate(otxtDesignModifiedBefore.value)==true)
                boolIsValid =true; 
            else
            {   SetFocus(otxtDesignModifiedBefore);
                return false;
            } 
          
        if (otxtDesignCompletedAfter != null && trim(otxtDesignCompletedAfter.value) != "")
            if (isDate(otxtDesignCompletedAfter.value)==true)
                boolIsValid =true; 
            else
            {   SetFocus(otxtDesignCompletedAfter);
                return false;
            } 
            
        if (otxtDesignCompletedBefore != null && trim(otxtDesignCompletedBefore.value) != "")
            if (isDate(otxtDesignCompletedBefore.value)==true)
                boolIsValid =true; 
            else
            {   SetFocus(otxtDesignCompletedBefore);
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
        var otxtSearchId = document.getElementById('<%=txtSearchId.ClientID %>');
        var otxtSearchName = document.getElementById('<%=txtSearchName.ClientID %>');
        var otxtReqRequestedBy = document.getElementById('<%=txtReqRequestedBy.ClientID %>');
        var ohdnReqRequestBy = document.getElementById('<%=hdnReqRequestBy.ClientID %>');
        var oddlReqStatus = document.getElementById('<%=ddlReqStatus.ClientID %>');
        
        var otxtReqAssignedTo = document.getElementById('<%=txtReqAssignedTo.ClientID %>');
        var ohdnReqAssignedTo = document.getElementById('<%=hdnReqAssignedTo.ClientID %>');
        
        var otxtReqAssignedBy = document.getElementById('<%=txtReqAssignedBy.ClientID %>');
        var ohdnReqAssignedBy = document.getElementById('<%=hdnReqAssignedBy.ClientID %>');
        
        var otxtReqProject = document.getElementById('<%=txtReqProject.ClientID %>');
        var ohdnReqProjectId = document.getElementById('<%=hdnReqProjectId.ClientID %>');
        
        var oddlReqAssingmentGrp = document.getElementById('<%=ddlReqAssingmentGrp.ClientID %>');
        var otxtReqCreatedAfter = document.getElementById('<%=txtReqCreatedAfter.ClientID %>');
        var otxtReqCreatedBefore = document.getElementById('<%=txtReqCreatedBefore.ClientID %>');
        var otxtReqModifiedAfter = document.getElementById('<%=txtReqModifiedAfter.ClientID %>');
        var otxtReqModifiedBefore = document.getElementById('<%=txtReqModifiedBefore.ClientID %>');
        var otxtReqCompletedAfter = document.getElementById('<%=txtReqCompletedAfter.ClientID %>');
        var otxtReqCompletedBefore = document.getElementById('<%=txtReqCompletedBefore.ClientID %>');
        
        if (otxtSearchId != null) otxtSearchId.value = "";
        if (otxtSearchName != null) otxtSearchName.value = "";
        if (otxtReqRequestedBy != null) otxtReqRequestedBy.value = "";
        if (ohdnReqRequestBy != null) ohdnReqRequestBy.value = "";
        if (oddlReqStatus != null) oddlReqStatus.selectedIndex = 0;
        
        if (otxtReqAssignedTo != null) otxtReqAssignedTo.value = "";
        if (ohdnReqAssignedTo != null) ohdnReqAssignedTo.value = "";
        
        if (otxtReqAssignedBy != null) otxtReqAssignedBy.value = "";
        if (ohdnReqAssignedBy != null) ohdnReqAssignedBy.value = "";
        
        if (otxtReqProject != null) otxtReqProject.value = "";
        if (ohdnReqProjectId != null) ohdnReqProjectId.value = "";
        
        if (oddlReqAssingmentGrp != null) oddlReqAssingmentGrp.selectedIndex = 0;
        if (otxtReqCreatedAfter != null) otxtReqCreatedAfter.value = "";
        if (otxtReqCreatedBefore != null) otxtReqCreatedBefore.value = "";
        if (otxtReqModifiedAfter != null) otxtReqModifiedAfter.value = "";
        if (otxtReqModifiedBefore != null) otxtReqModifiedBefore.value = "";
        if (otxtReqCompletedAfter != null) otxtReqCompletedAfter.value = "";
        if (otxtReqCompletedBefore != null) otxtReqCompletedBefore.value = "";
     
        var oddlDesignConfidenceLevel = document.getElementById('<%=ddlDesignConfidenceLevel.ClientID %>');
        var oddlDesignStatus = document.getElementById('<%=ddlDesignStatus.ClientID %>');
        var otxtDesignCreatedBy = document.getElementById('<%=txtDesignCreatedBy.ClientID %>');
        var ohdnDesignCreatedBy = document.getElementById('<%=hdnDesignCreatedBy.ClientID %>');
        var otxtDesignCreatedAfter = document.getElementById('<%=txtDesignCreatedAfter.ClientID %>');
        var otxtDesignCreatedBefore = document.getElementById('<%=txtDesignCreatedBefore.ClientID %>');
        var otxtDesignModifiedAfter = document.getElementById('<%=txtDesignModifiedAfter.ClientID %>');
        var otxtDesignModifiedBefore = document.getElementById('<%=txtDesignModifiedBefore.ClientID %>');
        var otxtDesignCompletedAfter = document.getElementById('<%=txtDesignCompletedAfter.ClientID %>');
        var otxtDesignCompletedBefore = document.getElementById('<%=txtDesignCompletedBefore.ClientID %>');
        
       if (oddlDesignConfidenceLevel != null) oddlDesignConfidenceLevel.selectedIndex = 0;
       if (oddlDesignStatus != null) oddlDesignStatus.selectedIndex = 0;
       if (otxtDesignCreatedBy != null) otxtDesignCreatedBy.value = "";
       if (ohdnDesignCreatedBy != null) ohdnDesignCreatedBy.value = "";
       if (otxtDesignCreatedAfter != null) otxtDesignCreatedAfter.value = "";
       if (otxtDesignCreatedBefore != null) otxtDesignCreatedBefore.value = "";
       if (otxtDesignModifiedAfter != null) otxtDesignModifiedAfter.value = "";
       if (otxtDesignModifiedBefore != null) otxtDesignModifiedBefore.value = "";
       if (otxtDesignCompletedAfter != null) otxtDesignCompletedAfter.value = "";
       if (otxtDesignCompletedBefore != null) otxtDesignCompletedBefore.value = "";
       
       
        
     }

</script>

    <asp:Panel ID="pnlAllow" runat="server" Visible="false">
        <table width="100%" cellpadding="4" cellspacing="2" border="0">
            <tr>
                <td rowspan="2">
                    <img src="/images/workload48.gif" border="0" align="absmiddle" /></td>
                <td class="header" width="100%" valign="bottom">
                    Service Search</td>
            </tr>
            <tr>
                <td width="100%" valign="top">
                    Search on service information related to requests.</td>
            </tr>
        </table>
        <table width="100%" cellpadding="4" cellspacing="2" border="0">
            <tr>
                <td width="20%">
                    <asp:Label ID="lblSearchType" runat="server" CssClass="default" Text="Search Type" /></td>
                <td width="30%">
                    <asp:DropDownList ID="ddlSearchType" runat="server" AutoPostBack="True" CssClass="default"
                        OnSelectedIndexChanged="ddlSearchType_SelectedIndexChanged">
                        <asp:ListItem Selected="True" Value="request">Request</asp:ListItem>
                        <asp:ListItem Value="design">Design</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td width="20%" align="left">
                </td>
                <td width="30%" align="left">
                </td>
            </tr>
            <asp:Panel ID="pnlBasicSearch" runat="server" Visible="false">
                <tr>
                    <td width="20%" align="left">
                        <asp:Label ID="lblSearchId" runat="server" CssClass="default" Text="Request Id" /></td>
                    <td width="30%" align="left">
                        <asp:TextBox ID="txtSearchId" runat="server" Width="250" CssClass="default" /></td>
                    <td width="20%" align="left">
                        <asp:Label ID="lblSearchName" runat="server" CssClass="default" Text="Request Name" /></td>
                    <td width="30%" align="left">
                        <asp:TextBox ID="txtSearchName" runat="server" Width="250" CssClass="default" /></td>
                </tr>
            </asp:Panel>
            <asp:Panel ID="pnlReqAdvancedSearch" runat="server" Visible="false">
                <tr>
                    <td width="20%" align="left">
                        <asp:Label ID="lblReqRequestedBy" runat="server" CssClass="default" Text="Requested By" /></td>
                    <td width="30%" align="left">
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtReqRequestedBy" runat="server" Width="250" CssClass="default" /></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:HiddenField ID="hdnReqRequestBy" Value="" runat="server" />
                                    <div id="divReqRequestdBy" runat="server" style="overflow: hidden; position: absolute;
                                        display: none; background-color: #FFFFFF; border: solid 1px #CCCCCC">
                                        <asp:ListBox ID="lstReqRequestedBy" runat="server" CssClass="default" />
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td width="20%" align="left">
                        <asp:Label ID="lblReqStatus" runat="server" CssClass="default" Text="Status" />
                    </td>
                    <td width="30%" align="left">
                        <asp:DropDownList ID="ddlReqStatus" runat="server" Width="250" CssClass="default" />
                    </td>
                </tr>
                <tr>
                    <td width="20%" align="left">
                        <asp:Label ID="lblReqAssignedBy" runat="server" CssClass="default" Text="Assigned By" /></td>
                    <td width="30%" align="left">
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtReqAssignedBy" runat="server" Width="250" CssClass="default" /></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:HiddenField ID="hdnReqAssignedBy" Value="" runat="server" />
                                    <div id="divReqAssignedBy" runat="server" style="overflow: hidden; position: absolute;
                                        display: none; background-color: #FFFFFF; border: solid 1px #CCCCCC">
                                        <asp:ListBox ID="lstReqAssignedBy" runat="server" CssClass="default" />
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td width="20%" align="left">
                        <asp:Label ID="lblReqAssignedTo" runat="server" CssClass="default" Text="Assigned To" /></td>
                    <td width="30%" align="left">
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtReqAssignedTo" runat="server" Width="250" CssClass="default" /></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:HiddenField ID="hdnReqAssignedTo" Value="" runat="server" />
                                    <div id="divReqAssignedTo" runat="server" style="overflow: hidden; position: absolute;
                                        display: none; background-color: #FFFFFF; border: solid 1px #CCCCCC">
                                        <asp:ListBox ID="lstReqAssignedTo" runat="server" CssClass="default" />
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td width="20%" align="left">
                        <asp:Label ID="lblReqProject" runat="server" CssClass="default" Text="Project" /></td>
                    <td width="30%" align="left">
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtReqProject" runat="server" Width="250" CssClass="default" /></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:HiddenField ID="hdnReqProjectId" Value="" runat="server" />
                                    <div id="divReqProject" runat="server" style="overflow: hidden; position: absolute;
                                        display: none; background-color: #FFFFFF; border: solid 1px #CCCCCC">
                                        <asp:ListBox ID="lstReqProject" runat="server" CssClass="default" />
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td width="20%" align="left">
                        <asp:Label ID="lblReqAssignmentGrp" runat="server" CssClass="default" Text="Assignment Group" />
                    </td>
                    <td width="30%" align="left">
                        <asp:DropDownList ID="ddlReqAssingmentGrp" runat="server" Width="250" CssClass="default" />
                    </td>
                </tr>
                <tr>
                    <td width="20%" align="left">
                        <asp:Label ID="lblReqCreatedAfter" runat="server" CssClass="default" Text="Created After" />
                    </td>
                    <td width="30%" align="left">
                        <asp:TextBox ID="txtReqCreatedAfter" runat="server" Width="100" CssClass="default" />
                        <asp:ImageButton ID="imgReqCreatedAfterDate" runat="server" CssClass="default" ImageUrl="/images/calendar.gif"
                            ImageAlign="AbsMiddle" />
                    </td>
                    <td width="20%" align="left">
                        <asp:Label ID="lblReqCreatedBefore" runat="server" CssClass="default" Text="Created Before" />
                    </td>
                    <td width="30%" align="left">
                        <asp:TextBox ID="txtReqCreatedBefore" runat="server" Width="100" CssClass="default" />
                        <asp:ImageButton ID="imgReqCreatedBeforeDate" runat="server" CssClass="default" ImageUrl="/images/calendar.gif"
                            ImageAlign="AbsMiddle" />
                    </td>
                </tr>
                <tr>
                    <td width="20%" align="left">
                        <asp:Label ID="lblReqModifiedAfter" runat="server" CssClass="default" Text="Modified After" />
                    </td>
                    <td width="30%" align="left">
                        <asp:TextBox ID="txtReqModifiedAfter" runat="server" Width="100" CssClass="default" />
                        <asp:ImageButton ID="imgReqModifiedAfterDate" runat="server" CssClass="default" ImageUrl="/images/calendar.gif"
                            ImageAlign="AbsMiddle" />
                    </td>
                    <td width="20%" align="left">
                        <asp:Label ID="lblReqModifiedBefore" runat="server" CssClass="default" Text="Modified Before" />
                    </td>
                    <td width="30%" align="left">
                        <asp:TextBox ID="txtReqModifiedBefore" runat="server" Width="100" CssClass="default" />
                        <asp:ImageButton ID="imgReqModifiedBeforeDate" runat="server" CssClass="default"
                            ImageUrl="/images/calendar.gif" ImageAlign="AbsMiddle" />
                    </td>
                </tr>
                <tr>
                    <td width="20%" align="left">
                        <asp:Label ID="lblReqCompletedAfter" runat="server" CssClass="default" Text="Completed After" />
                    </td>
                    <td width="30%" align="left">
                        <asp:TextBox ID="txtReqCompletedAfter" runat="server" Width="100" CssClass="default" />
                        <asp:ImageButton ID="imgReqCompletedAfterDate" runat="server" CssClass="default"
                            ImageUrl="/images/calendar.gif" ImageAlign="AbsMiddle" />
                    </td>
                    <td width="20%" align="left">
                        <asp:Label ID="lblReqCompletedBefore" runat="server" CssClass="default" Text="Completed Before" />
                    </td>
                    <td width="30%" align="left">
                        <asp:TextBox ID="txtReqCompletedBefore" runat="server" Width="100" CssClass="default" />
                        <asp:ImageButton ID="imgReqCompletedBeforeDate" runat="server" CssClass="default"
                            ImageUrl="/images/calendar.gif" ImageAlign="AbsMiddle" />
                    </td>
                </tr>
            </asp:Panel>
            <asp:Panel ID="pnlDesignAdvancedSearch" runat="server" Visible="false">
                <tr>
                    <td width="20%" align="left">
                        <asp:Label ID="lblDesignConfidenceLevel" runat="server" CssClass="default" Text="Confidence Level" /></td>
                    <td width="30%" align="left">
                        <asp:DropDownList ID="ddlDesignConfidenceLevel" runat="server" Width="250" CssClass="default" />
                    </td>
                    <td width="20%" align="left">
                        <asp:Label ID="lblDesignStatus" runat="server" CssClass="default" Text="Status" />
                    </td>
                    <td width="30%" align="left">
                        <asp:DropDownList ID="ddlDesignStatus" runat="server" Width="250" CssClass="default" />
                    </td>
                </tr>
                <tr>
                    <td width="20%" align="left">
                        <asp:Label ID="lblDesignCreatedBy" runat="server" CssClass="default" Text="Created By" /></td>
                    <td width="30%" align="left">
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtDesignCreatedBy" runat="server" Width="250" CssClass="default" /></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:HiddenField ID="hdnDesignCreatedBy" Value="" runat="server" />
                                    <div id="divDesignCreatedBy" runat="server" style="overflow: hidden; position: absolute;
                                        display: none; background-color: #FFFFFF; border: solid 1px #CCCCCC">
                                        <asp:ListBox ID="lstDesignCreatedBy" runat="server" CssClass="default" />
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td width="20%" align="left">
                    </td>
                    <td width="30%" align="left">
                    </td>
                </tr>
                <tr>
                    <td width="20%" align="left">
                        <asp:Label ID="lblDesignCreatedAfter" runat="server" CssClass="default" Text="Created After" />
                    </td>
                    <td width="30%" align="left">
                        <asp:TextBox ID="txtDesignCreatedAfter" runat="server" Width="100" CssClass="default" />
                        <asp:ImageButton ID="imgDesignCreatedAfterDate" runat="server" CssClass="default"
                            ImageUrl="/images/calendar.gif" ImageAlign="AbsMiddle" />
                    </td>
                    <td width="20%" align="left">
                        <asp:Label ID="lblDesignCreatedBefore" runat="server" CssClass="default" Text="Created Before" />
                    </td>
                    <td width="30%" align="left">
                        <asp:TextBox ID="txtDesignCreatedBefore" runat="server" Width="100" CssClass="default" />
                        <asp:ImageButton ID="imgDesignCreatedBeforeDate" runat="server" CssClass="default"
                            ImageUrl="/images/calendar.gif" ImageAlign="AbsMiddle" />
                    </td>
                </tr>
                <tr>
                    <td width="20%" align="left">
                        <asp:Label ID="lblDesignModifiedAfter" runat="server" CssClass="default" Text="Modified After" />
                    </td>
                    <td width="30%" align="left">
                        <asp:TextBox ID="txtDesignModifiedAfter" runat="server" Width="100" CssClass="default" />
                        <asp:ImageButton ID="imgDesignModifiedAfterDate" runat="server" CssClass="default"
                            ImageUrl="/images/calendar.gif" ImageAlign="AbsMiddle" />
                    </td>
                    <td width="20%" align="left">
                        <asp:Label ID="lblDesignModifiedBefore" runat="server" CssClass="default" Text="Modified Before" />
                    </td>
                    <td width="30%" align="left">
                        <asp:TextBox ID="txtDesignModifiedBefore" runat="server" Width="100" CssClass="default" />
                        <asp:ImageButton ID="imgDesignModifiedBeforeDate" runat="server" CssClass="default"
                            ImageUrl="/images/calendar.gif" ImageAlign="AbsMiddle" />
                    </td>
                </tr>
                <tr>
                    <td width="20%" align="left">
                        <asp:Label ID="lblDesignCompletedAfter" runat="server" CssClass="default" Text="Completed After" />
                    </td>
                    <td width="30%" align="left">
                        <asp:TextBox ID="txtDesignCompletedAfter" runat="server" Width="100" CssClass="default" />
                        <asp:ImageButton ID="imgDesignCompletedAfterDate" runat="server" CssClass="default"
                            ImageUrl="/images/calendar.gif" ImageAlign="AbsMiddle" />
                    </td>
                    <td width="20%" align="left">
                        <asp:Label ID="lblDesignCompletedBefore" runat="server" CssClass="default" Text="Completed Before" />
                    </td>
                    <td width="30%" align="left">
                        <asp:TextBox ID="txtDesignCompletedBefore" runat="server" Width="100" CssClass="default" />
                        <asp:ImageButton ID="imgDesignCompletedBeforeDate" runat="server" CssClass="default"
                            ImageUrl="/images/calendar.gif" ImageAlign="AbsMiddle" />
                    </td>
                </tr>
            </asp:Panel>
            <asp:Panel ID="pnlCommon" runat="server" Visible="true">
                <tr>
                    <td width="20%">
                        Results per Page</td>
                    <td width="30%" align="left">
                        <asp:DropDownList ID="ddlSearch" runat="server" Width="75" CssClass="default">
                            <asp:ListItem Value="10" Text="10" />
                            <asp:ListItem Value="25" Text="25" Selected="True" />
                            <asp:ListItem Value="50" Text="50" />
                            <asp:ListItem Value="100" Text="100" />
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
                        <asp:Button ID="btnSearch" Text="Search" Width="100" CssClass="default" runat="server"
                            OnClick="btnSearch_Click" />
                    </td>
                    <td width="20%">
                    </td>
                    <td width="30%">
                    </td>
                </tr>
            </asp:Panel>
        </table>
        <asp:Panel ID="pnlResults" runat="server" Visible="false">
            <table width="95%" cellpadding="4" cellspacing="3" border="0">
                <tr>
                    <td class="default">
                        <asp:Label ID="lblRecords" runat="server" CssClass="bigger" /></td>
                    <td class="default" align="right">
                        <asp:LinkButton ID="btnBack" runat="server" Text="Previous Page" OnClick="btnBack_Click" />&nbsp;&nbsp;|&nbsp;&nbsp;<asp:LinkButton
                            ID="btnNext" runat="server" Text="Next Page" OnClick="btnNext_Click" />&nbsp;&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Panel ID="panService" runat="server" Visible="false">
                            <table width="100%" cellpadding="4" cellspacing="0" border="0" style="border: solid 1px #CCCCCC">
                                <asp:DataList ID="dlServices" runat="server" CellPadding="2" CellSpacing="1" Width="100%"
                                    OnItemDataBound="dlServices_ItemDataBound">
                                    <HeaderTemplate>
                                        <tr bgcolor="#EEEEEE">
                                            <td align="left" width="15%" nowrap>
                                                <asp:LinkButton ID="lnkHeaderRequestId" runat="server" CssClass="tableheader" Text="<b>Request#</b>"
                                                    OnClick="btnOrder_Click" CommandArgument="RequestNumber" />
                                            </td>
                                            <td align="left" width="50%">
                                                <asp:LinkButton ID="lnkHeaderServiceName" runat="server" CssClass="tableheader" Text="<b>Service Name </b>"
                                                    OnClick="btnOrder_Click" CommandArgument="ServiceName" />
                                            </td>
                                            <td align="left" width="10%">
                                                <asp:LinkButton ID="lnkHeaderServiceSubmitted" runat="server" CssClass="tableheader"
                                                    Text="<b>Submitted</b>" OnClick="btnOrder_Click" CommandArgument="SubmittedOn" />
                                            </td>
                                            <td align="center" width="15%">
                                                <asp:LinkButton ID="lnkHeaderServiceProgress" runat="server" CssClass="tableheader"
                                                    Text="<b>Progress</b>" />
                                            </td>
                                            <td align="left" width="10%">
                                                <asp:LinkButton ID="lnkHeaderServiceStatus" runat="server" CssClass="tableheader"
                                                    Text="<b>Status</b>" OnClick="btnOrder_Click" CommandArgument="ServiceStatus" />
                                            </td>
                                        </tr>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td align="left" nowrap>
                                                <asp:LinkButton ID="lnkRequestId" runat="server" CssClass="lookup" />
                                            </td>
                                            <td align="left">
                                                <asp:LinkButton ID="lnkServiceName" runat="server" CssClass="lookup" />
                                            </td>
                                            <td align="left">
                                                <asp:Label ID="lblServiceSubmitted" runat="server" CssClass="default" />
                                            </td>
                                            <td align="center">
                                                <asp:Label ID="lblServiceProgress" runat="server" CssClass="default" />
                                            </td>
                                            <td align="left">
                                                <asp:Label ID="lblServiceStatus" runat="server" CssClass="default" />
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <AlternatingItemTemplate>
                                        <tr bgcolor="#F6F6F6">
                                            <td align="left" nowrap>
                                                <asp:LinkButton ID="lnkRequestId" runat="server" CssClass="lookup" />
                                            </td>
                                            <td align="left">
                                                <asp:LinkButton ID="lnkServiceName" runat="server" CssClass="lookup" />
                                            </td>
                                            <td align="left">
                                                <asp:Label ID="lblServiceSubmitted" runat="server" CssClass="default" />
                                            </td>
                                            <td align="center">
                                                <asp:Label ID="lblServiceProgress" runat="server" CssClass="default" />
                                            </td>
                                            <td align="left">
                                                <asp:Label ID="lblServiceStatus" runat="server" CssClass="default" />
                                            </td>
                                        </tr>
                                    </AlternatingItemTemplate>
                                </asp:DataList>
                                <tr>
                                    <td colspan="6">
                                        <asp:Label ID="lblService" runat="server" CssClass="default" Visible="false" Text="<img src='/images/spacer.gif' border='0' height='1' width='25' /><img src='/images/alert.gif' border='0' align='absmiddle'> No results found" /></td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="panDesign" runat="server" Visible="false">
                            <table width="100%" cellpadding="4" cellspacing="0" border="0" style="border: solid 1px #CCCCCC">
                                <asp:Repeater ID="rptDesign" runat="server">
                                    <HeaderTemplate>
                                        <tr bgcolor="#EEEEEE">
                                            <td>
                                            </td>
                                            <td>
                                                #</td>
                                            <td nowrap>
                                                <asp:LinkButton ID="btnDesignId" runat="server" CssClass="tableheader" Text="<b>Design ID</b>"
                                                    OnClick="btnOrder_Click" CommandArgument="id" /></td>
                                            <td nowrap>
                                                <asp:LinkButton ID="btnDesignNickName" runat="server" CssClass="tableheader" Text="<b>Nickname</b>"
                                                    OnClick="btnOrder_Click" CommandArgument="name" /></td>
                                            <td nowrap>
                                                <asp:LinkButton ID="btnDesignReqBy" runat="server" CssClass="tableheader" Text="<b>Requested By</b>"
                                                    OnClick="btnOrder_Click" CommandArgument="UserName" /></td>
                                            <td nowrap>
                                                <asp:LinkButton ID="btnDesignProjNumber" runat="server" CssClass="tableheader" Text="<b>Project Number</b>"
                                                    OnClick="btnOrder_Click" CommandArgument="project_number" /></td>
                                            <td nowrap>
                                                <asp:LinkButton ID="btnDesignProjName" runat="server" CssClass="tableheader" Text="<b>Project Name</b>"
                                                    OnClick="btnOrder_Click" CommandArgument="project_name" /></td>
                                            <td nowrap>
                                                <asp:LinkButton ID="btnDesignStatus" runat="server" CssClass="tableheader" Text="<b>Status</b>"
                                                    OnClick="btnOrder_Click" CommandArgument="number" /></td>
                                        </tr>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                                            <td>
                                                <%# DataBinder.Eval(Container.DataItem, "RowNum")%>
                                            </td>
                                            <td width="10%" nowrap>
                                                <a class="lookup" href='/datapoint/service/design.aspx?t=<%#Request.QueryString["t"] %>&q=<%# oFunction.encryptQueryString(DataBinder.Eval(Container.DataItem, "id").ToString()) %>'>
                                                    <%# DataBinder.Eval(Container.DataItem, "id") %>
                                                </a>
                                            </td>
                                            <td width="20%" nowrap>
                                                <%# DataBinder.Eval(Container.DataItem, "name") %>
                                            </td>
                                            <td width="20%" nowrap>
                                                <asp:Label ID="lblUser" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "userid") %>' /></td>
                                            <td width="10%" nowrap title='ProjectID: <%# DataBinder.Eval(Container.DataItem, "projectid") %>'>
                                                <%# DataBinder.Eval(Container.DataItem, "project_number") %>
                                            </td>
                                            <td width="20%" nowrap>
                                                <a class="lookup" href='/datapoint/projects/datapoint_projects.aspx?id=<%# oFunction.encryptQueryString(DataBinder.Eval(Container.DataItem, "projectid").ToString())  %>'>
                                                    <%# DataBinder.Eval(Container.DataItem, "project_name") %>
                                                </a>
                                            </td>
                                            <td width="20%" nowrap align="left">
                                                <asp:Label ID="lblStatus" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "DesignStatusName") %>' /></td>
                                            <asp:Label ID="lblExecuted" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem, "executed").ToString() %>' />
                                            <asp:Label ID="lblCompleted" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem, "completed").ToString() %>' />
                                            <asp:Label ID="lblFinished" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem, "finished").ToString() %>' />
                                            <asp:Label ID="lblDelete" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem, "deleted").ToString() %>' />
                                            <asp:Label ID="lblDeleteForecast" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem, "forecast_deleted").ToString() %>' />
                                            <asp:Label ID="lblDeleteRequest" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem, "request_deleted").ToString() %>' />
                                            <asp:Label ID="lblDeleteProject" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem, "project_deleted").ToString() %>' />
                                        </tr>
                                    </ItemTemplate>
                                    <AlternatingItemTemplate>
                                        <tr bgcolor="#F6F6F6">
                                            <td>
                                                <img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                                            <td>
                                                <%# DataBinder.Eval(Container.DataItem, "RowNum")%>
                                            </td>
                                            <td width="10%" nowrap>
                                                <a class="lookup" href='/datapoint/service/design.aspx?t=<%#Request.QueryString["t"] %>&q=<%# oFunction.encryptQueryString(DataBinder.Eval(Container.DataItem, "id").ToString()) %>'>
                                                    <%# DataBinder.Eval(Container.DataItem, "id") %>
                                                </a>
                                            </td>
                                            <td width="20%" nowrap>
                                                <%# DataBinder.Eval(Container.DataItem, "name") %>
                                            </td>
                                            <td width="20%" nowrap>
                                                <asp:Label ID="lblUser" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "userid") %>' /></td>
                                            <td width="10%" nowrap title='ProjectID: <%# DataBinder.Eval(Container.DataItem, "projectid") %>'>
                                                <%# DataBinder.Eval(Container.DataItem, "project_number") %>
                                            </td>
                                            <td width="20%" nowrap>
                                                <a class="lookup" href='/datapoint/projects/datapoint_projects.aspx?id=<%# oFunction.encryptQueryString(DataBinder.Eval(Container.DataItem, "projectid").ToString())  %>'>
                                                    <%# DataBinder.Eval(Container.DataItem, "project_name") %>
                                                </a>
                                            </td>
                                            <td width="20%" nowrap align="left">
                                                <asp:Label ID="lblStatus" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "DesignStatusName") %>' /></td>
                                            <asp:Label ID="lblExecuted" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem, "executed").ToString() %>' />
                                            <asp:Label ID="lblCompleted" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem, "completed").ToString() %>' />
                                            <asp:Label ID="lblFinished" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem, "finished").ToString() %>' />
                                            <asp:Label ID="lblDelete" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem, "deleted").ToString() %>' />
                                            <asp:Label ID="lblDeleteForecast" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem, "forecast_deleted").ToString() %>' />
                                            <asp:Label ID="lblDeleteRequest" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem, "request_deleted").ToString() %>' />
                                            <asp:Label ID="lblDeleteProject" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem, "project_deleted").ToString() %>' />
                                        </tr>
                                    </AlternatingItemTemplate>
                                </asp:Repeater>
                                <tr>
                                    <td colspan="8">
                                        <asp:Label ID="lblDesign" runat="server" CssClass="default" Visible="false" Text="<img src='/images/spacer.gif' border='0' height='1' width='25' /><img src='/images/alert.gif' border='0' align='absmiddle'> No results found" /></td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </asp:Panel>
   
    <asp:Panel ID="pnlDenied" runat="server" Visible="false">
        <br />
            <table width="100%" cellpadding="0" cellspacing="5" border="0">
                <tr>
                    <td rowspan="2"><img src="/images/ico_error.gif" border="0" align="absmiddle" id="IMG1" onclick="return IMG1_onclick()" /></td>
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
    
<asp:HiddenField ID="hdnSearchType" runat="server" />



<asp:HiddenField ID="hdnOrder" Value="0" runat="server" />
<asp:HiddenField ID="hdnOrderBy" value ="" runat="server" />
<asp:HiddenField ID="hdnPageNo" Value="1" runat="server" />
<asp:HiddenField ID="hdnRecsPerPage" Value="0" runat="server" />

</asp:Content>
