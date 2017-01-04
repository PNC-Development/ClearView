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
    public partial class order : System.Web.UI.UserControl
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
                            LinkButton oRemove = (LinkButton)ri.FindControl("btnRemoveOrder");
                            oRemove.Enabled = (lblQuantity.Text == lblReceived.Text);
                            LinkButton oDelete = (LinkButton)ri.FindControl("btnDeleteOrder");
                            oDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this order?');");
                        }
                        btnUpdateOrder.Visible = false;
                        btnAddOrder.Attributes.Add("onclick", "return ValidateText('" + txtOrderTracking.ClientID + "','Please enter a tracking number')" +
                            " && ValidateDropDown('" + ddlOrderModel.ClientID + "','Please select a model')" +
                            " && ValidateHidden0('" + hdnOrderAddress.ClientID + "','" + txtOrderAddress.ClientID + "','Please select a location')" +
                            " && ValidateDropDown('" + ddlOrderClass.ClientID + "','Please select a class')" +
                            " && ValidateDropDown('" + ddlOrderEnvironment.ClientID + "','Please select an environment')" +
                            " && ValidateNumber0('" + txtOrderQuantity.ClientID + "','Please enter a valid quantity')" +
                            " && ValidateDate('" + txtOrderDate.ClientID + "','Please enter a valid order date')" +
                            ";");
                    }
                    if (intOrderAddress > 0)
                        txtOrderAddress.Text = oLocation.GetFull(intOrderAddress);
                    hdnOrderAddress.Value = intOrderAddress.ToString();
                    imgOrderDate.Attributes.Add("onclick", "return ShowCalendar('" + txtOrderDate.ClientID + "');");
                    ddlOrderClass.Attributes.Add("onchange", "PopulateEnvironments('" + ddlOrderClass.ClientID + "','" + ddlOrderEnvironment.ClientID + "',0);");
                    ddlOrderEnvironment.Attributes.Add("onchange", "UpdateDropDownHidden('" + ddlOrderEnvironment.ClientID + "','" + hdnOrderEnvironment.ClientID + "');");
                    btnOrderAddress.Attributes.Add("onclick", "return OpenLocations('" + hdnOrderAddress.ClientID + "','" + txtOrderAddress.ClientID + "');");
                }
            }
        }
        public void LoadLists(int _platformid)
        {
            ddlOrderModel.DataTextField = "name";
            ddlOrderModel.DataValueField = "id";
            ddlOrderModel.DataSource = oModelsProperties.GetPlatforms(1, _platformid, 1);
            ddlOrderModel.DataBind();
            ddlOrderModel.Items.Insert(0, new ListItem("-- SELECT --", "0"));
        }
        protected void btnAddOrder_Click(Object Sender, EventArgs e)
        {
            oOrder.Add(txtOrderTracking.Text, txtOrderName.Text, Int32.Parse(txtOrderQuantity.Text), Int32.Parse(ddlOrderModel.SelectedItem.Value), Int32.Parse(ddlOrderClass.SelectedItem.Value), Int32.Parse(Request.Form[hdnOrderEnvironment.UniqueID]), Int32.Parse(Request.Form[hdnOrderAddress.UniqueID]), Int32.Parse(oSetting.Get("confidence_100")), DateTime.Parse(txtOrderDate.Text));
            Response.Redirect(oPage.GetFullLink(intPage) + "?id=" + Request.QueryString["id"] + "&div=O" + "&order_add=true");
        }
        protected void btnEditOrder_Click(Object Sender, EventArgs e)
        {
            LinkButton oButton = (LinkButton)Sender;
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
            LinkButton oButton = (LinkButton)Sender;
            //oOrder.Delete(Int32.Parse(oButton.CommandArgument));
            Response.Redirect(oPage.GetFullLink(intPage) + "?id=" + Request.QueryString["id"] + "&div=O" + "&order_remove=true");
        }
        protected void btnDeleteOrder_Click(Object Sender, EventArgs e)
        {
            LinkButton oButton = (LinkButton)Sender;
            oOrder.Delete(Int32.Parse(oButton.CommandArgument));
            Response.Redirect(oPage.GetFullLink(intPage) + "?id=" + Request.QueryString["id"] + "&div=O" + "&order_delete=true");
        }
    }
}