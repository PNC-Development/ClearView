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
using Microsoft.ApplicationBlocks.Data;
using System.Collections.Generic;

namespace NCC.ClearView.Presentation.Web
{
    public partial class datapoint_service_search : BasePage
    {
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        private string dsnServiceEditor = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ServiceEditorDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intDataPointAvailableService = Int32.Parse(ConfigurationManager.AppSettings["DATAPOINT_AVAILABLE_SERVICE"]);
        protected int intLists = Int32.Parse(ConfigurationManager.AppSettings["DATAPOINT_LISTS"]);
        protected int intViewRequest = Int32.Parse(ConfigurationManager.AppSettings["ResourceRequest"]);
        protected DataPoint oDataPoint;
        protected Functions oFunction;
        protected Users oUser;
        protected Models oModel;
        protected Applications oApplications;
        protected Confidence oConfidence;
        private ServiceRequests oServiceRequest;
        private ResourceRequest oResourceRequest;
        private Services oService;

        protected Variables oVariable;
        protected Pages oPage;

        protected int intProfile = 0;
        protected int intApplication = 0;
        protected string strResults = "";
        protected int intCounter = 0;
        protected int intPage = 1;
        protected int intPerPage = 0;
        protected string strCookie = "SERVICE_SEARCH";
        protected StatusLevels oStatusLevel;
        private bool boolAdvSearch = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticateUser();
            Master.Page.Title = "DataPoint | Service Search";
            if (Request.Cookies["profileid"] != null && Request.Cookies["profileid"].Value != "")
                intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);

