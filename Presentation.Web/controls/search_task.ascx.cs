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
using NCC.ClearView.Presentation.Web.Custom;
using System.Text;

namespace NCC.ClearView.Presentation.Web
{
    public partial class search_task : System.Web.UI.UserControl
    {
        private DataSet ds;
        private string dsnServiceEditor = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ServiceEditorDSN"]].ConnectionString;
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        protected int intWorkload = Int32.Parse(ConfigurationManager.AppSettings["ViewWorkload"]);
        protected int intWorkloadManager = Int32.Parse(ConfigurationManager.AppSettings["ViewWorkloadManager"]);
        protected int intProfile;
        protected int intApplication = 0;
        protected int intPage = 0;
        protected Pages oPage;
        protected Organizations oOrganization;
        protected Segment oSegment;
        protected Applications oApplication;
        protected RequestItems oRequestItem;
        protected Services oService;
        protected Requests oRequest;
        protected RequestFields oRequestField;
        protected Search oSearch;
        protected Users oUser;
        protected ServiceRequests oServiceRequest;
        protected ResourceRequest oResourceRequest;
        protected StatusLevels oStatusLevel;
        protected Documents oDocument;
        protected StatusLevels oStatus;
        protected Customized oCustomized;
        protected Variables oVariable;
        protected Functions oFunction;
        protected int intRecords = 20;
        protected int intRecordStart = 1;
        protected string strProject = "";
        protected string strDetails = "";
        protected string strPriority = "";
        protected string strDocuments = "";
        protected string strTaskName = "";
        protected string strMenuTab1 = "";
        private string strEMailIdsBCC = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oPage = new Pages(intProfile, dsn);
            oOrganization = new Organizations(intProfile, dsn);
            oSegment = new Segment(intProfile, dsn);
            oApplication = new Applications(intProfile, dsn);
            oRequestItem = new RequestItems(intProfile, dsn);
            oService = new Services(intProfile, dsn);
            oRequest = new Requests(intProfile, dsn);
            oRequestField = new RequestFields(intProfile, dsn);
            oSearch = new Search(intProfile, dsn);
            oUser = new Users(intProfile, dsn);
            oServiceRequest = new ServiceRequests(intProfile, dsn);
            oResourceRequest = new ResourceRequest(intProfile, dsn);
            oStatusLevel = new StatusLevels();
            oDocument = new Documents(intProfile, dsn);
            oStatus = new StatusLevels();
            oCustomized = new Customized(intProfile, dsn);
            oVariable = new Variables(intEnvironment);
            oFunction = new Functions(intProfile, dsn, intEnvironment);

            //Menus
            int intMenuTab = 0;
            if (Request.QueryString["menu_tab"] != null && Request.QueryString["menu_tab"] != "")
                intMenuTab = Int32.Parse(Request.QueryString["menu_tab"]);
            //Tab oTab = new Tab("", intMenuTab, "divMenu1", true, false);
            Tab oTab = new Tab(hdnType.ClientID, intMenuTab, "divMenu1", true, false);

            oTab.AddTab("Overall Search", "");
            oTab.AddTab("Search By Departmen", "");
            oTab.AddTab("Search By Group", "");
            oTab.AddTab("Search By Team Lead", "");
            oTab.AddTab("Search By Resource", "");
            strMenuTab1 = oTab.GetTabs();

            //End Menus

