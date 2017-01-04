<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="sys_whatsnew.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.sys_whatsnew" %>

<table width="100%" cellpadding="0" cellspacing="0" border="0" class="default">

    
    <tr>
        <td><img src="/images/table_topleft.gif" border="0" width="5" height="26"></td>
        <td nowrap background="/images/table_top.gif" class="greentableheader" width="100%"> 
                 <table width="100%" >
                    <tr>
                        <td align="left" class="greentableheader">News</td>
                        <td align ="right">
                            <%--<asp:ImageButton  ImageUrl ="~/images/rss_button.gif" ID="ImageButton1"  runat="server" OnClick="imgRSS_Click" />--%>
                        </td>
                    </tr>
                 </table>
         </td>
        <td><img src="/images/table_topright.gif" border="0" width="5" height="26"></td>
    </tr>

    <tr>
        <td background="/images/table_left.gif"><img src="/images/table_left.gif" width="5" height="10"></td>
        <td width="100%" bgcolor="#FFFFFF">
            <br />
            <asp:repeater ID="rptNew" runat="server">
                <ItemTemplate>
                    <table cellpadding="4" cellspacing="0" border="0">
                        
                        <tr>
                            <td class="header" valign="bottom"><%# DataBinder.Eval(Container.DataItem, "Title") %></td>
                        </tr>
                        <tr>
                            <td valign="top" align ="left" colspan="2" ><%# DataBinder.Eval(Container.DataItem, "description") %></td>
                        </tr>
                        <asp:Panel ID="panAttachment" runat="server" Visible="false">
                            <tr>
                                <td  align ="left"  class="default" colspan="2">
                                <asp:Label ID="lblAttachment" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "attachment") %>' />
                                </td>
                            </tr>
                        </asp:Panel>
                        <tr >
                            <td class="default" > Author: <%# DataBinder.Eval(Container.DataItem, "CreatedByName").ToString()%></td>
                        </tr>
                         <tr>
                            <td class="default" > Published Date: <%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "Created").ToString()).ToString()%></td>
                        </tr>
                        
                    </table>
                </ItemTemplate>
                <SeparatorTemplate>
                    <table width="100%" cellpadding="4" cellspacing="0" border="0">
                        <tr>
                            <td><span style="width:100%;border-bottom:1 dotted #999999;"/></td>
                        </tr>
                    </table>
                </SeparatorTemplate>
                 <FooterTemplate>
                    
                 
                    
                </FooterTemplate>
            </asp:repeater>
            <asp:Label ID="lblNew" runat="server" CssClass="default" Visible="false" Text="<img src='/images/alert.gif' border='0' align='absmiddle'> There are no updates" />
            <p>&nbsp;</p>
            <table width="100%" cellpadding="1" cellspacing="0" border="0">
                     <tr>
                     <td> 
                         <asp:Label ID="lblPageNo" runat="server" Text=""></asp:Label>  </td>
                     <td align="right">
                         <input id="txtHdnPageNo" style="width: 28px" type="hidden" value="1" runat="server" />
                         <asp:LinkButton  ID="lnkBtnPrevious" runat="server" OnClick="lnkBtnPrevious_Click">Previous</asp:LinkButton>
                         <asp:LinkButton ID="lnkBtnNext" runat="server" OnClick="lnkBtnNext_Click">Next</asp:LinkButton>
                     </td>
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