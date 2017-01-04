<%@ Page Language="C#" MasterPageFile="~/datapoint.Master" AutoEventWireup="true" CodeBehind="DesignView.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.DesignView" Title="Design View" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" runat="server">

   <div id="divMain" >
   <br />
           <table id ="tblHeading" width="95%" style ="margin-left =2%" cellpadding="2"   cellspacing="0" border="0" class="default">
                <tr>
                    <td nowrap class="greentableheader" width="80%">Design Request Summary </td>
                    <td align="right"><input id="btnPrint" type="button" value="Print Page" class="default" onclick="window.print();"></td>
                   
                </tr>
                <tr><td colspan="2"><%=strHTMLPageHeader %> </td></tr>
            </table>

   
            <table id ="tblProjectInfo" width="95%" style ="margin-left =2%" cellpadding="0" cellspacing="0" border="0" class="default">
                    <tr>
                        <td nowrap class="greentableheader" width="100%">Project Information</td>
                    </tr>
                    
                    <tr>
                        <td  width="100%" bgcolor="#FFFFFF">
                            <table id="tblProject" width="100%" cellpadding="4" cellspacing="0" border="0" runat="server" >
                                <tr>
                                    <td valign="top" align ="left" width="100%" colspan="2" ><% =strHTMLProjectInfo %></td>
                                </tr>
                                <tr>
                                    <td valign="top" align ="left" width="100%" colspan="2" ><hr /></td>
                                </tr>
                            </table>
                        </td>
                        
                    </tr>
                 
            </table>


        <table id ="tblForecastDetails" width="95%" style ="margin-left =2%" cellpadding="0" cellspacing="0" border="0" class="default">
        <tr>
            <td nowrap class="greentableheader" width="100%">Design Details</td>
        </tr>
        
        <tr>
            <td >

                <table id="tblForeCastGeneral" width="100%" cellpadding="4" cellspacing="0" border="0" runat="server" >
                    <tr>
                        <td valign="top" align ="left" colspan="2" width="100%" ><% =strHTMLForecastInfoGeneral%></td>
                    </tr>
                </table>
               

                <table id="tblForeCastPlatform" width="100%" cellpadding="4" cellspacing="0" border="0" runat="server" >
                    <tr>
                        <td valign="top" align ="left" colspan="2" width="100%" ><% =strHTMLForecastInfoPlatform%></td>
                    </tr>
                </table>
                <table id="tblForeCaststorage" width="100%" cellpadding="4" cellspacing="0" border="0" runat="server" >
                    <tr>
                        <td valign="top" align ="left" colspan="2" ><% =strHTMLForecastInfoStorage%></td>
                    </tr>
                </table>
                <table id="tblBackupInfo" width="100%" cellpadding="4" cellspacing="0" border="0" runat="server" >
                    <tr>
                        <td valign="top" align ="left" colspan="2" ><% =strHTMLForecastBackupInfo %></td>
                    </tr>
                    
                    <tr>
                        <td valign="top" align ="left" colspan="2" >
                            <table id="tblBackupInclusion" width="100%" cellspacing="2"  border="0" runat="server" >
                                <tr bgcolor="#EEEEEE">
                                    <td class="bold">Backup Inclusions</td>
                                </tr>
                                <tr>
                                    <td class="bold">File/Folder</td>
                                </tr>
                                <tr>
                                    <td valign="top" align ="left" colspan="2"  >
                                        <asp:repeater ID="rptInclusions" runat="server">
                                        <ItemTemplate>
                                        <tr>
                                            <td><%# DataBinder.Eval(Container.DataItem, "path") %></td>                                                               
                                        </tr>
                                        </ItemTemplate>
                                        </asp:repeater>
                                     </td>
                                </tr>
                                <tr>
                                      <td colspan="2">
                                        <asp:Label ID="lblNoneInclusions" runat="server" CssClass="default" Visible="false" Text="There are no inclusions..." />
                                        </td>
                                 </tr>
                            </table>
                        </td>
                    </tr>
                     <tr>
                        <td valign="top" align ="left" colspan="2" >
                             <table id="tblBackupExclusion" width="100%" border="0" runat="server" >
                               <tr bgcolor="#EEEEEE">
                                    <td class="bold">Backup Exclusions</td>
                                </tr>
                                <tr>
                                    <td class="bold">File/Folder</td>
                                </tr>
                                <tr>
                                    <td valign="top" align ="left" colspan="2"  >
                                         <asp:repeater ID="rptExclusions" runat="server">
                                               <ItemTemplate>
                                                    <tr>
                                                        <td><%# DataBinder.Eval(Container.DataItem, "path") %></td>                                                               
                                                    </tr>
                                               </ItemTemplate>
                                         </asp:repeater>
                                    </td>
                                </tr>
                                 <tr>
                                     <td colspan="2">
                                        <asp:Label ID="lblNoneExclusions" runat="server" CssClass="default" Visible="false" Text="There are no exclusions..." />
                                     </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" align ="left" colspan="2" >
                            <table id="tblBackupArchive" width="100%" border="0" runat="server" >
                                <tr bgcolor="#EEEEEE">
                                    <td class="bold">Archive Requirements</td>
                                </tr>
                                <tr>
                                    <td class="bold">File/Folder</td>
                                </tr>
                                <tr>
                                    <td valign="top" align ="left" colspan="2"  >
                                        <asp:repeater ID="rptRetention" runat="server">
                                            <ItemTemplate>
                                               <tr>
                                                  <td colspan="3"><%# DataBinder.Eval(Container.DataItem, "path") %></td>                                                               
                                               </tr>
                                               <tr>
                                                  <td>&nbsp;&nbsp;&nbsp;</td>
                                                  <td nowrap>First Archival:</td>
                                                  <td width="100%"><%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "first").ToString()).ToShortDateString() %></td>
                                                  <td></td>
                                               </tr>
                                               <tr>
                                                  <td>&nbsp;&nbsp;&nbsp;</td>
                                                  <td nowrap>Archive Period:</td>
                                                  <td width="100%"><%# (DataBinder.Eval(Container.DataItem, "number").ToString() == "0" ? "" : DataBinder.Eval(Container.DataItem, "number") + " ") %><%# DataBinder.Eval(Container.DataItem, "type") %></td>
                                                  <td></td>
                                               </tr>
                                               <tr>
                                                  <td>&nbsp;&nbsp;&nbsp;</td>
                                                  <td nowrap>Start Time:</td>
                                                  <td width="100%"><%# DataBinder.Eval(Container.DataItem, "hour") %> <%# DataBinder.Eval(Container.DataItem, "switch") %></td>
                                                  <td></td>
                                               </tr>
                                               <tr>
                                                  <td>&nbsp;&nbsp;&nbsp;</td>
                                                  <td nowrap>Frequency:</td>
                                                  <td width="100%"><%# DataBinder.Eval(Container.DataItem, "occurence") %></td>
                                                  <td></td>
                                               </tr>
                                            </ItemTemplate>
                                        </asp:repeater>
                                    </td>
                                </tr>
                                 <tr>
                                     <td colspan="2">
                                        <asp:Label ID="lblNoneRetention" runat="server" CssClass="default" Visible="false" Text="There are no archive requirements..." />
                                     </td>
                                </tr>
                             </table>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" align ="left" colspan="2" >
                         <table id="tblBackupAdditionalInfo" width="100%" cellpadding="4" cellspacing="0" border="0" runat="server" >
                            <tr>
                                <td valign="top" align ="left" colspan="2" ><% =strHTMLForecastBackupAddInfo%></td>
                            </tr>
                         </table>
                        </td>
                    </tr>
                </table>
              
                
            </td>
             
        </tr>
        <tr>
            <td width="100%"><hr /></td>
        </tr>
        </table>

       
        <table id ="tblConfigurationDetails" width="95%" style ="margin-left =2%" cellpadding="0" cellspacing="0" border="0" class="default">
            <tr>
                <td nowrap class="greentableheader" width="100%">Configuration Details</td>
            </tr>
            <tr>
                <td valign="top" align ="left" colspan="2" ><% =strHTMLConfigDetailsApp %></td>
               
            </tr>
            <tr>
                <td valign="top" align ="left" colspan="2" >&nbsp;&nbsp;&nbsp;</td>
            </tr>
            <tr>
                <td valign="top" align ="left" colspan="2" ><% =strHTMLConfigDetailsDevice %></td>
               
            </tr>
            <tr>
                <td valign="top" align ="left" colspan="2" >&nbsp;&nbsp;&nbsp;</td>
            </tr>
            <tr>
                <td valign="top" align ="left" colspan="2" ><% =strHTMLConfigDetailsUser%></td>
               
            </tr>
            <tr>
                <td valign="top" align ="left" colspan="2" >&nbsp;&nbsp;&nbsp;</td>
            </tr>
            <tr>
                <td valign="top" align ="left" colspan="2" ><% =strHTMLConfigDetailsStorage%></td>
               
            </tr>
        </table>     
        
        </div>
</asp:Content>
