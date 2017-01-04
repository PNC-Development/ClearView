<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wm_ie.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.wm_ie" %>



<script type="text/javascript">
    function ShowInvolvementReason(oDDL, oDiv) {
        oDiv = document.getElementById(oDiv);
        if (oDDL.options[oDDL.selectedIndex].value == "No")
            oDiv.style.display = "inline";
        else
            oDiv.style.display = "none";
    }
    // My Javascript start - Vijay
    function EditAsset(strRequest,strItem,strNumber,strID)
    {
       return OpenWindow('IDCASSET_TYPE','?rid=' + strRequest + '&iid=' + strItem + '&num=' + strNumber + '&id=' + strID);
    }
    
    function EditResource(strRequest,strItem,strNumber,strID)
    {
       return OpenWindow('IDCRESOURCE_ASSIGN','?rid=' + strRequest + '&iid=' + strItem + '&num=' + strNumber + '&id=' + strID);
    }
    
    // My javascript end - Vijay
    function refreshIE() {
        document.forms[0].submit();
    }
  
</script>
<asp:Panel ID="panWorkload" runat="server" Visible="false">
    <table width="100%" border="0" cellSpacing="0" cellPadding="0" class="default">
        <tr>
            <td>
                <table width="100%" border="0" cellSpacing="0" cellPadding="0">
                    <tr>
                        <td><asp:ImageButton ID="btnSave" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/tool_save.gif" OnClick="btnSave_Click" /></td>
                        <td><asp:ImageButton ID="btnPrint" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/tool_print.gif" /></td>
                        <td><asp:ImageButton ID="btnComplete" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/tool_complete.gif" OnClick="btnComplete_Click" /></td>
                        <td width="100%">
                            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                <tr>
                                    <td><img src="/images/tool_left.gif" border="0" width="6" height="26"></td>
                                    <td nowrap background="/images/tool_back.gif" width="100%"><img src="/images/tool_back.gif" border="0" width="2" height="26"></td>
                                    <td><img src="/images/tool_right.gif" border="0" width="6" height="26"></td>
                                </tr>
                            </table>
                        </td>
                        <td><asp:ImageButton ID="btnClose" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/tool_close.gif" /></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="default">
                <table width="100%" border="0" cellSpacing="2" cellPadding="2" class="default">
                    <tr>
                        <td valign="top">
                            <table border="0" cellpadding="3" cellspacing="2">
                                <tr>
                                    <td nowrap><b>Name:</b></td>
                                    <td><asp:Label ID="lblName" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td nowrap><b>Number:</b></td>
                                    <td><asp:Label ID="lblNumber" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td nowrap><b>Type:</b></td>
                                    <td><asp:Label ID="lblType" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td nowrap><b>Service:</b></td>
                                    <td><asp:Label ID="lblService" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td nowrap><b>Last Updated:</b></td>
                                    <td><asp:Label ID="lblUpdated" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td nowrap><b>Overall Status:</b></td>
                                    <td><asp:Label ID="lblStatus" runat="server" CssClass="default" /></td>
                                </tr>
                                <tr>
                                    <td nowrap><b>Custom Task Name:</b></td>
                                    <td><asp:TextBox ID="txtCustom" runat="server" CssClass="default" Width="300" MaxLength="100" /></td>
                                </tr>
                            </table>
                        </td>
                        <td valign="top" align="right"><iframe src="/frame/did_you_know.aspx" frameborder="0" scrolling="no" width="300" height="125"></iframe></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr height="16">
            <td><img src="/images/spacer.gif" border="0" height="16" width="1" /></td>
        </tr>
        <tr>
            <td>
                <table cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <%=strTabs %>
                        <td width="100%" background="/images/TabEmptyBackground.gif">&nbsp;</td>
                    </tr>
                </table>

                    <table width="100%" height="100%" border="0" cellSpacing="0" cellPadding="0" class="default">
                        <tr>
                            <td colspan="7" align="center" class="cmcontents">
                                <table width="100%" height="100%" border="0" cellSpacing="2" cellPadding="2" class="default">
                                    <tr> 
                                        <td valign="top">
		                                    <div id="divTab1" style='<%=boolDetails == true ? "display:inline" : "display:none" %>'>
		                                        <br />
                                                <table width="100%" cellpadding="0" cellspacing="5" border="0">
                                                    <tr>
                                                        <td rowspan="2" valign="top"><img src="/images/details.gif" border="0" align="absmiddle" /></td>
                                                        <td class="hugeheader" width="100%" valign="bottom">Project Details</td>
                                                    </tr>
                                                    <tr>
                                                        <td width="100%" valign="top">Here are the details of this project.</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" background="/images/lines.gif">&nbsp;</td>
                                                    </tr>
                                                </table><br />
                                                <table width="100%" cellpadding="4" cellspacing="3" border="0">
                                                    <tr>
                                                        <td nowrap>Project Phase:</td>
                                                        <td width="100%"><asp:Label ID="lblDetPhase" runat="server" CssClass="default" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap>Project Status:</td>
                                                        <td width="100%"><asp:Label ID="lblDetStatus" runat="server" CssClass="default" /></td>
                                                    </tr>
