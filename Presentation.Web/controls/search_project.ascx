<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="search_project.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.search_project" %>


<script type="text/javascript">
/*
    var strReportPath123 = null;
    function ValidateSearch(oH, oNA, oNU, oP, oBy, oS) {
        oH = document.getElementById(oH);
        if (oH.value == "1") {
            oNA = document.getElementById(oNA);
            oNU = document.getElementById(oNU);
            oP = document.getElementById(oP);
            oBy = document.getElementById(oBy);
            if (trim(oNA.value) == "" && trim(oNU.value) == "" && oP.selectedIndex == 0 && oBy.value == 0)
            {
                alert('Please enter a project name, project number, portfolio or submitter');
                oNA.focus();
                return false;
            }
        }
        if (oH.value == "6") {
            oS = document.getElementById(oS);
            if (trim(oS.value) == "")
            {
                alert('Please enter a skill');
                oS.focus();
                return false;
            }
        }
        return true;
    }
    function ViewWorkload(strUrl) {
        if(event.srcElement.tagName != 'A')
            window.navigate(strUrl);
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
    function ShowScorecard(oReport, strReport, oMaxim) {
        oReport = document.getElementById(oReport);
        oReport.src = strReport;
        strReportPath123 = strReport;
        oMaxim = document.getElementById(oMaxim);
        oMaxim.disabled = false;
        return false;
    }
    function ShowScorecardMax() {
        window.open("/frame/loading.htm?referrer=" + strReportPath123,"_blank","height=600,width=800,menubar=no,resizable=yes,scrollbars=no,status=no,toolbar=no");
        return false;
    }
    */
</script>
<table width="100%" cellpadding="0" cellspacing="5" border="0">
    <tr>
        <td rowspan="2"><img src="/images/ProjectResults.gif" border="0" align="absmiddle" /></td>
        <td class="header" width="100%" valign="bottom"><asp:Label ID="lblProjectName" runat="server" CssClass="header" /></td>
    </tr>
    <tr>
        <td width="100%" valign="top">The following shows all of the resources assigned to the project.</td>
    </tr>
</table>
<br />
<table width="100%" cellpadding="3" cellspacing="2" border="0">
    <tr>
        <td>
            <table width="100%" cellpadding="3" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                <tr bgcolor="#EEEEEE">
                    <td nowrap width="1">&nbsp;</td>
                    <td nowrap><b>Technician</b></td>
                    <td nowrap><b>Name</b></td>
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
                            <td width="1"><asp:Label ID="lblImage" runat="server" CssClass="default" Text='' /></td>
                            <td><asp:Label ID="lblUser" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "userid") %>' /></td>
                            <td><%# DataBinder.Eval(Container.DataItem, "name") %></td>
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
        </td>
    </tr>
</table>
