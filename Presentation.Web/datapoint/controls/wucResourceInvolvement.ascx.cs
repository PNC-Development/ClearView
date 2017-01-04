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
    public partial class wucResourceInvolvement : System.Web.UI.UserControl
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
	
        private int? intResourceRequestId;
        public int? ResourceRequestId
        {
            get { return intResourceRequestId; }
            set { intResourceRequestId = value; }
        }
        private int? intResourceId;
        public int? ResourceId
        {
            get { return intResourceId; }
            set { intResourceId = value; }
        }
        private string strSearchBy;
        public string SearchBy
        {
            get { return strSearchBy; }
            set { strSearchBy = value; }
        }

        private DataPoint oDataPoint;
        private ServiceRequests oServiceRequest;
        private Users oUser;
        private Services oService;
        protected StatusLevels oStatusLevel;
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
                oStatusLevel = new StatusLevels();
                oFunction = new Functions(intProfile, dsn, intEnvironment);
                oProject = new Projects(intProfile, dsn);
                oLog = new Log(intProfile, dsn);
                int intProjectID = 0;
                if (intProjectId != null)
                    Int32.TryParse(intProjectId.ToString(), out intProjectID);
                oLog.AddEvent("PROJECT_SEARCH", "", oProject.Get(intProjectID, "number") + ": ResourceInvolvement started at " + DateTime.Now.ToString(), LoggingType.Debug);
                LoadResourceInvolvement();
                oLog.AddEvent("PROJECT_SEARCH", "", oProject.Get(intProjectID, "number") + ": ResourceInvolvement finished at " + DateTime.Now.ToString(), LoggingType.Debug);
            }
        }

        private void LoadResourceInvolvement()
        {

            DataSet ds = oDataPoint.GetServiceRequestResourceInvolvement(
                            
                (intProjectId > 0?intProjectId: null),
                (intRequestId > 0 ? intRequestId : null),
                (intResourceRequestId > 0 ? intResourceRequestId : null),
                (intResourceId > 0 ? intResourceId : null),
                1);

            if (ds.Tables.Count != 0)
            {
                if (ds.Tables.Count> 1 && ds.Tables[1].Rows.Count > 0)
                    ds.Relations.Add("relationship", ds.Tables[0].Columns["RRWFId"], ds.Tables[1].Columns["parent"], false);
                //rptResourceInvolvement.DataSource = ds.Tables[0];
                rptResourceInvolvement.DataSource = ds;
                rptResourceInvolvement.DataBind();
                lblResourceInvolvement.Visible = (rptResourceInvolvement.Items.Count == 0);
                foreach (RepeaterItem ri in rptResourceInvolvement.Items)
                {
                    Label lblResource = (Label)ri.FindControl("lblResource");
                    lblResource.Text = "<a href=\"javascript:void(0);\" onclick=\"OpenWindow('PROFILE','?userid=" + lblResource.ToolTip + "');\">" + lblResource.Text + "</a>";
                    Label lblRequest = (Label)ri.FindControl("lblRequest");
                    Label lblRRID = (Label)ri.FindControl("lblRRID");
                    if (lblRRID.Text == "" || lblRRID.Text == "0")
                        lblRequest.Text = "<a href=\"javascript:void(0);\" onclick=\"OpenNewWindowMenu('/datapoint/service/request.aspx?t=" + oFunction.encryptQueryString(lblRequest.ToolTip) + " &q=" + oFunction.encryptQueryString(lblRequest.ToolTip) + " ', '800', '600');\">" + lblRequest.Text + "</a>";
                    else
                        lblRequest.Text = "<a href=\"javascript:void(0);\" onclick=\"OpenNewWindowMenu('/datapoint/service/resource.aspx?id=" + oFunction.encryptQueryString(lblRRID.Text) + "', '800', '600');\">" + lblRequest.Text + "</a>";
                    Label lblService = (Label)ri.FindControl("lblService");
                    lblService.Text = "<a href=\"javascript:void(0);\" onclick=\"OpenWindow('SERVICES_DETAIL','?sid=" + lblService.ToolTip + "');\">" + lblService.Text + "</a>";
                    Label lblUsed = (Label)ri.FindControl("lblUsed");
                    Label lblAllocated = (Label)ri.FindControl("lblAllocated");
                    Label lblProgress = (Label)ri.FindControl("lblProgress");
                    double dblProgress = 0;
                    if (double.Parse(lblUsed.Text) > 0 && double.Parse(lblAllocated.Text) > 0)
                        dblProgress = double.Parse(lblUsed.Text) / double.Parse(lblAllocated.Text) * 100;
                    if (Int32.Parse(lblProgress.Text) == 3)
                        dblProgress = 100.00;
                    lblProgress.Text = oServiceRequest.GetStatusBarIn(dblProgress, "100", "12", true); ;
                    Label lblTime = (Label)ri.FindControl("lblTime");
                    double dblSLA = 0.00;
                    double.TryParse(lblTime.ToolTip, out dblSLA);
                    double dblElapsed = 0.00;
                    double.TryParse(lblTime.Text, out dblElapsed);
                    if (dblSLA > 0.00 && dblElapsed > 0.00)
                        lblTime.CssClass = (dblElapsed > dblSLA ? "redbold" : "bluebold");

                    Label lblCompleted = (Label)ri.FindControl("lblCompleted");
                    if (lblCompleted != null && lblCompleted.Text != "")
                        lblCompleted.Text = "&nbsp;@&nbsp;";

                    Label lblUpdatedBy = (Label)ri.FindControl("lblUpdatedBy");
                    lblUpdatedBy.Text = "<a href=\"javascript:void(0);\" onclick=\"OpenWindow('PROFILE','?userid=" + lblUpdatedBy.ToolTip + "');\">" + lblUpdatedBy.Text + "</a>";

                    LinkButton btnEdit = (LinkButton)ri.FindControl("btnEdit");
                    Panel pnlDelete = (Panel)ri.FindControl("pnlDelete");
                    LinkButton btnVirtualView = (LinkButton)ri.FindControl("btnVirtualView");
                    int intService = Int32.Parse(btnEdit.ToolTip);

                    if (pnlDelete.ToolTip == "1")
                        pnlDelete.Visible = true;
                    else
                    {
                        pnlDelete.Visible = false;

                        if (oUser.IsAdmin(intProfile) || oService.IsManager(intService, intProfile))
                        {
                            btnEdit.Enabled = true;
                            btnEdit.Attributes.Add("onclick", "return OpenNewWindowMenu('/datapoint/service/manager.aspx?id=" + btnVirtualView.ToolTip + "', '800', '600');");
                        }
                        else
                            btnEdit.Enabled = false;

                        btnVirtualView.Attributes.Add("onclick", "return OpenNewWindowMenu('/frame/resource_request.aspx?rrid=" + btnVirtualView.ToolTip + "', '800', '600');");
                    }

                    Label lblComments = (Label)ri.FindControl("lblComments");
                    if (lblComments.Text == "")
                    {
                        HtmlTable tblCurrentStatusUpdates = (HtmlTable)ri.FindControl("tblCurrentStatusUpdates");
                        tblCurrentStatusUpdates.Visible = false;
                    }
                    else
                    {
                        Label lblResourceStatus = (Label)ri.FindControl("lblResourceStatus");
                        lblResourceStatus.Text = "<a href=\"javascript:void(0);\" onclick=\"OpenWindow('PROFILE','?userid=" + lblResourceStatus.ToolTip + "');\">" + lblResourceStatus.Text + "</a>";
                    }
                    LinkButton lnkbtnAdditionalComments = (LinkButton)ri.FindControl("lnkbtnAdditionalComments");
                    HtmlTableRow trAdditionalComments = (HtmlTableRow)ri.FindControl("trAdditionalComments");
                    Repeater rptServices = (Repeater)ri.FindControl("rptServices");
                    lnkbtnAdditionalComments.Visible = (rptServices.Items.Count > 0);
                    lnkbtnAdditionalComments.Attributes.Add("onclick", "ShowHideDiv2('" + trAdditionalComments.ClientID + "');return false;");
                    foreach (RepeaterItem ri2 in rptServices.Items)
                    {
                        Label lblResourceStatus = (Label)ri2.FindControl("lblResourceStatus");
                        lblResourceStatus.Text = "<a href=\"javascript:void(0);\" onclick=\"OpenWindow('PROFILE','?userid=" + lblResourceStatus.ToolTip + "');\">" + lblResourceStatus.Text + "</a>";
                    }
                }
            }

        }

        protected void dlResourceInvolvement_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;












                DataSet dsRsourceAdditionalUpdates = oDataPoint.GetServiceRequestResourceInvolvement(
                                                    null,
                                                    null,
                                                    Int32.Parse(drv["RRID"].ToString()),
                                                    null,
                                                    0);
                LinkButton lnkbtnAdditionalComments = (LinkButton)e.Item.FindControl("lnkbtnAdditionalComments");
                lnkbtnAdditionalComments.Visible = false;
                if (dsRsourceAdditionalUpdates.Tables[0].Rows.Count > 0)
                {
                    DataList dlResourceAdditionalUpdates = (DataList)e.Item.FindControl("dlResourceAdditionalUpdates");
                    dlResourceAdditionalUpdates.DataSource = dsRsourceAdditionalUpdates.Tables[0];
                    dlResourceAdditionalUpdates.DataBind();

                    
                    lnkbtnAdditionalComments.Visible = true;
                   // lnkbtnAdditionalComments.Attributes.Add("onclick", "ShowHideDiv2('" + dlResourceAdditionalUpdates.ClientID + "');return false;");

                    HtmlTableRow trAdditionalComments = (HtmlTableRow)e.Item.FindControl("trAdditionalComments");
                    lnkbtnAdditionalComments.Attributes.Add("onclick", "ShowHideDiv2('" + trAdditionalComments.ClientID + "');return false;");
                }

            }
        }

        protected void dlResourceAdditionalUpdates_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;

                Image imgPreviousStatus = (Image)e.Item.FindControl("imgPreviousStatus");
                imgPreviousStatus.ImageUrl = "~/images/" + drv["weekly_image"].ToString() + ".gif";

                LinkButton lnkPreviousStatusResource = (LinkButton)e.Item.FindControl("lnkPreviousStatusResource");
                lnkPreviousStatusResource.Text = drv["RRWFUserName"].ToString();
                lnkPreviousStatusResource.ToolTip = "User Id :" + drv["RRWFUserId"].ToString();
                lnkPreviousStatusResource.Attributes.Add("onclick", "return OpenWindow('PROFILE','?userid=" + drv["RRWFUserId"].ToString() + "');");


                Label lblPreviousStatusUpdatedDate = (Label)e.Item.FindControl("lblPreviousStatusUpdatedDate");
                lblPreviousStatusUpdatedDate.Text = drv["RRUpdateWeeklyModified"].ToString();

                Label lblPreviousStatusComment = (Label)e.Item.FindControl("lblPreviousStatusComment");
                lblPreviousStatusComment.Text = drv["RRUpdateWeeklyComments"].ToString();
            }
        }
        }
    }