<asp:Panel id="panFinancials" runat="server" Visible="false">
                                                    <tr>
                                                        <td nowrap>Financial Status:</td>
                                                        <td width="100%"><asp:Label ID="lblDetFinancials" runat="server" CssClass="default" /></td>
                                                    </tr>
</asp:Panel>
                                                    <tr>
                                                        <td nowrap>Project Manager:</td>
                                                        <td width="100%"><asp:Label ID="lblDetManager" runat="server" CssClass="default" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap>Project Description:</td>
                                                        <td width="100%"><asp:Label ID="lblDetDesc" runat="server" CssClass="default" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap>Project Start Date:</td>
                                                        <td width="100%"><asp:Label ID="lblDetStart" runat="server" CssClass="default" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap>Project End Date:</td>
                                                        <td width="100%"><asp:Label ID="lblDetEnd" runat="server" CssClass="default" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td nowrap>Location of URL:</td>
                                                        <td width="100%"><asp:Label ID="lblDetURL" runat="server" CssClass="default" /></td>
                                                    </tr>
                                                </table>
                                                <p>&nbsp;</p>
		                                    </div>

		                                    <div id="divTab3" style='<%=boolDesigns == true ? "display:inline" : "display:none" %>'>
		                                        <br />
                                                <table width="100%" cellpadding="0" cellspacing="5" border="0">
                                                    <tr>
                                                        <td rowspan="2" valign="top"><img src="/images/forecast2.gif" border="0" align="absmiddle" /></td>
                                                        <td class="hugeheader" width="100%" valign="bottom">Designs</td>
                                                    </tr>
                                                    <tr>
                                                        <td width="100%" valign="top">Design builder is used to whiteboard project hardware and software requirements.</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" background="/images/lines.gif">&nbsp;</td>
                                                    </tr>
                                                </table><br />

                                                <table cellpadding="4" cellspacing="0" border="0">
                                                    <tr>
                                                        <td class="default"><b>View:</b></td>
                                                        <td class="default"><asp:Label ID="lblFilter" runat="server" CssClass="default" /></td>
                                                        <td><img src="/images/spacer.gif" border="0" width="5" height="1" /></td>
                                                        <td>[<asp:LinkButton ID="btnFilter" runat="server" Text="Filter" />]</td>
                                                        <td>[<asp:LinkButton ID="btnClear" runat="server" Text="Show All" OnClick="btnClear_Click" Enabled="false" />]</td>
                                                    </tr>
                                                </table>
                                                <br />
                                                <table width="100%" cellpadding="4" cellspacing="0" border="0" align="center"  style="border:solid 1px #CCCCCC">
                                                    <tr bgcolor="#EEEEEE">
                                                        <td valign="bottom"><b><u>Name:</u></b></td>
                                                        <td valign="bottom" align="center"><b><u>Platform:</u></b></td>
                                                        <td valign="bottom" nowrap><b><u>Model:</u></b></td>
                                                        <td valign="bottom" align="center"><b><u>Commitment<br />Date:</u></b></td>
                                                        <td valign="bottom" align="center"><b><u>QTY:</u></b></td>
                                                        <td valign="bottom" align="center"><b><u>Acquisition<br />Costs:</u></b></td>
                                                        <!--
                                                        <td valign="bottom" align="center"><b><u>Operational<br />Costs:</u></b></td>
                                                        -->
                                                        <td valign="bottom" align="center"><b><u>Storage:</u></b></td>
                                                        <td valign="bottom" align="center"><b><u>Backup:</u></b></td>
                                                        <!--
                                                        <td valign="bottom" align="center"><b><u>AMP:</u></b></td>
                                                        <td valign="bottom" align="center"><b><u>HRs:</u></b></td>
                                                        -->
                                                        <td valign="bottom" align="center"><b><u>Confidence:</u></b></td>
                                                        <td valign="bottom" align="center"><b><u>% Done:</u></b></td>
                                                        <td width="1"></td>
                                                    </tr>
                                                    <asp:repeater ID="rptAll" runat="server">
                                                        <ItemTemplate>
                                                            <tr>
                                                                <asp:Label ID="lblId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "id") %>' />
                                                                <asp:Label ID="lblPlatformId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "platformid") %>' />
                                                                <asp:Label ID="lblRecovery" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "recovery_number") %>' />
                                                                <asp:Label ID="lblRequestId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "requestid") %>' />
                                                                <td valign="top"><%# (DataBinder.Eval(Container.DataItem, "name").ToString() == "" ? "---" : DataBinder.Eval(Container.DataItem, "name").ToString()) %></td>
                                                                <td valign="top" align="center"><asp:Label ID="lblPlatform" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "platform") %>' /></td>
                                                                <td valign="top" nowrap><asp:Label ID="lblModel" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "modelid") %>' /></td>
                                                                <td valign="top" align="center" nowrap><asp:Label ID="lblCommitment" runat="server" CssClass="default" Text='<%# (DataBinder.Eval(Container.DataItem, "implementation").ToString() == "" ? "" : DateTime.Parse(DataBinder.Eval(Container.DataItem, "implementation").ToString()).ToShortDateString()) %>' /></td>
                                                                <td valign="top" align="center" nowrap><asp:Label ID="lblQuantity" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "quantity") %>' /></td></td>
                                                                <td valign="top" align="right" nowrap><asp:Label ID="lblAcquisition" runat="server" CssClass="default" /><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                                                                <!--
                                                                <td valign="top" align="right" nowrap><asp:Label ID="lblOperational" runat="server" CssClass="default" /><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                                                                -->
                                                                <td valign="top" align="right" nowrap><asp:Label ID="lblStorage" runat="server" CssClass="default" /><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                                                                <td valign="top" align="center" nowrap><a href="javascript:void(0);" onclick="OpenWindow('FORECAST_BACKUP','<%# DataBinder.Eval(Container.DataItem, "id") %>');"><img src="/images/backup.gif" border="0" align="absmiddle" /></a></td>
                                                                <!--
                                                                <td valign="top" align="right" nowrap><asp:Label ID="lblAmp" runat="server" CssClass="default" /><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                                                                <td valign="top" align="right" nowrap><asp:Label ID="lblHours" runat="server" CssClass="default" /><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                                                                -->
                                                                <td valign="top" align="center"><%# DataBinder.Eval(Container.DataItem, "confidence") %></td>
                                                                <td valign="top" align="center" nowrap><asp:Label ID="lblStep" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "step") %>' /></td>
                                                                <td valign="top" align="right" width="1">
                                                                    <asp:Panel ID="panExecute" runat="server" Visible="false">
                                                                        [<a href="javascript:void(0);" onclick="EditForecast('<%# DataBinder.Eval(Container.DataItem, "id") %>');">Edit</a>]&nbsp;&nbsp;[<asp:LinkButton ID="btnDelete" runat="server" OnClick="btnDelete_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' Text="Delete" />]&nbsp;&nbsp;[<asp:LinkButton ID="btnExecute" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' Text="Execute" />]
                                                                    </asp:Panel>
                                                                    <asp:Panel ID="panComplete" runat="server" Visible="false">
                                                                        <img src="/images/check_small.gif" border="0" align="absmiddle" />&nbsp;<asp:Label ID="lblComplete" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "completed") %>' />
                                                                    </asp:Panel>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                        <AlternatingItemTemplate>
                                                            <tr bgcolor="F6F6F6">
                                                                <asp:Label ID="lblId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "id") %>' />
                                                                <asp:Label ID="lblPlatformId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "platformid") %>' />
                                                                <asp:Label ID="lblRecovery" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "recovery_number") %>' />
                                                                <asp:Label ID="lblRequestId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "requestid") %>' />
                                                                <td valign="top"><%# (DataBinder.Eval(Container.DataItem, "name").ToString() == "" ? "---" : DataBinder.Eval(Container.DataItem, "name").ToString()) %></td>
                                                                <td valign="top" align="center"><asp:Label ID="lblPlatform" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "platform") %>' /></td>
                                                                <td valign="top" nowrap><asp:Label ID="lblModel" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "modelid") %>' /></td>
                                                                <td valign="top" align="center" nowrap><asp:Label ID="lblCommitment" runat="server" CssClass="default" Text='<%# (DataBinder.Eval(Container.DataItem, "implementation").ToString() == "" ? "" : DateTime.Parse(DataBinder.Eval(Container.DataItem, "implementation").ToString()).ToShortDateString()) %>' /></td>
                                                                <td valign="top" align="center" nowrap><asp:Label ID="lblQuantity" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "quantity") %>' /></td>
                                                                <td valign="top" align="right" nowrap><asp:Label ID="lblAcquisition" runat="server" CssClass="default" /><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                                                                <!--
                                                                <td valign="top" align="right" nowrap><asp:Label ID="lblOperational" runat="server" CssClass="default" /><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                                                                -->
                                                                <td valign="top" align="right" nowrap><asp:Label ID="lblStorage" runat="server" CssClass="default" /><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                                                                <td valign="top" align="center" nowrap><a href="javascript:void(0);" onclick="OpenWindow('FORECAST_BACKUP','<%# DataBinder.Eval(Container.DataItem, "id") %>');"><img src="/images/backup.gif" border="0" align="absmiddle" /></a></td>
                                                                <!--
                                                                <td valign="top" align="right" nowrap><asp:Label ID="lblAmp" runat="server" CssClass="default" /><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                                                                <td valign="top" align="right" nowrap><asp:Label ID="lblHours" runat="server" CssClass="default" /><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                                                                -->
                                                                <td valign="top" align="center"><%# DataBinder.Eval(Container.DataItem, "confidence") %></td>
                                                                <td valign="top" align="center" nowrap><asp:Label ID="lblStep" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "step") %>' /></td>
                                                                <td valign="top" align="right" width="1">
                                                                    <asp:Panel ID="panExecute" runat="server" Visible="false">
                                                                        [<a href="javascript:void(0);" onclick="EditForecast('<%# DataBinder.Eval(Container.DataItem, "id") %>');">Edit</a>]&nbsp;&nbsp;[<asp:LinkButton ID="btnDelete" runat="server" OnClick="btnDelete_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' Text="Delete" />]&nbsp;&nbsp;[<asp:LinkButton ID="btnExecute" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' Text="Execute" />]
                                                                    </asp:Panel>
                                                                    <asp:Panel ID="panComplete" runat="server" Visible="false">
                                                                        <img src="/images/check_small.gif" border="0" align="absmiddle" />&nbsp;<asp:Label ID="lblComplete" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "completed") %>' />
                                                                    </asp:Panel>
                                                                </td>
                                                            </tr>
                                                        </AlternatingItemTemplate>
                                                    </asp:repeater>
                                                    <tr>
                                                        <td colspan="12">
                                                            <asp:Label ID="lblNone" runat="server" CssClass="default" Visible="false" Text="<img src='/images/spacer.gif' border='0' height='1' width='25' /><img src='/images/alert.gif' border='0' align='absmiddle'> You have not added any equipment to this forecast" />
                                                        </td>
                                                    </tr>
                                                    <tr bgcolor="#EEEEEE">
                                                        <td valign="top"></td>
                                                        <td valign="top"></td>
                                                        <td valign="top"></td>
                                                        <td valign="top"></td>
                                                        <td valign="top" align="center"><b><asp:Label ID="lblQuantityTotal" runat="server" CssClass="default" /></b></td>
                                                        <td valign="top" align="right"><b><asp:Label ID="lblAcquisitionTotal" runat="server" CssClass="default" /></b><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                                                        <!--
                                                        <td valign="top" align="right"><b><asp:Label ID="lblOperationalTotal" runat="server" CssClass="default" /></b><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                                                        -->
                                                        <td valign="top" align="right"><b><asp:Label ID="lblStorageTotal" runat="server" CssClass="default" /></b><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                                                        <td valign="top"></td>
                                                        <!--
                                                        <td valign="top" align="right"><b><asp:Label ID="lblAmpTotal" runat="server" CssClass="default" /></b><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                                                        <td valign="top" align="right"><b><asp:Label ID="lblHoursTotal" runat="server" CssClass="default" /></b><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
                                                        -->
                                                        <td valign="top"></td>
                                                        <td valign="top"></td>
                                                        <td valign="top"></td>
                                                    </tr>
                                                </table>
                                                <table width="100%" cellpadding="4" cellspacing="3" border="0">
                                                    <tr height="1">
                                                        <td colspan="2"><img src="/images/spacer.gif" border="0" width="1" height="1" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                                                <tr>
                                                                    <td><asp:Button ID="btnNew" runat="Server" CssClass="default" Width="125" Text="Design Environment" /> <asp:Button ID="Button1" runat="Server" CssClass="default" Width="125" Text="Print Design" /></td>
                                                                    <td align="right"><asp:Button ID="btnDeleteForecast" runat="Server" CssClass="default" Width="125" OnClick="btnDeleteForecast_Click" Text="Delete Design" Enabled="false" /></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>

                                                <p>&nbsp;</p>
		                                    </div>
		                                   
		                                    <div id="divTab4" style='<%=boolExecution == true ? "display:inline" : "display:none" %>'>
		                                        <br />
                                                <table width="100%" cellpadding="0" cellspacing="5" border="0">
                                                    <tr>
                                                        <td rowspan="2" valign="top"><img src="/images/ico_check.gif" border="0" align="absmiddle" /></td>
                                                        <td class="hugeheader" width="100%" valign="bottom">Weekly Status</td>
                                                    </tr>
                                                    <tr>
                                                        <td width="100%" valign="top">Update your progress on this request by moving the slider and adding weekly status updates.</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" background="/images/lines.gif">&nbsp;</td>
                                                    </tr>
                                                </table><br />
                                                <table width="100%" cellpadding="4" cellspacing="3" border="0">
                                                    <tr>
                                                        <td colspan="2">
                                                <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                                    <tr>
                                                        <td width="50%" valign="top">
                                                            <table width="100%" cellpadding="4" cellspacing="3" border="0">
                                                                <asp:Panel ID="panSlider" runat="server" Visible="false">
                                                                <tr>
                                                                    <td nowrap>Completed (%):</td>
                                                                    <td width="100%"><SliderCC:Slider ID="sldHours" _ParentElement="divTab4" _Hidden="hdnHours" runat="server" _DivClass="default" _PercentClass="result" _Width="300" _ShowDiv="true" _RestrictLess="true" /> <asp:Label ID="lblHours" runat="server" CssClass="required" Visible="false" Text="No hours have been allocated for this initiative" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2">&nbsp;</td>
                                                                </tr>
                                                                </asp:Panel>
                                                                <tr>
                                                                    <td nowrap>Status Updates:</td>
                                                                    <td width="100%">
                                                                        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="default" >
                                                                            <asp:ListItem Text="-- SELECT --" Value="0" />
                                                                            <asp:ListItem Text="Red" Value="1" />
                                                                            <asp:ListItem Text="Yellow" Value="2" />
                                                                            <asp:ListItem Text="Green" Value="3" />
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2"><b>Comments / Issues:</b></td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2"><asp:TextBox ID="txtComments" runat="server" CssClass="default" Width="90%" TextMode="MultiLine" Rows="13" /></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td width="50%" valign="top">
                                                            <table width="100%" cellpadding="4" cellspacing="3" border="0">
                                                                <tr>
                                                                    <td colspan="2"><b>Deliverable Completeness:</b></td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2"><img src="/images/spacer.gif" border="0" height="1" width="1" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td nowrap>Statement of Work (%):</td>
                                                                    <td width="100%"><SliderCC:Slider ID="sldSlide1" _ParentElement="divTab4" _Hidden="hdnSlide1" runat="server" _DivClass="default" _PercentClass="result" _Width="300" _ShowDiv="true" _RestrictLess="false" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2"><img src="/images/spacer.gif" border="0" height="1" width="1" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td nowrap>Solution Alternatives (%):</td>
                                                                    <td width="100%"><SliderCC:Slider ID="sldSlide2" _ParentElement="divTab4" _Hidden="hdnSlide2" runat="server" _DivClass="default" _PercentClass="result" _Width="300" _ShowDiv="true" _RestrictLess="false" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2"><img src="/images/spacer.gif" border="0" height="1" width="1" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td nowrap>Solution Recommendation (%):</td>
                                                                    <td width="100%"><SliderCC:Slider ID="sldSlide3" _ParentElement="divTab4" _Hidden="hdnSlide3" runat="server" _DivClass="default" _PercentClass="result" _Width="300" _ShowDiv="true" _RestrictLess="false" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2"><img src="/images/spacer.gif" border="0" height="1" width="1" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td nowrap>High Level Design (%):</td>
                                                                    <td width="100%"><SliderCC:Slider ID="sldSlide4" _ParentElement="divTab4" _Hidden="hdnSlide4" runat="server" _DivClass="default" _PercentClass="result" _Width="300" _ShowDiv="true" _RestrictLess="false" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2"><img src="/images/spacer.gif" border="0" height="1" width="1" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td nowrap>Detailed Design (%):</td>
                                                                    <td width="100%"><SliderCC:Slider ID="sldSlide5" _ParentElement="divTab4" _Hidden="hdnSlide5" runat="server" _DivClass="default" _PercentClass="result" _Width="300" _ShowDiv="true" _RestrictLess="false" /></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <table width="100%" cellpadding="3" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                                                                <tr bgcolor="#EEEEEE">
                                                                    <td nowrap><b>Date</b></td>
                                                                    <td nowrap>&nbsp;</td>
                                                                    <td nowrap><b>Status</b></td>
                                                                    <td nowrap>&nbsp;</td>
                                                                    <td nowrap><b>Comments</b></td>
                                                                </tr>
                                                                <asp:repeater ID="rptStatus" runat="server">
                                                                    <ItemTemplate>
                                                                        <tr class="default">
                                                                            <td nowrap><a href="javascript:void(0);" onclick="ShowStatus('<%# DataBinder.Eval(Container.DataItem, "id") %>','r');"><%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "modified").ToString()).ToShortDateString() %>&nbsp;<%# DateTime.Parse(DataBinder.Eval(Container.DataItem, "modified").ToString()).ToShortTimeString() %></a></td>
                                                                            <td nowrap>&nbsp;</td>
                                                                            <td nowrap><asp:Label ID="lblStatus" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "status") %>' /></td>
                                                                            <td nowrap>&nbsp;</td>
                                                                            <td width="100%"><asp:Label ID="lblComments" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "comments") %>' /></td>
                                                                        </tr>
                                                                    </ItemTemplate>
                                                                </asp:repeater>
                                                            <tr>
                                                                <td colspan="5">
                                                                    <asp:Label ID="lblNoStatus" runat="server" CssClass="default" Visible="false" Text="<img src='/images/alert.gif' border='0' align='absmiddle'> There are no updates" />
                                                                </td>
                                                            </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
		                                    </div>	
		                                    
		                                    <div id="divTab2" style='<%=boolIDC == true ? "display:inline" : "display:none" %>'>
		                                        <br />
                                                <table width="100%" cellpadding="0" cellspacing="5" border="0">
                                                    <tr>
                                                        <td rowspan="2" valign="top"><img src="/images/server.gif" border="0" align="absmiddle" /></td>
                                                        <td class="hugeheader" width="100%" valign="bottom">Investigation</td>
                                                    </tr>
                                                    <tr>
                                                        <td width="100%" valign="top">The IDC database has been imported into ClearView. Use this form to update the IDC database.</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" background="/images/lines.gif">&nbsp;</td>
                                                    </tr>
                                                </table><br />
                                                <table width="100%" cellpadding="4" cellspacing="3" border="0">
                                                    <tr>
                                                        <td>
                                                            <!-- VIJAY - Develop Code -->
                                                            <table width="100%" cellpadding="2" cellspacing="2" border="0">
                                                                <tr>
                                                                    <td width="50%" valign="top">
                                                                        <table cellpadding="2" cellspacing="2" border="0" width="100%">
                                                                            <tr>
                                                                                <td class="bold" nowrap>Investigated: </td>
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
                                                                                <td class="bold" nowrap>Follow-Up Date:</td>
                                                                                <td width="100%">
                                                                                    <asp:TextBox ID="txtFollowupDate" runat="server" CssClass="default"></asp:TextBox>
                                                                                    <asp:ImageButton ID="imgFollowUpDate" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" />
                                                                                 
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="bold" nowrap>Phase Engaged:</td>
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
                                                                                <td class="bold" nowrap>Involvement:</td>
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
                                                                                <td class="bold" nowrap>Project Class:</td>
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
                                                                                <td class="bold" nowrap>IDC SPOC:</td>
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
                                                                                <td class="bold" nowrap>Investigated By:</td>
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
                                                                                <td class="bold" nowrap>Date Engaged:</td>
                                                                                <td width="100%">
                                                                                    <asp:TextBox ID="txtDateEngaged" runat="server" CssClass="default"></asp:TextBox>
                                                                                    <asp:ImageButton ID="imgDate" runat="server" ImageAlign="AbsMiddle" ImageUrl="/images/calendar.gif" />                            
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="bold" nowrap>Size of Effort:</td>
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
                                                                                <td class="bold" nowrap>EIT Testing:</td>
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
                                                                                <td class="bold" nowrap>Enterprise Release:</td>
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
                                                                                <td class="bold" nowrap>No Involve Reason:</td>
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
                                                                    <td class="bold" colspan="2">Comments:</td>
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
                                                </table>
		                                    </div>		                                    	                                    

                                            <div id="divTab6" style='<%=boolMyDocuments == true ? "display:inline" : "display:none" %>'>
                                                <br />
                                                <table width="100%" cellpadding="0" cellspacing="5" border="0">
                                                    <tr>
                                                        <td rowspan="2" valign="top"><img src="/images/documents_mine.gif" border="0" align="absmiddle" /></td>
                                                        <td class="hugeheader" width="100%" valign="bottom">My Documents</td>
                                                    </tr>
                                                    <tr>
                                                        <td width="100%" valign="top">My Documents are documents that are only for you, or can be shared with others.  To add a document and configure the permissions, click the <b>upload</b> button.</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" background="/images/lines.gif">&nbsp;</td>
                                                    </tr>
                                                </table><br />
                                                <table width="100%" cellpadding="2" cellspacing="1" border="0">
                                                    <tr>
                                                        <td><asp:Button ID="btnDocuments" runat="server" Text="Upload" Width="100" CssClass="default" /></td>
                                                        <td align="right"><asp:CheckBox ID="chkMyDescription" runat="server" CssClass="default" Text="Show Descriptions" AutoPostBack="true" OnCheckedChanged="chkMyDescription_Change" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2"><asp:Label ID="lblMyDocuments" runat="server" CssClass="default" /></td>
                                                    </tr>
                                                </table>
                                                <p>&nbsp;</p>
                                            </div>
                                            <div id="divTab7" style='<%=boolDocuments == true ? "display:inline" : "display:none" %>'>
                                                <br />
                                                <table width="100%" cellpadding="0" cellspacing="5" border="0">
                                                    <tr>
                                                        <td rowspan="2" valign="top"><img src="/images/documents.gif" border="0" align="absmiddle" /></td>
                                                        <td class="hugeheader" width="100%" valign="bottom">Project Documents</td>
                                                    </tr>
                                                    <tr>
                                                        <td width="100%" valign="top">Project Documents are shared documents that can be viewed by other project resources.</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" background="/images/lines.gif">&nbsp;</td>
                                                    </tr>
                                                </table><br />
                                                <table width="100%" cellpadding="2" cellspacing="1" border="0">
                                                    <tr>
                                                        <td align="right"><asp:CheckBox ID="chkDescription" runat="server" CssClass="default" Text="Show Descriptions" AutoPostBack="true" OnCheckedChanged="chkDescription_Change" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td><asp:Label ID="lblDocuments" runat="server" CssClass="default" /></td>
                                                    </tr>
                                                </table>
                                                <p>&nbsp;</p>
                                            </div>
                                            <div id="divTab8"  style='<%=boolResource == true ? "display:inline" : "display:none" %>'>
                                                <br />
                                                <table width="100%" cellpadding="0" cellspacing="5" border="0">
                                                    <tr>
                                                        <td rowspan="2" valign="top"><img src="/images/users40.gif" border="0" align="absmiddle" /></td>
                                                        <td class="hugeheader" width="100%" valign="bottom">Resource Involvement</td>
                                                    </tr>
                                                    <tr>
                                                        <td width="100%" valign="top">The following list shows all resources involved in this project. Send a Communication to someone by completing the form at the bottom.</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" background="/images/lines.gif">&nbsp;</td>
                                                    </tr>
                                                </table><br />
                                                <table width="100%" cellpadding="2" cellspacing="1" border="0">
                                                    <tr>
                                                        <td>
                                                            <table width="100%" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                                                                <tr bgcolor="#EEEEEE">
                                                                    <td nowrap><b>Resource</b></td>
                                                                    <td nowrap><b>Department</b></td>
                                                                    <td nowrap align="right"><b>Allocated</b></td>
                                                                    <td nowrap align="right"><b>Used</b></td>
                                                                    <td nowrap align="right"><b>Completed</b></td>
                                                                    <td nowrap align="center"><b>Status</b></td>
                                                                </tr>
                                                                <asp:repeater ID="rptInvolvement" runat="server">
                                                                    <ItemTemplate>
                                                                        <tr class="default">
                                                                            <asp:Label ID="lblId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem, "id") %>' />
                                                                            <td><asp:Label ID="lblUser" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "userid") %>' /></td>
                                                                            <td><asp:Label ID="lblItem" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "itemid") %>' /></td>
                                                                            <td align="right"><asp:Label ID="lblAllocated" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "allocated") %>' /></td>
                                                                            <td align="right"><asp:Label ID="lblUsed" runat="server" CssClass="default" Text='' /></td>
                                                                            <td align="right"><asp:Label ID="lblPercent" runat="server" CssClass="default" Text='' /></td>
                                                                            <td align="center"><asp:Label ID="lblStatus" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "status") %>' /></td>
                                                                        </tr>
                                                                    </ItemTemplate>
                                                                </asp:repeater>
                                                            <tr>
                                                                <td colspan="7">
                                                                    <asp:Label ID="lblNoInvolvement" runat="server" CssClass="default" Visible="false" Text="<img src='/images/alert.gif' border='0' align='absmiddle'> There are no resources involved in this project" />
                                                                </td>
                                                            </tr>
                                                            </table>
                                                            <br /><br />
                                                            <table width="100%" cellpadding="3" cellspacing="2" border="0">
                                                                <tr>
                                                                    <td colspan="2" class="greentableheader">Send Communication</td>
                                                                </tr>
                                                                <tr>
                                                                    <td nowrap>Resource:</td>
                                                                    <td width="100%"><asp:DropDownList ID="ddlResource" runat="server" CssClass="default" Width="250" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td nowrap>Communication:</td>
                                                                    <td width="100%">
                                                                        <asp:DropDownList ID="ddlCommunication" runat="server" CssClass="default" Width="250">
                                                                            <asp:ListItem Value="-- SELECT --" />
                                                                            <asp:ListItem Value="Email" />
                                                                            <asp:ListItem Value="Pager" />
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td nowrap>Message:</td>
                                                                    <td width="100%"><asp:TextBox ID="txtMessage" runat="server" CssClass="default" TextMode="MultiLine" Rows="8" Width="80%" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td nowrap>&nbsp;</td>
                                                                    <td width="100%"><asp:Button ID="btnMessage" runat="server" CssClass="default" Text="Send" Width="75" OnClick="btnMessage_Click" /></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
		                                </td>
		                            </tr>
		                        </table>
		                    </td>
	                    </tr>
	                </table>
            </td>
        </tr>
    </table>
