<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="admin_DataWarehouse.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.admin_DataWarehouse"  %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Web Content Management Administration</title>
    <link rel="stylesheet" type="text/css" href="/css/default.css" />
    <script type="text/javascript" src="/javascript/global.js"></script>
    <script type="text/javascript" src="/javascript/default.js"></script>
    <script type="text/javascript">  
  
    </script>
    <meta http-equiv="refresh" content="60" />

</head>
<body style="margin-top:0; margin-left:0" >
    <form id="frmAdminDW" runat="server">
    <div style="height:100%; overflow:auto">
        <table id="tblSelect" width="98%" cellpadding="3" cellspacing="0" border="0"  style="text-align: center" runat="server">
            <tr><td colspan="2">&nbsp;</td></tr>
            <tr><td colspan="2" style="text-align:left"><b>ClearView Data Warehouse : Job Status</b></td></tr>
            <tr><td colspan="2">&nbsp;</td></tr>
            
            <tr align="left">
                <td colspan="2" width="100%" bgcolor="#FFFFFF">    
                    <fieldset>
                        <legend class="tableheader"><b>Last Run Status</b></legend>
                            <table width="100%" cellpadding="4" cellspacing="0" border="0">
                            <tr valign="top">
                                <td  width ="30%"><b>Last Run Date</b> </td>
                                <td  width ="70%"><asp:Label ID="lblLastRunDate" runat="server" CssClass="default" Text="" /></td>
                            </tr> 
                            <tr valign="top">
                                <td width ="30%"><b>Last Run Time</b> </td>
                                <td width ="70%"><asp:Label ID="lblLastRunTime" runat="server" CssClass="default" Text="" /></td>
                            </tr> 
                            <tr valign="top">
                                <td width ="30%"><b>Last Run Status</b> </td>
                                <td width ="70%"><asp:Label ID="lblLastRunStatus" runat="server" CssClass="default" Text="" /></td>
                            </tr> 
                             <tr valign="top">
                                <td width ="30%"><b>Last Run Outcome Message</b> </td>
                                <td width ="70%"><asp:Label ID="lblRunOutComeMsg" runat="server" CssClass="default" Text="" /></td>
                            </tr> 
                            </table>
                      </fieldset>
                </td>
            </tr>
            
            <tr align="left">
                <td colspan="2" width="100%" bgcolor="#FFFFFF">    
                    <fieldset>
                        <legend class="tableheader"><b>Next Scheduled Run</b></legend>
                            <table width="100%" cellpadding="4" cellspacing="0" border="0">
                                <tr valign="top">
                                    <td width ="30%"><b>Next Run Date</b> </td>
                                    <td width ="70%"><asp:Label ID="lblNextRunDate" runat="server" CssClass="default" Text="" /></td>
                                </tr> 
                                <tr valign="top">
                                    <td width ="30%"><b>Next Run Time</b> </td>
                                    <td width ="70%"><asp:Label ID="lblNextRunTime" runat="server" CssClass="default" Text="" /></td>
                                </tr> 
                            </table>
                      </fieldset>
                </td>
            </tr>
            
            <tr align="left">
                <td colspan="2" width="100%" bgcolor="#FFFFFF">    
                    <fieldset>
                        <legend class="tableheader"><b>Run Job</b></legend>
                            <table width="100%" cellpadding="4" cellspacing="0" border="0">
                                <tr valign="top">
                                    <td width ="30%"><b>Current Job Status</b> </td>
                                    <td width ="70%">
                                        <asp:Label ID="lblCurrentJobStatus" runat="server" CssClass="default" Text=""  Width="100"/>&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btnRunJob" runat="server" CssClass="default" Width="100" Text="Run Job" OnClick="btnRunJob_Click" /> &nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btnRefresh" runat="server" CssClass="default" Width="100" Text="Refresh" OnClick="btnRefresh_Click" /> 
                                    </td>
                                </tr> 
                            </table>
                      </fieldset>
                </td>
            </tr>
            <tr align="left">
                <td colspan="2"></td>
            </tr>
             
              <tr align="left">
                <td colspan="2" width="100%" bgcolor="#FFFFFF">    
                    <fieldset>
                        <legend class="tableheader"><b>Steps</b></legend>
                            <table width="100%" cellpadding="4" cellspacing="0" border="0">
                                <tr valign="top">
                                    <td >
                                    
                                      <asp:DataList ID="dlJobSteps" runat="server" CellPadding="2" CellSpacing="1"  Width="100%" OnItemDataBound="dlJobSteps_ItemDataBound" >
                                        <HeaderTemplate>
                                            <tr style="background :#EEEEEE">
                                                <td  style=" text-align:left;vertical-align :top" > <b></b></td>
                                                <td  style="width:20% ;  text-align:left;vertical-align :top" > <b>Step Id</b></td>
                                                <td  style="width:20% ;  text-align:left;vertical-align :top" > <b>Step Name</b></td>
                                                <td  style="width:20% ;  text-align:left;vertical-align :top" > <b>Last Run Date</b></td>
                                                <td  style="width:20% ;  text-align:left;vertical-align :top" > <b>Last Run Time</b></td>
                                                <td  style="width:20% ;  text-align:left;vertical-align :top" > <b>Last Run Outcome</b></td>
                                            </tr>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr align="left" valign ="top" style="background:none">
                                               <td ><asp:Label ID="lblDLStepStatus" runat="server" CssClass="default" Text="" Width="20"/></td>
                                               <td ><asp:Label ID="lblDLStepId" runat="server" CssClass="default" Text="" /></td>
                                               <td ><asp:Label ID="lblDLStepName" runat="server" CssClass="default" Text="" /></td>
                                               <td ><asp:Label ID="lblDLStepLastRunDate" runat="server" CssClass="default" Text="" /></td>
                                               <td ><asp:Label ID="lblDLStepLastRunTime" runat="server" CssClass="default" Text="" /></td>
                                               <td ><asp:Label ID="lblDLStepLastRunOutCome" runat="server" CssClass="default" Text="" /></td>
                                            </tr>
                                        </ItemTemplate>
                                     </asp:DataList>
                     
                                    </td>
                                    
                                </tr> 
                            </table>
                      </fieldset>
                </td>
            </tr>
        </table>
    </div>  
    <asp:HiddenField ID="hdnCurrentExecutionStepId" runat="server" Value="0"/>
    </form>
</body>
</html>
