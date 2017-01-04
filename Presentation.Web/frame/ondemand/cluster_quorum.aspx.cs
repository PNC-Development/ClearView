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
    public partial class cluster_quorum : BasePage
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
        protected Servers oServer;
        protected OperatingSystems oOperatingSystem;
        protected ServerName oServerName;
        protected int intAnswer = 0;
        protected int intCluster = 0;
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
            oServer = new Servers(intProfile, dsn);
            oOperatingSystem = new OperatingSystems(intProfile, dsn);
            oServerName = new ServerName(intProfile, dsn);
            if (Request.QueryString["aid"] != null && Request.QueryString["aid"] != "")
                intAnswer = Int32.Parse(Request.QueryString["aid"]);
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                intCluster = Int32.Parse(Request.QueryString["id"]);
            if (Request.QueryString["save"] != null)
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "saved", "<script type=\"text/javascript\">RefreshOpeningWindow();alert('Cluster Quorum Configuration Saved');window.close();<" + "/" + "script>");
            if (intCluster > 0)
            {
                StringBuilder sbSQL = new StringBuilder(strSQL);
                StringBuilder sbHidden = new StringBuilder(strHidden);
                DataSet ds = oForecast.GetAnswer(intAnswer);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    int intClass = Int32.Parse(ds.Tables[0].Rows[0]["classid"].ToString());
                    ds = oCluster.Get(intCluster);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        if (!IsPostBack)
                        {
                            bool boolProd = false;
                            bool boolQA = false;
                            bool boolTest = false;
                            bool boolUnder = false;
                            if (oClass.IsProd(intClass))
                            {
                                boolProd = true;
                                if (oForecast.GetAnswer(intAnswer, "test") == "1")
                                    boolTest = true;
                                if (oForecast.GetAnswerPlatform(intAnswer, intUnder48Q, intUnder48A) == true)
                                    boolUnder = true;
                            }
                            else if (oClass.IsQA(intClass))
                                boolQA = true;
                            else
                                boolTest = true;
                            int intRequest = oForecast.GetRequestID(intAnswer, true);
                            // Check if SQL is on any of the instances
                            bool boolSQL = false;
                            ds = oCluster.GetInstances(intCluster);
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                if (dr["sql"].ToString() == "1")
                                {
                                    boolSQL = true;
                                    break;
                                }
                            }
                            
                            bool boolWin2008 = false;
                            bool boolSQL2008 = false;
                            DataSet dsServers = oServer.GetAnswer(intAnswer);
                            foreach (DataRow drServer in dsServers.Tables[0].Rows)
                            {
                                int intServer = Int32.Parse(drServer["id"].ToString());
                                if (boolWin2008 == false && oOperatingSystem.IsWindows2008(Int32.Parse(drServer["osid"].ToString())) == true)
                                    boolWin2008 = true;
                                DataSet dsComponents = oServerName.GetComponentDetailSelected(intServer, 1);
                                foreach (DataRow drComponent in dsComponents.Tables[0].Rows)
                                {
                                    if (drComponent["sql"].ToString() == "1" || drComponent["dbase"].ToString() == "1")
                                    {
                                        if (drComponent["name"].ToString().Contains("2008") == true)
                                            boolSQL2008 = true;
                                    }
                                }
                            }
                            // Load Table
                            int intModel = oForecast.GetModel(intAnswer);
                            bool boolHADisabled = (oModelsProperties.IsHighAvailability(intModel) == false);
                            panView.Visible = true;
                            ds = oStorage.GetLuns(intAnswer, 0, intCluster, 0, 0);
                            bool boolOther = false;
                            int intNumber = 0;
                            bool boolCreateCLU = true;
                            bool boolCreateDTC = boolSQL;
                            if (boolSQL == true && boolWin2008 == true && boolSQL2008 == true)
                            {
                                // For SQL Server 2008 on a Windows 2008 cluster, no "P" MSDTC drive is needed.
                                // 12/2/2011 - Per Moskal, 2008 now needs the MSDTC drive.
                                //boolCreateDTC = false;
                            }
                           
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                if ((dr["driveid"].ToString() == "-1") || ((dr["driveid"].ToString() == "-10") && boolSQL == true))
                                {
                                    // -1 is hard coded to be Q: (CLU) drive
                                    // -10 is hard coded to be P: (DTC) drive
                                    intNumber++;
                                    boolOther = !boolOther;
                                    sbSQL.Append("<tr");
                                    sbSQL.Append(boolOther ? " bgcolor=\"F6F6F6\"" : "");
                                    sbSQL.Append(">");
                                    sbSQL.Append("<td valign=\"top\">");
                                    sbSQL.Append(intNumber);
                                    sbSQL.Append("</td>");
                                    if (dr["driveid"].ToString() == "-1")
                                    {
                                        sbSQL.Append("<td valign=\"top\">Q:</td>");
                                        boolCreateCLU = false;
                                    }
                                    if (dr["driveid"].ToString() == "-10" && boolSQL == true)
                                    {
                                        sbSQL.Append("<td valign=\"top\">P:</td>");
                                        boolCreateDTC = false;
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
                                        sbSQL.Append("<td valign=\"top\"><input type=\"text\" name=\"ValidTextbox0\" class=\"default\" style=\"width:75px\" onblur=\"UpdateText(this,'HDN_");
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
                                        sbSQL.Append("<td valign=\"top\"><input type=\"text\" name=\"ValidTextbox0\" class=\"default\" style=\"width:75px\" onblur=\"UpdateText(this,'HDN_");
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
                                        sbSQL.Append("<td valign=\"top\"><input type=\"text\" name=\"ValidTextbox0\" class=\"default\" style=\"width:75px\" onblur=\"UpdateText(this,'HDN_");
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
                                    if (boolProd == true && boolUnder == true)
                                    {
                                        sbSQL.Append("<select class=\"default\" onchange=\"UpdateDDL(this,'HDN_");
                                        sbSQL.Append(dr["id"].ToString());
                                        sbSQL.Append("_REPLICATED');\" style=\"width:75px;\">");
                                        sbSQL.Append("<option value=\"Yes\"");
                                        sbSQL.Append(dr["replicated"].ToString() == "1" ? " selected" : "");
                                        sbSQL.Append(">Yes</option>");
                                        sbSQL.Append("<option value=\"No\"");
                                        sbSQL.Append(dr["replicated"].ToString() == "0" ? " selected" : "");
                                        sbSQL.Append(">No</option>");
                                        sbSQL.Append("</select>");
                                    }
                                    else
                                    {
                                        sbSQL.Append("<select class=\"default\" disabled=\"disabled\" style=\"width:75px;\">");
                                        sbSQL.Append("<option value=\"No\">No</option>");
                                        sbSQL.Append("</select>");
                                    }
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
                                    if (boolProd == true)
                                    {
                                        sbHidden.Append("<input type=\"hidden\" name=\"HDN_");
                                        sbHidden.Append(dr["id"].ToString());
                                        sbHidden.Append("_SIZE\" id=\"HDN_");
                                        sbHidden.Append(dr["id"].ToString());
                                        sbHidden.Append("_SIZE\" value=\"");
                                        sbHidden.Append(dr["size"].ToString());
                                        sbHidden.Append("\" />");
                                    }
                                    else
                                    {
                                        sbHidden.Append("<input type=\"hidden\" name=\"HDN_");
                                        sbHidden.Append(dr["id"].ToString());
                                        sbHidden.Append("_SIZE\" id=\"HDN_");
                                        sbHidden.Append(dr["id"].ToString());
                                        sbHidden.Append("_SIZE\" value=\"0\" />");
                                    }
                                    if (boolQA == true)
                                    {
                                        sbHidden.Append("<input type=\"hidden\" name=\"HDN_");
                                        sbHidden.Append(dr["id"].ToString());
                                        sbHidden.Append("_SIZE_QA\" id=\"HDN_");
                                        sbHidden.Append(dr["id"].ToString());
                                        sbHidden.Append("_SIZE_QA\" value=\"");
                                        sbHidden.Append(dr["size_qa"].ToString());
                                        sbHidden.Append("\" />");
                                    }
                                    else
                                    {
                                        sbHidden.Append("<input type=\"hidden\" name=\"HDN_");
                                        sbHidden.Append(dr["id"].ToString());
                                        sbHidden.Append("_SIZE_QA\" id=\"HDN_");
                                        sbHidden.Append(dr["id"].ToString());
                                        sbHidden.Append("_SIZE_QA\" value=\"0\" />");
                                    }
                                    if (boolTest == true)
                                    {
                                        sbHidden.Append("<input type=\"hidden\" name=\"HDN_");
                                        sbHidden.Append(dr["id"].ToString());
                                        sbHidden.Append("_SIZE_TEST\" id=\"HDN_");
                                        sbHidden.Append(dr["id"].ToString());
                                        sbHidden.Append("_SIZE_TEST\" value=\"");
                                        sbHidden.Append(dr["size_test"].ToString());
                                        sbHidden.Append("\" />");
                                    }
                                    else
                                    {
                                        sbHidden.Append("<input type=\"hidden\" name=\"HDN_");
                                        sbHidden.Append(dr["id"].ToString());
                                        sbHidden.Append("_SIZE_TEST\" id=\"HDN_");
                                        sbHidden.Append(dr["id"].ToString());
                                        sbHidden.Append("_SIZE_TEST\" value=\"0\" />");
                                    }
                                    if (boolProd == true && boolUnder == true)
                                    {
                                        sbHidden.Append("<input type=\"hidden\" name=\"HDN_");
                                        sbHidden.Append(dr["id"].ToString());
                                        sbHidden.Append("_REPLICATED\" id=\"HDN_");
                                        sbHidden.Append(dr["id"].ToString());
                                        sbHidden.Append("_REPLICATED\" value=\"");
                                        sbHidden.Append(dr["replicated"].ToString() == "1" ? "Yes" : "No");
                                        sbHidden.Append("\" />");
                                    }
                                    else
                                    {
                                        sbHidden.Append("<input type=\"hidden\" name=\"HDN_");
                                        sbHidden.Append(dr["id"].ToString());
                                        sbHidden.Append("_REPLICATED\" id=\"HDN_");
                                        sbHidden.Append(dr["id"].ToString());
                                        sbHidden.Append("_REPLICATED\" value=\"No\" />");
                                    }
                                    sbHidden.Append("<input type=\"hidden\" name=\"HDN_");
                                    sbHidden.Append(dr["id"].ToString());
                                    sbHidden.Append("_HIGH_AVAILABILITY\" id=\"HDN_");
                                    sbHidden.Append(dr["id"].ToString());
                                    sbHidden.Append("_HIGH_AVAILABILITY\" value=\"");
                                    sbHidden.Append(dr["high_availability"].ToString() == "1" ? "Yes" : "No");
                                    sbHidden.Append("\" />");
                                }
                            }
                            btnSave.Attributes.Add("onclick", "return EnsureTextbox0();");
                            if (boolCreateCLU == true)
                                oStorage.AddLun(intAnswer, 0, intCluster, 0, 0, -1, (boolProd ? 1.00 : 0.00), (boolQA ? 1.00 : 0.00), (boolTest ? 1.00 : 0.00));
                            if (boolCreateDTC == true)
                                oStorage.AddLun(intAnswer, 0, intCluster, 0, 0, -10, (boolProd ? 1.00 : 0.00), (boolQA ? 1.00 : 0.00), (boolTest ? 1.00 : 0.00));
                            if (boolCreateCLU == true || boolCreateDTC == true)
                                Response.Redirect(Request.Path + "?aid=" + intAnswer + "&id=" + intCluster);
                        }
                    }
                }

                strSQL = sbSQL.ToString();
                strHidden = sbHidden.ToString();
            }
            btnClose.Attributes.Add("onclick", "return window.close();");
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "loader", "<script type=\"text/javascript\">CatchClose();<" + "/" + "script>");
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
            Response.Redirect(Request.Path + "?aid=" + intAnswer + "&id=" + intCluster + "&save=true");
        }
    }
}
