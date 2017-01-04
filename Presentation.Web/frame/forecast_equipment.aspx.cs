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
using System.Text;

namespace NCC.ClearView.Presentation.Web
{
    public partial class forecast_equipment : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intForecastPage = Int32.Parse(ConfigurationManager.AppSettings["ForecastEdit"]);
        protected int intViewWorkload = Int32.Parse(ConfigurationManager.AppSettings["ViewWorkload"]);
        protected int intForceOverride = Int32.Parse(ConfigurationManager.AppSettings["ForceForecastOverride"]);
        protected int intConfidenceUnlock = Int32.Parse(ConfigurationManager.AppSettings["CONFIDENCE_UNLOCK"]);
        protected int intPlatformS = Int32.Parse(ConfigurationManager.AppSettings["ServerPlatformID"]);
        protected int intViewPage = Int32.Parse(ConfigurationManager.AppSettings["ViewWorkload"]);
        protected string strIARB = ConfigurationManager.AppSettings["IARB"];
        protected int intIARB = Int32.Parse(ConfigurationManager.AppSettings["IARB_PAGEID"]);
        protected int intProfile;
        protected Forecast oForecast;
        protected Solution oSolution;
        protected Platforms oPlatform;
        protected Pages oPage;
        protected Users oUser;
        protected Functions oFunction;

        protected int intForecast;
        protected int intID = 0;
        protected string strSteps = "";
        private string strEMailIdsBCC = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oForecast = new Forecast(intProfile, dsn);
            oSolution = new Solution(intProfile, dsn);
            oPlatform = new Platforms(intProfile, dsn);
            oPage = new Pages(intProfile, dsn);
            oUser = new Users(intProfile, dsn);
            oFunction = new Functions(intProfile, dsn, intEnvironment);

            if (Request.QueryString["parent"] != null && Request.QueryString["parent"] != "")
                intForecast = Int32.Parse(Request.QueryString["parent"]);
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                intID = Int32.Parse(Request.QueryString["id"]);

            // Check that person has access to design builder
            if (oUser.GetApplicationUrl(intProfile, intForecastPage) == "")
            {
                panDenied.Visible = true;
                hypDesign.NavigateUrl = "/datapoint/service/design.aspx?id=" + oFunction.encryptQueryString(intID.ToString());
            }
            else
            {
                panAllow.Visible = true;
                int intReq = 0;
                int intProj = 0;
                int intForecastTemp = 0;
                if (intID > 0)
                    intForecastTemp = Int32.Parse(oForecast.GetAnswer(intID, "forecastid"));
                else if (intForecast > 0)
                    intForecastTemp = intForecast;
                if (intForecastTemp > 0)
                {
                    intReq = Int32.Parse(oForecast.Get(intForecastTemp, "requestid"));
                    Requests oRequest = new Requests(0, dsn);
                    intProj = oRequest.GetProjectNumber(intReq);
                }
                if (Request.QueryString["saved"] != null)
                    Page.ClientScript.RegisterStartupScript(typeof(Page), "saved", "<script type=\"text/javascript\">CheckRefresh('" + oPage.GetFullLink(intForecastPage) + "?id=" + intForecast.ToString() + "','" + oPage.GetFullLink(intViewWorkload) + "?pid=" + intProj.ToString() + "');<" + "/" + "script>");
                if (Request.QueryString["unlock"] != null)
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "unlocked", "<script type=\"text/javascript\">alert('Invalid Unlock Code\\n\\nPlease contact your ClearView administrator to acquire a valid code');<" + "/" + "script>");

