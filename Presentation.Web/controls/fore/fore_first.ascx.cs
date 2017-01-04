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
    public partial class fore_first : System.Web.UI.UserControl
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);

        protected int intLocation = Int32.Parse(ConfigurationManager.AppSettings["OPSLocationID"]);
        protected int intForceOverride = Int32.Parse(ConfigurationManager.AppSettings["ForceForecastOverride"]);
        protected int intProfile;
        protected Forecast oForecast;
        protected Platforms oPlatform;
        protected Locations oLocation;
        protected Classes oClass;
        protected Environments oEnvironment;
        protected ServerName oServerName;
        protected DataPoint oDataPoint;
        protected int intForecast;
        protected int intID = 0;
        protected int intPlatform;
        protected string strLocation = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oForecast = new Forecast(intProfile, dsn);
            oPlatform = new Platforms(intProfile, dsn);
            oLocation = new Locations(intProfile, dsn);
            oClass = new Classes(intProfile, dsn);
            oEnvironment = new Environments(intProfile, dsn);
            oServerName = new ServerName(intProfile, dsn);
            oDataPoint = new DataPoint(intProfile, dsn);
            if (Request.QueryString["parent"] != null && Request.QueryString["parent"] != "")
                intForecast = Int32.Parse(Request.QueryString["parent"]);
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                intID = Int32.Parse(Request.QueryString["id"]);
            if (Request.QueryString["step"] != null && Request.QueryString["step"] != "")
                panUpdate.Visible = true;
            else
                panNavigation.Visible = true;
            if (!IsPostBack)
                LoadLists();
            int intAddress = 0;
            int intRecovery = 0;
            bool boolRecoveryMany = false;
            if (intID > 0)
            {
                DataSet ds = oForecast.GetAnswer(intID);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    bool boolHundred = false;
                    int intConfidence = Int32.Parse(ds.Tables[0].Rows[0]["confidenceid"].ToString());
                    if (intConfidence > 0)
                    {
                        Confidence oConfidence = new Confidence(intProfile, dsn);
                        string strConfidence = oConfidence.Get(intConfidence, "name");
                        if (strConfidence.Contains("100%") == true)
                            boolHundred = true;
                    }
                    if (boolHundred == true)
                    {
                        panUpdate.Visible = false;
                        panNavigation.Visible = false;
                        btnHundred.Visible = true;
                    }
                    intPlatform = Int32.Parse(ds.Tables[0].Rows[0]["platformid"].ToString());
                    int intStep = Int32.Parse(ds.Tables[0].Rows[0]["step"].ToString());
                    intForecast = Int32.Parse(ds.Tables[0].Rows[0]["forecastid"].ToString());
                    DataSet dsSteps = oForecast.GetSteps(intPlatform, 1);
                    if (dsSteps.Tables[0].Rows.Count == intStep)
                        btnNext.Text = "Finish";
                    if (intStep == 0 || intStep == 1)
                        btnBack.Enabled = false;
                    boolRecoveryMany = (oForecast.IsDRUnder48(intID, false) && oForecast.IsDRManyToOne(intID));
                    if (!IsPostBack)
                    {
                        lblPlatform.Text = oPlatform.GetName(intPlatform);
                        lblPlatform.Visible = true;
                        int intClass = Int32.Parse(ds.Tables[0].Rows[0]["classid"].ToString());
                        if (ds.Tables[0].Rows[0]["override"].ToString() == "1" || ds.Tables[0].Rows[0]["override"].ToString() == "-1")
                        {
                            radYes.Checked = true;
                            divOverride.Style["display"] = "inline";
                            if (ds.Tables[0].Rows[0]["breakfix"].ToString() == "1")
                            {
                                radBreakYes.Checked = true;
                                divChange.Style["display"] = "inline";
                                txtChange.Text = ds.Tables[0].Rows[0]["change"].ToString();
                                divName.Style["display"] = "inline";
                                int intName = Int32.Parse(ds.Tables[0].Rows[0]["nameid"].ToString());
                                if (oClass.Get(intClass, "pnc") == "1")
                                    txtDeviceName.Text = oServerName.GetNameFactory(intName, 0);
                                else
                                    txtDeviceName.Text = oServerName.GetName(intName, 0);
                                txtQuantity.ReadOnly = true;
                            }
                            else
                                radBreakNo.Checked = true;
                        }
                        else
                            radNo.Checked = true;
                        txtName.Text = ds.Tables[0].Rows[0]["name"].ToString();
                        try
                        {
                            ddlClass.SelectedValue = intClass.ToString();
                        }
                        catch { }
                        if (ddlClass.SelectedIndex == 0)
                        {
                            // Class is not available, add it.
                            ddlClass.Items.Add(new ListItem(oClass.Get(intClass, "name") + " *", intClass.ToString()));
                            ddlClass.SelectedValue = intClass.ToString();
                        }
                        if (oClass.IsProd(intClass) && oClass.Get(intClass, "pnc") != "1")
                        {
                            divTest.Style["display"] = "inline";
                            chkTest.Checked = (ds.Tables[0].Rows[0]["test"].ToString() == "1");
                        }
                        int intEnv = Int32.Parse(ds.Tables[0].Rows[0]["environmentid"].ToString());
                        hdnEnvironment.Value = intEnv.ToString();
                        ddlEnvironment.SelectedValue = intEnv.ToString();
                        ddlEnvironment.Enabled = true;
                        ddlEnvironment.DataTextField = "name";
                        ddlEnvironment.DataValueField = "id";
                        ddlEnvironment.DataSource = oClass.GetEnvironment(intClass, 1);
                        ddlEnvironment.DataBind();
                        ddlEnvironment.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                        if (lblPlatform.Text.ToUpper() == "SERVER" && ddlClass.SelectedItem.Text.ToUpper() == "PRODUCTION")
                            ddlMaintenance.Enabled = true;
                        if (lblPlatform.Text.ToUpper() == "SERVER" && ddlClass.SelectedItem.Text.ToUpper() == "TEST")
                            ddlMaintenance.Enabled = true;
                        if (lblPlatform.Text.ToUpper() == "SERVER")
                            divServerType.Style["display"] = "inline";
                        intAddress = Int32.Parse(ds.Tables[0].Rows[0]["addressid"].ToString());
                        ddlMaintenance.SelectedValue = ds.Tables[0].Rows[0]["maintenanceid"].ToString();
                        int intApplicationID = Int32.Parse(ds.Tables[0].Rows[0]["applicationid"].ToString());
                        ddlApplications.SelectedValue = intApplicationID.ToString();
                        DataSet dsSubApplications = oServerName.GetSubApplications(intApplicationID, 1);
                        if (dsSubApplications.Tables[0].Rows.Count > 0)
                        {
                            int intSubApplicationID = Int32.Parse(ds.Tables[0].Rows[0]["subapplicationid"].ToString());
                            ddlSubApplications.DataTextField = "name";
                            ddlSubApplications.DataValueField = "id";
                            ddlSubApplications.DataSource = dsSubApplications;
                            ddlSubApplications.DataBind();
                            ddlSubApplications.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                            divSubApplications.Style["display"] = "inline";
                            ddlSubApplications.SelectedValue = intSubApplicationID.ToString();
                            hdnSubApplication.Value = intSubApplicationID.ToString();
                        }
                        txtQuantity.Text = ds.Tables[0].Rows[0]["quantity"].ToString();
                        radResiliencyYes.Checked = (ds.Tables[0].Rows[0]["resiliency"].ToString() == "1");
                        radResiliencyNo.Checked = (radResiliencyYes.Checked == false);
                        if (txtQuantity.Text == "0")
                            txtQuantity.Text = "";
                        intRecovery = Int32.Parse(ds.Tables[0].Rows[0]["recovery_number"].ToString());
                    }
                }
                else
                {
                    btnBack.Enabled = false;
                    ddlPlatform.Visible = true;
                    intAddress = intLocation;
                    radNo.Checked = true;
                }
            }
            else
            {
                btnBack.Enabled = false;
                ddlPlatform.Visible = true;
                intAddress = intLocation;
                radNo.Checked = true;
            }
            if (intAddress > 0)
                intLocation = intAddress;
            strLocation = oLocation.LoadDDL("ddlState", "ddlCity", "ddlAddress", hdnLocation.ClientID, intLocation, true, "ddlCommon");
            hdnLocation.Value = intLocation.ToString();
            ddlClass.Attributes.Add("onchange", "PopulateEnvironments('" + ddlClass.ClientID + "','" + ddlEnvironment.ClientID + "',1);EnableMaintenance();ShowTestOption('" + divTest.ClientID + "');");
            ddlPlatform.Attributes.Add("onchange", "ShowServerType('" + divServerType.ClientID + "');");
            ddlApplications.Attributes.Add("onchange", "PopulateSubApplications('" + ddlApplications.ClientID + "','" + ddlSubApplications.ClientID + "','" + divSubApplications.ClientID + "');");
            ddlEnvironment.Attributes.Add("onchange", "UpdateDropDownHidden('" + ddlEnvironment.ClientID + "','" + hdnEnvironment.ClientID + "');");
            ddlSubApplications.Attributes.Add("onchange", "UpdateDropDownHidden('" + ddlSubApplications.ClientID + "','" + hdnSubApplication.ClientID + "');");
            btnNext.Attributes.Add("onclick", "return EnsureOverride('" + radYes.ClientID + "','" + radNo.ClientID + "','" + radBreakYes.ClientID + "','" + radBreakNo.ClientID + "','" + txtChange.ClientID + "')" +
                " && ValidateDropDown('" + ddlPlatform.ClientID + "','Please select a platform')" +
                " && ValidateHidden0('" + hdnLocation.ClientID + "','ddlState','Please select a location')" +
                " && ValidateDropDown('" + ddlClass.ClientID + "','Please select a class')" +
                " && ValidateDropDown('" + ddlEnvironment.ClientID + "','Please select an environment')" +
                " && EnsureMaintenance('" + ddlMaintenance.ClientID + "','Please select a maintenance window')" +
                " && ValidateNumber0('" + txtQuantity.ClientID + "','Please enter a valid quantity')" +
                " && ValidateHiddenDisabled('" + hdnSubApplication.ClientID + "','" + ddlSubApplications.ClientID + "','Please select a value')" +
                " && ForeRecoveryFirst(" + (boolRecoveryMany ? "true" : "false") + ",'" + txtQuantity.ClientID + "'," + intRecovery.ToString() + ")" +
                " && ValidateTextDisabled('" + txtDeviceName.ClientID + "', 'Please enter a valid device name')" +
                " && EnsureDeviceName('" + radYes.ClientID + "','" + radBreakYes.ClientID + "','" + txtDeviceName.ClientID + "','" + ddlClass.ClientID + "')" +
                ";");
            btnUpdate.Attributes.Add("onclick", "return EnsureOverride('" + radYes.ClientID + "','" + radNo.ClientID + "','" + radBreakYes.ClientID + "','" + radBreakNo.ClientID + "','" + txtChange.ClientID + "')" +
                " && ValidateDropDown('" + ddlPlatform.ClientID + "','Please select a platform')" +
                " && ValidateHidden0('" + hdnLocation.ClientID + "','ddlState','Please select a location')" +
                " && ValidateDropDown('" + ddlClass.ClientID + "','Please select a class')" +
                " && ValidateDropDown('" + ddlEnvironment.ClientID + "','Please select an environment')" +
                " && EnsureMaintenance('" + ddlMaintenance.ClientID + "','Please select a maintenance window')" +
                " && ValidateNumber0('" + txtQuantity.ClientID + "','Please enter a valid quantity')" +
                " && ValidateHiddenDisabled('" + hdnSubApplication.ClientID + "','" + ddlSubApplications.ClientID + "','Please select a value')" +
                " && ForeRecoveryFirst(" + (boolRecoveryMany ? "true" : "false") + ",'" + txtQuantity.ClientID + "'," + intRecovery.ToString() + ")" +
                " && ValidateTextDisabled('" + txtDeviceName.ClientID + "', 'Please enter a valid device name')" +
                " && EnsureDeviceName('" + radYes.ClientID + "','" + radBreakYes.ClientID + "','" + txtDeviceName.ClientID + "','" + ddlClass.ClientID + "')" +
                ";");
            radYes.Attributes.Add("onclick", "ShowHideDiv('" + divOverride.ClientID + "','inline');ShowHideDivCheck('" + divChange.ClientID + "',document.getElementById('" + radBreakYes.ClientID + "'));ShowHideDivCheck('" + divName.ClientID + "',document.getElementById('" + radBreakYes.ClientID + "'));LockQuantityRadio('" + radYes.ClientID + "','" + radBreakYes.ClientID + "','" + txtQuantity.ClientID + "');");
            radNo.Attributes.Add("onclick", "ShowHideDiv('" + divOverride.ClientID + "','none');ShowHideDiv('" + divChange.ClientID + "','none');ShowHideDiv('" + divName.ClientID + "','none');LockQuantity('" + txtQuantity.ClientID + "',false);");
            radBreakYes.Attributes.Add("onclick", "ShowHideDiv('" + divChange.ClientID + "','inline');ShowHideDiv('" + divName.ClientID + "','inline');LockQuantity('" + txtQuantity.ClientID + "',true);");
            radBreakNo.Attributes.Add("onclick", "ShowHideDiv('" + divChange.ClientID + "','none');ShowHideDiv('" + divName.ClientID + "','none');LockQuantity('" + txtQuantity.ClientID + "',false);");
            btnClose.Attributes.Add("onclick", "return window.close();");
        }
        private void LoadLists()
        {
            ddlPlatform.DataTextField = "name";
            ddlPlatform.DataValueField = "platformid";
            ddlPlatform.DataSource = oPlatform.GetForecasts(1);
            ddlPlatform.DataBind();
            ddlPlatform.Items.Insert(0, new ListItem("-- SELECT --", "0"));
            ddlPlatform.Attributes.Add("onchange", "EnableMaintenance();");
            ddlClass.DataTextField = "name";
            ddlClass.DataValueField = "id";
            ddlClass.DataSource = oClass.GetForecasts(1);
            ddlClass.DataBind();
            ddlClass.Items.Insert(0, new ListItem("-- SELECT --", "0"));
            MaintenanceWindow oMaintenanceWindow = new MaintenanceWindow(intProfile, dsn);
            ddlMaintenance.DataTextField = "name";
            ddlMaintenance.DataValueField = "id";
            ddlMaintenance.DataSource = oMaintenanceWindow.Gets(1);
            ddlMaintenance.DataBind();
            ddlMaintenance.Items.Insert(0, new ListItem("-- SELECT --", "0"));
            ServerName oServerName = new ServerName(intProfile, dsn);
            ddlApplications.DataTextField = "name";
            ddlApplications.DataValueField = "id";
            ddlApplications.DataSource = oServerName.GetApplicationsForecast(1);
            ddlApplications.DataBind();
            ddlApplications.Items.Insert(0, new ListItem("-- NONE --", "0"));
        }
        private bool Save()
        {
            if (intID == 0)
            {
                intPlatform = Int32.Parse(ddlPlatform.SelectedItem.Value);
                intID = oForecast.AddAnswer(Int32.Parse(Request.QueryString["parent"]), intPlatform, 0, intProfile);
            }
            bool boolOverride = false;
            bool boolAlready = false;
            if (radYes.Checked && oForecast.GetAnswer(intID, "override") == "1")
                boolAlready = true;
            else if (radYes.Checked)
                boolOverride = true;
            int intApplicationID = Int32.Parse(ddlApplications.SelectedItem.Value);
            ServerName oServerName = new ServerName(0, dsn);
            int intSubApplicationID = 0;
            if (oServerName.GetSubApplications(intApplicationID, 1).Tables[0].Rows.Count > 0 && Request.Form[hdnSubApplication.UniqueID] != "")
                intSubApplicationID = Int32.Parse(Request.Form[hdnSubApplication.UniqueID]);
            int intOverride = (boolAlready == true ? 1 : (boolOverride == true ? (radBreakYes.Checked ? 1 : -1) : 0));
            int intClass = Int32.Parse(ddlClass.SelectedItem.Value);
            bool boolName = true;
            if (intOverride == 1)
            {
                if (radBreakYes.Checked == true)
                {
                    int intName = 0;
                    if (oClass.Get(intClass, "pnc") == "1")
                        intName = oServerName.GetNameFactory(txtDeviceName.Text);
                    else
                        intName = oServerName.GetName(txtDeviceName.Text);
                    if (intName == 0)
                        boolName = false;
                    else
                        oForecast.UpdateAnswer(intID, intOverride, 1, txtChange.Text, intName, Request.ServerVariables["REMOTE_HOST"], "", txtName.Text, Int32.Parse(Request.Form[hdnLocation.UniqueID]), intClass, (chkTest.Checked ? 1 : 0), Int32.Parse(Request.Form[hdnEnvironment.UniqueID]), Int32.Parse(ddlMaintenance.SelectedItem.Value), intApplicationID, intSubApplicationID, Int32.Parse(txtQuantity.Text), (radResiliencyYes.Checked ? 1 : 0));
                }
                else
                    oForecast.UpdateAnswer(intID, intOverride, 0, "", 0, Request.ServerVariables["REMOTE_HOST"], "", txtName.Text, Int32.Parse(Request.Form[hdnLocation.UniqueID]), intClass, (chkTest.Checked ? 1 : 0), Int32.Parse(Request.Form[hdnEnvironment.UniqueID]), Int32.Parse(ddlMaintenance.SelectedItem.Value), intApplicationID, intSubApplicationID, Int32.Parse(txtQuantity.Text), (radResiliencyYes.Checked ? 1 : 0));
            }
            else
                oForecast.UpdateAnswer(intID, intOverride, 0, "", 0, Request.ServerVariables["REMOTE_HOST"], "", txtName.Text, Int32.Parse(Request.Form[hdnLocation.UniqueID]), intClass, (chkTest.Checked ? 1 : 0), Int32.Parse(Request.Form[hdnEnvironment.UniqueID]), Int32.Parse(ddlMaintenance.SelectedItem.Value), intApplicationID, intSubApplicationID, Int32.Parse(txtQuantity.Text), (radResiliencyYes.Checked ? 1 : 0));

            if (boolName == true)
            {
                // Set up the forecast steps done table to check if a certain step of a forecast is done.
                DataSet dsSteps = oForecast.GetSteps(intPlatform, 1);
                DataSet dsStepsDone = oForecast.GetStepsDone(intID, 0);
                if (dsSteps.Tables[0].Rows.Count != dsStepsDone.Tables[0].Rows.Count)
                {
                    int intCount = 0;
                    foreach (DataRow drStep in dsSteps.Tables[0].Rows)
                    {
                        intCount++;
                        oForecast.AddStepDone(intID, intCount, 0);
                    }
                }
            }
            else
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "save", "<script type=\"text/javascript\">alert('Please enter a valid device name!\\n\\nThe device name you entered was not found\\n\\nIf you think this device exists, please contact your ClearView administrator');<" + "/" + "script>");
            return boolName;
        }
        protected void btnBack_Click(Object Sender, EventArgs e)
        {
            oForecast.UpdateAnswerStep(intID, -1, -1);
            Response.Redirect(Request.Path + "?id=" + intID.ToString());
        }
        protected void btnNext_Click(Object Sender, EventArgs e)
        {
            bool boolSave = Save();
            if (boolSave == true)
            {
                oForecast.UpdateAnswerStep(intID, 1, 1);
                Response.Redirect(Request.Path + "?id=" + intID.ToString() + "&save=true");
            }
        }
        protected void btnUpdate_Click(Object Sender, EventArgs e)
        {
            bool boolSave = Save();
            if (boolSave == true)
            {
                int intStep = Int32.Parse(Request.QueryString["step"]);
                string strAlert = oForecast.AddStepDone(intID, intStep, 1);
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "save", "<script type=\"text/javascript\">" + strAlert + "window.navigate('" + Request.Path + "?id=" + intID.ToString() + "&save=true');<" + "/" + "script>");
            }
        }
        protected void btnCancel_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path + "?id=" + intID.ToString());
        }
    }
}