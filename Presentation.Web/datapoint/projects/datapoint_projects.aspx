<%@ Page Language="C#" MasterPageFile="~/datapoint.Master" AutoEventWireup="true" CodeBehind="datapoint_projects.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.datapoint_projects" Title="Untitled Page" %>
<%@ Register Src="~/datapoint/controls/wucProjectInfo.ascx"
    TagName="wucProjectInfo" TagPrefix="ucProjectInfo" %>

<%@ Register Src="~/datapoint/controls/wucProjectRequestDetails.ascx"
    TagName="wucProjectRequestDetails" TagPrefix="ucProjectRequestDetails" %>

<%@ Register Src="~/datapoint/controls/wucProjectFinancials.ascx"
    TagName="wucProjectFinancials" TagPrefix="ucProjectFinancials" %>
    
<%@ Register Src="~/datapoint/controls/wucResourceInvolvement.ascx"
    TagName="wucResourceInvolvement" TagPrefix="ucResourceInvolvement" %>
    
<%@ Register Src="~/datapoint/controls/wucServiceProgression.ascx" TagName="wucServiceProgression"
    TagPrefix="ucServiceProgression" %>
  
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" runat="server">
<asp:Panel ID="pnlAllow" runat="server" Visible="false">
    
    <asp:Panel ID="pnlProjectInfo" runat="server" Visible="true">
        <table width="100%" cellpadding="5" cellspacing="2" border="0">
            <tr>
                <td class="header" colspan="2">
                    Project Information</td>
            </tr>
        </table>
        <table width="95%" cellpadding="4" cellspacing="2" border="0">
            <tr>
                <td width="20%" align="left">
                    Project #</td>
                <td width="30%" align="left">
                    <asp:Label ID="lblProjectNumber" runat="server" CssClass="default" Text="" /></td>
                <td width="20%" align="left">
                    Project Name</td>
                <td width="30%" align="left">
                    <asp:Label ID="lblProjectName" runat="server" CssClass="default" Text="" />
                </td>
            </tr>
            <tr>
                <td width="20%" align="left">
                    Project Manager</td>
                <td width="30%" align="left">
                    <asp:Label ID="lblProjectManager" runat="server" CssClass="lookup" Text="" /></td>
                <td width="20%" align="left">
                    Project Type</td>
                <td width="30%" align="left">
                    <asp:Label ID="lblProjectType" runat="server" CssClass="default" Text=" " />
                </td>
            </tr>
            <tr>
                <td width="20%" align="left">
                    Project Description</td>
                <td width="30%" align="left">
                    <asp:Label ID="lblProjectDescription" runat="server" CssClass="default" Text="" /></td>
                <td width="20%" align="left">
                    Project Status</td>
                <td width="30%" align="left">
                    <asp:Label ID="lblProjectStatus" runat="server" CssClass="default" Text="" />
                </td>
            </tr>
            <tr>
                <td width="20%" align="left">
                    Executive Sponsor</td>
                <td width="30%" align="left">
                    <asp:Label ID="lblExecutiveSponsor" runat="server" CssClass="lookup" Text=" " />
                </td>
                <td width="20%" align="left">
                    Working Sponsor
                </td>
                <td width="30%" align="left">
                    <asp:Label ID="lblWorkingSponsor" runat="server" CssClass="lookup" Text="" /></td>
            </tr>
        </table>
        <table width="95%" cellpadding="4" cellspacing="2" border="0">
            <tr>
                <td>
                    <hr />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="pnlTabs" runat="server" Visible="true" >
        <%=strMenuTab1%>
        <div id="divMenu1" class="tabbing"  >
            <div id="divProjectInfo" style="width:100%;">
                <ucProjectInfo:wucProjectInfo ID="ucProjectInfo" runat="server" />
            </div>
            <div id="divProjectRequestInfo" >
                <ucProjectRequestDetails:wucProjectRequestDetails ID="ucProjectRequestDetails" runat="server" />
            </div>
            <div id="divProjectFinancials" >
                <ucProjectFinancials:wucProjectFinancials ID="ucProjectFinancials" runat="server" />
            </div>
            <div id="divProjectPriority">
                <table id="tblProjectPriority" width="95%" cellpadding="4" cellspacing="2"
                    border="0">
                    <tr>
                        <td class="header" width="100%" valign="bottom">
                            Project Priority</td>
                    </tr>
                    <tr>
                        <td width="100%" valign="bottom">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td width="100%" align="center" valign="top">
                            <%=strHTMLProjectPriority %>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="divProjectDocuments">
                <asp:Label ID="lblProjectDocuments" runat="server" CssClass="default" Text="Disabled" /></td>
            </div>
            <div id="divProjectScoreCard">
                <asp:Label ID="lblProjectScoreCard" runat="server" CssClass="default" Text="Disabled" /></td>
            </div>
            <div id="divProjectHistory">
                <asp:Label ID="lblProjectHistory" runat="server" CssClass="default" Text="Disabled" /></td>
            </div>

            <div id="divAssets" style="display: none">
                <table width="100%" cellpadding="5" cellspacing="2" border="0">
                    <tr>
                        <td class="header" colspan="2"><b>Assets</b></td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <table width="100%" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                                <tr bgcolor="#EEEEEE">
                                    <td><b><u>Device Name:</u></b></td>
                                    <td><b><u>Device Serial #:</u></b></td>
                                    <td><b><u>Device Model:</u></b></td>
                                    <td><b><u>Status:</u></b></td>
                                    <td><b><u>Design:</u></b></td>
                                    <td><b><u>Device Type:</u></b></td>
                                </tr>
                                <asp:repeater ID="rptAssets" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <asp:Label ID="lblErrorStep" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "ErrorStep") %>' />
                                            <asp:Label ID="lblErrorReason" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "ErrorReason") %>' />
                                            <td valign="top"><asp:Label ID="lblName" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "servername") %>' ToolTip='<%# DataBinder.Eval(Container.DataItem, "serverid") %>' /></td>
                                            <td valign="top"><asp:Label ID="lblSerial" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "serial") %>' ToolTip='<%# DataBinder.Eval(Container.DataItem, "assetid") %>' /></td>
                                            <td valign="top"><%# DataBinder.Eval(Container.DataItem, "modelname") %></td>
                                            <td valign="top"><asp:Label ID="lblStatus" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "designstatusname") %>' /></td>
                                            <td valign="top"><asp:Label ID="lblDesign" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "forecastanswerid") %>' /></td>
                                            <td valign="top"><%# DataBinder.Eval(Container.DataItem, "typename") %></td>
                                        </tr>
                                    </ItemTemplate>
                                    <AlternatingItemTemplate>
                                        <tr bgcolor="F6F6F6">
                                            <asp:Label ID="lblErrorStep" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "ErrorStep") %>' />
                                            <asp:Label ID="lblErrorReason" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "ErrorReason") %>' />
                                            <td valign="top"><asp:Label ID="lblName" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "servername") %>' ToolTip='<%# DataBinder.Eval(Container.DataItem, "serverid") %>' /></td>
                                            <td valign="top"><asp:Label ID="lblSerial" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "serial") %>' ToolTip='<%# DataBinder.Eval(Container.DataItem, "assetid") %>' /></td>
                                            <td valign="top"><%# DataBinder.Eval(Container.DataItem, "modelname") %></td>
                                            <td valign="top"><asp:Label ID="lblStatus" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "designstatusname") %>' /></td>
                                            <td valign="top"><asp:Label ID="lblDesign" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "forecastanswerid") %>' /></td>
                                            <td valign="top"><%# DataBinder.Eval(Container.DataItem, "typename") %></td>
                                        </tr>
                                    </AlternatingItemTemplate>
                                </asp:repeater>
                                <tr>
                                    <td colspan="10">
                                        <asp:Label ID="lblAssets" runat="server" CssClass="default" Visible="false" Text="<img src='/images/spacer.gif' border='0' height='1' width='25' /><img src='/images/alert.gif' border='0' align='absmiddle'> There are no assets related to this project" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="divProvisioningStatus" >
                 <%=strMenuDivs %>
                <div id="divMenu2" class="tabbing">
                 <%=strDivs %>
                 </div>
            </div>
            
            <div id="divServices" style="display: none">
                  <ucServiceProgression:wucServiceProgression ID="ucServiceProgression" runat="server" />
            </div>
            <div id="divResourceInvolvement">
                <ucResourceInvolvement:wucResourceInvolvement ID="ucResourceInvolvement" runat="server" />
            </div>
            
        </div>
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
    
    <asp:HiddenField ID="hdnTab" runat="server" />
     <asp:HiddenField ID="hdnProvisioningTab" runat="server" />
    <div id="spnWait"><p><img src="/images/savingBlue.gif" border="0" align="absmiddle" /></p><p><b>Loading...</b></p></div>
</asp:Content>