            oDataPoint = new DataPoint(intProfile, dsn);
            oFunction = new Functions(intProfile, dsn, intEnvironment);
            oUser = new Users(intProfile, dsn);
            oModel = new Models(intProfile, dsn);
            oApplications = new Applications(intProfile, dsn);
            oService = new Services(intProfile, dsn);
            oServiceRequest = new ServiceRequests(intProfile, dsn);
            oResourceRequest = new ResourceRequest(intProfile, dsn);
            oVariable = new Variables(intEnvironment);
            oPage = new Pages(intProfile, dsn);
            oStatusLevel = new StatusLevels();
            oConfidence = new Confidence(intProfile, dsn);
            Response.Cookies["storage"].Value = "0";
            Response.Cookies["applications"].Value = "0";

        

            
            if (oUser.IsAdmin(intProfile) == true || (oDataPoint.GetPagePermission(intApplication, "SERVICE") == true || intDataPointAvailableService == 1))
            {
                if (Request.QueryString["advanced"] != null && Request.QueryString["advanced"].ToString() == "true")
                    boolAdvSearch = true;

                if (!IsPostBack)
                {   if (Request.QueryString["search"] != null && Request.QueryString["search"].ToString() == "request")
                    {
                        ddlSearchType.ClearSelection();
                        ddlSearchType.Items.FindByValue("request").Selected = true;
                    }
                    else if (Request.QueryString["search"] != null && Request.QueryString["search"].ToString() == "design")
                    {
                        ddlSearchType.ClearSelection();
                        ddlSearchType.Items.FindByValue("design").Selected = true;
                    }
                    LoadList();
                }
                 



              //Set controls and add attributes
                setControls();
                AddControlAttributes();
            } 
            else
                pnlDenied.Visible = true;

            
        }

        private void setControls()
        {
            pnlAllow.Visible = true;
            pnlResults.Visible = false;
            panService.Visible = false;
            panDesign.Visible = false;
            pnlBasicSearch.Visible = true;
            pnlReqAdvancedSearch.Visible = false;
            pnlDesignAdvancedSearch.Visible = false;
            
            if (ddlSearchType.SelectedValue.ToString() == "request")
            {
                lblSearchId.Text = "Request Id";
                lblSearchName.Text = "Request Name";
                if (boolAdvSearch == true)
                {
                    pnlReqAdvancedSearch.Visible = true;
                    pnlDesignAdvancedSearch.Visible = false;
                    lnkbtnAdvancedSearch.Visible = false;
                    lnkbtnBasicSearch.Visible = true;
                }
                else
                {
                    lnkbtnAdvancedSearch.Visible = true;
                    lnkbtnBasicSearch.Visible = false;
                }
                
            }
            else if (ddlSearchType.SelectedValue.ToString() == "design")
            {
                lblSearchId.Text = "Design Id";
                lblSearchName.Text = "Design Name";
                if (boolAdvSearch == true)
                {
                    pnlReqAdvancedSearch.Visible = false;
                    pnlDesignAdvancedSearch.Visible = true;
                    lnkbtnAdvancedSearch.Visible = false;
                    lnkbtnBasicSearch.Visible = true;
                }
                else
                {
                    lnkbtnAdvancedSearch.Visible = true;
                    lnkbtnBasicSearch.Visible = false;
                }
            }
            txtSearchId.Focus();
        }
        private void LoadList()
        {
            

            DataSet dsStatus = SqlHelper.ExecuteDataset(dsn, CommandType.Text, oStatusLevel.RequestStatusList());
            ddlReqStatus.DataTextField = "name";
            ddlReqStatus.DataValueField = "id";
            ddlReqStatus.DataSource = dsStatus;
            ddlReqStatus.DataBind();
            ddlReqStatus.Items.Insert(0,new ListItem("--All--", "-100"));
       

            DataSet dsApp = oApplications.Gets(1);
            ddlReqAssingmentGrp.DataTextField="name";
            ddlReqAssingmentGrp.DataValueField = "applicationid";
            ddlReqAssingmentGrp.DataSource = dsApp;
            ddlReqAssingmentGrp.DataBind();
            ddlReqAssingmentGrp.Items.Insert(0, new ListItem("--All--", "0"));

            ddlDesignStatus.Items.Insert(0, new ListItem("--All--", "-100"));
            ddlDesignStatus.Items.Insert(1, new ListItem("Design", "0"));
            ddlDesignStatus.Items.Insert(2, new ListItem("Executed", "1"));
            ddlDesignStatus.Items.Insert(3, new ListItem("QA", "2"));
            ddlDesignStatus.Items.Insert(4, new ListItem("Completed", "3"));
            ddlDesignStatus.Items.Insert(5, new ListItem("Deleted", "-10"));

            DataSet dsConfidence = oConfidence.Gets(1);
            ddlDesignConfidenceLevel.DataTextField = "name";
            ddlDesignConfidenceLevel.DataValueField = "id";
            ddlDesignConfidenceLevel.DataSource = dsConfidence;
            ddlDesignConfidenceLevel.DataBind();
            ddlDesignConfidenceLevel.Items.Insert(0, new ListItem("--All--", "-1"));
           
        }

        private void AddControlAttributes()
        {
            lnkbtnClearHistory.Attributes.Add("onclick", "ClearSearchCriteria();");
            ddlSearchType.Attributes.Add("onchange", "ClearSearchCriteria();");
        
            txtSearchId.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnSearch.ClientID + "').click();return false;}} else {return true}; ");              
            txtSearchName.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnSearch.ClientID + "').click();return false;}} else {return true}; ");            

            txtReqRequestedBy.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'300','195','" + divReqRequestdBy.ClientID + "','" + lstReqRequestedBy.ClientID + "','" + hdnReqRequestBy.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
            lstReqRequestedBy.Attributes.Add("ondblclick", "AJAXClickRow();");

            txtReqAssignedBy.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'300','195','" + divReqAssignedBy.ClientID + "','" + lstReqAssignedBy.ClientID + "','" + hdnReqAssignedBy.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
            lstReqAssignedBy.Attributes.Add("ondblclick", "AJAXClickRow();");

            txtReqAssignedTo.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'300','195','" + divReqAssignedTo.ClientID + "','" + lstReqAssignedTo.ClientID + "','" + hdnReqAssignedTo.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
            lstReqAssignedTo.Attributes.Add("ondblclick", "AJAXClickRow();");


            txtReqProject.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'300','195','" + divReqProject.ClientID + "','" + lstReqProject.ClientID + "','" + hdnReqProjectId.ClientID + "','" + oVariable.URL() + "/frame/projects.aspx',2);");
            lstReqProject.Attributes.Add("ondblclick", "AJAXClickRow();");

            imgReqCreatedAfterDate.Attributes.Add("onclick", "return ShowCalendar('" + txtReqCreatedAfter.ClientID + "');");
            imgReqCreatedBeforeDate.Attributes.Add("onclick", "return ShowCalendar('" + txtReqCreatedBefore.ClientID + "');");

            imgReqModifiedAfterDate.Attributes.Add("onclick", "return ShowCalendar('" + txtReqModifiedAfter.ClientID + "');");
            imgReqModifiedBeforeDate.Attributes.Add("onclick", "return ShowCalendar('" + txtReqModifiedBefore.ClientID + "');");

            imgReqCompletedAfterDate.Attributes.Add("onclick", "return ShowCalendar('" + txtReqCompletedAfter.ClientID + "');");
            imgReqCompletedBeforeDate.Attributes.Add("onclick", "return ShowCalendar('" + txtReqCompletedBefore.ClientID + "');");

            //Design
            txtDesignCreatedBy.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'300','195','" + divDesignCreatedBy.ClientID + "','" + lstDesignCreatedBy.ClientID + "','" + hdnDesignCreatedBy.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
            lstDesignCreatedBy.Attributes.Add("ondblclick", "AJAXClickRow();");

            imgDesignCreatedAfterDate.Attributes.Add("onclick", "return ShowCalendar('" + txtDesignCreatedAfter.ClientID + "');");
            imgDesignCreatedBeforeDate.Attributes.Add("onclick", "return ShowCalendar('" + txtDesignCreatedBefore.ClientID + "');");

            imgDesignModifiedAfterDate.Attributes.Add("onclick", "return ShowCalendar('" + txtDesignModifiedAfter.ClientID + "');");
            imgDesignModifiedBeforeDate.Attributes.Add("onclick", "return ShowCalendar('" + txtDesignModifiedBefore.ClientID + "');");

            imgDesignCompletedAfterDate.Attributes.Add("onclick", "return ShowCalendar('" + txtDesignCompletedAfter.ClientID + "');");
            imgDesignCompletedBeforeDate.Attributes.Add("onclick", "return ShowCalendar('" + txtDesignCompletedBefore.ClientID + "');");


            if (boolAdvSearch==true)  
            {
                if (ddlSearchType.SelectedValue.ToString()=="request")
                    btnSearch.Attributes.Add("onclick", "return ValidateReqAdvanceSearchControls()" +
                    " && ProcessButton(this)" +
                    ";");
                else if (ddlSearchType.SelectedValue.ToString()=="design")
                    btnSearch.Attributes.Add("onclick", "return ValidateDesignAdvanceSearchControls()" +
                    " && ProcessButton(this)" +
                    ";");
             }
            else 
            {
                if (ddlSearchType.SelectedValue.ToString()=="request")
                    btnSearch.Attributes.Add("onclick", "return ValidateBasicSearchControls()" +
                    " && ProcessButton(this)" +
                    ";");
                else if (ddlSearchType.SelectedValue.ToString()=="design")
                    btnSearch.Attributes.Add("onclick", "return ValidateBasicSearchControls()" +
                    " && ProcessButton(this)" +
                    ";");
             }
        }

        protected void lnkbtnAdvancedSearch_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request.Path + "?advanced=true&search=" + ddlSearchType.SelectedValue.ToString());
        }

        protected void lnkbtnClearHistory_Click(object sender, EventArgs e)
        {

        }

        protected void btnAdvancedSearch_Click(object sender, EventArgs e)
        {
            hdnPageNo.Value = "1";
            hdnRecsPerPage.Value = ddlSearch.SelectedItem.Value;
            LoadData();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            //Basic Search
            hdnPageNo.Value = "1";
            hdnRecsPerPage.Value = ddlSearch.SelectedItem.Value;
            LoadData();
                            
        }

        protected void lnkbtnBasicSearch_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request.Path + "?advanced=false&search=" + ddlSearchType.SelectedValue.ToString());
        }

        private void LoadData()
        {
            pnlResults.Visible = true;
            panService.Visible = false;
            panDesign.Visible = false;
            DataSet ds=new DataSet();

            if (boolAdvSearch == false)
            {
                if (ddlSearchType.SelectedValue.ToString() == "request")
                {
                    ds = oDataPoint.GetServiceRequestSearchResults(
                                      txtSearchId.Text.Trim(),
                                      txtSearchName.Text.Trim(),
                                      null, null, null, null,null,null, null, null, null, null, null, null,
                                      hdnOrderBy.Value.ToString(), Int32.Parse(hdnOrder.Value.ToString()), Int32.Parse(hdnPageNo.Value), Int32.Parse(hdnRecsPerPage.Value));

                    if (ds.Tables[0].Rows.Count == 1)
                        if (ds.Tables[0].Rows[0]["automate"].ToString() != "1" && ds.Tables[0].Rows[0]["ResourceRequestID"].ToString() != "" && ds.Tables[0].Rows[0]["ResourceRequestID"].ToString() != "0")
                            Response.Redirect("/datapoint/service/resource.aspx?id=" + oFunction.encryptQueryString(ds.Tables[0].Rows[0]["ResourceRequestID"].ToString()));
                        else
                            Response.Redirect("/datapoint/service/request.aspx?t=" + oFunction.encryptQueryString(ds.Tables[0].Rows[0]["RequestNumber"].ToString()) + "&q=" + oFunction.encryptQueryString(ds.Tables[0].Rows[0]["RequestNumber"].ToString()));

                    SetControlsForRequestSearchResult(ds);
                }
                else if (ddlSearchType.SelectedValue.ToString() == "design")
                {
                    ds = oDataPoint.GetServiceDesignSearchResults(
                                    txtSearchId.Text.Trim(),
                                    txtSearchName.Text.Trim(),
                                    null,null,null,null,null,null,null,null,null,null,
                                    hdnOrderBy.Value.ToString(), Int32.Parse(hdnOrder.Value.ToString()), Int32.Parse(hdnPageNo.Value), Int32.Parse(hdnRecsPerPage.Value));

                    if (ds.Tables[0].Rows.Count == 1)
                        Response.Redirect("/datapoint/service/design.aspx?id=" + oFunction.encryptQueryString(ds.Tables[0].Rows[0]["id"].ToString()));
                    
                    SetControlsForDesignSearchResult(ds);

                }
            }
            else if (boolAdvSearch == true)
            {
                if (ddlSearchType.SelectedValue.ToString() == "request")
                {
                    ds = oDataPoint.GetServiceRequestSearchResults(
                                  txtSearchId.Text.Trim(), txtSearchName.Text.Trim(),
                                  ((ddlReqStatus.SelectedValue.ToString() != "-100") ? Int32.Parse(ddlReqStatus.SelectedValue.ToString()) : (int?)null),
                                  (hdnReqRequestBy.Value != "" ? Int32.Parse(hdnReqRequestBy.Value) : (int?)null),
                                  ((hdnReqAssignedBy.Value != "") ? Int32.Parse(hdnReqAssignedBy.Value) : (int?)null),
                                  ((ddlReqAssingmentGrp.SelectedValue.ToString() != "0") ? Int32.Parse(ddlReqAssingmentGrp.SelectedValue.ToString()) : (int?)null),
                                  ((hdnReqAssignedTo.Value != "") ? Int32.Parse(hdnReqAssignedTo.Value) : (int?)null),
                                  ((hdnReqProjectId.Value != "") ? Int32.Parse(hdnReqProjectId.Value) : (int?)null),
                                  ((txtReqCreatedAfter.Text.Trim() != "") ? DateTime.Parse(txtReqCreatedAfter.Text.Trim()) : (DateTime?)null),
                                  ((txtReqCreatedBefore.Text.Trim() != "") ? DateTime.Parse(txtReqCreatedBefore.Text.Trim()) : (DateTime?)null),
                                  ((txtReqModifiedAfter.Text.Trim() != "") ? DateTime.Parse(txtReqModifiedAfter.Text.Trim()) : (DateTime?)null),
                                  ((txtReqModifiedBefore.Text.Trim() != "") ? DateTime.Parse(txtReqModifiedBefore.Text.Trim()) : (DateTime?)null),
                                  ((txtReqCompletedAfter.Text.Trim() != "") ? DateTime.Parse(txtReqCompletedAfter.Text.Trim()) : (DateTime?)null),
                                  ((txtReqCompletedBefore.Text.Trim() != "") ? DateTime.Parse(txtReqCompletedBefore.Text.Trim()) : (DateTime?)null),
                                  hdnOrderBy.Value.ToString(), Int32.Parse(hdnOrder.Value.ToString()), Int32.Parse(hdnPageNo.Value), Int32.Parse(hdnRecsPerPage.Value));

                    if (ds.Tables[0].Rows.Count == 1)
                        if (ds.Tables[0].Rows[0]["automate"].ToString() != "1")
                            Response.Redirect("/datapoint/service/resource.aspx?id=" + oFunction.encryptQueryString(ds.Tables[0].Rows[0]["ResourceRequestID"].ToString()));
                        else
                            Response.Redirect("/datapoint/service/request.aspx?t=" + oFunction.encryptQueryString(ds.Tables[0].Rows[0]["RequestNumber"].ToString()) + "&q=" + oFunction.encryptQueryString(ds.Tables[0].Rows[0]["RequestNumber"].ToString()));
                    SetControlsForRequestSearchResult(ds);
                }
                else if (ddlSearchType.SelectedValue.ToString() == "design")
                {
                    ds = oDataPoint.GetServiceDesignSearchResults(
                                    txtSearchId.Text.Trim(),
                                    txtSearchName.Text.Trim(),
                                    ((ddlDesignStatus.SelectedValue.ToString() != "-100") ? Int32.Parse(ddlDesignStatus.SelectedValue.ToString()) : (int?)null),
                                    ((ddlDesignConfidenceLevel.SelectedValue.ToString() != "-1") ? Int32.Parse(ddlDesignConfidenceLevel.SelectedValue.ToString()) : (int?)null),
                                    (hdnDesignCreatedBy.Value != "" ? Int32.Parse(hdnDesignCreatedBy.Value) : (int?)null),
                                    null,
                                    ((txtDesignCreatedAfter.Text.Trim() != "") ? DateTime.Parse(txtDesignCreatedAfter.Text.Trim()) : (DateTime?)null),
                                    ((txtDesignCreatedBefore.Text.Trim() != "") ? DateTime.Parse(txtDesignCreatedBefore.Text.Trim()) : (DateTime?)null),
                                    ((txtDesignModifiedAfter.Text.Trim() != "") ? DateTime.Parse(txtDesignModifiedAfter.Text.Trim()) : (DateTime?)null),
                                    ((txtDesignModifiedBefore.Text.Trim() != "") ? DateTime.Parse(txtDesignModifiedBefore.Text.Trim()) : (DateTime?)null),
                                    ((txtDesignCompletedAfter.Text.Trim() != "") ? DateTime.Parse(txtDesignCompletedAfter.Text.Trim()) : (DateTime?)null),
                                    ((txtDesignCompletedBefore.Text.Trim() != "") ? DateTime.Parse(txtDesignCompletedBefore.Text.Trim()) : (DateTime?)null),
                                    hdnOrderBy.Value.ToString(), Int32.Parse(hdnOrder.Value.ToString()), Int32.Parse(hdnPageNo.Value), Int32.Parse(hdnRecsPerPage.Value));

                    if (ds.Tables[0].Rows.Count == 1)
                        Response.Redirect("/datapoint/service/design.aspx?id=" + oFunction.encryptQueryString(ds.Tables[0].Rows[0]["id"].ToString()));

                    SetControlsForDesignSearchResult(ds);

                }
            }

                
            if (ds.Tables[0].Rows.Count  > 0)
            {
                long lngDisplayRecords = Int64.Parse(ds.Tables[0].Rows[0]["rownum"].ToString()) + Int64.Parse((ds.Tables[0].Rows.Count-1).ToString());
                lblRecords.Text = "Showing Results <b>" + ds.Tables[0].Rows[0]["rownum"].ToString() + " - " + lngDisplayRecords.ToString() + "</b> of <b>" + ds.Tables[1].Rows[0]["TotalRecords"].ToString() + "...";
            }
            else
                lblRecords.Text = "No Results Found...";


            // Calculate total numbers of pages
            long lngRecsPerPage =Int64.Parse(hdnRecsPerPage.Value);
            if (lngRecsPerPage != 0)
            {
                long lngTotalRecsCount = Int64.Parse(ds.Tables[1].Rows[0]["TotalRecords"].ToString());
                long lngPgCount = lngTotalRecsCount / lngRecsPerPage+ ((lngTotalRecsCount % lngRecsPerPage)>0?1:0);
                if (Int32.Parse(hdnRecsPerPage.Value) != 0)
                {
                    // Display Next button
                    if (lngPgCount - 1 >= Convert.ToInt64(hdnPageNo.Value))
                        btnNext.Enabled = true;
                    else
                        btnNext.Enabled = false;
                    // Display Prev button
                    if ((Convert.ToInt64(hdnPageNo.Value)) > 1)
                        btnBack.Enabled = true;
                    else
                        btnBack.Enabled = false;
                }
            }
            else
            {
                btnNext.Enabled = false;
                btnBack.Enabled = false;
            }
        }

        protected void SetControlsForDesignSearchResult(DataSet ds)
        {
            panDesign.Visible = true;
            rptDesign.DataSource = ds;
            rptDesign.DataBind();

            lblDesign.Visible = (rptDesign.Items.Count == 0);

            foreach (RepeaterItem ri in rptDesign.Items)
            {
                int intUser = 0;
                Label lblUser = (Label)ri.FindControl("lblUser");
                if (lblUser.Text != "")
                    intUser = Int32.Parse(lblUser.Text);
                if (intUser > 0)
                    lblUser.Text = "<a href=\"javascript:void(0);\" class=\"lookup\" onclick=\"OpenWindow('PROFILE','?userid=" + intUser.ToString() + "');\">" + oUser.GetFullName(intUser) + " [" + oUser.GetName(intUser) + "]</a>";
                else
                    lblUser.Text = "---";

                Label lblStatus = (Label)ri.FindControl("lblStatus");
                Label lblDelete = (Label)ri.FindControl("lblDelete");
                Label lblDeleteForecast = (Label)ri.FindControl("lblDeleteForecast");
                Label lblDeleteRequest = (Label)ri.FindControl("lblDeleteRequest");
                Label lblDeleteProject = (Label)ri.FindControl("lblDeleteProject");
                Label lblCompleted = (Label)ri.FindControl("lblCompleted");
                Label lblFinished = (Label)ri.FindControl("lblFinished");
                Label lblExecuted = (Label)ri.FindControl("lblExecuted");
                if (lblDelete != null && lblDelete.Text == "1")
                    lblStatus.Text = "Design Deleted!";
                else if (lblDeleteForecast != null && lblDeleteForecast.Text == "1")
                    lblStatus.Text = "Forecast Deleted!";
                else if (lblDeleteRequest != null && lblDeleteRequest.Text == "1")
                    lblStatus.Text = "Request Deleted!";
                else if (lblDeleteProject != null && lblDeleteProject.Text == "1")
                    lblStatus.Text = "Project Deleted!";
                else if (lblFinished.Text != "")
                    lblStatus.Text = "Completed on " + lblFinished.Text;
                else if (lblCompleted.Text != "")
                    lblStatus.Text = "QA initiated on " + lblCompleted.Text;
                else if (lblExecuted.Text != "")
                    lblStatus.Text = "Executed on " + lblExecuted.Text;
                else
                {
                }

            }
        }

        protected void SetControlsForRequestSearchResult(DataSet ds)
        {
            panService.Visible = true;

            dlServices.DataSource = ds.Tables[0];
            dlServices.DataBind();

            dlServices.Visible = (dlServices.Items.Count > 0);
            lblService.Visible = (dlServices.Items.Count == 0);
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


                Label lblServiceSubmitted = (Label)e.Item.FindControl("lblServiceSubmitted");
                lblServiceSubmitted.Text = (drv["RequestSubmitted"] != DBNull.Value ? DateTime.Parse(drv["RequestSubmitted"].ToString()).ToShortDateString() : "");
                lblServiceSubmitted.ToolTip = drv["RequestSubmitted"].ToString();

                //Get the Progress 
                Label lblServiceProgress = (Label)e.Item.FindControl("lblServiceProgress");
                Label lblServiceStatus = (Label)e.Item.FindControl("lblServiceStatus");
                if (drv["Automate"] != DBNull.Value && drv["Automate"].ToString() == "1")
                {
                    lblServiceProgress.Text = oServiceRequest.GetStatusBarIn(100.00, "100", "12", true);
                    lblServiceStatus.Text = "Complete";
                }
                else
                {
                    if (drv["ResourceRequestID"].ToString() == "")
                    {
                        lblServiceProgress.Text = lblServiceStatus.Text = "<i>Unavailable</i>";
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

                        List<WorkflowStatus> RR = oResourceRequest.GetStatus(null, intRRId, null, null, null, null, false, dsnServiceEditor);
                        if (RR.Count > 0)
                            lblServiceStatus.Text = RR[0].status;
                        else
                            lblServiceStatus.Text = "Not Available";
                    }
                }

                //Label lblServiceStatus = (Label)e.Item.FindControl("lblServiceStatus");
                //lblServiceStatus.Text = drv["ServiceStatusName"].ToString();



              

            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            hdnPageNo.Value = Convert.ToString(Convert.ToInt64(hdnPageNo.Value) -1);
            LoadData();
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            hdnPageNo.Value = Convert.ToString(Convert.ToInt64(hdnPageNo.Value) + 1);
            LoadData();
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
            
            LoadData();
        }

        protected void ddlSearchType_SelectedIndexChanged(object sender, EventArgs e)
        {
           setControls();
        }

    }
}