                //        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "saved", "<script type=\"text/javascript\">if(window.opener!=null)window.opener.navigate('" + oPage.GetFullLink(intForecastPage) + "?id=" + intForecast.ToString() + "');window.close();<" + "/" + "script>");
                //            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "saved", "<script type=\"text/javascript\">alert('Forecast Equipment Saved');if(window.opener!=null)window.opener.navigate('" + oPage.GetFullLink(intForecastPage) + "?id=" + intForecast.ToString() + "');window.close();<" + "/" + "script>");
                int intStep = 0;
                int intPlatform = 0;
                int intRequest = 0;
                DataSet ds = new DataSet();
                string strTitle = "";
                if (intID > 0)
                {
                    bool boolHundred = false;
                    bool boolOverride = false;
                    ds = oForecast.GetAnswer(intID);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        intRequest = oForecast.GetRequestID(intID, false);
                        intForecast = Int32.Parse(ds.Tables[0].Rows[0]["forecastid"].ToString());
                        int intConfidence = Int32.Parse(ds.Tables[0].Rows[0]["confidenceid"].ToString());
                        if (intConfidence > 0)
                        {
                            Confidence oConfidence = new Confidence(intProfile, dsn);
                            string strConfidence = oConfidence.Get(intConfidence, "name");
                            if (strConfidence.Contains("100%") == true)
                                boolHundred = true;
                        }
                        intStep = Int32.Parse(ds.Tables[0].Rows[0]["step"].ToString());
                        intPlatform = Int32.Parse(ds.Tables[0].Rows[0]["platformid"].ToString());
                        if (ds.Tables[0].Rows[0]["override"].ToString() == "1" || ds.Tables[0].Rows[0]["override"].ToString() == "-1")
                            boolOverride = true;
                        else if (intPlatform == intPlatformS)
                            oForecast.UpdateAnswerModel(intID, 0);
                        if (Request.QueryString["step"] != null && Request.QueryString["step"] != "")
                        {
                            DataSet dsTemp = oForecast.GetSteps(intPlatform, 1);
                            int _step = Int32.Parse(Request.QueryString["step"]);
                            if (_step <= dsTemp.Tables[0].Rows.Count)
                                intStep = _step;
                        }
                    }
                    if (intRequest != 0)
                        panLocked.Visible = true;

