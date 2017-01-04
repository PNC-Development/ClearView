<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="inventory_old.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.inventory_old" %>



<script type="text/javascript">
    function ChangeFrame(oCell, oShow, oHide1, oHide2, oHidden, strHidden) {
        oShow = document.getElementById(oShow);
        oShow.style.display = "inline";
        oHide1 = document.getElementById(oHide1);
        oHide1.style.display = "none";
        oHide2 = document.getElementById(oHide2);
        oHide2.style.display = "none";
    	var oRow = oCell.parentElement;
	    for (var yy=0; yy<oRow.children.length; yy++) {
    		var oNot = oRow.getElementsByTagName("td").item(yy);
    		if (oNot.className == "cmbutton")
                oNot.style.border = "1px solid #94a6b5"
    	}
	    oCell.style.borderTop = "3px solid orange"
        oCell.style.borderBottom = "1px solid #FFFFFF"
        oHidden = document.getElementById(oHidden);
        oHidden.value = strHidden;
    }
    function ShowDetail(oImg, oDiv) {
        oImg = document.getElementById(oImg);
        oDiv = document.getElementById(oDiv);
        if (oDiv.style.display == 'inline') {
            oDiv.style.display = 'none';
            SwapImage(oImg, '/images/biggerPlus.gif');
        }
        else {
            oDiv.style.display = 'inline';
            SwapImage(oImg, '/images/biggerMinus.gif');
        }
    }
   