</asp:Panel>
<asp:Panel ID="panDenied" runat="server" Visible="false">
    <table width="100%" cellpadding="2" cellspacing="2" border="0">
        <tr>
            <td><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
            <td class="header"><img src="/images/bigX.gif" border="0" align="absmiddle" /> Access Denied</td>
        </tr>
        <tr><td colspan="2">&nbsp;</td></tr>
        <tr>
            <td><img src="/images/spacer.gif" border="0" width="15" height="1" /></td>
            <td>You do not have rights to view this item.</td>
        </tr>
        <tr><td colspan="2">&nbsp;</td></tr>
        <tr>
            <td colspan="2"><hr size="1" noshade /></td>
        </tr>
        <tr>
            <td colspan="2" align="right">
                <table width="100%" cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td class="footer"></td>
                        <td align="right"><asp:Button ID="btnDenied" runat="server" CssClass="default" Width="75" Text="Close" /></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Panel>
<asp:Label ID="lblResourceWorkflow" runat="server" Visible="false" />
<asp:HiddenField ID="hdnTab" runat="server" />
<asp:HiddenField ID="hdnInvestigatedBy" runat="server" />
<asp:HiddenField ID="hdnIDCSPOC" runat="server" />
<asp:Label ID="lblInvestigateID" runat="server" Visible="false" />
