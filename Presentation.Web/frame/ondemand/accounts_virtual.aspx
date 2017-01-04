<%@ Page Language="C#" MasterPageFile="~/frame.Master" AutoEventWireup="true" CodeBehind="accounts_virtual.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.accounts_virtual" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" runat="server">
<script type="text/javascript">
    function CheckAdmin(oCheck, oDDL1) {
        oDDL1 = document.getElementById(oDDL1);
        if (oCheck.checked == true) {
        }
        else {
        }
    }
    function ShowAccountDetail(oDiv) {
        oDiv = document.getElementById(oDiv);
        if (oDiv.style.display == 'inline')
            oDiv.style.display = 'none';
        else
            oDiv.style.display = 'inline';
    }
    var oReturn = 0;
    var oActiveXAccenture = null;
    function EnsureAccenture(oHidden, strUsers) {
        oHidden = document.getElementById(oHidden);
        oActiveXAccenture = new ActiveXObject("Microsoft.XMLHTTP");
        oActiveXAccenture.onreadystatechange = EnsureAccenture_a;
        oActiveXAccenture.open("GET", "/frame/ajax/ajax_accenture.aspx?u=GET", false);
        oActiveXAccenture.send("<ajax><value>" + escape(oHidden.value) + "</value><value>" + escape(strUsers) + "</value></ajax>");
        //return false;
        while (oReturn == 0) {
        }
        if (oReturn == -1)
            return false;
        else
            return true;
    }
    function EnsureAccenture_a() {
        if (oActiveXAccenture.readyState == 4)
        {
            if (oActiveXAccenture.status == 200) {
                var or = oActiveXAccenture.responseXML.documentElement.childNodes;
                if (or[0].childNodes[0].text == "0") {
                    oReturn = -1;
                    alert('You cannot add an ACCENTURE user with a non-ACCENTURE user\n\nACCENTURE workstations are to be kept separate from non-ACCENTURE workstations.\n\nPlease create another workstation for this user');
                    return false;
                }
                else {
                    oReturn = 1;
                    alert(or[0].childNodes[0].text);
                    //document.forms[0].submit();
                }
            }
            else 
                alert('There was a problem getting the information');
        }
    }
</script>
<table width="100%" cellpadding="0" cellspacing="5" border="0">
    <tr>
        <td rowspan="2"><img src="/images/account_request.gif" border="0" align="absmiddle" /></td>
        <td class="header" width="100%" valign="bottom">ClearView Virtual Machine Account Configuration</td>
    </tr>
    <tr>
        <td width="100%" valign="top">Add the accounts you wish to have access to this virtual machine</td>
    </tr>
