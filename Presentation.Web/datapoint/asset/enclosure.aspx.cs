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
using NCC.ClearView.Presentation.Web.Custom;
using Microsoft.ApplicationBlocks.Data;

namespace NCC.ClearView.Presentation.Web
{
    public partial class enclosure : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);

        protected int intDataPointAvailableAsset = Int32.Parse(ConfigurationManager.AppSettings["DATAPOINT_AVAILABLE_ASSET"]);
        protected DataPoint oDataPoint;
        protected Users oUser;
        protected Servers oServer;
        protected Asset oAsset;
        protected Forecast oForecast;
        protected Platforms oPlatform;
        protected Types oType;
        protected Models oModel;
        protected ModelsProperties oModelsProperties;
        protected IPAddresses oIPAddresses;
        protected Functions oFunction;
        protected OperatingSystems oOperatingSystem;
        protected ServicePacks oServicePack;
        protected Classes oClass;
        protected Environments oEnvironment;
        protected Resiliency oResiliency;
        protected int intProfile = 0;
        protected int intApplication = 0;
        protected int intAsset = 0;
        protected string strMenuTab1 = "";
        protected int intHighlight = 0;
        private int intID = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            if (Request.Cookies["profileid"] != null && Request.Cookies["profileid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            oDataPoint = new DataPoint(intProfile, dsn);
            oUser = new Users(intProfile, dsn);
            oServer = new Servers(intProfile, dsn);
            oAsset = new Asset(intProfile, dsnAsset);
            oForecast = new Forecast(intProfile, dsn);
            oPlatform = new Platforms(intProfile, dsn);
            oType = new Types(intProfile, dsn);
            oModel = new Models(intProfile, dsn);
            oModelsProperties = new ModelsProperties(intProfile, dsn);
            oIPAddresses = new IPAddresses(intProfile, dsnIP, dsn);
            oFunction = new Functions(intProfile, dsn, intEnvironment);
            oOperatingSystem = new OperatingSystems(intProfile, dsn);
            oServicePack = new ServicePacks(intProfile, dsn);
            oClass = new Classes(intProfile, dsn);
            oEnvironment = new Environments(intProfile, dsn);
            oResiliency = new Resiliency(intProfile, dsn);
            if (oUser.IsAdmin(intProfile) == true || (oDataPoint.GetPagePermission(intApplication, "ASSET") == true || intDataPointAvailableAsset == 1))
            {
                panAllow.Visible = true;
                if (Request.QueryString["save"] != null)
                    panSave.Visible = true;
                if (Request.QueryString["error"] != null)
                {
                    panError.Visible = true;
                    lblError.Text = "Name Already Exists";
                }
                Int32.TryParse(oFunction.decryptQueryString(Request.QueryString["id"]), out intID);
                if (Request.QueryString["close"] != null)
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "close", "<script type=\"text/javascript\">window.close();<" + "/" + "script>");
                else if (intID > 0)
                {
                    DataSet ds = oDataPoint.GetAsset(intID);
                    if (ds.Tables[0].Rows.Count == 1)
                    {
                        // Load General Information
                        intAsset = Int32.Parse(ds.Tables[0].Rows[0]["id"].ToString());
                        lblAssetID.Text = "#" + intAsset.ToString();
                        string strSerial = ds.Tables[0].Rows[0]["serial"].ToString();
                        string strAsset = ds.Tables[0].Rows[0]["asset"].ToString();

                        string strHeader = (strSerial.Length > 15 ? strSerial.Substring(0, 15) + "..." : strSerial);
                        lblHeader.Text = "&quot;" + strHeader.ToUpper() + "&quot;";
                        Master.Page.Title = "DataPoint | Enclosure (" + strHeader + ")";
                        lblHeaderSub.Text = "Provides all the information about an enclosure...";
                        int intMenuTab = 0;
                        if (Request.QueryString["menu_tab"] != null && Request.QueryString["menu_tab"] != "")
                            intMenuTab = Int32.Parse(Request.QueryString["menu_tab"]);
                        Tab oTab = new Tab(hdnTab.ClientID, intMenuTab, "divMenu1", true, false);
                        oTab.AddTab("Asset Information", "");
                        oTab.AddTab("Location Information", "");
                        oTab.AddTab("Virtual Connect IPs", "");
                        oTab.AddTab("Resource Dependencies", "");
                        oTab.AddTab("Provisioning History", "");
                        oTab.AddTab("Blades", "");
                        //oTab.AddTab("Network Adapter Configuration", "");
                        strMenuTab1 = oTab.GetTabs();
                        if (oUser.IsAdmin(intProfile) == true || oDataPoint.GetFieldPermission(intProfile, "SERVER_ADMIN") == true)
                            panOldLocationInfo.Visible = true;

                        if (!IsPostBack)
                        {
                            //DataSet dsAssets = oServer.GetAsset(intAsset);
                            //foreach (DataRow drAsset in dsAssets.Tables[0].Rows)
                            //{
                            //    if (drAsset["latest"].ToString() == "1")
                            //    {
                            //        intAsset = Int32.Parse(drAsset["assetid"].ToString());
                            //        intAssetClass = Int32.Parse(drAsset["classid"].ToString());
                            //        intAssetEnv = Int32.Parse(drAsset["environmentid"].ToString());
                            //        break;
                            //    }
                            //}

                            // Asset Information
                            oDataPoint.LoadTextBox(txtPlatformSerial, intProfile, null, "", lblPlatformSerial, fldPlatformSerial, "ENCLOSURE_SERIAL", strSerial, "", false, true);
                            oDataPoint.LoadTextBox(txtPlatformAsset, intProfile, null, "", lblPlatformAsset, fldPlatformAsset, "ENCLOSURE_ASSET", strAsset, "", false, true);
                            
                            int intAssetAttribute = Int32.Parse(oAsset.Get(intAsset, "asset_attribute"));
                            oDataPoint.LoadDropDown(ddlAssetAttribute, intProfile, null, "", lblAssetAttribute, fldAssetAttribute, "ASSET_ATTRIBUTE", "Name", "AttributeId", oAsset.getAssetAttributes(null, "", 1), intAssetAttribute, true, false, false);
                            oDataPoint.LoadTextBox(txtAssetAttributeComment, intProfile, null, "", lblAssetAttributeComment, fldAssetAttributeComment, "ASSET_ATTRIBUTE_COMMENT", oAsset.getAssetAttributesComments(intAsset), "", false, true);
                            ddlAssetAttribute.Attributes.Add("onclick", "return SetControlsForAssetAttributes()");

                            ddlPlatform.Attributes.Add("onchange", "PopulatePlatformTypes('" + ddlPlatform.ClientID + "','" + ddlPlatformType.ClientID + "','" + ddlPlatformModel.ClientID + "','" + ddlPlatformModelProperty.ClientID + "');ResetDropDownHidden('" + hdnModel.ClientID + "');");
                            ddlPlatformType.Attributes.Add("onchange", "PopulatePlatformModels('" + ddlPlatformType.ClientID + "','" + ddlPlatformModel.ClientID + "','" + ddlPlatformModelProperty.ClientID + "');ResetDropDownHidden('" + hdnModel.ClientID + "');");
                            ddlPlatformModel.Attributes.Add("onchange", "PopulatePlatformModelProperties('" + ddlPlatformModel.ClientID + "','" + ddlPlatformModelProperty.ClientID + "');ResetDropDownHidden('" + hdnModel.ClientID + "');");
                            ddlPlatformModelProperty.Attributes.Add("onchange", "UpdateDropDownHidden('" + ddlPlatformModelProperty.ClientID + "','" + hdnModel.ClientID + "');");
                            int intModel = Int32.Parse(oAsset.Get(intAsset, "modelid"));

                            hdnModel.Value = intModel.ToString();
                            int intModelParent = Int32.Parse(oModelsProperties.Get(intModel, "modelid"));
                            int intType = oModel.GetType(intModelParent);
                            int intPlatform = oType.GetPlatform(intType);
                            oDataPoint.LoadDropDown(ddlPlatform, intProfile, null, "", lblPlatform, fldPlatform, "ENCLOSURE_PLATFORM", "name", "platformid", oPlatform.Gets(1), intPlatform, false, false, true);
                            oDataPoint.LoadDropDown(ddlPlatformType, intProfile, null, "", lblPlatformType, fldPlatformType, "ENCLOSURE_TYPE", "name", "id", oType.Gets(intPlatform, 1), intType, false, false, true);
                            oDataPoint.LoadDropDown(ddlPlatformModel, intProfile, null, "", lblPlatformModel, fldPlatformModel, "ENCLOSURE_MODEL", "name", "id", oModel.Gets(intType, 1), intModelParent, false, false, true);
                            oDataPoint.LoadDropDown(ddlPlatformModelProperty, intProfile, null, "", lblPlatformModelProperty, fldPlatformModelProperty, "ENCLOSURE_MODEL_PROP", "name", "id", oModelsProperties.GetModels(0, intModelParent, 1), intModel, false, false, true);

                            // Get Asset
                            DataSet dsAsset = oAsset.GetEnclosure(intAsset);
                            if (dsAsset.Tables[0].Rows.Count > 0)
                            {
                                oDataPoint.LoadTextBox(txtName, intProfile, null, "", lblName, fldName, "ENCLOSURE_NAME", dsAsset.Tables[0].Rows[0]["name"].ToString(), "", false, false);
                                if (txtName.Text != "")
                                    lblHeader.Text += "&nbsp;&nbsp;&nbsp;[" + txtName.Text + "]";
                                lblStatus.Text = dsAsset.Tables[0].Rows[0]["statusname"].ToString();
                                int intClass = Int32.Parse(dsAsset.Tables[0].Rows[0]["classid"].ToString());
                                int intEnv = Int32.Parse(dsAsset.Tables[0].Rows[0]["environmentid"].ToString());
                                hdnEnvironment.Value = intEnv.ToString();
                             
                                oDataPoint.LoadDropDown(ddlPlatformClass, intProfile, null, "", lblPlatformClass, fldPlatformClass, "ENCLOSURE_CLASS", "name", "id", oClass.Gets(1), intClass, false, false, true);
                                oDataPoint.LoadDropDown(ddlPlatformEnvironment, intProfile, null, "", lblPlatformEnvironment, fldPlatformEnvironment, "ENCLOSURE_ENVIRONMENT", "name", "id", oClass.GetEnvironment(intClass, 0), intEnv, false, false, true);
                                oDataPoint.LoadTextBox(txtPlatformVLAN, intProfile, null, "", lblPlatformVLAN, fldPlatformVLAN, "ENCLOSURE_VLAN", dsAsset.Tables[0].Rows[0]["vlan"].ToString(), "", false, true);
                                if (dsAsset.Tables[0].Rows[0]["oa_ip"].ToString() != "")
                                    oDataPoint.LoadTextBox(txtPlatformOA, intProfile, btnOA, "https://" + dsAsset.Tables[0].Rows[0]["oa_ip"].ToString(), lblPlatformOA, fldPlatformOA, "ENCLOSURE_OA", dsAsset.Tables[0].Rows[0]["oa_ip"].ToString(), "", false, true);
                                else
                                    oDataPoint.LoadTextBox(txtPlatformOA, intProfile, null, "", lblPlatformOA, fldPlatformOA, "ENCLOSURE_OA", dsAsset.Tables[0].Rows[0]["oa_ip"].ToString(), "", false, true);
                                int intResiliency = 0;
                                Int32.TryParse(dsAsset.Tables[0].Rows[0]["resiliencyid"].ToString(), out intResiliency);
                                oDataPoint.LoadDropDown(ddlPlatformResiliency, intProfile, null, "", lblPlatformResiliency, fldPlatformResiliency, "ENCLOSURE_RESILIENCY", "name", "id", oResiliency.Gets(1), intResiliency, false, false, true);

                                lblOldlocation.Text = dsAsset.Tables[0].Rows[0]["OldLocation"].ToString();
                                lblOldRoom.Text = dsAsset.Tables[0].Rows[0]["OldRoom"].ToString();
                                lblOldRack.Text = dsAsset.Tables[0].Rows[0]["OldRack"].ToString();

                                txtLocation.Text = dsAsset.Tables[0].Rows[0]["Location"].ToString();
                                txtRoom.Text = dsAsset.Tables[0].Rows[0]["Room"].ToString();
                                txtZone.Text = dsAsset.Tables[0].Rows[0]["Zone"].ToString();
                                txtRack.Text = dsAsset.Tables[0].Rows[0]["Rack"].ToString();
                                txtRackPosition.Text = dsAsset.Tables[0].Rows[0]["Rackposition"].ToString();
                                oDataPoint.LoadTextBox(txtRackPosition, intProfile, null, "", lblRackPositionValue, fldLocation, "CHANGE_LOCATION", dsAsset.Tables[0].Rows[0]["rackposition"].ToString(), "", false, true);
                                hdnRackId.Value = dsAsset.Tables[0].Rows[0]["NewRackId"].ToString();

                                oDataPoint.LoadButton(btnSelectLocation, intProfile, fldLocation, "CHANGE_LOCATION",
                                                     "return LoadLocationRoomRack('" + "rack" + "','" + hdnRackId.ClientID + "', '" + txtLocation.ClientID + "','" + txtRoom.ClientID + "','" + txtZone.ClientID + "','" + txtRack.ClientID + "');");


                                //DR Counterpart
                                int intDR = oAsset.GetDR(intAsset);
                                string strDR = oAsset.Get(intDR, "serial");
                                if (intDR > 0)
                                    oDataPoint.LoadTextBox(txtPlatformDRCounterPart, intProfile, btnPlatformDRCounterPart, "/datapoint/asset/datapoint_asset_search.aspx?t=serial&q=" + oFunction.encryptQueryString(strDR) + "&id=" + oFunction.encryptQueryString(intDR.ToString()), lblPlatformDRCounterPart, fldPlatformDRCounterPart, "BLADE_DR", strDR, "", true, false);
                                else
                                    oDataPoint.LoadTextBox(txtPlatformDRCounterPart, intProfile, null, "", lblPlatformDRCounterPart, fldPlatformDRCounterPart, "BLADE_DR", "", "", true, false);

                                ddlStatus.SelectedValue = dsAsset.Tables[0].Rows[0]["status"].ToString();
                                ddlStatus.Enabled = (intAssetAttribute == (int)AssetAttribute.Ok);
                                panStatus.Visible = (ddlStatus.Enabled == false);
                            }
                            else
                                Response.Redirect("/datapoint/asset/enclosure_deploy.aspx?t=serial&q=" + oFunction.encryptQueryString(strSerial) + "&id=" + oFunction.encryptQueryString(intAsset.ToString()) + "&r=0");

                            // Resource Dependencies
                            AssetOrder oAssetOrder = new AssetOrder(0, dsn, dsnAsset, intEnvironment);
                            Services oService = new Services(0, dsn);
                            ServiceRequests oServiceRequest = new ServiceRequests(0, dsn);
                            rptServiceRequests.DataSource = oAssetOrder.GetByAsset(intAsset, false);
                            rptServiceRequests.DataBind();
                            trServiceRequests.Visible = (rptServiceRequests.Items.Count == 0);
                            foreach (RepeaterItem ri in rptServiceRequests.Items)
                            {
                                Label lblServiceID = (Label)ri.FindControl("lblServiceID");
                                int intService = Int32.Parse(lblServiceID.Text);
                                Label lblDetails = (Label)ri.FindControl("lblDetails");
                                Label lblProgress = (Label)ri.FindControl("lblProgress");

                                if (lblProgress.Text == "")
                                    lblProgress.Text = "<i>Unavailable</i>";
                                else
                                {
                                    int intResource = Int32.Parse(lblProgress.Text);
                                    double dblAllocated = 0.00;
                                    double dblUsed = 0.00;
                                    int intStatus = 0;
                                    bool boolAssigned = false;
                                    DataSet dsResource = oDataPoint.GetServiceRequestResource(intResource);
                                    if (dsResource.Tables[0].Rows.Count > 0)
                                        Int32.TryParse(dsResource.Tables[0].Rows[0]["status"].ToString(), out intStatus);
                                    foreach (DataRow drResource in dsResource.Tables[1].Rows)
                                    {
                                        boolAssigned = true;
                                        dblAllocated += double.Parse(drResource["allocated"].ToString());
                                        dblUsed += double.Parse(drResource["used"].ToString());
                                        intStatus = Int32.Parse(drResource["status"].ToString());
                                    }
                                    if (intStatus == (int)ResourceRequestStatus.Closed)
                                        lblProgress.Text = oServiceRequest.GetStatusBar(100.00, "100", "12", true);
                                    else if (intStatus == (int)ResourceRequestStatus.Cancelled)
                                        lblProgress.Text = "Cancelled";
                                    else if (boolAssigned == false)
                                    {
                                        string strManager = "";
                                        DataSet dsManager = oService.GetUser(intService, 1);  // Managers
                                        foreach (DataRow drManager in dsManager.Tables[0].Rows)
                                        {
                                            if (strManager != "")
                                                strManager += "\\n";
                                            int intManager = Int32.Parse(drManager["userid"].ToString());
                                            strManager += " - " + oUser.GetFullName(intManager) + " [" + oUser.GetName(intManager) + "]";
                                        }
                                        lblProgress.Text = "<a href=\"javascript:void(0);\" class=\"lookup\" onclick=\"alert('This request is pending assignment by the following...\\n\\n" + strManager + "');\">Pending Assignment</a>";
                                    }
                                    else if (dblAllocated > 0.00)
                                        lblProgress.Text = oServiceRequest.GetStatusBar((dblUsed / dblAllocated) * 100.00, "100", "12", true);
                                    else
                                        lblProgress.Text = "<i>N / A</i>";
                                    lblDetails.Text = "<a href=\"javascript:void(0);\" class=\"lookup\" onclick=\"OpenNewWindowMenu('/datapoint/service/resource.aspx?id=" + oFunction.encryptQueryString(intResource.ToString()) + "', '800', '600');\">" + lblDetails.Text + "</a>";
                                }
                            }

                            // Provioning History
                            rptHistory.DataSource = oAsset.GetProvisioningHistory(oDataPoint.AssetHistorySelect(intAsset));
                            rptHistory.DataBind();
                            lblHistory.Visible = (rptHistory.Items.Count == 0);
                            oDataPoint.LoadPanel(panProvisioning, intProfile, fldProvisioning, "ENCLOSURE_STATUS");

                            // Virtual Connects
                            rptVirtualConnect.DataSource = oAsset.GetEnclosureVCs(intAsset, 1);
                            rptVirtualConnect.DataBind();
                            lblVirtualConnect.Visible = (rptVirtualConnect.Items.Count == 0);
                            oDataPoint.LoadButton(btnVC, intProfile, fldVC, "ENCLOSURE_VC", "return OpenWindow('ASSET_DEPLOY_VCs','" + intAsset.ToString() + "');");

                            if (Request.QueryString["highlight"] != null && Request.QueryString["highlight"] != "")
                                intHighlight = Int32.Parse(Request.QueryString["highlight"]);
                            // Blades
                            rptBlades.DataSource = oAsset.GetEnclosureBlades(intAsset);
                            rptBlades.DataBind();
                            lblBlades.Visible = (rptBlades.Items.Count == 0);

                            // Redeploy
                            oDataPoint.LoadButton(btnRedeploy, intProfile, fldRedeploy, "ENCLOSURE_REDEPLOY");
                            if (rptBlades.Items.Count == 0)
                                btnRedeploy.Attributes.Add("onclick", "return confirm('Are you sure you want to redeploy this enclosure?') && confirm('WARNING! Redeploying an asset means that ALL the information regarding this asset will be erased.\\n\\nAre you sure you want to redeploy this enclosure?');");
                            else
                                btnRedeploy.Attributes.Add("onclick", "return confirm('Are you sure you want to redeploy this enclosure?\\n\\WARNING: The blades associated with this enclosure have not been redployed!\\nIt is STRONGLY recommended that you redeploy the blades first to avoid corrupting the asset database.\\n\\nAre you sure you want to continue?');");
                            oDataPoint.LoadButton(btnRedeployBlades, intProfile, fldRedeployBlades, "ENCLOSURE_REDEPLOY_BLADES");
                            if (rptBlades.Items.Count == 0)
                                btnRedeployBlades.Attributes.Add("onclick", "alert('All blades have already been redeployed');return false;;");
                            else
                                btnRedeployBlades.Attributes.Add("onclick", "return confirm('Are you sure you want to redeploy ALL the blades in this enclosure?') && confirm('WARNING! Redeploying an asset means that ALL the information regarding this asset will be erased.\\n\\nAre you sure you want to redeploy ALL the blades in this enclosure?');");
                        }
                    }
                    else
                    {
                        if (Request.QueryString["t"] != null && Request.QueryString["q"] != null)
                            Response.Redirect("/datapoint/asset/datapoint_asset_search.aspx?t=" + Request.QueryString["t"] + "&q=" + Request.QueryString["q"] + "&r=0");
                        else
                            Response.Redirect("/datapoint/asset/datapoint_asset_search.aspx");
                    }
                }
                else if (Request.QueryString["q"] != null && Request.QueryString["q"] != "")
                {
                    string strQuery = oFunction.decryptQueryString(Request.QueryString["q"]);
                    DataSet ds = oDataPoint.GetAssetName(strQuery, intID, 0, "", "", 0);
                    if (ds.Tables[0].Rows.Count == 1)
                    {
                        intAsset = Int32.Parse(ds.Tables[0].Rows[0]["assetid"].ToString());
                        Response.Redirect(Request.Path + "?t=" + Request.QueryString["t"] + "&q=" + Request.QueryString["q"] + "&id=" + oFunction.encryptQueryString(intAsset.ToString()));
                    }
                    else
                        Response.Redirect("/datapoint/asset/datapoint_asset_search.aspx?multiple=true&t=" + Request.QueryString["t"] + "&q=" + Request.QueryString["q"]);
                }
                else
                    Response.Redirect("/datapoint/asset/datapoint_asset_search.aspx");
                btnClose.Attributes.Add("onclick", "window.close();return false;");
                btnPrint.Attributes.Add("onclick", "window.print();return false;");
                btnSave.Attributes.Add("onclick", oDataPoint.LoadValidation());
                btnSaveClose.Attributes.Add("onclick", oDataPoint.LoadValidation());
                ddlPlatformClass.Attributes.Add("onchange", "PopulateEnvironments('" + ddlPlatformClass.ClientID + "','" + ddlPlatformEnvironment.ClientID + "',0);");
                ddlPlatformEnvironment.Attributes.Add("onchange", "UpdateDropDownHidden('" + ddlPlatformEnvironment.ClientID + "','" + hdnEnvironment.ClientID + "');");
                //ddlLocationState.Attributes.Add("onchange", "LoadLocationCity('" + ddlLocationCity.ClientID + "');LoadLocationAddress('" + ddlLocationAddress.ClientID + "');PopulateCitys('" + ddlLocationState.ClientID + "','" + ddlLocationCity.ClientID + "');ResetDropDownHidden('" + hdnAddress.ClientID + "');");
                //ddlLocationCity.Attributes.Add("onchange", "LoadLocationAddress('" + ddlLocationAddress.ClientID + "');PopulateAddresss('" + ddlLocationCity.ClientID + "','" + ddlLocationAddress.ClientID + "');ResetDropDownHidden('" + hdnAddress.ClientID + "');");
                //ddlLocationAddress.Attributes.Add("onchange", "UpdateDropDownHidden('" + ddlLocationAddress.ClientID + "','" + hdnAddress.ClientID + "');");
            }
            else
                panDenied.Visible = true;
        }
        protected void btnNew_Click(Object Sender, EventArgs e)
        {
            Response.Redirect("/datapoint/asset/datapoint_asset_search.aspx");
        }
        protected void btnSave_Click(Object Sender, EventArgs e)
        {
            int intSave = Save();
            Response.Redirect(Request.Path + "?t=" + Request.QueryString["t"] + "&q=" + Request.QueryString["q"] + "&id=" + oFunction.encryptQueryString(intAsset.ToString()) + "&" + (intSave > 0 ? "save" : "error") + "=" + intSave.ToString());
        }
        protected void btnSaveClose_Click(Object Sender, EventArgs e)
        {
            int intSave = Save();
            Response.Redirect(Request.Path + "?t=" + Request.QueryString["t"] + "&q=" + Request.QueryString["q"] + "&id=" + oFunction.encryptQueryString(intAsset.ToString()) + "&" + (intSave > 0 ? "close" : "error") + "=" + intSave.ToString());
        }
        protected int Save()
        {
            string strOldName = oAsset.GetStatus(intAsset, "name");
            oAsset.Update(intAsset, Int32.Parse(Request.Form[hdnModel.UniqueID]), txtPlatformSerial.Text, txtPlatformAsset.Text, Int32.Parse(ddlAssetAttribute.SelectedValue));
            oAsset.addAssetAttributesForAsset(intAsset, Int32.Parse(ddlAssetAttribute.SelectedValue), txtAssetAttributeComment.Text.Trim(), intProfile);

            DataSet dsEnclosure = oAsset.GetEnclosures((int)AssetStatus.InUse);
            bool boolFound = false;
            foreach (DataRow drEnclosure in dsEnclosure.Tables[0].Rows)
            {
                if (drEnclosure["name"].ToString().Trim().ToUpper() == txtName.Text.Trim().ToUpper() && drEnclosure["name"].ToString().Trim().ToUpper() != strOldName.Trim().ToUpper())
                {
                    boolFound = true;
                    break;
                }
            }
            int intReturn = 1;
            if (boolFound == false)
                oAsset.UpdateEnclosure(intAsset, txtName.Text, Int32.Parse(ddlStatus.SelectedItem.Value), intProfile, DateTime.Now, Int32.Parse(ddlPlatformClass.SelectedItem.Value), Int32.Parse(Request.Form[hdnEnvironment.UniqueID]), Int32.Parse(hdnRackId.Value), txtRackPosition.Text, Int32.Parse(txtPlatformVLAN.Text), txtPlatformOA.Text, Int32.Parse(ddlPlatformResiliency.SelectedItem.Value));
            else
                intReturn = -100;
            return intReturn;
        }
        protected void btnRedeploy_Click(Object Sender, EventArgs e)
        {
            oAsset.DeleteEnclosure(intAsset, true, intProfile);
            Response.Redirect("/datapoint/asset/datapoint_asset_search.aspx?t=serial&q=" + oFunction.encryptQueryString(oAsset.Get(intAsset, "serial")) + "&r=0");
        }
        protected void btnRedeployBlades_Click(Object Sender, EventArgs e)
        {
            oAsset.DeleteEnclosureBlades(intAsset, true, intProfile);
            Response.Redirect("/datapoint/asset/datapoint_asset_search.aspx?t=serial&q=" + oFunction.encryptQueryString(oAsset.Get(intAsset, "serial")) + "&r=0");
        }
    }
}
