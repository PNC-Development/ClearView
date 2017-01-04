<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="rr_IDC.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.rr_IDC" %>



<script type="text/javascript">
    function ShowInvolvementReason(oDDL, oDiv) {
        oDiv = document.getElementById(oDiv);
        if (oDDL.options[oDDL.selectedIndex].value == "No")
            oDiv.style.display = "inline";
        else
            oDiv.style.display = "none";
    }
    function EditAsset(strRequest,strItem,strNumber,strID)
    {
       return OpenWindow('IDCASSET_TYPE','?rid=' + strRequest + '&iid=' + strItem + '&num=' + strNumber + '&id=' + strID);
    }
    
    function EditResource(strRequest,strItem,strNumber,strID)
    {
       return OpenWindow('IDCRESOURCE_ASSIGN','?rid=' + strRequest + '&iid=' + strItem + '&num=' + strNumber + '&id=' + strID);
    }
    function refreshIE() {
        document.forms[0].submit();
    }
</script>
<br />
<table width="100%" border="0" cellSpacing="2" cellPadding="4" class="default">
    <tr>
        <td class="header"><asp:Label ID="lblHeader" runat="server" CssClass="header" /></td>
    </tr>
    <tr>
        <td>
            <table width="100%" cellpadding="2" cellspacing="2" border="0">
                <tr>
                    <td width="50%" valign="top">
                        <table cellpadding="2" cellspacing="2" border="0" width="100%">
                            <tr>
                                <td nowrap>Estimated Start Date:</td>
                                <td width="100%"><asp:TextBox ID="txtStart" runat="server" CssClass="default" Width="100" MaxLength="10" /> <asp:ImageButton ID="imgStart" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
                            </tr>
                            <tr>
                                <td nowrap>Investigated: </td>
                                <td width="100%">
                                    <asp:DropDownList ID="drpInvestigated" runat="server" CssClass="default" Width="200">
                                        <asp:ListItem Selected="True">-- SELECT --</asp:ListItem>
                                        <asp:ListItem>Not Started</asp:ListItem>
                                        <asp:ListItem>In Progress</asp:ListItem>
                                        <asp:ListItem>Complete</asp:ListItem>
                                        <asp:ListItem>2008 Initiative</asp:ListItem>
                                        <asp:ListItem>Hold For Start Date</asp:ListItem>
                                    </asp:DropDownList>                            
                                </td>
                            </tr>
                            <tr>
                                <td nowrap>Follow-Up Date:</td>
                                <td width="100%">
                                    <asp:TextBox ID="txtFollowupDate" runat="server" CssClass="default"></asp:TextBox>
                                    <asp:ImageButton ID="imgFollowUpDate" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" />
                                 
                                </td>
                            </tr>
                            <tr>
                                <td nowrap>Phase Engaged:</td>
                                <td width="100%">
                                    <asp:DropDownList ID="drpPhase" runat="server" CssClass="default" Width="200">
                                        <asp:ListItem Selected="true">-- SELECT --</asp:ListItem>
                                        <asp:ListItem>Idea</asp:ListItem>
                                        <asp:ListItem>Discovery</asp:ListItem>
                                        <asp:ListItem>Analysis</asp:ListItem>
                                        <asp:ListItem>Design</asp:ListItem>
                                        <asp:ListItem>Build</asp:ListItem>
                                        <asp:ListItem>Test</asp:ListItem>
                                        <asp:ListItem>Deploy</asp:ListItem>
                                    </asp:DropDownList>                           
                                </td>
                            </tr>
                            <tr>
                                <td nowrap>Involvement:</td>
                                <td width="100%">
                                    <asp:DropDownList ID="drpInvolvement" runat="server" CssClass="default" Width="200">
                                        <asp:ListItem Selected="true">-- SELECT --</asp:ListItem>
                                        <asp:ListItem>Yes</asp:ListItem>
                                        <asp:ListItem>No</asp:ListItem>
                                        <asp:ListItem>TBD</asp:ListItem>
                                    </asp:DropDownList>                            
                                </td>
                            </tr>
                            <tr>
                                <td nowrap>Project Class:</td>
                                <td width="100%">
                                    <asp:DropDownList ID="drpProjectClass" runat="server" CssClass="default" Width="200">
                                        <asp:ListItem Selected="true">-- SELECT --</asp:ListItem>
                                        <asp:ListItem>Simple</asp:ListItem>
                                        <asp:ListItem>Complex</asp:ListItem>
                                        <asp:ListItem>TBD</asp:ListItem>
                                    </asp:DropDownList>                            
                                </td>
                            </tr>
                            <tr>
                                <td nowrap>IDC SPOC:</td>
                                <td width="100%">
                                    <table cellpadding="0" cellspacing="0" border="0">
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtIDCSPOC" runat="server" Width="200" CssClass="default" />                                        
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div id="divIDCSPOC" runat="server" style="overflow: hidden; position: absolute; display: none; background-color: #FFFFFF; border: solid 1px #CCCCCC">
                                                    <asp:ListBox ID="lstIDCSPOC" runat="server" CssClass="default" />
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td width="50%" valign="top">
                        <table cellpadding="2" cellspacing="2" border="0" width="100%">
                            <tr>
                                <td nowrap>Estimated End Date:</td>
                                <td width="100%"><asp:TextBox ID="txtEnd" runat="server" CssClass="default" Width="100" MaxLength="10" /> <asp:ImageButton ID="imgEnd" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" /></td>
                            </tr>
                            <tr>
                                <td nowrap>Investigated By:</td>
                                <td width="100%">
                                    <table cellpadding="0" cellspacing="0" border="0">
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtInvestigatedBy" runat="server" Width="200" CssClass="default" />                                        
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div id="divAJAX" runat="server" style="overflow: hidden; position: absolute; display: none; background-color: #FFFFFF; border: solid 1px #CCCCCC">
                                                    <asp:ListBox ID="lstAJAX" runat="server" CssClass="default" />
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td nowrap>Date Engaged:</td>
                                <td width="100%">
                                    <asp:TextBox ID="txtDateEngaged" runat="server" CssClass="default"></asp:TextBox>
                                    <asp:ImageButton ID="imgDate" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" />                            
                                </td>
                            </tr>
                            <tr>
                                <td nowrap>Size of Effort:</td>
                                <td width="100%">
                                    <asp:DropDownList ID="drpEffortSize" runat="server" CssClass="default" Width="200">
                                        <asp:ListItem Selected="true">-- SELECT --</asp:ListItem>
                                        <asp:ListItem>Small</asp:ListItem>
                                        <asp:ListItem>Medium</asp:ListItem>
                                        <asp:ListItem>Large</asp:ListItem>
                                    </asp:DropDownList>                           
                                </td>
                            </tr>
                            <tr>
                                <td nowrap>EIT Testing:</td>
                                <td width="100%">
                                    <asp:DropDownList ID="drpEIT" runat="server" CssClass="default" Width="200">
                                        <asp:ListItem Selected="true">-- SELECT --</asp:ListItem>
                                        <asp:ListItem>Yes</asp:ListItem>
                                        <asp:ListItem>No</asp:ListItem>
                                        <asp:ListItem>TBD</asp:ListItem>
                                    </asp:DropDownList>                            
                                </td>
                            </tr>
                            <tr>
                                <td nowrap>Enterprise Release:</td>
                                <td width="100%">
                                    <asp:DropDownList ID="drpEnterprise" runat="server" CssClass="default" Width="200">
                                        <asp:ListItem Selected="true">-- SELECT --</asp:ListItem>
                                        <asp:ListItem>Yes</asp:ListItem>
                                        <asp:ListItem>No</asp:ListItem>
                                        <asp:ListItem>TBD</asp:ListItem>
                                    </asp:DropDownList>                            
                                </td>
                            </tr>
                            <tr id="divInvolvement" runat="server" style="display:none">
                                <td nowrap>No Involve Reason:</td>
                                <td width="100%">
                                    <asp:DropDownList ID="drpNoInvolve" runat="server" CssClass="default" Width="200">
                                        <asp:ListItem Selected="true">-- SELECT --</asp:ListItem>
                                        <asp:ListItem>Production Support</asp:ListItem>
                                        <asp:ListItem>No Requirements</asp:ListItem>
                                    </asp:DropDownList>                            
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">Comments:</td>
                </tr>
                <tr>
                    <td width="100%" colspan="2">
                        <asp:TextBox ID="txtComment" runat="server" TextMode="MultiLine" Width="80%" Rows="8"
                            CssClass="default"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="2" class="greentableheader">Resource Assignments</td>
                </tr>
                <tr>
                    <td colspan="2">
                        <table id="tblResource" width="100%" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                            <tr bgcolor="#eeeeee">
                                <td nowrap style="text-decoration: underline">
                                    <b>Service Type</b></td>
                                <td nowrap style="text-decoration: underline">
                                    <b>Requested By</b></td>
                                <td style="text-decoration: underline" align="center">
                                    <b>Date Requested</b></td>
                                <td style="text-decoration: underline" align="center">
                                    <b>Fulfillment Date</b></td>
                                <td style="text-decoration: underline">
                                    <b>Resource Assigned</b></td>
                                <td nowrap style="text-decoration: underline">
                                    <b>Status</b></td>
                                <td nowrap>
                                </td>
                            </tr>
                            <asp:Repeater ID="rptResource" runat="server" EnableViewState="true">
                                <ItemTemplate>
                                    <tr>
                                        <asp:Label ID="lblID" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem,"id") %>'></asp:Label>
                                        <asp:Label ID="lblResID" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem,"resourcetypeid") %>'></asp:Label>
                                        <td valign="top">
                                            <asp:Label ID="lblResourceType" runat="server"></asp:Label></td>
                                        <td valign="top">
                                            <asp:Label ID="lblRequestedBy" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"requestedby") %>'></asp:Label></td>
                                        <td nowrap valign="top" align="center">
                                            <asp:Label ID="lblRequestedDate" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"requesteddate") %>'></asp:Label></td>
                                        <td nowrap valign="top" align="center">
                                            <asp:Label ID="lblFulfillDate" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"fulfilldate") %>'></asp:Label></td>
                                        <td valign="top">
                                            <asp:Label ID="lblResourceAssigned" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"resourceassigned") %>'></asp:Label></td>
                                        <td valign="top">
                                            <asp:Label ID="lblStatus" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"status") %>'></asp:Label></td>
                                        <td align="right">
                                            <asp:Panel ID="panEditable" runat="server" Visible="false">
                                                [<a href="javascript:void(0);" onclick="EditResource('<%#DataBinder.Eval(Container.DataItem,"requestid") %>','<%#DataBinder.Eval(Container.DataItem,"itemid") %>','<%#DataBinder.Eval(Container.DataItem,"number") %>','<%#DataBinder.Eval(Container.DataItem,"id") %>');">Edit</a>]&nbsp;&nbsp;[<asp:LinkButton
                                                    ID="btnDelete" runat="server" OnClick="btnDelete_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>'
                                                    Text="Delete" />]
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                            <tr>
                                <td colspan="7">
                                    <asp:Label ID="lblNoRes" runat="server" CssClass="default" Visible="false" Text="<img src='/images/alert.gif' border='0' align='absmiddle'> There are 0 items..." />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" class="greentableheader"><asp:Button ID="btnAddRes" runat="server" Text="Add" CssClass="default" Width="100" /></td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="2" class="greentableheader">Technology Assets</td>
                </tr>
                <tr>
                    <td colspan="2">
                        <table id="tblAsset" width="100%" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                            <tr bgcolor="#eeeeee">
                                <td nowrap style="text-decoration: underline">
                                    <b>Asset Type</b></td>
                                <td nowrap style="text-decoration: underline">
                                    <b>Sale Status</b></td>
                                <td style="text-decoration: underline">
                                    <b>Last Modified By</b></td>
                                <td nowrap style="text-decoration: underline">
                                    <b>Last Updated</b></td>
                                <td nowrap>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Repeater ID="rptAssets" runat="server" EnableViewState="true">
                                        <ItemTemplate>
                                            <tr>
                                                <asp:Label ID="lblID" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem,"id") %>'></asp:Label>
                                                <asp:Label ID="lblAssetID" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem,"asset_typeid") %>'></asp:Label>
                                                <td valign="top">
                                                    <asp:Label ID="lblAssetType" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"assetname") %>'></asp:Label></td>
                                                <td valign="top">
                                                    <asp:Label ID="lblSaleStatus" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"salestatus") %>'></asp:Label></td>
                                                <td valign="top">
                                                    <asp:Label ID="lblModified" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"lastmodified") %>'></asp:Label></td>
                                                <td valign="top">
                                                    <asp:Label ID="lblUpdated" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"modified") %>'></asp:Label></td>
                                                <td align="right">
                                                    <asp:Panel ID="panEditable" runat="server" Visible="false">
                                                        [<a href="javascript:void(0);" onclick="EditAsset('<%#DataBinder.Eval(Container.DataItem,"requestid") %>','<%#DataBinder.Eval(Container.DataItem,"itemid") %>','<%#DataBinder.Eval(Container.DataItem,"number") %>','<%#DataBinder.Eval(Container.DataItem,"id") %>');">Edit</a>]&nbsp;&nbsp;[<asp:LinkButton
                                                            ID="btnDelete" runat="server" OnClick="btnDelete_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>'
                                                            CommandName="Asset" Text="Delete" />]
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Label ID="lblNoAsset" runat="server" CssClass="default" Visible="false" Text="<img src='/images/alert.gif' border='0' align='absmiddle'> There are 0 items..." />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" class="greentableheader"><asp:Button ID="btnAddAsset" runat="server" Text="Add" CssClass="default" Width="100" /></td>
                </tr>
            </table>               
        </td>
    </tr>
    <tr>
        <td><hr size="1" noshade /></td>
    </tr>
    <tr>
        <td>
            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td>
                        <table cellpadding="4" cellspacing="4" border="0">
                            <tr>
                                <td><asp:Button ID="btnBack" runat="server" OnClick="btnBack_Click" CssClass="default" Text="<<  Back" Width="100" /></td>
                                <td>
                                    <asp:Panel ID="panDeliverable" runat="server" Visible="false">
                                        <asp:Button ID="btnDeliverable" runat="server" CssClass="default" Text="Service Deliverables" Width="150" />
                                    </asp:Panel>
                                </td>
                                <td><asp:Button ID="btnNext" runat="server" OnClick="btnNext_Click" CssClass="default" Text="Next  >>" Width="100" /></td>
                            </tr>
                        </table>
                    </td>
                    <td align="right">
                        <table cellpadding="4" cellspacing="4" border="0">
                            <tr>
                                <td><asp:Button ID="btnCancel1" runat="server" OnClick="btnCancel_Click" CssClass="default" Text="Cancel Request" Width="125" /></td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
</asp:Panel>
<asp:Label ID="lblItem" runat="server" Visible="false" />
<asp:Label ID="lblNumber" runat="server" Visible="false" />
<asp:Label ID="lblService" runat="server" Visible="false" />
<asp:Label ID="lblPRequest" runat="server" Visible="false" />
<asp:HiddenField ID="hdnWorking" runat="server" />
<asp:HiddenField ID="hdnExecutive" runat="server" />
<asp:HiddenField ID="hdnInvestigatedBy" runat="server" />
<asp:HiddenField ID="hdnIDCSPOC" runat="server" />
