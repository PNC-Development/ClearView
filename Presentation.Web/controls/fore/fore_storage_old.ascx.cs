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
    public partial class fore_storage_old : System.Web.UI.UserControl
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);

        protected int intSharedPlatform = Int32.Parse(ConfigurationManager.AppSettings["SharedPlatformID"]);
        protected int intProfile;
        protected Forecast oForecast;
        protected Platforms oPlatform;
        protected Models oModel;
        protected ModelsProperties oModelsProperties;
        protected Locations oLocation;
        protected Classes oClass;
        protected int intForecast;
        protected int intID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oForecast = new Forecast(intProfile, dsn);
            oPlatform = new Platforms(intProfile, dsn);
            oModel = new Models(intProfile, dsn);
            oModelsProperties = new ModelsProperties(intProfile, dsn);
            oLocation = new Locations(intProfile, dsn);
            oClass = new Classes(intProfile, dsn);
            if (Request.QueryString["parent"] != null && Request.QueryString["parent"] != "")
                intForecast = Int32.Parse(Request.QueryString["parent"]);
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                intID = Int32.Parse(Request.QueryString["id"]);
            if (Request.QueryString["step"] != null && Request.QueryString["step"] != "")
                panUpdate.Visible = true;
            else
                panNavigation.Visible = true;
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
                    int intPlatform = Int32.Parse(ds.Tables[0].Rows[0]["platformid"].ToString());
                    int intStep = Int32.Parse(ds.Tables[0].Rows[0]["step"].ToString());
                    bool boolTest = (ds.Tables[0].Rows[0]["test"].ToString() == "1");
                    int intQuantity = Int32.Parse(ds.Tables[0].Rows[0]["quantity"].ToString());
                    int intAddress = Int32.Parse(ds.Tables[0].Rows[0]["addressid"].ToString());
                    intForecast = Int32.Parse(ds.Tables[0].Rows[0]["forecastid"].ToString());
                    DataSet dsSteps = oForecast.GetSteps(intPlatform, 1);
                    if (dsSteps.Tables[0].Rows.Count == intStep)
                        btnNext.Text = "Finish";
                    if (intStep == 0 || intStep == 1)
                        btnBack.Enabled = false;
                    if (oLocation.GetAddress(intAddress, "storage") != "1")
                    {
                        radNo.Checked = true;
                        radYes.Enabled = false;
                        radYes.ToolTip = "Location " + oLocation.GetFull(intAddress) + " is not configured for storage";
                    }
                    else
                    {
                        bool boolProduction = oClass.IsProd(Int32.Parse(ds.Tables[0].Rows[0]["classid"].ToString()));
                        bool boolQA = oClass.IsQA(Int32.Parse(ds.Tables[0].Rows[0]["classid"].ToString()));
                        bool boolNone = (ds.Tables[0].Rows[0]["storage"].ToString() == "-2");
                        bool boolRequired = oForecast.IsHACluster(intID);
                        bool boolNoReplication = oForecast.IsDROver48(intID, false);
                        // Get Model
                        int intModel = oForecast.GetModel(intID);
                        if (oModelsProperties.IsStorageDE_FDriveMustBeOnSAN(intModel) == true)
                            boolRequired = true;
                        Types oType = new Types(intProfile, dsn);
                        if (oType.GetPlatform(oModelsProperties.GetType(intModel)) == intSharedPlatform)
                            boolRequired = true;
                        if (boolRequired == true)
                        {
                            radNo.Enabled = false;
                            if (boolNone == false)
                                divYes.Style["display"] = "inline";
                            radNo.ToolTip = "Storage Required for Clusters";
                        }
                        if (!IsPostBack)
                        {
                            if (boolProduction == true)
                            {
                                panOldHighProduction.Visible = true;
                                panOldStandardProduction.Visible = true;
                                panOldLowProduction.Visible = true;
                            }
                            if (boolQA == true)
                            {
                                panOldHighQA.Visible = true;
                                panOldStandardQA.Visible = true;
                                panOldLowQA.Visible = true;
                            }
                            if ((boolProduction == false && boolQA == false) || (boolProduction == true && boolTest == true))
                            {
                                panOldHighTest.Visible = true;
                                panOldStandardTest.Visible = true;
                                panOldLowTest.Visible = true;
                            }
                            if (boolNoReplication == false)     // If Replication = true
                            {
                                panOldStandardReplication.Visible = true;
                                ddlOldStandardReplicated.Enabled = false;
                                ddlOldStandardReplicated.SelectedValue = "Yes";
                                ddlOldStandardReplicated.ToolTip = "Under 48 Hours requires Replication";
                                divOldStandardReplicated.Style["display"] = "inline";
                            }
                            if (ds.Tables[0].Rows[0]["storage"].ToString() == "1")
                            {
                                radYes.Checked = true;
                                ds = oForecast.GetStorageOS(intID);
                                panReset.Visible = (ds.Tables[0].Rows.Count > 0);
                                ds = oForecast.GetStorage(intID);
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    divYes.Style["display"] = "inline";
                                    if (ds.Tables[0].Rows[0]["high"].ToString() == "1")
                                    {
                                        chkOldHigh.Checked = true;
                                        divOldHigh.Style["display"] = "inline";
                                        txtOldHighRequire.Text = ds.Tables[0].Rows[0]["high_total"].ToString();
                                        txtOldHighRequireQA.Text = ds.Tables[0].Rows[0]["high_qa"].ToString();
                                        txtOldHighRequireTest.Text = ds.Tables[0].Rows[0]["high_test"].ToString();
                                    }
                                    if (ds.Tables[0].Rows[0]["standard"].ToString() == "1")
                                    {
                                        chkOldStandard.Checked = true;
                                        divOldStandard.Style["display"] = "inline";
                                        txtOldStandardRequire.Text = ds.Tables[0].Rows[0]["standard_total"].ToString();
                                        txtOldStandardRequireQA.Text = ds.Tables[0].Rows[0]["standard_qa"].ToString();
                                        txtOldStandardRequireTest.Text = ds.Tables[0].Rows[0]["standard_test"].ToString();
                                        double dblStandardReplicated = double.Parse(ds.Tables[0].Rows[0]["standard_replicated"].ToString());
                                        if (dblStandardReplicated > 0.00)
                                        {
                                            divOldStandardReplicated.Style["display"] = "inline";
                                            txtOldStandardReplicated.Text = dblStandardReplicated.ToString();
                                            ddlOldStandardReplicated.SelectedValue = "Yes";
                                        }
                                    }
                                    if (ds.Tables[0].Rows[0]["low"].ToString() == "1")
                                    {
                                        chkOldLow.Checked = true;
                                        divOldLow.Style["display"] = "inline";
                                        txtOldLowRequire.Text = ds.Tables[0].Rows[0]["low_total"].ToString();
                                        txtOldLowRequireQA.Text = ds.Tables[0].Rows[0]["low_qa"].ToString();
                                        txtOldLowRequireTest.Text = ds.Tables[0].Rows[0]["low_test"].ToString();
                                    }
                                }
                            }
                            else if (ds.Tables[0].Rows[0]["storage"].ToString() == "-1")
                                radLater.Checked = true;
                            else if (boolRequired == true && boolNone == false)
                                radYes.Checked = true;
                            else
                                radNo.Checked = true;
                        }
                        chkOldHigh.Attributes.Add("onclick", "ShowHideDivCheck('" + divOldHigh.ClientID + "',this);");
                        chkOldStandard.Attributes.Add("onclick", "ShowHideDivCheck('" + divOldStandard.ClientID + "',this);");
                        ddlOldStandardReplicated.Attributes.Add("onchange", "ShowHideDivDDL('" + divOldStandardReplicated.ClientID + "',this,'Yes');");
                        chkOldLow.Attributes.Add("onclick", "ShowHideDivCheck('" + divOldLow.ClientID + "',this);");
                        btnNext.Attributes.Add("onclick", "return EnsureStorage('" + radYes.ClientID + "','" + chkOldHigh.ClientID + "','" + chkOldStandard.ClientID + "','" + chkOldLow.ClientID + "')" +
                            " && EnsureStorageOld('" + radYes.ClientID + "','" + chkOldHigh.ClientID + "','" + txtOldHighRequire.ClientID + "','" + txtOldHighRequireQA.ClientID + "','" + txtOldHighRequireTest.ClientID + "'," + (boolProduction ? "true" : "false") + ",false,null,null)" +
                            " && EnsureStorageOld('" + radYes.ClientID + "','" + chkOldStandard.ClientID + "','" + txtOldStandardRequire.ClientID + "','" + txtOldStandardRequireQA.ClientID + "','" + txtOldStandardRequireTest.ClientID + "'," + (boolProduction ? "true" : "false") + "," + (boolNoReplication ? "true" : "false") + ",'" + ddlOldStandardReplicated.ClientID + "','" + txtOldStandardReplicated.ClientID + "')" +
                            " && EnsureStorageOld('" + radYes.ClientID + "','" + chkOldLow.ClientID + "','" + txtOldLowRequire.ClientID + "','" + txtOldLowRequireQA.ClientID + "','" + txtOldLowRequireTest.ClientID + "'," + (boolProduction ? "true" : "false") + ",false,null,null)" +
                            ";");
                        btnUpdate.Attributes.Add("onclick", "return EnsureStorage('" + radYes.ClientID + "','" + chkOldHigh.ClientID + "','" + chkOldStandard.ClientID + "','" + chkOldLow.ClientID + "')" +
                            " && EnsureStorageOld('" + radYes.ClientID + "','" + chkOldHigh.ClientID + "','" + txtOldHighRequire.ClientID + "','" + txtOldHighRequireQA.ClientID + "','" + txtOldHighRequireTest.ClientID + "'," + (boolProduction ? "true" : "false") + ",false,null,null)" +
                            " && EnsureStorageOld('" + radYes.ClientID + "','" + chkOldStandard.ClientID + "','" + txtOldStandardRequire.ClientID + "','" + txtOldStandardRequireQA.ClientID + "','" + txtOldStandardRequireTest.ClientID + "'," + (boolProduction ? "true" : "false") + "," + (boolNoReplication ? "true" : "false") + ",'" + ddlOldStandardReplicated.ClientID + "','" + txtOldStandardReplicated.ClientID + "')" +
                            " && EnsureStorageOld('" + radYes.ClientID + "','" + chkOldLow.ClientID + "','" + txtOldLowRequire.ClientID + "','" + txtOldLowRequireQA.ClientID + "','" + txtOldLowRequireTest.ClientID + "'," + (boolProduction ? "true" : "false") + ",false,null,null)" +
                            ";");
                        btnReset.Attributes.Add("onclick", "return OpenWindow('RESET_STORAGE', '?id=" + intID.ToString() + "');");
                    }
                }
            }
            radLater.Attributes.Add("onclick", "ShowHideDiv('" + divYes.ClientID + "','none');");
            radNo.Attributes.Add("onclick", "ShowHideDiv('" + divYes.ClientID + "','none');");
            radYes.Attributes.Add("onclick", "ShowHideDiv('" + divYes.ClientID + "','inline');");
            btnClose.Attributes.Add("onclick", "return window.close();");
        }
        protected int Save()
        {
            oForecast.DeleteStorage(intID);
            if (radYes.Checked == true)
            {
                int intHigh = (chkOldHigh.Checked ? 1 : 0);
                double dblHighTotal = 0.00;
                double dblHighQA = 0.00;
                double dblHighTest = 0.00;
                if (intHigh == 1)
                {
                    if (panOldHighProduction.Visible == true)
                        dblHighTotal = double.Parse(txtOldHighRequire.Text);
                    if (panOldHighQA.Visible == true)
                        dblHighQA = double.Parse(txtOldHighRequireQA.Text);
                    if (panOldHighTest.Visible == true)
                        dblHighTest = double.Parse(txtOldHighRequireTest.Text);
                }
                int intStandard = (chkOldStandard.Checked ? 1 : 0);
                double dblStandardTotal = 0.00;
                double dblStandardQA = 0.00;
                double dblStandardTest = 0.00;
                double dblStandardReplicated = 0.00;
                if (intStandard == 1)
                {
                    if (panOldStandardProduction.Visible == true)
                    {
                        dblStandardTotal = double.Parse(txtOldStandardRequire.Text);
                        if (panOldStandardReplication.Visible == true && ddlOldStandardReplicated.SelectedIndex == (ddlOldStandardReplicated.Items.Count - 1))
                            dblStandardReplicated = double.Parse(txtOldStandardReplicated.Text);
                    }
                    if (panOldStandardQA.Visible == true)
                        dblStandardQA = double.Parse(txtOldStandardRequireQA.Text);
                    if (panOldStandardTest.Visible == true)
                        dblStandardTest = double.Parse(txtOldStandardRequireTest.Text);
                }
                int intLow = (chkOldLow.Checked ? 1 : 0);
                double dblLowTotal = 0.00;
                double dblLowQA = 0.00;
                double dblLowTest = 0.00;
                if (intLow == 1)
                {
                    if (panOldLowProduction.Visible == true)
                        dblLowTotal = double.Parse(txtOldLowRequire.Text);
                    if (panOldLowQA.Visible == true)
                        dblLowQA = double.Parse(txtOldLowRequireQA.Text);
                    if (panOldLowTest.Visible == true)
                        dblLowTest = double.Parse(txtOldLowRequireTest.Text);
                }
                oForecast.AddStorage(intID, intHigh, dblHighTotal, dblHighQA, dblHighTest, 0.00, "", 0.00, intStandard, dblStandardTotal, dblStandardQA, dblStandardTest, dblStandardReplicated, "", 0.00, intLow, dblLowTotal, dblLowQA, dblLowTest, 0.00, "", 0.00);
            }
            int intStorage = 0;
            if (radLater.Checked == true)
                intStorage = -1;
            else if (radYes.Checked == true)
                intStorage = 1;
            oForecast.UpdateStorage(intID, intStorage);
            return (radNo.Checked == true || radYes.Checked == true ? 1 : 0);
        }
        protected void btnBack_Click(Object Sender, EventArgs e)
        {
            oForecast.UpdateAnswerStep(intID, -1, -1);
            Response.Redirect(Request.Path + "?id=" + intID.ToString());
        }
        protected void btnNext_Click(Object Sender, EventArgs e)
        {
            int intDone = 0;
            if (radNo.Checked == true)
                intDone = 1;
            else
                intDone = Save();
            oForecast.UpdateAnswerStep(intID, 1, intDone);
            Response.Redirect(Request.Path + "?id=" + intID.ToString() + "&save=true");
        }
        protected void btnUpdate_Click(Object Sender, EventArgs e)
        {
            int intDone = 0;
            //if (radNo.Checked == true)
            //    intDone = 1;
            //else
            intDone = Save();
            int intStep = Int32.Parse(Request.QueryString["step"]);
            string strAlert = oForecast.AddStepDone(intID, intStep, intDone);
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "save", "<script type=\"text/javascript\">" + strAlert + "window.navigate('" + Request.Path + "?id=" + intID.ToString() + "&save=true');<" + "/" + "script>");
        }
        protected void btnCancel_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path + "?id=" + intID.ToString());
        }
    }
}