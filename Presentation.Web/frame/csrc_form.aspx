<%@ Page Language="C#" MasterPageFile="~/frame.Master" AutoEventWireup="true" CodeBehind="csrc_form.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.csrc_form" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" Runat="Server"> 
<script type="text/javascript">
 function chkHidden(chk,oHidden)
 {   
   var chk = document.getElementById(chk);
   var oHidden = document.getElementById(oHidden);  
   oHidden.value = chk.checked;
   return true;
 }
 
 function CheckCSRC(o1,o2,o3,o4,oDiv) {
        o1 = document.getElementById(o1);
        o2 = document.getElementById(o2);
        o3 = document.getElementById(o3);
        o4 = document.getElementById(o4);
        oDiv = document.getElementById(oDiv);
        if (o1.checked == true || o2.checked == true || o3.checked == true || o4.checked == true)
            oDiv.style.display = "inline";
        else
            oDiv.style.display = "none";
    }
 
</script>
<table width="100%" height="100%" cellpadding="3" cellspacing="0" border="0" align="center">
   <tr height="1">
      <td class="frame" nowrap>&nbsp;CSRC Information</td>
      <td class="frame" align="right"><a href="javascript:void(0);" onclick="parent.HidePanel();"><img src="/images/close.gif" border="0" title="Close"></a></td>
   </tr>
   <tr>
      <td colspan="2">
      <div style="width:100%;height:100%; overflow:auto;">        
       <table width="100%" cellpadding="0" cellspacing="0" border="0" class="default">          
           <tr>
               <td colspan="3" style="border-left: solid 1px #999999; border-right: solid 1px #999999;
                   border-top: solid 1px #999999; border-bottom: solid 1px #999999;">
                   <table width="100%" cellpadding="4" cellspacing="3" border="0" class="default">
                       <tr>
                           <td colspan="2">
                               <table width="85%" cellpadding="4" cellspacing="3" border="0" class="default">                                   
                                    <tr>
                                        <td nowrap><b>PMM Phase:</b></td>
                                        <td width="100%">
                                            <asp:CheckBox ID="chkDiscovery" Text="Discovery" runat="server" CssClass="default"  />
                                            <asp:CheckBox ID="chkPlanning" Text="Planning" runat="server" CssClass="default"  />
                                            <asp:CheckBox ID="chkExecution" Text="Execution" runat="server" CssClass="default"  />
                                            <asp:CheckBox ID="chkClosing" Text="Closing" runat="server" CssClass="default"  />
                                        </td>
                                    </tr>
                                    <div id="divCSRC" runat="server" style="display:none"> 
                                   <tr>                                    
                                        <td colspan="2">                                       
                                              <div id="divDiscovery" runat="server" style="display: none">
                                                <br />
                                                <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                                    <tr>
                                                        <td colspan="3" bgcolor="#6A8359" class="whitedefault">
                                                            <div style="padding: 3"><b>Discovery</b></div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3" style="border-left: solid 1px #999999; border-right: solid 1px #999999;
                                                            border-top: solid 1px #999999;">
                                                            <table width="100%" cellpadding="4" cellspacing="3" border="0" class="default">
                                                                <tr>
                                                                    <td nowrap><b>Phase Start Date:</b></td>
                                                                    <td width="50%"><asp:TextBox ID="txtCSRCSD" runat="server" CssClass="default" Width="100"  /></td>
                                                                    <td nowrap><b>Internal Labor:</b></td>
                                                                    <td width="50%"><asp:TextBox ID="txtCSRCID" runat="server" CssClass="default" Width="100"  /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td nowrap><b>Phase End Date:</b></td>
                                                                    <td width="50%"><asp:TextBox ID="txtCSRCED" runat="server" CssClass="default" Width="100"  /></td>                                                                                                                                                
                                                                    <td nowrap><b>External Labor:</b></td>                                                                        
                                                                    <td width="50%"><asp:TextBox ID="txtCSRCExD" runat="server" CssClass="default" Width="100"  /></td>                                                                                                                                               
                                                                </tr>
                                                                <tr>
                                                                    <td nowrap>&nbsp;</td>                                                                        
                                                                    <td width="50%">&nbsp;</td>                                                                        
                                                                    <td nowrap><b>HW/SW/One Time Cost:</b></td>                                                                        
                                                                    <td width="50%"><asp:TextBox ID="txtCSRCHD" runat="server" CssClass="default" Width="100"  /></td>                                                                                             
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td><img src="/images/shadow_box_l.gif" border="0" width="5" height="12"></td>                                                            
                                                        <td width="100%" background="/images/shadow_box.gif"></td>                                                       
                                                        <td><img src="/images/shadow_box_r.gif" border="0" width="5" height="12"></td>
                                                    </tr>
                                                </table>
                                            </div>
                                              <div id="divPlanning" runat="server" style="display: none">
                                                <br />
                                                <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                                    <tr>
                                                        <td colspan="3" bgcolor="#6A8359" class="whitedefault">
                                                            <div style="padding: 3"> <b>Planning</b></div>                                                               
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3" style="border-left: solid 1px #999999; border-right: solid 1px #999999;
                                                            border-top: solid 1px #999999;">
                                                            <table width="100%" cellpadding="4" cellspacing="3" border="0" class="default">
                                                                <tr>
                                                                    <td nowrap><b>Phase Start Date:</b></td>                                                                        
                                                                    <td width="50%"><asp:TextBox ID="txtCSRCSP" runat="server" CssClass="default" Width="100"   /></td>                                                                   
                                                                    <td nowrap><b>Internal Labor:</b></td>                                                                        
                                                                    <td width="50%"><asp:TextBox ID="txtCSRCIP" runat="server" CssClass="default" Width="100"  /></td>                                                                  
                                                                </tr>
                                                                <tr>
                                                                    <td nowrap><b>Phase End Date:</b></td>                                                                        
                                                                    <td width="50%"><asp:TextBox ID="txtCSRCEP" runat="server" CssClass="default" Width="100"   /></td>                                                                
                                                                    <td nowrap><b>External Labor:</b></td>                                                                        
                                                                    <td width="50%"><asp:TextBox ID="txtCSRCExP" runat="server" CssClass="default" Width="100"   /></td>                                                                                             
                                                                </tr>
                                                                <tr>
                                                                    <td nowrap>&nbsp;</td>                                                                        
                                                                    <td width="50%">&nbsp;</td>                                                                        
                                                                    <td nowrap><b>HW/SW/One Time Cost:</b></td>                                                                        
                                                                    <td width="50%"><asp:TextBox ID="txtCSRCHP" runat="server" CssClass="default" Width="100"  /></td>                                                               
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td><img src="/images/shadow_box_l.gif" border="0" width="5" height="12"></td>                                                            
                                                        <td width="100%" background="/images/shadow_box.gif"></td>                                                        
                                                        <td><img src="/images/shadow_box_r.gif" border="0" width="5" height="12"></td>                                                            
                                                    </tr>
                                                </table>
                                            </div>
                                              <div id="divExecution" runat="server" style="display: none">
                                                <br />
                                                <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                                    <tr>
                                                        <td colspan="3" bgcolor="#6A8359" class="whitedefault">
                                                            <div style="padding: 3"><b>Execution</b></div>                                                                
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3" style="border-left: solid 1px #999999; border-right: solid 1px #999999;
                                                            border-top: solid 1px #999999;">
                                                            <table width="100%" cellpadding="4" cellspacing="3" border="0" class="default">
                                                                <tr>
                                                                    <td nowrap><b>Phase Start Date:</b></td>                                                                        
                                                                    <td width="50%"><asp:TextBox ID="txtCSRCSE" runat="server" CssClass="default" Width="100"   /></td>                                                                      
                                                                    <td nowrap><b>Internal Labor:</b></td>                                                                        
                                                                    <td width="50%"><asp:TextBox ID="txtCSRCIE" runat="server" CssClass="default" Width="100"   /></td>                                                                                                                                       
                                                                </tr>
                                                                <tr>
                                                                    <td nowrap><b>Phase End Date:</b></td>                                                                        
                                                                    <td width="50%"><asp:TextBox ID="txtCSRCEE" runat="server" CssClass="default" Width="100"    /></td>                                                           
                                                                    <td nowrap><b>External Labor:</b></td>                                                                        
                                                                    <td width="50%"><asp:TextBox ID="txtCSRCExE" runat="server" CssClass="default" Width="100"   /></td>                                                                                                                                                
                                                                </tr>
                                                                <tr>
                                                                    <td nowrap>&nbsp;</td>                                                                        
                                                                    <td width="50%">&nbsp;</td>                                                                        
                                                                    <td nowrap><b>HW/SW/One Time Cost:</b></td>                                                                        
                                                                    <td width="50%"><asp:TextBox ID="txtCSRCHE" runat="server" CssClass="default" Width="100"  /></td>                                                   
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td><img src="/images/shadow_box_l.gif" border="0" width="5" height="12"></td>                                                            
                                                        <td width="100%" background="/images/shadow_box.gif"></td>                                                        
                                                        <td><img src="/images/shadow_box_r.gif" border="0" width="5" height="12"></td>                                                            
                                                    </tr>
                                                </table>
                                            </div>
                                              <div id="divClosing" runat="server" style="display: none">
                                                <br />
                                                <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                                    <tr>
                                                        <td colspan="3" bgcolor="#6A8359" class="whitedefault">
                                                            <div style="padding: 3"><b>Closing</b></div>                                                                
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3" style="border-left: solid 1px #999999; border-right: solid 1px #999999;
                                                            border-top: solid 1px #999999;">
                                                            <table width="100%" cellpadding="4" cellspacing="3" border="0" class="default">
                                                                <tr>
                                                                    <td nowrap><b>Phase Start Date:</b></td>                                                                        
                                                                    <td width="50%"><asp:TextBox ID="txtCSRCSC" runat="server" CssClass="default" Width="100"  /></td>                                                                                                                                                
                                                                    <td nowrap><b>Internal Labor:</b></td>                                                                        
                                                                    <td width="50%"><asp:TextBox ID="txtCSRCIC" runat="server" CssClass="default" Width="100"  /></td>                                                                                                                                               
                                                                </tr>
                                                                <tr>
                                                                    <td nowrap><b>Phase End Date:</b></td>                                                                        
                                                                    <td width="50%"><asp:TextBox ID="txtCSRCEC" runat="server" CssClass="default" Width="100"  /></td>                                                                                                                                                
                                                                    <td nowrap><b>External Labor:</b></td>                                                                        
                                                                    <td width="50%"><asp:TextBox ID="txtCSRCExC" runat="server" CssClass="default" Width="100"  /></td>                                                                                                                                  
                                                                </tr>
                                                                <tr>
                                                                    <td nowrap>&nbsp;</td>                                                                        
                                                                    <td width="50%">&nbsp;</td>                                                                        
                                                                    <td nowrap><b>HW/SW/One Time Cost:</b></td>                                                                        
                                                                    <td width="50%"><asp:TextBox ID="txtCSRCHC" runat="server" CssClass="default" Width="100"  /></td>                                                                                                                                               
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td><img src="/images/shadow_box_l.gif" border="0" width="5" height="12"></td>                                                            
                                                        <td width="100%" background="/images/shadow_box.gif">
                                                        </td>
                                                        <td><img src="/images/shadow_box_r.gif" border="0" width="5" height="12"></td>                                                            
                                                    </tr>
                                                </table>
                                            </div>                                                                              
                                        </td>
                                   </tr>
                                   </div>
                               </table>
                           </td>
                       </tr>                              
                   </table>
               </td>
           </tr> 
            <tr>
              <td colspan="2" align="right"><asp:Button ID="btnUpdate" runat="server" Text="Update CSRC" CssClass="default" Width="95" OnClick="btnUpdate_Click" /></td>              
           </tr>         
       </table> 
       </div>      
     </td>
   </tr>   
</table>  
<asp:HiddenField ID="hdnPCRD" runat="server" /> 
<asp:HiddenField ID="hdnPCRC" runat="server" />
<asp:HiddenField ID="hdnStatus" runat="server" />
</asp:Content>

 
