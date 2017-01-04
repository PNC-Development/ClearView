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
                LoadStorage();
            }
        }
    }
    protected void LoadStorage()
    {
        DataSet dsStorage = GetStorage();
        rptStorage.DataSource = dsStorage;
        rptStorage.DataBind();
        foreach (RepeaterItem ri in rptStorage.Items)
        {
            CheckBox _shared = (CheckBox)ri.FindControl("chkStorageSize");
            _shared.Checked = (_shared.Text == "1");
            _shared.Text = "";
        }
        if (Get(intID, "os") == "W")
        {
            trStorageApp.Visible = true;
            DataSet dsApp = GetStorage(-1000);
            if (dsApp.Tables[0].Rows.Count > 0)
            {
                int intTemp = 0;
                if (Int32.TryParse(dsApp.Tables[0].Rows[0]["size"].ToString(), out intTemp) == true)
                    txtStorageSizeE.Text = intTemp.ToString();
            }
        }
    }
    public DataSet GetStorage()
    {
        SqlParameter[] arParams = new SqlParameter[1];
        arParams[0] = new SqlParameter("@designid", intID);
        return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getAccentureStorages", arParams);
    }
    public DataSet GetStorage(int intDrive)
    {
        SqlParameter[] arParams = new SqlParameter[2];
        arParams[0] = new SqlParameter("@designid", intID);
        arParams[1] = new SqlParameter("@driveid", intDrive);
        return SqlHelper.ExecuteDataset(dsn, CommandType.StoredProcedure, "pr_getAccentureStorage", arParams);
    }
    public DataSet Get(int _id)
    {
        SqlParameter[] arParams = new SqlParameter[1];
        arParams[0] = new SqlParameter("@id", _id);
        return SqlHelper.ExecuteDataset(dsn, CommandType.Text, "SELECT * FROM cv_accenture WHERE id = @id AND deleted = 0", arParams);
    }
    public string Get(int _id, string _column)
    {
        DataSet ds = Get(_id);
        if (ds.Tables[0].Rows.Count > 0)
            return ds.Tables[0].Rows[0][_column].ToString();
        else
            return "";
    }
    
    
</script>
<script type="text/javascript">
</script>
<table width="100%" cellpadding="5" cellspacing="2" border="0">
    <tr>
        <td class="bigger" colspan="2"><b>Storage Configuration</b></td>
    </tr>
    <tr>
        <td colspan="2">
            <table width="100%" cellpadding="4" cellspacing="0" border="0" align="center" style="border:solid 1px #CCCCCC">
                <tr bgcolor="#EEEEEE">
                    <td width="100%"><b><u>Path:</u></b></td>
                    <td nowrap><b><u>Shared:</u></b></td>
                    <td nowrap><b><u>Size:</u></b></td>
                </tr>
                <tr id="trStorageApp" runat="server" visible="false">
                    <td width="100%">E:\**&nbsp;&nbsp;&nbsp;&nbsp;<span class="footer">(Reserved: Application Drive)</span></td>
                    <td nowrap><asp:CheckBox ID="chkStorageSizeE" runat="server" Enabled="false" Checked="false" /></td>
                    <td nowrap><asp:Label ID="txtStorageSizeE" runat="server" /> GB</td>
                </tr>
                <asp:repeater ID="rptStorage" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td valign="top" width="100%"><%# DataBinder.Eval(Container.DataItem, "letter") %><%# DataBinder.Eval(Container.DataItem, "path") %></td>
                            <td valign="top" nowrap><asp:CheckBox ID="chkStorageSize" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "shared") %>' Enabled="false" /></td>
                            <td valign="top" nowrap><%# DataBinder.Eval(Container.DataItem, "size") %> GB</td>
                        </tr>
                    </ItemTemplate>
                </asp:repeater>
            </table>
            <div align="right">** = If you do not require an application drive, set it to zero (0) GB.</div>
        </td>
    </tr>
</table>
</asp:Content>