            if (Request.QueryString["applicationid"] != null && Request.QueryString["applicationid"] != "")
                intApplication = Int32.Parse(Request.QueryString["applicationid"]);
            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);
            if (Request.Cookies["application"] != null && Request.Cookies["application"].Value != "")
                intApplication = Int32.Parse(Request.Cookies["application"].Value);
            lblTitle.Text = oPage.Get(intPage, "title");
            bool boolUserSearch = false;
            if (Request.QueryString["rid"] != null && Request.QueryString["rid"] != "")
            {
                panReview.Visible = (Request.QueryString["page"] == null);
                panResult.Visible = true;
                int intRequest = Int32.Parse(Request.QueryString["rid"]);
                strTaskName = oServiceRequest.GetName(intRequest);
                lblTaskName.Text = "&quot;" + strTaskName + "&quot;";
                // PRIORITY
                // DETAILS
                DataSet dsDetails = oResourceRequest.GetWorkflowRequestAll(intRequest);
                if (dsDetails.Tables[0].Rows.Count > 0)
                    strDetails = oRequestField.GetBodyWorkflow(Int32.Parse(dsDetails.Tables[0].Rows[0]["id"].ToString()), dsnServiceEditor, intEnvironment, dsnAsset, dsnIP);
                // DOCUMENTS
                strDocuments = oDocument.GetDocuments_Request(intRequest, intProfile, oVariable.DocumentsFolder(), 1, (Request.QueryString["doc"] != null));
                // GetDocuments(string _physical, int _projectid, int _requestid, int _userid, int _security, bool _show_description, bool _mine)
                //strDocuments = oDocument.GetDocuments(Request.PhysicalApplicationPath, 0, intRequest, intProfile, 1, (Request.QueryString["doc"] != null), false);
                // HISTORY
                // INVOLVEMENT
                DataSet dsInvolvement = oResourceRequest.GetWorkflowRequestAll(intRequest);
                int intOldUser = 0;
                foreach (DataRow dr in dsInvolvement.Tables[0].Rows)
                {
                    if (intOldUser == Int32.Parse(dr["userid"].ToString()))
                        dr.Delete();
                    else
                        intOldUser = Int32.Parse(dr["userid"].ToString());
                }
                ddlResource.DataValueField = "userid";
                ddlResource.DataTextField = "userid";
                ddlResource.DataSource = dsInvolvement;
                ddlResource.DataBind();

                foreach (ListItem oItem in ddlResource.Items)
                    oItem.Text = oUser.GetFullName(Int32.Parse(oItem.Value));
                ddlResource.Items.Insert(0, new ListItem("-- SELECT --", "0"));
                dsInvolvement = oResourceRequest.GetWorkflowRequestAll(intRequest);
                int intOldItem = 0;
                intOldUser = 0;
                foreach (DataRow dr in dsInvolvement.Tables[0].Rows)
                {
                    if (intOldItem == Int32.Parse(dr["itemid"].ToString()) && intOldUser == Int32.Parse(dr["userid"].ToString()))
                        dr.Delete();
                    else
                    {
                        intOldItem = Int32.Parse(dr["itemid"].ToString());
                        intOldUser = Int32.Parse(dr["userid"].ToString());
                    }
                }
                rptInvolvement.DataSource = dsInvolvement;
                rptInvolvement.DataBind();
                lblNoInvolvement.Visible = (rptInvolvement.Items.Count == 0);
                foreach (RepeaterItem ri in rptInvolvement.Items)
                {
                    Label _id = (Label)ri.FindControl("lblId");
                    Label _user = (Label)ri.FindControl("lblUser");
                    Label _status = (Label)ri.FindControl("lblStatus");
                    Label _image = (Label)ri.FindControl("lblImage");
                    int intStatus = Int32.Parse(_status.Text);
                    int intUser = Int32.Parse(_user.Text);
                    _user.Text = oUser.GetFullName(intUser);
                    Label _item = (Label)ri.FindControl("lblItem");
                    int intItem2 = Int32.Parse(_item.Text);
                    Label _allocated = (Label)ri.FindControl("lblAllocated");
                    Label _used = (Label)ri.FindControl("lblUsed");
                    double dblAllocated = oResourceRequest.GetAllocatedRequest(intRequest, intUser, intItem2);
                    double dblUsed = oResourceRequest.GetUsedRequest(intRequest, intUser, intItem2);
                    Label _percent = (Label)ri.FindControl("lblPercent");
                    _allocated.Text = dblAllocated.ToString();
                    _used.Text = dblUsed.ToString();
                    if (dblAllocated > 0)
                    {
                        dblUsed = dblUsed / dblAllocated;
                        _percent.Text = dblUsed.ToString("P");
                    }
                    else
                        _percent.Text = dblAllocated.ToString("P");
                    bool boolTPMDone = false;
                    if (intItem2 == 0)
                        _item.Text = "Project Coordinator";
                    else if (intItem2 == -1)
                        _item.Text = "Design Implementation (Pending Execution)";
                    else
                    {
                        int intApp = oRequestItem.GetItemApplication(intItem2);
                        _item.Text = oApplication.GetName(intApp);
                    }
                    _status.Text = oStatus.Name(intStatus);
                    if (boolTPMDone == true)
                        _status.Text = "Closed";
                }
                // SERVICE PROGRESS
                DataSet dsAll = oResourceRequest.GetWorkflowRequestAll(intRequest);
                if (Request.QueryString["sid"] != null && Request.QueryString["sid"] != "")
                {
                    ds = oSearch.GetTask(Int32.Parse(Request.QueryString["sid"]));
                    if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0]["userid"].ToString() == intProfile.ToString())
                    {
                        string strType = ds.Tables[0].Rows[0]["type"].ToString();
                        switch (strType)
                        {
                            case "2":
                                string strDepartment = ds.Tables[0].Rows[0]["department"].ToString();
                                if (strDepartment != "0")
                                {
                                    foreach (DataRow drAll in dsAll.Tables[0].Rows)
                                    {
                                        if (oRequestItem.GetItemApplication(Int32.Parse(drAll["itemid"].ToString())) != Int32.Parse(strDepartment))
                                            drAll.Delete();
                                    }
                                }
                                break;
                            case "5":
                                string strResource = ds.Tables[0].Rows[0]["technician"].ToString();
                                if (strResource != "0")
                                {
                                    foreach (DataRow drAll in dsAll.Tables[0].Rows)
                                    {
                                        if (Int32.Parse(drAll["userid"].ToString()) != Int32.Parse(strResource))
                                            drAll.Delete();
                                    }
                                }
                                else
                                {
                                    foreach (DataRow drAll in dsAll.Tables[0].Rows)
                                    {
                                        if (oRequestItem.GetItemApplication(Int32.Parse(drAll["itemid"].ToString())) != intApplication)
                                            drAll.Delete();
                                    }
                                }
                                boolUserSearch = true;
                                break;
                            case "4":
                                string strLead = ds.Tables[0].Rows[0]["lead"].ToString();
                                if (strLead != "0")
                                {
                                    int intLead = Int32.Parse(strLead);
                                    foreach (DataRow drAll in dsAll.Tables[0].Rows)
                                    {
                                        if (oUser.GetManager(Int32.Parse(drAll["userid"].ToString()), true) != intLead)
                                            drAll.Delete();
                                    }
                                }
                                else
                                {
                                    foreach (DataRow drAll in dsAll.Tables[0].Rows)
                                    {
                                        if (oRequestItem.GetItemApplication(Int32.Parse(drAll["itemid"].ToString())) != intApplication)
                                            drAll.Delete();
                                    }
                                }
                                break;
                            case "3":
                                string strItem = ds.Tables[0].Rows[0]["itemid"].ToString();
                                if (strItem != "0")
                                {
                                    foreach (DataRow drAll in dsAll.Tables[0].Rows)
                                    {
                                        if (Int32.Parse(drAll["itemid"].ToString()) != Int32.Parse(strItem))
                                            drAll.Delete();
                                    }
                                }
                                else
                                {
                                    foreach (DataRow drAll in dsAll.Tables[0].Rows)
                                    {
                                        if (oRequestItem.GetItemApplication(Int32.Parse(drAll["itemid"].ToString())) != intApplication)
                                            drAll.Delete();
                                    }
                                }
                                break;
                        }
                    }
                }
                else if (Request.QueryString["aid"] != null && Request.QueryString["aid"] != "")
                {
                    foreach (DataRow drAll in dsAll.Tables[0].Rows)
                    {
                        int intItem = Int32.Parse(drAll["itemid"].ToString());
                        if (oRequestItem.GetItemApplication(intItem) != Int32.Parse(Request.QueryString["aid"]))
                            drAll.Delete();
                    }
                }
                DataView dv = dsAll.Tables[0].DefaultView;
                string strSort = (boolUserSearch ? "userid" : "itemid");
                if (Request.QueryString["sort"] != null)
                {
                    if (Request.QueryString["sort"].StartsWith("itemid") || Request.QueryString["sort"].StartsWith("userid"))
                        strSort = Request.QueryString["sort"];
                }
                btnSortService.Enabled = (strSort.StartsWith("itemid") == false);
                btnSortUser.Enabled = (strSort.StartsWith("userid") == false);
                dv.Sort = strSort;
                int intOldSort = -10;
                bool boolOther = false;
                bool boolOther2 = false;
                StringBuilder sbTemp = new StringBuilder();
                StringBuilder sbProject = new StringBuilder(strProject);
                double dblOAllocated = 0.00;
                double dblOUsed = 0.00;
                int intOStatus = 3;
                int intCount = 0;
                bool boolExpand = (Request.QueryString["expand"] != null);
                btnExpand.Text = (boolExpand ? "<img src=\"/images/minus.gif\" border=\"0\" align=\"absmiddle\"> Hide All" : "<img src=\"/images/plus.gif\" border=\"0\" align=\"absmiddle\"> Show All");
                foreach (DataRowView dr in dv)
                {
                    boolOther2 = !boolOther2;
                    int intSort = Int32.Parse(dr[strSort].ToString());
                    if (intOldSort != intSort)
                    {
                        if (sbTemp.ToString() != "")
                        {
                            sbTemp.Insert(0, "<table width=\"100%\" cellpadding=\"3\" cellspacing=\"0\" border=\"0\" align=\"center\" style=\"border:solid 1px #CCCCCC\"><tr bgcolor=\"#EEEEEE\"><td width=\"30%\" class=\"tableheader\"><b>Resource:</b></td><td class=\"tableheader\" width=\"40%\"><b>Service:</b></td><td class=\"tableheader\" width=\"30%\"><b>Last Updated:</b></td><td class=\"tableheader\" nowrap><b>Progress:</b></td><td></td></tr>");
                            sbTemp.Append("</table>");
                            sbProject.Append("<tr");
                            sbProject.Append(boolOther2 == false ? " bgcolor=\"#F6F6F6\"" : "");
                            sbProject.Append("><td><a href=\"javascript:void(0);\" onclick=\"ShowDetail('imgDetail_");
                            sbProject.Append(intOldSort.ToString());
                            sbProject.Append("','divDetail_");
                            sbProject.Append(intOldSort.ToString());
                            sbProject.Append("');\"><img id=\"imgDetail_");
                            sbProject.Append(intOldSort.ToString());
                            sbProject.Append("\" src=\"/images/");
                            sbProject.Append(boolExpand ? "biggerMinus" : "biggerPlus");
                            sbProject.Append(".gif\" border=\"0\" align=\"absmiddle\" /></a></td>");
                            string strName = "Unavailable";
                            if (strSort == "itemid")
                                strName = (intOldSort == 0 ? "Project Coordinator" : (intOldSort == -1 ? "Design Implementation (Pending Execution)" : oApplication.GetName(oRequestItem.GetItemApplication(intOldSort)) + " <a href=\"javascript:void(0);\" onclick=\"alert('Placeholder for Service Detail');\" class=\"bold\">" + oRequestItem.GetItem(intOldSort, "service_title") + "</a>"));
                            else if (strSort == "userid")
                                strName = oUser.GetFullName(intOldSort);
                            sbProject.Append("<td class=\"default\" width=\"100%\">");
                            sbProject.Append(strName);
                            sbProject.Append(" (");
                            sbProject.Append(intCount.ToString());
                            sbProject.Append(")</td>");
                            double dblOProgress = 0.00;
                            if (dblOAllocated == 0.00 && dblOUsed == 0.00 && intOStatus == 3)
                            {
                                dblOAllocated = 1.00;
                                dblOUsed = 1.00;
                            }
                            if (dblOAllocated > 0.00)
                                dblOProgress = (dblOUsed / dblOAllocated) * 100.00;
                            intOStatus = 0;
                            dblOUsed = 0.00;
                            dblOAllocated = 0.00;
                            sbProject.Append("<td>");
                            sbProject.Append(oServiceRequest.GetStatusBar(dblOProgress, "100", "12", true));
                            sbProject.Append("</td><td><img src=\"/images/spacer.gif\" border=\"0\" width=\"50\" height=\"1\"/></td></tr>");
                            sbProject.Append("<tr");
                            sbProject.Append(boolOther2 == false ? " bgcolor=\"#F6F6F6\"" : "");
                            sbProject.Append("><td></td><td colspan=\"3\" id=\"divDetail_");
                            sbProject.Append(intOldSort.ToString());
                            sbProject.Append("\" style=\"display:");
                            sbProject.Append(boolExpand ? "inline" : "none");
                            sbProject.Append("\">");
                            sbProject.Append(sbTemp.ToString());
                            sbProject.Append("</td>");
                            sbProject.Append("<tr><td colspan=\"3\">&nbsp;</td></tr>");
                            sbTemp = new StringBuilder();
                            intCount = 0;
                            boolOther = false;
                        }
                    }
                    intOldSort = intSort;
                    intCount = intCount + 1;
                    boolOther = !boolOther;
                    int intResource = Int32.Parse(dr["id"].ToString());
                    int intUser = Int32.Parse(dr["userid"].ToString());
                    int intItem = Int32.Parse(dr["itemid"].ToString());
                    int intService = Int32.Parse(dr["serviceid"].ToString());
                    sbTemp.Append("<tr class=\"default\" onmouseover=\"CellRowOver(this);\" onmouseout=\"CellRowOut(this);\" onclick=\"if(event.srcElement.tagName != 'A')OpenWindow('RESOURCE_REQUEST_MANAGER','");
                    sbTemp.Append(intResource.ToString());
                    sbTemp.Append("');\"");
                    sbTemp.Append(boolOther == false ? " bgcolor=\"#F6F6F6\"" : "");
                    sbTemp.Append(">");
                    sbTemp.Append("<td valign=\"top\">");
                    sbTemp.Append(oUser.GetFullName(intUser));
                    sbTemp.Append("</td>");
                    sbTemp.Append("<td valign=\"top\">");
                    sbTemp.Append(intItem == 0 ? "Project Coordinator" : (intItem == -1 ? "Design Implementation (Pending Execution)" : oService.GetName(intService)));
                    sbTemp.Append("</td>");
                    sbTemp.Append("<td valign=\"top\">");
                    sbTemp.Append(dr["name"].ToString());
                    sbTemp.Append("</td>");
                    sbTemp.Append("<td valign=\"top\">");
                    sbTemp.Append(dr["modified"].ToString());
                    sbTemp.Append("</td>");
                    double dblAllocated = double.Parse(oResourceRequest.GetWorkflow(intResource, "allocated"));
                    int intStatus = Int32.Parse(oResourceRequest.GetWorkflow(intResource, "status"));
                    if (intStatus > -1 && intStatus < 3)
                        intOStatus = intStatus;
                    dblOAllocated += dblAllocated;
                    double dblUsed = oResourceRequest.GetWorkflowUsed(intResource);
                    dblOUsed += dblUsed;
                    double dblProgress = 0.00;
                    if (dblAllocated == 0.00 && dblUsed == 0.00 && intStatus == 3)
                    {
                        dblAllocated = 1.00;
                        dblUsed = 1.00;
                    }
                    if (dblAllocated > 0.00)
                        dblProgress = (dblUsed / dblAllocated) * 100.00;
                    sbTemp.Append("<td valign=\"top\" align=\"right\">");
                    sbTemp.Append(oServiceRequest.GetStatusBar(dblProgress, "100", "6", true));
                    sbTemp.Append("</td>");
                    sbTemp.Append("</tr>");
                }
                if (sbTemp.ToString() != "")
                {
                    sbTemp.Insert(0, "<table width=\"100%\" cellpadding=\"3\" cellspacing=\"0\" border=\"0\" align=\"center\" style=\"border:solid 1px #CCCCCC\"><tr bgcolor=\"#EEEEEE\"><td width=\"15%\" class=\"tableheader\"><b>Resource:</b></td><td class=\"tableheader\" width=\"35%\"><b>Service:</b></td><td class=\"tableheader\" width=\"25%\"><b>Name:</b></td><td class=\"tableheader\" width=\"25%\"><b>Last Updated:</b></td><td class=\"tableheader\" nowrap><b>Progress:</b></td><td></td></tr>");
                    sbTemp.Append("</table>");
                    sbProject.Append("<tr");
                    sbProject.Append(boolOther2 == true ? " bgcolor=\"#F6F6F6\"" : "");
                    sbProject.Append("><td><a href=\"javascript:void(0);\" onclick=\"ShowDetail('imgDetail_");
                    sbProject.Append(intOldSort.ToString());
                    sbProject.Append("','divDetail_");
                    sbProject.Append(intOldSort.ToString());
                    sbProject.Append("');\"><img id=\"imgDetail_");
                    sbProject.Append(intOldSort.ToString());
                    sbProject.Append("\" src=\"/images/");
                    sbProject.Append(boolExpand ? "biggerMinus" : "biggerPlus");
                    sbProject.Append(".gif\" border=\"0\" align=\"absmiddle\" /></a></td>");
                    string strName = "Unavailable";
                    if (strSort == "itemid")
                        strName = (intOldSort == 0 ? "Project Coordinator" : (intOldSort == -1 ? "Design Implementation (Pending Execution)" : oApplication.GetName(oRequestItem.GetItemApplication(intOldSort)) + " <a href=\"javascript:void(0);\" onclick=\"alert('Placeholder for Service Detail');\" class=\"bold\">" + oRequestItem.GetItem(intOldSort, "service_title") + "</a>"));
                    else if (strSort == "userid")
                        strName = oUser.GetFullName(intOldSort);
                    sbProject.Append("<td class=\"default\" width=\"100%\">");
                    sbProject.Append(strName);
                    sbProject.Append(" (");
                    sbProject.Append(intCount.ToString());
                    sbProject.Append(")</td>");
                    double dblOProgress = 0.00;
                    if (dblOAllocated == 0.00 && dblOUsed == 0.00 && intOStatus == 3)
                    {
                        dblOAllocated = 1.00;
                        dblOUsed = 1.00;
                    }
                    if (dblOAllocated > 0.00)
                        dblOProgress = (dblOUsed / dblOAllocated) * 100.00;
                    intOStatus = 0;
                    dblOUsed = 0.00;
                    dblOAllocated = 0.00;
                    sbProject.Append("<td>");
                    sbProject.Append(oServiceRequest.GetStatusBar(dblOProgress, "100", "12", true));
                    sbProject.Append("</td><td><img src=\"/images/spacer.gif\" border=\"0\" width=\"50\" height=\"1\"/></td></tr>");
                    sbProject.Append("<tr");
                    sbProject.Append(boolOther2 == true ? " bgcolor=\"#F6F6F6\"" : "");
                    sbProject.Append("><td></td><td colspan=\"3\" id=\"divDetail_");
                    sbProject.Append(intOldSort.ToString());
                    sbProject.Append("\" style=\"display:");
                    sbProject.Append(boolExpand ? "inline" : "none");
                    sbProject.Append("\">");
                    sbProject.Append(sbTemp.ToString());
                    sbProject.Append("</td>");
                }
                if (sbProject.ToString() != "")
                {
                    sbProject.Insert(0, "<table width=\"100%\" cellpadding=\"3\" cellspacing=\"0\" border=\"0\" class=\"default\"><tr><td class=\"bold\" colspan=\"2\">" + (strSort == "itemid" ? "Service" : "Resource") + ":</td><td class=\"bold\" colspan=\"2\">Progress:</td></tr>");
                    sbProject.Append("</table>");
                }

                strProject = sbProject.ToString();
            }
            else if (Request.QueryString["sid"] != null && Request.QueryString["sid"] != "")
                LoadSearch(Int32.Parse(Request.QueryString["sid"]));
            else
            {
                panSearch.Visible = true;
                LoadLists();
                imgOStart.Attributes.Add("onclick", "return ShowCalendar('" + txtOStart.ClientID + "');");
                imgOEnd.Attributes.Add("onclick", "return ShowCalendar('" + txtOEnd.ClientID + "');");
                btnSearch.Attributes.Add("onclick", "return ValidateSearch('" + hdnType.ClientID + "','" + txtName.ClientID + "','" + txtNumber.ClientID + "','" + hdnSubmittedBy.ClientID + "');");
            }
            if (!IsPostBack)
            {
                hdnType.Value = "O";
                hdnSubmittedBy.Value = "0";
            }
            btnClose.Attributes.Add("onclick", "return CloseWindow();");
            hypClear.NavigateUrl = oPage.GetFullLink(intPage);
            txtSubmittedBy.Attributes.Add("onkeyup", "return AJAXTextBoxGet(this,'300','195','" + divSubmittedBy.ClientID + "','" + lstSubmittedBy.ClientID + "','" + hdnSubmittedBy.ClientID + "','" + oVariable.URL() + "/frame/users.aspx',2);");
            lstSubmittedBy.Attributes.Add("ondblclick", "AJAXClickRow();");
        }
        private void LoadLists()
        {
            ddlDepartment.DataValueField = "applicationid";
            ddlDepartment.DataTextField = "name";
            ddlDepartment.DataSource = oApplication.Gets(1);
            ddlDepartment.DataBind();
            ddlDepartment.Items.Insert(0, new ListItem("-- ALL --", "0"));
            ddlGroup.DataValueField = "itemid";
            ddlGroup.DataTextField = "name";
            ddlGroup.DataSource = oRequestItem.GetItems(intApplication, 0, 1);
            ddlGroup.DataBind();
            ddlGroup.Items.Insert(0, new ListItem("-- ALL --", "0"));
            // COMMENTED 06/26/08: Change team lead search from both "itemid" and "managerid" to just "managerid"
            //ddlLead.DataValueField = "itemid";
            ddlLead.DataValueField = "userid";
            ddlLead.DataTextField = "managerapp";
            ddlLead.DataSource = oRequestItem.GetItemsManagers(intApplication, 1);
            ddlLead.DataBind();
            ddlLead.Items.Insert(0, new ListItem("-- ALL --", "0"));
            int intManager = oApplication.GetManager(intApplication);
            ddlTechnician.DataValueField = "userid";
            ddlTechnician.DataTextField = "username";
            ddlTechnician.DataSource = oUser.GetManagerReports(intManager);
            ddlTechnician.DataBind();
            ddlTechnician.Items.Insert(0, new ListItem("-- ALL --", "0"));
            ddlDStatus.Items.Insert(0, new ListItem("-- ALL --", "0"));
            ddlTStatus.Items.Insert(0, new ListItem("-- ALL --", "0"));
            ddlOStatus.Items.Insert(0, new ListItem("-- ALL --", "0"));
            ddlGStatus.Items.Insert(0, new ListItem("-- ALL --", "0"));
            ddlLStatus.Items.Insert(0, new ListItem("-- ALL --", "0"));
        }
        protected void btnSort_Click(Object Sender, EventArgs e)
        {
            LinkButton oOrder = (LinkButton)Sender;
            string strOrder = "";
            if (Request.QueryString["sort"] != null)
            {
                if (Request.QueryString["sort"] == oOrder.CommandArgument)
                    strOrder = oOrder.CommandArgument + " DESC";
            }
            if (strOrder == "")
                strOrder = oOrder.CommandArgument;
            if (Request.QueryString["expand"] == null)
                Reload("?rid=" + Request.QueryString["rid"] + "&sid=" + Request.QueryString["sid"] + "&sort=" + strOrder);
            else
                Reload("?rid=" + Request.QueryString["rid"] + "&sid=" + Request.QueryString["sid"] + "&sort=" + strOrder + "&expand=true");
        }
        protected void btnExpand_Click(Object Sender, EventArgs e)
        {
            string strOrder = "";
            if (Request.QueryString["sort"] != null)
                strOrder = "&sort=" + Request.QueryString["sort"];
            if (Request.QueryString["expand"] == null)
                Reload("?rid=" + Request.QueryString["rid"] + "&sid=" + Request.QueryString["sid"] + strOrder + "&expand=true");
            else
                Reload("?rid=" + Request.QueryString["rid"] + "&sid=" + Request.QueryString["sid"] + strOrder);
        }
        private void LoadSearch(int _search)
        {
            lblTitle.Text = "Search Results";
            ds = oSearch.GetTask(_search);
            if (ds.Tables[0].Rows.Count > 0)
            {
                panResults.Visible = true;
                lblSearch.Text = _search.ToString();
                string strSort = "";
                if (Request.QueryString["sort"] != null && Request.QueryString["sort"] != "")
                    strSort = "&sort=" + Request.QueryString["sort"];
                btnPrint.Attributes.Add("onclick", "return OpenWindow('NEW_WINDOW','/frame/print_search.aspx?sid=" + _search.ToString() + strSort + "');");
                if (ds.Tables[0].Rows[0]["userid"].ToString() == intProfile.ToString())
                {
                    string strType = ds.Tables[0].Rows[0]["type"].ToString();
                    string strClause = "cv_requests.deleted = 0 AND cv_requests.projectid < 1";
                    string strOr1 = "";
                    string strOr2 = "";
                    string strSQL = "SELECT DISTINCT '?rid=' + CAST(cv_requests.requestid AS varchar(10)) + '&sid=" + _search.ToString() + "' AS query, cv_service_requests.name, 'CVT' + CAST(cv_requests.requestid AS varchar(10)) AS number, cv_resource_requests.status, cv_requests.created FROM cv_requests LEFT OUTER JOIN cv_service_requests ON cv_service_requests.requestid = cv_requests.requestid AND cv_service_requests.deleted = 0 INNER JOIN cv_resource_requests LEFT OUTER JOIN cv_request_items INNER JOIN cv_applications ON cv_request_items.applicationid = cv_applications.applicationid AND cv_applications.deleted = 0 ON cv_request_items.itemid = cv_resource_requests.itemid AND cv_request_items.deleted = 0 INNER JOIN cv_resource_requests_workflow INNER JOIN cv_users ON cv_resource_requests_workflow.userid = cv_users.userid AND cv_users.deleted = 0 AND cv_users.enabled = 1 ON cv_resource_requests_workflow.parent = cv_resource_requests.id AND cv_resource_requests_workflow.deleted = 0 AND cv_resource_requests_workflow.status > -3 AND cv_resource_requests_workflow.status <> -1 AND cv_resource_requests_workflow.status <> 0 AND cv_resource_requests_workflow.status < 6 ON cv_requests.requestid = cv_resource_requests.requestid AND cv_resource_requests.deleted = 0 AND cv_resource_requests.status > -3 AND cv_resource_requests.status <> -1 AND cv_resource_requests.status <> 0 AND cv_resource_requests.status < 6 WHERE ";
                    string strApplication = " AND cv_request_items.applicationid = " + intApplication.ToString();
                    switch (strType)
                    {
                        case "1":
                            strApplication = "";
                            string strOName = ds.Tables[0].Rows[0]["oname"].ToString().Trim();
                            if (strOName != "")
                            {
                                strClause += " AND cv_service_requests.name LIKE '%" + strOName + "%'";
                                lblResults.Text = "Task Name LIKE &quot;" + strOName + "&quot;";
                            }
                            string strONumber = ds.Tables[0].Rows[0]["onumber"].ToString().Trim();
                            if (strONumber != "")
                            {
                                string strNumber = strONumber;
                                if (strNumber.StartsWith("CVT") == true)
                                    strNumber = strNumber.Substring(3);
                                strClause += " AND cv_service_requests.requestid LIKE '%" + strNumber + "%'";
                                if (lblResults.Text != "")
                                    lblResults.Text += " AND ";
                                lblResults.Text += "Task Number LIKE &quot;" + strONumber + "&quot;";
                            }
                            string strOStatus = ds.Tables[0].Rows[0]["ostatus"].ToString();
                            if (strOStatus != "0")
                            {
                                strClause += " AND cv_resource_requests_workflow.status = " + strOStatus;
                                if (lblResults.Text != "")
                                    lblResults.Text += " AND ";
                                lblResults.Text += "Status = &quot;" + oStatusLevel.Name(Int32.Parse(strOStatus)) + "&quot;";
                            }
                            string strOBy = ds.Tables[0].Rows[0]["oby"].ToString();
                            if (strOBy != "0")
                            {
                                strClause += " AND cv_requests.userid = " + strOBy;
                                if (lblResults.Text != "")
                                    lblResults.Text += " AND ";
                                lblResults.Text += "Submitted By = &quot;" + oUser.GetFullName(Int32.Parse(strOBy)) + "&quot;";
                            }
                            string strOStart = ds.Tables[0].Rows[0]["ostart"].ToString();
                            if (strOStart != "")
                            {
                                strOStart = DateTime.Parse(strOStart).ToShortDateString();
                                strClause += " AND cv_requests.created > " + strOStart;
                                if (lblResults.Text != "")
                                    lblResults.Text += " AND ";
                                lblResults.Text += "Submitted On > &quot;" + strOStart + "&quot;";
                            }
                            string strOEnd = ds.Tables[0].Rows[0]["oend"].ToString();
                            if (strOEnd != "")
                            {
                                strOEnd = DateTime.Parse(strOEnd).ToShortDateString();
                                strClause += " AND cv_requests.created < " + strOEnd;
                                if (lblResults.Text != "")
                                    lblResults.Text += " AND ";
                                lblResults.Text += "Submitted On < &quot;" + strOEnd + "&quot;";
                            }
                            break;
                        case "2":
                            string strDepartment = ds.Tables[0].Rows[0]["department"].ToString();
                            if (strDepartment == "0")
                            {
                                strApplication = "";
                                lblResults.Text += "Department = &quot;ALL&quot;";
                            }
                            else
                            {
                                strApplication = " AND cv_request_items.applicationid = " + strDepartment;
                                lblResults.Text += "Department = &quot;" + oApplication.GetName(Int32.Parse(strDepartment)) + "&quot;";
                            }
                            string strDStatus = ds.Tables[0].Rows[0]["dstatus"].ToString();
                            if (strDStatus != "0")
                            {
                                strClause += " AND cv_resource_requests_workflow.status = " + strDStatus;
                                if (lblResults.Text != "")
                                    lblResults.Text += " AND ";
                                lblResults.Text += "Status = &quot;" + oStatusLevel.Name(Int32.Parse(strDStatus)) + "&quot;";
                            }
                            string strDStart = ds.Tables[0].Rows[0]["dstart"].ToString();
                            if (strDStart != "")
                            {
                                strDStart = DateTime.Parse(strDStart).ToShortDateString();
                                strClause += " AND cv_requests.created > " + strDStart;
                                if (lblResults.Text != "")
                                    lblResults.Text += " AND ";
                                lblResults.Text += "Request Date > &quot;" + strDStart + "&quot;";
                            }
                            string strDEnd = ds.Tables[0].Rows[0]["dend"].ToString();
                            if (strDEnd != "")
                            {
                                strDEnd = DateTime.Parse(strDEnd).ToShortDateString();
                                strClause += " AND cv_requests.created < " + strDEnd;
                                if (lblResults.Text != "")
                                    lblResults.Text += " AND ";
                                lblResults.Text += "Request Date < &quot;" + strDEnd + "&quot;";
                            }
                            break;
                        case "5":
                            strApplication = "";
                            string strTechnician = ds.Tables[0].Rows[0]["technician"].ToString();
                            if (strTechnician != "0")
                            {
                                strClause += " AND cv_resource_requests_workflow.userid = " + strTechnician;
                                lblResults.Text += "Technician = &quot;" + oUser.GetFullName(Int32.Parse(strTechnician)) + "&quot;";
                            }
                            string strTStatus = ds.Tables[0].Rows[0]["tstatus"].ToString();
                            if (strTStatus != "0")
                            {
                                strClause += " AND cv_resource_requests_workflow.status = " + strTStatus;
                                if (lblResults.Text != "")
                                    lblResults.Text += " AND ";
                                lblResults.Text += "Status = &quot;" + oStatusLevel.Name(Int32.Parse(strTStatus)) + "&quot;";
                            }
                            string strTStart = ds.Tables[0].Rows[0]["tstart"].ToString();
                            if (strTStart != "")
                            {
                                strTStart = DateTime.Parse(strTStart).ToShortDateString();
                                strClause += " AND cv_requests.created > " + strTStart;
                                if (lblResults.Text != "")
                                    lblResults.Text += " AND ";
                                lblResults.Text += "Request Date > &quot;" + strTStart + "&quot;";
                            }
                            string strTEnd = ds.Tables[0].Rows[0]["tend"].ToString();
                            if (strTEnd != "")
                            {
                                strTEnd = DateTime.Parse(strTEnd).ToShortDateString();
                                strClause += " AND cv_requests.created < " + strTEnd;
                                if (lblResults.Text != "")
                                    lblResults.Text += " AND ";
                                lblResults.Text += "Request Date < &quot;" + strTEnd + "&quot;";
                            }
                            break;
                        case "4":
                            string strLead = ds.Tables[0].Rows[0]["lead"].ToString();
                            if (strLead != "0")
                            {
                                int intLead = Int32.Parse(strLead);
                                strClause += " AND cv_users.manager = " + intLead.ToString();
                                lblResults.Text += "Team Lead = &quot;" + oUser.GetFullName(intLead) + "&quot;";
                            }
                            string strLStatus = ds.Tables[0].Rows[0]["lstatus"].ToString();
                            if (strLStatus != "0")
                            {
                                strClause += " AND cv_resource_requests_workflow.status = " + strLStatus;
                                if (lblResults.Text != "")
                                    lblResults.Text += " AND ";
                                lblResults.Text += "Status = &quot;" + oStatusLevel.Name(Int32.Parse(strLStatus)) + "&quot;";
                            }
                            string strLStart = ds.Tables[0].Rows[0]["lstart"].ToString();
                            if (strLStart != "")
                            {
                                strLStart = DateTime.Parse(strLStart).ToShortDateString();
                                strClause += " AND cv_requests.created > " + strLStart;
                                if (lblResults.Text != "")
                                    lblResults.Text += " AND ";
                                lblResults.Text += "Request Date < &quot;" + strLStart + "&quot;";
                            }
                            string strLEnd = ds.Tables[0].Rows[0]["lend"].ToString();
                            if (strLEnd != "")
                            {
                                strLEnd = DateTime.Parse(strLEnd).ToShortDateString();
                                strClause += " AND cv_requests.created < " + strLEnd;
                                if (lblResults.Text != "")
                                    lblResults.Text += " AND ";
                                lblResults.Text += "Request Date > &quot;" + strLEnd + "&quot;";
                            }
                            break;
                        case "3":
                            string strItem = ds.Tables[0].Rows[0]["itemid"].ToString();
                            if (strItem != "0")
                            {
                                strClause += " AND cv_resource_requests.itemid = " + strItem;
                                lblResults.Text += "Activity Type = &quot;" + oRequestItem.GetItemName(Int32.Parse(strItem)) + "&quot;";
                            }
                            string strGStatus = ds.Tables[0].Rows[0]["gstatus"].ToString();
                            if (strGStatus != "0")
                            {
                                strClause += " AND cv_resource_requests_workflow.status = " + strGStatus;
                                if (lblResults.Text != "")
                                    lblResults.Text += " AND ";
                                lblResults.Text += "Status = &quot;" + oStatusLevel.Name(Int32.Parse(strGStatus)) + "&quot;";
                            }
                            string strGStart = ds.Tables[0].Rows[0]["gstart"].ToString();
                            if (strGStart != "")
                            {
                                strGStart = DateTime.Parse(strGStart).ToShortDateString();
                                strClause += " AND cv_requests.created > " + strGStart;
                                if (lblResults.Text != "")
                                    lblResults.Text += " AND ";
                                lblResults.Text += "Request Date < &quot;" + strGStart + "&quot;";
                            }
                            string strGEnd = ds.Tables[0].Rows[0]["gend"].ToString();
                            if (strGEnd != "")
                            {
                                strGEnd = DateTime.Parse(strGEnd).ToShortDateString();
                                strClause += " AND cv_requests.created < " + strGEnd;
                                if (lblResults.Text != "")
                                    lblResults.Text += " AND ";
                                lblResults.Text += "Request Date > &quot;" + strGEnd + "&quot;";
                            }
                            break;
                    }
                    strClause = strClause + strApplication;
                    //string strAlways = " AND cv_service_requests.id IS NOT NULL";
                    string strAlways = "";
                    if (strOr1 != "" && strOr2 != "")
                        strClause = strClause + " AND " + strOr1 + strAlways + " OR " + strClause + " AND " + strOr2 + strAlways;
                    else
                        strClause = strClause + strAlways;
                    //                Response.Write(strClause);
                    hdnSQL.Value = strSQL + strClause;
                    ds = SqlHelper.ExecuteDataset(dsn, CommandType.Text, strSQL + strClause);
                    lblPage.Text = "1";
                    lblSort.Text = "";
                    if (Request.QueryString["page"] != null && Request.QueryString["page"] != "")
                        lblPage.Text = Request.QueryString["page"];
                    if (Request.QueryString["sort"] != null && Request.QueryString["sort"] != "")
                        lblSort.Text = Request.QueryString["sort"];
                    lblTopPaging.Text = "";
                    LoadPaging(Int32.Parse(lblPage.Text), lblSort.Text);
                }
                else
                    panDenied.Visible = true;
            }
            else
                panDenied.Visible = true;
        }
        protected void btnSearch_Click(Object Sender, EventArgs e)
        {
            string strType = Request.Form[hdnType.UniqueID];
            int intSearch = 0;
            string strStart = "";
            string strEnd = "";
            switch (strType)
            {
                case "1":

                    DateTime start = new DateTime();
                    DateTime end = new DateTime();

                    if (DateTime.TryParse(txtOStart.Text, out start))
                    {
                        strStart = start.ToShortDateString();
                    }
                    else 
                    {
                        strStart = string.Empty;
                    }

                    if (DateTime.TryParse(txtOEnd.Text, out end))
                    {
                        strEnd = end.ToShortDateString();
                    }
                    else
                    {
                        strEnd = string.Empty;
                    }

                    int intSegment = 0;
                    if (Request.Form[hdnSegment.UniqueID] != "")
                        intSegment = Int32.Parse(Request.Form[hdnSegment.UniqueID]);
                    intSearch = oSearch.AddTaskOverall(intProfile, intApplication, strType, txtName.Text, txtNumber.Text, Int32.Parse(ddlOStatus.SelectedItem.Value), Int32.Parse(Request.Form[hdnSubmittedBy.UniqueID]), strStart, strEnd);
                    break;
                case "2":
                    intSearch = oSearch.AddTask(intProfile, intApplication, strType, Int32.Parse(ddlDepartment.SelectedItem.Value), Int32.Parse(ddlDStatus.SelectedItem.Value), "", "");
                    break;
                case "3":
                    intSearch = oSearch.AddTaskGroup(intProfile, intApplication, strType, Int32.Parse(ddlGroup.SelectedItem.Value), Int32.Parse(ddlGStatus.SelectedItem.Value), strStart, strEnd);
                    break;
                case "4":
                    intSearch = oSearch.AddTaskLead(intProfile, intApplication, strType, Int32.Parse(ddlLead.SelectedItem.Value), Int32.Parse(ddlLStatus.SelectedItem.Value), strStart, strEnd);
                    break;
                case "5":
                    intSearch = oSearch.AddTaskTechnician(intProfile, intApplication, strType, Int32.Parse(ddlTechnician.SelectedItem.Value), Int32.Parse(ddlTStatus.SelectedItem.Value), strStart, strEnd);
                    break;
            }
            Reload("?sid=" + intSearch.ToString());
        }
        private void LoopRepeater(string _sort, int _start)
        {
            if (_start > ds.Tables[0].Rows.Count)
                _start = 0;
            intRecordStart = _start + 1;
            DataView dv = ds.Tables[0].DefaultView;
            if (Request.QueryString["sort"] != null)
                dv.Sort = Request.QueryString["sort"];
            int intCount = _start + intRecords;
            if (dv.Count < intCount)
                intCount = dv.Count;
            int ii = 0;
            lblRecords.Text = "Requests " + intRecordStart.ToString() + " - " + intCount.ToString() + " of " + dv.Count.ToString();
            for (ii = 0; ii < _start; ii++)
                dv[0].Delete();
            int intTotalCount = (dv.Count - intRecords);
            for (ii = 0; ii < intTotalCount; ii++)
                dv[intRecords].Delete();
            rptView.DataSource = dv;
            rptView.DataBind();
            foreach (RepeaterItem ri in rptView.Items)
            {
                Label _progress = (Label)ri.FindControl("lblProgress");
                Label _status = (Label)ri.FindControl("lblStatus");
                _status.Text = oStatusLevel.HTML(Int32.Parse(_status.Text));
                Label _requestor = (Label)ri.FindControl("lblRequestor");
                Label _updated = (Label)ri.FindControl("lblUpdated");
                Label _number = (Label)ri.FindControl("lblNumber");
                if (_number.Text == "")
                    _number.Text = "<i>TBD...</i>";
                double dblUsed = 0.00;
                double dblAllocated = 0.00;
                if (_progress.Text.Contains("?rid=") == true)
                {
                    int intRequest = 0;
                    if (Request.QueryString["sid"] != null && Request.QueryString["sid"] != "")
                    {
                        string strProgress = _progress.Text.Substring(0, _progress.Text.IndexOf("&"));
                        intRequest = Int32.Parse(strProgress.Substring(5));
                    }
                    else
                        intRequest = Int32.Parse(_progress.Text.Substring(5));
                    _updated.Text = oRequest.GetLastUpdated(intRequest);
                    DataSet dsAll = oResourceRequest.GetWorkflowRequestAll(intRequest);
                    foreach (DataRow drAll in dsAll.Tables[0].Rows)
                    {
                        dblAllocated += double.Parse(drAll["allocated"].ToString());
                        dblUsed += oResourceRequest.GetWorkflowUsed(Int32.Parse(drAll["id"].ToString()));
                    }
                    _requestor.Text = oUser.GetFullName(Int32.Parse(oRequest.Get(intRequest, "userid")));
                }
                else if (_progress.Text.Contains("?rid=") == true)
                {
                    int intRequest = Int32.Parse(_progress.Text.Substring(5));
                    DataSet dsAll = oResourceRequest.GetWorkflowRequestAll(intRequest);
                    foreach (DataRow drAll in dsAll.Tables[0].Rows)
                    {
                        dblAllocated += double.Parse(drAll["allocated"].ToString());
                        dblUsed += oResourceRequest.GetWorkflowUsed(Int32.Parse(drAll["id"].ToString()));
                    }
                    _requestor.Text = oUser.GetFullName(Int32.Parse(oRequest.Get(intRequest, "userid")));
                }
                double dblProgress = 0.00;
                if (dblAllocated > 0.00)
                    dblProgress = (dblUsed / dblAllocated) * 100.00;
                _progress.Text = oServiceRequest.GetStatusBar(dblProgress, "100", "6", true);
            }
            lblNone.Visible = (rptView.Items.Count == 0);
            _start++;
        }
        protected void btnOrder_Click(Object Sender, EventArgs e)
        {
            LinkButton oOrder = (LinkButton)Sender;
            string strPage = "";
            string strOrder = "";
            if (Request.QueryString["sort"] != null)
            {
                if (Request.QueryString["sort"] == oOrder.CommandArgument)
                    strOrder = oOrder.CommandArgument + " DESC";
            }
            if (strOrder == "")
                strOrder = oOrder.CommandArgument;
            if (Request.QueryString["page"] != null)
                strPage = Request.QueryString["page"];
            Reload("?sid=" + Request.QueryString["sid"] + "&sort=" + strOrder + "&page=" + strPage);
        }
        protected void btnPage_Click(Object Sender, ImageClickEventArgs e)
        {
            string strOrder = "";
            if (Request.QueryString["sort"] != null)
                strOrder = Request.QueryString["sort"];
            Reload("?sid=" + Request.QueryString["sid"] + "&sort=" + strOrder + "&page=" + txtPage.Text);
        }
        private void LoadPaging(int intStart, string _sort)
        {
            int intCount = ds.Tables[0].Rows.Count;
            double dblEnd = Math.Ceiling(Double.Parse(intCount.ToString()) / Double.Parse(intRecords.ToString()));
            int intEnd = Int32.Parse(dblEnd.ToString());
            int ii = 0;
            txtPage.Text = intStart.ToString();
            lblPages.Text = intEnd.ToString();
            if (intEnd < 7)
            {
                for (ii = 1; ii < intEnd; ii++)
                {
                    LoadLink(lblTopPaging, ii, ", ", intStart);
                    LoadLink(lblBottomPaging, ii, ", ", intStart);
                }
                LoadLink(lblTopPaging, intEnd, "", intStart);
                LoadLink(lblBottomPaging, intEnd, "", intStart);
            }
            else
            {
                if (intStart < 5)
                {
                    for (ii = 1; ii < 6 && ii < intEnd; ii++)
                    {
                        LoadLink(lblTopPaging, ii, ", ", intStart);
                        LoadLink(lblBottomPaging, ii, ", ", intStart);
                    }
                    if (ii < intEnd)
                    {
                        LoadLink(lblTopPaging, ii, " .... ", intStart);
                        LoadLink(lblBottomPaging, ii, " .... ", intStart);
                    }
                    LoadLink(lblTopPaging, intEnd, "", intStart);
                    LoadLink(lblBottomPaging, intEnd, "", intStart);
                }
                else if (intStart > (intEnd - 4))
                {
                    LoadLink(lblTopPaging, 1, " .... ", intStart);
                    LoadLink(lblBottomPaging, 1, " .... ", intStart);
                    for (ii = (intEnd - 5); ii < intEnd && ii > 0; ii++)
                    {
                        LoadLink(lblTopPaging, ii, ", ", intStart);
                        LoadLink(lblBottomPaging, ii, ", ", intStart);
                    }
                    LoadLink(lblTopPaging, intEnd, "", intStart);
                    LoadLink(lblBottomPaging, intEnd, "", intStart);
                }
                else
                {
                    LoadLink(lblTopPaging, 1, " .... ", intStart);
                    LoadLink(lblBottomPaging, 1, " .... ", intStart);
                    for (ii = (intStart - 2); ii < (intStart + 3) && ii < intEnd && ii > 0; ii++)
                    {
                        if (ii == (intStart + 2))
                        {
                            LoadLink(lblTopPaging, ii, " .... ", intStart);
                            LoadLink(lblBottomPaging, ii, " .... ", intStart);
                        }
                        else
                        {
                            LoadLink(lblTopPaging, ii, ", ", intStart);
                            LoadLink(lblBottomPaging, ii, ", ", intStart);
                        }
                    }
                    LoadLink(lblTopPaging, intEnd, "", intStart);
                    LoadLink(lblBottomPaging, intEnd, "", intStart);
                }
            }
            LoopRepeater(_sort, ((intStart - 1) * intRecords));
        }
        private void LoadLink(Label _label, int _number, string _spacer, int _start)
        {
            StringBuilder sb = new StringBuilder(_label.Text);

            if (_number == _start)
            {
                sb.Append("<b><font style=\"color:#CC0000\">");
                sb.Append(_number.ToString());
                sb.Append("</font></b>");
            }
            else
            {
                string strSort = "";
                if (Request.QueryString["sort"] != null)
                {
                    strSort = Request.QueryString["sort"];
                }

                sb.Append("<a href=\"");
                sb.Append(oPage.GetFullLink(intPage));
                sb.Append("?sid=");
                sb.Append(Request.QueryString["sid"]);
                sb.Append("&sort=");
                sb.Append(strSort);
                sb.Append("&page=");
                sb.Append(_number.ToString());
                sb.Append("\" title=\"Go to Page ");
                sb.Append(_number.ToString());
                sb.Append("\">");
                sb.Append(_number.ToString());
                sb.Append("</a>");
            }

            if (_spacer != "")
            {
                sb.Append(_spacer);
            }

            _label.Text = sb.ToString();
        }
        protected void Reload(string _redirect)
        {
            Response.Redirect(oPage.GetFullLink(intPage) + _redirect);
        }
        protected void btnMessage_Click(Object Sender, EventArgs e)
        {
            int intUser = Int32.Parse(ddlResource.SelectedItem.Value);
            if (ddlCommunication.SelectedItem.Value.ToUpper() == "EMAIL")
            {
                string strEmail = oUser.GetName(intUser);
                oFunction.SendEmail("", strEmail, oUser.GetName(intProfile), strEMailIdsBCC, "ClearView Communication from " + oUser.GetFullName(intProfile), oRequest.GetBody2(Int32.Parse(Request.QueryString["rid"]), intEnvironment, false) + "<table width=\"100%\" border=\"0\" cellpadding=\"2\" cellspacing=\"1\"><tr><td><span style=\"width:100%;border-bottom:1 dotted #999999;\"/></td></tr></table>" + txtMessage.Text, true, false);
            }
            else
            {
                string strPager = oUser.Get(intUser, "pager") + "@archwireless.net";
                oFunction.SendEmail("", strPager, oUser.GetName(intProfile), strEMailIdsBCC, "ClearView Communication from " + oUser.GetFullName(intProfile), txtMessage.Text, false, true);
            }
            Response.Redirect(oPage.GetFullLink(intPage) + "?rid=" + Request.QueryString["rid"] + "&sid=" + Request.QueryString["sid"]);
        }
        protected double GetFloat(string _float)
        {
            if (_float == "")
                return 0.00;
            else
                return double.Parse(_float);
        }
    }
}