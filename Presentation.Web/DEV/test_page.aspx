<%@ Page Language="C#" MasterPageFile="~/clearview.Master" AutoEventWireup="true" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" Runat="Server">
<script runat="server">
        private DataSet ds;
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected string strTitle = ConfigurationManager.AppSettings["appTitle"];
        protected Applications oApplication;
        protected Pages oPage;
        protected AppPages oAppPage;
        protected PageControls oPageControl;
        protected Designer oDesigner;
        protected Settings oSetting;
        protected Users oUser;
        protected ServiceRequests oServiceRequest;
        protected int intProfile;
        protected int intApplication = 0;
        protected int intPage = 0;
        protected string strMenu = "";
        protected int intCount = 0;
        protected string strPage = "";
        protected string strPageRefresh = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        Control oControl;
        Page.Title = strTitle;
        if (Request.QueryString["down"] != null)
        {
            oControl = (Control)LoadControl("/controls/sys/sys_down.ascx");
            PH3.Controls.Add(oControl);
        }
        else
        {
            oControl = (Control)LoadControl("/controls/sys/sys_will_down.ascx");
            PHDown.Controls.Add(oControl);
            if (Request.Cookies["profileid"] == null || Request.Cookies["profileid"].Value == "")
            {
                Response.Cookies["userloginreferrer"].Value = Request.Url.PathAndQuery;
                oControl = (Control)LoadControl("/controls/sys/sys_login.ascx");
                PH3.Controls.Add(oControl);
            }
            else
            {
                intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
                oUser = new Users(intProfile, dsn);
                if (intProfile > 0)
                    lblName.Text = "<b>Welcome,</b> " + oUser.GetFullName(intProfile) + "&nbsp;&nbsp;(" + oUser.GetName(intProfile).ToUpper() + ")";
                oPage = new Pages(intProfile, dsn);
                oAppPage = new AppPages(intProfile, dsn);
                oApplication = new Applications(intProfile, dsn);
                oPageControl = new PageControls(intProfile, dsn);
                oDesigner = new Designer(intProfile, dsn);
                oServiceRequest = new ServiceRequests(intProfile, dsn);
                DataSet dsReturned = oServiceRequest.GetReturned(intProfile);
                if (dsReturned.Tables[0].Rows.Count > 0)
                {
                    oControl = (Control)LoadControl("/controls/sys/sys_returned.ascx");
                    PH3.Controls.Add(oControl);
                }
                oControl = (Control)LoadControl("/controls/sys/sys_topnav.ascx");
                PH1.Controls.Add(oControl);
                /*
                oControl = (Control)LoadControl("/controls/sys/sys_leftnav.ascx");
                PH2.Controls.Add(oControl);
                */
                intApplication = 0;
                intPage = 6;
                if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                    intApplication = Int32.Parse(Request.QueryString["applicationid"]);
                if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                    intPage = Int32.Parse(Request.QueryString["pageid"]);
                if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                    intApplication = Int32.Parse(Request.Cookies["application"].Value);

                AddPages(intApplication);
                
                string strVariables = "";
                foreach (string strName in Request.QueryString)
                {
                    if (strName != "pageid" && strName != "apppageid")
                        strVariables += (strVariables == "" ? "?" : "&") + strName + "=" + Request.QueryString[strName];
                }
                if (intPage > 0)
                    strPage = oPage.GetFullLink(intPage).Substring(1) + strVariables;
                if (intApplication > 0 || intPage > 0)
                {
                    if (intApplication > 0)
                    {
                        ds = oApplication.Get(intApplication);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            this.Page.Title = "ClearView | " + ds.Tables[0].Rows[0]["name"].ToString();
                        }
                    }
                    if (intPage > 0)
                    {
                        this.Page.Title = "ClearView | " + oPage.Get(intPage, "browsertitle");
                        // Load Page Controls
                        ds = oPageControl.GetPage(intPage, 1);
                        if (ds.Tables[0].Rows.Count == 0)
                        {
                            oControl = (Control)LoadControl("/controls/sys/sys_pages.ascx");
                            PH3.Controls.Add(oControl);
                        }
                        else
                        {
                            ContentPlaceHolder oPlaceHolder;
                            oPlaceHolder = (ContentPlaceHolder)Master.FindControl("AllContent");
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                Control oTempControl = oPlaceHolder.FindControl(dr["placeholder"].ToString());
                                if (oTempControl != null)
                                {
                                    oControl = LoadControl(Request.ApplicationPath + dr["path"].ToString());
                                    oTempControl.Controls.Add(oControl);
                                }
                            }
                        }

                        // Load Refresh (if applicable)
                        string strRefresh = "0";
                        DataSet dsRefresh = oAppPage.Get(intPage, intApplication);
                        if (dsRefresh.Tables[0].Rows.Count > 0)
                            strRefresh = dsRefresh.Tables[0].Rows[0]["refresh"].ToString();
                        if (strRefresh != "" && strRefresh != "0")
                        {
                            // Add refresh to page (strRefresh should be in SECONDS)
                            strPageRefresh = "<meta http-equiv=\"refresh\" content=\"" + strRefresh + "\">";
                        }
                    }
                    else
                    {
                        // Load User's Home Page Controls
                        oControl = (Control)LoadControl("/controls/sys/sys_personal.ascx");
                        PH3.Controls.Add(oControl);
                        //oControl = (Control)LoadControl("/controls/sys/sys_new.ascx");
                        oControl = (Control)LoadControl("/controls/sys/sys_whatsnew.ascx");
                        PH3.Controls.Add(oControl);
                        ds = oDesigner.Get(intProfile, 1);
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            oControl = (Control)LoadControl(dr["path"].ToString());
                            PH3.Controls.Add(oControl);
                        }
                    }
                }
                else
                {
                    // Load Available Applications
                    oControl = (Control)LoadControl("/controls/sys/sys_application.ascx");
                    PH3.Controls.Add(oControl);
                }
            }
        }
        oControl = (Control)LoadControl("/controls/sys/sys_rotator_header.ascx");
        PH4.Controls.Add(oControl);
    }
    protected void AddPages(int _application)
    {
        DataSet ds = oPage.Gets(_application, intProfile, 0, 1, 1);
        int _parent = oPage.GetParent(intPage);
        StringBuilder sb = new StringBuilder(strMenu);

        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            int _page = Int32.Parse(dr["pageid"].ToString());
            string strShow = "none";
            //string strSubMenu = (_page == intPage || _page == _parent ? AddPages(_page, _application) : "");
            string strSubMenu = AddPages(_page, _application);
            if ((_page == intPage && strSubMenu != "") || (_parent == _page && strSubMenu != "") || (_page == oPage.GetParent(_parent) && strSubMenu != ""))
            {
                strShow = "inline";
            }

            intCount++;
            sb.Append("<tr>");
            sb.Append("<td><img src=\"/images/table_topLeft.gif\" border=\"0\" width=\"5\" height=\"26\"></td>");

            string strHelp = oPage.Get(_page, "tooltip");
            if (strHelp != "")
            {
                strHelp = " title=\"" + strHelp + "\"";
            }

            sb.Append("<td nowrap background=\"/images/table_top.gif\" width=\"100%\"");
            sb.Append(strHelp);
            sb.Append("><img src=\"");
            sb.Append(dr["navimage"].ToString() == "" ? "/images/spacer.gif" : dr["navimage"].ToString());
            sb.Append("\" border=\"0\" align=\"absmiddle\" width=\"24\" height=\"24\" /> <a ");
            sb.Append(oPage.GetHref(_page));
            sb.Append(" class=\"greentableheader\">");
            sb.Append(oPage.Get(_page, "menutitle"));
            sb.Append("</a></td>");
            sb.Append("<td><img src=\"/images/table_topRight.gif\" border=\"0\" width=\"5\" height=\"26\"></td>");
            sb.Append("</tr>");
            sb.Append("<tr id=\"div");
            sb.Append(intCount.ToString());
            sb.Append("t\" style=\"display:");
            sb.Append(strShow);
            sb.Append("\">");
            sb.Append("<td background=\"/images/table_left.gif\"><img src=\"/images/table_left.gif\" width=\"5\" height=\"10\"></td>");
            sb.Append("<td width=\"100%\" bgcolor=\"#FFFFFF\">");
            sb.Append(strSubMenu);
            sb.Append("</td>");
            sb.Append("<td background=\"/images/table_right.gif\"><img src=\"/images/table_right.gif\" width=\"5\" height=\"10\"></td>");
            sb.Append("</tr>");
            sb.Append("<tr id=\"div");
            sb.Append(intCount.ToString());
            sb.Append("b\" style=\"display:");
            sb.Append(strShow);
            sb.Append("\">");
            sb.Append("<td><img src=\"/images/table_bottomLeft.gif\" border=\"0\" width=\"5\" height=\"9\"></td>");
            sb.Append("<td width=\"100%\" background=\"/images/table_bottom.gif\"></td>");
            sb.Append("<td><img src=\"/images/table_bottomRight.gif\" border=\"0\" width=\"5\" height=\"9\"></td>");
            sb.Append("</tr>");
            sb.Append("<tr><td colspan=\"3\" height=\"5\"><img src=\"/images/spacer.gif\" border=\"0\" height=\"5\" width=\"1\"></td></tr>");
        }

        strMenu = sb.ToString();
    }
    protected string AddPages(int _parent, int _application)
    {
        StringBuilder sb = new StringBuilder();

        DataSet ds = oPage.Gets(_application, intProfile, _parent, 1, 1);
        Response.Write(_parent.ToString() + " = " + ds.Tables[0].Rows.Count.ToString() + " (" + DateTime.Now.ToString() + ")" + "<br/>");
        int intRow = 0;
        int intTotal = ds.Tables[0].Rows.Count;
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            intRow++;
            if (intRow == 1)
            {
                sb.Append("<tr>");
                sb.Append("<td height=\"5\" colspan=\"3\"><img src=\"/images/spacer.gif\" width=\"1\" height=\"5\"></td>");
                sb.Append("</tr>");
            }
            int intChild = Int32.Parse(dr["pageid"].ToString());
            string strHelp = oPage.Get(intChild, "tooltip");
            if (strHelp != "")
                strHelp = " title=\"" + strHelp + "\"";
            sb.Append("<tr");
            sb.Append(strHelp);
            sb.Append(">");
            sb.Append("<td><img src=\"/images/spacer.gif\" width=\"5\" height=\"5\"></td>");
            sb.Append("<td valign=\"top\">&nbsp;<img src=\"/images/menu.gif\" border=\"0\" align=\"absmiddle\"/></td>");
            string strTotal = "";

            if (oPage.Get(intChild, "sproc") != "")
            {
                Response.Write(intChild.ToString() + "...." + oPage.Get(intChild, "sproc") + " started on " + DateTime.Now.ToString() + "<br/>");
                DataSet dsTotal = oPage.GetTotal(intProfile, oPage.Get(intChild, "sproc"));
                if (dsTotal.Tables[0].Rows.Count > 0)
                    strTotal = "<span class=\"leftnavp\"> (" + dsTotal.Tables[0].Rows.Count + ")</span>";
                Response.Write(intChild.ToString() + "...." + oPage.Get(intChild, "sproc") + " = " + dsTotal.Tables[0].Rows.Count + " (" + DateTime.Now.ToString() + ")" + "<br/>");
            }

            sb.Append("<td valign=\"top\" width=\"100%\"><a ");
            sb.Append(oPage.GetHref(intChild));
            sb.Append(" class=\"leftnav\">");
            sb.Append(oPage.Get(intChild, "menutitle"));
            sb.Append(strTotal);
            sb.Append("</a></td>");
            sb.Append("</tr>");

            if (intRow < intTotal)
            {
                sb.Append("<tr>");
                sb.Append("<td height=\"10\" colspan=\"3\" background=\"/images/gray_dot.gif\"><img src=\"/images/gray_dot.gif\" width=\"1\" height=\"10\"></td>");
                sb.Append("</tr>");
            }
        }
        if (sb.ToString() != "")
        {
            sb.Insert(0, "<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\">");
            sb.Append("</table>");
        }

        return sb.ToString();
    }
