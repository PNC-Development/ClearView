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
    public partial class fore_server : System.Web.UI.UserControl
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);

        protected int intForecastPage = Int32.Parse(ConfigurationManager.AppSettings["ForecastEdit"]);
        protected int intImplementorDistributed = Int32.Parse(ConfigurationManager.AppSettings["ITEMID_IMPLEMENTOR_DISTRIBUTED"]);
        protected int intImplementorMidrange = Int32.Parse(ConfigurationManager.AppSettings["ITEMID_IMPLEMENTOR_MIDRANGE"]);
        protected int intDRHourQuestion = Int32.Parse(ConfigurationManager.AppSettings["DR_HOUR_QUESTION"]);
        protected int intDRHourResponse = Int32.Parse(ConfigurationManager.AppSettings["DR_HOUR_RESPONSE"]);
        protected int intDRRecoveryQuestion = Int32.Parse(ConfigurationManager.AppSettings["DR_RECOVERY_QUESTION"]);
        protected int intDRRecoveryResponse = Int32.Parse(ConfigurationManager.AppSettings["DR_RECOVERY_RESPONSE"]);
        protected int intWorkstationPlatform = Int32.Parse(ConfigurationManager.AppSettings["WorkstationPlatformID"]);
        protected int intProfile;
        protected Forecast oForecast;
        protected Solution oSolution;
        protected Platforms oPlatform;
        protected Classes oClass;
        protected int intForecast;
        protected int intID = 0;
        protected string strQuestions;
        protected string strAttributes = "";
        protected string strHidden = "";
        protected bool boolShowNumber = false;
        protected bool boolProduction = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oForecast = new Forecast(intProfile, dsn);
            oSolution = new Solution(intProfile, dsn);
            oPlatform = new Platforms(intProfile, dsn);
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
                    int intClass = Int32.Parse(ds.Tables[0].Rows[0]["classid"].ToString());
                    if (oClass.IsProd(intClass))
                        boolProduction = true;
                    int intEnvir = Int32.Parse(ds.Tables[0].Rows[0]["environmentid"].ToString());
                    int intStep = Int32.Parse(ds.Tables[0].Rows[0]["step"].ToString());
                    intForecast = Int32.Parse(ds.Tables[0].Rows[0]["forecastid"].ToString());
                    lblRecovery.Text = ds.Tables[0].Rows[0]["recovery_number"].ToString();
                    lblQuantity.Text = ds.Tables[0].Rows[0]["quantity"].ToString();
                    DataSet dsSteps = oForecast.GetSteps(intPlatform, 1);
                    if (dsSteps.Tables[0].Rows.Count == intStep)
                        btnNext.Text = "Finish";
                    if (intStep == 0 || intStep == 1)
                        btnBack.Enabled = false;
                    if (!IsPostBack)
                        LoadPlatform(intPlatform, intClass, intEnvir);
                }
            }
            
            Control ctrl = Page.ParseControl(strQuestions + strHidden);
            tdQuestions.Controls.Add(ctrl);

            btnNext.Attributes.Add("onclick", "return " + strAttributes);
            btnUpdate.Attributes.Add("onclick", "return " + strAttributes);
            btnClose.Attributes.Add("onclick", "return window.close();");

        }

        private void LoadPlatform(int _platformid, int _classid, int _environmentid)
        {
            StringBuilder sb = new StringBuilder();
            lblPlatform.Text = _platformid.ToString();
            strQuestions = "";
            string strNext = "";
            int intCount = 0;
            int intCount2 = 0;
            DataSet ds = oForecast.GetQuestionPlatform(_platformid, _classid, _environmentid);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                int intQuestion = Int32.Parse(dr["id"].ToString());
                string strDisplay = GetDisplay(intQuestion);
                int intType = Int32.Parse(dr["type"].ToString());
                DataSet dsResponses = oForecast.GetResponsesNoCustom(intQuestion, 1);
                DataSet dsAnswers = oForecast.GetAnswerPlatform(intID, intQuestion);
                intCount++;
                sb.Append(strNext);
                strNext = "";
                sb.Append("<tr id=\"divQ_");
                sb.Append(intQuestion.ToString());
                sb.Append("\" style=\"display:");
                sb.Append(strDisplay);
                sb.Append("\">");
                sb.Append("<td>");
                sb.Append("<table cellpadding=\"2\" cellspacing=\"1\" border=\"0\">");
                sb.Append("<tr>");

                if (boolShowNumber == true)
                {
                    sb.Append("<td valign=\"top\">");
                    sb.Append(intCount.ToString());
                    sb.Append(".)</td>");
                }

                sb.Append("<td valign=\"top\" colspan=\"2\">");
                //sb.Append(dr["question"].ToString());
                //sb.Append(dr["required"].ToString() == "1" ? "<font class=\"required\">&nbsp;*</font>" : "");
                sb.Append("  <asp:Label ID=\"lblQuestion" + intQuestion.ToString());
                sb.Append("\" runat=\"server\" CssClass=\"default\" Text =\"" + dr["question"].ToString());
                sb.Append(dr["required"].ToString() == "1" ? "<font class='required'>&nbsp;*</font>" : "");
                sb.Append("\" />");

                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");

                if (boolShowNumber == true)
                {
                    sb.Append("<td>&nbsp;</td>");
                }

                sb.Append("<td>");
                string strAnswer = "";
                string strCustom = "";
                int intResponse = 0;
                bool boolVariance = false;

                switch (intType)
                {
                    case 1:
                        // Drop Down List
                        sb.Append("<select id=\"divA_");
                        sb.Append(intQuestion.ToString());
                        sb.Append("\" class=\"default\" onchange=\"UpdateDDL('");
                        sb.Append(intQuestion.ToString());
                        sb.Append("');\">");
                        sb.Append("<option value=\"0\">-- SELECT --</option>");

                        foreach (DataRow drResponse in dsResponses.Tables[0].Rows)
                        {
                            intResponse = Int32.Parse(drResponse["id"].ToString());
                            bool boolValue = oForecast.GetAnswerPlatform(intID, intQuestion, intResponse);
                            if (boolValue == true)
                            {
                                strAnswer = drResponse["id"].ToString();
                                boolVariance = (drResponse["variance"].ToString() == "1");
                            }
                            if (drResponse["custom"].ToString() != "0")
                            {
                                string strCustomDisplay = "none";
                                if (boolValue == true)
                                {
                                    strCustomDisplay = "inline";
                                    strCustom = oForecast.GetAnswerPlatformCustom(intID, intQuestion, intResponse);
                                    strHidden += "<input type=\"hidden\" name=\"hdnC_" + intResponse.ToString() + "\" id=\"hdnC_" + intResponse.ToString() + "\" value=\"" + strCustom + "\" />";
                                }
                                else
                                    strHidden += "<input type=\"hidden\" name=\"hdnC_" + intResponse.ToString() + "\" id=\"hdnC_" + intResponse.ToString() + "\" value=\"\" />";
                                strNext += AddCustom(intResponse, dr["required"].ToString(), strCustomDisplay, (boolValue ? strCustom : ""), intQuestion.ToString(), "0", "divA_", intResponse.ToString());
                            }
                            sb.Append("<option value=\"");
                            sb.Append(intResponse.ToString());
                            sb.Append("\"");
                            sb.Append(boolValue ? " SELECTED" : "");
                            sb.Append(">");
                            sb.Append(drResponse["response"].ToString());
                            sb.Append("</option>");
                        }
                        strHidden += "<input type=\"hidden\" name=\"hdnQ_" + intQuestion.ToString() + "\" id=\"hdnQ_" + intQuestion.ToString() + "\" value=\"" + strAnswer + "\" />";
                        sb.Append("</select>");
                        break;
                    case 2:
                        // Radio List
                        sb.Append("<table cellpadding=\"1\" cellspacing=\"1\" border=\"0\" class=\"default\">");
                        intCount2 = 0;
                        foreach (DataRow drResponse in dsResponses.Tables[0].Rows)
                        {
                            intResponse = Int32.Parse(drResponse["id"].ToString());
                            intCount2++;
                            bool boolValue = oForecast.GetAnswerPlatform(intID, intQuestion, intResponse);
                            if (boolValue == true)
                            {
                                strAnswer = intResponse.ToString();
                                boolVariance = (drResponse["variance"].ToString() == "1");
                            }
                            if (drResponse["custom"].ToString() != "0")
                            {
                                string strCustomDisplay = "none";
                                if (boolValue == true)
                                {
                                    strCustomDisplay = "inline";
                                    strCustom = oForecast.GetAnswerPlatformCustom(intID, intQuestion, intResponse);
                                    strHidden += "<input type=\"hidden\" name=\"hdnC_" + intResponse.ToString() + "\" id=\"hdnC_" + intResponse.ToString() + "\" value=\"" + strCustom + "\" />";
                                }
                                else
                                    strHidden += "<input type=\"hidden\" name=\"hdnC_" + intResponse.ToString() + "\" id=\"hdnC_" + intResponse.ToString() + "\" value=\"\" />";
                                strNext += AddCustom(intResponse, dr["required"].ToString(), strCustomDisplay, (boolValue ? strCustom : ""), intQuestion.ToString(), intCount2.ToString(), "radQ_", "");
                            }
                            sb.Append("<tr><td><input id=\"radQ_");
                            sb.Append(intQuestion.ToString());
                            sb.Append("_");
                            sb.Append(intCount2.ToString());
                            sb.Append("\" type=\"radio\" name=\"");
                            sb.Append(intQuestion.ToString());
                            sb.Append("\" value=\"");
                            sb.Append(intResponse.ToString());
                            sb.Append("\" onclick=\"UpdateQuestion('");
                            sb.Append(intQuestion.ToString());
                            sb.Append("','");
                            sb.Append(intResponse.ToString());
                            sb.Append("');\"");
                            sb.Append(boolValue ? " checked" : "");
                            sb.Append(" /><label for=\"radQ_");
                            sb.Append(intQuestion.ToString());
                            sb.Append("_");
                            sb.Append(intCount2.ToString());
                            sb.Append("\">");
                            sb.Append(drResponse["response"].ToString());
                            sb.Append("</label></td></tr>");
                        }
                        strHidden += "<input type=\"hidden\" name=\"hdnQ_" + intQuestion.ToString() + "\" id=\"hdnQ_" + intQuestion.ToString() + "\" value=\"" + strAnswer + "\" />";
                        sb.Append("</table>");
                        break;
                    case 3:
                        // Check Box List
                        sb.Append("<table cellpadding=\"1\" cellspacing=\"1\" border=\"0\" class=\"default\">");
                        intCount2 = 0;
                        foreach (DataRow drResponse in dsResponses.Tables[0].Rows)
                        {
                            intResponse = Int32.Parse(drResponse["id"].ToString());
                            intCount2++;
                            bool boolValue = oForecast.GetAnswerPlatform(intID, intQuestion, Int32.Parse(drResponse["id"].ToString()));
                            string strResponse = intResponse + "|";
                            if (boolValue == true)
                            {
                                strAnswer += intResponse.ToString() + "|";
                                if (boolVariance == false)
                                    boolVariance = (drResponse["variance"].ToString() == "1");
                            }
                            if (drResponse["custom"].ToString() != "0")
                            {
                                string strCustomDisplay = "none";
                                if (boolValue == true)
                                {
                                    strCustomDisplay = "inline";
                                    strCustom = oForecast.GetAnswerPlatformCustom(intID, intQuestion, intResponse);
                                    strHidden += "<input type=\"hidden\" name=\"hdnC_" + intResponse.ToString() + "\" id=\"hdnC_" + intResponse.ToString() + "\" value=\"" + strCustom + "\" />";
                                }
                                else
                                    strHidden += "<input type=\"hidden\" name=\"hdnC_" + intResponse.ToString() + "\" id=\"hdnC_" + intResponse.ToString() + "\" value=\"\" />";
                                strNext += AddCustom(intResponse, dr["required"].ToString(), strCustomDisplay, (boolValue ? strCustom : ""), intQuestion.ToString(), intCount2.ToString(), "chkQ_", "");
                            }
                            sb.Append("<tr><td><input id=\"chkQ_");
                            sb.Append(intQuestion.ToString());
                            sb.Append("_");
                            sb.Append(intCount2.ToString());
                            sb.Append("\" type=\"checkbox\" name=\"chkQ_");
                            sb.Append(intQuestion.ToString());
                            sb.Append("_");
                            sb.Append(intCount2.ToString());
                            sb.Append("\" value=\"");
                            sb.Append(intResponse.ToString());
                            sb.Append("\" onclick=\"UpdateCheck('hdnQ_");
                            sb.Append(intQuestion.ToString());
                            sb.Append("', this, '");
                            sb.Append(strResponse);
                            sb.Append("','');\"");
                            sb.Append(boolValue ? " checked" : "");
                            sb.Append(" /><label for=\"chkQ_");
                            sb.Append(intQuestion.ToString());
                            sb.Append("_");
                            sb.Append(intCount2.ToString());
                            sb.Append("\">");
                            sb.Append(drResponse["response"].ToString());
                            sb.Append("</label></td></tr>");
                        }
                        strHidden += "<input type=\"hidden\" name=\"hdnQ_" + intQuestion.ToString() + "\" id=\"hdnQ_" + intQuestion.ToString() + "\" value=\"" + strAnswer + "\" />";
                        sb.Append("</table>");
                        break;
                }
                sb.Append("</td>");
                sb.Append("<td id=\"divV_");
                sb.Append(intQuestion.ToString());
                sb.Append("\" style=\"display:");
                sb.Append(boolVariance ? "inline" : "none");
                sb.Append("\"><img src=\"/images/required.gif\" border=\"0\" align=\"absmiddle\"/> <span class=\"reddefault\">Variance Recommended</span></td>");
                sb.Append("</tr>");
                sb.Append("</table>");
                sb.Append("</td>");
                sb.Append("</tr>");
            }

            strQuestions = sb.ToString();

            if (strQuestions != "")
            {
                strQuestions = "<table cellpadding=\"5\" cellspacing=\"2\" border=\"0\">" + strQuestions + "</table>";
            }
        }
        private string AddCustom(int _custom, string _required, string _display, string _text, string _question, string _count, string _preface, string _value)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<tr id=\"divC_");
            sb.Append(_custom.ToString());
            sb.Append("\" style=\"display:");
            sb.Append(_display);
            sb.Append("\">");
            sb.Append("<td>");
            sb.Append("<table cellpadding=\"2\" cellspacing=\"1\" border=\"0\">");
            sb.Append("<tr>");
            sb.Append("<td valign=\"top\">");
            sb.Append(oForecast.GetResponse(Int32.Parse(oForecast.GetResponse(_custom, "custom")), "response"));
            sb.Append("<font class=\"required\">&nbsp;*</font>");
            sb.Append("</td>");
            sb.Append("</tr>");
            sb.Append("<tr>");
            sb.Append("<td>");
            sb.Append("<input type=\"text\" id=\"txt_");
            sb.Append(_question);
            sb.Append("_");
            sb.Append(_count);
            sb.Append("\" onblur=\"UpdateCustomValue(this, 'hdnC_");
            sb.Append(_custom.ToString());
            sb.Append("');\" class=\"default\" style=\"width:200px\" value=\"");
            sb.Append(_text);
            sb.Append("\" />");
            sb.Append("</td>");
            sb.Append("</tr>");
            sb.Append("</table>");
            sb.Append("</td>");
            sb.Append("</tr>");

            if (strAttributes != "")
            {
                strAttributes += " && ";
            }
            if (_count == "0")
            {
                strAttributes += "EnsureTextDDL('txt_" + _question + "_" + _count + "','" + _preface + _question + "','" + _value + "','Please enter a value')";
            }
            else
            {
                strAttributes += "EnsureTextRadio('txt_" + _question + "_" + _count + "','" + _preface + _question + "_" + _count + "','Please enter a value')";
            }

            return sb.ToString();
        }
        private string GetDisplay(int _questionid)
        {
            string strDisplay = "";
            string strInitialDisplay = "";
            DataSet dsAffects = oForecast.GetAffectsByAffected(_questionid);
            if (dsAffects.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow drAffect in dsAffects.Tables[0].Rows)
                {
                    strInitialDisplay = (drAffect["state"].ToString() == "1" ? "inline" : "none");
                    int intQuestion = Int32.Parse(drAffect["questionid"].ToString());
                    DataSet dsAnswer = oForecast.GetAnswerPlatform(intID, intQuestion);
                    bool boolChanged = false;
                    foreach (DataRow drAnswer in dsAnswer.Tables[0].Rows)
                    {
                        string strTemp = oForecast.GetAffects(intQuestion, _questionid, Int32.Parse(drAnswer["responseid"].ToString()));
                        if (strTemp != "")
                            strDisplay = (strTemp == "1" ? "inline" : "none");
                        if (strDisplay == "inline" && strInitialDisplay == "none")
                        {
                            strInitialDisplay = strDisplay;
                            boolChanged = true;
                            break;
                        }
                    }
                    if (boolChanged == true)
                        break;
                }
            }
            if (strInitialDisplay == "")
                strInitialDisplay = "inline";
            return strInitialDisplay;
        }

        protected int Save()
        {
            int intDone = 1;
            oForecast.DeleteAnswerPlatform(intID);
            foreach (string strForm in Request.Form)
            {
                if (strForm.StartsWith("hdnQ_"))
                {
                    if (intID == 0)
                    {
                        int intPlatform = Int32.Parse(lblPlatform.Text);
                        intID = oForecast.AddAnswer(intForecast, intPlatform, 0, intProfile);
                    }
                    int intQuestion = Int32.Parse(strForm.Substring(5));
                    string strInitalDisplay = GetDisplay(intQuestion);
                    if (strInitalDisplay == "inline")
                    {
                        string strTemp = Request.Form[strForm];
                        int intAnswer = 0;
                        int intCustom = 0;
                        if (strTemp.Contains("|"))
                        {
                            while (strTemp != "")
                            {
                                if (strTemp.Contains("_"))
                                    intAnswer = Int32.Parse(strTemp.Substring(0, strTemp.IndexOf("_")));
                                else
                                    intAnswer = Int32.Parse(strTemp.Substring(0, strTemp.IndexOf("|")));
                                strTemp = strTemp.Substring(strTemp.IndexOf("|") + 1);
                                intCustom = Int32.Parse(oForecast.GetResponse(intAnswer, "custom"));
                                if (intCustom > 0)
                                    oForecast.AddAnswerPlatform(intID, intQuestion, intAnswer, Request.Form["hdnC_" + intAnswer.ToString()]);
                                else
                                    oForecast.AddAnswerPlatform(intID, intQuestion, intAnswer, "");
                            }
                        }
                        else if (strTemp != "" && strTemp != "0")
                        {
                            intAnswer = Int32.Parse(strTemp);
                            intCustom = Int32.Parse(oForecast.GetResponse(intAnswer, "custom"));
                            if (intCustom > 0)
                                oForecast.AddAnswerPlatform(intID, intQuestion, intAnswer, Request.Form["hdnC_" + intAnswer.ToString()]);
                            else
                                oForecast.AddAnswerPlatform(intID, intQuestion, intAnswer, "");
                        }
                        else
                        {
                            if (oForecast.GetQuestion(intQuestion, "required") == "1")
                            {
                                DataSet dsAffects = oForecast.GetAffectsByAffected(intQuestion);
                                foreach (DataRow drAffect in dsAffects.Tables[0].Rows)
                                {
                                    int intQ = Int32.Parse(drAffect["questionid"].ToString());
                                    DataSet dsAnswer = oForecast.GetAnswerPlatform(intID, intQ);
                                    bool boolOK = false;
                                    foreach (DataRow drAnswer in dsAnswer.Tables[0].Rows)
                                    {
                                        string strDisplay = oForecast.GetAffects(intQ, intQuestion, Int32.Parse(drAnswer["responseid"].ToString()));
                                        if (strDisplay != "")
                                            strDisplay = (strDisplay == "1" ? "inline" : "none");
                                        if (strDisplay == "none")
                                            boolOK = true;
                                    }
                                    if (boolOK == false)
                                        intDone = 0;
                                }
                            }
                        }
                    }
                }
            }
            bool boolRecoveryOne = oForecast.IsDROneToOne(intID);
            bool boolRecoveryMany = oForecast.IsDRManyToOne(intID);
            bool boolRecoveryNone = oForecast.IsDROver48(intID, false);
            int intRecovery = (boolRecoveryNone ? 0 : (boolRecoveryOne ? Int32.Parse(lblQuantity.Text) : (boolRecoveryMany ? Int32.Parse(lblRecovery.Text) : 0)));
            oForecast.UpdateAnswerRecovery(intID, intRecovery);

            if (boolProduction == true)
                oForecast.EnforceRecovery(intID, intDRHourQuestion, intDRHourResponse, intDRRecoveryQuestion, intDRRecoveryResponse, Int32.Parse(lblQuantity.Text));

            return intDone;
        }

        protected void btnBack_Click(Object Sender, EventArgs e)
        {
            oForecast.UpdateAnswerStep(intID, -1, -1);
            Response.Redirect(Request.Path + "?id=" + intID.ToString());
        }

        protected void btnNext_Click(Object Sender, EventArgs e)
        {
            int intDone = Save();
            OnDemandTasks oOnDemandTasks = new OnDemandTasks(0, dsn);
            if (intDone == 1)
            {
                if (oForecast.CanAutoProvision(intID) == false && oOnDemandTasks.GetPending(intID).Tables[0].Rows.Count == 0)
                {
                    // Add Resource Request
                    int intForecast = Int32.Parse(oForecast.GetAnswer(intID, "forecastid"));
                    int intRequest = Int32.Parse(oForecast.Get(intForecast, "requestid"));
                    Requests oRequest = new Requests(0, dsn);
                    int intProject = oRequest.GetProjectNumber(intRequest);
                    ResourceRequest oResourceRequest = new ResourceRequest(0, dsn);
                    int intImplementor = 0;
                    string strType = "Distributed";
                    DataSet dsResource = oResourceRequest.GetProjectItem(intProject, intImplementorDistributed);
                    if (oForecast.GetPlatformDistributed(intID, intWorkstationPlatform) == false)
                    {
                        dsResource = oResourceRequest.GetProjectItem(intProject, intImplementorMidrange);
                        strType = "Midrange";
                    }
                    if (dsResource.Tables[0].Rows.Count > 0)
                        intImplementor = (dsResource.Tables[0].Rows[0]["userid"].ToString() == "" ? 0 : Int32.Parse(dsResource.Tables[0].Rows[0]["userid"].ToString()));
                    if (intImplementor > 0)
                    {
                        int intNextNumber = oResourceRequest.GetNumber(intRequest);
                        int intResourceParent = oResourceRequest.Add(intRequest, -1, -1, intNextNumber, "Provisioning Task (" + strType + ")", 0, 6.00, 2, 1, 1, 1);
                        int intResourceWorkflow = oResourceRequest.AddWorkflow(intResourceParent, 0, "Provisioning Task (" + strType + ")", intImplementor, 0, 6.00, 2, 0);
                        oOnDemandTasks.AddPending(intID, intResourceWorkflow);
                        oResourceRequest.UpdateAssignedBy(intResourceParent, -999);
                    }
                }
            }
            oForecast.UpdateAnswerStep(intID, 1, intDone);
            Response.Redirect(Request.Path + "?id=" + intID.ToString() + "&save=true");
        }

        protected void btnUpdate_Click(Object Sender, EventArgs e)
        {
            int intDone = Save();
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