                    int intRow = intStep - 1;
                    DataSet dsSteps = oForecast.GetSteps(intPlatform, 1);
                    DataSet dsStepsDone = oForecast.GetStepsDone(intID, 1);
                    if (Request.QueryString["save"] != null)
                        Page.ClientScript.RegisterStartupScript(typeof(Page), "save", "<script type=\"text/javascript\">CheckRefresh('" + oPage.GetFullLink(intForecastPage) + "?id=" + intForecast.ToString() + "','" + oPage.GetFullLink(intViewWorkload) + "?pid=" + intProj.ToString() + "');<" + "/" + "script>");
                    if (dsSteps.Tables[0].Rows.Count <= intRow && Request.QueryString["step"] == null)
                    {
                        if (Request.QueryString["save"] != null && dsStepsDone.Tables[0].Rows.Count == dsSteps.Tables[0].Rows.Count)
                            Response.Redirect(Request.Path + "?parent=" + intForecast.ToString() + "&id=" + intID.ToString() + "&saved=true");
                        else
                        {
                            LoadSteps(intID, intPlatform);
                            if (boolHundred == true && panLocked.Visible == false)
                            {
                                panHundred.Visible = true;
                                lblConfidenceUnlock.Text = Request.ServerVariables["REMOTE_HOST"];
                            }
                        }
                        strTitle = "Design Summary";
                    }
                    else if (dsSteps.Tables[0].Rows.Count > 0)
                    {
                        panStep.Visible = true;
                        //imgStep.ImageUrl = "/images/num" + intStep.ToString() + ".gif";
                        imgStep.ImageUrl = dsSteps.Tables[0].Rows[intRow]["image_path"].ToString();
                        lblTitle.Text = dsSteps.Tables[0].Rows[intRow]["name"].ToString();
                        strTitle = dsSteps.Tables[0].Rows[intRow]["name"].ToString();
                        lblSubTitle.Text = dsSteps.Tables[0].Rows[intRow]["subtitle"].ToString();
                        string strPath = dsSteps.Tables[0].Rows[intRow]["path"].ToString();
                        if (boolOverride == true)
                        {
                            if (dsSteps.Tables[0].Rows[intRow]["override_path"].ToString() != "")
                                strPath = dsSteps.Tables[0].Rows[intRow]["override_path"].ToString();
                            panOverride.Visible = true;
                        }
                        if (strPath != "")
                            PHStep.Controls.Add((UserControl)LoadControl(strPath));
                    }
                }
                else
                {
                    panStep.Visible = true;
                    imgStep.ImageUrl = "/images/start_design.gif";
                    strTitle = "Start Design";
                    lblTitle.Text = strTitle;
                    lblSubTitle.Text = "Please complete the following information to start your design.";
                    PHStep.Controls.Add((UserControl)LoadControl("/controls/fore/fore_first.ascx"));
                }
                Master.Page.Title = strTitle;
                btnConfidenceUnlock.Attributes.Add("onclick", "return ValidateText('" + txtConfidenceUnlock.ClientID + "','Please enter an unlock code') && ValidateText('" + txtConfidenceReason.ClientID + "','Please enter a reason for unlocking this design');");
            }
            btnClose.Attributes.Add("onclick", "return window.close();");
        }
        private void LoadSteps(int _id, int _platformid)
        {
            panSteps.Visible = true;
            // Reset the additional information screen.
            oForecast.DeleteStepAdditionalDone(_id);
            DataSet ds = oForecast.GetSteps(_platformid, 1);
            DataSet dsStepsDone = oForecast.GetStepsDone(_id, 0);
            int intCount = 0;
            int intStepCount = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                intStepCount++;
                bool boolAdd = true;
                if (dr["additional"].ToString() == "1")
                {
                    boolAdd = false;
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
                                    boolAdd = true;
                            }
                        }
                    }
                }
                if (boolAdd == true)
                {
                    StringBuilder sb = new StringBuilder(strSteps);
                    intCount++;

                    sb.Append("<table width=\"100%\" cellpadding=\"0\" cellspacing=\"5\" border=\"0\">");
                    sb.Append("<tr>");
                    
                    string strURL = "<a href=\"" + Request.Path + "?id=" + _id.ToString() + "&step=" + intStepCount.ToString() + "\">";
                    string strImage = "<img src=\"/images/ico_hourglass40.gif\" border=\"0\" align=\"absmiddle\" />";

                    if (dsStepsDone.Tables[0].Rows.Count > 0 && dsStepsDone.Tables[0].Rows[intCount - 1]["done"].ToString() == "1")
                    {
                        strImage = "<img src=\"/images/ico_check40.gif\" border=\"0\" align=\"absmiddle\" />";
                    }

                    sb.Append("<td rowspan=\"2\">");
                    sb.Append(strURL);
                    sb.Append(strImage);
                    sb.Append("</a></td>");
                    sb.Append("<td class=\"bold\" width=\"100%\" valign=\"bottom\">");
                    sb.Append(strURL);
                    sb.Append("Step ");
                    sb.Append(intCount.ToString());
                    sb.Append(": ");
                    sb.Append(dr["name"].ToString());
                    sb.Append("</a></td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td width=\"100%\" valign=\"top\">");
                    sb.Append(dr["subtitle"].ToString());
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    sb.Append("</table>");
                    sb.Append("<br/>");

                    strSteps = sb.ToString();
                }
            }
        }
        protected void btnConfidenceUnlock_Click(Object Sender, EventArgs e)
        {
            Encryption oEncryption = new Encryption();

            string strEMailIdsBCC = oFunction.GetGetEmailAlertsEmailIds("EMAILGRP_DESIGN_BUILDER");

            string strDate = DateTime.Today.Month.ToString() + DateTime.Today.Day.ToString() + DateTime.Today.Year.ToString();
            if (txtConfidenceUnlock.Text == oEncryption.Encrypt(lblConfidenceUnlock.Text + strDate, "UNLOCK_100"))
            {
                if (oForecast.GetAnswer(intID, "override") == "-1")
                {
                    // Send Email to iARB stating that they do not have to approve the design
                    string[] strEmail;
                    char[] strSplit = { ';' };
                    strEmail = strIARB.Split(strSplit);
                    for (int ii = 0; ii < strEmail.Length; ii++)
                    {
                        if (strEmail[ii].Trim() != "")
                        {
                            string strAddress = strEmail[ii];
                            oFunction.SendEmail("Review Board Approval UNLOCKED", strAddress, "", strEMailIdsBCC, "Review Board Approval UNLOCKED", "<p><b>An overridden design has been unlocked and no longer requires your approval.</b><p><p>" + oForecast.GetAnswerBody(intID, intEnvironment, dsnAsset, dsnIP) + "</p>", true, false);
                        }
                    }
                }
                oForecast.AddAnswerUnlock(intID, intProfile, txtConfidenceReason.Text);
                oForecast.UpdateAnswerConfidence(intID, intConfidenceUnlock);
                int intForecast = Int32.Parse(oForecast.GetAnswer(intID, "forecastid"));
                int intRequest = Int32.Parse(oForecast.Get(intForecast, "requestid"));
                Requests oRequest = new Requests(0, dsn);
                int intProject = oRequest.GetProjectNumber(intRequest);
                OnDemandTasks oOnDemandTasks = new OnDemandTasks(0, dsn);
                ResourceRequest oResourceRequest = new ResourceRequest(0, dsn);
                Variables oVariable = new Variables(intEnvironment);
                Projects oProject = new Projects(0, dsn);
                DataSet ds = oOnDemandTasks.GetPending(intID);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    int intResourceWorkflow = Int32.Parse(dr["resourceid"].ToString());
                    int intResourceParent = oResourceRequest.GetWorkflowParent(intResourceWorkflow);
                    int intImplementor = Int32.Parse(oResourceRequest.GetWorkflow(intResourceWorkflow, "userid"));
                    oResourceRequest.UpdateItemAndService(intResourceParent, -1, -1);
                    oResourceRequest.UpdateName(intResourceParent, "Provisioning Task [Unlocked]");
                    string strDefault = oUser.GetApplicationUrl(intImplementor, intViewPage);
                    string strSpacerRow = "<tr><td colspan=\"3\"><img src=\"" + oVariable.ImageURL() + "/images/spacer.gif\" border=\"0\" width=\"1\" height=\"7\" /></td></tr>";
                    string strCC = "";
                    Delegates oDelegate = new Delegates(0, dsn);
                    DataSet dsDelegates = oDelegate.Gets(intImplementor);
                    foreach (DataRow drDelegate in dsDelegates.Tables[0].Rows)
                        strCC += oUser.GetName(Int32.Parse(drDelegate["delegate"].ToString())) + ";";
                    if (strDefault == "")
                        oFunction.SendEmail("ClearView Provisioning Task Unlocked", oUser.GetName(intImplementor), strCC, strEMailIdsBCC, "ClearView Provisioning Task Unlocked", "<p><b>A provisioning request has been unlocked. This request no longer requires you to take action.</b><p><p>" + oProject.GetBody(intProject, intEnvironment, false) + "</p>", true, false);
                    else
                        oFunction.SendEmail("ClearView Provisioning Task Unlocked", oUser.GetName(intImplementor), strCC, strEMailIdsBCC, "ClearView Provisioning Task Unlocked", "<p><b>A provisioning request has been unlocked. This request no longer requires you to take action.</b><p><p>" + oProject.GetBody(intProject, intEnvironment, false) + "</p><p><a href=\"" + oVariable.URL() + "/redirect.aspx?referrer=/" + strDefault + oPage.GetFullLink(intViewPage) + "?pid=" + intProject.ToString() + "\" target=\"_blank\">Click here to review this project.</a></p>", true, false);
                }
                oOnDemandTasks.DeleteAll(intID);
                Response.Redirect(Request.Path + "?id=" + intID.ToString());
            }
            else
                Response.Redirect(Request.Path + "?id=" + intID.ToString() + "&unlock=true");
        }
    }
}
