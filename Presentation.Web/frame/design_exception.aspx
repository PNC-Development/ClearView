<%@ Page Language="C#" MasterPageFile="~/frame.Master" AutoEventWireup="true" CodeBehind="design_exception.aspx.cs" Inherits="NCC.ClearView.Presentation.Web.design_exception" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" Runat="Server">
<script type="text/javascript">
    function Done(accepted, chk) {
        chk = document.getElementById(chk);
        if (accepted) {
            if (chk.checked) {
                var control = GetQuerystringParameterByName("control");
                var chk = parent.document.getElementById(control);
                if (chk != null) {
                    chk.checked = true;
                    parent.ShowDesignDIVExceptionOK();
                }
                else
                    alert('Oops! There was a problem confirming this design');
                parent.HidePanel();
            }
            else
                alert('Select the checkbox indicating you understand the terms and conditions of this disclaimer');
        }
        else
            parent.HidePanel();
        return false;
    }
</script>
<br />
<table width="100%" cellpadding="5" cellspacing="3" border="0">
    <tr>
        <td class="header">IMPORTANT PLEASE READ DISCLAIMER:</td>
    </tr>
    <tr>
        <td>
            <p>The Aritecture Review Board (ARB) exception justification text box is a method for the project to provide a justification and seek approval to implement a solution that varies from the functional company standard(s) and or policies. The ARB resources are <b>the ONLY resources</b> (<a href="javascript:void(0);" onclick="alert('<%=ARB %>');">click here to view ARB resources</a>) that will read the contents within the exception justification text box. NOTE: The ARB exception justification text box is not used as a set of instructions to build out the non-standard environment that the project is requesting. There are no humans or build resources to intercept the build instructions because the process is automated. ClearView will build only what has been permitted by technology domain owners.</p>
            There are 2 avenues in which a project can achieve the expected non-standard build results:
            <ol>
                <li>Prior to approving and executing the design, ClearView can be configured to accept the newly approved technology solution. New technology solutions are introduced by the engineering department and are configured in ClearView based of a set of written functional standards.<br /><br /></li>
                <li>After the environment has been built by ClearView based of the current standards configured in ClearView, the project can work with the various technology domain owners to achieve and validate the expected build results. Only the technology domain owners have the knowledge and expertise to represent their technology domain.</li>
            </ol>
            <p><asp:CheckBox ID="chkTerms" runat="server" Text="I understand the terms and the conditions of this disclaimer and wish to proceed with the ARB exception justification." /></p>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Button ID="btnAgree" runat="server" CssClass="default" Width="75" Text="Agree" />
            <asp:Button ID="btnDisagree" runat="server" CssClass="default" Width="75" Text="Disagree" />
        </td>
    </tr>
</table>
</asp:Content>