//    function LoadTabber(strA, strDiv) 
//    {
//        ChangeTab(document.getElementById('aTab4'),'divTab4',null,null,true);
//        
//    }
//    
    function setTabvalue(strTab)
    {
        var hdnTab = document.getElementById("<%=hdnTab.ClientID%>");
        if (hdnTab!= null)
        {
            hdnTab.value = strTab;
        }
    }
    
    function LoadCurrentTab() 
    {
        var hdnTab = document.getElementById("<%=hdnTab.ClientID%>");
      if (hdnTab!= null && hdnTab.value !="" && hdnTab.value !=0)
      {
        var strA="";
        var strDiv="";
        
        strA='aTab'+hdnTab.value;
        strDiv='divTab'+hdnTab.value;
        ChangeTab(document.getElementById(strA),strDiv,null,null,true);
      }
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
                    <td rowspan="2"><img src="/images/inventory.gif" border="0" align="absmiddle" /></td>
                    <td class="header" width="100%" valign="bottom">Inventory Manager</td>
                </tr>
                <tr>
                    <td width="100%" valign="top">Manage the inventory related to on-demand activities.</td>
                </tr>
            </table>
            <table width="100%" cellpadding="4" cellspacing="3" border="0">
                <tr>
                    <td>
                        <table width="100%" cellpadding="0" cellspacing="0" border="0" class="default">
                            <tr>
                                <td width="6"><img src="/images/box_top_left.gif" border="0" width="6" height="6"></td>
                                <td width="100%" nowrap background="/images/box_top.gif"></td>
                                <td width="6"><img src="/images/box_top_right.gif" border="0" width="6" height="6"></td>
                            </tr>
                            <tr>
                                <td width="6" background="/images/box_left.gif"><img src="/images/box_left.gif" width="6" height="6"></td>
                                <td width="100%" bgcolor="#FFFFFF" valign="top">
                                    <%=strPlatforms %>
                                </td>
                                <td width="6" background="/images/box_right.gif"><img src="/images/box_right.gif" width="6" height="6"></td>
                            </tr>
                            <tr>
                                <td width="6"><img src="/images/box_bottom_left.gif" border="0" width="6" height="6"></td>
                                <td width="100%" background="/images/box_bottom.gif"></td>
                                <td width="6"><img src="/images/box_bottom_right.gif" border="0" width="6" height="6"></td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <asp:Panel ID="panSelect" runat="server" Visible="false">
                <table width="100%" height="100%" cellpadding="0" cellspacing="0" border="0">
                    <tr> 
                        <td valign="middle" width="100%" height="100%" bgcolor="#FFFFFF">
                            <p>&nbsp;</p>
                            <p>&nbsp;</p>
                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td align="center" class="hugeheader"><img src="/images/bigalert.gif" border="0" align="absmiddle" /> Please select a platform...</td>
                                </tr>
                            </table>
                            <p>&nbsp;</p>
                            <p>&nbsp;</p>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="panShow" runat="server" Visible="false">
                <table width="100%" cellpadding="4" cellspacing="3" border="0">
                    <tr>
                        <td>
                            <table cellpadding="0" cellspacing="0" border="0">
                                <tr>
                                    <%=strTabs %>
                                    <td width="100%" background="/images/TabEmptyBackground.gif">&nbsp;</td>
                                </tr>
                            </table>
                            <div id="divTab1" style='<%=boolAction == true ? "display:inline" : "display:none" %>'>
                                <table width="100%" cellpadding="3" cellspacing="2" border="0">
                                    <tr>
                                        <td><asp:PlaceHolder ID="PHAction" runat="server" /></td>
                                    </tr>
                                </table>
                            </div>
                            <div id="divTab2" style='<%=boolDemand == true ? "display:inline" : "display:none" %>'>
                                <table width="100%" cellpadding="0" cellspacing="2" border="0">
                                    <tr>
                                        <td><asp:PlaceHolder ID="PHDemand" runat="server" /></td>
                                    </tr>
                                </table>
                            </div>
                            <div id="divTab3" style='<%=boolSupply == true ? "display:inline" : "display:none" %>'>
                                <table width="100%" cellpadding="3" cellspacing="2" border="0">
                                    <tr>
                                        <td><asp:PlaceHolder ID="PHSupply" runat="server" /></td>
                                    </tr>
                                </table>
                            </div>
                            <div id="divTab4" style='<%=boolOrder == true ? "display:inline" : "display:none" %>'>
                                <table width="100%" cellpadding="3" cellspacing="2" border="0">
                                    <tr>
                                        <td><asp:PlaceHolder ID="PHOrder" runat="server" /></td>
                                    </tr>
                                </table>
                            </div>
                            <div id="divTab5" style='<%=boolOrderView == true ? "display:inline" : "display:none" %>'>
                                <table width="100%" cellpadding="3" cellspacing="2" border="0">
                                    <tr>
                                        <td><asp:PlaceHolder ID="PHOrderView" runat="server" /></td>
                                    </tr>
                                </table>
                            </div>
                            <div id="divTab6" style='<%=boolAdd == true ? "display:inline" : "display:none" %>'>
                                <table width="100%" cellpadding="3" cellspacing="2" border="0">
                                    <tr>
                                        <td><asp:PlaceHolder ID="PHAdd" runat="server" /></td>
                                    </tr>
                                </table>
                            </div>
                            <div id="divTab7" style='<%=boolSettings == true ? "display:inline" : "display:none" %>'>
                                <table width="100%" cellpadding="3" cellspacing="2" border="0">
                                    <tr>
                                        <td><asp:PlaceHolder ID="PHSettings" runat="server" /></td>
                                    </tr>
                                </table>
                            </div>
                            <div id="divTab8" style='<%=boolForms == true ? "display:inline" : "display:none" %>'>
                                <table width="100%" cellpadding="3" cellspacing="2" border="0">
                                    <tr>
                                        <td><asp:PlaceHolder ID="PHForms" runat="server" /></td>
                                    </tr>
                                </table>
                            </div>
                            <div id="divTab9" style='<%=boolAlert == true ? "display:inline" : "display:none" %>'>
                                <table width="100%" cellpadding="3" cellspacing="2" border="0">
                                    <tr>
                                        <td><asp:PlaceHolder ID="PHAlert" runat="server" /></td>
                                    </tr>
                                </table>
                            </div>
                            <div id="divTab10" style='<%=boolSecurity == true ? "display:inline" : "display:none" %>'>
                                <table width="100%" cellpadding="3" cellspacing="2" border="0">
                                    <tr>
                                        <td>
                                            <table width="100%" cellpadding="4" cellspacing="0" border="0">
                                                <tr>
                                                    <td colspan="2" class="header">Security</td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">Administer who has access to this platform.</td>
                                                </tr>
                                                <tr>
                                                    <td nowrap>Administrator:</td>
                                                    <td width="100%">
                                                        <table cellpadding="0" cellspacing="0" border="0">
                                                            <tr>
                                                                <td><asp:TextBox ID="txtAdministrator" runat="server" Width="300" CssClass="default" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <div id="divAdministrator" runat="server" style="overflow:hidden; position:absolute; display:none; background-color:#FFFFFF; border:solid 1px #CCCCCC">
                                                                        <asp:ListBox ID="lstAdministrator" runat="server" CssClass="default" />
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td nowrap>&nbsp;</td>
                                                    <td width="100%"><asp:Button ID="btnAdministrator" runat="server" CssClass="default" Width="100" Text="Add Administrator" OnClick="btnAdministrator_Click" /></td>
                                                </tr>
                                            </table>
                                            <br />
                                            <table width="600" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                                                <tr bgcolor="#EEEEEE">
                                                    <td nowrap><b>Administrator Name</b></td>
                                                    <td nowrap>&nbsp;</td>
                                                </tr>
                                                <asp:repeater ID="rptAdministrators" runat="server">
                                                    <ItemTemplate>
                                                        <tr class="default">
                                                            <td width="100%"><%# DataBinder.Eval(Container.DataItem, "username") %></td>
                                                            <td align="right"><asp:LinkButton ID="btnDelete" runat="server" OnClick="btnDelete_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' Text="Delete" /></td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:repeater>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:Label ID="lblAdministrators" runat="server" CssClass="default" Visible="false" Text="<img src='/images/alert.gif' border='0' align='absmiddle'> There are no administrators" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
                <p>&nbsp;</p>
            </asp:Panel>
        </td>
        <td background="/images/table_right.gif"><img src="/images/table_right.gif" width="5" height="10"></td>
    </tr>
    <tr>
        <td><img src="/images/table_bottomLeft.gif" border="0" width="5" height="9"></td>
        <td width="100%" background="/images/table_bottom.gif"></td>
        <td><img src="/images/table_bottomRight.gif" border="0" width="5" height="9"></td>
    </tr>
</table>
<asp:HiddenField ID="hdnTab" runat="server" />
<asp:HiddenField ID="hdnAdministrator" runat="server" />
