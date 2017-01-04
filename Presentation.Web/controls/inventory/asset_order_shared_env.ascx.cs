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
    public partial class asset_order_shared_env : System.Web.UI.UserControl
    {

        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        private string dsnServiceEditor = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ServiceEditorDSN"]].ConnectionString;

        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intDefaultLocation = Int32.Parse(ConfigurationManager.AppSettings["OPSLocationID"]);

        protected Pages oPage;
        protected Users oUser;
        protected Platforms oPlatform;
        protected Asset oAsset;
        protected AssetOrder oAssetOrder;
        protected AssetSharedEnvOrder oAssetSharedEnvOrder;
        protected Locations oLocation;
        protected ModelsProperties oModelsProperties;
        protected Models oModel;
        protected Types oType;
        protected Classes oClass;

        protected Requests oRequest;
        protected ResourceRequest oResourceRequest;
        protected Services oService;
        protected ServiceRequests oServiceRequest;
        protected VMWare oVMWare;
        protected OperatingSystems oOperatingSystem;
        protected int intProfile;
        protected int intApplication = 0;
        protected int intPage = 0;
        protected string strLocation = "";
        protected int intModel = 0;
        protected int intPlatform = 0;
        protected int intParent = 0;
        protected int intOrderType = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oPage = new Pages(intProfile, dsn);
            oUser = new Users(intProfile, dsn);
            oPlatform = new Platforms(intProfile, dsn);
            oAsset = new Asset(intProfile, dsnAsset);
            oAssetOrder = new AssetOrder(intProfile, dsn, dsnAsset, intEnvironment);
            oAssetSharedEnvOrder = new AssetSharedEnvOrder(intProfile, dsn, dsnAsset, intEnvironment);

            oLocation = new Locations(intProfile, dsn);

            oModel = new Models(intProfile, dsn);
            oModelsProperties = new ModelsProperties(intProfile, dsn);
            oType = new Types(intProfile, dsn);
            oClass = new Classes(intProfile, dsn);

            oRequest = new Requests(intProfile, dsn);
            oResourceRequest = new ResourceRequest(intProfile, dsn);
            oService = new Services(intProfile, dsn);
            oServiceRequest = new ServiceRequests(intProfile, dsn);
            oVMWare = new VMWare(intProfile, dsn);
            oOperatingSystem = new OperatingSystems(intProfile, dsn);

            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                intPlatform = Int32.Parse(Request.QueryString["id"]);


            if (intPlatform != 0)
            {
                pnlOrder.Visible=true; 
                if (!IsPostBack)
                {
                    PopulateLocations();

                    AddControlsAttributes();

                    LoadLists(intPlatform);

                    LoadOS();

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
                            LoadRequest();
                        }
                        else
                        {
                            hdnOrderId.Value = Request.QueryString["orderid"];
                            LoadRequest();
                        }
                    }
                }
                else
                {
                    PopulateLocations();
                    Page.ClientScript.RegisterStartupScript(typeof(Page), "loadCurrentTab", "<script type=\"text/javascript\">window.top.LoadCurrentTab();<" + "/" + "script>");
                }
            }
            else
            {
                pnlDenied.Visible = true;
            }



        }

        public void AddControlsAttributes()
        {
            
            string strValidations = "";

            intOrderType=(rblASEOrderType.SelectedValue!=""?Int32.Parse(rblASEOrderType.SelectedValue):0);

            strValidations = strValidations + " return ValidateRadioList('" + rblASEOrderType.ClientID + "','Please select the shared environment type')";
            strValidations = strValidations + " && ValidateText('" + txtDescription.ClientID + "','Please enter a request description')";

            if (intOrderType == (int)AssetSharedEnvOrderType.AddCluster || intOrderType == (int)AssetSharedEnvOrderType.AddHost)
            {
                strValidations = strValidations + " && ValidateDropDown('" + ddlModel.ClientID + "','Please select a model')";
                strValidations = strValidations + " && ValidateHidden0('" + hdnLocation.ClientID + "','ddlCommon','Please select location')";
                strValidations = strValidations + " && ValidateDropDown('" + ddlClass.ClientID + "','Please select class')";
                strValidations = strValidations + " && ValidateDropDown('" + ddlEnvironment.ClientID + "','Please select environment')";
            }

            if (intOrderType == (int)AssetSharedEnvOrderType.AddStorage)
                strValidations = strValidations + " && ValidateNumber0('" + txtStorageAmt.ClientID + "','Please enter storage amount')";

            strValidations = strValidations + " && ValidateDate('" + txtRequestedByDate.ClientID + "','Please enter requested by date')";

            if (intOrderType == (int)AssetSharedEnvOrderType.AddCluster)
                strValidations = strValidations + " && ValidateRadioList('" + rblClusterTypes.ClientID + "','Please select cluster type')";

            strValidations = strValidations + " && validateTreeViewNodeSelection()";

            if (intOrderType == (int)AssetSharedEnvOrderType.AddCluster ||
                intOrderType == (int)AssetSharedEnvOrderType.AddHost)
                strValidations = strValidations + " && validateAssetSelection()";

            btnSave.Attributes.Clear();
            btnSave.Attributes.Add("onclick", strValidations +
                                            " && ProcessControlButton()" +
                                            ";");

            btnSubmitRequest.Attributes.Clear();
            btnSubmitRequest.Attributes.Add("onclick", strValidations +
                                            " && confirm('Are you sure you want to submit request ?')" +
                                            " && ProcessControlButton()" +
                                            ";");

            btnViewSelection.Attributes.Add("onclick", "return ValidateRadioList('" + rblASEOrderType.ClientID + "','Please select the shared environment type')" +
                                                    " &&  ValidateDropDown('" + ddlModel.ClientID + "','Please select a model')" +
                                                    " &&  ValidateHidden0('" + hdnLocation.ClientID + "','ddlCommon','Please select location')" +
                                                    " &&  ValidateDropDown('" + ddlClass.ClientID + "','Please select class')" +
                                                    " &&  ValidateDropDown('" + ddlEnvironment.ClientID + "','Please select environment')" +
                                                    " &&  ProcessControlButton()" +
                                                    ";");
            ddlClass.Attributes.Clear();
            ddlClass.Attributes.Add("onchange", "PopulateEnvironments('" + ddlClass.ClientID + "','" + ddlEnvironment.ClientID + "',0);");

            ddlEnvironment.Attributes.Clear();
            ddlEnvironment.Attributes.Add("onchange", "UpdateDropDownHidden('" + ddlEnvironment.ClientID + "','" + hdnEnvironment.ClientID + "');");

            imgbtnRequestedByDate.Attributes.Clear();
            imgbtnRequestedByDate.Attributes.Add("onclick", "return ShowCalendar('" + txtRequestedByDate.ClientID + "');");

            rblASEOrderType.Attributes.Add("onclick", "return DisplayFieldsForEnvSelection();");
            rblClusterTypes.Attributes.Add("onclick", "return DisplayFunctions();");


        }

        public void LoadLists(int _platformid)
        {
            //Model
            ddlModel.DataTextField = "name";
            ddlModel.DataValueField = "id";
            ddlModel.DataSource = oModelsProperties.Gets(0, 1);// oModelsProperties.GetPlatforms(0, _platformid, 1);
            ddlModel.DataBind();
            ddlModel.Items.Insert(0, new ListItem("-- SELECT --", "0"));

            //Class
            ddlClass.DataTextField = "name";
            ddlClass.DataValueField = "id";
            ddlClass.DataSource = oClass.Gets(1);
            ddlClass.DataBind();
            ddlClass.Items.Insert(0, new ListItem("-- SELECT --", "0"));


        }

        private void LoadOS()
        {
            DataSet dsOS = oOperatingSystem.Gets(1);
            foreach (DataRow drOS in dsOS.Tables[0].Rows)
                chkFunctionVMWARE.Items.Add(new ListItem(drOS["name"].ToString(), drOS["id"].ToString()));
        }

        #region Load Tree View with Folder, Cluster, Host Based Env. Type Selection

        private void LoadFolderClusterHosts()
        {
            tvParent.Nodes.Clear();
            LoadVirtualCenters();
            lblParentSelect.Style.Add("display", (tvParent.Nodes.Count > 0 ? "none" : "inline"));
        }
        private void LoadVirtualCenters()
        {
            DataSet ds = oVMWare.GetVirtualCenters(1);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["name"].ToString();
                oNode.ToolTip = dr["name"].ToString();
                oNode.Value = dr["id"].ToString();
                oNode.ImageUrl = "/images/folder.gif";

                oNode.SelectAction = TreeNodeSelectAction.None;
                tvParent.Nodes.Add(oNode);
                LoadDatacenter(Int32.Parse(dr["id"].ToString()), oNode);
            }

            tvParent.ExpandDepth = 2;
            //tvParent.Attributes.Add("oncontextmenu", "return false;");
            tvParent.Attributes.Add("onclick", "OnTreeClick(event)");

        }
        private void LoadDatacenter(int _parent, TreeNode oParent)
        {
            DataSet ds = oVMWare.GetDatacenters(_parent, 1);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["name"].ToString();
                oNode.ToolTip = dr["name"].ToString();
                oNode.Value = dr["id"].ToString();
                oNode.ImageUrl = "/images/folder.gif";
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                oParent.ChildNodes.Add(oNode);
                LoadFolder(Int32.Parse(dr["id"].ToString()), oNode);
            }
        }
        private void LoadFolder(int _parent, TreeNode oParent)
        {
            DataSet ds = oVMWare.GetFolders(_parent, 0);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["name"].ToString();
                oNode.ToolTip = dr["name"].ToString();
                oNode.Value = dr["id"].ToString();
                oNode.ImageUrl = "/images/folder.gif";
                oNode.SelectAction = TreeNodeSelectAction.Expand;

                if (intOrderType == (int)AssetSharedEnvOrderType.AddCluster)
                {
                    oNode.ShowCheckBox = true;
                    oNode.Checked= (dr["id"].ToString()==hdnParentId.Value?true:false);
                }
                oParent.ChildNodes.Add(oNode);
                if (intOrderType == (int)AssetSharedEnvOrderType.AddHost ||
                    intOrderType == (int)AssetSharedEnvOrderType.AddStorage)
                    LoadCluster(Int32.Parse(dr["id"].ToString()), oNode);
            }
        }
        private void LoadCluster(int _parent, TreeNode oParent)
        {
            DataSet ds = oVMWare.GetClusters(_parent, 0);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["name"].ToString();
                oNode.ToolTip = dr["name"].ToString();
                oNode.Value = dr["id"].ToString();
                oNode.ImageUrl = "/images/folder.gif";
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                if (intOrderType == (int)AssetSharedEnvOrderType.AddHost ||
                    intOrderType == (int)AssetSharedEnvOrderType.AddStorage)
                {   oNode.ShowCheckBox = true;
                    oNode.Checked = (dr["id"].ToString() == hdnParentId.Value ? true : false);
                }
                oParent.ChildNodes.Add(oNode);
                //if (intOrderType == (int)AssetSharedEnvOrderType.AddStorage)//for Storage - Load Host
                //    LoadHost(Int32.Parse(dr["id"].ToString()), oNode);

            }
        }
        private void LoadHost(int _parent, TreeNode oParent)
        {
            DataSet ds = oVMWare.GetHosts(_parent, 0);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["name"].ToString();
                oNode.ToolTip = dr["name"].ToString();
                oNode.Value = dr["id"].ToString();
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                if (intOrderType == (int)AssetSharedEnvOrderType.AddStorage)
                {
                    oNode.ShowCheckBox = true;
                    oNode.Checked = (dr["id"].ToString() == hdnParentId.Value ? true : false);
                }
                oParent.ChildNodes.Add(oNode);
            }
        }

        #endregion

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

        protected void btnViewSelection_Click(Object Sender, EventArgs e)
        {
            intOrderType = (rblASEOrderType.SelectedValue != "" ? Int32.Parse(rblASEOrderType.SelectedValue) : 0);
            //Add Control Attributes
            AddControlsAttributes();

            //Load Folder or Cluster Or Host Based on Env. Type Selection
            LoadFolderClusterHosts();

            //Load Available Hosts Assets

            if (intOrderType == (int)AssetSharedEnvOrderType.AddCluster ||
                intOrderType == (int)AssetSharedEnvOrderType.AddHost)
                PopulateAvailableAssets();
            else
                pnlAssetsAvailable.Visible=false;


            //Populate Selection
            PopulateSelection();

            setControlsForSelection(false);

            
        }

        private void setControlsForSelection(bool boolEnabled)
        {
            //Disable further selection
            rblASEOrderType.Enabled = boolEnabled;
            ddlModel.Enabled = boolEnabled;
            //trLocation.Enabled = true;
            ddlClass.Enabled = boolEnabled;
            ddlEnvironment.Enabled = boolEnabled;
            btnViewSelection.Enabled = boolEnabled;
        }
        protected void btnClearSelection_Click(Object Sender, EventArgs e)
        {
            setControlsForSelection(true);
            SetControls(false, true);

        }
     
        protected void btnNewRequest_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(oPage.GetFullLink(intPage) + "?id=" + Request.QueryString["id"] + "&div=O");
        }


        #region Request Load/Save

        protected void LoadRequest()
        {
            int intOrderId = Int32.Parse(hdnOrderId.Value);
            DataSet ds = oAssetSharedEnvOrder.Get(intOrderId);


            if (ds.Tables[0].Rows.Count > 0)
            {
                SetControls(false, true);
                DataRow dr = ds.Tables[0].Rows[0];

                hdnOrderId.Value = dr["OrderId"].ToString();
                hdnRequestId.Value = dr["RequestId"].ToString();
                intOrderType = Int32.Parse(dr["OrderType"].ToString());
                rblASEOrderType.SelectedValue = dr["OrderType"].ToString();

                txtDescription.Text = dr["NickName"].ToString();

                ddlModel.SelectedValue = dr["ModelId"].ToString();
                intModel = Int32.Parse(dr["ModelId"].ToString());

                hdnLocation.Value = dr["LocationId"].ToString();
                PopulateLocations();

                ddlClass.SelectedValue = dr["ClassId"].ToString();

                if (dr["ClassId"].ToString() != "")
                {
                    ddlEnvironment.Items.Clear();
                    ddlEnvironment.DataTextField = "name";
                    ddlEnvironment.DataValueField = "id";
                    ddlEnvironment.DataSource = oClass.GetEnvironment(Int32.Parse(dr["ClassId"].ToString()), 0);
                    ddlEnvironment.DataBind();
                    ddlEnvironment.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                }
                hdnEnvironment.Value = (dr["EnvironmentId"].ToString() != "" ? dr["EnvironmentId"].ToString() : "0");
                ddlEnvironment.SelectedValue = (dr["EnvironmentId"].ToString() != "" ? dr["EnvironmentId"].ToString() : "0");

                if (intOrderType == (int)AssetSharedEnvOrderType.AddCluster) //Shared Env. Type ="Cluster"
                {
                    rblClusterTypes.SelectedValue = dr["ClusterType"].ToString();
                    chkFunctionVMWARE.Enabled = false;
                    //chkFunctionWorkstation.Checked = (dr["Function_VMWARE_Workstation"].ToString() == "1" ? true : false);
                    //chkFunctionServer.Checked = (dr["Function_VMWARE_Server"].ToString() == "1" ? true : false);
                    //chkFunctionWindows.Checked = (dr["Function_VMWARE_Windows"].ToString() == "1" ? true : false);
                    //chkFunctionLinux.Checked = (dr["Function_VMWARE_Linux"].ToString() == "1" ? true : false);

                    chkFunctionContainer.Checked = (dr["Function_SUN_Container"].ToString() == "1" ? true : false);
                    chkFunctionLDOM.Checked = (dr["Function_SUN_LDOM"].ToString() == "1" ? true : false);
                }
                txtStorageAmt.Text = (dr["StorageAmt"].ToString() != "" ? dr["StorageAmt"].ToString() : "0.00");
                txtRequestedByDate.Text = DateTime.Parse(dr["RequestedByDate"].ToString()).ToShortDateString();
                hdnParentId.Value = dr["ParentId"].ToString();
                //Populate Parent 
                LoadFolderClusterHosts();
                

                bool boolInProcessOrder = (Int32.Parse(dr["StatusId"].ToString()) == (int)AssestOrderReqStatus.Pending ? true : false);
                SetControls((boolInProcessOrder ? false : true), (boolInProcessOrder ? false : false));

                //Populate Available Assets
                if (intOrderType == (int)AssetSharedEnvOrderType.AddCluster || intOrderType == (int)AssetSharedEnvOrderType.AddHost)
                {
                    pnlAssetsAvailable.Visible = true;
                    if (boolInProcessOrder)
                        PopulateAvailableAssets();
                    else
                        PopulateAssetsSelectedOnly();
                }
                else
                    pnlAssetsAvailable.Visible = false;

                if (boolInProcessOrder == false) //For Submitted requests
                {
                    pnlInfo.Visible = true;
                    lblInfo.Text = "";
                    btnNewRequest.Visible=true;
                }
                btnSave.Enabled = (boolInProcessOrder == true ? true : false);
                btnSubmitRequest.Enabled = (boolInProcessOrder == true ? true : false);

                btnViewSelection.Enabled = false;
                btnClearSelection.Enabled = (boolInProcessOrder == true ? true : false);

                //Handle Selections
                setControlsForSelection(false);
                

            }

        }

        private void resursiveChildNode(TreeNode oNode)
        { 
            if (oNode.ChildNodes.Count>0)
            {
                foreach(TreeNode oChildNode in oNode.ChildNodes)
                {
                    resursiveChildNode(oChildNode);
                }
            }else
                if (oNode.Checked == true)
                        hdnParentId.Value = oNode.Value;
                
        }

        protected void SaveRequest(int intStatus)
        {
            intOrderType = Int32.Parse(rblASEOrderType.SelectedValue);
            int intClusterType = 0;
            if (hdnRequestId.Value == "") hdnRequestId.Value = "0";
            if (hdnItemId.Value == "") hdnItemId.Value = "0";
            if (hdnNumber.Value == "") hdnNumber.Value = "0";
            if (rblClusterTypes.SelectedValue=="1")
               intClusterType= Int32.Parse(rblClusterTypes.SelectedValue);
           if (txtStorageAmt.Text == "") txtStorageAmt.Text = "0.00";
           
           
            intModel = Int32.Parse(ddlModel.SelectedValue);


            foreach (TreeNode oNode in tvParent.Nodes)
            {
                if (oNode.ChildNodes.Count > 0)
                    resursiveChildNode(oNode);
                else
                {
                    if (oNode.Checked == true)
                        hdnParentId.Value = oNode.Value;
                }
            }
            intParent = Int32.Parse(hdnParentId.Value);

            if (intStatus == (int)AssestOrderReqStatus.Active)
            {
                
                int intNewRequest = oRequest.AddTask(0, intProfile, txtDescription.Text, DateTime.Now, DateTime.Now);
                hdnRequestId.Value = intNewRequest.ToString();
                int intServiceId = 0;
                int intItemID = 0;
                if (intOrderType == (int)AssetSharedEnvOrderType.AddCluster)
                    intServiceId = Int32.Parse(ConfigurationManager.AppSettings["SERVICEID_ASSET_SHARED_ENV_ADD_CLUSTER"]);
                else if (intOrderType == (int)AssetSharedEnvOrderType.AddHost)
                    intServiceId = Int32.Parse(ConfigurationManager.AppSettings["SERVICEID_ASSET_SHARED_ENV_ADD_HOST"]);
                else if (intOrderType == (int)AssetSharedEnvOrderType.AddStorage)
                    intServiceId = Int32.Parse(ConfigurationManager.AppSettings["SERVICEID_ASSET_SHARED_ENV_ADD_STORAGE"]);

                    intItemID = oService.GetItemId(intServiceId);
                    hdnItemId.Value = intItemID.ToString();
                    hdnNumber.Value = "1";
            }

            if (hdnOrderId.Value == "" || hdnOrderId.Value == "0")  //New Order
            {
                int intOrderId = intOrderId = oAssetOrder.AddOrderId(intProfile);
                hdnOrderId.Value = intOrderId.ToString();

                oAssetSharedEnvOrder.AddOrder(Int32.Parse(hdnOrderId.Value),
                                            Int32.Parse(hdnRequestId.Value),
                                            Int32.Parse(hdnItemId.Value),
                                            Int32.Parse(hdnNumber.Value),
                                            intOrderType,
                                            txtDescription.Text.Trim(),
                                            Int32.Parse(ddlModel.SelectedValue),
                                            Int32.Parse(hdnLocation.Value),
                                            Int32.Parse(ddlClass.SelectedValue),
                                            Int32.Parse((hdnEnvironment.Value != "" ? hdnEnvironment.Value : "0")),
                                            intParent,
                                            DateTime.Parse(txtRequestedByDate.Text.Trim()),
                                            intClusterType,
                                            0,
                                            0,
                                            0,
                                            0,
                                            //(chkFunctionWorkstation.Checked == true ? 1 : 0),
                                            //(chkFunctionServer.Checked == true ? 1 : 0),
                                            //(chkFunctionWindows.Checked == true ? 1 : 0),
                                            //(chkFunctionLinux.Checked == true ? 1 : 0),
                                            (chkFunctionContainer.Checked == true ? 1 : 0),
                                            (chkFunctionLDOM.Checked == true ? 1 : 0),
                                            float.Parse(txtStorageAmt.Text),
                                            intStatus,
                                            intProfile);
                
            }
            else  //In Process Order
            {
                oAssetSharedEnvOrder.UpdateOrder(Int32.Parse(hdnOrderId.Value),
                                                Int32.Parse(hdnRequestId.Value),
                                                Int32.Parse(hdnItemId.Value),
                                                Int32.Parse(hdnNumber.Value),
                                                intOrderType,
                                                txtDescription.Text.Trim(),
                                                Int32.Parse(ddlModel.SelectedValue),
                                                Int32.Parse(hdnLocation.Value),
                                                Int32.Parse(ddlClass.SelectedValue),
                                                Int32.Parse((hdnEnvironment.Value != "" ? hdnEnvironment.Value : "0")),
                                                intParent,
                                                DateTime.Parse(txtRequestedByDate.Text.Trim()),
                                                intClusterType,
                                                0,
                                                0,
                                                0,
                                                0,
                                                //(chkFunctionWorkstation.Checked == true ? 1 : 0),
                                                //(chkFunctionServer.Checked == true ? 1 : 0),
                                                //(chkFunctionWindows.Checked == true ? 1 : 0),
                                                //(chkFunctionLinux.Checked == true ? 1 : 0),
                                                (chkFunctionContainer.Checked == true ? 1 : 0),
                                                (chkFunctionLDOM.Checked == true ? 1 : 0),
                                                float.Parse(txtStorageAmt.Text),
                                                intStatus,
                                                intProfile);
            }

           

            //1. Remove Previous Host Asset selection - In case of selection changes
            oAssetOrder.DeleteAssetOrderAssetSelection(Int32.Parse(hdnOrderId.Value));

            //2. Add Host Asset Selection for Cluster Or Host
            if (intOrderType == (int)AssetSharedEnvOrderType.AddCluster || intOrderType == (int)AssetSharedEnvOrderType.AddHost)
            {
                foreach (DataListItem dlItem in dlAssetsSelection.Items)
                {
                    HiddenField hdnAssetId = (HiddenField)dlItem.FindControl("hdnAssetId");
                    CheckBox chkSelectAsset = (CheckBox)dlItem.FindControl("chkSelectAsset");
                    if (chkSelectAsset.Checked == true)
                    {
                        oAssetOrder.AddRemoveAssetOrderAssetSelection(
                                                Int32.Parse(hdnOrderId.Value),
                                                Int32.Parse(hdnAssetId.Value),
                                                intProfile,1);
                    }
                }
            }

            if (intStatus == (int)AssestOrderReqStatus.Active )
            {
                //1. For selected assets set  NewOrderId=RequestID
                if (intOrderType == (int)AssetSharedEnvOrderType.AddCluster || intOrderType == (int)AssetSharedEnvOrderType.AddHost)
                {   
                    DataSet dsAssetSelected = oAssetOrder.GetAssetOrderAssetSelection(Int32.Parse(hdnOrderId.Value));
                    foreach (DataRow dr in dsAssetSelected.Tables[0].Rows)
                    {
                        oAsset.updateNewOrderId(Int32.Parse(hdnOrderId.Value), Int32.Parse(dr["AssetId"].ToString()));
                    }
                }

                //2. for Cluster -Add record in Cluster /Server Table
                if (intOrderType == (int)AssetSharedEnvOrderType.AddCluster || intOrderType == (int)AssetSharedEnvOrderType.AddHost)
                {
                   CreateClusterAndAddHostsToServer();
                }

                //For Storage Create the Service Requests
                if (intOrderType == (int)AssetSharedEnvOrderType.AddStorage)
                {
                    InitiateAddStorageWorkOrder();
                }

            }
        }

        protected void CreateClusterAndAddHostsToServer()
        {
            ServerName oServerName = new ServerName(0, dsn);
            Servers oServer = new Servers(0, dsn);
            Cluster oCluster = new Cluster(0, dsn);
            int intClass = Int32.Parse(ddlClass.SelectedValue);
            int intEnv = Int32.Parse(ddlEnvironment.SelectedValue);
            int intAddress = Int32.Parse(hdnLocation.Value);
            int intRequestId = Int32.Parse(hdnRequestId.Value);
            int intVMWareClusterId = 0;
            int intClusterID =0;
            int intSelectedCount = 0;
            int intSelectedDRCount = 0;

            DataSet dsAssets = oAsset.GetAssetsByOrder(Int32.Parse(hdnOrderId.Value));
            intSelectedCount=dsAssets.Tables[0].Rows.Count;
            intSelectedDRCount = dsAssets.Tables[0].Rows.Count;

            intModel = Int32.Parse(ddlModel.SelectedValue);
            if (intOrderType == (int)AssetSharedEnvOrderType.AddCluster)//for Cluster -Add record in Cluster Table
            {
                string strPrefix = "XCV";
                string strDatastoreNotify = "xassa3x";
                int intServerName = oServerName.Add(intClass, intEnv, intAddress, strPrefix, intProfile, "VMWARE_CLUSTER_IM", 1, dsnServiceEditor);
                string strClusterName = oServerName.GetName(intServerName, 0);
                intVMWareClusterId = oVMWare.AddCluster(intParent, intModel, strClusterName, 0, 0,
                                    10000, "", strDatastoreNotify, 1, 500, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 100, 2, 100, 500, 100,
                                    10, 10, 10, 1, 1, 1);
                foreach (ListItem lstOS in chkFunctionVMWARE.Items)
                {
                    if (lstOS.Selected)
                        oVMWare.AddClusterOS(intVMWareClusterId, Int32.Parse(lstOS.Value));
                }
                intClusterID = oCluster.Add(intRequestId, strClusterName, intSelectedCount, intSelectedDRCount, 0);

            }
            else if (intOrderType == (int)AssetSharedEnvOrderType.AddHost)
            { 
                intVMWareClusterId=Int32.Parse(hdnParentId.Value);
            }

            //For Cluster OR Host -Add Server Assets Table
            if (intOrderType == (int)AssetSharedEnvOrderType.AddCluster ||
                intOrderType == (int)AssetSharedEnvOrderType.AddHost)
            {
                int intNumber = 0;
                int intDomain = 0;  
                int intOS = 0;  
                int intSP = 0;  
                string strNodePrefix = "XNV";
                foreach (DataRow drAsset in dsAssets.Tables[0].Rows)
                {
                        //Mark the Asset to Instock
                        //oAsset.UpdateStatus(Int32.Parse(hdnAssetId.Value), "", (int)AssetStatus.InStock, intProfile, DateTime.Now);
                        intNumber++;
                        int intserverId = oServer.Add(intRequestId, 0, intModel, 0, intClusterID, intNumber, intOS, intSP, 0, intDomain, 0, 1, 0, (intSelectedDRCount > 0 ? 1 : 0), 0, "", 0, 0, 1, 1, 1, 1, 0, (oClass.Get(intClass, "pnc") == "1" ? 1 : 0), intVMWareClusterId, 0);
                        int intAsset = Int32.Parse(drAsset["id"].ToString());
                        oServer.AddAsset(intserverId, intAsset, intClass, intEnvironment, 0, 0);
                        //oServer.UpdateStep(intserverId, 1);
                        oAsset.Update(intAsset, (int)AssetAttribute.Reserve);
                        //string strHostName = "";
                        //oAsset.AddStatus(intAsset, strHostName, (int)AssetStatus.Reserved, intProfile, DateTime.Now);
                        //Based on the asset selection create name for hosts
                        //intServerName = oServerName.Add(intClass, intEnv, intAddress, strNodePrefix, intProfile, oAsset.Get(intAsset, "serial"), 1);
                        //string strHostName = oServerName.GetName(intServerName, 0);
                        //
                   
                }
                //Set the server step to one
                DataSet dsServer = oServer.GetRequests(intRequestId, 1);
                foreach (DataRow dr in dsServer.Tables[0].Rows)
                {
                    oServer.UpdateStep(Int32.Parse(dr["id"].ToString()), 1);
                }
            }
            ////if (Int32.Parse(rblASEOrderType.SelectedValue) == (int)AssetSharedEnvOrderType.AddHost)//for Host -Add record in Host Table
            ////{
            ////    intVMWareClusterId = 123456789;
            ////    int intNumber = 0;
            ////    int intDomain = 0;  // josh
            ////    int intOS = 0;  // josh
            ////    int intSP = 0;  // josh
            ////    string strPrefix = "XNV";
            ////    int intSelectedDRCount = 0;
            ////    foreach (DataListItem dlItem in dlAssetsSelection.Items)
            ////    {
            ////        HiddenField hdnAssetId = (HiddenField)dlItem.FindControl("hdnAssetId");
            ////        CheckBox chkSelectAsset = (CheckBox)dlItem.FindControl("chkSelectAsset");
            ////        if (chkSelectAsset.Checked == true)
            ////        {
            ////            //Mark the Asset to Instock
            ////            //oAsset.UpdateStatus(Int32.Parse(hdnAssetId.Value), "", (int)AssetStatus.InStock, intProfile, DateTime.Now);
            ////            intNumber++;
            ////            int intserverId = oServer.Add(intRequestId, 0, intModel, 0, 0, intNumber, intOS, intSP, 0, intDomain, 0, 1, 0, (intSelectedDRCount > 0 ? 1 : 0), 0, "", 0, 0, 1, 1, 1, 1, 0, (oClass.Get(intClass, "pnc") == "1" ? 1 : 0), intVMWareClusterId, 0);
            ////            int intAsset = Int32.Parse(hdnAssetId.Value);
            ////            oServer.AddAsset(intserverId, intAsset, intClass, intEnvironment, 0, 0);
            ////            //Based on the asset selection create name for hosts
            ////            //intServerName = oServerName.Add(intClass, intEnv, intAddress, strPrefix, intProfile, oAsset.Get(intAsset, "serial"), 1);
            ////            //string strHostName = oServerName.GetName(intServerName, 0);
            ////            //oAsset.AddStatus(intAsset, strHostName, (int)AssetStatus.InUse, intProfile, DateTime.Now);


            ////        }
            ////    }
            ////}



        }

        protected void InitiateAddStorageWorkOrder()
        {

            ServiceDetails oServiceDetail = new ServiceDetails(intProfile, dsn);
            int intRequest = Int32.Parse(hdnRequestId.Value);
            int intServiceId = 0;
            intServiceId = Int32.Parse(ConfigurationManager.AppSettings["SERVICEID_ASSET_SHARED_ENV_ADD_STORAGE"]);
            string strServerTitle = "Inventory Management - Shared Environment Add Storage";
            int intItemID = oService.GetItemId(intServiceId);
            int intNumber = 1;
            double dblServiceHours = oServiceDetail.GetHours(intServiceId, 1);
            int intResource = oServiceRequest.AddRequest(intRequest, intItemID, intServiceId, 1, dblServiceHours, 2, intNumber, dsnServiceEditor);
            oServiceRequest.Update(intRequest, strServerTitle);
            oResourceRequest.UpdateName(intResource, strServerTitle);
            oServiceRequest.Add(intRequest, 1, 1);
        
        }

        protected void PopulateAvailableAssets()
        {

            intModel = Int32.Parse(ddlModel.SelectedValue.ToString());
            lblAssetsSelectedCount.Text = "0";

            pnlAssetsAvailable.Visible = true;
            dlAssetsSelection.DataSource = null;
            dlAssetsSelection.DataBind();
            string strFilterCriteria = "";
            strFilterCriteria = strFilterCriteria + " LocationId =" + hdnLocation.Value;
            strFilterCriteria = strFilterCriteria + " AND ";
            strFilterCriteria = strFilterCriteria + " ClassId =" + ddlClass.SelectedValue.ToString();
            strFilterCriteria = strFilterCriteria + " AND ";
            strFilterCriteria = strFilterCriteria + " EnvironmentId =" + hdnEnvironment.Value;
            strFilterCriteria = "( " + strFilterCriteria + " )";

            string strSort = "AssetSerial";

            DataSet ds = oAsset.GetAssetsAll(intModel, (int)AssetStatus.Available);

            DataTable dtAssets = null;
            dtAssets = ds.Tables[0];
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



            DataRow[] drSelect = null;
            drSelect = dtAssets.Select(strFilterCriteria, strSort);
            dlAssetsSelection.DataSource = drSelect;
            dlAssetsSelection.DataBind();


         
        }

        protected void PopulateAssetsSelectedOnly()
        {
            DataTable dtSelected = null;
            DataSet dsAssetSelection = oAssetOrder.GetAssetOrderAssetSelection(Int32.Parse(hdnOrderId.Value));
            lblAssetsSelectedCount.Text = "0";

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

        protected void PopulateLocations()
        {

            if (hdnLocation.Value == "") hdnLocation.Value = "0";
            strLocation = oLocation.LoadDDL("ddlState", "ddlCity", "ddlAddress", hdnLocation.ClientID, Int32.Parse(hdnLocation.Value), true, "ddlCommon");

        }

        protected void PopulateSelection()
        {
            PopulateLocations();

            ddlEnvironment.DataTextField = "name";
            ddlEnvironment.DataValueField = "id";
            ddlEnvironment.DataSource = oClass.GetEnvironment(Int32.Parse(ddlClass.SelectedValue), 0);
            ddlEnvironment.DataBind();
            ddlEnvironment.Items.Insert(0, new ListItem("-- SELECT --", "0"));

            ddlEnvironment.SelectedValue = (hdnEnvironment.Value != "" ? hdnEnvironment.Value : "0");
        }

        #endregion


        #region Available & Selected Host Assets

        protected void dlAssetsSelection_ItemDataBound(object sender, DataListItemEventArgs e)
        {

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                DataRow drv = (DataRow)e.Item.DataItem;

                HiddenField hdnAssetId = (HiddenField)e.Item.FindControl("hdnAssetId");
                hdnAssetId.Value = drv["AssetId"].ToString();

                CheckBox chkSelectAsset = (CheckBox)e.Item.FindControl("chkSelectAsset");
                chkSelectAsset.Attributes.Add("onclick", "return getSelectedAssetsCount();");

                Label lblAssetsSelectionSerial = (Label)e.Item.FindControl("lblAssetsSelectionSerial");
                lblAssetsSelectionSerial.Text = drv["AssetSerial"].ToString();

                Label lblAssetsSelectionAssetTag = (Label)e.Item.FindControl("lblAssetsSelectionAssetTag");
                lblAssetsSelectionAssetTag.Text = drv["AssetTag"].ToString();

                Label lblAssetsSelectionLocation = (Label)e.Item.FindControl("lblAssetsSelectionLocation");

                if (drv["LocationId"].ToString() != "")
                {
                    string strLocationCommonName = oLocation.GetAddress(Int32.Parse(drv["LocationId"].ToString()), "commonname");
                    if (strLocationCommonName == "")
                        lblAssetsSelectionLocation.Text = drv["Location"].ToString();
                    else
                        lblAssetsSelectionLocation.Text = strLocationCommonName;
                }


                Label lblAssetsSelectionClass = (Label)e.Item.FindControl("lblAssetsSelectionClass");
                lblAssetsSelectionClass.Text = drv["Class"].ToString();

                Label lblAssetsSelectionEnvironment = (Label)e.Item.FindControl("lblAssetsSelectionEnvironment");
                lblAssetsSelectionEnvironment.Text = drv["Environment"].ToString();


                //Previous Selection
                DataSet dsAssetSelected = oAssetOrder.GetAssetOrderAssetSelection(
                                                                    Int32.Parse(hdnOrderId.Value),
                                                                    Int32.Parse(hdnAssetId.Value));
                    if (dsAssetSelected.Tables[0].Rows.Count == 1)
                    {
                        int intTotalAssetsSelected = Int32.Parse(lblAssetsSelectedCount.Text) + 1;
                        lblAssetsSelectedCount.Text = intTotalAssetsSelected.ToString();
                        chkSelectAsset.Checked = true;
                    }
                    
            }

        }

        #endregion

        private void SetControls(bool _boolViewMode, bool _boolClearControls)
        {
            bool boolEnabled = true;
            if (_boolViewMode == true)
                boolEnabled = false;

            rblASEOrderType.Enabled = boolEnabled;
            if (_boolClearControls == true) rblASEOrderType.SelectedIndex = -1;

            txtDescription.Enabled = boolEnabled;
            if (_boolClearControls == true) txtDescription.Text = "";

            ddlModel.Enabled = boolEnabled;
            if (_boolClearControls == true) ddlModel.SelectedValue = "0";

            if (_boolClearControls == true) hdnLocation.Value = "0";

            ddlClass.Enabled = boolEnabled;
            if (_boolClearControls == true) ddlClass.SelectedValue = "0";

            ddlEnvironment.Enabled = boolEnabled;
            if (_boolClearControls == true) ddlEnvironment.SelectedIndex = -1;

            txtStorageAmt.Enabled = boolEnabled;
            if (_boolClearControls == true) txtStorageAmt.Text = "0.00";

            txtRequestedByDate.Enabled = boolEnabled;
            if (_boolClearControls == true) txtRequestedByDate.Text = "";

            rblClusterTypes.Enabled = boolEnabled;
            if (_boolClearControls == true) rblClusterTypes.SelectedIndex = -1;
            //chkFunctionWorkstation.Checked = false;
            //chkFunctionServer.Checked = false;
            //chkFunctionWindows.Checked = false;
            //chkFunctionLinux.Checked = false;
            chkFunctionContainer.Checked = false;
            chkFunctionLDOM.Checked = false;

            tvParent.Enabled = boolEnabled;
            if (_boolClearControls == true) tvParent.Nodes.Clear();

            if (_boolClearControls == true)
            {

                //hdnOrderId.Value = "0";
                hdnRequestId.Value = "0";
                hdnItemId.Value = "0";
                hdnNumber.Value = "0";
                hdnLocation.Value = "0";
                hdnEnvironment.Value = "0";
                hdnParentId.Value = "0";
                lblAssetsSelectedCount.Text = "0";
                pnlAssetsAvailable.Visible = false;
                dlAssetsSelection.DataSource = null;
                dlAssetsSelection.DataBind();
            }

            btnSubmitRequest.Enabled = boolEnabled;
            btnSave.Enabled = boolEnabled;

            PopulateLocations();


        }
    }
}