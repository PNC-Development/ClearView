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
namespace NCC.ClearView.Presentation.Web
{
    public partial class workstation_vmware_control : System.Web.UI.UserControl
    {

        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        private string dsnRemote = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["RemoteDSN"]].ConnectionString;
        private string dsnZeus = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ZeusDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intImplementorDistributed = Int32.Parse(ConfigurationManager.AppSettings["ITEMID_IMPLEMENTOR_DISTRIBUTED"]);
        protected int intImplementorMidrange = Int32.Parse(ConfigurationManager.AppSettings["ITEMID_IMPLEMENTOR_MIDRANGE"]);
        protected int intWorkstationPlatform = Int32.Parse(ConfigurationManager.AppSettings["WorkstationPlatformID"]);
        protected int intProfile;
        protected int intApplication = 0;
        protected int intPage = 0;
        protected Asset oAsset;
        protected Forecast oForecast;
        protected Workstations oWorkstation;
        protected ServiceRequests oServiceRequest;
        protected OnDemandTasks oOnDemandTasks;
        protected int intID = 0;
        protected int intRequest = 0;
        protected string strMenuTab1 = "";
        protected string strDivs = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oAsset = new Asset(intProfile, dsnAsset);
            oForecast = new Forecast(intProfile, dsn);
            oWorkstation = new Workstations(intProfile, dsn);
            oServiceRequest = new ServiceRequests(intProfile, dsn);
            oOnDemandTasks = new OnDemandTasks(intProfile, dsn);