</table>
<table width="100%" border="0" cellpadding="2" cellspacing="3">
    <tr>
        <td colspan="2">
            <table width="100%" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
                <tr>
                    <td nowrap><img src="/images/bigAlert.gif" border="0" align="absmiddle" /></td>
                    <td width="100%"><b>NOTE:</b> Only one person can remotely access this workstation at a time.</td>
                </tr>
            </table>
        </td>
    </tr>
    <asp:Panel ID="panProduction" runat="server" Visible="false">
    <tr>
        <td colspan="2">
            <table width="100%" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #999999" bgcolor="#f9f9f9">
                <tr>
                    <td nowrap><img src="/images/bigAlert.gif" border="0" align="absmiddle" /></td>
                    <td width="100%"><b>NOTE:</b> For accounts requested after this build, you will need to submit a LAN Access Form.</td>
                </tr>
            </table>
        </td>
    </tr>
    </asp:Panel>
    <tr>
        <td width="50%" height="100%" valign="top">
            <table width="100%" height="100%" cellpadding="0" cellspacing="0" border="0">
                <tr height="1">
                    <td><img src="/images/table_topleft.gif" border="0" width="5" height="26"></td>
                    <td nowrap background="/images/table_top.gif" class="greentableheader" width="100%">Account Properties</td>
                    <td><img src="/images/table_topright.gif" border="0" width="5" height="26"></td>
                </tr>
                <tr>
                    <td background="/images/table_left.gif"><img src="/images/table_left.gif" width="5" height="10"></td>
                    <td width="100%" bgcolor="#FFFFFF" valign="top">
                        <table width="100%" border="0" cellspacing="4" cellpadding="4">
                            <tr>
                                <td nowrap>Workstation:</td>
                                <td width="100%"><asp:Label ID="lblWorkstation" runat="server" CssClass="default" /></td>
                            </tr>
                            <tr>
                                <td nowrap>Domain:</td>
                                <td width="100%"><asp:Label ID="lblDomain" runat="server" CssClass="default" /></td>
                            </tr>
                            <tr>
                                <td nowrap>User:</td>
                                <td width="100%">
                                    <table cellpadding="0" cellspacing="0" border="0">
                                        <tr>
                                            <td><asp:TextBox ID="txtUser" runat="server" Width="300" CssClass="default" /></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div id="divAJAX" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                                                    <asp:ListBox ID="lstAJAX" runat="server" CssClass="default" />
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td nowrap class="footer">&nbsp;</td>
                                <td class="footer">(Please enter a valid LAN ID, First Name, or Last Name)</td>
                            </tr>
                            <tr>
                                <td></td>
                                <td nowrap><asp:LinkButton ID="btnManager" runat="server" Text="User Not Appearing in the List? Click Here." /></td>
                            </tr>
                        </table>
                    </td>
                    <td background="/images/table_right.gif"><img src="/images/table_right.gif" width="5" height="10"></td>
                </tr>
                <tr height="1">
                    <td><img src="/images/table_bottomLeft.gif" border="0" width="5" height="9"></td>
                    <td width="100%" background="/images/table_bottom.gif"></td>
                    <td><img src="/images/table_bottomRight.gif" border="0" width="5" height="9"></td>
                </tr>
            </table>
        </td>
        <td width="50%" height="100%" valign="top">
            <table width="100%" height="100%" cellpadding="0" cellspacing="0" border="0">
                <tr height="1">
                    <td><img src="/images/table_topleft.gif" border="0" width="5" height="26"></td>
                    <td nowrap background="/images/table_top.gif" class="greentableheader" width="100%">Account Permissions</td>
                    <td><img src="/images/table_topright.gif" border="0" width="5" height="26"></td>
                </tr>
                <tr>
                    <td background="/images/table_left.gif"><img src="/images/table_left.gif" width="5" height="10"></td>
                    <td width="100%" bgcolor="#FFFFFF" valign="top">
                        <table width="100%" border="0" cellspacing="2" cellpadding="2">
                            <tr>
                                <td nowrap>Remote Desktop:</td>
                                <td width="100%"><asp:CheckBox ID="chkRemote" runat="server" CssClass="default" Checked="true" /></td>
                            </tr>
                            <tr>
                                <td colspan="2" class="footer">User can remote to this workstation with limited rights</td>
                            </tr>
                            <asp:Panel ID="panAdmin" runat="server" Visible="false">
                            <tr>
                                <td colspan="2">&nbsp;</td>
                            </tr>
                            <tr>
                                <td nowrap>Administrator:</td>
                                <td width="100%"><asp:CheckBox ID="chkAdmin" runat="server" CssClass="default" /></td>
                            </tr>
                            <tr>
                                <td colspan="2" class="footer">User has full access to this workstation</td>
                            </tr>
                            </asp:Panel>
                        </table>
                    </td>
                    <td background="/images/table_right.gif"><img src="/images/table_right.gif" width="5" height="10"></td>
                </tr>
                <tr height="1">
                    <td><img src="/images/table_bottomLeft.gif" border="0" width="5" height="9"></td>
                    <td width="100%" background="/images/table_bottom.gif"></td>
                    <td><img src="/images/table_bottomRight.gif" border="0" width="5" height="9"></td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td colspan="2" align="right"><asp:Button ID="btnAdd" runat="server" CssClass="default" Width="125" OnClick="btnAdd_Click" Text="Add Account" /></td>
    </tr>
    <tr>
        <td colspan="2" align="center" class="bigcheck">
            <asp:Label ID="lblSaved" runat="server" Text="<img src='/images/bigCheck.gif' border='0' align='absmiddle' /> Account Added" Visible="false" />
            <asp:Label ID="lblDelete" runat="server" Text="<img src='/images/bigCheck.gif' border='0' align='absmiddle' /> Account Deleted" Visible="false" />
            <asp:Label ID="lblDone" runat="server" Text="<img src='/images/bigCheck.gif' border='0' align='absmiddle' /> Account Request Completed!" Visible="false" />
        </td>
    </tr>
    <tr><td colspan="2">&nbsp;</td></tr>
    <tr>
        <td colspan="2">
            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td><img src="/images/table_topleft.gif" border="0" width="5" height="26"></td>
                    <td nowrap background="/images/table_top.gif" class="greentableheader" width="100%">Current Configuration</td>
                    <td><img src="/images/table_topright.gif" border="0" width="5" height="26"></td>
                </tr>
                <tr>
                    <td background="/images/table_left.gif"><img src="/images/table_left.gif" width="5" height="10"></td>
                    <td width="100%" bgcolor="#FFFFFF">
            <div style="height:100%; width:100%; overflow:auto">
            <table width="100%" cellpadding="4" cellspacing="0" border="0" align="center">
                <tr>
                    <td><b><u>Username:</u></b></td>
                    <td align="center"><b><u>Remote Desktop:</u></b></td>
                    <td align="center"><b><u>Administrator:</u></b></td>
                    <td></td>
                </tr>
                <asp:repeater ID="rptAccounts" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td valign="top"><%# oUser.GetFullName(Int32.Parse(DataBinder.Eval(Container.DataItem, "userid").ToString())) %> (<%# oUser.GetName(Int32.Parse(DataBinder.Eval(Container.DataItem, "userid").ToString()))%>)</td>
                            <td valign="top" align="center"><%# "<img src=\"/images/" + (DataBinder.Eval(Container.DataItem, "remote").ToString() == "1" ? "check" : "cancel") + ".gif\" border=\"0\" align=\"absmiddle\"/>"%></td>
                            <td valign="top" align="center"><%# "<img src=\"/images/" + (DataBinder.Eval(Container.DataItem, "admin").ToString() == "1" ? "check" : "cancel") + ".gif\" border=\"0\" align=\"absmiddle\"/>"%></td>
                            <td valign="top" align="right"><asp:LinkButton ID="btnDelete" runat="server" Text="Delete" OnClick="btnDelete_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' /></td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr bgcolor="F6F6F6">
                            <td valign="top"><%# oUser.GetFullName(Int32.Parse(DataBinder.Eval(Container.DataItem, "userid").ToString())) %> (<%# oUser.GetName(Int32.Parse(DataBinder.Eval(Container.DataItem, "userid").ToString()))%>)</td>
                            <td valign="top" align="center"><%# "<img src=\"/images/" + (DataBinder.Eval(Container.DataItem, "remote").ToString() == "1" ? "check" : "cancel") + ".gif\" border=\"0\" align=\"absmiddle\"/>" %></td>
                            <td valign="top" align="center"><%# "<img src=\"/images/" + (DataBinder.Eval(Container.DataItem, "admin").ToString() == "1" ? "check" : "cancel") + ".gif\" border=\"0\" align=\"absmiddle\"/>"%></td>
                            <td valign="top" align="right"><asp:LinkButton ID="btnDelete" runat="server" Text="Delete" OnClick="btnDelete_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' /></td>
                        </tr>
                    </AlternatingItemTemplate>
                </asp:repeater>
                <tr>
                    <td colspan="2">
                        <asp:Label ID="lblNone" runat="server" CssClass="default" Visible="false" Text="<img src='/images/spacer.gif' border='0' height='1' width='25' /><img src='/images/alert.gif' border='0' align='absmiddle'> You have not added any accounts to this request" />
                    </td>
                </tr>
            </table>
            </div>
                    </td>
                    <td background="/images/table_right.gif"><img src="/images/table_right.gif" width="5" height="10"></td>
                </tr>
                <tr>
                    <td><img src="/images/table_bottomLeft.gif" border="0" width="5" height="9"></td>
                    <td width="100%" background="/images/table_bottom.gif"></td>
                    <td><img src="/images/table_bottomRight.gif" border="0" width="5" height="9"></td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td></td>
        <td align="right"><asp:Button ID="btnSkip" runat="server" CssClass="default" Width="125" OnClick="btnSkip_Click" Text="Skip Accounts" /> <asp:Button ID="btnSubmit" runat="server" CssClass="default" Width="125" OnClick="btnSubmit_Click" Text="Submit Accounts" /></td>
    </tr>
</table>
<asp:HiddenField ID="hdnUser" runat="server" />
<asp:Label ID="lblBuild" runat="server" Visible="false" />
<asp:Label ID="lblStep" runat="server" Visible="false" />
</asp:Content>
