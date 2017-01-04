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
    public partial class sys_new_report : System.Web.UI.UserControl
    {
        protected Reports oReport;
        protected Functions oFunction;
        protected Requests oRequest;
        protected Services oService;
        protected ServiceRequests oServiceRequest;
        protected Pages oPage;
        protected Applications oApplication;
        protected int intEnvironment = Int32.Parse(ConfigurationManager.AppSettings["Environment"]);
        private string dsn = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DSN"]].ConnectionString;
        private string dsnAsset = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["AssetDSN"]].ConnectionString;
        private string dsnIP = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["IpDSN"]].ConnectionString;
        private string dsnServiceEditor = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["ServiceEditorDSN"]].ConnectionString;

        // Vijay Code - Start 
        protected int intService = Int32.Parse(ConfigurationManager.AppSettings["REPORT_SERVICEID"]);
        protected int intResourceRequestApprove = Int32.Parse(ConfigurationManager.AppSettings["ResourceRequestApprove"]);
        // Vijay Code - End 

        protected int intAssignPage = Int32.Parse(ConfigurationManager.AppSettings["AssignWorkload"]);
        protected int intViewPage = Int32.Parse(ConfigurationManager.AppSettings["ViewWorkload"]);
        protected int intProfile;
        protected int intPage = 0;
        protected bool boolPreview = true;

        // Vijay Code - Start 
        protected int intRequest;
        protected int intItemID;
        protected int intRepID;
        // Vijay Code - End 

        private Variables oVariables;

        protected void Page_Load(object sender, EventArgs e)
        {
            intProfile = Int32.Parse(Request.Cookies["profileid"].Value);
            oReport = new Reports(intProfile, dsn);
            oFunction = new Functions(intProfile, dsn, intEnvironment);
            oPage = new Pages(intProfile, dsn);
            oApplication = new Applications(intProfile, dsn);
            oRequest = new Requests(intProfile, dsn);
            oService = new Services(intProfile, dsn);
            oServiceRequest = new ServiceRequests(intProfile, dsn);

            // Vijay Code - Start
            if (!IsPostBack)
            {

                drpDataSource.DataSource = oReport.GetDataSources(1);
                drpDataSource.DataTextField = "name";
                drpDataSource.DataValueField = "id";
                drpDataSource.DataBind();
                drpDataSource.Items.Insert(0, "-- SELECT --");


                lstAppsAvailable.DataValueField = "applicationid";
                lstAppsAvailable.DataTextField = "name";
                lstAppsAvailable.DataSource = oApplication.Gets(1);
                lstAppsAvailable.DataBind();

                drpChartType.DataSource = oReport.GetCharts(1);
                drpChartType.DataTextField = "name";
                drpChartType.DataValueField = "id";
                drpChartType.DataBind();
                drpChartType.Items.Insert(0, new ListItem("-- SELECT --", "0"));
            }
            // Vijay Code - End

            if (Request.QueryString["pageid"] != null && Request.QueryString["pageid"] != "")
                intPage = Int32.Parse(Request.QueryString["pageid"]);

            // Vijay Code - Start
            if (Request.QueryString["repid"] != null && Request.QueryString["repid"] != "")
                intRepID = Int32.Parse(Request.QueryString["repid"]);
            // Vijay Code - End

            lblTitle.Text = oPage.Get(intPage, "title");
            bool boolForm = true;

            if (Request.QueryString["s"] != null && Request.QueryString["s"] != "")
            {
                if (Request.QueryString["no_prev"] != null)
                    boolPreview = false;
                chkPreview.Checked = boolPreview;
                Find(Request.QueryString["s"]);
                boolForm = false;
                panNoForm.Visible = true;
                panSearch.Visible = true;
            }
            else if (Request.QueryString["order"] != null && Request.QueryString["order"] != "")
            {
                if (Request.QueryString["order"] == "sent")
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "send_order", "<script type=\"text/javascript\">alert('Your order has been submitted');<" + "/script>");
                if (Request.QueryString["order"] == "form")
                {
                    boolForm = false;
                    panNoForm.Visible = true;
                    panOrder.Visible = true;
                }
                if (Request.QueryString["order"] == "other")
                {
                    boolForm = false;
                    panNoForm.Visible = true;
                    panOther.Visible = true;
                }
            }
            if (boolForm == true)
            {
                panReport.Visible = true;
                if (Request.QueryString["add"] != null && Request.QueryString["add"] != "")
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "add_fav", "<script type=\"text/javascript\">alert('The report has been added to your favorites');<" + "/script>");
                if (Request.QueryString["delete"] != null && Request.QueryString["delete"] != "")
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "remove_fav", "<script type=\"text/javascript\">alert('The report has been removed from your favorites');<" + "/script>");
                TreeNode oNode = new TreeNode();
                oNode = new TreeNode();
                oNode.Text = "Favorites";
                oNode.ToolTip = "Favorites";
                oNode.ImageUrl = "/images/folder.gif";
                oNode.SelectAction = TreeNodeSelectAction.Expand;
                oTreeview.Nodes.Add(oNode);
                LoadFavorites(oNode);
                LoadReports(0, oTreeview, null);
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "NavResize", "<script type=\"text/javascript\">window.onload = Loader;<" + "/script>");
            }
            lnkAdd.Attributes.Add("onclick", "return CheckReport('" + hdnReport.ClientID + "','');");
            lnkRemove.Attributes.Add("onclick", "return CheckReport('" + hdnReport.ClientID + "','Are you sure you want to remove this report from your favorites?');");
            txtSearch.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnSearch.ClientID + "').click();return false;}} else {return true}; ");
            btnSearch.Attributes.Add("onclick", "return ValidateText('" + txtSearch.ClientID + "','Please enter some text to search');");
            txtSearch2.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnSearch2.ClientID + "').click();return false;}} else {return true}; ");
            btnSearch2.Attributes.Add("onclick", "return ValidateText('" + txtSearch2.ClientID + "','Please enter some text to search');");
            imgMaximize.Attributes.Add("onclick", "return NewMaximize('');");

            // Vijay Code - Start
            btnOrder.Attributes.Add("onclick", "return ValidateList('" + lstAppsCurrent.ClientID + "','Please select atleast one application');");
            btnAppsAdd.Attributes.Add("onclick", "return MoveList('" + lstAppsAvailable.ClientID + "','" + lstAppsCurrent.ClientID + "','" + hdnApps.ClientID + "','" + lstAppsCurrent.ClientID + "');");
            lstAppsAvailable.Attributes.Add("ondblclick", "return MoveList('" + lstAppsAvailable.ClientID + "','" + lstAppsCurrent.ClientID + "','" + hdnApps.ClientID + "','" + lstAppsCurrent.ClientID + "');");
            btnAppsRemove.Attributes.Add("onclick", "return MoveList('" + lstAppsCurrent.ClientID + "','" + lstAppsAvailable.ClientID + "','" + hdnApps.ClientID + "','" + lstAppsCurrent.ClientID + "');");
            lstAppsCurrent.Attributes.Add("ondblclick", "return MoveList('" + lstAppsCurrent.ClientID + "','" + lstAppsAvailable.ClientID + "','" + hdnApps.ClientID + "','" + lstAppsCurrent.ClientID + "');");
            drpChartType.Attributes.Add("onchange", "AjaxGetChartURL('" + drpChartType.ClientID + "');");
            btnNext.Attributes.Add("onclick", "return ValidateText('" + txtTitle.ClientID + "','Please enter a report title') " +
                     " && ValidateDropDown('" + drpDataSource.ClientID + "','Please make a selection for data source') " +
                     " && ValidateDropDown('" + drpChartType.ClientID + "','Please make a selection for chart type') " +
                     ";");
            // Vijay Code - End
        }
        private void LoadReports(int _parent, TreeView oTree, TreeNode oParent)
        {
            DataSet ds = oReport.Gets(_parent, 1);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                int intReport = Int32.Parse(dr["reportid"].ToString());
                DataSet dsChildren = oReport.Gets(intReport, 1);
                if (dsChildren.Tables[0].Rows.Count > 0)
                {
                    // Load Parent Reports
                    if (oReport.GetRole(intProfile, intReport).Tables[0].Rows.Count > 0)
                    {
                        TreeNode oNode = new TreeNode();
                        oNode.Text = dr["title"].ToString();
                        oNode.ToolTip = dr["title"].ToString();
                        string strImage = dr["image"].ToString().Trim();
                        if (dr["image"].ToString().Trim() != "")
                            oNode.ImageUrl = dr["image"].ToString();
                        else
                            oNode.ImageUrl = "/images/folder.gif";
                        if (dr["path"].ToString().Trim() == "" && dr["physical"].ToString().Trim() == "")
                        {
                            oNode.SelectAction = TreeNodeSelectAction.Expand;
                            if (strImage == "")
                                strImage = "/images/folder.gif";
                        }
                        else
                        {
                            if (dr["path"].ToString().Trim() != "")
                                oNode.NavigateUrl = "javascript:SelectReport('/frame/loading.htm?referrer=/frame/report.aspx?r=" + oFunction.encryptQueryString(dr["reportid"].ToString()) + "','frmReport','" + dr["reportid"].ToString() + "','" + hdnReport.ClientID + "');";
                            else
                                oNode.NavigateUrl = "javascript:SelectReport('" + dr["physical"].ToString() + "','frmReport','" + dr["reportid"].ToString() + "','" + hdnReport.ClientID + "');";
                            if (strImage == "")
                                strImage = "/images/report.gif";
                        }
                        oNode.ImageUrl = strImage;
                        if (oTree == null)
                            oParent.ChildNodes.Add(oNode);
                        else
                            oTree.Nodes.Add(oNode);
                        LoadReports(intReport, null, oNode);
                    }
                }
            }
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                int intReport = Int32.Parse(dr["reportid"].ToString());
                DataSet dsChildren = oReport.Gets(intReport, 1);
                if (dsChildren.Tables[0].Rows.Count == 0)
                {
                    // Load Parent Reports
                    if (oReport.GetRole(intProfile, intReport).Tables[0].Rows.Count > 0)
                    {
                        TreeNode oNode = new TreeNode();
                        oNode.Text = dr["title"].ToString();
                        oNode.ToolTip = dr["title"].ToString();
                        string strImage = dr["image"].ToString().Trim();
                        if (dr["path"].ToString().Trim() == "" && dr["physical"].ToString().Trim() == "")
                        {
                            oNode.SelectAction = TreeNodeSelectAction.Expand;
                            if (strImage == "")
                                strImage = "/images/folder.gif";
                        }
                        else
                        {
                            if (dr["path"].ToString().Trim() != "")
                                oNode.NavigateUrl = "javascript:SelectReport('/frame/loading.htm?referrer=/frame/report.aspx?r=" + oFunction.encryptQueryString(dr["reportid"].ToString()) + "','frmReport','" + dr["reportid"].ToString() + "','" + hdnReport.ClientID + "');";
                            else
                                oNode.NavigateUrl = "javascript:SelectReport('" + dr["physical"].ToString() + "','frmReport','" + dr["reportid"].ToString() + "','" + hdnReport.ClientID + "');";
                            if (strImage == "")
                                strImage = "/images/report.gif";
                        }
                        oNode.ImageUrl = strImage;
                        if (oTree == null)
                            oParent.ChildNodes.Add(oNode);
                        else
                            oTree.Nodes.Add(oNode);
                        LoadReports(intReport, null, oNode);
                    }
                }
            }
            oTreeview.CollapseAll();
            oTreeview.Attributes.Add("oncontextmenu", "return false;");
        }
        private void LoadFavorites(TreeNode oParent)
        {
            DataSet ds = oReport.GetFavorites(intProfile);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                TreeNode oNode = new TreeNode();
                oNode.Text = dr["title"].ToString();
                oNode.ToolTip = dr["title"].ToString();
                if (dr["image"].ToString().Trim() != "")
                    oNode.ImageUrl = dr["image"].ToString();
                else
                    oNode.ImageUrl = "/images/report.gif";
                if (dr["path"].ToString().Trim() == "" && dr["physical"].ToString().Trim() == "")
                    oNode.SelectAction = TreeNodeSelectAction.Expand;
                else
                {
                    if (dr["path"].ToString().Trim() != "")
                        oNode.NavigateUrl = "javascript:SelectReport('/frame/loading.htm?referrer=/frame/report.aspx?r=" + oFunction.encryptQueryString(dr["reportid"].ToString()) + "','frmReport','" + dr["reportid"].ToString() + "','" + hdnReport.ClientID + "');";
                    else
                        oNode.NavigateUrl = "javascript:SelectReport('" + dr["physical"].ToString() + "','frmReport','" + dr["reportid"].ToString() + "','" + hdnReport.ClientID + "');";
                }
                oParent.ChildNodes.Add(oNode);
            }
        }
        protected void lnkOrder_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(oPage.GetFullLink(intPage) + "?order=form");
        }
        protected void lnkAdd_Click(Object Sender, EventArgs e)
        {
            string strReport = Request.Form[hdnReport.UniqueID];
            if (strReport != "")
            {
                oReport.AddFavorite(intProfile, Int32.Parse(strReport));
                Response.Redirect(oPage.GetFullLink(intPage) + "?add=sent");
            }
            Response.Redirect(oPage.GetFullLink(intPage));
        }
        protected void lnkRemove_Click(Object Sender, EventArgs e)
        {
            string strReport = Request.Form[hdnReport.UniqueID];
            if (strReport != "")
            {
                oReport.DeleteFavorite(intProfile, Int32.Parse(strReport));
                Response.Redirect(oPage.GetFullLink(intPage) + "?delete=sent");
            }
            Response.Redirect(oPage.GetFullLink(intPage));
        }
        protected void btnCancel_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(oPage.GetFullLink(intPage));
        }
        protected void btnOrder_Click(Object Sender, EventArgs e)
        {
            // Vijay Code - Start
            string strApps = Request.Form[hdnApps.UniqueID];
            intRequest = oRequest.Add(0, intProfile);
            intItemID = oService.GetItemId(intService);
            int intResource = oServiceRequest.AddRequest(intRequest, intItemID, intService, 0, 0.00, 2, 0, dsnServiceEditor);
            oServiceRequest.Add(intRequest, 1, 1);
            oReport.UpdateOrderReport(intRepID, intRequest, intItemID, 0);
            oServiceRequest.Update(intRequest, oReport.GetOrderReport(intRequest, intItemID, 0, "title"));
            while (strApps != "")
            {
                string strAppId = strApps.Substring(0, strApps.IndexOf("&"));
                strApps = strApps.Substring(strApps.IndexOf("&") + 1);
                int intAppId = Int32.Parse(strAppId.Substring(0, strAppId.IndexOf("_")));
                oReport.AddOrderReportApplications(intRepID, oApplication.GetName(intAppId));
            }
            if (oServiceRequest.NotifyApproval(intResource, intResourceRequestApprove, intEnvironment, "", dsnServiceEditor) == false)
                oServiceRequest.NotifyTeamLead(intItemID, intResource, intAssignPage, intViewPage, intEnvironment,  "", dsnServiceEditor, dsnAsset, dsnIP, 0);

            Response.Redirect(oPage.GetFullLink(intPage) + "?order=sent");
            // Vijay Code - End
        }
        protected void btnSearch_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(oPage.GetFullLink(intPage) + "?s=" + txtSearch.Text);
        }
        protected void btnSearch2_Click(Object Sender, EventArgs e)
        {
            Response.Redirect(oPage.GetFullLink(intPage) + "?s=" + txtSearch2.Text);
        }
        protected void chkPreview_Change(Object Sender, EventArgs e)
        {
            Response.Redirect(oPage.GetFullLink(intPage) + "?s=" + Request.QueryString["s"] + (chkPreview.Checked ? "" : "&no_prev=true"));
        }
        private void Find(string _search)
        {
            txtSearch2.Text = _search;
            DataSet ds = oReport.Gets(intProfile, _search);
            if (boolPreview == true)
            {
                panViewPreview.Visible = true;
                rptViewPreview.DataSource = ds;
                rptViewPreview.DataBind();
                lblViewPreview.Visible = (ds.Tables[0].Rows.Count == 0);
            }
            else
            {
                panView.Visible = true;
                rptView.DataSource = ds;
                rptView.DataBind();
                lblView.Visible = (ds.Tables[0].Rows.Count == 0);
            }
        }
        protected void btnSubmit_Click(Object Sender, EventArgs e)
        {
            oVariables = new Variables(intEnvironment);
            // Vijay Code - Start
            string strPath = "";
            string strFile = "";
            string strAbsPath = "";
            if (UploadReport.PostedFile != null && UploadReport.FileName != "")
            {
                string strExtension = UploadReport.FileName;
                string strType = strExtension.Substring(0, 3);
                strExtension = strExtension.Substring(strExtension.LastIndexOf("."));
                strFile = DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + "_" + intProfile.ToString() + strExtension;
                strPath = oVariables.UploadsFolder() + strFile;
                UploadReport.PostedFile.SaveAs(strPath);
                strAbsPath = oVariables.UploadsFolder() + strFile;
            }
            int intReportID = oReport.AddOrderReport(txtTitle.Text, drpDataSource.SelectedItem.Text, drpChartType.SelectedItem.Text, strAbsPath, txtInstructions.Text, txtExclusion.Text);
            Response.Redirect(oPage.GetFullLink(intPage) + "?order=other&repid=" + intReportID);
            // Vijay Code - End
        }   
    }
}