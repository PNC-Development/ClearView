<%@ Page Language="C#" MasterPageFile="~/frame.Master" AutoEventWireup="true" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="AllContent" runat="server">
<script runat="server">
    private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
    private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
    private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
    protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
    protected int intProfile;
    protected Forecast oForecast;
    protected Users oUser;
    protected Functions oFunction;
    protected Variables oVariable;

    protected int intID;

    protected void Page_Load(object sender, EventArgs e)
    {
        intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
        oForecast = new Forecast(intProfile, dsn);
        oUser = new Users(intProfile, dsn);
        oFunction = new Functions(intProfile, dsn, intEnvironment);
        oVariable = new Variables(intEnvironment);

        if (Request.QueryString["id"] != null)
            Int32.TryParse(Request.QueryString["id"], out intID);
        Page.Title = "ClearView Account Configuration";
        
        if (!IsPostBack)
        {
            if (intID > 0)
            {
                rptAccounts.DataSource = GetAccounts();
                rptAccounts.DataBind();
                foreach (RepeaterItem ri in rptAccounts.Items)
                {
                    string strPermissions = "";
                    Label _permissions = (Label)ri.FindControl("lblPermissions");
                    switch (_permissions.Text)
                    {
                        case "0":
                            _permissions.Text = "-----";
                            break;
                        case "D":
                            _permissions.Text = "Developer";
                            break;
                        case "P":
                            _permissions.Text = "Promoter";
                            break;
                        case "S":
                            _permissions.Text = "AppSupport";
                            break;
                        case "U":
                            _permissions.Text = "AppUsers";
                            break;
                    }
                    if (_permissions.ToolTip == "1")
                        _permissions.Text += " (R/D)";
                }
                if (rptAccounts.Items.Count == 0)
                    lblNone.Visible = true;
            }
        }
    }
    public DataSet GetAccounts()
    {
        SqlParameter[] arParams = new SqlParameter[1];
        arParams[0] = new SqlParameter("@designid", intID);
        return SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT * FROM cv_accenture_accounts WHERE designid = @designid AND deleted = 0", arParams);
    }
    
    
</script>
<script type="text/javascript">
</script>
<table width="100%" cellpadding="5" cellspacing="2" border="0">
    <tr>
        <td class="bigger" colspan="2"><b>Account Configuration</b></td>
    </tr>
    <tr>
        <td colspan="2">
            <table width="100%" cellpadding="4" cellspacing="0" border="0" style="border:solid 1px #CCCCCC">
                <tr bgcolor="#EEEEEE">
                    <td><b><u>User:</u></b></td>
                    <td><b><u>Permission:</u></b></td>
                </tr>
                <asp:repeater ID="rptAccounts" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td valign="top"><%# oUser.GetFullName(Int32.Parse(DataBinder.Eval(Container.DataItem, "userid").ToString())) %> (<%# oUser.GetName(Int32.Parse(DataBinder.Eval(Container.DataItem, "userid").ToString()))%>)</td>
                            <td valign="top"><asp:Label ID="lblPermissions" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "access") %>' ToolTip='<%# DataBinder.Eval(Container.DataItem, "remote") %>' /></td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr bgcolor="F6F6F6">
                            <td valign="top"><%# oUser.GetFullName(Int32.Parse(DataBinder.Eval(Container.DataItem, "userid").ToString()))%> (<%# oUser.GetName(Int32.Parse(DataBinder.Eval(Container.DataItem, "userid").ToString()))%>)</td>
                            <td valign="top"><asp:Label ID="lblPermissions" runat="server" CssClass="default" Text='<%# DataBinder.Eval(Container.DataItem, "access") %>' ToolTip='<%# DataBinder.Eval(Container.DataItem, "remote") %>' /></td>
                        </tr>
                    </AlternatingItemTemplate>
                </asp:repeater>
                <tr>
                    <td colspan="2">
                        <asp:Label ID="lblNone" runat="server" CssClass="default" Visible="false" Text="<img src='/images/spacer.gif' border='0' height='1' width='25' /><img src='/images/alert.gif' border='0' align='absmiddle'> You have not added any accounts to this request" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
</asp:Content>
