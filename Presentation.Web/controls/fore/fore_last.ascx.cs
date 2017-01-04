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
using System.Net.Mail;
using System.Text;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;

namespace NCC.ClearView.Presentation.Web
{
    public partial class fore_last : System.Web.UI.UserControl
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
      

        protected int intEdit = Int32.Parse(ConfigurationManager.AppSettings["ForecastEdit"]);
        protected string strIARB = ConfigurationManager.AppSettings["IARB"];
        protected int intIARB = Int32.Parse(ConfigurationManager.AppSettings["IARB_PAGEID"]);
        protected int intProfile;
        protected Forecast oForecast;
        protected Platforms oPlatform;
        protected Locations oLocation;
        protected Confidence oConfidence;
        protected Models oModel;
        protected ModelsProperties oModelsProperties;
        protected int intForecast;
        protected int intID = 0;
        protected int intQuantity = 0;
        protected bool boolRecoveryOne = false;
        protected bool boolRecoveryMany = false;
        protected bool boolHA = false;
        protected int intModel = 0;
        protected Variables oVariable;
        protected Classes oClass;
        protected Functions oFunction;
        protected bool boolVariance = false;
        protected bool boolFound = false;
        protected bool boolOverridePending = false;
        protected bool boolOverrideRejected = false;
        protected bool boolOverrideAlready = false;
        protected bool boolOverrideNone = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oForecast = new Forecast(intProfile, dsn);
            oPlatform = new Platforms(intProfile, dsn);
            oLocation = new Locations(intProfile, dsn);
            oConfidence = new Confidence(intProfile, dsn);
            oModel = new Models(intProfile, dsn);
            oModelsProperties = new ModelsProperties(intProfile, dsn);
            oVariable = new Variables(intEnvironment);
            oClass = new Classes(intProfile, dsn);
            oFunction = new Functions(intProfile, dsn, intEnvironment);
            if (Request.QueryString["parent"] != null && Request.QueryString["parent"] != "")
                intForecast = Int32.Parse(Request.QueryString["parent"]);
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                intID = Int32.Parse(Request.QueryString["id"]);
            if (Request.QueryString["step"] != null && Request.QueryString["step"] != "")
                panUpdate.Visible = true;
            else
                panNavigation.Visible = true;
            if (Request.QueryString["code"] != null)
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "code", "<script type=\"text/javascript\">alert('Invalid Override Code!\\n\\nPlease contact your ClearView administrator.');<" + "/" + "script>");
            if (!IsPostBack)
                LoadLists();
            lblBurn.Text = DateTime.Today.AddDays(14).ToShortDateString();
            if (intID > 0)
            {
                DataSet ds = oForecast.GetAnswer(intID);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    bool boolHundred = false;
                    int intConfidence = Int32.Parse(ds.Tables[0].Rows[0]["confidenceid"].ToString());
                    if (intConfidence > 0)
                    {
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
                    intForecast = Int32.Parse(ds.Tables[0].Rows[0]["forecastid"].ToString());
                    intQuantity = Int32.Parse(ds.Tables[0].Rows[0]["quantity"].ToString());
                    switch (ds.Tables[0].Rows[0]["override"].ToString())
                    {
                        case "-10":
                            panOverride.Visible = true;
                            boolOverrideRejected = true;
                            lblStatus.Text = "<img src=\"/images/error.gif\" border=\"0\" align=\"absmiddle\"/> Rejected by the Review Board";
                            lblStatusSub.Text = "The board has reviewed your overridden design and has denied your request. Please select &quot;No&quot; to the Override question located on the first page to continue.";
                            panComments.Visible = true;
                            lblComments.Text = ds.Tables[0].Rows[0]["comments"].ToString();
                            break;
                        case "-1":
                            panOverride.Visible = true;
                            boolOverridePending = true;
                            lblStatus.Text = "<img src=\"/images/pending.gif\" border=\"0\" align=\"absmiddle\"/> Pending Approval from Review Board";
                            lblStatusSub.Text = "The board has been contacted and is currently reviewing your design. You will be contacted when your design has been reviewed. <br/><b>NOTE:</b> Because overridden designs require approval, the on-demand SLA no longer applies. The SLA for overridden designs is 15 days.";
                            lblComments.Text = ds.Tables[0].Rows[0]["comments"].ToString();
                            break;
                        case "0":
                            panOverride.Visible = false;
                            boolOverrideNone = true;
                            break;
                        case "1":
                            panOverride.Visible = true;
                            boolOverrideAlready = true;
                            lblStatus.Text = "<img src=\"/images/check.gif\" border=\"0\" align=\"absmiddle\"/> Approved by the Review Board";
                            lblStatusSub.Text = "The board has reviewed your overridden design and has accepted your request. You may now continue with the configuraion of your design.";
                            panComments.Visible = true;
                            lblComments.Text = ds.Tables[0].Rows[0]["comments"].ToString();
                            break;
                    }
                    lblComments.Text = ds.Tables[0].Rows[0]["comments"].ToString();
                    if (lblComments.Text == "")
                        lblComments.Text = "<i>None</i>";
                    DataSet dsSteps = oForecast.GetSteps(intPlatform, 1);
                    if (dsSteps.Tables[0].Rows.Count == intStep)
                        btnNext.Text = "Finish";
                    if (intStep == 0 || intStep == 1)
                        btnBack.Enabled = false;
                    boolRecoveryOne = (oForecast.IsDRUnder48(intID, false) && oForecast.IsDROneToOne(intID));
                    boolRecoveryMany = (oForecast.IsDRUnder48(intID, false) && oForecast.IsDRManyToOne(intID));
                    boolHA = oForecast.IsHARoom(intID);
                    int intServerModel = oForecast.GetModel(intID);
                    intModel = intServerModel;
                    if (oModelsProperties.Get(intServerModel).Tables[0].Rows.Count > 0)
                        intModel = Int32.Parse(oModelsProperties.Get(intServerModel, "modelid"));
                    if (!IsPostBack)
                    {
                        int intCount = 0;
                        bool boolCompleted = true;
                        string strCompleted = "";
                        DataSet dsStepsDone = oForecast.GetStepsDone(intID, 0);
                        foreach (DataRow drStep in dsSteps.Tables[0].Rows)
                        {
                            intCount++;
                            if (dsStepsDone.Tables[0].Rows[intCount - 1]["done"].ToString() == "0")
                            {
                                if (Int32.Parse(dsStepsDone.Tables[0].Rows[intCount - 1]["step"].ToString()) != dsStepsDone.Tables[0].Rows.Count)
                                {
                                    boolCompleted = false;
                                    strCompleted = dsStepsDone.Tables[0].Rows[intCount - 1]["step"].ToString();
                                    break;
                                }
                            }
                        }
                        panHA.Visible = boolHA;
                        txtHA.Text = ds.Tables[0].Rows[0]["ha"].ToString();
                        if (txtHA.Text == "0")
                            txtHA.Text = "";
                        panRecovery.Visible = (boolRecoveryMany == true);
                        if (ds.Tables[0].Rows[0]["implementation"].ToString() != "")
                            txtDate.Text = DateTime.Parse(ds.Tables[0].Rows[0]["implementation"].ToString()).ToShortDateString();
                        ddlConfidence.SelectedValue = ds.Tables[0].Rows[0]["confidenceid"].ToString();
                        txtRecovery.Text = ds.Tables[0].Rows[0]["recovery_number"].ToString();
                        if (txtRecovery.Text == "0")
                            txtRecovery.Text = "";
                        imgDate.Attributes.Add("onclick", "return ShowCalendar('" + txtDate.ClientID + "');");
                        btnNext.Attributes.Add("onclick", "return ValidateDate('" + txtDate.ClientID + "','Please enter a valid Start Build Date')" +
                            " && ValidateDateToday('" + txtDate.ClientID + "','The Start Build Date must occur after today')" +
                            " && EnsureConfidence('" + ddlConfidence.ClientID + "'," + (boolCompleted ? "true" : "false") + ",'NOTE: You have not completed this design.\\n\\nOnly completed designs can be set to 100% confidence. Please set the confidence to 80% and save to review your incomplete steps.\\n\\nStep: " + strCompleted + "')" +
                            " && EnsureConfidence2('" + ddlConfidence.ClientID + "'," + (intModel == 0 ? "true" : "false") + ",'NOTE: ClearView is unable to find a solution for your design.\\n\\nOnly designs with available solutions can be set to 100% or 80% confidence level.')" +
                            " && EnsureOverride(" + (boolOverrideNone == true ? "false" : "true") + ",'" + (boolOverrideRejected == true ? "The override for this design was rejected. Please set uncheck the override option (located on the first page) to continue." : "") + "')" +
                            " && ForeRecoveryLast(" + (boolRecoveryMany ? "true" : "false") + ",'" + txtRecovery.ClientID + "'," + intQuantity.ToString() + ")" +
                            " && ForeHALast(" + (boolHA ? "true" : "false") + ",'" + txtHA.ClientID + "'," + intQuantity.ToString() + ")" +
                            ";");
                        btnUpdate.Attributes.Add("onclick", "return ValidateDate('" + txtDate.ClientID + "','Please enter a valid Start Build Date')" +
                            " && ValidateDateToday('" + txtDate.ClientID + "','The Start Build Date must occur after today')" +
                            " && EnsureConfidence('" + ddlConfidence.ClientID + "'," + (boolCompleted ? "true" : "false") + ",'NOTE: You have not completed this design.\\n\\nOnly completed designs can be set to 100% confidence. Please set the confidence to 80% and save to review your incomplete steps.\\n\\nStep: " + strCompleted + "')" +
                            " && EnsureConfidence2('" + ddlConfidence.ClientID + "'," + (intModel == 0 ? "true" : "false") + ",'NOTE: ClearView is unable to find a solution for your design.\\n\\nOnly designs with available solutions can be set to 100% or 80% confidence level.')" +
                            " && EnsureOverride(" + (boolOverrideNone == true ? "false" : "true") + ",'" + (boolOverrideRejected == true ? "The override for this design was rejected. Please set uncheck the override option (located on the first page) to continue." : "") + "')" +
                            " && ForeRecoveryLast(" + (boolRecoveryMany ? "true" : "false") + ",'" + txtRecovery.ClientID + "'," + intQuantity.ToString() + ")" +
                            " && ForeHALast(" + (boolHA ? "true" : "false") + ",'" + txtHA.ClientID + "'," + intQuantity.ToString() + ")" +
                            ";");
                        btnClose.Attributes.Add("onclick", "return window.close();");
                    }
                }
            }
        }
        private void LoadLists()
        {
            ddlConfidence.DataValueField = "id";
            ddlConfidence.DataTextField = "name";
            ddlConfidence.DataSource = oConfidence.Gets(1);
            ddlConfidence.DataBind();
        }
        private bool Save()
        {
            int intHA = 0;
            if (panHA.Visible == true)
                intHA = Int32.Parse(txtHA.Text);
            int intRecovery = (boolRecoveryOne ? intQuantity : 0);
            if (panRecovery.Visible == true)
                intRecovery = Int32.Parse(txtRecovery.Text);
            oForecast.UpdateAnswer(intID, DateTime.Parse(txtDate.Text), Int32.Parse(ddlConfidence.SelectedItem.Value), intProfile);
            oForecast.UpdateAnswerRecovery(intID, intRecovery);
            oForecast.UpdateAnswerHA(intID, intHA);

            string strConfidence = ddlConfidence.SelectedItem.Text;
            bool boolConfidence = (strConfidence.Contains("80%") || strConfidence.Contains("100%"));
            if (strConfidence.Contains("100%") && boolOverridePending == true)
            {
                int intLocation = Int32.Parse(oForecast.GetAnswer(intID, "addressid"));
                int intClass = Int32.Parse(oForecast.GetAnswer(intID, "classid"));
                //if (oClass.Get(intClass, "pnc") == "1")
                //{
                //    // Skip Dalton for PNC Integration
                //    oForecast.UpdateAnswerApproval(intID, 1, -1000, "PNC Override Automatically Approved");
                //}
                //else
                //{
                    // Send to IARB for approval
                    Users oUser = new Users(intProfile, dsn);
                    Pages oPage = new Pages(intProfile, dsn);
                    string[] strEmail;
                    char[] strSplit = { ';' };
                    strEmail = strIARB.Split(strSplit);
                    for (int ii = 0; ii < strEmail.Length; ii++)
                    {
                        if (strEmail[ii].Trim() != "")
                        {
                            string strAddress = strEmail[ii];
                            //int intUser = oUser.GetId(strAddress);
                            //string strDefault = oUser.GetApplicationUrl(intUser, intIARB);
                            //if (strDefault == "")
                            //oFunction.SendEmail("Review Board Approval", oUser.GetName(intUser), "", strBCC, "Review Board Approval", "<p><b>A design has been overridden and requires your approval.</b><p><p>" + oForecast.GetAnswerBody(intID, intEnvironment) + "</p>", true, false);
                            //else
                            //oFunction.SendEmail("Review Board Approval", oUser.GetName(intUser), "", strBCC, "Review Board Approval", "<p><b>A design has been overridden and requires your approval.</b><p><p>" + oForecast.GetAnswerBody(intID, intEnvironment) + "</p><p><a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=/" + strDefault + oPage.GetFullLink(intIARB) + "?id=" + intID.ToString() + "\" target=\"_blank\">Click here to review this design.</a></p>", true, false);
                            string strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DESIGN_BUILDER");
                            oFunction.SendEmail("Review Board Approval", strAddress, "", strEMailIdsBCC, "Review Board Approval", "<p><b>A design has been overridden and requires your approval.</b><p><p>" + oForecast.GetAnswerBody(intID, intEnvironment, dsnAsset, dsnIP) + "</p><p>To view this design, log into <a href=\"" + oVariable.URL() + "\" target=\"_blank\">ClearView</a> and click &quot;Design Builder&quot; | &quot;Override Approval&quot;</p>", true, false);
                        }
                    }
                //}
            }

            // VIJAY - start
            if (intModel > 0)
            {
                int intTypeId = Int32.Parse(oModel.Get(intModel, "typeid"));
                Types oType = new Types(intProfile, dsn);
                string strTypeName = oType.Get(intTypeId, "name");
                string strBody = GenerateText();
                string strEMailIdsTO = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DESIGN_BUILDER");
                if (boolConfidence == true && ((strTypeName == "Distributed") || (strTypeName == "Midrange") || boolVariance == true) && strBody != "" && boolFound == true)
                    oFunction.SendEmail("Clearview Design Profile (Standalone Server)", strEMailIdsTO, "", "", "Clearview Design Profile (Standalone Server)", strBody, false, true);
                if (boolConfidence == true && (((strTypeName == "Distributed") || (strTypeName == "Midrange")) && boolVariance == true) && strBody != "" && boolFound == true)
                    oFunction.SendEmail("Clearview Design Profile (Variance)", strEMailIdsTO, "", "", "Clearview Design Profile (Variance)", strBody, false, true);
            }
            // VIJAY - end
            return true;
        }
        protected void btnBack_Click(Object Sender, EventArgs e)
        {
            int intDecrement = -1;
            DataSet ds = oForecast.GetAnswer(intID);
            if (ds.Tables[0].Rows.Count > 0)
            {
                int intPlatform = Int32.Parse(ds.Tables[0].Rows[0]["platformid"].ToString());
                int intStep = Int32.Parse(ds.Tables[0].Rows[0]["step"].ToString());
                int intRow = intStep - 2;
                DataSet dsSteps = oForecast.GetSteps(intPlatform, 1);
                if (intStep > 0 && intStep > 1 && dsSteps.Tables[0].Rows[intRow]["additional"].ToString() == "1")
                {
                    intDecrement = -2;
                    DataSet dsResponses = oForecast.GetResponses(intID);
                    if (dsResponses.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow drResponse in dsResponses.Tables[0].Rows)
                        {
                            DataSet dsAdditional = oForecast.GetResponseAdditional(Int32.Parse(drResponse["id"].ToString()));
                            foreach (DataRow drAdditional in dsAdditional.Tables[0].Rows)
                            {
                                string strPath = oForecast.GetStepAdditional(Int32.Parse(drAdditional["additionalid"].ToString()), "path");
                                if (strPath != "")
                                {
                                    intDecrement = -1;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            oForecast.UpdateAnswerStep(intID, intDecrement, -1);
            Response.Redirect(Request.Path + "?id=" + intID.ToString());
        }
        protected void btnNext_Click(Object Sender, EventArgs e)
        {
            bool boolError = Save();
            string strStep = "";
            if (Request.QueryString["step"] != null && Request.QueryString["step"] != "")
                strStep = "&step=" + Request.QueryString["step"];
            if (boolError == false)
                Response.Redirect(Request.Path + "?id=" + intID.ToString() + strStep + "&code=true");
            else
            {
                oForecast.UpdateAnswerStep(intID, 1, 1);
                CheckBlade();
            }
        }
        protected void btnUpdate_Click(Object Sender, EventArgs e)
        {
            bool boolError = Save();
            string strStep = "";
            if (Request.QueryString["step"] != null && Request.QueryString["step"] != "")
                strStep = "&step=" + Request.QueryString["step"];
            if (boolError == false)
                Response.Redirect(Request.Path + "?id=" + intID.ToString() + strStep + "&code=true");
            else
            {
                int intStep = Int32.Parse(Request.QueryString["step"]);
                oForecast.AddStepDone(intID, intStep, 1);
                CheckBlade();
            }
        }
        private void CheckBlade()
        {
            int intQuantity = 0;
            int intTotal = 0;
            DataSet ds = oForecast.GetAnswer(intID);
            if (ds.Tables[0].Rows.Count > 0)
                intQuantity = (ds.Tables[0].Rows[0]["quantity"].ToString() == "" ? 0 : Int32.Parse(ds.Tables[0].Rows[0]["quantity"].ToString()));
            DataSet dsStorage = oForecast.GetStorage(intID);
            if (dsStorage.Tables[0].Rows.Count > 0)
            {
                int intHigh = Int32.Parse(dsStorage.Tables[0].Rows[0]["high_total"].ToString());
                int intStandard = Int32.Parse(dsStorage.Tables[0].Rows[0]["standard_total"].ToString());
                int intLow = Int32.Parse(dsStorage.Tables[0].Rows[0]["low_total"].ToString());
                intTotal = intHigh + intStandard + intLow;
            }
            Response.Redirect(Request.Path + "?id=" + intID.ToString() + "&save=true");
        }
        // VIJAY - START
        private string GenerateText()
        {
            Pages oPage = new Pages(intProfile, dsn);
            StringBuilder oBuilder = new StringBuilder();
            Forecast forecast = new Forecast(intProfile, dsn);
            DataSet dsForecast = forecast.GetForecast(intID);
            if (dsForecast.Tables[0].Rows.Count > 0)
            {
                boolFound = true;
                oBuilder.Append("*** PROJECT INFORMATION ***\n");
                oBuilder.Append("Project Name:\t\t\t" + dsForecast.Tables[0].Rows[0]["name"].ToString().Trim() + "\n");
                oBuilder.Append("Project Coordinator:\t\t" + dsForecast.Tables[0].Rows[0]["project_lead"].ToString().Trim() + "\n");
                oBuilder.Append("Project Number:\t\t\t" + dsForecast.Tables[0].Rows[0]["number"].ToString().Trim() + "\n");
                oBuilder.Append("Integration Engineer:\t\t" + dsForecast.Tables[0].Rows[0]["integration_engineer"].ToString().Trim() + "\n");
                oBuilder.Append("Project Type:\t\t\t" + dsForecast.Tables[0].Rows[0]["bd"].ToString().Trim() + "\n");
                oBuilder.Append("Support Technician:\t\t" + dsForecast.Tables[0].Rows[0]["technical_lead"].ToString().Trim() + "\n");
                oBuilder.Append("Project Status:\t\t\t" + dsForecast.Tables[0].Rows[0]["status"].ToString().Trim() + "\n");
                oBuilder.Append("Portfolio:\t\t\t\t" + dsForecast.Tables[0].Rows[0]["organization"].ToString().Trim() + "\n\n");

                oBuilder.Append("*** DESIGN INFORMATION ***\n");
                oBuilder.Append("Designed By:\t\t\t" + dsForecast.Tables[0].Rows[0]["requestor"].ToString().Trim() + "\n");
                oBuilder.Append("Quantity:\t\t\t\t" + dsForecast.Tables[0].Rows[0]["quantity"].ToString().Trim() + "\n");
                oBuilder.Append("Model:\t\t\t\t" + dsForecast.Tables[0].Rows[0]["modelname"].ToString().Trim() + "\n");
                oBuilder.Append("Commitment Date:\t\t\t" + Convert.ToDateTime(dsForecast.Tables[0].Rows[0]["commitment"].ToString()).ToShortDateString().Trim() + "\n");
                oBuilder.Append("Confidence Level:\t\t\t" + dsForecast.Tables[0].Rows[0]["confidence"].ToString().Trim() + "\n");
                oBuilder.Append("Total Acquisition Costs:\t" + dsForecast.Tables[0].Rows[0]["acquisition"].ToString().Trim() + "\n");
                oBuilder.Append("Last Updated:\t\t\t" + Convert.ToDateTime(dsForecast.Tables[0].Rows[0]["modified"].ToString()).ToShortDateString().Trim() + "\n");
                oBuilder.Append("Total Operational Costs:\t" + dsForecast.Tables[0].Rows[0]["operational"].ToString().Trim() + "\n");
                oBuilder.Append("Total Storage: \t\t\tHigh: " + dsForecast.Tables[0].Rows[0]["high_total"].ToString().Trim() + " GB | Standard: " + dsForecast.Tables[0].Rows[0]["standard_total"] + " GB | Low: " + dsForecast.Tables[0].Rows[0]["low_total"] + " GB \n");
                oBuilder.Append("Total AMP Draw:\t\t\t" + dsForecast.Tables[0].Rows[0]["amp"].ToString().Trim() + " AMP(s) \n");
                oBuilder.Append("Selection Matrix Overide:\t" + dsForecast.Tables[0].Rows[0]["override"].ToString().Trim() + " \n");
                oBuilder.Append("Platform:\t\t\t\t" + dsForecast.Tables[0].Rows[0]["platform"].ToString().Trim() + "\n");
                oBuilder.Append("Location:\t\t\t\t" + dsForecast.Tables[0].Rows[0]["location"].ToString().Trim() + "\n");
                oBuilder.Append("Web Software:\t\t\t" + dsForecast.Tables[0].Rows[0]["web"].ToString().Trim() + "" + (dsForecast.Tables[0].Rows[0]["web_variance"].ToString().Trim() == "1" ? " [VARIANCE]" : "") + "\n");
                if (dsForecast.Tables[0].Rows[0]["web_variance"].ToString().Trim() == "1")
                    boolVariance = true;
                oBuilder.Append("Database Software:\t\t" + dsForecast.Tables[0].Rows[0]["db"].ToString().Trim() + "" + (dsForecast.Tables[0].Rows[0]["db_variance"].ToString().Trim() == "1" ? " [VARIANCE]" : "") + "\n");
                if (dsForecast.Tables[0].Rows[0]["db_variance"].ToString().Trim() == "1")
                    boolVariance = true;
                oBuilder.Append("Class:\t\t\t\t" + dsForecast.Tables[0].Rows[0]["class"].ToString().Trim() + "\n");
                oBuilder.Append("Operating System:\t\t\t" + dsForecast.Tables[0].Rows[0]["operating_system"].ToString().Trim() + "" + (dsForecast.Tables[0].Rows[0]["os_variance"].ToString().Trim() == "1" ? " [VARIANCE]" : "") + "\n");
                if (dsForecast.Tables[0].Rows[0]["os_variance"].ToString().Trim() == "1")
                    boolVariance = true;
                oBuilder.Append("Environment:\t\t\t" + dsForecast.Tables[0].Rows[0]["environment"].ToString().Trim() + "\n");
                oBuilder.Append("Backup Required:\t\t\t" + dsForecast.Tables[0].Rows[0]["backup_frequency"].ToString().Trim() + "\n");
                oBuilder.Append("Minimum amount of CPU:\t\t" + dsForecast.Tables[0].Rows[0]["cores"].ToString().Trim() + " CPU(s)" + (dsForecast.Tables[0].Rows[0]["cores_variance"].ToString().Trim() == "1" ? " [VARIANCE]" : "") + "\n");
                if (dsForecast.Tables[0].Rows[0]["cores_variance"].ToString().Trim() == "1")
                    boolVariance = true;
                oBuilder.Append("Recovery Type:\t\t\t" + dsForecast.Tables[0].Rows[0]["recovery"].ToString().Trim() + "" + (dsForecast.Tables[0].Rows[0]["recovery_variance"].ToString().Trim() == "1" ? " [VARIANCE]" : "") + "\n");
                if (dsForecast.Tables[0].Rows[0]["recovery_variance"].ToString().Trim() == "1")
                    boolVariance = true;
                oBuilder.Append("Minimum amount of RAM:\t\t" + dsForecast.Tables[0].Rows[0]["ram"].ToString().Trim() + " GB" + (dsForecast.Tables[0].Rows[0]["ram_variance"].ToString().Trim() == "1" ? " [VARIANCE]" : "") + "\n");
                if (dsForecast.Tables[0].Rows[0]["ram_variance"].ToString().Trim() == "1")
                    boolVariance = true;
                oBuilder.Append("High Availability Method:\t" + dsForecast.Tables[0].Rows[0]["high"].ToString().Trim() + "" + (dsForecast.Tables[0].Rows[0]["high_variance"].ToString().Trim() == "1" ? " [VARIANCE]" : "") + "\n");
                if (dsForecast.Tables[0].Rows[0]["high_variance"].ToString().Trim() == "1")
                    boolVariance = true;
                oBuilder.Append("Special Requirements:\t\t" + dsForecast.Tables[0].Rows[0]["special"].ToString().Trim() + "" + (dsForecast.Tables[0].Rows[0]["special_variance"].ToString().Trim() == "1" ? " [VARIANCE]" : "") + "\n");
                if (dsForecast.Tables[0].Rows[0]["special_variance"].ToString().Trim() == "1")
                    boolVariance = true;
                oBuilder.Append("Disaster Recovery Requirement:\n" + dsForecast.Tables[0].Rows[0]["dr"].ToString().Trim() + "" + (dsForecast.Tables[0].Rows[0]["dr_variance"].ToString().Trim() == "1" ? " [VARIANCE]" : "") + "\n\n");
                if (dsForecast.Tables[0].Rows[0]["dr_variance"].ToString().Trim() == "1")
                    boolVariance = true;
                oBuilder.Append("Click the following link to view this design...\n");
                oBuilder.Append(oVariable.URL() + "/redirect.aspx?referrer=/frame/forecast_equipment.aspx?id=" + intID.ToString() + "\n");
            }
            return oBuilder.ToString();
        }
        // VIJAY - END
        protected void btnCancel_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path + "?id=" + intID.ToString());
        }
    }
}