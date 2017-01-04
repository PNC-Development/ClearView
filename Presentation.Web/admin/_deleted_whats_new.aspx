<%@ Page Language="C#" %>
<script runat="server">
    private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
    private New oNew;
    private int intProfile;
    private int intID;
    private void Page_Load()
    {
        Response.Cookies["loginreferrer"].Value = "/admin/whats_new.aspx";
        Response.Cookies["loginreferrer"].Expires = DateTime.Now.AddDays(30);
        if (Request.Cookies["adminid"] != null && Request.Cookies["adminid"].Value != "")
            intProfile = Int32.Parse(Request.Cookies["adminid"].Value);
        else
            Response.Redirect("/admin/login.aspx");
        oNew = new New(intProfile, dsn);
        if (Request.QueryString["id"] == null)
        {
            panView.Visible = true;
            LoopRepeater();
        }
        else
        {
            panAdd.Visible = true;
            intID = Int32.Parse(Request.QueryString["id"]);
            if (!IsPostBack)
            {
                DataSet ds = oNew.Get(intID);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtTitle.Text = ds.Tables[0].Rows[0]["title"].ToString();
                    txtDescription.Text = ds.Tables[0].Rows[0]["description"].ToString();
                    txtAttachment.Text = ds.Tables[0].Rows[0]["attachment"].ToString();
                    chkEnabled.Checked = (ds.Tables[0].Rows[0]["enabled"].ToString() == "1");
                    btnOrder.Attributes.Add("onclick", "return OpenWindow('SUPPORTORDER','" + hdnId.ClientID + "','" + hdnOrder.ClientID + "&type=NEW" + "',false,400,400);");
                    btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this item?');");
                }
                else
                {
                    btnOrder.Enabled = false;
                    btnDelete.Enabled = false;
                }
            }
        }
        btnBrowse.Attributes.Add("onclick", "return OpenWindow('FILEBROWSER','" + txtAttachment.ClientID + "','',false,400,600);");
    }
    private void LoopRepeater()
    {
        DataSet ds = oNew.Gets(0);
        DataView dv = ds.Tables[0].DefaultView;
        if (Request.QueryString["sort"] != null)
            dv.Sort = Request.QueryString["sort"].ToString();
        rptView.DataSource = dv;
        rptView.DataBind();
        foreach (RepeaterItem ri in rptView.Items)
        {
            ImageButton oDelete = (ImageButton)ri.FindControl("btnDelete");
            oDelete.Attributes.Add("onClick", "return confirm('Are you sure you want to delete this item?');");
            ImageButton oEnable = (ImageButton)ri.FindControl("btnEnable");
            if (oEnable.ImageUrl == "/admin/images/enabled.gif")
            {
                oEnable.ToolTip = "Click to disable";
                oEnable.Attributes.Add("onClick", "return confirm('Are you sure you want to disable this item?');");
            }
            else
                oEnable.ToolTip = "Click to enable";
        }
    }
    public void OrderView(Object Sender, EventArgs e)
    {
        LinkButton oButton = (LinkButton)Sender;
        string strSort;
        if (Request.QueryString["sort"] == null)
            strSort = oButton.CommandArgument + " ASC";
        else
            if (Request.QueryString["sort"].ToString() == (oButton.CommandArgument + " ASC"))
                strSort = oButton.CommandArgument + " DESC";
            else
                strSort = oButton.CommandArgument + " ASC";
        Response.Redirect(Request.Path + "?sort=" + strSort);
    }
    public void btnAdd_Click(Object Sender, EventArgs e)
    {
        if (intID == 0)
            oNew.Add(txtTitle.Text, txtDescription.Text, txtAttachment.Text, (oNew.Gets(0).Tables[0].Rows.Count + 1), (chkEnabled.Checked ? 1 : 0));
        else
            oNew.Update(intID, txtTitle.Text, txtDescription.Text, txtAttachment.Text, (chkEnabled.Checked ? 1 : 0));
        if (Request.Form[hdnOrder.UniqueID] != "")
        {
            string strOrder = Request.Form[hdnOrder.UniqueID];
            int intCount = 0;
            while (strOrder != "")
            {
                intCount++;
                int intId = Int32.Parse(strOrder.Substring(0, strOrder.IndexOf("&")));
                strOrder = strOrder.Substring(strOrder.IndexOf("&") + 1);
                oNew.UpdateOrder(intId, intCount);
            }
        }
        Response.Redirect(Request.Path);
    }
    public void btnEnable_Click(Object Sender, ImageClickEventArgs e)
    {
        ImageButton oButton = (ImageButton)Sender;
        oNew.Enable(Int32.Parse(oButton.CommandArgument), (oButton.ImageUrl == "/admin/images/enabled.gif" ? 0 : 1));
        Response.Redirect(Request.Path);
    }
    public void btnDeleteLink_Click(Object Sender, ImageClickEventArgs e)
    {
        ImageButton oButton = (ImageButton)Sender;
        oNew.Delete(Int32.Parse(oButton.CommandArgument));
        Response.Redirect(Request.Path);
    }
    public void btnNew_Click(Object Sender, EventArgs e)
    {
        Response.Redirect(Request.Path + "?id=0");
    }
    public void btnCancel_Click(Object Sender, EventArgs e)
    {
        Response.Redirect(Request.Path);
    }
    public void btnDelete_Click(Object Sender, EventArgs e)
    {
        oNew.Delete(Int32.Parse(Request.Form[hdnId.UniqueID]));
        Response.Redirect(Request.Path);
    }
