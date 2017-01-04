<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="documentrepository_all.ascx.cs" Inherits="NCC.ClearView.Presentation.Web.documentrepository_all" %>


<script type="text/javascript">
    function ValidateDirName(oDirectory) {
        oDirectory = document.getElementById(oDirectory);
        var myArray = ["'", "%", "+"];
        for (ii in myArray) { 
            if (ValidateDirNameValue(oDirectory.value, myArray[ii]) == true)
            {
                alert(myArray[ii] + " is an invalid character and must be removed from the directory name");
                return false;
            }
        };
        return true;
    }
    function ValidateDirNameValue(strDirectory, strChar) {
        return (strDirectory.indexOf(strChar) > -1);
    }
</script>
<table width="100%" cellpadding="0" cellspacing="0" border="0" class="default">
    <tr>
        <td><img src="/images/table_topleft.gif" border="0" width="5" height="26"></td>
        <td width="100%" background="/images/table_top.gif" class="greentableheader"><asp:Label ID="lblTitle" runat="server" CssClass="greentableheader" /></td>
        <td><img src="/images/table_topright.gif" border="0" width="5" height="26"></td>
    </tr>
    <tr>
        <td background="/images/table_left.gif"><img src="/images/table_left.gif" width="5" height="10"></td>
        <td width="100%" bgcolor="#FFFFFF">
            <br />
            <asp:Panel ID="panView" runat="server" Visible="true">
                <%=strMenuTab1 %>
                <div id="divMenu1"> 
                <div id="divTab1" style='<%=boolApp == true ? "display:inline" : "display:none" %>'>
                    <br />
                    <table width="100%" cellpadding="0" cellspacing="5" border="0">
                        <tr>
                            <td rowspan="2"><img src="/images/documents.gif" border="0" align="absmiddle" /></td>
                            <td class="header" width="100%" valign="bottom"> Department Documents</td>
                        </tr>
                        <tr>
                            <td width="100%" valign="top">Lists all files associated with your department.</td>
                        </tr>
                    </table>
                    <table width="100%" cellpadding="4" cellspacing="3" border="0">
                        <tr>
                            <td><asp:Label ID="lblError" runat="server" CssClass="bigred" /></td>
                        </tr>
                        <tr>
                            <td align="left"><asp:Label ID="lblFolder" runat="server" CssClass="default" /></td>
                        </tr>
                        <tr>
                            <td colspan="2">
                              <table width="100%" cellpadding="5" cellspacing="0" border="0" class="default" style="border:solid 1px #CCCCCC">                                                
                                <asp:Repeater ID="rptDocs" runat="server">
                                   <HeaderTemplate>                          
                                      <tr bgcolor="#EEEEEE">                                 
                                            <td></td>
                                            <td><asp:LinkButton ID="btnName" runat="server" CssClass="tableheader" Text="<b>Name</b>" OnClick="btnOrder_Click" CommandArgument="name"/></td>
                                            <td><asp:LinkButton ID="btnType" runat="server" CssClass="tableheader" Text="<b>Type</b>" OnClick="btnOrder_Click" CommandArgument="type"/></td>
                                            <td><asp:LinkButton ID="btnSize" runat="server" CssClass="tableheader" Text="<b>Size (Bytes)</b>" OnClick="btnOrder_Click" CommandArgument="size"/></td>                                
                                            <td><asp:LinkButton ID="btnModifiedBy" runat="server" CssClass="tableheader" Text="<b>Modified By</b>" OnClick="btnOrder_Click" CommandArgument="profileid"/></td>                                
                                            <td><asp:LinkButton ID="btnModifiedOn" runat="server" CssClass="tableheader" Text="<b>Last Modified</b>" OnClick="btnOrder_Click" CommandArgument="modified"/></td>                           
                                            <td></td>
                                            <td></td>
                                            <td></td>
                                      </tr>
                                   </HeaderTemplate>
                                   <ItemTemplate>
                                      <tr>
                                            <asp:Label ID="lblPath" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem,"path") %>'/>
                                            <asp:Label ID="lblDeleted" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem,"deleted") %>'/>
                                            <asp:Label ID="lblId" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem,"id") %>' />
                                            <td nowrap><asp:ImageButton ID="imgType" runat="server" ImageUrl='<%# DataBinder.Eval(Container.DataItem, "type").ToString()== "Folder" ? "/images/folder24.gif": GetIcon(DataBinder.Eval(Container.DataItem, "type").ToString()) %>' /></td>
                                            <td width="40%" nowrap><asp:LinkButton ID="btnName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "name") %>' CommandArgument='<%# DataBinder.Eval(Container.DataItem, "path") %>' OnClick="Name_Click" /></td> 
                                            <td width="15%" nowrap><asp:Label ID="lblType" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "type") %>' /></td>
                                            <td width="15%" nowrap><asp:Label ID="lblSize" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "size") %>' /></td>
                                            <td width="15%" nowrap><%# oUser.GetFullName(Int32.Parse(DataBinder.Eval(Container.DataItem, "profileid").ToString())) %></td>
                                            <td width="15%" nowrap><%# DataBinder.Eval(Container.DataItem, "modified") %></td>
                                            <td nowrap><asp:ImageButton ID="imgRename" runat="server" ImageUrl="/images/rename.gif" BorderStyle="none" ToolTip="Rename" /></td>
                                            <td nowrap><asp:ImageButton ID="imgDelete" runat="server" ImageUrl="/images/cancel.gif" BorderStyle="none" OnClick="imgDelete_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' ToolTip="Delete" /></td>                                 
                                            <td nowrap><asp:ImageButton ID="imgShare" runat="server" ImageUrl="/images/docshare.gif" BorderStyle="none" ToolTip="Share" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' /></td>
                                      </tr> 
                                   </ItemTemplate>   
                                   <AlternatingItemTemplate>
                                      <tr bgcolor="#F6F6F6">
                                            <asp:Label ID="lblPath" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem,"path") %>'/>
                                            <asp:Label ID="lblDeleted" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem,"deleted") %>'/>
                                            <asp:Label ID="lblId" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem,"id") %>' />
                                            <td nowrap><asp:ImageButton ID="imgType" runat="server" ImageUrl='<%# DataBinder.Eval(Container.DataItem, "type").ToString()== "Folder" ? "/images/folder24.gif": GetIcon(DataBinder.Eval(Container.DataItem, "type").ToString()) %>' /></td>
                                            <td width="40%" nowrap><asp:LinkButton ID="btnName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "name") %>' CommandArgument='<%# DataBinder.Eval(Container.DataItem, "path") %>' OnClick="Name_Click" /></td> 
                                            <td width="15%" nowrap><asp:Label ID="lblType" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "type") %>' /></td>
                                            <td width="15%" nowrap><asp:Label ID="lblSize" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "size") %>' /></td>
                                            <td width="15%" nowrap><%# oUser.GetFullName(Int32.Parse(DataBinder.Eval(Container.DataItem, "profileid").ToString())) %></td>
                                            <td width="15%" nowrap><%# DataBinder.Eval(Container.DataItem, "modified") %></td>
                                            <td nowrap><asp:ImageButton ID="imgRename" runat="server" ImageUrl="/images/rename.gif" BorderStyle="none" ToolTip="Rename" /></td>
                                            <td nowrap><asp:ImageButton ID="imgDelete" runat="server" ImageUrl="/images/cancel.gif" BorderStyle="none" OnClick="imgDelete_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' ToolTip="Delete" /></td>                                 
                                            <td nowrap><asp:ImageButton ID="imgShare" runat="server" ImageUrl="/images/docshare.gif" BorderStyle="none" ToolTip="Share" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' /></td>
                                      </tr>                            
                                   </AlternatingItemTemplate>
                                </asp:Repeater> 
                                <tr><td colspan="9" class="default"><asp:Label ID="lblNoDocs" runat="server" Visible="false" Text="<img src='/images/alert.gif' border='0' align='absmiddle'> There are no files / folders" /></td></tr>
                              </table>
                            </td>
                        </tr>   
                        <tr>
                            <td colspan="2">
                                <table cellpadding="3" cellspacing="2" border="0">
                                    <tr>
                                        <td nowrap>Create a New Directory: </td>                                             
                                        <td nowrap><asp:TextBox ID="txtDirectory" MaxLength="50" runat="server" Width="500" CssClass="default" /></td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;</td>
                                        <td nowrap><asp:Button ID="btnCreate" runat="server" Text="Create" Width="100" OnClick="btnCreate_Click" CssClass="default" /></td>
                                    </tr>
                                    <tr>
                                        <td nowrap>Upload a File: </td>                 
                                        <td nowrap><asp:FileUpload ID="txtFile" runat="server" Width="500" Height="18" /></td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;</td>
                                        <td nowrap><asp:Button ID="btnUpload" runat="server" Text="Upload" Width="100" OnClick="btnUpload_Click" CssClass="default" /></td>
                                    </tr>                     
                                </table>
                            </td>
                        </tr>                                            
                        <tr>
                            <td colspan="2">&nbsp;</td>
                        </tr>
                    </table>
                </div>
                <div id="divTab2" style='<%=boolOthers == true ? "display:inline" : "display:none" %>'>
                    <br />
                    <table width="100%" cellpadding="0" cellspacing="5" border="0">
                        <tr>
                            <td rowspan="2"><img src="/images/documents_dept_shared.gif" border="0" align="absmiddle" /></td>
                            <td class="header" width="100%" valign="bottom"> Shared Documents</td>
                        </tr>
                        <tr>
                            <td width="100%" valign="top">Lists all shared files shared by other departments.</td>
                        </tr>
                    </table>
                    <table width="100%" cellpadding="4" cellspacing="3" border="0">                       
                        <tr>
                            <td colspan="2">
                              <table width="100%" cellpadding="5" cellspacing="0" border="0" class="default" style="border:solid 1px #CCCCCC">                                                
                                <asp:Repeater ID="rptOthers" runat="server">
                                   <ItemTemplate>
                                        <tr>
                                            <td><img src="/images/folder24.gif" border="0" align="absmiddle" /></td>
                                            <td width="100%"><a href="javascript:void(0);" onclick="ShowHideDiv2('div<%# DataBinder.Eval(Container.DataItem, "applicationid") %>');"><%# DataBinder.Eval(Container.DataItem, "name") %> (<%# DataBinder.Eval(Container.DataItem, "docs") %>)</a></td>
                                        </tr>
                                        <tr id='div<%#DataBinder.Eval(Container.DataItem, "applicationid").ToString()%>' style="display:none">
                                            <td colspan="2">
                                              <table width="100%" cellpadding="5" cellspacing="0" border="0" class="default" style="border:solid 1px #CCCCCC">                                                
                                                <asp:repeater id="rptShared" datasource='<%# ((DataRowView)Container.DataItem).Row.GetChildRows("relationship") %>' runat="server">
                                                   <HeaderTemplate>                          
                                                      <tr bgcolor="#EEEEEE">                                 
                                                            <td></td>
                                                            <td colspan="2" nowrap><b>Name</b></td>
                                                            <td><b>Type</b></td>
                                                            <td><b>Size (Bytes)</b></td>
                                                            <td><b>Permission</b></td>
                                                            <td><b>Last Modified</b></td>
                                                      </tr>
                                                   </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td></td>
                                                            <asp:Label ID="lblDeleted" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem,"[\"deleted\"]") %>'/>
                                                            <asp:Label ID="lblId" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem,"[\"docid\"]") %>' />
                                                            <asp:Label ID="lblOwnerId" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem,"[\"ownerid\"]") %>' />
                                                            <td nowrap><asp:ImageButton ID="imgType" runat="server" ImageUrl='<%# DataBinder.Eval(Container.DataItem, "[\"type\"]").ToString()== "Folder" ? "/images/folder24.gif": GetIcon(DataBinder.Eval(Container.DataItem, "[\"type\"]").ToString()) %>' /></td>
                                                            <td width="40%" nowrap><asp:LinkButton ID="btnName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "[\"name\"]") %>' CommandArgument='<%# DataBinder.Eval(Container.DataItem, "[\"path\"]")%>' OnClick="Name_Click" /></td> 
                                                            <td width="15%" nowrap><asp:Label ID="lblType" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "[\"type\"]") %>' /></td>
                                                            <td width="15%" nowrap><asp:Label ID="lblSize" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "[\"size\"]") %>' /></td>
                                                            <td width="15%" nowrap><%# DataBinder.Eval(Container.DataItem, "[\"permission\"]") %></td>
                                                            <td width="15%" nowrap><%# DataBinder.Eval(Container.DataItem, "[\"modified\"]") %></td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:repeater>
                                                </table>
                                            </td>
                                        </tr> 
                                   </ItemTemplate>   
                                   <AlternatingItemTemplate>
                                        <tr bgcolor="#F6F6F6">
                                            <td><img src="/images/folder24.gif" border="0" align="absmiddle" /></td>
                                            <td width="100%"><a href="javascript:void(0);" onclick="ShowHideDiv2('div<%# DataBinder.Eval(Container.DataItem, "applicationid") %>');"><%# DataBinder.Eval(Container.DataItem, "name") %> (<%# DataBinder.Eval(Container.DataItem, "docs") %>)</a></td>
                                        </tr>
                                        <tr bgcolor="#F6F6F6" id='div<%#DataBinder.Eval(Container.DataItem, "applicationid").ToString()%>' style="display:none">
                                            <td colspan="2">
                                              <table width="100%" cellpadding="5" cellspacing="0" border="0" class="default" style="border:solid 1px #CCCCCC">                                                
                                                <asp:repeater id="rptShared" datasource='<%# ((DataRowView)Container.DataItem).Row.GetChildRows("relationship") %>' runat="server">
                                                   <HeaderTemplate>                          
                                                      <tr bgcolor="#EEEEEE">                                 
                                                            <td></td>
                                                            <td colspan="2" nowrap><b>Name</b></td>
                                                            <td><b>Type</b></td>
                                                            <td><b>Size (Bytes)</b></td>
                                                            <td><b>Permission</b></td>
                                                            <td><b>Last Modified</b></td>
                                                      </tr>
                                                   </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td></td>
                                                            <asp:Label ID="lblDeleted" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem,"[\"deleted\"]") %>'/>
                                                            <asp:Label ID="lblId" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem,"[\"docid\"]") %>' />
                                                            <asp:Label ID="lblOwnerId" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container.DataItem,"[\"ownerid\"]") %>' />
                                                            <td nowrap><asp:ImageButton ID="imgType" runat="server" ImageUrl='<%# DataBinder.Eval(Container.DataItem, "[\"type\"]").ToString()== "Folder" ? "/images/folder24.gif": GetIcon(DataBinder.Eval(Container.DataItem, "[\"type\"]").ToString()) %>' /></td>
                                                            <td width="40%" nowrap><asp:LinkButton ID="btnName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "[\"name\"]") %>' CommandArgument='<%# DataBinder.Eval(Container.DataItem, "[\"path\"]")%>' OnClick="Name_Click" /></td> 
                                                            <td width="15%" nowrap><asp:Label ID="lblType" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "[\"type\"]") %>' /></td>
                                                            <td width="15%" nowrap><asp:Label ID="lblSize" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "[\"size\"]") %>' /></td>
                                                            <td width="15%" nowrap><%# DataBinder.Eval(Container.DataItem, "[\"permission\"]") %></td>
                                                            <td width="15%" nowrap><%# DataBinder.Eval(Container.DataItem, "[\"modified\"]") %></td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:repeater>
                                                </table>
                                            </td>
                                        </tr> 
                                   </AlternatingItemTemplate>   
                                </asp:Repeater> 
                                <tr><td colspan="20" class="default"><asp:Label ID="lblNoShares" runat="server" Visible="false" Text="<img src='/images/alert.gif' border='0' align='absmiddle'> There are no shared files" /></td></tr>
                              </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">&nbsp;</td>
                        </tr>
                    </table>                    
                </div> 
                </div.                         
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
                                    <td align="right"><asp:Button ID="btnClose" runat="server" CssClass="default" Width="75" Text="Close" /></td>
                                </tr>
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
<input type="hidden" id="hdnDir" runat="server" />
<input type="hidden" id="hdnType" runat="server" />
<asp:HiddenField ID="hdnSubmittedBy" runat="server" />
<asp:Label ID="lblCurrent" runat="server" Visible="false" />
