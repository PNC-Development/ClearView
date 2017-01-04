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
    public partial class datapoint_projects : BasePage
    {

        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intDataPointAvailableProject = Int32.Parse(ConfigurationManager.AppSettings["DATAPOINT_AVAILABLE_PROJECT"]);
        protected bool boolDataPointProjectShowAdditional = (ConfigurationManager.AppSettings["DATAPOINT_PROJECT_SHOW_ADDITIONAL"] == "1");
        protected DataPoint oDataPoint;
        protected Users oUser;
        protected Functions oFunction;
        protected Services oService;
        protected ServiceRequests oServiceRequest;
        protected Log oLog;
        protected Projects oProject;
        private int intProfile = 0;
        private int intApplication = 0;
        private int intProjectId = 0;
        protected string strMenuTab1 = "";
        protected string strDivs = "";
        protected string strMenuDivs = "";
        protected string strHTMLProjectPriority = "";
        protected Tab oTabProvisioning;
        private int intCount = 1;
        private int intDebug = 0;
      
        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            if (Request.Cookies["profileid"] != null && Request.Cookies["profileid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            
            oDataPoint = new DataPoint(intProfile, dsn);
            oUser = new Users(intProfile, dsn);
            oFunction = new Functions(intProfile, dsn, intEnvironment);
            oService = new Services(intProfile, dsn);
            oServiceRequest = new ServiceRequests(intProfile, dsn);
            oLog = new Log(intProfile, dsn);
            oProject = new Projects(intProfile, dsn);
            
            if (oUser.IsAdmin(intProfile) == true || (oDataPoint.GetPagePermission(intApplication, "PROJECT") == true || intDataPointAvailableProject == 1))
            {
                if (Request.QueryString["debug"] != null && Request.QueryString["debug"] != "")
                {
                    intDebug = Int32.Parse(Request.QueryString["debug"]);
                }
                if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
                {
                    intProjectId = Int32.Parse( oFunction.decryptQueryString(Request.QueryString["id"]));
                    oLog.AddEvent("PROJECT_SEARCH", "", oProject.Get(intProjectId, "number") + ": LoadData started at " + DateTime.Now.ToString(), LoggingType.Debug);
                    LoadData();
                    oLog.AddEvent("PROJECT_SEARCH", "", oProject.Get(intProjectId, "number") + ": LoadData finished at " + DateTime.Now.ToString(), LoggingType.Debug);
                }
                pnlAllow.Visible = true;

                //Tabs
                int intMenuTab = 0;
             
                Tab oTab = new Tab(hdnTab.ClientID, intMenuTab, "divMenu1", true, false);

                oTab.AddTab("Project Information", "");
                oTab.AddTab("Request", "");
                oTab.AddTab("Financials", "");
                oTab.AddTab("Priority", "");
                oTab.AddTab("Documents", "");
                oTab.AddTab("Scorecard", "");
                oTab.AddTab("History", "");
                oTab.AddTab("Assets", "");
                if (boolDataPointProjectShowAdditional == true)
                {
                    oTab.AddTab("Service(s) Progression", "");
                    oTab.AddTab("Resource(s) Involvement", "");
                }
                oTab.AddTab("Provisioning Status", "");
                
                strMenuTab1 = oTab.GetTabs();
            }
            else
                pnlDenied.Visible = true;

        }

        private void LoadData()
        {
            DataSet dsProject;
            int intMenuTab1 = 0;
            oTabProvisioning = new Tab(hdnProvisioningTab.ClientID, intMenuTab1, "divMenu2", false, false);

            DateTime datStart = DateTime.Now;
           
            dsProject = oDataPoint.GetProjectSearchResults(
                                intProjectId,
                                null,
                                null,
                                null,
                                null, null, null, null, null, null, null, null,
                                null, 0, 1, 0);
            
            if (dsProject.Tables[0].Rows.Count == 1)
            {
                DataRow dr = dsProject.Tables[0].Rows[0];
                Master.Page.Title = "DataPoint | Project ( " + dr["ProjectName"].ToString() + " )";
                lblProjectNumber.Text=  (dr["ProjectNumber"]!=DBNull.Value?dr["ProjectNumber"].ToString():"") ;
                

                lblProjectName.Text = (dr["ProjectName"] != DBNull.Value ? dr["ProjectName"].ToString() : "");
                lblProjectName.ToolTip =  dr["ProjectId"].ToString();

                string strProjectManager = "";
                strProjectManager= (dr["ProjectLeadName"] != DBNull.Value ? dr["ProjectLeadName"].ToString() : "");
                strProjectManager += (dr["ProjectLeadXID"].ToString() != "" ? "(" + dr["ProjectLeadXID"].ToString() + ")" : "");
                lblProjectManager.Text = strProjectManager;
                lblProjectManager.Attributes.Add("onclick", "return OpenWindow('PROFILE','?userid=" + dr["ProjectLead"].ToString() + "');");
                

                lblProjectType.Text = (dr["ProjectBaseDiscretion"] != DBNull.Value ? dr["ProjectBaseDiscretion"].ToString() : "");
                lblProjectStatus.Text = (dr["ProjectStatus"] != DBNull.Value ? dr["ProjectStatus"].ToString() : "");

                string strExecutiveSponsor = "";
                strExecutiveSponsor = (dr["ProjectExecutiveSponsorName"] != DBNull.Value ? dr["ProjectExecutiveSponsorName"].ToString() : "");
                strExecutiveSponsor += (dr["ProjectExecutiveSponsorXID"].ToString() != "" ? "(" + dr["ProjectExecutiveSponsorXID"].ToString() + ")" : "");
                lblExecutiveSponsor.Text = strExecutiveSponsor;
                lblExecutiveSponsor.Attributes.Add("onclick", "return OpenWindow('PROFILE','?userid=" + dr["ProjectExecutiveSponsorID"].ToString() + "');");

                string strWorkingSponsor = "";
                strWorkingSponsor= (dr["ProjectWorkingSponsorName"] != DBNull.Value ? dr["ProjectWorkingSponsorName"].ToString() : "");
                strWorkingSponsor += (dr["ProjectWorkingSponsorXID"].ToString() != "" ? "(" + dr["ProjectWorkingSponsorXID"].ToString() + ")" : "");
                lblWorkingSponsor.Text = strWorkingSponsor;
                lblWorkingSponsor.Attributes.Add("onclick", "return OpenWindow('PROFILE','?userid=" + dr["ProjectWorkingSponsorID"].ToString() + "');");

                //Get Project Info Details
                if (intDebug == 0 || intDebug >= 1)
                    ucProjectInfo.ProjectId = Int32.Parse(dr["ProjectId"].ToString());
                else
                    ucProjectInfo.Visible = false;

                //Get Project Request Details
                if (intDebug == 0 || intDebug >= 2)
                    ucProjectRequestDetails.ProjectId = Int32.Parse(dr["ProjectId"].ToString());
                else
                    ucProjectRequestDetails.Visible = false;

                //Get Project Financials
                if (intDebug == 0 || intDebug >= 3)
                    ucProjectFinancials.ProjectId = Int32.Parse(dr["ProjectId"].ToString());
                else
                    ucProjectFinancials.Visible = false;

                // Get Project Priority
                if (intDebug == 0 || intDebug >= 4)
                {
                    ProjectRequest oProjectRequest;
                    oProjectRequest = new ProjectRequest(intProfile, dsn);
                    DataSet ds1 = oProjectRequest.GetProjectRequestDetails(intProjectId);
                    if (ds1.Tables.Count != 0 && ds1.Tables[0].Rows.Count > 0)
                    {
                        if (ds1.Tables[0].Rows[0]["RequestID"].ToString() != "0")
                            strHTMLProjectPriority = oProjectRequest.GetPriority(Int32.Parse(ds1.Tables[0].Rows[0]["RequestID"].ToString()), intEnvironment);
                    }
                }

                // Get Project Documents

                // Get Project Scorecard

                // Get Project History

                //Get Project Assets
                if (intDebug == 0 || intDebug >= 5)
                {
                    DataSet dsProjectAsset = oDataPoint.GetProjectAssets(intProjectId);
                    rptAssets.DataSource = dsProjectAsset.Tables[0];
                    rptAssets.DataBind();
                    //rptAssets.Visible = (rptAssets.Items.Count > 0);
                    lblAssets.Visible = (rptAssets.Items.Count == 0);
                    foreach (RepeaterItem ri in rptAssets.Items)
                    {
                        Label lblName = (Label)ri.FindControl("lblName");
                        Label lblSerial = (Label)ri.FindControl("lblSerial");
                        string strURL = oDataPoint.GetAssetSerialOrTag(lblSerial.Text, "", "url");
                        lblSerial.Text = "<a href=\"javascript:void(0);\" onclick=\"OpenNewWindowMenu('/datapoint/asset/" + strURL + ".aspx?t=serial&q=" + oFunction.encryptQueryString(lblSerial.Text) + "&id=" + oFunction.encryptQueryString(lblSerial.ToolTip) + "', '800', '600');\" target=\"_blank\">" + lblSerial.Text + "</a>";
                        Label lblStatus = (Label)ri.FindControl("lblStatus");
                        Label lblErrorStep = (Label)ri.FindControl("lblErrorStep");
                        Label lblErrorReason = (Label)ri.FindControl("lblErrorReason");
                        if (lblErrorStep.Text != "")
                            lblStatus.Text = "<img src='/images/alert.gif' border='0' align='absmiddle'> " + "( Step #" + lblErrorStep.Text + ")" + lblErrorReason.Text;
                        Label lblDesign = (Label)ri.FindControl("lblDesign");
                        lblDesign.Text = "<a href=\"javascript:void(0);\" onclick=\"OpenNewWindowMenu('/datapoint/service/design.aspx?q=" + oFunction.encryptQueryString(lblDesign.Text) + "', '800', '600');\" target=\"_blank\">" + lblDesign.Text + "</a>";

                        oTabProvisioning.AddTab(lblName.Text, "");
                        strDivs += "<div  id=\"divTab" + intCount.ToString() + "\" style=\"display:inline\"><iframe width=\"100%\" height=\"100%\" frameborder=\"0\" scrolling=\"auto\" src=\"/datapoint/asset/provisioning_status.aspx" + "?id=" + oFunction.encryptQueryString(lblName.ToolTip) + "&c=" + intCount.ToString() + "\"></iframe></div>";
                        intCount++;

                        lblName.Text = "<a href=\"javascript:void(0);\" onclick=\"OpenNewWindowMenu('/datapoint/asset/server.aspx?t=name " + "&q=" + oFunction.encryptQueryString(lblName.Text) + "', '800', '600');\" target=\"_blank\">" + lblName.Text + "</a>";
                    }
                }

                if (boolDataPointProjectShowAdditional == true)
                {

                    //ucResourceInvolvement.Visible = false;

                    //Get Service Progression 
                    if (intDebug == 0 || intDebug >= 6)
                        ucServiceProgression.ProjectId = Int32.Parse(dr["ProjectId"].ToString());
                    else
                        ucServiceProgression.Visible = false;

                    //Get Resource Involvement
                    if (intDebug == 0 || intDebug >= 7)
                        ucResourceInvolvement.ProjectId = Int32.Parse(dr["ProjectId"].ToString());
                    else
                        ucResourceInvolvement.Visible = false;
                }
                else
                {
                    ucServiceProgression.Visible = false;
                    ucResourceInvolvement.Visible = false;
                }

                strMenuDivs = oTabProvisioning.GetTabs();


            }
        }

        protected void dlAssets_ItemDataBound(object sender, DataListItemEventArgs e) 
        {
            ModelsProperties oModelsProperties;
             oModelsProperties = new ModelsProperties(intProfile, dsn);
             Types oType = new Types(intProfile, dsn);
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;


                HiddenField hdnAssetId = (HiddenField)e.Item.FindControl("hdnAssetId");
                hdnAssetId.Value = drv["AssetId"].ToString();

                LinkButton lnkDeviceName = (LinkButton)e.Item.FindControl("lnkDeviceName");
                lnkDeviceName.Text = drv["ServerName"].ToString();
                lnkDeviceName.ToolTip = "Server Id: " + drv["serverid"].ToString();

                lnkDeviceName.Attributes.Add("onclick", "return OpenNewWindowMenu('/datapoint/asset/server.aspx?t=name " + "&q=" + oFunction.encryptQueryString(drv["ServerName"].ToString()) + "', '800', '600');");

                LinkButton lnkDeviceSerial = (LinkButton)e.Item.FindControl("lnkDeviceSerial");
                lnkDeviceSerial.Text = (drv["Serial"] != DBNull.Value ? drv["Serial"].ToString() : "--");
                if (drv["Serial"] != DBNull.Value)
                {
                    string strURL = oDataPoint.GetAssetSerialOrTag(drv["Serial"].ToString(), "", "url");
                    lnkDeviceSerial.Attributes.Add("onclick", "return OpenNewWindowMenu('/datapoint/asset/" + strURL + ".aspx?t=serial&q=" + oFunction.encryptQueryString(drv["Serial"].ToString()) + "&id=" + oFunction.encryptQueryString(drv["AssetId"].ToString()) + "', '800', '600');");
                }

                Label lblDeviceModel = (Label)e.Item.FindControl("lblDeviceModel");
                lblDeviceModel.Text = (drv["ModelName"] != DBNull.Value ? drv["ModelName"].ToString() : "--");
                lblDeviceModel.ToolTip = "Model #: " + drv["ModelId"].ToString();

                Label lblStatus = (Label)e.Item.FindControl("lblStatus");
                lblStatus.Text = (drv["DesignStatusName"] != DBNull.Value ? drv["DesignStatusName"].ToString() : "--");
                if (drv["ErrorStep"]!= DBNull.Value)
                {
                  
                    lblStatus.Text="<img src='/images/alert.gif' border='0' align='absmiddle'> " + "( Step #"+drv["ErrorStep"].ToString()+")"+drv["ErrorReason"].ToString();
                }
                

                LinkButton lnkDesign = (LinkButton)e.Item.FindControl("lnkDesign");
                lnkDesign.Text = (drv["DesignName"] != DBNull.Value ? drv["DesignName"].ToString() : "") + " # " +
                                 (drv["ForecastAnswerId"] != DBNull.Value ? drv["ForecastAnswerId"].ToString() : "");
                lnkDesign.Attributes.Add("onclick", "return OpenNewWindowMenu('/datapoint/service/design.aspx?q=" + oFunction.encryptQueryString(drv["ForecastAnswerId"].ToString()) + "', '800', '600');");

           
                Label lblDeviceType = (Label)e.Item.FindControl("lblDeviceType");
                lblDeviceType.Text = (drv["TypeName"] != DBNull.Value ? drv["TypeName"].ToString() : "--");
                lblDeviceType.ToolTip = "Type Id: " + drv["TypeId"].ToString();

                if (intDebug == 0 || intDebug >= 8)
                {
                    //Load the provisioning status for each asset...
                    oTabProvisioning.AddTab(drv["servername"].ToString(), "");
                    strDivs += "<div  id=\"divTab" + intCount.ToString() + "\" style=\"display:inline\"><iframe width=\"100%\" height=\"100%\" frameborder=\"0\" scrolling=\"auto\" src=\"/datapoint/asset/provisioning_status.aspx" + "?id=" + oFunction.encryptQueryString(drv["serverid"].ToString()) + "&c=" + intCount.ToString() + "\"></iframe></div>";
                }

                intCount++;
            }
        
        }

        protected void dlServices_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;

                LinkButton lnkRequestId = (LinkButton)e.Item.FindControl("lnkRequestId");
                lnkRequestId.Text = drv["ReqServiceNumber"].ToString();

                if (drv["automate"].ToString() != "1")
                    lnkRequestId.Attributes.Add("onclick", "return OpenNewWindowMenu('/datapoint/service/resource.aspx?id=" + oFunction.encryptQueryString(drv["ResourceRequestID"].ToString()) + "', '800', '600');");
                else
                    lnkRequestId.Attributes.Add("onclick", "return OpenNewWindowMenu('/datapoint/service/request.aspx?t=" + oFunction.encryptQueryString(drv["RequestNumber"].ToString()) + " &q=" + oFunction.encryptQueryString(drv["RequestNumber"].ToString()) + " ', '800', '600');");


                LinkButton lnkServiceName = (LinkButton)e.Item.FindControl("lnkServiceName");
                lnkServiceName.Text = drv["ServiceName"].ToString();
                lnkServiceName.ToolTip = drv["ServiceId"].ToString();

                lnkServiceName.Attributes.Add("onclick", "return OpenWindow('SERVICES_DETAIL','?sid=" +drv["ServiceId"].ToString() + "');");

                Label lblProgress = (Label)e.Item.FindControl("lblServiceProgress");
                lblProgress.Text = drv["ResourceRequestID"].ToString();

                Label lblRequestStatus = (Label)e.Item.FindControl("lblServiceStatus");
                lblRequestStatus.Text = drv["ServiceStatusName"].ToString();

                ((Label)e.Item.FindControl("lblServiceSubmitted")).Text=drv["RequestSubmitted"].ToString();

                ((Label)e.Item.FindControl("lblServiceLastUpdated")).Text = drv["RequestModified"].ToString();
                
                //Get the progress bar

                if (lblRequestStatus.Text == "Submitted")
                {  if (lblProgress.Text == "")
                        lblProgress.Text = "<i>Unavailable</i>";
                    else
                    {
                        int intResource = Int32.Parse(lblProgress.Text);
                        double dblAllocated = 0.00;
                        double dblUsed = 0.00;
                        bool boolAssigned = false;
                        DataSet dsResource = oDataPoint.GetServiceRequestResource(intResource);
                        foreach (DataRow drResource in dsResource.Tables[1].Rows)
                        {
                            if (drResource["deleted"].ToString() == "0")
                            {
                                boolAssigned = true;
                                dblAllocated += double.Parse(drResource["allocated"].ToString());
                                dblUsed += double.Parse(drResource["used"].ToString());
                            }
                        }
                        if (boolAssigned == false)
                        {
                            if (drv["OnDemand"] == DBNull.Value || drv["OnDemand"].ToString() == "0")
                            {   if (drv["Automate"] == DBNull.Value || drv["Automate"].ToString() == "0")
                                {
                                    string strManager = "<tr><td colspan=\"3\"><img src=\"/images/spacer.gif\" border=\"0\" width=\"15\" height=\"1\"/></td></tr>";
                                    DataSet dsManager = oService.GetUser(Int32.Parse(drv["serviceid"].ToString()), 1);  // Managers
                                    foreach (DataRow drManager in dsManager.Tables[0].Rows)
                                    {
                                        int intManager = Int32.Parse(drManager["userid"].ToString());
                                        strManager += "<tr><td><img src=\"/images/spacer.gif\" border=\"0\" width=\"15\" height=\"1\"/></td><td>-</td><td><a href=\"javascript:void(0);\" class=\"lookup\" onclick=\"OpenWindow('PROFILE','?userid=" + intManager.ToString() + "');\">" + oUser.GetFullName(intManager) + " [" + oUser.GetName(intManager) + "]</a></td></tr>";
                                    }
                                    lblProgress.Text = "Pending Assignment [<a href=\"javascript:void(0);\" class=\"lookup\" onclick=\"ShowHideDiv2('divAssign" + intResource.ToString() + "');\">View Service Managers</a>]<div id=\"divAssign" + intResource.ToString() + "\" style=\"display:none\"><table cellpadding=\"2\" cellspacing=\"2\" border=\"0\">" + strManager + "</table></div>";
                                }
                                else
                                    lblProgress.Text = oServiceRequest.GetStatusBarIn(100.00, "100", "12", true);
                            }
                            else
                            {
                                Forecast oForecast = new Forecast(intProfile, dsn);
                                ModelsProperties oModelsProperties = new ModelsProperties(intProfile, dsn);
                                Types oType = new Types(intProfile, dsn);
                                DataSet dsService = oForecast.GetAnswerService(Int32.Parse( drv["requestid"].ToString()));
                                if (dsService.Tables[0].Rows.Count > 0)
                                {
                                    int intAnswer = Int32.Parse(dsService.Tables[0].Rows[0]["id"].ToString());
                                    int intModel = Int32.Parse(dsService.Tables[0].Rows[0]["modelid"].ToString());
                                    int intType = oModelsProperties.GetType(intModel);
                                    string strExecute = oType.Get(intType, "forecast_execution_path");
                                    if (strExecute != "")
                                        lblProgress.Text = "<a href=\"javascript:void(0);\" class=\"lookup\" title=\"Click here to execute this service\" onclick=\"OpenWindow('FORECAST_EXECUTE','" + strExecute + "?id=" + intAnswer.ToString() + "');\">Execute</a>";
                                    else
                                        lblProgress.Text = "<a href=\"javascript:void(0);\" class=\"lookup\" title=\"Click here to execute this service\" onclick=\"alert('Execution has not been configured for asset type " + oType.Get(intType, "name") + "');\">Execute</a>";
                                }
                            }
                        }
                        else if (dblAllocated > 0.00)
                            lblProgress.Text = oServiceRequest.GetStatusBarIn((dblUsed / dblAllocated) * 100.00, "100", "12", true);
                        else
                            lblProgress.Text = "<i>N / A</i>";
                        
                    }
                }
                else
                  lblProgress.Text = "---";
                


            }
        }

      
    }
}
