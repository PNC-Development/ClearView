<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserContactInfo.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.UserContactInfo" %>


            
   <asp:Panel ID="pnlContactInfo" runat="server" Visible="true">
        <table  width="100%" cellpadding="2" cellspacing="5" border="0">
            <tr>
                <td class="header" colspan="2">
                    <img src="/images/additional.gif" border="0" align="absmiddle" />My Contact Information
                </td>
            </tr>
            <tr>
                <td class="header" colspan="2" align="center">
                     <asp:Label ID="lblContactInfoSaved" runat="server" Text="<img src='/images/bigCheck.gif' border='0' align='absmiddle' /> Contact Information Saved"  Visible="false"  />
                </td>
            </tr>
            <tr>
            <td colspan="2" align="center">
                <fieldset>
                    <legend class="tableheader"><b>Work</b></legend>
                    <table width="100%" cellpadding="0" cellspacing="3" border="0">
                        <tr> 
                            <td nowrap width="20%">Title:</td>
                            <td width="30%"><asp:TextBox ID="txtWorkTitle" CssClass="default" runat="server" Width="200" MaxLength="100" /></td>
                            <td nowrap width="20%"></td>
                            <td nowrap width="30%"></td>
                        </tr>
                        <tr> 
                            <td nowrap width="20%">Company:</td>
                            <td width="30%"><asp:TextBox ID="txtWorkCompany" CssClass="default" runat="server" Width="200" MaxLength="100"/></td>
                            <td nowrap width="20%">Department:</td>
                            <td width="30%"><asp:TextBox ID="txtWorkDepartment" CssClass="default" runat="server" Width="200" MaxLength="100"/></td>
                        </tr>
                        <tr> 
                            <td nowrap width="20%">Phone #:</td>
                            <td width="30%"><asp:TextBox ID="txtWorkPhoneNo" CssClass="default" runat="server" Width="100" MaxLength="50" ToolTip="For example <111-111-1111>"/></td>
                            <td nowrap width="20%">Fax #:</td>
                            <td width="30%"><asp:TextBox ID="txtWorkFaxNo" CssClass="default" runat="server" Width="100" MaxLength="50" ToolTip="For example <111-111-1111>"/></td>
                        </tr>
                        <tr> 
                            <td nowrap  width="20%">Cell #:</td>
                            <td width="30%"><asp:TextBox ID="txtWorkCellNo" CssClass="default" runat="server" Width="100" MaxLength="50" ToolTip="For example <111-111-1111>"/></td>
                            <td nowrap width="20%">Pager #:</td>
                            <td width="30%"><asp:TextBox ID="txtWorkPager" CssClass="default" runat="server" Width="100" MaxLength="50"/>
                                            @<asp:dropdownlist ID="ddlWorkPagerAt" CssClass="default" runat="server"/>
                            </td>
                        </tr>
                        <tr> 
                            <td nowrap width="20%">Email:</td>
                            <td width="30%"><asp:TextBox ID="txtWorkEmail" CssClass="default" runat="server" Width="200" MaxLength="100" ToolTip="For example <someone@example.com>"/></td>
                            <td nowrap width="20%"></td>
                            <td nowrap width="30%"></td>
                        </tr>
                        <tr> 
                            <td nowrap width="20%"><b>Address:</b></td>
                            <td width="30%"></td>
                            <td nowrap width="20%"></td>
                            <td width="30%"></td>
                        </tr>
                        <tr> 
                            <td nowrap>Mail Locator:</td>
                            <td width="30%"><asp:TextBox ID="txtWorkMailLocator" CssClass="default" runat="server" Width="200" MaxLength="100"/></td>
                            <td nowrap>Office/Cube #:</td>
                            <td width="30%"><asp:TextBox ID="txtWorkOfficeNo" CssClass="default" runat="server" Width="200" MaxLength="100"/></td>
                        </tr>
                        <tr> 
                            <td nowrap width="20%">Address 1:</td>
                            <td width="30%"><asp:TextBox ID="txtWorkAddressLine1" CssClass="default" runat="server" Width="200" MaxLength="100"/> </td>
                            <td nowrap width="20%">Address 2(Apt, floor, suite, etc.):</td>
                            <td width="30%"><asp:TextBox ID="txtWorkAddressLine2" CssClass="default" runat="server" Width="200" MaxLength="100"/> </td>
                        </tr>
                        <tr> 
                            <td nowrap width="20%">City:</td>
                            <td width="30%"><asp:TextBox ID="txtWorkCity" CssClass="default" runat="server" Width="200" MaxLength="100"/> </td>
                            <td nowrap width="20%">State:</td>
                            <td width="30%"><asp:TextBox ID="txtWorkState" CssClass="default" runat="server" Width="200" MaxLength="100"/> </td>
                        </tr>
                        <tr> 
                            <td nowrap width="20%">ZIP Code:</td>
                            <td width="30%"><asp:TextBox ID="txtWorkZIP" CssClass="default" runat="server" Width="100" MaxLength="50"/> </td>
                            <td nowrap width="20%">Country:</td>
                            <td width="30%"><asp:TextBox ID="txtWorkCountry" CssClass="default" runat="server" Width="200" MaxLength="100"/></td>
                        </tr>
                    </table>
                </fieldset>
                <br />
                <fieldset>
                     <legend class="tableheader"><b>Home</b></legend>
                     <table width="100%" cellpadding="0" cellspacing="3" border="0">
                        <tr> 
                            <td nowrap width="20%">Phone #:</td>
                            <td width="30%"><asp:TextBox ID="txtHomePhoneNo" CssClass="default" runat="server" Width="100" MaxLength="50" ToolTip="For example <111-111-1111>"/></td>
                            <td nowrap width="20%">Fax #:</td>
                            <td width="30%"><asp:TextBox ID="txtHomeFaxNo" CssClass="default" runat="server" Width="100" MaxLength="50" ToolTip="For example <111-111-1111>"/></td>
                        </tr>
                        <tr> 
                            <td nowrap  width="20%">Cell #:</td>
                            <td width="30%"><asp:TextBox ID="txtHomeCellNo" CssClass="default" runat="server" Width="100" MaxLength="50" ToolTip="For example <111-111-1111>"/></td>
                             <td nowrap width="20%">Pager #:</td>
                            <td width="30%"><asp:TextBox ID="txtHomePager" CssClass="default" runat="server" Width="100" MaxLength="50"/>
                                            @<asp:dropdownlist ID="ddlHomePagerAt" CssClass="default" runat="server"/>
                            </td>
                        </tr>
                        <tr> 
                            <td nowrap width="20%">Email:</td>
                            <td width="30%"><asp:TextBox ID="txtHomeEmail" CssClass="default" runat="server" Width="200" MaxLength="100" ToolTip="For example <someone@example.com>"/></td>
                            <td nowrap width="20%"></td>
                            <td nowrap width="30%"></td>
                        </tr>
                        <tr> 
                            <td nowrap width="20%"><b>Address:</b></td>
                            <td width="30%"></td>
                            <td nowrap width="20%"></td>
                            <td width="30%"></td>
                        </tr>
                        <tr> 
                            <td nowrap width="20%">Address 1:</td>
                            <td width="30%"><asp:TextBox ID="txtHomeAddressLine1" CssClass="default" runat="server" Width="200" MaxLength="100"/> </td>
                            <td nowrap width="20%">Address 2(Apt, floor, suite, etc.):</td>
                            <td width="30%"><asp:TextBox ID="txtHomeAddressLine2" CssClass="default" runat="server" Width="200" MaxLength="100"/> </td>
                        </tr>
                        <tr> 
                            <td nowrap width="20%">City:</td>
                            <td width="30%"><asp:TextBox ID="txtHomeCity" CssClass="default" runat="server" Width="200" MaxLength="100"/> </td>
                             <td nowrap width="20%">State:</td>
                            <td width="30%"><asp:TextBox ID="txtHomeState" CssClass="default" runat="server" Width="200" MaxLength="100"/> </td>
                        </tr>
                        <tr> 
                            <td nowrap width="20%">ZIP Code:</td>
                            <td width="30%"><asp:TextBox ID="txtHomeZip" CssClass="default" runat="server" Width="100" MaxLength="50"/> </td>
                            <td nowrap width="20%">Country:</td>
                            <td width="30%"><asp:TextBox ID="txtHomeCountry" CssClass="default" runat="server" Width="200" MaxLength="100"/></td>
                        </tr>
                     </table>
                </fieldset>
            
            </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <br />
                    <asp:Button ID="btnSaveContactInfo" CssClass="default" runat="server" Text="Save" Width="125" OnClick="btnSaveContactInfo_Click" />
                </td>
            </tr>
        </table>
   </asp:Panel>
<asp:HiddenField ID="hdnUserId" runat="server" />
          
