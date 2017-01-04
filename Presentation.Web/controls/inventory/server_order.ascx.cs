using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using NCC.ClearView.Application.Core;
namespace NCC.ClearView.Presentation.Web
{
    public partial class server_order : System.Web.UI.UserControl
    {

        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intLocation = Int32.Parse(ConfigurationManager.AppSettings["OPSLocationID"]);
        protected Pages oPage;
        protected Users oUser;
        protected Platforms oPlatform;
        protected Asset oAsset;
        protected IPAddresses oIPAddresses;
        protected Locations oLocation;
        protected Host oHost;
        protected Forecast oForecast;
        protected ModelsProperties oModelsProperties;
        protected Models oModel;
        protected Types oType;
        protected Confidence oConfidence;
        protected Classes oClass;
        protected Orders oOrder;
        protected Settings oSetting;
        protected int intProfile;
        protected int intApplication = 0;
        protected int intPage = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oPage = new Pages(intProfile, dsn);
            oUser = new Users(intProfile, dsn);
            oPlatform = new Platforms(intProfile, dsn);
            oAsset = new Asset(intProfile, dsnAsset);
            oIPAddresses = new IPAddresses(intProfile, dsnIP, dsn);
            oLocation = new Locations(intProfile, dsn);
            oHost = new Host(intProfile, dsn);
            oForecast = new Forecast(intProfile, dsn);
            oModelsProperties = new ModelsProperties(intProfile, dsn);
            oModel = new Models(intProfile, dsn);
            oType = new Types(intProfile, dsn);
            oConfidence = new Confidence(intProfile, dsn);
            oClass = new Classes(intProfile, dsn);
            oOrder = new Orders(intProfile, dsnAsset);
            oSetting = new Settings(intProfile, dsn);
            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            if (Request.QueryString["order_add"] != null)
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "order_add", "<script type=\"text/javascript\">alert('Device(s) successfully ordered');<" + "/" + "script>");
            if (Request.QueryString["order_edit"] != null)
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "order_edit", "<script type=\"text/javascript\">alert('Order successfully updated');<" + "/" + "script>");
            if (Request.QueryString["order_delete"] != null)
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "order_delete", "<script type=\"text/javascript\">alert('Order successfully deleted');<" + "/" + "script>");
            if (!IsPostBack)
            {
                int intPlatform = 0;
               
                if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                    intPlatform = Int32.Parse(Request.QueryString["id"]);
                if (intPlatform > 0)
                {
                    int intOrderAddress = 0;
                    LoadLists(intPlatform);
                    if (Request.QueryString["orderid"] != null && Request.QueryString["orderid"] != "")
                    {
                        panHost.Visible = false;
                        panOrders.Visible = false;
                        panOrderUpdate.Visible = true;
                        int intOrder = Int32.Parse(Request.QueryString["orderid"]);
                        DataSet dsOrder = oOrder.Get(intOrder);
                        txtOrderName.Text = dsOrder.Tables[0].Rows[0]["name"].ToString();
                        txtOrderTracking.Text = dsOrder.Tables[0].Rows[0]["tracking"].ToString();
                        ddlOrderModel.SelectedValue = dsOrder.Tables[0].Rows[0]["modelid"].ToString();
                        intOrderAddress = Int32.Parse(dsOrder.Tables[0].Rows[0]["addressid"].ToString());
                        txtOrderAddress.Text = oLocation.GetFull(intOrderAddress);
                        hdnOrderAddress.Value = intOrderAddress.ToString();
                        int intOrderClass = Int32.Parse(dsOrder.Tables[0].Rows[0]["classid"].ToString());
                        ddlOrderClass.SelectedValue = intOrderClass.ToString();
                        int intOrderEnv = Int32.Parse(dsOrder.Tables[0].Rows[0]["environmentid"].ToString());
                        ddlOrderEnvironment.Enabled = true;
                        ddlOrderEnvironment.DataTextField = "name";
                        ddlOrderEnvironment.DataValueField = "id";
                        ddlOrderEnvironment.DataSource = oClass.GetEnvironment(intOrderClass, 0);
                        ddlOrderEnvironment.DataBind();
                        ddlOrderEnvironment.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                        ddlOrderEnvironment.SelectedValue = intOrderEnv.ToString();
                        hdnOrderEnvironment.Value = intOrderEnv.ToString();
                        txtOrderQuantity.Text = dsOrder.Tables[0].Rows[0]["quantity"].ToString();
                        txtOrderDate.Text = DateTime.Parse(dsOrder.Tables[0].Rows[0]["ordered"].ToString()).ToShortDateString();
                        ddlOrderStatus.SelectedValue = dsOrder.Tables[0].Rows[0]["status"].ToString();
                        txtOrderComments.Text = dsOrder.Tables[0].Rows[0]["comments"].ToString();
                        btnAddOrder.Visible = false;
                        btnUpdateOrder.Attributes.Add("onclick", "return ValidateText('" + txtOrderTracking.ClientID + "','Please enter a tracking number')" +
                            " && ValidateDropDown('" + ddlOrderModel.ClientID + "','Please select a model')" +
                            " && ValidateHidden0('" + hdnOrderAddress.ClientID + "','" + txtOrderAddress.ClientID + "','Please select a location')" +
                            " && ValidateDropDown('" + ddlOrderClass.ClientID + "','Please select a class')" +
                            " && ValidateDropDown('" + ddlOrderEnvironment.ClientID + "','Please select an environment')" +
                            " && ValidateNumber0('" + txtOrderQuantity.ClientID + "','Please enter a valid quantity')" +
                            " && ValidateDate('" + txtOrderDate.ClientID + "','Please enter a valid order date')" +
                            " && ProcessButton(this)" +
                            ";");
                    }
                    else
                    {
                        intOrderAddress = intLocation;
                        rptOrders.DataSource = oOrder.Gets(intPlatform);
                        rptOrders.DataBind();
                        lblNoOrders.Visible = (rptOrders.Items.Count == 0);
                        foreach (RepeaterItem ri in rptOrders.Items)
                        {
                            Label lblQuantity = (Label)ri.FindControl("lblQuantity");
                            Label lblReceived = (Label)ri.FindControl("lblReceived");
                            int intQuantity = Int32.Parse(lblQuantity.Text);
                            int intReceived = Int32.Parse(lblReceived.Text);
                            if (intReceived > 0)
                            {
                                if (intQuantity == intReceived)
                                {
                                    Button btnRemoveOrder = (Button)ri.FindControl("btnRemoveOrder");
                                    btnRemoveOrder.Attributes.Add("onclick", "return confirm('Are you sure you want to remove this order?\\n\\nNOTE: This does not cancel the order. It simply removes it from this view.');");
                                    btnRemoveOrder.Enabled = true;
                                }
                                Panel panRemove = (Panel)ri.FindControl("panRemove");
                                panRemove.Visible = true;
                            }
                            else
                            {
                                Panel panEdit = (Panel)ri.FindControl("panEdit");
                                panEdit.Visible = true;
                                Button btnDeleteOrder = (Button)ri.FindControl("btnDeleteOrder");
                                btnDeleteOrder.Attributes.Add("onclick", "return confirm('Are you sure you want to cancel this order?');");
                            }
                        }
                        btnUpdateOrder.Visible = false;
                        btnAddOrder.Attributes.Add("onclick", "return ValidateText('" + txtOrderTracking.ClientID + "','Please enter a tracking number')" +
                            " && ValidateDropDown('" + ddlOrderModel.ClientID + "','Please select a model')" +
                            " && ValidateHidden0('" + hdnOrderAddress.ClientID + "','" + txtOrderAddress.ClientID + "','Please select a location')" +
                            " && ValidateDropDown('" + ddlOrderClass.ClientID + "','Please select a class')" +
                            " && ValidateDropDown('" + ddlOrderEnvironment.ClientID + "','Please select an environment')" +
                            " && ValidateNumber0('" + txtOrderQuantity.ClientID + "','Please enter a valid quantity')" +
                            " && ValidateDate('" + txtOrderDate.ClientID + "','Please enter a valid order date')" +
                            " && ProcessButton(this)" +
                            ";");
                    }
                    if (intOrderAddress > 0)
                        txtOrderAddress.Text = oLocation.GetFull(intOrderAddress);
                    imgOrderDate.Attributes.Add("onclick", "return ShowCalendar('" + txtOrderDate.ClientID + "');");
                    ddlOrderClass.Attributes.Add("onchange", "PopulateEnvironments('" + ddlOrderClass.ClientID + "','" + ddlOrderEnvironment.ClientID + "',0);");
                    ddlOrderEnvironment.Attributes.Add("onchange", "UpdateDropDownHidden('" + ddlOrderEnvironment.ClientID + "','" + hdnOrderEnvironment.ClientID + "');");
                    btnOrderAddress.Attributes.Add("onclick", "return OpenLocations('" + hdnOrderAddress.ClientID + "','" + txtOrderAddress.ClientID + "');");
                    hdnOrderAddress.Value = intOrderAddress.ToString();
                    rptHosts.DataSource = oForecast.GetAnswersPlatform(intPlatform);
                    rptHosts.DataBind();
                    foreach (RepeaterItem ri in rptHosts.Items)
                    {
                        Label lblComplete = (Label)ri.FindControl("lblComplete");
                        Button btnEdit = (Button)ri.FindControl("btnEdit");
                        Button btnDelete = (Button)ri.FindControl("btnDelete");
                        Button btnExecute = (Button)ri.FindControl("btnExecute");
                        Button btnConfigure = (Button)ri.FindControl("btnConfigure");
                        Label lblHost = (Label)ri.FindControl("lblHost");
                        int intHost = Int32.Parse(lblHost.Text);
                        if (lblComplete.Text != "")
                        {
                            btnEdit.Enabled = false;
                            btnDelete.Enabled = false;
                            btnExecute.Enabled = false;
                            btnConfigure.Attributes.Add("onclick", "return OpenWindow('DEPLOY_HOST','" + oHost.Get(intHost, "path") + "?hostid=" + lblHost.Text + "&id=" + btnConfigure.CommandArgument + "');");
                        }
                        else
                        {
                            btnConfigure.Enabled = false;
                            Label lblRequestId = (Label)ri.FindControl("lblRequestId");
                            Label lblAnswer = (Label)ri.FindControl("lblAnswer");
                            int intID = Int32.Parse(lblAnswer.Text);
                            int intModel = Int32.Parse(oHost.Get(intHost, "modelid"));
                            int intType = oModel.GetType(Int32.Parse(oModelsProperties.Get(intModel, "modelid")));
                            Label lblCommitment = (Label)ri.FindControl("lblCommitment");
                            btnEdit.Attributes.Add("onclick", "return OpenWindow('INVENTORY_HOST','?id=" + intPlatform.ToString() + "&hostid=" + intID.ToString() + "');");
                            btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this line item?');");
                            if (oConfidence.Get(Int32.Parse(oForecast.GetAnswer(intID, "confidenceid")), "name").ToUpper().Contains("100") == true)
                            {
                                int intRequestID = 0;
                                if (lblRequestId.Text != "")
                                    intRequestID = Int32.Parse(lblRequestId.Text);
                                if (lblCommitment.Text == DateTime.Today.ToShortDateString() || (lblCommitment.Text != "" && DateTime.Parse(lblCommitment.Text) < DateTime.Today) || intRequestID > 0)
                                {
                                    btnDelete.Enabled = false;
                                    btnEdit.Text = "View";
                                    string strExecute = oType.Get(intType, "forecast_execution_path");
                                    if (strExecute != "")
                                        btnExecute.Attributes.Add("onclick", "return OpenWindow('FORECAST_EXECUTE','" + strExecute + "?id=" + intID.ToString() + "');");
                                    else
                                        btnExecute.Attributes.Add("onclick", "alert('Forecast execution has not been configured for asset type " + oType.Get(intType, "name") + "');return false;");
                                }
                                else
                                    btnExecute.Enabled = false;
                            }
                            else
                                btnExecute.Enabled = false;
                        }
                    }
                    lblNone.Visible = (rptHosts.Items.Count == 0);
                    btnAddHost.Attributes.Add("onclick", "return OpenWindow('INVENTORY_HOST','?id=" + intPlatform.ToString() + "&hostid=0');");
                }
            }
        }
        protected void LoadLists(int _platformid)
        {
            ddlOrderModel.DataTextField = "name";
            ddlOrderModel.DataValueField = "id";
            ddlOrderModel.DataSource = oModelsProperties.GetPlatforms(0, _platformid, 1);
            ddlOrderModel.DataBind();
            ddlOrderModel.Items.Insert(0, new ListItem("-- SELECT --", "0"));
            ddlOrderClass.DataTextField = "name";
            ddlOrderClass.DataValueField = "id";
            ddlOrderClass.DataSource = oClass.Gets(1);
            ddlOrderClass.DataBind();
            ddlOrderClass.Items.Insert(0, new ListItem("-- SELECT --", "0"));
            MaintenanceWindow oMaintenanceWindow = new MaintenanceWindow(intProfile, dsn);
        }
        protected void btnDelete_Click(Object Sender, EventArgs e)
        {
            Button oButton = (Button)Sender;
            oForecast.DeleteAnswer(Int32.Parse(oButton.CommandArgument));
            Response.Redirect(oPage.GetFullLink(intPage) + "?id=" + Request.QueryString["id"] + "&delete=true");
        }
        protected void btnAddOrder_Click(Object Sender, EventArgs e)
        {
            oOrder.Add(txtOrderTracking.Text, txtOrderName.Text, Int32.Parse(txtOrderQuantity.Text), Int32.Parse(ddlOrderModel.SelectedItem.Value), Int32.Parse(ddlOrderClass.SelectedItem.Value), Int32.Parse(Request.Form[hdnOrderEnvironment.UniqueID]), Int32.Parse(Request.Form[hdnOrderAddress.UniqueID]), Int32.Parse(oSetting.Get("confidence_100")), DateTime.Parse(txtOrderDate.Text));
            Response.Redirect(oPage.GetFullLink(intPage) + "?id=" + Request.QueryString["id"] + "&div=O" + "&order_add=true");
        }
        protected void btnEditOrder_Click(Object Sender, EventArgs e)
        {
            Button oButton = (Button)Sender;
            Response.Redirect(oPage.GetFullLink(intPage) + "?id=" + Request.QueryString["id"] + "&div=O" + "&orderid=" + oButton.CommandArgument);
        }
        protected void btnUpdateOrder_Click(Object Sender, EventArgs e)
        {
            int intOrder = Int32.Parse(Request.QueryString["orderid"]);
            oOrder.Update(intOrder, txtOrderTracking.Text, txtOrderName.Text, Int32.Parse(txtOrderQuantity.Text), Int32.Parse(ddlOrderModel.SelectedItem.Value), Int32.Parse(ddlOrderClass.SelectedItem.Value), Int32.Parse(Request.Form[hdnOrderEnvironment.UniqueID]), Int32.Parse(Request.Form[hdnOrderAddress.UniqueID]), Int32.Parse(oSetting.Get("confidence_100")), DateTime.Parse(txtOrderDate.Text), Int32.Parse(ddlOrderStatus.SelectedItem.Value), txtOrderComments.Text);
            Response.Redirect(oPage.GetFullLink(intPage) + "?id=" + Request.QueryString["id"] + "&div=O" + "&order_edit=true");
        }
        protected void btnRemoveOrder_Click(Object Sender, EventArgs e)
        {
            Button oButton = (Button)Sender;
            //oOrder.Delete(Int32.Parse(oButton.CommandArgument));
            Response.Redirect(oPage.GetFullLink(intPage) + "?id=" + Request.QueryString["id"] + "&div=O" + "&order_remove=true");
        }
        protected void btnDeleteOrder_Click(Object Sender, EventArgs e)
        {
            Button oButton = (Button)Sender;
            oOrder.Delete(Int32.Parse(oButton.CommandArgument));
            Response.Redirect(oPage.GetFullLink(intPage) + "?id=" + Request.QueryString["id"] + "&div=O" + "&order_delete=true");
        }
    }
}