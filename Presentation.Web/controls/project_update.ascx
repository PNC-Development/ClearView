<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="project_update.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.project_update" %>



<script type="text/javascript">
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
                    <td rowspan="2"><img src="/images/project_update.gif" border="0" align="absmiddle" /></td>
                    <td class="header" width="100%" valign="bottom">Project Update</td>
                </tr>
                <tr>
                    <td width="100%" valign="top">This page is used to update project information.</td>
                </tr>
            </table>
            <asp:Panel ID="panNone" runat="server" Visible="false">
            <table width="100%" cellpadding="4" cellspacing="3" border="0">
                <tr>
                    <td nowrap>Project Name:</td>
                    <td width="100%"><asp:TextBox ID="txtNameSearch" runat="server" Width="250" CssClass="default" MaxLength="100" /></td>
                </tr>
                <tr>
                    <td nowrap>Project Number:</td>
                    <td width="100%"><asp:TextBox ID="txtNumberSearch" runat="server" Width="250" CssClass="default" MaxLength="100" /></td>
                </tr>
                <tr>
                    <td nowrap>Project Lead:</td>
                    <td width="100%">
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td><asp:TextBox ID="txtManagerSearch" runat="server" Width="250" CssClass="default" /></td>
                            </tr>
                            <tr>
                                <td>
                                    <div id="divManagerSearch" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                                        <asp:ListBox ID="lstManagerSearch" runat="server" CssClass="default" />
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td nowrap>Integration Engineer:</td>
                    <td width="100%">
                    <table cellpadding="0" cellspacing="0" border="0">
                        <tr>
                            <td><asp:TextBox ID="txtEngineerSearch" runat="server" Width="250" CssClass="default" /></td>
                        </tr>
                        <tr>
                            <td>
                                <div id="divEngineerSearch" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                                    <asp:ListBox ID="lstEngineerSearch" runat="server" CssClass="default" />
                                </div>
                            </td>
                        </tr>
                    </table>
                    </td>
                </tr>
                <tr>
                    <td nowrap class="required"></td>
                    <td width="100%"><asp:Button ID="btnSearch" Text="Search" Width="100" CssClass="default" runat="server" OnClick="btnSearch_Click" /></td>
                </tr>
            </table>
            </asp:Panel>
            <asp:Panel ID="panSearch" runat="server" Visible="false">
            <table width="100%" cellpadding="4" cellspacing="3" border="0">
                <tr>
                    <td width="50%" valign="top">
                        <table width="100%" cellpadding="4" cellspacing="3" border="0">
                            <tr>
                                <td nowrap>Project Name:<font class="required">&nbsp;*</font></td>
                                <td width="100%"><asp:TextBox ID="txtName" runat="server" CssClass="default" Width="250" MaxLength="50" /></td>
                            </tr>
                            <tr>
                                <td nowrap>Project Number:<font class="required">&nbsp;*</font></td>
                                <td width="100%"><asp:TextBox ID="txtNumber" runat="server" CssClass="default" Width="100" MaxLength="20" /></td>
                            </tr>
                            <tr>
                                <td nowrap>Investment Class:<font class="required">&nbsp;*</font></td>
                                <td width="100%">
                                    <asp:DropDownList ID="ddlBaseDisc" runat="server" CssClass="default" ToolTip="If your initative is considerd Discretionary project DO NOT USE THIS FORM">
                                    <asp:ListItem Value="Acquisitions & Divestitures" />
                                    <asp:ListItem Value="Baseline" />
                                    <asp:ListItem Value="Client Billable/Contractual" />
                                    <asp:ListItem Value="Discretionary Project" />
                                    <asp:ListItem Value="Efficiency Initiatives" />
                                    <asp:ListItem Value="General Management & Administration" />
                                    <asp:ListItem Value="Client Implementations & Conversions" />
                                    <asp:ListItem Value="Non-Billable FTE" />
                                    <asp:ListItem Value="Application Support" />
                                    <asp:ListItem Value="Non-Technology" />
                                    <asp:ListItem Value="Regulatory & Compliance" />
                                    <asp:ListItem Value="Integration" />
                                    <asp:ListItem Value="Retained" />
                                    </asp:DropDownList></td>
                                </td>
                            </tr>
                            <tr>
                                <td nowrap>Sponsoring Portfolio:<font class="required">&nbsp;*</font></td>
                                <td width="100%"><asp:DropDownList ID="ddlPortfolio" runat="server" CssClass="default" Width="250" /></td>
                            </tr>
                            <tr>
                                <td nowrap>Segment:</td>
                                <td width="100%">
                                    <asp:DropDownList ID="ddlSegment" CssClass="default" runat="server" Width="250" Enabled="false" >
                                        <asp:ListItem Value="-- Please select a Sponsoring Portfolio --" />
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td nowrap>Executive Sponsor:</td>
                                <td width="100%">
                                    <table cellpadding="0" cellspacing="0" border="0">
                                        <tr>
                                            <td><asp:TextBox ID="txtExecutive" runat="server" Width="250" CssClass="default" /></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div id="divExecutive" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                                                    <asp:ListBox ID="lstExecutive" runat="server" CssClass="default" />
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td nowrap>Working Sponsor:</td>
                                <td width="100%">
                                    <table cellpadding="0" cellspacing="0" border="0">
                                        <tr>
                                            <td><asp:TextBox ID="txtWorking" runat="server" Width="250" CssClass="default" /></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div id="divWorking" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                                                    <asp:ListBox ID="lstWorking" runat="server" CssClass="default" />
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td nowrap>Project Lead:</td>
                                <td width="100%">
                                    <table cellpadding="0" cellspacing="0" border="0">
                                        <tr>
                                            <td><asp:TextBox ID="txtManager" runat="server" Width="250" CssClass="default" /></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div id="divManager" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                                                    <asp:ListBox ID="lstManager" runat="server" CssClass="default" />
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td nowrap>Integration Engineer:</td>
                                <td width="100%">
                                    <table cellpadding="0" cellspacing="0" border="0">
                                        <tr>
                                            <td><asp:TextBox ID="txtEngineer" runat="server" Width="250" CssClass="default" /></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div id="divEngineer" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                                                    <asp:ListBox ID="lstEngineer" runat="server" CssClass="default" />
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td nowrap>Technical Lead:</td>
                                <td width="100%">
                                    <table cellpadding="0" cellspacing="0" border="0">
                                        <tr>
                                            <td><asp:TextBox ID="txtLead" runat="server" Width="250" CssClass="default" /></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div id="divLead" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                                                    <asp:ListBox ID="lstLead" runat="server" CssClass="default" />
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td nowrap>Project Status:</td>
                                <td width="100%"><asp:Label ID="lblStatus" runat="server" CssClass="default" /></td>
                            </tr>
                            <tr>
                                <td nowrap></td>
                                <td width="100%"><asp:Button ID="btnSave" runat="server" CssClass="default" Width="75" Text="Save" OnClick="btnSave_Click" /></td>
                            </tr>
                        </table>
                    </td>
                    <td width="50%" valign="top" align="center">
                        <table width="200" cellpadding="4" cellspacing="3" border="0">
                            <tr>
                                <td class="required">* = Required Field</td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            </asp:Panel>
            <asp:Panel ID="panMultiple" runat="server" Visible="false">
            <table width="100%" cellpadding="4" cellspacing="3" border="0">
                <tr>
                    <td class="bold">Please select one of the following projects...</td>
                </tr>
                <tr>
                    <td>
                        <table width="100%" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                            <tr bgcolor="#EEEEEE">
                                <td nowrap><b>Project Name</b></td>
                                <td nowrap><b>Project Number</b></td>
                                <td nowrap><b>Initiative Type</b></td>
                                <td nowrap><b>Organization</b></td>
                                <td nowrap><b>Status</b></td>
                            </tr>
                            <%=strMultiple %>
                        </table>
                    </td>
                </tr>
            </table>
            </asp:Panel>
            <p>&nbsp;</p>
        </td>
        <td background="/images/table_right.gif"><img src="/images/table_right.gif" width="5" height="10"></td>
    </tr>
    <tr>
        <td><img src="/images/table_bottomLeft.gif" border="0" width="5" height="9"></td>
        <td width="100%" background="/images/table_bottom.gif"></td>
        <td><img src="/images/table_bottomRight.gif" border="0" width="5" height="9"></td>
    </tr>
</table>
<asp:HiddenField ID="hdnExecutive" runat="server" />
<asp:HiddenField ID="hdnWorking" runat="server" />
<asp:HiddenField ID="hdnManager" runat="server" />
<asp:HiddenField ID="hdnEngineer" runat="server" />
<asp:HiddenField ID="hdnLead" runat="server" />
<asp:HiddenField ID="hdnSegment" runat="server" />
<asp:HiddenField ID="hdnManagerSearch" runat="server" />
<asp:HiddenField ID="hdnEngineerSearch" runat="server" />
<asp:Label ID="lblProject" runat="server" Visible="false" />