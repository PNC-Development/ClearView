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
    public partial class wucServiceProgression : System.Web.UI.UserControl
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        
        private int? intProjectId;
        public int? ProjectId
        {
            get { return intProjectId; }
            set { intProjectId = value; }
        }
        
        private int? intRequestId;
        public int? RequestId
        {
            get { return intRequestId; }
            set { intRequestId = value; }
        }

        private int? intResourceAssigned;
        public int? ResourceAssigned
        {
            get { return intResourceAssigned; }
            set { intResourceAssigned = value; }
        }

        private DataPoint oDataPoint;
        private ServiceRequests oServiceRequest;
        private Users oUser;
        private Services oService;
        private StatusLevels oStatusLevel;
        protected Functions oFunction;
        private Projects oProject;
        private Log oLog;
        private int intProfile = 0;
        private int intApplication = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                if (Request.Cookies["profileid"] != null && Request.Cookies["profileid"].Value != "")
                    intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
                if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                    intApplication = Int32.Parse(Request.QueryString["applicationid"]);
                if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                    intApplication = Int32.Parse(Request.Cookies["application"].Value);
                oDataPoint = new DataPoint(intProfile, dsn);
                oUser = new Users(intProfile, dsn);
                oServiceRequest = new ServiceRequests(intProfile, dsn);
                oService = new Services(intProfile, dsn);
                oFunction = new Functions(intProfile, dsn, intEnvironment);
                oStatusLevel = new StatusLevels();
                oProject = new Projects(intProfile, dsn);
                oLog = new Log(intProfile, dsn);
                int intProjectID = 0;
                if (intProjectId != null)
                    Int32.TryParse(intProjectId.ToString(), out intProjectID);
                oLog.AddEvent("PROJECT_SEARCH", "", oProject.Get(intProjectID, "number") + ": ServiceProgression started at " + DateTime.Now.ToString(), LoggingType.Debug);
                LoadServiceProgression();
                oLog.AddEvent("PROJECT_SEARCH", "", oProject.Get(intProjectID, "number") + ": ServiceProgression finished at " + DateTime.Now.ToString(), LoggingType.Debug);
            }
        }

        private void LoadServiceProgression()
        {
            DataSet dsServices = oDataPoint.GetServiceRequestSearchResults(
                                       "",
                                       "",
                                       null, null, null, null,
                                      (intResourceAssigned > 0 ? intResourceAssigned : null),
                                      (intProjectId > 0 ? intProjectId : null),
                                       null, null, null, null, null, null,
                                       hdnOrderBy.Value.ToString(), Int32.Parse(hdnOrder.Value.ToString()), Int32.Parse(hdnPageNo.Value), Int32.Parse(hdnRecsPerPage.Value));


            rptServices.DataSource = dsServices.Tables[0];
            rptServices.DataBind();
            foreach (RepeaterItem ri in rptServices.Items)
            {
                Label lblAutomate = (Label)ri.FindControl("lblAutomate");
                Label lblRRID = (Label)ri.FindControl("lblRRID");
                Label lblRequest = (Label)ri.FindControl("lblRequest");
                if (lblAutomate.Text != "1")
                    lblRequest.Text = "<a href=\"javascript:void(0);\" onclick=\"OpenNewWindowMenu('/datapoint/service/resource.aspx?id=" + oFunction.encryptQueryString(lblRRID.Text) + "', '800', '600');\">" + lblRequest.Text + "</a>";
                else
                    lblRequest.Text = "<a href=\"javascript:void(0);\" onclick=\"OpenNewWindowMenu('/datapoint/service/request.aspx?t=" + oFunction.encryptQueryString(lblRequest.ToolTip) + " &q=" + oFunction.encryptQueryString(lblRequest.ToolTip) + " ', '800', '600');\">" + lblRequest.Text + "</a>";
                Label lblService = (Label)ri.FindControl("lblService");
                lblService.Text = "<a href=\"javascript:void(0);\" onclick=\"OpenWindow('SERVICES_DETAIL','?sid=" + lblService.ToolTip + "');\">" + lblService.Text + "</a>";
                Label lblRequestedBy = (Label)ri.FindControl("lblRequestedBy");
                lblRequestedBy.Text = "<a href=\"javascript:void(0);\" onclick=\"OpenWindow('PROFILE','?userid=" + lblRequestedBy.ToolTip + "');\">" + lblRequestedBy.Text + "</a>";
                Label lblAssignedBy = (Label)ri.FindControl("lblAssignedBy");
                lblAssignedBy.Text = "<a href=\"javascript:void(0);\" onclick=\"OpenWindow('PROFILE','?userid=" + lblAssignedBy.ToolTip + "');\">" + lblAssignedBy.Text + "</a>";
                Label lblAssignedTo = (Label)ri.FindControl("lblAssignedTo");
                lblAssignedTo.Text = "<a href=\"javascript:void(0);\" onclick=\"OpenWindow('PROFILE','?userid=" + lblAssignedTo.ToolTip + "');\">" + lblAssignedTo.Text + "</a>";
                Label lblProgress = (Label)ri.FindControl("lblProgress");
                //Get the Progress 
                if (lblAutomate.Text == "1")
                    lblProgress.Text = oServiceRequest.GetStatusBarIn(100.00, "100", "12", true);
                else
                {
                    if (lblRRID.Text == "")
                    {
                        lblProgress.Text = "<i>Unavailable</i>";
                    }
                    else
                    {
                        int intRRId = Int32.Parse(lblRRID.Text);
                        double dblAllocated = 0.00;
                        double dblUsed = 0.00;
                        bool boolAssigned = false;
                        DataSet dsResource = oDataPoint.GetServiceRequestResource(intRRId);
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
                            Label lblOnDemand = (Label)ri.FindControl("lblOnDemand");
                            if (lblOnDemand.Text == "" || lblOnDemand.Text == "0")
                            {
                                Label lblServiceID = (Label)ri.FindControl("lblServiceID");
                                string strManager = "<tr><td colspan=\"3\"><img src=\"/images/spacer.gif\" border=\"0\" width=\"15\" height=\"1\"/></td></tr>";
                                DataSet dsManager = oService.GetUser(Int32.Parse(lblServiceID.Text), 1);  // Managers
                                foreach (DataRow drManager in dsManager.Tables[0].Rows)
                                {
                                    int intManager = Int32.Parse(drManager["userid"].ToString());
                                    strManager += "<tr><td><img src=\"/images/spacer.gif\" border=\"0\" width=\"15\" height=\"1\"/></td><td>-</td><td><a href=\"javascript:void(0);\" class=\"lookup\" onclick=\"OpenWindow('PROFILE','?userid=" + intManager.ToString() + "');\">" + oUser.GetFullName(intManager) + " [" + oUser.GetName(intManager) + "]</a></td></tr>";
                                }
                                lblProgress.Text = "Pending Assignment [<a href=\"javascript:void(0);\" class=\"lookup\" onclick=\"ShowHideDiv2('divAssign" + intRRId.ToString() + "');\">View Service Managers</a>]<div id=\"divAssign" + intRRId.ToString() + "\" style=\"display:none\"><table cellpadding=\"2\" cellspacing=\"2\" border=\"0\">" + strManager + "</table></div>";
                            }
                            else
                            {
                                Forecast oForecast = new Forecast(intProfile, dsn);
                                ModelsProperties oModelsProperties = new ModelsProperties(intProfile, dsn);
                                Types oType = new Types(intProfile, dsn);
                                Label lblRequestID = (Label)ri.FindControl("lblRequestID");
                                DataSet dsService = oForecast.GetAnswerService(Int32.Parse(lblRequestID.Text));
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
            }

            //dlServices.Visible = (dlServices.Items.Count > 0);
            lblServices.Visible = (rptServices.Items.Count == 0);
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
                lnkServiceName.Attributes.Add("onclick", "return OpenWindow('SERVICES_DETAIL','?sid=" + drv["ServiceId"].ToString() + "');");

                LinkButton lnkRequestedBy = (LinkButton)e.Item.FindControl("lnkRequestedBy");
                lnkRequestedBy.Text = drv["RequestorName"].ToString() + (drv["RequestorXID"].ToString() != "" ? "(" + drv["RequestorXID"].ToString() + ")" : "");
                lnkRequestedBy.Attributes.Add("onclick", "return OpenWindow('PROFILE','?userid=" + drv["userid"].ToString() + "');");
               

                LinkButton lnkAssignedBy = (LinkButton)e.Item.FindControl("lnkAssignedBy");
                lnkAssignedBy.Text = drv["UserAssignedByName"].ToString() + (drv["UserAssignedByXID"].ToString() != "" ? "(" + drv["UserAssignedByXID"].ToString() + ")" : "");
                lnkAssignedBy.Attributes.Add("onclick", "return OpenWindow('PROFILE','?userid=" + drv["AssignedBy"].ToString() + "');");

                LinkButton lnkAssignedTo = (LinkButton)e.Item.FindControl("lnkAssignedTo");
                lnkAssignedTo.Text = drv["UserAssignedToName"].ToString() + (drv["UserAssignedToXID"].ToString() != "" ? "(" + drv["UserAssignedToXID"].ToString() + ")" : "");
                lnkAssignedTo.Attributes.Add("onclick", "return OpenWindow('PROFILE','?userid=" + drv["AssignedTo"].ToString() + "');");


                Label lblServiceSubmitted = (Label)e.Item.FindControl("lblServiceSubmitted");
                lblServiceSubmitted.Text = (drv["RequestSubmitted"] != DBNull.Value ? DateTime.Parse(drv["RequestSubmitted"].ToString()).ToShortDateString() : "");
                lblServiceSubmitted.ToolTip = drv["RequestSubmitted"].ToString();

                Label lblServiceLastUpdated = (Label)e.Item.FindControl("lblServiceLastUpdated");
                lblServiceLastUpdated.Text = (drv["RequestModified"] != DBNull.Value ? DateTime.Parse(drv["RequestModified"].ToString()).ToShortDateString() : "");
                lblServiceLastUpdated.ToolTip = drv["RequestModified"].ToString();
                
                //Get the Progress 
                Label lblServiceProgress = (Label)e.Item.FindControl("lblServiceProgress");
                if (drv["Automate"] != DBNull.Value && drv["Automate"].ToString() == "1")
                {
                    lblServiceProgress.Text = oServiceRequest.GetStatusBarIn(100.00, "100", "12", true);
                }
                else
                {
                    if (drv["ResourceRequestID"].ToString() == "")
                    {
                        lblServiceProgress.Text = "<i>Unavailable</i>";
                    }
                    else
                    {
                        int intRRId = Int32.Parse(drv["ResourceRequestID"].ToString());
                        double dblAllocated = 0.00;
                        double dblUsed = 0.00;
                        bool boolAssigned = false;
                        DataSet dsResource = oDataPoint.GetServiceRequestResource(intRRId);
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
                            {
                               string strManager = "<tr><td colspan=\"3\"><img src=\"/images/spacer.gif\" border=\"0\" width=\"15\" height=\"1\"/></td></tr>";
                               DataSet dsManager = oService.GetUser(Int32.Parse(drv["serviceid"].ToString()), 1);  // Managers
                               foreach (DataRow drManager in dsManager.Tables[0].Rows)
                               {
                                    int intManager = Int32.Parse(drManager["userid"].ToString());
                                    strManager += "<tr><td><img src=\"/images/spacer.gif\" border=\"0\" width=\"15\" height=\"1\"/></td><td>-</td><td><a href=\"javascript:void(0);\" class=\"lookup\" onclick=\"OpenWindow('PROFILE','?userid=" + intManager.ToString() + "');\">" + oUser.GetFullName(intManager) + " [" + oUser.GetName(intManager) + "]</a></td></tr>";
                               }
                               lblServiceProgress.Text = "Pending Assignment [<a href=\"javascript:void(0);\" class=\"lookup\" onclick=\"ShowHideDiv2('divAssign" + intRRId.ToString() + "');\">View Service Managers</a>]<div id=\"divAssign" + intRRId.ToString() + "\" style=\"display:none\"><table cellpadding=\"2\" cellspacing=\"2\" border=\"0\">" + strManager + "</table></div>";
                            }
                            else
                            {
                                Forecast oForecast = new Forecast(intProfile, dsn);
                                ModelsProperties oModelsProperties = new ModelsProperties(intProfile, dsn);
                                Types oType = new Types(intProfile, dsn);
                                DataSet dsService = oForecast.GetAnswerService(Int32.Parse(drv["requestid"].ToString()));
                                if (dsService.Tables[0].Rows.Count > 0)
                                {
                                    int intAnswer = Int32.Parse(dsService.Tables[0].Rows[0]["id"].ToString());
                                    int intModel = Int32.Parse(dsService.Tables[0].Rows[0]["modelid"].ToString());
                                    int intType = oModelsProperties.GetType(intModel);
                                    string strExecute = oType.Get(intType, "forecast_execution_path");
                                    if (strExecute != "")
                                        lblServiceProgress.Text = "<a href=\"javascript:void(0);\" class=\"lookup\" title=\"Click here to execute this service\" onclick=\"OpenWindow('FORECAST_EXECUTE','" + strExecute + "?id=" + intAnswer.ToString() + "');\">Execute</a>";
                                    else
                                        lblServiceProgress.Text = "<a href=\"javascript:void(0);\" class=\"lookup\" title=\"Click here to execute this service\" onclick=\"alert('Execution has not been configured for asset type " + oType.Get(intType, "name") + "');\">Execute</a>";
                                }
                            }
                        }
                        else if (dblAllocated > 0.00)
                            lblServiceProgress.Text = oServiceRequest.GetStatusBarIn((dblUsed / dblAllocated) * 100.00, "100", "12", true);
                        else
                            lblServiceProgress.Text = "<i>N / A</i>";
                        
                    }
                }

                Label lblServiceStatus = (Label)e.Item.FindControl("lblServiceStatus");
                lblServiceStatus.Text = drv["ServiceStatusName"].ToString();

                if (intProjectId == 0 || intProjectId ==null) //Showing for project
                {
                    if (drv["projectid"] != DBNull.Value && Int32.Parse(drv["projectid"].ToString()) > 0)
                    {
                        Label lblProjectNumber = (Label)e.Item.FindControl("lblProjectNumber");
                        lblProjectNumber.Text = drv["ProjectNumber"].ToString();

                        LinkButton lnkProjectName = (LinkButton)e.Item.FindControl("lnkProjectName");
                        lnkProjectName.Text = drv["ProjectName"].ToString();

                        lnkProjectName.Attributes.Add("onclick", "return OpenNewWindowMenu('/datapoint/projects/datapoint_projects.aspx?id=" + oFunction.encryptQueryString(drv["projectid"].ToString()) + "', '800', '600');");
                        HtmlTableRow trProject = (HtmlTableRow)e.Item.FindControl("trProject");
                        trProject.Visible = true;
                    }
                }
                
                //Label lblRequestedBy = (Label)e.Item.FindControl("lblRequestedBy");
                //lblRequestedBy.Text = (drv["TypeName"] != DBNull.Value ? drv["TypeName"].ToString() : "--");
                //lblRequestedBy.ToolTip = "Type Id: " + drv["TypeId"].ToString();


            }
        }

        protected void btnOrder_Click(Object Sender, EventArgs e)
        {
            LinkButton oOrder = (LinkButton)Sender;

            if (hdnOrderBy.Value == oOrder.CommandArgument)
            {
                if (hdnOrder.Value == "1")
                    hdnOrder.Value = "0";
                else if (hdnOrder.Value == "0")
                    hdnOrder.Value = "1";
            }
            else
            {
                hdnOrderBy.Value = oOrder.CommandArgument;
                hdnOrder.Value = "0";
            }

            LoadServiceProgression();
        }

  
    }
}