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
    public partial class rr_server_decommission_NEW : System.Web.UI.UserControl
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intCore = Int32.Parse(ConfigurationManager.AppSettings["CoreEnvironmentID"]);
        protected int intLocation = Int32.Parse(ConfigurationManager.AppSettings["OPSLocationID"]);
        protected Pages oPage;
        protected RequestItems oRequestItem;
        protected Requests oRequest;
        protected Servers oServer;
        protected Classes oClass;
        protected Environments oEnvironment;
        protected Locations oLocation;
        protected Asset oAsset;
        protected Applications oApplication;
        protected Forecast oForecast;
        protected Functions oFunction;
        protected ModelsProperties oModelsProperties;
        protected Users oUser;
        protected Customized oCustomized;
        protected Platforms oPlatform;
        protected int intProfile;
        protected int intPage = 0;
        protected int intApplication = 0;
        protected string strContacts = "";
        protected string strLocation = "";
        protected string strEdit = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oPage = new Pages(intProfile, dsn);
            oRequestItem = new RequestItems(intProfile, dsn);
            oRequest = new Requests(intProfile, dsn);
            oServer = new Servers(intProfile, dsn);
            oClass = new Classes(intProfile, dsn);
            oEnvironment = new Environments(intProfile, dsn);
            oLocation = new Locations(intProfile, dsn);
            oAsset = new Asset(intProfile, dsnAsset);
            oApplication = new Applications(intProfile, dsn);
            oForecast = new Forecast(intProfile, dsn);
            oFunction = new Functions(intProfile, dsn, intEnvironment);
            oModelsProperties = new ModelsProperties(intProfile, dsn);
            oUser = new Users(intProfile, dsn);
            oCustomized = new Customized(intProfile, dsn);
            oPlatform = new Platforms(intProfile, dsn);
            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            if (Request.QueryString["rid"] != "" && Request.QueryString["rid"] != null)
            {
                LoadValues();
                int intItem = Int32.Parse(lblItem.Text);
                int intApp = oRequestItem.GetItemApplication(intItem);
                string strDeliverable = oApplication.Get(intApp, "deliverables_doc");
                if (strDeliverable != "")
                {
                    panDeliverable.Visible = true;
                    btnDeliverable.Attributes.Add("onclick", "return OpenWindow('NEW_WINDOW','" + strDeliverable + "');");
                }
                if (Request.QueryString["n"] != null && Request.QueryString["n"] != "")
                {
                    if (!IsPostBack)
                        LoadServer();
                }
                else if (Request.QueryString["formid"] == null || Request.QueryString["formid"] == "")
                    btnNext.Enabled = false;
                btnContinue.Attributes.Add("onclick", "return ValidateText('" + txtName.ClientID + "','Please enter a server name')" +
                    " && ProcessButton(this)" +
                    ";");
            }
            strLocation = oLocation.LoadDDL("ddlState", "ddlCity", "ddlAddress", hdnLocation.ClientID, intLocation, true, "ddlCommon");
            hdnLocation.Value = intLocation.ToString();
            ddlClass.Attributes.Add("onchange", "WaitDDL('" + divClass.ClientID + "');");
            ddlEnvironment.Attributes.Add("onchange", "WaitDDL('" + divEnvironment.ClientID + "');");
            txtName.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnContinue.ClientID + "').click();return false;}} else {return true}; ");
            imgDate.Attributes.Add("onclick", "return ShowCalendar('" + txtDate.ClientID + "');");
            btnCancel1.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this service request?');");
            radRetrieveYes.Attributes.Add("onclick", "ShowHideDiv('" + trRetrieve1.ClientID + "','inline');ShowHideDiv('" + trRetrieve2.ClientID + "','inline');ShowHideDiv('" + trRetrieve3.ClientID + "','inline');");
            radRetrieveNo.Attributes.Add("onclick", "ShowHideDiv('" + trRetrieve1.ClientID + "','none');ShowHideDiv('" + trRetrieve2.ClientID + "','none');ShowHideDiv('" + trRetrieve3.ClientID + "','none');");
            imgPower.Attributes.Add("onclick", "return ShowCalendar('" + txtPower.ClientID + "');");
        }
        private void LoadValues()
        {
            //int intRequest = Int32.Parse(Request.QueryString["rid"]);
            //DataSet dsItems = oRequestItem.GetForms(intRequest);
            //if (dsItems.Tables[0].Rows.Count > 0)
            //{
            //    foreach (DataRow drItem in dsItems.Tables[0].Rows)
            //    {
            //        if (drItem["done"].ToString() == "-1")
            //        {
            //            lblItem.Text = drItem["itemid"].ToString();
            //            lblNumber.Text = drItem["number"].ToString();
            //            break;
            //        }
            //    }
            //}
            int intRequest = Int32.Parse(Request.QueryString["rid"]);
            DataSet dsItems;
            int intForm = 0;
            if (Request.QueryString["formid"] != null && Request.QueryString["formid"] != "")
            {
                intForm = Int32.Parse(Request.QueryString["formid"]);
                dsItems = oRequestItem.GetForm(intRequest, intForm);
            }
            else
                dsItems = oRequestItem.GetForms(intRequest);
            if (dsItems.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow drItem in dsItems.Tables[0].Rows)
                {
                    if (drItem["done"].ToString() == "-1" || intForm > 0)
                    {
                        lblItem.Text = drItem["itemid"].ToString();
                        if (intForm > 0 && Request.QueryString["num"] != null && Request.QueryString["num"] != "")
                            lblNumber.Text = Request.QueryString["num"];
                        else
                            lblNumber.Text = drItem["number"].ToString();
                        break;
                    }
                }
            }
            // Load Data
            if (Request.QueryString["formid"] != null && Request.QueryString["formid"] != "")
            {
                int intItem = Int32.Parse(lblItem.Text);
                int intNumber = Int32.Parse(lblNumber.Text);
                DataSet dsEdit = oCustomized.GetDecommissionServer(intRequest, intItem, intNumber);
                if (dsEdit.Tables[0].Rows.Count > 0)
                {
                    DataRow drEdit = dsEdit.Tables[0].Rows[0];
                    panEdit.Visible = true;
                    txtName.Text = drEdit["servername"].ToString();
                    btnContinue.Enabled = false;
                    //btnBack.Enabled = false;
                    strEdit = oCustomized.GetDecommissionServerBody(intRequest, intItem, intNumber, dsnAsset);
                    txtPower.Text = drEdit["poweroff_new"].ToString();
                    if (drEdit["poweroff_new"].ToString() == "")
                        txtPower.Text = DateTime.Parse(drEdit["poweroff"].ToString()).ToShortDateString();
                    else
                        txtPower.Text = DateTime.Parse(drEdit["poweroff_new"].ToString()).ToShortDateString();
                }
                btnNext.Text = "Update";
                btnBack.Text = "Cancel";
                btnNext.Attributes.Add("onclick", "return ValidateDate('" + txtPower.ClientID + "','Please enter a valid date')" +
                    " && ValidateDateToday('" + txtPower.ClientID + "','The date must occur after today')" +
                    " && ProcessButton(this)" +
                    ";");
                btnCancel1.Visible = false;
            }
        }
        protected void btnContinue_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(oPage.GetFullLink(intPage) + "?rid=" + Request.QueryString["rid"] + "&n=" + oFunction.encryptQueryString(txtName.Text.Trim()));
        }
        private void LoadServer()
        {
            string strName = oFunction.decryptQueryString(Request.QueryString["n"]);
            txtName.Text = strName;
            bool boolFound = false;
            bool boolAlready = false;

            DataSet dsRecom = oAsset.GetDecommissionRecommission(strName);
            foreach (DataRow drRecom in dsRecom.Tables[0].Rows)
            {
                if (drRecom["name"].ToString().ToUpper().Trim() == strName.ToUpper().Trim())
                {
                    DateTime datDecom = DateTime.Now;
                    boolAlready = true;
                    lblAlreadySerial.Text = drRecom["serial"].ToString();
                    lblAlreadySerialDR.Text = drRecom["serialdr"].ToString();
                    lblAlreadyBy.Text = drRecom["username"].ToString();
                    lblAlreadyOn.Text = drRecom["created"].ToString();
                    lblAlreadyReason.Text = drRecom["reason"].ToString();
                    lblAlreadyPower.Text = drRecom["decom"].ToString();
                    if (drRecom["running"].ToString() == "-1")
                        lblAlreadyStatus.Text = "Manual Intervention Required";
                    else if (drRecom["running"].ToString() == "-2")
                    {
                        boolAlready = false;
                        lblAlreadyStatus.Text = "Cancelled";
                    }
                    else if (drRecom["running"].ToString() == "2")
                        lblAlreadyStatus.Text = "In Progress";
                    else if (drRecom["running"].ToString() == "3")
                        lblAlreadyStatus.Text = "Completed";
                    else if (drRecom["running"].ToString() == "1")
                        lblAlreadyStatus.Text = "Running...";
                    else if (drRecom["running"].ToString() == "0")
                    {
                        if (drRecom["recommissioned"].ToString() != "")
                        {
                            boolAlready = false;
                            lblAlreadyStatus.Text = "Recommissioned on " + drRecom["recommissioned"].ToString();
                        }
                        else if (drRecom["destroyed"].ToString() != "")
                        {
                            lblAlreadyStatus.Text = "Powered off on " + drRecom["turnedoff"].ToString();
                            lblAlreadyStatus.Text += "<br/>Finished on " + drRecom["destroyed"].ToString();
                        }
                        else if (drRecom["destroy"].ToString() != "")
                        {
                            lblAlreadyStatus.Text = "Powered off on " + drRecom["turnedoff"].ToString();
                            lblAlreadyStatus.Text += "<br/>Will be finished on " + drRecom["destroy"].ToString();
                        }
                        else if (drRecom["turnedoff"].ToString() != "")
                            lblAlreadyStatus.Text = "Powered off on " + drRecom["turnedoff"].ToString();
                        else
                            lblAlreadyStatus.Text = "Will be powered off on " + drRecom["decom"].ToString();
                    }
                    else
                        lblAlreadyStatus.Text = "Status Unavailable";
                }
            }

            if (boolAlready == false)
            {
                DataSet ds = oServer.GetDecommission(strName);
                if (ds.Tables[0].Rows.Count == 1)
                {
                    boolFound = true;
                    bool boolPermit = false;
                    int intServer = Int32.Parse(ds.Tables[0].Rows[0]["id"].ToString());

                    int intRequest = Int32.Parse(Request.QueryString["rid"]);
                    int intProject = oRequest.GetProjectNumber(intRequest);
                    Projects oProject = new Projects(intProfile, dsn);
                    bool boolDemo = false;
                    string strNumber = oProject.Get(intProject, "number");
                    // Check to see if Demo
                    DataSet dsDemo = oFunction.GetSetupValuesByKey("DEMO_PROJECT");
                    foreach (DataRow drDemo in dsDemo.Tables[0].Rows)
                    {
                        if (strNumber == drDemo["Value"].ToString())
                        {
                            boolDemo = true;
                            break;
                        }
                    }

                    if (boolDemo == false && 
                        (String.IsNullOrEmpty(oServer.Get(intServer, "build_ready")) == true
                        || oServer.Get(intServer, "rebuilding") == "1"))
                    {
                        panBuilding.Visible = true;
                        btnNext.Enabled = false;
                    }
                    else
                    {
                        int intAnswer = 0;
                        if (ds.Tables[0].Rows[0]["answerid"].ToString() != "")
                            intAnswer = Int32.Parse(ds.Tables[0].Rows[0]["answerid"].ToString());
                        int intUser = 0;
                        if (ds.Tables[0].Rows[0]["userid"].ToString() != "")
                            intUser = Int32.Parse(ds.Tables[0].Rows[0]["userid"].ToString());
                        int intOwner = 0;
                        int intPrimary = 0;
                        int intSecondary = 0;
                        int intRequestor = 0;
                        string strAgree = "";
                        if (intAnswer > 0)
                        {
                            if (oForecast.GetAnswer(intAnswer, "appcontact") != "")
                                intOwner = Int32.Parse(oForecast.GetAnswer(intAnswer, "appcontact"));
                            if (oForecast.GetAnswer(intAnswer, "admin1") != "")
                                intPrimary = Int32.Parse(oForecast.GetAnswer(intAnswer, "admin1"));
                            if (oForecast.GetAnswer(intAnswer, "admin2") != "")
                                intSecondary = Int32.Parse(oForecast.GetAnswer(intAnswer, "admin2"));
                            if (oForecast.GetAnswer(intAnswer, "userid") != "")
                                intRequestor = Int32.Parse(oForecast.GetAnswer(intAnswer, "userid"));
                            if (intProfile == intOwner || intProfile == intPrimary || intProfile == intSecondary || intProfile == intRequestor)
                                boolPermit = true;

                            if (intOwner > 0)
                                strAgree += " - " + oUser.GetFullName(intOwner) + " (" + oUser.GetName(intOwner) + ")\\n";
                            if (intPrimary > 0)
                                strAgree += " - " + oUser.GetFullName(intPrimary) + " (" + oUser.GetName(intPrimary) + ")\\n";
                            if (intSecondary > 0)
                                strAgree += " - " + oUser.GetFullName(intSecondary) + " (" + oUser.GetName(intSecondary) + ")\\n";
                            if (intRequestor > 0)
                                strAgree += " - " + oUser.GetFullName(intRequestor) + " (" + oUser.GetName(intRequestor) + ")\\n";
                        }
                        if (oApplication.Get(intApplication, "decom") == "1" || oUser.IsAdmin(intProfile) || intProfile == intUser)
                            boolPermit = true;

                        if (boolPermit == true)
                        {
                            chkAgree.Attributes.Add("onclick", "ShowAgreeAlert(this,\"" + strAgree + "\");");
                            int intAsset = 0;
                            int intAssetDR = 0;
                            DataSet dsAsset = oServer.GetAssets(intServer);
                            foreach (DataRow drAsset in dsAsset.Tables[0].Rows)
                            {
                                if (drAsset["latest"].ToString() == "1")
                                    intAsset = Int32.Parse(drAsset["assetid"].ToString());
                                if (drAsset["dr"].ToString() == "1")
                                    intAssetDR = Int32.Parse(drAsset["assetid"].ToString());
                            }

                            if (intAsset > 0)
                            {
                                panFound.Visible = true;
                                panDetail.Visible = true;

                                int intClass = 0;
                                int intEnv = 0;
                                int intAddress = 0;
                                try
                                {
                                    intClass = Int32.Parse(oAsset.GetServerOrBlade(intAsset, "classid"));
                                    intEnv = Int32.Parse(oAsset.GetServerOrBlade(intAsset, "environmentid"));
                                    intAddress = Int32.Parse(oAsset.GetServerOrBlade(intAsset, "addressid"));
                                }
                                catch
                                {
                                    DataSet dsCatch = oServer.GetAsset(intAsset);
                                    if (dsCatch.Tables[0].Rows.Count > 0)
                                    {
                                        intClass = Int32.Parse(dsCatch.Tables[0].Rows[0]["classid"].ToString());
                                        intEnv = Int32.Parse(dsCatch.Tables[0].Rows[0]["environmentid"].ToString());
                                        intAddress = Int32.Parse(dsCatch.Tables[0].Rows[0]["addressid"].ToString());
                                    }
                                }
                                if (intClass == 0 && intAnswer > 0)
                                    Int32.TryParse(oForecast.GetAnswer(intAnswer, "classid"), out intClass);
                                if (intEnv == 0 && intAnswer > 0)
                                    Int32.TryParse(oForecast.GetAnswer(intAnswer, "environmentid"), out intEnv);
                                if (intAddress == 0 && intAnswer > 0)
                                    Int32.TryParse(oForecast.GetAnswer(intAnswer, "addressid"), out intAddress);
                                int intModel = 0;
                                if (oAsset.Get(intAsset, "modelid") != "")
                                    intModel = Int32.Parse(oAsset.Get(intAsset, "modelid"));
                                lblModel.Text = oModelsProperties.Get(intModel, "name");
                                lblModel.ToolTip = "ModelID: " + intModel.ToString();
                                lblSerial.Text = oAsset.Get(intAsset, "serial");
                                lblSerial.ToolTip = "AssetID: " + intAsset.ToString();
                                if (intAssetDR > 0)
                                {
                                    panDR.Visible = true;
                                    lblSerialDR.Text = oAsset.Get(intAssetDR, "serial");
                                    lblSerialDR.ToolTip = "AssetID: " + intAssetDR.ToString();
                                }
                                lblClass.Text = oClass.Get(intClass, "name");
                                lblClass.ToolTip = intClass.ToString();
                                lblEnvironment.Text = oEnvironment.Get(intEnv, "name");
                                lblAddress.Text = oLocation.GetFull(intAddress);
                                panValid.Visible = true;
                                lblId.Text = intServer.ToString();
                                btnNext.Attributes.Add("onclick", "return ValidateCheck('" + chkAgree.ClientID + "','Please check the box stating that you agree to the disclaimer notice')" +
                                    " && ValidateDate('" + txtDate.ClientID + "','Please enter a valid date')" +
                                    " && ValidateDateToday('" + txtDate.ClientID + "','The date must occur after today')" +
                                    GetClass(intServer, intClass, intEnv) +
                                    " && ValidateText('" + txtReason.ClientID + "','Please enter a reason')" +
                                    " && ValidateRadioButtons('" + radRetrieveYes.ClientID + "','" + radRetrieveNo.ClientID + "','Please select whether or not special hardware should be retrieved')" +
                                    " && (document.getElementById('" + radRetrieveYes.ClientID + "').checked == false || (document.getElementById('" + radRetrieveYes.ClientID + "').checked == true && ValidateText('" + txtRetrieve.ClientID + "','Please provide a description')))" +
                                    " && (document.getElementById('" + radRetrieveYes.ClientID + "').checked == false || (document.getElementById('" + radRetrieveYes.ClientID + "').checked == true && ValidateText('" + txtRetrieveAddress.ClientID + "','Please provide an address')))" +
                                    " && (document.getElementById('" + radRetrieveYes.ClientID + "').checked == false || (document.getElementById('" + radRetrieveYes.ClientID + "').checked == true && ValidateText('" + txtRetrieveLocator.ClientID + "','Please provide a locator')))" +
                                    " && ProcessButton(this)" +
                                    ";");
                            }
                            else
                                boolFound = false;
                        }
                        else
                        {
                            panInvalid.Visible = true;
                            if (intUser > 0)
                                strContacts += "<tr><td>Server Owner:</td><td>" + oUser.GetFullName(intUser) + " (" + oUser.GetName(intUser) + ")" + "</td></tr>";
                            if (intOwner > 0)
                                strContacts += "<tr><td>Departmental Manager:</td><td>" + oUser.GetFullName(intOwner) + " (" + oUser.GetName(intOwner) + ")" + "</td></tr>";
                            if (intPrimary > 0)
                                strContacts += "<tr><td>Application Technical Lead:</td><td>" + oUser.GetFullName(intPrimary) + " (" + oUser.GetName(intPrimary) + ")" + "</td></tr>";
                            if (intSecondary > 0)
                                strContacts += "<tr><td>Administrative Contact:</td><td>" + oUser.GetFullName(intSecondary) + " (" + oUser.GetName(intSecondary) + ")" + "</td></tr>";
                            if (intRequestor > 0)
                                strContacts += "<tr><td>Design Initiated By:</td><td>" + oUser.GetFullName(intRequestor) + " (" + oUser.GetName(intRequestor) + ")" + "</td></tr>";
                            DataSet dsDecoms = oApplication.GetDecoms();
                            if (dsDecoms.Tables[0].Rows.Count > 0)
                            {
                                strContacts += "<tr><td colspan=\"2\">Alternatively, you can contact a resource from one of the following departments:</td></tr>";
                                foreach (DataRow drDecom in dsDecoms.Tables[0].Rows)
                                    strContacts += "<tr><td></td><td>" + drDecom["name"].ToString() + "</td></tr>";
                            }
                            btnNext.Enabled = false;
                        }
                    }
                }
                else if (ds.Tables[0].Rows.Count > 1)
                {
                    boolFound = true;
                    panMore.Visible = true;
                    btnNext.Enabled = false;
                }

                if (boolFound == false)
                {
                    panNotFound.Visible = true;
                    lblName.Text = strName;
                    if (Request.QueryString["yn"] != null)
                    {
                        if (Request.QueryString["yn"] == "y")
                        {
                            panConfirm.Visible = true;
                            radYes.Checked = true;
                            LoadLists();
                            int intClass = 0;
                            if (Request.QueryString["cid"] != null && Request.QueryString["cid"] != "")
                                intClass = Int32.Parse(Request.QueryString["cid"]);
                            if (oClass.Get(intClass).Tables[0].Rows.Count > 0)
                            {
                                panClass.Visible = true;
                                ddlClass.SelectedValue = intClass.ToString();
                                ddlEnvironment.Enabled = true;
                                Environments oEnvironment = new Environments(intProfile, dsn);
                                ddlEnvironment.DataValueField = "id";
                                ddlEnvironment.DataTextField = "name";
                                ddlEnvironment.DataSource = oClass.GetEnvironment(intClass, 0);
                                ddlEnvironment.DataBind();
                                ddlEnvironment.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                                int intEnv = 0;
                                if (Request.QueryString["eid"] != null && Request.QueryString["eid"] != "")
                                    intEnv = Int32.Parse(Request.QueryString["eid"]);
                                if (oEnvironment.Get(intEnv).Tables[0].Rows.Count > 0)
                                {
                                    panEnvironment.Visible = true;
                                    panDetail.Visible = true;
                                    ddlEnvironment.SelectedValue = intEnv.ToString();
                                    lblId.Text = "0";
                                    btnNext.Attributes.Add("onclick", "return ValidateDropDown('" + ddlPlatform.ClientID + "','Please select a platform')" +
                                        " && ValidateDropDown('" + ddlPlatformType.ClientID + "','Please select a type')" +
                                        " && ValidateDropDown('" + ddlPlatformModel.ClientID + "','Please select a model')" +
                                        " && ValidateDropDown('" + ddlPlatformModelProperty.ClientID + "','Please select a model property')" +
                                        " && ValidateText('" + txtSerial.ClientID + "','Please enter a serial number')" +
                                        " && ValidateHidden0('" + hdnLocation.ClientID + "','ddlState','Please select a location')" +
                                        " && ValidateCheck('" + chkAgree.ClientID + "','Please check the box stating that you agree to the disclaimer notice')" +
                                        " && ValidateDate('" + txtDate.ClientID + "','Please enter a valid date')" +
                                        " && ValidateDateToday('" + txtDate.ClientID + "','The date must occur after today')" +
                                        GetClass(0, intClass, intEnv) +
                                        " && ValidateText('" + txtReason.ClientID + "','Please enter a reason')" +
                                        " && ValidateRadioButtons('" + radRetrieveYes.ClientID + "','" + radRetrieveNo.ClientID + "','Please select whether or not special hardware should be retrieved')" +
                                        " && (document.getElementById('" + radRetrieveYes.ClientID + "').checked == false || (document.getElementById('" + radRetrieveYes.ClientID + "').checked == true && ValidateText('" + txtRetrieve.ClientID + "','Please provide a description')))" +
                                        " && (document.getElementById('" + radRetrieveYes.ClientID + "').checked == false || (document.getElementById('" + radRetrieveYes.ClientID + "').checked == true && ValidateText('" + txtRetrieveAddress.ClientID + "','Please provide an address')))" +
                                        " && (document.getElementById('" + radRetrieveYes.ClientID + "').checked == false || (document.getElementById('" + radRetrieveYes.ClientID + "').checked == true && ValidateText('" + txtRetrieveLocator.ClientID + "','Please provide a locator')))" +
                                        " && ProcessButton(this)" +
                                        ";");
                                }
                                else
                                    btnNext.Enabled = false;
                            }
                            else
                                btnNext.Enabled = false;
                        }
                    }
                    else
                        btnNext.Enabled = false;
                }
            }
            else
            {
                panAlready.Visible = true;
                btnNext.Enabled = false;
            }
        }
        private string GetClass(int _serverid, int _classid, int _environmentid)
        {
            string strReturn = "";
            //if (oClass.IsProd(_classid) || (_serverid > 0 && oServer.Get(_serverid, "infrastructure") == "1") || (oClass.IsDR(_classid) && _environmentid == intCore))
            //{
            //    if (oClass.IsProd(_classid))
            //        lblChange.Text = "Production Class";
            //    else if (_serverid > 0 && oServer.Get(_serverid, "infrastructure") == "1")
            //        lblChange.Text = "Infrastructure Server";
            //    else if (oClass.IsDR(_classid) && _environmentid == intCore)
            //        lblChange.Text = "Hot DR Class";
            //    panChange.Visible = true;
            //    strReturn = " && ValidateTextLength('" + txtChange.ClientID + "', 'Please enter a valid change control number\\n\\n - Must start with either \"CHG\" or \"PTM\"\\n - Must be exactly 10 characters in length', 10, ['CHG','PTM'], ['CHG0000000','PTM0000000','CHG1111111','PTM1111111','CHG9999999','PTM9999999','CHGXXXXXXX','PTMXXXXXXX'])";
            //}
            lblChange.Text = "Effective 12/7/12, all servers (regardless of environment or function) require an approved change control";
            panChange.Visible = true;
            strReturn = " && ValidateTextLength('" + txtChange.ClientID + "', 'Please enter a valid change control number\\n\\n - Must start with either \"CHG\" or \"PTM\"\\n - Must be exactly 10 characters in length', 10, ['CHG','PTM'], ['CHG0000000','PTM0000000','CHG1111111','PTM1111111','CHG9999999','PTM9999999','CHGXXXXXXX','PTMXXXXXXX'])";
            return strReturn;
        }
        protected void LoadLists()
        {
            ddlPlatform.Attributes.Add("onchange", "PopulatePlatformTypes('" + ddlPlatform.ClientID + "','" + ddlPlatformType.ClientID + "','" + ddlPlatformModel.ClientID + "','" + ddlPlatformModelProperty.ClientID + "');ResetDropDownHidden('" + hdnModel.ClientID + "');");
            ddlPlatform.DataTextField = "name";
            ddlPlatform.DataValueField = "platformid";
            ddlPlatform.DataSource = oPlatform.Gets(1);
            ddlPlatform.DataBind();
            ddlPlatform.Items.Insert(0, new ListItem("-- SELECT --", "0"));
            ddlPlatformType.Attributes.Add("onchange", "PopulatePlatformModels('" + ddlPlatformType.ClientID + "','" + ddlPlatformModel.ClientID + "','" + ddlPlatformModelProperty.ClientID + "');ResetDropDownHidden('" + hdnModel.ClientID + "');");
            ddlPlatformType.Items.Insert(0, new ListItem("-- Select a platform --", "0"));
            ddlPlatformType.Enabled = false;
            ddlPlatformModel.Attributes.Add("onchange", "PopulatePlatformModelPropertiesAll('" + ddlPlatformModel.ClientID + "','" + ddlPlatformModelProperty.ClientID + "');ResetDropDownHidden('" + hdnModel.ClientID + "');");
            ddlPlatformModel.Items.Insert(0, new ListItem("-- Select a type --", "0"));
            ddlPlatformModel.Enabled = false;
            ddlPlatformModelProperty.Attributes.Add("onchange", "UpdateDropDownHidden('" + ddlPlatformModelProperty.ClientID + "','" + hdnModel.ClientID + "');");
            ddlPlatformModelProperty.Items.Insert(0, new ListItem("-- Select a model --", "0"));
            ddlPlatformModelProperty.Enabled = false;
            ddlClass.DataTextField = "name";
            ddlClass.DataValueField = "id";
            ddlClass.DataSource = oClass.Gets(1);
            ddlClass.DataBind();
            ddlClass.Items.Insert(0, new ListItem("-- SELECT --", "0"));
        }
        protected void radYes_Change(Object Sender, EventArgs e)
        {
            Response.Redirect(oPage.GetFullLink(intPage) + "?rid=" + Request.QueryString["rid"] + "&n=" + Request.QueryString["n"] + "&yn=y");
        }
        protected void radNo_Change(Object Sender, EventArgs e)
        {
            Response.Redirect(oPage.GetFullLink(intPage) + "?rid=" + Request.QueryString["rid"]);
        }
        protected void ddlClass_Change(Object Sender, EventArgs e)
        {
            if (ddlClass.SelectedIndex > 0)
                Response.Redirect(oPage.GetFullLink(intPage) + "?rid=" + Request.QueryString["rid"] + "&n=" + Request.QueryString["n"] + "&yn=y" + "&cid=" + ddlClass.SelectedItem.Value);
            else
                Response.Redirect(oPage.GetFullLink(intPage) + "?rid=" + Request.QueryString["rid"] + "&n=" + Request.QueryString["n"] + "&yn=y");
        }
        protected void ddlEnvironment_Change(Object Sender, EventArgs e)
        {
            if (ddlEnvironment.SelectedIndex > 0)
                Response.Redirect(oPage.GetFullLink(intPage) + "?rid=" + Request.QueryString["rid"] + "&n=" + Request.QueryString["n"] + "&yn=y" + "&cid=" + Request.QueryString["cid"] + "&eid=" + ddlEnvironment.SelectedItem.Value);
            else
                Response.Redirect(oPage.GetFullLink(intPage) + "?rid=" + Request.QueryString["rid"] + "&n=" + Request.QueryString["n"] + "&yn=y" + "&cid=" + Request.QueryString["cid"]);
        }
        protected void btnBack_Click(Object Sender, EventArgs e)
        {
            int intRequest = Int32.Parse(Request.QueryString["rid"]);
            if (btnBack.Text != "Cancel")
                oRequestItem.UpdateForm(intRequest, false);
            Response.Redirect(oPage.GetFullLink(intPage) + "?rid=" + intRequest.ToString());
        }
        protected void btnNext_Click(Object Sender, EventArgs e)
        {
            int intRequest = Int32.Parse(Request.QueryString["rid"]);
            int intItem = Int32.Parse(lblItem.Text);
            int intNumber = Int32.Parse(lblNumber.Text);
            if (btnNext.Text != "Update")
            {
                int intServer = Int32.Parse(lblId.Text);
                int intClass = 0;
                int intEnv = 0;
                int intAddress = 0;
                int intModel = 0;
                if (panConfirm.Visible == true)
                {
                    Int32.TryParse(ddlClass.SelectedItem.Value, out intClass);
                    Int32.TryParse(ddlEnvironment.SelectedItem.Value, out intEnv);
                    Int32.TryParse(Request.Form[hdnLocation.UniqueID], out intAddress);
                    Int32.TryParse(Request.Form[hdnModel.UniqueID], out intModel);
                }
                if (intClass == 0)
                    Int32.TryParse(lblClass.ToolTip, out intClass);
                oCustomized.DeleteDecommissionServer(intServer);
                oCustomized.AddDecommissionServer(intRequest, intItem, intNumber, txtName.Text, intServer, DateTime.Parse(txtDate.Text), txtChange.Text, txtReason.Text, intClass, intEnv, intAddress, intModel, txtSerial.Text, txtDR.Text, (radRetrieveYes.Checked ? 1 : 0), (radRetrieveYes.Checked ? txtRetrieve.Text : ""), (radRetrieveYes.Checked ? txtRetrieveAddress.Text : ""), (radRetrieveYes.Checked ? txtRetrieveLocator.Text : ""));
                oRequestItem.UpdateForm(intRequest, true);
            }
            else
            {
                // Get Decom Record...
                bool boolDecomAuto = false;
                bool boolDecomManual = false;
                bool boolDecomSubmitted = false;
                DateTime datDecom = DateTime.Now.AddDays(1.00);

                DataSet dsDecommission = oAsset.GetDecommission(intRequest, intNumber, 2);
                if (dsDecommission.Tables[0].Rows.Count > 0)
                {
                    boolDecomAuto = true;
                    if (DateTime.TryParse(dsDecommission.Tables[0].Rows[0]["decom"].ToString(), out datDecom))
                        boolDecomSubmitted = (datDecom < DateTime.Now);
                }
                else
                {
                    dsDecommission = oCustomized.GetDecommissionServer(intRequest, intItem, intNumber);
                    if (dsDecommission.Tables[0].Rows.Count > 0)
                    {
                        boolDecomManual = true;
                        if (dsDecommission.Tables[0].Rows[0]["poweroff_new"].ToString() != "")
                        {
                            if (DateTime.TryParse(dsDecommission.Tables[0].Rows[0]["poweroff_new"].ToString(), out datDecom))
                                boolDecomSubmitted = (datDecom < DateTime.Now);
                        }
                        else if (DateTime.TryParse(dsDecommission.Tables[0].Rows[0]["poweroff"].ToString(), out datDecom))
                            boolDecomSubmitted = (datDecom < DateTime.Now);
                    }
                }

                if (boolDecomSubmitted == false)
                {
                    oCustomized.UpdateDecommissionServer(intRequest, intItem, intNumber, txtPower.Text);
                    if (boolDecomManual == false)
                        oAsset.UpdateDecommission(intRequest, intItem, intNumber, txtPower.Text);
                }
            }
            Response.Redirect(oPage.GetFullLink(intPage) + "?rid=" + intRequest.ToString());
        }
        protected void btnCancel_Click(Object Sender, EventArgs e)
        {
            int intRequest = Int32.Parse(Request.QueryString["rid"]);
            oRequest.Cancel(intRequest);
            Response.Redirect(oPage.GetFullLink(intPage));
        }
    }
}