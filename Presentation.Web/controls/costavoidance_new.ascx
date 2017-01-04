<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="costavoidance_new.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.costavoidance_new" %>


<script type="text/javascript">
    function EnsureNumber(oControl, strAlert) {
        var oControl2 = document.getElementById(oControl);
        if (trim(oControl2.value) == "")
            return true;
        else
            return ValidateNumber0(oControl, strAlert);
    }
</script>
<table width="100%" cellpadding="0" cellspacing="0" border="0" class="default">
    <tr>
        <td><img src="/images/table_topleft.gif" border="0" width="5" height="26"></td>
        <td nowrap background="/images/table_top.gif" class="greentableheader" width="100%"><asp:Label ID="lblTitle" runat="server" CssClass="greentableheader" /></td>
        <td><img src="/images/table_topright.gif" border="0" width="5" height="26"></td>
    </tr>
    <tr>
        <td background="/images/table_left.gif"><img src="/images/table_left.gif" width="5" height="10"></td>
        <td width="100%" bgcolor="#FFFFFF">
            <br /> 
            <table width="100%" cellpadding="0" cellspacing="5" border="0">
                <tr>
                    <td rowspan="2"><img src="/images/cost.gif" border="0" align="absmiddle" /></td>
                    <td class="header" width="100%" valign="bottom"> Cost Avoidance</td>
                </tr>
                <tr>
                    <td width="100%" valign="top">Complete the following form to submit your cost avoidance.</td>
                </tr>
            </table>
              <table width="100%" cellpadding="4" cellspacing="3" border="0">
               <tr>
                    <td nowrap>Cost Avoidance Opportunity:<font class="required">&nbsp;*</font></td>
                    <td width="100%"><asp:TextBox ID="txtCAO" runat="server" CssClass="default" Width="400" MaxLength="100" /></td>
                </tr>
                <tr>
                    <td nowrap>Description:</td>
                    <td width="100%"><asp:TextBox ID="txtDescription" runat="server" CssClass="default" Width="400" Rows="7" TextMode="MultiLine" /></td>
                </tr>  
                <tr>
                    <td nowrap valign="top">Upload Case Study:</td>
                    <td width="100%">
                        <asp:Panel ID="panUpload" runat="server" Visible="false">
                            <asp:FileUpload id="fileUpload" runat="server" CssClass="default" Width="400" />
                        </asp:Panel>
                        <asp:Panel ID="panUploaded" runat="server" Visible="false">
                            <asp:HyperLink id="hypUpload" runat="server" Target="_blank" Text="Click Here to View File" />&nbsp;&nbsp;&nbsp;<asp:Button ID="btnDeleteAttachment" runat="server" CssClass="default" Width="75" Text="Delete" OnClick="btnDeleteAttachment_Click" />
                        </asp:Panel>
                    </td>                      
                </tr> 
                <tr>
                    <td nowrap>Additional Cost Avoidance:</td>
                    <td width="100%"><asp:TextBox ID="txtAddtlCA" runat="server" CssClass="default" Width="150" MaxLength="100" /> (Please enter a valid $ amount)</td>
                </tr>
                <tr>
                    <td nowrap valign="top">Date:<font class="required">&nbsp;*</font></td>
                    <td width="100%"><asp:TextBox id="txtDate" runat="server" CssClass="default" Width="100" /> <asp:ImageButton ID="imgDate" runat="server" CssClass="default" ImageUrl="/images/calendar.gif" ImageAlign="AbsMiddle" /></td>
                </tr> 
                <tr>
                    <td nowrap>Category/Item Mapping:</td>
                    <td width="100%" class="default">
                       <asp:Panel ID="panNew" runat="server" Visible="false">
                           <a href="javascript:void(0);" onclick="return OpenWindow('COST_AVOIDANCE','?id=<%= intId %>');" >Click to map Category/Items</a>
                       </asp:Panel>          
                       <asp:Panel ID="panView" runat="server" Visible="false">
                           <asp:Button ID="btnView" runat="server" CssClass="default" Width="75" Text="View" /> 
                       </asp:Panel>               
                    </td>
                </tr>
                <tr>
                    <td colspan="2"><hr size="1" noshade /></td>
                </tr>
                <tr>
                    <td class="required">* Required Field</td>
                    <td width="100%" align="right"><asp:Button ID="btnSave" runat="server" CssClass="default" Text="Submit" Width="85" OnClick="btnSave_Click" Visible="false"/> <asp:Button ID="btnUpdate" runat="server" CssClass="default"  Text="Update" Width="100" OnClick="btnUpdate_Click" Visible="false" /></td>
                </tr>                     
            </table>   
        </td>
        <td background="/images/table_right.gif"><img src="/images/table_right.gif" width="5" height="10"></td>
    </tr>
    <tr>
        <td><img src="/images/table_bottomLeft.gif" border="0" width="5" height="9"></td>
        <td width="100%" background="/images/table_bottom.gif"></td>
        <td><img src="/images/table_bottomRight.gif" border="0" width="5" height="9"></td>
    </tr>
</table>
 <input type="hidden" id="hdnCount" runat="server" />