</script>
<script type="text/javascript">
</script>
<table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0">
    <tr height="1" style="display:none"> 
        <td bgcolor="#007253"><img src="/images/header_wide.aspx" border="0" /></td>
        <td bgcolor="#007253" align="right"><asp:PlaceHolder ID="PH4" runat="server" /></td>
    </tr>
    <tr height="1"> 
        <td colspan="2">
            <table width="100%" border="0" cellspacing="0" cellpadding="5">
                <tr style="background-color:#FFFFFF; display:inline">
                    <td><img src="/images/PNCHeaderLogo.gif" border="0" /></td>
                    <td align="right"><img src="/images/HeaderGradient.gif" border="0" /></td>
                </tr>
                <tr style="background-color:#FFFFFF; display:none">
                    <td background="/images/PNCLogoBack.gif" width="100%"><img src="/images/PNCLogo.gif" border="0" /></td>
                    <td align="right"><img src="/images/HeaderGradient.gif" border="0" /></td>
                </tr>
                <tr>
                    <td colspan="2" align="right" class="whitedefault"><DIV id=thinOrangeBar><asp:label ID="lblName" runat="server" CssClass="whitedefault" />&nbsp;&nbsp;</DIV></td>
                </tr>
            </table>
        </td>
    </tr>
    <tr bgcolor="#000000" height="1"> 
        <td colspan="2" height="26" background="/images/button_back.gif">
            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td><asp:PlaceHolder ID="PH1" runat="server" /></td>
                    <td align="right"></td>
                </tr>
            </table>
        </td>
    </tr>
    <tr height="1"> 
        <td colspan="2" bgcolor="#E9E9E9"><asp:PlaceHolder ID="PHDown" runat="server" /></td>
    </tr>
    <tr> 
        <td colspan="2" bgcolor="#E9E9E9">
            <table width="98%" height="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                <tr height="1">
                    <td align="left" valign="top"><img src="/images/spacer.gif" width="180" height="1" /></td>
                    <td align="left" valign="top"><img src="/images/spacer.gif" width="12" height="1" /></td>
                    <td align="left" valign="top">&nbsp;</td>
                </tr>
                <tr> 
                    <td width="180" align="left" valign="top">
                        <asp:PlaceHolder ID="PH2" runat="server" />
                    </td>
                    <td width="12" align="left" valign="top">&nbsp;</td>
                    <td width="100%" align="left" valign="top">
                        <asp:PlaceHolder ID="PH3" runat="server" />
                        <p>&nbsp;</p>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
<script type="text/javascript">EnablePostBack('<%=strPage%>','<%=Request.Path%>');</script>
</asp:Content>