</script>
<script type="text/javascript">
    function Edit(strId) {
        var strURL = '<%=Request.Path %>';
        window.navigate(strURL + "?id=" + strId);
    }
</script>
<html>
<head>
<title>ClearView Administration</title>
<link rel="stylesheet" type="text/css" href="/css/default.css" />
<script type="text/javascript" src="/javascript/global.js"></script>
</head>
<body topmargin="0" leftmargin="0">
<form id="Form1" runat="server">
        <table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
        <td height="100%">
        <div style="height:100%; overflow:auto">
<table width="98%" cellpadding="0" cellspacing="0" border="0" align="center">
		<tr><td colspan="2">&nbsp;</td></tr>
		<tr> 
		    <td><b>What's New</b></td>
		    <td align="right"><asp:LinkButton ID="btnNew" runat="server" Text="Add New" OnClick="btnNew_Click" /></td>
		</tr>
		<tr><td colspan="2">&nbsp;</td></tr>
		<tr>
		    <td colspan="2" align="center">
                <asp:Panel ID="panView" runat="server" Visible="false">
                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <asp:repeater ID="rptView" runat="server">
                                    <HeaderTemplate>
                                        <table width="100%" cellpadding="3" cellspacing="0" border="0" align="center">
                                            <tr bgcolor="#CCCCCC">
                                                <td class="bold">&nbsp;</td>
                                                <td width="50%" class="bold"><asp:linkbutton ID="lnkName" Text="Name" CommandArgument="name" CssClass="bold" OnClick="OrderView" runat="server"/></td>
                                                <td width="50%" class="bold"><asp:linkbutton ID="lnkModified" Text="Last Modified" CommandArgument="modified" CssClass="bold" OnClick="OrderView" runat="server"/></td>
                                                <td class="bold" align="center"><asp:linkbutton ID="lnkEnabled" Text="Enabled" CommandArgument="enabled" CssClass="bold" OnClick="OrderView" runat="server"/></td>
                                            </tr>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr bgcolor="#EFEFEF" onmouseover="AdminMouseOver(this);" onmouseout="AdminMouseOut(this);" onclick="if(event.srcElement.tagName != 'INPUT')Edit('<%# DataBinder.Eval(Container.DataItem, "id") %>');" class="default">
                                            <td width="5%" align="left">&nbsp;<asp:ImageButton ID="btnDelete" OnClick="btnDeleteLink_Click" runat="server" ToolTip="Delete" ImageAlign="AbsMiddle" ImageUrl="/admin/images/delete.gif" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' /></td>
                                            <td width="50%">&nbsp;<%# DataBinder.Eval(Container.DataItem, "title") %></td>
                                            <td width="50%">&nbsp;<%# DataBinder.Eval(Container.DataItem, "modified") %></td>
                                            <td align="center"><asp:ImageButton ID="btnEnable" runat="server" ImageAlign="AbsMiddle" ImageUrl='<%# (DataBinder.Eval(Container.DataItem, "enabled").ToString() == "1") ? "/admin/images/enabled.gif" : "/admin/images/disabled.gif" %>' CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' OnClick="btnEnable_Click" /></td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </table>
                                    </FooterTemplate>
                                </asp:repeater>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>

                <asp:Panel id="panAdd" runat="server" Visible="false">
                    <table width="95%" cellpadding="3" cellspacing="0" border="0" align="center">
                        <tr> 
                            <td class="default" width="100px">Title:</td>
                            <td><asp:textbox ID="txtTitle" CssClass="default" runat="server" Width="90%" MaxLength="100"/></td>
                        </tr>
                        <tr> 
                            <td class="default" width="100px">Description:</td>
                            <td><asp:textbox ID="txtDescription" CssClass="default" runat="server" Width="90%" TextMode="MultiLine" Rows="15" /></td>
                        </tr>
                        <tr> 
                            <td class="default" width="100px">Attachment:</td>
                            <td><asp:textbox ID="txtAttachment" CssClass="default" runat="server" Width="90%" MaxLength="100"/> <asp:button ID="btnBrowse" runat="server" Text="..." Width="25" CssClass="default"/></td>
                        </tr>
                        <tr> 
                            <td class="default">Enabled:</td>
                            <td><asp:CheckBox ID="chkEnabled" runat="server" Checked="true" /></td>
                        </tr>
                        <tr> 
                            <td class="cmdefault">&nbsp;</td>
                            <td><asp:Button ID="btnOrder" runat="server" Text="Change Order" Width="150" CssClass="default" /></td>
                        </tr>
                        <tr><td height="5" colspan="2">&nbsp;</td></tr>
                        <tr> 
                            <td>&nbsp;</td>
                            <td>
                                <asp:button ID="btnAdd" CssClass="default" runat="server" Text="Add" Width="75" OnClick="btnAdd_Click" />
                                <asp:button ID="btnDelete" CssClass="default" runat="server" Text="Delete" Width="75" OnClick="btnDelete_Click" />
                                <asp:button ID="btnCancel" CssClass="default" runat="server" Text="Cancel" Width="75" OnClick="btnCancel_Click" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
    </table>
        </div>
        </td>
        </tr>
        </table>
    <input type="hidden" id="hdnId" runat="server" />
    <input type="hidden" id="hdnOrder" runat="server" />
</form>
</body>
</html>
