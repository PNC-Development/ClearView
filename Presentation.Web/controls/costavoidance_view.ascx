<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="costavoidance_view.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.costavoidance_view" %>


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
                    <td nowrap>Cost Avoidance Opportunity:</td>
                    <td width="100%"><asp:Label ID="lblCAO" runat="server" CssClass="default" /></td>
                </tr>
                <tr>
                    <td nowrap>Description:</td>
                    <td width="100%"><asp:Label ID="lblDescription" runat="server" CssClass="default" /></td>
                </tr>  
                <tr>
                    <td nowrap valign="top">Upload Case Study:</td>
                    <td width="100%">                                             
                      <asp:HyperLink id="hypUpload" runat="server" Target="_blank" Text="Click Here to View File" /> 
                    </td>                      
                </tr> 
                <tr>
                    <td nowrap>Additional Cost Avoidance:</td>
                    <td width="100%"><asp:Label ID="lblAddtlCA" runat="server" CssClass="default" /></td>
                </tr>
                <tr>
                    <td nowrap valign="top">Date:</td>
                    <td width="100%"><asp:Label ID="lblDate" runat="server" CssClass="default" /></td>
                </tr> 
                <tr>
                    <td nowrap valign="top">Submitter:</td>
                    <td width="100%"><asp:Label ID="lblSubmitter" runat="server" CssClass="default" /></td>
                </tr> 
                <tr>
                    <td nowrap valign="top">Date Submitted:</td>
                    <td width="100%"><asp:Label ID="lblDateSubmit" runat="server" CssClass="default" /></td>
                </tr> 
                <tr>
                    <td colspan="2">&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="2">
                          <table width="100%" cellpadding="4" cellspacing="0" border="0" align="left" style="border: solid 1px #CCCCCC">
                              <asp:repeater id="rptView" runat="server">
                                  <HeaderTemplate>
                                      <tr bgcolor="#EEEEEE">
                                          <td><b>Category</b></td>
                                          <td><b>Item</b></td>                                      
                                          <td><b>Amount</b></td> 
                                          <td width="1"></td>                                     
                                      </tr>
                                  </HeaderTemplate>
                                  <ItemTemplate>
                                      <tr class="default">
                                          <td valign="top" nowrap><%# oCustomized.GetCategory(Int32.Parse(DataBinder.Eval(Container.DataItem, "categoryid").ToString()),"name") %></td>                                
                                          <td valign="top" nowrap><%# oCustomized.GetItem(Int32.Parse(DataBinder.Eval(Container.DataItem, "itemid").ToString()),"name")  %></td>                            
                                          <td valign="top" nowrap><%# String.Format("{0:C}", Double.Parse(oCustomized.GetItem(Int32.Parse(DataBinder.Eval(Container.DataItem, "itemid").ToString()), "amount")))%></td>                                                                    
                                      </tr>
                                  </ItemTemplate>
                              </asp:repeater>
                          </table>
                    </td>
                </tr>   
                <tr>
                    <td colspan="2">
                       <asp:Label ID="lblNone" runat="server" CssClass="default" Visible="false" Text="<img src='/images/alert.gif' border='0' align='absmiddle'> There are (0) items ..." />
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
 
