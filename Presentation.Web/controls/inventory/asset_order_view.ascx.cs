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
    public partial class asset_order_view : System.Web.UI.UserControl
    {

        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        private string dsnServiceEditor = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ServiceEditorDSN"]].ConnectionString;

        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intDefaultLocation = Int32.Parse(ConfigurationManager.AppSettings["OPSLocationID"]);

        protected Pages oPage;
        protected Users oUser;
        protected Asset oAsset;
        protected AssetOrder oAssetOrder;
        
        protected Models oModel;
        protected ModelsProperties oModelsProperties;
        protected Locations oLocation;
        protected Classes oClass;
        protected Requests oRequest;
        protected ResourceRequest oResourceRequest;
        protected Services oService;
        protected ServiceRequests oServiceRequest;
        protected Platforms oPlatform;
        protected StatusLevels oStatusLevel;
        protected int intProfile;
        protected int intApplication = 0;
        protected int intPage = 0;
        protected int intPlatform = 0;

      
      
        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oPage = new Pages(intProfile, dsn);
            oUser = new Users(intProfile, dsn);
            oPlatform = new Platforms(intProfile, dsn);
            oStatusLevel = new StatusLevels(intProfile, dsn);
            oAssetOrder = new AssetOrder(intProfile, dsn, dsnAsset, intEnvironment);
          
            oLocation = new Locations(intProfile, dsn);
            oClass = new Classes(intProfile, dsn);
            oModel = new Models(intProfile, dsn);
            oModelsProperties = new ModelsProperties(intProfile, dsn);
           
           
            
            oRequest = new Requests(intProfile, dsn);
            oResourceRequest = new ResourceRequest(intProfile, dsn);
            oService = new Services(intProfile, dsn);
            oServiceRequest = new ServiceRequests(intProfile, dsn);
           
            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                intPlatform = Int32.Parse(Request.QueryString["id"]);

            intPlatform = 1;
            if (intPlatform != 0)
            {
                pnlAllow.Visible = true;
                if (!IsPostBack)
                {
                    LoadDefaultValues(intPlatform);

                    btnApplyFilter.Attributes.Add("onclick", "return ValidateDate('" + txtStartDate.ClientID + "','Please enter start date')" +
                           " && ValidateDate('" + txtEndDate.ClientID + "','Please enter end date')" +
                           ";");


                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(typeof(Page), "loadCurrentTab", "<script type=\"text/javascript\">window.top.LoadCurrentTab();<" + "/" + "script>");

                }
            }
            else
            {
                pnlDenied.Visible = true;
            }
            
            
 
        }

        public void LoadDefaultValues(int _platformid)
        {

            lstStatus.DataValueField = "StatusValue";
            lstStatus.DataTextField = "StatusDescription";
            lstStatus.DataSource = oStatusLevel.GetStatusList("ASSETORDERSTATUS");
            lstStatus.DataBind();
            //lstStatus.Items.Insert(0, new ListItem("-- SELECT --", ""));

            ddlModel.DataTextField = "name";
            ddlModel.DataValueField = "id";
            ddlModel.DataSource = oModelsProperties.GetPlatforms(0, _platformid, 0);
            ddlModel.DataBind();
            ddlModel.Items.Insert(0, new ListItem("-- SELECT --", "0"));

            txtStartDate.Text = DateTime.Today.AddYears(-1).ToShortDateString();
            txtEndDate.Text = DateTime.Today.ToShortDateString();
            imgStartDate.Attributes.Add("onclick", "return ShowCalendar('" + txtStartDate.ClientID + "');");
            imgEndDate.Attributes.Add("onclick", "return ShowCalendar('" + txtEndDate.ClientID + "');");
        }

        protected void btnOrder_Click(Object Sender, EventArgs e)
        {
            LinkButton oOrder = (LinkButton)Sender;
            if (hdnOrderBy.Value == oOrder.CommandArgument)
            {
                if (hdnOrder.Value == "1")
                    hdnOrder.Value = "0";
                else if (hdnOrder.Value == "0")
                    hdnOrder.Value = "1";
            }
            else
            {
                hdnOrderBy.Value = oOrder.CommandArgument;
                hdnOrder.Value = "0";
            }

            populateProcurementRequests();
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            hdnPageNo.Value = Convert.ToString(Convert.ToInt64(hdnPageNo.Value) + 1);
            populateProcurementRequests();
        }

        protected void btnPrevious_Click(object sender, EventArgs e)
        {
            hdnPageNo.Value = Convert.ToString(Convert.ToInt64(hdnPageNo.Value) - 1);
            populateProcurementRequests();
        }

        protected void btnApplyFilter_Click(Object Sender, EventArgs e)
        {

            populateProcurementRequests();

        }

        protected void populateProcurementRequests()
        {
            string strStatusIds = "";
            foreach (ListItem oList in lstStatus.Items)
            {
                if (oList.Selected == true)
                    strStatusIds = (strStatusIds == "" ? oList.Value : strStatusIds + "," + oList.Value);
            }

            DataSet ds = oAssetOrder.Get(
                                intPlatform, 
                                (ddlModel.SelectedValue != "0" ? Int32.Parse(ddlModel.SelectedValue) : (int?)null),
                                strStatusIds,
                                (txtStartDate.Text.Trim() != "" ? DateTime.Parse(txtStartDate.Text.Trim()) : (DateTime?)null),
                                (txtEndDate.Text.Trim() != "" ? DateTime.Parse(txtEndDate.Text.Trim()) : (DateTime?)null),
                                hdnOrderBy.Value.ToString(), Int32.Parse(hdnOrder.Value.ToString()),
                                Int32.Parse(hdnPageNo.Value), Int32.Parse(hdnRecsPerPage.Value));

            dlAssetOrders.DataSource = ds.Tables[0];
            dlAssetOrders.DataBind();

            if (ds.Tables[0].Rows.Count > 0)
            {
                long lngDisplayRecords = Int64.Parse(ds.Tables[0].Rows[0]["rownum"].ToString()) + Int64.Parse((ds.Tables[0].Rows.Count - 1).ToString());
                lblRecords.Text = "Showing Results <b>" + ds.Tables[0].Rows[0]["rownum"].ToString() + " - " + lngDisplayRecords.ToString() + "</b> of <b>" + ds.Tables[1].Rows[0]["TotalRecords"].ToString() + "</b>...";
                tdNavigation.Visible = true;
            }
            else
            {
                lblRecords.Text = "No Results Found...";
                tdNavigation.Visible = false;
            }

            // Calculate total numbers of pages
            long lngRecsPerPage = Int64.Parse(hdnRecsPerPage.Value);
            if (lngRecsPerPage != 0)
            {
                long lngTotalRecsCount = Int64.Parse(ds.Tables[1].Rows[0]["TotalRecords"].ToString());
                long lngPgCount = lngTotalRecsCount / lngRecsPerPage + ((lngTotalRecsCount % lngRecsPerPage) > 0 ? 1 : 0);
                if (Int32.Parse(hdnRecsPerPage.Value) != 0)
                {
                    // Display Next button
                    if (lngPgCount - 1 >= Convert.ToInt64(hdnPageNo.Value))
                        btnNext.Enabled = true;
                    else
                        btnNext.Enabled = false;
                    // Display Prev button
                    if ((Convert.ToInt64(hdnPageNo.Value)) > 1)
                        btnPrevious.Enabled = true;
                    else
                        btnPrevious.Enabled = false;
                }
            }
            else
            {
                btnNext.Enabled = false;
                btnPrevious.Enabled = false;
            }
        
        }

        protected void dlAssetOrders_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;

                LinkButton lnkOrderId = (LinkButton)e.Item.FindControl("lnkOrderId");
                lnkOrderId.Text = drv["OrderId"].ToString();
                lnkOrderId.CommandArgument = drv["OrderId"].ToString();

                Label lblRequestId = (Label)e.Item.FindControl("lblRequestId");
                lblRequestId.Text = "CVT" + drv["RequestId"].ToString();

                Label lblNickName = (Label)e.Item.FindControl("lblNickName");
                lblNickName.Text = drv["NickName"].ToString();

                Label lblModel = (Label)e.Item.FindControl("lblModel");
                lblModel.Text = drv["ModelName"].ToString();

                Label lblOrderType = (Label)e.Item.FindControl("lblOrderType");
                lblOrderType.Text = drv["OrderTypeName"].ToString();

                Label lblQuantityRequested = (Label)e.Item.FindControl("lblQuantityRequested");
                lblQuantityRequested.Text = drv["RequestedQuantity"].ToString();

                //Label lblQuantityProcure = (Label)e.Item.FindControl("lblQuantityProcure");
                //lblQuantityProcure.Text = drv["ProcureQuantity"].ToString();

                //Label lblQuantityReDeploy = (Label)e.Item.FindControl("lblQuantityReDeploy");
                //lblQuantityReDeploy.Text = drv["ReDeployQuantity"].ToString();

                //Label lblQuantityReturned = (Label)e.Item.FindControl("lblQuantityReturned");
                //lblQuantityReturned.Text = drv["ReturnedQuantity"].ToString();

                Label lblLocation = (Label)e.Item.FindControl("lblLocation");
                lblLocation.Text = drv["Location"].ToString();

                Label lblClass = (Label)e.Item.FindControl("lblClass");
                lblClass.Text = drv["Class"].ToString();

                Label lblEnvironment = (Label)e.Item.FindControl("lblEnvironment");
                lblEnvironment.Text = drv["Environment"].ToString();

                //Label lblPurchaseOrderStatus = (Label)e.Item.FindControl("lblPurchaseOrderStatus");
                //lblPurchaseOrderStatus.Text = drv["PurchaseOrderStatus"].ToString();

                //Label lblVendorOrderStatus = (Label)e.Item.FindControl("lblVendorOrderStatus");
                //lblVendorOrderStatus.Text = drv["VendorOrderStatus"].ToString();

                Label lblSubmittedDate = (Label)e.Item.FindControl("lblSubmittedDate");
                lblSubmittedDate.Text = DateTime.Parse(drv["created"].ToString()).ToShortDateString();

                Label lblStatus = (Label)e.Item.FindControl("lblStatus");
                lblStatus.Text = drv["OrderStatus"].ToString();

                Label lblLastUpdates = (Label)e.Item.FindControl("lblLastUpdates");
                lblLastUpdates.Text = "";
                DataSet dsRR = oAssetOrder.GetAssetOrderResourceRequestsByRequest(Int32.Parse(drv["RequestId"].ToString()), Int32.Parse(drv["Number"].ToString()));
                dsRR.Tables[0].DefaultView.Sort = "RRModified Desc ";
                DataTable dtRR = dsRR.Tables[0].DefaultView.ToTable();
                if (dtRR.Rows.Count > 0)
                    lblLastUpdates.Text = dtRR.Rows[0]["RRName"].ToString() + "<br/>" + DateTime.Parse(dtRR.Rows[0]["RRModified"].ToString()).ToShortDateString(); 


            }
        }


        protected void btnViewOrder_Click(Object Sender, EventArgs e)
        {
            LinkButton oButton = (LinkButton)Sender;
            Response.Redirect(oPage.GetFullLink(intPage) + "?id=" + Request.QueryString["id"] + "&div=O" + "&orderid=" + oButton.CommandArgument);
        }
        
    }
}