            //Menu
            int intMenuTab = 0;
            if (Request.QueryString["menu_tab"] != null && Request.QueryString["menu_tab"] != "")
                intMenuTab = Int32.Parse(Request.QueryString["menu_tab"]);
            Tab oTab = new Tab("", intMenuTab, "divMenu1", true, false);
            //End Menu



            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            int intForecast = 0;
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
            {
                intID = Int32.Parse(Request.QueryString["id"]);
                intForecast = Int32.Parse(oForecast.GetAnswer(intID, "forecastid"));
                intRequest = oForecast.GetRequestID(intID, true);
                DataSet dsRequest = oServiceRequest.Get(intRequest);
                if (dsRequest.Tables[0].Rows.Count > 0)
                    Response.Redirect(Request.Path + "?rid=" + intRequest);
            }
            if (Request.QueryString["rid"] != null && Request.QueryString["rid"] != "")
                intRequest = Int32.Parse(Request.QueryString["rid"]);
            if (Request.QueryString["notify"] != null)
            {
                string strRedirect = Request.Url.PathAndQuery;
                strRedirect = strRedirect.Substring(0, strRedirect.IndexOf("&notify"));
                if (Request.QueryString["notify"] == "none")
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "close", "<script type=\"text/javascript\">window.navigate('" + strRedirect + "');<" + "/" + "script>");
                else if (Request.QueryString["notify"] == "true")
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "close", "<script type=\"text/javascript\">alert('Your design implementor has been successfully notified!');window.navigate('" + strRedirect + "');<" + "/" + "script>");
                else if (Request.QueryString["notify"] == "false")
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "close", "<script type=\"text/javascript\">alert('Your design implementor has ALREADY been notified!');window.navigate('" + strRedirect + "');<" + "/" + "script>");
                else
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "close", "<script type=\"text/javascript\">alert('There was a problem notifying your design implementor\\n\\nPlease contact your ClearView administrator\\n\\nRequestID: " + intRequest.ToString() + "');window.navigate('" + strRedirect + "');<" + "/" + "script>");
            }
            if (intID > 0)
            {
                string strSchedule = oForecast.GetAnswer(intID, "execution");
                if (strSchedule != "")
                {
                    DateTime datSchedule = DateTime.Parse(strSchedule);
                    txtScheduleDate.Text = datSchedule.ToShortDateString();
                    txtScheduleTime.Text = datSchedule.ToShortTimeString();
                    btnSchedule.Text = "Update the Build";
                }
                panBegin.Visible = true;
                bool boolAssigned = false;

                Requests oRequest = new Requests(intProfile, dsn);
                if (oForecast.CanAutoProvision(intID) == true)
                    boolAssigned = true;
                else if (oOnDemandTasks.GetPending(intID).Tables[0].Rows.Count > 0)
                    boolAssigned = true;

                bool boolVMware = false;

                DataSet ds = oWorkstation.GetVirtualRequests(intRequest);
                if (ds.Tables[0].Rows.Count > 0)
                    boolVMware = (ds.Tables[0].Rows[0]["vmware"].ToString() == "1");

                lblVMware.Text = (boolVMware ? "1" : "0");
                if (boolVMware == true)
                    panSchedule.Visible = true;

                if (intForecast == 0)
                    btnStart.Attributes.Add("onclick", "return ProcessButton(this);");
                else if (boolAssigned == false)
                {
                    btnStart.Attributes.Add("onclick", "alert('You cannot execute a design until a design implementor has been assigned.');return false;");
                    btnSchedule.Attributes.Add("onclick", "alert('You cannot schedule the execution of a design until a design implementor has been assigned.');return false;");
                }
                else
                {
                    lblNotify.Visible = true;
                    //btnStart.Attributes.Add("onclick", "return confirm('NOTE: Billing will begin: " + DateTime.Today.ToShortDateString() + "\\n\\nAre you sure you want to continue?') && OpenWindow('AUTO_PROVISIONING','?answerid=" + intID.ToString() + "&modelid=" + oForecast.GetModel(intID).ToString() + "');");
                    btnStart.Attributes.Add("onclick", "return confirm('NOTE: Billing will begin: " + DateTime.Today.ToShortDateString() + "\\n\\nAre you sure you want to continue?');");
                    btnSchedule.Attributes.Add("onclick", "return ValidateDate('" + txtScheduleDate.ClientID + "','Please enter a valid schedule date')" +
                        " && ValidateDateToday('" + txtScheduleDate.ClientID + "','The scheduled date must occur after today')" +
                        " && ValidateTime('" + txtScheduleTime.ClientID + "','Please enter a valid start time')" +
                        " && confirm('NOTE: Billing will begin: ' + document.getElementById('" + txtScheduleDate.ClientID + "').value + '\\n\\nAre you sure you want to continue?')" +
                        " && ProcessButton(this)" +
                        ";");
                }

                imgScheduleDate.Attributes.Add("onclick", "return ShowCalendar('" + txtScheduleDate.ClientID + "');");
            }
            else if (intRequest > 0)
            {
                intRequest = Int32.Parse(Request.QueryString["rid"]);
                DateTime datSchedule = DateTime.Now;
                DataSet ds = oWorkstation.GetVirtualRequests(intRequest);
                bool boolPending = false;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    int intAnswer = Int32.Parse(ds.Tables[0].Rows[0]["answerid"].ToString());
                    string strSchedule = oForecast.GetAnswer(intAnswer, "execution");
                    if (strSchedule != "")
                    {
                        datSchedule = DateTime.Parse(strSchedule);
                        lblScheduleDate.Text = datSchedule.ToShortDateString();
                        lblScheduleTime.Text = datSchedule.ToShortTimeString();
                        if (DateTime.Now < datSchedule)
                            boolPending = true;
                    }
                }
                if (boolPending == true)
                {
                    panPending.Visible = true;
                    TimeSpan oSpan = datSchedule.Subtract(DateTime.Now);
                    if (oSpan.Seconds > 0)
                        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "cdown", "<script type=\"text/javascript\">StartClockCountdown('" + lblCountdown.ClientID + "'," + oSpan.TotalMilliseconds.ToString() + ");<" + "/" + "script>");
                    else
                        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "cdown", "<script type=\"text/javascript\">StartClockCountdown('" + lblCountdown.ClientID + "',0);<" + "/" + "script>");
                }
                else
                {
                    panStart.Visible = true;
                    int intCount = 0;
                    Functions oFunction = new Functions(intProfile, dsn, intEnvironment);
                   
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        intCount++;
                        int intDevice = Int32.Parse(dr["id"].ToString());
                        int intAsset = Int32.Parse(dr["assetid"].ToString());
                        string strName = "Device " + intCount.ToString();
                        if (intAsset > 0)
                        {
                            string strTempName = oAsset.Get(intAsset, "name");
                            if (strTempName != "")
                                strName = strTempName;
                        }
                        if (intCount == 1)
                        {
                            //strTab += "<td><img src=\"/images/TabOnLeftCap.gif\"></td><td nowrap background=\"/images/TabOnBackground.gif\"><a href=\"javascript:void(0);\" onclick=\"ChangeTab(this,'divTab" + intCount.ToString() + "',null,null,true);\" class=\"tabheader\">" + strName + "</a></td><td><img src=\"/images/TabOnRightCap.gif\"></td>";
                            oTab.AddTab(strName, "");
                            strDivs += "<div id=\"divTab" + intCount.ToString() + "\" style=\"display:inline\"><iframe width=\"100%\" height=\"100%\" frameborder=\"0\" scrolling=\"auto\" src=\"/ondemand/ondemand_workstation_virtual.aspx?id=" + oFunction.encryptQueryString(intDevice.ToString()) + "&c=" + intCount.ToString() + "\"></iframe></div>";
                        }
                        else
                        {
                            // strTab += "<td><img src=\"/images/TabOffLeftCap.gif\"></td><td nowrap background=\"/images/TabOffBackground.gif\"><a href=\"javascript:void(0);\" onclick=\"ChangeTab(this,'divTab" + intCount.ToString() + "',null,null,true);\" class=\"tabheader\">" + strName + "</a></td><td><img src=\"/images/TabOffRightCap.gif\"></td>";
                            oTab.AddTab(strName, "");
                            strDivs += "<div id=\"divTab" + intCount.ToString() + "\" style=\"display:none\"><iframe width=\"100%\" height=\"100%\" frameborder=\"0\" scrolling=\"auto\" src=\"/ondemand/ondemand_workstation_virtual.aspx?id=" + oFunction.encryptQueryString(intDevice.ToString()) + "&c=" + intCount.ToString() + "\"></iframe></div>";
                        }
                    }
                    //if (strTab != "")
                    //    strMenuTab1 += "<tr>" + strTab + "<td width=\"100%\" background=\"/images/TabEmptyBackground.gif\">&nbsp;</td></tr>";
                    //strMenuTab1 = "<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\">" + strMenuTab1 + "</table>";
                    strMenuTab1 = oTab.GetTabs();
                }
            }
        }
        protected void btnStart_Click(Object Sender, EventArgs e)
        {
            oForecast.UpdateAnswerExecution(intID, "");
            Start(false);
        }
        protected void btnSchedule_Click(Object Sender, EventArgs e)
        {
            DateTime datSchedule = DateTime.Parse(txtScheduleDate.Text + " " + txtScheduleTime.Text);
            oForecast.UpdateAnswerExecution(intID, datSchedule.ToString());
            Start(true);
        }
        private void Start(bool _schedule)
        {
            bool boolVMware = (lblVMware.Text == "1");
            intRequest = oForecast.GetRequestID(intID, true);
            oForecast.UpdateAnswer(intID, intRequest);
            oForecast.DeleteReset(intID);
            oServiceRequest.Add(intRequest, 1, 1);
            oForecast.UpdateAnswerExecuted(intID, DateTime.Now.ToString(), intProfile);
            if (boolVMware == true)
                oWorkstation.StartVirtual(intRequest);
            else
                oWorkstation.StartVirtual(intRequest, dsnRemote, dsnAsset, intEnvironment, dsnZeus);
            int intModel = oForecast.GetModelAsset(intID);
            if (intModel == 0)
                intModel = oForecast.GetModel(intID);
            bool boolNotify = oForecast.NotifyImplementor(intID, intModel, intImplementorDistributed, intWorkstationPlatform, intImplementorMidrange, intEnvironment, intProfile, dsnAsset, dsnIP);
            if (_schedule == true)
                Response.Redirect(Request.Path + "?rid=" + intRequest);
            else if (oForecast.CanAutoProvision(intID) == true)
                Response.Redirect(Request.Path + "?rid=" + intRequest + "&notify=none");
            else
                Response.Redirect(Request.Path + "?rid=" + intRequest + "&notify=" + (boolNotify ? "true" : "false"));
        }
        protected void btnBack_Click(Object Sender, EventArgs e)
        {
            oForecast.UpdateAnswerExecution(intID, "");
            int intModel = oForecast.GetModel(intID);
            ModelsProperties oModelsProperties = new ModelsProperties(0, dsn);
            int intType = oModelsProperties.GetType(intModel);
            Types oType = new Types(0, dsn);
            string strExecute = oType.Get(intType, "forecast_execution_path");
            OnDemand oOnDemand = new OnDemand(0, dsn);
            int intCount = 0;
            DataSet ds = oOnDemand.GetWizardStepsDoneBack(intID);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                intCount++;
                if (intCount != ds.Tables[0].Rows.Count)
                    oOnDemand.DeleteWizardStepDone(intID, Int32.Parse(dr["step"].ToString()));
            }
            Response.Redirect(strExecute + "?id=" + intID.ToString());
        }
    }
}