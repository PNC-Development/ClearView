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
    public partial class asset_order : System.Web.UI.UserControl
    {

        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        private string dsnServiceEditor = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ServiceEditorDSN"]].ConnectionString;

        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intDefaultLocation = Int32.Parse(ConfigurationManager.AppSettings["OPSLocationID"]);
        protected int intAssignPage = Int32.Parse(ConfigurationManager.AppSettings["AssignWorkload"]);
        protected int intViewPage = Int32.Parse(ConfigurationManager.AppSettings["ViewWorkload"]);

        protected Pages oPage;
        protected Users oUser;
        protected Platforms oPlatform;
        protected Asset oAsset;
        protected AssetOrder oAssetOrder;
        protected Locations oLocation;
        protected ModelsProperties oModelsProperties;
        protected Models oModel;
        protected Types oType;
        protected Classes oClass;
       
        protected Requests oRequest;
        protected ResourceRequest oResourceRequest;
        protected Services oService;
        protected ServiceRequests oServiceRequest;
        protected Resiliency oResiliency;
        protected OperatingSystems oOperatingSystem;
      
        protected int intProfile;
        protected int intApplication = 0;
        protected int intPage = 0;
        protected string strLocation = "";
        protected int intModel = 0;
        protected int intPlatform = 0;

        protected bool boolShowAssetSummary = false;
        protected bool boolShowAssetSelection = false;
        

        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oPage = new Pages(intProfile, dsn);
            oUser = new Users(intProfile, dsn);
            oPlatform = new Platforms(intProfile, dsn);
            oAsset = new Asset(intProfile, dsnAsset);
            oAssetOrder = new AssetOrder(intProfile, dsn, dsnAsset, intEnvironment);
            oLocation = new Locations(intProfile, dsn);
            oModel = new Models(intProfile, dsn);
            oModelsProperties = new ModelsProperties(intProfile, dsn);
            oType = new Types(intProfile, dsn);
            oClass = new Classes(intProfile, dsn);
            oRequest = new Requests(intProfile, dsn);
            oResourceRequest = new ResourceRequest(intProfile, dsn);
            oService = new Services(intProfile, dsn);
            oServiceRequest = new ServiceRequests(intProfile, dsn);
            oResiliency = new Resiliency(intProfile, dsn);
            oOperatingSystem = new OperatingSystems(intProfile, dsn);

            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                intPlatform = Int32.Parse(Request.QueryString["id"]);

            //intPlatform = 1;
            if (intPlatform != 0)
            {
                pnlOrder.Visible = true;
                
                if (!IsPostBack)
                {
                    AddControlsAttributes();
                    LoadLists(intPlatform);
                    if (Request.QueryString["orderid"] != null && Request.QueryString["orderid"] != "")
                    {

                        if (Request.QueryString["submitted"] != null && Request.QueryString["submitted"] == "true")
                        {
                            lblInfo.Text = "Your information has been submitted successfully.";
                            pnlInfo.Visible = true;
                            pnlOrder.Style.Add("display", "none");
                        }
                        else if (Request.QueryString["saved"] != null && Request.QueryString["saved"] == "true")
                        {
                            lblInfo.Text = "Your information has been saved successfully.";
                            pnlInfo.Visible = true;
                            btnNewRequest.Visible = false;
                            hdnOrderId.Value = Request.QueryString["orderid"];
                            LoadOrderRequest();
                        }
                        else
                        {
                            hdnOrderId.Value = Request.QueryString["orderid"];
                            LoadOrderRequest();
                        }
                    }

                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(typeof(Page), "loadCurrentTab", "<script type=\"text/javascript\">window.top.LoadCurrentTab();<" + "/" + "script>");
                }

                PopulateLocations();
            }
            else 
            {
                pnlDenied.Visible = true;
            }

            
 
        }

        protected void AddControlsAttributes()
        {

            ddlClass.Attributes.Clear();
            ddlClass.Attributes.Add("onchange", "PopulateEnvironments('" + ddlClass.ClientID + "','" + ddlEnvironment.ClientID + "',0);");

            ddlModel.Attributes.Clear();
            ddlModel.Attributes.Add("onchange", "return getAssetCategory()" +
                                                " && setFieldsForOrderTypeAndModelSelection()" +
                                                " && ProcessControlButton()" +
                                                ";");

            rblOrderType.Attributes.Add("onclick", "return setFieldsForOrderTypeAndModelSelection()" +
                                                   " && ProcessControlButton()" +
                                                   ";");

            ddlEnvironment.Attributes.Clear();
            ddlEnvironment.Attributes.Add("onchange", "UpdateDropDownHidden('" + ddlEnvironment.ClientID + "','" + hdnEnvironment.ClientID + "');");

            imgbtnRequestedByDate.Attributes.Clear();
            imgbtnRequestedByDate.Attributes.Add("onclick", "return ShowCalendar('" + txtRequestedByDate.ClientID + "');");



            btnViewSelection.Attributes.Clear();
            btnViewSelection.Attributes.Add("onclick", "return ValidateRadioList('" + rblOrderType.ClientID + "','Please select Order type')" +
                                                        " && ValidateDropDown('" + ddlModel.ClientID + "','Please select a model')" +
                                                        " && ProcessControlButton()" +
                                                        ";");
         
            btnSaveSelection.Attributes.Clear();
            btnSaveSelection.Attributes.Add("onclick", "return Validations(false)" +
                                                       " && ProcessControlButton()" +
                                                       ";");
            btnSave.Attributes.Clear();
            btnSave.Attributes.Add("onclick", "return Validations(false)" +
                                                       " && ProcessControlButton()" +
                                                       ";");
            btnSubmitRequest.Attributes.Clear();
            btnSubmitRequest.Attributes.Add("onclick", "return Validations(true)" +
                                                        " && confirm('Are you sure you want to submit request ?')" +
                                                        " && ProcessControlButton()" +
                                                        ";");
            if (intPlatform == 4)
                btnSelectLocation.Attributes.Add("onclick", "return LoadLocationRoomRack('" + "zone" + "','" + hdnZoneId.ClientID + "', '" + txtLocation.ClientID + "', '" + txtRoom.ClientID + "','" + txtZone.ClientID + "','');");
            else
                btnSelectLocation.Attributes.Add("onclick", "return LoadLocationRoomRack('" + "rack" + "','" + hdnRackId.ClientID + "', '" + txtLocation.ClientID + "', '" + txtRoom.ClientID + "','" + txtZone.ClientID + "','" + txtRack.ClientID + "');");
            
        }

        protected void LoadLists(int _platformid)
        {
            //Model
            ddlModel.DataTextField = "name";
            ddlModel.DataValueField = "id";
            ddlModel.DataSource = oModelsProperties.GetPlatforms(1, _platformid,1);
            ddlModel.DataBind();
            ddlModel.Items.Insert(0, new ListItem("-- SELECT --", "0"));

            //Class
            ddlClass.DataTextField = "name";
            ddlClass.DataValueField = "id";
            ddlClass.DataSource = oClass.Gets(1);
            ddlClass.DataBind();
            ddlClass.Items.Insert(0, new ListItem("-- SELECT --", "0"));

            //Enclosure
            ddlEnclosure.DataTextField = "name";
            ddlEnclosure.DataValueField = "id";
            ddlEnclosure.DataSource = oAsset.GetEnclosures((int)AssetStatus.InUse);
            ddlEnclosure.DataBind();
            ddlEnclosure.Items.Insert(0, new ListItem("*** UNKNOWN ***", "-1"));
            ddlEnclosure.Items.Insert(0, new ListItem("-- SELECT --", "0"));

            ddlResiliency.DataValueField = "id";
            ddlResiliency.DataTextField = "name";
            ddlResiliency.DataSource = oResiliency.Gets(1);
            ddlResiliency.DataBind();
            ddlResiliency.Items.Insert(0, new ListItem("*** UNKNOWN ***", "-1"));
            ddlResiliency.Items.Insert(0, new ListItem("-- SELECT --", "0"));

            ddlOperatingSystemGroup.DataValueField = "id";
            ddlOperatingSystemGroup.DataTextField = "name";
            ddlOperatingSystemGroup.DataSource = oOperatingSystem.GetGroups(1);
            ddlOperatingSystemGroup.DataBind();
            ddlOperatingSystemGroup.Items.Insert(0, new ListItem("*** UNKNOWN ***", "-1"));
            ddlOperatingSystemGroup.Items.Insert(0, new ListItem("-- SELECT --", "0"));

            DataSet dsSwitches = oAsset.GetSwitchsByRack(0, 1);
            ddlSwitch1.DataTextField = ddlSwitch2.DataTextField = "name";
            ddlSwitch1.DataValueField = ddlSwitch2.DataValueField = "id";
            ddlSwitch1.DataSource = ddlSwitch2.DataSource = dsSwitches;
            ddlSwitch1.DataBind();
            ddlSwitch2.DataBind();
            ddlSwitch1.Items.Insert(0, new ListItem("-- SELECT --", "0"));
            ddlSwitch2.Items.Insert(0, new ListItem("-- SELECT --", "0"));
        }

        protected void LoadOrderRequest()
        {
            DataSet ds = oAssetOrder.Get(Int32.Parse(hdnOrderId.Value));
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                trRequest.Visible = btnUpdate.Visible = true;
                btnUpdate.CommandArgument = dr["StatusId"].ToString();
                hdnRequestId.Value = dr["RequestId"].ToString();
                lblRequestID.Text = "CVT" + hdnRequestId.Value;
                hdnItemId.Value = dr["ItemId"].ToString();
                hdnNumber.Value = dr["Number"].ToString();

                rblOrderType.SelectedValue = dr["OrderType"].ToString();
                txtDescription.Text = dr["NickName"].ToString();
                ddlModel.SelectedValue = dr["ModelId"].ToString();
                intModel = Int32.Parse(dr["ModelId"].ToString());
                hdnAssetCategoryId.Value = oModelsProperties.Get(intModel, "asset_category");

                txtLocation.Text = dr["Location"].ToString();
                txtRoom.Text = dr["Room"].ToString();
                txtRack.Text = dr["Rack"].ToString();
                hdnRackId.Value = dr["RackId"].ToString();
                txtRackPos.Text = dr["RackPos"].ToString();

                //hdnLocation.Value = dr["LocationId"].ToString();
                //ddlRoom.SelectedValue = dr["RoomId"].ToString();
                //ddlRack.SelectedValue = dr["RackId"].ToString();

                ddlClass.SelectedValue = dr["ClassId"].ToString();

                if (dr["ClassId"].ToString() != "")
                {
                    ddlEnvironment.DataTextField = "name";
                    ddlEnvironment.DataValueField = "id";

                    ddlEnvironment.SelectedValue = null;
                    ddlEnvironment.DataSource = oClass.GetEnvironment(Int32.Parse(dr["ClassId"].ToString()), 0);
                    ddlEnvironment.DataBind();
                    ddlEnvironment.Items.Insert(0, new ListItem("-- SELECT --", "0"));

                    hdnEnvironment.Value = (dr["EnvironmentId"].ToString() != "" ? dr["EnvironmentId"].ToString() : "0");
                    ddlEnvironment.SelectedValue = (dr["EnvironmentId"].ToString() != "" ? dr["EnvironmentId"].ToString() : "0");

                }
                else
                {
                    hdnEnvironment.Value = (dr["EnvironmentId"].ToString() != "" ? dr["EnvironmentId"].ToString() : "0");
                    ddlEnvironment.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                    ddlEnvironment.SelectedValue = (dr["EnvironmentId"].ToString() != "" ? dr["EnvironmentId"].ToString() : "0");
                }

                ddlResiliency.SelectedValue = dr["ResiliencyId"].ToString();

                ddlEnclosure.SelectedValue = dr["EnclosureId"].ToString();
                txtEnclosureSlot.Text = dr["EnclosureSlot"].ToString();
                ddlOperatingSystemGroup.SelectedValue = dr["OperatingSystemGroupId"].ToString();

                txtQuantity.Text = dr["RequestedQuantity"].ToString();

                txtRequestedByDate.Text = DateTime.Parse(dr["RequestedByDate"].ToString()).ToShortDateString();

                chkClustered.Checked = (dr["IsClustered"].ToString() == "1");
                chkBootLuns.Checked = (dr["IsBootLun"].ToString() == "1");


                bool boolInProcessOrder = (Int32.Parse(dr["StatusId"].ToString()) == (int)AssestOrderReqStatus.Pending ? true : false);

                trAssetsSummary.Style.Add("display", "none");
                trAssetsSelection.Style.Add("display", "none");

                //Load Asset Summary and Selected Assets
                if ((Int32.Parse(dr["OrderType"].ToString()) == (int)AssetOrderType.ReDeploy) ||
                    (Int32.Parse(dr["OrderType"].ToString()) == (int)AssetOrderType.Movement) ||
                    (Int32.Parse(dr["OrderType"].ToString()) == (int)AssetOrderType.Dispose))
                {
                    if (boolInProcessOrder == true)
                        PopulateAvailableAssetsSummary();
                    else
                        PopulateSelectedAssetsOnly();
                }

                //Load Comments
                PopulateOrderReqComments();

                //Verify if request is completed or in progress
                SetControls((boolInProcessOrder ? false : true), false);
                btnViewSelection.Enabled = false;

                if (boolInProcessOrder == false) //For Submitted requests
                {
                    pnlInfo.Visible = true;
                    lblInfo.Text = "";
                    btnNewRequest.Visible = true;
                }

            }
            else
            {
                trRequest.Visible = btnUpdate.Visible = false;
            }
            PopulateLocations();
        }

        protected void PopulateOrderReqComments()
        {
            //Load comments
            if (hdnOrderId.Value != "")
            {

                DataSet dsComments = oAssetOrder.GetAssetOrderComments(Int32.Parse(hdnOrderId.Value));
                dlOrderReqComments.DataSource = dsComments;
                dlOrderReqComments.DataBind();
                lblOrderReqCommentsNoComments.Visible = (dlOrderReqComments.Items.Count == 0);
            }

        }

        protected void PopulateAvailableAssetsSummary()
        {
            trAssetsSummary.Style.Add("Display", "inline");

            intModel = Int32.Parse(ddlModel.SelectedValue);

            dlAssetsSummary.DataSource = null;
            dlAssetsSummary.DataBind();
            dlAssetsSelection.DataSource = null;
            dlAssetsSelection.DataBind();

            lblTotalAssetsSelected.Text = "0";

            DataTable dtAssets = null;
            DataSet dsAssets = null;

            dsAssets = oAsset.GetAssetsAll(intModel, (int)AssetStatus.Available);
            dtAssets = dsAssets.Tables[0];

            if (Int32.Parse(rblOrderType.SelectedValue) == (int)AssetOrderType.Movement)
            {
                dsAssets = oAsset.GetAssetsAll(intModel, (int)AssetStatus.InUse);
                if (dtAssets != null)
                {
                    foreach (DataRow dr in dsAssets.Tables[0].Rows)
                    { dtAssets.ImportRow(dr); }
                }
                else
                    dtAssets = dsAssets.Tables[0];
            }

            //Remove assets from selection -where previous request is in process for the assets
            if (dtAssets != null)
            {
                DataSet dsWIPAssets = oAssetOrder.GetActiveAssetOrderAssets(intModel);
                DataTable dtWIPAssets = dsWIPAssets.Tables[0];
                foreach (DataRow dr in dtWIPAssets.Rows)
                {
                    DataRow[] drAssestsToRemove = null;
                    string strFilterCriteriaRemove;
                    strFilterCriteriaRemove = "AssetId =" + dr["AssetId"].ToString();
                    drAssestsToRemove = dtAssets.Select(strFilterCriteriaRemove);
                    foreach (DataRow drTmp in drAssestsToRemove)
                    {
                        dtAssets.Rows.Remove(drTmp);
                    }
                }
            }

            string[] strColumnNames ={ "LocationId", "Location", "ClassId", "Class", "EnvironmentId", "Environment" };
            string strSort = " Location,Class,Environment ";
            DataTable dtSummary = dtAssets.DefaultView.ToTable(true, strColumnNames);
            dtSummary.Columns.Add("Available", Type.GetType("System.Int32"));
            dtSummary.Columns.Add("Selected", Type.GetType("System.Int32"));
            dtSummary.Columns.Add("FilterCriteria", Type.GetType("System.String"));

            foreach (DataRow drSummary in dtSummary.Rows)
            {
                DataRow[] drAvailables = null;
                string strFilterCriteria = "";
                strFilterCriteria = strFilterCriteria + (drSummary["LocationId"] == DBNull.Value ? " LocationId IS Null" : (drSummary["LocationId"].ToString() == "" ? " LocationId =''" : "LocationId =" + drSummary["LocationId"].ToString()));
                strFilterCriteria = strFilterCriteria + " AND ";
                strFilterCriteria = strFilterCriteria + (drSummary["ClassId"] == DBNull.Value ? " ClassId IS Null" : (drSummary["ClassId"].ToString() == "" ? " ClassId =''" : " ClassId =" + drSummary["ClassId"].ToString()));
                strFilterCriteria = strFilterCriteria + " AND ";
                strFilterCriteria = strFilterCriteria + (drSummary["EnvironmentId"] == DBNull.Value ? " EnvironmentId IS Null" : (drSummary["EnvironmentId"].ToString() == "" ? " EnvironmentId =''" : " EnvironmentId =" + drSummary["EnvironmentId"].ToString()));
                strFilterCriteria = "( " + strFilterCriteria + " )";

                drAvailables = dtAssets.Select(strFilterCriteria);
                drSummary["Available"] = drAvailables.Length;
                drSummary["Selected"] = 0;
                drSummary["FilterCriteria"] = strFilterCriteria;

                if (hdnOrderId.Value != "" && hdnOrderId.Value != "0") //Get the selected asset count 
                {

                    foreach (DataRow drAvailable in drAvailables)
                    {
                        DataSet dsSelected = oAssetOrder.GetAssetOrderAssetSelection(Int32.Parse(hdnOrderId.Value), Int32.Parse(drAvailable["AssetId"].ToString()));
                        if (dsSelected.Tables[0].Rows.Count > 0)
                        {
                            drSummary["Selected"] = Int32.Parse(drSummary["Selected"].ToString()) + 1;
                            int intTotalAssetsSelected = Int32.Parse(lblTotalAssetsSelected.Text) + 1;
                            lblTotalAssetsSelected.Text = intTotalAssetsSelected.ToString();

                        }
                    }


                }
            }
            dtSummary.DefaultView.Sort = strSort;
            dlAssetsSummary.DataSource = dtSummary;
            dlAssetsSummary.DataBind();
        }

        protected void PopulateSelectedAssetsOnly()
        {
            trAssetsSelection.Style.Add("Display", "inline");

            DataTable dtSelected = null;
            DataSet dsAssetSelection = oAssetOrder.GetAssetOrderAssetSelection(Int32.Parse(hdnOrderId.Value));
            foreach (DataRow drSelected in dsAssetSelection.Tables[0].Rows)
            {
                DataSet dsAsset = oAsset.GetAssetsAll(Int32.Parse(drSelected["assetid"].ToString()));
                if (dtSelected != null)
                {
                    foreach (DataRow drtemp in dsAsset.Tables[0].Rows)
                    {
                        dtSelected.ImportRow(drtemp);
                    }
                }
                else
                    dtSelected = dsAsset.Tables[0];
            }
            string strSort = "AssetSerial";

            DataRow[] drSelect = null;
            dtSelected.DefaultView.Sort = strSort;
            drSelect = dtSelected.Select();
            dlAssetsSelection.DataSource = drSelect;
            dlAssetsSelection.DataBind();
        }

        protected void SaveRequest(int intStatus)
        {
            //Set Default Values For Request , Item , Number
            int intServiceId = 0;
            string strServerTitle = "";
            if (hdnRequestId.Value == "") hdnRequestId.Value = "0";
            if (hdnItemId.Value == "") hdnItemId.Value = "0";
            if (hdnNumber.Value == "") hdnNumber.Value = "0";


            
            if (Int32.Parse(rblOrderType.SelectedValue)==(int)AssetOrderType.Dispose)
            {
                hdnRackId.Value = "0";
                ddlEnclosure.SelectedValue = "0";
                txtEnclosureSlot.Text= "0";
                ddlClass.SelectedValue = "0";
                ddlEnvironment.SelectedValue = "0";
                hdnEnvironment.Value = "0";
            }


            if (intStatus == (int)AssestOrderReqStatus.Active)
            {
                //Create new request 
                int intRequest = oRequest.AddTask(0, intProfile, txtDescription.Text, DateTime.Now, DateTime.Now);
                hdnRequestId.Value = intRequest.ToString();
             
            }

            //Add / Update Order Request
            if (hdnOrderId.Value == "")
            {
                int intOrderId = oAssetOrder.AddOrderId(intProfile);
                hdnOrderId.Value = intOrderId.ToString();

                oAssetOrder.AddOrder(Int32.Parse(hdnOrderId.Value),
                                  Int32.Parse(hdnRequestId.Value), Int32.Parse(hdnItemId.Value), Int32.Parse(hdnNumber.Value),
                                  Int32.Parse(rblOrderType.SelectedValue),
                                  txtDescription.Text.Trim(),
                                  Int32.Parse(ddlModel.SelectedValue),
                                  0,
                                  0,
                                  (hdnAssetCategoryId.Value=="4"?Int32.Parse(hdnZoneId.Value):0),
                                  (hdnAssetCategoryId.Value != "4" ? Int32.Parse(hdnRackId.Value) : 0),
                                  txtRackPos.Text, Int32.Parse(ddlResiliency.SelectedItem.Value), Int32.Parse(ddlOperatingSystemGroup.SelectedItem.Value),
                                  Int32.Parse(ddlClass.SelectedValue),
                                  Int32.Parse((hdnEnvironment.Value != "" ? hdnEnvironment.Value : "0")),
                                  Int32.Parse(ddlEnclosure.SelectedValue),
                                  (txtEnclosureSlot.Text.Trim() != "" ? Int32.Parse(txtEnclosureSlot.Text.Trim()) : 0),
                                  Int32.Parse(txtQuantity.Text.Trim()),
                                  (rblOrderType.SelectedValue == "1" ? Int32.Parse(txtQuantity.Text.Trim()) : 0),
                                  (rblOrderType.SelectedValue == "2" ? Int32.Parse(txtQuantity.Text.Trim()) : 0), 0,
                                  DateTime.Parse(txtRequestedByDate.Text.Trim()), (chkClustered.Checked ? 1 : 0),
                                  (chkSanAttached.Checked ? 1 : 0), (chkBootLuns.Checked ? 1 : 0), Int32.Parse(ddlSwitch1.SelectedItem.Value),
                                  txtPort1.Text, Int32.Parse(ddlSwitch2.SelectedItem.Value), txtPort2.Text, intStatus, intProfile);
               

            }
            else
            {
                if (intStatus == (int)AssestOrderReqStatus.UpdateOnly)
                {
                    // Get old status
                    intStatus = Int32.Parse(btnUpdate.CommandArgument);
                }
                oAssetOrder.UpdateOrder(Int32.Parse(hdnOrderId.Value),
                                  Int32.Parse(hdnRequestId.Value), Int32.Parse(hdnItemId.Value), Int32.Parse(hdnNumber.Value),
                                  Int32.Parse(rblOrderType.SelectedValue),
                                  txtDescription.Text.Trim(),
                                  Int32.Parse(ddlModel.SelectedValue),
                                  0,0,
                                  (hdnAssetCategoryId.Value == "4" ? Int32.Parse(hdnZoneId.Value) : 0),
                                  (hdnAssetCategoryId.Value != "4" ? Int32.Parse(hdnRackId.Value) : 0),
                                  txtRackPos.Text, Int32.Parse(ddlResiliency.SelectedItem.Value), Int32.Parse(ddlOperatingSystemGroup.SelectedItem.Value),
                                  Int32.Parse(ddlClass.SelectedValue),
                                  Int32.Parse((hdnEnvironment.Value != "" ? hdnEnvironment.Value : "0")),
                                  Int32.Parse(ddlEnclosure.SelectedValue),
                                  (txtEnclosureSlot.Text.Trim() != "" ? Int32.Parse(txtEnclosureSlot.Text.Trim()) : 0),
                                  Int32.Parse(txtQuantity.Text.Trim()),
                                  (rblOrderType.SelectedValue == "1" ? Int32.Parse(txtQuantity.Text.Trim()) : 0),
                                  (rblOrderType.SelectedValue == "2" ? Int32.Parse(txtQuantity.Text.Trim()) : 0), 0,
                                  DateTime.Parse(txtRequestedByDate.Text.Trim()), (chkClustered.Checked ? 1 : 0),
                                  (chkSanAttached.Checked ? 1 : 0), (chkBootLuns.Checked ? 1 : 0), Int32.Parse(ddlSwitch1.SelectedItem.Value),
                                  txtPort1.Text, Int32.Parse(ddlSwitch2.SelectedItem.Value), txtPort2.Text, intStatus, intProfile);
            }
           
            //Added or deleted the assets to cv_asset_order_asset selection
            SaveAssetSelection(intStatus);

            //Add Commetns
            SaveReqComment();

            /* While finally submitting the request - 
            1. For selected assets set  NewOrderId=RequestID
            2. If it's Re-Deployment clear the asset configuration
            3. If it's Asset Movement set Asset Attribute =Moving
            
              */
            if (intStatus == (int)AssestOrderReqStatus.Active)
            {
                oAssetOrder.InitiateNextServiceRequestOrCompleteRequest(Int32.Parse(hdnOrderId.Value), 0, 0, true, dsnServiceEditor, intAssignPage, intViewPage, dsnAsset, dsnIP);
            }

        }

        protected void SaveAssetSelection(int intStatus)
        {
      
            if ((Int32.Parse(rblOrderType.SelectedValue) == (int)AssetOrderType.ReDeploy ||
                 Int32.Parse(rblOrderType.SelectedValue) == (int)AssetOrderType.Movement ||
                 Int32.Parse(rblOrderType.SelectedValue) == (int)AssetOrderType.Dispose) &&
                 intStatus == (int)AssestOrderReqStatus.Pending)
            {
                foreach (DataListItem dlItem in dlAssetsSelection.Items)
                {
                    HiddenField hdnAssetId = (HiddenField)dlItem.FindControl("hdnAssetId");
                    CheckBox chkSelectAsset = (CheckBox)dlItem.FindControl("chkSelectAsset");
                    oAssetOrder.AddRemoveAssetOrderAssetSelection(
                                                            Int32.Parse(hdnOrderId.Value),
                                                            Int32.Parse(hdnAssetId.Value),
                                                            intProfile, (chkSelectAsset.Checked == true ? 1 : -1));

                }
            }
        
        }

        protected void SaveReqComment()
        {
            if (txtOrderReqComments.Text.Trim() != "" && hdnOrderId.Value != "")
            {
                oAssetOrder.AddUpdateAssetOrderComment(
                                    0,
                                    Int32.Parse(hdnOrderId.Value),
                                    txtOrderReqComments.Text.Trim(),
                                    intProfile, 0);
                txtOrderReqComments.Text = "";
            }
        }

        protected void btnViewSelection_Click(Object Sender, EventArgs e)
        {
            //Load Asset Summary
            intModel = Int32.Parse(ddlModel.SelectedValue);
           
            PopulateAvailableAssetsSummary();

            rblOrderType.Enabled = false;
            ddlModel.Enabled = false;
            btnViewSelection.Enabled = false;

            PopulateLocations();

        }

        protected void btnClearSelection_Click(Object Sender, EventArgs e)
        {
            if (hdnOrderId.Value != "")
                oAssetOrder.DeleteAssetOrderAssetSelection(Int32.Parse(hdnOrderId.Value));

            rblOrderType.Enabled = true;
            ddlModel.Enabled = true;
            btnViewSelection.Enabled = true;
            SetControls(false, true);

        }

        protected void btnSubmitRequest_Click(Object Sender, EventArgs e)
        {
            SaveRequest((int)AssestOrderReqStatus.Active);
            Response.Redirect(oPage.GetFullLink(intPage) + "?id=" + Request.QueryString["id"] + "&div=O" + "&orderid=" + hdnOrderId.Value + "&submitted=true");
        }

        protected void btnSave_Click(Object Sender, EventArgs e)
        {
            SaveRequest((int)AssestOrderReqStatus.Pending);
            Response.Redirect(oPage.GetFullLink(intPage) + "?id=" + Request.QueryString["id"] + "&div=O" + "&orderid=" + hdnOrderId.Value + "&saved=true");
        }

        protected void btnSaveSelection_Click(Object Sender, EventArgs e)
        {
            SaveRequest((int)AssestOrderReqStatus.Pending);
            trAssetsSelection.Style.Add("Display", "none");
            LoadOrderRequest();
        }

        protected void btnNewRequest_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(oPage.GetFullLink(intPage) + "?id=" + Request.QueryString["id"] + "&div=O");
        }

        protected void btnAddOrderReqComments_Click(Object Sender, EventArgs e)
        {
            SaveReqComment();
        }

        protected void lnkbtnOrderReqDelete_Click(Object Sender, EventArgs e)
        {
            LinkButton oButton = (LinkButton)Sender;
            oAssetOrder.DeleteAssetOrderComment(Int32.Parse(oButton.CommandArgument), intProfile);
            LoadOrderRequest();
        }

        protected void dlAssetsSummary_ItemDataBound(object sender, DataListItemEventArgs e)
        {

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.SelectedItem)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;

                Label lblAssetsSummaryLocation = (Label)e.Item.FindControl("lblAssetsSummaryLocation");
                if (drv["LocationId"].ToString() != "")
                {
                    string strLocationCommonName = oLocation.GetAddress(Int32.Parse(drv["LocationId"].ToString()), "commonname");
                    if (strLocationCommonName == "")
                        lblAssetsSummaryLocation.Text = drv["Location"].ToString();
                    else
                        lblAssetsSummaryLocation.Text = strLocationCommonName;

                    lblAssetsSummaryLocation.ToolTip=drv["Location"].ToString();
                }



                Label lblAssetsSummaryClass = (Label)e.Item.FindControl("lblAssetsSummaryClass");
                lblAssetsSummaryClass.Text = drv["Class"].ToString();

                Label lblAssetsSummaryEnviorment = (Label)e.Item.FindControl("lblAssetsSummaryEnviorment");
                lblAssetsSummaryEnviorment.Text = drv["Environment"].ToString();

                Label lblAssetsSummaryCount = (Label)e.Item.FindControl("lblAssetsSummaryCount");
                lblAssetsSummaryCount.Text = drv["Available"].ToString();

                Label lblAssetsSummarySelected = (Label)e.Item.FindControl("lblAssetsSummarySelected");
                lblAssetsSummarySelected.Text = drv["Selected"].ToString();

                LinkButton lnkbtnAssetsAvailableLocationSelect = (LinkButton)e.Item.FindControl("lnkbtnAssetsAvailableLocationSelect");
                lnkbtnAssetsAvailableLocationSelect.CommandArgument = drv["filterCriteria"].ToString();
                lnkbtnAssetsAvailableLocationSelect.CommandName = "SELECTAVAILABLE";

            }

        }
  
        protected void dlAssetsSummary_Command(object sender, DataListCommandEventArgs e)
        {
            // Set the SelectedIndex property to display selected item in the DataList.
            if (e.CommandName.ToUpper() == "SELECTAVAILABLE")
            {
                dlAssetsSummary.SelectedIndex = e.Item.ItemIndex;

                PopulateAvailableAssetsSummary();

                string strFilterCriteria = e.CommandArgument.ToString();
                //string strSort = "Enclosure, Slot, AssetSerial";
                string strSort = "AssetSerial";

                DataTable dtAssets = null;
                DataSet dsAssets = null;

                //Get the Available Assets
                dsAssets = oAsset.GetAssetsAll(intModel, (int)AssetStatus.Available);
                dtAssets = dsAssets.Tables[0];

                if (Int32.Parse(rblOrderType.SelectedValue) == (int)AssetOrderType.Movement)
                {
                    //Get the In Use Assets
                    dsAssets = oAsset.GetAssetsAll(intModel, (int)AssetStatus.InUse);
                    if (dtAssets != null)
                    {
                        foreach (DataRow dr in dsAssets.Tables[0].Rows)
                        { dtAssets.ImportRow(dr); }
                    }
                    else
                        dtAssets = dsAssets.Tables[0];
                }

                //Remove assets from selection -where previous request is in process for the assets
                if (dtAssets != null)
                {
                    DataSet dsWIPAssets = oAssetOrder.GetActiveAssetOrderAssets(intModel);
                    DataTable dtWIPAssets = dsWIPAssets.Tables[0];
                    foreach (DataRow dr in dtWIPAssets.Rows)
                    {
                        DataRow[] drAssestsToRemove = null;
                        string strFilterCriteriaRemove;
                        strFilterCriteriaRemove= "AssetId =" + dr["AssetId"].ToString();
                        drAssestsToRemove = dtAssets.Select(strFilterCriteriaRemove);
                        foreach (DataRow drTmp in drAssestsToRemove)
                        {
                            dtAssets.Rows.Remove(drTmp);
                        }
                    }
                }
 
                DataRow[] drSelect = null;
                drSelect = dtAssets.Select(strFilterCriteria, strSort);

                dlAssetsSelection.DataSource = drSelect;
                dlAssetsSelection.DataBind();

                
                trAssetsSelection.Style.Add("Display", "inline");
                PopulateLocations();

                txtSearch.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnSearch.ClientID + "').click();return false;}} else {return true}; ");
                btnSearch.Attributes.Add("onclick", "SearchText('" + txtSearch.ClientID + "','" + dlAssetsSelection.ClientID + "');return false;");

            }

        }

        protected void dlAssetsSelection_ItemDataBound(object sender, DataListItemEventArgs e)
        {

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRow drv = (DataRow)e.Item.DataItem;
                HiddenField hdnAssetId = (HiddenField)e.Item.FindControl("hdnAssetId");
                hdnAssetId.Value = drv["AssetId"].ToString();

                Label lblAssetsSelectionSerial = (Label)e.Item.FindControl("lblAssetsSelectionSerial");
                lblAssetsSelectionSerial.Text = drv["AssetSerial"].ToString();

                Label lblAssetsSelectionAssetTag = (Label)e.Item.FindControl("lblAssetsSelectionAssetTag");
                lblAssetsSelectionAssetTag.Text = drv["AssetTag"].ToString();

                Label lblAssetsSelectionAssetStatus = (Label)e.Item.FindControl("lblAssetsSelectionAssetStatus");
                lblAssetsSelectionAssetStatus.Text = drv["AssetStatus"].ToString();


                Label lblAssetsSelectionLocation = (Label)e.Item.FindControl("lblAssetsSelectionLocation");

                if (drv["LocationId"].ToString() != "")
                {
                    string strLocationCommonName = oLocation.GetAddress(Int32.Parse(drv["LocationId"].ToString()), "commonname");
                    if (strLocationCommonName == "")
                        lblAssetsSelectionLocation.Text = drv["Location"].ToString();
                    else
                        lblAssetsSelectionLocation.Text = strLocationCommonName;

                    lblAssetsSelectionLocation.ToolTip = drv["Location"].ToString();
                }



                Label lblAssetsSelectionRoom = (Label)e.Item.FindControl("lblAssetsSelectionRoom");
                lblAssetsSelectionRoom.Text = drv["Room"].ToString();

                Label lblAssetsSelectionRack = (Label)e.Item.FindControl("lblAssetsSelectionRack");
                lblAssetsSelectionRack.Text = drv["Rack"].ToString();

                Label lblAssetsSelectionClass = (Label)e.Item.FindControl("lblAssetsSelectionClass");
                lblAssetsSelectionClass.Text = drv["Class"].ToString();
                Label lblAssetsSelectionEnvironment = (Label)e.Item.FindControl("lblAssetsSelectionEnvironment");
                lblAssetsSelectionEnvironment.Text = drv["Environment"].ToString();

                Label lblAssetsSelectionEnclosure = (Label)e.Item.FindControl("lblAssetsSelectionEnclosure");
                lblAssetsSelectionEnclosure.Text = drv["Enclosure"].ToString();

                Label lblAssetsSelectionEnclosureSlot = (Label)e.Item.FindControl("lblAssetsSelectionEnclosureSlot");
                lblAssetsSelectionEnclosureSlot.Text = drv["Slot"].ToString();


                if (hdnOrderId.Value != "") //In case of pending request
                {
                    DataSet dsAssetDeploymentStatus = oAssetOrder.GetAssetOrderAssetSelection(Int32.Parse(hdnOrderId.Value),
                                                                                                 Int32.Parse(hdnAssetId.Value));
                    if (dsAssetDeploymentStatus.Tables[0].Rows.Count > 0)
                    {
                        CheckBox chkSelectAsset = (CheckBox)e.Item.FindControl("chkSelectAsset");
                        chkSelectAsset.Checked = true;
                    }
                }
            }

        }

        protected void dlOrderReqComments_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            Users oUser = new Users(intProfile, dsn);
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;
                Label lblOrderReqComments = (Label)e.Item.FindControl("lblOrderReqComments");
                lblOrderReqComments.Text = drv["Comments"].ToString();

                Label lblOrderReqUpdatedBy = (Label)e.Item.FindControl("lblOrderReqUpdatedBy");
                lblOrderReqUpdatedBy.Text = oUser.GetFullName(Int32.Parse(drv["ModifiedBy"].ToString()));

                Label lblOrderReqLastUpdated = (Label)e.Item.FindControl("lblOrderReqLastUpdated");
                lblOrderReqLastUpdated.Text = drv["modified"].ToString();

                LinkButton lnkbtnOrderReqDelete = (LinkButton)e.Item.FindControl("lnkbtnOrderReqDelete");
                lnkbtnOrderReqDelete.Text = "Delete";
                lnkbtnOrderReqDelete.CommandArgument = drv["Id"].ToString();

                if (intProfile != Int32.Parse(drv["ModifiedBy"].ToString()) || drv["OrderStatusId"].ToString() == "4")
                    lnkbtnOrderReqDelete.Enabled = false;
                else 
                    lnkbtnOrderReqDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this comment?')&& ProcessControlButton();");
            }

        }

        protected void PopulateLocations()
        {
            //Populate Location Selection
            if (intPlatform == 4)
            {
                if (hdnZoneId.Value == "")
                    hdnZoneId.Value = "0";
                Zones oZone = new Zones(intProfile, dsn);

                if (hdnRackId.Value == "")
                    hdnRackId.Value = "0";

                DataSet dsLocation = oZone.Gets(Int32.Parse(hdnRackId.Value));
                if (dsLocation.Tables[0].Rows.Count > 0)
                {
                    txtLocation.Text = dsLocation.Tables[0].Rows[0]["Location"].ToString();
                    txtRoom.Text = dsLocation.Tables[0].Rows[0]["Room"].ToString(); ;
                    txtZone.Text = dsLocation.Tables[0].Rows[0]["Zone"].ToString(); ;
                }
            }
            else
            {
                if (hdnRackId.Value == "")
                    hdnRackId.Value = "0";
                RacksNew oRack = new RacksNew(intProfile, dsn);

                DataSet dsLocation = oRack.Gets(Int32.Parse(hdnRackId.Value));
                if (dsLocation.Tables[0].Rows.Count > 0)
                {
                    txtLocation.Text = dsLocation.Tables[0].Rows[0]["Location"].ToString();
                    txtRoom.Text = dsLocation.Tables[0].Rows[0]["Room"].ToString(); ;
                    txtZone.Text = dsLocation.Tables[0].Rows[0]["Zone"].ToString(); ;
                    txtRack.Text = dsLocation.Tables[0].Rows[0]["Rack"].ToString(); ;
                }
            }
           
        }

        private void SetControls(bool _boolViewMode, bool _boolClearControls)
        {
            bool boolEnabled = true;
            if (_boolViewMode == true)
                boolEnabled = false;

            rblOrderType.Enabled = boolEnabled;
            if (_boolClearControls == true) rblOrderType.SelectedIndex = -1;

            //txtDescription.Enabled = boolEnabled;
            if (_boolClearControls == true) txtDescription.Text = "";

            //ddlModel.Enabled = boolEnabled;
            if (_boolClearControls == true) ddlModel.SelectedValue = "0";

            btnSelectLocation.Enabled = boolEnabled;
            if (_boolClearControls == true)
            {   txtLocation.Text="";
                txtRoom.Text="";
                txtRack.Text = "";
                hdnRackId.Value = "0";
            }

            //ddlClass.Enabled = boolEnabled;
            if (_boolClearControls == true) ddlClass.SelectedValue = "0";

            //ddlEnvironment.Enabled = boolEnabled;
            if (_boolClearControls == true) ddlEnvironment.SelectedValue = "0";

            //ddlEnclosure.Enabled = boolEnabled;
            if (_boolClearControls == true) ddlEnclosure.SelectedValue = "0";

            //txtEnclosureSlot.Enabled = boolEnabled;
            if (_boolClearControls == true) txtEnclosureSlot.Text = "0";

            //txtQuantity.Enabled = boolEnabled;
            if (_boolClearControls == true) txtQuantity.Text = "0";


            //txtRequestedByDate.Enabled = boolEnabled;
            if (_boolClearControls == true) txtRequestedByDate.Text = "";

            //chkClustered.Enabled = boolEnabled;
            if (_boolClearControls == true) chkClustered.Checked = false;

            //chkBootLuns.Enabled = boolEnabled;
            if (_boolClearControls == true) chkBootLuns.Checked = false;

            //txtOrderReqComments.Enabled = boolEnabled;
            if (_boolClearControls == true) txtOrderReqComments.Text = "";

            if (_boolClearControls == true)
            {
                hdnItemId.Value = "0";
                hdnNumber.Value = "0";
                hdnAssetCategoryId.Value = "0";
                hdnEnvironment.Value = "0";
                lblTotalAssetsSelected.Text = "0";

                dlAssetsSummary.DataSource = null;
                dlAssetsSummary.DataBind();

                dlAssetsSelection.DataSource = null;
                dlAssetsSelection.DataBind();


                dlOrderReqComments.DataSource = null;
                dlOrderReqComments.DataBind();

                trReqCommentList.Style.Add("Display", "none");
                trAssetsSummary.Style.Add("Display","none");
                trAssetsSelection.Style.Add("Display","none");
            }

            btnSave.Enabled = boolEnabled;
            btnSubmitRequest.Enabled = boolEnabled;

            btnSaveSelection.Enabled = boolEnabled;
            //btnViewSelection.Enabled = boolEnabled;
            btnClearSelection.Enabled = boolEnabled;


        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            SaveRequest((int)AssestOrderReqStatus.UpdateOnly);
            Response.Redirect(oPage.GetFullLink(intPage) + "?id=" + Request.QueryString["id"] + "&div=O" + "&orderid=" + hdnOrderId.Value + "&saved=true");
        }

      
    }
}