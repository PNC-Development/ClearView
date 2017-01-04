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
    public partial class cluster_instance_config : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);

        protected int intUnder48A = Int32.Parse(ConfigurationManager.AppSettings["DR_HOUR_RESPONSE"]);
        protected int intUnder48Q = Int32.Parse(ConfigurationManager.AppSettings["DR_HOUR_QUESTION"]);
        protected int intProfile;
        protected Cluster oCluster;
        protected Storage oStorage;
        protected Forecast oForecast;
        protected Models oModel;
        protected ModelsProperties oModelsProperties;
        protected Classes oClass;
        protected int intAnswer = 0;
        protected int intCluster = 0;
        protected int intInstance = 0;
        protected bool boolProd = false;
        protected bool boolQA = false;
        protected bool boolTest = false;
        protected string strSQL = "";
        protected string strHidden = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oCluster = new Cluster(intProfile, dsn);
            oStorage = new Storage(intProfile, dsn);
            oForecast = new Forecast(intProfile, dsn);
            oModel = new Models(intProfile, dsn);
            oModelsProperties = new ModelsProperties(intProfile, dsn);
            oClass = new Classes(intProfile, dsn);
            if (Request.QueryString["aid"] != null && Request.QueryString["aid"] != "")
                intAnswer = Int32.Parse(Request.QueryString["aid"]);
            if (Request.QueryString["cid"] != null && Request.QueryString["cid"] != "")
                intCluster = Int32.Parse(Request.QueryString["cid"]);
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                intInstance = Int32.Parse(Request.QueryString["id"]);
            if (intInstance > 0)
            {
                StringBuilder sbSQL = new StringBuilder(strSQL);
                StringBuilder sbHidden = new StringBuilder(strHidden);
                DataSet ds = oCluster.GetInstance(intInstance);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (!IsPostBack)
                    {
                        int intRequest = oForecast.GetRequestID(intAnswer, true);
                        int intModel = oForecast.GetModel(intAnswer);
                        bool boolHADisabled = (oModelsProperties.IsHighAvailability(intModel) == false);
                        panView.Visible = true;
                        ds = oStorage.GetLuns(intAnswer, intInstance, intCluster, 0, 0);
                        bool boolOverride = (oForecast.GetAnswer(intAnswer, "storage_override") == "1");
                      
                        int intClass = Int32.Parse(oForecast.GetAnswer(intAnswer, "classid"));
                        if (oClass.IsProd(intClass))
                        {
                            boolProd = true;
                            if (oForecast.GetAnswer(intAnswer, "test") == "1")
                                boolTest = true;
                            
                        }
                        else if (oClass.IsQA(intClass))
                            boolQA = true;
                        else
                            boolTest = true;
                        bool boolOther = false;
                        int intNumber = 0;
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            intNumber++;
                            boolOther = !boolOther;
                            sbSQL.Append("<tr");
                            sbSQL.Append(boolOther ? " bgcolor=\"F6F6F6\"" : "");
                            sbSQL.Append(">");
                            sbSQL.Append("<td valign=\"top\">");
                            sbSQL.Append(intNumber);
                            sbSQL.Append("</td>");
                            string strLetter = dr["letter"].ToString();
                            if (strLetter == "")
                            {
                                if (dr["driveid"].ToString() == "-1000")
                                    strLetter = "E";
                                else if (dr["driveid"].ToString() == "-100")
                                    strLetter = "F";
                                else if (dr["driveid"].ToString() == "-10")
                                    strLetter = "P";
                                else if (dr["driveid"].ToString() == "-1")
                                    strLetter = "Q";
                            }
                            if (boolOverride == false)
                            {
                                sbSQL.Append("<td valign=\"top\">");
                                sbSQL.Append(strLetter);
                                sbSQL.Append(":");
                                sbSQL.Append(dr["path"].ToString());
                                sbSQL.Append("</td>");
                            }
                            else
                            {
                                sbSQL.Append("<td valign=\"top\">");
                                sbSQL.Append(dr["path"].ToString());
                                sbSQL.Append("</td>");
                            }
                            sbSQL.Append("<td valign=\"top\">");
                            sbSQL.Append("<select class=\"default\" onchange=\"UpdateDDL(this,'HDN_");
                            sbSQL.Append(dr["id"].ToString());
                            sbSQL.Append("_PERFORMANCE');\" style=\"width:100px;\">");
                            sbSQL.Append("<option value=\"High\"");
                            sbSQL.Append(dr["performance"].ToString() == "High" ? " selected" : "");
                            sbSQL.Append(">High</option>");
                            sbSQL.Append("<option value=\"Standard\"");
                            sbSQL.Append(dr["performance"].ToString() == "Standard" ? " selected" : "");
                            sbSQL.Append(">Standard</option>");
                            sbSQL.Append("<option value=\"Low\"");
                            sbSQL.Append(dr["performance"].ToString() == "Low" ? " selected" : "");
                            sbSQL.Append(">Low</option>");
                            sbSQL.Append("</select>");
                            sbSQL.Append("</td>");

                            if (boolProd == true)
                            {
                                sbSQL.Append("<td valign=\"top\"><input type=\"text\" name=\"ValidTextbox0\" class=\"default\" style=\"width:75px\"");
                                sbSQL.Append(intNumber == 1 && ds.Tables[0].Rows.Count > 1 ? " disabled" : "");
                                sbSQL.Append(" onblur=\"UpdateText(this,'HDN_");
                                sbSQL.Append(dr["id"].ToString());
                                sbSQL.Append("_SIZE');\" value=\"");
                                sbSQL.Append(dr["size"].ToString());
                                sbSQL.Append("\" /> GB</td>");
                            }
                            else
                            {
                                sbSQL.Append("<td valign=\"top\"><input type=\"text\" name=\"ValidTextbox0\" class=\"default\" style=\"width:75px\" value=\"0\" disabled=\"disabled\" /> GB</td>");
                            }
                            if (boolQA == true)
                            {
                                sbSQL.Append("<td valign=\"top\"><input type=\"text\" name=\"ValidTextbox0\" class=\"default\" style=\"width:75px\"");
                                sbSQL.Append(intNumber == 1 && ds.Tables[0].Rows.Count > 1 ? " disabled" : "");
                                sbSQL.Append(" onblur=\"UpdateText(this,'HDN_");
                                sbSQL.Append(dr["id"].ToString());
                                sbSQL.Append("_SIZE_QA');\" value=\"");
                                sbSQL.Append(dr["size_qa"].ToString());
                                sbSQL.Append("\" /> GB</td>");
                            }
                            else
                            {
                                sbSQL.Append("<td valign=\"top\"><input type=\"text\" name=\"ValidTextbox0\" class=\"default\" style=\"width:75px\" value=\"0\" disabled=\"disabled\" /> GB</td>");
                            }
                            if (boolTest == true)
                            {
                                sbSQL.Append("<td valign=\"top\"><input type=\"text\" name=\"ValidTextbox0\" class=\"default\" style=\"width:75px\"");
                                sbSQL.Append(intNumber == 1 && ds.Tables[0].Rows.Count > 1 ? " disabled" : "");
                                sbSQL.Append(" onblur=\"UpdateText(this,'HDN_");
                                sbSQL.Append(dr["id"].ToString());
                                sbSQL.Append("_SIZE_TEST');\" value=\"");
                                sbSQL.Append(dr["size_test"].ToString());
                                sbSQL.Append("\" /> GB</td>");
                            }
                            else
                            {
                                sbSQL.Append("<td valign=\"top\"><input type=\"text\" name=\"ValidTextbox0\" class=\"default\" style=\"width:75px\" value=\"0\" disabled=\"disabled\" /> GB</td>");
                            }
                            
                            sbSQL.Append("<td valign=\"top\">");
                            sbSQL.Append("<select class=\"default\" onchange=\"UpdateDDL(this,'HDN_");
                            sbSQL.Append(dr["id"].ToString());
                            sbSQL.Append("_REPLICATED');\" style=\"width:75px;\">");
                            sbSQL.Append("<option value=\"No\"");
                            sbSQL.Append(dr["replicated"].ToString() == "0" ? " selected" : "");
                            sbSQL.Append(">No</option>");
                            sbSQL.Append("<option value=\"Yes\"");
                            sbSQL.Append(dr["replicated"].ToString() == "1" ? " selected" : "");
                            sbSQL.Append(">Yes</option>");
                            sbSQL.Append("</select>");
                            sbSQL.Append("</td>");
                            sbSQL.Append("<td valign=\"top\">");
                            sbSQL.Append("<select class=\"default\" onchange=\"UpdateDDL(this,'HDN_");
                            sbSQL.Append(dr["id"].ToString());
                            sbSQL.Append("_HIGH_AVAILABILITY');\" style=\"width:75px;\"");
                            sbSQL.Append(boolHADisabled == true ? " disabled" : "");
                            sbSQL.Append(">");
                            sbSQL.Append("<option value=\"No\"");
                            sbSQL.Append(dr["high_availability"].ToString() == "0" ? " selected" : "");
                            sbSQL.Append(">No</option>");
                            sbSQL.Append("<option value=\"Yes\"");
                            sbSQL.Append(dr["high_availability"].ToString() == "1" ? " selected" : "");
                            sbSQL.Append(">Yes</option>");
                            sbSQL.Append("</select>");
                            sbSQL.Append("</td>");
                            sbSQL.Append("</tr>");

                            sbHidden.Append("<input type=\"hidden\" name=\"HDN_");
                            sbHidden.Append(dr["id"].ToString());
                            sbHidden.Append("_PERFORMANCE\" id=\"HDN_");
                            sbHidden.Append(dr["id"].ToString());
                            sbHidden.Append("_PERFORMANCE\" value=\"");
                            sbHidden.Append(dr["performance"].ToString());
                            sbHidden.Append("\" />");
                            sbHidden.Append("<input type=\"hidden\" name=\"HDN_");
                            sbHidden.Append(dr["id"].ToString());
                            sbHidden.Append("_SIZE\" id=\"HDN_");
                            sbHidden.Append(dr["id"].ToString());
                            sbHidden.Append("_SIZE\" value=\"");
                            sbHidden.Append(dr["size"].ToString());
                            sbHidden.Append("\" />");
                            sbHidden.Append("<input type=\"hidden\" name=\"HDN_");
                            sbHidden.Append(dr["id"].ToString());
                            sbHidden.Append("_SIZE_QA\" id=\"HDN_");
                            sbHidden.Append(dr["id"].ToString());
                            sbHidden.Append("_SIZE_QA\" value=\"");
                            sbHidden.Append(dr["size_qa"].ToString());
                            sbHidden.Append("\" />");
                            sbHidden.Append("<input type=\"hidden\" name=\"HDN_");
                            sbHidden.Append(dr["id"].ToString());
                            sbHidden.Append("_SIZE_TEST\" id=\"HDN_");
                            sbHidden.Append(dr["id"].ToString());
                            sbHidden.Append("_SIZE_TEST\" value=\"");
                            sbHidden.Append(dr["size_test"].ToString());
                            sbHidden.Append("\" />");
                            sbHidden.Append("<input type=\"hidden\" name=\"HDN_");
                            sbHidden.Append(dr["id"].ToString());
                            sbHidden.Append("_REPLICATED\" id=\"HDN_");
                            sbHidden.Append(dr["id"].ToString());
                            sbHidden.Append("_REPLICATED\" value=\"");
                            sbHidden.Append(dr["replicated"].ToString() == "1" ? "Yes" : "No");
                            sbHidden.Append("\" />");
                            sbHidden.Append("<input type=\"hidden\" name=\"HDN_");
                            sbHidden.Append(dr["id"].ToString());
                            sbHidden.Append("_HIGH_AVAILABILITY\" id=\"HDN_");
                            sbHidden.Append(dr["id"].ToString());
                            sbHidden.Append("_HIGH_AVAILABILITY\" value=\"");
                            sbHidden.Append(dr["high_availability"].ToString() == "1" ? "Yes" : "No");
                            sbHidden.Append("\" />");
                        }
                    }
                }

                strSQL = sbSQL.ToString();
                strHidden = sbHidden.ToString();
            }
            btnClose.Attributes.Add("onclick", "return window.close();");
        }
        protected void btnSave_Click(Object Sender, EventArgs e)
        {
            foreach (string strForm in Request.Form)
            {
                if (strForm.StartsWith("HDN_") == true)
                {
                    int intID = Int32.Parse(strForm.Substring(4, strForm.IndexOf("_", 4) - 4));
                    oStorage.UpdateLun(intID, "", Request.Form["HDN_" + intID.ToString() + "_PERFORMANCE"], double.Parse(Request.Form["HDN_" + intID.ToString() + "_SIZE"]), double.Parse(Request.Form["HDN_" + intID.ToString() + "_SIZE_QA"]), double.Parse(Request.Form["HDN_" + intID.ToString() + "_SIZE_TEST"]), (Request.Form["HDN_" + intID.ToString() + "_REPLICATED"] == "Yes" ? 1 : 0), (Request.Form["HDN_" + intID.ToString() + "_HIGH_AVAILABILITY"] == "Yes" ? 1 : 0));
                }
            }
            Response.Redirect(Request.Path + "?aid=" + intAnswer + "&cid=" + intCluster + "&id=" + intInstance);
        }
    }
}
