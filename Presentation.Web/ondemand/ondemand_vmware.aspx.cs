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
using System.IO;

namespace NCC.ClearView.Presentation.Web
{
    public partial class ondemand_vmware : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected string strRefresh = "15";
        protected string strResult = "";
        protected string strName = "";
        protected string strPreviewName = "";
        protected string strPreviewToken = "";
        protected int intShowBuild = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            OnDemand oOnDemand = new OnDemand(0, dsn);
            Classes oClass = new Classes(0, dsn);
            Forecast oForecast = new Forecast(0, dsn);
            Servers oServer = new Servers(0, dsn);
            Models oModel = new Models(0, dsn);
            Users oUser = new Users(0, dsn);
            ModelsProperties oModelsProperties = new ModelsProperties(0, dsn);
            Functions oFunction = new Functions(0, dsn, intEnvironment);
            Asset oAsset = new Asset(0, dsnAsset);
            Tokens oToken = new Tokens(0, dsn);
            string strOnLoad = "";
            string strError = "";

            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
            {
                int intServer = Int32.Parse(oFunction.decryptQueryString(Request.QueryString["id"]));
                DataSet ds = oServer.Get(intServer);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    int intAnswer = Int32.Parse(ds.Tables[0].Rows[0]["answerid"].ToString());
                    int intClass = Int32.Parse(oForecast.GetAnswer(intAnswer, "classid"));
                    int intAsset = 0;
                    if (ds.Tables[0].Rows[0]["assetid"].ToString() != "")
                        intAsset = Int32.Parse(ds.Tables[0].Rows[0]["assetid"].ToString());

                    string strSerial = oAsset.Get(intAsset, "serial");
                    strName = oServer.GetName(intServer, true);
                    if (strName == "")
                    {
                        if (intAsset > 0 && oAsset.Get(intAsset, "name") != "")
                            strName = oAsset.Get(intAsset, "name");
                        else
                            strName = "Device " + Request.QueryString["c"];
                    }
                    int intModel = oForecast.GetModelAsset(intAnswer);
                    if (intModel == 0)
                        intModel = oForecast.GetModel(intAnswer);
                    if (oModelsProperties.IsInventory(intModel) == true)
                    {
                        panInventoryYes.Visible = true;
                        intModel = Int32.Parse(oModelsProperties.Get(intModel, "modelid"));
                        int intType = oModel.GetType(intModel);
                        int intCurrent = Int32.Parse(ds.Tables[0].Rows[0]["step"].ToString());
                        int intCurrentWithError = oServer.GetStep(intServer);
                        strPreviewName = strName;
                        DataSet dsSteps = oOnDemand.GetSteps(intType, 1);
                        if (dsSteps.Tables[0].Rows.Count >= intCurrent)
                        {
                            int intNewStep = Int32.Parse(dsSteps.Tables[0].Rows[intCurrent - 1]["id"].ToString());
                            DataSet dsStep = oOnDemand.GetStep(intNewStep);
                            if (dsStep.Tables[0].Rows.Count > 0)
                            {
                                string strPath = dsStep.Tables[0].Rows[0]["path"].ToString();
                                if (dsStep.Tables[0].Rows[0]["show_build"].ToString() == "1")
                                {
                                    chkPreview.Disabled = false;
                                    if (!IsPostBack && Request.QueryString["preview"] != null)
                                    {
                                        intShowBuild = 1;
                                        chkPreview.Checked = true;
                                        // Token
                                        strPreviewToken = oToken.Add(strPreviewName, 50);
                                    }
                                }
                                else
                                    chkPreview.Disabled = true;
                                if (strPath != "")
                                {
                                    Control oControl = (Control)LoadControl(strPath);
                                    PH.Controls.Add(oControl);
                                    strOnLoad = "redirectWait();";
                                }
                                else
                                    strOnLoad = "redirectAJAX('" + intServer.ToString() + "','" + intCurrentWithError.ToString() + "');";
                            }
                        }
                        else
                        {
                            btnDesign.Visible = true;
                            btnDesign.Attributes.Add("onclick", "return OpenWindow('FORECAST_EQUIPMENT','?id=" + intAnswer.ToString() + "');");
                            panDone.Visible = true;
                            if (ds.Tables[0].Rows[0]["completed"].ToString() != "")
                                lblCompleted.Text = DateTime.Parse(ds.Tables[0].Rows[0]["completed"].ToString()).ToString();
                            else
                                lblCompleted.Text = DateTime.Now.ToString();
                        }
                        int intStep = 0;
                        StringBuilder sbStep = new StringBuilder();
                        bool boolError = false;
                        foreach (DataRow drStep in dsSteps.Tables[0].Rows)
                        {
                            intStep++;
                            string strClass = "cantclose";
                            if (drStep["type"].ToString() == "1")
                                strClass = "canclose";
                            if (drStep["type"].ToString() == "-1")
                                strClass = "default";
                            DataSet dsResult = oOnDemand.GetStepDoneServer(intServer, intStep);
                            if (intStep < intCurrent)
                            {
                                string strImage = "<img src=\"/images/check.gif\" border=\"0\" align=\"absmiddle\">";
                                string strDone = "";
                                string strMessage = "";
                                if (dsResult.Tables[0].Rows.Count > 0)
                                {
                                    foreach (DataRow drResult in dsResult.Tables[0].Rows)
                                        strMessage += drResult["result"].ToString();
                                    if (dsResult.Tables[0].Rows[0]["error"].ToString() == "1")
                                        strImage = "<img src=\"/images/error.gif\" border=\"0\" align=\"absmiddle\">";
                                    if (dsResult.Tables[0].Rows[0]["finished"].ToString() != "")
                                        strDone = "&nbsp;&nbsp;(" + DateTime.Parse(dsResult.Tables[0].Rows[0]["finished"].ToString()).ToString() + ")";
                                }
                                sbStep.Append("<tr><td>");
                                sbStep.Append(strImage);
                                sbStep.Append("</td><td nowrap><a href=\"javascript:void(0);\" onclick=\"ShowHideResult('divResult_");
                                sbStep.Append(intAnswer.ToString());
                                sbStep.Append("_");
                                sbStep.Append(intStep.ToString());
                                sbStep.Append("');\">");
                                sbStep.Append(drStep["title"].ToString());
                                sbStep.Append("</a>");
                                sbStep.Append(strDone);
                                sbStep.Append("</td></tr>");
                                sbStep.Append("<tr id=\"divResult_");
                                sbStep.Append(intAnswer.ToString());
                                sbStep.Append("_");
                                sbStep.Append(intStep.ToString());
                                sbStep.Append("\" style=\"display:none\"><td></td><td>");
                                sbStep.Append(drStep["zeus"].ToString() == "1" && strSerial != "" ? " [<a href=\"javascript:void(0);\" onclick=\"OpenWindow('ZEUS_STATUS','" + strSerial + "');\">View Status</a>]" : (strMessage == "" ? "No information" : strMessage));
                                sbStep.Append("</td></tr>");
                            }
                            else if (intStep == intCurrent)
                            {
                                strError = "";
                                if (dsResult.Tables[0].Rows.Count > 0)
                                {
                                    if (dsResult.Tables[0].Rows[0]["error"].ToString() == "1")
                                    {
                                        boolError = true;
                                        sbStep.Append("<tr><td><img src=\"/images/error.gif\" border=\"0\" align=\"absmiddle\"></td><td class=\"");
                                        sbStep.Append(strClass);
                                        sbStep.Append("\" nowrap><a href=\"javascript:void(0);\" onclick=\"ShowHideResult('divResult_");
                                        sbStep.Append(intAnswer.ToString());
                                        sbStep.Append("_");
                                        sbStep.Append(intStep.ToString());
                                        sbStep.Append("');\">");
                                        sbStep.Append(drStep["title"].ToString());
                                        sbStep.Append("</a></td></tr>");
                                        sbStep.Append("<tr id=\"divResult_");
                                        sbStep.Append(intAnswer.ToString());
                                        sbStep.Append("_");
                                        sbStep.Append(intStep.ToString());
                                        sbStep.Append("\" style=\"display:none\"><td></td><td>");
                                        strError = dsResult.Tables[0].Rows[0]["result"].ToString();
                                        if (strError.Contains("~") == true)
                                            strError = strError.Substring(0, strError.IndexOf("~"));
                                        sbStep.Append(strError);
                                        DataSet dsError = oServer.GetError(intServer, intStep);
                                        string incident = "";
                                        int assigned = 0;
                                        if (dsError.Tables[0].Rows.Count > 0)
                                        {
                                            incident = dsError.Tables[0].Rows[0]["incident"].ToString();
                                            Int32.TryParse(dsError.Tables[0].Rows[0]["assigned"].ToString(), out assigned);
                                            if (string.IsNullOrEmpty(incident) == false)
                                            {
                                                sbStep.Append("<br/><br/>Tracking # " + incident);
                                                bool IncidentFound = false;
                                                DataSet dsKey = oFunction.GetSetupValuesByKey("INCIDENTS");
                                                if (dsKey.Tables[0].Rows.Count > 0)
                                                {
                                                    string incidents = dsKey.Tables[0].Rows[0]["Value"].ToString();
                                                    StreamReader theReader = new StreamReader(incidents);
                                                    string theContents = theReader.ReadToEnd();
                                                    string[] theLines = theContents.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                                                    foreach (string theLine in theLines)
                                                    {
                                                        if (theLine.Contains(incident))
                                                        {
                                                            IncidentFound = true;
                                                            string[] theFields = theLine.Split(new char[] { ',' }, StringSplitOptions.None);
                                                            string person = theFields[5].Replace("\"", "");
                                                            if (String.IsNullOrEmpty(person) == false)
                                                                sbStep.Append("<br/>Assigned To: " + person);
                                                            string group = theFields[4].Replace("\"", "");
                                                            if (String.IsNullOrEmpty(group) == false)
                                                                sbStep.Append("<br/>Group: " + group);
                                                            break;
                                                        }
                                                    }
                                                }
                                                if (IncidentFound == false)
                                                {
                                                    if (assigned > 0)
                                                        sbStep.Append("<br/>Assigned To: " + oUser.GetFullNameWithLanID(assigned));
                                                }
                                            }
                                        }
                                        if (strError != "")
                                        {
                                            sbStep.Append("<br/><br/><a class='build_error' href=\"javascript:void(0);\" onclick=\"OpenWindow('PROVISIONING_ERROR', '");
                                            sbStep.Append(oFunction.encryptQueryString(dsResult.Tables[0].Rows[0]["result"].ToString()) + "&incident=" + incident);
                                            sbStep.Append("');\"><img src='/images/plus.gif' border='0' align='absmiddle'/>&nbsp;For more information about this error, click here</a>");
                                        }
                                        sbStep.Append("</td></tr>");
                                    }
                                }
                                if (boolError == false)
                                {
                                    if (drStep["interact_path"].ToString() == "")
                                    {
                                        sbStep.Append("<tr><td><img src=\"/images/green_arrow.gif\" border=\"0\" align=\"absmiddle\"></td><td class=\"");
                                        sbStep.Append(strClass);
                                        sbStep.Append("\" nowrap><b>");
                                        sbStep.Append(drStep["title"].ToString());
                                        sbStep.Append("</b></td></tr>");
                                    }
                                    else
                                    {
                                        sbStep.Append("<tr><td><img src=\"/images/alert.gif\" border=\"0\" align=\"absmiddle\"></td><td class=\"");
                                        sbStep.Append(strClass);
                                        sbStep.Append("\" nowrap><b>");
                                        sbStep.Append(drStep["title"].ToString());
                                        sbStep.Append("</b></td></tr>");
                                        sbStep.Append("<tr><td></td><td><input type=\"button\" onclick=\"OpenWindow('NEW_WINDOW','");
                                        sbStep.Append(drStep["interact_path"].ToString());
                                        sbStep.Append("?id=");
                                        sbStep.Append(intServer.ToString());
                                        sbStep.Append("');\" value=\"Click Here\" class=\"default\" style=\"width:100px\"></td></tr>");
                                    }
                                }
                            }
                            else if (intStep > intCurrent)
                            {
                                sbStep.Append("<tr><td></td><td class=\"");
                                sbStep.Append(strClass);
                                sbStep.Append("\">");
                                sbStep.Append(drStep["title"].ToString());
                                sbStep.Append("</td></tr>");
                            }
                        }
                        sbStep.Insert(0, "<p><table border=\"0\" cellpadding=\"4\" cellspacing=\"3\">");
                        sbStep.Append("</table></p>");
                        strResult += sbStep.ToString();
                    }
                    else
                    {
                        panInventoryNo.Visible = true;
                        lblInventory.Text = oModelsProperties.Get(intModel, "name");
                    }
                }
            }
            if (strOnLoad != "")
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "start_count", "<script type=\"text/javascript\">window.onload = new Function(\"" + strOnLoad + "\");" + (strError == "" ? "" : "LoadError('" + strError.Replace("'", "") + "');") + "<" + "/" + "script>");
        }
        protected void chkPreview_Change(Object Sender, EventArgs e)
        {
            Response.Redirect(Request.Path + "?id=" + Request.QueryString["id"] + "&c=" + Request.QueryString["c"] + (chkPreview.Checked ? "&preview=true" : ""));
        }
    }
}
