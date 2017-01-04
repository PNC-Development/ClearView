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
    public partial class cluster_instance : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        
        protected int intUnder48A = Int32.Parse(ConfigurationManager.AppSettings["DR_HOUR_RESPONSE"]);
        protected int intUnder48Q = Int32.Parse(ConfigurationManager.AppSettings["DR_HOUR_QUESTION"]);
        protected int intProfile;
        protected OnDemand oOnDemand;
        protected Cluster oCluster;
        protected Forecast oForecast;
        protected Storage oStorage;
        protected Requests oRequest;
        protected Models oModel;
        protected ModelsProperties oModelsProperties;
        protected Classes oClass;
        protected int intAnswer = 0;
        protected int intRequest = 0;
        protected int intCluster = 0;
        protected int intInstance = 0;
        protected string strSQL = "";
        protected string strHidden = "";
        protected bool boolProd = false;
        protected bool boolQA = false;
        protected bool boolTest = false;
        protected bool boolMidrange = false;
        protected bool boolOverride = false;
        protected string strPaths = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oOnDemand = new OnDemand(intProfile, dsn);
            oCluster = new Cluster(intProfile, dsn);
            oForecast = new Forecast(intProfile, dsn);
            oStorage = new Storage(intProfile, dsn);
            oRequest = new Requests(intProfile, dsn);
            oModel = new Models(intProfile, dsn);
            oModelsProperties = new ModelsProperties(intProfile, dsn);
            oClass = new Classes(intProfile, dsn);
            if (Request.QueryString["aid"] != null && Request.QueryString["aid"] != "")
                intAnswer = Int32.Parse(Request.QueryString["aid"]);
            if (Request.QueryString["cid"] != null && Request.QueryString["cid"] != "")
                intCluster = Int32.Parse(Request.QueryString["cid"]);
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                intInstance = Int32.Parse(Request.QueryString["id"]);
            if (Request.QueryString["save"] != null)
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "save", "<script type=\"text/javascript\">RefreshOpeningWindow();alert('Storage Configuration Saved');window.close();<" + "/" + "script>");
            if (Request.QueryString["refresh"] != null)
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "refresh", "<script type=\"text/javascript\">RefreshOpeningWindow();<" + "/" + "script>");

            StringBuilder sbSQL = new StringBuilder(strSQL);
            StringBuilder sbHidden = new StringBuilder(strHidden);

            btnStorageOverride.Attributes.Add("onclick", "return OpenWindow('STORAGE_OVERRIDE','" + intAnswer.ToString() + "')");

            if (intAnswer > 0)
            {
                DataSet ds = oForecast.GetAnswer(intAnswer);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    int intModel = oForecast.GetModel(intAnswer);
                    int intType = oModelsProperties.GetType(intModel);
                    int intParent = 0;
                    if (oForecast.IsOSMidrange(intAnswer) == true)
                        boolMidrange = true;
                    intParent = Int32.Parse(oModelsProperties.Get(intModel, "modelid"));
                    bool boolHADisabled = (oModelsProperties.IsHighAvailability(intModel) == false);
                    bool boolUnder = false;
                    int intClass = Int32.Parse(ds.Tables[0].Rows[0]["classid"].ToString());
                    boolOverride = (ds.Tables[0].Rows[0]["storage_override"].ToString() == "1");

                    if (boolOverride == true)
                    {
                        btnAdd.Text = "Add a Lun";
                        btnAdd.CommandArgument = "ADDLUN";
                        btnStorageOverride.Visible = false;
                    }

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

                    if (intCluster > 0)
                    {
                        intRequest = oForecast.GetRequestID(intAnswer, true);
                        ds = oCluster.Get(intCluster);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            if (!IsPostBack)
                            {


                                ////Delete the luns (if specified )
                                if (boolOverride == true)
                                {
                                    if (Request.QueryString["dellun"] != null && Request.QueryString["dellun"] != "")
                                    {
                                        oStorage.DeleteLunByLunID(Int32.Parse(Request.QueryString["dellun"]));
                                        Response.Redirect(Request.Path + "?aid=" + intAnswer + "&cid=" + intCluster + "&id=" + intInstance + "&refresh=true");
                                    }
                                }

                                panView.Visible = true;
                                ds = oCluster.GetInstance(intInstance);
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    txtName.Text = ds.Tables[0].Rows[0]["nickname"].ToString();
                                    if (ds.Tables[0].Rows[0]["sql"].ToString() == "1" && boolOverride == false)
                                        btnAdd.Enabled = false;

                                    if (boolMidrange == true && boolOverride == false)
                                        btnAdd.Text = "Add Filesystem";
                                    bool boolOther = false;
                                    int intRow = 0;
                                    ds = oStorage.GetLuns(intAnswer, intInstance, intCluster, 0, 0);
                                    foreach (DataRow dr in ds.Tables[0].Rows)
                                    {
                                        int intDrive = Int32.Parse(dr["driveid"].ToString());
                                        if (intDrive > 0 || intDrive == -100 || intDrive == -1000 || (boolOverride == true && intDrive == 0))
                                        {
                                            lblLun.Text = dr["id"].ToString();
                                            intRow++;
                                            boolOther = !boolOther;
                                            sbSQL.Append("<tr");
                                            sbSQL.Append(boolOther ? " bgcolor=\"F6F6F6\"" : "");
                                            sbSQL.Append(">");

                                            if (boolOverride == true)
                                            {
                                                sbSQL.Append("<td valign=\"top\" align=\"right\">[<a href=\"");
                                                sbSQL.Append(Request.Path);
                                                sbSQL.Append("?aid=");
                                                sbSQL.Append(Request.QueryString["aid"]);
                                                sbSQL.Append("&cid=");
                                                sbSQL.Append(Request.QueryString["cid"]);
                                                sbSQL.Append("&id=");
                                                sbSQL.Append(Request.QueryString["id"]);
                                                sbSQL.Append("&refresh=true&dellun=");
                                                sbSQL.Append(dr["id"].ToString());
                                                sbSQL.Append("\" onclick=\"return confirm('Are you sure you want to delete this lun?');\" title=\"Delete\">Delete</a>]&nbsp;&nbsp;");
                                                sbSQL.Append(intRow);
                                                sbSQL.Append("&nbsp;&nbsp;&nbsp;</td>");
                                            }
                                            else
                                            {
                                                sbSQL.Append("<td valign=\"top\" align=\"right\">");
                                                sbSQL.Append(intRow);
                                                sbSQL.Append("&nbsp;&nbsp;&nbsp;</td>");
                                            }

                                            string strPath = dr["path"].ToString();
                                            if (strPath != "")
                                            {
                                                if (strPaths != "")
                                                    strPaths += ",";
                                                while (strPath != "" && strPath.EndsWith("\\") == true)
                                                    strPath = strPath.Substring(0, strPath.Length - 1);
                                                strPaths += "\"" + strPath + "\"";
                                            }
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
                                            if (boolMidrange == true || boolOverride == true)
                                            {
                                                sbSQL.Append("<td valign=\"top\"><input type=\"text\" name=\"ValidTextbox\" class=\"default\" style=\"width:125px\" onblur=\"UpdateTextPath(this,'HDN_");
                                                sbSQL.Append(dr["id"].ToString());
                                                sbSQL.Append("_PATH');\" onblur=\"BlurPath(this,'HDN_");
                                                sbSQL.Append(dr["id"].ToString());
                                                sbSQL.Append("_PATH','");
                                                sbSQL.Append(strPath);
                                                sbSQL.Append("');\" value=\"");
                                                sbSQL.Append(strPath);
                                                sbSQL.Append("\" /></td>");
                                            }
                                            else
                                            {
                                                sbSQL.Append("<td valign=\"top\">");
                                                sbSQL.Append(strLetter);
                                                sbSQL.Append(":");
                                                sbSQL.Append(strPath);
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
                                            sbHidden.Append("_PATH\" id=\"HDN_");
                                            sbHidden.Append(dr["id"].ToString());
                                            sbHidden.Append("_PATH\" value=\"");
                                            sbHidden.Append(strPath);
                                            sbHidden.Append("\" />");
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
                                            // Delete Mount Point (if there)
                                            if (Request.QueryString["del"] != null && Request.QueryString["del"] != "")
                                                oStorage.DeleteMountPoint(Int32.Parse(Request.QueryString["del"]));
                                            // Add Mount Points
                                            DataSet dsPoints = oStorage.GetMountPoints(Int32.Parse(dr["id"].ToString()));
                                            int intPoint = 0;
                                            foreach (DataRow drPoint in dsPoints.Tables[0].Rows)
                                            {
                                                intRow++;
                                                intPoint++;
                                                boolOther = !boolOther;
                                                sbSQL.Append("<tr");
                                                sbSQL.Append(boolOther ? " bgcolor=\"F6F6F6\"" : "");
                                                sbSQL.Append(">");
                                                sbSQL.Append("<td valign=\"top\" align=\"right\">[<a href=\"");
                                                sbSQL.Append(Request.Path);
                                                sbSQL.Append("?aid=");
                                                sbSQL.Append(Request.QueryString["aid"]);
                                                sbSQL.Append("&cid=");
                                                sbSQL.Append(Request.QueryString["cid"]);
                                                sbSQL.Append("&id=");
                                                sbSQL.Append(Request.QueryString["id"]);
                                                sbSQL.Append("&refresh=true&del=");
                                                sbSQL.Append(drPoint["id"].ToString());
                                                sbSQL.Append("\" onclick=\"return confirm('Are you sure you want to delete this ");
                                                sbSQL.Append(boolMidrange ? "filesystem" : "mount point");
                                                sbSQL.Append("?');\" title=\"Delete\">Delete</a>]&nbsp;&nbsp;");
                                                sbSQL.Append(intRow);
                                                sbSQL.Append("&nbsp;&nbsp;&nbsp;</td>");
                                                strPath = drPoint["path"].ToString();
                                                if (strPath != "")
                                                {
                                                    if (strPaths != "")
                                                        strPaths += ",";
                                                    while (strPath != "" && strPath.EndsWith("\\") == true)
                                                        strPath = strPath.Substring(0, strPath.Length - 1);
                                                    strPaths += "\"" + strPath + "\"";
                                                }
                                                if (boolMidrange == true)
                                                {
                                                    sbSQL.Append("<td valign=\"top\"><input type=\"text\" name=\"ValidTextbox\" class=\"default\" style=\"width:125px\" onblur=\"UpdateTextPath(this,'MNT_");
                                                    sbSQL.Append(drPoint["id"].ToString());
                                                    sbSQL.Append("_PATH');\" onblur=\"BlurPath(this,'MNT_");
                                                    sbSQL.Append(drPoint["id"].ToString());
                                                    sbSQL.Append("_PATH','");
                                                    sbSQL.Append(strPath);
                                                    sbSQL.Append("');\" value=\"");
                                                    sbSQL.Append(strPath);
                                                    sbSQL.Append("\" /></td>");
                                                }
                                                else
                                                {
                                                    sbSQL.Append("<td valign=\"top\">");
                                                    sbSQL.Append(strLetter);
                                                    sbSQL.Append(":\\SH");
                                                    sbSQL.Append(dr["driveid"].ToString());
                                                    sbSQL.Append("VOL");
                                                    sbSQL.Append(intPoint < 10 ? "0" : "");
                                                    sbSQL.Append(intPoint.ToString());
                                                    sbSQL.Append("</td>");
                                                }
                                                sbSQL.Append("<td valign=\"top\">");
                                                sbSQL.Append("<select class=\"default\" onchange=\"UpdateDDL(this,'MNT_");
                                                sbSQL.Append(drPoint["id"].ToString());
                                                sbSQL.Append("_PERFORMANCE');\" style=\"width:100px;\">");
                                                sbSQL.Append("<option value=\"High\"");
                                                sbSQL.Append(drPoint["performance"].ToString() == "High" ? " selected" : "");
                                                sbSQL.Append(">High</option>");
                                                sbSQL.Append("<option value=\"Standard\"");
                                                sbSQL.Append(drPoint["performance"].ToString() == "Standard" ? " selected" : "");
                                                sbSQL.Append(">Standard</option>");
                                                sbSQL.Append("<option value=\"Low\"");
                                                sbSQL.Append(drPoint["performance"].ToString() == "Low" ? " selected" : "");
                                                sbSQL.Append(">Low</option>");
                                                sbSQL.Append("</select>");
                                                sbSQL.Append("</td>");
                                                if (boolProd == true)
                                                {
                                                    sbSQL.Append("<td valign=\"top\"><input type=\"text\" name=\"ValidTextbox0\" class=\"default\" style=\"width:75px\" onblur=\"UpdateText(this,'MNT_");
                                                    sbSQL.Append(drPoint["id"].ToString());
                                                    sbSQL.Append("_SIZE');\" value=\"");
                                                    sbSQL.Append(drPoint["size"].ToString());
                                                    sbSQL.Append("\" /> GB</td>");
                                                }
                                                else
                                                {
                                                    sbSQL.Append("<td valign=\"top\"><input type=\"text\" name=\"ValidTextbox0\" class=\"default\" style=\"width:75px\" value=\"0\" disabled=\"disabled\" /> GB</td>");
                                                }
                                                if (boolQA == true)
                                                {
                                                    sbSQL.Append("<td valign=\"top\"><input type=\"text\" name=\"ValidTextbox0\" class=\"default\" style=\"width:75px\" onblur=\"UpdateText(this,'MNT_");
                                                    sbSQL.Append(drPoint["id"].ToString());
                                                    sbSQL.Append("_SIZE_QA');\" value=\"");
                                                    sbSQL.Append(drPoint["size_qa"].ToString());
                                                    sbSQL.Append("\" /> GB</td>");
                                                }
                                                else
                                                {
                                                    sbSQL.Append("<td valign=\"top\"><input type=\"text\" name=\"ValidTextbox0\" class=\"default\" style=\"width:75px\" value=\"0\" disabled=\"disabled\" /> GB</td>");
                                                }
                                                if (boolTest == true)
                                                {
                                                    sbSQL.Append("<td valign=\"top\"><input type=\"text\" name=\"ValidTextbox0\" class=\"default\" style=\"width:75px\" onblur=\"UpdateText(this,'MNT_");
                                                    sbSQL.Append(drPoint["id"].ToString());
                                                    sbSQL.Append("_SIZE_TEST');\" value=\"");
                                                    sbSQL.Append(drPoint["size_test"].ToString());
                                                    sbSQL.Append("\" /> GB</td>");
                                                }
                                                else
                                                {
                                                    sbSQL.Append("<td valign=\"top\"><input type=\"text\" name=\"ValidTextbox0\" class=\"default\" style=\"width:75px\" value=\"0\" disabled=\"disabled\" /> GB</td>");
                                                }
                                                sbSQL.Append("<td valign=\"top\">");
                                                if (boolProd == true && boolUnder == true)
                                                {
                                                    sbSQL.Append("<select class=\"default\" onchange=\"UpdateDDL(this,'MNT_");
                                                    sbSQL.Append(drPoint["id"].ToString());
                                                    sbSQL.Append("_REPLICATED');\" style=\"width:75px;\">");
                                                    sbSQL.Append("<option value=\"Yes\"");
                                                    sbSQL.Append(drPoint["replicated"].ToString() == "1" ? " selected" : "");
                                                    sbSQL.Append(">Yes</option>");
                                                    sbSQL.Append("<option value=\"No\"");
                                                    sbSQL.Append(drPoint["replicated"].ToString() == "0" ? " selected" : "");
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
                                                sbSQL.Append("<select class=\"default\" onchange=\"UpdateDDL(this,'MNT_");
                                                sbSQL.Append(drPoint["id"].ToString());
                                                sbSQL.Append("_HIGH_AVAILABILITY');\" style=\"width:75px;\"");
                                                sbSQL.Append(boolHADisabled == true ? " disabled" : "");
                                                sbSQL.Append(">");
                                                sbSQL.Append("<option value=\"No\"");
                                                sbSQL.Append(drPoint["high_availability"].ToString() == "0" ? " selected" : "");
                                                sbSQL.Append(">No</option>");
                                                sbSQL.Append("<option value=\"Yes\"");
                                                sbSQL.Append(drPoint["high_availability"].ToString() == "1" ? " selected" : "");
                                                sbSQL.Append(">Yes</option>");
                                                sbSQL.Append("</select>");
                                                sbSQL.Append("</td>");
                                                sbSQL.Append("</tr>");
                                                sbHidden.Append("<input type=\"hidden\" name=\"MNT_");
                                                sbHidden.Append(drPoint["id"].ToString());
                                                sbHidden.Append("_PATH\" id=\"MNT_");
                                                sbHidden.Append(drPoint["id"].ToString());
                                                sbHidden.Append("_PATH\" value=\"");
                                                sbHidden.Append(strPath);
                                                sbHidden.Append("\" />");
                                                sbHidden.Append("<input type=\"hidden\" name=\"MNT_");
                                                sbHidden.Append(drPoint["id"].ToString());
                                                sbHidden.Append("_PERFORMANCE\" id=\"MNT_");
                                                sbHidden.Append(drPoint["id"].ToString());
                                                sbHidden.Append("_PERFORMANCE\" value=\"");
                                                sbHidden.Append(drPoint["performance"].ToString());
                                                sbHidden.Append("\" />");
                                                if (boolProd == true)
                                                {
                                                    sbHidden.Append("<input type=\"hidden\" name=\"MNT_");
                                                    sbHidden.Append(drPoint["id"].ToString());
                                                    sbHidden.Append("_SIZE\" id=\"MNT_");
                                                    sbHidden.Append(drPoint["id"].ToString());
                                                    sbHidden.Append("_SIZE\" value=\"");
                                                    sbHidden.Append(drPoint["size"].ToString());
                                                    sbHidden.Append("\" />");
                                                }
                                                else
                                                {
                                                    sbHidden.Append("<input type=\"hidden\" name=\"MNT_");
                                                    sbHidden.Append(drPoint["id"].ToString());
                                                    sbHidden.Append("_SIZE\" id=\"MNT_");
                                                    sbHidden.Append(drPoint["id"].ToString());
                                                    sbHidden.Append("_SIZE\" value=\"0\" />");
                                                }
                                                if (boolQA == true)
                                                {
                                                    sbHidden.Append("<input type=\"hidden\" name=\"MNT_");
                                                    sbHidden.Append(drPoint["id"].ToString());
                                                    sbHidden.Append("_SIZE_QA\" id=\"MNT_");
                                                    sbHidden.Append(drPoint["id"].ToString());
                                                    sbHidden.Append("_SIZE_QA\" value=\"");
                                                    sbHidden.Append(drPoint["size_qa"].ToString());
                                                    sbHidden.Append("\" />");
                                                }
                                                else
                                                {
                                                    sbHidden.Append("<input type=\"hidden\" name=\"MNT_");
                                                    sbHidden.Append(drPoint["id"].ToString());
                                                    sbHidden.Append("_SIZE_QA\" id=\"MNT_");
                                                    sbHidden.Append(drPoint["id"].ToString());
                                                    sbHidden.Append("_SIZE_QA\" value=\"0\" />");
                                                }
                                                if (boolTest == true)
                                                {
                                                    sbHidden.Append("<input type=\"hidden\" name=\"MNT_");
                                                    sbHidden.Append(drPoint["id"].ToString());
                                                    sbHidden.Append("_SIZE_TEST\" id=\"MNT_");
                                                    sbHidden.Append(drPoint["id"].ToString());
                                                    sbHidden.Append("_SIZE_TEST\" value=\"");
                                                    sbHidden.Append(drPoint["size_test"].ToString());
                                                    sbHidden.Append("\" />");
                                                }
                                                else
                                                {
                                                    sbHidden.Append("<input type=\"hidden\" name=\"MNT_");
                                                    sbHidden.Append(drPoint["id"].ToString());
                                                    sbHidden.Append("_SIZE_TEST\" id=\"MNT_");
                                                    sbHidden.Append(drPoint["id"].ToString());
                                                    sbHidden.Append("_SIZE_TEST\" value=\"0\" />");
                                                }
                                                if (boolProd == true && boolUnder == true)
                                                {
                                                    sbHidden.Append("<input type=\"hidden\" name=\"MNT_");
                                                    sbHidden.Append(drPoint["id"].ToString());
                                                    sbHidden.Append("_REPLICATED\" id=\"MNT_");
                                                    sbHidden.Append(drPoint["id"].ToString());
                                                    sbHidden.Append("_REPLICATED\" value=\"");
                                                    sbHidden.Append(drPoint["replicated"].ToString() == "1" ? "Yes" : "No");
                                                    sbHidden.Append("\" />");
                                                }
                                                else
                                                {
                                                    sbHidden.Append("<input type=\"hidden\" name=\"MNT_");
                                                    sbHidden.Append(drPoint["id"].ToString());
                                                    sbHidden.Append("_REPLICATED\" id=\"MNT_");
                                                    sbHidden.Append(drPoint["id"].ToString());
                                                    sbHidden.Append("_REPLICATED\" value=\"No\" />");
                                                }
                                                sbHidden.Append("<input type=\"hidden\" name=\"MNT_");
                                                sbHidden.Append(drPoint["id"].ToString());
                                                sbHidden.Append("_HIGH_AVAILABILITY\" id=\"MNT_");
                                                sbHidden.Append(drPoint["id"].ToString());
                                                sbHidden.Append("_HIGH_AVAILABILITY\" value=\"");
                                                sbHidden.Append(drPoint["high_availability"].ToString() == "1" ? "Yes" : "No");
                                                sbHidden.Append("\" />");
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (boolOverride == false)
            {
                btnAdd.Attributes.Add("onclick", "EnsureTextbox() && EnsureTextbox0()");
                btnSave.Attributes.Add("onclick", "return ValidateText('" + txtName.ClientID + "','Please enter a custom name for this cluster') && EnsureTextbox() && EnsureTextbox0();");
            }
            else
            {
                btnSave.Attributes.Add("onclick", "return ValidateText('" + txtName.ClientID + "','Please enter a custom name for this cluster') && EnsureValidText() && EnsureTextbox0();");
                btnAdd.Attributes.Add("onclick", "return EnsureValidText() && EnsureTextbox0()");
            }

            btnClose.Attributes.Add("onclick", "return window.close();");
            btnDenied.Attributes.Add("onclick", "return window.close();");

            strSQL = sbSQL.ToString();
            strHidden = sbHidden.ToString();
        }
        protected void btnAdd_Click(Object Sender, EventArgs e)
        {
            if (btnAdd.CommandArgument == "ADDLUN")
            {
                foreach (string strForm in Request.Form)
                {
                    if (strForm.StartsWith("HDN_") == true)
                    {
                        int intID = Int32.Parse(strForm.Substring(4, strForm.IndexOf("_", 4) - 4));
                        oStorage.UpdateLun(intID, Request.Form["HDN_" + intID.ToString() + "_PATH"], Request.Form["HDN_" + intID.ToString() + "_PERFORMANCE"], double.Parse(Request.Form["HDN_" + intID.ToString() + "_SIZE"]), double.Parse(Request.Form["HDN_" + intID.ToString() + "_SIZE_QA"]), double.Parse(Request.Form["HDN_" + intID.ToString() + "_SIZE_TEST"]), (Request.Form["HDN_" + intID.ToString() + "_REPLICATED"] == "Yes" ? 1 : 0), (Request.Form["HDN_" + intID.ToString() + "_HIGH_AVAILABILITY"] == "Yes" ? 1 : 0));
                    }
                }
                oStorage.AddLun(intAnswer, intInstance, intCluster, 0, 0, 0, (boolProd ? 1.00 : 0.00), (boolQA ? 1.00 : 0.00), (boolTest ? 1.00 : 0.00));
                Response.Redirect(Request.Path + "?aid=" + intAnswer + "&cid=" + intCluster + "&id=" + intInstance + "&refresh=true");


            }
            else
            {
                foreach (string strForm in Request.Form)
                {
                    if (strForm.StartsWith("HDN_") == true)
                    {
                        int intID = Int32.Parse(strForm.Substring(4, strForm.IndexOf("_", 4) - 4));
                        oStorage.UpdateLun(intID, Request.Form["HDN_" + intID.ToString() + "_PATH"], Request.Form["HDN_" + intID.ToString() + "_PERFORMANCE"], double.Parse(Request.Form["HDN_" + intID.ToString() + "_SIZE"]), double.Parse(Request.Form["HDN_" + intID.ToString() + "_SIZE_QA"]), double.Parse(Request.Form["HDN_" + intID.ToString() + "_SIZE_TEST"]), (Request.Form["HDN_" + intID.ToString() + "_REPLICATED"] == "Yes" ? 1 : 0), (Request.Form["HDN_" + intID.ToString() + "_HIGH_AVAILABILITY"] == "Yes" ? 1 : 0));
                    }
                    if (strForm.StartsWith("MNT_") == true)
                    {
                        int intMountID = Int32.Parse(strForm.Substring(4, strForm.IndexOf("_", 4) - 4));
                        oStorage.UpdateMountPoint(intMountID, Request.Form["MNT_" + intMountID.ToString() + "_PATH"], Request.Form["MNT_" + intMountID.ToString() + "_PERFORMANCE"], double.Parse(Request.Form["MNT_" + intMountID.ToString() + "_SIZE"]), double.Parse(Request.Form["MNT_" + intMountID.ToString() + "_SIZE_QA"]), double.Parse(Request.Form["MNT_" + intMountID.ToString() + "_SIZE_TEST"]), (Request.Form["MNT_" + intMountID.ToString() + "_REPLICATED"] == "Yes" ? 1 : 0), (Request.Form["MNT_" + intMountID.ToString() + "_HIGH_AVAILABILITY"] == "Yes" ? 1 : 0));
                    }
                }
                oStorage.AddMountPoint(Int32.Parse(lblLun.Text), "", "Standard", 0.00, 0.00, 0.00, 0, 0);
                Response.Redirect(Request.Path + "?aid=" + intAnswer + "&cid=" + intCluster + "&id=" + intInstance + "&refresh=true");
            }
        }
        protected void btnSave_Click(Object Sender, EventArgs e)
        {
            oCluster.UpdateInstance(intInstance, txtName.Text);
            foreach (string strForm in Request.Form)
            {
                if (strForm.StartsWith("HDN_") == true)
                {
                    int intID = Int32.Parse(strForm.Substring(4, strForm.IndexOf("_", 4) - 4));
                    oStorage.UpdateLun(intID, Request.Form["HDN_" + intID.ToString() + "_PATH"], Request.Form["HDN_" + intID.ToString() + "_PERFORMANCE"], double.Parse(Request.Form["HDN_" + intID.ToString() + "_SIZE"]), double.Parse(Request.Form["HDN_" + intID.ToString() + "_SIZE_QA"]), double.Parse(Request.Form["HDN_" + intID.ToString() + "_SIZE_TEST"]), (Request.Form["HDN_" + intID.ToString() + "_REPLICATED"] == "Yes" ? 1 : 0), (Request.Form["HDN_" + intID.ToString() + "_HIGH_AVAILABILITY"] == "Yes" ? 1 : 0));
                }
                if (strForm.StartsWith("MNT_") == true)
                {
                    int intMountID = Int32.Parse(strForm.Substring(4, strForm.IndexOf("_", 4) - 4));
                    oStorage.UpdateMountPoint(intMountID, Request.Form["MNT_" + intMountID.ToString() + "_PATH"], Request.Form["MNT_" + intMountID.ToString() + "_PERFORMANCE"], double.Parse(Request.Form["MNT_" + intMountID.ToString() + "_SIZE"]), double.Parse(Request.Form["MNT_" + intMountID.ToString() + "_SIZE_QA"]), double.Parse(Request.Form["MNT_" + intMountID.ToString() + "_SIZE_TEST"]), (Request.Form["MNT_" + intMountID.ToString() + "_REPLICATED"] == "Yes" ? 1 : 0), (Request.Form["MNT_" + intMountID.ToString() + "_HIGH_AVAILABILITY"] == "Yes" ? 1 : 0));
                }
            }
            oCluster.UpdateQuorum(intCluster, 1);
            Response.Redirect(Request.Path + "?aid=" + intAnswer + "&cid=" + intCluster + "&id=" + intInstance + "&save=true");
        }
    }
}
